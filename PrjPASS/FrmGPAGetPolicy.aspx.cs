using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using QRCoder;
using System.Drawing;
//using iTextSharp.tool.xml.pipeline.html;
//using iTextSharp.tool.xml.pipeline.css;
//using iTextSharp.tool.xml.pipeline.end;
//using iTextSharp.tool.xml.parser;
using iTextSharp.text.html.simpleparser;
//using iTextSharp.tool.xml;
using System.Text;
using System.Xml;
using System.Configuration;
using Winnovative;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using PrjPASS;
using System.Data.SqlClient;

namespace ProjectPASS
{
    public partial class FrmGPAGetPolicy : System.Web.UI.Page
    {
        public string folderPath = ConfigurationManager.AppSettings["uploadPath"] + DateTime.Now.ToString("dd-MMM-yyyy");
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        // Controls if the current HTML page will be rendered to PDF or as a normal page
        bool convertToPdf = false;
        bool IsWithoutHeaderFooter = false;

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

        protected void btnGetPolicy_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtPolicyId.Text))
            {
                string code = txtPolicyId.Text + txtCustomerName.Text;
                string level = "0";
                QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, eccLevel);
                QRCode qrCode = new QRCode(qrCodeData);

                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

                imgBarCode.Height = 150;

                imgBarCode.Width = 150;

                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {

                    using (MemoryStream ms = new MemoryStream())
                    {

                        // bitMap.Save(Server.MapPath("/Contents/images/" + txtPolicyId.Text.Replace("/", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                        bitMap.Save("D:/ProjectPASS/ProjectPASS/Contents/Images/" + txtPolicyId.Text.Replace("/", "") + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();

                        imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);

                    }
                }
                string vWebSiteName = ConfigurationManager.AppSettings["vWebSiteName"].ToString();

                //string example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p><img src='http://'" + vWebSiteName + "/Contents/Images/child_plans.png' width='271' height='86' alt='QR Code'/>";
                string example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p><img src='D:\ProjectPASS\ProjectPASS\Contents\Images\" + txtPolicyId.Text.Replace("/", "") + ".png' />";
                var example_css = @".headline{font-size:200%}";

                String contents = example_html;
                System.IO.File.WriteAllText(@"D:\" + txtPolicyId.Text.Replace("/", "") + ".html", contents);

                CreatePDF(example_html, example_css);

                ConvertToPDF(example_html, example_css);

            }
            else
            {
                Alert.Show("Please Enter Policy Number", "FrmGPAGetPolicy.aspx");
            }


        }
        protected void ConvertToPDF(string strHtml, string strCss)
        {
            //Create a byte array that will eventually hold our final PDF
            Byte[] bytes;

            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                var doc = new iTextSharp.text.Document();


                //Create a writer that's bound to our PDF abstraction and our stream
                var writer = PdfWriter.GetInstance(doc, ms);


                //Open the document for writing
                doc.Open();

                /**************************************************
                 * Example #2                                     *
                 *                                                *
                 * Use the XMLWorker to parse the HTML.           *
                 * Only inline CSS and absolutely linked          *
                 * CSS is supported                               *
                 * ************************************************/

                //XMLWorker also reads from a TextReader and not directly from a string
                using (var srHtml = new StringReader(strHtml))
                {
                    //Parse the HTML
                    //Commented on 14-Dec-2015
                    // iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                }

                /**************************************************
                 * Example #3                                     *
                 *                                                *
                 * Use the XMLWorker to parse HTML and CSS        *
                 * ************************************************/

                //In order to read CSS as a string we need to switch to a different constructor
                //that takes Streams instead of TextReaders.
                //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams

                //using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strCss)))
                //{
                //    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strHtml)))
                //    {
                //        //Parse the HTML
                //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                //    }
                //}

                doc.Close();


                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + txtPolicyId.Text.Replace("/", "") + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(bytes);
            CommonExtensions.fn_AddLogForDownload(txtPolicyId.Text.Replace("/", ""), "FrmGPAGetPolicy.aspx");//Added By Rajesh Soni 19/02/2020
            Response.End();
           
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
        protected void CreatePDF(string strHtml, string strCss)
        {
            var bytes = Encoding.UTF8.GetBytes(strHtml);
            string cssPath = HttpContext.Current.Server.MapPath("~/Contents");
            try
            {
                using (MemoryStream input = new MemoryStream(bytes))
                {
                    MemoryStream output = new MemoryStream();
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 0, 0, 30, 65);

                    PdfWriter pdfWrt = PdfWriter.GetInstance(pdfDoc, output);

                    pdfWrt.CloseStream = false;
                    //pdfWrt.PageEvent = new PdfWriterEvents("watermark");
                    pdfDoc.Open();
                    //Commented on 14-Dec-2015
                    //HtmlPipelineContext context = new HtmlPipelineContext(null);
                    //context.SetTagFactory(iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory());
                    //context.SetImageProvider(new MyImageProvider());
                    //var cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                    //cssResolver.AddCssFile(cssPath + "\\main.css", true);
                    //IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(context, new PdfWriterPipeline(pdfDoc, pdfWrt)));
                    //var worker = new XMLWorker(pipeline, true);
                    //var parser = new XMLParser();
                    //parser.AddListener(worker);
                    //using (TextReader sr = new StringReader(strHtml))
                    //{
                    //    parser.Parse(sr);
                    //}
                    //Commented on 14-Dec-2015
                    pdfDoc.Close();
                    byte[] test = output.GetBuffer();
                    File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Contents/PDFs/" + txtPolicyId.Text.Replace("/", "") + ".pdf"), test);
                    CommonExtensions.fn_AddLogForDownload(txtPolicyId.Text.Replace("/", ""), "FrmGPAGetPolicy.aspx");//Added By Rajesh Soni 19/02/2020
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void GetCoverSectionDetails(ref string trRowCoverHeader, ref string trSectionARow, ref string trExtSectionARow, ref string trSectionBRow)
        {
            bool IsCoverAvailable = false;
            string strSectionACoverName = string.Empty; string strSectionACoverSI = string.Empty; string strSectionACoverSIText = string.Empty;
            string strExtSectionACoverName = string.Empty; string strExtSectionACoverSI = string.Empty; string strExtSectionACoverSIText = string.Empty;
            string strSectionBCoverName = string.Empty; string strSectionBCoverSI = string.Empty; string strSectionBCoverSIText = string.Empty;

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand = "PROC_GET_COVER_SECTION_DATA_FOR_PDF";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            db.AddParameter(dbCommand, "vCertificateNo", DbType.String, ParameterDirection.Input, "vCertificateNo", DataRowVersion.Current, txtCertificateNumber.Text.Trim());
            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            strSectionACoverSIText = string.IsNullOrEmpty(dr["COVER_SI_TEXT"].ToString().Trim()) ? "" : "(" + dr["COVER_SI_TEXT"].ToString() + ")";
                            if (strSectionACoverName.Length == 0) //if first loop then no br tag else for line break br is added
                            {
                                strSectionACoverName = dr["NAME_OF_BENEFIT"].ToString();
                                strSectionACoverSI = Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionACoverSIText;
                            }
                            else
                            {
                                strSectionACoverName = strSectionACoverName + "<br>" + dr["NAME_OF_BENEFIT"].ToString();
                                strSectionACoverSI = strSectionACoverSI + "<br>" + Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionACoverSIText;
                            }
                        }
                        string td1SectionARow = "<td style='border: 1px solid black' width='20%'><p>Section A</p></td>";
                        string td2SectionARow = "<td style='border: 1px solid black' width='39%'><p>" + strSectionACoverName + "</p></td>";
                        string td3SectionARow = "<td style='border: 1px solid black;text-align:right' width='39%'><p>" + strSectionACoverSI + "</p></td>";
                        trSectionARow = "<tr>" + td1SectionARow + td2SectionARow + td3SectionARow + "</tr>";
                        IsCoverAvailable = true;
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strExtSectionACoverSIText = string.IsNullOrEmpty(dr["COVER_SI_TEXT"].ToString().Trim()) ? "" : "(" + dr["COVER_SI_TEXT"].ToString() + ")";
                            if (strExtSectionACoverName.Length == 0)
                            {
                                strExtSectionACoverName = dr["NAME_OF_BENEFIT"].ToString();
                                strExtSectionACoverSI = Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strExtSectionACoverSIText;
                            }
                            else
                            {
                                strExtSectionACoverName = strExtSectionACoverName + "<br>" + dr["NAME_OF_BENEFIT"].ToString();
                                strExtSectionACoverSI = strExtSectionACoverSI + "<br>" + Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strExtSectionACoverSIText;
                            }
                        }

                        string td1ExtSectionARow = "<td style='border: 1px solid black' width='20%'><p>Extensions under Section A</p></td>";
                        string td2ExtSectionARow = "<td style='border: 1px solid black' width='39%'><p>" + strExtSectionACoverName + "</p></td>";
                        string td3ExtSectionARow = "<td style='border: 1px solid black;text-align:right' width='39%'><p>" + strExtSectionACoverSI + "</p></td>";
                        trExtSectionARow = "<tr>" + td1ExtSectionARow + td2ExtSectionARow + td3ExtSectionARow + "</tr>";
                        IsCoverAvailable = true;
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            strSectionBCoverSIText = string.IsNullOrEmpty(dr["COVER_SI_TEXT"].ToString().Trim()) ? "" : "(" + dr["COVER_SI_TEXT"].ToString() + ")";
                            if (strSectionBCoverName.Length == 0)
                            {
                                strSectionBCoverName = dr["NAME_OF_BENEFIT"].ToString();
                                strSectionBCoverSI = Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionBCoverSIText;
                            }
                            else
                            {
                                strSectionBCoverName = strSectionBCoverName + "<br>" + dr["NAME_OF_BENEFIT"].ToString();
                                strSectionBCoverSI = strSectionBCoverSI + "<br>" + Convert.ToDecimal(dr["SUMINSURED"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol() + " " + strSectionBCoverSIText;
                            }
                        }

                        string td1SectionBRow = "<td style='border: 1px solid black' width='20%'><p>Section B</p></td>";
                        string td2SectionBRow = "<td style='border: 1px solid black' width='39%'><p>" + strSectionBCoverName + "</p></td>";
                        string td3SectionBRow = "<td style='border: 1px solid black;text-align:right' width='39%'><p>" + strSectionBCoverSI + "</p></td>";
                        trSectionBRow = "<tr>" + td1SectionBRow + td2SectionBRow + td3SectionBRow + "</tr>";
                        IsCoverAvailable = true;
                    }
                }
            }

            if (IsCoverAvailable)
            {
                string td1CoverHeader = "<td style='border:1px solid black' width='20%'><p><strong>Coverage Details</strong></p></td>";
                string td2CoverHeader = "<td style='border:1px solid black' width='39%'><p><strong>Name of the Benefit</strong></p></td>";
                string td3CoverHeader = "<td style='border:1px solid black;text-align:center' width='39%'><p><strong>Sum Insured (&#8377;)</strong></p></td>";
                trRowCoverHeader = "<tr>" + td1CoverHeader + td2CoverHeader + td3CoverHeader + "</tr>";
            }
        }
        protected void btnGPACertificate_Click(object sender, EventArgs e)
        {
            if (txtCertificateNumber.Text.Trim() == "")
            {
                Alert.Show("Certificate Number Cannot Be Blank", "FrmGPAGetPolicy.aspx");
                return;
            }
            if (txtCertificateNumber.Text.IsContainSQLKeywordsOrIsContainSpecialChars())
            {
                Alert.Show("Invalid Charaters Entered", "FrmGPAGetPolicy.aspx");
                return;
            }
            else
            {
                // The current ASP.NET page will be rendered to PDF when its Render method will be called by framework
                convertToPdf = true;
            }
        }

        private void GetGPACertificateDetails(
              ref string CertificateNumber
            , ref string ProductName
            , ref string PolicyIssuingOfficeAddress
            , ref string IntermediaryName
            , ref string IntermediaryCode
            , ref string PolicyholderName
            , ref string PolicyholderAddress
            , ref string PolicyholderAddress2
            , ref string PolicyholderBusinessDescription
            , ref string PolicyholderTelephoneNumber
            , ref string PolicyholderEmailAddress
            , ref string PolicyNumber
            , ref string PolicyInceptionDateTime
            , ref string PolicyExpiryDateTime
            , ref string TotalNumberOfInsuredPersons
            , ref string RowCoverHeader
            , ref string SectionARow
            , ref string ExtSectionARow
            , ref string SectionBRow
            , ref string NameofInsuredPerson
            , ref string DateOfBirth
            , ref string Gender
            , ref string EmailId
            , ref string MobileNo
            , ref string SumInsured
            , ref string NomineeDetails
            , ref string SectionACoverPremium
            , ref string ExtensionstoSectionASectionBCoverPremium
            , ref string LoadingsDiscounts
            , ref string ServiceTax
            , ref string SwachhBharatCess
            , ref string KrishiKalyanCess
            , ref string NetPremiumRoundedOff
            , ref string StampDuty
            , ref string Receipt_Challan_No
            , ref string Receipt_Challan_No_Dated
            , ref string PolicyIssueDate
            , ref string IntermediaryLandline
            , ref string IntermediaryMobile
            , ref string TotalAmount
            , ref bool IsCertificateNumberExists
            , ref string ugstPercentage
            , ref string ugstAmount
            , ref string cgstPercentage
            , ref string cgstAmount
            , ref string igstPercentage
            , ref string igstAmount
            , ref string sgstPercentage
            , ref string sgstAmount
            , ref string totalgstAmount
            , ref string vProposerPinCode
            , ref string addCol1
            , ref string polStartDate
            , ref string UINNo
            )
        {
            string strSectionACoverName = string.Empty; string strSectionACoverSI = string.Empty;
            string strExtSectionACoverName = string.Empty; string strExtSectionACoverSI = string.Empty;
            string strSectionBCoverName = string.Empty; string strSectionBCoverSI = string.Empty;
            string trCoverHeader = string.Empty;
            string trSectionARow = string.Empty;
            string trExtSectionARow = string.Empty;
            string trSectionBRow = string.Empty;
            IsCertificateNumberExists = false;
            GetCoverSectionDetails(ref trCoverHeader, ref trSectionARow, ref trExtSectionARow, ref trSectionBRow);

            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_GPA_CERTIFICATE_DETAILS";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            db.AddParameter(dbCommand, "vCertificateNo", DbType.String, ParameterDirection.Input, "vCertificateNo", DataRowVersion.Current, txtCertificateNumber.Text.Trim());
            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsCertificateNumberExists = true;
                    //lblSeatingCapacityt.Text = ds.Tables[0].Rows[0][0].ToString();
                    CertificateNumber = ds.Tables[0].Rows[0]["vCertificateNo"].ToString();
                    ProductName = ds.Tables[0].Rows[0]["ProductName"].ToString();
                    PolicyIssuingOfficeAddress = ds.Tables[0].Rows[0]["PolicyIssuingOfficeAddress"].ToString();
                    IntermediaryName = ds.Tables[0].Rows[0]["IntermediaryName"].ToString();
                    IntermediaryCode = ds.Tables[0].Rows[0]["IntermediaryCode"].ToString();

                    IntermediaryLandline = ds.Tables[0].Rows[0]["IntermediaryLandline"].ToString();
                    IntermediaryMobile = ds.Tables[0].Rows[0]["IntermediaryMobile"].ToString();

                    PolicyholderName = ds.Tables[0].Rows[0]["PolicyholderName"].ToString();
                    PolicyholderAddress = ds.Tables[0].Rows[0]["PolicyholderAddress"].ToString();
                    PolicyholderAddress2 = ds.Tables[0].Rows[0]["PolicyholderAddress2"].ToString();
                    PolicyholderBusinessDescription = ds.Tables[0].Rows[0]["PolicyholderBusinessDescription"].ToString();
                    PolicyholderTelephoneNumber = ds.Tables[0].Rows[0]["PolicyholderTelephoneNumber"].ToString();
                    PolicyholderEmailAddress = ds.Tables[0].Rows[0]["PolicyholderEmailAddress"].ToString();
                    PolicyNumber = ds.Tables[0].Rows[0]["PolicyNumber"].ToString();
                    PolicyInceptionDateTime = ds.Tables[0].Rows[0]["PolicyInceptionDateTime"].ToString();
                    PolicyExpiryDateTime = ds.Tables[0].Rows[0]["PolicyExpiryDateTime"].ToString();
                    TotalNumberOfInsuredPersons = ds.Tables[0].Rows[0]["TotalNumberOfInsuredPersons"].ToString();
                    RowCoverHeader = trCoverHeader;
                    SectionARow = trSectionARow;
                    ExtSectionARow = trExtSectionARow;
                    SectionBRow = trSectionBRow;
                    NameofInsuredPerson = ds.Tables[0].Rows[0]["NameofInsuredPerson"].ToString();
                    DateOfBirth = ds.Tables[0].Rows[0]["DateOfBirth"].ToString();
                    Gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                    EmailId = ds.Tables[0].Rows[0]["EmailId"].ToString();
                    MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                    SumInsured = ds.Tables[0].Rows[0]["SumInsured"].ToString();
                    NomineeDetails = ds.Tables[0].Rows[0]["NomineeDetails"].ToString();
                    SectionACoverPremium = ds.Tables[0].Rows[0]["SectionACoverPremium"].ToString();
                    ExtensionstoSectionASectionBCoverPremium = ds.Tables[0].Rows[0]["ExtensionstoSectionASectionBCoverPremium"].ToString();
                    LoadingsDiscounts = ds.Tables[0].Rows[0]["LoadingsDiscounts"].ToString();
                    ServiceTax = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                    SwachhBharatCess = ds.Tables[0].Rows[0]["SwachhBharatCess"].ToString();
                    KrishiKalyanCess = ds.Tables[0].Rows[0]["KrishiKalyanCess"].ToString();
                    NetPremiumRoundedOff = ds.Tables[0].Rows[0]["NetPremiumRoundedOff"].ToString();
                    StampDuty = ds.Tables[0].Rows[0]["StampDuty"].ToString();
                    Receipt_Challan_No = ds.Tables[0].Rows[0]["Receipt_Challan_No"].ToString();
                    Receipt_Challan_No_Dated = ds.Tables[0].Rows[0]["Receipt_Challan_No_Dated"].ToString();
                    PolicyIssueDate = ds.Tables[0].Rows[0]["PolicyIssueDate"].ToString();
                    TotalAmount = ds.Tables[0].Rows[0]["TotalAmount"].ToString();
                    ugstPercentage = ds.Tables[0].Rows[0]["ugstPercentage"].ToString();
                    ugstAmount = ds.Tables[0].Rows[0]["ugstAmount"].ToString();
                    cgstPercentage = ds.Tables[0].Rows[0]["cgstPercentage"].ToString();
                    cgstAmount = ds.Tables[0].Rows[0]["cgstAmount"].ToString();
                    sgstPercentage = ds.Tables[0].Rows[0]["sgstPercentage"].ToString();
                    sgstAmount = ds.Tables[0].Rows[0]["sgstAmount"].ToString();
                    igstPercentage = ds.Tables[0].Rows[0]["igstPercentage"].ToString();
                    igstAmount = ds.Tables[0].Rows[0]["igstAmount"].ToString();
                    totalgstAmount = ds.Tables[0].Rows[0]["totalGSTAmount"].ToString();
                    vProposerPinCode = ds.Tables[0].Rows[0]["vProposerPinCode"].ToString();
                    addCol1 = ds.Tables[0].Rows[0]["addCol1"].ToString();
                    polStartDate = ds.Tables[0].Rows[0]["polStartDate"].ToString();
                    UINNo = ds.Tables[0].Rows[0]["UINNo"].ToString();
                }
            }

        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (rdoGPAPolicyType.Checked == true)
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF.html";
                string htmlBody = File.ReadAllText(strPath);
                string CertificateNumber = string.Empty;
                string ProductName = string.Empty;
                string PolicyIssuingOfficeAddress = string.Empty;

                string IntermediaryName = string.Empty;
                string IntermediaryCode = string.Empty;
                string IntermediaryLandline = string.Empty;
                string IntermediaryMobile = string.Empty;

                string PolicyholderName = string.Empty;
                string PolicyholderAddress = string.Empty;
                string PolicyholderAddress2 = string.Empty;
                string PolicyholderBusinessDescription = string.Empty;
                string PolicyholderTelephoneNumber = string.Empty;
                string PolicyholderEmailAddress = string.Empty;
                string PolicyNumber = string.Empty;
                string PolicyInceptionDateTime = string.Empty;
                string PolicyExpiryDateTime = string.Empty;
                string TotalNumberOfInsuredPersons = string.Empty;

                string RowCoverHeader = string.Empty;
                string SectionARow = string.Empty;
                string ExtSectionARow = string.Empty;
                string SectionBRow = string.Empty;

                string NameofInsuredPerson = string.Empty;
                string DateOfBirth = string.Empty;
                string Gender = string.Empty;
                string EmailId = string.Empty;
                string MobileNo = string.Empty;
                string SumInsured = string.Empty;
                string NomineeDetails = string.Empty;
                string SectionACoverPremium = string.Empty;
                string ExtensionstoSectionASectionBCoverPremium = string.Empty;
                string LoadingsDiscounts = string.Empty;
                string ServiceTax = string.Empty;
                string SwachhBharatCess = string.Empty;
                string KrishiKalyanCess = string.Empty;
                string NetPremiumRoundedOff = string.Empty;
                string StampDuty = string.Empty;
                string Receipt_Challan_No = string.Empty;
                string Receipt_Challan_No_Dated = string.Empty;
                string PolicyIssueDate = string.Empty;
                string TotalAmount = string.Empty;
                bool IsCertificateNumberExists = false;
                string prod_name = string.Empty;
                //gst changes
                string ugstPercentage = string.Empty;
                string ugstAmount = string.Empty;
                string cgstPercentage = string.Empty;
                string cgstAmount = string.Empty;
                string sgstPercentage = string.Empty;
                string sgstAmount = string.Empty;
                string igstPercentage = string.Empty;
                string igstAmount = string.Empty;
                string totalGSTAmount = string.Empty;
                string vProposerPinCode = string.Empty;
                string addCol1 = string.Empty;
                string polStartDate = string.Empty;
                string UINNo = string.Empty;

                if (convertToPdf)
                {
                    if (!String.IsNullOrEmpty(txtCertificateNumber.Text))
                    {
                        Database db = DatabaseFactory.CreateDatabase("cnPASS");


                        using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT vProductName from TBL_GPA_POLICY_TABLE where vCertificateNo=@vCertificateNo";
                                cmd.Connection = sqlCon;

                                var sqlParamUser = new SqlParameter("vCertificateNo", SqlDbType.VarChar);
                                sqlParamUser.Value = txtCertificateNumber.Text.Trim();
                                cmd.Parameters.Add(sqlParamUser);

                                sqlCon.Open();
                                object objProd = cmd.ExecuteScalar();
                                prod_name = Convert.ToString(objProd);
                                sqlCon.Close();
                            }
                        }
                    }

                    if (prod_name.ToUpper().Contains("PROTECT"))
                    {
                        GenerateGPAPotectPDF(txtCertificateNumber.Text);
                    }
                    else
                    {

                        GetGPACertificateDetails(
                         ref CertificateNumber
                       , ref ProductName
                       , ref PolicyIssuingOfficeAddress
                       , ref IntermediaryName
                       , ref IntermediaryCode
                       , ref PolicyholderName
                       , ref PolicyholderAddress
                       , ref PolicyholderAddress2
                       , ref PolicyholderBusinessDescription
                       , ref PolicyholderTelephoneNumber
                       , ref PolicyholderEmailAddress
                       , ref PolicyNumber
                       , ref PolicyInceptionDateTime
                       , ref PolicyExpiryDateTime
                       , ref TotalNumberOfInsuredPersons
                       , ref RowCoverHeader
                       , ref SectionARow
                       , ref ExtSectionARow
                       , ref SectionBRow
                       , ref NameofInsuredPerson
                       , ref DateOfBirth
                       , ref Gender
                       , ref EmailId
                       , ref MobileNo
                       , ref SumInsured
                       , ref NomineeDetails
                       , ref SectionACoverPremium
                       , ref ExtensionstoSectionASectionBCoverPremium
                       , ref LoadingsDiscounts
                       , ref ServiceTax
                       , ref SwachhBharatCess
                       , ref KrishiKalyanCess
                       , ref NetPremiumRoundedOff
                       , ref StampDuty
                       , ref Receipt_Challan_No
                       , ref Receipt_Challan_No_Dated
                       , ref PolicyIssueDate
                       , ref IntermediaryLandline
                       , ref IntermediaryMobile
                       , ref TotalAmount
                       , ref IsCertificateNumberExists
                       , ref ugstPercentage
                       , ref ugstAmount
                       , ref cgstPercentage
                       , ref cgstAmount
                       , ref igstPercentage
                       , ref igstAmount
                       , ref sgstPercentage
                       , ref sgstAmount
                       , ref totalGSTAmount
                       , ref vProposerPinCode
                       , ref addCol1
                       , ref polStartDate
                       , ref UINNo
                       );
                        if (IsCertificateNumberExists)
                        {
                            string strHtml = string.Empty;

                            //DateTime dtStartDate = Convert.ToDateTime(polStartDate);

                            DateTime dtStartDate = DateTime.ParseExact(polStartDate, "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            DateTime dtGSTDate = Convert.ToDateTime(ConfigurationManager.AppSettings["GSTDate"].ToString());

                            if (dtStartDate < dtGSTDate)
                            {
                                StringWriter sw = new StringWriter();
                                StringReader sr = new StringReader(sw.ToString());
                                strHtml = htmlBody;  // sr.ReadToEnd();

                                string SignPath = ConfigurationManager.AppSettings["SignPath"].ToString();
                                strHtml = strHtml.Replace("@SignPath", SignPath);
                                strHtml = strHtml.Replace("@productname", ProductName);
                                strHtml = strHtml.Replace("@PolicyIssuingOfficeAddress", PolicyIssuingOfficeAddress);
                                strHtml = strHtml.Replace("@IntermediaryName", IntermediaryName);
                                strHtml = strHtml.Replace("@IntermediaryCode", IntermediaryCode);

                                strHtml = strHtml.Replace("@IntermediaryLandline", IntermediaryLandline);
                                strHtml = strHtml.Replace("@IntermediaryMobile", IntermediaryMobile);

                                strHtml = strHtml.Replace("@PolicyholderName", PolicyholderName);
                                strHtml = strHtml.Replace("@PolicyholderAddress", PolicyholderAddress);
                                PolicyholderAddress2 = PolicyholderAddress2.Replace("(stateCode)", "");
                                strHtml = strHtml.Replace("@PolicyholderLine2Address", PolicyholderAddress2);
                                strHtml = strHtml.Replace("@PolicyholderBusinessDescription", PolicyholderBusinessDescription);
                                strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", PolicyholderTelephoneNumber);
                                strHtml = strHtml.Replace("@PolicyholderEmailAddress", PolicyholderEmailAddress);
                                //strHtml = strHtml.Replace("@PolicyNumber", PolicyNumber + "/" + txtCertificateNumber.Text.Trim());
                                strHtml = strHtml.Replace("@PolicyNumber", txtCertificateNumber.Text.Trim());
                                strHtml = strHtml.Replace("@PolicyInceptionDateTime", PolicyInceptionDateTime);
                                //manish start
                                strHtml = strHtml.Replace("@Enroll", PolicyInceptionDateTime.Substring(24));
                                //manish end
                                //Added By Nilesh
                                string _Date = PolicyInceptionDateTime.Substring(24);
                                DateTime dt = Convert.ToDateTime(_Date);

                                string FDate = dt.ToString("dd/MM/yyyy");
                                strHtml = strHtml.Replace("@ddateForRisk", FDate);

                                //nilesh end
                                strHtml = strHtml.Replace("@PolicyExpiryDateTime", PolicyExpiryDateTime);
                                strHtml = strHtml.Replace("@TotalNumberOfInsuredPersons", TotalNumberOfInsuredPersons);

                                strHtml = strHtml.Replace("@RowCoverHeader", string.IsNullOrEmpty(RowCoverHeader) ? "" : RowCoverHeader);
                                strHtml = strHtml.Replace("@RowSectionA", string.IsNullOrEmpty(SectionARow) ? "" : SectionARow);
                                strHtml = strHtml.Replace("@RowExtSectionA", string.IsNullOrEmpty(ExtSectionARow) ? "" : ExtSectionARow);
                                strHtml = strHtml.Replace("@RowSectionB", string.IsNullOrEmpty(SectionBRow) ? "" : SectionBRow);

                                strHtml = strHtml.Replace("@NameofInsuredPerson", NameofInsuredPerson);
                                strHtml = strHtml.Replace("@DateOfBirth", DateOfBirth); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));
                                //Added By Nilesh
                                if (string.IsNullOrEmpty(Gender))
                                {
                                    strHtml = strHtml.Replace("@salutation", "");
                                }
                                else
                                {
                                    if (Gender.Trim() == "M" || Gender.Trim() == "Male")
                                    {
                                        strHtml = strHtml.Replace("@salutation", "Mr.");
                                    }
                                    else if (Gender.Trim() == "F")
                                    {
                                        strHtml = strHtml.Replace("@salutation", "Mrs.");
                                    }
                                    else
                                    {
                                        strHtml = strHtml.Replace("@salutation", "");
                                    }
                                }
                                //End By Nilesh
                                strHtml = strHtml.Replace("@Gender", Gender);
                                strHtml = strHtml.Replace("@EmailId", EmailId);
                                strHtml = strHtml.Replace("@MobileNo", MobileNo);
                                strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@NomineeDetails", NomineeDetails);
                                strHtml = strHtml.Replace("@SectionACoverPremium", Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@ExtensionstoSectionASectionBCoverPremium", Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                                strHtml = strHtml.Replace("@LoadingsDiscounts", string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@ServiceTax", Convert.ToDecimal(ServiceTax).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@SwachhBharatCess", Convert.ToDecimal(SwachhBharatCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@KrishiKalyanCess", Convert.ToDecimal(KrishiKalyanCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@NetPremiumRoundedOff", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@StampDuty", Convert.ToDecimal(StampDuty).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@Receipt_Challan_No", Receipt_Challan_No);
                                strHtml = strHtml.Replace("@Challan_No_Dated", Receipt_Challan_No_Dated);
                                strHtml = strHtml.Replace("@PolicyIssueDate", PolicyIssueDate);
                                strHtml = strHtml.Replace("@TotalAmount", TotalAmount);
                                strHtml = strHtml.Replace("@KotakGroupAccidentCareUIN", UINNo == "" ? "" : UINNo);


                                // Get the current page HTML string by rendering into a TextWriter object
                                TextWriter outTextWriter = new StringWriter();
                                HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                                base.Render(outHtmlTextWriter);

                            }
                            else
                            {
                                strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_With_GST.html";
                                htmlBody = File.ReadAllText(strPath);
                                string custStateCode = string.Empty;

                                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                                using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
                                {
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + vProposerPinCode + "'";
                                        cmd.Connection = sqlCon;
                                        sqlCon.Open();
                                        object objCustState = cmd.ExecuteScalar();
                                        custStateCode = Convert.ToString(objCustState);
                                    }
                                }
                                StringWriter sw = new StringWriter();
                                StringReader sr = new StringReader(sw.ToString());
                                strHtml = htmlBody;  // sr.ReadToEnd();

                                string SignPath = ConfigurationManager.AppSettings["SignPath"].ToString();
                                strHtml = strHtml.Replace("@SignPath", SignPath);
                                strHtml = strHtml.Replace("@certificateNo", CertificateNumber);
                                strHtml = strHtml.Replace("@productname", ProductName);
                                strHtml = strHtml.Replace("@PolicyIssuingOfficeAddress", PolicyIssuingOfficeAddress);
                                strHtml = strHtml.Replace("@IntermediaryName", IntermediaryName);
                                strHtml = strHtml.Replace("@IntermediaryCode", IntermediaryCode);

                                strHtml = strHtml.Replace("@IntermediaryLandline", IntermediaryLandline);
                                strHtml = strHtml.Replace("@IntermediaryMobile", IntermediaryMobile);

                                strHtml = strHtml.Replace("@PolicyholderName", PolicyholderName);
                                strHtml = strHtml.Replace("@PolicyholderAddress", PolicyholderAddress);
                                PolicyholderAddress2 = PolicyholderAddress2.Replace("stateCode", custStateCode);
                                strHtml = strHtml.Replace("@PolicyholderLine2Address", PolicyholderAddress2);
                                strHtml = strHtml.Replace("@PolicyholderBusinessDescription", PolicyholderBusinessDescription);
                                strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", PolicyholderTelephoneNumber);
                                strHtml = strHtml.Replace("@PolicyholderEmailAddress", PolicyholderEmailAddress);
                                //strHtml = strHtml.Replace("@PolicyNumber", PolicyNumber + "/" + txtCertificateNumber.Text.Trim());
                                strHtml = strHtml.Replace("@PolicyNumber", txtCertificateNumber.Text.Trim());
                                strHtml = strHtml.Replace("@PolicyInceptionDateTime", PolicyInceptionDateTime);
                                //manish start
                                strHtml = strHtml.Replace("@Enroll", PolicyInceptionDateTime.Substring(24));
                                //manish end
                                //Added By Nilesh
                                string _Date = PolicyInceptionDateTime.Substring(24);
                                //DateTime dt = Convert.ToDateTime(_Date);

                                //string FDate = dt.ToString("dd/MM/yyyy");
                                strHtml = strHtml.Replace("@ddateForRisk", _Date);

                                //nilesh end
                                strHtml = strHtml.Replace("@PolicyExpiryDateTime", PolicyExpiryDateTime);
                                strHtml = strHtml.Replace("@TotalNumberOfInsuredPersons", TotalNumberOfInsuredPersons);

                                strHtml = strHtml.Replace("@RowCoverHeader", string.IsNullOrEmpty(RowCoverHeader) ? "" : RowCoverHeader);
                                strHtml = strHtml.Replace("@RowSectionA", string.IsNullOrEmpty(SectionARow) ? "" : SectionARow);
                                strHtml = strHtml.Replace("@RowExtSectionA", string.IsNullOrEmpty(ExtSectionARow) ? "" : ExtSectionARow);
                                strHtml = strHtml.Replace("@RowSectionB", string.IsNullOrEmpty(SectionBRow) ? "" : SectionBRow);

                                strHtml = strHtml.Replace("@NameofInsuredPerson", NameofInsuredPerson);
                                strHtml = strHtml.Replace("@DateOfBirth", DateOfBirth); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));
                                //Added By Nilesh
                                if (string.IsNullOrEmpty(Gender))
                                {
                                    strHtml = strHtml.Replace("@salutation", "");
                                }
                                else
                                {
                                    if (Gender.Trim() == "M" || Gender.Trim() == "Male")
                                    {
                                        strHtml = strHtml.Replace("@salutation", "Mr.");
                                    }
                                    else if (Gender.Trim() == "F")
                                    {
                                        strHtml = strHtml.Replace("@salutation", "Mrs.");
                                    }
                                    else
                                    {
                                        strHtml = strHtml.Replace("@salutation", "");
                                    }
                                }
                                //End By Nilesh
                                strHtml = strHtml.Replace("@Gender", Gender);
                                strHtml = strHtml.Replace("@EmailId", EmailId);
                                strHtml = strHtml.Replace("@MobileNo", MobileNo);
                                strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@NomineeDetails", NomineeDetails);
                                strHtml = strHtml.Replace("@SectionACoverPremium", Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@extnSectionAmount", Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                                strHtml = strHtml.Replace("@LoadingsDiscounts", string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@ServiceTax", Convert.ToDecimal(ServiceTax).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@SwachhBharatCess", Convert.ToDecimal(SwachhBharatCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@KrishiKalyanCess", Convert.ToDecimal(KrishiKalyanCess).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@NetPremiumRoundedOff", Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@StampDuty", Convert.ToDecimal(StampDuty).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                                strHtml = strHtml.Replace("@Receipt_Challan_No", Receipt_Challan_No);
                                strHtml = strHtml.Replace("@Challan_No_Dated", Receipt_Challan_No_Dated);
                                strHtml = strHtml.Replace("@PolicyIssueDate", PolicyIssueDate);
                                strHtml = strHtml.Replace("@TotalAmount", TotalAmount);

                                string customString = string.Empty;

                                if (!String.IsNullOrEmpty(addCol1))
                                {
                                    string[] strArr = addCol1.Split(' ');
                                    // customString = "this " + strArr[1] + " day of " + strArr[0] + " of " + strArr[2];

                                    if (String.IsNullOrEmpty(strArr[1]))
                                    {
                                        customString = "this " + strArr[2] + " day of " + strArr[0] + " of year " + strArr[3];
                                    }
                                    else
                                    {
                                        customString = "this " + strArr[1] + " day of " + strArr[0] + " of year " + strArr[2];
                                    }

                                }

                                strHtml = strHtml.Replace("@polIssueString", customString);

                                string igstData = string.Empty;
                                string cgstugstData = string.Empty;
                                string cgstsgstData = string.Empty;

                                if (igstPercentage != "0")
                                {
                                    string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                                    igstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px'><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>IGST@" + igstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='20px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + igstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                                }
                                else
                                {
                                    if (cgstPercentage != "0" && ugstPercentage != "0")
                                    {
                                        string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                                        cgstugstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px' ><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>SGST@" + sgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>UGST@" + ugstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='10px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + ugstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + cgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                                    }
                                    if (cgstPercentage != "0" && sgstPercentage != "0")
                                    {
                                        string loadDisc = string.IsNullOrEmpty(LoadingsDiscounts) ? "0.00" : Convert.ToDecimal(LoadingsDiscounts).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                                        cgstsgstData = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='10px'><p>Section A Cover Premium (&#8377;)</p></td><td style='border:1px solid black' width='100px'><p>Extensions to Section A / Section B Cover Premium (&#8377;)</p></td><td style = 'border:1px solid black' width='50px'><p> Loadings / Discounts(&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>Taxable Value of Services (&#8377;)</p></td><td style='border:1px solid black' width='50px'><p>SGST@" + sgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='50px'><p>Total Amount (&#8377;)</p></td></tr><tr><td style='border:1px solid black;text-align:center' width='10px'><p>" + Convert.ToDecimal(SectionACoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='100px'><p>" + Convert.ToDecimal(ExtensionstoSectionASectionBCoverPremium).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + loadDisc + "</p></td><td style='border:1px solid black;text-align:center' width='50px'><p>" + Convert.ToDecimal(NetPremiumRoundedOff).ToIndianCurrencyFormatWithoutRuppeeSymbol() + "</p></td><td style='border:1px solid black' width='50px'><p>" + sgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + cgstAmount + "</p></td><td style='border:1px solid black' width='50px'><p>" + TotalAmount + "</p></td></tr></tbody></table>";
                                    }
                                }

                                strHtml = strHtml.Replace("@cgstugstData", cgstugstData == "" ? "" : cgstugstData);

                                strHtml = strHtml.Replace("@cgstsgstData", cgstsgstData == "" ? "" : cgstsgstData);

                                strHtml = strHtml.Replace("@igstData", igstData == "" ? "" : igstData);

                                strHtml = strHtml.Replace("@KotakGroupAccidentCareUIN", UINNo == "" ? "" : UINNo);


                                // Get the current page HTML string by rendering into a TextWriter object
                                TextWriter outTextWriter = new StringWriter();
                                HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                                base.Render(outHtmlTextWriter);


                            }



                            // Obtain the current page HTML string
                            string currentPageHtmlString = strHtml; //outTextWriter.ToString();

                            // Create a HTML to PDF converter object with default settings
                            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                            // Set license key received after purchase to use the converter in licensed mode
                            // Leave it not set to use the converter in demo mode
                            string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();

                            htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

                            // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                            // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                            htmlToPdfConverter.ConversionDelay = 2;

                            // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                            htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                            // Add Header

                            // Enable header in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                            // Optionally add a space between header and the page body
                            // The spacing for first page and the subsequent pages can be set independently
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                            htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");

                            // Draw header elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                                DrawHeader(htmlToPdfConverter, false);

                            // Add Footer

                            // Enable footer in the generated PDF document
                            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                            // Optionally add a space between footer and the page body
                            // Leave this option not set for no spacing
                            htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                            // Draw footer elements
                            if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                                DrawFooter(htmlToPdfConverter, false, true);

                            // Use the current page URL as base URL
                            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;

                            //// Convert the current page HTML string to a PDF document in a memory buffer
                            //// For Live
                            byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                            byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                            //// For Live End Here 

                            ////// For Dev
                            //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                            //////byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                            ////// For Dev End here





                            #region DigitalSign

                            //Winnovative.Document pdfDocument = null;
                            //pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocumentObject(currentPageHtmlString, baseUrl);

                            //HtmlElementMapping digitalSignatureMapping = htmlToPdfConverter.HtmlElementsMappingOptions.HtmlElementsMappingResult.GetElementByMappingId("digital_signature_element");
                            //if (digitalSignatureMapping != null)
                            //{
                            //    Winnovative.PdfPage digitalSignaturePage = digitalSignatureMapping.PdfRectangles[0].PdfPage;
                            //    RectangleF digitalSignatureRectangle = digitalSignatureMapping.PdfRectangles[0].Rectangle;

                            //    string certificateFilePath = @"D:\Downloads\WnvHtmlToPdf-v12.16\DemoAppFiles\Input\Certificates\wnvpdf.pfx"; //Server.MapPath("~/DemoAppFiles/Input/Certificates/wnvpdf.pfx");

                            //    // Get the certificate from password protected PFX file
                            //    DigitalCertificatesCollection certificates = DigitalCertificatesStore.GetCertificates(certificateFilePath, "winnovative");
                            //    DigitalCertificate certificate = certificates[0];

                            //    // Create the digital signature
                            //    DigitalSignatureElement signature = new DigitalSignatureElement(digitalSignatureRectangle, certificate);
                            //    signature.Reason = "Protect the document from unwanted changes";
                            //    signature.ContactInfo = "The contact email is support@winnovative-software.com";
                            //    signature.Location = "Development server";
                            //    digitalSignaturePage.AddElement);
                            //}

                            //// Save the PDF document in a memory buffer
                            //outPdfBuffer = pdfDocument.Save();

                            #endregion

                            // Send the PDF as response to browser

                            // Set response content type
                            Response.AddHeader("Content-Type", "application/pdf");

                            // Instruct the browser to open the PDF file as an attachment or inline
                            //Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), txtCertificateNumber.Text.Trim()));

                            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), txtCertificateNumber.Text.Trim()));

                            // Write the PDF document buffer to HTTP response
                            //Response.BinaryWrite(outPdfBuffer);
                            Response.BinaryWrite(outPdfBuffer);

                            // End the HTTP response and stop the current page processing
                            CommonExtensions.fn_AddLogForDownload(CertificateNumber, "FrmGPAGetPolicy.aspx");//Added By Rajesh Soni on 19/02/2020   
                            Response.End();

                            
                        }
                        else
                        {
                            base.Render(writer);
                            //Alert.Show("Certificate Number Does Not Exists", "FrmGPAGetPolicy.aspx");
                            //return;
                        }
                    }
                }
                else
                {
                    base.Render(writer);
                }
            }

            else if (rdoHDColicyType.Checked == true)
            {

                try
                {
                    if (!string.IsNullOrEmpty(txtCertificateNumber.Text))
                    {
                        string CertificateNumber = txtCertificateNumber.Text;
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ToString()))
                        {
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            //using (SqlCommand cmd = new SqlCommand())
                            {
                                //cmd.CommandText = "SELECT top 1 vCertificateNo from TBL_HDC_POLICY_TABLE where vUploadId not like '%can%' and vCertificateNo=" + "'" + CertificateNumber + "' order by dCreatedDate desc";
                                //cmd.CommandText = "SELECT top 1 vCertificateNo from tbl_hdc_policy_replica_table where vUploadId not like '%can%' and vCertificateNo=" + "'" + CertificateNumber + "' order by dCreatedDate desc" 
                                //    +"UNION ALL SELECT top 1 vCertificateNo from tbl_hdc_policy_table where vUploadId not like '%can%' and vCertificateNo = " + "'" + CertificateNumber + "' order by dCreatedDate desc"
                                //    +"AND NOT EXISTS (SELECT top 1 vCertificateNo from tbl_hdc_policy_replica_table where vUploadId not like '%can%' and vCertificateNo = " + "'" + CertificateNumber + "' order by dCreatedDate desc)";
                                SqlCommand cmd = new SqlCommand("PROC_GET_REPLICA_HDC_CERTIFICATENO", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@vCertificateNo", txtCertificateNumber.Text.Trim());
                               // cmd.Connection = con;
                                object objProd = cmd.ExecuteScalar();
                                CertificateNumber = Convert.ToString(objProd);
                                con.Close();
                                if (!string.IsNullOrEmpty(CertificateNumber))
                                {
                                    createHDCPDF(CertificateNumber);
                                }
                                else
                                {
                                    base.Render(writer);
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(txtLoanAccountNumber.Text) && !string.IsNullOrEmpty(txtCustDOB.Text))
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ToString()))
                        {
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }

                            //SqlCommand cmd = new SqlCommand("PROC_GET_HDC_CERTIFICATE_BY_DOB_LOAN_ACCOUNT_NUMBER", con);
                            SqlCommand cmd = new SqlCommand("PROC_GET_REPLICA_HDC_CERTIFICATE_BY_DOB_LOAN_ACCOUNT_NUMBER", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@vCustomerDob", txtCustDOB.Text.Trim());
                            cmd.Parameters.AddWithValue("@vAccountNo", txtLoanAccountNumber.Text.Trim());
                            string vCertificateNumber = Convert.ToString(cmd.ExecuteScalar());
                            if (!string.IsNullOrEmpty(vCertificateNumber))
                            {
                                createHDCPDF(vCertificateNumber);
                            }
                            else
                            {
                                //return;
                                //Alert.Show("HDC Policy details not available.");
                                base.Render(writer);
                            }

                        }
                    }

                    //CommonExtensions.fn_AddLogForDownload(CertificateNumber, "FrmGPAGetPolicy.aspx");//Added By Rajesh Soni on 19/02/2020
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogException(ex, "createHDCPDF");
                }

            }
            else
            {
                base.Render(writer);
            }


        }

        

        private void createHDCPDF(string certificateNumber)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPass"].ToString()))
            {
                con.Open();
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_PDF_CompleteLetter.html";
                string htmlBody = File.ReadAllText(strPath);
                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(sw.ToString());
                string strHtml = htmlBody;

                //SqlCommand command = new SqlCommand("PROC_GET_COVER_SECTION_DATA_FOR_HDC_PDF_TEST", con);
                SqlCommand command = new SqlCommand("PROC_GET_REPLICA_COVER_SECTION_DATA_FOR_HDC_PDF_TEST", con);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                command.Parameters.AddWithValue("@vCertificateNo", certificateNumber);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    // Change the logic to print pdf as there is same format for Floater and Individual policy

                    //if (ds.Tables[0].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["InsuredName2"].ToString()))
                    if (ds.Tables[0].Rows.Count > 0 )
                    {
                        strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_Floater_PDF_CompleteLetter.html";
                        htmlBody = File.ReadAllText(strPath);
                        sw = new StringWriter();
                        sr = new StringReader(sw.ToString());
                        strHtml = htmlBody;
                        Generate_HDC_Floater_PDF(con, ds, strHtml, certificateNumber);
                    }


                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    strPath = AppDomain.CurrentDomain.BaseDirectory + "HDC_PDF_CompleteLetter.html";
                    //    htmlBody = File.ReadAllText(strPath);
                    //    sw = new StringWriter();
                    //    sr = new StringReader(sw.ToString());
                    //    strHtml = htmlBody;

                    //    GenerateNonEmailHDCPDF(con, ds, strHtml, certificateNumber);
                    //}
                }
            }
        }

        private void Generate_HDC_Floater_PDF(SqlConnection con, DataSet ds, string strHtml, string certificateNumber)
        {
            try
            {
                string accidentalDeath = string.Empty;
                string permTotalDisable = string.Empty;
                string permPartialDisable = string.Empty;
                string tempTotalDisable = string.Empty;
                string carraigeBody = string.Empty;
                string funeralExpense = string.Empty;
                string medicalExpense = string.Empty;
                string purchaseBlood = string.Empty;
                string transportation = string.Empty;
                string compassionate = string.Empty;
                string disappearance = string.Empty;
                string modifyResidence = string.Empty;
                string costOfSupport = string.Empty;
                string commonCarrier = string.Empty;
                string childrenGrant = string.Empty;
                string marraigeExpense = string.Empty;
                string sportsActivity = string.Empty;
                string widowHood = string.Empty;

                string ambulanceChargesString = string.Empty;
                string dailyCashString = string.Empty;
                string accidentalHospString = string.Empty;
                string opdString = string.Empty;
                string accidentalDentalString = string.Empty;
                string convalString = string.Empty;
                string burnsString = string.Empty;
                string brokenBones = string.Empty;
                string comaString = string.Empty;
                string domesticTravelString = string.Empty;
                string lossofEmployString = string.Empty;
                string onDutyCover = string.Empty;
                string legalExpenses = string.Empty;

                string reducingCoverString = string.Empty;
                string assignmentString = string.Empty;

                //gst
                string custStateCode = string.Empty;
                string igstString = string.Empty;
                string cgstsgstString = string.Empty;
                string cgstugstString = string.Empty;
                string vCustomerType = string.Empty;
                #region HDC CERTIFICATE OF INSURANCE

                strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                strHtml = strHtml.Replace("@masterDate", ds.Tables[0].Rows[0]["vMasterPolicyIssueDate"].ToString());
                strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyIssueLocation"].ToString());
                strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vMasterPolicyHolder"].ToString());
                strHtml = strHtml.Replace("@Customers", ds.Tables[0].Rows[0]["vGroupType"].ToString());
                strHtml = strHtml.Replace("@GroupType", ds.Tables[0].Rows[0]["vGroupType"].ToString());
                vCustomerType = ds.Tables[0].Rows[0]["vCustomerType"].ToString();

                string AccountNo = ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString() + " / " + ds.Tables[0].Rows[0]["vAccountNo"].ToString();
                if (AccountNo.Substring(0, 3) == " / ")
                {
                    AccountNo = AccountNo.Substring(2, AccountNo.Length - 2);
                }
                strHtml = strHtml.Replace("@MemberShipIDEmpNOAccNo", AccountNo);
                strHtml = strHtml.Replace("@CreditAmountOutStandingCreditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                strHtml = strHtml.Replace("@CreditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
                strHtml = strHtml.Replace("@DeductibleBaseCovers", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
                strHtml = strHtml.Replace("@DescriptionRemark", ds.Tables[0].Rows[0]["vComments"].ToString());
                strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
                strHtml = strHtml.Replace("@ProposarGSTN", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                //Added By Nilesh
                strHtml = strHtml.Replace("@ProposarMobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                strHtml = strHtml.Replace("@ProposarEmailId", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                //End By Nilesh
                strHtml = strHtml.Replace("@PolicyType", ds.Tables[0].Rows[0]["vPolicyType"].ToString() == "" ? "New" : Convert.ToString(ds.Tables[0].Rows[0]["vPolicyType"]));
                strHtml = strHtml.Replace("@PreviousPolicyNo", ds.Tables[0].Rows[0]["vAddCol2"].ToString() == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["vAddCol2"].ToString()));
                #endregion


                #region DETAILS OF THE INSURED PERSON(S) UNDER THE POLICY

                strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
                strHtml = strHtml.Replace("@issuedBranchkgi", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
                strHtml = strHtml.Replace("@issuedDate", ds.Tables[0].Rows[0]["vTransactionDate"].ToString());
                strHtml = strHtml.Replace("@startDate", ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString());
                strHtml = strHtml.Replace("@endDate", ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString());
                strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString());// + ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                strHtml = strHtml.Replace("@FinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());
                strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
                strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                strHtml = strHtml.Replace("@gstin", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
                strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["vCustomerDob"].ToString());
                strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                strHtml = strHtml.Replace("@category", ds.Tables[0].Rows[0]["vCustomerCategory"].ToString());
                strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());
                strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeDOB"].ToString());
                strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vNomineeAppointee"].ToString());
                strHtml = strHtml.Replace("@deduct", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
                strHtml = strHtml.Replace("@cuAadhar", ds.Tables[0].Rows[0]["vCustomerAadhar"].ToString());
                strHtml = strHtml.Replace("@proposeradd", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());



                #endregion


                #region  INTERMEDIARY DETAILS
                strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryContact"].ToString());
                strHtml = strHtml.Replace("@intermediaryLandline", ds.Tables[0].Rows[0]["vIntermediaryLandline"].ToString());

                if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                {
                    strHtml = strHtml.Replace("KOTAK GROUP SMART CASH", "KOTAK GROUP SMART CASH – MICRO INSURANCE");
                    strHtml = strHtml.Replace("Kotak Group Smart Cash", "Kotak Group Smart Cash – Micro Insurance");
                }


                if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
                {
                    strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash – Micro Insurance UIN:KOTHMGP076V011819");
                }
                else
                {
                    strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash UIN: KOTHLGP19014V011819");
                }


                #endregion

                #region  COVERAGE DETAILS

                #region Code for Covers

                StringBuilder coverstring = new StringBuilder();
                if (ds.Tables[1].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        coverstring.Append("<tr>");
                        if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "B")
                        {
                            coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Base Covers </strong></td>");
                            coverstring.Append("</tr><tr>");
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        else if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "O")
                        {

                            coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Optional Covers </strong></td>");
                            coverstring.Append("</tr><tr>");
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        else {
                            coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                            coverstring.Append("<td style='border: 1px solid black; '><p >" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                        }
                        coverstring.Append("</tr>");
                    }
                }

                #endregion

                strHtml = strHtml.Replace("@CoversString", coverstring.ToString());


                #region Code for Important Conditions

                strHtml = strHtml.Replace("@Condition1", ds.Tables[0].Rows[0]["vCondition1"].ToString());
                strHtml = strHtml.Replace("@Condition2", ds.Tables[0].Rows[0]["vCondition2"].ToString());
                strHtml = strHtml.Replace("@Condition3", ds.Tables[0].Rows[0]["vCondition3"].ToString());
                strHtml = strHtml.Replace("@Condition4", ds.Tables[0].Rows[0]["vCondition4"].ToString());
                strHtml = strHtml.Replace("@Condition5", ds.Tables[0].Rows[0]["vCondition5"].ToString());

                #endregion

                #endregion

                #region  PREMIUM DETAILS

                strHtml = strHtml.Replace("@cgstpercentagestring", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@ugstpercentagestring", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@sgstpercentagestring", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
                strHtml = strHtml.Replace("@igstpercentagestring", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());


                strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
                strHtml = strHtml.Replace("@cgstsgstString", ds.Tables[0].Rows[0]["vCGST"].ToString());
                strHtml = strHtml.Replace("@ugstsgstString", ds.Tables[0].Rows[0]["vUGST"].ToString());
                strHtml = strHtml.Replace("@sgstsgstString", ds.Tables[0].Rows[0]["vSGST"].ToString());
                strHtml = strHtml.Replace("@igstsgstString", ds.Tables[0].Rows[0]["vIGST"].ToString());
                strHtml = strHtml.Replace("@TotalpremiumString", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());

                #endregion


                #region  TAX DETAILS

                strHtml = strHtml.Replace("@gstn", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
                strHtml = strHtml.Replace("@Category", ds.Tables[0].Rows[0]["vCategory"].ToString());
                strHtml = strHtml.Replace("@SACCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
                strHtml = strHtml.Replace("@Description", ds.Tables[0].Rows[0]["vDescription"].ToString());
                strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());


                strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["vStampDuty"].ToString());
                strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
                strHtml = strHtml.Replace("@challanDate", ds.Tables[0].Rows[0]["vChallanDate"].ToString());


                #endregion

                #region Floater Policy Details
                DataTable dtFloaterNominee = ds.Tables[2];


                string tbodyFloaterNominee = string.Empty;
                tbodyFloaterNominee = @"<tr><td style='border: 1px solid black;width:90px'> Insured Name </th> <td style='border: 1px solid black'> Insured Relationship</th> <td style='border: 1px solid black'> Insured Type </th> <td style='border: 1px solid black' width='12%'> DOB/AGE </th><td style='border: 1px solid black'> Gender </th><td style='border: 1px solid black'> Nominee Name </th><td style='border: 1px solid black' width='5%'> Nominee Relation </th><td style='border: 1px solid black' width='8%'> Nominee DOB/AGE </th></tr> ";

                foreach (DataRow dr in dtFloaterNominee.Rows)
                {
                    strHtml = strHtml.Replace("@ProposarPanAdhar", dr["CustomerPANorAdhar"].ToString());
                    strHtml = strHtml.Replace("@NameofFinancier", dr["vFinancerName"].ToString());
                    strHtml = strHtml.Replace("@ProposarName", dr["vCustomerName"].ToString());
                    strHtml = strHtml.Replace("@ProposarAddress", dr["ProposarAddress"].ToString());

                    if (!string.IsNullOrEmpty(dr["InsuredName1"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredName1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredRelation1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredDOB1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["InsuredGender1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeName1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeRelation1"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'> " + dr["NomineeDOB1"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }


                    if (!string.IsNullOrEmpty(dr["InsuredName2"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation2"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB2"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName3"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation3"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB3"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                }


                    if (!string.IsNullOrEmpty(dr["InsuredName4"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation4"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB4"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName5"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation5"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB5"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }



                    if (!string.IsNullOrEmpty(dr["InsuredName6"].ToString()))
                    {
                        tbodyFloaterNominee += "<tr>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredName6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredRelation6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + vCustomerType + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredDOB6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["InsuredGender6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeName6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeRelation6"].ToString() + " </td>";
                        tbodyFloaterNominee += "<td style='border: 1px solid black; font-size:small'>" + dr["NomineeDOB6"].ToString() + " </td>";
                        tbodyFloaterNominee += "</tr>";
                    }

                }

                strHtml = strHtml.Replace("@tbody", tbodyFloaterNominee.ToString());
                //strHtml = strHtml.Replace("NULL", "");

                #endregion

                #region HDC RISK

                //strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());


                string _Date1 = ds.Tables[0].Rows[0]["vTransactionDate"].ToString();
                DateTime dtDateHDCRisk = Convert.ToDateTime(_Date1);

                string TransactionDateHDCRisk = dtDateHDCRisk.ToString("dd/MM/yyyy");
                strHtml = strHtml.Replace("@TransactionDateHDCRisk", TransactionDateHDCRisk);
                string mentionedGender = ds.Tables[0].Rows[0]["vCustomerGender"].ToString();
                if (string.IsNullOrEmpty(mentionedGender))
                {
                    strHtml = strHtml.Replace("@salutation", "");
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "M" || ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "Male")
                    {
                        strHtml = strHtml.Replace("@salutation", "Mr.");
                    }
                    else if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "F")
                    {
                        strHtml = strHtml.Replace("@salutation", "Mrs.");
                    }
                    else
                    {
                        strHtml = strHtml.Replace("@salutation", "");
                    }
                }
                strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                strHtml = strHtml.Replace("@addressofinsured1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                strHtml = strHtml.Replace("@addressofinsured2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                strHtml = strHtml.Replace("@addressofinsured3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                strHtml = strHtml.Replace("@addressofinsuredCity", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                strHtml = strHtml.Replace("@addressofinsuredState", ds.Tables[0].Rows[0]["vProposerState"].ToString());
                strHtml = strHtml.Replace("@Pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                strHtml = strHtml.Replace("@mobileno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());

                strHtml = strHtml.Replace("@productname", ds.Tables[0].Rows[0]["vProductName"].ToString());
                #endregion

                #region HDC 80D CERTIFICATE
                string _Date = ds.Tables[0].Rows[0]["vAccountDebitDate"].ToString();
                DateTime dt = Convert.ToDateTime(_Date);

                string FDate = dt.ToString("dd/MM/yyyy");
                strHtml = strHtml.Replace("@ddateForRisk", FDate);
                strHtml = strHtml.Replace("@TotalPremium", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());
                strHtml = strHtml.Replace("@paymentmode", ds.Tables[0].Rows[0]["vPaymentMode"].ToString());
                int policytnur = Convert.ToInt32(ds.Tables[0].Rows[0]["vPolicyTenure"].ToString());
                //double totalpremium = Convert.ToDouble(ds.Tables[0].Rows[0]["vTotalPremium"].ToString());


                string startdate = ds.Tables[0].Rows[0]["vPolicyStartDate"].ToString();
                string enddate = ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString();

                DateTime date = Convert.ToDateTime(startdate);
                string startdateyear1 = date.Year.ToString();
                int MonthofStartYear = date.Month;
                DateTime date1 = Convert.ToDateTime(enddate);
                string enddateyear2 = date1.Year.ToString();
                int shortenddateyear2 = Convert.ToInt32(enddateyear2.Substring(2)) - 1;
                string year5 = Convert.ToString(shortenddateyear2);
                string FYForLUMSUMyear4;
                if (MonthofStartYear > 3)
                {
                    FYForLUMSUMyear4 = Convert.ToInt32(startdateyear1) + "-" + (Convert.ToInt32(startdateyear1) + 1);
                }
                else
                {
                    FYForLUMSUMyear4 = (Convert.ToInt32(startdateyear1) - 1) + "-" + (startdateyear1.Substring(2));
                }
                strHtml = strHtml.Replace("@Year", FYForLUMSUMyear4);

                int YearDuration = Convert.ToInt32(enddateyear2) - Convert.ToInt32(startdateyear1);
                string totalpremiumamount = ds.Tables[0].Rows[0]["vTotalPremium"].ToString();
                double totalpremiumamt = Convert.ToDouble(totalpremiumamount);
                double amount2 = totalpremiumamt / YearDuration;
                //string amount2 = Convert.ToString(amount1);
                double amount = Math.Round(amount2, 2);
                StringBuilder sb = new StringBuilder();
                sb.Append("<table style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri'>");
                sb.Append("<td style='width:200;border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");
                sb.Append("<p style='margin-left: 20px;'><span>Financial Year</span></p>");
                sb.Append("</td>");
                sb.Append("<td style='width:650;border:1px solid black;border-collapse:collapse;'>");
                sb.Append("<p style='margin-left: 20px;'><span>Year wise proportionate premium allowed for Deduction under Section 80D</span></p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                string FYForYearWiseLumsumDividendYear02;
                if (MonthofStartYear > 3)
                {
                    FYForYearWiseLumsumDividendYear02 = startdateyear1;
                }
                else
                {
                    int Yeart = Convert.ToInt32(startdateyear1) - 1;
                    FYForYearWiseLumsumDividendYear02 = Convert.ToString(Yeart);
                }
                for (int H = 0; H < YearDuration; H++)
                {
                    DataTable dt1 = new DataTable();
                    sb.Append("<tr style='border:1px solid black;border-collapse:collapse;font-family:Calibri;'>");

                    int Year00 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02) + H;
                    int sum = H + 1;
                    int Year01 = Convert.ToInt32(FYForYearWiseLumsumDividendYear02.Substring(2)) + sum;

                    string year6 = Convert.ToString(Year00) + "-" + Convert.ToString(Year01);

                    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:200;'>");
                    sb.Append("<p style='margin-left: 20px;'> " + year6 + " </p>");
                    sb.Append("</td>");
                    sb.Append("<td style='border:1px solid black;border-collapse:collapse;width:650;'>");
                    sb.Append("<p style='margin-left: 20px;'> " + amount + " </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                strHtml = strHtml.Replace("@testHTMLTABLE", sb.ToString());
                #endregion

                strHtml = strHtml.Replace("@KotakGroupSmartCashUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupSmartCashUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupSmartCashUIN"]));
                // below code for download pdf
                TextWriter outTextWriter = new StringWriter();
                HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                base.Render(outHtmlTextWriter);
                string currentPageHtmlString = strHtml; //outTextWriter.ToString();
                                                        // Create a HTML to PDF converter object with default settings
                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
                // Set license key received after purchase to use the converter in licensed mode
                // Leave it not set to use the converter in demo mode
                string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();
                htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";
                                                                 // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                                                                 // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                htmlToPdfConverter.ConversionDelay = 2;
                // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
                // Add Header
                // Enable header in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                // Optionally add a space between header and the page body
                // The spacing for first page and the subsequent pages can be set independently
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");
                // Draw header elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                    DrawHeader(htmlToPdfConverter, false);
                // Add Footer
                // Enable footer in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
                // Optionally add a space between footer and the page body
                // Leave this option not set for no spacing
                htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");
                // Draw footer elements
                if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                    DrawFooter(htmlToPdfConverter, false, true);
                // Use the current page URL as base URL
                string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                // Convert the current page HTML string to a PDF document in a memory buffer
               //// For Live
               // byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
               // byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
               // // For Live End Here

                //// For Dev
                byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                //// For Dev End here 

                // Send the PDF as response to browser

                // Set response content type
                Response.AddHeader("Content-Type", "application/pdf");

                // Instruct the browser to open the PDF file as an attachment or inline
                //Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), txtCertificateNumber.Text.Trim()));

                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=HDCPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNumber.Trim().ToString()));

                // Write the PDF document buffer to HTTP response
                //Response.BinaryWrite(outPdfBuffer);
                Response.BinaryWrite(outPdfBuffer);

                // End the HTTP response and stop the current page processing
                CommonExtensions.fn_AddLogForDownload(certificateNumber, "FrmGPAGetPolicy.aspx");//Added By Rajesh Soni on 19/02/2020
                Response.End();

                
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Generate_HDC_Floater_PDF  , FrmGPAGetPolicy.aspx");
            }
        }

        private void GenerateNonEmailHDCPDF(SqlConnection con, DataSet ds, string strHtml, string certificateNumber)
        {
            string accidentalDeath = string.Empty;
            string permTotalDisable = string.Empty;
            string permPartialDisable = string.Empty;
            string tempTotalDisable = string.Empty;
            string carraigeBody = string.Empty;
            string funeralExpense = string.Empty;
            string medicalExpense = string.Empty;
            string purchaseBlood = string.Empty;
            string transportation = string.Empty;
            string compassionate = string.Empty;
            string disappearance = string.Empty;
            string modifyResidence = string.Empty;
            string costOfSupport = string.Empty;
            string commonCarrier = string.Empty;
            string childrenGrant = string.Empty;
            string marraigeExpense = string.Empty;
            string sportsActivity = string.Empty;
            string widowHood = string.Empty;

            string ambulanceChargesString = string.Empty;
            string dailyCashString = string.Empty;
            string accidentalHospString = string.Empty;
            string opdString = string.Empty;
            string accidentalDentalString = string.Empty;
            string convalString = string.Empty;
            string burnsString = string.Empty;
            string brokenBones = string.Empty;
            string comaString = string.Empty;
            string domesticTravelString = string.Empty;
            string lossofEmployString = string.Empty;
            string onDutyCover = string.Empty;
            string legalExpenses = string.Empty;

            string reducingCoverString = string.Empty;
            string assignmentString = string.Empty;

            //gst
            string custStateCode = string.Empty;
            string igstString = string.Empty;
            string cgstsgstString = string.Empty;
            string cgstugstString = string.Empty;

            #region HDC CERTIFICATE OF INSURANCE

            strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
            strHtml = strHtml.Replace("@masterDate", ds.Tables[0].Rows[0]["vMasterPolicyIssueDate"].ToString());
            strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyIssueLocation"].ToString());
            strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vMasterPolicyHolder"].ToString());
            strHtml = strHtml.Replace("@Customers", ds.Tables[0].Rows[0]["vGroupType"].ToString());
            strHtml = strHtml.Replace("@GroupType", ds.Tables[0].Rows[0]["vGroupType"].ToString());



            #endregion


            #region DETAILS OF THE INSURED PERSON(S) UNDER THE POLICY

            strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
            strHtml = strHtml.Replace("@policyCategory", ds.Tables[0].Rows[0]["vPolicyCategory"].ToString());
            strHtml = strHtml.Replace("@issuedBranchkgi", ds.Tables[0].Rows[0]["vKGIBranchAddress"].ToString());
            strHtml = strHtml.Replace("@issuedDate", ds.Tables[0].Rows[0]["vTransactionDate"].ToString());
            strHtml = strHtml.Replace("@startDate", ds.Tables[0].Rows[0]["vPolicyStartdate"].ToString());
            strHtml = strHtml.Replace("@endDate", ds.Tables[0].Rows[0]["vPolicyEndDate"].ToString());
            strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vUnique_Id_No"].ToString());// + ds.Tables[0].Rows[0]["vAccountNo"].ToString());
            strHtml = strHtml.Replace("@FinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());
            strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());
            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
            strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
            strHtml = strHtml.Replace("@gstin", ds.Tables[0].Rows[0]["vCustomerGSTIN"].ToString());
            strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
            strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["vCustomerDob"].ToString());
            strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
            strHtml = strHtml.Replace("@category", ds.Tables[0].Rows[0]["vCustomerCategory"].ToString());
            strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
            strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
            strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());
            strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
            strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeDOB"].ToString());
            strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vNomineeAppointee"].ToString());
            strHtml = strHtml.Replace("@deduct", ds.Tables[0].Rows[0]["vDeductableofBaseCover"].ToString());
            strHtml = strHtml.Replace("@cuAadhar", ds.Tables[0].Rows[0]["vCustomerAadhar"].ToString());
            strHtml = strHtml.Replace("@proposeradd", ds.Tables[0].Rows[0]["vProposeAdd"].ToString());



            #endregion


            #region  INTERMEDIARY DETAILS
            strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
            strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
            strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryContact"].ToString());
            strHtml = strHtml.Replace("@intermediaryLandline", ds.Tables[0].Rows[0]["vIntermediaryLandline"].ToString());


            if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
            {
                strHtml = strHtml.Replace("KOTAK GROUP SMART CASH", "KOTAK GROUP SMART CASH – MICRO INSURANCE");
                strHtml = strHtml.Replace("Kotak Group Smart Cash", "Kotak Group Smart Cash – Micro Insurance");
            }

            if (ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString().Substring(0, 2) == "13")
            {
                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash – Micro Insurance UIN:KOTHMGP076V011819");
            }
            else
            {
                strHtml = strHtml.Replace("@ProductWithUIN", "Kotak Group Smart Cash UIN: KOTHLGP19014V011819");
            }
            #endregion

            #region  COVERAGE DETAILS

            #region Code for Covers

            StringBuilder coverstring = new StringBuilder();
            if (ds.Tables[1].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    coverstring.Append("<tr>");
                    if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "B")
                    {
                        coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Base Covers </strong></td>");
                        coverstring.Append("</tr><tr>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center;font-size:small'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                    }
                    else if (ds.Tables[1].Rows[i]["SRNO"].ToString() == "1" && ds.Tables[1].Rows[i]["vCoverType"].ToString() == "O")
                    {

                        coverstring.Append("<td style='border-left: 1px solid black;'></td><td colspan='2' style='border-right: 1px solid black;'><strong>Optional Covers </strong></td>");
                        coverstring.Append("</tr><tr>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center;font-size:small'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                    }
                    else {
                        coverstring.Append("<td style='border: 1px solid black; '><p style='text-align:center;font-size:small'>" + ds.Tables[1].Rows[i]["SRNO"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["vCovers"].ToString() + "</p></td>");
                        coverstring.Append("<td style='border: 1px solid black; '><p style='font-size:small'>" + ds.Tables[1].Rows[i]["Orders"].ToString() + "</p></td>");
                    }
                    coverstring.Append("</tr>");
                }
            }

            #endregion

            strHtml = strHtml.Replace("@CoversString", coverstring.ToString());


            #region Code for Important Conditions

            strHtml = strHtml.Replace("@Condition1", ds.Tables[0].Rows[0]["vCondition1"].ToString());
            strHtml = strHtml.Replace("@Condition2", ds.Tables[0].Rows[0]["vCondition2"].ToString());
            strHtml = strHtml.Replace("@Condition3", ds.Tables[0].Rows[0]["vCondition3"].ToString());
            strHtml = strHtml.Replace("@Condition4", ds.Tables[0].Rows[0]["vCondition4"].ToString());
            strHtml = strHtml.Replace("@Condition5", ds.Tables[0].Rows[0]["vCondition5"].ToString());

            #endregion

            #endregion

            #region  PREMIUM DETAILS

            strHtml = strHtml.Replace("@cgstpercentagestring", ds.Tables[0].Rows[0]["vCGSTPercentage"].ToString());
            strHtml = strHtml.Replace("@ugstpercentagestring", ds.Tables[0].Rows[0]["vUGSTPercentage"].ToString());
            strHtml = strHtml.Replace("@sgstpercentagestring", ds.Tables[0].Rows[0]["vSGSTPercentage"].ToString());
            strHtml = strHtml.Replace("@igstpercentagestring", ds.Tables[0].Rows[0]["vIGSTPercentage"].ToString());


            strHtml = strHtml.Replace("@NetPremiumString", ds.Tables[0].Rows[0]["vNetPremium"].ToString());
            strHtml = strHtml.Replace("@cgstsgstString", ds.Tables[0].Rows[0]["vCGST"].ToString());
            strHtml = strHtml.Replace("@ugstsgstString", ds.Tables[0].Rows[0]["vUGST"].ToString());
            strHtml = strHtml.Replace("@sgstsgstString", ds.Tables[0].Rows[0]["vSGST"].ToString());
            strHtml = strHtml.Replace("@igstsgstString", ds.Tables[0].Rows[0]["vIGST"].ToString());
            strHtml = strHtml.Replace("@TotalpremiumString", ds.Tables[0].Rows[0]["vTotalPremium"].ToString());

            #endregion


            #region  TAX DETAILS

            strHtml = strHtml.Replace("@gstn", ds.Tables[0].Rows[0]["vKGIGSTN"].ToString());
            strHtml = strHtml.Replace("@Category", ds.Tables[0].Rows[0]["vCategory"].ToString());
            strHtml = strHtml.Replace("@SACCode", ds.Tables[0].Rows[0]["vSacCode"].ToString());
            strHtml = strHtml.Replace("@Description", ds.Tables[0].Rows[0]["vDescription"].ToString());
            strHtml = strHtml.Replace("@invoiceno", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());


            strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["vStampDuty"].ToString());
            strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNo"].ToString());
            strHtml = strHtml.Replace("@challanDate", ds.Tables[0].Rows[0]["vChallanDate"].ToString());


            #endregion

            // below code for download pdf
            TextWriter outTextWriter = new StringWriter();
            HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
            base.Render(outHtmlTextWriter);
            string currentPageHtmlString = strHtml; //outTextWriter.ToString();
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();
            htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";
            // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
            // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
            htmlToPdfConverter.ConversionDelay = 2;
            // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
            htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);
            // Add Header
            // Enable header in the generated PDF document
            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
            // Optionally add a space between header and the page body
            // The spacing for first page and the subsequent pages can be set independently
            // Leave this option not set for no spacing
            htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
            htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");
            // Draw header elements
            if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                DrawHeader(htmlToPdfConverter, false);
            // Add Footer
            // Enable footer in the generated PDF document
            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            // Optionally add a space between footer and the page body
            // Leave this option not set for no spacing
            htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");
            // Draw footer elements
            if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                DrawFooter(htmlToPdfConverter, false, true);
            // Use the current page URL as base URL
            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            // Convert the current page HTML string to a PDF document in a memory buffer
            //For Live
            byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
            byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
            //For Live End Here

            //// For Dev
            //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
            //// byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
            //// For Dev End here 

            // Send the PDF as response to browser
            // Set response content type
            Response.AddHeader("Content-Type", "application/pdf");
            // Instruct the browser to open the PDF file as an attachment or inline
            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=HDCPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), certificateNumber.Trim().ToString()));
            // Write the PDF document buffer to HTTP response
            //Response.BinaryWrite(outPdfBuffer);
            Response.BinaryWrite(outPdfBuffer);
            // End the HTTP response and stop the current page processing
            CommonExtensions.fn_AddLogForDownload(certificateNumber, "FrmGPAGetPolicy.aspx");//Added By Rajesh Soni on 19/02/2020 
            Response.End();

            
        }

        private void GenerateGPAPotectPDF(string certNo)
        {

            string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_CompleteLetter_With_GST.html";
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            using (SqlConnection sqlCon = new SqlConnection(db.ConnectionString))
            {
                sqlCon.Open();

                string htmlBody = File.ReadAllText(strPath);
                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(sw.ToString());
                string strHtml = htmlBody;

                SqlCommand command = new SqlCommand("PROC_GET_COVER_SECTION_DATA_FOR_PDF_TEST", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@vCertificateNo", "271216000116");
                command.Parameters.AddWithValue("@vCertificateNo", certNo);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //DateTime dtStartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                        DateTime dtStartDate = DateTime.ParseExact(ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString(), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dtGSTDate = Convert.ToDateTime(ConfigurationManager.AppSettings["GSTDate"].ToString());


                        string accidentalDeath = string.Empty;
                        string permTotalDisable = string.Empty;
                        string permPartialDisable = string.Empty;
                        string tempTotalDisable = string.Empty;
                        string carraigeBody = string.Empty;
                        string funeralExpense = string.Empty;
                        string medicalExpense = string.Empty;
                        string purchaseBlood = string.Empty;
                        string transportation = string.Empty;
                        string compassionate = string.Empty;
                        string disappearance = string.Empty;
                        string modifyResidence = string.Empty;
                        string costOfSupport = string.Empty;
                        string commonCarrier = string.Empty;
                        string childrenGrant = string.Empty;
                        string marraigeExpense = string.Empty;
                        string sportsActivity = string.Empty;
                        string widowHood = string.Empty;

                        string ambulanceChargesString = string.Empty;
                        string dailyCashString = string.Empty;
                        string accidentalHospString = string.Empty;
                        string opdString = string.Empty;
                        string accidentalDentalString = string.Empty;
                        string convalString = string.Empty;
                        string burnsString = string.Empty;
                        string brokenBones = string.Empty;
                        string comaString = string.Empty;
                        string domesticTravelString = string.Empty;
                        string lossofEmployString = string.Empty;
                        string onDutyCover = string.Empty;
                        string legalExpenses = string.Empty;

                        string reducingCoverString = string.Empty;
                        string assignmentString = string.Empty;
                        string custStateCode = string.Empty;
                        string igstString = string.Empty;
                        string cgstsgstString = string.Empty;
                        string cgstugstString = string.Empty;

                        if (dtStartDate >= dtGSTDate)
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = "SELECT TOP 1 NUM_STATE_CD FROM STATE_CITY_DISTRICT_PINCODE WHERE NUM_PINCODE='" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString() + "'";
                                cmd.Connection = sqlCon;

                                object objCustState = cmd.ExecuteScalar();
                                custStateCode = Convert.ToString(objCustState);
                            }


                            strHtml = strHtml.Replace("@createdDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dCreatedDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@customerName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeAge"].ToString());
                            strHtml = strHtml.Replace("@masterDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["vMasterPolicyDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());


                            strHtml = strHtml.Replace("@addressline1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@addressline2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());

                            strHtml = strHtml.Replace("@addressline3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                            strHtml = strHtml.Replace("@city", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                            strHtml = strHtml.Replace("@pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());
                            strHtml = strHtml.Replace("@state", ds.Tables[0].Rows[0]["vProposerState"].ToString());

                            strHtml = strHtml.Replace("@mobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@emailID", ds.Tables[0].Rows[0]["vEmailId"].ToString());

                            //strHtml = strHtml.Replace("@productName", ds.Tables[0].Rows[0]["vProductName"].ToString()); //done changes for cert no
                            //strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@productName", "KOTAK GROUP ACCIDENT PROTECT"); //done changes for cert no
                            strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vpolicyType"].ToString() == "" ? "New" : ds.Tables[0].Rows[0]["vpolicyType"].ToString());

                            //manish start
                            //   strHtml = strHtml.Replace("@Enroll", PolicyInceptionDateTime.Substring(24));
                            //manish end
                            strHtml = strHtml.Replace("@prevPolicyNo", ds.Tables[0].Rows[0]["vprevPolicyNumber"].ToString());
                            strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyLoc"].ToString());


                            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderAddress", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString()); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@PolicyholderLine2Address", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerCity"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerState"].ToString() + "(" + custStateCode + ")-" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                            strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderEmailAddress", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                            //  strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@policyStartDate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@policyEndDate", ds.Tables[0].Rows[0]["dPolicyEndDate"].ToString());

                            strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                            strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());


                            strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                            strHtml = strHtml.Replace("@occupation", ds.Tables[0].Rows[0]["vOccupation"].ToString());

                            strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                            strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["dCustomerDob"].ToString());
                            strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                            strHtml = strHtml.Replace("@category", "");
                            strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                            //strHtml = strHtml.Replace("@sumInsured", ds.Tables[0].Rows[0]["nPlanSI"].ToString());
                            //strHtml = strHtml.Replace("@sumInsured", Convert.ToDecimal(ds.Tables[0].Rows[0]["nPlanSI"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@sumInsured", "");


                            strHtml = strHtml.Replace("@sumBasis", ds.Tables[0].Rows[0]["vSIBasis"].ToString());

                            strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                            strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());

                            strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                            //strHtml = strHtml.Replace("@nomineeRelDOB", "");
                            strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vAppointeeName"].ToString());

                            string igstPercentage = ds.Tables[0].Rows[0]["igstPercentage"].ToString();
                            string cgstPercentage = ds.Tables[0].Rows[0]["cgstPercentage"].ToString();
                            string sgstPercentage = ds.Tables[0].Rows[0]["sgstPercentage"].ToString();
                            string ugstPercentage = ds.Tables[0].Rows[0]["ugstPercentage"].ToString();
                            string igstAmount = ds.Tables[0].Rows[0]["igstAmount"].ToString();
                            string cgstAmount = ds.Tables[0].Rows[0]["cgstAmount"].ToString();
                            string sgstAmount = ds.Tables[0].Rows[0]["sgstAmount"].ToString();
                            string ugstAmount = ds.Tables[0].Rows[0]["ugstAmount"].ToString();


                            if (igstPercentage != "0")
                            {
                                igstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center;font-size:x-large'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center;font-size:x-large'>IGST@" + igstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center;font-size:x-large'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center;font-size:x-large'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center;font-size:x-large'>" + igstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center;font-size:x-large'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && sgstPercentage != "0")
                            {
                                cgstsgstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center;font-size:x-large'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center;font-size:x-large'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center;font-size:x-large'>SGST@" + sgstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center;font-size:x-large'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center;font-size:x-large'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center;font-size:x-large'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center;font-size:x-large'>" + sgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center;font-size:x-large'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            if (cgstPercentage != "0" && ugstPercentage != "0")
                            {
                                cgstugstString = "<table width='98%' cellpadding='5' style='border:1px solid black;border-collapse: collapse;'><tbody><tr><td style='border:1px solid black' width='20%'><p style='text-align:center;font-size:x-large'>Taxable Value Of Services (Rs.)</p></td><td style='border:1px solid black' width='20%' colspan='4'><p style='text-align:center;font-size:x-large'>CGST@" + cgstPercentage + "%</p></td><td style='border:1px solid black' width='20%'><p style='text-align:center;font-size:x-large'>UGST@" + ugstPercentage + "%</p></td><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center;font-size:x-large'> Total Amount(Rs.) </p></td></tr><tr><td style = 'border:1px solid black' width = '20%' ><p style = 'text-align:center;font-size:x-large'>" + ds.Tables[0].Rows[0]["nNetPremium"].ToString() + "</p></td><td style = 'border:1px solid black' width = '20%' colspan = '4' ><p style = 'text-align:center;font-size:x-large'> " + cgstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center;font-size:x-large'>" + ugstAmount + "</p></td><td style = 'border:1px solid black' width = '20%'><p style = 'text-align:center;font-size:x-large'> " + ds.Tables[0].Rows[0]["nTotalPolicyPremium"] + " </p></td></tr></tbody></table>";
                            }

                            strHtml = strHtml.Replace("@igstString", igstString == "" ? "" : igstString);
                            strHtml = strHtml.Replace("@cgstsgstString", cgstsgstString == "" ? "" : cgstsgstString);
                            strHtml = strHtml.Replace("@cgstugstString", cgstugstString == "" ? "" : cgstugstString);

                            string policyIssuance = ds.Tables[0].Rows[0]["vAdditional_column_1"].ToString();
                            string customString = string.Empty;

                            if (!String.IsNullOrEmpty(policyIssuance))
                            {
                                string[] strArr = policyIssuance.Split(' ');
                                if (String.IsNullOrEmpty(strArr[1]))
                                {
                                    customString = "this " + strArr[2] + " day of " + strArr[0] + " of year " + strArr[3];
                                }
                                else
                                {
                                    customString = "this " + strArr[1] + " day of " + strArr[0] + " of year " + strArr[2];
                                }

                            }

                            strHtml = strHtml.Replace("@polIssueString", customString);

                            if (ds.Tables[0].Rows[0]["vAccidentalDeathAD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString().Trim()))
                                {
                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString() + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vPermTotalDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString().Trim()))
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString() + ")</p></td></tr> ";
                                }
                                else
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString() + "</p></td></tr> ";
                                }
                            }
                            //
                            if (ds.Tables[0].Rows[0]["vPermPartialDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString().Trim()))
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString() + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vTempTotalDisableTTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString().Trim()))
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCarraigeDeadBody"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString().Trim()))
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vFuneralExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString().Trim()))
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalMedicalExp"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString().Trim()))
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vPurchaseofBlood"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPurchaseofBloodSIText"].ToString().Trim()))
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vPurchaseofBloodSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vTransportationofImpMedicine"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTransportationofImpMedicineSIText"].ToString().Trim()))
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vTransportationofImpMedicineSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCompassionateVisit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString().Trim()))
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDisappearanceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString().Trim()))
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString() + "</p></td></tr>";
                                }

                            }

                            if (ds.Tables[0].Rows[0]["vModificationofVehicle"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString().Trim()))
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCostSupportItems"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString().Trim()))
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCommonCarrier"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString().Trim()))
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vChildEduGrant"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString().Trim()))
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vMarraigeExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString().Trim()))
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSportsActivityCover"].ToString() == "Y")
                            {
                                //if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vSportsActivityCoverSIText"].ToString()))
                                //{
                                //    sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>"+ ds.Tables[0].Rows[0]["nSportsActivityCoverSI"].ToString() + "&nbsp;("+ ds.Tables[0].Rows[0]["vSportsActivityCoverSIText"].ToString() + ")</p></td></tr>";
                                //}
                                //  else
                                //  {
                                //sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>"+ ds.Tables[0].Rows[0]["nSportsActivityCoverSI"].ToString() + "</p></td></tr>";
                                sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                                // }
                            }

                            if (ds.Tables[0].Rows[0]["vWidowhoodCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString().Trim()))
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAmbulanceCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString().Trim()))
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString() + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vDailyCashBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString().Trim()))
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalHospitalization"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString().Trim()))
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOPDTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString().Trim()))
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalDentalExpense"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString().Trim()))
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vConvalescenceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString().Trim()))
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBurnBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString().Trim()))
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBrokenBones"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString().Trim()))
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vComa"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vComaSIText"].ToString().Trim()))
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nComaSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vComaSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nComaSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDomesticTravelForTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString().Trim()))
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString().Trim()))
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOnDutyCover"].ToString() == "Y")
                            {
                                //if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOnDutyCoverSIText"].ToString()))
                                //{
                                //    onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOnDutyCoverSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vOnDutyCoverSIText"].ToString() + ")</p></td></tr>";
                                //}
                                //  else
                                //  {
                                //onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOnDutyCoverSI"].ToString() + "</p></td></tr>";
                                onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                                // }
                            }

                            if (ds.Tables[0].Rows[0]["vLegalExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString().Trim()))
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSIBasis"].ToString().ToLower() == "reducing")
                            {
                                reducingCoverString = "<tr><td style='border:1px solid black' width='10%'><p  style='text-align:center;font-size:x-large'>1</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>Reducing Sum Insured Covers</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vProposalType"].ToString().ToLower() == "credit linked")
                            {
                                assignmentString = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center;font-size:x-large'>2</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>Assignment</p></td></tr>";
                            }



                            //         strHtml = strHtml.Replace("@premium", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@premium", Convert.ToDecimal(ds.Tables[0].Rows[0]["nNetPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            //strHtml = strHtml.Replace("@serviceTax", ds.Tables[0].Rows[0]["nServiceTax"].ToString());
                            //   strHtml = strHtml.Replace("@serviceTax", Convert.ToDecimal(ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            //strHtml = strHtml.Replace("@sbc", ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString());
                            //  strHtml = strHtml.Replace("@sbc", Convert.ToDecimal(ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            //strHtml = strHtml.Replace("@kkc", ds.Tables[0].Rows[0]["nKKC"].ToString());
                            //  strHtml = strHtml.Replace("@kkc", Convert.ToDecimal(ds.Tables[0].Rows[0]["nKKC"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@amount", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                            strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryNumber"].ToString());
                            strHtml = strHtml.Replace("@intermediaryLandline", "");
                            strHtml = strHtml.Replace("@challanDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dChallanDate"]).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            //strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["nStampDuty"].ToString());
                            strHtml = strHtml.Replace("@stampduty", Convert.ToDecimal(ds.Tables[0].Rows[0]["nStampDuty"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            //  if (!String.IsNullOrEmpty(accidentalDeath))
                            //   {
                            strHtml = strHtml.Replace("@accidentalDeathString", accidentalDeath == "" ? "" : accidentalDeath);
                            //  }

                            //   if (!String.IsNullOrEmpty(permTotalDisable))
                            //   {
                            strHtml = strHtml.Replace("@permTotalDisableString", permTotalDisable == "" ? "" : permTotalDisable);
                            //     }

                            //   if (!String.IsNullOrEmpty(permPartialDisable))
                            //   {
                            strHtml = strHtml.Replace("@permTotalPartialString", permPartialDisable == "" ? "" : permPartialDisable);
                            //   }

                            //    if (!String.IsNullOrEmpty(tempTotalDisable))
                            //    {
                            strHtml = strHtml.Replace("@tempTotalDisableString", tempTotalDisable == "" ? "" : tempTotalDisable);
                            //    }

                            //   if (!String.IsNullOrEmpty(carraigeBody))
                            //  {
                            strHtml = strHtml.Replace("@carraigeBodyString", carraigeBody == "" ? "" : carraigeBody);
                            // }
                            // if (!String.IsNullOrEmpty(funeralExpense))
                            // {
                            strHtml = strHtml.Replace("@funeralExpenseString", funeralExpense == "" ? "" : funeralExpense);
                            //  }
                            //  if (!String.IsNullOrEmpty(medicalExpense))
                            //  {
                            strHtml = strHtml.Replace("@medicalExpenseString", medicalExpense == "" ? "" : medicalExpense);
                            //   }
                            //   if (!String.IsNullOrEmpty(purchaseBlood))
                            // {
                            strHtml = strHtml.Replace("@purchasebloodString", purchaseBlood == "" ? "" : purchaseBlood);
                            //   }
                            //  if (!String.IsNullOrEmpty(transportation))
                            //  {
                            strHtml = strHtml.Replace("@transportationString", transportation == "" ? "" : transportation);
                            //  }
                            //  if (!String.IsNullOrEmpty(compassionate))
                            //  {
                            strHtml = strHtml.Replace("@compassionateString", compassionate == "" ? "" : compassionate);
                            //  }
                            //  if (!String.IsNullOrEmpty(disappearance))
                            //  {
                            strHtml = strHtml.Replace("@disappearanceString", disappearance == "" ? "" : disappearance);
                            //  }
                            //   if (!String.IsNullOrEmpty(modifyResidence))
                            // {
                            strHtml = strHtml.Replace("@modifyResidenceString", modifyResidence == "" ? "" : modifyResidence);
                            // }
                            //   if (!String.IsNullOrEmpty(costOfSupport))
                            //   {
                            strHtml = strHtml.Replace("@costOfSupportString", costOfSupport == "" ? "" : costOfSupport);
                            //  }
                            //  if (!String.IsNullOrEmpty(commonCarrier))
                            //  {
                            strHtml = strHtml.Replace("@commonCarrierString", commonCarrier == "" ? "" : commonCarrier);
                            //   }
                            //  if (!String.IsNullOrEmpty(childrenGrant))
                            //  {
                            strHtml = strHtml.Replace("@childrenGrantString", childrenGrant == "" ? "" : childrenGrant);
                            //   }
                            //  if (!String.IsNullOrEmpty(marraigeExpense))
                            //  {
                            strHtml = strHtml.Replace("@marraigeExpenseString", marraigeExpense == "" ? "" : marraigeExpense);
                            //  }
                            //  if (!String.IsNullOrEmpty(sportsActivity))
                            //  {
                            strHtml = strHtml.Replace("@sportsActivityString", sportsActivity == "" ? "" : sportsActivity);
                            //  }
                            //  if (!String.IsNullOrEmpty(widowHood))
                            // {
                            strHtml = strHtml.Replace("@widowHoodString", widowHood == "" ? "" : widowHood);
                            //  }
                            //  if (!String.IsNullOrEmpty(ambulanceChargesString))
                            //  {
                            strHtml = strHtml.Replace("@ambulanceChargesString", ambulanceChargesString == "" ? "" : ambulanceChargesString);
                            //  }
                            //  if (!String.IsNullOrEmpty(dailyCashString))
                            //  {
                            strHtml = strHtml.Replace("@dailyCashString", dailyCashString == "" ? "" : dailyCashString);
                            //  }
                            //   if (!String.IsNullOrEmpty(accidentalHospString))
                            // {
                            strHtml = strHtml.Replace("@accidentalHospString", accidentalHospString == "" ? "" : accidentalHospString);
                            // }
                            // if (!String.IsNullOrEmpty(convalString))
                            //  {
                            strHtml = strHtml.Replace("@convalString", convalString == "" ? "" : convalString);
                            //  }
                            // if (!String.IsNullOrEmpty(burnsString))
                            //  {
                            strHtml = strHtml.Replace("@burnsString", burnsString == "" ? "" : burnsString);
                            //  }
                            // if (!String.IsNullOrEmpty(brokenBones))
                            //  {
                            strHtml = strHtml.Replace("@brokenBones", brokenBones == "" ? "" : brokenBones);
                            //  }
                            //  if (!String.IsNullOrEmpty(comaString))
                            //  {
                            strHtml = strHtml.Replace("@comaString", comaString == "" ? "" : comaString);
                            // }
                            // if (!String.IsNullOrEmpty(domesticTravelString))
                            //  {
                            strHtml = strHtml.Replace("@domesticTravelString", domesticTravelString == "" ? "" : domesticTravelString);
                            // }
                            //   if (!String.IsNullOrEmpty(lossofEmployString))
                            //   {
                            strHtml = strHtml.Replace("@lossofEmployString", lossofEmployString == "" ? "" : lossofEmployString);
                            //  }
                            //if (!String.IsNullOrEmpty(onDutyCover))
                            //{
                            strHtml = strHtml.Replace("@onDutyCover", onDutyCover == "" ? "" : onDutyCover);
                            //}
                            // if (!String.IsNullOrEmpty(legalExpenses))
                            // {
                            strHtml = strHtml.Replace("@legalExpenses", legalExpenses == "" ? "" : legalExpenses);
                            // }
                            //    if (!String.IsNullOrEmpty(reducingCoverString))
                            //   {
                            strHtml = strHtml.Replace("@reducingCoverString", reducingCoverString == "" ? "" : reducingCoverString);
                            // }
                            //   if (!String.IsNullOrEmpty(assignmentString))
                            //  {
                            strHtml = strHtml.Replace("@assignmentString", assignmentString == "" ? "" : assignmentString);
                            // }

                            strHtml = strHtml.Replace("@accidentalDentalString", accidentalDentalString == "" ? "" : accidentalDentalString);
                            strHtml = strHtml.Replace("@opdString", opdString == "" ? "" : opdString);
                            strHtml = strHtml.Replace("@KotakGroupAccidentProtectUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]));
                        }
                        else
                        {
                            strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF_CompleteLetter_Revised.html";
                            htmlBody = File.ReadAllText(strPath);
                            sw = new StringWriter();
                            sr = new StringReader(sw.ToString());
                            strHtml = htmlBody;

                            strHtml = strHtml.Replace("@createdDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dCreatedDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@masterPolicy", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@certificateNo", ds.Tables[0].Rows[0]["vCertificateNo"].ToString());
                            strHtml = strHtml.Replace("@customerName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@nomineeDOB", ds.Tables[0].Rows[0]["vNomineeAge"].ToString());
                            strHtml = strHtml.Replace("@masterDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["vMasterPolicyDate"]).ToString("dd-MMM-yyyy"));
                            strHtml = strHtml.Replace("@vFinancerName", ds.Tables[0].Rows[0]["vFinancerName"].ToString());


                            strHtml = strHtml.Replace("@addressline1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                            strHtml = strHtml.Replace("@addressline2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());

                            strHtml = strHtml.Replace("@addressline3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                            strHtml = strHtml.Replace("@city", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                            strHtml = strHtml.Replace("@pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());
                            strHtml = strHtml.Replace("@state", ds.Tables[0].Rows[0]["vProposerState"].ToString());

                            strHtml = strHtml.Replace("@mobileNo", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@emailID", ds.Tables[0].Rows[0]["vEmailId"].ToString());

                            //strHtml = strHtml.Replace("@productName", ds.Tables[0].Rows[0]["vProductName"].ToString()); //done changes for cert no
                            //strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vMasterPolicyNo"].ToString());
                            strHtml = strHtml.Replace("@productName", "KOTAK GROUP ACCIDENT PROTECT"); //done changes for cert no
                            strHtml = strHtml.Replace("@policyType", ds.Tables[0].Rows[0]["vpolicyType"].ToString() == "" ? "New" : ds.Tables[0].Rows[0]["vpolicyType"].ToString());

                            //manish start
                            //   strHtml = strHtml.Replace("@Enroll", PolicyInceptionDateTime.Substring(24));
                            //manish end
                            strHtml = strHtml.Replace("@prevPolicyNo", ds.Tables[0].Rows[0]["vprevPolicyNumber"].ToString());
                            strHtml = strHtml.Replace("@issuedAt", ds.Tables[0].Rows[0]["vMasterPolicyLoc"].ToString());


                            strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderAddress", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString()); //Convert.ToDateTime(DateOfBirth).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@PolicyholderLine2Address", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerCity"].ToString() + "," + ds.Tables[0].Rows[0]["vProposerState"].ToString() + "-" + ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                            strHtml = strHtml.Replace("@PolicyholderTelephoneNumber", ds.Tables[0].Rows[0]["vMobileNo"].ToString());
                            strHtml = strHtml.Replace("@PolicyholderEmailAddress", ds.Tables[0].Rows[0]["vEmailId"].ToString());
                            //  strHtml = strHtml.Replace("@SumInsured", Convert.ToDecimal(SumInsured).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@policyStartDate", ds.Tables[0].Rows[0]["dPolicyStartDate"].ToString());
                            strHtml = strHtml.Replace("@policyEndDate", ds.Tables[0].Rows[0]["dPolicyEndDate"].ToString());

                            strHtml = strHtml.Replace("@memberID", ds.Tables[0].Rows[0]["vAccountNo"].ToString());
                            strHtml = strHtml.Replace("@creditTenure", ds.Tables[0].Rows[0]["vLoanTenure"].ToString());


                            strHtml = strHtml.Replace("@customerType", ds.Tables[0].Rows[0]["vCustomerType"].ToString());
                            strHtml = strHtml.Replace("@occupation", ds.Tables[0].Rows[0]["vOccupation"].ToString());

                            strHtml = strHtml.Replace("@relationInsured", ds.Tables[0].Rows[0]["vRelationWithInsured"].ToString());
                            strHtml = strHtml.Replace("@dob", ds.Tables[0].Rows[0]["dCustomerDob"].ToString());
                            strHtml = strHtml.Replace("@gender", ds.Tables[0].Rows[0]["vCustomerGender"].ToString());
                            strHtml = strHtml.Replace("@category", "");
                            strHtml = strHtml.Replace("@creditAmount", ds.Tables[0].Rows[0]["vLoanOutAmount"].ToString());
                            //strHtml = strHtml.Replace("@sumInsured", ds.Tables[0].Rows[0]["nPlanSI"].ToString());
                            //strHtml = strHtml.Replace("@sumInsured", Convert.ToDecimal(ds.Tables[0].Rows[0]["nPlanSI"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@sumInsured", "");


                            strHtml = strHtml.Replace("@sumBasis", ds.Tables[0].Rows[0]["vSIBasis"].ToString());

                            strHtml = strHtml.Replace("@comments", ds.Tables[0].Rows[0]["vComments"].ToString());
                            strHtml = strHtml.Replace("@nomineeName", ds.Tables[0].Rows[0]["vNomineeName"].ToString());

                            strHtml = strHtml.Replace("@nomineeRelation", ds.Tables[0].Rows[0]["vNomineeRelation"].ToString());
                            //strHtml = strHtml.Replace("@nomineeRelDOB", "");
                            strHtml = strHtml.Replace("@appointee", ds.Tables[0].Rows[0]["vAppointeeName"].ToString());


                            if (ds.Tables[0].Rows[0]["vAccidentalDeathAD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString().Trim()))
                                {
                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDeathADSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDeath = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Death (AD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDeathADSI"].ToString() + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vPermTotalDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString().Trim()))
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermTotalDisablePTDSIText"].ToString() + ")</p></td></tr> ";
                                }
                                else
                                {
                                    permTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Total Disablement (PTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPermTotalDisablePTDSI"].ToString() + "</p></td></tr> ";
                                }
                            }
                            //
                            if (ds.Tables[0].Rows[0]["vPermPartialDisablePTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString().Trim()))
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vPermPartialDisablePTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    permPartialDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Permanent Partial Disablement  (PPD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPermPartialDisablePTDSI"].ToString() + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vTempTotalDisableTTD"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString().Trim()))
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vTempTotalDisableTTDSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    tempTotalDisable = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Temporary Total Disablement (TTD)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nTempTotalDisableTTDSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCarraigeDeadBody"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString().Trim()))
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vCarraigeDeadBodySIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    carraigeBody = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Carraige of Dead Body</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCarraigeDeadBodySI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vFuneralExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString().Trim()))
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vFuneralExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    funeralExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Funeral Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nFuneralExpensesSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalMedicalExp"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString().Trim()))
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalMedicalExpSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    medicalExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Medical Expenses Extension</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalMedicalExpSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vPurchaseofBlood"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vPurchaseofBloodSIText"].ToString().Trim()))
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vPurchaseofBloodSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    purchaseBlood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Purchase of Blood</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nPurchaseofBloodSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vTransportationofImpMedicine"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vTransportationofImpMedicineSIText"].ToString().Trim()))
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vTransportationofImpMedicineSItext"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    transportation = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Transportation of imported medicine</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nTransportationofImpMedicineSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCompassionateVisit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString().Trim()))
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vCompassionateVisitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    compassionate = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Compassionate Visit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCompassionateVisitSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDisappearanceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString().Trim()))
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vDisappearanceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    disappearance = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Disappearance Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDisappearanceBenefitSI"].ToString() + "</p></td></tr>";
                                }

                            }

                            if (ds.Tables[0].Rows[0]["vModificationofVehicle"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString().Trim()))
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vModificationofVehicleSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    modifyResidence = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Modification of Residence / Vehicle</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nModificationofVehicleSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCostSupportItems"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString().Trim()))
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vCostSupportItemsSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    costOfSupport = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Cost of Support Items</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCostSupportItemsSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vCommonCarrier"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString().Trim()))
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vCommonCarrierSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    commonCarrier = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Common Carrier</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nCommonCarrierSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vChildEduGrant"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString().Trim()))
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vChildEduGrantSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    childrenGrant = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Children Education Grant</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nChildEduGrantSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vMarraigeExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString().Trim()))
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vMarraigeExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    marraigeExpense = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Marriage expenses for Children</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nMarraigeExpensesSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSportsActivityCover"].ToString() == "Y")
                            {
                                //if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vSportsActivityCoverSIText"].ToString()))
                                //{
                                //    sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>"+ ds.Tables[0].Rows[0]["nSportsActivityCoverSI"].ToString() + "&nbsp;("+ ds.Tables[0].Rows[0]["vSportsActivityCoverSIText"].ToString() + ")</p></td></tr>";
                                //}
                                //  else
                                //  {
                                //sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>"+ ds.Tables[0].Rows[0]["nSportsActivityCoverSI"].ToString() + "</p></td></tr>";
                                sportsActivity = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Sports Activity Cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                                // }
                            }

                            if (ds.Tables[0].Rows[0]["vWidowhoodCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString().Trim()))
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vWidowhoodCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    widowHood = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>14</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Widowhood cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nWidowhoodCoverSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAmbulanceCover"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString().Trim()))
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAmbulanceCoverSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    ambulanceChargesString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>1</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Ambulance Charges</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAmbulanceCoverSI"].ToString() + "</p></td></tr>";
                                }
                            }
                            if (ds.Tables[0].Rows[0]["vDailyCashBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString().Trim()))
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vDailyCashBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    dailyCashString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>2</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospital Daily Cash Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDailyCashBenefitSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalHospitalization"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString().Trim()))
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalHospitalizationSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalHospString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>3</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Hospitilization (inpatient)</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalHospitalizationSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOPDTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString().Trim()))
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vOPDTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    opdString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>4</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>OPD Treatment</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOPDTreatmentSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vAccidentalDentalExpense"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString().Trim()))
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vAccidentalDentalExpenseSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    accidentalDentalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>5</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Accidental Dental Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nAccidentalDentalExpenseSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vConvalescenceBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString().Trim()))
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vConvalescenceBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    convalString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>6</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Convalescence Benefit</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nConvalescenceBenefitSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBurnBenefit"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString().Trim()))
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vBurnBenefitSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    burnsString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>7</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Burns</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nBurnBenefitSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vBrokenBones"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString().Trim()))
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vBrokenBonesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    brokenBones = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>8</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Broken Bones</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nBrokenBonesSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vComa"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vComaSIText"].ToString().Trim()))
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nComaSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vComaSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    comaString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>9</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Coma</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nComaSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vDomesticTravelForTreatment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString().Trim()))
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vDomesticTravelForTreatmentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    domesticTravelString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>10</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Domestic travel for medical treatment due to accident</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nDomesticTravelForTreatmentSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vLossOfEmployment"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString().Trim()))
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vLossOfEmploymentSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    lossofEmployString = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>11</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Loss of Employment due to accident*</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nLossOfEmploymentSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vOnDutyCover"].ToString() == "Y")
                            {
                                //if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vOnDutyCoverSIText"].ToString()))
                                //{
                                //    onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOnDutyCoverSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vOnDutyCoverSIText"].ToString() + ")</p></td></tr>";
                                //}
                                //  else
                                //  {
                                //onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'></p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nOnDutyCoverSI"].ToString() + "</p></td></tr>";
                                onDutyCover = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>12</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>On Duty cover</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>Yes</p></td></tr>";
                                // }
                            }

                            if (ds.Tables[0].Rows[0]["vLegalExpenses"].ToString() == "Y")
                            {
                                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString().Trim()))
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString() + "&nbsp;(" + ds.Tables[0].Rows[0]["vLegalExpensesSIText"].ToString() + ")</p></td></tr>";
                                }
                                else
                                {
                                    legalExpenses = " <tr><td style='border:1px solid black' width='10%'><p style='text-align:center'>13</p></td><td style='border:1px solid black' width='40%'><p style='text-align:left'>Legal Expenses</p></td><td style='border:1px solid black' colspan='3' width='50%'><p style='text-align:right'>" + ds.Tables[0].Rows[0]["nLegalExpensesSI"].ToString() + "</p></td></tr>";
                                }
                            }

                            if (ds.Tables[0].Rows[0]["vSIBasis"].ToString().ToLower() == "reducing")
                            {
                                reducingCoverString = "<tr><td style='border:1px solid black' width='10%'><p  style='text-align:center;font-size:x-large'>1</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>Reducing Sum Insured Covers</p></td></tr>";
                            }

                            if (ds.Tables[0].Rows[0]["vProposalType"].ToString().ToLower() == "credit linked")
                            {
                                assignmentString = "<tr><td style='border:1px solid black' width='10%'><p style='text-align:center;font-size:x-large'>2</p></td><td style='border:1px solid black' width='40%' colspan='2'><p style='text-align:left'>Assignment</p></td></tr>";
                            }



                            //         strHtml = strHtml.Replace("@premium", ds.Tables[0].Rows[0]["nNetPremium"].ToString());
                            strHtml = strHtml.Replace("@premium", Convert.ToDecimal(ds.Tables[0].Rows[0]["nNetPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            //strHtml = strHtml.Replace("@serviceTax", ds.Tables[0].Rows[0]["nServiceTax"].ToString());
                            strHtml = strHtml.Replace("@serviceTax", Convert.ToDecimal(ds.Tables[0].Rows[0]["nServiceTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            //strHtml = strHtml.Replace("@sbc", ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString());
                            strHtml = strHtml.Replace("@sbc", Convert.ToDecimal(ds.Tables[0].Rows[0]["nSwachBharatTax"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            //strHtml = strHtml.Replace("@kkc", ds.Tables[0].Rows[0]["nKKC"].ToString());
                            strHtml = strHtml.Replace("@kkc", Convert.ToDecimal(ds.Tables[0].Rows[0]["nKKC"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());
                            strHtml = strHtml.Replace("@amount", ds.Tables[0].Rows[0]["nTotalPolicyPremium"].ToString());
                            strHtml = strHtml.Replace("@intermediaryCd", ds.Tables[0].Rows[0]["vIntermediaryCode"].ToString());
                            strHtml = strHtml.Replace("@intermediaryName", ds.Tables[0].Rows[0]["vIntermediaryName"].ToString());
                            strHtml = strHtml.Replace("@intermediaryMobile", ds.Tables[0].Rows[0]["vIntermediaryNumber"].ToString());
                            strHtml = strHtml.Replace("@intermediaryLandline", "");
                            strHtml = strHtml.Replace("@challanDate", Convert.ToDateTime(ds.Tables[0].Rows[0]["dChallanDate"]).ToString("dd-MMM-yyyy"));

                            strHtml = strHtml.Replace("@challanNumber", ds.Tables[0].Rows[0]["vChallanNumber"].ToString());
                            //strHtml = strHtml.Replace("@stampduty", ds.Tables[0].Rows[0]["nStampDuty"].ToString());
                            strHtml = strHtml.Replace("@stampduty", Convert.ToDecimal(ds.Tables[0].Rows[0]["nStampDuty"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol());

                            //  if (!String.IsNullOrEmpty(accidentalDeath))
                            //   {
                            strHtml = strHtml.Replace("@accidentalDeathString", accidentalDeath == "" ? "" : accidentalDeath);
                            //  }

                            //   if (!String.IsNullOrEmpty(permTotalDisable))
                            //   {
                            strHtml = strHtml.Replace("@permTotalDisableString", permTotalDisable == "" ? "" : permTotalDisable);
                            //     }

                            //   if (!String.IsNullOrEmpty(permPartialDisable))
                            //   {
                            strHtml = strHtml.Replace("@permTotalPartialString", permPartialDisable == "" ? "" : permPartialDisable);
                            //   }

                            //    if (!String.IsNullOrEmpty(tempTotalDisable))
                            //    {
                            strHtml = strHtml.Replace("@tempTotalDisableString", tempTotalDisable == "" ? "" : tempTotalDisable);
                            //    }

                            //   if (!String.IsNullOrEmpty(carraigeBody))
                            //  {
                            strHtml = strHtml.Replace("@carraigeBodyString", carraigeBody == "" ? "" : carraigeBody);
                            // }
                            // if (!String.IsNullOrEmpty(funeralExpense))
                            // {
                            strHtml = strHtml.Replace("@funeralExpenseString", funeralExpense == "" ? "" : funeralExpense);
                            //  }
                            //  if (!String.IsNullOrEmpty(medicalExpense))
                            //  {
                            strHtml = strHtml.Replace("@medicalExpenseString", medicalExpense == "" ? "" : medicalExpense);
                            //   }
                            //   if (!String.IsNullOrEmpty(purchaseBlood))
                            // {
                            strHtml = strHtml.Replace("@purchasebloodString", purchaseBlood == "" ? "" : purchaseBlood);
                            //   }
                            //  if (!String.IsNullOrEmpty(transportation))
                            //  {
                            strHtml = strHtml.Replace("@transportationString", transportation == "" ? "" : transportation);
                            //  }
                            //  if (!String.IsNullOrEmpty(compassionate))
                            //  {
                            strHtml = strHtml.Replace("@compassionateString", compassionate == "" ? "" : compassionate);
                            //  }
                            //  if (!String.IsNullOrEmpty(disappearance))
                            //  {
                            strHtml = strHtml.Replace("@disappearanceString", disappearance == "" ? "" : disappearance);
                            //  }
                            //   if (!String.IsNullOrEmpty(modifyResidence))
                            // {
                            strHtml = strHtml.Replace("@modifyResidenceString", modifyResidence == "" ? "" : modifyResidence);
                            // }
                            //   if (!String.IsNullOrEmpty(costOfSupport))
                            //   {
                            strHtml = strHtml.Replace("@costOfSupportString", costOfSupport == "" ? "" : costOfSupport);
                            //  }
                            //  if (!String.IsNullOrEmpty(commonCarrier))
                            //  {
                            strHtml = strHtml.Replace("@commonCarrierString", commonCarrier == "" ? "" : commonCarrier);
                            //   }
                            //  if (!String.IsNullOrEmpty(childrenGrant))
                            //  {
                            strHtml = strHtml.Replace("@childrenGrantString", childrenGrant == "" ? "" : childrenGrant);
                            //   }
                            //  if (!String.IsNullOrEmpty(marraigeExpense))
                            //  {
                            strHtml = strHtml.Replace("@marraigeExpenseString", marraigeExpense == "" ? "" : marraigeExpense);
                            //  }
                            //  if (!String.IsNullOrEmpty(sportsActivity))
                            //  {
                            strHtml = strHtml.Replace("@sportsActivityString", sportsActivity == "" ? "" : sportsActivity);
                            //  }
                            //  if (!String.IsNullOrEmpty(widowHood))
                            // {
                            strHtml = strHtml.Replace("@widowHoodString", widowHood == "" ? "" : widowHood);
                            //  }
                            //  if (!String.IsNullOrEmpty(ambulanceChargesString))
                            //  {
                            strHtml = strHtml.Replace("@ambulanceChargesString", ambulanceChargesString == "" ? "" : ambulanceChargesString);
                            //  }
                            //  if (!String.IsNullOrEmpty(dailyCashString))
                            //  {
                            strHtml = strHtml.Replace("@dailyCashString", dailyCashString == "" ? "" : dailyCashString);
                            //  }
                            //   if (!String.IsNullOrEmpty(accidentalHospString))
                            // {
                            strHtml = strHtml.Replace("@accidentalHospString", accidentalHospString == "" ? "" : accidentalHospString);
                            // }
                            // if (!String.IsNullOrEmpty(convalString))
                            //  {
                            strHtml = strHtml.Replace("@convalString", convalString == "" ? "" : convalString);
                            //  }
                            // if (!String.IsNullOrEmpty(burnsString))
                            //  {
                            strHtml = strHtml.Replace("@burnsString", burnsString == "" ? "" : burnsString);
                            //  }
                            // if (!String.IsNullOrEmpty(brokenBones))
                            //  {
                            strHtml = strHtml.Replace("@brokenBones", brokenBones == "" ? "" : brokenBones);
                            //  }
                            //  if (!String.IsNullOrEmpty(comaString))
                            //  {
                            strHtml = strHtml.Replace("@comaString", comaString == "" ? "" : comaString);
                            // }
                            // if (!String.IsNullOrEmpty(domesticTravelString))
                            //  {
                            strHtml = strHtml.Replace("@domesticTravelString", domesticTravelString == "" ? "" : domesticTravelString);
                            // }
                            //   if (!String.IsNullOrEmpty(lossofEmployString))
                            //   {
                            strHtml = strHtml.Replace("@lossofEmployString", lossofEmployString == "" ? "" : lossofEmployString);
                            //  }
                            //if (!String.IsNullOrEmpty(onDutyCover))
                            //{
                            strHtml = strHtml.Replace("@onDutyCover", onDutyCover == "" ? "" : onDutyCover);
                            //}
                            // if (!String.IsNullOrEmpty(legalExpenses))
                            // {
                            strHtml = strHtml.Replace("@legalExpenses", legalExpenses == "" ? "" : legalExpenses);
                            // }
                            //    if (!String.IsNullOrEmpty(reducingCoverString))
                            //   {
                            strHtml = strHtml.Replace("@reducingCoverString", reducingCoverString == "" ? "" : reducingCoverString);
                            // }
                            //   if (!String.IsNullOrEmpty(assignmentString))
                            //  {
                            strHtml = strHtml.Replace("@assignmentString", assignmentString == "" ? "" : assignmentString);
                            // }

                            strHtml = strHtml.Replace("@accidentalDentalString", accidentalDentalString == "" ? "" : accidentalDentalString);
                            strHtml = strHtml.Replace("@opdString", opdString == "" ? "" : opdString);
                        }

                        #region HDCRISKFORPROTECT
                        string _Date1 = ds.Tables[0].Rows[0]["dAccountDebitDate"].ToString();
                        //DateTime dtDateHDCRisk = Convert.ToDateTime(_Date1);

                        //string TransactionDateHDCRisk = dtDateHDCRisk.ToString("dd/MM/yyyy");
                        strHtml = strHtml.Replace("@TransactionDateHDCRisk", _Date1);

                        string mentionedGender = ds.Tables[0].Rows[0]["vCustomerGender"].ToString();
                        if (string.IsNullOrEmpty(mentionedGender))
                        {
                            strHtml = strHtml.Replace("@salutation", "");
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "M" || ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "Male")
                            {
                                strHtml = strHtml.Replace("@salutation", "Mr.");
                            }
                            else if (ds.Tables[0].Rows[0]["vCustomerGender"].ToString().Trim() == "F")
                            {
                                strHtml = strHtml.Replace("@salutation", "Mrs.");
                            }
                            else
                            {
                                strHtml = strHtml.Replace("@salutation", "");
                            }
                        }
                        strHtml = strHtml.Replace("@insuredName", ds.Tables[0].Rows[0]["vCustomerName"].ToString());
                        strHtml = strHtml.Replace("@addressofinsured1", ds.Tables[0].Rows[0]["vProposerAddLine1"].ToString());
                        strHtml = strHtml.Replace("@addressofinsured2", ds.Tables[0].Rows[0]["vProposerAddLine2"].ToString());
                        strHtml = strHtml.Replace("@addressofinsured3", ds.Tables[0].Rows[0]["vProposerAddLine3"].ToString());
                        strHtml = strHtml.Replace("@addressofinsuredCity", ds.Tables[0].Rows[0]["vProposerCity"].ToString());
                        strHtml = strHtml.Replace("@addressofinsuredState", ds.Tables[0].Rows[0]["vProposerState"].ToString());
                        strHtml = strHtml.Replace("@Pincode", ds.Tables[0].Rows[0]["vProposerPinCode"].ToString());

                        strHtml = strHtml.Replace("@mobileno", ds.Tables[0].Rows[0]["vMobileNo"].ToString());

                        strHtml = strHtml.Replace("@productname", ds.Tables[0].Rows[0]["vProductName"].ToString());
                        strHtml = strHtml.Replace("@KotakGroupAccidentProtectUIN", Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]) == "" ? "" : Convert.ToString(ds.Tables[0].Rows[0]["KotakGroupAccidentProtectUIN"]));
                        #endregion

                        // Get the current page HTML string by rendering into a TextWriter object
                        TextWriter outTextWriter = new StringWriter();
                        HtmlTextWriter outHtmlTextWriter = new HtmlTextWriter(outTextWriter);
                        base.Render(outHtmlTextWriter);

                        // Obtain the current page HTML string
                        string currentPageHtmlString = strHtml; //outTextWriter.ToString();

                        // Create a HTML to PDF converter object with default settings
                        HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                        // Set license key received after purchase to use the converter in licensed mode
                        // Leave it not set to use the converter in demo mode
                        string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();

                        htmlToPdfConverter.LicenseKey = winnovative_key; // "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

                        // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                        // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                        htmlToPdfConverter.ConversionDelay = 2;

                        // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                        htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                        // Add Header

                        // Enable header in the generated PDF document
                        htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                        // Optionally add a space between header and the page body
                        // The spacing for first page and the subsequent pages can be set independently
                        // Leave this option not set for no spacing
                        htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                        htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");

                        // Draw header elements
                        if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                            DrawHeader(htmlToPdfConverter, false);

                        // Add Footer

                        // Enable footer in the generated PDF document
                        htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                        // Optionally add a space between footer and the page body
                        // Leave this option not set for no spacing
                        htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");

                        // Draw footer elements
                        if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                            DrawFooter(htmlToPdfConverter, false, true);

                        // Use the current page URL as base URL
                        string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;

                        // Convert the current page HTML string to a PDF document in a memory buffer
                       // For Live
                        byte[] _outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                        byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                       // For Live End Here


                        ////// For Dev
                        //byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(currentPageHtmlString, baseUrl);
                        ////// byte[] outPdfBuffer = clsDigitalCertificate.Sign(_outPdfBuffer);
                        ////// For Dev End here 

                        // Send the PDF as response to browser

                        // Set response content type
                        Response.AddHeader("Content-Type", "application/pdf");

                        // Instruct the browser to open the PDF file as an attachment or inline
                        //Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), txtCertificateNumber.Text.Trim()));

                        Response.AddHeader("Content-Disposition", String.Format("attachment; filename=GPAPolicySchedule_{1}.pdf; size={0}", outPdfBuffer.Length.ToString(), txtCertificateNumber.Text.Trim()));

                        // Write the PDF document buffer to HTTP response
                        //Response.BinaryWrite(outPdfBuffer);
                        Response.BinaryWrite(outPdfBuffer);

                        // End the HTTP response and stop the current page processing
                        CommonExtensions.fn_AddLogForDownload(txtPolicyId.Text, "FrmGPAGetPolicy.aspx");//Added By Rajesh Soni on 19/02/2020
                        Response.End();

                       

                    }
                }

            }

        }

        private void DigitalSign()
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = ConfigurationManager.AppSettings["winnovative_key"].ToString(); // "4W9+bn19bn5ue2B+bn1/YH98YHd3d3c=";

            // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
            // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
            htmlToPdfConverter.ConversionDelay = 2;

            Winnovative.Document pdfDocument = null;
            try
            {
                // string baseUrl = 
                string baseUrl = @"D:\ProjectPass\PrjPASS\digitalSignTest.html";  //baseUrlTextBox.Text;
                string htmlWithDigitalSignatureMarker = ""; // htmlStringTextBox.Text;

                string currentPageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                string rootUrl = currentPageUrl.Substring(0, currentPageUrl.LastIndexOf('/')) + "/../../";
                htmlWithDigitalSignatureMarker = System.IO.File.ReadAllText(baseUrl);

                // Convert a HTML string with a marker for digital signature to a PDF document object
                pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocumentObject(htmlWithDigitalSignatureMarker, baseUrl);

                // Make the HTML element with 'digital_signature_element' mapping ID a link to digital signature properties
                HtmlElementMapping digitalSignatureMapping = htmlToPdfConverter.HtmlElementsMappingOptions.HtmlElementsMappingResult.GetElementByMappingId("digital_signature_element");
                if (digitalSignatureMapping != null)
                {
                    Winnovative.PdfPage digitalSignaturePage = digitalSignatureMapping.PdfRectangles[0].PdfPage;
                    RectangleF digitalSignatureRectangle = digitalSignatureMapping.PdfRectangles[0].Rectangle;

                    string certificateFilePath = @"D:\Downloads\WnvHtmlToPdf-v12.16\DemoAppFiles\Input\Certificates\wnvpdf.pfx"; //Server.MapPath("~/DemoAppFiles/Input/Certificates/wnvpdf.pfx");

                    // Get the certificate from password protected PFX file
                    DigitalCertificatesCollection certificates = DigitalCertificatesStore.GetCertificates(certificateFilePath, "winnovative");
                    DigitalCertificate certificate = certificates[0];

                    // Create the digital signature
                    DigitalSignatureElement signature = new DigitalSignatureElement(digitalSignatureRectangle, certificate);
                    signature.Reason = "Protect the document from unwanted changes";
                    signature.ContactInfo = "The contact email is support@winnovative-software.com";
                    signature.Location = "Development server";
                    digitalSignaturePage.AddElement(signature);
                }

                // Save the PDF document in a memory buffer
                byte[] outPdfBuffer = pdfDocument.Save();

                // Send the PDF as response to browser

                // Set response content type
                Response.AddHeader("Content-Type", "application/pdf");

                // Instruct the browser to open the PDF file as an attachment or inline
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=Digital_Signatures.pdf; size={0}", outPdfBuffer.Length.ToString()));

                // Write the PDF document buffer to HTTP response
                Response.BinaryWrite(outPdfBuffer);

                // End the HTTP response and stop the current page processing
                Response.End();
            }
            finally
            {
                // Close the PDF document
                if (pdfDocument != null)
                    pdfDocument.Close();
            }
        }

        void htmlToPdfConverter_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
        {
            // Set the header visibility in first, odd and even pages
            if (true)
            {
                if (eventParams.PageNumber == 1)
                    eventParams.Page.ShowHeader = true;
                else if ((eventParams.PageNumber % 2) == 0 && !true)
                    eventParams.Page.ShowHeader = false;
                else if ((eventParams.PageNumber % 2) == 1 && !true)
                    eventParams.Page.ShowHeader = false;
            }

            // Set the footer visibility in first, odd and even pages
            if (true)
            {
                if (eventParams.PageNumber == 1)
                    eventParams.Page.ShowFooter = true;
                else if ((eventParams.PageNumber % 2) == 0 && !true)
                    eventParams.Page.ShowFooter = false;
                else if ((eventParams.PageNumber % 2) == 1 && !true)
                    eventParams.Page.ShowFooter = false;
            }
        }

        private void DrawHeader(HtmlToPdfConverter htmlToPdfConverter, bool drawHeaderLine)
        {
            string headerHtmlUrl = IsWithoutHeaderFooter ? Server.MapPath("~/Header_HTML_WithoutHeader.html") : Server.MapPath("~/Header_HTML.html");

            // Set the header height in points
            htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 60;

            // Set header background color
            System.Drawing.Color colour = IsWithoutHeaderFooter ? ColorTranslator.FromHtml("#ffffff") : ColorTranslator.FromHtml("#ec3237"); ;
            htmlToPdfConverter.PdfHeaderOptions.HeaderBackColor = colour; // System.Drawing.Color.Red;

            // Create a HTML element to be added in header
            HtmlToPdfElement headerHtml = new HtmlToPdfElement(headerHtmlUrl);

            // Set the HTML element to fit the container height
            headerHtml.FitHeight = true;

            // Add HTML element to header
            htmlToPdfConverter.PdfHeaderOptions.AddElement(headerHtml);

            if (drawHeaderLine)
            {
                // Calculate the header width based on PDF page size and margins
                float headerWidth = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width -
                            htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin;

                // Calculate header height
                float headerHeight = htmlToPdfConverter.PdfHeaderOptions.HeaderHeight;

                // Create a line element for the bottom of the header
                LineElement headerLine = new LineElement(0, headerHeight - 1, headerWidth, headerHeight - 1);

                // Set line color
                headerLine.ForeColor = System.Drawing.Color.Gray;

                // Add line element to the bottom of the header
                htmlToPdfConverter.PdfHeaderOptions.AddElement(headerLine);
            }
        }

        private void DrawFooter(HtmlToPdfConverter htmlToPdfConverter, bool addPageNumbers, bool drawFooterLine)
        {
            string footerHtmlUrl = IsWithoutHeaderFooter ? Server.MapPath("~/Header_HTML_WithoutFooter.html") : Server.MapPath("~/Footer_HTML.html");
            // Set the footer height in points
            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 60;

            // Set footer background color
            htmlToPdfConverter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.White;

            // Create a HTML element to be added in footer
            HtmlToPdfElement footerHtml = new HtmlToPdfElement(footerHtmlUrl);

            // Set the HTML element to fit the container height
            footerHtml.FitHeight = true;

            // Add HTML element to footer
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerHtml);

            // Add page numbering
            if (addPageNumbers)
            {
                // Create a text element with page numbering place holders &p; and & P;
                TextElement footerText = new TextElement(0, 30, "Page &p; of &P;  ",
                    new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));

                // Align the text at the right of the footer
                footerText.TextAlign = HorizontalTextAlign.Right;

                // Set page numbering text color
                footerText.ForeColor = System.Drawing.Color.Navy;

                // Embed the text element font in PDF
                footerText.EmbedSysFont = true;

                // Add the text element to footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerText);
            }

            drawFooterLine = IsWithoutHeaderFooter ? false : true;
            if (drawFooterLine)
            {
                // Calculate the footer width based on PDF page size and margins
                float footerWidth = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width -
                            htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin;

                // Create a line element for the top of the footer
                LineElement footerLine = new LineElement(0, 0, footerWidth, 0);

                // Set line color
                footerLine.ForeColor = System.Drawing.Color.Gray;

                // Add line element to the bottom of the footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerLine);
            }
        }

        protected void btnGPACertificate_WithoutHeaderFooter_Click(object sender, EventArgs e)
        {
            convertToPdf = true;
            IsWithoutHeaderFooter = true;
        }

        //Commented on 14-Dec-2015
        //public class MyImageProvider : AbstractImageProvider
        //{
        //    public override string GetImageRootPath()
        //    {
        //        return HttpContext.Current.Server.MapPath("~/Contents/Images");
        //    }
        //} 
    }
}