using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Obout.Grid;
using Obout.Interface;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace ProjectPASS
{
    public partial class FrmForgotPassword : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Count_MaxSendEmail"] = 1;

                Random random = new Random();
                int randomNumber1 = random.Next(1, 99);
                int randomNumber2 = random.Next(1, 99);
                ViewState["TotalOfRandom"] = randomNumber1 + randomNumber2;
                lblQuestion.Text = randomNumber1.ToString() + " + " + randomNumber2.ToString();
            }
            if (Session["vUserLoginId"] != null)
            {
                Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                return;
            }

            
            
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string pswd = string.Empty;
            Validate();
            if (Page.IsValid)
            {
                //check if max send password on email reached
                int MaxSendEmail = 2;
                int n;
                var isNumeric = int.TryParse(txtAnswer.Text, out n);

                if (isNumeric == false)
                {
                    Alert.Show("Error: Please Enter Numeric Correct Total / Answer for the Security Question", "FrmForgotPassword.aspx");
                }
                else if (Convert.ToInt32(ViewState["TotalOfRandom"]) != Convert.ToInt32(txtAnswer.Text))
                {
                    Alert.Show("Error: Wrong Total / Answer", "FrmForgotPassword.aspx");
                }
                else if ((Convert.ToInt16(ViewState["Count_MaxSendEmail"]) >= MaxSendEmail))
                {
                    Alert.Show("Maximum 2 times password can be sent to email id", "FrmSecuredLogin.aspx");
                }
                else
                {
                    bool IsResetPasswordLinkRequested = false;
                    string Token = string.Empty;

                    string cAuthModeAD = ConfigurationManager.AppSettings["cAuthModeAD"].ToString();

                    bool IsExternalUser = false;
                    GetUserDetails(out IsExternalUser);
                    if (IsExternalUser == false && cAuthModeAD == "true") //IF INTERNAL USER THEN WINDOWS CREDENTIALS SHOULD WORK AS AD IS ENABLED
                    {
                        Alert.Show("Error: Kindly use your windows credentials to login, if forgot password your windows credentials then please contact local IT support", "FrmSecuredLogin.aspx");
                    }
                    else
                    {
                        if (IsMatch(out pswd, out IsResetPasswordLinkRequested, out Token))
                        {
                            if (IsResetPasswordLinkRequested == false)
                            {
                                string Link = GetResetPasswordLink(Token);
                                //send password to email id;
                                string msg = SendEmail(txtEmailId.Text.Trim(), txtLoginId.Text.Trim(), pswd, Link);
                                if (msg == "success")
                                {
                                    Alert.Show("Password Sent To Registered Email Address.", "FrmSecuredLogin.aspx");
                                    ViewState["Count_MaxSendEmail"] = Convert.ToInt16(ViewState["Count_MaxSendEmail"]) + 1;
                                }
                                else
                                {
                                    Alert.Show("Error: Could not send Email, please try again later.", "FrmSecuredLogin.aspx");
                                }
                            }
                            else
                            {
                                Alert.Show("Password Already Sent To Registered Email Address, if not received then please contact PASS Admin", "FrmSecuredLogin.aspx");
                            }
                        }
                        else
                        {
                            Alert.Show("Login Id and Email Id Combination did not match, please ask pass admin to update your email id", "FrmSecuredLogin.aspx");
                        }
                    }
                }
            }
        }

        private string GetResetPasswordLink(string Token)
        {
            string LoginId = PrjPASS.StringCipher.Encrypt(txtLoginId.Text.Trim());
            string EmailId = PrjPASS.StringCipher.Encrypt(txtEmailId.Text.Trim());
            string NewGUID = PrjPASS.StringCipher.Encrypt(Token);

            string ResetPageLink = ConfigurationManager.AppSettings["ResetPageLink"].ToString();
            string ResetLink = ResetPageLink + "?key1=" + EmailId + "&key2=" + NewGUID + "&key3=" + LoginId;
            return ResetLink;
        }


        private bool IsMatch(out string pswd, out bool IsResetPasswordLinkRequested, out string Token)
        {
            pswd = string.Empty;
            bool IsMatched = false;
            IsResetPasswordLinkRequested = false;

            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_VERIFY_LOGINID_EMAILID_COMBINATION";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            Token = Guid.NewGuid().ToString("N");

            db.AddParameter(dbCommand, "LOGINID", DbType.String, ParameterDirection.Input, "LOGINID", DataRowVersion.Current, txtLoginId.Text.Trim());
            db.AddParameter(dbCommand, "EMAILID", DbType.String, ParameterDirection.Input, "EMAILID", DataRowVersion.Current, txtEmailId.Text.Trim());
            db.AddParameter(dbCommand, "TOKEN", DbType.String, ParameterDirection.Input, "TOKEN", DataRowVersion.Current, Token);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsMatched = ds.Tables[0].Rows[0][0].ToString() == "1" ? true : false;
                    pswd = ds.Tables[0].Rows[0][1].ToString();
                    IsResetPasswordLinkRequested = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsResetPasswordLinkRequested"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["IsResetPasswordLinkRequested"].ToString());
                }
            }

            return IsMatched;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmSecuredLogin.aspx");
        }

        private string SendEmail(string ToEmailIds, string LoginId, string Password, string Link)
        {
            string ActualToEmailIds = ToEmailIds;
            

            //ToEmailIds = ToEmailIds + ";" + smtp_DefaultCCMailId;
            string strMessage = string.Empty;
            string[] ToEmailAddr = ToEmailIds.Split(';');

            string smtp_Username = ConfigurationManager.AppSettings["smtp_Username"].ToString();
            string smtp_Password = ConfigurationManager.AppSettings["smtp_Password"].ToString();
            string smtp_Host = ConfigurationManager.AppSettings["smtp_Host"].ToString();
            string smtp_FromMailId = "noreply@kotak.com"; //ConfigurationManager.AppSettings["smtp_FromMailId"].ToString();
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


                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBodyForgotPassword.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("@LoginId", LoginId);
                //MailBody = MailBody.Replace("@Password", Password);
                MailBody = MailBody.Replace("@ResetPageLink", Link);
                

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(smtp_FromMailId);
                mm.Subject = "PASS Credentials";
                mm.Body = MailBody;
                mm.IsBodyHtml = true;

                foreach (var toMailId in ToEmailAddr)
                {
                    strPath = string.Empty;
                    MailBody = string.Empty;

                    if (toMailId != null)
                    {
                        if (toMailId.Length > 5)
                        {
                            mm.To.Add(toMailId);
                        }
                    }
                }

                //mm.CC.Add(smtp_DefaultCCMailId);

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                

                client.Send(mm);
                strMessage = "success";
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                string strPathErrorFile = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPathErrorFile, "Error: " + strMessage);
            }

            return strMessage;
        }

        private void GetUserDetails(out bool IsExternalUser)
        {
            IsExternalUser = false;
            try
            {
                Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection conn = new SqlConnection(dbCOMMON.ConnectionString))
                {
                    string query = "SELECT  * FROM TBL_USER_LOGIN UL WHERE UL.vUserLoginId =@login";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    var sqlParamUser = new SqlParameter("login", SqlDbType.VarChar);
                    sqlParamUser.Value = txtLoginId.Text;
                    cmd.Parameters.Add(sqlParamUser);

                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsExternalUser = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsExternalUser"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                PrjPASS.ExceptionUtility.LogException(ex, "GetUserDetails on forgot password page");
            }
        }
    }
}

