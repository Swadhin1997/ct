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
    public partial class FrmHDCClaimStatus : System.Web.UI.Page
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
            }
        }



        private void FnHideDataFields()
        {

        }

        private void FnShowDataFields()
        {


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
                        SqlCommand cmd = new SqlCommand("PROC_GET_HDC_CLAIM_STATUS", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VSearchNo", txtCertificateNumber.Text);
                        File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimStatus fetching data for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
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
                                //FnShowDataFields();
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimStatus data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                                gvPaymentDetails.DataSource = dtCertificateDetails;
                                gvPaymentDetails.DataBind();
                            }

                            else
                            {
                                File.AppendAllText(folderPath + "\\log.txt", "FrmHDCClaimStatusno data fetched for : " + txtCertificateNumber.Text + " " + DateTime.Now + Environment.NewLine);
                                Alert.Show("No Payment Data Found for this claim or certificate number!!");
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

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmHDCClaimStatus.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }


        protected void btnExit_Click1(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }

}