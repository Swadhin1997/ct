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
using System.Threading;
using cryptoPDF.api;
using System.Web.ModelBinding;

namespace PrjPASS
{

    public partial class FrmEProposalVerification : System.Web.UI.Page
    {


        private static UrlshortenerService service;
        private static readonly Regex ShortUrlRegex =
                   new Regex("^http[s]?://goo.gl/", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool IsShortUrl(string url)
        {
            return ShortUrlRegex.IsMatch(url);
        }

        public Cls_General_Functions wsGen = new Cls_General_Functions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Session["vUserLoginId"].ToString()))
                {
                    Response.Redirect("~/FrmSecuredLogin.aspx",false);
                }
                FillDropProductname();
            }
            else
            {
                //   ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseModalSaveProposal();", true);
            }


        }

        protected void SetIntermediaryIMDLocation(string strIntermediaryCode)
        {

            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnBPOS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_INTERMEDIARY_BRANCH_AUTOCOMPLETE";
                    cmd.Parameters.AddWithValue("@keyWord", "");
                    cmd.Parameters.AddWithValue("@vIntermediaryCode", strIntermediaryCode);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IntrCds.Add(string.Format("{0}~{1}", sdr["vBranchId"], sdr["vBranchDescription"]));
                        }
                    }
                    conn.Close();
                }
            }
            IntrCds.ToArray();
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
        protected void btnGetIntermediaryCode_Click(object sender, EventArgs e)
        {
            string IntermediaryCode = hfIntermediaryCode.Value;
            FillDrpBranchCode(IntermediaryCode);
        }
        protected void btnCreatePhysicalDocument_Click(object sender, EventArgs e)
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


            if (!FileUploadBulkScanPhysicalForm.HasFile)
            {
                lblstatus.Text = "Error: File Upload Unsuccessful, Please select a pdf (.pdf) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }
            if (Hddnfilename.Value.Trim() != "")
            {
                lblstatus.Text = "Error: Already Document Uploaded!";
                FileUploadBulkScanPhysicalForm.Dispose();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }
            String fileName = FileUploadBulkScanPhysicalForm.PostedFile.FileName;
            string fileExt = System.IO.Path.GetExtension(FileUploadBulkScanPhysicalForm.FileName);

            if (fileExt.Trim().ToLower() != ".pdf")
            {
                lblstatus.Text = "Error: File Upload Unsuccessful, Please select a pdf (.pdf) file to upload";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                bool IsUploadSuccessfull = UploadFileForScannedDoc(FileUploadTransactionId, ref dirFullPath, ref Newfilename);
                if (IsUploadSuccessfull)
                {
                    Hddnfilename.Value = Newfilename;
                    Hddndirectorypath.Value = dirFullPath;
                    lblstatus.Text = "File Uploaded successfully.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "HideProgress();", true);

                }
                else
                {
                    lblstatus.Text = "Error: File Upload Unsuccessful, please contact developer";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
        }
        private bool UploadFileForScannedDoc(string FileTranId, ref string dirFullPath, ref string Newfilename)
        {
            bool IsUploadSuccessfull = false;
            dirFullPath = string.Empty;

            if (FileUploadBulkScanPhysicalForm.HasFile)
            {
                try
                {
                    string strDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string filename = Path.GetFileName(FileUploadBulkScanPhysicalForm.FileName);

                    Newfilename = FileTranId + "$" + filename;
                    dirFullPath = Server.MapPath("~/Uploads/PhysicalScanDocument/") + Newfilename;
                    FileUploadBulkScanPhysicalForm.SaveAs(Server.MapPath("~/Uploads/PhysicalScanDocument/") + Newfilename);

                    IsUploadSuccessfull = true;


                    if (ConfigurationManager.AppSettings["IsProdEnvironment"].ToString() == "1")
                    {
                        try
                        {
                            string serverPath = @"\\10.221.12.44\d$\KGIPASSPUBLISH\Uploads\PhysicalScanDocument\";
                            FileUploadBulkScanPhysicalForm.PostedFile.SaveAs(serverPath + Newfilename);
                        }
                        catch (Exception ex2)
                        {
                            ExceptionUtility.LogException(ex2, "UploadFileForScannedDoc Method for 10.221.12.44 IP ");
                        }

                    }
                }
                catch (Exception ex)
                {
                    IsUploadSuccessfull = false;
                    ExceptionUtility.LogException(ex, "UploadFileForScannedDoc Method");
                }
            }

            return IsUploadSuccessfull;
        }
        private bool RenamePhysicalFile(string PrevFilename, string sourceFile, string Newfilename, ref string dirFullPath)
        {
            bool IsUploadSuccessfull = false;
            dirFullPath = string.Empty;
            String sourcedirectory = string.Empty;
            sourcedirectory = Server.MapPath("~/Uploads/PhysicalScanDocument/");
            try
            {
                // Create a FileInfo  
                System.IO.FileInfo fi = new System.IO.FileInfo(sourceFile);
                // Check if file is there  
                if (fi.Exists)
                {
                    File.Copy(Path.Combine(sourcedirectory, PrevFilename), Path.Combine(sourcedirectory, Newfilename));
                    File.Delete(Path.Combine(sourcedirectory, PrevFilename));
                    string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);

                    dirFullPath = baseUrl + "/kgipass/Uploads/PhysicalScanDocument/" + Newfilename;
                    IsUploadSuccessfull = true;

                    if (ConfigurationManager.AppSettings["IsProdEnvironment"].ToString() == "1")
                    {
                        try
                        {
                            string serverPath = @"\\10.221.12.44\d$\KGIPASSPUBLISH\Uploads\PhysicalScanDocument\";
                            File.Copy(Path.Combine(serverPath, PrevFilename), Path.Combine(serverPath, Newfilename));
                            File.Delete(Path.Combine(serverPath, PrevFilename));

                        }
                        catch (Exception ex2)
                        {
                            ExceptionUtility.LogException(ex2, "RenamePhysicalFile Method for 10.221.12.44 IP ");
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                IsUploadSuccessfull = false;
                ExceptionUtility.LogException(ex, "RenamePhysicalFile Error");
            }


            return IsUploadSuccessfull;
        }
        private bool Validation(out string strErrorMsg)
        {
            bool IsError = false;
            try
            {

                strErrorMsg = "";

                Regex mobilereg = new Regex(@"^[0-9]{10}$");

                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
        @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                Regex emailreg = new Regex(strRegex);
                if (txtClientName.Text == "")
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer Name";
                }
                else if (txtaddress.Text == "")
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer Address";
                }
                else if (txtmobile.Text.Trim() == "")
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer Mobile No";
                }
                else if (!mobilereg.IsMatch(txtmobile.Text.Trim()))
                {
                    IsError = true;
                    strErrorMsg = "Please Enter valid Customer Mobile No";
                }
                
                else if (ConfigurationManager.AppSettings["BlackListedNo"].Contains(txtmobile.Text.Trim()))
                {
                    IsError = true;
                    strErrorMsg = "Please provide correct mobile no or provided mobile no cannot be used as it is blacklisted";
                }
                
               
                else if (txtemail.Text.Trim() == "")
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer Email ID";
                }
                else if (!emailreg.IsMatch(txtemail.Text.Trim()))
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Valid Customer Email ID";
                }
                else if (Session["vUserEmailId"].ToString().ToUpper() == txtemail.Text.ToUpper().Trim())
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer Email Id should be different from Login Email ID";
                }
                else if (txtsuminsured.Text.Trim() == "")
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Sum Insured Opted";
                }
                
                else if (!Regex.IsMatch(txtsuminsured.Text.Trim(), @"^[0-9]+(\.[0-9][0-9]?)?$"))
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Numeric Amount Till 2 Decimal Value";
                }
                else if (drpproduct.SelectedItem.Value == "0")
                {
                    IsError = true;
                    strErrorMsg = "Please Select Product";
                }
                else if (txtIntermediaryName.Text.Trim() == "")
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Intermediary Name";
                }
                else if (drpbranchlocation.SelectedItem.Value == "0")
                {
                    IsError = true;
                    strErrorMsg = "Please Select KGI Branch Location";
                }
                else if (txtpremiumamt.Text.Trim() == "")
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Premium Amount";
                }
                else if (!Regex.IsMatch(txtpremiumamt.Text.Trim(), @"^[0-9]+(\.[0-9][0-9]?)?$"))
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Numeric Amount Till 2 Decimal Value";
                }
                else if (txtproposalno.Text.Trim() == "")
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Proposal No";
                }
                else if (Hddnfilename.Value.Trim() == "")
                {
                    IsError = true;
                    strErrorMsg = "Please Upload Scanned Document";
                }



            }
            catch (Exception ex)
            {
                IsError = false;
                strErrorMsg = ex.Message;
                ExceptionUtility.LogException(ex, "Validation Method");
            }
            return IsError;
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetIntermediaryCode(string prefix)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_INTERMEDIARY_CODE_AUTO";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IntrCds.Add(string.Format("{0}~{1}", sdr["TXT_INTERMEDIARY_NAME"], sdr["TXT_INTERMEDIARY_CD"]));
                        }
                    }
                    conn.Close();
                }
            }
            return IntrCds.ToArray();
        }
        protected void FillDrpBranchCode(string IMDCode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_INTERMEDIARY_BRANCH_AUTOCOMPLETE";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "keyWord", DbType.String, ParameterDirection.Input, "keyWord", DataRowVersion.Current, "");
            db.AddParameter(dbCommand, "vIntermediaryCode", DbType.String, ParameterDirection.Input, "vIntermediaryCode", DataRowVersion.Current, IMDCode);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                drpbranchlocation.Items.Clear();

                drpbranchlocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    drpbranchlocation.Items.Insert(1, new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[i]["vBranchDescription"].ToString(), ds.Tables[0].Rows[i]["vBranchId"].ToString()));

                }
                drpbranchlocation.Items.FindByValue("0").Selected = true;
            }
            else
            {
                drpbranchlocation.Items.Clear();

            }
        }

        protected void FillDropProductname()
        {
            
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_PRODUCTNAME";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);


            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                drpproduct.Items.Clear();

                drpproduct.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    drpproduct.Items.Insert(1, new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[i]["TXT_PRODUCT_NAME_TO_DISPLAY"].ToString(), ds.Tables[0].Rows[i]["PRODUCTCODE"].ToString()));

                }
                drpproduct.Items.FindByValue("0").Selected = true;
            }
            else
            {
                drpproduct.Items.Clear();

            }
        }

        #region CodeForGridView

        public IEnumerable<ProjectPASS.EproposalDetails> EprposalGridView_GetData([Control("txtSearchReferenceNo")] string ReferenceNo, int maximumRows, int startRowIndex, out int totalRowCount)
        {
            int pageSize = maximumRows;
            int pageIndex = 0;

            totalRowCount = GetEProposalDetails(ReferenceNo).Count();

            if (startRowIndex > 0)
            {
                pageIndex = (int)Math.Round(((double)startRowIndex / (double)pageSize));
            }

            return GetEProposalDetails(ReferenceNo).OrderByDescending(x => x.RowCreatedOn).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public IEnumerable<EproposalDetails> GetEProposalDetails(string RefernceNo)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_EPROPOSAL_DETAILS";

                    cmd.Parameters.AddWithValue("@ReferenceNo", string.IsNullOrEmpty(RefernceNo) ? "" : RefernceNo.Trim());
                    cmd.Parameters.AddWithValue("@LoginUserId", Session["vUserLoginId"].ToString());
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return this.CreateEproposalDetails(reader);
                        }
                    }
                    conn.Close();
                }
            }
        }

        private EproposalDetails CreateEproposalDetails(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("No detail exist.");
            }

            return new EproposalDetails
            {
                UniqueRowId = Convert.ToString(reader["UniqueRowId"]),
                ReferenceNo = Convert.ToString(reader["ReferenceNo"]),
                CustomerName = Convert.ToString(reader["CustomerName"]),
                CustomerMobile = Convert.ToString(reader["CustomerMobile"]),
                CustomerEmail = Convert.ToString(reader["CustomerEmail"]),
                Product = Convert.ToString(reader["Product"]),
                SumInsuredAmt = Convert.ToString(reader["SumInsuredAmt"]),
                IMDCODE = Convert.ToString(reader["IMDCODE"]),
                BranchCode = Convert.ToString(reader["BranchCode"]),
                BranchLocationName = Convert.ToString(reader["BranchLocationName"]),
                PremiumAmt = Convert.ToString(reader["PremiumAmt"]),
                ProposalNo = Convert.ToString(reader["ProposalNo"]),
                RowCreatedOn = Convert.ToDateTime(reader["RowCreatedOn"]),
                RowCreatedBy = Convert.ToString(reader["RowCreatedBy"]),
                FileName = Convert.ToString(reader["FileName"]),
                FilePath = Convert.ToString(reader["FilePath"]),
                //  IsFileUploadedToKites = Convert.ToString(reader["IsFileUploadedToKites"]),
                // FileUploadedToKitesOn = Convert.ToDateTime(reader["FileUploadedToKitesOn"]),
                // IsSMSSent = Convert.ToString(reader["IsSMSSent"]),
                // IsEmailSent = Convert.ToString(reader["IsEmailSent"]),
                // IsProposalVerified = Convert.ToString(reader["IsProposalVerified"]),
                //ProposalVerifiedOn = Convert.ToDateTime(reader["ProposalVerifiedOn"]),
            };
        }

        protected void EproposalGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            EprposalGridView.PageIndex = e.NewPageIndex;
        }

        protected void btnSearchReference_Click(object sender, EventArgs e)
        {
            EprposalGridView.DataBind();
        }

        #endregion
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            string strErrorMsg = string.Empty;
            string Type = string.Empty;
            string PrevFilename = string.Empty;
            string sourceFile = string.Empty;
            string Newfilename = string.Empty;
            string dirFullPath = string.Empty;
            string Productname = string.Empty;
            string mobileno = string.Empty;
            string reviewurl = string.Empty;
            string emailid = string.Empty;
            string googleShortURL = string.Empty;
            String customername= string.Empty;
            if (Validation(out strErrorMsg))
            {
                lblstatus.Text = "Error: " + strErrorMsg;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                Type = "Add";
                PrevFilename = Hddnfilename.Value;
                sourceFile = Hddndirectorypath.Value;
                string Date = DateTime.Now.Date.ToString("yyyy/MM/dd").Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
                string Hour = DateTime.Now.Hour.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
                string Minute = DateTime.Now.Minute.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
                string Second = DateTime.Now.Second.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
                string Millisecond = DateTime.Now.Millisecond.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");

                string VreferenceNo = Date + Hour + Minute + Second + Millisecond; //Refernce NO
                String[] filename = PrevFilename.Split('$');
                Newfilename = VreferenceNo + "_" + filename[1];
                Productname = drpproduct.SelectedItem.Text;
                mobileno = txtmobile.Text.Trim();
                emailid = txtemail.Text.Trim();
                customername = txtClientName.Text.Trim();
                String EreviewLink = string.Empty;
                RenamePhysicalFile(PrevFilename, sourceFile, Newfilename, ref dirFullPath);

                string ReviewLink = ConfigurationManager.AppSettings["ReviewEproposalLink"].ToString() + "" + Encryption.EncryptText(VreferenceNo);//Review Link 
                Uri uriResult;
                bool IsValidURL = Uri.TryCreate(ReviewLink, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (IsValidURL)
                {
                    GoogleURLShortner(ReviewLink, out googleShortURL);
                    EreviewLink = googleShortURL;
                    if (googleShortURL.Trim()=="")
                    {
                        EreviewLink = ReviewLink;

                    }
                }
                else
                {
                    EreviewLink = ReviewLink;
                }

                bool IsSMSSent = SendConfirmationSMS(mobileno, EreviewLink, Productname);
                string strMailSubject = "Your Proposal is Ready for E Validation, Unique ID: "+VreferenceNo;
                string IsEmailSent =SendEmail(emailid, VreferenceNo, EreviewLink, strMailSubject,Productname,customername);
            
                SaveRequestResponse_EProposal(VreferenceNo, Type, Newfilename, dirFullPath, IsSMSSent, IsEmailSent, ReviewLink, googleShortURL, ref strErrorMsg);
                if (strErrorMsg == "Success")
                {
                    Reset();
                    btnSearchReference_Click(this, new EventArgs());
                    Alert.Show("Successfully E-Proposal Details Submitted.");
                }
                else
                {
                    Alert.Show("Your Details Not submitted, please try again.");
                }

            }
        }
        private void Reset()
        {
            txtClientName.Text = "";
            txtaddress.Text = "";
            txtmobile.Text = "";
            txtemail.Text = "";
            txtsuminsured.Text = "";
            txtIntermediaryName.Text = "";
            hfIntermediaryCode.Value = "";
            drpbranchlocation.ClearSelection();
            txtpremiumamt.Text = "";
            txtproposalno.Text = "";
            FileUploadBulkScanPhysicalForm.Dispose();
            Hddnfilename.Value = "";
            Hddndirectorypath.Value = "";
            drpproduct.ClearSelection();
        }
        private void SaveRequestResponse_EProposal(String VreferenceID, String Type, String filename, String filepath,bool IsSMSSent,string IsEmailSent,string EproposalReviewurl,string googleshorturl, ref string messsage)
        {


            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_INSERT_E_PROPOSAL_DTLS";

                        cmd.Parameters.AddWithValue("@vReferenceNo", VreferenceID);
                        cmd.Parameters.AddWithValue("@vCustomerName", txtClientName.Text);
                        cmd.Parameters.AddWithValue("@vCustomerAddress", txtaddress.Text);
                        cmd.Parameters.AddWithValue("@vCustomerMobile", txtmobile.Text);
                        cmd.Parameters.AddWithValue("@vCustomerEmail", txtemail.Text);
                        cmd.Parameters.AddWithValue("@vProduct", drpproduct.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@vSumInsuredAmt", txtsuminsured.Text.Trim());
                        cmd.Parameters.AddWithValue("@vIMDCODE", hfIntermediaryCode.Value);
                        cmd.Parameters.AddWithValue("@vIMDName", txtIntermediaryName.Text);
                        cmd.Parameters.AddWithValue("@vBranchCode", drpbranchlocation.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@vBranchLocationName", drpbranchlocation.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@vPremiumAmt", txtpremiumamt.Text);
                        cmd.Parameters.AddWithValue("@vProposalNo", txtproposalno.Text);
                        cmd.Parameters.AddWithValue("@vRowCreatedBy", Session["vUserLoginId"].ToString());
                        cmd.Parameters.AddWithValue("@vFileName", filename);
                        cmd.Parameters.AddWithValue("@vFilePath", filepath);
                        cmd.Parameters.AddWithValue("@vIsFileUploadedToKites", 0);
                        cmd.Parameters.AddWithValue("@IsSMSSent", IsSMSSent == false ? 0 : 1);
                        cmd.Parameters.AddWithValue("@IsEmailSent", IsEmailSent!= "Success" ? 0 : 1);
                        cmd.Parameters.AddWithValue("@vIsProposalVerified", 0);
                        cmd.Parameters.AddWithValue("@IsActive", 1);
                        cmd.Parameters.AddWithValue("@vAction", Type);
                        cmd.Parameters.AddWithValue("@otpnumber", "");
                        cmd.Parameters.AddWithValue("@vEproposalReviewurl", EproposalReviewurl);
                        cmd.Parameters.AddWithValue("@vGoogleShorturl", googleshorturl);

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        messsage = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                messsage = "error";
                ExceptionUtility.LogException(ex, "SaveRequestResponse_EProposal Method");
            }
        }

        private bool SendConfirmationSMS(string mobileno,string reviewurl,string productname)
        {
            bool IsSMSSent = false;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnConnect"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SAVE_PASS_EPROPOSAL_VERIFICATION_TO_TRANS_SMS_LOG";
                        cmd.Parameters.AddWithValue("@MobileNo", mobileno);
                        cmd.Parameters.AddWithValue("@ReviewConfirmURL", reviewurl);
                        cmd.Parameters.AddWithValue("@ProductName",productname);
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        IsSMSSent = true;
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogException(ex, "SendConfirmationSMS Method");
                return IsSMSSent;

            }
            return IsSMSSent;

        }


        private string SendEmail(string ToEmailIds, string VreferenceNo,string EreviewLink, string MailSubject,string Productname,string customername)
        {



          

            string strMessage = string.Empty;
            string emailId = ToEmailIds;
            string strPath = string.Empty;
            string MailBody = string.Empty;

            try
            {
          
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "EproposalEmailbody.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("@vReferenceNo", VreferenceNo);
                MailBody = MailBody.Replace("@ProductName", Productname);
                MailBody = MailBody.Replace("@ReviewURl", EreviewLink);
                MailBody = MailBody.Replace("@Customername", customername);
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"], "Kotak General");
                mm.Subject = MailSubject;
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;

                smtpClient.Send(mm);
                strMessage = "Success";
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                string strPathErrorFile = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPathErrorFile, "Error: " + strMessage);
            }
            return strMessage;


        }

      
        private void GoogleURLShortner(string longURL, out string shortURL)
        {
            shortURL = string.Empty;
            try
            {
                string ErrorMsg = string.Empty;
                WebRequest.DefaultWebProxy = null;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
                string UserId = ConfigurationManager.AppSettings["UserIdForShortURL"].ToString();
                string AccessKey = ConfigurationManager.AppSettings["AccessKeyForShortURL"].ToString();
                proxy.ConvertLongURLToShortURL(UserId, AccessKey, longURL, out shortURL, out ErrorMsg);
                proxy.Close();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
            }
        }

        protected void DownloadPDFFile(object sender, EventArgs e)
        {
            try
            {
                string Filename = (sender as LinkButton).CommandArgument;
                string Filepath = ConfigurationManager.AppSettings["EproposalPdfPath"].ToString() + "" + Filename;
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + Filename);
                Response.TransmitFile(Filepath);
                Response.End();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "DownloadPDFFile");
            }

        }
    }

}
