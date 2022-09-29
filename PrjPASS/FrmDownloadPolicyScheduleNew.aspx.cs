//using System.Data.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Web.Services3.Security.Tokens;
//using System.Data.SqlClient;

using ProjectPASS;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Winnovative;
using Newtonsoft.Json;

namespace PrjPASS
{
    public partial class FrmDownloadPolicyScheduleNew : System.Web.UI.Page
    {
        public int interactionID = 0;
        public string certificateNo = string.Empty;
        bool IsWithoutHeaderFooter = false;
        bool IsEmailRequest = false;
        string EmailSentMessage = string.Empty;
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        string userName = ConfigurationManager.AppSettings["userName"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();
        string TalismaSessionKey = ConfigurationManager.AppSettings["TalismaSessionKey"].ToString();
        int InteractioniServiceArrayIndex = Convert.ToInt32(ConfigurationManager.AppSettings["InteractioniServiceArrayIndex"].ToString());
        int ContactiServiceArrayIndex = Convert.ToInt32(ConfigurationManager.AppSettings["ContactiServiceArrayIndex"]);
        string ContactiserviceURL = ConfigurationManager.AppSettings["ContactiserviceURL"];
        string InteractioniserviceURL = ConfigurationManager.AppSettings["InteractioniserviceURL"];
        string FrmDownloadPolicyScheduleLog = AppDomain.CurrentDomain.BaseDirectory + "//FrmDownloadPolicyScheduleNew//log.txt";
        string FrmDownloadPolicyScheduleLogDirectory = AppDomain.CurrentDomain.BaseDirectory + "//FrmDownloadPolicyScheduleNew";
        string logFile = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/FrmDownloadPolicyScheduleNew_Error.txt";
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");
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

            //if (!IsPostBack)
            //{
            //    if (Session["vUserLoginId"] != null)
            //    {
            //        if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
            //        {
            //            bool chkAuth;
            //            string pageName = this.Page.ToString().Substring(4, this.Page.ToString().Substring(4).Length - 5) + ".aspx";
            //            chkAuth = wsGen.Fn_Check_Rights_For_Page(Session["vRoleCode"].ToString(), pageName);
            //            if (chkAuth == false)
            //            {
            //                Alert.Show("Access Denied", "FrmMainMenu.aspx");
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
            //        return;
            //    }
            //    Directory.CreateDirectory(folderPath);
            //}
            divtechnicalissueMessage.Visible = false;
            DivCancelledPolicyMessage.Visible = false;

            if ((ViewState["certificateNo"] != null) ||
                (ViewState["CRNNo"]) != null ||
                (ViewState["LANNumber"]) != null)
            {
                string Certificate = ViewState["certificateNo"] != null ? ViewState["certificateNo"].ToString() : "";
                string CRN = ViewState["CrnNo"] != null ? ViewState["CrnNo"].ToString() : "";
                string LoanNumber = ViewState["LoanNumber"] != null ? ViewState["LoanNumber"].ToString() : "";
                string PolicyNumber = ViewState["PolicyNumber"] != null ? ViewState["PolicyNumber"].ToString() : "";
                string AccountNumber = ViewState["AccountNumber"] != null ? ViewState["AccountNumber"].ToString() : "";
                string GroupUniqueIdentificationNumber = ViewState["GroupUniqueIdentificationNumber"] != null ? ViewState["GroupUniqueIdentificationNumber"].ToString() : "";
                string DOB = ViewState["DOB"] != null ? ViewState["DOB"].ToString() : "";
                string RegisteredMobileNumber = ViewState["RegisteredMobileNumber"] != null ? ViewState["RegisteredMobileNumber"].ToString() : "";
                string RegisteredEmailID = ViewState["RegisteredEmailID"] != null ? ViewState["RegisteredEmailID"].ToString() : "";

                //string PAN = ViewState["PANNumber"] != null ? ViewState["PANNumber"].ToString() : "";

                //fnSearchPolicyDetails(PolicyNumber, CRN, LoanNumber, RegisteredMobileNumber, DOB, RegisteredEmailID);
                fnSearchPolicyDetails(PolicyNumber, Certificate, CRN, LoanNumber, AccountNumber, GroupUniqueIdentificationNumber, RegisteredMobileNumber, DOB, RegisteredEmailID);
            }
        }

        //protected void btnOTPSend_Click(object sender, EventArgs e)
        //{
        //    string ErrorMessage = string.Empty;
        //    string msg = "";

        //    if (!fnCheckPolicyMobEmailRecord(txtPolicyNumber.Text, txtEmailID.Text, txtMobileNumber.Text))
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openInvalidPolicyErrorModal();", true);
        //        //UpdatePanel1.Update();
        //    }
        //    else
        //    {
        //        if (Validation(out ErrorMessage))
        //        {
        //            if (txtMobileNumber.Text.Length == 10)
        //            {
        //                string mobileNumber = txtMobileNumber.Text.Trim();
        //                msg = GenerateOTPNew(mobileNumber);
        //            }
        //            else if (!string.IsNullOrEmpty(txtEmailID.Text))
        //            {
        //                string EmailID = txtEmailID.Text.Trim();
        //                msg = GenerateOTPNewForEmail(EmailID);
        //            }

        //            if (msg == "success")
        //            {
        //                txtPolicyNumber.Enabled = false;
        //                txtMobileNumber.Enabled = false;
        //                txtEmailID.Enabled = false;
        //                btnOTPSend.Visible = false;
        //                otpPanel.Visible = true;
        //                btnMobileVerify.Focus();
        //                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "testpage", "runme();", true);
        //                //UpdatePanel1.Update();
        //            }
        //            else
        //            {
        //                txtPolicyNumber.Enabled = true;
        //                File.AppendAllText(FrmDownloadPolicyScheduleLog, "could not generate OTP for mobile number" + txtMobileNumber.Text + " Email ID " + txtEmailID.Text + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
        //                Alert.Show("could not generate OTP, kindly try after sometime");

        //            }
        //        }
        //        else
        //        {
        //            Alert.Show(ErrorMessage);
        //        }

        //    }

        //}

        private bool fnCheckPolicyMobEmailRecord(string PolicyNumber, string Email, string MobileNumber)
        {
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Searching for Policy record   Policy number  " + PolicyNumber + "  Email " + Email + "  Mobile " + MobileNumber + "   " + DateTime.Now.ToString() + System.Environment.NewLine);
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
                            res = true;
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

                //bool IsSendEmailSuccess = SendOTPEmail(GeneratedOTP, emailID);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "OTP generated for  Email ID " + emailID + "  and otp " + GeneratedOTP + "   " + DateTime.Now.ToString() + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                GeneratedOTP = "";
                ExceptionUtility.LogException(ex, "Error in GenerateOTP on FrmSTP Page");
            }
            return string.IsNullOrEmpty(GeneratedOTP) ? "" : "success";
        }

        //private bool SendOTPEmail(string generatedOTP, string emailID)
        //{
        //    bool Success = true;
        //    try
        //    {
        //        string emailId = txtEmailID.Text.Trim();
        //        string strPath = string.Empty;
        //        string MailBody = string.Empty;
        //        SmtpClient smtpClient = new SmtpClient();
        //        smtpClient.Port = 25;
        //        smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
        //        smtpClient.Timeout = 3600000;
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
        //        strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBody_SchedulePolicy_OTP.html";
        //        MailBody = File.ReadAllText(strPath);
        //        MailBody = MailBody.Replace("#Name", lblName.Text.Trim());
        //        MailBody = MailBody.Replace("#OTP", generatedOTP.ToString());
        //        MailMessage mm = new MailMessage();
        //        mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
        //        mm.Subject = string.Format("OTP for Policy Schedule - {0}", txtPolicyNumber.Text.Trim());
        //        mm.Body = MailBody;
        //        mm.To.Add(emailId);
        //        mm.IsBodyHtml = true;
        //        mm.BodyEncoding = UTF8Encoding.UTF8;

        //        smtpClient.Send(mm);
        //        Success = true;
        //        File.AppendAllText(FrmDownloadPolicyScheduleLog, "OTP " + generatedOTP + "sent to Email ID" + emailID + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, "SendOTPEmail");
        //        sectionMain.Visible = false;
        //        sectionError.Visible = true;
        //        Success = false;
        //    }
        //    return Success;
        //}

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

        //private bool Validation(out string ErrorMessage)
        //{
        //    ErrorMessage = string.Empty;
        //    if (string.IsNullOrEmpty(txtPolicyNumber.Text.ToString()))
        //    {
        //        ErrorMessage = "Please enter policy number!";
        //    }


        //    if (!string.IsNullOrEmpty(txtMobileNumber.Text.ToString()))
        //    {
        //        if (!Regex.IsMatch(txtMobileNumber.Text.Trim(), "^[0-9]*$") || txtMobileNumber.Text.Length != 10)
        //        {
        //            ErrorMessage = "Please enter valid 10 digit mobile number!";
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(txtEmailID.Text.Trim()))
        //    {
        //        if (!Regex.IsMatch(txtEmailID.Text.Trim(), @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
        //                                                   @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
        //                                                   @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
        //        {
        //            ErrorMessage = "Please enter valid E-Mail Address !";
        //        }

        //    }

        //    return ErrorMessage.Length > 0 ? false : true;
        //}

        //protected void onClickbtnMobileVerify(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bool isSuccess = false;
        //        if (!IsValidPolicyNumber(txtPolicyNumber.Text.Trim()))
        //        {
        //            otpPanel.Visible = false;
        //            txtPolicyNumber.Enabled = true;
        //            txtMobileNumber.Enabled = true;
        //            txtEmailID.Enabled = true;
        //            btnOTPSend.Visible = true;
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openInvalidPolicyErrorModal();", true);
        //            //UpdatePanel1.Update();
        //        }

        //        else if (txtOtpNumber.Text == HttpContext.Current.Session["OTPNumber"].ToString() && ValidateMobileEmailWithPolicy(txtMobileNumber.Text.Trim()))
        //        {
        //            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Verifying OTP " + txtOtpNumber.Text + "with mobile Number" + txtMobileNumber.Text + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
        //            otpPanel.Visible = false;
        //            dvPolicyDetails.Visible = true;
        //        }
        //        else
        //        {
        //            otpPanel.Visible = false;
        //            txtPolicyNumber.Enabled = true;
        //            txtMobileNumber.Enabled = true;
        //            txtEmailID.Enabled = true;
        //            btnOTPSend.Visible = true;
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openErrorModal();", true);
        //            //UpdatePanel1.Update();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        ExceptionUtility.LogException(ex, "onClickbtnMobileVerify");
        //    }

        //}

        //private bool IsValidPolicyNumber(string v)
        //{
        //    bool valid = true;
        //    try
        //    {
        //        File.AppendAllText(FrmDownloadPolicyScheduleLog, "Validating policy number " + v.ToString() + DateTime.Now.ToString() + System.Environment.NewLine);

        //        DataTable dt = new DataTable();

        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
        //        {
        //            if (con.State == ConnectionState.Closed)
        //            {
        //                con.Open();
        //            }

        //            SqlCommand cmd = new SqlCommand("PROC_GET_POLICY_NUMBER_AVAILABILITY_FOR_SCHEDULE", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@PolicyNumber", txtPolicyNumber.Text.Trim());
        //            SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //            adp.Fill(dt);
        //            if (dt.Rows.Count > 0)
        //            {
        //                valid = true;
        //            }
        //            else
        //            {
        //                valid = false;
        //            }
        //            return valid;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, "ValidateMobileWithPolicy");
        //        sectionMain.Visible = false;
        //        sectionError.Visible = true;

        //    }
        //    return valid;
        //}

        //private bool ValidateMobileEmailWithPolicy(string MobileNumber)
        //{
        //    bool valid = true;
        //    try
        //    {
        //        DataTable dtValidateMobile = new DataTable();

        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
        //        {
        //            if (con.State == ConnectionState.Closed)
        //            {
        //                con.Open();
        //            }

        //            SqlCommand cmd = new SqlCommand("PROC_GET_POLICY_DETAILS_FOR_SCHEDULE", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@PolicyNumber", txtPolicyNumber.Text.Trim());
        //            SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //            adp.Fill(dtValidateMobile);
        //            if (dtValidateMobile.Rows.Count > 0)
        //            {
        //                if (dtValidateMobile.Rows[0]["TXT_MOBILE"].ToString() == MobileNumber || dtValidateMobile.Rows[0]["TXT_EMAIL"].ToString().ToLower() == txtEmailID.Text.Trim().ToLower())
        //                {
        //                    lblName.Text = dtValidateMobile.Rows[0]["TXT_SALUTATION"].ToString() + " " + dtValidateMobile.Rows[0]["TXT_CUSTOMER_NAME"].ToString();
        //                    hdnProductCode.Value = dtValidateMobile.Rows[0]["NUM_PRODUCT_CODE"].ToString();
        //                    hdnDeptCode.Value = dtValidateMobile.Rows[0]["NUM_DEPARTMENT_CODE"].ToString();
        //                    if (!string.IsNullOrEmpty(dtValidateMobile.Rows[0]["TXT_EMAIL"].ToString()))
        //                    {
        //                        txtEmailforPolicy.Text = dtValidateMobile.Rows[0]["TXT_EMAIL"].ToString();
        //                    }

        //                    valid = true;
        //                }
        //                else
        //                {
        //                    valid = false;
        //                }

        //            }
        //            else
        //            {
        //                valid = true;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, "ValidateMobileWithPolicy");
        //        sectionMain.Visible = false;
        //        sectionError.Visible = true;

        //    }
        //    return valid;
        //}


        //protected void onClickbtnMobileReSend(object sender, EventArgs e)
        //{
        //  //  btnOTPSend_Click(sender, e);
        //}


        //protected void SendSchedulePolicyEmail(object sender, EventArgs e)
        //{
        //    File.AppendAllText(FrmDownloadPolicyScheduleLog, "Started to send mail  " + DateTime.Now.ToString() + System.Environment.NewLine);
        //    bool IsSendOnMail = true;
        //    bool IsDownLoad = false;
        //    if (!Regex.IsMatch(txtEmailforPolicy.Text.Trim(), @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
        //                                                   @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
        //                                                   @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
        //    {
        //        Alert.Show("Please enter valid Email Address !");
        //        return;
        //    }

        //    string InteractionID = CreateInteraction(txtPolicyNumber.Text.Trim(), txtMobileNumber.Text.Trim(), txtEmailforPolicy.Text.Trim(), IsDownLoad).ToString();
        //    File.AppendAllText(FrmDownloadPolicyScheduleLog, "Interaction Created " + interactionID + " " + DateTime.Now.ToString() + System.Environment.NewLine);
        //    AddSearchLog(txtPolicyNumber.Text.Trim(), txtMobileNumber.Text.Trim(), txtEmailforPolicy.Text.Trim(), IsSendOnMail, IsDownLoad, InteractionID);
        //    try
        //    {
        //        string emailId = txtEmailforPolicy.Text.Trim();
        //        string strPath = string.Empty;
        //        string MailBody = string.Empty;
        //        SmtpClient smtpClient = new SmtpClient();
        //        smtpClient.Port = 25;
        //        smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
        //        smtpClient.Timeout = 3600000;
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
        //        strPath = AppDomain.CurrentDomain.BaseDirectory + "Schedule_Policy_Email_Body.html";
        //        MailBody = File.ReadAllText(strPath);
        //        MailBody = MailBody.Replace("#FullCustomerName", lblName.Text.Trim());
        //        MailMessage mm = new MailMessage();
        //        mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
        //        mm.Subject = string.Format(ConfigurationManager.AppSettings["Schedule_Policy_email_Subject"], txtPolicyNumber.Text.Trim());
        //        mm.Body = MailBody;
        //        mm.To.Add(emailId);
        //        mm.IsBodyHtml = true;
        //        mm.BodyEncoding = UTF8Encoding.UTF8;
        //        string attachmentFilename = GetPolicySchedulePath(txtPolicyNumber.Text.Trim());
        //        //Alert.Show("File Name " + attachmentFilename.ToString());
        //        if (attachmentFilename != null)
        //        {
        //            Attachment attachment = new Attachment(Path.GetFullPath(attachmentFilename), MediaTypeNames.Application.Octet);
        //            ContentDisposition disposition = attachment.ContentDisposition;
        //            disposition.CreationDate = File.GetCreationTime(attachmentFilename);
        //            disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
        //            disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
        //            disposition.FileName = Path.GetFileName(attachmentFilename);
        //            disposition.Size = new FileInfo(attachmentFilename).Length;
        //            disposition.DispositionType = DispositionTypeNames.Attachment;
        //            mm.Attachments.Add(attachment);
        //        }

        //        smtpClient.Send(mm);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openPolicyEmailSentModel();", true);
        //        //UpdatePanel1.Update();
        //    }
        //    catch (Exception ex)
        //    {
        //        File.AppendAllText(FrmDownloadPolicyScheduleLog, " Error in SendSchedulePolicyEmail  error message " + ex.ToString() + " error stackstrace  " + ex.StackTrace + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openPolicyEmailNotSentModel();", true);
        //        //UpdatePanel1.Update();
        //        sectionMain.Visible = false;
        //        sectionError.Visible = true;
        //    }
        //}

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
                       // cmd.Parameters.AddWithValue("@vOTP", HttpContext.Current.Session["OTPNumber"].ToString());
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
               // string USERNAME = lblName.Text.Trim();
                returnValue1 = objContactWebService.ResolveContact(false, objPropertyInfo, out nContactID, out szError);

               // File.AppendAllText(FrmDownloadPolicyScheduleLog, "  returnValue1  " + returnValue1+ "" + DateTime.Now.ToString() + System.Environment.NewLine);

                if (nContactID == -1)
                {

                    returnValue1 = objContactWebService.CreateContact(MobileNumber, objPropertyInfo, true, true, out nContactID, out szError);
                   // File.AppendAllText(FrmDownloadPolicyScheduleLog, "  returnValue1  " + returnValue1 + "" + DateTime.Now.ToString() + System.Environment.NewLine);


                }

              // string hdnDeptCode = hdnDeptCode.Value;

                string CaseTypePropText = "", CaseTypePropValue = "", LOB = "", LOB21711 = "";
               
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " hdnDeptCode  :   " + " - "+ hdnDeptCode.Value+"  " + DateTime.Now.ToString() + System.Environment.NewLine);


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
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 0 LOB Prop Value  :  " + LOB21711.ToString() + DateTime.Now.ToString() + System.Environment.NewLine);
                //


                // 
                objInteractionPropertyInfo[1] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[1].propertyID = 21481;                 // 21481 = Casetype
                objInteractionPropertyInfo[1].propValue = CaseTypePropValue;
                objInteractionPropertyInfo[1].rowID = -1;
                objInteractionPropertyInfo[1].relJoinID = -1;
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 1 CaseType propertyID  21481 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 1 CaseType Prop Value :   " + CaseTypePropValue.ToString() + DateTime.Now.ToString() + System.Environment.NewLine);

                //


                //
                objInteractionPropertyInfo[2] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[2].propertyID = 21483;                //Calltype
                objInteractionPropertyInfo[2].propValue = "73";                 //73 for  "Website Query";
                objInteractionPropertyInfo[2].rowID = -1;
                objInteractionPropertyInfo[2].relJoinID = -1;

                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 2 CallType propertyID  21483 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 2 CallType Prop Value : 73  : " + DateTime.Now.ToString() + System.Environment.NewLine);
                //

                //
                objInteractionPropertyInfo[3] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[3].propertyID = 21484; //SubCalltype Property ID
                if (isDownload)
                {
                    objInteractionPropertyInfo[3].propValue = "288"; // policy download
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 SubCallType propertyID  21484 " + DateTime.Now.ToString() + System.Environment.NewLine);
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 subCallType Prop Value : 288 : " + DateTime.Now.ToString() + System.Environment.NewLine);
                }
                else
                {
                    objInteractionPropertyInfo[3].propValue = "289"; // policy email
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 SubCallType propertyID  21484 " + DateTime.Now.ToString() + System.Environment.NewLine);
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 subCallType Prop Value  : 289  : " + DateTime.Now.ToString() + System.Environment.NewLine);
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
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 4 Prop Value  : " + userMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                //


                //
                objInteractionPropertyInfo[5] = new InteractionWS.PropertyInfo();
                objInteractionPropertyInfo[5].propertyID = 21617; //
                objInteractionPropertyInfo[5].propValue = "1"; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                objInteractionPropertyInfo[5].rowID = -1;
                objInteractionPropertyInfo[5].relJoinID = -1;
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 5 propertyID  21617 " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 5 Prop Value :  1  : " + userMsg + DateTime.Now.ToString() + System.Environment.NewLine);
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
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 7 Prop Value :  " + LOB + DateTime.Now.ToString() + System.Environment.NewLine);

                //

                #region
                //if (hdnDeptCode.Value == "31")
                //{
                //    File.AppendAllText(FrmDownloadPolicyScheduleLog, "  Motor : - " + hdnDeptCode.Value + "" + DateTime.Now.ToString() + System.Environment.NewLine);

                //    CaseTypePropText = "Motor";
                //    CaseTypePropValue = "12328"; // Case type Property Value
                //    LOB = "11221";           // LOB Property ID
                //    LOB21711 = "11221";          // LOB Case Type motor
                //}
                //else if (hdnDeptCode.Value == "28")
                //{
                //    File.AppendAllText(FrmDownloadPolicyScheduleLog, "  Health : - " + hdnDeptCode.Value + "" + DateTime.Now.ToString() + System.Environment.NewLine);

                //    CaseTypePropText = "Health";
                //    CaseTypePropValue = "12324";  // Case Type Property Value
                //    LOB = "11220";            // LOB Property ID
                //    LOB21711 = "11220";           // LOB Case Type health
                //}
                //else
                //{
                //    File.AppendAllText(FrmDownloadPolicyScheduleLog, "  Other : - " + hdnDeptCode.Value + "" + DateTime.Now.ToString() + System.Environment.NewLine);

                //    CaseTypePropText = "Other";
                //    CaseTypePropValue = "12331";   // Case Type Property Value
                //    LOB = "11219";             // LOB Property ID
                //    LOB21711 = "11219";           // LOB Case Type other
                //}


                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Creating Interaction  " + DateTime.Now.ToString() + System.Environment.NewLine);

                ////  Setting LOB and LOB Case Type
                //objInteractionPropertyInfo[0] = new InteractionWS.PropertyInfo();
                //objInteractionPropertyInfo[0].propertyID = 21711;
                //objInteractionPropertyInfo[0].propValue = LOB21711;
                //objInteractionPropertyInfo[0].rowID = -1;
                //objInteractionPropertyInfo[0].relJoinID = -1;
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 0 LOB propertyID  21711 :" + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 0 LOB Prop Value  " + LOB21711.ToString() + DateTime.Now.ToString() + System.Environment.NewLine);
                ////
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 0 propertyID   : " + objInteractionPropertyInfo[0].propertyID + "Date: " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 0 Prop Value : " + objInteractionPropertyInfo[0].propValue + DateTime.Now.ToString() + System.Environment.NewLine);


                //// 
                //objInteractionPropertyInfo[1] = new InteractionWS.PropertyInfo();
                //objInteractionPropertyInfo[1].propertyID = 21481;                 // 21481 = Casetype
                //objInteractionPropertyInfo[1].propValue = CaseTypePropValue;
                //objInteractionPropertyInfo[1].rowID = -1;
                //objInteractionPropertyInfo[1].relJoinID = -1;
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 1 CaseType propertyID 21481 :  " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 1 CaseType Prop Value  " + CaseTypePropValue.ToString() + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 1 propertyID   : " + objInteractionPropertyInfo[1].propertyID + "Date: " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 1 Prop Value : " + objInteractionPropertyInfo[1].propValue + DateTime.Now.ToString() + System.Environment.NewLine);

                ////


                ////
                //objInteractionPropertyInfo[2] = new InteractionWS.PropertyInfo();
                //objInteractionPropertyInfo[2].propertyID = 21483;                //CallType 21483	
                //objInteractionPropertyInfo[2].propValue = "87044";                 //73 for  "Website Query";
                //objInteractionPropertyInfo[2].rowID = -1;
                //objInteractionPropertyInfo[2].relJoinID = -1;

                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 2 propertyID    : " + objInteractionPropertyInfo[2].propertyID + "Date: " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 2 Prop Value : " + objInteractionPropertyInfo[2].propValue + DateTime.Now.ToString() + System.Environment.NewLine);

                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 2 CallType propertyID  21483 :" + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 2 CallType Prop Value 87044 :" + DateTime.Now.ToString() + System.Environment.NewLine);
                ////

                ////
                //objInteractionPropertyInfo[3] = new InteractionWS.PropertyInfo();
                //objInteractionPropertyInfo[3].propertyID = 21484; //SubCalltype Property ID
                //if (isDownload)
                //{
                //    objInteractionPropertyInfo[3].propValue = "87135"; // policy download
                //    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 SubCallType propertyID  21484 :" + DateTime.Now.ToString() + System.Environment.NewLine);
                //    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 subCallType Prop Value  87135 :" + DateTime.Now.ToString() + System.Environment.NewLine);
                //}
                //else
                //{
                //    objInteractionPropertyInfo[3].propValue = "289"; // policy email
                //    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 SubCallType propertyID  21484 : " + DateTime.Now.ToString() + System.Environment.NewLine);
                //    File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 subCallType Prop Value  289 : " + DateTime.Now.ToString() + System.Environment.NewLine);
                //}

                //objInteractionPropertyInfo[3].rowID = -1;
                //objInteractionPropertyInfo[3].relJoinID = -1;

                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 propertyID   : " + objInteractionPropertyInfo[3].propertyID + "Date: " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 3 Prop Value : " + objInteractionPropertyInfo[3].propValue + DateTime.Now.ToString() + System.Environment.NewLine);

                ////

                ////
                //objInteractionPropertyInfo[4] = new InteractionWS.PropertyInfo();
                //objInteractionPropertyInfo[4].propertyID = 21619; //
                //objInteractionPropertyInfo[4].propValue = userMsg; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                //objInteractionPropertyInfo[4].rowID = -1;
                //objInteractionPropertyInfo[4].relJoinID = -1;

                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 4 propertyID   : " + objInteractionPropertyInfo[4].propertyID + "Date: " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 4 Prop Value : " + objInteractionPropertyInfo[4].propValue + DateTime.Now.ToString() + System.Environment.NewLine);

                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 4 propertyID  21619 : " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 4 Prop Value : " + userMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                ////


                ////
                //objInteractionPropertyInfo[5] = new InteractionWS.PropertyInfo();
                //objInteractionPropertyInfo[5].propertyID = 21617; //
                //objInteractionPropertyInfo[5].propValue = "11084"; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                //objInteractionPropertyInfo[5].rowID = -1;
                //objInteractionPropertyInfo[5].relJoinID = -1;
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 5 propertyID   : " + objInteractionPropertyInfo[5].propertyID + "Date: " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 5 Prop Value : " + objInteractionPropertyInfo[5].propValue + DateTime.Now.ToString() + System.Environment.NewLine);

                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 5 propertyID  21617 : " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 5 Prop Value  11084 : " + userMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                ////

                ////
                //objInteractionPropertyInfo[6] = new InteractionWS.PropertyInfo();
                //objInteractionPropertyInfo[6].propertyID = 22235; //
                //objInteractionPropertyInfo[6].propValue = "86944";  // "Policy sent successfully"; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                //objInteractionPropertyInfo[6].rowID = -1;
                //objInteractionPropertyInfo[6].relJoinID = -1;
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 6 propertyID  22235 :" + +objInteractionPropertyInfo[6].propertyID + "Date: " +  DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 6 Prop Value  86944 : " + userMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 6 Prop Value : " + objInteractionPropertyInfo[6].propValue + DateTime.Now.ToString() + System.Environment.NewLine);

                ////

                ////
                //objInteractionPropertyInfo[7] = new InteractionWS.PropertyInfo();
                //objInteractionPropertyInfo[7].propertyID = 23369; //
                //objInteractionPropertyInfo[7].propValue = LOB; // DSGSGDSGDSFDFDF"; // WHAT WILL BE THE PROP VALUE?
                //objInteractionPropertyInfo[7].rowID = -1;
                //objInteractionPropertyInfo[7].relJoinID = -1;

                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 7 propertyID  23369 : " + objInteractionPropertyInfo[7].propertyID + "Date: " + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 7 Prop Value : " + LOB + DateTime.Now.ToString() + System.Environment.NewLine);
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element 7 Prop Value : " + objInteractionPropertyInfo[7].propValue + DateTime.Now.ToString() + System.Environment.NewLine);

                ////
                //File.AppendAllText(FrmDownloadPolicyScheduleLog, " Element  Prop Value : " + objInteractionPropertyInfo[0].propertyID + DateTime.Now.ToString() + System.Environment.NewLine);

                //returnValue1 = objInteractionWebService.CreateInteraction(nContactID, phone, DateTime.Now, receivedByUSerID, MediaId,
                //                                          Direction, Subject, teamId, AssignedtoUserID, AliasID, Priority, Resolved, contactMsg, userMsg, attach,
                //                                          objInteractionPropertyInfo, UpdateReadOnly, MandatoryCheck, out interactionId, out eventId, out error,
                //                                          out contactPreviouslyUnblocked);
                #endregion

                var json = JsonConvert.SerializeObject(objInteractionPropertyInfo);
               
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Request json " + json.ToString() + "  " + DateTime.Now.ToString() + System.Environment.NewLine);

                returnValue1 = objInteractionWebService.CreateInteraction(nContactID, phone, DateTime.Now, receivedByUSerID, MediaId,
                                                         Direction, Subject, teamId, AssignedtoUserID, AliasID, Priority, Resolved, contactMsg, userMsg, attach,
                                                         objInteractionPropertyInfo, UpdateReadOnly, MandatoryCheck, out interactionId, out eventId, out error,
                                                         out contactPreviouslyUnblocked);

                interactionID = Convert.ToInt32(interactionId);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, " Interaction" + interactionID.ToString() + "  " + DateTime.Now.ToString() + System.Environment.NewLine);

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
                    + " product code  " + ProductCode + "  " + DateTime.Now.ToString() + System.Environment.NewLine);

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
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, "File Generated for policy number " + PolicyNumber + "  file name " + filename + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
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

        //protected void DownloadSchedulePolicy(object sender, EventArgs e)
        //{
        //    File.AppendAllText(FrmDownloadPolicyScheduleLog, "Download Policy Schedule start  " + DateTime.Now.ToString() + System.Environment.NewLine);
        //    bool IsSendOnMail = false;
        //    bool IsDownLoad = true;
        //    string interactionId = CreateInteraction(txtPolicyNumber.Text.Trim(), txtMobileNumber.Text.Trim(), txtEmailforPolicy.Text.Trim(), IsDownLoad).ToString();
        //    File.AppendAllText(FrmDownloadPolicyScheduleLog, "Interaction Created " + interactionID + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
        //    AddSearchLog(txtPolicyNumber.Text.Trim(), txtMobileNumber.Text.Trim(), txtEmailforPolicy.Text.Trim(), IsSendOnMail, IsDownLoad, interactionId);
        //    try
        //    {

        //        string PolicyNumber = txtPolicyNumber.Text.Trim();
        //        string ProductCode = "";
        //        if (!string.IsNullOrEmpty(hdnProductCode.Value))
        //        {
        //            ProductCode = hdnProductCode.Value;
        //        }
        //        File.AppendAllText(FrmDownloadPolicyScheduleLog, "Downloading start for Policy Number" + PolicyNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
        //        File.AppendAllText(FrmDownloadPolicyScheduleLog, "Product Code" + ProductCode + "  " + DateTime.Now.ToString() + System.Environment.NewLine);

        //        WebRequest.DefaultWebProxy = null;
        //        string ErrorMsg = string.Empty;
        //        PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
        //        byte[] objByte = proxy.KGIGetPolicyDocumentForPortal("16e9e45962de4725a83994c4c3145517", PolicyNumber, ProductCode, ref ErrorMsg); //1000401000 //1000340100

        //        if (string.IsNullOrEmpty(ErrorMsg))
        //        {
        //            HttpContext.Current.Response.Clear();
        //            HttpContext.Current.Response.ContentType = "application/force-download";
        //            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + PolicyNumber + ".pdf");
        //            HttpContext.Current.Response.BinaryWrite(objByte);
        //            //Response.End();
        //            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //            CommonExtensions.fn_AddLogForDownload(PolicyNumber, "FrmDownloadPolicySchedule.aspx");//Added by Rajesh Soni 24/02/2020
        //            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.

        //        }
        //        else
        //        {
        //            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Error Occured While Download Policy " + PolicyNumber + "  ErrorMessage "
        //                + ErrorMsg + DateTime.Now.ToString() + System.Environment.NewLine);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        File.AppendAllText(FrmDownloadPolicyScheduleLog, "Error Occured While Download Policy error message " + ex.ToString() + "  Error stack "
        //            + ex.StackTrace + DateTime.Now.ToString() + System.Environment.NewLine);
        //        sectionMain.Visible = false;
        //        sectionError.Visible = true;
        //    }
        //}

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmDownloadPolicyScheduleNew.aspx");
        }
        private void RadioGPSHPolicy_CheckedChanged(Object sender, EventArgs e)
        {
            // Change the check box position to be opposite its current position.
            if (RadioGPSHPolicy.Checked)
            {
                ddlPrimaryParameter.Visible =false ;
            }
            else
            {
                ddlPrimaryParameter.Visible = true;
            }
        }
       
        //protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridView1.PageIndex = e.NewPageIndex;
        //    this.BindGrid();
        //}
        //private void BindGrid()
        //{
        //    string constr = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT CreatedBy,MobileNumber,CityDistrictName,CountryName FROM TBL_CUSTOMER_DETAILS"))
        //        {
        //            using (SqlDataAdapter sda = new SqlDataAdapter())
        //            {
        //                cmd.Connection = con;
        //                sda.SelectCommand = cmd;
        //                using (DataTable dt = new DataTable())
        //                {
        //                    sda.Fill(dt);
        //                    GridView1.DataSource = dt;
        //                    GridView1.DataBind();
        //                }
        //            }
        //        }
        //    }
        //}


        protected void btnSearch_Click(object sender, EventArgs e)
        {



            if(ddlPrimaryParameter.SelectedValue == "Policy Number")
            {
                ViewState["PolicyNumber"] = txtPrimaryParameter.Text;
            }
            else if(ddlPrimaryParameter.SelectedValue == "Certificate Number")
            {
                ViewState["certificateNo"] = txtPrimaryParameter.Text;
            }
            else if (ddlPrimaryParameter.SelectedValue == "CRN Number")
            {
                ViewState["CrnNo"] = txtPrimaryParameter.Text;
            }
            else if (ddlPrimaryParameter.Text == "Account Number")
            {
                ViewState["AccountNumber"] = txtPrimaryParameter.Text;
            }
            else if (ddlPrimaryParameter.Text == "Loan Account Number")
            {
                ViewState["LoanNumber"] = txtPrimaryParameter.Text;
            }
            else if (ddlPrimaryParameter.Text == "Group Unique Identification Number")
            {
                ViewState["GroupUniqueIdentificationNumber"] = txtPrimaryParameter.Text;
            }
             if (ddlSecondaryParameter.Text == "Date Of Birth")
            {
                ViewState["DOB"] = txtDOB.Text;
            }
            else if (ddlSecondaryParameter.Text == "Registered Mobile Number")
            {
                ViewState["RegisteredMobileNumber"] = txtSecondaryParameter.Text;
               
                if (ViewState["RegisteredMobileNumber"].ToString().Length > 10 || ViewState["RegisteredMobileNumber"].ToString().Length < 10)
                {

                    Alert.Show(" Please enter Valid 10 digit registered mobile number");
                    return;
                }


            }
            else if (ddlSecondaryParameter.Text == "Registered Email ID")
            {
                ViewState["RegisteredEmailID"] = txtSecondaryParameter.Text;
            }
            string Certificate = ViewState["certificateNo"] != null ? ViewState["certificateNo"].ToString() : "";
            string CRN = ViewState["CrnNo"] != null ? ViewState["CrnNo"].ToString() : "";
            string LoanNumber = ViewState["LoanNumber"] != null ? ViewState["LoanNumber"].ToString() : "";
            string PolicyNumber = ViewState["PolicyNumber"] != null ? ViewState["PolicyNumber"].ToString() : "";
            string AccountNumber = ViewState["AccountNumber"] != null ? ViewState["AccountNumber"].ToString() : "";
            string GroupUniqueIdentificationNumber = ViewState["GroupUniqueIdentificationNumber"] != null ? ViewState["GroupUniqueIdentificationNumber"].ToString() : "";
            string DOB = ViewState["DOB"] != null ? ViewState["DOB"].ToString() : "";
            string RegisteredMobileNumber = ViewState["RegisteredMobileNumber"] != null ? ViewState["RegisteredMobileNumber"].ToString() : "";
            string RegisteredEmailID = ViewState["RegisteredEmailID"] != null ? ViewState["RegisteredEmailID"].ToString() : "";
            
            //string PAN = ViewState["PANNumber"] != null ? ViewState["PANNumber"].ToString() : "";

            fnSearchPolicyDetails(PolicyNumber, Certificate, CRN, LoanNumber, AccountNumber, GroupUniqueIdentificationNumber, RegisteredMobileNumber, DOB, RegisteredEmailID);
            //ViewState["PANNumber"] = txtPANnumber.Text;
           // fnSearchPolicyDetails(ViewState["certificateNo"].ToString(), ViewState["CrnNo"].ToString(), ViewState["LoanNumber"].ToString());
        }

        private void fnSearchPolicyDetails(string PolicyNumber, string Certificate, string CRNNumber, string LANNumber, string AccountNumber, string GroupUniqueIdentificationNumber, string MobileNo, string DOB, String EmailId)
        {

         
            if (!string.IsNullOrEmpty(DOB))
            {
                
                DateTime dt = DateTime.ParseExact(DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (dt.Date > DateTime.Now.Date)
                {
                    Alert.Show(" Date Of Birth should Not be Future Date");
                    return;
                }
            }

            if (PolicyNumber.Length > 20 )
            {

                Alert.Show(" Policy Number cannot be greater than 20 digits” ");
                return;
            }
            if (Certificate.Length > 20)
            {

                Alert.Show(" Certificate Number cannot be greater than 20 digits ");
                return;
            }
            if (CRNNumber.Length > 20)
            {

                Alert.Show(" CRN Number cannot be greater than 20 digits ");
                return;
            }
            if (LANNumber.Length > 20)
            {

                Alert.Show(" Loan Account Number cannot be greater than 20 digits ");
                return;
            }
            if (AccountNumber.Length > 20)
            {

                Alert.Show(" Account Number cannot be greater than 20 digits  ");
                return;
            }
            if (GroupUniqueIdentificationNumber.Length > 20)
            {

                Alert.Show(" Group Unique Identification Number cannot be greater than 20 digits  ");
                return;
            }
            if (EmailId.Length > 200)
            {

                Alert.Show(" Please enter Valid registered email id ");
                return;
            }
            GvPolicyData.DataSource = null;
            GvPolicyData.DataBind();
            GvPolicyData.Visible = false;

           //int columncount = GvPolicyData.Rows[0].Cells.Count;
            //GvPolicyData.Rows[0].Cells.Clear();
            //GvPolicyData.Columns.Clear();

            if (string.IsNullOrWhiteSpace(txtPrimaryParameter.Text) ||  string.IsNullOrWhiteSpace(ddlPrimaryParameter.Text) || string.IsNullOrWhiteSpace(ddlSecondaryParameter.Text))
            {
                Alert.Show("Please select and enter value of all search parameter to search policy details.");
                GvPolicyData.DataSource = null;
                return;
            }
            else if(ddlSecondaryParameter.Text != "Date Of Birth" && string.IsNullOrWhiteSpace(txtSecondaryParameter.Text))
            {
                Alert.Show("Please enter Secondary Parameter value to search policy details.");
                GvPolicyData.DataSource = null;
                return;
            }
            else if (ddlSecondaryParameter.Text == "Date Of Birth" && string.IsNullOrWhiteSpace(txtDOB.Text))
            {
                Alert.Show("Please select Date Of Birth parameter to search policy details.");
                GvPolicyData.DataSource = null;
                return;
            }
            else if(RadioGPSHPolicy.Checked == false && RadioORPolicy.Checked==false)
                {
                Alert.Show("Please checked check box for search policy details.");
                GvPolicyData.DataSource = null;
                return;
            }
                if (string.IsNullOrEmpty(txtSecondaryParameter.Text)
                && string.IsNullOrEmpty(txtPrimaryParameter.Text)
                && string.IsNullOrEmpty(ddlSecondaryParameter.Text)
                && string.IsNullOrEmpty(ddlPrimaryParameter.Text))
               
               
            {
                Alert.Show("Please enter any two search parameter to search policy details.");
                GvPolicyData.DataSource = null;
                return;
            }
            try
            {
                bool IsInPolicyStatus = false;
               

                Check_policyStatus(PolicyNumber,out IsInPolicyStatus);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (RadioORPolicy.Checked || RadioGPSHPolicy.Checked )
                    { 
                        using (SqlCommand cmd = new SqlCommand("SP_GET_RECORD_FROM_GIST_PASS_DOWNLOAD_new", con))
                        {
                            cmd.CommandTimeout = 240;
                            cmd.CommandType = CommandType.StoredProcedure;
                            
                            cmd.Parameters.AddWithValue("@vPolicyNumber", PolicyNumber);
                            cmd.Parameters.AddWithValue("@vCertNumber", Certificate);
                            cmd.Parameters.AddWithValue("@vCRNNumber", CRNNumber);
                            cmd.Parameters.AddWithValue("@vAccountNumber", AccountNumber);
                            cmd.Parameters.AddWithValue("@vLANNumber", LANNumber);
                            cmd.Parameters.AddWithValue("@vGroupUniqueIdentificationNumber", GroupUniqueIdentificationNumber);
                            cmd.Parameters.AddWithValue("@vMobileNo", MobileNo);
                            cmd.Parameters.AddWithValue("@varDate", DOB);
                            cmd.Parameters.AddWithValue("@EmailID", EmailId);

                            //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                            //DataSet dtPolicyRecord = new DataSet();
                            //DataTable ds = new DataTable();
                            //sda.Fill(dtPolicyRecord);
                            //GvPolicyData.DataSource = dtPolicyRecord;
                            //GvPolicyData.DataBind();
                            //cmd.Parameters.AddWithValue("@vPANNumber", txtPANnumber.Text);
                            DataSet ds = new DataSet();
                            DataTable dtPolicyRecord = new DataTable();
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        
                            adp.Fill(dtPolicyRecord);


                            if (dtPolicyRecord.Rows.Count == 0)
                            {
                                if(IsInPolicyStatus == true)
                                {
                                    DivCancelledPolicyMessage.Visible = true;
                                    GvPolicyData.DataSource = null;
                                    //gvPolicyData.DataBind();
                                    GvPolicyData.Visible = false;
                                }
                                else
                                {
                                    GvPolicyData.DataSource = dtPolicyRecord;
                                    GvPolicyData.DataBind();
                                    GvPolicyData.Visible = true;
                                    dtPolicyRecord.Clear();
                                }
                            }
                            else
                            {



                                // gvPolicyData.DataSource = dtPolicyRecord;
                                // gvPolicyData.DataBind();



                                GvPolicyData.DataSource = dtPolicyRecord;
                                GvPolicyData.DataBind();
                                GvPolicyData.Visible = true;
                                ViewState.Clear();
                            } 

                        }
                         
                    }
                    else
                        {
                        using (SqlCommand cmd = new SqlCommand("GET_POLICY_NUMBER_PASS_DOWNLOAD_DOB_CRN", con))
                        {
                            cmd.CommandTimeout = 240;
                            cmd.CommandType = CommandType.StoredProcedure;
                            
                            cmd.Parameters.AddWithValue("@vCrnNo", CRNNumber);
                            cmd.Parameters.AddWithValue("@vCustomerDob", DOB);

                            DataTable dtPolicyRecord = new DataTable();
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            adp.Fill(dtPolicyRecord);
                            GvPolicyData.DataSource = dtPolicyRecord;
                            GvPolicyData.DataBind();

                        }

                    }

                }


               

                }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, "Error in btnSearch_Click "
                    + ex.ToString() + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while searching Policy details");
            }
        }

        #region
        private void Check_policyStatus(string PolicyNumber,  out bool IsInPolicyStatus) //IsinProgress = 1 for inprocess , 0 for done  
        {


            IsInPolicyStatus = false;
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_GET_DOWNLOAD_POLICY_STATUS";

                        cmd.Parameters.AddWithValue("@PolicyNumber", PolicyNumber);
                        

                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(ds);
                    }
                }

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //IsInProgressStatus_exists = ds.Tables[0].Rows[0]["IsInProgress"].ToString() ;
                            //IsInPolicyStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsInProgress"]);
                            //bool IsInProgressStatus_exists1 = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsInProgress"]);
                            IsInPolicyStatus = ds.Tables[0].Rows[0]["IsPolicyStatus"].ToString() == "0" ? false : true;


                        }
                    }
                }
            }
            catch (Exception ex)
            {

                File.AppendAllText(logFile, "Error in btnSearch_Click_STATUS "
                     + ex.ToString() + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while searching Policy details");
            }
        }

        #endregion




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
                    cmd.Parameters.AddWithValue("@PolicyNumber", MobileNumber.Trim());
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtValidateMobile);
                    if (dtValidateMobile.Rows.Count > 0)
                    {
                        
                           // lblName.Text = dtValidateMobile.Rows[0]["TXT_SALUTATION"].ToString() + " " + dtValidateMobile.Rows[0]["TXT_CUSTOMER_NAME"].ToString();
                            hdnProductCode.Value = dtValidateMobile.Rows[0]["NUM_PRODUCT_CODE"].ToString();
                            hdnDeptCode.Value = dtValidateMobile.Rows[0]["NUM_DEPARTMENT_CODE"].ToString();
                            //if (!string.IsNullOrEmpty(dtValidateMobile.Rows[0]["TXT_EMAIL"].ToString()))
                            //{
                            //    txtEmailforPolicy.Text = dtValidateMobile.Rows[0]["TXT_EMAIL"].ToString();
                            //}

                            valid = true;
                       
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


        #region Download link
        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            certificateNo = (sender as LinkButton).CommandArgument;
            LinkButton button = (LinkButton)sender;
            var CustomerName = (string)button.Attributes["data-myData"];
            var emailId = (string)button.Attributes["data-ID"];
            var productcode = (string)button.Attributes["data-Productcode"];
            var productName = (string)button.Attributes["data-ProductName"];
            var MobileNumber = (string)button.Attributes["data-myMobile"];
            ValidateMobileEmailWithPolicy(certificateNo.Trim());

            if (productName.Contains("KOTAK GROUP ACCIDENT PROTECT"))
            {
                fnGenerateGPA_Protect_Schedule(certificateNo);
            }
            else if (productName.Contains("GROUP ACCIDENT CARE"))
            {
                #region PDF for Care
                //File.AppendAllText(logFile, " if (productName.Contains(GROUP ACCIDENT CARE)) for gpa care certificate :" + certificateNo + " " + DateTime.Now + Environment.NewLine);
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_With_GST_Test_HeaderFooter.html";
                string htmlBody = File.ReadAllText(strPath);
                string custStateCode = string.Empty;
                string PolicyIssuingOfficeAddress = string.Empty;

                string IntermediaryName = string.Empty;
                string IntermediaryCode = string.Empty;
                string IntermediaryLandline = string.Empty;
                string IntermediaryMobile = string.Empty;

                string PolicyholderName = string.Empty;
                string PolicyholderAddress = string.Empty;
                string PolicyholderAddress2 = string.Empty;
                string PolicyholderBusinessDescription = string.Empty;
                string PolicyholderTelephoneNumber = string.Empty;
                string PolicyholderEmailAddress = string.Empty;
                string PolicyNumber = string.Empty;
                string PolicyInceptionDateTime = string.Empty;
                string PolicyExpiryDateTime = string.Empty;
                string TotalNumberOfInsuredPersons = string.Empty;

                string RowCoverHeader = string.Empty;
                string SectionARow = string.Empty;
                string ExtSectionARow = string.Empty;
                string SectionBRow = string.Empty;

                string NameofInsuredPerson = string.Empty;
                string DateOfBirth = string.Empty;
                string Gender = string.Empty;
                string EmailId = string.Empty;
                string MobileNo = string.Empty;
                string SumInsured = string.Empty;
                string NomineeDetails = string.Empty;
                string SectionACoverPremium = string.Empty;
                string ExtensionstoSectionASectionBCoverPremium = string.Empty;
                string LoadingsDiscounts = string.Empty;
                string ServiceTax = string.Empty;
                string SwachhBharatCess = string.Empty;
                string KrishiKalyanCess = string.Empty;
                string NetPremiumRoundedOff = string.Empty;
                string StampDuty = string.Empty;
                string Receipt_Challan_No = string.Empty;
                string Receipt_Challan_No_Dated = string.Empty;
                string PolicyIssueDate = string.Empty;
                string TotalAmount = string.Empty;
                bool IsCertificateNumberExists = false;
                //string prod_name = string.Empty;
                //gst changes
                string ugstPercentage = string.Empty;
                string ugstAmount = string.Empty;
                string cgstPercentage = string.Empty;
                string cgstAmount = string.Empty;
                string sgstPercentage = string.Empty;
                string sgstAmount = string.Empty;
                string igstPercentage = string.Empty;
                string igstAmount = string.Empty;
                string totalGSTAmount = string.Empty;
                string vProposerPinCode = string.Empty;
                string addCol1 = string.Empty;
                string polStartDate = string.Empty;
                string createdDate = string.Empty;
                string address1 = string.Empty;
                string address2 = string.Empty;
                string address3 = string.Empty;
                string certNo = string.Empty;
                string UINNo = string.Empty;
                string placeOfSupply = string.Empty;
                string proposalNo = string.Empty;

                GetGPACertificateDetails(ref PolicyIssuingOfficeAddress
   , ref IntermediaryName
   , ref IntermediaryCode
   , ref PolicyholderName
   , ref PolicyholderAddress
   , ref PolicyholderAddress2
   , ref PolicyholderBusinessDescription
   , ref PolicyholderTelephoneNumber
   , ref PolicyholderEmailAddress
   , ref PolicyNumber
   , ref PolicyInceptionDateTime
   , ref PolicyExpiryDateTime
   , ref TotalNumberOfInsuredPersons
   , ref RowCoverHeader
   , ref SectionARow
   , ref ExtSectionARow
   , ref SectionBRow
   , ref NameofInsuredPerson
   , ref DateOfBirth
   , ref Gender
   , ref EmailId
   , ref MobileNo
   , ref SumInsured
   , ref NomineeDetails
   , ref SectionACoverPremium
   , ref ExtensionstoSectionASectionBCoverPremium
   , ref LoadingsDiscounts
   , ref ServiceTax
   , ref SwachhBharatCess
   , ref KrishiKalyanCess
   , ref NetPremiumRoundedOff
   , ref StampDuty
   , ref Receipt_Challan_No
   , ref Receipt_Challan_No_Dated
   , ref PolicyIssueDate
   , ref IntermediaryLandline
   , ref IntermediaryMobile
   , ref TotalAmount
   , ref IsCertificateNumberExists
    // , certNo
    , ref ugstPercentage
       , ref ugstAmount
       , ref cgstPercentage
       , ref cgstAmount
       , ref igstPercentage
       , ref igstAmount
       , ref sgstPercentage
       , ref sgstAmount
       , ref totalGSTAmount
       , ref vProposerPinCode
       , ref addCol1
       , ref polStartDate
       , ref createdDate
       , ref address1
       , ref address2
       , ref address3
       , ref UINNo
         , ref placeOfSupply
          , ref proposalNo);

                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(sw.ToString());

                string strHtml = htmlBody;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + vProposerPinCode + "'";
                        cmd.Connection = con;
                        con.Open();
                        object objCustState = cmd.ExecuteScalar();
                        custStateCode = Convert.ToString(objCustState);
                    }
                }

                strHtml = strHtml.Replace("@PolicyIssuingOfficeAddress", PolicyIssuingOfficeAddress);
                strHtml = strHtml.Replace("@IntermediaryName", IntermediaryName);
                strHtml = strHtml.Replace("@IntermediaryCode", IntermediaryCode);

                strHtml = strHtml.Replace("@IntermediaryLandline", IntermediaryLandline);
                strHtml = strHtml.Replace("@IntermediaryMobile", IntermediaryMobile);

                strHtml = strHtml.Replace("@PolicyholderName", PolicyholderName);
                strHtml = strHtml.Replace("@PolicyholderAddress", PolicyholderAddress);
                string existPolicyholderAddress2 = string.Empty;
                existPolicyholderAddress2 = PolicyholderAddress2.Replace("(stateCode)", "");
                PolicyholderAddress2 = PolicyholderAddress2.Replace("stateCode", custStateCode);
                strHtml = strHtml.Replace("@PolicyholderLine2Address", PolicyholderAddress2);
                strHtml = strHtml.Replace("@PolicyholderBusinessDescription", PolicyholderBusinessDescription);
                strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", PolicyholderTelephoneNumber);
                strHtml = strHtml.Replace("@PolicyholderEmailAddress", PolicyholderEmailAddress);
                //strHtml = strHtml.Replace("@PolicyNumber", PolicyNumber + "/" + certNo); //done changes for cert no

                strHtml = strHtml.Replace("@PolicyNumber", certificateNo);
                strHtml = strHtml.Replace("@PolicyInceptionDateTime", PolicyInceptionDateTime);
                //manish start
                strHtml = strHtml.Replace("@Enroll", PolicyInceptionDateTime.Substring(24));
                //manish end
                strHtml = strHtml.Replace("@PolicyExpiryDateTime", PolicyExpiryDateTime);
                strHtml = strHtml.Replace("@TotalNumberOfInsuredPersons", TotalNumberOfInsuredPersons);

                strHtml = strHtml.Replace("@RowCoverHeader", string.IsNullOrEmpty(RowCoverHeader) ? "" : RowCoverHeader);
                strHtml = strHtml.Replace("@RowSectionA", string.IsNullOrEmpty(SectionARow) ? "" : SectionARow);
                strHtml = strHtml.Replace("@RowExtSectionA", string.IsNullOrEmpty(ExtSectionARow) ? "" : ExtSectionARow);
                strHtml = strHtml.Replace("@RowSectionB", string.IsNullOrEmpty(SectionBRow) ? "" : SectionBRow);

                strHtml = strHtml.Replace("@NameofInsuredPerson", NameofInsuredPerson);
                strHtml = strHtml.Replace("@DateOfBirth", DateOfBirth); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));
                strHtml = strHtml.Replace("@Gender", Gender);
                strHtml = strHtml.Replace("@EmailId", EmailId);
                strHtml = strHtml.Replace("@MobileNo", MobileNo);
                strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@NomineeDetails", NomineeDetails);
                strHtml = strHtml.Replace("@SectionACoverPremium", Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@ExtensionstoSectionASectionBCoverPremium", Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@LoadingsDiscounts", string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@ServiceTax", Convert.ToDecimal(ServiceTax).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@SwachhBharatCess", Convert.ToDecimal(SwachhBharatCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@KrishiKalyanCess", Convert.ToDecimal(KrishiKalyanCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@NetPremiumRoundedOff", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@StampDuty", Convert.ToDecimal(StampDuty).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@Receipt_Challan_No", Receipt_Challan_No);
                strHtml = strHtml.Replace("@Challan_No_Dated", Receipt_Challan_No_Dated);
                strHtml = strHtml.Replace("@PolicyIssueDate", PolicyIssueDate);
                strHtml = strHtml.Replace("@TotalAmount", TotalAmount);

                strHtml = strHtml.Replace("@masterPolicy", PolicyNumber);
                strHtml = strHtml.Replace("@certificateNo", certificateNo);
                strHtml = strHtml.Replace("@createdDate", createdDate);
                strHtml = strHtml.Replace("@customerName", fnGetCustomerName(certificateNo, "GPA"));
                strHtml = strHtml.Replace("@productName", "KOTAK GROUP ACCIDENT CARE");

                strHtml = strHtml.Replace("@addressline1", address1);
                strHtml = strHtml.Replace("@addressline2", address2);
                strHtml = strHtml.Replace("@addressline3", address3);
                strHtml = strHtml.Replace("@statepincode", existPolicyholderAddress2);


                string customString = string.Empty;

                if (!String.IsNullOrEmpty(addCol1))
                {
                    string[] strArr = addCol1.Split(' ');
                    // customString = "this " + strArr[1] + " day of " + strArr[0] + " of " + strArr[2];

                    if (String.IsNullOrEmpty(strArr[1]))
                    {
                        customString = "this " + strArr[2] + " day of " + strArr[0] + " of year " + strArr[3];
                    }
                    else
                    {
                        customString = "this " + strArr[1] + " day of " + strArr[0] + " of year " + strArr[2];
                    }

                }

                strHtml = strHtml.Replace("@polIssueString", customString);

                string igstData = string.Empty;
                string cgstugstData = string.Empty;
                string cgstsgstData = string.Empty;

                if (igstPercentage != "0")
                {
                    string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                    igstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px'><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>IGST@" + igstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='20px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + igstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                }
                else
                {
                    if (cgstPercentage != "0" && ugstPercentage != "0")
                    {
                        string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                        cgstugstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px' ><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>SGST@" + sgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>UGST@" + ugstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='10px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + ugstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + cgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                    }
                    if (cgstPercentage != "0" && sgstPercentage != "0")
                    {
                        string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                        cgstsgstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='10px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px'><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>SGST@" + sgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='10px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + sgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + cgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                    }
                }

                strHtml = strHtml.Replace("@cgstugstData", cgstugstData == "" ? "" : cgstugstData);

                strHtml = strHtml.Replace("@cgstsgstData", cgstsgstData == "" ? "" : cgstsgstData);

                strHtml = strHtml.Replace("@igstData", igstData == "" ? "" : igstData);

                strHtml = strHtml.Replace("@KotakGroupAccidentCareUIN", UINNo == "" ? "" : UINNo);

                //CR_P1_450_Start Kuwar Tax Invoice_GPA_Policy 
                #region TaxInvoiceGPAPolicy

                //GPA_GenerateGPAProtectPDF()
                StringBuilder taxinvoice = new StringBuilder();
                taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                int temp = 0;
                string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                string netPremium = TotalAmount;
                if (netPremium.Contains('.'))
                {
                    temp = Convert.ToInt32(netPremium.Substring(0, netPremium.IndexOf('.')));

                }
                else
                {
                    temp = Convert.ToInt32(netPremium);
                }

                string NetPremiumInWord = ConvertAmountInWord(temp);

                // QR Code
                string suppliGSTN = ConfigurationManager.AppSettings["GstRegNo"].ToString();//hardcord value to be passs
                string kgiStateCode = suppliGSTN.Substring(0, 2);                                                                    // string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                string buyerGSTN = "";
                //string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                string transactionDate = polStartDate;
                int noofHSNCode = 0;
                // string hsnCode = "";
                string hsnCode = ConfigurationManager.AppSettings["SacCode"].ToString();//hardcode value to be pass
                string receiptNumber = Receipt_Challan_No;
                if (hsnCode != "")
                {
                    var tempcount = hsnCode.Split(' ').Length;
                    for (int i = 0; i < tempcount; i++)
                    {
                        noofHSNCode++;
                    }

                }
                string Imagepath = string.Empty;
                CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                string kgiStateName = string.Empty;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                        cmd.Connection = con;
                        con.Open();
                        object objStaeName = cmd.ExecuteScalar();
                        kgiStateName = Convert.ToString(objStaeName);
                    }
                }
                Imagepath = Imagepath == "error" ? "" : Imagepath;
                strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);


                strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                //GPA Policy
                strHtml = strHtml.Replace("@gistinno", "");
                strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                strHtml = strHtml.Replace("@customername", PolicyholderName);
                strHtml = strHtml.Replace("@emailId", EmailId);
                strHtml = strHtml.Replace("@contactno", MobileNo);
                strHtml = strHtml.Replace("@address", PolicyholderAddress);
                strHtml = strHtml.Replace("@address1", PolicyholderAddress2);
                // strHtml = strHtml.Replace("@address2", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());// add 3 address
                strHtml = strHtml.Replace("@imdcode", IntermediaryCode);
                strHtml = strHtml.Replace("@receiptno", Receipt_Challan_No);
                strHtml = strHtml.Replace("@customerstatecode", custStateCode);
                //strHtml = strHtml.Replace("@customerstatecode", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst statecode of customer require
                strHtml = strHtml.Replace("@supplyname", placeOfSupply);//gst state name require of customer

                //strHtml = strHtml.Replace("@gistinno", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                // strHtml = strHtml.Replace("@KotakGstNo", ConfigurationManager.AppSettings["GstRegNo"].ToString());//hardcore in html
                strHtml = strHtml.Replace("@name", kgiName);
                strHtml = strHtml.Replace("@panNo", kgiPanno);
                strHtml = strHtml.Replace("@cinNo", kgiCINno);


                //  strHtml = strHtml.Replace("@vKGIBranchAddress", ConfigurationManager.AppSettings["AddressWithLine"].ToString());//not found
                strHtml = strHtml.Replace("@invoicedate", polStartDate);
                strHtml = strHtml.Replace("@invoiceno", certificateNo);
                strHtml = strHtml.Replace("@proposalno", "");
                // strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["add_col_1"].ToString()); // not present in the SP
                strHtml = strHtml.Replace("@partnerappno", "");// this column is there as per jay
                                                               //strHtml = strHtml.Replace("@irn", certNo);
                strHtml = strHtml.Replace("@kgistatecode", kgiStateCode);//gst state code of kotak uncomment 
                strHtml = strHtml.Replace("@kgistatename", kgiStateName);//gst state code of kotak uncommentuncomment
                strHtml = strHtml.Replace("@irn", certificateNo);

                //GPA Policy
                strHtml = strHtml.Replace("@totalpremium", TotalAmount);
                strHtml = strHtml.Replace("@netamount", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@NetPremiumString", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                strHtml = strHtml.Replace("@totalgst", totalGSTAmount);

                strHtml = strHtml.Replace("@cgstpercent", cgstPercentage);
                strHtml = strHtml.Replace("@ugstpercent", ugstPercentage);
                strHtml = strHtml.Replace("@sgstpercent", sgstPercentage);
                strHtml = strHtml.Replace("@igstpercent", igstPercentage);

                //GPA Policy
                strHtml = strHtml.Replace("@cgstamt", cgstAmount);
                strHtml = strHtml.Replace("@ugstamt", ugstAmount);
                strHtml = strHtml.Replace("@sgstamt", sgstAmount);
                strHtml = strHtml.Replace("@igstamt", igstAmount);

                strHtml = strHtml.Replace("@cessrate", "0");
                strHtml = strHtml.Replace("@cessamt", SwachhBharatCess);

                string tdservicetax = string.Empty;
                string dataservicetax = string.Empty;
                if (ServiceTax != "0" && ServiceTax != "")
                {
                    tdservicetax = "<td style='border: 1px solid black' width='5%'><p style ='font-size:small'><strong>Service Tax</strong></p></td> ";
                    dataservicetax = "<td style ='border:1px solid black' width = '5%'><p> " + Convert.ToDecimal(ServiceTax).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td>";


                }
                strHtml = strHtml.Replace("@servicetx", tdservicetax == "" ? "" : tdservicetax);
                strHtml = strHtml.Replace("@servictaxh", dataservicetax == "" ? "" : dataservicetax);


                strHtml = strHtml.Replace("@totalgross", TotalAmount);// change1
                strHtml = strHtml.Replace("@totalinvoicevalueinfigure", TotalAmount);
                strHtml = strHtml.Replace("@totalinvoicevalueinwords", NetPremiumInWord.ToString());
                #endregion
                //CR_450_End_Kuwar_Tax_Invoice GPA Policy

                TextWriter outTextWriter = new StringWriter();
                HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                //base.Render(outHtmlTextWriter);

                string currentPageHtmlString = strHtml; //outTextWriter.ToString();

                // Create a HTML to PDF converter object with default settings
                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                // Set license key received after purchase to use the converter in licensed mode
                // Leave it not set to use the converter in demo mode
                string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();

                htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

                // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                htmlToPdfConverter.ConversionDelay = 2;

                // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                // Add Header

                // Enable header in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                // Optionally add a space between header and the page body
                // The spacing for first page and the subsequent pages can be set independently
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");

                // Draw header elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                    DrawHeader(htmlToPdfConverter, false);

                // Add Footer

                // Enable footer in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                // Optionally add a space between footer and the page body
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                // Draw footer elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                    DrawFooter(htmlToPdfConverter, false, true);

                // Use the current page URL as base URL
                string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;


                ////// Convert the current page HTML string to a PDF document in a memory buffer
                //// For Live
                byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                //// For Live End Here 


                // For Dev
                //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                //byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                // For Dev End here 

                Response.AddHeader("Content-Type", "application/pdf");
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNo));
                Response.BinaryWrite(outPdfBuffer);
                CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadPolicyScheduleNew.aspx");//Added By Rajesh Soni on 20/02/2020
                Response.End();

                #endregion
            }

            else if (productName.Contains("Smart Cash") || productName.Contains("Micro Insurance"))
            {
                fnGenerateHDCSchedule(certificateNo);
               
            }
            else
            {
                DownloadCertificateFromGIST(emailId, CustomerName, certificateNo, productcode, MobileNumber);
            }

        }
        #endregion


        private void fnGenerateGPA_Protect_Schedule(string certificateNo)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            try
            {
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();
                    string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_CompleteLetter_With_GST - Copy.html";
                    string htmlBody = File.ReadAllText(strPath);
                    StringWriter sw = new StringWriter();
                    StringReader sr = new StringReader(sw.ToString());
                    string strHtml = htmlBody;

                    SqlCommand command = new SqlCommand("PROC_GET_COVER_SECTION_DATA_FOR_PDF_TEST", con);
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                    command.Parameters.AddWithValue("@vCertificateNo", certificateNo);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            //  GenerateEmailPDF(con, ds, strHtml, certNo, emailID);

                            string accidentalDeath = string.Empty;
                            string permTotalDisable = string.Empty;
                            string permPartialDisable = string.Empty;
                            string tempTotalDisable = string.Empty;
                            string carraigeBody = string.Empty;
                            string funeralExpense = string.Empty;
                            string medicalExpense = string.Empty;
                            string purchaseBlood = string.Empty;
                            string transportation = string.Empty;
                            string compassionate = string.Empty;
                            string disappearance = string.Empty;
                            string modifyResidence = string.Empty;
                            string costOfSupport = string.Empty;
                            string commonCarrier = string.Empty;
                            string childrenGrant = string.Empty;
                            string marraigeExpense = string.Empty;
                            string sportsActivity = string.Empty;
                            string widowHood = string.Empty;

                            string ambulanceChargesString = string.Empty;
                            string dailyCashString = string.Empty;
                            string accidentalHospString = string.Empty;
                            string opdString = string.Empty;
                            string accidentalDentalString = string.Empty;
                            string convalString = string.Empty;
                            string burnsString = string.Empty;
                            string brokenBones = string.Empty;
                            string comaString = string.Empty;
                            string domesticTravelString = string.Empty;
                            string lossofEmployString = string.Empty;
                            string onDutyCover = string.Empty;
                            string legalExpenses = string.Empty;

                            string reducingCoverString = string.Empty;
                            string assignmentString = string.Empty;

                            //gst
                            string custStateCode = string.Empty;
                            string igstString = string.Empty;
                            string cgstsgstString = string.Empty;
                            string cgstugstString = string.Empty;

                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "'";
                                cmd.Connection = con;
                                object objCustState = cmd.ExecuteScalar();
                                custStateCode = Convert.ToString(objCustState);
                            }

                            strHtml = strHtml.Replace("@createdDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dCreatedDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@customerName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeAge"].ToString());
                            strHtml = strHtml.Replace("@masterDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["vMasterPolicyDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());


                            strHtml = strHtml.Replace("@addressline1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@addressline2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());

                            strHtml = strHtml.Replace("@addressline3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                            strHtml = strHtml.Replace("@city", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                            strHtml = strHtml.Replace("@pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());
                            strHtml = strHtml.Replace("@state", ds.Tables[0].Rows[0]["vProposerState"].ToString());

                            strHtml = strHtml.Replace("@mobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@emailID", ds.Tables[0].Rows[0]["vEmailId"].ToString());

                            strHtml = strHtml.Replace("@productName", "KOTAK GROUP ACCIDENT PROTECT"); //done changes for cert no
                            strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vpolicyType"].ToString() == "" ? "New" : ds.Tables[0].Rows[0]["vpolicyType"].ToString());


                            strHtml = strHtml.Replace("@prevPolicyNo", ds.Tables[0].Rows[0]["vprevPolicyNumber"].ToString());
                            strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyLoc"].ToString());


                            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderAddress", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString()); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@PolicyholderLine2Address", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerCity"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerState"].ToString() + "(" + custStateCode + ")-" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                            strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderEmailAddress", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                            //  strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@policyStartDate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@policyEndDate", ds.Tables[0].Rows[0]["dPolicyEndDate"].ToString());

                            strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                            strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());


                            strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                            strHtml = strHtml.Replace("@occupation", ds.Tables[0].Rows[0]["vOccupation"].ToString());

                            strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                            strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["dCustomerDob"].ToString());
                            strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                            strHtml = strHtml.Replace("@category", "");
                            strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                            strHtml = strHtml.Replace("@sumInsured", "");


                            strHtml = strHtml.Replace("@sumBasis", ds.Tables[0].Rows[0]["vSIBasis"].ToString());

                            strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                            strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());

                            strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                            strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vAppointeeName"].ToString());

                            string igstPercentage = ds.Tables[0].Rows[0]["igstPercentage"].ToString();
                            string cgstPercentage = ds.Tables[0].Rows[0]["cgstPercentage"].ToString();
                            string sgstPercentage = ds.Tables[0].Rows[0]["sgstPercentage"].ToString();
                            string ugstPercentage = ds.Tables[0].Rows[0]["ugstPercentage"].ToString();
                            string igstAmount = ds.Tables[0].Rows[0]["igstAmount"].ToString();
                            string cgstAmount = ds.Tables[0].Rows[0]["cgstAmount"].ToString();
                            string sgstAmount = ds.Tables[0].Rows[0]["sgstAmount"].ToString();
                            string ugstAmount = ds.Tables[0].Rows[0]["ugstAmount"].ToString();

                            if (igstPercentage != "0")
                            {
                                igstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center'>IGST@" + igstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'>" + igstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && sgstPercentage != "0")
                            {
                                cgstsgstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center'>SGST@" + sgstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'>" + sgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && ugstPercentage != "0")
                            {
                                cgstugstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center'>UGST@" + ugstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'>" + ugstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            strHtml = strHtml.Replace("@igstString", igstString == "" ? "" : igstString);
                            strHtml = strHtml.Replace("@cgstsgstString", cgstsgstString == "" ? "" : cgstsgstString);
                            strHtml = strHtml.Replace("@cgstugstString", cgstugstString == "" ? "" : cgstugstString);

                            string policyIssuance = ds.Tables[0].Rows[0]["vAdditional_column_1"].ToString();
                            string customString = string.Empty;

                            if (!String.IsNullOrEmpty(policyIssuance))
                            {
                                string[] strArr = policyIssuance.Split(' ');
                                if (String.IsNullOrEmpty(strArr[1]))
                                {
                                    customString = "this " + strArr[2] + " day of " + strArr[0] + " of year " + strArr[3];
                                }
                                else
                                {
                                    customString = "this " + strArr[1] + " day of " + strArr[0] + " of year " + strArr[2];
                                }

                            }

                            strHtml = strHtml.Replace("@polIssueString", customString);



                            if (ds.Tables[0].Rows[0]["vAccidentalDeathAD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString().Trim()))
                                {
                                    //accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString() + ")</p></td></tr>";

                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vPermTotalDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString().Trim()))
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString() + ")</p></td></tr> ";
                                }
                                else
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString())) + "</p></td></tr> ";
                                }
                            }
                            //
                            if (ds.Tables[0].Rows[0]["vPermPartialDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString().Trim()))
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vTempTotalDisableTTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString().Trim()))
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCarraigeDeadBody"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString().Trim()))
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vFuneralExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString().Trim()))
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalMedicalExp"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString().Trim()))
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vPurchaseofBlood"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPurchaseofBloodSIText"].ToString().Trim()))
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPurchaseofBloodSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vTransportationofImpMedicine"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTransportationofImpMedicineSIText"].ToString().Trim()))
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vTransportationofImpMedicineSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCompassionateVisit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString().Trim()))
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDisappearanceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString().Trim()))
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString() + "</p></td></tr>";
                                }

                            }

                            if (ds.Tables[0].Rows[0]["vModificationofVehicle"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString().Trim()))
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCostSupportItems"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString().Trim()))
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCommonCarrier"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString().Trim()))
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vChildEduGrant"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString().Trim()))
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vMarraigeExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString().Trim()))
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSportsActivityCover"].ToString() == "Y")
                            {
                                sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vWidowhoodCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString().Trim()))
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAmbulanceCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString().Trim()))
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vDailyCashBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString().Trim()))
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalHospitalization"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString().Trim()))
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOPDTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString().Trim()))
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalDentalExpense"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString().Trim()))
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vConvalescenceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString().Trim()))
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBurnBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString().Trim()))
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBrokenBones"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString().Trim()))
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vComa"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vComaSIText"].ToString().Trim()))
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nComaSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vComaSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nComaSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDomesticTravelForTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString().Trim()))
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString().Trim()))
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOnDutyCover"].ToString() == "Y")
                            {
                                onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vLegalExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString().Trim()))
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSIBasis"].ToString().ToLower() == "reducing")
                            {
                                reducingCoverString = "<tr><td style='border:1px solid black' width='10%'><p  style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>8.23-Reducing Sum Insured Covers</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vProposalType"].ToString().ToLower() == "credit linked")
                            {
                                assignmentString = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>8.22-Assignment</p></td></tr>";
                            }



                            strHtml = strHtml.Replace("@premium", Convert.ToDecimal(ds.Tables[0].Rows[0]["nNetPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            strHtml = strHtml.Replace("@serviceTax", Convert.ToDecimal(ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@amount", Convert.ToDecimal(ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                            strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryNumber"].ToString());
                            strHtml = strHtml.Replace("@intermediaryLandline", "");
                            strHtml = strHtml.Replace("@challanDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dChallanDate"]).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            //strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["nStampDuty"].ToString());
                            strHtml = strHtml.Replace("@stampduty", Convert.ToDecimal(ds.Tables[0].Rows[0]["nStampDuty"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                            {
                                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash – Micro Insurance UIN:KOTHMGP076V011819");
                            }
                            else
                            {
                                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash UIN: KOTHLGP19014V011819");
                            }

                            strHtml = strHtml.Replace("@accidentalDeathString", accidentalDeath == "" ? "" : accidentalDeath);
                            strHtml = strHtml.Replace("@permTotalDisableString", permTotalDisable == "" ? "" : permTotalDisable);
                            strHtml = strHtml.Replace("@permTotalPartialString", permPartialDisable == "" ? "" : permPartialDisable);
                            strHtml = strHtml.Replace("@tempTotalDisableString", tempTotalDisable == "" ? "" : tempTotalDisable);
                            strHtml = strHtml.Replace("@carraigeBodyString", carraigeBody == "" ? "" : carraigeBody);
                            strHtml = strHtml.Replace("@funeralExpenseString", funeralExpense == "" ? "" : funeralExpense);
                            strHtml = strHtml.Replace("@medicalExpenseString", medicalExpense == "" ? "" : medicalExpense);
                            strHtml = strHtml.Replace("@purchasebloodString", purchaseBlood == "" ? "" : purchaseBlood);
                            strHtml = strHtml.Replace("@transportationString", transportation == "" ? "" : transportation);
                            strHtml = strHtml.Replace("@compassionateString", compassionate == "" ? "" : compassionate);
                            strHtml = strHtml.Replace("@disappearanceString", disappearance == "" ? "" : disappearance);
                            strHtml = strHtml.Replace("@modifyResidenceString", modifyResidence == "" ? "" : modifyResidence);
                            strHtml = strHtml.Replace("@costOfSupportString", costOfSupport == "" ? "" : costOfSupport);
                            strHtml = strHtml.Replace("@commonCarrierString", commonCarrier == "" ? "" : commonCarrier);
                            strHtml = strHtml.Replace("@childrenGrantString", childrenGrant == "" ? "" : childrenGrant);
                            strHtml = strHtml.Replace("@marraigeExpenseString", marraigeExpense == "" ? "" : marraigeExpense);
                            strHtml = strHtml.Replace("@sportsActivityString", sportsActivity == "" ? "" : sportsActivity);
                            strHtml = strHtml.Replace("@widowHoodString", widowHood == "" ? "" : widowHood);
                            strHtml = strHtml.Replace("@ambulanceChargesString", ambulanceChargesString == "" ? "" : ambulanceChargesString);
                            strHtml = strHtml.Replace("@dailyCashString", dailyCashString == "" ? "" : dailyCashString);
                            strHtml = strHtml.Replace("@accidentalHospString", accidentalHospString == "" ? "" : accidentalHospString);
                            strHtml = strHtml.Replace("@convalString", convalString == "" ? "" : convalString);
                            strHtml = strHtml.Replace("@burnsString", burnsString == "" ? "" : burnsString);
                            strHtml = strHtml.Replace("@brokenBones", brokenBones == "" ? "" : brokenBones);
                            strHtml = strHtml.Replace("@comaString", comaString == "" ? "" : comaString);
                            strHtml = strHtml.Replace("@domesticTravelString", domesticTravelString == "" ? "" : domesticTravelString);
                            strHtml = strHtml.Replace("@lossofEmployString", lossofEmployString == "" ? "" : lossofEmployString);
                            strHtml = strHtml.Replace("@onDutyCover", onDutyCover == "" ? "" : onDutyCover);
                            strHtml = strHtml.Replace("@legalExpenses", legalExpenses == "" ? "" : legalExpenses);
                            strHtml = strHtml.Replace("@reducingCoverString", reducingCoverString == "" ? "" : reducingCoverString);
                            strHtml = strHtml.Replace("@assignmentString", assignmentString == "" ? "" : assignmentString);
                            strHtml = strHtml.Replace("@accidentalDentalString", accidentalDentalString == "" ? "" : accidentalDentalString);
                            strHtml = strHtml.Replace("@opdString", opdString == "" ? "" : opdString);
                            // Get the current page HTML string by rendering into a TextWriter object

                            #region HDCRISKFORPROTECT
                            string _Date1 = ds.Tables[0].Rows[0]["dAccountDebitDate"].ToString();
                           // DateTime dtDateHDCRisk = Convert.ToDateTime(_Date1);
                            DateTime dtDateHDCRisk = DateTime.ParseExact(_Date1, "dd/MM/yyyy", null);

                            string TransactionDateHDCRisk = dtDateHDCRisk.ToString("dd/MM/yyyy");
                            strHtml = strHtml.Replace("@TransactionDateHDCRisk", TransactionDateHDCRisk);

                            string mentionedGender = ds.Tables[0].Rows[0]["vCustomerGender"].ToString();
                            if (string.IsNullOrEmpty(mentionedGender))
                            {
                                strHtml = strHtml.Replace("@salutation", "");
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "M" || ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "Male")
                                {
                                    strHtml = strHtml.Replace("@salutation", "Mr.");
                                }
                                else if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "F")
                                {
                                    strHtml = strHtml.Replace("@salutation", "Mrs.");
                                }
                                else
                                {
                                    strHtml = strHtml.Replace("@salutation", "");
                                }
                            }
                            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@addressofinsured1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@addressofinsured2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                            strHtml = strHtml.Replace("@addressofinsured3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                            strHtml = strHtml.Replace("@addressofinsuredCity", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                            strHtml = strHtml.Replace("@addressofinsuredState", ds.Tables[0].Rows[0]["vProposerState"].ToString());
                            strHtml = strHtml.Replace("@Pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                            strHtml = strHtml.Replace("@mobileno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());

                            strHtml = strHtml.Replace("@productname", ds.Tables[0].Rows[0]["vProductName"].ToString());
                            #endregion

                            strHtml = strHtml.Replace("@KotakGroupAccidentProtectUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]));

                            //CR_P1_450_Start Kuwar Tax Invoice_GPA_Policy 
                            #region TaxInvoiceGPAPolicy

                            //GPA_GenerateGPAProtectPDF()
                            StringBuilder taxinvoice = new StringBuilder();
                            taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                            int temp = 0;
                            string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                            string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                            string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                            string totalPremium = ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString();
                            if (totalPremium.Contains('.'))
                            {
                                temp = Convert.ToInt32(totalPremium.Substring(0, totalPremium.IndexOf('.')));

                            }
                            else
                            {
                                temp = Convert.ToInt32(totalPremium);
                            }

                            string totalPremiumInWord = ConvertAmountInWord(temp);

                            // QR Code
                            string suppliGSTN = ConfigurationManager.AppSettings["GstRegNo"].ToString();
                            string kgiStateCode = suppliGSTN.Substring(0, 2);
                            // string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                            string buyerGSTN = "";
                            //string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                            string transactionDate = ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString();
                            int noofHSNCode = 0;
                            // string hsnCode = "";
                            string hsnCode = ConfigurationManager.AppSettings["SacCode"].ToString();
                            string receiptNumber = ds.Tables[0].Rows[0]["vChallanNumber"].ToString();
                            if (hsnCode != "")
                            {
                                var tempcount = hsnCode.Split(' ').Length;
                                for (int i = 0; i < tempcount; i++)
                                {
                                    noofHSNCode++;
                                }

                            }
                            string Imagepath = string.Empty;
                            CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                            Imagepath = Imagepath == "error" ? "" : Imagepath;
                            string kgiStateName = string.Empty;
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                                cmd.Connection = con;
                                //sqlCon.Open();
                                object objStaeName = cmd.ExecuteScalar();
                                kgiStateName = Convert.ToString(objStaeName);
                            }
                            strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);


                            strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@gistinno", "");
                            strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                            strHtml = strHtml.Replace("@customername", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@emailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                            strHtml = strHtml.Replace("@contactno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@address", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@address1", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                            strHtml = strHtml.Replace("@address2", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());// add 3 address
                            strHtml = strHtml.Replace("@imdcode", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@receiptno", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            strHtml = strHtml.Replace("@customerstatecode", custStateCode);
                            //strHtml = strHtml.Replace("@customerstatecode", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst statecode of customer require
                            strHtml = strHtml.Replace("@supplyname", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst state name require of customer
                                                                                                                        //GPA Policy PROC_GET_COVER_SECTION_DATA_FOR_PDF_TEST proc

                            strHtml = strHtml.Replace("@KotakGstNo", ConfigurationManager.AppSettings["GstRegNo"].ToString());//not found
                            strHtml = strHtml.Replace("@name", kgiName);
                            strHtml = strHtml.Replace("@panNo", kgiPanno);
                            strHtml = strHtml.Replace("@cinNo", kgiCINno);
                            strHtml = strHtml.Replace("@kgistatecode", kgiStateCode);//gst state code of kotak uncomment 
                            strHtml = strHtml.Replace("@kgistatename", kgiStateName);//gst state code of kotak uncommentuncomment
                            strHtml = strHtml.Replace("@invoicedate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["vAdditional_column_4"].ToString());
                            strHtml = strHtml.Replace("@partnerappno", "");
                            strHtml = strHtml.Replace("@irn", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            //GPA Policy PROC_GET_COVER_SECTION_DATA_FOR_PDF_TEST proc

                            strHtml = strHtml.Replace("@totalpremium", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@netamount", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@totalgst", ds.Tables[0].Rows[0]["TotalGSTAmount"].ToString());

                            strHtml = strHtml.Replace("@cgstpercent", ds.Tables[0].Rows[0]["cgstPercentage"].ToString());
                            strHtml = strHtml.Replace("@ugstpercent", ds.Tables[0].Rows[0]["ugstPercentage"].ToString());
                            strHtml = strHtml.Replace("@sgstpercent", ds.Tables[0].Rows[0]["sgstPercentage"].ToString());
                            strHtml = strHtml.Replace("@igstpercent", ds.Tables[0].Rows[0]["igstPercentage"].ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@cgstamt", ds.Tables[0].Rows[0]["cgstAmount"].ToString());
                            strHtml = strHtml.Replace("@ugstamt", ds.Tables[0].Rows[0]["ugstAmount"].ToString());
                            strHtml = strHtml.Replace("@sgstamt", ds.Tables[0].Rows[0]["sgstAmount"].ToString());
                            strHtml = strHtml.Replace("@igstamt", ds.Tables[0].Rows[0]["igstAmount"].ToString());
                            strHtml = strHtml.Replace("@cessrate", "0");
                            strHtml = strHtml.Replace("@cessamt", ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString());

                            string tdservicetax = string.Empty;
                            string dataservicetax = string.Empty;
                            string tem = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["nServiceTax"].ToString()) ? "0" : ds.Tables[0].Rows[0]["nServiceTax"].ToString() == "null" ? null : ds.Tables[0].Rows[0]["nServiceTax"].ToString();

                            //if (ds.Tables[0].Rows[0]["nServiceTax"].ToString()!="0" && (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["nServiceTax"].ToString())))
                            if (tem != "0" && !string.IsNullOrEmpty(tem))
                            {
                                tdservicetax = "<td style='border: 1px solid black' width='5%'><p style ='font-size:small'><strong>Service Tax</strong></p></td> ";
                                dataservicetax = "<td style ='border:1px solid black' width = '5%'><p> " + Convert.ToDecimal(ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td>";
                            }
                            strHtml = strHtml.Replace("@servictaxh", dataservicetax == "" ? "" : dataservicetax);
                            strHtml = strHtml.Replace("@servicetx", tdservicetax == "" ? "" : tdservicetax);


                            strHtml = strHtml.Replace("@totalgross", totalPremium);// change1
                            strHtml = strHtml.Replace("@totalinvoicevalueinfigure", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@totalinvoicevalueinwords", totalPremiumInWord.ToString());
                            #endregion
                            //CR_450_End_Kuwar_Tax_Invoice GPA Policy

                            TextWriter outTextWriter = new StringWriter();
                            HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);

                            string currentPageHtmlString = strHtml;

                            // Create a HTML to PDF converter object with default settings
                            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                            // Set license key received after purchase to use the converter in licensed mode
                            // Leave it not set to use the converter in demo mode
                            string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();

                            htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

                            // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                            // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                            htmlToPdfConverter.ConversionDelay = 2;

                            // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                            htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                            // Add Header

                            // Enable header in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                            // Optionally add a space between header and the page body
                            // The spacing for first page and the subsequent pages can be set independently
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                            htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");

                            // Draw header elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                                DrawHeader(htmlToPdfConverter, false);

                            // Add Footer

                            // Enable footer in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                            // Optionally add a space between footer and the page body
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                            // Draw footer elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                                DrawFooter(htmlToPdfConverter, false, true);

                            // Use the current page URL as base URL
                            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;

                            //// For Live
                            byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                            byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                            //// For Live End Here 

                            //// For Dev
                            //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                            //// byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                            //// For Dev End here 


                            Response.AddHeader("Content-Type", "application/pdf");
                            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNo));
                            Response.BinaryWrite(outPdfBuffer);
                            CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadPolicyScheduleNew.aspx");
                            Response.End();


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, "GenerateGPAProtectPDF ::Error occured  Certificate number :" + certificateNo
                    + "   " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while gereating GPA Protect Schedule for download");
            }
        }






        private void GetGPACertificateDetails(
              ref string PolicyIssuingOfficeAddress
            , ref string IntermediaryName
            , ref string IntermediaryCode
            , ref string PolicyholderName
            , ref string PolicyholderAddress
            , ref string PolicyholderAddress2
            , ref string PolicyholderBusinessDescription
            , ref string PolicyholderTelephoneNumber
            , ref string PolicyholderEmailAddress
            , ref string PolicyNumber
            , ref string PolicyInceptionDateTime
            , ref string PolicyExpiryDateTime
            , ref string TotalNumberOfInsuredPersons
            , ref string RowCoverHeader
            , ref string SectionARow
            , ref string ExtSectionARow
            , ref string SectionBRow
            , ref string NameofInsuredPerson
            , ref string DateOfBirth
            , ref string Gender
            , ref string EmailId
            , ref string MobileNo
            , ref string SumInsured
            , ref string NomineeDetails
            , ref string SectionACoverPremium
            , ref string ExtensionstoSectionASectionBCoverPremium
            , ref string LoadingsDiscounts
            , ref string ServiceTax
            , ref string SwachhBharatCess
            , ref string KrishiKalyanCess
            , ref string NetPremiumRoundedOff
            , ref string StampDuty
            , ref string Receipt_Challan_No
            , ref string Receipt_Challan_No_Dated
            , ref string PolicyIssueDate
            , ref string IntermediaryLandline
            , ref string IntermediaryMobile
            , ref string TotalAmount
            , ref bool IsCertificateNumberExists
            , ref string ugstPercentage
            , ref string ugstAmount
            , ref string cgstPercentage
            , ref string cgstAmount
            , ref string igstPercentage
            , ref string igstAmount
            , ref string sgstPercentage
            , ref string sgstAmount
            , ref string totalgstAmount
            , ref string vProposerPinCode
            , ref string addCol1
            , ref string polStartDate
             , ref string createdDate
            , ref string address1
            , ref string address2
            , ref string address3
            , ref string UINNo
           , ref string placeOfSupply
            , ref string proposalNo)
        {
            string strSectionACoverName = string.Empty; string strSectionACoverSI = string.Empty;
            string strExtSectionACoverName = string.Empty; string strExtSectionACoverSI = string.Empty;
            string strSectionBCoverName = string.Empty; string strSectionBCoverSI = string.Empty;
            string trCoverHeader = string.Empty;
            string trSectionARow = string.Empty;
            string trExtSectionARow = string.Empty;
            string trSectionBRow = string.Empty;
            //IsCertificateNumberExists = false;
            GetCoverSectionDetails(ref trCoverHeader, ref trSectionARow, ref trExtSectionARow, ref trSectionBRow);

            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_GPA_CERTIFICATE_DETAILS";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            db.AddParameter(dbCommand, "vCertificateNo", DbType.String, ParameterDirection.Input, "vCertificateNo", DataRowVersion.Current, certificateNo);
            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsCertificateNumberExists = true;
                    //lblSeatingCapacityt.Text = ds.Tables[0].Rows[0][0].ToString();

                    PolicyIssuingOfficeAddress = ds.Tables[0].Rows[0]["PolicyIssuingOfficeAddress"].ToString();
                    IntermediaryName = ds.Tables[0].Rows[0]["IntermediaryName"].ToString();
                    IntermediaryCode = ds.Tables[0].Rows[0]["IntermediaryCode"].ToString();

                    IntermediaryLandline = ds.Tables[0].Rows[0]["IntermediaryLandline"].ToString();
                    IntermediaryMobile = ds.Tables[0].Rows[0]["IntermediaryMobile"].ToString();

                    PolicyholderName = ds.Tables[0].Rows[0]["PolicyholderName"].ToString();
                    PolicyholderAddress = ds.Tables[0].Rows[0]["PolicyholderAddress"].ToString();
                    PolicyholderAddress2 = ds.Tables[0].Rows[0]["PolicyholderAddress2"].ToString();
                    PolicyholderBusinessDescription = ds.Tables[0].Rows[0]["PolicyholderBusinessDescription"].ToString();
                    PolicyholderTelephoneNumber = ds.Tables[0].Rows[0]["PolicyholderTelephoneNumber"].ToString();
                    PolicyholderEmailAddress = ds.Tables[0].Rows[0]["PolicyholderEmailAddress"].ToString();
                    PolicyNumber = ds.Tables[0].Rows[0]["PolicyNumber"].ToString();
                    PolicyInceptionDateTime = ds.Tables[0].Rows[0]["PolicyInceptionDateTime"].ToString();
                    PolicyExpiryDateTime = ds.Tables[0].Rows[0]["PolicyExpiryDateTime"].ToString();
                    TotalNumberOfInsuredPersons = ds.Tables[0].Rows[0]["TotalNumberOfInsuredPersons"].ToString();
                    RowCoverHeader = trCoverHeader;
                    SectionARow = trSectionARow;
                    ExtSectionARow = trExtSectionARow;
                    SectionBRow = trSectionBRow;
                    NameofInsuredPerson = ds.Tables[0].Rows[0]["NameofInsuredPerson"].ToString();
                    DateOfBirth = ds.Tables[0].Rows[0]["DateOfBirth"].ToString();
                    Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                    EmailId = ds.Tables[0].Rows[0]["EmailId"].ToString();
                    MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                    SumInsured = ds.Tables[0].Rows[0]["SumInsured"].ToString();
                    NomineeDetails = ds.Tables[0].Rows[0]["NomineeDetails"].ToString();
                    SectionACoverPremium = ds.Tables[0].Rows[0]["SectionACoverPremium"].ToString();
                    ExtensionstoSectionASectionBCoverPremium = ds.Tables[0].Rows[0]["ExtensionstoSectionASectionBCoverPremium"].ToString();
                    LoadingsDiscounts = ds.Tables[0].Rows[0]["LoadingsDiscounts"].ToString();
                    ServiceTax = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                    SwachhBharatCess = ds.Tables[0].Rows[0]["SwachhBharatCess"].ToString();
                    KrishiKalyanCess = ds.Tables[0].Rows[0]["KrishiKalyanCess"].ToString();
                    NetPremiumRoundedOff = ds.Tables[0].Rows[0]["NetPremiumRoundedOff"].ToString();
                    StampDuty = ds.Tables[0].Rows[0]["StampDuty"].ToString();
                    Receipt_Challan_No = ds.Tables[0].Rows[0]["Receipt_Challan_No"].ToString();
                    Receipt_Challan_No_Dated = ds.Tables[0].Rows[0]["Receipt_Challan_No_Dated"].ToString();
                    PolicyIssueDate = ds.Tables[0].Rows[0]["PolicyIssueDate"].ToString();
                    TotalAmount = ds.Tables[0].Rows[0]["TotalAmount"].ToString();
                    ugstPercentage = ds.Tables[0].Rows[0]["ugstPercentage"].ToString();
                    ugstAmount = ds.Tables[0].Rows[0]["ugstAmount"].ToString();
                    cgstPercentage = ds.Tables[0].Rows[0]["cgstPercentage"].ToString();
                    cgstAmount = ds.Tables[0].Rows[0]["cgstAmount"].ToString();
                    sgstPercentage = ds.Tables[0].Rows[0]["sgstPercentage"].ToString();
                    sgstAmount = ds.Tables[0].Rows[0]["sgstAmount"].ToString();
                    igstPercentage = ds.Tables[0].Rows[0]["igstPercentage"].ToString();
                    igstAmount = ds.Tables[0].Rows[0]["igstAmount"].ToString();
                    totalgstAmount = ds.Tables[0].Rows[0]["totalGSTAmount"].ToString();
                    vProposerPinCode = ds.Tables[0].Rows[0]["vProposerPinCode"].ToString();
                    addCol1 = ds.Tables[0].Rows[0]["addCol1"].ToString();
                    polStartDate = ds.Tables[0].Rows[0]["polStartDate"].ToString();
                    createdDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["createdDate"]).ToString("dd-MMM-yyyy");
                    address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                    address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                    address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                    UINNo = ds.Tables[0].Rows[0]["UINNo"].ToString();
                    // CR_450_Kuwar
                    placeOfSupply = ds.Tables[0].Rows[0]["PlaceOfSupply"].ToString();
                    proposalNo = ds.Tables[0].Rows[0]["Additional_column_4"].ToString();
                    // CR_450_Kuwar
                }
            }
        }

        private void GetCoverSectionDetails(ref string trRowCoverHeader, ref string trSectionARow, ref string trExtSectionARow, ref string trSectionBRow)
        {
            bool IsCoverAvailable = false;
            string strSectionACoverName = string.Empty; string strSectionACoverSI = string.Empty; string strSectionACoverSIText = string.Empty;
            string strExtSectionACoverName = string.Empty; string strExtSectionACoverSI = string.Empty; string strExtSectionACoverSIText = string.Empty;
            string strSectionBCoverName = string.Empty; string strSectionBCoverSI = string.Empty; string strSectionBCoverSIText = string.Empty;

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand = "PROC_GET_COVER_SECTION_DATA_FOR_PDF";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            db.AddParameter(dbCommand, "vCertificateNo", DbType.String, ParameterDirection.Input, "vCertificateNo", DataRowVersion.Current, certificateNo);
            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            strSectionACoverSIText = string.IsNullOrEmpty(dr["COVER_SI_TEXT"].ToString().Trim()) ? "" : "(" + dr["COVER_SI_TEXT"].ToString() + ")";
                            if (strSectionACoverName.Length == 0) //if first loop then no br tag else for line break br is added
                            {
                                strSectionACoverName = dr["NAME_OF_BENEFIT"].ToString();
                                strSectionACoverSI = Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionACoverSIText;
                            }
                            else
                            {
                                strSectionACoverName = strSectionACoverName + "<br>" + dr["NAME_OF_BENEFIT"].ToString();
                                strSectionACoverSI = strSectionACoverSI + "<br>" + Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionACoverSIText;
                            }
                        }
                        string td1SectionARow = "<td style='border: 1px solid black' width='20%'><p>Section A</p></td>";
                        string td2SectionARow = "<td style='border: 1px solid black' width='39%'><p>" + strSectionACoverName + "</p></td>";
                        string td3SectionARow = "<td style='border: 1px solid black;text-align:right' width='39%'><p>" + strSectionACoverSI + "</p></td>";
                        trSectionARow = "<tr>" + td1SectionARow + td2SectionARow + td3SectionARow + "</tr>";
                        IsCoverAvailable = true;
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strExtSectionACoverSIText = string.IsNullOrEmpty(dr["COVER_SI_TEXT"].ToString().Trim()) ? "" : "(" + dr["COVER_SI_TEXT"].ToString() + ")";
                            if (strExtSectionACoverName.Length == 0)
                            {
                                strExtSectionACoverName = dr["NAME_OF_BENEFIT"].ToString();
                                strExtSectionACoverSI = Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strExtSectionACoverSIText;
                            }
                            else
                            {
                                strExtSectionACoverName = strExtSectionACoverName + "<br>" + dr["NAME_OF_BENEFIT"].ToString();
                                strExtSectionACoverSI = strExtSectionACoverSI + "<br>" + Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strExtSectionACoverSIText;
                            }
                        }

                        string td1ExtSectionARow = "<td style='border: 1px solid black' width='20%'><p>Extensions under Section A</p></td>";
                        string td2ExtSectionARow = "<td style='border: 1px solid black' width='39%'><p>" + strExtSectionACoverName + "</p></td>";
                        string td3ExtSectionARow = "<td style='border: 1px solid black;text-align:right' width='39%'><p>" + strExtSectionACoverSI + "</p></td>";
                        trExtSectionARow = "<tr>" + td1ExtSectionARow + td2ExtSectionARow + td3ExtSectionARow + "</tr>";
                        IsCoverAvailable = true;
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            strSectionBCoverSIText = string.IsNullOrEmpty(dr["COVER_SI_TEXT"].ToString().Trim()) ? "" : "(" + dr["COVER_SI_TEXT"].ToString() + ")";
                            if (strSectionBCoverName.Length == 0)
                            {
                                strSectionBCoverName = dr["NAME_OF_BENEFIT"].ToString();
                                strSectionBCoverSI = Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionBCoverSIText;
                            }
                            else
                            {
                                strSectionBCoverName = strSectionBCoverName + "<br>" + dr["NAME_OF_BENEFIT"].ToString();
                                strSectionBCoverSI = strSectionBCoverSI + "<br>" + Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionBCoverSIText;
                            }
                        }

                        string td1SectionBRow = "<td style='border: 1px solid black' width='20%'><p>Section B</p></td>";
                        string td2SectionBRow = "<td style='border: 1px solid black' width='39%'><p>" + strSectionBCoverName + "</p></td>";
                        string td3SectionBRow = "<td style='border: 1px solid black;text-align:right' width='39%'><p>" + strSectionBCoverSI + "</p></td>";
                        trSectionBRow = "<tr>" + td1SectionBRow + td2SectionBRow + td3SectionBRow + "</tr>";
                        IsCoverAvailable = true;
                    }
                }
            }

            if (IsCoverAvailable)
            {
                string td1CoverHeader = "<td style='border:1px solid black' width='20%'><p><strong>Coverage Details</strong></p></td>";
                string td2CoverHeader = "<td style='border:1px solid black' width='39%'><p><strong>Name of the Benefit</strong></p></td>";
                string td3CoverHeader = "<td style='border:1px solid black;text-align:center' width='39%'><p><strong>Sum Insured (&#8377;)</strong></p></td>";
                trRowCoverHeader = "<tr>" + td1CoverHeader + td2CoverHeader + td3CoverHeader + "</tr>";
            }
        }


        private void DownloadCertificateFromGIST(string emailId, string CustomerName, string certificateNo, string ProductCode, string MobileNumber)
        {


            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Download Policy Schedule start  " + DateTime.Now.ToString() + System.Environment.NewLine);
            File.AppendAllText(logFile, "Download Policy Schedule start " +  DateTime.Now + Environment.NewLine);
            bool IsSendOnMail = false;
            bool IsDownLoad = true;
            string interactionId = CreateInteraction(certificateNo, MobileNumber, emailId, IsDownLoad).ToString();
            File.AppendAllText(FrmDownloadPolicyScheduleLog, "Interaction Created " + interactionID + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            AddSearchLog(certificateNo, MobileNumber, emailId, IsSendOnMail, IsDownLoad, interactionId);
            try
            {

                string PolicyNumber = certificateNo;
                //string ProductCode = "";
                if (!string.IsNullOrEmpty(hdnProductCode.Value))
                {
                    ProductCode = hdnProductCode.Value;
                }
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "Downloading start for Policy Number" + PolicyNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "Product Code" + ProductCode + "  " + DateTime.Now.ToString() + System.Environment.NewLine);




            WebRequest.DefaultWebProxy = null;
            string ErrorMsg = string.Empty;
            PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
            byte[] objByte = proxy.KGIGetPolicyDocumentForPortal("16e9e45962de4725a83994c4c3145517", certificateNo, ProductCode, ref ErrorMsg); //1000401000 //1000340100

            if (string.IsNullOrEmpty(ErrorMsg))
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/force-download";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + certificateNo + ".pdf");
                HttpContext.Current.Response.BinaryWrite(objByte);
                //Response.End();
                HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadPolicyScheduleNew.aspx");//Added by Rajesh Soni 24/02/2020
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.

            }
            else
            {
                    // Alert.Show("Error Occured While Download Policy " + certificateNo + ":"+ ErrorMsg + "  ErrorMessage ");
                    // Label1.Text = "“Sorry!! Due to some technical issue we are unable to provide the required information. Please try again after sometime. For assistance call on 1800 266 4545 or email us at care@kotak.com”";
                    ExceptionUtility.LogEvent("Error Occured While Download Policy " + certificateNo + "  ErrorMessage "
                    + ErrorMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                    File.AppendAllText(logFile, "Error Occured While Download Policy " + certificateNo + "  ErrorMessage "
                    + ErrorMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                    File.AppendAllText(FrmDownloadPolicyScheduleLog, "Error Occured While Download Policy " + certificateNo + "  ErrorMessage "
                    + ErrorMsg + DateTime.Now.ToString() + System.Environment.NewLine);
                    divtechnicalissueMessage.Visible = true;
                   
                    return;
                }



                //// fnAddLogGistPolicyDownload(emailId, CustomerName, certificateNo, Session["vUserLoginId"].ToString().ToUpper(), "Download");
                //string ErrorMsg = string.Empty;
                //PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();

                ////File.AppendAllText(logFile, System.Environment.NewLine + "Getting PDF byte brom GIST   certificateNo  " + certificateNo
                ////    + ", ProductCode " + ProductCode + " " + DateTime.Now.ToString() + System.Environment.NewLine);

                //byte[] objByte = proxy.KGIGetPolicyDocumentForPortal("81062f2fc69b4639af5bf33e86c66408", certificateNo, ProductCode, ref ErrorMsg);

                //string fileName = certificateNo + ".pdf";
                //if (ErrorMsg.Trim().Length <= 0)
                //{

                //    Response.Clear();
                //    Response.ContentType = "application/force-download";
                //    //Response.AddHeader("content-disposition", "attachment;filename=1010404900.pdf");
                //    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                //    Response.BinaryWrite(objByte);
                //    CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadPolicyScheduleNew.aspx");//Added By Rajesh Soni on 20/02/2020
                //    Response.End();
                //}
                //else
                //{
                //    Alert.Show("Some error occurred. If you are facing this issue repetatively contact to PASS support team.");
                //    File.AppendAllText(logFile, " DownloadCertificateFromGIST " + ErrorMsg + DateTime.Now + Environment.NewLine);
                //    return;
                //}
            }
            catch (Exception ex)
            {
                File.AppendAllText(FrmDownloadPolicyScheduleLog, "Error Occured While Download Policy error message " + ex.ToString() + "  Error stack "
                    + ex.StackTrace + DateTime.Now.ToString() + System.Environment.NewLine);
                sectionMain.Visible = false;
                sectionError.Visible = true;
            }
        }


        private void fnGenerateHDCSchedule(string certificateNo, string emailId, string CustomerName)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ToString()))
            {
                con.Open();
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_PDF_CompleteLetter.html";
                string htmlBody = File.ReadAllText(strPath);
                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(sw.ToString());
                string strHtml = htmlBody;

                SqlCommand command = new SqlCommand("PROC_GET_COVER_SECTION_DATA_FOR_HDC_PDF_TEST", con);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                command.Parameters.AddWithValue("@vCertificateNo", certificateNo);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_Floater_PDF_CompleteLetter.html";
                        htmlBody = File.ReadAllText(strPath);
                        sw = new StringWriter();
                        sr = new StringReader(sw.ToString());
                        strHtml = htmlBody;

                        GenerateNonEmailHDC_Flotaer_PDF(con, ds, strHtml, certificateNo, emailId, CustomerName);

                    }
                }
                //File.AppendAllText(logFile, "html body created for certificate number " + certificateNo + Environment.NewLine);
            }
        }

        private void GenerateNonEmailHDC_Flotaer_PDF(SqlConnection con, DataSet ds, string strHtml, string certificateNo)
        {
            try
            {
                string accidentalDeath = string.Empty;
                string permTotalDisable = string.Empty;
                string permPartialDisable = string.Empty;
                string tempTotalDisable = string.Empty;
                string carraigeBody = string.Empty;
                string funeralExpense = string.Empty;
                string medicalExpense = string.Empty;
                string purchaseBlood = string.Empty;
                string transportation = string.Empty;
                string compassionate = string.Empty;
                string disappearance = string.Empty;
                string modifyResidence = string.Empty;
                string costOfSupport = string.Empty;
                string commonCarrier = string.Empty;
                string childrenGrant = string.Empty;
                string marraigeExpense = string.Empty;
                string sportsActivity = string.Empty;
                string widowHood = string.Empty;

                string ambulanceChargesString = string.Empty;
                string dailyCashString = string.Empty;
                string accidentalHospString = string.Empty;
                string opdString = string.Empty;
                string accidentalDentalString = string.Empty;
                string convalString = string.Empty;
                string burnsString = string.Empty;
                string brokenBones = string.Empty;
                string comaString = string.Empty;
                string domesticTravelString = string.Empty;
                string lossofEmployString = string.Empty;
                string onDutyCover = string.Empty;
                string legalExpenses = string.Empty;

                string reducingCoverString = string.Empty;
                string assignmentString = string.Empty;

                //gst
                string custStateCode = string.Empty;
                string igstString = string.Empty;
                string cgstsgstString = string.Empty;
                string cgstugstString = string.Empty;
                string vCustomerType = string.Empty;

                //CR_450_added By Kuwar_start
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "'";
                    cmd.Connection = con;
                    //sqlCon.Open();
                    object objCustState = cmd.ExecuteScalar();
                    custStateCode = Convert.ToString(objCustState);
                }

                //Cr_450 Added By Kuwar End


                #region HDC CERTIFICATE OF INSURANCE

                strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                strHtml = strHtml.Replace("@masterDate", ds.Tables[0].Rows[0]["vMasterPolicyIssueDate"].ToString());
                strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyIssueLocation"].ToString());
                strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vMasterPolicyHolder"].ToString());
                strHtml = strHtml.Replace("@Customers", ds.Tables[0].Rows[0]["vGroupType"].ToString());
                strHtml = strHtml.Replace("@GroupType", ds.Tables[0].Rows[0]["vGroupType"].ToString());
                vCustomerType = ds.Tables[0].Rows[0]["vCustomerType"].ToString();

                string AccountNo = ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString() + " / " + ds.Tables[0].Rows[0]["vAccountNo"].ToString();
                if (AccountNo.Substring(0, 3) == " / ")
                {
                    AccountNo = AccountNo.Substring(2, AccountNo.Length - 2);
                }
                strHtml = strHtml.Replace("@MemberShipIDEmpNOAccNo", AccountNo);
                strHtml = strHtml.Replace("@CreditAmountOutStandingCreditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                strHtml = strHtml.Replace("@CreditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
                strHtml = strHtml.Replace("@DeductibleBaseCovers", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
                strHtml = strHtml.Replace("@DescriptionRemark", ds.Tables[0].Rows[0]["vComments"].ToString());
                strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
                strHtml = strHtml.Replace("@ProposarGSTN", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                //Added By Nilesh
                strHtml = strHtml.Replace("@ProposarMobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                strHtml = strHtml.Replace("@ProposarEmailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                //End By Nilesh
                #endregion


                #region DETAILS OF THE INSURED PERSON(S) UNDER THE POLICY

                strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
                strHtml = strHtml.Replace("@issuedBranchkgi", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
                strHtml = strHtml.Replace("@issuedDate", ds.Tables[0].Rows[0]["vTransactionDate"].ToString());
                strHtml = strHtml.Replace("@startDate", ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString());
                strHtml = strHtml.Replace("@endDate", ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString());
                strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString());// + ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                strHtml = strHtml.Replace("@FinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());
                strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
                strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                strHtml = strHtml.Replace("@gstin", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["vCustomerDob"].ToString());
                strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                strHtml = strHtml.Replace("@category", ds.Tables[0].Rows[0]["vCustomerCategory"].ToString());
                strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());
                strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeDOB"].ToString());
                strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vNomineeAppointee"].ToString());
                strHtml = strHtml.Replace("@deduct", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
                strHtml = strHtml.Replace("@cuAadhar", ds.Tables[0].Rows[0]["vCustomerAadhar"].ToString());
                strHtml = strHtml.Replace("@proposeradd", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());



                #endregion


                #region  INTERMEDIARY DETAILS
                strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryContact"].ToString());
                strHtml = strHtml.Replace("@intermediaryLandline", ds.Tables[0].Rows[0]["vIntermediaryLandline"].ToString());

                if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                {
                    strHtml = strHtml.Replace("KOTAK GROUP SMART CASH", "KOTAK GROUP SMART CASH – MICRO INSURANCE");
                    strHtml = strHtml.Replace("Kotak Group Smart Cash", "Kotak Group Smart Cash – Micro Insurance");
                }


                if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                {
                    strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash – Micro Insurance UIN:KOTHMGP076V011819");
                }
                else
                {
                    strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash UIN: KOTHLGP19014V011819");
                }

                #endregion

                #region  COVERAGE DETAILS

                #region Code for Covers

                StringBuilder coverstring = new StringBuilder();
                if (ds.Tables[1].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        coverstring.Append("<tr>");
                        if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "B")
                        {
                            coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Base Covers </strong></td>");
                            coverstring.Append("</tr><tr>");
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        else if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "O")
                        {

                            coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Optional Covers </strong></td>");
                            coverstring.Append("</tr><tr>");
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        else
                        {
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        coverstring.Append("</tr>");
                    }
                }

                #endregion

                strHtml = strHtml.Replace("@CoversString", coverstring.ToString());


                #region Code for Important Conditions

                strHtml = strHtml.Replace("@Condition1", ds.Tables[0].Rows[0]["vCondition1"].ToString());
                strHtml = strHtml.Replace("@Condition2", ds.Tables[0].Rows[0]["vCondition2"].ToString());
                strHtml = strHtml.Replace("@Condition3", ds.Tables[0].Rows[0]["vCondition3"].ToString());
                strHtml = strHtml.Replace("@Condition4", ds.Tables[0].Rows[0]["vCondition4"].ToString());
                strHtml = strHtml.Replace("@Condition5", ds.Tables[0].Rows[0]["vCondition5"].ToString());

                #endregion

                #endregion

                #region  PREMIUM DETAILS

                strHtml = strHtml.Replace("@cgstpercentagestring", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@ugstpercentagestring", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@sgstpercentagestring", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@igstpercentagestring", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());


                strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                strHtml = strHtml.Replace("@cgstsgstString", ds.Tables[0].Rows[0]["vCGST"].ToString());
                strHtml = strHtml.Replace("@ugstsgstString", ds.Tables[0].Rows[0]["vUGST"].ToString());
                strHtml = strHtml.Replace("@sgstsgstString", ds.Tables[0].Rows[0]["vSGST"].ToString());
                strHtml = strHtml.Replace("@igstsgstString", ds.Tables[0].Rows[0]["vIGST"].ToString());
                strHtml = strHtml.Replace("@TotalpremiumString", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());

                #endregion


                #region  TAX DETAILS

                strHtml = strHtml.Replace("@gstn", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
                strHtml = strHtml.Replace("@Category", ds.Tables[0].Rows[0]["vCategory"].ToString());
                strHtml = strHtml.Replace("@SACCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
                strHtml = strHtml.Replace("@Description", ds.Tables[0].Rows[0]["vDescription"].ToString());
                strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());


                strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["vStampDuty"].ToString());
                strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
                strHtml = strHtml.Replace("@challanDate", ds.Tables[0].Rows[0]["vChallanDate"].ToString());


                #endregion

                #region Floater Policy Details
                DataTable dtFloaterNominee = ds.Tables[2];


                string tbodyFloaterNominee = string.Empty;
                tbodyFloaterNominee = @"<tr><td style='border: 1px solid black;width:90px'> Insured Name </th> <td style='border: 1px solid black'> Insured Relationship</th> <td style='border: 1px solid black'> Insured Type </th> <td style='border: 1px solid black' width='12%'> DOB/AGE </th><td style='border: 1px solid black'> Gender </th><td style='border: 1px solid black'> Nominee Name </th><td style='border: 1px solid black' width='5%'> Nominee Relation </th><td style='border: 1px solid black' width='8%'> Nominee DOB/AGE </th></tr> ";

                foreach (DataRow dr in dtFloaterNominee.Rows)
                {
                    strHtml = strHtml.Replace("@ProposarPanAdhar", dr["CustomerPANorAdhar"].ToString());
                    strHtml = strHtml.Replace("@NameofFinancier", dr["vFinancerName"].ToString());
                    strHtml = strHtml.Replace("@ProposarName", dr["vCustomerName"].ToString());
                    strHtml = strHtml.Replace("@ProposarAddress", dr["ProposarAddress"].ToString());

                    if (!string.IsNullOrEmpty(dr["InsuredName1"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredName1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredRelation1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredDOB1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredGender1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeName1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeRelation1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeDOB1"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }


                    if (!string.IsNullOrEmpty(dr["InsuredName2"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB2"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName3"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB3"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }


                    if (!string.IsNullOrEmpty(dr["InsuredName4"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB4"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName5"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB5"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName6"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB6"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }

                }

                strHtml = strHtml.Replace("@tbody", tbodyFloaterNominee.ToString());
                //strHtml = strHtml.Replace("NULL", "");

                #endregion

                #region HDC RISK
                string _Date1 = ds.Tables[0].Rows[0]["vTransactionDate"].ToString();
                DateTime dtDateHDCRisk = Convert.ToDateTime(_Date1);

                string TransactionDateHDCRisk = dtDateHDCRisk.ToString("dd/MM/yyyy");
                strHtml = strHtml.Replace("@TransactionDateHDCRisk", TransactionDateHDCRisk);

                string mentionedGender = ds.Tables[0].Rows[0]["vCustomerGender"].ToString();
                if (string.IsNullOrEmpty(mentionedGender))
                {
                    strHtml = strHtml.Replace("@salutation", "");
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "M" || ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "Male")
                    {
                        strHtml = strHtml.Replace("@salutation", "Mr.");
                    }
                    else if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "F")
                    {
                        strHtml = strHtml.Replace("@salutation", "Mrs.");
                    }
                    else
                    {
                        strHtml = strHtml.Replace("@salutation", "");
                    }
                }
                strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                strHtml = strHtml.Replace("@addressofinsured1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                strHtml = strHtml.Replace("@addressofinsured2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                strHtml = strHtml.Replace("@addressofinsured3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                strHtml = strHtml.Replace("@addressofinsuredCity", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                strHtml = strHtml.Replace("@addressofinsuredState", ds.Tables[0].Rows[0]["vProposerState"].ToString());
                strHtml = strHtml.Replace("@Pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                strHtml = strHtml.Replace("@mobileno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());

                strHtml = strHtml.Replace("@productname", ds.Tables[0].Rows[0]["vProductName"].ToString());
                #endregion


                #region HDC 80D CERTIFICATE
                string _Date = ds.Tables[0].Rows[0]["vAccountDebitDate"].ToString();
                DateTime dt = Convert.ToDateTime(_Date);

                string FDate = dt.ToString("dd/MM/yyyy");
                strHtml = strHtml.Replace("@ddateForRisk", FDate);
                strHtml = strHtml.Replace("@TotalPremium", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());
                strHtml = strHtml.Replace("@paymentmode", ds.Tables[0].Rows[0]["vPaymentMode"].ToString());
                int policytnur = Convert.ToInt32(ds.Tables[0].Rows[0]["vPolicyTenure"].ToString());
                //double totalpremium = Convert.ToDouble(ds.Tables[0].Rows[0]["vTotalPremium"].ToString());


                string startdate = ds.Tables[0].Rows[0]["vPolicyStartDate"].ToString();
                string enddate = ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString();

                DateTime date = Convert.ToDateTime(startdate);
                string startdateyear1 = date.Year.ToString();
                int MonthofStartYear = date.Month;
                DateTime date1 = Convert.ToDateTime(enddate);
                string enddateyear2 = date1.Year.ToString();
                int shortenddateyear2 = Convert.ToInt32(enddateyear2.Substring(2)) - 1;
                string year5 = Convert.ToString(shortenddateyear2);
                string FYForLUMSUMyear4;
                if (MonthofStartYear > 3)
                {
                    FYForLUMSUMyear4 = Convert.ToInt32(startdateyear1) + "-" + (Convert.ToInt32(startdateyear1) + 1);
                }
                else
                {
                    FYForLUMSUMyear4 = (Convert.ToInt32(startdateyear1) - 1) + "-" + (startdateyear1.Substring(2));
                }
                strHtml = strHtml.Replace("@Year", FYForLUMSUMyear4);

                int YearDuration = Convert.ToInt32(enddateyear2) - Convert.ToInt32(startdateyear1);
                string totalpremiumamount = ds.Tables[0].Rows[0]["vTotalPremium"].ToString();
                double totalpremiumamt = Convert.ToDouble(totalpremiumamount);
                double amount2 = totalpremiumamt / YearDuration;
                //string amount2 = Convert.ToString(amount1);
                double amount = Math.Round(amount2, 2);
                StringBuilder sb = new StringBuilder();
                sb.Append("<table style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri'>");
                sb.Append("<td style='width:200;border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                sb.Append("<p style='margin-left: 20px;'><span>Financial Year</span></p>");
                sb.Append("</td>");
                sb.Append("<td style='width:650;border:1px solid black;border-collapse:collapse;'>");
                sb.Append("<p style='margin-left: 20px;'><span>Year wise proportionate premium allowed for Deduction under Section 80D</span></p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                string FYForYearWiseLumsumDividendYear02;
                if (MonthofStartYear > 3)
                {
                    FYForYearWiseLumsumDividendYear02 = startdateyear1;
                }
                else
                {
                    int Yeart = Convert.ToInt32(startdateyear1) - 1;
                    FYForYearWiseLumsumDividendYear02 = Convert.ToString(Yeart);
                }
                for (int H = 0; H < YearDuration; H++)
                {
                    DataTable dt1 = new DataTable();
                    sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");

                    int Year00 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02) + H;
                    int sum = H + 1;
                    int Year01 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02.Substring(2)) + sum;

                    string year6 = Convert.ToString(Year00) + "-" + Convert.ToString(Year01);

                    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:200;'>");
                    sb.Append("<p style='margin-left: 20px;'> " + year6 + " </p>");
                    sb.Append("</td>");
                    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:650;'>");
                    sb.Append("<p style='margin-left: 20px;'> " + amount + " </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                strHtml = strHtml.Replace("@testHTMLTABLE", sb.ToString());
                #endregion

                strHtml = strHtml.Replace("@KotakGroupSmartCashUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupSmartCashUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupSmartCashUIN"]));

                #region TaxInvoice
                //CR_450_Start Kuwar
                //HDC_Floater_PDF_NonEmail
                StringBuilder taxinvoice = new StringBuilder();
                if (ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString() == "")
                {

                    taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                    int temp = 0;
                    string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                    string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                    string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                    string totalPremium = ds.Tables[0].Rows[0]["vTotalPremium"].ToString();

                    if (totalPremium.Contains('.'))
                    {
                        temp = Convert.ToInt32(totalPremium.Substring(0, totalPremium.IndexOf('.')));

                    }
                    else
                    {
                        temp = Convert.ToInt32(totalPremium);
                    }
                    string totalPremiumInWord = ConvertAmountInWord(temp);
                    //string stateCode = 

                    // QR Code
                    string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                    string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                    string transactionDate = ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString();
                    int noofHSNCode = 0;
                    string hsnCode = ds.Tables[0].Rows[0]["vSacCode"].ToString();
                    string receiptNumber = ds.Tables[0].Rows[0]["vChallanNo"].ToString();
                    if (hsnCode != "")
                    {
                        var tempcount = hsnCode.Split(' ').Length;
                        for (int i = 0; i < tempcount; i++)
                        {
                            noofHSNCode++;
                        }

                    }

                    string Imagepath = string.Empty;
                    CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                    string kgiStateCode = suppliGSTN.Substring(0, 2); // getting kgi state code 
                    string kgiStateName = string.Empty;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                        cmd.Connection = con;
                        //sqlCon.Open();
                        object objStaeName = cmd.ExecuteScalar();
                        kgiStateName = Convert.ToString(objStaeName);

                    }
                    Imagepath = Imagepath == "error" ? "" : Imagepath;
                    strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);


                    strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                    //HDC Policy
                    strHtml = strHtml.Replace("@gistinno", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                    strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                    strHtml = strHtml.Replace("@customername", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                    strHtml = strHtml.Replace("@emailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                    strHtml = strHtml.Replace("@contactno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                    strHtml = strHtml.Replace("@address", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());
                    strHtml = strHtml.Replace("@imdcode", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                    strHtml = strHtml.Replace("@receiptno", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
                    strHtml = strHtml.Replace("@customerstatecode", custStateCode);
                    //strHtml = strHtml.Replace("@customerstatecode", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst statecode of customer require
                    strHtml = strHtml.Replace("@supplyname", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst state name require of customer

                    //strHtml = strHtml.Replace("@gistinno", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                    strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
                    strHtml = strHtml.Replace("@name", kgiName);
                    strHtml = strHtml.Replace("@panNo", kgiPanno);
                    strHtml = strHtml.Replace("@cinNo", kgiCINno);
                    //strHtml = strHtml.Replace("@address", " ");
                    strHtml = strHtml.Replace("@vKGIBranchAddress", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
                    strHtml = strHtml.Replace("@invoicedate", ds.Tables[0].Rows[0]["vpolicyStartDate"].ToString());
                    strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                    strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["vAddCol1"].ToString());

                    strHtml = strHtml.Replace("@partnerappno", "");// this column is there as per jay
                    strHtml = strHtml.Replace("@statecode", kgiStateCode);//gst state code of kotak data coming from table
                    strHtml = strHtml.Replace("@statename", kgiStateName);//gst state code of kotak
                    strHtml = strHtml.Replace("@irn", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                    //HDC Policy
                    strHtml = strHtml.Replace("@hsnDescription", ds.Tables[0].Rows[0]["vDescription"].ToString());
                    strHtml = strHtml.Replace("@HSNCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
                    strHtml = strHtml.Replace("@totalpremium", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());
                    strHtml = strHtml.Replace("@netamount", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                    strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                    strHtml = strHtml.Replace("@totalgst", ds.Tables[0].Rows[0]["vTotalGSTAmount"].ToString());

                    strHtml = strHtml.Replace("@cgstpercent", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
                    strHtml = strHtml.Replace("@ugstpercent", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
                    strHtml = strHtml.Replace("@sgstpercent", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
                    strHtml = strHtml.Replace("@igstpercent", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());

                    //HDC Policy

                    strHtml = strHtml.Replace("@totalamount", totalPremium);
                    strHtml = strHtml.Replace("@cgstamt", ds.Tables[0].Rows[0]["vCGST"].ToString());
                    strHtml = strHtml.Replace("@ugstamt", ds.Tables[0].Rows[0]["vUGST"].ToString());
                    strHtml = strHtml.Replace("@sgstamt", ds.Tables[0].Rows[0]["vSGST"].ToString());
                    strHtml = strHtml.Replace("@igstamt", ds.Tables[0].Rows[0]["vIGST"].ToString());

                    //   strHtml = strHtml.Replace("@cessrate", ds.Tables[0].Rows[0]["vSGST"].ToString());
                    //  strHtml = strHtml.Replace("@cessamt", ds.Tables[0].Rows[0]["vIGST"].ToString());

                    strHtml = strHtml.Replace("@cessrate", "0");
                    strHtml = strHtml.Replace("@cessamt", "0");
                    strHtml = strHtml.Replace("@totalgross", totalPremium);// change1
                    strHtml = strHtml.Replace("@totalinvoicevalueinwords", totalPremiumInWord.ToString());
                    //   strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                    //  strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                }
                else
                {

                    taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: none'>");

                    strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                }
                //CR_450_End_Kuwar HDC Policy
                #endregion

                // below code for download pdf
                TextWriter outTextWriter = new StringWriter();
                HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                //base.Render(outHtmlTextWriter);
                string currentPageHtmlString = strHtml; //outTextWriter.ToString();
                                                        // Create a HTML to PDF converter object with default settings
                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
                // Set license key received after purchase to use the converter in licensed mode
                // Leave it not set to use the converter in demo mode
                string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();
                htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";
                                                                 // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                                                                 // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                htmlToPdfConverter.ConversionDelay = 2;
                // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
                // Add Header
                // Enable header in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                // Optionally add a space between header and the page body
                // The spacing for first page and the subsequent pages can be set independently
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");
                // Draw header elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                    DrawHeader(htmlToPdfConverter, false);
                // Add Footer
                // Enable footer in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
                // Optionally add a space between footer and the page body
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");
                // Draw footer elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                    DrawFooter(htmlToPdfConverter, false, true);
                // Use the current page URL as base URL
                string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                // Convert the current page HTML string to a PDF document in a memory buffer
                // For Live
                byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                // For Live End Here 

                ////// For Dev
                //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                ////// byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                ////// For Dev End here 

                // Send the PDF as response to browser

                // Set response content type
                Response.AddHeader("Content-Type", "application/pdf");

                // Instruct the browser to open the PDF file as an attachment or inline
                //Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), txtCertificateNumber.Text.Trim()));

                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=HDCPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNo.Trim().ToString()));

                // Write the PDF document buffer to HTTP response
                //Response.BinaryWrite(outPdfBuffer);
                Response.BinaryWrite(outPdfBuffer);

                // End the HTTP response and stop the current page processing
                CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadGPAPolicy.aspx");//Added By Rajesh Soni 19/02/2020
                Response.End();


            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GenerateNonEmailHDC_Flotaer_PDF");
            }
        }

        private void GenerateNonEmailHDC_Flotaer_PDF(SqlConnection con, DataSet ds, string strHtml, string certificateNo, string emailId, string CustomerName)
        {
            try
            {
                string accidentalDeath = string.Empty;
                string permTotalDisable = string.Empty;
                string permPartialDisable = string.Empty;
                string tempTotalDisable = string.Empty;
                string carraigeBody = string.Empty;
                string funeralExpense = string.Empty;
                string medicalExpense = string.Empty;
                string purchaseBlood = string.Empty;
                string transportation = string.Empty;
                string compassionate = string.Empty;
                string disappearance = string.Empty;
                string modifyResidence = string.Empty;
                string costOfSupport = string.Empty;
                string commonCarrier = string.Empty;
                string childrenGrant = string.Empty;
                string marraigeExpense = string.Empty;
                string sportsActivity = string.Empty;
                string widowHood = string.Empty;

                string ambulanceChargesString = string.Empty;
                string dailyCashString = string.Empty;
                string accidentalHospString = string.Empty;
                string opdString = string.Empty;
                string accidentalDentalString = string.Empty;
                string convalString = string.Empty;
                string burnsString = string.Empty;
                string brokenBones = string.Empty;
                string comaString = string.Empty;
                string domesticTravelString = string.Empty;
                string lossofEmployString = string.Empty;
                string onDutyCover = string.Empty;
                string legalExpenses = string.Empty;

                string reducingCoverString = string.Empty;
                string assignmentString = string.Empty;

                //gst
                string custStateCode = string.Empty;
                string igstString = string.Empty;
                string cgstsgstString = string.Empty;
                string cgstugstString = string.Empty;
                string vCustomerType = string.Empty;

                //CR_450_added By Kuwar_start
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "'";
                    cmd.Connection = con;
                    //sqlCon.Open();
                    object objCustState = cmd.ExecuteScalar();
                    custStateCode = Convert.ToString(objCustState);
                }
                //Cr_450 Added By Kuwar End

                #region HDC CERTIFICATE OF INSURANCE

                strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                strHtml = strHtml.Replace("@masterDate", ds.Tables[0].Rows[0]["vMasterPolicyIssueDate"].ToString());
                strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyIssueLocation"].ToString());
                strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vMasterPolicyHolder"].ToString());
                strHtml = strHtml.Replace("@Customers", ds.Tables[0].Rows[0]["vGroupType"].ToString());
                strHtml = strHtml.Replace("@GroupType", ds.Tables[0].Rows[0]["vGroupType"].ToString());
                vCustomerType = ds.Tables[0].Rows[0]["vCustomerType"].ToString();

                string AccountNo = ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString() + " / " + ds.Tables[0].Rows[0]["vAccountNo"].ToString();
                if (AccountNo.Substring(0, 3) == " / ")
                {
                    AccountNo = AccountNo.Substring(2, AccountNo.Length - 2);
                }
                strHtml = strHtml.Replace("@MemberShipIDEmpNOAccNo", AccountNo);
                strHtml = strHtml.Replace("@CreditAmountOutStandingCreditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                strHtml = strHtml.Replace("@CreditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
                strHtml = strHtml.Replace("@DeductibleBaseCovers", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
                strHtml = strHtml.Replace("@DescriptionRemark", ds.Tables[0].Rows[0]["vComments"].ToString());
                strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
                strHtml = strHtml.Replace("@ProposarGSTN", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                //Added By Nilesh
                strHtml = strHtml.Replace("@ProposarMobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                strHtml = strHtml.Replace("@ProposarEmailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                //End By Nilesh
                #endregion


                #region DETAILS OF THE INSURED PERSON(S) UNDER THE POLICY

                strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
                strHtml = strHtml.Replace("@issuedBranchkgi", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
                strHtml = strHtml.Replace("@issuedDate", ds.Tables[0].Rows[0]["vTransactionDate"].ToString());
                strHtml = strHtml.Replace("@startDate", ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString());
                strHtml = strHtml.Replace("@endDate", ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString());
                strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString());// + ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                strHtml = strHtml.Replace("@FinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());
                strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
                strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                strHtml = strHtml.Replace("@gstin", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["vCustomerDob"].ToString());
                strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                strHtml = strHtml.Replace("@category", ds.Tables[0].Rows[0]["vCustomerCategory"].ToString());
                strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());
                strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeDOB"].ToString());
                strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vNomineeAppointee"].ToString());
                strHtml = strHtml.Replace("@deduct", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
                strHtml = strHtml.Replace("@cuAadhar", ds.Tables[0].Rows[0]["vCustomerAadhar"].ToString());
                strHtml = strHtml.Replace("@proposeradd", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());



                #endregion


                #region  INTERMEDIARY DETAILS
                strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryContact"].ToString());
                strHtml = strHtml.Replace("@intermediaryLandline", ds.Tables[0].Rows[0]["vIntermediaryLandline"].ToString());

                if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                {
                    strHtml = strHtml.Replace("KOTAK GROUP SMART CASH", "KOTAK GROUP SMART CASH – MICRO INSURANCE");
                    strHtml = strHtml.Replace("Kotak Group Smart Cash", "Kotak Group Smart Cash – Micro Insurance");
                }


                if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                {
                    strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash – Micro Insurance UIN:KOTHMGP076V011819");
                }
                else
                {
                    strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash UIN: KOTHLGP19014V011819");
                }

                #endregion

                #region  COVERAGE DETAILS

                #region Code for Covers

                StringBuilder coverstring = new StringBuilder();
                if (ds.Tables[1].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        coverstring.Append("<tr>");
                        if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "B")
                        {
                            coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Base Covers </strong></td>");
                            coverstring.Append("</tr><tr>");
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        else if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "O")
                        {

                            coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Optional Covers </strong></td>");
                            coverstring.Append("</tr><tr>");
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        else
                        {
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        coverstring.Append("</tr>");
                    }
                }

                #endregion

                strHtml = strHtml.Replace("@CoversString", coverstring.ToString());


                #region Code for Important Conditions

                strHtml = strHtml.Replace("@Condition1", ds.Tables[0].Rows[0]["vCondition1"].ToString());
                strHtml = strHtml.Replace("@Condition2", ds.Tables[0].Rows[0]["vCondition2"].ToString());
                strHtml = strHtml.Replace("@Condition3", ds.Tables[0].Rows[0]["vCondition3"].ToString());
                strHtml = strHtml.Replace("@Condition4", ds.Tables[0].Rows[0]["vCondition4"].ToString());
                strHtml = strHtml.Replace("@Condition5", ds.Tables[0].Rows[0]["vCondition5"].ToString());

                #endregion

                #endregion

                #region  PREMIUM DETAILS

                strHtml = strHtml.Replace("@cgstpercentagestring", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@ugstpercentagestring", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@sgstpercentagestring", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@igstpercentagestring", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());


                strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                strHtml = strHtml.Replace("@cgstsgstString", ds.Tables[0].Rows[0]["vCGST"].ToString());
                strHtml = strHtml.Replace("@ugstsgstString", ds.Tables[0].Rows[0]["vUGST"].ToString());
                strHtml = strHtml.Replace("@sgstsgstString", ds.Tables[0].Rows[0]["vSGST"].ToString());
                strHtml = strHtml.Replace("@igstsgstString", ds.Tables[0].Rows[0]["vIGST"].ToString());
                strHtml = strHtml.Replace("@TotalpremiumString", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());

                #endregion


                #region  TAX DETAILS

                strHtml = strHtml.Replace("@gstn", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
                strHtml = strHtml.Replace("@Category", ds.Tables[0].Rows[0]["vCategory"].ToString());
                strHtml = strHtml.Replace("@SACCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
                strHtml = strHtml.Replace("@Description", ds.Tables[0].Rows[0]["vDescription"].ToString());
                strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());


                strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["vStampDuty"].ToString());
                strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
                strHtml = strHtml.Replace("@challanDate", ds.Tables[0].Rows[0]["vChallanDate"].ToString());


                #endregion

                #region Floater Policy Details
                DataTable dtFloaterNominee = ds.Tables[2];


                string tbodyFloaterNominee = string.Empty;
                tbodyFloaterNominee = @"<tr><td style='border: 1px solid black;width:90px'> Insured Name </th> <td style='border: 1px solid black'> Insured Relationship</th> <td style='border: 1px solid black'> Insured Type </th> <td style='border: 1px solid black' width='12%'> DOB </th><td style='border: 1px solid black'> Gender </th><td style='border: 1px solid black'> Nominee Name </th><td style='border: 1px solid black' width='5%'> Nominee Relation </th><td style='border: 1px solid black' width='8%'> Nominee DOB </th></tr> ";

                foreach (DataRow dr in dtFloaterNominee.Rows)
                {
                    strHtml = strHtml.Replace("@ProposarPanAdhar", dr["CustomerPANorAdhar"].ToString());
                    strHtml = strHtml.Replace("@NameofFinancier", dr["vFinancerName"].ToString());
                    strHtml = strHtml.Replace("@ProposarName", dr["vCustomerName"].ToString());
                    strHtml = strHtml.Replace("@ProposarAddress", dr["ProposarAddress"].ToString());

                    if (!string.IsNullOrEmpty(dr["InsuredName1"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredName1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredRelation1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredDOB1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredGender1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeName1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeRelation1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeDOB1"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }


                    if (!string.IsNullOrEmpty(dr["InsuredName2"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB2"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName3"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB3"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }


                    if (!string.IsNullOrEmpty(dr["InsuredName4"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB4"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName5"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB5"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName6"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB6"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }

                }

                strHtml = strHtml.Replace("@tbody", tbodyFloaterNominee.ToString());
                //strHtml = strHtml.Replace("NULL", "");

                #endregion

                #region HDC RISK
                string _Date1 = ds.Tables[0].Rows[0]["vTransactionDate"].ToString();
                DateTime dtDateHDCRisk = Convert.ToDateTime(_Date1);

                string TransactionDateHDCRisk = dtDateHDCRisk.ToString("dd/MM/yyyy");
                strHtml = strHtml.Replace("@TransactionDateHDCRisk", TransactionDateHDCRisk);

                string mentionedGender = ds.Tables[0].Rows[0]["vCustomerGender"].ToString();
                if (string.IsNullOrEmpty(mentionedGender))
                {
                    strHtml = strHtml.Replace("@salutation", "");
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "M" || ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "Male")
                    {
                        strHtml = strHtml.Replace("@salutation", "Mr.");
                    }
                    else if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "F")
                    {
                        strHtml = strHtml.Replace("@salutation", "Mrs.");
                    }
                    else
                    {
                        strHtml = strHtml.Replace("@salutation", "");
                    }
                }
                strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                strHtml = strHtml.Replace("@addressofinsured1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                strHtml = strHtml.Replace("@addressofinsured2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                strHtml = strHtml.Replace("@addressofinsured3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                strHtml = strHtml.Replace("@addressofinsuredCity", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                strHtml = strHtml.Replace("@addressofinsuredState", ds.Tables[0].Rows[0]["vProposerState"].ToString());
                strHtml = strHtml.Replace("@Pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                strHtml = strHtml.Replace("@mobileno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());

                strHtml = strHtml.Replace("@productname", ds.Tables[0].Rows[0]["vProductName"].ToString());
                #endregion


                #region HDC 80D CERTIFICATE
                string _Date = ds.Tables[0].Rows[0]["vAccountDebitDate"].ToString();
                DateTime dt = Convert.ToDateTime(_Date);

                string FDate = dt.ToString("dd/MM/yyyy");
                strHtml = strHtml.Replace("@ddateForRisk", FDate);
                strHtml = strHtml.Replace("@TotalPremium", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());
                strHtml = strHtml.Replace("@paymentmode", ds.Tables[0].Rows[0]["vPaymentMode"].ToString());
                int policytnur = Convert.ToInt32(ds.Tables[0].Rows[0]["vPolicyTenure"].ToString());
                //double totalpremium = Convert.ToDouble(ds.Tables[0].Rows[0]["vTotalPremium"].ToString());


                string startdate = ds.Tables[0].Rows[0]["vPolicyStartDate"].ToString();
                string enddate = ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString();

                DateTime date = Convert.ToDateTime(startdate);
                string startdateyear1 = date.Year.ToString();
                int MonthofStartYear = date.Month;
                DateTime date1 = Convert.ToDateTime(enddate);
                string enddateyear2 = date1.Year.ToString();
                int shortenddateyear2 = Convert.ToInt32(enddateyear2.Substring(2)) - 1;
                string year5 = Convert.ToString(shortenddateyear2);
                string FYForLUMSUMyear4;
                if (MonthofStartYear > 3)
                {
                    FYForLUMSUMyear4 = Convert.ToInt32(startdateyear1) + "-" + (Convert.ToInt32(startdateyear1.Substring(2)) + 1);
                }
                else
                {
                    FYForLUMSUMyear4 = (Convert.ToInt32(startdateyear1) - 1) + "-" + (startdateyear1.Substring(2));
                }
                strHtml = strHtml.Replace("@Year", FYForLUMSUMyear4);

                int YearDuration = Convert.ToInt32(enddateyear2) - Convert.ToInt32(startdateyear1);
                string totalpremiumamount = ds.Tables[0].Rows[0]["vTotalPremium"].ToString();
                double totalpremiumamt = Convert.ToDouble(totalpremiumamount);
                double amount2 = totalpremiumamt / YearDuration;
                //string amount2 = Convert.ToString(amount1);
                double amount = Math.Round(amount2, 2);
                StringBuilder sb = new StringBuilder();
                sb.Append("<table style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri'>");
                sb.Append("<td style='width:200;border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                sb.Append("<p style='margin-left: 20px;'><span>Financial Year</span></p>");
                sb.Append("</td>");
                sb.Append("<td style='width:650;border:1px solid black;border-collapse:collapse;'>");
                sb.Append("<p style='margin-left: 20px;'><span>Year wise proportionate premium allowed for Deduction under Section 80D</span></p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                string FYForYearWiseLumsumDividendYear02;
                if (MonthofStartYear > 3)
                {
                    FYForYearWiseLumsumDividendYear02 = startdateyear1;
                }
                else
                {
                    int Yeart = Convert.ToInt32(startdateyear1) - 1;
                    FYForYearWiseLumsumDividendYear02 = Convert.ToString(Yeart);
                }
                for (int H = 0; H < YearDuration; H++)
                {
                    DataTable dt1 = new DataTable();
                    sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");

                    int Year00 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02) + H;
                    int sum = H + 1;
                    int Year01 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02.Substring(2)) + sum;

                    string year6 = Convert.ToString(Year00) + "-" + Convert.ToString(Year01);

                    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:200;'>");
                    sb.Append("<p style='margin-left: 20px;'> " + year6 + " </p>");
                    sb.Append("</td>");
                    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:650;'>");
                    sb.Append("<p style='margin-left: 20px;'> " + amount + " </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                strHtml = strHtml.Replace("@testHTMLTABLE", sb.ToString());
                #endregion

                strHtml = strHtml.Replace("@KotakGroupSmartCashUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupSmartCashUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupSmartCashUIN"]));

                #region TaxInvoice
                //CR_450_Start Kuwar
                //HDC_Floater_PDF_NonEmail
                StringBuilder taxinvoice = new StringBuilder();
                if (ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString() == "")
                {

                    taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                    int temp = 0;
                    string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                    string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                    string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                    string totalPremium = ds.Tables[0].Rows[0]["vTotalPremium"].ToString();
                    // int temp = Convert.ToInt32(totalPremium.Substring(0, totalPremium.IndexOf('.')));
                    if (totalPremium.Contains('.'))
                    {
                        temp = Convert.ToInt32(totalPremium.Substring(0, totalPremium.IndexOf('.')));

                    }
                    else
                    {
                        temp = Convert.ToInt32(totalPremium);
                    }
                    string totalPremiumInWord = ConvertAmountInWord(temp);
                    //string stateCode = 

                    // QR Code
                    string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                    string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                    string transactionDate = ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString();
                    int noofHSNCode = 0;
                    string hsnCode = ds.Tables[0].Rows[0]["vSacCode"].ToString();
                    string receiptNumber = ds.Tables[0].Rows[0]["vChallanNo"].ToString();
                    if (hsnCode != "")
                    {
                        var tempcount = hsnCode.Split(' ').Length;
                        for (int i = 0; i < tempcount; i++)
                        {
                            noofHSNCode++;
                        }

                    }
                    string Imagepath = string.Empty;
                    CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                    string kgiStateCode = suppliGSTN.Substring(0, 2); // getting kgi state code 
                    string kgiStateName = string.Empty;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                        cmd.Connection = con;
                        //sqlCon.Open();
                        object objStaeName = cmd.ExecuteScalar();
                        kgiStateName = Convert.ToString(objStaeName);
                    }
                    Imagepath = Imagepath == "error" ? "" : Imagepath;
                    strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);


                    strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                    //HDC Policy
                    strHtml = strHtml.Replace("@gistinno", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                    strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                    strHtml = strHtml.Replace("@customername", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                    strHtml = strHtml.Replace("@emailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                    strHtml = strHtml.Replace("@contactno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                    strHtml = strHtml.Replace("@address", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());
                    strHtml = strHtml.Replace("@imdcode", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                    strHtml = strHtml.Replace("@receiptno", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
                    strHtml = strHtml.Replace("@customerstatecode", custStateCode);
                    strHtml = strHtml.Replace("@supplyname", ds.Tables[0].Rows[0]["vProposerState"].ToString());

                    strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
                    strHtml = strHtml.Replace("@name", kgiName);
                    strHtml = strHtml.Replace("@panNo", kgiPanno);
                    strHtml = strHtml.Replace("@cinNo", kgiCINno);

                    strHtml = strHtml.Replace("@vKGIBranchAddress", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
                    strHtml = strHtml.Replace("@invoicedate", ds.Tables[0].Rows[0]["vpolicyStartDate"].ToString());
                    strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                    strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["vAddCol1"].ToString());

                    strHtml = strHtml.Replace("@partnerappno", "");// this column is there as per jay
                    strHtml = strHtml.Replace("@statecode", kgiStateCode);//gst state code of kotak
                    strHtml = strHtml.Replace("@statename", kgiStateName);//gst state code of kotak
                    strHtml = strHtml.Replace("@irn", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                    //HDC Policy
                    strHtml = strHtml.Replace("@hsnDescription", ds.Tables[0].Rows[0]["vDescription"].ToString());
                    strHtml = strHtml.Replace("@HSNCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
                    strHtml = strHtml.Replace("@totalpremium", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());
                    strHtml = strHtml.Replace("@netamount", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                    strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                    strHtml = strHtml.Replace("@totalgst", ds.Tables[0].Rows[0]["vTotalGSTAmount"].ToString());

                    strHtml = strHtml.Replace("@cgstpercent", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
                    strHtml = strHtml.Replace("@ugstpercent", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
                    strHtml = strHtml.Replace("@sgstpercent", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
                    strHtml = strHtml.Replace("@igstpercent", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());
                    //HDC Policy
                    strHtml = strHtml.Replace("@cgstamt", ds.Tables[0].Rows[0]["vCGST"].ToString());
                    strHtml = strHtml.Replace("@ugstamt", ds.Tables[0].Rows[0]["vUGST"].ToString());
                    strHtml = strHtml.Replace("@sgstamt", ds.Tables[0].Rows[0]["vSGST"].ToString());
                    strHtml = strHtml.Replace("@igstamt", ds.Tables[0].Rows[0]["vIGST"].ToString());

                    strHtml = strHtml.Replace("@cessrate", "0");
                    strHtml = strHtml.Replace("@cessamt", "0");
                    strHtml = strHtml.Replace("@totalgross", totalPremium);// change1
                    strHtml = strHtml.Replace("@totalamount", totalPremium);
                    strHtml = strHtml.Replace("@totalinvoicevalueinwords", totalPremiumInWord.ToString());
                }
                else
                {

                    taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: none'>");

                    strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                }
                //CR_450_End_Kuwar HDC Policy
                #endregion
                // below code for download pdf
                TextWriter outTextWriter = new StringWriter();
                HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                //base.Render(outHtmlTextWriter);
                string currentPageHtmlString = strHtml; //outTextWriter.ToString();
                                                        // Create a HTML to PDF converter object with default settings
                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
                // Set license key received after purchase to use the converter in licensed mode
                // Leave it not set to use the converter in demo mode
                string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();
                htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";
                                                                 // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                                                                 // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                htmlToPdfConverter.ConversionDelay = 2;
                // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
                // Add Header
                // Enable header in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                // Optionally add a space between header and the page body
                // The spacing for first page and the subsequent pages can be set independently
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");
                // Draw header elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                    DrawHeader(htmlToPdfConverter, false);
                // Add Footer
                // Enable footer in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
                // Optionally add a space between footer and the page body
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");
                // Draw footer elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                    DrawFooter(htmlToPdfConverter, false, true);
                // Use the current page URL as base URL
                string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                //Convert the current page HTML string to a PDF document in a memory buffer
                //// For Live
                byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                //// For Live End Here 

                //// For Dev
                //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                //// byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                //// For Dev End here 

                // saving schedule copy 
                string filelocation = ConfigurationManager.AppSettings["KotakPolicySchedules"].ToString();
                string filename = filelocation + "\\" + certificateNo + ".pdf";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                File.WriteAllBytes(filename, outPdfBuffer);


                // Send the PDF to user Email ID if valid Email ID available.

                if (string.IsNullOrEmpty(emailId))
                {
                    Alert.Show("Email ID is blank");
                }
                else if (Regex.IsMatch(emailId.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                {
                    //fnSendEmail(outPdfBuffer, emailId, CustomerName, certificateNo);
                    fnsendmail(filename, emailId, CustomerName, certificateNo);

                    //Alert.Show(" Policy sent to " + emailId);
                    //return;
                }
                else
                {
                    Alert.Show(" Invalid Email ID " + emailId);
                }

            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, "Error in GenerateNonEmailHDC_Flotaer_PDF CertificateNo " + certificateNo
                    + "  and Email ID " + emailId + "  " + ex.ToString() + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while gereating HDC Floater Schedule");
            }
        }

        private void fnsendmail(string filename, string emailId, string customerName, string certificateNo)
        {
            try
            {
                //string emailId = txtEmailforPolicy.Text.Trim();
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
                MailBody = MailBody.Replace("#FullCustomerName", customerName);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                mm.Subject = string.Format(ConfigurationManager.AppSettings["Schedule_Policy_email_Subject"], certificateNo);
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(filename);
                mm.Attachments.Add(attachment);


                smtpClient.Send(mm);
                fnAddLogGistPolicyDownload(emailId, customerName, certificateNo, Session["vUserLoginId"].ToString().ToUpper(), "Email");
                //Alert.Show("Policy schedule sent on Email ID " + emailId);
                //Response.Write(string.Format("<script>alert('Email sent successfully to {0} ')</script>",emailId) );
                IsEmailRequest = true;
                Session["ErrorCallingPage"] = "FrmDownloadPolicyScheduleNew.aspx";
                string vStatusMsg = "Policy sent to " + emailId;
                EmailSentMessage = vStatusMsg;
                //Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;

            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, "Error in fnSendMail certificate number  " + certificateNo + "    "
                    + ex.ToString() + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while sending email to " + emailId);
            }
        }

        private void fnSendEmail(byte[] objByte, string emailId, string CustomerName, string certificateNo)
        {
            //File.AppendAllText(logFile, "fnSendEmail : Sending Email certificate number  " + certificateNo + " , email id  " + emailId
            //    + " " + System.DateTime.Now.ToString() + " " + System.Environment.NewLine);
            try
            {
                //string emailId = txtEmailforPolicy.Text.Trim();
                string strPath = string.Empty;
                string MailBody = string.Empty;
                string filename = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "Schedule_Policy_Email_Body.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#FullCustomerName", CustomerName);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                mm.Subject = string.Format(ConfigurationManager.AppSettings["Schedule_Policy_email_Subject"], certificateNo);
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;




                MemoryStream memoryStream = new MemoryStream(objByte);
                mm.Attachments.Add(new Attachment(memoryStream, certificateNo + ".pdf", MediaTypeNames.Application.Pdf));

                smtpClient.Send(mm);
                fnAddLogGistPolicyDownload(emailId, CustomerName, certificateNo, Session["vUserLoginId"].ToString().ToUpper(), "Email");
                Alert.Show("Policy schedule sent on Email ID " + emailId);
                //File.AppendAllText(logFile, "fnSendEmail : Sending Email certificate number  " + certificateNo + " , email id  " + emailId
                //    + " " + System.DateTime.Now.ToString() + " " + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, "Error in fnSendMail certificate number  " + certificateNo + "    "
                    + ex.ToString() + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while sending email to " + emailId);
            }
        }

        private void fnAddLogGistPolicyDownload(string vEmailId, string vCustomerName, string vCertificateNo, string vUserloginID, string vMode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_INSERT_TBL_LOG_GISTPOLICYDOWNLOAD", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vEmailId", vEmailId);
                        cmd.Parameters.AddWithValue("@vCustomerName", vCustomerName);
                        cmd.Parameters.AddWithValue("@vCertificateNo", vCertificateNo);
                        cmd.Parameters.AddWithValue("@vUserloginID", vUserloginID);
                        cmd.Parameters.AddWithValue("@vMode", vMode);
                        //CR: 431 Added By Rajesh Soni 20/02/2020
                        cmd.Parameters.AddWithValue("@vSource", CommonExtensions.fn_Get_Browser_Info()["Name"].ToString());
                        cmd.Parameters.AddWithValue("@vIPAddress", CommonExtensions.GetLocalIPAddress());
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, "Error in fnAddLogGistPolicy " +
                    ex.ToString() + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured. ");
            }
        }

        protected void gvPolicyData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvPolicyData.PageIndex = e.NewPageIndex;
            // gvPolicyData.DataBind();
            if (ddlPrimaryParameter.SelectedValue == "Policy Number")
            {
                ViewState["PolicyNumber"] = txtPrimaryParameter.Text.Trim();
            }
            else if (ddlPrimaryParameter.SelectedValue == "Certificate Number")
            {
                ViewState["certificateNo"] = txtPrimaryParameter.Text.Trim();
            }
            else if (ddlPrimaryParameter.SelectedValue == "CRN Number")
            {
                ViewState["CrnNo"] = txtPrimaryParameter.Text.Trim();
            }
            else if (ddlPrimaryParameter.Text == "Account Number")
            {
                ViewState["AccountNumber"] = txtPrimaryParameter.Text.Trim();
            }
            else if (ddlPrimaryParameter.Text == "Loan Account Number")
            {
                ViewState["LoanNumber"] = txtPrimaryParameter.Text.Trim();
            }
            else if (ddlPrimaryParameter.Text == "Group Unique Identification Number")
            {
                ViewState["GroupUniqueIdentificationNumber"] = txtPrimaryParameter.Text.Trim();
            }
            if (ddlSecondaryParameter.Text == "Date Of Birth")
            {
                ViewState["DOB"] = txtDOB.Text;
            }
            else if (ddlSecondaryParameter.Text == "Registered Mobile Number")
            {
                ViewState["RegisteredMobileNumber"] = txtSecondaryParameter.Text.Trim();
                if (ViewState["RegisteredMobileNumber"].ToString().Length > 10 || ViewState["RegisteredMobileNumber"].ToString().Length < 10)
                {

                    Alert.Show(" Please enter Valid 10 digit registered mobile number");
                    return;
                }

            }
            else if (ddlSecondaryParameter.Text == "Registered Email ID")
            {
                ViewState["RegisteredEmailID"] = txtSecondaryParameter.Text.Trim();
            }
            string Certificate = ViewState["certificateNo"] != null ? ViewState["certificateNo"].ToString() : "";
            string CRN = ViewState["CrnNo"] != null ? ViewState["CrnNo"].ToString() : "";
            string LoanNumber = ViewState["LoanNumber"] != null ? ViewState["LoanNumber"].ToString() : "";
            string PolicyNumber = ViewState["PolicyNumber"] != null ? ViewState["PolicyNumber"].ToString() : "";
            string AccountNumber = ViewState["AccountNumber"] != null ? ViewState["AccountNumber"].ToString() : "";
            string GroupUniqueIdentificationNumber = ViewState["GroupUniqueIdentificationNumber"] != null ? ViewState["GroupUniqueIdentificationNumber"].ToString() : "";
            string DOB = ViewState["DOB"] != null ? ViewState["DOB"].ToString() : "";
            string RegisteredMobileNumber = ViewState["RegisteredMobileNumber"] != null ? ViewState["RegisteredMobileNumber"].ToString() : "";
            string RegisteredEmailID = ViewState["RegisteredEmailID"] != null ? ViewState["RegisteredEmailID"].ToString() : "";
            fnSearchPolicyDetails(PolicyNumber, Certificate, CRN, LoanNumber, AccountNumber, GroupUniqueIdentificationNumber, RegisteredMobileNumber, DOB, RegisteredEmailID);

        }


        private string fnGetCustomerName(string certificateNo, string v)
        {
            string customerName = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand();
                    if (v == "GPA")
                    {
                        cmd.CommandText = "select vCustomerName from dbo.TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "'  and dCreatedDate > '2017-07-01' and vuploadID not like '%canc%'"; //gst condition

                    }
                    if (v == "HDC")
                    {
                        cmd.CommandText = "select vCustomerName from dbo.TBL_HDC_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "'  and dCreatedDate > '2017-07-01' and vuploadID not like '%canc%'"; //gst condition
                    }

                    cmd.Connection = con;
                    object cmdReturn = cmd.ExecuteScalar();
                    customerName = Convert.ToString(cmdReturn);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, "fnGetCustomerName ::Error occured  : certificate number " + certificateNo
                    + "     " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while getting customer name from policy details.");
            }
            return customerName;
        }

        private string ConvertAmountInWord(int TotalPremium)
        {
            //  TotalPremiumInWord = "";

            string[] word = { "zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten" ,
                              "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifiteen", "Sixteen", "SevenTeen", "Eighteen", "Nineteen" };
            string[] numbers = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            string totalAmountInWord = string.Empty;
            //  int totalpremium = Convert.ToInt32(TotalPremium);

            try
            {


                if (TotalPremium == 0)
                {
                    return "";
                }
                else if (TotalPremium < 19)
                {
                    totalAmountInWord = word[TotalPremium];
                    return totalAmountInWord;
                }
                else if (TotalPremium < 100)
                {
                    totalAmountInWord = numbers[TotalPremium / 10 - 2] + ((TotalPremium % 10 > 0) ? "" + ConvertAmountInWord(TotalPremium % 10) : "");
                    return totalAmountInWord;

                }
                else if (TotalPremium < 1000)
                {
                    totalAmountInWord = word[TotalPremium / 100] + " Hundred " + ((TotalPremium % 100 > 0) ? "" + ConvertAmountInWord(TotalPremium % 100) : "");
                    return totalAmountInWord;

                }
                else if (TotalPremium < 100000)
                {
                    totalAmountInWord = ConvertAmountInWord(TotalPremium / 1000) + "   Thousand   " + ((TotalPremium % 1000 > 0) ? "" + ConvertAmountInWord(TotalPremium % 1000) : "");
                    return totalAmountInWord;

                }
                else if (TotalPremium < 100000)//less than lakh
                {
                    totalAmountInWord = ConvertAmountInWord(TotalPremium / 1000) + "   Thousand  " + ConvertAmountInWord(TotalPremium % 1000);
                    return totalAmountInWord;
                }
                else if (TotalPremium < 10000000)//upto ten lakh
                {
                    totalAmountInWord = ConvertAmountInWord(TotalPremium / 100000) + "  Lakh  " + ConvertAmountInWord(TotalPremium % 100000);
                    return totalAmountInWord;
                }
                else
                {
                    return totalAmountInWord = "conversion failed";
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, "ConvertAmountInword ::Error occured  : certificate number " + certificateNo
                    + "     " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);

                return totalAmountInWord = "Conversion Failed Please check log for More details";
            }
        }
        //CR_450_End Tax Invoice Kuwar


        private void CreateQRCodeImage(string certificateno, string supplierGSTN, string buyerGSTN, string transactionDate, int noOfLines, string sacCode, string receiptno, out string strQRCodeImageSRC)
        {
            try
            {
                string documentType = "Tax Invoice";
                strQRCodeImageSRC = ConfigurationManager.AppSettings["QRCodeImageSRC"].ToString();
                string strQRCodeString = "GSTN of Supplier: " + supplierGSTN + Environment.NewLine +
                                        "GSTN of Buyer: " + buyerGSTN + Environment.NewLine +
                                        "Document Number: " + certificateno + Environment.NewLine +
                                        "Document Type : " + documentType + Environment.NewLine +
                                        "Date of Creation of Invoice : " + transactionDate + Environment.NewLine +
                                        "No of Lines: " + noOfLines + Environment.NewLine +
                                        "HSN code: " + sacCode + Environment.NewLine +
                                        "IRN :" + " " + Environment.NewLine +
                                        "Premium Receipt Number: " + receiptno;


                string imagePath = Server.MapPath("~/KotakBundledPolicyQRCodeImgFiles/QRCode/");
                // string imagePath = ConfigurationManager.AppSettings["qrCodeFilePath"].ToString();
                MessagingToolkit.QRCode.Codec.QRCodeEncoder qRCodeEncoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
                qRCodeEncoder.QRCodeVersion = 0;
                qRCodeEncoder.QRCodeEncodeMode = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
                qRCodeEncoder.QRCodeErrorCorrect = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.Q;
                Bitmap bitmap = qRCodeEncoder.Encode(strQRCodeString);
                string qrImagePath = imagePath + " " + certificateno + ".png";
                strQRCodeImageSRC = strQRCodeImageSRC + " " + certificateno + ".png";

                bitmap.Save(qrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                // ExceptionUtility.LogException(ex, "CreateQRCodeImage");
                strQRCodeImageSRC = "error";
                File.AppendAllText(logFile, "CreateQRCodeImage ::Error occured  : certificate number " + certificateNo
                   + "     " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while getting Barcode on policy certificate.");

            }
        }


        private void DrawHeader(HtmlToPdfConverter htmlToPdfConverter, bool drawHeaderLine)
        {
            string headerHtmlUrl = IsWithoutHeaderFooter ? Server.MapPath("~/Header_HTML_WithoutHeader.html") : Server.MapPath("~/Header_HTML.html");

            // Set the header height in points
            htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 60;

            // Set header background color
            System.Drawing.Color colour = IsWithoutHeaderFooter ? ColorTranslator.FromHtml("#ffffff") : ColorTranslator.FromHtml("#ec3237"); ;
            htmlToPdfConverter.PdfHeaderOptions.HeaderBackColor = colour; // System.Drawing.Color.Red;

            // Create a HTML element to be added in header
            HtmlToPdfElement headerHtml = new HtmlToPdfElement(headerHtmlUrl);

            // Set the HTML element to fit the container height
            headerHtml.FitHeight = true;

            // Add HTML element to header
            htmlToPdfConverter.PdfHeaderOptions.AddElement(headerHtml);

            if (drawHeaderLine)
            {
                // Calculate the header width based on PDF page size and margins
                float headerWidth = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width -
                            htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin;

                // Calculate header height
                float headerHeight = htmlToPdfConverter.PdfHeaderOptions.HeaderHeight;

                // Create a line element for the bottom of the header
                LineElement headerLine = new LineElement(0, headerHeight - 1, headerWidth, headerHeight - 1);

                // Set line color
                headerLine.ForeColor = System.Drawing.Color.Gray;

                // Add line element to the bottom of the header
                htmlToPdfConverter.PdfHeaderOptions.AddElement(headerLine);
            }
        }


        private void DrawFooter(HtmlToPdfConverter htmlToPdfConverter, bool addPageNumbers, bool drawFooterLine)
        {
            string footerHtmlUrl = IsWithoutHeaderFooter ? Server.MapPath("~/Header_HTML_WithoutFooter.html") : Server.MapPath("~/Footer_HTML.html");
            // Set the footer height in points
            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 60;

            // Set footer background color
            htmlToPdfConverter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.White;

            // Create a HTML element to be added in footer
            HtmlToPdfElement footerHtml = new HtmlToPdfElement(footerHtmlUrl);

            // Set the HTML element to fit the container height
            footerHtml.FitHeight = true;

            // Add HTML element to footer
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerHtml);

            // Add page numbering
            if (addPageNumbers)
            {
                // Create a text element with page numbering place holders &p; and & P;
                TextElement footerText = new TextElement(0, 30, "Page &p; of &P;  ",
                    new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));

                // Align the text at the right of the footer
                footerText.TextAlign = HorizontalTextAlign.Right;

                // Set page numbering text color
                footerText.ForeColor = System.Drawing.Color.Navy;

                // Embed the text element font in PDF
                footerText.EmbedSysFont = true;

                // Add the text element to footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerText);
            }

            drawFooterLine = IsWithoutHeaderFooter ? false : true;
            if (drawFooterLine)
            {
                // Calculate the footer width based on PDF page size and margins
                float footerWidth = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width -
                            htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin;

                // Create a line element for the top of the footer
                LineElement footerLine = new LineElement(0, 0, footerWidth, 0);

                // Set line color
                footerLine.ForeColor = System.Drawing.Color.Gray;

                // Add line element to the bottom of the footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerLine);
            }
        }


        private void fnGenerateHDCSchedule(string certificateNo)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ToString()))
            {
                con.Open();
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_PDF_CompleteLetter.html";
                string htmlBody = File.ReadAllText(strPath);
                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(sw.ToString());
                string strHtml = htmlBody;

                SqlCommand command = new SqlCommand("PROC_GET_COVER_SECTION_DATA_FOR_HDC_PDF_TEST", con);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                command.Parameters.AddWithValue("@vCertificateNo", certificateNo);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_Floater_PDF_CompleteLetter.html";
                        htmlBody = File.ReadAllText(strPath);
                        sw = new StringWriter();
                        sr = new StringReader(sw.ToString());
                        strHtml = htmlBody;

                        // GenerateNonEmailHDC_Flotaer_PDF(con, ds, strHtml, certificateNo);
                        GenerateNonEmailHDC_Flotaer_PDF(con, ds, strHtml, certificateNo);

                    }
                }
                //File.AppendAllText(folderPath + "\\log.txt", "html body created" + Environment.NewLine);
            }
        }


        protected void gvPolicyData_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "SendMail")
            {
                string EmailId = Convert.ToString(e.CommandArgument);
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rowIndex = gvRow.RowIndex;
                string certificateNo = gvRow.Cells[0].Text;
                string CustomerName = gvRow.Cells[1].Text;
                string ProductName = gvRow.Cells[4].Text;
                string ProductCode = gvRow.Cells[5].Text;

                //
                if (ProductName.Contains("KOTAK GROUP ACCIDENT PROTECT"))
                {
                    fnGenerateGPA_Protect_Schedule(certificateNo, EmailId, CustomerName);
                }
                else if (ProductName.Contains("GROUP ACCIDENT CARE"))
                {
                    #region PDF for Care
                    //File.AppendAllText(logFile, " if (productName.Contains(GROUP ACCIDENT CARE)) for gpa care certificate :" + certificateNo + " " + DateTime.Now + Environment.NewLine);
                    string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_With_GST_Test_HeaderFooter.html";
                    string htmlBody = File.ReadAllText(strPath);
                    string custStateCode = string.Empty;
                    string PolicyIssuingOfficeAddress = string.Empty;

                    string IntermediaryName = string.Empty;
                    string IntermediaryCode = string.Empty;
                    string IntermediaryLandline = string.Empty;
                    string IntermediaryMobile = string.Empty;

                    string PolicyholderName = string.Empty;
                    string PolicyholderAddress = string.Empty;
                    string PolicyholderAddress2 = string.Empty;
                    string PolicyholderBusinessDescription = string.Empty;
                    string PolicyholderTelephoneNumber = string.Empty;
                    string PolicyholderEmailAddress = string.Empty;
                    string PolicyNumber = string.Empty;
                    string PolicyInceptionDateTime = string.Empty;
                    string PolicyExpiryDateTime = string.Empty;
                    string TotalNumberOfInsuredPersons = string.Empty;

                    string RowCoverHeader = string.Empty;
                    string SectionARow = string.Empty;
                    string ExtSectionARow = string.Empty;
                    string SectionBRow = string.Empty;

                    string NameofInsuredPerson = string.Empty;
                    string DateOfBirth = string.Empty;
                    string Gender = string.Empty;
                    string emailId = string.Empty;
                    string MobileNo = string.Empty;
                    string SumInsured = string.Empty;
                    string NomineeDetails = string.Empty;
                    string SectionACoverPremium = string.Empty;
                    string ExtensionstoSectionASectionBCoverPremium = string.Empty;
                    string LoadingsDiscounts = string.Empty;
                    string ServiceTax = string.Empty;
                    string SwachhBharatCess = string.Empty;
                    string KrishiKalyanCess = string.Empty;
                    string NetPremiumRoundedOff = string.Empty;
                    string StampDuty = string.Empty;
                    string Receipt_Challan_No = string.Empty;
                    string Receipt_Challan_No_Dated = string.Empty;
                    string PolicyIssueDate = string.Empty;
                    string TotalAmount = string.Empty;
                    bool IsCertificateNumberExists = false;
                    //string prod_name = string.Empty;
                    //gst changes
                    string ugstPercentage = string.Empty;
                    string ugstAmount = string.Empty;
                    string cgstPercentage = string.Empty;
                    string cgstAmount = string.Empty;
                    string sgstPercentage = string.Empty;
                    string sgstAmount = string.Empty;
                    string igstPercentage = string.Empty;
                    string igstAmount = string.Empty;
                    string totalGSTAmount = string.Empty;
                    string vProposerPinCode = string.Empty;
                    string addCol1 = string.Empty;
                    string polStartDate = string.Empty;
                    string createdDate = string.Empty;
                    string address1 = string.Empty;
                    string address2 = string.Empty;
                    string address3 = string.Empty;
                    string certNo = string.Empty;
                    string UINNo = string.Empty;
                    string placeOfSupply = string.Empty;
                    string proposalNo = string.Empty;

                    GetGPACertificateDetails(ref PolicyIssuingOfficeAddress
       , ref IntermediaryName
       , ref IntermediaryCode
       , ref PolicyholderName
       , ref PolicyholderAddress
       , ref PolicyholderAddress2
       , ref PolicyholderBusinessDescription
       , ref PolicyholderTelephoneNumber
       , ref PolicyholderEmailAddress
       , ref PolicyNumber
       , ref PolicyInceptionDateTime
       , ref PolicyExpiryDateTime
       , ref TotalNumberOfInsuredPersons
       , ref RowCoverHeader
       , ref SectionARow
       , ref ExtSectionARow
       , ref SectionBRow
       , ref NameofInsuredPerson
       , ref DateOfBirth
       , ref Gender
       , ref emailId
       , ref MobileNo
       , ref SumInsured
       , ref NomineeDetails
       , ref SectionACoverPremium
       , ref ExtensionstoSectionASectionBCoverPremium
       , ref LoadingsDiscounts
       , ref ServiceTax
       , ref SwachhBharatCess
       , ref KrishiKalyanCess
       , ref NetPremiumRoundedOff
       , ref StampDuty
       , ref Receipt_Challan_No
       , ref Receipt_Challan_No_Dated
       , ref PolicyIssueDate
       , ref IntermediaryLandline
       , ref IntermediaryMobile
       , ref TotalAmount
       , ref IsCertificateNumberExists
        // , certNo
        , ref ugstPercentage
           , ref ugstAmount
           , ref cgstPercentage
           , ref cgstAmount
           , ref igstPercentage
           , ref igstAmount
           , ref sgstPercentage
           , ref sgstAmount
           , ref totalGSTAmount
           , ref vProposerPinCode
           , ref addCol1
           , ref polStartDate
           , ref createdDate
           , ref address1
           , ref address2
           , ref address3
           , ref UINNo
            , ref placeOfSupply
             , ref proposalNo);
                    StringWriter sw = new StringWriter();
                    StringReader sr = new StringReader(sw.ToString());

                    string strHtml = htmlBody;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + vProposerPinCode + "'";
                            cmd.Connection = con;
                            con.Open();
                            object objCustState = cmd.ExecuteScalar();
                            custStateCode = Convert.ToString(objCustState);
                        }
                    }

                    strHtml = strHtml.Replace("@PolicyIssuingOfficeAddress", PolicyIssuingOfficeAddress);
                    strHtml = strHtml.Replace("@IntermediaryName", IntermediaryName);
                    strHtml = strHtml.Replace("@IntermediaryCode", IntermediaryCode);

                    strHtml = strHtml.Replace("@IntermediaryLandline", IntermediaryLandline);
                    strHtml = strHtml.Replace("@IntermediaryMobile", IntermediaryMobile);

                    strHtml = strHtml.Replace("@PolicyholderName", PolicyholderName);
                    strHtml = strHtml.Replace("@PolicyholderAddress", PolicyholderAddress);
                    string existPolicyholderAddress2 = string.Empty;
                    existPolicyholderAddress2 = PolicyholderAddress2.Replace("(stateCode)", "");
                    PolicyholderAddress2 = PolicyholderAddress2.Replace("stateCode", custStateCode);
                    strHtml = strHtml.Replace("@PolicyholderLine2Address", PolicyholderAddress2);
                    strHtml = strHtml.Replace("@PolicyholderBusinessDescription", PolicyholderBusinessDescription);
                    strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", PolicyholderTelephoneNumber);
                    strHtml = strHtml.Replace("@PolicyholderEmailAddress", PolicyholderEmailAddress);
                    //strHtml = strHtml.Replace("@PolicyNumber", PolicyNumber + "/" + certNo); //done changes for cert no

                    strHtml = strHtml.Replace("@PolicyNumber", certificateNo);
                    strHtml = strHtml.Replace("@PolicyInceptionDateTime", PolicyInceptionDateTime);
                    //manish start
                    strHtml = strHtml.Replace("@Enroll", PolicyInceptionDateTime.Substring(24));
                    //manish end
                    strHtml = strHtml.Replace("@PolicyExpiryDateTime", PolicyExpiryDateTime);
                    strHtml = strHtml.Replace("@TotalNumberOfInsuredPersons", TotalNumberOfInsuredPersons);

                    strHtml = strHtml.Replace("@RowCoverHeader", string.IsNullOrEmpty(RowCoverHeader) ? "" : RowCoverHeader);
                    strHtml = strHtml.Replace("@RowSectionA", string.IsNullOrEmpty(SectionARow) ? "" : SectionARow);
                    strHtml = strHtml.Replace("@RowExtSectionA", string.IsNullOrEmpty(ExtSectionARow) ? "" : ExtSectionARow);
                    strHtml = strHtml.Replace("@RowSectionB", string.IsNullOrEmpty(SectionBRow) ? "" : SectionBRow);

                    strHtml = strHtml.Replace("@NameofInsuredPerson", NameofInsuredPerson);
                    strHtml = strHtml.Replace("@DateOfBirth", DateOfBirth); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));
                    strHtml = strHtml.Replace("@Gender", Gender);
                    strHtml = strHtml.Replace("@EmailId", emailId);
                    strHtml = strHtml.Replace("@MobileNo", MobileNo);
                    strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@NomineeDetails", NomineeDetails);
                    strHtml = strHtml.Replace("@SectionACoverPremium", Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@ExtensionstoSectionASectionBCoverPremium", Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@LoadingsDiscounts", string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@ServiceTax", Convert.ToDecimal(ServiceTax).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@SwachhBharatCess", Convert.ToDecimal(SwachhBharatCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@KrishiKalyanCess", Convert.ToDecimal(KrishiKalyanCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@NetPremiumRoundedOff", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@StampDuty", Convert.ToDecimal(StampDuty).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@Receipt_Challan_No", Receipt_Challan_No);
                    strHtml = strHtml.Replace("@Challan_No_Dated", Receipt_Challan_No_Dated);
                    strHtml = strHtml.Replace("@PolicyIssueDate", PolicyIssueDate);
                    strHtml = strHtml.Replace("@TotalAmount", TotalAmount);

                    strHtml = strHtml.Replace("@masterPolicy", PolicyNumber);
                    strHtml = strHtml.Replace("@certificateNo", certificateNo);
                    //Added By Nilesh 
                    string _Date = createdDate;
                    DateTime dt = Convert.ToDateTime(_Date);
                    string FDate = dt.ToString("dd/MM/yyyy");
                    strHtml = strHtml.Replace("@createdDate", FDate);
                    //strHtml = strHtml.Replace("@createdDate", createdDate);
                    //End By Nilesh

                    strHtml = strHtml.Replace("@customerName", fnGetCustomerName(certificateNo, "GPA"));
                    strHtml = strHtml.Replace("@productName", "KOTAK GROUP ACCIDENT CARE");

                    strHtml = strHtml.Replace("@addressline1", address1);
                    strHtml = strHtml.Replace("@addressline2", address2);
                    strHtml = strHtml.Replace("@addressline3", address3);
                    strHtml = strHtml.Replace("@statepincode", existPolicyholderAddress2);
                    //Added By Nilesh for CR353
                    if (string.IsNullOrEmpty(Gender))
                    {
                        strHtml = strHtml.Replace("@salutation", "");
                    }
                    else
                    {
                        if (Gender.Trim() == "M" || Gender == "Male")
                        {
                            strHtml = strHtml.Replace("@salutation", "Mr.");
                        }
                        else if (Gender.Trim() == "F")
                        {
                            strHtml = strHtml.Replace("@salutation", "Mrs.");
                        }
                        else
                        {
                            strHtml = strHtml.Replace("@salutation", "");
                        }
                    }
                    //End By Nilesh for CR353


                    string customString = string.Empty;

                    if (!String.IsNullOrEmpty(addCol1))
                    {
                        string[] strArr = addCol1.Split(' ');
                        // customString = "this " + strArr[1] + " day of " + strArr[0] + " of " + strArr[2];

                        if (String.IsNullOrEmpty(strArr[1]))
                        {
                            customString = "this " + strArr[2] + " day of " + strArr[0] + " of year " + strArr[3];
                        }
                        else
                        {
                            customString = "this " + strArr[1] + " day of " + strArr[0] + " of year " + strArr[2];
                        }

                    }

                    strHtml = strHtml.Replace("@polIssueString", customString);

                    string igstData = string.Empty;
                    string cgstugstData = string.Empty;
                    string cgstsgstData = string.Empty;

                    if (igstPercentage != "0")
                    {
                        string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                        igstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px'><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>IGST@" + igstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='20px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + igstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                    }
                    else
                    {
                        if (cgstPercentage != "0" && ugstPercentage != "0")
                        {
                            string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                            cgstugstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px' ><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>SGST@" + sgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>UGST@" + ugstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='10px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + ugstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + cgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                        }
                        if (cgstPercentage != "0" && sgstPercentage != "0")
                        {
                            string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                            cgstsgstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='10px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px'><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>SGST@" + sgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='10px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + sgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + cgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                        }
                    }

                    strHtml = strHtml.Replace("@cgstugstData", cgstugstData == "" ? "" : cgstugstData);

                    strHtml = strHtml.Replace("@cgstsgstData", cgstsgstData == "" ? "" : cgstsgstData);

                    strHtml = strHtml.Replace("@igstData", igstData == "" ? "" : igstData);

                    strHtml = strHtml.Replace("@KotakGroupAccidentCareUIN", UINNo == "" ? "" : UINNo);

                    //CR_P1_450_Start Kuwar Tax Invoice_GPA_Policy 
                    #region TaxInvoiceGPAPolicy

                    //GPA_GenerateGPAProtectPDF()
                    StringBuilder taxinvoice = new StringBuilder();
                    taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                    int temp = 0;
                    string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                    string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                    string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                    string netPremium = TotalAmount;
                    if (netPremium.Contains('.'))
                    {
                        temp = Convert.ToInt32(netPremium.Substring(0, netPremium.IndexOf('.')));

                    }
                    else
                    {
                        temp = Convert.ToInt32(netPremium);
                    }

                    string NetPremiumInWord = ConvertAmountInWord(temp);

                    // QR Code
                    string suppliGSTN = ConfigurationManager.AppSettings["GstRegNo"].ToString();//hardcord value to be passs
                    // string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                    string buyerGSTN = "";
                    //string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                    string transactionDate = polStartDate;
                    int noofHSNCode = 0;
                    // string hsnCode = "";
                    string hsnCode = ConfigurationManager.AppSettings["SacCode"].ToString();//hardcode value to be pass
                    string receiptNumber = Receipt_Challan_No;
                    if (hsnCode != "")
                    {
                        var tempcount = hsnCode.Split(' ').Length;
                        for (int i = 0; i < tempcount; i++)
                        {
                            noofHSNCode++;
                        }

                    }
                    string Imagepath = string.Empty;
                    CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                    Imagepath = Imagepath == "error" ? "" : Imagepath;
                    string kgiStateCode = suppliGSTN.Substring(0, 2); // getting kgi state code 
                    string kgiStateName = string.Empty;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                            cmd.Connection = con;
                            con.Open();
                            object objStaeName = cmd.ExecuteScalar();
                            kgiStateName = Convert.ToString(objStaeName);
                        }

                    }

                    strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);


                    strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                    //GPA Policy
                    strHtml = strHtml.Replace("@gistinno", "");
                    strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                    strHtml = strHtml.Replace("@customername", PolicyholderName);
                    strHtml = strHtml.Replace("@emailId", EmailId);
                    strHtml = strHtml.Replace("@contactno", MobileNo);
                    strHtml = strHtml.Replace("@address", PolicyholderAddress);
                    strHtml = strHtml.Replace("@address1", PolicyholderAddress2);

                    strHtml = strHtml.Replace("@imdcode", IntermediaryCode);
                    strHtml = strHtml.Replace("@receiptno", Receipt_Challan_No);
                    strHtml = strHtml.Replace("@customerstatecode", custStateCode);

                    strHtml = strHtml.Replace("@kgistatecode", kgiStateCode);//gst state code of kotak uncomment
                    strHtml = strHtml.Replace("@kgistatename", kgiStateName);//gst state code of kotak

                    strHtml = strHtml.Replace("@supplyname", placeOfSupply);//gst state name require of customer
                    strHtml = strHtml.Replace("@name", kgiName);
                    strHtml = strHtml.Replace("@panNo", kgiPanno);
                    strHtml = strHtml.Replace("@cinNo", kgiCINno);

                    strHtml = strHtml.Replace("@invoicedate", polStartDate);
                    strHtml = strHtml.Replace("@invoiceno", certificateNo);
                    strHtml = strHtml.Replace("@proposalno", proposalNo);

                    strHtml = strHtml.Replace("@partnerappno", "");// this column is there as per jay
                                                                   // strHtml = strHtml.Replace("@irn", certNo);
                    strHtml = strHtml.Replace("@irn", certificateNo);
                    //GPA Policy
                    strHtml = strHtml.Replace("@totalpremium", TotalAmount);
                    strHtml = strHtml.Replace("@netamount", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@NetPremiumString", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@totalgst", totalGSTAmount);

                    strHtml = strHtml.Replace("@cgstpercent", cgstPercentage);
                    strHtml = strHtml.Replace("@ugstpercent", ugstPercentage);
                    strHtml = strHtml.Replace("@sgstpercent", sgstPercentage);
                    strHtml = strHtml.Replace("@igstpercent", igstPercentage);

                    //GPA Policy
                    strHtml = strHtml.Replace("@cgstamt", cgstAmount);
                    strHtml = strHtml.Replace("@ugstamt", ugstAmount);
                    strHtml = strHtml.Replace("@sgstamt", sgstAmount);
                    strHtml = strHtml.Replace("@igstamt", igstAmount);

                    strHtml = strHtml.Replace("@cessrate", "0");
                    strHtml = strHtml.Replace("@cessamt", SwachhBharatCess);

                    string tdservicetax = string.Empty;
                    string dataservicetax = string.Empty;

                    if (ServiceTax != "0" && !string.IsNullOrEmpty(ServiceTax))
                    {
                        tdservicetax = "<td style='border: 1px solid black' width='5%'><p style ='font-size:small'><strong>Service Tax</strong></p></td> ";
                        dataservicetax = "<td style ='border:1px solid black' width = '5%'><p> " + Convert.ToDecimal(ServiceTax).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td>";
                    }
                    strHtml = strHtml.Replace("@servicetx", tdservicetax == "" ? "" : tdservicetax);
                    strHtml = strHtml.Replace("@servictaxh", dataservicetax == "" ? "" : dataservicetax);

                    strHtml = strHtml.Replace("@totalgross", TotalAmount);// change1 

                    strHtml = strHtml.Replace("@totalinvoicevalueinfigure", TotalAmount);
                    strHtml = strHtml.Replace("@totalinvoicevalueinwords", NetPremiumInWord.ToString());
                    #endregion
                    //CR_450_End_Kuwar_Tax_Invoice GPA Policy

                    TextWriter outTextWriter = new StringWriter();
                    HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                    base.Render(outHtmlTextWriter);

                    string currentPageHtmlString = strHtml; //outTextWriter.ToString();

                    // Create a HTML to PDF converter object with default settings
                    HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                    // Set license key received after purchase to use the converter in licensed mode
                    // Leave it not set to use the converter in demo mode
                    string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();

                    htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

                    // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                    // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                    htmlToPdfConverter.ConversionDelay = 2;

                    // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                    htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                    // Add Header

                    // Enable header in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                    // Optionally add a space between header and the page body
                    // The spacing for first page and the subsequent pages can be set independently
                    // Leave this option not set for no spacing
                    htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                    htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");

                    // Draw header elements
                    if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                        DrawHeader(htmlToPdfConverter, false);

                    // Add Footer

                    // Enable footer in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                    // Optionally add a space between footer and the page body
                    // Leave this option not set for no spacing
                    htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                    // Draw footer elements
                    if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                        DrawFooter(htmlToPdfConverter, false, true);

                    // Use the current page URL as base URL
                    string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;


                    //// Convert the current page HTML string to a PDF document in a memory buffer
                    //// For Live
                    byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                    byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                    //// For Live End Here 


                    //For Dev
                    //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                    //byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                    // For Dev End here 
                    //Send Email ID to register Email ID 
                    // Send the PDF to user Email ID if valid Email ID available.

                    // saving schedule copy 
                    string filelocation = ConfigurationManager.AppSettings["KotakPolicySchedules"].ToString();
                    string filename = filelocation + "\\" + certificateNo + ".pdf";
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    File.WriteAllBytes(filename, outPdfBuffer);


                    if (string.IsNullOrEmpty(EmailId))
                    {
                        Alert.Show("Email ID is blank");
                    }

                    else if (Regex.IsMatch(EmailId.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                    {
                        //fnSendEmail(outPdfBuffer, EmailId.Trim() , CustomerName, certificateNo);
                        //File.AppendAllText(logFile, "calling fnsendmail filename " + filename + " Emailid " + EmailId.Trim() +
                        //    "Customer Name " + CustomerName + "    CertificateNo  " + certificateNo + "  " + DateTime.Now.ToString()
                        //    + System.Environment.NewLine);
                        fnsendmail(filename, EmailId.Trim(), CustomerName, certificateNo);

                        return;
                        //    Alert.Show(" Policy sent to " + EmailId);
                    }
                    else
                    {
                        Alert.Show(" Invalid Email ID " + EmailId);
                    }

                    #endregion
                }

                else if (ProductName.Contains("Smart Cash") || ProductName.Contains("Micro Insurance"))
                {
                    fnGenerateHDCSchedule(certificateNo, EmailId, CustomerName);
                }
                else
                // check product in GIST to send Email
                {
                    string ErrorMsg = string.Empty;
                    PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
                    //File.AppendAllText(logFile, System.Environment.NewLine + "Getting PDF byte brom GIST   certificateNo  " 
                    //    + certificateNo + ", ProductCode " + ProductCode + " " + DateTime.Now.ToString() + System.Environment.NewLine);
                    byte[] objByte = proxy.KGIGetPolicyDocumentForPortal("81062f2fc69b4639af5bf33e86c66408", certificateNo, ProductCode, ref ErrorMsg);
                    if (ErrorMsg.Length < 5)
                    {
                        fnSendEmail(objByte, EmailId, CustomerName, certificateNo);
                    }
                    else
                    {
                        File.AppendAllText(logFile, " gvPolicyData_RowCommand  Error while fetting pdf bytes  " + ErrorMsg + DateTime.Now + Environment.NewLine);
                        Alert.Show("Some error occurred. If you are facing this issue repetatively contact to PASS support team.");
                    }
                }
            }
        }

        private void fnGenerateGPA_Protect_Schedule(string certificateNo, string emailId, string customerName)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            try
            {
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();
                    string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_CompleteLetter_With_GST - Copy.html";
                    string htmlBody = File.ReadAllText(strPath);
                    StringWriter sw = new StringWriter();
                    StringReader sr = new StringReader(sw.ToString());
                    string strHtml = htmlBody;

                    SqlCommand command = new SqlCommand("PROC_GET_COVER_SECTION_DATA_FOR_PDF_TEST", con);
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                    command.Parameters.AddWithValue("@vCertificateNo", certificateNo);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            //  GenerateEmailPDF(con, ds, strHtml, certNo, emailID);

                            string accidentalDeath = string.Empty;
                            string permTotalDisable = string.Empty;
                            string permPartialDisable = string.Empty;
                            string tempTotalDisable = string.Empty;
                            string carraigeBody = string.Empty;
                            string funeralExpense = string.Empty;
                            string medicalExpense = string.Empty;
                            string purchaseBlood = string.Empty;
                            string transportation = string.Empty;
                            string compassionate = string.Empty;
                            string disappearance = string.Empty;
                            string modifyResidence = string.Empty;
                            string costOfSupport = string.Empty;
                            string commonCarrier = string.Empty;
                            string childrenGrant = string.Empty;
                            string marraigeExpense = string.Empty;
                            string sportsActivity = string.Empty;
                            string widowHood = string.Empty;

                            string ambulanceChargesString = string.Empty;
                            string dailyCashString = string.Empty;
                            string accidentalHospString = string.Empty;
                            string opdString = string.Empty;
                            string accidentalDentalString = string.Empty;
                            string convalString = string.Empty;
                            string burnsString = string.Empty;
                            string brokenBones = string.Empty;
                            string comaString = string.Empty;
                            string domesticTravelString = string.Empty;
                            string lossofEmployString = string.Empty;
                            string onDutyCover = string.Empty;
                            string legalExpenses = string.Empty;

                            string reducingCoverString = string.Empty;
                            string assignmentString = string.Empty;

                            //gst
                            string custStateCode = string.Empty;
                            string igstString = string.Empty;
                            string cgstsgstString = string.Empty;
                            string cgstugstString = string.Empty;

                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "'";
                                cmd.Connection = con;
                                //sqlCon.Open();
                                object objCustState = cmd.ExecuteScalar();
                                custStateCode = Convert.ToString(objCustState);
                            }

                            strHtml = strHtml.Replace("@createdDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dCreatedDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@customerName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeAge"].ToString());
                            strHtml = strHtml.Replace("@masterDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["vMasterPolicyDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());


                            strHtml = strHtml.Replace("@addressline1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@addressline2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());

                            strHtml = strHtml.Replace("@addressline3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                            strHtml = strHtml.Replace("@city", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                            strHtml = strHtml.Replace("@pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());
                            strHtml = strHtml.Replace("@state", ds.Tables[0].Rows[0]["vProposerState"].ToString());

                            strHtml = strHtml.Replace("@mobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@emailID", ds.Tables[0].Rows[0]["vEmailId"].ToString());

                            //strHtml = strHtml.Replace("@productName", ds.Tables[0].Rows[0]["vProductName"].ToString()); //done changes for cert no
                            //strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@productName", "KOTAK GROUP ACCIDENT PROTECT"); //done changes for cert no
                            strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vpolicyType"].ToString() == "" ? "New" : ds.Tables[0].Rows[0]["vpolicyType"].ToString());

                            //manish start
                            //   strHtml = strHtml.Replace("@Enroll", PolicyInceptionDateTime.Substring(24));
                            //manish end
                            strHtml = strHtml.Replace("@prevPolicyNo", ds.Tables[0].Rows[0]["vprevPolicyNumber"].ToString());
                            strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyLoc"].ToString());


                            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderAddress", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString()); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@PolicyholderLine2Address", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerCity"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerState"].ToString() + "(" + custStateCode + ")-" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                            strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderEmailAddress", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                            //  strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@policyStartDate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@policyEndDate", ds.Tables[0].Rows[0]["dPolicyEndDate"].ToString());

                            strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                            strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());


                            strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                            strHtml = strHtml.Replace("@occupation", ds.Tables[0].Rows[0]["vOccupation"].ToString());

                            strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                            strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["dCustomerDob"].ToString());
                            strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                            strHtml = strHtml.Replace("@category", "");
                            strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                            //strHtml = strHtml.Replace("@sumInsured", ds.Tables[0].Rows[0]["nPlanSI"].ToString());
                            //strHtml = strHtml.Replace("@sumInsured", Convert.ToDecimal(ds.Tables[0].Rows[0]["nPlanSI"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@sumInsured", "");


                            strHtml = strHtml.Replace("@sumBasis", ds.Tables[0].Rows[0]["vSIBasis"].ToString());

                            strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                            strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());

                            strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                            //strHtml = strHtml.Replace("@nomineeRelDOB", "");
                            strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vAppointeeName"].ToString());

                            string igstPercentage = ds.Tables[0].Rows[0]["igstPercentage"].ToString();
                            string cgstPercentage = ds.Tables[0].Rows[0]["cgstPercentage"].ToString();
                            string sgstPercentage = ds.Tables[0].Rows[0]["sgstPercentage"].ToString();
                            string ugstPercentage = ds.Tables[0].Rows[0]["ugstPercentage"].ToString();
                            string igstAmount = ds.Tables[0].Rows[0]["igstAmount"].ToString();
                            string cgstAmount = ds.Tables[0].Rows[0]["cgstAmount"].ToString();
                            string sgstAmount = ds.Tables[0].Rows[0]["sgstAmount"].ToString();
                            string ugstAmount = ds.Tables[0].Rows[0]["ugstAmount"].ToString();

                            if (igstPercentage != "0")
                            {
                                igstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center'>IGST@" + igstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'>" + igstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && sgstPercentage != "0")
                            {
                                cgstsgstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center'>SGST@" + sgstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'>" + sgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && ugstPercentage != "0")
                            {
                                cgstugstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center'>UGST@" + ugstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'>" + ugstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            strHtml = strHtml.Replace("@igstString", igstString == "" ? "" : igstString);
                            strHtml = strHtml.Replace("@cgstsgstString", cgstsgstString == "" ? "" : cgstsgstString);
                            strHtml = strHtml.Replace("@cgstugstString", cgstugstString == "" ? "" : cgstugstString);

                            string policyIssuance = ds.Tables[0].Rows[0]["vAdditional_column_1"].ToString();
                            string customString = string.Empty;

                            if (!String.IsNullOrEmpty(policyIssuance))
                            {
                                string[] strArr = policyIssuance.Split(' ');
                                if (String.IsNullOrEmpty(strArr[1]))
                                {
                                    customString = "this " + strArr[2] + " day of " + strArr[0] + " of year " + strArr[3];
                                }
                                else
                                {
                                    customString = "this " + strArr[1] + " day of " + strArr[0] + " of year " + strArr[2];
                                }

                            }

                            strHtml = strHtml.Replace("@polIssueString", customString);



                            if (ds.Tables[0].Rows[0]["vAccidentalDeathAD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString().Trim()))
                                {
                                    //accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString() + ")</p></td></tr>";

                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vPermTotalDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString().Trim()))
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString() + ")</p></td></tr> ";
                                }
                                else
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString())) + "</p></td></tr> ";
                                }
                            }
                            //
                            if (ds.Tables[0].Rows[0]["vPermPartialDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString().Trim()))
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vTempTotalDisableTTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString().Trim()))
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCarraigeDeadBody"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString().Trim()))
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vFuneralExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString().Trim()))
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalMedicalExp"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString().Trim()))
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vPurchaseofBlood"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPurchaseofBloodSIText"].ToString().Trim()))
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPurchaseofBloodSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vTransportationofImpMedicine"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTransportationofImpMedicineSIText"].ToString().Trim()))
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vTransportationofImpMedicineSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCompassionateVisit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString().Trim()))
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDisappearanceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString().Trim()))
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString() + "</p></td></tr>";
                                }

                            }

                            if (ds.Tables[0].Rows[0]["vModificationofVehicle"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString().Trim()))
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCostSupportItems"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString().Trim()))
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCommonCarrier"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString().Trim()))
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vChildEduGrant"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString().Trim()))
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vMarraigeExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString().Trim()))
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSportsActivityCover"].ToString() == "Y")
                            {
                                //if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vSportsActivityCoverSIText"].ToString()))
                                //{
                                //    sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>"+ ds.Tables[0].Rows[0]["nSportsActivityCoverSI"].ToString() + "&nbsp;("+ ds.Tables[0].Rows[0]["vSportsActivityCoverSIText"].ToString() + ")</p></td></tr>";
                                //}
                                //  else
                                //  {
                                //sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>"+ ds.Tables[0].Rows[0]["nSportsActivityCoverSI"].ToString() + "</p></td></tr>";
                                sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                                // }
                            }

                            if (ds.Tables[0].Rows[0]["vWidowhoodCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString().Trim()))
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAmbulanceCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString().Trim()))
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vDailyCashBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString().Trim()))
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalHospitalization"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString().Trim()))
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOPDTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString().Trim()))
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalDentalExpense"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString().Trim()))
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vConvalescenceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString().Trim()))
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBurnBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString().Trim()))
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBrokenBones"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString().Trim()))
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vComa"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vComaSIText"].ToString().Trim()))
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nComaSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vComaSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nComaSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDomesticTravelForTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString().Trim()))
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString().Trim()))
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOnDutyCover"].ToString() == "Y")
                            {
                                //if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOnDutyCoverSIText"].ToString()))
                                //{
                                //    onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOnDutyCoverSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vOnDutyCoverSIText"].ToString() + ")</p></td></tr>";
                                //}
                                //  else
                                //  {
                                //onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOnDutyCoverSI"].ToString() + "</p></td></tr>";
                                onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                                // }
                            }

                            if (ds.Tables[0].Rows[0]["vLegalExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString().Trim()))
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSIBasis"].ToString().ToLower() == "reducing")
                            {
                                reducingCoverString = "<tr><td style='border:1px solid black' width='10%'><p  style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>8.23-Reducing Sum Insured Covers</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vProposalType"].ToString().ToLower() == "credit linked")
                            {
                                assignmentString = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>8.22-Assignment</p></td></tr>";
                            }

                            #region commented code
                            //    strHtml = strHtml.Replace("@accidentalDeath", ds.Tables[0].Rows[0]["vAccidentalDeathAD"].ToString());
                            //  strHtml = strHtml.Replace("@permanentDisable", ds.Tables[0].Rows[0]["vPermTotalDisablePTD"].ToString());
                            // strHtml = strHtml.Replace("@permanentpartialDisable", ds.Tables[0].Rows[0]["vPermPartialDisablePTD"].ToString());
                            // strHtml = strHtml.Replace("@temptotalDisable", ds.Tables[0].Rows[0]["vTempTotalDisableTTD"].ToString());
                            //strHtml = strHtml.Replace("@carraige", ds.Tables[0].Rows[0]["vCarriageOfDeadBody"].ToString());
                            //strHtml = strHtml.Replace("@funeral", ds.Tables[0].Rows[0]["vFuneralExpenses"].ToString());
                            //strHtml = strHtml.Replace("@accMedicalExpense", ds.Tables[0].Rows[0]["vAccidentalMedicalExp"].ToString());
                            //strHtml = strHtml.Replace("@purchaseblood", ds.Tables[0].Rows[0]["vPurchaseofBlood"].ToString());

                            //strHtml = strHtml.Replace("@transportation", ds.Tables[0].Rows[0]["vTransportationofImpMedicine"].ToString());
                            //strHtml = strHtml.Replace("@compassionate", ds.Tables[0].Rows[0]["vCompassionateVisit"].ToString());
                            //strHtml = strHtml.Replace("@disappearance", ds.Tables[0].Rows[0]["vDisappearanceBenefit"].ToString());
                            //strHtml = strHtml.Replace("@modifyresidence", ds.Tables[0].Rows[0]["vModificationofVehicle"].ToString());
                            //strHtml = strHtml.Replace("@costofSupport", ds.Tables[0].Rows[0]["vCostOfSupportItems"].ToString());
                            //strHtml = strHtml.Replace("@commonCarrier", ds.Tables[0].Rows[0]["vCommonCarrier"].ToString());
                            //strHtml = strHtml.Replace("@childrenGrant", ds.Tables[0].Rows[0]["vChildEduGrant"].ToString());
                            //strHtml = strHtml.Replace("@marraigeexpense", ds.Tables[0].Rows[0]["vMarriageBenefitChild"].ToString());
                            //strHtml = strHtml.Replace("@sportsAcitivity", ds.Tables[0].Rows[0]["vSportsActivityCover"].ToString());
                            // strHtml = strHtml.Replace("@widowCover", ds.Tables[0].Rows[0]["vWidowhoodCover"].ToString());


                            //strHtml = strHtml.Replace("@ambulanceCharge", ds.Tables[0].Rows[0]["vAmbulanceCover"].ToString());
                            //strHtml = strHtml.Replace("@accidentalCash", ds.Tables[0].Rows[0]["vDailyCashBenefit"].ToString());
                            //strHtml = strHtml.Replace("@accidentalHospital", ds.Tables[0].Rows[0]["vAccidentalHospitalization"].ToString());
                            //strHtml = strHtml.Replace("@opdTreat", ds.Tables[0].Rows[0]["vOPDTreatment"].ToString());
                            //strHtml = strHtml.Replace("@accidentDental", ds.Tables[0].Rows[0]["vAccidentalDentalExpense"].ToString());

                            //strHtml = strHtml.Replace("@convalescence", ds.Tables[0].Rows[0]["vConvalescenceBenefit"].ToString());
                            //strHtml = strHtml.Replace("@burns", ds.Tables[0].Rows[0]["vBurns"].ToString());
                            //strHtml = strHtml.Replace("@brokenBones", ds.Tables[0].Rows[0]["vBrokenBones"].ToString());
                            //strHtml = strHtml.Replace("@coma", ds.Tables[0].Rows[0]["vComa"].ToString());
                            //strHtml = strHtml.Replace("@domesticTravel", ds.Tables[0].Rows[0]["vDomesticTravelForTreatment"].ToString());
                            //strHtml = strHtml.Replace("@lossOfEmploy", ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString());
                            //strHtml = strHtml.Replace("@onDutyCover", ds.Tables[0].Rows[0]["vOnDutyCover"].ToString());
                            //strHtml = strHtml.Replace("@legalExpenses", ds.Tables[0].Rows[0]["vLegalExpenses"].ToString());
                            #endregion

                            //         strHtml = strHtml.Replace("@premium", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@premium", Convert.ToDecimal(ds.Tables[0].Rows[0]["nNetPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            //strHtml = strHtml.Replace("@serviceTax", ds.Tables[0].Rows[0]["nServiceTax"].ToString());
                            strHtml = strHtml.Replace("@serviceTax", Convert.ToDecimal(ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            //strHtml = strHtml.Replace("@sbc", ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString());
                            //  strHtml = strHtml.Replace("@sbc", Convert.ToDecimal(ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            //strHtml = strHtml.Replace("@kkc", ds.Tables[0].Rows[0]["nKKC"].ToString());
                            //   strHtml = strHtml.Replace("@kkc", Convert.ToDecimal(ds.Tables[0].Rows[0]["nKKC"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@amount", Convert.ToDecimal(ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                            strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryNumber"].ToString());
                            strHtml = strHtml.Replace("@intermediaryLandline", "");
                            strHtml = strHtml.Replace("@challanDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dChallanDate"]).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            //strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["nStampDuty"].ToString());
                            strHtml = strHtml.Replace("@stampduty", Convert.ToDecimal(ds.Tables[0].Rows[0]["nStampDuty"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                            {
                                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash – Micro Insurance UIN:KOTHMGP076V011819");
                            }
                            else
                            {
                                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash UIN: KOTHLGP19014V011819");
                            }



                            //  if (!String.IsNullOrEmpty(accidentalDeath))
                            //   {
                            strHtml = strHtml.Replace("@accidentalDeathString", accidentalDeath == "" ? "" : accidentalDeath);
                            //  }

                            //   if (!String.IsNullOrEmpty(permTotalDisable))
                            //   {
                            strHtml = strHtml.Replace("@permTotalDisableString", permTotalDisable == "" ? "" : permTotalDisable);
                            //     }

                            //   if (!String.IsNullOrEmpty(permPartialDisable))
                            //   {
                            strHtml = strHtml.Replace("@permTotalPartialString", permPartialDisable == "" ? "" : permPartialDisable);
                            //   }

                            //    if (!String.IsNullOrEmpty(tempTotalDisable))
                            //    {
                            strHtml = strHtml.Replace("@tempTotalDisableString", tempTotalDisable == "" ? "" : tempTotalDisable);
                            //    }

                            //   if (!String.IsNullOrEmpty(carraigeBody))
                            //  {
                            strHtml = strHtml.Replace("@carraigeBodyString", carraigeBody == "" ? "" : carraigeBody);
                            // }
                            // if (!String.IsNullOrEmpty(funeralExpense))
                            // {
                            strHtml = strHtml.Replace("@funeralExpenseString", funeralExpense == "" ? "" : funeralExpense);
                            //  }
                            //  if (!String.IsNullOrEmpty(medicalExpense))
                            //  {
                            strHtml = strHtml.Replace("@medicalExpenseString", medicalExpense == "" ? "" : medicalExpense);
                            //   }
                            //   if (!String.IsNullOrEmpty(purchaseBlood))
                            // {
                            strHtml = strHtml.Replace("@purchasebloodString", purchaseBlood == "" ? "" : purchaseBlood);
                            //   }
                            //  if (!String.IsNullOrEmpty(transportation))
                            //  {
                            strHtml = strHtml.Replace("@transportationString", transportation == "" ? "" : transportation);
                            //  }
                            //  if (!String.IsNullOrEmpty(compassionate))
                            //  {
                            strHtml = strHtml.Replace("@compassionateString", compassionate == "" ? "" : compassionate);
                            //  }
                            //  if (!String.IsNullOrEmpty(disappearance))
                            //  {
                            strHtml = strHtml.Replace("@disappearanceString", disappearance == "" ? "" : disappearance);
                            //  }
                            //   if (!String.IsNullOrEmpty(modifyResidence))
                            // {
                            strHtml = strHtml.Replace("@modifyResidenceString", modifyResidence == "" ? "" : modifyResidence);
                            // }
                            //   if (!String.IsNullOrEmpty(costOfSupport))
                            //   {
                            strHtml = strHtml.Replace("@costOfSupportString", costOfSupport == "" ? "" : costOfSupport);
                            //  }
                            //  if (!String.IsNullOrEmpty(commonCarrier))
                            //  {
                            strHtml = strHtml.Replace("@commonCarrierString", commonCarrier == "" ? "" : commonCarrier);
                            //   }
                            //  if (!String.IsNullOrEmpty(childrenGrant))
                            //  {
                            strHtml = strHtml.Replace("@childrenGrantString", childrenGrant == "" ? "" : childrenGrant);
                            //   }
                            //  if (!String.IsNullOrEmpty(marraigeExpense))
                            //  {
                            strHtml = strHtml.Replace("@marraigeExpenseString", marraigeExpense == "" ? "" : marraigeExpense);
                            //  }
                            //  if (!String.IsNullOrEmpty(sportsActivity))
                            //  {
                            strHtml = strHtml.Replace("@sportsActivityString", sportsActivity == "" ? "" : sportsActivity);
                            //  }
                            //  if (!String.IsNullOrEmpty(widowHood))
                            // {
                            strHtml = strHtml.Replace("@widowHoodString", widowHood == "" ? "" : widowHood);
                            //  }
                            //  if (!String.IsNullOrEmpty(ambulanceChargesString))
                            //  {
                            strHtml = strHtml.Replace("@ambulanceChargesString", ambulanceChargesString == "" ? "" : ambulanceChargesString);
                            //  }
                            //  if (!String.IsNullOrEmpty(dailyCashString))
                            //  {
                            strHtml = strHtml.Replace("@dailyCashString", dailyCashString == "" ? "" : dailyCashString);
                            //  }
                            //   if (!String.IsNullOrEmpty(accidentalHospString))
                            // {
                            strHtml = strHtml.Replace("@accidentalHospString", accidentalHospString == "" ? "" : accidentalHospString);
                            // }
                            // if (!String.IsNullOrEmpty(convalString))
                            //  {
                            strHtml = strHtml.Replace("@convalString", convalString == "" ? "" : convalString);
                            //  }
                            // if (!String.IsNullOrEmpty(burnsString))
                            //  {
                            strHtml = strHtml.Replace("@burnsString", burnsString == "" ? "" : burnsString);
                            //  }
                            // if (!String.IsNullOrEmpty(brokenBones))
                            //  {
                            strHtml = strHtml.Replace("@brokenBones", brokenBones == "" ? "" : brokenBones);
                            //  }
                            //  if (!String.IsNullOrEmpty(comaString))
                            //  {
                            strHtml = strHtml.Replace("@comaString", comaString == "" ? "" : comaString);
                            // }
                            // if (!String.IsNullOrEmpty(domesticTravelString))
                            //  {
                            strHtml = strHtml.Replace("@domesticTravelString", domesticTravelString == "" ? "" : domesticTravelString);
                            // }
                            //   if (!String.IsNullOrEmpty(lossofEmployString))
                            //   {
                            strHtml = strHtml.Replace("@lossofEmployString", lossofEmployString == "" ? "" : lossofEmployString);
                            //  }
                            //if (!String.IsNullOrEmpty(onDutyCover))
                            //{
                            strHtml = strHtml.Replace("@onDutyCover", onDutyCover == "" ? "" : onDutyCover);
                            //}
                            // if (!String.IsNullOrEmpty(legalExpenses))
                            // {
                            strHtml = strHtml.Replace("@legalExpenses", legalExpenses == "" ? "" : legalExpenses);
                            // }
                            //    if (!String.IsNullOrEmpty(reducingCoverString))
                            //   {
                            strHtml = strHtml.Replace("@reducingCoverString", reducingCoverString == "" ? "" : reducingCoverString);
                            // }
                            //   if (!String.IsNullOrEmpty(assignmentString))
                            //  {
                            strHtml = strHtml.Replace("@assignmentString", assignmentString == "" ? "" : assignmentString);
                            // }

                            strHtml = strHtml.Replace("@accidentalDentalString", accidentalDentalString == "" ? "" : accidentalDentalString);
                            strHtml = strHtml.Replace("@opdString", opdString == "" ? "" : opdString);

                            #region HDCRISKFORPROTECT
                            string _Date1 = ds.Tables[0].Rows[0]["dAccountDebitDate"].ToString();
                            DateTime dtDateHDCRisk = Convert.ToDateTime(_Date1);

                            string TransactionDateHDCRisk = dtDateHDCRisk.ToString("dd/MM/yyyy");
                            strHtml = strHtml.Replace("@TransactionDateHDCRisk", TransactionDateHDCRisk);

                            string mentionedGender = ds.Tables[0].Rows[0]["vCustomerGender"].ToString();
                            if (string.IsNullOrEmpty(mentionedGender))
                            {
                                strHtml = strHtml.Replace("@salutation", "");
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "M" || ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "Male")
                                {
                                    strHtml = strHtml.Replace("@salutation", "Mr.");
                                }
                                else if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "F")
                                {
                                    strHtml = strHtml.Replace("@salutation", "Mrs.");
                                }
                                else
                                {
                                    strHtml = strHtml.Replace("@salutation", "");
                                }
                            }
                            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@addressofinsured1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@addressofinsured2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                            strHtml = strHtml.Replace("@addressofinsured3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                            strHtml = strHtml.Replace("@addressofinsuredCity", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                            strHtml = strHtml.Replace("@addressofinsuredState", ds.Tables[0].Rows[0]["vProposerState"].ToString());
                            strHtml = strHtml.Replace("@Pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                            strHtml = strHtml.Replace("@mobileno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());

                            strHtml = strHtml.Replace("@productname", ds.Tables[0].Rows[0]["vProductName"].ToString());
                            #endregion

                            strHtml = strHtml.Replace("@KotakGroupAccidentProtectUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]));

                            //CR_P1_450_Start Kuwar Tax Invoice_GPA_Policy 
                            #region TaxInvoiceGPAPolicy

                            //GPA_GenerateGPAProtectPDF()
                            StringBuilder taxinvoice = new StringBuilder();
                            taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                            int temp = 0;
                            string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                            string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                            string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                            string netPremium = ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString();
                            if (netPremium.Contains('.'))
                            {
                                temp = Convert.ToInt32(netPremium.Substring(0, netPremium.IndexOf('.')));

                            }
                            else
                            {
                                temp = Convert.ToInt32(netPremium);
                            }

                            string NetPremiumInWord = ConvertAmountInWord(temp);

                            // QR Code
                            string suppliGSTN = ConfigurationManager.AppSettings["GstRegNo"].ToString();
                            string kgiStateCode = suppliGSTN.Substring(0, 2); // getting kgi state code 
                            // string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                            string buyerGSTN = "";
                            //string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                            string transactionDate = ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString();
                            int noofHSNCode = 0;
                            // string hsnCode = "";
                            string hsnCode = ConfigurationManager.AppSettings["SacCode"].ToString();
                            string receiptNumber = ds.Tables[0].Rows[0]["vChallanNumber"].ToString();
                            if (hsnCode != "")
                            {
                                var tempcount = hsnCode.Split(' ').Length;
                                for (int i = 0; i < tempcount; i++)
                                {
                                    noofHSNCode++;
                                }

                            }
                            string Imagepath = string.Empty;
                            CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                            Imagepath = Imagepath == "error" ? "" : Imagepath;
                            string kgiStateName = string.Empty;
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                                cmd.Connection = con;
                                //sqlCon.Open();
                                object objStaeName = cmd.ExecuteScalar();
                                kgiStateName = Convert.ToString(objStaeName);
                            }
                            strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);


                            strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@gistinno", "");
                            strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                            strHtml = strHtml.Replace("@customername", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@emailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                            strHtml = strHtml.Replace("@contactno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@address", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@address1", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                            strHtml = strHtml.Replace("@address2", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());// add 3 address
                            strHtml = strHtml.Replace("@imdcode", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@receiptno", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            strHtml = strHtml.Replace("@customerstatecode", custStateCode);
                            //strHtml = strHtml.Replace("@customerstatecode", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst statecode of customer require
                            strHtml = strHtml.Replace("@supplyname", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst state name require of customer

                            //strHtml = strHtml.Replace("@gistinno", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                            strHtml = strHtml.Replace("@KotakGstNo", ConfigurationManager.AppSettings["GstRegNo"].ToString());//not found
                            strHtml = strHtml.Replace("@name", kgiName);
                            strHtml = strHtml.Replace("@panNo", kgiPanno);
                            strHtml = strHtml.Replace("@cinNo", kgiCINno);

                            //  strHtml = strHtml.Replace("@vKGIBranchAddress", ConfigurationManager.AppSettings["AddressWithLine"].ToString());//not found
                            strHtml = strHtml.Replace("@invoicedate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["vAdditional_column_4"].ToString());
                            // strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["add_col_1"].ToString()); // not present in the SP
                            strHtml = strHtml.Replace("@partnerappno", "");// this column is there as per jay
                            strHtml = strHtml.Replace("@irn", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@kgistatecode", kgiStateCode);//gst state code of kotak uncomment 
                            strHtml = strHtml.Replace("@kgistatename", kgiStateName);//gst state code of kotak uncommentuncomment

                            strHtml = strHtml.Replace("@totalpremium", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@netamount", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@totalgst", ds.Tables[0].Rows[0]["TotalGSTAmount"].ToString());

                            strHtml = strHtml.Replace("@cgstpercent", ds.Tables[0].Rows[0]["cgstPercentage"].ToString());
                            strHtml = strHtml.Replace("@ugstpercent", ds.Tables[0].Rows[0]["ugstPercentage"].ToString());
                            strHtml = strHtml.Replace("@sgstpercent", ds.Tables[0].Rows[0]["sgstPercentage"].ToString());
                            strHtml = strHtml.Replace("@igstpercent", ds.Tables[0].Rows[0]["igstPercentage"].ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@cgstamt", ds.Tables[0].Rows[0]["cgstAmount"].ToString());
                            strHtml = strHtml.Replace("@ugstamt", ds.Tables[0].Rows[0]["ugstAmount"].ToString());
                            strHtml = strHtml.Replace("@sgstamt", ds.Tables[0].Rows[0]["sgstAmount"].ToString());
                            strHtml = strHtml.Replace("@igstamt", ds.Tables[0].Rows[0]["igstAmount"].ToString());

                            strHtml = strHtml.Replace("@cessrate", "0");
                            strHtml = strHtml.Replace("@cessamt", ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString());

                            string tdservicetax = string.Empty;
                            string dataservicetax = string.Empty;
                            if (ds.Tables[0].Rows[0]["nServiceTax"].ToString() != "0" && ds.Tables[0].Rows[0]["nServiceTax"].ToString() != " ")
                            {
                                tdservicetax = "<td style='border: 1px solid black' width='5%'><p style ='font-size:small'><strong>Service Tax</strong></p></td> ";
                                dataservicetax = "<td style ='border:1px solid black' width = '5%'><p> " + Convert.ToDecimal(ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td>";
                            }
                            strHtml = strHtml.Replace("@servictaxh", tdservicetax == "" ? "" : tdservicetax);
                            strHtml = strHtml.Replace("@servicetx", dataservicetax == "" ? "" : dataservicetax);
                            strHtml = strHtml.Replace("@totalgross", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());// change1
                            strHtml = strHtml.Replace("@totalinvoicevalueinfigure", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@totalinvoicevalueinwords", NetPremiumInWord.ToString());
                            #endregion
                            //CR_450_End_Kuwar_Tax_Invoice GPA Policy

                            // Get the current page HTML string by rendering into a TextWriter object
                            TextWriter outTextWriter = new StringWriter();
                            HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);

                            string currentPageHtmlString = strHtml;

                            //byte[] arrSign;
                            //byte[] arr;

                            //arr = convertToPdfNew(currentPageHtmlString, "");
                            //arrSign = Sign(arr);

                            // Create a HTML to PDF converter object with default settings
                            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                            // Set license key received after purchase to use the converter in licensed mode
                            // Leave it not set to use the converter in demo mode
                            string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();

                            htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

                            // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                            // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                            htmlToPdfConverter.ConversionDelay = 2;

                            // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                            htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                            // Add Header

                            // Enable header in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                            // Optionally add a space between header and the page body
                            // The spacing for first page and the subsequent pages can be set independently
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                            htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");

                            // Draw header elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                                DrawHeader(htmlToPdfConverter, false);

                            // Add Footer

                            // Enable footer in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                            // Optionally add a space between footer and the page body
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                            // Draw footer elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                                DrawFooter(htmlToPdfConverter, false, true);

                            // Use the current page URL as base URL
                            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;

                            ////For Live
                            byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                            byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                            //// For Live

                            //For Dev
                            //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                            //byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                            // For Dev

                            // Send Email to user if valid Email available


                            // saving schedule copy 
                            string filelocation = ConfigurationManager.AppSettings["KotakPolicySchedules"].ToString();
                            string filename = filelocation + "\\" + certificateNo + ".pdf";
                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                            }
                            File.WriteAllBytes(filename, outPdfBuffer);


                            if (string.IsNullOrEmpty(emailId))
                            {
                                Alert.Show("Emial ID is blank");
                            }

                            else if (Regex.IsMatch(emailId.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                            {
                                // fnSendEmail(outPdfBuffer, emailId, customerName, certificateNo);
                                fnsendmail(filename, emailId, customerName, certificateNo);
                                //Alert.Show(" Policy sent to " + emailId);

                            }
                            else
                            {
                                Alert.Show(" Invalid Email ID " + emailId);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, " fnGenerateGPA_Protect_Schedule(string certificateNo, string emailId," +
                    " string customerName)  certificateNumber  " + certificateNo + "  Email ID " + emailId + "    ::Error occured  :" +
                    "  " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while gereating GPA Protect Schedule");
            }
        }
        void htmlToPdfConverter_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
        {
            // Set the header visibility in first, odd and even pages
            if (true)
            {
                if (eventParams.PageNumber == 1)
                    eventParams.Page.ShowHeader = true;
                else if ((eventParams.PageNumber % 2) == 0 && !true)
                    eventParams.Page.ShowHeader = false;
                else if ((eventParams.PageNumber % 2) == 1 && !true)
                    eventParams.Page.ShowHeader = false;
            }

            // Set the footer visibility in first, odd and even pages
            if (true)
            {
                if (eventParams.PageNumber == 1)
                    eventParams.Page.ShowFooter = true;
                else if ((eventParams.PageNumber % 2) == 0 && !true)
                    eventParams.Page.ShowFooter = false;
                else if ((eventParams.PageNumber % 2) == 1 && !true)
                    eventParams.Page.ShowFooter = false;
            }
        }

    }
}