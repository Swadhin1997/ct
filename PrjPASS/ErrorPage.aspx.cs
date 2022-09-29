using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectPASS
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = Request.QueryString["vErrorMsg"].ToString();
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            if (Session["ErrorCallingPage"] != null)
            {
                Response.Redirect(Session["ErrorCallingPage"].ToString());
            }
            else
            {
                Response.Redirect("FrmMainMenu.aspx");
            }
        }
    }
}
