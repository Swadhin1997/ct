using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmEHDCClaimDocUpload : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
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
                        string base64String = Convert.ToBase64String(requestedByte, 0, requestedByte.Length);
                        System.Text.UTF8Encoding encoding2 = new System.Text.UTF8Encoding();
                        Byte[] byteArray2 = encoding2.GetBytes(base64String);



                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg staring calling service to upload document " + DateTime.Now + Environment.NewLine);

                        ServiceReference1.CustomerPortalServiceClient objClient = new ServiceReference1.CustomerPortalServiceClient();
                        //string status = proxy.CreateDocumentDMS(username, password, InwardId, ApplicationNo, CustomerId, DocumentUniqNo, PolicyNo, ClaimNo, Source, FileName, requestedByte, DocumentType);

                        string status = objClient.CreateDocumentDMS(ConfigurationManager.AppSettings["strUserIdDocs"].ToString(), ConfigurationManager.AppSettings["strPasswordDocs"].ToString(), "", "", "", "", hdnCertificateNumber.Value, txtClaimNumber.Text, "HDCPASS", fulClaimForm.PostedFile.FileName, byteArray2, "");

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
                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload document uploaded successfully for claim " + txtClaimNumber.Text + " " + DateTime.Now + Environment.NewLine);

                            Alert.Show("Document uploaded successfully for  - " + txtClaimNumber.Text, "FrmMainMenu.aspx");
                            return;
                        }
                        else
                        {
                            File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload document not uploaded  for claim " + txtClaimNumber.Text + " and status is " + status + " " + DateTime.Now + Environment.NewLine);
                            Alert.Show("Document not uploaded due to technical error for - " + txtClaimNumber.Text + " Try again later!!", "FrmMainMenu.aspx");
                            return;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimDocUpload Error occured in upload for : " + txtClaimNumber.Text + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("Document not uploaded. Something went wrong - Try again later !!", "FrmMainMenu.aspx");
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}