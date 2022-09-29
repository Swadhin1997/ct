using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PrjPASS
{
    public partial class downloadquotepdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string str20 = Encryption.EncryptText("130717000019 0 -7");
            //string str21 = Encryption.EncryptText("180717003804 -15");

            if (Request.QueryString["key"] != null)
            {
                string encryptedQuoteId = Request.QueryString["key"].ToString();
                string decryptedquoteid = Encryption.DecryptText(encryptedQuoteId);
                string strQuoteNo = decryptedquoteid; // "270617000005 0";
                string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();
                string fileName = string.Format("{0}.pdf", strQuoteNo);
                string pdfPath = (KotakQuotesPDFFiles) + fileName;

                //We have given KGI-SVCPASS Full Control on both the servers (12.124 and 12.125) to D:\KotakQuotesPDFFiles folder and it is working well.
                //D:\KotakQuotesPDFFiles is a shared folder between 12.124 and 12.125

                string pdfPath_124 = @"\\KGI-P-APP-BRIDGE-1-V126\KotakQuotesPDFFiles\" + fileName;
                string pdfPath_125 = @"\\KGI-P-APP-BRIDGE-2-V125\KotakQuotesPDFFiles\" + fileName;
                string pdfPath_191 = @"\\KGI-P-APP-PASS-1-V191\KotakQuotesPDFFiles\" + fileName;
                string pdfPath_193 = @"\\KGI-P-APP-PASS-3-V193\KotakQuotesPDFFiles\" + fileName;

                if (File.Exists(pdfPath))
                {
                    lblMsg.Text = "File Downloaded Successfully...";
                    DownloadSavedPDF(strQuoteNo, pdfPath);
                }
                else if (File.Exists(pdfPath_124))
                {
                    lblMsg.Text = "File Downloaded Successfully...";
                    DownloadSavedPDF(strQuoteNo, pdfPath_124);
                }
                else if (File.Exists(pdfPath_125))
                {
                    lblMsg.Text = "File Downloaded Successfully...";
                    DownloadSavedPDF(strQuoteNo, pdfPath_125);
                }
                else if (File.Exists(pdfPath_191))
                {
                    lblMsg.Text = "File Downloaded Successfully...";
                    DownloadSavedPDF(strQuoteNo, pdfPath_191);
                }
                else if (File.Exists(pdfPath_193))
                {
                    lblMsg.Text = "File Downloaded Successfully...";
                    DownloadSavedPDF(strQuoteNo, pdfPath_193);
                }
                else
                {
                    lblMsg.Text = "Quote PDF for Quote Number: " + strQuoteNo + " does not exists...";
                }
            }
            else
            {
                lblMsg.Text = "Invalid Request..";
            }
        }

        private void DownloadSavedPDF(string strQuoteNo, string PDFFilePath)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + strQuoteNo + ".pdf");
            Response.TransmitFile(PDFFilePath);
            Response.Flush();
        }
    }
}