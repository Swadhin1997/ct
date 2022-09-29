using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace PrjPASS
{
    public partial class FrmPaymentConfirmation : System.Web.UI.Page
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
        string produtinfo = string.Empty;
        string amount = string.Empty;
        string firstName = string.Empty;
        string encryptStatus = string.Empty;
        string encryptTxnId = string.Empty;
        string intermediaryCode = string.Empty;
        string quoteNumber = string.Empty;
        string eposFlag = string.Empty;
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
        


        public string logfile = "log_kgipass_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
        public string folderPath = ConfigurationManager.AppSettings["xmlpath"] + DateTime.Now.ToString("dd-MMM-yyyy");


        int retCnt = 0;
        
        

        public String cardnum = string.Empty;
        public String issuing_bank = string.Empty;
        public String card_type = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               

                if (!IsPostBack)
                {
                    //if (Session["loaded"] == null) //to check refresh
                    //{
                    //    Session["loaded"] = "1";
                   
                    Directory.CreateDirectory(folderPath);

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
                            produtinfo = Request.Form[keys[i]];
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
                            eposFlag = Request.Form[keys[i]];
                        }

                        if (keys[i] == "udf3")
                        {
                            custID = Request.Form[keys[i]];
                        }

                        //if (keys[i] == "udf4")
                        //{
                        //    mobile = Request.Form[keys[i]];
                        //}

                        //if (keys[i] == "lastname")
                        //{
                        //    lastName = Request.Form[keys[i]];
                        //}


                    }

                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Status received :" + status + " for proposal number " + txnid + " and payuid : " + mihpayid + " " + DateTime.Now + Environment.NewLine);





                    if (status.Trim().ToLower() == "success")
                    {
                        
                        status = "1";
                        //sms code
                        UpdatePaymentStatus();//update payment status as success
                        SendSMSToCustomer();

                    }
                    else if (status.Trim().ToLower() == "failure")
                    {
                        status = "2";
                    }

                    else
                    {
                        File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PageLoad() status is null for proposal number : " + txnid + "redirecting to error page " + DateTime.Now + Environment.NewLine);
                        Response.Redirect("FrmCustomErrorPage.aspx",true);                       
                    }

                    encryptStatus = EncryptText(status);
                    encryptTxnId = EncryptText(txnid);

                    if (string.IsNullOrEmpty(error_Message)) //in case no error message is return from payu
                    {
                        error_Message = ConfigurationManager.AppSettings["gatewayError"];
                    }

                    Database db = DatabaseFactory.CreateDatabase("cnPASS");
                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();

                        SqlCommand cmdCheck = new SqlCommand("GET_EXISTING_PAYMENT_DATA", con);
                        cmdCheck.CommandType = CommandType.StoredProcedure;
                        cmdCheck.Parameters.AddWithValue("@txnid", txnid);
                        Object intCount = cmdCheck.ExecuteScalar();
                        int returnCnt = Convert.ToInt32(intCount);

                        if (returnCnt > 0)
                        {
                            SqlCommand cmdHistory = new SqlCommand("INSERT_PAYMENT_DATA_HISTORY", con);
                            cmdHistory.CommandType = CommandType.StoredProcedure;
                            cmdHistory.Parameters.AddWithValue("@txnid", txnid);
                            cmdHistory.ExecuteNonQuery();
                        }

                        SqlCommand command = new SqlCommand("INSERT_PAYMENT_DETAILS", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@productType", produtinfo);
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

                    }

                    // lblProposalNumber.Text = txnid;
                    lblTransactionId.Text = mihpayid;
                    // lblQuoteNumber.Text = quoteNumber;
                    lblQuoteNumberFailed.Text = quoteNumber;
                    lblProposalNumberFailed.Text = txnid;

                    if (Convert.ToInt32(status) == 1)
                    {
                        PaymentEntryAPI();

                        if (String.IsNullOrEmpty(PaymentId_Entry))
                        {
                            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PageLoad() payment id is null for proposal number : " + txnid + "redirecting to error page " + DateTime.Now + Environment.NewLine);
                           // SendMail(); //changes to be done in the template
                            Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId, true);
                        }
                        else if (!String.IsNullOrEmpty(PaymentId_Entry))
                        {
                            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PageLoad() payment id for proposal number : " + txnid + " is " + PaymentId_Entry + DateTime.Now + Environment.NewLine);
                            PaymentAllocationAPI();

                            if (string.IsNullOrEmpty(paymentId_allocation))
                            {
                                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PageLoad() payment allocation id is null for proposal number : " + txnid + "redirecting to error page " + DateTime.Now + Environment.NewLine);
                               // SendMail(); //changes to be done in the template
                                Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId, true);
                            }
                            else
                            {
                                PaymentTaggingAPI();
                                if (string.IsNullOrEmpty(lblPolicyNumber.Text))
                                {
                                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PageLoad() policy number is null for proposal number : " + txnid + "redirecting to error page " + DateTime.Now + Environment.NewLine);
                                   // SendMail(); //changes to be done in the template
                                    Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId, true);
                                }
                            }
                        }


                        divFailure.Visible = false;
                        divSuccess.Visible = true;
                        //UpdatePaymentStatus(); already commented
                       // SendMail();
                        SendSMS();
                    }
                    else
                    {
                        divSuccess.Visible = false;
                        divFailure.Visible = true;
                        UpdatePaymentStatus();
                        lblError.Text = error_Message;

                    }
                //}
            }
            }

            catch(ThreadAbortException ex)
            {
               
            }
            catch(Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in PageLoad " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmErrorPage.aspx?pay="+encryptStatus+"&txn="+encryptTxnId);
            }
            

        }
        #region commented code
        //private void CheckifPageRefreshed()
        //{
        //    try
        //    {
        //        File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::start of the method: CheckifPageRefreshed  for proposal number " + txnid + " " + DateTime.Now + Environment.NewLine);

        //        Database dbCheck = DatabaseFactory.CreateDatabase("cnPASS");
        //        using (SqlConnection con = new SqlConnection(dbCheck.ConnectionString))
        //        {
        //            con.Open();

        //            SqlCommand cmdCheck = new SqlCommand("PROC_GET_MAPPING_DATA_PROPOSAL", con);
        //            cmdCheck.CommandType = CommandType.StoredProcedure;
        //            cmdCheck.Parameters.AddWithValue("@txnid", txnid);
        //            Object intCount = cmdCheck.ExecuteScalar();
        //            int returnCnt = Convert.ToInt32(intCount);

        //            if (returnCnt > 0) //get the number of record count
        //            {
        //                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::mapping proposal data count :" + returnCnt + " for proposal number " + txnid + " " + DateTime.Now + Environment.NewLine);
        //                Response.Redirect("FrmCustomErrorPage.aspx", true);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in CheckifPageRefreshed " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
        //        Response.Redirect("FrmCustomErrorPage.aspx",true);
        //    }
        //}
        #endregion

        private void PaymentTaggingAPI()
        {
            try
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Start PaymentTaggingAPI " + DateTime.Now + Environment.NewLine);
                string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
                string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                ServiceReference1.ServiceResult objServiceResult = new ServiceReference1.ServiceResult();
                objServiceResult.UserData = new ServiceReference1.clsUserData();
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestDataXMLPaymentTaggingAPI());
                proxy.PaymentTagging(strUserId, strPassword, ref objServiceResult);
                string Policynumber = objServiceResult.UserData.PolicyNO;

                if (objServiceResult.UserData.ErrorText.Length > 0)
                {
                    paymentTagggingError = objServiceResult.UserData.ErrorText;
                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PaymentTaggingAPI() Error occured in paymententry for proposal number " + txnid + " and error is : " + objServiceResult.UserData.ErrorText + DateTime.Now + Environment.NewLine);
                    //Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);

                    Database db = DatabaseFactory.CreateDatabase("cnPASS");
                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("update_payment_tagging_error_details", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@proposalNumber", txnid);
                        command.Parameters.AddWithValue("@error", objServiceResult.UserData.ErrorText);
                        command.ExecuteNonQuery();
                    }

                }
                else
                {
                    lblPolicyNumber.Text = Policynumber;
                    Database db = DatabaseFactory.CreateDatabase("cnPASS");
                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();
                        //to update policy and covernote number
                        SqlCommand command = new SqlCommand("update_payment_tagging_details", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@proposalNumber", txnid);
                        command.Parameters.AddWithValue("@paymentId", PaymentId_Entry);
                        command.Parameters.AddWithValue("@paymentAllocationId", paymentId_allocation);
                        command.Parameters.AddWithValue("@policyNumber", Policynumber);
                        command.ExecuteNonQuery();
                    }

                    //Email to customer policy schedule
                    SendMailToCustomer();
                }



            }
            catch(Exception ex)
            {

            }
        }

        private string GetRequestDataXMLPaymentTaggingAPI()
        {
            try
            {
                string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
                string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();
                string DataString = string.Empty;
                string FileName = string.Empty;
                string strXmlPath = "";
                strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "XmlRequest\\PaymentTagging.xml";
                XmlNode node = null;
                XmlDocument xmlfile = null;
                string xmlString = "";

                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);

                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();

                    SqlCommand cmdCheck = new SqlCommand("get_policy_start_date", con);
                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.Parameters.AddWithValue("@proposalNumber", txnid);
                    //Object retDate = cmdCheck.ExecuteScalar();
                    //policyStartDate = Convert.ToString(retDate);

                    SqlDataAdapter sda = new SqlDataAdapter(cmdCheck);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                       policyStartDate = ds.Tables[0].Rows[0]["PolicyStartDate"].ToString();
                       proposalStartDate =  ds.Tables[0].Rows[0]["ProposalStartDate"].ToString();
                    }
                }

                    DateTime dtAppDate = DateTime.Now;
                string strCurrentDate = dtAppDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/IntermediaryId");
                node.Attributes["Value"].Value = intermediaryCode;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/BusinessChannelId");
                node.Attributes["Value"].Value = intermediaryCode;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/OfficeCodeUnmasked");
                node.Attributes["Value"].Value = intermediaryLocCode;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/OfficeCode");
                node.Attributes["Value"].Value = intermediaryLocCode;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsPaymentIdGrid/ClsPaymentIdGrid/CollectionOffice");
                node.Attributes["Value"].Value = intermediaryLocCode;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/InstrumentNo");
                node.Attributes["Value"].Value = mihpayid;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/InstrumentAmount");
                node.Attributes["Value"].Value = amount;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/UserId");
                node.Attributes["Value"].Value = strUserId;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/TransactionTime");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/TransactionId");
                node.Attributes["Value"].Value = applicationNumber;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/CustomerName");
                node.Attributes["Value"].Value = firstName;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/PolicyStartDate");
                node.Attributes["Value"].Value = policyStartDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/PropApplicationNo");
                node.Attributes["Value"].Value = applicationNumber;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/PropCustId");
                node.Attributes["Value"].Value = custID;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/ProposalAmount");
                node.Attributes["Value"].Value = amount;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/ProposalDate");
                node.Attributes["Value"].Value = proposalStartDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/ProposalNo");
                node.Attributes["Value"].Value = txnid;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/TransactionDate");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsProposalDetailsGrd/ClsProposalDetailsGrd/UnpaidAmount");
                node.Attributes["Value"].Value = amount;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsPaymentIdGrid/ClsPaymentIdGrid/ApplicationNo");
                node.Attributes["Value"].Value = applicationNumber;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsPaymentIdGrid/ClsPaymentIdGrid/BalanceAmount");
                node.Attributes["Value"].Value = amount;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsPaymentIdGrid/ClsPaymentIdGrid/PaymenID");
                node.Attributes["Value"].Value = paymentId_allocation;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsPaymentIdGrid/ClsPaymentIdGrid/PaymentApplicationNo");
                node.Attributes["Value"].Value = applicationNumber;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsPaymentIdGrid/ClsPaymentIdGrid/PaymentDate");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentTagging/ClsPaymentIdGrid/ClsPaymentIdGrid/TotalAmount");
                node.Attributes["Value"].Value = amount;

                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");
                DataString = xmlString.ToString();
                FileName = txnid +"_" + "PaymentTagging_Request.xml";
                System.IO.File.WriteAllText(folderPath + "\\" + FileName, DataString);

                return xmlString;
            }
            catch(Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in GetRequestDataXMLPaymentTaggingAPI " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                return "";
            }
        }

        private void PaymentAllocationAPI()
        {
            try
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Start PaymentAllocationAPI " + DateTime.Now + Environment.NewLine);
                string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
                string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();                

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                ServiceReference1.ServiceResult objServiceResult = new ServiceReference1.ServiceResult();
                objServiceResult.UserData = new ServiceReference1.clsUserData();
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestDataXMLPaymentAllocationAPI());
                proxy.PaymentAllocation(strUserId, strPassword, ref objServiceResult);
                paymentId_allocation = objServiceResult.UserData.PaymentId;

                if (objServiceResult.UserData.ErrorText.Length > 0)
                {
                    paymentAllocationError = objServiceResult.UserData.ErrorText;

                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PaymentAllocationAPI() Error occured in paymententry for proposal number " + txnid + " and error is : " + objServiceResult.UserData.ErrorText + DateTime.Now + Environment.NewLine);
                    //Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
                    Database db = DatabaseFactory.CreateDatabase("cnPASS");
                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("update_payment_allocation_error_details", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@proposalNumber", txnid);
                        command.Parameters.AddWithValue("@error", objServiceResult.UserData.ErrorText);                        
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    
                    Database db = DatabaseFactory.CreateDatabase("cnPASS");
                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("update_payment_allocation_details", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@proposalNumber", txnid);
                        command.Parameters.AddWithValue("@paymentId", PaymentId_Entry);
                        command.Parameters.AddWithValue("@paymentAllocationId", paymentId_allocation);                        
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch(Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in PaymentAllocationAPI " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
            }
        }

        private string GetRequestDataXMLPaymentAllocationAPI()
        {
            try
            {
                string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
                string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();
                string DataString = string.Empty;
                string FileName = string.Empty;
                string strXmlPath = "";
                strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "\\XmlRequest\\PaymentAllocation.xml";
                XmlNode node = null;
                XmlDocument xmlfile = null;
                string xmlString = "";

                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentAllocation/OfficeCd");
                node.Attributes["Value"].Value = intermediaryLocCode;

                DateTime dtAppDate = DateTime.Now;
                string strCurrentDate = dtAppDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentAllocation/ParentPaymentID");
                node.Attributes["Value"].Value = PaymentId_Entry;
                
                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentAllocation/CustID");
                node.Attributes["Value"].Value = custID;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentAllocation/ApplicationNo");
                node.Attributes["Value"].Value = applicationNumber;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentAllocation/Amount");
                node.Attributes["Value"].Value = amount;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentAllocation/TransactionID");
                node.Attributes["Value"].Value = applicationNumber;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentAllocation/UserID");
                node.Attributes["Value"].Value = strUserId;

                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");
                DataString = xmlString.ToString();
                FileName = txnid + "_" + "PaymentAllocation_Request.xml";
                System.IO.File.WriteAllText(folderPath + "\\" + FileName, DataString);
                return xmlString;

            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in GetRequestDataXMLPaymentAllocationAPI " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                return "";
            }
        }

        private void PaymentEntryAPI()
        {
            try
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Start PaymentEntryAPI " + DateTime.Now + Environment.NewLine);
                string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
                string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                ServiceReference1.ServiceResult objServiceResult = new ServiceReference1.ServiceResult();
                objServiceResult.UserData = new ServiceReference1.clsUserData();             
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestDataXMLPaymentEntryAPI());
                objServiceResult.UserData.IsInternalRiskGrid = true;
                objServiceResult.UserData.WorksheetInByte = true;

                if (String.IsNullOrEmpty(objServiceResult.UserData.ConsumeProposalXML.ToString()))
                {
                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PaymentEntryAPI() ConsumeProposalXML is null for proposal number " + txnid + DateTime.Now + Environment.NewLine);
                    Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
                }

                proxy.PaymentEntry(strUserId, strPassword, ref objServiceResult);
                double paymentamount = objServiceResult.UserData.PaymentAmount;
                string ResultXML = objServiceResult.UserData.UserResultXml;

                
                string FileName = txnid + "_" + "PaymentEntry_Response.xml";
                System.IO.File.WriteAllText(folderPath + "\\" + FileName, ResultXML);

                if (objServiceResult.UserData.ErrorText.Length > 0)
                {
                    paymentIdError = objServiceResult.UserData.ErrorText;
                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::PaymentEntryAPI() Error occured in paymententry for proposal number " + txnid + " and error is : " + objServiceResult.UserData.ErrorText +  DateTime.Now + Environment.NewLine);

                    Database db = DatabaseFactory.CreateDatabase("cnPASS");
                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("insert_payment_error_details", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@proposalNumber", txnid);
                        command.Parameters.AddWithValue("@error", objServiceResult.UserData.ErrorText);                        
                        command.Parameters.AddWithValue("@custID", custID);
                        command.ExecuteNonQuery();
                    }


                    //Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
                }
                else
                {
                    PaymentId_Entry = objServiceResult.UserData.PaymentId;
                    Database db = DatabaseFactory.CreateDatabase("cnPASS");
                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("insert_payment_entry_details", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@proposalNumber", txnid);
                        command.Parameters.AddWithValue("@paymentID", PaymentId_Entry);
                        command.Parameters.AddWithValue("@paymentAmt", amount);
                        command.Parameters.AddWithValue("@proposalAmt", amount);
                        command.Parameters.AddWithValue("@custID", custID);
                        //command.ExecuteNonQuery();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            PaymentId_Entry = reader[0].ToString();

                        }
                        reader.Close();


                    }
                }
            }
            catch(Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in PaymentEntryAPI " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
            }
        }

        private string GetRequestDataXMLPaymentEntryAPI()
        {
            try
            {
                string strXmlPath = "";
                XmlNode node = null;
                XmlDocument xmlfile = null;
                string xmlString = "", FileName="", DataString = "";

                strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "\\XmlRequest\\PaymentEntry.xml";

                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);

                GetIntermediaryBusinessDetails();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/IntermediaryCode");
                node.Attributes["Value"].Value = intermediaryCode;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/ProducerCode");
                node.Attributes["Value"].Value = intermediaryCode;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/BusinessLocation");
                node.Attributes["Value"].Value = intermediaryLocCode;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/DepositOfficeCode");
                node.Attributes["Value"].Value = intermediaryLocCode;

                string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/TransactionTime");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/PayerCustomerId");
                node.Attributes["Value"].Value = custID;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/PaymentNumber");
                node.Attributes["Value"].Value = mihpayid; //Pay You Transaction Id

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/PaymentDate");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/DepositAdviceDate");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/DecimalPaymentAmount");
                node.Attributes["Value"].Value = amount;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/UserId");
                node.Attributes["Value"].Value = ConfigurationManager.AppSettings["strUserId"];

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/HoRefDate");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/PaymentEntry/ReceiptDate");
                node.Attributes["Value"].Value = strCurrentDate;

                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");
                DataString = xmlString.ToString();
                FileName =  txnid +"_"+ "PaymentEntry_Request.xml";
                System.IO.File.WriteAllText(folderPath+"\\"+FileName, DataString);
                return xmlString;

            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in GetRequestDataXMLPaymentEntryAPI " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                return "";                
            }
        }

        private void GetIntermediaryBusinessDetails()
        {
           try
            {               
                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();

                    SqlCommand command = new SqlCommand("GET_INTERCODE_FROM_PROPOSAL", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@propNumber", txnid);

                    SqlDataAdapter sdaCode = new SqlDataAdapter(command);
                    DataSet dsCode = new DataSet();
                    sdaCode.Fill(dsCode);

                    if (dsCode.Tables[0].Rows.Count > 0)
                    {
                        intermediaryCode = dsCode.Tables[0].Rows[0]["PropIntermediaryDetails_IntermediaryCode"].ToString();
                        custID = dsCode.Tables[0].Rows[0]["CustomerId"].ToString();
                        applicationNumber = dsCode.Tables[0].Rows[0]["ApplicationNumber"].ToString();
                    }

                    if (!string.IsNullOrEmpty(intermediaryCode))
                    {
                        SqlCommand cmdIntermediary = new SqlCommand("PROC_GET_INTERMEDIARY_BUSINESS_CHANNEL_TYPE", con);
                        cmdIntermediary.CommandType = CommandType.StoredProcedure;
                        cmdIntermediary.Parameters.AddWithValue("@TXT_INTERMEDIARY_CD", intermediaryCode);
                        SqlDataAdapter sda = new SqlDataAdapter(cmdIntermediary);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            intermediaryLocCode = ds.Tables[0].Rows[0]["NUM_OFFICE_CD"].ToString();                            
                            intermediaryLocName = ds.Tables[0].Rows[0]["TXT_OFFICE"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in GetIntermediaryBusinessDetails :  error message is " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
            }
        }

        private void UpdatePaymentStatus()
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();

                    SqlCommand cmdCheck = new SqlCommand("UPDATE_PAYMENT_STATUS", con);
                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.Parameters.AddWithValue("@propNumber", txnid);
                    cmdCheck.Parameters.AddWithValue("@status", status == "1" ? "SUCCESS" : "FAILURE");
                    cmdCheck.ExecuteNonQuery();

                }
            }
            catch(Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in UpdatePaymentStatus for proposal number : " + txnid + " and error message is " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
            }
        
        }

        private void SendSMS()
        {
            //to do
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnConnect");

                string messageString = ConfigurationManager.AppSettings["messageString"];

                messageString = messageString.Replace("@proposalNumber",txnid);
                messageString = messageString.Replace("@intCode", intermediaryCode);
                messageString = messageString.Replace("@payutxnid", mihpayid);
                messageString = messageString.Replace("@quoteNumber", quoteNumber);

                if (eposFlag.ToLower() != "epos")
                {

                    string mobileNum = ConfigurationManager.AppSettings["mobilenumbers"];
                    if (!String.IsNullOrEmpty(mobileNum))
                    {
                        string[] arrMobile = mobileNum.Split(',');

                        using (SqlConnection con = new SqlConnection(db.ConnectionString))
                        {
                            con.Open();

                            for (int i = 0; i < arrMobile.Count(); i++)
                            {
                                if (!String.IsNullOrEmpty(arrMobile[i]))
                                {
                                    SqlCommand cmdCheck = new SqlCommand("INSERT_DATA_SMS_LOG", con);
                                    cmdCheck.CommandType = CommandType.StoredProcedure;
                                    cmdCheck.Parameters.AddWithValue("@mobile", arrMobile[i]);
                                    cmdCheck.Parameters.AddWithValue("@msg", messageString);
                                    cmdCheck.ExecuteNonQuery();
                                }

                            }
                        }
                    }
                }
                else if(eposFlag.ToLower() == "epos")
                {
                    string mobileNum = ConfigurationManager.AppSettings["mobilenumbers_epos"];
                    if (!String.IsNullOrEmpty(mobileNum))
                    {
                        string[] arrMobile = mobileNum.Split(',');

                        using (SqlConnection con = new SqlConnection(db.ConnectionString))
                        {
                            con.Open();

                            for (int i = 0; i < arrMobile.Count(); i++)
                            {
                                if (!String.IsNullOrEmpty(arrMobile[i]))
                                {
                                    SqlCommand cmdCheck = new SqlCommand("INSERT_DATA_SMS_LOG", con);
                                    cmdCheck.CommandType = CommandType.StoredProcedure;
                                    cmdCheck.Parameters.AddWithValue("@mobile", arrMobile[i]);
                                    cmdCheck.Parameters.AddWithValue("@msg", messageString);
                                    cmdCheck.ExecuteNonQuery();
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in SendSMS " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
            }
        }

        private void SendSMSToCustomer()
        {
            //to do
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnConnect");

                string messageString = ConfigurationManager.AppSettings["messageStringForCustomer"];

                messageString = messageString.Replace("@payutxnid", mihpayid);
                
                string mobileNum = mobile;
                    if (!String.IsNullOrEmpty(mobileNum))
                    {
                       

                        using (SqlConnection con = new SqlConnection(db.ConnectionString))
                        {
                            con.Open();
                            SqlCommand cmdCheck = new SqlCommand("INSERT_DATA_CUSTOMER_SMS_LOG", con);
                            cmdCheck.CommandType = CommandType.StoredProcedure;
                            cmdCheck.Parameters.AddWithValue("@mobile", mobile);
                            cmdCheck.Parameters.AddWithValue("@msg", messageString);
                            cmdCheck.ExecuteNonQuery();
                        }
                    }
               
                
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in SendSMSToCustomer " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
            }
        }

        //private void SendMail()
        //{
        //    try
        //    {

        //        string intermediaryLocCode = string.Empty;
        //        string intermediaryLocName = string.Empty;
        //        string customerID = string.Empty;


        //        Database db = DatabaseFactory.CreateDatabase("cnPASS");
        //        using (SqlConnection con = new SqlConnection(db.ConnectionString))
        //        {
        //            con.Open();                    

        //            SqlCommand command = new SqlCommand("GET_INTERCODE_FROM_PROPOSAL", con);
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@propNumber", txnid);

        //            SqlDataAdapter sdaCode = new SqlDataAdapter(command);
        //            DataSet dsCode = new DataSet();
        //            sdaCode.Fill(dsCode);

        //            if (dsCode.Tables[0].Rows.Count > 0)
        //            {
        //                intermediaryCode = dsCode.Tables[0].Rows[0]["PropIntermediaryDetails_IntermediaryCode"].ToString();
        //                customerID = dsCode.Tables[0].Rows[0]["CustomerId"].ToString();
        //            }

        //                if (!string.IsNullOrEmpty(intermediaryCode))
        //            {
        //                SqlCommand cmdIntermediary = new SqlCommand("PROC_GET_INTERMEDIARY_BUSINESS_CHANNEL_TYPE", con);
        //                cmdIntermediary.CommandType = CommandType.StoredProcedure;
        //                cmdIntermediary.Parameters.AddWithValue("@TXT_INTERMEDIARY_CD", intermediaryCode);
        //                SqlDataAdapter sda = new SqlDataAdapter(cmdIntermediary);
        //                DataSet ds = new DataSet();
        //                sda.Fill(ds);

        //                if (ds.Tables[0].Rows.Count > 0)
        //                {                            
        //                    intermediaryLocCode = ds.Tables[0].Rows[0]["NUM_OFFICE_CD"].ToString();
        //                    intermediaryLocName = ds.Tables[0].Rows[0]["TXT_OFFICE"].ToString();
        //                }
        //            }

        //        }

        //        if(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["careEmail"].Trim()))
        //        {

        //            string emailId = ConfigurationManager.AppSettings["careEmail"].Trim();
        //            string[] arrMobile = emailId.Split(',');

        //        }

        //        if (Regex.IsMatch(ConfigurationManager.AppSettings["careEmail"].Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
        //        {
        //            string strPath = string.Empty;
        //            string MailBody = string.Empty;
        //            SmtpClient smtpClient = new SmtpClient();
        //            smtpClient.Port = 25;
        //            smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
        //            smtpClient.Timeout = 3600000;
        //            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            smtpClient.UseDefaultCredentials = false;
        //            smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);

        //            MailBody = "We have received payment against proposal number : " + txnid;

        //            strPath = ConfigurationManager.AppSettings["email_body"];
        //            MailBody = File.ReadAllText(strPath);

        //            MailBody = MailBody.Replace("@propNumber", txnid);
        //            MailBody = MailBody.Replace("@officeCode", intermediaryLocCode);
        //            MailBody = MailBody.Replace("@officeName", intermediaryLocName);
        //            MailBody = MailBody.Replace("@intermediaryId", intermediaryCode);
        //            MailBody = MailBody.Replace("@custId", customerID);
        //            MailBody = MailBody.Replace("@custName", firstName);
        //            MailBody = MailBody.Replace("@transactionID", mihpayid);

        //            MailMessage mm = new MailMessage();
        //            mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
        //            mm.Subject = "Payment Confirmation for Intermediary : " + intermediaryCode;
        //            mm.Body = MailBody;
        //            //mm.Body = "mail";
        //            mm.IsBodyHtml = true;

        //            mm.To.Add(ConfigurationManager.AppSettings["careEmail"]);

        //            mm.BodyEncoding = UTF8Encoding.UTF8;
        //            smtpClient.Send(mm);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in SendMail " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
        //        Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
        //    }
        //}

        private void SendMail()
        {
            try
            {

                //string intermediaryLocCode = string.Empty;
                //string intermediaryLocName = string.Empty;
                //string customerID = string.Empty;


                //Database db = DatabaseFactory.CreateDatabase("cnPASS");
                //using (SqlConnection con = new SqlConnection(db.ConnectionString))
                //{
                //    con.Open();

                //    SqlCommand command = new SqlCommand("GET_INTERCODE_FROM_PROPOSAL", con);
                //    command.CommandType = CommandType.StoredProcedure;
                //    command.Parameters.AddWithValue("@propNumber", txnid);

                //    SqlDataAdapter sdaCode = new SqlDataAdapter(command);
                //    DataSet dsCode = new DataSet();
                //    sdaCode.Fill(dsCode);

                //    if (dsCode.Tables[0].Rows.Count > 0)
                //    {
                //        intermediaryCode = dsCode.Tables[0].Rows[0]["PropIntermediaryDetails_IntermediaryCode"].ToString();
                //        customerID = dsCode.Tables[0].Rows[0]["CustomerId"].ToString();
                //    }

                //    if (!string.IsNullOrEmpty(intermediaryCode))
                //    {
                //        SqlCommand cmdIntermediary = new SqlCommand("PROC_GET_INTERMEDIARY_BUSINESS_CHANNEL_TYPE", con);
                //        cmdIntermediary.CommandType = CommandType.StoredProcedure;
                //        cmdIntermediary.Parameters.AddWithValue("@TXT_INTERMEDIARY_CD", intermediaryCode);
                //        SqlDataAdapter sda = new SqlDataAdapter(cmdIntermediary);
                //        DataSet ds = new DataSet();
                //        sda.Fill(ds);

                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            intermediaryLocCode = ds.Tables[0].Rows[0]["NUM_OFFICE_CD"].ToString();
                //            businessLocation = ds.Tables[0].Rows[0]["NUM_OFFICE_CD"].ToString();
                //            intermediaryLocName = ds.Tables[0].Rows[0]["TXT_OFFICE"].ToString();
                //        }
                //    }

                //}

                if (eposFlag.ToLower() != ConfigurationManager.AppSettings["epos_user"].Trim())
                {
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["careEmail"].Trim()))
                    {

                        string emailId = ConfigurationManager.AppSettings["careEmail"].Trim();
                        string[] arrMail = emailId.Split(',');

                        string strPath = string.Empty;
                        string MailBody = string.Empty;
                        SmtpClient smtpClient = new SmtpClient();
                        smtpClient.Port = 25;
                        smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                        smtpClient.Timeout = 3600000;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);

                        MailBody = "We have received payment against proposal number : " + txnid;

                        strPath = ConfigurationManager.AppSettings["email_body"];
                        MailBody = File.ReadAllText(strPath);

                        MailBody = MailBody.Replace("@propNumber", txnid);
                        MailBody = MailBody.Replace("@officeCode", intermediaryLocCode);
                        MailBody = MailBody.Replace("@officeName", intermediaryLocName);
                        MailBody = MailBody.Replace("@intermediaryId", intermediaryCode);
                        MailBody = MailBody.Replace("@custId", custID);
                        MailBody = MailBody.Replace("@custName", firstName);
                        MailBody = MailBody.Replace("@transactionID", mihpayid);
                        MailBody = MailBody.Replace("@Amount", amount);                        
                        MailBody = MailBody.Replace("@PaymentID", PaymentId_Entry != "" ? PaymentId_Entry : paymentIdError);
                        MailBody = MailBody.Replace("@PaymentAllocationID", paymentId_allocation != "" ? paymentId_allocation : paymentAllocationError);
                        MailBody = MailBody.Replace("@PolicyNumber", lblPolicyNumber.Text != "" ? lblPolicyNumber.Text : paymentTagggingError);

                        MailMessage mm = new MailMessage();
                        mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                        mm.Subject = "Payment Confirmation for Intermediary : " + intermediaryCode;
                        mm.Body = MailBody;
                        //mm.Body = "mail";
                        mm.IsBodyHtml = true;

                        for (int i = 0; i < arrMail.Count(); i++)
                        {

                            if (Regex.IsMatch(arrMail[i].Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                            {
                                mm.To.Add(arrMail[i]);

                            }
                        }

                        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["replyToEmail"]))
                        {
                            MailAddress adminAddress = new MailAddress(ConfigurationManager.AppSettings["replyToEmail"]);
                            mm.ReplyTo = adminAddress;
                        }

                        mm.BodyEncoding = UTF8Encoding.UTF8;
                        smtpClient.Send(mm);


                    }
                }
                else if(eposFlag.ToLower()== ConfigurationManager.AppSettings["epos_user"].Trim())
                {
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["careEmail_epos"].Trim()))
                    {

                        string emailId = ConfigurationManager.AppSettings["careEmail_epos"].Trim();
                        string[] arrMail = emailId.Split(',');

                        string strPath = string.Empty;
                        string MailBody = string.Empty;
                        SmtpClient smtpClient = new SmtpClient();
                        smtpClient.Port = 25;
                        smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                        smtpClient.Timeout = 3600000;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);

                        MailBody = "We have received payment against proposal number : " + txnid;

                        strPath = ConfigurationManager.AppSettings["email_body"];
                        MailBody = File.ReadAllText(strPath);

                        MailBody = MailBody.Replace("@propNumber", txnid);
                        MailBody = MailBody.Replace("@officeCode", intermediaryLocCode);
                        MailBody = MailBody.Replace("@officeName", intermediaryLocName);
                        MailBody = MailBody.Replace("@intermediaryId", intermediaryCode);
                        MailBody = MailBody.Replace("@custId", custID);
                        MailBody = MailBody.Replace("@custName", firstName);
                        MailBody = MailBody.Replace("@transactionID", mihpayid);
                        MailBody = MailBody.Replace("@Amount", amount);
                        MailBody = MailBody.Replace("@PaymentID", PaymentId_Entry != "" ? PaymentId_Entry : paymentIdError);
                        MailBody = MailBody.Replace("@PaymentAllocationID", paymentId_allocation != "" ? paymentId_allocation : paymentAllocationError);
                        MailBody = MailBody.Replace("@PolicyNumber", lblPolicyNumber.Text != "" ? lblPolicyNumber.Text : paymentTagggingError);

                        MailMessage mm = new MailMessage();
                        mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                        mm.Subject = "Payment Confirmation for Intermediary : " + intermediaryCode;
                        mm.Body = MailBody;
                        //mm.Body = "mail";
                        mm.IsBodyHtml = true;

                        for (int i = 0; i < arrMail.Count(); i++)
                        {

                            if (Regex.IsMatch(arrMail[i].Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                            {
                                mm.To.Add(arrMail[i]);

                            }
                        }

                        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["replyToEmail"]))
                        {
                            MailAddress adminAddress = new MailAddress(ConfigurationManager.AppSettings["replyToEmail"]);
                            mm.ReplyTo = adminAddress;
                        }

                        mm.BodyEncoding = UTF8Encoding.UTF8;
                        smtpClient.Send(mm);


                    }
                }
                
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in SendMail " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
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
            catch(Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in EncryptText " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
                return null;
            }
            
         

        }

        protected void btnGetPolicy_Click(object sender, EventArgs e)
        {
            try
            {
                string ErrorMsg = string.Empty;
                PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
                //byte[] objByte = proxy.KGIGetPolicyDocumentForPortal("81062f2fc69b4639af5bf33e86c66408", lblPolicyNumber.Text, "3121", ref ErrorMsg); //1000401000 //1000340100
                byte[] objByte = proxy.KGIGetPolicyDocumentForPASS("81062f2fc69b4639af5bf33e86c66408", lblPolicyNumber.Text, "3121", txnid, ref ErrorMsg); //1000401000 //1000340100
                string fileName = lblPolicyNumber.Text + ".pdf";
                if (ErrorMsg.Trim().Length <= 0)
                {
                    Response.Clear();
                    Response.ContentType = "application/force-download";
                    //Response.AddHeader("content-disposition", "attachment;filename=1010404900.pdf");
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.BinaryWrite(objByte);
                    Response.End();
                 
                }
            }
            catch(Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in btnGetPolicy_Click " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
              //  Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
            }
        }


        private void SendMailToCustomer()
        {
            try
            {
                //save pdf in the folder

                string ErrorMsg = string.Empty;
                PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
                //byte[] objByte = proxy.KGIGetPolicyDocumentForPortal("81062f2fc69b4639af5bf33e86c66408", lblPolicyNumber.Text, "3121", ref ErrorMsg); //1000401000 //1000340100
                byte[] objByte = proxy.KGIGetPolicyDocumentForPASS("81062f2fc69b4639af5bf33e86c66408", lblPolicyNumber.Text, "3121", txnid, ref ErrorMsg); //1000401000 //1000340100
                string fileName = lblPolicyNumber.Text + ".pdf";
                if (ErrorMsg.Trim().Length <= 0)
                {
                   // byte[] outPdfBuffer = clsDigitalCertificate.Sign(objByte);
                    File.WriteAllBytes(ConfigurationManager.AppSettings["policy_pdf"] + "\\" + fileName, objByte);
                    
                }
                else
                {
                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error message return while generating pdf for " + lblPolicyNumber.Text + " and error is : " + ErrorMsg.Trim() + " " + DateTime.Now + Environment.NewLine);
                    Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId,true);
                }

                if (!String.IsNullOrEmpty(email.Trim()))
                    {
                        
                        string strPath = string.Empty;
                        string MailBody = string.Empty;
                        SmtpClient smtpClient = new SmtpClient();
                        smtpClient.Port = 25;
                        smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                        smtpClient.Timeout = 3600000;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);

                        MailBody = "We have received payment against proposal number : " + txnid;

                        strPath = ConfigurationManager.AppSettings["email_body_customer"];
                        MailBody = File.ReadAllText(strPath);

                        MailBody = MailBody.Replace("@productName", "Private Car");
                        MailBody = MailBody.Replace("@policyNumber", lblPolicyNumber.Text);
                        MailBody = MailBody.Replace("@payuMerchantId", mihpayid);
                        

                        MailMessage mm = new MailMessage();
                        mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                        mm.Subject = "Payment Successful";
                        mm.Body = MailBody;
                        //mm.Body = "mail";
                        mm.IsBodyHtml = true;
                    string attachmentFilename = ConfigurationManager.AppSettings["policy_pdf"] + "\\" + fileName;

                    Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;

                    mm.Attachments.Add(attachment);
                    mm.To.Add(email);

                        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["replyToEmail"]))
                        {
                            MailAddress adminAddress = new MailAddress(ConfigurationManager.AppSettings["replyToEmail"]);
                            mm.ReplyTo = adminAddress;
                        }

                        mm.BodyEncoding = UTF8Encoding.UTF8;
                        smtpClient.Send(mm);


                    }
                
                

            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmPaymentConfirmation.aspx ::Error occured in SendMailToCustomer " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmErrorPage.aspx?pay=" + encryptStatus + "&txn=" + encryptTxnId);
            }
        }


    }
}