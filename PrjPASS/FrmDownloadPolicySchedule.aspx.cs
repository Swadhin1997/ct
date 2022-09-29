using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Microsoft.Web.Services3.Security.Tokens;
using System.Security.Authentication;

namespace PrjPASS
{
    public partial class FrmDownloadPolicySchedule : System.Web.UI.Page
    {
        public int interactionID = 0;
        string userName = ConfigurationManager.AppSettings["userName"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();
        string TalismaSessionKey = ConfigurationManager.AppSettings["TalismaSessionKey"].ToString();
        int InteractioniServiceArrayIndex = Convert.ToInt32(ConfigurationManager.AppSettings["InteractioniServiceArrayIndex"].ToString());
        int ContactiServiceArrayIndex = Convert.ToInt32(ConfigurationManager.AppSettings["ContactiServiceArrayIndex"]);
        string ContactiserviceURL = ConfigurationManager.AppSettings["ContactiserviceURL"];
        string InteractioniserviceURL = ConfigurationManager.AppSettings["InteractioniserviceURL"];
        string FrmDownloadPolicyScheduleLog = AppDomain.CurrentDomain.BaseDirectory + "//FrmDownloadPolicySchedule//log.txt";
        string FrmDownloadPolicyScheduleLogDirectory = AppDomain.CurrentDomain.BaseDirectory + "//FrmDownloadPolicySchedule";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(FrmDownloadPolicyScheduleLogDirectory))
            {
                Directory.CreateDirectory(FrmDownloadPolicyScheduleLogDirectory);
            }

            if (!File.Exists(FrmDownloadPolicyScheduleLog))
            {
                File.Create(FrmDownloadPolicyScheduleLog);
            }
        }

        protected void btnOTPSend_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            string msg = "";

            if (!fnCheckPolicyMobEmailRecord(txtPolicyNumber.Text, txtEmailID.Text, txtMobileNumber.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openInvalidPolicyErrorModal();", true);
                UpdatePanel1.Update();
            }
            else
            {
                if (Validation(out ErrorMessage))
                {
                    if (txtMobileNumber.Text.Length == 10)
                    {
                        string mobileNumber = txtMobileNumber.Text.Trim();
                        msg = GenerateOTPNew(mobileNumber);
                    }
                    else if (!string.IsNullOrEmpty(txtEmailID.Text))
                    {
                        string EmailID = txtEmailID.Text.Trim();
                        msg = GenerateOTPNewForEmail(EmailID);
                    }

                    if (msg == "success")
                    {
                        txtPolicyNumber.Enabled = false;
                        txtMobileNumber.Enabled = false;
                        txtEmailID.Enabled = false;
                        btnOTPSend.Visible = false;
                        otpPanel.Visible = true;
                        btnMobileVerify.Focus();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "testpage", "runme();", true);
                        UpdatePanel1.Update();
                    }
                    else
                    {
                        txtPolicyNumber.Enabled = true;
                        File.AppendAllText(FrmDownloadPolicyScheduleLog, "could not generate OTP for mobile number" + txtMobileNumber.Text + " Email ID " + txtEmailID.Text + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                        Alert.Show("could not generate OTP, kindly try after sometime");

                    }
                }
                else
                {
                    Alert.Show(ErrorMessage);
                }

            }

        }

        private bool fnCheckPolicyMobEmailRecord(string PolicyNumber, string Email, string MobileNumber)
        {
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Searching for Policy record   Policy number  " + PolicyNumber + "  Email " + Email + "  Mobile " + MobileNumber + "   " + DateTime.Now.ToString() + System.Environment.NewLine);
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_POLICY_NUMBER_EMAIL_MOBILE", con))
                    {
                        DataTable dt = new DataTable();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PolicyNumber", PolicyNumber);
                        cmd.Parameters.AddWithValue("@EmailID", Email);
                        cmd.Parameters.AddWithValue("@MobileNumber", MobileNumber);
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            res= true;
                            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Record found for Policy number  " + PolicyNumber + "  Email " + Email + "  Mobile " + MobileNumber + "   " + DateTime.Now.ToString() + System.Environment.NewLine);
                        }
                        else
                        {
                            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Record not found for Policy number  " + PolicyNumber + "  Email " + Email + "  Mobile " + MobileNumber + "   " + DateTime.Now.ToString() + System.Environment.NewLine);
                            res = false;
                        }
                        return res;
                    }
                }

            }
            catch (Exception ex)
            {
                res = false;
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "Error Occured " + ex.ToString() + "  inner exception  " + ex.InnerException + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            }
            return res;
        }

        private string GenerateOTPNewForEmail(string emailID)
        {
            string GeneratedOTP = string.Empty;
            try
            {
                Random r = new Random();
                GeneratedOTP = r.Next(100000, 999999).ToString();
                HttpContext.Current.Session["OTPNumber"] = GeneratedOTP;

                bool IsSendEmailSuccess = SendOTPEmail(GeneratedOTP, emailID);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "OTP generated for  Email ID " + emailID + "  and otp " + GeneratedOTP +  "   " + DateTime.Now.ToString() + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                GeneratedOTP = "";
                ExceptionUtility.LogException(ex, "Error in GenerateOTP on FrmSTP Page");
            }
            return string.IsNullOrEmpty(GeneratedOTP) ? "" : "success";
        }

        private bool SendOTPEmail(string generatedOTP, string emailID)
        {
            bool Success = true;
            try
            {
                string emailId = txtEmailID.Text.Trim();
                string strPath = string.Empty;
                string MailBody = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBody_SchedulePolicy_OTP.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#Name", lblName.Text.Trim());
                MailBody = MailBody.Replace("#OTP", generatedOTP.ToString());
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                mm.Subject = string.Format("OTP for Policy Schedule - {0}", txtPolicyNumber.Text.Trim());
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;

                smtpClient.Send(mm);
                Success = true;
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "OTP " + generatedOTP + "sent to Email ID" + emailID + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SendOTPEmail");
                sectionMain.Visible = false;
                sectionError.Visible = true;
                Success = false;
            }
            return Success;
        }

        private string GenerateOTPNew(string mobileNumber)
        {
            string GeneratedOTP = string.Empty;
            try
            {
                Random r = new Random();
                GeneratedOTP = r.Next(100000, 999999).ToString();
                HttpContext.Current.Session["OTPNumber"] = GeneratedOTP;

                if (mobileNumber.Length == 10)
                {
                    bool IsSendSMSSuccess = SendSMS(GeneratedOTP, mobileNumber);
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, "OTP " + GeneratedOTP + "sent to Mobile number " + mobileNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                GeneratedOTP = "";
                ExceptionUtility.LogException(ex, "Error in GenerateOTP on FrmSTP Page");
            }
            return string.IsNullOrEmpty(GeneratedOTP) ? "" : "success";
        }

        private bool SendSMS(string GeneratedOTP, string MobileNumber)
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

                    //string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);

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
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, "OTP " + GeneratedOTP + "sent to mobile Number" + MobileNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                IsSendSMSSuccess = false;
                ExceptionUtility.LogException(ex, "Error in SendSMS on FrmDownloadPolicy Page");
                sectionMain.Visible = false;
                sectionError.Visible = true;
            }
            return IsSendSMSSuccess;
        }

        private bool Validation(out string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            if (string.IsNullOrEmpty(txtPolicyNumber.Text.ToString()))
            {
                ErrorMessage = "Please enter policy number!";
            }


            if (!string.IsNullOrEmpty(txtMobileNumber.Text.ToString()))
            {
                if (!Regex.IsMatch(txtMobileNumber.Text.Trim(), "^[0-9]*$") || txtMobileNumber.Text.Length != 10)
                {
                    ErrorMessage = "Please enter valid 10 digit mobile number!";
                }

            }

            if (!string.IsNullOrEmpty(txtEmailID.Text.Trim()))
            {
                if (!Regex.IsMatch(txtEmailID.Text.Trim(), @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                                           @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                {
                    ErrorMessage = "Please enter valid E-Mail Address !";
                }

            }

            return ErrorMessage.Length > 0 ? false : true;
        }

        protected void onClickbtnMobileVerify(object sender, EventArgs e)
        {
            try
            {
                bool isSuccess = false;
                if (!IsValidPolicyNumber(txtPolicyNumber.Text.Trim()))
                {
                    otpPanel.Visible = false;
                    txtPolicyNumber.Enabled = true;
                    txtMobileNumber.Enabled = true;
                    txtEmailID.Enabled = true;
                    btnOTPSend.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openInvalidPolicyErrorModal();", true);
                    UpdatePanel1.Update();
                }

                else if (txtOtpNumber.Text == HttpContext.Current.Session["OTPNumber"].ToString() && ValidateMobileEmailWithPolicy(txtMobileNumber.Text.Trim()))
                {
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, "Verifying OTP " + txtOtpNumber.Text + "with mobile Number" + txtMobileNumber.Text + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    otpPanel.Visible = false;
                    dvPolicyDetails.Visible = true;
                }
                else
                {
                    otpPanel.Visible = false;
                    txtPolicyNumber.Enabled = true;
                    txtMobileNumber.Enabled = true;
                    txtEmailID.Enabled = true;
                    btnOTPSend.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openErrorModal();", true);
                    UpdatePanel1.Update();
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogException(ex, "onClickbtnMobileVerify");
            }

        }

        private bool IsValidPolicyNumber(string v)
        {
            bool valid = true;
            try
            {
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "Validating policy number " + v.ToString() + DateTime.Now.ToString() + System.Environment.NewLine);

                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("PROC_GET_POLICY_NUMBER_AVAILABILITY_FOR_SCHEDULE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyNumber", txtPolicyNumber.Text.Trim());
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                    }
                    return valid;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "ValidateMobileWithPolicy");
                sectionMain.Visible = false;
                sectionError.Visible = true;

            }
            return valid;
        }

        private bool ValidateMobileEmailWithPolicy(string MobileNumber)
        {
            bool valid = true;
            try
            {
                DataTable dtValidateMobile = new DataTable();

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("PROC_GET_POLICY_DETAILS_FOR_SCHEDULE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyNumber", txtPolicyNumber.Text.Trim());
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtValidateMobile);
                    if (dtValidateMobile.Rows.Count > 0)
                    {
                        if (dtValidateMobile.Rows[0]["TXT_MOBILE"].ToString() == MobileNumber || dtValidateMobile.Rows[0]["TXT_EMAIL"].ToString().ToLower() == txtEmailID.Text.Trim().ToLower())
                        {
                            lblName.Text = dtValidateMobile.Rows[0]["TXT_SALUTATION"].ToString() + " " + dtValidateMobile.Rows[0]["TXT_CUSTOMER_NAME"].ToString();
                            hdnProductCode.Value = dtValidateMobile.Rows[0]["NUM_PRODUCT_CODE"].ToString();
                            hdnDeptCode.Value = dtValidateMobile.Rows[0]["NUM_DEPARTMENT_CODE"].ToString();
                            if (!string.IsNullOrEmpty(dtValidateMobile.Rows[0]["TXT_EMAIL"].ToString()))
                            {
                                txtEmailforPolicy.Text = dtValidateMobile.Rows[0]["TXT_EMAIL"].ToString();
                            }

                            valid = true;
                        }
                        else
                        {
                            valid = false;
                        }

                    }
                    else
                    {
                        valid = true;
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "ValidateMobileWithPolicy");
                sectionMain.Visible = false;
                sectionError.Visible = true;

            }
            return valid;
        }


        protected void onClickbtnMobileReSend(object sender, EventArgs e)
        {
            btnOTPSend_Click(sender, e);
        }


        protected void SendSchedulePolicyEmail(object sender, EventArgs e)
        {
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Started to send mail  " + DateTime.Now.ToString() + System.Environment.NewLine);
            bool IsSendOnMail = true;
            bool IsDownLoad = false;
            if (!Regex.IsMatch(txtEmailforPolicy.Text.Trim(), @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                                           @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                Alert.Show("Please enter valid Email Address !");
                return;
            }

            string InteractionID = CreateInteraction(txtPolicyNumber.Text.Trim(), txtMobileNumber.Text.Trim(), txtEmailforPolicy.Text.Trim(), IsDownLoad).ToString();
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Interaction Created " + interactionID + " " + DateTime.Now.ToString() + System.Environment.NewLine);
            AddSearchLog(txtPolicyNumber.Text.Trim(), txtMobileNumber.Text.Trim(), txtEmailforPolicy.Text.Trim(), IsSendOnMail, IsDownLoad, InteractionID);
            try
            {
                string emailId = txtEmailforPolicy.Text.Trim();
                string strPath = string.Empty;
                string MailBody = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "Schedule_Policy_Email_Body.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#FullCustomerName", lblName.Text.Trim());
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                mm.Subject = string.Format(ConfigurationManager.AppSettings["Schedule_Policy_email_Subject"], txtPolicyNumber.Text.Trim());
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                string attachmentFilename = GetPolicySchedulePath(txtPolicyNumber.Text.Trim());
                //Alert.Show("File Name " + attachmentFilename.ToString());
                if (attachmentFilename != null)
                {
                    Attachment attachment = new Attachment(Path.GetFullPath(attachmentFilename), MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mm.Attachments.Add(attachment);
                }

                smtpClient.Send(mm);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openPolicyEmailSentModel();", true);
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Error in SendSchedulePolicyEmail  error message " +ex.ToString()  + " error stackstrace  "+ex.StackTrace+"  " + DateTime.Now.ToString() + System.Environment.NewLine);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openPolicyEmailNotSentModel();", true);
                UpdatePanel1.Update();
                sectionMain.Visible = false;
                sectionError.Visible = true;
            }
        }

        private void AddSearchLog(string PolicyNumber, string MobileNumber, string EmailAddress, bool isSendOnMail, bool isDownLoad, string InteractionID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_ADD_TBL_SCHEDULE_POLICY_LOG", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vPolicyNumber", PolicyNumber);
                        cmd.Parameters.AddWithValue("@vMobileNumber", MobileNumber);
                        cmd.Parameters.AddWithValue("@vEmailAddress", EmailAddress);
                        cmd.Parameters.AddWithValue("@vInteractionID", interactionID);
                        cmd.Parameters.AddWithValue("@vOTP", HttpContext.Current.Session["OTPNumber"].ToString());
                        cmd.Parameters.AddWithValue("@bIsSendOnMail", isSendOnMail);
                        cmd.Parameters.AddWithValue("@bIsDownLoad", isDownLoad);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Error in AddSearchlog  error message " + ex.ToString() + " error stackstrace  " + ex.StackTrace + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                sectionMain.Visible = false;
                sectionError.Visible = true;
            }
        }

        private int CreateInteraction(string PolicyNumber, string MobileNumber, string EmailAddress, bool isDownload)
        {

            WebRequest.DefaultWebProxy = null;

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.XmlElement talismaSession = xmlDoc.CreateElement(TalismaSessionKey);
            UsernameToken objUserNameToken = new UsernameToken(userName, password, PasswordOption.SendPlainText);

            objUserNameToken.AnyElements.Add(talismaSession);

            ContactWS.ContactWebService objContactWebService = null;
            ContactWS.PropertyInfo[] objPropertyInfo = null;
            objContactWebService = new ContactWS.ContactWebService();
            objContactWebService.RequestSoapContext.Security.Tokens.Add(objUserNameToken);
            objPropertyInfo = new ContactWS.PropertyInfo[ContactiServiceArrayIndex];
            objContactWebService.Url = ContactiserviceURL;

            InteractionWS.InteractionWebService objInteractionWebService = null;
            InteractionWS.PropertyInfo[] objInteractionPropertyInfo = null;
            objInteractionWebService = new InteractionWS.InteractionWebService();
            objInteractionWebService.RequestSoapContext.Security.Tokens.Add(objUserNameToken);
            objInteractionPropertyInfo = new InteractionWS.PropertyInfo[InteractioniServiceArrayIndex];
            objInteractionWebService.Url = InteractioniserviceURL;

            InteractionWS.InteractionAttachmentData[] attach = new InteractionWS.InteractionAttachmentData[0];

            try
            {
                long returnValue1 = 0;
                long nContactID = 0;
                string phone = MobileNumber;
                string Subject = string.Format("Policy schedule download {0}", PolicyNumber);
                string userMsg = "";
                string contactMsg = "";
                long interactionId = 0;
                long eventId = 0;
                string error = string.Empty;
                bool contactPreviouslyUnblocked;
                int receivedByUSerID = 2;
                int MediaId = 8;
                int Direction = 1;
                int teamId = 4;
                int AssignedtoUserID = 1; // 2;
                int AliasID = 1;
                int Priority = 1;
                int Resolved = 1;
                bool UpdateReadOnly = true;
                bool MandatoryCheck = true;

                objPropertyInfo[0] = new ContactWS.PropertyInfo();
                objPropertyInfo[0].propertyID = 57;
                objPropertyInfo[0].propValue = EmailAddress;
                objPropertyInfo[0].rowID = -1;
                objPropertyInfo[0].relJoinID = -1;


                string szError;
                string USERNAME = lblName.Text.Trim();
                returnValue1 = objContactWebService.ResolveContact(false, objPropertyInfo, out nContactID, out szError);


                if (nContactID == -1)
                {

                    returnValue1 = objContactWebService.CreateContact(USERNAME, objPropertyInfo, true, true, out nContactID, out szError);

                }


                string CaseTypePropText = "", CaseTypePropValue = "", LOB = "", LOB21711 = "";

                if (hdnDeptCode.Value == "31")
                {
                    CaseTypePropText = "Motor";
                    CaseTypePropValue = "9"; // Case type Property Value
                    LOB = "21711";           // LOB Property ID
                    LOB21711 = "3";          // LOB Case Type motor
                }
                else if (hdnDeptCode.Value == "28")
                {
                    CaseTypePropText = "Health";
                    CaseTypePropValue = "5";  // Case Type Property Value
                    LOB = "21711";            // LOB Property ID
                    LOB21711 = "2";           // LOB Case Type health
                }
                else
                {
                    CaseTypePropText = "Other";
                    CaseTypePropValue = "12";   // Case Type Property Value
                    LOB = "21711";             // LOB Property ID
                    LOB21711 = "1";           // LOB Case Type other
                }


                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Creating Interaction  " + DateTime.Now.ToString() + System.Environment.NewLine);

                //  Setting LOB and LOB Case Type
                objInteractionPropertyInfo[0] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[0].propertyID = 21711;
                objInteractionPropertyInfo[0].propValue = LOB21711;
                objInteractionPropertyInfo[0].rowID = -1;
                objInteractionPropertyInfo[0].relJoinID = -1;
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 0 LOB propertyID  21711 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 0 LOB Prop Value  "+ LOB21711.ToString() + DateTime.Now.ToString() + System.Environment.NewLine);
                //


                // 
                objInteractionPropertyInfo[1] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[1].propertyID = 21481;                 // 21481 = Casetype
                objInteractionPropertyInfo[1].propValue = CaseTypePropValue;
                objInteractionPropertyInfo[1].rowID = -1;
                objInteractionPropertyInfo[1].relJoinID = -1;
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 1 CaseType propertyID  21481 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 1 CaseType Prop Value  " + CaseTypePropValue.ToString() + DateTime.Now.ToString() + System.Environment.NewLine);

                //


                //
                objInteractionPropertyInfo[2] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[2].propertyID = 21483;                //Calltype
                objInteractionPropertyInfo[2].propValue = "73";                 //73 for  "Website Query";
                objInteractionPropertyInfo[2].rowID = -1;
                objInteractionPropertyInfo[2].relJoinID = -1;

                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 2 CallType propertyID  21483 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 2 CallType Prop Value 73 " + DateTime.Now.ToString() + System.Environment.NewLine);
                //

                //
                objInteractionPropertyInfo[3] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[3].propertyID = 21484; //SubCalltype Property ID
                if (isDownload)
                {
                    objInteractionPropertyInfo[3].propValue = "288"; // policy download
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 SubCallType propertyID  21484 " + DateTime.Now.ToString() + System.Environment.NewLine);
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 subCallType Prop Value  288 " + DateTime.Now.ToString() + System.Environment.NewLine);
                }
                else
                {
                    objInteractionPropertyInfo[3].propValue = "289"; // policy email
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 SubCallType propertyID  21484 " + DateTime.Now.ToString() + System.Environment.NewLine);
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 subCallType Prop Value  289 " + DateTime.Now.ToString() + System.Environment.NewLine);
                }

                objInteractionPropertyInfo[3].rowID = -1;
                objInteractionPropertyInfo[3].relJoinID = -1;
                
                //

                //
                objInteractionPropertyInfo[4] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[4].propertyID = 21619; //
                objInteractionPropertyInfo[4].propValue = userMsg; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                objInteractionPropertyInfo[4].rowID = -1;
                objInteractionPropertyInfo[4].relJoinID = -1;

                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 4 propertyID  21619 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 4 Prop Value  "+userMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                //


                //
                objInteractionPropertyInfo[5] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[5].propertyID = 21617; //
                objInteractionPropertyInfo[5].propValue = "1"; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                objInteractionPropertyInfo[5].rowID = -1;
                objInteractionPropertyInfo[5].relJoinID = -1;
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 5 propertyID  21617 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 5 Prop Value  1 " + userMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                //

                //
                objInteractionPropertyInfo[6] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[6].propertyID = 22235; //
                objInteractionPropertyInfo[6].propValue = "86944";  // "Policy sent successfully"; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                objInteractionPropertyInfo[6].rowID = -1;
                objInteractionPropertyInfo[6].relJoinID = -1;
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 6 propertyID  22235 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 6 Prop Value  86944 " + userMsg + DateTime.Now.ToString() + System.Environment.NewLine);

                //

                //
                objInteractionPropertyInfo[7] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[7].propertyID = 23369; //
                objInteractionPropertyInfo[7].propValue = LOB; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                objInteractionPropertyInfo[7].rowID = -1;
                objInteractionPropertyInfo[7].relJoinID = -1;

                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 7 propertyID  23369 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 7 Prop Value  " + LOB + DateTime.Now.ToString() + System.Environment.NewLine);

                //

                returnValue1 = objInteractionWebService.CreateInteraction(nContactID, phone, DateTime.Now, receivedByUSerID, MediaId,
                                                          Direction, Subject, teamId, AssignedtoUserID, AliasID, Priority, Resolved, contactMsg, userMsg, attach,
                                                          objInteractionPropertyInfo, UpdateReadOnly, MandatoryCheck, out interactionId, out eventId, out error,
                                                          out contactPreviouslyUnblocked);


                interactionID = Convert.ToInt32(interactionId);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Interaction" +interactionID.ToString()+"  " +  DateTime.Now.ToString() + System.Environment.NewLine);

            }
            catch (Exception ex)
            {
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Error in CreateInteraction  error message " + ex.ToString() + " error stackstrace  " + ex.StackTrace + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            }
            return interactionID;
        }

        private string GetPolicySchedulePath(string PolicyNumber)
        {
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Generating Policy schedule for Email. " + DateTime.Now.ToString() + System.Environment.NewLine);
            string ProductCode = "";
            string filename = "";


            if (!string.IsNullOrEmpty(hdnProductCode.Value))
            {
                ProductCode = hdnProductCode.Value;
            }
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Policy Number " + PolicyNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Productcode " + ProductCode + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            try
            {
                string ErrorMsg = string.Empty;
                WebRequest.DefaultWebProxy = null;
                PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();

                File.AppendAllText(FrmDownloadPolicyScheduleLog, "Getting Policy from GIST  Policy number " + PolicyNumber 
                    + " product code  " +ProductCode+"  "+ DateTime.Now.ToString() + System.Environment.NewLine);

                byte[] objByte = proxy.KGIGetPolicyDocumentForPortal("16e9e45962de4725a83994c4c3145517", PolicyNumber, ProductCode, ref ErrorMsg);
                if (ErrorMsg.Length <= 0)
                {
                    string filelocation = ConfigurationManager.AppSettings["KotakPolicySchedules"].ToString();
                    filename = filelocation + "\\" + PolicyNumber + ".pdf";
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    File.WriteAllBytes(filename, objByte);
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, "File Generated for policy number "+PolicyNumber+"  file name " + filename + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    return filename;
                }
                else
                {
                    Alert.Show("Some Error Occured. Kindly try after some time.", "FrmDownloadPolicySchedule.aspx");
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, "Error Occured " + ErrorMsg + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    return "";
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetPolicySchedulePath");
                sectionMain.Visible = false;
                sectionError.Visible = true;
            }
            return filename;
        }

        protected void DownloadSchedulePolicy(object sender, EventArgs e)
        {
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Download Policy Schedule start  " + DateTime.Now.ToString() + System.Environment.NewLine);
            bool IsSendOnMail = false;
            bool IsDownLoad = true;
            string interactionId = CreateInteraction(txtPolicyNumber.Text.Trim(), txtMobileNumber.Text.Trim(), txtEmailforPolicy.Text.Trim(), IsDownLoad).ToString();
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Interaction Created " + interactionID + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            AddSearchLog(txtPolicyNumber.Text.Trim(), txtMobileNumber.Text.Trim(), txtEmailforPolicy.Text.Trim(), IsSendOnMail, IsDownLoad, interactionId);
            try
            {

                string PolicyNumber = txtPolicyNumber.Text.Trim();
                string ProductCode = "";
                if (!string.IsNullOrEmpty(hdnProductCode.Value))
                {
                    ProductCode = hdnProductCode.Value;
                }
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "Downloading start for Policy Number" + PolicyNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "Product Code" + ProductCode + "  " + DateTime.Now.ToString() + System.Environment.NewLine);

                WebRequest.DefaultWebProxy = null;
                string ErrorMsg = string.Empty;
                PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
                byte[] objByte = proxy.KGIGetPolicyDocumentForPortal("16e9e45962de4725a83994c4c3145517", PolicyNumber, ProductCode, ref ErrorMsg); //1000401000 //1000340100

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = "application/force-download";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + PolicyNumber + ".pdf");
                    HttpContext.Current.Response.BinaryWrite(objByte);
                    //Response.End();
                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    CommonExtensions.fn_AddLogForDownload(PolicyNumber, "FrmDownloadPolicySchedule.aspx");//Added by Rajesh Soni 24/02/2020
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                    
                }
                else
                {
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, "Error Occured While Download Policy " + PolicyNumber + "  ErrorMessage "
                        + ErrorMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                }

            }
            catch (Exception ex)
            {
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "Error Occured While Download Policy error message " + ex.ToString() + "  Error stack " 
                    + ex.StackTrace + DateTime.Now.ToString() + System.Environment.NewLine);
                sectionMain.Visible = false;
                sectionError.Visible = true;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmDownloadPolicySchedule.aspx");
        }

    }
}