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
    public class EmployeePrimaryDetailsWD
    {
        public string SelectedPlan { get; set; }
        public string SelectedSixMonthPlan { get; set; }
        public string VerifiedOTP { get; set; }
        public string EmployeeAddressLine1 { get; set; }
        public string EmployeeAddressLine2 { get; set; }

        public string EmployeeAddressLine3 { get; set; }
        public string Pincode { get; set; }
        public string EmpMobileNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmailId { get; set; }
        public string SelectedPremium { get; set; }


        public string Height { get; set; }
        public string Weight { get; set; }
        public string NomineeName { get; set; }
        public string NomineeDOB { get; set; }
        public string UniqueRowId { get; set; }
        public string NomineeRelationWithProposer { get; set; }
        public string PaymentSuccess { get; set; }
        public bool IsRecordActive { get; set; }
        public bool IsGoodHealth { get; set; }
        public bool IsAgreedTermsCondition { get; set; }

    }
    public partial class FrmKGHC : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["key"] != null)
            {
                string Encrypted_EmpCode = Request.QueryString["key"].ToString();
                string EmpCode = Encryption.DecryptText(Encrypted_EmpCode);
                DataSet dsHealthSTPDetails = new DataSet();

                dsHealthSTPDetails = GetHealthSTPDetails(EmpCode);

                if (dsHealthSTPDetails != null)
                {
                    SetAllFields(dsHealthSTPDetails);
                }
                else
                {
                    //Alert.Show("No Data Exists for the given Employee Code", "FrmSTP.aspx");
                    //return;
                }
            }
        }


        private DataSet GetHealthSTPDetails(string EmployeeCode)
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "[dbo].[PROC_GET_HEALTH_KGHC_DETAILS]";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "EmployeeCode", DbType.String, ParameterDirection.Input, "EmployeeCode", DataRowVersion.Current, EmployeeCode);

                dbCommand.CommandType = CommandType.StoredProcedure;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "PROC_GET_HEALTH_KGHC_DETAILS Method, FrmKGHC Page");
            }
            return ds;
        }


        private void SetAllFields(DataSet dsHealthSTPDetails)
        {
            try
            {
                DataTable dtMemberDetails = dsHealthSTPDetails.Tables[0];
                lblEmployeeName.Text = dtMemberDetails.Rows[0]["EmployeeName"].ToString();
                lblPhone.Text = dtMemberDetails.Rows[0]["ContactNumber"].ToString();
                string strTabContent = string.Empty;
                string strTabNav = string.Empty;

                lblPremiumType1.Text = "Net Premium";
                lblPremiumAmount1.Text = dtMemberDetails.Rows[0]["6MonthsPremium1"].ToString();
                lblPremium2.Text = "Net Premium";
                lblNetPremium2Amount.Text = dtMemberDetails.Rows[0]["6MonthsPremium2"].ToString();
                lblGST1.Text = dtMemberDetails.Rows[0]["GST1"].ToString();
                lblGST2.Text = dtMemberDetails.Rows[0]["GST2"].ToString();
                lblSelectedPremiumMessage.Text = "Total Amount Payable - &#x20b9; ";
                lblSelectedPremium.Text = dtMemberDetails.Rows[0]["TotalAmountPayable1"].ToString();
                //lblDuductible1.Text = dtMemberDetails.Rows[0]["SumInsured1"].ToString();
                //lblDuductible2.Text = dtMemberDetails.Rows[0]["SumInsured2"].ToString();
                lblTotalAmountPayable1.Text = dtMemberDetails.Rows[0]["TotalAmountPayable1"].ToString();
                lblTotalAmountPayable2.Text = dtMemberDetails.Rows[0]["TotalAmountPayable2"].ToString();
                lblEmpEmailId.Text = dtMemberDetails.Rows[0]["EmailAddress"].ToString();
                lblEmployeeCode.Text = dtMemberDetails.Rows[0]["EmployeeID"].ToString();

                foreach (DataRow row in dtMemberDetails.Rows)
                {
                    string Title = row["Title"].ToString();
                    string caseMemberGender = row["Gender"].ToString();
                    string Releation = row["Relation"].ToString();
                    string EmailAddress = row["EmailAddress"].ToString();
                    string EmpCode = row["EmployeeID"].ToString();

                    if (caseMemberGender.ToUpper() == "M")
                    {
                        Title = "Mr";
                    }
                    if (caseMemberGender.ToUpper() == "F")
                    {
                        Title = "Miss";
                    }

                    string UniqueRowId = row["UniqueRowId"].ToString();
                    strTabNav = strTabNav + "<li role='presentation' class=''><a href='#UniqueRowId' aria-controls='' role='tab' data-toggle='tab' aria-expanded='false'></a></li>";
                    string Row1 = "<div id='" + Releation + UniqueRowId + "' role='tabpanel' class='tab-pane active' name='Self' customAttribut_MemberId='" + UniqueRowId + "'><div class='row' id='row" + Releation + "'>Row1</div>";
                    string Row2 = "<div class='row' id='row" + Releation + "2'>Row2</div>";
                    string Row3 = "<div class='row' id='row" + Releation + "3'>Row3</div></div>";
                    string Row4 = "<div class='row' id='row" + Releation + "4'>Row4</div></div>";
                    string MemberTitleMr = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Title</dt><dd><select id='cboTitle" + Releation + "' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px; readonly'><option value='Mr' selected>Mr</option><option value='Mrs'>Mrs</option><option value='Miss'>Miss</option></select></dd></dl></div>";
                    string MemberTitleMrs = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Title</dt><dd><select id='cboTitle" + Releation + "' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px; readonly'><option value='Mr'>Mr</option><option value='Mrs' selected>Mrs</option><option value='Miss'>Miss</option></select></dd></dl></div>";
                    string MemberTitleMiss = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Title</dt><dd><select id='cboTitle" + Releation + "' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px; readonly'><option value='Mr'>Mr</option><option value='Mrs'>Mrs</option><option value='Miss' selected>Miss</option></select></dd></dl></div>";

                    string MemberTitle = string.Empty;

                    if (Title == "Mr")
                    {
                        MemberTitle = MemberTitleMr;
                    }
                    else if (Title == "Mrs")
                    {
                        MemberTitle = MemberTitleMrs;
                    }
                    else if (Title == "Miss")
                    {
                        MemberTitle = MemberTitleMiss;
                    }

                    string MemberName = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Name*</dt><dd>" + row["EmployeeName"].ToString() + "</dd></dl></div>";
                    string MemberDOB = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>DOB*</dt><dd>" + row["DOB"].ToString() + "</dd></dl></div>";
                    string MemberAge = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Age*</dt><dd>" + row["Age"].ToString() + "</dd></dl></div>";
                    //string ContactNumber = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Mobile No</dt><dd>" + row["ContactNumber"].ToString() + "</dd></dl></div>";<input id='txt" + Releation + "Weight' name='" + Releation + " Weight' value='' type='text' class='form-control' style='width:60%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='3' /></dd>
                    string ContactNumber = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Mobile No*</dt><dd><input id='txt" + Releation + "mobileNo' name='" + Releation + " MobileNo' value='" + row["ContactNumber"].ToString() + "' type='text' class='form-control' style='width:110%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='10' /></dd></dl></div>";
                    string MemberGender = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Gender*</dt><dd>" + row["Gender"].ToString() + "</dd></dl></div>";
                    string Relation = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Relation*</dt><dd>" + row["Relation"].ToString() + "</dd></dl></div>";
                    EmailAddress = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Email ID*</dt><dd>" + row["EmailAddress"].ToString() + "</dd></dl></div>";
                    EmpCode = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Employee Code*</dt><dd>" + row["EmployeeID"].ToString() + "</dd></dl></div>";
                    string MemberHeight = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Height (in Cms)</dt><dd><input id='txt" + Releation + "Height' name='" + Releation + " Height' value='' type='text' class='form-control' style='width:60%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='3' /></dd></dl></div>";
                    string MemberWeight = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Weight (in Kgs)</dt><dd><input id='txt" + Releation + "Weight' name='" + Releation + " Weight' value='' type='text' class='form-control' style='width:60%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='3' /></dd></dl></div>";



                    string NomineeName = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Nominee Name*</dt><dd><input id='txt" + Releation + "NomineeName' name='' value='' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='50' /></dd></dl></div>";
                    string EmtRow = "<div class='col-sm-1'></div>";
                    string EmtRow2 = "<div class='col-sm-1'></div>";
                    string EmtRow3 = "<div class='col-sm-1'></div>";



                    string NomineeRelationWithProposer = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Relation With Proposer*</dt><dd><select id='cboNomineeRelationWithProposer" + Releation + "' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px;'><option value='Select' selected>Select</option><option value='Husband'>Husband</option><option value='Wife'>Wife</option><option value='Mother'>Mother</option><option value='father'>Father</option><option value='Son'>Son</option><option value='Daughter'>Daughter</option><option value='aunt'>Aunt</option><option value='Brother'>Brother</option><option value='brother in Law'>Brother In Law</option><option value='Daughter in Law'>Daughter in Law</option><option value='Friend'>Friend</option><option value='Fiance'>Fiance</option><option value='Father in law'>Father In Law</option><option value='grandfather'>Grandfather</option><option value='Grandmother'>Grandmother</option><option value='Mother in law'>Mother In Law</option><option value='Nephew'>Nephew</option><option value='niece'>Niece</option><option value='Sister'>Sister</option><option value='Sister in law'>Sister In Law</option><option value='Son in law'>Son In Law</option><option value='uncle'>Uncle</option></select></dd></dl></div>";

                    string NomineeDOB = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Nominee DOB*</dt><dd><input id='txt" + Releation + "NomineeDOB' name='" + Releation + " Nominee DOB' value='' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='dd/mm/yyyy' maxlength='10' readonly/></dd></dl></div>";

                    string EmpAddress1 = string.Empty;
                    string EmpAddress2 = string.Empty;
                    string EmpAddress3 = string.Empty;
                    string EmpPincode = string.Empty;
                    string chkIsNomineeSame = string.Empty; //AS PER DISCUSSION WITH GARIMA, THIS IS NOT REQUIRED ONWARDS DEV: 01-FEB-2018
                    string AlternateMobileNumber = string.Empty;
                    string SecondRow = string.Empty;
                    string ThirdRow = string.Empty;
                    string FourthRow = string.Empty;


                    EmpAddress1 = "<div class='col-sm-3'><dl style='margin-bottom:5px'><dt>Address Line 1*</dt><dd><input id='txt" + Releation + "AddressLine1' name='txt" + Releation + "AddressLine1' value='' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='' maxlength='50' /></dd></dl></div>";
                    EmpAddress2 = "<div class='col-sm-3'><dl style='margin-bottom:5px'><dt>Address Line 2</dt><dd><input id='txt" + Releation + "AddressLine2' name='txt" + Releation + "AddressLine2' value='' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='' maxlength='50' /></dd></dl></div>";
                    EmpAddress3 = "<div class='col-sm-3'><dl style='margin-bottom:5px'><dt>Address Line 3</dt><dd><input id='txt" + Releation + "AddressLine3' name='txt" + Releation + "AddressLine3' value='' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='' maxlength='50' /></dd></dl></div>";
                    EmpPincode = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Pincode*</dt><dd><input id='txt" + Releation + "Pincode' name='txt" + Releation + "Pincode' value='' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='' maxlength='8' /></dd></dl></div>";

                    string Note = "<div class='col-sm-10'><p>**Note:- Please provide the address on which you want to receive the GOQii Fitness device and the policy copy</p></div>";

                    if (Releation.ToLower() == "self")
                    {

                        SecondRow = SecondRow + Row2.Replace("Row2", MemberHeight + MemberWeight);
                        ThirdRow = ThirdRow + Row3.Replace("Row3", EmpAddress1 + EmpAddress2 + EmpAddress3 + EmpPincode + Note);
                        FourthRow = FourthRow + Row4.Replace("Row4", NomineeName + EmtRow + NomineeRelationWithProposer + EmtRow2 + NomineeDOB);

                        //strTabContent = strTabContent + Row1.Replace("Row1", MemberTitle + MemberName + MemberDOB + AlternateMobileNumber + MemberAge + MemberGender + Relation + EmailAddress + EmpCode + MemberHeight + MemberWeight + EmpAddress1 + EmpAddress2 + EmpPincode + NomineeName + NomineeRelationWithProposer + NomineeDOB + chkIsNomineeSame + SecondRow);
                        strTabContent = strTabContent + Row1.Replace("Row1", MemberTitle + MemberName + MemberDOB + ContactNumber + MemberAge + MemberGender + Relation + EmailAddress + EmpCode + chkIsNomineeSame) + SecondRow + ThirdRow + FourthRow;

                    }


                    ;
                }
                LiteralTabContent.Text = strTabContent;
                UpdateSTPIsPageViewedFlag();


            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SetAllFields Method, FrmKGHC Page");
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string SaveEmployeePrimaryDetails(EmployeePrimaryDetailsWD objEmployeePrimaryDetails)
        {
            string Msg = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.PROC_SAVE_HEALTH_WD_PRIMARY_DETAILS";

                        cmd.Parameters.AddWithValue("@EmployeeCode", objEmployeePrimaryDetails.EmployeeCode);
                        cmd.Parameters.AddWithValue("@SelectedPlan", objEmployeePrimaryDetails.SelectedPlan);
                        cmd.Parameters.AddWithValue("@EmpMobileNumber", objEmployeePrimaryDetails.EmpMobileNumber);
                        cmd.Parameters.AddWithValue("@SelectedSixMonthPlan", objEmployeePrimaryDetails.SelectedSixMonthPlan);
                        cmd.Parameters.AddWithValue("@VerifiedOTP ", objEmployeePrimaryDetails.VerifiedOTP);
                        cmd.Parameters.AddWithValue("@EmployeeAddressLine1", objEmployeePrimaryDetails.EmployeeAddressLine1);
                        cmd.Parameters.AddWithValue("@EmployeeAddressLine2", objEmployeePrimaryDetails.EmployeeAddressLine2);
                        cmd.Parameters.AddWithValue("@EmployeeAddressLine3", objEmployeePrimaryDetails.EmployeeAddressLine3);
                        cmd.Parameters.AddWithValue("@Pincode", objEmployeePrimaryDetails.Pincode);
                        cmd.Parameters.AddWithValue("@MemberHeight ", objEmployeePrimaryDetails.Height);
                        cmd.Parameters.AddWithValue("@MemberWeight ", objEmployeePrimaryDetails.Weight);
                        cmd.Parameters.AddWithValue("@NomineeName  ", objEmployeePrimaryDetails.NomineeName);
                        cmd.Parameters.AddWithValue("@NomineeDOB ", objEmployeePrimaryDetails.NomineeDOB);
                        cmd.Parameters.AddWithValue("@NomineeRelationWithProposer ", objEmployeePrimaryDetails.NomineeRelationWithProposer);
                        cmd.Parameters.AddWithValue("@IsRecordActive ", true);
                        cmd.Parameters.AddWithValue("@IsGoodHealth ", true);
                        cmd.Parameters.AddWithValue("@IsAgreedTermsCondition  ", true);

                        cmd.Connection = conn;
                        conn.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            Msg = "success";
                            //SendAcknowledgementEmail(objEmployeePrimaryDetails.EmployeeEmailId, objEmployeePrimaryDetails.EmployeeCode, objEmployeePrimaryDetails.EmployeeName, objEmployeePrimaryDetails.SelectedPremium, objEmployeePrimaryDetails.AccountNumber, objEmployeePrimaryDetails.IsKLTEmployee);
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Msg = "error";
                ExceptionUtility.LogException(ex, "SaveMemberDetails FrmKGHC.aspx");
            }
            return Msg;
        }


        [WebMethod]
        [ScriptMethod]
        public static string GenerateOTPNew(string MobileNumber, string EmailId, string EmployeeCode, string EmployeeName)
        {
            string GeneratedOTP = string.Empty;
            try
            {
                Random r = new Random();
                GeneratedOTP = r.Next(100000, 999999).ToString();

                bool IsOTPSavedToDB = SaveOTP(EmployeeCode.Trim(), GeneratedOTP, "Insert");
                if (IsOTPSavedToDB)
                {
                    HttpContext.Current.Session["OTPNumber"] = GeneratedOTP;
                    //SendOTPEmail(EmailId, EmployeeCode, GeneratedOTP, EmployeeName);

                    if (MobileNumber.Length == 10)
                    {
                        bool IsSendSMSSuccess = SendSMS(GeneratedOTP, MobileNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                GeneratedOTP = "";
                ExceptionUtility.LogException(ex, "Error in GenerateOTP on FrmKGC Page");
            }
            return string.IsNullOrEmpty(GeneratedOTP) ? "" : "success";
        }


        private static bool SendSMS(string GeneratedOTP, string MobileNumber)
        {
            bool IsSendSMSSuccess = false;

            try
            {
                string strPath = string.Empty;
                string smsBody = string.Empty;

                smsBody = ConfigurationManager.AppSettings["smsBody"];
                smsBody = smsBody.Replace("@otpNumber", Convert.ToString(GeneratedOTP));

                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "0")
                {
                    // string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
                    var client = new System.Net.WebClient();
                    var content = client.DownloadString(URI);
                }
                else
                {
                    string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                    string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                    string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();
                    string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                    string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();

                    string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();

                    WebProxy proxy = new WebProxy(proxy_Address_Full);
                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;

                    //  string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                    var client = new System.Net.WebClient();
                    client.Proxy = proxy;

                    client.Proxy = WebRequest.DefaultWebProxy;
                    client.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    client.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);

                    var content = client.DownloadString(URI);
                    IsSendSMSSuccess = true;
                }
            }
            catch (Exception ex)
            {
                IsSendSMSSuccess = false;
                ExceptionUtility.LogException(ex, "Error in SendSMS on FrmKGHC Page");
            }
            return IsSendSMSSuccess;
        }

        private static bool SaveOTP(string EmployeeCode, string GeneratedOTP, string ActionName)
        {
            bool IsOTPSavedToDB = false;
            int RowsAffected = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_OTP_FOR_HEALTH_WD";

                        cmd.Parameters.AddWithValue("@EmployeeCode", EmployeeCode.Trim());
                        cmd.Parameters.AddWithValue("@GeneratedOTP", GeneratedOTP);
                        cmd.Parameters.AddWithValue("@Action", ActionName);

                        cmd.Connection = conn;
                        conn.Open();
                        RowsAffected = cmd.ExecuteNonQuery();
                        if (RowsAffected > 0)
                        {
                            IsOTPSavedToDB = true;
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveOTP, FrmKGHC Page");
            }
            return IsOTPSavedToDB;
        }

        public string ValidateOTP(string OTPNumber, string EmployeeCode)
        {
            string Msg = string.Empty;
           
            if (HttpContext.Current.Session["OTPNumber"] != null && OTPNumber.Trim().Length > 0)
            {
                if (OTPNumber == HttpContext.Current.Session["OTPNumber"].ToString())
                {
                    Msg = "success";
                    Random r = new Random();
                    string randomTxnId = r.Next(100000, 999999).ToString();
                    //SaveOTP(EmployeeCode, OTPNumber, "Update");

                    string[] hashVarsSeq;
                    string hash_string = string.Empty;
                    // propNumber = "201808270000245";

                    hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|');
                    foreach (string hash_var in hashVarsSeq)
                    {
                        if (hash_var == "key")
                        {
                            hash_string = hash_string + ConfigurationManager.AppSettings["key_payu"];
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "txnid")
                        {
                            hash_string = hash_string + lblPhone.Text + randomTxnId;
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "amount")
                        {
                            //string[] prem = lblTotalPremium.Text.Split(' ');

                            hash_string = hash_string + Convert.ToDecimal(lblSelectedPremium.Text).ToString("g29");
                            //hash_string = hash_string + Convert.ToDecimal(prem[1]).ToString("g29");
                            hash_string = hash_string + '|';
                        }

                        else if (hash_var == "productinfo")
                        {
                            hash_string = hash_string + ConfigurationManager.AppSettings["productInfoEmpHC"];
                            hash_string = hash_string + '|';
                        }

                        else if (hash_var == "firstname")
                        {
                            hash_string = hash_string + lblEmployeeName.Text;
                            hash_string = hash_string + '|';
                        }

                        else if (hash_var == "email")
                        {
                            hash_string = hash_string + lblEmpEmailId.Text;
                            hash_string = hash_string + '|';
                        }

                        else if (hash_var == "mobile")
                        {
                            hash_string = hash_string + lblPhone.Text;
                            hash_string = hash_string + '|';
                        }

                        else if (hash_var == "udf1")
                        {
                            hash_string = hash_string + lblEmployeeCode.Text;
                            hash_string = hash_string + '|';
                        }

                        else if (hash_var == "udf2")
                        {
                            hash_string = hash_string + ConfigurationManager.AppSettings["typeOfUserEmpHC"];
                            hash_string = hash_string + '|';
                        }

                        else if (hash_var == "udf3")
                        {
                            hash_string = hash_string + lblEmployeeCode.Text;
                            hash_string = hash_string + '|';
                        }

                        else if (hash_var == "udf4")
                        {
                            hash_string = hash_string + lblPhone.Text;
                            hash_string = hash_string + '|';
                        }

                        else if (hash_var == "udf5")
                        {
                            string vFinalString = "NA" + "," + lblEmployeeName.Text + "," + ConfigurationManager.AppSettings["typeOfUserEmpHC"] + "," + lblPhone.Text + "," + lblEmployeeCode.Text + "," + "NA" + "," + DateTime.Now.ToString("dd/MM/yyyy") + "," + DateTime.Now.ToString("dd/MM/yyyy") + "," + "KEHC" + "," + "Pass_User" + "," + lblPhone.Text + "," + (Convert.ToString(Session["OTPNumber"]) == "0" || Session["OTPNumber"] == null ? "WithoutOTP" : "WithOTP") + "," + (Session["OTPNumber"] == null ? "0" : Session["OTPNumber"]);
                            hash_string = hash_string + vFinalString;
                            //hash_string = hash_string + "NA";
                            hash_string = hash_string + '|';
                        }

                        else
                        {

                            hash_string = hash_string + (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
                            hash_string = hash_string + '|';
                        }
                    }


                    hash_string += ConfigurationManager.AppSettings["salt_payu"];
                    string hash1 = Generatehash512(hash_string).ToLower();
                    string action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";

                    if (!string.IsNullOrEmpty(hash1))
                    {
                        //hash.Value = hash1;
                        //txnid.Value = txnid1;

                        System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                        data.Add("hash", hash1);
                        data.Add("txnid", lblPhone.Text + randomTxnId);
                        data.Add("key", ConfigurationManager.AppSettings["key_payu"]);
                        //string[] prem = lblTotalPremium.Text.Split(' ');
                        string AmountForm = Convert.ToDecimal(lblSelectedPremium.Text).ToString("g29");// eliminating trailing zeros
                                                                                                       //amount.Text = AmountForm;
                                                                                                       //string AmountForm = Convert.ToDecimal(prem[1]).ToString("g29");
                        data.Add("amount", AmountForm);
                        data.Add("firstname", lblEmployeeName.Text);
                        data.Add("email", lblEmpEmailId.Text);
                        data.Add("phone", lblPhone.Text);
                        data.Add("productinfo", ConfigurationManager.AppSettings["productInfoEmpHC"]);
                        data.Add("surl", ConfigurationManager.AppSettings["surlEmpHealthCare"]);
                        data.Add("furl", ConfigurationManager.AppSettings["furlEmpHealthCare"]);
                        data.Add("lastname", "");
                        data.Add("curl", "");
                        data.Add("address1", "");
                        data.Add("address2", "");
                        data.Add("city", "");
                        data.Add("state", "");
                        data.Add("country", "");
                        data.Add("zipcode", "");
                        data.Add("udf1", lblEmployeeCode.Text);
                        data.Add("udf2", ConfigurationManager.AppSettings["typeOfUserEmpHC"]);
                        data.Add("udf3", lblEmployeeCode.Text);
                        data.Add("udf4", lblPhone.Text);
                        string vFinalString = "NA" + "," + lblEmployeeName.Text + "," + ConfigurationManager.AppSettings["typeOfUserEmpHC"] + "," + lblPhone.Text + "," + lblEmployeeCode.Text + "," + "NA" + "," + DateTime.Now.ToString("dd/MM/yyyy") + "," + DateTime.Now.ToString("dd/MM/yyyy") + "," + "KEHC" + "," + "Pass_User" + "," + lblPhone.Text + "," + (Convert.ToString(Session["OTPNumber"]) == "0" || Session["OTPNumber"] == null ? "WithoutOTP" : "WithOTP") + "," + (Session["OTPNumber"] == null ? "0" : Session["OTPNumber"]);
                        data.Add("udf5", vFinalString);
                        data.Add("pg", "");

                        Database db = DatabaseFactory.CreateDatabase("cnPASS");
                        using (SqlConnection con = new SqlConnection(db.ConnectionString))
                        {

                            con.Open();

                            SqlCommand cmdCheck = new SqlCommand("PROC_PAYU_SAVE_REQUEST_DETAILS", con);
                            cmdCheck.CommandType = CommandType.StoredProcedure;
                            cmdCheck.Parameters.AddWithValue("@hash", hash1);
                            cmdCheck.Parameters.AddWithValue("@txnid", lblPhone.Text + randomTxnId);
                            cmdCheck.Parameters.AddWithValue("@vkey", ConfigurationManager.AppSettings["key_payu"]);
                            cmdCheck.Parameters.AddWithValue("@amount", AmountForm);
                            cmdCheck.Parameters.AddWithValue("@firstname", lblEmployeeName.Text);
                            cmdCheck.Parameters.AddWithValue("@email", lblEmpEmailId.Text);
                            cmdCheck.Parameters.AddWithValue("@phone", hdnMobileNumber.Value);
                            cmdCheck.Parameters.AddWithValue("@productinfo", ConfigurationManager.AppSettings["productInfoEmpHC"]);
                            cmdCheck.Parameters.AddWithValue("@surl", ConfigurationManager.AppSettings["surlEmpHealthCare"]);
                            cmdCheck.Parameters.AddWithValue("@furl", ConfigurationManager.AppSettings["furlEmpHealthCare"]);
                            cmdCheck.Parameters.AddWithValue("@lastname", "");
                            cmdCheck.Parameters.AddWithValue("@curl", "");
                            cmdCheck.Parameters.AddWithValue("@address1", "");
                            cmdCheck.Parameters.AddWithValue("@address2", "");
                            cmdCheck.Parameters.AddWithValue("@city", "");
                            cmdCheck.Parameters.AddWithValue("@state", "");
                            cmdCheck.Parameters.AddWithValue("@country", "");
                            cmdCheck.Parameters.AddWithValue("@zipcode", "");
                            cmdCheck.Parameters.AddWithValue("@udf1", lblEmployeeCode.Text);
                            cmdCheck.Parameters.AddWithValue("@udf2", ConfigurationManager.AppSettings["typeOfUserEmpHC"]);
                            cmdCheck.Parameters.AddWithValue("@udf3", lblEmployeeCode.Text);
                            cmdCheck.Parameters.AddWithValue("@udf4", lblPhone.Text);
                            cmdCheck.Parameters.AddWithValue("@udf5", vFinalString);
                            cmdCheck.Parameters.AddWithValue("@pg", "");
                            cmdCheck.ExecuteNonQuery();
                        }

                        using (SqlConnection con = new SqlConnection(db.ConnectionString))
                        {

                            con.Open();

                            SqlCommand cmdCheck = new SqlCommand("[dbo].[PROC_UPDATE_TXNID_AGAINST_EMPLOYEE]", con);
                            cmdCheck.CommandType = CommandType.StoredProcedure;
                            cmdCheck.Parameters.AddWithValue("@EmployeeID", lblEmployeeCode.Text);
                            cmdCheck.Parameters.AddWithValue("@txnid", lblPhone.Text + randomTxnId);

                            cmdCheck.ExecuteNonQuery();
                        }

                        string strForm = PreparePOSTForm(action1, data);
                        Page.Controls.Add(new LiteralControl(strForm));
                    }
                    else
                    {
                        //SavePaymentFailedStatus(false, EmployeeCode);
                    }

                }

               

            }
            return Msg;
        }

        //public static void SavePaymentFailedStatus(bool PaymentStatus, string EmployeeCode)
        //{
        //    try
        //    {
        //        string Msg = string.Empty;
        //        using (SqlConnection conn = new SqlConnection())
        //        {
        //            conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.CommandText = "PROC_SAVEFAILEDPAYMENT_HEALTH_WD_PRIMARY_DETAILS";
        //                cmd.Parameters.AddWithValue("@EmployeeCode ", EmployeeCode);
        //                cmd.Parameters.AddWithValue("@PaymentSuccess ", PaymentStatus);
        //                cmd.Connection = conn;
        //                conn.Open();
        //                if (cmd.ExecuteNonQuery() > 0)
        //                {
        //                    Msg = "success";

        //                }

        //                conn.Close();
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, "SavePaymentFailedStatus, FrmKGHC Page");
        //    }
        //}


        protected void ExportExcel(object sender, EventArgs e)
        {
            //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand("Select * from [dbo].[TBL_HEALTHCARE_WD_EXCEL_UPLOAD_DETAILS]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, "Customers");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xls");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }
        }

        public string Generatehash512(string text)
        {
            try
            {
                byte[] message = Encoding.UTF8.GetBytes(text);

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] hashValue;
                SHA512Managed hashString = new SHA512Managed();
                string hex = "";
                hashValue = hashString.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
            catch (Exception ex)
            {
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in Generatehash512 for text :" + text + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                //CommonUtility.Fn_LogEvent("FrmReviewConfirm.aspx :: Proposal " + propNumber + " Error occured in Generatehash512 for text :" + text + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace);
                ExceptionUtility.LogException(ex, "Generatehash512, FrmKGHC Page");
                Response.Redirect("FrmCustomErrorPage.aspx");
                return null;
            }
        }

        private string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
            try
            {
                //Set a name for the form
                string formID = "PostForm";
                //Build the form using the specified data to be posted.
                StringBuilder strForm = new StringBuilder();
                strForm.Append("<form id=\"" + formID + "\" name=\"" +
                               formID + "\" action=\"" + url +
                               "\" method=\"POST\">");

                foreach (System.Collections.DictionaryEntry key in data)
                {

                    strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                                   "\" value=\"" + key.Value + "\">");
                }


                strForm.Append("</form>");
                //Build the JavaScript which will do the Posting operation.
                StringBuilder strScript = new StringBuilder();
                strScript.Append("<script language='javascript'>");
                strScript.Append("var v" + formID + " = document." +
                                 formID + ";");
                strScript.Append("v" + formID + ".submit();");
                strScript.Append("</script>");
                //Return the form and the script concatenated.
                //(The order is important, Form then JavaScript)
                return strForm.ToString() + strScript.ToString();
            }

            catch (Exception ex)
            {
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in Preparepostform for url :" + url + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                //CommonUtility.Fn_LogEvent("FrmCustomerReviewConfirm.aspx :: Error occured in Preparepostform for url :" + url + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace);
                ExceptionUtility.LogException(ex, "PreparePOSTForm, FrmKGHC Page");
                Response.Redirect("FrmCustomErrorPage.aspx");
                return null;
            }

        }

        protected void btnMobileVerify_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateOTP(txtOtpNumber.Text, lblEmployeeCode.Text);
            }

            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "btnMobileVerify_Click, FrmKGHC Page");
            }
        }


        

        [WebMethod]
        [ScriptMethod]
        public static string ValidateOTP1(string OTPNumber, string EmployeeCode)
        {
            string Msg = string.Empty;
            if (HttpContext.Current.Session["OTPNumber"] != null && OTPNumber.Trim().Length > 0)
            {
                if (OTPNumber == HttpContext.Current.Session["OTPNumber"].ToString())
                {
                    Msg = "success";
                    SaveOTP(EmployeeCode, OTPNumber, "Update");
                }

            }
            return Msg;
        }

        private void UpdateSTPIsPageViewedFlag()
        {
            try
            {
                if (lblEmployeeCode.Text.Trim() != "" && lblEmployeeCode.Text.Trim() != "-")
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "PROC_SAVE_KGHC_PAGE_VIEWED_FLAG";

                            cmd.Parameters.AddWithValue("@EmployeeCode", lblEmployeeCode.Text.Trim());

                            cmd.Connection = conn;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "UpdateSTPIsPageViewedFlag Method");
            }
        }
    }
}
