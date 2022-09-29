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


namespace PrjPASS
{
    public partial class HealthSaveProposal : System.Web.UI.Page
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
                objServiceResult.UserData.ProductCode = "2876";
                objServiceResult.UserData.ModeOfOperation = "NEWPOLICY";
                //objServiceResult.UserData.AuthenticateKey = Auth(strUserId, strPassword);
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestXML());
                objServiceResult.UserData.ProposalGenerationMode = ServiceReference1.clsUserData.ProposalMode.SaveProposal;
                objServiceResult.UserData.IsInternalRiskGrid = true;
                objServiceResult.UserData.WorksheetInByte = true;

                objServiceResult.UserData.ErrorText = "";

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
                    string TotalPremium = objServiceResult.UserData.TotalPremium.ToString();
                    string NetPremium = objServiceResult.UserData.NetPremium.ToString();
                    string ServiceTax = objServiceResult.UserData.ServiceTax.ToString();

                    string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "HealthSaveProposalResultXML.xml";
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
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "HealthSaveProposal_Request_Murli.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "HealthSaveProposal_Request.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "HealthSaveProposal_Request_Ashutosh.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "HealthSaveProposal_Request_Bhavik.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "HealthSaveProposal_Request_Ashutosh_2Members.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "HealthSaveProposal_Request_Ashutosh_1Members.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "HealthSaveProposal_Request_Ashutosh_1Members_19_SEP.XML"; 
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "QH20160920655SaveProposal_Request.XML";
            strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "HealthSaveProposal_Request24112016.XML";
            


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