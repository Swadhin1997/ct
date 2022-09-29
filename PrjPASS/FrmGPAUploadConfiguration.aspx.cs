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

namespace ProjectPASS
{
    public partial class FrmGPAUploadConfiguration : System.Web.UI.Page
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
            }
        }
        //protected void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    Database db = DatabaseFactory.CreateDatabase("cnPASS");
        //    string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
        //    string vTemplateId = txtConfigurationId.Text;
        //    SqlConnection _con = new SqlConnection(db.ConnectionString);
        //    _con.Open();
        //    SqlTransaction _tran = _con.BeginTransaction();
        //    try
        //    {
        //        // extract the rows to insert/update from the hidden field    
        //        string excelData = Grid1ExcelData.Value;

        //        // extract the ids of the rows to delete from the hidden field
        //        string excelDeletedIds = Grid1ExcelDeletedIds.Value;

        //        string[] rowSeparator = new string[] { "|*row*|" };
        //        string[] cellSeparator = new string[] { "|*cell*|" };

        //        string[] dataRows = excelData.Split(rowSeparator, StringSplitOptions.None);

        //        for (int i = 0; i < dataRows.Length; i++)
        //        {
        //            string[] dataCells = dataRows[i].Split(cellSeparator, StringSplitOptions.None);

        //            string vSourceColumnIndex = dataCells[1];
        //            string vSourceColumnName = dataCells[2];
        //            string vDestinationColumnIndex = dataCells[3];
        //            string vDestinationColumnName = dataCells[4];
        //            string vDestinationType = dataCells[5];
        //            string vDestinationLenght = dataCells[6];
        //            string bExcludeForPolicyUpload = dataCells[7];
        //            string bExcludeForClaimsUpload = dataCells[8];
        //            string bExcludeForEndorseUpload = dataCells[9];
        //            string bMandatoryForPolicy = dataCells[10];
        //            string bMandatoryForClaims = dataCells[11];
        //            string bMandatoryForEndorse = dataCells[12];


        //            String lcINSSTR = "Insert into TBL_GPA_COLUMN_MAPPING_MASTER (vTemplateId,vTemplateName,vSourceColumnIndex,vSourceColumnName,vDestinationColumnIndex,vDestinationColumnName,vDestinationType,vDestinationLenght,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bExcludeForPolicyUpload,bExcludeForClaimsUpload,bExcludeForEndorseUpload,bMandatoryForPolicy,bMandatoryForClaims,bMandatoryForEndorse)values " +
        //                "('" + vTemplateId.ToUpper() + "','" + txtConfigurationName.Text.ToUpper() + "','" + vSourceColumnIndex + "','" + vSourceColumnName + "','" + vDestinationColumnIndex + "','" + vDestinationColumnName + "','" + vDestinationType + "','" + vDestinationLenght + "','" + Session["vUserLoginId"] + "','" + Session["vUserLoginId"] + "',getdate(),getdate(),'" + bExcludeForPolicyUpload + "','" + bExcludeForClaimsUpload + "','" + bExcludeForEndorseUpload + "','" + bMandatoryForPolicy + "','" + bMandatoryForClaims + "','" + bMandatoryForEndorse + "')";

        //            SqlCommand  _insertCmd = new SqlCommand(lcINSSTR, _con);
        //            _insertCmd.Transaction = _tran;
        //            _insertCmd.ExecuteNonQuery();

        //        }
        //        _tran.Commit();
        //        _con.Close();
        //        Alert.Show("Record Updated SuccessFully","FrmGPAUploadConfiguration.aspx");
        //    }
        //    catch (Exception ex)
        //    {
        //        lblstatus.Text = ex.Message;
        //    }
        //}
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
            string vTemplateId = txtConfigurationId.Text;
            SqlConnection _con = new SqlConnection(db.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();
            try
            {
                foreach (GridViewRow grvRow in gvSubDetails.Rows)
                {
                    string vSourceColumnIndex = (grvRow.Cells[1].FindControl("txtvSourceColumnIndex") as TextBox).Text;
                    string vSourceColumnName = (grvRow.Cells[2].FindControl("txtvSourceColumnName") as TextBox).Text;
                    string vDestinationColumnIndex = (grvRow.Cells[3].FindControl("txtvDestinationColumnIndex") as Label).Text;
                    string vDestinationColumnName = (grvRow.Cells[4].FindControl("txtvDestinationColumnName") as Label).Text;
                    string vDestinationType = (grvRow.Cells[5].FindControl("txtvDestinationType") as Label).Text;
                    string vDestinationLenght = (grvRow.Cells[6].FindControl("txtvDestinationLenght") as Label).Text;
                    string bExcludeForPolicyUpload = (grvRow.Cells[7].FindControl("drpbExcludeForPolicyUpload") as DropDownList).SelectedItem.Value;
                    string bExcludeForClaimsUpload = (grvRow.Cells[8].FindControl("drpbExcludeForClaimsUpload") as DropDownList).SelectedItem.Value;
                    string bExcludeForEndorseUpload = (grvRow.Cells[9].FindControl("drpbExcludeForEndorseUpload") as DropDownList).SelectedItem.Value;
                    string bMandatoryForPolicy = (grvRow.Cells[10].FindControl("drpbMandatoryForPolicy") as DropDownList).SelectedItem.Value;
                    string bMandatoryForClaims = (grvRow.Cells[11].FindControl("drpbMandatoryForClaims") as DropDownList).SelectedItem.Value;
                    string bMandatoryForEndorse = (grvRow.Cells[12].FindControl("drpbMandatoryForEndorse") as DropDownList).SelectedItem.Value;


                    String lcINSSTR = "Insert into TBL_GPA_COLUMN_MAPPING_MASTER (vTemplateId,vTemplateName,vSourceColumnIndex,vSourceColumnName,vDestinationColumnIndex,vDestinationColumnName,vDestinationType,vDestinationLenght,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bExcludeForPolicyUpload,bExcludeForClaimsUpload,bExcludeForEndorseUpload,bMandatoryForPolicy,bMandatoryForClaims,bMandatoryForEndorse)values " +
                        "('" + vTemplateId.ToUpper() + "','" + txtConfigurationName.Text.ToUpper() + "','" + vSourceColumnIndex + "','" + vSourceColumnName + "','" + vDestinationColumnIndex + "','" + vDestinationColumnName + "','" + vDestinationType + "','" + vDestinationLenght + "','" + Session["vUserLoginId"] + "','" + Session["vUserLoginId"] + "',getdate(),getdate(),'" + bExcludeForPolicyUpload + "','" + bExcludeForClaimsUpload + "','" + bExcludeForEndorseUpload + "','" + bMandatoryForPolicy + "','" + bMandatoryForClaims + "','" + bMandatoryForEndorse + "')";

                    SqlCommand _insertCmd = new SqlCommand(lcINSSTR, _con);
                    _insertCmd.Transaction = _tran;
                    _insertCmd.ExecuteNonQuery();

                }
                _tran.Commit();
                _con.Close();
                Alert.Show("Record Updated SuccessFully", "FrmGPAUploadConfiguration.aspx");
            }
            catch (Exception ex)
            {
                lblstatus.Text = ex.Message;
            }
        }
        private void FirstGridViewRow()
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("vTemplateId", typeof(string)));
            dt.Columns.Add(new DataColumn("vSourceColumnIndex", typeof(string)));
            dt.Columns.Add(new DataColumn("vSourceColumnName", typeof(string)));
            dt.Columns.Add(new DataColumn("vDestinationColumnIndex", typeof(string)));
            dt.Columns.Add(new DataColumn("vDestinationColumnName", typeof(string)));
            dt.Columns.Add(new DataColumn("vDestinationType", typeof(string)));
            dt.Columns.Add(new DataColumn("vDestinationLenght", typeof(string)));
            dt.Columns.Add(new DataColumn("bExcludeForPolicyUpload", typeof(string)));
            dt.Columns.Add(new DataColumn("bExcludeForClaimsUpload", typeof(string)));
            dt.Columns.Add(new DataColumn("bExcludeForEndorseUpload", typeof(string)));
            dt.Columns.Add(new DataColumn("bMandatoryForPolicy", typeof(string)));
            dt.Columns.Add(new DataColumn("bMandatoryForClaims", typeof(string)));
            dt.Columns.Add(new DataColumn("bMandatoryForEndorse", typeof(string)));

            sqlCommand = "SELECT TOP 100 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'TBL_GPA_POLICY_TABLE' and COLUMN_NAME not in ('vCreatedBy','vModifiedBy','dCreatedDate','dModifiedDate')";
                DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                DataSet ds = null;
                ds = dbCOMMON.ExecuteDataSet(dbCommand);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["vTemplateId"] = txtConfigurationId.Text;
                        dr["vSourceColumnIndex"] = row["ORDINAL_POSITION"];
                        dr["vSourceColumnName"] = row["COLUMN_NAME"].ToString().Substring(1);
                        dr["vDestinationColumnIndex"] = row["ORDINAL_POSITION"];
                        dr["vDestinationColumnName"] = row["COLUMN_NAME"];
                        dr["vDestinationType"] = row["DATA_TYPE"];
                        dr["vDestinationLenght"] = row["CHARACTER_MAXIMUM_LENGTH"];
                        dr["bExcludeForPolicyUpload"] = "N";
                        dr["bExcludeForClaimsUpload"] = "N";
                        dr["bExcludeForEndorseUpload"] = "N";
                        dr["bMandatoryForPolicy"] = "N";
                        dr["bMandatoryForClaims"] = "N";
                        dr["bMandatoryForEndorse"] = "N";
                        dt.Rows.Add(dr);
                    }
                }
            
            ViewState["CurrentTable"] = dt;
            gvSubDetails.DataSource = dt;
            gvSubDetails.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
            string vTemplateId = "";
            SqlConnection _con = new SqlConnection(db.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();
            try
            {
                vTemplateId = wsGen.fn_Gen_Doc_Master_No("TEMP", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
                _tran.Commit();
                _con.Close();
                txtConfigurationId.Text = vTemplateId;
                FirstGridViewRow();
            }
            catch (Exception ex)
            {
                // log exception
                _tran.Rollback();
                Alert.Show(ex.Message.ToString());
            }
        }


        protected void BindData(string vTemplateId)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand = "Select Top 100 * from TBL_GPA_COLUMN_MAPPING_MASTER where vTemplateId='" + vTemplateId + "'";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            gvSubDetails.DataSource = ds.Tables[0];
            gvSubDetails.DataBind();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}

