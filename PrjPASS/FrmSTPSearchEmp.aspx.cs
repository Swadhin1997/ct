using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Services;
using System.Web.Script.Services;
using System.Globalization;
using System.Configuration;
using System.Net;
using System.Data.SqlClient;
using System.Net.Mail;
using System.IO;
using System.Text.RegularExpressions;

namespace PrjPASS
{

    public partial class FrmSTPSearchEmp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string SearchEmpCode(string EmployeeCode, string Password)
        {
            string url = "norecordfound";
            try
            {
                if (Password == "Password@123#")
                {
                    url = ReturnURL(EmployeeCode);
                }
                else
                {
                    url = "passwordnotmatch";
                }
            }
            catch (Exception ex)
            {
                url = "norecordfound";
                ExceptionUtility.LogException(ex, "SearchEmpCode Method - FrmSTPSearchEmp.aspx.cs");
            }
            return url;
        }

        [WebMethod]
        [ScriptMethod]
        public static string ValidateAndSearchEmpCode(string EmployeeCode, string EmailId, string ContactNumber)
        {
            string url = "norecordfound";
            try
            {
                if (VaidateEmp(EmployeeCode, EmailId, ContactNumber))
                {
                    url = ReturnURL(EmployeeCode);
                }
                else
                {
                    url = "passwordnotmatch";
                }
            }
            catch (Exception ex)
            {
                url = "norecordfound";
                ExceptionUtility.LogException(ex, "ValidatendSearchEmpCode Method - FrmSTPSearchEmp.aspx.cs");
            }
            return url;
        }

        private static bool VaidateEmp(string EmployeeCode, string EmailId, string ContactNumber)
        {
            bool Valid = false;
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_VALIDATE_EMP_STP_SEARCH";
                        cmd.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                        cmd.Parameters.AddWithValue("@EmailId", EmailId);
                        cmd.Parameters.AddWithValue("@ContactNumber", ContactNumber);
                        cmd.Connection = con;
                        con.Open();
                        int Count = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                        if (Count == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                ExceptionUtility.LogException(ex, "VaidateEmp Method - FrmSTPSearchEmp.aspx.cs");
            }
            return Valid;
        }

        private static string ReturnURL(string EmployeeCode)
        {
            var url = "norecordfound";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_HEALTH_STP_EMPLOYEE_SEARCH";

                        cmd.Parameters.AddWithValue("@EmployeeCode", EmployeeCode.Trim());

                        cmd.Connection = conn;
                        conn.Open();
                        var EmpUrl = cmd.ExecuteScalar();
                        conn.Close();

                        if (EmpUrl != null)
                        {
                            Uri uriResult;
                            bool IsValidURL = Uri.TryCreate(EmpUrl.ToString(), UriKind.Absolute, out uriResult)
                                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                            if (!IsValidURL)
                            {
                                url = "norecordfound";
                            }
                            else
                            {
                                url = EmpUrl.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                url = "norecordfound";
                ExceptionUtility.LogException(ex, "ReturnURL Method - FrmSTPSearchEmp.aspx.cs");
            }

            return url;
        }


    }
}