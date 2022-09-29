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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmHDCChallanUpload : System.Web.UI.Page
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

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            TemplateTable.Columns.Add("Certificate No", typeof(string));
            TemplateTable.Columns.Add("Challan No", typeof(string));
            TemplateTable.Columns.Add("Challan Date", typeof(string));

            DataRow newBlankRow = TemplateTable.NewRow();
            TemplateTable.Rows.InsertAt(newBlankRow, 0);

            string filePath = Server.MapPath("~/Reports");

            string _DownloadableProductFileName = "CHALLAN_UPLOAD_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "CHALLAN_UPLOAD_SHEET", strfilename) == true)
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
            bool res = false;
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

            if (i < maxCount)
            {
                res = false;
            }
            else {
                res = true;
            }

            //Close Binary reader and File stream
            _BinaryReader.Close();
            myFile.Close();
            return res;

        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            string allowedExtensions = ".xlsx";

            if (FileUpload1.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                if (fileExtension != allowedExtensions)
                {
                    Session["ErrorCallingPage"] = "FrmHDCChallanUpload.aspx";
                    string vStatusMsg = "Invalid file Extension, Only XLSX files are allowed to be Uploaded";
                    Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                    return;
                }

            }

            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            try
            {
                //Upload and save the file

                string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(FileUpload1.PostedFile.FileName);

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

                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                    DataTable dtExcelData = new DataTable();

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {

                        oda.Fill(dtExcelData);
                    }

                    excel_con.Close();
                    if (dtExcelData.Rows.Count > 0)
                    {
                        List<string> InvalidCertNo = new List<string>();
                        List<string> lstChallanDate = new List<string>();
                        List<string> InvalidChallan = new List<string>();
                        List<string> lstCertNo = new List<string>();

                        for (int i = 0; i < dtExcelData.Rows.Count; i++)
                        {
                            string returnVal = "";
                            string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                            if (!string.IsNullOrEmpty(dtExcelData.Rows[i]["Certificate No"].ToString()))
                            {
                                using (SqlConnection sqlCon = new SqlConnection(consString))
                                {
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        cmd.CommandText = "PROC_UPDATE_HDC_CHALLAN_DETAILS";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@vCertificateNo", dtExcelData.Rows[i]["Certificate No"].ToString());
                                        cmd.Parameters.AddWithValue("@vChallanNo", dtExcelData.Rows[i]["Challan No"].ToString());
                                        cmd.Parameters.AddWithValue("@vChallanDate", dtExcelData.Rows[i]["Challan Date"].ToString());
                                        cmd.Parameters.Add("@UpdateStatus", SqlDbType.VarChar, 100);
                                        cmd.Parameters["@UpdateStatus"].Direction = ParameterDirection.Output;
                                        cmd.Connection = sqlCon;
                                        sqlCon.Open();
                                        cmd.ExecuteNonQuery();
                                        returnVal = cmd.Parameters["@UpdateStatus"].Value.ToString();
                                        if (returnVal == "0")
                                        {
                                            InvalidCertNo.Add(dtExcelData.Rows[i]["Certificate No"].ToString());
                                        }
                                        else if (returnVal == "Invalid Date")
                                        {
                                            lstChallanDate.Add(dtExcelData.Rows[i]["Certificate No"].ToString());
                                        }
                                        else if (returnVal == "Invalid Challan No")
                                        {
                                            InvalidChallan.Add(dtExcelData.Rows[i]["Certificate No"].ToString());
                                        }
                                        sqlCon.Close();
                                    }
                                }

                            }
                            else
                            {
                                lstCertNo.Add((i+1).ToString());

                            }

                        }
 
                        if (lstChallanDate.Count() > 0)
                        {
                            string vCertNoD = string.Join(",", lstChallanDate.ToArray());
                            Session["ErrorCallingPage"] = "FrmHDCChallanUpload.aspx";
                            string vStatusMsg = "Challan date of vCertificateNo " + vCertNoD + " not in valid format";
                            Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                            return;
                        }

                        if (InvalidChallan.Count() > 0)
                        {
                            string vCertNoD = string.Join(",", InvalidChallan.ToArray());
                            Session["ErrorCallingPage"] = "FrmHDCChallanUpload.aspx";
                            string vStatusMsg = "Challan No of vCertificateNo " + vCertNoD + " not in valid format";
                            Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                            return;
                        }

                        if (lstCertNo.Count() > 0)
                        {
                            string vCertNoD = string.Join(",", lstCertNo.ToArray());
                            Session["ErrorCallingPage"] = "FrmHDCChallanUpload.aspx";
                            string vStatusMsg = "Certificate No is blank at row number " + vCertNoD + " ";
                            Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                            return;
                        }

                        if (InvalidCertNo.Count() > 0)
                        {
                            string vCertNo = string.Join(",", InvalidCertNo.ToArray());
                            Session["ErrorCallingPage"] = "FrmHDCChallanUpload.aspx";
                            string vStatusMsg = "vCertificateNo " + vCertNo + " not found";
                            Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                            return;
                        }

                        Session["ErrorCallingPage"] = "FrmHDCChallanUpload.aspx";
                        string vUploadMsg = "Challan Details Updated Sucessfully";
                        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vUploadMsg, false);
                        return;
                    }
                    else
                    {
                        Session["ErrorCallingPage"] = "FrmHDCChallanUpload.aspx";
                        string vStatusMsg = "Upload Failed – Please provide certificate information";
                        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}