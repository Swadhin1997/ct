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
using System.Threading;

namespace PrjPASS
{
    public class HealthWellnessMemberDetails
    {
        public string PolicyNumber { get; set; }
        public string MemberID { get; set; }
        public string RelationshipWithProposer { get; set; }
        public string NameOfProposer { get; set; }
        public string Gender { get; set; }
        public string NameOfMember { get; set; }
        public string DOB { get; set; }
        public string ContatNumber { get; set; }
        public string EmailID { get; set; }
        public string PortabilityUsed { get; set; }

    }

    public class HealthWellnessEmployeePrimaryDetails
    {
        public string PolicyNumber { get; set; }
        public string SourceType { get; set; }
        public string PolicystartDate { get; set; }
        public string PolicyEndDate { get; set; }
        public string InstallMentPolicy { get; set; }
        public string NameOfProposer { get; set; }
        public string MemberID { get; set; }
        public string RelationshipWithProposer { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string PreExistingCondition { get; set; }
        public string MyProperty { get; set; }
    }


    public partial class FrmHealthWellness : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddAttributesToControl();
            if (!IsPostBack)
            {
                if (Session["MemberDetails"] != null)
                {
                    HealthWellnessMemberDetails objMemberDetails = null;
                    objMemberDetails = (HealthWellnessMemberDetails)Session["MemberDetails"];

                    drpMembers.Items.Clear();
                    DataTable dt = GetHealthWellnessDetails(objMemberDetails.PolicyNumber, objMemberDetails.DOB);
                    if (dt.Rows.Count > 0)
                    {
                        showPolicyDetails(dt);
                        SetDropDownMemberDetails(dt);
                    }
                }
                lblerrorMessage.InnerText = "";

            }



            if (Session["MemberDetails"] == null)
            {
                lblwelcomeMessage.InnerHtml = "Welcome to Kotak Health Premier – Wellness Program  </br>  Registration Page";
                dvDrpMemberDetails.Visible = false;
                dvMessage.Visible = false;
            }

        }

        private void AddAttributesToControl()
        {
            try
            {
                txtHeightFeet.Attributes["type"] = "number";
                txtHeightFeet.Attributes["min"] = "1";
                txtHeightFeet.Attributes["max"] = "11";
                txtHeightFeet.Attributes["step"] = "1";

                txtHeightInch.Attributes["type"] = "number";
                txtHeightInch.Attributes["min"] = "0";
                txtHeightInch.Attributes["max"] = "11";
                txtHeightInch.Attributes["step"] = "1";


                txtWeight.Attributes["type"] = "number";
                txtWeight.Attributes["min"] = "15";
                txtWeight.Attributes["max"] = "100";
                txtWeight.Attributes["step"] = "1";


                txtWater.Attributes["type"] = "number";
                txtWater.Attributes["min"] = "2";
                txtWater.Attributes["max"] = "20";
                txtWater.Attributes["step"] = "1";


                BloodPressureHigh.Attributes["type"] = "number";
                BloodPressureHigh.Attributes["min"] = "80";
                BloodPressureHigh.Attributes["max"] = "180";
                BloodPressureHigh.Attributes["step"] = "1";

                BloodPressureLow.Attributes["type"] = "number";
                BloodPressureLow.Attributes["min"] = "60";
                BloodPressureLow.Attributes["max"] = "140";
                BloodPressureLow.Attributes["step"] = "1";
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogException(ex, "AddAttributesToControl ");
            }
        }

        private DataTable GetHealthWellnessDetails(string PolicyNo, string DOB)
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_WELLNESSDATA_BY_POLICYNO";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "PolicyNo", DbType.String, ParameterDirection.Input, "PolicyNo", DataRowVersion.Current, PolicyNo);
                db.AddParameter(dbCommand, "DOB", DbType.String, ParameterDirection.Input, "DOB", DataRowVersion.Current, DOB);
                dbCommand.CommandType = CommandType.StoredProcedure;
                ds = db.ExecuteDataSet(dbCommand);

                if (ds.Tables.Count == 0)
                {
                    if (Session["MemberDetails"] != null)
                    { Session.Remove("MemberDetails"); }

                    Alert.Show("Policy Number and Date of Birth not matched", "FrmHealthWellness.aspx");

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetHealthWellnessDetails Method");
            }

            return ds.Tables[0];
        }

        protected void btnSearchPolicyDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string strPolicyNumber = txtpolicyNumber.Value.Trim();
                string strDob = txtDateofBirth.Value.Trim();

                if (strPolicyNumber == "" || strDob == "")
                {
                    Alert.Show("Policy Number or Date of Birth can not be blank", "FrmHealthWellness.aspx");
                }
                else
                {
                    HealthWellnessMemberDetails objHealthWellnessMemberDetails = new HealthWellnessMemberDetails();
                    objHealthWellnessMemberDetails.PolicyNumber = strPolicyNumber;
                    objHealthWellnessMemberDetails.DOB = strDob;
                    Session["MemberDetails"] = objHealthWellnessMemberDetails;

                    DataTable dtHealthDetails = GetHealthWellnessDetails(strPolicyNumber, strDob);
                    if (dtHealthDetails.Rows.Count > 0)
                    {
                        showPolicyDetails(dtHealthDetails);
                        SetDropDownMemberDetails(dtHealthDetails);
                    }
                }



            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, " btnSearchPolicyDetails_Click ");

            }

        }

        private void showPolicyDetails(DataTable dtHealthDetails)
        {
            try
            {

                dvDrpMemberDetails.Visible = true;
                dvTblMembers.Visible = true;

                StringBuilder sb = new StringBuilder();
                sb = sb.Append("<table class=\"table-bordered table-striped table-condensed cf col-md-12 \"> <thead class=\"cf\"> ");
                sb.Append("<tr><th>Sr. No.</th>");
                sb.Append("<th>Policy No</th>");
                sb.Append("<th>Name of the Proposer </th>");
                sb.Append("<th>Member ID </th>");
                sb.Append("<th>Name of Member </th>");
                sb.Append("<th>Relationship</th>");
                sb.Append("<th>Status </th> </tr> </thead>");
                sb.Append("<tbody>");
                int i = 1;
                int PendingCases = 0;
                if (dtHealthDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtHealthDetails.Rows)
                    {
                        HdnPolicyNumber.Value = dr["Policy No"].ToString();
                        HdnPlanToken.Value = dr["ValueAddedBenefit"].ToString();
                        sb.Append("<tr>");
                        sb.Append("<td data-title=\"Sr. No.\" > " + i.ToString() + "</td>");
                        sb.Append("<td data-title=\"Policy No\">" + dr["Policy No"].ToString() + "</td> <td data-title=\"Name of the Proposer\">" + dr["Name of the Proposer"].ToString() + "</td> <td data-title=\"Member ID\"> " + dr["Member ID"].ToString() + "</td> <td data-title=\"Name of the member\"> " + dr["Name of the member"].ToString() + "</td> <td data-title=\"Relationship\"> " + dr["Relationship with Proposer"].ToString() + " </td> <td data-title=\"Status\"> ");
                        if (dr["Status"].ToString() == "Registration pending")
                        {
                            PendingCases++;
                            sb.Append("<span style='color:Red'> ");
                        }
                        else { sb.Append("<span style='color:Green'> "); }
                        sb.Append("" + dr["Status"].ToString() + " </span></td>");
                        sb.Append("</tr> ");
                        i++;
                    }
                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("</br>");
                    sb.Append("</br>");

                    ltPolicyDetails.Text = sb.ToString();
                    lblwelcomeMessage.InnerText = "Member Details";
                    if (PendingCases == 0)
                    {
                        lblMessage.InnerText = "All details for policy members provided already.";
                        lblMessage.Visible = true;
                        dvMessage.Visible = true;
                        dvDrpMemberDetails.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, " showPolicyDetails ");
            }
        }

        private void SetDropDownMemberDetails(DataTable dtHealthDetails)
        {
            try
            {
                tblSearchPolicy.Visible = false;

                drpMembers.Items.Add(new ListItem("--Select--", "0"));

                foreach (DataRow dr in dtHealthDetails.Rows)
                {
                    if (dr["Status"].ToString() == "Registration pending")
                        drpMembers.Items.Add(new ListItem(dr["Name of the member"].ToString() + " (" + dr["Relationship with Proposer"].ToString() + ")", dr["Member ID"].ToString()));
                }

                drpMembers.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, " SetDropDownMemberDetails ");
            }
        }

        protected void drpMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HdnMemberID.Value = drpMembers.SelectedValue.Trim().ToString();
                //  lblTitle.InnerText = "Details for Member ID " + HdnMemberID.Value + ",Name / Relationship " + drpMembers.SelectedItem.ToString();
                lblTitle.InnerText = "Details for " + drpMembers.SelectedItem.ToString()+" (" + HdnMemberID.Value+ ")";
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, " drpMembers_SelectedIndexChanged ");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidationMessage() == "")
            {
                SaveMemberHealthDetails();
            }
            else
            {
                string msg = ValidationMessage();
                lblerrorMessage.InnerHtml = msg.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "pop", "openmodalHealthWelness();", true);
            }

        }

        private string ValidationMessage()
        {
            string strValidationMessage = " ";
            string strRequiredItems = "  Details are mandatory. <ul>";
            try
            {
                if (string.IsNullOrEmpty(txtHeightFeet.Value.Trim()))
                {
                    strRequiredItems = strRequiredItems + "<li>Section Biometrics - Question 1 (For Height in Feet)</li> ";
                }

                if (string.IsNullOrEmpty(txtHeightInch.Value.Trim()))
                {
                    strRequiredItems = strRequiredItems + " <li>Section Biometrics - Question 1 (For Height in Inch)</li> ";
                }

                if (string.IsNullOrEmpty(txtWeight.Value.Trim()))
                {
                    strRequiredItems = strRequiredItems + " <li>Section Biometrics - Question 2</li>";
                }

                if (BloodGroup.SelectedValue.Trim() == "--Select--")
                {
                    strRequiredItems = strRequiredItems + " <li>Section Biometrics - Question 3</li> ";
                }

                if (RdodiabYes.Checked == false && RdodiabNo.Checked == false)
                {
                    strRequiredItems = strRequiredItems + " <li>Section Personal Health - Question 1</li> ";
                }

                if (inlineradio1BloodPressure.Checked == false && inlineradio2BloodPressure.Checked == false)
                {
                    strRequiredItems = strRequiredItems + " <li>Section Personal Health - Question 2</li></li> ";
                }

                if (inlineradio1Cholestrol.Checked == false && inlineradio2Cholestrol.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Personal Health - Question 3</li></li>";
                }

                if (Radio1Cancer.Checked == false && Radio2Cancer.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Medical History - Question 1</li>";
                }

                if (Radio1HeartRelated.Checked == false && Radio2HeartRelated.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Medical History - Question 2 </li>";
                }

                if (Radio1Stroke.Checked == false && Radio2Stroke.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Medical History - Question 3</li>";
                }


                if (Radio1Kidney.Checked == false && Radio2Kidney.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Medical History - Question 4</li>";
                }


                if (Radio1ent.Checked == false && Radio2ent.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Medical History - Question 5</li>";
                }

                if (Radio1ent.Checked == true && string.IsNullOrEmpty(txtENTspecification.Value.Trim().ToString()))
                {
                    strRequiredItems = strRequiredItems + "<li>Section Medical History - Question 5 specification</li>";
                }

                if (Radio1Anemia.Checked == false && Radio2Anemia.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Medical History - Question 6</li>";
                }

                if (Radio1Anemia.Checked == true && string.IsNullOrEmpty(txtAnemiaspecification.Value.Trim().ToString()))
                {
                    strRequiredItems = strRequiredItems + "<li>Section Medical History  - Question 6 specification</li>";
                }

                if (Radio1LifestyleClass.Checked == false && Radio2LifestyleClass.Checked == false && Radio3LifestyleClass.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Lifestyle - Question 1</li>";
                }

                if (Radio1SleepHours.Checked == false && Radio2SleepHours.Checked == false && Radio3SleepHours.Checked == false && Radio4SleepHours.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Lifestyle - Question 2</li>";
                }

                if (Radio1VegFruits.Checked == false && Radio2VegFruits.Checked == false && Radio3VegFruits.Checked == false && Radio4VegFruits.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Lifestyle - Question 3</li>";
                }

                if (Radio1Smoking.Checked == false && Radio2Smoking.Checked == false && Radio3Smoking.Checked == false && Radio4Smoking.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Lifestyle - Question 4</li>";
                }

                if (Radio1Alchohol.Checked == false && Radio2Alchohol.Checked == false && Radio3Alchohol.Checked == false && Radio4Alchohol.Checked == false && Radio5Alchohol.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Lifestyle - Question 6</li>";
                }

                if (Radio1EnjoyWork.Checked == false && Radio2EnjoyWork.Checked == false && Radio3EnjoyWork.Checked == false && Radio4EnjoyWork.Checked == false && Radio5EnjoyWork.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Occupational Health - Question 1</li>";
                }

                if (Radio1StressLevel.Checked == false && Radio2StressLevel.Checked == false && Radio3StressLevel.Checked == false && Radio4StressLevel.Checked == false && Radio5StressLevel.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Occupational Health -  Question 2</li>";
                }

                if (Radio1WorkLoad.Checked == false && Radio2WorkLoad.Checked == false && Radio3WorkLoad.Checked == false && Radio4WorkLoad.Checked == false && Radio5WorkLoad.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Occupational Health - Question 3</li>";
                }

                if (Radio1Balance.Checked == false && Radio2Balance.Checked == false && Radio3Balance.Checked == false && Radio4Balance.Checked == false && Radio5Balance.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Social and Environment Wellness - Question 1</li>";
                }

                if (Radio1Interaction.Checked == false && Radio2Interaction.Checked == false && Radio3Interaction.Checked == false && Radio4Interaction.Checked == false && Radio5Interaction.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Social and Environment Wellness - Question 2</li>";
                }

                if (Radio1Satisfied.Checked == false && Radio2Satisfied.Checked == false && Radio3Satisfied.Checked == false && Radio4Satisfied.Checked == false && Radio5Satisfied.Checked == false)
                {
                    strRequiredItems = strRequiredItems + "<li>Section Social and Environment Wellness - Question 3</li>";
                }





                if (strRequiredItems.Length > 1 && strRequiredItems != "  Details are mandatory. <ul>")
                    strValidationMessage = strRequiredItems + "</ul>";

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "ValidationMessage");
            }
            return strValidationMessage.Remove(0, 1);
        }

        private void SaveMemberHealthDetails()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    using (SqlCommand cmd = new SqlCommand("PROC_INSERT_HEALTH_WELLNESS_MEMBER_DETAILS", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MemberID", HdnMemberID.Value.Trim());
                        cmd.Parameters.AddWithValue("@HeightFeet", txtHeightFeet.Value.Trim());
                        cmd.Parameters.AddWithValue("@HeightInch", txtHeightInch.Value.Trim());
                        cmd.Parameters.AddWithValue("@WeightInKG", txtWeight.Value.Trim());
                        cmd.Parameters.AddWithValue("@BloodGroup", BloodGroup.SelectedValue.Trim());
                        cmd.Parameters.AddWithValue("@BloodPressureHigh", BloodPressureHigh.Value.Trim());
                        cmd.Parameters.AddWithValue("@BloodPressureLow", BloodPressureLow.Value.Trim());
                        cmd.Parameters.AddWithValue("@Diabetes", RdodiabYes.Checked == true ? true : false);
                        cmd.Parameters.AddWithValue("@HighBloodPressure", inlineradio1BloodPressure.Checked == true ? true : false);
                        cmd.Parameters.AddWithValue("@CholestrolCheckInLastFiveYears", inlineradio1Cholestrol.Checked == true ? true : false);
                        cmd.Parameters.AddWithValue("@CholesterolLevel", txtCholesterolLevel.Value.Trim());
                        cmd.Parameters.AddWithValue("@HDLCholesterol", txtHDLCholesterol.Value.Trim());
                        cmd.Parameters.AddWithValue("@MedicalHistoryCancer", Radio1Cancer.Checked == true ? true : false);
                        cmd.Parameters.AddWithValue("@MedicalHistoryHeartRelated", Radio1HeartRelated.Checked == true ? true : false);
                        cmd.Parameters.AddWithValue("@MedicalHistoryStroke", Radio1Stroke.Checked == true ? true : false);
                        cmd.Parameters.AddWithValue("@MedicalHistoryKidneyLung", Radio1Kidney.Checked == true ? true : false);
                        cmd.Parameters.AddWithValue("@MedicalHistoryPartOne", Radio1ent.Checked == true ? true : false);
                        cmd.Parameters.AddWithValue("@MedicalHistoryPartOneSpecify", txtENTspecification.Value.Trim());
                        cmd.Parameters.AddWithValue("@MedicalHistoryPartTwo", Radio1Anemia.Checked == true ? true : false);
                        cmd.Parameters.AddWithValue("@MedicalHistoryPartTwoSpecify", txtAnemiaspecification.Value.Trim());
                        cmd.Parameters.AddWithValue("@LifestyleClassification", Request.Form["i-radioLifestyle"].ToString());
                        cmd.Parameters.AddWithValue("@SleepHours", Request.Form["i-radioSleepHour"].ToString());
                        cmd.Parameters.AddWithValue("@VegFruitConsumeEachDay", Request.Form["i-radioVegetables"].ToString());
                        cmd.Parameters.AddWithValue("@GlassesOfWaterEachDay", txtWater.Value.Trim());
                        cmd.Parameters.AddWithValue("@SmokingHabits", Request.Form["i-radioSmoking"].ToString());
                        cmd.Parameters.AddWithValue("@AlcoholConsumption", Request.Form["i-radioAlcohol"].ToString());
                        cmd.Parameters.AddWithValue("@EnjoyTheWork", Request.Form["i-OccupationalHealthEnjoyWork"].ToString());
                        cmd.Parameters.AddWithValue("@StressInWorkEnvironmentIsManageable", Request.Form["i-OccupationalHealthManageable"].ToString());
                        cmd.Parameters.AddWithValue("@SatisfyToManageAndControlWorkLoad", Request.Form["i-OccupationalHealthAbility"].ToString());
                        cmd.Parameters.AddWithValue("@BalanceBetweenWorkFamilyLife", Request.Form["i-SocialGoodBalance"].ToString());
                        cmd.Parameters.AddWithValue("@InteractionsFamilyFriends", Request.Form["i-SocialInteractions"].ToString());
                        cmd.Parameters.AddWithValue("@SatisfiedWithLife", Request.Form["i-SocialSatisfied"].ToString());
                        cmd.Parameters.Add("@OutputMessage", SqlDbType.VarChar, 1000);
                        cmd.Parameters["@OutputMessage"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        // Alert.Show(cmd.Parameters["@OutputMessage"].Value.ToString(), "FrmHealthWellness.aspx");

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "swal", "ShowSweetAlert(\" " + cmd.Parameters["@OutputMessage"].Value.ToString() + " \");", true);


                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveMemberHealthDetails");
            }
        }

        protected void lbtnLogOut_Click(object sender, EventArgs e)
        {
            Session.Remove("MemberDetails");
            Response.Redirect("FrmHealthWellness.aspx");
        }

        protected void btnShowMember_Click(object sender, EventArgs e)
        {
            string PolicyNo = HdnPolicyNumber.Value.Trim(),
                   MemberID = HdnMemberID.Value.Trim(),
                   PlanToken = HdnPlanToken.Value.Trim(),
                   PartnerKey = ConfigurationManager.AppSettings["TrueWorthKey"].ToString();
            string Url = string.Format("https://insurance.thewellnesscorner.com/kotak?policyNo={0}&memberID={1}&planToken={2}&partnerKey={3}",PolicyNo,MemberID,PlanToken,PartnerKey);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "swal", "ShowDisclaimerSweetAlert(\" " + Url + " \");", true);
        }
    }
}
