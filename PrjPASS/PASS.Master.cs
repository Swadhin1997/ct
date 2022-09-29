using System;
using System.Collections;
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
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Text;
  
namespace ProjectPASS
{
    public partial class PASS : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Added for VAPT Session Hijack fix

            if (Session["vUserLoginId"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (Session["vUserLoginId"].ToString().ToUpper() != "EMP00001")
                {

                    if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                    {
                        // redirect to the login page in real application
                        Alert.Show("You are not logged in.", "FrmSecuredLogin.aspx");
                        return;
                    }
                }
            }
            else
            {
                if (Request.Path.ToLower() != "/kgipass/frmforgotpassword.aspx" && Request.Path.ToLower() != "/kgipass/frmresetpassword.aspx")
                {
                    Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    return;
                }
            }

            // Added for VAPT Session Hijack fix Ended Here




            //if (Session["vUserLoginId"] == null && Request.Path.ToLower() != "/frmforgotpassword.aspx")

            if(Request.Path.ToLower() == "/frmreviewconfirm.aspx" || Request.Path.ToLower() == "/frmpaymentconfirmation.aspx" || Request.Path.ToLower() == "/frmerrorpage.aspx" || Request.Path.ToLower() == "/frmcustomerrorpage.aspx")
            {

            }
            else if (Session["vUserLoginId"] == null && Request.Path.ToLower() != "/kgipass/frmforgotpassword.aspx" && Request.Path.ToLower() != "/kgipass/frmresetpassword.aspx")
            {
                Alert.Show("Session Invalid,Please Login Again", "FrmSecuredLogin.aspx");
                return;
            }
            else
            {
                //RadSkinManager1.Skin = Session["vThemeName"].ToString();
            }
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["vTittle"]))
                {
                    //lblOptionTittle.Text = Request.QueryString["vTittle"].ToString();
                    Session["vTittle"] = Request.QueryString["vTittle"].ToString();
                }
                else
                {
                    //lblOptionTittle.Text = Session["vTittle"].ToString();
                }
                //if (Session["dsParentMenu"] != null)
                //{
                //    DataSet ds = (DataSet)Session["dsParentMenu"];
                //    BuildRibbonMenu(RdToolBarRequisition, ds.Tables[0]);
                //}
                //else
                //{
                //    Database db = DatabaseFactory.CreateDatabase("cnIMS");
                //    DataSet ds = null;
                //    System.Data.Common.DbCommand dbCommandWrapper = db.GetSqlStringCommand("Select B.* from TBL_USER_TO_FAVOURITE_MENU_MAPPING A,TBL_DMENU_MMS_TABLE B  where A.vEmployeeCode = '" + Session["vEmployeeCode"] + "' and A.nOptionID = B.nOptionID");
                //    ds = db.ExecuteDataSet(dbCommandWrapper);
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        Session["dsParentMenu"] = ds;
                //        BuildRibbonMenu(RdToolBarRequisition, ds.Tables[0]);
                //    }
                //}
            }

            /////SR90624  START - HASMUKH - 17-MAY-2021

            if (Session["IsBancaUser"] != null)
            {
                bool IsBancaUser = Convert.ToBoolean(Session["IsBancaUser"]);

                if (IsBancaUser)
                {
                    string currentPageName = Request.Url.Segments[Request.Url.Segments.Length - 1].ToLower();
                    string DisablePagesForBancaUsers = ConfigurationManager.AppSettings["DisablePagesForBancaUsers"].ToString();
                    if (DisablePagesForBancaUsers.Contains(currentPageName))
                    {
                        Response.Redirect("FrmMainMenu.aspx");
                    }
                }
                
            }
            else
            {
                Session.Abandon();
                Response.Redirect("FrmSecuredLogin.aspx");
            }

            /////SR90624  END - HASMUKH - 17-MAY-2021
        }

        //protected void RdToolBar_ButtonClick(object sender, RadToolBarEventArgs e)
        //{
        //    if (e.Item.Index == 4)
        //    {
        //        Response.Redirect("FrmMainMenu.aspx");
        //    }
        //}
        //protected void BuildRibbonMenu(RadMenu menu, DataTable dt)
        //{
        //    Database db = DatabaseFactory.CreateDatabase("cnIMS");
        //    // Get DataView
        //    DataView dataView = new DataView(dt);
        //    dataView.Sort = "nOptionID";
        //    try
        //    {
        //        for (int i = 0; i < dataView.Count; i++)
        //        {
        //            DataRow row = dataView[i].Row;

        //            //if (row["cParentFlag"].ToString() == "Y")
        //            //{
        //                RadMenuItem RadMenuItemTmp = CreateRadMenuItem(row);
        //                menu.Items.Add(RadMenuItemTmp);
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Alert.Show(ex.Message);
        //        return;
        //    }
        //}
        //private RadMenuItem CreateRadMenuItem(DataRow row)
        //{
        //    RadMenuItem ret = new RadMenuItem();
        //    ret.Text = row["vOptionName"].ToString();
        //    ret.NavigateUrl = row["NavigateUrl"].ToString();
        //    return ret;
        //}        
    }
}
