using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text;
using ProjectPASS;
using System.Globalization;
using System.Drawing;

namespace PrjPASS
{
    public partial class FrmHealthClaimsUTRUploadStatus : System.Web.UI.Page
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

        protected void btnGetPolicy_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dFrom = DateTime.Now;
                bool isValidDateFrom = txtFromDate.Text.Trim().Equals("") ? false : DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dFrom);

                if (!txtFromDate.Text.Trim().Equals("") && !isValidDateFrom)
                {
                    Alert.Show("From Date should be in dd/MM/yyyy format e.g. 23/12/2021");
                    txtFromDate.Text = "";
                    return;
                }

                DateTime dTo = DateTime.Now;
                bool isValidDateTo = txtToDate.Text.Trim().Equals("") ? false : DateTime.TryParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dTo);

                if (!txtToDate.Text.Trim().Equals("") && !isValidDateTo)
                {
                    Alert.Show("To Date should be in dd/MM/yyyy format e.g. 23/12/2021");
                    txtToDate.Text = "";
                    return;
                }

                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "dbo.PROC_GET_HEALTH_CLAIMS_UTR_DATA";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", isValidDateFrom ? (DateTime?)dFrom : null);
                        cmd.Parameters.AddWithValue("@ToDate", isValidDateTo ? (DateTime?)dTo : null);
                        cmd.Parameters.AddWithValue("@ClaimNumber", txtClaimNumber.Text.Trim().Equals("") ? null : txtClaimNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@UploadID", txtUploadId.Text.Trim().Equals("") ? null : txtUploadId.Text.Trim());
                        cmd.Connection = sqlCon;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Columns["dCreatedDate"] != null)
                                dt.Columns.Remove("dCreatedDate");

                            if (dt.Columns["nErrorFlagForOrder"] != null)
                                dt.Columns.Remove("nErrorFlagForOrder");

                            #region Renaming the Columns to Original or readable names
                            SqlCommand cmd1 = new SqlCommand("SELECT  * FROM TBL_HEALTH_CLAIMS_UTR_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'", sqlCon);
                            da = new SqlDataAdapter(cmd1);
                            DataTable dtOriginalColumns = new DataTable();
                            da.Fill(dtOriginalColumns);

                            foreach (DataColumn col in dt.Columns)
                            {
                                DataRow[] drRow = dtOriginalColumns.Select("vDestinationColumnName='" + col.ColumnName + "'");
                                if (drRow.Length > 0)
                                {
                                    col.ColumnName = drRow[0]["vSourceColumnName"].ToString();
                                }
                            }
                            #endregion

                            string filePath = Server.MapPath("~/Reports");

                            string _DownloadableProductFileName = "HEALTH_CLAIMS_UPLOAD_DUMP_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

                            String strfilename = filePath + "\\" + _DownloadableProductFileName;

                            if (System.IO.File.Exists(strfilename))
                            {
                                System.IO.File.Delete(strfilename);
                            }

                            if (ExportDataTableToExcel(dt, "UPLOAD_DUMP", strfilename) == true)
                            {
                                DownloadFile(strfilename);
                            }
                        }
                        else
                        {
                            Alert.Show("No Records found for selected filters");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.Show("Some exception has occurred - " + ex.Message);
                clsAppLogs.LogException(ex);
            }
        }

        private bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        {
            //Create an Excel application instance
            ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
            ExcelRange oRng;

            try
            {
                // Work sheet
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheet.Name = worksheetName;

                string LoggedInUser = Session["vUserLoginId"] == null ? "" : Session["vUserLoginId"].ToString().ToUpper();
                excelSheet.Cells[1, 1].Value = "Health Claims UTR Uploaded Records Dump";
                excelSheet.Cells[2, 1].Value = "Report Taken On Date : " + DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/") + ", Downloaded By - " + LoggedInUser;

                string filters = "Filter(s) - ";

                if (!txtFromDate.Text.Trim().Equals(""))
                {
                    filters += "From Date - " + txtFromDate.Text.Trim() + ", ";
                }
                if (!txtToDate.Text.Trim().Equals(""))
                {
                    filters += "To Date - " + txtToDate.Text.Trim() + ", ";
                }
                if (!txtClaimNumber.Text.Trim().Equals(""))
                {
                    filters += "Claim Number - " + txtClaimNumber.Text.Trim() + ", ";
                }
                if (!txtUploadId.Text.Trim().Equals(""))
                {
                    filters += "UploadID - " + txtUploadId.Text.Trim();
                }

                excelSheet.Cells[3, 1].Value = filters == "Filter(s) - " ? "" : filters.Trim().TrimEnd(',');

                // loop through each row and add values to our sheet
                int rowcount = 4;

                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;

                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 5)
                        {
                            excelSheet.Cells[5, i].Value = dataTable.Columns[i - 1].ColumnName;
                        }

                        excelSheet.Cells[rowcount + 1, i].Value = datarow[i - 1].ToString();
                    }
                }

                oRng = (ExcelRange)excelSheet.Cells[5, 1, 5, dataTable.Columns.Count];
                BorderAround(oRng, Color.FromArgb(79, 129, 189));

                oRng = (ExcelRange)excelSheet.Cells[5, 1, 5, dataTable.Columns.Count];
                FormattingExcelCells(oRng, Color.FromArgb(0, 176, 240), Color.White, true);

                //now we resize the columns
                excelSheet.Cells.AutoFitColumns();

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
        private static void BorderAround(ExcelRange range, Color colour)
        {
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, colour);
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

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}