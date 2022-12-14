using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmErrorPage : System.Web.UI.Page
    {
        public string logfile = "log_kgipass_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string status = DecryptText(Request.QueryString["pay"]);
                string txn = DecryptText(Request.QueryString["txn"]);

                lblProposalNumber.Text = txn;

                if (status == "1")
                {
                    divSuccess.Visible = true;
                }
                else
                {
                    divFailure.Visible = false;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmErrorPage.aspx ::Error occured in PageLoad and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
                

            }

        }

        public string DecryptText(string cipherText)
        {
            try
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
                        cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmErrorPage.aspx ::Error occured in Decrypttext for text :" + cipherText + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
                return null;

            }
        }
    }
}