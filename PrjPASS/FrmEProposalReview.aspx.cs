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
using System.Data.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Security.Cryptography;




namespace PrjPASS
{
    public partial class FrmEProposalReview : System.Web.UI.Page
    {
        public int interactionID = 0;
        public string ReferenceNo = string.Empty;
        public bool isVerified = false;
        public bool isVerifiedon = false;
        public string shorturl = string.Empty;
        public string vUserEmailID = string.Empty;
        string userName = ConfigurationManager.AppSettings["userName"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();
        string FrmEProposalReviewScheduleLog = AppDomain.CurrentDomain.BaseDirectory + "//FrmEProposalReviewSchedule//log.txt";
        string FrmEProposalReviewScheduleLogDirectory = AppDomain.CurrentDomain.BaseDirectory + "//FrmEProposalReviewSchedule";
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Directory.Exists(FrmEProposalReviewScheduleLogDirectory))
            {
                Directory.CreateDirectory(FrmEProposalReviewScheduleLogDirectory);
            }

            if (!File.Exists(FrmEProposalReviewScheduleLog))
            {
                File.Create(FrmEProposalReviewScheduleLog);
            }
            string encryptRefno = Request.QueryString["ReferenceNo"];
            ReferenceNo = Encryption.DecryptText(encryptRefno);
            GetEproposalDetails(ReferenceNo);
            ViewDetails();
        }

        private void GetEproposalDetails(string ReferenceNo)
        {
            if (ReferenceNo.Length > 0)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_GET_E_PROPOSAL_REVIEW_DETAILS";
                        cmd.Parameters.AddWithValue("@ReferenceNo", ReferenceNo);
                        cmd.Connection = conn;
                        conn.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                lbl_customername.InnerText = sdr["CustomerName"].ToString();
                                lbl_custaddress.InnerText = sdr["CustomerAddress"].ToString();
                                lbl_customermobileno.InnerText = sdr["CustomerMobile"].ToString();
                                lbl_customeremailid.InnerText = sdr["CustomerEmail"].ToString();
                                lbl_product.InnerText = sdr["Product"].ToString();
                                lbl_suminsuredamt.InnerText = sdr["SumInsuredAmt"].ToString();
                                lbl_Imdcode.InnerText = sdr["IMDCODE"].ToString();
                                lbl_branchcode.InnerText = sdr["BranchCode"].ToString();
                                lbl_premiumamt.InnerText = sdr["PremiumAmt"].ToString();
                                lbl_physicalproposalno.InnerText = sdr["ProposalNo"].ToString();
                                isVerified = sdr["IsProposalVerified"].ToString() == "False" ? false : true;
                                isVerifiedon = string.IsNullOrEmpty(sdr["ProposalVerifiedOn"].ToString()) ? false : true;
                                vUserEmailID= sdr["UserEmailID"].ToString();
                                //   myframe.Src = sdr["FilePath"].ToString();
                                iframeDiv.Controls.Add(new LiteralControl("<iframe src=\"" + sdr["FilePath"].ToString() + "\" style='width:100%;height:100%'></iframe><br/>"));
                                //iframeDiv.Controls.Add(new LiteralControl("<iframe src=\"" + sdr["FilePath"].ToString() + "\" style='width:100%;height:100%'></iframe><br/>"));
                            }
                        }
                        conn.Close();
                    }


                }
            }
        }

        protected void ViewDetails()
        {
            if (isVerified==true&& isVerifiedon==false)//No action
            {
                ReviewDetails.Style.Add("display", "inline-block");
                //sectionRecordNotFound.Visible = true;
                SectionSuccess.Style.Add("display", "none");
                norecord.Style.Add("display", "none");
                SectionRejected.Style.Add("display", "none");
                submitpanel.Style.Add("display", "block");
            }
            else if(isVerified == true && isVerifiedon == true)//accepted
            {
                 ReviewDetails.Style.Add("display", "none");
                SectionSuccess.Style.Add("display", "inline-block");
                SectionRejected.Style.Add("display", "none");
                norecord.Style.Add("display", "none");
                submitpanel.Style.Add("display", "none");
                lblreferenceno1.Text = ReferenceNo;
                // submitpanel.Visible = true;

            }
            else if(isVerified == false && isVerifiedon == true)//rejected
            {
                ReviewDetails.Style.Add("display", "none");
                norecord.Style.Add("display", "none");
                SectionRejected.Style.Add("display", "inline-block");
                submitpanel.Style.Add("display", "none");
                SectionSuccess.Style.Add("display", "none");
                lblreferenceno.Text = ReferenceNo;
            }
        }
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            String rejectreason = hddnrejectreason.Value;
            string customername = lbl_customername.InnerText.Trim();
            string customeremailid = lbl_customeremailid.InnerText.Trim();
            string customermobileno = lbl_customermobileno.InnerText.Trim();
            string ErrorMessage = string.Empty;
            string strtype = hddntype.Value;
            if (strtype == "Approve")
            {
               string msg = "";
           
                msg = GenerateOTPNew(lbl_customermobileno.InnerText.Trim(), customeremailid);
                if (msg == "success")
                {

                    String otp = HttpContext.Current.Session["OTPNumber"].ToString();
                    SaveOTP(otp, ReferenceNo);
                    otpPanel.Visible = true;
                    btnMobileVerify.Focus();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "testpage", "runme();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "otpModalshow();", true);
                    GetEproposalDetails(ReferenceNo);
                    UpdatePanel1.Update();
                    ViewDetails();
                }
                else
                {
                    File.WriteAllText(FrmEProposalReviewScheduleLog, "could not generate OTP for mobile number" + customermobileno + " Email ID " + customeremailid + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    Alert.Show("could not generate OTP, kindly try after sometime");

                }
            }
            else
            {
                
            
               
                bool isrejectmailsent=SendRejectEmail(customername, vUserEmailID, rejectreason);
                SaveVerified(ReferenceNo, 0);
                Alert.Show("Successfully Rejected E-proposal Verification.");
                GetEproposalDetails(ReferenceNo);
                ViewDetails();
            }


        }

        protected void onClickbtnMobileVerify(object sender, EventArgs e)
        {
            try
            {
                bool isSuccess = false;
                string customername = lbl_customername.InnerText;
                string customeremailid = lbl_customeremailid.InnerText;
                string customermobileno = lbl_customermobileno.InnerText;
                string product = lbl_product.InnerText;

                if (txtOtpNumber.Text == hiddenotp.Value)
                {
                    File.WriteAllText(FrmEProposalReviewScheduleLog, "Verifying OTP " + txtOtpNumber.Text + "with mobile Number" + customermobileno + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    otpPanel.Visible = false;
                    //ReviewDetails.Visible = false;
                    isSuccess = SaveVerified(ReferenceNo,1);
                    if (isSuccess)
                    {
                        bool isApproveMailSent=SendApprovalEmail(product, customeremailid, customername);
                        bool isSMSAprrovalSent = SendApprovalSMS(customermobileno, ReferenceNo);
                        File.WriteAllText(FrmEProposalReviewScheduleLog, "SendApprovalEmail :" + isApproveMailSent + ",SendApprovalSMS :" + isSMSAprrovalSent + "  " + DateTime.Now.ToString() + System.Environment.NewLine);

                        Alert.Show("Successfully Accepted E-proposal Verification.");
                        GetEproposalDetails(ReferenceNo);
                        ViewDetails();

                    }
                }
                else
                {

                    Alert.Show("OTP entered is wrong , try again!");

                    UpdatePanel1.Update();
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogException(ex, "onClickbtnMobileVerify");
            }

        }

        protected void onClickbtnMobileReSend(object sender, EventArgs e)
        {
            btnsubmit_Click(sender, e);
        }
        private string GenerateOTPNew(string mobileNumber, string emailID)
        {
            string GeneratedOTP = string.Empty;
            string customername = string.Empty;
            string productname = string.Empty;
            try
            {
                Random r = new Random();
                GeneratedOTP = r.Next(100000, 999999).ToString();
                HttpContext.Current.Session["OTPNumber"] = GeneratedOTP;
                hiddenotp.Value = HttpContext.Current.Session["OTPNumber"].ToString();
                customername = lbl_customername.InnerText;
                productname = lbl_product.InnerText;
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
      @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
      @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex emailreg = new Regex(strRegex);
                File.WriteAllText(FrmEProposalReviewScheduleLog, "mobileNumber"+ mobileNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);

              
                if (emailreg.IsMatch(emailID.Trim()))
                {
                    bool IsSendEmailSuccess = SendOTPEmail(GeneratedOTP, emailID, customername, productname);
                    File.WriteAllText(FrmEProposalReviewScheduleLog, "OTP generated for Email ID " + emailID + "  and otp " + GeneratedOTP + "   " + DateTime.Now.ToString() + System.Environment.NewLine);

                }
                if (mobileNumber.Length == 10)
                {
                   
                    bool IsSendSMSSuccess = SendSMS(GeneratedOTP, mobileNumber);
                    File.WriteAllText(FrmEProposalReviewScheduleLog, "OTP SENT SendSMS" + IsSendSMSSuccess + "," + GeneratedOTP + "sent to Mobile number " + mobileNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                }

            }
            catch (Exception ex)
            {
                GeneratedOTP = "";
                ExceptionUtility.LogException(ex, "Error in GenerateOTP on FrmSTP Page");
            }
            return string.IsNullOrEmpty(GeneratedOTP) ? "" : "success";
        }
        private bool SendOTPEmail(string generatedOTP, string emailID,string customername,string productname)
        {
            bool Success = true;
            try
            {
                string emailId = emailID;
                string strPath = string.Empty;
                string MailBody = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBody_EProposal_Review.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#OTP", generatedOTP.ToString());
                MailBody = MailBody.Replace("#Customername", customername);
                MailBody = MailBody.Replace("#Product", productname);

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"], "Kotak General");
              
                mm.Subject = string.Format("OTP for e-Validation of Proposal, Unique ID: {0}", ReferenceNo);
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;

                smtpClient.Send(mm);
                Success = true;
                File.WriteAllText(FrmEProposalReviewScheduleLog, "OTP " + generatedOTP + "sent to Email ID" + emailID + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SendOTPEmail");
                //sectionMain.Visible = false;
                // sectionError.Visible = true;
                Success = false;
            }
            return Success;
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
                    File.WriteAllText(FrmEProposalReviewScheduleLog, "SMS BODY  " + smsBody +" "+ DateTime.Now.ToString() + System.Environment.NewLine);

                    File.WriteAllText(FrmEProposalReviewScheduleLog, "OTP " + GeneratedOTP + "sent to mobile Number" + MobileNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                IsSendSMSSuccess = false;
                ExceptionUtility.LogException(ex, "Error in SendSMS on frmEpropsalReview Page");
                //sectionMain.Visible = false;
                //sectionError.Visible = true;
            }
            return IsSendSMSSuccess;
        }

        private bool SendApprovalEmail(string product, string emailID,String Customername)
        {
            bool Success = true;
            try
            {
                string emailId = emailID;
                string strPath = string.Empty;
                string MailBody = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "EProposal_Approve_MailBody.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#Product", product);
                MailBody = MailBody.Replace("#Customername", Customername);

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"], "Kotak General");
                mm.Subject = "OTP E-Validation of Proposal- Successful";
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;

                smtpClient.Send(mm);
                Success = true;
                File.WriteAllText(FrmEProposalReviewScheduleLog, "OTP E-Validation of Proposal- Successful sent to Email ID" + emailID + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SendApprovalEmail");
                //sectionMain.Visible = false;
                // sectionError.Visible = true;
                Success = false;
            }
            return Success;
        }

        private bool SendApprovalSMS(string mobileno,string Eproposal)
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
                        cmd.CommandText = "SAVE_PASS_EPROPOSAL_APPROVAL_TO_TRANS_SMS_LOG";
                        cmd.Parameters.AddWithValue("@MobileNo", mobileno);
                        cmd.Parameters.AddWithValue("@EProposal", Eproposal);
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
                ExceptionUtility.LogException(ex, "SendApprovalSMS Method");
            }
            return IsSMSSent;

        }

        private bool SendRejectEmail(string clientname, string emailID,string reason)
        {
            bool Success = true;
            try
            {
                string emailId = emailID;
                string strPath = string.Empty;
                string MailBody = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "Eproposal_Reject_MailBody.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#Clientname", clientname);
                MailBody = MailBody.Replace("#Reason", reason);

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"], "Kotak General");
                mm.Subject = string.Format("Rejection of the Proposal for Customer {0}, Unique ID: {1}",clientname ,ReferenceNo);

                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;

                smtpClient.Send(mm);
                Success = true;
                File.WriteAllText(FrmEProposalReviewScheduleLog, "Rejection Mail  sent to Email ID" + emailID + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SendRejectEmail");
               
                Success = false;
            }
            return Success;
        }

        //private bool SendRejectSMS(string mobileno, clientname)
        //{
        //    bool IsSMSSent = false;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection())
        //        {
        //            conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnConnect"].ConnectionString;
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.CommandText = "SAVE_PASS_EPROPOSAL_REJECT_TO_TRANS_SMS_LOG";
        //                cmd.Parameters.AddWithValue("@MobileNo", mobileno);
        //                cmd.Parameters.AddWithValue("@MobileNo", mobileno);
        //                cmd.Parameters.AddWithValue("@MobileNo", mobileno);
        //                cmd.Connection = conn;
        //                conn.Open();
        //                cmd.ExecuteNonQuery();
        //                conn.Close();
        //                IsSMSSent = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, "SendApprovalSMS Method");
        //    }
        //    return IsSMSSent;

        //}
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmDownloadPolicySchedule.aspx");
        }
        protected void SaveOTP(string OTP, string referenceno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SAVE_E_PROPOSAL_REVIEW_OTP";
                        cmd.Parameters.AddWithValue("@OTP", OTP);
                        cmd.Parameters.AddWithValue("@ReferenceNo", referenceno);
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveOTP Method");
            }

        }
        private bool SaveVerified(string referenceno,int val)
        {
            bool IsSaveVerified = false;
            String RejectReason = hddnrejectreason.Value;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SAVE_E_PROPOSAL_REVIEW_VERIFIED";
                        cmd.Parameters.AddWithValue("@ReferenceNo", referenceno);
                        cmd.Parameters.AddWithValue("@IsVerified", val);
                        cmd.Parameters.AddWithValue("@RejectReason", String.IsNullOrEmpty(RejectReason.Trim())? "": RejectReason);
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        IsSaveVerified = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveVerified Method");
            }
            return IsSaveVerified;
        }
    }
}