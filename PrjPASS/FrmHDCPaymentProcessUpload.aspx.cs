using Microsoft.Practices.EnterpriseLibrary.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmHDCPaymentProcessUpload : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");
        string logFile = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/FrmHDCPaymentProcessUpload.txt";
        DataTable dtExcelData = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!File.Exists(logFile))
            {
                File.Create(logFile);
            }

            if (!IsPostBack)
            {
                if (Session["vUserLoginId"] != null)
                {
                    if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
                    {
                        bool chkAuth;
                        string pageName = this.Page.ToString().Substring(4, this.Page.ToString().Substring(4).Length - 5) + ".aspx";
                        chkAuth = wsGen.Fn_Check_Rights_For_Page(Session["vRoleCode"].ToString(), pageName);
                        if (chkAuth == false)
                        {
                            Alert.Show("Access Denied", "FrmMainMenu.aspx");
                            return;
                        }
                    }

                }
                else
                {
                    Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    return;
                }

                Directory.CreateDirectory(folderPath);

                // clnClaimIntimationDate.DateMax = DateTime.Now;
                //   txtClaimIntimationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //   txtClaimRegistrationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //   txtClaimIntimationDate.Attributes.Add("readonly", "readonly");

            }
        }


        private void ResetControls()
        {

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmHDCPaymentProcessUpload.aspx");
        }


        protected void btnExit_Click1(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuPaymentDetail.HasFile)
                {
                    if (!fuPaymentDetail.FileName.Contains(".xls") || !fuPaymentDetail.FileName.Contains(".xlsx"))
                    {
                        Alert.Show("Please select valid excel file");
                    }
                }

                string strDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                string filename = Path.GetFileName(fuPaymentDetail.FileName);
                string dirFullPath = Server.MapPath("~/Uploads/") + strDate.Replace("/", "").Replace("-", "").Replace(":", "") + "_" + filename;
                fuPaymentDetail.SaveAs(Server.MapPath("~/Uploads/") + strDate.Replace("/", "").Replace("-", "").Replace(":", "") + "_" + filename);

                string conString = string.Empty;
                string extension = Path.GetExtension(dirFullPath);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;

                }
                conString = string.Format(conString, dirFullPath);
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();


                    //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                    //dtExcelData.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
                    //    new DataColumn("Name", typeof(string)),
                    //    new DataColumn("Salary",typeof(decimal)) });

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();
                    Session["dtExcelData"] = dtExcelData;
                    string ValidatePaymentDataResult = string.Empty;

                    ValidatePaymentData(dtExcelData, out ValidatePaymentDataResult);

                    if (string.IsNullOrEmpty(ValidatePaymentDataResult))
                    {
                        dvGridView.Visible = true;
                        gvdtExcel.DataSource = dtExcelData;
                        gvdtExcel.DataBind();
                    }
                    else
                    {
                        dvGridView.Visible = false;
                        Alert.Show(ValidatePaymentDataResult.Replace("\r", " ").Replace("\n", " "));
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool ValidatePaymentData(DataTable dtExcelData, out string validatePaymentDataResult)
        {
            validatePaymentDataResult = string.Empty;
            bool Result = true;
            try
            {
                foreach (DataRow dr in dtExcelData.Rows)
                {
                    string ClaimNumber = dr["Claim Number"].ToString();
                    string strApprovalDate = DateTime.Parse(dr["Approval Date"].ToString()).ToString("dd/MM/yyyy");
                    string strFundTransfer_ChequeDate = DateTime.Parse(dr["Fund Transfer_Cheque Date"].ToString()).ToString("dd/MM/yyyy");
                    if (DateTime.ParseExact(strApprovalDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(strFundTransfer_ChequeDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        validatePaymentDataResult = validatePaymentDataResult + string.Format(" Fund Transfer or Cheque Date can not before the Approval Date for claim number {0}", ClaimNumber + System.Environment.NewLine);
                        Result = false;
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, " ValidatePaymentData ");
            }
            return Result;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["dtExcelData"];
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        using (SqlCommand cmd = new SqlCommand("PROC_INSERT_TBL_PAYMENT_DATA_UPLOAD_TABLE", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@vClaimNumber", dr["Claim Number"].ToString());
                            cmd.Parameters.AddWithValue("@vCertificateNumber", dr["Certificate Number"].ToString());
                            if (!string.IsNullOrEmpty(dr["Date of Admission"].ToString()))
                            {
                                string dateOFAdmission = DateTime.Parse(dr["Date of Admission"].ToString()).ToString("dd/MM/yyyy");
                                cmd.Parameters.AddWithValue("@vDateofAdmission", dateOFAdmission.ToString());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@vDateofAdmission", "");
                            }

                            if (!string.IsNullOrEmpty(dr["Date of Discharge"].ToString()))
                            {
                                string dateOfDischarge = DateTime.Parse(dr["Date of Discharge"].ToString()).ToString("dd/MM/yyyy");
                                cmd.Parameters.AddWithValue("@vDateofDischarge", dateOfDischarge.ToString());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@vDateofDischarge", "");
                            }

                            cmd.Parameters.AddWithValue("@vPayeeName", dr["Payee Name"].ToString());
                            cmd.Parameters.AddWithValue("@vPayeeAccountNumber", dr["Payee Account Number"].ToString());
                            cmd.Parameters.AddWithValue("@vPayeeIFSCCode", dr["Payee IFSC Code"].ToString());
                            cmd.Parameters.AddWithValue("@vClaimType", dr["Claim Type"].ToString());
                            cmd.Parameters.AddWithValue("@vSettlementType", dr["Settlement Type"].ToString());
                            cmd.Parameters.AddWithValue("@vPaymentType", dr["Payment Type"].ToString());
                            cmd.Parameters.AddWithValue("@vPaymentMode", dr["Payment Mode"].ToString());
                            cmd.Parameters.AddWithValue("@vDDLocation", dr["DD Location"].ToString());
                            cmd.Parameters.AddWithValue("@vPANNumber", dr["PAN Number"].ToString());
                            cmd.Parameters.AddWithValue("@vGSTNumber", dr["GST Number"].ToString());
                            cmd.Parameters.AddWithValue("@vInvoiceNumber", dr["Invoice Number"].ToString());
                            if (!string.IsNullOrEmpty(dr["Invoice Date"].ToString()))
                            {
                                string dateInvoiceDate = DateTime.Parse(dr["Invoice Date"].ToString()).ToString("dd/MM/yyyy");
                                cmd.Parameters.AddWithValue("@vInvoiceDate", dateInvoiceDate.ToString());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@vInvoiceDate", "");
                            }

                            cmd.Parameters.AddWithValue("@dFinalApprovedAmount", dr["Final Approved Amount"].ToString());
                            cmd.Parameters.AddWithValue("@nIGST", dr["IGST"].ToString());
                            cmd.Parameters.AddWithValue("@nCGST", dr["CGST"].ToString());
                            cmd.Parameters.AddWithValue("@nSGST", dr["SGST"].ToString());
                            cmd.Parameters.AddWithValue("@nUGST", dr["UGST"].ToString());
                            cmd.Parameters.AddWithValue("@nTDSAmount", dr["TDS Amount"].ToString());
                            cmd.Parameters.AddWithValue("@nFinalPayableAmount", dr["Final Payable Amount"].ToString());

                            if (!string.IsNullOrEmpty(dr["Approval Date"].ToString()))
                            {
                                string dateApprovalDate = DateTime.Parse(dr["Approval Date"].ToString()).ToString("dd/MM/yyyy");
                                cmd.Parameters.AddWithValue("@vApprovalDate", dateApprovalDate);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@vApprovalDate", "");
                            }
                            cmd.Parameters.AddWithValue("@vUTRNumber", dr["UTR Number"].ToString());

                            if (!string.IsNullOrEmpty(dr["Fund Transfer_Cheque Date"].ToString()))
                            {
                                string DateofFundTransfer = DateTime.Parse(dr["Fund Transfer_Cheque Date"].ToString()).ToString("dd/MM/yyyy");
                                cmd.Parameters.AddWithValue("@vFundTransfer_ChequeDate", DateofFundTransfer.ToString());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@vFundTransfer_ChequeDate", "");
                            }
                            cmd.Parameters.AddWithValue("@vRemarks", dr["Remarks"].ToString());
                            cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                            cmd.ExecuteNonQuery();
                            Session.Remove("dtExcelData");
                            dvGridView.Visible = false;

                            fnSendSMS(dr["Claim Number"].ToString(), dr["Certificate Number"].ToString(), dr["Final Payable Amount"].ToString(), dr["UTR Number"].ToString());
                        }
                    }
                }
                Alert.Show("Data Uploaded Successfully", "FrmHDCPaymentProcessUpload.aspx");
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "btnSubmit_Click ");
                Alert.Show("Some Error Occured, Kindly contact Administrator.");
            }
        }

        private void fnSendSMS(string claimNumber, string CertificateNumber, string Amount, string UTR)
        {
            try
            {
                string strPath = string.Empty;
                string smsBody = string.Empty;

                string MobileNumber = fnGetMobileNumber(CertificateNumber);

                smsBody = "Dear Customer,The claim amount has been transferred to your bank account for Rs. " + Amount + "-with UTR no. " + UTR
                         + " For further assistance request you to co-ordinate with your ESFB Branch.Thank You,Kotak General Insurance Co.Ltd.";
                using (SqlConnection cnCon = new SqlConnection(ConfigurationManager.ConnectionStrings["cnConnect"].ToString()))
                {
                    if (cnCon.State == ConnectionState.Closed)
                    {
                        cnCon.Open();
                    }
                    SqlCommand cmd = new SqlCommand("INSERT_DATA_CUSTOMER_SMS_LOG_HDC_SCHEDULER", cnCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mobile", MobileNumber);
                    cmd.Parameters.AddWithValue("@msg", smsBody);
                    cmd.ExecuteNonQuery();
                   
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnSendSMS ");
                Alert.Show("Some Error Occured while sending SMS, Kindly contact Administrator.");
            }
        }

        private string fnGetMobileNumber(string certificateNumber)
        {
            string MobileNo = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_MOBILENO_FROM_HDC_CERTIFICATENO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCertificateNumber", certificateNumber);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            MobileNo = reader[0].ToString();
                            return MobileNo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "btnSubmit_Click ");
                Alert.Show("Some Error Occured, Kindly contact Administrator.");
            }
            return MobileNo;
        }

        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            string strfilename = AppDomain.CurrentDomain.BaseDirectory + "/PaymentProcess_UploadTemplate.xlsx";
            DownloadFile(strfilename);
        }

        private bool DownloadFile(string strfilename)
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(strfilename);
            FileStream myFile = new FileStream(strfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            string _DownloadableProductFileName = strfilename;
            //Reads file as binary values
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            long startBytes = 0;

            string _EncodedData = HttpUtility.UrlEncode(_DownloadableProductFileName, Encoding.UTF8);

            //Clear the content of the response
            Response.Clear();
            Response.Buffer = false;
            Response.AddHeader("Accept-Ranges", "bytes");
            Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");


            //Set the ContentType
            Response.ContentType = "application/octet-stream";

            //Add the file name and attachment, 
            //which will force the open/cancel/save dialog to show, to the header
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName.Name);

            //Add the file size into the response header
            Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
            Response.AddHeader("Connection", "Keep-Alive");

            //Set the Content Encoding type
            Response.ContentEncoding = Encoding.UTF8;

            //Send data
            _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

            //Dividing the data in 1024 bytes package
            int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

            //Download in block of 1024 bytes
            int i;
            for (i = 0; i < maxCount && Response.IsClientConnected; i++)
            {
                Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                Response.Flush();
            }

            //compare packets transferred with total number of packets
            if (i < maxCount) return false;
            return true;

            //Close Binary reader and File stream
            _BinaryReader.Close();
            myFile.Close();
        }
    }
}