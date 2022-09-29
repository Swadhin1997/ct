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

namespace ProjectPASS
{
    public partial class FrmCPRenewalUpload : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString);
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        string productCode = string.Empty;
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
            DataSet DS = null;
            if (String.IsNullOrEmpty(FileUpload1.FileName.ToString()))
            {
                Alert.Show("Please Select valid excel file or Excel file not selected!");
                return;
            }

            string cYearMonth = "", vUploadId = "";
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
            vUploadId = "CPUPL" + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt");
            try
            {
                //Upload and save the file
                string Message = "";
                string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(excelPath);
                long count = ImportExcelToSQL(excelPath, Session["vUserLoginId"].ToString(), vUploadId, out Message);
                Alert.Show("Total " + count.ToString() + " records uploaded", "FrmCPRenewalUpload.aspx");
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Error in Upload method, FrmCPRenewalUpload Page.");
            }
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

        public long ImportExcelToSQL(string excelfilepath, string ExcelUploadedBy, string UploadedFileName, out string Message)
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

                    //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                    //dtExcelData.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
                    //    new DataColumn("Name", typeof(string)),
                    //    new DataColumn("Salary",typeof(decimal)) });

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();
                    if (dtExcelData.Rows.Count <= 0)
                    {
                        Alert.Show("Datatable row count is 0.", "FrmCPRenewalUpload.aspx");
                        return 0;
                    }
                    //else
                    //{
                    //    string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    //    using (SqlConnection con = new SqlConnection(consString))
                    //    {
                    //        return SaveRenewalNotice(dtExcelData, ExcelUploadedBy, UploadedFileName, out Message);
                    //    }

                    //}
                    else
                    {
                        string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(consString)) ;

                        Count = BulkCopyToDatabase(dtExcelData, con);
                        return Count;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Error in ImportExcelToSQL on FrmCPRenewalUpload.aspx Page");
                Alert.Show("Some Error Occured. Please check the file format of uploaded file and all columns are available and date in dd-MMM-yy format");
            }
            return Count;
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
                        sqlBulkCopy.DestinationTableName = "dbo.TBL_CP_RENEWALUPLOAD";

                        // Column Mappping for SQLBulkCopy
                        sqlBulkCopy.ColumnMappings.Add("Month", "Month");
                        sqlBulkCopy.ColumnMappings.Add("PolicyNumber", "PolicyNumber");
                        sqlBulkCopy.ColumnMappings.Add("BranchName", "BranchName");
                        sqlBulkCopy.ColumnMappings.Add("Source", "Source");
                        sqlBulkCopy.ColumnMappings.Add("ProductName", "ProductName");
                        sqlBulkCopy.ColumnMappings.Add("ProductCode", "ProductCode");
                        sqlBulkCopy.ColumnMappings.Add("PolicyStartDate", "PolicyStartDate");
                        sqlBulkCopy.ColumnMappings.Add("PolicyEndDate", "PolicyEndDate");
                        sqlBulkCopy.ColumnMappings.Add("Make", "Make");
                        sqlBulkCopy.ColumnMappings.Add("ProposerInsuredName", "ProposerInsuredName");
                        sqlBulkCopy.ColumnMappings.Add("ProposerEmailId", "ProposerEmailId");
                        sqlBulkCopy.ColumnMappings.Add("ProposerMobileNumber", "ProposerMobileNumber");
                        sqlBulkCopy.ColumnMappings.Add("ProposerDateofBirth", "ProposerDateofBirth");
                        sqlBulkCopy.ColumnMappings.Add("RegdNo", "RegdNo");
                        sqlBulkCopy.ColumnMappings.Add("VehicleEngineNumber", "VehicleEngineNumber");
                        sqlBulkCopy.ColumnMappings.Add("VehicleChassisNumber", "VehicleChassisNumber");
                        sqlBulkCopy.ColumnMappings.Add("IDVSI", "IDVSI");
                        sqlBulkCopy.ColumnMappings.Add("FlInd", "FlInd");
                        sqlBulkCopy.ColumnMappings.Add("Agebucket", "Agebucket");
                        sqlBulkCopy.ColumnMappings.Add("IntermediaryCode", "IntermediaryCode");
                        sqlBulkCopy.ColumnMappings.Add("IntermediaryName", "IntermediaryName");
                        sqlBulkCopy.ColumnMappings.Add("PremDue", "PremDue");
                        sqlBulkCopy.ColumnMappings.Add("NCBRN", "NCBRN");
                        sqlBulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
                        sqlBulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                        sqlBulkCopy.ColumnMappings.Add("HomeRiskLocationPincode", "HomeRiskLocationPincode");

                        con.Open();
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
                            Alert.Show(String.Format("Error Occured : Column {0} contains data with a length greater than {1}", column, length),"FrmCPRenewalUpload.aspx");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Error in BulkCopyToDatabase on FrmCPRenewalUpload.aspx Page");
                Alert.Show("Some Error Occured. Please check the file format of uploaded file and all columns are available and date in dd-MMM-yy format. Detail Exception " + ex.ToString(), "FrmCPRenewalUpload.aspx");
            }

            return filesInserted;

        }


        public DataSet SaveRenewalNotice(DataTable dtExcelData, string ExcelUploadedBy, string UploadedFileName, out string Message)
        {
            Message = string.Empty;
            DataSet dataset = new DataSet();
            SqlParameter[] param = null;
            try
            {
                param = new SqlParameter[3];

                param[0] = new SqlParameter("@xml", GetXML(dtExcelData, UploadedFileName));
                param[1] = new SqlParameter("@hdoc", 0);
                param[2] = new SqlParameter("@ExcelUploadedBy", ExcelUploadedBy);
                dataset = ExecuteDataset(con, CommandType.StoredProcedure, "PROC_SAVE_BULK_CP_UPLOAD", param);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                throw new Exception(ex.Message);
            }
            finally
            {
                param = null;
            }
            return dataset;
        }

        private bool IsValidDate(string dateString)
        {
            try
            {
                string[] formats = {
                   "M/d/yyyy h:mm:ss tt",
                   "M/d/yyyy h:mm tt",
                   "MM/dd/yyyy hh:mm:ss",
                   "M/d/yyyy h:mm:ss",
                   "M/d/yyyy hh:mm tt",
                   "M/d/yyyy hh tt",
                   "M/d/yyyy h:mm",
                   "M/d/yyyy h:mm",
                   "MM/dd/yyyy hh:mm",
                   "M/dd/yyyy hh:mm",
                   "dd/MM/yyyy h:mm:ss tt",
                   "dd/MM/yyyy",
                   "M/d/yyyy",
                   "MM/dd/yyyy" ,
                   "dd-MMM-yyyy",
                   "dd-MMM-yy",
                   "d-MMM-yy",
                };

                // string[] formats = { "dd-mmm-yyyy"};

                DateTime dateValue;

                if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "IsValidDate()");
                return false;
            }
        }

        private string GetXML(DataTable dt, string UploadedFileName)
        {
            StringBuilder xml = new StringBuilder(string.Empty);
            int UniqueRowId = 0;
            try
            {


                DateTime date;
                string policyStartdate = null, policyEndDate = null, proposerDateofBirth = null;

                DataColumnCollection columns = dt.Columns;
                xml.Append("<Table>");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PolicyNumber"].ToString().Length > 0)
                    {
                        UniqueRowId++;
                        xml.Append("<Row");
                        xml.Append(string.Format(" UniqueRowId = \"{0}\"", UniqueRowId));
                        xml.Append(string.Format(" UploadId = \"{0}\"", columns.Contains("UploadId") ? dr["UploadId"].ToString() : ""));
                        xml.Append(string.Format(" Month = \"{0}\"", columns.Contains("Month") ? dr["Month"].ToString() : ""));
                        xml.Append(string.Format(" PolicyNumber = \"{0}\"", dr["PolicyNumber"].ToString()));
                        xml.Append(string.Format(" BranchName = \"{0}\"", dr["BranchName"].ToString()));
                        xml.Append(string.Format(" Source = \"{0}\"", dr["Source"].ToString()));
                        xml.Append(string.Format(" ProductName = \"{0}\"", dr["ProductName"].ToString()));
                        xml.Append(string.Format(" ProductCode = \"{0}\"", dr["ProductCode"].ToString()));

                        if (IsValidDate(dr["PolicyStartDate"].ToString()))
                        {
                            policyStartdate = Convert.ToDateTime(dr["PolicyStartDate"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            throw new Exception("Policy start Date can not parse " + dr["PolicyStartDate"].ToString() + " Policy number" + dr["PolicyNumber"].ToString());
                        }
                        xml.Append(string.Format(" PolicyStartDate = \"{0}\"", policyStartdate));

                        if (IsValidDate(dr["PolicyEndDate"].ToString()))
                        {
                            policyEndDate = Convert.ToDateTime(dr["PolicyEndDate"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            throw new Exception("Policy End Date can not parse " + dr["PolicyEndDate"].ToString() + " Policy number" + dr["PolicyNumber"].ToString());
                        }
                        xml.Append(string.Format(" PolicyEndDate = \"{0}\"", policyEndDate));


                        xml.Append(string.Format(" Make = \"{0}\"", dr["Make"].ToString()));
                        xml.Append(string.Format(" ProposerInsuredName = \"{0}\"", dr["ProposerInsuredName"].ToString()));
                        xml.Append(string.Format(" ProposerEmailId = \"{0}\"", dr["ProposerEmailId"].ToString()));
                        xml.Append(string.Format(" ProposerMobileNumber = \"{0}\"", dr["ProposerMobileNumber"].ToString()));

                        proposerDateofBirth = "";
                        if (IsValidDate(dr["ProposerDateofBirth"].ToString()))
                        {
                            proposerDateofBirth = Convert.ToDateTime(dr["ProposerDateofBirth"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            proposerDateofBirth = "";
                        }
                        xml.Append(string.Format(" ProposerDateofBirth = \"{0}\"", proposerDateofBirth));

                        xml.Append(string.Format(" RegdNo = \"{0}\"", dr["RegdNo"].ToString()));
                        xml.Append(string.Format(" VehicleEngineNumber = \"{0}\"", dr["VehicleEngineNumber"].ToString()));
                        xml.Append(string.Format(" VehicleChassisNumber = \"{0}\"", dr["VehicleChassisNumber"].ToString()));

                        xml.Append(string.Format(" IDVSI = \"{0}\"", dr["IDVSI"].ToString()));
                        xml.Append(string.Format(" FlInd = \"{0}\"", dr["FlInd"].ToString()));
                        xml.Append(string.Format(" Agebucket = \"{0}\"", dr["Agebucket"].ToString()));
                        xml.Append(string.Format(" IntermediaryCode = \"{0}\"", dr["IntermediaryCode"].ToString()));


                        xml.Append(string.Format(" IntermediaryName = \"{0}\"", dr["IntermediaryName"].ToString()));
                        xml.Append(string.Format(" PremDue = \"{0}\"", dr["PremDue"].ToString()));
                        if (dr["PremDue"].ToString() == "0" || string.IsNullOrEmpty(dr["PremDue"].ToString()))
                        {
                            Alert.Show("Premium amount can not be 0 or empty for policy number " + dr["PolicyNumber"].ToString(), "FrmCPRenewalUpload.aspx");
                            return null;
                        }

                        xml.Append(string.Format(" NCBRN = \"{0}\"", dr["NCBRN"].ToString()));
                        xml.Append(string.Format(" HomeRiskLocationPincode = \"{0}\"", dr["HomeRiskLocationPincode"].ToString()));

                        xml.Append(" />");
                    }
                }

                xml.Append("</Table>");
                return xml.ToString();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Error in GetXML on FrmCPRenewalUpload.aspx Page");
                Alert.Show("Some Error Occured. Please check the file format of uploaded file and all columns are available and date in dd-MMM-yy format");
            }
            finally
            {
                //xml = null;
            }
            return xml.ToString();
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
            string strFilepath = ConfigurationManager.AppSettings["CPRenewalUploadFormat"].ToString();
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

        //public bool DownloadSampleFile(string strfilename)
        //{
        //    string filePath = Server.MapPath(strfilename);
        //    //filePath = Server.MapPath(filePath);
        //    string _DownloadableProductFileName = filePath;

        //    System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
        //    FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        //    //Reads file as binary values
        //    BinaryReader _BinaryReader = new BinaryReader(myFile);

        //    long startBytes = 0;
        //    string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
        //    string _EncodedData = HttpUtility.UrlEncode
        //        (_DownloadableProductFileName, Encoding.UTF8) + lastUpdateTiemStamp;

        //    //Clear the content of the response
        //    Response.Clear();
        //    Response.Buffer = false;
        //    Response.AddHeader("Accept-Ranges", "bytes");
        //    Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
        //    Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);

        //    //Set the ContentType
        //    Response.ContentType = "application/octet-stream";

        //    //Add the file name and attachment, 
        //    //which will force the open/cancel/save dialog to show, to the header
        //    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName.Name);

        //    //Add the file size into the response header
        //    Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
        //    Response.AddHeader("Connection", "Keep-Alive");

        //    //Set the Content Encoding type
        //    Response.ContentEncoding = Encoding.UTF8;

        //    //Send data
        //    _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

        //    //Dividing the data in 1024 bytes package
        //    int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

        //    //Download in block of 1024 bytes
        //    int i;
        //    for (i = 0; i < maxCount && Response.IsClientConnected; i++)
        //    {
        //        Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
        //        Response.Flush();
        //    }

        //    //compare packets transferred with total number of packets
        //    if (i < maxCount) return false;
        //    return true;

        //    //Close Binary reader and File stream
        //    _BinaryReader.Close();
        //    myFile.Close();
        //}
    }
}