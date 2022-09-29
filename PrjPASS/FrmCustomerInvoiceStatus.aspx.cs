using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmCustomerInvoiceStatus : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!IsPostBack)
                {
                    //if (Session["loaded"] == null) //to check refresh
                    //{
                    //    Session["loaded"] = "1";

                   // Directory.CreateDirectory(folderPath);

                    string[] keys = Request.Form.AllKeys;

                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (keys[i] == "mihpayid")
                        {
                            mihpayid = Request.Form[keys[i]];
                            lblPAYUTransactionId.Text = mihpayid;
                        }
                        if (keys[i] == "status")
                        {
                            status = Request.Form[keys[i]];
                            
                            lblThankYouOrSorry.Text = status.ToLower() == "success" ? "Thank You" : "Sorry";
                            lblPaymentStatus.Text = status.ToUpper();
                        }
                        if (keys[i] == "unmappedstatus")
                        {
                            unmappedstatus = Request.Form[keys[i]];
                            //Response.Write(unmappedstatus);
                        }
                        if (keys[i] == "txnid")
                        {
                            txnid = Request.Form[keys[i]];//strvpc_MerchTxnRef
                            //Response.Write(txnid);
                            lblKGITransactionId.Text = txnid;
                        }
                        if (keys[i] == "error_Message")
                        {
                            error_Message = Request.Form[keys[i]];
                            //Response.Write(error_Message);
                        }
                        if (keys[i] == "PG_TYPE")
                        {
                            PG_TYPE = Request.Form[keys[i]];
                            //Response.Write(PG_TYPE);
                        }
                        if (keys[i] == "bank_ref_num")
                        {
                            bank_ref_num = Request.Form[keys[i]];//strvpc_MerchTxnRef
                            //Response.Write(bank_ref_num);
                        }
                        if (keys[i] == "bankcode")
                        {
                            bankcode = Request.Form[keys[i]];
                            //Response.Write(bankcode);
                        }
                        if (keys[i] == "email")
                        {
                            email = Request.Form[keys[i]];
                            //Response.Write(email);
                            lblCustomerEmail.Text = MaskEmailId(email);
                        }

                        if (keys[i] == "phone")
                        {
                            mobile = Request.Form[keys[i]];
                            //Response.Write(mobile);
                            lblCustomerPhone.Text = MaskMobnumber(mobile);
                        }

                        if (keys[i] == "productinfo")
                        {
                            produtinfo = Request.Form[keys[i]];
                            //Response.Write(produtinfo);
                        }

                        if (keys[i] == "amount")
                        {
                            amount = Request.Form[keys[i]];
                            //Response.Write(amount);
                            lblTransactionAmount.Text = amount;
                        }

                        if (keys[i] == "firstname")
                        {
                            firstName = Request.Form[keys[i]];
                            //Response.Write(firstName);
                            lblCustomerName.Text = firstName;
                        }

                        if (keys[i] == "udf1")
                        {
                            quoteNumber = Request.Form[keys[i]];
                            //Response.Write(quoteNumber);
                        }

                        if (keys[i] == "udf2")
                        {
                            eposFlag = Request.Form[keys[i]];
                            //Response.Write(eposFlag);
                        }

                        if (keys[i] == "udf3")
                        {
                            custID = Request.Form[keys[i]];
                            //Response.Write(custID);
                        }
                    }

                    SaveTransactionStatus
                        (txnid, produtinfo, amount, firstName, email, mobile, mihpayid, status, unmappedstatus, error_Message);


                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmCustomerInvoiceStatus, Page Load");
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }


        }

        private string MaskEmailId(string EmailId)
        {
            string result = EmailId;
            try
            {
                string input = EmailId;
                string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
                result = Regex.Replace(input, pattern, m => new string('*', m.Length));
            }
            catch (Exception ex)
            {
                result = EmailId;
                ExceptionUtility.LogException(ex, "MaskEmailId");
            }
            return result;
        }

        private string MaskMobnumber(string mobNumber)
        {
            string result = mobNumber;
            try
            {
                result = mobNumber.Substring(mobNumber.Length - 4).PadLeft(mobNumber.Length, '*');
            }
            catch (Exception ex)
            {
                result = mobNumber;
                ExceptionUtility.LogException(ex, "MaskEmailId");
            }
            return result;
        }

        private void SaveTransactionStatus(string TransactionId, string ProductInfo, string Amount, string CustomerName, string CustomerEmailId
                                        , string  CustomerMobileNumber, string  PAYUTxnID, string  PaymentStatus, string  unmappedstatus, string  error_Message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_PAYU_INVOICE_LINKS_PAYMENT_STATUS";

                        cmd.Parameters.AddWithValue("@TransactionId", TransactionId);
                        cmd.Parameters.AddWithValue("@ProductInfo", ProductInfo);
                        cmd.Parameters.AddWithValue("@Amount", Amount);
                        cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                        cmd.Parameters.AddWithValue("@CustomerEmailId", CustomerEmailId);
                        cmd.Parameters.AddWithValue("@CustomerMobileNumber", CustomerMobileNumber);
                        cmd.Parameters.AddWithValue("@PAYUTxnID", PAYUTxnID);
                        cmd.Parameters.AddWithValue("@PaymentStatus", PaymentStatus);
                        cmd.Parameters.AddWithValue("@unmappedstatus", unmappedstatus);
                        cmd.Parameters.AddWithValue("@error_Message", error_Message);

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveTransactionStatus Method");
            }
        }
    }
}