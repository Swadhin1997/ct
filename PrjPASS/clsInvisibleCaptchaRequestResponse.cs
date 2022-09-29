using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Script.Serialization;

namespace PrjPASS
{
    [DataContract]
    public class clsInvisibleCaptchaRequest
    {
        [DataMember]
        public string vTokenFromGoogleCaptchaService { get; set; }
        [DataMember]
        public string vLoginEmailId { get; set; }
        [DataMember]
        public string vSourceIPAddress { get; set; }

    }

    [DataContract]
    public class clsInvisibleCaptchaResponse
    {
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public double? score { get; set; }
        [DataMember]
        public string action { get; set; }
        [DataMember]
        public DateTime challenge_ts { get; set; }
        [DataMember]
        public string challenge_ts_as_string { get; set; }
        [DataMember]
        public string hostname { get; set; }

        [DataMember(Name = "error-codes")]
        [JsonProperty(PropertyName ="error-codes")]
        public List<string> error_codes { get; set; }
       
    }

    public class clsInvisibleCaptcha
    {
        public static clsInvisibleCaptchaResponse Fn_ReCaptchaSiteVerify(clsInvisibleCaptchaRequest objTokenVerifyRequest)
        {
            clsInvisibleCaptchaResponse objTokenVerifyResponse = new clsInvisibleCaptchaResponse();
            try
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                string captchaTokenVerifyURL = ConfigurationManager.AppSettings["InvisibleCaptchaSiteVerifyURL"].ToString();
                string secret = HttpContext.Current.Server.UrlEncode(ConfigurationManager.AppSettings["InvisibleCaptchaServerSideKey"]);
                string response = HttpContext.Current.Server.UrlEncode(objTokenVerifyRequest.vTokenFromGoogleCaptchaService);
                captchaTokenVerifyURL = string.Format(captchaTokenVerifyURL + "?secret={0}&response={1}", secret, response);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(captchaTokenVerifyURL);
                request.Method = "Get";

                if (ConfigurationManager.AppSettings["IsUseNetworkProxyForCaptcha"].ToString() == "1")
                {
                    WebProxy proxy = new WebProxy(ConfigurationManager.AppSettings["proxy_Address_Full"].ToString(), true);

                    string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                    string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();
                    string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();

                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;
                    request.Proxy = proxy;
                    request.Proxy = WebRequest.DefaultWebProxy;
                    request.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    request.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                }

                WebResponse responseFromGoogleCaptchaService = request.GetResponse();
                using (Stream responseStream = responseFromGoogleCaptchaService.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    objTokenVerifyResponse = (new JavaScriptSerializer()).Deserialize<clsInvisibleCaptchaResponse>(reader.ReadToEnd());
                    if (objTokenVerifyResponse != null)
                    {
                        objTokenVerifyResponse.challenge_ts_as_string = objTokenVerifyResponse.challenge_ts.ToString("dd/MM/yyyy HH:mm:ss").Replace("-", "/");
                        objTokenVerifyResponse.success = objTokenVerifyResponse.success && objTokenVerifyResponse.score >= Convert.ToDouble(ConfigurationManager.AppSettings["InvisibleCaptchaScoreThreshold"]);
                    }
                    else
                    {
                        objTokenVerifyResponse = new clsInvisibleCaptchaResponse();
                        objTokenVerifyResponse.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (objTokenVerifyResponse == null)
                    objTokenVerifyResponse = new clsInvisibleCaptchaResponse();
                objTokenVerifyResponse.success = false;
                clsAppLogs.LogEvent("Token Verification Request: " + (new JavaScriptSerializer()).Serialize(objTokenVerifyRequest) + ", Exception: " + ex.Message + ", Stack Trace: " + ex.StackTrace);
            }
            WebRequest.DefaultWebProxy = null;
            clsAppLogs.LogEvent("Token Verification Request: " + (new JavaScriptSerializer()).Serialize(objTokenVerifyRequest) + ", Token Verification Response: " + (new JavaScriptSerializer()).Serialize(objTokenVerifyResponse));

            return objTokenVerifyResponse;
        }
    }
}