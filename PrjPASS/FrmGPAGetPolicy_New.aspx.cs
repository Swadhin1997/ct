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
using iTextSharp.text.html.simpleparser;

using System.Text;
using System.Xml;
using System.Configuration;
using System.Collections;

using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Obout.Grid;
using ProjectPASS;
using System.Xml.Serialization;
using System.Globalization;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text.RegularExpressions;

using System.Web.Services;
using System.Web.Configuration;


namespace ProjectPASS
{
    public partial class FrmGPAGetPolicy_New : System.Web.UI.Page
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
                    Alert.Show("Invalid Session or Session TimeOut", "FrmSecuredLogin.aspx");
                    return;
                }
            }
        }

        protected void btnGetPolicy_Click(object sender, EventArgs e)
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

                    bitMap.Save(Server.MapPath("/Contents/images/" + txtPolicyId.Text.Replace("/", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                    byte[] byteImage = ms.ToArray();

                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);

                }
            }
            string vWebSiteName = ConfigurationManager.AppSettings["vWebSiteName"].ToString();

            //string example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p><img src='http://'" + vWebSiteName + "/Contents/Images/child_plans.png' width='271' height='86' alt='QR Code'/>";
            string example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p><img src='E:\ProjectPASS\ProjectPASS\Contents\Images\" + txtPolicyId.Text.Replace("/", "") + ".png' />";
            var example_css = @".headline{font-size:200%}";

            String contents = example_html;
            System.IO.File.WriteAllText(@"E:\" + txtPolicyId.Text.Replace("/", "") + ".html", contents);

            CreatePDF(example_html, example_css);

            ConvertToPDF(example_html, example_css);

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
                var doc = new Document();


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
                    Document pdfDoc = new Document(PageSize.A4, 0, 0, 30, 65);

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

                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void CreatePDF_New(string strQuoteNo, string strHTML)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + strQuoteNo + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //string fileName = string.Empty;

            //DateTime fileCreationDatetime = DateTime.Now;

            //fileName = string.Format("{0}.pdf", fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + fileCreationDatetime.ToString(@"HHmmss"));

            //string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;

            Document pdfDoc = new Document(PageSize.A4, 13f, 13f, 140f, 0f);

            //using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
            //{
            //step 1

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

            try
            {
                // step 2
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfWriter.PageEvent = new PrjPASS.ITextEvents_ForGPA();

                //open the stream 
                pdfDoc.Open();

                //for (int i = 0; i < 2; i++)
                //{
                //Paragraph para = new Paragraph("Product Type: Private Car", new Font(Font.BOLD));
                //para.Alignment = Element.ALIGN_CENTER;
                //pdfDoc.Add(para);

                StringReader sr = new StringReader(strHTML);
                htmlparser.Parse(sr);

                //pdfDoc.NewPage();

                //}

                pdfDoc.Close();

                pdfWriter.Close();

                Response.Write(pdfDoc);
                Response.End();

            }
            catch (Exception ex)
            {
                string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strXmlPath, "Error: " + ex.Message.ToString());
            }

            finally
            {


            }

            //return pdfDoc;
            //}
        }

        protected void btnGetPolicy_New_Click(object sender, EventArgs e)
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "GPA_PDF.html";
            string htmlBody = File.ReadAllText(strPath);
            var example_css = @".headline{font-size:200%}";
            CreatePDF_New("123456789", htmlBody);

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