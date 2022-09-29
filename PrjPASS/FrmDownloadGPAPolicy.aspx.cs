using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Winnovative;
using cryptoPDF.api;


namespace PrjPASS
{
    [System.Runtime.InteropServices.Guid("41A52E12-FEE7-4002-9D25-0E3C2DC2C4DB")]
    public partial class FrmDownloadGPAPolicy : System.Web.UI.Page
    {
        public string certificateNo = string.Empty;
        public string customerName = string.Empty;
        public string accountNumber = string.Empty;
        public string dob = string.Empty;
        string folderPath = ConfigurationManager.AppSettings["pdf_path"].ToString() + DateTime.Now.ToString("dd-MMM-yyyy");
        string folderPath_NonEmail = ConfigurationManager.AppSettings["pdf_nonemailpath"].ToString() + DateTime.Now.ToString("dd-MMM-yyyy");


        bool IsWithoutHeaderFooter = false;

        public string logfile = "log_gpadownload_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGetPolicy_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = 0;
                lblError.Text = string.Empty;

                if (!String.IsNullOrEmpty(txtPolicyNumber.Text))
                {
                    cnt++;
                }

                if (!String.IsNullOrEmpty(txtCRN.Text))
                {
                    cnt++;
                }

                if (!String.IsNullOrEmpty(txtdate.Value))
                {
                    cnt++;
                }

                if (!String.IsNullOrEmpty(txtAccountNumber.Text))
                {
                    cnt++;
                }

                if (cnt < 2)
                {
                    if (RadioGPAPolicy.Checked == true)
                    {
                        lblError.Text = "Enter any two fields to download GPA policy";
                    }
                    else if (RadioHDCPolicy.Checked == true)
                    {
                        lblError.Text = "Enter any two fields to download Kotak Group Smart Cash policy";
                    }
                    else
                    {
                        lblError.Text = "Enter any two fields to download GHI policy";
                    }

                    return;
                }

                //if(String.IsNullOrEmpty(txtdate.Value))
                //{
                //    lblError.Text = "Please enter Date Of Birth";
                //    return;
                //}

                if (!String.IsNullOrEmpty(txtdate.Value))
                {
                    dob = Convert.ToDateTime(txtdate.Value).ToString("dd/MM/yyyy");
                }

                if (RadioGPAPolicy.Checked == false && RadioHDCPolicy.Checked == false && RadioGHIPolicy.Checked == false)
                {
                    lblError.Text = "Please select Policy Type.";
                    return;
                }

                string crn = txtCRN.Text;
                certificateNo = txtPolicyNumber.Text;
                accountNumber = txtAccountNumber.Text;
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string prod_name = string.Empty;


                if (!string.IsNullOrEmpty(txtPolicyNumber.Text) && RadioHDCPolicy.Checked)
                {
                    #region create hdc certificate if policy number available

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    if (!Directory.Exists(folderPath_NonEmail))
                    {
                        Directory.CreateDirectory(folderPath_NonEmail);
                    }

                    // CR 176
                    using (SqlConnection sqlConHDC = new SqlConnection(db.ConnectionString))
                    {
                        //using (SqlCommand cmd = new SqlCommand())
                        {
                            // cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_HDC_POLICY_TABLE where vUploadId not like '%can%' and vCertificateNo=" + "'" + certificateNo + "' order by dCreatedDate desc";
                            // cmd.CommandText = "SELECT top 1 vCertificateNo from tbl_hdc_policy_replica_table where vUploadId not like '%can%' and vCertificateNo=" + "'" + certificateNo + "' order by dCreatedDate desc";
                            //cmd.CommandText = "SELECT top 1 vCertificateNo from tbl_hdc_policy_replica_table where vUploadId not like '%can%' and vCertificateNo=" + "'" + certificateNo + "' order by dCreatedDate desc UNION ALL SELECT top 1 vCertificateNo from tbl_hdc_policy_table where vUploadId not like '%can%' and vCertificateNo = " + "'" + certificateNo + "' order by dCreatedDate desc AND NOT EXISTS(SELECT top 1 vCertificateNo from tbl_hdc_policy_replica_table where vUploadId not like '%can%' and vCertificateNo = " + "'" + certificateNo + "' order by dCreatedDate desc)";
                            //cmd.Connection = sqlConHDC;
                            sqlConHDC.Open();
                            SqlCommand cmd = new SqlCommand("PROC_GET_REPLICA_HDC_CERTIFICATENO", sqlConHDC);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@vCertificateNo", txtPolicyNumber.Text.Trim());
                            object objProd = cmd.ExecuteScalar();
                            certificateNo = Convert.ToString(objProd);
                            sqlConHDC.Close();
                            if (string.IsNullOrWhiteSpace(certificateNo))
                            {
                                //lblError.Text = "No data found for the parameters entered";
                                //return;
                                GetCertificateFromGIST(txtPolicyNumber.Text.Trim());
                                return;
                            }
                            else
                            {
                                createHDCPDF(certificateNo);
                            }
                        }
                    }

                    #endregion  

                }

                if (string.IsNullOrEmpty(txtPolicyNumber.Text) && RadioHDCPolicy.Checked)
                {
                    #region create hdc certificate if policy number is empty
                    using (SqlConnection sqlConHDC = new SqlConnection(db.ConnectionString))
                    {
                       // using (SqlCommand cmd = new SqlCommand())
                        {

                            if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(dob))
                            {
                                //cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_HDC_POLICY_TABLE where "
                                //cmd.CommandText = "SELECT top 1 vCertificateNo from tbl_hdc_policy_replica_table where "
                                // + "vUploadId not like '%can%'  and vCrnNo = " + "'" + crn + "' and "
                                // + "convert(varchar, cast(vCustomerDob as date), 101) ="
                                // + "convert(varchar, cast(substring('" + dob + "', 4, 2) + '/' + substring('" + dob + "', 0, 3) + "
                                // + "'/' + substring('" + dob + "', 7, 4) as date), 101)"
                                // + " order by dCreatedDate desc";
                                SqlCommand cmd = new SqlCommand("GET_REPLICA_HDC_DOB_CRN", sqlConHDC);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@vCrnNo", txtCRN.Text.Trim());
                                cmd.Parameters.AddWithValue("@vCustomerDob", txtdate);
                                sqlConHDC.Open();
                                object objProd = cmd.ExecuteScalar();
                                certificateNo = Convert.ToString(objProd);
                                sqlConHDC.Close();
                                createHDCPDF(certificateNo);
                                
                            }

                            else if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(accountNumber))
                            {
                                //cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_HDC_POLICY_TABLE where vUploadId not like '%can%'  and vCrnNo=" + "'" + crn + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                                // cmd.CommandText = "SELECT top 1 vCertificateNo from tbl_hdc_policy_replica_table where vUploadId not like '%can%'  and vCrnNo=" + "'" + crn + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                                SqlCommand cmd = new SqlCommand("GET_REPLICA_HDC_ACCOUNTNO_CRN", sqlConHDC);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@vCrnNo", txtCRN.Text.Trim());
                                cmd.Parameters.AddWithValue("@vAccountNo", txtAccountNumber.Text.Trim());
                                sqlConHDC.Open();
                                object objProd = cmd.ExecuteScalar();
                                certificateNo = Convert.ToString(objProd);
                                sqlConHDC.Close();
                                createHDCPDF(certificateNo);


                            }

                            //else if (!String.IsNullOrEmpty(dob) && !String.IsNullOrEmpty(accountNumber))
                            else
                            {
                                //cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_HDC_POLICY_TABLE where vUploadId not like '%can%'  and " +
                                //cmd.CommandText = "SELECT top 1 vCertificateNo from tbl_hdc_policy_replica_table where vUploadId not like '%can%'  and " +
                                //"convert(varchar, cast(vCustomerDob as date), 101) = "
                                //+ "convert(varchar, cast(substring('" + dob + "', 4, 2) + '/' + substring('" + dob + "', 0, 3) + "
                                //+ "'/' + substring('" + dob + "', 7, 4) as date), 101) and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                                SqlCommand cmd = new SqlCommand("PROC_GET_REPLICA_HDC_CERTIFICATE_BY_DOB_LOAN_ACCOUNT_NUMBER", sqlConHDC);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@vCustomerDob", txtdate);
                                cmd.Parameters.AddWithValue("@vAccountNo", txtAccountNumber.Text.Trim());
                                sqlConHDC.Open();
                                object objProd = cmd.ExecuteScalar();
                                certificateNo = Convert.ToString(objProd);
                                sqlConHDC.Close();
                                createHDCPDF(certificateNo);
                            }

                            //cmd.Connection = sqlConHDC;
                            //sqlConHDC.Open();
                            //object objProd = cmd.ExecuteScalar();
                            //certificateNo = Convert.ToString(objProd);
                            //sqlConHDC.Close();
                            //createHDCPDF(certificateNo);
                        }
                    }
                    #endregion

                }

                if (!string.IsNullOrEmpty(txtPolicyNumber.Text) && RadioGHIPolicy.Checked)
                {
                    #region create hdc certificate if policy number available

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    if (!Directory.Exists(folderPath_NonEmail))
                    {
                        Directory.CreateDirectory(folderPath_NonEmail);
                    }

                    // CR 176
                    using (SqlConnection sqlConHDC = new SqlConnection(db.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GHI_POLICY_DATA where vUploadId not like '%can%' and vCertificateNo=" + "'" + certificateNo + "' order by dCreatedDate desc";
                            cmd.Connection = sqlConHDC;
                            sqlConHDC.Open();
                            object objProd = cmd.ExecuteScalar();
                            certificateNo = Convert.ToString(objProd);
                            sqlConHDC.Close();
                            if (string.IsNullOrWhiteSpace(certificateNo))
                            {
                                lblError.Text = "No data found for the parameters entered";
                                return;
                            }
                            else
                            {
                                createGHIPDF(certificateNo);
                            }
                        }
                    }

                    #endregion  

                }

                if (string.IsNullOrEmpty(txtPolicyNumber.Text) && RadioGHIPolicy.Checked)
                {
                    #region create hdc certificate if policy number is empty
                    using (SqlConnection sqlConHDC = new SqlConnection(db.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {

                            if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(dob))
                            {
                                cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GHI_POLICY_DATA where "
                                + "vUploadId not like '%can%'  and vCrnNo = " + "'" + crn + "' and "
                                + "convert(varchar, cast(vCustomerDob as date), 101) ="
                                + "convert(varchar, cast(substring('" + dob + "', 4, 2) + '/' + substring('" + dob + "', 0, 3) + "
                                + "'/' + substring('" + dob + "', 7, 4) as date), 101)"
                                + " order by dCreatedDate desc";

                            }

                            else if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(accountNumber))
                            {
                                cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GHI_POLICY_DATA where vUploadId not like '%can%'  and vCrnNo=" + "'" + crn + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                            }

                            //else if (!String.IsNullOrEmpty(dob) && !String.IsNullOrEmpty(accountNumber))
                            else
                            {
                                cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GHI_POLICY_DATA where vUploadId not like '%can%'  and " +
                                "convert(varchar, cast(vCustomerDob as date), 101) = "
                                + "convert(varchar, cast(substring('" + dob + "', 4, 2) + '/' + substring('" + dob + "', 0, 3) + "
                                + "'/' + substring('" + dob + "', 7, 4) as date), 101) and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                            }

                            cmd.Connection = sqlConHDC;
                            sqlConHDC.Open();
                            object objProd = cmd.ExecuteScalar();
                            certificateNo = Convert.ToString(objProd);
                            sqlConHDC.Close();
                            createGHIPDF(certificateNo);
                        }
                    }
                    #endregion

                }

                bool IsGPAGistData = false;
                if (String.IsNullOrEmpty(certificateNo))
                {

                    #region Code to download GPA Policy
                    using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {

                            if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(dob))
                            {
                                cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GPA_POLICY_TABLE where vEndorsementType not like '%canc%' and vCrnNo=" + "'" + crn + "' and dCustomerDob=" + "'" + dob + "' order by dCreatedDate desc";
                            }

                            else if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(accountNumber))
                            {
                                cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GPA_POLICY_TABLE where vEndorsementType not like '%canc%' and  vCrnNo=" + "'" + crn + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                            }

                            //else if (!String.IsNullOrEmpty(dob) && !String.IsNullOrEmpty(accountNumber))
                            else
                            {
                                cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GPA_POLICY_TABLE where vEndorsementType not like '%canc%' and dCustomerDob=" + "'" + dob + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                            }

                            cmd.Connection = sqlCon;
                            sqlCon.Open();
                            object objProd = cmd.ExecuteScalar();
                            certificateNo = Convert.ToString(objProd);
                            sqlCon.Close();
                        }
                    }
                    #endregion

                }

                else if (IsGPAGistData && String.IsNullOrEmpty(certificateNo))
                {
                    using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {

                            if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(dob))
                            {
                                cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GPA_POLICY_TABLE_GIST where vEndorsementType not like '%canc%' and vCrnNo=" + "'" + crn + "' and dCustomerDob=" + "'" + dob + "' order by dCreatedDate desc";
                            }

                            else if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(accountNumber))
                            {
                                cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GPA_POLICY_TABLE_GIST where vEndorsementType not like '%canc%' and  vCrnNo=" + "'" + crn + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                            }

                            else
                            {
                                cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GPA_POLICY_TABLE_GIST where vEndorsementType not like '%canc%' and dCustomerDob=" + "'" + dob + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                            }

                            cmd.Connection = sqlCon;
                            sqlCon.Open();
                            object objProd = cmd.ExecuteScalar();
                            certificateNo = Convert.ToString(objProd);
                            sqlCon.Close();
                        }
                    }

                    if (!String.IsNullOrEmpty(certificateNo))
                    {
                        IsGPAGistData = true;
                    }

                }

                else if (IsGPAGistData && String.IsNullOrEmpty(certificateNo))
                {
                    //lblError.Text = "No data found for the parameters entered";
                    //gist service to be called
                    GetCertificateFromGIST(certificateNo);
                    return;
                }



                using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (!String.IsNullOrEmpty(dob))
                        {
                            cmd.CommandText = "select count(1) from dbo.TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "'  and dCustomerDob=" + "'" + dob + "' and dCreatedDate > '2017-07-01' and vEndorsementType not like '%canc%'"; //gst condition
                        }

                        else if (!String.IsNullOrEmpty(crn))
                        {
                            cmd.CommandText = "select count(1) from dbo.TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "'  and vCrnNo=" + "'" + crn + "' and dCreatedDate > '2017-07-01' and vEndorsementType not like '%canc%'"; //gst condition
                        }

                        else
                        {
                            cmd.CommandText = "select count(1) from dbo.TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "'  and vAccountNo=" + "'" + accountNumber + "' and dCreatedDate > '2017-07-01' and vEndorsementType not like '%canc%'"; //gst condition
                        }

                        cmd.Connection = sqlCon;
                        sqlCon.Open();

                        object cmdReturn = cmd.ExecuteScalar();
                        int recCount = Convert.ToInt32(cmdReturn);

                        if (recCount < 1)
                        {
                            if (IsGPAGistData == false)
                            {
                                if (!String.IsNullOrEmpty(dob))
                                {
                                    cmd.CommandText = "select count(1) from dbo.TBL_GPA_POLICY_TABLE_GIST where vCertificateNo=" + "'" + certificateNo + "'  and dCustomerDob=" + "'" + dob + "' and dCreatedDate > '2017-07-01' and vEndorsementType not like '%canc%'"; //gst condition
                                }

                                else if (!String.IsNullOrEmpty(crn))
                                {
                                    cmd.CommandText = "select count(1) from dbo.TBL_GPA_POLICY_TABLE_GIST where vCertificateNo=" + "'" + certificateNo + "'  and vCrnNo=" + "'" + crn + "' and dCreatedDate > '2017-07-01' and vEndorsementType not like '%canc%'"; //gst condition
                                }

                                else
                                {
                                    cmd.CommandText = "select count(1) from dbo.TBL_GPA_POLICY_TABLE_GIST where vCertificateNo=" + "'" + certificateNo + "'  and vAccountNo=" + "'" + accountNumber + "' and dCreatedDate > '2017-07-01' and vEndorsementType not like '%canc%'"; //gst condition
                                }

                                object cmdReturndata = cmd.ExecuteScalar();
                                int GISTrecCount = Convert.ToInt32(cmdReturndata);

                                if (GISTrecCount > 0)
                                {
                                    IsGPAGistData = true;
                                }
                            }
                        }

                        if (IsGPAGistData == false && recCount < 1)
                        {
                            GetCertificateFromGIST(certificateNo);
                            return;
                        }

                        //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::Fetching data for gpa protect certificate :" + certificateNo + " and dob " + dob + " " + DateTime.Now + Environment.NewLine);


                        if (IsGPAGistData == false)
                        {
                            if (!String.IsNullOrEmpty(dob))
                            {
                                cmd.CommandText = "SELECT vProductName,vCustomerName from TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "' and dCustomerDob=" + "'" + dob + "' order by dCreatedDate desc";
                            }

                            else if (!String.IsNullOrEmpty(crn))
                            {
                                cmd.CommandText = "SELECT vProductName,vCustomerName from TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "' and vCrnNo=" + "'" + crn + "' order by dCreatedDate desc";
                            }

                            else
                            {
                                cmd.CommandText = "SELECT vProductName,vCustomerName from TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                            }

                            //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::query is :" + cmd.CommandText + " " + DateTime.Now + Environment.NewLine);

                        }

                        else
                        {
                            if (!String.IsNullOrEmpty(dob))
                            {
                                cmd.CommandText = "SELECT vProductName,vCustomerName from TBL_GPA_POLICY_TABLE_GIST where vCertificateNo=" + "'" + certificateNo + "' and dCustomerDob=" + "'" + dob + "' order by dCreatedDate desc";
                            }

                            else if (!String.IsNullOrEmpty(crn))
                            {
                                cmd.CommandText = "SELECT vProductName,vCustomerName from TBL_GPA_POLICY_TABLE_GIST where vCertificateNo=" + "'" + certificateNo + "' and vCrnNo=" + "'" + crn + "' order by dCreatedDate desc";
                            }

                            else
                            {
                                cmd.CommandText = "SELECT vProductName,vCustomerName from TBL_GPA_POLICY_TABLE_GIST where vCertificateNo=" + "'" + certificateNo + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                            }
                        }
                        // cmd.Connection = sqlCon;
                        //   sqlCon.Open();                            
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::dataset count :" + ds.Tables[0].Rows.Count + DateTime.Now + Environment.NewLine);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            prod_name = ds.Tables[0].Rows[0]["vProductName"].ToString();
                            customerName = ds.Tables[0].Rows[0]["vCustomerName"].ToString();
                        }
                        //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::prodname :" + prod_name + DateTime.Now + Environment.NewLine);
                    }
                }

                if (prod_name.ToUpper().Contains("PROTECT"))
                {
                    if (IsGPAGistData == false)
                    {
                        //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::Fetching data for gpa protect certificate :" + certificateNo + " " + DateTime.Now + Environment.NewLine);
                        GenerateGPAPotectPDF(certificateNo);
                    }
                    else
                    {
                        GenerateGPAProtectPDF_GIST(certificateNo);
                    }
                }
                else
                {
                    //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::Fetching data for gpa care certificate :" + certificateNo + " " + DateTime.Now + Environment.NewLine);
                    string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_With_GST_Test_HeaderFooter.html";
                    string htmlBody = File.ReadAllText(strPath);
                    string custStateCode = string.Empty;
                    string PolicyIssuingOfficeAddress = string.Empty;

                    string IntermediaryName = string.Empty;
                    string IntermediaryCode = string.Empty;
                    string IntermediaryLandline = string.Empty;
                    string IntermediaryMobile = string.Empty;

                    string PolicyholderName = string.Empty;
                    string PolicyholderAddress = string.Empty;
                    string PolicyholderAddress2 = string.Empty;
                    string PolicyholderBusinessDescription = string.Empty;
                    string PolicyholderTelephoneNumber = string.Empty;
                    string PolicyholderEmailAddress = string.Empty;
                    string PolicyNumber = string.Empty;
                    string PolicyInceptionDateTime = string.Empty;
                    string PolicyExpiryDateTime = string.Empty;
                    string TotalNumberOfInsuredPersons = string.Empty;

                    string RowCoverHeader = string.Empty;
                    string SectionARow = string.Empty;
                    string ExtSectionARow = string.Empty;
                    string SectionBRow = string.Empty;

                    string NameofInsuredPerson = string.Empty;
                    string DateOfBirth = string.Empty;
                    string Gender = string.Empty;
                    string EmailId = string.Empty;
                    string MobileNo = string.Empty;
                    string SumInsured = string.Empty;
                    string NomineeDetails = string.Empty;
                    string SectionACoverPremium = string.Empty;
                    string ExtensionstoSectionASectionBCoverPremium = string.Empty;
                    string LoadingsDiscounts = string.Empty;
                    string ServiceTax = string.Empty;
                    string SwachhBharatCess = string.Empty;
                    string KrishiKalyanCess = string.Empty;
                    string NetPremiumRoundedOff = string.Empty;
                    string StampDuty = string.Empty;
                    string Receipt_Challan_No = string.Empty;
                    string Receipt_Challan_No_Dated = string.Empty;
                    string PolicyIssueDate = string.Empty;
                    string TotalAmount = string.Empty;
                    bool IsCertificateNumberExists = false;
                    //string prod_name = string.Empty;
                    //gst changes
                    string ugstPercentage = string.Empty;
                    string ugstAmount = string.Empty;
                    string cgstPercentage = string.Empty;
                    string cgstAmount = string.Empty;
                    string sgstPercentage = string.Empty;
                    string sgstAmount = string.Empty;
                    string igstPercentage = string.Empty;
                    string igstAmount = string.Empty;
                    string totalGSTAmount = string.Empty;
                    string vProposerPinCode = string.Empty;
                    string addCol1 = string.Empty;
                    string polStartDate = string.Empty;
                    string createdDate = string.Empty;
                    string address1 = string.Empty;
                    string address2 = string.Empty;
                    string address3 = string.Empty;
                    string certNo = string.Empty;
                    string UINNo = string.Empty;
                    string placeOfSupply = string.Empty;
                    string proposalNo = string.Empty;

                    GetGPACertificateDetails(ref PolicyIssuingOfficeAddress
       , ref IntermediaryName
       , ref IntermediaryCode
       , ref PolicyholderName
       , ref PolicyholderAddress
       , ref PolicyholderAddress2
       , ref PolicyholderBusinessDescription
       , ref PolicyholderTelephoneNumber
       , ref PolicyholderEmailAddress
       , ref PolicyNumber
       , ref PolicyInceptionDateTime
       , ref PolicyExpiryDateTime
       , ref TotalNumberOfInsuredPersons
       , ref RowCoverHeader
       , ref SectionARow
       , ref ExtSectionARow
       , ref SectionBRow
       , ref NameofInsuredPerson
       , ref DateOfBirth
       , ref Gender
       , ref EmailId
       , ref MobileNo
       , ref SumInsured
       , ref NomineeDetails
       , ref SectionACoverPremium
       , ref ExtensionstoSectionASectionBCoverPremium
       , ref LoadingsDiscounts
       , ref ServiceTax
       , ref SwachhBharatCess
       , ref KrishiKalyanCess
       , ref NetPremiumRoundedOff
       , ref StampDuty
       , ref Receipt_Challan_No
       , ref Receipt_Challan_No_Dated
       , ref PolicyIssueDate
       , ref IntermediaryLandline
       , ref IntermediaryMobile
       , ref TotalAmount
       , ref IsCertificateNumberExists
        // , certNo
        , ref ugstPercentage
           , ref ugstAmount
           , ref cgstPercentage
           , ref cgstAmount
           , ref igstPercentage
           , ref igstAmount
           , ref sgstPercentage
           , ref sgstAmount
           , ref totalGSTAmount
           , ref vProposerPinCode
           , ref addCol1
           , ref polStartDate
           , ref createdDate
           , ref address1
           , ref address2
           , ref address3
           , ref UINNo
            , ref placeOfSupply
            , ref proposalNo);

                    StringWriter sw = new StringWriter();
                    StringReader sr = new StringReader(sw.ToString());

                    string strHtml = htmlBody;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + vProposerPinCode + "'";
                            cmd.Connection = con;
                            con.Open();
                            object objCustState = cmd.ExecuteScalar();
                            custStateCode = Convert.ToString(objCustState);
                        }
                    }

                    strHtml = strHtml.Replace("@PolicyIssuingOfficeAddress", PolicyIssuingOfficeAddress);
                    strHtml = strHtml.Replace("@IntermediaryName", IntermediaryName);
                    strHtml = strHtml.Replace("@IntermediaryCode", IntermediaryCode);

                    strHtml = strHtml.Replace("@IntermediaryLandline", IntermediaryLandline);
                    strHtml = strHtml.Replace("@IntermediaryMobile", IntermediaryMobile);

                    strHtml = strHtml.Replace("@PolicyholderName", PolicyholderName);
                    strHtml = strHtml.Replace("@PolicyholderAddress", PolicyholderAddress);
                    string existPolicyholderAddress2 = string.Empty;
                    existPolicyholderAddress2 = PolicyholderAddress2.Replace("(stateCode)", "");
                    PolicyholderAddress2 = PolicyholderAddress2.Replace("stateCode", custStateCode);
                    strHtml = strHtml.Replace("@PolicyholderLine2Address", PolicyholderAddress2);
                    strHtml = strHtml.Replace("@PolicyholderBusinessDescription", PolicyholderBusinessDescription);
                    strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", PolicyholderTelephoneNumber);
                    strHtml = strHtml.Replace("@PolicyholderEmailAddress", PolicyholderEmailAddress);
                    //strHtml = strHtml.Replace("@PolicyNumber", PolicyNumber + "/" + certNo); //done changes for cert no

                    strHtml = strHtml.Replace("@PolicyNumber", certificateNo);
                    strHtml = strHtml.Replace("@PolicyInceptionDateTime", PolicyInceptionDateTime);
                    //manish start
                    strHtml = strHtml.Replace("@Enroll", PolicyInceptionDateTime.Substring(24));
                    //manish end
                    strHtml = strHtml.Replace("@PolicyExpiryDateTime", PolicyExpiryDateTime);
                    strHtml = strHtml.Replace("@TotalNumberOfInsuredPersons", TotalNumberOfInsuredPersons);

                    strHtml = strHtml.Replace("@RowCoverHeader", string.IsNullOrEmpty(RowCoverHeader) ? "" : RowCoverHeader);
                    strHtml = strHtml.Replace("@RowSectionA", string.IsNullOrEmpty(SectionARow) ? "" : SectionARow);
                    strHtml = strHtml.Replace("@RowExtSectionA", string.IsNullOrEmpty(ExtSectionARow) ? "" : ExtSectionARow);
                    strHtml = strHtml.Replace("@RowSectionB", string.IsNullOrEmpty(SectionBRow) ? "" : SectionBRow);

                    strHtml = strHtml.Replace("@NameofInsuredPerson", NameofInsuredPerson);
                    strHtml = strHtml.Replace("@DateOfBirth", DateOfBirth); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));
                    strHtml = strHtml.Replace("@Gender", Gender);
                    strHtml = strHtml.Replace("@EmailId", EmailId);
                    strHtml = strHtml.Replace("@MobileNo", MobileNo);
                    strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@NomineeDetails", NomineeDetails);
                    strHtml = strHtml.Replace("@SectionACoverPremium", Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@ExtensionstoSectionASectionBCoverPremium", Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@LoadingsDiscounts", string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@ServiceTax", string.IsNullOrEmpty(ServiceTax) ? "0.00" : Convert.ToDecimal(ServiceTax).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@SwachhBharatCess", string.IsNullOrEmpty(SwachhBharatCess) ? "0.00" :Convert.ToDecimal(SwachhBharatCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@KrishiKalyanCess", string.IsNullOrEmpty(KrishiKalyanCess) ? "0.00" :Convert.ToDecimal(KrishiKalyanCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@NetPremiumRoundedOff", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@StampDuty", string.IsNullOrEmpty(StampDuty) ? "0.00" :Convert.ToDecimal(StampDuty).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@Receipt_Challan_No", Receipt_Challan_No);
                    strHtml = strHtml.Replace("@Challan_No_Dated", Receipt_Challan_No_Dated);
                    strHtml = strHtml.Replace("@PolicyIssueDate", PolicyIssueDate);
                    strHtml = strHtml.Replace("@TotalAmount", TotalAmount);

                    strHtml = strHtml.Replace("@masterPolicy", PolicyNumber);
                    strHtml = strHtml.Replace("@certificateNo", certificateNo);
                    //Added By Nilesh 
                    string _Date = createdDate;
                    DateTime dt = Convert.ToDateTime(_Date);
                    string FDate = dt.ToString("dd/MM/yyyy");
                    strHtml = strHtml.Replace("@createdDate", FDate);
                    //strHtml = strHtml.Replace("@createdDate", createdDate);
                    //End By Nilesh
                    strHtml = strHtml.Replace("@customerName", customerName);
                    strHtml = strHtml.Replace("@productName", "KOTAK GROUP ACCIDENT CARE");

                    strHtml = strHtml.Replace("@addressline1", address1);
                    strHtml = strHtml.Replace("@addressline2", address2);
                    strHtml = strHtml.Replace("@addressline3", address3);
                    strHtml = strHtml.Replace("@statepincode", existPolicyholderAddress2);

                    //Added By Nilesh for CR353
                    if (string.IsNullOrEmpty(Gender))
                    {
                        strHtml = strHtml.Replace("@salutation", "");
                    }
                    else
                    {
                        if (Gender.Trim() == "M" || Gender == "Male")
                        {
                            strHtml = strHtml.Replace("@salutation", "Mr.");
                        }
                        else if (Gender.Trim() == "F")
                        {
                            strHtml = strHtml.Replace("@salutation", "Mrs.");
                        }
                        else
                        {
                            strHtml = strHtml.Replace("@salutation", "");
                        }
                    }
                    //End By Nilesh for CR353


                    string customString = string.Empty;

                    if (!String.IsNullOrEmpty(addCol1))
                    {
                        string[] strArr = addCol1.Split(' ');
                        // customString = "this " + strArr[1] + " day of " + strArr[0] + " of " + strArr[2];

                        if (String.IsNullOrEmpty(strArr[1]))
                        {
                            customString = "this " + strArr[2] + " day of " + strArr[0] + " of year " + strArr[3];
                        }
                        else
                        {
                            customString = "this " + strArr[1] + " day of " + strArr[0] + " of year " + strArr[2];
                        }

                    }

                    strHtml = strHtml.Replace("@polIssueString", customString);

                    string igstData = string.Empty;
                    string cgstugstData = string.Empty;
                    string cgstsgstData = string.Empty;

                    if (igstPercentage != "0")
                    {
                        string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                        igstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px'><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>IGST@" + igstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='20px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + igstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                    }
                    else
                    {
                        if (cgstPercentage != "0" && ugstPercentage != "0")
                        {
                            string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                            cgstugstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px' ><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>SGST@" + sgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>UGST@" + ugstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='10px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + ugstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + cgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                        }
                        if (cgstPercentage != "0" && sgstPercentage != "0")
                        {
                            string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                            cgstsgstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='10px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px'><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>SGST@" + sgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='10px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + sgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + cgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                        }
                    }

                    strHtml = strHtml.Replace("@cgstugstData", cgstugstData == "" ? "" : cgstugstData);

                    strHtml = strHtml.Replace("@cgstsgstData", cgstsgstData == "" ? "" : cgstsgstData);

                    strHtml = strHtml.Replace("@igstData", igstData == "" ? "" : igstData);

                    strHtml = strHtml.Replace("@KotakGroupAccidentCareUIN", UINNo == "" ? "" : UINNo);

                    //CR_P1_450_Start Kuwar Tax Invoice_GPA_Policy 
                    #region TaxInvoiceGPAPolicy

                    //GPA_GenerateGPAProtectPDF()
                    StringBuilder taxinvoice = new StringBuilder();
                    taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                    int temp = 0;
                    string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                    string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                    string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                    string totalPremium = TotalAmount;
                    if (totalPremium.Contains('.'))
                    {
                        temp = Convert.ToInt32(totalPremium.Substring(0, totalPremium.IndexOf('.')));

                    }
                    else
                    {
                        temp = Convert.ToInt32(totalPremium);
                    }

                    string totalPremiumInWord = ConvertAmountInWord(temp);

                    // QR Code GPA
                    string suppliGSTN = ConfigurationManager.AppSettings["GstRegNo"].ToString();//hardcord value to be passs
                    string buyerGSTN = "";
                    string transactionDate = polStartDate;
                    int noofHSNCode = 0;
                    string receiptNumber = Receipt_Challan_No;
                    string hsnCode = ConfigurationManager.AppSettings["SacCode"].ToString();//hardcode value to be pass
                    if (hsnCode != "")
                    {
                        var tempcount = hsnCode.Split(' ').Length;
                        for (int i = 0; i < tempcount; i++)
                        {
                            noofHSNCode++;
                        }

                    }
                    string Imagepath = string.Empty;
                    CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                    Imagepath = Imagepath == "error" ? "" : Imagepath;
                    string kgiStateCode = suppliGSTN.Substring(0, 2); // getting kgi state code 
                    string kgiStateName = string.Empty;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                            cmd.Connection = con;
                            con.Open();
                            object objStaeName = cmd.ExecuteScalar();
                           kgiStateName = Convert.ToString(objStaeName);
                           
                        }

                    }
                    strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);

                    strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                    //GPA Policy
                    strHtml = strHtml.Replace("@gistinno", "");
                    strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                    strHtml = strHtml.Replace("@customername", customerName);
                    strHtml = strHtml.Replace("@emailId", EmailId);
                    strHtml = strHtml.Replace("@contactno", MobileNo);
                    strHtml = strHtml.Replace("@address", PolicyholderAddress);
                    strHtml = strHtml.Replace("@address1", PolicyholderAddress2);
                    // strHtml = strHtml.Replace("@address2", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());// add 3 address
                    strHtml = strHtml.Replace("@imdcode", IntermediaryCode);
                    strHtml = strHtml.Replace("@receiptno", Receipt_Challan_No);
                    strHtml = strHtml.Replace("@customerstatecode", custStateCode);
                    //strHtml = strHtml.Replace("@customerstatecode", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst statecode of customer require
                    strHtml = strHtml.Replace("@supplyname", placeOfSupply);//gst state name require of customer

                    //GPA Policy
                    strHtml = strHtml.Replace("@name", kgiName);
                    strHtml = strHtml.Replace("@panNo", kgiPanno);
                    strHtml = strHtml.Replace("@cinNo", kgiCINno);
                    strHtml = strHtml.Replace("@invoicedate", polStartDate);
                    strHtml = strHtml.Replace("@invoiceno", certificateNo);
                    strHtml = strHtml.Replace("@proposalno", proposalNo);
                    strHtml = strHtml.Replace("@partnerappno", "");
                    //strHtml = strHtml.Replace("@irn", certNo);

                    strHtml = strHtml.Replace("@kgistatecode", kgiStateCode);//gst state code of kotak
                    strHtml = strHtml.Replace("@kgistatename", kgiStateName);//gst state code of kotak
                    strHtml = strHtml.Replace("@irn", certificateNo);

                    strHtml = strHtml.Replace("@totalpremium", TotalAmount);
                    strHtml = strHtml.Replace("@netamount", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@NetPremiumString", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                    strHtml = strHtml.Replace("@totalgst", totalGSTAmount);

                    strHtml = strHtml.Replace("@cgstpercent", cgstPercentage);
                    strHtml = strHtml.Replace("@ugstpercent", ugstPercentage);
                    strHtml = strHtml.Replace("@sgstpercent", sgstPercentage);
                    strHtml = strHtml.Replace("@igstpercent", igstPercentage);

                    //GPA Policy
                    strHtml = strHtml.Replace("@cgstamt", cgstAmount);
                    strHtml = strHtml.Replace("@ugstamt", ugstAmount);
                    strHtml = strHtml.Replace("@sgstamt", sgstAmount);
                    strHtml = strHtml.Replace("@igstamt", igstAmount);

                    strHtml = strHtml.Replace("@cessrate", "0");
                    strHtml = strHtml.Replace("@cessamt", SwachhBharatCess);
                    string tdservicetax = string.Empty;
                    string dataservicetax = string.Empty;
                    if (ServiceTax != "0" && !string.IsNullOrEmpty(ServiceTax))
                    {
                        tdservicetax = "<td style='border: 1px solid black' width='5%'><p style ='font-size:small'><strong>Service Tax</strong></p></td> ";
                        dataservicetax = "<td style ='border:1px solid black' width = '5%'><p> " + Convert.ToDecimal(ServiceTax).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td>";


                    }
                    strHtml = strHtml.Replace("@servicetx", tdservicetax == "" ? "" : tdservicetax);
                    strHtml = strHtml.Replace("@servictaxh", dataservicetax == "" ? "" : dataservicetax);
                    strHtml = strHtml.Replace("@totalgross", TotalAmount);// change1
                    strHtml = strHtml.Replace("@totalinvoicevalueinfigure", TotalAmount);
                    strHtml = strHtml.Replace("@totalinvoicevalueinwords", totalPremiumInWord.ToString());
                    #endregion
                    //CR_450_End_Kuwar_Tax_Invoice GPA Policy

                    TextWriter outTextWriter = new StringWriter();
                    HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                    base.Render(outHtmlTextWriter);

                    string currentPageHtmlString = strHtml; //outTextWriter.ToString();

                    // Create a HTML to PDF converter object with default settings
                    HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                    // Set license key received after purchase to use the converter in licensed mode
                    // Leave it not set to use the converter in demo mode
                    string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();

                    htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

                    // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                    // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                    htmlToPdfConverter.ConversionDelay = 2;

                    // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                    htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                    // Add Header

                    // Enable header in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                    // Optionally add a space between header and the page body
                    // The spacing for first page and the subsequent pages can be set independently
                    // Leave this option not set for no spacing
                    htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                    htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");

                    // Draw header elements
                    if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                        DrawHeader(htmlToPdfConverter, false);

                    // Add Footer

                    // Enable footer in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                    // Optionally add a space between footer and the page body
                    // Leave this option not set for no spacing
                    htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                    // Draw footer elements
                    if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                        DrawFooter(htmlToPdfConverter, false, true);

                    // Use the current page URL as base URL
                    string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;


                    //// Convert the current page HTML string to a PDF document in a memory buffer
                    // For Live
                    byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                    byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                    // For Live End Here 


                    //// For Dev
                    //byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                    //byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                    //// For Dev End here 

                    //string filePath = Server.MapPath("~/GPADownload");
                    //string fileName = "GPAPolicySchedule_" + certificateNo;

                    //String strfilename = filePath + "\\" + fileName;
                    //DownloadFile(strfilename);

                    Response.AddHeader("Content-Type", "application/pdf");


                    Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNo));


                    Response.BinaryWrite(outPdfBuffer);

                    CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadGPAGetPolicy.aspx");//Added By Rajesh Soni on 19/02/2020
                    Response.End();



                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
            }
        }
        private void GetCertificateFromGIST(string certificateNo)
        {
            try
            {
                string crn = txtCRN.Text;
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                if (!String.IsNullOrEmpty(certificateNo))
                {
                    using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            if (!String.IsNullOrEmpty(dob))
                            {
                                // cmd.CommandText = "select count(1) from dbo.TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "'  and dCustomerDob=" + "'" + dob + "' and dCreatedDate > '2017-07-01' and vEndorsementType not like '%canc%'"; //gst condition
                                cmd.CommandText = "SP_GET_RECORD_COUNT_DOB_FROM_GIST";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@nCertNumber", certificateNo);
                                cmd.Parameters.AddWithValue("@dDateOfBirth", dob);
                            }

                            else if (!String.IsNullOrEmpty(crn))
                            {
                                // cmd.CommandText = "select count(1) from dbo.TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "'  and vCrnNo=" + "'" + crn + "' and dCreatedDate > '2017-07-01' and vEndorsementType not like '%canc%'"; //gst condition
                                cmd.CommandText = "SP_GET_RECORD_COUNT_CRN_FROM_GIST";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@nCertNumber", certificateNo);
                                cmd.Parameters.AddWithValue("@nCRNNumber", crn);
                            }

                            else
                            {
                                //cmd.CommandText = "select count(1) from dbo.TBL_GPA_POLICY_TABLE where vCertificateNo=" + "'" + certificateNo + "'  and vAccountNo=" + "'" + accountNumber + "' and dCreatedDate > '2017-07-01' and vEndorsementType not like '%canc%'"; //gst condition
                                cmd.CommandText = "SP_GET_RECORD_COUNT_ACCOUNT_FROM_GIST";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@nCertNumber", certificateNo);
                                cmd.Parameters.AddWithValue("@nAccountNumber", accountNumber);
                            }

                            cmd.Connection = sqlCon;
                            sqlCon.Open();

                            object cmdReturn = cmd.ExecuteScalar();
                            int recCount = Convert.ToInt32(cmdReturn);

                            if (recCount < 1)
                            {
                                lblError.Text = "No data found for the parameters entered";
                                return;
                            }
                            else
                            {
                                DownloadCertificateFromGIST(certificateNo);
                            }
                        }
                    }
                }
                else
                {

                    using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {

                            if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(dob))
                            {
                                cmd.CommandText = "SP_GET_CERTIFICATE_NUMBER_FROM_GIST";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@nCRNNumber", crn);
                                cmd.Parameters.AddWithValue("@dDateOfBirth", dob);
                            }

                            else if (!String.IsNullOrEmpty(crn) && !String.IsNullOrEmpty(accountNumber))
                            {
                                cmd.CommandText = "SP_GET_CERTIFICATE_NUMBER_FROM_GIST_ACCNUMBER";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@nCRNNumber", crn);
                                cmd.Parameters.AddWithValue("@nAccountNumber", accountNumber);
                            }

                            else
                            {
                                // cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_GPA_POLICY_TABLE where dCustomerDob=" + "'" + dob + "' and vAccountNo=" + "'" + accountNumber + "' order by dCreatedDate desc";
                                cmd.CommandText = "SP_GET_CERTIFICATE_NUMBER_FROM_GIST_DOB";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@dDateOfBirth", dob);
                                cmd.Parameters.AddWithValue("@nAccountNumber", accountNumber);
                            }

                            cmd.Connection = sqlCon;
                            sqlCon.Open();
                            object objProd = cmd.ExecuteScalar();
                            certificateNo = Convert.ToString(objProd);
                            sqlCon.Close();
                        }
                    }

                    if (String.IsNullOrEmpty(certificateNo))
                    {
                        lblError.Text = "No data found for the parameters entered";
                        return;
                    }
                    else
                    {
                        DownloadCertificateFromGIST(certificateNo);
                    }
                }



            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "GetCertificateFromGIST ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
            }
        }

        private void DownloadCertificateFromGIST(string certificateNo)
        {
            string ErrorMsg = string.Empty;
            PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
            byte[] objByte = proxy.KGIGetPolicyDocumentForPortal("81062f2fc69b4639af5bf33e86c66408", certificateNo, "4202", ref ErrorMsg);

            //  byte[] objByte = proxy.KGIGetPolicyDocumentForPASS("81062f2fc69b4639af5bf33e86c66408", certificateNo, "4202", "", ref ErrorMsg);


            string fileName = certificateNo + ".pdf";
            if (ErrorMsg.Trim().Length <= 0)
            {
                Response.Clear();
                Response.ContentType = "application/force-download";
                //Response.AddHeader("content-disposition", "attachment;filename=1010404900.pdf");
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.BinaryWrite(objByte);
                CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadGPAGetPolicy.aspx");//Added By Rajesh Soni on 20/02/2020
                Response.End();

            }
            else
            {
                lblError.Text = "No data found for the parameters entered";
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "GetCertificateFromGIST ::Errormsg return from service  :" + ErrorMsg + DateTime.Now + Environment.NewLine);
                return;
            }
        }

        private void createHDCPDF(string certificateNo)
        {
            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::start of hdc certificate :" + certificateNo + " and dob " + dob + " " + DateTime.Now + Environment.NewLine);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ToString()))
            {
                con.Open();
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_PDF_CompleteLetter.html";
                string htmlBody = File.ReadAllText(strPath);
                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(sw.ToString());
                string strHtml = htmlBody;

                // SqlCommand command = new SqlCommand("PROC_GET_COVER_SECTION_DATA_FOR_HDC_PDF_TEST", con);
                SqlCommand command = new SqlCommand("PROC_GET_REPLICA_COVER_SECTION_DATA_FOR_HDC_PDF_TEST", con);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                command.Parameters.AddWithValue("@vCertificateNo", certificateNo);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::hdc certificate :" + certificateNo + " and count " + ds.Tables[0].Rows.Count + " " + DateTime.Now + Environment.NewLine);

                        strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_Floater_PDF_CompleteLetter.html";
                        htmlBody = File.ReadAllText(strPath);
                        sw = new StringWriter();
                        sr = new StringReader(sw.ToString());
                        strHtml = htmlBody;

                        GenerateNonEmailHDC_Flotaer_PDF(con, ds, strHtml, certificateNo);


                        // Changed the below logic as floater and individual will hav esame template 
                        //if (ds.Tables[2].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["InsuredName2"].ToString()))
                        //{
                        //    strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_Floater_PDF_CompleteLetter.html";
                        //    htmlBody = File.ReadAllText(strPath);
                        //    sw = new StringWriter();
                        //    sr = new StringReader(sw.ToString());
                        //    strHtml = htmlBody;

                        //    GenerateNonEmailHDC_Flotaer_PDF(con, ds, strHtml, certificateNo);


                        //}
                        //else
                        //{
                        //    strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_PDF_CompleteLetter.html";
                        //    htmlBody = File.ReadAllText(strPath);
                        //    sw = new StringWriter();
                        //    sr = new StringReader(sw.ToString());
                        //    strHtml = htmlBody;

                        //    GenerateNonEmailHDCPDF(con, ds, strHtml, certificateNo);

                        //}
                    }

                }

                //File.AppendAllText(folderPath + "\\log.txt", "html body created" + Environment.NewLine);
            }
        }

        private void createGHIPDF(string certificateNo)
        {
            DataSet dsGCIData = new DataSet();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ToString()))

            {
                SqlCommand command = new SqlCommand("PROC_GET_GHI_POLICY_DATA_DOWNLOAD", con);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                command.Parameters.AddWithValue("@vCertificateNo", certificateNo);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                dataAdapter.Fill(dsGCIData);

            }

            if (dsGCIData.Tables.Count > 0)
            {
                if (dsGCIData.Tables[0].Rows.Count > 0)
                {
                    string GCIPdfHeaderPath = AppDomain.CurrentDomain.BaseDirectory + "GroupSecureShield_NewPolicyHeader_GHI.html";
                    string GCIHeaderHtml = "";
                    GCIHeaderHtml = File.ReadAllText(GCIPdfHeaderPath);


                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblWHeader_source}", "");
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblRefNo}", "GEN/WEL/SG/0008.2/" + certificateNo);
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblType}", Convert.ToDateTime(dsGCIData.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("dd-MMM-yyyy"));
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblMasterCustomerName}", Convert.ToDateTime(dsGCIData.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("dd-MMM-yyyy"));
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblDate}", Convert.ToDateTime(dsGCIData.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("dd-MMM-yyyy"));
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblCustomerName}", dsGCIData.Tables[0].Rows[0]["vCustomerName"].ToString());
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblCustomerAddressLetter}", dsGCIData.Tables[0].Rows[0]["vMailingAddressofTheInsured"].ToString());
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblPolicyNo}", certificateNo);
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblProductName}", dsGCIData.Tables[1].Rows[0]["vKGIProductName"].ToString());
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblEmail}", ConfigurationManager.AppSettings["EmailAdd"].ToString());
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblToll_Free_No}", ConfigurationManager.AppSettings["TollFreeNumber"].ToString());
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblContactNumber1}", ConfigurationManager.AppSettings["lblContactNumber1"].ToString());
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblNoOfDays}", "");
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblCompanyAddress}", ConfigurationManager.AppSettings["AddressWithoutLine"].ToString());
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblCompanyName_2}", ConfigurationManager.AppSettings["lblCompanyName"].ToString());
                    GCIHeaderHtml = GCIHeaderHtml.Replace("{lblwelcomeletterFooter}", "");



                    string GCIPDFPath = "";
                    // string strPath = "D:\\GPABulkPrint\\HTML\\Kotak_GroupSecureShield_PolicySchedule.htm";
                    //string strPath = AppDomain.CurrentDomain.BaseDirectory + "Kotak_GroupSecureShield_PolicySchedule.html";
                    string strPath = AppDomain.CurrentDomain.BaseDirectory + "GHI_Polici_Schedule_New.html";
                    //            
                    string htmlBody = File.ReadAllText(strPath);

                    htmlBody = htmlBody.Replace("{lblNewPolicyheader}", GCIHeaderHtml.ToString());
                    htmlBody = htmlBody.Replace("{lblMasterPolicyNo}", dsGCIData.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                    htmlBody = htmlBody.Replace("{lblPolicyIssueDate}", dsGCIData.Tables[1].Rows[0]["vMasterPolicyIssuancedate1"].ToString());
                    htmlBody = htmlBody.Replace("{lblMasterCustomerName}", dsGCIData.Tables[0].Rows[0]["vMasterPolicyHolderName"].ToString());
                    htmlBody = htmlBody.Replace("{lblType}", dsGCIData.Tables[1].Rows[0]["vGroupType"].ToString());//  
                    htmlBody = htmlBody.Replace("{lblPolicyNo}", dsGCIData.Tables[0].Rows[0]["vCertificateNo"].ToString());
                    htmlBody = htmlBody.Replace("{lblPolicyType}", dsGCIData.Tables[0].Rows[0]["vBusinessType"].ToString());
                    htmlBody = htmlBody.Replace("{lblPolicyIssuedAt}", dsGCIData.Tables[0].Rows[0]["vPolicyIssuedAt"].ToString());
                    htmlBody = htmlBody.Replace("{lblCustomerName}", dsGCIData.Tables[0].Rows[0]["vCustomerName"].ToString());
                    htmlBody = htmlBody.Replace("{lblPrevPollicyNo}", dsGCIData.Tables[0].Rows[0]["vPreviousPolicyNumber"].ToString());
                    htmlBody = htmlBody.Replace("{lblIssuanceDate}", dsGCIData.Tables[0].Rows[0]["vAccountDebitDate1"].ToString());
                    htmlBody = htmlBody.Replace("{lblPolicyServicingAddress}", dsGCIData.Tables[0].Rows[0]["vPolicyIssuedAt"].ToString());
                    htmlBody = htmlBody.Replace("{lblCustomerAddress}", dsGCIData.Tables[0].Rows[0]["vMailingAddressofTheInsured2"].ToString().Replace("{br}", " "));// vGSTRegistrationNo
                    htmlBody = htmlBody.Replace("{lblGSTIN}", "");
                    // htmlBody = htmlBody.Replace("{lblGSTIN}", dsGCIData.Tables[1].Rows[0]["vGSTRegistrationNo"].ToString());
                    htmlBody = htmlBody.Replace("{lblCustomerMobile}", dsGCIData.Tables[0].Rows[0]["vMobileNo"].ToString());
                    htmlBody = htmlBody.Replace("{lblCustomerEmailID}", dsGCIData.Tables[0].Rows[0]["vEmailId"].ToString());
                    htmlBody = htmlBody.Replace("{lblPolicyStartTime}", dsGCIData.Tables[0].Rows[0]["vPolicyStartdate1"].ToString());//Convert.ToDateTime(dsGCIData.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("HH:MM"));
                    htmlBody = htmlBody.Replace("{lblPolicyStartDate}", dsGCIData.Tables[0].Rows[0]["dPolicyEndDate1"].ToString());//dPolicyStartDate Convert.ToDateTime(dsGCIData.Tables[0].Rows[0]["vAccountDebitDate"].ToString()).ToString("ddMMyyyy");
                    htmlBody = htmlBody.Replace("{lblPolicyEndDate}", dsGCIData.Tables[0].Rows[0]["dPolicyEndDate1"].ToString());// Convert.ToDateTime(dsGCIData.Tables[0].Rows[0]["dPolicyEndDate"].ToString()).ToString("dd/MM/yyyy")); //dPolicyEndDate
                    htmlBody = htmlBody.Replace("{lblInstallmentoption}", "NO");// dsGCIData.Tables[0].Rows[0]["vInstallmentOption"].ToString());
                    htmlBody = htmlBody.Replace("{lblInstalmentFrequency}", "NA");// dsGCIData.Tables[0].Rows[0]["vInstalmentFrequency"].ToString());
                                                                                  // htmlBody = htmlBody.Replace("{lblPreExistingwaitingperiod}", dsGCIData.Tables[0].Rows[0]["vPreExistingwaitingperiod"].ToString());
                    htmlBody = htmlBody.Replace("{lblAgentCode}", dsGCIData.Tables[1].Rows[0]["vIntermediaryCode"].ToString());
                    htmlBody = htmlBody.Replace("{lblAgentName}", dsGCIData.Tables[1].Rows[0]["vIntermediaryName"].ToString());
                    htmlBody = htmlBody.Replace("{lblAgentEmailId}", "");// dsGCIData.Tables[0].Rows[0]["vIntermediaryEmailId"].ToString());//vIntermediaryEmailId
                    htmlBody = htmlBody.Replace("{lblAgentLandline}", dsGCIData.Tables[1].Rows[0]["vIntermediaryLandlineNo"].ToString());
                    htmlBody = htmlBody.Replace("{lblPremium}", dsGCIData.Tables[0].Rows[0]["nNetPremium"].ToString());// dsGCIData.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                                                                                                                       //htmlBody = htmlBody.Replace("{lblPremtable}", dsGCIData.Tables[0].Rows[0][""].ToString());
                    htmlBody = htmlBody.Replace("{lblNetPrem}", dsGCIData.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());// dsGCIData.Tables[1].Rows[0]["nNetPremium"].ToString());//
                    htmlBody = htmlBody.Replace("{lblInstallmentDetails}", "");
                    htmlBody = htmlBody.Replace("{lblContactNumber1}", ConfigurationManager.AppSettings["lblContactNumber1"].ToString());
                    htmlBody = htmlBody.Replace("{SumInsured}", dsGCIData.Tables[0].Rows[0]["vTotalPolicySumInsured"].ToString());
                    htmlBody = htmlBody.Replace("{SumInsuredBasis}", dsGCIData.Tables[1].Rows[0]["vPlolicyCategory"].ToString());
                    htmlBody = htmlBody.Replace("{ContactDetails}", dsGCIData.Tables[0].Rows[0]["vMobileNo"].ToString());
                    htmlBody = htmlBody.Replace("{lblMasterPolicyLoaction}", dsGCIData.Tables[1].Rows[0]["vMasterPolicyLocation"].ToString());

                    htmlBody = htmlBody.Replace("{CGST}", dsGCIData.Tables[0].Rows[0]["cgstAmount"].ToString());
                    htmlBody = htmlBody.Replace("{@UGST }", dsGCIData.Tables[0].Rows[0]["ugstAmount"].ToString());
                    htmlBody = htmlBody.Replace("{@SGST }", dsGCIData.Tables[0].Rows[0]["sgstAmount"].ToString());
                    htmlBody = htmlBody.Replace("{@IGST}", dsGCIData.Tables[0].Rows[0]["igstAmount"].ToString());




                    htmlBody = htmlBody.Replace("{ACCNO}", dsGCIData.Tables[0].Rows[0]["vAccountNo"].ToString());// +"/"+ dsGCIData.Tables[1].Rows[0]["vAccountNo"].ToString()

                    //{ lblParticularDetails}

                    string lblParticularDetails = "";
                    lblParticularDetails = lblParticularDetails + "<table  class='text - content' width='100%' style='font-size:12px' border='1' cellspacing='0' cellpadding='0'>";
                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td width='10%'>  </td>";
                    lblParticularDetails = lblParticularDetails + "<td width='45%'> Particular </td>";
                    lblParticularDetails = lblParticularDetails + "<td width='45%'> Details </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";

                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td width='10%' rowspan='15' valign='top'> Insured Person Information </td>";
                    lblParticularDetails = lblParticularDetails + "<td width='45%'> Membership ID/ Employee Number/ Account Number pertaining to Credit(#)</td>";
                    lblParticularDetails = lblParticularDetails + "<td width='45%'>" + dsGCIData.Tables[0].Rows[0]["vAccountNo"].ToString() + " </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";



                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Credit Tenure (#) </td>";
                    lblParticularDetails = lblParticularDetails + "<td>" + " " + "</td>";// + dsGCIData.Tables[1].Rows[0]["vPolicyTenure"].ToString() +
                    lblParticularDetails = lblParticularDetails + "</tr>";

                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Name of the Insured Person </td>";
                    lblParticularDetails = lblParticularDetails + "<td>" + dsGCIData.Tables[0].Rows[0]["vCustomerName"].ToString() + " </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";


                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Applicant/Co-applicant </td>";
                    lblParticularDetails = lblParticularDetails + "<td>" + "" + "</td>";//+ dsGCIData.Tables[0].Rows[0][""].ToString() + "</td>";//Applicant_CoApplicant
                    lblParticularDetails = lblParticularDetails + "</tr>";


                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Occupation (Salaried/Self-employed) (#) </td>";
                    lblParticularDetails = lblParticularDetails + "<td> </td>";// + dsGCIData.Tables[0].Rows[0]["vSalariedNonSalaried"].ToString() + 
                    lblParticularDetails = lblParticularDetails + "</tr>";



                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Relationship with the Insured Person </td>";
                    lblParticularDetails = lblParticularDetails + "<td>" + "Self" + " </td>";//+ dsGCIData.Tables[0].Rows[0]["vRelationshipWithInsuredPerson"].ToString() +
                    lblParticularDetails = lblParticularDetails + "</tr>";



                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Date of Birth DD/MM /YYYY </td>";
                    lblParticularDetails = lblParticularDetails + "<td>" + dsGCIData.Tables[0].Rows[0]["vCustomerDob"].ToString() + " </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";



                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Age </td>";
                    lblParticularDetails = lblParticularDetails + "<td>" + dsGCIData.Tables[0].Rows[0]["vAge"].ToString() + "  </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";




                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Gender </td>";
                    lblParticularDetails = lblParticularDetails + "<td> " + dsGCIData.Tables[0].Rows[0]["vCustomerGender"].ToString() + "   </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";



                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Category </td>";
                    lblParticularDetails = lblParticularDetails + "<td> " + dsGCIData.Tables[0].Rows[0]["vCustomerType"].ToString() + "  </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";



                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Credit Amount/Outstanding Credit Amount (#) </td>";
                    lblParticularDetails = lblParticularDetails + "<td>" + "" + "</td>";// + dsGCIData.Tables[0].Rows[0][""].ToString() + "   </td>";//CreditAmountOutstandingCreditAmount
                    lblParticularDetails = lblParticularDetails + "</tr>";



                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Sum Insured </td>";
                    lblParticularDetails = lblParticularDetails + "<td> " + "" + " </td>";//dsGCIData.Tables[0].Rows[0]["vTotalPolicySumInsured"].ToString() 
                    lblParticularDetails = lblParticularDetails + "</tr>";


                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Sum Insured Basis(#) </td>";
                    lblParticularDetails = lblParticularDetails + "<td> " + dsGCIData.Tables[0].Rows[0]["vTotalPolicySumInsured"].ToString() + " </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";


                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Description/Remarks/Pre-existing Condition</td>";
                    lblParticularDetails = lblParticularDetails + "<td> </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";
                    lblParticularDetails = lblParticularDetails + "</table>";


                    lblParticularDetails = lblParticularDetails + "<table class='text - content' width='100%' style='font-size:12px' border='1' cellspacing='0' cellpadding='0'>";

                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td rowspan='3'  width='10%'>Nominee Details </td>";
                    lblParticularDetails = lblParticularDetails + "<td  width='45%'>Name </td>";
                    lblParticularDetails = lblParticularDetails + "<td  width='45%'> " + dsGCIData.Tables[0].Rows[0]["vNomineeName"].ToString() + " </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";




                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Relationship with the Insured Person </td>";
                    lblParticularDetails = lblParticularDetails + "<td> " + dsGCIData.Tables[0].Rows[0]["vNomineeRelation"].ToString() + " </td>";
                    lblParticularDetails = lblParticularDetails + "</tr>";



                    lblParticularDetails = lblParticularDetails + "<tr>";
                    lblParticularDetails = lblParticularDetails + "<td>Appointee Details in case Nominee is a Minor </td>";
                    lblParticularDetails = lblParticularDetails + "<td>" + (!string.IsNullOrEmpty(dsGCIData.Tables[0].Rows[0]["vAdditionalColumn8"].ToString()) ? (dsGCIData.Tables[0].Rows[0]["vAdditionalColumn8"].ToString() + " / " + dsGCIData.Tables[0].Rows[0]["vAdditionalColumn9"].ToString()) : "") + "   </td>";//vAppointeeDetails //dsGCIData.Tables[0].Rows[0]["vNomineeName"].ToString() 

                    lblParticularDetails = lblParticularDetails + "</tr>";




                    lblParticularDetails += "</table>";


                    lblParticularDetails += " <br> (#) Applicable only to Credit linked policies";


                    //htmlBody = htmlBody.Replace("{lblParticularDetails}", lblParticularDetails);
                    htmlBody = htmlBody.Replace("{lblParticularDetails}", "");


                    string lblCoverageDetails = "<table class='text - content' width='100%' border='1' cellspacing='0' cellpadding='0'>";
                    lblCoverageDetails += "<tr>";
                    lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' align='center' style ='font-family: Arial; font-size: 13px; ' ><strong> Sr.No </strong></td>";
                    lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' align='center' style ='font-family: Arial; font-size: 13px; width: 40%;; ' ><strong> Base Covers</strong></td>";
                    lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' align='center' style ='font-family: Arial; font-size: 13px; width: 28%;' ><strong> Sum Insured Limits</strong></td>";
                    lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' align='center' style ='font-family: Arial; font-size: 13px;width: 28%;' ><strong> Description/ Remarks</strong></td>";
                    lblCoverageDetails += "</tr>";


                    if (dsGCIData.Tables[2].Rows.Count > 0)
                    {
                        int j = 0, k = 0;

                        for (int i = 0; i < dsGCIData.Tables[4].Rows.Count; i++)
                        {
                            if ((dsGCIData.Tables[4].Rows[i]["CoversType"]).ToString() == "B") // if (((dsGCIData.Tables[4].Rows[i]["CoverNames"]).ToString() == "In-patient treatment") || ((dsGCIData.Tables[4].Rows[i]["CoverNames"]).ToString() == "Pre Hospitalisation Medical Expenses") || ((dsGCIData.Tables[4].Rows[i]["CoverNames"]).ToString() == "Post Hospitalisation Medical Expenses") || ((dsGCIData.Tables[4].Rows[i]["CoverNames"]).ToString() == "Day Care Treatment") || ((dsGCIData.Tables[6].Rows[i]["CoverNames"]).ToString() == "Domiciliary Hospitalisation") || ((dsGCIData.Tables[6].Rows[i]["CoverNames"]).ToString() == "Emergency Ambulance") || ((dsGCIData.Tables[6].Rows[i]["CoverNames"]).ToString() == "Donor Expenses"))
                            {
                                lblCoverageDetails += "<tr>";
                                lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' align='center' style ='font-family: Arial; font-size: 13px; '>" + (i == 0 ? 1 : i + 1) + " </td>";
                                lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' style ='font-family: Arial; font-size: 13px; '> " + (dsGCIData.Tables[4].Rows[i]["CoverNames"]).ToString() + "  </td>";
                                lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' style ='font-family: Arial; font-size: 13px; '>" + (dsGCIData.Tables[4].Rows[i]["CoverLimits"]).ToString() + "</td>";
                                lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' style ='font-family: Arial; font-size: 13px; '>" + (dsGCIData.Tables[4].Rows[i]["CoverDesc"]).ToString() + "</td>";
                                lblCoverageDetails += "</tr>";

                            }
                            else
                            {

                                if (j == 0)
                                {
                                    lblCoverageDetails += "<tr>";
                                    lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' align='center' style ='font-family: Arial; font-size: 13px; ' ><strong> Sr.No</strong> </td>";
                                    lblCoverageDetails += "<td class='text-premium td4 tdtop' text - content' align='center' style ='font-family: Arial; font-size: 13px; ' ><strong>  Optional Covers </b></td>";
                                    lblCoverageDetails += "<td class='text-premium td4 tdtop' text - content' align='center'  style ='font-family: Arial; font-size: 13px; ' > <strong> Sum Insured Limits </strong></td>";
                                    lblCoverageDetails += "<td class='text-premium td4 tdtop' text - content' align='center'  style ='font-family: Arial; font-size: 13px; ' > </td>";
                                    j = 1;
                                    lblCoverageDetails += "<tr>";
                                }
                                lblCoverageDetails += "<tr>";
                                lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' align='center' style ='font-family: Arial; font-size: 13px; '>" + (k == 0 ? 1 : k + 1) + " </td>";
                                lblCoverageDetails += "<td class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 13px; '> " + (dsGCIData.Tables[4].Rows[i]["CoverNames"]).ToString() + "  </td>";
                                lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' style ='font-family: Arial; font-size: 13px; '>" + (dsGCIData.Tables[4].Rows[i]["CoverLimits"]).ToString() + "</td>";
                                lblCoverageDetails += "<td class='text-premium td4 tdtop text - content' style ='font-family: Arial; font-size: 13px; '>" + (dsGCIData.Tables[4].Rows[i]["CoverDesc"]).ToString() + "</td>";
                                lblCoverageDetails += "</tr>";
                                k++;
                            }
                        }
                    }
                    lblCoverageDetails += "</table>";

                    //   lblCoverageDetails += "<br> (*) For Salaried Persons and Credit linked policies only";
                    //  #endregion

                    htmlBody = htmlBody.Replace("{lblCoverageDetails}", lblCoverageDetails);
                    #region Floater Policy Details
                    DataTable dtFloaterNominee = dsGCIData.Tables[2];


                    string tbodyFloaterNominee = string.Empty;
                    tbodyFloaterNominee = "<table width='100%' border='1' cellspacing='0' style='font-family: Arial; font-size: 13px;' cellpadding='0' class='tdleft td3'><tr><td style='font-family: Arial; font-size: 13px;' align='center'> <strong>Member ID</strong></th> <td style='font-family: Arial; font-size: 13px;' align='center'>    <strong>Insured Name </strong> </th> <td style='font-family: Arial; font-size: 13px;' align='center'>        <strong>Insured Relation</strong></th>  <td style='font-family: Arial; font-size: 13px;' align='center'>   <strong>Date of Birth</strong> </th><td style='font-family: Arial; font-size: 13px;' align='center'>   <strong>Age </strong> </th><td align='center'>     <strong>Gender</strong> </th><td style='font-family: Arial; font-size: 13px;' align='center'> <strong>Date of Joining</strong> </th><td style='font-family: Arial; font-size: 13px;' align='center'> <strong>Pre-existing Condition</strong>  </th></tr> ";

                    foreach (DataRow dr in dtFloaterNominee.Rows)
                    {

                        if (!string.IsNullOrEmpty(dr["InsuredName1"].ToString()))
                        {
                            tbodyFloaterNominee += "<tr>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'> " + dr["nMemberID"].ToString() + "</td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'> " + dr["InsuredName1"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>  Self </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'> " + dr["InsuredDOB1"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'> " + dr["InsuredAge1"].ToString() + " </td>";
                            //  tbodyFloaterNominee += "<td >" + dr["Age2"].ToString() + " </td>";//
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'> " + dr["InsuredGender1"].ToString() + " </td>";

                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["vSelfDateofJoining"].ToString() + " </td>";//" + dr["NomineeName2"].ToString() + "
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>NO</td>";//" + dr["NomineeRelation2"].ToString() + "

                            tbodyFloaterNominee += "</tr>";
                        }


                        if (!string.IsNullOrEmpty(dr["InsuredName2"].ToString()))
                        {
                            tbodyFloaterNominee += "<tr>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'> " + dr["nInsured2MemberID"].ToString() + "</td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredName2"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredRelation2"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredDOB2"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredAge2"].ToString() + " </td>";//
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredGender2"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["vInsured1MemberDateofJoining"].ToString() + " </td>";//" + dr["NomineeName2"].ToString() + "
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>NO </td>";//" + dr["NomineeRelation2"].ToString() + "
                            tbodyFloaterNominee += "</tr>";
                        }



                        if (!string.IsNullOrEmpty(dr["InsuredName3"].ToString()))
                        {
                            tbodyFloaterNominee += "<tr>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["nInsured3MemberID"].ToString() + "</td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredName3"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredRelation3"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredDOB3"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredAge3"].ToString() + " </td>";//
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredGender3"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["vInsured2MemberDateofJoining"].ToString() + " </td>";//" + dr["NomineeName2"].ToString() + "
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>NO</td>";//" + dr["NomineeRelation2"].ToString() + "

                            tbodyFloaterNominee += "</tr>";
                        }


                        if (!string.IsNullOrEmpty(dr["InsuredName4"].ToString()))
                        {
                            tbodyFloaterNominee += "<tr>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["nInsured4MemberID"].ToString() + "</td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredName4"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredRelation4"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredDOB4"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredAge4"].ToString() + " </td>";//
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredGender4"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["vInsured3MemberDateofJoining"].ToString() + " </td>";//" + dr["NomineeName2"].ToString() + "
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>NO</td>";//" + dr["NomineeRelation2"].ToString() + "
                            tbodyFloaterNominee += "</tr>";
                        }



                        if (!string.IsNullOrEmpty(dr["InsuredName5"].ToString()))
                        {
                            tbodyFloaterNominee += "<tr>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["nInsured5MemberID"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredName5"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredRelation5"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredDOB5"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredAge5"].ToString() + " </td>";//
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>" + dr["InsuredGender5"].ToString() + " </td>";
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'> " + dr["vInsured4MemberDateofJoining"].ToString() + "</td>";//" + dr["NomineeName2"].ToString() + "
                            tbodyFloaterNominee += "<td style='font-family: Arial; font-size: 13px;'>NO</td>";// " + dr["NomineeDOB2"].ToString() + "
                            tbodyFloaterNominee += "</tr>";
                        }



                        //if (!string.IsNullOrEmpty(dr["InsuredName6"].ToString()))
                        //{
                        //    tbodyFloaterNominee += "<tr>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'> Memberid1  </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'>" + dr["InsuredName6"].ToString() + " </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'>" + dr["InsuredRelation6"].ToString() + " </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'>" + dr["InsuredDOB6"].ToString() + " </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'>" + dr["InsuredGender6"].ToString() + " </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'> </td>";//" + dr["NomineeName2"].ToString() + "
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'> </td>";//" + dr["NomineeRelation2"].ToString() + "
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'></td>";// " + dr["NomineeDOB2"].ToString() + "
                        //    tbodyFloaterNominee += "</tr>";
                        //}

                    }
                    tbodyFloaterNominee += "</table>";

                    //   lblCoverageDetails += "<br> (*) For Salaried Persons and Credit linked policies only";
                    #endregion

                    htmlBody = htmlBody.Replace("@tbody", tbodyFloaterNominee.ToString());
                    // strHtml = strHtml.Replace("NULL", "");



                    string NOMINEEDetails = string.Empty;
                    NOMINEEDetails = "<tr><td  align='center'> <strong>Insured Name</strong></th> <td align='center'>   <strong>Nominee Name</strong> </th> <td align='center'>        <strong>Relationship with the Insured Person</strong></th>  <td align='center'>   <strong>Age </strong> </th><td align='center'> <strong>Appointee Details in case Nominee is a Minor</strong> </th></tr> ";

                    foreach (DataRow dr in dtFloaterNominee.Rows)
                    {

                        if (!string.IsNullOrEmpty(dr["InsuredName1"].ToString()))
                        {
                            NOMINEEDetails += "<tr>";
                            NOMINEEDetails += "<td > " + dr["InsuredName1"].ToString() + " </td>";
                            NOMINEEDetails += "<td > " + dr["NomineeName1"].ToString() + "</td>";
                            NOMINEEDetails += "<td >  " + dr["NomineeRelation1"].ToString() + " </td>";
                            NOMINEEDetails += "<td >  " + dr["vNomineeAge"].ToString() + "</td>";
                            NOMINEEDetails += "<td >" + dsGCIData.Tables[0].Rows[0]["vAdditionalColumn8"].ToString() + " / " + dsGCIData.Tables[0].Rows[0]["vAdditionalColumn9"].ToString() + "</td>";// " + dr["NomineeDetails1"].ToString() + "
                            NOMINEEDetails += "</tr>";
                        }


                        if (!string.IsNullOrEmpty(dr["InsuredName2"].ToString()))
                        {
                            NOMINEEDetails += "<tr>";
                            NOMINEEDetails += "<td >" + dr["InsuredName2"].ToString() + " </td>";
                            NOMINEEDetails += "<td > " + dr["NomineeName2"].ToString() + "</td>";
                            NOMINEEDetails += "<td >  " + dr["NomineeRelation2"].ToString() + " </td>";
                            NOMINEEDetails += "<td >" + dr["vNomineeAge2"].ToString() + " </td>";//
                            NOMINEEDetails += "<td > </td>";//" + dr["NomineeName2"].ToString() + "

                            NOMINEEDetails += "</tr>";
                        }



                        if (!string.IsNullOrEmpty(dr["InsuredName3"].ToString()))
                        {
                            NOMINEEDetails += "<tr>";
                            NOMINEEDetails += "<td >" + dr["InsuredName3"].ToString() + " </td>";
                            NOMINEEDetails += "<td > " + dr["NomineeName3"].ToString() + "</td>";
                            NOMINEEDetails += "<td >  " + dr["NomineeRelation3"].ToString() + " </td>";
                            NOMINEEDetails += "<td >" + dr["vNomineeAge4"].ToString() + " </td>";//
                            NOMINEEDetails += "<td > </td>";//" + dr["NomineeName2"].ToString() + "

                            NOMINEEDetails += "</tr>";
                        }


                        if (!string.IsNullOrEmpty(dr["InsuredName4"].ToString()))
                        {
                            NOMINEEDetails += "<tr>";
                            NOMINEEDetails += "<td >" + dr["InsuredName4"].ToString() + " </td>";
                            NOMINEEDetails += "<td > " + dr["NomineeName4"].ToString() + "</td>";
                            NOMINEEDetails += "<td >  " + dr["NomineeRelation4"].ToString() + " </td>";
                            NOMINEEDetails += "<td > " + dr["vNomineeAge4"].ToString() + "</td>";
                            NOMINEEDetails += "<td > </td>";//" + dr["NomineeName2"].ToString() + "

                            NOMINEEDetails += "</tr>";
                        }



                        if (!string.IsNullOrEmpty(dr["InsuredName5"].ToString()))
                        {
                            NOMINEEDetails += "<tr>";
                            NOMINEEDetails += "<td >" + dr["InsuredName5"].ToString() + " </td>";
                            NOMINEEDetails += "<td > " + dr["NomineeName5"].ToString() + "</td>";
                            NOMINEEDetails += "<td >  " + dr["NomineeRelation5"].ToString() + " </td>";
                            NOMINEEDetails += "<td >" + dr["vNomineeAge5"].ToString() + " </td>";//
                            NOMINEEDetails += "<td > </td>";//" + dr["NomineeName2"].ToString() + "
                            NOMINEEDetails += "</tr>";
                        }



                        //if (!string.IsNullOrEmpty(dr["InsuredName6"].ToString()))
                        //{
                        //    tbodyFloaterNominee += "<tr>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'> Memberid1  </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'>" + dr["InsuredName6"].ToString() + " </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'>" + dr["InsuredRelation6"].ToString() + " </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'>" + dr["InsuredDOB6"].ToString() + " </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'>" + dr["InsuredGender6"].ToString() + " </td>";
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'> </td>";//" + dr["NomineeName2"].ToString() + "
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'> </td>";//" + dr["NomineeRelation2"].ToString() + "
                        //    tbodyFloaterNominee += "<td style='border: 1px solid black'></td>";// " + dr["NomineeDOB2"].ToString() + "
                        //    tbodyFloaterNominee += "</tr>";
                        //}

                    }
                    // tbodyFloaterNominee += "</table>";

                    //   lblCoverageDetails += "<br> (*) For Salaried Persons and Credit linked policies only";

                    htmlBody = htmlBody.Replace("{@NOMINEEDetails}", NOMINEEDetails.ToString());







                    //lblClause = lblClause + "<table width='100%' border='1' cellspacing='0' cellpadding='0'>";
                    //lblClause = lblClause + "<tr>";
                    //lblClause = lblClause + "<td class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 13px; '> 1. </td>";
                    //lblClause = lblClause + "<td colspan='6' class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 13px; '> "+ dsGCIData.Tables[1].Rows[0]["vCondition1"].ToString() + " </td>";
                    //lblClause = lblClause + "</tr>";
                    //lblClause = lblClause + "<tr>";
                    //lblClause = lblClause + "<td class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 13px; '> 2. </td>";
                    //lblClause = lblClause + "<td colspan='6' class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 13px; '> " + dsGCIData.Tables[1].Rows[0]["vCondition2"].ToString() + " </td>";
                    //lblClause = lblClause + "</tr>";
                    //lblClause = lblClause + "<tr>";
                    //lblClause = lblClause + "<td class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 18px; '> 3. </td>";
                    //lblClause = lblClause + "<td colspan='6' class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 18px; '> " + dsGCIData.Tables[1].Rows[0]["vCondition3"].ToString() + " </td>";
                    //lblClause = lblClause + "</tr>";
                    //lblClause = lblClause + "<tr>";
                    //lblClause = lblClause + "<td class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 18px; '> 4. </td>";
                    //lblClause = lblClause + "<td colspan='6' class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 18px; '> " + dsGCIData.Tables[1].Rows[0]["vCondition4"].ToString() + " </td>";
                    //lblClause = lblClause + "</tr>";
                    //lblClause = lblClause + "<tr>";
                    //lblClause = lblClause + "<td class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 18px; '> 5. </td>";
                    //lblClause = lblClause + "<td colspan='6' class='text-premium td4 tdtop' style ='font-family: Arial; font-size: 18px; '> " + dsGCIData.Tables[1].Rows[0]["vCondition5"].ToString() + " </td>";
                    //lblClause = lblClause + "</tr>";
                    //lblClause = lblClause + "</table>";
                    string lblClause = "<table width='100%' border='1' cellspacing='0' cellpadding='0' class='tdleft td3 text - content'>";
                    lblClause += "<tr>";

                    lblClause += "<td width='5%' class='text-premium td4 tdtop text - content' align='center' ><b>Sr.No.</b></td>";
                    lblClause += "<td  colspan='6'  class='text-premium td4 tdtop text - content' align='center' style ='font-family: Arial; font-size: 13px; ' ><b>Condition  </b>  </td>";

                    lblClause += "</tr>";
                    if (dsGCIData.Tables[3].Rows.Count > 0)
                    {

                        for (int i = 0; i < dsGCIData.Tables[3].Rows.Count; i++)
                        {

                            //ListofCriticallIllness += "</ul> ";
                            lblClause += "<td class='text-premium td4 tdtop text - content' align='center' style ='font-family: Arial; font-size: 13px;'>" + (i == 0 ? 1 : i + 1) + " </td>";
                            lblClause += "<td colspan='6' class='text-premium td4 tdtop text - content' style ='font-family: Arial; font-size: 13px; '>" + (dsGCIData.Tables[3].Rows[i]["conditionsValue"]).ToString() + "</td>";
                            lblClause += "</tr>";


                        }
                    }
                    lblClause += "</table>";



                    htmlBody = htmlBody.Replace("{lblClause}", lblClause);
                    htmlBody = htmlBody.Replace("{lblExclusion}", "");
                    htmlBody = htmlBody.Replace("{lblWebsite}", ConfigurationManager.AppSettings["lblWebsite"].ToString());
                    htmlBody = htmlBody.Replace("{lblTollFreeNo}", ConfigurationManager.AppSettings["TollFreeNumber"].ToString());
                    htmlBody = htmlBody.Replace("{lblAddress}", ConfigurationManager.AppSettings["AddressWithLineGHI"].ToString());
                    htmlBody = htmlBody.Replace("{br}", "</br>");
                    htmlBody = htmlBody.Replace("{lblseniorcitizenEmailkotak}", ConfigurationManager.AppSettings["seniorcitizenEmailkotak"].ToString());
                    htmlBody = htmlBody.Replace("{lblgrievanceofficerEmailkotak}", ConfigurationManager.AppSettings["lblgrievanceofficerEmailkotak"].ToString());
                    htmlBody = htmlBody.Replace("{lblgbicwebsite}", ConfigurationManager.AppSettings["website"].ToString());
                    htmlBody = htmlBody.Replace("{lblEmailAdd}", ConfigurationManager.AppSettings["EmailAdd"].ToString());

                    string GstNumber = dsGCIData.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                    if (GstNumber.Length == 15)
                    {
                        htmlBody = htmlBody.Replace("{lblR1}", GstNumber[0].ToString());
                        htmlBody = htmlBody.Replace("{lblR2}", GstNumber[1].ToString());
                        htmlBody = htmlBody.Replace("{lblR3}", GstNumber[2].ToString());
                        htmlBody = htmlBody.Replace("{lblR4}", GstNumber[3].ToString());
                        htmlBody = htmlBody.Replace("{lblR5}", GstNumber[4].ToString());
                        htmlBody = htmlBody.Replace("{lblR6}", GstNumber[5].ToString());
                        htmlBody = htmlBody.Replace("{lblR7}", GstNumber[6].ToString());
                        htmlBody = htmlBody.Replace("{lblR8}", GstNumber[7].ToString());
                        htmlBody = htmlBody.Replace("{lblR9}", GstNumber[8].ToString());
                        htmlBody = htmlBody.Replace("{lblR10}", GstNumber[9].ToString());
                        htmlBody = htmlBody.Replace("{lblR11}", GstNumber[10].ToString());
                        htmlBody = htmlBody.Replace("{lblR12}", GstNumber[11].ToString());
                        htmlBody = htmlBody.Replace("{lblR13}", GstNumber[12].ToString());
                        htmlBody = htmlBody.Replace("{lblR14}", GstNumber[13].ToString());
                        htmlBody = htmlBody.Replace("{lblR15}", GstNumber[14].ToString());
                    }
                    else
                    {
                        htmlBody = htmlBody.Replace("{lblR1}", "");
                        htmlBody = htmlBody.Replace("{lblR2}", "");
                        htmlBody = htmlBody.Replace("{lblR3}", "");
                        htmlBody = htmlBody.Replace("{lblR4}", "");
                        htmlBody = htmlBody.Replace("{lblR5}", "");
                        htmlBody = htmlBody.Replace("{lblR6}", "");
                        htmlBody = htmlBody.Replace("{lblR7}", "");
                        htmlBody = htmlBody.Replace("{lblR8}", "");
                        htmlBody = htmlBody.Replace("{lblR9}", "");
                        htmlBody = htmlBody.Replace("{lblR10}", "");
                        htmlBody = htmlBody.Replace("{lblR11}", "");
                        htmlBody = htmlBody.Replace("{lblR12}", "");
                        htmlBody = htmlBody.Replace("{lblR13}", "");
                        htmlBody = htmlBody.Replace("{lblR14}", "");
                        htmlBody = htmlBody.Replace("{lblR15}", "");
                    }
                    htmlBody = htmlBody.Replace("{lblCategory}", dsGCIData.Tables[1].Rows[0]["vTAXCategory"].ToString());// dsGCIData.Tables[1].Rows[0]["vPlanCategory"].ToString());
                    htmlBody = htmlBody.Replace("{lblSACCode}", dsGCIData.Tables[0].Rows[0]["vSACCode"].ToString());
                    htmlBody = htmlBody.Replace("{lblDesc}", dsGCIData.Tables[0].Rows[0]["vTaxDescription"].ToString());//vDesc
                    htmlBody = htmlBody.Replace("{lblInvoice}", certificateNo);// dsGCIData.Tables[0].Rows[0]["vInvoiceNumber"].ToString());
                    htmlBody = htmlBody.Replace("{lblStampDuty}", dsGCIData.Tables[0].Rows[0]["vStampDuty"].ToString());
                    htmlBody = htmlBody.Replace("{lblChallanNo}", dsGCIData.Tables[0].Rows[0]["vChallanNo"].ToString());
                    string vdate = (dsGCIData.Tables[1].Rows[0]["dChallanNoDefaceNoDate1"].ToString());
                    if (!string.IsNullOrEmpty(vdate))
                    {
                        htmlBody = htmlBody.Replace("{lblCD1}", vdate[0].ToString());
                        htmlBody = htmlBody.Replace("{lblCD2}", vdate[1].ToString());
                        htmlBody = htmlBody.Replace("{lblCD3}", vdate[2].ToString());
                        htmlBody = htmlBody.Replace("{lblCD4}", vdate[3].ToString());
                        htmlBody = htmlBody.Replace("{lblCD5}", vdate[4].ToString());
                        htmlBody = htmlBody.Replace("{lblCD6}", vdate[5].ToString());
                        htmlBody = htmlBody.Replace("{lblCD7}", vdate[6].ToString());
                        htmlBody = htmlBody.Replace("{lblCD8}", vdate[7].ToString());
                        htmlBody = htmlBody.Replace("{lblPolicyServicingAddress}", "");
                    }
                    else
                    {

                        htmlBody = htmlBody.Replace("{lblCD1}", "");
                        htmlBody = htmlBody.Replace("{lblCD2}", "");
                        htmlBody = htmlBody.Replace("{lblCD3}", "");
                        htmlBody = htmlBody.Replace("{lblCD4}", "");
                        htmlBody = htmlBody.Replace("{lblCD5}", "");
                        htmlBody = htmlBody.Replace("{lblCD6}", "");
                        htmlBody = htmlBody.Replace("{lblCD7}", "");
                        htmlBody = htmlBody.Replace("{lblCD8}", "");
                        htmlBody = htmlBody.Replace("{lblPolicyServicingAddress}", "");
                    }
                    string vDay = Convert.ToDateTime(dsGCIData.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("dd");
                    string vMonth = Convert.ToDateTime(dsGCIData.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("MMMMMMMMM");
                    string vYear = Convert.ToDateTime(dsGCIData.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("yyyy");

                    htmlBody = htmlBody.Replace("{lblDay}", vDay);
                    htmlBody = htmlBody.Replace("{lblMonth}", vMonth);
                    htmlBody = htmlBody.Replace("{lblYear}", vYear);
                    // htmlBody = htmlBody.Replace("{lblCovernoteDtls}", "");
                    htmlBody = htmlBody.Replace("{lblCompanyName}", ConfigurationManager.AppSettings["lblCompanyName"].ToString());

                    string GHI_80D = AppDomain.CurrentDomain.BaseDirectory + "80D_for_GHI.htm";
                    string lbl80DCertificate = File.ReadAllText(GHI_80D);
                    htmlBody = htmlBody.Replace("{lbl80DCertificate}", lbl80DCertificate);
                    htmlBody = htmlBody.Replace("{lblPremium1}", dsGCIData.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                    htmlBody = htmlBody.Replace("{lblCompanyName}", ConfigurationManager.AppSettings["lblCompanyName"].ToString());
                    htmlBody = htmlBody.Replace("{lblNameOfProposer}", dsGCIData.Tables[0].Rows[0]["vCustomerName"].ToString());
                    htmlBody = htmlBody.Replace("{lblPolicyNo}", certificateNo);
                    htmlBody = htmlBody.Replace("{lblCovernoteDtls}", "");
                    htmlBody = htmlBody.Replace("{lblWebsite}", ConfigurationManager.AppSettings["website"].ToString());
                    htmlBody = htmlBody.Replace("{lblPaymentMode}", "NEFT/RTGS");
                    htmlBody = htmlBody.Replace("{lblInstrumentDate}", Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy"));
                    htmlBody = htmlBody.Replace("{lblCustomerAddressLetter}", dsGCIData.Tables[0].Rows[0]["vMailingAddressofTheInsured"].ToString());

                    // {lblPremtable}

                    string Premtable = "";
                    Premtable = Premtable + "<table width='100%' cellspacing='0' cellpadding='0' class='tdleft td3 text - content'>";
                    Premtable = Premtable + "<tr>";

                    Premtable = Premtable + "<td class='text-premium td4 tdtop' align='center' > CGST @ 9%";
                    Premtable = Premtable + "</td>";

                    Premtable = Premtable + "<td class='text-premium td4 tdtop' align='center' > SGST @ 9%";
                    Premtable = Premtable + "</td>";

                    Premtable = Premtable + "<tr>";
                    Premtable = Premtable + "<td class='text-premium td4 tdtop' align='center' > " + dsGCIData.Tables[0].Rows[0]["cgstAmount"].ToString();
                    Premtable = Premtable + "</td>";

                    Premtable = Premtable + "<td class='text-premium td4 tdtop' align='center' >" + dsGCIData.Tables[0].Rows[0]["sgstAmount"].ToString();
                    Premtable = Premtable + "</td>";

                    Premtable = Premtable + "<tr>";

                    Premtable = Premtable + "</table>";

                    htmlBody = htmlBody.Replace("{lblPremtable}", Premtable);

                    string FinacialYear = "";
                    DateTime today = DateTime.Today;
                    if (today.Month <= 3)
                    {
                        FinacialYear = today.AddYears(-1).ToString("yyyy") + " - " + today.ToString("yy");
                    }
                    else
                    {
                        FinacialYear = today.ToString("yyyy") + " - " + today.AddYears(1).ToString("yy");
                    }
                    string DeductionBenefit = "<br> <br>";
                    DeductionBenefit = DeductionBenefit + "A)Lump Sum Benefit";
                    DeductionBenefit = DeductionBenefit + "<table cellspacing='0' cellpadding='0' class='tdleft td3 text - content' border='1' width='60%'>";
                    DeductionBenefit = DeductionBenefit + "<tr>";
                    DeductionBenefit = DeductionBenefit + "<td class='text-premium td4 tdtop'>";
                    DeductionBenefit = DeductionBenefit + " Financial Year </td>";
                    DeductionBenefit = DeductionBenefit + "<td class='text-premium td4 tdtop'>";
                    DeductionBenefit = DeductionBenefit + "Annual Lumpsum premium allowed for Deduction under Section 80D </td>";
                    DeductionBenefit = DeductionBenefit + "</tr>";
                    DeductionBenefit = DeductionBenefit + "<tr>";
                    DeductionBenefit = DeductionBenefit + "<td class='text-premium td4 tdtop'>" + FinacialYear.ToString();
                    DeductionBenefit = DeductionBenefit + "</td>";
                    DeductionBenefit = DeductionBenefit + "<td class='text-premium td4 tdtop'>" + dsGCIData.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString();
                    DeductionBenefit = DeductionBenefit + "</td>";
                    DeductionBenefit = DeductionBenefit + "</tr>";
                    DeductionBenefit = DeductionBenefit + "</table> <br>";
                    DeductionBenefit = DeductionBenefit + "<b>Or</b> <br>";
                    DeductionBenefit = DeductionBenefit + "B) Year wise proportionate Benefit/Deduction:";
                    DeductionBenefit = DeductionBenefit + "<table cellspacing='0' cellpadding='0' class='tdleft td3 text - content' border='1' width='60%'>";
                    DeductionBenefit = DeductionBenefit + "<tr>";
                    DeductionBenefit = DeductionBenefit + "<td class='text-premium td4 tdtop'>";
                    DeductionBenefit = DeductionBenefit + " Financial Year </td>";
                    DeductionBenefit = DeductionBenefit + "<td class='text-premium td4 tdtop'>";
                    DeductionBenefit = DeductionBenefit + "Year wise proportionate premium allowed for Deduction under Section 80D </td>";
                    DeductionBenefit = DeductionBenefit + "</tr>";
                    DeductionBenefit = DeductionBenefit + "<tr>";
                    DeductionBenefit = DeductionBenefit + "<td class='text-premium td4 tdtop'>" + FinacialYear.ToString();
                    DeductionBenefit = DeductionBenefit + "</td>";
                    DeductionBenefit = DeductionBenefit + "<td class='text-premium td4 tdtop'>" + dsGCIData.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString();
                    DeductionBenefit = DeductionBenefit + "</td>";
                    DeductionBenefit = DeductionBenefit + "</tr>";
                    DeductionBenefit = DeductionBenefit + "</table> <br>";



                    htmlBody = htmlBody.Replace("any amendments made there to", "any amendments made there to subject to satisfaction of the conditions mentioned therein." + DeductionBenefit);

                    #region cr 705 health card addition 

                    //  htmlBody = htmlBody.Replace("","");
                    string MemberCard = AppDomain.CurrentDomain.BaseDirectory + "Member_HealthCard.html";
                    string lblMemberCard = File.ReadAllText(MemberCard);


                    if (dsGCIData.Tables[2].Rows.Count > 0)
                    {

                        for (int i = 0; i <= 4; i++)
                        {
                            if (i == 0)
                            {
                                if (!string.IsNullOrEmpty(dsGCIData.Tables[2].Rows[0]["nMemberID"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredName1"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredDOB1"].ToString()))
                                {
                                    htmlBody = htmlBody.Replace("{lblMemberCard}", lblMemberCard);
                                    htmlBody = htmlBody.Replace("{lblMemberID}", dsGCIData.Tables[2].Rows[0]["nMemberID"].ToString());
                                    htmlBody = htmlBody.Replace("{lblName}", dsGCIData.Tables[2].Rows[0]["InsuredName1"].ToString());
                                    htmlBody = htmlBody.Replace("{lblAge}", dsGCIData.Tables[2].Rows[0]["InsuredAge1"].ToString());
                                    htmlBody = htmlBody.Replace("{lblGender}", dsGCIData.Tables[2].Rows[0]["InsuredGender1"].ToString());
                                    htmlBody = htmlBody.Replace("{lblDOB}", Convert.ToDateTime(dsGCIData.Tables[2].Rows[0]["InsuredDOB1"]).ToString("dd/MM/yyyy"));
                                }
                                else
                                {
                                    htmlBody = htmlBody.Replace("{lblMemberCard}", "");
                                }
                            }
                            else if (i == 1)
                            {
                                if (!string.IsNullOrEmpty(dsGCIData.Tables[2].Rows[0]["nInsured2MemberID"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredName2"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredDOB2"].ToString()))
                                {
                                    htmlBody = htmlBody.Replace("{lblMemberCard1}", lblMemberCard);
                                    htmlBody = htmlBody.Replace("{lblMemberID}", dsGCIData.Tables[2].Rows[0]["nInsured2MemberID"].ToString());
                                    htmlBody = htmlBody.Replace("{lblName}", dsGCIData.Tables[2].Rows[0]["InsuredName2"].ToString());
                                    htmlBody = htmlBody.Replace("{lblAge}", dsGCIData.Tables[2].Rows[0]["InsuredAge2"].ToString());
                                    htmlBody = htmlBody.Replace("{lblGender}", dsGCIData.Tables[2].Rows[0]["InsuredGender2"].ToString());
                                    htmlBody = htmlBody.Replace("{lblDOB}", Convert.ToDateTime(dsGCIData.Tables[2].Rows[0]["InsuredDOB2"]).ToString("dd/MM/yyyy"));
                                }
                                else
                                {
                                    htmlBody = htmlBody.Replace("{lblMemberCard1}", "");
                                }
                            }
                            else if (i == 2)
                            {
                                if (!string.IsNullOrEmpty(dsGCIData.Tables[2].Rows[0]["nInsured3MemberID"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredName3"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredDOB3"].ToString()))
                                {
                                    htmlBody = htmlBody.Replace("{lblMemberCard2}", lblMemberCard);
                                    htmlBody = htmlBody.Replace("{lblMemberID}", dsGCIData.Tables[2].Rows[0]["nInsured3MemberID"].ToString());
                                    htmlBody = htmlBody.Replace("{lblName}", dsGCIData.Tables[2].Rows[0]["InsuredName3"].ToString());
                                    htmlBody = htmlBody.Replace("{lblAge}", dsGCIData.Tables[2].Rows[0]["InsuredAge3"].ToString());
                                    htmlBody = htmlBody.Replace("{lblGender}", dsGCIData.Tables[2].Rows[0]["InsuredGender3"].ToString());
                                    htmlBody = htmlBody.Replace("{lblDOB}", Convert.ToDateTime(dsGCIData.Tables[2].Rows[0]["InsuredDOB3"]).ToString("dd/MM/yyyy"));
                                }
                                else
                                {
                                    htmlBody = htmlBody.Replace("{lblMemberCard2}", "");
                                }
                            }
                            else if (i == 3)
                            {
                                if (!string.IsNullOrEmpty(dsGCIData.Tables[2].Rows[0]["nInsured4MemberID"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredName4"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredDOB4"].ToString()))
                                {
                                    htmlBody = htmlBody.Replace("{lblMemberCard3}", lblMemberCard);
                                    htmlBody = htmlBody.Replace("{lblMemberID}", dsGCIData.Tables[2].Rows[0]["nInsured4MemberID"].ToString());
                                    htmlBody = htmlBody.Replace("{lblName}", dsGCIData.Tables[2].Rows[0]["InsuredName4"].ToString());
                                    htmlBody = htmlBody.Replace("{lblAge}", dsGCIData.Tables[2].Rows[0]["InsuredAge4"].ToString());
                                    htmlBody = htmlBody.Replace("{lblGender}", dsGCIData.Tables[2].Rows[0]["InsuredGender4"].ToString());
                                    htmlBody = htmlBody.Replace("{lblDOB}", Convert.ToDateTime(dsGCIData.Tables[2].Rows[0]["InsuredDOB4"]).ToString("dd/MM/yyyy"));
                                }
                                else
                                {

                                    htmlBody = htmlBody.Replace("{lblMemberCard3}", "");
                                }
                            }
                            else if (i == 4)
                            {
                                if (!string.IsNullOrEmpty(dsGCIData.Tables[2].Rows[0]["nInsured5MemberID"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredName5"].ToString()) && !string.IsNullOrWhiteSpace(dsGCIData.Tables[2].Rows[0]["InsuredDOB5"].ToString()))
                                {
                                    htmlBody = htmlBody.Replace("{lblMemberCard4}", lblMemberCard);
                                    htmlBody = htmlBody.Replace("{lblMemberID}", dsGCIData.Tables[2].Rows[0]["nInsured5MemberID"].ToString());
                                    htmlBody = htmlBody.Replace("{lblName}", dsGCIData.Tables[2].Rows[0]["InsuredName5"].ToString());
                                    htmlBody = htmlBody.Replace("{lblAge}", dsGCIData.Tables[2].Rows[0]["InsuredAge5"].ToString());
                                    htmlBody = htmlBody.Replace("{lblGender}", dsGCIData.Tables[2].Rows[0]["InsuredGender5"].ToString());
                                    htmlBody = htmlBody.Replace("{lblDOB}", Convert.ToDateTime(dsGCIData.Tables[2].Rows[0]["InsuredDOB5"]).ToString("dd/MM/yyyy"));

                                }
                                else
                                {
                                    htmlBody = htmlBody.Replace("{lblMemberCard4}", "");
                                }
                            }
                            htmlBody = htmlBody.Replace("{lblPolicyNo}", certificateNo);
                            htmlBody = htmlBody.Replace("{lblValidity}", dsGCIData.Tables[0].Rows[0]["dPolicyEndDate1"].ToString());
                            htmlBody = htmlBody.Replace("{lblServProvider}", ConfigurationManager.AppSettings["lblServiceProvider"].ToString());
                            htmlBody = htmlBody.Replace("{lblEmail}", ConfigurationManager.AppSettings["GHIEmailAdd"].ToString());
                            htmlBody = htmlBody.Replace("{lblWebsite}", ConfigurationManager.AppSettings["GHIWebsite"].ToString());
                            htmlBody = htmlBody.Replace("{lblTollFree}", ConfigurationManager.AppSettings["GHITollFreeNo"].ToString());
                            htmlBody = htmlBody.Replace("{lblCompanyName}", ConfigurationManager.AppSettings["lblCompanyName"].ToString());
                            htmlBody = htmlBody.Replace("{lblAddressL1}", ConfigurationManager.AppSettings["GHIAddress"].ToString());

                        }


                    }
                    #endregion

                    //CR_P1_450_Start Kuwar Tax Invoice generateGHI_PDF
                    StringBuilder taxinvoice = new StringBuilder();
                    string customerGSTIN = dsGCIData.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                    if (customerGSTIN == "")
                    {

                        taxinvoice.Append("<p style='page-break-before: always'></p>");
                        htmlBody = htmlBody.Replace("{pagebreak}", taxinvoice.ToString());
                        taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                        int temp = 0;
                        string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                        string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                        string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                        string totalPremium = dsGCIData.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString();
                        //  string totalPremium = TotalAmount;
                        if (totalPremium.Contains('.'))
                        {
                            temp = Convert.ToInt32(totalPremium.Substring(0, totalPremium.IndexOf('.')));

                        }
                        else
                        {
                            temp = Convert.ToInt32(totalPremium);
                        }

                        string totalPremiumInWord = ConvertAmountInWord(temp);

                        string suppliGSTN = dsGCIData.Tables[1].Rows[0]["vGSTRegistrationNo"].ToString();
                        string kgiStateCode = suppliGSTN.Substring(0, 2);
                        string buyerGSTN = "";
                        string transactionDate = dsGCIData.Tables[0].Rows[0]["vPolicyStartdate"].ToString();
                        int noofHSNCode = 0;
                        string hsnCode = dsGCIData.Tables[0].Rows[0]["vSacCode"].ToString();
                        string receiptNumber = dsGCIData.Tables[0].Rows[0]["vChallanNo"].ToString();
                        if (hsnCode != "")
                        {
                            var tempcount = hsnCode.Split(' ').Length;
                            for (int i = 0; i < tempcount; i++)
                            {
                                noofHSNCode++;
                            }

                        }

                        string Imagepath = string.Empty;
                        CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                        Imagepath = Imagepath == "error" ? "" : Imagepath;
                        string kgiStateName = string.Empty;
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ToString()))
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                                cmd.Connection = con;
                                con.Open();
                                object objStaeName = cmd.ExecuteScalar();
                                kgiStateName = Convert.ToString(objStaeName);

                            }
                        }
                        htmlBody = htmlBody.Replace("@divQRImagehtml", Imagepath);

                        //htmlBody = htmlBody.Replace("@divhtml", taxinvoice.ToString());
                        //htmlBody = htmlBody.Replace("{divhtml}", taxinvoice.ToString());
                        //taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");

                        htmlBody = htmlBody.Replace("{divhtml}", taxinvoice.ToString());


                        //generateGHI_PDF()

                        htmlBody = htmlBody.Replace("{gistinno}", dsGCIData.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                        htmlBody = htmlBody.Replace("{GSTcustomerId}", dsGCIData.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                        htmlBody = htmlBody.Replace("{customername}", dsGCIData.Tables[2].Rows[0]["vCustomerName"].ToString());
                        htmlBody = htmlBody.Replace("{emailId}", dsGCIData.Tables[0].Rows[0]["vEmailId"].ToString());
                        htmlBody = htmlBody.Replace("{contactno}", dsGCIData.Tables[0].Rows[0]["vMobileNo"].ToString());
                        htmlBody = htmlBody.Replace("{address}", dsGCIData.Tables[2].Rows[0]["ProposarAddress"].ToString());

                        htmlBody = htmlBody.Replace("{imdcode}", dsGCIData.Tables[1].Rows[0]["vIntermediaryCode"].ToString());
                        htmlBody = htmlBody.Replace("{receiptno}", dsGCIData.Tables[0].Rows[0]["vChallanNo"].ToString());
                        htmlBody = htmlBody.Replace("{customerstatecode}", dsGCIData.Tables[0].Rows[0]["NUM_State_CD"].ToString());

                        htmlBody = htmlBody.Replace("{supplyname}", dsGCIData.Tables[0].Rows[0]["TXT_State"].ToString());//not found in GHI Table

                        htmlBody = htmlBody.Replace("{name}", kgiName);
                        htmlBody = htmlBody.Replace("{KotakGstNo}", dsGCIData.Tables[1].Rows[0]["vGSTRegistrationNo"].ToString());// found in plan master table insta
                        htmlBody = htmlBody.Replace("{panNo}", kgiPanno);
                        htmlBody = htmlBody.Replace("{cinNo}", kgiCINno);
                        htmlBody = htmlBody.Replace("{vKGIBranchAddress}", dsGCIData.Tables[0].Rows[0]["vPolicyIssuedAt"].ToString());//kgibranch address
                        htmlBody = htmlBody.Replace("{invoicedate}", dsGCIData.Tables[0].Rows[0]["vPolicyStartdate"].ToString());
                        htmlBody = htmlBody.Replace("{invoiceno}", dsGCIData.Tables[0].Rows[0]["vCertificateNo"].ToString());
                        htmlBody = htmlBody.Replace("{proposalno}", dsGCIData.Tables[0].Rows[0]["vCertificateNo"].ToString());
                        htmlBody = htmlBody.Replace("{partnerappno}", "");
                        htmlBody = htmlBody.Replace("{statecode}", kgiStateCode);
                        htmlBody = htmlBody.Replace("{kgistatename}", kgiStateName);//gst state code of kotak
                        htmlBody = htmlBody.Replace("{irn}", dsGCIData.Tables[0].Rows[0]["vCertificateNo"].ToString());

                        htmlBody = htmlBody.Replace("{hsnDescription}", dsGCIData.Tables[0].Rows[0]["vTaxDescription"].ToString());
                        htmlBody = htmlBody.Replace("{HSNCode}", dsGCIData.Tables[0].Rows[0]["vSacCode"].ToString());

                        htmlBody = htmlBody.Replace("{totalpre}", dsGCIData.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                        htmlBody = htmlBody.Replace("{netamount}", dsGCIData.Tables[0].Rows[0]["nNetPremium"].ToString());
                        htmlBody = htmlBody.Replace("{NetPre}", dsGCIData.Tables[0].Rows[0]["nNetPremium"].ToString());
                        htmlBody = htmlBody.Replace("{totalgst}", dsGCIData.Tables[0].Rows[0]["TotalGSTAmount"].ToString());
                        // //generateGHI_PDF()

                        htmlBody = htmlBody.Replace("{cgstpercent}", dsGCIData.Tables[0].Rows[0]["CGSTPercentage"].ToString());
                        htmlBody = htmlBody.Replace("{ugstpercent}", dsGCIData.Tables[0].Rows[0]["UGSTPercentage"].ToString());
                        htmlBody = htmlBody.Replace("{sgstpercent}", dsGCIData.Tables[0].Rows[0]["SGSTPercentage"].ToString());
                        htmlBody = htmlBody.Replace("{igstpercent}", dsGCIData.Tables[0].Rows[0]["IGSTPercentage"].ToString());
                        //generateGHI_PDF()

                        htmlBody = htmlBody.Replace("{totalinvoicevalueinfigure}", dsGCIData.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                        htmlBody = htmlBody.Replace("{cgstamt}", dsGCIData.Tables[0].Rows[0]["CGSTamount"].ToString());
                        htmlBody = htmlBody.Replace("{ugstamt}", dsGCIData.Tables[0].Rows[0]["UGSTamount"].ToString());
                        htmlBody = htmlBody.Replace("{sgstamt}", dsGCIData.Tables[0].Rows[0]["SGSTamount"].ToString());
                        htmlBody = htmlBody.Replace("{igstamt}", dsGCIData.Tables[0].Rows[0]["IGSTamount"].ToString());

                        //htmlBody = htmlBody.Replace("{cessrate}", ds.Tables[0].Rows[0]["vSGST"].ToString());
                        //htmlBody = htmlBody.Replace("{cessamt}", ds.Tables[0].Rows[0]["vIGST"].ToString());
                        htmlBody = htmlBody.Replace("{cessrate}", "0");
                        htmlBody = htmlBody.Replace("{cessamt}", "0");
                        htmlBody = htmlBody.Replace("{totalgross}", totalPremium);// change1
                        htmlBody = htmlBody.Replace("{totalinvoicevalueinwords}", totalPremiumInWord.ToString());
                        //   strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                        //  strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                    }
                    else
                    {
                        taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: none'>");

                        htmlBody = htmlBody.Replace("{divhtml}", taxinvoice.ToString());
                    }
                    //CR_P1_450_End Kuwar Tax Invoice

                    TextWriter outTextWriter = new StringWriter();
                    HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);

                    // below code for download pdf
                    //base.Render(outHtmlTextWriter);
                    string currentPageHtmlString = htmlBody; //outTextWriter.ToString();
                                                             // Create a HTML to PDF converter object with default settings
                    HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
                    // Set license key received after purchase to use the converter in licensed mode
                    // Leave it not set to use the converter in demo mode
                    string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();
                    htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";
                                                                     // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                                                                     // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                    htmlToPdfConverter.ConversionDelay = 2;
                    // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                    htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
                    // Add Header
                    // Enable header in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                    // Optionally add a space between header and the page body
                    // The spacing for first page and the subsequent pages can be set independently
                    // Leave this option not set for no spacing
                    htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                    htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");
                    // Draw header elements
                    if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                        DrawHeader(htmlToPdfConverter, false);
                    // Add Footer
                    // Enable footer in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
                    // Optionally add a space between footer and the page body
                    // Leave this option not set for no spacing
                    htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");
                    // Draw footer elements
                    if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                        //  DrawFooter(htmlToPdfConverter, false, true);
                  // Added by Kuwar For GHI Footer to show UIN No in PDF
                        DrawFooterGHI(htmlToPdfConverter, false, true);
                    // Use the current page URL as base URL
                    string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                    // Convert the current page HTML string to a PDF document in a memory buffer
                    // For Live
                    byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                   byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                    // For Live End Here 

                    ////// For Dev
                    //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                    ////// byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                    ////// For Dev End here 

                    // Send the PDF as response to browser

                    // Set response content type
                    Response.AddHeader("Content-Type", "application/pdf");

                    // Instruct the browser to open the PDF file as an attachment or inline
                    //Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), txtCertificateNumber.Text.Trim()));

                    Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GHIPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNo.Trim().ToString()));

                    // Write the PDF document buffer to HTTP response
                    //Response.BinaryWrite(outPdfBuffer);
                    Response.BinaryWrite(outPdfBuffer);

                    // End the HTTP response and stop the current page processing
                    CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadGPAPolicy.aspx");//Added By Rajesh Soni 19/02/2020
                    Response.End();
                }
            }
            //GroupSecureShield_NewPolicyHeader_GHI


        }

        private void GenerateNonEmailHDC_Flotaer_PDF(SqlConnection con, DataSet ds, string strHtml, string certificateNo)
        {
            try
            {
                string accidentalDeath = string.Empty;
                string permTotalDisable = string.Empty;
                string permPartialDisable = string.Empty;
                string tempTotalDisable = string.Empty;
                string carraigeBody = string.Empty;
                string funeralExpense = string.Empty;
                string medicalExpense = string.Empty;
                string purchaseBlood = string.Empty;
                string transportation = string.Empty;
                string compassionate = string.Empty;
                string disappearance = string.Empty;
                string modifyResidence = string.Empty;
                string costOfSupport = string.Empty;
                string commonCarrier = string.Empty;
                string childrenGrant = string.Empty;
                string marraigeExpense = string.Empty;
                string sportsActivity = string.Empty;
                string widowHood = string.Empty;

                string ambulanceChargesString = string.Empty;
                string dailyCashString = string.Empty;
                string accidentalHospString = string.Empty;
                string opdString = string.Empty;
                string accidentalDentalString = string.Empty;
                string convalString = string.Empty;
                string burnsString = string.Empty;
                string brokenBones = string.Empty;
                string comaString = string.Empty;
                string domesticTravelString = string.Empty;
                string lossofEmployString = string.Empty;
                string onDutyCover = string.Empty;
                string legalExpenses = string.Empty;

                string reducingCoverString = string.Empty;
                string assignmentString = string.Empty;

                //gst
                string custStateCode = string.Empty;
                string igstString = string.Empty;
                string cgstsgstString = string.Empty;
                string cgstugstString = string.Empty;
                string vCustomerType = string.Empty;

                //CR_450_added By Kuwar_start
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "'";
                    cmd.Connection = con;
                    //sqlCon.Open();
                    object objCustState = cmd.ExecuteScalar();
                    custStateCode = Convert.ToString(objCustState);
                }
                
                //Cr_450 Added By Kuwar End


                #region HDC CERTIFICATE OF INSURANCE

                strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                strHtml = strHtml.Replace("@masterDate", ds.Tables[0].Rows[0]["vMasterPolicyIssueDate"].ToString());
                strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyIssueLocation"].ToString());
                strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vMasterPolicyHolder"].ToString());
                strHtml = strHtml.Replace("@Customers", ds.Tables[0].Rows[0]["vGroupType"].ToString());
                strHtml = strHtml.Replace("@GroupType", ds.Tables[0].Rows[0]["vGroupType"].ToString());
                vCustomerType = ds.Tables[0].Rows[0]["vCustomerType"].ToString();

                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::hdc certificate :" + certificateNo + " and generating pdf " + DateTime.Now + Environment.NewLine);


                string AccountNo = ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString() + " / " + ds.Tables[0].Rows[0]["vAccountNo"].ToString();
                if (AccountNo.Substring(0, 3) == " / ")
                {
                    AccountNo = AccountNo.Substring(2, AccountNo.Length - 2);
                }
                strHtml = strHtml.Replace("@MemberShipIDEmpNOAccNo", AccountNo);
                strHtml = strHtml.Replace("@CreditAmountOutStandingCreditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                strHtml = strHtml.Replace("@CreditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
                strHtml = strHtml.Replace("@DeductibleBaseCovers", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
                strHtml = strHtml.Replace("@DescriptionRemark", ds.Tables[0].Rows[0]["vComments"].ToString());
                strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
                strHtml = strHtml.Replace("@ProposarGSTN", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                //Added By Nilesh
                strHtml = strHtml.Replace("@ProposarMobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                strHtml = strHtml.Replace("@ProposarEmailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                //End By Nilesh
                strHtml = strHtml.Replace("@PolicyType", ds.Tables[0].Rows[0]["vPolicyType"].ToString() == "" ? "New" : Convert.ToString(ds.Tables[0].Rows[0]["vPolicyType"]));
                strHtml = strHtml.Replace("@PreviousPolicyNo", ds.Tables[0].Rows[0]["vAddCol2"].ToString() == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["vAddCol2"].ToString()));
                #endregion

                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::hdc certificate :" + certificateNo + " and details insured " + DateTime.Now + Environment.NewLine);

                #region DETAILS OF THE INSURED PERSON(S) UNDER THE POLICY

                strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
                strHtml = strHtml.Replace("@issuedBranchkgi", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
                strHtml = strHtml.Replace("@issuedDate", ds.Tables[0].Rows[0]["vTransactionDate"].ToString());
                strHtml = strHtml.Replace("@startDate", ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString());
                strHtml = strHtml.Replace("@endDate", ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString());
                strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString());// + ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                strHtml = strHtml.Replace("@FinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());
                strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
                strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                strHtml = strHtml.Replace("@gstin", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["vCustomerDob"].ToString());
                strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                strHtml = strHtml.Replace("@category", ds.Tables[0].Rows[0]["vCustomerCategory"].ToString());
                strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());
                strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeDOB"].ToString());
                strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vNomineeAppointee"].ToString());
                strHtml = strHtml.Replace("@deduct", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
                strHtml = strHtml.Replace("@cuAadhar", ds.Tables[0].Rows[0]["vCustomerAadhar"].ToString());
                strHtml = strHtml.Replace("@proposeradd", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());



                #endregion


                #region  INTERMEDIARY DETAILS
                strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryContact"].ToString());
                strHtml = strHtml.Replace("@intermediaryLandline", ds.Tables[0].Rows[0]["vIntermediaryLandline"].ToString());

                if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                {
                    strHtml = strHtml.Replace("KOTAK GROUP SMART CASH", "KOTAK GROUP SMART CASH – MICRO INSURANCE");
                    strHtml = strHtml.Replace("Kotak Group Smart Cash", "Kotak Group Smart Cash – Micro Insurance");
                }


                if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                {
                    strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash – Micro Insurance UIN:KOTHMGP076V011819");
                }
                else
                {
                    strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash UIN: KOTHLGP19014V011819");
                }

                #endregion

                #region  COVERAGE DETAILS

                #region Code for Covers

                StringBuilder coverstring = new StringBuilder();
                if (ds.Tables[1].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        coverstring.Append("<tr>");
                        if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "B")
                        {
                            coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Base Covers </strong></td>");
                            coverstring.Append("</tr><tr>");
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        else if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "O")
                        {

                            coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Optional Covers </strong></td>");
                            coverstring.Append("</tr><tr>");
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        else
                        {
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        coverstring.Append("</tr>");
                    }
                }

                #endregion

                strHtml = strHtml.Replace("@CoversString", coverstring.ToString());


                #region Code for Important Conditions

                strHtml = strHtml.Replace("@Condition1", ds.Tables[0].Rows[0]["vCondition1"].ToString());
                strHtml = strHtml.Replace("@Condition2", ds.Tables[0].Rows[0]["vCondition2"].ToString());
                strHtml = strHtml.Replace("@Condition3", ds.Tables[0].Rows[0]["vCondition3"].ToString());
                strHtml = strHtml.Replace("@Condition4", ds.Tables[0].Rows[0]["vCondition4"].ToString());
                strHtml = strHtml.Replace("@Condition5", ds.Tables[0].Rows[0]["vCondition5"].ToString());

                #endregion

                #endregion

                #region  PREMIUM DETAILS

                strHtml = strHtml.Replace("@cgstpercentagestring", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@ugstpercentagestring", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@sgstpercentagestring", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@igstpercentagestring", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());


                strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                strHtml = strHtml.Replace("@cgstsgstString", ds.Tables[0].Rows[0]["vCGST"].ToString());
                strHtml = strHtml.Replace("@ugstsgstString", ds.Tables[0].Rows[0]["vUGST"].ToString());
                strHtml = strHtml.Replace("@sgstsgstString", ds.Tables[0].Rows[0]["vSGST"].ToString());
                strHtml = strHtml.Replace("@igstsgstString", ds.Tables[0].Rows[0]["vIGST"].ToString());
                strHtml = strHtml.Replace("@TotalpremiumString", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());

                #endregion


                #region  TAX DETAILS

                strHtml = strHtml.Replace("@gstn", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
                strHtml = strHtml.Replace("@Category", ds.Tables[0].Rows[0]["vCategory"].ToString());
                strHtml = strHtml.Replace("@SACCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
                strHtml = strHtml.Replace("@Description", ds.Tables[0].Rows[0]["vDescription"].ToString());
                strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());


                strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["vStampDuty"].ToString());
                strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
                strHtml = strHtml.Replace("@challanDate", ds.Tables[0].Rows[0]["vChallanDate"].ToString());


                #endregion

                #region Floater Policy Details
                DataTable dtFloaterNominee = ds.Tables[2];


                string tbodyFloaterNominee = string.Empty;
                tbodyFloaterNominee = @"<tr><td style='border: 1px solid black;width:90px'> Insured Name </th> <td style='border: 1px solid black'> Insured Relationship</th> <td style='border: 1px solid black'> Insured Type </th> <td style='border: 1px solid black' width='12%'> DOB/AGE </th><td style='border: 1px solid black'> Gender </th><td style='border: 1px solid black'> Nominee Name </th><td style='border: 1px solid black' width='5%'> Nominee Relation </th><td style='border: 1px solid black' width='8%'> Nominee DOB/AGE </th></tr> ";

                foreach (DataRow dr in dtFloaterNominee.Rows)
                {
                    strHtml = strHtml.Replace("@ProposarPanAdhar", dr["CustomerPANorAdhar"].ToString());
                    strHtml = strHtml.Replace("@NameofFinancier", dr["vFinancerName"].ToString());
                    strHtml = strHtml.Replace("@ProposarName", dr["vCustomerName"].ToString());
                    strHtml = strHtml.Replace("@ProposarAddress", dr["ProposarAddress"].ToString());

                    if (!string.IsNullOrEmpty(dr["InsuredName1"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredName1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredRelation1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredDOB1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredGender1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeName1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeRelation1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeDOB1"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }


                    if (!string.IsNullOrEmpty(dr["InsuredName2"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB2"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName3"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB3"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }


                    if (!string.IsNullOrEmpty(dr["InsuredName4"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB4"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName5"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB5"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName6"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB6"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }

                }

                strHtml = strHtml.Replace("@tbody", tbodyFloaterNominee.ToString());
                //strHtml = strHtml.Replace("NULL", "");

                #endregion

                #region HDC RISK
                string _Date1 = ds.Tables[0].Rows[0]["vTransactionDate"].ToString();
                DateTime dtDateHDCRisk = Convert.ToDateTime(_Date1);

                string TransactionDateHDCRisk = dtDateHDCRisk.ToString("dd/MM/yyyy");
                strHtml = strHtml.Replace("@TransactionDateHDCRisk", TransactionDateHDCRisk);

                string mentionedGender = ds.Tables[0].Rows[0]["vCustomerGender"].ToString();
                if (string.IsNullOrEmpty(mentionedGender))
                {
                    strHtml = strHtml.Replace("@salutation", "");
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "M" || ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "Male")
                    {
                        strHtml = strHtml.Replace("@salutation", "Mr.");
                    }
                    else if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "F")
                    {
                        strHtml = strHtml.Replace("@salutation", "Mrs.");
                    }
                    else
                    {
                        strHtml = strHtml.Replace("@salutation", "");
                    }
                }
                strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                strHtml = strHtml.Replace("@addressofinsured1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                strHtml = strHtml.Replace("@addressofinsured2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                strHtml = strHtml.Replace("@addressofinsured3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                strHtml = strHtml.Replace("@addressofinsuredCity", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                strHtml = strHtml.Replace("@addressofinsuredState", ds.Tables[0].Rows[0]["vProposerState"].ToString());
                strHtml = strHtml.Replace("@Pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                strHtml = strHtml.Replace("@mobileno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());

                strHtml = strHtml.Replace("@productname", ds.Tables[0].Rows[0]["vProductName"].ToString());
                #endregion

                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::hdc certificate :" + certificateNo + " start of 80D section " + DateTime.Now + Environment.NewLine);

                #region HDC 80D CERTIFICATE
                string _Date = ds.Tables[0].Rows[0]["vAccountDebitDate"].ToString();
                DateTime dt = Convert.ToDateTime(_Date);

                string FDate = dt.ToString("dd/MM/yyyy");
                strHtml = strHtml.Replace("@ddateForRisk", FDate);
                strHtml = strHtml.Replace("@TotalPremium", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());
                strHtml = strHtml.Replace("@paymentmode", ds.Tables[0].Rows[0]["vPaymentMode"].ToString());
                int policytnur = Convert.ToInt32(ds.Tables[0].Rows[0]["vPolicyTenure"].ToString());
                //double totalpremium = Convert.ToDouble(ds.Tables[0].Rows[0]["vTotalPremium"].ToString());


                string startdate = ds.Tables[0].Rows[0]["vPolicyStartDate"].ToString();
                string enddate = ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString();

                DateTime date = Convert.ToDateTime(startdate);
                string startdateyear1 = date.Year.ToString();
                int MonthofStartYear = date.Month;
                DateTime date1 = Convert.ToDateTime(enddate);
                string enddateyear2 = date1.Year.ToString();
                int shortenddateyear2 = Convert.ToInt32(enddateyear2.Substring(2)) - 1;
                string year5 = Convert.ToString(shortenddateyear2);
                string FYForLUMSUMyear4;
                if (MonthofStartYear > 3)
                {
                    FYForLUMSUMyear4 = Convert.ToInt32(startdateyear1) + "-" + (Convert.ToInt32(startdateyear1) + 1);
                }
                else
                {
                    FYForLUMSUMyear4 = (Convert.ToInt32(startdateyear1) - 1) + "-" + (startdateyear1.Substring(2));
                }
                strHtml = strHtml.Replace("@Year", FYForLUMSUMyear4);

                int YearDuration = Convert.ToInt32(enddateyear2) - Convert.ToInt32(startdateyear1);
                string totalpremiumamount = ds.Tables[0].Rows[0]["vTotalPremium"].ToString();
                double totalpremiumamt = Convert.ToDouble(totalpremiumamount);
                double amount2 = totalpremiumamt / YearDuration;
                //string amount2 = Convert.ToString(amount1);
                double amount = Math.Round(amount2, 2);
                StringBuilder sb = new StringBuilder();
                sb.Append("<table style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri'>");
                sb.Append("<td style='width:200;border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                sb.Append("<p style='margin-left: 20px;'><span>Financial Year</span></p>");
                sb.Append("</td>");
                sb.Append("<td style='width:650;border:1px solid black;border-collapse:collapse;'>");
                sb.Append("<p style='margin-left: 20px;'><span>Year wise proportionate premium allowed for Deduction under Section 80D</span></p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                string FYForYearWiseLumsumDividendYear02;
                if (MonthofStartYear > 3)
                {
                    FYForYearWiseLumsumDividendYear02 = startdateyear1;
                }
                else
                {
                    int Yeart = Convert.ToInt32(startdateyear1) - 1;
                    FYForYearWiseLumsumDividendYear02 = Convert.ToString(Yeart);
                }
                for (int H = 0; H < YearDuration; H++)
                {
                    DataTable dt1 = new DataTable();
                    sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");

                    int Year00 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02) + H;
                    int sum = H + 1;
                    int Year01 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02.Substring(2)) + sum;

                    string year6 = Convert.ToString(Year00) + "-" + Convert.ToString(Year01);

                    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:200;'>");
                    sb.Append("<p style='margin-left: 20px;'> " + year6 + " </p>");
                    sb.Append("</td>");
                    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:650;'>");
                    sb.Append("<p style='margin-left: 20px;'> " + amount + " </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                strHtml = strHtml.Replace("@testHTMLTABLE", sb.ToString());
                #endregion
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::hdc certificate :" + certificateNo + " end of 80D section " + DateTime.Now + Environment.NewLine);

                strHtml = strHtml.Replace("@KotakGroupSmartCashUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupSmartCashUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupSmartCashUIN"]));

                #region TaxInvoice
                //CR_450_Start Kuwar
                //HDC_Floater_PDF_NonEmail
                StringBuilder taxinvoice = new StringBuilder();
                if (ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString() == "")
                {

                    taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                    int temp = 0;
                    string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                    string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                    string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                    string totalPremium = ds.Tables[0].Rows[0]["vTotalPremium"].ToString();

                    if (totalPremium.Contains('.'))
                    {
                        temp = Convert.ToInt32(totalPremium.Substring(0, totalPremium.IndexOf('.')));

                    }
                    else
                    {
                        temp = Convert.ToInt32(totalPremium);
                    }
                    string totalPremiumInWord = ConvertAmountInWord(temp);
                    //string stateCode = 

                    // QR Code
                    string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                    string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                    string transactionDate = ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString();
                    int noofHSNCode = 0;
                    string hsnCode = ds.Tables[0].Rows[0]["vSacCode"].ToString();
                    string receiptNumber = ds.Tables[0].Rows[0]["vChallanNo"].ToString();
                    if (hsnCode != "")
                    {
                        var tempcount = hsnCode.Split(' ').Length;
                        for (int i = 0; i < tempcount; i++)
                        {
                            noofHSNCode++;
                        }

                    }
                    
                    string Imagepath = string.Empty;
                    CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                    string kgiStateCode = suppliGSTN.Substring(0, 2); // getting kgi state code 
                    string kgiStateName = string.Empty;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                        cmd.Connection = con;
                        //sqlCon.Open();
                        object objStaeName = cmd.ExecuteScalar();
                        kgiStateName = Convert.ToString(objStaeName);
                        
                    }
                    Imagepath = Imagepath == "error" ? "" : Imagepath;
                    strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);


                    strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                    //HDC Policy
                    strHtml = strHtml.Replace("@gistinno", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                    strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                    strHtml = strHtml.Replace("@customername", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                    strHtml = strHtml.Replace("@emailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                    strHtml = strHtml.Replace("@contactno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                    strHtml = strHtml.Replace("@address", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());
                    strHtml = strHtml.Replace("@imdcode", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                    strHtml = strHtml.Replace("@receiptno", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
                    strHtml = strHtml.Replace("@customerstatecode", custStateCode);
                    //strHtml = strHtml.Replace("@customerstatecode", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst statecode of customer require
                    strHtml = strHtml.Replace("@supplyname", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst state name require of customer

                    //strHtml = strHtml.Replace("@gistinno", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                    strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
                    strHtml = strHtml.Replace("@name", kgiName);
                    strHtml = strHtml.Replace("@panNo", kgiPanno);
                    strHtml = strHtml.Replace("@cinNo", kgiCINno);
                    //strHtml = strHtml.Replace("@address", " ");
                    strHtml = strHtml.Replace("@vKGIBranchAddress", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
                    strHtml = strHtml.Replace("@invoicedate", ds.Tables[0].Rows[0]["vpolicyStartDate"].ToString());
                    strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                    strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["vAddCol1"].ToString());

                    strHtml = strHtml.Replace("@partnerappno", "");// this column is there as per jay
                    strHtml = strHtml.Replace("@statecode", kgiStateCode);//gst state code of kotak data coming from table
                    strHtml = strHtml.Replace("@statename", kgiStateName);//gst state code of kotak
                    strHtml = strHtml.Replace("@irn", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                    //HDC Policy
                    strHtml = strHtml.Replace("@hsnDescription", ds.Tables[0].Rows[0]["vDescription"].ToString());
                    strHtml = strHtml.Replace("@HSNCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
                    strHtml = strHtml.Replace("@totalpremium", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());
                    strHtml = strHtml.Replace("@netamount", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                    strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                    strHtml = strHtml.Replace("@totalgst", ds.Tables[0].Rows[0]["vTotalGSTAmount"].ToString());

                    strHtml = strHtml.Replace("@cgstpercent", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
                    strHtml = strHtml.Replace("@ugstpercent", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
                    strHtml = strHtml.Replace("@sgstpercent", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
                    strHtml = strHtml.Replace("@igstpercent", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());

                    //HDC Policy

                    strHtml = strHtml.Replace("@totalamount", totalPremium);
                    strHtml = strHtml.Replace("@cgstamt", ds.Tables[0].Rows[0]["vCGST"].ToString());
                    strHtml = strHtml.Replace("@ugstamt", ds.Tables[0].Rows[0]["vUGST"].ToString());
                    strHtml = strHtml.Replace("@sgstamt", ds.Tables[0].Rows[0]["vSGST"].ToString());
                    strHtml = strHtml.Replace("@igstamt", ds.Tables[0].Rows[0]["vIGST"].ToString());

                    //   strHtml = strHtml.Replace("@cessrate", ds.Tables[0].Rows[0]["vSGST"].ToString());
                    //  strHtml = strHtml.Replace("@cessamt", ds.Tables[0].Rows[0]["vIGST"].ToString());

                    strHtml = strHtml.Replace("@cessrate", "0");
                    strHtml = strHtml.Replace("@cessamt", "0");
                    strHtml = strHtml.Replace("@totalgross", totalPremium);// change1
                    strHtml = strHtml.Replace("@totalinvoicevalueinwords", totalPremiumInWord.ToString());
                    //   strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                    //  strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                }
                else
                {

                    taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: none'>");

                    strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                }
                //CR_450_End_Kuwar HDC Policy
                #endregion

                // below code for download pdf
                TextWriter outTextWriter = new StringWriter();
                HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                //base.Render(outHtmlTextWriter);
                string currentPageHtmlString = strHtml; //outTextWriter.ToString();
                                                        // Create a HTML to PDF converter object with default settings
                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
                // Set license key received after purchase to use the converter in licensed mode
                // Leave it not set to use the converter in demo mode
                string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();
                htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";
                                                                 // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                                                                 // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                htmlToPdfConverter.ConversionDelay = 2;
                // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
                // Add Header
                // Enable header in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                // Optionally add a space between header and the page body
                // The spacing for first page and the subsequent pages can be set independently
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");
                // Draw header elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                    DrawHeader(htmlToPdfConverter, false);
                // Add Footer
                // Enable footer in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
                // Optionally add a space between footer and the page body
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");
                // Draw footer elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                    DrawFooter(htmlToPdfConverter, false, true);
                // Use the current page URL as base URL
                string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                // Convert the current page HTML string to a PDF document in a memory buffer
                // For Live
                byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::hdc certificate :" + certificateNo + " and sending for sign " + DateTime.Now + Environment.NewLine);
                byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                // For Live End Here 

                //// For Dev
                //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                //// byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                //// For Dev End here 

                // Send the PDF as response to browser

                // Set response content type
                Response.AddHeader("Content-Type", "application/pdf");

                // Instruct the browser to open the PDF file as an attachment or inline
                //Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), txtCertificateNumber.Text.Trim()));

                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=HDCPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNo.Trim().ToString()));

                // Write the PDF document buffer to HTTP response
                //Response.BinaryWrite(outPdfBuffer);
                Response.BinaryWrite(outPdfBuffer);
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::hdc certificate :" + certificateNo + " sign pdf generated " + DateTime.Now + Environment.NewLine);
                // End the HTTP response and stop the current page processing
                CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadGPAPolicy.aspx");//Added By Rajesh Soni 19/02/2020
                Response.End();


            }
            catch (Exception ex)
            {
                // ExceptionUtility.LogException(ex, "GenerateNonEmailHDC_Flotaer_PDF");
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnGetPolicy ::hdc certificate error :" + ex.Message + " and stack trace: " + ex.StackTrace + " "  + DateTime.Now + Environment.NewLine);
            }
        }

        private void GenerateNonEmailHDCPDF(SqlConnection con, DataSet ds, string strHtml, string certificateNo)
        {
            string accidentalDeath = string.Empty;
            string permTotalDisable = string.Empty;
            string permPartialDisable = string.Empty;
            string tempTotalDisable = string.Empty;
            string carraigeBody = string.Empty;
            string funeralExpense = string.Empty;
            string medicalExpense = string.Empty;
            string purchaseBlood = string.Empty;
            string transportation = string.Empty;
            string compassionate = string.Empty;
            string disappearance = string.Empty;
            string modifyResidence = string.Empty;
            string costOfSupport = string.Empty;
            string commonCarrier = string.Empty;
            string childrenGrant = string.Empty;
            string marraigeExpense = string.Empty;
            string sportsActivity = string.Empty;
            string widowHood = string.Empty;

            string ambulanceChargesString = string.Empty;
            string dailyCashString = string.Empty;
            string accidentalHospString = string.Empty;
            string opdString = string.Empty;
            string accidentalDentalString = string.Empty;
            string convalString = string.Empty;
            string burnsString = string.Empty;
            string brokenBones = string.Empty;
            string comaString = string.Empty;
            string domesticTravelString = string.Empty;
            string lossofEmployString = string.Empty;
            string onDutyCover = string.Empty;
            string legalExpenses = string.Empty;

            string reducingCoverString = string.Empty;
            string assignmentString = string.Empty;

            //gst
            string custStateCode = string.Empty;
            string igstString = string.Empty;
            string cgstsgstString = string.Empty;
            string cgstugstString = string.Empty;

            //CR_450_added By Kuwar_start
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "'";
                cmd.Connection = con;
                // con.Open();
                object objCustState = cmd.ExecuteScalar();
                custStateCode = Convert.ToString(objCustState);
            }
            //Cr_450 Added By Kuwar End

            #region HDC CERTIFICATE OF INSURANCE

            strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
            strHtml = strHtml.Replace("@masterDate", ds.Tables[0].Rows[0]["vMasterPolicyIssueDate"].ToString());
            strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyIssueLocation"].ToString());
            strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vMasterPolicyHolder"].ToString());
            strHtml = strHtml.Replace("@Customers", ds.Tables[0].Rows[0]["vGroupType"].ToString());
            strHtml = strHtml.Replace("@GroupType", ds.Tables[0].Rows[0]["vGroupType"].ToString());



            #endregion


            #region DETAILS OF THE INSURED PERSON(S) UNDER THE POLICY

            strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
            strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
            strHtml = strHtml.Replace("@issuedBranchkgi", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
            strHtml = strHtml.Replace("@issuedDate", ds.Tables[0].Rows[0]["vTransactionDate"].ToString());
            strHtml = strHtml.Replace("@startDate", ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString());
            strHtml = strHtml.Replace("@endDate", ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString());
            strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString());// + ds.Tables[0].Rows[0]["vAccountNo"].ToString());
            strHtml = strHtml.Replace("@FinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());
            strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
            strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
            strHtml = strHtml.Replace("@gstin", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
            strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
            strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["vCustomerDob"].ToString());
            strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
            strHtml = strHtml.Replace("@category", ds.Tables[0].Rows[0]["vCustomerCategory"].ToString());
            strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
            strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
            strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());
            strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
            strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeDOB"].ToString());
            strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vNomineeAppointee"].ToString());
            strHtml = strHtml.Replace("@deduct", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
            strHtml = strHtml.Replace("@cuAadhar", ds.Tables[0].Rows[0]["vCustomerAadhar"].ToString());
            strHtml = strHtml.Replace("@proposeradd", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());



            #endregion


            #region  INTERMEDIARY DETAILS
            strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
            strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
            strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryContact"].ToString());
            strHtml = strHtml.Replace("@intermediaryLandline", ds.Tables[0].Rows[0]["vIntermediaryLandline"].ToString());

            if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
            {
                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash – Micro Insurance UIN:KOTHMGP076V011819");
            }
            else
            {
                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash UIN: KOTHLGP19014V011819");
            }

            #endregion

            #region  COVERAGE DETAILS

            #region Code for Covers

            StringBuilder coverstring = new StringBuilder();
            if (ds.Tables[1].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    coverstring.Append("<tr>");
                    if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "B")
                    {
                        coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Base Covers </strong></td>");
                        coverstring.Append("</tr><tr>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center;font-size:small'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                    }
                    else if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "O")
                    {

                        coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Optional Covers </strong></td>");
                        coverstring.Append("</tr><tr>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center;font-size:small'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                    }
                    else
                    {
                        coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center;font-size:small'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                    }
                    coverstring.Append("</tr>");
                }
            }

            #endregion

            strHtml = strHtml.Replace("@CoversString", coverstring.ToString());


            #region Code for Important Conditions

            strHtml = strHtml.Replace("@Condition1", ds.Tables[0].Rows[0]["vCondition1"].ToString());
            strHtml = strHtml.Replace("@Condition2", ds.Tables[0].Rows[0]["vCondition2"].ToString());
            strHtml = strHtml.Replace("@Condition3", ds.Tables[0].Rows[0]["vCondition3"].ToString());
            strHtml = strHtml.Replace("@Condition4", ds.Tables[0].Rows[0]["vCondition4"].ToString());
            strHtml = strHtml.Replace("@Condition5", ds.Tables[0].Rows[0]["vCondition5"].ToString());

            #endregion

            #endregion

            #region  PREMIUM DETAILS

            strHtml = strHtml.Replace("@cgstpercentagestring", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
            strHtml = strHtml.Replace("@ugstpercentagestring", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
            strHtml = strHtml.Replace("@sgstpercentagestring", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
            strHtml = strHtml.Replace("@igstpercentagestring", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());


            strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
            strHtml = strHtml.Replace("@cgstsgstString", ds.Tables[0].Rows[0]["vCGST"].ToString());
            strHtml = strHtml.Replace("@ugstsgstString", ds.Tables[0].Rows[0]["vUGST"].ToString());
            strHtml = strHtml.Replace("@sgstsgstString", ds.Tables[0].Rows[0]["vSGST"].ToString());
            strHtml = strHtml.Replace("@igstsgstString", ds.Tables[0].Rows[0]["vIGST"].ToString());
            strHtml = strHtml.Replace("@TotalpremiumString", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());

            #endregion


            #region  TAX DETAILS

            strHtml = strHtml.Replace("@gstn", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
            strHtml = strHtml.Replace("@Category", ds.Tables[0].Rows[0]["vCategory"].ToString());
            strHtml = strHtml.Replace("@SACCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
            strHtml = strHtml.Replace("@Description", ds.Tables[0].Rows[0]["vDescription"].ToString());
            strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());


            strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["vStampDuty"].ToString());
            strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
            strHtml = strHtml.Replace("@challanDate", ds.Tables[0].Rows[0]["vChallanDate"].ToString());


            #endregion

            #region TaxInvoice
            //CR_450_Start Kuwar
            //HDC_Floater_PDF_NonEmail
            StringBuilder taxinvoice = new StringBuilder();
            if (ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString() == "")
            {

                taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                int temp = 0;
                string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                string totalPremium = ds.Tables[0].Rows[0]["vTotalPremium"].ToString();

                if (totalPremium.Contains('.'))
                {
                    temp = Convert.ToInt32(totalPremium.Substring(0, totalPremium.IndexOf('.')));

                }
                else
                {
                    temp = Convert.ToInt32(totalPremium);
                }
                string totalPremiumInWord = ConvertAmountInWord(temp);
                //string stateCode = 

                // QR Code
                string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                string transactionDate = ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString();
                int noofHSNCode = 0;
                string hsnCode = ds.Tables[0].Rows[0]["vSacCode"].ToString();
                string receiptNumber = ds.Tables[0].Rows[0]["vChallanNo"].ToString();
                if (hsnCode != "")
                {
                    //check more than one sacCode****
                    var tempcount = hsnCode.Split(' ').Length;
                    for (int i = 0; i < tempcount; i++)
                    {
                        noofHSNCode++;
                    }

                }

                string Imagepath = string.Empty;
                CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                string kgiStateCode = suppliGSTN.Substring(0, 2); // getting kgi state code 
                string kgiStateName = string.Empty;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                    cmd.Connection = con;
                    //sqlCon.Open();
                    object objStaeName = cmd.ExecuteScalar();
                    kgiStateName = Convert.ToString(objStaeName);

                }
                Imagepath = Imagepath == "error" ? "" : Imagepath;
                strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);


                strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                //HDC Policy
                strHtml = strHtml.Replace("@gistinno", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                strHtml = strHtml.Replace("@customername", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                strHtml = strHtml.Replace("@emailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                strHtml = strHtml.Replace("@contactno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                strHtml = strHtml.Replace("@address", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());
                strHtml = strHtml.Replace("@imdcode", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                strHtml = strHtml.Replace("@receiptno", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
                strHtml = strHtml.Replace("@customerstatecode", custStateCode);
                //strHtml = strHtml.Replace("@customerstatecode", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst statecode of customer require
                strHtml = strHtml.Replace("@supplyname", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst state name require of customer

                //strHtml = strHtml.Replace("@gistinno", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                strHtml = strHtml.Replace("@KotakGstNo", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
                strHtml = strHtml.Replace("@name", kgiName);
                strHtml = strHtml.Replace("@panNo", kgiPanno);
                strHtml = strHtml.Replace("@cinNo", kgiCINno);

                strHtml = strHtml.Replace("@vKGIBranchAddress", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
                strHtml = strHtml.Replace("@invoicedate", ds.Tables[0].Rows[0]["vpolicyStartDate"].ToString());
                strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["vAddCol1"].ToString());

                strHtml = strHtml.Replace("@partnerappno", "");// this column is there as per jay
                strHtml = strHtml.Replace("@statecode", kgiStateCode);//gst state code of kotak data coming from table
                strHtml = strHtml.Replace("@statename", kgiStateName);//gst state code of kotak
                strHtml = strHtml.Replace("@irn", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                //HDC Policy
                strHtml = strHtml.Replace("@hsnDescription", ds.Tables[0].Rows[0]["vDescription"].ToString());
                strHtml = strHtml.Replace("@HSNCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
                strHtml = strHtml.Replace("@totalpremium", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());
                strHtml = strHtml.Replace("@netamount", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                strHtml = strHtml.Replace("@totalgst", ds.Tables[0].Rows[0]["vTotalGSTAmount"].ToString());

                strHtml = strHtml.Replace("@cgstpercent", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@ugstpercent", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@sgstpercent", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@igstpercent", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());

                //HDC Policy

                strHtml = strHtml.Replace("@totalamount", totalPremium);
                strHtml = strHtml.Replace("@cgstamt", ds.Tables[0].Rows[0]["vCGST"].ToString());
                strHtml = strHtml.Replace("@ugstamt", ds.Tables[0].Rows[0]["vUGST"].ToString());
                strHtml = strHtml.Replace("@sgstamt", ds.Tables[0].Rows[0]["vSGST"].ToString());
                strHtml = strHtml.Replace("@igstamt", ds.Tables[0].Rows[0]["vIGST"].ToString());

                //   strHtml = strHtml.Replace("@cessrate", ds.Tables[0].Rows[0]["vSGST"].ToString());
                //  strHtml = strHtml.Replace("@cessamt", ds.Tables[0].Rows[0]["vIGST"].ToString());

                strHtml = strHtml.Replace("@cessrate", "0");
                strHtml = strHtml.Replace("@cessamt", "0");
                strHtml = strHtml.Replace("@totalgross", totalPremium);// change1
                strHtml = strHtml.Replace("@totalinvoicevalueinwords", totalPremiumInWord.ToString());

            }
            else
            {

                taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: none'>");

                strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
            }
            //CR_450_End_Kuwar HDC Policy
            #endregion

            // below code for download pdf
            TextWriter outTextWriter = new StringWriter();
            HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
            //base.Render(outHtmlTextWriter);
            string currentPageHtmlString = strHtml; //outTextWriter.ToString();
                                                    // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();
            htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";
                                                             // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                                                             // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
            htmlToPdfConverter.ConversionDelay = 2;
            // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
            htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
            // Add Header
            // Enable header in the generated PDF document
            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
            // Optionally add a space between header and the page body
            // The spacing for first page and the subsequent pages can be set independently
            // Leave this option not set for no spacing
            htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
            htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");
            // Draw header elements
            if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                DrawHeader(htmlToPdfConverter, false);
            // Add Footer
            // Enable footer in the generated PDF document
            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            // Optionally add a space between footer and the page body
            // Leave this option not set for no spacing
            htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");
            // Draw footer elements
            if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                DrawFooter(htmlToPdfConverter, false, true);
            // Use the current page URL as base URL
            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            // Convert the current page HTML string to a PDF document in a memory buffer
            // For Live
            byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
            byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
            // For Live End Here 

            //// For Dev
            //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
            //// byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
            //// For Dev End here 

            // Send the PDF as response to browser

            // Set response content type
            Response.AddHeader("Content-Type", "application/pdf");

            // Instruct the browser to open the PDF file as an attachment or inline
            //Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), txtCertificateNumber.Text.Trim()));

            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=HDCPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNo.Trim().ToString()));

            // Write the PDF document buffer to HTTP response
            //Response.BinaryWrite(outPdfBuffer);
            Response.BinaryWrite(outPdfBuffer);

            // End the HTTP response and stop the current page processing
            CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadGPAGetPolicy.aspx");//Added By Rajesh Soni on 20/02/2020
            Response.End();


        }

        private byte[] convertToPdfNew(string currentPageHtmlString, string strBaseUrlString)
        {
            byte[] outPdfBuffer = null;
            try
            {
                //File.AppendAllText(folderPath + "\\log.txt", "start of convert to pdf method" + Environment.NewLine);

                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
                string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();
                htmlToPdfConverter.LicenseKey = winnovative_key;
                htmlToPdfConverter.ConversionDelay = 2;

                htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");
                //File.AppendAllText(folderPath + "\\log.txt", "assign header for the pdf start" + Environment.NewLine);

                if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                    DrawHeader(htmlToPdfConverter, false);

                htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                //File.AppendAllText(folderPath + "\\log.txt", "assign header for the pdf end" + Environment.NewLine);

                //File.AppendAllText(folderPath + "\\log.txt", "assign footer for the pdf start" + Environment.NewLine);

                if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                    DrawFooter(htmlToPdfConverter, false, true);


                //File.AppendAllText(folderPath + "\\log.txt", "assigd footer for the pdf end" + Environment.NewLine);

                string htmlString = currentPageHtmlString;
                string baseUrl = "";

                //File.AppendAllText(folderPath + "\\log.txt", "converting html" + Environment.NewLine);

                outPdfBuffer = htmlToPdfConverter.ConvertHtml(htmlString, baseUrl);

                //File.AppendAllText(folderPath + "\\log.txt", "bytes generation complete" + Environment.NewLine);


            }

            catch (Exception ex)
            {
                File.AppendAllText(folderPath + "\\log.txt", "Error occured in converto to pdf method : " + ex.Message + " and stack trace is : " + ex.StackTrace + Environment.NewLine);
            }

            return outPdfBuffer;
        }





        //code added for deigital sign
        static string licensekey = ConfigurationManager.AppSettings["license_key"].ToString();
        static pdfapi _oPdfapi = null;

        //code end for digital sign
        private void GetGPACertificateDetails(
              ref string PolicyIssuingOfficeAddress
            , ref string IntermediaryName
            , ref string IntermediaryCode
            , ref string PolicyholderName
            , ref string PolicyholderAddress
            , ref string PolicyholderAddress2
            , ref string PolicyholderBusinessDescription
            , ref string PolicyholderTelephoneNumber
            , ref string PolicyholderEmailAddress
            , ref string PolicyNumber
            , ref string PolicyInceptionDateTime
            , ref string PolicyExpiryDateTime
            , ref string TotalNumberOfInsuredPersons
            , ref string RowCoverHeader
            , ref string SectionARow
            , ref string ExtSectionARow
            , ref string SectionBRow
            , ref string NameofInsuredPerson
            , ref string DateOfBirth
            , ref string Gender
            , ref string EmailId
            , ref string MobileNo
            , ref string SumInsured
            , ref string NomineeDetails
            , ref string SectionACoverPremium
            , ref string ExtensionstoSectionASectionBCoverPremium
            , ref string LoadingsDiscounts
            , ref string ServiceTax
            , ref string SwachhBharatCess
            , ref string KrishiKalyanCess
            , ref string NetPremiumRoundedOff
            , ref string StampDuty
            , ref string Receipt_Challan_No
            , ref string Receipt_Challan_No_Dated
            , ref string PolicyIssueDate
            , ref string IntermediaryLandline
            , ref string IntermediaryMobile
            , ref string TotalAmount
            , ref bool IsCertificateNumberExists
            , ref string ugstPercentage
            , ref string ugstAmount
            , ref string cgstPercentage
            , ref string cgstAmount
            , ref string igstPercentage
            , ref string igstAmount
            , ref string sgstPercentage
            , ref string sgstAmount
            , ref string totalgstAmount
            , ref string vProposerPinCode
            , ref string addCol1
            , ref string polStartDate
             , ref string createdDate
            , ref string address1
            , ref string address2
            , ref string address3
            , ref string UINNo
            , ref string placeOfSupply
            , ref string proposalNo)
        {
            string strSectionACoverName = string.Empty; string strSectionACoverSI = string.Empty;
            string strExtSectionACoverName = string.Empty; string strExtSectionACoverSI = string.Empty;
            string strSectionBCoverName = string.Empty; string strSectionBCoverSI = string.Empty;
            string trCoverHeader = string.Empty;
            string trSectionARow = string.Empty;
            string trExtSectionARow = string.Empty;
            string trSectionBRow = string.Empty;
            //IsCertificateNumberExists = false;
            GetCoverSectionDetails(ref trCoverHeader, ref trSectionARow, ref trExtSectionARow, ref trSectionBRow);

            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_GPA_CERTIFICATE_DETAILS";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            db.AddParameter(dbCommand, "vCertificateNo", DbType.String, ParameterDirection.Input, "vCertificateNo", DataRowVersion.Current, certificateNo);
            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsCertificateNumberExists = true;
                    //lblSeatingCapacityt.Text = ds.Tables[0].Rows[0][0].ToString();

                    PolicyIssuingOfficeAddress = ds.Tables[0].Rows[0]["PolicyIssuingOfficeAddress"].ToString();
                    IntermediaryName = ds.Tables[0].Rows[0]["IntermediaryName"].ToString();
                    IntermediaryCode = ds.Tables[0].Rows[0]["IntermediaryCode"].ToString();

                    IntermediaryLandline = ds.Tables[0].Rows[0]["IntermediaryLandline"].ToString();
                    IntermediaryMobile = ds.Tables[0].Rows[0]["IntermediaryMobile"].ToString();

                    PolicyholderName = ds.Tables[0].Rows[0]["PolicyholderName"].ToString();
                    PolicyholderAddress = ds.Tables[0].Rows[0]["PolicyholderAddress"].ToString();
                    PolicyholderAddress2 = ds.Tables[0].Rows[0]["PolicyholderAddress2"].ToString();
                    PolicyholderBusinessDescription = ds.Tables[0].Rows[0]["PolicyholderBusinessDescription"].ToString();
                    PolicyholderTelephoneNumber = ds.Tables[0].Rows[0]["PolicyholderTelephoneNumber"].ToString();
                    PolicyholderEmailAddress = ds.Tables[0].Rows[0]["PolicyholderEmailAddress"].ToString();
                    PolicyNumber = ds.Tables[0].Rows[0]["PolicyNumber"].ToString();
                    PolicyInceptionDateTime = ds.Tables[0].Rows[0]["PolicyInceptionDateTime"].ToString();
                    PolicyExpiryDateTime = ds.Tables[0].Rows[0]["PolicyExpiryDateTime"].ToString();
                    TotalNumberOfInsuredPersons = ds.Tables[0].Rows[0]["TotalNumberOfInsuredPersons"].ToString();
                    RowCoverHeader = trCoverHeader;
                    SectionARow = trSectionARow;
                    ExtSectionARow = trExtSectionARow;
                    SectionBRow = trSectionBRow;
                    NameofInsuredPerson = ds.Tables[0].Rows[0]["NameofInsuredPerson"].ToString();
                    DateOfBirth = ds.Tables[0].Rows[0]["DateOfBirth"].ToString();
                    Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                    EmailId = ds.Tables[0].Rows[0]["EmailId"].ToString();
                    MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                    SumInsured = ds.Tables[0].Rows[0]["SumInsured"].ToString();
                    NomineeDetails = ds.Tables[0].Rows[0]["NomineeDetails"].ToString();
                    SectionACoverPremium = ds.Tables[0].Rows[0]["SectionACoverPremium"].ToString();
                    ExtensionstoSectionASectionBCoverPremium = ds.Tables[0].Rows[0]["ExtensionstoSectionASectionBCoverPremium"].ToString();
                    LoadingsDiscounts = ds.Tables[0].Rows[0]["LoadingsDiscounts"].ToString();
                    ServiceTax = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                    SwachhBharatCess = ds.Tables[0].Rows[0]["SwachhBharatCess"].ToString();
                    KrishiKalyanCess = ds.Tables[0].Rows[0]["KrishiKalyanCess"].ToString();
                    NetPremiumRoundedOff = ds.Tables[0].Rows[0]["NetPremiumRoundedOff"].ToString();
                    StampDuty = ds.Tables[0].Rows[0]["StampDuty"].ToString();
                    Receipt_Challan_No = ds.Tables[0].Rows[0]["Receipt_Challan_No"].ToString();
                    Receipt_Challan_No_Dated = ds.Tables[0].Rows[0]["Receipt_Challan_No_Dated"].ToString();
                    PolicyIssueDate = ds.Tables[0].Rows[0]["PolicyIssueDate"].ToString();
                    TotalAmount = ds.Tables[0].Rows[0]["TotalAmount"].ToString();
                    ugstPercentage = ds.Tables[0].Rows[0]["ugstPercentage"].ToString();
                    ugstAmount = ds.Tables[0].Rows[0]["ugstAmount"].ToString();
                    cgstPercentage = ds.Tables[0].Rows[0]["cgstPercentage"].ToString();
                    cgstAmount = ds.Tables[0].Rows[0]["cgstAmount"].ToString();
                    sgstPercentage = ds.Tables[0].Rows[0]["sgstPercentage"].ToString();
                    sgstAmount = ds.Tables[0].Rows[0]["sgstAmount"].ToString();
                    igstPercentage = ds.Tables[0].Rows[0]["igstPercentage"].ToString();
                    igstAmount = ds.Tables[0].Rows[0]["igstAmount"].ToString();
                    totalgstAmount = ds.Tables[0].Rows[0]["totalGSTAmount"].ToString();
                    vProposerPinCode = ds.Tables[0].Rows[0]["vProposerPinCode"].ToString();
                    addCol1 = ds.Tables[0].Rows[0]["addCol1"].ToString();
                    polStartDate = ds.Tables[0].Rows[0]["polStartDate"].ToString();
                    createdDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["createdDate"]).ToString("dd-MMM-yyyy");
                    address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                    address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                    address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                    UINNo = ds.Tables[0].Rows[0]["UINNo"].ToString();
                    // CR_450_Kuwar GPA
                    placeOfSupply = ds.Tables[0].Rows[0]["PlaceOfSupply"].ToString();
                    proposalNo = ds.Tables[0].Rows[0]["Additional_column_4"].ToString();
                    // CR_450_Kuwar GPA
                }
            }
        }

        private void GetCoverSectionDetails(ref string trRowCoverHeader, ref string trSectionARow, ref string trExtSectionARow, ref string trSectionBRow)
        {
            bool IsCoverAvailable = false;
            string strSectionACoverName = string.Empty; string strSectionACoverSI = string.Empty; string strSectionACoverSIText = string.Empty;
            string strExtSectionACoverName = string.Empty; string strExtSectionACoverSI = string.Empty; string strExtSectionACoverSIText = string.Empty;
            string strSectionBCoverName = string.Empty; string strSectionBCoverSI = string.Empty; string strSectionBCoverSIText = string.Empty;

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand = "PROC_GET_COVER_SECTION_DATA_FOR_PDF";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            db.AddParameter(dbCommand, "vCertificateNo", DbType.String, ParameterDirection.Input, "vCertificateNo", DataRowVersion.Current, certificateNo);
            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            strSectionACoverSIText = string.IsNullOrEmpty(dr["COVER_SI_TEXT"].ToString().Trim()) ? "" : "(" + dr["COVER_SI_TEXT"].ToString() + ")";
                            if (strSectionACoverName.Length == 0) //if first loop then no br tag else for line break br is added
                            {
                                strSectionACoverName = dr["NAME_OF_BENEFIT"].ToString();
                                strSectionACoverSI = Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionACoverSIText;
                            }
                            else
                            {
                                strSectionACoverName = strSectionACoverName + "<br>" + dr["NAME_OF_BENEFIT"].ToString();
                                strSectionACoverSI = strSectionACoverSI + "<br>" + Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionACoverSIText;
                            }
                        }
                        string td1SectionARow = "<td style='border: 1px solid black' width='20%'><p>Section A</p></td>";
                        string td2SectionARow = "<td style='border: 1px solid black' width='39%'><p>" + strSectionACoverName + "</p></td>";
                        string td3SectionARow = "<td style='border: 1px solid black;text-align:right' width='39%'><p>" + strSectionACoverSI + "</p></td>";
                        trSectionARow = "<tr>" + td1SectionARow + td2SectionARow + td3SectionARow + "</tr>";
                        IsCoverAvailable = true;
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strExtSectionACoverSIText = string.IsNullOrEmpty(dr["COVER_SI_TEXT"].ToString().Trim()) ? "" : "(" + dr["COVER_SI_TEXT"].ToString() + ")";
                            if (strExtSectionACoverName.Length == 0)
                            {
                                strExtSectionACoverName = dr["NAME_OF_BENEFIT"].ToString();
                                strExtSectionACoverSI = Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strExtSectionACoverSIText;
                            }
                            else
                            {
                                strExtSectionACoverName = strExtSectionACoverName + "<br>" + dr["NAME_OF_BENEFIT"].ToString();
                                strExtSectionACoverSI = strExtSectionACoverSI + "<br>" + Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strExtSectionACoverSIText;
                            }
                        }

                        string td1ExtSectionARow = "<td style='border: 1px solid black' width='20%'><p>Extensions under Section A</p></td>";
                        string td2ExtSectionARow = "<td style='border: 1px solid black' width='39%'><p>" + strExtSectionACoverName + "</p></td>";
                        string td3ExtSectionARow = "<td style='border: 1px solid black;text-align:right' width='39%'><p>" + strExtSectionACoverSI + "</p></td>";
                        trExtSectionARow = "<tr>" + td1ExtSectionARow + td2ExtSectionARow + td3ExtSectionARow + "</tr>";
                        IsCoverAvailable = true;
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            strSectionBCoverSIText = string.IsNullOrEmpty(dr["COVER_SI_TEXT"].ToString().Trim()) ? "" : "(" + dr["COVER_SI_TEXT"].ToString() + ")";
                            if (strSectionBCoverName.Length == 0)
                            {
                                strSectionBCoverName = dr["NAME_OF_BENEFIT"].ToString();
                                strSectionBCoverSI = Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionBCoverSIText;
                            }
                            else
                            {
                                strSectionBCoverName = strSectionBCoverName + "<br>" + dr["NAME_OF_BENEFIT"].ToString();
                                strSectionBCoverSI = strSectionBCoverSI + "<br>" + Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionBCoverSIText;
                            }
                        }

                        string td1SectionBRow = "<td style='border: 1px solid black' width='20%'><p>Section B</p></td>";
                        string td2SectionBRow = "<td style='border: 1px solid black' width='39%'><p>" + strSectionBCoverName + "</p></td>";
                        string td3SectionBRow = "<td style='border: 1px solid black;text-align:right' width='39%'><p>" + strSectionBCoverSI + "</p></td>";
                        trSectionBRow = "<tr>" + td1SectionBRow + td2SectionBRow + td3SectionBRow + "</tr>";
                        IsCoverAvailable = true;
                    }
                }
            }

            if (IsCoverAvailable)
            {
                string td1CoverHeader = "<td style='border:1px solid black' width='20%'><p><strong>Coverage Details</strong></p></td>";
                string td2CoverHeader = "<td style='border:1px solid black' width='39%'><p><strong>Name of the Benefit</strong></p></td>";
                string td3CoverHeader = "<td style='border:1px solid black;text-align:center' width='39%'><p><strong>Sum Insured (&#8377;)</strong></p></td>";
                trRowCoverHeader = "<tr>" + td1CoverHeader + td2CoverHeader + td3CoverHeader + "</tr>";
            }
        }

        void htmlToPdfConverter_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
        {
            // Set the header visibility in first, odd and even pages
            if (true)
            {
                if (eventParams.PageNumber == 1)
                    eventParams.Page.ShowHeader = true;
                else if ((eventParams.PageNumber % 2) == 0 && !true)
                    eventParams.Page.ShowHeader = false;
                else if ((eventParams.PageNumber % 2) == 1 && !true)
                    eventParams.Page.ShowHeader = false;
            }

            // Set the footer visibility in first, odd and even pages
            if (true)
            {
                if (eventParams.PageNumber == 1)
                    eventParams.Page.ShowFooter = true;
                else if ((eventParams.PageNumber % 2) == 0 && !true)
                    eventParams.Page.ShowFooter = false;
                else if ((eventParams.PageNumber % 2) == 1 && !true)
                    eventParams.Page.ShowFooter = false;
            }
        }

        private void DrawHeader(HtmlToPdfConverter htmlToPdfConverter, bool drawHeaderLine)
        {
            string headerHtmlUrl = IsWithoutHeaderFooter ? Server.MapPath("~/Header_HTML_WithoutHeader.html") : Server.MapPath("~/Header_HTML.html");

            // Set the header height in points
            htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 60;

            // Set header background color
            System.Drawing.Color colour = IsWithoutHeaderFooter ? ColorTranslator.FromHtml("#ffffff") : ColorTranslator.FromHtml("#ec3237"); ;
            htmlToPdfConverter.PdfHeaderOptions.HeaderBackColor = colour; // System.Drawing.Color.Red;

            // Create a HTML element to be added in header
            HtmlToPdfElement headerHtml = new HtmlToPdfElement(headerHtmlUrl);

            // Set the HTML element to fit the container height
            headerHtml.FitHeight = true;

            // Add HTML element to header
            htmlToPdfConverter.PdfHeaderOptions.AddElement(headerHtml);

            if (drawHeaderLine)
            {
                // Calculate the header width based on PDF page size and margins
                float headerWidth = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width -
                            htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin;

                // Calculate header height
                float headerHeight = htmlToPdfConverter.PdfHeaderOptions.HeaderHeight;

                // Create a line element for the bottom of the header
                LineElement headerLine = new LineElement(0, headerHeight - 1, headerWidth, headerHeight - 1);

                // Set line color
                headerLine.ForeColor = System.Drawing.Color.Gray;

                // Add line element to the bottom of the header
                htmlToPdfConverter.PdfHeaderOptions.AddElement(headerLine);
            }
        }

        private void DrawFooter(HtmlToPdfConverter htmlToPdfConverter, bool addPageNumbers, bool drawFooterLine)
        {
            string footerHtmlUrl = IsWithoutHeaderFooter ? Server.MapPath("~/Header_HTML_WithoutFooter.html") : Server.MapPath("~/Footer_HTML.html");
            // Set the footer height in points
            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 60;

            // Set footer background color
            htmlToPdfConverter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.White;

            // Create a HTML element to be added in footer
            HtmlToPdfElement footerHtml = new HtmlToPdfElement(footerHtmlUrl);

            // Set the HTML element to fit the container height
            footerHtml.FitHeight = true;

            // Add HTML element to footer
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerHtml);

            // Add page numbering
            if (addPageNumbers)
            {
                // Create a text element with page numbering place holders &p; and & P;
                TextElement footerText = new TextElement(0, 30, "Page &p; of &P;  ",
                    new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));

                // Align the text at the right of the footer
                footerText.TextAlign = HorizontalTextAlign.Right;

                // Set page numbering text color
                footerText.ForeColor = System.Drawing.Color.Navy;

                // Embed the text element font in PDF
                footerText.EmbedSysFont = true;

                // Add the text element to footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerText);
            }

            drawFooterLine = IsWithoutHeaderFooter ? false : true;
            if (drawFooterLine)
            {
                // Calculate the footer width based on PDF page size and margins
                float footerWidth = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width -
                            htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin;

                // Create a line element for the top of the footer
                LineElement footerLine = new LineElement(0, 0, footerWidth, 0);

                // Set line color
                footerLine.ForeColor = System.Drawing.Color.Gray;

                // Add line element to the bottom of the footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerLine);
            }
        }

        private void GenerateGPAPotectPDF(string cert)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            try
            {
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();
                    string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_CompleteLetter_With_GST - Copy.html";
                    string htmlBody = File.ReadAllText(strPath);
                    StringWriter sw = new StringWriter();
                    StringReader sr = new StringReader(sw.ToString());
                    string strHtml = htmlBody;

                    SqlCommand command = new SqlCommand("PROC_GET_COVER_SECTION_DATA_FOR_PDF_TEST", con);
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                    command.Parameters.AddWithValue("@vCertificateNo", cert);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            //  GenerateEmailPDF(con, ds, strHtml, certNo, emailID);

                            string accidentalDeath = string.Empty;
                            string permTotalDisable = string.Empty;
                            string permPartialDisable = string.Empty;
                            string tempTotalDisable = string.Empty;
                            string carraigeBody = string.Empty;
                            string funeralExpense = string.Empty;
                            string medicalExpense = string.Empty;
                            string purchaseBlood = string.Empty;
                            string transportation = string.Empty;
                            string compassionate = string.Empty;
                            string disappearance = string.Empty;
                            string modifyResidence = string.Empty;
                            string costOfSupport = string.Empty;
                            string commonCarrier = string.Empty;
                            string childrenGrant = string.Empty;
                            string marraigeExpense = string.Empty;
                            string sportsActivity = string.Empty;
                            string widowHood = string.Empty;

                            string ambulanceChargesString = string.Empty;
                            string dailyCashString = string.Empty;
                            string accidentalHospString = string.Empty;
                            string opdString = string.Empty;
                            string accidentalDentalString = string.Empty;
                            string convalString = string.Empty;
                            string burnsString = string.Empty;
                            string brokenBones = string.Empty;
                            string comaString = string.Empty;
                            string domesticTravelString = string.Empty;
                            string lossofEmployString = string.Empty;
                            string onDutyCover = string.Empty;
                            string legalExpenses = string.Empty;

                            string reducingCoverString = string.Empty;
                            string assignmentString = string.Empty;

                            //gst
                            string custStateCode = string.Empty;
                            string igstString = string.Empty;
                            string cgstsgstString = string.Empty;
                            string cgstugstString = string.Empty;

                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "'";
                                cmd.Connection = con;
                                //sqlCon.Open();
                                object objCustState = cmd.ExecuteScalar();
                                custStateCode = Convert.ToString(objCustState);
                            }

                            strHtml = strHtml.Replace("@createdDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dCreatedDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@customerName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeAge"].ToString());
                            strHtml = strHtml.Replace("@masterDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["vMasterPolicyDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());


                            strHtml = strHtml.Replace("@addressline1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@addressline2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());

                            strHtml = strHtml.Replace("@addressline3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                            strHtml = strHtml.Replace("@city", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                            strHtml = strHtml.Replace("@pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());
                            strHtml = strHtml.Replace("@state", ds.Tables[0].Rows[0]["vProposerState"].ToString());

                            strHtml = strHtml.Replace("@mobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@emailID", ds.Tables[0].Rows[0]["vEmailId"].ToString());

                            //strHtml = strHtml.Replace("@productName", ds.Tables[0].Rows[0]["vProductName"].ToString()); //done changes for cert no
                            //strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@productName", "KOTAK GROUP ACCIDENT PROTECT"); //done changes for cert no
                            strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vpolicyType"].ToString() == "" ? "New" : ds.Tables[0].Rows[0]["vpolicyType"].ToString());

                            //manish start
                            //   strHtml = strHtml.Replace("@Enroll", PolicyInceptionDateTime.Substring(24));
                            //manish end
                            strHtml = strHtml.Replace("@prevPolicyNo", ds.Tables[0].Rows[0]["vprevPolicyNumber"].ToString());
                            strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyLoc"].ToString());


                            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderAddress", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString()); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@PolicyholderLine2Address", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerCity"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerState"].ToString() + "(" + custStateCode + ")-" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                            strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderEmailAddress", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                            //  strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@policyStartDate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@policyEndDate", ds.Tables[0].Rows[0]["dPolicyEndDate"].ToString());

                            strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                            strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());


                            strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                            strHtml = strHtml.Replace("@occupation", ds.Tables[0].Rows[0]["vOccupation"].ToString());

                            strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                            strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["dCustomerDob"].ToString());
                            strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                            strHtml = strHtml.Replace("@category", "");
                            strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                            //strHtml = strHtml.Replace("@sumInsured", ds.Tables[0].Rows[0]["nPlanSI"].ToString());
                            //strHtml = strHtml.Replace("@sumInsured", Convert.ToDecimal(ds.Tables[0].Rows[0]["nPlanSI"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@sumInsured", "");


                            strHtml = strHtml.Replace("@sumBasis", ds.Tables[0].Rows[0]["vSIBasis"].ToString());

                            strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                            strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());

                            strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                            //strHtml = strHtml.Replace("@nomineeRelDOB", "");
                            strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vAppointeeName"].ToString());

                            string igstPercentage = ds.Tables[0].Rows[0]["igstPercentage"].ToString();
                            string cgstPercentage = ds.Tables[0].Rows[0]["cgstPercentage"].ToString();
                            string sgstPercentage = ds.Tables[0].Rows[0]["sgstPercentage"].ToString();
                            string ugstPercentage = ds.Tables[0].Rows[0]["ugstPercentage"].ToString();
                            string igstAmount = ds.Tables[0].Rows[0]["igstAmount"].ToString();
                            string cgstAmount = ds.Tables[0].Rows[0]["cgstAmount"].ToString();
                            string sgstAmount = ds.Tables[0].Rows[0]["sgstAmount"].ToString();
                            string ugstAmount = ds.Tables[0].Rows[0]["ugstAmount"].ToString();

                            if (igstPercentage != "0")
                            {
                                igstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center'>IGST@" + igstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'>" + igstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && sgstPercentage != "0")
                            {
                                cgstsgstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center'>SGST@" + sgstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'>" + sgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && ugstPercentage != "0")
                            {
                                cgstugstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center'>UGST@" + ugstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'>" + ugstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            strHtml = strHtml.Replace("@igstString", igstString == "" ? "" : igstString);
                            strHtml = strHtml.Replace("@cgstsgstString", cgstsgstString == "" ? "" : cgstsgstString);
                            strHtml = strHtml.Replace("@cgstugstString", cgstugstString == "" ? "" : cgstugstString);

                            string policyIssuance = ds.Tables[0].Rows[0]["vAdditional_column_1"].ToString();
                            string customString = string.Empty;

                            if (!String.IsNullOrEmpty(policyIssuance))
                            {
                                string[] strArr = policyIssuance.Split(' ');
                                if (String.IsNullOrEmpty(strArr[1]))
                                {
                                    customString = "this " + strArr[2] + " day of " + strArr[0] + " of year " + strArr[3];
                                }
                                else
                                {
                                    customString = "this " + strArr[1] + " day of " + strArr[0] + " of year " + strArr[2];
                                }

                            }

                            strHtml = strHtml.Replace("@polIssueString", customString);



                            if (ds.Tables[0].Rows[0]["vAccidentalDeathAD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString().Trim()))
                                {
                                    //accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString() + ")</p></td></tr>";

                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vPermTotalDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString().Trim()))
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString() + ")</p></td></tr> ";
                                }
                                else
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString())) + "</p></td></tr> ";
                                }
                            }
                            //
                            if (ds.Tables[0].Rows[0]["vPermPartialDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString().Trim()))
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vTempTotalDisableTTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString().Trim()))
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCarraigeDeadBody"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString().Trim()))
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vFuneralExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString().Trim()))
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalMedicalExp"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString().Trim()))
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vPurchaseofBlood"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPurchaseofBloodSIText"].ToString().Trim()))
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPurchaseofBloodSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vTransportationofImpMedicine"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTransportationofImpMedicineSIText"].ToString().Trim()))
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vTransportationofImpMedicineSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCompassionateVisit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString().Trim()))
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDisappearanceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString().Trim()))
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString() + "</p></td></tr>";
                                }

                            }

                            if (ds.Tables[0].Rows[0]["vModificationofVehicle"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString().Trim()))
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCostSupportItems"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString().Trim()))
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCommonCarrier"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString().Trim()))
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vChildEduGrant"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString().Trim()))
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vMarraigeExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString().Trim()))
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSportsActivityCover"].ToString() == "Y")
                            {
                                //if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vSportsActivityCoverSIText"].ToString()))
                                //{
                                //    sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>"+ ds.Tables[0].Rows[0]["nSportsActivityCoverSI"].ToString() + "&nbsp;("+ ds.Tables[0].Rows[0]["vSportsActivityCoverSIText"].ToString() + ")</p></td></tr>";
                                //}
                                //  else
                                //  {
                                //sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>"+ ds.Tables[0].Rows[0]["nSportsActivityCoverSI"].ToString() + "</p></td></tr>";
                                sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                                // }
                            }

                            if (ds.Tables[0].Rows[0]["vWidowhoodCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString().Trim()))
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAmbulanceCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString().Trim()))
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vDailyCashBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString().Trim()))
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalHospitalization"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString().Trim()))
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOPDTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString().Trim()))
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalDentalExpense"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString().Trim()))
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vConvalescenceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString().Trim()))
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBurnBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString().Trim()))
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBrokenBones"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString().Trim()))
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vComa"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vComaSIText"].ToString().Trim()))
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nComaSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vComaSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nComaSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDomesticTravelForTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString().Trim()))
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString().Trim()))
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOnDutyCover"].ToString() == "Y")
                            {
                                //if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOnDutyCoverSIText"].ToString()))
                                //{
                                //    onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOnDutyCoverSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vOnDutyCoverSIText"].ToString() + ")</p></td></tr>";
                                //}
                                //  else
                                //  {
                                //onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOnDutyCoverSI"].ToString() + "</p></td></tr>";
                                onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                                // }
                            }

                            if (ds.Tables[0].Rows[0]["vLegalExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString().Trim()))
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSIBasis"].ToString().ToLower() == "reducing")
                            {
                                reducingCoverString = "<tr><td style='border:1px solid black' width='10%'><p  style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>8.23-Reducing Sum Insured Covers</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vProposalType"].ToString().ToLower() == "credit linked")
                            {
                                assignmentString = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>8.22-Assignment</p></td></tr>";
                            }

                            #region commented code
                            //    strHtml = strHtml.Replace("@accidentalDeath", ds.Tables[0].Rows[0]["vAccidentalDeathAD"].ToString());
                            //  strHtml = strHtml.Replace("@permanentDisable", ds.Tables[0].Rows[0]["vPermTotalDisablePTD"].ToString());
                            // strHtml = strHtml.Replace("@permanentpartialDisable", ds.Tables[0].Rows[0]["vPermPartialDisablePTD"].ToString());
                            // strHtml = strHtml.Replace("@temptotalDisable", ds.Tables[0].Rows[0]["vTempTotalDisableTTD"].ToString());
                            //strHtml = strHtml.Replace("@carraige", ds.Tables[0].Rows[0]["vCarriageOfDeadBody"].ToString());
                            //strHtml = strHtml.Replace("@funeral", ds.Tables[0].Rows[0]["vFuneralExpenses"].ToString());
                            //strHtml = strHtml.Replace("@accMedicalExpense", ds.Tables[0].Rows[0]["vAccidentalMedicalExp"].ToString());
                            //strHtml = strHtml.Replace("@purchaseblood", ds.Tables[0].Rows[0]["vPurchaseofBlood"].ToString());

                            //strHtml = strHtml.Replace("@transportation", ds.Tables[0].Rows[0]["vTransportationofImpMedicine"].ToString());
                            //strHtml = strHtml.Replace("@compassionate", ds.Tables[0].Rows[0]["vCompassionateVisit"].ToString());
                            //strHtml = strHtml.Replace("@disappearance", ds.Tables[0].Rows[0]["vDisappearanceBenefit"].ToString());
                            //strHtml = strHtml.Replace("@modifyresidence", ds.Tables[0].Rows[0]["vModificationofVehicle"].ToString());
                            //strHtml = strHtml.Replace("@costofSupport", ds.Tables[0].Rows[0]["vCostOfSupportItems"].ToString());
                            //strHtml = strHtml.Replace("@commonCarrier", ds.Tables[0].Rows[0]["vCommonCarrier"].ToString());
                            //strHtml = strHtml.Replace("@childrenGrant", ds.Tables[0].Rows[0]["vChildEduGrant"].ToString());
                            //strHtml = strHtml.Replace("@marraigeexpense", ds.Tables[0].Rows[0]["vMarriageBenefitChild"].ToString());
                            //strHtml = strHtml.Replace("@sportsAcitivity", ds.Tables[0].Rows[0]["vSportsActivityCover"].ToString());
                            // strHtml = strHtml.Replace("@widowCover", ds.Tables[0].Rows[0]["vWidowhoodCover"].ToString());


                            //strHtml = strHtml.Replace("@ambulanceCharge", ds.Tables[0].Rows[0]["vAmbulanceCover"].ToString());
                            //strHtml = strHtml.Replace("@accidentalCash", ds.Tables[0].Rows[0]["vDailyCashBenefit"].ToString());
                            //strHtml = strHtml.Replace("@accidentalHospital", ds.Tables[0].Rows[0]["vAccidentalHospitalization"].ToString());
                            //strHtml = strHtml.Replace("@opdTreat", ds.Tables[0].Rows[0]["vOPDTreatment"].ToString());
                            //strHtml = strHtml.Replace("@accidentDental", ds.Tables[0].Rows[0]["vAccidentalDentalExpense"].ToString());

                            //strHtml = strHtml.Replace("@convalescence", ds.Tables[0].Rows[0]["vConvalescenceBenefit"].ToString());
                            //strHtml = strHtml.Replace("@burns", ds.Tables[0].Rows[0]["vBurns"].ToString());
                            //strHtml = strHtml.Replace("@brokenBones", ds.Tables[0].Rows[0]["vBrokenBones"].ToString());
                            //strHtml = strHtml.Replace("@coma", ds.Tables[0].Rows[0]["vComa"].ToString());
                            //strHtml = strHtml.Replace("@domesticTravel", ds.Tables[0].Rows[0]["vDomesticTravelForTreatment"].ToString());
                            //strHtml = strHtml.Replace("@lossOfEmploy", ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString());
                            //strHtml = strHtml.Replace("@onDutyCover", ds.Tables[0].Rows[0]["vOnDutyCover"].ToString());
                            //strHtml = strHtml.Replace("@legalExpenses", ds.Tables[0].Rows[0]["vLegalExpenses"].ToString());
                            #endregion

                            //         strHtml = strHtml.Replace("@premium", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@premium", Convert.ToDecimal(ds.Tables[0].Rows[0]["nNetPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            //strHtml = strHtml.Replace("@serviceTax", ds.Tables[0].Rows[0]["nServiceTax"].ToString());

                            strHtml = strHtml.Replace("@serviceTax", Convert.ToDecimal(ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            //strHtml = strHtml.Replace("@sbc", ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString());
                            //  strHtml = strHtml.Replace("@sbc", Convert.ToDecimal(ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            //strHtml = strHtml.Replace("@kkc", ds.Tables[0].Rows[0]["nKKC"].ToString());
                            //   strHtml = strHtml.Replace("@kkc", Convert.ToDecimal(ds.Tables[0].Rows[0]["nKKC"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@amount", Convert.ToDecimal(ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                            strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryNumber"].ToString());
                            strHtml = strHtml.Replace("@intermediaryLandline", "");
                            strHtml = strHtml.Replace("@challanDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dChallanDate"]).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            //strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["nStampDuty"].ToString());
                            strHtml = strHtml.Replace("@stampduty", Convert.ToDecimal(ds.Tables[0].Rows[0]["nStampDuty"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                            {
                                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash – Micro Insurance UIN:KOTHMGP076V011819");
                            }
                            else
                            {
                                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash UIN: KOTHLGP19014V011819");
                            }



                            //  if (!String.IsNullOrEmpty(accidentalDeath))
                            //   {
                            strHtml = strHtml.Replace("@accidentalDeathString", accidentalDeath == "" ? "" : accidentalDeath);
                            //  }

                            //   if (!String.IsNullOrEmpty(permTotalDisable))
                            //   {
                            strHtml = strHtml.Replace("@permTotalDisableString", permTotalDisable == "" ? "" : permTotalDisable);
                            //     }

                            //   if (!String.IsNullOrEmpty(permPartialDisable))
                            //   {
                            strHtml = strHtml.Replace("@permTotalPartialString", permPartialDisable == "" ? "" : permPartialDisable);
                            //   }

                            //    if (!String.IsNullOrEmpty(tempTotalDisable))
                            //    {
                            strHtml = strHtml.Replace("@tempTotalDisableString", tempTotalDisable == "" ? "" : tempTotalDisable);
                            //    }

                            //   if (!String.IsNullOrEmpty(carraigeBody))
                            //  {
                            strHtml = strHtml.Replace("@carraigeBodyString", carraigeBody == "" ? "" : carraigeBody);
                            // }
                            // if (!String.IsNullOrEmpty(funeralExpense))
                            // {
                            strHtml = strHtml.Replace("@funeralExpenseString", funeralExpense == "" ? "" : funeralExpense);
                            //  }
                            //  if (!String.IsNullOrEmpty(medicalExpense))
                            //  {
                            strHtml = strHtml.Replace("@medicalExpenseString", medicalExpense == "" ? "" : medicalExpense);
                            //   }
                            //   if (!String.IsNullOrEmpty(purchaseBlood))
                            // {
                            strHtml = strHtml.Replace("@purchasebloodString", purchaseBlood == "" ? "" : purchaseBlood);
                            //   }
                            //  if (!String.IsNullOrEmpty(transportation))
                            //  {
                            strHtml = strHtml.Replace("@transportationString", transportation == "" ? "" : transportation);
                            //  }
                            //  if (!String.IsNullOrEmpty(compassionate))
                            //  {
                            strHtml = strHtml.Replace("@compassionateString", compassionate == "" ? "" : compassionate);
                            //  }
                            //  if (!String.IsNullOrEmpty(disappearance))
                            //  {
                            strHtml = strHtml.Replace("@disappearanceString", disappearance == "" ? "" : disappearance);
                            //  }
                            //   if (!String.IsNullOrEmpty(modifyResidence))
                            // {
                            strHtml = strHtml.Replace("@modifyResidenceString", modifyResidence == "" ? "" : modifyResidence);
                            // }
                            //   if (!String.IsNullOrEmpty(costOfSupport))
                            //   {
                            strHtml = strHtml.Replace("@costOfSupportString", costOfSupport == "" ? "" : costOfSupport);
                            //  }
                            //  if (!String.IsNullOrEmpty(commonCarrier))
                            //  {
                            strHtml = strHtml.Replace("@commonCarrierString", commonCarrier == "" ? "" : commonCarrier);
                            //   }
                            //  if (!String.IsNullOrEmpty(childrenGrant))
                            //  {
                            strHtml = strHtml.Replace("@childrenGrantString", childrenGrant == "" ? "" : childrenGrant);
                            //   }
                            //  if (!String.IsNullOrEmpty(marraigeExpense))
                            //  {
                            strHtml = strHtml.Replace("@marraigeExpenseString", marraigeExpense == "" ? "" : marraigeExpense);
                            //  }
                            //  if (!String.IsNullOrEmpty(sportsActivity))
                            //  {
                            strHtml = strHtml.Replace("@sportsActivityString", sportsActivity == "" ? "" : sportsActivity);
                            //  }
                            //  if (!String.IsNullOrEmpty(widowHood))
                            // {
                            strHtml = strHtml.Replace("@widowHoodString", widowHood == "" ? "" : widowHood);
                            //  }
                            //  if (!String.IsNullOrEmpty(ambulanceChargesString))
                            //  {
                            strHtml = strHtml.Replace("@ambulanceChargesString", ambulanceChargesString == "" ? "" : ambulanceChargesString);
                            //  }
                            //  if (!String.IsNullOrEmpty(dailyCashString))
                            //  {
                            strHtml = strHtml.Replace("@dailyCashString", dailyCashString == "" ? "" : dailyCashString);
                            //  }
                            //   if (!String.IsNullOrEmpty(accidentalHospString))
                            // {
                            strHtml = strHtml.Replace("@accidentalHospString", accidentalHospString == "" ? "" : accidentalHospString);
                            // }
                            // if (!String.IsNullOrEmpty(convalString))
                            //  {
                            strHtml = strHtml.Replace("@convalString", convalString == "" ? "" : convalString);
                            //  }
                            // if (!String.IsNullOrEmpty(burnsString))
                            //  {
                            strHtml = strHtml.Replace("@burnsString", burnsString == "" ? "" : burnsString);
                            //  }
                            // if (!String.IsNullOrEmpty(brokenBones))
                            //  {
                            strHtml = strHtml.Replace("@brokenBones", brokenBones == "" ? "" : brokenBones);
                            //  }
                            //  if (!String.IsNullOrEmpty(comaString))
                            //  {
                            strHtml = strHtml.Replace("@comaString", comaString == "" ? "" : comaString);
                            // }
                            // if (!String.IsNullOrEmpty(domesticTravelString))
                            //  {
                            strHtml = strHtml.Replace("@domesticTravelString", domesticTravelString == "" ? "" : domesticTravelString);
                            // }
                            //   if (!String.IsNullOrEmpty(lossofEmployString))
                            //   {
                            strHtml = strHtml.Replace("@lossofEmployString", lossofEmployString == "" ? "" : lossofEmployString);
                            //  }
                            //if (!String.IsNullOrEmpty(onDutyCover))
                            //{
                            strHtml = strHtml.Replace("@onDutyCover", onDutyCover == "" ? "" : onDutyCover);
                            //}
                            // if (!String.IsNullOrEmpty(legalExpenses))
                            // {
                            strHtml = strHtml.Replace("@legalExpenses", legalExpenses == "" ? "" : legalExpenses);
                            // }
                            //    if (!String.IsNullOrEmpty(reducingCoverString))
                            //   {
                            strHtml = strHtml.Replace("@reducingCoverString", reducingCoverString == "" ? "" : reducingCoverString);
                            // }
                            //   if (!String.IsNullOrEmpty(assignmentString))
                            //  {
                            strHtml = strHtml.Replace("@assignmentString", assignmentString == "" ? "" : assignmentString);
                            // }

                            strHtml = strHtml.Replace("@accidentalDentalString", accidentalDentalString == "" ? "" : accidentalDentalString);
                            strHtml = strHtml.Replace("@opdString", opdString == "" ? "" : opdString);


                            #region HDCRISKFORPROTECT
                            string _Date1 = ds.Tables[0].Rows[0]["dAccountDebitDate"].ToString();
                            //DateTime dtDateHDCRisk = Convert.ToDateTime(_Date1);

                            //string TransactionDateHDCRisk = dtDateHDCRisk.ToString("dd/MM/yyyy");
                            strHtml = strHtml.Replace("@TransactionDateHDCRisk", _Date1);

                            string mentionedGender = ds.Tables[0].Rows[0]["vCustomerGender"].ToString();
                            if (string.IsNullOrEmpty(mentionedGender))
                            {
                                strHtml = strHtml.Replace("@salutation", "");
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "M" || ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "Male")
                                {
                                    strHtml = strHtml.Replace("@salutation", "Mr.");
                                }
                                else if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "F")
                                {
                                    strHtml = strHtml.Replace("@salutation", "Mrs.");
                                }
                                else
                                {
                                    strHtml = strHtml.Replace("@salutation", "");
                                }
                            }
                            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@addressofinsured1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@addressofinsured2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                            strHtml = strHtml.Replace("@addressofinsured3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                            strHtml = strHtml.Replace("@addressofinsuredCity", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                            strHtml = strHtml.Replace("@addressofinsuredState", ds.Tables[0].Rows[0]["vProposerState"].ToString());
                            strHtml = strHtml.Replace("@Pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                            strHtml = strHtml.Replace("@mobileno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());

                            strHtml = strHtml.Replace("@productname", ds.Tables[0].Rows[0]["vProductName"].ToString());
                            #endregion

                            strHtml = strHtml.Replace("@KotakGroupAccidentProtectUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]));
                            //#region HDC80DCERTIFICATEFORPROTECT
                            //string _Date = ds.Tables[0].Rows[0]["dAccountDebitDate"].ToString();
                            //DateTime dt = Convert.ToDateTime(_Date);

                            //string FDate = dt.ToString("dd/MM/yyyy");
                            //strHtml = strHtml.Replace("@ddateForRisk", FDate);
                            //strHtml = strHtml.Replace("@TotalPremium", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            //strHtml = strHtml.Replace("@paymentmode", ds.Tables[0].Rows[0]["vUniqueAccDebitRefNo"].ToString());
                            ////int policytnur = Convert.ToInt32(ds.Tables[0].Rows[0]["vPolicyTenure"].ToString());
                            ////double totalpremium = Convert.ToDouble(ds.Tables[0].Rows[0]["vTotalPremium"].ToString());


                            //string startdate = ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString();
                            //string enddate = ds.Tables[0].Rows[0]["dPolicyEndDate"].ToString();

                            //DateTime date = Convert.ToDateTime(startdate);
                            //string startdateyear1 = date.Year.ToString();
                            //int MonthofStartYear = date.Month;
                            //DateTime date1 = Convert.ToDateTime(enddate);
                            //string enddateyear2 = date1.Year.ToString();
                            //int shortenddateyear2 = Convert.ToInt32(enddateyear2.Substring(2)) - 1;
                            //string year5 = Convert.ToString(shortenddateyear2);
                            //string FYForLUMSUMyear4;
                            //if (MonthofStartYear > 3)
                            //{
                            //    FYForLUMSUMyear4 = Convert.ToInt32(startdateyear1) + "-" + (Convert.ToInt32(startdateyear1) + 1);
                            //}
                            //else
                            //{
                            //    FYForLUMSUMyear4 = (Convert.ToInt32(startdateyear1) - 1) + "-" + (startdateyear1.Substring(2));
                            //}
                            //strHtml = strHtml.Replace("@Year", FYForLUMSUMyear4);

                            //int YearDuration = Convert.ToInt32(enddateyear2) - Convert.ToInt32(startdateyear1);
                            //string totalpremiumamount = ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString();
                            //double totalpremiumamt = Convert.ToDouble(totalpremiumamount);
                            //double amount2 = totalpremiumamt / YearDuration;
                            ////string amount2 = Convert.ToString(amount1);
                            //double amount = Math.Round(amount2, 2);
                            //StringBuilder sb = new StringBuilder();
                            //sb.Append("<table style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                            //sb.Append("<tbody>");
                            //sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri'>");
                            //sb.Append("<td style='width:200;border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                            //sb.Append("<p style='margin-left: 20px;'><span>Financial Year</span></p>");
                            //sb.Append("</td>");
                            //sb.Append("<td style='width:650;border:1px solid black;border-collapse:collapse;'>");
                            //sb.Append("<p style='margin-left: 20px;'><span>Year wise proportionate premium allowed for Deduction under Section 80D</span></p>");
                            //sb.Append("</td>");
                            //sb.Append("</tr>");

                            //string FYForYearWiseLumsumDividendYear02;
                            //if (MonthofStartYear > 3)
                            //{
                            //    FYForYearWiseLumsumDividendYear02 = startdateyear1;
                            //}
                            //else
                            //{
                            //    int Yeart = Convert.ToInt32(startdateyear1) - 1;
                            //    FYForYearWiseLumsumDividendYear02 = Convert.ToString(Yeart);
                            //}
                            //for (int H = 0; H < YearDuration; H++)
                            //{
                            //    DataTable dt1 = new DataTable();
                            //    sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");

                            //    int Year00 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02) + H;
                            //    int sum = H + 1;
                            //    int Year01 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02.Substring(2)) + sum;

                            //    string year6 = Convert.ToString(Year00) + "-" + Convert.ToString(Year01);

                            //    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:200;'>");
                            //    sb.Append("<p style='margin-left: 20px;'> " + year6 + " </p>");
                            //    sb.Append("</td>");
                            //    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:650;'>");
                            //    sb.Append("<p style='margin-left: 20px;'> " + amount + " </p>");
                            //    sb.Append("</td>");
                            //    sb.Append("</tr>");
                            //}
                            //sb.Append("</tbody>");
                            //sb.Append("</table>");
                            //strHtml = strHtml.Replace("@testHTMLTABLE", sb.ToString());
                            //#endregion

                            //CR_P1_450_Start Kuwar Tax Invoice_GPA_Policy 
                            #region TaxInvoiceGPAPolicy

                            //GPA_GenerateGPAProtectPDF()
                            StringBuilder taxinvoice = new StringBuilder();
                            taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                            int temp = 0;
                            string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                            string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                            string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                            string totalPremium = ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString();
                            if (totalPremium.Contains('.'))
                            {
                                temp = Convert.ToInt32(totalPremium.Substring(0, totalPremium.IndexOf('.')));

                            }
                            else
                            {
                                temp = Convert.ToInt32(totalPremium);
                            }

                            string totalPremiumInWord = ConvertAmountInWord(temp);

                            // QR Code
                            string suppliGSTN = ConfigurationManager.AppSettings["GstRegNo"].ToString();
                            string kgiStateCode = suppliGSTN.Substring(0, 2); // getting kgi state code 
                            // string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                            string buyerGSTN = "";
                            //string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                            string transactionDate = ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString();
                            int noofHSNCode = 0;
                            // string hsnCode = "";
                            string hsnCode = ConfigurationManager.AppSettings["SacCode"].ToString();
                            string receiptNumber = ds.Tables[0].Rows[0]["vChallanNumber"].ToString();
                            if (hsnCode != "")
                            {
                                var tempcount = hsnCode.Split(' ').Length;
                                for (int i = 0; i < tempcount; i++)
                                {
                                    noofHSNCode++;
                                }

                            }
                            string Imagepath = string.Empty;
                            CreateQRCodeImage(certificateNo, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                            Imagepath = Imagepath == "error" ? "" : Imagepath;
                            string kgiStateName = string.Empty;
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT TOP 1 Txt_State FROM STATE_CITY_DISTRICT_PINCODE WHERE num_state_CD='" + kgiStateCode + "'";
                                cmd.Connection = con;
                                //sqlCon.Open();
                                object objStaeName = cmd.ExecuteScalar();
                                kgiStateName = Convert.ToString(objStaeName);
                                //kgiStateName = "MAHARASHTRA";
                            }
                            strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);


                            strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@gistinno", "");
                            strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                            strHtml = strHtml.Replace("@customername", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@emailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                            strHtml = strHtml.Replace("@contactno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@address", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@address1", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                            strHtml = strHtml.Replace("@address2", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());// add 3 address
                            strHtml = strHtml.Replace("@imdcode", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@receiptno", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            strHtml = strHtml.Replace("@customerstatecode", custStateCode);
                            //strHtml = strHtml.Replace("@customerstatecode", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst statecode of customer require
                            strHtml = strHtml.Replace("@supplyname", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst state name require of customer
                                                                                                                        //GPA Policy PROC_GET_COVER_SECTION_DATA_FOR_PDF_TEST proc

                            strHtml = strHtml.Replace("@KotakGstNo", ConfigurationManager.AppSettings["GstRegNo"].ToString());//not found
                            strHtml = strHtml.Replace("@name", kgiName);
                            strHtml = strHtml.Replace("@panNo", kgiPanno);
                            strHtml = strHtml.Replace("@cinNo", kgiCINno);

                            strHtml = strHtml.Replace("@invoicedate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@kgistatecode", kgiStateCode);//gst state code of kotak uncomment 
                            strHtml = strHtml.Replace("@kgistatename", kgiStateName.ToString());//gst state code of kotak uncommentuncomment
                            strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["vAdditional_column_4"].ToString());
                            strHtml = strHtml.Replace("@partnerappno", "");
                            strHtml = strHtml.Replace("@irn", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            //GPA Policy PROC_GET_COVER_SECTION_DATA_FOR_PDF_TEST proc

                            strHtml = strHtml.Replace("@totalpremium", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@netamount", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@totalgst", ds.Tables[0].Rows[0]["TotalGSTAmount"].ToString());

                            strHtml = strHtml.Replace("@cgstpercent", ds.Tables[0].Rows[0]["cgstPercentage"].ToString());
                            strHtml = strHtml.Replace("@ugstpercent", ds.Tables[0].Rows[0]["ugstPercentage"].ToString());
                            strHtml = strHtml.Replace("@sgstpercent", ds.Tables[0].Rows[0]["sgstPercentage"].ToString());
                            strHtml = strHtml.Replace("@igstpercent", ds.Tables[0].Rows[0]["igstPercentage"].ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@cgstamt", ds.Tables[0].Rows[0]["cgstAmount"].ToString());
                            strHtml = strHtml.Replace("@ugstamt", ds.Tables[0].Rows[0]["ugstAmount"].ToString());
                            strHtml = strHtml.Replace("@sgstamt", ds.Tables[0].Rows[0]["sgstAmount"].ToString());
                            strHtml = strHtml.Replace("@igstamt", ds.Tables[0].Rows[0]["igstAmount"].ToString());
                            strHtml = strHtml.Replace("@cessrate", "0");
                            strHtml = strHtml.Replace("@cessamt", ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString());

                            string tdservicetax = string.Empty;
                            string dataservicetax = string.Empty;
                            if (ds.Tables[0].Rows[0]["nServiceTax"].ToString() != "0" && ds.Tables[0].Rows[0]["nServiceTax"].ToString() != "")
                            {
                                tdservicetax = "<td style='border: 1px solid black' width='5%'><p style ='font-size:small'><strong>Service Tax</strong></p></td> ";
                                dataservicetax = "<td style ='border:1px solid black' width = '5%'><p> " + Convert.ToDecimal(ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td>";
                            }
                            strHtml = strHtml.Replace("@servictaxh", tdservicetax == "" ? "" : tdservicetax);
                            strHtml = strHtml.Replace("@servicetx", dataservicetax == "" ? "" : dataservicetax);
                            strHtml = strHtml.Replace("@totalgross", totalPremium);// change1
                            strHtml = strHtml.Replace("@totalinvoicevalueinfigure", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@totalinvoicevalueinwords", totalPremiumInWord.ToString());
                            #endregion
                            //CR_450_End_Kuwar_Tax_Invoice GPA Policy

                            // Get the current page HTML string by rendering into a TextWriter object
                            TextWriter outTextWriter = new StringWriter();
                            HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);

                            string currentPageHtmlString = strHtml;

                            //byte[] arrSign;
                            //byte[] arr;

                            //arr = convertToPdfNew(currentPageHtmlString, "");
                            //arrSign = Sign(arr);

                            // Create a HTML to PDF converter object with default settings
                            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                            // Set license key received after purchase to use the converter in licensed mode
                            // Leave it not set to use the converter in demo mode
                            string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();

                            htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

                            // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                            // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                            htmlToPdfConverter.ConversionDelay = 2;

                            // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                            htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                            // Add Header

                            // Enable header in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                            // Optionally add a space between header and the page body
                            // The spacing for first page and the subsequent pages can be set independently
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                            htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");

                            // Draw header elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                                DrawHeader(htmlToPdfConverter, false);

                            // Add Footer

                            // Enable footer in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                            // Optionally add a space between footer and the page body
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                            // Draw footer elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                                DrawFooter(htmlToPdfConverter, false, true);

                            // Use the current page URL as base URL
                            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;

                            // Convert the current page HTML string to a PDF document in a memory buffer
                            byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                            byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);

                            //string filePath = Server.MapPath("~/GPADownload");
                            //string fileName = "GPAPolicySchedule_" + certificateNo;

                            //String strfilename = filePath + "\\" + fileName;
                            //DownloadFile(strfilename);

                            Response.AddHeader("Content-Type", "application/pdf");


                            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNo));


                            Response.BinaryWrite(outPdfBuffer);

                            CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadGPAGetPolicy.aspx");//Added By Rajesh Soni on 20/02/2020
                            Response.End();




                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "GenerateGPAProtectPDF ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
            }
        }

        //CR_450_Start_Tax_Invoice Kuwar
        private void CreateQRCodeImage(string certificateno, string supplierGSTN, string buyerGSTN, string transactionDate, int noOfLines, string sacCode, string receiptno, out string strQRCodeImageSRC)
        {
            try
            {
                string documentType = "Tax Invoice";
                strQRCodeImageSRC = ConfigurationManager.AppSettings["QRCodeImageSRC"].ToString();
                string strQRCodeString = "GSTN of Supplier: " + supplierGSTN + Environment.NewLine +
                                        "GSTN of Buyer: " + buyerGSTN + Environment.NewLine +
                                        "Document Number: " + certificateno + Environment.NewLine +
                                        "Document Type : " + documentType + Environment.NewLine +
                                        "Date of Creation of Invoice : " + transactionDate + Environment.NewLine +
                                        "No of Lines: " + noOfLines + Environment.NewLine +
                                        "HSN code: " + sacCode + Environment.NewLine +
                                        "IRN :" + "" + Environment.NewLine +                              // blank will print
                                         "Premium Receipt Number: " + receiptno;
                // "GSTN of Supplier: " + supplierGSTN + " GSTN of Buyer:" + buyerGSTN + " Document Number :" + certificateno + " Document Type :" + documentType + " Date of Creation of Invoice: " + transactionDate + " No of Lines:" + noOfLines + " HSN code: " + sacCode + " IRN :" + certificateno +  ; 
                //string imagePath = Server.MapPath("~/KotakBundledPolicyQRCodeImgFiles/");
                string imagePath = Server.MapPath("~/KotakBundledPolicyQRCodeImgFiles/QRCode/");
                MessagingToolkit.QRCode.Codec.QRCodeEncoder qRCodeEncoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
                qRCodeEncoder.QRCodeVersion = 0;
                qRCodeEncoder.QRCodeEncodeMode = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
                qRCodeEncoder.QRCodeErrorCorrect = MessagingToolkit.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.Q;
                Bitmap bitmap = qRCodeEncoder.Encode(strQRCodeString);
                string qrImagePath = imagePath + " " + certificateno + ".png";
                strQRCodeImageSRC = strQRCodeImageSRC + " " + certificateno + ".png";
                //bitmap.Save(qrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Save(qrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                strQRCodeImageSRC = "error";
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "GenerateGPAProtectPDF  CreateQRCodeImage ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                // ExceptionUtility.LogException(ex, "CreateQRCodeImage");
            }
        }

        private string ConvertAmountInWord(int TotalPremium)
        {
            //  TotalPremiumInWord = "";

            string[] word = { "zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten" ,
                              "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifiteen", "Sixteen", "SevenTeen", "Eighteen", "Nineteen" };
            string[] numbers = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            string totalAmountInWord = string.Empty;
            //  int totalpremium = Convert.ToInt32(TotalPremium);
            try
            {
                if (TotalPremium == 0)
                {
                    return "";
                }
                else if (TotalPremium < 19)
                {
                    totalAmountInWord = word[TotalPremium];
                    return totalAmountInWord;
                }
                else if (TotalPremium < 100)
                {
                    totalAmountInWord = numbers[TotalPremium / 10 - 2] + ((TotalPremium % 10 > 0) ? "" + ConvertAmountInWord(TotalPremium % 10) : "");
                    return totalAmountInWord;

                }
                else if (TotalPremium < 1000)
                {
                    totalAmountInWord = word[TotalPremium / 100] + " Hundred " + ((TotalPremium % 100 > 0) ? "" + ConvertAmountInWord(TotalPremium % 100) : "");
                    return totalAmountInWord;

                }
                else if (TotalPremium < 100000)
                {
                    totalAmountInWord = ConvertAmountInWord(TotalPremium / 1000) + "   Thousand   " + ((TotalPremium % 1000 > 0) ? "" + ConvertAmountInWord(TotalPremium % 1000) : "");
                    return totalAmountInWord;

                }
                else if (TotalPremium < 100000)//less than lakh
                {
                    totalAmountInWord = ConvertAmountInWord(TotalPremium / 1000) + "   Thousand  " + ConvertAmountInWord(TotalPremium % 1000);
                    return totalAmountInWord;
                }
                else if (TotalPremium < 10000000)//upto ten lakh
                {
                    totalAmountInWord = ConvertAmountInWord(TotalPremium / 100000) + "  Lakh  " + ConvertAmountInWord(TotalPremium % 100000);
                    return totalAmountInWord;
                }
                else
                {
                    return totalAmountInWord = "conversion failed";
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "CreateQRCodeImage ::Error occured  : certificate number " + certificateNo
                    + "     " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);

                return totalAmountInWord = "Conversion Failed Please check log for More details";
            }
        }

        //CR_450_End Tax Invoice Kuwar

        // CR_705_GPA INSTA_POLICY_DOWNLOAD

        private void GenerateGPAProtectPDF_GIST(string certificate_No)
        {
            try
            {
                string Filepath = "";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                
                    SqlCommand command = new SqlCommand("PROC_GET_COVER_SECTION_DATA_FOR_PDF_TEST_GIST", con); // For Protect
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@vCertificateNo", certificate_No);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds);
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    if (ds.Tables.Count > 0)
                    {                       
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_CompleteLetter_With_GST.html";
                            string htmlBody = File.ReadAllText(strPath);
                            StringWriter sw = new StringWriter();
                            StringReader sr = new StringReader(sw.ToString());
                            string strHtml = htmlBody;

                            string accidentalDeath = string.Empty;
                            string permTotalDisable = string.Empty;
                            string permPartialDisable = string.Empty;
                            string tempTotalDisable = string.Empty;
                            string carraigeBody = string.Empty;
                            string funeralExpense = string.Empty;
                            string medicalExpense = string.Empty;
                            string purchaseBlood = string.Empty;
                            string transportation = string.Empty;
                            string compassionate = string.Empty;
                            string disappearance = string.Empty;
                            string modifyResidence = string.Empty;
                            string costOfSupport = string.Empty;
                            string commonCarrier = string.Empty;
                            string childrenGrant = string.Empty;
                            string marraigeExpense = string.Empty;
                            string sportsActivity = string.Empty;
                            string widowHood = string.Empty;

                            string ambulanceChargesString = string.Empty;
                            string dailyCashString = string.Empty;
                            string accidentalHospString = string.Empty;
                            string opdString = string.Empty;
                            string accidentalDentalString = string.Empty;
                            string convalString = string.Empty;
                            string burnsString = string.Empty;
                            string brokenBones = string.Empty;
                            string comaString = string.Empty;
                            string domesticTravelString = string.Empty;
                            string lossofEmployString = string.Empty;
                            string onDutyCover = string.Empty;
                            string legalExpenses = string.Empty;

                            string reducingCoverString = string.Empty;
                            string assignmentString = string.Empty;

                            //gst
                            string custStateCode = string.Empty;
                            string igstString = string.Empty;
                            string cgstsgstString = string.Empty;
                            string cgstugstString = string.Empty;
                            using (SqlCommand cmd = new SqlCommand())
                            {

                                cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "'";
                                cmd.Connection = con;
                                if (con.State == ConnectionState.Closed)
                                {
                                    con.Open();
                                }
                                object objCustState = cmd.ExecuteScalar();
                                custStateCode = Convert.ToString(objCustState);
                                if (con.State == ConnectionState.Open)
                                {
                                    con.Close();
                                }
                            }

                            strHtml = strHtml.Replace("@createdDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dCreatedDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@customerName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeAge"].ToString());
                            strHtml = strHtml.Replace("@masterDate", Convert.ToDateTime(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["vMasterPolicyDate"].ToString()) ? DateTime.Now.ToString() : ds.Tables[0].Rows[0]["vMasterPolicyDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());


                            strHtml = strHtml.Replace("@addressline1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@addressline2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());

                            strHtml = strHtml.Replace("@PlaceofSupply", ds.Tables[0].Rows[0]["vProposerState"].ToString());
                            strHtml = strHtml.Replace("@SupplyStateCode", custStateCode);


                            strHtml = strHtml.Replace("@addressline3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                            strHtml = strHtml.Replace("@city", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                            strHtml = strHtml.Replace("@pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());
                            strHtml = strHtml.Replace("@state", ds.Tables[0].Rows[0]["vProposerState"].ToString());

                            strHtml = strHtml.Replace("@mobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@emailID", ds.Tables[0].Rows[0]["vEmailId"].ToString());

                            strHtml = strHtml.Replace("@productName", "KOTAK GROUP ACCIDENT PROTECT"); //done changes for cert no
                            strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vBusinesssType"].ToString() == "" ? "New" : ds.Tables[0].Rows[0]["vBusinesssType"].ToString());

                            strHtml = strHtml.Replace("@prevPolicyNo", ds.Tables[0].Rows[0]["vprevPolicyNumber"].ToString());
                            strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyLoc"].ToString());


                            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderAddress", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString()); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@PolicyholderLine2Address", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerCity"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerState"].ToString() + "(" + custStateCode + ")");

                            strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderEmailAddress", ds.Tables[0].Rows[0]["vEmailId"].ToString());

                            strHtml = strHtml.Replace("@policyStartDate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@policyEndDate", ds.Tables[0].Rows[0]["dPolicyEndDate"].ToString());

                            strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                            strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());


                            strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                            strHtml = strHtml.Replace("@occupation", ds.Tables[0].Rows[0]["vOccupation"].ToString());

                            strHtml = strHtml.Replace("@relationInsured", "Self");//ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                            strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["dCustomerDob"].ToString());
                            strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                            strHtml = strHtml.Replace("@category", "");
                            strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                            strHtml = strHtml.Replace("@sumInsured", "");
                            strHtml = strHtml.Replace("@productname", ds.Tables[0].Rows[0]["vProductName"].ToString());

                            strHtml = strHtml.Replace("@sumBasis", ds.Tables[0].Rows[0]["vSIBasis"].ToString());

                            strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                            strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());

                            strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                            strHtml = strHtml.Replace("@appointee", (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAppointeeName"].ToString()) ? (ds.Tables[0].Rows[0]["vAppointeeName"].ToString() + " / " + ds.Tables[0].Rows[0]["vAppointeeRelation"].ToString()) : ""));//ds.Tables[0].Rows[0]["vAppointeeName"].ToString() +" / " + ds.Tables[0].Rows[0]["vAppointeeRelation"].ToString() + " / " + ds.Tables[0].Rows[0]["vAppointeeDOB"].ToString());

                            strHtml = strHtml.Replace("@PolicyIssuingOffice", ds.Tables[0].Rows[0]["vPolicyIssuedAt"].ToString());
                            strHtml = strHtml.Replace("@GSTIN", ds.Tables[0].Rows[0]["vAdditional_column_3"].ToString());

                            string igstPercentage = ds.Tables[0].Rows[0]["igstPercentage"].ToString();
                            string cgstPercentage = ds.Tables[0].Rows[0]["cgstPercentage"].ToString();
                            string sgstPercentage = ds.Tables[0].Rows[0]["sgstPercentage"].ToString();
                            string ugstPercentage = ds.Tables[0].Rows[0]["ugstPercentage"].ToString();
                            string igstAmount = ds.Tables[0].Rows[0]["igstAmount"].ToString();
                            string cgstAmount = ds.Tables[0].Rows[0]["cgstAmount"].ToString();
                            string sgstAmount = ds.Tables[0].Rows[0]["sgstAmount"].ToString();
                            string ugstAmount = ds.Tables[0].Rows[0]["ugstAmount"].ToString();

                            if (igstPercentage != "0")
                            {
                                igstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody style='font - size:small'><tr><td style='border:1px solid black' width='20%'><p style='text-align:center; font-size:small'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center; font-size:small'>IGST@" + igstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center; font-size:small'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center; font-size:small'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center; font-size:small'>" + igstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center; font-size:small'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && sgstPercentage != "0")
                            {
                                cgstsgstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody style='font - size:small'><tr><td style='border:1px solid black' width='20%'><p style='text-align:center; font-size:small'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center; font-size:small'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center; font-size:small'>SGST@" + sgstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center; font-size:small'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center; font-size:small'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center; font-size:small'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center; font-size:small'>" + sgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center; font-size:small'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && ugstPercentage != "0")
                            {
                                cgstugstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody style='font - size:small'><tr><td style='border:1px solid black' width='20%'><p style='text-align:center; font-size:small'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center; font-size:small'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center; font-size:small'>UGST@" + ugstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center; font-size:small'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center; font-size:small'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center; font-size:small'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center; font-size:small'>" + ugstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center; font-size:small'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            string _Date1 = DateTime.ParseExact(ds.Tables[0].Rows[0]["dAccountDebitDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                            // DateTime dtDateHDCRisk = Convert.ToDateTime(_Date1);

                            string TransactionDateHDCRisk = _Date1;
                            strHtml = strHtml.Replace("@TransactionDateHDCRisk", TransactionDateHDCRisk);

                            string mentionedGender = ds.Tables[0].Rows[0]["vCustomerGender"].ToString();
                            if (string.IsNullOrEmpty(mentionedGender))
                            {
                                strHtml = strHtml.Replace("@salutation", "");
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "M" || ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "Male")
                                {
                                    strHtml = strHtml.Replace("@salutation", "Mr.");
                                }
                                else if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "F")
                                {
                                    strHtml = strHtml.Replace("@salutation", "Mrs.");
                                }
                                else
                                {
                                    strHtml = strHtml.Replace("@salutation", "");
                                }
                            }

                            strHtml = strHtml.Replace("@igstString", igstString == "" ? "" : igstString);
                            strHtml = strHtml.Replace("@cgstsgstString", cgstsgstString == "" ? "" : cgstsgstString);
                            strHtml = strHtml.Replace("@cgstugstString", cgstugstString == "" ? "" : cgstugstString);

                            string policyIssuance = ds.Tables[0].Rows[0]["dCreatedDate"].ToString();
                            string customString = string.Empty;

                            //if (!String.IsNullOrEmpty(policyIssuance))
                            //{
                            //    string[] strArr = policyIssuance.Split(' ');
                            //    if (String.IsNullOrEmpty(strArr[1]))
                            //    {
                            //        customString = "this " + strArr[2] + " day of " + strArr[0] + " of year " + strArr[3];
                            //    }
                            //    else
                            //    {
                            //        customString = "this " + strArr[1] + " day of " + strArr[0] + " of year " + strArr[2];
                            //    }

                            //}

                            string vDay = Convert.ToDateTime(ds.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("dd");
                            string vMonth = Convert.ToDateTime(ds.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("MMMMMMMMM");
                            string vYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["dCreatedDate"].ToString()).ToString("yyyy");

                            customString = "this " + vDay + " of " + vMonth + " of " + vYear + ".";




                            strHtml = strHtml.Replace("@polIssueString", customString);



                            if ((ds.Tables[0].Rows[0]["vAccidentalDeathAD"].ToString() == "Y") || (ds.Tables[0].Rows[0]["vAccidentalDeathAD"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString().Trim()))
                                {
                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if ((ds.Tables[0].Rows[0]["vPermTotalDisablePTD"].ToString() == "Y") || (ds.Tables[0].Rows[0]["vPermTotalDisablePTD"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString().Trim()))
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString() + ")</p></td></tr> ";
                                }
                                else
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString())) + "</p></td></tr> ";
                                }
                            }
                            //
                            if ((ds.Tables[0].Rows[0]["vPermPartialDisablePTD"].ToString() == "Y") || (ds.Tables[0].Rows[0]["vPermPartialDisablePTD"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString().Trim()))
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if ((ds.Tables[0].Rows[0]["vTempTotalDisableTTD"].ToString() == "Y") || (ds.Tables[0].Rows[0]["vTempTotalDisableTTD"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString().Trim()))
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString())) + "</p></td></tr>";
                                }
                            }



                            if ((ds.Tables[0].Rows[0]["vCarraigeDeadBody"].ToString() == "Y") || (ds.Tables[0].Rows[0]["vCarraigeDeadBody"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString().Trim()))
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if ((ds.Tables[0].Rows[0]["vFuneralExpenses"].ToString() == "Y") || (ds.Tables[0].Rows[0]["vFuneralExpenses"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString().Trim()))
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalMedicalExp"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vAccidentalMedicalExp"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString().Trim()))
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vPurchaseofBlood"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vPurchaseofBlood"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPurchaseofBloodSIText"].ToString().Trim()))
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vPurchaseofBloodSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vTransportationofImpMedicine"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vTransportationofImpMedicine"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTransportationofImpMedicineSIText"].ToString().Trim()))
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vTransportationofImpMedicineSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCompassionateVisit"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vCompassionateVisit"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString().Trim()))
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDisappearanceBenefit"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vDisappearanceBenefit"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString().Trim()))
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString() + "</p></td></tr>";
                                }

                            }

                            if (ds.Tables[0].Rows[0]["vModificationofVehicle"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vModificationofVehicle"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString().Trim()))
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCostSupportItems"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vCostSupportItems"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString().Trim()))
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCommonCarrier"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vCommonCarrier"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString().Trim()))
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vChildEduGrant"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vChildEduGrant"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString().Trim()))
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vMarraigeExpenses"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vMarraigeExpenses"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString().Trim()))
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSportsActivityCover"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vSportsActivityCover"].ToString() == "1"))
                            {
                                sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vWidowhoodCover"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vWidowhoodCover"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString().Trim()))
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAmbulanceCover"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vAmbulanceCover"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString().Trim()))
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString())) + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vDailyCashBenefit"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vDailyCashBenefit"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString().Trim()))
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalHospitalization"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vAccidentalHospitalization"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString().Trim()))
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOPDTreatment"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vOPDTreatment"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString().Trim()))
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalDentalExpense"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vAccidentalDentalExpense"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString().Trim()))
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vConvalescenceBenefit"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vConvalescenceBenefit"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString().Trim()))
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBurnBenefit"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vBurnBenefit"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString().Trim()))
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBrokenBones"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vBrokenBones"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString().Trim()))
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vComa"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vComa"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vComaSIText"].ToString().Trim()))
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nComaSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vComaSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nComaSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDomesticTravelForTreatment"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vDomesticTravelForTreatment"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString().Trim()))
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString().Trim()))
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOnDutyCover"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vOnDutyCover"].ToString() == "1"))
                            {
                                onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vLegalExpenses"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vLegalExpenses"].ToString() == "1"))
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString().Trim()))
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString())) + "&nbsp;(" + ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString())) + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSIBasis"].ToString().ToLower() == "reducing")
                            {
                                reducingCoverString = "<tr><td style='border:1px solid black' width='10%'><p  style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>8.23-Reducing Sum Insured Covers</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vProposalType"].ToString().ToLower() == "credit linked")
                            {
                                assignmentString = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>8.22-Assignment</p></td></tr>";
                            }

                            strHtml = strHtml.Replace("@premium", Convert.ToDecimal(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["nNetPremium"].ToString()) ? "0" : ds.Tables[0].Rows[0]["nNetPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            strHtml = strHtml.Replace("@serviceTax", Convert.ToDecimal(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["nServiceTax"].ToString()) ? "0" : ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@amount", Convert.ToDecimal(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString()) ? "0" : ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                            strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryNumber"].ToString());
                            strHtml = strHtml.Replace("@intermediaryLandline", "");
                            strHtml = strHtml.Replace("@challanDate", Convert.ToDateTime(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["dChallanDate"].ToString()) ? DateTime.Now.ToString() : ds.Tables[0].Rows[0]["dChallanDate"]).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            strHtml = strHtml.Replace("@stampduty", Convert.ToDecimal(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["nStampDuty"].ToString()) ? "0" : ds.Tables[0].Rows[0]["nStampDuty"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            strHtml = strHtml.Replace("@accidentalDeathString", accidentalDeath == "" ? "" : accidentalDeath);
                            strHtml = strHtml.Replace("@permTotalDisableString", permTotalDisable == "" ? "" : permTotalDisable);
                            strHtml = strHtml.Replace("@permTotalPartialString", permPartialDisable == "" ? "" : permPartialDisable);
                            strHtml = strHtml.Replace("@tempTotalDisableString", tempTotalDisable == "" ? "" : tempTotalDisable);
                            strHtml = strHtml.Replace("@carraigeBodyString", carraigeBody == "" ? "" : carraigeBody);
                            strHtml = strHtml.Replace("@funeralExpenseString", funeralExpense == "" ? "" : funeralExpense);
                            strHtml = strHtml.Replace("@medicalExpenseString", medicalExpense == "" ? "" : medicalExpense);
                            strHtml = strHtml.Replace("@purchasebloodString", purchaseBlood == "" ? "" : purchaseBlood);
                            strHtml = strHtml.Replace("@transportationString", transportation == "" ? "" : transportation);
                            strHtml = strHtml.Replace("@compassionateString", compassionate == "" ? "" : compassionate);
                            strHtml = strHtml.Replace("@disappearanceString", disappearance == "" ? "" : disappearance);
                            strHtml = strHtml.Replace("@modifyResidenceString", modifyResidence == "" ? "" : modifyResidence);
                            strHtml = strHtml.Replace("@costOfSupportString", costOfSupport == "" ? "" : costOfSupport);
                            strHtml = strHtml.Replace("@commonCarrierString", commonCarrier == "" ? "" : commonCarrier);
                            strHtml = strHtml.Replace("@childrenGrantString", childrenGrant == "" ? "" : childrenGrant);
                            strHtml = strHtml.Replace("@marraigeExpenseString", marraigeExpense == "" ? "" : marraigeExpense);
                            strHtml = strHtml.Replace("@sportsActivityString", sportsActivity == "" ? "" : sportsActivity);
                            strHtml = strHtml.Replace("@widowHoodString", widowHood == "" ? "" : widowHood);
                            strHtml = strHtml.Replace("@ambulanceChargesString", ambulanceChargesString == "" ? "" : ambulanceChargesString);
                            strHtml = strHtml.Replace("@dailyCashString", dailyCashString == "" ? "" : dailyCashString);
                            strHtml = strHtml.Replace("@accidentalHospString", accidentalHospString == "" ? "" : accidentalHospString);
                            strHtml = strHtml.Replace("@convalString", convalString == "" ? "" : convalString);
                            strHtml = strHtml.Replace("@burnsString", burnsString == "" ? "" : burnsString);
                            strHtml = strHtml.Replace("@brokenBones", brokenBones == "" ? "" : brokenBones);
                            strHtml = strHtml.Replace("@comaString", comaString == "" ? "" : comaString);
                            strHtml = strHtml.Replace("@domesticTravelString", domesticTravelString == "" ? "" : domesticTravelString);
                            strHtml = strHtml.Replace("@lossofEmployString", lossofEmployString == "" ? "" : lossofEmployString);
                            strHtml = strHtml.Replace("@onDutyCover", onDutyCover == "" ? "" : onDutyCover);
                            strHtml = strHtml.Replace("@legalExpenses", legalExpenses == "" ? "" : legalExpenses);
                            strHtml = strHtml.Replace("@reducingCoverString", reducingCoverString == "" ? "" : reducingCoverString);
                            strHtml = strHtml.Replace("@assignmentString", assignmentString == "" ? "" : assignmentString);

                            strHtml = strHtml.Replace("@accidentalDentalString", accidentalDentalString == "" ? "" : accidentalDentalString);
                            strHtml = strHtml.Replace("@opdString", opdString == "" ? "" : opdString);


                            if (ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString() == "Y" || (ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString() == "1"))
                            {
                                strHtml = strHtml.Replace("@LossOFJob", "(*) For Salaried Persons and Credit linked policies only");

                            }
                            else
                            {
                                strHtml = strHtml.Replace("@LossOFJob", "");
                            }

                            strHtml = strHtml.Replace("@KotakGroupAccidentProtectUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]));

                            //CR_P1_450_Start Kuwar Tax Invoice_GPA_Policy 
                            #region TaxInvoiceGPAPolicy

                            //GPA_GenerateGPAProtectPDF()
                            StringBuilder taxinvoice = new StringBuilder();
                            taxinvoice.Append("<div id='taxInvoice' style='font - family:Calibri; display: inline'>");
                            int temp = 0;
                            string kgiPanno = ConfigurationManager.AppSettings["KGIPanNo"].ToString();
                            string kgiCINno = ConfigurationManager.AppSettings["CIN"].ToString();
                            string kgiName = ConfigurationManager.AppSettings["lblCompanyName"].ToString();
                            string totalpremium = ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString();
                            if (totalpremium.Contains('.'))
                            {
                                temp = Convert.ToInt32(totalpremium.Substring(0, totalpremium.IndexOf('.')));

                            }
                            else
                            {
                                temp = Convert.ToInt32(totalpremium);
                            }

                            string totalPremiumInWord = ConvertAmountInWord(temp);

                            // QR Code
                            string suppliGSTN = ConfigurationManager.AppSettings["GstRegNo"].ToString();
                            // string suppliGSTN = ds.Tables[0].Rows[0]["vKGIGSTN"].ToString();
                            string buyerGSTN = "";
                            //string buyerGSTN = ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString();
                            string transactionDate = ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString();
                            int noofHSNCode = 0;
                            // string hsnCode = "";
                            string hsnCode = ConfigurationManager.AppSettings["SacCode"].ToString();
                            string receiptNumber = ds.Tables[0].Rows[0]["vChallanNumber"].ToString();
                            if (hsnCode != "")
                            {
                                var tempcount = hsnCode.Split(' ').Length;
                                for (int i = 0; i < tempcount; i++)
                                {
                                    noofHSNCode++;
                                }

                            }
                            string Imagepath = string.Empty;
                            CreateQRCodeImage(certificate_No, suppliGSTN, buyerGSTN, transactionDate, noofHSNCode, hsnCode, receiptNumber, out Imagepath);
                            Imagepath = Imagepath == "error" ? "" : Imagepath;
                            strHtml = strHtml.Replace("@divQRImagehtml", Imagepath);

                            strHtml = strHtml.Replace("@divhtml", taxinvoice.ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@gistinno", "");
                            strHtml = strHtml.Replace("@GSTcustomerId", "");//not there this column
                            strHtml = strHtml.Replace("@customername", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@emailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                            strHtml = strHtml.Replace("@contactno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@address", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@address1", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                            strHtml = strHtml.Replace("@address2", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());// add 3 address
                            strHtml = strHtml.Replace("@imdcode", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@receiptno", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            strHtml = strHtml.Replace("@customerstatecode", custStateCode);

                            strHtml = strHtml.Replace("@supplyname", ds.Tables[0].Rows[0]["vProposerState"].ToString());//gst state name require of customer

                            strHtml = strHtml.Replace("@name", kgiName);
                            strHtml = strHtml.Replace("@panNo", kgiPanno);
                            strHtml = strHtml.Replace("@cinNo", kgiCINno);

                            strHtml = strHtml.Replace("@invoicedate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@proposalno", ds.Tables[0].Rows[0]["vAdditional_column_4"].ToString());
                            strHtml = strHtml.Replace("@partnerappno", "");// this column is there as per jay

                            strHtml = strHtml.Replace("@irn", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@totalpremium", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@netamount", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@totalgst", ds.Tables[0].Rows[0]["TotalGSTAmount"].ToString());

                            strHtml = strHtml.Replace("@cgstpercent", ds.Tables[0].Rows[0]["cgstPercentage"].ToString());
                            strHtml = strHtml.Replace("@ugstpercent", ds.Tables[0].Rows[0]["ugstPercentage"].ToString());
                            strHtml = strHtml.Replace("@sgstpercent", ds.Tables[0].Rows[0]["sgstPercentage"].ToString());
                            strHtml = strHtml.Replace("@igstpercent", ds.Tables[0].Rows[0]["igstPercentage"].ToString());
                            //GPA Policy
                            strHtml = strHtml.Replace("@cgstamt", ds.Tables[0].Rows[0]["cgstAmount"].ToString());
                            strHtml = strHtml.Replace("@ugstamt", ds.Tables[0].Rows[0]["ugstAmount"].ToString());
                            strHtml = strHtml.Replace("@sgstamt", ds.Tables[0].Rows[0]["sgstAmount"].ToString());
                            strHtml = strHtml.Replace("@igstamt", ds.Tables[0].Rows[0]["igstAmount"].ToString());

                            strHtml = strHtml.Replace("@cessrate", "0");
                            strHtml = strHtml.Replace("@cessamt", ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString());

                            string tdservicetax = string.Empty;
                            string dataservicetax = string.Empty;
                            string tempserviceTax = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["nServiceTax"].ToString()) ? "0" : ds.Tables[0].Rows[0]["nServiceTax"].ToString();
                            if (tempserviceTax != "0" && !string.IsNullOrEmpty(tempserviceTax))
                            {
                                tdservicetax = "<td style='border: 1px solid black' width='5%'><p style ='font-size:small'><strong>Service Tax</strong></p></td> ";
                                dataservicetax = "<td style ='border:1px solid black' width = '5%'><p> " + Convert.ToDecimal(ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td>";
                            }
                            strHtml = strHtml.Replace("@servictaxh", tdservicetax == "" ? "" : tdservicetax);
                            strHtml = strHtml.Replace("@servicetx", dataservicetax == "" ? "" : dataservicetax);

                            strHtml = strHtml.Replace("@totalinvoicevalueinfigure", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@totalinvoicevalueinwords", totalPremiumInWord.ToString());
                            #endregion
                            //CR_450_End_Kuwar_Tax_Invoice GPA Policy


                            // Get the current page HTML string by rendering into a TextWriter object
                            TextWriter outTextWriter = new StringWriter();
                            HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);

                            string currentPageHtmlString = strHtml;

                            //byte[] arrSign;
                            //byte[] arr;

                            //arr = convertToPdfNew(currentPageHtmlString, "");
                            //arrSign = Sign(arr);

                            // Create a HTML to PDF converter object with default settings
                            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                            // Set license key received after purchase to use the converter in licensed mode
                            // Leave it not set to use the converter in demo mode
                            string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();

                            htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

                            // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                            // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                            htmlToPdfConverter.ConversionDelay = 2;

                            // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                            htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                            // Add Header

                            // Enable header in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                            // Optionally add a space between header and the page body
                            // The spacing for first page and the subsequent pages can be set independently
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                            htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");

                            // Draw header elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                                DrawHeader(htmlToPdfConverter, false);

                            // Add Footer

                            // Enable footer in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                            // Optionally add a space between footer and the page body
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                            // Draw footer elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                                DrawFooter(htmlToPdfConverter, false, true);

                            // Use the current page URL as base URL
                            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;

                            // Convert the current page HTML string to a PDF document in a memory buffer
                            byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                            byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);

                            Response.AddHeader("Content-Type", "application/pdf");

                            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNo));

                            Response.BinaryWrite(outPdfBuffer);

                            CommonExtensions.fn_AddLogForDownload(certificateNo, "FrmDownloadGPAGetPolicy.aspx");//Added By Rajesh Soni on 20/02/2020
                            Response.End();

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "GenerateGPAProtectPDF_GIST ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
            }

        }

        // start Added to show UIN No on footer for GHI Policy
        private  void DrawFooterGHI(HtmlToPdfConverter htmlToPdfConverter, bool addPageNumbers, bool drawFooterLine)
        {
            string footerHtmlUrl = IsWithoutHeaderFooter  ? Server.MapPath("~/Header_HTML_WithoutFooter.html") : Server.MapPath("~/Footer_GHI_HTML.html") ; // IsWithoutHeaderFooter ? Properties.Settings.Default.html_withoutfooter : Properties.Settings.Default.html_withfooter;
                                                      // Set the footer height in points
            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 60;

            // Set footer background color
            //htmlToPdfConverter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.White;
            htmlToPdfConverter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.LightGray;


            // Create a HTML element to be added in footer
            HtmlToPdfElement footerHtml = new HtmlToPdfElement(footerHtmlUrl);

            // Set the HTML element to fit the container height
            footerHtml.FitHeight = true;

            // Add HTML element to footer
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerHtml);

            // Add page numbering
            if (addPageNumbers)
            {
                // Create a text element with page numbering place holders &p; and & P;
                TextElement footerText = new TextElement(0, 30, "Page &p; of &P;  ",
                    new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));

                // Align the text at the right of the footer
                footerText.TextAlign = HorizontalTextAlign.Right;

                // Set page numbering text color
                footerText.ForeColor = System.Drawing.Color.Navy;

                // Embed the text element font in PDF
                footerText.EmbedSysFont = true;

                // Add the text element to footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerText);
            }

            drawFooterLine = IsWithoutHeaderFooter ? false : true;
            if (drawFooterLine)
            {
                // Calculate the footer width based on PDF page size and margins
                float footerWidth = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width -
                            htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin;

                // Create a line element for the top of the footer
                LineElement footerLine = new LineElement(0, 0, footerWidth, 0);

                // Set line color
                footerLine.ForeColor = System.Drawing.Color.Gray;

                // Add line element to the bottom of the footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerLine);
            }
        }

        // End Added to show UIN No on footer for GHI Policy
    }
}