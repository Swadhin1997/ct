using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace PrjPASS
{
    public partial class PrivateCarSaveProposal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void SaveProposal()
        {
            bool IsSuccess = false;
            string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
            string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();
            try
            {
                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.ServiceResult objServiceResult = new ServiceReference1.ServiceResult();
                objServiceResult.UserData = new ServiceReference1.clsUserData();

                objServiceResult.UserData.IsWorkSheetRequired = false;
                objServiceResult.UserData.UserId = strUserId;
                objServiceResult.UserData.Password = strPassword;
                objServiceResult.UserData.UserRole = "ADMIN";
                objServiceResult.UserData.ProductCode = "3121";
                objServiceResult.UserData.ModeOfOperation = "NEWPOLICY";
                //objServiceResult.UserData.AuthenticateKey = Auth(strUserId, strPassword);
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestXML());
                objServiceResult.UserData.IsInternalRiskGrid = true;
                objServiceResult.UserData.ProposalGenerationMode = ServiceReference1.clsUserData.ProposalMode.SaveProposal;
                objServiceResult.UserData.ErrorText = "";
                objServiceResult.UserData.WorksheetInByte = true;
                


                //proxy.SaveProposal(objServiceResult.UserData.AuthenticateKey, ref objServiceResult);
                proxy.SaveProposal(strUserId, strPassword, ref objServiceResult);
                proxy.Close();

                IsSuccess = objServiceResult.UserData.ErrorText.Trim().Length > 0 ? false : true;

                if (objServiceResult.UserData.ErrorText.Trim().Length > 0)
                {
                    Response.Write(objServiceResult.UserData.ErrorText.Trim());
                }
                else
                {
                    string ResultXML = objServiceResult.UserData.UserResultXml;

                    string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarSaveProposalResultXML.xml";
                    File.WriteAllText(strXmlPath, String.Empty);
                    File.WriteAllText(strXmlPath, ResultXML);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        private string GetRequestXML()
        {
            string strXmlPath = "";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarSaveProposal_Request.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarSaveProposal_Request_Hasmukh.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "new.xml";
            strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarSaveProposal_Request_30SEP.XML";


            XmlNode node = null;
            XmlDocument xmlfile = null;
            string xmlString = "";
            try
            {
                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);

                DateTime dtAppDate = DateTime.Now;
                string strCurrentDate = dtAppDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
            return xmlString;
        }

        protected void btnSaveProposal_Click(object sender, EventArgs e)
        {
            SaveProposal();
        }
    }
}