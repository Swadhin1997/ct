using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.IO;

using OfficeInterop = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;


using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Data.SqlClient;

namespace PrjPASS
{
    public static class CommonExtensions
    {
        public static string ToIndianCurrencyFormat(this decimal number)
        {
            // set currency format

            string curCulture = Thread.CurrentThread.CurrentCulture.ToString();
            System.Globalization.NumberFormatInfo currencyFormat = new
                System.Globalization.CultureInfo(curCulture).NumberFormat;

            currencyFormat.CurrencyNegativePattern = 1;

            string currency_in_dollar = number.ToString("c", currencyFormat);

            /// for indian currency

            System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("en-IN");

            string currency_in_rupees = number.ToString("C2", info);

            return currency_in_rupees;
        }

        public static string ToIndianCurrencyFormatWithoutRuppeeSymbol(this decimal number)
        {
            // set currency format

            string curCulture = Thread.CurrentThread.CurrentCulture.ToString();
            System.Globalization.NumberFormatInfo currencyFormat = new
                System.Globalization.CultureInfo(curCulture).NumberFormat;

            currencyFormat.CurrencyNegativePattern = 1;

            string currency_in_dollar = number.ToString("c", currencyFormat);

            /// for indian currency

            System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("en-IN");

            string currency_in_rupees = number.ToString("C2", info);
            currency_in_rupees = currency_in_rupees.Substring(1);
            return currency_in_rupees;
        }

        public static string GetFormattedDate(DateTime datetime)
        {
            return datetime.ToString("dd-MMM-yyyy");
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {

                    return ip.ToString();
                }
            }
            return "Local IP Not Found";
        }

        public static bool IsContainSpecialChars(this string strInputValue)
        {
            strInputValue = strInputValue.Trim();
            return strInputValue.Any(ch => !Char.IsLetterOrDigit(ch));
        }

        public static bool IsContainSQLKeywordsOrIsContainSpecialChars(this string strInputValue)
        {
            bool IsContainSQLKeywords = false;
            bool IsContainSpecialChars = false;

            string[] stringArray = { "ADD", "CONSTRAINT", "ALTER", "COLUMN", "TABLE", "ALL", "AND", "ANY", "BACKUP ", "DATABASE", "BETWEEN", "CASE",
            "CHECK", "COLUMN", "CONSTRAINT", "CREATE", "INDEX", "REPLACE ", "VIEW", "PROCEDURE", "UNIQUE", "DEFAULT", "DELETE", "DESC", "DISTINCT", "DROP", "EXEC",
            "EXISTS", "FOREIGN", "KEY", "FROM", "FULL ", "OUTER", "JOIN", "GROUP BY", "HAVING", "IN", "INDEX", "INNER", "INSERT", "INSERT", "NULL", "JOIN", "LEFT",
            "LIKE", "LIMIT", "NOT", "ORDER BY", "OUTER JOIN", "PRIMARY", "KEY", "PROCEDURE", "PROC", "RIGHT", "ROWNUM",
            "SELECT", "DISTINCT", "INTO", "TOP", "SET", "TABLE", "TRUNCATE", "UNION", "UPDATE", "VALUES", "VIEW", "WHERE", "WAIT", "DELAY", "SET", "WHILE", "MASTER" };

            IsContainSQLKeywords = (stringArray.Any(strInputValue.Contains));

            strInputValue = strInputValue.Trim();
            IsContainSpecialChars = strInputValue.Any(ch => !Char.IsLetterOrDigit(ch));

            if (IsContainSQLKeywords || IsContainSpecialChars)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string GetIPAddress()
        {
            HttpContext context = HttpContext.Current;

            string Ip= context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(Ip))
            {
                Ip= context.Request.ServerVariables["REMOTE_ADDR"];
            }

            return Ip;
        }
        public static Hashtable fn_Get_Browser_Info()
        {
            Hashtable HTOfBrowserInfo = new Hashtable();
            HttpBrowserCapabilities bc = System.Web.HttpContext.Current.Request.Browser;

            HTOfBrowserInfo.Add("Type", bc.Type);
            HTOfBrowserInfo.Add("Name", bc.Browser);
            HTOfBrowserInfo.Add("Version", bc.Version);
            HTOfBrowserInfo.Add("Major Version", bc.MajorVersion);
            HTOfBrowserInfo.Add("Minor Version", bc.MinorVersion);
            HTOfBrowserInfo.Add("Platform", bc.Platform);
            HTOfBrowserInfo.Add("Is Beta", bc.Beta);
            HTOfBrowserInfo.Add("Is Crawler", bc.Crawler);
            HTOfBrowserInfo.Add("Is AOL", bc.AOL);
            HTOfBrowserInfo.Add("Is Win16", bc.Win16);
            HTOfBrowserInfo.Add("Is Win32", bc.Win32);
            HTOfBrowserInfo.Add("Supports Frames", bc.Frames);
            HTOfBrowserInfo.Add("Supports Tables", bc.Tables);
            HTOfBrowserInfo.Add("Supports Cookies", bc.Cookies);
            HTOfBrowserInfo.Add("Supports VB Script", bc.VBScript);
            HTOfBrowserInfo.Add("Supports JavaScript", bc.JavaScript);
            HTOfBrowserInfo.Add("Supports Java Applets", bc.JavaApplets);
            HTOfBrowserInfo.Add("Supports ActiveX Controls", bc.ActiveXControls);
            HTOfBrowserInfo.Add("CDF", bc.CDF);

           
            return HTOfBrowserInfo;
        }

        public static void fn_AddLogForDownload(string policyno,string module)
        {
            string logFile = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/FrmDownloadRenewalNotice_Error.txt";
            string ipaddress = CommonExtensions.GetIPAddress();
            Hashtable ht = CommonExtensions.fn_Get_Browser_Info();
            string source = ht["Name"].ToString();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_INSERT_DOC_DOWNLOAD_LOG", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vPolicyNo", policyno);
                        cmd.Parameters.AddWithValue("@vSource", source);
                        cmd.Parameters.AddWithValue("@vIPAddress", ipaddress);
                        cmd.Parameters.AddWithValue("@vStatus", "Success");
                        cmd.Parameters.AddWithValue("@vModule", module); 
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, " Error occured fnAddLogForRenewalNoticeDownload for Policy Number " + policyno
                                     + " from Module " + module + " Error description " + ex.ToString() + "    "
                                     + DateTime.Now.ToString() + System.Environment.NewLine);
            }
        }

        //Start : CR 471
        public static bool fn_Check_MobileNo(string mobileno)
        {
            bool IsExist = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_CHECK_MOBILENO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MobileNo", mobileno);

                       int i=Convert.ToInt32(cmd.ExecuteScalar());
                        if (i == 1)
                            IsExist = true;
                    }
                }
                return IsExist;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fn_Check_MobileNo");
                return IsExist;
            }
        }
        //End 


        //CR_P1_676_Start Pan Number validation 
        public static bool fn_Check_PanNumber(string panNumber)
        {
            bool isExist= true;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_CHECK_PANCARDNO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vPanCardNo", panNumber);

                        int result = Convert.ToInt32(cmd.ExecuteScalar());
                        if (result == 1)
                        {
                            isExist = false;
                        }
                        
                        //if(isValid)
                    }
                }
                return isExist;
            }
            catch(Exception ex)
            {
                ExceptionUtility.LogException(ex , "fn_Check_PanNumber");
                return isExist;
            }
        }
        //CR_P1_676_End Pan Number validation 

    }

    public static class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Decrypt(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public static string Encrypt(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

    

    }
}