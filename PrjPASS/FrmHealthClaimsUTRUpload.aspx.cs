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
    public partial class FrmHealthClaimsUTRUpload : System.Web.UI.Page
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
            string vUTRNumber = "";
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

            vUploadId = wsDocNo.fn_Gen_Doc_Master_No("UTRUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);

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
                    string sqlCommand = "SELECT  * FROM TBL_HEALTH_CLAIMS_UTR_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'";
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

                        foreach (DataRow excelrow in dtExcelData.Rows)
                        {
                            string vDestinationFieldName = "";
                            string vSourceFieldName = "";
                            string vFieldValue = "";
                            string vErrorDesc = "";
                            string lastClaimStatus = "";

                            string[] chkValidFlag;

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

                                        #region Basic Validations
                                        chkValidFlag = Fn_Check_Business_Validation(vDestinationFieldName, vSourceFieldName, vFieldValue);
                                        #endregion

                                        if (chkValidFlag[0] == "false")
                                        {
                                            insertflag = false;
                                            vErrorDesc = vErrorDesc + chkValidFlag[1].ToString() + ";";
                                        }

                                        #region Duplication Check on the basis of KGI Claim Number, UTR Number
                                        if (insertflag && vDestinationFieldName == "vUTRNumber")
                                        {
                                            try
                                            {
                                                vKGIClaimNumber = excelrow["KGI Claim Number"] == DBNull.Value || excelrow["KGI Claim Number"] == null ? "" : excelrow["KGI Claim Number"].ToString().Trim();
                                                vUTRNumber = excelrow["UTR Number"] == DBNull.Value || excelrow["UTR Number"] == null ? "" : excelrow["UTR Number"].ToString().Trim();

                                                //Checking if this combination exists in uploaded excel sheet(dtExcelData)

                                                searchExpression = "[KGI Claim Number]='" + vKGIClaimNumber + "' AND [UTR Number]='" + vUTRNumber + "'";

                                                foundRows = dtExcelData.Select(searchExpression);

                                                if (foundRows.Length > 1)
                                                {
                                                    string[] ckvalidflag = new string[2];
                                                    ckvalidflag[0] = "false";
                                                    ckvalidflag[1] = "KGI Claim Number - " + vKGIClaimNumber + ", UTR Number - " + vUTRNumber + " combination is repeated in uploaded excel sheet";
                                                    insertflag = false;
                                                    vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                }
                                                if (insertflag)
                                                {
                                                    //Checking if this combination exists in TBL_HEALTH_CLAIMS_UTR_DATA
                                                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM TBL_HEALTH_CLAIMS_UTR_DATA WHERE vKGIClaimNumber=@vKGIClaimNumber AND vUTRNumber=@vUTRNumber", _con);

                                                    cmd.Parameters.Add(new SqlParameter("@vKGIClaimNumber", vKGIClaimNumber));
                                                    cmd.Parameters.Add(new SqlParameter("@vUTRNumber", vUTRNumber));

                                                    int recordsFound = Convert.ToInt32(cmd.ExecuteScalar());
                                                    if (recordsFound > 0)
                                                    {
                                                        string[] ckvalidflag = new string[2];
                                                        ckvalidflag[0] = "false";
                                                        ckvalidflag[1] = "KGI Claim Number - " + vKGIClaimNumber + ", UTR Number - " + vUTRNumber + " combination already exists in Table";
                                                        insertflag = false;
                                                        vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ExceptionUtility.LogException(ex, "FrmHealthClaimsUTRUpload.aspx.cs - Upload()");
                                                if (_tran.Connection != null)
                                                    _tran.Rollback();

                                                if (_con.State == ConnectionState.Open)
                                                    _con.Close();
                                                Session["ErrorCallingPage"] = "FrmHealthClaimsUTRUpload.aspx";
                                                string vStatusMsg = "No Records for Upload or " + ex.Message.ToString().Replace("\r\n", "");
                                                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                                                return;
                                            }
                                        }
                                        #endregion

                                        #region Checking the Last Status of KGI Claim Number(Should be allowed if last status is Approved or Reopen Approved)
                                        if (insertflag && vDestinationFieldName == "vKGIClaimNumber" && !string.IsNullOrWhiteSpace(vFieldValue))
                                        {
                                            try
                                            {
                                                SqlCommand cmd = new SqlCommand("SELECT TOP 1 vClaimStatus FROM TBL_HEALTH_CLAIMS_DATA WITH(NOLOCK) WHERE vKGIClaimNumber = @vKGIClaimNumber ORDER BY dCreatedDate DESC", _con);
                                                cmd.Parameters.Add(new SqlParameter("@vKGIClaimNumber", vFieldValue));

                                                dr = cmd.ExecuteReader();
                                                if (dr.Read())
                                                {
                                                    lastClaimStatus = Convert.ToString(dr["vClaimStatus"]).ToLower();
                                                }
                                                if (lastClaimStatus != "approved" && lastClaimStatus != "reopen approved")
                                                {
                                                    string[] ckvalidflag = new string[2];
                                                    ckvalidflag[0] = "false";
                                                    ckvalidflag[1] = "UTR Upload not allowed as KGI Claim Number - " + vFieldValue + " is not in Approved/Reopen Approved Status";
                                                    insertflag = false;
                                                    vErrorDesc = vErrorDesc + ckvalidflag[1].ToString() + ";";
                                                }
                                                dr.Close();
                                            }
                                            catch (Exception ex)
                                            {
                                                ExceptionUtility.LogException(ex, "FrmHealthClaimsUTRUpload.aspx.cs - Upload()");
                                                if (_tran.Connection != null)
                                                    _tran.Rollback();

                                                if (_con.State == ConnectionState.Open)
                                                    _con.Close();
                                                Session["ErrorCallingPage"] = "FrmHealthClaimsUTRUpload.aspx";
                                                string vStatusMsg = "No Records for Upload or " + ex.Message.ToString().Replace("\r\n", "");
                                                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                                                return;
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                if (insertflag == false)
                                {
                                    excelrow["vTransType"] = "UTRU";
                                    excelrow["vErrorFlag"] = "Y";
                                    excelrow["vErrorDesc"] = vErrorDesc;
                                    excelrow["vCreatedBy"] = LoggedInUserName;
                                }
                                else
                                {
                                    excelrow["vTransType"] = "UTRU";
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

                                        sqlBulkCopy.DestinationTableName = "dbo.TBL_HEALTH_CLAIMS_UTR_DATA_ERROR_LOG";

                                        //[OPTIONAL]: Map the Excel columns with that of the database table
                                        //Getting Columns and Mapping from the Mapping Table

                                        sqlCommand = "SELECT  * FROM TBL_HEALTH_CLAIMS_UTR_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'";
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
                                            ExceptionUtility.LogException(ex, "FrmHealthClaimsUTRUpload.aspx");
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
                                        sqlBulkCopy.DestinationTableName = "dbo.TBL_HEALTH_CLAIMS_UTR_DATA";

                                        //[OPTIONAL]: Map the Excel columns with that of the database table
                                        //Getting Columns and Mapping from the Mapping Table

                                        //sqlCommand = "SELECT  * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'";
                                        sqlCommand = "SELECT  * FROM TBL_HEALTH_CLAIMS_UTR_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'";

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
                                            ExceptionUtility.LogException(ex, "FrmHealthClaimsUTRUpload.aspx");
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
                        //    Session["ErrorCallingPage"] = "FrmHealthClaimsUTRUpload.aspx";
                        //    string vStatusMsg = "Error: Excel Data is not Valid, Please contact Administrator or no valid data to upload with id : " + vUploadId;
                        //    Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        //    return;
                        //}
                    }
                    else
                    {
                        if (_tran.Connection != null)
                            _tran.Rollback();

                        sqlCommand = "DELETE FROM TBL_HEALTH_CLAIMS_UTR_DATA Where vUploadId ='" + vUploadId + "'";
                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                        dbCOMMON.ExecuteNonQuery(dbCommand);

                        Session["ErrorCallingPage"] = "FrmHealthClaimsUTRUpload.aspx";
                        string vStatusMsg = "No Records to Upload";
                        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        return;
                    }
                }
                Session["ErrorCallingPage"] = "FrmHealthClaimsUTRUpload.aspx";
                string vStatusMsg1 = "Data Uploaded with Upload Id - " + vUploadId + ", Number of Valid Records = " + validRecords + ", Number of Invalid Records = " + inValidRecords;
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg1, false);
                return;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmHealthClaimsUTRUpload.aspx.cs - Upload()");
                if (_tran.Connection != null)
                    _tran.Rollback();
                string sqlCommand = "DELETE FROM TBL_HEALTH_CLAIMS_UTR_DATA Where vUploadId ='" + vUploadId + "'";
                DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                dbCOMMON.ExecuteNonQuery(dbCommand);

                Session["ErrorCallingPage"] = "FrmHealthClaimsUTRUpload.aspx";
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

            switch (vDestinationFieldName)
            {
                case "vKGIClaimNumber":
                case "vUTRNumber":
                    if (string.IsNullOrWhiteSpace(vFieldValue))
                    {
                        ckvalidflag = new string[2];
                        ckvalidflag[0] = "false";
                        ckvalidflag[1] = vSourceFieldName + " cannot be empty";
                    }
                    break;
                case "vUTRDate":
                    if (string.IsNullOrWhiteSpace(vFieldValue))
                    {
                        ckvalidflag = new string[2];
                        ckvalidflag[0] = "false";
                        ckvalidflag[1] = vSourceFieldName + " cannot be empty. Should be in valid Date format (DD/MM/YYYY eg. 23/12/2015)";
                    }
                    else if (!IsValidDateTimeFormat(vFieldValue, "dd/MM/yyyy"))
                    {
                        ckvalidflag = new string[2];
                        ckvalidflag[0] = "false";
                        ckvalidflag[1] = vSourceFieldName + " should be in valid Date format (DD/MM/YYYY eg. 23/12/2015)";
                    }
                    break;
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

            string sqlCommand = "SELECT  * FROM TBL_HEALTH_CLAIMS_UTR_COLUMN_MAPPING_MASTER where bExcludeForUpload ='N'";
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

            string _DownloadableProductFileName = "HEALTH_CLAIMS_UTR_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "Claims UTR Data", strfilename) == true)
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