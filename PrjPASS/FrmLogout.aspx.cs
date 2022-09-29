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

namespace ProjectPASS
{
    public partial class FrmLogout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

            FormsAuthentication.SignOut();

            if (Session["vUserLoginId"] != null)
            {
                Session["vUserLoginId"] = null;
            }

            ExpireAllCookies();

            Session.Abandon(); // Session Expire but cookie do exist
            if (Response.Cookies["ASP.NET_SessionId"] != null)
            {
                if (Response.Cookies["ASP.NET_SessionId"].Value != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-30); //Delete the cookie
                }
            }

            if (Response.Cookies["AuthToken"] != null)
            {
                if (Response.Cookies["AuthToken"].Value != null)
                {
                    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddDays(-30); //Delete the cookie
                }
            }
            Response.Redirect("FrmSecuredLogin.aspx");
        }

        private void ExpireAllCookies()
        {
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
    }
}
