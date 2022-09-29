using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;
using System.Web.Script.Services;
using System.Globalization;
using System.Configuration;
using System.Net;
using System.Data.SqlClient;
using System.Net.Mail;
using System.IO;
using ClosedXML.Excel;
using System.Security.Cryptography;

namespace PrjPASS
{
    public class CustomerInfo
    {
        public string MobileNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Age { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }


    }
    public partial class FrmGHICustomerInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod]
        public static string ValidateAndAddEmployee(CustomerInfo objCustomerDetails)
        {
            string Msg = string.Empty;
            string Status = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {

                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[PROC_VALIDATE_GHI_CUSTOMER]";
                        cmd.Parameters.AddWithValue("@EmployeeCode", objCustomerDetails.EmployeeCode);
                        cmd.Connection = conn;
                        conn.Open();
                        Status = Convert.ToString(cmd.ExecuteScalar());
                        //SqlDataAdapter sda = new SqlDataAdapter(cmdCheckUser);
                        //DataSet ds = new DataSet();
                        //sda.Fill(ds);
                    }
                }
                if (Convert.ToBoolean(Status == "Fail"))
                {
                    Msg = "Fail";

                    return Msg;
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection())
                    {

                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                        using (SqlCommand cmdAdd = new SqlCommand())
                        {
                            ///SqlCommand cmdAdd = new SqlCommand("[dbo].[PROC_ADD_GHI_CUSTOMER]", conn);/
                            //SqlCommand cmdAdd = new SqlCommand();
                            cmdAdd.CommandType = CommandType.StoredProcedure;
                            cmdAdd.CommandText = "[dbo].[PROC_ADD_GHI_CUSTOMER]";
                            cmdAdd.Parameters.AddWithValue("@EmployeeCode", objCustomerDetails.EmployeeCode);
                            cmdAdd.Parameters.AddWithValue("@Name", objCustomerDetails.Name);
                            cmdAdd.Parameters.AddWithValue("@DOB", objCustomerDetails.DOB);
                            cmdAdd.Parameters.AddWithValue("@Age", objCustomerDetails.Age);
                            cmdAdd.Parameters.AddWithValue("@Gender", objCustomerDetails.Gender);
                            cmdAdd.Parameters.AddWithValue("@EmailId", objCustomerDetails.EmailId);
                            cmdAdd.Parameters.AddWithValue("@MobileNo", objCustomerDetails.MobileNumber);
                            //Msg = "Success";

                            cmdAdd.Connection = conn;
                            conn.Open();
                            if (cmdAdd.ExecuteNonQuery() > 0)
                            {
                                Msg = "success";
                                //SendAcknowledgementEmail(objEmployeePrimaryDetails.EmployeeEmailId, objEmployeePrimaryDetails.EmployeeCode, objEmployeePrimaryDetails.EmployeeName, objEmployeePrimaryDetails.SelectedPremium, objEmployeePrimaryDetails.AccountNumber, objEmployeePrimaryDetails.IsKLTEmployee);
                            }
                            return Msg;
                        }
                    }
                    //conn.Close();

                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
            }

            return Msg;
        }
    }
}