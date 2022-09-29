using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

using Microsoft.Office.Interop.Excel;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

using System.Runtime.InteropServices;


using System.Net.Mail;
using System.Net.Mime;


namespace PrjPASS
{
    public partial class FrmSMSAndDailyReports : System.Web.UI.Page
    {
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRepeater();
                //SendDaily_MINI_GIST_DOCS_DUMP_Report();
            }
        }

        private void BindRepeater()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnConnect"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_GET_TRANS_SMS_LOG_DATA";


                        string MessageType = "";

                        if (rbtnClaimCount.Checked)
                        {
                            MessageType = "Car Secure Claims";
                        }
                        else if (rbtnTotalPremium.Checked)
                        {
                            MessageType = "Policy issuance count from GIST";
                        }
                        else if (rbtnQuoteCount.Checked)
                        {
                            MessageType = "Quote Count Mobile";
                        }

                        string DeliveryStatus = rbtnTimeOut.Checked ? "TimeOut" : "Other";
                        string Duration = drpDuration.SelectedValue;

                        cmd.Parameters.AddWithValue("@MessageType", MessageType);
                        cmd.Parameters.AddWithValue("@DeliveryStatus", DeliveryStatus);
                        cmd.Parameters.AddWithValue("@Duration", Duration);

                        cmd.Connection = conn;
                        conn.Open();

                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        dt.Load(dr);

                        rptSMSData.DataSource = dt;
                        rptSMSData.DataBind();

                        conn.Close();

                        if (dt != null)
                        {
                            if (dt.Rows.Count <= 0)
                            {
                                btnSendSMS.Visible = false;
                            }
                            else
                            {
                                btnSendSMS.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Error Occured, " + ex.Message);
                ExceptionUtility.LogException(ex, "BindRepeater Method FrmSMSAndDailyReports");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
            BindRepeater();
        }

        protected void btnSendSMS_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("sno", typeof(System.String));

            foreach (RepeaterItem item in rptSMSData.Items)
            {
                System.Web.UI.WebControls.CheckBox chkRow = item.FindControl("chkRow") as System.Web.UI.WebControls.CheckBox;
                HiddenField hdnsno = item.FindControl("hdnsno") as HiddenField;

                if (chkRow != null)
                {
                    if (chkRow.Checked)
                    {
                        //call update trans sms table status to 0
                        dt.Rows.Add(hdnsno.Value);
                    }
                }
            }

            UpdateStatusInTransSMSLogTable(dt);

            BindRepeater();
        }

        private void UpdateStatusInTransSMSLogTable(System.Data.DataTable dtSNO)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnConnect"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_UPDATE_TRANS_SMS_LOG_STATUS";

                        cmd.Parameters.AddWithValue("@xml", GetXML(dtSNO));
                        cmd.Parameters.AddWithValue("@hdoc", 0);

                        cmd.Connection = conn;
                        conn.Open();

                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        dt.Load(dr);

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Error Occured, " + ex.Message);
                ExceptionUtility.LogException(ex, "UpdateStatusInTransSMSLogTable Method FrmSMSAndDailyReports");
            }
        }

        private string GetXML(System.Data.DataTable dt)
        {
            StringBuilder xml = new StringBuilder(string.Empty);
            try
            {
                DataColumnCollection columns = dt.Columns;
                xml.Append("<Table>");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["sno"].ToString().Length > 0)
                    {
                        xml.Append("<Row");
                        xml.Append(string.Format(" sno = \"{0}\"", columns.Contains("sno") ? dr["sno"].ToString() : ""));
                        xml.Append(" />");
                    }
                }
                xml.Append("</Table>");

                return xml.ToString();
            }
            finally
            {
                xml = null;
            }
        }

        public void SendDaily_MINI_GIST_DOCS_DUMP_Report()
        {
            try
            {
                string SQLQuery = "EXEC PROC_GET_MINI_GIST_DOCS_DUMP_FOR_BUSINESS_SMS_APP";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString);
                SqlCommand cmd = new SqlCommand(SQLQuery, con);
                cmd.CommandTimeout = 7200;
                DataSet ds = new DataSet("myDataset");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                //Console.WriteLine("Executing DB Proc = PROC_GET_MINI_GIST_DOCS_DUMP_FOR_BUSINESS_SMS_APP");

                da.Fill(ds);

                //Console.WriteLine("Saving Dataset to Excel..................");

                string FilePath = SaveExcel_Mini_Gist_Docs(ds);

                //Console.WriteLine("Saving Dataset to Excel........File Path: " + FilePath);
                if (!string.IsNullOrEmpty(FilePath))
                {
                    string ToEmailIds = ConfigurationManager.AppSettings["KotakBossEmailIds_ForMiniGISTDocs"].ToString();

                    Console.WriteLine("Sending Mail To: " + ToEmailIds);

                    //For MiniGIST sanjay sir told not to attache file
                    SendEmail_Mini_GIST(ToEmailIds, FilePath);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SendDaily_MINI_GIST_DOCS_DUMP_Report Method FrmSMSAndDailyReports");
            }
        }

        public string SaveExcel_Mini_Gist_Docs(DataSet ds)
        {
            string ExcelFileName = "MiniGISTDocsDumpReport";
            string date = string.Format("{0:ddMMyyyy}", DateTime.Now);
            date = date + DateTime.Now.Millisecond.ToString();

            string FolderName = string.Format("{0:dd-MMM-yyyy}", DateTime.Now);
            bool IsFolderExists = Directory.Exists(@"\\KGI-P-FILE-1-V005\kgi-datadump$\ImportantReports\" + FolderName);
            if (!IsFolderExists)
            {
                Directory.CreateDirectory(@"\\KGI-P-FILE-1-V005\kgi-datadump$\ImportantReports\" + FolderName);
            }

            string network_sharedrivepath = System.Configuration.ConfigurationManager.AppSettings["network_sharedrivepath"].ToString();

            string FilePath = network_sharedrivepath + "\\" + FolderName + "\\" + ExcelFileName + "" + "_" + date + ".xlsx";

            //string date = string.Format("{0:ddMMyyyy}", DateTime.Now);
            //date = date + DateTime.Now.Millisecond.ToString();
            //string FilePath = "D:\\Reports\\MiniGISTDocsDumpReport" + "_" + date + ".xlsx";

            Application ExcelApp = new Application();
            Workbook ExcelWorkBook = null;
            Worksheet ExcelWorkSheet = null;

            ExcelApp.Visible = false;
            ExcelWorkBook = ExcelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

            List<string> SheetNames = new List<string>();
            SheetNames.Add("GISTDocsDumpReport");

            try
            {
                for (int i = 1; i < ds.Tables.Count; i++)
                    ExcelWorkBook.Worksheets.Add(); //Adding New sheet in Excel Workbook

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    int r = 1; // Initialize Excel Row Start Position  = 1

                    ExcelWorkSheet = ExcelWorkBook.Worksheets[i + 1];

                    //Writing Columns Name in Excel Sheet

                    for (int col = 1; col < ds.Tables[i].Columns.Count; col++)
                    {
                        ExcelWorkSheet.Cells[r, col] = ds.Tables[i].Columns[col - 1].ColumnName;
                        ExcelWorkSheet.Cells[r, col].Font.Bold = true;
                        ExcelWorkSheet.Cells[r, col].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                        if (ds.Tables[i].Columns.Count - 1 == col)
                        {
                            ExcelWorkSheet.Cells[r, col + 1] = ds.Tables[i].Columns[ds.Tables[i].Columns.Count - 1].ColumnName;
                            ExcelWorkSheet.Cells[r, col + 1].Font.Bold = true;
                            ExcelWorkSheet.Cells[r, col + 1].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                        }
                    }
                    r++;

                    //Writing Rows into Excel Sheet
                    for (int row = 0; row < ds.Tables[i].Rows.Count; row++) //r stands for ExcelRow and col for ExcelColumn
                    {
                        // Excel row and column start positions for writing Row=1 and Col=1
                        for (int col = 1; col < ds.Tables[i].Columns.Count; col++)
                        {
                            ExcelWorkSheet.Cells[r, col] = ds.Tables[i].Rows[row][col - 1].ToString();
                            if (ds.Tables[i].Columns.Count - 1 == col)
                            {
                                ExcelWorkSheet.Cells[r, col + 1] = ds.Tables[i].Rows[row][ds.Tables[i].Columns.Count - 1].ToString();
                            }
                        }
                        r++;
                    }
                    ExcelWorkSheet.Name = SheetNames[i];//Renaming the ExcelSheets

                }

                ExcelWorkBook.SaveAs(FilePath);
                ExcelWorkBook.Close();
                ExcelApp.Quit();
                Marshal.ReleaseComObject(ExcelWorkSheet);
                Marshal.ReleaseComObject(ExcelWorkBook);
                Marshal.ReleaseComObject(ExcelApp);
            }
            catch (Exception exHandle)
            {
                FilePath = string.Empty;
                ExceptionUtility.LogException(exHandle, "SaveExcel_Mini_Gist_Docs Method FrmSMSAndDailyReports");
                Environment.Exit(0);
            }
            finally
            {

                //foreach (Process process in Process.GetProcessesByName("Excel"))
                //process.Kill();
            }
            return FilePath;
        }

        private void SendEmail_Mini_GIST(string ToEmailIds, string FilePath)
        {
            string ActualToEmailIds = ToEmailIds;
            string smtp_DefaultCCMailId = ConfigurationManager.AppSettings["smtp_DefaultCCMailId"].ToString();

            string strMessage = string.Empty;
            string[] ToEmailAddr = ToEmailIds.Split(';');

            string smtp_Username = ConfigurationManager.AppSettings["smtp_Username"].ToString();
            string smtp_Password = ConfigurationManager.AppSettings["smtp_Password"].ToString();
            string smtp_Host = ConfigurationManager.AppSettings["smtp_Host"].ToString();
            string smtp_FromMailId = ConfigurationManager.AppSettings["smtp_FromMailId"].ToString();
            string strPath = string.Empty;
            string MailBody = string.Empty;

            //For MiniGIST sanjay sir told not to attache file
            string attachmentFilename = null; //FilePath;


            strPath = string.Empty;
            MailBody = string.Empty;

            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = smtp_Host; //"192.168.201.61"; //"kgirelay.kgi.kotakgroup.com";
                                         //client.EnableSsl = true;
                client.Timeout = 3600000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(smtp_Username, smtp_Password);

                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBody_MiniGIST.html";
                MailBody = File.ReadAllText(strPath);
                StringBuilder strTable = new StringBuilder();

                strTable.Append("<table style='border: 1px solid black;border-collapse: collapse;'>");
                strTable.Append("<th style='border: 1px solid black;text-align: left;padding: 10px;'>ReportName</th>");
                strTable.Append("<th style='border: 1px solid black;text-align: left;padding: 10px;'>FilePath</th>");

                string ExcelFileName = "MiniGISTDocsDumpReport";

                strTable.Append("<tr style='border: 1px solid black;text-align: left;padding: 10px;'>");
                strTable.Append("<td style='border: 1px solid black;text-align: left;padding: 10px;'>");
                strTable.Append(ExcelFileName);
                strTable.Append("</td>");
                strTable.Append("<td style='border: 1px solid black;text-align: left;padding: 10px;'>");
                strTable.Append(FilePath.Replace("\\\\\\", @"\\"));
                strTable.Append("</td>");
                strTable.Append("</tr>");

                strTable.Append("</table>");

                MailBody = MailBody.Replace("@Location", strTable.ToString());

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(smtp_FromMailId);
                mm.Subject = "Mini GIST Docs Dump Report";
                mm.Body = MailBody;
                mm.IsBodyHtml = true;

                foreach (var toMailId in ToEmailAddr)
                {
                    if (toMailId != null)
                    {
                        if (toMailId.Length > 9)
                        {
                            mm.To.Add(toMailId);
                        }
                    }
                }
                //mm.CC.Add(smtp_DefaultCCMailId);

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (attachmentFilename != null)
                {
                    Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mm.Attachments.Add(attachment);
                }

                client.Send(mm);
                strMessage = "success";
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SendEmail_Mini_GIST Method FrmSMSAndDailyReports");
                Environment.Exit(0);
            }
        }

        protected void btnDownloadMiniGist_Click(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
            try
            {
                MISCReportServiceReference.MISCReportServiceClient objProxy = new MISCReportServiceReference.MISCReportServiceClient();
                string Error = objProxy.GetMiniGistDocsDumpReport();
                if (Error.Trim().Length > 0)
                {
                    lblErrorMsg.Text = Error;
                }
                else
                {
                    Response.Write("<script>alert('Mail Sent.');</script>");
                }
            }
            catch (Exception ex)
            {
                lblErrorMsg.Text = ex.Message;
            }
        }
    }
}