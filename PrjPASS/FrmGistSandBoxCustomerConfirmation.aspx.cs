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


namespace PrjPASS
{
    public partial class FrmGistSandBoxCustomerConfirmation : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnvCustomerId.Value = DecryptText(Request.QueryString["vCustomerId"]); // To Be Un-Commented
                DataSet DS = GetCustomerData(hdnvCustomerId.Value);
                if (DS.Tables[0].Rows.Count > 0)
                {
                    if (DS.Tables[0].Rows[0]["vconfirmed"].ToString() == "1")
                    {
                        Alert.Show("Customer has alredy confirmed");
                        Response.Redirect("FrmGIStSandBoxThankYouPage.aspx");

                    }


                    hdnvMobileNo.Value = DS.Tables[0].Rows[0]["vMobileNo"].ToString();
                    hdnvPolicyNo.Value = DS.Tables[0].Rows[0]["vPolicyNumber"].ToString();
                    hdnvEmail.Value = DS.Tables[0].Rows[0]["vEmailId"].ToString();
                    lblCustomerName.Text = DS.Tables[0].Rows[0]["vCustomerName"].ToString();
                    lblPlace.Text= DS.Tables[0].Rows[0]["vPlace"].ToString();
                    lbldate.Text= DateTime.Now.ToString("dd/MM/yyyy").ToString();
                }

            }
            //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmCustomerReviewConfirm.aspx start of page loading for " + propNumber + " " + DateTime.Now + Environment.NewLine);



        }

        public static string DecryptText(string cipherText)
        {
            string EncryptionKey = "KGIMAV2BNI1907";
            byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        protected void btnMakePayment_Click(object sender, EventArgs e)
        {
            try
            {
                //otp code 

                int Num = int.Parse(hdnOTPSentCount.Value);

                if (Page.IsValid && Num < 3)
                {
                    //      anchorLink.Disabled = true;
                    Num = int.Parse(hdnOTPSentCount.Value) + 1;
                    hdnOTPSentCount.Value = Num.ToString();

                    string mobileno = hdnvMobileNo.Value;
                    string emailid = hdnvEmail.Value;
                    string name = lblCustomerName.Text;
                    string propNumber = hdnvPolicyNo.Value;
                    string generateOTP = GenerateOTP(mobileno, emailid, "", name, propNumber);

                    otpPanel.Visible = true;
                    btnMakePayment.Visible = false;
                    btnMobileVerify.Focus();
                    ScriptManager.RegisterStartupScript(UpdatePanel_Detail1, UpdatePanel_Detail1.GetType(), "testpage", "runme();", true);
                    UpdatePanel_Detail1.Update();
                }

                else
                {
                    //  anchorLink.Disabled = false;
                    cvtxtOtpNumber.IsValid = false;
                    cvtxtOtpNumber.ErrorMessage = "Maximum OTP Send limit is over, kindly contact nearest branch. Or Check your latest OTP";
                }
            }
            catch (Exception ex)
            {
                // File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in btnMakePayment :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                UpdateSandBoxTBL();
                Response.Redirect("FrmGIStSandBoxThankYouPage.aspx");
            }
        }

        protected void onClickbtnMobileVerify(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (txtOtpNumber.Text == HttpContext.Current.Session["OTPNumber"].ToString())
                {
                    bool otpFlag = UpdateOTPData(HttpContext.Current.Session["OTPId"].ToString(), hdnOTPSentCount.Value, hdnvPolicyNo.Value, Convert.ToInt32(txtOtpNumber.Text), "3121");
                    if (otpFlag)
                    {
                        UpdateSandBoxTBL();
                        Response.Redirect("FrmGIStSandBoxThankYouPage.aspx");
                        //Alert.Show("OTP is correct"); return;
                        //onclick_btnPayment(sender, e);
                    }
                    else
                    {

                        Alert.Show("OTP is not correct"); return;

                        //  File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in onClickbtnMobileVerify for proposal :" + proposal + " and is in else condition of otpflag " + DateTime.Now + Environment.NewLine);
                        //Response.Redirect("FrmCustomErrorPage.aspx");
                    }
                }
            }
        }
        protected void UpdateSandBoxTBL()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_UPD_SAVE_IRDA_GIST_SANDBOX";

                    cmd.Parameters.AddWithValue("@vCustomerId", hdnvCustomerId.Value);

                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (chkSummaryAgree.Checked)
            {
                e.IsValid = true;
                chkSummaryAgree.Disabled = true;
            }
            else
                e.IsValid = false;
            btnMakePayment.Focus();
        }
        private bool UpdateOTPData(string identity, string otpCount, string proposal, int otpNumber, string prodCode)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("UPDATE_OTP_DATA", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", Convert.ToInt32(identity));
                    command.Parameters.AddWithValue("@propNumber", proposal);
                    command.Parameters.AddWithValue("@OTPNoFromCustomer", otpNumber);
                    command.Parameters.AddWithValue("@productcode", prodCode);
                    command.Parameters.AddWithValue("@AttemptCount", Convert.ToInt32(otpCount));
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                //  File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in UpdateOTPData for proposal :" + proposal + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
            }
            return false;

        }

        protected void onClickbtnMobileReSend(object sender, EventArgs e)
        {
            btnMakePayment_Click(sender, e);
        }
        private string GenerateOTP(string mobileno, string emailid, string v, string name, string propNumber)
        {
            try
            {
                //mobileno = "7045041046";

                string strPath = string.Empty;
                string smsBody = string.Empty;

                Random r = new Random();
                int OTPNumber = r.Next(100000, 999999);
                DataSet ds = new DataSet();
                ds = InsertOTPData("0", mobileno, hdnvCustomerId.Value, lblCustomerName.Text, propNumber, OTPNumber.ToString(), "", "", "", "", "3121");

                smsBody = ConfigurationManager.AppSettings["smsBody"];
                //smsBody = File.ReadAllText(strPath);
                smsBody = smsBody.Replace("@otpNumber", Convert.ToString(OTPNumber));

                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "0")
                {
                   // string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
                    var client = new System.Net.WebClient();
                    var content = client.DownloadString(URI);
                }
                else
                {
                    string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                    string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                    string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();
                    string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                    string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();

                    string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();
                    WebProxy proxy = new WebProxy(proxy_Address_Full);
                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;

                    // string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);


                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                    var client = new System.Net.WebClient();
                    client.Proxy = proxy;

                    client.Proxy = WebRequest.DefaultWebProxy;
                    client.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    client.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);

                    var content = client.DownloadString(URI);
                    client.Proxy = null;
                }

                WebRequest.DefaultWebProxy = null;
                HttpContext.Current.Session["OTPId"] = Convert.ToString(ds.Tables[0].Rows[0]["Id"]);
                HttpContext.Current.Session["OTPNumber"] = Convert.ToString(ds.Tables[0].Rows[0]["OTP"]);
                return OTPNumber.ToString();
            }

            catch (Exception ex)
            {
                //  File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in GenerateOTP for proposal number:" + lblProposalText.Text + " and error message is : " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
                return null;
            }

        }
        protected void OnServerValidatecvtxtOtpNumber(object sender, ServerValidateEventArgs e)
        {
            if (txtOtpNumber.Text == HttpContext.Current.Session["OTPNumber"].ToString())
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;

                //int Num = int.Parse(hdnOTPSentCount.Value) + 1;
                //hdnOTPSentCount.Value = Num.ToString();

                //agreewithbtn.Visible = false;
                otpPanel.Visible = true;
                txtOtpNumber.Text = "";
                btnMakePayment.Enabled = false;
                btnMobileReSend.Enabled = false;
                btnMobileVerify.Focus();
                cvtxtOtpNumber.ErrorMessage = "Please provide valid otp number.";
                ScriptManager.RegisterStartupScript(UpdatePanel_Detail1, UpdatePanel_Detail1.GetType(), "testpage", "runme();", true);
            }
        }
        private DataSet GetCustomerData(string customerId)
        {
            DataSet ds = new DataSet();
            string[] vErrorMsg = new string[2];
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    SqlCommand command = new SqlCommand("PROC_GET_IRDA_GIST_SANDBOX_CUSTOMER_DETAILS", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@vCustomerId", customerId);
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    sda.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;

                    }
                    //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "KGI_ADService : Return string for otp " + retString + " for mobile " + vMobileNo + " " + DateTime.Now + Environment.NewLine);

                }
                return ds;
            }
            catch (Exception ex)
            {

                // File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "KGI_ADService : Error occured in VerifyOTP " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                vErrorMsg[0] = "Fail";
                vErrorMsg[1] = ex.Message;
                return null;
                // return vErrorMsg;
            }




        }

        private DataSet InsertOTPData(string identity, string mobileno, string custID, string custName, string propNumber, string otpNumber, string v4, string v5, string v6, string v7, string productcode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            using (SqlConnection con = new SqlConnection(db.ConnectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("INSERT_OTP_DATA", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@identity", identity);
                command.Parameters.AddWithValue("@mobileno", mobileno);
                command.Parameters.AddWithValue("@custID", custID);
                command.Parameters.AddWithValue("@custName", custName);
                command.Parameters.AddWithValue("@propNumber", propNumber);
                command.Parameters.AddWithValue("@otpNumber", otpNumber);
                command.Parameters.AddWithValue("@productcode", productcode);
                DataSet myDataSet = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(myDataSet);
                return myDataSet;
            }


        }

    }
}