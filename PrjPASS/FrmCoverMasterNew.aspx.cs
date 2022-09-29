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
using Obout.ComboBox;

namespace ProjectPASS
{
    public partial class FrmCoverMasterNew : System.Web.UI.Page
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
                BindProductData();
                //BindData();
                FillDrpSchemas();
            }
        }

        private void BindProductData()
        {

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand = "SELECT * FROM TBL_PRODUCT_MASTER";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet dsCover = null;
            dsCover = db.ExecuteDataSet(dbCommand);
            if (dsCover.Tables[0].Rows.Count > 0)
            {
                drpProduct.DataValueField = "Product_Code";
                drpProduct.DataTextField = "Product_Code";
                drpProduct.DataSource = dsCover.Tables[0];
                drpProduct.DataBind();
                ComboBoxItem l_lstItem = new ComboBoxItem("Select", "-1");
                drpProduct.Items.Insert(0, l_lstItem);
            }
            else
            {
                Alert.Show("No Schema Defined in Master");
                return;
            }

        }

        protected void FillDrpSchemas()
        {
            string StrCoverDBColumnList = string.Empty;
            string StrCoverSIDBColumnList = string.Empty;

            if (drpProduct.SelectedValue == "2023")
            {
                StrCoverDBColumnList = ConfigurationManager.AppSettings["StrCoverDBColumnList2023"].ToString();
                StrCoverSIDBColumnList = ConfigurationManager.AppSettings["StrCoverSIDBColumnList2023"].ToString();
            }

            else if (drpProduct.SelectedValue == "2025")
            {
                StrCoverDBColumnList = ConfigurationManager.AppSettings["StrCoverDBColumnList2025"].ToString();
                StrCoverSIDBColumnList = ConfigurationManager.AppSettings["StrCoverSIDBColumnList2025"].ToString();
            }
            else
            {
                return;
            }
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            SqlConnection con = new SqlConnection(db.ConnectionString);
            DataSet dsCover =new DataSet();
            using (SqlCommand cmd = new SqlCommand("PROC_GET_COVER_DBCOLUMN_PLANMASTER", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@vColumnList", StrCoverDBColumnList);
                //string sqlCommand = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'TBL_GPA_POLICY_TABLE' and COLUMN_NAME in (" + StrCoverDBColumnList + ") ";
                //DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
                //dsCover = db.ExecuteDataSet(dbCommand);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dsCover);

                if (dsCover.Tables[0].Rows.Count > 0)
                {
                    drplCoverField.DataValueField = "COLUMN_NAME";
                    drplCoverField.DataTextField = "COLUMN_NAME";
                    drplCoverField.DataSource = dsCover.Tables[0];
                    drplCoverField.DataBind();
                    ComboBoxItem l_lstItem = new ComboBoxItem("Select", "NA");
                    drplCoverField.Items.Insert(0, "Select");
                }
                else
                {
                    Alert.Show("No Schema Defined in Master");
                    return;
                }
            }

            using (SqlCommand cmd = new SqlCommand("PROC_GET_COVER_SI_DBCOLUMN_PLANMASTER", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@vColumnList", StrCoverSIDBColumnList);
                DataSet dsCoverSI = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dsCoverSI);
                if (dsCoverSI.Tables[0].Rows.Count > 0)
                {
                    drplCoverSIField.DataValueField = "COLUMN_NAME";
                    drplCoverSIField.DataTextField = "COLUMN_NAME";
                    drplCoverSIField.DataSource = dsCoverSI.Tables[0];
                    drplCoverSIField.DataBind();
                    ComboBoxItem l_lstItem = new ComboBoxItem("Select", "NA");
                    drplCoverSIField.Items.Insert(0, "Select");
                }
                else
                {
                    Alert.Show("No Schema Defined in Master");
                    return;
                }
            }



        }

        protected void gvSubDetails_RowUpdating(object sender, GridRecordEventArgs e)
        {
            string vCoverCode = e.Record["vCoverCode"].ToString();
            string vCoverDesc = e.Record["vCoverDesc"].ToString();
            string nCoverSI = e.Record["nCoverSI"].ToString();
            string bIsActive = e.Record["bIsActive"].ToString();
            string vCoverFieldInDB = e.Record["vCoverFieldInDB"].ToString();
            string vCoverSIFieldInDB = e.Record["vCoverSIFieldInDB"].ToString();
            try
            {
                //Open the SqlConnection     
                Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
                //string sqlCommand;
                //DbCommand dbCommand;
                //sqlCommand = "update TBL_COVER_MASTER set vCoverFieldInDB = '" + vCoverFieldInDB + "',vCoverSIFieldInDB='" + vCoverSIFieldInDB + "', bIsActive='" + bIsActive + "',vCoverDesc='" + vCoverDesc + "',nCoverSI='" + nCoverSI + "',vModifiedBy='" + Session["vUserLoginId"] + "' where vCoverCode = '" + vCoverCode + "'";
                //dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                //dbCOMMON.ExecuteNonQuery(dbCommand);
                SqlConnection con = new SqlConnection(dbCOMMON.ConnectionString);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand("PROC_UPDATE_TBL_COVER_MASTER_FIELDS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vCoverFieldInDB", vCoverFieldInDB.Trim().ToString());
                    cmd.Parameters.AddWithValue("@vCoverSIFieldInDB", vCoverSIFieldInDB.Trim().ToString());
                    cmd.Parameters.AddWithValue("@bIsActive", bIsActive.Trim().ToString());
                    cmd.Parameters.AddWithValue("@vCoverDesc", vCoverDesc.Trim().ToString());
                    cmd.Parameters.AddWithValue("@nCoverSI", nCoverSI.ToString());
                    cmd.Parameters.AddWithValue("@vModifiedBy", Session["vUserLoginId"].ToString());
                    cmd.Parameters.AddWithValue("@vCoverCode", vCoverCode.Trim().ToString());
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
            }
            BindData();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(txtCoverDesc.Text))
            {
                Alert.Show("Cover Name is Mandatory!");
            }

            else if (String.IsNullOrEmpty(txtCoverSI.Text))
            {
                Alert.Show("Cover SI is Mandatory!");
            }

            else if (drplCoverField.SelectedItem.ToString() == "Select")
            {
                Alert.Show("Cover Field is Mandatory!");
            }

            else if (drplCoverSIField.SelectedItem.ToString() == "Select")
            {
                Alert.Show("Cover SI Field is Mandatory!");
            }

            else if (drpProduct.SelectedText == "")
            {
                Alert.Show("Product Name is Mandatory!");
            }

            else
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                Cls_General_Functions wsDocNo = new Cls_General_Functions();
                string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
                string vCoverCode = "";
                SqlConnection _con = new SqlConnection(db.ConnectionString);
                _con.Open();
                SqlTransaction _tran = _con.BeginTransaction();
                try
                {
                    {
                        vCoverCode = wsDocNo.fn_Gen_Doc_Master_No("COVE", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);

                        //string lcINSSTR = "Insert into TBL_COVER_MASTER (vCoverCode,vCoverDesc,nCoverSI,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive,vCoverFieldInDB,vCoverSIFieldInDB,vProductCode,vProductName)values " +
                        //    "('" + vCoverCode.ToUpper() + "','" + txtCoverDesc.Text.ToUpper() + "','" + txtCoverSI.Text + "','" + Session["vUserLoginId"] + "','" + Session["vUserLoginId"] + "',getdate(),getdate(),'Y','" + drpCoverField.SelectedValue.Trim() + "','" + drpCoverSIField.SelectedValue.Trim() + "')";

                        string lcINSSTR = "Insert into TBL_COVER_MASTER (vCoverCode,vCoverDesc,nCoverSI,vCreatedBy,vModifiedBy,dCreatedDate,dModifiedDate,bIsActive,vCoverFieldInDB,vCoverSIFieldInDB,vProductCode,vProductName) values " +
                            "(@vCoverCode,@vCoverDesc,@nCoverSI,@vCreatedBy,@vModifiedBy,getdate(),getdate(),@bIsActive,@vCoverFieldInDB,@vCoverSIFieldInDB,@vProductCode,@vProductName)";

                        SqlCommand _insertCmd = new SqlCommand(lcINSSTR, _con);

                        var sqlParamUser = new SqlParameter("@vCoverCode", SqlDbType.VarChar);
                        sqlParamUser.Value = vCoverCode.ToUpper().Trim();
                        _insertCmd.Parameters.Add(sqlParamUser);


                        sqlParamUser = new SqlParameter("@vCoverDesc", SqlDbType.VarChar);
                        sqlParamUser.Value = txtCoverDesc.Text.Trim();
                        _insertCmd.Parameters.Add(sqlParamUser);

                        sqlParamUser = new SqlParameter("@nCoverSI", SqlDbType.VarChar);
                        sqlParamUser.Value = txtCoverSI.Text.Trim();
                        _insertCmd.Parameters.Add(sqlParamUser);


                        sqlParamUser = new SqlParameter("@vCreatedBy", SqlDbType.VarChar);
                        sqlParamUser.Value = Session["vUserLoginId"].ToString();
                        _insertCmd.Parameters.Add(sqlParamUser);

                        sqlParamUser = new SqlParameter("@vModifiedBy", SqlDbType.VarChar);
                        sqlParamUser.Value = Session["vUserLoginId"].ToString();
                        _insertCmd.Parameters.Add(sqlParamUser);

                        sqlParamUser = new SqlParameter("@bIsActive", SqlDbType.VarChar);
                        sqlParamUser.Value = 'Y';
                        _insertCmd.Parameters.Add(sqlParamUser);

                        sqlParamUser = new SqlParameter("@vCoverFieldInDB", SqlDbType.VarChar);
                        sqlParamUser.Value = drplCoverField.SelectedValue.Trim();
                        _insertCmd.Parameters.Add(sqlParamUser);

                        sqlParamUser = new SqlParameter("@vCoverSIFieldInDB", SqlDbType.VarChar);
                        sqlParamUser.Value = drplCoverSIField.SelectedValue.Trim();
                        _insertCmd.Parameters.Add(sqlParamUser);

                        sqlParamUser = new SqlParameter("@vProductCode", SqlDbType.VarChar);
                        sqlParamUser.Value = drpProduct.SelectedValue;
                        _insertCmd.Parameters.Add(sqlParamUser);


                        sqlParamUser = new SqlParameter("@vProductName", SqlDbType.VarChar);
                        sqlParamUser.Value = lblProductName.Text;
                        _insertCmd.Parameters.Add(sqlParamUser);

                        _insertCmd.Transaction = _tran;
                        _insertCmd.ExecuteNonQuery();

                    }

                    _tran.Commit();
                    _con.Close();

                    Alert.Show("Cover Created for " + txtCoverDesc.Text + " ,Please Note Cover Code : " + vCoverCode, "FrmCoverMasterNew.aspx");

                }
                catch (Exception ex)
                {
                    // log exception
                    _tran.Rollback();
                    Alert.Show(ex.Message.ToString());
                }
            }
        }


        protected void BindData()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "Select a.vCoverCode,a.vCoverDesc,a.nCoverSI,a.bIsActive,a.vCoverFieldInDB,a.vCoverSIFieldInDB from TBL_COVER_MASTER a Order by a.vCoverDesc";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet ds = null;

            ds = db.ExecuteDataSet(dbCommand);

            DataTable dtRequisition = new DataTable();

            dtRequisition = ds.Tables[0];

            //Create DataTable

            DataTable dt = new DataTable();

            //Put some columns in it.

            dt.Columns.Add(new DataColumn("vCoverCode", typeof(string)));
            dt.Columns.Add(new DataColumn("vCoverDesc", typeof(string)));
            dt.Columns.Add(new DataColumn("nCoverSI", typeof(double)));
            dt.Columns.Add(new DataColumn("bIsActive", typeof(string)));
            dt.Columns.Add(new DataColumn("vCoverFieldInDB", typeof(string)));
            dt.Columns.Add(new DataColumn("vCoverSIFieldInDB", typeof(string)));

            if (dtRequisition.Rows.Count > 0)
            {
                foreach (DataRow row in dtRequisition.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["vCoverCode"] = row["vCoverCode"];
                    dr["vCoverDesc"] = row["vCoverDesc"];
                    dr["nCoverSI"] = row["nCoverSI"];
                    dr["vCoverFieldInDB"] = row["vCoverFieldInDB"];
                    dr["vCoverSIFieldInDB"] = row["vCoverSIFieldInDB"];

                    if (row["bIsActive"].ToString() == "Y")
                    {
                        dr["bIsActive"] = "Yes";
                    }
                    else
                    {
                        dr["bIsActive"] = "No";
                    }
                    dt.Rows.Add(dr);
                }
                gvSubDetails.DataSource = dt;
                gvSubDetails.DataBind();
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["vCoverCode"] = "";
                dr["vCoverDesc"] = "";
                dr["nCoverSI"] = 0;
                dr["bIsActive"] = "No";
                dr["vCoverFieldInDB"] = " ";
                dr["vCoverSIFieldInDB"] = " ";
                dt.Rows.Add(dr);
                gvSubDetails.DataSource = dt;
                gvSubDetails.DataBind();
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void drpProduct_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
        {
            if (drpProduct.SelectedValue != "-1")
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SELECT Product_Name FROM TBL_PRODUCT_MASTER where Product_code= @Product_code";
                        cmd.Parameters.AddWithValue("@Product_code" , drpProduct.SelectedValue.ToString());
                        cmd.Connection = sqlCon;
                        sqlCon.Open();
                        object objProd = cmd.ExecuteScalar();
                        lblProductName.Text = Convert.ToString(objProd);
                        sqlCon.Close();
                    }
                }



                //string sqlCommand = "Select a.vCoverCode,a.vCoverDesc,a.nCoverSI,a.bIsActive,a.vCoverFieldInDB,a.vCoverSIFieldInDB from TBL_COVER_MASTER a where vProductCode="'"+ drpProduct.SelectedValue +"'" Order by a.vCoverDesc";
                string sqlCommand = "Select a.vCoverCode,a.vCoverDesc,a.nCoverSI,a.bIsActive,a.vCoverFieldInDB,a.vCoverSIFieldInDB,a.vProductName from TBL_COVER_MASTER a where vProductCode=@vProductCode Order by a.vCoverDesc";
                DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);

                var sqlParam = new SqlParameter("@vProductCode", SqlDbType.VarChar);
                sqlParam.Value = drpProduct.SelectedValue.Trim().ToString();
                dbCommand.Parameters.Add(sqlParam);
                DataSet ds = null;

                ds = db.ExecuteDataSet(dbCommand);

                DataTable dtRequisition = new DataTable();

                dtRequisition = ds.Tables[0];

                //Create DataTable

                DataTable dt = new DataTable();

                //Put some columns in it.

                dt.Columns.Add(new DataColumn("vCoverCode", typeof(string)));
                dt.Columns.Add(new DataColumn("vCoverDesc", typeof(string)));
                dt.Columns.Add(new DataColumn("nCoverSI", typeof(double)));
                dt.Columns.Add(new DataColumn("bIsActive", typeof(string)));
                dt.Columns.Add(new DataColumn("vCoverFieldInDB", typeof(string)));
                dt.Columns.Add(new DataColumn("vCoverSIFieldInDB", typeof(string)));
                dt.Columns.Add(new DataColumn("vProductName", typeof(string)));

                if (dtRequisition.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRequisition.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["vCoverCode"] = row["vCoverCode"];
                        dr["vCoverDesc"] = row["vCoverDesc"];
                        dr["nCoverSI"] = row["nCoverSI"];
                        dr["vCoverFieldInDB"] = row["vCoverFieldInDB"];
                        dr["vCoverSIFieldInDB"] = row["vCoverSIFieldInDB"];
                        dr["vProductName"] = row["vProductName"];

                        if (row["bIsActive"].ToString() == "Y")
                        {
                            dr["bIsActive"] = "Yes";
                        }
                        else
                        {
                            dr["bIsActive"] = "No";
                        }
                        dt.Rows.Add(dr);
                    }
                    gvSubDetails.DataSource = dt;
                    gvSubDetails.DataBind();
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["vCoverCode"] = "";
                    dr["vCoverDesc"] = "";
                    dr["nCoverSI"] = 0;
                    dr["bIsActive"] = "No";
                    dr["vCoverFieldInDB"] = " ";
                    dr["vCoverSIFieldInDB"] = " ";
                    dt.Rows.Add(dr);
                    gvSubDetails.DataSource = dt;
                    gvSubDetails.DataBind();
                }

                drplCoverField.Items.Clear();
                drplCoverSIField.Items.Clear();
                FillDrpSchemas();

            }
            else
            {

            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (drpProduct.SelectedValue != "-1")
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                //string sqlCommand = "Select a.vCoverCode,a.vCoverDesc,a.nCoverSI,a.bIsActive,a.vCoverFieldInDB,a.vCoverSIFieldInDB from TBL_COVER_MASTER a where vProductCode="'"+ drpProduct.SelectedValue +"'" Order by a.vCoverDesc";
                string sqlCommand = "Select a.vCoverCode,a.vCoverDesc,a.nCoverSI,a.bIsActive,a.vCoverFieldInDB,a.vCoverSIFieldInDB,a.vProductName from TBL_COVER_MASTER a where vProductCode=@vProductCode Order by a.vCoverDesc";
                DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
                var sqlParamUser = new SqlParameter("@vProductCode", SqlDbType.VarChar);
                sqlParamUser.Value = drpProduct.SelectedValue.Trim().ToString();
                dbCommand.Parameters.Add(sqlParamUser);
                DataSet ds = null;

                ds = db.ExecuteDataSet(dbCommand);

                DataTable dtRequisition = new DataTable();

                dtRequisition = ds.Tables[0];

                //Create DataTable

                DataTable dt = new DataTable();

                //Put some columns in it.

                dt.Columns.Add(new DataColumn("vCoverCode", typeof(string)));
                dt.Columns.Add(new DataColumn("vCoverDesc", typeof(string)));
                dt.Columns.Add(new DataColumn("nCoverSI", typeof(double)));
                dt.Columns.Add(new DataColumn("bIsActive", typeof(string)));
                dt.Columns.Add(new DataColumn("vCoverFieldInDB", typeof(string)));
                dt.Columns.Add(new DataColumn("vCoverSIFieldInDB", typeof(string)));
                dt.Columns.Add(new DataColumn("vProductName", typeof(string)));

                if (dtRequisition.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRequisition.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["vCoverCode"] = row["vCoverCode"];
                        dr["vCoverDesc"] = row["vCoverDesc"];
                        dr["nCoverSI"] = row["nCoverSI"];
                        dr["vCoverFieldInDB"] = row["vCoverFieldInDB"];
                        dr["vCoverSIFieldInDB"] = row["vCoverSIFieldInDB"];
                        dr["vProductName"] = row["vProductName"];

                        if (row["bIsActive"].ToString() == "Y")
                        {
                            dr["bIsActive"] = "Yes";
                        }
                        else
                        {
                            dr["bIsActive"] = "No";
                        }
                        dt.Rows.Add(dr);
                    }
                    gvSubDetails.DataSource = dt;
                    gvSubDetails.DataBind();
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["vCoverCode"] = "";
                    dr["vCoverDesc"] = "";
                    dr["nCoverSI"] = 0;
                    dr["bIsActive"] = "No";
                    dr["vCoverFieldInDB"] = " ";
                    dr["vCoverSIFieldInDB"] = " ";
                    dt.Rows.Add(dr);
                    gvSubDetails.DataSource = dt;
                    gvSubDetails.DataBind();
                }

            }
            else
            {

            }
        }
    }
}

