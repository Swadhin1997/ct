using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Obout.Grid;
//using Obout.ComboBox;
using ProjectPASS;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.Services;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net.Mime;

using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;

using System.ServiceModel.Activation;


using System.Web.Script.Serialization;

using Microsoft.VisualBasic;

using System.Net;
using System.Security.Cryptography;


using Google;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Urlshortener.v1.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.ModelBinding;

namespace PrjPASS
{
    public partial class FrmQuoteReassignment : System.Web.UI.Page
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
                    //Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    //return;
                }
                
                int NumberOfDaysQuoteDetailsRequired = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfDaysQuoteDetailsRequired"].ToString());


            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseModalSaveProposal();", true);
            }



        }

             
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

                
        #region CodeForGridView

        public IEnumerable<ProjectPASS.QuoteDetails> QuoteGridView_GetData([Control("txtSearchQuoteNumber")] string QuoteNumber, int maximumRows, int startRowIndex, out int totalRowCount)
        {

            int pageSize = maximumRows;
            int pageIndex = 0;
            //int totalCount = 0;
            totalRowCount = GetQuoteDetails().Count();

            if (startRowIndex > 0)
            {
                pageIndex = (int)Math.Round(((double)startRowIndex / (double)pageSize));
            }

            return GetQuoteDetails().OrderByDescending(x => x.QuoteDate).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public IEnumerable<QuoteDetails> GetQuoteDetails()
        {
            int NumberOfDaysQuoteDetailsRequired = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfDaysQuoteDetailsRequired"].ToString());
            string LoginUserId = Session["vUserLoginId"].ToString();
            string QuoteNumber = txtSearchQuoteNumber.Text;

            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_QUOTE_REASSIGNMENT_DETAILS";
                    cmd.Parameters.AddWithValue("@LoginUserId", LoginUserId);
                    cmd.Parameters.AddWithValue("@QuoteNumber", string.IsNullOrEmpty(QuoteNumber) ? "" : QuoteNumber.Trim());
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return this.CreateQuoteDetails(reader);
                        }
                    }
                    conn.Close();
                }
            }
        }

        private QuoteDetails CreateQuoteDetails(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("No Quote detail exist.");
            }

            return new QuoteDetails
            {
                QuoteNumber = Convert.ToString(reader["QuoteNumber"]),
                QuoteDate = Convert.ToDateTime(reader["CreatedDate"]),
                Make = Convert.ToString(reader["Manufacture"]),
                Model = Convert.ToString(reader["Model"]),
                Variant = Convert.ToString(reader["ModelVariant"]),
                TotalPremium = Convert.ToString(reader["TotalPremium"]),
                ProposalNumber = Convert.ToString(reader["ProposalNumber"]),
                BusinessType = Convert.ToString(reader["PropGeneralProposalInformation_BusinessType_Mandatary"]),
                CustomerType = Convert.ToString(reader["CustomerType"]),
                PaymentStatus = Convert.ToString(reader["PaymentStatus"]),
                QuoteVersion = Convert.ToInt16(reader["QuoteVersion"]),
                IsProposalExistsForQuoteNumber = Convert.ToString(reader["IsProposalExistsForQuoteNumber"]),
                ReviewAndConfirmLink = Convert.ToString(reader["PaymentStatus"]).ToUpper() == "SUCCESS" ? "" : Convert.ToString(reader["ReviewAndConfirmLink"]),
            };
        }

        protected void QuoteGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            QuoteGridView.PageIndex = e.NewPageIndex;
        }



        protected void QuoteGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool isNewBusiness = false;
            string NetPremium = string.Empty;
            string ServiceTax = string.Empty;
            string TotalPremium = string.Empty;
            string RequestXML = string.Empty;
            string ResultXML = string.Empty;
            string MarketMovement = string.Empty;
            long CreditScoreId = 0;
            string CreditScoreCustomerName = string.Empty;
            string CreditScoreIDProof = string.Empty;
            string CreditScoreIDProofNumber = string.Empty;

            string CGSTAmount = string.Empty;
            string CGSTPercentage = string.Empty;
            string SGSTAmount = string.Empty;
            string SGSTPercentage = string.Empty;
            string IGSTAmount = string.Empty;
            string IGSTPercentage = string.Empty;
            string UGSTAmount = string.Empty;
            string UGSTPercentage = string.Empty;
            string TotalGSTAmount = string.Empty;
            bool IsFastlaneFlow = false;

            txtSearchQuoteNumber.Text = "";
            int NumberOfDaysQuoteDetailsRequired = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfDaysQuoteDetailsRequired"].ToString());

            if (e.CommandName == "recalculate")
            {
                //GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //int SelectedQuoteVersion = int.Parse(rowSelect.Cells[1].Text);

                //hdnQuoteVersion.Value = SelectedQuoteVersion.ToString();

                //hdnCreditScoreId.Value = "0";
                //btnOpenCustomerPopUp.Visible = true;
                //LinkButton lnkRecalculate = (LinkButton)e.CommandSource;
                //string QuoteNumber = lnkRecalculate.CommandArgument;
                //GetResultXMLFromDB(QuoteNumber, ref isNewBusiness, ref NetPremium, ref ServiceTax, ref TotalPremium, ref RequestXML, ref ResultXML, ref MarketMovement, ref CreditScoreId, ref CreditScoreCustomerName, ref CreditScoreIDProof, ref CreditScoreIDProofNumber, ref CGSTAmount, ref CGSTPercentage, ref SGSTAmount, ref SGSTPercentage, ref IGSTAmount, ref IGSTPercentage, ref UGSTAmount, ref UGSTPercentage, ref TotalGSTAmount, ref IsFastlaneFlow, ref SelectedQuoteVersion);
                //ReCalculatePremium(RequestXML, QuoteNumber, MarketMovement, CreditScoreId, CreditScoreCustomerName, CreditScoreIDProof, CreditScoreIDProofNumber, IsFastlaneFlow, SelectedQuoteVersion);
            }
            else if (e.CommandName == "QuoteNumber")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                //GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //int rowIndex = rowSelect.RowIndex;

                //int QuoteVersion = int.Parse(rowSelect.Cells[1].Text);
                //hdnQuoteVersion.Value = QuoteVersion.ToString();
                //hdnMaxQuoteVersion.Value = QuoteVersion.ToString(); //THIS IS REQUIRED BECAUSE PDF NEEDS TO BE DOWNLOADED OF THE SAME VERSION;
                ////Reference the GridView Row.
                //GridViewRow row = QuoteGridView.Rows[rowIndex];

                ////Access Cell values.
                ////DateTime dtQuoteDate = Convert.ToDateTime(row.Cells[2].Text);
                ////btnOpenCustomerPopUp.Visible = dtQuoteDate.ToShortDateString() == DateTime.Now.ToShortDateString() ? true : false;
                //btnOpenCustomerPopUp.Visible = false;

                //GetResultXMLFromDB(e.CommandArgument.ToString(), ref isNewBusiness, ref NetPremium, ref ServiceTax, ref TotalPremium, ref RequestXML, ref ResultXML, ref MarketMovement, ref CreditScoreId, ref CreditScoreCustomerName, ref CreditScoreIDProof, ref CreditScoreIDProofNumber, ref CGSTAmount, ref CGSTPercentage, ref SGSTAmount, ref SGSTPercentage, ref IGSTAmount, ref IGSTPercentage, ref UGSTAmount, ref UGSTPercentage, ref TotalGSTAmount, ref IsFastlaneFlow, ref QuoteVersion);
                //bool IsRollover = isNewBusiness ? false : true;
                //SetResultXMLValuesToPopUpLabel(false, ResultXML, e.CommandArgument.ToString(), NetPremium, ServiceTax, TotalPremium, IsRollover, CreditScoreId, CreditScoreCustomerName, CreditScoreIDProof, CreditScoreIDProofNumber, CGSTAmount, CGSTPercentage, SGSTAmount, SGSTPercentage, IGSTAmount, IGSTPercentage, UGSTAmount, UGSTPercentage, TotalGSTAmount, QuoteVersion.ToString(), RequestXML);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalViewPremium();", true);
            }
            else if (e.CommandName == "modifyquote")
            {
                //GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //LinkButton lnkModifyQuote = (LinkButton)e.CommandSource;
                //string QuoteNumber = lnkModifyQuote.CommandArgument;
                //int QuoteVersion = int.Parse(gvr.Cells[1].Text);
                //Response.Redirect("~/FrmPremiumCalculatorMotor.aspx?quotenumber=" + QuoteNumber + "&quoteversion=" + QuoteVersion);
            }
        }

        protected void btnSearchQuoteNumber_Click(object sender, EventArgs e)
        {
            QuoteGridView.DataBind();
        }
        #endregion


        protected void QuoteGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnProposalNumber = e.Row.FindControl("hdnProposalNumber") as HiddenField;
                LinkButton lnkRecalculate = e.Row.FindControl("lnkRecalculate") as LinkButton;
                LinkButton lnkModifyQuote = e.Row.FindControl("lnkModifyQuote") as LinkButton;
                HiddenField hdnIsProposalExistsForQuoteNumber = e.Row.FindControl("hdnIsProposalExistsForQuoteNumber") as HiddenField;

                if (hdnProposalNumber.Value.ToString().Trim().Length > 0)
                {
                    lnkRecalculate.Visible = false;
                    lnkModifyQuote.Visible = false;
                }

                if (hdnIsProposalExistsForQuoteNumber.Value.Trim() == "1")
                {
                    lnkRecalculate.Visible = false;
                    lnkModifyQuote.Visible = false;
                }
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetPincode(string prefix)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_PINCODE";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IntrCds.Add(string.Format("{0}~{1}", sdr["NUM_PINCODE"], sdr["TXT_PINCODE_LOCALITY"]));
                        }
                    }
                    conn.Close();
                }
            }
            return IntrCds.ToArray();
        }

        
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetUsers(string prefix)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_USERS_AUTO";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IntrCds.Add(string.Format("{0}~{1}", sdr["vUserLoginId"], sdr["vUserLoginDesc"]));
                        }

                    }
                    conn.Close();
                }
            }
            return IntrCds.ToArray();
        }


        protected void OboutAssignQuote_Click(object sender, EventArgs e)
        {
            DataTable dtAssignDetails = new DataTable();
            dtAssignDetails.Columns.Add(new DataColumn("QuoteNumber"));
            dtAssignDetails.Columns.Add(new DataColumn("AssignedTo"));
            dtAssignDetails.Columns.Add(new DataColumn("Remark"));
            dtAssignDetails.Columns.Add(new DataColumn("QuoteVersion"));
            DataRow dr;
            int i = 0;
            foreach (GridViewRow gvrow in QuoteGridView.Rows)
            {
                HiddenField field = (HiddenField)QuoteGridView.Rows[i].FindControl("hdnAssignedTo");
                TextBox txtAssign = (TextBox)QuoteGridView.Rows[i].FindControl("txtAssignedTo");
                TextBox txtRema = (TextBox)QuoteGridView.Rows[i].FindControl("txtRemark");
                CheckBox chk = (CheckBox)gvrow.FindControl("chkRow");
                if (chk != null & chk.Checked)
                {
                    dr = dtAssignDetails.NewRow();
                    dr[0] = gvrow.Cells[1].Text;
                    dr[1] = field.Value.ToString();
                    dr[2] = txtRema.Text;
                    dr[3] = gvrow.Cells[2].Text;

                    if (!string.IsNullOrEmpty(txtRema.Text) && txtAssign.Text.Contains(field.Value.ToString()) && !string.IsNullOrEmpty(txtAssign.Text.Trim()) && !string.IsNullOrEmpty(field.Value.ToString()))
                    {
                        dtAssignDetails.Rows.Add(dr);
                    }
                    else
                    {
                        Alert.Show("Please select Assignee and Remarks for the selected quotes.");
                    }
                }
                i++;
            }

            fnAddReassignDetails(dtAssignDetails);


        }

        private void fnAddReassignDetails(DataTable dtAssignDetails)
        {
            try
            {
                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand("PRC_INSERT_QUOTE_REASSIGNMENT_DETAILS"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@TBL_QUOTE_REASSIGNMENT_DETAILS", dtAssignDetails);
                        cmd.Parameters.AddWithValue("@vAssignedBy", Session["vUserLoginId"].ToString());
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        // Alert.Show("Quotes assigned successfuly", "FrmQuoteReassignment.aspx");
                        Alert.Show("Quotes assigned successfuly");
                        QuoteGridView.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}