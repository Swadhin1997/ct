using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class DMSDocumentSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearchDocumentDMS_Click(object sender, EventArgs e)
        {
            string InwardId = "KP000116112900004"; // KP000116111100005
            string CustomerId = "1000013977";

            ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
            proxy.Endpoint.Behaviors.Add(new CustomBehavior());


            ////ServiceReference1.ImagePath[] imgPath = proxy.SearchDocumentDMS("CMCDocs", "cmc123", "KP000116111100005", "", "1000013427", "", "", "", "");
            
            //ServiceReference1.ImagePath[] imgPath = proxy.SearchDocumentDMS("CMCDocs", "cmc123", InwardId, "", CustomerId, "", "", "", "");
            //ServiceReference1.ImagePathDMS[] imgPathDMS = proxy.ViewDocumentDMS("CMCDocs", "cmc123", InwardId + ".tif", imgPath[0].ImgPath);

            ServiceReference1.ImagePath[] imgPath = proxy.SearchDocumentDMS("CMCDocs", "cmc123", "", "", "", "", "", "10110000436", "");
            ServiceReference1.ImagePathDMS[] imgPathDMS = proxy.ViewDocumentDMS("CMCDocs", "cmc123", "10110000436.ZIP", imgPath[0].ImgPath);

            
            proxy.Close();

            byte[] bytesInStream = imgPathDMS[0].ResponseMemoryStream.ToArray(); // simpler way of converting to array
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment;filename=" + InwardId + ".ZIP");
            Response.BinaryWrite(bytesInStream);
            Response.End();
        }

        public string Auth(string UserId, string Passwd)
        {
            ServiceReference4.CustomerPortalServiceClient proxy = new ServiceReference4.CustomerPortalServiceClient();

            proxy.Endpoint.Behaviors.Add(new CustomBehavior());

            ServiceReference4.ServiceResult objServiceResult = new ServiceReference4.ServiceResult();

            string AuthKey = proxy.Authenticate(UserId, Passwd);
            proxy.Close();

            return AuthKey;
            //return "";
        }

        private void GetPolicyDocumentForPortal()
        {
            //ServiceReference4.CustomerPortalServiceClient proxy = new ServiceReference4.CustomerPortalServiceClient();
            //proxy.Endpoint.Behaviors.Add(new CustomBehavior());

            //ServiceReference4.ServiceResult objServiceResult = new ServiceReference4.ServiceResult();
            //objServiceResult.UserData = new ServiceReference4.clsUserData();



            //string strProductCode = "3121";
            //string strProposalNumber =  "201609010000191";//"201608310000010";
            //string strProposalDate = "02/09/2016";
            //string strPolicyDocCode = "AP";// "AP";
            //string strPwd = "cmc!12345";
            //string strCustomerID = "1000010399";//"1000011628";
            //string strUserId = "3102710000";
            //string strLVAuthToken = Auth(strUserId, strPwd);

            //objServiceResult.UserData.UserId = strUserId;
            //objServiceResult.UserData.AuthenticateKey = strLVAuthToken;
            //objServiceResult.UserData.ProductCode = strProductCode;
            //objServiceResult.UserData.ProposalNumber = strProposalNumber;
            //objServiceResult.UserData.ProposalDate = strProposalDate;
            //objServiceResult.UserData.PolicyDocCode = strPolicyDocCode; // "AP";
            //objServiceResult.UserData.Password = strPwd;
            //objServiceResult.UserData.CustomerId = strCustomerID;


            //byte[] objbyte = proxy.GetPolicyDocumentForPortal(strLVAuthToken, ref objServiceResult);

            //Response.Clear();
            //Response.ContentType = "application/force-download";
            //Response.AddHeader("content-disposition", "attachment;filename=test.pdf");
            //Response.BinaryWrite(objbyte);
            //Response.End();
            ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
            proxy.Endpoint.Behaviors.Add(new CustomBehavior());

            ServiceReference1.ServiceResult objServiceResult = new ServiceReference1.ServiceResult();
            objServiceResult.UserData = new ServiceReference1.clsUserData();



            string strProductCode = "2876";
            //string strProductCode = "3121"; 
            string strProposalNumber = "201610200000378";//"201609210000189";
            //string strProposalDate = "24/10/2016";// "27 /09/2016";
            string strPolicyDocCode = "AP";
            string strPwd = "cmc!12345";
            //string strCustomerID = "1000012445";// "1000012244";
            string strUserId = "3102710000";// "3102710000";
            string strLVAuthToken = Auth(strUserId, strPwd);

            objServiceResult.UserData.UserId = strUserId;
            objServiceResult.UserData.AuthenticateKey = strLVAuthToken;
            objServiceResult.UserData.ProductCode = strProductCode;
            objServiceResult.UserData.ProposalNumber = strProposalNumber;
            objServiceResult.UserData.ProposalDate = "";// strProposalDate;
            objServiceResult.UserData.PolicyDocCode = strPolicyDocCode; // "AP";
            objServiceResult.UserData.Password = strPwd;
            objServiceResult.UserData.CustomerId = "";// strCustomerID;
            // objServiceResult.UserData.PolicyNO = "1000007400";
            //objServiceResult.UserData.PolicyNumber = "1000246000";// "1000007400";

            byte[] objbyte = proxy.GetPolicyDocumentForPortal(strUserId, strPwd, ref objServiceResult);

            if (objServiceResult.UserData.ErrorText.Length > 0)
            {
                Response.Write(objServiceResult.UserData.ErrorText);
            }
            else
            {
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment;filename=test.pdf");
                Response.BinaryWrite(objbyte);
                Response.End();
            }
        }

        protected void btnCreateDocumentDMS_Click(object sender, EventArgs e)
        {
            try
            {
                string InwardId = "";
                string CustomerId = "";
                string username = "";
                string password = "";
                string ApplicationNo = "";
                string DocumentUniqNo = "";
                string PolicyNo = "";
                string ClaimNo = "10110000585"; // "10110000436"; // "10410000179";
                string Source = "TEST";
                string FileName = "PolicyNumber_1000345500.pdf"; //"PolicyNumber_1000345500.zip";
                string DocumentType = "";
                
                //string FilePath = @"C:\Users\gc0062\Desktop\Desktop\calendar.zip";
                //string FilePath = @"C:\Users\gc0062\Desktop\Desktop\PolicyNumber_1000345500.zip";
                string FilePath = @"C:\Users\gc0062\Desktop\Desktop\PolicyNumber_1000345500.pdf";

                byte[] requestedByte = System.IO.File.ReadAllBytes(FilePath);

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();

                string status = proxy.CreateDocumentDMS(username, password, InwardId, ApplicationNo, CustomerId, DocumentUniqNo, PolicyNo, ClaimNo, Source, FileName, requestedByte, DocumentType);

                proxy.Close();
            }
            catch (Exception Ex)
            {
                Response.Write(Ex.Message);
            }
        }

        protected void btnDownloadDocumentDMS_Click(object sender, EventArgs e)
        {
            try
            {
                // string InwardId = "KP000116112900004"; // KP000116111100005
                // string CustomerId = "1000013977";

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());


                //ServiceReference1.ImagePath[] imgPath = proxy.SearchDocumentDMS("CMCDocs", "cmc123", "KP000116111100005", "", "1000013427", "", "", "", "");
                ServiceReference1.ImagePath[] imgPath = proxy.SearchDocumentDMS("CMCDocs", "cmc123", "", "", "", "", "", "10410000179", "");

                ServiceReference1.ImagePathDMS[] imgPathDMS = proxy.ViewDocumentDMS("CMCDocs", "cmc123", "Images_01122016055915.zip", imgPath[7].ImgPath);


                //proxy.CreateDocumentDMS(username, password, InwardId, ApplicationNo, CustomerId, DocumentUniqNo, PolicyNo, ClaimNo, Source, FileName, requestedByte, DocumentType)

                proxy.Close();

                byte[] bytesInStream = imgPathDMS[0].ResponseMemoryStream.ToArray(); // simpler way of converting to array
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment;filename=Images_01122016055915.zip");
                Response.BinaryWrite(bytesInStream);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}