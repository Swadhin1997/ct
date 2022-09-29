using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using cryptoPDF.api;
using System.Data.SqlClient;
using System.Data;

namespace PrjPASS
{
    /// <summary>
    /// Summary description for DigitalCertificate
    /// </summary>
    public sealed class clsDigitalCertificate
    {
        //static string licensekey = "TmFtZTpGdXR1cmVHZW5lcmFsaXxTTjoxMDAwMnxEYXRlOjEwLzMxLzIwMTZ8bGljOnVSZU1UK1c1WXdnNWx3OVFjY2VmWElNL0RCOUJIWGZKOE0yd3IrcjZpL0N2TWcwMWFUUzdqZVp3UnhTcCtKc1pGTVpEOUpuZDhoQ0RWRDJnSEZBUllrT25oQkVZa1A0SjVHcTRRbUdueENMMkk3TDZmbWJXMEN5N0FaTjl6SHROVGczU2pCU0FrR1l2b0M1NlFOenQ0RXpXU05OZHZleE5STzBZRURIbHhaeUZUbUt4UDZuNVVuSEFvU0hYY1FzYVNjaW1CQlZBalFEVWZSRU5MdzRtZXpvQmU4UHUxdFVkN0VPS1JOK2J0Ry9mcnE0M0JkZjkyVVdWcWtXQWM2TVdzc01hem96SVVTempZWStaTWRJRXY5dU5ZYW80L01nZzk2ZFVDVFBUZVUyK01XczltRmNFSW1lV3RzdEdBNmxpMkxENlNSNEZ2QTRidDJFbHBtYjFyUT09";
        static string licensekey = "TmFtZTpQREZBdXRvU2lnbmVyfFNOOjEwMDAxfERhdGU6MDEvMDEvMjA0NXxsaWM6UVBtNDNxT2sycXhySStyMllEeWdjaXBxYnBPTmtFUTdPT0ZJTmdjYUIvT2pMNFMyUjRBOGZGTjdaS1MrSXEzdmRydXZSVWdZM1V5RU4xMUhPeXpqNWRUVzZnTXBGR21SMVhpRjN1akNPT2lmMW8xdDRaNXJQKzVUQ1BpRWNqZE9xV1ovNjJQMExVTDhvVGJaRGxUcXlCUVFtZUVQU1cvVGsvR1M4ZGVPSXVid0tGV01VK3djWWV2N1pueTdiWG5OUERjdUszT0swQjdtTU1ic0VPZ3RuMGV0VzFWUkRnbzdTbllHY1JFR0p4K2dvZHROcUtzcXdkZEtFQnFjYkR2SURmOHU0eEJENTladEx2ME15YjNzU0xITVR3ekk5VEpWREwvMFlUcUpwUXV0QWdCUW1DV2R5RENMWmduV01XMGxFMXhjMWM0Y3Izblp0dlVvUUdWdEJBPT0=";

        //  static string licensekey = ConfigurationSettings.AppSettings["licensekey"];
        static pdfapi _oPdfapi = null;
        private static pdfapi oPdfapi
        {
            get
            {
                if (_oPdfapi == null)
                    _oPdfapi = new pdfapi(licensekey);
                return _oPdfapi;
            }
        }

        public clsDigitalCertificate()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        

        public static byte[] Sign(byte[] inputPDF)
        {
            //HSM Parametters
            //if 64bit then use path "C:\\Program Files\\SafeNet\\LunaClient\\cryptoki.dll"

            string pkcsLibraryPath = ConfigurationManager.AppSettings["pkcsLibraryPath"].ToString(); // "C:\\Program Files\\SafeNet\\LunaClient\\win32\\cryptoki.dll";
            //string SerialNumber = ConfigurationManager.AppSettings["SerialNumber"].ToString(); // "511573014"; //partition number
            string Label = ConfigurationManager.AppSettings["Label"].ToString(); // "par1"; //name
            string certificateID = ConfigurationManager.AppSettings["certificateID"].ToString(); // "987654";
            string SerialNumber = fnGetSerialNumberfromDB();
            string certificateLabel = ConfigurationManager.AppSettings["certificateLabel"].ToString(); // "mahesh";
            string hsmpassword = ConfigurationManager.AppSettings["hsmpassword"].ToString(); // "userpin";

            SigImageType sigImageType = new SigImageType();
            sigImageType.ShowSigImage = false;
            sigImageType.SigImagePath = null;
            sigImageType.ShowTextWithImage = false;

            //Pages to Sign Parametters
            Pages pts = new Pages();
            //            pts.SignatureOnPage = Pages.PagesToSign.Last;
            pts.SignatureOnPage = Pages.PagesToSign.Last;

            //Position of signature on page
            StandardSignatureBox.position pos = StandardSignatureBox.position.BOTTOMLEFT;

            //SignatureBox parametter
            int Height = 100;
            int Width = 200;
            string SigningReason = "";
            string SigningLocation = ConfigurationManager.AppSettings["SigningLocation"].ToString(); // "Mumbai";
            cryptoPDF.api.StandardSignatureBox sigbox = new StandardSignatureBox(Height, Width, pos, SigningReason, SigningLocation, sigImageType);
            //Document Password
            cryptoPDF.PasswordProtect passprt = new cryptoPDF.PasswordProtect();
            //         passprt.Enable = true;
            passprt.Enable = false;
            passprt.OwnerPassword = "password";
            passprt.UserPassword = "password";

            //Added on 20/08/2019 By Nilesh
            cryptoPDF.api.securityparams objSecurity = new securityparams();
            objSecurity.OwnerPassword = ConfigurationManager.AppSettings["ownerPwd"].ToString();
            ////   objSecurity.UserPassword = "123";
            objSecurity.Allow_Copy = false;
            objSecurity.ALLOW_PRINTING = true;
            objSecurity.ALLOW_DEGRADED_PRINTING = false;
            //End on 20/08/2019 By Nilesh

            //Sign PDF
            //            return oPdfapi.signPdf(pkcsLibraryPath, SerialNumber, Label, certificateLabel, certificateID, inputPDF, Net.Crypto.PDF.HashAlgorithm.SHA1, hsmpassword, sigbox, pts, false, false, passprt); //visible signature
            //commnet on 20/08/2019
            //return oPdfapi.signPdf(pkcsLibraryPath, SerialNumber, Label, certificateLabel, certificateID, inputPDF, Net.Crypto.PDF.HashAlgorithm.SHA1, hsmpassword, sigbox, pts, true, false, passprt); //invisible signature
            return oPdfapi.signPdf(pkcsLibraryPath, SerialNumber, Label, certificateLabel, certificateID, inputPDF, Net.Crypto.PDF.HashAlgorithm.SHA1, hsmpassword, sigbox, pts, true, false, "", objSecurity);
        }

        private static string fnGetSerialNumberfromDB()
        {
            string SerialNumber = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ToString()))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_SERIAL_NUMBER", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SerialNumber = cmd.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnGetSectificateIDfromDB()s");
            }
            return SerialNumber;
        }
    }
}