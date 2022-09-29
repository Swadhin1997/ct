using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Winnovative;
using System.Drawing;
using Winnovative;

namespace PrjPASS
{
    public class clsConvertHtmlToPdf
    {
        public string winnovative_key = ConfigurationManager.AppSettings["winnovative_key"].ToString();
        public bool convertToPdf = false;
        public bool IsWithoutHeaderFooter = false;

        public string HeaderFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\htmlbodytemplates\Header_HTML.html";
        public string FooterFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\htmlbodytemplates\Footer_HTML.html";

        #region html to pdf private functions
        public byte[] ConvertToPdfNew(string strHtmlString, out string strErrorMsg)
        {
            byte[] outPdfBuffer = null;

            try
            {
                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                htmlToPdfConverter.LicenseKey = winnovative_key;
                htmlToPdfConverter.ConversionDelay = 2;

                htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

                htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                htmlToPdfConverter.PdfDocumentOptions.Y = float.Parse("0");
                htmlToPdfConverter.PdfDocumentOptions.TopSpacing = float.Parse("0");



                if (htmlToPdfConverter.PdfDocumentOptions.ShowHeader)
                    DrawHeader(htmlToPdfConverter, false);

                htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                htmlToPdfConverter.PdfDocumentOptions.BottomSpacing = float.Parse("0");



                if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
                    DrawFooter(htmlToPdfConverter, false, true);




                string htmlString = strHtmlString;
                string baseUrl = "";



                outPdfBuffer = htmlToPdfConverter.ConvertHtml(htmlString, baseUrl);

                strErrorMsg = "";


            }

            catch (Exception ex)
            {
                strErrorMsg = "error";
                ExceptionUtility.LogException(ex, "");
            }

            return outPdfBuffer;
        }

        private void htmlToPdfConverter_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
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
            string headerHtmlUrl = IsWithoutHeaderFooter ? AppDomain.CurrentDomain.BaseDirectory + @"\htmlbodytemplates\Header_HTML_WithoutHeader.html" : HeaderFilePath;

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
            string footerHtmlUrl = IsWithoutHeaderFooter ? AppDomain.CurrentDomain.BaseDirectory + @"\htmlbodytemplates\Header_HTML_WithoutFooter.html" : FooterFilePath;

            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 60;

            // Set footer background color
            //htmlToPdfConverter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.White;
            htmlToPdfConverter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.LightGray;


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

        private static string MaskString(string unmask)
        {
            if (unmask == "NA")
            {
                return unmask;
            }
            else
            {
                StringBuilder sb = new StringBuilder(unmask);
                for (int i = 0; i < 5; i++)
                {
                    sb[i] = 'X';
                }
                unmask = sb.ToString();
                return unmask;
            }
        }

        #endregion
    }
}
