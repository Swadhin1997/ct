using System;
using System.Collections;
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
using System.DirectoryServices;
using System.Net;
using PrjPASS;
using System.DirectoryServices.AccountManagement;

namespace ProjectPASS
{
    public partial class FrmSecuredLogin : System.Web.UI.Page
    {
        //void Page_Init(object sender, EventArgs e)
        //{
        //    this.ViewStateUserKey = this.Session.SessionID;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "0"); // Proxies.
            Response.Cache.SetCacheability(HttpCacheability.NoCache); // Stop Caching in IE
            Response.Cache.SetNoStore(); // Stop Caching in Firefox

            this.Form.DefaultButton = btnGetProperty.UniqueID;

            if (!IsPostBack)
            {
                /*
                #region Session Hijack Fixation
                if (System.Web.HttpContext.Current.Session != null)
                {
                    if (Session["vUserLoginId"] != null) // e.g. this is after an initial logon
                    {
                        string sKey = (string)Session["vUserLoginId"];
                        string sUser = (string)HttpContext.Current.Cache.Remove(sKey);

                        // Added to solve Session-Fixation-vulnerability-in-ASP-NET  
                        if (Request.Cookies["ASP.NET_SessionId"] != null)
                        {
                            Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                        }

                        if (Request.Cookies["AuthToken"] != null)
                        {
                            Response.Cookies["AuthToken"].Value = string.Empty;
                            Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                        }
                        // Added to solve Session-Fixation-vulnerability-in-ASP-NET  
                    }
                }
                Session.Abandon();
                #endregion

                

                */

                ExpireAllCookies();

                txtUserID.Focus();
                Session["CTR"] = "0";
                hdnMSG.Value = "";
            }
        }

        public bool AuthenticateUser(string domain, string username, string password, string LdapPath, out string Errmsg)
        {
            Errmsg = "";
            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(LdapPath, domainAndUsername, password);
            try
            {
                // Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                // Update the new path to the user in the directory 
                LdapPath = result.Path;
                string _filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                Errmsg = ex.Message;
                return false;
                throw new Exception("Error authenticating user." + ex.Message);
            }
            return true;
        }
        protected void btnGetProperty_Click(object sender, EventArgs e)
        {
            #region Validating the Google reCaptcha Token using Captcha Service
            clsInvisibleCaptchaRequest objTokenVerifyRequest = new clsInvisibleCaptchaRequest();
            objTokenVerifyRequest.vTokenFromGoogleCaptchaService = hdnTokenFromCaptchaService.Value;
            objTokenVerifyRequest.vLoginEmailId = txtUserID.Text;
            objTokenVerifyRequest.vSourceIPAddress = hdnLoginFromIPAddress.Value;

            clsInvisibleCaptchaResponse response = clsInvisibleCaptcha.Fn_ReCaptchaSiteVerify(objTokenVerifyRequest);
            hdnMSG.Value = response.success ? "success" : "failed";
            #endregion

            if (hdnMSG.Value != "success")
            {
                lblMessage1.Text = ConfigurationManager.AppSettings["InvisibleCaptchaErrorMessage"];
                return;
            }
            string IPAddress = hdnLoginFromIPAddress.Value;
            string cAuthModeAD = ConfigurationManager.AppSettings["cAuthModeAD"].ToString();

            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            if (cAuthModeAD == "true")
            {
                string dominName = string.Empty;
                string adPath = string.Empty;
                string userName = txtUserID.Text.Trim().ToUpper();
                string strError = string.Empty;
                try
                {
                    using (SqlConnection conn = new SqlConnection(dbCOMMON.ConnectionString))
                    {
                        string query = "SELECT  * FROM TBL_USER_LOGIN UL WHERE UL.vUserLoginId =@login";
                        SqlCommand cmd = new SqlCommand(query, conn);

                        var sqlParamUser = new SqlParameter("login", SqlDbType.VarChar);
                        sqlParamUser.Value = txtUserID.Text;
                        cmd.Parameters.Add(sqlParamUser);

                        DataSet ds = new DataSet();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            bool IsExternalUser = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsExternalUser"].ToString());
                            string bIsActivate = Convert.ToString(ds.Tables[0].Rows[0]["bIsActivate"].ToString());
                            string vLocked = Convert.ToString(ds.Tables[0].Rows[0]["vLocked"].ToString());
                            if (IsExternalUser == false)
                            {
                                if (bIsActivate.ToUpper() == "N")
                                {
                                    SqlCommand sqlcmd1 = new SqlCommand();  ///Added By Shafee 29-Jan-2021 Start
                                    sqlcmd1.CommandType = CommandType.StoredProcedure;
                                    sqlcmd1.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                                    sqlcmd1.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                                    sqlcmd1.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                                    sqlcmd1.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                                    sqlcmd1.Connection = conn;
                                    conn.Open();
                                    int i = sqlcmd1.ExecuteNonQuery();
                                    if (i > 0)
                                    {
                                        string Message = string.Empty;
                                        Message = "Login Failed";
                                    }
                                    conn.Close();//End here
                                    string PASSAccounntDeactivationMsg = ConfigurationManager.AppSettings["PASSAccounntDeactivationMsg"].ToString(); //SR95970 - Hasmukh
                                    lblstatus.Text = PASSAccounntDeactivationMsg; // "PASS Account Deactivated, Contact PASS Administrator."; //SR95970 - Hasmukh
                                    lblstatus.ForeColor = System.Drawing.Color.Red;
                                    return;
                                }

                                if (vLocked.ToUpper() == "Y")
                                {
                                    SqlCommand sqlcmd2 = new SqlCommand();  ///Added By Shafee 29-Jan-2021 Start
                                    sqlcmd2.CommandType = CommandType.StoredProcedure;
                                    sqlcmd2.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                                    sqlcmd2.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                                    sqlcmd2.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                                    sqlcmd2.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                                    sqlcmd2.Connection = conn;
                                    conn.Open();
                                    int i = sqlcmd2.ExecuteNonQuery();
                                    if (i > 0)
                                    {
                                        string Message = string.Empty;
                                        Message = "Login Failed";
                                    }
                                    conn.Close();//End here

                                    lblstatus.Text = "PASS Account Locked, Contact PASS Administrator.";
                                    lblstatus.ForeColor = System.Drawing.Color.Red;
                                    return;
                                }

                                //if (true == AuthenticateUser(dominName, userName, DecryptedPassword(txtPassword.Text), adPath, out strError))
                                bool IsValidADLogin = false;
                                if (ConfigurationManager.AppSettings["IsCallADWebService"].ToString() == "0")
                                {
                                    IsValidADLogin = Fn_Authenticate_Active_Directory_User_Internal(userName, DecryptedPassword(txtPassword.Text));
                                }
                                else
                                {
                                    IsValidADLogin = ValidateADLogin(userName, DecryptedPassword(txtPassword.Text));
                                }

                                if (IsValidADLogin)
                                {
                                    string queryRole = "SELECT * FROM TBL_USER_ID_TO_ROLE_MAPPING WHERE vUserLoginId =@login";
                                    SqlCommand cmdRole = new SqlCommand(queryRole, conn);
                                    var sqlParamUserRole = new SqlParameter("login", SqlDbType.VarChar);
                                    sqlParamUserRole.Value = txtUserID.Text;
                                    cmdRole.Parameters.Add(sqlParamUserRole);
                                    DataSet dsRole = new DataSet();
                                    SqlDataAdapter adapterRole = new SqlDataAdapter(cmdRole);
                                    adapterRole.Fill(dsRole);

                                    if (dsRole.Tables[0].Rows.Count > 0)
                                    {
                                        Session["vUserLoginId"] = txtUserID.Text;
                                        Session["vUserLoginDesc"] = dsRole.Tables[0].Rows[0]["vUserLoginDesc"].ToString();
                                        Session["vThemeName"] = ds.Tables[0].Rows[0]["vThemeName"].ToString();
                                        Session["vRoleCode"] = dsRole.Tables[0].Rows[0]["vRoleCode"].ToString();
                                        Session["RegionalDeptHeadEmailId"] = ds.Tables[0].Rows[0]["RegionalDeptHeadEmailId"].ToString();
                                        Session["IsUWApproval"] = ds.Tables[0].Rows[0]["isUWapproval"].ToString();
                                        Session["IsAllowBundledPolicyBooking"] = ds.Tables[0].Rows[0]["IsAllowBundledPolicyBooking"].ToString();
                                        Session["IsExternalUser"] = IsExternalUser;
                                        Session["IsBancaUser"] = string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["BANCA_NONBANCA"])) ? false : (Convert.ToString(ds.Tables[0].Rows[0]["BANCA_NONBANCA"]) == "BANCA" ? true : false); //SR90624 - HASMUKH - 14_MAY_2021
                                        // Added to solve Session-Fixation-vulnerability-in-ASP-NET  
                                        // createa a new GUID and save into the session
                                        string guid = Guid.NewGuid().ToString();
                                        Session["AuthToken"] = guid;
                                        // now create a new cookie with this guid value
                                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                        LoggedIn(txtUserID.Text, DecryptedPassword(txtPassword.Text));
                                    }
                                    else
                                    {
                                        SqlCommand sqlcmd3 = new SqlCommand();  ///Added By Shafee 29-Jan-2021 Start
                                        sqlcmd3.CommandType = CommandType.StoredProcedure;
                                        sqlcmd3.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                                        sqlcmd3.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                                        sqlcmd3.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                                        sqlcmd3.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                                        sqlcmd3.Connection = conn;
                                        conn.Open();
                                        int i = sqlcmd3.ExecuteNonQuery();
                                        if (i > 0)
                                        {
                                            string Message = string.Empty;
                                            Message = "Login Failed";
                                        }
                                        conn.Close();//End here
                                        lblstatus.Text = "No Role Mapped to the User";
                                        lblstatus.ForeColor = System.Drawing.Color.Red;
                                        txtUserID.Focus();
                                        return;
                                    }
                                }
                                else
                                {
                                    SqlCommand sqlcmd4 = new SqlCommand();  ///Added By Shafee 29-Jan-2021 Start
                                    sqlcmd4.CommandType = CommandType.StoredProcedure;
                                    sqlcmd4.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                                    sqlcmd4.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                                    sqlcmd4.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                                    sqlcmd4.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                                    sqlcmd4.Connection = conn;
                                    conn.Open();
                                    int i = sqlcmd4.ExecuteNonQuery();
                                    if (i > 0)
                                    {
                                        string Message = string.Empty;
                                        Message = "Login Failed";
                                    }
                                    conn.Close();//End here
                                    lblstatus.Text = "Invalid Credentials!";
                                }
                            }
                            else
                            {
                                Fn_User_Login();
                            }
                        }
                        else
                        {
                            lblstatus.Text = "Login Failed, Invalid Username or Password Entered.";
                        }
                    }

                    if (!string.IsNullOrEmpty(strError))
                    {
                        lblstatus.Text = "Login Failed, Invalid Username or Password Entered.";
                    }
                }
                catch (Exception ex)
                {
                    lblstatus.Text = ex.Message;
                }
                finally
                {

                }
            }
            else
            {
                Fn_User_Login();
            }
        }

        protected void LoggedIn(string vUserLoginId, string vPassword)
        {

            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");   // Added by shafee Jan 29-1-21
                                                                            // if these credentials are already in the Cache 
            string sKey = txtUserID.Text + DecryptedPassword(txtPassword.Text);
            string sUser = Convert.ToString(Cache[sKey]);
            if (sUser == null || sUser == String.Empty)
            {
                // No Cache item, so sesion is either expired or user is new sign-on
                // Set the cache item and Session hit-test for this user---
                TimeSpan SessTimeOut = new TimeSpan(0, 0, 0, 30, 0);
                HttpContext.Current.Cache.Insert(sKey, sKey, null, DateTime.MaxValue, SessTimeOut,
                System.Web.Caching.CacheItemPriority.NotRemovable, null);
                //Session["user"] = txtUserID.Text + DecryptedPassword(txtPassword.Text);

                // Added to solve Session-Fixation-vulnerability-in-ASP-NET  
                // createa a new GUID and save into the session
                string guid = Guid.NewGuid().ToString();
                Session["AuthToken"] = guid;
                // now create a new cookie with this guid value
                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                // Added to solve Session-Fixation-vulnerability-in-ASP-NET   

                FormsAuthentication.SetAuthCookie(vUserLoginId, false);
                var authTicket = new FormsAuthenticationTicket(1, vUserLoginId, DateTime.Now, DateTime.Now.AddMinutes(20), false, "");
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);

                using (SqlConnection conn = new SqlConnection(dbCOMMON.ConnectionString))  //Added by Shafee 29-jan-21--start
                {
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                    sqlcmd.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                    sqlcmd.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                    sqlcmd.Parameters.AddWithValue("@vLastLoginStatus", "Success");

                    sqlcmd.Connection = conn;
                    conn.Open();
                    int i = sqlcmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        string Message = string.Empty;
                        Message = "Login success";
                    }
                    conn.Close();

                }//End here

                // Let them in - redirect to main page, etc.
                Response.Redirect("FrmMainMenu.aspx");
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(dbCOMMON.ConnectionString))  //Added by Shafee 29-jan-21--start
                {
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                    sqlcmd.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                    sqlcmd.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                    sqlcmd.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                    sqlcmd.Connection = conn;
                    conn.Open();
                    int i = sqlcmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        string Message = string.Empty;
                        Message = "Login Failed";
                    }
                    conn.Close();

                }//End here
                // cache item exists, so too bad... 
                Alert.Show("DUPLICATE LOGIN ATTEMPT!!![IF YOU ARE SAME USER AND HAVE NOT CLICKED ON LOGOUT BUTTON BEFORE, PLEASE TRY AFTER 1 MINS]");
                return;
            }

            //Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            //string sqlCommand = "UPDATE TBL_USER_LOGIN SET cIsLoggedIn ='Y' where vUserLoginId ='" + vUserLoginId + "'";
            //DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
            //dbCOMMON.ExecuteNonQuery(dbCommand);
        }
        protected void Fn_User_Login()
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            //string sqlCommand = "SELECT  dbo.fnEncDecRc4('Kotak',vUserPassword) vUserPassword,bIsActivate,vLocked,vUserLoginId,vThemeName FROM TBL_USER_LOGIN UL" +
            //" WHERE UL.vUserLoginId ='" + txtUserID.Text + "'";
            //DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
            //DataSet ds = null;
            //ds = dbCOMMON.ExecuteDataSet(dbCommand);

            using (SqlConnection conn = new SqlConnection(dbCOMMON.ConnectionString))
            {
                //CR 132
                //string query = "SELECT  dbo.fnEncDecRc4('Kotak',vUserPassword) vUserPassword,bIsActivate,vLocked,vUserLoginId,vThemeName,IsAllowLoginFromChotuPASS,RegionalDeptHeadEmailId FROM TBL_USER_LOGIN UL" +
                //" WHERE UL.vUserLoginId =@login";
                //CR 132
                string query = "SELECT  dbo.fnEncDecRc4('Kotak',vUserPassword) vUserPassword,bIsActivate,vLocked,vUserLoginId,vThemeName,IsAllowLoginFromChotuPASS,RegionalDeptHeadEmailId,isnull(isUWapproval,0) as isUWapproval, isnull(IsAllowBundledPolicyBooking, 0) as IsAllowBundledPolicyBooking, IsExternalUser, BANCA_NONBANCA,vUserEmailId FROM TBL_USER_LOGIN UL" +
               " WHERE UL.vUserLoginId =@login";

                SqlCommand cmd = new SqlCommand(query, conn);

                var sqlParamUser = new SqlParameter("login", SqlDbType.VarChar);
                sqlParamUser.Value = txtUserID.Text;
                cmd.Parameters.Add(sqlParamUser);

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["bIsActivate"].ToString() == "N")
                    {
                        //Added by Shafee 29-jan-21--start
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                        sqlcmd.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                        sqlcmd.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                        sqlcmd.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                        sqlcmd.Connection = conn;
                        conn.Open();
                        int i = sqlcmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            string Message = string.Empty;
                            Message = "Login Failed";
                        }
                        conn.Close();
                        //End here

                        lblstatus.Text = "Account Deactivated, Contact System Administrator.";
                        lblstatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowLoginFromChotuPASS"]) == false)
                    {
                        //Added by Shafee 29-jan-21--start
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                        sqlcmd.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                        sqlcmd.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                        sqlcmd.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                        sqlcmd.Connection = conn;
                        conn.Open();
                        int i = sqlcmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            string Message = string.Empty;
                            Message = "Login Failed";
                        }
                        conn.Close();
                        //End here

                        lblstatus.Text = "<BR>You Are Not Allowed To Login From This Application.";
                        lblstatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    if (ds.Tables[0].Rows[0]["vLocked"].ToString() == "Y")
                    {
                        //Added by Shafee 29-jan-21--start
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                        sqlcmd.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                        sqlcmd.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                        sqlcmd.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                        sqlcmd.Connection = conn;
                        conn.Open();
                        int i = sqlcmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            string Message = string.Empty;
                            Message = "Login Failed";
                        }
                        conn.Close();
                        //End here

                        lblstatus.Text = "Account Locked, Contact System Administrator.";
                        lblstatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    if (ds.Tables[0].Rows[0]["vUserPassword"].ToString() != DecryptedPassword(txtPassword.Text))
                    {
                        Session["CTR"] = Convert.ToString(Convert.ToDouble(Session["CTR"]) + 1);

                        lblstatus.Text = "Login Failed, Invalid Username or Password Entered.";
                        lblstatus.ForeColor = System.Drawing.Color.Red;
                        txtUserID.Focus();

                        //Added by Shafee 29-jan-21--start
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                        sqlcmd.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                        sqlcmd.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                        sqlcmd.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                        sqlcmd.Connection = conn;
                        conn.Open();
                        int i = sqlcmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            string Message = string.Empty;
                            Message = "Login Failed";
                        }
                        conn.Close();
                        //End here

                        if (Convert.ToDouble(Session["CTR"]) == 3)
                        {
                            lblstatus.Text = "Attempt Failed 2 times,ID Gets Locked on 3rd Attempt";
                            lblstatus.ForeColor = System.Drawing.Color.MediumVioletRed;

                        }
                        if (Convert.ToDouble(Session["CTR"]) == 3)
                        {
                            lblstatus.Text = "Attempt Failed 3 times,Account Locked";
                            lblstatus.ForeColor = System.Drawing.Color.MediumVioletRed;
                            Database db = DatabaseFactory.CreateDatabase("cnPASS");
                            string StrSqlUpdCommand;
                            string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();

                            SqlConnection _con = new SqlConnection(db.ConnectionString);
                            _con.Open();
                            SqlTransaction _tran = _con.BeginTransaction();
                            try
                            {

                                //                                StrSqlUpdCommand = "UPDATE TBL_USER_LOGIN SET vLocked='Y',dLockedOn=GETDATE() WHERE vUserLoginId='" + txtUserID.Text + "'";

                                StrSqlUpdCommand = "UPDATE TBL_USER_LOGIN SET vLocked='Y',dLockedOn=GETDATE(), IsResetPasswordLinkRequested = 0 WHERE vUserLoginId=@login";

                                SqlCommand _insertCmd = new SqlCommand(StrSqlUpdCommand, _con);

                                var sqlParamLogin = new SqlParameter("login", SqlDbType.VarChar);
                                sqlParamLogin.Value = txtUserID.Text;
                                _insertCmd.Parameters.Add(sqlParamLogin);


                                _insertCmd.Transaction = _tran;
                                _insertCmd.ExecuteNonQuery();
                                _tran.Commit();
                                _con.Close();
                            }
                            catch (Exception ex)
                            {
                                // log exception
                                _tran.Rollback();
                                _con.Close();
                                Alert.Show(ex.Message.ToString());
                            }
                        }
                    }
                    else
                    {
                        if (ds.Tables[0].Rows[0]["vUserLoginId"].ToString().ToUpper() == "EMP00001")
                        {
                            Fn_Admin_Login();
                        }
                        else
                        {
                            string sqlCommand = "SELECT * FROM TBL_USER_ID_TO_ROLE_MAPPING WHERE vUserLoginId =@login ";
                            //dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                            //DataSet dsRole = null;
                            //dsRole = dbCOMMON.ExecuteDataSet(dbCommand);

                            SqlCommand cmdRole = new SqlCommand(sqlCommand, conn);

                            var sqlParamRole = new SqlParameter("login", SqlDbType.VarChar);
                            sqlParamRole.Value = txtUserID.Text;

                            cmdRole.Parameters.Add(sqlParamRole);

                            DataSet dsRole = new DataSet();
                            SqlDataAdapter adapterRole = new SqlDataAdapter(cmdRole);
                            adapterRole.Fill(dsRole);

                            if (dsRole.Tables[0].Rows.Count > 0)
                            {
                                Session["vUserLoginId"] = txtUserID.Text;
                                Session["vUserLoginDesc"] = dsRole.Tables[0].Rows[0]["vUserLoginDesc"].ToString();
                                Session["vThemeName"] = ds.Tables[0].Rows[0]["vThemeName"].ToString();
                                Session["vRoleCode"] = dsRole.Tables[0].Rows[0]["vRoleCode"].ToString();
                                Session["RegionalDeptHeadEmailId"] = ds.Tables[0].Rows[0]["RegionalDeptHeadEmailId"].ToString();
                                Session["IsUWApproval"] = ds.Tables[0].Rows[0]["isUWapproval"].ToString();
                                Session["IsAllowBundledPolicyBooking"] = ds.Tables[0].Rows[0]["IsAllowBundledPolicyBooking"].ToString();
                                Session["IsExternalUser"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsExternalUser"].ToString());
                                Session["IsBancaUser"] = string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["BANCA_NONBANCA"])) ? false : (Convert.ToString(ds.Tables[0].Rows[0]["BANCA_NONBANCA"]) == "BANCA" ? true : false); //SR90624 - HASMUKH - 14_MAY_2021
                                Session["vUserEmailId"] = ds.Tables[0].Rows[0]["vUserEmailId"].ToString(); //CR P2_081 - ATISH - 13_July_2022

                                // Added to solve Session-Fixation-vulnerability-in-ASP-NET  
                                // createa a new GUID and save into the session
                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;
                                // now create a new cookie with this guid value
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                LoggedIn(txtUserID.Text, DecryptedPassword(txtPassword.Text));
                                

                            }
                            else
                            {
                                //Added by Shafee 29-jan-21--start
                                SqlCommand sqlcmd = new SqlCommand();
                                sqlcmd.CommandType = CommandType.StoredProcedure;
                                sqlcmd.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                                sqlcmd.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                                sqlcmd.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                                sqlcmd.Parameters.AddWithValue("@vLastLoginStatus", "Failed");

                                sqlcmd.Connection = conn;
                                conn.Open();
                                int i = sqlcmd.ExecuteNonQuery();
                                if (i > 0)
                                {
                                    string Message = string.Empty;
                                    Message = "Login Failed";
                                }
                                conn.Close();
                                //End here

                                lblstatus.Text = "No Role Mapped to the User";
                                lblstatus.ForeColor = System.Drawing.Color.Red;
                                txtUserID.Focus();
                                return;
                            }
                        }
                    }
                }


                else
                {
                    lblstatus.Text = "Login Failed, Invalid Username or Password Entered.";
                    lblstatus.ForeColor = System.Drawing.Color.Red;
                    txtUserID.Focus();
                    return;
                }
            }
        }
        protected void Fn_Admin_Login()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsGen = new Cls_General_Functions();
            //string sqlCommand = "SELECT * FROM TBL_USER_LOGIN WHERE vUserLoginId ='" + txtUserID.Text + "' and vUserPassword =dbo.fnEncDecRc4('Kotak','" + DecryptedPassword(txtPassword.Text) + "')";

            using (SqlConnection conn = new SqlConnection(db.ConnectionString))
            {
                string query = "SELECT * FROM TBL_USER_LOGIN WHERE vUserLoginId =@login and vUserPassword =dbo.fnEncDecRc4('Kotak',@password)";

                SqlCommand cmd = new SqlCommand(query, conn);

                var sqlParamUser = new SqlParameter("login", SqlDbType.VarChar);
                sqlParamUser.Value = txtUserID.Text;

                var sqlParamPwd = new SqlParameter("password", SqlDbType.VarChar);
                sqlParamPwd.Value = DecryptedPassword(txtPassword.Text);

                cmd.Parameters.Add(sqlParamUser);
                cmd.Parameters.Add(sqlParamPwd);

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);



                //    DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
                //   DataSet ds = null;
                //  ds = db.ExecuteDataSet(dbCommand);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["vUserLoginId"].ToString().ToUpper() == "EMP00001")
                    {
                        Session["vUserLoginId"] = txtUserID.Text;
                        Session["vUserLoginDesc"] = ds.Tables[0].Rows[0]["vUserLoginDesc"].ToString().ToUpper();
                        Session["RegionalDeptHeadEmailId"] = ds.Tables[0].Rows[0]["RegionalDeptHeadEmailId"].ToString();
                        Session["IsAllowBundledPolicyBooking"] = ds.Tables[0].Rows[0]["IsAllowBundledPolicyBooking"].ToString();
                        Session["IsExternalUser"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsExternalUser"].ToString());
                        Session["IsBancaUser"] = string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["BANCA_NONBANCA"])) ? false : (Convert.ToString(ds.Tables[0].Rows[0]["BANCA_NONBANCA"]) == "BANCA" ? true : false); //SR90624 - HASMUKH - 14_MAY_2021
                        Session["vUserEmailId"] = ds.Tables[0].Rows[0]["vUserEmailId"].ToString(); //CR P2_081 - ATISH - 13_July_2022


                        //Added by Shafee 29-jan-21--start
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "PROC_UPDATE_IPADDRESS_AND_LOGINSTATUS";
                        sqlcmd.Parameters.AddWithValue("@vUserLoginId", txtUserID.Text);
                        sqlcmd.Parameters.AddWithValue("@vLoginFromIPAddress", hdnLoginFromIPAddress.Value);
                        sqlcmd.Parameters.AddWithValue("@vLastLoginStatus", "Success");

                        sqlcmd.Connection = conn;
                        conn.Open();
                        int i = sqlcmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            string Message = string.Empty;
                            Message = "Login Success";
                        }
                        conn.Close();
                        //End here
                        // Added to solve Session-Fixation-vulnerability-in-ASP-NET  
                        // createa a new GUID and save into the session
                        string guid = Guid.NewGuid().ToString();
                        Session["AuthToken"] = guid;
                        // now create a new cookie with this guid value
                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                        FormsAuthentication.SetAuthCookie("EMP00001", false);
                        var authTicket = new FormsAuthenticationTicket(1, "EMP00001", DateTime.Now, DateTime.Now.AddMinutes(20), false, "");
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        Response.Cookies.Add(authCookie);

                        Response.Redirect("FrmMainMenu.aspx", true);
                    }
                }
            }
        }

        private void ExpireAllCookies()
        {
            FormsAuthentication.SignOut();
            if (HttpContext.Current != null)
            {
                int cookieCount = HttpContext.Current.Request.Cookies.Count;
                for (var i = 0; i < cookieCount; i++)
                {
                    var cookie = HttpContext.Current.Request.Cookies[i];
                    if (cookie != null)
                    {
                        var expiredCookie = new HttpCookie(cookie.Name)
                        {
                            Expires = DateTime.Now.AddDays(-1),
                            Domain = cookie.Domain
                        };
                        HttpContext.Current.Response.Cookies.Add(expiredCookie); // overwrite it
                    }
                }

                // clear cookies server side
                HttpContext.Current.Request.Cookies.Clear();
            }
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private string DecryptedPassword(string EncryptedPassword)
        {
            return clsAESEncrytDecry.DecryptStringAES(Base64Decode(EncryptedPassword));
        }

        private bool ValidateADLogin(string LoginId, string Password)
        {
            try
            {
                ////to avoide error: "The request was aborted: Could not create SSL/TLS secure channel"

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                ////

                PrjPASS.ADBank_ServiceReference.Service1Client proxy = new PrjPASS.ADBank_ServiceReference.Service1Client();
                bool IsValidUser = proxy.ValidateUser(LoginId, Password);
                proxy.Close();

                return IsValidUser;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "ValidateADLogin");
                return false;
            }
        }

        public bool Fn_Authenticate_Active_Directory_User_Internal(string vLoginEmailId, string vPassword)
        {
            try
            {
                string dominName = string.Empty;
                string adPath = string.Empty;
                string ADuserName = ConfigurationManager.AppSettings["ADuser"].ToString();
                string ADpassword = ConfigurationManager.AppSettings["ADpwd"].ToString();
                string directoryPath = ConfigurationManager.AppSettings["DirectoryPath"].ToString();
                string directoryDomain = ConfigurationManager.AppSettings["DirectoryDomain"].ToString();
                using (var context = new PrincipalContext(ContextType.Domain, directoryDomain, ADuserName, ADpassword)) //SR93096 - Hasmukh - Fix for issue found during UAT AD migration on PASS login page - passing AD credentials to context
                {
                    using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, vLoginEmailId))
                    {
                        bool retField = context.ValidateCredentials(vLoginEmailId, vPassword.Trim());

                        if (retField == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Fn_Authenticate_Active_Directory_User_Internal");
                return false;
            }
        }
    }
}
