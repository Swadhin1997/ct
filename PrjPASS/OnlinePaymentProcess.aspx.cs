using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayUPayment;

namespace PrjPASS
{
    public partial class OnlinePaymentProcess : System.Web.UI.Page
    {
        string mihpayid = string.Empty;
        string status = string.Empty;
        string unmappedstatus = string.Empty;
        string txnid = string.Empty;
        string PG_TYPE = string.Empty;
        string bank_ref_num = string.Empty;
        string bankcode = string.Empty;
        string error_Message = string.Empty;
        string email = string.Empty;
        string mobile = string.Empty;
        string productinfo = string.Empty;
        string amount = string.Empty;
        string firstName = string.Empty;
        string encryptStatus = string.Empty;
        string encryptTxnId = string.Empty;
        string intermediaryCode = string.Empty;
        string quoteNumber = string.Empty;
        string transactionFlag = string.Empty;
        string custID = string.Empty;
        string intermediaryLocCode = string.Empty;
        string intermediaryLocName = string.Empty;
        string PaymentId_Entry = string.Empty;
        string applicationNumber = string.Empty;
        string paymentId_allocation = string.Empty;
        string policyStartDate = string.Empty;
        string proposalStartDate = string.Empty;
        string paymentIdError = string.Empty;
        string paymentAllocationError = string.Empty;
        string paymentTagggingError = string.Empty;
        string Policynumber = string.Empty;
        string payerCode = string.Empty;
        string payerType = string.Empty;
        string payerName = string.Empty;
        string responseString = string.Empty;
        string workFlowId = string.Empty;
        string loggedInUser = string.Empty;
        string loggedInUserEmail = string.Empty;
        string loggedInUserMobile = string.Empty;
        string productCode = string.Empty;
        string paymentTaggingError = string.Empty;
        string businesstype = string.Empty;
        string vPrevPolicyNumber = string.Empty;
        string vBusinessType = string.Empty;
        string Policyno = string.Empty;
        string proposalStartDate_new = string.Empty;
        public string logfile = "log_pass_payment_service_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

        int retCnt = 0;

        public String cardnum = string.Empty;
        public String issuing_bank = string.Empty;
        public String card_type = string.Empty;
        public string folderPath = ConfigurationManager.AppSettings["xmlpath"] + DateTime.Now.ToString("dd-MMM-yyyy");
        public PayUVerifyRequest objVerify = new PayUVerifyRequest();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    divSuccess.Visible = false;
                    divFailure.Visible = false;

                    // Directory.CreateDirectory(folderPath);

                    string[] keys = Request.Form.AllKeys;

                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (keys[i] == "mihpayid")
                        {
                            mihpayid = Request.Form[keys[i]];
                        }
                        if (keys[i] == "status")
                        {
                            status = Request.Form[keys[i]];
                        }
                        if (keys[i] == "unmappedstatus")
                        {
                            unmappedstatus = Request.Form[keys[i]];
                        }
                        if (keys[i] == "txnid")
                        {
                            txnid = Request.Form[keys[i]];//strvpc_MerchTxnRef
                        }
                        if (keys[i] == "error_Message")
                        {
                            error_Message = Request.Form[keys[i]];
                        }
                        if (keys[i] == "PG_TYPE")
                        {
                            PG_TYPE = Request.Form[keys[i]];
                        }
                        if (keys[i] == "bank_ref_num")
                        {
                            bank_ref_num = Request.Form[keys[i]];//strvpc_MerchTxnRef
                        }
                        if (keys[i] == "bankcode")
                        {
                            bankcode = Request.Form[keys[i]];
                        }
                        if (keys[i] == "cardnum")
                        {
                            cardnum = Request.Form[keys[i]];
                        }
                        if (keys[i] == "issuing_bank")
                        {
                            issuing_bank = Request.Form[keys[i]];
                        }
                        if (keys[i] == "card_type")
                        {
                            card_type = Request.Form[keys[i]];
                        }

                        if (keys[i] == "email")
                        {
                            email = Request.Form[keys[i]];
                        }

                        if (keys[i] == "phone")
                        {
                            mobile = Request.Form[keys[i]];
                        }

                        if (keys[i] == "productinfo")
                        {
                            productinfo = Request.Form[keys[i]];
                        }

                        if (keys[i] == "amount")
                        {
                            amount = Request.Form[keys[i]];
                        }

                        if (keys[i] == "firstname")
                        {
                            firstName = Request.Form[keys[i]];
                        }

                        if (keys[i] == "udf1")
                        {
                            quoteNumber = Request.Form[keys[i]];
                        }

                        if (keys[i] == "udf2")
                        {
                            transactionFlag = Request.Form[keys[i]];
                        }

                        if (keys[i] == "udf3")
                        {
                            custID = Request.Form[keys[i]];
                        }

                        if (keys[i] == "udf4")
                        {
                            intermediaryCode = Request.Form[keys[i]];
                        }

                        if (keys[i] == "udf5")
                        {
                            responseString = Request.Form[keys[i]];
                        }

                        //if (keys[i] == "udf6")
                        //{
                        //    payerName = Request.Form[keys[i]];
                        //}

                        //if (keys[i] == "udf7")
                        //{
                        //    payerType = Request.Form[keys[i]];
                        //}

                        //if (keys[i] == "udf8")
                        //{
                        //    workFlowId = Request.Form[keys[i]];
                        //}

                        //if (keys[i] == "udf9")
                        //{
                        //    applicationNumber = Request.Form[keys[i]];
                        //}

                        //if (keys[i] == "udf10")
                        //{
                        //    loggedInUser = Request.Form[keys[i]];
                        //}

                    }

                    //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "OnlinePayment : Response received from Payu : " + mihpayid + " for proposal " + txnid + " with flag as " + status + DateTime.Now + Environment.NewLine);
                    // CommonUtility.Fn_LogEvent("OnlinePayment : Response received from Payu : " + mihpayid + " for proposal " + txnid + " with flag as " + status);

                    string[] arrResponseDetails = new string[11];
                    arrResponseDetails = responseString.Split(',');

                    payerCode = arrResponseDetails[0];
                    payerName = arrResponseDetails[1];
                    payerType = arrResponseDetails[2];
                    workFlowId = arrResponseDetails[3];
                    applicationNumber = arrResponseDetails[4];
                    loggedInUser = arrResponseDetails[5];
                    //Changed by Rahul to make this page global
                    policyStartDate = arrResponseDetails[6];
                    proposalStartDate = arrResponseDetails[7];
                    productCode = arrResponseDetails[8];
                    businesstype = arrResponseDetails[9];
                    Policyno = arrResponseDetails[10];

                    //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "OnlinePayment : Response received from Payu : for proposal " + txnid + " with flag as " + status + " and product code " + productCode + " and businesstype " + businesstype + " " + DateTime.Now + Environment.NewLine);
                    //CommonUtility.Fn_LogEvent("OnlinePayment : Response received from Payu : for proposal " + txnid + " with flag as " + status + " and product code " + productCode + " and businesstype " + businesstype);

                    encryptStatus = EncryptText(status);
                    encryptTxnId = EncryptText(txnid);

                    if (string.IsNullOrEmpty(error_Message)) //in case no error message is return from payu
                    {
                        error_Message = ConfigurationManager.AppSettings["gatewayError"];
                    }

                    //Database db = DatabaseFactory.CreateDatabase("cnBPOS");
                    Database db = DatabaseFactory.CreateDatabase("cnPASS");
                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();

                        SqlCommand cmdCheck = new SqlCommand("PROC_GET_EXISTING_PAYMENT_DATA", con);
                        cmdCheck.CommandType = CommandType.StoredProcedure;
                        cmdCheck.Parameters.AddWithValue("@txnid", txnid);
                        Object intCount = cmdCheck.ExecuteScalar();
                        int returnCnt = Convert.ToInt32(intCount);

                       

                        if (returnCnt > 0 && status.Trim().ToLower() == "success")
                        {
                            throw new Exception("Getting multiple response");
                        }

                        if (returnCnt > 0)
                        {
                            //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "OnlinePayment : Found existing payment data for proposal : " + txnid + DateTime.Now + Environment.NewLine);
                            //CommonUtility.Fn_LogEvent("OnlinePayment : Found existing payment data for proposal : " + txnid);

                            SqlCommand cmdHistory = new SqlCommand("PROC_INSERT_PAYMENT_DATA_HISTORY", con);
                            cmdHistory.CommandType = CommandType.StoredProcedure;
                            cmdHistory.Parameters.AddWithValue("@txnid", txnid);
                            cmdHistory.ExecuteNonQuery();
                        }

                        if (status.Trim().ToLower() == "success")
                        {
                            status = "1";
                            //sms code
                            // UpdatePaymentStatus();//to confirm
                            //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "OnlinePayment : Sending SMS Start to : " + mobile + DateTime.Now + Environment.NewLine);
                            //CommonUtility.Fn_LogEvent("OnlinePayment : Sending SMS Start to : " + mobile);

                            //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "OnlinePayment : Sending SMS End to : " + mobile + DateTime.Now + Environment.NewLine);
                            // CommonUtility.Fn_LogEvent("OnlinePayment : Sending SMS End to : " + mobile);

                        }
                        else if (status.Trim().ToLower() == "failure")
                        {
                            status = "2";
                            try
                            {
                                if (ConfigurationManager.AppSettings["PayuPaymentStatusRealTimeCheckRequired"].Equals("1"))
                                {
                                    //CommonUtility.Fn_LogEvent("OnlinePayment : Checking payment status in real time for txnid " + txnid);

                                    objVerify.key = ConfigurationManager.AppSettings["key_payu"];
                                    objVerify.command = ConfigurationManager.AppSettings["PayuGetPaymentStatusMethod"];
                                    objVerify.salt = ConfigurationManager.AppSettings["salt_payu"];
                                    objVerify.var1 = txnid;

                                    string hashvalue = Generatehash512(objVerify.key + "|" + objVerify.command + "|" + objVerify.var1 + "|" + objVerify.salt);

                                    int retryCount = 1;
                                    int maxPaymentStatusCheckAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["PayuPaymentStatusRealTimeCheckMaxRetry"]);
                                    while (retryCount <= maxPaymentStatusCheckAttempts)
                                    {
                                        retryCount++;
                                        PayUResponseDetails obj = fn_callPayuService(hashvalue);
                                        if (obj != null)
                                        {
                                            if (obj.transaction_details != null)
                                            {
                                                if (obj.transaction_details.Count > 0)
                                                {
                                                    PayUResponse response = obj.transaction_details[0];
                                                    //CommonUtility.Fn_LogEvent("OnlinePayment : Payment status for txnid " + txnid + " - " + response.error_code + ", " + response.status + ", " + response.unmappedstatus + ", " + response.field9);
                                                    if (response.error_code.Equals(ConfigurationManager.AppSettings["PayuPaymentStatusSuccessErrorCode"]))
                                                    {
                                                        status = "1";
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // CommonUtility.Fn_LogEvent("OnlinePayment : Exception in :: Real time payment status check for txnid - " + txnid + ", Exception :" + ex.Message + " to stacktrace : " + ex.StackTrace);
                                ExceptionUtility.LogException(ex, "PayuStatusFailed, OnlinePaymentProcess Page TransactionID :" + txnid);
                            }
                        }

                        else
                        {
                            //   File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PageLoad() status is null for proposal number : " + txnid + "redirecting to error page " + DateTime.Now + Environment.NewLine);
                            Response.Redirect("FrmCustomErrorPage.aspx?vpolicyerror=OnlinePaymentProcessError&pay=" + encryptStatus + "&txn=" + encryptTxnId, true);
                        }

                        if (Convert.ToBoolean(status == "1"))
                        {
                            divFailure.Visible = false;
                            divSuccess.Visible = true;
                        }
                        else
                        {
                            divFailure.Visible = true;
                            divSuccess.Visible = false;

                            lblTxnIDFailed.Text = txnid;
                            lblMihPayIDFailed.Text = mihpayid;
                            lblError.Text = status + ", " + unmappedstatus;
                        }

                        SqlCommand command = new SqlCommand("PROC_INSERT_PAYMENT_DETAILS", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@productType", "Kotak_Employee_HealthCare");
                        command.Parameters.AddWithValue("@mihpayid", mihpayid);
                        command.Parameters.AddWithValue("@amount", amount);
                        command.Parameters.AddWithValue("@tranactionId", txnid);
                        command.Parameters.AddWithValue("@status", Convert.ToInt32(status));
                        command.Parameters.AddWithValue("@authStatus", unmappedstatus + " " + error_Message);
                        command.Parameters.AddWithValue("@payrefNum", bank_ref_num);
                        command.Parameters.AddWithValue("@bankCode", bankcode);
                        command.Parameters.AddWithValue("@cardNumber", cardnum);
                        command.Parameters.AddWithValue("@issuingBank", issuing_bank);
                        command.Parameters.AddWithValue("@cardType", card_type);
                        command.Parameters.AddWithValue("@payName", firstName);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@mobile", mobile);
                        command.Parameters.AddWithValue("@quote", quoteNumber);
                        retCnt = command.ExecuteNonQuery();

                        command = new SqlCommand("[dbo].[PROC_UPDATE_PAYMENT_STATUS_AGAINST_TXNID]", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@txnid", txnid);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@PayUID", mihpayid);
                        command.ExecuteNonQuery();

                    }
                }
            }

            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "PageLoad, OnlinePaymentProcess Page TransactionID :" + txnid);
            }
        }

        public PayUResponseDetails fn_callPayuService(string hashvalue)
        {
            string network_domain = ConfigurationManager.AppSettings["network_domain"];
            string network_Username = ConfigurationManager.AppSettings["network_Username"];
            string network_Password = ConfigurationManager.AppSettings["network_Password"];
            string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"];
            string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"];
            //string proxy_Url = ConfigurationManager.AppSettings["proxy_FullAddress"];
            string proxy_Url = ConfigurationManager.AppSettings["proxy_Address_Full"];
            string PayuGetPaymentStatusURL = ConfigurationManager.AppSettings["PayuGetPaymentStatusURL"];

            PayUResponseDetails obj = new PayUResponseDetails();
            try
            {
                WebProxy proxy = new WebProxy(proxy_Url);
                proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                proxy.UseDefaultCredentials = true;
                WebRequest.DefaultWebProxy = proxy;

                var client = new WebClient();
                var method = "POST";
                var parameters = new NameValueCollection();
                client.Proxy = WebRequest.DefaultWebProxy;
                client.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                client.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);

                parameters.Add("key", objVerify.key);
                parameters.Add("command", objVerify.command);
                parameters.Add("hash", hashvalue);
                parameters.Add("var1", txnid);

                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
                /* Always returns a byte[] array data as a response. */
                var response_data = Encoding.UTF8.GetString(client.UploadValues(PayuGetPaymentStatusURL, method, parameters));

                JObject o1 = JObject.Parse(response_data);

                dynamic result = JsonConvert.DeserializeObject(o1.ToString());

                obj.msg = result.msg;
                obj.status = result.status;
                obj.transaction_details = new List<PayUResponse>();

                if (obj.status == 1)
                {
                    foreach (var file in result.transaction_details)
                    {
                        string valuess = file.Value.ToString();
                        PayUResponse objarray = JsonConvert.DeserializeObject<PayUResponse>(file.Value.ToString());

                        if (!string.IsNullOrEmpty(objarray.txnid))
                        {
                            obj.transaction_details.Add(objarray);
                        }
                    }
                }
                client.Proxy = null;
                WebRequest.DefaultWebProxy = null;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fn_callPayuService, OnlinePaymentProcess Page TransactionID :" + txnid);
                // CommonUtility.Fn_LogEvent("OnlinePayment : Error occured in fn_callPayuService() for : " + txnid + " and error message is " + ex.Message + " and stack trace is " + ex.StackTrace);
            }
            return obj;
        }

        public static string Generatehash512(string text)
        {
            try
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
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Generatehash512, OnlinePaymentProcess Page");
                return null;
            }
        }



        public string EncryptText(string clearText)
        {
            try
            {
                string EncryptionKey = "KGIMAV2BNI1907";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            catch (Exception ex)
            {
                //  File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in EncryptText " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                ExceptionUtility.LogException(ex, "EncryptText, OnlinePaymentProcess Page TransactionID :" + txnid);
                Response.Redirect("FrmCustomErrorPage.aspx?vpolicyerror=EncryptErrorOnlinePayment");
                return null;
            }
        }
    }
}