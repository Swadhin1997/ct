using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using PrjPASS;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.Net;

namespace PrjPASS
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //Force Use TLS1.2 (.net framework 4.7.1 is installed)
            //ref: https://codeshare.co.uk/blog/how-to-force-a-net-website-to-use-tls-12/
            //if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
            //{
            //    ServicePointManager.Expect100Continue = true;
            //    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager
            //    ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            //}

            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteTable.Routes.MapPageRoute("Home", "AadhaarUpdate", "~/FrmAadhaarUpdate.aspx");
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            string strError = Server.GetLastError().ToString();
            //string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
            //File.WriteAllText(strXmlPath, String.Empty);
            //File.WriteAllText(strXmlPath, "Error: " + Server.GetLastError().ToString());
            PrjPASS.ExceptionUtility.LogException(Server.GetLastError(), "Application_Error block in Global.aspx");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");

            //Check If it is a new session or not , if not then do the further checks
            if (Request.Cookies["ASP.NET_SessionId"] != null && Request.Cookies["ASP.NET_SessionId"].Value != null)
            {
                string newSessionID = Request.Cookies["ASP.NET_SessionID"].Value;
                //Check the valid length of your Generated Session ID
                if (newSessionID.Length <= 24)
                {
                    string path = HttpContext.Current.Request.Url.AbsolutePath;

                    string NonLoggedInPages = ConfigurationManager.AppSettings["NonLoggedInPages"].ToString();
                    string[] Array_NonLoggedInPages = NonLoggedInPages.Split(';');
                    bool IsSkipSessionCheck = false;
                    foreach (string item in Array_NonLoggedInPages)
                    {
                        if (item.Length > 2)
                        {
                            IsSkipSessionCheck = path.ToLower().Contains(item.ToLower());
                            if (IsSkipSessionCheck)
                            {
                                break;
                            }
                            else
                            {
                                //LogEvent("item: " + item + " -:- path:" + path);
                            }
                        }
                    }

                    if (IsSkipSessionCheck == false)
                    {
                        //Log the attack details here
                        ExceptionUtility.LogException(new HttpException("Invalid Request"), "Global.asax.cs Application_BeginRequest 1, Accessed Page: " + path);
                        throw new HttpException("Invalid Request");
                    }
                }

                //Genrate Hash key for this User,Browser and machine and match with the Entered NewSessionID
                if (GenerateHashKey() != newSessionID.Substring(24))
                {
                    string path = HttpContext.Current.Request.Url.AbsolutePath;

                    string NonLoggedInPages = ConfigurationManager.AppSettings["NonLoggedInPages"].ToString();
                    string[] Array_NonLoggedInPages = NonLoggedInPages.Split(';');
                    bool IsSkipSessionCheck = false;
                    foreach (string item in Array_NonLoggedInPages)
                    {
                        if (item.Length > 2)
                        {
                            IsSkipSessionCheck = path.ToLower().Contains(item.ToLower());
                            if (IsSkipSessionCheck)
                            {
                                break;
                            }
                        }
                    }

                    if (IsSkipSessionCheck == false)
                    {
                        //Log the attack details here
                        ExceptionUtility.LogException(new HttpException("Invalid Request"), "Global.asax.cs Application_BeginRequest 2, Accessed Page: " + path);
                        Response.Redirect("FrmLogout.aspx");
                        //throw new HttpException("Invalid Request");
                    }
                }

                //Use the default one so application will work as usual//ASP.NET_SessionId
                Request.Cookies["ASP.NET_SessionId"].Value = Request.Cookies["ASP.NET_SessionId"].Value.Substring(0, 24);
            }
        }

        private string GenerateHashKey()
        {
            try
            {

                StringBuilder myStr = new StringBuilder();
                myStr.Append(Request.Browser.Browser);
                myStr.Append(Request.Browser.Platform);
                myStr.Append(Request.Browser.MajorVersion);
                myStr.Append(Request.Browser.MinorVersion);

                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {
                        myStr.Append(authTicket.Name);
                    }
                }

                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] hashdata = sha.ComputeHash(Encoding.UTF8.GetBytes(myStr.ToString()));
                return Convert.ToBase64String(hashdata);
            }
            catch
            {
                Response.Redirect("FrmLogout.aspx");
                return "";
            }
        }

        bool IsLocalEnvironment = ConfigurationManager.AppSettings["IsLocalEnvironment"].ToString() == "1" ? true : false;

        protected void Application_EndRequest(object sender, EventArgs e)
        {

            //Fix for VAPT Point: Session cookie attributes are not set properly
            //Another solution written in web.config
            try
            {
                if (Request.IsSecureConnection)
                {
                    if (Response.Cookies.Count > 0)
                    {
                        foreach (string s in Response.Cookies.AllKeys)
                        {
                            if (s == FormsAuthentication.FormsCookieName || "asp.net_sessionid".Equals(s, StringComparison.InvariantCultureIgnoreCase))
                            {
                                Response.Cookies[s].Secure = true;
                                //Response.Cookies[s].Path = IsLocalEnvironment ? "/" : "/kgipass/";
                            }
                        }
                    }
                }

                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    string[] paths = Request.Url.AbsoluteUri.Split('/');
                    Response.Cookies["ASP.NET_SessionId"].Value = Request.Cookies["ASP.NET_SessionId"].Value.Contains(GenerateHashKey()) == false ? Request.Cookies["ASP.NET_SessionId"].Value + GenerateHashKey() : Request.Cookies["ASP.NET_SessionId"].Value;
                    Response.Cookies["ASP.NET_SessionId"].Path = IsLocalEnvironment ? "/" : "/" + paths[3]; //Request.ApplicationPath; // "/KGIPASS";
                    LogEvent("ASP.NET_SessionId: " + Response.Cookies["ASP.NET_SessionId"].Value);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Application_EndRequest");
            }


        }

        public void LogEvent(string Message)
        {
            try
            {
                string buildPath = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/EventLog"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/EventLog");
                }

                string logFile = AppDomain.CurrentDomain.BaseDirectory + "/EventLog/Evnt_" + buildPath + ".txt";

                // Open the log file for append and write the log
                File.AppendAllText(logFile, string.Format("********** {0} **********", DateTime.Now.ToString()) + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, "==============================================" + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, Message + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, "==============================================" + "\n" + Environment.NewLine);
            }
            catch (Exception exc)
            {

            }
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            // It adds an entry to the Session object so the sessionID is kept for the entire session
            Session["init"] = "session start";

            // secure the ASP.NET Session ID only if using SSL
            // if you don't check for the issecureconnection, it will not work.
            if (Request.IsSecureConnection == true)
            {
                Response.Cookies["ASP.NET_SessionId"].Secure = true;
                //Response.Cookies["ASP.NET_SessionId"].Path = IsLocalEnvironment ? "/" : "/kgipass";
            }
        }


        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }

        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {

            // Fix for VAPT issue : CHECK IF HTTP REQUEST IS ALTERED : SHOULD NOT ALLOW CHANGE FROM POST TO GET
            // SEE TN POLICY VAPT REPORT
            // A local adversary can steal sensitive information from the browser's history Case 2.


            if (Request.HttpMethod != "GET")
            {
                return; // Nothing to do
            }

            var hasPostParams = (Request.QueryString["__EVENTTARGET"] ??
                                   Request.QueryString["__VIEWSTATE"] ??
                                   Request.QueryString["__EVENTARGUMENT"] ??
                                   Request.QueryString["__EVENTVALIDATION"]) != null;

            if (hasPostParams)
            {
                // TODO : log error (I suggest to also log HttpContext.Current.Request.RawUrl) and throw new exception
                throw new HttpException(405, "GET not Allowed.");
            }
        }
    }
}
