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

namespace PrjPASS
{
    public partial class FrmSTPRenewal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["key"] != null && Request.QueryString["key2"] != null)
                {
                    string Encrypted_EmpCode = Request.QueryString["key"].ToString();
                    string Encrypted_ExpiringPolicyNumber = Request.QueryString["key2"].ToString();
                    string EmpCode = Encryption.DecryptText(Encrypted_EmpCode);
                    string ExpiringPolicyNumber = Encryption.DecryptText(Encrypted_ExpiringPolicyNumber);

                    DataSet dsHealthSTPDetails = new DataSet();

                    dsHealthSTPDetails = GetHealthSTPDetails(EmpCode, ExpiringPolicyNumber);

                    if (dsHealthSTPDetails != null)
                    {
                        if (dsHealthSTPDetails.Tables.Count == 2) //2 tables are mandatory to be returned from database
                        {
                            if (dsHealthSTPDetails.Tables[0].Rows.Count > 0 && dsHealthSTPDetails.Tables[1].Rows.Count > 0)
                            {
                                SetAllFields(dsHealthSTPDetails);
                            }
                        }
                        else
                        {
                            //Alert.Show("No Data Exists for the given Employee Code", "FrmSTP.aspx");
                            //return;
                            //Response.Redirect("");
                        }
                    }
                    else
                    {
                        //Alert.Show("No Data Exists for the given Employee Code", "FrmSTP.aspx");
                        //return;
                    }
                }
            }
        }

        private DataSet GetHealthSTPDetails(string EmployeeCode, string ExpiringPolicyNumber)
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_HEALTH_STP_RENEWAL_DETAILS";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "EmployeeCode", DbType.String, ParameterDirection.Input, "EmployeeCode", DataRowVersion.Current, EmployeeCode);
                db.AddParameter(dbCommand, "ExpiringPolicyNumber", DbType.String, ParameterDirection.Input, "ExpiringPolicyNumber", DataRowVersion.Current, ExpiringPolicyNumber);

                dbCommand.CommandType = CommandType.StoredProcedure;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetHealthSTPDetails Method");
            }
            return ds;
        }



        private void SetAllFields(DataSet dsHealthSTPDetails)
        {
            try
            {
                

                DataTable dtHealthSTPDetails = dsHealthSTPDetails.Tables[0];
                DataTable dtMemberDetails = dsHealthSTPDetails.Tables[1];

                lblPolicyExpiryDate.Text = Convert.ToDateTime(dtHealthSTPDetails.Rows[0]["PolicyExpiryDate"].ToString()).AddDays(-1).ToString("dd-MMM-yyyy");

                lblExpiringPolicyNumber.Text = dtHealthSTPDetails.Rows[0]["ExpiringPolicyNumber"].ToString();
                hdnExpiringPolicyNumber.Value = dtHealthSTPDetails.Rows[0]["ExpiringPolicyNumber"].ToString();

                hdnAccountNumber.Value = dtHealthSTPDetails.Rows[0]["Account_Number"].ToString();
                btnMakePayment.Text = "Debit My Salary A/c No. " + hdnAccountNumber.Value;
                lblAccountNumber.Text = dtHealthSTPDetails.Rows[0]["Account_Number"].ToString();

                lblEmployeeCode.Text = dtHealthSTPDetails.Rows[0]["EmployeeCode"].ToString();
                lblEmployeeName.Text = dtHealthSTPDetails.Rows[0]["EmployeeName"].ToString();

                lblEmpEmailId.Text = dtHealthSTPDetails.Rows[0]["EmailAddress"].ToString();
                lblPhone.Text = dtHealthSTPDetails.Rows[0]["ContactNumber"].ToString();

                lblDeductible.Text = dtHealthSTPDetails.Rows[0]["Duductible"].ToString();
                lblSumInsured.Text = dtHealthSTPDetails.Rows[0]["SumInsured"].ToString();

               

                string FloaterCombi = dtHealthSTPDetails.Rows[0]["FloaterCombi"].ToString().Replace("A", " Adult").Replace("C", " Child");
                lblFamilyFloater.Text = FloaterCombi;

                string AddressLine1 = dtHealthSTPDetails.Rows[0]["EmployeeAddressLine1"].ToString();
                string AddressLine2 = dtHealthSTPDetails.Rows[0]["EmployeeAddressLine2"].ToString();
                string Pincode = dtHealthSTPDetails.Rows[0]["Pincode"].ToString();

                // CR 142 start
                bool IsKLTEmployee = dtHealthSTPDetails.Rows[0]["IsKLTEmployee"].ToString() == "True" ? true : false;
                hdnIsKLTEmployee.Value = IsKLTEmployee ? "true" : "false";

                if (IsKLTEmployee)
                {
                    li4.Visible = false;
                    li5.Visible = false;
                    term_KLT.Visible = true;
                    term_NonKLT.Visible = false;
                }
                else
                {
                    li4.Visible = true;
                    li5.Visible = true;
                    term_KLT.Visible = false ;
                    term_NonKLT.Visible = true;
                }

                if (IsKLTEmployee)
                {
                    P1.Visible = false;
                }
                else
                {
                    P1.Visible = true;
                }

                hdnMonthlyPremiumAmount.Value = dtHealthSTPDetails.Rows[0]["MonthlyPremiumAmount"].ToString();
                hdnMonthlyGST.Value = dtHealthSTPDetails.Rows[0]["MonthlyGST"].ToString();
                hdnMonthlyTotalPayable.Value = dtHealthSTPDetails.Rows[0]["MonthlyTotalPayable"].ToString();

                lblPremiumAmount.Text = dtHealthSTPDetails.Rows[0]["YearlyPremiumAmount"].ToString();
                lblGST.Text = dtHealthSTPDetails.Rows[0]["YearlyGST"].ToString();
                lblAmountPayable.Text = dtHealthSTPDetails.Rows[0]["YearlyTotalPayable"].ToString();

                hdnYearlyPremiumAmount.Value = dtHealthSTPDetails.Rows[0]["YearlyPremiumAmount"].ToString();
                hdnYearlyGST.Value = dtHealthSTPDetails.Rows[0]["YearlyGST"].ToString();
                hdnYearlyTotalPayable.Value = dtHealthSTPDetails.Rows[0]["YearlyTotalPayable"].ToString();

                lblSelectedPremiumMessage.Text = "Yearly Amount Payable - &#x20b9; ";
                lblSelectedPremium.Text = dtHealthSTPDetails.Rows[0]["YearlyTotalPayable"].ToString();

                //lblMinMontlyAmountPayable.Text = Convert.ToDecimal(hdnMonthlyPremiumAmount.Value).ToString();
                // CR 142 End

                string strTabNav = string.Empty;
                string strTabContent = string.Empty;
                bool IsActiveTabSet = false;

                foreach (DataRow row in dtMemberDetails.Rows)
                {
                    string Title = "Mr";
                    string caseMemberGender = row["MemberGender"].ToString();
                    string ReleationshipWithEmployee = row["ReleationshipWithEmployee"].ToString();

                    if (caseMemberGender.ToUpper() == "M")
                    {
                        Title = "Mr";
                    }
                    if (caseMemberGender.ToUpper() == "F")
                    {
                        Title = "Miss";
                    }
                    if (caseMemberGender.ToUpper() == "F" && ReleationshipWithEmployee.ToUpper() == "SPOUSE")
                    {
                        Title = "Mrs";
                    }
                    if (caseMemberGender.ToUpper() == "F" && ReleationshipWithEmployee.ToUpper() == "MOTHER")
                    {
                        Title = "Mrs";
                    }

                    string couldbe_NomineeName = row["NomineeName"].ToString();
                    if (string.IsNullOrEmpty(couldbe_NomineeName.Trim()) && ReleationshipWithEmployee.ToLower() != "self")
                    {
                        couldbe_NomineeName = dtHealthSTPDetails.Rows[0]["EmployeeName"].ToString();
                    }

                    string couldbe_NomineeDOB = row["NomineeDOB"].ToString();

                    if (!string.IsNullOrEmpty(couldbe_NomineeDOB.Trim())) //if not null
                    {
                        couldbe_NomineeDOB = Convert.ToDateTime(row["NomineeDOB"].ToString()).ToString("dd/MM/yyyy");
                    }

                    if (string.IsNullOrEmpty(couldbe_NomineeDOB.Trim()) && ReleationshipWithEmployee.ToLower() != "self")
                    {
                        couldbe_NomineeDOB = Convert.ToDateTime(dtHealthSTPDetails.Rows[0]["EmployeeDOB"].ToString()).ToString("dd/MM/yyyy");
                    }
                    

                    string TabActiveClass = IsActiveTabSet == true ? "" : "active";

                    string UniqueRowId = row["UniqueRowId"].ToString();
                    strTabNav = strTabNav + "<li role='presentation' class='" + TabActiveClass + "'><a href='#" + ReleationshipWithEmployee + UniqueRowId + "' aria-controls='" + ReleationshipWithEmployee + "' role='tab' data-toggle='tab' aria-expanded='false'>" + ReleationshipWithEmployee + "</a></li>";

                    string FinalRow = "<div id='" + ReleationshipWithEmployee + UniqueRowId + "' role='tabpanel' class='tab-pane " + TabActiveClass + "' name='" + ReleationshipWithEmployee + "' customAttribut_MemberId='" + UniqueRowId + "'><div class='row' id='row" + ReleationshipWithEmployee + "'>FinalRow</div></div>";

                    string MemberTitleMr = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Title</dt><dd><select id='cboTitle" + ReleationshipWithEmployee + "' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px;'><option value='Mr' selected>Mr</option><option value='Mrs'>Mrs</option><option value='Miss'>Miss</option></select></dd></dl></div>";
                    string MemberTitleMrs = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Title</dt><dd><select id='cboTitle" + ReleationshipWithEmployee + "' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px;'><option value='Mr'>Mr</option><option value='Mrs' selected>Mrs</option><option value='Miss'>Miss</option></select></dd></dl></div>";
                    string MemberTitleMiss = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Title</dt><dd><select id='cboTitle" + ReleationshipWithEmployee + "' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px;'><option value='Mr'>Mr</option><option value='Mrs'>Mrs</option><option value='Miss' selected>Miss</option></select></dd></dl></div>";
                    
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

                    string MemberName = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Name</dt><dd>" + row["MemberName"].ToString() + "</dd></dl></div>";
                    string MemberDOB = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>DOB</dt><dd>" + row["MemberDOB"].ToString() + "</dd></dl></div>";
                    string MemberAge = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Age</dt><dd>" + row["MemberAge"].ToString() + "</dd></dl></div>";
                    string MemberGender = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Gender</dt><dd>" + row["MemberGender"].ToString() + "</dd></dl></div>";
                    string ReleationshipWithEmployee2 = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Relation</dt><dd>" + ReleationshipWithEmployee + "</dd></dl></div>";

                    string MemberHeight = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Height (in cm)</dt><dd><input id='txt" + ReleationshipWithEmployee + "Height' name='" + ReleationshipWithEmployee + " Height' value='" + row["MemberHeight"].ToString() + "' type='text' class='form-control' style='width:30%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='3' /></dd></dl></div>";
                    string MemberWeight = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Weight</dt><dd><input id='txt" + ReleationshipWithEmployee + "Weight' name='" + ReleationshipWithEmployee + " Weight' value='" + row["MemberWeight"].ToString() + "' type='text' class='form-control' style='width:60%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='3' /></dd></dl></div>";

                    string NomineeName = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Nominee Name</dt><dd><input id='txt" + ReleationshipWithEmployee + "NomineeName' name='" + ReleationshipWithEmployee + " Nominee Name' value='" + couldbe_NomineeName + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='50' /></dd></dl></div>";

                    string NomineeRelationWithProposer = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Relation With Proposer</dt><dd><select id='cboNomineeRelationWithProposer" + ReleationshipWithEmployee + "' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px;'><option value='Select' selected>Select</option><option value='Husband'>Husband</option><option value='Wife'>Wife</option><option value='Mother'>Mother</option><option value='father'>Father</option><option value='Son'>Son</option><option value='Daughter'>Daughter</option><option value='aunt'>Aunt</option><option value='Brother'>Brother</option><option value='brother in Law'>Brother In Law</option><option value='Daughter in Law'>Daughter in Law</option><option value='Friend'>Friend</option><option value='Fiance'>Fiance</option><option value='Father in law'>Father In Law</option><option value='grandfather'>Grandfather</option><option value='Grandmother'>Grandmother</option><option value='Mother in law'>Mother In Law</option><option value='Nephew'>Nephew</option><option value='niece'>Niece</option><option value='Sister'>Sister</option><option value='Sister in law'>Sister In Law</option><option value='Son in law'>Son In Law</option><option value='uncle'>Uncle</option></select></dd></dl></div>";

                    string NomineeDOB = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Nominee DOB</dt><dd><input id='txt" + ReleationshipWithEmployee + "NomineeDOB' name='" + ReleationshipWithEmployee + " Nominee DOB' value='" + couldbe_NomineeDOB + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='dd/mm/yyyy' maxlength='10' readonly /></dd></dl></div>";

                    string EmpAddress1 = string.Empty;
                    string EmpAddress2 = string.Empty;
                    string EmpPincode = string.Empty;
                    string chkIsNomineeSame = string.Empty; //AS PER DISCUSSION WITH GARIMA, THIS IS NOT REQUIRED ONWARDS DEV: 01-FEB-2018
                    string AlternateMobileNumber = string.Empty;

                    if (ReleationshipWithEmployee.ToLower() == "self")
                    {
                        EmpAddress1 = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Address Line 1</dt><dd><input id='txt" + ReleationshipWithEmployee + "AddressLine1' name='" + ReleationshipWithEmployee + "AddressLine1' value='" + AddressLine1 + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='' maxlength='50' /></dd></dl></div>";
                        EmpAddress2 = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Address Line 2</dt><dd><input id='txt" + ReleationshipWithEmployee + "AddressLine2' name='" + ReleationshipWithEmployee + "AddressLine2' value='" + AddressLine2 + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='' maxlength='50' /></dd></dl></div>";
                        EmpPincode = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Pincode</dt><dd><input id='txt" + ReleationshipWithEmployee + "Pincode' name='" + ReleationshipWithEmployee + "Pincode' value='" + Pincode + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='' maxlength='8' /></dd></dl></div>";
                        //chkIsNomineeSame = "<div class='col-lg-2' style='positon: absolute; overflow: hidden'><dl style='margin-bottom:5px'><dt style='font-size: 13px;'>Tick To Replicate Same Nominee</dt><dd><div class='checkbox c-checkbox needsclick' id='divNomineeCheckbox' style='margin-top: 3px;'><label><input name='chkNomineeIsSame' type='checkbox' id='chkNomineeIsSame' class='needsclick'><span class='fa fa-check'></span></label></div></dd></dl></div>";

                        MemberTitle = MemberTitle.Replace("col-sm-2", "col-sm-1");
                        MemberName = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Name</dt><dd>" + row["MemberName"].ToString() + "</dd></dl></div>";
                        AlternateMobileNumber = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Mobile Number</dt><dd><input id='txtAlternateMobileNumber' name='MobileNumber' value='" + lblPhone.Text + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='' maxlength='50' /></dd></dl></div>";
                        MemberDOB = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>DOB</dt><dd>" + row["MemberDOB"].ToString() + "</dd></dl></div>";
                        MemberAge = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Age</dt><dd>" + row["MemberAge"].ToString() + "</dd></dl></div>";
                        MemberGender = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Gender</dt><dd>" + row["MemberGender"].ToString() + "</dd></dl></div>";
                        ReleationshipWithEmployee2 = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Relation</dt><dd>" + ReleationshipWithEmployee + "</dd></dl></div>";

                        MemberHeight = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Height (in cm)</dt><dd><input id='txt" + ReleationshipWithEmployee + "Height' name='" + ReleationshipWithEmployee + " Height' value='" + row["MemberHeight"].ToString() + "' type='text' class='form-control' style='width:30%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='3' /></dd></dl></div>";
                        MemberWeight = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Weight</dt><dd><input id='txt" + ReleationshipWithEmployee + "Weight' name='" + ReleationshipWithEmployee + " Weight' value='" + row["MemberWeight"].ToString() + "' type='text' class='form-control' style='width:60%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='3' /></dd></dl></div>";

                        NomineeName = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Nominee Name</dt><dd><input id='txt" + ReleationshipWithEmployee + "NomineeName' name='" + ReleationshipWithEmployee + " Nominee Name' value='" + couldbe_NomineeName + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='50' /></dd></dl></div>";

                        NomineeRelationWithProposer = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Relation With Proposer</dt><dd><select id='cboNomineeRelationWithProposer" + ReleationshipWithEmployee + "' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px;'><option value='Select' selected>Select</option><option value='Husband'>Husband</option><option value='Wife'>Wife</option><option value='Mother'>Mother</option><option value='father'>Father</option><option value='Son'>Son</option><option value='Daughter'>Daughter</option><option value='aunt'>Aunt</option><option value='Brother'>Brother</option><option value='brother in Law'>Brother In Law</option><option value='Daughter in Law'>Daughter in Law</option><option value='Friend'>Friend</option><option value='Fiance'>Fiance</option><option value='Father in law'>Father In Law</option><option value='grandfather'>Grandfather</option><option value='Grandmother'>Grandmother</option><option value='Mother in law'>Mother In Law</option><option value='Nephew'>Nephew</option><option value='niece'>Niece</option><option value='Sister'>Sister</option><option value='Sister in law'>Sister In Law</option><option value='Son in law'>Son In Law</option><option value='uncle'>Uncle</option></select></dd></dl></div>";

                        NomineeDOB = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Nominee DOB</dt><dd><input id='txt" + ReleationshipWithEmployee + "NomineeDOB' name='" + ReleationshipWithEmployee + " Nominee DOB' value='" + couldbe_NomineeDOB + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' placeholder='dd/mm/yyyy' maxlength='10' readonly /></dd></dl></div>";

                        
                        strTabContent = strTabContent + FinalRow.Replace("FinalRow", MemberTitle + MemberName +  MemberDOB + AlternateMobileNumber + MemberAge + MemberGender + ReleationshipWithEmployee2 + MemberHeight + MemberWeight + EmpAddress1 + EmpAddress2 + EmpPincode + NomineeName + NomineeRelationWithProposer + NomineeDOB + chkIsNomineeSame);
                    }
                    else
                    {
                        strTabContent = strTabContent + FinalRow.Replace("FinalRow", MemberTitle + MemberName + MemberDOB + MemberAge + MemberGender + ReleationshipWithEmployee2 + MemberHeight + MemberWeight + EmpAddress1 + EmpAddress2 + EmpPincode + NomineeName + NomineeRelationWithProposer + NomineeDOB + chkIsNomineeSame);
                    }
                     IsActiveTabSet = true;
                }

                LiteralTabNavigation.Text = strTabNav;
                LiteralTabContent.Text = strTabContent;

                hdnIsEmployeeRecordPresent.Value = "1";
                UpdateSTPIsPageViewedFlag();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SetAllFields Method");
            }
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
                            cmd.CommandText = "PROC_SAVE_STP_PAGE_VIEWED_FLAG";

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


        [WebMethod]
        [ScriptMethod]
        public static string SaveEmployeePrimaryDetails(EmployeePrimaryDetails objEmployeePrimaryDetails)
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
                        cmd.CommandText = "PROC_SAVE_HEALTH_STP_PRIMARY_RENEWAL_DETAILS";

                        cmd.Parameters.AddWithValue("@SelectedPremiumDeductionType", objEmployeePrimaryDetails.SelectedPlan);
                        cmd.Parameters.AddWithValue("@FinalOneTimePasswordEnteredByEmployee", objEmployeePrimaryDetails.FinalOneTimePasswordEnteredByEmployee);
                        cmd.Parameters.AddWithValue("@EmployeeAddressLine1", objEmployeePrimaryDetails.EmployeeAddressLine1);
                        cmd.Parameters.AddWithValue("@EmployeeAddressLine2", objEmployeePrimaryDetails.EmployeeAddressLine2);
                        cmd.Parameters.AddWithValue("@Pincode", objEmployeePrimaryDetails.Pincode);
                        cmd.Parameters.AddWithValue("@EmployeeCode", objEmployeePrimaryDetails.EmployeeCode);
                        cmd.Parameters.AddWithValue("@AlternateMobileNumber", objEmployeePrimaryDetails.AlternateMobileNumber);
                        cmd.Parameters.AddWithValue("@BRMCode", objEmployeePrimaryDetails.BRMCode);
                        cmd.Parameters.AddWithValue("@ExpiringPolicyNumber", objEmployeePrimaryDetails.ExpiringPolicyNumber);

                        cmd.Connection = conn;
                        conn.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            Msg = "success";
                            bool IsYearlyOptionSelected = objEmployeePrimaryDetails.SelectedPlan.ToLower() == "yearly" ? true : false;
                            SendAcknowledgementEmail(objEmployeePrimaryDetails.EmployeeEmailId, objEmployeePrimaryDetails.EmployeeCode, objEmployeePrimaryDetails.EmployeeName, objEmployeePrimaryDetails.SelectedPremium, objEmployeePrimaryDetails.AccountNumber, objEmployeePrimaryDetails.IsKLTEmployee, IsYearlyOptionSelected);
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Msg = "error";
                ExceptionUtility.LogException(ex, "SaveMemberDetails FrmSTP.aspx");
            }
            return Msg;
        }

        [WebMethod]
        [ScriptMethod]
        public static string SaveMemberDetails(List<MemberDetails> listMemberDetails)
        {
            string Msg = string.Empty;
            try
            {
                foreach (MemberDetails objMemberDetail in listMemberDetails)
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "PROC_SAVE_HEALTH_STP_RENEWAL_MEMBER_DETAILS";

                            cmd.Parameters.AddWithValue("@UniqueRowId", objMemberDetail.UniqueRowId);
                            cmd.Parameters.AddWithValue("@ReleationshipWithEmployee", objMemberDetail.ReleationshipWithEmployee);
                            cmd.Parameters.AddWithValue("@Height", objMemberDetail.Height);
                            cmd.Parameters.AddWithValue("@Weight", objMemberDetail.Weight);
                            cmd.Parameters.AddWithValue("@NomineeName", objMemberDetail.NomineeName);
                            DateTime dateNomineeDOB = DateTime.ParseExact(objMemberDetail.NomineeDOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            cmd.Parameters.AddWithValue("@NomineeDOB", dateNomineeDOB); // Convert.ToDateTime(objMemberDetail.NomineeDOB).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@EmployeeCode", objMemberDetail.EmployeeCode);
                            cmd.Parameters.AddWithValue("@Title", objMemberDetail.MemberTitle);
                            cmd.Parameters.AddWithValue("@NomineeRelationWithProposer", objMemberDetail.MemberNomineeRelationWithProposer);
                            cmd.Parameters.AddWithValue("@ExpiringPolicyNumber", objMemberDetail.ExpiringPolicyNumber);

                            cmd.Connection = conn;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                Msg = "success";
            }
            catch (Exception ex)
            {
                Msg = "Error";
                ExceptionUtility.LogException(ex, "SaveMemberDetails FrmSTP.aspx");
            }
            return Msg;
        }

        [WebMethod]
        [ScriptMethod]
        public static string ValidateOTP(string OTPNumber, string EmployeeCode)
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
                    SendOTPEmail(EmailId, EmployeeCode, GeneratedOTP, EmployeeName);

                    if (MobileNumber.Length == 10)
                    {
                        bool IsSendSMSSuccess = SendSMS(GeneratedOTP, MobileNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                GeneratedOTP = "";
                ExceptionUtility.LogException(ex, "Error in GenerateOTP on FrmSTP Page");
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
                    //  string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
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

                    // string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
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
                ExceptionUtility.LogException(ex, "Error in SendSMS on FrmSTP Page");
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
                        cmd.CommandText = "PROC_SAVE_OTP_FOR_HEALTH_STP";

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
                ExceptionUtility.LogException(ex, "SaveOTP, FrmSTP Page");
            }
            return IsOTPSavedToDB;
        }

        private static string SendOTPEmail(string ToEmailId, string EmployeeCode, string OTPNumber, string EmployeeName)
        {
            string smtp_DefaultCCMailId = ConfigurationManager.AppSettings["smtp_DefaultCCMailId"].ToString();
            string strMessage = string.Empty;

            string smtp_Username = ConfigurationManager.AppSettings["smtp_Username"].ToString();
            string smtp_Password = ConfigurationManager.AppSettings["smtp_Password"].ToString();
            string smtp_Host = ConfigurationManager.AppSettings["smtp_Host"].ToString();
            string smtp_FromMailId = "noreply@kotak.com";
            string strPath = string.Empty;
            string MailBody = string.Empty;

            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = smtp_Host; //"192.168.201.61"; //"kgirelay.kgi.kotakgroup.com";
                                         //client.EnableSsl = true;
                client.Timeout = 3600000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(smtp_Username, smtp_Password);


                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBoby_STP_OTP.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#EmployeeName#", EmployeeName);
                MailBody = MailBody.Replace("#ReplaceText#", "One Time Password For Kotak Super Top Up Plan is " + OTPNumber);


                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(smtp_FromMailId);
                mm.Subject = "OTP For Kotak Super Top-Up Plan";
                mm.Body = MailBody;
                mm.IsBodyHtml = true;

                mm.To.Add(ToEmailId);

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                strMessage = "success";
            }
            catch (Exception ex)
            {
                strMessage = "error while sending OTP on mail";
                ExceptionUtility.LogException(ex, "SendEmail Method");
            }

            return strMessage;
        }

        private static string SendAcknowledgementEmail(string ToEmailId, string EmployeeCode, string EmployeeName, string Amount, string AccountNumber, bool IsKLTEmployee, bool IsYearlyOptionSelected)
        {
            string smtp_DefaultCCMailId = ConfigurationManager.AppSettings["smtp_DefaultCCMailId"].ToString();
            string strMessage = string.Empty;

            string smtp_Username = ConfigurationManager.AppSettings["smtp_Username"].ToString();
            string smtp_Password = ConfigurationManager.AppSettings["smtp_Password"].ToString();
            string smtp_Host = ConfigurationManager.AppSettings["smtp_Host"].ToString();
            string smtp_FromMailId = "noreply@kotak.com";
            string strPath = string.Empty;
            string MailBody = string.Empty;

            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = smtp_Host; //"192.168.201.61"; //"kgirelay.kgi.kotakgroup.com";
                                         //client.EnableSsl = true;
                client.Timeout = 3600000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(smtp_Username, smtp_Password);


                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBoby_STP_OTP.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#EmployeeName#", EmployeeName);

                if (IsYearlyOptionSelected)
                {
                    MailBody = MailBody.Replace("#ReplaceText#", "Based on the consent provided by you, an amount of Rs. #Yearly_EMI# will be debited from your salary account " + AccountNumber + " within the next 30 days. <br><br>Your policy will commence the same day of salary account debit. For example, if your account is debited on 31st Jul 2018, your policy will commence from 31st Jul 2018.<br>");
                    MailBody = MailBody.Replace("#Yearly_EMI#", Amount);
                }
                else
                {
                    MailBody = MailBody.Replace("#ReplaceText#", "Based on the consent provided by you, an EMI of Rs. #Monthly_EMI# will be debited from your salary account " + AccountNumber + " within the next 30 days. This EMI debit will continue every month for the next 12 months, until the expiry of the policy term. <br><br>Your policy will commence the same day of salary account debit. For example, if your account is debited on 31st Jul 2018, your policy will commence from 31st Jul 2018.<br>");
                    MailBody = MailBody.Replace("#Monthly_EMI#", Amount);
                }


                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(smtp_FromMailId);
                mm.Subject = "Thank You for selecting Kotak Super Top-Up Plan";
                mm.Body = MailBody;
                mm.IsBodyHtml = true;

                mm.To.Add(ToEmailId);

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                strMessage = "success";
            }
            catch (Exception ex)
            {
                strMessage = "error while sending Acknowledgement Email";
                ExceptionUtility.LogException(ex, "SendAcknowledgementEmail Method, FrmSTP Page");
            }

            return strMessage;
        }
    }
}