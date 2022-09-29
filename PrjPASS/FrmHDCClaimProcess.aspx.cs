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
    public partial class FrmHDCClaimProcess : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");


        static int PreviousnFinalPayableAmount = 0;
        static int totalPolicySumInsured = 0;

        int FinalApprovedAmt1 = 0;
        int IGST1 = 0; int CGST1 = 0; int SGST1 = 0;
        int UGST1 = 0; int TDSAmt1 = 0; int FinalPayableAmt1 = 0;


        int FinalApprovedAmt2 = 0;
        int IGST2 = 0; int CGST2 = 0; int SGST2 = 0;
        int UGST2 = 0; int TDSAmt2 = 0; int FinalPayableAmt2 = 0;


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

                // clnClaimIntimationDate.DateMax = DateTime.Now;
                //   txtClaimIntimationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //   txtClaimRegistrationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //   txtClaimIntimationDate.Attributes.Add("readonly", "readonly");
                txtDateOfAdmission.Attributes.Add("readonly", "readonly");
                txtDateOfDischarge.Attributes.Add("readonly", "readonly");
                txtFinalPayableAmt.Attributes.Add("readonly", "readonly");
                txtFinalPayableAmt2.Attributes.Add("readonly", "readonly");
            }
        }

        private void FnHideDataFields()
        {
            d1.Visible = false; d2.Visible = false; d3.Visible = false; d4.Visible = false; d5.Visible = false;
            d6.Visible = false; d7.Visible = false; d8.Visible = false; d9.Visible = false; d10.Visible = false;
            d11.Visible = false; d12.Visible = false; tblPaymentProcess.Visible = false;
            clnDateOfAdmission.Visible = false;
            clnDateofDischarge.Visible = false;
        }

        private void FnShowDataFields()
        {
            d1.Visible = true; d2.Visible = true; d3.Visible = true; d4.Visible = true; d5.Visible = true;
            d6.Visible = true; d7.Visible = true; d8.Visible = true; d9.Visible = true; d10.Visible = true;
            d11.Visible = true; d12.Visible = true; tblPaymentProcess.Visible = true;
            clnDateOfAdmission.Visible = true;
            clnDateofDischarge.Visible = true;

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
                        SqlCommand cmd = new SqlCommand("PROC_HDC_CLAIM_DETAILS_FOR_PAYMENT_PROCESS", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCertificateNumber", txtCertificateNumber.Text);
                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg fetching data for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataSet dsHDCPayment = new DataSet();
                        DataTable dtCertificateDetails = new DataTable();
                        sda.Fill(dsHDCPayment);

                        dtCertificateDetails = dsHDCPayment.Tables[0];

                        PreviousnFinalPayableAmount = Convert.ToInt32(dsHDCPayment.Tables[1].Rows[0]["AppliedAmount"].ToString());

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

                                txtPayeeName.Text = dtCertificateDetails.Rows[0]["vMemberName"].ToString();
                                txtPayeeAccNo.Text = dtCertificateDetails.Rows[0]["vAccountNumber"].ToString();
                                DrpPaymentType.SelectedValue = "Indemnity";

                                totalPolicySumInsured = Convert.ToInt32(dtCertificateDetails.Rows[0]["vTotalPolicySumInsured"].ToString());
                                hdnvTotalPolicySumInsured.Value = dtCertificateDetails.Rows[0]["vTotalPolicySumInsured"].ToString();


                                txtClaimNumber.Attributes.Add("Disabled", "");
                                txtCertNumber.Attributes.Add("Disabled", "");
                                txtDateOfDischarge.Attributes.Add("Disabled", "");
                                txtDateOfAdmission.Attributes.Add("Disabled", "");

                            }

                            else
                            {
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimReg no data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                                Alert.Show("No Data Found for this claim or certificate number!!");
                                return;
                            }

                            if (dsHDCPayment.Tables[2].Rows.Count > 0)
                            {
                                d14.Style["top"] = "75%";
                                d13.Visible = true;
                                gvPrevPaymentDetails.DataSource = dsHDCPayment.Tables[2];
                                gvPrevPaymentDetails.DataBind();
                            }
                            else
                            {
                                d13.Visible = false;
                                d14.Style["top"] = "30%";
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

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }







        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmHDCClaimProcess.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Regex PanCardRegex = new Regex(@"[A-Z]{5}\d{4}[A-Z]{1}");
            Regex NumRegex = new Regex("^[0-9]+$");
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
                    Alert.Show("Settlement type can not be blank!!");
                    return;
                }


                List<clsHDCClaimPaymentDetail> PaymentDetails = new List<clsHDCClaimPaymentDetail>();

                clsHDCClaimPaymentDetail objHDCClaimPaymentDetail = new clsHDCClaimPaymentDetail();
                clsHDCClaimPaymentDetail objHDCClaimPaymentDetail2 = new clsHDCClaimPaymentDetail();

                if (chkp1.Checked == true)
                {
                    #region obj 1
                    objHDCClaimPaymentDetail.vCertificateNumber = txtCertNumber.Text.Trim();
                    objHDCClaimPaymentDetail.vClaimNumber = txtClaimNumber.Text.Trim();
                    if (!string.IsNullOrEmpty(txtPayeeName.Text))
                    {
                        objHDCClaimPaymentDetail.vPayeeName = txtPayeeName.Text.Trim();
                    }
                    else
                    {
                        Alert.Show("In Row 1 Payee Name is blank");
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtPayeeAccNo.Text))
                    {
                        if (txtPayeeAccNo.Text.Contains(".") || txtPayeeAccNo.Text.Contains("-"))
                        {
                            Alert.Show("In Row 1 Payee Account number can not contain Decimal or negative sign.");
                            return;
                        }
                        else
                        {
                            objHDCClaimPaymentDetail.vPayeeAccountNumber = txtPayeeAccNo.Text.Trim();
                        }

                    }
                    else
                    {
                        Alert.Show("In Row 1 Payee Account number is blank");
                        return;
                    }


                    if (!string.IsNullOrEmpty(txtIfscCode.Text))
                    {
                        Regex IfscRegex = new Regex("^[A-Za-z]{4}[a-zA-Z0-9]{7}$");

                        if (txtIfscCode.Text.Length > 11)
                        {
                            Alert.Show("In Row 1 IFSC code is more than 11 characters.");
                            return;
                        }
                        else if (!IfscRegex.IsMatch(txtIfscCode.Text.Trim()))
                        {
                            Alert.Show("In Row 1 IFSC code is not valid.");
                            return;
                        }
                        else
                        {
                            objHDCClaimPaymentDetail.vPayeeIFSCCode = txtIfscCode.Text.Trim();
                        }
                    }
                    else
                    {
                        Alert.Show("In Row 1 IFSC code is blank");
                        return;
                    }

                    if (DrpSettelmentType.SelectedValue == "Fully Settled" || DrpSettelmentType.SelectedValue == "Claim Reopen Paid")
                    {
                        if (!string.IsNullOrEmpty(DrpPaymentType.SelectedValue.ToString()))
                        {
                            objHDCClaimPaymentDetail.vPaymentType = DrpPaymentType.SelectedValue.ToString();
                        }
                        else
                        {
                            Alert.Show("In Row 1 Payment type is not selected");
                            return;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(DrpPaymentType.SelectedValue.ToString()))
                        {
                            objHDCClaimPaymentDetail.vPaymentType = DrpPaymentType.SelectedValue.ToString();
                            if (DrpSettelmentType.SelectedValue == "Claim Rejected/Repudiated" || DrpSettelmentType.SelectedValue == "Claim Withdrawn/CWP")
                            {
                                if (objHDCClaimPaymentDetail.vPaymentType.Contains("ind"))
                                {
                                    Alert.Show("Payment type Indemnity not allowed for Claim Rejected/Repudiated and Claim Withdrawn/CWP settlement");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            Alert.Show("In Row 1 payment type 1 not selected.");
                            return;
                        }
                    }

                    if (DrpSettelmentType.SelectedValue == "Fully Settled" || DrpSettelmentType.SelectedValue == "Claim Reopen Paid")
                    {
                        if (!string.IsNullOrEmpty(DrpPaymentMode.SelectedValue))
                        {
                            objHDCClaimPaymentDetail.vPaymentMode = DrpPaymentMode.SelectedValue.ToString();
                        }
                        else
                        {
                            Alert.Show("In Row 1 Payment mode is not selected");
                            return;
                        }
                    }

                    objHDCClaimPaymentDetail.vPaymentMode = DrpPaymentMode.SelectedValue.ToString();

                    if (DrpPaymentMode.SelectedValue == "DD")
                    {
                        if (string.IsNullOrEmpty(txtDDLocation.Text.Trim()))
                        {
                            Alert.Show("In Row 1 DD Location is mandatory as Payment mode selected by DD.");
                            return;
                        }
                        else
                        {
                            objHDCClaimPaymentDetail.vDDLocation = txtDDLocation.Text.Trim();
                        }

                    }
                    else
                    {
                        objHDCClaimPaymentDetail.vDDLocation = "";
                    }

                    if (!string.IsNullOrEmpty(txtPANNumber.Text))
                    {
                        if (!PanCardRegex.IsMatch(txtPANNumber.Text))
                        {
                            Alert.Show("PAN Card number is not valid in first row.");
                            return;
                        }
                        else
                        {
                            objHDCClaimPaymentDetail.vPANNumber = txtPANNumber.Text.Trim();
                        }
                    }


                    if (txtGSTNumber.Text.Contains("."))
                    {
                        Alert.Show("In Row 1 GST number decimal is not allowed.");
                        return;
                    }

                    if (txtInvoiceNumber.Text.Contains("."))
                    {
                        Alert.Show("In Row 1 Invoice number decimal is not allowed");
                        return;
                    }



                    objHDCClaimPaymentDetail.vPANNumber = txtPANNumber.Text.Trim();
                    objHDCClaimPaymentDetail.vGSTNumber = txtGSTNumber.Text.Trim();
                    objHDCClaimPaymentDetail.vInvoiceNumber = txtInvoiceNumber.Text.Trim();

                    string InvoiceDate = txtInvoiceDate.Text;
                    //DateTime date = DateTime.ParseExact(InvoiceDate, "dd-MMM-yyyy", null);
                    objHDCClaimPaymentDetail.vInvoiceDate = InvoiceDate.ToString();


                    if (DrpSettelmentType.SelectedValue == "Fully Settled" || DrpSettelmentType.SelectedValue == "Claim Reopen Paid")
                    {
                        int Approvedamount = Convert.ToInt32(txtFinalApprovedAmt.Text);
                        if (Approvedamount <= 0)
                        {
                            Alert.Show("In row 1 Final approved amount can not be Zero or negative for settelment type Fully Settled/ Claim Reopen Paid ");
                        }
                    }




                    if (string.IsNullOrEmpty(txtFinalApprovedAmt.Text.Trim()))
                    {
                        Alert.Show("In row 1 Final approved amount can not be empty.");
                        return;
                    }

                    if (txtFinalApprovedAmt.Text.Contains("."))
                    {
                        Alert.Show("In row 1 Decimal not allowed in Final approved amount.");
                        return;
                    }

                    if (txtFinalApprovedAmt.Text.Contains("-"))
                    {
                        Alert.Show("In row 1 negative sign not allowed in Final approved amount.");
                        return;
                    }

                    if (txtIGST.Text.Contains("."))
                    {
                        Alert.Show("In row 1 Decimal not allowed in IGST amount.");
                        return;
                    }

                    if (txtIGST.Text.Contains("-"))
                    {
                        Alert.Show("In row 1 negative sign not allowed in IGST amount.");
                        return;
                    }

                    if (txtCGST.Text.Contains("."))
                    {
                        Alert.Show("In row 1 Decimal not allowed in CGST amount.");
                        return;
                    }

                    if (txtCGST.Text.Contains("-"))
                    {
                        Alert.Show("In row 1 negative sign not allowed in CGST amount.");
                        return;
                    }

                    if (txtSGST.Text.Contains("."))
                    {
                        Alert.Show("In row 1 Decimal not allowed in SGST amount.");
                        return;
                    }

                    if (txtSGST.Text.Contains("-"))
                    {
                        Alert.Show("In row 1 negative sign not allowed in SGST amount.");
                        return;
                    }

                    if (txtUGST.Text.Contains("."))
                    {
                        Alert.Show("In row 1 Decimal not allowed in UGST amount.");
                        return;
                    }

                    if (txtUGST.Text.Contains("-"))
                    {
                        Alert.Show("In row 1 negative sign not allowed in UGST amount.");
                        return;
                    }

                    if (txtTDSAmount.Text.Contains("."))
                    {
                        Alert.Show("In row 1 Decimal not allowed in TDSAmount amount.");
                        return;
                    }

                    if (txtTDSAmount.Text.Contains("-"))
                    {
                        Alert.Show("In row 1 negative sign not allowed in TDSAmount amount.");
                        return;
                    }

                    FinalPayableAmt1 = Convert.ToInt32(hdnFinalPayableAmt1.Value.ToString());


                    if (!NumRegex.IsMatch(FinalPayableAmt1.ToString()))
                    {
                        Alert.Show("In row 1 Final Payable Amount is not valid.");
                        return;
                    }

                    if (FinalPayableAmt1.ToString().Contains("."))
                    {
                        Alert.Show("In row 1 Decimal not allowed in Final Payable Amount amount.");
                        return;
                    }

                    if (FinalPayableAmt1.ToString().Contains("-"))
                    {
                        Alert.Show("In row 1 negative sign not allowed in Final Payable Amount amount.");
                        return;
                    }

                    objHDCClaimPaymentDetail.nFinalApprovedAmount = Convert.ToInt32(txtFinalApprovedAmt.Text.Trim());
                    objHDCClaimPaymentDetail.nIGST = string.IsNullOrEmpty(txtIGST.Text) ? 0 : Convert.ToInt32(txtIGST.Text.Trim());
                    objHDCClaimPaymentDetail.nCGST = string.IsNullOrEmpty(txtCGST.Text) ? 0 : Convert.ToInt32(txtCGST.Text.Trim());
                    objHDCClaimPaymentDetail.nSGST = string.IsNullOrEmpty(txtSGST.Text) ? 0 : Convert.ToInt32(txtSGST.Text.Trim());
                    objHDCClaimPaymentDetail.nUGST = string.IsNullOrEmpty(txtUGST.Text) ? 0 : Convert.ToInt32(txtUGST.Text.Trim());
                    objHDCClaimPaymentDetail.nTDSAmount = string.IsNullOrEmpty(txtTDSAmount.Text) ? 0 : Convert.ToInt32(txtTDSAmount.Text.Trim());
                    objHDCClaimPaymentDetail.nFinalPayableAmount = string.IsNullOrEmpty(hdnFinalPayableAmt1.Value.ToString()) ? 0 : Convert.ToInt32(hdnFinalPayableAmt1.Value.ToString());

                    if (DrpPaymentType.SelectedValue.ToString().Contains("ind"))
                    {
                        if (totalPolicySumInsured < (Convert.ToInt32(objHDCClaimPaymentDetail.nFinalPayableAmount) + Convert.ToInt32(PreviousnFinalPayableAmount)))
                        {
                            Alert.Show(string.Format("Sum of Final Payable Amount {0} and Previous paid Amount {1} exceeds Sum Assured of  {2}", objHDCClaimPaymentDetail.nFinalPayableAmount.ToString(), PreviousnFinalPayableAmount.ToString(), totalPolicySumInsured.ToString()));
                            return;
                        }
                    }
                    

                    PaymentDetails.Add(objHDCClaimPaymentDetail);
                    #endregion
                }

                if (chkp2.Checked == true)
                {
                    #region obj 2
                    objHDCClaimPaymentDetail2.vCertificateNumber = txtCertNumber.Text.Trim();
                    objHDCClaimPaymentDetail2.vClaimNumber = txtClaimNumber.Text.Trim();

                    if (!string.IsNullOrEmpty(txtPayeeName2.Text))
                    {
                        objHDCClaimPaymentDetail2.vPayeeName = txtPayeeName2.Text.Trim();
                    }
                    else
                    {
                        Alert.Show("In Row 2 Payee Name is blank");
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtPayeeAccNo2.Text))
                    {
                        if (txtPayeeAccNo2.Text.Contains(".") || txtPayeeAccNo2.Text.Contains("-"))
                        {
                            Alert.Show("In Row 2 Payee Account number can not contain Decimal or negative sign.");
                            return;
                        }
                        else
                        {
                            objHDCClaimPaymentDetail2.vPayeeAccountNumber = txtPayeeAccNo2.Text.Trim();
                        }

                    }
                    else
                    {
                        Alert.Show("In Row 2 Payee Account number is blank");
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtIfscCode2.Text))
                    {
                        Regex IfscRegex = new Regex("^[A-Za-z]{4}[a-zA-Z0-9]{7}$");
                        if (!IfscRegex.IsMatch(txtIfscCode2.Text.Trim()))
                        {
                            Alert.Show("In Row 2 IFSC code is not valid.");
                            return;
                        }
                        else if (txtIfscCode2.Text.Length > 11)
                        {
                            Alert.Show("In Row 2 IFSC code more than 11 character.");
                            return;
                        }
                        else
                        {
                            objHDCClaimPaymentDetail2.vPayeeIFSCCode = txtIfscCode2.Text.Trim();
                        }
                    }
                    else
                    {
                        Alert.Show("In Row 2 IFSC code is blank");
                        return;
                    }


                    if (DrpSettelmentType.SelectedValue == "Fully Settled" || DrpSettelmentType.SelectedValue == "Claim Reopen Paid")
                    {

                        if (!string.IsNullOrEmpty(DrpPaymentType2.SelectedValue.ToString()))
                        {
                            objHDCClaimPaymentDetail2.vPaymentType = DrpPaymentType2.SelectedValue.ToString();
                        }
                        else
                        {
                            Alert.Show("In Row 2 Payment type is not selected");
                            return;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(DrpPaymentType2.SelectedValue.ToString()))
                        {
                            objHDCClaimPaymentDetail2.vPaymentType = DrpPaymentType2.SelectedValue.ToString();
                            if (DrpSettelmentType.SelectedValue == "Claim Rejected/Repudiated" || DrpSettelmentType.SelectedValue == "Claim Withdrawn/CWP")
                            {
                                if (objHDCClaimPaymentDetail2.vPaymentType.Contains("ind"))
                                {
                                    Alert.Show("Payment type Indemnity not allowed for Claim Rejected/Repudiated and Claim Withdrawn/CWP settlement");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            Alert.Show("In Row 2 Payment type is not selected.");
                            return;
                        }
                    }


                    if (DrpSettelmentType.SelectedValue == "Fully Settled" || DrpSettelmentType.SelectedValue == "Claim Reopen Paid")
                    {
                        if (!string.IsNullOrEmpty(DrpPaymentMode2.SelectedValue))
                        {
                            objHDCClaimPaymentDetail2.vPaymentMode = DrpPaymentMode2.SelectedValue.ToString();
                        }
                        else
                        {
                            Alert.Show("In Row 2 Payment mode is not selected");
                            return;
                        }
                    }
                    objHDCClaimPaymentDetail2.vPaymentMode = DrpPaymentMode2.SelectedValue.ToString();

                    if (DrpPaymentMode2.SelectedValue == "DD")
                    {
                        if (string.IsNullOrEmpty(txtDDLocation2.Text.Trim()))
                        {
                            Alert.Show("In Row 2 DD Location is mandatory as Payment mode selected is DD.");
                            return;
                        }
                        else
                        {
                            objHDCClaimPaymentDetail2.vDDLocation = txtDDLocation2.Text.Trim();
                        }

                    }
                    else
                    {
                        objHDCClaimPaymentDetail2.vDDLocation = "";
                    }

                    if (!string.IsNullOrEmpty(txtPANNumber2.Text))
                    {
                        if (!PanCardRegex.IsMatch(txtPANNumber2.Text))
                        {
                            Alert.Show("PAN Card number is not valid in second row.");
                            return;
                        }
                        else
                        {
                            objHDCClaimPaymentDetail2.vPANNumber = txtPANNumber2.Text.Trim();
                        }
                    }

                    if (txtGSTNumber2.Text.Contains("."))
                    {
                        Alert.Show("In Row 2 GST number decimal is not allowed.");
                        return;
                    }

                    if (txtInvoiceNumber2.Text.Contains("."))
                    {
                        Alert.Show("In Row 2 Invoice number decimal is not allowed");
                        return;
                    }


                    objHDCClaimPaymentDetail2.vPANNumber = txtPANNumber2.Text.Trim();
                    objHDCClaimPaymentDetail2.vGSTNumber = txtGSTNumber2.Text.Trim();
                    objHDCClaimPaymentDetail2.vInvoiceNumber = txtInvoiceNumber2.Text.Trim();

                    string InvoiceDate = txtInvoiceDate2.Text;
                    //DateTime date = DateTime.ParseExact(InvoiceDate, "dd-MMM-yyyy", null);
                    objHDCClaimPaymentDetail2.vInvoiceDate = InvoiceDate.ToString();

                    if (DrpSettelmentType.SelectedValue == "Fully Settled" || DrpSettelmentType.SelectedValue == "Claim Reopen Paid")
                    {
                        int Approvedamount = Convert.ToInt32(txtFinalApprovedAmt2.Text);
                        if (Approvedamount <= 0)
                        {
                            Alert.Show("In row 2 Final approved amount can not be Zero or negative for settelment type Fully Settled/ Claim Reopen Paid ");
                        }
                    }

                    if (string.IsNullOrEmpty(txtFinalApprovedAmt2.Text.Trim()))
                    {
                        Alert.Show("In row 2 Final approved amount can not be empty.");
                        return;
                    }

                    if (txtFinalApprovedAmt2.Text.Contains("."))
                    {
                        Alert.Show("In row 2 Final approved amount Decimal is not allowed.");
                        return;
                    }

                    if (txtFinalApprovedAmt2.Text.Contains("-"))
                    {
                        Alert.Show("In row 2 Final approved amount negative is not allowed.");
                        return;
                    }

                    objHDCClaimPaymentDetail2.nFinalApprovedAmount = string.IsNullOrEmpty(txtFinalApprovedAmt2.Text) ? 0 : Convert.ToInt32(txtFinalApprovedAmt2.Text.Trim());



                    if (txtIGST2.Text.Contains("."))
                    {
                        Alert.Show("In row 2 Decimal not allowed in IGST amount.");
                        return;
                    }

                    if (txtIGST2.Text.Contains("-"))
                    {
                        Alert.Show("In row 2 negative sign not allowed in IGST amount.");
                        return;
                    }

                    if (txtCGST2.Text.Contains("."))
                    {
                        Alert.Show("In row 2 Decimal not allowed in CGST amount.");
                        return;
                    }

                    if (txtCGST2.Text.Contains("-"))
                    {
                        Alert.Show("In row 2 negative sign not allowed in CGST amount.");
                        return;
                    }


                    if (txtSGST2.Text.Contains("."))
                    {
                        Alert.Show("In row 2 Decimal not allowed in SGST amount.");
                        return;
                    }

                    if (txtSGST2.Text.Contains("-"))
                    {
                        Alert.Show("In row 2 negative sign not allowed in SGST amount.");
                        return;
                    }


                    if (txtUGST2.Text.Contains("."))
                    {
                        Alert.Show("In row 2 Decimal not allowed in UGST amount.");
                        return;
                    }

                    if (txtUGST2.Text.Contains("-"))
                    {
                        Alert.Show("In row 2 negative sign not allowed in SGST amount.");
                        return;
                    }

                    FinalPayableAmt2 = Convert.ToInt32(hdnFinalPayableAmt2.Value.ToString());
                    if (!NumRegex.IsMatch(FinalPayableAmt2.ToString()))
                    {
                        Alert.Show("In row 2 Final Payable Amount is not valid.");
                        return;
                    }


                    if (FinalPayableAmt2.ToString().Contains("."))
                    {
                        Alert.Show("In row 2 Decimal not allowed in Final Payable Amount amount.");
                        return;
                    }

                    if (FinalPayableAmt2.ToString().Contains("-"))
                    {
                        Alert.Show("In row 2 negative sign not allowed in Final Payable Amount amount.");
                        return;
                    }


                    objHDCClaimPaymentDetail2.nIGST = string.IsNullOrEmpty(txtIGST2.Text) ? 0 : Convert.ToInt32(txtIGST2.Text.Trim());
                    objHDCClaimPaymentDetail2.nCGST = string.IsNullOrEmpty(txtCGST2.Text) ? 0 : Convert.ToInt32(txtCGST2.Text.Trim());
                    objHDCClaimPaymentDetail2.nSGST = string.IsNullOrEmpty(txtSGST2.Text) ? 0 : Convert.ToInt32(txtSGST2.Text.Trim());
                    objHDCClaimPaymentDetail2.nUGST = string.IsNullOrEmpty(txtUGST2.Text) ? 0 : Convert.ToInt32(txtUGST2.Text.Trim());
                    objHDCClaimPaymentDetail2.nTDSAmount = string.IsNullOrEmpty(txtTDSAmount2.Text) ? 0 : Convert.ToInt32(txtTDSAmount2.Text.Trim());
                    objHDCClaimPaymentDetail2.nFinalPayableAmount = string.IsNullOrEmpty(hdnFinalPayableAmt2.Value.ToString()) ? 0 : Convert.ToInt32(hdnFinalPayableAmt2.Value.ToString());
                    
                    if (objHDCClaimPaymentDetail2.vPaymentType.Contains("ind"))
                    {
                        if (totalPolicySumInsured < (Convert.ToInt32(objHDCClaimPaymentDetail2.nFinalPayableAmount) + Convert.ToInt32(PreviousnFinalPayableAmount)))
                        {
                            Alert.Show(string.Format("Previous paid claim amount is {0} . Final Payment amount entered in indemnity grid is  {1}  exceeds policy Sum Insured {2}", PreviousnFinalPayableAmount.ToString() , objHDCClaimPaymentDetail2.nFinalPayableAmount.ToString(), totalPolicySumInsured.ToString()));
                            return;
                        }
                    }
                    
                    PaymentDetails.Add(objHDCClaimPaymentDetail2);

                    #endregion
                }


                if (objHDCClaimPaymentDetail.vPaymentType == "Indemnity")
                {
                    int nFinalApprovedAmount = Convert.ToInt32(objHDCClaimPaymentDetail.nFinalApprovedAmount);
                    int nTotalPolicySumInsured = Convert.ToInt32(hdnvTotalPolicySumInsured.Value.ToString());
                    if (nFinalApprovedAmount > nTotalPolicySumInsured)
                    {
                        Alert.Show("For indemnity payment Approved amount can not be greater than Policy sum assured !");
                        return;
                    }
                }


                if (objHDCClaimPaymentDetail2.vPaymentType == "Indemnity")
                {
                    int nFinalApprovedAmount = Convert.ToInt32(objHDCClaimPaymentDetail2.nFinalApprovedAmount);
                    int nTotalPolicySumInsured = Convert.ToInt32(hdnvTotalPolicySumInsured.Value.ToString());
                    if (nFinalApprovedAmount > nTotalPolicySumInsured)
                    {
                        Alert.Show("For indemnity payment Approved amount can not be greater than Policy sum assured !");
                        return;
                    }
                }


                if (!(DrpSettelmentType.SelectedValue == "Fully Settled") && !(DrpSettelmentType.SelectedValue == "Claim Reopen Paid"))
                {
                    if (DrpPaymentType.SelectedValue == "Indemnity" && DrpPaymentType2.SelectedValue == "Indemnity")
                    {
                        Alert.Show("If Settlement Type is not Fully Settled or Reopen paid then only Expense payment is allowed  !");
                        return;
                    }
                    else
                    {
                        fnSavePaymentProcessDetails(PaymentDetails);
                    }
                }
                else
                {
                    fnSavePaymentProcessDetails(PaymentDetails);
                }

                //if (DrpSettelmentType.SelectedValue != "Claim Reopen Paid")
                //{
                //    if (fnCheckPreviousPaymentDetails(PaymentDetails))
                //    {
                //        Alert.Show(string.Format("Payment entry already done for Claim number {0} ", txtCertificateNumber.Text));
                //    }
                //    else
                //    {
                //        fnSavePaymentProcessDetails(PaymentDetails);

                //    }
                //}
                //else
                //{
                //fnSavePaymentProcessDetails(PaymentDetails);
                //}

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "btnSave_Click   on FrmHDCClaimProcess.aspx.cs");
            }
        }

        private bool fnCheckPreviousPaymentDetails(List<clsHDCClaimPaymentDetail> paymentDetails)
        {
            bool res = false;
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    SqlCommand cmd = new SqlCommand("PROC_HDC_CLAIM_PAYMENT_DETAILS", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vClaimNumber", txtClaimNumber.Text.Trim());
                    SqlDataReader dr = cmd.ExecuteReader();


                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string PaymentType = dr["vPaymentType"].ToString();

                            foreach (clsHDCClaimPaymentDetail pd in paymentDetails)
                            {
                                if (pd.vPaymentType == PaymentType)
                                {
                                    Alert.Show(string.Format("{0} payment already done for claim number {1} .", PaymentType, pd.vClaimNumber));
                                    res = false;
                                }
                                else
                                {
                                    res = true;
                                }
                            }
                        }
                    }

                }
                return res;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnCheckPreviousPaymentDetails   ");
            }
            return res;
        }



        private void fnSavePaymentProcessDetails(List<clsHDCClaimPaymentDetail> paymentDetails)
        {
            string paymentType = string.Empty;
            try
            {
                foreach (clsHDCClaimPaymentDetail pd in paymentDetails)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        SqlCommand cmd = new SqlCommand("PROC_INSERT_TBL_HDC_PAYMENT_PROCESS", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCertificateNumber", pd.vCertificateNumber.Trim());
                        cmd.Parameters.AddWithValue("@vClaimNumber", pd.vClaimNumber.Trim());
                        cmd.Parameters.AddWithValue("@vPayeeName", pd.vPayeeName.Trim());
                        cmd.Parameters.AddWithValue("@vPayeeAccountNumber", pd.vPayeeAccountNumber.Trim());
                        cmd.Parameters.AddWithValue("@vPayeeIFSCCode", pd.vPayeeIFSCCode.Trim());
                        cmd.Parameters.AddWithValue("@vPaymentType", pd.vPaymentType.Trim());
                        cmd.Parameters.AddWithValue("@vPaymentMode", pd.vPaymentMode.Trim());
                        cmd.Parameters.AddWithValue("@vDDLocation", !string.IsNullOrEmpty(pd.vDDLocation.Trim()) ? pd.vDDLocation.Trim() : "");
                        cmd.Parameters.AddWithValue("@vPANNumber", !string.IsNullOrEmpty(pd.vPANNumber.Trim()) ? pd.vPANNumber.Trim() : "");
                        cmd.Parameters.AddWithValue("@vGSTNumber", !string.IsNullOrEmpty(pd.vGSTNumber.Trim()) ? pd.vGSTNumber.Trim() : "");
                        cmd.Parameters.AddWithValue("@vInvoiceNumber", !string.IsNullOrEmpty(pd.vInvoiceNumber.Trim()) ? pd.vInvoiceNumber.Trim() : "");
                        cmd.Parameters.AddWithValue("@vInvoiceDate", !string.IsNullOrEmpty(pd.vInvoiceDate.Trim()) ? pd.vInvoiceDate.Trim() : "");
                        cmd.Parameters.AddWithValue("@nFinalApprovedAmount", pd.nFinalApprovedAmount.ToString());
                        cmd.Parameters.AddWithValue("@nIGST", pd.nIGST);
                        cmd.Parameters.AddWithValue("@nCGST", pd.nCGST);
                        cmd.Parameters.AddWithValue("@nSGST", pd.nSGST);
                        cmd.Parameters.AddWithValue("@nUGST", pd.nUGST);
                        cmd.Parameters.AddWithValue("@nTDSAmount", pd.nTDSAmount);
                        cmd.Parameters.AddWithValue("@nFinalPayableAmount", pd.nFinalPayableAmount);
                        cmd.Parameters.AddWithValue("@vSettlementType", DrpSettelmentType.SelectedValue);
                        cmd.Parameters.AddWithValue("@ClaimType", txtClaimType.Text);
                        cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.ExecuteNonQuery();
                    }

                    paymentType = paymentType + " , " + pd.vPaymentType;
                    paymentType.Remove(0, 3);
                }

                Alert.Show(string.Format("Payment details saved for Claim number {0} and Payment type {1}", txtCertificateNumber.Text, paymentType), "FrmHDCClaimProcess.aspx");

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fnSavePaymentProcessDetails  ");
                Alert.Show("Some error occured. Kindly contact Adinistrator.");
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
            string pincode = hdnPinCode.Value;

        }



        protected void btnExit_Click1(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void chkp1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkp1.Checked == true)
            {
                txtPayeeName.Enabled = true;
                txtPayeeAccNo.Enabled = true;
                txtIfscCode.Enabled = true;
                DrpPaymentType.Enabled = true;
                DrpPaymentMode.Enabled = true;
                txtDDLocation.Enabled = true;
                txtPANNumber.Enabled = true;
                txtGSTNumber.Enabled = true;
                txtInvoiceNumber.Enabled = true;
                txtInvoiceDate.Enabled = true;
                txtFinalApprovedAmt.Enabled = true;
                txtIGST.Enabled = true;
                txtCGST.Enabled = true;
                txtSGST.Enabled = true;
                txtUGST.Enabled = true;
                txtTDSAmount.Enabled = true;
            }
            else
            {
                txtPayeeName.Enabled = false;
                txtPayeeAccNo.Enabled = false;
                txtIfscCode.Enabled = false;
                DrpPaymentType.Enabled = false;
                DrpPaymentMode.Enabled = false;
                txtDDLocation.Enabled = false;
                txtPANNumber.Enabled = false;
                txtGSTNumber.Enabled = false;
                txtInvoiceNumber.Enabled = false;
                txtInvoiceDate.Enabled = false;
                txtFinalApprovedAmt.Enabled = false;
                txtIGST.Enabled = false;
                txtCGST.Enabled = false;
                txtSGST.Enabled = false;
                txtUGST.Enabled = false;
                txtTDSAmount.Enabled = false;
                txtFinalPayableAmt.Enabled = false;
            }
        }

        protected void chkp2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkp2.Checked == true)
            {
                txtPayeeName2.Enabled = true;
                txtPayeeAccNo2.Enabled = true;
                txtIfscCode2.Enabled = true;
                DrpPaymentType2.Enabled = true;
                DrpPaymentMode2.Enabled = true;
                txtDDLocation2.Enabled = true;
                txtPANNumber2.Enabled = true;
                txtGSTNumber2.Enabled = true;
                txtInvoiceNumber2.Enabled = true;
                txtInvoiceDate2.Enabled = true;
                txtFinalApprovedAmt2.Enabled = true;
                txtIGST2.Enabled = true;
                txtCGST2.Enabled = true;
                txtSGST2.Enabled = true;
                txtUGST2.Enabled = true;
                txtTDSAmount2.Enabled = true;

            }
            else
            {

                txtPayeeName2.Enabled = false;
                txtPayeeAccNo2.Enabled = false;
                txtIfscCode2.Enabled = false;
                DrpPaymentType2.Enabled = false;
                DrpPaymentMode2.Enabled = false;
                txtDDLocation2.Enabled = false;
                txtPANNumber2.Enabled = false;
                txtGSTNumber2.Enabled = false;
                txtInvoiceNumber2.Enabled = false;
                txtInvoiceDate2.Enabled = false;
                txtFinalApprovedAmt2.Enabled = false;
                txtIGST2.Enabled = false;
                txtCGST2.Enabled = false;
                txtSGST2.Enabled = false;
                txtUGST2.Enabled = false;
                txtTDSAmount2.Enabled = false;
                txtFinalPayableAmt2.Enabled = false;
            }



        }

        protected void DrpPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string PaymentType1 = DrpPaymentType.SelectedValue.ToString().Trim();
            string PaymentType2 = DrpPaymentType2.SelectedValue.ToString().Trim();
            if (!string.IsNullOrEmpty(PaymentType1) && !string.IsNullOrEmpty(PaymentType2))
            {
                if (PaymentType1 == PaymentType2)
                {
                    Alert.Show(string.Format("Payment type {0} selected already. ", DrpPaymentType.SelectedValue.ToString()));
                    DrpPaymentType.SelectedValue = "";
                    return;
                }
            }
        }

        protected void DrpPaymentType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string PaymentType1 = DrpPaymentType.SelectedValue.ToString().Trim();
            string PaymentType2 = DrpPaymentType2.SelectedValue.ToString().Trim();


            if (!string.IsNullOrEmpty(PaymentType1) && !string.IsNullOrEmpty(PaymentType2))
            {
                if (PaymentType1 == PaymentType2)
                {
                    Alert.Show(string.Format("Payment type {0} selected already", DrpPaymentType2.SelectedValue.ToString()));
                    DrpPaymentType2.SelectedValue = "";
                    return;
                }
            }
        }

        protected void txtFinalApprovedAmt_TextChanged(object sender, EventArgs e)
        {
            calculateRow1FinalPayable();
        }

        private void calculateRow1FinalPayable()
        {
            try
            {
                FinalApprovedAmt1 = Convert.ToInt32(txtFinalApprovedAmt.Text);
                IGST1 = Convert.ToInt32(txtIGST.Text);
                CGST1 = Convert.ToInt32(txtCGST.Text);
                SGST1 = Convert.ToInt32(txtSGST.Text);
                UGST1 = Convert.ToInt32(txtUGST.Text);
                TDSAmt1 = Convert.ToInt32(txtUGST.Text);
                FinalPayableAmt1 = (FinalApprovedAmt1 + IGST1 + CGST1 + SGST1 + UGST1) - TDSAmt1;
                hdnFinalPayableAmt1.Value = FinalPayableAmt1.ToString();
                txtFinalPayableAmt.Text = Convert.ToString(FinalPayableAmt1.ToString());
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "calculateRow1FinalPayable");
            }
        }

        protected void txtIGST_TextChanged(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// Class holds Payment details for the HDC payment entry details
    /// </summary>
    public class clsHDCClaimPaymentDetail
    {
        public string vCertificateNumber { get; set; }
        public string vClaimNumber { get; set; }
        public string vPayeeName { get; set; }
        public string vPayeeAccountNumber { get; set; }
        public string vPayeeIFSCCode { get; set; }
        public string vPaymentType { get; set; }
        public string vPaymentMode { get; set; }
        public string vDDLocation { get; set; }
        public string vPANNumber { get; set; }
        public string vGSTNumber { get; set; }
        public string vInvoiceNumber { get; set; }
        public string vInvoiceDate { get; set; }
        public decimal nFinalApprovedAmount { get; set; }
        public decimal nIGST { get; set; }
        public decimal nCGST { get; set; }
        public decimal nSGST { get; set; }
        public decimal nUGST { get; set; }
        public decimal nTDSAmount { get; set; }
        public decimal nFinalPayableAmount { get; set; }

    }

}