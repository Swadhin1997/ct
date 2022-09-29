using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class TalismaPullSMS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateInteraction_Click(object sender, EventArgs e)
        {
            lblstatus.Text = "-";
            try
            {
                if (txtFromMobileNumber.Text.Trim().Length == 10 && txtMessage.Text.Trim().Length > 0)
                {
                    string mobnumber = txtFromMobileNumber.Text.Trim();
                    string mesg = txtMessage.Text.Trim();
                    string timstmp = DateTime.Now.ToString();

                    string strTalismaExtratorService = ConfigurationManager.AppSettings["strTalismaExtratorService"].ToString();
                    string URL = strTalismaExtratorService + "?to=1234&from=" + mobnumber + "&timestamp=" + timstmp + "&message=" + mesg;

                    HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
                    request.KeepAlive = false;
                    request.ProtocolVersion = HttpVersion.Version10;
                    request.Method = "POST";
                    request.Timeout = 30000;
                    request.ContentLength = 0;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    StreamReader streamr = new StreamReader(response.GetResponseStream());

                    lblstatus.Text = streamr.ReadToEnd() + "<br>Responded At: " + DateTime.Now.ToString() + "<br>Posted URL is:<br>" + URL;
                }
            }
            catch (Exception ex)
            {
                lblstatus.Text = ex.Message;
            }
        }
    }
}