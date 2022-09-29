using Microsoft.Practices.EnterpriseLibrary.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Google.Apis.Urlshortener.v1;
using System.Text.RegularExpressions;
using PrjPASS;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1.Data;
using System.Net.Mail;
using System.Net.Mime;

namespace ProjectPASS
{
    public partial class FrmHealthRenewalPolicyUpload : System.Web.UI.Page
    {
        private static UrlshortenerService service;
        private static readonly Regex ShortUrlRegex =
                   new Regex("^http[s]?://goo.gl/", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool IsShortUrl(string url)
        {
            return ShortUrlRegex.IsMatch(url);
        }

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
            string allowedExtensions = ".xlsx";

            // Added by Rohit on 13/02/2018

            if (String.IsNullOrEmpty(FileUpload1.FileName.ToString()))
            {
                Alert.Show("Please Select valid excel file or Excel file not selected");
                return;
            }
            if (!FileUpload1.HasFile)
            {
                Alert.Show("Please select valid excel file for upload");
                Session["ErrorCallingPage"] = "FrmHealthRenewalUpload.aspx";
                string vStatusMsg = "Please select valid excel file for upload";
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
            }           

            // Added by Rohit on 13/02/2018 End here

            if (FileUpload1.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                if (fileExtension != allowedExtensions)
                {
                    Session["ErrorCallingPage"] = "FrmHealthRenewalPolicyUpload.aspx";
                    string vStatusMsg = "Invalid file Extension, Only XLSX files are allowed to be Uploaded";
                    Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                    return;
                }

            }
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cYearMonth = "", vUploadId = "";


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

            vUploadId = wsDocNo.fn_Gen_Doc_Master_No("RUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
            // _con.Close();

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

                    bool GetMappingData = false;



                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {

                        oda.Fill(dtExcelData);

                    }

                    excel_con.Close();

                    if (dtExcelData.Rows.Count > 0)
                    {

                        if (dtExcelData != null)
                        {
                            foreach (DataRow row in dtExcelData.Rows)
                            {
                                row["UploadId"] = vUploadId;
                            }
                        }


                        if (dtExcelData != null)
                        {
                            if (dtExcelData.Rows.Count > 0)
                            {
                                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;


                                using (SqlConnection con = new SqlConnection(consString))
                                {

                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                    {

                                        //Set the database table name

                                        sqlBulkCopy.DestinationTableName = "dbo.TBL_HEALTH_RENEWAL_POLICY_TABLE";

                                        string sqlCommand = "SELECT  * FROM TBL_HEALTH_RENEWAL_COLUMN_MAPPING_MASTER";
                                        DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        DataSet ds = null;
                                        ds = dbCOMMON.ExecuteDataSet(dbCommand);

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            foreach (DataRow row in ds.Tables[0].Rows)
                                            {
                                                sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                                            }
                                        }
                                        con.Open();
                                        sqlBulkCopy.WriteToServer(dtExcelData);
                                        con.Close();
                                    }
                                }

                                //manish
                                foreach (DataRow excelrow in dtExcelData.Rows)
                                {
                                    string PayuURL = string.Empty;
                                    string PayuURLShorted = string.Empty;
                                    string ErrorMessage = string.Empty;

                                    CreateInvoiceLink(excelrow[dtExcelData.Columns["Policy_Number"].ToString().Trim()].ToString(), excelrow[dtExcelData.Columns["New Total"].ToString().Trim()].ToString(), excelrow[dtExcelData.Columns["Mobile_Number"].ToString().Trim()].ToString(), excelrow[dtExcelData.Columns["Email_ID"].ToString().Trim()].ToString(), excelrow[dtExcelData.Columns["Proposer_Full_Name"].ToString().Trim()].ToString(), DateTime.Now.ToString("MM/dd/yyyy"), excelrow[dtExcelData.Columns["Policy_End_Date"].ToString().Trim()].ToString(), ref PayuURL, ref PayuURLShorted, ref ErrorMessage);

                                    if (String.IsNullOrEmpty(ErrorMessage))
                                    {
                                        UpdateInvoiceLink(excelrow[dtExcelData.Columns["Policy_Number"].ToString().Trim()].ToString(), PayuURL, vUploadId, PayuURLShorted);
                                    }
                                    else
                                    {
                                        UpdateInvoiceLinkError(excelrow[dtExcelData.Columns["Policy_Number"].ToString().Trim()].ToString(), ErrorMessage, vUploadId, PayuURLShorted);
                                    }
                                }

                                SendMailForRenewalDump(vUploadId);



                            }
                        }
                    }
                    else
                    {
                        //sqlCommand = "Delete from TBL_GPA_POLICY_TABLE_TEMP where vuploadId ='" + vUploadId + "'";
                        //dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                        //dbCOMMON.ExecuteNonQuery(dbCommand);
                        //_tran.Rollback();
                        //Session["ErrorCallingPage"] = "FrmGPACancelUpload.aspx";
                        //string vStatusMsg = "No Policy Record for Upload";
                        //Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        return;
                    }
                }
                _tran.Commit();
                Session["ErrorCallingPage"] = "FrmHealthRenewalPolicyUpload.aspx";
                string vStatusMsg1 = "Renewal Policy Uploaded with Upload Id  " + vUploadId;
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg1, false);
                return;
            }
            catch (Exception ex)
            {
                //string sqlCommand = "Delete from TBL_GPA_POLICY_TABLE_TEMP where vuploadId ='" + vUploadId + "'";
                //DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                //dbCOMMON.ExecuteNonQuery(dbCommand);
                //_tran.Rollback();
                Session["ErrorCallingPage"] = "FrmHealthRenewalPolicyUpload.aspx";
                string vStatusMsg = "No Records for Upload or " + ex.Message.ToString().Replace("\r\n", "");
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
                //log the error
            }
        }

        private void SendMailForRenewalDump(string vUploadId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString))
                {
                    con.Open();
                    string query = "select * from TBL_HEALTH_RENEWAL_POLICY_TABLE where vUploadId = '" + vUploadId + "'";
                    SqlCommand command = new SqlCommand(query, con);
                    command.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string filePath = Server.MapPath("~/Reports");

                        //  string filePath = @"D:\Manish\Reports";

                        string _DownloadableProductFileName = "HEALTH_RENEWAL_POLICY_UPLOAD_DUMP_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".xls";

                        String strfilename = filePath + "\\" + _DownloadableProductFileName;

                        if (System.IO.File.Exists(strfilename))
                        {
                            System.IO.File.Delete(strfilename);
                        }

                        if (ExportDataTableToExcel(dt, "HEALTH_RENEWAL_POLICY_UPLOAD_DUMP", strfilename) == true)
                        {
                            string strPath = string.Empty;
                            string MailBody = string.Empty;
                            SmtpClient smtpClient = new SmtpClient();
                            smtpClient.Port = 25;
                            smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"].ToString();
                            smtpClient.Timeout = 3600000;
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"].ToString(), ConfigurationManager.AppSettings["smtp_mail_password"].ToString());

                            MailMessage mm = new MailMessage();
                            mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"].ToString());
                            mm.Subject = "Renewal Data Dump";
                            mm.Body = MailBody;
                            //mm.Body = "mail";
                            mm.IsBodyHtml = true;
                            mm.To.Add(ConfigurationManager.AppSettings["smtp_mail_ToMailId"].ToString());

                            mm.BodyEncoding = UTF8Encoding.UTF8;
                            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;



                            string attachmentFilename = strfilename;

                            Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                            ContentDisposition disposition = attachment.ContentDisposition;
                            disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                            disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                            disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                            disposition.FileName = Path.GetFileName(attachmentFilename);
                            disposition.Size = new FileInfo(attachmentFilename).Length;
                            disposition.DispositionType = DispositionTypeNames.Attachment;

                            mm.Attachments.Add(attachment);
                            smtpClient.Send(mm);

                        }
                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SendMailForRenewalDump");
            }
        }



        private void UpdateInvoiceLinkError(string policyNumber, string errorMessage, string uploadId, string payuURLshort)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("PROC_UPDATE_HEALTH_RENEWAL_INVOICE_ERR_DATA", con);
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                    command.Parameters.AddWithValue("@vPolicyNumber", policyNumber);
                    command.Parameters.AddWithValue("@vInvoiceLinkError", errorMessage);
                    command.Parameters.AddWithValue("@vUploadId", uploadId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "UpdateInvoiceLinkError");
            }
        }

        private void UpdateInvoiceLink(string policyNumber, string payuURL, string uploadId, string payuURLshort)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("PROC_UPDATE_HEALTH_RENEWAL_INVOICE_DATA", con);
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                    command.Parameters.AddWithValue("@vPolicyNumber", policyNumber);
                    command.Parameters.AddWithValue("@vInvoiceLink", payuURL);
                    command.Parameters.AddWithValue("@vUploadId", uploadId);
                    command.Parameters.AddWithValue("@vShortLink", payuURLshort);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "UpdateInvoiceLink");
            }
        }

        private void CreateInvoiceLink(string policyNumber, string totPremium, string Mobile, string Email, string CustomerName, string noticeDate, string policyEndDate, ref string PayuURL, ref string shortURL, ref string responseMsg)
        {
            try
            {
                string url = string.Empty;
                string GoogleURL = string.Empty;

                //double valid = (Convert.ToDateTime(policyEndDate) - Convert.ToDateTime(noticeDate)).TotalDays;
                //if (valid > 30)
                //{
                //    valid = 30;
                //}
                //string validation = (Convert.ToDateTime(policyEndDate) - Convert.ToDateTime(noticeDate)).TotalDays.ToString();
                string validation = "30";

                //  string validation = valid.ToString();

                var obj = new CreateInvoice
                {
                    amount = totPremium
                  ,
                    txnid = policyNumber
                  ,
                    productinfo = "Created By " + Session["vUserLoginId"].ToString().ToUpper() + " " + Session["vUserLoginDesc"].ToString().ToUpper() //productinfo = ProductInfo
                  ,
                    firstname = CustomerName
                  ,
                    email = Email
                  ,
                    phone = Mobile
                  ,
                    validation_period = validation
                  ,
                    address1 = ""
                  ,
                    city = ""
                  ,
                    state = ""
                  ,
                    country = ""
                  ,
                    zipcode = ""
                  ,
                    send_email_now = "0"
                  ,
                    send_sms = "0"

                };
                var json = new JavaScriptSerializer().Serialize(obj);

                string Url = ConfigurationManager.AppSettings["PayUWebService"].ToString();

                string method = "create_invoice";
                string salt = ConfigurationManager.AppSettings["salt"].ToString();
                string key = ConfigurationManager.AppSettings["key"].ToString();
                //string var1 = "{"amount":"10","txnid":"abc3332","productinfo":"jnvjrenv","firstname":"test","email":"test @test.com","phone":"1234567890","address1":"testaddress","city":"test","state":"test","country":"test","zipcode":"122002","template_id":"14","validation_period":6,"send_email_now":"1"}";
                string var1 = json; //"{'amount':'10','txnid':'abc3332','productinfo':'jnvjrenv','firstname':'Hasmukh','email':'kgi.hasmukh-jain@kotak.com','phone':'7738284116','address1':'testaddress','city':'test','state':'test','country':'test','zipcode':'122002','template_id':'14','validation_period':1,'send_email_now':'1'}";
                string var2 = " ";//TokenID of the merchant
                string var3 = " ";//Amount to be use in refund

                string toHash = key + "|" + method + "|" + var1 + "|" + salt;

                string Hashed = Generatehash512(toHash);

                string postString = "key=" + key +
                    "&command=" + method +
                    "&hash=" + Hashed +
                    "&var1=" + var1 +
                    "&var2=" + var2 +
                    "&var3=" + var3 +
                    "&udf1=" + "testUDF";

                string IsUseNetworkProxy = ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString();

                string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();

                string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();
                string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();

                WebRequest myWebRequest = WebRequest.Create(Url);
                if (IsUseNetworkProxy == "1")
                {
                    WebProxy proxy = new WebProxy(proxy_Address_Full);
                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;
                    myWebRequest.Proxy = proxy;

                    /////
                    myWebRequest.Proxy = WebRequest.DefaultWebProxy;
                    myWebRequest.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    myWebRequest.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                }

                myWebRequest.Method = "POST";
                myWebRequest.ContentType = "application/x-www-form-urlencoded";
                myWebRequest.Timeout = 180000;
                StreamWriter requestWriter = new StreamWriter(myWebRequest.GetRequestStream());
                requestWriter.Write(postString);
                requestWriter.Close();

                StreamReader responseReader = new StreamReader(myWebRequest.GetResponse().GetResponseStream());
                WebResponse myWebResponse = myWebRequest.GetResponse();
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);

                string response = readStream.ReadToEnd();
                if (IsJson(response))
                {
                    JObject account = JObject.Parse(response);
                    Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                    url = values["URL"];

                    Uri uriResult;
                    bool IsValidURL = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (IsValidURL)
                    {
                        PayuURL = url;
                        if (true)
                        {
                            GoogleURL = string.Empty;
                            GoogleURLShortner(url, out GoogleURL);

                            Uri uriResultshort;
                            bool IsValidURLshort = Uri.TryCreate(GoogleURL, UriKind.Absolute, out uriResultshort)
                                && (uriResultshort.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                            if (IsValidURLshort)
                            {
                                shortURL = GoogleURL;
                            }
                        }
                    }
                    else
                    {
                        responseMsg = response;
                    }
                }
                else
                {
                    responseMsg = response;
                }
            }

            catch (JsonReaderException jex)
            {
                responseMsg = jex.Message;
                ExceptionUtility.LogException(jex, "CreateInvoiceLink");
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "CreateInvoiceLink");
            }

        }

        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        private void GoogleURLShortner(string longURL, out string shortURL)
        {
            shortURL = string.Empty;
            // If we did not construct the service so far, do it now.
            if (service == null)
            {
                BaseClientService.Initializer initializer = new BaseClientService.Initializer();
                // You can enter your developer key for services requiring a developer key.
                initializer.ApiKey = "AIzaSyAUDi9QAlEmAPy1lhPwfEHxDeXiQtnKgUI";
                service = new UrlshortenerService(initializer);
            }

            string url = longURL;
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            // Execute methods on the UrlShortener service based upon the type of the URL provided.
            try
            {
                string resultURL;
                if (IsShortUrl(url))
                {
                    // Expand the URL by using a Url.Get(..) request.
                    Url result = service.Url.Get(url).Execute();
                    resultURL = result.LongUrl;
                }
                else
                {
                    // Shorten the URL by inserting a new Url.
                    Url toInsert = new Url { LongUrl = url };
                    toInsert = service.Url.Insert(toInsert).Execute();
                    resultURL = toInsert.Id;
                }
                shortURL = resultURL;
                //string s = string.Format("<a href=\"{0}\">{0}</a>", resultURL);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            string sqlCommand = "select * from TBL_HEALTH_RENEWAL_COLUMN_MAPPING_MASTER";
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

            string _DownloadableProductFileName = "HEALTH_RENEWAL_UPLOAD_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "HEALTH_RENEWAL_UPLOAD_SHEET", strfilename) == true)
            {
                DownloadFile(strfilename);
            }
        }

        protected string[] Fn_Check_Business_Validation(string vFieldName, string vFieldValue)
        {
            string[] ckvalidflag = new string[2];
            ckvalidflag[0] = "true";
            ckvalidflag[1] = " ";

            //if (vFieldName == "vCertificateNo")
            //{
            //    if (vFieldValue.Length < 5)
            //    {
            //        ckvalidflag[0] = "false";
            //        ckvalidflag[1] = "Policy Id Lenght is Less then 5";
            //    }
            //}

            return ckvalidflag;
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