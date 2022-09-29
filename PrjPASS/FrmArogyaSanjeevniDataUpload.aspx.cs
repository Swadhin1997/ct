using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text;
using Obout.ComboBox;
using ProjectPASS;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Net;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Urlshortener.v1.Data;
using System.Web.Script.Serialization;
//using System.Security.Policy;

namespace PrjPASS
{
    public partial class FrmArogyaSanjeevniDataUpload : System.Web.UI.Page
    {
        private static UrlshortenerService service;
        private static readonly Regex ShortUrlRegex =
                   new Regex("^http[s]?://goo.gl/", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool IsShortUrl(string url)
        {
            return ShortUrlRegex.IsMatch(url);
        }


        public Cls_General_Functions wsGen = new Cls_General_Functions();
        protected void Page_Load(object sender, EventArgs e)
        {
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
            }
        }

        protected void Upload(object sender, EventArgs e)
        {

            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cYearMonth = "", vUploadId = "";
            string vCertificateNo = "";
            SqlConnection _con = new SqlConnection(dbCOMMON.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();
            string filePath = Server.MapPath("~/Reports/AROGYA");
            string strfilename = string.Empty;
            cYearMonth = DateTime.Now.Year.ToString();
            DataTable dt = new DataTable();
            string ErrMsg = string.Empty;
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                cYearMonth = cYearMonth + "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                cYearMonth = cYearMonth + DateTime.Now.Month.ToString();
            }

            //vUploadId = wsDocNo.fn_Gen_Doc_Master_No("HUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);

            _tran.Commit();

            try
            {

                //Upload and save the file
                // string fileName =  FileUpload1.PostedFile.FileName + "_" + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt");
                string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(FileUpload1.PostedFile.FileName);

                // string excelPath = Server.MapPath("~/Uploads/") + fileName + ".xlsx";
                clsAppLogs.LogEvent("UploadId - " + vUploadId + ", User - " + Session["vUserLoginId"] + ", File uploaded - " + Path.GetFileName(FileUpload1.PostedFile.FileName));
                FileUpload1.SaveAs(excelPath);

                string conString = string.Empty;

                string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);

                switch (extension)
                {

                    case ".xls": //Excel 97-03

                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;

                        break;

                    case ".xlsx": //Excel 07 or higher

                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;

                        break;

                }

                conString = string.Format(conString, excelPath);

                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    // int i = 0;
                    excel_con.Open();

                    string sheet1 = "UPLOAD_SHEET$"; //excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                    DataTable dtExcelData = new DataTable();



                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", excel_con))
                    {

                        oda.Fill(dtExcelData);

                    }

                    excel_con.Close();

                    if (dtExcelData.Rows.Count > 0)
                    {
                        //SqlBulkCopy objbulk = new SqlBulkCopy(con);
                        string googleShortURL = string.Empty;
                        //string vQuoteId = wsDocNo.fn_Gen_Cert_No("QAS", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
                        string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                        //string ConfrimURLPage = ConfigurationManager.AppSettings["vArogyaSanjeevniLink"].ToString() + "?vCustomerId ="+ EncryptText(dtExcelData.Rows[0]["Customer Code"].ToString());
                        //string ConfrimURLPage = ConfigurationManager.AppSettings["vArogyaSanjeevniLink"].ToString() + "?vQuoteId =" + EncryptText(vQuoteId);
                        // string ConfirmLink = ConfigurationManager.AppSettings["vConfirmLink"].ToString() + "?vCustomerId=" + EncryptText(vCustomerId);
                        //GoogleURLShortner(ConfrimURLPage, out googleShortURL);
                        //for (int i = 0; i < dtExcelData.Columns.Count; i++)
                        //{
                            //string vQuoteId = wsDocNo.fn_Gen_Cert_No("QAS", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
                            //string ConfrimURLPage = ConfigurationManager.AppSettings["vArogyaSanjeevniLink"].ToString() + "?vCustomerId =" + EncryptText(dtExcelData.Rows[0]["Customer Code"].ToString());

                            dt.Columns.Add(new DataColumn("ID" , typeof(String)));
                            dt.Columns.Add(new DataColumn("Customer Code", typeof(String)));
                            dt.Columns.Add(new DataColumn("Risk Type", typeof(String)));
                            dt.Columns.Add(new DataColumn("Title", typeof(String)));
                            dt.Columns.Add(new DataColumn("Proposer First Name", typeof(String)));
                            dt.Columns.Add(new DataColumn("Proposer Middle Name", typeof(String)));
                            dt.Columns.Add(new DataColumn("Proposer Last Name", typeof(String)));
                            dt.Columns.Add(new DataColumn("Gender", typeof(String)));
                            dt.Columns.Add(new DataColumn("DOB", typeof(String)));
                            dt.Columns.Add(new DataColumn("Contact number", typeof(String)));
                            dt.Columns.Add(new DataColumn("Email Address", typeof(String)));
                            dt.Columns.Add(new DataColumn("Marital Status", typeof(String)));
                            dt.Columns.Add(new DataColumn("Sum Insured", typeof(String)));
                            dt.Columns.Add(new DataColumn("Address Line 1", typeof(String)));
                            dt.Columns.Add(new DataColumn("Address Line 2", typeof(String)));
                            dt.Columns.Add(new DataColumn("Address Line 3", typeof(String)));
                            dt.Columns.Add(new DataColumn("Pin Code", typeof(String)));

                            dt.Columns.Add(new DataColumn("PAN Card No", typeof(String)));
                            dt.Columns.Add(new DataColumn("Occupation", typeof(String)));
                            dt.Columns.Add(new DataColumn("Intermediary Code", typeof(String)));
                           // dt.Columns.Add(new DataColumn("Intermediary Code", typeof(string)));
                             dt.Columns.Add(new DataColumn("Intermediary Location Code", typeof(String)));

                            dt.Columns.Add(new DataColumn("Any existing policy with KGI?", typeof(String)));
                           // dt.Columns.Add(new DataColumn("Sum Insured", typeof(String)));
                            dt.Columns.Add(new DataColumn("Member Premium", typeof(String)));

                            dt.Columns.Add(new DataColumn("Cross Sell Discount@10%", typeof(String)));
                            dt.Columns.Add(new DataColumn("Net Premium", typeof(String)));

                            dt.Columns.Add(new DataColumn("GST@18%", typeof(String)));
                            dt.Columns.Add(new DataColumn("Total Payable Amount", typeof(string)));
                            dt.Columns.Add(new DataColumn("Campaign source", typeof(String)));
                            dt.Columns.Add(new DataColumn("Campaign name", typeof(String)));

                            dt.Columns.Add(new DataColumn("Campaign reference", typeof(String)));
                            dt.Columns.Add(new DataColumn("Generated Link", typeof(String)));
                            dt.Columns.Add(new DataColumn("vQuoteId", typeof(String)));
                            dt.Columns.Add(new DataColumn("vConfrimationShortURL", typeof(String)));
                            dt.Columns.Add(new DataColumn("dCreatedDate", typeof(string)));

                            foreach (DataRow dritem in dtExcelData.Rows)
                            //for (int i = 0; i < length; i++)
                            {
                            string vQuoteId = wsDocNo.fn_Gen_Cert_No("UPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
                            string ConfrimURLPage = ConfigurationManager.AppSettings["vArogyaSanjeevniLink"].ToString()+"?vCustomerId="+EncryptText(dritem["Customer Code"].ToString())+"&vQuoteId="+EncryptText(vQuoteId);
                            CallFirebaseApiForShortURLConversion(ConfrimURLPage, out googleShortURL,out ErrMsg);

                            DataRow dr = dt.NewRow();

                                dr["ID"] = dritem["ID"].ToString();
                                dr["Customer Code"] = dritem["Customer Code"].ToString();
                                dr["Risk Type"] = dritem["Risk Type"].ToString();
                                dr["Title"] = dritem["Title"].ToString();

                                dr["Proposer First Name"] = dritem["Proposer First Name"].ToString();
                                dr["Proposer Middle Name"] = dritem["Proposer Middle Name"].ToString();
                                dr["Proposer Last Name"] = dritem["Proposer Last Name"].ToString();
                                dr["Gender"] = dritem["Gender"].ToString();

                              string  tempcustomerdob = dritem["DOB"].ToString();
                              string[] splitDOB = tempcustomerdob.Replace('-','/').Split('/');
                               dritem["DOB"] = splitDOB[1] + '/' + splitDOB[0] + '/' + splitDOB[2];
                                dr["DOB"] =dritem["DOB"].ToString();
                                dr["Contact number"] = dritem["Contact number"].ToString();
                                dr["Email Address"] = dritem["Email Address"].ToString();
                                dr["Marital Status"] = dritem["Marital Status"].ToString();
                                dr["Sum Insured"] = dritem["Sum Insured"].ToString();
                                dr["Address Line 1"] = dritem["Address Line 1"].ToString();
                                dr["Address Line 2"] = dritem["Address Line 2"].ToString();
                                dr["Address Line 3"] = dritem["Address Line 3"].ToString();
                                dr["Pin Code"] = dritem["Pin Code"].ToString();
                                dr["PAN Card No"] = dritem["PAN Card No"].ToString();

                                dr["Occupation"] = dritem["Occupation"].ToString();
                                dr["Intermediary Code"] = dritem["Intermediary Code"].ToString();
                              

                                dr["Intermediary Location Code"] = dritem["Intermediary Location Code"].ToString();
                                dr["Any existing policy with KGI?"] = dritem["Any existing policy with KGI?"].ToString();
                                dr["Sum Insured"] = dritem["Sum Insured"].ToString();
                                dr["Member Premium"] = dritem["Member Premium"].ToString();
                                dr["Cross Sell Discount@10%"] = dritem["Cross Sell Discount@10%"].ToString();
                                dr["Net Premium"] = dritem["Net Premium"].ToString();
                                dr["GST@18%"] = dritem["GST@18%"].ToString();
                                dr["Total Payable Amount"] = dritem["Total Payable Amount"].ToString();
                                dr["Campaign source"] = dritem["Campaign source"].ToString();

                                dr["Campaign name"] = dritem["Campaign name"].ToString();
                                dr["Campaign reference"] = dritem["Campaign reference"].ToString();
                                dr["vQuoteId"] = vQuoteId;//dritem["Campaign reference"].ToString();
                                dr["vConfrimationShortURL"] = googleShortURL;
                                dr["Generated Link"] = ConfrimURLPage;
                                dr["dCreatedDate"] = DateTime.Now;
                                
                                dt.Rows.Add(dr);
                            }


                            

                        //}

                        if (dt.Rows.Count > 0)
                        {

                       
                            foreach (DataRow drexceluploaddata in dt.Rows)
                            {
                              using (SqlConnection con = new SqlConnection(consString))
                              {
                                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                                     
                                objbulk.DestinationTableName = "TBL_AROGYA_SANJEEVNI_UPLOAD_DATA";
                                

                                objbulk.ColumnMappings.Add("[ID]", "ID");
                                objbulk.ColumnMappings.Add("[Customer Code]", "vCustomerCode");
                                objbulk.ColumnMappings.Add("[Risk Type]", "vRiskType");
                                objbulk.ColumnMappings.Add("[Title]", "vTitle");
                                objbulk.ColumnMappings.Add("[Proposer First Name]", "vProposerFirstName");

                                objbulk.ColumnMappings.Add("[Proposer Middle Name]", "vProposerMiddleName");
                                objbulk.ColumnMappings.Add("[Proposer Last Name]", "vProposerLastName");
                                objbulk.ColumnMappings.Add("[Gender]", "vGender");
                                objbulk.ColumnMappings.Add("[DOB]", "vDOB");

                                objbulk.ColumnMappings.Add("[Contact number]", "vContactnumber");
                                objbulk.ColumnMappings.Add("[Email Address]", "vEmailAddress");
                                objbulk.ColumnMappings.Add("[Marital Status]", "vMaritalStatus");
                                objbulk.ColumnMappings.Add("[Sum Insured]", "vSumInsured");
                                objbulk.ColumnMappings.Add("[Address Line 1]", "vAddressLine1");
                                objbulk.ColumnMappings.Add("[Address Line 2]", "vAddressLine2");
                                objbulk.ColumnMappings.Add("[Address Line 3]", "vAddressLine3");
                                objbulk.ColumnMappings.Add("[Pin Code]", "vPinCode");
                                objbulk.ColumnMappings.Add("[PAN Card No]", "vPANCardNo");
                                objbulk.ColumnMappings.Add("[Occupation]", "vOccupation");
                                objbulk.ColumnMappings.Add("[Intermediary Code]", "vIntermediaryCode");
                                objbulk.ColumnMappings.Add("[Intermediary Code]", "vIntermediary");
                                objbulk.ColumnMappings.Add("[Intermediary Location Code]", "vLocationCode");
                                objbulk.ColumnMappings.Add("[Any existing policy with KGI?]", "vAnyexistingpolicywithKGI");
                                objbulk.ColumnMappings.Add("[Sum Insured]", "vSumInsured1");
                                objbulk.ColumnMappings.Add("[Member Premium]", "vMemberPremium");
                                objbulk.ColumnMappings.Add("[Cross Sell Discount@10%]", "vCrossSellDiscount10Percent");
                                objbulk.ColumnMappings.Add("[Net Premium]", "vNetPremium");
                                objbulk.ColumnMappings.Add("[GST@18%]", "vGST18Percent");
                                objbulk.ColumnMappings.Add("[Total Payable Amount]", "vTotalPayableAmount");
                                objbulk.ColumnMappings.Add("[Campaign source]", "vCampaignsource");
                                objbulk.ColumnMappings.Add("[Campaign name]", "vCampaignname");
                                objbulk.ColumnMappings.Add("[Campaign reference]", "vCampaignreference");
                                objbulk.ColumnMappings.Add("[Generated Link]", "VGeneratedLink");
                                objbulk.ColumnMappings.Add("[vQuoteId]", "vQuoteId");
                                objbulk.ColumnMappings.Add("[vConfrimationShortURL]", "vConfrimationShortURL");
                                objbulk.ColumnMappings.Add("[dCreatedDate]", "dCreatedDate");

                                con.Open(); //Open DataBase conection  

                                objbulk.WriteToServer(dt);

                                Alert.Show("Data Uploaded");
                                    string _DownloadableProductFileName = "ArogyaSanjeevani_Upload_SHEET" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";
                                    strfilename = filePath + "\\" + _DownloadableProductFileName;
                                    if (ExportDataTableToExcel(dt, "ArHDC_UPLOAD_SHEET", strfilename) == true)
                                    {
                                       
                                    DownloadFile(strfilename);
                                    }
                                    btnImport.Enabled = false;
                                return;
                                //Business Validation Commented on 15-Dec-2015
                            }

                            // }
                        }
                            //objbulk.WriteToServer(dtExcelData);
                            // }
                            //if (ExportDataTableToExcel(dt, "ArHDC_UPLOAD_SHEET", strfilename) == true)
                            //{
                            //    DownloadFile(strfilename);
                            //}
                        }
                        



                    }
                    else
                    {
                        _tran.Rollback();

                        string vStatusMsg = "No Records to Upload";
                        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        return;
                    }
                }
                //_tran.Commit();
                Session["ErrorCallingPage"] = "FrmArogyaSanjeevniDataUpload.aspx";
                string vStatusMsg1 = "Data Uploaded with Upload Id  " + vUploadId; // + " and Certificate No.: " + vCertificateNo; //certificate no cannot be appended bcoz per row one certificate number gets created
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg1, false);
                return;
            }




            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmArogyaSanjeevniDataUpload.aspx.cs.Upload ");
                _tran.Rollback();

                Session["ErrorCallingPage"] = "FrmArogyaSanjeevniDataUpload.aspx";
                string vStatusMsg = "No Records for Upload or " + ex.Message.ToString().Replace("\r\n", "");
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
                //log the error
            }
        }

        private void UpdateGSTData(DataTable dtValidatedExcelData)
        {
            try
            {
                //
                PrjPASS.GSTService.GSTBreakUpTaxDetails gstBreakup = new PrjPASS.GSTService.GSTBreakUpTaxDetails();
                PrjPASS.GSTService.GCCommonClient objCommon = new PrjPASS.GSTService.GCCommonClient();

                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                for (int i = 0; i < dtValidatedExcelData.Rows.Count; i++)
                {

                    DataTable dtTemp = new DataTable();

                    string custStateCode = string.Empty;
                    string interStateCode = string.Empty;

                    try
                    {

                        using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + dtValidatedExcelData.Rows[i]["ProposerPinCode"].ToString() + "'";
                                cmd.Connection = sqlCon;
                                sqlCon.Open();
                                object objCustState = cmd.ExecuteScalar();
                                custStateCode = Convert.ToString(objCustState);

                                cmd.CommandText = "SELECT INTERMEDIARY_LOCATION FROM TBL_HDC_INTERMEDIARY_LOC  WHERE INTERMEDIARY_CODE='" + dtValidatedExcelData.Rows[i]["IntermediaryCode"].ToString() + "'";
                                object objInterState = cmd.ExecuteScalar();
                                if (objInterState == null)
                                {
                                    interStateCode = "0002"; //default set to mumbai location
                                }
                                else
                                {
                                    interStateCode = Convert.ToString(objInterState);
                                }
                                sqlCon.Close();
                            }
                        }

                        // gstBreakup = objCommon.GSTBreakUpTaxDetails(Convert.ToDateTime(dtValidatedExcelData.Rows[i]["PolicyStartDate"].ToString()).ToString("dd/MM/yyyy"), dtValidatedExcelData.Rows[i]["NetPremium"].ToString(), dtValidatedExcelData.Rows[i]["IntermediaryCode"].ToString(), interStateCode, custStateCode, ConfigurationManager.AppSettings["HDCProdCode"].ToString());
                        gstBreakup = objCommon.GSTBreakUpTaxDetails(Convert.ToDateTime("01-Jan-21").ToString("dd/MM/yyyy"), dtValidatedExcelData.Rows[i]["NetPremium"].ToString(), dtValidatedExcelData.Rows[i]["IntermediaryCode"].ToString(), interStateCode, custStateCode, ConfigurationManager.AppSettings["HDCProdCode"].ToString());

                        //what if we get something in error message
                        //passing policy start date in the parameter
                        //whether need to cross check the net premium total premium
                        //how we will show net premium
                        //round off logic to confirm with tcs

                        if (String.IsNullOrEmpty(gstBreakup.ErrroMessage))
                        {
                            if (string.IsNullOrWhiteSpace(gstBreakup.UGSTPercentage) && string.IsNullOrWhiteSpace(gstBreakup.UGSTAmount)
                                && string.IsNullOrWhiteSpace(gstBreakup.CGSTPercentage) && string.IsNullOrWhiteSpace(gstBreakup.CGSTAmount)
                                && string.IsNullOrWhiteSpace(gstBreakup.SGSTPercentage) && string.IsNullOrWhiteSpace(gstBreakup.SGSTAmount)
                                && string.IsNullOrWhiteSpace(gstBreakup.IGSTPercentage) && string.IsNullOrWhiteSpace(gstBreakup.IGSTAmount)
                                && string.IsNullOrWhiteSpace(gstBreakup.TotalGSTAmount))
                            {
                                writeErrorHDC(dtTemp, dtValidatedExcelData, i, "No Response received from GST Service", db);
                            }
                            else
                            {
                                using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                                {
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        //cmd.CommandText = "UPDATE TBL_HDC_POLICY_TABLE SET vUGSTPercentage = '" + gstBreakup.UGSTPercentage + "',vUGST = '" + gstBreakup.UGSTAmount + "',vCGSTPercentage='" + gstBreakup.CGSTPercentage + "',vCGST='" + gstBreakup.CGSTAmount + "',vSGSTPercentage='" + gstBreakup.SGSTPercentage + "',vSGST='" + gstBreakup.SGSTAmount + "',vIGSTPercentage='" + gstBreakup.IGSTPercentage + "',vIGST='" + gstBreakup.IGSTAmount + "',vTotalGSTAmount='" + gstBreakup.TotalGSTAmount + "',vGSTCustState='" + gstBreakup.CustomerGSTStateIdentifier + "',vGSTIntermediaryState='" + gstBreakup.IntermediaryGSTStateIdentifier + "' where vCertificateNo='" + dtValidatedExcelData.Rows[i]["CertificateNo"].ToString() + "'";
                                        cmd.CommandText = "UPDATE tbl_hdc_policy_replica_table SET vUGSTPercentage = '" + gstBreakup.UGSTPercentage + "',vUGST = '" + gstBreakup.UGSTAmount + "',vCGSTPercentage='" + gstBreakup.CGSTPercentage + "',vCGST='" + gstBreakup.CGSTAmount + "',vSGSTPercentage='" + gstBreakup.SGSTPercentage + "',vSGST='" + gstBreakup.SGSTAmount + "',vIGSTPercentage='" + gstBreakup.IGSTPercentage + "',vIGST='" + gstBreakup.IGSTAmount + "',vTotalGSTAmount='" + gstBreakup.TotalGSTAmount + "',vGSTCustState='" + gstBreakup.CustomerGSTStateIdentifier + "',vGSTIntermediaryState='" + gstBreakup.IntermediaryGSTStateIdentifier + "' where vCertificateNo='" + dtValidatedExcelData.Rows[i]["CertificateNo"].ToString() + "'";
                                        cmd.Connection = sqlCon;
                                        sqlCon.Open();
                                        cmd.ExecuteNonQuery();

                                        double netPrem = Convert.ToDouble(dtValidatedExcelData.Rows[i]["TotalPremium"].ToString()) - Convert.ToDouble(gstBreakup.TotalGSTAmount);

                                        //cmd.CommandText = "UPDATE TBL_GPA_POLICY_TABLE SET nNetPremium ='" + netPrem.ToString() + "' where vCertificateNo='" + dtValidatedExcelData.Rows[i]["CertificateNo"].ToString() + "'";
                                        //cmd.ExecuteNonQuery();
                                        sqlCon.Close();
                                    }
                                }
                            }
                        }
                        else
                        {
                            #region commented code
                            //dtTemp = dtValidatedExcelData.Clone();

                            //dtValidatedExcelData.Rows[i]["vTransType"] = "PUP";
                            //dtValidatedExcelData.Rows[i]["vErrorFlag"] = "Y";
                            //dtValidatedExcelData.Rows[i]["vErrorDesc"] = gstBreakup.ErrroMessage;

                            //dtTemp.ImportRow(dtValidatedExcelData.Rows[i]);

                            //Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
                            //using (SqlConnection con = new SqlConnection(db.ConnectionString))
                            //{
                            //    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            //    {
                            //        sqlBulkCopy.DestinationTableName = "dbo.TBL_HDC_POLICY_TABLE_ERROR_LOG";
                            //        string sqlCommand = "SELECT  * FROM TBL_HDC_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N'";
                            //        DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                            //        DataSet ds = null;
                            //        ds = dbCOMMON.ExecuteDataSet(dbCommand);

                            //        if (ds.Tables[0].Rows.Count > 0)
                            //        {
                            //            sqlBulkCopy.ColumnMappings.Add("vTransType", "vTransType");
                            //            sqlBulkCopy.ColumnMappings.Add("vErrorFlag", "vErrorFlag");
                            //            sqlBulkCopy.ColumnMappings.Add("vErrorDesc", "vErrorDesc");

                            //            foreach (DataRow row in ds.Tables[0].Rows)
                            //            {
                            //                sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                            //            }
                            //        }

                            //        con.Open();
                            //        sqlBulkCopy.WriteToServer(dtTemp);
                            //        con.Close();
                            //    }

                            //    using (SqlCommand cmd = new SqlCommand())
                            //    {
                            //        cmd.CommandText = "DELETE FROM TBL_HDC_POLICY_TABLE WHERE vCertificateNo='" + dtValidatedExcelData.Rows[i]["CertificateNo"].ToString() + "'";
                            //        cmd.Connection = con;
                            //        con.Open();
                            //        cmd.ExecuteNonQuery();

                            //        //double netPrem = Convert.ToDouble(dtValidatedExcelData.Rows[i]["TotalPolicyPremium"].ToString()) - Convert.ToDouble(gstBreakup.TotalGSTAmount);

                            //        //cmd.CommandText = "UPDATE TBL_GPA_POLICY_TABLE SET nNetPremium ='" + netPrem.ToString() + " where vCertificateNo='" + dtValidatedExcelData.Rows[i]["CertificateNo"].ToString() + "'";
                            //        //cmd.ExecuteNonQuery();

                            //        con.Close();
                            //    }

                            //}
                            #endregion
                            writeErrorHDC(dtTemp, dtValidatedExcelData, i, gstBreakup.ErrroMessage, db);

                        }

                    }

                    catch (Exception ex)
                    {
                        ExceptionUtility.LogException(ex, "FrmArogyaSanjeevniDataUpload-writeErrorHDC");
                        writeErrorHDC(dtTemp, dtValidatedExcelData, i, "Error while fetching GST", db);

                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmArogyaSanjeevniDataUpload-UpdateGSTData");

            }
        }

        private void writeErrorHDC(DataTable dtTemp, DataTable dtValidatedExcelData, int i, string ErrorMessage, Database db)
        {

            dtTemp = dtValidatedExcelData.Clone();

            dtValidatedExcelData.Rows[i]["vTransType"] = "PUP";
            dtValidatedExcelData.Rows[i]["vErrorFlag"] = "Y";
            dtValidatedExcelData.Rows[i]["vErrorDesc"] = ErrorMessage;

            dtTemp.ImportRow(dtValidatedExcelData.Rows[i]);

            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            try
            {
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "dbo.TBL_HDC_POLICY_TABLE_ERROR_LOG";
                        string sqlCommand = "SELECT  * FROM TBL_HDC_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N'";
                        DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                        DataSet ds = null;
                        ds = dbCOMMON.ExecuteDataSet(dbCommand);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            sqlBulkCopy.ColumnMappings.Add("vTransType", "vTransType");
                            sqlBulkCopy.ColumnMappings.Add("vErrorFlag", "vErrorFlag");
                            sqlBulkCopy.ColumnMappings.Add("vErrorDesc", "vErrorDesc");

                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                            }
                        }

                        con.Open();
                        try
                        {
                            sqlBulkCopy.WriteToServer(dtTemp);
                            con.Close();

                        }

                        catch (SqlException ex)
                        {


                            ExceptionUtility.LogException(ex, "writeErrorHDC  ");
                            if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                            {
                                string pattern = @"\d+";
                                Match match = Regex.Match(ex.Message.ToString(), pattern);
                                var index = Convert.ToInt32(match.Value) - 1;

                                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                                var sortedColumns = fi.GetValue(sqlBulkCopy);
                                var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                                FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                                var metadata = itemdata.GetValue(items[index]);

                                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                ExceptionUtility.LogException(ex, String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
                                throw new Exception(String.Format("Column: {0} contains data with a length greater than: {1}", column, length));

                            }

                            //  throw;
                        }

                    }

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        //cmd.CommandText = "DELETE FROM TBL_HDC_POLICY_TABLE WHERE vCertificateNo='" + dtValidatedExcelData.Rows[i]["CertificateNo"].ToString() + "'";
                        cmd.CommandText = "DELETE FROM tbl_hdc_policy_replica_table WHERE vCertificateNo='" + dtValidatedExcelData.Rows[i]["CertificateNo"].ToString() + "'";
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();

                        //double netPrem = Convert.ToDouble(dtValidatedExcelData.Rows[i]["TotalPolicyPremium"].ToString()) - Convert.ToDouble(gstBreakup.TotalGSTAmount);

                        //cmd.CommandText = "UPDATE TBL_GPA_POLICY_TABLE SET nNetPremium ='" + netPrem.ToString() + " where vCertificateNo='" + dtValidatedExcelData.Rows[i]["CertificateNo"].ToString() + "'";
                        //cmd.ExecuteNonQuery();

                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Exception Occured for WriteErrorHDC ");
            }

        }

        protected string[] Fn_Check_Business_Validation(string vFieldName, string vFieldValue)
        {
            string[] ckvalidflag = new string[2];
            ckvalidflag[0] = "true";
            ckvalidflag[1] = " ";

            return ckvalidflag;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }




        protected void btnExport_Click(object sender, EventArgs e)
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            string sqlCommand = "SELECT  * FROM TBL_AROGYA_SANJEEVNI_UPLOAD_DATA";
            DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
            DataSet dsColumnNames = null;
            dsColumnNames = dbCOMMON.ExecuteDataSet(dbCommand);

            if (dsColumnNames.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsColumnNames.Tables[0].Rows)
                {
                    TemplateTable.Columns.Add(row["vSourceColumnName"].ToString());
                }
            }

            DataRow newBlankRow1 = TemplateTable.NewRow();
            TemplateTable.Rows.InsertAt(newBlankRow1, 0);


            string filePath = Server.MapPath("~/Reports");

            string _DownloadableProductFileName = "HDC_UPLOAD_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "HDC_UPLOAD_SHEET", strfilename) == true)
            {
                DownloadFile(strfilename);
            }
        }

        private bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        {
            //Creae an Excel application instance

            ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
            ExcelRange oRng;

            try
            {

                // Workk sheet
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheet.Name = worksheetName;

                //excelSheet.Cells[1, 1] = "Detail";
                //excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

                // loop through each row and add values to our sheet
                int rowcount = 0;


                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;

                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 1)
                        {
                            excelSheet.Cells[1, i].Value = dataTable.Columns[i - 1].ColumnName;
                            // excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
                        }

                        excelSheet.Cells[rowcount + 1, i].Value = datarow[i - 1].ToString();

                        //for alternate rows
                        if (rowcount > 1)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    oRng = (ExcelRange)excelSheet.Cells[rowcount, dataTable.Columns.Count];
                                    FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
                                }
                            }
                        }
                    }
                }

                // now we resize the columns
                oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
                oRng.AutoFitColumns();
                BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));

                oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
                FormattingExcelCells(oRng, "#000099", System.Drawing.Color.White, true);


                //now save the workbook and exit Excel

                excelApp.Save();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "ExprtDataTableToExcel()");
                Alert.Show(ex.Message);
                return false;
            }
            finally
            {
                excelApp = null;
            }
        }
        private void BorderAround(ExcelRange range, int colour)
        {
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.ColorTranslator.FromOle(colour));
        }
        public void FormattingExcelCells(ExcelRange range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.Indexed = 16;
            range.Style.Font.Size = 12;
            //range.Style.Font.Color = System.Drawing.Color.White;
            if (IsFontbool == true)
            {
                range.Style.Font.Bold = IsFontbool;
            }
        }

     
        public bool DownloadFile(string strfilename)
        {
            bool res = false;
            try
            {
                ExceptionUtility.LogEvent("Start FrmMobileRenewalUpload.aspx DownloadFile method");
                string filePath = Server.MapPath("~/Reports/AROGYA/");
                string _DownloadableProductFileName = strfilename;

                System.IO.FileInfo FileName = new System.IO.FileInfo(strfilename);
                FileStream myFile = new FileStream(strfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                //Reads file as binary values
                BinaryReader _BinaryReader = new BinaryReader(myFile);

                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode
                    (_DownloadableProductFileName, Encoding.UTF8) + lastUpdateTiemStamp;

                //Clear the content of the response
                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);

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
                ExceptionUtility.LogEvent("End FrmMobileRenewalUpload.aspx DownloadFile method");

                //compare packets transferred with total number of packets
                if (i < maxCount)
                {
                    res = false;
                }
                else
                {
                    res = true;
                }

                //Close Binary reader and File stream
                _BinaryReader.Close();
                myFile.Close();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "DownloadFile");
                Alert.Show(ex.Message);
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnUpload_Click ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
            }
            return res;
        }
        public static string EncryptText(string clearText)
        {
            string EncryptionKey = "KGIMAV2BNI1907";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string DecryptText(string cipherText)
        {
            string EncryptionKey = "KGIMAV2BNI1907";
            byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        private void GoogleURLShortner(string longURL, out string shortURL)
        {
            shortURL = string.Empty;
            // If we did not construct the service so far, do it now.
            if (service == null)
            {
                BaseClientService.Initializer initializer = new BaseClientService.Initializer();
                // You can enter your developer key for services requiring a developer key.
                initializer.ApiKey = "AIzaSyAUDi9QAlEmAPy1lhPwfEHxDeXiQtnKgUI";
                service = new UrlshortenerService(initializer);
            }

            string url = longURL;
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            // Execute methods on the UrlShortener service based upon the type of the URL provided.
            try
            {
                string resultURL;
                if (IsShortUrl(url))
                {
                    // Expand the URL by using a Url.Get(..) request.
                    Url result = service.Url.Get(url).Execute();
                    resultURL = result.LongUrl;
                }
                else
                {
                    // Shorten the URL by inserting a new Url.
                    Url toInsert = new Url { LongUrl = url };
                    toInsert = service.Url.Insert(toInsert).Execute();
                    resultURL = toInsert.Id;
                }
                shortURL = resultURL;
                //string s = string.Format("<a href=\"{0}\">{0}</a>", resultURL);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
            }
        }

        private static bool CallFirebaseApiForShortURLConversion(string LongURL, out string ShortURL, out string ErrMsg)
        {
            bool IsSuccess = false;
            ShortURL = string.Empty;
            ErrMsg = string.Empty;
            try
            {
                string FirbaseClientURL = ConfigurationManager.AppSettings["FirbaseClientURL"].ToString();

                RootObjectReq requests = new RootObjectReq();
                Suffix objsuffix = new Suffix();
                objsuffix.option = "SHORT";

                requests.longDynamicLink = FirbaseClientURL + System.Web.HttpUtility.UrlEncode(LongURL);
                requests.suffix = objsuffix;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string output = serializer.Serialize(requests);

                string FirebaseURL = ConfigurationManager.AppSettings["FirebaseURL"].ToString();


                string strUri = FirebaseURL;

                //SR73845 - to fix - added the below 2 lines before (HttpWebRequest)WebRequest.Create(uri); earlier these lines were inside the if condition The request was aborted: Could not create SSL/TLS secure channel
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager


                Uri uri = new Uri(strUri);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                //WebRequest request = WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                request.Timeout = 30000; //1 minute = 60000 miliseconds
                request.KeepAlive = true;

                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() != "0")
                {

                    string proxy_network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                    string proxy_network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                    string proxy_network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();
                    string proxy_Url = ConfigurationManager.AppSettings["proxy_Url"].ToString();

                    WebProxy proxy = new WebProxy(proxy_Url, true);
                    proxy.Credentials = new NetworkCredential(proxy_network_Username, proxy_network_Password, proxy_network_domain);
                    request.Proxy = proxy;
                    request.Credentials = new NetworkCredential(proxy_network_Username, proxy_network_Password, proxy_network_domain);
                    request.Proxy.Credentials = new NetworkCredential(proxy_network_Username, proxy_network_Password, proxy_network_domain);
                }

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                string serOut = jsonSerializer.Serialize(requests);

                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(serOut);
                }

                WebResponse responce = request.GetResponse();
                Stream reader = responce.GetResponseStream();

                StreamReader sReader = new StreamReader(reader);
                string outResult = sReader.ReadToEnd();
                sReader.Close();

                if (IsJson(outResult))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer2 = new System.Web.Script.Serialization.JavaScriptSerializer();
                    RootObjectRes objRootObjectRes = jsonSerializer2.Deserialize<RootObjectRes>(outResult);
                    if (objRootObjectRes != null)
                    {
                        if (objRootObjectRes.shortLink != null)
                        {
                            ShortURL = objRootObjectRes.shortLink;
                            IsSuccess = true;
                            ErrMsg = "";
                        }
                        else
                        {
                            IsSuccess = false;
                        }
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
                else
                {
                    IsSuccess = false;
                    ErrMsg = "Response is not in JSON format: " + outResult;
                }

                WebRequest.DefaultWebProxy = null;
            }
            catch (WebException e)
            {
                clsAppLogs.LogException(e);
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        IsSuccess = false;
                        ErrMsg = text;
                        clsAppLogs.LogException(e);
                    }
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                ErrMsg = ex.Message;
                clsAppLogs.LogException(ex);
            }
            finally
            {
                WebRequest.DefaultWebProxy = null; //SR73845 - Hasmukh
            }

            return IsSuccess;
        }
        //private void GoogleURLShortner(string longURL, out string shortURL)
        //{
        //    shortURL = string.Empty;
        //    try
        //    {
        //        string ErrorMsg = string.Empty;
        //        WebRequest.DefaultWebProxy = null;
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager
        //        //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
        //        PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
        //        string UserId = ConfigurationManager.AppSettings["UserIdForShortURL"].ToString();
        //        string AccessKey = ConfigurationManager.AppSettings["AccessKeyForShortURL"].ToString();
        //        proxy.ConvertLongURLToShortURL(UserId, AccessKey, longURL, out shortURL, out ErrorMsg);
        //        proxy.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
        //    }
        //}

        public class Suffix
        {
            public string option { get; set; }
        }

        public class RootObjectReq
        {
            public string longDynamicLink { get; set; }
            public Suffix suffix { get; set; }
        }

        public class Warning
        {
            public string warningCode { get; set; }
            public string warningMessage { get; set; }
        }

        public class RootObjectRes
        {
            public string shortLink { get; set; }
            public List<Warning> warning { get; set; }
            public string previewLink { get; set; }
        }

        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }
    }
}