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
    public partial class FrmResetPassword : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string LoginId = PrjPASS.StringCipher.Decrypt(Request.QueryString["key3"].ToString());
                string EmailId = PrjPASS.StringCipher.Decrypt(Request.QueryString["key1"].ToString());
                string Token = PrjPASS.StringCipher.Decrypt(Request.QueryString["key2"].ToString());

                if (IsValidToken(LoginId, EmailId, Token) == false)
                {
                    Alert.Show("Invalid Token or Token Expired", "FrmSecuredLogin.aspx");
                    return;
                }
                else
                {
                    ViewState["Count_MaxSendEmail"] = 1;

                    Random random = new Random();
                    int randomNumber1 = random.Next(1, 99);
                    int randomNumber2 = random.Next(1, 99);
                    ViewState["TotalOfRandom"] = randomNumber1 + randomNumber2;
                    lblQuestion.Text = randomNumber1.ToString() + " + " + randomNumber2.ToString();
                }

                
            }


            if (Session["vUserLoginId"] != null)
            {
                Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                return;
            }
        }

        private bool IsValidToken(string LoginId, string EmailId, string Token)
        {
            bool IsValidToken = false;

            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_VERIFY_LOGINID_EMAILID_COMBINATION_AND_TOKEN";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "LOGINID", DbType.String, ParameterDirection.Input, "LOGINID", DataRowVersion.Current, LoginId);
                db.AddParameter(dbCommand, "EMAILID", DbType.String, ParameterDirection.Input, "EMAILID", DataRowVersion.Current, EmailId);
                db.AddParameter(dbCommand, "TOKEN", DbType.String, ParameterDirection.Input, "TOKEN", DataRowVersion.Current, Token);

                DataSet ds = null;
                ds = db.ExecuteDataSet(dbCommand);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsValidToken = ds.Tables[0].Rows[0]["STATUS"].ToString() == "1" ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                PrjPASS.ExceptionUtility.LogException(ex, "IsValidToken");
            }

            return IsValidToken;
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
                    Alert.Show("Error: Please Enter Numeric Correct Total / Answer for the Security Question", "FrmSecuredLogin.aspx");
                }
                else if (Convert.ToInt32(ViewState["TotalOfRandom"]) != Convert.ToInt32(txtAnswer.Text))
                {
                    Alert.Show("Error: Wrong Total / Answer", "FrmSecuredLogin.aspx");
                }
                else if ((Convert.ToInt16(ViewState["Count_MaxSendEmail"]) >= MaxSendEmail))
                {
                    Alert.Show("Maximum 2 times password can be sent to email id", "FrmSecuredLogin.aspx");
                }
                else if (txtNewPassword.Text.Trim() != txtConfirmNewPassword.Text.Trim())
                {
                    Alert.Show("Password not matching with confirm new password", "FrmSecuredLogin.aspx");
                }   
                else
                {
                    //UPDATE PASSWORD TO DATABASE
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString);
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        { con.Open(); }
                        SqlCommand cmd = new SqlCommand("PROC_UPDATE_USER_PASSWORD", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LoginId", txtLoginId.Text.Trim());
                        cmd.Parameters.AddWithValue("@EMailId", txtEmailId.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtConfirmNewPassword.Text.Trim());
                        int i = cmd.ExecuteNonQuery();
                        if (i > 1)
                        {
                            Alert.Show("Password Updated, Please Login With New Password !!!", "FrmSecuredLogin.aspx");
                        }
                    }
                    catch (Exception ex)
                    {
                        PrjPASS.ExceptionUtility.LogException(ex, "btnSave_Click");
                    }
                }
            }
        }

       

        

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmSecuredLogin.aspx");
        }

      
    }
}

