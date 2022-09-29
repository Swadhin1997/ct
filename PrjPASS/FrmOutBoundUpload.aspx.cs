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
using PrjPASS;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Net.Mail;
using System.Net;
using Microsoft.Web.Services3.Security.Tokens;
using System.Security.Authentication;

namespace ProjectPASS
{

    public class clsTalismaIteraction
    {
        public string InteractionID { get; set; }
        public string PolicyNo { get; set; }
        //Added By Nilesh 26Aug2019
        public string PolicyNoPropID { get; set; }
        //end by Nilesh
        public string ProposalNo { get; set; }
        public string PartnerAppNo { get; set; }
        public string ContactNo { get; set; }
        public string LOB { get; set; }
        public string CaseType { get; set; }
        public string CallType { get; set; }
        public string SubCallType { get; set; }
        public string RTO_Reason { get; set; }
        public string ContactName { get; set; }
        public string FirstAssigned { get; set; }
        public string FirstAssignedPropValue { get; set; }
        public string FCR_NONFCR { get; set; }
        public string FollowUPDay { get; set; }
        public string InteractionStatus { get; set; }
        public string ErrorMessage { get; set; }
        public string LOBPropID { get; set; }
        public string LOBPropValue { get; set; }
        public string CaseTypePropID { get; set; }
        public string CaseTypePropValue { get; set; }
        public string CallTypePropID { get; set; }
        public string CallTypePropValue { get; set; }
        public string SubCallTypePropID { get; set; }
        public string SubCallTypePropValue { get; set; }
        public string RTO_ReasonPropID { get; set; }
        public string RTO_ReasonPropValue { get; set; }
        public string FCRPropID { get; set; }
        public string FCRPropValue { get; set; }
        public string FirstAssignedPropID { get; set; }
        public string uploadID { get; set; }

    }





    public partial class FrmOutBoundUpload : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString);
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        string productCode = string.Empty, vUploadId = "";


        string userName = ConfigurationManager.AppSettings["userName"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();
        string TalismaSessionKey = ConfigurationManager.AppSettings["TalismaSessionKey"].ToString();
        int InteractioniServiceArrayIndex = Convert.ToInt32(ConfigurationManager.AppSettings["InteractioniServiceArrayIndex"].ToString());
        int ContactiServiceArrayIndex = Convert.ToInt32(ConfigurationManager.AppSettings["ContactiServiceArrayIndex"]);
        string ContactiserviceURL = ConfigurationManager.AppSettings["ContactiserviceURL"];
        string InteractioniserviceURL = ConfigurationManager.AppSettings["InteractioniserviceURL"];
        string TalismaInteractionLogFile = AppDomain.CurrentDomain.BaseDirectory + "TalismaInteractionLog.txt";
        string TalismaInteractionLogFile_Error = AppDomain.CurrentDomain.BaseDirectory + "TalismaInteractionLog_error.txt";

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

            if (!File.Exists(TalismaInteractionLogFile))
            {
                File.Create(TalismaInteractionLogFile);
            }

        }

        protected void Upload(object sender, EventArgs e)
        {
            DataSet DS = null;
            if (String.IsNullOrEmpty(FileUpload1.FileName.ToString()))
            {
                Alert.Show("Please Select valid excel file or Excel file not selected!");
                return;
            }

            string cYearMonth = "";
            string vCertificateNo = "";

            cYearMonth = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                cYearMonth = cYearMonth + "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                cYearMonth = cYearMonth + DateTime.Now.Month.ToString();
            }

            //vUploadId = wsDocNo.fn_Gen_Doc_Master_No("CPUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
            //vUploadId = "OBUPL" + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt").Replace("/", "_").Replace(":", "").Replace(" ", "");
            vUploadId = fngetUploadID();
            hfUploadId.Value = vUploadId.ToString();
            try
            {
                //Upload and save the file
                string Message = "";
                string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(excelPath);
                //long count = ImportExcelToSQL(excelPath, Session["vUserLoginId"].ToString(), vUploadId, out Message);
                ImportExcelToSQL(excelPath, Session["vUserLoginId"].ToString(), FileUpload1.FileName.ToString(), out Message);
                Alert.Show("File Uploaded Successfully.", "FrmOutBoundUpload.aspx");
            }
            catch (Exception ex)
            {
                //ExceptionUtility.LogException(ex, "Error in Upload method, FrmOutBoundUpload Page.");
                File.AppendAllText(TalismaInteractionLogFile_Error, "FrmOutBoundUpload Error occured in Function Upload  and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
            }
        }

        private string fngetUploadID()
        {
            string uploadid = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PRC_GET_UPLOAD_ID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                uploadid = dr[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ExceptionUtility.LogException(ex, "fngetUploadID");
                File.AppendAllText(TalismaInteractionLogFile_Error, "FrmOutBoundUpload Error occured in Function fngetUploadID and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
            }
            return uploadid;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
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

        public void ImportExcelToSQL(string excelfilepath, string ExcelUploadedBy, string UploadedFileName, out string Message)
        {
            Message = string.Empty;
            long Count = 0;
            try
            {
                string conString = string.Empty;
                string extension = Path.GetExtension(excelfilepath);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;

                }
                conString = string.Format(conString, excelfilepath);
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    DataTable dtExcelData = new DataTable();

                    dtExcelData.Columns.Add("InteractionID", typeof(String));
                    dtExcelData.Columns.Add("PolicyNo", typeof(String));
                    dtExcelData.Columns.Add("ProposalNo", typeof(String));
                    dtExcelData.Columns.Add("PartnerApplicationNo", typeof(String));
                    dtExcelData.Columns.Add("ContactNo", typeof(String));
                    dtExcelData.Columns.Add("Lineofbusiness", typeof(String));
                    dtExcelData.Columns.Add("CaseType", typeof(String));
                    dtExcelData.Columns.Add("CallType", typeof(String));
                    dtExcelData.Columns.Add("SubCallType", typeof(String));
                    dtExcelData.Columns.Add("ContactName", typeof(String));
                    dtExcelData.Columns.Add("FirstAssigned", typeof(String));
                    dtExcelData.Columns.Add("[FCR/NONFCR]", typeof(String));
                    dtExcelData.Columns.Add("FollowUPDay", typeof(String));
                    dtExcelData.Columns.Add("InteractionStatus", typeof(String));
                    dtExcelData.Columns.Add("RTO_Reason", typeof(String));

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT  InteractionID,PolicyNo," +
                        "Format([ProposalNo], \"#####\") as ProposalNo , Format([PartnerApplicationNo], \"#####\") as PartnerApplicationNo," +
                        "ContactNo,Lineofbusiness,CaseType,CallType,SubCallType,RTO_Reason,ContactName,FirstAssigned,[FCR/NONFCR]," +
                        "FollowUPDay,InteractionStatus FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();
                    if (dtExcelData.Rows.Count <= 0)
                    {
                        Alert.Show("Datatable row count is 0.", "FrmOutBoundUpload.aspx");
                        return;
                    }
                    else
                    {

                        string ExcelValidationMesage = string.Empty;

                        bool res = fnValidateExcelData(dtExcelData, out ExcelValidationMesage);

                        if (res)
                        {
                            dtExcelData.Columns.Add("vUploadID", typeof(String));
                            foreach (DataRow dr in dtExcelData.Rows)
                            {
                                dr["vUploadID"] = vUploadId;
                            }

                            string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                            using (SqlConnection con = new SqlConnection(consString))
                            {
                                BulkCopyToDatabase(dtExcelData, con);
                            }
                            DataTable dtValidatedData;
                            if (dtExcelData.Rows[0]["PolicyNo"].ToString() == "")
                            {
                                dtValidatedData = fnGetValidatedDataWithoutPolicyNo(vUploadId);
                            }
                            else
                            {
                                dtValidatedData = fnGetValidatedData(vUploadId);
                            }
                            string InteractionID, PolicyNo, PolicyNoPropID, ProposalNo, PartnerAppNo,
                                   ContactNo, LOB, CaseType, CallType, SubCallType, RTO_Reason, ContactName,
                                   FirstAssigned, FCR_NONFCR, FollowUPDay, InteractionStatus, ErrorMessage,
                                   LOBPropID, LOBPropValue, CaseTypePropID, CaseTypePropValue,
                                   CallTypePropID, CallTypePropValue, SubCallTypePropID, SubCallTypePropValue;

                            List<clsTalismaIteraction> lstTalisma = new List<clsTalismaIteraction>();
                            foreach (DataRow dr in dtValidatedData.Rows)
                            {
                                clsTalismaIteraction oclsTalismaIteraction = new clsTalismaIteraction();

                                oclsTalismaIteraction.InteractionID = dr["vInteractionID"].ToString();
                                if (dtExcelData.Rows[0]["PolicyNo"].ToString() == "")
                                {
                                    oclsTalismaIteraction.PolicyNo = "";
                                }
                                else
                                {
                                    oclsTalismaIteraction.PolicyNo = dr["vPolicyNo"].ToString();
                                }
                                //Added By Nilesh
                                oclsTalismaIteraction.PolicyNoPropID = dr["Policy_No_PropID"].ToString();
                                //End By Nilesh
                                oclsTalismaIteraction.ProposalNo = dr["vProposalNo"].ToString();
                                oclsTalismaIteraction.PartnerAppNo = dr["vPartnerApplicationNo"].ToString();
                                oclsTalismaIteraction.ContactNo = dr["vContactNo"].ToString();

                                oclsTalismaIteraction.LOB = dr["vLineofbusiness"].ToString();
                                oclsTalismaIteraction.LOBPropID = dr["LOBPropID"].ToString();
                                oclsTalismaIteraction.LOBPropValue = dr["LOBPropValue"].ToString();


                                oclsTalismaIteraction.CaseType = dr["vCaseType"].ToString();
                                oclsTalismaIteraction.CaseTypePropID = dr["CaseTypePropID"].ToString();
                                oclsTalismaIteraction.CaseTypePropValue = dr["CaseTypePropValue"].ToString();


                                oclsTalismaIteraction.CallType = dr["vCallType"].ToString();
                                oclsTalismaIteraction.CallTypePropID = dr["CallTypePropID"].ToString();
                                oclsTalismaIteraction.CallTypePropValue = dr["CallTypePropValue"].ToString();



                                oclsTalismaIteraction.SubCallType = dr["vSubCallType"].ToString();
                                oclsTalismaIteraction.SubCallTypePropID = dr["SubCallTypePropID"].ToString();
                                oclsTalismaIteraction.SubCallTypePropValue = dr["SubCallTypePropValue"].ToString();


                                oclsTalismaIteraction.RTO_Reason = dr["vRTO_Reason"].ToString();
                                if (!string.IsNullOrEmpty(dr["vRTO_Reason"].ToString()))
                                {
                                    string[] RTO_Reason_PROPID_VALUE = new string[2];

                                    RTO_Reason_PROPID_VALUE = fnGetRTO_ReasonPropID(oclsTalismaIteraction.RTO_Reason);

                                    if (!string.IsNullOrEmpty(RTO_Reason_PROPID_VALUE[0]))
                                    {
                                        oclsTalismaIteraction.RTO_ReasonPropID = RTO_Reason_PROPID_VALUE[0];
                                        oclsTalismaIteraction.RTO_ReasonPropValue = RTO_Reason_PROPID_VALUE[1];
                                    }
                                    else
                                    {
                                        oclsTalismaIteraction.ErrorMessage = "RTO Reason is not valid";
                                    }
                                }




                                oclsTalismaIteraction.ContactName = dr["vContactName"].ToString();



                                oclsTalismaIteraction.FCR_NONFCR = dr["vFCR/NONFCR"].ToString();
                                oclsTalismaIteraction.FCRPropID = dr["FCR_NFCR_PropID"].ToString();
                                oclsTalismaIteraction.FCRPropValue = dr["FCR_NFCR_PropValue"].ToString();

                                oclsTalismaIteraction.FirstAssigned = dr["vFirstAssigned"].ToString();
                                oclsTalismaIteraction.FirstAssignedPropID = dr["AssignedToPropID"].ToString();
                                oclsTalismaIteraction.FirstAssignedPropValue = fnGetFirstAssignedPropValue(dr["vFirstAssigned"].ToString());

                                oclsTalismaIteraction.FollowUPDay = dr["vFollowUPDay"].ToString();
                                oclsTalismaIteraction.InteractionStatus = dr["vInteractionStatus"].ToString();

                                oclsTalismaIteraction.ErrorMessage = dr["vErrorMessage"].ToString();
                                oclsTalismaIteraction.uploadID = vUploadId;

                                if (!string.IsNullOrEmpty(oclsTalismaIteraction.PolicyNo))
                                {
                                    if (oclsTalismaIteraction.PolicyNo.Length != 10)
                                    {
                                        oclsTalismaIteraction.ErrorMessage = " Policy Number length not equal to 10. ";
                                    }

                                }


                                if (!string.IsNullOrEmpty(oclsTalismaIteraction.InteractionID))
                                {
                                    if (string.IsNullOrEmpty(oclsTalismaIteraction.SubCallType) || string.IsNullOrEmpty(oclsTalismaIteraction.InteractionStatus))
                                    {
                                        oclsTalismaIteraction.ErrorMessage += " Interaction Sub call type or status not provided with Interaction ID.";
                                    }
                                }


                                if (oclsTalismaIteraction.SubCallType == "RTO Call" && string.IsNullOrEmpty(oclsTalismaIteraction.RTO_Reason))
                                {
                                    oclsTalismaIteraction.ErrorMessage += " If Subcall Type is RTO Call then RTO Reason is mandatory.";
                                }


                                if (string.IsNullOrEmpty(oclsTalismaIteraction.PolicyNo) && string.IsNullOrEmpty(oclsTalismaIteraction.ContactNo))
                                {
                                    oclsTalismaIteraction.ErrorMessage += " If policy number is not available then contact number is mandatory.";
                                }

                                if (string.IsNullOrEmpty(oclsTalismaIteraction.ProposalNo) && string.IsNullOrEmpty(oclsTalismaIteraction.PolicyNo) && string.IsNullOrEmpty(oclsTalismaIteraction.ContactNo))
                                {
                                    if (oclsTalismaIteraction.ProposalNo.Length != 16)
                                    {
                                        oclsTalismaIteraction.ErrorMessage += " If policy number or contact number is not available then 16 digit Proposal number is mandatory.";
                                    }
                                }



                                if (oclsTalismaIteraction.ContactNo.Length != 10)
                                {
                                    oclsTalismaIteraction.ErrorMessage += " Contact number must be 10 digit number.";
                                }

                                Regex MobNumRegex = new Regex("^[0-9]{10}$");

                                if (!MobNumRegex.IsMatch(oclsTalismaIteraction.ContactNo))
                                {
                                    oclsTalismaIteraction.ErrorMessage += " Contact number must be 10 digit number.";
                                }

                                if (String.IsNullOrEmpty(oclsTalismaIteraction.PolicyNo))
                                {
                                    if (string.IsNullOrEmpty(oclsTalismaIteraction.ContactName))
                                    {
                                        oclsTalismaIteraction.ErrorMessage += " Policy number is empty then Contact Name is compulsary.";
                                    }
                                }
                                //commnet by Nilesh
                                File.AppendAllText(TalismaInteractionLogFile_Error, "FrmOutBoundUpload Error occured in dtFollowUpDay " + oclsTalismaIteraction.FollowUPDay + "  " + DateTime.Now + Environment.NewLine);
                                DateTime dtFollowUpDay = Convert.ToDateTime(oclsTalismaIteraction.FollowUPDay);
                               

                                if (dtFollowUpDay.Date >= DateTime.Now.Date)
                                {

                                }
                                else
                                {
                                    oclsTalismaIteraction.ErrorMessage += " Follow up day must be Today or greater than today.";
                                }

                                if (!string.IsNullOrEmpty(oclsTalismaIteraction.PolicyNo))
                                {
                                    oclsTalismaIteraction.ProposalNo = "";
                                    oclsTalismaIteraction.PartnerAppNo = "";
                                    //oclsTalismaIteraction.ContactNo = "";
                                    //oclsTalismaIteraction.ContactName = "";

                                    if (fnCheckPolicyExistsInTalisma(oclsTalismaIteraction.PolicyNo))
                                    {

                                    }
                                    else
                                    {
                                        oclsTalismaIteraction.ErrorMessage += " Policy not found.";
                                    }
                                }
                                //commnet by Nilesh
                                lstTalisma.Add(oclsTalismaIteraction);

                            }

                            foreach (clsTalismaIteraction c in lstTalisma)
                            {

                                if (!string.IsNullOrEmpty(c.ErrorMessage))
                                {
                                    fnSaveErrorLog(c);
                                }

                                if (string.IsNullOrEmpty(c.InteractionID) && string.IsNullOrEmpty(c.ErrorMessage))
                                {
                                    fnCreateNewInteraction(c);
                                }

                                else
                                {
                                    //fnUpdateInteraction()
                                }
                            }



                            fnSendBatchProcessEmail(vUploadId);


                            fndeleteTempUploadData(vUploadId);


                        }
                        else
                        {
                            Alert.Show(" Data related issue in Excel file. Please refer message " + ExcelValidationMesage.ToString());
                            return;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                fnDeleteAllData(vUploadId);
                //ExceptionUtility.LogException(ex, "Error in ImportExcelToSQL on FrmOutBoundUpload.aspx Page");
                File.AppendAllText(TalismaInteractionLogFile_Error, "FrmOutBoundUpload Error occured in Function ImportExcelToSQL and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured. Please check the file format of uploaded file and all columns are available and date in dd-MMM-yy format");
                return;
            }
        }


        private string[] fnGetRTO_ReasonPropID(string RTO_Reason)
        {
            string[] RTOReason_PropID_value = new string[2];
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_RTO_REASON_PROP_ID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RTO_Reason", RTO_Reason);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            {
                                while (dr.Read())
                                {
                                    RTOReason_PropID_value[0] = dr["vRTO_Reason_PropertyID"].ToString();
                                    RTOReason_PropID_value[1] = dr["vRTO_Reason_PropertyValue"].ToString();
                                }
                            }
                        }
                    }
                }
                File.AppendAllText(TalismaInteractionLogFile, System.Environment.NewLine);
                File.AppendAllText(TalismaInteractionLogFile, string.Format("RTOReason_PropID_value[0]  {0}", RTOReason_PropID_value[0]));
                File.AppendAllText(TalismaInteractionLogFile, string.Format("RTOReason_PropID_value[1]  {1}", RTOReason_PropID_value[1]));
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnGetRTO_ReasonPropID");
            }
            return RTOReason_PropID_value;
        }


        //Added By Nilesh
        private bool fnCheckContactExistsInTalisma(string contactname, long contactno)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GETCONTACT_DETAILS_FROM_TALISMA", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vContactName", contactname);
                        cmd.Parameters.AddWithValue("@vContactNumber", Convert.ToString(contactno));
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {
                                res = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnCheckPolicyExistsInTalisma");

            }
            return res;
        }

        private DataTable fnRetriveContactDetailsFromTalisma(string Policy_Number)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GETPOLICY_DETAILS_FROM_TALISMA", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vPolicy_Number", Policy_Number);
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnCheckPolicyExistsInTalisma");

            }
            return dt;
        }
        //End By Nilesh

        private bool fnCheckPolicyExistsInTalisma(string policyNo)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GETPOLICY_DETAILS_FROM_TALISMA", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vPolicyNo", policyNo);
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {
                                res = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnCheckPolicyExistsInTalisma");

            }
            return res;
        }

        private string fnGetFirstAssignedPropValue(string FirstAssigned)
        {
            string FirstAssignedPropValue = "";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_FIRST_ASSIGNED_PROP_VALUE", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FirstAssigned", FirstAssigned);
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {

                                FirstAssignedPropValue = dr[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnGetFirstAssignedPropValue");
            }
            return FirstAssignedPropValue;
        }

        private void fnSendBatchProcessEmail(string vUploadId)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PRC_GET_OUTBOUND_BATCH_LEVEL_DATA", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vUploadID", vUploadId);
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(ds);
                    }
                }

                string Mailbody = "";

                string strPath = AppDomain.CurrentDomain.BaseDirectory + "OutBound_email_bodydetails.html";
                Mailbody = File.ReadAllText(strPath);
                Mailbody = Mailbody.Replace("#BatchNo", ds.Tables[0].Rows[0]["BatchNo"].ToString());
                Mailbody = Mailbody.Replace("#BatchUploadDate", ds.Tables[1].Rows[0]["DateofBatchUpload"].ToString());
                Mailbody = Mailbody.Replace("#BatchUploadTime", ds.Tables[2].Rows[0]["TimeofBatchUpload"].ToString());
                Mailbody = Mailbody.Replace("#Countofrecordsuploaded", ds.Tables[3].Rows[0]["Total Record Uploaded"].ToString());
                Mailbody = Mailbody.Replace("#Countofsuccessfulrecords", ds.Tables[4].Rows[0]["Successfull Record"].ToString());
                Mailbody = Mailbody.Replace("#Countofrejectedrecords", ds.Tables[5].Rows[0]["Rejected Record"].ToString());

                fnSendMail(Mailbody);

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnSendBatchProcessEmail");
            }
        }

        private void fndeleteTempUploadData(string vUploadId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    { con.Open(); }
                    using (SqlCommand cmd = new SqlCommand("PROC_DELETE_TEMP_TBL_OUTBOUNND_DATA_UPLOAD", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vUploadID", vUploadId);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fndeleteTempUploadData");
                return;
            }
        }

        private void fnDeleteAllData(string vUploadId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (SqlCommand cmd = new SqlCommand("PROC_DELETE_OUTBOUND_DATA_ALL_TABLES", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vUploadID", vUploadId.ToString());
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnDeleteAllData");
                Alert.Show("Some error has occurred. Kindly try agaim. ");
                return;
            }
        }

        private void fnSaveErrorLog(clsTalismaIteraction c)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_INSERT_TBL_OUTBOUND_DATA_ERROR_TABLE", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vUploadID", c.uploadID);
                        cmd.Parameters.AddWithValue("@vInteractionID", "");
                        cmd.Parameters.AddWithValue("@vPolicyNo", c.PolicyNo);
                        cmd.Parameters.AddWithValue("@vProposalNo", c.ProposalNo);
                        cmd.Parameters.AddWithValue("@vPartnerApplicationNo", c.PartnerAppNo);
                        cmd.Parameters.AddWithValue("@vContactNo", c.ContactNo);
                        cmd.Parameters.AddWithValue("@vLineofbusiness", c.LOB);
                        cmd.Parameters.AddWithValue("@vCaseType", c.CaseType);
                        cmd.Parameters.AddWithValue("@vCalltype", c.CallType);
                        cmd.Parameters.AddWithValue("@vSubCalltype", c.SubCallType);
                        cmd.Parameters.AddWithValue("@vRTO_Reason", c.RTO_Reason);
                        cmd.Parameters.AddWithValue("@vContactName", c.ContactName);
                        cmd.Parameters.AddWithValue("@vFirstAssigned", c.FirstAssigned);
                        cmd.Parameters.AddWithValue("@vFCR_NONFCR", c.FCR_NONFCR);
                        string FollowUpDate = string.Empty;
                        if (!string.IsNullOrEmpty(c.FollowUPDay))
                        {
                            FollowUpDate = Convert.ToDateTime(c.FollowUPDay).ToString("dd/MM/yyyy");
                            cmd.Parameters.AddWithValue("@vFollowUPDay", FollowUpDate);
                        }
                        cmd.Parameters.AddWithValue("@vInteractionStatus", c.InteractionStatus);
                        cmd.Parameters.AddWithValue("@vErrorMessage", c.ErrorMessage);
                        cmd.Parameters.AddWithValue("@VcreatedBy", Session["vUserLoginId"].ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                fnDeleteAllData(hfUploadId.Value.ToString());
                ExceptionUtility.LogException(ex, "fnSaveErrorLog");
                return;
            }
        }

        private void fnCreateNewInteraction(clsTalismaIteraction c)
        {
            try
            {
                //int interactionID = 0;
                int interactionID = fnGenerateInteraction(c);

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_SAVE_TBL_OUTBOUND_DATA", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vUploadID", c.uploadID);
                        cmd.Parameters.AddWithValue("@vInteractionID", interactionID);
                        cmd.Parameters.AddWithValue("@vPolicyNo", c.PolicyNo);
                        cmd.Parameters.AddWithValue("@vProposalNo", c.ProposalNo);
                        cmd.Parameters.AddWithValue("@vPartnerApplicationNo", c.PartnerAppNo);
                        cmd.Parameters.AddWithValue("@vContactNo", c.ContactNo);
                        cmd.Parameters.AddWithValue("@vLineofbusiness", c.LOB);
                        cmd.Parameters.AddWithValue("@vCaseType", c.CaseType);
                        cmd.Parameters.AddWithValue("@vCalltype", c.CallType);
                        cmd.Parameters.AddWithValue("@vSubCalltype", c.SubCallType);
                        cmd.Parameters.AddWithValue("@vRTO_Reason", c.RTO_Reason);
                        cmd.Parameters.AddWithValue("@vContactName", c.ContactName);
                        cmd.Parameters.AddWithValue("@vFirstAssigned", c.FirstAssigned);
                        cmd.Parameters.AddWithValue("@vFCR_NONFCR", c.FCR_NONFCR);
                        string FollowUpDate = string.Empty;
                        if (!string.IsNullOrEmpty(c.FollowUPDay))
                        {
                            FollowUpDate = Convert.ToDateTime(c.FollowUPDay).ToString("dd/MM/yyyy");
                            cmd.Parameters.AddWithValue("@vFollowUPDay", FollowUpDate);
                        }
                        cmd.Parameters.AddWithValue("@vInteractionStatus", c.InteractionStatus);
                        cmd.Parameters.AddWithValue("@vErrorMessage", c.ErrorMessage);
                        cmd.Parameters.AddWithValue("@VcreatedBy", Session["vUserLoginId"].ToString());
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                fnDeleteAllData(hfUploadId.Value.ToString());
                ExceptionUtility.LogException(ex, "fnCreateNewInteraction");
                return;
            }
        }

        private int fnGenerateInteraction(clsTalismaIteraction c)
        {
            WebRequest.DefaultWebProxy = null;

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.XmlElement talismaSession = xmlDoc.CreateElement(TalismaSessionKey);
            UsernameToken objUserNameToken = new UsernameToken(userName, password, PasswordOption.SendPlainText);

            objUserNameToken.AnyElements.Add(talismaSession);

            PrjPASS.ContactWS.ContactWebService objContactWebService = null;
            PrjPASS.ContactWS.PropertyInfo[] objPropertyInfo = null;
            objContactWebService = new PrjPASS.ContactWS.ContactWebService();
            objContactWebService.RequestSoapContext.Security.Tokens.Add(objUserNameToken);
            objPropertyInfo = new PrjPASS.ContactWS.PropertyInfo[ContactiServiceArrayIndex];
            objContactWebService.Url = ContactiserviceURL;

            PrjPASS.InteractionWS.InteractionWebService objInteractionWebService = null;
            PrjPASS.InteractionWS.PropertyInfo[] objInteractionPropertyInfo = null;
            objInteractionWebService = new PrjPASS.InteractionWS.InteractionWebService();
            objInteractionWebService.RequestSoapContext.Security.Tokens.Add(objUserNameToken);
            objInteractionPropertyInfo = new PrjPASS.InteractionWS.PropertyInfo[12];
            objInteractionWebService.Url = InteractioniserviceURL;

            PrjPASS.InteractionWS.InteractionAttachmentData[] attach = new PrjPASS.InteractionWS.InteractionAttachmentData[0];
            int interactionID = 0;
            try
            {
                long returnValue1 = 0;
                long nContactID = 0;
                string phone = string.IsNullOrEmpty(c.ContactNo) ? "" : c.ContactNo;
                string Subject = string.Format("Outbound Upload {0}", c.PolicyNo);
                string userMsg = "";
                string contactMsg = "";
                long interactionId = 0;
                long eventId = 0;
                string error = string.Empty;
                bool contactPreviouslyUnblocked;
                int receivedByUSerID = 2;
                int MediaId = 8;
                int Direction = 1;
                int teamId = 4;
                int AssignedtoUserID = 1; // 2;
                int AliasID = 1;
                int Priority = 1;
                int Resolved = 1;
                bool UpdateReadOnly = true;
                bool MandatoryCheck = true;
                objPropertyInfo[0] = new PrjPASS.ContactWS.PropertyInfo();
                //comment By Nilesh
                //objPropertyInfo[0].propertyID = 57;
                //objPropertyInfo[0].propValue = "";
                //objPropertyInfo[0].rowID = -1;
                //objPropertyInfo[0].relJoinID = -1;
                //Comment End
                string szError;
                string USERNAME = c.ContactName;
                // Remove this code on live_ Start
                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
                // Remove this code on live_ End
                //Added By Nilesh
                if (!string.IsNullOrEmpty(c.ContactName) && (!string.IsNullOrEmpty(c.ContactNo)))
                {
                    objPropertyInfo[0].propertyID = 79;
                    objPropertyInfo[0].propValue = c.ContactNo;
                    objPropertyInfo[0].rowID = -1;
                    objPropertyInfo[0].relJoinID = -1;
                    //if (fnCheckContactExistsInTalisma(c.ContactName, Convert.ToInt64(c.ContactNo)))
                    //{
                    returnValue1 = objContactWebService.ResolveContact(false, objPropertyInfo, out nContactID, out szError);
                    //}
                    if (nContactID == -1)
                    {
                        returnValue1 = objContactWebService.CreateContact(USERNAME, objPropertyInfo, true, true, out nContactID, out szError);
                    }
                    File.AppendAllText(TalismaInteractionLogFile, System.Environment.NewLine);
                    File.AppendAllText(TalismaInteractionLogFile, string.Format("ResolveContact returnValue {0}", returnValue1));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", ContactID1 {0}", nContactID));
                }
                else
                {
                    if (fnCheckPolicyExistsInTalisma(c.PolicyNo))
                    {
                        DataTable dtPolicyNo = fnRetriveContactDetailsFromTalisma(c.PolicyNo);
                        string vContactName = dtPolicyNo.Rows[0]["ContactName"].ToString();
                        string vPhone = dtPolicyNo.Rows[0]["Phone"].ToString();
                        objPropertyInfo[0].propertyID = 79; //phone no
                        objPropertyInfo[0].propValue = vPhone;
                        objPropertyInfo[0].rowID = -1;
                        objPropertyInfo[0].relJoinID = -1;
                        //if (fnRetriveContactDetailsFromTalisma(c.ContactName, Convert.ToInt64(c.ContactNo)))
                        if (!string.IsNullOrEmpty(vPhone))
                        {
                            returnValue1 = objContactWebService.ResolveContact(false, objPropertyInfo, out nContactID, out szError);
                            if (nContactID == -1)
                            {
                                returnValue1 = objContactWebService.CreateContact(USERNAME, objPropertyInfo, true, true, out nContactID, out szError);
                            }
                            File.AppendAllText(TalismaInteractionLogFile, string.Format(",ContactID2  {0}", nContactID));
                        }
                        else
                        {
                            //if (nContactID == -1)
                            // {
                            returnValue1 = objContactWebService.CreateContact(USERNAME, objPropertyInfo, true, true, out nContactID, out szError);
                            if (nContactID == -1)
                            {
                                returnValue1 = objContactWebService.CreateContact(USERNAME, objPropertyInfo, true, true, out nContactID, out szError);
                            }
                            File.AppendAllText(TalismaInteractionLogFile, string.Format(",CreateContact returnValue3 {0}", returnValue1));
                            File.AppendAllText(TalismaInteractionLogFile, string.Format(",ContactID3  {0}", nContactID));
                            // }
                        }
                    }
                    else
                    {
                        c.ErrorMessage += " Policy not found.";
                    }
                }
                //End By Nilesh

                //Comment By Nilesh
                //returnValue1 = objContactWebService.ResolveContact(false, objPropertyInfo, out nContactID, out szError);
                //if (nContactID == -1)
                //{
                //    returnValue1 = objContactWebService.CreateContact(USERNAME, objPropertyInfo, true, true, out nContactID, out szError);

                //}
                //End By Nilesh

                File.AppendAllText(TalismaInteractionLogFile, System.Environment.NewLine);
                File.AppendAllText(TalismaInteractionLogFile, "Creating Interaction " + DateTime.Now.ToString() + " " + System.Environment.NewLine);
                File.AppendAllText(TalismaInteractionLogFile, string.Format(" Policy No {0}", c.PolicyNo));

                int i = 0;

                //Added By Nilesh
                if (!string.IsNullOrEmpty(c.PolicyNoPropID))
                {
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = Convert.ToInt64(c.PolicyNoPropID);
                    //objInteractionPropertyInfo[i].propValue = c.LOBPropValue;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    //File.AppendAllText(TalismaInteractionLogFile, string.Format(", LOB {0}", c.LOB));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", PolicyNo Prop ID {0}", c.PolicyNoPropID));
                    //File.AppendAllText(TalismaInteractionLogFile, string.Format(", LOB Prop ID {0}", c.LOBPropValue));
                }
                //End By Nilesh
                //  Setting LOB and LOB Case Type
                if (!string.IsNullOrEmpty(c.LOBPropValue))
                {
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = Convert.ToInt64(c.LOBPropID);
                    objInteractionPropertyInfo[i].propValue = c.LOBPropValue;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", LOB {0}", c.LOB));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", LOB Prop ID {0}", c.LOBPropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", LOB Prop ID {0}", c.LOBPropValue));
                }
                // 

                if (!string.IsNullOrEmpty(c.CaseTypePropID))
                {
                    i++;
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = Convert.ToInt64(c.CaseTypePropID);
                    objInteractionPropertyInfo[i].propValue = c.CaseTypePropValue;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Case Type {0}", c.CaseType));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Case Type Prop ID  {0}", c.CaseTypePropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Case Type Prop Value {0}", c.CaseTypePropValue));
                }



                if (!string.IsNullOrEmpty(c.CallTypePropID))
                {
                    i++;
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = Convert.ToInt64(c.CallTypePropID);
                    objInteractionPropertyInfo[i].propValue = c.CallTypePropValue;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Call Type {0}", c.CallType));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Call Type PropID {0}", c.CallTypePropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Call Type PropValue {0}", c.CallTypePropValue));
                }


                if (!string.IsNullOrEmpty(c.SubCallTypePropID))
                {
                    i++;
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = Convert.ToInt64(c.SubCallTypePropID); //SubCalltype Property ID
                    objInteractionPropertyInfo[i].propValue = c.SubCallTypePropValue;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Sub Call Type {0}", c.SubCallType));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Sub Call Type PropID {0}", c.SubCallTypePropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Sub Call Type PropValue {0}", c.SubCallTypePropValue));
                }


                if (!string.IsNullOrEmpty(c.RTO_Reason))
                {
                    i++;
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = Convert.ToInt64(c.RTO_ReasonPropID); //SubCalltype Property ID
                    objInteractionPropertyInfo[i].propValue = c.RTO_ReasonPropValue;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", RTO Reason {0}", c.RTO_Reason));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", RTO Reason PropID {0}", c.RTO_ReasonPropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", RTO Reason PropValue {0}", c.RTO_ReasonPropValue));
                }




                if (!string.IsNullOrEmpty(c.PolicyNo))
                {
                    i++;
                    long PolicyNoPropId = fnGetPolicyNoPropertyPropID();
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = PolicyNoPropId; //
                    objInteractionPropertyInfo[i].propValue = c.PolicyNo; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;

                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Policy No Prop ID {0}", PolicyNoPropId));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Policy No Prop Value {0}", c.PolicyNo));
                }

                if (!string.IsNullOrEmpty(c.FCRPropValue))
                {
                    i++;
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = Convert.ToInt64(c.FCRPropID); //   FCR Property ID
                    objInteractionPropertyInfo[i].propValue = c.FCRPropValue; //      FCR Property Value
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", FCR_NONFCR {0}", c.FCR_NONFCR));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", FCRPropID {0}", c.FCRPropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", FCR Prop Value {0}", c.FCRPropValue));
                }


                if (!string.IsNullOrEmpty(c.FirstAssignedPropValue))
                {
                    i++;
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = Convert.ToInt64(c.FirstAssignedPropID); //  Assigned to Prop value
                    objInteractionPropertyInfo[i].propValue = c.FirstAssignedPropValue;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", First Assigned {0}", c.FirstAssigned));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", First Assigned Prop ID {0}", c.FirstAssignedPropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", First Assigned Prop value {0}", c.FirstAssignedPropValue));
                }


                if (!string.IsNullOrEmpty(c.FollowUPDay))
                {
                    i++;
                    long FollowUpdayPropID = fnGetFollowUpDayPropValue();
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = FollowUpdayPropID;
                    objInteractionPropertyInfo[i].propValue = c.FollowUPDay;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Follow Up Day Prop ID {0}", FollowUpdayPropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Follow Up Day Prop Value {0}", c.FollowUPDay));
                }


                if (!string.IsNullOrEmpty(c.PartnerAppNo))
                {
                    i++;
                    long PartnerApplicationNoPropID = fnGetPartnerAppNoPropID();
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = PartnerApplicationNoPropID;
                    objInteractionPropertyInfo[i].propValue = c.PartnerAppNo;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", PartnerApplication No Prop ID {0}", PartnerApplicationNoPropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", PartnerApplication Prop Value {0}", c.PartnerAppNo));
                }



                if (!string.IsNullOrEmpty(c.ProposalNo))
                {
                    i++;
                    long ProposalNoPropID = fnGetProposalNoPropID();
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = ProposalNoPropID;
                    objInteractionPropertyInfo[i].propValue = c.ProposalNo;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Proposal No Prop ID {0}", ProposalNoPropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", Proposal No Prop Value {0}", c.ProposalNo));
                }

                if (!string.IsNullOrEmpty(c.ContactName))
                {
                    i++;
                    long ContactNamePropID = fnGetContactNamePropID();
                    objInteractionPropertyInfo[i] = new PrjPASS.InteractionWS.PropertyInfo();
                    objInteractionPropertyInfo[i].propertyID = ContactNamePropID;
                    objInteractionPropertyInfo[i].propValue = c.ContactName;
                    objInteractionPropertyInfo[i].rowID = -1;
                    objInteractionPropertyInfo[i].relJoinID = -1;
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", ContactName Prop ID {0}", ContactNamePropID));
                    File.AppendAllText(TalismaInteractionLogFile, string.Format(", ContactName Prop Value {0}", c.ContactName));
                }


                returnValue1 = objInteractionWebService.CreateInteraction(nContactID, phone, DateTime.Now, receivedByUSerID, MediaId,
                                                          Direction, Subject, teamId, AssignedtoUserID, AliasID, Priority, Resolved, contactMsg, userMsg, attach,
                                                          objInteractionPropertyInfo, UpdateReadOnly, MandatoryCheck, out interactionId, out eventId, out error,
                                                          out contactPreviouslyUnblocked);


                interactionID = Convert.ToInt32(interactionId);
                File.AppendAllText(TalismaInteractionLogFile, string.Format(", Interaction ID {0}", interactionID));
                File.AppendAllText(TalismaInteractionLogFile, System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "CreateInteraction");
            }
            return interactionID;
        }

        private long fnGetContactNamePropID()
        {
            long ContactNamePropertyID = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_CONTACTNAME_PROPID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {
                                ContactNamePropertyID = Convert.ToInt64(dr[0].ToString());
                            }
                        }
                    }
                }
                return ContactNamePropertyID;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnGetContactNamePropID");
            }
            return ContactNamePropertyID;
        }

        private long fnGetProposalNoPropID()
        {
            long ProposalNoPropertyID = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_PROPOSAL_NO_PROPID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {
                                ProposalNoPropertyID = Convert.ToInt64(dr[0].ToString());
                            }
                        }
                    }
                }
                return ProposalNoPropertyID;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnGetProposalNoPropID");
            }
            return ProposalNoPropertyID;
        }

        private long fnGetPartnerAppNoPropID()
        {
            long PartnerAppNoPropertyID = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_PARTNER_APP_NO_PROPID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {
                                PartnerAppNoPropertyID = Convert.ToInt64(dr[0].ToString());
                            }
                        }
                    }
                }
                return PartnerAppNoPropertyID;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnGetPartnerAppNoPropID");
            }
            return PartnerAppNoPropertyID;
        }

        private long fnGetFollowUpDayPropValue()
        {
            long FollowUpDayPropertyID = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_FOLLOWUP_DAY_PROPID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {
                                FollowUpDayPropertyID = Convert.ToInt64(dr[0].ToString());
                            }
                        }
                    }
                }
                return FollowUpDayPropertyID;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnGetFollowUpDayPropValue");
            }
            return FollowUpDayPropertyID;
        }

        private long fnGetPolicyNoPropertyPropID()
        {
            long PolicyNoPropertyID = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_POLICY_NO_PROPID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {
                                PolicyNoPropertyID = Convert.ToInt64(dr[0].ToString());
                            }
                        }
                    }
                }
                return PolicyNoPropertyID;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnGetPropertyPropValue");
            }
            return PolicyNoPropertyID;
        }

        private DataTable fnGetValidatedData(string vUploadId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_TBL_OUTBOUNND_VALIDATED_DATA_UPLOAD", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vUploadID", vUploadId.ToString());
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                    }


                }
            }
            catch (Exception ex)
            {
                fnDeleteAllData(hfUploadId.Value.ToString());
                ExceptionUtility.LogException(ex, "fnGetValidatedData ");
                Alert.Show("Some Error Occured.");
                return null;
            }

            return dt;
        }


        private DataTable fnGetValidatedDataWithoutPolicyNo(string vUploadId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_TBL_OUTBOUNND_VALIDATED_DATA_UPLOAD_WITHOUT_POLICY", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vUploadID", vUploadId.ToString());
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                    }


                }
            }
            catch (Exception ex)
            {
                fnDeleteAllData(hfUploadId.Value.ToString());
                ExceptionUtility.LogException(ex, "fnGetValidatedData ");
                Alert.Show("Some Error Occured.");
                return null;
            }

            return dt;
        }

        private bool fnValidateExcelData(DataTable dtExcelData, out string excelValidationMesage)
        {
            excelValidationMesage = "";
            bool b = false;
            int i = 0;
            string InteractionID, PolicyNo, ProposalNo, PartnerAppNo,
            ContactNo, LOB, CaseType, CallType, SubCallType, RTO_Reason, ContactName,
              FirstAssigned, FCR_NONFCR, FollowUPDay, InteractionStatus;

            try
            {
                foreach (DataRow dr in dtExcelData.Rows)
                {
                    i++;
                    InteractionID = dr["InteractionID"].ToString();
                    PolicyNo = dr["PolicyNo"].ToString();
                    ProposalNo = dr["ProposalNo"].ToString();
                    PartnerAppNo = dr["PartnerApplicationNo"].ToString();
                    ContactNo = dr["ContactNo"].ToString();
                    LOB = dr["Lineofbusiness"].ToString();
                    CaseType = dr["CaseType"].ToString();
                    CallType = dr["CallType"].ToString();
                    SubCallType = dr["SubCallType"].ToString();
                    RTO_Reason = dr["RTO_Reason"].ToString();
                    ContactName = dr["ContactName"].ToString();
                    FirstAssigned = dr["FirstAssigned"].ToString();
                    FCR_NONFCR = dr["FCR/NONFCR"].ToString();
                    FollowUPDay = dr["FollowUPDay"].ToString();
                    InteractionStatus = dr["InteractionStatus"].ToString();
                    b = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnValidateExcelData");
                Alert.Show(excelValidationMesage + " " + ex.Message.ToString());
                b = false;
            }
            return b;
        }

        private long BulkCopyToDatabase(DataTable dtExcelData, SqlConnection con)
        {
            var filesInserted = 0L;
            try
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {

                    try
                    {
                        sqlBulkCopy.DestinationTableName = "dbo.TEMP_TBL_OUTBOUNND_DATA_UPLOAD";

                        // Column Mappping for SQLBulkCopy
                        sqlBulkCopy.ColumnMappings.Add("vUploadID", "vUploadID");
                        sqlBulkCopy.ColumnMappings.Add("InteractionID", "vInteractionID");
                        sqlBulkCopy.ColumnMappings.Add("PolicyNo", "vPolicyNo");
                        sqlBulkCopy.ColumnMappings.Add("ProposalNo", "vProposalNo");
                        sqlBulkCopy.ColumnMappings.Add("PartnerApplicationNo", "vPartnerApplicationNo");
                        sqlBulkCopy.ColumnMappings.Add("ContactNo", "vContactNo");
                        sqlBulkCopy.ColumnMappings.Add("Lineofbusiness", "vLineofbusiness");
                        sqlBulkCopy.ColumnMappings.Add("CaseType", "vCaseType");
                        sqlBulkCopy.ColumnMappings.Add("CallType", "vCalltype");
                        sqlBulkCopy.ColumnMappings.Add("SubCallType", "vSubCalltype");
                        sqlBulkCopy.ColumnMappings.Add("ContactName", "vContactName");
                        sqlBulkCopy.ColumnMappings.Add("FirstAssigned", "vFirstAssigned");
                        sqlBulkCopy.ColumnMappings.Add("FCR/NONFCR", "vFCR/NONFCR");
                        sqlBulkCopy.ColumnMappings.Add("FollowUPDay", "vFollowUPDay");
                        sqlBulkCopy.ColumnMappings.Add("InteractionStatus", "vInteractionStatus");
                        sqlBulkCopy.ColumnMappings.Add("RTO_Reason", "vRTO_Reason");

                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        sqlBulkCopy.NotifyAfter = dtExcelData.Rows.Count;
                        sqlBulkCopy.SqlRowsCopied += (s, e) => filesInserted = e.RowsCopied;
                        sqlBulkCopy.WriteToServer(dtExcelData);

                        con.Close();
                    }
                    catch (Exception ex)
                    {

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
                            Alert.Show(String.Format("Error Occured : Column {0} contains data with a length greater than {1}", column, length), "FrmCPRenewalUpload.aspx");
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(TalismaInteractionLogFile_Error, "FrmOutBoundUpload Error occured in Function BulkCopyToDatabase and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                //ExceptionUtility.LogException(ex, "Error in BulkCopyToDatabase on FrmOutBoundUpload.aspx Page");
                Alert.Show("Some Error Occured. Please check the file format of uploaded file and all columns are available and date in dd-MMM-yy format. Detail Exception " + ex.ToString(), "FrmCPRenewalUpload.aspx");
            }

            return filesInserted;

        }







        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Create the DataAdapter & DataSet
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                // Return the dataset
                return ds;
            }
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            //Set the command time out to 15 minutes.
            command.CommandTimeout = 900;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string strFilepath = AppDomain.CurrentDomain.BaseDirectory + "//" + ConfigurationManager.AppSettings["OutBoundUploadTemplatePath"].ToString();
            //string strFileName = Server.MapPath(strFilepath);
            DownloadSampleFile(strFilepath);
        }

        private bool DownloadSampleFile(string strfilename)
        {
            try
            {
                string path = strfilename;
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(file.FullName);
                    Response.End();

                }
                else
                {
                    Alert.Show("File not available. Kindly contact admin. FilePath = " + path.ToString());
                }
            }
            catch (Exception ex)
            {
                Alert.Show(" ErrorMessage " + ex.ToString());
                ExceptionUtility.LogException(ex, "DownloadSampleFile");
                return false;
            }

            return true;
        }



        public void fnSendMail(string MailBody)
        {
            try
            {
                string strPath = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["OB_smtp_Host"].ToString();
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["OB_smtp_Username"].ToString(), ConfigurationManager.AppSettings["OB_smtp_Password"].ToString());

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["OB_smtp_FromMailId"].ToString());
                mm.Subject = ConfigurationManager.AppSettings["OB_mail_subject"].ToString();
                mm.Body = MailBody;
                mm.IsBodyHtml = true;
                string ToEmail = ConfigurationManager.AppSettings["OB_smtp_mail_ToMailId"].ToString();
                string[] Email = ToEmail.Split(';');
                foreach (string email in Email)
                {
                    mm.To.Add(email);
                }

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                smtpClient.Send(mm);

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnSendMail");
            }

        }


    }
}