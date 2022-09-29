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
using System.Globalization;
using System.Drawing;

namespace PrjPASS
{
    public partial class FrmHealthClaimsUpload : System.Web.UI.Page
    {
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
            int validRecords = 0;
            int inValidRecords = 0;
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cYearMonth = "", vUploadId = "";
            string vKGIClaimNumber = "";
            SqlConnection _con = new SqlConnection(dbCOMMON.ConnectionString);
            SqlDataReader dr = null;
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();

            cYearMonth = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                cYearMonth = cYearMonth + "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                cYearMonth = cYearMonth + DateTime.Now.Month.ToString();
            }

            vUploadId = wsDocNo.fn_Gen_Doc_Master_No("HCUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);

            _tran.Commit();

            try
            {
                string LoggedInUserName = Session["vUserLoginId"].ToString().ToUpper();

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
                    excel_con.Open();

                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString(); //""Claims Data$"";
                    DataTable dtExcelData = new DataTable();
                    bool GetMappingData = false;
                    string sqlCommand = "SELECT  * FROM TBL_HEALTH_CLAIMS_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'";
                    DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                    DataSet ds = null;

                    ds = dbCOMMON.ExecuteDataSet(dbCommand);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GetMappingData = true;
                    }
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }

                    excel_con.Close();

                    if (dtExcelData.Rows.Count > 0)
                    {
                        for (int i = 1; i <= dtExcelData.Columns.Count; i++)
                        {
                            dtExcelData.Columns[i - 1].ColumnName = dtExcelData.Columns[i - 1].ColumnName.Trim();
                        }

                        dtExcelData.Columns.Add("vErrorFlag");
                        dtExcelData.Columns.Add("vErrorDesc");
                        dtExcelData.Columns.Add("vTransType");
                        dtExcelData.Columns.Add("vCreatedBy");
                        dtExcelData.Columns.Add("vUploadId");

                        long lastClaimNumber = 0;
                        foreach (DataRow excelrow in dtExcelData.Rows)
                        {
                            string vDestinationFieldName = "";
                            string vSourceFieldName = "";
                            string vFieldValue = "";
                            string vErrorDesc = "";
                            string vClaimStage = "";
                            string vClaimStatus = "";
                            string vClaimType = "";
                            string vDisallowedAmount = "0";
                            string bMandatoryForRegistration = "";
                            string bMandatoryForSettlement = "";
                            string bMandatoryForPayment = "";
                            string bMandatoryForReopen = "";
                            string[] chkValidFlag;
                            string bIsKGIClaimNumberUploadedByUser = "1";

                            if (GetMappingData == true)
                            {
                                bool insertflag = true;

                                for (int i = 1; i <= dtExcelData.Columns.Count; i++)
                                {
                                    string searchExpression = "vSourceColumnName = '" + dtExcelData.Columns[i - 1].ColumnName + "'";

                                    DataRow[] foundRows = ds.Tables[0].Select(searchExpression);

                                    if (foundRows.Count() > 0)
                                    {
                                        vDestinationFieldName = foundRows[0]["vDestinationColumnName"].ToString();
                                        vSourceFieldName = foundRows[0]["vSourceColumnName"].ToString();

                                        vFieldValue = excelrow[dtExcelData.Columns[i - 1].ColumnName].ToString().Trim();
                                        excelrow.SetField<string>(dtExcelData.Columns[i - 1].ColumnName, vFieldValue);

                                        bMandatoryForRegistration = foundRows[0]["bMandatoryForRegistration"].ToString();
                                        bMandatoryForSettlement = foundRows[0]["bMandatoryForSettlement"].ToString();
                                        bMandatoryForPayment = foundRows[0]["bMandatoryForPayment"].ToString();
                                        bMandatoryForReopen = foundRows[0]["bMandatoryForReopen"].ToString();

                                        vClaimStage = excelrow["Claim Stage"] == DBNull.Value || excelrow["Claim Stage"] == null ? "" : excelrow["Claim Stage"].ToString().Trim().ToLower();
                                        vClaimStatus = excelrow["Claim Status"] == DBNull.Value || excelrow["Claim Status"] == null ? "" : excelrow["Claim Status"].ToString().Trim().ToLower();
                                        vClaimType = excelrow["Claim Type"] == DBNull.Value || excelrow["Claim Type"] == null ? "" : excelrow["Claim Type"].ToString().Trim().ToLower();
                                        vDisallowedAmount = excelrow["Disallowed amount"] == DBNull.Value || excelrow["Disallowed amount"] == null ? "0" : excelrow["Disallowed amount"].ToString().Trim();

                                        #region Checking if Mandatory Fields are entered or not
                                        if (vDestinationFieldName == "vClaimStage" && string.IsNullOrWhiteSpace(vClaimStage))
                                        {
                                            string[] ckvalidflag = new string[2];
                                            ckvalidflag[0] = "false";
                                            ckvalidflag[1] = vSourceFieldName + " is Mandatory";
                                            insertflag = false;
                                            vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                        }
                                        else if (
                                            (
                                                (vClaimStage == "registration" && bMandatoryForRegistration == "Y")
                                                || (vClaimStage == "settlement" && bMandatoryForSettlement == "Y")
                                                || (vClaimStage == "payment" && bMandatoryForPayment == "Y")
                                                || (vClaimStage == "reopen" && bMandatoryForReopen == "Y")
                                            )
                                                && string.IsNullOrWhiteSpace(vFieldValue)
                                          )
                                        {
                                            string[] ckvalidflag = new string[2];
                                            ckvalidflag[0] = "false";
                                            ckvalidflag[1] = vSourceFieldName + " is Mandatory when Claim Stage is " + vClaimStage;
                                            insertflag = false;
                                            vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                        }
                                        #endregion

                                        #region Checking if Conditional Mandatory Fields are entered or not
                                        switch (vDestinationFieldName)
                                        {
                                            case "vAuthorisationNumber":
                                            case "vPreAuthDiscription":
                                            case "vPreAuthDate":
                                            case "vPreAuthTime":
                                            case "vPreAuthAmount":
                                            case "vBufferAmount":
                                            case "vPreAuthRemarks":
                                                if (vClaimStage == "settlement" && vClaimType == "cashless" && string.IsNullOrWhiteSpace(vFieldValue))
                                                {
                                                    string[] ckvalidflag = new string[2];
                                                    ckvalidflag[0] = "false";
                                                    ckvalidflag[1] = vSourceFieldName + " cannot be blank when Claim Stage is Settlement and Claim Type is Cashless";
                                                    insertflag = false;
                                                    vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                }
                                                break;

                                            case "vHospitalCode":
                                            case "vHospitalName":
                                            case "vHospitalAddressLine1":
                                            case "vHospitalAddressLandmark":
                                            case "vHospitalCity":
                                            case "vHospitalState":
                                            case "vHospitalPincode":
                                            case "vHealthCareProviderType":
                                            case "vHealthCareProviderCategory":
                                            case "vIsNetwork":
                                                if (vClaimStage == "payment" && vClaimType == "cashless" && string.IsNullOrWhiteSpace(vFieldValue))
                                                {
                                                    string[] ckvalidflag = new string[2];
                                                    ckvalidflag[0] = "false";
                                                    ckvalidflag[1] = vSourceFieldName + " cannot be blank when Claim Stage is Payment and Claim Type is Cashless";
                                                    insertflag = false;
                                                    vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                }
                                                break;

                                            case "vDisallowedReason":
                                                double dDisallowedAmount = 0;
                                                bool isDoubleNumber = double.TryParse(vDisallowedAmount, out dDisallowedAmount);
                                                if (!isDoubleNumber)
                                                {
                                                    string[] ckvalidflag = new string[2];
                                                    ckvalidflag[0] = "false";
                                                    ckvalidflag[1] = "Disallowed amount should be numeric (10,2) e.g. 1234 or 1234.66";
                                                    insertflag = false;
                                                    vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                }
                                                else if (dDisallowedAmount > 0 && string.IsNullOrWhiteSpace(vFieldValue))
                                                {
                                                    string[] ckvalidflag = new string[2];
                                                    ckvalidflag[0] = "false";
                                                    ckvalidflag[1] = vSourceFieldName + " cannot be blank when Disallowed amount is greater than 0(ZERO)";
                                                    insertflag = false;
                                                    vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                }
                                                break;

                                            case "vQueryClosureRejectionReason":
                                            case "vQueryClosureRejectionDescription":
                                                if (vClaimStage == "settlement" && vClaimStatus == "rejected" && string.IsNullOrWhiteSpace(vFieldValue))
                                                {
                                                    string[] ckvalidflag = new string[2];
                                                    ckvalidflag[0] = "false";
                                                    ckvalidflag[1] = vSourceFieldName + " cannot be blank when Claim Stage is Settlement and Claim Status is Rejected";
                                                    insertflag = false;
                                                    vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                }
                                                break;

                                            case "vKGIClaimNumber":
                                            case "vKGIChildClaimNumber":
                                                if (vClaimStage == "registration" && vClaimStatus == "open" && !string.IsNullOrWhiteSpace(vFieldValue))
                                                {
                                                    string[] ckvalidflag = new string[2];
                                                    ckvalidflag[0] = "false";
                                                    ckvalidflag[1] = vSourceFieldName + " should be blank when Claim Stage is Registration and Claim Status is Open";
                                                    insertflag = false;
                                                    vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                }
                                                break;
                                        }
                                        #endregion

                                        #region Checking if Claim Status is valid or not for entered Claim Stage
                                        if (vDestinationFieldName == "vClaimStage" && !string.IsNullOrWhiteSpace(vClaimStage))
                                        {
                                            if (vClaimStage == "registration" && vClaimStatus != "open" && vClaimStatus != "under process" && vClaimStatus != "deficiency")
                                            {
                                                string[] ckvalidflag = new string[2];
                                                ckvalidflag[0] = "false";
                                                ckvalidflag[1] = vSourceFieldName + " when Registration then Claim Status must be one of the Open/Under Process/Deficiency";
                                                insertflag = false;
                                                vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                            }
                                            else if (vClaimStage == "settlement" && vClaimStatus != "rejected")
                                            {
                                                string[] ckvalidflag = new string[2];
                                                ckvalidflag[0] = "false";
                                                ckvalidflag[1] = vSourceFieldName + " when Settlement then Claim Status must be Rejected";
                                                insertflag = false;
                                                vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                            }
                                            else if (vClaimStage == "payment" && vClaimStatus != "approved")
                                            {
                                                string[] ckvalidflag = new string[2];
                                                ckvalidflag[0] = "false";
                                                ckvalidflag[1] = vSourceFieldName + " when Payment then Claim Status must be Approved";
                                                insertflag = false;
                                                vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                            }
                                            if (vClaimStage == "reopen" && vClaimStatus != "reopen" && vClaimStatus != "reopen under process" && vClaimStatus != "reopen rejected" && vClaimStatus != "reopen approved")
                                            {
                                                string[] ckvalidflag = new string[2];
                                                ckvalidflag[0] = "false";
                                                ckvalidflag[1] = vSourceFieldName + " when Reopen then Claim Status must be one of the Reopen/Reopen Under Process/Reopen Rejected/Reopen Approved";
                                                insertflag = false;
                                                vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                            }
                                        }

                                        #endregion

                                        #region Duplication Check and Generating Claim Number When Stage-Registration & Status-Open and Claim number is blank
                                        //Generate Claim Number if vClaimStage is "registration" and vClaimStatus is "Open" and Claim Number is blank
                                        //If vClaimStage is "registration" and vClaimStatus is "Open" and Claim Number is not blank, checking if same already exists
                                        if (insertflag && vDestinationFieldName == "vKGIClaimNumber" && vClaimStage == "registration" && vClaimStatus == "open")
                                        {
                                            try
                                            {
                                                #region Duplication Check on the basis of Claim Number, Registration, Open within the uploaded sheet
                                                if (!string.IsNullOrWhiteSpace(vFieldValue))
                                                {
                                                    searchExpression = "[KGI Claim Number]='" + vFieldValue + "' AND [Claim Stage]='" + vClaimStage + "' AND [Claim Status]='" + vClaimStatus + "'";

                                                    foundRows = dtExcelData.Select(searchExpression);

                                                    if (foundRows.Length > 1)
                                                    {
                                                        string[] ckvalidflag = new string[2];
                                                        ckvalidflag[0] = "false";
                                                        ckvalidflag[1] = "KGI Claim Number - " + vFieldValue + ", Claim Stage - Registration, Claim Status - Open is repeated in the uploaded excel sheet";
                                                        insertflag = false;
                                                        vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                    }
                                                }
                                                #endregion

                                                #region Duplication Check on the basis of Policy Number, Member ID, Coverage Description,Parent Coverage Description, Admission Date
                                                if (insertflag)
                                                {
                                                    string vPolicyNumber = excelrow["Policy Number"] == DBNull.Value || excelrow["Policy Number"] == null ? "" : excelrow["Policy Number"].ToString().Trim();
                                                    string vMemberID = excelrow["Member ID"] == DBNull.Value || excelrow["Member ID"] == null ? "" : excelrow["Member ID"].ToString().Trim();
                                                    string vCoverageDescription = excelrow["Coverage Description"] == DBNull.Value || excelrow["Coverage Description"] == null ? "" : excelrow["Coverage Description"].ToString().Trim();
                                                    string vParentCoverageDescription = excelrow["Parent Coverage Description"] == DBNull.Value || excelrow["Parent Coverage Description"] == null ? "" : excelrow["Parent Coverage Description"].ToString().Trim();
                                                    string vDateOfAdmission = excelrow["Date of admission"] == DBNull.Value || excelrow["Date of admission"] == null ? "" : excelrow["Date of admission"].ToString().Trim();

                                                    //Checking if this combination exists in uploaded excel sheet(dtExcelData)

                                                    searchExpression = "[Policy Number]='" + vPolicyNumber + "' AND [Member ID]='" + vMemberID + "' AND [Coverage Description]='" + vCoverageDescription + "' AND [Parent Coverage Description]='" + vParentCoverageDescription + "' AND [Date of admission]='" + vDateOfAdmission + "' AND [Claim Stage]='Registration' AND [Claim Status]='Open'";

                                                    foundRows = dtExcelData.Select(searchExpression);

                                                    if (foundRows.Length > 1)
                                                    {
                                                        string[] ckvalidflag = new string[2];
                                                        ckvalidflag[0] = "false";
                                                        ckvalidflag[1] = "Policy Number - " + vPolicyNumber + ", Member ID - " + vMemberID + ", Coverage Description - " + vCoverageDescription + ", Parent Coverage Description - " + vParentCoverageDescription + ", Date of admission - " + vDateOfAdmission + " combination is repeated in uploaded excel sheet with Stage - Registration and Status - Open";
                                                        insertflag = false;
                                                        vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                    }
                                                    if (insertflag)
                                                    {
                                                        //Checking if this combination exists in TBL_HEALTH_CLAIMS_DATA
                                                        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM TBL_HEALTH_CLAIMS_DATA WHERE vClaimStage=@vClaimStage AND vClaimStatus=@vClaimStatus AND vPolicyNumber=@vPolicyNumber AND vMemberID=@vMemberID AND vCoverageDescription=@vCoverageDescription AND vParentCoverageDescription=@vParentCoverageDescription AND vDateOfAdmission=@vDateOfAdmission", _con);

                                                        cmd.Parameters.Add(new SqlParameter("@vClaimStage", "Registration"));
                                                        cmd.Parameters.Add(new SqlParameter("@vClaimStatus", "Open"));
                                                        cmd.Parameters.Add(new SqlParameter("@vPolicyNumber", vPolicyNumber));
                                                        cmd.Parameters.Add(new SqlParameter("@vMemberID", vMemberID));
                                                        cmd.Parameters.Add(new SqlParameter("@vCoverageDescription", vCoverageDescription));
                                                        cmd.Parameters.Add(new SqlParameter("@vParentCoverageDescription", vParentCoverageDescription));
                                                        cmd.Parameters.Add(new SqlParameter("@vDateOfAdmission", vDateOfAdmission));

                                                        int recordsFound = Convert.ToInt32(cmd.ExecuteScalar());
                                                        if (recordsFound > 0)
                                                        {
                                                            string[] ckvalidflag = new string[2];
                                                            ckvalidflag[0] = "false";
                                                            ckvalidflag[1] = "Policy Number - " + vPolicyNumber + ", Member ID - " + vMemberID + ", Coverage Description - " + vCoverageDescription + ", Parent Coverage Description - " + vParentCoverageDescription + ", Date of admission - " + vDateOfAdmission + " combination already exists in Table with Stage - Registration and Status - Open";
                                                            insertflag = false;
                                                            vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region Duplication Check on the basis of Claim Number, Registration, Open within the Table. Generate KGI Claim Number if blank in uploaded sheet
                                                if (insertflag)
                                                {
                                                    if (lastClaimNumber == 0 && string.IsNullOrWhiteSpace(vFieldValue))
                                                    {
                                                        SqlCommand cmd = new SqlCommand("SELECT IIF(ISNULL(MAX(CONVERT(BIGINT, vKGIClaimNumber)), 0) = 0, 1000000, MAX(CONVERT(BIGINT, vKGIClaimNumber) + 1)) FROM TBL_HEALTH_CLAIMS_DATA WITH(NOLOCK)", _con);
                                                        lastClaimNumber = Convert.ToInt64(cmd.ExecuteScalar());
                                                    }
                                                    //Adding KGI Claim Number whereever it is blank uploaded by user
                                                    if (string.IsNullOrWhiteSpace(vFieldValue))
                                                    {
                                                        excelrow[dtExcelData.Columns[i - 1].ColumnName] = lastClaimNumber;
                                                        lastClaimNumber++;
                                                        bIsKGIClaimNumberUploadedByUser = "0";
                                                    }
                                                    else
                                                    {
                                                        SqlCommand cmd = new SqlCommand("SELECT TOP 1 vClaimStatus FROM TBL_HEALTH_CLAIMS_DATA WHERE vKGIClaimNumber=@vKGIClaimNumber ORDER BY dCreatedDate DESC", _con);
                                                        cmd.Parameters.Add(new SqlParameter("@vKGIClaimNumber", vFieldValue));
                                                        dr = cmd.ExecuteReader();
                                                        if (dr.Read())
                                                        {
                                                            string[] ckvalidflag = new string[2];
                                                            ckvalidflag[0] = "false";
                                                            ckvalidflag[1] = vSourceFieldName + "(" + vFieldValue + ") already exist in Table in Status - " + dr["vClaimStatus"].ToString();
                                                            insertflag = false;
                                                            vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                        }
                                                        dr.Close();
                                                    }
                                                }
                                                #endregion
                                            }
                                            catch (Exception ex)
                                            {
                                                ExceptionUtility.LogException(ex, "FrmHealthClaimsUpload.aspx.cs - Upload()");
                                                if (_tran.Connection != null)
                                                    _tran.Rollback();

                                                if (_con.State == ConnectionState.Open)
                                                    _con.Close();
                                                Session["ErrorCallingPage"] = "FrmHealthClaimsUpload.aspx";
                                                string vStatusMsg = "No Records for Upload or " + ex.Message.ToString().Replace("\r\n", "");
                                                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                                                return;
                                            }
                                        }

                                        #endregion

                                        #region Claim Stage - Claim Status Validations When Claim Status is not Open
                                        if (vDestinationFieldName == "vClaimStatus" && insertflag && vClaimStatus != "open")
                                        {
                                            #region Checking if KGI Claim Number is present or not for NON open Status
                                            string ClaimNumber = excelrow["KGI Claim Number"].ToString().Trim();
                                            if (string.IsNullOrWhiteSpace(ClaimNumber))
                                            {
                                                string[] ckvalidflag = new string[2];
                                                ckvalidflag[0] = "false";
                                                ckvalidflag[1] = vSourceFieldName + " - " + vFieldValue + " is not allowed as KGI Claim Number is blank";
                                                insertflag = false;
                                                vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                            }
                                            #endregion

                                            #region Validations based on Last Claim status for Claim Number
                                            else
                                            {
                                                try
                                                {
                                                    #region Checking if there is atleast one record present in open status for Claim Number
                                                    SqlCommand cmd = new SqlCommand("SELECT 1 FROM TBL_HEALTH_CLAIMS_DATA WITH(NOLOCK) WHERE vKGIClaimNumber=@vKGIClaimNumber AND vClaimStatus='open'", _con);
                                                    cmd.Parameters.Add(new SqlParameter("@vKGIClaimNumber", ClaimNumber));
                                                    dr = cmd.ExecuteReader();
                                                    if (!dr.Read())
                                                    {
                                                        string[] ckvalidflag = new string[2];
                                                        ckvalidflag[0] = "false";
                                                        ckvalidflag[1] = vSourceFieldName + " - " + vFieldValue + " is not allowed as - " + ClaimNumber + " not found in Open Status";
                                                        insertflag = false;
                                                        vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                    }
                                                    dr.Close();

                                                    #endregion

                                                    string lastStatusQuery = "SELECT TOP 1 vClaimStatus FROM TBL_HEALTH_CLAIMS_DATA WITH(NOLOCK) WHERE vKGIClaimNumber=@vKGIClaimNumber ORDER BY dCreatedDate DESC";
                                                    string lastClaimStatus = "";
                                                    string lastClaimStatusDB = "";

                                                    if (insertflag)
                                                    {
                                                        #region Getting Last Claim status for Claim Number
                                                        cmd = new SqlCommand(lastStatusQuery, _con);
                                                        cmd.Parameters.Add(new SqlParameter("@vKGIClaimNumber", ClaimNumber));
                                                        dr = cmd.ExecuteReader();
                                                        if (dr.Read())
                                                        {
                                                            lastClaimStatusDB = Convert.ToString(dr["vClaimStatus"]);
                                                            lastClaimStatus = lastClaimStatusDB.ToLower();
                                                        }
                                                        if (string.IsNullOrWhiteSpace(lastClaimStatus))
                                                        {
                                                            string[] ckvalidflag = new string[2];
                                                            ckvalidflag[0] = "false";
                                                            ckvalidflag[1] = vSourceFieldName + " - " + vFieldValue + " is not allowed as Status for - " + ClaimNumber + " not found";
                                                            insertflag = false;
                                                            vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                        }
                                                        dr.Close();

                                                        #endregion

                                                        #region Validating incoming status against Last Status for KGI Claim number
                                                        if (insertflag)
                                                        {
                                                            switch (vClaimStatus)
                                                            {
                                                                case "under process":
                                                                case "deficiency":
                                                                case "rejected":
                                                                case "approved":
                                                                    if (lastClaimStatus == "rejected" || lastClaimStatus == "approved" || lastClaimStatus == "reopen" ||
                                                                        lastClaimStatus == "reopen under process" || lastClaimStatus == "reopen rejected" || lastClaimStatus == "reopen approved")
                                                                    {
                                                                        string[] ckvalidflag = new string[2];
                                                                        ckvalidflag[0] = "false";
                                                                        ckvalidflag[1] = vSourceFieldName + " - " + vFieldValue + " is not allowed as - " + ClaimNumber + " is in Status - " + lastClaimStatusDB;
                                                                        insertflag = false;
                                                                        vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                                    }
                                                                    break;

                                                                case "reopen":
                                                                    if (lastClaimStatus != "approved" && lastClaimStatus != "rejected" && lastClaimStatus != "reopen approved" && lastClaimStatus != "reopen rejected")
                                                                    {
                                                                        string[] ckvalidflag = new string[2];
                                                                        ckvalidflag[0] = "false";
                                                                        ckvalidflag[1] = vSourceFieldName + " - " + vFieldValue + " is not allowed as - " + ClaimNumber + " not found in Rejected/Approved/Reopen Rejected/Reopen Approved Status";
                                                                        insertflag = false;
                                                                        vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                                    }
                                                                    else if (lastClaimStatus == "reopen under process")
                                                                    {
                                                                        string[] ckvalidflag = new string[2];
                                                                        ckvalidflag[0] = "false";
                                                                        ckvalidflag[1] = vSourceFieldName + " - " + vFieldValue + " is not allowed as - " + ClaimNumber + " is in Status - " + lastClaimStatusDB;
                                                                        insertflag = false;
                                                                        vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                                    }
                                                                    break;

                                                                case "reopen under process":
                                                                case "reopen rejected":
                                                                case "reopen approved":
                                                                    if (lastClaimStatus != "reopen" && lastClaimStatus != "reopen under process")
                                                                    {
                                                                        string[] ckvalidflag = new string[2];
                                                                        ckvalidflag[0] = "false";
                                                                        ckvalidflag[1] = vSourceFieldName + " - " + vFieldValue + " is not allowed as - " + ClaimNumber + " not found in Reopen/Reopen Under Process Status";
                                                                        insertflag = false;
                                                                        vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                                    }
                                                                    else if (lastClaimStatus == "approved" || lastClaimStatus == "rejected" || lastClaimStatus == "reopen approved" || lastClaimStatus == "reopen rejected")
                                                                    {
                                                                        string[] ckvalidflag = new string[2];
                                                                        ckvalidflag[0] = "false";
                                                                        ckvalidflag[1] = vSourceFieldName + " - " + vFieldValue + " is not allowed as - " + ClaimNumber + " is in Status - " + lastClaimStatusDB;
                                                                        insertflag = false;
                                                                        vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                                    }

                                                                    break;
                                                            }//End of Switch-Case
                                                        }
                                                        #endregion
                                                    }

                                                }//End of try
                                                catch (Exception ex)
                                                {
                                                    ExceptionUtility.LogException(ex, "FrmHealthClaimsUpload.aspx.cs - Upload()");
                                                    if (_tran.Connection != null)
                                                        _tran.Rollback();

                                                    if (_con.State == ConnectionState.Open)
                                                        _con.Close();
                                                    Session["ErrorCallingPage"] = "FrmHealthClaimsUpload.aspx";
                                                    string vStatusMsg = "No Records for Upload or " + ex.Message.ToString().Replace("\r\n", "");
                                                    Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                                                    return;
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        #region Other Business Validations for NON Empty Fields
                                        chkValidFlag = Fn_Check_Business_Validation(vDestinationFieldName, vSourceFieldName, vFieldValue);
                                        #endregion

                                        if (chkValidFlag[0] == "false")
                                        {
                                            insertflag = false;
                                            vErrorDesc = vErrorDesc + chkValidFlag[1].ToString() + ";";
                                        }
                                    }
                                }
                                if (insertflag == false)
                                {
                                    excelrow["vTransType"] = "HCUP";
                                    excelrow["vErrorFlag"] = "Y";
                                    excelrow["vErrorDesc"] = vErrorDesc;
                                    excelrow["vCreatedBy"] = LoggedInUserName;

                                    if (bIsKGIClaimNumberUploadedByUser == "0")
                                    {
                                        excelrow["KGI Claim Number"] = "";
                                        lastClaimNumber--;
                                    }
                                }
                                else
                                {
                                    excelrow["vTransType"] = "HCUP";
                                    excelrow["vErrorFlag"] = "N";
                                    excelrow["vErrorDesc"] = "";
                                    excelrow["vCreatedBy"] = LoggedInUserName;
                                }
                            }
                        }

                        #region Separating Validated Data and Error Log Data
                        DataTable dtValidatedExcelData = null;
                        DataTable dtUploadErrorLog = null;

                        string searchExpressionPass = "vErrorFlag = 'N'";
                        DataRow[] foundRows1 = dtExcelData.Select(searchExpressionPass);
                        if (foundRows1.Count() > 0)
                        {
                            dtValidatedExcelData = foundRows1.CopyToDataTable();
                            validRecords = foundRows1.Length;
                        }
                        string searchExpressionFail = "vErrorFlag = 'Y'";
                        DataRow[] foundRows2 = dtExcelData.Select(searchExpressionFail);
                        if (foundRows2.Count() > 0)
                        {
                            dtUploadErrorLog = foundRows2.CopyToDataTable();
                            inValidRecords = foundRows2.Length;
                        }

                        if (dtValidatedExcelData != null)
                        {
                            foreach (DataRow row in dtValidatedExcelData.Rows)
                            {
                                row["vUploadId"] = vUploadId;
                            }
                        }
                        if (dtUploadErrorLog != null)
                        {
                            foreach (DataRow row in dtUploadErrorLog.Rows)
                            {
                                row["vUploadId"] = vUploadId;
                            }
                        }
                        #endregion

                        #region Bulk Copying Error Log Data
                        if (dtUploadErrorLog != null)
                        {
                            if (dtUploadErrorLog.Rows.Count > 0)
                            {
                                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                                using (SqlConnection con = new SqlConnection(consString))
                                {
                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                    {
                                        //Set the database table name

                                        sqlBulkCopy.DestinationTableName = "dbo.TBL_HEALTH_CLAIMS_DATA_ERROR_LOG";

                                        //[OPTIONAL]: Map the Excel columns with that of the database table
                                        //Getting Columns and Mapping from the Mapping Table

                                        sqlCommand = "SELECT  * FROM TBL_HEALTH_CLAIMS_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'";
                                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        ds = null;
                                        ds = dbCOMMON.ExecuteDataSet(dbCommand);

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            sqlBulkCopy.ColumnMappings.Add("vTransType", "vTransType");
                                            sqlBulkCopy.ColumnMappings.Add("vErrorFlag", "vErrorFlag");
                                            sqlBulkCopy.ColumnMappings.Add("vErrorDesc", "vErrorDesc");
                                            sqlBulkCopy.ColumnMappings.Add("vCreatedBy", "vCreatedBy");
                                            sqlBulkCopy.ColumnMappings.Add("vUploadId", "vUploadId");

                                            foreach (DataRow row in ds.Tables[0].Rows)
                                            {
                                                sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                                            }
                                        }
                                        con.Open();
                                        try
                                        {
                                            sqlBulkCopy.WriteToServer(dtUploadErrorLog);
                                        }

                                        catch (SqlException ex)
                                        {
                                            ExceptionUtility.LogException(ex, "FrmHealthClaimsUpload.aspx");
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
                                                throw new Exception(String.Format("sqlBulkCopy.WriteToServer(dtUploadErrorLog);    catch Column: {0} contains data with a length greater than: {1}", column, length));
                                            }
                                            throw;
                                        }
                                        catch (Exception ex)
                                        {
                                            ExceptionUtility.LogException(ex, "sqlBulkCopy.WriteToServer(dtUploadErrorLog);");
                                        }
                                        con.Close();
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Bulk Copying Validated Data

                        if (dtValidatedExcelData != null)
                        {
                            if (dtValidatedExcelData.Rows.Count > 0)
                            {
                                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                                using (SqlConnection con = new SqlConnection(consString))
                                {
                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                    {
                                        //Set the database table name
                                        sqlBulkCopy.DestinationTableName = "dbo.TBL_HEALTH_CLAIMS_DATA";

                                        //[OPTIONAL]: Map the Excel columns with that of the database table
                                        //Getting Columns and Mapping from the Mapping Table

                                        //sqlCommand = "SELECT  * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'";
                                        sqlCommand = "SELECT  * FROM TBL_HEALTH_CLAIMS_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'";

                                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        ds = null;
                                        ds = dbCOMMON.ExecuteDataSet(dbCommand);

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            sqlBulkCopy.ColumnMappings.Add("vTransType", "vTransType");
                                            sqlBulkCopy.ColumnMappings.Add("vErrorFlag", "vErrorFlag");
                                            sqlBulkCopy.ColumnMappings.Add("vErrorDesc", "vErrorDesc");
                                            sqlBulkCopy.ColumnMappings.Add("vCreatedBy", "vCreatedBy");
                                            sqlBulkCopy.ColumnMappings.Add("vUploadId", "vUploadId");

                                            foreach (DataRow row in ds.Tables[0].Rows)
                                            {
                                                sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                                            }
                                        }
                                        con.Open();
                                        try
                                        {
                                            sqlBulkCopy.WriteToServer(dtValidatedExcelData);
                                        }
                                        catch (SqlException ex)
                                        {
                                            ExceptionUtility.LogException(ex, "FrmHealthClaimsUpload.aspx");
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
                                            throw;
                                        }
                                        con.Close();
                                    }
                                }
                            }
                        }
                        #endregion

                        //else
                        //{
                        //    Session["ErrorCallingPage"] = "FrmHealthClaimsUpload.aspx";
                        //    string vStatusMsg = "Error: Excel Data is not Valid, Please contact Administrator or no valid data to upload with id : " + vUploadId;
                        //    Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        //    return;
                        //}
                    }
                    else
                    {
                        if (_tran.Connection != null)
                            _tran.Rollback();

                        sqlCommand = "DELETE FROM TBL_HEALTH_CLAIMS_DATA Where vUploadId ='" + vUploadId + "'";
                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                        dbCOMMON.ExecuteNonQuery(dbCommand);

                        Session["ErrorCallingPage"] = "FrmHealthClaimsUpload.aspx";
                        string vStatusMsg = "No Records to Upload";
                        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        return;
                    }
                }
                Session["ErrorCallingPage"] = "FrmHealthClaimsUpload.aspx";
                string vStatusMsg1 = "Data Uploaded with Upload Id - " + vUploadId + ", Number of Valid Records = " + validRecords + ", Number of Invalid Records = " + inValidRecords;
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg1, false);
                return;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmHealthClaimsUpload.aspx.cs - Upload()");
                if (_tran.Connection != null)
                    _tran.Rollback();
                string sqlCommand = "DELETE FROM TBL_HEALTH_CLAIMS_DATA Where vUploadId ='" + vUploadId + "'";
                DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                dbCOMMON.ExecuteNonQuery(dbCommand);

                Session["ErrorCallingPage"] = "FrmHealthClaimsUpload.aspx";
                string vStatusMsg = "No Records for Upload or " + ex.Message.ToString().Replace("\r\n", "");
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
            }
        }

        protected string[] Fn_Check_Business_Validation(string vDestinationFieldName, string vSourceFieldName, string vFieldValue)
        {
            string[] ckvalidflag = new string[2];
            ckvalidflag[0] = "true";
            ckvalidflag[1] = " ";

            if (!string.IsNullOrWhiteSpace(vFieldValue))
            {
                switch (vDestinationFieldName)
                {
                    case "vPolicyStartDate":
                    case "vPolicyEndDate":
                    case "vPatientDateofBirth":
                    case "vPreAuthDate":
                    case "vDateReported":
                    case "vDateofLoss":
                    case "vDateOfAdmission":
                    case "vDateOfDischarge":
                    case "vDateOfInjurySustainedOrDiseaseOrIllnessFirstDetected":
                    case "vRejectionReopenDate":
                    case "vClosureReopenDate":
                    case "vQueryReminderSentDate1":
                    case "vQueryReminderSentDate2":
                    case "vQueryReminderSentDate3":
                    case "vQueryReminderSentDate4":
                    case "vQueryReminderSentDate5":
                    case "vInvestigationRequestSentDate":
                    case "vInvestigationResponseReceivedDate":
                    case "vClaimProcessedDate":
                    case "vStatusChangeDate":
                        if (!IsValidDateTimeFormat(vFieldValue, "dd/MM/yyyy"))
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be in valid Date format (DD/MM/YYYY eg. 23/12/2015)";
                        }
                        break;
                    case "vLastModifiedDateTime":
                        if (!IsValidDateTimeFormat(vFieldValue, "dd/MM/yyyy HH:mm"))
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be in valid Date Time format (DD/MM/YYYY HH24:mm eg. 23/12/2015 16:15)";
                        }
                        break;
                    case "vPreAuthTime":
                    case "vReportedTime":
                    case "vTimeofLoss":
                    case "vTimeOfAdmission":
                    case "vTimeOfDischarge":
                        if (!IsValidDateTimeFormat("01/01/2021 " + vFieldValue, "dd/MM/yyyy HH:mm"))
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be in valid Time format (HH24:mm eg. 16:15)";
                        }
                        break;
                    case "vClaimStage":
                        if (vFieldValue.ToLower() != "registration" && vFieldValue.ToLower() != "settlement" && vFieldValue.ToLower() != "payment" && vFieldValue.ToLower() != "reopen")
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be one of the Registration/Settlement/Payment/Reopen";
                        }
                        break;
                    case "vClaimClassification":
                        if (vFieldValue.ToLower() != "ipd" && vFieldValue.ToLower() != "opd")
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be one of the IPD/OPD";
                        }
                        break;
                    case "vWasPreAuthorisationProvided":
                    case "vIsNetwork":
                    case "vFIROrMLCDone":
                    case "vWhetherTheClaimFallsUnderTheExclusion30DayExclusion":
                    case "vWhetherTheClaimFallsUnderTheExclusion2Years":
                    case "vWhetherTheClaimFallsUnderTheExclusionPEDOf4Years":
                    case "vWhetherTheClaimFallsUnderAnySublimit":
                    case "vIsCancelledChequeReceived":
                    case "vIsPANCardCopyReceived":
                        if (vFieldValue.ToLower() != "yes" && vFieldValue.ToLower() != "no")
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be one of the Yes/No";
                        }
                        break;
                    case "vClaimType":
                        if (vFieldValue.ToLower() != "cashless" && vFieldValue.ToLower() != "reimbursement")
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be one of the Cashless/Reimbursement";
                        }
                        break;
                    case "vSystemOfMedicalClaimMaster":
                        if (vFieldValue.ToLower() != "allopathy" && vFieldValue.ToLower() != "homeopathy" && vFieldValue.ToLower() != "ayurveda" && vFieldValue.ToLower() != "unni")
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be one of the Allopathy/Homeopathy/Ayurveda/Unni";
                        }
                        break;
                    case "vPlanOfTreatment":
                        if (vFieldValue.ToLower() != "medical" && vFieldValue.ToLower() != "surgical" && vFieldValue.ToLower() != "both")
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be one of the Medical/Surgical/Both";
                        }
                        break;
                    case "vDischargeType":
                        if (vFieldValue.ToLower() != "healthy" && vFieldValue.ToLower() != "death" && vFieldValue.ToLower() != "transferred")
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be one of the Healthy/Death/Transferred";
                        }
                        break;
                    case "vPayeeType":
                        if (vFieldValue.ToLower() != "member" && vFieldValue.ToLower() != "hospital")
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be one of the Member/Hospital";
                        }
                        break;
                    case "vModeOfPayment":
                        if (vFieldValue.ToLower() != "neft" && vFieldValue.ToLower() != "cheque" && vFieldValue.ToLower() != "dd")
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " must be one of the NEFT/Cheque/DD";
                        }
                        break;
                    case "vReserveAmount":
                        double vReserveAmount = 0;
                        bool isDoubleNumber = double.TryParse(vFieldValue, out vReserveAmount);
                        if (!isDoubleNumber)
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " should be number (10,2) e.g. 1234 or 1234.66 and should be always greater than 1(ONE)";
                        }
                        else if (vReserveAmount <= 1)
                        {
                            ckvalidflag = new string[2];
                            ckvalidflag[0] = "false";
                            ckvalidflag[1] = vSourceFieldName + " should be always greater than 1(ONE)";
                        }
                        break;
                        //case "vQueryClosureRejectionReason":
                        //    if (vFieldValue.ToLower() != "not in policy terms and conditions" && vFieldValue.ToLower() != "misrrepresentation/fraud"
                        //        && vFieldValue.ToLower() != "admission not justified" && vFieldValue.ToLower() != "comes under term exclusion"
                        //        && vFieldValue.ToLower() != "preexisting dieseses" && vFieldValue.ToLower() != "sum insured exausted" && vFieldValue.ToLower() != "na"
                        //        && vFieldValue.ToLower() != "date of admission not within policy period" && vFieldValue.ToLower() != "member not covered")
                        //    {
                        //        ckvalidflag = new string[2];
                        //        ckvalidflag[0] = "false";
                        //        ckvalidflag[1] = vSourceFieldName + " must be one of the Not in policy terms and conditions/(Misrrepresentation/Fraud)/Admission not justified/Comes under term exclusion/Preexisting dieseses/Sum Insured Exausted/Date of Admission not within policy period/Member not covered/NA";
                        //    }
                        //    break;
                }
            }
            return ckvalidflag;
        }

        public bool IsValidDateTimeFormat(string date, string dateTimeFormat)
        {
            DateTime d;
            return DateTime.TryParseExact(date, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
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

            string sqlCommand = "SELECT  * FROM TBL_HEALTH_CLAIMS_COLUMN_MAPPING_MASTER where bExcludeForUpload ='N'";
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

            string _DownloadableProductFileName = "HEALTH_CLAIMS_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "Claims Data", strfilename) == true)
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
                // Work sheet
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheet.Name = worksheetName;

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
                    }
                }
                // now we resize the columns
                oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
                oRng.AutoFitColumns();

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
        public static void FormattingExcelCells(ExcelRange range, Color backColor, Color foreColor, bool isBold)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(backColor);
            range.Style.Font.Color.SetColor(foreColor);
            range.Style.Font.Size = 12;
            range.Style.Font.Bold = isBold;
        }

        public bool DownloadFile(string strfilename)
        {
            string filePath = Server.MapPath("~/Reports");
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

            //compare packets transferred with total number of packets
            if (i < maxCount) return false;
            return true;

            //Close Binary reader and File stream
            _BinaryReader.Close();
            myFile.Close();
        }
    }
}