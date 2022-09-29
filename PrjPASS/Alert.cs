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


/// <summary> 
/// A JavaScript alert 
/// </summary> 
public static class Alert
{

    /// <summary> 
    /// Shows a client-side JavaScript alert in the browser. 
    /// </summary> 
    /// <param name="message">The message to appear in the alert.</param> 
    public static void Show(string message, string vRedirectPath)
    {
        // Cleans the message to allow single quotation marks 
        string cleanMessage = message.Replace("'", "\\'");
        //string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";
        Page page = HttpContext.Current.CurrentHandler as Page;


        // Gets the executing web page 
        

        // Checks if the handler is a Page and that the script isn't allready on the Page 
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            ScriptManager.RegisterStartupScript(page, typeof(Alert), "Redit", "alert('" + cleanMessage + "'); window.location='" + vRedirectPath + "';", true);
        }
    }
    public static void Show(string message)
    {
        // Cleans the message to allow single quotation marks 
        string cleanMessage = message.Replace("'", "\\'");
        //string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";
        Page page = HttpContext.Current.CurrentHandler as Page;


        // Gets the executing web page 


        // Checks if the handler is a Page and that the script isn't allready on the Page 
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            ScriptManager.RegisterStartupScript(page,typeof(Alert), "Redit", "alert('" + cleanMessage + "');", true);
        }
    }
}


