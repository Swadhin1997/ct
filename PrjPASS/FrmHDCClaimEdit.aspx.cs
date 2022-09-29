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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmHDCClaimEdit : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");
        protected string ActiveTab { get; private set; }
        static string DateofAdmission;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                txtRemarkClaim.Text = !string.IsNullOrEmpty(hftxtRemark.Value) ? hftxtRemark.Value.ToString().Trim() : string.Empty;
                txtDisDesc.Text = !string.IsNullOrEmpty(hftxtDiseasDesc.Value) ? hftxtDiseasDesc.ToString().Trim() : string.Empty;

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
                }
                else
                {
                    Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    return;
                }

                Directory.CreateDirectory(folderPath);
                FnBindICDMasterData();
                //clnClaimIntimationDate.DateMax = DateTime.Now;
                ////   txtClaimIntimationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtClaimRegistrationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtClaimIntimationDate.Attributes.Add("readonly", "readonly");
                //txtDateOfAdmission.Attributes.Add("readonly", "readonly");
                //txtDateOfDischarge.Attributes.Add("readonly", "readonly");
                txtDateofDeath.Attributes.Add("readonly", "readonly");
                txtDateofDischarge.Attributes.Add("readonly", "readonly");
                txtFinalReportSubmitDate.Attributes.Add("readonly", "readonly");
                txtInvestigationDate.Attributes.Add("readonly", "readonly");
                txtRemarkClaim.MaxLength = 5000;
                txtDisDesc.MaxLength = 4000;
                txtHospitalPinCode.MaxLength = 6;
                txtICUDays.MaxLength = 3;
                txtHospitalName.MaxLength = 100;
                txtHospitalName.Attributes.Add("maxlength", txtHospitalName.MaxLength.ToString());
                txtHospitalAddress.MaxLength = 500;
                txtHospitalAddress.Attributes.Add("maxlength", txtHospitalAddress.MaxLength.ToString());

                txtHospitalCity.MaxLength = 15;
                txtHospitalCity.Attributes.Add("maxlength", txtHospitalCity.MaxLength.ToString());

                txtHospitalState.MaxLength = 30;
                txtHospitalState.Attributes.Add("maxlength", txtHospitalState.MaxLength.ToString());

                txtInvestigatorName.MaxLength = 50;
                txtInvestigatorName.Attributes.Add("maxlength", txtInvestigatorName.MaxLength.ToString());


                txtInvestigatorAddress.MaxLength = 100;
                txtInvestigatorAddress.Attributes.Add("maxlength", txtInvestigatorAddress.MaxLength.ToString());


                txtNonICUDays.MaxLength = 3;
                txtRemarkClaim.Attributes.Add("maxlength", txtRemarkClaim.MaxLength.ToString());
                txtDisDesc.Attributes.Add("maxlength", txtDisDesc.MaxLength.ToString());
                txtRemarkClaim.AutoPostBack = false;
                txtDisDesc.AutoPostBack = false;
                txtRemarkClaim.Text.Replace(System.Environment.NewLine, "");
                txtDisDesc.Text.Replace(System.Environment.NewLine, "");
           }
            if (IsPostBack)
            {
                txtRemarkClaim.Text = hftxtRemark.Value.ToString().Trim();
                txtDisDesc.Text = hftxtDiseasDesc.Value.ToString().Trim();
                fnShowControls();
            }
            else
            {
                fnHideControls();
            }
            
        }

        private void fnHideControls()
        {
            dv1.Visible = false; dv2.Visible = false; dv3.Visible = false; dv4.Visible = false; dv5.Visible = false;
            dv6.Visible = false; dv7.Visible = false; dv8.Visible = false; dv9.Visible = false; dv10.Visible = false;
            dv11.Visible = false; dv12.Visible = false; dv13.Visible = false; dv14.Visible = false; dv15.Visible = false;
            dv16.Visible = false; dv17.Visible = false; dv18.Visible = false; dv19.Visible = false; dv20.Visible = false;
            dv21.Visible = false; dv22.Visible = false; dv23.Visible = false; dv24.Visible = false; dv25.Visible = false;
            dv26.Visible = false; dv27.Visible = false; dv28.Visible = false; dv29.Visible = false; dv30.Visible = false;
            dv31.Visible = false; dv32.Visible = false; dv33.Visible = false; dv34.Visible = false; dv35.Visible = false;
            dv36.Visible = false; dv37.Visible = false; dv38.Visible = false; dv39.Visible = false; dv40.Visible = false;
            dv41.Visible = false; dv42.Visible = false; dv43.Visible = false; dv44.Visible = false; dv45.Visible = false;
            dv46.Visible = false; dv47.Visible = false; dv48.Visible = false; dv49.Visible = false; dv50.Visible = false;
            dv51.Visible = false; dv52.Visible = false; dv53.Visible = false; dv54.Visible = false; dv55.Visible = false;
            dv56.Visible = false; dv57.Visible = false; dv58.Visible = false; dv59.Visible = false; dv60.Visible = false;
            dv61.Visible = false; dv62.Visible = false; 
        }


        private void fnShowControls()
        {
            dv1.Visible = true; dv2.Visible = true; dv3.Visible = true; dv4.Visible = true; dv5.Visible = true;
            dv6.Visible = true; dv7.Visible = true; dv8.Visible = true; dv9.Visible = true; dv10.Visible = true;
            dv11.Visible = true; dv12.Visible = true; dv13.Visible = true; dv14.Visible = true; dv15.Visible = true;
            dv16.Visible = true; dv17.Visible = true; dv18.Visible = true; dv19.Visible = true; dv20.Visible = true;
            dv21.Visible = true; dv22.Visible = true; dv23.Visible = true; dv24.Visible = true; dv25.Visible = true;
            dv26.Visible = true; dv27.Visible = true; dv28.Visible = true; dv29.Visible = true; dv30.Visible = true;
            dv31.Visible = true; dv32.Visible = true; dv33.Visible = true; dv34.Visible = true; dv35.Visible = true;
            dv36.Visible = true; dv37.Visible = true; dv38.Visible = true; dv39.Visible = true; dv40.Visible = true;
            dv41.Visible = true; dv42.Visible = true; dv43.Visible = true; dv44.Visible = true; dv45.Visible = true;
            dv46.Visible = true; dv47.Visible = true; dv48.Visible = true; dv49.Visible = true; dv50.Visible = true;
            dv51.Visible = true; dv52.Visible = true; dv53.Visible = true; dv54.Visible = true; dv55.Visible = true;
            dv56.Visible = true; dv57.Visible = true; dv58.Visible = true; dv59.Visible = true; dv60.Visible = true;
            dv61.Visible = true; dv62.Visible = true;
            txtDisDesc.Text = hftxtDiseasDesc.Value.ToString();
            txtRemarkClaim.Text = hftxtRemark.Value.ToString();
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

                    SqlCommand cmd = new SqlCommand("PROC_GET_LEVEL4_ICD_MASTER_DATA", con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    drpIDCch4.DataSource = dt;
                    drpIDCch4.DataBind();
                    drpIDCch4.DataTextField = "DisplayText";
                    drpIDCch4.DataValueField = "Level4No";
                    drpIDCch4.Items.Insert(0, new ListItem("", ""));
                    drpIDCch4.DataBind();



                    cmd = new SqlCommand("PROC_GET_LEVEL3_ICD_MASTER_DATA", con);
                    adp = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adp.Fill(dt);
                    drpIDCch3.DataSource = dt;
                    drpIDCch3.DataBind();
                    drpIDCch3.DataTextField = "DisplayText";
                    drpIDCch3.DataValueField = "Level3No";
                    drpIDCch3.Items.Insert(0, new ListItem("", ""));
                    drpIDCch3.DataBind();

                    cmd = new SqlCommand("PROC_GET_LEVEL2_ICD_MASTER_DATA", con);
                    adp = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adp.Fill(dt);
                    drpIDCch2.DataSource = dt;
                    drpIDCch2.DataBind();
                    drpIDCch2.DataTextField = "DisplayText";
                    drpIDCch2.DataValueField = "Level2No";
                    drpIDCch2.Items.Insert(0, new ListItem("", ""));
                    drpIDCch2.DataBind();


                    cmd = new SqlCommand("PROC_GET_LEVEL1_ICD_DATA", con);
                    adp = new SqlDataAdapter(cmd);
                    dt = new DataTable();
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

        private void ResetControls()
        {
            txtCertificateNumber.Text = string.Empty;
            txtClaimedAmount.Text = string.Empty;
            txtClaimNumberClaim.Text = string.Empty;
            txtCustomerMobileClaim.Text = string.Empty;
            txtCustomerNameclaim.Text = string.Empty;
            txtDateofDischarge.Text = string.Empty;
            txtDisDesc.Text = string.Empty;
            txtExpenseAmount.Text = string.Empty;
            txtRemarkClaim.Text = string.Empty;
            drpIDCch1.SelectedIndex = 0;
            drpIDCch2.SelectedIndex = 0;
            drpIDCch3.SelectedIndex = 0;
            drpIDCch4.SelectedIndex = 0;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }



        protected void btnSearchClaimNumber_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtClaimNumberClaim.Text))
                {
                    Alert.Show("Please enter valid claim number");
                    return;
                }


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("PROC_HDC_CLAIM_DETAILS_BY_CLAIMNUM", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vClaimNumber", txtClaimNumberClaim.Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        //FnGetExistingICDData(txtClaimNumberClaim.Text);
                        while (dr.Read())
                        {
                            txtCustomerMobileClaim.Text = dr["vMobileNo"].ToString();
                            txtCustomerNameclaim.Text = dr["vCustomerName"].ToString();
                            txtCertificateNumber.Text = dr["vCertificateNumber"].ToString();
                            if (!string.IsNullOrEmpty(dr["dDateOfDischarge"].ToString()))
                            {
                                txtDateofDischarge.Text = DateTime.Parse(dr["dDateOfDischarge"].ToString()).ToString("dd/MM/yyyy");
                            }
                            //txtDisDesc.Text = dr["vDiseaseDesc"].ToString().Trim().Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("     ",""); 
                            hftxtDiseasDesc.Value = dr["vDiseaseDesc"].ToString().Trim();
                            txtDisDesc.Text = hftxtDiseasDesc.Value.ToString();
                            txtExpenseAmount.Text = dr["nExpenseAmount"].ToString().Replace(".00","");
                            txtClaimedAmount.Text = dr["dClaimAmount"].ToString().Replace(".00", "");
                            //txtRemarkClaim.Text = dr["vRemark"].ToString().Trim().Replace("\r","").Replace("\n","").Replace("\t","").Replace("     ", "");
                            hftxtRemark.Value = dr["vRemark"].ToString().Trim();
                            txtRemarkClaim.Text = hftxtRemark.Value.ToString();
                            if (!string.IsNullOrEmpty(dr["dDateofDeath"].ToString()))
                            {
                                txtDateofDeath.Text = DateTime.Parse(dr["dDateofDeath"].ToString()).ToString("dd/MM/yyyy");
                            }

                            txtICUDays.Text = dr["nICUdays"].ToString();
                            txtNonICUDays.Text = dr["nNonICUdays"].ToString();
                            txtHospitalName.Text = dr["vHospitalName"].ToString();
                            txtHospitalAddress.Text = dr["vHospitalAddress"].ToString();
                            txtHospitalPinCode.Text = dr["vHospitalPinCode"].ToString();
                            txtHospitalCity.Text = dr["vHospitalCity"].ToString();
                            txtHospitalState.Text = dr["vHospitalState"].ToString();
                            txtInvestigatorName.Text = dr["vInvestigatorName"].ToString();
                            txtInvestigatorAddress.Text = dr["vInvestigatorAddress"].ToString();
                            if (!string.IsNullOrEmpty(dr["dInvestigationDate"].ToString()))
                            {
                                txtInvestigationDate.Text = DateTime.Parse(dr["dInvestigationDate"].ToString()).ToString("dd/MM/yyyy");
                            }
                            if (!string.IsNullOrEmpty(dr["dInvestigationReportSubmitDate"].ToString()))
                            {
                                txtFinalReportSubmitDate.Text = DateTime.Parse(dr["dInvestigationReportSubmitDate"].ToString()).ToString("dd/MM/yyyy");
                            }
                            if (!string.IsNullOrEmpty(dr["dDateofAdmission"].ToString()))
                            {
                                DateofAdmission = DateTime.Parse(dr["dDateofAdmission"].ToString()).ToString("dd/MM/yyyy");
                            }

                            hfPolicyStartDate.Value = DateTime.Parse(dr["vPolicyStartdate"].ToString()).ToString("dd/MM/yyyy");
                            txtCustomerNameclaim.Enabled = false;
                            txtCertificateNumber.Enabled = false;
                            txtClaimNumberClaim.Enabled = false;

                            txtRemarkClaim.Text.Replace(System.Environment.NewLine, "");
                            txtDisDesc.Text.Replace(System.Environment.NewLine, "");

                        }
                        FnGetExistingICDData(txtClaimNumberClaim.Text);
                        fnShowControls();
                    }
                    else
                    {
                        ResetControls();
                        Alert.Show("No Data Found !!");
                        fnHideControls();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, " btnSearchClaimNumber_Click");
                Alert.Show("Some Error Occured. Kindly contact administrator.");

            }
        }

        private void FnGetExistingICDData(string ClaimNumber)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("PRC_GET_HDC_ICD_DEATA_BY_CLAIM_NUMBER", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vClaimNo", ClaimNumber);
                    DataTable dt = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    drpIDCch1.SelectedValue = dt.Rows[0]["vIDCCH1"].ToString();

                    if (string.IsNullOrEmpty(dt.Rows[0]["vIDCCH2"].ToString()))
                    {
                        fnBindICD2Dropdown();
                    }
                    else
                    {
                        fnBindICD2Dropdown();
                        drpIDCch2.SelectedValue = dt.Rows[0]["vIDCCH2"].ToString();
                    }


                    if (string.IsNullOrEmpty(dt.Rows[0]["vIDCCH3"].ToString()))
                    {
                        fnBindICD3Dropdown();
                    }
                    else
                    {
                        fnBindICD3Dropdown();
                        drpIDCch3.SelectedValue = dt.Rows[0]["vIDCCH3"].ToString();
                    }

                    if (string.IsNullOrEmpty(dt.Rows[0]["vIDCCH4"].ToString()))
                    {
                        fnBindICD4Dropdown();
                    }
                    else
                    {
                        fnBindICD4Dropdown();
                        drpIDCch4.SelectedValue = dt.Rows[0]["vIDCCH4"].ToString();
                    }
                    

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void drpIDCch1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindICD2Dropdown();
        }

        private void fnBindICD2Dropdown()
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
                    drpIDCch2.Items.Clear();
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
                    fnShowControls();

                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FnBindICDMasterData");
            }
        }

        protected void drpIDCch2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindICD3Dropdown();
                      
        }

        private void fnBindICD3Dropdown()
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
                    cmd.Parameters.AddWithValue("@level1No", drpIDCch1.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@level2No", drpIDCch2.SelectedValue.ToString());
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    drpIDCch3.Items.Clear();
                    drpIDCch3.DataSource = dt;
                    drpIDCch3.DataBind();
                    drpIDCch3.DataTextField = "DisplayText";
                    drpIDCch3.DataValueField = "Level3No";
                    drpIDCch3.Items.Insert(0, new ListItem("", ""));
                    drpIDCch3.DataBind();

                    drpIDCch4.Items.Clear();
                    drpIDCch4.DataSource = null;
                    drpIDCch4.DataBind();
                    fnShowControls();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnBindICD3Dropdown");
            }
        }

        protected void drpIDCch3_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnBindICD4Dropdown();
                       
        }

        private void fnBindICD4Dropdown()
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
                    drpIDCch4.Items.Clear();
                    drpIDCch4.DataSource = dt;
                    drpIDCch4.DataBind();
                    drpIDCch4.DataTextField = "DisplayText";
                    drpIDCch4.DataValueField = "Level4No";
                    drpIDCch4.Items.Insert(0, new ListItem("", ""));
                    drpIDCch4.DataBind();
                    fnShowControls();
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnBindICD4Dropdown");
            }
        }

        protected void drpIDCch4_SelectedIndexChanged(object sender, EventArgs e)
        {
            fnShowControls();
        }

        protected void btnUpdateClaimDetails_Click(object sender, EventArgs e)
        {
            string DateofDischarge = Request.Form[txtDateofDischarge.UniqueID];
            string DateofDeath = Request.Form[txtDateofDeath.UniqueID];
            string DateofInvestidation = Request.Form[txtInvestigationDate.UniqueID];
            string DateofFinalReportSubmit = Request.Form[txtFinalReportSubmitDate.UniqueID];
            string Remark = Request.Form[txtRemarkClaim.UniqueID];
            string DiseaseDescr = Request.Form[txtDisDesc.UniqueID];


            Regex mregex = new Regex(@"^\d{10}$");
            Regex nRegex = new Regex("^[0-9]*$");

            if (!mregex.IsMatch(txtCustomerMobileClaim.Text))
            {
                Alert.Show("Customer Mobile number must be 10 digit mobile number");
                fnShowControls();
                return;
            }

            if (string.IsNullOrEmpty(DateofDischarge))
            {
                Alert.Show("Discharge Date is mandatory !!");
                fnShowControls();
                return;
            }


            if (drpIDCch1.SelectedValue == "" || drpIDCch2.SelectedValue == "")
            {
                Alert.Show("ICD Chapter Level 1 and 2 are mandatory");
                fnShowControls();
                return;
            }

            if (!string.IsNullOrEmpty(txtClaimedAmount.Text))
            {
                if (txtClaimedAmount.Text.Contains(".") || txtClaimedAmount.Text.Contains("-"))
                {
                    Alert.Show("Negative or Decimal number not allowed in Claimed amount");
                    fnShowControls();
                    return;
                }

                if (!nRegex.IsMatch(txtClaimedAmount.Text))
                {
                    Alert.Show("Only numbers are allowed in Claimed amount");
                    fnShowControls();
                    return;
                }

            }
            else
            {
                Alert.Show("Claimed amount is mandatory !!");
                fnShowControls();
                return;
            }

            if (!string.IsNullOrEmpty(txtClaimedAmount.Text))
            {

                if (txtClaimedAmount.Text.Contains(".") || txtClaimedAmount.Text.Contains("-"))
                {
                    Alert.Show("Negative or Decimal number not allowed in Claimed amount");
                    fnShowControls();
                    return;
                }
                if (!nRegex.IsMatch(txtClaimedAmount.Text))
                {
                    Alert.Show("Only numbers are allowed in Claimed amount");
                    fnShowControls();
                    return;
                }

            }

            if (txtRemarkClaim.Text.Trim().Length > 5000)
            {
                Alert.Show("Remark is valid upto 5000 characters");
                fnShowControls();
                return;
            }


            if (txtDisDesc.Text.Trim().Length > 4000)
            {
                Alert.Show("Disease Description is valid upto 4000 characters");
                fnShowControls();
                return;
            }


            if (!string.IsNullOrEmpty(txtExpenseAmount.Text))
            {

                if (txtExpenseAmount.Text.Contains(".") || txtExpenseAmount.Text.Contains("-"))
                {
                    Alert.Show("Negative or Decimal number not allowed in Expense amount");
                    fnShowControls();
                    return;
                }
                if (!nRegex.IsMatch(txtExpenseAmount.Text))
                {
                    Alert.Show("Only numbers are allowed in Expense amount");
                    fnShowControls();
                    return;
                }

            }
            else
            {
                Alert.Show("Expense amount is mandatory");
                fnShowControls();
                return;
            }


            Regex regexICUDays = new Regex(@"^[0-9]+$");
            if (!regexICUDays.IsMatch(txtICUDays.Text))
            {
                Alert.Show("ICU days must be between 0 to 999");
                fnShowControls();
                return;
            }

            if (!regexICUDays.IsMatch(txtNonICUDays.Text))
            {
                Alert.Show("Non ICU days must be between 0 to 999");
                fnShowControls();
                return;
            }

            if (string.IsNullOrEmpty(txtHospitalName.Text))
            {
                Alert.Show("Hospital Name can not be blank !!");
                fnShowControls();
                return;
            }

            if (!string.IsNullOrEmpty(txtHospitalPinCode.Text.Trim()))
            {
                Regex PinCodeRegex = new Regex("^[1-9][0-9]{5}$");
                if (!PinCodeRegex.IsMatch(txtHospitalPinCode.Text))
                {
                    Alert.Show("Hospital Pin code is not valid !!");
                    fnShowControls();
                    return;
                }

            }

           

            if (!string.IsNullOrEmpty(DateofDischarge))
            {
                if (DateTime.ParseExact(DateofDischarge, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    Alert.Show(string.Format("Date Of Discharge {0} cannot be before the Date Of Admission {1}", DateofDischarge, DateofAdmission));
                    fnShowControls();
                    return;
                }
            }


            if (!String.IsNullOrEmpty(DateofDischarge))
            {

                if (DateTime.ParseExact(DateofDischarge, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    Alert.Show(string.Format("Date Of Discharge {0} cannot be before the Date Of Admission {1}", DateofDischarge, DateofAdmission));
                    fnShowControls();
                    return;
                }
            }


            if (!String.IsNullOrEmpty(hfPolicyStartDate.Value) && !String.IsNullOrEmpty(DateofDeath))
            {

                if (DateTime.ParseExact(DateofDeath, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(hfPolicyStartDate.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    Alert.Show(string.Format("Date Of Death {0} cannot be before the Policy start date {1} ", DateofDeath, hfPolicyStartDate.Value.ToString()));
                    fnShowControls();
                    return;
                }
            }


            if (!String.IsNullOrEmpty(DateofInvestidation))
            {
                if (DateTime.ParseExact(DateofInvestidation, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    Alert.Show(string.Format("Date Of Investigation {0} cannot be before the Date of Admission {1} ", DateofInvestidation, DateofAdmission));
                    fnShowControls();
                    return;
                }
            }

            if (!String.IsNullOrEmpty(DateofInvestidation) && !String.IsNullOrEmpty(DateofFinalReportSubmit))
            {
                if (DateTime.ParseExact(DateofFinalReportSubmit, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    Alert.Show(string.Format("Date Of Final Report Submission {0} cannot be before the Date of Admission {1} ", DateofFinalReportSubmit, DateofAdmission));
                    fnShowControls();
                    return;
                }
            }





            if (!String.IsNullOrEmpty(DateofFinalReportSubmit))
            {
                if (string.IsNullOrEmpty(DateofInvestidation))
                {
                    Alert.Show(string.Format("Investigation date must be selected if selecting Final Report submit date"));
                    fnShowControls();
                    return;
                }

                if (DateTime.ParseExact(DateofFinalReportSubmit, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofAdmission, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    Alert.Show(string.Format("Date Of Final Report Submission {0} cannot be before the Date of Admission {1} ", DateofFinalReportSubmit, DateofAdmission));
                    fnShowControls();
                    return;
                }

                if (DateTime.ParseExact(DateofFinalReportSubmit, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateofInvestidation, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    Alert.Show(string.Format("Date Of Final Report Submission {0} cannot be before the Date of Investigation {1} ", DateofFinalReportSubmit, DateofInvestidation));
                    fnShowControls();
                    return;
                }

            }

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPAss"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("PROC_UPDATE_HDC_CLAIMS_DETAILS", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vClaimNumber", txtClaimNumberClaim.Text);
                        cmd.Parameters.AddWithValue("@vCertificateNumber", txtCertificateNumber.Text);
                        if (!string.IsNullOrEmpty(txtCustomerMobileClaim.Text))
                        {
                            cmd.Parameters.AddWithValue("@vCustomerMobileNumber", txtCustomerMobileClaim.Text);
                        }
                        

                        if (!string.IsNullOrEmpty(drpIDCch1.SelectedValue.ToString()))
                        {
                            cmd.Parameters.AddWithValue("@vICDCh1", drpIDCch1.SelectedValue.ToString());
                        }

                        if (!string.IsNullOrEmpty(drpIDCch2.SelectedValue.ToString()))
                        {
                            cmd.Parameters.AddWithValue("@vICDCh2", drpIDCch2.SelectedValue.ToString());
                        }

                        if (!string.IsNullOrEmpty(drpIDCch3.SelectedValue.ToString()))
                        {
                            cmd.Parameters.AddWithValue("@vICDCh3", drpIDCch3.SelectedValue.ToString());
                        }

                        if (!string.IsNullOrEmpty(drpIDCch4.SelectedValue.ToString()))
                        {
                            cmd.Parameters.AddWithValue("@vICDCh4", drpIDCch4.SelectedValue.ToString());
                        }

                        if (!string.IsNullOrEmpty(txtClaimedAmount.Text))
                        {
                            cmd.Parameters.AddWithValue("@dClaimedAmount", txtClaimedAmount.Text);
                        }

                        if (!string.IsNullOrEmpty(txtExpenseAmount.Text))
                        {
                            cmd.Parameters.AddWithValue("@dExpenseAmount", txtExpenseAmount.Text);
                        }

                        if (!string.IsNullOrEmpty(Remark))
                        {
                            cmd.Parameters.AddWithValue("@vRemarks", Remark);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vRemarks", " ");
                        }

                        if (!string.IsNullOrEmpty(DiseaseDescr))
                        {
                            cmd.Parameters.AddWithValue("@vDiseaseDescription", DiseaseDescr);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vDiseaseDescription", " ");
                        }

                        if (!string.IsNullOrEmpty(txtICUDays.Text))
                        {
                            cmd.Parameters.AddWithValue("@nIcuDay", txtICUDays.Text);
                        }

                        if (!string.IsNullOrEmpty(txtNonICUDays.Text))
                        {
                            cmd.Parameters.AddWithValue("@nNonICUday", txtNonICUDays.Text);
                        }

                        if (!string.IsNullOrEmpty(txtHospitalName.Text))
                        {
                            cmd.Parameters.AddWithValue("@vHospitalName", txtHospitalName.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vHospitalName", " ");
                        }

                        if (!string.IsNullOrEmpty(txtHospitalAddress.Text))
                        {
                            cmd.Parameters.AddWithValue("@vHospitalAddress", txtHospitalAddress.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vHospitalAddress", " ");
                        }


                        if (!string.IsNullOrEmpty(txtHospitalPinCode.Text))
                        {
                            cmd.Parameters.AddWithValue("@vHospitalPinCode", txtHospitalPinCode.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vHospitalPinCode", " ");
                        }

                        if (!string.IsNullOrEmpty(txtHospitalCity.Text))
                        {
                            cmd.Parameters.AddWithValue("@vHospitalCity", txtHospitalCity.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vHospitalCity", " ");
                        }

                        if (!string.IsNullOrEmpty(txtHospitalState.Text))
                        {
                            cmd.Parameters.AddWithValue("@vHospitalState", txtHospitalState.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vHospitalState", " ");
                        }

                        if (!string.IsNullOrEmpty(txtInvestigatorName.Text))
                        {
                            cmd.Parameters.AddWithValue("@vInvestigatorName", txtInvestigatorName.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vInvestigatorName", " ");
                        }


                        if (!string.IsNullOrEmpty(txtInvestigatorAddress.Text))
                        {
                            cmd.Parameters.AddWithValue("@vInvestigatorAddress", txtInvestigatorAddress.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vInvestigatorAddress", " ");
                        }

                        if (!string.IsNullOrEmpty(DateofDischarge))
                        {
                            cmd.Parameters.AddWithValue("@vDateofDischarge",DateofDischarge);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vDateofDischarge", "");
                        }


                        if (!string.IsNullOrEmpty(DateofDeath))
                        {
                            cmd.Parameters.AddWithValue("@dDateofDeath", DateofDeath);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@dDateofDeath", DBNull.Value);
                        }


                        if (!string.IsNullOrEmpty(DateofInvestidation))
                        {
                            cmd.Parameters.AddWithValue("@dInvestigationDate", DateofInvestidation);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@dInvestigationDate", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(DateofFinalReportSubmit))
                        {
                            cmd.Parameters.AddWithValue("@dFinalReportSubmitDate", DateofFinalReportSubmit);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@dFinalReportSubmitDate", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.Parameters.Add("@Result", SqlDbType.VarChar, 800);
                        cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                        int count = cmd.ExecuteNonQuery();
                        string Result = Convert.ToString(cmd.Parameters["@Result"].Value);
                        //Alert.Show("Result " + Result);
                        if (Result.Contains("Fail"))
                        {
                            Alert.Show("Details not updated, Please try again.", "FrmMainMenu.aspx");
                        }
                        else
                        {
                            Alert.Show("Claim Details updated Successfully", "FrmMainMenu.aspx");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "btnUpdateClaimDetails_Click");
                Alert.Show("Some Error Occured. Kindly Contact Administrator.");
            }

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

        //protected void imgClear_Click(object sender, ImageClickEventArgs e)
        //{
        //    clnDateOfDischarge.SelectedDate = new DateTime(0);
        //}
    }
}