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
    public partial class FrmHDCClaimReopen : System.Web.UI.Page
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



                }
                else
                {
                    Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    return;
                }

                Directory.CreateDirectory(folderPath);


                string ReopenDate = DateTime.Now.ToString("dd/MM/yyyy");
                txtReopenDate.Text = ReopenDate;
                txtDateOfAdmission.Attributes.Add("readonly", "readonly");
                txtDateOfDischarge.Attributes.Add("readonly", "readonly");
                txtReopenDate.Attributes.Add("readonly", "readonly");
            }
        }

        private void FnHideDataFields()
        {
            d1.Visible = false; d2.Visible = false; d3.Visible = false; d4.Visible = false; d5.Visible = false;
            d6.Visible = false; d7.Visible = false; d8.Visible = false; d9.Visible = false; d10.Visible = false;
            d11.Visible = false; d12.Visible = false; d13.Visible = false; d14.Visible = false;
            d15.Visible = false; d16.Visible = false; d61.Visible = false; d64.Visible = false;

        }

        private void FnShowDataFields()
        {
            d1.Visible = true; d2.Visible = true; d3.Visible = true; d4.Visible = true; d5.Visible = true;
            d6.Visible = true; d7.Visible = true; d8.Visible = true; d9.Visible = true; d10.Visible = true;
            d11.Visible = true; d12.Visible = true; d13.Visible = true; d14.Visible = true;
            d15.Visible = true; d16.Visible = true; d61.Visible = true; d64.Visible = true;

        }




        protected void btnSearchClaim_Click(object sender, EventArgs e)
        {
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
                        SqlCommand cmd = new SqlCommand("PROC_HDC_CLAIM_DETAILS_FOR_REOPEN", con);
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
                                FnShowDataFields();
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);

                                txtClaimNumber.Text = dtCertificateDetails.Rows[0]["vClaimNumber"].ToString();
                                txtCertNumber.Text = dtCertificateDetails.Rows[0]["vCertificateNumber"].ToString();

                                string DateofDischarge = dtCertificateDetails.Rows[0]["dDateOfDischarge"].ToString();
                                string d = Convert.ToDateTime(DateofDischarge).ToString("dd/MM/yyyy");
                                txtDateOfDischarge.Text = d.ToString();


                                string DateofAdmission = dtCertificateDetails.Rows[0]["dDateOfAdmission"].ToString();
                                d = Convert.ToDateTime(DateofAdmission).ToString("dd/MM/yyyy");
                                txtDateOfAdmission.Text = d.ToString();

                                hdnvTotalPolicySumInsured.Value = dtCertificateDetails.Rows[0]["vTotalPolicySumInsured"].ToString();


                                txtClaimNumber.Attributes.Add("Disabled", "");
                                txtCertNumber.Attributes.Add("Disabled", "");
                                txtDateOfDischarge.Attributes.Add("Disabled", "");
                                txtDateOfAdmission.Attributes.Add("Disabled", "");

                            }

                            else
                            {
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg no data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                                Alert.Show(string.Format("Previous claim settlement was not in {0} for this claim or certificate number!!", "'Claim Withdrawn/CWP' , 'Claim Rejected/Repudiated' "));
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


        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmHDCClaimReopen.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
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
                    Alert.Show("Date of admission can not be blank!!");
                    return;
                }

                if (string.IsNullOrEmpty(txtDateOfDischarge.Text))
                {
                    Alert.Show("Discharge date can not be blank!!");
                    return;
                }

                if (string.IsNullOrEmpty(DrpSettelmentType.SelectedValue))
                {
                    Alert.Show("Reason of Reopening can not be blank");
                    return;
                }

                Regex regex = new Regex(@"[\d]");

                if (!regex.IsMatch(txtClaimedAmount.Text))
                {
                    Alert.Show("Claimed amount is not valid");
                    return;
                }


                if (txtClaimedAmount.Text.Contains("."))
                {
                    Alert.Show("Decimal not allowed in claimed amount");
                    return;
                }

                if (Convert.ToInt32(txtClaimedAmount.Text) < 0)
                {
                    Alert.Show("Claimed amount is not valid");
                    return;
                }




                if (!String.IsNullOrEmpty(txtExpenseAmount.Text))
                {
                    if (!regex.IsMatch(txtExpenseAmount.Text))
                    {
                        Alert.Show("Expense amount is not valid");
                        return;
                    }

                    if (txtExpenseAmount.Text.Contains("."))
                    {
                        Alert.Show("Decimal not allowed in expense amount.");
                        return;
                    }

                    if (Convert.ToInt32(txtExpenseAmount.Text) < 0)
                    {
                        Alert.Show("Expense amount is not valid");
                        return;
                    }
                }
                
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                   // string DateofDischarge = Request.Form[txtDateOfDischarge.UniqueID];
                        
                    using (SqlCommand cmd = new SqlCommand("PROC_INSERT_HDC_CLAIM_REOPEN_TABLE", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vClaimNumber", txtClaimNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@vCertificateNumber", txtCertNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@vDateofAdmission", txtDateOfAdmission.Text);
                        cmd.Parameters.AddWithValue("@vDateofDischarge", txtDateOfDischarge.Text);
                        cmd.Parameters.AddWithValue("@vReasonofReopening", DrpSettelmentType.SelectedValue);
                        cmd.Parameters.AddWithValue("@nClaimedAmount", txtClaimedAmount.Text);
                        cmd.Parameters.AddWithValue("@nExpenseAmount", string.IsNullOrEmpty(txtExpenseAmount.Text) ? "0" : txtExpenseAmount.Text);
                        cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.ExecuteNonQuery();
                        Alert.Show(string.Format("Claim Reopen details for calaim number {0} submitted successfully !!", txtClaimNumber.Text), "FrmHDCClaimReopen.aspx");
                    }



                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "btnSave_Click   on FrmHDCClaimReopen.aspx.cs");
                Alert.Show("Some Error occured, Kindly contact administrator");
            }
        }


        protected void btnExit_Click1(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }


    }

}


