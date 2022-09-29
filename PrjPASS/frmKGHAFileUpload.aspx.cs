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
using System.Data.OleDb;
using System.Threading;
using OfficeInterop = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;


using OfficeOpenXml;
using OfficeOpenXml.Style;
using CCA.Util;
using System.Collections.Specialized;

namespace PrjPASS
{


    public partial class frmKGHAFileUpload : System.Web.UI.Page
    {
        string FrmEProposalReviewScheduleLogDirectory = AppDomain.CurrentDomain.BaseDirectory + "//Uploads//KGHADocument";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(FrmEProposalReviewScheduleLogDirectory))
            {
                Directory.CreateDirectory(FrmEProposalReviewScheduleLogDirectory);
            }

            if (!IsPostBack)
            {
                if (Session["vUserLoginId"] != null)
                {
                   
                }
                else
                {
                    //Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    //return;
                }

            }
        }




        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        protected void btnCreateBulkKGHADoc_Click(object sender, EventArgs e)
        {

            string dirFullPath = string.Empty;
            string Message = string.Empty;
            string Newfilename = string.Empty;
            string Date = DateTime.Now.Date.ToString("yyyy/MM/dd").Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Hour = DateTime.Now.Hour.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Minute = DateTime.Now.Minute.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Second = DateTime.Now.Second.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string Millisecond = DateTime.Now.Millisecond.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            string FileUploadTransactionId = Date + Hour + Minute + Second + Millisecond;


            if (!FileUploadBulkKGHADoc.HasFile)
            {
                lblstatus.Text = "Error: File Upload Unsuccussful, Please select a xlsx (.xlsx) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }
           
            String fileName = FileUploadBulkKGHADoc.PostedFile.FileName;
            string fileExt = System.IO.Path.GetExtension(FileUploadBulkKGHADoc.FileName);

            if (fileExt.Trim().ToLower() != ".xlsx")
            {
                lblstatus.Text = "Error: File Upload Unsuccussful, Please select a pdf (.xlsx) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                bool IsUploadSuccessfull = UploadFileForScheduler(FileUploadTransactionId, ref dirFullPath);
                if (IsUploadSuccessfull)
                {
                    string message = SaveBulkInvoiceFileUploadInformation(FileUploadTransactionId, fileName, dirFullPath, Session["vUserLoginId"].ToString().ToUpper());
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
                }
                else
                {
                    lblstatus.Text = "Error: File Upload Unsuccessful, please contact developer";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
        }
        private bool UploadFileForScheduler(string FileTranId, ref string dirFullPath)
        {
            bool IsUploadSuccessfull = false;
            dirFullPath = string.Empty;
            if (FileUploadBulkKGHADoc.HasFile)
            {
                try
                {
                    string strDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string filename = FileTranId + "_" + Path.GetFileName(FileUploadBulkKGHADoc.FileName);
                    dirFullPath = Server.MapPath("~/Uploads/KGHADocument/") +filename;
                    FileUploadBulkKGHADoc.SaveAs(Server.MapPath("~/Uploads/KGHADocument/") +  filename);
                    if (ConfigurationManager.AppSettings["IsProdEnvironment"].ToString() == "1")
                    {
                        try
                        {
                            string serverPath = @"\\10.221.12.44\d$\KGIPASSPUBLISH\Uploads\KGHADocument\";
                            FileUploadBulkKGHADoc.PostedFile.SaveAs(serverPath + filename);
                        }
                        catch (Exception ex2)
                        {
                            ExceptionUtility.LogException(ex2, "UploadFileForScannedDoc Method for 10.221.12.44 IP ");
                        }

                    }
                    IsUploadSuccessfull = true;
                }
                catch (Exception ex)
                {
                    IsUploadSuccessfull = false;
                    ExceptionUtility.LogException(ex, "UploadFileForScheduler");
                }
            }

            return IsUploadSuccessfull;
        }

        private string SaveBulkInvoiceFileUploadInformation(string FileUploadTransactionId, string FileName, string FileFullPath, string FileUploadedBy)
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
                        cmd.CommandText = "PROC_SAVE_BULK_KGHA_FILE_UPLOAD_INFORMATION";

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
                ExceptionUtility.LogException(ex, "SaveBulkInvoiceFileUploadInformation");
            }

            return Message;
        }
    }

}


