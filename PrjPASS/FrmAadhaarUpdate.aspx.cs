using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Globalization;



using System.Net;



namespace PrjPASS
{
    public partial class FrmAadhaarUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("FrmSecuredLogin.aspx"); //aadhar updation is not required: sanjay sir: 06-Feb-2020
        }

        protected void btnSendOTP_Click(object sender, EventArgs e)
        {
            string s = ListBox1.Items.Count.ToString();
        }

        protected void btnAddPolicy_Click(object sender, EventArgs e)
        {
            if (ListBox1.Items.Count < 10)
            {
                if (txtPolicyNumber.Text.Trim().Length >= 8)
                {
                    if (HasConsecutiveChars(txtPolicyNumber.Text.Trim(), 7))
                    {
                        Alert.Show("Please enter valid Policy Number");
                    }
                    else
                    {
                        ListItem item = ListBox1.Items.FindByText(txtPolicyNumber.Text.Trim());
                        if (item == null)
                        {
                            ListBox1.Items.Add(txtPolicyNumber.Text.Trim());
                            ListBox1.Visible = true;
                            btnRemoveSelected.Visible = true;
                            lblLabelPolicyNumber.Visible = true;
                            hdnIsAtleastOnePolicyAddedInListBox.Value = "1";
                        }
                    }
                }
                else
                {
                    Alert.Show("Policy Number should be greater than 7 characters");
                }
                txtPolicyNumber.Text = "";
            }
        }

        protected void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            if (ListBox1.Items.Count > 0)
            {
                ListBox1.Items.Remove(ListBox1.SelectedItem);
                if (ListBox1.Items.Count == 0)
                {
                    ListBox1.Visible = false;
                    btnRemoveSelected.Visible = false;
                    lblLabelPolicyNumber.Visible = false;
                    hdnIsAtleastOnePolicyAddedInListBox.Value = "0";
                }
            }
        }

        protected void btnOTPSend_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (Validation(out ErrorMessage))
            {
                string msg = GenerateOTPNew(txtMobileNumber.Text.Trim(), txtEmailId.Text.Trim());
                if (msg == "success")
                {
                    btnAddPolicy.Visible = false;
                    btnRemoveSelected.Visible = false;

                    otpPanel.Visible = true;
                    btnMobileVerify.Focus();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "testpage", "runme();", true);
                    UpdatePanel1.Update();
                }
                else
                {
                    Alert.Show("could not generate OTP, kindly try after sometime");
                }
            }
            else
            {
                Alert.Show(ErrorMessage);
            }
        }

        public string GenerateOTPNew(string MobileNumber, string EmailId)
        {
            string GeneratedOTP = string.Empty;
            try
            {
                Random r = new Random();
                GeneratedOTP = r.Next(100000, 999999).ToString();
                HttpContext.Current.Session["OTPNumber"] = GeneratedOTP;

                if (MobileNumber.Length == 10)
                {
                    bool IsSendSMSSuccess = SendSMS(GeneratedOTP, MobileNumber);
                }

                SendOTPEmail(EmailId, GeneratedOTP);

            }
            catch (Exception ex)
            {
                GeneratedOTP = "";
                ExceptionUtility.LogException(ex, "Error in GenerateOTP on FrmSTP Page");
            }
            return string.IsNullOrEmpty(GeneratedOTP) ? "" : "success";
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
                otpPanel.Visible = true;
                txtOtpNumber.Text = "";
                btnOTPSend.Enabled = false;
                btnMobileReSend.Enabled = false;
                btnMobileVerify.Focus();
                cvtxtOtpNumber.ErrorMessage = "Please provide valid otp number.";
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "testpage", "runme();", true);
            }
        }

        protected void onClickbtnMobileReSend(object sender, EventArgs e)
        {
            btnOTPSend_Click(sender, e);
        }

        protected void onClickbtnMobileVerify(object sender, EventArgs e)
        {
            bool isSuccess = false;
            if (txtOtpNumber.Text == HttpContext.Current.Session["OTPNumber"].ToString())
            {
                isSuccess = Save_AADHAAR_PAN();

                if (isSuccess)
                {
                    sectionMain.Visible = false;
                    sectionThankYou.Visible = true;
                    SendAcknowledgementEmail(txtEmailId.Text.Trim(), txtFullName.Text.Trim(), lblSRNumber.Text);
                    SendConfirmationSMS();
                }
                else
                {
                    sectionMain.Visible = false;
                    sectionThankYou.Visible = false;
                    sectionError.Visible = true;
                }
            }
        }

        private bool Save_AADHAAR_PAN()
        {
            bool isSuccess = false;
            string SRNumber = string.Empty;
            try
            {
                var SB = new StringBuilder();
                foreach (ListItem lst in ListBox1.Items)
                {
                    SB.Append("" + lst.Value + ",");
                }

                var strPolicyNum = SB.ToString().Substring(0, (SB.Length - 1));

                //string filePathAADHAAR = fileUploadAadhaar.PostedFile != null ? fileUploadAadhaar.PostedFile.FileName : "";
                //string filePathPAN = fileUploadPan.PostedFile != null ? fileUploadPan.PostedFile.FileName : "";


                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_AADHAAR_PAN_NUMBER";

                        cmd.Parameters.AddWithValue("@AadhaarNumber", Encryption.EncryptText(txtAadhaarNumber.Text.Trim()));
                        cmd.Parameters.AddWithValue("@PANNumber", Encryption.EncryptText(txtPanNumber.Text.Trim()));

                        //cmd.Parameters.AddWithValue("@AADHAAR_File", string.IsNullOrEmpty(filePathAADHAAR) ? null : GetBinaryFile(filePathAADHAAR));
                        //cmd.Parameters.AddWithValue("@PAN_File", string.IsNullOrEmpty(filePathPAN) ? null : GetBinaryFile(filePathPAN));

                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@DateOfBirth", Encryption.EncryptText(txtDOB.Text.Trim()));
                        cmd.Parameters.AddWithValue("@Gender", Encryption.EncryptText(radioMale.Checked ? "Male" : "Female"));
                        cmd.Parameters.AddWithValue("@EmailId", Encryption.EncryptText(txtEmailId.Text.Trim()));
                        cmd.Parameters.AddWithValue("@MobileNumber", Encryption.EncryptText(txtMobileNumber.Text.Trim()));
                        cmd.Parameters.AddWithValue("@PolicyNumber", strPolicyNum);
                        cmd.Parameters.AddWithValue("@OTP", txtOtpNumber.Text.Trim());

                        cmd.Connection = conn;
                        conn.Open();
                        SRNumber = cmd.ExecuteScalar().ToString();
                        if (SRNumber.Contains("SR"))
                        {
                            isSuccess = true;
                            lblSRNumber.Text = SRNumber;
                            lblCurrentDatetime.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:ss tt");
                        }
                        else
                        {
                            isSuccess = false;
                            lblSRNumber.Text = "";
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Save_AADHAAR_PAN");
            }
            return isSuccess;
        }

        private byte[] GetBinaryFile(string filePath)
        {
            byte[] bytes = null;
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetBinaryFile , FrmAadhaarUpdate : filePath: " + filePath);
            }
            return bytes;
        }

        private string SendOTPEmail(string ToEmailId, string OTPNumber)
        {
            string strMessage = string.Empty;

            string smtp_Username = ConfigurationManager.AppSettings["smtp_Username"].ToString();
            string smtp_Password = ConfigurationManager.AppSettings["smtp_Password"].ToString();
            string smtp_Host = ConfigurationManager.AppSettings["smtp_Host"].ToString();
            string smtp_FromMailId = "noreply@kotak.com";
            string strPath = string.Empty;
            string MailBody = string.Empty;

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


                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBody_AADHAAR_OTP.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#Name#", txtFullName.Text.Trim());
                MailBody = MailBody.Replace("#ReplaceText#", "One Time Password For Updating AADHAAR and PAN Number is " + OTPNumber);


                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(smtp_FromMailId);
                mm.Subject = "KGI: OTP For Updating AADHAAR and PAN Number";
                mm.Body = MailBody;
                mm.IsBodyHtml = true;

                mm.To.Add(ToEmailId);

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                strMessage = "success";
            }
            catch (Exception ex)
            {
                strMessage = "error while sending OTP on mail";
                ExceptionUtility.LogException(ex, "SendEmail Method");
            }

            return strMessage;
        }

        private static bool SendSMS(string GeneratedOTP, string MobileNumber)
        {
            bool IsSendSMSSuccess = false;

            try
            {
                string strPath = string.Empty;
                string smsBody = string.Empty;

                smsBody = ConfigurationManager.AppSettings["smsBody"];
                smsBody = smsBody.Replace("@otpNumber", Convert.ToString(GeneratedOTP));

                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "0")
                {
                    // string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
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

                    // string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                    var client = new System.Net.WebClient();
                    client.Proxy = proxy;

                    client.Proxy = WebRequest.DefaultWebProxy;
                    client.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    client.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);

                    var content = client.DownloadString(URI);
                    IsSendSMSSuccess = true;
                }
            }
            catch (Exception ex)
            {
                IsSendSMSSuccess = false;
                ExceptionUtility.LogException(ex, "Error in SendSMS on FrmSTP Page");
            }
            return IsSendSMSSuccess;
        }




        private bool Validation(out string ErrorMessage)
        {
            ErrorMessage = string.Empty;

            if (txtFullName.Text.Trim().Length <= 0)
            {
                ErrorMessage = "Please enter full name !";
            }
            else if (txtDOB.Text.Trim().Length <= 0)
            {
                ErrorMessage = "Please enter date of birth !";
            }
            else if (!IsValidDateFormat(txtDOB.Text.Trim()))
            {
                ErrorMessage = "Please enter valid date of birth in format: dd/mm/yyyy !";
            }
            else if (txtEmailId.Text.Trim().Length <= 0)
            {
                ErrorMessage = "Please enter email id !";
            }
            else if (!IsValidEmailAddress(txtEmailId.Text.Trim()))
            {
                ErrorMessage = "Please enter valid email id !";
            }
            else if (txtMobileNumber.Text.Trim().Length != 10)
            {
                ErrorMessage = "Please enter valid 10 digit numeric mobile number!";
            }
            else if (IsValidNumber(txtMobileNumber.Text.Trim()) == false)
            {
                ErrorMessage = "Please enter valid 10 digit numeric mobile number!";
            }
            else if (txtAadhaarNumber.Text.Trim().Length != 12)
            {
                ErrorMessage = "Please enter valid 12 digit numeric aadhaar number !";
            }
            else if (IsValidNumber(txtAadhaarNumber.Text.Trim()) == false)
            {
                ErrorMessage = "Please enter valid 12 digit numeric aadhaar number !";
            }
            else if (HasConsecutiveChars(txtAadhaarNumber.Text.Trim(), txtAadhaarNumber.MaxLength))
            {
                ErrorMessage = "Please enter valid 12 digit numeric aadhaar number !";
            }
            else if (txtPanNumber.Text.Trim().Length != 10)
            {
                ErrorMessage = "Please enter valid 10 digit alphanumeric pan number !";
            }
            else if (HasConsecutiveChars(txtPanNumber.Text.Trim(), txtPanNumber.MaxLength))
            {
                ErrorMessage = "Please enter valid 10 digit alphanumeric pan number !";
            }
            else if (hdnIsAtleastOnePolicyAddedInListBox.Value == "0")
            {
                ErrorMessage = "Please enter policy number !";
            }
            else if (chkIAgree.Checked == false)
            {
                ErrorMessage = "Please provide your consent by ticking the box above !";
            }
            return ErrorMessage.Length > 0 ? false : true;
        }

        public bool IsValidDateFormat(string date)
        {
            DateTime d;
            return DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        }

        public bool IsValidEmailAddress(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool IsValidNumber(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        private static string SendAcknowledgementEmail(string ToEmailId, string CustomerName, string SRNumber)
        {
            string smtp_DefaultCCMailId = ConfigurationManager.AppSettings["smtp_DefaultCCMailId"].ToString();
            string strMessage = string.Empty;

            string smtp_Username = ConfigurationManager.AppSettings["smtp_Username"].ToString();
            string smtp_Password = ConfigurationManager.AppSettings["smtp_Password"].ToString();
            string smtp_Host = ConfigurationManager.AppSettings["smtp_Host"].ToString();
            string smtp_FromMailId = "noreply@kotak.com";
            string strPath = string.Empty;
            string MailBody = string.Empty;

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


                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBody_AADHAAR_OTP.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#Name#", CustomerName);
                MailBody = MailBody.Replace("#ReplaceText#", " We have received your request for linking AADHAAR and PAN with your policies via Request SR Number: #SRNUMBER# along with your consent<br><br><p>i.	To validate/authenticate your Aadhar number with UIDAI<br>ii.	To collect, store, share and use the details provided above in accordance with the applicable norms</p>");
                MailBody = MailBody.Replace("#SRNUMBER#", SRNumber);

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(smtp_FromMailId);
                mm.Subject = "Kotak General Insurance: AADHAAR and PAN Update Confirmation: #" + SRNumber;
                mm.Body = MailBody;
                mm.IsBodyHtml = true;

                mm.To.Add(ToEmailId);

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                strMessage = "success";
            }
            catch (Exception ex)
            {
                strMessage = "error while sending Acknowledgement Email";
                ExceptionUtility.LogException(ex, "SendAcknowledgementEmail Method, FrmAadhaarUpdate Page");
            }

            return strMessage;
        }

        private void SendConfirmationSMS()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnConnect"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SAVE_TO_TRANS_SMS_LOG_AADHAAR_PAN_SR_NUMBER";

                        cmd.Parameters.AddWithValue("@MobileNo", txtMobileNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@SRNumber", lblSRNumber.Text.Trim());

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SendConfirmationSMS Method");
            }
        }

        private bool HasConsecutiveChars(string str)
        {
            bool IsHasConsecutiveChars = false;
            try
            {


                string re = @"(?x)
            ^
            # fail if...
            (?!
                # repeating numbers
                (\d) \1+ $
                |
                # sequential ascending
                (?:0(?=1)|1(?=2)|2(?=3)|3(?=4)|4(?=5)|5(?=6)|6(?=7)|7(?=8)|8(?=9)|9(?=0)){5} \d $
                |
                # sequential descending
                (?:0(?=9)|1(?=0)|2(?=1)|3(?=2)|4(?=3)|5(?=4)|6(?=5)|7(?=6)|8(?=7)|9(?=8)){5} \d $
            )
            # match any other combinations of 6 digits
            \d{6}
            $
        ";

                string[] numbers = { "102", "111111", "123456", "654321", "123455", "321123", "111112" };

                if (Regex.IsMatch(str, re))
                {
                    IsHasConsecutiveChars = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "HasConsecutiveChars");
            }
            return IsHasConsecutiveChars;
        }

        public static bool HasConsecutiveChars(string source, int sequenceLength)
        {
            return Regex.IsMatch(source, "([a-zA-Z1-9])\\1{" + (sequenceLength - 1) + "}");
        }
    }
}