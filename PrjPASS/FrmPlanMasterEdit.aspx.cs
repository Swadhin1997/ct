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
using Obout.Grid;

namespace ProjectPASS
{
    public partial class FrmPlanMasterEdit : System.Web.UI.Page
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
                FillDrpPlans();
                //FillDrpCovers();
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
        protected void FillDrpCovers()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            if (!String.IsNullOrEmpty(cmbProduct.SelectedValue))
            { 
                string sqlCommand = "select vCoverCode,vCoverDesc,nCoverSI,'N' as bIsActive,vProductName from TBL_COVER_MASTER where vProductCode=@vProductCode  order by vCoverDesc";
                DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
                var SqlParam = new SqlParameter("@vProductCode", SqlDbType.VarChar);
                dbCommand.Parameters.Add(SqlParam);
                SqlParam.Value = cmbProduct.SelectedValue.ToString();
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
        protected void FillDrpPlans()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            if (!String.IsNullOrEmpty(cmbProduct.SelectedValue))
            {
                //string sqlCommand = "select distinct vPlanCode,vPlanDesc from TBL_PLAN_HEAD_MASTER order by vPlanDesc";
                string sqlCommand = "select distinct vPlanCode,vPlanDesc from TBL_PLAN_HEAD_MASTER where vProductCode=@vProductCode order by vPlanDesc";
                SqlParameter p = new SqlParameter("@vProductCode", SqlDbType.VarChar);
                p.Value = cmbProduct.SelectedValue.ToString();
                DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
                dbCommand.Parameters.Add(p);
                DataSet dsPlan = null;
                dsPlan = db.ExecuteDataSet(dbCommand);
                drpPlanList.DataValueField = "vPlanCode";
                drpPlanList.DataTextField = "vPlanDesc";
                drpPlanList.DataSource = dsPlan.Tables[0];
                drpPlanList.DataBind();

                if (dsPlan.Tables[0].Rows.Count > 0)
                {
                    Obout.ComboBox.ComboBoxItem l_lstItem = new ComboBoxItem("Select", "ALL");
                    drpPlanList.Items.Insert(0, l_lstItem);
                }
                else
                {
                    Obout.ComboBox.ComboBoxItem l_lstItem = new Obout.ComboBox.ComboBoxItem("No-Plans Defined in Master", "No");
                    drpPlanList.Items.Insert(0, l_lstItem);
                }
            }
        }
        //called on row edit command
        protected void gvSubDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //gvSubDetails.EditIndex = e.NewEditIndex;
            //BindData();
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //Find the DropDownList in the Row

                DropDownList drpbIsActive = (e.Row.FindControl("drpbIsActive") as DropDownList);

                //Select the Country of Customer in DropDownList

                string bIsActive = (e.Row.FindControl("lblbIsActive") as Label).Text;

                drpbIsActive.Items.FindByValue(bIsActive).Selected = true;

            }

        }

        //called when cancel edit mode
        protected void gvSubDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //bool IsUpdated = false;

            //string vCoverCode = gvSubDetails.DataKeys[e.RowIndex].Value.ToString();
            ////getting row field details
            //TextBox txtvCovColNameInExcel = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("vCovColNameInExcel");
            //DropDownList drpbIsActive = (DropDownList)gvSubDetails.Rows[e.RowIndex].FindControl("drpbIsActive");

            //try
            //{
            //    //Open the SqlConnection     
            //    Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            //    string sqlCommand;
            //    DbCommand dbCommand;
            //    sqlCommand = "update TBL_PLAN_MASTER set bIsActive='" + drpbIsActive + "',vCovColNameInExcel='" + txtvCovColNameInExcel.Text + "',vModifiedBy='" + Session["vUserLoginId"] + "' where vCoverCode = '" + vCoverCode + "' and vPlanCode ='"+drpPlanList.SelectedValue+"'";
            //    dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
            //    dbCOMMON.ExecuteNonQuery(dbCommand);
            //    IsUpdated = true;
            //}
            //catch (Exception ex)
            //{
            //    IsUpdated = false;
            //}
            //gvSubDetails.EditIndex = -1;
            //BindData();
        }

        //called on row update command
        protected void gvSubDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //bool IsUpdated = false;

            //string vCoverCode = gvSubDetails.DataKeys[e.RowIndex].Value.ToString();
            ////getting row field details
            //TextBox txtvCovColNameInExcel = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("vCovColNameInExcel");
            //DropDownList drpbIsActive = (DropDownList)gvSubDetails.Rows[e.RowIndex].FindControl("drpbIsActive");

            //try
            //{
            //    //Open the SqlConnection     
            //    Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            //    string sqlCommand;
            //    DbCommand dbCommand;
            //    sqlCommand = "update TBL_PLAN_MASTER set bIsActive='" + drpbIsActive + "',vCovColNameInExcel='" + txtvCovColNameInExcel.Text + "',vModifiedBy='" + Session["vUserLoginId"] + "' where vCoverCode = '" + vCoverCode + "' and vPlanCode ='" + drpPlanList.SelectedValue + "'";
            //    dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
            //    dbCOMMON.ExecuteNonQuery(dbCommand);
            //    IsUpdated = true;
            //}
            //catch (Exception ex)
            //{
            //    IsUpdated = false;
            //}
            //gvSubDetails.EditIndex = -1;
            //BindData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (txtPlanDesc.Text == "")
            //{
            //    FillDrpCovers();
            //    Alert.Show("Please Enter Plan Description");
            //    return;
            //}
            if (txtStampDuty.Text == "")
            {
                FillDrpCovers();
                Alert.Show("Please Enter Valid Stamp Duty Value");
                return;
            }
            if (Convert.ToDouble(txtStampDuty.Text) == 0)
            {
                FillDrpCovers();
                Alert.Show("Please Enter Valid Stamp Duty Value");
                return;
            }
            if (txtSecAPrem.Text == "")
            {
                FillDrpCovers();
                Alert.Show("Please Enter Valid Section A Premium Value");
                return;
            }
            if (txtExtToSecAPrem.Text == "")
            {
                FillDrpCovers();
                Alert.Show("Please Enter Valid Extension To Section A Premium Value");
                return;
            }
            if (txtSecBPrem.Text == "")
            {
                FillDrpCovers();
                Alert.Show("Please Enter Valid Section B Premium Value");
                return;
            }
            if (txtPlanSI.Text == "")
            {
                FillDrpCovers();
                Alert.Show("Please Enter Valid Plan SI Value");
                return;
            }
            if (Convert.ToDouble(txtPlanSI.Text) == 0)
            {
                FillDrpCovers();
                Alert.Show("Please Enter Valid Plan SI Value");
                return;
            }

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

                vPLANCode = drpPlanList.SelectedValue;

                string lcINSSTR = "DELETE FROM TBL_PLAN_HEAD_MASTER WHERE vPlanCode = @vPlanCode";
                SqlCommand _insertCmd = new SqlCommand(lcINSSTR, _con);
                SqlParameter p = new SqlParameter("@vPlanCode", SqlDbType.VarChar);
                p.Value = drpPlanList.SelectedValue.ToString();
                _insertCmd.Parameters.Add(p);
                _insertCmd.Transaction = _tran;
                _insertCmd.ExecuteNonQuery();

                lcINSSTR = "DELETE FROM TBL_PLAN_DETAIL_MASTER WHERE vPlanCode =@vPlanCode";
                _insertCmd = new SqlCommand(lcINSSTR, _con);
                SqlParameter p2 = new SqlParameter("@vPlanCode", SqlDbType.VarChar);
                p2.Value = drpPlanList.SelectedValue.ToString();
                _insertCmd.Parameters.Add(p2);
                _insertCmd.Transaction = _tran;
                _insertCmd.ExecuteNonQuery();

                string vPlanDesc = drpPlanList.SelectedText;
                double nStampDuty = Convert.ToDouble(txtStampDuty.Text);
                double nSecAPremium = Convert.ToDouble(txtSecAPrem.Text);
                double nExtToSecAPremium = Convert.ToDouble(txtExtToSecAPrem.Text);
                double nSecBPremium = Convert.ToDouble(txtSecBPrem.Text);
                double nPlanSI = Convert.ToDouble(txtPlanSI.Text);

               
                DateTime masterDate;
                if (Calendar1.SelectedDate.ToShortDateString() == "1/1/0001")
                {
                    //date has not been modified
                    //                    masterDate = DateTime.Parse(txtFromDate.Text);

                    if (String.IsNullOrEmpty(hdnMasterDate.Value))
                    {
                        masterDate = DateTime.Parse("1/1/1900");
                    }
                    else
                    {
                        masterDate = DateTime.Parse(hdnMasterDate.Value);
                    }
                }
                else
                {
                    //date has been modified
                    masterDate = DateTime.Parse(Calendar1.SelectedDate.ToString());
                }

                //lcINSSTR = "Insert into TBL_PLAN_HEAD_MASTER (vPLANCode,vPLANDesc,nStampDuty,nSecAPremium,nExtToSecAPremium,nSecBPremium,nPlanSI,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive,vProductCode,vProductName,vProposalType,vSIBasis,vFinancerName,vMasterPolicyNo,vMasterPolicyDate,vMasterPolicyLoc,vLoanType,vMinPolicyTenure,vMaxPolicyTenure)values " +
                //       "('" + vPLANCode.ToUpper() + "','" + vPlanDesc + " '," + nStampDuty + "," + nSecAPremium + "," + nExtToSecAPremium + "," + nSecBPremium + "," + nPlanSI + ",'" + Session["vUserLoginId"] + "','" + Session["vUserLoginId"] + "',getdate(),getdate(),'Y','"+ cmbProduct.SelectedText+"','"+hdnProductName.Value+"','"+cmbProposal.SelectedText +"','"+cmbSIBasis.SelectedText+"','"+txtFinancer.Text +"','"+txtMasterPolicyNo.Text+"','"+ masterDate.ToShortDateString()+"','"+ txtMasterPolicyLoc.Text+"','"+ txtLoanType.Text+"','"+ txtMinPolicyTenure.Text+"','"+ txtMaxPolicyTenure.Text+"')";

                lcINSSTR = "Insert into TBL_PLAN_HEAD_MASTER (vPLANCode,vPLANDesc,nStampDuty,nSecAPremium,nExtToSecAPremium,nSecBPremium,nPlanSI,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive,vProductCode,vProductName,vProposalType,vSIBasis,vFinancerName,vMasterPolicyNo,vMasterPolicyDate,vMasterPolicyLoc,vLoanType,vMinPolicyTenure,vMaxPolicyTenure)values " +
                "(@vPLANCode,@vPLANDesc,@nStampDuty,@nSecAPremium,@nExtToSecAPremium,@nSecBPremium,@nPlanSI,@vCreatedBy,@vModifiedBy,getdate(),getdate(),@bIsActive,@vProductCode,@vProductName,@vProposalType,@vSIBasis,@vFinancerName,@vMasterPolicyNo,@vMasterPolicyDate,@vMasterPolicyLoc,@vLoanType,@vMinPolicyTenure,@vMaxPolicyTenure)";


                _insertCmd = new SqlCommand(lcINSSTR, _con);
                _insertCmd.Transaction = _tran;
                

                p = new SqlParameter("@vPLANCode", SqlDbType.VarChar);
                p.Value = vPLANCode.ToString();
                _insertCmd.Parameters.Add(p);


                p = new SqlParameter("@vPLANDesc", SqlDbType.VarChar);
                p.Value = vPlanDesc.ToString();
                _insertCmd.Parameters.Add(p);


                p = new SqlParameter("@nStampDuty", SqlDbType.VarChar);
                p.Value = nStampDuty.ToString();
                _insertCmd.Parameters.Add(p);


                p = new SqlParameter("@nSecAPremium", SqlDbType.VarChar);
                p.Value = nSecAPremium.ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@nExtToSecAPremium", SqlDbType.VarChar);
                p.Value = nExtToSecAPremium.ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@nSecBPremium", SqlDbType.VarChar);
                p.Value = nSecBPremium.ToString();
                _insertCmd.Parameters.Add(p);


                p = new SqlParameter("@nPlanSI", SqlDbType.VarChar);
                p.Value = nPlanSI.ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vCreatedBy", SqlDbType.VarChar);
                p.Value = Session["vUserLoginId"].ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vModifiedBy", SqlDbType.VarChar);
                p.Value = Session["vUserLoginId"].ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@bIsActive", SqlDbType.VarChar);
                p.Value = 'Y';
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vProductCode", SqlDbType.VarChar);
                p.Value = cmbProduct.SelectedText.ToString() ;
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vProductName", SqlDbType.VarChar);
                p.Value = hdnProductName.Value.ToString();
                _insertCmd.Parameters.Add(p);


                p = new SqlParameter("@vProposalType", SqlDbType.VarChar);
                p.Value = cmbProposal.SelectedText.ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vSIBasis", SqlDbType.VarChar);
                p.Value = cmbSIBasis.SelectedText.ToString();
                _insertCmd.Parameters.Add(p);


                p = new SqlParameter("@vFinancerName", SqlDbType.VarChar);
                p.Value = txtFinancer.Text.ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vMasterPolicyNo", SqlDbType.VarChar);
                p.Value = txtMasterPolicyNo.Text.ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vMasterPolicyDate", SqlDbType.VarChar);
                p.Value = masterDate.ToShortDateString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vMasterPolicyLoc", SqlDbType.VarChar);
                p.Value = txtMasterPolicyLoc.Text.ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vLoanType", SqlDbType.VarChar);
                p.Value = txtLoanType.Text.ToString();
                _insertCmd.Parameters.Add(p);

                p = new SqlParameter("@vMinPolicyTenure", SqlDbType.VarChar);
                p.Value = txtMinPolicyTenure.Text.ToString();
                _insertCmd.Parameters.Add(p);


                p = new SqlParameter("@vMaxPolicyTenure", SqlDbType.VarChar);
                p.Value = txtMaxPolicyTenure.Text.ToString();
                _insertCmd.Parameters.Add(p);

                _insertCmd.ExecuteNonQuery();
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

                    _insertCmd = new SqlCommand(lcINSSTR, _con);


                    var sqlPar = new SqlParameter("@vPLANCode", SqlDbType.VarChar);
                    sqlPar.Value = vPLANCode.Trim().ToUpper();
                    _insertCmd.Parameters.Add(sqlPar);

                    sqlPar = new SqlParameter("@vCoverCode", SqlDbType.VarChar);
                    sqlPar.Value = vCoverCode.Trim();
                    _insertCmd.Parameters.Add(sqlPar);

                    sqlPar = new SqlParameter("@vCreatedBy", SqlDbType.VarChar);
                    sqlPar.Value = Session["vUserLoginId"].ToString();
                    _insertCmd.Parameters.Add(sqlPar);

                    sqlPar = new SqlParameter("@vModifiedBy", SqlDbType.VarChar);
                    sqlPar.Value = Session["vUserLoginId"].ToString();
                    _insertCmd.Parameters.Add(sqlPar);

                    sqlPar = new SqlParameter("@bIsActive", SqlDbType.VarChar);
                    sqlPar.Value = bIsActive.ToString();
                    _insertCmd.Parameters.Add(sqlPar);

                    sqlPar = new SqlParameter("@nCoverSI", SqlDbType.VarChar);
                    sqlPar.Value = nCoverSI.ToString();
                    _insertCmd.Parameters.Add(sqlPar);


                    sqlPar = new SqlParameter("@vCoverSIText", SqlDbType.VarChar);
                    sqlPar.Value = nCoverSI.ToString();
                    _insertCmd.Parameters.Add(sqlPar);
               
                    _insertCmd.Transaction = _tran;
                    _insertCmd.ExecuteNonQuery();
                }
                _tran.Commit();
                _con.Close();
                Session["ErrorCallingPage"] = "FrmPLANMasterEdit.aspx";
                string vStatusMsg = "Plan " + vPlanDesc + " Created Successfully, Please Note PLAN ID: " + vPLANCode;
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;

            }
            catch (Exception ex)
            {
                // log exception
                _tran.Rollback();
                _con.Close();
                Session["ErrorCallingPage"] = "FrmPLANMasterEdit.aspx";
                string vStatusMsg = ex.Message;
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
            }
        }
        protected void BindData()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            //string sqlCommand = "Select a.nStampDuty,a.nSecAPremium,a.nExtToSecAPremium,a.nSecBPremium,a.nPlanSI,a.vPlanCode,a.vPlanDesc,c.bIsActive,"+
            //    " c.vCoverCode,b.vCoverDesc,c.nCoverSI,c.vCoverSIText from TBL_PLAN_HEAD_MASTER a ,TBL_COVER_MASTER b,TBL_PLAN_DETAIL_MASTER c where a.vPlanCode = c.vPlanCode "+
            //    " and c.vCoverCode = b.vCoverCode and a.vPlanCode = '" + drpPlanList.SelectedValue + "' Order by a.vPlanDesc";

            string sqlCommand = "Select a.nStampDuty,a.nSecAPremium,a.nExtToSecAPremium,a.nSecBPremium,a.nPlanSI,a.vPlanCode,a.vPlanDesc,c.bIsActive," +
                " c.vCoverCode,b.vCoverDesc,c.nCoverSI,c.vCoverSIText,a.vProposalType,a.vSIBasis,a.vFinancerName,a.vMasterPolicyNo,a.vMasterPolicyDate, a.vMasterPolicyLoc,a.vLoanType, a.vMinPolicyTenure,a.vMaxPolicytenure, a.vProductName from TBL_PLAN_HEAD_MASTER a ,TBL_COVER_MASTER b,TBL_PLAN_DETAIL_MASTER c where a.vPlanCode = c.vPlanCode " +
                " and c.vCoverCode = b.vCoverCode and a.vPlanCode = @vPlanCode Order by a.vPlanDesc";

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet ds = null;
            var sqlPar = new SqlParameter("@vPlanCode", SqlDbType.VarChar);
            sqlPar.Value = drpPlanList.SelectedValue.ToString();
            dbCommand.Parameters.Add(sqlPar);


            ds = db.ExecuteDataSet(dbCommand);

            DataTable dtRequisition = new DataTable();

            dtRequisition = ds.Tables[0];

            //Create DataTable

            DataTable dt = new DataTable();

            //Put some columns in it.

            //dt.Columns.Add(new DataColumn("vPlanCode", typeof(string)));
            //dt.Columns.Add(new DataColumn("vPlanDesc", typeof(string)));
            dt.Columns.Add(new DataColumn("vCoverCode", typeof(string)));
            dt.Columns.Add(new DataColumn("vCoverDesc", typeof(string)));
            dt.Columns.Add(new DataColumn("nCoverSI", typeof(string)));
            dt.Columns.Add(new DataColumn("vCoverSIText", typeof(string)));
            dt.Columns.Add(new DataColumn("bIsActive", typeof(string)));
            dt.Columns.Add(new DataColumn("vProductName", typeof(string)));


            // Create the record
            if (dtRequisition.Rows.Count > 0)
            {
                txtExtToSecAPrem.Text = dtRequisition.Rows[0]["nExtToSecAPremium"].ToString();
                txtPlanSI.Text = dtRequisition.Rows[0]["nPlanSI"].ToString();
                txtSecAPrem.Text = dtRequisition.Rows[0]["nSecAPremium"].ToString();
                txtSecBPrem.Text = dtRequisition.Rows[0]["nSecBPremium"].ToString();
                txtStampDuty.Text = dtRequisition.Rows[0]["nStampDuty"].ToString();
                cmbProposal.SelectedValue = dtRequisition.Rows[0]["vProposalType"].ToString();                
                cmbSIBasis.SelectedValue = dtRequisition.Rows[0]["vSIBasis"].ToString();
                txtFinancer.Text = dtRequisition.Rows[0]["vFinancerName"].ToString();
                txtMasterPolicyNo.Text = dtRequisition.Rows[0]["vMasterPolicyNo"].ToString();
                if (String.IsNullOrEmpty(dtRequisition.Rows[0]["vMasterPolicyDate"].ToString()))
                {
                    txtFromDate.Text = "";
                }
                else
                {
                    txtFromDate.Text = Convert.ToDateTime(dtRequisition.Rows[0]["vMasterPolicyDate"]).ToString("dd/MM/yyyy");
                    if (txtFromDate.Text == "01/01/1900")
                    {
                        txtFromDate.Text = "";
                    }
                }
                if (String.IsNullOrEmpty(dtRequisition.Rows[0]["vMasterPolicyDate"].ToString()))
                {
                    hdnMasterDate.Value = "";
                }
                else
                {
                    hdnMasterDate.Value = Convert.ToDateTime(dtRequisition.Rows[0]["vMasterPolicyDate"]).ToString();
                }
                txtMasterPolicyLoc.Text = dtRequisition.Rows[0]["vMasterPolicyLoc"].ToString();
                txtLoanType.Text = dtRequisition.Rows[0]["vLoanType"].ToString();
                txtMinPolicyTenure.Text = dtRequisition.Rows[0]["vMinPolicyTenure"].ToString();
                txtMaxPolicyTenure.Text = dtRequisition.Rows[0]["vMaxPolicyTenure"].ToString();

                foreach (DataRow row in dtRequisition.Rows)
                {
                    DataRow dr = dt.NewRow();
                    //dr["vPlanCode"] = row["vPlanCode"];
                    //dr["vPlanDesc"] = row["vPlanDesc"];
                    dr["vCoverCode"] = row["vCoverCode"];
                    dr["vCoverDesc"] = row["vCoverDesc"];
                    dr["nCoverSI"] = row["nCoverSI"];
                    dr["vCoverSIText"] = row["vCoverSIText"];
                    dr["bIsActive"] = row["bIsActive"];
                    dr["vProductName"] = row["vProductName"];
                    dt.Rows.Add(dr);
                }
                gvSubDetails.DataSource = dt;
                gvSubDetails.DataBind();
            }
            else
            {
                Alert.Show("There was error processing the Request or Plan Does Not Exist");
                return;
            }
            
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void drpPlanList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void cmbProduct_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
        {            
            
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string prod_name = string.Empty;

            using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Product_Name FROM TBL_PRODUCT_MASTER where Product_code=@Product_code";
                    cmd.Connection = sqlCon;
                    cmd.Parameters.AddWithValue("@Product_code", cmbProduct.SelectedValue.ToString());
                    sqlCon.Open();
                    object objProd = cmd.ExecuteScalar();
                    prod_name = Convert.ToString(objProd);
                    sqlCon.Close();
                }
            }

            if (!String.IsNullOrEmpty(prod_name))
            {
                hdnProductName.Value = prod_name;
            }
            else
            {
                hdnProductName.Value = "";
            }

            drpPlanList.Items.Clear();
            FillDrpPlans();
            FillDrpCovers();
        }

        protected void txtMinPolicyTenure_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtMinPolicyTenure.Text, "[^0-9]"))
            {
                Alert.Show("Please enter only numbers for Min Policy Tenure.");
                txtMinPolicyTenure.Text = txtMinPolicyTenure.Text.Remove(txtMinPolicyTenure.Text.Length - 1);
            }
        }

        protected void txtMaxPolicyTenure_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtMaxPolicyTenure.Text, "[^0-9]"))
            {
                Alert.Show("Please enter only numbers for Max Policy Tenure.");
                txtMaxPolicyTenure.Text = txtMaxPolicyTenure.Text.Remove(txtMaxPolicyTenure.Text.Length - 1);
            }
        }
    }
}

