using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


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

namespace PrjPASS
{
    public partial class digitalsign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //DigitalSign();
            GetXYPositionInHTMLPageForSpecificHTMLElement();
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

        protected void GetXYPositionInHTMLPageForSpecificHTMLElement()
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = ConfigurationManager.AppSettings["winnovative_key"].ToString(); // "4W9+bn19bn5ue2B+bn1/YH98YHd3d3c=";

            // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
            // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
            htmlToPdfConverter.ConversionDelay = 2;

            // Select the HTML elements for which to retrieve location and other information from HTML document
            //htmlToPdfConverter.HtmlElementsMappingOptions.HtmlElementSelectors = new string[] { "digital_signature_element" };
            htmlToPdfConverter.HtmlElementsMappingOptions.HtmlElementSelectors = new string[] { "digital_signature_element" };
            
            Document pdfDocument = null;
            try
            {
                // Convert HTML page to a PDF document object which can be further modified to highlight the selected elements
                //pdfDocument = htmlToPdfConverter.ConvertUrlToPdfDocumentObject(@"D:\Downloads\GPAPolicySchedule.pdf");
                pdfDocument = htmlToPdfConverter.ConvertUrlToPdfDocumentObject(@"D:\ProjectPass\PrjPASS\GPA_PDF.html"); //this path will the html page on which data-mapping is enabled see below code:
                /*
                put this code in your html file
                  <div data-mapping-enabled="true" data-mapping-id="digital_signature_element" style="width: 320px; padding: 5px 5px 5px 5px; border: 2px solid royalblue">
                    <span style="font-size: 16px; font-weight: bold; text-decoration: underline; color: navy">Click to open the digital signature properties</span><br />
                    <br />
                    <img alt="Logo Image" style="width: 300px" src="Images/Logo.jpg" />
                  </div>
                */
                

                // Highlight the selected elements in PDF with colored rectangles
                foreach (HtmlElementMapping htmlElementInfo in htmlToPdfConverter.HtmlElementsMappingOptions.HtmlElementsMappingResult)
                {
                    // Get other information about HTML element
                    string htmlElementTagName = htmlElementInfo.HtmlElementTagName;
                    string htmlElementID = htmlElementInfo.HtmlElementId;

                    // Hightlight the HTML element in PDF

                    // A HTML element can span over many PDF pages and therefore the mapping of the HTML element in PDF document consists 
                    // in a list of rectangles, one rectangle for each PDF page where this element was rendered
                    foreach (HtmlElementPdfRectangle htmlElementLocationInPdf in htmlElementInfo.PdfRectangles)
                    {
                        // Get the HTML element location in PDF page
                        PdfPage htmlElementPdfPage = htmlElementLocationInPdf.PdfPage;
                        RectangleF htmlElementRectangleInPdfPage = htmlElementLocationInPdf.Rectangle;

                        // Highlight the HTML element element with a colored rectangle in PDF
                        RectangleElement highlightRectangle = new RectangleElement(htmlElementRectangleInPdfPage.X, htmlElementRectangleInPdfPage.Y,
                            htmlElementRectangleInPdfPage.Width, htmlElementRectangleInPdfPage.Height);

                        if (htmlElementTagName.ToLower() == "h1")
                            highlightRectangle.ForeColor = Color.Blue;
                        else if (htmlElementTagName.ToLower() == "h2")
                            highlightRectangle.ForeColor = Color.Green;
                        else if (htmlElementTagName.ToLower() == "h3")
                            highlightRectangle.ForeColor = Color.Red;
                        else if (htmlElementTagName.ToLower() == "h4")
                            highlightRectangle.ForeColor = Color.Yellow;
                        else if (htmlElementTagName.ToLower() == "h5")
                            highlightRectangle.ForeColor = Color.Indigo;
                        else if (htmlElementTagName.ToLower() == "h6")
                            highlightRectangle.ForeColor = Color.Orange;
                        else
                            highlightRectangle.ForeColor = Color.Navy;

                        highlightRectangle.LineStyle.LineDashStyle = LineDashStyle.Solid;

                        htmlElementPdfPage.AddElement(highlightRectangle);
                    }
                }

                // Save the PDF document in a memory buffer
                byte[] outPdfBuffer = pdfDocument.Save();

                // Send the PDF as response to browser

                // Set response content type
                Response.AddHeader("Content-Type", "application/pdf");

                // Instruct the browser to open the PDF file as an attachment or inline
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=Select_in_API_HTML_Elements_to_Retrieve.pdf; size={0}", outPdfBuffer.Length.ToString()));

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


    }
}