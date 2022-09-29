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
    public partial class FrmCMCDocsViewer : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ADLoginId"] == null && IsPostBack == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalADLogin();", true);
            }
            else
            {
                if (Session["ADLoginId"] != null)
                {
                    txtSearchNumber.Enabled = true;
                    btnSearchDocument.Enabled = true;
                }
            }

            if (!IsPostBack)
            {
                ViewState["FilePath"] = "";
                rptViewDocuments.DataSource = null;
                rptViewDocuments.DataBind();
            }
        }


      

        protected void btnSearchDocument_Click(object sender, EventArgs e)
        {
            ViewState["FilePath"] = "";
            DataSet ds = new DataSet();
            ds = GetDocumentPaths();

            //if (ds != null)
            //{
            //    if (ds.Tables.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            BindRepeater(ds);
            //        }
            //    }
            //}

            BindRepeater(ds);

        }

        private DataSet GetDocumentPaths()
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnCMCDocsDBConn");

                string InwardId = rdbtnInwardId.Checked ? txtSearchNumber.Text.Trim() : string.Empty;
                string ApplicationNumber = rdbtnApplicationNumber.Checked ? txtSearchNumber.Text.Trim() : string.Empty;
                string CustomerId = rdbtnCustomerId.Checked ? txtSearchNumber.Text.Trim() : string.Empty;
                string DocumentUniqueNumber = rdbtnDocumentUniqueNumber.Checked ? txtSearchNumber.Text.Trim() : string.Empty;
                string PolicyNumber = rdbtnPolicyNumber.Checked ? txtSearchNumber.Text.Trim() : string.Empty;
                string ClaimNumber = rdbtnClaimNumber.Checked ? txtSearchNumber.Text.Trim() : string.Empty;
                string LoginId = "HASMUKH";
                string UserName = "HASMUKH";

                string sqlCommand = "PROC_GET_DMS_DOCUMENT_PATH";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "InwardId", DbType.String, ParameterDirection.Input, "InwardId", DataRowVersion.Current, InwardId);
                db.AddParameter(dbCommand, "ApplicationNumber", DbType.String, ParameterDirection.Input, "ApplicationNumber", DataRowVersion.Current, ApplicationNumber);
                db.AddParameter(dbCommand, "CustomerId", DbType.String, ParameterDirection.Input, "CustomerId", DataRowVersion.Current, CustomerId);
                db.AddParameter(dbCommand, "DocumentUniqueNumber", DbType.String, ParameterDirection.Input, "DocumentUniqueNumber", DataRowVersion.Current, DocumentUniqueNumber);
                db.AddParameter(dbCommand, "PolicyNumber", DbType.String, ParameterDirection.Input, "PolicyNumber", DataRowVersion.Current, PolicyNumber);
                db.AddParameter(dbCommand, "ClaimNumber", DbType.String, ParameterDirection.Input, "ClaimNumber", DataRowVersion.Current, ClaimNumber);
                //db.AddParameter(dbCommand, "LoginId", DbType.String, ParameterDirection.Input, "LoginId", DataRowVersion.Current, LoginId);
                //db.AddParameter(dbCommand, "UserName", DbType.String, ParameterDirection.Input, "UserName", DataRowVersion.Current, UserName);

                dbCommand.CommandType = CommandType.StoredProcedure;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetDocumentPaths Method");
            }
            return ds;
        }

        private void BindRepeater(DataSet ds)
        {
            rptViewDocuments.DataSource = ds.Tables[0];
            rptViewDocuments.DataBind();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Session["ADLoginId"] = null;
            Response.Redirect("FrmMainMenu.aspx");
        }

     

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtLoginId.Text.Trim().Length > 0 && txtPassword.Text.Trim().Length > 0)
            {
                bool IsValidUser = ValidateADLogin(txtLoginId.Text.Trim(), txtPassword.Text.Trim());
                if (IsValidUser)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseModalADLogin();", true);
                    txtSearchNumber.Enabled = true;
                    btnSearchDocument.Enabled = true;
                    Session["ADLoginId"] = txtLoginId.Text.Trim();
                }
                else
                {
                    Alert.Show("Invalid Login Id or password", "FrmCMCDocsViewer.aspx");
                }
            }
        }

        public bool ValidateADLogin(string LoginId, string Password)
        {

            try
            {
                ////to avoide error: "The request was aborted: Could not create SSL/TLS secure channel"

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                ////

                ADBank_ServiceReference.Service1Client proxy = new ADBank_ServiceReference.Service1Client();
                bool IsValidUser = proxy.ValidateUser(LoginId, Password);
                proxy.Close();


                return IsValidUser;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "ValidateADLogin");
                return false;
            }
        }

        protected void rptViewDocuments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (rptViewDocuments.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    Label lblFooter = (Label)e.Item.FindControl("lblEmptyData");
                    lblFooter.Visible = true;
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkViewDocument = (LinkButton)e.Item.FindControl("lnkViewDocument");
                ImageButton imgBtnDocumentPath = (ImageButton)e.Item.FindControl("imgBtnDocumentPath");
                if (lnkViewDocument.Text.Trim().Length > 0)
                {
                    string[] items = lnkViewDocument.Text.Trim().Split('/');
                    string lastItem = items[items.Length - 1];
                    lnkViewDocument.Text = lastItem;

                    ScriptManager.GetCurrent(this).RegisterPostBackControl(imgBtnDocumentPath);
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkViewDocument);
                }
            }


        }

        protected void rptViewDocuments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "View")
                {
                    LinkButton lnkViewDocument = (LinkButton)e.Item.FindControl("lnkViewDocument");
                    ImageButton imgBtnDocumentPath = (ImageButton)e.Item.FindControl("imgBtnDocumentPath");
                    
                    string FTPPath = e.CommandArgument.ToString();

                    string filename = lnkViewDocument.Text.Trim();
                    string ftpPath = "", localPath = "";

                    localPath = "Repository/tempFiles/";


                    //string StrPath = gdAnnexureDetails.DataKeys[ronum].Value.ToString();
                    FileInfo exstfile = new FileInfo(Server.MapPath(localPath + filename));
                    if (DownLoad(localPath, FTPPath, filename) == true)
                    {
                        ViewState["FilePath"] = "";
                        ViewState["FilePath"] = localPath + filename;
                    }
                }

                if (e.CommandName == "Download")
                {
                    string FTPPath = e.CommandArgument.ToString();
                    LinkButton lnkViewDocument = (LinkButton)e.Item.FindControl("lnkViewDocument");
                    DownloadFile(FTPPath, lnkViewDocument.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "rptViewDocuments_ItemCommand");
                Alert.Show("Could not download the file");
            }
        }

        protected void DownloadFile(string FTPPath, string fileName)
        {
            try
            {
                string FTPUsername = ConfigurationManager.AppSettings["CMCDocs_FTPUsername"].ToString();
                string FTPPassword = ConfigurationManager.AppSettings["CMCDocs_FTPPassword"].ToString();

                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPPath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
                request.UsePassive = true;
                request.UseBinary = true;
                request.EnableSsl = false;

                //Fetch the Response and read it into a MemoryStream object.
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                using (MemoryStream stream = new MemoryStream())
                {
                    //Download the File.
                    response.GetResponseStream().CopyTo(stream);
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
            }
            catch (WebException ex)
            {
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }
        }

        public bool DownLoad(string localFilePath, string ftpFilePath, string fileName)
        {
            bool retVal = false;
            try
            {
                string localPath = Server.MapPath(localFilePath);
                string Login = System.Configuration.ConfigurationManager.AppSettings["CMCDocs_FTPUsername"].ToString();
                string Pass = System.Configuration.ConfigurationManager.AppSettings["CMCDocs_FTPPassword"].ToString();
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpFilePath);
                requestFileDownload.Credentials = new NetworkCredential(Login, Pass);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                requestFileDownload.Proxy = null;
                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();

                Stream responseStream = responseFileDownload.GetResponseStream();
                FileStream writeStream = new FileStream(localPath + "\\" + fileName, FileMode.Create);

                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);

                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }

                responseStream.Close();
                writeStream.Close();

                requestFileDownload = null;
                responseFileDownload = null;
                retVal = true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "DownLoad");
            }
            return retVal;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmCMCDocsViewer.aspx");
        }
    }
}