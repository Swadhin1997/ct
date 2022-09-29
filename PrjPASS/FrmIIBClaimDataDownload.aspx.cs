using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using Microsoft.Office.Interop.Excel;
using System.Globalization;
using PrjPASS;

namespace ProjectPASS
{
    public partial class FrmIIBClaimDownload : System.Web.UI.Page
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
        protected void btnGetClaimData_Click(object sender, EventArgs e)
        {
            try
            {
                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                using (SqlConnection sqlCon = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string strQuery = "SELECT [ID] ,[vRegNo],[vChasisNo],[vEngineNo],[vInsurerName],[vTypeOfClaim], FORMAT ([dDateOfLoss], 'dd/MM/yyyy ') dDateOfLoss,FORMAT([dClaimIntimationDate], 'dd/MM/yyyy ') dClaimIntimationDate,[vODClaimsPaid],[vOdOpenClaimProvison],[vOdCloseClaimProvison],[vWhetherTotalLossClaim],[vWhetherTheftClaim],[vTotalTPClaimsPaid],[vTpOpenClaimProvison],[vClaimstatus],[vExpensesPaid],[vErrorCode],[vWarningmessage],FORMAT([dCreatedDate], 'dd/MM/yyyy ') dCreatedDate,FORMAT([dModifiedDate], 'dd/MM/yyyy ') dModifiedDate ,[vCreatedBy] FROM [dbo].[tbl_IIB_CLAIM_DATA] A";

                        string strCondition = string.Empty;

                        if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
                        {
                            strCondition += " vCreatedBy ='" + Session["vUserLoginId"].ToString().ToUpper() + "' and";
                        }

                        if (txtRegNo.Text.Trim() != "")
                        {
                            strCondition += " vRegNo ='" + txtRegNo.Text.Trim() + "' and";
                        }
                        if (txtChassisNo.Text.Trim() != "")
                        {
                            strCondition += " vChasisNo='" + txtChassisNo.Text.Trim() + "' and";
                        }
                        if (txtEngineNo.Text.Trim() != "")
                        {
                            strCondition += " vEngineNo='" + txtEngineNo.Text.Trim() + "' and";
                        }
                        string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString();
                        string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();

                        if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
                        {
                            strCondition += " DateAdd(day, datediff(day, 0, dCreatedDate), 0) between Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ")";
                        }
                        else if (txtFromDate.Text.Trim() != "")
                        {
                            strCondition += " DateAdd(day, datediff(day, 0, dCreatedDate), 0)>= Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and";
                        }
                        else if (txtToDate.Text.Trim() != "")
                        {
                            strCondition = " DateAdd(day, datediff(day, 0, dCreatedDate), 0)<= Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ") and";
                        }

                        if (strCondition.Trim() != "")
                        {
                            strQuery = strQuery + " Where" + strCondition.TrimEnd(new char[] { 'a', 'n', 'd' });
                        }
                        strQuery += " UNION ALL SELECT [ID] ,[vRegNo],[vChasisNo],[vEngineNo],[vInsurerName],[vTypeOfClaim], FORMAT ([dDateOfLoss], 'dd/MM/yyyy ') dDateOfLoss,FORMAT([dClaimIntimationDate], 'dd/MM/yyyy ') dClaimIntimationDate,[vODClaimsPaid],[vOdOpenClaimProvison],[vOdCloseClaimProvison],[vWhetherTotalLossClaim],[vWhetherTheftClaim],[vTotalTPClaimsPaid],[vTpOpenClaimProvison],[vClaimstatus],[vExpensesPaid],[vErrorCode],[vWarningmessage],FORMAT([dCreatedDate], 'dd/MM/yyyy ') dCreatedDate,FORMAT([dModifiedDate], 'dd/MM/yyyy ') dModifiedDate ,[vCreatedBy] FROM [dbo].[tbl_IIB_CLAIM_DATA_ERROR] B";

                        if (strCondition.Trim() != "")
                        {
                            strQuery = strQuery + " Where" + strCondition.TrimEnd(new char[] { 'a', 'n', 'd' });
                        }
                        //if (txtFromDate.Text == "" && txtChassisNo.Text.Trim() != " ")
                        //{
                        //    Alert.Show("Please Select From Date.");
                        //    return;
                        //}
                        //if (txtToDate.Text == "" && txtChassisNo.Text.Trim() != " ")
                        //{
                        //    Alert.Show("Please Select To Date.");
                        //    return;
                        //}

                        cmd.CommandText = strQuery;
                        cmd.Connection = sqlCon;
                        sqlCon.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        da.Fill(dt);
                        sqlCon.Close();

                        //POLICY DATA
                        strQuery = "SELECT [regNo],[chasisNo],[engineNo],[policyNo],[policyStatus],[termOfPolicy],[has90DaysCrossedAfterExpiryDate],[riskExpiryDate],FORMAT([dCreatedDate], 'dd/MM/yyyy ') dCreatedDate,[vCreatedBy] FROM [dbo].[tbl_IIB_CLAIM_DATA_POLICY]";

                        strCondition = string.Empty;

                        if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
                        {
                            strCondition += " vCreatedBy ='" + Session["vUserLoginId"].ToString().ToUpper() + "' and";
                        }
                        if (txtRegNo.Text.Trim() != "")
                        {
                            strCondition += " regNo ='" + txtRegNo.Text.Trim() + "' and";
                        }
                        if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
                        {
                            strCondition += " DateAdd(day, datediff(day, 0, dCreatedDate), 0) between Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ")";
                        }
                        else if (txtFromDate.Text.Trim() != "")
                        {
                            strCondition += " DateAdd(day, datediff(day, 0, dCreatedDate), 0)>= Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and";
                        }
                        else if (txtToDate.Text.Trim() != "")
                        {
                            strCondition = " DateAdd(day, datediff(day, 0, dCreatedDate), 0)<= Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ") and";
                        }
                        if (strCondition.Trim() != "")
                        {
                            strQuery = strQuery + " Where" + strCondition.TrimEnd(new char[] { 'a', 'n', 'd' });
                        }
                        cmd.CommandText = strQuery;
                        cmd.Connection = sqlCon;
                        sqlCon.Open();
                        da = new SqlDataAdapter(cmd);
                        System.Data.DataTable dtPolicy = new System.Data.DataTable();
                        da.Fill(dtPolicy);

                        if (dt.Rows.Count > 0 || dtPolicy.Rows.Count > 0)
                        {
                            string filePath = Server.MapPath("~/Reports");

                            //string _DownloadableProductFileName = "IIB_CLAIMS_DATA _"+ DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".xls";
                            string _DownloadableProductFileName = "IIB_CLAIMS_AND_POLICY_DATA _" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xls";
                            String strfilename = filePath + "\\" + _DownloadableProductFileName;

                            if (System.IO.File.Exists(strfilename))
                            {
                                System.IO.File.Delete(strfilename);
                            }

                            if (ExportDataTableToExcel(dt, dtPolicy, "IIB_CLAIMS_DATA", strfilename) == true)
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
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
            }
        }
        private bool ExportDataTableToExcel(System.Data.DataTable dt, System.Data.DataTable dtPolicy,string worksheetName, string saveAsLocation)
        {
            //Creae an Excel application instance

            ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
            ExcelRange oRng;
            
            try
            {
                // Workk sheet
                worksheetName = "Claims";
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheet.Name = worksheetName;

                excelSheet.Cells[1, 1].Value = "IIB Claims Data Details";
                excelSheet.Cells[2, 1].Value = "Report Taken On Date : " + DateTime.Now.ToString("dd/MM/yyyy");

                // loop through each row and add values to our sheet
                int rowcount = 2;
                System.Data.DataTable dataTable = dt;
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
                oRng = (ExcelRange)excelSheet.Cells[3, dataTable.Columns.Count];
                //FormattingExcelCells(oRng, "#000099", System.Drawing.Color.White, true);
                //now save the workbook and exit Excel


                #region Policy Data Tab
                // Workk sheet
                worksheetName = "Policy";
                var excelSheetPolicy = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheetPolicy.Name = worksheetName;

                excelSheetPolicy.Cells[1, 1].Value = "IIB Policy Data Details";
                excelSheetPolicy.Cells[2, 1].Value = "Report Taken On Date : " + DateTime.Now.ToString("dd/MM/yyyy");

                // loop through each row and add values to our sheet
                int rowcount1 = 2;
                
                foreach (DataRow datarow in dtPolicy.Rows)
                {
                    rowcount1 += 1;

                    for (int i = 1; i <= dtPolicy.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount1 == 3)
                        {
                            excelSheetPolicy.Cells[3, i].Value = dtPolicy.Columns[i - 1].ColumnName;
                            // excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
                        }
                        excelSheetPolicy.Cells[rowcount1 + 1, i].Value = datarow[i - 1].ToString();
                        //for alternate rows
                        if (rowcount1 > 2)
                        {
                            if (i == dtPolicy.Columns.Count)
                            {
                                if (rowcount1 % 2 == 0)
                                {
                                    oRng = (ExcelRange)excelSheetPolicy.Cells[rowcount1, dtPolicy.Columns.Count];
                                    //FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
                                }
                            }
                        }
                    }
                }
                // now we resize the columns
                oRng = (ExcelRange)excelSheetPolicy.Cells[3, dtPolicy.Columns.Count];
                oRng.AutoFitColumns();
                BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));
                oRng = (ExcelRange)excelSheetPolicy.Cells[3, dtPolicy.Columns.Count];

                #endregion
               
                excelApp.Save();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "");
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
            bool flag = false;
            try
            {
                string filePath = Server.MapPath("~/Reports");
                string _DownloadableProductFileName = strfilename;

                System.IO.FileInfo FileName = new System.IO.FileInfo(strfilename);
                using (FileStream myFile = new FileStream(strfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
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
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                    return flag;

                    //Close Binary reader and File stream
                    _BinaryReader.Close();
                    myFile.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                return flag;
            }
          }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}