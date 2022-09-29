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
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using CCA.Util;
using System.Text.RegularExpressions;
using Winnovative;
using System.Net.Mime;

namespace PrjPASS
{
    public class KGHAMemberDetails
    {
        public string ProposalNo { get; set; }
        public string MemberName { get; set; }
        public string MemberDOB { get; set; }
        public string MemberAge { get; set; }
        public string MemberGender { get; set; }
        public string MemberOccupation { get; set; }
        public string MartialStatus { get; set; }
        public string MemberRelationship { get; set; }
        public string NomineeName { get; set; }
        public string NomineeDOB { get; set; }
        public string NomineeReslationShip { get; set; }
         public string Relationship { get; set; }
    }

    public class ProposerDetails
    {
        public string Title { get; set; }
        public string SelectedPlan { get; set; }
        public string FinalOneTimePasswordEnteredByEmployee { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Mobileno { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string SelectedPremium { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelationship { get; set; }
        public string NomineeDOB { get; set; }
        public string ProposalNo { get; set; }
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }
    }
    public class KGHASelfDetails
    {
        public string ProposalNo { get; set; }
        public string MemberName { get; set; }
        public string MemberDOB { get; set; }

        public string MemberAge { get; set; }
        public string Relation { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelationship { get; set; }
        public string NomineeDOB { get; set; }
        public string gender { get; set; }

    }
    public partial class frmKGHARegistrationdetails : System.Web.UI.Page
    {
        


        public string action1 = string.Empty;
        public string Proposalno = string.Empty;
        //end added for payment entry 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["key"] != null)
                {
                    string Encrypted_Proposalno = Request.QueryString["key"].ToString();
                    Proposalno = Encryption.DecryptText(Encrypted_Proposalno);
                    hiddenProposalNo.Value = Proposalno;
                    DataSet dsKGHAPaymentDetails = new DataSet();

                    dsKGHAPaymentDetails= GetKGHAPaymentDetails(Proposalno);
                    DataSet dsKGHAProposerDetails = new DataSet();

                    dsKGHAProposerDetails = GetKGHAProposerDetails(Proposalno);
                    if (dsKGHAPaymentDetails != null)
                    {
                        if (dsKGHAPaymentDetails.Tables.Count == 1)
                        {
                            if (dsKGHAPaymentDetails.Tables[0].Rows.Count > 0)
                            {

                                DataTable dtKGHAPaymentDetails = dsKGHAPaymentDetails.Tables[0];
                                string IsinProgress = dtKGHAPaymentDetails.Rows[0]["IsinProgress"].ToString().ToLower();
                                string orderstatus = dtKGHAPaymentDetails.Rows[0]["order_status"].ToString().ToLower();
                                if (IsinProgress=="false")
                                {
                                    hdnIsEmployeeRecordPresent.Value = "2";
                                }
                                else if (IsinProgress == "true" && orderstatus == "success")
                                {
                                    hdnIsEmployeeRecordPresent.Value = "3";
                                }
                                else if (IsinProgress == "true" && orderstatus == "failure")
                                {
                                    hdnIsEmployeeRecordPresent.Value = "4";
                                }
                            }
                            else
                            {
                                GetProposerDetails(dsKGHAProposerDetails);
                            }
                        }
                        else
                        {
                            GetProposerDetails(dsKGHAProposerDetails);
                        }
                    }
                else
                    {
                        GetProposerDetails(dsKGHAProposerDetails);

                    }

                   
                }
            }
        }
        private void  GetProposerDetails(DataSet dsKGHAProposerDetails)
        {
           
            try
            {
                if (dsKGHAProposerDetails != null)
                {
                    if (dsKGHAProposerDetails.Tables.Count == 1)
                    {
                        if (dsKGHAProposerDetails.Tables[0].Rows.Count > 0)
                        {
                           
                            SetAllFields(dsKGHAProposerDetails);

                        }
                        
                    }

                }
            }
            
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetKGHAPaymentDetails Method");
            }
       
        }
        private DataSet GetKGHAPaymentDetails(string Proposalno)
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_KGHA_PAYMENT_DETAILS";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "ProposalNo", DbType.String, ParameterDirection.Input, "ProposalNo", DataRowVersion.Current, Proposalno);

                dbCommand.CommandType = CommandType.StoredProcedure;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetKGHAPaymentDetails Method");
            }
            return ds;
        }
        private DataSet GetKGHAProposerDetails(string Proposalno)
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_KGHA_PROPOSER_DETAILS";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "ProposalNo", DbType.String, ParameterDirection.Input, "ProposalNo", DataRowVersion.Current, Proposalno);

                dbCommand.CommandType = CommandType.StoredProcedure;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetKGHAProposerDetails Method");
            }
            return ds;
        }



        private void SetAllFields(DataSet dsKGHAProposerDetails)
        {
            try
            {
                DataTable dtKGHAProposerDetails = dsKGHAProposerDetails.Tables[0];
                string UniqueID = dtKGHAProposerDetails.Rows[0]["UniqueRowId"].ToString();
                string VerifiedCRN = dtKGHAProposerDetails.Rows[0]["VerifiedCRN"].ToString();
                string ProposerName = dtKGHAProposerDetails.Rows[0]["ProposerName"].ToString();
                string Title = dtKGHAProposerDetails.Rows[0]["Title"].ToString();
                string DOB = Convert.ToDateTime(dtKGHAProposerDetails.Rows[0]["MemberDOB"]).ToString("MM/dd/yyyy");
                string Gender = dtKGHAProposerDetails.Rows[0]["Gender"].ToString();
                string Relation = dtKGHAProposerDetails.Rows[0]["Relation"].ToString();
                string ContactNumber = dtKGHAProposerDetails.Rows[0]["ContactNumber"].ToString();
                string EmailAddress = dtKGHAProposerDetails.Rows[0]["EmailAddress"].ToString();
                string Occupation = dtKGHAProposerDetails.Rows[0]["Occupation"].ToString();
                string MaritalStatus = dtKGHAProposerDetails.Rows[0]["MaritalStatus"].ToString();
                string city = dtKGHAProposerDetails.Rows[0]["city"].ToString();
                string State = dtKGHAProposerDetails.Rows[0]["State"].ToString();
                string PinCode = dtKGHAProposerDetails.Rows[0]["PinCode"].ToString();
                string EmployeeID = dtKGHAProposerDetails.Rows[0]["EmployeeID"].ToString();
                string RowCreatedOn = dtKGHAProposerDetails.Rows[0]["RowCreatedOn"].ToString();
                string NomineeName = dtKGHAProposerDetails.Rows[0]["NomineeName"].ToString();
                string NomineeRelationship = dtKGHAProposerDetails.Rows[0]["NomineeRelationship"].ToString();
                string NomineeDOB = Convert.ToDateTime(dtKGHAProposerDetails.Rows[0]["NomineeDOB"]).ToString("MM/dd/yyyy");
                string AddressLine1 = dtKGHAProposerDetails.Rows[0]["AddressLine1"].ToString();
                string AddressLine2 = dtKGHAProposerDetails.Rows[0]["AddressLine2"].ToString();
                string Age = dtKGHAProposerDetails.Rows[0]["Age"].ToString();
                string empcode = dtKGHAProposerDetails.Rows[0]["EmployeeID"].ToString();
                hiddenempcode.Value = empcode;

                hiddenMemberName.Value = ProposerName;
                hiddenMemberAge.Value = Age;
                hiddenMemberDOB.Value = DOB;
                Hiddenmartialstatus.Value = MaritalStatus;
                hiddenNomineeName.Value = NomineeName;
                hiddenNomineeRelationship.Value = NomineeRelationship;
                hiddenRelation.Value = Relation;
                Hiddenmartialstatus.Value = MaritalStatus;
                Hiddenoccupation.Value = Occupation;
                hiddenNomineeDob.Value = NomineeDOB.Contains("1900")?"": NomineeDOB;
                HiddenGender.Value = Gender;

                lblEmployeeName.Text = ProposerName;
                lblVerifiedCRN.Text = VerifiedCRN;
                lblEmpEmailId.Text = EmailAddress;
                lblPhone.Text = ContactNumber;

                string strTabNav = string.Empty;
                string strTabContent = string.Empty;
                bool IsActiveTabSet = false;
                if (Gender.ToUpper() == "M")
                {
                    Title = "Mr";
                }
                else if (Gender.ToUpper() == "F")
                {
                    Title = "Mrs";
                }

                string MemberTitleMr = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Title</dt><dd><select id='cboTitle' disabled='disabled' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px;'><option value='Mr.' selected>Mr.</option></select></dd></dl></div>";
                string MemberTitleMrs = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Title</dt><dd><select id='cboTitle' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px;'><option value='Mrs.' selected>Mrs.</option><option value='Ms.'>Ms.</option></select></dd></dl></div>";
                string MemberTitle = string.Empty;

                if (Title == "Mr")
                {
                    MemberTitle = MemberTitleMr;
                }
                else if (Title == "Mrs")
                {
                    MemberTitle = MemberTitleMrs;
                }


                string MemberName = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Name</dt><dd><input id='txtmembername' name='txtmembername' value='" + ProposerName + "' type='text' class='form-control' readonly='true' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='90' /></dd></dl></div>";
                string MemberDOB = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>DOB</dt><dd><input id='txtmemberdob' name='txtmemberdob' disabled='disabled'  value='" + DOB + "' type='text'  readonly='true' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='15' /></dd></dl></div>";
                string MemberContactNo = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Mobile No</dt><dd><input id='txtmembermobileno' name='txtmembermobileno' onkeypress='return IsNumeric(event);' ondrop='return false;' onpaste='return false;'  value='" + ContactNumber + "' type='tel' pattern='[0-9]*' maxlength='10' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='12' /></dd></dl></div>";
                string MemberAge = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Age</dt><dd><input id='txtmemberage' name='txtmemberage'  value='" + Age + "' readonly='true' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='15' /></dd></dl></div>";
                string MemberGender = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Gender</dt><dd><input id='txtmembergender' name='txtmembergender'  readonly='true' value='" + Gender + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='15' /></dd></dl></div>";
                string MemberRelation = "<div class='col-sm-3'><dl style='margin-bottom:5px'><dt>Relationship with the Proposer</dt><dd><input id='txtmemberself' name='txtmemberself'  readonly='true' value='Self' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='15' /></dd></dl></div>";
                string MemberOccupation = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>occupation</dt><dd><input id='txtmemberoccupation' name='txtmemberoccupation'  readonly='true' value='" + Occupation + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='15' /></dd></dl></div>";
                string MemberMartialstatus = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Martial Status</dt><dd><input id='txtmembermartialstatus' name='txtmembermartialstatus'  readonly='true' value='" + MaritalStatus + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='15' /></dd></dl></div>";

                string MemberEmailid = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Email ID</dt><dd><input id='txtmemberemailid' name='txtmemberemailid'  readonly='true' value='" + EmailAddress + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='15' /></dd></dl></div>";
                string MemberVerifiedCrn = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Verified CRN</dt><dd><input id='txtmemberverifiedcrn' name='txtmemberverifiedcrn' value='" + VerifiedCRN + "'  readonly='true' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' maxlength='15' /></dd></dl></div>";
                string MemberAddress1 = "<div class='col-sm-3'><dl style='margin-bottom:5px'><dt>Address Line 1</dt><dd><input id='txtmemberaddress1' name='txtmemberaddress1' value='" + AddressLine1 + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' /></dd></dl></div>";
                string MemberAddress2 = "<div class='col-sm-3'><dl style='margin-bottom:5px'><dt>Address Line 2</dt><dd><input id='txtmemberaddress2' name='txtmemberaddress2' value='" + AddressLine2 + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' /></dd></dl></div>";
                string MemberCity = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>City</dt><dd><input id='txtmembercity' name='txtmembercity' value='" + city + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' /></dd></dl></div>";
                string MemberState = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>State</dt><dd><input id='txtmemberstate' name='txtmemberstate' value='" + State + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' /></dd></dl></div>";
                string MemberPincode = "<div class='col-sm-1'><dl style='margin-bottom:5px'><dt>Pin Code</dt><dd><input id='txtmemberpincode' name='txtmemberpincode' value='" + PinCode + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' /></dd></dl></div>";
                string MemberNomineeName = string.Empty;
                if (NomineeName.Trim() != "")
                {
                    MemberNomineeName = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Nominee Name</dt><dd><input id='txtmemberNomineeName' name='txtmemberNomineeName' value='" + NomineeName + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' /></dd></dl></div>";
                }
                string MemberNomineeDOB = string.Empty;
                NomineeDOB = NomineeDOB.Contains("1900") ? "" : NomineeDOB;
                if (NomineeDOB!="")
                {
                    MemberNomineeDOB = "<div class='col-sm-2'><dl style='margin-bottom:5px'><dt>Nominee Date of Birth</dt><dd><input id='txtmemberNomineeDOB' name='txtmemberNomineeDOB' value='" + NomineeDOB + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' /></dd></dl></div>";
                }
                string MemberNomineeRelation = string.Empty;
                if (NomineeRelationship.Trim() != "")
                {
                    string fatherselected = NomineeRelationship == "Father" ? "Selected" : "";
                    string Motherselected = NomineeRelationship == "Mother" ? "Selected" : "";
                    string Sisterselected = NomineeRelationship == "Sister" ? "Selected" : "";
                    string Sonrselected = NomineeRelationship == "Son" ? "Selected" : "";
                    string Daughterselected = NomineeRelationship == "Daughter" ? "Selected" : "";
                    string Spouseselected = NomineeRelationship == "Spouse" ? "Selected" : "";
                    string Brotherselected = NomineeRelationship == "Brother" ? "Selected" : "";
                    string Father_In_Lawselected = NomineeRelationship == "Father-In-Law" ? "Selected" : "";
                    string Mother_In_Law = NomineeRelationship == "Mother-In-Law" ? "Selected" : "";
                    MemberNomineeRelation = "<div class='col-sm-3'><dl style='margin-bottom:5px'><dt>Relationship with Insured</dt><dd><select id='ddnomineerelationship' name='ddnomineerelationship' class='form-control' style='width: 100%;height:25px;padding: 2px 7px;font-size: 13px;'><option value='Father' "+fatherselected+">Father</option><option value='Mother' "+Motherselected+">Mother</option><option value='Sister' "+Sisterselected+">Sister</option><option value='Son' "+Sonrselected+">Son</option><option value='Daughter' "+Daughterselected+">Daughter</option><option value='Spouse' "+Spouseselected+">Spouse</option><option value='Brother' "+Brotherselected+">Brother</option><option value='Father-In-Law' "+Father_In_Lawselected+">Father-In-Law</option> <option value='Mother-In-Law' "+Mother_In_Law+">Mother-In-Law</option></select></dd></dl></div>";
                }
                //<input id='txtmemberNomineeRelationship' name='txtmemberNomineeRelationship' value='" + NomineeRelationship + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' />

                string MemberProposalNo = "<div class='col-sm-3'><dl style='margin-bottom:5px'><dt>Proposal No</dt><dd><input id='txtmemberProposalno' name='txtmemberProposalno'  readonly='true' value='" + Proposalno + "' type='text' class='form-control' style='width:100%;height:25px;padding: 2px 7px;font-size: 13px;' /></dd></dl></div>";

                strTabNav = strTabNav + "<li role='presentation' class='Active'><a href='#Proposer' aria-controls='Proposer' role='tab' data-toggle='tab' aria-expanded='false'>Self</a></li>";
                string FinalRow = "<div id='Proposer' role='tabpanel' class='tab-pane active' name='Proposer' customAttribut_MemberId='Proposer" + UniqueID + "'><div class='row' id='row" + UniqueID + "'>FinalRow</div></div>";

                strTabContent = strTabContent + FinalRow.Replace("FinalRow", MemberTitle + MemberName + MemberDOB + MemberContactNo + MemberAge + MemberGender + MemberRelation + MemberEmailid
                    + MemberVerifiedCrn + MemberAddress1 + MemberAddress2 + MemberCity + MemberState + MemberPincode + MemberNomineeName + MemberNomineeRelation + MemberOccupation + MemberMartialstatus + MemberNomineeDOB +
                   MemberProposalNo);

                
                LiteralTabNavigation.Text = strTabNav;
                LiteralTabContent.Text = strTabContent;

                hdnIsEmployeeRecordPresent.Value = "1";
                //UpdateSTPIsPageViewedFlag();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SetAllFields Method");
            }
        }


       


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string SaveEmployeePrimaryDetails(ProposerDetails objProposerPrimaryDetails)
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
                        cmd.CommandText = "PROC_SAVE_KGHA_PRIMARY_DETAILS";
                        cmd.Parameters.AddWithValue("@Title", objProposerPrimaryDetails.Title);
                        cmd.Parameters.AddWithValue("@ProposalNo", objProposerPrimaryDetails.ProposalNo);
                        cmd.Parameters.AddWithValue("@SelectedPlan", objProposerPrimaryDetails.SelectedPlan);
                        cmd.Parameters.AddWithValue("@FinalOneTimePasswordEnteredByEmployee", objProposerPrimaryDetails.FinalOneTimePasswordEnteredByEmployee);
                        cmd.Parameters.AddWithValue("@AddressLine1", objProposerPrimaryDetails.AddressLine1);
                        cmd.Parameters.AddWithValue("@AddressLine2", objProposerPrimaryDetails.AddressLine2);
                        cmd.Parameters.AddWithValue("@Mobileno", objProposerPrimaryDetails.Mobileno);
                        cmd.Parameters.AddWithValue("@city", objProposerPrimaryDetails.City);
                        cmd.Parameters.AddWithValue("@State", objProposerPrimaryDetails.State);
                        cmd.Parameters.AddWithValue("@Pincode", objProposerPrimaryDetails.Pincode);
                        cmd.Parameters.AddWithValue("@SelectedPremium", objProposerPrimaryDetails.SelectedPremium);
                        cmd.Parameters.AddWithValue("@nomineename", objProposerPrimaryDetails.NomineeName);
                        cmd.Parameters.AddWithValue("@nomineerelationship", objProposerPrimaryDetails.NomineeRelationship);
                        // DateTime dateNomineeDOB = Convert.ToDateTime(objProposerPrimaryDetails.NomineeDOB);
                        if (objProposerPrimaryDetails.NomineeDOB.Trim() != "")
                        {
                            DateTime dateNomineeDOB = Convert.ToDateTime(objProposerPrimaryDetails.NomineeDOB);
                            cmd.Parameters.AddWithValue("@nomineedob", dateNomineeDOB.ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@nomineedob", objProposerPrimaryDetails.NomineeDOB);
                        }
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
                ExceptionUtility.LogException(ex, "SaveMemberDetails frmKGHARegistrationdetails.aspx");
            }
            return Msg;
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string SaveMemberDetails(List<KGHAMemberDetails> listMemberDetails)
        {
            string Msg = string.Empty;
            string paymentredirect = string.Empty;
            string ProposalNo = string.Empty;
            try
            {
                foreach (KGHAMemberDetails KGHAMemberDetails in listMemberDetails)
                {
                    ProposalNo = KGHAMemberDetails.ProposalNo;
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "PROC_SAVE_KGHA_MEMBER_DETAILS";

                            cmd.Parameters.AddWithValue("@ProposalNo", KGHAMemberDetails.ProposalNo);
                            cmd.Parameters.AddWithValue("@MemberName", KGHAMemberDetails.MemberName);
                            if (KGHAMemberDetails.MemberDOB.Trim() != "")
                            {
                                DateTime dateMemberDOB = Convert.ToDateTime(KGHAMemberDetails.MemberDOB);
                                cmd.Parameters.AddWithValue("@MemberDOB", dateMemberDOB.ToString("MM/dd/yyyy"));

                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@MemberDOB", KGHAMemberDetails.MemberDOB);
                            }
                            cmd.Parameters.AddWithValue("@MemberAge", KGHAMemberDetails.MemberAge);
                            cmd.Parameters.AddWithValue("@MemberGender", KGHAMemberDetails.MemberGender);
                            cmd.Parameters.AddWithValue("@MemberOccupation", KGHAMemberDetails.MemberOccupation);
                            cmd.Parameters.AddWithValue("@MartialStatus", KGHAMemberDetails.MartialStatus);
                            if (KGHAMemberDetails.NomineeDOB.Trim() != "")
                            {
                                DateTime dateNomineeDOB = Convert.ToDateTime(KGHAMemberDetails.NomineeDOB);
                                cmd.Parameters.AddWithValue("@NomineeDOB", dateNomineeDOB.ToString("MM/dd/yyyy"));

                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@NomineeDOB", KGHAMemberDetails.MemberDOB);
                            }
                            //  DateTime dateNomineeDOB = DateTime.ParseExact(KGHAMemberDetails.NomineeDOB, "dd/MM/yyyy", null);
                            // cmd.Parameters.AddWithValue("@NomineeDOB", dateNomineeDOB); 
                            cmd.Parameters.AddWithValue("@MemberRelationship", KGHAMemberDetails.MemberRelationship);
                            cmd.Parameters.AddWithValue("@NomineeName", KGHAMemberDetails.NomineeName);
                            cmd.Parameters.AddWithValue("@NomineeRelationship", KGHAMemberDetails.NomineeReslationShip);
                            cmd.Parameters.AddWithValue("@Relationship", KGHAMemberDetails.Relationship);
                            
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
                ExceptionUtility.LogException(ex, "SaveMemberDetails frmKGHARegistrationdetails.aspx");
            }
            return Msg;
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string ValidateOTP(string OTPNumber, string EmployeeCode, String ProposalNo)
        {
            string Msg = string.Empty;
            try
            {
                if (HttpContext.Current.Session["OTPNumber"] != null && OTPNumber.Trim().Length > 0)
                {
                    if (OTPNumber == HttpContext.Current.Session["OTPNumber"].ToString())
                    {
                        Msg = "success";
                        SaveOTP(EmployeeCode, OTPNumber, "Update", ProposalNo);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Error in ValidateOTP on frmKGHARegistrationdetails Page");
            }
             return Msg;
           // return "success";
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string GenerateOTPNew(string MobileNumber, string EmailId, string EmployeeCode, string EmployeeName, string ProposalNo)
        {
            string GeneratedOTP = string.Empty;
            try
            {
                Random r = new Random();
                GeneratedOTP = r.Next(100000, 999999).ToString();

                bool IsOTPSavedToDB = SaveOTP(EmployeeCode.Trim(), GeneratedOTP, "Insert", ProposalNo);
                if (IsOTPSavedToDB)
                {
                    HttpContext.Current.Session["OTPNumber"] = GeneratedOTP;

                   
                    string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    Regex emailreg = new Regex(strRegex);
                    if (MobileNumber.Length == 10)
                    {
                        bool IsSendSMSSuccess = SendSMS(GeneratedOTP, MobileNumber);
                        clsAppLogs.LogEvent("OTP " + GeneratedOTP + "sent to Mobile number " + MobileNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                    }
                    if (emailreg.IsMatch(EmailId.Trim()))
                    {
                        bool IsSendEmailSuccess = SendOTPEmail(EmailId,  EmployeeCode, GeneratedOTP,  EmployeeName);
                        clsAppLogs.LogEvent("OTP " + GeneratedOTP + "OTP GENERATED FOR to EMAIL " + EmailId + "  " + DateTime.Now.ToString() + System.Environment.NewLine);

                    }
                }
            }
            catch (Exception ex)
            {
                GeneratedOTP = "";
                ExceptionUtility.LogException(ex, "Error in GenerateOTP on frmKGHARegistrationdetails Page");
            }
            return string.IsNullOrEmpty(GeneratedOTP) ? "" : "success";
        }
      
        protected  void btnConfirmPayment_click(object sender, EventArgs e)
        {
         
            string Msg = string.Empty;
            string action1 = string.Empty;
            string SelectedPremium = string.Empty;
            string MemberName = string.Empty;
            string AddressLine1 = string.Empty;
            string AddressLine2 = string.Empty;
            string City = string.Empty;
            string State = string.Empty;
            string Pincode = string.Empty;
            string Mobileno = string.Empty;
            string MemberEmail = string.Empty;
            string Proposalno = hiddenProposalNo.Value;

            string premiumamount = string.Empty;
            string Tid = Regex.Replace(Proposalno, "[^0-9.]", "");
            //string Tid = randomrtid();
            try
            {
                DataSet dsKGHAProposerDetails = new DataSet();

                dsKGHAProposerDetails = GetKGHAProposerDetails(Proposalno);
                if (dsKGHAProposerDetails != null)
                {
                    if (dsKGHAProposerDetails.Tables.Count == 1)
                    {
                        if (dsKGHAProposerDetails.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtKGHAProposerDetails = dsKGHAProposerDetails.Tables[0];
                             MemberName = dtKGHAProposerDetails.Rows[0]["ProposerName"].ToString();
                             AddressLine1 = dtKGHAProposerDetails.Rows[0]["AddressLine1"].ToString();
                             AddressLine2 = dtKGHAProposerDetails.Rows[0]["AddressLine2"].ToString();
                             City = dtKGHAProposerDetails.Rows[0]["city"].ToString();
                             State = dtKGHAProposerDetails.Rows[0]["State"].ToString();
                             Pincode = dtKGHAProposerDetails.Rows[0]["PinCode"].ToString();
                             Mobileno = dtKGHAProposerDetails.Rows[0]["ContactNumber"].ToString();
                             MemberEmail = dtKGHAProposerDetails.Rows[0]["EmailAddress"].ToString();
                            premiumamount= dtKGHAProposerDetails.Rows[0]["SelectedPremium"].ToString();


                        }
                          
                    }
                    
                }
                
             



                System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in hash table for data post
                //string AmountForm = Convert.ToDecimal(premiumamount).ToString("g29");// eliminating trailing zeros
                string AmountForm = Convert.ToDecimal("1").ToString("g29");// eliminating trailing zeros

                data.Add("tid", Tid);
                data.Add("merchant_id", ConfigurationManager.AppSettings["KGHAMerchentId"].ToString());
                data.Add("order_id", Proposalno);
                data.Add("amount", AmountForm);
                data.Add("currency", "INR");
                data.Add("redirect_url", ConfigurationManager.AppSettings["KGHAsurl"]);
                data.Add("cancel_url", ConfigurationManager.AppSettings["KGHAfurl"]);
                data.Add("billing_name", MemberName);
                data.Add("billing_address", AddressLine1 + " " + AddressLine2);
                data.Add("billing_city",City);
                data.Add("billing_state", State);
                data.Add("billing_zip", Pincode);
                data.Add("billing_country", "India");
                data.Add("billing_tel",Mobileno);
                data.Add("billing_email", MemberEmail);
                data.Add("delivery_name", MemberName);
                data.Add("delivery_address",AddressLine1 + " " + AddressLine2);
                data.Add("delivery_city",City);
                data.Add("delivery_state",State);
                data.Add("delivery_zip",Pincode);
                data.Add("delivery_country", "India");
                data.Add("delivery_tel", Mobileno);
                data.Add("merchant_param1", "additional Info.");
                data.Add("merchant_param2", "additional Info.");
                data.Add("merchant_param3", "CP");
                data.Add("merchant_param4", "additional Info.");
                data.Add("merchant_param5", "additional Info.");
                data.Add("promo_code", "");
                data.Add("customer_identifier", "");

                CCACrypto ccaCrypto = new CCACrypto();
                string ccaRequest = string.Empty;
                string workingKey = ConfigurationManager.AppSettings["KGHAWorkingKey"].ToString();
                string accessCode = ConfigurationManager.AppSettings["KGHAAccessCode"].ToString();
                foreach (System.Collections.DictionaryEntry key in data)
                {
                    ccaRequest = ccaRequest + key.Key + "=" + key.Value + "&";
                }
                string strEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);

                data.Add("encRequest", strEncRequest);
                data.Add("access_code", accessCode);



                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                    con.Open();

                    SqlCommand cmdCheck = new SqlCommand("PROC_CCAVN_KGHA_SAVE_REQUEST_DETAILS", con);
                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.Parameters.AddWithValue("@tid", Tid);
                    cmdCheck.Parameters.AddWithValue("@merchant_id", ConfigurationManager.AppSettings["KGHAMerchentId"].ToString());
                    cmdCheck.Parameters.AddWithValue("@order_id", Proposalno);
                    cmdCheck.Parameters.AddWithValue("@amount", AmountForm);
                    cmdCheck.Parameters.AddWithValue("@currency", "INR");
                    cmdCheck.Parameters.AddWithValue("@redirect_url", ConfigurationManager.AppSettings["KGHAsurl"]);
                    cmdCheck.Parameters.AddWithValue("@cancel_url", ConfigurationManager.AppSettings["KGHAfurl"]);
                    cmdCheck.Parameters.AddWithValue("@billing_name", "Kotak Mahindra General Insurance Company Ltd");
                    cmdCheck.Parameters.AddWithValue("@billing_address", "27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East");
                    cmdCheck.Parameters.AddWithValue("@billing_city", "Mumbai");
                    cmdCheck.Parameters.AddWithValue("@billing_state", "Maharashtra");
                    cmdCheck.Parameters.AddWithValue("@billing_zip", "400051");
                    cmdCheck.Parameters.AddWithValue("@billing_country", "India");
                    cmdCheck.Parameters.AddWithValue("@billing_tel", "1800 266 4545");
                    cmdCheck.Parameters.AddWithValue("@billing_email", "care@kotak.com");
                    cmdCheck.Parameters.AddWithValue("@delivery_name", MemberName);
                    cmdCheck.Parameters.AddWithValue("@delivery_address", AddressLine1 + " " + AddressLine2);
                    cmdCheck.Parameters.AddWithValue("@delivery_city", City);
                    cmdCheck.Parameters.AddWithValue("@delivery_state", State);
                    cmdCheck.Parameters.AddWithValue("@delivery_zip", Pincode);
                    cmdCheck.Parameters.AddWithValue("@delivery_country", "India");
                    cmdCheck.Parameters.AddWithValue("@delivery_tel", Mobileno);
                    cmdCheck.Parameters.AddWithValue("@merchant_param1", "additional Info.");
                    cmdCheck.Parameters.AddWithValue("@merchant_param2", "additional Info.");
                    cmdCheck.Parameters.AddWithValue("@merchant_param3", "CP");
                    cmdCheck.Parameters.AddWithValue("@merchant_param4", "additional Info.");
                    cmdCheck.Parameters.AddWithValue("@merchant_param5", "additional Info.");
                    cmdCheck.Parameters.AddWithValue("@promo_code", "");
                    cmdCheck.Parameters.AddWithValue("@customer_identifier", "");
                    cmdCheck.ExecuteNonQuery();
                }
                action1 = ConfigurationManager.AppSettings["KGHAPaymentURL"].ToString();
                string strForm = PreparePOSTForm(action1, data);
                Page.Controls.Add(new LiteralControl(strForm));




            }
            catch (Exception ex)
            {
                clsAppLogs.LogEvent("FrmKGHAReviewConfirm.aspx ::Error occured in onclick_btnpayment and error is " + ex.Message + " and stack trace is : " + ex.StackTrace);
                ExceptionUtility.LogException(ex, "paymentlink Method, frmKGHARegistrationdetails Page");

                Response.Redirect("FrmCustomErrorPage.aspx");
            }

        }

        //protected  string  randomrtid()
        //{

        //    string Date = DateTime.Now.Date.ToString("yyyy/MM/dd").Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
        //    string Hour = DateTime.Now.Hour.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
        //    string Minute = DateTime.Now.Minute.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
        //    string Second = DateTime.Now.Second.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
        //    string Millisecond = DateTime.Now.Millisecond.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
        //    string TID = Date + Hour + Minute + Second + Millisecond;
        //    return TID;
        //}
       


     
        private string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
           
            try
            {
                //Set a name for the form
                string formID = "nonseamless";
                //Build the form using the specified data to be posted.
                StringBuilder strForm = new StringBuilder();
                strForm.Append("<form id=\"" + formID + "\" name=\"" +
                               formID + "\" action=\"" + url +
                               "\" method=\"POST\">");

                foreach (System.Collections.DictionaryEntry key in data)
                {
                    if (key.Key.ToString() == "encRequest")
                    {
                        strForm.Append("<input type=\"hidden\" id=\"encRequest\" name=\"encRequest\" value=\"" + key.Value + "\">");
                    }
                    else if (key.Key.ToString() == "access_code")
                    {
                        strForm.Append("<input type=\"hidden\" id=\"Hidden1\" name=\"access_code\" value='" + key.Value + "'>");
                    }
                }


            


                strForm.Append("</form>");
                //Build the JavaScript which will do the Posting operation.
                StringBuilder strScript = new StringBuilder();
                strScript.Append("<script language='javascript'>");
                strScript.Append("var v" + formID + " = document." +
                                 formID + ";");
                strScript.Append("v" + formID + ".submit();");
                strScript.Append("</script>");

                return strForm.ToString() + strScript.ToString();

            }

            catch (Exception ex)
            {
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in Preparepostform for url :" + url + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                // LogEvent.Fn_LogEvent("FrmKGHAReviewConfirm.aspx :: Error occured in Preparepostform for url :" + url + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace);
                clsAppLogs.LogEvent("FrmKGHAReviewConfirm.aspx :: Error occured in Preparepostform for url :" + url + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace);
                //Response.Redirect("FrmCustomErrorPage.aspx");
                return "Fail";
            }

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
                ExceptionUtility.LogException(ex, "Error in SendSMS on frmKGHARegistrationdetails Page");
            }
            return IsSendSMSSuccess;
        }

        private static bool SaveOTP(string EmployeeCode, string GeneratedOTP, string ActionName, string ProposalNo)
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
                        cmd.CommandText = "PROC_SAVE_OTP_FOR_KGHA";

                        cmd.Parameters.AddWithValue("@EmployeeCode", EmployeeCode.Trim());
                        cmd.Parameters.AddWithValue("@GeneratedOTP", GeneratedOTP);
                        cmd.Parameters.AddWithValue("@ProposalNo", ProposalNo);
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
                ExceptionUtility.LogException(ex, "SaveOTP, frmKGHARegistrationdetails Page");
            }
            return IsOTPSavedToDB;
        }

        private static bool SendOTPEmail(string ToEmailId, string EmployeeCode, string OTPNumber, string EmployeeName)
        {



             bool Ismailsent = false;
            string emailId = ToEmailId;
            string strPath = string.Empty;
            string MailBody = string.Empty;

            try
            {

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBoby_KGHA_OTP.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("#EmployeeName#", EmployeeName);
                MailBody = MailBody.Replace("#ReplaceText#", "One Time Password For Kotak Group Health Assure is " + OTPNumber);


               
               
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"], "Kotak Mahindra General Insurance Co Ltd");
                mm.Subject = "OTP For Kotak Group Health Assure";
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;

                smtpClient.Send(mm);
                Ismailsent =true;
            }
            catch (Exception ex)
            {
                Ismailsent = false;
                string strPathErrorFile = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPathErrorFile, "Error: " + Ismailsent);

            }
            return Ismailsent;
        }

        //private static string SendAcknowledgementEmail(string ToEmailId, string EmployeeCode, string EmployeeName, string Amount, string AccountNumber, bool IsKLTEmployee)
        //{
        //    string smtp_DefaultCCMailId = ConfigurationManager.AppSettings["smtp_DefaultCCMailId"].ToString();
        //    string strMessage = string.Empty;

        //    string smtp_Username = ConfigurationManager.AppSettings["smtp_Username"].ToString();
        //    string smtp_Password = ConfigurationManager.AppSettings["smtp_Password"].ToString();
        //    string smtp_Host = ConfigurationManager.AppSettings["smtp_Host"].ToString();
        //    string smtp_FromMailId = "noreply@kotak.com";
        //    string strPath = string.Empty;
        //    string MailBody = string.Empty;

        //    try
        //    {
        //        SmtpClient client = new SmtpClient();
        //        client.Port = 25;
        //        client.Host = smtp_Host; //"192.168.201.61"; //"kgirelay.kgi.kotakgroup.com";
        //                                 //client.EnableSsl = true;
        //        client.Timeout = 3600000;
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new System.Net.NetworkCredential(smtp_Username, smtp_Password);


        //        strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBoby_STP_OTP.html";
        //        MailBody = File.ReadAllText(strPath);
        //        MailBody = MailBody.Replace("#EmployeeName#", EmployeeName);

        //        bool IsShowYearlyPaymentOptionOnly = true; //CR306
        //        if (IsKLTEmployee || IsShowYearlyPaymentOptionOnly)
        //        {
        //            MailBody = MailBody.Replace("#ReplaceText#", "Based on the consent provided by you, an amount of Rs. #Yearly_EMI# will be debited from your salary account " + AccountNumber + " within the next 30 days. <br><br>Your policy will commence the same day of salary account debit. For example, if your account is debited on 31st Jul 2018, your policy will commence from 31st Jul 2018.<br>");
        //            MailBody = MailBody.Replace("#Yearly_EMI#", Amount);
        //        }
        //        else
        //        {
        //            MailBody = MailBody.Replace("#ReplaceText#", "Based on the consent provided by you, an EMI of Rs. #Monthly_EMI# will be debited from your salary account " + AccountNumber + " within the next 30 days. This EMI debit will continue every month for the next 12 months, until the expiry of the policy term. <br><br>Your policy will commence the same day of salary account debit. For example, if your account is debited on 31st Jul 2018, your policy will commence from 31st Jul 2018.<br>");
        //            MailBody = MailBody.Replace("#Monthly_EMI#", Amount);
        //        }


        //        MailMessage mm = new MailMessage();
        //        mm.From = new MailAddress(smtp_FromMailId);
        //        mm.Subject = "Thank You for selecting Kotak Super Top-Up Plan";
        //        mm.Body = MailBody;
        //        mm.IsBodyHtml = true;

        //        mm.To.Add(ToEmailId);

        //        mm.BodyEncoding = UTF8Encoding.UTF8;
        //        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

        //        client.Send(mm);
        //        strMessage = "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        strMessage = "error while sending Acknowledgement Email";
        //        ExceptionUtility.LogException(ex, "SendAcknowledgementEmail Method, frmKGHARegistrationdetails Page");
        //    }

        //    return strMessage;
        //}
    }
}