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
using Obout.ListBox;
using Obout.ComboBox;

namespace ProjectPASS
{
    public partial class FrmUserRightsToRoleMapping : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        ArrayList arraylist1 = new ArrayList();
        ArrayList arraylist2 = new ArrayList();

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
                FillDrpModuleList();
                FillDrpUserRoleList();
            }
        }

        protected void ClearAfterSaveUpdate()
        {
            drpUserRoleList.Enabled = true;
            drpUserRoleList.SelectedIndex = 0;
            lstMenuRole.Items.Clear();
            lstMenuMaster.Items.Clear();
        }

        protected void FilldrpMenuMaster(string vRoleCode, string vModule)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand = "select A.* from TBL_DMENU_PASS_TABLE A where A.nOptionID NOT IN (Select nOptionID From TBL_USER_ROLE_TO_RIGHTS_MAPPING Where  vRoleCode = @vRoleCode) and nParentID = @vModule order by A.vOptionName";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

            db.AddParameter(dbCommand, "vRoleCode", DbType.String, ParameterDirection.Input, "vRoleCode", DataRowVersion.Current, vRoleCode);
            db.AddParameter(dbCommand, "vModule", DbType.String, ParameterDirection.Input, "vModule", DataRowVersion.Current, vModule);

            DataSet dsMenuMaster = null;
            dsMenuMaster = db.ExecuteDataSet(dbCommand);
            lstMenuMaster.DataValueField = "nOptionID";
            lstMenuMaster.DataTextField = "vOptionName";
            lstMenuMaster.DataSource = dsMenuMaster.Tables[0];
            lstMenuMaster.DataBind();

            sqlCommand = "Select * From TBL_USER_ROLE_TO_RIGHTS_MAPPING Where vRoleCode = @vRoleCode";
            dbCommand = db.GetSqlStringCommand(sqlCommand);

            db.AddParameter(dbCommand, "vRoleCode", DbType.String, ParameterDirection.Input, "vRoleCode", DataRowVersion.Current, drpUserRoleList.SelectedValue);

            DataSet dsMenuRights = null;
            dsMenuRights = db.ExecuteDataSet(dbCommand);

            if (dsMenuRights.Tables[0].Rows.Count > 0)
            {
                lstMenuRole.DataValueField = "nOptionID";
                lstMenuRole.DataTextField = "vOptionName";
                lstMenuRole.DataSource = dsMenuRights.Tables[0];
                lstMenuRole.DataBind();
            }
            drpUserRoleList.Enabled = false;
        }

        protected void FillDrpModuleList()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "select nOptionID,vOptionName from TBL_DMENU_PASS_TABLE where nGroupId=0";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet dsModule = null;
            dsModule = db.ExecuteDataSet(dbCommand);
            drpModule.DataValueField = "nOptionID";
            drpModule.DataTextField = "vOptionName";
            drpModule.DataSource = dsModule.Tables[0];
            drpModule.DataBind();
            if (dsModule.Tables[0].Rows.Count > 0)
            {
                ComboBoxItem l_lstItem = new ComboBoxItem("Select", "ALL");
                drpModule.Items.Insert(0, l_lstItem);
            }
            else
            {
                ComboBoxItem l_lstItem = new ComboBoxItem("No-Modules Defined in Master", "No");
                drpModule.Items.Insert(0, l_lstItem);

            }
        }

        protected void FillDrpUserRoleList()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "select vRoleCode,vRoleDesc from TBL_ROLE_MASTER  order by vRoleDesc";
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
            string StrSqlInsCommand, StrSqlDelCommand;
            string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
            SqlCommand _insertCmd;
            SqlConnection _con = new SqlConnection(db.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();
            try
            {
                StrSqlDelCommand = "Delete from TBL_USER_ROLE_TO_RIGHTS_MAPPING Where vRoleCode = @vRoleCode";
                _insertCmd = new SqlCommand(StrSqlDelCommand, _con);

                _insertCmd.Parameters.AddWithValue("@vRoleCode", drpUserRoleList.SelectedValue);

                _insertCmd.Transaction = _tran;
                _insertCmd.ExecuteNonQuery();

                foreach (System.Web.UI.WebControls.ListItem item in lstMenuRole.Items)
                {
                    StrSqlInsCommand = "INSERT INTO TBL_USER_ROLE_TO_RIGHTS_MAPPING(nOptionID,vOptionName,vRoleCode,vRoleDesc,vCreatedBy,dCreatedDate) Values " +
                    " (@nOptionID,@vOptionName,@vRoleCode,@vRoleDesc,@vCreatedBy,@dCreatedDate)";
                    _insertCmd = new SqlCommand(StrSqlInsCommand, _con);

                    _insertCmd.Parameters.Clear();

                    _insertCmd.Parameters.AddWithValue("@nOptionID", item.Value);
                    _insertCmd.Parameters.AddWithValue("@vOptionName", item.Text);
                    _insertCmd.Parameters.AddWithValue("@vRoleCode", drpUserRoleList.SelectedValue);
                    _insertCmd.Parameters.AddWithValue("@vRoleDesc", drpUserRoleList.SelectedText);
                    _insertCmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"]);
                    _insertCmd.Parameters.AddWithValue("@dCreatedDate", DateTime.Now);

                    _insertCmd.Transaction = _tran;
                    _insertCmd.ExecuteNonQuery();

                }
                _tran.Commit();
                ClearAfterSaveUpdate();
                Alert.Show("Record Saved Successfully");
            }
            catch (Exception ex)
            {
                _tran.Rollback();
                Alert.Show(ex.Message);
            }
            lstMenuRole.Items.Clear();
            lstMenuMaster.Items.Clear();
            FilldrpMenuMaster(drpUserRoleList.SelectedValue, drpModule.SelectedValue);

        }
        protected void btn1_Click(object sender, EventArgs e)
        {
            //lbltxt.Visible = false;
            if (lstMenuMaster.SelectedIndex >= -1)
            {
                for (int i = 0; i < lstMenuMaster.Items.Count; i++)
                {
                    if (lstMenuMaster.Items[i].Selected)
                    {
                        if (!arraylist1.Contains(lstMenuMaster.Items[i]))
                        {
                            arraylist1.Add(lstMenuMaster.Items[i]);
                        }
                    }
                }
                for (int i = 0; i < arraylist1.Count; i++)
                {
                    if (!lstMenuRole.Items.Contains(((ListItem)arraylist1[i])))
                    {

                        lstMenuRole.Items.Add(((ListItem)arraylist1[i]));

                    }
                    lstMenuMaster.Items.Remove(((ListItem)arraylist1[i]));
                }
                lstMenuRole.SelectedIndex = -1;
            }
            else
            {
                //lbltxt.Visible = true;
                Alert.Show("Please select atleast one in Listbox1 to move");
            }
        }
        protected void btn2_Click(object sender, EventArgs e)
        {
            //lbltxt.Visible = false;
            while (lstMenuMaster.Items.Count != 0)
            {
                for (int i = 0; i < lstMenuMaster.Items.Count; i++)
                {
                    lstMenuRole.Items.Add(lstMenuMaster.Items[i]);
                    lstMenuMaster.Items.Remove(lstMenuMaster.Items[i]);
                }
            }
        }
        protected void btn3_Click(object sender, EventArgs e)
        {
            //lbltxt.Visible = false;
            if (lstMenuRole.SelectedIndex >= -1)
            {
                for (int i = 0; i < lstMenuRole.Items.Count; i++)
                {
                    if (lstMenuRole.Items[i].Selected)
                    {
                        if (!arraylist2.Contains(lstMenuRole.Items[i]))
                        {
                            arraylist2.Add(lstMenuRole.Items[i]);
                        }
                    }
                }
                for (int i = 0; i < arraylist2.Count; i++)
                {
                    if (!lstMenuMaster.Items.Contains(((ListItem)arraylist2[i])))
                    {
                        lstMenuMaster.Items.Add(((ListItem)arraylist2[i]));
                    }
                    lstMenuRole.Items.Remove(((ListItem)arraylist2[i]));
                }
                lstMenuMaster.SelectedIndex = -1;
            }
            else
            {
                //lbltxt.Visible = true;
                Alert.Show("Please select atleast one in Listbox2 to move");
            }
        }
        protected void btn4_Click(object sender, EventArgs e)
        {
            //lbltxt.Visible = false;
            while (lstMenuRole.Items.Count != 0)
            {
                for (int i = 0; i < lstMenuRole.Items.Count; i++)
                {
                    lstMenuMaster.Items.Add(lstMenuRole.Items[i]);
                    lstMenuRole.Items.Remove(lstMenuRole.Items[i]);
                }
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAfterSaveUpdate();
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void drpModule_SelectedIndexChanged1(object sender, ComboBoxItemEventArgs e)
        {
            if (drpUserRoleList.SelectedValue == "ALL")
            {
                Alert.Show("Please Select Role.");
                drpModule.SelectedValue = "ALL";
                return;
            }

            FilldrpMenuMaster(drpUserRoleList.SelectedValue, drpModule.SelectedValue);
        }
    }
}

