using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Text;
using ProjectPASS;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
//using System.Data.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using PrjPASS;

namespace ProjectPASS
{
    [System.Runtime.InteropServices.Guid("41A52E12-FEE7-4002-9D25-0E3C2DC2C4DB")]
    public partial class FrmMobileRenewalUpload : System.Web.UI.Page
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

        protected  void Upload(object sender, EventArgs e)
        {
            //ExceptionUtility.LogEvent("Start FrmMobileRenewalUpload.aspx Upload method");

            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            if (String.IsNullOrEmpty(FileUpload1.FileName.ToString()))
            {
                Alert.Show ("Kindly select file to upload");
            }
            string vUploadId = "", cYearMonth = "";
            string excelPath = string.Empty;
            string uploadedFileName = string.Empty;
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

            //vUploadId = wsDocNo.fn_Gen_Doc_Master_No("MRUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
            //_tran.Commit();
            excelPath = Server.MapPath("~/Uploads/MobileRenewal/") + Path.GetFileName(FileUpload1.FileName.ToString());
            uploadedFileName = Path.GetFileName(FileUpload1.FileName.ToString());
            FileUpload1.SaveAs(excelPath);
            _con.Close();
            //ExceptionUtility.LogEvent("Calling FrmMobileRenewalUpload.aspx ImportExcelToSQL method");
            ImportExcelToSQL(excelPath, Session["vUserLoginId"].ToString(),uploadedFileName);


        }
        public void ImportExcelToSQL(string excelfilepath, string ExcelUploadedBy, string UploadedFileName)
        {
            //ExceptionUtility.LogEvent("Start FrmMobileRenewalUpload.aspx ImportExcelToSQL method");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            SqlConnection cs = new SqlConnection(dbCOMMON.ConnectionString);
            string conString = string.Empty;
            bool isDeletedRows = false;
            long Count = 0l;
            var filesInserted = 0L;
            int UniqueRowId = 0;
            string extension = Path.GetExtension(excelfilepath);
            string filePath = Server.MapPath("~/Reports/MobileRenewal");
            DataTable dterror = new DataTable();
            bool IsDownload = false;
            //string[] uploadfilename = UploadedFileName.Split('.');
            //string upfileName = "";


            //upfileName = (Path.GetFileNameWithoutExtension(UploadedFileName));
            
            string _DownloadableProductFileName = "MOBILE_RN_ERROR_DUMP_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xlsx";
            DataTable dtUploadErrorLog = null;
            DataTable dtValidatedExcelData = null;

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
            try
            {
                using (OleDbConnection con = new OleDbConnection(conString))
                {
                    con.Open();
                    string sheet1 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();//"MOBILE_RENEWAL_UPLOAD_SHEET$"; /*con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();*/
                    DataTable dtExcel = new DataTable();
                    bool GetMappingData = false;

                    string sqlCommand = "SELECT  * FROM TBL_MOBILE_RN_UPLOAD_MAPPING  ";

                    DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);

                    DataSet ds = null;

                    ds = dbCOMMON.ExecuteDataSet(dbCommand);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GetMappingData = true;
                    }


                    using (OleDbDataAdapter oda = new OleDbDataAdapter("Select * from [" + sheet1 + "]", con))
                    {

                        oda.Fill(dtExcel);
                    }

                    //Adding manula column to the excel
                    dtExcel.Columns.Add("UniqueRowId").SetOrdinal(0);
                    dtExcel.Columns.Add("DataUploadId").SetOrdinal(1);
                    dtExcel.Columns.Add("DataUploadedBy").SetOrdinal(2);
                    dtExcel.Columns.Add("RowCreatedOn").SetOrdinal(3);
                    dtExcel.Columns.Add("UploadedFileName").SetOrdinal(4);
                    
                    if (dtExcel.Rows.Count < 0)
                    {
                        Alert.Show("Datatable row count is 0.", "FrmMobileRenewalUpload.aspx");
                    }
                    else
                    {

                        dtExcel.Columns.Add("vErrorFlag");
                        dtExcel.Columns.Add("vErrorDesc");


                        // bool insertflag = false;
                        foreach (DataRow drExcel in dtExcel.Rows)
                        {
                            string vDestinationFieldName = "";
                            string vSourceFieldName = "";
                            string vFieldValue = "";
                            string vErrorDesc = "";
                            string bMandatoryForPolicy = "";
                            string[] chkValidFlag;
                            //  UniqueRowId++;
                            bool insertflag = true;


                            drExcel["DataUploadId"] = UploadedFileName;
                            // drExcel["UniqueRowId"] = UniqueRowId;  //uncomment
                            drExcel["DataUploadedBy"] = Session["vUserLoginId"].ToString();
                            drExcel["RowCreatedOn"] = Convert.ToDateTime(DateTime.Now);
                            for (int i = 1; i <= dtExcel.Columns.Count - 2; i++)
                            {
                                string searchExpression = "vSourceColumnName = '" + dtExcel.Columns[i - 1].ColumnName.ToString().Trim() + "'";

                                DataRow[] foundRows = ds.Tables[0].Select(searchExpression);

                                if (foundRows.Count() > 0)
                                {
                                    vDestinationFieldName = foundRows[0]["vDestinationColumnName"].ToString();
                                    vSourceFieldName = foundRows[0]["vSourceColumnName"].ToString();
                                    //if (vSourceFieldName == "PolicyTenure")
                                    //{

                                    //}
                                    vFieldValue = drExcel[dtExcel.Columns[i - 1].ColumnName.ToString().Trim()].ToString();
                                    chkValidFlag = Fn_Check_Business_Validation(vDestinationFieldName, vFieldValue);
                                    if (vSourceFieldName == "DueDate")
                                    {
                                        if (!string.IsNullOrEmpty(vFieldValue))
                                        {
                                            DateTime dDate;

                                            if (DateTime.TryParse(vFieldValue, out dDate))
                                            {
                                                vFieldValue = String.Format("{0:dd-MMM-yyyy}", dDate);
                                            }
                                            else
                                            {
                                                chkValidFlag[0] = "false";
                                                chkValidFlag[1] = vSourceFieldName + "Invalid Date Format.";
                                            }

                                        }
                                    }

                                    if (vSourceFieldName == "PolicyNo" || vSourceFieldName == "KMBLCRN" || vSourceFieldName == "DueDate" || vSourceFieldName == "CustomerName" || vSourceFieldName == "Product" || vSourceFieldName == "Premium" || vSourceFieldName == "MobileNo" || vSourceFieldName == "BankBRMName" || vSourceFieldName == "BankBRMLoginID")
                                    {
                                        if (String.IsNullOrEmpty(vFieldValue))
                                        {

                                            chkValidFlag[0] = "false";
                                            chkValidFlag[1] = vSourceFieldName + " is mandatory";

                                        }
                                    }
                                    if (chkValidFlag[0] == "false")
                                    {
                                        insertflag = false;

                                        vErrorDesc = vErrorDesc + chkValidFlag[1].ToString();
                                    }
                                }
                            }
                            if (insertflag == false)
                            {

                              drExcel["vErrorFlag"] = "Y";
                                drExcel["vErrorDesc"] = vErrorDesc;
                            }
                            else
                            {
                                drExcel["vErrorFlag"] = "N";
                                drExcel["vErrorDesc"] = "";
                            }
                        }

                        string searchExpressionPass = "vErrorFlag = 'N'";
                        DataRow[] foundRows1 = dtExcel.Select(searchExpressionPass);
                        if (foundRows1.Count() > 0)
                        {
                            dtValidatedExcelData = dtExcel.Select(searchExpressionPass).CopyToDataTable();
                        }
                        string searchExpressionFail = "vErrorFlag = 'Y'";
                        DataRow[] foundRows2 = dtExcel.Select(searchExpressionFail);
                        if (foundRows2.Count() > 0)
                        {
                            dtUploadErrorLog = dtExcel.Select(searchExpressionFail).CopyToDataTable();
                        }


                        if (dtUploadErrorLog != null)
                        {
                            //ExceptionUtility.LogEvent("Entered dtUploadErrorLog ");
                            String strfilename = filePath + "\\" + _DownloadableProductFileName;
                            
                            if (dtUploadErrorLog.Rows.Count > 0)
                            {
                                dterror.Columns.Add(new DataColumn("PolicyNo", typeof(string)));
                                dterror.Columns.Add(new DataColumn("KMBLCRN", typeof(string)));
                                dterror.Columns.Add(new DataColumn("DueDate", typeof(string)));
                                dterror.Columns.Add(new DataColumn("CustomerName", typeof(string)));
                                dterror.Columns.Add(new DataColumn("Product", typeof(string)));
                                dterror.Columns.Add(new DataColumn("Premium", typeof(string)));
                                dterror.Columns.Add(new DataColumn("MobileNo", typeof(string)));
                                dterror.Columns.Add(new DataColumn("BankBRMName", typeof(string)));
                                dterror.Columns.Add(new DataColumn("BankBRMLoginID", typeof(string)));
                                dterror.Columns.Add(new DataColumn("vErrorFlag", typeof(string)));
                                dterror.Columns.Add(new DataColumn("vErrorDesc", typeof(string)));
                                foreach (DataRow dr in dtExcel.Rows)
                                {

                                    DataRow datarw = dterror.NewRow();

                                    datarw["PolicyNo"] = dr["PolicyNo"].ToString();
                                    datarw["KMBLCRN"] = dr["KMBLCRN"].ToString();
                                    datarw["DueDate"] = Convert.ToDateTime(dr["DueDate"]).ToString("dd-MMM-yyyy");
                                    datarw["CustomerName"] = dr["CustomerName"].ToString();
                                    datarw["Product"] = dr["Product"].ToString();
                                    datarw["Premium"] = dr["Premium"].ToString();
                                    datarw["MobileNo"] = dr["MobileNo"].ToString();
                                    datarw["BankBRMName"] = dr["BankBRMName"].ToString();
                                    datarw["BankBRMLoginID"] = dr["BankBRMLoginID"].ToString();
                                    datarw["vErrorFlag"] = dr["vErrorFlag"].ToString();
                                    datarw["vErrorDesc"] = dr["vErrorDesc"].ToString();

                                    dterror.Rows.Add(datarw);
                                   
                                    //if (ExportDataTableToExcel(dterror, "MOBILE_RENEWAL_UPLOAD_ERROR_DUMP", strfilename) == true)
                                    //{

                                       // //ExceptionUtility.LogEvent("Entered FrmMobileRenewalUpload.aspx ExportDataTableToExcel dtUploadErrorLog:-  ");
                                        //string messagess = "Error: Excel Data is not Valid, Please check error description in the downloaded excel file";
                                        //alert(messagess);
                                      //  IsDownload = DownloadFile(strfilename);
                                        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Error: Excel Data is not Valid, Please check error description in the downloaded excel file!!!');", true);
                                        //string messages = "Error: Excel Data is not Valid, Please check error description in the downloaded excel file";
                                        //alert(messages);
                                        //if (IsDownload)
                                        //{
                                        //    //Session["ErrorCallingPage"] = "FrmMobileRenewalUpload.aspx";

                                        //    string message = "Error: Excel Data is not Valid, Please check error description in the downloaded excel file";
                                        //    alert(message);
                                        //    Response.Write("<script>window.open('messages'-blank');</script>");
                                        //    //Alert.Show( "Error: Excel Data is not Valid, Please check error description in the downloaded excel file");
                                        //    //Response.Redirect("FrmMobileRenewalUpload.aspx?vErrorMsg=" + vStatusMsg1, false);
                                        //    return;
                                        //}

                                  //  }
                                }
                                if (ExportDataTableToExcel(dterror, "MOBILE_RENEWAL_UPLOAD_ERROR_DUMP", strfilename) == true)
                                {

                                    //ExceptionUtility.LogEvent("Entered FrmMobileRenewalUpload.aspx ExportDataTableToExcel dtUploadErrorLog:-  ");
                                    //string messagess = "Error: Excel Data is not Valid, Please check error description in the downloaded excel file";
                                    //alert(messagess);
                                    IsDownload = DownloadFile(strfilename);
                                }

                                }
                        }

                        if (dtValidatedExcelData != null)
                        {

                            //ExceptionUtility.LogEvent("Entered FrmMobileRenewalUpload.aspx ExportDataTableToExcel dtValidatedExcelData:-  ");
                            string vUploadId = "", cYearMonth = "";
                            string excelPath = string.Empty;
                            string uploadedFileName = string.Empty;
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
                            vUploadId = wsDocNo.fn_Gen_Doc_Master_No("MRUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
                            _tran.Commit();

                            isDeletedRows = Fn_DeleteCommand();
                            if (isDeletedRows)
                            {
                                foreach (DataRow drExcel in dtExcel.Rows)
                                {
                                    UniqueRowId++;
                                    drExcel["DataUploadId"] = vUploadId;
                                    // drExcel["UniqueRowId"] = UniqueRowId;  //uncomment
                                    drExcel["DataUploadedBy"] = Session["vUserLoginId"].ToString();
                                    drExcel["RowCreatedOn"] = Convert.ToDateTime(DateTime.Now);
                                    // drExcel["RowCreatedOn"] = Convert.ToDateTime(DateTime.Now);
                                    drExcel["UploadedFileName"] = UploadedFileName;
                                }
                                using (SqlBulkCopy sqlbulkinsert = new SqlBulkCopy(cs))
                                {

                                    cs.Open();
                                    sqlbulkinsert.DestinationTableName = "TBL_MOBILE_APP_RENEWAL_UPLOAD"; // "TBL_GPA_POLICY_TABLE"; //change

                                    sqlbulkinsert.ColumnMappings.Add("UniqueRowId", "UniqueRowId");
                                    sqlbulkinsert.ColumnMappings.Add("RowCreatedOn", "RowCreatedOn");
                                    sqlbulkinsert.ColumnMappings.Add("DataUploadedBy", "DataUploadedBy");
                                    sqlbulkinsert.ColumnMappings.Add("DataUploadId", "DataUploadId");
                                    sqlbulkinsert.ColumnMappings.Add("UploadedFileName", "UploadedFileName");

                                    sqlbulkinsert.ColumnMappings.Add("PolicyNo", "PolicyNo");
                                    sqlbulkinsert.ColumnMappings.Add("KMBLCRN", "KMBLCRN");
                                    sqlbulkinsert.ColumnMappings.Add("DueDate", "DueDate");
                                    sqlbulkinsert.ColumnMappings.Add("CustomerName", "CustomerName");
                                    sqlbulkinsert.ColumnMappings.Add("Product", "Product");
                                    sqlbulkinsert.ColumnMappings.Add("Premium", "Premium");
                                    sqlbulkinsert.ColumnMappings.Add("MobileNo", "MobileNo");
                                    sqlbulkinsert.ColumnMappings.Add("BankBRMName", "BankBRMName");
                                    sqlbulkinsert.ColumnMappings.Add("BankBRMLoginID", "BankBRMLoginID");
                                    //bulkinsert.ColumnMappings.Add("DataUploadId", "DataUploadId");

                                    if (con.State == ConnectionState.Closed)
                                    {
                                        con.Open();
                                    }
                                    sqlbulkinsert.NotifyAfter = dtExcel.Rows.Count;
                                    sqlbulkinsert.SqlRowsCopied += (s, e) => filesInserted = e.RowsCopied;
                                    // cons.Open();
                                    try
                                    {
                                        sqlbulkinsert.WriteToServer(dtExcel);
                                        Session["ErrorCallingPage"] = "FrmMobileRenewalUpload.aspx";
                                        string vStatusMsg1 = "Data Uploaded with Upload Id  " + vUploadId; // + " and Certificate No.: " + vCertificateNo; //certificate no cannot be appended bcoz per row one certificate number gets created
                                        alert(vStatusMsg1);
                                        return;
                                    }
                                    catch (Exception ex)
                                    {
                                        //ExceptionUtility.LogEvent("Entered Exception FrmMobileRenewalUpload.aspx dtValidatedExcelData:-  ");
                                        if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                                        {
                                            string pattern = @"\d+";
                                            Match match = Regex.Match(ex.Message.ToString(), pattern);
                                            var index = Convert.ToInt32(match.Value) - 1;

                                            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                                            var sortedColumns = fi.GetValue(sqlbulkinsert);
                                            var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                                            FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                                            var metadata = itemdata.GetValue(items[index]);

                                            var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                            var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                            Alert.Show(String.Format("Error Occured : Column {0} contains data with a length greater than {1}", column, length), "FrmMobileRenewalUpload.aspx");
                                            ExceptionUtility.LogException(ex, "Error Occured : Column {0} contains data with a length greater than {1}"+ column   + length +    "FrmMobileRenewalUpload.aspx");
                                        }


                                    }
                                    con.Close();
                                    cs.Close();

                                    // _tran.Commit();
                                    //Session["ErrorCallingPage"] = "FrmMobileRenewalUpload.aspx";
                                    //string vStatusMsg1 = "Data Uploaded with Upload Id  " + vUploadId; // + " and Certificate No.: " + vCertificateNo; //certificate no cannot be appended bcoz per row one certificate number gets created
                                    //alert(vStatusMsg1);                                                                //Response.Redirect("FrmMobileRenewalUpload.aspx?vErrorMsg=" + vStatusMsg1, false);
                                    // Alert.Show(vStatusMsg1);
                                    //Response.Redirect("FrmMobileRenewalUpload.aspx");

                                }
                                //}
                                // }


                            }
                        }

                        //else
                        //{

                        //        Session["ErrorCallingPage"] = "FrmMobileRenewalUpload.aspx";
                        //        string vStatusMsg = "Error: Excel Data is not Valid, Please contact Administrator or no valid data to upload";
                        //        alert(vStatusMsg);
                        //    return;

                        //}


                    }

                }
            }catch(Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmMobileRenewalUpload.aspx ImportExcelToSQL() exception occured " + UploadedFileName);
            }

        }


        protected bool Fn_DeleteCommand()
        {
            var cs = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
           
            bool isrowsdeleted = false;
            using (var connection = new SqlConnection(cs))
            {
               
               
               SqlDataAdapter sda = new SqlDataAdapter("Select * from TBL_MOBILE_APP_RENEWAL_UPLOAD", connection);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string strSqlInsCommand = "INSERT INTO TBL_MOBILE_APP_RENEWAL_UPLOAD_HISTORY  ([RowCreatedOn],[DataUploadedBy],[DataUploadId],[UploadedFileName],[PolicyNo],[KMBLCRN],[DueDate] " +
                                   "   , [CustomerName], [Product], [Premium], [MobileNo], [BankBRMName], [BankBRMLoginID] )Select  [RowCreatedOn],[DataUploadedBy],[DataUploadId],[UploadedFileName],[PolicyNo],[KMBLCRN],[DueDate] " +
                                   "   , [CustomerName], [Product], [Premium], [MobileNo], [BankBRMName], [BankBRMLoginID] from TBL_MOBILE_APP_RENEWAL_UPLOAD";

                    SqlCommand _cmd = new SqlCommand(strSqlInsCommand, connection);
                    connection.Open();
                    _cmd.ExecuteNonQuery();


                    string StrSqlDelCommand = "Delete from TBL_MOBILE_APP_RENEWAL_UPLOAD  ";
                    SqlCommand sqldelcmd = new SqlCommand(StrSqlDelCommand, connection);
                    var rowsaffected = sqldelcmd.ExecuteNonQuery();

                    //_trans.Commit();
                    connection.Close();

                    if (rowsaffected > 0)
                    {
                        isrowsdeleted = true;
                    }
                }
                else
                {
                    isrowsdeleted = true;
                }
            }
            return isrowsdeleted;
        }
     
        protected void btnExport_Click(object sender, EventArgs e)
        {

            //ExceptionUtility.LogEvent("Start btnExport_Click method");
            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            //TemplateTable.Columns.Add("UniqueRowId");
            //TemplateTable.Columns.Add("RowCreatedOn");
            //TemplateTable.Columns.Add("DataUploadedBy");
            //TemplateTable.Columns.Add("DataUploadId");
            //TemplateTable.Columns.Add("UploadedFileName");
            TemplateTable.Columns.Add("PolicyNo");
            TemplateTable.Columns.Add("KMBLCRN");
            TemplateTable.Columns.Add("DueDate");
            TemplateTable.Columns.Add("CustomerName");
            TemplateTable.Columns.Add("Product");
            TemplateTable.Columns.Add("Premium");
            TemplateTable.Columns.Add("MobileNo");
            TemplateTable.Columns.Add("BankBRMName");
            TemplateTable.Columns.Add("BankBRMLoginID");
            
            
            
           
            
            DataRow newBlankRow = TemplateTable.NewRow();
            TemplateTable.Rows.InsertAt(newBlankRow, 0);
            
            string filePath = Server.MapPath("~/Reports/MobileRenewal/");

            string _DownloadableProductFileName = "MOBILE_RENEWAL_UPLOAD_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "MOBILE_RENEWAL_UPLOAD_SHEET", strfilename) == true)
            {
                //ExceptionUtility.LogEvent("Calling DownloadFile method");
                DownloadFile(strfilename);
            }
        }

        public bool DownloadFile(string strfilename)
        {
            bool res = false;
            try
            {
                //ExceptionUtility.LogEvent("Start FrmMobileRenewalUpload.aspx DownloadFile method");
            string filePath = Server.MapPath("~/Reports/MobileRenewal/");
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
            //ExceptionUtility.LogEvent("End FrmMobileRenewalUpload.aspx DownloadFile method");

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

        protected void btnExit_Click(object sender , EventArgs e)
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
                                   // FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
                                }
                            }
                        }
                    }
                }

                // now we resize the columns
                oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
                oRng.AutoFitColumns();
               // BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));

                oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
               // FormattingExcelCells(oRng, "#000099", System.Drawing.Color.White, true);


                //now save the workbook and exit Excel

                excelApp.Save();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "ExportDataTableToExcel");
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
        protected string[] Fn_Check_Business_Validation(string vFieldName, string vFieldValue)
        {
            //ExceptionUtility.LogEvent("Start Fn_Check_Business_Validation");
            string[] ckvalidflag = new string[2];
            ckvalidflag[0] = "true";
            ckvalidflag[1] = " ";

            return ckvalidflag;
        }

        private void alert(string message)
        {
            Response.Write("<script>alert('" + message + "')</script>");
        }
    }
}