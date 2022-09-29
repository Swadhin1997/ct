using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using ProjectPASS;

namespace PrjPASS
{
    public partial class FrmTalismaInteraction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUploadInteractonFile_Click(object sender, EventArgs e)
        {

            string dirFullPath = string.Empty;
            string Message = string.Empty;

            string Date = DateTime.Now.Date.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Hour = DateTime.Now.Hour.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Minute = DateTime.Now.Minute.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Second = DateTime.Now.Second.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Millisecond = DateTime.Now.Millisecond.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            //string FinalOutputFileName = Date + Hour + Minute + Second + Millisecond + "_BulkInvoiceLinks.xls";
            //string outPutPath = Server.MapPath("~/Reports/") + FinalOutputFileName.Replace(" ", "");
            string FileUploadTransactionId = Date + Hour + Minute + Second + Millisecond;


            if (!FileUploadBulkInvoiceByScheduler.HasFile)
            {
                lblstatus.Text = "Error: File Upload Unsuccussful, Please select a excel (.xlsx) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }

            String fileName = FileUploadBulkInvoiceByScheduler.PostedFile.FileName;
            string fileExt = System.IO.Path.GetExtension(FileUploadBulkInvoiceByScheduler.FileName);

            if (fileExt.Trim().ToLower() != ".xlsx")
            {
                lblstatus.Text = "Error: File Upload Unsuccussful, Please select a excel (.xlsx) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                bool IsUploadSuccessfull = UploadFileForSchedulerTalisma(FileUploadTransactionId, ref dirFullPath);
                if (IsUploadSuccessfull)
                {
                    //Save Record into database

                    string message = SaveBulkInteractionFileUploadInformation(FileUploadTransactionId, fileName, dirFullPath, Session["vUserLoginId"].ToString().ToUpper());
                    if (message == "success")
                    {
                        lblstatus.Text = "File Uploaded succussfully.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
                        lblstatus.Text = "Error: File Upload Unsuccussful, Error:" + message;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "HideProgress();", true);

                    //FileProcessGridView.DataBind();
                }
                else
                {
                    lblstatus.Text = "Error: File Upload Unsuccussful, please contact developer";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
        }

        private bool UploadFileForSchedulerTalisma(string FileTranId, ref string dirFullPath)
        {
            bool IsUploadSuccessfull = false;
            dirFullPath = string.Empty;
            if (FileUploadBulkInvoiceByScheduler.HasFile)
            {
                try
                {
                    string strDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string filename = Path.GetFileName(FileUploadBulkInvoiceByScheduler.FileName);
                    dirFullPath = Server.MapPath("~/Uploads/") + FileTranId + "_" + filename;
                    FileUploadBulkInvoiceByScheduler.SaveAs(Server.MapPath("~/Uploads/") + FileTranId + "_" + filename);

                    IsUploadSuccessfull = true;
                }
                catch (Exception ex)
                {
                    IsUploadSuccessfull = false;
                    ExceptionUtility.LogException(ex, "UploadFileForSchedulerTalisma");
                }
            }

            return IsUploadSuccessfull;
        }

        private string SaveBulkInteractionFileUploadInformation(string FileUploadTransactionId, string FileName, string FileFullPath, string FileUploadedBy)
        {
            string Message = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_BULK_INTERACTION_FILE_UPLOAD_INFORMATION";

                        cmd.Parameters.AddWithValue("@FileUploadTransactionId", FileUploadTransactionId);
                        cmd.Parameters.AddWithValue("@FileName", FileName);
                        cmd.Parameters.AddWithValue("@FileFullPath", FileFullPath);
                        cmd.Parameters.AddWithValue("@FileUploadedBy", FileUploadedBy);

                        cmd.Connection = conn;
                        conn.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            Message = "success";
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                ExceptionUtility.LogException(ex, "SaveBulkInteractionFileUploadInformation");
            }

            return Message;
        }

        public IEnumerable<ProjectPASS.FileUploadedInformation> FileProcessGridView_GetData(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            int pageSize = maximumRows;
            int pageIndex = 0;

            totalRowCount = GetFileDetails().Count();

            if (startRowIndex > 0)
            {
                pageIndex = (int)Math.Round(((double)startRowIndex / (double)pageSize));
            }

            return GetFileDetails().OrderByDescending(x => x.FileUploadedOn).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public IEnumerable<FileUploadedInformation> GetFileDetails()
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_TALISMA_INTERACTION_FILE_UPLOADED_DETAILS";

                    cmd.Parameters.AddWithValue("@LoginUserId", Session["vUserLoginId"].ToString());
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return this.CreateFileSavedDetails(reader);
                        }
                    }
                    conn.Close();
                }
            }
        }

        private FileUploadedInformation CreateFileSavedDetails(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("No detail exist.");
            }

            return new FileUploadedInformation
            {
                FileUploadTransactionId = Convert.ToString(reader["FileUploadTransactionId"]),
                FileName = Convert.ToString(reader["FileName"]),
                FileUploadedBy = Convert.ToString(reader["FileUploadedBy"]),
                FileUploadedOn = Convert.ToString(reader["FileUploadedOn"]),
                IsFileProcessed = Convert.ToString(reader["IsFileProcessed"]),
                FileProcessedOn = Convert.ToString(reader["FileProcessedOn"]),
            };
        }

        protected void FileProcessGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FileProcessGridView.PageIndex = e.NewPageIndex;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

    }
}