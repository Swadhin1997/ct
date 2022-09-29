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

namespace PrjPASS
{
    public partial class FrmHDCPolicyUpload : System.Web.UI.Page
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

            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cYearMonth = "", vUploadId = "";
            string vCertificateNo = "";
            SqlConnection _con = new SqlConnection(dbCOMMON.ConnectionString);
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

            vUploadId = wsDocNo.fn_Gen_Doc_Master_No("HUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);

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

                    excel_con.Open();

                    string sheet1 = "HDC_UPLOAD_SHEET$"; //excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                    DataTable dtExcelData = new DataTable();

                    bool GetMappingData = false;

                    string sqlCommand = "SELECT  * FROM TBL_HDC_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N'";

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

                        dtExcelData.Columns.Add("vErrorFlag");
                        dtExcelData.Columns.Add("vErrorDesc");
                        dtExcelData.Columns.Add("vTransType");
                        //Business Validation Commented on 15-Dec-2015

                        foreach (DataRow excelrow in dtExcelData.Rows)
                        {

                            string vDestinationFieldName = "";
                            string vSourceFieldName = "";
                            string vFieldValue = "";
                            string vErrorDesc = "";
                            string bMandatoryForPolicy = "";
                            string[] chkValidFlag;

                            if (GetMappingData == true)
                            {
                                bool insertflag = true;

                                for (int i = 1; i <= dtExcelData.Columns.Count; i++)
                                {
                                    string searchExpression = "vSourceColumnName = '" + dtExcelData.Columns[i - 1].ColumnName.ToString().Trim() + "'";

                                    DataRow[] foundRows = ds.Tables[0].Select(searchExpression);

                                    if (foundRows.Count() > 0)
                                    {
                                        vDestinationFieldName = foundRows[0]["vDestinationColumnName"].ToString();
                                        vSourceFieldName = foundRows[0]["vSourceColumnName"].ToString();
                                        vFieldValue = excelrow[dtExcelData.Columns[i - 1].ColumnName.ToString().Trim()].ToString();
                                        bMandatoryForPolicy = foundRows[0]["bMandatoryForPolicy"].ToString();

                                        if (bMandatoryForPolicy == "Y" && vFieldValue.Trim().Length == 0)
                                        {
                                            string[] ckvalidflag = new string[2];
                                            ckvalidflag[0] = "false";
                                            ckvalidflag[1] = vSourceFieldName + " is Mandatory";
                                            insertflag = false;
                                            vErrorDesc = vErrorDesc + ckvalidflag[1].ToString();
                                        }

                                        chkValidFlag = Fn_Check_Business_Validation(vDestinationFieldName, vFieldValue);

                                        //if (vSourceFieldName == "PolicyTenure")
                                        //{
                                        //    if (!String.IsNullOrEmpty(hdnMintenure.Value) && !String.IsNullOrEmpty(hdnMaxtenure.Value))
                                        //    {
                                        //        if (Convert.ToInt32(vFieldValue) >= Convert.ToInt32(hdnMintenure.Value) && Convert.ToInt32(vFieldValue) <= Convert.ToInt32(hdnMaxtenure.Value))
                                        //        {
                                        //            //do nothing
                                        //        }
                                        //        else
                                        //        {
                                        //            string[] ckvalidflag = new string[2];
                                        //            ckvalidflag[0] = "false";
                                        //            ckvalidflag[1] = vSourceFieldName + " is not between the valid range";
                                        //            insertflag = false;
                                        //            vErrorDesc = vErrorDesc + ckvalidflag[1].ToString();
                                        //        }
                                        //    }
                                        //}


                                        if (chkValidFlag[0] == "false")
                                        {
                                            insertflag = false;

                                            vErrorDesc = vErrorDesc + chkValidFlag[1].ToString();
                                        }
                                    }

                                }
                                if (insertflag == false)
                                {

                                    excelrow["vTransType"] = "HUP";
                                    excelrow["vErrorFlag"] = "Y";
                                    excelrow["vErrorDesc"] = vErrorDesc;
                                }
                                else
                                {
                                    excelrow["vTransType"] = "HUP";
                                    excelrow["vErrorFlag"] = "N";
                                    excelrow["vErrorDesc"] = "";
                                }
                            }
                        }

                        DataTable dtValidatedExcelData = null;
                        DataTable dtUploadErrorLog = null;

                        string searchExpressionPass = "vErrorFlag = 'N'";
                        DataRow[] foundRows1 = dtExcelData.Select(searchExpressionPass);
                        if (foundRows1.Count() > 0)
                        {
                            dtValidatedExcelData = dtExcelData.Select(searchExpressionPass).CopyToDataTable();
                        }
                        string searchExpressionFail = "vErrorFlag = 'Y'";
                        DataRow[] foundRows2 = dtExcelData.Select(searchExpressionFail);
                        if (foundRows2.Count() > 0)
                        {
                            dtUploadErrorLog = dtExcelData.Select(searchExpressionFail).CopyToDataTable();
                        }

                        if (dtValidatedExcelData != null)
                        {
                            foreach (DataRow row in dtValidatedExcelData.Rows)
                            {
                                row["UploadId"] = vUploadId;
                            }
                        }
                        if (dtUploadErrorLog != null)
                        {
                            foreach (DataRow row in dtUploadErrorLog.Rows)
                            {
                                row["UploadId"] = vUploadId;
                            }
                        }
                        // Add Certificate No to dtValidatedExcelData
                        if (dtValidatedExcelData != null)
                        {
                            foreach (DataRow row in dtValidatedExcelData.Rows)
                            {
                                vCertificateNo = wsDocNo.fn_Gen_Cert_No(DateTime.Now.ToString("ddMMyy"), Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
                                string vCertificateNoInExcel = ConfigurationManager.AppSettings["vCertificateNoInExcel"].ToString();
                                row[vCertificateNoInExcel] = vCertificateNo;
                            }
                        }

                        //if (dtValidatedExcelData != null)
                        //{
                        //    foreach (DataRow row in dtValidatedExcelData.Rows)
                        //    {
                        //        row["ProductCode"] = cmbProductCode.SelectedText;
                        //    }
                        //}




                        //else
                        //{
                        //    Session["ErrorCallingPage"] = "FrmGPAPolicyUpload.aspx";
                        //    string vStatusMsg = "Error: Excel Data is not Valid, Please contact Administrator";
                        //    Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        //    return;
                        //}

                        //End of Add Certificate No

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

                                        sqlBulkCopy.DestinationTableName = "dbo.TBL_HDC_POLICY_TABLE_ERROR_LOG";

                                        //[OPTIONAL]: Map the Excel columns with that of the database table


                                        //Getting Columns and Mapping from the Mapping Table

                                        sqlCommand = "SELECT  * FROM TBL_HDC_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N'";
                                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        ds = null;
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
                                            sqlBulkCopy.WriteToServer(dtUploadErrorLog);
                                        }
                                        
                                        catch (SqlException ex)
                                        {
                                            ExceptionUtility.LogException(ex, "HrmHDCUpload.Upload");
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

                                        //sqlBulkCopy.DestinationTableName = "dbo.TBL_HDC_POLICY_TABLE";
                                        sqlBulkCopy.DestinationTableName = "dbo.tbl_hdc_policy_replica_table";

                                        //[OPTIONAL]: Map the Excel columns with that of the database table


                                        //Getting Columns and Mapping from the Mapping Table

                                        //sqlCommand = "SELECT  * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N'";
                                        sqlCommand = "SELECT  * FROM TBL_HDC_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N'";

                                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        ds = null;
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
                                            sqlBulkCopy.WriteToServer(dtValidatedExcelData);
                                        }
                                        catch (SqlException ex)
                                        {
                                            ExceptionUtility.LogException(ex, "HrmHDCUpload.Upload");
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

                                        UpdateGSTData(dtValidatedExcelData);

                                        //string UpdateStr = "";

                                        //sqlCommand = "select a.nCoverSI,a.bIsActive,b.vCoverFieldInDB,b.vCoverSIFieldInDB,b.vCoverSITextFieldInDB,a.vCoverSIText from TBL_PLAN_DETAIL_MASTER a,TBL_COVER_MASTER b " +
                                        //" where A.vCoverCode = b.vCoverCode and vPlanCode='" + drpPlanList.SelectedValue + "'";

                                        //dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        //DataSet dsPlan = null;
                                        //dsPlan = dbCOMMON.ExecuteDataSet(dbCommand);

                                        //if (dsPlan.Tables[0].Rows.Count > 0)
                                        //{
                                        //    foreach (DataRow planrow in dsPlan.Tables[0].Rows)
                                        //    {
                                        //        if (planrow["bIsActive"].ToString().Trim() == "Y")
                                        //        {
                                        //            UpdateStr = UpdateStr + "," + planrow["vCoverFieldInDB"] + " = 'Y'," + planrow["vCoverSIFieldInDB"] + " = '" + planrow["nCoverSI"] + "'," + planrow["vCoverSITextFieldInDB"] + " = '" + planrow["vCoverSIText"].ToString().Replace("'", "''") + "'";
                                        //        }
                                        //        else
                                        //        {
                                        //            UpdateStr = UpdateStr + "," + planrow["vCoverFieldInDB"] + " = 'N'," + planrow["vCoverSIFieldInDB"] + " = '0'," + planrow["vCoverSITextFieldInDB"] + " = 'NA'";
                                        //        }
                                        //    }
                                        //}

                                        // sqlCommand = "select * from TBL_PLAN_HEAD_MASTER " +
                                        //" where vPlanCode='" + drpPlanList.SelectedValue + "'";

                                        // dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        // DataSet dsPlanHead = null;
                                        // dsPlanHead = dbCOMMON.ExecuteDataSet(dbCommand);
                                        // if (dsPlanHead.Tables[0].Rows.Count > 0)
                                        //{
                                        //    //Customized for GPA
                                        //    //sqlCommand = "UPDATE TBL_GPA_POLICY_TABLE SET nStampDuty="+ dsPlanHead.Tables[0].Rows [0]["nStampDuty"] + ", "+
                                        //    //" nSectionAPrem="+ dsPlanHead.Tables[0].Rows[0]["nSecAPremium"] + ", "+
                                        //    //" nExtToSectionAPrem="+ dsPlanHead.Tables[0].Rows[0]["nExtToSecAPremium"] + ","+
                                        //    //" nSectionBPrem="+ dsPlanHead.Tables[0].Rows[0]["nSecBPremium"] + ",nPlanSI="+ dsPlanHead.Tables[0].Rows[0]["nPlanSI"] + "," +
                                        //    //" dAccountDebitDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103), dCustomerDob = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dCustomerDob, 101) as DATE),103),dPolicyEndDate = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dPolicyEndDate, 101) as DATE),103), dProposalDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103),dPolicyStartDate =Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103) " + UpdateStr + " Where vUploadId ='" + vUploadId + "'";
                                        //    //dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        //    //dbCOMMON.ExecuteNonQuery(dbCommand);


                                        //    //sqlCommand = "UPDATE TBL_GPA_POLICY_TABLE SET vAdditional_column_1=GETDATE(),nStampDuty=" + dsPlanHead.Tables[0].Rows[0]["nStampDuty"] + ", " +
                                        //    //" nSectionAPrem=" + dsPlanHead.Tables[0].Rows[0]["nSecAPremium"] + ", " +
                                        //    //" nExtToSectionAPrem=" + dsPlanHead.Tables[0].Rows[0]["nExtToSecAPremium"] + "," +
                                        //    //" nSectionBPrem=" + dsPlanHead.Tables[0].Rows[0]["nSecBPremium"] + ",nPlanSI=" + dsPlanHead.Tables[0].Rows[0]["nPlanSI"] + "," +
                                        //    //" dAccountDebitDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103), dCustomerDob = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dCustomerDob, 101) as DATE),103),dPolicyEndDate = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dPolicyEndDate, 101) as DATE),103), dProposalDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103),dPolicyStartDate =Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103) " + UpdateStr + " Where vUploadId ='" + vUploadId + "'";
                                        //    ////" dAccountDebitDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 103) as DATE),103), dCustomerDob = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dCustomerDob, 103) as DATE),103),dPolicyEndDate = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dPolicyEndDate, 103) as DATE),103), dProposalDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 103) as DATE),103),dPolicyStartDate =Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 103) as DATE),103) " + UpdateStr + " Where vUploadId ='" + vUploadId + "'";

                                        //    //  sqlCommand = "UPDATE TBL_GPA_POLICY_TABLE SET vAdditional_column_1=GETDATE(),nStampDuty=" + dsPlanHead.Tables[0].Rows[0]["nStampDuty"] + ", " +
                                        //    //" nSectionAPrem=" + dsPlanHead.Tables[0].Rows[0]["nSecAPremium"] + ", " +
                                        //    //" nExtToSectionAPrem=" + dsPlanHead.Tables[0].Rows[0]["nExtToSecAPremium"] + "," +
                                        //    //" nSectionBPrem=" + dsPlanHead.Tables[0].Rows[0]["nSecBPremium"] + ",nPlanSI=" + dsPlanHead.Tables[0].Rows[0]["nPlanSI"] + "," +
                                        //    //" dAccountDebitDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103), dCustomerDob = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dCustomerDob, 101) as DATE),103),dPolicyEndDate = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dPolicyEndDate, 101) as DATE),103), dProposalDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103),dPolicyStartDate =Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103) " + UpdateStr +"," + 
                                        //    //" vProposalType='" + dsPlanHead.Tables[0].Rows[0]["vProposalType"] +"',vSIBasis='"+ dsPlanHead.Tables[0].Rows[0]["vSIBasis"]+"',vFinancerName='"+ dsPlanHead.Tables[0].Rows[0]["vFinancerName"]+"',vMasterPolicyDate='"+ dsPlanHead.Tables[0].Rows[0]["vMasterPolicyDate"]+"',vMasterPolicyLoc='"+ dsPlanHead.Tables[0].Rows[0]["vMasterPolicyLoc"]+"',vPlanCode='"+ drpPlanList.SelectedValue + "', vLoanType='"+ dsPlanHead.Tables[0].Rows[0]["vLoanType"]+"' where vUploadId='"+ vUploadId+"'";
                                        //    //" dAccountDebitDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 103) as DATE),103), dCustomerDob = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dCustomerDob, 103) as DATE),103),dPolicyEndDate = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dPolicyEndDate, 103) as DATE),103), dProposalDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 103) as DATE),103),dPolicyStartDate =Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 103) as DATE),103) " + UpdateStr + " Where vUploadId ='" + vUploadId + "'";

                                        //    sqlCommand = "UPDATE TBL_GPA_POLICY_TABLE SET vAdditional_column_1=GETDATE(),nStampDuty=" + dsPlanHead.Tables[0].Rows[0]["nStampDuty"] + ", " +
                                        //" nSectionAPrem=" + dsPlanHead.Tables[0].Rows[0]["nSecAPremium"] + ", " +
                                        //" nExtToSectionAPrem=" + dsPlanHead.Tables[0].Rows[0]["nExtToSecAPremium"] + "," +
                                        //" nSectionBPrem=" + dsPlanHead.Tables[0].Rows[0]["nSecBPremium"] + ",nPlanSI=" + dsPlanHead.Tables[0].Rows[0]["nPlanSI"] + "," +
                                        //" dAccountDebitDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103), dCustomerDob = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dCustomerDob, 101) as DATE),103),dPolicyEndDate = Convert(VARCHAR(20),CAST(CONVERT(Datetime, dPolicyEndDate, 101) as DATE),103), dProposalDate= Convert(VARCHAR(20),CAST(CONVERT(Datetime, dAccountDebitDate, 101) as DATE),103),dPolicyStartDate =Convert(VARCHAR(20),CAST(CONVERT(Datetime, dPolicyStartDate, 101) as DATE),103) " + UpdateStr + "," +
                                        //" vProposalType='" + dsPlanHead.Tables[0].Rows[0]["vProposalType"] + "',vSIBasis='" + dsPlanHead.Tables[0].Rows[0]["vSIBasis"] + "',vFinancerName='" + dsPlanHead.Tables[0].Rows[0]["vFinancerName"] + "',vMasterPolicyDate='" + dsPlanHead.Tables[0].Rows[0]["vMasterPolicyDate"] + "',vMasterPolicyLoc='" + dsPlanHead.Tables[0].Rows[0]["vMasterPolicyLoc"] + "',vPlanCode='" + drpPlanList.SelectedValue + "', vLoanType='" + dsPlanHead.Tables[0].Rows[0]["vLoanType"] + "' where vUploadId='" + vUploadId + "'";


                                        //    dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        //    //db.AddParameter(dbCommand, "vCoverSIText", DbType.String, ParameterDirection.Input, "vCoverSIText", DataRowVersion.Current, vCoverSIText);
                                        //    dbCOMMON.ExecuteNonQuery(dbCommand);

                                        //}
                                        //else
                                        //{
                                        //    Session["ErrorCallingPage"] = "FrmGPAPolicyUpload.aspx";
                                        //    string vStatusMsg = "No Plan Header Records Founds";
                                        //    Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                                        //    return;
                                        //}
                                    }
                                }
                            }
                        }
                        else
                        {
                            Session["ErrorCallingPage"] = "FrmHDCPolicyUpload.aspx";
                            string vStatusMsg = "Error: Excel Data is not Valid, Please contact Administrator or no valid data to upload with id : " + vUploadId;
                            Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                            return;
                        }
                    }
                    else
                    {
                        _tran.Rollback();
                       //sqlCommand = "DELETE FROM TBL_HDC_POLICY_TABLE Where vUploadId ='" + vUploadId + "'";
                        sqlCommand = "DELETE FROM tbl_hdc_policy_replica_table Where vUploadId ='" + vUploadId + "'";
                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                        dbCOMMON.ExecuteNonQuery(dbCommand);

                        Session["ErrorCallingPage"] = "FrmHDCPolicyUpload.aspx";
                        string vStatusMsg = "No Records to Upload";
                        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        return;
                    }
                }
                //_tran.Commit();
                Session["ErrorCallingPage"] = "FrmHDCPolicyUpload.aspx";
                string vStatusMsg1 = "Data Uploaded with Upload Id  " + vUploadId; // + " and Certificate No.: " + vCertificateNo; //certificate no cannot be appended bcoz per row one certificate number gets created
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg1, false);
                return;
            }

            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex , "FrmHDCPolicyUpload.aspx.cs.Upload ");
                _tran.Rollback();
                //string sqlCommand = "DELETE FROM TBL_HDC_POLICY_TABLE Where vUploadId ='" + vUploadId + "'";
                string sqlCommand = "DELETE FROM tbl_hdc_policy_replica_table Where vUploadId ='" + vUploadId + "'";
                DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                dbCOMMON.ExecuteNonQuery(dbCommand);

                Session["ErrorCallingPage"] = "FrmHDCPolicyUpload.aspx";
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

                        //gstBreakup = objCommon.GSTBreakUpTaxDetails(Convert.ToDateTime(dtValidatedExcelData.Rows[i]["PolicyStartDate"].ToString()).ToString("dd/MM/yyyy"), dtValidatedExcelData.Rows[i]["NetPremium"].ToString(), dtValidatedExcelData.Rows[i]["IntermediaryCode"].ToString(), interStateCode, custStateCode, ConfigurationManager.AppSettings["HDCProdCode"].ToString());
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
                        ExceptionUtility.LogException(ex, "FrmHDCPolicyUpload-writeErrorHDC");
                        writeErrorHDC(dtTemp, dtValidatedExcelData, i, "Error while fetching GST", db);

                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmHDCPolicyUpload-UpdateGSTData");

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
                               // throw new Exception(String.Format("Column: {0} contains data with a length greater than: {1}", column, length));

                            }

                            //throw;
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

            string sqlCommand = "SELECT  * FROM TBL_HDC_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload ='N'";
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