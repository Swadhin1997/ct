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

namespace ProjectPASS
{
    public partial class FrmUserRoleMaster : System.Web.UI.Page
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
                BindData();
            }

        }

        ////called on row edit command
        //protected void gvSubDetails_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    gvSubDetails.EditIndex = e.NewEditIndex;
        //    BindData();
        //}


        ////called when cancel edit mode
        //protected void gvSubDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    bool IsUpdated = false;
        //    string vRoleCode = gvSubDetails.DataKeys[e.RowIndex].Value.ToString();
        //    //getting row field details
        //    TextBox txtRoleDescEditGrid = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtRoleDescEditGrid");
           
        //    try
        //    {
        //        //Open the SqlConnection     
        //        Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
        //        string sqlCommand;
        //        sqlCommand = "update TBL_Role_MASTER set vRoleDesc='" + txtRoleDescEditGrid.Text + "',dModifiedDate=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "' where vRoleCode = '" + vRoleCode + "'";
        //        DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
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
        protected void gvSubDetails_RowUpdating(object sender, GridRecordEventArgs e)
        {
            bool IsUpdated = false;
            string vRoleCode = e.Record["vRoleCode"].ToString();
            string vRoleDesc = e.Record["vRoleDesc"].ToString();
            
            try
            {
                //Open the SqlConnection     
                Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
                string sqlCommand;
                sqlCommand = "update TBL_Role_MASTER set vRoleDesc = @vRoleDesc , dModifiedDate = GETDATE(), vModifiedBy = @vModifiedBy where vRoleCode = @vRoleCode";
                DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);

                dbCOMMON.AddParameter(dbCommand, "vRoleDesc", DbType.String, ParameterDirection.Input, "vRoleDesc", DataRowVersion.Current, vRoleDesc);
                dbCOMMON.AddParameter(dbCommand, "vModifiedBy", DbType.String, ParameterDirection.Input, "vModifiedBy", DataRowVersion.Current, Session["vUserLoginId"].ToString());
                dbCOMMON.AddParameter(dbCommand, "vRoleCode", DbType.String, ParameterDirection.Input, "vRoleCode", DataRowVersion.Current, vRoleCode);

                dbCOMMON.ExecuteNonQuery(dbCommand);
                IsUpdated = true;
            }
            catch (Exception ex)
            {
                IsUpdated = false;
            }
            if (IsUpdated)
            {
                lblstatus.Text = "'" + vRoleDesc + "'  details updated successfully!";
                lblstatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblstatus.Text = "Error while updating '" + vRoleDesc + "'  details";
                lblstatus.ForeColor = System.Drawing.Color.Red;
            }
            //gvSubDetails.EditIndex = -1;
            BindData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
            string vRoleCode = "";
            SqlCommand _insertCmd;
            SqlConnection _con = new SqlConnection(db.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();
            try
            {
                {
                    vRoleCode = wsDocNo.fn_Gen_Doc_Master_No("ROLE", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);

                    string lcINSSTR = "INSERT INTO TBL_ROLE_MASTER (vRoleCode,vRoleDesc,vCreatedBy,dCreatedDate)" +
                    " Values(@vRoleCode, @vRoleDesc, @vCreatedBy, @dCreatedDate)";
                    _insertCmd = new SqlCommand(lcINSSTR, _con);

                    _insertCmd.Parameters.AddWithValue("@vRoleCode", vRoleCode);
                    _insertCmd.Parameters.AddWithValue("@vRoleDesc", txtRoleDesc.Text.ToUpper());
                    _insertCmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString());
                    _insertCmd.Parameters.AddWithValue("@dCreatedDate", DateTime.Now);

                    _insertCmd.Transaction = _tran;
                    _insertCmd.ExecuteNonQuery();

                }

                _tran.Commit();
                _con.Close();


                //FillEmptyDataGrid();
                Alert.Show("Role Generated for " + txtRoleDesc.Text + " ,Please Note Role Code : " + vRoleCode, "FrmUserRoleMaster.aspx");
                txtRoleCode.Text = "";
                txtRoleDesc.Text = "";
            }
            catch (Exception ex)
            {
                // log exception
                _tran.Rollback();
                Alert.Show(ex.Message.ToString());
            }

        }


        protected void BindData()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "Select * from TBL_ROLE_MASTER a Order by vRoleDesc";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet ds = null;

            ds = db.ExecuteDataSet(dbCommand);

            DataTable dtRequisition = new DataTable();

            dtRequisition = ds.Tables[0];

            //Create DataTable

            DataTable dt = new DataTable();

            //Put some columns in it.

            dt.Columns.Add(new DataColumn("vRoleCode", typeof(string)));
            dt.Columns.Add(new DataColumn("vRoleDesc", typeof(string)));
          

            // Create the record
            foreach (DataRow row in dtRequisition.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["vRoleCode"] = row["vRoleCode"];
                dr["vRoleDesc"] = row["vRoleDesc"];
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
    }
}

