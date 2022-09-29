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
using Obout.ComboBox;

namespace ProjectPASS
{
    public partial class FrmUserToRoleMapping : System.Web.UI.Page
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
                FillDrpUserList();
                FillDrpUserRoleList();
                BindData();
            }

        }
        protected void FillDrpUserList()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand = string.Empty;

            //if (Session["vUserLoginId"].ToString().ToUpper() == "EMP00001")
            //{
            //    //get all the user created by EMP00001
            //    sqlCommand = "select vUserLoginId,vUserLoginDesc from TBL_USER_LOGIN  order by vUserLoginDesc";
            //}
            //else
            //{
            //    //get only the user created by the logged in user
            //    sqlCommand = "select vUserLoginId,vUserLoginDesc from TBL_USER_LOGIN WHERE vCreatedBy = '" + Session["vUserLoginId"].ToString().ToUpper() + "' order by vUserLoginDesc";
            //}

            sqlCommand = "select vUserLoginId,vUserLoginDesc from TBL_USER_LOGIN  order by vUserLoginDesc";

            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet dsUser = null;
            dsUser = db.ExecuteDataSet(dbCommand);
            drpUserList.DataValueField = "vUserLoginId";
            drpUserList.DataTextField = "vUserLoginDesc";
            drpUserList.DataSource = dsUser.Tables[0];
            drpUserList.DataBind();

            if (dsUser.Tables[0].Rows.Count > 0)
            {
                ComboBoxItem l_lstItem = new ComboBoxItem("Select", "ALL");
                drpUserList.Items.Insert(0, "Select");
            }
            else
            {
                ComboBoxItem l_lstItem = new ComboBoxItem("No-User Defined in Master", "No");
                drpUserList.Items.Insert(0, "No-User Defined in Master");

            }
        }
        protected void gvSubDetails_RowUpdating(object sender, GridRecordEventArgs e)
        {
        }
        protected void FillDrpUserRoleList()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = string.Empty;

            if (Session["vUserLoginId"].ToString().ToUpper() == "EMP00001")
            {
                sqlCommand = "select vRoleCode,vRoleDesc from TBL_ROLE_MASTER  order by vRoleDesc";
            }
            else
            {
                sqlCommand = "SELECT A.VROLECODE, B.VROLEDESC FROM TBL_DEPT_ROLE_MAPPING A INNER JOIN TBL_ROLE_MASTER B ON A.VROLECODE = B.vRoleCode INNER JOIN TBL_USER_DEPT_MAPPING C ON C.DEPT_CODE = A.DEPT_CODE WHERE C.vUserLoginId = '" + Session["vUserLoginId"].ToString().ToUpper() + "'";
            }


            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet dsRole = null;
            dsRole = db.ExecuteDataSet(dbCommand);
            drpUserRoleList.DataValueField = "vRoleCode";
            drpUserRoleList.DataTextField = "vRoleDesc";
            drpUserRoleList.DataSource = dsRole.Tables[0];
            drpUserRoleList.DataBind();

            if (dsRole.Tables[0].Rows.Count > 0)
            {
                ComboBoxItem l_lstItem = new ComboBoxItem("Select", "ALL");
                drpUserRoleList.Items.Insert(0, l_lstItem);
            }
            else
            {
                ComboBoxItem l_lstItem = new ComboBoxItem("No-Role Defined in Master", "No");
                drpUserRoleList.Items.Insert(0, l_lstItem);

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
            SqlConnection _con = new SqlConnection(db.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();
            try
            {
                {

                    String lcINSSTR = "DELETE FROM TBL_USER_ID_TO_ROLE_MAPPING where vUserLoginId =  @vUserLoginId and vRoleCode = @vRoleCode";

                    SqlCommand _insertCmd = new SqlCommand(lcINSSTR, _con);

                    _insertCmd.Parameters.AddWithValue("@vUserLoginId", drpUserList.SelectedValue);
                    _insertCmd.Parameters.AddWithValue("@vRoleCode", drpUserRoleList.SelectedValue);

                    _insertCmd.Transaction = _tran;
                    _insertCmd.ExecuteNonQuery();

                    _insertCmd.Parameters.Clear();

                    lcINSSTR = "Insert into TBL_USER_ID_TO_ROLE_MAPPING (vUserLoginId,vUserLoginDesc,vRoleCode,vRoleDesc,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate) values " +
                        "(@vUserLoginId,@vUserLoginDesc,@vRoleCode,@vRoleDesc,@vCreatedBy,@vModifiedBy,@dCreatedDate,@dModifiedDate)";

                    _insertCmd = new SqlCommand(lcINSSTR, _con);

                    _insertCmd.Parameters.AddWithValue("@vUserLoginId", drpUserList.SelectedValue);
                    _insertCmd.Parameters.AddWithValue("@vUserLoginDesc", drpUserList.SelectedItem.Text);
                    _insertCmd.Parameters.AddWithValue("@vRoleCode", drpUserRoleList.SelectedValue);
                    _insertCmd.Parameters.AddWithValue("@vRoleDesc", drpUserRoleList.SelectedText);
                    _insertCmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString());
                    _insertCmd.Parameters.AddWithValue("@vModifiedBy", Session["vUserLoginId"].ToString());
                    _insertCmd.Parameters.AddWithValue("@dCreatedDate", DateTime.Now);
                    _insertCmd.Parameters.AddWithValue("@dModifiedDate", DateTime.Now);

                    _insertCmd.Transaction = _tran;
                    _insertCmd.ExecuteNonQuery();

                }

                _tran.Commit();
                _con.Close();

                Alert.Show("User " + drpUserList.SelectedItem.Text + " ,Mapped to Role : " + drpUserRoleList.SelectedText, "FrmUserToRoleMapping.aspx");
                drpUserList.SelectedIndex = 0;
                drpUserRoleList.SelectedIndex = 0;

                BindData();
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

            string sqlCommand = "Select a.vUserLoginId,a.vUserLoginDesc,a.vRoleCode,a.vRoleDesc from TBL_USER_ID_TO_ROLE_MAPPING a Order by a.vRoleDesc";
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
            dt.Columns.Add(new DataColumn("vRoleCode", typeof(string)));
            dt.Columns.Add(new DataColumn("vRoleDesc", typeof(string)));

            // Create the record
            foreach (DataRow row in dtRequisition.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["vUserLoginId"] = row["vUserLoginId"];
                dr["vUserLoginDesc"] = row["vUserLoginDesc"];
                dr["vRoleCode"] = row["vRoleCode"];
                dr["vRoleDesc"] = row["vRoleDesc"];
                dt.Rows.Add(dr);
            }
            gvSubDetails.DataSource = dt;
            gvSubDetails.DataBind();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}

