using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;

namespace ProjectPASS
{
    public partial class FrmMainMenu : System.Web.UI.Page
    {
        public Cls_General_Functions wsCHKLOGIN = new Cls_General_Functions();
        public int Count;
        public DataSet ds = null;
        public string CreateString = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Session["FirstMenuXml"] != null)
            {
                //rdRibMenu.LoadXml(Session["FirstMenuXml"].ToString());
            }
            else
            {
                addTab();
            }
        }
        private void addTab()
        {
            if (Session["dsParent"] != null)
            {
                DataSet ds = (DataSet)Session["dsParent"];
                //BuildRibbonMenu(rdRibMenu, ds.Tables[0]);
                BuildNormalMenu(rdRibMenu, ds.Tables[0]);
            }
            else
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                System.Data.Common.DbCommand dbCommandWrapper = db.GetSqlStringCommand("Select * from TBL_DMENU_PASS_TABLE where cParentFlag='Y'");
                ds = db.ExecuteDataSet(dbCommandWrapper);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["dsParent"] = ds;
                    //BuildRibbonMenu(rdRibMenu, ds.Tables[0]);
                    BuildNormalMenu(rdRibMenu, ds.Tables[0]);
                }
            }
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //fn_check_first_login();
                fn_get_user_detail();
            }
        }

    
        protected void fn_get_user_detail()
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            string Strselect = "select vUserLoginDesc,vRoleDesc,vRoleCode from TBL_USER_ID_TO_ROLE_MAPPING  where  vUserLoginId='" + Session["vUserLoginId"].ToString() + "'";
            DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(Strselect);
            DataSet ds = null;

            ds = dbCOMMON.ExecuteDataSet(dbCommand);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblusername.Text = ds.Tables[0].Rows[0]["vUserLoginDesc"].ToString().ToUpper();
                lblrolename.Text = ds.Tables[0].Rows[0]["vRoleDesc"].ToString().ToUpper();
                Session["vRoleCode"] = ds.Tables[0].Rows[0]["vRoleCode"].ToString();
            }
        }
        protected void BuildNormalMenu(Menu menu, DataTable dt)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            DbCommand dbCommand;
            string strSqlCommand;
            // Get DataView
            DataView dataView = new DataView(dt);
            dataView.Sort = "nParentID";
            try
            {
                for (int i = 0; i < dataView.Count; i++)
                {
                    DataRow row = dataView[i].Row;

                    if (row["cParentFlag"].ToString() == "Y")
                    {
                        MenuItem RibbonBarTabTmp = CreateRibbonBarTab(row);
                        RibbonBarTabTmp.Selectable = false;
                        //if (row["vAccessKey"].ToString().Trim() != "0")
                        //{
                        //    RibbonBarTabTmp.AccessKey = row["vAccessKey"].ToString().Trim();
                        //}
                        menu.Items.Add(RibbonBarTabTmp);

                        strSqlCommand = "Select nGroupID,vGroupName,max(ImageUrl) ImageUrl,vAccessKey vAccessKey from  TBL_DMENU_PASS_TABLE where  nParentID = '" + row["nOptionID"] + "' group by nGroupID,vGroupName,vAccessKey";
                        DataSet dsGroup = null;
                        dbCommand = db.GetSqlStringCommand(strSqlCommand);
                        dsGroup = db.ExecuteDataSet(dbCommand);
                        foreach (DataRow rowgroup in dsGroup.Tables[0].Rows)
                        {
                            MenuItem RibbonBarGroupTmp = new MenuItem();
                            //RibbonBarGroupTmp.Attributes["ExpandDirection"] = "Down";
                            //RadMenuItem RibbonBarMenuTmp = new RadMenuItem();
                            //RibbonBarMenuTmp.Width = Unit.Pixel(150);
                            //RibbonBarMenuTmp.Size = RibbonBarItemSize.Large;
                            //if (rowgroup["vAccessKey"].ToString().Trim() != "0")
                            // {
                            //RibbonBarMenuTmp.AccessKey = rowgroup["vAccessKey"].ToString().Trim();
                            //}
                            // RibbonBarMenuTmp.ImageUrlLarge = rowgroup["ImageUrl"].ToString();
                            RibbonBarGroupTmp.Text = rowgroup["vGroupName"].ToString();
                            RibbonBarGroupTmp.Selectable = false;
                            //RibbonBarTabTmp.Groups.Add(RibbonBarGroupTmp);//Tab to Group Link
                            RibbonBarTabTmp.ChildItems.Add(RibbonBarGroupTmp);//Group to Menu Link                                                    

                            strSqlCommand = "Select nOptionID,vOptionName,vMenuAccessKey,NavigateUrl,vMenuImageUrl from  TBL_DMENU_PASS_TABLE where  nParentID = '" + row["nOptionID"] + "' and nGroupID = '" + rowgroup["nGroupID"] + "'";
                            DataSet dsOption = null;
                            dbCommand = db.GetSqlStringCommand(strSqlCommand);
                            dsOption = db.ExecuteDataSet(dbCommand);
                            foreach (DataRow rowOption in dsOption.Tables[0].Rows)
                            {
                                MenuItem RibbonBarMenuItemTmp = new MenuItem();

                                //Check for Rights
                                DataSet dsRights = null;
                                dbCommand = db.GetSqlStringCommand("Select * from TBL_USER_ROLE_TO_RIGHTS_MAPPING where vRoleCode = '" + Session["vRoleCode"] + "' and nOptionID = '" + rowOption["nOptionID"] + "' ");
                                dsRights = db.ExecuteDataSet(dbCommand);
                                if (Session["vUserLoginId"] != null)
                                {
                                    if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
                                    {
                                        if (dsRights.Tables[0].Rows.Count > 0)
                                        {
                                            RibbonBarMenuItemTmp.Enabled = true;
                                        }
                                        else
                                        {
                                            RibbonBarMenuItemTmp.Enabled = false;
                                        }
                                    }
                                    else
                                    {
                                        RibbonBarMenuItemTmp.Enabled = true;
                                    }
                                }
                                else
                                {
                                    Session.Abandon();
                                    Response.Redirect("FrmSecuredLogin.aspx");
                                }
                                //End Check for Rights

                                //if (rowOption["vMenuAccessKey"].ToString().Trim() != "0")
                                //{
                                //    RibbonBarMenuItemTmp.AccessKey = rowOption["vMenuAccessKey"].ToString().Trim();
                                //}
                                RibbonBarMenuItemTmp.Text = rowOption["vOptionName"].ToString();
                                //RibbonBarMenuItemTmp.Value = rowOption["NavigateUrl"].ToString();
                                RibbonBarMenuItemTmp.NavigateUrl = rowOption["NavigateUrl"].ToString();
                                //RibbonBarMenuItemTmp.ImageUrl = rowOption["vMenuImageUrl"].ToString();

                                /////SR90624  START - HASMUKH - 17-MAY-2021

                                if (Session["IsBancaUser"] != null)
                                {
                                    bool IsBancaUser = Convert.ToBoolean(Session["IsBancaUser"]);

                                    if (IsBancaUser)
                                    {
                                        string NavigateUrl = rowOption["NavigateUrl"].ToString().ToLower();
                                        string DisablePagesForBancaUsers = ConfigurationManager.AppSettings["DisablePagesForBancaUsers"].ToString();

                                        if (DisablePagesForBancaUsers.Contains(NavigateUrl) == false)
                                        {
                                            RibbonBarGroupTmp.ChildItems.Add(RibbonBarMenuItemTmp);// Menu to MenuItem Link
                                        }

                                        //if (rowOption["NavigateUrl"].ToString().ToLower() != "frmpremiumcalculatormotor.aspx" && rowOption["NavigateUrl"].ToString().ToLower() != "frmfinalizequotations.aspx" && rowOption["NavigateUrl"].ToString().ToLower() != "frmquotereassignment.aspx")
                                        //{
                                        //    RibbonBarGroupTmp.ChildItems.Add(RibbonBarMenuItemTmp);// Menu to MenuItem Link
                                        //}
                                    }
                                    else
                                    {
                                        RibbonBarGroupTmp.ChildItems.Add(RibbonBarMenuItemTmp);// Menu to MenuItem Link
                                    }
                                }
                                else
                                {
                                    Session.Abandon();
                                    Response.Redirect("FrmSecuredLogin.aspx");
                                }

                                /////SR90624  END - HASMUKH - 17-MAY-2021
                            }
                        }
                    }
                }
                //Session["FirstMenuXml"] = menu.GetXml();
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
                return;
            }
        }
        //protected void BuildRibbonMenu(Menu menu, DataTable dt)
        //{
        //    Database db = DatabaseFactory.CreateDatabase("cnPASS");
        //    DbCommand dbCommand;
        //    string strSqlCommand;
        //    // Get DataView
        //    DataView dataView = new DataView(dt);
        //    dataView.Sort = "nParentID";
        //    try
        //    {
        //        for (int i = 0; i < dataView.Count; i++)
        //        {
        //            DataRow row = dataView[i].Row;

        //            if (row["cParentFlag"].ToString() == "Y")
        //            {
        //                MenuItem RibbonBarTabTmp = CreateRibbonBarTab(row);
                        
        //                menu.Items.Add(RibbonBarTabTmp);

        //                strSqlCommand = "Select nGroupID,vGroupName,max(ImageUrl) ImageUrl,vAccessKey vAccessKey from  TBL_DMENU_PASS_TABLE where  nParentID = '" + row["nOptionID"] + "' group by nGroupID,vGroupName,vAccessKey";
        //                DataSet dsGroup = null;
        //                dbCommand = db.GetSqlStringCommand(strSqlCommand);
        //                dsGroup = db.ExecuteDataSet(dbCommand);
        //                foreach (DataRow rowgroup in dsGroup.Tables[0].Rows)
        //                {
        //                    MenuItem RibbonBarGroupTmp = new MenuItem();
        //                    //RibbonBarGroupTmp.Attributes["ExpandDirection"] = "Down";
        //                    //RibbonBarMenu RibbonBarMenuTmp = new RibbonBarMenu();
        //                    //RibbonBarMenuTmp.Width = Unit.Pixel(150);
        //                    //RibbonBarMenuTmp.Size = RibbonBarItemSize.Large;
        //                    //if (rowgroup["vAccessKey"].ToString().Trim() != "0")
        //                    //{
        //                    //    RibbonBarMenuTmp.AccessKey = rowgroup["vAccessKey"].ToString().Trim();
        //                    //}
        //                    //RibbonBarMenuTmp.ImageUrlLarge = rowgroup["ImageUrl"].ToString();
        //                    RibbonBarGroupTmp.Text = rowgroup["vGroupName"].ToString();
        //                    RibbonBarTabTmp.ChildItems.Add(RibbonBarGroupTmp);//Tab to Group Link
        //                    //RibbonBarGroupTmp.Items.Add(RibbonBarMenuTmp);//Group to Menu Link                                                    

        //                    strSqlCommand = "Select nOptionID,vOptionName,vMenuAccessKey,NavigateUrl,vMenuImageUrl from  TBL_DMENU_PASS_TABLE where  nParentID = '" + row["nOptionID"] + "' and nGroupID = '" + rowgroup["nGroupID"] + "'";
        //                    DataSet dsOption = null;
        //                    dbCommand = db.GetSqlStringCommand(strSqlCommand);
        //                    dsOption = db.ExecuteDataSet(dbCommand);
        //                    foreach (DataRow rowOption in dsOption.Tables[0].Rows)
        //                    {
        //                        MenuItem RibbonBarMenuItemTmp = new MenuItem();

        //                        //Check for Rights
        //                        DataSet dsRights = null;
        //                        dbCommand = db.GetSqlStringCommand("Select * from TBL_RIGHTS_TO_ROLE_MAPPING where vRoleCode = '" + Session["vRoleCode"] + "' and nOptionID = '" + rowOption["nOptionID"] + "' and vPropertyCode = '" + Session["vPropertyCode"] + "'");
        //                        dsRights = db.ExecuteDataSet(dbCommand);
        //                        if (Session["vEmployeeCode"] != null)
        //                        {
        //                            if (Session["vEmployeeCode"].ToString().ToUpper() != "EMP00001")
        //                            {
        //                                if (dsRights.Tables[0].Rows.Count > 0)
        //                                {
        //                                    RibbonBarMenuItemTmp.Enabled = true;
        //                                }
        //                                else
        //                                {
        //                                    RibbonBarMenuItemTmp.Enabled = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                RibbonBarMenuItemTmp.Enabled = true;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Session.Abandon();
        //                            Response.Redirect("FrmIMSLogin.aspx");
        //                        }
        //                        //End Check for Rights

        //                        //if (rowOption["vMenuAccessKey"].ToString().Trim() != "0")
        //                        //{
        //                        //    RibbonBarMenuItemTmp.AccessKey = rowOption["vMenuAccessKey"].ToString().Trim();
        //                        //}
        //                        RibbonBarMenuItemTmp.Text = rowOption["vOptionName"].ToString();
        //                        RibbonBarMenuItemTmp.Value = rowOption["NavigateUrl"].ToString();
        //                        RibbonBarMenuItemTmp.NavigateUrl = rowOption["NavigateUrl"].ToString();
        //                        RibbonBarMenuItemTmp.ImageUrl = rowOption["vMenuImageUrl"].ToString();
        //                        RibbonBarGroupTmp.ChildItems.Add(RibbonBarMenuItemTmp);// Menu to MenuItem Link
        //                    }
        //                }
        //            }
        //        }
        //        //Session["FirstMenuXml"] = menu.GetXml();
        //    }
        //    catch (Exception ex)
        //    {
        //        Alert.Show(ex.Message);
        //        return;
        //    }
        //}

        private MenuItem CreateRibbonBarTab(DataRow row)
        {
            MenuItem ret = new MenuItem();
            ret.Text = row["vOptionName"].ToString();
            return ret;
        }
        protected void fn_check_first_login()
        {
            string StrChkLogin = "";
            string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString();string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
            String StrUpdateHead="",vCurrentCulture="";
            StrChkLogin=wsCHKLOGIN.fn_check_first_login(Session["vEmployeeCode"].ToString()).ToString();
            if (StrChkLogin == "N")
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection _con = new SqlConnection(db.ConnectionString))
                {
                    _con.Open();
                    using (SqlTransaction _tran = _con.BeginTransaction())
                    {
                        try
                        {
                            vCurrentCulture = AppDomain.CurrentDomain.FriendlyName.ToString()+" "+ System.Globalization.CultureInfo.CurrentCulture.Name;
                            StrUpdateHead = "UPDATE TBL_USER_LOGIN SET dLastLogin=convert(datetime,'" + DateTime.Now.ToString(cCultureFormat) + "'," + cDateFormat + ") WHERE vEmployeeCode='" + Session["vEmployeeCode"].ToString() + "'";
                            SqlCommand _updateCmd = new SqlCommand(StrUpdateHead, _con);
                            _updateCmd.Transaction = _tran;
                            _updateCmd.ExecuteNonQuery();
                            _tran.Commit();
                            _con.Close();
                        }
                        catch (Exception ex)
                        {
                            // log exception
                            _tran.Rollback();
                            using (SqlTransaction _tran1 = _con.BeginTransaction())
                            {
                            String StrInsertHead = "insert into Error_Log_Query values('" + StrUpdateHead.Replace("'", "") + "','" + ex.Message + " " + vCurrentCulture + "')";
                            SqlCommand _updateCmd = new SqlCommand(StrInsertHead, _con);
                            _updateCmd.Transaction = _tran1;
                            _updateCmd.ExecuteNonQuery();
                            _tran1.Commit();
                            _con.Close();
                            }
                            Alert.Show(ex.Message.ToString());
                        }
                    }
                }
            }
            else
            {
                Alert.Show("This Is Your First Login, For Security Reasons Please Change Your Password.");
                Response.Redirect("FrmChangeUserPassword.aspx?vTittle=Change Password&vFirst=Y");
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmLogout.aspx");
        }
    }
}
