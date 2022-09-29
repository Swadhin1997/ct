using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using iTextSharp.text.html;
using System.Text.RegularExpressions;

namespace PrjPASS
{
    //Thanks to : http://stackoverflow.com/questions/18996323/add-header-and-footer-for-pdf-using-itextsharp

    public class ITextEvents_Bundled : PdfPageEventHelper
    {
        public string ProductName;
        public ITextEvents_Bundled(string strProductName)
        {
            ProductName = strProductName;
        }
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;


        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion

        public override void OnStartPage(PdfWriter writer, iTextSharp.text.Document document)
        {

            /*
            //Header Image
            iTextSharp.text.Image imghead = iTextSharp.text.Image.GetInstance(new Uri("http://www.kotakgeneralinsurance.com/images/logo.png"));
            imghead.SetAbsolutePosition(0, 0);
            PdfContentByte cbhead = writer.DirectContent;
            PdfTemplate tp = cbhead.CreateTemplate(273, 95);
            tp.AddImage(imghead);


            cbhead.AddTemplate(tp, 0, 842 - 95);




            PdfContentByte cb = writer.DirectContent;

            document.SetMargins(35, 35, 100, 82);



            float textBase = document.Bottom - 62;
            //cb.RestoreState();




            //document.NewPage();
            base.OnStartPage(writer, document);
            */

        }
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(80, 80);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {

            }
            catch (System.IO.IOException ioe)
            {

            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);

            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(Font.NORMAL);

            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(Font.NORMAL);

            Phrase p1Header = new Phrase("Kotak General Insurance", baseFontNormal);

            iTextSharp.text.Image imghead = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "/Images/kgipasslogo.png");
            imghead.SetAbsolutePosition(0, 0);
            imghead.ScaleAbsolute(120f, 40f);
            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);

            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            PdfPCell pdfCell1 = new PdfPCell();
            PdfPCell pdfCell2 = new PdfPCell(imghead);
            PdfPCell pdfCell3 = new PdfPCell();
            String text = "Page " + writer.PageNumber + " of ";
            //String FooterText = @"" + "";

            //Add paging to header
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 7);
                cb.SetTextMatrix(document.PageSize.GetRight(150), document.PageSize.GetTop(25));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                //Adds "12" in Page 1 of 12
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(170) + len, document.PageSize.GetTop(25));
            }
            //Add paging to footer
            {
                //cb.BeginText();
                //cb.SetFontAndSize(bf, 7);
                //cb.SetTextMatrix(document.PageSize.GetLeft(10), document.PageSize.GetBottom(30));
                //cb.ShowText(FooterText);
                //cb.EndText();
                //float len = bf.GetWidthPoint(FooterText, 7);
                //cb.AddTemplate(footerTemplate, document.PageSize.GetRight(380) + len, document.PageSize.GetBottom(30));

                /*Paragraph footer = new Paragraph(FooterText, FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL));
                Paragraph footer1 = new Paragraph("Kotak Mahindra General Insurance Company Ltd. CIN: U66000MH2014PLC260291", FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.BOLD));

                footer.Alignment = Element.ALIGN_CENTER;
                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.TotalWidth = 1000;
                footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell2 = new PdfPCell(footer1);
                cell2.Border = 0;
                cell2.Padding = -5;
                //cell.BackgroundColor = Color.GRAY;
                cell2.PaddingLeft = 0;
                footerTbl.AddCell(cell2);

                PdfPCell cell = new PdfPCell(footer);
                cell.Border = 0;
                cell.Padding = -5;
                //cell.BackgroundColor = Color.GRAY;
                cell.PaddingLeft = 0;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -1, 10, 45, writer.DirectContent);*/

                Font fontNormal7size = new Font(Font.TIMES_ROMAN, 7f, Font.NORMAL);
                Font fontBold7size = new Font(Font.TIMES_ROMAN, 7f, Font.BOLD);
                
                Paragraph footer1 = new Paragraph("Kotak Mahindra General Insurance Company Ltd.", FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.BOLD));
                //Paragraph footer2 = new Paragraph("\nCIN: U66000MH2014PLC260291. Registered Office: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai - 400051. Maharashtra, India.", FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL));
                Chunk objChunk1 = new Chunk("\nCIN: U66000MH2014PLC260291 ", fontNormal7size);
                Chunk objChunk2 = new Chunk("Registered Office: ", fontBold7size);
                Chunk objChunk3 = new Chunk("27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai - 400051. Maharashtra, India.", fontNormal7size);
                Phrase Phrase1 = new Phrase();
                Phrase1.Add(objChunk1);
                Phrase1.Add(objChunk2);
                Phrase1.Add(objChunk3);
                Paragraph footer2 = new Paragraph(); footer2.Add(Phrase1);

                Chunk objChunk4 = new Chunk("\n\nOffice: ", fontBold7size);
                Chunk objChunk5 = new Chunk("8th Floor, Kotak Infiniti, Bldg. 21, Infinity IT Park, Off WEH, Gen. AK Vaidya Marg, Dindoshi, Malad(E), Mumbai - 400097.India.", fontNormal7size);
                Phrase Phrase2 = new Phrase();
                Phrase2.Add(objChunk4);
                Phrase2.Add(objChunk5);
                
                Paragraph footer3 = new Paragraph(); footer3.Add(Phrase2);
                //Paragraph footer3 = new Paragraph("\n\nOffice: 8th Floor, Kotak Infiniti, Bldg. 21, Infinity IT Park, Off WEH, Gen. AK Vaidya Marg, Dindoshi, Malad(E), Mumbai - 400097.India.", FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL));
                Paragraph footer4 = new Paragraph("\n\n\nToll Free: 1800 266 4545 Email: care@kotak.com Website: www.kotakgeneralinsurance.com IRDAI Reg. No. 152", FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL));

                footer2.Alignment = Element.ALIGN_CENTER;

                PdfPTable footerTbl1 = new PdfPTable(1);
                footerTbl1.TotalWidth = 1000;
                footerTbl1.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell1 = new PdfPCell(footer1);
                cell1.Border = 0;
                cell1.Padding = 1;
                //cell1.BackgroundColor = Color.LIGHT_GRAY;
                cell1.PaddingLeft = 0;
                footerTbl1.AddCell(cell1);
                footerTbl1.WriteSelectedRows(0, -1, 250, 45, writer.DirectContent);

                PdfPTable footerTbl2 = new PdfPTable(1);
                footerTbl2.TotalWidth = 1000;
                footerTbl2.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell2 = new PdfPCell(footer2);
                cell2.Border = 0;
                cell2.Padding = 1;
                //cell.BackgroundColor = Color.GRAY;
                cell2.PaddingLeft = 0;
                footerTbl2.AddCell(cell2);
                footerTbl2.WriteSelectedRows(0, -1, 80, 45, writer.DirectContent);

                PdfPTable footerTbl3 = new PdfPTable(1);
                footerTbl3.TotalWidth = 1000;
                footerTbl3.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell3 = new PdfPCell(footer3);
                cell3.Border = 0;
                cell3.Padding = 1;
                //cell.BackgroundColor = Color.GRAY;
                cell3.PaddingLeft = 0;
                footerTbl3.AddCell(cell3);
                footerTbl3.WriteSelectedRows(0, -1, 100, 45, writer.DirectContent);

                PdfPTable footerTbl4 = new PdfPTable(1);
                footerTbl4.TotalWidth = 1000;
                footerTbl4.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell4 = new PdfPCell(footer4);
                cell4.Border = 0;
                cell4.Padding = 1;
                //cell.BackgroundColor = Color.GRAY;
                cell3.PaddingLeft = 0;
                footerTbl4.AddCell(cell4);
                footerTbl4.WriteSelectedRows(0, -1, 152, 45, writer.DirectContent);
            }
            //Row 8
            //PdfPCell pdfCell8 = new PdfPCell();
            //Row 9
            //PdfPCell pdfCell9 = new PdfPCell();
            //Row 2
            //PdfPCell pdfCell4 = new PdfPCell();
            //Row 3


            PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + CommonExtensions.GetFormattedDate(PrintTime), baseFontBig));
            //PdfPCell pdfCell6 = new PdfPCell(new Phrase("Car Secure - Bundled", baseFontNormal));
            PdfPCell pdfCell6 = new PdfPCell(new Phrase(ProductName, baseFontNormal));
            PdfPCell pdfCell7 = new PdfPCell(new Phrase("Time:" + string.Format("{0:t}", DateTime.Now), baseFontBig));


            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;

            pdfCell1.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            //pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;


            //pdfCell4.Colspan = 3;
            //pdfCell8.Colspan = 3;
            //pdfCell9.Colspan = 3;


            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            //pdfCell4.Border = 0;
            pdfCell5.Border = 0;
            pdfCell6.Border = 0;
            pdfCell7.Border = 0;
            //pdfCell8.Border = 0;
            //pdfCell9.Border = 0;

            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            //pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(new PdfPCell(pdfCell2));
            pdfTab.AddCell(pdfCell3);
            //pdfTab.AddCell(pdfCell8);
            //pdfTab.AddCell(pdfCell9);

            //pdfTab.AddCell(pdfCell4);
            pdfTab.AddCell(pdfCell5);
            pdfTab.AddCell(pdfCell6);
            pdfTab.AddCell(pdfCell7);

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;


            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(10, document.PageSize.Height - 73);
            cb.LineTo(document.PageSize.Width - 10, document.PageSize.Height - 73);
            cb.Stroke();

            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(10, document.PageSize.Height - 90);
            cb.LineTo(document.PageSize.Width - 10, document.PageSize.Height - 90);
            cb.Stroke();


            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 7);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.SetColorFill(Color.DARK_GRAY);
            footerTemplate.EndText();
        }
    }
}
