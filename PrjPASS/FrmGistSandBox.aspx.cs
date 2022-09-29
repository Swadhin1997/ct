using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Obout.Grid;
//using Obout.ComboBox;
using ProjectPASS;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.Services;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net.Mime;

using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;

using System.ServiceModel.Activation;


using System.Web.Script.Serialization;

using Microsoft.VisualBasic;

using System.Net;
using System.Security.Cryptography;


using Google;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Urlshortener.v1.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.ModelBinding;
using System.Data.OleDb;
using System.Threading;
using OfficeInterop = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;


using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace PrjPASS
{
    public partial class FrmGistSandBox : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGnrtLink_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtProductName.Text.Trim()))
            {
                Alert.Show("Please Enter Product Name"); return;
            }
            else if (string.IsNullOrEmpty(txtCustomerName.Text.Trim()))
            {
                Alert.Show("Please Enter Customer Name"); return;
            }
            else if (string.IsNullOrEmpty(txtEmailId.Text.Trim()))
            {
                Alert.Show("Please Enter Email Address"); return;
            }
            else if (!Regex.IsMatch(txtEmailId.Text.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                Alert.Show("Please Enter Valid Email Address"); return;
            }
            else if (string.IsNullOrEmpty(txtMobNo.Text.Trim()))
            {
                Alert.Show("Please Enter Customer Mobile Number"); return;
            }
            else if (txtMobNo.Text.Trim().Length != 10)
            {
                Alert.Show("Please Enter 10 Digit Mobile Number"); return;
            }
            else if (!Regex.IsMatch(txtMobNo.Text.Trim(), "^[0-9]*$"))
            {
                Alert.Show("Please Enter Valid Mobile Number"); return;
            }
            else if (string.IsNullOrEmpty(txtPlace.Text.Trim()))
            {
                Alert.Show("Please Enter Place"); return;
            }
            else if (!Regex.IsMatch(txtPolicyNo.Text.Trim(), "^[0-9]*$"))
            {
                Alert.Show("Please Enter policy Number"); return;
            }


            string vFinalURL = string.Empty;
            string vCustomerId = string.Empty;
            string[] vErrorMsg = new string[2];
            vCustomerId = fn_Gen_Cert_No("1000", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"));
            //List<string> CustomerEmail = new List<string>();
            //CustomerEmail.Add("singh.rahul@kotak.com");
            string ConfirmLink = ConfigurationManager.AppSettings["vConfirmLink"].ToString() + "?vCustomerId=" + EncryptText(vCustomerId);
            string googleShortURL = string.Empty;
            string ShortURL = string.Empty; string ErrorMessage = string.Empty;
            // Prj_Lib_Common_Utility.CommonUtility.Fn_ConvertToShortURL(vLoggedInUser, ReviewAndConfirmLink, out ShortURL, out ErrorMessage);


            lnkGeneratedLinkShort.HRef = ConfirmLink;
            lnkGeneratedLinkShort.InnerText = ConfirmLink;
            googleShortURL = string.Empty;
            GoogleURLShortner(ConfirmLink, out googleShortURL);

            if (!string.IsNullOrEmpty(googleShortURL))
            {
                vFinalURL = googleShortURL;
                lnkGeneratedLinkShort.HRef = googleShortURL;
                lnkGeneratedLinkShort.InnerText = googleShortURL;
            }
            //else
            //{
            //    vFinalURL = ConfirmLink;
            //    //Prj_Lib_Common_Utility.CommonUtility.Fn_LogEvent("PaymentPage.aspx.cs: Could not create short url due to following error message: " + ErrorMessage + " - Function Name: Fn_Create_And_Send_Payment_Link");
            //}
            Fn_Save_CustomerDetals_Link(ConfirmLink, googleShortURL, vCustomerId);
        }
        private void Fn_Save_CustomerDetals_Link(string ConfirmLink, string ConfirmShortURL, string vCustomerId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_IRDA_GIST_SANDBOX";

                        cmd.Parameters.AddWithValue("@vCustomerId", vCustomerId);
                        cmd.Parameters.AddWithValue("@vCustomerName", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@vEmailId", txtEmailId.Text);
                        cmd.Parameters.AddWithValue("@vMobileNo", txtMobNo.Text);
                        cmd.Parameters.AddWithValue("@vProductName", txtProductName.Text);
                        cmd.Parameters.AddWithValue("@vPolicyNumber", txtPolicyNo.Text);
                        cmd.Parameters.AddWithValue("@vPlace", txtPlace.Text);
                        cmd.Parameters.AddWithValue("@vConfirmationLink", ConfirmLink);
                        cmd.Parameters.AddWithValue("@vConfirmationShortURL", ConfirmShortURL);
                        cmd.Parameters.AddWithValue("@nNoOfLinkValidationDays", 45);
                        cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString());

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //ExceptionUtility.LogException(ex, "SaveReviewConfirmLink Method");
            }
        }
        private void GoogleURLShortner(string longURL, out string shortURL)
        {
            shortURL = string.Empty;
            try
            {
                string ErrorMsg = string.Empty;
                WebRequest.DefaultWebProxy = null;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
                string UserId = ConfigurationManager.AppSettings["UserIdForShortURL"].ToString();
                string AccessKey = ConfigurationManager.AppSettings["AccessKeyForShortURL"].ToString();
                proxy.ConvertLongURLToShortURL(UserId, AccessKey, longURL, out shortURL, out ErrorMsg);
                proxy.Close();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
            }
        }
        public static string EncryptText(string clearText)
        {
            string EncryptionKey = "KGIMAV2BNI1907";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string DecryptText(string cipherText)
        {
            string EncryptionKey = "KGIMAV2BNI1907";
            byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public string fn_Gen_Cert_No(string vDocumentType, string vDocumentContext)
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            SqlConnection Conn = new SqlConnection(dbCOMMON.ConnectionString);
            //if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            SqlTransaction Trans = Conn.BeginTransaction();
            string functionReturnValue = null;
            try
            {
                string vCurrentYear = null, vCurrentMonth = null, vTransactionNumberCur = null, vCurrentDay = null;

                int vDifferenceInYear = Convert.ToInt32(vDocumentContext.Substring(6, 4)) - 1991;

                if (vDifferenceInYear > 36)
                {
                    vDifferenceInYear = vDifferenceInYear % 36;
                }
                if (vDifferenceInYear < 10)
                {
                    vCurrentYear = Convert.ToString(vDifferenceInYear).Trim();
                }
                else
                {
                    vCurrentYear = Strings.LTrim(Convert.ToString(Strings.Chr(vDifferenceInYear + 55)));
                }

                vCurrentMonth = Strings.LTrim(Convert.ToString(Strings.Chr(Convert.ToInt32(Strings.Mid(vDocumentContext, 4, 2)) + 64)));


                string _UpdateCommand = "UPDATE TBL_TRANSATION_ID_SEQUENCE_GISTSANDBOX SET vLastNo=vLastNo WHERE vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' AND cCharacterDay='" + vCurrentDay + "'";
                SqlCommand _Command = new SqlCommand(_UpdateCommand, Conn);
                _Command.Transaction = Trans;
                if (_Command.ExecuteNonQuery() <= 0)
                {
                    string lcINSSTR = "INSERT INTO TBL_TRANSATION_ID_SEQUENCE_GISTSANDBOX(vTransType,cCharacterYear,cCharacterMonth,cCharacterDay,vLastNo)";
                    lcINSSTR = lcINSSTR + " VALUES ('" + vDocumentType + "','" + vCurrentYear + "','" + vCurrentMonth + "','" + vCurrentDay + "','0')";
                    _Command = new SqlCommand(lcINSSTR, Conn);
                    _Command.Transaction = Trans;
                    _Command.ExecuteNonQuery();
                }

                string _SelectCmd = "Select vLastNo from TBL_TRANSATION_ID_SEQUENCE_GISTSANDBOX where vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' AND cCharacterDay='" + vCurrentDay + "'";
                SqlDataAdapter Adapter = new SqlDataAdapter();
                _Command = new SqlCommand(_SelectCmd, Conn);
                _Command.Transaction = Trans;
                Adapter.SelectCommand = _Command;
                DataSet dsDocNo = new DataSet();

                Adapter.Fill(dsDocNo);

                if (dsDocNo.Tables[0].Rows.Count > 0)
                {
                    vTransactionNumberCur = Convert.ToString(Convert.ToDouble(dsDocNo.Tables[0].Rows[0]["vLastNo"]) + 1);
                }
                else
                {
                    vTransactionNumberCur = "1";
                }

                vTransactionNumberCur = vTransactionNumberCur.ToString().PadLeft(6, '0');

                functionReturnValue = vDocumentType + vTransactionNumberCur;

                _UpdateCommand = "UPDATE TBL_TRANSATION_ID_SEQUENCE_GISTSANDBOX SET vLastNo='" + Convert.ToDouble(vTransactionNumberCur) + "' WHERE vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' AND cCharacterDay='" + vCurrentDay + "' ";
                try
                {
                    _Command = new SqlCommand(_UpdateCommand, Conn);
                    _Command.Transaction = Trans;
                    _Command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    functionReturnValue = "Error :" + e.Message;
                }
                Trans.Commit();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
                // CommonUtility.Fn_LogException(ex);
            }
            return functionReturnValue;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void btnSendLink_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(lnkGeneratedLinkShort.HRef))
                {
                    Alert.Show("Please generate the link first"); return;
                }
                sendLinkBySms(txtMobNo.Text, lnkGeneratedLinkShort.HRef);
                //if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["careEmail_epos"].Trim()))
                //{
                string emailId = txtEmailId.Text.Trim();//ConfigurationManager.AppSettings["careEmail_epos"].Trim();
                string[] arrMail = emailId.Split(',');

                string strPath = string.Empty;
                string MailBody = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);

                //Database db = DatabaseFactory.CreateDatabase("cnPASS");
                //using (SqlConnection con = new SqlConnection(db.ConnectionString))
                //{
                //    con.Open();
                //    SqlCommand command = new SqlCommand("INSERT_PROPOSAL_MODIFY_DETAILS", con);
                //    command.CommandType = CommandType.StoredProcedure;
                //    command.Parameters.AddWithValue("@proposal", propNumber);
                //    command.Parameters.AddWithValue("@mobile", lblMobile.Text);
                //    command.Parameters.AddWithValue("@email", lblEmail.Text);
                //    command.Parameters.AddWithValue("@quote", lblQuoteNumber.Text);
                //    string details = txtDetails.Text.Replace('\r', ' ');
                //    details = details.Replace('\n', ' ');
                //    SqlParameter prmDetail = new SqlParameter();

                //    prmDetail.ParameterName = "@text";
                //    prmDetail.SqlDbType = SqlDbType.VarChar;
                //    prmDetail.Direction = ParameterDirection.Input;
                //    command.Parameters.Add(prmDetail);
                //    prmDetail.Value = details;
                //    command.ExecuteNonQuery();
                //}

                strPath = ConfigurationManager.AppSettings["ConfirmationLink"];
                MailBody = File.ReadAllText(strPath);


                // MailBody = MailBody.Replace("@emailID", txtEmailId.Text);
                MailBody = MailBody.Replace("ConfirmationLink", lnkGeneratedLinkShort.InnerText);
                MailBody = MailBody.Replace("customername", txtCustomerName.Text);




                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                mm.Subject = "Customer Confirmation for Participation in Sandbox Initiative";
                mm.Body = MailBody;
                //mm.Body = "mail";
                mm.IsBodyHtml = true;

                for (int i = 0; i < arrMail.Count(); i++)
                {

                    if (Regex.IsMatch(arrMail[i].Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                    {
                        mm.To.Add(arrMail[i]);

                    }
                }

                //if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["replyToEmail"]))
                //{
                //    MailAddress adminAddress = new MailAddress(ConfigurationManager.AppSettings["replyToEmail"]);
                //    mm.ReplyTo = adminAddress;
                //}

                mm.BodyEncoding = UTF8Encoding.UTF8;
                smtpClient.Send(mm);

                //}

                lblMsg.Text = "Link sent to the customer successfully.";
                btnSendLink.Enabled = false;
                // Response.Redirect("FrmThankYouPage.aspx", false);
            }
            catch (Exception ex)
            {
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in sendmail details for proposal :" + proposal + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                //Response.Redirect("FrmCustomErrorPage.aspx");
            }

        }
        private void sendLinkBySms(string mobileno, string link)
        {
            try
            {
                //mobileno = "7045041046";

                string strPath = string.Empty;
                string smsBody = string.Empty;

                Random r = new Random();
                // int OTPNumber = r.Next(100000, 999999);


                smsBody = ConfigurationManager.AppSettings["ConfirmationsmsBody"];
                //smsBody = File.ReadAllText(strPath);
                //link = "<a href='"+link+ "'> " + link + " </a>";
                //  link = link.Replace("http://", "");
                smsBody = smsBody.Replace("link", link);

                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "0")
                {
                    // string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
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

                    //  string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);


                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                    var client = new System.Net.WebClient();
                    client.Proxy = proxy;

                    client.Proxy = WebRequest.DefaultWebProxy;
                    client.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    client.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);

                    var content = client.DownloadString(URI);
                    client.Proxy = null;
                }

                WebRequest.DefaultWebProxy = null;
                // HttpContext.Current.Session["OTPId"] = Convert.ToString(ds.Tables[0].Rows[0]["Id"]);
                // HttpContext.Current.Session["OTPNumber"] = Convert.ToString(ds.Tables[0].Rows[0]["OTP"]);
                // return OTPNumber.ToString();
            }

            catch (Exception ex)
            {
                //  File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in GenerateOTP for proposal number:" + lblProposalText.Text + " and error message is : " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
                // return null;
            }

        }
    }
}