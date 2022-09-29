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
using Obout.Grid;
using Obout.Interface;
using System.Web.Services;
using PrjPASS;
using System.Text.RegularExpressions;
using System.Web.ModelBinding;

namespace ProjectPASS
{
    public partial class FrmCreateUserMasterNewEdit : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUsrLoginPass.Text = "";
                txtUsrEmpDesc.Text = "";

                if (Session["vUserLoginId"] != null)
                {
                    if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
                    {
                        bool chkAuth;
                        string pageName = this.Page.ToString().Substring(4, this.Page.ToString().Substring(4).Length - 5) + ".aspx";
                        chkAuth = wsGen.Fn_Check_Rights_For_Page(Session["vRoleCode"].ToString(), pageName);
                        if (chkAuth == false)
                        {
                            //ViewState["vUserLoginId"] = null;
                            Alert.Show("Access Denied", "FrmMainMenu.aspx");
                            return;
                        }
                    }
                    //ViewState["vUserLoginId"] = Session["vUserLoginId"].ToString();
                }
                else
                {
                    Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    return;
                }
                BindData();
                FillDeptList();
            }
            else
            {
                txtUsrLoginPass.Attributes["value"] = txtUsrLoginPass.Text.Trim();
            }

            //if (ViewState["vUserLoginId"] != null)
            //{
            //    if (ViewState["vUserLoginId"].ToString().ToLower() != Session["vUserLoginId"].ToString().ToLower())
            //    {
            //        Response.Redirect("FrmMainMenu.aspx");
            //    }
            //}
            //else
            //{
            //    Response.Redirect("FrmMainMenu.aspx");
            //}
            

            lblMarketMovementDeviation.Text = ConfigurationManager.AppSettings["MarketMovementDeviation"].ToString();
        }

        protected void FillDeptList()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_DEPT_FOR_LOGGED_IN_USER";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "vUserLoginId", DbType.String, ParameterDirection.Input, "vUserLoginId", DataRowVersion.Current, Session["vUserLoginId"].ToString().ToUpper());

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                drpDept.DataValueField = "DEPT_CODE";
                drpDept.DataTextField = "DEPT_NAME";
                drpDept.DataSource = ds.Tables[0];
                drpDept.DataBind();

                //drpDept.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

                FillDeptRoleList(drpDept.SelectedValue);
            }
        }

        protected void FillDeptRoleList(string deptCode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_DEPT_ROLE_MAPPED";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "DEPT_CODE", DbType.String, ParameterDirection.Input, "DEPT_CODE", DataRowVersion.Current, deptCode);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                drpRole.DataValueField = "VROLECODE";
                drpRole.DataTextField = "VROLEDESC";
                drpRole.DataSource = ds.Tables[0];
                drpRole.DataBind();
            }
        }

        private bool VerifyIntermediary(string IntermediaryCode)
        {
            bool IsCorrect = false;
            if (string.IsNullOrEmpty(IntermediaryCode))
            {
                IsCorrect = true;
            }
            else
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_VERIFY_INTERMEDIARY";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "INTERMEDIARY_CODE", DbType.String, ParameterDirection.Input, "INTERMEDIARY_CODE", DataRowVersion.Current, IntermediaryCode);

                DataSet ds = null;
                ds = db.ExecuteDataSet(dbCommand);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsCorrect = ds.Tables[0].Rows[0][0].ToString() == "1" ? true : false;
                    }
                }
            }
            return IsCorrect;
        }

        ////called on row edit command
        //protected void gvSubDetails_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    gvSubDetails.EditIndex = e.NewEditIndex;
        //    BindData();
        //}


        //called when cancel edit mode
        //protected void gvSubDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    bool IsUpdated = false;
        //    string vUserLoginId = gvSubDetails.DataKeys[e.RowIndex].Value.ToString();
        //    //getting row field details
        //    TextBox txtUserEmpDescEditGrid = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtUserEmpDescEditGrid");
        //    TextBox txtUserPassEditGrid = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtUserPassEditGrid");
        //    DropDownList drpuserstatusEditGrid = (DropDownList)gvSubDetails.Rows[e.RowIndex].FindControl("drpuserstatusEditGrid");

        //    try
        //    {
        //        //Open the SqlConnection     
        //        Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
        //        string sqlCommand;
        //        string vUserPassword = "";
        //        //Update Query to update the Datatable      
        //        sqlCommand = "Select a.vUserLoginId,a.vUserLoginDesc,a.vUserPassword,a.bIsActivate from TBL_USER_LOGIN a where  vUserLoginId = '" + vUserLoginId + "' Order by a.vUserLoginDesc";
        //        DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
        //        DataSet ds = null;
        //        ds = dbCOMMON.ExecuteDataSet(dbCommand);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0]["vUserPassword"].ToString() != txtUserPassEditGrid.Text)
        //            {
        //                vUserPassword = ",vUserPassword=dbo.fnEncDecRc4('Kotak','" + txtUserPassEditGrid.Text + "')";
        //            }
        //            else
        //            {
        //                vUserPassword = "";
        //            }
        //        }
        //        if (drpuserstatusEditGrid.SelectedItem.Value == "N")
        //        {
        //            sqlCommand = "update TBL_USER_LOGIN set bIsActivate='N',dDeactivatedon=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "',vLocked='N',vUserLoginDesc='" + txtUserEmpDescEditGrid.Text + "' " + vUserPassword + " where vUserLoginId = '" + vUserLoginId + "'";
        //        }
        //        else
        //        {
        //            sqlCommand = "update TBL_USER_LOGIN set bIsActivate='Y',dModifieddate=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "',vLocked='N',vUserLoginDesc='" + txtUserEmpDescEditGrid.Text + "' " + vUserPassword + " where vUserLoginId = '" + vUserLoginId + "'";
        //        }
        //        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
        //        dbCOMMON.ExecuteNonQuery(dbCommand);
        //        IsUpdated = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        IsUpdated = false;
        //    }
        //    gvSubDetails.EditIndex = -1;
        //    BindData();
        //}

        //called on row update command
        //protected void gvSubDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    bool IsUpdated = false;
        //    // int index = Convert.ToInt32(e.CommandArgument.ToString());

        //    //Get the primary key value using the DataKeyValue.   
        //    string vUserLoginId = gvSubDetails.DataKeys[e.RowIndex].Value.ToString();
        //    //getting row field details
        //    TextBox txtUserEmpDescEditGrid = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtUserEmpDescEditGrid");
        //    TextBox txtUserPassEditGrid = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtUserPassEditGrid");
        //    DropDownList drpuserstatusEditGrid = (DropDownList)gvSubDetails.Rows[e.RowIndex].FindControl("drpuserstatusEditGrid");

        //    try
        //    {
        //        //Open the SqlConnection     
        //        Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
        //        string sqlCommand;
        //        string vUserPassword = "";
        //        //Update Query to update the Datatable      
        //        sqlCommand = "Select a.vUserLoginId,a.vUserLoginDesc,a.vUserPassword,a.bIsActivate from TBL_USER_LOGIN a where  vUserLoginId = '" + vUserLoginId + "' Order by a.vUserLoginDesc";
        //        DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
        //        DataSet ds = null;
        //        ds = dbCOMMON.ExecuteDataSet(dbCommand);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0]["vUserPassword"].ToString() != txtUserPassEditGrid.Text)
        //            {
        //                vUserPassword = ",vUserPassword=dbo.fnEncDecRc4('Kotak','" + txtUserPassEditGrid.Text + "')";
        //            }
        //            else
        //            {
        //                vUserPassword = "";
        //            }
        //        }
        //        if (drpuserstatusEditGrid.SelectedItem.Value == "N")
        //        {
        //            sqlCommand = "update TBL_USER_LOGIN set bIsActivate='N',dDeactivatedon=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "',vLocked='N',vUserLoginDesc='" + txtUserEmpDescEditGrid.Text + "' " + vUserPassword + " where vUserLoginId = '" + vUserLoginId + "'";
        //        }
        //        else
        //        {
        //            sqlCommand = "update TBL_USER_LOGIN set bIsActivate='Y',dModifieddate=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "',vLocked='N',vUserLoginDesc='" + txtUserEmpDescEditGrid.Text + "' " + vUserPassword + " where vUserLoginId = '" + vUserLoginId + "'";
        //        }
        //        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
        //        dbCOMMON.ExecuteNonQuery(dbCommand);
        //        IsUpdated = true;  
        //    }
        //    catch (Exception ex)
        //    {
        //        IsUpdated = false;
        //    }
        //    if (IsUpdated)
        //    {
        //        lblstatus.Text = "'" + txtUserEmpDescEditGrid.Text + "'  details updated successfully!";
        //        lblstatus.ForeColor = System.Drawing.Color.Green;
        //    }
        //    else
        //    {
        //        lblstatus.Text = "Error while updating '" + txtUserEmpDescEditGrid.Text + "'  details";
        //        lblstatus.ForeColor = System.Drawing.Color.Red;
        //    }
        //    gvSubDetails.EditIndex = -1;
        //    BindData();
        //}
        protected void gvSubDetails_RowUpdating(object sender, GridRecordEventArgs e)
        {
            bool IsUpdated = false;
            // int index = Convert.ToInt32(e.CommandArgument.ToString());

            string vUserLoginId = e.Record["vUserLoginId"].ToString();
            string vUserLoginDesc = e.Record["vUserLoginDesc"].ToString();
            string vUserPasswordgrd = e.Record["vUserPassword"].ToString();
            string bIsActivate = e.Record["bIsActivate"].ToString();
            string vIntermediaryCode = e.Record["vIntermediaryCode"].ToString();
            string vIntermediaryBranch = e.Record["vIntermediaryBranch"].ToString();

            if (VerifyIntermediary(vIntermediaryCode))
            {
                try
                {
                    //Open the SqlConnection     
                    Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
                    string sqlCommand;
                    string vUserPassword = "";
                    //Update Query to update the Datatable      
                    sqlCommand = "Select a.vUserLoginId,a.vUserLoginDesc,a.vUserPassword,a.bIsActivate,vIntermediaryCode, vIntermediaryBranch from TBL_USER_LOGIN a where  vUserLoginId = '" + vUserLoginId + "' Order by a.vUserLoginDesc";
                    DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                    DataSet ds = null;
                    ds = dbCOMMON.ExecuteDataSet(dbCommand);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["vUserPassword"].ToString() != vUserPasswordgrd)
                        {
                            vUserPassword = ",vUserPassword=dbo.fnEncDecRc4('Kotak','" + vUserPasswordgrd + "')";
                        }
                        else
                        {
                            vUserPassword = "";
                        }
                    }
                    if (bIsActivate == "N")
                    {
                        sqlCommand = "update TBL_USER_LOGIN set vIntermediaryCode = '" + vIntermediaryCode + "', vIntermediaryBranch = '" + vIntermediaryBranch + "', bIsActivate='N',dDeactivatedon=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "',vLocked='N',vUserLoginDesc='" + vUserLoginDesc + "' " + vUserPassword + " where vUserLoginId = '" + vUserLoginId + "'";
                    }
                    else
                    {
                        sqlCommand = "update TBL_USER_LOGIN set vIntermediaryCode = '" + vIntermediaryCode + "', vIntermediaryBranch = '" + vIntermediaryBranch + "', bIsActivate='Y',dModifieddate=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "',vLocked='N',vUserLoginDesc='" + vUserLoginDesc + "' " + vUserPassword + " where vUserLoginId = '" + vUserLoginId + "'";
                    }
                    dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                    dbCOMMON.ExecuteNonQuery(dbCommand);
                    IsUpdated = true;
                }
                catch (Exception ex)
                {
                    IsUpdated = false;
                }
                if (IsUpdated)
                {
                    lblstatus.Text = "'" + vUserLoginDesc + "'  details updated successfully!";
                    lblstatus.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblstatus.Text = "Error while updating '" + vUserLoginDesc + "'  details";
                    lblstatus.ForeColor = System.Drawing.Color.Red;
                }
                //gvSubDetails.EditIndex = -1;
                BindData();
            }
            else
            {
                Alert.Show("Intermediary Code Not Present.");
            }
        }

        private void SaveUserDetails(UserDetails objUserDetails)
        {
            if (VerifyIntermediary(objUserDetails.vIntermediaryCode))
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                Cls_General_Functions wsDocNo = new Cls_General_Functions();
                string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
                string vEmpCode = objUserDetails.vUserLoginId.ToUpper();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString);
                try
                {
                    if (con.State == ConnectionState.Closed)
                    { con.Open(); }
                    SqlCommand cmd = new SqlCommand("PROC_SAVEUSERDETAILS", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vUserLoginId", objUserDetails.vUserLoginId);
                    cmd.Parameters.AddWithValue("@vUserLoginDesc", objUserDetails.vUserLoginDesc);
                    cmd.Parameters.AddWithValue("@vUserPassword", objUserDetails.vUserPassword);
                    cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString());
                    cmd.Parameters.AddWithValue("@bIsActivate", OboutChkActive.Checked == true ? 'Y' : 'N');
                    cmd.Parameters.AddWithValue("@vIntermediaryCode", objUserDetails.vIntermediaryCode);
                    cmd.Parameters.AddWithValue("@vIntermediaryBranch", objUserDetails.vIntermediaryBranch);
                    cmd.Parameters.AddWithValue("@vUserEmailId", objUserDetails.vUserEmailId);
                    cmd.Parameters.AddWithValue("@IsExternalUser", (chkIsExternalUser.Checked == true) ? 1 : 0);
                    cmd.Parameters.AddWithValue("@Min_MarketMovement", Convert.ToInt32(txtMinMarketMovementDeviation.Text));
                    cmd.Parameters.AddWithValue("@DEPT_CODE", drpDept.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@vRoleCode", drpRole.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@vRoleDesc", drpRole.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@IsInsert", (HdFldSave.Value.ToString() == "Update") ? 0 : 1);
                    cmd.Parameters.AddWithValue("@IsAllowLoginFromMobile", (OboutChkMobileLogin.Checked == true) ? 1 : 0);
                    cmd.Parameters.AddWithValue("@IsAllowLoginFromChotuPASS", (OboutChkChotuPASSLogin.Checked == true) ? 1 : 0);
                    cmd.Parameters.AddWithValue("@TypeOfUser", drpUserType.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@IsAllowEPOSQuoteView", OboutChkEPOSQuoteView.Checked == true ? 1 : 0);
                    cmd.Parameters.AddWithValue("@RegionalDeptHeadEmailId", ObouttxtRegDeptHeadEmail.Text);
                    cmd.Parameters.AddWithValue("@vLocked", OboutChkLocked.Checked == true ? 'Y' : 'N');
                    SqlParameter outputMessage = new SqlParameter("@outputMessage", SqlDbType.VarChar, 200);
                    outputMessage.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputMessage);
                    cmd.ExecuteNonQuery();
                    string ReturnMessage = outputMessage.Value.ToString();
                    Alert.Show(ReturnMessage.ToString(), "FrmCreateUserMasterNewEdit.aspx");

                }
                catch (Exception ex)
                {
                    // log exception
                    ExceptionUtility.LogException(ex, "SaveUserDetails(UserDetails objUserDetails)");
                    Alert.Show("Some error occured");
                }
                finally
                {
                    con.Close();
                    HdFldSave.Value = "Insert";
                    // BindData();
                }
            }
            else
            {
                Alert.Show("Intermediary Code Not Present.");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                string ErrorMsg = string.Empty;
                if (IsError(out ErrorMsg))
                {
                    Alert.Show(ErrorMsg);
                }
                else
                {
                    UserDetails objUserDetails = new UserDetails();
                    objUserDetails.vUserLoginId = txtUserId.Text.Trim();
                    objUserDetails.vUserLoginDesc = txtUsrEmpDesc.Text.Trim();
                    objUserDetails.vIntermediaryBranch = txtIntermediaryBranch.Text.Trim();
                    objUserDetails.vIntermediaryCode = txtIntermediaryCode.Text.Trim();
                    objUserDetails.vUserPassword = txtUsrLoginPass.Text.Trim();
                    objUserDetails.vUserEmailId = txtemail.Text.Trim();
                    objUserDetails.IsExternalUser = chkIsExternalUser.Checked;
                    objUserDetails.Min_MarketMovement = Convert.ToInt32(txtMinMarketMovementDeviation.Text.Trim());
                    SaveUserDetails(objUserDetails);
                }
            }
        }

        private bool IsError(out string ErrorMsg)
        {
            bool IsError = false;
            ErrorMsg = "";
            string MarketMovementDeviation = ConfigurationManager.AppSettings["MarketMovementDeviation"].ToString();
            string vIsProdEnvironment = ConfigurationManager.AppSettings["IsProdEnvironment"].ToString();
            bool bIsProdEnvironment = vIsProdEnvironment == "1" ? true : false; //SR92987 - HASMUKH : For UAT internal KGI user creation user id must start with U and for PROD must start with G

            if (!Regex.IsMatch(txtMinMarketMovementDeviation.Text.Trim(), @"^-?\d{0,9}(\.\d{1,2})?$"))
            {
                IsError = true;
                ErrorMsg = "Please Enter Valid Market Movement, Only 2 digits after decimal are allowed and allowed characters are numbers, single dot(.) and minus(-) symbol";
            }
            else if (Convert.ToDecimal(txtMinMarketMovementDeviation.Text) < Convert.ToDecimal(MarketMovementDeviation) || Convert.ToDecimal(txtMinMarketMovementDeviation.Text) > Convert.ToDecimal(0))
            {
                IsError = true;
                ErrorMsg = "Please Enter Valid Market Movement, It should be between 0 to " + MarketMovementDeviation;
            }
            else if (chkIsExternalUser.Checked && string.IsNullOrEmpty(txtIntermediaryCode.Text.Trim()))
            {
                IsError = true;
                ErrorMsg = "Please select correct intermediary code as you have selected external user";
            }
            else if (chkIsExternalUser.Checked && string.IsNullOrEmpty(hfIntermediaryCode.Value.Trim()))
            {
                IsError = true;
                ErrorMsg = "Please select correct intermediary code as you have selected external user";
            }
            else if (txtIntermediaryCode.Text.Trim() != hfIntermediaryCode.Value.Trim())
            {
                IsError = true;
                ErrorMsg = "Entered intermediary code does not match with selected intermedairy code";
            }
            else if (chkIsExternalUser.Checked == false && txtUserId.Text.StartsWith("G") == false && bIsProdEnvironment)
            {
                IsError = true;
                ErrorMsg = "For Internal User Creation Login Id Must Start With 'G' letter";
            }
            else if (chkIsExternalUser.Checked == false && txtUserId.Text.StartsWith("U") == false && bIsProdEnvironment == false)
            {
                IsError = true;
                ErrorMsg = "For Internal UAT User Creation Login Id Must Start With 'U' letter"; //SR92987 - HASMUKH - For UAT internal KGI user creation user id must start with U and for PROD must start with G
            }
            return IsError;
        }

        protected void BindData()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "Select top 10 a.vUserLoginId,a.vUserLoginDesc,a.vUserLoginId,a.vUserPassword,a.bIsActivate,a.vThemeName, vIntermediaryCode,vIntermediaryBranch from TBL_USER_LOGIN a Order by a.vUserLoginDesc";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet ds = null;

            ds = db.ExecuteDataSet(dbCommand);

            DataTable dtRequisition = new DataTable();

            dtRequisition = ds.Tables[0];

            //Create DataTable

            DataTable dt = new DataTable();

            //Put some columns in it.

            dt.Columns.Add(new DataColumn("vUserLoginId", typeof(string)));
            dt.Columns.Add(new DataColumn("vUserLoginDesc", typeof(string)));
            dt.Columns.Add(new DataColumn("vUserPassword", typeof(string)));
            dt.Columns.Add(new DataColumn("bIsActivate", typeof(string)));

            dt.Columns.Add(new DataColumn("vIntermediaryCode", typeof(string)));
            dt.Columns.Add(new DataColumn("vIntermediaryBranch", typeof(string)));

            // Create the record
            foreach (DataRow row in dtRequisition.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["vUserLoginId"] = row["vUserLoginId"];
                dr["vUserLoginDesc"] = row["vUserLoginDesc"];
                dr["vUserPassword"] = row["vUserPassword"];

                dr["vIntermediaryCode"] = row["vIntermediaryCode"];
                dr["vIntermediaryBranch"] = row["vIntermediaryBranch"];

                if (row["bIsActivate"].ToString() == "Y")
                {
                    dr["bIsActivate"] = "Active";
                }
                else
                {
                    dr["bIsActivate"] = "Deactivated";
                }
                dt.Rows.Add(dr);
            }
            //Session["dt"] = dt;
            gvSubDetails.DataSource = dt;
            gvSubDetails.DataBind();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void btnGetIntermediaryCode_Click(object sender, EventArgs e)
        {
            //string IntermediaryCode = hfIntermediaryCode.Value;
            //lblIntermediaryCode.Text = IntermediaryCode;
            //SetIntermediaryBusinessChaneelType(IntermediaryCode);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetIntermediaryCode(string prefix)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_INTERMEDIARY_CODE_AUTO";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IntrCds.Add(string.Format("{0}~{1}", sdr["TXT_INTERMEDIARY_CD"], sdr["TXT_INTERMEDIARY_NAME"]));
                        }
                    }
                    conn.Close();
                }
            }
            return IntrCds.ToArray();
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IEnumerable<ProjectPASS.UserDetails> UserGridView_GetData(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            int pageSize = maximumRows;
            int pageIndex = 0;
            //int totalCount = 0;
            totalRowCount = GetUser().Count();

            if (startRowIndex > 0)
            {
                pageIndex = (int)Math.Round(((double)startRowIndex / (double)pageSize));
            }


            return GetUser().OrderBy(x => x.vUserLoginDesc).Skip(pageIndex * pageSize).Take(pageSize);
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void UserGridView_UpdateItem(string vUserLoginId)
        {
            UserDetails user = GetUser().Where(x => x.vUserLoginId == vUserLoginId).SingleOrDefault();

            // Load the item here, e.g. item = MyDataLayer.Find(id);
            if (user == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with LoginId {0} was not found", vUserLoginId));
                return;
            }
            TryUpdateModel(user);
            if (ModelState.IsValid)
            {
                user.vUserPassword = "Kotak@123";
                SaveUserDetails(user);
            }
        }

        public IEnumerable<UserDetails> GetUser(string LoginId = null, bool? isActive = null)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                //using (SqlCommand cmd = new SqlCommand())
                //{
                //    cmd.CommandType = CommandType.Text;
                //    cmd.CommandText = "Select a.vUserLoginId,ltrim(a.vUserLoginDesc) as vUserLoginDesc,a.vUserEmailId,a.vUserPassword,a.bIsActivate,a.vThemeName, vIntermediaryCode,vIntermediaryBranch from TBL_USER_LOGIN a Order by 2";
                //    cmd.Connection = conn;
                //    conn.Open();
                //    using (SqlDataReader reader = cmd.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {
                //            yield return this.CreateUser(reader);
                //        }
                //    }
                //    conn.Close();
                //}

                string your_String = txtSearch.Text.Trim();
                string SearchExpression = Regex.Replace(your_String, @"[^0-9a-zA-Z]+", "");

                using (SqlCommand cmd = new SqlCommand("PROC_SEARCHUSERDATA", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchExpression", SearchExpression);
                    cmd.Parameters.AddWithValue("@vUserLoginId", Session["vUserLoginId"].ToString());
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return this.CreateUser(reader);
                        }
                    }
                    conn.Close();
                }



            }
        }

        private UserDetails CreateUser(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("No User detail exist.");
            }

            return new UserDetails
            {
                vUserLoginId = Convert.ToString(reader["vUserLoginId"]),
                vUserLoginDesc = Convert.ToString(reader["vUserLoginDesc"]),
                vUserEmailId = Convert.ToString(reader["vUserEmailId"]),
                vIntermediaryCode = Convert.IsDBNull(reader["vIntermediaryCode"]) ? string.Empty : Convert.ToString(reader["vIntermediaryCode"]),
                vIntermediaryBranch = Convert.IsDBNull(reader["vIntermediaryBranch"]) ? string.Empty : Convert.ToString(reader["vIntermediaryBranch"]),
                bIsActivate = Convert.ToString(reader["bIsActivate"]).ToUpper() == "Y" ? true : false //Convert.ToBoolean(reader["bIsActivate"])
            };
        }

        protected void UserGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            UserGridView.PageIndex = e.NewPageIndex;
        }

        protected void drpDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(drpDept.SelectedValue))
            {
                FillDeptRoleList(drpDept.SelectedValue);
            }
        }

        protected void OboutChkMobileLogin_CheckedChanged(object sender, EventArgs e)
        {
            //if (OboutChkMobileLogin.Checked == true)
            //{

            //    drpUserType.SelectedValue = "BPOS";
            //    drpUserType.Enabled = false;
            //    OboutChkEPOSQuoteView.Checked = false;
            //    OboutChkEPOSQuoteView.Enabled = false;
            //}
            //else
            //{
            //    OboutChkEPOSQuoteView.Enabled = true;
            //    drpUserType.Enabled = true;
            //    drpUserType.SelectedValue = "KGI";
            //}
            return;
        }

        protected void OboutReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmCreateUserMasterNewEdit.aspx");
        }

        protected void OboutBtnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim().IsContainSpecialChars())
            {
                Alert.Show("Please enter valid userid, special characters not allowed");
                return;
            }
            else if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                Alert.Show("Please enter Userid to search details");
                return;
            }
            else
            {
                UserDetailsGridView.DataBind();
            }
        }

        protected void UserDetailsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnSave.Text = "Update";
                int index = UserDetailsGridView.SelectedIndex;
                string vUserLoginId = UserDetailsGridView.DataKeys[index].Value.ToString();
                ViewState["vUserLoginId"] = vUserLoginId;
                HdFldSave.Value = "Update";
                fngetUserData(vUserLoginId);
                txtUsrEmpDesc.Focus();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "gvUSerdata_SelectedIndexChanged()");
                Alert.Show("Some error occured, check error log.");
            }
        }

        private void fngetUserData(string vUserLoginId)
        {
            try
            {

                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString);
                using (SqlCommand cmd = new SqlCommand("PRC_getUSerData", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vUserLoginId", vUserLoginId.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtUsrLoginPass.Attributes["value"] = "Pass@w0rd123";  // This password only for handling the validation part and not used for update.
                            txtUserId.Text = reader["vUserLoginId"].ToString();
                            txtemail.Text = reader["vUserEmailId"].ToString();
                            txtUsrEmpDesc.Text = reader["vUserLoginDesc"].ToString();
                            txtIntermediaryBranch.Text = reader["vIntermediaryBranch"].ToString();
                            hfIntermediaryCode.Value = reader["vIntermediaryCode"].ToString(); // Also store value in hiddenfield to avoid wrong entries
                            txtIntermediaryCode.Text = reader["vIntermediaryCode"].ToString();
                            txtMinMarketMovementDeviation.Text = reader["Min_MarketMovement"].ToString();
                            chkIsExternalUser.Checked = (reader["IsExternalUser"].ToString() == "True") ? true : false;
                            drpDept.SelectedValue = reader["DEPT_CODE"].ToString();
                            FillDeptRoleList(drpDept.SelectedValue);
                            drpRole.SelectedValue = reader["vRoleCode"].ToString();
                            OboutChkMobileLogin.Checked = (reader["IsAllowLoginFromMobile"].ToString() == "True") ? true : false;
                            OboutChkChotuPASSLogin.Checked = (reader["IsAllowLoginFromChotuPASS"].ToString() == "True") ? true : false;
                            drpUserType.SelectedValue = reader["TypeOfUser"].ToString();
                            OboutChkEPOSQuoteView.Checked = (reader["IsAllowEPOSQuoteView"].ToString() == "True") ? true : false;
                            ObouttxtRegDeptHeadEmail.Text = reader["RegionalDeptHeadEmailId"].ToString();
                            OboutChkLocked.Checked = (reader["vLocked"].ToString() == "Y") ? true : false;
                            OboutChkActive.Checked = (reader["bIsActivate"].ToString() == "Y") ? true : false;
                        }
                    }
                    txtUsrLoginPass.Enabled = false;
                    txtUserId.Enabled = false;



                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "fngetUserData");
                Response.Write(ex.ToString());

            }
        }

        public IEnumerable<ProjectPASS.UserDetails> UserDetailsGridView_GetData([Control("txtSearch")] string vUserloginID, int maximumRows, int startRowIndex, out int totalRowCount)
        {
            int pageSize = maximumRows;
            int pageIndex = 0;
            //int totalCount = 0;
            totalRowCount = GetUser().Count();

            if (startRowIndex > 0)
            {
                pageIndex = (int)Math.Round(((double)startRowIndex / (double)pageSize));
            }


            return GetUser().OrderBy(x => x.vUserLoginDesc).Skip(pageIndex * pageSize).Take(pageSize);
        }

    }
}

