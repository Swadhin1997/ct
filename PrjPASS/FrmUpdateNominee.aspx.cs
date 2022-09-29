using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;
using System.Text;

namespace PrjPASS
{
    public partial class FrmUpdateNominee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            dvNomineeDetails.Visible = false;
            PolicyHolderNameDIV.Visible = false;

        }

        protected void btnOTPSend_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (Validation(out ErrorMessage))
            {
                string mobileNumber = GetMobileNumberByPolicyNo(txtPolicyNumber.Text.ToString().Trim());
                string msg = GenerateOTPNew(mobileNumber);
                if (msg == "success")
                {
                    txtPolicyNumber.Enabled = false;
                    btnOTPSend.Visible = false;
                    otpPanel.Visible = true;
                    NoteDiv.Visible = true;
                    btnMobileVerify.Focus();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "testpage", "runme();", true);
                    UpdatePanel1.Update();
                }
                else
                {
                    txtPolicyNumber.Enabled = true;
                    Alert.Show("could not generate OTP, kindly try after sometime");

                }
            }
            else
            {
                Alert.Show(ErrorMessage);
            }
        }

        private string GetMobileNumberByPolicyNo(string PolicyNo)
        {
            string MobileNumber = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("PROC_GET_MOBILE_NUMBER_FROM_POLICY", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyNumber", PolicyNo);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MobileNumber = dt.Rows[0]["TXT_MOBILE"].ToString();
                        lblMobMessage.Text = "An OTP is sent to your mobile number ending with " + MobileNumber.Substring(MobileNumber.Length - 4);
                        return MobileNumber;
                    }
                    else
                    {
                        otpPanel.Visible = false;
                        NoteDiv.Visible = false;
                        Alert.Show("Mobile number not available for this policy." , "FrmUpdateNominee.aspx");
                        //return "Mobile number not available for this policy.";
                        return "";
                    }
                }

            }
            catch (Exception ex)
            {
                Alert.Show(ex.ToString());
                return "";
            }
        }

        private string GenerateOTPNew(string MobileNumber)
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
                  //  string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
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
            if (string.IsNullOrEmpty(txtPolicyNumber.Text.ToString()))
            {
                ErrorMessage = "Please enter policy number!";
            }

            return ErrorMessage.Length > 0 ? false : true;
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
                NoteDiv.Visible = true;
                txtOtpNumber.Text = "";
                btnOTPSend.Enabled = false;
                btnMobileReSend.Enabled = false;
                btnMobileVerify.Focus();
                cvtxtOtpNumber.ErrorMessage = "Please provide valid otp number.";
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "testpage", "runme();", true);
            }
        }

        protected void onClickbtnMobileVerify(object sender, EventArgs e)
        {
            bool isSuccess = false;
            if (txtOtpNumber.Text == HttpContext.Current.Session["OTPNumber"].ToString())
            {
                ShowNomineeDetails(txtPolicyNumber.Text.ToString().Trim());
                otpPanel.Visible = false;
                NoteDiv.Visible = false;
                dvNomineeDetails.Visible = true;
            }
        }

        private void ShowNomineeDetails(string PolicyNumber)
        {
            DataTable dt = new DataTable("NomineeDetails");
            string strDetail = string.Empty;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("PROC_GET_POLICY_NOMINEE_DETAIL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PolicyNumber", PolicyNumber);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    PolicyHolderNameDIV.Visible = true;
                    btnOTPSendDIV.Visible = false;
                    lblPolicyHolderName.Text = dt.Rows[0]["TXT_CUSTOMER_NAME"].ToString();
                    //txtNomineeName.Text = dt.Rows[0]["NomineeName"].ToString();
                    //txtDOB.Text = dt.Rows[0]["dob"].ToString();
                    //drpNomineeRelationship.SelectedValue = dt.Rows[0]["relation"].ToString();
                    //txtAppointeename.Text = dt.Rows[0]["AppointeeName"].ToString();
                    //txtAppointeeDOB.Text = dt.Rows[0]["AppointeeDOB"].ToString();
                }
            }
            otpPanel.Visible = false;
            NoteDiv.Visible = false;
        }

        protected void onClickbtnMobileReSend(object sender, EventArgs e)
        {
            btnOTPSend_Click(sender, e);
        }

        protected void BtnUpdateNominee_Click(object sender, EventArgs e)
        {
            String ErrorMessage = string.Empty;
            if (isvalidUpdateData(out ErrorMessage))
            {
                FnAddNewNominneDetails();
            }
            else
            {
                Alert.Show("" + ErrorMessage.ToString());
                PolicyHolderNameDIV.Visible = true;
                btnOTPSendDIV.Visible = false;
                otpPanel.Visible = false;
                NoteDiv.Visible = false;
                dvNomineeDetails.Visible = true;
            }

        }

        private void FnAddNewNominneDetails()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("PROC_INSERT_NEW_NOMINEE_DETAIL", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyNumber", txtPolicyNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@PolicyHolderName", lblPolicyHolderName.Text.Trim());
                    cmd.Parameters.AddWithValue("@NomineeName", txtNomineeName.Text.Trim());
                    cmd.Parameters.AddWithValue("@NomineeDOB", DateTime.ParseExact(txtDOB.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@NomineeRelationship", drpNomineeRelationship.SelectedItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@Appointeename", txtAppointeename.Text.Trim());
                    if(ddlAppointeeRelationship.SelectedItem.ToString() == "Select")
                    {
                        cmd.Parameters.AddWithValue("@AppointeeRelationship", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@AppointeeRelationship", ddlAppointeeRelationship.SelectedItem.ToString());
                    }

                    cmd.ExecuteNonQuery();
                    fnSendEmailtoKotakCare(txtPolicyNumber.Text, lblPolicyHolderName.Text.Trim(), txtNomineeName.Text.Trim(), txtDOB.Text.Trim(), drpNomineeRelationship.SelectedItem.Text.Trim(), txtAppointeename.Text.Trim(), ddlAppointeeRelationship.SelectedItem.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FnAddNewNominneDetails");
            }
        }

        private void fnSendEmailtoKotakCare(string Policynumber, string PolicyHolderName, string NomineeName, string DOB, string NomineeRelationship, string Appointeename, string AppointeeRelationship)
        {
            try
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["KotakCareEmail"].Trim()))
                {
                    string emailId = ConfigurationManager.AppSettings["KotakCareEmail"].Trim();
                    string strPath = string.Empty;
                    string MailBody = string.Empty;
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Port = 25;
                    smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                    smtpClient.Timeout = 3600000;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                    strPath = AppDomain.CurrentDomain.BaseDirectory +"Update_Nominee_email_bodydetails.html";
                    MailBody = File.ReadAllText(strPath);
                    MailBody = MailBody.Replace("#Policynumber", Policynumber);
                    MailBody = MailBody.Replace("#PolicyHolderName", PolicyHolderName);
                    MailBody = MailBody.Replace("#NomineeName", NomineeName);
                    MailBody = MailBody.Replace("#DOB", DOB);
                    MailBody = MailBody.Replace("#NomineeRelationship", NomineeRelationship);
                    MailBody = MailBody.Replace("#Appointeename", string.IsNullOrEmpty(Appointeename) ? "NA" :Appointeename);
                    MailBody = MailBody.Replace("#AppointeeRelationship", AppointeeRelationship != "Select" ? AppointeeRelationship : "NA" );
                    MailMessage mm = new MailMessage();
                    mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                    mm.Subject = string.Format(ConfigurationManager.AppSettings["Update_Nominee_email_Subject"], Policynumber);
                    mm.Body = MailBody;
                    mm.To.Add(emailId);
                    mm.IsBodyHtml = true;
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    smtpClient.Send(mm);

                }


                ScriptManager.RegisterStartupScript(this, this.GetType(), "swal", "ShowSuccessAlert('Nominee details updated successfully.');", true);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnSendEmailtoKotakCare");
                Alert.Show("" + ex.ToString());
            }
        }



        private bool isvalidUpdateData(out string ErrorMessage)
        {
            ErrorMessage = string.Empty;

            if (lblPolicyHolderName.Text == txtNomineeName.Text)
            {
                ErrorMessage = "Policy holder name and nominee name can not be same.";
                txtDOB.Text = "";
                return false;
            }
            if (string.IsNullOrEmpty(txtDOB.Text.ToString()))
            {
                ErrorMessage = "Please enter Date of Birth of Nominee.";
                txtDOB.Text = "";
                return false;
            }

            if (string.IsNullOrEmpty(txtNomineeName.Text.ToString()))
            {
                ErrorMessage = "Please enter nominee name.";
                txtDOB.Text = "";
                return false;
            }

            if (!Regex.IsMatch(txtNomineeName.Text.Trim(), @"^[\p{L}\p{M}' \.\-]+$"))
            {
                ErrorMessage = "Please enter valid nominee name.";
                txtDOB.Text = "";
                return false;
            }

            if (drpNomineeRelationship.SelectedItem.ToString() == "Select")
            {
                ErrorMessage = "Please select relationship with nominee.";
                txtDOB.Text = "";
                return false;
            }

            return ErrorMessage.Length > 0 ? false : true;
        }

        protected void BtnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://www.kotakgeneralinsurance.com/");
        }
    }
}