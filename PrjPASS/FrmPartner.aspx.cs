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

namespace PrjPASS
{
    public partial class FrmPartner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ServiceReference2.PartnerIntegrationServiceClient proxy = new ServiceReference2.PartnerIntegrationServiceClient();
            

            string strXML = GetRequestXML();

            string strXmlPathq = AppDomain.CurrentDomain.BaseDirectory + "PartnerRequest.xml";
            File.WriteAllText(strXmlPathq, String.Empty);
            File.WriteAllText(strXmlPathq, strXML);

            string result = proxy.SavePartner_integration(strXML);

            string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PartnerResult.xml";
            File.WriteAllText(strXmlPath, String.Empty);
            File.WriteAllText(strXmlPath, result);
        }

        private string GetRequestXML()
        {
            string strXmlPath = "";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarWorkingFine.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarNew.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCar.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PartnerXML.XML";
            strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PartnerXML_Copy.XML";

            XmlNode node = null;
            XmlDocument xmlfile = null;
            string xmlString = "";
            try
            {
                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/UserID");
                node.InnerXml = txtUserId.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/Password");
                node.InnerXml = txtPassword.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/ProductType");
                node.InnerXml = txtProductType.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/TransactionType");
                node.InnerXml = txtTransactionType.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/TransactionID");
                node.InnerXml = txtTransaction.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/PolicyNo");
                node.InnerXml = txtPolicyNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/ProposalDate");
                node.InnerXml = txtProposalDate.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/ProposalTime");
                node.InnerXml = txtProposalTime.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/PolicyStartDate");
                node.InnerXml = txtPolicyStartDate.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/PolicyEndDate");
                node.InnerXml = txtPolicyEndDate.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/ProposerType");
                node.InnerXml = txtProposalType.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/CustomerName");
                node.InnerXml = txtCustomerName.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/EngineNos");
                node.InnerXml = txtEngineNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/ChassisNos");
                node.InnerXml = txtChessisNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/Make");
                node.InnerXml = txtMake.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/Model");
                node.InnerXml = txtModel.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/Variant");
                node.InnerXml = txtVariant.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/IDV");
                node.InnerXml = txtIDV.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/TotalA");
                node.InnerXml = txtTotalA.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/TotalB");
                node.InnerXml = txtTotalB.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/GrossPremium");
                node.InnerXml = txtGWP.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/ServiceORSalesTax");
                node.InnerXml = txtServiceSalesTax.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/NetPremium");
                node.InnerXml = txtNetPremium.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/TransactionReference");
                node.InnerXml = txtTransactioReference.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/TransactionDate");
                node.InnerXml = txtTransactionDate.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/TransactionAmount");
                node.InnerXml = txtTransactionAmount.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/Prefix");
                node.InnerXml = txtPrefix.Text.Trim();

                /////////////////////////////////////

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/VehicleRegistrationNumber");
                node.InnerXml = txtVehicleRegistrationNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/BiFuelKit");
                node.InnerXml = txtBiFuelKit.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/BiFuelKitIDV");
                node.InnerXml = txtBiFuelKitIDV.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/NCB");
                node.InnerXml = txtNCB.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/LegalLiabilityPaidDriver");
                node.InnerXml = txtLegalLiabilityPaidDriver.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/ZeroDepreciation");
                node.InnerXml = txtZeroDepreciation.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/EngineProtector");
                node.InnerXml = txtEngineProtector.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/ReturnToInvoice");
                node.InnerXml = txtReturnToInvoice.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/PAUnnamedPassengerSI");
                node.InnerXml = txtPAUnnamedPassengerSI.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/Consumables_Addon");
                node.InnerXml = txtConsumables_Addon.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/RoadsideAssistance");
                node.InnerXml = txtRoadsideAssistance.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/IMTNos");
                node.InnerXml = txtIMTNos.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/PartnerIntegration/Additional_Info");
                node.InnerXml = txtAdditional_Info.Text.Trim();

                xmlString = xmlfile.InnerXml;

            }
            catch (Exception ex)
            {

            }
            return xmlString;
        }
    }
}