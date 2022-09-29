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
    public partial class FrmChangePassword : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["vUserLoginId"] == null)
            {
                Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                return;
            }

            if (Session["IsExternalUser"] == null)
            {
                Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                return;
            }

            string cAuthModeAD = ConfigurationManager.AppSettings["cAuthModeAD"].ToString();

            if (Convert.ToBoolean(Session["IsExternalUser"]) == false && cAuthModeAD == "true")
            {
                Alert.Show("KGI Internal users can change the password using windows change password feature.", "FrmMainMenu.aspx");
                return;
            }

            if (!IsPostBack)
            { 
                fn_get_user_detail();
            }

        }

        protected void fn_get_user_detail()
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            string Strselect = "select vUserLoginDesc,vRoleDesc,vRoleCode,vUserLoginId from TBL_USER_ID_TO_ROLE_MAPPING  where  vUserLoginId='" + Session["vUserLoginId"].ToString() + "'";
            DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(Strselect);
            DataSet ds = null;

            ds = dbCOMMON.ExecuteDataSet(dbCommand);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtUserId.Text = ds.Tables[0].Rows[0]["vUserLoginId"].ToString().ToUpper();
                //lblrolename.Text = ds.Tables[0].Rows[0]["vRoleDesc"].ToString().ToUpper();
                Session["vRoleCode"] = ds.Tables[0].Rows[0]["vRoleCode"].ToString();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Validate();
            if (Page.IsValid)
            {
                //check if old password is correct;
                if (GetOldPassword())
                {
                    if (txtOldPassword.Text.Trim() != txtNewPassword.Text.Trim())
                    {


                        Database db = DatabaseFactory.CreateDatabase("cnPASS");
                        Cls_General_Functions wsDocNo = new Cls_General_Functions();
                        string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
                        string vEmpCode = txtUserId.Text;
                        SqlConnection _con = new SqlConnection(db.ConnectionString);
                        _con.Open();
                        SqlTransaction _tran = _con.BeginTransaction();
                        try
                        {
                            {
                                string lcINSSTR = "Update TBL_USER_LOGIN SET vUserPassword = dbo.fnEncDecRc4('Kotak','" + txtNewPassword.Text.Trim() + "') where vUserLoginId='" + Session["vUserLoginId"] + "'";

                                SqlCommand _insertCmd = new SqlCommand(lcINSSTR, _con);
                                _insertCmd.Transaction = _tran;
                                _insertCmd.ExecuteNonQuery();

                            }

                            _tran.Commit();
                            _con.Close();

                            Alert.Show("Password Changed Successfully, Please Login Again", "FrmSecuredLogin.aspx");
                        }
                        catch (Exception ex)
                        {
                            _tran.Rollback();
                            Alert.Show(ex.Message.ToString());
                        }
                    }
                    else
                    {
                        Alert.Show("Please try another password", "FrmChangePassword.aspx");
                    }
                }
                else
                {
                    Alert.Show("Old Password is Incorrect", "FrmChangePassword.aspx");
                }
            }
        }

        private bool GetOldPassword()
        {
            bool IsCorrect = false;

            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_VERIFY_OLD_PASSWORD";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "LOGIN_ID", DbType.String, ParameterDirection.Input, "LOGIN_ID", DataRowVersion.Current, Session["vUserLoginId"]);
            db.AddParameter(dbCommand, "OLD_PASSWORD", DbType.String, ParameterDirection.Input, "OLD_PASSWORD", DataRowVersion.Current, txtOldPassword.Text.Trim());

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsCorrect = ds.Tables[0].Rows[0][0].ToString() == "1" ? true : false;
                }
            }

            return IsCorrect;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        
    }
}

