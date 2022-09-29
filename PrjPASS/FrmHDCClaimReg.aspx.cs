using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmHDCClaimReg : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");
        public string logfile = "log_FrmHDCClaimReg_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
        string Message = string.Empty;
        string emailid;
        protected string ActiveTab { get; private set; }

        public class RootObject
        {
            public string docId { get; set; }
            public string status { get; set; }
        }
        public class clsRequestUploadKites
        {
            public string dept { get; set; }
            public string author { get; set; }
            public string app { get; set; }
            public string docTyp { get; set; }
            public string docN { get; set; }
            public string previous_Policy_No { get; set; }
            public string inwardNo { get; set; }
            public string customerId { get; set; }
            public string parentFolderIndex { get; set; }
            public string application_No { get; set; }
            public string certificate_No { get; set; }
            public string master_Policy_No { get; set; }
            public string child_Intermediary_Code { get; set; }
            public string parent_Intermediary_Code { get; set; }
            public string endorsement_No { get; set; }
            public string policy_Number { get; set; }
            public string claim_Number { get; set; }
            public byte[] binData { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["vUserLoginId"] != null)
                {
                    if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
                    {
                        bool chkAuth;
                        string pageName = this.Page.ToString().Substring(4, this.Page.ToString().Substring(4).Length - 5) + ".aspx";
                        chkAuth = wsGen.Fn_Check_Rights_For_Page(Session["vRoleCode"].ToString(), pageName);
                        string userid = Session["vUserLoginId"].ToString();
                        if (chkAuth == false)
                        {
                            Alert.Show("Access Denied", "FrmMainMenu.aspx");
                            return;
                        }
                    }
                }
                else
                {
                    Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    return;
                }

                Directory.CreateDirectory(folderPath);

                //clnClaimIntimationDate.DateMax = DateTime.Now;
                txtClaimIntimationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtClaimRegistrationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtClaimIntimationDate.Attributes.Add("readonly", "readonly");
                txtDateOfAdmission.Attributes.Add("readonly", "readonly");
                txtDateOfDischarge.Attributes.Add("readonly", "readonly");
            }
        }

        public string FindEmailId(string userid)
        {
            string emailidt = string.Empty;
            string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(consString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("SELECT vUserEmailId FROM TBL_USER_LOGIN WHERE vUserLoginId ='" + userid + "'", con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {  
                                emailidt = dr[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg Error occured while finding Email ID  and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
            }
            return emailidt;
        }

        public string FindUserName(string userid)
        {
            string username = string.Empty;
            string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(consString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("SELECT vUserLoginDesc FROM TBL_USER_LOGIN WHERE vUserLoginId ='" + userid + "'", con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                username = dr[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg Error occured while finding Username and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
            }
            return username;
        }

        protected void btnSearchCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCertificateNumber.Text) && string.IsNullOrEmpty(txtLoanAccNo.Text))
                {
                    ResetControls();
                    Alert.Show("Enter Policy Number or Loan Account number !!");
                    return;
                }

                else
                {
                    ResetControls();
                    string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                    using (SqlConnection con = new SqlConnection(consString))
                    {
                        SqlCommand cmd = new SqlCommand("PROC_HDC_GET_DETAILS", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCertificateNumber", !string.IsNullOrEmpty(txtCertificateNumber.Text) ? txtCertificateNumber.Text : "");
                        cmd.Parameters.AddWithValue("@vLoanAccNumber", !string.IsNullOrEmpty(txtLoanAccNo.Text) ? txtLoanAccNo.Text : "");
                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fetching data for Certificate Number  : " + txtCertificateNumber.Text + " / Loan Account number  " + txtLoanAccNo.Text + "  " + DateTime.Now + Environment.NewLine);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataSet dsCertificateDetails = new DataSet();
                        sda.Fill(dsCertificateDetails);
                        //DrpCustomerName0.DataSource = null;
                        //drpRelation0.DataSource = null;
                        DrpCustomerName0.Items.Clear();
                        drpRelation0.Items.Clear();
                        //DrpCustomerName0.DataBind();
                        //drpRelation0.DataBind();

                        if (dsCertificateDetails.Tables[0] != null)
                        {
                            if (dsCertificateDetails.Tables[0].Rows.Count > 0)
                            {
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                                txtCustomerName.Text = dsCertificateDetails.Tables[0].Rows[0]["vCustomerName"].ToString();
                                txtCustomerType.Text = dsCertificateDetails.Tables[0].Rows[0]["vCustomerType"].ToString();
                                txtCustomerMobile.Text = dsCertificateDetails.Tables[0].Rows[0]["vMobileNo"].ToString();
                                txtMasterPolicyNumber.Text = dsCertificateDetails.Tables[0].Rows[0]["vMasterPolicyNo"].ToString();
                                txtMasterPolicyHolder.Text = dsCertificateDetails.Tables[0].Rows[0]["vMasterPolicyHolder"].ToString();
                                txtProductName.Text = dsCertificateDetails.Tables[0].Rows[0]["vProductName"].ToString();
                                txtAccountNumber.Text = dsCertificateDetails.Tables[0].Rows[0]["vAccountNo"].ToString();
                                txtPolicyStartDate.Text = Convert.ToDateTime(dsCertificateDetails.Tables[0].Rows[0]["vPolicyStartdate"].ToString()).ToString("dd/MM/yyyy");
                                txtPolicyEndDate.Text = Convert.ToDateTime(dsCertificateDetails.Tables[0].Rows[0]["vPolicyEndDate"].ToString()).ToString("dd/MM/yyyy");
                                txtCertificateNumber.Enabled = false;
                                txtLoanAccNo.Enabled = false;
                            }


                            if (dsCertificateDetails.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow rd in dsCertificateDetails.Tables[1].Rows)
                                {
                                    DrpCustomerName0.Items.Add(rd["vCustomerName"].ToString());
                                    drpRelation0.Items.Add(rd["vRelation"].ToString());
                                }
                            }

                            else
                            {
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg no data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                                Alert.Show("No Data Found !!");
                                return;
                            }
                        }

                        else
                        {
                            Alert.Show("No Data Found !!");
                            return;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg Error occured in search certificate : " + txtCertificateNumber.Text + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Claim number not generated - something went wrong - Try again later !!", "FrmMainMenu.aspx");
            }
        }

        private void ResetControls()
        {
            //txtCustomerName.Text = string.Empty;
            txtCustomerType.Text = string.Empty;
            txtCustomerMobile.Text = string.Empty;
            txtMasterPolicyNumber.Text = string.Empty;
            txtMasterPolicyHolder.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtPolicyStartDate.Text = string.Empty;
            txtPolicyEndDate.Text = string.Empty;
            txtClaimedAmount.Text = string.Empty;
            txtAccountNumber.Text = string.Empty;
            txtRemark.Text = string.Empty;

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string claimNumber = string.Empty;


                Regex mregex = new Regex(@"^\d{10}$");
                if (!mregex.IsMatch(txtCustomerMobile.Text))
                {
                    Alert.Show("Customer Mobile number must be 10 digit mobile number");
                    return;
                }

                if (!string.IsNullOrEmpty(txtCallerNumber.Text))
                {
                    if (!mregex.IsMatch(txtCallerNumber.Text))
                    {
                        Alert.Show("Caller Mobile number must be 10 digit mobile number or empty.");
                        return;
                    }
                }


                if (DrpCustomerName0.SelectedValue == "Select")
                {
                    Alert.Show("Member Name must be selected");
                    return;
                }


                if (String.IsNullOrEmpty(txtClaimIntimationDate.Text))
                {
                    Alert.Show("Claim Intimation Date cannot be blank !!");
                    return;
                }

                DateTime dateClaimIntimation = DateTime.ParseExact(txtClaimIntimationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime datePolicyStart = DateTime.ParseExact(txtPolicyStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (DateTime.Compare(datePolicyStart, dateClaimIntimation) > 0)
                {
                    string strPolicyStartDate = datePolicyStart.ToString("dd-MMM-yyyy");
                    string strClaimIntimationDate = dateClaimIntimation.ToString("dd-MMM-yyyy");

                    string MessagedtCompare = string.Format("Claim Intimation date {0} cannot be before the Policy Start Date {1}", strClaimIntimationDate, strPolicyStartDate);
                    Alert.Show(MessagedtCompare);
                    return;
                }


                if (String.IsNullOrEmpty(txtDateOfAdmission.Text))
                {
                    Alert.Show("Date of Admission cannot be blank !!");
                    return;
                }

                if (!String.IsNullOrEmpty(txtDateOfDischarge.Text))
                {
                    if (DateTime.ParseExact(txtDateOfDischarge.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        Alert.Show("Date of Discharge cannot be before Date of Admission");
                        return;
                    }
                }

                if (DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(txtPolicyStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) || DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtPolicyEndDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    Alert.Show("Date of Admission should be between Policy Start Date & End Date including both days");
                    return;
                }

                if (DrpClaimIntimationBy.SelectedValue == "Select")
                {
                    Alert.Show("Claim Intimation by must be selected.");
                    return;
                }


                if (DrpCustomerName0.SelectedValue == "Select")
                {
                    Alert.Show("Customer name must selected !!");
                    return;
                }


                if (drpRelation0.SelectedValue == "Select")
                {
                    Alert.Show("Relation with claimant must selected !!");
                    return;
                }



                if (string.IsNullOrEmpty(txtClaimedAmount.Text))
                {
                    Alert.Show("Claim amount can not be blank !!");
                    txtClaimedAmount.Focus();
                    return;
                }

                Regex nonNumericRegex = new Regex(@"\D");
                if (nonNumericRegex.IsMatch(txtClaimedAmount.Text))
                {
                    Alert.Show("Please Enter valid Claim amount!!");
                    txtClaimedAmount.Focus();
                    return;
                }


                if (txtClaimedAmount.Text.Contains('.') || txtClaimedAmount.Text.Contains('-'))
                {
                    Alert.Show("Please Enter valid Claim amount!!");
                    txtClaimedAmount.Focus();
                    return;
                }

                //if (string.IsNullOrEmpty(txtAccountNumber.Text))
                //{
                //    Alert.Show("Account Number can not be blank !!");
                //    txtAccountNumber.Focus();
                //    return;
                //}

                if (!string.IsNullOrEmpty(txtRemark.Text))
                {
                    if (txtRemark.Text.Length > 4000)
                    {
                        Alert.Show("Remarks valid upto 4000 characters!!");
                        txtAccountNumber.Focus();
                        return;
                    }
                }

                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                using (SqlConnection con = new SqlConnection(consString))
                {
                    SqlCommand cmdCheck = new SqlCommand("PROC_GET_CLAIM_REGISTER_COUNT", con);
                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.Parameters.AddWithValue("@vCertificateNumber", txtCertificateNumber.Text);
                    cmdCheck.Parameters.AddWithValue("@dDateOfAdmission", DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));

                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg getting existing claim data for : " + txtCertificateNumber.Text + " and date of admission " + txtDateOfAdmission.Text + " " + DateTime.Now + Environment.NewLine);

                    con.Open();

                    SqlDataReader drCheck = cmdCheck.ExecuteReader();
                    {
                        if (drCheck.HasRows)
                        {
                            while (drCheck.Read())
                            {
                                if (!String.IsNullOrEmpty(drCheck[0].ToString()))
                                {

                                    ClientScriptManager CSM = Page.ClientScript;
                                    if (!ReturnValue())
                                    {
                                        string strconfirm = "<script>if(!window.confirm('Claim Number " + drCheck[0] + " Already registerd for selected Certificate Number and Admission Date. To register new claim click on OK else click cancle to exit.')){window.location.href='FrmHDCClaimReg.aspx'}</script>";
                                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                                    }

                                    //Message = "Claim Number " + drCheck[0] + " already register for the selected Admission Date !!";
                                    //Alert.Show("Claim " + drCheck[0] + " already register for the selected Admission Date !!");
                                    //return;
                                }
                            }

                        }
                    }

                    drCheck.Close();


                    SqlCommand cmd = new SqlCommand("PROC_INSERT_HDC_CLAIMS_DETAILS", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg inserting claim data for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);

                    cmd.Parameters.AddWithValue("@vCertificateNumber", txtCertificateNumber.Text);
                    cmd.Parameters.AddWithValue("@vClaimIntimationBy", DrpClaimIntimationBy.SelectedValue);
                    cmd.Parameters.AddWithValue("@vCallerContactNumber", txtCallerNumber.Text);
                    cmd.Parameters.AddWithValue("@dDateOfAdmission", DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@dDateOfDischarge", String.IsNullOrEmpty(txtDateOfDischarge.Text) ? txtDateOfDischarge.Text : DateTime.ParseExact(txtDateOfDischarge.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                    cmd.Parameters.AddWithValue("@dDateOfIntimation", DateTime.ParseExact(txtClaimIntimationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@dDateOfRegistration", DateTime.ParseExact(txtClaimRegistrationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@vCustomerMobile", txtCustomerMobile.Text);
                    cmd.Parameters.AddWithValue("@vRelation", drpRelation0.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@vMemberName", DrpCustomerName0.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@dClaimAmount", txtClaimedAmount.Text.Trim());
                    cmd.Parameters.AddWithValue("@vAccountNumber", txtAccountNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@vRemark", txtRemark.Text.Trim());

                    SqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                //Alert.Show("Claim Number generated - " + dr[0].ToString());     
                                claimNumber = dr[0].ToString();
                                Session["_claimNumber"] = claimNumber;
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg claim number generated for : " + txtCertificateNumber.Text + " and claim number is " + claimNumber + " " + DateTime.Now + Environment.NewLine);
                                fnSendHDCClaimRegSMS(claimNumber);
                                //lblMessage.Text = "Claim Number generated - " + dr[0].ToString() + " - wait while the documents are getting uploaded";
                            }

                        }
                    }
                    string emailid = FindEmailId(Session["vUserLoginId"].ToString());
                    Session["emailidreturn"] = emailid;
                    fnsendmail(emailid);
                    con.Close();
                }

                UploadDocuments(claimNumber);


            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg Error occured in insert claim data for : " + txtCertificateNumber.Text + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Claim number not generated - something went wrong - Try again later !!", "FrmMainMenu.aspx");
            }
        }

        private void fnsendmail(string emailId)
        {
            try
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail Started" + DateTime.Now + Environment.NewLine);
                //string emailId = txtEmailforPolicy.Text.Trim();
                string strPath = string.Empty;
                string MailBody = string.Empty;
                string userid = Convert.ToString(Session["vUserLoginId"]);
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                if(Session["InUploadDocumentSection"] == "1")
                {
                    strPath = AppDomain.CurrentDomain.BaseDirectory + "Schedule_Claim_Email_Body_ForDocumentUpload_Trigger.html";
                }
                else
                {
                    strPath = AppDomain.CurrentDomain.BaseDirectory + "Schedule_Claim_Email_Body_Trigger.html";

                }
                MailBody = File.ReadAllText(strPath);
                string user_name = FindUserName(userid);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail :: UserName "+ user_name.ToString()  + DateTime.Now + Environment.NewLine);
                MailBody = MailBody.Replace("#CustomerName", user_name);
                MailBody = MailBody.Replace("#Claim_Number", Convert.ToString(Session["_claimNumber"]));
                MailBody = MailBody.Replace("#Policy_Number", txtCertificateNumber.Text);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail :: Before Converting dd/MMM/yyyy DateOFAdmission " + txtDateOfAdmission.Text + " " + DateTime.Now + Environment.NewLine);
                string strDateOFAdmission = DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail :: After Converting dd/MMM/yyyy DateOFAdmission " + strDateOFAdmission + DateTime.Now + Environment.NewLine);
                MailBody = MailBody.Replace("#date_of_loss", strDateOFAdmission);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail :: Before Converting dd/MMM/yyyy ClaimRegistrationDate " + txtClaimRegistrationDate.Text + " " + DateTime.Now + Environment.NewLine);

                string strClaimRegDate = DateTime.ParseExact(txtClaimRegistrationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);//Convert.ToDateTime(txtClaimRegistrationDate.Text);
                //string strClaimRegDate = dt1.ToString("dd/MMM/yyyy");
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail :: After Converting dd/MMM/yyyy ClaimRegistrationDate " + strClaimRegDate + " " + DateTime.Now + Environment.NewLine);
                MailBody = MailBody.Replace("#Date_Of_Registration", strClaimRegDate);
                //MailBody = MailBody.Replace("#Intimation_Time", txtClaimIntimationDate.Text);
                MailBody = MailBody.Replace("#Customer_Name", txtCustomerName.Text);
                MailBody = MailBody.Replace("#Master_Policy_Holder_Name", txtMasterPolicyHolder.Text);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail :: Policy Number " + txtCertificateNumber.Text + DateTime.Now + Environment.NewLine);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                if (Session["InUploadDocumentSection"] == "1")
                {
                    mm.Subject = string.Format(ConfigurationManager.AppSettings["Schedule_Claim_DocUpload_email_Subject"], Convert.ToString(Session["_claimNumber"]));
                }
                else
                {
                    mm.Subject = string.Format(ConfigurationManager.AppSettings["Schedule_Claim_email_Subject"], txtCertificateNumber.Text);
                }
                    
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(filename);
                //mm.Attachments.Add(attachment);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail smtpClient Started "+ DateTime.Now + Environment.NewLine);
                smtpClient.Send(mm);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail smtpClient Ended "  + DateTime.Now + Environment.NewLine);
                //Alert.Show("Policy schedule sent on Email ID " + emailId);
                //Response.Write(string.Format("<script>alert('Email sent successfully to {0} ')</script>",emailId) );
                //IsEmailRequest = true;
                //Alert.Show("Email Sent Successfully.");
                Session["ErrorCallingPage"] = "FrmHDCCLAIMReg.aspx";
                //string vStatusMsg = "Policy sent to " + emailId;
                //EmailSentMessage = vStatusMsg;
                //Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "Error in fnsendmail on  FrmHDCClaimReg for claim numberand error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while sending email to " + emailId);
            }
        }

        private void fnSendHDCClaimRegSMS(string claimNumber)
        {
            try
            {
                string strPath = string.Empty;
                string smsBody = string.Empty;

                smsBody = "Dear Customer,Your claim has been successfully lodged with Kotak General Insurance with claim registration no." +
                    claimNumber + ". You will be intimated once the claim is settled. For further assistance request you to co-ordinate" +
                    "with your ESFB Branch.Thank You,Kotak General Insurance Co.Ltd.";


                string SmsBody = ConfigurationManager.AppSettings["HDCClaimIntimationSMSBody"].ToString();
                SmsBody = SmsBody.Replace("@claimNumber", claimNumber);

                using (SqlConnection cnCon = new SqlConnection(ConfigurationManager.ConnectionStrings["cnConnect"].ToString()))
                {
                    if (cnCon.State == ConnectionState.Closed)
                    {
                        cnCon.Open();
                    }
                    SqlCommand cmd = new SqlCommand("INSERT_DATA_CUSTOMER_SMS_LOG_HDC_SCHEDULER", cnCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mobile", txtCustomerMobile.Text);
                    cmd.Parameters.AddWithValue("@msg", SmsBody);
                    cmd.ExecuteNonQuery();

                    File.AppendAllText(folderPath + "\\log.txt", "SMS sent to Mobile Number " + txtCustomerMobile.Text.ToString() + "  for claim number" + claimNumber + " " + DateTime.Now + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "Error in fnSendHDCClaimRegSMS on  FrmHDCClaimReg for claim number" + claimNumber + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while sending SMS. Kindly Contact Administrator");
            }
        }

        private bool ReturnValue()
        {
            return false;
        }

        private void UploadDocuments(string claimNumber)
        {
            try
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg starting uploading documents for claim number " + claimNumber + " " + DateTime.Now + Environment.NewLine);

                int nMaxFileSize = 4194304; //4 MB

                if (fulClaimForm.HasFile)
                {
                    if (fulClaimForm.PostedFile.ContentLength > nMaxFileSize)
                    {
                        Alert.Show("Maximum Filesize Allowed is 4 MB for Claim Form - Claim number generated - " + claimNumber + " - you can go to upload screen and try to upload again.", "FrmMainMenu.aspx");
                        return;
                    }

                    string fileExtension = Path.GetExtension(fulClaimForm.PostedFile.FileName);

                    string[] extensionsAllowed = ConfigurationManager.AppSettings["uploadExtension"].ToString().Split(',');

                    if (!extensionsAllowed.Contains(fileExtension))
                    {
                        Alert.Show("Uploaded file extension is not allowed - Claim number generated - " + claimNumber + " - you can go to upload screen and try to upload again.", "FrmMainMenu.aspx");
                        return;
                    }

                    //if (extension != ".zip" || extension!=".pdf" || extension != ".jpeg" || extension != ".xls" || extension != ".xlsx" || extension != ".doc" || extension != ".docx" || extension != ".pdf" || extension != ".pdf" || extension != ".pdf" || extension != ".pdf" || extension != ".pdf" || extension != ".pdf")
                    //{
                    //    Alert.Show("Uploaded file extension is not allowed for Claim Form - Claim number generated - " + claimNumber + " - you can go to upload screen and try to upload again.", "FrmMainMenu.aspx");
                    //    return;
                    //}

                    string uploadPath = folderPath + "\\" + Path.GetFileName(fulClaimForm.PostedFile.FileName);
                    fulClaimForm.SaveAs(uploadPath);

                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg upload file saved for : " + txtCertificateNumber.Text + " and file name is " + fulClaimForm.PostedFile.FileName + " " + DateTime.Now + Environment.NewLine);

                    //byte[] requestedByte = System.IO.File.ReadAllBytes(uploadPath);
                    //string base64String = Convert.ToBase64String(requestedByte, 0, requestedByte.Length);
                    //System.Text.UTF8Encoding encoding2 = new System.Text.UTF8Encoding();
                    //Byte[] byteArray2 = encoding2.GetBytes(base64String);

                    //System.Diagnostics.Debug.WriteLine("byteArray2"+byteArray2 );



                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg staring calling service to upload document " + DateTime.Now + Environment.NewLine);

                    // ServiceReference1.CustomerPortalServiceClient objClient = new ServiceReference1.CustomerPortalServiceClient();
                    //string status = proxy.CreateDocumentDMS(username, password, InwardId, ApplicationNo, CustomerId, DocumentUniqNo, PolicyNo, ClaimNo, Source, FileName, requestedByte, DocumentType);

                    // string status = objClient.CreateDocumentDMS(ConfigurationManager.AppSettings["strUserIdDocs"].ToString(), ConfigurationManager.AppSettings["strPasswordDocs"].ToString(), "", "", "", "", txtCertificateNumber.Text, claimNumber, "HDCPASS", fulClaimForm.PostedFile.FileName, byteArray2, "");


                    string FilePath = uploadPath;
                    string certificate_No = txtCertificateNumber.Text;
                    string FileNameWithExtension = Path.GetFileName(FilePath);
                    string FileType = "";
                    string DocId = "";
                    string ErrorMessageKites = "";
                    //UploadDoc(FilePath, certificate_No, FileNameWithExtension, FileType, out DocId, out ErrorMessageKites);

                    UploadDoc(FilePath, certificate_No, FileNameWithExtension, FileType,claimNumber, out DocId, out ErrorMessageKites);// Added For Kites search
                    string status = DocId;

                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg end calling service to upload document " + Convert.ToString(status) + DateTime.Now + Environment.NewLine);
                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg end calling service to upload document " + DateTime.Now + Environment.NewLine);

                    string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                    using (SqlConnection con = new SqlConnection(consString))
                    {
                        SqlCommand cmd = new SqlCommand("PROC_UPDATE_HDC_DOCUMENT_DETAILS", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg updating document data for : " + claimNumber + " " + DateTime.Now + Environment.NewLine);

                        cmd.Parameters.AddWithValue("@vClaimNumber", claimNumber);
                        cmd.Parameters.AddWithValue("@vFileName", fulClaimForm.PostedFile.FileName);
                        cmd.Parameters.AddWithValue("@vStatus", status);
                        cmd.Parameters.AddWithValue("@dDateModified", DateTime.Now);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    if (status.ToLower().Contains("inserted successfully") || !string.IsNullOrEmpty(status))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["emailidreturn"])))
                        {
                            Session["InUploadDocumentSection"] = "1";
                            string emailtoafterDocumentUpload = Session["emailidreturn"].ToString();
                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail START " + claimNumber + " " + DateTime.Now + Environment.NewLine);
                            fnsendmail(emailtoafterDocumentUpload);

                        }
                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fnsendmail END " + claimNumber + " " + DateTime.Now + Environment.NewLine);
                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg document uploaded successfully for claim " + claimNumber + " " + DateTime.Now + Environment.NewLine);

                        Alert.Show(Message.ToString() + "New Claim number generated - " + claimNumber + " and document uploaded successfully !!", "FrmMainMenu.aspx");
                        return;
                    }
                    else
                    {
                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg document not uploaded  for claim " + claimNumber + " and status is " + status + " " + DateTime.Now + Environment.NewLine);
                        Alert.Show(Message.ToString() + "New Claim number generated - " + claimNumber + " and document not uploadeddue to technical error - you can go to upload screen and try to upload again!!", "FrmMainMenu.aspx");
                        return;
                    }

                }


                if (!fulClaimForm.HasFile)
                {
                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg no document uploaded for claim " + claimNumber + " " + DateTime.Now + Environment.NewLine);
                    Alert.Show(Message.ToString() + " New Claim number generated - " + claimNumber, "FrmMainMenu.aspx");
                }



            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg Error occured in upload documents for claim : " + claimNumber + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Claim number generated - " + claimNumber + " but document not uploaded due to technical error - you can go to upload screen and try to upload again!!", "FrmMainMenu.aspx");
            }
        }

        protected void DrpCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(DrpCustomerName0.SelectedIndex.ToString());
            drpRelation0.SelectedIndex = index;
            drpRelation0.Enabled = false;
        }

        protected void ClearDate(object sender, EventArgs e)
        {
            txtDateOfDischarge.Text = string.Empty;
        }


        public void UploadDoc(string FilePath, string certificate_No, string FileNameWithExtension, string FileType, string ClaimNo, out string DocId, out string ErrorMessage)
        {
            DocId = string.Empty;
            ErrorMessage = string.Empty;
            string status = "";
            try
            {
                string uploadPath = FilePath;
                byte[] bytes = System.IO.File.ReadAllBytes(uploadPath);
                UploadDocToKites(bytes, FileNameWithExtension, FileType,ClaimNo ,out DocId, out status, out ErrorMessage);
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "UploadDoc Error occured while uploading file and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                //ErrorMessage = ex.Message;
            }
        }

        public void UploadDocToKites(byte[] bytes, string FileNameWithExtension, string FileType, out string DocId, out string Status, out string ErrorMessage)
        {
            DocId = string.Empty;
            ErrorMessage = string.Empty;
            Status = string.Empty;
            try
            {
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                System.Text.UTF8Encoding encoding2 = new System.Text.UTF8Encoding();
                Byte[] byteArray2 = encoding2.GetBytes(base64String);

                string parentFolderIndex = ConfigurationManager.AppSettings["parentFolderIndex"].ToString(); //could be different for UAT and PROD hence configured
                clsRequestUploadKites objKiteReq = new clsRequestUploadKites
                {
                    dept = "",
                    author = "",
                    app = "",
                    docTyp = FileType, //"pancard"
                    docN = FileNameWithExtension,
                    previous_Policy_No = "",
                    inwardNo = "",
                    customerId = "",
                    parentFolderIndex = parentFolderIndex, //"4411",
                    application_No = "",
                    certificate_No = "", // certificate_No,
                    master_Policy_No = "",
                    child_Intermediary_Code = "",
                    parent_Intermediary_Code = "",
                    endorsement_No = "",
                    policy_Number = "",
                    claim_Number = "",
                    binData = byteArray2
                };

                UploadService(objKiteReq, out DocId, out Status, out ErrorMessage);
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "UploadDocToKites Error occured while uploading file and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                ErrorMessage = ex.Message;
            }
        }

        private void UploadService(clsRequestUploadKites objKiteReq, out string DocId, out string Status, out string ErrorMessage)
        {
            DocId = "";
            Status = "";
            ErrorMessage = "";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager

            System.Web.Script.Serialization.JavaScriptSerializer JSSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            JSSerializer.MaxJsonLength = Int32.MaxValue;
            string jsonContent = JSSerializer.Serialize(objKiteReq);

            string URL = ConfigurationManager.AppSettings["KitesUploadServiceURL"].ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";

            string IsUseNetworkProxyForKites = ConfigurationManager.AppSettings["IsUseNetworkProxyForKites"].ToString();
            if (IsUseNetworkProxyForKites == "1")
            {
                WebProxy proxy = new WebProxy(ConfigurationManager.AppSettings["proxy_Url"].ToString());

                string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();
                string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();

                proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                proxy.UseDefaultCredentials = true;
                WebRequest.DefaultWebProxy = proxy;
                request.Proxy = proxy;
                request.Proxy = WebRequest.DefaultWebProxy;
                request.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                request.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
            }

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Flush();
                dataStream.Close();
            }

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    string s = reader.ReadToEnd();

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    RootObject r = js.Deserialize<RootObject>(s);
                    DocId = r.docId;
                    Status = r.status;
                    ErrorMessage = "";
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    ErrorMessage = errorText;
                    DocId = "";
                    Status = "";
                    File.AppendAllText(folderPath + "\\log.txt", "UploadService Error occured while uploading file and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                    File.AppendAllText(folderPath + "\\log.txt", "UploadService Error occured while uploading file and error is " + ErrorMessage + " " + DateTime.Now + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                DocId = "";
                Status = "";
                ErrorMessage = ex.Message;
                File.AppendAllText(folderPath + "\\log.txt", "UploadService Error occured while uploading file and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                File.AppendAllText(folderPath + "\\log.txt", "UploadService Error occured while uploading file and error is " + ErrorMessage + " " + DateTime.Now + Environment.NewLine);
                //clsAppLogs.LogException(ex);
            }
            finally
            {
                WebRequest.DefaultWebProxy = null;
            }
        }

        //protected void btnSearchClaimNumber_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
        //        {
        //            if (con.State == ConnectionState.Closed)
        //            {
        //                con.Open();
        //            }

        //            SqlCommand cmd = new SqlCommand("PROC_HDC_CLAIM_DETAILS_BY_CERTIFICATENUM", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@vClaimNumber", txtClaimNumberClaim.Text);
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            if (dr.HasRows)
        //            {
        //                while (dr.Read())
        //                {
        //                    txtAccountNumberClaim.Text = dr["vAccountNumber"].ToString();
        //                    txtCallerNumberClaim.Text = dr["vCallerContactNumber"].ToString();
        //                    txtClaimedAmountClaim.Text = dr["dClaimAmount"].ToString();
        //                    txtClaimIntimationDateClaim.Text = Convert.ToDateTime(dr["dClaimIntimationDate"].ToString()).ToShortDateString();
        //                    txtClaimRegDateClaim.Text = Convert.ToDateTime(dr["dClaimRegDate"].ToString()).ToShortDateString();
        //                    txtCustomerMobileClaim.Text = dr["vMobileNo"].ToString();
        //                    txtCustomerNameclaim.Text = dr["vCustomerName"].ToString();
        //                    txtCustomerTypeClaim.Text = dr["vCustomerType"].ToString();
        //                    txtDateofAdmissionClaim.Text = Convert.ToDateTime(dr["dDateOfAdmission"].ToString()).ToShortDateString();
        //                    txtDateofDischargeClaim.Text = Convert.ToDateTime(dr["dDateOfDischarge"].ToString()).ToShortDateString();
        //                    txtLineofBusinessClaim.Text = "Health";
        //                    txtMasterPolicyHolderClaim.Text = dr["vMasterPolicyHolder"].ToString();
        //                    txtMasterPolicyNumClaim.Text = dr["vMasterPolicyNo"].ToString();
        //                    txtMemberIDClaim.Text = dr["vMemberID"].ToString();
        //                    txtMemberNameClaim.Text = dr["vMemberName"].ToString();
        //                    txtPolicyEndDateClaim.Text = Convert.ToDateTime(dr["vPolicyEndDate"].ToString()).ToShortDateString();
        //                    txtPolicyStartDateClaim.Text = Convert.ToDateTime(dr["vPolicyStartdate"].ToString()).ToShortDateString();
        //                    txtProductNameClaim.Text = dr["vProductName"].ToString();
        //                    txtRemarkClaim.Text = dr["vRemark"].ToString();
        //                }

        //                Response.Redirect("~/FrmHDCClaimReg.aspx/#tab2");

        //            }
        //            else
        //            {
        //                Alert.Show("No Data Found !!");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, " btnSearchClaimNumber_Click");
        //        Alert.Show("Some Error Occured. Kindly contact administrator.");

        //    }
        //}

        public void UploadDocToKites(byte[] bytes, string FileNameWithExtension, string FileType,string ClaimNo, out string DocId, out string Status, out string ErrorMessage)
        {
            DocId = string.Empty;
            ErrorMessage = string.Empty;
            Status = string.Empty;
            try
            {
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                System.Text.UTF8Encoding encoding2 = new System.Text.UTF8Encoding();
                Byte[] byteArray2 = encoding2.GetBytes(base64String);

                string parentFolderIndex = ConfigurationManager.AppSettings["parentFolderIndex"].ToString(); //could be different for UAT and PROD hence configured
                clsRequestUploadKites objKiteReq = new clsRequestUploadKites
                {
                    dept = "",
                    author = "",
                    app = "",
                    docTyp = FileType, //"pancard"
                    docN = FileNameWithExtension,
                    previous_Policy_No = "",
                    inwardNo = "",
                    customerId = "",
                    parentFolderIndex = parentFolderIndex, //"4411",
                    application_No = "",
                    certificate_No = "", // certificate_No,
                    master_Policy_No = "",
                    child_Intermediary_Code = "",
                    parent_Intermediary_Code = "",
                    endorsement_No = "",
                    policy_Number = "",
                    claim_Number = ClaimNo,
                    binData = byteArray2
                };

                UploadService(objKiteReq, out DocId, out Status, out ErrorMessage);
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "UploadDocToKites Error occured while uploading file and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                ErrorMessage = ex.Message;
            }
        }

    }
}