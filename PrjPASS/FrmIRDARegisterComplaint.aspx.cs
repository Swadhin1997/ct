using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace ProjectPASS
{
    public partial class FrmIRDARegisterComplaint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Upload(object sender, EventArgs e)
        {

            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cYearMonth = "", vUploadId = "";

            SqlConnection _con = new SqlConnection(dbCOMMON.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();

            cYearMonth = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                cYearMonth = cYearMonth + "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                cYearMonth = cYearMonth + DateTime.Now.Month.ToString();
            }

            vUploadId = wsDocNo.fn_Gen_Doc_Master_No("PUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);

            DataTable dtUploadErrorLog = new DataTable();
            dtUploadErrorLog.Columns.Add(new DataColumn("vUploadId", typeof(string)));
            dtUploadErrorLog.Columns.Add(new DataColumn("vPolicyId", typeof(string)));
            dtUploadErrorLog.Columns.Add(new DataColumn("dCreatedDate", typeof(DateTime)));
            dtUploadErrorLog.Columns.Add(new DataColumn("bUploadStatus", typeof(string)));
            dtUploadErrorLog.Columns.Add(new DataColumn("vUploadRemarks", typeof(string)));

            try
            {

                //Upload and save the file

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

                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                    DataTable dtExcelData = new DataTable();

                    bool GetMappingData = false;

                    string sqlCommand = "SELECT  * FROM TBL_GPA_COLUMN_MAPPING_MASTER WHERE bExcludeForPolicyUpload='N'";

                    DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);

                    DataSet ds = null;

                    ds = dbCOMMON.ExecuteDataSet(dbCommand);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GetMappingData = true;
                    }

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {

                        oda.Fill(dtExcelData);

                    }

                    excel_con.Close();

                    if (dtExcelData.Rows.Count > 0)
                    {
                        DataTable dtValidatedExcelData = dtExcelData.Copy();//it will copy data and structure

                        dtValidatedExcelData.Columns.Add("vUploadId");

                        foreach (DataRow row in dtValidatedExcelData.Rows)
                        {
                            row["vUploadId"] = vUploadId;
                        }

                        foreach (DataRow excelrow in dtExcelData.Rows)
                        {
                            string vDestinationFieldName = "";
                            string vSourceFieldName = "";
                            string vFieldValue = "";
                            string vPolicyId = "";
                            string vErrorDesc = "";
                            string[] chkValidFlag;

                            if (GetMappingData == true)
                            {
                                bool insertflag = true;

                                for (int vSourceColumnIndex = 0; vSourceColumnIndex < dtExcelData.Columns.Count; vSourceColumnIndex = vSourceColumnIndex + 1)
                                {
                                    string searchExpression = "vSourceColumnIndex = " + vSourceColumnIndex + "";

                                    DataRow[] foundRows = ds.Tables[0].Select(searchExpression);

                                    if (foundRows.Count() > 0)
                                    {
                                        vDestinationFieldName = foundRows[0]["vDestinationColumnName"].ToString();
                                        vSourceFieldName = foundRows[0]["vSourceColumnName"].ToString();
                                        vFieldValue = excelrow[vSourceColumnIndex].ToString();

                                        if (vDestinationFieldName == "vPolicyId")
                                        {
                                            vPolicyId = vFieldValue;
                                        }

                                        chkValidFlag = Fn_Check_Business_Validation(vDestinationFieldName, vFieldValue);

                                        if (chkValidFlag[0] == "false")
                                        {
                                            insertflag = false;

                                            vErrorDesc = vErrorDesc + chkValidFlag[1].ToString();
                                        }
                                    }
                                }
                                if (insertflag == false)
                                {
                                    DataRow dr = dtUploadErrorLog.NewRow();
                                    dr["vUploadId"] = vUploadId;
                                    dr["vPolicyId"] = vPolicyId;
                                    dr["bUploadStatus"] = "Failed";
                                    dr["vUploadRemarks"] = vErrorDesc;
                                    dr["dCreatedDate"] = DateTime.Now;

                                    dtUploadErrorLog.Rows.Add(dr);

                                    string searchExpression = "PolicyId = '" + vPolicyId + "'";

                                    DataRow[] foundRows = dtValidatedExcelData.Select(searchExpression);

                                    if (foundRows.Count() > 0)
                                    {
                                        dtValidatedExcelData.Rows.Remove(foundRows[0]);
                                    }
                                }
                            }
                        }
                        if (dtValidatedExcelData.Rows.Count > 0)
                        {
                            string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                            using (SqlConnection con = new SqlConnection(consString))
                            {

                                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                {

                                    //Set the database table name

                                    sqlBulkCopy.DestinationTableName = "dbo.TBL_GPA_POLICY_TABLE";

                                    //[OPTIONAL]: Map the Excel columns with that of the database table


                                    //Getting Columns and Mapping from the Mapping Table

                                    sqlCommand = "SELECT  * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N' ";
                                    dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                    ds = null;
                                    ds = dbCOMMON.ExecuteDataSet(dbCommand);

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow row in ds.Tables[0].Rows)
                                        {
                                            sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                                        }
                                    }
                                    con.Open();
                                    sqlBulkCopy.WriteToServer(dtValidatedExcelData);
                                    con.Close();
                                }
                            }
                            using (SqlConnection con = new SqlConnection(consString))
                            {
                                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                {
                                    sqlBulkCopy.ColumnMappings.Add("vUploadId", "vUploadId");
                                    sqlBulkCopy.ColumnMappings.Add("vPolicyId", "vPolicyId");
                                    sqlBulkCopy.ColumnMappings.Add("bUploadStatus", "bUploadStatus");
                                    sqlBulkCopy.ColumnMappings.Add("vUploadRemarks", "vUploadRemarks");
                                    sqlBulkCopy.ColumnMappings.Add("dCreatedDate", "dCreatedDate");
                                    sqlBulkCopy.DestinationTableName = "dbo.TBL_GPA_POLICY_TABLE_ERROR_LOG";
                                    con.Open();
                                    sqlBulkCopy.WriteToServer(dtUploadErrorLog);
                                    con.Close();
                                }
                            }
                        }
                    }
                }

                Response.Write("Data Uploaded, To Check the Status Kindly Use Check Upload Status Option with Upload Id : " + vUploadId);
                Response.End();
            }
            catch (Exception ex)
            {
                _tran.Commit();
                Response.Write(ex.Message);
                Response.End();
                //log the error
            }
        }
        protected string[] Fn_Check_Business_Validation(string vFieldName, string vFieldValue)
        {
            string[] ckvalidflag = new string[2];

            if (vFieldName == "vPolicyId")
            {
                if (vFieldValue.Length < 5)
                {
                    ckvalidflag[0] = "false";
                    ckvalidflag[1] = "Policy Id Lenght is Less then 5";
                }
            }

            return ckvalidflag;
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}