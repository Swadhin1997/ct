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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmEHDCClaimReg : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");
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

                clnClaimIntimationDate.DateMax = DateTime.Now;
                txtClaimRegistrationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtClaimRegistrationDate.Attributes.Add("readonly", "readonly");
                txtClaimIntimationDate.Attributes.Add("readonly", "readonly");
                txtDateOfAdmission.Attributes.Add("readonly", "readonly");
                txtDateOfDischarge.Attributes.Add("readonly", "readonly");
                txtPolicyStartDate.Attributes.Add("readonly", "readonly");
                txtPolicyEndDate.Attributes.Add("readonly", "readonly");
            }
        }

        protected void btnSearchCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtCertificateNumber.Text))
                {
                    ResetControls();
                    Alert.Show("Enter Policy Number !!");
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
                        cmd.Parameters.AddWithValue("@vCertificateNumber", txtCertificateNumber.Text);
                        File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg fetching data for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dtCertificateDetails = new DataTable();
                        sda.Fill(dtCertificateDetails);

                        if (dtCertificateDetails != null)
                        {
                            if (dtCertificateDetails.Rows.Count > 0)
                            {
                                File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);

                                txtCustomerName.Text = dtCertificateDetails.Rows[0]["vCustomerName"].ToString();
                                txtCustomerType.Text = dtCertificateDetails.Rows[0]["vCustomerType"].ToString();
                                txtCustomerMobile.Text = dtCertificateDetails.Rows[0]["vMobileNo"].ToString();
                                txtMasterPolicyNumber.Text = dtCertificateDetails.Rows[0]["vMasterPolicyNo"].ToString();
                                txtMasterPolicyHolder.Text = dtCertificateDetails.Rows[0]["vMasterPolicyHolder"].ToString();
                                txtProductName.Text = dtCertificateDetails.Rows[0]["vProductName"].ToString();
                                txtPolicyStartDate.Text = Convert.ToDateTime(dtCertificateDetails.Rows[0]["vPolicyStartdate"].ToString()).ToString("dd/MM/yyyy");
                                txtPolicyEndDate.Text = Convert.ToDateTime(dtCertificateDetails.Rows[0]["vPolicyEndDate"].ToString()).ToString("dd/MM/yyyy");
                            }

                            else
                            {
                                File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg no data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
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
                File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg Error occured in search certificate : " + txtCertificateNumber.Text + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Claim number not generated - something went wrong - Try again later !!", "FrmMainMenu.aspx");
            }
        }

        private void ResetControls()
        {
            txtCustomerName.Text = string.Empty;
            txtCustomerType.Text = string.Empty;
            txtCustomerMobile.Text = string.Empty;
            txtMasterPolicyNumber.Text = string.Empty;
            txtMasterPolicyHolder.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtPolicyStartDate.Text = string.Empty;
            txtPolicyEndDate.Text = string.Empty;
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

                if (String.IsNullOrEmpty(txtCertificateNumber.Text))
                {
                    Alert.Show("Enter Policy Number !!");
                    txtCertificateNumber.Focus();
                    return;
                }


                if (String.IsNullOrEmpty(txtCustomerName.Text))
                {
                    Alert.Show("Enter Customer Name !!");
                    txtCustomerName.Focus();
                    return;
                }

                if (String.IsNullOrEmpty(txtCustomerType.Text))
                {
                    Alert.Show("Enter Customer Type !!");
                    txtCustomerType.Focus();
                    return;
                }


                Regex pattern = new Regex(@"^\d{10}$");
                if (!pattern.IsMatch(txtCustomerMobile.Text))
                {
                    Alert.Show("Enter valid mobile number !!");
                    txtCustomerMobile.Focus();
                    return;
                }


                if (String.IsNullOrEmpty(txtCustomerMobile.Text))
                {
                    Alert.Show("Enter Customer mobile number !!");
                    txtCustomerMobile.Focus();
                    return;
                }




                if (String.IsNullOrEmpty(txtMasterPolicyNumber.Text))
                {
                    Alert.Show("Enter Master Policy Number !!");
                    txtMasterPolicyNumber.Focus();
                    return;
                }

                if (String.IsNullOrEmpty(txtMasterPolicyHolder.Text))
                {
                    Alert.Show("Enter Master Policy Holder !!");
                    txtMasterPolicyHolder.Focus();
                    return;
                }

                if (String.IsNullOrEmpty(txtLineOfBusiness.Text))
                {
                    Alert.Show("Enter Line of Business !!");
                    txtLineOfBusiness.Focus();
                    return;
                }

                if (String.IsNullOrEmpty(txtProductName.Text))
                {
                    Alert.Show("Enter Product Name !!");
                    txtProductName.Focus();
                    return;
                }

                if (String.IsNullOrEmpty(txtPolicyStartDate.Text))
                {
                    Alert.Show("Enter Policy Start date !!");
                    txtPolicyStartDate.Focus();
                    return;
                }

                if (String.IsNullOrEmpty(txtPolicyEndDate.Text))
                {
                    Alert.Show("Enter Policy End date !!");
                    txtPolicyEndDate.Focus();
                    return;
                }


                DateTime datePolicyStart = DateTime.ParseExact(txtPolicyStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime datePolicyEnd = DateTime.ParseExact(txtPolicyEndDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (datePolicyStart > datePolicyEnd)
                {
                    string strPolicyStartDate = datePolicyStart.ToString("dd-MMM-yyyy");
                    string strPolicyEndDate = datePolicyEnd.ToString("dd-MMM-yyyy");

                    string Message = string.Format("Policy start date {0} cannot be greater than Policy End Date {1}", strPolicyStartDate, strPolicyEndDate);
                    Alert.Show(Message);
                    return;
                }


                if (String.IsNullOrEmpty(txtClaimIntimationDate.Text))
                {
                    Alert.Show("Claim Intimation Date cannot be blank !!");
                    txtClaimIntimationDate.Focus();
                    return;
                }

                DateTime dateClaimIntimation = DateTime.ParseExact(txtClaimIntimationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if ( DateTime.Compare(datePolicyStart , dateClaimIntimation) > 0)
                {
                    string strPolicyStartDate = datePolicyStart.ToString("dd-MMM-yyyy");
                    string strClaimIntimationDate = dateClaimIntimation.ToString("dd-MMM-yyyy");

                    string Message = string.Format("Claim Intimation date {0} cannot be before the Policy Start Date {1}", strClaimIntimationDate, strPolicyStartDate);
                    Alert.Show(Message);
                    return;
                }

                if (String.IsNullOrEmpty(txtDateOfAdmission.Text))
                {
                    Alert.Show("Date of Admission cannot be blank !!");
                    txtDateOfAdmission.Focus();
                    return;
                }

                if (!String.IsNullOrEmpty(txtDateOfDischarge.Text))
                {
                    if (DateTime.ParseExact(txtDateOfDischarge.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        Alert.Show("Date Of Discharge cannot be less than Date Of Admission");
                        return;
                    }
                }

                if (DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(txtPolicyStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) || DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtPolicyEndDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    Alert.Show("Date Of Admission should be between policy start and end date");
                    return;
                }


                if (String.IsNullOrEmpty(drpRelation.SelectedValue.ToString()))
                {
                    Alert.Show("Relation with claimant must selected !!");
                    return;
                }

                if (string.IsNullOrEmpty(txtMemberID.Text))
                {
                    Alert.Show("Member ID can not be blank !!");
                    txtMemberID.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtMemberName.Text))
                {
                    Alert.Show("Member Name can not be blank !!");
                    txtMemberName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtClaimAmount.Text))
                {
                    Alert.Show("Claim amount can not be blank !!");
                    txtClaimAmount.Focus();
                    return;
                }

                Regex nonNumericRegex = new Regex(@"\D");
                if (nonNumericRegex.IsMatch(txtClaimAmount.Text))
                {
                    Alert.Show("Please Enter valid Claim amount!!");
                    txtClaimAmount.Focus();
                    return;
                }


                if (string.IsNullOrEmpty(txtAccountNumber.Text))
                {
                    Alert.Show("Account Number can not be blank !!");
                    txtAccountNumber.Focus();
                    return;
                }

                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                using (SqlConnection con = new SqlConnection(consString))
                {
                    SqlCommand cmdCheck = new SqlCommand("PROC_GET_CLAIM_REGISTER_COUNT", con);
                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.Parameters.AddWithValue("@vCertificateNumber", txtCertificateNumber.Text);
                    cmdCheck.Parameters.AddWithValue("@dDateOfAdmission", DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));

                    File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg getting existing claim data for : " + txtCertificateNumber.Text + " and date of admission " + txtDateOfAdmission.Text + " " + DateTime.Now + Environment.NewLine);

                    con.Open();

                    SqlDataReader drCheck = cmdCheck.ExecuteReader();
                    {
                        if (drCheck.HasRows)
                        {
                            while (drCheck.Read())
                            {
                                if (!String.IsNullOrEmpty(drCheck[0].ToString()))
                                {
                                    Alert.Show("Claim " + drCheck[0] + " already register for the selected Admission Date !!");
                                    return;
                                }
                            }

                        }
                    }

                    drCheck.Close();


                    SqlCommand cmd = new SqlCommand("PROC_INSERT_HDC_CLAIMS_DETAILS", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg inserting claim data for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);

                    cmd.Parameters.AddWithValue("@vCertificateNumber", txtCertificateNumber.Text);
                    cmd.Parameters.AddWithValue("@vClaimIntimationBy", cmbClaimIntimationBy.SelectedText);
                    cmd.Parameters.AddWithValue("@vCallerContactNumber", txtCallerNumber.Text);
                    cmd.Parameters.AddWithValue("@dDateOfAdmission", DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@dDateOfDischarge", String.IsNullOrEmpty(txtDateOfDischarge.Text) ? txtDateOfDischarge.Text : DateTime.ParseExact(txtDateOfDischarge.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                    cmd.Parameters.AddWithValue("@dDateOfIntimation", DateTime.ParseExact(txtClaimIntimationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@dDateOfRegistration", DateTime.ParseExact(txtClaimRegistrationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@vRelation", drpRelation.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@vMemberID", txtMemberID.Text.ToString());
                    cmd.Parameters.AddWithValue("@vMemberName", txtMemberName.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@dClaimAmount", txtClaimAmount.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@vAccountNumber", txtAccountNumber.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@vRemark", txtRemark.Text.Trim().ToString());
                    
                    SqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                //Alert.Show("Claim Number generated - " + dr[0].ToString());     
                                claimNumber = dr[0].ToString();
                                File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg claim number generated for : " + txtCertificateNumber.Text + " and claim number is " + claimNumber + " " + DateTime.Now + Environment.NewLine);

                                //lblMessage.Text = "Claim Number generated - " + dr[0].ToString() + " - wait while the documents are getting uploaded";
                            }

                        }
                    }

                    con.Close();
                }

                SaveClaimDetails(claimNumber);
                UploadDocuments(claimNumber);


            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg Error occured in insert claim data for : " + txtCertificateNumber.Text + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Claim number not generated - something went wrong - Try again later !!", "FrmMainMenu.aspx");
            }
        }

        private void SaveClaimDetails(string claimNumber)
        {
            try
            {
                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                using (SqlConnection con = new SqlConnection(consString))
                {
                    SqlCommand cmd = new SqlCommand("PROC_INSERT_TBL_HDC_CLAIMS_CHILD_TABLE", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg SaveClaimDetails data for : " + claimNumber + " " + DateTime.Now + Environment.NewLine);

                    cmd.Parameters.AddWithValue("@vClaimNumber", claimNumber.ToString());
                    cmd.Parameters.AddWithValue("@vPolicyNumber", txtCertificateNumber.Text);
                    cmd.Parameters.AddWithValue("@vCustomerName", txtCustomerName.Text);
                    cmd.Parameters.AddWithValue("@vCustomerType", txtCustomerType.Text);
                    cmd.Parameters.AddWithValue("@vCustomerMobile", txtCustomerMobile.Text);
                    cmd.Parameters.AddWithValue("@vMasterPolicyNumber", txtMasterPolicyNumber.Text);
                    cmd.Parameters.AddWithValue("@vMasterPolicyHolder", txtMasterPolicyHolder.Text);
                    cmd.Parameters.AddWithValue("@vLOB", txtLineOfBusiness.Text);
                    cmd.Parameters.AddWithValue("@vProductName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@dPolicyStartDate", DateTime.ParseExact(txtPolicyStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@dPolicyEndDate", DateTime.ParseExact(txtPolicyEndDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SendSMSforClaimIntimation(txtCustomerMobile.Text, claimNumber);
                }
            }
            catch (Exception ex)
            {
                
                File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg Error occured in SaveClaimDetails for claim : " + claimNumber + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                //Alert.Show("Claim number generated - " + claimNumber + " but document not uploaded due to technical error - you can go to upload screen and try to upload again!!", "FrmMainMenu.aspx");
                Alert.Show("Claim number generated - " + claimNumber + " but some error occured!!", "FrmMainMenu.aspx");
            }
        }

        private void SendSMSforClaimIntimation(string MobileNumber, string ClaimNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(MobileNumber) && (MobileNumber.Length == 10))
                {

                    string SmsBody = ConfigurationManager.AppSettings["HDCClaimIntimationSMSBody"].ToString();
                    SmsBody = SmsBody.Replace("@claimNumber", ClaimNumber);

                    using (SqlConnection cnCon = new SqlConnection(ConfigurationManager.ConnectionStrings["cnConnect"].ToString()))
                    {
                        if (cnCon.State == ConnectionState.Closed)
                        {
                            cnCon.Open();
                        }
                        SqlCommand cmd = new SqlCommand("INSERT_DATA_CUSTOMER_SMS_LOG_HDC_SCHEDULER", cnCon);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mobile", MobileNumber);
                        cmd.Parameters.AddWithValue("@msg", SmsBody);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg Error occured in SendSMSforClaimIntimation for mobile number : " + MobileNumber + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
            }
        }

        private void UploadDocuments(string claimNumber)
        {
            try
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg starting uploading documents for claim number " + claimNumber + " " + DateTime.Now + Environment.NewLine);

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

                    File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg upload file saved for : " + txtCertificateNumber.Text + " and file name is " + fulClaimForm.PostedFile.FileName + " " + DateTime.Now + Environment.NewLine);


                    byte[] requestedByte = System.IO.File.ReadAllBytes(uploadPath);
                    string base64String = Convert.ToBase64String(requestedByte, 0, requestedByte.Length);
                    System.Text.UTF8Encoding encoding2 = new System.Text.UTF8Encoding();
                    Byte[] byteArray2 = encoding2.GetBytes(base64String);



                    File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg staring calling service to upload document " + DateTime.Now + Environment.NewLine);

                    ServiceReference1.CustomerPortalServiceClient objClient = new ServiceReference1.CustomerPortalServiceClient();
                    //string status = proxy.CreateDocumentDMS(username, password, InwardId, ApplicationNo, CustomerId, DocumentUniqNo, PolicyNo, ClaimNo, Source, FileName, requestedByte, DocumentType);

                    string status = objClient.CreateDocumentDMS(ConfigurationManager.AppSettings["strUserIdDocs"].ToString(), ConfigurationManager.AppSettings["strPasswordDocs"].ToString(), "", "", "", "", txtCertificateNumber.Text, claimNumber, "HDCPASS", fulClaimForm.PostedFile.FileName, byteArray2, "");
                    

                    

                    File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg end calling service to upload document " + DateTime.Now + Environment.NewLine);

                    string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                    using (SqlConnection con = new SqlConnection(consString))
                    {
                        SqlCommand cmd = new SqlCommand("PROC_UPDATE_HDC_DOCUMENT_DETAILS", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg updating document data for : " + claimNumber + " " + DateTime.Now + Environment.NewLine);

                        cmd.Parameters.AddWithValue("@vClaimNumber", claimNumber);
                        cmd.Parameters.AddWithValue("@vFileName", fulClaimForm.PostedFile.FileName);
                        cmd.Parameters.AddWithValue("@vStatus", status);
                        cmd.Parameters.AddWithValue("@dDateModified", DateTime.Now);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    if (status.ToLower().Contains("inserted successfully") || !string.IsNullOrEmpty(status))
                    {
                        File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg document uploaded successfully for claim " + claimNumber + " " + DateTime.Now + Environment.NewLine);

                        Alert.Show("Claim number generated - " + claimNumber + " and document uploaded successfully !!", "FrmMainMenu.aspx");
                        return;
                    }
                    else
                    {
                        File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg document not uploaded  for claim " + claimNumber + " and status is " + status + " " + DateTime.Now + Environment.NewLine);
                        Alert.Show("Claim number generated - " + claimNumber + " and document not uploadeddue to technical error - you can go to upload screen and try to upload again!!", "FrmMainMenu.aspx");
                        return;
                    }

                }


                if (!fulClaimForm.HasFile)
                {
                    File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg no document uploaded for claim " + claimNumber + " " + DateTime.Now + Environment.NewLine);
                    Alert.Show("Claim number generated - " + claimNumber, "FrmMainMenu.aspx");
                }



            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmEHDCClaimReg Error occured in upload documents for claim : " + claimNumber + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Claim number generated - " + claimNumber + " but document not uploaded due to technical error - you can go to upload screen and try to upload again!!", "FrmMainMenu.aspx");
            }
        }
    }
}