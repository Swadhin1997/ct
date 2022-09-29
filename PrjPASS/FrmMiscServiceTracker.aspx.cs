using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace PrjPASS
{
    public partial class FrmMiscServiceTracker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DrawStatusTables();
        }

        private void DrawStatusTables()
        {
            DataSet ds = GetAllStatus();

            StringBuilder tbodySMS = new StringBuilder();
            StringBuilder tbodyReport = new StringBuilder();
            StringBuilder tbodyService = new StringBuilder();
            StringBuilder tbodyError = new StringBuilder();
            StringBuilder tbodySMSstatusTracker = new StringBuilder();
            StringBuilder tbodyMailstatusTracker = new StringBuilder();



            DataTable dtSMSStatus = new DataTable();
            DataTable dtReportStatus = new DataTable();
            DataTable dtService = new DataTable();
            DataTable dtError = new DataTable();
            DataTable dtGCSMSstatus = new DataTable();
            DataTable dtGCMailstatus = new DataTable();

            dtSMSStatus = ds.Tables[0];
            dtReportStatus = ds.Tables[1];
            dtService = ds.Tables[2];
            dtError = ds.Tables[3];
            dtGCSMSstatus = ds.Tables[4];
            dtGCMailstatus = ds.Tables[5];

            if (dtSMSStatus.Rows.Count > 0)
            {
                tbodySMS.Append("<tbody>");
                foreach (DataRow row in dtSMSStatus.Rows)
                {
                    string ModuleCode = row["ModuleCode"].ToString();
                    string ModuelDescription = row["ModuelDescription"].ToString();
                    string Status = row["Status"].ToString();
                    string StatusLastCheckedOn = Convert.ToDateTime(row["StatusLastCheckedOn"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss tt");

                    string style = Status.ToLower() == "true" ? "style='color:#656565;'" : "style='color:red;'";
                    string tr = "<tr "+ style + "><td>" + ModuelDescription + "</td><td>" + Status + "</td><td>" + StatusLastCheckedOn + "</td></tr>";
                    tbodySMS.Append(tr);
                }

                tbodySMS.Append("</tbody>");
                LiteralSMSStatus.Text = tbodySMS.ToString();
            }

            if (dtReportStatus.Rows.Count > 0)
            {
                tbodyReport.Append("<tbody>");
                foreach (DataRow row in dtReportStatus.Rows)
                {
                    string ModuleCode = row["ModuleCode"].ToString();
                    string ModuelDescription = row["ModuelDescription"].ToString();
                    string Status = row["Status"].ToString();
                    string StatusLastCheckedOn = Convert.ToDateTime(row["StatusLastCheckedOn"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss tt");

                    string style = Status.ToLower() == "true" ? "style='color:#656565;'" : "style='color:red;'";
                    string tr = "<tr " + style + "><td>" + ModuelDescription + "</td><td>" + Status + "</td><td>" + StatusLastCheckedOn + "</td></tr>";
                    tbodyReport.Append(tr);
                }

                tbodyReport.Append("</tbody>");
                LiteralReportStatus.Text = tbodyReport.ToString();
            }


            if (dtService.Rows.Count > 0)
            {
                tbodyService.Append("<tbody>");
                foreach (DataRow row in dtService.Rows)
                {
                    string ModuleCode = row["ModuleCode"].ToString();
                    string ModuelDescription = row["ModuelDescription"].ToString();
                    string Status = row["Status"].ToString();
                    string StatusLastCheckedOn = Convert.ToDateTime(row["StatusLastCheckedOn"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss tt");

                    string style = Status.ToLower() == "true" ? "style='color:#656565;'" : "style='color:red;'";
                    string tr = "<tr " + style + "><td>" + ModuelDescription + "</td><td>" + Status + "</td><td>" + StatusLastCheckedOn + "</td></tr>";
                    tbodyService.Append(tr);
                }

                tbodyService.Append("</tbody>");
                LiteralServiceAvailabilityStatus.Text = tbodyService.ToString();
            }

            if (dtError.Rows.Count > 0)
            {
                tbodyError.Append("<tbody>");
                foreach (DataRow row in dtError.Rows)
                {
                    string ModuleCode = row["ModuleCode"].ToString();
                    string ModuelDescription = row["ModuelDescription"].ToString();
                    string StatusLastCheckedOn = Convert.ToDateTime(row["StatusLastCheckedOn"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss tt");
                    string ErrorMessage = row["ErrorMessage"].ToString();

                    string tr = "<tr><td>" + ModuleCode + "</td><td>" + ErrorMessage + "</td><td>" + StatusLastCheckedOn + "</td></tr>";
                    tbodyError.Append(tr);
                }

                tbodyError.Append("</tbody>");
                LiteralError.Text = tbodyError.ToString();


                if (dtGCSMSstatus.Rows.Count > 0)
                {
                    tbodySMSstatusTracker.Append(@"<div class='table-responsive'> <table class='table'><thead><tr><th>REPORT TYPE</th><th>MODULE NAME</th><th>REPORT DESC</th><th>SMS FLAG</th><th>TOTAL COUNT</th><th>PENDING TOTAL COUNT</th></tr></thead><tbody>");
                    foreach (DataRow dr in dtGCSMSstatus.Rows)
                    {
                        string ReportType = dr["NUM_REPORT_TYPE"].ToString();
                        string ModuleName = dr["TXT_MODULE_NAME"].ToString();
                        string ReportDesc = dr["TXT_REPORT_DESC"].ToString();
                        string SmsFlag = dr["TXT_SMS_FLAG"].ToString();
                        string TotalCount = dr["TOTAL_COUNT"].ToString();
                        string PendingTotalCount = dr["PENDING_TOTAL_COUNT"].ToString();
                        string tr = "<tr><td>" + ReportType + "</td><td>" + ModuleName + "</td><td>" + ReportDesc + "</td><td>" + SmsFlag + "</td><td>" + TotalCount+"</td><td>" + PendingTotalCount + "</td></tr></tbody>";
                        tbodySMSstatusTracker.Append(tr);
                    }
                    tbodySMSstatusTracker.Append("</table>  </div>");
                }
                else
                {
                    tbodySMSstatusTracker.Append("Data not available.");
                }

                LiteralSMSstatusTracker.Text = tbodySMSstatusTracker.ToString();

                if (dtGCMailstatus.Rows.Count > 0)
                {
                    tbodyMailstatusTracker.Append(@"<div class='table-responsive'> <table class='table'><thead><tr><th>REPORT TYPE</th><th>MODULE NAME</th><th>REPORT DESC</th><th>MAIL FLAG</th><th>TOTAL COUNT</th><th>PENDING TOTAL COUNT</th></tr></thead><tbody>");
                    foreach (DataRow dr in dtGCMailstatus.Rows)
                    {
                        string ReportType = dr["NUM_REPORT_TYPE"].ToString();
                        string ModuleName = dr["TXT_MODULE_NAME"].ToString();
                        string ReportDesc = dr["TXT_REPORT_DESC"].ToString();
                        string MailFlag = dr["TXT_MAIL_FLAG"].ToString();
                        string TotalCount = dr["TOTAL_COUNT"].ToString();
                        string PendingTotalCount = dr["PENDING_TOTAL_COUNT"].ToString();
                        string tr = "<tr><td>" + ReportType + "</td><td>" + ModuleName + "</td><td>" + ReportDesc + "</td><td>" + MailFlag + "</td><td>" + TotalCount + "</td><td>" + PendingTotalCount + "</td></tr></tbody>";
                        tbodyMailstatusTracker.Append(tr);
                    }
                    tbodyMailstatusTracker.Append("</table>  </div>");
                }
                else
                {
                    tbodyMailstatusTracker.Append("Data not available.");
                }

                LiteralMailstatusTracker.Text = tbodyMailstatusTracker.ToString();

            }

        }
        private DataSet GetAllStatus()
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_DAILY_MISC_SERVICE_TRACKER_STATUS";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 60000;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetSMSStatus Method");
            }
            return ds;
        }
    }
}