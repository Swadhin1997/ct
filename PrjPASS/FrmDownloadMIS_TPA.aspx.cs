using Microsoft.Practices.EnterpriseLibrary.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace PrjPASS
{
    public partial class FrmDownloadMIS_TPA : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();


        public string logfile = "log_DownloadMIS_TPA" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
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

        protected void btnDownloadMIS_Click(object sender, EventArgs e)
        {
            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "entered into btnDownloadMIS_Click :- " + DateTime.Now + Environment.NewLine);
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            if (Session["UploadedFile"] == null)
            {
                string vStatusMsg = "Please Upload the file first.";
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
            }

            string conString = string.Empty;

            string extension = Path.GetExtension(Session["UploadedFile"].ToString());

            switch (extension)
            {

                case ".xls": //Excel 97-03

                    conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;

                    break;

                case ".xlsx": //Excel 07 or higher

                    conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;

                    break;

            }

            string UploadedFilePath = Server.MapPath("~/Uploads/") + Path.GetFileName(Session["UploadedFile"].ToString());

            conString = string.Format(conString, UploadedFilePath);

            using (OleDbConnection excel_con = new OleDbConnection(conString))
            {

                excel_con.Open();

                string sheet1 = "TPA_MIS_SHEET$";

                DataTable dtExcelData = new DataTable();

                using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                {
                    oda.Fill(dtExcelData);
                }

                excel_con.Close();


                if (dtExcelData.Rows.Count > 0)
                {
                    string arrresult = string.Empty;

                    var rowsWithParent = dtExcelData.AsEnumerable().Where(r => r["Certificate No"] == null || r["Certificate No"].ToString() == "");

                    //dtExcelData.Select("email is NULL or email=''")

                    if (rowsWithParent.Count() >= 0)
                    {
                        DataRow[] rowtest = dtExcelData.Select();
                        string[] excelUploadCertificateNo = Array.ConvertAll(rowtest, row => row["Certificate No"].ToString());
                        arrresult = string.Join(",", excelUploadCertificateNo);
                        bool IsDownload = false;
                        string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                        //CR_P1_633_Start MIS requirement for Paramount TPA
                        DataTable dt = new DataTable();
                        DataTable dataTable = new DataTable();
                        DataTable dtr = new DataTable();
                        DataSet dsTPAMIS = new DataSet();
                        string tpaCPCertificateNo;
                        //string []  excelUploadCertficateNo ;
                        List<string> excelGSTUploadCertficateNo = new List<string>();
                        bool resetloop = false;
                        //CR_P1_633_End MIS requirement for Paramount TPA
                        try
                        {
                            using (SqlConnection sqlCon = new SqlConnection(consString))
                            {
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    cmd.CommandText = "PROC_GET_TPA_MIS_DOWNLOAD";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@vCertificateNo", arrresult);
                                    cmd.Connection = sqlCon;
                                    sqlCon.Open();
                                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                                    //CR_Start HDC Policy
                                    //DataTable dt = new DataTable();

                                    //  da.Fill(dt);
                                    da.Fill(dsTPAMIS);
                                    da.Fill(dt);
                                    //var k = dt;
                                    //k.Select(arrresult);
                                    sqlCon.Close();
                                }

                            }
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < excelUploadCertificateNo.Length; i++)
                                {
                                    int j;
                                    //for ( j = 0; j < dsTPAMIS.Tables[0].Rows.Count; j++)
                                    for (j = 0; j < dt.Rows.Count; j++)
                                    {
                                        try
                                        {

                                            if (excelUploadCertificateNo[i].Contains(dt.Rows[j]["vCertificateNo"].ToString()))
                                            {

                                                resetloop = true;
                                                break;
                                            }
                                            //else if (resetloop == false)
                                            //{
                                            //    continue;
                                            //}
                                        }
                                        catch (Exception ex)
                                        {
                                            var error = ex.Message;
                                            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnDownloadGPAPolicy ::Exception Occured   " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                                        }
                                    }

                                    if (j == dt.Rows.Count)
                                    {
                                        using (SqlConnection cons = new SqlConnection(consString))
                                        {
                                            //tpaCPCertificateNo = idlist[i];
                                            excelGSTUploadCertficateNo.Add(excelUploadCertificateNo[i]);
                                            var gistcertificateno = string.Join(",", excelGSTUploadCertficateNo);
                                        }
                                    }
                                }
                                //Check GST certificateNo greater than zero
                                if (excelGSTUploadCertficateNo.Count > 0)
                                {
                                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "Entered into GISTView :: " + DateTime.Now + Environment.NewLine);
                                    using (SqlConnection cons = new SqlConnection(consString))
                                    {
                                        SqlCommand gistcmd = new SqlCommand();
                                        var exceluploadedcertificate = string.Join(",", excelGSTUploadCertficateNo);

                                        gistcmd.CommandText = "PROC_GET_TPA_MIS_FROMGIST_DOWNLOAD";
                                        gistcmd.CommandType = CommandType.StoredProcedure;
                                        gistcmd.Parameters.AddWithValue("@vCertificateNo", exceluploadedcertificate);
                                        gistcmd.Connection = cons;
                                        cons.Open();
                                        SqlDataAdapter sd = new SqlDataAdapter(gistcmd);
                                        sd.Fill(dataTable);

                                        if (dataTable.Rows.Count > 0)
                                        {
                                            //for (int i = 0; i < k.Length; i++)
                                            //{
                                            //    int j;
                                            //    //for ( j = 0; j < dsTPAMIS.Tables[0].Rows.Count; j++)
                                            //    for (j = 0; j < dataTable.Rows.Count; j++)
                                            //    {
                                            //        try
                                            //        {

                                        //            if (k[i].Contains(dt.Rows[j]["vCertificateNo"].ToString()))
                                        //            {

                                        //                resetloop = true;
                                        //                break;
                                        //            }
                                        //            //else if (resetloop == false)
                                        //            //{
                                        //            //    continue;
                                        //            //}
                                        //        }
                                        //        catch (Exception ex)
                                        //        {
                                        //            var error = ex.Message;
                                        //        }
                                        //    }

                                        //    if (j == dt.Rows.Count)
                                        //    {
                                        //        using (SqlConnection cons = new SqlConnection(consString))
                                        //        {
                                        //            //tpaCPCertificateNo = idlist[i];
                                        //            excelGSTUploadCertficateNo.Add(excelUploadCertificateNo[i]);
                                        //            var gistcertificateno = string.Join(",", excelGSTUploadCertficateNo);
                                        //        }
                                        //    }
                                        //}

                                        #region gist view cloumn created

                                        dtr.Columns.Add(new DataColumn("PRODUCT NAME", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("TRANSACTION TYPE", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Business Type", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("KGI Branch Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("KGI Branch Code", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("vUINno", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Intermediary Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Intermediary Code", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Master Policy Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Master Policy Holder Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Group Type", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Proposal Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Plan Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Plan Code", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Policy Issue Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Channel Type", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Proposal Type", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("TPA Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("TPA Code", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Policy Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Transaction Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Policy Start Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Policy End Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Policy Tenure", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Policy Type", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Customer Type", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Bank/ Financier Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Proposer NAME", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Proposer Address Line 1", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Proposer Address Line 2", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Proposer Address Line 3", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Proposer City", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Proposer State", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Proposer Pin Code", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Mobile No", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Email ID", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Total Policy Sum Insured", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Deductible Base Cover", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Hospital Daily Cash Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Accident Daily Cash Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("ICU Daily Cash Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Convalescence Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Companion Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Joint Hospitalisation", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Parent Accommodation", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Maternity Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("New Born Baby Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("AYUSH Treatment Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Worldwide Cover", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Day Care Procedure Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Surgery Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Accidental Hospitalisation Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Broken Bones", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Burns", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Personal Accident Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Critical Illness Benefit", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 1", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 2", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 3", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 4", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 5", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 6", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 7", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 8", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 9", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Fixed Condition 10", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Optional Condition 1", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Optional Condition 2", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Optional Condition 3", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Optional Condition 4", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Optional Condition 5", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Optional Condition 6", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Lives Count", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Relationship with Prosper", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Age", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Member ID", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Member ID Medical Declaration/Injury Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 PED Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Member Entry Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Member Exit Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Gender", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Unique Identification Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 CRN/Account No.", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Nominee Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Nominee Relationship with Proposer", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 1 Nominee DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Relationship with Prosper", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Age", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Member ID", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Member ID Medical Declaration/Injury Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 PED Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Member Entry Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Member Exit Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Gender", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Unique Identification Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 CRN/Account No.", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Nominee Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Nominee Relationship with Proposer", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 2 Nominee DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Relationship with Prosper", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Age", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Member ID", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Member ID Medical Declaration/Injury Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 PED Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Member Entry Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Member Exit Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Gender", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Unique Identification Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 CRN/Account No.", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Nominee Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Nominee Relationship with Proposer", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 3 Nominee DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Relationship with Prosper", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Age", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Member ID", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Member ID Medical Declaration/Injury Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 PED Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Member Entry Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Member Exit Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Gender", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Unique Identification Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 CRN/Account No.", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Nominee Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Nominee Relationship with Proposer", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 4 Nominee DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Relationship with Prosper", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Age", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Member ID", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Member ID Medical Declaration/Injury Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 PED Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Member Entry Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Member Exit Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Gender", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Unique Identification Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 CRN/Account No.", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Nominee Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Nominee Relationship with Proposer", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 5 Nominee DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Relationship with Prosper", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Age", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Member ID", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Member ID Medical Declaration/Injury Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 PED Details", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Member Entry Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Member Exit Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Gender", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Unique Identification Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 CRN/Account No.", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Nominee Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Nominee Relationship with Proposer", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Insured 6 Nominee DOB", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Installment Frequency", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Total no. of Installments", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Cover Section Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Endorsement Type", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Endorsement Remarks", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Endorsement Effective Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Endorsement Issue Date", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Endorsement Number", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Endorsement Name", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Endorsement/Cancellation Initiation", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Reason for Cancellation", typeof(string)));
                                        dtr.Columns.Add(new DataColumn("Description for Cancellation", typeof(string)));
                                        #endregion gist view column created
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            DataRow dr = dtr.NewRow();

                                            dr["PRODUCT NAME"] = dataRow["PRODUCT NAME"].ToString();
                                            dr["TRANSACTION TYPE"] = dataRow["TRANSACTION TYPE"].ToString();
                                            dr["Business Type"] = dataRow["Business Type"].ToString();
                                            dr["KGI Branch Name"] = dataRow["KGI Branch Name"].ToString();
                                            dr["KGI Branch Code"] = dataRow["KGI Branch Code"].ToString();
                                            dr["vUINno"] = dataRow["vUINno"].ToString();
                                            dr["Intermediary Name"] = dataRow["Intermediary Name"].ToString();
                                            dr["Intermediary Code"] = dataRow["Intermediary Code"].ToString();
                                            dr["Master Policy Number"] = dataRow["Master Policy Number"].ToString();
                                            dr["Master Policy Holder Name"] = dataRow["Master Policy Holder Name"].ToString();
                                            dr["Group Type"] = dataRow["Group Type"].ToString();
                                            dr["Proposal Number"] = dataRow["Proposal Number"].ToString();
                                            dr["Plan Name"] = dataRow["Plan Name"].ToString();
                                            dr["Plan Code"] = dataRow["Plan Code"].ToString();
                                            dr["Policy Issue Date"] = Convert.ToDateTime(dataRow["Policy Issue Date"]).ToString("dd/MM/yyyy");
                                            dr["Channel Type"] = dataRow["Channel Type"].ToString();
                                            dr["Proposal Type"] = dataRow["Proposal Type"].ToString();
                                            dr["TPA Name"] = dataRow["TPA Name"].ToString();
                                            dr["TPA Code"] = dataRow["TPA Code"].ToString();
                                            dr["Policy Number"] = dataRow["Policy Number"].ToString();
                                            dr["Transaction Date"] = Convert.ToDateTime(dataRow["Transaction Date"]).ToString("dd/MMM/yyyy");
                                            dr["Policy Start Date"] = Convert.ToDateTime(dataRow["Policy Start Date"]).ToString("dd/MMM/yyyy").Replace("/", "-");
                                            dr["Policy End Date"] = Convert.ToDateTime(dataRow["Policy End Date"]).ToString("dd/MMM/yyyy");
                                            dr["Policy Tenure"] = dataRow["Policy Tenure"].ToString();
                                            dr["Policy Type"] = dataRow["Policy Type"].ToString();
                                            dr["Customer Type"] = dataRow["Customer Type"].ToString();
                                            dr["Bank/ Financier Name"] = dataRow["Bank/ Financier Name"].ToString();
                                            dr["Proposer NAME"] = dataRow["Proposer NAME"].ToString();
                                            dr["Proposer Address Line 1"] = dataRow["Proposer Address Line 1"].ToString();
                                            dr["Proposer Address Line 2"] = dataRow["Proposer Address Line 2"].ToString();
                                            dr["Proposer Address Line 3"] = dataRow["Proposer Address Line 3"].ToString();
                                            dr["Proposer City"] = dataRow["Proposer City"].ToString();
                                            dr["Proposer State"] = dataRow["Proposer State"].ToString();
                                            dr["Proposer Pin Code"] = dataRow["Proposer Pin Code"].ToString();
                                            dr["Mobile No"] = dataRow["Mobile No"].ToString();
                                            dr["Email ID"] = dataRow["Email ID"].ToString();
                                            dr["Total Policy Sum Insured"] = dataRow["Total Policy Sum Insured"].ToString();
                                            // dr["Deductible Base Cover"] = dataRow["Deductible Base Cover"].ToString();
                                            dr["Hospital Daily Cash Benefit"] = dataRow["Hospital Daily Cash Benefit"].ToString();
                                            dr["Accident Daily Cash Benefit"] = dataRow["Accident Daily Cash Benefit"].ToString();
                                            dr["ICU Daily Cash Benefit"] = dataRow["ICU Daily Cash Benefit"].ToString();
                                            dr["Convalescence Benefit"] = dataRow["Convalescence Benefit"].ToString();
                                            dr["Companion Benefit"] = dataRow["Companion Benefit"].ToString();
                                            dr["Joint Hospitalisation"] = dataRow["Joint Hospitalisation"].ToString();
                                            dr["Parent Accommodation"] = dataRow["Parent Accommodation"].ToString();
                                            dr["Maternity Benefit"] = dataRow["Maternity Benefit"].ToString();
                                            dr["New Born Baby Benefit"] = dataRow["New Born Baby Benefit"].ToString();
                                            dr["AYUSH Treatment Benefit"] = dataRow["AYUSH Treatment Benefit"].ToString();
                                            dr["Worldwide Cover"] = dataRow["Worldwide Cover"].ToString();
                                            dr["Day Care Procedure Benefit"] = dataRow["Day Care Procedure Benefit"].ToString();
                                            dr["Surgery Benefit"] = dataRow["Surgery Benefit"].ToString();
                                            dr["Accidental Hospitalisation Benefit"] = dataRow["Accidental Hospitalisation Benefit"].ToString();
                                            dr["Broken Bones"] = dataRow["Broken Bones"].ToString();
                                            dr["Burns"] = dataRow["Burns"].ToString();
                                            dr["Personal Accident Benefit"] = dataRow["Personal Accident Benefit"].ToString();
                                            dr["Critical Illness Benefit"] = dataRow["Critical Illness Benefit"].ToString();
                                            dr["Fixed Condition 1"] = dataRow["Fixed Condition 1"].ToString();
                                            dr["Fixed Condition 2"] = dataRow["Fixed Condition 2"].ToString();
                                            dr["Fixed Condition 3"] = dataRow["Fixed Condition 3"].ToString();
                                            dr["Fixed Condition 4"] = dataRow["Fixed Condition 4"].ToString();
                                            dr["Fixed Condition 5"] = dataRow["Fixed Condition 5"].ToString();
                                            dr["Fixed Condition 6"] = dataRow["Fixed Condition 6"].ToString();
                                            dr["Fixed Condition 7"] = dataRow["Fixed Condition 7"].ToString();
                                            dr["Fixed Condition 8"] = dataRow["Fixed Condition 8"].ToString();
                                            dr["Fixed Condition 9"] = dataRow["Fixed Condition 9"].ToString();
                                            dr["Fixed Condition 10"] = dataRow["Fixed Condition 10"].ToString();
                                            dr["Optional Condition 1"] = dataRow["Optional Condition 1"].ToString();
                                            dr["Optional Condition 2"] = dataRow["Optional Condition 2"].ToString();
                                            dr["Optional Condition 3"] = dataRow["Optional Condition 3"].ToString();
                                            dr["Optional Condition 4"] = dataRow["Optional Condition 4"].ToString();
                                            dr["Optional Condition 5"] = dataRow["Optional Condition 5"].ToString();
                                            dr["Optional Condition 6"] = dataRow["Optional Condition 6"].ToString();
                                            dr["Lives Count"] = dataRow["Lives Count"].ToString();
                                            dr["Insured 1 Name"] = dataRow["Insured 1 Name"].ToString();
                                            dr["Insured 1 Relationship with Prosper"] = dataRow["Insured 1 Relationship with Prosper"].ToString();
                                            dr["Insured 1 DOB"] = Convert.ToDateTime(dataRow["Insured 1 DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 1 Age"] = dataRow["Insured 1 Age"].ToString();
                                            dr["Insured 1 Member ID"] = dataRow["Insured 1 Member ID"].ToString();
                                            dr["Insured 1 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 1 Member ID Medical Declaration/Injury Details"].ToString();
                                            dr["Insured 1 PED Details"] = dataRow["Insured 1 PED Details"].ToString();
                                            dr["Insured 1 Member Entry Date"] = Convert.ToDateTime(dataRow["Insured 1 Member Entry Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 1 Member Exit Date"] = dataRow["Insured 1 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 1 Member Exit Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 1 Gender"] = dataRow["Insured 1 Gender"].ToString();
                                            dr["Insured 1 Unique Identification Number"] = dataRow["Insured 1 Unique Identification Number"].ToString();
                                            dr["Insured 1 CRN/Account No."] = dataRow["Insured 1 CRN/Account No."].ToString();
                                            dr["Insured 1 Nominee Name"] = dataRow["Insured 1 Nominee Name"].ToString();
                                            dr["Insured 1 Nominee Relationship with Proposer"] = dataRow["Insured 1 Nominee Relationship with Proposer"].ToString();
                                            dr["Insured 1 Nominee DOB"] = dataRow["Insured 1 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 1 Nominee DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 2 Name"] = dataRow["Insured 2 Name"].ToString();
                                            dr["Insured 2 Relationship with Prosper"] = dataRow["Insured 2 Relationship with Prosper"].ToString();
                                            dr["Insured 2 DOB"] = dataRow["Insured 2 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 2 DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 2 Age"] = dataRow["Insured 2 Age"].ToString();
                                            dr["Insured 2 Member ID"] = dataRow["Insured 2 Member ID"].ToString();
                                            dr["Insured 2 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 2 Member ID Medical Declaration/Injury Details"].ToString();
                                            dr["Insured 2 PED Details"] = dataRow["Insured 2 PED Details"].ToString();
                                            dr["Insured 2 Member Entry Date"] = dataRow["Insured 2 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 2 Member Entry Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 2 Member Exit Date"] = dataRow["Insured 2 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 2 Member Exit Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 2 Gender"] = dataRow["Insured 2 Gender"].ToString();
                                            dr["Insured 2 Unique Identification Number"] = dataRow["Insured 2 Unique Identification Number"].ToString();
                                            dr["Insured 2 CRN/Account No."] = dataRow["Insured 2 CRN/Account No."].ToString();
                                            dr["Insured 2 Nominee Name"] = dataRow["Insured 2 Nominee Name"].ToString();
                                            dr["Insured 2 Nominee Relationship with Proposer"] = dataRow["Insured 2 Nominee Relationship with Proposer"].ToString();
                                            dr["Insured 2 Nominee DOB"] = dataRow["Insured 2 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 2 Nominee DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 3 Name"] = dataRow["Insured 3 Name"].ToString();
                                            dr["Insured 3 Relationship with Prosper"] = dataRow["Insured 3 Relationship with Prosper"].ToString();
                                            dr["Insured 3 DOB"] = dataRow["Insured 3 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 3 DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 3 Age"] = dataRow["Insured 3 Age"].ToString();
                                            dr["Insured 3 Member ID"] = dataRow["Insured 3 Member ID"].ToString();
                                            dr["Insured 3 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 3 Member ID Medical Declaration/Injury Details"].ToString();
                                            dr["Insured 3 PED Details"] = dataRow["Insured 3 PED Details"].ToString();
                                            dr["Insured 3 Member Entry Date"] = dataRow["Insured 3 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 3 Member Entry Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 3 Member Exit Date"] = dataRow["Insured 3 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 3 Member Exit Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 3 Gender"] = dataRow["Insured 3 Gender"].ToString();
                                            dr["Insured 3 Unique Identification Number"] = dataRow["Insured 3 Unique Identification Number"].ToString();
                                            dr["Insured 3 CRN/Account No."] = dataRow["Insured 3 CRN/Account No."].ToString();
                                            dr["Insured 3 Nominee Name"] = dataRow["Insured 3 Nominee Name"].ToString();
                                            dr["Insured 3 Nominee Relationship with Proposer"] = dataRow["Insured 3 Nominee Relationship with Proposer"].ToString();
                                            dr["Insured 3 Nominee DOB"] = dataRow["Insured 3 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 3 Nominee DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 4 Name"] = dataRow["Insured 4 Name"].ToString();
                                            dr["Insured 4 Relationship with Prosper"] = dataRow["Insured 4 Relationship with Prosper"].ToString();
                                            dr["Insured 4 DOB"] = dataRow["Insured 4 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 4 DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 4 Age"] = dataRow["Insured 4 Age"].ToString();
                                            dr["Insured 4 Member ID"] = dataRow["Insured 4 Member ID"].ToString();
                                            dr["Insured 4 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 4 Member ID Medical Declaration/Injury Details"].ToString();
                                            dr["Insured 4 PED Details"] = dataRow["Insured 4 PED Details"].ToString();
                                            dr["Insured 4 Member Entry Date"] = dataRow["Insured 4 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 4 Member Entry Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 4 Member Exit Date"] = dataRow["Insured 4 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 4 Member Exit Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 4 Gender"] = dataRow["Insured 4 Gender"].ToString();
                                            dr["Insured 4 Unique Identification Number"] = dataRow["Insured 4 Unique Identification Number"].ToString();
                                            dr["Insured 4 CRN/Account No."] = dataRow["Insured 4 CRN/Account No."].ToString();
                                            dr["Insured 4 Nominee Name"] = dataRow["Insured 4 Nominee Name"].ToString();
                                            dr["Insured 4 Nominee Relationship with Proposer"] = dataRow["Insured 4 Nominee Relationship with Proposer"].ToString();
                                            dr["Insured 4 Nominee DOB"] = dataRow["Insured 4 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 4 Nominee DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 5 Name"] = dataRow["Insured 5 Name"].ToString();
                                            dr["Insured 5 Relationship with Prosper"] = dataRow["Insured 5 Relationship with Prosper"].ToString();
                                            dr["Insured 5 DOB"] = dataRow["Insured 5 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 5 DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 5 Age"] = dataRow["Insured 5 Age"].ToString();
                                            dr["Insured 5 Member ID"] = dataRow["Insured 5 Member ID"].ToString();
                                            dr["Insured 5 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 5 Member ID Medical Declaration/Injury Details"].ToString();
                                            dr["Insured 5 PED Details"] = dataRow["Insured 5 PED Details"].ToString();
                                            dr["Insured 5 Member Entry Date"] = dataRow["Insured 5 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 5 Member Entry Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 5 Member Exit Date"] = dataRow["Insured 5 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 5 Member Exit Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 5 Gender"] = dataRow["Insured 5 Gender"].ToString();
                                            dr["Insured 5 Unique Identification Number"] = dataRow["Insured 5 Unique Identification Number"].ToString();
                                            dr["Insured 5 CRN/Account No."] = dataRow["Insured 5 CRN/Account No."].ToString();
                                            dr["Insured 5 Nominee Name"] = dataRow["Insured 5 Nominee Name"].ToString();
                                            dr["Insured 5 Nominee Relationship with Proposer"] = dataRow["Insured 5 Nominee Relationship with Proposer"].ToString();
                                            dr["Insured 5 Nominee DOB"] = dataRow["Insured 5 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 5 Nominee DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 6 Name"] = dataRow["Insured 6 Name"].ToString();
                                            dr["Insured 6 Relationship with Prosper"] = dataRow["Insured 6 Relationship with Prosper"].ToString();
                                            dr["Insured 6 DOB"] = dataRow["Insured 6 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 6 DOB"]).ToString("dd-MM-yyyy");
                                            dr["Insured 6 Age"] = dataRow["Insured 6 Age"].ToString();
                                            dr["Insured 6 Member ID"] = dataRow["Insured 6 Member ID"].ToString();
                                            dr["Insured 6 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 6 Member ID Medical Declaration/Injury Details"].ToString();
                                            dr["Insured 6 PED Details"] = dataRow["Insured 6 PED Details"].ToString();
                                            dr["Insured 6 Member Entry Date"] = dataRow["Insured 6 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 6 Member Entry Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 6 Member Exit Date"] = dataRow["Insured 6 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 6 Member Exit Date"]).ToString("dd-MM-yyyy");
                                            dr["Insured 6 Gender"] = dataRow["Insured 6 Gender"].ToString();
                                            dr["Insured 6 Unique Identification Number"] = dataRow["Insured 6 Unique Identification Number"].ToString();
                                            dr["Insured 6 CRN/Account No."] = dataRow["Insured 6 CRN/Account No."].ToString();
                                            dr["Insured 6 Nominee Name"] = dataRow["Insured 6 Nominee Name"].ToString();
                                            dr["Insured 6 Nominee Relationship with Proposer"] = dataRow["Insured 6 Nominee Relationship with Proposer"].ToString();
                                            dr["Insured 6 Nominee DOB"] = dataRow["Insured 6 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 6 Nominee DOB"]).ToString("dd-MM-yyyy");
                                            dr["Installment Frequency"] = dataRow["Installment Frequency"].ToString();
                                            dr["Total no. of Installments"] = dataRow["Total no. of Installments"].ToString();
                                            dr["Cover Section Name"] = dataRow["Cover Section Name"].ToString();
                                            dr["Endorsement Type"] = dataRow["Endorsement Type"].ToString();
                                            dr["Endorsement Remarks"] = dataRow["Endorsement Remarks"].ToString();
                                            dr["Endorsement Effective Date"] = dataRow["Endorsement Effective Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Endorsement Effective Date"]).ToString("dd-MM-yyyy");
                                            dr["Endorsement Issue Date"] = dataRow["Endorsement Issue Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Endorsement Issue Date"]).ToString("dd-MM-yyyy");
                                            dr["Endorsement Number"] = dataRow["Endorsement Number"].ToString();
                                            dr["Endorsement Name"] = dataRow["Endorsement Name"].ToString();
                                            dr["Endorsement/Cancellation Initiation"] = dataRow["Endorsement/Cancellation Initiation"].ToString();
                                            dr["Reason for Cancellation"] = dataRow["Reason for Cancellation"].ToString();
                                            dr["Description for Cancellation"] = dataRow["Description for Cancellation"].ToString();

                                            //Adding Row to datatable 'DTR'
                                            dtr.Rows.Add(dr);
                                        }
                                        // dtr.Rows.Add(gistTpaColumn);
                                        // string ProductName = dataTable.Rows[0]["Product Name"].ToString();
                                        //string Policy_Issue_Date = dataTable.Rows[1]["Policy Issue Date"].ToString();
                                    }

                                    
                                    cons.Close();
                                    //CR_End HDC Policy
                                    //}
                                }
                            }
                           

                            }
                            else if (dt.Rows.Count <= 0)
                            {
                                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnDownloadMIS_CLICK ::Entered into if condition if no reords found in HDC table " + DateTime.Now + Environment.NewLine);
                                using (SqlConnection con = new SqlConnection(consString))
                                {
                                    using (SqlCommand gistcmd = new SqlCommand())
                                    {
                                  
                                        gistcmd.CommandText = "PROC_GET_TPA_MIS_FROMGIST_DOWNLOAD";
                                        gistcmd.CommandType = CommandType.StoredProcedure;
                                        gistcmd.Parameters.AddWithValue("@vCertificateNo", arrresult);
                                  
                                        gistcmd.Connection = con;
                                        con.Open();
                                        SqlDataAdapter sd = new SqlDataAdapter(gistcmd);
                                      
                                        sd.Fill(dataTable);
                                        con.Close();
                                        if (dataTable.Rows.Count > 0)
                                        {
                                            #region GistData column
                                            dtr.Columns.Add(new DataColumn("PRODUCT NAME", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("TRANSACTION TYPE", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Business Type", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("KGI Branch Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("KGI Branch Code", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("vUINno", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Intermediary Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Intermediary Code", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Master Policy Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Master Policy Holder Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Group Type", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Proposal Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Plan Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Plan Code", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Policy Issue Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Channel Type", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Proposal Type", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("TPA Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("TPA Code", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Policy Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Transaction Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Policy Start Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Policy End Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Policy Tenure", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Policy Type", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Customer Type", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Bank/ Financier Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Proposer NAME", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Proposer Address Line 1", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Proposer Address Line 2", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Proposer Address Line 3", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Proposer City", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Proposer State", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Proposer Pin Code", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Mobile No", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Email ID", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Total Policy Sum Insured", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Deductible Base Cover", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Hospital Daily Cash Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Accident Daily Cash Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("ICU Daily Cash Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Convalescence Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Companion Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Joint Hospitalisation", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Parent Accommodation", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Maternity Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("New Born Baby Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("AYUSH Treatment Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Worldwide Cover", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Day Care Procedure Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Surgery Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Accidental Hospitalisation Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Broken Bones", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Burns", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Personal Accident Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Critical Illness Benefit", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 1", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 2", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 3", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 4", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 5", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 6", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 7", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 8", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 9", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Fixed Condition 10", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Optional Condition 1", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Optional Condition 2", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Optional Condition 3", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Optional Condition 4", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Optional Condition 5", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Optional Condition 6", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Lives Count", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Relationship with Prosper", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Age", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Member ID", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Member ID Medical Declaration/Injury Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 PED Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Member Entry Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Member Exit Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Gender", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Unique Identification Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 CRN/Account No.", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Nominee Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Nominee Relationship with Proposer", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 1 Nominee DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Relationship with Prosper", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Age", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Member ID", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Member ID Medical Declaration/Injury Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 PED Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Member Entry Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Member Exit Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Gender", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Unique Identification Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 CRN/Account No.", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Nominee Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Nominee Relationship with Proposer", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 2 Nominee DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Relationship with Prosper", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Age", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Member ID", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Member ID Medical Declaration/Injury Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 PED Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Member Entry Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Member Exit Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Gender", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Unique Identification Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 CRN/Account No.", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Nominee Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Nominee Relationship with Proposer", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 3 Nominee DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Relationship with Prosper", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Age", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Member ID", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Member ID Medical Declaration/Injury Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 PED Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Member Entry Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Member Exit Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Gender", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Unique Identification Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 CRN/Account No.", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Nominee Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Nominee Relationship with Proposer", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 4 Nominee DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Relationship with Prosper", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Age", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Member ID", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Member ID Medical Declaration/Injury Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 PED Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Member Entry Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Member Exit Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Gender", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Unique Identification Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 CRN/Account No.", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Nominee Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Nominee Relationship with Proposer", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 5 Nominee DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Relationship with Prosper", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Age", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Member ID", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Member ID Medical Declaration/Injury Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 PED Details", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Member Entry Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Member Exit Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Gender", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Unique Identification Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 CRN/Account No.", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Nominee Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Nominee Relationship with Proposer", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Insured 6 Nominee DOB", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Installment Frequency", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Total no. of Installments", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Cover Section Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Endorsement Type", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Endorsement Remarks", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Endorsement Effective Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Endorsement Issue Date", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Endorsement Number", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Endorsement Name", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Endorsement/Cancellation Initiation", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Reason for Cancellation", typeof(string)));
                                            dtr.Columns.Add(new DataColumn("Description for Cancellation", typeof(string)));
                                            #endregion gist data
                                            foreach (DataRow dataRow in dataTable.Rows)
                                            {
                                                DataRow dr = dtr.NewRow();

                                                dr["PRODUCT NAME"] = dataRow["PRODUCT NAME"].ToString();
                                                dr["TRANSACTION TYPE"] = dataRow["TRANSACTION TYPE"].ToString();
                                                dr["Business Type"] = dataRow["Business Type"].ToString();
                                                dr["KGI Branch Name"] = dataRow["KGI Branch Name"].ToString();
                                                dr["KGI Branch Code"] = dataRow["KGI Branch Code"].ToString();
                                                dr["vUINno"] = dataRow["vUINno"].ToString();
                                                dr["Intermediary Name"] = dataRow["Intermediary Name"].ToString();
                                                dr["Intermediary Code"] = dataRow["Intermediary Code"].ToString();
                                                dr["Master Policy Number"] = dataRow["Master Policy Number"].ToString();
                                                dr["Master Policy Holder Name"] = dataRow["Master Policy Holder Name"].ToString();
                                                dr["Group Type"] = dataRow["Group Type"].ToString();
                                                dr["Proposal Number"] = dataRow["Proposal Number"].ToString();
                                                dr["Plan Name"] = dataRow["Plan Name"].ToString();
                                                dr["Plan Code"] = dataRow["Plan Code"].ToString();
                                                dr["Policy Issue Date"] = Convert.ToDateTime(dataRow["Policy Issue Date"]).ToString("dd/MM/yyyy");
                                                dr["Channel Type"] = dataRow["Channel Type"].ToString();
                                                dr["Proposal Type"] = dataRow["Proposal Type"].ToString();
                                                dr["TPA Name"] = dataRow["TPA Name"].ToString();
                                                dr["TPA Code"] = dataRow["TPA Code"].ToString();
                                                dr["Policy Number"] = dataRow["Policy Number"].ToString();
                                                dr["Transaction Date"] = Convert.ToDateTime(dataRow["Transaction Date"]).ToString("dd/MMM/yyyy");
                                                dr["Policy Start Date"] = Convert.ToDateTime(dataRow["Policy Start Date"]).ToString("dd/MMM/yyyy").Replace("/", "-");
                                                dr["Policy End Date"] = Convert.ToDateTime(dataRow["Policy End Date"]).ToString("dd/MMM/yyyy");
                                                dr["Policy Tenure"] = dataRow["Policy Tenure"].ToString();
                                                dr["Policy Type"] = dataRow["Policy Type"].ToString();
                                                dr["Customer Type"] = dataRow["Customer Type"].ToString();
                                                dr["Bank/ Financier Name"] = dataRow["Bank/ Financier Name"].ToString();
                                                dr["Proposer NAME"] = dataRow["Proposer NAME"].ToString();
                                                dr["Proposer Address Line 1"] = dataRow["Proposer Address Line 1"].ToString();
                                                dr["Proposer Address Line 2"] = dataRow["Proposer Address Line 2"].ToString();
                                                dr["Proposer Address Line 3"] = dataRow["Proposer Address Line 3"].ToString();
                                                dr["Proposer City"] = dataRow["Proposer City"].ToString();
                                                dr["Proposer State"] = dataRow["Proposer State"].ToString();
                                                dr["Proposer Pin Code"] = dataRow["Proposer Pin Code"].ToString();
                                                dr["Mobile No"] = dataRow["Mobile No"].ToString();
                                                dr["Email ID"] = dataRow["Email ID"].ToString();
                                                dr["Total Policy Sum Insured"] = dataRow["Total Policy Sum Insured"].ToString();
                                                // dr["Deductible Base Cover"] = dataRow["Deductible Base Cover"].ToString();
                                                dr["Hospital Daily Cash Benefit"] = dataRow["Hospital Daily Cash Benefit"].ToString();
                                                dr["Accident Daily Cash Benefit"] = dataRow["Accident Daily Cash Benefit"].ToString();
                                                dr["ICU Daily Cash Benefit"] = dataRow["ICU Daily Cash Benefit"].ToString();
                                                dr["Convalescence Benefit"] = dataRow["Convalescence Benefit"].ToString();
                                                dr["Companion Benefit"] = dataRow["Companion Benefit"].ToString();
                                                dr["Joint Hospitalisation"] = dataRow["Joint Hospitalisation"].ToString();
                                                dr["Parent Accommodation"] = dataRow["Parent Accommodation"].ToString();
                                                dr["Maternity Benefit"] = dataRow["Maternity Benefit"].ToString();
                                                dr["New Born Baby Benefit"] = dataRow["New Born Baby Benefit"].ToString();
                                                dr["AYUSH Treatment Benefit"] = dataRow["AYUSH Treatment Benefit"].ToString();
                                                dr["Worldwide Cover"] = dataRow["Worldwide Cover"].ToString();
                                                dr["Day Care Procedure Benefit"] = dataRow["Day Care Procedure Benefit"].ToString();
                                                dr["Surgery Benefit"] = dataRow["Surgery Benefit"].ToString();
                                                dr["Accidental Hospitalisation Benefit"] = dataRow["Accidental Hospitalisation Benefit"].ToString();
                                                dr["Broken Bones"] = dataRow["Broken Bones"].ToString();
                                                dr["Burns"] = dataRow["Burns"].ToString();
                                                dr["Personal Accident Benefit"] = dataRow["Personal Accident Benefit"].ToString();
                                                dr["Critical Illness Benefit"] = dataRow["Critical Illness Benefit"].ToString();
                                                dr["Fixed Condition 1"] = dataRow["Fixed Condition 1"].ToString();
                                                dr["Fixed Condition 2"] = dataRow["Fixed Condition 2"].ToString();
                                                dr["Fixed Condition 3"] = dataRow["Fixed Condition 3"].ToString();
                                                dr["Fixed Condition 4"] = dataRow["Fixed Condition 4"].ToString();
                                                dr["Fixed Condition 5"] = dataRow["Fixed Condition 5"].ToString();
                                                dr["Fixed Condition 6"] = dataRow["Fixed Condition 6"].ToString();
                                                dr["Fixed Condition 7"] = dataRow["Fixed Condition 7"].ToString();
                                                dr["Fixed Condition 8"] = dataRow["Fixed Condition 8"].ToString();
                                                dr["Fixed Condition 9"] = dataRow["Fixed Condition 9"].ToString();
                                                dr["Fixed Condition 10"] = dataRow["Fixed Condition 10"].ToString();
                                                dr["Optional Condition 1"] = dataRow["Optional Condition 1"].ToString();
                                                dr["Optional Condition 2"] = dataRow["Optional Condition 2"].ToString();
                                                dr["Optional Condition 3"] = dataRow["Optional Condition 3"].ToString();
                                                dr["Optional Condition 4"] = dataRow["Optional Condition 4"].ToString();
                                                dr["Optional Condition 5"] = dataRow["Optional Condition 5"].ToString();
                                                dr["Optional Condition 6"] = dataRow["Optional Condition 6"].ToString();
                                                dr["Lives Count"] = dataRow["Lives Count"].ToString();
                                                dr["Insured 1 Name"] = dataRow["Insured 1 Name"].ToString();
                                                dr["Insured 1 Relationship with Prosper"] = dataRow["Insured 1 Relationship with Prosper"].ToString();
                                                dr["Insured 1 DOB"] = Convert.ToDateTime(dataRow["Insured 1 DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 1 Age"] = dataRow["Insured 1 Age"].ToString();
                                                dr["Insured 1 Member ID"] = dataRow["Insured 1 Member ID"].ToString();
                                                dr["Insured 1 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 1 Member ID Medical Declaration/Injury Details"].ToString();
                                                dr["Insured 1 PED Details"] = dataRow["Insured 1 PED Details"].ToString();
                                                dr["Insured 1 Member Entry Date"] = Convert.ToDateTime(dataRow["Insured 1 Member Entry Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 1 Member Exit Date"] = dataRow["Insured 1 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 1 Member Exit Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 1 Gender"] = dataRow["Insured 1 Gender"].ToString();
                                                dr["Insured 1 Unique Identification Number"] = dataRow["Insured 1 Unique Identification Number"].ToString();
                                                dr["Insured 1 CRN/Account No."] = dataRow["Insured 1 CRN/Account No."].ToString();
                                                dr["Insured 1 Nominee Name"] = dataRow["Insured 1 Nominee Name"].ToString();
                                                dr["Insured 1 Nominee Relationship with Proposer"] = dataRow["Insured 1 Nominee Relationship with Proposer"].ToString();
                                                dr["Insured 1 Nominee DOB"] = dataRow["Insured 1 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 1 Nominee DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 2 Name"] = dataRow["Insured 2 Name"].ToString();
                                                dr["Insured 2 Relationship with Prosper"] = dataRow["Insured 2 Relationship with Prosper"].ToString();
                                                dr["Insured 2 DOB"] = dataRow["Insured 2 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 2 DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 2 Age"] = dataRow["Insured 2 Age"].ToString();
                                                dr["Insured 2 Member ID"] = dataRow["Insured 2 Member ID"].ToString();
                                                dr["Insured 2 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 2 Member ID Medical Declaration/Injury Details"].ToString();
                                                dr["Insured 2 PED Details"] = dataRow["Insured 2 PED Details"].ToString();
                                                dr["Insured 2 Member Entry Date"] = dataRow["Insured 2 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 2 Member Entry Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 2 Member Exit Date"] = dataRow["Insured 2 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 2 Member Exit Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 2 Gender"] = dataRow["Insured 2 Gender"].ToString();
                                                dr["Insured 2 Unique Identification Number"] = dataRow["Insured 2 Unique Identification Number"].ToString();
                                                dr["Insured 2 CRN/Account No."] = dataRow["Insured 2 CRN/Account No."].ToString();
                                                dr["Insured 2 Nominee Name"] = dataRow["Insured 2 Nominee Name"].ToString();
                                                dr["Insured 2 Nominee Relationship with Proposer"] = dataRow["Insured 2 Nominee Relationship with Proposer"].ToString();
                                                dr["Insured 2 Nominee DOB"] = dataRow["Insured 2 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 2 Nominee DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 3 Name"] = dataRow["Insured 3 Name"].ToString();
                                                dr["Insured 3 Relationship with Prosper"] = dataRow["Insured 3 Relationship with Prosper"].ToString();
                                                dr["Insured 3 DOB"] = dataRow["Insured 3 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 3 DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 3 Age"] = dataRow["Insured 3 Age"].ToString();
                                                dr["Insured 3 Member ID"] = dataRow["Insured 3 Member ID"].ToString();
                                                dr["Insured 3 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 3 Member ID Medical Declaration/Injury Details"].ToString();
                                                dr["Insured 3 PED Details"] = dataRow["Insured 3 PED Details"].ToString();
                                                dr["Insured 3 Member Entry Date"] = dataRow["Insured 3 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 3 Member Entry Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 3 Member Exit Date"] = dataRow["Insured 3 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 3 Member Exit Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 3 Gender"] = dataRow["Insured 3 Gender"].ToString();
                                                dr["Insured 3 Unique Identification Number"] = dataRow["Insured 3 Unique Identification Number"].ToString();
                                                dr["Insured 3 CRN/Account No."] = dataRow["Insured 3 CRN/Account No."].ToString();
                                                dr["Insured 3 Nominee Name"] = dataRow["Insured 3 Nominee Name"].ToString();
                                                dr["Insured 3 Nominee Relationship with Proposer"] = dataRow["Insured 3 Nominee Relationship with Proposer"].ToString();
                                                dr["Insured 3 Nominee DOB"] = dataRow["Insured 3 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 3 Nominee DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 4 Name"] = dataRow["Insured 4 Name"].ToString();
                                                dr["Insured 4 Relationship with Prosper"] = dataRow["Insured 4 Relationship with Prosper"].ToString();
                                                dr["Insured 4 DOB"] = dataRow["Insured 4 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 4 DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 4 Age"] = dataRow["Insured 4 Age"].ToString();
                                                dr["Insured 4 Member ID"] = dataRow["Insured 4 Member ID"].ToString();
                                                dr["Insured 4 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 4 Member ID Medical Declaration/Injury Details"].ToString();
                                                dr["Insured 4 PED Details"] = dataRow["Insured 4 PED Details"].ToString();
                                                dr["Insured 4 Member Entry Date"] = dataRow["Insured 4 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 4 Member Entry Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 4 Member Exit Date"] = dataRow["Insured 4 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 4 Member Exit Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 4 Gender"] = dataRow["Insured 4 Gender"].ToString();
                                                dr["Insured 4 Unique Identification Number"] = dataRow["Insured 4 Unique Identification Number"].ToString();
                                                dr["Insured 4 CRN/Account No."] = dataRow["Insured 4 CRN/Account No."].ToString();
                                                dr["Insured 4 Nominee Name"] = dataRow["Insured 4 Nominee Name"].ToString();
                                                dr["Insured 4 Nominee Relationship with Proposer"] = dataRow["Insured 4 Nominee Relationship with Proposer"].ToString();
                                                dr["Insured 4 Nominee DOB"] = dataRow["Insured 4 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 4 Nominee DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 5 Name"] = dataRow["Insured 5 Name"].ToString();
                                                dr["Insured 5 Relationship with Prosper"] = dataRow["Insured 5 Relationship with Prosper"].ToString();
                                                dr["Insured 5 DOB"] = dataRow["Insured 5 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 5 DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 5 Age"] = dataRow["Insured 5 Age"].ToString();
                                                dr["Insured 5 Member ID"] = dataRow["Insured 5 Member ID"].ToString();
                                                dr["Insured 5 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 5 Member ID Medical Declaration/Injury Details"].ToString();
                                                dr["Insured 5 PED Details"] = dataRow["Insured 5 PED Details"].ToString();
                                                dr["Insured 5 Member Entry Date"] = dataRow["Insured 5 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 5 Member Entry Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 5 Member Exit Date"] = dataRow["Insured 5 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 5 Member Exit Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 5 Gender"] = dataRow["Insured 5 Gender"].ToString();
                                                dr["Insured 5 Unique Identification Number"] = dataRow["Insured 5 Unique Identification Number"].ToString();
                                                dr["Insured 5 CRN/Account No."] = dataRow["Insured 5 CRN/Account No."].ToString();
                                                dr["Insured 5 Nominee Name"] = dataRow["Insured 5 Nominee Name"].ToString();
                                                dr["Insured 5 Nominee Relationship with Proposer"] = dataRow["Insured 5 Nominee Relationship with Proposer"].ToString();
                                                dr["Insured 5 Nominee DOB"] = dataRow["Insured 5 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 5 Nominee DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 6 Name"] = dataRow["Insured 6 Name"].ToString();
                                                dr["Insured 6 Relationship with Prosper"] = dataRow["Insured 6 Relationship with Prosper"].ToString();
                                                dr["Insured 6 DOB"] = dataRow["Insured 6 DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 6 DOB"]).ToString("dd-MM-yyyy");
                                                dr["Insured 6 Age"] = dataRow["Insured 6 Age"].ToString();
                                                dr["Insured 6 Member ID"] = dataRow["Insured 6 Member ID"].ToString();
                                                dr["Insured 6 Member ID Medical Declaration/Injury Details"] = dataRow["Insured 6 Member ID Medical Declaration/Injury Details"].ToString();
                                                dr["Insured 6 PED Details"] = dataRow["Insured 6 PED Details"].ToString();
                                                dr["Insured 6 Member Entry Date"] = dataRow["Insured 6 Member Entry Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 6 Member Entry Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 6 Member Exit Date"] = dataRow["Insured 6 Member Exit Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 6 Member Exit Date"]).ToString("dd-MM-yyyy");
                                                dr["Insured 6 Gender"] = dataRow["Insured 6 Gender"].ToString();
                                                dr["Insured 6 Unique Identification Number"] = dataRow["Insured 6 Unique Identification Number"].ToString();
                                                dr["Insured 6 CRN/Account No."] = dataRow["Insured 6 CRN/Account No."].ToString();
                                                dr["Insured 6 Nominee Name"] = dataRow["Insured 6 Nominee Name"].ToString();
                                                dr["Insured 6 Nominee Relationship with Proposer"] = dataRow["Insured 6 Nominee Relationship with Proposer"].ToString();
                                                dr["Insured 6 Nominee DOB"] = dataRow["Insured 6 Nominee DOB"] is DBNull ? "" : Convert.ToDateTime(dataRow["Insured 6 Nominee DOB"]).ToString("dd-MM-yyyy");
                                                dr["Installment Frequency"] = dataRow["Installment Frequency"].ToString();
                                                dr["Total no. of Installments"] = dataRow["Total no. of Installments"].ToString();
                                                dr["Cover Section Name"] = dataRow["Cover Section Name"].ToString();
                                                dr["Endorsement Type"] = dataRow["Endorsement Type"].ToString();
                                                dr["Endorsement Remarks"] = dataRow["Endorsement Remarks"].ToString();
                                                dr["Endorsement Effective Date"] = dataRow["Endorsement Effective Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Endorsement Effective Date"]).ToString("dd-MM-yyyy");
                                                dr["Endorsement Issue Date"] = dataRow["Endorsement Issue Date"] is DBNull ? "" : Convert.ToDateTime(dataRow["Endorsement Issue Date"]).ToString("dd-MM-yyyy");
                                                dr["Endorsement Number"] = dataRow["Endorsement Number"].ToString();
                                                dr["Endorsement Name"] = dataRow["Endorsement Name"].ToString();
                                                dr["Endorsement/Cancellation Initiation"] = dataRow["Endorsement/Cancellation Initiation"].ToString();
                                                dr["Reason for Cancellation"] = dataRow["Reason for Cancellation"].ToString();
                                                dr["Description for Cancellation"] = dataRow["Description for Cancellation"].ToString();

                                            //Adding Row to datatable 'DTR'
                                            dtr.Rows.Add(dr);
                                        }
                                       
                                        //con.Close();
                                    }
                                      else
                                      {
                                        //con.Close();
                                        Alert.Show("No Record Found in the Gist table", "FrmMainMenu.aspx");
                                        return;
                                       
                                    }
                                    
                                        //CR_End HDC Policy
                                    }
                                }
                            }
                            else
                            {  // if no record in HDC main table as well as in GIST*****

                                string vErrorMsg = "Download Failed - Please provide correct certificate information";
                                Response.Redirect("ErrorPage.aspx?vErrorMsg =" + vErrorMsg, false);

                            }
                            DataSet ds = new DataSet();
                            string[] temp;
                  
                            ds.Tables.Add(dt);
                            ds.Tables.Add(dtr);
                        if (ds.Tables.Count > 0)
                        {
                                if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
                                {
                                    string filePath = Server.MapPath("~/Reports");

                                    string _DownloadableProductFileName = "TPA_MIS_DOWNLOAD_DUMP_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls";

                                    String strfilename = filePath + "\\" + _DownloadableProductFileName;

                                    if (System.IO.File.Exists(strfilename))
                                    {
                                        System.IO.File.Delete(strfilename);
                                    }
                                    foreach (DataTable dts in ds.Tables)
                                    {
                                        ExportDataTableToExcel(dts, "TPA_MIS_DOWNLOAD_DUMP_", strfilename);
                                    }
                                    IsDownload = DownloadFile(strfilename);
                                    //if (ExportDataTableToExcels(dt, "TPA_MIS_DOWNLOAD_DUMP_", strfilename) == true)
                                    //{
                                    //    IsDownload = DownloadFile(strfilename);
                                    //}
                                    //sqlCon.Close();

                                }
                                //CR_P1_633_End MIS requirement for Paramount TPA

                            }
                        //CR_P1_633_End MIS requirement for Paramount TPA


                             using (SqlConnection sqlConn = new SqlConnection(consString))
                            {
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    cmd.CommandText = "PROC_INSERT_MIS_DOWNLOAD_LOG";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@bIsDownload", IsDownload);
                                    cmd.Parameters.AddWithValue("@vUserloginID", Session["vUserLoginId"].ToString());
                                    cmd.Parameters.AddWithValue("@vFileName", Session["UploadedFile"].ToString());
                                    cmd.Connection = sqlConn;
                                    sqlConn.Open();
                                    cmd.ExecuteNonQuery();
                                    sqlConn.Close();
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnDownloadMIS_CLICK ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                            Response.Redirect("ErrorPage.aspx?vErrorMsg=" + ex.Message, false);
                            return;
                        }
                    }

                    else
                    {

                        string vStatusMsg = "Download Failed – Please provide certificate information";
                        File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnDownloadMIS_CLICK : StatusErrorMsg:-  " + vStatusMsg + DateTime.Now + Environment.NewLine);
                        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        return;
                    }

                }

            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            TemplateTable.Columns.Add("Certificate No");

            DataRow newBlankRow = TemplateTable.NewRow();
            TemplateTable.Rows.InsertAt(newBlankRow, 0);

            string filePath = Server.MapPath("~/Reports");

            string _DownloadableProductFileName = "TPA_MIS_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "TPA_MIS_SHEET", strfilename) == true)
            {
                DownloadFile(strfilename);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        private bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        {
            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "Entered into ExportDataTableToExcel ::  " + DateTime.Now + Environment.NewLine);
            //Creae an Excel application instance

            ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
            ExcelRange oRng;

            try
            {

                // Workk sheet
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                //excelSheet.Name = worksheetName;

                //CR_P1_633 Start TPA MIS requirement for Paramount TPA
                if (worksheetName == "TPA_MIS_SHEET")
                {
                    excelSheet.Name = worksheetName;
                }
                else
                {
                    if (dataTable.TableName == "Table1")
                    {
                        excelSheet.Name = "TPA_MIS_DOWNLOAD_DUMP";
                    }
                    else if (dataTable.TableName == "Table2" && dataTable.Rows.Count > 0)
                    {
                        excelSheet.Name = "TPA_MIS_GIST_DOWNLOAD_DUMP";
                        excelSheet.View.ShowGridLines = true;
                        excelSheet.DefaultRowHeight = 20;
                        excelSheet.Cells.Style.Font.Name = "Calibri";
                        excelSheet.Cells.Style.Font.Size = 9;
                    }


                }
                //CR_P1_633 Start TPA MIS requirement for Paramount TPA

                //excelSheet.Cells[1, 1] = "Detail";
                //excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

                // loop through each row and add values to our sheet
                int rowcount = 0;


                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;

                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 1)
                        {
                            excelSheet.Cells[1, i].Value = dataTable.Columns[i - 1].ColumnName;
                            // excelSheet.Cells.Font.Color = System.Drawing.Color.Black;

                            //CR_P1_633_Added Start
                            excelSheet.Cells[1, i].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            excelSheet.Cells[1, i].Style.Font.Bold = true;
                            //CR_P1_633_Added End
                        }

                        excelSheet.Cells[rowcount + 1, i].Value = datarow[i - 1].ToString();

                        //for alternate rows
                        if (rowcount > 1)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    oRng = (ExcelRange)excelSheet.Cells[rowcount, dataTable.Columns.Count];
                                    FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
                                }
                            }
                        }
                    }
                }

                // now we resize the columns
                oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
                oRng.AutoFitColumns();
                BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));

                oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
                // FormattingExcelCells(oRng, "#000099", System.Drawing.Color.White, true);


                //now save the workbook and exit Excel

                excelApp.Save();
                return true;
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "ExportDataTableToExcel ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Alert.Show(ex.Message);
                return false;
            }
            finally
            {
                excelApp = null;
            }
        }

        private void BorderAround(ExcelRange range, int colour)
        {
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.ColorTranslator.FromOle(colour));
        }
        public void FormattingExcelCells(ExcelRange range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.Indexed = 16;
            range.Style.Font.Size = 12;
            //range.Style.Font.Color = System.Drawing.Color.White;
            if (IsFontbool == true)
            {
                range.Style.Font.Bold = IsFontbool;
            }
        }

        public bool DownloadFile(string strfilename)
        {
            bool res = false;
            try
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "Entered into DownloadFile() :: " + DateTime.Now + Environment.NewLine);
                //bool res = false;
                string filePath = Server.MapPath("~/Reports");
                string _DownloadableProductFileName = strfilename;

            System.IO.FileInfo FileName = new System.IO.FileInfo(strfilename);
            FileStream myFile = new FileStream(strfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //Reads file as binary values
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            long startBytes = 0;
            string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
            string _EncodedData = HttpUtility.UrlEncode
                (_DownloadableProductFileName, Encoding.UTF8) + lastUpdateTiemStamp;

            //Clear the content of the response
            Response.Clear();
            Response.Buffer = false;
            Response.AddHeader("Accept-Ranges", "bytes");
            Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
            Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);

            //Set the ContentType
            Response.ContentType = "application/octet-stream";

            //Add the file name and attachment, 
            //which will force the open/cancel/save dialog to show, to the header
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName.Name);

            //Add the file size into the response header
            Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
            Response.AddHeader("Connection", "Keep-Alive");

            //Set the Content Encoding type
            Response.ContentEncoding = Encoding.UTF8;

            //Send data
            _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

            //Dividing the data in 1024 bytes package
            int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

            //Download in block of 1024 bytes
            int i;
            for (i = 0; i < maxCount && Response.IsClientConnected; i++)
            {
                Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                Response.Flush();
            }

            if (i < maxCount)
            {
                res = false;
            }
            else {
                res = true;
            }

                //Close Binary reader and File stream
                _BinaryReader.Close();
                myFile.Close();
                //return res;
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnUpload_Click ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
            }
            return res;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {



            string allowedExtensions = ".xlsx";

            if (FileUpload1.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                if (fileExtension != allowedExtensions)
                {
                    Session["ErrorCallingPage"] = "FrmDownloadMIS_TPA.aspx";
                    string vStatusMsg = "Invalid file Extension, Only XLSX files are allowed to be Uploaded";
                    Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                    return;
                }
            }

            try
            {
                string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(FileUpload1.PostedFile.FileName);

                FileUpload1.SaveAs(excelPath);

                string conString = string.Empty;

                string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);

                switch (extension)
                {

                    case ".xls": //Excel 97-03

                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;

                        break;

                    case ".xlsx": //Excel 07 or higher

                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;

                        break;

                }


                conString = string.Format(conString, excelPath);

                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {

                    excel_con.Open();

                    string sheet1 = "TPA_MIS_SHEET$";

                    DataTable dtExcelData = new DataTable();

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }

                    excel_con.Close();

                    if (dtExcelData.Rows.Count > 0)
                    {
                        List<string> dtcol = dtExcelData.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName).ToList();

                        if (dtcol.Count() == 1 && dtcol[0].ToString().ToUpper() == "CERTIFICATE NO")
                        {
                            List<string> dRowParent = new List<string>();

                            for (int i = 0; i < dtExcelData.Rows.Count; i++)
                            {
                                if (dtExcelData.Rows[i]["Certificate No"].ToString() == "")
                                {
                                    dRowParent.Add(i.ToString());
                                }
                            }


                            if (dRowParent.Count() != dtExcelData.Rows.Count)
                            {
                                Session["UploadedFile"] = FileUpload1.PostedFile.FileName;

                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('File Uploaded SuccessFully!!!');", true);
                            }
                            else
                            {
                                Session["UploadedFile"] = null;

                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('No Records Found!!!');", true);
                            }

                        }

                        else
                        {
                            Session["UploadedFile"] = null;

                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('File Error!!!');", true);
                        }

                    }
                    else
                    {
                        Session["UploadedFile"] = null;

                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('No Records Found!!!');", true);
                    }

                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "btnUpload_Click ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                throw ex;
            }

        }
    }
}