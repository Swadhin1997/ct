using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Obout.Grid;
//using Obout.ComboBox;
using ProjectPASS;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.Services;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net.Mime;

using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;

using System.ServiceModel.Activation;


using System.Web.Script.Serialization;

using Microsoft.VisualBasic;

using System.Net;
using System.Security.Cryptography;


using Google;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Urlshortener.v1.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.ModelBinding;
using System.Data.OleDb;
using System.Threading;
using OfficeInterop = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;


using OfficeOpenXml;
using OfficeOpenXml.Style;
using CCA.Util;
using System.Collections.Specialized;

namespace PrjPASS
{
    public class GenerateInvoiceResult
    {
        public string tiny_url { get; set; }
        public int invoice_id { get; set; }
        public string qr_code { get; set; }
        public int invoice_status { get; set; }
        public string error_desc { get; set; }
       
    }

    public class Root3
    {
        public GenerateInvoiceResult Generate_Invoice_Result { get; set; }
        public string error_desc { get; set; }
        public string invoice_id { get; set; }
        public string tiny_url { get; set; }
        public string qr_code { get; set; }
        public int invoice_status { get; set; }
        public string error_code { get; set; }
        public string merchant_reference_no { get; set; }

    }
    public class CCFile
    {
        public string name { get; set; }
        public string content { get; set; }
    }

    public class Root
    {
        public string customer_name { get; set; }

        public string bill_delivery_type { get; set; }
        public long customer_mobile_no { get; set; }
        public string customer_email_id { get; set; }
        public string customer_email_subject { get; set; }
        public string invoice_description { get; set; }
        public string currency { get; set; }
        public int valid_for { get; set; }
        public string valid_type { get; set; }
        public decimal amount { get; set; }
        public string merchant_reference_no { get; set; }
        public string merchant_reference_no1 { get; set; }
        public string merchant_reference_no2 { get; set; }
        public string merchant_reference_no3 { get; set; }
        public string merchant_reference_no4 { get; set; }
        public string redirect_url { get; set; }
        //public string cancel_url { get; set; }
       // public string sub_acc_id { get; set; }
        public string terms_and_conditions { get; set; }
        public string sms_content { get; set; }
       // public List<CCFile> files { get; set; }

    }

    public partial class FrmCreateInvoiceLinkKpay : System.Web.UI.Page
    {
        private static UrlshortenerService service;
        private static readonly Regex ShortUrlRegex =
                   new Regex("^http[s]?://goo.gl/", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        //#region sandeep Kpay

        //string mihpayid = string.Empty;
        //string status = string.Empty;
        //string unmappedstatus = string.Empty;
        //string txnid = string.Empty;
        //string PG_TYPE = string.Empty;
        //string bank_ref_num = string.Empty;
        //string bankcode = string.Empty;
        //string error_Message = string.Empty;
        //string email = string.Empty;
        //string mobile = string.Empty;
        //string produtinfo = string.Empty;
        //string amount = string.Empty;
        //string firstName = string.Empty;
        //string encryptStatus = string.Empty;
        //string encryptTxnId = string.Empty;
        //string intermediaryCode = string.Empty;
        //string quoteNumber = string.Empty;
        //string eposFlag = string.Empty;
        //string custID = string.Empty;
        //string intermediaryLocCode = string.Empty;
        //string intermediaryLocName = string.Empty;
        //string PaymentId_Entry = string.Empty;
        //string applicationNumber = string.Empty;
        //string paymentId_allocation = string.Empty;
        //string policyStartDate = string.Empty;
        //string proposalStartDate = string.Empty;
        //string paymentIdError = string.Empty;
        //string paymentAllocationError = string.Empty;
        //string paymentTagggingError = string.Empty;
        //string encResp = string.Empty;
        //string orderNo = string.Empty;
        //string crossSellUrl = string.Empty;
        //string order_id = string.Empty;
        //string tracking_id = string.Empty;
        //string bank_ref_no = string.Empty;



        //#endregion

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
                    //Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    //return;
                }

                //rbtIndividual.Checked = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposal();", true);

                //string script = "$(document).ready(function () { $('[id*=btnCreateInvoiceBulk]').click(); });";
                //ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);
            
            //#region sandeep redirect url
            //try
            //{


            //        CCACrypto ccaCrypto = new CCACrypto();
            //        string workingKey = "8C04E7175291C53474B024D5AC080F7A";
            //        //string workingKey = ConfigurationManager.AppSettings["workingKey"].ToString();
            //        //if (Session["loaded"] == null) //to check refresh
            //        //{
            //        //    Session["loaded"] = "1";

            //        // Directory.CreateDirectory(folderPath);

            //        string[] keys = Request.Form.AllKeys;

            //        for (int i = 0; i < keys.Length; i++)
            //        {
            //            if (keys[i] == "encResp")
            //            {
            //                encResp = Request.Form[keys[i]];
            //                // NameValueCollection param = getResponseMap(encResp);
            //                // string workingKey = ConfigurationManager.AppSettings["workingKey"].ToString();
            //                String ResJson = ccaCrypto.Decrypt(encResp, workingKey);
            //                string[] segments = ResJson.Split('&');
                           
            //                order_id = segments[0];
            //                tracking_id = segments[1];
            //                //dynaic data = JObject.Parse(ResJson);
            //                //JObject account = JObject.Parse(ResJson);
            //                //Response.Write(custID);
            //                for (int j = 0; j < segments.Length; j++)
            //                {

            //                    if (segments[j].Split('=')[0] == "order_id")
            //                    {
            //                        order_id = segments[j].Split('=')[1];
            //                    }
            //                    if (segments[j].Split('=')[0] == "tracking_id")
            //                    {
            //                        tracking_id = segments[j].Split('=')[1];

            //                    }
            //                    if (segments[j].Split('=')[0] == "bank_ref_no")
            //                    {
            //                        bank_ref_no = segments[j].Split('=')[1];
            //                    }

            //                }
            //            }
            //            if (keys[i] == "orderNo")
            //            {
            //                orderNo = Request.Form[keys[i]];
            //                //Response.Write(custID);
            //            }
                        
            //            if (keys[i] == "crossSellUrl")
            //            {
            //                crossSellUrl = Request.Form[keys[i]];
            //                // NameValueCollection param = getResponseMap(encResp);

            //                // String ResJson = ccaCrypto.Decrypt(encResp, workingKey);
            //                //dynamic data = JObject.Parse(ResJson);
            //                //JObject account = JObject.Parse(ResJson);
            //                //Response.Write(custID);
            //            }
                        
            //        }

            //        // SaveTransactionStatus
            //        //  (txnid, produtinfo, amount, firstName, email, mobile, mihpayid, status, unmappedstatus, error_Message);


                
            //}
            //catch (Exception ex)
            //{
            //    ExceptionUtility.LogException(ex, "FrmCustomerInvoiceStatusKpay, Page Load");
            //    Response.Write(ex.Message);
            //    Response.Write(ex.StackTrace);
            //}

            //    #endregion

            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseModalSaveProposal();", true);
                btnCreateInvoiceBulk.Text = "Create Invoice Bulk";
            }
        }






        private bool ValidationCreateInvoice(out string strErrorMsg)
        {
            bool IsError = false;
            strErrorMsg = "";

            if (string.IsNullOrEmpty(txtTransactionId.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Transaction ID";
            }
            else if (string.IsNullOrEmpty(txtProductInfo.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Product Info";
            }
           // else if (!Regex.IsMatch(txtAmount.Text.Trim(), "^[0-9]*$"))
                 else if (!Regex.IsMatch(txtAmount.Text.Trim(), @"^[0-9]+(\.[0-9][0-9]?)?$"))
            {
                IsError = true;
                strErrorMsg = "Please Enter Numeric Amount Till 2 Decimal Value";
            }
            else if (string.IsNullOrEmpty(txtCustomerName.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Name";
            }
            else if (string.IsNullOrEmpty(txtCustEmailAddress.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Email Address";
            }
            else if (!Regex.IsMatch(txtCustEmailAddress.Text.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                IsError = true;
                strErrorMsg = "Please Enter Valid Email Address";
            }
            else if (string.IsNullOrEmpty(txtCustMobileNumber.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Mobile Number";
            }
            else if (txtCustMobileNumber.Text.Trim().Length != 10)
            {
                IsError = true;
                strErrorMsg = "Please Enter 10 Digit Mobile Number";
            }
            else if (!Regex.IsMatch(txtCustMobileNumber.Text.Trim(), "^[0-9]*$"))
            {
                IsError = true;
                strErrorMsg = "Please Enter Valid Mobile Number";
            }
            else if (string.IsNullOrEmpty(txtValidationPeriod.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Validation Period";
            }
            else if (!Regex.IsMatch(txtValidationPeriod.Text.Trim(), "^[0-9]*$"))
            {
                IsError = true;
                strErrorMsg = "Please Enter Valid Numeric Value";
            }
            else if (Convert.ToInt32(txtValidationPeriod.Text.Trim()) <= 0)
            {
                IsError = true;
                strErrorMsg = "Please Enter Validation Period Greater Than 0";
            }
            return IsError;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        public static DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl)
        {

            DataTable dt = new DataTable();

            int TempColumn = 0;



            foreach (XmlNode node in xnl.Item(0).ChildNodes)
            {

                TempColumn++;

                DataColumn dc = new DataColumn(node.Name, System.Type.GetType("System.String"));

                if (dt.Columns.Contains(node.Name))
                {

                    dt.Columns.Add(dc.ColumnName = dc.ColumnName + TempColumn.ToString());

                }

                else
                {

                    dt.Columns.Add(dc);

                }

            }

            int ColumnsCount = dt.Columns.Count;
            for (int i = 0; i < xnl.Count; i++)
            {

                DataRow dr = dt.NewRow();

                for (int j = 0; j < ColumnsCount; j++)
                {

                    dr[j] = xnl.Item(i).ChildNodes[j].InnerText;

                }

                dt.Rows.Add(dr);

            }

            return dt;

        }



        private void SaveInvoiceLink(string PayInvoiceURL, string ShortURL, string CustomerEmailId, string QuoteNumber, string CustomerMobileNumber, string TransactionId)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_KPAY_INVOICE_LINK";

                        cmd.Parameters.AddWithValue("@QuoteNumber", QuoteNumber);
                        cmd.Parameters.AddWithValue("@CustomerEmailId", CustomerEmailId);
                        cmd.Parameters.AddWithValue("@PayInvoiceURL", PayInvoiceURL);

                        cmd.Parameters.AddWithValue("@CustomerMobileNumber", CustomerMobileNumber);
                        cmd.Parameters.AddWithValue("@ShortURL", ShortURL);

                        cmd.Parameters.AddWithValue("@TransactionId", TransactionId);

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Error Occured, " + ex.Message);
                ExceptionUtility.LogException(ex, "SaveInvoiceLink Method");
            }
        }

        private void SaveInvoiceLinkSingle(string PayInvoiceURL, string ShortURL)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_KPAY_INVOICE_LINKS_SINGLE";

                        cmd.Parameters.AddWithValue("@TransactionId", txtTransactionId.Text.Trim());
                        cmd.Parameters.AddWithValue("@ProductInfo", txtProductInfo.Text.Trim());
                        cmd.Parameters.AddWithValue("@Amount", txtAmount.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerEmailId", txtCustEmailAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerMobileNumber", txtCustMobileNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@ValidationPeriod", txtValidationPeriod.Text.Trim());
                        cmd.Parameters.AddWithValue("@MarchantReferrenceNo1", txtMarchantReferrenceNo1.Text.Trim());
                        cmd.Parameters.AddWithValue("@MarchantReferrenceNo2", txtMarchantReferrenceNo2.Text.Trim());
                        cmd.Parameters.AddWithValue("@IsSendKPayEmail", chkSendKPayEmail.Checked);
                        cmd.Parameters.AddWithValue("@IsSendKPaySMS", chkSendKPaySMS.Checked);
                        cmd.Parameters.AddWithValue("@IsSendKGIEmail", chkSendKGIEmail.Checked);
                        cmd.Parameters.AddWithValue("@IsSendKGISMS", chkSendKGISMS.Checked);
                        cmd.Parameters.AddWithValue("@IsSendShortURL", chkSendShortURL.Checked);
                        cmd.Parameters.AddWithValue("@IsDataFromBulkUpload", false);
                        cmd.Parameters.AddWithValue("@PayInvoiceURL", PayInvoiceURL);
                        cmd.Parameters.AddWithValue("@ShortURL", ShortURL);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Error Occured, " + ex.Message);
                ExceptionUtility.LogException(ex, "SaveInvoiceLink Method");
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

        private void GoogleURLShortner2(string longURL, out string shortURL)
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
                string IsUseNetworkProxy = ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString();

                string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();

                string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();
                string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();

                WebRequest myWebRequest = WebRequest.Create("http://www.googleapis.com/");
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

                WebRequest.DefaultWebProxy = null;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
            }
        }

        private void GoogleURLShortner(string longURL, out string shortURL)
        {
            shortURL = string.Empty;
            try
            {
                string ErrorMsg = string.Empty;
                WebRequest.DefaultWebProxy = null;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
                string UserId = ConfigurationManager.AppSettings["UserIdForShortURL"].ToString();
                string AccessKey = ConfigurationManager.AppSettings["AccessKeyForShortURL"].ToString();
                proxy.ConvertLongURLToShortURL(UserId, AccessKey, longURL, out shortURL, out ErrorMsg);
                proxy.Close();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
            }
        }

     
        private string Create_Invoice_New(string TransactionId, string TotalPremium, string customer_name, string EmailAddress, string MobileNumber, string ProductInfo, string validationPeriod,string MarchantReferrenceNo1, string MarchantReferrenceNo2, bool IsSendKPayEmail, bool IsSendKPaySMS, ref string responseMsg)
        {
            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Error.txt", "TotalPremium: " + TotalPremium);
            //string logFile = AppDomain.CurrentDomain.BaseDirectory + "/Error.txt";
            //StreamWriter sw = new StreamWriter(logFile, true);
            //sw.WriteLine("TotalPremium: " + TotalPremium);
            //sw.Close();
            String status = "0";
            string URL = string.Empty;
            string workingKey = ConfigurationManager.AppSettings["workingKeyKpay"].ToString();
            string strEncRequest = ConfigurationManager.AppSettings["EncRequest"].ToString();
            //string strAccessCode = "AVPJ04JA10AO54JPOA";
            string strAccessCode = ConfigurationManager.AppSettings["AccessCodeKpay"].ToString();  //"AVIZ03IC70AF52ZIFA";// put the access key in the quotes provided here.

            var obj = new Root
            {
                
                customer_name = customer_name,
                customer_email_id = EmailAddress,
                customer_mobile_no = Convert.ToInt64(MobileNumber),
                bill_delivery_type = "BOTH",
                currency = "INR",
                valid_type = "days",
                valid_for = Convert.ToInt32(validationPeriod),
                customer_email_subject = "Test",
                amount = Convert.ToDecimal(TotalPremium),
                merchant_reference_no1 = MarchantReferrenceNo1,
                merchant_reference_no2 = MarchantReferrenceNo1,
                merchant_reference_no = TransactionId,
                merchant_reference_no3 = "Pass",
                merchant_reference_no4 = Session["vUserLoginId"].ToString(),
                invoice_description = TransactionId,
                sms_content= "Pls pay your LegalEntity_Name bill # Invoice_ID for Invoice_Amount online at CCAvenue Pay_Link .",
                terms_and_conditions = "Test",
                redirect_url ="http://localhost:3638/FrmCreateInvoiceLinkKpay.aspx",
                //cancel_url = "http://localhost:3638/FrmCreateInvoiceLinkKpay.aspx",
                //sub_acc_id ="",

                //files = new List<CCFile>() {
                //new CCFile() {
                //    name = "test.doc",
                //    content = ""

                //}
                //}
            };

            
            var json = new JavaScriptSerializer().Serialize(obj);
            //Console.WriteLine(json);
            ExceptionUtility.LogEvent("Create_Invoice Req " + json);
            //ExceptionUtility.LogException(json, "Create_Invoice Method " + json);
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager
                CCACrypto ccaCrypto = new CCACrypto();
                string Url = ConfigurationManager.AppSettings["PayUWebService"].ToString();
                string PostUrl = ConfigurationManager.AppSettings["PostUrl"].ToString();  //"https://apitest.ccavenue.com/apis/servlet/DoWebTrans";  //API URL: Staging API URL:- https://apitest.ccavenue.com/apis/servlet/DoWebTrans

                string method = "generateQuickInvoice";
                string salt = ConfigurationManager.AppSettings["salt"].ToString();
                string key = ConfigurationManager.AppSettings["key"].ToString();
                
                strEncRequest = ccaCrypto.Encrypt(json.ToString(), workingKey);
                string postString = "enc_request=" + strEncRequest +
                "&access_code=" + strAccessCode +
                "&command=generateQuickInvoice" +
                "&request_type=JSON"+
                "&version=1.2";         

                string IsUseNetworkProxy = ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString();

                string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();

                string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();
                string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();

                WebRequest myWebRequest = WebRequest.Create(PostUrl);
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

                //StreamReader responseReader = new StreamReader(myWebRequest.GetResponse().GetResponseStream());
                WebResponse myWebResponse = myWebRequest.GetResponse();
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);

                string response = readStream.ReadToEnd();
                

                String encResJson = "";
                NameValueCollection param = getResponseMap(response);
                if (param != null && param.Count == 2)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        if ("status".Equals(param.Keys[i]))
                        {
                            status = param[i];
                        }
                        if ("enc_response".Equals(param.Keys[i]))
                        {
                            encResJson = param[i];
                            
                        }
                       

                    }
                    
                    if (!"".Equals(status) && status.Equals("0"))
                    {
                        String ResJson = ccaCrypto.Decrypt(encResJson, workingKey);
                        dynamic data = JObject.Parse(ResJson);
                        JObject account = JObject.Parse(ResJson);
                        
    
                        Root3 myDeserializedClass = JsonConvert.DeserializeObject<Root3>(ResJson);
                       
                        ExceptionUtility.LogEvent("Create_Invoice Res " + ResJson);

                        //URL = myDeserializedClass.Generate_Invoice_Result.tiny_url;
                        URL = myDeserializedClass.tiny_url;
                        //responseMsg = myDeserializedClass.Generate_Invoice_Result.error_desc;
                        responseMsg = myDeserializedClass.error_desc;

                    }
                    else if (!"".Equals(status) && status.Equals("1"))
                    {
                        
                        ExceptionUtility.LogEvent("failure response from ccAvenues " + encResJson);

                    }

                }

                WebRequest.DefaultWebProxy = null;
            }
            catch (JsonReaderException jex)
            {
                responseMsg = jex.ToString();
                ExceptionUtility.LogException(jex, "Create_Invoice_New Method");
            }
            catch (Exception ex)
            {
                responseMsg = ex.ToString();
                ExceptionUtility.LogException(ex, "Create_Invoice_New Method ");
            }
            return URL;
        }
        private static NameValueCollection getResponseMap(String message)
        {
            NameValueCollection Params = new NameValueCollection();
            if (message != null || !"".Equals(message))
            {
                string[] segments = message.Split('&');
                foreach (string seg in segments)
                {
                    string[] parts = seg.Split('=');
                    if (parts.Length > 0)
                    {
                        string Key = parts[0].Trim();
                        string Value = parts[1].Trim();
                        Params.Add(Key, Value);
                    }
                }
            }
            return Params;
        }

       
        private void Create_Invoice_BULK(string TransactionId, string TotalPremium, string customer_name, string EmailAddress, string MobileNumber, string ProductInfo, string validationPeriod, string IsSendKPayEmail, string IsSendKPaySMS, bool IsCreateShortURL, ref string KPayURL, ref string shortURL, ref string responseMsg)
        {
            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Error.txt", "TotalPremium: " + TotalPremium);
            //string logFile = AppDomain.CurrentDomain.BaseDirectory + "/Error.txt";
            //StreamWriter sw = new StreamWriter(logFile, true);
            //sw.WriteLine("TotalPremium: " + TotalPremium);
            //sw.Close();

            string URL = string.Empty;
            string GoogleURL = string.Empty;
            string status = "0";
            
            string workingKey = ConfigurationManager.AppSettings["workingKeyKpay"].ToString();
            string strEncRequest = ConfigurationManager.AppSettings["EncRequest"].ToString();
            string strAccessCode = ConfigurationManager.AppSettings["AccessCodeKpay"].ToString();  //"AVIZ03IC70AF52ZIFA";// put the access key in the quotes provided here.

            var obj = new Root
            {

                customer_name = customer_name,
                customer_email_id = EmailAddress,
                customer_mobile_no = Convert.ToInt64(MobileNumber),
                bill_delivery_type = "BOTH",
                currency = "INR",
                valid_type = "days",
                valid_for = Convert.ToInt32(validationPeriod),
                customer_email_subject = "Test",
                amount = Convert.ToDecimal(TotalPremium),
                merchant_reference_no = TransactionId,
                merchant_reference_no3 = "Pass-Bulk",
                merchant_reference_no4 = Session["vUserLoginId"].ToString(),
                invoice_description = TransactionId,
                sms_content = "Pls pay your LegalEntity_Name bill # Invoice_ID for Invoice_Amount online at CCAvenue Pay_Link .",
                terms_and_conditions = "Test",
                //files = new List<CCFile>() {
                //new CCFile() {
                //    name = "Somestreet",
                //    content = "10"

                //}
                //}
            };


            var json = new JavaScriptSerializer().Serialize(obj);
            //Console.WriteLine(json);
            ExceptionUtility.LogEvent("Create_Invoice Req " + json);

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager
                CCACrypto ccaCrypto = new CCACrypto();
                string Url = ConfigurationManager.AppSettings["PayUWebService"].ToString();
                string PostUrl = ConfigurationManager.AppSettings["PostUrl"].ToString();  //"https://apitest.ccavenue.com/apis/servlet/DoWebTrans";  //API URL: Staging API URL:- https://apitest.ccavenue.com/apis/servlet/DoWebTrans

                string method = "generateQuickInvoice";
                string salt = ConfigurationManager.AppSettings["salt"].ToString();
                string key = ConfigurationManager.AppSettings["key"].ToString();

                strEncRequest = ccaCrypto.Encrypt(json.ToString(), workingKey);
                string postString = "enc_request=" + strEncRequest +
                "&access_code=" + strAccessCode +
                "&command=generateQuickInvoice" +
                "&request_type=JSON" +
                "&version=1.2";

                string IsUseNetworkProxy = ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString();

                string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();

                string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();
                string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();

                WebRequest myWebRequest = WebRequest.Create(PostUrl);
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

                //StreamReader responseReader = new StreamReader(myWebRequest.GetResponse().GetResponseStream());
                WebResponse myWebResponse = myWebRequest.GetResponse();
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);

                string response = readStream.ReadToEnd();
                String encResJson = "";
                NameValueCollection param = getResponseMap(response);
                ExceptionUtility.LogEvent("Create_Invoice response Res " + response);
                if (param != null && param.Count == 2)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        if ("status".Equals(param.Keys[i]))
                        {
                            status = param[i];
                        }
                        if ("enc_response".Equals(param.Keys[i]))
                        {
                            encResJson = param[i];

                        }
                    }
                        if (!"".Equals(status) && status.Equals("0"))
                        {
                            String ResJson = ccaCrypto.Decrypt(encResJson, workingKey);
                            dynamic data = JObject.Parse(ResJson);
                            #region
                            if (IsJson(ResJson))
                            {

                                JObject account = JObject.Parse(ResJson);
                                Root3 myDeserializedClass = JsonConvert.DeserializeObject<Root3>(ResJson);
                               // URL = myDeserializedClass.Generate_Invoice_Result.tiny_url;
                                URL = myDeserializedClass.tiny_url;
                                //responseMsg = myDeserializedClass.Generate_Invoice_Result.error_desc;
                                responseMsg = myDeserializedClass.error_desc;
                                ExceptionUtility.LogEvent("Create_Invoice Res " + ResJson);
                                Uri uriResult;
                                bool IsValidURL = Uri.TryCreate(URL, UriKind.Absolute, out uriResult)
                                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                                if (IsValidURL)
                                {
                                    KPayURL = URL;
                                    if (IsCreateShortURL)
                                    {
                                        GoogleURL = string.Empty;
                                        GoogleURLShortner(URL, out GoogleURL);
                                       // Console.WriteLine(GoogleURL);
                                        Uri uriResultshort;
                                        bool IsValidURLshort = Uri.TryCreate(GoogleURL, UriKind.Absolute, out uriResultshort)
                                            && (uriResultshort.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                                    if (GoogleURL.Contains("kgi.page.link"))
                                    {
                                        IsValidURLshort = true;
                                        ExceptionUtility.LogEvent("Create_Invoice IsValidURLshort Make True " + IsValidURLshort);
                                    }

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
                        #endregion

                        else if (!"".Equals(status) && status.Equals("1"))
                        {
                            ExceptionUtility.LogEvent("failure response from ccAvenues " + encResJson);

                        }
                    }
                

            }


            catch (JsonReaderException jex)
            {
                responseMsg = jex.Message;
                ExceptionUtility.LogException(jex, "Create_Invoice Method");
            }
            catch (Exception ex)
            {
                responseMsg = ex.Message;
                ExceptionUtility.LogException(ex, "Create_Invoice Method");
            }
        }


        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        #region CodeForGridView

        public IEnumerable<ProjectPASS.KPaySavedDetails> QuoteGridView_GetData([Control("txtSearchTransaction")] string TransactionId, int maximumRows, int startRowIndex, out int totalRowCount)
        {
            int pageSize = maximumRows;
            int pageIndex = 0;

            totalRowCount = GetKPaySavedDetails(TransactionId).Count();

            if (startRowIndex > 0)
            {
                pageIndex = (int)Math.Round(((double)startRowIndex / (double)pageSize));
            }

            return GetKPaySavedDetails(TransactionId).OrderByDescending(x => x.CreatedDate).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public IEnumerable<KPaySavedDetails> GetKPaySavedDetails(string TransactionId)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_KPAY_DETAILS";

                    cmd.Parameters.AddWithValue("@TransactionId", string.IsNullOrEmpty(TransactionId) ? "" : TransactionId.Trim());
                    cmd.Parameters.AddWithValue("@LoginUserId", Session["vUserLoginId"].ToString());
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return this.CreateKPaySavedDetails(reader);
                        }
                    }
                    conn.Close();
                }
            }
        }

        private KPaySavedDetails CreateKPaySavedDetails(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("No detail exist.");
            }

            return new KPaySavedDetails
            {
                TransactionId = Convert.ToString(reader["TransactionId"]),
                ProductInfo = Convert.ToString(reader["ProductInfo"]),
                Amount = Convert.ToString(reader["Amount"]),
                CustomerName = Convert.ToString(reader["CustomerName"]),
                CustomerEmailId = Convert.ToString(reader["CustomerEmailId"]),
                CustomerMobileNumber = Convert.ToString(reader["CustomerMobileNumber"]),
                ValidationPeriod = Convert.ToString(reader["ValidationPeriod"]),
                PayInvoiceURL = Convert.ToString(reader["PayInvoiceURL"]),
                ShortURL = Convert.ToString(reader["ShortURL"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                CreatedBy = Convert.ToString(reader["CreatedBy"]),
            };
        }

        protected void QuoteGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            QuoteGridView.PageIndex = e.NewPageIndex;
        }

        #endregion


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
                    cmd.CommandText = "PROC_GET_KPAY_BULK_FILE_UPLOADED_DETAILS";

                    cmd.Parameters.AddWithValue("@LoginUserId", Session["vUserLoginId"].ToString());
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

        protected void btnCreateInvoiceSingle_Click(object sender, EventArgs e)
        {
            lnkGeneratedLinkFull.HRef = "#";
            lnkGeneratedLinkFull.InnerText = "";

            lnkGeneratedLinkShort.HRef = "#";
            lnkGeneratedLinkShort.InnerText = "";
            chkSendKPaySMS.Checked = true; //CR826 - Sandeep
            chkSendKPayEmail.Checked = true; //CR826 - Sandeep
            lblstatus.Text = string.Empty;

            string responseMsg = string.Empty;
            string strErrorMsg = string.Empty;
            string googleShortURL = string.Empty;

            if (ValidationCreateInvoice(out strErrorMsg))
            {
                lblstatus.Text = "Error: " + strErrorMsg;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                string KPayLink = Create_Invoice_New(
                    txtTransactionId.Text.Trim(),
                    txtAmount.Text.Trim(),
                    txtCustomerName.Text.Trim(),
                    txtCustEmailAddress.Text.Trim(),
                    txtCustMobileNumber.Text.Trim(),
                    txtProductInfo.Text.Trim(),
                    txtValidationPeriod.Text.Trim(),
                    txtMarchantReferrenceNo1.Text.Trim(),
                    txtMarchantReferrenceNo2.Text.Trim(),
                    chkSendKPayEmail.Checked,
                    chkSendKPaySMS.Checked,
                    ref responseMsg);

                Uri uriResult;
                bool IsValidURL = Uri.TryCreate(KPayLink, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                ExceptionUtility.LogEvent("Create_Invoice uriResult " + IsValidURL);                                                     
                if (IsValidURL)
                {
                    if (chkSendShortURL.Checked)
                    {
                        googleShortURL = string.Empty;
                        GoogleURLShortner(KPayLink, out googleShortURL);
                        ExceptionUtility.LogEvent("Create_Invoice googleShortURL " + googleShortURL);
                        Uri uriResultshort;
                        bool IsValidURLshort = Uri.TryCreate(googleShortURL, UriKind.Absolute, out uriResultshort)
                            && (uriResultshort.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                        ExceptionUtility.LogEvent("Create_Invoice uriResultshort " + IsValidURLshort);
                            if (googleShortURL.Contains("kgi.page.link"))
                        {
                            IsValidURLshort = true;
                            ExceptionUtility.LogEvent("Create_Invoice IsValidURLshort Make True " + IsValidURLshort);
                        }
                        if (IsValidURLshort)
                        {
                            lnkGeneratedLinkShort.HRef = googleShortURL;
                            lnkGeneratedLinkShort.InnerText = googleShortURL;
                        }
                    }

                    lnkGeneratedLinkFull.HRef = KPayLink;
                    lnkGeneratedLinkFull.InnerText = KPayLink;

                    SaveInvoiceLinkSingle(KPayLink, googleShortURL);

                    if (chkSendKGIEmail.Checked)
                    {
                        // 
                    }

                    if (chkSendKGISMS.Checked)
                    {
                        //
                    }
                }
                else
                {
                    lblstatus.Text = "Error: " + responseMsg;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }

            }

        }

        private bool UploadFile(ref string dirFullPath)
        {
            bool IsUploadSuccessfull = false;
            dirFullPath = string.Empty;
            if (FileUploadBulkInvoice.HasFile)
            {
                try
                {
                    string strDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string filename = Path.GetFileName(FileUploadBulkInvoice.FileName);
                    dirFullPath = Server.MapPath("~/Uploads/") + strDate.Replace("/", "").Replace("-", "").Replace(":", "") + "_" + filename;
                    FileUploadBulkInvoice.SaveAs(Server.MapPath("~/Uploads/") + strDate.Replace("/", "").Replace("-", "").Replace(":", "") + "_" + filename);

                    IsUploadSuccessfull = true;
                }
                catch (Exception ex)
                {
                    IsUploadSuccessfull = false;
                    ExceptionUtility.LogException(ex, "UploadFile");
                }
            }

            return IsUploadSuccessfull;
        }

        private bool UploadFileForScheduler(string FileTranId, ref string dirFullPath)
        {
            bool IsUploadSuccessfull = false;
            dirFullPath = string.Empty;
            if (FileUploadBulkInvoiceByScheduler.HasFile)
            {
                try
                {
                    string strDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string filename = Path.GetFileName(FileUploadBulkInvoiceByScheduler.FileName);
                    dirFullPath = Server.MapPath("~/Uploads/") + FileTranId + "_" + filename;
                    FileUploadBulkInvoiceByScheduler.SaveAs(Server.MapPath("~/Uploads/") + FileTranId + "_" + filename);

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


        private void DownloadExcelFromDatatable(DataTable dt, string fileName)
        {
            string strDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strfilename = strDate.Replace("/", "").Replace("-", "").Replace(":", "") + "_" + fileName;
            string attachment = "attachment; filename=" + strfilename.Replace(".xlsx", "") + ".xls";
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


                excelSheet.Cells[1, 1].Value = "KPay Bulk Invoice Link Status";
                excelSheet.Cells[2, 1].Value = "Report Taken On Date : " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

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

                        if (!string.IsNullOrEmpty(Convert.ToString(datarow[i - 1])))
                        {
                            if (Convert.ToString(datarow[i - 1]).Length > 0)
                            {
                                string firstChar = datarow[i - 1].ToString().Substring(0, 1);
                                if (firstChar == "=" || firstChar == "+" || firstChar == "-")
                                {
                                    excelSheet.Cells[rowcount + 1, i].Value = "'" + datarow[i - 1].ToString();
                                }
                                else
                                {
                                    excelSheet.Cells[rowcount + 1, i].Value = datarow[i - 1].ToString();
                                }
                            }
                        }

                        //for alternate rows
                        if (rowcount > 2)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    oRng = (ExcelRange)excelSheet.Cells[rowcount, dataTable.Columns.Count];
                                    //FormattingExcelCells(oRng, "#ffffff", System.Drawing.Color.Black, false);
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
                //FormattingExcelCells(oRng, "#ffffff", System.Drawing.Color.White, true);


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

        public static void ExportToExcel(DataSet dataSet, string outputPath)
        {
            //Console.WriteLine(dataSet.Tables[0].Columns.Count);

            // Create the Excel Application object
            OfficeInterop.Application excelApp = new OfficeInterop.Application();

            // Create a new Excel Workbook
            OfficeInterop.Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);

            int sheetIndex = 0;

            // Copy each DataTable
            foreach (System.Data.DataTable dt in dataSet.Tables)
            {

                // Copy the DataTable to an object array
                object[,] rawData = new object[dt.Rows.Count + 1, dt.Columns.Count];

                // Copy the column names to the first row of the object array
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    rawData[0, col] = dt.Columns[col].ColumnName;
                }

                // Copy the values to the object array
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        rawData[row + 1, col] = "'" + dt.Rows[row].ItemArray[col];
                        //rawData[row + 1, col] = dt.Rows[row].ItemArray[col];
                    }
                }

                // Calculate the final column letter
                string finalColLetter = string.Empty;
                string colCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                int colCharsetLen = colCharset.Length;

                if (dt.Columns.Count > colCharsetLen)
                {
                    finalColLetter = colCharset.Substring(
                        (dt.Columns.Count - 1) / colCharsetLen - 1, 1);
                }

                finalColLetter += colCharset.Substring(
                        (dt.Columns.Count - 1) % colCharsetLen, 1);

                // Create a new Sheet
                OfficeInterop.Worksheet excelSheet = (OfficeInterop.Worksheet)excelWorkbook.Sheets.Add(
                    excelWorkbook.Sheets.get_Item(++sheetIndex),
                    Type.Missing, 1, OfficeInterop.XlSheetType.xlWorksheet);

                excelSheet.Name = dt.TableName;

                // Fast data export to Excel
                string excelRange = string.Format("A1:{0}{1}",
                    finalColLetter, dt.Rows.Count + 1);

                excelSheet.get_Range(excelRange, Type.Missing).Value2 = rawData;

                // Mark the first row as BOLD
                ((OfficeInterop.Range)excelSheet.Rows[1, Type.Missing]).Font.Bold = true;
            }

            // Save and Close the Workbook
            excelWorkbook.SaveAs(outputPath, OfficeInterop.XlFileFormat.xlWorkbookNormal, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, OfficeInterop.XlSaveAsAccessMode.xlExclusive,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            excelWorkbook.Close(true, Type.Missing, Type.Missing);
            //excelWorkbook = null;

            // Release the Application object
            excelApp.Quit();
            //excelApp = null;

            Marshal.ReleaseComObject(excelWorkbook);
            //Marshal.ReleaseComObject(excelWorkbook);
            Marshal.ReleaseComObject(excelApp);

            // Collect the unreferenced objects
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        protected void btnCreateInvoiceBulk_Click(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(5000);

            string dirFullPath = string.Empty;
            string Message = string.Empty;

            string Date = DateTime.Now.Date.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Hour = DateTime.Now.Hour.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Minute = DateTime.Now.Minute.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Second = DateTime.Now.Second.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Millisecond = DateTime.Now.Millisecond.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string FinalOutputFileName = Date + Hour + Minute + Second + Millisecond + "_BulkInvoiceLinks.xls";
            string outPutPath = Server.MapPath("~/Reports/") + FinalOutputFileName.Replace(" ", "");



            if (!FileUploadBulkInvoice.HasFile)
            {
                lblstatus.Text = "Error: File Upload Unsuccussful, Please select a excel (.xlsx) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }

            String fileName = FileUploadBulkInvoice.PostedFile.FileName;
            string fileExt = System.IO.Path.GetExtension(FileUploadBulkInvoice.FileName);

            if (fileExt.Trim().ToLower() != ".xlsx")
            {
                lblstatus.Text = "Error: File Upload Unsuccussful, Please select a excel (.xlsx) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                bool IsUploadSuccessfull = UploadFile(ref dirFullPath);
                if (IsUploadSuccessfull)
                {
                    DataSet ds = new DataSet();
                    ds = ImportExcelToSQL(dirFullPath);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //DownloadExcelFromDatatable(ds.Tables[0], fileName);
                                //ExportToExcel(ds, outPutPath);
                                ExportDataTableToExcel(ds.Tables[0], "Test", outPutPath);

                                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                //strUrl = strUrl.Replace("http", "https");
                                lblStatusSuccess.Text = "File Processing Successfull, Click To Download The File: ";
                                lnkDownloadLink.HRef = strUrl + "KGIPASS/Reports/" + FinalOutputFileName.Replace(" ", "");
                                lnkDownloadLink.InnerText = "BulkInvoiceLinks.xls";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "HideProgress();", true);
                            }
                            else
                            {
                                lblstatus.Text = "Error: File Could not be processed due to some unknown error";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                        }
                        else
                        {
                            lblstatus.Text = "Error: File Could not be processed due to some unknown error";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                    }
                    else
                    {
                        lblstatus.Text = "Error: File Could not be processed due to some unknown error";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                }
                else
                {
                    lblstatus.Text = "Error: File Upload Unsuccussful, please contact developer";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
        }

        public DataSet ImportExcelToSQL(string excelfilepath)
        {
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

                    return ValidateAndCreateInvoice(dtExcelData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet ValidateAndCreateInvoice(DataTable dtExcelData)
        {
            DataSet dataset = new DataSet();
            if (dtExcelData != null)
            {
                if (dtExcelData.Rows.Count > 0)
                {
                    DataTable dt = new DataTable("MyTable");
                    dt.Columns.Add(new DataColumn("TransactionId", typeof(string)));
                    dt.Columns.Add(new DataColumn("ProductInfo", typeof(string)));
                    dt.Columns.Add(new DataColumn("Amount", typeof(string)));
                    dt.Columns.Add(new DataColumn("CustomerName", typeof(string)));
                    dt.Columns.Add(new DataColumn("CustomerEmailId", typeof(string)));
                    dt.Columns.Add(new DataColumn("CustomerMobileNumber", typeof(string)));
                    dt.Columns.Add(new DataColumn("ValidationPeriod", typeof(string)));
                    dt.Columns.Add(new DataColumn("IsSendKPayEmail", typeof(string)));//CR826 - Sandeep
                    dt.Columns.Add(new DataColumn("IsSendKPaySMS", typeof(string)));//CR826 - Sandeep
                    dt.Columns.Add(new DataColumn("IsSendShortURL", typeof(string)));
                    dt.Columns.Add(new DataColumn("KPayURL", typeof(string)));
                    dt.Columns.Add(new DataColumn("KPayURLShorted", typeof(string)));
                    dt.Columns.Add(new DataColumn("ErrorMessage", typeof(string)));

                    foreach (DataRow dr in dtExcelData.Rows)
                    {
                        string TransactionId = dr[0].ToString();
                        string ProductInfo = dr[1].ToString();
                        string Amount = dr[2].ToString();
                        string CustomerName = dr[3].ToString();
                        string CustomerEmailId = dr[4].ToString();
                        string CustomerMobileNumber = dr[5].ToString();
                        string ValidationPeriod = dr[6].ToString();
                        string IsSendKPayEmail = "1"; // dr[7].ToString(); //CR826 - Sandeep
                        string IsSendKPaySMS = "1"; // dr[8].ToString(); //CR826 - Sandeep
                        string IsCreateShortURL = dr[7].ToString();

                        string KPayURL = string.Empty;
                        string KPayURLShorted = string.Empty;
                        string ErrorMessage = string.Empty;

                        //call create invoice and get response
                        Create_Invoice_BULK(TransactionId, Amount, CustomerName, CustomerEmailId, CustomerMobileNumber, ProductInfo, ValidationPeriod,
                            IsSendKPayEmail,
                            IsSendKPaySMS,
                            IsCreateShortURL == "1" ? true : false,
                            ref KPayURL, ref KPayURLShorted, ref ErrorMessage);



                        DataRow newRow = dt.NewRow();
                        newRow["TransactionId"] = TransactionId;
                        newRow["ProductInfo"] = ProductInfo;
                        newRow["Amount"] = Amount;
                        newRow["CustomerName"] = CustomerName;
                        newRow["CustomerEmailId"] = CustomerEmailId;
                        newRow["CustomerMobileNumber"] = CustomerMobileNumber;
                        newRow["ValidationPeriod"] = ValidationPeriod;
                        newRow["IsSendKPayEmail"] = IsSendKPayEmail;
                        newRow["IsSendKPaySMS"] = IsSendKPaySMS;
                        newRow["IsSendShortURL"] = IsCreateShortURL;

                        newRow["KPayURL"] = KPayURL;
                        newRow["KPayURLShorted"] = KPayURLShorted;
                        newRow["ErrorMessage"] = ErrorMessage;

                        dt.Rows.Add(newRow);
                    }

                    dataset.Tables.Add(dt);

                    SaveKPayBulk(dt);
                }
            }
            return dataset;
        }

        public void SaveKPayBulk(DataTable dtExcelData)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_KPAY_INVOICE_LINKS_BULK";

                        cmd.Parameters.AddWithValue("@xml", GetXMLLead(dtExcelData));
                        cmd.Parameters.AddWithValue("@hdoc", 0);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Save KPay Bulk Method");
            }
        }

        private string GetXMLLead(DataTable dt)
        {
            StringBuilder xml = new StringBuilder(string.Empty);
            int UniqueRowId = 0;
            try
            {
                xml.Append("<Table>");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ErrorMessage"].ToString().Trim().Length <= 0)
                    {
                        UniqueRowId++;
                        xml.Append("<Row");
                        xml.Append(string.Format(" UniqueRowId = \"{0}\"", UniqueRowId));
                        xml.Append(string.Format(" TransactionId = \"{0}\"", dr["TransactionId"].ToString()));
                        xml.Append(string.Format(" ProductInfo = \"{0}\"", dr["ProductInfo"].ToString()));
                        xml.Append(string.Format(" Amount = \"{0}\"", dr["Amount"].ToString()));
                        xml.Append(string.Format(" CustomerName = \"{0}\"", dr["CustomerName"].ToString()));
                        xml.Append(string.Format(" CustomerEmailId = \"{0}\"", dr["CustomerEmailId"].ToString()));
                        xml.Append(string.Format(" CustomerMobileNumber = \"{0}\"", dr["CustomerMobileNumber"].ToString()));
                        xml.Append(string.Format(" ValidationPeriod = \"{0}\"", dr["ValidationPeriod"].ToString()));
                        xml.Append(string.Format(" IsSendKPayEmail = \"{0}\"", dr["IsSendKPayEmail"].ToString()));
                        xml.Append(string.Format(" IsSendKPaySMS = \"{0}\"", dr["IsSendKPaySMS"].ToString()));
                        xml.Append(string.Format(" IsSendKGIEmail = \"{0}\"", 0));
                        xml.Append(string.Format(" IsSendKGISMS = \"{0}\"", 0));
                        xml.Append(string.Format(" IsSendShortURL = \"{0}\"", dr["IsSendShortURL"].ToString()));
                        xml.Append(string.Format(" IsDataFromBulkUpload = \"{0}\"", 1));
                        xml.Append(string.Format(" PayInvoiceURL = \"{0}\"", dr["KPayURL"].ToString()));
                        xml.Append(string.Format(" ShortURL = \"{0}\"", dr["KPayURLShorted"].ToString()));
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
        protected void btnSearchTransaction_Click(object sender, EventArgs e)
        {
            QuoteGridView.DataBind();
        }


        protected void btnCreateInvoiceBulkByScheduler_Click(object sender, EventArgs e)
        {

            string dirFullPath = string.Empty;
            string Message = string.Empty;

            string Date = DateTime.Now.Date.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Hour = DateTime.Now.Hour.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Minute = DateTime.Now.Minute.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Second = DateTime.Now.Second.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Millisecond = DateTime.Now.Millisecond.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            //string FinalOutputFileName = Date + Hour + Minute + Second + Millisecond + "_BulkInvoiceLinks.xls";
            //string outPutPath = Server.MapPath("~/Reports/") + FinalOutputFileName.Replace(" ", "");
            string FileUploadTransactionId = Date + Hour + Minute + Second + Millisecond;


            if (!FileUploadBulkInvoiceByScheduler.HasFile)
            {
                lblstatus.Text = "Error: File Upload Unsuccussful, Please select a excel (.xlsx) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }

            String fileName = FileUploadBulkInvoiceByScheduler.PostedFile.FileName;
            string fileExt = System.IO.Path.GetExtension(FileUploadBulkInvoiceByScheduler.FileName);

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

                    string message = SaveBulkInvoiceFileUploadInformation(FileUploadTransactionId, fileName, dirFullPath, Session["vUserLoginId"].ToString().ToUpper());
                    if (message == "success")
                    {
                        lblstatus.Text = "File Uploaded succussfully.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
                        lblstatus.Text = "Error: File Upload Unsuccussful, Error:" + message;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "HideProgress();", true);

                    FileProcessGridView.DataBind();
                }
                else
                {
                    lblstatus.Text = "Error: File Upload Unsuccussful, please contact developer";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
        }

        private string SaveBulkInvoiceFileUploadInformation(string FileUploadTransactionId, string FileName, string FileFullPath, string FileUploadedBy)
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
                        cmd.CommandText = "PROC_SAVE_BULK_INVOICE_FILE_KPAY_UPLOAD_INFORMATION";

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
    }

}