using Microsoft.Practices.EnterpriseLibrary.Data;
using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmHDCClaimDetails : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["vUserLoginId"] != null)
                {
                    if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
                    {
                        bool chkAuth;
                        string pageName = this.Page.ToString().Substring(4, this.Page.ToString().Substring(4).Length - 5) + ".aspx";
                        chkAuth = wsGen.Fn_Check_Rights_For_Page(Session["vRoleCode"].ToString(), pageName);
                        if (chkAuth == false)
                        {
                            Alert.Show("Access Denied", "FrmMainMenu.aspx");
                            return;
                        }
                    }

                    FnHideDataFields();

                    FnBindICDMasterData();



                }
                else
                {
                    Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    return;
                }

                Directory.CreateDirectory(folderPath);

                // clnClaimIntimationDate.DateMax = DateTime.Now;
                //   txtClaimIntimationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //   txtClaimRegistrationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //   txtClaimIntimationDate.Attributes.Add("readonly", "readonly");
                txtDateOfAdmission.Attributes.Add("readonly", "readonly");
                txtDateOfDischarge.Attributes.Add("readonly", "readonly");
                txtDateofDeath.Attributes.Add("readonly", "readonly");
                txtFinalSubmitDate.Attributes.Add("readonly", "readonly");
                txtInvestigationDate.Attributes.Add("readonly", "readonly");
                txtInvestigatorName.MaxLength = 50;
                txtHospitalName.MaxLength = 100;
                txtHospitalAddress.MaxLength = 500;
                txtInvestigatorName.MaxLength = 50;
                txtInvestigatorAddress.MaxLength = 500;
                txtHospitalPinCode.MaxLength = 6;
                txtDiseaseDescription.MaxLength = 4000;
                txtClaimDetailRemark.MaxLength = 4000;
                txtDiseaseDescription.Attributes.Add("maxlength", txtDiseaseDescription.MaxLength.ToString());
                txtClaimDetailRemark.Attributes.Add("maxlength", txtClaimDetailRemark.MaxLength.ToString());

            }
        }



        private void FnHideDataFields()
        {
            d1.Visible = false; d2.Visible = false; d3.Visible = false; d4.Visible = false; d5.Visible = false;
            d6.Visible = false; d7.Visible = false; d8.Visible = false; d9.Visible = false; d10.Visible = false;
            d11.Visible = false; d12.Visible = false; d13.Visible = false; d14.Visible = false; d15.Visible = false;
            d16.Visible = false; d17.Visible = false; d18.Visible = false; d19.Visible = false; d20.Visible = false;
            d21.Visible = false; d22.Visible = false; d23.Visible = false; d24.Visible = false; d25.Visible = false;
            d26.Visible = false; d27.Visible = false; d28.Visible = false; d29.Visible = false; d30.Visible = false;
            d31.Visible = false; d32.Visible = false; d33.Visible = false; d34.Visible = false; d35.Visible = false;
            d36.Visible = false; d37.Visible = false; d38.Visible = false; d39.Visible = false; d40.Visible = false;
            d41.Visible = false; d42.Visible = false; d43.Visible = false; d44.Visible = false; d45.Visible = false;
            d46.Visible = false; d47.Visible = false; d48.Visible = false; d49.Visible = false; d50.Visible = false;
            d51.Visible = false; d52.Visible = false; d53.Visible = false; d54.Visible = false; d55.Visible = false;
            d56.Visible = false; d57.Visible = false; d61.Visible = false; d62.Visible = false; d64.Visible = false; d65.Visible = false; 
        }

        private void FnShowDataFields()
        {
            d1.Visible = true; d2.Visible = true; d3.Visible = true; d4.Visible = true; d5.Visible = true;
            d6.Visible = true; d7.Visible = true; d8.Visible = true; d9.Visible = true; d10.Visible = true;
            d11.Visible = true; d12.Visible = true; d13.Visible = true; d14.Visible = true; d15.Visible = true;
            d16.Visible = true; d17.Visible = true; d18.Visible = true; d19.Visible = true; d20.Visible = true;
            d21.Visible = true; d22.Visible = true; d23.Visible = true; d24.Visible = true; d25.Visible = true;
            d26.Visible = true; d27.Visible = true; d28.Visible = true; d29.Visible = true; d30.Visible = true;
            d31.Visible = true; d32.Visible = true; d33.Visible = true; d34.Visible = true; d35.Visible = true;
            d36.Visible = true; d37.Visible = true; d38.Visible = true; d39.Visible = true; d40.Visible = true;
            d41.Visible = true; d42.Visible = true; d43.Visible = true; d44.Visible = true; d45.Visible = true;
            d46.Visible = true; d47.Visible = true; d48.Visible = true; d49.Visible = true; d50.Visible = true;
            d51.Visible = true; d52.Visible = true; d53.Visible = true; d54.Visible = true; d55.Visible = true;
            d56.Visible = true; d57.Visible = true; d61.Visible = true; d62.Visible = true; d64.Visible = true; d65.Visible = true;

        }


        private void FnBindICDMasterData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPAss"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("PROC_GET_LEVEL1_ICD_DATA", con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    drpIDCch1.DataSource = dt;
                    drpIDCch1.DataBind();
                    drpIDCch1.DataTextField = "DisplayText";
                    drpIDCch1.DataValueField = "Level1No";
                    drpIDCch1.Items.Insert(0, new ListItem("", ""));
                    drpIDCch1.DataBind();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FnBindICDMasterData");
            }
        }

        protected void btnSearchClaim_Click(object sender, EventArgs e)
        {


            if (fnCheckClaimDetails(txtCertificateNumber.Text))
            {
                Alert.Show("Claim Details already submitted for this claim. To Update Details kindly Use Edit Claim Details form.");
                return;
            }


            try
            {
                if (String.IsNullOrEmpty(txtCertificateNumber.Text))
                {
                    ResetControls();
                    Alert.Show("Enter Policy Number/Claim Number !!");
                    return;
                }

                else
                {
                    ResetControls();
                    string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;





                    using (SqlConnection con = new SqlConnection(consString))
                    {
                        SqlCommand cmd = new SqlCommand("PROC_HDC_CLAIM_DETAILS", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCertificateNumber", txtCertificateNumber.Text);
                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fetching data for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dtCertificateDetails = new DataTable();
                        sda.Fill(dtCertificateDetails);

                        if (dtCertificateDetails != null)
                        {
                            if (dtCertificateDetails.Rows.Count > 0)
                            {
                                d58.Attributes.Add("Disabled", "");
                                d59.Attributes.Add("Disabled", "");
                                d60.Attributes.Add("Disabled", "");
                                txtCertificateNumber.Enabled = false;
                                FnShowDataFields();
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);

                                txtClaimNumber.Text = dtCertificateDetails.Rows[0]["vClaimNumber"].ToString();
                                txtCertNumber.Text = dtCertificateDetails.Rows[0]["vCertificateNumber"].ToString();

                                string DateofDischarge = dtCertificateDetails.Rows[0]["dDateOfDischarge"].ToString();
                                string d = !string.IsNullOrEmpty(DateofDischarge) ? Convert.ToDateTime(DateofDischarge).ToString("dd/MM/yyyy") : "";
                                txtDateOfDischarge.Text = d.ToString();

                                hfPolicyStartDate.Value = Convert.ToDateTime(dtCertificateDetails.Rows[0]["vPolicyStartdate"]).ToString("dd/MM/yyyy");
                                string DateofAdmission = dtCertificateDetails.Rows[0]["dDateOfAdmission"].ToString();
                                d = !string.IsNullOrEmpty(DateofAdmission) ? Convert.ToDateTime(DateofAdmission).ToString("dd/MM/yyyy") : "";
                                txtDateOfAdmission.Text = d.ToString();




                                txtClaimNumber.Attributes.Add("Disabled", "");
                                txtCertNumber.Attributes.Add("Disabled", "");
                            }

                            else
                            {
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg no data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                                Alert.Show("No Data Found for this claim or certificate number!!");
                                return;
                            }
                        }

                        else
                        {
                            Alert.Show("No Data Found !!");
                            return;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg Error occured in search certificate : " + txtCertificateNumber.Text + " and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Alert.Show("something went wrong - Try again later !!", "FrmMainMenu.aspx");
            }
        }

        private bool fnCheckClaimDetails(string vClaimNumber)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ToString()))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (SqlCommand cmd = new SqlCommand("select  * from TBL_HDC_CLAIMS_DETAILS_TABLE where vClaimNumber = @vClaimNumber", con))
                    {
                        cmd.Parameters.AddWithValue("@vClaimNumber", vClaimNumber);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            res = true;
                            return res;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnCheckClaimDetails");
                res = false;
            }
            return res;
        }

        private void ResetControls()
        {
            txtClaimNumber.Text = string.Empty;
            txtCertNumber.Text = string.Empty;
            //txtCustomerMobile.Text = string.Empty;
            //txtMasterPolicyNumber.Text = string.Empty;
            //txtMasterPolicyHolder.Text = string.Empty;
            //txtProductName.Text = string.Empty;
            //txtPolicyStartDate.Text = string.Empty;
            //txtPolicyEndDate.Text = string.Empty;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void drpIDCch2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPAss"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("PROC_GET_LEVEL3_ICD_DATA", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@level2No", drpIDCch2.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@level1No", drpIDCch1.SelectedValue.ToString());
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    drpIDCch3.DataSource = dt;
                    drpIDCch3.DataBind();
                    drpIDCch3.DataTextField = "DisplayText";
                    drpIDCch3.DataValueField = "Level3No";
                    drpIDCch3.Items.Insert(0, new ListItem("", ""));
                    drpIDCch3.DataBind();

                    drpIDCch4.Items.Clear();
                    drpIDCch4.DataSource = null;
                    drpIDCch4.DataBind();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FnBindICDMasterData");
            }
        }

        protected void drpIDCch1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPAss"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("PROC_GET_LEVEL2_ICD_DATA", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@level1No", drpIDCch1.SelectedValue.ToString());
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    drpIDCch2.DataSource = dt;
                    drpIDCch2.DataBind();
                    drpIDCch2.DataTextField = "DisplayText";
                    drpIDCch2.DataValueField = "Level2No";
                    drpIDCch2.Items.Insert(0, new ListItem("", ""));
                    drpIDCch2.DataBind();

                    drpIDCch3.Items.Clear();
                    drpIDCch3.DataSource = null;
                    drpIDCch3.DataBind();
                    drpIDCch4.Items.Clear();
                    drpIDCch4.DataSource = null;
                    drpIDCch4.DataBind();

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FnBindICDMasterData");
            }
        }

        protected void drpIDCch3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPAss"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("PROC_GET_LEVEL4_ICD_DATA", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@level3No", drpIDCch3.SelectedValue.ToString());
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    drpIDCch4.DataSource = dt;
                    drpIDCch4.DataBind();
                    drpIDCch4.DataTextField = "DisplayText";
                    drpIDCch4.DataValueField = "Level4No";
                    drpIDCch4.Items.Insert(0, new ListItem("", ""));
                    drpIDCch4.DataBind();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FnBindICDMasterData");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmHDCClaimDetails.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string DateofDischarge = Request.Form[txtDateOfDischarge.UniqueID];
            string DateofAdmission = Request.Form[txtDateOfAdmission.UniqueID];
            string DateofDeath = Request.Form[txtDateofDeath.UniqueID];
            string DateofInvestidation = Request.Form[txtInvestigationDate.UniqueID];
            string DateofFinalReportSubmit = Request.Form[txtFinalSubmitDate.UniqueID];

            Regex regexICUDays = new Regex(@"^[0-9]+$");
            try
            {
                if (string.IsNullOrEmpty(txtClaimNumber.Text))
                {
                    Alert.Show("Claim Number can not be empty !!");
                    return;
                }


                if (string.IsNullOrEmpty(txtCertNumber.Text) || string.IsNullOrEmpty(txtCertificateNumber.Text))
                {
                    Alert.Show("Certificate Number can not be blank !!");
                    return;
                }

                if (string.IsNullOrEmpty(txtDateOfAdmission.Text))
                {
                    Alert.Show("Certificate Number can not be blank!!");
                    return;
                }

                if (string.IsNullOrEmpty(DateofDischarge))
                {
                    Alert.Show("Discharge date can not be blank!!");
                    return;
                }



                if (!string.IsNullOrEmpty(DateofDischarge))
                {
                    if (DateTime.ParseExact(DateofDischarge, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        Alert.Show(string.Format("Date Of Discharge {0} cannot be before the Date Of Admission {1}", DateofDischarge, DateofAdmission));
                        return;
                    }
                }


                if (!String.IsNullOrEmpty(DateofDischarge))
                {

                    if (DateTime.ParseExact(DateofDischarge, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        Alert.Show(string.Format("Date Of Discharge {0} cannot be before the Date Of Admission {1}", DateofDischarge, DateofAdmission));
                        return;
                    }
                }


                if (!String.IsNullOrEmpty(hfPolicyStartDate.Value) && !String.IsNullOrEmpty(DateofDeath))
                {

                    if (DateTime.ParseExact(DateofDeath, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(hfPolicyStartDate.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        Alert.Show(string.Format("Date Of Death {0} cannot be before the Policy start date {1} ", DateofDeath, hfPolicyStartDate.Value.ToString()));
                        return;
                    }
                }




                if (!regexICUDays.IsMatch(txtICUDays.Text))
                {
                    Alert.Show("Days in ICU must be a valid number !!");
                    return;
                }


                if (string.IsNullOrEmpty(txtICUDays.Text))
                {
                    Alert.Show("Days in ICU can not be blank !!");
                    return;
                }


                if (!regexICUDays.IsMatch(txtNonICUDays.Text))
                {
                    Alert.Show("Days in Non ICU must be a valid number !!");
                    return;
                }

                if (string.IsNullOrEmpty(txtNonICUDays.Text))
                {
                    Alert.Show("Days in NON ICU can not be blank !!");
                    return;
                }


                if (string.IsNullOrEmpty(txtHospitalName.Text))
                {
                    Alert.Show("Hospital Name can not be blank !!");
                    return;
                }



                if (!string.IsNullOrEmpty(txtHospitalPinCode.Text))
                {
                    Regex PinCodeRegex = new Regex("^[1-9][0-9]{5}$");
                    if (!PinCodeRegex.IsMatch(txtHospitalPinCode.Text))
                    {
                        Alert.Show("Hospital Pin code not valid !!");
                        return;
                    }

                }


                if (!string.IsNullOrEmpty(txtInvestigatorName.Text))
                {

                }

                if (string.IsNullOrEmpty(txtExpenseAmt.Text))
                {
                    Alert.Show("Expense amount can not be be blank !!");
                    return;
                }


                Regex regex = new Regex("^[0-9]*$");

                if (!regex.IsMatch(txtExpenseAmt.Text))
                {
                    Alert.Show("Expense amount must contains only numbers !!");
                    return;
                }

                if (txtExpenseAmt.Text.Contains("-") || txtExpenseAmt.Text.Contains("."))
                {
                    Alert.Show("Expense amount can not be negative or contains decimal point !!");
                    return;
                }


                if (!String.IsNullOrEmpty(DateofInvestidation))
                {
                    if (DateTime.ParseExact(DateofInvestidation, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        Alert.Show(string.Format("Date Of Investigation {0} cannot be before the Date of Admission {1} ", DateofInvestidation, DateofAdmission));
                        return;
                    }
                }

                if (!String.IsNullOrEmpty(DateofInvestidation) && !String.IsNullOrEmpty(DateofFinalReportSubmit))
                {
                    if (DateTime.ParseExact(DateofFinalReportSubmit, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        Alert.Show(string.Format("Date Of Final Report Submission {0} cannot be before the Date of Admission {1} ", DateofFinalReportSubmit, DateofAdmission));
                        return;
                    }
                }





                if (!String.IsNullOrEmpty(DateofFinalReportSubmit))
                {
                    if (string.IsNullOrEmpty(DateofInvestidation))
                    {
                        Alert.Show(string.Format("Investigation date must be selected if selecting Final Report submit date"));
                        return;
                    }

                    if (DateTime.ParseExact(DateofFinalReportSubmit, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        Alert.Show(string.Format("Date Of Final Report Submission {0} cannot be before the Date of Admission {1} ", DateofFinalReportSubmit, DateofAdmission));
                        return;
                    }

                    if (DateTime.ParseExact(DateofFinalReportSubmit, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofInvestidation, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        Alert.Show(string.Format("Date Of Final Report Submission {0} cannot be before the Date of Investigation {1} ", DateofFinalReportSubmit, DateofInvestidation));
                        return;
                    }

                }



                if (DrpBenefitType.SelectedItem.ToString() == "Select")
                {
                    Alert.Show("Benefit type must be selected !!");
                    return;
                }

                if (drpIDCch1.SelectedValue == "")
                {
                    Alert.Show("IDC Chapter Level 1 is mandatory !!");
                    return;
                }


                if (drpIDCch2.SelectedValue == "")
                {
                    Alert.Show("IDC Chapter Level 2 is mandatory !!");
                    return;
                }


                if (txtDiseaseDescription.Text.Length > 4000)
                {
                    Alert.Show("Maximum 4000 characters are allowed in Disease Description !!");
                    return;
                }

                if (txtClaimDetailRemark.Text.Length > 4000)
                {
                    Alert.Show("Maximum 4000 characters are allowed in Claim Detail Remark");
                    return;
                }

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("PROC_INSERT_TBL_HDC_CLAIMS_DETAILS_TABLE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //  cmd.Parameters.AddWithValue("@dDateOfRegistration", DateTime.ParseExact(txtClaimRegistrationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@vCertificateNumber", txtCertNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@vClaimNumber", txtClaimNumber.Text.Trim());
                    if (!string.IsNullOrEmpty(txtDateOfAdmission.Text))
                    {

                        cmd.Parameters.AddWithValue("@dDateofAdmission", DateTime.ParseExact(txtDateOfAdmission.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@dDateofAdmission", "");
                    }

                    if (!string.IsNullOrEmpty(txtDateOfDischarge.Text))
                    {
                        cmd.Parameters.AddWithValue("@dDateofDischarge", DateTime.ParseExact(txtDateOfDischarge.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@dDateofDischarge", "");
                    }

                    if (!string.IsNullOrEmpty(txtDateofDeath.Text))
                    {
                        cmd.Parameters.AddWithValue("@dDateofDeath", DateTime.ParseExact(txtDateofDeath.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@dDateofDeath", "");
                    }
                    cmd.Parameters.AddWithValue("@nICUdays", txtICUDays.Text.Trim());
                    cmd.Parameters.AddWithValue("@nNonICUdays", txtNonICUDays.Text.Trim());
                    cmd.Parameters.AddWithValue("@vHospitalName", txtHospitalName.Text.Trim());
                    cmd.Parameters.AddWithValue("@vHospitalAddress", txtHospitalAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@vHospitalPinCode", txtHospitalPinCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@vHospitalCity", txtHospitalCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@vHospitalState", txtHospitalState.Text.Trim());
                    cmd.Parameters.AddWithValue("@vClaimSettlingOffice", txtClaimSettlingOffice.Text.Trim());
                    cmd.Parameters.AddWithValue("@vInvestigatorName", txtInvestigatorName.Text.Trim());
                    cmd.Parameters.AddWithValue("@vInvestigatorAddress", txtInvestigatorAddress.Text.Trim());
                    if (!string.IsNullOrEmpty(txtInvestigationDate.Text))
                    {
                        cmd.Parameters.AddWithValue("@dInvestigationDate", DateTime.ParseExact(txtInvestigationDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@dInvestigationDate", "");
                    }

                    if (!string.IsNullOrEmpty(txtFinalSubmitDate.Text))
                    {
                        cmd.Parameters.AddWithValue("@dInvestigationReportSubmitDate", DateTime.ParseExact(txtFinalSubmitDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@dInvestigationReportSubmitDate", "");
                    }

                    cmd.Parameters.AddWithValue("@vBenefitType", DrpBenefitType.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@nExpenseAmount", txtExpenseAmt.Text.Trim());
                    cmd.Parameters.AddWithValue("@vIDCCH1", drpIDCch1.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@vIDCCH2", drpIDCch2.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@vIDCCH3", drpIDCch3.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@vIDCCH4", drpIDCch4.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@vDiseaseDesc", txtDiseaseDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@vRemarkClaimDetail", txtClaimDetailRemark.Text.Trim());
                    cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                    cmd.ExecuteNonQuery();
                    Alert.Show(string.Format("Details saved for claim number {0}", txtClaimNumber.Text), "FrmHDCClaimDetails.aspx");
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "btnSave_Click   on FrmHDCClaimDetails.aspx.cs");
            }
        }



        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetPincode(string prefix)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_PINCODE";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IntrCds.Add(string.Format("{0}~{1}", sdr["NUM_PINCODE"], sdr["TXT_PINCODE_LOCALITY"]));
                        }
                        conn.Close();
                    }
                }
            }
            return IntrCds.ToArray();
        }

        protected void btnGetPincodeDetails_Click(object sender, EventArgs e)
        {

            lblPincodeLocality.Text = hdnPinCodeLocality.Value;
            string pincode = hdnPinCode.Value;
            SetStateCityDistrict(hdnPinCode.Value, hdnPinCodeLocality.Value);
        }

        private void SetStateCityDistrict(string value1, string value2)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_STATE_CITY_DISTRICT";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "NUM_PINCODE", DbType.String, ParameterDirection.Input, "@NUM_PINCODE", DataRowVersion.Current, txtHospitalPinCode.Text);
            db.AddParameter(dbCommand, "TXT_PINCODE_LOCALITY", DbType.String, ParameterDirection.Input, "@TXT_PINCODE_LOCALITY", DataRowVersion.Current, lblPincodeLocality.Text);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtHospitalState.Text = ds.Tables[0].Rows[0]["StateName"].ToString();
                    txtHospitalCity.Text = ds.Tables[0].Rows[0]["CityName"].ToString();
                    //  txtHospitalState.Attributes.Add("readonly", "readonly");
                    //  txtHospitalCity.Attributes.Add("readonly", "readonly");

                }
            }
        }

        protected void btnExit_Click1(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }

}