using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text;

namespace ProjectPASS
{
    public partial class FrmGPACancelUploadStatus : System.Web.UI.Page
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
                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

            using (SqlConnection sqlCon = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand("PROC_GET_DATA_GPA_CANCLE_UPLOAD_STATUS" , sqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string strvPolicyId = "";
                    string strvUploadId = "";

                    if (txtPolicyId.Text.Trim() != "")
                    {
                        //strvPolicyId = " and vCertificateNo ='" + txtPolicyId.Text.Trim() + "'";
                        cmd.Parameters.AddWithValue("@vPolicyId"  ,txtPolicyId.Text.Trim());
                    }
                    if (txtUploadId.Text.Trim() != "")
                    {
                        //strvUploadId = " and vUploadId='" + txtUploadId.Text.Trim() + "'";
                        cmd.Parameters.AddWithValue("@vUploadId" , txtUploadId.Text.Trim());
                    }
                    if (txtFromDate.Text == "" && txtUploadId.Text.Trim() != " ")
                    {
                        Alert.Show("Please Select From Date.");
                        return;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@vFromDate" , txtFromDate.Text.Trim());
                    }
                    if (txtToDate.Text == "" && txtUploadId.Text.Trim() != " ")
                    {
                        Alert.Show("Please Select To Date.");
                        return;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@vToDate" , txtToDate.Text.Trim());
                    }
                    string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString();
                    string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();

                    //cmd.CommandText = "SELECT * FROM TBL_GPA_POLICY_TABLE where vTransType ='PUP' and DateAdd(day, datediff(day,0, dModifiedDate), 0) between Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ") " + strvPolicyId + " " + strvUploadId + "  UNION ALL" +
                    //" SELECT * FROM TBL_GPA_POLICY_TABLE_ERROR_LOG where vTransType ='CPL' and DateAdd(day, datediff(day,0, dCreatedDate), 0) between Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ") " + strvPolicyId + " " + strvUploadId + "";

                        cmd.Connection = sqlCon;
                        sqlCon.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            string filePath = Server.MapPath("~/Reports");

                            //  string filePath = @"D:\Manish\Reports";

                            string _DownloadableProductFileName = "GPA_CANCEL_UPLOAD_DUMP_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".xls";

                            String strfilename = filePath + "\\" + _DownloadableProductFileName;

                            if (System.IO.File.Exists(strfilename))
                            {
                                System.IO.File.Delete(strfilename);
                            }

                            if (ExportDataTableToExcel(dt, "GPA_CANCEL_UPLOAD_DUMP", strfilename) == true)
                            {
                                DownloadFile(strfilename);
                            }
                            sqlCon.Close();
                        }
                        else
                        {
                            sqlCon.Close();
                            Alert.Show("No Records found for selected filters");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Session["ErrorCallingPage"] = "FrmGPACancelUploadStatus.aspx";
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + ex.Message + " and stack trace : " + ex.StackTrace, false);

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

                excelSheet.Cells[1, 1].Value = "Cancel Uploads Details Status";
                excelSheet.Cells[2, 1].Value = "Report Taken On Date : " + DateTime.Now.ToShortDateString();

                // loop through each row and add values to our sheet
                int rowcount = 2;


                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;

                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 3)
                        {
                            excelSheet.Cells[3, i].Value = dataTable.Columns[i - 1].ColumnName;
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
                                    FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
                                }
                            }
                        }
                    }
                }

                // now we resize the columns
                oRng = (ExcelRange)excelSheet.Cells[3, dataTable.Columns.Count];
                oRng.AutoFitColumns();
                BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));

                oRng = (ExcelRange)excelSheet.Cells[3, dataTable.Columns.Count];
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