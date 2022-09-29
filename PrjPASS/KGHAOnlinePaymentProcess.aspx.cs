using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
//using Prj_Lib_Common_Utility;
using CCA.Util;
using PayUPayment;
using System.Data.Common;

namespace PrjPASS
{
    public partial class KGHAOnlinePaymentProcess : System.Web.UI.Page
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
        public string logfile = "log_KGHA_payment_service_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";


        int retCnt = 0;

        public String cardnum = string.Empty;
        public String issuing_bank = string.Empty;
        public String card_type = string.Empty;
        public string folderPath = ConfigurationManager.AppSettings["xmlpath"] + DateTime.Now.ToString("dd-MMM-yyyy");
        public PayUVerifyRequest objVerify = new PayUVerifyRequest();

        //Start CR 634
        public String payment_source = string.Empty;
        public String cardToken = string.Empty;
        bool IsCardPayment = false;


        //CCAVN Response nodes
        string tid;
        string order_id;
        string tracking_id;
        string bank_ref_no;
        string order_status;
        string failure_message;
        string payment_mode;
        string card_name;
        string status_code;
        string status_message;
        string currency;
        string billing_name;
        string billing_address;
        string billing_city;
        string billing_state;
        string billing_zip;
        string billing_country;
        string billing_tel;
        string billing_email;
        string delivery_name;
        string delivery_address;
        string delivery_city;
        string delivery_state;
        string delivery_zip;
        string delivery_country;
        string delivery_tel;
        string merchant_param1;
        string merchant_param2;
        string merchant_param3;
        string merchant_param4;
        string merchant_param5;
        string vault;
        string offer_type;
        string offer_code;
        string discount_value;
        string mer_amount;
        string eci_value;
        string retry;
        string response_code;
        string billing_notes;
        string trans_date;
        string bin_country;




        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    divSuccess.Visible = false;
                    divFailure.Visible = false;
                    clsAppLogs.LogEvent("KGHAOnlinePaymentPayment : Method Started 1");

                    string workingKey = ConfigurationManager.AppSettings["KGHAWorkingKey"].ToString();
                    CCACrypto ccaCrypto = new CCACrypto();
                    string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
                    NameValueCollection Params = new NameValueCollection();
                    string[] segments = encResponse.Split('&');
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
                    clsAppLogs.LogEvent("KGHAOnlinePaymentPayment : Method Started 2");


                    Directory.CreateDirectory(folderPath);

                    string[] keys = Request.Form.AllKeys;

                    for (int i = 0; i < Params.Count; i++)
                    {
                        if (Params.Keys[i] == "order_id")
                        {
                            order_id = Params[i];
                        }
                        if (Params.Keys[i] == "tracking_id")
                        {
                            tracking_id = Params[i];
                        }
                        if (Params.Keys[i] == "bank_ref_no")
                        {
                            bank_ref_no = Params[i];
                        }
                        if (Params.Keys[i] == "order_status")
                        {
                            order_status = Params[i];
                        }
                        if (Params.Keys[i] == "failure_message")
                        {
                            failure_message = Params[i];
                        }
                        if (Params.Keys[i] == "payment_mode")
                        {
                            payment_mode = Params[i];
                        }
                        if (Params.Keys[i] == "card_name")
                        {
                            card_name = Params[i];
                        }
                        if (Params.Keys[i] == "status_code")
                        {
                            status_code = Params[i];
                        }
                        if (Params.Keys[i] == "status_message")
                        {
                            status_message = Params[i];
                        }
                        if (Params.Keys[i] == "currency")
                        {
                            currency = Params[i];
                        }
                        if (Params.Keys[i] == "amount")
                        {
                            amount = Params[i];
                        }

                        if (Params.Keys[i] == "billing_name")
                        {
                            billing_name = Params[i];
                        }

                        if (Params.Keys[i] == "billing_address")
                        {
                            billing_address = Params[i];
                        }

                        if (Params.Keys[i] == "billing_city")
                        {
                            billing_city = Params[i];
                        }

                        if (Params.Keys[i] == "billing_state")
                        {
                            billing_state = Params[i];
                        }

                        if (Params.Keys[i] == "billing_zip")
                        {
                            billing_zip = Params[i];
                        }

                        if (Params.Keys[i] == "billing_country")
                        {
                            billing_country = Params[i];
                        }

                        if (Params.Keys[i] == "billing_tel")
                        {
                            billing_tel = Params[i];
                        }

                        if (Params.Keys[i] == "billing_email")
                        {
                            billing_email = Params[i];
                        }

                        if (Params.Keys[i] == "delivery_name")
                        {
                            delivery_name = Params[i];
                        }

                        if (Params.Keys[i] == "delivery_address")
                        {
                            delivery_address = Params[i];
                        }

                        if (Params.Keys[i] == "delivery_city")
                        {
                            delivery_city = Params[i];

                        }
                        if (Params.Keys[i] == "delivery_address")
                        {
                            delivery_address = Params[i];
                        }
                        if (Params.Keys[i] == "delivery_state")
                        {
                            delivery_state = Params[i];
                        }
                        if (Params.Keys[i] == "delivery_zip")
                        {
                            delivery_zip = Params[i];
                        }
                        if (Params.Keys[i] == "delivery_country")
                        {
                            delivery_country = Params[i];
                        }
                        if (Params.Keys[i] == "delivery_tel")
                        {
                            delivery_tel = Params[i];
                        }
                        if (Params.Keys[i] == "merchant_param1")
                        {
                            merchant_param1 = Params[i];
                        }
                        if (Params.Keys[i] == "merchant_param2")
                        {
                            merchant_param2 = Params[i];
                        }
                        if (Params.Keys[i] == "merchant_param3")
                        {
                            merchant_param3 = Params[i];
                        }
                        if (Params.Keys[i] == "merchant_param4")
                        {
                            merchant_param4 = Params[i];
                        }
                        if (Params.Keys[i] == "merchant_param5")
                        {
                            merchant_param5 = Params[i];
                        }
                        if (Params.Keys[i] == "vault")
                        {
                            vault = Params[i];
                        }
                        if (Params.Keys[i] == "offer_type")
                        {
                            offer_type = Params[i];
                        }
                        if (Params.Keys[i] == "offer_code")
                        {
                            offer_code = Params[i];
                        }
                        if (Params.Keys[i] == "discount_value")
                        {
                            discount_value = Params[i];
                        }
                        if (Params.Keys[i] == "mer_amount")
                        {
                            mer_amount = Params[i];
                        }
                        if (Params.Keys[i] == "eci_value")
                        {
                            eci_value = Params[i];
                        }
                        if (Params.Keys[i] == "retry")
                        {
                            retry = Params[i];
                        }
                        if (Params.Keys[i] == "response_code")
                        {
                            response_code = Params[i];
                        }
                        if (Params.Keys[i] == "billing_notes")
                        {
                            billing_notes = Params[i];
                        }
                        if (Params.Keys[i] == "trans_date")
                        {
                            trans_date = Params[i];
                        }
                        if (Params.Keys[i] == "bin_country")
                        {
                            bin_country = Params[i];
                        }

                        if (Params.Keys[i] == "tid")
                        {
                            tid = Params[i];
                        }
                    }

                    //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "OnlinePayment : Response received from Payu : " + mihpayid + " for proposal " + txnid + " with flag as " + status + DateTime.Now + Environment.NewLine);
                    clsAppLogs.LogEvent("KGHAOnlinePaymentPayment : Response received from CCAV : tracking_id " + tracking_id + "order_status " + order_status + " for proposal " + order_id + " with flag as " + status_message + "response string " + encResponse);

                    string[] arrResponseDetails = new string[11];
                    arrResponseDetails = responseString.Split(',');

                    payerCode = arrResponseDetails[0];
                    payerName = delivery_name;

                    policyStartDate = trans_date;
                    proposalStartDate = trans_date;
                    clsAppLogs.LogEvent("KGHAOnlinePaymentPayment : Response received from CCAV : for proposal " + order_id + " with flag as " + status_message + " and product code " + productCode + " and businesstype " + businesstype);

                    encryptStatus = EncryptText(order_status);
                    encryptTxnId = EncryptText(order_id);
                    clsAppLogs.LogEvent("KGHAOnlinePaymentPayment : Status :" + order_status);

                    if (string.IsNullOrEmpty(order_status) && string.IsNullOrEmpty(status_message)) //in case no error message is return from payu
                    {
                        error_Message = ConfigurationManager.AppSettings["gatewayError"];
                    }


                    using (SqlConnection con = new SqlConnection())
                    {
                        con.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                        con.Open();

                        if (order_status.Trim().ToLower() == "success")
                        {
                            status = "1";


                        }
                        else if (order_status.Trim().ToLower() == "failure")
                        {
                            status = "2";

                            clsAppLogs.LogEvent("got failed response from payu " + txnid);

                        }

                        else
                        {
                            //   File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PageLoad() status is null for proposal number : " + txnid + "redirecting to error page " + DateTime.Now + Environment.NewLine);
                            Response.Redirect("FrmCustomErrorPage.aspx?vpolicyerror=OnlinePaymentProcessError&pay=" + encryptStatus + "&txn=" + encryptTxnId, true);
                        }

                        //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "OnlinePayment : Inserting payment data for proposal : " + txnid + DateTime.Now + Environment.NewLine);
                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : Inserting payment data for proposal : " + order_id);

                        SqlCommand command = new SqlCommand("PROC_CCAVN_KGHA_SAVE_RESPONSE_DETAILS", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@order_id ", order_id);
                        command.Parameters.AddWithValue("@tracking_id", tracking_id);
                        command.Parameters.AddWithValue("@bank_ref_no", bank_ref_no);
                        command.Parameters.AddWithValue("@order_status", order_status);
                        command.Parameters.AddWithValue("@failure_message", failure_message);
                        command.Parameters.AddWithValue("@payment_mode", payment_mode);
                        command.Parameters.AddWithValue("@card_name", card_name);
                        command.Parameters.AddWithValue("@status_code", status_code);
                        command.Parameters.AddWithValue("@status_message", status_message);
                        command.Parameters.AddWithValue("@vault", vault);
                        command.Parameters.AddWithValue("@offer_type", offer_type);
                        command.Parameters.AddWithValue("@offer_code", offer_code);
                        command.Parameters.AddWithValue("@discount_value", discount_value);
                        command.Parameters.AddWithValue("@mer_amount", mer_amount);
                        command.Parameters.AddWithValue("@eci_value", eci_value);
                        command.Parameters.AddWithValue("@retry", retry);
                        command.Parameters.AddWithValue("@response_code", response_code);
                        command.Parameters.AddWithValue("@billing_notes", billing_notes);
                        command.Parameters.AddWithValue("@trans_date", trans_date);
                        command.Parameters.AddWithValue("@bin_country", bin_country);

                        retCnt = command.ExecuteNonQuery();
                    }

                    if (Convert.ToInt32(status) == 1)
                    {
                        divFailure.Visible = false;
                        divSuccess.Visible = true;
                        lblproposalno.Text = order_id;
                        string proposalno = order_id;

                        string PaymentUpdate = SavePaymentUpdate(proposalno,  status);
                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : SavePaymentUpdate Insert " + PaymentUpdate);

                        //fn_callInstaService();
                        //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "KGHAOnlinePaymentProcess : Start of PaymentEntryAPI for : " + txnid + " for transaction flag " + transactionFlag + " " + DateTime.Now + Environment.NewLine);
                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : Start of PaymentEntryAPI for : " + tracking_id + " for transaction flag " + status_message);
                        //PaymentEntryAPI();
                        //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "KGHAOnlinePaymentProcess : End of PaymentEntryAPI for : " + txnid + " for transaction flag " + transactionFlag + " " + DateTime.Now + Environment.NewLine);
                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : End of PaymentEntryAPI for : " + tracking_id + " for transaction flag " + status_message);

                        string mobileno = delivery_tel;
                        string proposername = delivery_name;
                        string proposeremail = billing_email;
                        string policyno = randomrtid();
                        string policypath = string.Empty;
                        String IstranscriptEmailSent = string.Empty;
                        //Create PolicyPDF
                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : Sending SMS Start to : " + delivery_tel);
                        bool smssend = SendSuccessPaymentSMS(mobileno, proposername, mer_amount);
                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : Sending SMS Success to : " + smssend);
                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : Sending EMAIL Start to : " + delivery_tel);

                        string strMailSubject = "Payment confirmation for Kotak Group Health Assure Plan with Kotak General Insurance";

                        String IsPayMailSent = SendKGHA_SuccessPaymentMAIL(proposeremail, proposalno, strMailSubject, proposername, mer_amount);

                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : Sending Payment EMAIL Success to : " + IsPayMailSent);
                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : Creating Policy PDF for No : " + policyno);
                        string Filename = string.Empty;
                        string Filepath = string.Empty;
                        string DecFilename = string.Empty;
                        string DecFilepath = string.Empty;
                        string isPDFSAVEFailed = createKGHAPDF(policyno, proposalno);
                        if (string.IsNullOrEmpty(isPDFSAVEFailed.Trim()))
                        {
                            Filename = policyno + ".pdf";
                            DecFilename = policyno + "_GOOD_DECLARATION.pdf";
                            string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakKGHAPolicyPDFFiles"].ToString();
                            Filepath = KotakQuotesPDFFiles + Filename;
                            DecFilepath = KotakQuotesPDFFiles + DecFilename;
                            string strMailSubject1 = string.Format("Transcript and Good Health Declaration for your Kotak Group Health Assure {0}.", proposalno);
                            IstranscriptEmailSent = SendKGHA_TranscriptPaymentMAIL(proposeremail, proposalno, policyno, DecFilename, strMailSubject1, proposername, mer_amount);
                            clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : Sending EMAIL Transacript Success to : " + IstranscriptEmailSent);
                        }
                        else
                        {
                            clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : Policy PDF IS not Created");

                        }



                        string Savepolicy_Details = SavepolicyDetails(proposalno, policyno, Filename, Filepath, DecFilename, DecFilepath, IstranscriptEmailSent, IsPayMailSent, smssend);
                        clsAppLogs.LogEvent("KGHAOnlinePaymentProcess : SavepolicyDetails inserted" + Savepolicy_Details);

                        //SendMail();
                        //  SendSMS();
                    }
                    else
                    {
                        divSuccess.Visible = false;
                        divFailure.Visible = true;
                        //  UpdatePaymentStatus();
                        lblError.Text = error_Message + " " + unmappedstatus;
                        lblProposalNumberFailed.Text = order_id;
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Getting multiple response"))
                {
                    divFailure.Visible = false;
                    divSuccess.Visible = true;
                    // lblproposalno.Text = Convert.ToString(HttpContext.Current.Session["Policynumber"]);
                }
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "KGHAOnlinePaymentProcess.aspx ::Catch after exception :" + ex.Message + " to stacktrace : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                clsAppLogs.LogEvent("KGHAOnlinePaymentProcess.aspx ::Catch after exception :" + ex.Message + " to stacktrace : " + ex.StackTrace);
                Response.Redirect("FrmCustomErrorPage.aspx");

            }
        }
        private string SavePaymentUpdate(string ProposalNo,string status)
        {
            string message = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_UPDATE_PAYMENT_DETAILS";

                        cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);
                        cmd.Parameters.AddWithValue("@IsPaymentFlag", Convert.ToInt32(status) == 1?1:0);
                       

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        message = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "error";
            }
            return message;
        }
        private string SavepolicyDetails(string ProposalNo, string PolicyNo, string Filename, string Filepath, string DecFilename, string DecFilepath, string IsPolicyMailSent, string IsPayMailSent, bool IsPaySMSSent)
        {
            string message = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_KGHA_POLICY_DETAILS";

                        cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);
                        cmd.Parameters.AddWithValue("@PolicyNo", PolicyNo);
                        cmd.Parameters.AddWithValue("@Filename", Filename);
                        cmd.Parameters.AddWithValue("@Filepath", Filepath);
                        cmd.Parameters.AddWithValue("@DecFilename", DecFilename);
                        cmd.Parameters.AddWithValue("@DecFilepath", DecFilepath);
                        cmd.Parameters.AddWithValue("@IsPolicyMailSent", IsPolicyMailSent != "Success" ? 0 : 1);
                        cmd.Parameters.AddWithValue("@IsPayMailSent", IsPayMailSent != "Success" ? 0 : 1);
                        cmd.Parameters.AddWithValue("@IsPaySMSSent", IsPaySMSSent == false ? 0 : 1);

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        message = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "error";
                ExceptionUtility.LogException(ex, "SavepolicyDetails, KGHAOnlinePaymentProcess Page");
            }
            return message;
        }
        protected string randomrtid()
        {

            string Date = DateTime.Now.Date.ToString("yyyy/MM/dd").Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Hour = DateTime.Now.Hour.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Minute = DateTime.Now.Minute.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Second = DateTime.Now.Second.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Millisecond = DateTime.Now.Millisecond.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string TID = Date + Hour + Minute + Second + Millisecond;
            return TID;
        }
        //private string createKGHAPDF(string policyno, string proposername)
        //{
        //    string strErrorMsg = "";

        //    try
        //    {
        //        string strPath = AppDomain.CurrentDomain.BaseDirectory + "KGHA_POLICY_PDF_DUMMY.html";
        //        string htmlBody = File.ReadAllText(strPath);
        //        StringWriter sw = new StringWriter();
        //        StringReader sr = new StringReader(sw.ToString());
        //        string strHtml = htmlBody;

        //        GenerateKGHA_Flotaer_PDF(strHtml, policyno, proposername, out strErrorMsg);
        //    }
        //    catch (Exception ex)
        //    {
        //        strErrorMsg = "Error";
        //        ExceptionUtility.LogException(ex, "createKGHAPDF Method");
        //    }
        //    return strErrorMsg;
        //}
        private string createKGHAPDF(string policyno,string proposalno)
        {
            string strErrorMsg = "";
            string strHTMLResult = "";
            string strDecHTMLResult = "";
            try
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "KGHA_POLICY_PDF.html";
                string htmlBody = File.ReadAllText(strPath);
                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(sw.ToString());
                string strtranscriptHtml = htmlBody;

                string strPath1 = AppDomain.CurrentDomain.BaseDirectory + "KGHA_GOOD_DEC.html";
                string htmlBody1 = File.ReadAllText(strPath1);
                StringWriter sw1 = new StringWriter();
                StringReader sr1 = new StringReader(sw1.ToString());
                string strDecHtml = htmlBody1;
                DataSet dsKGHAPaymentDetails = new DataSet();
                dsKGHAPaymentDetails = GetKGHAProposerDetails(proposalno);
                PolicyDatabind(strtranscriptHtml, policyno, dsKGHAPaymentDetails, out strHTMLResult);
               // DecDatabind(strDecHtml, dsKGHAPaymentDetails, out strDecHTMLResult);
                GenerateKGHA_Flotaer_PDF(strHTMLResult, strDecHtml, policyno, out strErrorMsg);
            }
            catch (Exception ex)
            {
                strErrorMsg = "Error";
                ExceptionUtility.LogException(ex, "createKGHAPDF Method");
            }
            return strErrorMsg;
        }
        private void PolicyDatabind(string strHtml, string policyno, DataSet dsKGHAPaymentDetails, out string strHTMLResult)
        {
            string tbody = string.Empty;
            try
            {
                clsProposerPaymentDetailsResponse objResponse = GetProposerDetails(dsKGHAPaymentDetails);

                strHtml = strHtml.Replace("@proposername", objResponse.VProposerName);
                strHtml = strHtml.Replace("@poilcyno", string.IsNullOrEmpty(objResponse.vPolicyNo.Trim())?policyno:objResponse.vPolicyNo);
                strHtml = strHtml.Replace("@ProposerGender", objResponse.vProposerGender);
                strHtml = strHtml.Replace("@proposerdob", objResponse.vProposerdob);
                strHtml = strHtml.Replace("@proposeraddress", objResponse.vProposerAddress);
                strHtml = strHtml.Replace("@proposermobile", objResponse.vProposerMobile);
                strHtml = strHtml.Replace("@proposeremail", objResponse.vProposeremail);
                strHtml = strHtml.Replace("@proposerocc", objResponse.vProposerocc);
                strHtml = strHtml.Replace("@Panno", "NA");
                
                strHtml = strHtml.Replace("@memberuniqueno", objResponse.vMemberuniqueid);
                strHtml = strHtml.Replace("@policystartdate", objResponse.vpolicystartDate);
                strHtml = strHtml.Replace("@policyendate", objResponse.vpolicystartEnd);

                strHtml = strHtml.Replace("@policytenure", Convert.ToInt32(objResponse.vpolicyTenure) > 1 ? objResponse.vpolicyTenure + " Years" : objResponse.vpolicyTenure + " Year");

                strHtml = strHtml.Replace("@totalmember", objResponse.vMemberCovered);
                strHtml = strHtml.Replace("@suminsured", objResponse.vSumInsured);
                strHtml = strHtml.Replace("@totalpremium", objResponse.vTotalPremium);

                tbody += "<table width='98%' cellpadding='5' style='border: 1px solid black; border-collapse:collapse;'><tbody>";

                if (dsKGHAPaymentDetails != null)
                {
                    if (dsKGHAPaymentDetails.Tables.Count > 2)
                    {
                        if (dsKGHAPaymentDetails.Tables[2].Rows.Count > 0)
                        {
                            DataTable dtKGHAProposerDetails = dsKGHAPaymentDetails.Tables[2];
                            int j = dsKGHAPaymentDetails.Tables[2].Rows.Count;
                            tbody += "<tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'></td>";
                            for (int i = 1; i <= j; i++)
                            {
                                tbody += "<td style='border: 1px solid black' width='" + 69 / Convert.ToInt32(objResponse.vMemberCovered) + "%'>Insured " + i + "</td>";
                            }

                            tbody += "</tr><tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'><p>Insured Name </p></td>";

                            for (int i = 0; i < j; i++)
                            {
                                tbody += "<td style = 'border: 1px solid black' width='" + 69 / j + "%' ><p>" + dtKGHAProposerDetails.Rows[i]["MemberName"].ToString() + "</p></td>";
                            }

                            tbody += "</tr><tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'><p>Relation with the Proposer </p></td>";

                            for (int i = 0; i < j; i++)
                            {
                                string memberrelationship = dtKGHAProposerDetails.Rows[i]["MemberRelationship"].ToString();
                                if (memberrelationship.ToUpper() == "SELF")
                                {
                                    memberrelationship = dtKGHAProposerDetails.Rows[i]["MemberRelationship"].ToString();
                                }
                                else if (memberrelationship.ToUpper() == "HUSBAND" || memberrelationship.ToUpper() == "WIFE")
                                {
                                    memberrelationship = dtKGHAProposerDetails.Rows[i]["MemberGender"].ToString() == "M" ? "Husband" : "Wife";
                                }
                                else if (memberrelationship.ToUpper() == "FATHER" || memberrelationship.ToUpper() == "MOTHER")
                                {
                                    memberrelationship = dtKGHAProposerDetails.Rows[i]["MemberGender"].ToString() == "M" ? "Son" : "Daughter";
                                }
                                tbody += "<td style = 'border: 1px solid black' width='" + 69 / j + "%' ><p>" + memberrelationship + "</p></td>";
                            }
                            tbody += "</tr><tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'><p>Gender </p></td>";

                            for (int i = 0; i < j; i++)
                            {
                                string membergender = dtKGHAProposerDetails.Rows[i]["MemberGender"].ToString() == "M" ? "Male" : "Female";

                                tbody += "<td style = 'border: 1px solid black' width='" + 69 / j + "%' ><p>" + membergender + "</p></td>";
                            }

                            tbody += "</tr><tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'><p>Date of Birth  </p></td>";

                            for (int i = 0; i < j; i++)
                            {
                                tbody += "<td style = 'border: 1px solid black' width='" + 69 / j + "%' ><p>" + Convert.ToDateTime(dtKGHAProposerDetails.Rows[i]["MemberDOB"]).ToString("dd/MMM/yyyy").Replace("/", " ") + "</p></td>";
                            }
                            tbody += "</tr><tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'><p>Occupation  </p></td>";

                            for (int i = 0; i < j; i++)
                            {
                                tbody += "<td style = 'border: 1px solid black' width='" + 69 / j + "%' ><p>" + dtKGHAProposerDetails.Rows[i]["MemberOccupation"].ToString() + "</p></td>";
                            }
                            tbody += "</tr><tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'><p>Marital Status  </p></td>";

                            for (int i = 0; i < j; i++)
                            {
                                tbody += "<td style = 'border: 1px solid black' width='" + 69 / j + "%' ><p>" + dtKGHAProposerDetails.Rows[i]["MartialStatus"].ToString() + "</p></td>";
                            }
                            tbody += "</tr><tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'><p>Nominee Name  </p></td>";

                            for (int i = 0; i < j; i++)
                            {
                                tbody += "<td style = 'border: 1px solid black' width='" + 69 / j + "%' ><p>" + dtKGHAProposerDetails.Rows[i]["NomineeName"].ToString() + "</p></td>";
                            }
                            tbody += "</tr><tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'><p>Nominee Relationship  </p></td>";

                            for (int i = 0; i < j; i++)
                            {
                                tbody += "<td style = 'border: 1px solid black' width='" + 69 / j + "%' ><p>" + dtKGHAProposerDetails.Rows[i]["NomineeRelationship"].ToString() + "</p></td>";
                            }
                            tbody += "</tr><tr style='border: 1px solid black'><td style='border: 1px solid black' width='30%'><p>Nominee DOB  </p></td>";

                            for (int i = 0; i < j; i++)
                            {
                                string nomineedob = dtKGHAProposerDetails.Rows[i]["NomineeDOB"].ToString().Contains("1900") ? "" : Convert.ToDateTime(dtKGHAProposerDetails.Rows[i]["NomineeDOB"]).ToString("dd/MMM/yyyy").Replace("/", " ");
                                tbody += "<td style = 'border: 1px solid black' width='" + 69 / j + "%' ><p>" + nomineedob + "</p></td>";
                            }
                            tbody += "</tr>";

                        }
                    }
                }
                tbody += "</tbody></table>";
                strHtml = strHtml.Replace("@tbody", tbody);
                strHtml = strHtml.Replace("@payamount", objResponse.vTotalPremium);
                strHtml = strHtml.Replace("@bankingref", objResponse.vbankingref);
                strHtml = strHtml.Replace("@transactiondate", objResponse.vtrandate);
                strHtml = strHtml.Replace("@bankdetails", objResponse.vBankName);
                strHtml = strHtml.Replace("@Onlinepayment", objResponse.vpaymentmode);
                strHtml = strHtml.Replace("@place", objResponse.vCity);
                strHtml = strHtml.Replace("@datetime", DateTime.Now.ToString("dd/MM/yyyy"));
                 

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "PolicyDatabind Method");
            }
            strHTMLResult = strHtml;
        }

        //private void DecDatabind(string strHtml, DataSet dsKGHAPaymentDetails, out string strDecHTMLResult)
        //{
        //    string tbody = string.Empty;
        //    try
        //    {
             
        //      }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, "DecDatabind Method");
        //    }
        //    strDecHTMLResult = strHtml;
        //}
        public static clsProposerPaymentDetailsResponse GetProposerDetails(DataSet dsKGHAProposerDetails)
        {
            clsProposerPaymentDetailsResponse objResponse = new clsProposerPaymentDetailsResponse();
            try
            {
                if (dsKGHAProposerDetails != null)
                {
                    if (dsKGHAProposerDetails.Tables.Count > 2)
                    {
                        if (dsKGHAProposerDetails.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtKGHAProposerDetails = dsKGHAProposerDetails.Tables[0];
                            DataTable dtKGHAProposerDetails2 = dsKGHAProposerDetails.Tables[1];


                            objResponse = new clsProposerPaymentDetailsResponse
                            {
                                vStatus = "Success",
                                vErrorMsg = "",

                                VProposalno = dtKGHAProposerDetails.Rows[0]["ProposalNo"].ToString(),
                                VProposerName = dtKGHAProposerDetails.Rows[0]["ProposerName"].ToString(),
                                vProposerGender = dtKGHAProposerDetails.Rows[0]["Gender"].ToString() != "M" ? "Female" : "Male",
                                vProposerAddress = dtKGHAProposerDetails.Rows[0]["AddressLine1"].ToString() + "," + dtKGHAProposerDetails.Rows[0]["AddressLine2"].ToString(),
                                vProposerdob = Convert.ToDateTime(dtKGHAProposerDetails.Rows[0]["DOB"]).ToString("dd/MMM/yyyy").Replace("/", " "),
                                vProposerMobile = dtKGHAProposerDetails.Rows[0]["ContactNumber"].ToString(),
                                vProposerocc = dtKGHAProposerDetails.Rows[0]["Occupation"].ToString(),
                                vProposeremail = dtKGHAProposerDetails.Rows[0]["EmailAddress"].ToString(),
                                vPolicyNo = dtKGHAProposerDetails.Rows[0]["PolicyNo"].ToString(),
                                // vpolicystartDate = Convert.ToDateTime(dtKGHAProposerDetails.Rows[0]["dPaymentDate"]).ToString("dd/MMM/yyyy").Replace("/", " "),
                                vpolicystartDate = Convert.ToDateTime(dtKGHAProposerDetails.Rows[0]["dPaymentDate"]).ToString("dd/MMM/yyyy HH:mm:ss").Replace("/", " "),
                                vpolicystartEnd = Convert.ToDateTime(dtKGHAProposerDetails.Rows[0]["dPaymentDate"]).AddYears(1).ToString("dd/MMM/yyyy").Replace("/", " "),
                                vplan = dtKGHAProposerDetails.Rows[0]["PlanName"].ToString(),
                                vTotalPremium = dtKGHAProposerDetails.Rows[0]["SelectedPremium"].ToString(),
                                vPlanDesc = dtKGHAProposerDetails2.Rows[0]["PlanDesc"].ToString(),
                                vSumInsured = dtKGHAProposerDetails2.Rows[0]["SumInsured"].ToString(),
                                vpolicyTenure = dtKGHAProposerDetails2.Rows[0]["policyTenure"].ToString(),
                                vNetPremium = dtKGHAProposerDetails2.Rows[0]["NetPremium"].ToString(),
                                vGST = dtKGHAProposerDetails2.Rows[0]["GST"].ToString(),
                                vMemberCovered = dtKGHAProposerDetails2.Rows[0]["MemberCovered"].ToString(),
                                vbankingref = dtKGHAProposerDetails.Rows[0]["bank_ref_no"].ToString(),
                                vtrandate = Convert.ToDateTime(dtKGHAProposerDetails.Rows[0]["dPaymentDate"]).ToString("dd/MMM/yyyy").Replace("/", " "),
                                vpaymentmode = dtKGHAProposerDetails.Rows[0]["payment_mode"].ToString(),
                                vMemberuniqueid = "AA" + dtKGHAProposerDetails.Rows[0]["UniqueRowId"].ToString(),
                                vBankName= dtKGHAProposerDetails.Rows[0]["card_name"].ToString(),
                                vCity = dtKGHAProposerDetails.Rows[0]["City"].ToString()
                            };

                        }


                    }

                }
            }

            catch (Exception ex)
            {
                objResponse.vStatus = "Fail";
                objResponse.vStatus = ex.Message;
                ExceptionUtility.LogException(ex, "GetKGHAPaymentDetails Method");
            }
            return objResponse;
        }


        private DataSet GetKGHAProposerDetails(string Proposalno)
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_KGHA_PROPOSER_PAYMENT_DETAILS";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "ProposalNo", DbType.String, ParameterDirection.Input, "ProposalNo", DataRowVersion.Current, Proposalno);

                dbCommand.CommandType = CommandType.StoredProcedure;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetKGHAProposerDetails Method");
            }
            return ds;
        }
        private void PDFave(byte[] _outPdfBuffer, bool IsWithoutHeaderFooter, string filename)
        {
            try
            {
                string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakKGHAPolicyPDFFiles"].ToString();
                if (ConfigurationManager.AppSettings["IsProdEnvironment"].ToString() == "1")
                {
                    try
                    {
                        string serverPath = @"\\10.221.12.44\d$\KGIPASSPUBLISH\Uploads\KGHADocument\KGHAPolicy_PDF\";
                        File.WriteAllBytes(serverPath + (IsWithoutHeaderFooter == false ? filename + ".pdf" : filename + ".pdf"), _outPdfBuffer); // Requires System.IO

                    }
                    catch (Exception ex2)
                    {
                        ExceptionUtility.LogException(ex2, "UploadFileForScannedDoc Method for 10.221.12.44 IP ");
                    }

                }
                File.WriteAllBytes(KotakQuotesPDFFiles + (IsWithoutHeaderFooter == false ? filename + ".pdf" : filename + ".pdf"), _outPdfBuffer); // Requires System.IO
                                                                                                                                                   // strErrorMsg = string.Empty;
            }
            catch (Exception ex)
            {
                // strErrorMsg = "error";
                ExceptionUtility.LogException(ex, "PDFave");
            }
        }
        private void GenerateKGHA_Flotaer_PDF(string strHtml,string  strDecHTMLResult, string policyno, out string strErrorMsg)
        {
            clsConvertHtmlToPdf objHtmlToPdf = new clsConvertHtmlToPdf();
            try
            {
                
                objHtmlToPdf.IsWithoutHeaderFooter = true;
                byte[] outPdfBuffer = objHtmlToPdf.ConvertToPdfNew(strHtml, out strErrorMsg);
                if (string.IsNullOrEmpty(strErrorMsg))
                {
                    PDFave(outPdfBuffer, true, policyno);
                }
                byte[] outPdfBuffer1 = objHtmlToPdf.ConvertToPdfNew(strDecHTMLResult, out strErrorMsg);
                if (string.IsNullOrEmpty(strErrorMsg))
                {
                    PDFave(outPdfBuffer1, true, policyno+"_GOOD_DECLARATION");
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = "error";
                ExceptionUtility.LogException(ex, "CreateQuoteScheduleHTMLtoPDF");
            }
        }
        private static bool SendSuccessPaymentSMS(string mobileno,string proposername,string amount)
        {
            bool IsSMSSent = false;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnConnect"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SAVE_PASS_KGHA_SUCCESS_PAYMENT_TO_TRANS_SMS_LOG";
                        cmd.Parameters.AddWithValue("@MobileNo", mobileno);
                        cmd.Parameters.AddWithValue("@proposername", proposername);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        IsSMSSent = true;
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogException(ex, "SendSuccessPaymentSMS Method");
                return IsSMSSent;

            }
            return IsSMSSent;

        }

        private static string SendKGHA_TranscriptPaymentMAIL(string ToEmailIds, string Proposalno, string policyno,string DecFilename, string MailSubject, string proposername,string amount)
        {





            string strMessage = string.Empty;
            string emailId = ToEmailIds;
            string strPath = string.Empty;
            string MailBody = string.Empty;
            amount = String.Format("{0:n0}", Convert.ToDouble(amount));
            try
            {

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "KHGA_TRANSCRIPT_MAIL_BODY.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("@Proposalno", Proposalno);
                MailBody = MailBody.Replace("@proposername", proposername);
                MailBody = MailBody.Replace("#Amount#", amount);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"], "Kotak General Insurance Co Ltd");
                mm.Subject = MailSubject;
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                string attachmentFilename = GetPolicySchedulePath(policyno);
                if (attachmentFilename != null)
                {
                    Attachment attachment = new Attachment(Path.GetFullPath(attachmentFilename),MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mm.Attachments.Add(attachment);
                }
                string attachmentFilename1 = GetPolicySchedulePath(policyno+ "_GOOD_DECLARATION");
                if (attachmentFilename1 != null)
                {
                    Attachment attachment = new Attachment(Path.GetFullPath(attachmentFilename1), MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename1);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename1);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename1);
                    disposition.FileName = Path.GetFileName(attachmentFilename1);
                    disposition.Size = new FileInfo(attachmentFilename1).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mm.Attachments.Add(attachment);
                }
                
                smtpClient.Send(mm);
                strMessage = "Success";
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                string strPathErrorFile = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPathErrorFile, "Error: " + strMessage);
            }
            return strMessage;


        }


        private static string SendKGHA_SuccessPaymentMAIL(string ToEmailIds, string Proposalno, string MailSubject, string proposername,string amount)
        {
            string strMessage = string.Empty;
            string emailId = ToEmailIds;
            string strPath = string.Empty;
            string MailBody = string.Empty;
            amount = String.Format("{0:n0}", Convert.ToDouble(amount));

            try
            {

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "KHGA_SUCCESS_PAYMENT_MAIL_BODY.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("@Proposalno", Proposalno);
                MailBody = MailBody.Replace("@proposername", proposername);
                MailBody = MailBody.Replace("#Amount#", amount);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"], "Kotak General Insurance Co Ltd");
                mm.Subject = MailSubject;
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;

                smtpClient.Send(mm);
                strMessage = "Success";
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                string strPathErrorFile = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPathErrorFile, "Error: " + strMessage);
            }
            return strMessage;
        }

        private static string GetPolicySchedulePath(string PolicyNumber)
        {
            string filename = string.Empty;



            try
            {
                string filelocation = ConfigurationManager.AppSettings["KotakKGHAPolicyPDFFiles"].ToString();
                filename = filelocation + "\\" + PolicyNumber + ".pdf";

                if (File.Exists(filename))
                {
                    //  File.Delete(filename);
                    return filename;
                }



            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetPolicySchedulePath");

            }
            return filename;
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
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in EncryptText " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx?vpolicyerror=EncryptErrorOnlinePayment");
                return null;
            }
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
                return null;
            }
        }



    }


}