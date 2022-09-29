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
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmHDCClaimDocUpload : System.Web.UI.Page
    {

        public Cls_General_Functions wsGen = new Cls_General_Functions();
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

        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Session["ID"] = Session["vUserLoginId"].ToString();
                if (Session["vUserLoginId"] != null)
                {
                    if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
                    {
                        bool chkAuth;
                        string pageName = this.Page.ToString().Substring(4, this.Page.ToString().Substring(4).Length - 5) + ".aspx";
                        chkAuth = wsGen.Fn_Check_Rights_For_Page(Session["vRoleCode"].ToString(), pageName);
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
            }
        }

        protected void btnSearchClaim_Click(object sender, EventArgs e)
        {
            try
            {
                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                using (SqlConnection con = new SqlConnection(consString))
                {
                    SqlCommand cmd = new SqlCommand("PROC_HDC_GET_CLAIM", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vClaimNumber", txtClaimNumber.Text);
                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload fetching certificate number for : " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);
                    con.Open();

                    SqlDataReader drCheck = cmd.ExecuteReader();
                    {
                        if (drCheck.HasRows)
                        {
                            while (drCheck.Read())
                            {
                               if(String.IsNullOrEmpty(drCheck[0].ToString()))
                                {
                                    divClaimLabel.Visible = false;
                                    divClaimUpload.Visible = false;
                                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload certificate number not found for : " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);
                                    Alert.Show("No Record Found for the claim number. Please Try Again !!");
                                }
                               else
                                {
                                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload certificate number found for : " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);
                                    hdnCertificateNumber.Value = drCheck[0].ToString();
                                    divClaimLabel.Visible = true;
                                    divClaimUpload.Visible = true;
                                }
                            }

                        }
                        else
                        {
                            divClaimLabel.Visible = false;
                            divClaimUpload.Visible = false;
                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload certificate number not found for : " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);
                            Alert.Show("No Record Found for the claim number. Please Try Again !!");
                        }
                    }
                }

                using (SqlConnection con = new SqlConnection(consString))
                {
                    SqlCommand cmd = new SqlCommand("PROC_HDC_GET_MASTERPOLICYHOLDERNAME", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vCertificateNumber", !string.IsNullOrEmpty(hdnCertificateNumber.Value) ? hdnCertificateNumber.Value : "");
                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload fetching data for Certificate Number  : " + hdnCertificateNumber.Value + "  " + DateTime.Now + Environment.NewLine);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet dsCertificateDetails = new DataSet();
                    sda.Fill(dsCertificateDetails);
                    if (dsCertificateDetails.Tables[0] != null)
                    {
                        if (dsCertificateDetails.Tables[0].Rows.Count > 0)
                        {
                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload data fetched for : " + hdnCertificateNumber.Value + " " + DateTime.Now + Environment.NewLine);
                            hdnMasterPolicyHolderName.Value = dsCertificateDetails.Tables[0].Rows[0]["vMasterPolicyHolder"].ToString();
                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload fetching data for MasterPolicyHolderName  : " + hdnMasterPolicyHolderName.Value + "  " + DateTime.Now + Environment.NewLine);
                        }
                    }
                    else
                    {
                        Alert.Show("No Data Found for MasterPolicyHolderName!!");
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload Error occured in search claim for : " + txtClaimNumber.Text + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Document not uploaded. Something went wrong - Try again later !!", "FrmMainMenu.aspx");
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload starting uploading documents for claim number " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);

                if (String.IsNullOrEmpty(hdnCertificateNumber.Value))
                {
                    File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload hdnCertificateNumber is null " + DateTime.Now + Environment.NewLine);

                    Alert.Show("Something went wrong !!. Try again later", "FrmMainMenu.aspx");
                    return;
                }
                else
                {
                    int nMaxFileSize = 4194304; //4 MB

                    if (fulClaimForm.HasFile)
                    {
                        if (fulClaimForm.PostedFile.ContentLength > nMaxFileSize)
                        {
                            Alert.Show("Maximum Filesize Allowed is 4 MB for upload document");
                            return;
                        }

                        string fileExtension = Path.GetExtension(fulClaimForm.PostedFile.FileName);

                        string[] extensionsAllowed = ConfigurationManager.AppSettings["uploadExtension"].ToString().Split(',');

                        if (!extensionsAllowed.Contains(fileExtension))
                        {
                            Alert.Show("Uploaded file extension is not allowed");
                            return;
                        }

                        //if (extension != ".zip" || extension!=".pdf" || extension != ".jpeg" || extension != ".xls" || extension != ".xlsx" || extension != ".doc" || extension != ".docx" || extension != ".pdf" || extension != ".pdf" || extension != ".pdf" || extension != ".pdf" || extension != ".pdf" || extension != ".pdf")
                        //{
                        //    Alert.Show("Uploaded file extension is not allowed for Claim Form - Claim number generated - " + claimNumber + " - you can go to upload screen and try to upload again.", "FrmMainMenu.aspx");
                        //    return;
                        //}

                        string uploadPath = folderPath + "\\" + Path.GetFileName(fulClaimForm.PostedFile.FileName);
                        fulClaimForm.SaveAs(uploadPath);

                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload upload file saved for : " + txtClaimNumber.Text + " and file name is " + fulClaimForm.PostedFile.FileName + " " + DateTime.Now + Environment.NewLine);

                        byte[] requestedByte = System.IO.File.ReadAllBytes(uploadPath);

                        

                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload staring calling service to upload document " + DateTime.Now + Environment.NewLine);

                        //string base64String = Convert.ToBase64String(requestedByte, 0, requestedByte.Length);
                        //System.Text.UTF8Encoding encoding2 = new System.Text.UTF8Encoding();
                        //Byte[] byteArray2 = encoding2.GetBytes(base64String);

                        //ServiceReference1.CustomerPortalServiceClient objClient = new ServiceReference1.CustomerPortalServiceClient();
                        //string status = proxy.CreateDocumentDMS(username, password, InwardId, ApplicationNo, CustomerId, DocumentUniqNo, PolicyNo, ClaimNo, Source, FileName, requestedByte, DocumentType);

                        //string status = objClient.CreateDocumentDMS(ConfigurationManager.AppSettings["strUserIdDocs"].ToString(), ConfigurationManager.AppSettings["strPasswordDocs"].ToString(), "", "", "", "", hdnCertificateNumber.Value.ToString(), txtClaimNumber.Text.Trim(), "HDCPASS", fulClaimForm.PostedFile.FileName, byteArray2, "");
                        //string status = objClient.CreateDocumentDMS(ConfigurationManager.AppSettings["strUserIdDocs"].ToString(), ConfigurationManager.AppSettings["strPasswordDocs"].ToString(), "", "", "1000045529", "", hdnCertificateNumber.Value.ToString(), txtClaimNumber.Text.Trim(), "", fulClaimForm.PostedFile.FileName, requestedByte, "claimForm");

                        string FilePath = uploadPath;
                        string certificate_No = hdnCertificateNumber.Value.ToString();
                        string FileNameWithExtension = Path.GetFileName(FilePath);
                        string FileType = "";
                        string DocId = "";
                        string ErrorMessageKites = "";
                        UploadDoc(FilePath, certificate_No, FileNameWithExtension, FileType, txtClaimNumber.Text, out DocId, out ErrorMessageKites);
                        string status = DocId;


                        #region
                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload end calling service to upload document " + Convert.ToString(status) + DateTime.Now + Environment.NewLine);
                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload end calling service to upload document " + DateTime.Now + Environment.NewLine);

                        string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                        using (SqlConnection con = new SqlConnection(consString))
                        {
                            SqlCommand cmdInsert = new SqlCommand("PROC_INSERT_HDC_DOCUMENT_DETAILS_HISTORY", con);
                            cmdInsert.CommandType = CommandType.StoredProcedure;

                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload inserting document data in history for : " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);

                            cmdInsert.Parameters.AddWithValue("@vClaimNumber", txtClaimNumber.Text);
                            con.Open();
                            cmdInsert.ExecuteNonQuery();
                            

                            SqlCommand cmd = new SqlCommand("PROC_UPDATE_HDC_DOCUMENT_DETAILS", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload updating document data for : " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);

                            cmd.Parameters.AddWithValue("@vClaimNumber", txtClaimNumber.Text);
                            cmd.Parameters.AddWithValue("@vFileName", fulClaimForm.PostedFile.FileName);
                            cmd.Parameters.AddWithValue("@vStatus", status);
                            cmd.Parameters.AddWithValue("@dDateModified", DateTime.Now);
                            
                            cmd.ExecuteNonQuery();
                        }

                        if (status.ToLower().Contains("inserted successfully") || !string.IsNullOrEmpty(status))
                        {
                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload document uploaded successfully for claim " + status + " " + DateTime.Now + Environment.NewLine);
                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload document uploaded successfully for claim " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);

                            Alert.Show("Document uploaded successfully for  - " + txtClaimNumber.Text, "FrmMainMenu.aspx");


                            using (SqlConnection con = new SqlConnection(consString))
                            {
                                //txtCertificateNumber,txtClaimRegistrationDate,txtClaimIntimationDate,txtCustomerName,txtMasterPolicyHolder
                                SqlCommand cmdSelect = new SqlCommand("SELECT vCertificateNumber, dClaimRegDate,dClaimIntimationDate,vMemberName FROM TBL_HDC_CLAIMS_TABLE WHERE vClaimNumber  ='" + txtClaimNumber.Text + "'", con);
                                cmdSelect.CommandType = CommandType.Text;
                                //cmd.Parameters.AddWithValue("@vClaimNumber", txtClaimNumber.Text);
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload inserting document data in history for : " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);

                                //cmdInsert.Parameters.AddWithValue("@vClaimNumber", txtClaimNumber.Text);
                                con.Open();
                                //cmdInsert.ExecuteNonQuery();
                                SqlDataAdapter sda = new SqlDataAdapter(cmdSelect);
                                DataSet dsCertificateDetails = new DataSet();
                                sda.Fill(dsCertificateDetails);
                                if (dsCertificateDetails.Tables[0] != null)
                                {
                                    if (dsCertificateDetails.Tables[0].Rows.Count > 0)
                                    {
                                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload data fetched for : " + hdnCertificateNumber.Value + " " + DateTime.Now + Environment.NewLine);
                                        //hdnMasterPolicyHolderName.Value = dsCertificateDetails.Tables[0].Rows[0]["vMasterPolicyHolder"].ToString();
                                        Session["ddRegSDate"] = dsCertificateDetails.Tables[0].Rows[0]["dClaimRegDate"];

                                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload RegistrationDate Before Conversion : " + Convert.ToString(Session["ddRegSDate"]) + " " + DateTime.Now + Environment.NewLine);
                                        hdnClaimRegistrationDate.Value = Convert.ToDateTime(dsCertificateDetails.Tables[0].Rows[0]["dClaimRegDate"]).ToString("dd/MM/yyyy");
                                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload RegistrationDate After Conversion : " + hdnClaimRegistrationDate.Value + " " + DateTime.Now + Environment.NewLine);


                                        //hdnClaimRegistrationDate.Value = Convert.ToDateTime(dsCertificateDetails.Tables[0].Rows[0]["dClaimRegDate"]).ToString("dd/MM/yyyy");
                                        //hdnClaimRegistrationDate.Value = DateTime.ParseExact(dsCertificateDetails.Tables[0].Rows[0]["dClaimRegDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        //hdnClaimIntimationDate.Value = dsCertificateDetails.Tables[0].Rows[0]["dClaimIntimationDate"].ToString();
                                        hdnCustomerName.Value = dsCertificateDetails.Tables[0].Rows[0]["vMemberName"].ToString();
                                    }
                                }
                                else
                                {
                                    Alert.Show("No Data Found !!");
                                    return;
                                }
                                string emailid = FindEmailId(Session["ID"].ToString());
                                if (!string.IsNullOrEmpty(emailid))
                                {
                                    fnsendmail(emailid);
                                }
                                else
                                {
                                    Alert.Show("Email Id not exists !!");
                                }
                            }
                            

                            return;
                        }
                        else
                        {
                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload document not uploaded  for claim " + txtClaimNumber.Text + " and status is " + status + " " + DateTime.Now + Environment.NewLine);
                            Alert.Show("Document not uploaded due to technical error for - " + txtClaimNumber.Text + " Try again later!!", "FrmMainMenu.aspx");
                            return;
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload Error occured in upload for : " + txtClaimNumber.Text + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Document not uploaded. Something went wrong - Try again later !!", "FrmMainMenu.aspx");
            }
        }

        private void fnsendmail(string emailId)
        {
            try
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload fnsendmail START : "+ DateTime.Now + Environment.NewLine);
                //string emailId = txtEmailforPolicy.Text.Trim();
                string strPath = string.Empty;
                string MailBody = string.Empty;
                string userid = Convert.ToString(Session["ID"]);
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "Schedule_Claim_Email_Body_ForDocumentUpload_Trigger.html";
                MailBody = File.ReadAllText(strPath);
                string user_name = FindUserName(userid);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload fnsendmail :: UserName " + user_name.ToString() + DateTime.Now + Environment.NewLine);
                MailBody = MailBody.Replace("#CustomerName", user_name);
                MailBody = MailBody.Replace("#Claim_Number", txtClaimNumber.Text);
                MailBody = MailBody.Replace("#Policy_Number", hdnCertificateNumber.Value);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload fnsendmail :: ClaimRegistrationDate " + hdnClaimRegistrationDate.Value +" " + DateTime.Now + Environment.NewLine);
                //DateTime dtClaimRegDate = Convert.ToDateTime(hdnClaimRegistrationDate.Value);
                //string strClaimRegDate = dtClaimRegDate.ToString("dd/MMM/yyyy");

                string strClaimRegDate = DateTime.ParseExact(hdnClaimRegistrationDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload fnsendmail :: After Converting dd/MMM/yyyy ClaimRegistrationDate " + strClaimRegDate + " " + DateTime.Now + Environment.NewLine);
                MailBody = MailBody.Replace("#Date_Of_Registration", strClaimRegDate);
                //MailBody = MailBody.Replace("#Intimation_Time", hdnClaimIntimationDate.Value);
                MailBody = MailBody.Replace("#Customer_Name", hdnCustomerName.Value);
                MailBody = MailBody.Replace("#Master_Policy_Holder_Name", hdnMasterPolicyHolderName.Value);

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                mm.Subject = string.Format(ConfigurationManager.AppSettings["Schedule_Claim_DocUpload_email_Subject"], txtClaimNumber.Text);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload fnsendmail START : " + mm.Subject + DateTime.Now + Environment.NewLine);
                mm.Body = MailBody;
                mm.To.Add(emailId);
                //File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload fnsendmail START : " + mm.Subject + DateTime.Now + Environment.NewLine);
                //mm.CC.Add();
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(filename);
                //mm.Attachments.Add(attachment);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload smtpClient START : " + DateTime.Now + Environment.NewLine);
                smtpClient.Send(mm);
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload smtpClient START : " + DateTime.Now + Environment.NewLine);
                //Alert.Show("Policy schedule sent on Email ID " + emailId);
                //Response.Write(string.Format("<script>alert('Email sent successfully to {0} ')</script>",emailId) );
                //IsEmailRequest = true;
                //Session["ErrorCallingPage"] = "FrmHDCClaimDocUpload.aspx";
                //string vStatusMsg = "Policy sent to " + emailId;
                //EmailSentMessage = vStatusMsg;
                //Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "Error in fnsendmail on  FrmHDCClaimDocUpload for claim numberand error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Some Error Occured while sending email to " + emailId);
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
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload Error occured while finding Email ID  and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
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
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload Error occured while finding Username and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
            }
            return username;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
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
                UploadDocToKites(bytes, FileNameWithExtension, FileType, ClaimNo,out DocId, out status, out ErrorMessage);
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "UploadDoc Error occured while uploading file and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                //ErrorMessage = ex.Message;
            }
        }

        public void UploadDocToKites(byte[] bytes, string FileNameWithExtension, string FileType, string ClaimNo, out string DocId, out string Status, out string ErrorMessage)
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
    }
}