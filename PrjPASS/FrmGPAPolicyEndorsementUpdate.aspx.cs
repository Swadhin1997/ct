using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Obout.Grid;

namespace ProjectPASS
{
    public partial class FrmGPAPolicyEndorsementUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData("1", "1", "");
            }
        }
        protected void BindData(string vCertificateNo, string vCrnNo, string vCustomerName)
        {
            string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
            int isGPA = 0;
            if (rdoGPAPolicyType.Checked == true)
            {
                isGPA = 1;
            }
                using (SqlConnection sqlCon = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_TBL_GPA_POLICY_TABLE_BIND_DATA", sqlCon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //string strvPolicyId ="";
                        //string strvCustomerId ="";
                        //string strvCustomerName ="";
                        if (vCertificateNo != "")
                        {
                            cmd.Parameters.AddWithValue("@vPolicyId", vCertificateNo);
                            //strvPolicyId = " and vCertificateNo='" + vCertificateNo + "'";
                        }
                        if (vCrnNo != "")
                        {
                            cmd.Parameters.AddWithValue("@vCustomerId", vCrnNo);
                            //strvCustomerId = " and vCrnNo='" + vCrnNo + "'";
                        }
                        if (vCustomerName != "")
                        {
                            cmd.Parameters.AddWithValue("@vCustomerName", vCustomerName);
                            //strvCustomerName = " and vCustomerName like '%" + vCustomerName + "%'";
                        }
                        cmd.Parameters.AddWithValue("@vIsGPA", isGPA);
                        // cmd.CommandText = "SELECT * FROM TBL_GPA_POLICY_TABLE where 1=1 " + strvPolicyId + " " + strvCustomerId + " " + strvCustomerName + " ";
                        cmd.Connection = sqlCon;
                        sqlCon.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            gvSubDetails.DataSource = dt;
                            gvSubDetails.DataBind();
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            gvSubDetails.DataSource = dt;
                            gvSubDetails.DataBind();
                            gvSubDetails.Rows[0].Visible = false;
                        }
                        sqlCon.Close();
                    }
                }
            }
               
        ////called on row edit command
        //protected void gvSubDetails_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    gvSubDetails.EditIndex = e.NewEditIndex;
        //    BindData(txtPolicyId.Text, txtCustomerId.Text, txtCustomerName.Text);
        //}


        //////called when cancel edit mode
        //protected void gvSubDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    gvSubDetails.EditIndex = -1;
        //    BindData(txtPolicyId.Text, txtCustomerId.Text, txtCustomerName.Text);
        //}

        //called on row update command

        protected void gvSubDetails_RowUpdating(object sender, GridRecordEventArgs e)
        {
            bool IsUpdated = false;

            try
            {
                int isGPA = 0;
                if (rdoGPAPolicyType.Checked == true)
                {
                    isGPA = 1;
                }
                //getting key value, row id
                string vCertificateNo = e.Record["vCertificateNo"].ToString();
                //getting row field details
                string vCrnNo = e.Record["vCrnNo"].ToString();
                string vAccountNo = e.Record["vAccountNo"].ToString();
                string vCustomerName = e.Record["vCustomerName"].ToString();
                string vCustomerGender = e.Record["vCustomerGender"].ToString();
                string dCustomerDob = e.Record["dCustomerDob"].ToString();
                string vProposerAddLine1 = e.Record["vProposerAddLine1"].ToString();
                string vProposerAddLine2 = e.Record["vProposerAddLine2"].ToString();
                string vProposerAddLine3 = e.Record["vProposerAddLine3"].ToString();
                string vProposerCity = e.Record["vProposerCity"].ToString();
                string vProposerState = e.Record["vProposerState"].ToString();
                string vProposerPinCode = e.Record["vProposerPinCode"].ToString();
                string vMobileNo = e.Record["vMobileNo"].ToString();
                string vEmailId = e.Record["vEmailId"].ToString();
                string vNomineeName = e.Record["vNomineeName"].ToString();
                string vNomineeRelation = e.Record["vNomineeRelation"].ToString();
                string vNomineeAge = e.Record["vNomineeAge"].ToString();
                string vNomineeGuardian = e.Record["vNomineeGuardian"].ToString();
                string vNomineeRelWithGuardian = e.Record["vNomineeRelWithGuardian"].ToString();
                string vCorporateID = e.Record["vCorporateID"].ToString();
                string vCorporateName = e.Record["vCorporateName"].ToString();
                string vEndorsementType = e.Record["vEndorsementType"].ToString();
                string vEndorsementReason = e.Record["vEndorsementReason"].ToString();
                string vEndorsementDesc = e.Record["vEndorsementDesc"].ToString();
                string dEndorsementEffectiveDate = e.Record["dEndorsementEffectiveDate"].ToString();
                string dEndorsementIssueDate = e.Record["dEndorsementIssueDate"].ToString();

                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlConnection sqlCon = new SqlConnection(consString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "PROC_INSERT_GPA_HISTORY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vCertificateNo", vCertificateNo);
                    //cmd.Parameters.AddWithValue("@vCustomerName", txtCustomerName.Text);
                    cmd.Parameters.AddWithValue("@vIsGPA", isGPA);                    
                    cmd.Connection = sqlCon;
                    sqlCon.Open();
                    IsUpdated = cmd.ExecuteNonQuery() > 0;
                    sqlCon.Close();

                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandText = "PROC_UPDATE_TBL_GPA_POLICY_TABLE_HISTORY";
                    cmd2.Parameters.AddWithValue("@vModifiedBy", Session["vUserLoginId"].ToString());
                    cmd2.Parameters.AddWithValue("@vCertificateNo", vCertificateNo);
                    cmd2.Parameters.AddWithValue("@vIsGPA", isGPA);
                    //cmd.CommandText = @"UPDATE TBL_GPA_POLICY_TABLE_HISTORY SET dModifiedDate=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "'  WHERE vCertificateNo=@vCertificateNo";
                    cmd2.Connection = sqlCon;
                    sqlCon.Open();
                    IsUpdated = cmd2.ExecuteNonQuery() > 0;
                    sqlCon.Close();

                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.CommandText = "PROC_UPDATE_TBL_GPA_POLICY_TABLE";
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@vCertificateNo", vCertificateNo);
                    cmd3.Parameters.AddWithValue("@vCrnNo", vCrnNo);
                    cmd3.Parameters.AddWithValue("@vAccountNo", vAccountNo);
                    cmd3.Parameters.AddWithValue("@vCustomerName", vCustomerName);
                    cmd3.Parameters.AddWithValue("@vCustomerGender", vCustomerGender);
                    cmd3.Parameters.AddWithValue("@dCustomerDob", dCustomerDob);
                    cmd3.Parameters.AddWithValue("@vProposerAddLine1", vProposerAddLine1);
                    cmd3.Parameters.AddWithValue("@vProposerAddLine2", vProposerAddLine2);
                    cmd3.Parameters.AddWithValue("@vProposerAddLine3", vProposerAddLine3);
                    cmd3.Parameters.AddWithValue("@vProposerCity", vProposerCity);
                    cmd3.Parameters.AddWithValue("@vProposerState", vProposerState);
                    cmd3.Parameters.AddWithValue("@vProposerPinCode", vProposerPinCode);
                    cmd3.Parameters.AddWithValue("@vMobileNo", vMobileNo);
                    cmd3.Parameters.AddWithValue("@vEmailId", vEmailId);
                    cmd3.Parameters.AddWithValue("@vNomineeName", vNomineeName);
                    cmd3.Parameters.AddWithValue("@vNomineeRelation", vNomineeRelation);
                    cmd3.Parameters.AddWithValue("@vNomineeAge", vNomineeAge);
                    cmd3.Parameters.AddWithValue("@vNomineeGuardian", vNomineeGuardian);
                    cmd3.Parameters.AddWithValue("@vNomineeRelWithGuardian", vNomineeRelWithGuardian);
                    cmd3.Parameters.AddWithValue("@vCorporateID", vCorporateID);
                    cmd3.Parameters.AddWithValue("@vCorporateName", vCorporateName);
                    cmd3.Parameters.AddWithValue("@vEndorsementType", vEndorsementType);
                    cmd3.Parameters.AddWithValue("@vEndorsementReason", vEndorsementReason);
                    cmd3.Parameters.AddWithValue("@vEndorsementDesc", vEndorsementDesc);
                    cmd3.Parameters.AddWithValue("@dEndorsementEffectiveDate", dEndorsementEffectiveDate);
                    cmd3.Parameters.AddWithValue("@dEndorsementIssueDate", dEndorsementIssueDate);
                    cmd3.Parameters.AddWithValue("@vModifiedBy", Session["vUserLoginId"].ToString());
                    cmd3.Parameters.AddWithValue("@vIsGPA", isGPA);
                        
                    cmd3.Connection = sqlCon;
                    sqlCon.Open();
                    IsUpdated = cmd3.ExecuteNonQuery() > 0;
                    sqlCon.Close();
                    if (IsUpdated)
                    {
                        lblstatus.Text = vCustomerName + " details updated successfully!";
                        lblstatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblstatus.Text = "Error while updating " + vCustomerName + "  details";
                        lblstatus.ForeColor = System.Drawing.Color.Red;
                    }
                    //gvSubDetails.EditIndex = -1;
                    BindData(vCertificateNo, vCrnNo, vCustomerName);
                    }            
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
                return;
            }
        }

        //protected void gvSubDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    bool IsUpdated = false;
        //    //getting key value, row id
        //    string vCertificateNo = gvSubDetails.DataKeys[e.RowIndex].Value.ToString();
        //    //getting row field details
        //    Label txtvCrnNo = (Label)gvSubDetails.Rows[e.RowIndex].FindControl("txtvCrnNo");
        //    TextBox txtvCustomerName = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvCustomerName");
        //    DropDownList drpGenderGrid = (DropDownList)gvSubDetails.Rows[e.RowIndex].FindControl("drpGenderGrid");
        //    TextBox txtdCustomerDob = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtdCustomerDob");
        //    TextBox txtvProposerAddLine1 = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvProposerAddLine1");
        //    TextBox txtvProposerAddLine2 = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvProposerAddLine2");
        //    TextBox txtvProposerAddLine3 = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvProposerAddLine3");
        //    TextBox txtvProposerCity = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvProposerCity");
        //    TextBox txtvProposerState = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvProposerState");
        //    TextBox txtvProposerPinCode = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvProposerPinCode");
        //    TextBox txtvMobileNo = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvMobileNo");
        //    TextBox txtvEmailId = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvEmailId");
        //    TextBox txtvNomineeName = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvNomineeName");
        //    TextBox txtvNomineeRelation = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvNomineeRelation");
        //    TextBox txtvNomineeAge = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvNomineeAge");
        //    TextBox txtvNomineeGuardian = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvNomineeGuardian");
        //    TextBox txtvNomineeRelWithGuardian = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvNomineeRelWithGuardian");
        //    TextBox txtvCorporateID = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvCorporateID");
        //    TextBox txtvCorporateName = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvCorporateName");
        //    TextBox txtvEndorsementType = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvEndorsementType");
        //    TextBox txtvEndorsementReason = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvEndorsementReason");
        //    TextBox txtvEndorsementDesc = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtvEndorsementDesc");
        //    TextBox txtdEndorsementEffectiveDate = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtdEndorsementEffectiveDate");
        //    TextBox txtdEndorsementIssueDate = (TextBox)gvSubDetails.Rows[e.RowIndex].FindControl("txtdEndorsementIssueDate");

        //    string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
        //    using (SqlConnection sqlCon = new SqlConnection(consString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            //here i'd added "@" for continuous string in new line
        //            cmd.CommandText = @"INSERT INTO TBL_GPA_POLICY_TABLE_HISTORY SELECT * FROM TBL_GPA_POLICY_TABLE WHERE vCertificateNo=@vCertificateNo";
        //            cmd.Parameters.AddWithValue("@vCertificateNo", vCertificateNo);
        //            //cmd.Parameters.AddWithValue("@vCustomerName", txtCustomerName.Text);
        //            cmd.Connection = sqlCon;
        //            sqlCon.Open();
        //            IsUpdated = cmd.ExecuteNonQuery() > 0;
        //            sqlCon.Close();

        //            cmd.CommandText = @"UPDATE TBL_GPA_POLICY_TABLE_HISTORY SET dModifiedDate=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "'  WHERE vCertificateNo=@vCertificateNo";
        //            cmd.Connection = sqlCon;
        //            sqlCon.Open();
        //            IsUpdated = cmd.ExecuteNonQuery() > 0;
        //            sqlCon.Close();

        //            cmd.CommandText = @"UPDATE TBL_GPA_POLICY_TABLE SET " +
        //            " vCustomerName=@vCustomerName," +
        //            " vCustomerGender=@vCustomerGender," +
        //            " dCustomerDob=@dCustomerDob," +
        //            " vProposerAddLine1=@vProposerAddLine1," +
        //            " vProposerAddLine2=@vProposerAddLine2," +
        //            " vProposerAddLine3=@vProposerAddLine3," +
        //            " vProposerCity=@vProposerCity," +
        //            " vProposerState=@vProposerState," +
        //            " vProposerPinCode=@vProposerPinCode," +
        //            " vMobileNo=@vMobileNo," +
        //            " vEmailId=@vEmailId," +
        //            " vNomineeName=@vNomineeName," +
        //            " vNomineeRelation=@vNomineeRelation," +
        //            " vNomineeAge=@vNomineeAge," +
        //            " vNomineeGuardian=@vNomineeGuardian," +
        //            " vNomineeRelWithGuardian=@vNomineeRelWithGuardian," +
        //            " vCorporateID=@vCorporateID," +
        //            " vCorporateName=@vCorporateName," +
        //            " vEndorsementType=@vEndorsementType," +
        //            " vEndorsementReason=@vEndorsementReason," +
        //            " vEndorsementDesc=@vEndorsementDesc," +
        //            " dEndorsementEffectiveDate=@dEndorsementEffectiveDate," +
        //            " dEndorsementIssueDate=@dEndorsementIssueDate," +
        //            " bEndorsementStatus='Y',dModifiedDate=GETDATE(),vModifiedBy='" + Session["vUserLoginId"] + "'  WHERE vCertificateNo=@vCertificateNo";
        //            cmd.Parameters.AddWithValue("@vCustomerName", txtvCustomerName.Text);
        //            cmd.Parameters.AddWithValue("@vCustomerGender", drpGenderGrid.SelectedValue);
        //            cmd.Parameters.AddWithValue("@dCustomerDob", txtdCustomerDob.Text);
        //            cmd.Parameters.AddWithValue("@vProposerAddLine1", txtvProposerAddLine1.Text);
        //            cmd.Parameters.AddWithValue("@vProposerAddLine2", txtvProposerAddLine2.Text);
        //            cmd.Parameters.AddWithValue("@vProposerAddLine3", txtvProposerAddLine3.Text);
        //            cmd.Parameters.AddWithValue("@vProposerCity", txtvProposerCity.Text);
        //            cmd.Parameters.AddWithValue("@vProposerState", txtvProposerState.Text);
        //            cmd.Parameters.AddWithValue("@vProposerPinCode", txtvProposerPinCode.Text);
        //            cmd.Parameters.AddWithValue("@vMobileNo", txtvMobileNo.Text);
        //            cmd.Parameters.AddWithValue("@vEmailId", txtvEmailId.Text);
        //            cmd.Parameters.AddWithValue("@vNomineeName", txtvNomineeName.Text);
        //            cmd.Parameters.AddWithValue("@vNomineeRelation", txtvNomineeRelation.Text);
        //            cmd.Parameters.AddWithValue("@vNomineeAge", txtvNomineeAge.Text);
        //            cmd.Parameters.AddWithValue("@vNomineeGuardian", txtvNomineeGuardian.Text);
        //            cmd.Parameters.AddWithValue("@vNomineeRelWithGuardian", txtvNomineeRelWithGuardian.Text);
        //            cmd.Parameters.AddWithValue("@vCorporateID", txtvCorporateID.Text);
        //            cmd.Parameters.AddWithValue("@vCorporateName", txtvCorporateName.Text);
        //            cmd.Parameters.AddWithValue("@vEndorsementType", txtvEndorsementType.Text);
        //            cmd.Parameters.AddWithValue("@vEndorsementReason", txtvEndorsementReason.Text);
        //            cmd.Parameters.AddWithValue("@vEndorsementDesc", txtvEndorsementDesc.Text);
        //            cmd.Parameters.AddWithValue("@dEndorsementEffectiveDate", txtdEndorsementEffectiveDate.Text);
        //            cmd.Parameters.AddWithValue("@dEndorsementIssueDate", txtdEndorsementIssueDate.Text);
        //            cmd.Connection = sqlCon;
        //            sqlCon.Open();
        //            IsUpdated = cmd.ExecuteNonQuery() > 0;
        //            sqlCon.Close();
        //        }
        //    }
        //    if (IsUpdated)
        //    {
        //        lblstatus.Text = "'" + txtvCustomerName.Text + "'  details updated successfully!";
        //        lblstatus.ForeColor = System.Drawing.Color.Green;
        //    }
        //    else
        //    {
        //        lblstatus.Text = "Error while updating '" + txtvCustomerName.Text + "'  details";
        //        lblstatus.ForeColor = System.Drawing.Color.Red;
        //    }
        //    gvSubDetails.EditIndex = -1;
        //    BindData(vCertificateNo, txtvCrnNo.Text, txtvCustomerName.Text);
        //}

        protected void btnGetPolicy_Click(object sender, EventArgs e)
        {
            lblstatus.Text = "";
            BindData(txtPolicyId.Text, txtCrnNo.Text, txtCustomerName.Text);
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}