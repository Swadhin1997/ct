using Microsoft.Practices.EnterpriseLibrary.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmHDCMISDownload : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");
        string logFile = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/HDCPaymentExcelDownloadLog.txt";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!File.Exists(logFile))
            {
                File.Create(logFile);
            }

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

                Directory.CreateDirectory(folderPath);

                // clnClaimIntimationDate.DateMax = DateTime.Now;
                //   txtClaimIntimationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //   txtClaimRegistrationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //   txtClaimIntimationDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

            }
        }


        private void ResetControls()
        {
            txtToDate.Text = string.Empty;
            txtFromDate.Text = string.Empty;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmHDCMISDownload.aspx");
        }


        protected void btnExit_Click1(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {


            File.AppendAllText(logFile, "Click on download Button  " + DateTime.Now.ToString() + System.Environment.NewLine);
            if (string.IsNullOrEmpty(txtFromDate.Text))
            {
                Alert.Show("From Date not selected");
                return;
            }

            if (string.IsNullOrEmpty(txtToDate.Text))
            {
                Alert.Show("To Date not selected");
                return;
            }



            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    //File.AppendAllText(logFile, "Calling SP PROC_GET_HDC_MIS_DOWNLOAD  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    //SqlCommand cmd = new SqlCommand("PROC_GET_HDC_MIS_DOWNLOAD", con);
                    File.AppendAllText(logFile, "Calling SP PROC_GET_REPLICA_HDC_MIS_DOWNLOAD  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    SqlCommand cmd = new SqlCommand("PROC_GET_REPLICA_HDC_MIS_DOWNLOAD", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FromDate", txtFromDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    File.AppendAllText(logFile, "Data stored into datatable " + DateTime.Now.ToString() + System.Environment.NewLine);
                    string Path = string.Format(ConfigurationManager.AppSettings["HDCExcelPath"].ToString(), DateTime.Now.ToString("ddMMyyyyhhmmss"));
                    File.AppendAllText(logFile, " Filename generated as " + Path.ToString() + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    File.AppendAllText(logFile, " Calling method ExportDataTableToExcel  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    //ExportDataTableToExcel(dt, "Payment Process Entry", Path);

                    if (dt.Rows.Count > 0)
                    {
                        string filePath = Server.MapPath("~/Reports");
                        string _DownloadableProductFileName = "HDC_MIS_REPORT_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls";
                        String strfilename = filePath + "\\" + _DownloadableProductFileName;
                        File.AppendAllText(logFile, " Downloadable Product FileName   " + _DownloadableProductFileName + DateTime.Now.ToString() + System.Environment.NewLine);

                        if (ExportDataTableToExcel(dt, "HDC MIS REPORT", strfilename) == true)
                        {
                            DownloadFile(strfilename);
                        }

                    }
                    else
                    {
                        Alert.Show("No record found for selected criteria");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                Alert.Show("Some Error Occured. Kindly Contact Administrator");
                ExceptionUtility.LogException(ex, "btnDownload_Click in FrmHDCPaymentDownload ");
            }

        }

        private bool DownloadFile(string strfilename)
        {
            File.AppendAllText(logFile, " File Downloading started   " + strfilename+"  " + DateTime.Now.ToString() + System.Environment.NewLine);
            bool res = false;
            try
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
                if (i < maxCount)
                {
                    res= false;
                }
                else {
                    res = true;
                }

                //Close Binary reader and File stream
                _BinaryReader.Close();
                myFile.Close();
                return res;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "DownloadFile "+strfilename);
            }
            return res;
        }

        private bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        {
            //Creae an Excel application instance
            DataTable DtPaymentProcess = dataTable;
            ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
            ExcelRange oRng;
            File.AppendAllText(logFile, " Initiated Excel variables " + DateTime.Now.ToString() + System.Environment.NewLine);
            try
            {
                // Workk sheet
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheet.Name = worksheetName;

                File.AppendAllText(logFile, " Excel worksheet initiated" + DateTime.Now.ToString() + System.Environment.NewLine);

                // loop through each row and add values to our sheet

                for (int i = 1; i < DtPaymentProcess.Columns.Count + 1; i++)
                {
                    excelSheet.Cells[1, i].Value = DtPaymentProcess.Columns[i - 1].ColumnName;
                }

                for (int j = 0; j < DtPaymentProcess.Rows.Count; j++)
                {
                    for (int k = 0; k < DtPaymentProcess.Columns.Count; k++)
                    {
                        excelSheet.Cells[j + 2, k + 1].Value = DtPaymentProcess.Rows[j].ItemArray[k].ToString();
                    }
                }

                File.AppendAllText(logFile, " Data added to all cells " + DateTime.Now.ToString() + System.Environment.NewLine);

                // now we resize the columns

                File.AppendAllText(logFile, " Code statrted for ExcelRange " + DateTime.Now.ToString() + System.Environment.NewLine);
                oRng = (ExcelRange)excelSheet.Cells[3, dataTable.Columns.Count];
                oRng.AutoFitColumns();
                BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));
                File.AppendAllText(logFile, " BorderAround executed  " + DateTime.Now.ToString() + System.Environment.NewLine);
                oRng = (ExcelRange)excelSheet.Cells[3, dataTable.Columns.Count];
                //FormattingExcelCells(oRng, "#ffffff", System.Drawing.Color.White, true);


                //now save the workbook and exit Excel

                excelApp.Save();
                File.AppendAllText(logFile, " Excel file saved " + DateTime.Now.ToString() + System.Environment.NewLine);
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

    }
}