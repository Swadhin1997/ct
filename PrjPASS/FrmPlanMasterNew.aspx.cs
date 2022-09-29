using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Obout.ComboBox;

namespace ProjectPASS
{
    public partial class FrmPlanMasterNew : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

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
                }
                else
                {
                    Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    return;
                }
                FillProductData();
               // FillDrpCovers();
                txtStampDuty.Text = "0.00";
                txtSecAPrem.Text = "0.00";
                txtExtToSecAPrem.Text = "0.00";
                txtSecBPrem.Text = "0.00";
                txtPlanSI.Text = "0.00";
            }
            FillDrpCovers();

        }

        private void FillProductData()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand = "SELECT * FROM TBL_PRODUCT_MASTER";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet dsCover = null;
            dsCover = db.ExecuteDataSet(dbCommand);
            if (dsCover.Tables[0].Rows.Count > 0)
            {
                cmbProduct.DataValueField = "Product_Code";
                cmbProduct.DataTextField = "Product_Code";
                cmbProduct.DataSource = dsCover.Tables[0];
                cmbProduct.DataBind();
                ComboBoxItem l_lstItem = new ComboBoxItem("Select", "-1");                    
                //ddlProduct.Items.Insert(0, l_lstItem);
                cmbProduct.Items.Insert(0, l_lstItem);
            }
            else
            {
                Alert.Show("No Schema Defined in Master");
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtPlanDesc.Text == "")
            {
              // FillDrpCovers();
                Alert.Show("Please Enter Plan Description");
                return;
            }
            if (txtStampDuty.Text == "")
            {
              //  FillDrpCovers();
                Alert.Show("Please Enter Valid Stamp Duty Value");
                return;
            }
            if (Convert.ToDouble(txtStampDuty.Text) == 0)
            {
              // FillDrpCovers();
                Alert.Show("Please Enter Valid Stamp Duty Value");
                return;
            }
            if (txtSecAPrem.Text == "")
            {
              // FillDrpCovers();
                Alert.Show("Please Enter Valid Section A Premium Value");
                return;
            }
            if (txtExtToSecAPrem.Text == "")
            {
               // FillDrpCovers();
                Alert.Show("Please Enter Valid Extension To Section A Premium Value");
                return;
            }
            if (txtSecBPrem.Text == "")
            {
               // FillDrpCovers();
                Alert.Show("Please Enter Valid Section B Premium Value");
                return;
            }
            if (txtPlanSI.Text == "")
            {
              //  FillDrpCovers();
                Alert.Show("Please Enter Valid Plan SI Value");
                return;
            }
            if (Convert.ToDouble(txtPlanSI.Text) == 0)
            {
               // FillDrpCovers();
                Alert.Show("Please Enter Valid Plan SI Value");
                return;
            }

            if (String.IsNullOrEmpty(cmbProduct.SelectedText))
            {
              //  FillDrpCovers();
                Alert.Show("Please Select Product Code");
                return;
            }

            //if (String.IsNullOrEmpty(cmb1.SelectedText))
            //{
            //    FillDrpCovers();
            //    Alert.Show("Please Select Proposal Type");
            //    return;
            //}

            //if (String.IsNullOrEmpty(cmbSIBasis.SelectedText))
            //{
            //    FillDrpCovers();
            //    Alert.Show("Please Select SI Basis");
            //    return;
            //}

            //if (String.IsNullOrEmpty(txtFinancer.Text))
            //{
            //    FillDrpCovers();
            //    Alert.Show("Please Enter Financer Name");
            //    return;
            //}

            //if (String.IsNullOrEmpty(txtMasterPolicy.Text))
            //{
            //    FillDrpCovers();
            //    Alert.Show("Please Enter Master Policy No");
            //    return;
            //}

            //if (String.IsNullOrEmpty(txtMsterPolicyLoc.Text))
            //{
            //    FillDrpCovers();
            //    Alert.Show("Please Enter Master Policy Location");
            //    return;
            //}

            if (String.IsNullOrEmpty(txtMinTenure.Text))
            {
                //FillDrpCovers();
                Alert.Show("Please Enter Min Policy Tenure");
                return;
            }

            if (String.IsNullOrEmpty(txtMaxTenure.Text))
            {
                //FillDrpCovers();
                Alert.Show("Please Enter Max Policy Tenure");
                return;
            }

            //if (String.IsNullOrEmpty(txtLoanType.Text))
            //{
            //    FillDrpCovers();
            //    Alert.Show("Please Enter Loan Type");
            //    return;
            //}

            //if (Calendar1.SelectedDate.ToShortDateString() == "1/1/0001")
            //{
            //    FillDrpCovers();
            //    Alert.Show("Please Enter Master Policy Issue Date");
            //    return;
            //}

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
            string vPLANCode = "";
            SqlConnection _con = new SqlConnection(db.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();
            try
            {
                // extract the rows to insert/update from the hidden field    
                string excelData = Grid1ExcelData.Value;

                // extract the ids of the rows to delete from the hidden field
                string excelDeletedIds = Grid1ExcelDeletedIds.Value;

                string[] rowSeparator = new string[] { "|*row*|" };
                string[] cellSeparator = new string[] { "|*cell*|" };

                string[] dataRows = excelData.Split(rowSeparator, StringSplitOptions.None);

                vPLANCode = wsDocNo.fn_Gen_Doc_Master_No("PLAN", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
                string vPlanDesc = txtPlanDesc.Text;
                double nStampDuty = Convert.ToDouble(txtStampDuty.Text);
                double nSecAPremium = Convert.ToDouble(txtSecAPrem.Text);
                double nExtToSecAPremium = Convert.ToDouble(txtExtToSecAPrem.Text);
                double nSecBPremium = Convert.ToDouble(txtSecBPrem.Text);
                double nPlanSI = Convert.ToDouble(txtPlanSI.Text);

                string productCode = cmbProduct.SelectedText.ToString();
                string productName = lblProdName.Text;
                string proposalType = cmb1.SelectedText.ToString();
                string siBasis = cmbSIBasis.SelectedText.ToString();
                string financerName = txtFinancer.Text;
                string masterPolicyNo = txtMasterPolicy.Text;
                string masterPolicydt = Calendar1.SelectedDate.ToString();                
                if (masterPolicydt.Contains("1/1/0001") || masterPolicydt.Contains("01-01-0001"))
                {
                    masterPolicydt = "1/1/1900 12:00:00 AM";
                }
                
                DateTime dtmasterPolicy = DateTime.Parse(masterPolicydt);
                string masterPolicyLoc = txtMsterPolicyLoc.Text;
                string loanType = txtLoanType.Text;
                string minPolicyTenure = txtMinTenure.Text;
                string maxPolicyTenure = txtMaxTenure.Text;

                //string lcINSSTR = "Insert into TBL_PLAN_HEAD_MASTER (vPLANCode,vPLANDesc,nStampDuty,nSecAPremium,nExtToSecAPremium,nSecBPremium,nPlanSI,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive)values " +
                //       "('" + vPLANCode.ToUpper() + "','" + vPlanDesc + " ',"+nStampDuty+ "," + nSecAPremium + "," + nExtToSecAPremium + "," + nSecBPremium + "," + nPlanSI + ",'" + Session["vUserLoginId"] + "','" + Session["vUserLoginId"] + "',getdate(),getdate(),'Y')";

                //string lcINSSTR = "Insert into TBL_PLAN_HEAD_MASTER (vPLANCode,vPLANDesc,nStampDuty,nSecAPremium,nExtToSecAPremium,nSecBPremium,nPlanSI,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive, vProductCode, vProductName, vProposalType, vSIBasis, vFinancerName, vMasterPolicyNo, vMasterPolicyDate, vMasterPolicyLoc, vLoanType, vMinPolicyTenure, vMaxPolicytenure)values " +
                //       "('" + vPLANCode.ToUpper() + "','" + vPlanDesc + " '," + nStampDuty + "," + nSecAPremium + "," + nExtToSecAPremium + "," + nSecBPremium + "," + nPlanSI + ",'" + Session["vUserLoginId"] + "','" + Session["vUserLoginId"] + "',getdate(),getdate(),'Y','"+ productCode +"', '"+ productName+"', '"+proposalType +"','"+ siBasis +"','"+ financerName+"','" + masterPolicyNo+ "','"+ dtmasterPolicy + "','"+ masterPolicyLoc+"','"+loanType +"','"+minPolicyTenure +"','"+ maxPolicyTenure+"' )";

                string lcINSSTR = "Insert into TBL_PLAN_HEAD_MASTER (vPLANCode,vPLANDesc,nStampDuty,nSecAPremium,nExtToSecAPremium,nSecBPremium,nPlanSI,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive, vProductCode, vProductName, vProposalType, vSIBasis, vFinancerName, vMasterPolicyNo, vMasterPolicyDate, vMasterPolicyLoc, vLoanType, vMinPolicyTenure, vMaxPolicytenure)values " +
                                   "(@vPLANCode,@vPlanDesc,@nStampDuty,@nSecAPremium,@nExtToSecAPremium ,@nSecBPremium ,@nPlanSI,@vUserLoginId,@vUserLoginId,getdate(),getdate(),'Y',@productCode,@productName,@proposalType,@siBasis,@financerName,@masterPolicyNo,@dtmasterPolicy,@masterPolicyLoc,@loanType,@minPolicyTenure,@maxPolicyTenure)";

                SqlCommand _insertCmd = new SqlCommand(lcINSSTR, _con);

                var SqlParam = new SqlParameter("@vPLANCode" , SqlDbType.VarChar);
                SqlParam.Value = vPLANCode.Trim().ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@vPlanDesc", SqlDbType.VarChar);
                SqlParam.Value = vPlanDesc.Trim().ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@nStampDuty", SqlDbType.VarChar);
                SqlParam.Value = nStampDuty.ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@nSecAPremium", SqlDbType.VarChar);
                SqlParam.Value = nSecAPremium.ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@nExtToSecAPremium", SqlDbType.VarChar);
                SqlParam.Value = nSecAPremium.ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@nSecBPremium", SqlDbType.VarChar);
                SqlParam.Value = nSecAPremium.ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@nPlanSI", SqlDbType.VarChar);
                SqlParam.Value = nSecAPremium.ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@vUserLoginId", SqlDbType.VarChar);
                SqlParam.Value = Session["vUserLoginId"].ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@productCode", SqlDbType.VarChar);
                SqlParam.Value = productCode.Trim().ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@productName", SqlDbType.VarChar);
                SqlParam.Value = productName.Trim().ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@proposalType", SqlDbType.VarChar);
                SqlParam.Value = proposalType.Trim().ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@siBasis", SqlDbType.VarChar);
                SqlParam.Value = siBasis.Trim().ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@financerName", SqlDbType.VarChar);
                SqlParam.Value = siBasis.Trim().ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@masterPolicyNo", SqlDbType.VarChar);
                SqlParam.Value = masterPolicyNo.Trim().ToString();
                _insertCmd.Parameters.Add(SqlParam);


                SqlParam = new SqlParameter("@dtmasterPolicy", SqlDbType.VarChar);
                //SqlParam.Value = dtmasterPolicy.ToString("dd/MM/yyyy");
                SqlParam.Value = dtmasterPolicy.ToShortDateString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@masterPolicyLoc", SqlDbType.VarChar);
                SqlParam.Value = masterPolicyLoc.ToString();
                _insertCmd.Parameters.Add(SqlParam);


                SqlParam = new SqlParameter("@loanType", SqlDbType.VarChar);
                SqlParam.Value = loanType.ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@minPolicyTenure", SqlDbType.VarChar);
                SqlParam.Value = minPolicyTenure.ToString();
                _insertCmd.Parameters.Add(SqlParam);

                SqlParam = new SqlParameter("@maxPolicyTenure", SqlDbType.VarChar);
                SqlParam.Value = minPolicyTenure.ToString();
                _insertCmd.Parameters.Add(SqlParam);


                _insertCmd.Transaction = _tran;
                _insertCmd.ExecuteNonQuery();

                //finds the controls within the gridview and updates them
                for (int i = 0; i < dataRows.Length; i++)
                {
                    string[] dataCells = dataRows[i].Split(cellSeparator, StringSplitOptions.None);

                    string vCoverCode = dataCells[0];
                    string vCoverDesc = dataCells[1];
                    string nCoverSI = dataCells[2];
                    string vCoverSIText = dataCells[3];
                    string bIsActive = dataCells[4];

                    if (bIsActive == "Y")
                    {
                        if (nCoverSI == "" || nCoverSI == "0")
                        {
                            Alert.Show("Please Enter value for CoverSI : " + vCoverDesc);
                            _tran.Rollback();
                            _con.Close();
                            return;
                        }
                    }
                    //lcINSSTR = "Insert into TBL_PLAN_DETAIL_MASTER (vPLANCode,vCoverCode,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive,nCoverSI,vCoverSIText)values " +
                    //   "('" + vPLANCode.ToUpper() + "','" + vCoverCode + " ','" + Session["vUserLoginId"] + "','" + Session["vUserLoginId"] + "',getdate(),getdate(),'" + bIsActive + "','" + nCoverSI + "',@vCoverSIText)";

                    lcINSSTR = "Insert into TBL_PLAN_DETAIL_MASTER (vPLANCode,vCoverCode,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive,nCoverSI,vCoverSIText)values " +
                                                                    "(@vPLANCode,@vCoverCode,@vCreatedBy,@vModifiedBy,getdate(),getdate(),@bIsActive,@nCoverSI,@vCoverSIText)";

                    //lcINSSTR = "Insert into TBL_PLAN_DETAIL_MASTER (vPLANCode,vCoverCode,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive,nCoverSI,vCoverSIText,vProductCode, vProductName, vProposalType, vSIBasis, vFinancerName, vMasterPolicyNo, vMasterPolicyDate, vMasterPolicyLoc, vLoanType, vMinPolicyTenure, vMaxPolicytenure)values " +
                    // "('" + vPLANCode.ToUpper() + "','" + vCoverCode + " ','" + Session["vUserLoginId"] + "','" + Session["vUserLoginId"] + "',getdate(),getdate(),'" + bIsActive + "','" + nCoverSI + "',@vCoverSIText,'"++"')";

                    _insertCmd = new SqlCommand(lcINSSTR, _con);
                    _insertCmd.Parameters.AddWithValue("@nCoverSI", nCoverSI.ToString());
                    _insertCmd.Parameters.AddWithValue("@bIsActive", bIsActive.ToString());
                    _insertCmd.Parameters.AddWithValue("@vModifiedBy", Session["vUserLoginId"].ToString());
                    _insertCmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString());
                    _insertCmd.Parameters.AddWithValue("@vCoverCode", vCoverCode);
                    _insertCmd.Parameters.AddWithValue("@vPLANCode", vPLANCode);
                    _insertCmd.Parameters.AddWithValue("@vCoverSIText", vCoverSIText);

                    _insertCmd.Transaction = _tran;
                    _insertCmd.ExecuteNonQuery();
                }
                _tran.Commit();
                _con.Close();
                Session["ErrorCallingPage"] = "FrmPLANMasterNew.aspx";
                string vStatusMsg = "Plan " + vPlanDesc + " Created Successfully, Please Note PLAN ID: " + vPLANCode;
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;

            }
            catch (Exception ex)
            {
                // log exception
                _tran.Rollback();
                _con.Close();
                Session["ErrorCallingPage"] = "FrmPLANMasterNew.aspx";
                string vStatusMsg = ex.Message;
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
            }

        }
        protected void FillDrpCovers()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            if (!String.IsNullOrEmpty(cmbProduct.SelectedText))
            {
                //string sqlCommand = "select vCoverCode,vCoverDesc,nCoverSI,'' as vCovColNameInExcel,'' as vCovSINameInExcel,'N' as bIsActive, vProductName from TBL_COVER_MASTER  order by vCoverDesc";
                string sqlCommand = "select vCoverCode,vCoverDesc,nCoverSI,'' as vCovColNameInExcel,'' as vCovSINameInExcel,'N' as bIsActive, vProductName from TBL_COVER_MASTER where vProductCode=@vProductCode order by vCoverDesc";
                DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
                var sqlparam = new SqlParameter("@vProductCode", SqlDbType.VarChar);
                sqlparam.Value = cmbProduct.SelectedText.ToString();
                dbCommand.Parameters.Add(sqlparam);
                DataSet dsCorporate = null;
                dsCorporate = db.ExecuteDataSet(dbCommand);

                if (dsCorporate.Tables[0].Rows.Count > 0)
                {
                    gvSubDetails.DataSource = dsCorporate.Tables[0];
                    gvSubDetails.DataBind();
                }
                else
                {
                    Alert.Show("No Covers Defined in Master");
                    return;
                }
            }
            else
            {
                gvSubDetails.DataSource = null;
                gvSubDetails.DataBind();
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string prod_name = string.Empty;

            using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Product_Name FROM TBL_PRODUCT_MASTER where Product_code=@Product_code";
                    cmd.Parameters.AddWithValue("@Product_code", cmbProduct.SelectedValue.ToString());
                    cmd.Connection = sqlCon;
                    sqlCon.Open();
                    object objProd = cmd.ExecuteScalar();
                    prod_name = Convert.ToString(objProd);
                    sqlCon.Close();
                }
            }

            if (!String.IsNullOrEmpty(prod_name))
            {
                lblProdName.Text = prod_name;
            }
            else
            {
                lblProdName.Text = "";
            }

            //FillDrpCovers();
        }

        protected void txtMinTenure_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtMinTenure.Text, "[^1-5]"))
            {
                Alert.Show("Please enter only numbers between 1-5 for Min Policy Tenure.");
                txtMinTenure.Text = txtMinTenure.Text.Remove(txtMinTenure.Text.Length - 1);
            }
        }

        protected void txtMaxTenure_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtMaxTenure.Text, "[^1-5]"))
            {
                Alert.Show("Please enter only numbers between 1-5 for Max Policy Tenure.");
                txtMaxTenure.Text = txtMaxTenure.Text.Remove(txtMaxTenure.Text.Length - 1);
            }
        }
    }
}

