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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Globalization;
namespace PrjPASS
{
    public partial class FrmIIBClaimDataUploadNew : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session.Timeout = 30;
                //For Testing Purpose
                //Fn_GET_CLAIM_DATA_FROM_XML();
                //clsIIBRequest objIIB = new clsIIBRequest();
                //objIIB.RegNo = "88888888888"; objIIB.ChassisNo = "8848484848484";  objIIB.EngineNo= "7676767678"; objIIB.policyNo = ""; objIIB.insurerName = "test";
                ////objIIB.ClaimObject = new string[4];
                //String[] objClaimDtl = { };
                //IIBWebService.IBWebServicePortTypeClient obj = new IIBWebService.IBWebServicePortTypeClient();
                ////System.Threading.Tasks.Task<IIBWebService.getResultsResponse> res = obj.getResultsAsync("xyz","xyz","hytfdr","78787878787","1324422","hjhjh","hjhjhjio");
                //var res = obj.getResults("xyz", "xyz", ref objIIB.RegNo, ref objIIB.ChassisNo, ref objIIB.EngineNo, objIIB.policyNo, objIIB.insurerName, out objClaimDtl);
                //Fn_Get_Claim_Details();
                //DownloadExcel();
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
            catch (Exception ex)
            {

            }
        }
       
       
       
        protected void btnDownloadLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmIIBClaimDataDownload.aspx");

        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
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

        public void DownloadExcel()
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            string sqlCommand = "SELECT * FROM tbl_IIB_CLAIM_DATA";
            DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
            DataSet dsColumnNames = null;
            dsColumnNames = dbCOMMON.ExecuteDataSet(dbCommand);
            DataTable dt = dsColumnNames.Tables[0];
            string attachment = "attachment; filename=CLAIM_DATA.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            string sqlCommand = "SELECT * FROM TBL_IIB_CLAIM_COLUMN_MAPPING_MASTER";
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

            string _DownloadableProductFileName = "IIB_CLAIM_DATA_UPLOAD_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "IIB_CLAIM_DATA_UPLOAD_TEMPLATE", strfilename) == true)
            {
                DownloadFile(strfilename);
            }
        }


        #region Download
        private static bool DataSetToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation, string logfile)
        {
            //File.AppendAllText(Properties.Settings.Default.logfile + logfile, "Status  : entered into fn DataSetToExcel" + DateTime.Now + Environment.NewLine);
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    dataTable.TableName = "Download_Log_Records";
                    ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dataTable.TableName);
                    workSheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;
                    pck.SaveAs(new FileInfo(saveAsLocation));
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                //File.AppendAllText(Properties.Settings.Default.logfile + logfile, "Error occured  : " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                return false;
            }
        }
        private static bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        {
            //Creae an Excel application instance
            //File.AppendAllText(Properties.Settings.Default.logfile + logfile, "Status  : entered into fn ExportDataTableToExcel" + DateTime.Now + Environment.NewLine);
            ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
            ExcelRange oRng;

            try
            {

                // Workk sheet
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheet.Name = worksheetName;
                int rowcount = 0;
                if (worksheetName.ToUpper() != "IIB_CLAIM_DATA_UPLOAD_TEMPLATE")
                {
                    rowcount = 2;
                    excelSheet.Cells[1, 1].Value = "CLAIM Download Details";
                    excelSheet.Cells[2, 1].Value = "Report Taken On Date : " + DateTime.Now.ToShortDateString();
                }
                else
                {
                    rowcount = 0;
                }
                // loop through each row and add values to our sheet



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
                        if (rowcount > 2)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    oRng = (ExcelRange)excelSheet.Cells[rowcount, dataTable.Columns.Count];
                                    //FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
                                }
                            }
                        }
                    }
                }

                // now we resize the columns
                oRng = (ExcelRange)excelSheet.Cells[3, dataTable.Columns.Count];
                oRng.AutoFitColumns();
                BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));
                rowcount = 1;
                for (int col = 1; col <= dataTable.Columns.Count; col++)
                {

                    oRng = (ExcelRange)excelSheet.Cells[rowcount, col];
                    FormattingExcelCells(oRng, "#873260", System.Drawing.Color.White, true);
                }


                //now save the workbook and exit Excel

                excelApp.Save();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                //  Alert.Show(ex.Message);
                //File.AppendAllText(Properties.Settings.Default.logfile + logfile, "Error occured  : " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                return false;
            }
            finally
            {
                excelApp = null;
            }
        }
        private static void BorderAround(ExcelRange range, int colour)
        {
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.ColorTranslator.FromOle(colour));
        }
        public static void FormattingExcelCells(ExcelRange range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.Indexed = 16;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
            range.Style.Font.Size = 12;
            //range.Style.Font.Color = System.Drawing.Color.White;
            if (IsFontbool == true)
            {
                range.Style.Font.Bold = IsFontbool;
            }
        }

        #endregion

        #region Existing Excel Generate Method
        //private bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        //{
        //    //Creae an Excel application instance

        //    ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
        //    ExcelRange oRng;

        //    try
        //    {

        //        // Workk sheet
        //        var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
        //        excelSheet.Name = worksheetName;

        //        //excelSheet.Cells[1, 1] = "Detail";
        //        //excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

        //        // loop through each row and add values to our sheet
        //        int rowcount = 0;


        //        foreach (DataRow datarow in dataTable.Rows)
        //        {
        //            rowcount += 1;

        //            for (int i = 1; i <= dataTable.Columns.Count; i++)
        //            {
        //                // on the first iteration we add the column headers
        //                if (rowcount == 1)
        //                {
        //                    excelSheet.Cells[1, i].Value = dataTable.Columns[i - 1].ColumnName;
        //                    // excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
        //                }

        //                excelSheet.Cells[rowcount + 1, i].Value = datarow[i - 1].ToString();

        //                //for alternate rows
        //                if (rowcount > 1)
        //                {
        //                    if (i == dataTable.Columns.Count)
        //                    {
        //                        if (rowcount % 2 == 0)
        //                        {
        //                            oRng = (ExcelRange)excelSheet.Cells[rowcount, dataTable.Columns.Count];
        //                            FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        // now we resize the columns
        //        oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
        //        oRng.AutoFitColumns();
        //        BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));

        //        oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
        //        FormattingExcelCells(oRng, "#000099", System.Drawing.Color.White, true);
        //        //now save the workbook and exit Excel

        //        excelApp.Save();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Alert.Show(ex.Message);
        //        return false;
        //    }
        //    finally
        //    {
        //        excelApp = null;
        //    }
        //}
        //private void BorderAround(ExcelRange range, int colour)
        //{
        //    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.ColorTranslator.FromOle(colour));
        //}
        //public void FormattingExcelCells(ExcelRange range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        //{
        //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    range.Style.Fill.BackgroundColor.Indexed = 16;
        //    range.Style.Font.Size = 12;
        //    //range.Style.Font.Color = System.Drawing.Color.White;
        //    if (IsFontbool == true)
        //    {
        //        range.Style.Font.Bold = IsFontbool;
        //    }
        //}

        #endregion
       
        protected void Upload(object sender, EventArgs e)
        {

            string dirFullPath = string.Empty;
            string Message = string.Empty;
            System.Threading.Thread.Sleep(3000);
            string allowedExtensions = ".xlsx";

            string Date = DateTime.Now.Date.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Hour = DateTime.Now.Hour.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Minute = DateTime.Now.Minute.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Second = DateTime.Now.Second.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Millisecond = DateTime.Now.Millisecond.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            //string FinalOutputFileName = Date + Hour + Minute + Second + Millisecond + "_BulkInvoiceLinks.xls";
            //string outPutPath = Server.MapPath("~/Reports/") + FinalOutputFileName.Replace(" ", "");
            string FileUploadTransactionId = Date + Hour + Minute + Second + Millisecond;


            if (!FileUpload1.HasFile)
            {
                lblstatus.Text = "Error: File Upload Unsuccussful, Please select a excel (.xlsx) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }
            if (String.IsNullOrEmpty(FileUpload1.FileName.ToString()))
            {
                Alert.Show("Please Select valid excel file or Excel file not selected!");
                return;
            }

            if (!FileUpload1.HasFile)
            {
                Alert.Show("Please select the file");
                //Session["ErrorCallingPage"] = "FrmGPAClaimsUpload.aspx";
                string vStatusMsg = "Please select valid excel file for upload";
                //Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
            }


            if (FileUpload1.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                if (fileExtension != allowedExtensions)
                {

                    Alert.Show("Invalid file Extension, Only XLSX files are allowed to be Uploaded");
                    //Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                    return;
                }
            }

            String fileName = FileUpload1.PostedFile.FileName;
            string fileExt = System.IO.Path.GetExtension(FileUpload1.FileName);

            if (fileExt.Trim().ToLower() != ".xlsx")
            {
                lblstatus.Text = "Error: File Upload Unsuccussful, Please select a excel (.xlsx) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                bool IsUploadSuccessfull = UploadFileForScheduler(FileUploadTransactionId, ref dirFullPath);
                if (IsUploadSuccessfull)
                {
                    //Save Record into database

                    string message = SaveBulkIIBCLAIMFileUploadInformation(FileUploadTransactionId, fileName, dirFullPath, Session["vUserLoginId"].ToString().ToUpper());
                    if (message == "success")
                    {
                        lblstatus.Text = "File Uploaded succussfully.";
                        // Alert.Show("Data uploaded successfully.");
                        ExceptionUtility.LogEvent("Error: File Upload Unsuccussful, please contact developer FrmIIBDataUploadNew" + fileName);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
                        lblstatus.Text = "Error: File Upload Unsuccussful, Error:" + message;
                        ExceptionUtility.LogEvent("FrmIIBDataUploadNew Error: File Upload Unsuccussful, Error :" + message);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "HideProgress();", true);
                    FileProcessGridView.DataBind();

                }
                else
                {
                    ExceptionUtility.LogEvent("Error: File Upload Unsuccussful, please contact developer FrmIIBDataUploadNew" + fileName);
                    lblstatus.Text = "Error: File Upload Unsuccussful, please contact developer";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
        }

        private string SaveBulkIIBCLAIMFileUploadInformation(string FileUploadTransactionId, string FileName, string FileFullPath, string FileUploadedBy)
        {
            string Message = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_BULK_IIB_CLAIM_SERVICE_UPLOAD_INFORMATION";

                        cmd.Parameters.AddWithValue("@FileUploadTransactionId", FileUploadTransactionId);
                        cmd.Parameters.AddWithValue("@FileName", FileName);
                        cmd.Parameters.AddWithValue("@FileFullPath", FileFullPath);
                        cmd.Parameters.AddWithValue("@FileUploadedBy", FileUploadedBy);

                        cmd.Connection = conn;
                        conn.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            Message = "success";
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                ExceptionUtility.LogException(ex, "SaveBulkInvoiceFileUploadInformation");
            }

            return Message;
        }

        private bool UploadFileForScheduler(string FileTranId, ref string dirFullPath)
        {
            bool IsUploadSuccessfull = false;
            dirFullPath = string.Empty;
            if (FileUpload1.HasFile)
            {
                try
                {
                    string strDate = DateTime.Now.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture);
                    string filename = Path.GetFileName(FileUpload1.FileName);
                    dirFullPath = Server.MapPath("~/Uploads/") + FileTranId + "_" + filename;
                    if (System.IO.File.Exists(dirFullPath))
                    {
                        //Alert.Show("File already exists");
                        ExceptionUtility.LogEvent("File already exists" + dirFullPath);
                        return false;
                        //IsUploadSuccessfull = false;
                    }
                    FileUpload1.SaveAs(Server.MapPath("~/Uploads/") + FileTranId + "_" + filename);

                   
                     
                    IsUploadSuccessfull = true;
                    
                }
                catch (Exception ex)
                {
                    IsUploadSuccessfull = false;
                    ExceptionUtility.LogException(ex, "UploadFileForScheduler");
                }
            }

            return IsUploadSuccessfull;
        }
        //protected void QuoteGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    QuoteGridView.PageIndex = e.NewPageIndex;
        //}

//#endregion


        #region CodeForFileProcessGridView

        public IEnumerable<ProjectPASS.FileUploadedInformation> FileProcessGridView_GetData(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            int pageSize = maximumRows;
            int pageIndex = 0;

            totalRowCount = GetFileDetails().Count();

            if (startRowIndex > 0)
            {
                pageIndex = (int)Math.Round(((double)startRowIndex / (double)pageSize));
            }

            return GetFileDetails().OrderByDescending(x => x.FileUploadedOn).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public IEnumerable<FileUploadedInformation> GetFileDetails()
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_IIB_CLAIM_BULK_FILE_UPLOADED_DETAILS";

                    //cmd.Parameters.AddWithValue("@LoginUserId", Session["vUserLoginId"].ToString());
                    cmd.Parameters.AddWithValue("@LoginUserId", "emp00001");
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return this.CreateFileSavedDetails(reader);
                        }
                    }
                    conn.Close();
                }
            }
        }

        private FileUploadedInformation CreateFileSavedDetails(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("No detail exist.");
            }

            return new FileUploadedInformation
            {
                FileUploadTransactionId = Convert.ToString(reader["FileUploadTransactionId"]),
                FileName = Convert.ToString(reader["FileName"]),
                FileUploadedBy = Convert.ToString(reader["FileUploadedBy"]),
                FileUploadedOn = Convert.ToString(reader["FileUploadedOn"]),
                IsFileProcessed = Convert.ToString(reader["IsFileProcessed"]),
                FileProcessedOn = Convert.ToString(reader["FileProcessedOn"]),
            };
        }

        protected void FileProcessGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FileProcessGridView.PageIndex = e.NewPageIndex;
        }

        #endregion

    }
}