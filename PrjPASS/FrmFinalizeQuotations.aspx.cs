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
using System.Net.Mail;
using System.Net.Mime;

using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;

using System.ServiceModel.Activation;


using System.Web.Script.Serialization;

using Microsoft.VisualBasic;

using System.Net;
using System.Security.Cryptography;


using Google;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Urlshortener.v1.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.ModelBinding;

namespace PrjPASS
{
    public partial class FrmFinalizeQuotations : System.Web.UI.Page
    {
        private static UrlshortenerService service;
        private static readonly Regex ShortUrlRegex =
                   new Regex("^http[s]?://goo.gl/", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool IsShortUrl(string url)
        {
            return ShortUrlRegex.IsMatch(url);
        }

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


                string strCustomerDOB = DateTime.Now.AddYears(-25).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                txtDateofBirth.Text = strCustomerDOB;

                int NumberOfDaysQuoteDetailsRequired = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfDaysQuoteDetailsRequired"].ToString());
                if (string.IsNullOrEmpty(HdnFromDate.Value.ToString()))
                {
                    txtFromDate.Text = DateTime.Now.AddDays(NumberOfDaysQuoteDetailsRequired).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    DateTime FromDate = DateTime.ParseExact(HdnFromDate.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    txtFromDate.Text = FromDate.Date.ToShortDateString();
                }
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseModalSaveProposal();", true);
            }
        }

        private string GetRequestXMLForRecalculate(string ExistingRequestXML, string IRDA_ProductCode, string EditedPolicyStartDate, ref bool IsRollover, out string PolicyStartDate, out string PolicyEndDate, out string ProposalDate)
        {

            PolicyStartDate = string.Empty;
            PolicyEndDate = string.Empty;
            ProposalDate = string.Empty;

            XmlNode node = null;
            XmlDocument xmlfile = null;
            string xmlString = "";
            try
            {
                TextReader sr = new StringReader(ExistingRequestXML);
                xmlfile = new XmlDocument();
                xmlfile.Load(sr);

                DateTime dtAppDate = DateTime.Now;
                string strCurrentDate = dtAppDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_NoPrevInsuranceFlag");
                //node.Attributes["Value"].Value = rbbtNewBusiness.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ExtraDD1");
                if (node == null)
                {
                    if (IRDA_ProductCode == "1063")
                    {
                        StringBuilder sbInnerXML = new StringBuilder();
                        string XML_Node = @"<PropRisks_ExtraDD1 Type=""String"" Value=""1063"" /><PropRisks_ExtraDD6 Type=""String"" Value=""3"" />";
                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block");
                        sbInnerXML = new StringBuilder(node.InnerXml);
                        sbInnerXML.Append(XML_Node);
                        node.InnerXml = sbInnerXML.ToString();
                    }
                    else if (IRDA_ProductCode == "1062")
                    {
                        StringBuilder sbInnerXML = new StringBuilder();
                        string XML_Node = @"<PropRisks_ExtraDD1 Type=""String"" Value=""1062"" /><PropRisks_ExtraDD6 Type=""String"" Value=""3"" />";
                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block");
                        sbInnerXML = new StringBuilder(node.InnerXml);
                        sbInnerXML.Append(XML_Node);
                        node.InnerXml = sbInnerXML.ToString();
                    }
                    else
                    {
                        StringBuilder sbInnerXML = new StringBuilder();
                        string XML_Node = @"<PropRisks_ExtraDD1 Type=""String"" Value=""1011"" /><PropRisks_ExtraDD6 Type=""String"" Value=""1"" />";
                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block");
                        sbInnerXML = new StringBuilder(node.InnerXml);
                        sbInnerXML.Append(XML_Node);
                        node.InnerXml = sbInnerXML.ToString();
                    }
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralNodes_ApplicationDate");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ExtraDD5");
                if (node == null)
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerType");
                    bool IsIndividual = true;
                    IsIndividual = (node.Attributes["Value"].Value == "" || node.Attributes["Value"].Value.ToUpper() == "I") ? true : false;

                    StringBuilder sbInnerXML = new StringBuilder();
                    string XML_Node = IsIndividual ? @"<PropRisks_ExtraDD5 Type=""String"" Value=""MALE"" />" : @"<PropRisks_ExtraDD5 Type=""String"" Value="""" />";
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block");
                    sbInnerXML = new StringBuilder(node.InnerXml);
                    sbInnerXML.Append(XML_Node);
                    node.InnerXml = sbInnerXML.ToString();
                }

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropBranchDetails_IMDBranchName");
                //node.Attributes["Value"].Value = ""; // drpBranchName.SelectedItem.Text;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropBranchDetails_IMDBranchCode");
                //node.Attributes["Value"].Value = ""; // drpBranchName.SelectedValue;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryCode");
                //node.Attributes["Value"].Value = lblIntermediaryCode.Text;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryName");
                //node.Attributes["Value"].Value = txtIntermediaryName.Text;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropProductName");
                //node.Attributes["Value"].Value = drpProductType.SelectedValue;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropFieldUserDetails_FiledUserUserID");
                //node.Attributes["Value"].Value = "";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_LeadGenerator");
                //node.Attributes["Value"].Value = txtLeadGenerator.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_BusinessType_Mandatary");
                IsRollover = node.Attributes["Value"].Value == "New Business" ? false : true;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropDistributionChannel_BusineeChanneltype");
                //node.Attributes["Value"].Value = lblIntermediaryBusineeChannelType.Text.Trim();

                //string CUSTOMERID_INDIVIDUAL = ConfigurationManager.AppSettings["CUSTOMERID_INDIVIDUAL"].ToString();
                //string CUSTOMERNAME_INDIVIDUAL = ConfigurationManager.AppSettings["CUSTOMERNAME_INDIVIDUAL"].ToString();
                //string CUSTOMERID_ORG = ConfigurationManager.AppSettings["CUSTOMERID_ORG"].ToString();
                //string CUSTOMERNAME_ORG = ConfigurationManager.AppSettings["CUSTOMERNAME_ORG"].ToString();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerID_Mandatary");
                //node.Attributes["Value"].Value = CustomerId > 0 ? CustomerId.ToString() : (rbctIndividual.Checked ? CUSTOMERID_INDIVIDUAL : CUSTOMERID_ORG);

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerName");
                //node.Attributes["Value"].Value = CustomerId > 0 ? CustomerName : (rbctIndividual.Checked ? CUSTOMERNAME_INDIVIDUAL : CUSTOMERNAME_ORG);

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_MainDriver");
                //node.Attributes["Value"].Value = rbctIndividual.Checked ? "Self - Owner Driver" : "Any Other";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerType");
                //node.Attributes["Value"].Value = rbctIndividual.Checked ? "I" : "C"; //rbctIndividual.Checked ? "Individual" : "Organization";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_PrevYearNCB");
                //node.Attributes["Value"].Value = drpPreviousYearNCBSlab.SelectedValue;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Manufacture");
                //node.Attributes["Value"].Value = drpVehicleMake.SelectedItem.Text.ToUpper();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ManufacturerCode");
                //node.Attributes["Value"].Value = drpVehicleMake.SelectedValue.ToUpper();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Model");
                //node.Attributes["Value"].Value = drpVehicleModel.SelectedItem.Text.ToUpper();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ModelCode");
                //node.Attributes["Value"].Value = drpVehicleModel.SelectedValue;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_TypeofPolicyHolder");
                //node.Attributes["Value"].Value = drpPolicyHolderType.SelectedValue;


                //if (drpVehicleSubType.SelectedIndex > 0)
                //{
                //    string[] strVST = drpVehicleSubType.SelectedItem.Text.Split('[');

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ModelVariant");
                //    node.Attributes["Value"].Value = strVST[0].Trim().ToUpper();

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VariantCode");
                //    node.Attributes["Value"].Value = drpVehicleSubType.SelectedValue.ToUpper();
                //}

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VehicleSegment");
                //node.Attributes["Value"].Value = lblVehicleSegment.Text.Trim().ToUpper();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_SeatingCapacity");
                //node.Attributes["Value"].Value = lblSeatingCapacityt.Text.Trim();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_FuelType");
                //node.Attributes["Value"].Value = lblFuelTypet.Text.Trim();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IsExternalCNGLPGAvailable");
                //node.Attributes["Value"].Value = chkCNGLPG.Checked ? "True" : "False";

                //if (chkCNGLPG.Checked)
                //{
                //    StringBuilder OtherDetailsGrid_InnerXML = new StringBuilder();
                //    string strExternalCNGLPG_XML_Node = string.Format(@"<CNGandLPGKitDetails Name=""CNG and LPG Kit"" Value=""0""><CNGandLPGKitDetails Type=""GroupData""><SI Name=""SI"" Value=""{0}"" Type=""Double""/></CNGandLPGKitDetails></CNGandLPGKitDetails>", txtLPGKitSumInsured.Text.Trim());
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                //    OtherDetailsGrid_InnerXML = new StringBuilder(node.InnerXml);
                //    OtherDetailsGrid_InnerXML.Append(strExternalCNGLPG_XML_Node);
                //    node.InnerXml = OtherDetailsGrid_InnerXML.ToString();

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CNGLPGkitValue");
                //    node.Attributes["Value"].Value = txtLPGKitSumInsured.Text.Trim();
                //}

                //DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string strDOR = dtDOR.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DateofRegistration");
                //node.Attributes["Value"].Value = strDOR;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Dateofpurchase");
                //node.Attributes["Value"].Value = strDOR;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ManufactureYear");
                //node.Attributes["Value"].Value = dtDOR.Year.ToString();


                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VehicleAge_Mandatary");
                //node.Attributes["Value"].Value = txtVehicleAge.Text.Trim();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RTOCode");
                //node.Attributes["Value"].Value = drpRTOLocation.SelectedValue.Trim();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_AuthorityLocation");
                //node.Attributes["Value"].Value = txtRTOAuthorityCode.Text.Trim();


                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RTOCluster");
                //node.Attributes["Value"].Value = lblRTOCluster.Text.Trim().ToUpper();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CubicCapacity");
                //node.Attributes["Value"].Value = lblCubicCapacityt.Text.Trim();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ModelCluster");
                //node.Attributes["Value"].Value = lblModelCluster.Text.Trim().ToUpper();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CSD");
                //node.Attributes["Value"].Value = chkCSD.Checked ? "True" : "False";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfClaimFreeYearsCompleted");
                //node.Attributes["Value"].Value = txtNoofClaimFreeYearsCompleted.Text.Trim();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IDVofthevehicle");
                //node.Attributes["Value"].Value = txtIDVofVehicle.Text.Trim() == "" ? "0" : txtIDVofVehicle.Text.Trim();

                //if (chkNEAR.Checked)
                //{
                //    StringBuilder NEAR_InnerXML = new StringBuilder();
                //    string XML_Node = string.Format(@"<NonElectricalAccessoriesDetails Name=""Non Electrical Accessories"" Value=""0""><NonElectricalAccessoriesDetails Type=""GroupData""><SI Name=""SI"" Value=""{0}"" Type=""Double""/></NonElectricalAccessoriesDetails></NonElectricalAccessoriesDetails><NonElectricalAccessiories Name=""NonElectricalAccessiories"" Value=""GRP288""><NonElectricalAccessiories Type=""GroupData""><Make Name=""Make"" Value=""hhhh"" Type=""String"" /><Model Name=""Model"" Value=""h11"" Type=""String"" /><Year Name=""Year"" Value=""2016"" Type=""Double"" /><IDV Name=""IDV"" Value=""{0}"" Type=""Double"" /><TypeofAccessories Name=""TypeofAccessories"" Value=""Lugguage Carrier"" Type=""String"" /><Description Name=""Description"" Value=""iiii"" Type=""String"" /><SerialNo Name=""SerialNo"" Value=""h1111"" Type=""String"" /><Remarks Name=""Remarks"" Value=""jjjj"" Type=""String"" /></NonElectricalAccessiories></NonElectricalAccessiories>", txtNeaSumInsured.Text.Trim(), txtNeaSumInsured.Text.Trim());
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                //    NEAR_InnerXML = new StringBuilder(node.InnerXml);
                //    NEAR_InnerXML.Append(XML_Node);
                //    node.InnerXml = NEAR_InnerXML.ToString();
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NonElectricalAccessories");
                //    node.Attributes["Value"].Value = txtNeaSumInsured.Text.Trim();

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IsNonElctrclAcesrsRequired");
                //    node.Attributes["Value"].Value = "True";
                //}

                //if (chkEAR.Checked)
                //{
                //    StringBuilder EAR_InnerXML = new StringBuilder();
                //    string XML_Node = string.Format(@"<ElectronicAccessoriesDetails Name=""Electronic Accessories"" Value=""0""><ElectronicAccessoriesDetails Type=""GroupData""><SI Name=""SI"" Value=""{0}"" Type=""Double""/></ElectronicAccessoriesDetails></ElectronicAccessoriesDetails><ElectricalAccessiories Name=""ElectricalAccessiories"" Value=""GRP289""><ElectricalAccessiories Type=""GroupData""><Make Name=""Make"" Value=""kkkk"" Type=""String"" /><Model Name=""Model"" Value=""k11"" Type=""String"" /><Year Name=""Year"" Value=""2015"" Type=""Double"" /><IDV Name=""IDV"" Value=""{0}"" Type=""Double"" /><TypeofAccessories Name=""TypeofAccessories"" Value=""Fan"" Type=""String"" /><Description Name=""Description"" Value=""lllll"" Type=""String"" /><SerialNo Name=""SerialNo"" Value=""l22"" Type=""String"" /><Remarks Name=""Remarks"" Value=""mmmm"" Type=""String"" /></ElectricalAccessiories></ElectricalAccessiories>", txtEaSumInsured.Text.Trim(), txtEaSumInsured.Text.Trim());
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                //    EAR_InnerXML = new StringBuilder(node.InnerXml);
                //    EAR_InnerXML.Append(XML_Node);
                //    node.InnerXml = EAR_InnerXML.ToString();

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ElectricalAccessories");
                //    node.Attributes["Value"].Value = txtEaSumInsured.Text.Trim();

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_EmployeesDiscount");
                //    node.Attributes["Value"].Value = "True";
                //}

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CompulsoryPAwithOwnerDriver");
                //node.Attributes["Value"].Value = rbctIndividual.Checked ? "True" : "False"; //rbCpwYes.Checked ? "True" : "False"; 
                //// this rbCpwYes is commented because as per the new requirement when customer type is individual then 
                //// only CompulsoryPAwithOwnerDriver will be true else false;


                ////node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ImprtdVehcleWoutPaymtCustmDuty");
                ////node.Attributes["Value"].Value = rbivwpYes.Checked ? "True" : "False";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VoluntaryDeductibleAmount");
                //node.Attributes["Value"].Value = drpVDA.SelectedValue;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ReturnToInvoice");
                //node.Attributes["Value"].Value = chkReturnToInvoice.Checked ? "True" : "False";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RoadsideAssistance");
                //node.Attributes["Value"].Value = chkRoadsideAssistance.Checked ? "True" : "False";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_EngineSecure");
                //node.Attributes["Value"].Value = chkEngineProtect.Checked ? "True" : "False";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DepreciationReimbursement");
                //node.Attributes["Value"].Value = chkDepreciationCover.Checked ? "True" : "False";


                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VlntryDedctbleFrDprctnCover");
                //node.Attributes["Value"].Value = chkDepreciationCover.Checked ? "1000" : "0";


                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ConsumablesExpenses");
                //node.Attributes["Value"].Value = chkConsumableCover.Checked ? "True" : "False";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_MarketMovement");
                //node.Attributes["Value"].Value = txtMarketMovement.Text.Trim();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_InsuredCreditScore");
                //node.Attributes["Value"].Value = txtInsuredCreditScore.Text.Trim();

                //if (chkPACoverUnnamedPersons.Checked)
                //{
                //    StringBuilder sbInnerXML = new StringBuilder();
                //    string XML_Node = @"<UnnamedPAcoverDetails Name=""Unnamed PA cover"" Value=""0""><UnnamedPAcoverDetails Type=""GroupData""><SI Name=""SI"" Value=""0"" Type=""Double""/></UnnamedPAcoverDetails></UnnamedPAcoverDetails>";
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                //    sbInnerXML = new StringBuilder(node.InnerXml);
                //    sbInnerXML.Append(XML_Node);
                //    node.InnerXml = sbInnerXML.ToString();

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NumberofPersonsUnnamed");
                //    node.Attributes["Value"].Value = txtNumberOfPersonsUnnamed.Text.Trim();

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CapitalSIPerPerson");
                //    node.Attributes["Value"].Value = drpCapitalSIPerPerson.Text.Trim();
                //}

                //if (chkPACoverNamedPersons.Checked)
                //{
                //    StringBuilder sbInnerXML = new StringBuilder();
                //    string XML_Node = @"<NamedPACoverDetails Name=""Named PA Cover"" Value=""0""><NamedPACoverDetails Type=""GroupData""><SI Name=""SI"" Value=""0"" Type=""Double""/></NamedPACoverDetails></NamedPACoverDetails>";
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                //    sbInnerXML = new StringBuilder(node.InnerXml);
                //    sbInnerXML.Append(XML_Node);
                //    node.InnerXml = sbInnerXML.ToString();

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail");
                //    node.Attributes["Value"].Value = "GRP291";

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonsNamed");
                //    node.Attributes["Value"].Value = txtNumberofPersonsNamed.Text.Trim();

                //    StringBuilder sbInnerXML2 = new StringBuilder();
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail");
                //    sbInnerXML2 = new StringBuilder(node.InnerXml);

                //    string xmlnode2 = @"<NamedpassengerNomineeDetail Type=""GroupData""><NamedPerson Name=""NamedPerson"" Value=""Rahul8"" Type=""String""/><CapitalSIPerPerson Name=""CapitalSIPerPerson"" Value=""0"" Type=""Double""/><Nominee Name=""Nominee"" Value=""ooooo"" Type=""String""/><Relationship Name=""Relationship"" Value=""Dependent Son"" Type=""String""/><AgeofNominee Name=""AgeofNominee"" Value=""16"" Type=""Double""/><Nameofappointee Name=""Nameofappointee"" Value=""pppp"" Type=""String""/><RelationshipToNominee Name=""RelationshipToNominee"" Value=""Dependent Son"" Type=""String""/></NamedpassengerNomineeDetail>";
                //    for (int i = 0; i < Convert.ToInt16(txtNumberofPersonsNamed.Text.Trim()); i++)
                //    {
                //        sbInnerXML2.Append(xmlnode2);
                //    }

                //    node.InnerXml = sbInnerXML2.ToString();

                //    int intNumberofPersonsNamed = 0;
                //    XmlNodeList nodeList_NamedpassengerNomineeDetail = node.SelectNodes("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail/NamedpassengerNomineeDetail");
                //    foreach (XmlElement item in nodeList_NamedpassengerNomineeDetail)
                //    {
                //        intNumberofPersonsNamed++;
                //        node = item.SelectSingleNode("CapitalSIPerPerson");
                //        node.Attributes["Value"].Value = drpCapitalSINamed.Text.Trim();

                //        if (intNumberofPersonsNamed == Convert.ToInt16(txtNumberofPersonsNamed.Text.Trim()))
                //        {
                //            break;
                //        }
                //    }

                //    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail/NamedpassengerNomineeDetail/CapitalSIPerPerson");
                //    //node.Attributes["Value"].Value = drpCapitalSINamed.Text.Trim();
                //}

                //if (chkPACoverPaidDriver.Checked)
                //{
                //    StringBuilder sbInnerXML = new StringBuilder();
                //    string XML_Node = @"<PaiddriverPAcoverDetails Name=""Paid driver PA cover"" Value=""0""><PaiddriverPAcoverDetails Type=""GroupData""><SI Name=""SI"" Value=""0"" Type=""Double""/></PaiddriverPAcoverDetails></PaiddriverPAcoverDetails>";
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                //    sbInnerXML = new StringBuilder(node.InnerXml);
                //    sbInnerXML.Append(XML_Node);
                //    node.InnerXml = sbInnerXML.ToString();

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonsPaidDriver");
                //    node.Attributes["Value"].Value = drpNoofPaidDrivers.SelectedValue;

                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_SumInsuredPaidDriver");
                //    node.Attributes["Value"].Value = drpSIPaidDriver.SelectedValue;
                //}

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_WiderLegalLiabilityToPaid");
                //node.Attributes["Value"].Value = chkWLLPD.Checked ? "True" : "False";

                //if (chkWLLPD.Checked)
                //{
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonWiderLegalLiability");
                //    node.Attributes["Value"].Value = txtNoofPersonsWLL.Text.Trim();
                //}

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_LegalLiabilityToEmployees");
                //node.Attributes["Value"].Value = chkLLEE.Checked ? "True" : "False";

                //if (chkLLEE.Checked)
                //{
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonsLiabilityEmployee");
                //    node.Attributes["Value"].Value = txtNoOfEmployees.Text.Trim();
                //}

                //DateTime dtAppDate1 = DateTime.Now.AddDays(2);
                //string strCurrentDate1 = dtAppDate1.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                //if (IsRollover)
                //{
                //DateTime dt = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //dt = dt.AddDays(1);
                //string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                //node.Attributes["Value"].Value = strdate;
                //}
                //else
                //{

                //}

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                //DateTime dt = DateTime.ParseExact(node.Attributes["Value"].Value.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //if (dt.Date < DateTime.Now.Date)
                //{
                //    node.Attributes["Value"].Value = strCurrentDate;
                //}

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromhour_Mandatary");
                node.Attributes["Value"].Value = "00:00"; // DateTime.Now.ToShortTimeString();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralNodes_PartnerApplicationDate");
                //node.Attributes["Value"].Value = strCurrentDate;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_BranchInwardDate");
                //node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropReferenceNoDate_ReferenceDate_Mandatary");
                node.Attributes["Value"].Value = strCurrentDate;

                if (IsRollover)
                {
                    //DateTime dt = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //dt = dt.AddDays(1);
                    //string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                    //node.Attributes["Value"].Value = strdate;

                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    //node.Attributes["Value"].Value = dt.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    //node.Attributes["Value"].Value = DateTime.Now.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                    //node.Attributes["Value"].Value = strCurrentDate;
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                DateTime dtPSD = DateTime.ParseExact(string.IsNullOrEmpty(EditedPolicyStartDate) ? node.Attributes["Value"].Value : EditedPolicyStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                node.Attributes["Value"].Value = string.IsNullOrEmpty(EditedPolicyStartDate) ? node.Attributes["Value"].Value : EditedPolicyStartDate;
                PolicyStartDate = node.Attributes["Value"].Value;

                if (IsRollover)
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    //node.Attributes["Value"].Value = dtPSD.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node.Attributes["Value"].Value = FN_Get_Policy_End_Date(PolicyStartDate, 1);
                    PolicyEndDate = node.Attributes["Value"].Value;
                }
                else
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    //node.Attributes["Value"].Value = dtPSD.AddYears(3).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node.Attributes["Value"].Value = FN_Get_Policy_End_Date(PolicyStartDate, 3);
                    PolicyEndDate = node.Attributes["Value"].Value;
                }

                if (dtPSD >= DateTime.Today) //N094369, Proposal Date Should Be Past Of Current Date
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_ProposalDate_Mandatary");
                    node.Attributes["Value"].Value = strCurrentDate;
                    ProposalDate = node.Attributes["Value"].Value;
                }
                else
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_ProposalDate_Mandatary");
                    node.Attributes["Value"].Value = string.IsNullOrEmpty(EditedPolicyStartDate) ? strCurrentDate : EditedPolicyStartDate;
                    ProposalDate = node.Attributes["Value"].Value;
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(EditedPolicyStartDate) ? strCurrentDate : EditedPolicyStartDate;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                //DateTime dt = DateTime.ParseExact(node.Attributes["Value"].Value.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //if (dt.Date < DateTime.Now.Date)
                //{
                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                //node.Attributes["Value"].Value = DateTime.Now.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                //node.Attributes["Value"].Value = strCurrentDate;
                //}
                //else
                //{

                //}

                /*if (rbbtRollOver.Checked)
                {
                    StringBuilder sbInnerXML = new StringBuilder();
                    string XML_Node = @"<PreviousPolicyDetails><PreviousPolicyDetails Type=""GroupData""><PropPreviousPolicyDetails_AddressLine2 Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_AddressLine3 Type=""String"" Value=""""/><PropPreviousPolicyDetails_AmountOfClaimsReject Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_City Type=""String"" Value=""""/><PropPreviousPolicyDetails_ClaimAmount Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_ClaimDate Type=""String"" Value=""""/><PropPreviousPolicyDetails_ClaimLodgedInPast Type=""Boolean"" Value=""False""/>
                    <PropPreviousPolicyDetails_ClaimNo Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_ClaimOutstanding Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_ClaimPremium Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_ClaimReconDate Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_ClaimSettled Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_ClaimsMode Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_CorporateCustomerId_Mandatary Type=""String"" Value=""BAJAJ ALLIANZ""/><PropPreviousPolicyDetails_Currency Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_DateofLoss Type=""String"" Value=""""/><PropPreviousPolicyDetails_DateofSale Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_Deductibles Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_DiscountonPremiumoffered Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_DocumentProof Type=""String"" Value=""No Claim""/><PropPreviousPolicyDetails_DurationOfInterruption Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_FullPartiIncdtThreats Type=""String"" Value=""""/><PropPreviousPolicyDetails_IncurredClaimRatio Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_InspectionDate Type=""String"" Value=""""/><PropPreviousPolicyDetails_InspectionDone Type=""Boolean"" Value=""False""/>
                    <PropPreviousPolicyDetails_InspectionDoneByWhom Type=""String"" Value=""""/><PropPreviousPolicyDetails_IsDataDeleted Type=""Boolean"" Value=""False""/>
                    <PropPreviousPolicyDetails_IsOldDataDeleted Type=""Boolean"" Value=""False""/><PropPreviousPolicyDetails_IssuingOfficeCode Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_NCBAbroadCheck Type=""Boolean"" Value=""False""/><PropPreviousPolicyDetails_NameOfTPA Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_NatureOfDisease Type=""String"" Value=""""/><PropPreviousPolicyDetails_NatureofLoss Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_NoOfClaims Type=""String"" Value=""""/><PropPreviousPolicyDetails_NoOfClaimsOutStanding Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_NoOfClaimsPaid Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_NoOfClaimsReject Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_NoOfInsuredAtEnd Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_NoOfInsuredCovered Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_OfficeAddress Type=""String"" Value=""aaaaaa""/><PropPreviousPolicyDetails_OfficeCode Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_PinCode Type=""String"" Value=""""/><PropPreviousPolicyDetails_PolicyEffectiveFrom Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_PolicyEffectiveTo Type=""String"" Value=""""/><PropPreviousPolicyDetails_PolicyNo Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_PolicyPremium Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_PolicyVariant Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_PolicyYear_Mandatary Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_PreviousInsPincode Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_ProductCode Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_QuantumOfClaim Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_ReasonForNotRenewing Type=""String"" Value=""""/><PropPreviousPolicyDetails_ReferenceNo Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_RiskCode Type=""String"" Value=""""/><PropPreviousPolicyDetails_State Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_StpsTknDelInciThrtsPvntRec Type=""String"" Value=""""/><PropPreviousPolicyDetails_SumInsuredOpted Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_SumInsuredPerFamily Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_TotalClaimsAmount Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_VehicleSold Type=""Boolean"" Value=""False""/><PropPreviousPolicyDetails_WhtrPrevPolicyCpySubmitted Type=""Boolean"" Value=""False""/>
                    <PropPreviousPolicyDetails_claimreportedorsetteled Type=""String"" Value=""""/></PreviousPolicyDetails></PreviousPolicyDetails>";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails");
                    sbInnerXML = new StringBuilder(node.InnerXml);
                    sbInnerXML.Append(XML_Node);
                    node.InnerXml = sbInnerXML.ToString();


                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPreviousPolicyDetails_PreviousPolicyType");
                    //node.Attributes["Value"].Value = drpCoverType.SelectedValue;
                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_TotalClaimsAmount");
                    //node.Attributes["Value"].Value = txtTotalClaimAmount.Text;

                    DateTime dtPE = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //dtPE = dtPE.AddDays(-1);
                    string strPE = dtPE.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyEffectiveTo");
                    node.Attributes["Value"].Value = strPE;
                    string PreviousPolicyStartDate = dtPE.AddYears(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyEffectiveFrom");
                    node.Attributes["Value"].Value = PreviousPolicyStartDate;

                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_NoOfClaims");
                    //node.Attributes["Value"].Value = drpTotalClaimCount.SelectedValue;

                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyNo");
                    //node.Attributes["Value"].Value = "11223344";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyYear_Mandatary");
                    node.Attributes["Value"].Value = dtPE.AddYears(-1).Year.ToString();  //"2015";

                    //if (drpCoverType.SelectedIndex == 0) //only comprehensive product type
                    //{
                    //    if (drpTotalClaimCount.SelectedIndex == 0) //no claim
                    //    {
                    //        string selectedValue = drpPreviousYearNCBSlab.SelectedItem.Text;
                    //        drpPreviousYearNCBSlab.SelectedIndex = drpPreviousYearNCBSlab.SelectedIndex + 1;
                    //        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoClaimBonusApplicable");
                    //        node.Attributes["Value"].Value = drpPreviousYearNCBSlab.SelectedItem.Text;
                    //        drpPreviousYearNCBSlab.SelectedIndex = drpPreviousYearNCBSlab.SelectedItem.Text == selectedValue ? drpPreviousYearNCBSlab.SelectedIndex : drpPreviousYearNCBSlab.SelectedIndex - 1;

                    //        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_IsNCBApplicable");
                    //        node.Attributes["Value"].Value = "True";
                    //    }
                    //}
                }*/

                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");

            }
            catch (Exception ex)
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPath, "Error: " + ex.StackTrace.ToString());

                ExceptionUtility.LogException(ex, "GetRequestXMLForRecalculate");

                lblstatus.Text = ex.StackTrace;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            finally
            {
            }
            return xmlString;
        }

        private string GetRequestXMLForSaveProposal(DataSet dsRequest, long CustomerId, string CustomerName, out bool isNewBusiness, out string RegistrationDate
            , out string IRDA_ProductCode, out bool IsSendPaymentLink, out string TenureOwnerDriver
            , out bool IsDepreciationCover, out bool IsDailyCarAllowance, out string ddlDailyCarAllowance, out bool IsKeyReplacement, out string ddlKeyReplacement, out bool IsLossofPersonalBelongings, out string ddlLossofPersonalBelongingsSI, out string drpVDA)
        {
            IsSendPaymentLink = true;

            isNewBusiness = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString() == "New Business" ? true : false;
            RegistrationDate = dsRequest.Tables[0].Rows[0]["PropRisks_DateofRegistration"].ToString();
            IRDA_ProductCode = dsRequest.Tables[0].Rows[0]["IRDA_ProductCode"].ToString();
            TenureOwnerDriver = dsRequest.Tables[0].Rows[0]["TenureForOwnerDriver"].ToString();
            TenureOwnerDriver = string.IsNullOrEmpty(TenureOwnerDriver) ? "0" : TenureOwnerDriver;

           
            IsDepreciationCover = dsRequest.Tables[0].Rows[0]["PropRisks_DepreciationReimbursement"].ToString() == "True" ? true : false;
            IsDailyCarAllowance = dsRequest.Tables[0].Rows[0]["PropRisks_DailyCarAllowanceChk"].ToString() == "True" ? true : false;
            ddlDailyCarAllowance = dsRequest.Tables[0].Rows[0]["PropRisks_DailyCarAllowanceSIDD"].ToString();
            IsKeyReplacement = dsRequest.Tables[0].Rows[0]["PropRisks_KeyReplacementChk"].ToString() == "True" ? true : false;
            ddlKeyReplacement = dsRequest.Tables[0].Rows[0]["PropRisks_KeyReplacementSIDD"].ToString();
            IsLossofPersonalBelongings = dsRequest.Tables[0].Rows[0]["PropRisks_LossofPersonalBelongingschk"].ToString() == "True" ? true : false;
            ddlLossofPersonalBelongingsSI = dsRequest.Tables[0].Rows[0]["PropRisks_LossofPersonalBelongingsSIDD"].ToString();
            drpVDA = dsRequest.Tables[0].Rows[0]["PropRisks_VoluntaryDeductibleAmount"].ToString();

            string strXmlPath = "";

            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarSaveProposal_Request_New.XML";
            strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarSaveProposal_CP.XML";

            XmlNode node = null;
            XmlDocument xmlfile = null;
            string xmlString = "";
            try
            {
                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);

                DateTime dtAppDate = DateTime.Now;
                string strCurrentDate = dtAppDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (dsRequest.Tables[0].Rows[0]["PropCustomerDtls_CustomerType"].ToString() == "Individual")
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ExtraDD5");
                    node.Attributes["Value"].Value = rbtMale.Checked ? "MALE" : "FEMALE";
                }
                else
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ExtraDD5");
                    node.Attributes["Value"].Value = ""; //SET BLANK FOR ORG
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ExtraDD1");
                node.Attributes["Value"].Value = IRDA_ProductCode;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ExtraDD6");
                node.Attributes["Value"].Value = TenureOwnerDriver == "0" ? "" : TenureOwnerDriver;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropMODetails_PrimaryMOCode");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropMODetails_PrimaryMOCode"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropMODetails_PrimaryMOName");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropMODetails_PrimaryMOName"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_BranchOfficeCode");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_OfficeCode"].ToString();

                string officeCode = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_OfficeCode"].ToString();
                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_DisplayOfficeCode");
                node.Attributes["Value"].Value = officeCode.Substring(1, officeCode.Length - 1);

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_OfficeCode");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_OfficeCode"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_OfficeName");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_OfficeName"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_NoPrevInsuranceFlag");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_NoPrevInsuranceFlag"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralNodes_ApplicationDate");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropBranchDetails_IMDBranchName");
                node.Attributes["Value"].Value = ""; // drpBranchName.SelectedItem.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropBranchDetails_IMDBranchCode");
                node.Attributes["Value"].Value = ""; // drpBranchName.SelectedValue;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryCode");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropIntermediaryDetails_IntermediaryCode"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryName");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropIntermediaryDetails_IntermediaryName"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropProductName");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropProductName"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropFieldUserDetails_FiledUserUserID");
                node.Attributes["Value"].Value = "";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_LeadGenerator");
                //node.Attributes["Value"].Value = txtLeadGenerator.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_BusinessType_Mandatary");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropDistributionChannel_BusineeChanneltype");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropDistributionChannel_BusineeChanneltype"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryType");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropIntermediaryDetails_IntermediaryType"].ToString();

                //string CUSTOMERID_INDIVIDUAL = ConfigurationManager.AppSettings["CUSTOMERID_INDIVIDUAL"].ToString();
                //string CUSTOMERNAME_INDIVIDUAL = ConfigurationManager.AppSettings["CUSTOMERNAME_INDIVIDUAL"].ToString();
                //string CUSTOMERID_ORG = ConfigurationManager.AppSettings["CUSTOMERID_ORG"].ToString();
                //string CUSTOMERNAME_ORG = ConfigurationManager.AppSettings["CUSTOMERNAME_ORG"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerID_Mandatary");
                node.Attributes["Value"].Value = CustomerId.ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerName");
                node.Attributes["Value"].Value = CustomerName;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_MainDriver");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_MainDriver"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_BasicODDeviation");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_BasicODDeviation"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_AddOnDeviation");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_AddOnDeviation"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_FirstName");
                // node.Attributes["Value"].Value = rbtIndividual.Checked ? txtFirstName.Text.Trim() : txtOrganizationName.Text.Trim();
                // BUG ID 1529
                node.Attributes["Value"].Value = rbtIndividual.Checked ? txtFirstName.Text.Trim() : "NA";
                // BUG ID 1529

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_LastName");
                //node.Attributes["Value"].Value = rbtIndividual.Checked ? txtLastName.Text.Trim() : txtOrganizationName.Text.Trim();
                // BUG ID 1529
                node.Attributes["Value"].Value = rbtIndividual.Checked ? txtLastName.Text.Trim() : "NA";
                // BUG ID 1529

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerType");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropCustomerDtls_CustomerType"].ToString() == "Individual" ? "I" : "C";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_PrevYearNCB");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_PrevYearNCB"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPreviousPolicyDetails_NCBPercentage");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_PrevYearNCB"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Manufacture");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_Manufacture"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ManufacturerCode");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_ManufacturerCode"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Model");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_Model"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ModelCode"); //uncommented on 06Oct2017
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_ModelCode"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_TypeofPolicyHolder");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_TypeofPolicyHolder"].ToString();


                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ModelVariant");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_ModelVariant"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VariantCode");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_VariantCode"].ToString();


                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VehicleSegment");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_VehicleSegment"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_SeatingCapacity");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_SeatingCapacity"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_FuelType");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_FuelType"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IsExternalCNGLPGAvailable");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_IsExternalCNGLPGAvailable"].ToString();

                if (rbtIndividual.Checked && TenureOwnerDriver != "0")
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/OwnerDriverNomineeDetails/OwnerDriverNomineeDetails/Nominee");
                    node.Attributes["Value"].Value = txtNomineeName.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/OwnerDriverNomineeDetails/OwnerDriverNomineeDetails/NomineeDOB");
                    node.Attributes["Value"].Value = txtNomineeDOB.Text.Trim();

                    DateTime dtNomineeDOB = DateTime.ParseExact(txtNomineeDOB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime now = DateTime.Today;
                    int age = now.Year - dtNomineeDOB.Year;
                    if (now < dtNomineeDOB.AddYears(age)) age--;
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/OwnerDriverNomineeDetails/OwnerDriverNomineeDetails/Age");
                    node.Attributes["Value"].Value = age.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_AgeCoverageDetails");
                    node.Attributes["Value"].Value = age.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/OwnerDriverNomineeDetails/OwnerDriverNomineeDetails/Relationship");
                    node.Attributes["Value"].Value = drpNomineeRelationship.SelectedValue;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/OwnerDriverNomineeDetails/OwnerDriverNomineeDetails/Nameofappointee");
                    node.Attributes["Value"].Value = txtNameOfAppointee.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/OwnerDriverNomineeDetails/OwnerDriverNomineeDetails/Relationshiptonominee");
                    node.Attributes["Value"].Value = drpRelationshipWithAppointee.SelectedValue;
                }
                else
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/OwnerDriverNomineeDetails");
                    node.RemoveAll();
                }


                if (dsRequest.Tables[0].Rows[0]["PropRisks_IsExternalCNGLPGAvailable"].ToString().ToLower() == "true")
                {
                    StringBuilder OtherDetailsGrid_InnerXML = new StringBuilder();
                    string strExternalCNGLPG_XML_Node = string.Format(@"<CNGandLPGKitDetails Name=""CNG and LPG Kit"" Value=""0""><CNGandLPGKitDetails Type=""GroupData""><SI Name=""SI"" Value=""{0}"" Type=""Double""/></CNGandLPGKitDetails></CNGandLPGKitDetails>", dsRequest.Tables[0].Rows[0]["PropRisks_CNGLPGkitValue"].ToString());
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    OtherDetailsGrid_InnerXML = new StringBuilder(node.InnerXml);
                    OtherDetailsGrid_InnerXML.Append(strExternalCNGLPG_XML_Node);
                    node.InnerXml = OtherDetailsGrid_InnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CNGLPGkitValue");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_CNGLPGkitValue"].ToString();
                }

                //DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string strDOR = dtDOR.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DateofRegistration");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_DateofRegistration"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Dateofpurchase");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_Dateofpurchase"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ManufactureYear");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_ManufactureYear"].ToString();


                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VehicleAge_Mandatary");
                //node.Attributes["Value"].Value = txtVehicleAge.Text.Trim();

                if (dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString().ToLower() == "roll over")
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RegistrationNumber");
                    node.Attributes["Value"].Value = txtRN1.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RegistrationNumber2");
                    node.Attributes["Value"].Value = txtRN2.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RegistrationNumber3");
                    node.Attributes["Value"].Value = txtRN3.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RegistrationNumber4");
                    node.Attributes["Value"].Value = txtRN4.Text.Trim();
                }
                else
                {
                    //IF IT IS A NEW BUSINESS CASE AND (RN3 OR RN4) IS BLANK THEN RN1 WILL BE "NEW" AND RN2, RN3, RN4 WILL BE SET BLANK
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RegistrationNumber");
                    node.Attributes["Value"].Value = (string.IsNullOrEmpty(txtRN3.Text.Trim()) || string.IsNullOrEmpty(txtRN4.Text.Trim())) ? "NEW" : txtRN1.Text.Trim();
                    txtRN1.Text = string.IsNullOrEmpty(txtRN3.Text.Trim()) || string.IsNullOrEmpty(txtRN4.Text.Trim()) ? "NEW" : txtRN1.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RegistrationNumber2");
                    node.Attributes["Value"].Value = txtRN1.Text.Trim() == "NEW" ? "" : txtRN2.Text.Trim();
                    txtRN2.Text = txtRN1.Text.Trim() == "NEW" ? "" : txtRN2.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RegistrationNumber3");
                    node.Attributes["Value"].Value = txtRN1.Text.Trim() == "NEW" ? "" : txtRN3.Text.Trim();
                    txtRN3.Text = txtRN1.Text.Trim() == "NEW" ? "" : txtRN3.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RegistrationNumber4");
                    node.Attributes["Value"].Value = txtRN1.Text.Trim() == "NEW" ? "" : txtRN4.Text.Trim();
                    txtRN4.Text = txtRN1.Text.Trim() == "NEW" ? "" : txtRN4.Text.Trim();
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Engineno");
                node.Attributes["Value"].Value = txtEngineNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ChassisNumber");
                node.Attributes["Value"].Value = txtChassisNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RTOCode");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_RTOCode"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_AuthorityLocation");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_AuthorityLocationName"].ToString();


                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RTOCluster");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_RTOCluster"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Zone_Mandatary");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_Zone_Mandatary"])) ? "Zone-A" : Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_Zone_Mandatary"]);

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CubicCapacity");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_CubicCapacity"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ModelCluster");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_ModelCluster"].ToString();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CSD");
                //node.Attributes["Value"].Value = chkCSD.Checked ? "True" : "False";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfClaimFreeYearsCompleted");
                //node.Attributes["Value"].Value = txtNoofClaimFreeYearsCompleted.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IDVofthevehicle");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_IDVofthevehicle"].ToString();

                string PropRisks_Manufacture = dsRequest.Tables[0].Rows[0]["PropRisks_Manufacture"].ToString();
                PropRisks_Manufacture = PropRisks_Manufacture.Replace("&", "&amp;");

                string PropRisks_Model = dsRequest.Tables[0].Rows[0]["PropRisks_Model"].ToString();
                PropRisks_Model = PropRisks_Model.Replace("&", "&amp;");

                if (Convert.ToInt32(dsRequest.Tables[0].Rows[0]["PropRisks_NonElectricalAccessories"].ToString()) > 0)
                {
                    StringBuilder NEAR_InnerXML = new StringBuilder();
                    string XML_Node = string.Format(@"<NonElectricalAccessoriesDetails Name=""Non Electrical Accessories"" Value=""0""><NonElectricalAccessoriesDetails Type=""GroupData""><SI Name=""SI"" Value=""{0}"" Type=""Double""/></NonElectricalAccessoriesDetails></NonElectricalAccessoriesDetails><NonElectricalAccessiories Name=""NonElectricalAccessiories"" Value=""GRP288""><NonElectricalAccessiories Type=""GroupData""><Make Name=""Make"" Value=""{1}"" Type=""String"" /><Model Name=""Model"" Value=""{2}"" Type=""String"" /><Year Name=""Year"" Value=""{3}"" Type=""Double"" /><IDV Name=""IDV"" Value=""{0}"" Type=""Double"" /><TypeofAccessories Name=""TypeofAccessories"" Value=""Lugguage Carrier"" Type=""String"" /><Description Name=""Description"" Value=""NA"" Type=""String"" /><SerialNo Name=""SerialNo"" Value=""NA"" Type=""String"" /><Remarks Name=""Remarks"" Value=""NA"" Type=""String"" /></NonElectricalAccessiories></NonElectricalAccessiories>", dsRequest.Tables[0].Rows[0]["PropRisks_NonElectricalAccessories"].ToString(), PropRisks_Manufacture, PropRisks_Model, dsRequest.Tables[0].Rows[0]["PropRisks_ManufactureYear"].ToString());
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    NEAR_InnerXML = new StringBuilder(node.InnerXml);
                    NEAR_InnerXML.Append(XML_Node);
                    node.InnerXml = NEAR_InnerXML.ToString();
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NonElectricalAccessories");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_NonElectricalAccessories"].ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IsNonElctrclAcesrsRequired");
                    node.Attributes["Value"].Value = "True";
                }

                if (Convert.ToInt32(dsRequest.Tables[0].Rows[0]["PropRisks_ElectricalAccessories"].ToString()) > 0)
                {
                    StringBuilder EAR_InnerXML = new StringBuilder();
                    string XML_Node = string.Format(@"<ElectronicAccessoriesDetails Name=""Electronic Accessories"" Value=""0""><ElectronicAccessoriesDetails Type=""GroupData""><SI Name=""SI"" Value=""{0}"" Type=""Double""/></ElectronicAccessoriesDetails></ElectronicAccessoriesDetails><ElectricalAccessiories Name=""ElectricalAccessiories"" Value=""GRP289""><ElectricalAccessiories Type=""GroupData""><Make Name=""Make"" Value=""{1}"" Type=""String"" /><Model Name=""Model"" Value=""{2}"" Type=""String"" /><Year Name=""Year"" Value=""{3}"" Type=""Double"" /><IDV Name=""IDV"" Value=""{0}"" Type=""Double"" /><TypeofAccessories Name=""TypeofAccessories"" Value=""Other"" Type=""String"" /><Description Name=""Description"" Value=""NA"" Type=""String"" /><SerialNo Name=""SerialNo"" Value=""NA"" Type=""String"" /><Remarks Name=""Remarks"" Value=""NA"" Type=""String"" /></ElectricalAccessiories></ElectricalAccessiories>", dsRequest.Tables[0].Rows[0]["PropRisks_ElectricalAccessories"].ToString(), PropRisks_Manufacture, PropRisks_Model, dsRequest.Tables[0].Rows[0]["PropRisks_ManufactureYear"].ToString());
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    EAR_InnerXML = new StringBuilder(node.InnerXml);
                    EAR_InnerXML.Append(XML_Node);
                    node.InnerXml = EAR_InnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ElectricalAccessories");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_ElectricalAccessories"].ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_EmployeesDiscount");
                    node.Attributes["Value"].Value = "True";
                }

                if (rbtIndividual.Checked)
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IsPAToOwnerDriverExcluded");
                    node.Attributes["Value"].Value = TenureOwnerDriver == "0" ? "True" : "False";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ValidDrvgLisc");
                    node.Attributes["Value"].Value = TenureOwnerDriver == "0" ? "False" : "True";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CompulsoryPAwithOwnerDriver");
                    node.Attributes["Value"].Value = TenureOwnerDriver == "0" ? "False" : "True"; //rbCpwYes.Checked ? "True" : "False"; 
                }
                else
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IsPAToOwnerDriverExcluded");
                    node.Attributes["Value"].Value = "True";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ValidDrvgLisc");
                    node.Attributes["Value"].Value = "False";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CompulsoryPAwithOwnerDriver");
                    node.Attributes["Value"].Value = "False";
                }

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CompulsoryPAwithOwnerDriver");
                //node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_CompulsoryPAwithOwnerDriver"].ToString(); //rbctIndividual.Checked ? "True" : "False"; //rbCpwYes.Checked ? "True" : "False"; 
                // this rbCpwYes is commented because as per the new requirement when customer type is individual then 
                // only CompulsoryPAwithOwnerDriver will be true else false;


                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ImprtdVehcleWoutPaymtCustmDuty");
                //node.Attributes["Value"].Value = rbivwpYes.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VoluntaryDeductibleAmount");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_VoluntaryDeductibleAmount"].ToString(); //drpVDA.SelectedValue;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ReturnToInvoice");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_ReturnToInvoice"].ToString(); //chkReturnToInvoice.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RoadsideAssistance");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_RoadsideAssistance"].ToString(); //chkRoadsideAssistance.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_EngineSecure");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_EngineSecure"].ToString(); // chkEngineProtect.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DepreciationReimbursement");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_DepreciationReimbursement"].ToString(); //chkDepreciationCover.Checked ? "True" : "False";


                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VlntryDedctbleFrDprctnCover");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_VlntryDedctbleFrDprctnCover"].ToString(); //chkDepreciationCover.Checked ? "1000" : "0";


                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ConsumablesExpenses");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_ConsumablesExpenses"].ToString(); //chkConsumableCover.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_MarketMovement");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_MarketMovement"].ToString(); //txtMarketMovement.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_InsuredCreditScore");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_InsuredCreditScore"].ToString(); //txtInsuredCreditScore.Text.Trim();

                //CR164
                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_LossofPersonalBelongingschk");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_LossofPersonalBelongingschk"])) ? "False" : dsRequest.Tables[0].Rows[0]["PropRisks_LossofPersonalBelongingschk"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_KeyReplacementChk");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_KeyReplacementChk"])) ? "False" : dsRequest.Tables[0].Rows[0]["PropRisks_KeyReplacementChk"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NCBProtectChk");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_NCBProtectChk"])) ? "False" : dsRequest.Tables[0].Rows[0]["PropRisks_NCBProtectChk"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_TyreCoverChk");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_TyreCoverChk"])) ? "False" : dsRequest.Tables[0].Rows[0]["PropRisks_TyreCoverChk"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DailyCarAllowanceChk");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_DailyCarAllowanceChk"])) ? "False" : dsRequest.Tables[0].Rows[0]["PropRisks_DailyCarAllowanceChk"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_LossofPersonalBelongingsSIDD");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_LossofPersonalBelongingsSIDD"])) ? "" : dsRequest.Tables[0].Rows[0]["PropRisks_LossofPersonalBelongingsSIDD"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_KeyReplacementSIDD");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_KeyReplacementSIDD"])) ? "" : dsRequest.Tables[0].Rows[0]["PropRisks_KeyReplacementSIDD"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DailyCarAllowanceSIDD");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_DailyCarAllowanceSIDD"])) ? "" : dsRequest.Tables[0].Rows[0]["PropRisks_DailyCarAllowanceSIDD"].ToString();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_TyreCoverText");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(Convert.ToString(dsRequest.Tables[0].Rows[0]["PropRisks_TyreCoverText"])) ? "" : dsRequest.Tables[0].Rows[0]["PropRisks_TyreCoverText"].ToString();
                //

                if (rbtIndividual.Checked && TenureOwnerDriver != "0")
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NameOfNominee");
                    node.Attributes["Value"].Value = txtNomineeName.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DateofBirthofNominee");
                    node.Attributes["Value"].Value = txtNomineeDOB.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Relationship");
                    node.Attributes["Value"].Value = drpNomineeRelationship.SelectedValue;
                }

                if (Convert.ToInt32(dsRequest.Tables[0].Rows[0]["PropRisks_CapitalSIPerPerson"].ToString()) > 0)
                {
                    StringBuilder sbInnerXML = new StringBuilder();
                    string XML_Node = @"<UnnamedPAcoverDetails Name=""Unnamed PA cover"" Value=""0""><UnnamedPAcoverDetails Type=""GroupData""><SI Name=""SI"" Value=""0"" Type=""Double""/></UnnamedPAcoverDetails></UnnamedPAcoverDetails>";
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    sbInnerXML = new StringBuilder(node.InnerXml);
                    sbInnerXML.Append(XML_Node);
                    node.InnerXml = sbInnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NumberofPersonsUnnamed");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_NumberofPersonsUnnamed"].ToString(); //txtNumberOfPersonsUnnamed.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CapitalSIPerPerson");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_CapitalSIPerPerson"].ToString(); // drpCapitalSIPerPerson.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_OtherChk2");
                    node.Attributes["Value"].Value = "True";
                }

                if (Convert.ToInt32(dsRequest.Tables[0].Rows[0]["PropRisks_NoOfPersonsNamed"].ToString()) > 0)
                {
                    StringBuilder sbInnerXML = new StringBuilder();
                    string XML_Node = @"<NamedPACoverDetails Name=""Named PA Cover"" Value=""0""><NamedPACoverDetails Type=""GroupData""><SI Name=""SI"" Value=""0"" Type=""Double""/></NamedPACoverDetails></NamedPACoverDetails>";
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    sbInnerXML = new StringBuilder(node.InnerXml);
                    sbInnerXML.Append(XML_Node);
                    node.InnerXml = sbInnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail");
                    node.Attributes["Value"].Value = "GRP291";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonsNamed");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_NoOfPersonsNamed"].ToString(); //txtNumberofPersonsNamed.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_OtherChk1");
                    node.Attributes["Value"].Value = "True";

                    StringBuilder sbInnerXML2 = new StringBuilder();
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail");
                    sbInnerXML2 = new StringBuilder(node.InnerXml);

                    //commented by hasmukh as named cover is removed - it is commented while working on CR132
                    //string xmlnode2 = string.Format(@"<NamedpassengerNomineeDetail Type=""GroupData""><NamedPerson Name=""NamedPerson"" Value=""{0}"" Type=""String""/><CapitalSIPerPerson Name=""CapitalSIPerPerson"" Value=""0"" Type=""Double""/><Nominee Name=""Nominee"" Value=""{1}"" Type=""String""/><Relationship Name=""Relationship"" Value=""{2}"" Type=""String""/><AgeofNominee Name=""AgeofNominee"" Value=""{3}"" Type=""Double""/><Nameofappointee Name=""Nameofappointee"" Value=""{4}"" Type=""String""/><RelationshipToNominee Name=""RelationshipToNominee"" Value=""{5}"" Type=""String""/></NamedpassengerNomineeDetail>", CustomerName, txtNomineeName.Text.Trim(), drpNomineeRelationship.SelectedValue, age, txtNameOfAppointee.Text.Trim(), drpRelationshipWithAppointee.SelectedValue);
                    //for (int i = 0; i < Convert.ToInt16(dsRequest.Tables[0].Rows[0]["PropRisks_NoOfPersonsNamed"].ToString()); i++)
                    //{
                    //    sbInnerXML2.Append(xmlnode2);
                    //}

                    node.InnerXml = sbInnerXML2.ToString();

                    int intNumberofPersonsNamed = 0;
                    XmlNodeList nodeList_NamedpassengerNomineeDetail = node.SelectNodes("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail/NamedpassengerNomineeDetail");
                    foreach (XmlElement item in nodeList_NamedpassengerNomineeDetail)
                    {
                        intNumberofPersonsNamed++;
                        node = item.SelectSingleNode("CapitalSIPerPerson");
                        node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["CapitalSIPerPerson"].ToString(); //drpCapitalSINamed.Text.Trim();

                        if (intNumberofPersonsNamed == Convert.ToInt16(dsRequest.Tables[0].Rows[0]["PropRisks_NoOfPersonsNamed"].ToString()))
                        {
                            break;
                        }
                    }

                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail/NamedpassengerNomineeDetail/CapitalSIPerPerson");
                    //node.Attributes["Value"].Value = drpCapitalSINamed.Text.Trim();
                }

                if (Convert.ToInt32(dsRequest.Tables[0].Rows[0]["PropRisks_NoOfPersonsPaidDriver"].ToString()) > 0)
                {
                    StringBuilder sbInnerXML = new StringBuilder();
                    string XML_Node = @"<PaiddriverPAcoverDetails Name=""Paid driver PA cover"" Value=""0""><PaiddriverPAcoverDetails Type=""GroupData""><SI Name=""SI"" Value=""0"" Type=""Double""/></PaiddriverPAcoverDetails></PaiddriverPAcoverDetails>";
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    sbInnerXML = new StringBuilder(node.InnerXml);
                    sbInnerXML.Append(XML_Node);
                    node.InnerXml = sbInnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonsPaidDriver");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_NoOfPersonsPaidDriver"].ToString(); // drpNoofPaidDrivers.SelectedValue;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_SumInsuredPaidDriver");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_SumInsuredPaidDriver"].ToString(); // drpSIPaidDriver.SelectedValue;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_OtherChk3");
                    node.Attributes["Value"].Value = "True";
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_WiderLegalLiabilityToPaid");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_WiderLegalLiabilityToPaid"].ToString(); //chkWLLPD.Checked ? "True" : "False";

                if (dsRequest.Tables[0].Rows[0]["PropRisks_WiderLegalLiabilityToPaid"].ToString().ToLower() == "true")
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonWiderLegalLiability");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_NoOfPersonWiderLegalLiability"].ToString(); //txtNoofPersonsWLL.Text.Trim();
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_LegalLiabilityToEmployees");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_LegalLiabilityToEmployees"].ToString(); //chkLLEE.Checked ? "True" : "False";

                if (dsRequest.Tables[0].Rows[0]["PropRisks_LegalLiabilityToEmployees"].ToString().ToLower() == "true")
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonsLiabilityEmployee");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_NoOfPersonsLiabilityEmployee"].ToString(); //txtNoOfEmployees.Text.Trim();
                }

                if (dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString().ToLower() == "roll over")
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_PolicyEffectivedate"].ToString();
                }
                else
                {
                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                    //node.Attributes["Value"].Value = strCurrentDate;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_PolicyEffectivedate"].ToString();
                }



                // CR 145 Start here
                string strPolicyStartDate = dsRequest.Tables[0].Rows[0]["PropPolicyEffectivedate_Fromdate_Mandatary"].ToString();
                string strPreviousPolicyEndDate = dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_PolicyEffectiveTo"].ToString();


                DateTime dtPSD = DateTime.ParseExact(strPolicyStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (isNewBusiness == true)
                {
                    if (dtPSD == DateTime.Today)
                    {
                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromhour_Mandatary");
                        node.Attributes["Value"].Value = DateTime.Now.ToString("HH:mm");
                    }
                    else if (dtPSD > DateTime.Today)
                    {
                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromhour_Mandatary");
                        node.Attributes["Value"].Value = "00:00";
                    }
                }
                else //roll over
                {

                    DateTime dtPreviousPolicyExpiryDate = DateTime.ParseExact(strPreviousPolicyEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime dtPolicyStartDate = DateTime.ParseExact(strPolicyStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    int numDaysDiff = Math.Abs(dtPolicyStartDate.Subtract(dtPreviousPolicyExpiryDate).Days);

                    if (numDaysDiff <= 1) //no break in
                    {
                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromhour_Mandatary");
                        node.Attributes["Value"].Value = "00:00";
                    }
                    else //break in
                    {
                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromhour_Mandatary");
                        node.Attributes["Value"].Value = DateTime.Now.ToString("HH:mm");
                    }
                }

                if (dtPSD < DateTime.Today) //CR215 4)	If policy start date is less than today’s date, user should not have option to send digital payment link, only save proposal option should be there. This applicable for all roles .	
                {
                    IsSendPaymentLink = false;
                }

                // CR 145 End here


                //string PolicyEffectiveDate = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_PolicyEffectivedate"].ToString();
                //DateTime dt_PolicyEffectiveDate = DateTime.ParseExact(PolicyEffectiveDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //if (dt_PolicyEffectiveDate > DateTime.Today)
                //{
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_ProposalDate_Mandatary");
                //    node.Attributes["Value"].Value = strCurrentDate;
                //}
                //else
                //{
                //    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_ProposalDate_Mandatary");
                //    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_PolicyEffectivedate"].ToString(); //strCurrentDate;
                //}

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_ProposalDate_Mandatary");
                node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_ProposalDate_Mandatary"].ToString(); //strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralNodes_PartnerApplicationDate");
                node.Attributes["Value"].Value = strCurrentDate;

                string strQuoteNo = lblQuoteNumber.Text.Replace("Quote No.: ", "");
                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralNodes_PartnerReferanceNO");
                node.Attributes["Value"].Value = txtPartnerApplicationNumber.Text.Trim() == "" ? strQuoteNo.Split(' ')[0] : txtPartnerApplicationNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropEndorsementDtls_TypeOfTransfer");
                node.Attributes["Value"].Value = drpTypeOfTransfer.SelectedValue;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_BranchInwardDate");
                //node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropReferenceNoDate_ReferenceDate_Mandatary");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_BreakInInsurance");
                node.Attributes["Value"].Value = string.IsNullOrEmpty(dsRequest.Tables[0].Rows[0]["PropRisks_BreakInInsurance"].ToString()) ? "No Break" : dsRequest.Tables[0].Rows[0]["PropRisks_BreakInInsurance"].ToString();

                if (dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString().ToLower() == "roll over")
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPolicyEffectivedate_Fromdate_Mandatary"].ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPolicyEffectivedate_Todate_Mandatary"].ToString(); //dt.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    //node.Attributes["Value"].Value = DateTime.Now.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                    //node.Attributes["Value"].Value = strCurrentDate;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPolicyEffectivedate_Fromdate_Mandatary"].ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPolicyEffectivedate_Todate_Mandatary"].ToString(); //dt.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString().ToLower() == "roll over")
                {
                    string PreviousInsurerName = drpPreviousInsurerName.SelectedValue.Replace("&", "&amp;");
                    StringBuilder sbInnerXML = new StringBuilder();
                    string XML_Node = string.Format(@"<PreviousPolicyDetails><PreviousPolicyDetails Type=""GroupData""><PropPreviousPolicyDetails_AddressLine2 Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_AddressLine3 Type=""String"" Value=""""/><PropPreviousPolicyDetails_AmountOfClaimsReject Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_City Type=""String"" Value=""""/><PropPreviousPolicyDetails_ClaimAmount Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_ClaimDate Type=""String"" Value=""""/><PropPreviousPolicyDetails_ClaimLodgedInPast Type=""Boolean"" Value=""False""/>
                    <PropPreviousPolicyDetails_ClaimNo Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_ClaimOutstanding Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_ClaimPremium Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_ClaimReconDate Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_ClaimSettled Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_ClaimsMode Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_CorporateCustomerId_Mandatary Type=""String"" Value=""{0}""/><PropPreviousPolicyDetails_Currency Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_DateofLoss Type=""String"" Value=""""/><PropPreviousPolicyDetails_DateofSale Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_Deductibles Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_DiscountonPremiumoffered Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_DocumentProof Type=""String"" Value=""No Claim""/><PropPreviousPolicyDetails_DurationOfInterruption Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_FullPartiIncdtThreats Type=""String"" Value=""""/><PropPreviousPolicyDetails_IncurredClaimRatio Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_InspectionDate Type=""String"" Value=""""/><PropPreviousPolicyDetails_InspectionDone Type=""Boolean"" Value=""False""/>
                    <PropPreviousPolicyDetails_InspectionDoneByWhom Type=""String"" Value=""""/><PropPreviousPolicyDetails_IsDataDeleted Type=""Boolean"" Value=""False""/>
                    <PropPreviousPolicyDetails_IsOldDataDeleted Type=""Boolean"" Value=""False""/><PropPreviousPolicyDetails_IssuingOfficeCode Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_NCBAbroadCheck Type=""Boolean"" Value=""False""/><PropPreviousPolicyDetails_NameOfTPA Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_NatureOfDisease Type=""String"" Value=""""/><PropPreviousPolicyDetails_NatureofLoss Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_NoOfClaims Type=""String"" Value=""""/><PropPreviousPolicyDetails_NoOfClaimsOutStanding Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_NoOfClaimsPaid Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_NoOfClaimsReject Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_NoOfInsuredAtEnd Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_NoOfInsuredCovered Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_OfficeAddress Type=""String"" Value=""NA""/><PropPreviousPolicyDetails_OfficeCode Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_PinCode Type=""String"" Value=""""/><PropPreviousPolicyDetails_PolicyEffectiveFrom Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_PolicyEffectiveTo Type=""String"" Value=""""/><PropPreviousPolicyDetails_PolicyNo Type=""String"" Value=""{1}""/>
                    <PropPreviousPolicyDetails_PolicyPremium Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_PolicyVariant Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_PolicyYear_Mandatary Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_PreviousInsPincode Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_ProductCode Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_QuantumOfClaim Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_ReasonForNotRenewing Type=""String"" Value=""""/><PropPreviousPolicyDetails_ReferenceNo Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_RiskCode Type=""String"" Value=""""/><PropPreviousPolicyDetails_State Type=""String"" Value=""""/>
                    <PropPreviousPolicyDetails_StpsTknDelInciThrtsPvntRec Type=""String"" Value=""""/><PropPreviousPolicyDetails_SumInsuredOpted Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_SumInsuredPerFamily Type=""Double"" Value=""0""/><PropPreviousPolicyDetails_TotalClaimsAmount Type=""Double"" Value=""0""/>
                    <PropPreviousPolicyDetails_VehicleSold Type=""Boolean"" Value=""False""/><PropPreviousPolicyDetails_WhtrPrevPolicyCpySubmitted Type=""Boolean"" Value=""False""/>
                    <PropPreviousPolicyDetails_claimreportedorsetteled Type=""String"" Value=""""/></PreviousPolicyDetails></PreviousPolicyDetails>", PreviousInsurerName, txtPreviousPolicyNumber.Text.Trim());

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails");
                    sbInnerXML = new StringBuilder(node.InnerXml);
                    sbInnerXML.Append(XML_Node);
                    node.InnerXml = sbInnerXML.ToString();


                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPreviousPolicyDetails_PreviousPolicyType");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_PreviousPolicyType"].ToString(); // drpCoverType.SelectedValue;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_TotalClaimsAmount");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_TotalClaimsAmount"].ToString(); //txtTotalClaimAmount.Text;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_ClaimAmount");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_TotalClaimsAmount"].ToString(); //txtTotalClaimAmount.Text;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyEffectiveTo");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_PolicyEffectiveTo"].ToString(); //strPE;


                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyEffectiveFrom");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_PolicyEffectiveFrom"].ToString(); //PreviousPolicyStartDate;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_NoOfClaims");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_NoOfClaims"].ToString(); // drpTotalClaimCount.SelectedValue;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_DocumentProof");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_NoOfClaims"].ToString(); // drpTotalClaimCount.SelectedValue;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyNo");
                    node.Attributes["Value"].Value = txtPreviousPolicyNumber.Text.Trim(); //dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_PolicyNo"].ToString(); //"11223344";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyYear_Mandatary");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropPreviousPolicyDetails_PolicyYear_Mandatary"].ToString();  //dtPE.AddYears(-1).Year.ToString();  //"2015";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoClaimBonusApplicable");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropRisks_NoClaimBonusApplicable"].ToString();  //drpPreviousYearNCBSlab.SelectedItem.Text;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_IsNCBApplicable");
                    node.Attributes["Value"].Value = dsRequest.Tables[0].Rows[0]["PropGeneralProposalInformation_IsNCBApplicable"].ToString(); //"True";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NCBConfirmation");
                    node.Attributes["Value"].Value = "On declaration";
                }

                #region _FinancierDetails Data XMl
                if (chkFinancier.Checked)
                {
                    StringBuilder sbInnerXML = new StringBuilder();
                    string XML_Node = string.Format(@"<FinancierDetails><FinancierDetails Type=""GroupData""><PropFinancierDetails_Address Type=""String"" Value=""Financier address""/><PropFinancierDetails_AgreementType Type=""String"" Value=""Hypothecation""/><PropFinancierDetails_FinancierCode_Mandatary Type=""Double"" Value=""1005""/><PropFinancierDetails_FinancierName Type=""String"" Value=""HDFC BANK LTD""/><PropFinancierDetails_IsDataDeleted Type=""Boolean"" Value=""False""/><PropFinancierDetails_IsOldDataDeleted Type=""Boolean"" Value=""False""/><PropFinancierDetails_LoanAccountNo Type=""String"" Value=""""/><PropFinancierDetails_Remarks Type=""String"" Value=""""/><PropFinancierDetails_SrNo_Mandatary Type=""Double"" Value=""1""/></FinancierDetails></FinancierDetails>");

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails");
                    sbInnerXML = new StringBuilder(node.InnerXml);
                    sbInnerXML.Append(XML_Node);
                    node.InnerXml = sbInnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/FinancierDetails/FinancierDetails/PropFinancierDetails_Address");
                    node.Attributes["Value"].Value = txtFinancierAddress.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/FinancierDetails/FinancierDetails/PropFinancierDetails_AgreementType");
                    node.Attributes["Value"].Value = txtFinacierAgrrementType.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/FinancierDetails/FinancierDetails/PropFinancierDetails_FinancierCode_Mandatary");
                    node.Attributes["Value"].Value = hdnFinancierCode.Value.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/FinancierDetails/FinancierDetails/PropFinancierDetails_FinancierName");
                    node.Attributes["Value"].Value = hdnFinancierName.Value.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/FinancierDetails/FinancierDetails/PropFinancierDetails_LoanAccountNo");
                    node.Attributes["Value"].Value = txtLoanAccountNumber.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/FinancierDetails/FinancierDetails/PropFinancierDetails_SrNo_Mandatary ");
                    node.Attributes["Value"].Value = txtFileNumber.Text.Trim();


                }

                #endregion


                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");

            }
            catch (Exception ex)
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPath, "Error: " + ex.StackTrace.ToString());

                ExceptionUtility.LogException(ex, "GetRequestXMLForSaveProposal");

                lblstatus.Text = ex.StackTrace;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            finally
            {
            }
            return xmlString;
        }

        public string FN_Get_Policy_End_Date(string vPolicyStartDate, int nPolicyTerm)
        {
            DateTime dt = DateTime.ParseExact(vPolicyStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (dt.Year % 4 == 0)
            {
                if (dt.Day == 29 && dt.Month == 2)
                {
                    return dt.AddYears(nPolicyTerm).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    return dt.AddYears(nPolicyTerm).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
            else
            {
                return dt.AddYears(nPolicyTerm).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }

        private DataSet GetRequestXMLDataset(string QuoteNumber, string QuoteVersion, string RegistrationNumber = null)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection();
            try
            {
                string ResponseXML = string.Empty;

                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_CALCULATE_PREMIUM_REQUEST_RESPONSE";
                    cmd.CommandTimeout = 99999;
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNumber);
                    cmd.Parameters.AddWithValue("@QuoteVersion", QuoteVersion);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", RegistrationNumber);
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                ExceptionUtility.LogException(ex, "GetRequestXMLDataset QuoteNumber: " + QuoteNumber + " : QuoteVersion:" + QuoteVersion);
            }
            return ds;
        }

        private void GetRTOAuthorityLocationFromXML(string RequestXML, out string PropRisks_AuthorityLocation) //here PropRisks_AuthorityLocation = MH01 OR MH02 etc.
        {
            PropRisks_AuthorityLocation = string.Empty;
            try
            {
                XmlDocument xmlfile = null;
                string xmlString = "";
                XmlNode SingleNode = null;
                xmlfile = new XmlDocument();
                xmlfile.LoadXml(RequestXML);

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_AuthorityLocation");
                PropRisks_AuthorityLocation = SingleNode.Attributes["Value"].Value;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetRTOAuthorityLocationFromXML Method FrmFinalizeQuotations.aspx.cs");
            }
        }

        /*private void GetResultXMLFromDB(string QuoteNumber, ref bool isNewBusiness, ref string NetPremium, ref string ServiceTax, ref string TotalPremium, ref string RequestXML, ref string ResultXML, ref string MarketMovement, ref long CreditScoreId, ref string CreditScoreCustomerName, ref string CreditScoreIDProof, ref string CreditScoreIDProofNumber, ref string CGSTAmount, ref string CGSTPercentage, ref string SGSTAmount, ref string SGSTPercentage, ref string IGSTAmount, ref string IGSTPercentage, ref string UGSTAmount, ref string UGSTPercentage, ref string TotalGSTAmount, ref bool IsFastlaneFlow, ref int SelectedQuoteVersion, ref string RegistrationDate, ref string IRDA_ProductCode, ref bool IsCustomerMale, ref string TenureOwnerDriver)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                string ResponseXML = string.Empty;
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_CALCULATE_PREMIUM_REQUEST_RESPONSE";
                    cmd.CommandTimeout = 9999;
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNumber);
                    cmd.Parameters.AddWithValue("@QuoteVersion", SelectedQuoteVersion);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            RequestXML = string.Format("{0}", sdr["RequestXML"]);
                            ResultXML = string.Format("{0}", sdr["ResponseXML"]);

                            isNewBusiness = sdr["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString() == "New Business" ? true : false;
                            NetPremium = sdr["NetPremium"].ToString();
                            ServiceTax = sdr["ServiceTax"].ToString();
                            TotalPremium = sdr["TotalPremium"].ToString();
                            MarketMovement = sdr["PropRisks_MarketMovement"].ToString();
                            CreditScoreId = Convert.ToInt64(sdr["CreditScoreId"].ToString());

                            CGSTAmount = sdr["CGSTAmount"].ToString();
                            CGSTPercentage = sdr["CGSTPercentage"].ToString();

                            SGSTAmount = sdr["SGSTAmount"].ToString();
                            SGSTPercentage = sdr["SGSTPercentage"].ToString();

                            IGSTAmount = sdr["IGSTAmount"].ToString();
                            IGSTPercentage = sdr["IGSTPercentage"].ToString();

                            UGSTAmount = sdr["UGSTAmount"].ToString();
                            UGSTPercentage = sdr["UGSTPercentage"].ToString();
                            TotalGSTAmount = sdr["TotalGSTAmount"].ToString();

                            IsFastlaneFlow = string.IsNullOrEmpty(sdr["IsFastlaneFlow"].ToString()) ? false : Convert.ToBoolean(sdr["IsFastlaneFlow"]);

                            RegistrationDate = sdr["PropRisks_DateofRegistration"].ToString();
                            IRDA_ProductCode = sdr["IRDA_ProductCode"].ToString();
                            TenureOwnerDriver = (Convert.ToString(sdr["TenureForOwnerDriver"].ToString()) == null || Convert.ToString(sdr["TenureForOwnerDriver"].ToString()) == "") ? "0" : sdr["TenureForOwnerDriver"].ToString();
                            hdnTenureOwnerDriver.Value = TenureOwnerDriver;

                            if (sdr["CustomerGender"] == null)
                            {
                                IsCustomerMale = true;
                            }
                            else if (sdr["CustomerGender"].ToString() == "")
                            {
                                IsCustomerMale = true;
                            }
                            else
                            {
                                IsCustomerMale = sdr["CustomerGender"].ToString() == "MALE" ? true : false;
                            }
                            break;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                
                throw new Exception(ex.Message);
            }
        }
        */

        private void GetResultXMLFromDB(string QuoteNumber, ref bool isNewBusiness, ref string NetPremium, ref string ServiceTax, ref string TotalPremium
            , ref string RequestXML, ref string ResultXML, ref string MarketMovement, ref long CreditScoreId, ref string CreditScoreCustomerName, ref string CreditScoreIDProof, ref string CreditScoreIDProofNumber
            , ref string CGSTAmount, ref string CGSTPercentage, ref string SGSTAmount, ref string SGSTPercentage, ref string IGSTAmount
            , ref string IGSTPercentage, ref string UGSTAmount, ref string UGSTPercentage, ref string TotalGSTAmount, ref bool IsFastlaneFlow
            , ref int SelectedQuoteVersion, ref string RegistrationDate, ref string IRDA_ProductCode, ref bool IsCustomerMale, ref string TenureOwnerDriver
            , ref bool IsIndividual, ref string CustMobileNumber, ref bool IsDepreciationCover, ref bool IsDailyCarAllowance, ref string ddlDailyCarAllowance
            , ref bool IsKeyReplacement, ref string ddlKeyReplacement, ref bool IsLossofPersonalBelongings, ref string ddlLossofPersonalBelongingsSI, ref string drpVDA)
        {
            DataSet ds = new DataSet();
            ds = GetRequestXMLDataset(QuoteNumber, SelectedQuoteVersion.ToString());
            try
            {
                string ResponseXML = string.Empty;

                RequestXML = string.Format("{0}", ds.Tables[0].Rows[0]["RequestXML"].ToString());
                ResultXML = string.Format("{0}", ds.Tables[0].Rows[0]["ResponseXML"]);

                isNewBusiness = ds.Tables[0].Rows[0]["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString() == "New Business" ? true : false;
                NetPremium = ds.Tables[0].Rows[0]["NetPremium"].ToString();
                ServiceTax = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                TotalPremium = ds.Tables[0].Rows[0]["TotalPremium"].ToString();
                MarketMovement = ds.Tables[0].Rows[0]["PropRisks_MarketMovement"].ToString();
                CreditScoreId = Convert.ToInt64(ds.Tables[0].Rows[0]["CreditScoreId"].ToString());

                CGSTAmount = ds.Tables[0].Rows[0]["CGSTAmount"].ToString();
                CGSTPercentage = ds.Tables[0].Rows[0]["CGSTPercentage"].ToString();

                SGSTAmount = ds.Tables[0].Rows[0]["SGSTAmount"].ToString();
                SGSTPercentage = ds.Tables[0].Rows[0]["SGSTPercentage"].ToString();

                IGSTAmount = ds.Tables[0].Rows[0]["IGSTAmount"].ToString();
                IGSTPercentage = ds.Tables[0].Rows[0]["IGSTPercentage"].ToString();

                UGSTAmount = ds.Tables[0].Rows[0]["UGSTAmount"].ToString();
                UGSTPercentage = ds.Tables[0].Rows[0]["UGSTPercentage"].ToString();
                TotalGSTAmount = ds.Tables[0].Rows[0]["TotalGSTAmount"].ToString();

                IsFastlaneFlow = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsFastlaneFlow"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["IsFastlaneFlow"]);

                RegistrationDate = ds.Tables[0].Rows[0]["PropRisks_DateofRegistration"].ToString();
                IRDA_ProductCode = ds.Tables[0].Rows[0]["IRDA_ProductCode"].ToString();
                TenureOwnerDriver = (Convert.ToString(ds.Tables[0].Rows[0]["TenureForOwnerDriver"].ToString()) == null || Convert.ToString(ds.Tables[0].Rows[0]["TenureForOwnerDriver"].ToString()) == "") ? "0" : ds.Tables[0].Rows[0]["TenureForOwnerDriver"].ToString();
                hdnTenureOwnerDriver.Value = TenureOwnerDriver;

                if (ds.Tables[0].Rows[0]["CustomerGender"] == null)
                {
                    IsCustomerMale = true;
                }
                else if (ds.Tables[0].Rows[0]["CustomerGender"].ToString() == "")
                {
                    IsCustomerMale = true;
                }
                else
                {
                    IsCustomerMale = ds.Tables[0].Rows[0]["CustomerGender"].ToString() == "MALE" ? true : false;
                }

                IsIndividual = ds.Tables[0].Rows[0]["PropCustomerDtls_CustomerType"].ToString() == "Individual" ? true : false;
                CustMobileNumber = ds.Tables[0].Rows[0]["CustMobileNumber"].ToString();
                IsDepreciationCover = ds.Tables[0].Rows[0]["PropRisks_DepreciationReimbursement"].ToString() == "True" ? true : false;
                IsDailyCarAllowance = ds.Tables[0].Rows[0]["PropRisks_DailyCarAllowanceChk"].ToString() == "True" ? true : false;
                ddlDailyCarAllowance = ds.Tables[0].Rows[0]["PropRisks_DailyCarAllowanceSIDD"].ToString();
                IsKeyReplacement = ds.Tables[0].Rows[0]["PropRisks_KeyReplacementChk"].ToString() == "True" ? true : false;
                ddlKeyReplacement = ds.Tables[0].Rows[0]["PropRisks_KeyReplacementSIDD"].ToString();
                IsLossofPersonalBelongings = ds.Tables[0].Rows[0]["PropRisks_LossofPersonalBelongingschk"].ToString() == "True" ? true : false;
                ddlLossofPersonalBelongingsSI = ds.Tables[0].Rows[0]["PropRisks_LossofPersonalBelongingsSIDD"].ToString();
                drpVDA = ds.Tables[0].Rows[0]["PropRisks_VoluntaryDeductibleAmount"].ToString();

                CreditScoreCustomerName = ds.Tables[0].Rows[0]["CreditScoreCustomerName"].ToString();
                CreditScoreIDProofNumber = ds.Tables[0].Rows[0]["CreditScoreIDProofNumber"].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string SetResultXMLValuesToPopUpLabel(bool IsRecalculate, string ResultXML, string strQuoteNo, string NetPremium, string ServiceTax, string TotalPremium, bool IsRollover, long CreditScoreId, string CreditScoreCustomerName, string CreditScoreIDProof, string CreditScoreIDProofNumber, string CGSTAmount, string CGSTPercentage, string SGSTAmount, string SGSTPercentage, string IGSTAmount, string IGSTPercentage, string UGSTAmount, string UGSTPercentage, string TotalGSTAmount, string MaxQuoteVersion, string RequestXML)
        {
            string strlblBasicTPPremium = "0.00";
            string strlblOwnDamagePremium = "0.00";
            string strlblConsumableCover = "0.00";
            string strlblDepreciationCover = "0.00";
            string strlblElectronicSI = "0.00";
            string strlblNonElectronicSI = "0.00";
            string strlblExternalBiFuelSI = "0.00";
            string strlblEngineProtect = "0.00";
            string strlblReturnToInvoice = "0.00";
            string strlblRSA = "0.00";
            string strlblLiabilityForBiFuel = "0.00";
            string strlblPAForUnnamedPassengerSI = "0.00";
            string strlblPAForNamedPassengerSI = "0.00";
            string strlblPAToPaidDriverSI = "0.00";
            string strlblPACoverForOwnerDriver = "0.00";
            string strlblLegalLiabilityToPaidDriverNo = "0.00";
            string strlblLLEOPDCC = "0.00";
            string strlblNCB = "0.00";
            string strlblVoluntaryDeduction = "0.00";
            string strlblVoluntaryDeductionforDepWaiver = "0.00";
            string strSystemIDV = "0.00";
            string strFinalIDV = "0.00";
            string strBasicODDeviation = "0";
            string strAddOnDeviation = "0";

            string strlblDailyCarAllowance = "0.00";
            string strlblKeyReplacement = "0.00";
            string strlblTyreCover = "0.00";
            string strlblNCBProtect = "0.00";
            string strlblLossofPersonalBelongings = "0.00";

            lblRateDCA.Text = "0.00";
            lblRateKR.Text = "0.00";
            lblRateTC.Text = "0.00";
            lblRateNCBP.Text = "0.00";
            lblRateLOPB.Text = "0.00";

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            XmlDocument xmlfile = null; DataTable dt = new DataTable();
            string xmlString = "";

            XmlNode SingleNode = null;

            try
            {
                xmlfile = new XmlDocument();
                xmlfile.LoadXml(ResultXML);

                //SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_KotakGrpEmployeeDiscount");
                //lblKGIDiscount.Text = SingleNode.InnerXml;

                //SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_NCPrm");
                //lblNCB.Text = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_MarketMovement"); //CODEMMC
                string strMarketMovement = SingleNode.InnerXml; //CODEMMC

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_BasicODDeviation");
                strBasicODDeviation = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_AddOnDeviation");
                strAddOnDeviation = SingleNode.InnerXml;

                //lblQuoteNumber.Text = strQuoteNo; //COMMENTED 
                //lblQuoteNumber.Text = strQuoteNo + " " + strMarketMovement + " (" + MaxQuoteVersion.ToString() + ")"; //ADDED CODEMMC

                if (strAddOnDeviation.ToString() == "0" && strBasicODDeviation.ToString() == "0")
                {
                    lblQuoteNumber.Text = strQuoteNo + " " + strMarketMovement + " (" + MaxQuoteVersion.ToString() + ")";
                }
                else
                {
                    lblQuoteNumber.Text = strQuoteNo + " " + strMarketMovement.ToString() + "," + strBasicODDeviation.ToString() + "," + strAddOnDeviation.ToString() + "  (" + MaxQuoteVersion.ToString() + ")"; //only append market movement to show in QUOTE PDF CODEMMC
                }

                string CampaignCode = "";
                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Extratxt1");
                if (SingleNode != null)
                {
                    CampaignCode = !string.IsNullOrEmpty(SingleNode.InnerXml) ? SingleNode.InnerXml : "-";
                    lblQuoteNumber.Text = !string.IsNullOrEmpty(SingleNode.InnerXml) ? lblQuoteNumber.Text + " (C)" : lblQuoteNumber.Text;
                }

                lblNetPremium.Text = Convert.ToDecimal(NetPremium).ToIndianCurrencyFormat();

                lblTotalPremium.Text = Convert.ToDecimal(TotalPremium).ToIndianCurrencyFormat();

                //decimal Kerala_19Percent = (Convert.ToDecimal(NetPremium) * 19) / 100; //CR775A - Asked to remove kerala cess - Hasmukh
                //decimal TotalPremiumKerala = Convert.ToDecimal(NetPremium) + Kerala_19Percent; //CR775A - Asked to remove kerala cess - Hasmukh
                //lblTotalPremiumKerala.Text = TotalPremiumKerala.ToIndianCurrencyFormat(); //CR775A - Asked to remove kerala cess - Hasmukh

                decimal PercentServiceTax = 18;
                string GSTEffectiveDate = ConfigurationManager.AppSettings["GSTEffectiveDate"].ToString();
                DateTime dt_GSTEffectiveDate = DateTime.ParseExact(GSTEffectiveDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (true) //if (DateTime.Today > dt_GSTEffectiveDate) //always true bcoz gst is always effective now
                {
                    PercentServiceTax = (Convert.ToDecimal(CGSTPercentage.ToString())
                    + Convert.ToDecimal(SGSTPercentage.ToString())
                    + Convert.ToDecimal(IGSTPercentage.ToString())
                    + Convert.ToDecimal(UGSTPercentage.ToString()));

                    lblServiceTax.Text = Convert.ToDecimal(TotalGSTAmount.ToString()).ToIndianCurrencyFormat();
                    lblGSTOrServiceTax.Text = "GST"; //Final
                    lblPercentServiceTax.Text = PercentServiceTax.ToString();
                }
                else
                {
                    lblServiceTax.Text = Convert.ToDecimal(ServiceTax).ToIndianCurrencyFormat();
                    lblGSTOrServiceTax.Text = "Service Tax";
                    lblPercentServiceTax.Text = "15";
                }

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_IDVFlag");
                lblSystemIDV.Text = Convert.ToDecimal(SingleNode.InnerXml).ToIndianCurrencyFormat();
                strSystemIDV = Convert.ToDecimal(SingleNode.InnerXml).ToString();

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_IDVofthevehicle");
                lblFinalIDV.Text = Convert.ToDecimal(SingleNode.InnerXml).ToIndianCurrencyFormat();
                strFinalIDV = Convert.ToDecimal(SingleNode.InnerXml).ToString();

                string PropRisks_AuthorityLocation = string.Empty;
                GetRTOAuthorityLocationFromXML(RequestXML, out PropRisks_AuthorityLocation);

                //SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_AuthorityLocation");
                lblRTO.Text = PropRisks_AuthorityLocation; // SingleNode.InnerXml;
                txtRN1.Text = PropRisks_AuthorityLocation.Substring(0, 2); //SingleNode.InnerXml.Substring(0, 2);
                txtRN1.Enabled = false;

                string RN2 = string.Empty;
                string AuthorityLocation = PropRisks_AuthorityLocation; // SingleNode.InnerXml; // "DL001";
                if (AuthorityLocation.Length == 5)
                {
                    RN2 = AuthorityLocation.Substring(AuthorityLocation.Length - 3);
                }
                else
                {
                    RN2 = AuthorityLocation.Substring(AuthorityLocation.Length - 2);
                }
                txtRN2.Text = RN2;
                txtRN2.Enabled = false;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_AuthorityLocation");
                string AuthorityLocationName = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_RTOCode");
                lblRTO.Text = "Code: " + SingleNode.InnerXml + " (" + AuthorityLocationName + " - " + lblRTO.Text.Trim() + ")";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropProductName");
                lblCoverType.Text = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropCustomerDtls_CustomerType");
                lblOwnershipType.Text = SingleNode.InnerXml.ToString().ToLower() == "i" ? "Individual" : "Organization";
                if (SingleNode.InnerXml.ToString().ToLower() == "i")
                {
                    rbtIndividual.Checked = true;
                    rbtOrganization.Checked = false;
                    // CR 145 Start Here
                    trCustomerRow9.Visible = true;
                    AccordionPane3.Visible = true;
                    // CR 145 End Here.
                }
                else
                {
                    rbtIndividual.Checked = false;
                    rbtOrganization.Checked = true;
                    // CR 145 Start Here
                    trCustomerRow9.Visible = false;
                    AccordionPane3.Visible = false;
                    // CR 145 End Here.
                }

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_CubicCapacity");
                lblCubicCapacity.Text = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_SeatingCapacity");
                lblSeatingCapacity.Text = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_TypeofPolicyHolder");
                lblPolicyHolderType.Text = SingleNode.InnerXml;

                if (IsRollover)
                {
                    lblDORorDOP.Text = "Ragistration Date";
                    SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_DateofRegistration");
                    lblRagistrationDate.Text = SingleNode.InnerXml;
                    rbtIsRollover.Checked = true;
                }
                else
                {
                    lblDORorDOP.Text = "Purchase Date";
                    SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Dateofpurchase");
                    lblRagistrationDate.Text = SingleNode.InnerXml;
                    rbtIsRollover.Checked = false;
                }
                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Manufacture");
                lblMake.Text = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Model");
                lblModel.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_ModelVariant");
                lblVariant.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_FuelType");
                lblFuelType.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropPolicyEffectivedate_Fromdate_Mandatary");
                lblPolicyStartDate.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_InsuredCreditScore");
                lblCreditScore.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";


                if (CreditScoreId > 0)
                {
                    lblCreditScoreCustomerName.Text = CreditScoreCustomerName;
                    lblCustomerIDProof.Text = CreditScoreIDProof;
                    lblCustomerIDProofNumber.Text = CreditScoreIDProofNumber;
                }
                else
                {
                    lblCreditScoreCustomerName.Text = "-";
                    lblCustomerIDProof.Text = "Pan Number";
                    lblCustomerIDProofNumber.Text = "-";
                }

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_NonElectricalAccessories");
                lblNonElectricalAccessoriesIDV.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "0";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_ElectricalAccessories");
                lblElectricalAccessoriesIDV.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "0";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropIntermediaryDetails_IntermediaryCode");
                if (SingleNode != null)
                {
                    lblIMDCode.Text = !string.IsNullOrEmpty(SingleNode.InnerXml) ? SingleNode.InnerXml : "-";
                }

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_ExtraDD5");
                if (SingleNode != null)
                {
                    lblCustomerGender.Text = !string.IsNullOrEmpty(SingleNode.InnerXml) ? SingleNode.InnerXml : "-";
                }

                if (IsRollover)
                {
                    SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropGeneralProposal_PreviousPolicyDetails_Col/GeneralProposal_PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyEffectiveTo");
                    lblPreviousPolicyExpiryDate.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";
                }

                XmlNodeList nodeList = xmlfile.DocumentElement.SelectNodes("/ServiceResult/GetUserData/PropRisks_Col/Risks");

                decimal PACoverForOwnerDriver = 0;

                foreach (XmlElement item in nodeList)
                {
                    dt.Merge(ConvertXmlNodeListToDataTable(item["PropRisks_CoverDetails_Col"].ChildNodes));

                    foreach (XmlNode node in item["PropRisks_CoverDetails_Col"].ChildNodes)
                    {
                        string PropCoverDetails_CoverGroups = node["PropCoverDetails_CoverGroups"].InnerText;
                        string PropCoverDetails_Premium = node["PropCoverDetails_Premium"].InnerText;
                        PropCoverDetails_CoverGroups = regex.Replace(PropCoverDetails_CoverGroups, " ");
                        string PropCoverDetails_Rate = node["PropCoverDetails_Rate"].InnerText;

                        switch (PropCoverDetails_CoverGroups)
                        {
                            case "Basic TP including TPPD premium":
                                lblBasicTPPremium.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblBasicTPPremium = PropCoverDetails_Premium;
                                break;
                            case "Own Damage":
                                lblOwnDamagePremium.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblOwnDamagePremium = PropCoverDetails_Premium;
                                lblRateBasicOD.Text = PropCoverDetails_Rate;
                                break;
                            case "Consumable Cover":
                                lblConsumableCover.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblConsumableCover = PropCoverDetails_Premium;
                                lblRateCC.Text = PropCoverDetails_Rate;
                                break;
                            case "Depreciation Cover":
                                lblDepreciationCover.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblDepreciationCover = PropCoverDetails_Premium;
                                lblRateDC.Text = PropCoverDetails_Rate;
                                break;
                            //case "Rallies OD":
                            //    lblRalliesOD.Text = PropCoverDetails_Premium;
                            //    break;
                            //case "Geographical Extension OD":
                            //    lblGeoExtension.Text = PropCoverDetails_Premium;
                            //    break;
                            //case "Trailer OD":
                            //    lblTrailerOD.Text = PropCoverDetails_Premium;
                            //    break;
                            case "Electronic Accessories OD":
                                lblElectronicSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblElectronicSI = PropCoverDetails_Premium;
                                break;
                            case "Non Electrical Accessories OD":
                                lblNonElectronicSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblNonElectronicSI = PropCoverDetails_Premium;
                                break;
                            case "CNG Kit OD":
                                lblExternalBiFuelSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblExternalBiFuelSI = PropCoverDetails_Premium;
                                break;
                            case "Engine Protect":
                                lblEngineProtect.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblEngineProtect = PropCoverDetails_Premium;
                                lblRateEP.Text = PropCoverDetails_Rate;
                                break;
                            case "Return to Invoice":
                                lblReturnToInvoice.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblReturnToInvoice = PropCoverDetails_Premium;
                                lblRateRTI.Text = PropCoverDetails_Rate;
                                break;
                            case "Road Side Assistance":
                                lblRSA.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblRSA = PropCoverDetails_Premium;
                                break;
                            //case "Restricted to Own Premises":
                            //    lblRestrictedToOwnPremises.Text = PropCoverDetails_Premium;
                            //    break;
                            case "CNG Kit TP":
                                lblLiabilityForBiFuel.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblLiabilityForBiFuel = PropCoverDetails_Premium;
                                break;
                            case "Unnamed Passengers Personal Accident":
                                lblPAForUnnamedPassengerSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblPAForUnnamedPassengerSI = PropCoverDetails_Premium;
                                break;
                            case "Named Passengers Personal Accident":
                                lblPAForNamedPassengerSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblPAForNamedPassengerSI = PropCoverDetails_Premium;
                                break;
                            case "Paid Driver PA Cover":
                                lblPAToPaidDriverSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblPAToPaidDriverSI = PropCoverDetails_Premium;
                                break;
                            case "Owner Driver":
                                lblPACoverForOwnerDriver.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblPACoverForOwnerDriver = PropCoverDetails_Premium;
                                PACoverForOwnerDriver = Convert.ToDecimal(PropCoverDetails_Premium);
                                break;
                            case "Legal Liability for paid driver cleaner conductor":
                                lblLegalLiabilityToPaidDriverNo.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblLegalLiabilityToPaidDriverNo = PropCoverDetails_Premium;
                                break;
                            case "Legal Liability for Employees other than paid driver conductor cleaner":
                                lblLLEOPDCC.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblLLEOPDCC = PropCoverDetails_Premium;
                                break;
                            //case "Rallies TP":
                            //    lblRalliesTP.Text = PropCoverDetails_Premium;
                            //    break;
                            //case "Liability to soldier sailor airman":
                            //    lblLSSA.Text = PropCoverDetails_Premium;
                            //    break;
                            //case "Geographical Extension TP":
                            //    lblGeoTP.Text = PropCoverDetails_Premium;
                            //    break;
                            //case "Trailer TP":
                            //    lblTrailerTP.Text = PropCoverDetails_Premium;
                            //    break;
                            //case "Restricted to Own Premises -TP":
                            //    lblRestrictedOwnPremisesTP.Text = PropCoverDetails_Premium;
                            //    break;
                            case "Daily Car Allowance":
                                lblDailyCarAllowance.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblDailyCarAllowance = PropCoverDetails_Premium;
                                lblRateDCA.Text = PropCoverDetails_Rate;
                                break;
                            case "Key Replacement":
                                lblKeyReplacement.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblKeyReplacement = PropCoverDetails_Premium;
                                lblRateKR.Text = PropCoverDetails_Rate;
                                break;
                            case "Tyre Cover":
                                lblTyreCover.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblTyreCover = PropCoverDetails_Premium;
                                lblRateTC.Text = PropCoverDetails_Rate;
                                break;
                            case "NCB Protect":
                                lblNCBProtect.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblNCBProtect = PropCoverDetails_Premium;
                                lblRateNCBP.Text = PropCoverDetails_Rate;
                                break;
                            case "Loss of Personal Belongings":
                                lblLossofPersonalBelongings.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblLossofPersonalBelongings = PropCoverDetails_Premium;
                                lblRateLOPB.Text = PropCoverDetails_Rate;
                                break;
                        }
                    }
                }

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_OriginalNCBPercentage");
                lblNCBPercentage.Text = SingleNode.InnerXml + "%";

                XmlNodeList nodeList_LoadingDiscount = xmlfile.DocumentElement.SelectNodes("/ServiceResult/GetUserData/PropLoadingDiscount_Col/LoadingDiscount");
                foreach (XmlElement item in nodeList_LoadingDiscount)
                {
                    if (item["PropLoadingDiscount_Description"].InnerText == "No Claim Bonus")
                    {
                        lblNCB.Text = Convert.ToDecimal(item["PropLoadingDiscount_EndorsementAmount"].InnerText).ToIndianCurrencyFormat();
                        strlblNCB = item["PropLoadingDiscount_EndorsementAmount"].InnerText;
                    }

                    if (item["PropLoadingDiscount_Description"].InnerText == "Voluntary Excess Discount")
                    {
                        lblVoluntaryDeduction.Text = Convert.ToDecimal(item["PropLoadingDiscount_EndorsementAmount"].InnerText).ToIndianCurrencyFormat();
                        strlblVoluntaryDeduction = item["PropLoadingDiscount_EndorsementAmount"].InnerText;
                    }

                    if (item["PropLoadingDiscount_Description"].InnerText == "Voluntary Deductible For Depreciation Cover discount")
                    {
                        lblVoluntaryDeductionforDepWaiver.Text = Convert.ToDecimal(item["PropLoadingDiscount_EndorsementAmount"].InnerText).ToIndianCurrencyFormat();
                        strlblVoluntaryDeductionforDepWaiver = item["PropLoadingDiscount_EndorsementAmount"].InnerText;
                    }
                }

                decimal dcmlTotalPremiumOwnDamageTable = Convert.ToDecimal(strlblOwnDamagePremium) + Convert.ToDecimal(strlblElectronicSI) + Convert.ToDecimal(strlblNonElectronicSI) + Convert.ToDecimal(strlblExternalBiFuelSI) +
                Convert.ToDecimal(strlblEngineProtect) + Convert.ToDecimal(strlblReturnToInvoice) + Convert.ToDecimal(strlblConsumableCover) +
                Convert.ToDecimal(strlblDepreciationCover) + Convert.ToDecimal(strlblRSA)
                + Convert.ToDecimal(strlblDailyCarAllowance)
                + Convert.ToDecimal(strlblTyreCover)
                + Convert.ToDecimal(strlblNCBProtect)
                + Convert.ToDecimal(strlblLossofPersonalBelongings)
                + Convert.ToDecimal(strlblKeyReplacement);

                dcmlTotalPremiumOwnDamageTable = dcmlTotalPremiumOwnDamageTable - Convert.ToDecimal(strlblNCB);
                dcmlTotalPremiumOwnDamageTable = dcmlTotalPremiumOwnDamageTable - Convert.ToDecimal(strlblVoluntaryDeduction);
                dcmlTotalPremiumOwnDamageTable = dcmlTotalPremiumOwnDamageTable - Convert.ToDecimal(strlblVoluntaryDeductionforDepWaiver);

                lblTotalPremiumOwnDamage.Text = dcmlTotalPremiumOwnDamageTable.ToIndianCurrencyFormat();
                decimal TotalPremiumOwnDamage = Convert.ToDecimal(dcmlTotalPremiumOwnDamageTable);

                decimal dcmlTotalPremiumLiability = Convert.ToDecimal(strlblBasicTPPremium) + Convert.ToDecimal(strlblLiabilityForBiFuel) +
                Convert.ToDecimal(strlblPAForUnnamedPassengerSI) + Convert.ToDecimal(strlblPAForNamedPassengerSI) + Convert.ToDecimal(strlblPAToPaidDriverSI) +
                Convert.ToDecimal(strlblPACoverForOwnerDriver) + Convert.ToDecimal(strlblLegalLiabilityToPaidDriverNo) + Convert.ToDecimal(strlblLLEOPDCC);

                lblTotalPremiumLiability.Text = dcmlTotalPremiumLiability.ToIndianCurrencyFormat();

                if (IsRecalculate)
                {
                    SaveResponseValues(strQuoteNo, strSystemIDV, strFinalIDV, NetPremium, ServiceTax, TotalPremium,
                    lblRTO.Text, lblRTO.Text, lblCoverType.Text, lblOwnershipType.Text,
                lblCubicCapacity.Text, lblSeatingCapacity.Text, lblPolicyHolderType.Text, lblRagistrationDate.Text, lblRagistrationDate.Text, lblMake.Text,
                lblModel.Text, lblVariant.Text, strlblBasicTPPremium, strlblOwnDamagePremium, strlblConsumableCover, strlblDepreciationCover,
                strlblElectronicSI, strlblNonElectronicSI, strlblExternalBiFuelSI, strlblEngineProtect, strlblReturnToInvoice, strlblRSA, strlblLiabilityForBiFuel,
                strlblPAForUnnamedPassengerSI, strlblPAForNamedPassengerSI, strlblPAToPaidDriverSI, strlblPACoverForOwnerDriver, strlblLegalLiabilityToPaidDriverNo,
                strlblLLEOPDCC, lblNCBPercentage.Text, strlblNCB, strlblVoluntaryDeduction, strlblVoluntaryDeductionforDepWaiver,
                dcmlTotalPremiumOwnDamageTable.ToString(), dcmlTotalPremiumLiability.ToString(),
                CGSTAmount, CGSTPercentage, SGSTAmount, SGSTPercentage, IGSTAmount, IGSTPercentage, UGSTAmount, UGSTPercentage, TotalGSTAmount, MaxQuoteVersion, lblRateBasicOD.Text, lblRateCC.Text, lblRateDC.Text, lblRateEP.Text, lblRateRTI.Text
                , strlblDailyCarAllowance, strlblTyreCover, strlblNCBProtect, strlblLossofPersonalBelongings, strlblKeyReplacement
                 , lblRateLOPB.Text, lblRateDCA.Text, lblRateKR.Text, lblRateNCBP.Text, lblRateTC.Text, CampaignCode);
                }

                string IRDA_ProductCode = string.Empty;
                string TenureOwnerDriver = string.Empty;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_ExtraDD1");
                if (SingleNode != null)
                {
                    if (!string.IsNullOrEmpty(SingleNode.InnerXml))
                    {
                        //IRDA_ProductCode = SingleNode.InnerXml;

                        if (SingleNode.InnerXml == "Kotak Car Secure (1+1)")
                        {
                            IRDA_ProductCode = "1011";
                        }
                        if (SingleNode.InnerXml == "Kotak Car Secure –3 years (3+3)")
                        {
                            IRDA_ProductCode = "1062";
                        }
                        if (SingleNode.InnerXml == "Kotak Car Secure –Bundled (3+1)")
                        {
                            IRDA_ProductCode = "1063";
                        }

                    }
                    else
                    {
                        throw new Exception("PropRisks_ExtraDD1 is null or empty");
                    }
                }
                else
                {
                    throw new Exception("PropRisks_ExtraDD1 is null or empty");
                }

                hdnIRDAProductCode.Value = IRDA_ProductCode;

                DateTime dt_NewBusinessCondition = DateTime.ParseExact("01/09/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (IsRollover == false && DateTime.ParseExact(lblRagistrationDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= dt_NewBusinessCondition.Date)
                {
                    if (IRDA_ProductCode == "1063")
                    {
                        lblODYearText.Text = "(1 Year)";
                        lblTPYearText.Text = "(3 Years)";
                    }
                    else if (IRDA_ProductCode == "1062")
                    {
                        lblODYearText.Text = "(3 Years)";
                        lblTPYearText.Text = "(3 Years)";
                    }
                }
                else
                {
                    lblODYearText.Text = "";
                    lblTPYearText.Text = "";
                }


                //SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_ExtraDD6");

                //if (SingleNode != null)
                //{
                //    if (!string.IsNullOrEmpty(SingleNode.InnerXml))
                //    {
                //        TenureOwnerDriver = SingleNode.InnerXml;
                //    }
                //    else
                //    {
                //        throw new Exception("PropRisks_ExtraDD6 is null or empty");
                //    }
                //}
                //else
                //{
                //    throw new Exception("PropRisks_ExtraDD6 is null or empty");
                //}



                if (hdnTenureOwnerDriver.Value == "" || hdnTenureOwnerDriver.Value == "0")
                {
                    lblTenureOwnerDriver.Text = "";
                    AccordionPane3.Visible = false;
                }
                else if (hdnTenureOwnerDriver.Value == "1")
                {
                    lblTenureOwnerDriver.Text = "1 Year";
                    AccordionPane3.Visible = true;
                }
                else if (hdnTenureOwnerDriver.Value == "3")
                {
                    lblTenureOwnerDriver.Text = "3 Years";
                    AccordionPane3.Visible = true;
                }

                // CR 354 Start Here

                int PremiumWithPAtoOwnerDriver = 0;
                int PremiumWithoutPAtoOwnerDriver = 0;

                #region Old Logic
                /*
                // Condition 2 where Product is 1+1 or 1+3 and with 1 year PA

                if ((IRDA_ProductCode == "1063" || IRDA_ProductCode == "1011")
                            //        &&
                            //(Convert.ToDouble(PACoverForOwnerDriver) > 0)
                            &&
                    ((hdnTenureOwnerDriver.Value == "1") || (hdnTenureOwnerDriver.Value == "0"))
                    )
                {
                    decimal a = TotalPremiumOwnDamage;
                    decimal b = PACoverForOwnerDriver;
                    decimal GSTAmount = (((a + b) * 18) / 100);

                    PremiumWithPAtoOwnerDriver = (int)Math.Round(((TotalPremiumOwnDamage) + (PACoverForOwnerDriver) + GSTAmount));


                    a = TotalPremiumOwnDamage;
                    GSTAmount = ((a) * 18) / 100;

                    PremiumWithoutPAtoOwnerDriver = (int)Math.Round(((TotalPremiumOwnDamage) + GSTAmount));


                    lblPremiumWithPAtoOwnerDriver.Text = PremiumWithPAtoOwnerDriver.ToString();
                    lblPremiumWithoutPAtoOwnerDriver.Text = PremiumWithoutPAtoOwnerDriver.ToString();

                }
                // Condition 2 End here


                // Condition 3 where Product is 1+1 or 1+3 and with 3 year PA

                if ((IRDA_ProductCode == "1063" || IRDA_ProductCode == "1011")
                                  //              &&
                                  //(Convert.ToInt32(PACoverForOwnerDriver) > 0)
                                  &&
                            (hdnTenureOwnerDriver.Value == "3")
                 )
                {

                    decimal a = TotalPremiumOwnDamage;
                    decimal b = PACoverForOwnerDriver / 3;
                    decimal GSTAmount = ((a + b) * 18) / 100;
                    PremiumWithPAtoOwnerDriver = (int)Math.Round((TotalPremiumOwnDamage + (PACoverForOwnerDriver / 3) + GSTAmount));

                    a = TotalPremiumOwnDamage;
                    GSTAmount = ((a) * 18) / 100;

                    PremiumWithoutPAtoOwnerDriver = (int)Math.Round((TotalPremiumOwnDamage + GSTAmount));

                    lblPremiumWithPAtoOwnerDriver.Text = PremiumWithPAtoOwnerDriver.ToString();
                    lblPremiumWithoutPAtoOwnerDriver.Text = PremiumWithoutPAtoOwnerDriver.ToString();

                }
                // Condition 3 End here

                // Condition 1 if PA Avaialble both line should dispaly
                if (Convert.ToDouble(PACoverForOwnerDriver) > 0)
                {
                    lilblPremiumWithoutPAtoOwnerDriver.Visible = true;
                    lilblPremiumWithPAtoOwnerDriver.Visible = true;
                }
                else // only Premium without PA line display
                {
                    lilblPremiumWithPAtoOwnerDriver.Visible = false;
                    lilblPremiumWithoutPAtoOwnerDriver.Visible = true;
                }
                // Condition 1 End here

                // Condition 4 New lines not to be displayed for 3+3
                if (IRDA_ProductCode == "1062")
                {
                    lilblPremiumWithoutPAtoOwnerDriver.Visible = false;
                    lilblPremiumWithPAtoOwnerDriver.Visible = false;
                }
                // Condition 4 End here 
                */
                #endregion

                #region New Logic
                decimal GSTAmount = ((TotalPremiumOwnDamage) * 18) / 100;
                PremiumWithoutPAtoOwnerDriver = (int)Math.Round(((TotalPremiumOwnDamage) + GSTAmount));
                lblPremiumWithoutPAtoOwnerDriver.Text = PremiumWithoutPAtoOwnerDriver.ToString();

                if (IRDA_ProductCode == "1062")
                {
                    lilblPremiumWithoutPAtoOwnerDriver.Visible = false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SetResultXMLValuesToPopUpLabel");

                lblstatus.Text = ex.StackTrace;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            finally
            {
            }
            return xmlString;
        }

        private string GetXmlUsingSingleNode() //Useful for single of node updation
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("E:\\Hasmukh\\TEST.xml");
            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryCode");
            node.Attributes["Value"].Value = "1000000000";
            string s = xmlDoc.InnerXml;
            return "";
        }

        private string GetXmlUsingNodeList() //Useful for list of node updation
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("E:\\Hasmukh\\TEST.xml");
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryCode");

            foreach (XmlNode node in nodeList)
            {
                node.Attributes["Value"].Value = "1000000000";
            }

            string s = xmlDoc.InnerXml;
            return "";
        }

        public string Auth(string UserId, string Passwd)
        {
            //ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();

            //proxy.Endpoint.Behaviors.Add(new CustomBehavior());

            //ServiceReference1.ServiceResult objServiceResult = new ServiceReference1.ServiceResult();

            //string AuthKey = proxy.Authenticate(UserId, Passwd);
            //proxy.Close();

            //return AuthKey;
            return "";
        }

        private void ReCalculatePremium(string ExistingRequestXML, string ExistingQuoteNumber, string MarketMovement, long CreditScoreId, string CreditScoreCustomerName
            , string CreditScoreIDProof, string CreditScoreIDProofNumber, bool IsFastlaneFlow, int SelectedQuoteVersionForRecalculate, bool IsNewBusiness
            , string RegistrationDate, string IRDA_ProductCode, string EditedPolicyStartDate, string TenureOwnerDriver
             , bool IsIndividual, string CustMobileNumber, bool IsDepreciationCover, bool IsDailyCarAllowance, string ddlDailyCarAllowance
            , bool IsKeyReplacement, string ddlKeyReplacement, bool IsLossofPersonalBelongings, string ddlLossofPersonalBelongingsSI, string drpVDA)
        {
            bool IsSuccess = false;
            string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
            string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();
            lblWarningText.Text = "";
            hdnIsWarningPresent.Value = "0";

            lblWarningTextForProposalSuccessPopUp.Text = "";
            trOptionButtonRowForLinkSendingOptions.Visible = true;
            trEmailIdAndRemarksRow.Visible = true;
            modalProposalSuccessFooter.Visible = false;

            //string strUserId = "GC0014"; string strPassword = "cmc123"; //for uat
            //string strUserId = "GI0033"; string strPassword = "May@2016"; //for prod
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
                objServiceResult.UserData.AuthenticateKey = Auth(strUserId, strPassword);
                bool IsRollover = false;
                string PolicyStartDate = string.Empty;
                string PolicyEndDate = string.Empty;
                string ProposalDate = string.Empty;

                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestXMLForRecalculate(ExistingRequestXML, IRDA_ProductCode, EditedPolicyStartDate, ref IsRollover, out PolicyStartDate, out PolicyEndDate, out ProposalDate));
                objServiceResult.UserData.IsInternalRiskGrid = true;

                objServiceResult.UserData.ErrorText = "";

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager

                proxy.CalculatePremium(strUserId, strPassword, ref objServiceResult);
                proxy.Close();

                IsSuccess = string.IsNullOrEmpty(objServiceResult.UserData.ErrorText) ? true : false;
                IsSuccess = objServiceResult.UserData.UserResultXml == null ? false : true;

                if (IsRollover)
                {
                    txtRN1.Enabled = false;
                    txtRN2.Enabled = false;
                }
                else
                {
                    txtRN1.Enabled = true;
                    txtRN2.Enabled = true;
                }

                string strQuoteNo = ExistingQuoteNumber; // string.Empty;
                int MaxQuoteVersion = 0;
                SaveRequestResponse(MarketMovement: MarketMovement, strRequestXml: objServiceResult.UserData.ConsumeProposalXML.ToString(), strResponseXml: objServiceResult.UserData.UserResultXml, IsSuccess: IsSuccess, strErrorMessage: objServiceResult.UserData.ErrorText, strQuoteNo: strQuoteNo, MaxQuoteVersion: ref MaxQuoteVersion);
                //SaveRequestValues(strQuoteNo, IsSuccess, objServiceResult.UserData.ErrorText);
                SaveRequestValues_ForRecalculate(ExistingQuoteNumber, strQuoteNo, IsSuccess, objServiceResult.UserData.ErrorText, CreditScoreId, SelectedQuoteVersionForRecalculate, MaxQuoteVersion, PolicyStartDate, PolicyEndDate, ProposalDate);

                hdnMaxQuoteVersion.Value = MaxQuoteVersion.ToString();

                if (!string.IsNullOrEmpty(objServiceResult.UserData.ErrorText))
                {
                    hdnCreditScoreId.Value = "0";
                    hdnIsFastlaneFlow.Value = "0";

                    lblstatus.Text = objServiceResult.UserData.ErrorText;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                else
                {
                    if (!string.IsNullOrEmpty(objServiceResult.UserData.WarningText))
                    {
                        string WarningText = TruncateWarningText(objServiceResult.UserData.WarningText.Trim());
                        WarningText = WarningText.Replace("|", " ");
                        WarningText = WarningText.Replace("Risk Score:", "|Risk Score:").Replace("Risk Rate:", "|Risk Rate:");
                        string[] Arr = WarningText.Split('|');
                        Regex regex = new Regex("[0-9]");
                        WarningText = regex.Replace(WarningText, "");

                        while (WarningText.Contains("Proposal will be created under Campaign"))
                        {
                            WarningText = WarningText.Replace("Proposal will be created under Campaign", "");
                        }

                        if (WarningText.Trim().Length > 15) //expecting atleast 10 char warning
                        {
                            if (WarningText.Contains("The Proposal has been referred for Credit Score discount"))
                            {
                                hdnIsWarningPresent.Value = "0"; //IF CREDIT SCORE WARNING THEN ALLOW SENDING PAYMENT LINK OR REVIEW CONFIRM LINK - (THIS REQUIRMENT GIVEN AND IMPLEMENTED ON 01-SEP-2017)
                                lblWarningText.Text = "Status: This Proposal will be referred for Credit Score Discount.";
                            }
                            else if (WarningText.ToLower().Contains("proposal will be created under campaign"))
                            {
                                hdnIsWarningPresent.Value = "0"; //IF CREDIT SCORE WARNING THEN ALLOW SENDING PAYMENT LINK OR REVIEW CONFIRM LINK - (THIS REQUIRMENT GIVEN AND IMPLEMENTED ON 01-SEP-2017)
                                lblWarningText.Text = "Status: " + WarningText;
                            }
                            else
                            {
                                if (Arr != null)
                                {
                                    if (Arr.Length == 3)
                                    {
                                        string RiskScore = Arr[1];
                                        string RiskRate = Arr[2];
                                        string[] RiskRateArr = Arr[2].Split(' ');
                                        if (RiskRateArr != null)
                                        {
                                            if (RiskRateArr.Length > 0)
                                            {
                                                RiskRate = RiskRateArr[0] + " " +  RiskRateArr[1];
                                                WarningText = RiskScore + "" + RiskRate;
                                            }
                                        }
                                    }
                                }

                                hdnIsWarningPresent.Value = "1"; //IF ANY OTHER WARNING PRESENT THEN DO NOT SENT THE PAYMENT OR REVIEW CONFIRM LINK
                                lblWarningText.Text = "Status: " + WarningText;
                            }
                        }
                    }
                    hdnCreditScoreId.Value = CreditScoreId.ToString();

                    hdnIsFastlaneFlow.Value = IsFastlaneFlow ? "1" : "0";

                    lblNetPremium.Text = Convert.ToDecimal(objServiceResult.UserData.NetPremium.ToString()).ToIndianCurrencyFormat();

                    lblTotalPremium.Text = Convert.ToDecimal(objServiceResult.UserData.TotalPremium.ToString()).ToIndianCurrencyFormat();

                    //decimal Kerala_19Percent = (Convert.ToDecimal(objServiceResult.UserData.NetPremium.ToString()) * 19) / 100; //CR775A - Asked to remove kerala cess - Hasmukh
                    //decimal TotalPremiumKerala = Convert.ToDecimal(objServiceResult.UserData.NetPremium.ToString()) + Kerala_19Percent; //CR775A - Asked to remove kerala cess - Hasmukh
                    //lblTotalPremiumKerala.Text = TotalPremiumKerala.ToIndianCurrencyFormat(); //CR775A - Asked to remove kerala cess - Hasmukh

                    decimal PercentServiceTax = 18;
                    string GSTEffectiveDate = ConfigurationManager.AppSettings["GSTEffectiveDate"].ToString();
                    DateTime dt_GSTEffectiveDate = DateTime.ParseExact(GSTEffectiveDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (true) //if (DateTime.Today > dt_GSTEffectiveDate) //always true bcoz gst is always effective now
                    {
                        PercentServiceTax = (Convert.ToDecimal(objServiceResult.UserData.CGSTPercentage.ToString())
                        + Convert.ToDecimal(objServiceResult.UserData.SGSTPercentage.ToString())
                        + Convert.ToDecimal(objServiceResult.UserData.IGSTPercentage.ToString())
                        + Convert.ToDecimal(objServiceResult.UserData.UGSTPercentage.ToString()));

                        lblServiceTax.Text = Convert.ToDecimal(objServiceResult.UserData.TotalGSTAmount.ToString()).ToIndianCurrencyFormat();
                        lblGSTOrServiceTax.Text = "GST";
                        lblPercentServiceTax.Text = PercentServiceTax.ToString();
                    }
                    else
                    {
                        lblServiceTax.Text = Convert.ToDecimal(objServiceResult.UserData.ServiceTax.ToString()).ToIndianCurrencyFormat();
                        lblGSTOrServiceTax.Text = "Service Tax";
                        lblPercentServiceTax.Text = "15";
                    }

                    string ResultXML = objServiceResult.UserData.UserResultXml;

                    //string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "ResultXML.xml";
                    //File.WriteAllText(strXmlPath, String.Empty);
                    //File.WriteAllText(strXmlPath, ResultXML);

                    SetResultXMLValuesToPopUpLabel(IsRecalculate: true
                        , ResultXML: ResultXML
                        , strQuoteNo: strQuoteNo, NetPremium: objServiceResult.UserData.NetPremium.ToString()
                        , ServiceTax: objServiceResult.UserData.ServiceTax.ToString()
                        , TotalPremium: objServiceResult.UserData.TotalPremium.ToString()
                        , IsRollover: IsRollover
                        , CreditScoreId: CreditScoreId
                        , CreditScoreCustomerName: CreditScoreCustomerName
                        , CreditScoreIDProof: CreditScoreIDProof
                        , CreditScoreIDProofNumber: CreditScoreIDProofNumber
                        , CGSTAmount: objServiceResult.UserData.CGSTAmount.ToString()
                        , CGSTPercentage: objServiceResult.UserData.CGSTPercentage.ToString()
                        , SGSTAmount: objServiceResult.UserData.SGSTAmount.ToString()
                        , SGSTPercentage: objServiceResult.UserData.SGSTPercentage.ToString()
                        , IGSTAmount: objServiceResult.UserData.IGSTAmount.ToString()
                        , IGSTPercentage: objServiceResult.UserData.IGSTPercentage.ToString()
                        , UGSTAmount: objServiceResult.UserData.UGSTAmount.ToString()
                        , UGSTPercentage: objServiceResult.UserData.UGSTPercentage.ToString()
                        , TotalGSTAmount: objServiceResult.UserData.TotalGSTAmount.ToString()
                        , MaxQuoteVersion: MaxQuoteVersion.ToString()
                        , RequestXML: ExistingRequestXML);

                    DownloadPDF(true, MaxQuoteVersion.ToString(), IsNewBusiness, RegistrationDate, IRDA_ProductCode, false, TenureOwnerDriver);

                    CreateQuotePDF objCreateQuotePDF = new CreateQuotePDF();
                    QuotePDFParams objQuotePDFParams = new QuotePDFParams();
                    objQuotePDFParams.ProductCode = 3121;
                    objQuotePDFParams.QuoteNumber = strQuoteNo;
                    objQuotePDFParams.chkIsGetCreditScore = true; // chkIsGetCreditScore.Checked;
                    objQuotePDFParams.txtFirstName = txtFirstName.Text.Trim();
                    objQuotePDFParams.txtLastName = txtLastName.Text.Trim();
                    objQuotePDFParams.CustomerName = CreditScoreCustomerName;
                    objQuotePDFParams.CreditScoreCustomerName = CreditScoreCustomerName;
                    objQuotePDFParams.drpDrivingLicenseNumberOrAadhaarNumber = CreditScoreIDProof;
                    objQuotePDFParams.txtDrivingLicenseNumberOrAadhaarNumber = CreditScoreIDProofNumber;
                    objQuotePDFParams.txtRTOAuthorityCode = lblRTO.Text; // txtRTOAuthorityCode.Text.Trim();
                    objQuotePDFParams.rbbtRollOver = IsRollover;
                    objQuotePDFParams.rbctIndividual = IsIndividual;
                    objQuotePDFParams.txtMobileNumber = CustMobileNumber;
                    objQuotePDFParams.drpTenureOwnerDriver = TenureOwnerDriver;
                    objQuotePDFParams.chkDepreciationCover = IsDepreciationCover; // chkDepreciationCover.Checked;
                    objQuotePDFParams.chkDailyCarAllowance = IsDailyCarAllowance; // chkDailyCarAllowance.Checked;
                    objQuotePDFParams.ddlDailyCarAllowance =  ddlDailyCarAllowance;
                    objQuotePDFParams.chkKeyReplacement = IsKeyReplacement; // chkKeyReplacement.Checked;
                    objQuotePDFParams.ddlKeyReplacement = ddlKeyReplacement;
                    objQuotePDFParams.chkLossofPersonalBelongings = IsLossofPersonalBelongings; // chkLossofPersonalBelongings.Checked;
                    objQuotePDFParams.ddlLossofPersonalBelongingsSI = ddlLossofPersonalBelongingsSI;
                    objQuotePDFParams.rbbtNewBusiness = IsNewBusiness;
                    objQuotePDFParams.txtDateOfRegistration = RegistrationDate;
                    objQuotePDFParams.drpProductType = IRDA_ProductCode; // drpProductType.SelectedValue;

                    objQuotePDFParams.NetPremium = Convert.ToDecimal(objServiceResult.UserData.NetPremium).ToIndianCurrencyFormat();
                    objQuotePDFParams.ServiceTax = objServiceResult.UserData.ServiceTax.ToString();
                    objQuotePDFParams.TotalPremium = Convert.ToDecimal(objServiceResult.UserData.TotalPremium).ToIndianCurrencyFormat();
                    objQuotePDFParams.CGSTAmount = objServiceResult.UserData.CGSTAmount.ToString();
                    objQuotePDFParams.CGSTPercentage = objServiceResult.UserData.CGSTPercentage.ToString();
                    objQuotePDFParams.SGSTAmount = objServiceResult.UserData.SGSTAmount.ToString();
                    objQuotePDFParams.SGSTPercentage = objServiceResult.UserData.SGSTPercentage.ToString();
                    objQuotePDFParams.IGSTAmount = objServiceResult.UserData.IGSTAmount.ToString();
                    objQuotePDFParams.IGSTPercentage = objServiceResult.UserData.IGSTPercentage.ToString();
                    objQuotePDFParams.UGSTAmount = objServiceResult.UserData.UGSTAmount.ToString();
                    objQuotePDFParams.UGSTPercentage = objServiceResult.UserData.UGSTPercentage.ToString();
                    objQuotePDFParams.TotalGSTAmount = Convert.ToDecimal(objServiceResult.UserData.TotalGSTAmount).ToIndianCurrencyFormat();
                    objQuotePDFParams.MaxQuoteVersion = MaxQuoteVersion;
                    objQuotePDFParams.PercentServiceTax = PercentServiceTax.ToString();
                    objQuotePDFParams.TotalPremiumKerala = "0"; // lblTotalPremiumKerala.Text; //CR775A - Asked to remove kerala cess - Hasmukh
                    objQuotePDFParams.drpVDA = drpVDA; //Voluntary Deductible Amount
                    objQuotePDFParams.VDEPCoverAmount = ConfigurationManager.AppSettings["VDEPCoverAmount"].ToString();
                    objQuotePDFParams.CustomerId = lblPDFCustomerId.Text; // = CustomerId.ToString();
                    objQuotePDFParams.ProposalNumber = lblPDFProposalNumber.Text; // = ProposalNumber;
                    objCreateQuotePDF.SaveQuotePDF(strQuoteNo, objServiceResult.UserData.ConsumeProposalXML.ToString(), ResultXML, objQuotePDFParams);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalViewPremium();", true);

                    ViewState["objQuoteDetails"] = null; //setting it null here so that gridiview can fetch data from database intead of viewstate itself
                    QuoteGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblstatus.Text = ex.Message + "------------------" + ex.StackTrace;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }

        }

        private void SaveRequestResponse_ForSaveProposal(string strRequestXml, string strResponseXml, bool IsSuccess, string strErrorMessage, string strQuoteNo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_PROPOSAL_REQUEST_RESPONSE";
                        cmd.Parameters.AddWithValue("@QuoteNo", strQuoteNo);
                        cmd.Parameters.AddWithValue("@RequestXML", strRequestXml);
                        cmd.Parameters.AddWithValue("@ResponseXML", strResponseXml);
                        cmd.Parameters.AddWithValue("@IsSuccess", IsSuccess);
                        cmd.Parameters.AddWithValue("@ErrorMessage", strErrorMessage);
                        cmd.Parameters.AddWithValue("@vUserLoginId", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.Parameters.AddWithValue("@MaxQuoteVersion", Convert.ToInt16(hdnMaxQuoteVersion.Value));

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveRequestResponse Method");
                throw new Exception(ex.Message);
            }
        }

        private void SaveRequestResponse(string MarketMovement, string strRequestXml, string strResponseXml, bool IsSuccess, string strErrorMessage, string strQuoteNo, ref int MaxQuoteVersion)
        {
            try
            {
                string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();
                //Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
                //SqlConnection _con = new SqlConnection(dbCOMMON.ConnectionString);
                //_con.Open();
                //SqlTransaction _tran = _con.BeginTransaction();
                //strQuoteNo = wsGen.fn_Gen_Cert_No(DateTime.Now.ToString("ddMMyy"), Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
                ////strQuoteNo = strQuoteNo + " " + MarketMovement; //CODEMMC // COMMENTING //DETACH MARKET MOVEMENT FROM QUOTENUMBER
                //_tran.Commit();
                //_con.Close();

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_CALCULATE_PREMIUM_REQUEST_RESPONSE";
                        cmd.Parameters.AddWithValue("@QuoteNo", strQuoteNo);
                        cmd.Parameters.AddWithValue("@RequestXML", strRequestXml);
                        cmd.Parameters.AddWithValue("@ResponseXML", strResponseXml);
                        cmd.Parameters.AddWithValue("@IsSuccess", IsSuccess);
                        cmd.Parameters.AddWithValue("@ErrorMessage", strErrorMessage);
                        cmd.Parameters.AddWithValue("@vUserLoginId", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.Parameters.AddWithValue("@PDFPath", IsSuccess ? KotakQuotesPDFFiles + strQuoteNo + ".pdf" : null);

                        cmd.Parameters.Add("@MaxQuoteVersion", SqlDbType.Int);
                        cmd.Parameters["@MaxQuoteVersion"].Direction = ParameterDirection.Output;

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MaxQuoteVersion = Convert.ToInt32(cmd.Parameters["@MaxQuoteVersion"].Value);

                        conn.Close();
                    }
                }

                // if requestxml or responsxml copied from database then it will not copy full string, so to analyse xml, first retrieve and save into a xml file
                //using (SqlConnection conn = new SqlConnection())
                //{
                //    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                //    using (SqlCommand cmd = new SqlCommand())
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.CommandText = "PROC_GET_CALCULATE_PREMIUM_REQUEST_RESPONSE";
                //        cmd.Connection = conn;
                //        conn.Open();
                //        using (SqlDataReader sdr = cmd.ExecuteReader())
                //        {
                //            while (sdr.Read())
                //            {
                //                string S = string.Format("{0}", sdr["RequestXML"]);
                //            }
                //        }
                //        conn.Close();
                //    }
                //}
            }


            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveRequestResponse Method");
                throw new Exception(ex.Message);
            }
        }

        private void SaveRequestValues_ForRecalculate(string ExistingQuoteNumber, string strQuoteNo, bool IsSuccess, string strErrorMessage, long CreditScoreId, int SelectedQuoteVersionForRecalculate, int MaxQuoteVersion, string PolicyStartDate, string PolicyEndDate, string ProposalDate)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_MOTOR_PREMIUM_CALC_REQUEST_FOR_RECALCULATE"; //copy all the values from existing quote number
                        cmd.Parameters.AddWithValue("@ExistingQuoteNumber", ExistingQuoteNumber);
                        cmd.Parameters.AddWithValue("@QuoteNumber", strQuoteNo);
                        cmd.Parameters.AddWithValue("@IsSuccess", IsSuccess);
                        cmd.Parameters.AddWithValue("@ErrorMessage", strErrorMessage);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.Parameters.AddWithValue("@CreditScoreId", CreditScoreId);
                        cmd.Parameters.AddWithValue("@RequestExecutedOnServer", CommonExtensions.GetLocalIPAddress());
                        cmd.Parameters.AddWithValue("@SelectedQuoteVersionForRecalculate", SelectedQuoteVersionForRecalculate);
                        cmd.Parameters.AddWithValue("@MaxQuoteVersion", MaxQuoteVersion);

                        cmd.Parameters.AddWithValue("@PolicyStartDate", PolicyStartDate);
                        cmd.Parameters.AddWithValue("@PolicyEndDate", PolicyEndDate);
                        cmd.Parameters.AddWithValue("@ProposalDate", ProposalDate);

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveRequestValues_ForRecalculate Method");
                throw new Exception(ex.Message);
            }


        }

        private void SaveResponseValues(string strQuoteNo, string SystemIDV,
        string FinalIDV, string NetPremium, string ServiceTax, string TotalPremium,
        string AuthorityLocation, string RTOCode, string ProductName, string CustomerType, string CubicCapacity, string SeatingCapacity, string TypeofPolicyHolder,
        string DateofRegistration, string Dateofpurchase, string Manufacture, string Model, string ModelVariant, string BasicTPIncludingTPPDPremium, string OwnDamage,
        string ConsumableCover, string DepreciationCover, string ElectronicAccessoriesOD, string NonElectricalAccessoriesOD, string CNGKitOD, string EngineProtect,
        string ReturnToInvoice, string RoadSideAssistance, string CNGKitTP, string UnnamedPassengersPersonalAccident, string NamedPassengersPersonalAccident,
        string PaidDriverPACover, string OwnerDriver, string LegalLiabilityForPaidDriverCleanerConductor, string LegalLiabilityForEmployeesOtherThanPaidDriverConductorCleaner,
        string NCBPercentage, string NoClaimBonus, string VoluntaryExcessDiscount, string VoluntaryDeductibleForDepreciationCoverDiscount, string TotalPremiumOwnDamage,
        string TotalPremiumLiability, string CGSTAmount, string CGSTPercentage, string SGSTAmount, string SGSTPercentage, string IGSTAmount,
        string IGSTPercentage, string UGSTAmount, string UGSTPercentage, string TotalGSTAmount, string MaxQuoteVersion,
        string Rate_BasicOD, string Rate_CC, string Rate_DC, string Rate_EP, string Rate_RTI
            , string DailyCarAllowance, string TyreCover, string NCBProtect, string LossofPersonalBelongings, string KeyReplacement
                 , string Rate_LOPB, string Rate_DCA, string Rate_KR, string Rate_NCBP, string Rate_TC, string CampaignCode)
        {

            //SAVE
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_MOTOR_PREMIUM_CALC_RESPONSE";
                        cmd.Parameters.AddWithValue("@QuoteNumber", strQuoteNo);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());

                        cmd.Parameters.AddWithValue("@SystemIDV", SystemIDV);
                        cmd.Parameters.AddWithValue("@FinalIDV", FinalIDV);

                        cmd.Parameters.AddWithValue("@NetPremium", NetPremium);
                        cmd.Parameters.AddWithValue("@ServiceTax", ServiceTax);
                        cmd.Parameters.AddWithValue("@TotalPremium", TotalPremium);

                        cmd.Parameters.AddWithValue("@AuthorityLocation", AuthorityLocation);
                        cmd.Parameters.AddWithValue("@RTOCode", RTOCode);
                        cmd.Parameters.AddWithValue("@ProductName", ProductName);
                        cmd.Parameters.AddWithValue("@CustomerType", CustomerType);
                        cmd.Parameters.AddWithValue("@CubicCapacity", CubicCapacity);
                        cmd.Parameters.AddWithValue("@SeatingCapacity", SeatingCapacity);
                        cmd.Parameters.AddWithValue("@TypeofPolicyHolder", TypeofPolicyHolder);
                        cmd.Parameters.AddWithValue("@DateofRegistration", DateofRegistration);
                        cmd.Parameters.AddWithValue("@Dateofpurchase", Dateofpurchase);
                        cmd.Parameters.AddWithValue("@Manufacture", Manufacture);
                        cmd.Parameters.AddWithValue("@Model", Model);
                        cmd.Parameters.AddWithValue("@ModelVariant", ModelVariant);
                        cmd.Parameters.AddWithValue("@BasicTPIncludingTPPDPremium", BasicTPIncludingTPPDPremium);
                        cmd.Parameters.AddWithValue("@OwnDamage", OwnDamage);
                        cmd.Parameters.AddWithValue("@ConsumableCover", ConsumableCover);
                        cmd.Parameters.AddWithValue("@DepreciationCover", DepreciationCover);
                        cmd.Parameters.AddWithValue("@ElectronicAccessoriesOD", ElectronicAccessoriesOD);
                        cmd.Parameters.AddWithValue("@NonElectricalAccessoriesOD", NonElectricalAccessoriesOD);
                        cmd.Parameters.AddWithValue("@CNGKitOD", CNGKitOD);
                        cmd.Parameters.AddWithValue("@EngineProtect", EngineProtect);
                        cmd.Parameters.AddWithValue("@ReturnToInvoice", ReturnToInvoice);
                        cmd.Parameters.AddWithValue("@RoadSideAssistance", RoadSideAssistance);
                        cmd.Parameters.AddWithValue("@CNGKitTP", CNGKitTP);
                        cmd.Parameters.AddWithValue("@UnnamedPassengersPersonalAccident", UnnamedPassengersPersonalAccident);
                        cmd.Parameters.AddWithValue("@NamedPassengersPersonalAccident", NamedPassengersPersonalAccident);
                        cmd.Parameters.AddWithValue("@PaidDriverPACover", PaidDriverPACover);
                        cmd.Parameters.AddWithValue("@OwnerDriver", OwnerDriver);
                        cmd.Parameters.AddWithValue("@LegalLiabilityForPaidDriverCleanerConductor", LegalLiabilityForPaidDriverCleanerConductor);
                        cmd.Parameters.AddWithValue("@LegalLiabilityForEmployeesOtherThanPaidDriverConductorCleaner", LegalLiabilityForEmployeesOtherThanPaidDriverConductorCleaner);
                        cmd.Parameters.AddWithValue("@NCBPercentage", NCBPercentage);
                        cmd.Parameters.AddWithValue("@NoClaimBonus", NoClaimBonus);
                        cmd.Parameters.AddWithValue("@VoluntaryExcessDiscount", VoluntaryExcessDiscount);
                        cmd.Parameters.AddWithValue("@VoluntaryDeductibleForDepreciationCoverDiscount", VoluntaryDeductibleForDepreciationCoverDiscount);
                        cmd.Parameters.AddWithValue("@TotalPremiumOwnDamage", TotalPremiumOwnDamage);
                        cmd.Parameters.AddWithValue("@TotalPremiumLiability", TotalPremiumLiability);

                        cmd.Parameters.AddWithValue("@CGSTAmount", CGSTAmount);
                        cmd.Parameters.AddWithValue("@CGSTPercentage", CGSTPercentage);
                        cmd.Parameters.AddWithValue("@SGSTAmount", SGSTAmount);
                        cmd.Parameters.AddWithValue("@SGSTPercentage", SGSTPercentage);
                        cmd.Parameters.AddWithValue("@IGSTAmount", IGSTAmount);
                        cmd.Parameters.AddWithValue("@IGSTPercentage", IGSTPercentage);
                        cmd.Parameters.AddWithValue("@UGSTAmount", UGSTAmount);
                        cmd.Parameters.AddWithValue("@UGSTPercentage", UGSTPercentage);
                        cmd.Parameters.AddWithValue("@TotalGSTAmount", TotalGSTAmount);
                        cmd.Parameters.AddWithValue("@QuoteVersion", MaxQuoteVersion);
                        cmd.Parameters.AddWithValue("@RateBasicOD", Rate_BasicOD);
                        cmd.Parameters.AddWithValue("@RateCC", Rate_CC);
                        cmd.Parameters.AddWithValue("@RateDC", Rate_DC);
                        cmd.Parameters.AddWithValue("@RateEP", Rate_EP);
                        cmd.Parameters.AddWithValue("@RateRTI", Rate_RTI);

                        cmd.Parameters.AddWithValue("@RateLOPB", Rate_LOPB);
                        cmd.Parameters.AddWithValue("@RateDCA", Rate_DCA);
                        cmd.Parameters.AddWithValue("@RateKR", Rate_KR);
                        cmd.Parameters.AddWithValue("@RateNCBP", Rate_NCBP);
                        cmd.Parameters.AddWithValue("@RateTC", Rate_TC);

                        cmd.Parameters.AddWithValue("@DailyCarAllowance", DailyCarAllowance);
                        cmd.Parameters.AddWithValue("@TyreCover", TyreCover);
                        cmd.Parameters.AddWithValue("@NCBProtect", NCBProtect);
                        cmd.Parameters.AddWithValue("@LossofPersonalBelongings", LossofPersonalBelongings);
                        cmd.Parameters.AddWithValue("@KeyReplacement", KeyReplacement);

                        cmd.Parameters.AddWithValue("@CampaignCode", CampaignCode); //CR455

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveResponseValues Method");
            }


        }

        private bool ValidationSaveProposal(out string strErrorMsg)
        {
            //string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime dtDOB = DateTime.ParseExact(txtDateofBirth.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //string strDateOfBirth = dtDOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            bool IsError = false;
            strErrorMsg = "";
           // int IsInFraud_RegistrationNumber = 0;
            //SandeepCR919
           // CheckFraudulentRegistrationNumber(txtRN1.Text.Trim() + "" + txtRN2.Text.Trim() + "" + txtRN3.Text.Trim() + "" + txtRN4.Text.Trim(),  out IsInFraud_RegistrationNumber);

            if (rbtExistingCustomer.Checked && string.IsNullOrEmpty(txtCustomerId.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer ID";
            }
            else if (rbtExistingCustomer.Checked && string.IsNullOrEmpty(txtCustomerFullName.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Full Name";
            }
            else if (!Regex.IsMatch(txtCustomerId.Text.Trim(), "^[0-9]*$") && rbtExistingCustomer.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Numeric Customer Id";
            }
            else if (string.IsNullOrEmpty(txtFirstName.Text.Trim()) && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer First Name";
            }
            else if (string.IsNullOrEmpty(txtLastName.Text.Trim()) && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Last Name";
            }
            //else if (string.IsNullOrEmpty(txtMiddleName.Text.Trim()) && rbtNewCustomer.Checked && rbtIndividual.Checked)
            //{
            //    IsError = true;
            //    strErrorMsg = "Please Enter Customer Middle Name";
            //}
            else if (string.IsNullOrEmpty(txtDateofBirth.Text.Trim()) && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Date of Birth";
            }
            else if (string.IsNullOrEmpty(txtEmailAddress.Text.Trim()) && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Email Address";
            }
            else if (!Regex.IsMatch(txtEmailAddress.Text.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase) && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Valid Email Address";
            }
            else if (string.IsNullOrEmpty(txtMobileNumber.Text.Trim()) && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Mobile Number";
            }
            else if (txtMobileNumber.Text.Trim().Length != 10 && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter 10 Digit Mobile Number";
            }
            else if (!Regex.IsMatch(txtMobileNumber.Text.Trim(), "^[0-9]*$") && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Valid Mobile Number";
            }
            else if (string.IsNullOrEmpty(txtPanNumber.Text.Trim()) && rbtNewCustomer.Checked && rbtIndividual.Checked && Convert.ToInt64(hdnCreditScoreId.Value) > 0)
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Pan Number";
            }
            else if (string.IsNullOrEmpty(txtPinCode.Text.Trim()) && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Pincode";
            }
            else if (string.IsNullOrEmpty(txtAddressLine1.Text.Trim()) && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Customer Address Line 1";
            }
            // CR 145 start here
            else if (ddlMaritalStatus.Text.Equals("--Select--") && rbtNewCustomer.Checked && rbtIndividual.Checked)
            {
                IsError = true;
                strErrorMsg = "Please select Marital Status";
            }
            // CR 145 Start 
            else if (!string.IsNullOrEmpty(txtUID.Text) && !Regex.IsMatch(txtUID.Text.Trim(), "^[0-9]{12}$"))
            {
                IsError = true;
                strErrorMsg = "UID number can be blank or provide correct 12 digit UID number";
            }
            // CR 145 End
            else if (rbtIndividual.Checked && rbtMale.Checked && (drpCustomerTitle.SelectedValue.ToUpper() == "MRS" || drpCustomerTitle.SelectedValue.ToUpper() == "MISS"))
            {
                IsError = true;
                strErrorMsg = "Please select correct customer title/salutation, Mrs or Miss cannot be taken for Male Customer";
            }
            else if (rbtIndividual.Checked && rbtFemale.Checked && (drpCustomerTitle.SelectedValue.ToUpper() == "MR"))
            {
                IsError = true;
                strErrorMsg = "Please select correct customer title/salutation, Mr cannot be taken for Female Customer";
            }
            else if (string.IsNullOrEmpty(txtContactPerson.Text.Trim()) && rbtNewCustomer.Checked && rbtOrganization.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Contact Person Name";
            }
            else if (string.IsNullOrEmpty(txtOrganizationName.Text.Trim()) && rbtNewCustomer.Checked && rbtOrganization.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Organization Name";
            }
            else if (string.IsNullOrEmpty(txtMobileNumberOrz.Text.Trim()) && rbtNewCustomer.Checked && rbtOrganization.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Valid Mobile Number";
            }
            else if (txtMobileNumberOrz.Text.Trim().Length != 10 && rbtNewCustomer.Checked && rbtOrganization.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter 10 Digit Mobile Number";
            }
            else if (!Regex.IsMatch(txtMobileNumberOrz.Text.Trim(), "^[0-9]*$") && rbtNewCustomer.Checked && rbtOrganization.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Valid Mobile Number";
            }

            else if (string.IsNullOrEmpty(txtEmailIdOrz.Text.Trim()) && rbtNewCustomer.Checked && rbtOrganization.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Organization Email Address";
            }
            else if (!Regex.IsMatch(txtEmailIdOrz.Text.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase) && rbtNewCustomer.Checked && rbtOrganization.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Valid Organization Email Address";
            }
            else if (string.IsNullOrEmpty(txtRN1.Text.Trim()) && rbtIsRollover.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Registration Number 1";
                Accordion1.SelectedIndex = 1;
            }
            else if (string.IsNullOrEmpty(txtRN2.Text.Trim()) && rbtIsRollover.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Registration Number 2";
                Accordion1.SelectedIndex = 1;
            }
            else if (string.IsNullOrEmpty(txtRN3.Text.Trim()) && rbtIsRollover.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Registration Number 3";
                Accordion1.SelectedIndex = 1;
            }
            else if (string.IsNullOrEmpty(txtRN4.Text.Trim()) && rbtIsRollover.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Registration Number 4";
                Accordion1.SelectedIndex = 1;
            }
            //else if (IsInFraud_RegistrationNumber > 0)
            //{
            //    IsError = true;
            //    strErrorMsg = "Please Enter Valid Registration Number ";
            //    Accordion1.SelectedIndex = 1;
            //}
            else if (string.IsNullOrEmpty(txtEngineNumber.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Engine Number";
                Accordion1.SelectedIndex = 1;
            }
            else if (string.IsNullOrEmpty(txtChassisNumber.Text.Trim()))
            {
                IsError = true;
                strErrorMsg = "Please Enter Chassis Number";
                Accordion1.SelectedIndex = 1;
            }
            else if (txtEngineNumber.Text.Trim().Length < 6)
            {
                IsError = true;
                strErrorMsg = "Please Enter minimmum 6 characters Engine Number";
                Accordion1.SelectedIndex = 1;
            }
            else if (txtChassisNumber.Text.Trim().Length != 17 && rbtIsRollover.Checked == false)
            {
                IsError = true;
                strErrorMsg = "Please Enter 17 characters Chassis Number for New Business, no less or more.";
                Accordion1.SelectedIndex = 1;
            }
            else if ((txtChassisNumber.Text.Trim().Length < 6 || txtChassisNumber.Text.Trim().Length > 17) && rbtIsRollover.Checked == true)
            {
                IsError = true;
                strErrorMsg = "Please Enter Minimum 6 Characters and Maximum 17 Characters Chassis Number for Rollover.";
                Accordion1.SelectedIndex = 1;
            }
            else if (!string.IsNullOrEmpty(txtPartnerApplicationNumber.Text.Trim()) && txtPartnerApplicationNumber.Text.Trim().Length < 5)
            {
                IsError = true;
                strErrorMsg = "Please Enter Minimum 5 Characters In Partner Application Number OR Keep It Blank";
                Accordion1.SelectedIndex = 1;
            }
            else if (drpPreviousInsurerName.SelectedIndex == 0 && rbtIsRollover.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Select Previous Insurer Name";
                Accordion1.SelectedIndex = 1;
            }
            else if (string.IsNullOrEmpty(txtPreviousPolicyNumber.Text.Trim()) && rbtIsRollover.Checked)
            {
                IsError = true;
                strErrorMsg = "Please Enter Previous Policy Number";
                Accordion1.SelectedIndex = 1;
            }
            else if (string.IsNullOrEmpty(txtNomineeName.Text.Trim()) && rbtIndividual.Checked && hdnTenureOwnerDriver.Value != "0")
            {
                IsError = true;
                strErrorMsg = "Please Enter Nominee Name";
                Accordion1.SelectedIndex = 2;
            }
            else if (string.IsNullOrEmpty(txtNomineeDOB.Text.Trim()) && rbtIndividual.Checked && hdnTenureOwnerDriver.Value != "0")
            {
                IsError = true;
                strErrorMsg = "Please Enter Nominee Date of Birth";
                Accordion1.SelectedIndex = 2;
            }
            else if (drpNomineeRelationship.SelectedIndex == 0 && rbtIndividual.Checked && hdnTenureOwnerDriver.Value != "0")
            {
                IsError = true;
                strErrorMsg = "Please Select Nominee Relationship";
                Accordion1.SelectedIndex = 2;
            }
            else if (chkFinancier.Checked == true)
            {
                if (string.IsNullOrEmpty(txtFinacierAgrrementType.Text.Trim()))
                {
                    IsError = true;
                    strErrorMsg = "Please enter the Financier agrrement type.";
                    Accordion1.SelectedIndex = 3;
                }
                else if (string.IsNullOrEmpty(txtFinancierName.Text.Trim()))
                {
                    IsError = true;
                    strErrorMsg = "Please enter the financier name.";
                    Accordion1.SelectedIndex = 3;
                }
                else if (hdnFinancierName.Value != txtFinancierName.Text.Trim())
                {
                    IsError = true;
                    strErrorMsg = "Please selcect corect financier name.";
                    Accordion1.SelectedIndex = 3;
                }

                else if (string.IsNullOrEmpty(txtFinancierAddress.Text.Trim()))
                {
                    IsError = true;
                    strErrorMsg = "Please enter the financier address.";
                    Accordion1.SelectedIndex = 3;
                }

            }
            return IsError;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        public static DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl)
        {

            DataTable dt = new DataTable();

            int TempColumn = 0;



            foreach (XmlNode node in xnl.Item(0).ChildNodes)
            {

                TempColumn++;

                DataColumn dc = new DataColumn(node.Name, System.Type.GetType("System.String"));

                if (dt.Columns.Contains(node.Name))
                {

                    dt.Columns.Add(dc.ColumnName = dc.ColumnName + TempColumn.ToString());

                }

                else
                {

                    dt.Columns.Add(dc);

                }

            }

            int ColumnsCount = dt.Columns.Count;
            for (int i = 0; i < xnl.Count; i++)
            {

                DataRow dr = dt.NewRow();

                for (int j = 0; j < ColumnsCount; j++)
                {

                    dr[j] = xnl.Item(i).ChildNodes[j].InnerText;

                }

                dt.Rows.Add(dr);

            }

            return dt;

        }

        protected void btnDownloadPremiumBreakup_Click(object sender, EventArgs e)
        {
            string MaxQuoteVersion = hdnMaxQuoteVersion.Value;
            DownloadSavedPDF(MaxQuoteVersion);
        }

        private void DownloadSavedPDF(string MaxQuoteVersion)
        {
            string strQuoteNo = lblQuoteNumber.Text.Replace("Quote No.: ", "");
            strQuoteNo = strQuoteNo.Split(' ')[0]; //CODEMMC // ADDED THIS LINE TO PRECAUTIONARY CHECK IF QUOTE NUMBER HAS SPACE DUE TO MARKET MOVEMENT APPEN THEN GET THE FIRST PART IF ANY SAVE INTO DATABASE

            string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();
            string fileName = string.Format("{0}.pdf", strQuoteNo + "_" + MaxQuoteVersion);
            string pdfPath = (KotakQuotesPDFFiles) + fileName;

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + strQuoteNo + "_" + MaxQuoteVersion + ".pdf");
            Response.TransmitFile(pdfPath);
            Response.End();
        }

        private void DownloadPDF(bool IsOnlySavePDF, string MaxQuoteVersion, bool IsNewBusiness, string RegistrationDate, string IRDA_ProductCode, bool IsProposalPDF, string TenureOwnerDriver)
        {
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=Panel.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            pnlTest.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            string strHtml = sr.ReadToEnd();
            strHtml = strHtml.Replace("11px", "7px");
            strHtml = strHtml.Replace("gray", "firebrick");
            strHtml = strHtml.Replace("dodgerblue", "firebrick");

            string strQuoteNo = lblQuoteNumber.Text.Replace("Quote No.: ", "");
            strQuoteNo = strQuoteNo.Split(' ')[0]; //CODEMMC // ADDED THIS LINE TO PRECAUTIONARY CHECK IF QUOTE NUMBER HAS SPACE DUE TO MARKET MOVEMENT APPEN THEN GET THE FIRST PART IF ANY SAVE INTO DATABASE

            string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();
            string fileName = string.Format("{0}.pdf", strQuoteNo + "_" + MaxQuoteVersion);
            string pdfPath = (KotakQuotesPDFFiles) + fileName;

            bool isSuccess = SavePDF(strQuoteNo, strHtml, MaxQuoteVersion, IsNewBusiness, RegistrationDate, IRDA_ProductCode, TenureOwnerDriver, IsProposalPDF);
            //if (!File.Exists(pdfPath))
            //{
            //    isSuccess = SavePDF(strQuoteNo, strHtml);
            //}
            //else
            //{
            //    isSuccess = true;
            //}

            if (!IsOnlySavePDF)
            {
                if (isSuccess)
                {
                    CreatePDF(strQuoteNo, strHtml, MaxQuoteVersion, IsNewBusiness, RegistrationDate, IRDA_ProductCode, IsProposalPDF, TenureOwnerDriver);
                }
                else
                {
                    lblstatus.Text = "Error: " + "Due to some technical Issue pdf could not be downloaded, kindly try again.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=Panel.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //Response.Write();
            //Response.End();

            //Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 20f, 5f);
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.Write(pdfDoc);
            //Response.End();
        }

        private void CreatePDF(string strQuoteNo, string strHTML, string MaxQuoteVersion, bool IsNewBusiness, string RegistrationDate, string IRDA_ProductCode, bool IsProposalPDF, string TenureOwnerDriver)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + strQuoteNo + "_" + MaxQuoteVersion + ".pdf");
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
                string strProductName = "Car Secure (1+1)";
                DateTime dt_NewBusinessCondition = DateTime.ParseExact("01/09/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (IsNewBusiness && DateTime.ParseExact(RegistrationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= dt_NewBusinessCondition.Date)
                {
                    switch (IRDA_ProductCode)
                    {
                        case "1011":
                            strProductName = "Car Secure (1+1)";
                            break;
                        case "1062":
                            strProductName = "Car Secure – 3 years";
                            break;
                        case "1063":
                            strProductName = "Car Secure – Bundled";
                            break;
                    }
                }

                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfWriter.PageEvent = new ITextEvents_Bundled(strProductName);

                //open the stream 
                pdfDoc.Open();

                //for (int i = 0; i < 2; i++)
                //{
                //Paragraph para = new Paragraph("Product Type: Private Car", new Font(Font.BOLD));
                //para.Alignment = Element.ALIGN_CENTER;
                //pdfDoc.Add(para);

                if (IsProposalPDF)
                {
                    if ((TenureOwnerDriver != "0" || rbtOrganization.Checked == true))
                    {
                        strHTML = strHTML.Replace(@"<li>This quote is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>", "");
                    }
                    else
                    {
                        strHTML = strHTML.Replace(@"<li>This quote is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>", "<li>This proposal is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>");
                    }
                }
                else
                {
                    if ((TenureOwnerDriver != "0" || rbtOrganization.Checked == true))
                    {
                        strHTML = strHTML.Replace(@"<li>This quote is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>", "");
                    }
                }

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

        private bool SavePDF(string strQuoteNumber, string strHTML, string MaxQuoteVersion, bool IsNewBusiness, string RegistrationDate, string IRDA_ProductCode, string TenureOwnerDriver, bool IsProposalPDF)
        {
            bool isSuccess = false;
            string fileName = string.Empty;

            DateTime fileCreationDatetime = DateTime.Now;

            fileName = string.Format("{0}.pdf", strQuoteNumber + "_" + MaxQuoteVersion);

            //pdfPath = AppDomain.CurrentDomain.BaseDirectory + (@"\PDFs\") + fileName;
            string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles_OldFormat"].ToString();
            string pdfPath = (KotakQuotesPDFFiles) + fileName;

            Document pdfDoc = new Document(PageSize.A4, 13f, 13f, 140f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

            try
            {
                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    string strProductName = "Car Secure (1+1)";
                    DateTime dt_NewBusinessCondition = DateTime.ParseExact("01/09/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (IsNewBusiness && DateTime.ParseExact(RegistrationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= dt_NewBusinessCondition.Date)
                    {
                        switch (IRDA_ProductCode)
                        {
                            case "1011":
                                strProductName = "Car Secure (1+1)";
                                break;
                            case "1062":
                                strProductName = "Car Secure – 3 years";
                                break;
                            case "1063":
                                strProductName = "Car Secure – Bundled";
                                break;
                        }
                    }

                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, fs);
                    pdfWriter.PageEvent = new ITextEvents_Bundled(strProductName);
                    pdfDoc.Open();

                    if (IsProposalPDF)
                    {
                        if ((TenureOwnerDriver != "0" || rbtOrganization.Checked == true))
                        {
                            strHTML = strHTML.Replace(@"<li>This quote is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>", "");
                        }
                        else
                        {
                            strHTML = strHTML.Replace(@"<li>This quote is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>", "<li>This proposal is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>");
                        }
                    }
                    else
                    {
                        if ((TenureOwnerDriver != "0" || rbtOrganization.Checked == true))
                        {
                            strHTML = strHTML.Replace(@"<li>This quote is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>", "");
                        }
                    }
                    StringReader sr = new StringReader(strHTML);
                    htmlparser.Parse(sr);


                    pdfDoc.Close();
                    isSuccess = true;
                    //pdfWriter.Flush();
                    //pdfWriter.Close();

                    //fs.Flush();
                    //fs.Close();
                    //fs.Dispose();
                }

                //DigitalSign(pdfPath);

            }
            catch (Exception ex)
            {
                isSuccess = false;
                ExceptionUtility.LogException(ex, "SavePDF Method");
            }

            finally
            {


            }

            return isSuccess;
        }

        private void DigitalSign(string PDFFilePath)
        {
            try
            {
                if (File.Exists(PDFFilePath))
                {
                    byte[] InputPDFinBytes = File.ReadAllBytes(PDFFilePath);

                    File.Delete(PDFFilePath);

                    byte[] SignedPDFinBytes = clsDigitalCertificate.Sign(InputPDFinBytes);
                    File.WriteAllBytes(PDFFilePath, SignedPDFinBytes);
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "DigitalSign Method");
            }
        }

        private void Reset()
        {
            lblOwnDamagePremium.Text = "0.00";
            lblElectronicSI.Text = "0.00";
            lblNonElectronicSI.Text = "0.00";
            lblExternalBiFuelSI.Text = "0.00";

            lblEngineProtect.Text = "0.00";
            lblReturnToInvoice.Text = "0.00";
            lblRSA.Text = "0.00";
            lblConsumableCover.Text = "0.00";
            lblDepreciationCover.Text = "0.00";

            lblVoluntaryDeductionforDepWaiver.Text = "0.00";
            lblVoluntaryDeduction.Text = "0.00";
            lblNCB.Text = "0.00";

            lblTotalPremiumOwnDamage.Text = "0.00";

            lblBasicTPPremium.Text = "0.00";
            lblLiabilityForBiFuel.Text = "0.00";
            lblPAForUnnamedPassengerSI.Text = "0.00";
            lblPAForNamedPassengerSI.Text = "0.00";
            lblPAToPaidDriverSI.Text = "0.00";
            lblPACoverForOwnerDriver.Text = "0.00";
            lblLegalLiabilityToPaidDriverNo.Text = "0.00";
            lblLLEOPDCC.Text = "0.00";
            lblTotalPremiumLiability.Text = "0.00";

            lblSystemIDV.Text = "0.00";
            lblFinalIDV.Text = "0.00";

            lblNetPremium.Text = "0.00";
            lblServiceTax.Text = "0.00";
            lblTotalPremium.Text = "0.00";
            //lblTotalPremiumKerala.Text = "0.00"; //CR775A - Asked to remove kerala cess - Hasmukh
            lblstatus.Text = "Status: ";

            lblRateBasicOD.Text = "0.00";
            lblRateRTI.Text = "0.00";
            lblRateDC.Text = "0.00";
            lblRateEP.Text = "0.00";
            lblRateCC.Text = "0.00";

            lblRateDCA.Text = "0.00";
            lblRateKR.Text = "0.00";
            lblRateTC.Text = "0.00";
            lblRateNCBP.Text = "0.00";
            lblRateLOPB.Text = "0.00";

            lblDailyCarAllowance.Text = "0.00";
            lblLossofPersonalBelongings.Text = "0.00";
            lblNCBProtect.Text = "0.00";
            lblTyreCover.Text = "0.00";
            lblKeyReplacement.Text = "0.00";

            hdnIRDAProductCode.Value = "";
            hdnTenureOwnerDriver.Value = "";

            lblCustomerId.Text = "";
            lblCustomerFullName.Text = "";
            lblCustomerType.Text = "";
            lblQuoteNum.Text = "";
            lblProposalNumber.Text = "";
            lblTotalPremiumAmount.Text = "";
            lblMake2.Text = "";
            lblModel2.Text = "";
            lblVariant2.Text = "";
            lblRegistrationNumber.Text = "";
            lblEngineNumber.Text = "";
            lblChassisNumber.Text = "";
            txtEmailId.Text = "";

            lblPDFCustomerId.Text = "";
            lblPDFProposalNumber.Text = "";
            lblCreditScoreCustomerName.Text = "";
            lblCustomerIDProofNumber.Text = "";
        }

        protected void btnSendPDF_Click(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            pnlTest.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            string strHtml = sr.ReadToEnd();
            strHtml = strHtml.Replace("11px", "7px");
            strHtml = strHtml.Replace("gray", "firebrick");
            strHtml = strHtml.Replace("dodgerblue", "firebrick");

            string strQuoteNo = lblQuoteNumber.Text.Replace("Quote No.: ", "");
            strQuoteNo = strQuoteNo.Split(' ')[0]; //CODEMMC // ADDED THIS LINE TO PRECAUTIONARY CHECK IF QUOTE NUMBER HAS SPACE DUE TO MARKET MOVEMENT APPEN THEN GET THE FIRST PART IF ANY SAVE INTO DATABASE

            string strProposalNumber = lblProposalNumber.Text;

            string MakeModelVariant = lblMake.Text + " " + lblModel.Text + " " + lblVariant.Text;

            string MaxQuoteVersion = hdnMaxQuoteVersion.Value;

            string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();
            string fileName = string.Format("{0}.pdf", strQuoteNo + "_" + MaxQuoteVersion);
            string pdfPath = (KotakQuotesPDFFiles) + fileName;

            if (!File.Exists(pdfPath))
            {
                bool IsNewBusiness = rbtIsRollover.Checked == false ? true : false;
                bool IsProposalPDF = true;
                SavePDF(strQuoteNo, strHtml, MaxQuoteVersion, IsNewBusiness, lblRagistrationDate.Text.Trim(), hdnIRDAProductCode.Value, hdnTenureOwnerDriver.Value, IsProposalPDF);
            }

            if (txtEmailId.Text.Trim().Length > 0)
            {
                string strMailSubject = "Quotation [" + strQuoteNo + "] for your vehicle " + MakeModelVariant;
                string msg = SendEmail(txtEmailId.Text.Trim(), strQuoteNo, strMailSubject, MakeModelVariant, strProposalNumber, MaxQuoteVersion);
                if (msg == "success")
                {
                    Alert.Show("Mail Sent Successfully!");
                }
                else
                {
                    Alert.Show(msg);
                }
            }
        }

        private string SendEmail(string ToEmailIds, string QuoteNumber, string MailSubject, string MakeModelVariant, string ProposalNumber, string QuoteVersion)
        {
            string ActualToEmailIds = ToEmailIds;
            string smtp_DefaultCCMailId = ConfigurationManager.AppSettings["smtp_DefaultCCMailId"].ToString();

            //ToEmailIds = ToEmailIds + ";" + smtp_DefaultCCMailId;
            string strMessage = string.Empty;
            string[] ToEmailAddr = ToEmailIds.Split(';');

            string smtp_Username = ConfigurationManager.AppSettings["smtp_Username"].ToString();
            string smtp_Password = ConfigurationManager.AppSettings["smtp_Password"].ToString();
            string smtp_Host = ConfigurationManager.AppSettings["smtp_Host"].ToString();
            string smtp_FromMailId = ConfigurationManager.AppSettings["smtp_FromMailId"].ToString();
            string strPath = string.Empty;
            string MailBody = string.Empty;

            //string attachmentFilename = AppDomain.CurrentDomain.BaseDirectory + (@"\PDFs\" + QuoteNumber + ".pdf");
            string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();
            string attachmentFilename = (KotakQuotesPDFFiles + QuoteNumber + "_" + QuoteVersion + ".pdf");

            string PayULink = string.Empty;
            string ReviewConfirmLink = string.Empty;
            if (hdnIsWarningPresent.Value == "0") //0 MEANS WARNING TEXT IS NOT PRESENT
            {
                if (optPayULink.Checked)
                {
                    PayULink = Create_Invoice(QuoteNumber.Replace(" ", "").Replace("-", "").Trim(), lblTotalPremium.Text.Substring(2, lblTotalPremium.Text.Trim().Length - 2).Replace(",", ""), lblCustomerFullName.Text, txtEmailId.Text.Trim(), rbtIndividual.Checked ? txtMobileNumber.Text.Trim() : txtMobileNumberOrz.Text.Trim(), MakeModelVariant, ProposalNumber);
                }
                else
                {
                    ReviewConfirmLink = Create_ReviewAndConfirmLink(QuoteNumber, ProposalNumber);
                }
            }
            //string PayULink = "https://test.payu.in/processInvoice?invoiceId=e676767cd80d362794d913a8267ccac62a6a37c018cd2c459a57696ff1ac260a";
            //string PayULink = string.Empty;
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = smtp_Host; //"192.168.201.61"; //"kgirelay.kgi.kotakgroup.com";
                                         //client.EnableSsl = true;
                client.Timeout = 3600000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(smtp_Username, smtp_Password);


                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBody.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("QuoteNumber", QuoteNumber);
                MailBody = MailBody.Replace("MakeModelVariant", MakeModelVariant.ToUpper());
                MailBody = MailBody.Replace("@Remarks", txtRemarks.Text.Trim());

                if (optPayULink.Checked)
                {
                    Uri uriResult;
                    bool IsValidURL = Uri.TryCreate(PayULink, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (IsValidURL)
                    {
                        //SAVE URL IN TABLE
                        string googleShortURL = string.Empty;
                        GoogleURLShortner(PayULink, out googleShortURL);
                        string shortURL = googleShortURL;

                        SaveInvoiceLink(PayULink, shortURL, ToEmailIds, QuoteNumber, "", ProposalNumber);

                        string buttonHTML = "<a href='" + shortURL + "' style='background-color: #1d4581;border: 1px solid black;color: white;padding: 1px 32px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px;cursor: pointer;margin-left:2px'> Click Here </a>&nbsp; to Make Payment <br /><br />";
                        MailBody = MailBody.Replace("PayULink", buttonHTML);
                    }
                    else
                    {
                        //do not create button to pay
                        MailBody = MailBody.Replace("PayULink", "");
                    }
                }
                else
                {
                    Uri uriResult;
                    bool IsValidURL = Uri.TryCreate(ReviewConfirmLink, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (IsValidURL)
                    {
                        string buttonHTML = "<a href='" + ReviewConfirmLink + "' style='background-color: #1d4581;border: 1px solid black;color: white;padding: 1px 32px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px;cursor: pointer;margin-left:2px'> Click Here </a>&nbsp; to Review and Make Payment <br /><br />";
                        MailBody = MailBody.Replace("PayULink", buttonHTML);
                    }
                    else
                    {
                        //do not create button to pay
                        MailBody = MailBody.Replace("PayULink", "");
                    }
                }

                MailSubject = MailSubject.Replace('\r', ' ').Replace('\n', ' ');
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(smtp_FromMailId);
                mm.Subject = MailSubject;
                mm.Body = MailBody;
                mm.IsBodyHtml = true;

                foreach (var toMailId in ToEmailAddr)
                {
                    strPath = string.Empty;
                    MailBody = string.Empty;

                    if (toMailId != null)
                    {
                        if (toMailId.Length > 5)
                        {
                            mm.To.Add(toMailId);
                        }
                    }
                }


                if (Session["RegionalDeptHeadEmailId"] != null)
                {
                    if (Session["RegionalDeptHeadEmailId"].ToString().Trim() != "")
                    {
                        string CCEmailIds = Session["RegionalDeptHeadEmailId"].ToString();
                        string[] CCEmailAddr = CCEmailIds.Split(';');
                        foreach (var CCMailId in CCEmailAddr)
                        {
                            if (CCMailId != null)
                            {
                                if (CCMailId.Length > 5)
                                {
                                    if (Regex.IsMatch(CCMailId.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                                    {
                                        mm.CC.Add(CCMailId);
                                    }
                                }
                            }
                        }
                    }
                }

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (attachmentFilename != null)
                {
                    Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mm.Attachments.Add(attachment);
                }

                client.Send(mm);
                strMessage = "success";
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                //string strPathErrorFile = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                //File.WriteAllText(strPathErrorFile, "Error: " + strMessage);
                ExceptionUtility.LogException(ex, "SendEmail Method");
            }

            return strMessage;
        }

        protected void btnOpenCustomerPopUp_Click(object sender, EventArgs e)
        {
            //string strRTOCode = txtRTOAuthorityCode.Text.Trim().Replace("-", "").Replace(" ", "");
            //txtRN1.Text = strRTOCode.Substring(0, 2);
            //txtRN2.Text = strRTOCode.Replace(txtRN1.Text, "");

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalViewPremium();", true);

            txtFirstName.Text = "";
            txtFirstName.Enabled = true;
            txtMiddleName.Text = "";
            txtMiddleName.Enabled = true;
            txtLastName.Text = "";
            txtLastName.Enabled = true;

            txtDateofBirth.Text = "";
            txtDateofBirth.Enabled = true;

            txtEmailAddress.Text = "";
            txtEmailAddress.Enabled = true;

            txtMobileNumber.Text = "";
            txtMobileNumber.Enabled = true;

            txtContactPerson.Text = "";
            txtContactPerson.Enabled = true;

            txtOrganizationName.Text = "";
            txtOrganizationName.Enabled = true;

            txtMobileNumberOrz.Text = "";
            txtMobileNumberOrz.Enabled = true;

            txtEmailIdOrz.Text = "";
            txtEmailIdOrz.Enabled = true;

            //txtRN1.Text = "";
            //txtRN2.Text = "";

            txtRN1.Enabled = txtRN1.Text.Trim().Length <= 0 ? true : false;
            txtRN2.Enabled = txtRN2.Text.Trim().Length <= 0 ? true : false;

            txtRN3.Text = "";
            txtRN3.Enabled = true;

            txtRN4.Text = "";
            txtRN3.Enabled = true;

            txtEngineNumber.Text = "";
            txtEngineNumber.Enabled = true;

            txtChassisNumber.Text = "";
            txtChassisNumber.Enabled = true;

            txtPreviousPolicyNumber.Text = "";
            txtNomineeName.Text = "";
            txtNomineeDOB.Text = "";
            txtNameOfAppointee.Text = "";

            txtPartnerApplicationNumber.Text = "";

            lblStatusSaveProposal.Text = "";

            Accordion1.SelectedIndex = 0;

            string strQuoteNo = lblQuoteNumber.Text.Replace("Quote No.: ", "");
            strQuoteNo = strQuoteNo.Split(' ')[0]; //CODEMMC // ADDED THIS LINE TO PRECAUTIONARY CHECK IF QUOTE NUMBER HAS SPACE DUE TO MARKET MOVEMENT APPEN THEN GET THE FIRST PART IF ANY SAVE INTO DATABASE

            txtPartnerApplicationNumber.Text = strQuoteNo;

            if (Convert.ToInt64(hdnCreditScoreId.Value) > 0)
            {
                SetCustomerDetailsFromCreditScore(Convert.ToInt64(hdnCreditScoreId.Value));
            }

            if (hdnIsFastlaneFlow.Value == "1")
            {
                SetFastlaneDetails(strQuoteNo);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposal();", true);
        }

        private void SetCustomerDetailsFromCreditScore(long CreditScoreId)
        {
            try
            {
                string ResponseXML = string.Empty;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_GET_CREDIT_SCORE_REQUEST_RESPONSE";
                        cmd.Parameters.AddWithValue("@CreditScoreId", CreditScoreId);
                        cmd.Connection = conn;
                        conn.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                txtFirstName.Text = sdr["CustomerFirstName"].ToString();
                                txtFirstName.Enabled = txtFirstName.Text.Length > 0 ? false : true;

                                txtMiddleName.Text = sdr["CustomerMiddleName"].ToString();
                                txtMiddleName.Enabled = txtMiddleName.Text.Length > 0 ? false : true;

                                txtLastName.Text = sdr["CustomerLastName"].ToString();
                                txtLastName.Enabled = txtLastName.Text.Length > 0 ? false : true;

                                //string strXmlPath22 = AppDomain.CurrentDomain.BaseDirectory + "TEST.txt";
                                string[] strCDOB = sdr["CustomerDateofBirth"].ToString().Split(' ');

                                //File.WriteAllText(strXmlPath22, strCDOB[0]);

                                DateTime dtDOB = DateTime.ParseExact(strCDOB[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                string strDOB = dtDOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                txtDateofBirth.Text = strDOB; // dtDOB.Date.ToString();
                                //txtDateofBirth.Enabled = txtDateofBirth.Text.Length > 0 ? false : true;

                                //if (sdr["CustomerGender"].ToString().ToLower() == "male")
                                //{
                                //    rbtMale.Checked = true;
                                //    rbtFemale.Checked = false;
                                //}
                                //else
                                //{
                                //    rbtMale.Checked = false;
                                //    rbtFemale.Checked = true;
                                //}

                                rbtMale.Enabled = false; //txtFirstName.Text.Length > 0 ? false : true;
                                rbtFemale.Enabled = false; //txtFirstName.Text.Length > 0 ? false : true;

                                //txtMobileNumber.Text = sdr["CustomerPhoneNumber"].ToString();
                                //txtMobileNumber.Enabled = txtMobileNumber.Text.Length > 0 ? false : true;

                                string Response_PhoneNumber = sdr["Response_PhoneNumber"].ToString();
                                string CustomerPhoneNumber = string.IsNullOrEmpty(sdr["CustomerPhoneNumber"].ToString()) ? "" : sdr["CustomerPhoneNumber"].ToString();
                                txtMobileNumber.Text = string.IsNullOrEmpty(Response_PhoneNumber) ? CustomerPhoneNumber : Response_PhoneNumber;
                                //txtMobileNumber.Enabled = txtMobileNumber.Text.Length > 0 ? false : true;

                                string Response_EmailAddress = sdr["Response_EmailAddress"].ToString();
                                txtEmailAddress.Text = string.IsNullOrEmpty(Response_EmailAddress) ? "" : Response_EmailAddress;


                                txtPinCode.Text = sdr["CustomerPincode"].ToString();
                                //txtPinCode.Enabled = txtPinCode.Text.Length > 0 ? false : true;

                                lblPincodeLocality.Text = sdr["PinCodeLocality"].ToString();
                                hdnStateId.Value = sdr["StateCode"].ToString();
                                lblStateName.Text = sdr["StateName"].ToString();
                                hdnDistrictId.Value = sdr["CityDistrictCode"].ToString();
                                lblDistrictName.Text = sdr["CityDistrictName"].ToString();
                                hdnCityId.Value = sdr["CityId"].ToString();
                                lblCityName.Text = sdr["CityName"].ToString();

                                //txtAddressLine1.Text = sdr["Response_AddressLine1"].ToString(); //SANJAY SIR TOLD TO COMMENT THIS LINE
                                ////txtAddressLine1.Enabled = txtAddressLine1.Text.Length > 0 ? false : true;

                                //txtAddressLine2.Text = sdr["Response_AddressLine2"].ToString(); //SANJAY SIR TOLD TO COMMENT THIS LINE
                                ////txtAddressLine2.Enabled = txtAddressLine2.Text.Length > 0 ? false : true;

                                //txtAddressLine3.Text = sdr["Response_AddressLine3"].ToString(); //SANJAY SIR TOLD TO COMMENT THIS LINE
                                ////txtAddressLine3.Enabled = txtAddressLine3.Text.Length > 0 ? false : true;

                                string Response_IncomeTaxPanNumber = sdr["Response_IncomeTaxPanNumber"].ToString();
                                string IncomeTaxPAN = string.IsNullOrEmpty(sdr["IncomeTaxPAN"].ToString()) ? "" : sdr["IncomeTaxPAN"].ToString();
                                txtPanNumber.Text = string.IsNullOrEmpty(Response_IncomeTaxPanNumber) ? IncomeTaxPAN : Response_IncomeTaxPanNumber;
                                txtPanNumber.Enabled = txtPanNumber.Text.Length > 0 ? false : true;
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SetCustomerDetailsFromCreditScore Method");
            }
        }

        private void SetFastlaneDetails(string QuoteNumber)
        {
            try
            {
                string ResponseXML = string.Empty;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_GET_FASTLANE_DETAILS_FOR_QUOTE";
                        cmd.Parameters.AddWithValue("@QuoteNumber", QuoteNumber);
                        cmd.Parameters.AddWithValue("@QuoteVersion", Convert.ToInt16(hdnMaxQuoteVersion.Value));
                        cmd.Connection = conn;
                        conn.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                if (!string.IsNullOrEmpty(sdr["IsFastlaneFlow"].ToString()))
                                {
                                    if (Convert.ToBoolean(sdr["IsFastlaneFlow"]))
                                    {
                                        txtRN1.Text = sdr["PropRisks_RegistrationNumber"].ToString().Trim();
                                        txtRN1.Enabled = txtRN1.Text.Length > 0 ? false : true;

                                        txtRN2.Text = sdr["PropRisks_RegistrationNumber2"].ToString().Trim();
                                        txtRN2.Enabled = txtRN2.Text.Length > 0 ? false : true;

                                        txtRN3.Text = sdr["PropRisks_RegistrationNumber3"].ToString().Trim();
                                        txtRN3.Enabled = txtRN3.Text.Length > 0 ? false : true;

                                        txtRN4.Text = sdr["PropRisks_RegistrationNumber4"].ToString().Trim();
                                        txtRN4.Enabled = txtRN4.Text.Length > 0 ? false : true;

                                        txtEngineNumber.Text = sdr["PropRisks_Engineno"].ToString();
                                        txtEngineNumber.Enabled = txtEngineNumber.Text.Length > 0 ? false : true;

                                        txtChassisNumber.Text = sdr["PropRisks_ChassisNumber"].ToString();
                                        txtChassisNumber.Enabled = txtChassisNumber.Text.Length > 0 ? false : true;
                                    }
                                }
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SetCustomerDetailsFromCreditScore Method");
            }
        }

        protected void btnSaveProposal_Click(object sender, EventArgs e)
        {
            lblCustomerId.Text = "";
            lblCustomerFullName.Text = "";
            lblCustomerType.Text = "";
            lblQuoteNum.Text = "";
            lblProposalNumber.Text = "";
            lblTotalPremiumAmount.Text = "";
            lblMake2.Text = "";
            lblModel2.Text = "";
            lblVariant2.Text = "";
            lblRegistrationNumber.Text = "";
            lblEngineNumber.Text = "";
            lblChassisNumber.Text = "";
            txtEmailId.Text = "";

            string strErrorMsg = string.Empty;
            lblStatusSaveProposal.Text = "";
            long CustomerId = 0;
            string CustomerName = string.Empty;

            if (ValidationSaveProposal(out strErrorMsg))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposal();", true);
                lblStatusSaveProposal.Text = "Error: " + strErrorMsg;
            }
            else
            {
                string strQuoteNo = lblQuoteNumber.Text.Replace("Quote No.: ", "");
                strQuoteNo = strQuoteNo.Split(' ')[0]; //CODEMMC // ADDED THIS LINE TO PRECAUTIONARY CHECK IF QUOTE NUMBER HAS SPACE DUE TO MARKET MOVEMENT APPEN THEN GET THE FIRST PART IF ANY SAVE INTO DATABASE

                if (rbtNewCustomer.Checked)
                {
                    CustomerId = 0;
                    CustomerName = string.Empty;
                }
                else
                {
                    CustomerId = Convert.ToInt64(txtCustomerId.Text.Trim());
                    CustomerName = txtCustomerFullName.Text.Trim();
                }

                if (CustomerId == 0)
                {
                    SaveCustomer(strQuoteNo, out strErrorMsg, out CustomerId, out CustomerName);
                }

                if (CustomerId > 0)
                {
                    string RegistrationNumber = txtRN1.Text.Trim() + "" + txtRN2.Text.Trim() + "" + txtRN3.Text.Trim() + "" + txtRN4.Text.Trim();
                    DataSet dsRequest = GetRequestXMLDataset(strQuoteNo.Trim(), hdnMaxQuoteVersion.Value, RegistrationNumber);

                    if (dsRequest.Tables.Count > 1 && Convert.ToBoolean(dsRequest.Tables[1].Rows[0]["IsInFraud_RegistrationNumber"]) == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposal();", true);
                        lblStatusSaveProposal.Text = "Error: Please entre correct registration number";
                    }
                    else
                    {
                        SaveProposal(dsRequest, CustomerId, CustomerName, strQuoteNo);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposal();", true);
                    lblStatusSaveProposal.Text = "Error: Error while saving customer. " + strErrorMsg;
                }
            }
        }

        private void CheckIfCustomerExists(string EmailAddress, out long CustomerId, out string CustomerName)
        {
            CustomerId = 0;
            CustomerName = string.Empty;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_CUSTOMER_DETAILS";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "EmailAddress", DbType.String, ParameterDirection.Input, "EmailAddress", DataRowVersion.Current, EmailAddress);


                dbCommand.CommandType = CommandType.StoredProcedure;
                DataSet ds = null;
                ds = db.ExecuteDataSet(dbCommand);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            CustomerId = Convert.ToInt64(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                            CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "CheckIfCustomerExists Method");
            }
        }

        private void SaveCustomer(string strQuoteNo, out string ErrorMsg, out long CustomerId, out string CustomerName)
        {
            ErrorMsg = string.Empty;
            CustomerId = 0;
            CustomerName = string.Empty;

            //CheckIfCustomerExists(txtEmailAddress.Text.Trim(), out CustomerId, out CustomerName);

            if (CustomerId == 0) //means customer does not exists
            {
                //ADD CUSTOMER TO GIST DB AND GET THE CUSTOMER ID
                AddCustomerToGIST(out ErrorMsg, out CustomerId, out CustomerName); //uncomment later when sanjay sir says

                //this below code need to be removed in future and uncomment the above method AddCustomerToGIST (when sanjay sir says)
                //start
                //if (rbtIndividual.Checked)
                //{
                //    CustomerId = Convert.ToInt64(ConfigurationManager.AppSettings["CUSTOMERID_INDIVIDUAL"].ToString());
                //    CustomerName = ConfigurationManager.AppSettings["CUSTOMERNAME_INDIVIDUAL"].ToString();
                //}
                //else
                //{
                //    CustomerId = Convert.ToInt64(ConfigurationManager.AppSettings["CUSTOMERID_ORG"].ToString());
                //    CustomerName = ConfigurationManager.AppSettings["CUSTOMERNAME_ORG"].ToString();
                //}
                //end

                SaveCustomerDetails(CustomerId, strQuoteNo);
            }
        }

        private void SaveCustomerDetails(long CustomerId, string QuoteNumber)
        {
            DateTime dtDOB = DateTime.Now;
            if (!string.IsNullOrEmpty(txtDateofBirth.Text.Trim()))
            {
                dtDOB = DateTime.ParseExact(txtDateofBirth.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strDateOfBirth = dtDOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_CUSTOMER_DETAILS";

                        cmd.Parameters.AddWithValue("@QuoteNumber", QuoteNumber);
                        cmd.Parameters.AddWithValue("@EmailAddress", rbtIndividual.Checked ? txtEmailAddress.Text.Trim() : txtEmailIdOrz.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerId", CustomerId); // hardcoded 0 till sanjay sir ask to replace
                        cmd.Parameters.AddWithValue("@FirstName", rbtIndividual.Checked ? txtFirstName.Text.Trim() : null);
                        cmd.Parameters.AddWithValue("@MiddleName", rbtIndividual.Checked ? txtMiddleName.Text.Trim() : null);
                        cmd.Parameters.AddWithValue("@LastName", rbtIndividual.Checked ? txtLastName.Text.Trim() : null);
                        cmd.Parameters.AddWithValue("@Gender", rbtIndividual.Checked ? (rbtMale.Checked ? "MALE" : "FEMALE") : null);
                        cmd.Parameters.AddWithValue("@DateOfBirth", rbtIndividual.Checked ? dtDOB.Date : (DateTime?)null);
                        cmd.Parameters.AddWithValue("@MobileNumber", rbtIndividual.Checked ? txtMobileNumber.Text.Trim() : txtMobileNumberOrz.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerType", rbtIndividual.Checked ? "Individual" : "Organization");
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.Parameters.AddWithValue("@ContactPerson", rbtOrganization.Checked ? txtContactPerson.Text.Trim() : null);
                        cmd.Parameters.AddWithValue("@OrganizationName", rbtOrganization.Checked ? txtOrganizationName.Text.Trim() : null);

                        cmd.Parameters.AddWithValue("@CustomerPincode", txtPinCode.Text.Trim());
                        cmd.Parameters.AddWithValue("@PinCodeLocality", lblPincodeLocality.Text.Trim());
                        cmd.Parameters.AddWithValue("@CountryID", "64");
                        cmd.Parameters.AddWithValue("@CountryName", "India");
                        cmd.Parameters.AddWithValue("@StateCode", hdnStateId.Value);
                        cmd.Parameters.AddWithValue("@StateName", lblStateName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CityDistrictCode", hdnDistrictId.Value);
                        cmd.Parameters.AddWithValue("@CityDistrictName", lblDistrictName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CityId", hdnCityId.Value);
                        cmd.Parameters.AddWithValue("@CityName", lblCityName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Landmark", "NA");
                        cmd.Parameters.AddWithValue("@AddressLine1", txtAddressLine1.Text.Trim());
                        cmd.Parameters.AddWithValue("@AddressLine2", txtAddressLine2.Text.Trim());
                        cmd.Parameters.AddWithValue("@AddressLine3", txtAddressLine3.Text.Trim());
                        cmd.Parameters.AddWithValue("@RegistrationNumber1", txtRN1.Text.Trim());
                        cmd.Parameters.AddWithValue("@RegistrationNumber2", txtRN2.Text.Trim());
                        cmd.Parameters.AddWithValue("@RegistrationNumber3", txtRN3.Text.Trim());
                        cmd.Parameters.AddWithValue("@RegistrationNumber4", txtRN4.Text.Trim());
                        cmd.Parameters.AddWithValue("@EngineNumber", txtEngineNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@ChassisNumber", txtChassisNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@NomineeName", txtNomineeName.Text.Trim());
                        cmd.Parameters.AddWithValue("@NomineeDOB", txtNomineeDOB.Text.Trim());
                        cmd.Parameters.AddWithValue("@NomineeRelationShip", drpNomineeRelationship.SelectedValue);
                        cmd.Parameters.AddWithValue("@AppointeeName", txtNameOfAppointee.Text.Trim());
                        cmd.Parameters.AddWithValue("@AppointeeRelationShip", drpRelationshipWithAppointee.SelectedValue);
                        cmd.Parameters.AddWithValue("@PanNumber", txtPanNumber.Text.Trim());

                        cmd.Parameters.AddWithValue("@MaxQuoteVersion", Convert.ToInt16(hdnMaxQuoteVersion.Value));

                        cmd.Parameters.AddWithValue("@PreviousInsurerName", drpPreviousInsurerName.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@PreviousPolicyNumber", txtPreviousPolicyNumber.Text.Trim());

                        cmd.Parameters.AddWithValue("@IsFinancierDetailsAvaialble", chkFinancier.Checked == true ? 1 : 0);
                        cmd.Parameters.AddWithValue("@FinancierAgreementType", txtFinacierAgrrementType.Text.Trim());
                        cmd.Parameters.AddWithValue("@FinancierName", hdnFinancierName.Value.Trim());
                        cmd.Parameters.AddWithValue("@FinancierAddress", txtFinancierAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@LoanAccountNumber", txtLoanAccountNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@FileNumber", txtFileNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaritalStatus", ddlMaritalStatus.Text.Trim());
                        cmd.Parameters.AddWithValue("@UIDNumber", Encryption.EncryptText(txtUID.Text.Trim()));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Error Occured, " + ex.Message);
                ExceptionUtility.LogException(ex, "SaveCustomerDetails Method");
            }
        }

        private void SaveProposalDetails(string QuoteNumber, string ProposalNumber, string CustomerId, string TotalPremium
            , string CGSTAmount, string CGSTPercentage
            , string SGSTAmount, string SGSTPercentage
            , string IGSTAmount, string IGSTPercentage
            , string UGSTAmount, string UGSTPercentage
            , string TotalGSTAmount, string ServiceTaxAmount, string ApplicationNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_PROPOSAL_DETAILS";

                        cmd.Parameters.AddWithValue("@QuoteNumber", QuoteNumber);
                        cmd.Parameters.AddWithValue("@ProposalNumber", ProposalNumber);
                        cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                        cmd.Parameters.AddWithValue("@TotalPremium", TotalPremium);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());

                        cmd.Parameters.AddWithValue("@CGSTAmount", CGSTAmount);
                        cmd.Parameters.AddWithValue("@CGSTPercentage", CGSTPercentage);

                        cmd.Parameters.AddWithValue("@SGSTAmount", SGSTAmount);
                        cmd.Parameters.AddWithValue("@SGSTPercentage", SGSTPercentage);

                        cmd.Parameters.AddWithValue("@IGSTAmount", IGSTAmount);
                        cmd.Parameters.AddWithValue("@IGSTPercentage", IGSTPercentage);

                        cmd.Parameters.AddWithValue("@UGSTAmount", UGSTAmount);
                        cmd.Parameters.AddWithValue("@UGSTPercentage", UGSTPercentage);

                        cmd.Parameters.AddWithValue("@TotalGSTAmount", TotalGSTAmount);
                        cmd.Parameters.AddWithValue("@ServiceTaxAmount", ServiceTaxAmount);

                        cmd.Parameters.AddWithValue("@ApplicationNumber", ApplicationNumber);

                        cmd.Parameters.AddWithValue("@MaxQuoteVersion", Convert.ToInt16(hdnMaxQuoteVersion.Value));

                        cmd.Parameters.AddWithValue("@TypeOfTransfer", drpTypeOfTransfer.SelectedValue);
                        cmd.Parameters.AddWithValue("@PartnerApplicationNumber", txtPartnerApplicationNumber.Text.Trim() == "" ? QuoteNumber : txtPartnerApplicationNumber.Text.Trim());

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Error Occured, " + ex.Message);
                ExceptionUtility.LogException(ex, "SaveProposalDetails Method");
            }
        }

        private void AddCustomerToGIST(out string ErrorMsg, out long CustomerId, out string CustomerName)
        {
            ErrorMsg = string.Empty;
            CustomerId = 0;
            CustomerName = string.Empty;
            try
            {

                string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
                string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();

                ServiceReference1.ServiceResult objServiceResult = new ServiceReference1.ServiceResult();
                objServiceResult.UserData = new ServiceReference1.clsUserData();

                objServiceResult.UserData.IsWorkSheetRequired = false;
                objServiceResult.UserData.UserId = strUserId;
                objServiceResult.UserData.Password = strPassword;
                objServiceResult.UserData.UserRole = "ADMIN";
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestXML_ThinCustomer());
                objServiceResult.UserData.IsInternalRiskGrid = true;

                objServiceResult.UserData.ErrorText = "";

                proxy.AddCustomer(strUserId, strPassword, ref objServiceResult);
                proxy.Close();
                if (objServiceResult.UserData.ErrorText.Length > 0)
                {
                    ErrorMsg = objServiceResult.UserData.ErrorText;
                    CustomerId = Convert.ToInt64(objServiceResult.UserData.CustomerId);
                    if (rbtIndividual.Checked)
                    {
                        CustomerName = txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim();
                    }
                    else
                    {
                        CustomerName = txtOrganizationName.Text.Trim();
                    }
                }
                else
                {
                    CustomerId = Convert.ToInt64(objServiceResult.UserData.CustomerId);
                    CustomerName = objServiceResult.UserData.CustomerName;
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
        }

        private string TruncateWarningText(string WarningText)
        {
            return WarningText.Trim().Replace(@"\r\n", " ")
                                .Replace("Approval is Pending for Registration Number - Others is Opted in the Policy", "")
                                .Replace("Application number is blank, so system is generating new Application Number", "")
                                .Replace("Approval is Pending for Registration Number - Others is Opted in the Policy.\r\nEscalate to Motor CO-UW\r\n.", "")
                                .Replace("Escalate to Motor CO-UW", "")
                                .Replace("\r\nApproval is Pending for Registration Number - Others is Opted in the Policy", "")
                                .Replace("N410128CTRLS,", "")
                                .Replace("N410128CTRLS", "")
                                .Replace("N410334CTRLS,", "")
                                .Replace("N410334CTRLS", "")
                                .Replace("OtherInformation11 Length Must Not Exceed 100", "")
                                .Replace("OtherInformation12 Length Must Not Exceed 100", "")
                                .Replace("OtherInformation11  Length Must Not Exceed 100", "") //DOUBLE SPACE
                                .Replace("OtherInformation12  Length Must Not Exceed 100", "")
                                .Replace("N331775CTRLS", "")
                                .Replace("N331776CTRLS", "")
                                .Replace("N331775CTRLS,", "")
                                .Replace("N331776CTRLS,", "")
                                .Replace("NCTRLS", "")
                                .Replace("NCTRLS,", "")
                                .Replace("OtherInformation Length Must Not Exceed", "")
                                .Replace("OtherInformation  Length Must Not Exceed", "")
                                .Replace("KeyReplacementRate  Length Must Not Exceed 100", "")
                                .Replace("N464169CTRLS", "")
                                .Replace("LossofPersonalBelongingsRate", "")
                                .Replace("NCBProtectRate", "")
                                .Replace("ReturnToInvoiceRate", "")
                                .Replace("TyreCvrRate", "")
                                .Replace("Length Must Not Exceed 100", "")
                                ;
        }

        private void SaveProposal(DataSet dsRequest, long CustomerId, string CustomerName, string QuoteNumber)
        {
            bool IsSuccess = false;
            string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
            string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();
            bool IsNewBusiness = false;
            string RegistrationDate = string.Empty;
            string IRDA_ProductCode = string.Empty;
            bool IsSendPaymentLink = true;
            string TenureOwnerDriver = string.Empty;

            bool IsDepreciationCover = false;
            bool IsDailyCarAllowance = false;
            string ddlDailyCarAllowance = "0";
            bool IsKeyReplacement = false;
            string ddlKeyReplacement = "0";
            bool IsLossofPersonalBelongings = false;
            string ddlLossofPersonalBelongingsSI = "0";
            string drpVDA = "0";

            //string strUserId = "GC0014"; string strPassword = "cmc123"; //for uat
            //string strUserId = "GI0033"; string strPassword = "May@2016"; //for prod
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
                objServiceResult.UserData.AuthenticateKey = Auth(strUserId, strPassword);
                string SaveProposalRequestXML = GetRequestXMLForSaveProposal(dsRequest, CustomerId, CustomerName, out IsNewBusiness, out RegistrationDate
                    , out IRDA_ProductCode, out IsSendPaymentLink, out TenureOwnerDriver, out IsDepreciationCover, out IsDailyCarAllowance,
                out ddlDailyCarAllowance, out IsKeyReplacement, out ddlKeyReplacement, out IsLossofPersonalBelongings, out ddlLossofPersonalBelongingsSI,  out drpVDA);
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(SaveProposalRequestXML);
                objServiceResult.UserData.IsInternalRiskGrid = true;
                objServiceResult.UserData.ErrorText = "";
                objServiceResult.UserData.ProposalGenerationMode = ServiceReference1.clsUserData.ProposalMode.SaveProposal;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager

                proxy.SaveProposal(strUserId, strPassword, ref objServiceResult);
                proxy.Close();

                IsSuccess = objServiceResult.UserData.ErrorText.Trim().Length > 0 ? false : true;

                SaveRequestResponse_ForSaveProposal(SaveProposalRequestXML, objServiceResult.UserData.UserResultXml, IsSuccess, objServiceResult.UserData.ErrorText, QuoteNumber);

                if (objServiceResult.UserData.ErrorText.Trim().Length > 0)
                {
                    lblStatusSaveProposal.Text = "Error: " + objServiceResult.UserData.ErrorText;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposal();", true);
                }
                else
                {
                    string ProposalNumber = objServiceResult.UserData.ProposalNumber;

                    //string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "ResultXML.xml";
                    //File.WriteAllText(strXmlPath, String.Empty);
                    //File.WriteAllText(strXmlPath, objServiceResult.UserData.UserResultXml);

                    lblCustomerId.Text = CustomerId.ToString();
                    lblCustomerFullName.Text = CustomerName; // rbtIndividual.Checked ? (txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim()) : txtOrganizationName.Text.Trim(); // CustomerName; change back to CustomerName when sanjay sir says
                    lblCustomerType.Text = rbtIndividual.Checked ? "Individual" : "Organization";
                    lblQuoteNum.Text = QuoteNumber;
                    lblProposalNumber.Text = ProposalNumber;
                    lblTotalPremiumAmount.Text = objServiceResult.UserData.TotalPremium.ToString();
                    lblMake2.Text = lblMake.Text;
                    lblModel2.Text = lblModel.Text;
                    lblVariant2.Text = lblVariant.Text;
                    lblRegistrationNumber.Text = txtRN1.Text.Trim() + " " + txtRN2.Text.Trim() + " " + txtRN3.Text.Trim() + " " + txtRN4.Text.Trim();
                    lblEngineNumber.Text = txtEngineNumber.Text.Trim();
                    lblChassisNumber.Text = txtChassisNumber.Text.Trim();
                    txtEmailId.Text = rbtIndividual.Checked ? txtEmailAddress.Text.Trim() : txtEmailIdOrz.Text.Trim();

                    lblPDFCustomerId.Text = CustomerId.ToString();
                    lblPDFProposalNumber.Text = ProposalNumber;
                    lblCreditScoreCustomerName.Text = CustomerName; //rbtIndividual.Checked ? (txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim()) : txtOrganizationName.Text.Trim(); // CustomerName; change back to CustomerName when sanjay sir says
                    lblCustomerIDProofNumber.Text = lblCustomerIDProofNumber.Text.Length <= 1 ? txtPanNumber.Text.Trim() : lblCustomerIDProofNumber.Text;


                    DownloadPDF(IsOnlySavePDF: true, MaxQuoteVersion: hdnMaxQuoteVersion.Value, IsNewBusiness: IsNewBusiness, RegistrationDate: RegistrationDate, IRDA_ProductCode: IRDA_ProductCode, IsProposalPDF: true, TenureOwnerDriver: TenureOwnerDriver);

                    if (!string.IsNullOrEmpty(objServiceResult.UserData.ProposalNumber))
                    {
                        if (Convert.ToDecimal(objServiceResult.UserData.TotalPremium).ToIndianCurrencyFormat() == lblTotalPremium.Text.Trim())
                        {
                            lblStatusSaveProposalSuccess.Text = "Proposal Saved Successfully !!!"; // Underwriting Approval is Pending..";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposalSuccess();", true);
                        }
                        else
                        {
                            lblStatusSaveProposalSuccess.Text = "Proposal saved successfully but there is <b>mismatch</b> between calculated premium and Premium from save proposal";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposalSuccess();", true);
                        }

                        //SAVE PROPOSAL NUMBER AND CUSTOMER ID AND QUOTE NUMBER IN DATABASE AND TOTAL PREMIUM
                        //SaveProposalDetails(lblQuoteNumber.Text.Trim().Substring(11, lblQuoteNumber.Text.Length - 11), ProposalNumber, CustomerId.ToString(), objServiceResult.UserData.TotalPremium.ToString());

                        string CGSTAmount = objServiceResult.UserData.CGSTAmount.ToString();
                        string CGSTPercentage = objServiceResult.UserData.CGSTPercentage.ToString();
                        string SGSTAmount = objServiceResult.UserData.SGSTAmount.ToString();
                        string SGSTPercentage = objServiceResult.UserData.SGSTPercentage.ToString();
                        string IGSTAmount = objServiceResult.UserData.IGSTAmount.ToString();
                        string IGSTPercentage = objServiceResult.UserData.IGSTPercentage.ToString();
                        string UGSTAmount = objServiceResult.UserData.UGSTAmount.ToString();
                        string UGSTPercentage = objServiceResult.UserData.UGSTPercentage.ToString();
                        string TotalGSTAmount = objServiceResult.UserData.TotalGSTAmount.ToString();
                        string ServiceTax = objServiceResult.UserData.ServiceTax.ToString();

                        string ApplicationNumber = objServiceResult.UserData.ApplicationNumber.ToString();

                        string strQuoteNo = lblQuoteNumber.Text.Replace("Quote No.: ", ""); //CODEMMC
                        strQuoteNo = strQuoteNo.Split(' ')[0]; //CODEMMC // ADDED THIS LINE TO PRECAUTIONARY CHECK IF QUOTE NUMBER HAS SPACE DUE TO MARKET MOVEMENT APPEN THEN GET THE FIRST PART IF ANY SAVE INTO DATABASE

                        SaveProposalDetails(strQuoteNo.Trim(), ProposalNumber, CustomerId.ToString(), objServiceResult.UserData.TotalPremium.ToString()
                            , CGSTAmount, CGSTPercentage, SGSTAmount, SGSTPercentage, IGSTAmount, IGSTPercentage, UGSTAmount, UGSTPercentage, TotalGSTAmount, ServiceTax, ApplicationNumber);

                        string WarningText = TruncateWarningText(objServiceResult.UserData.WarningText.Trim());
                        WarningText = WarningText.Replace("|", " ");
                        WarningText = WarningText.Replace("Risk Score:", "|Risk Score:").Replace("Risk Rate:", "|Risk Rate:");
                        string[] ArrWarnText = WarningText.Split('|');
                        Regex regex = new Regex("[0-9]");

                        WarningText = regex.Replace(WarningText, "");

                        if (ArrWarnText != null)
                        {
                            if (ArrWarnText.Length == 3)
                            {
                                string RiskScore = ArrWarnText[1];
                                string RiskRate = ArrWarnText[2];
                                string[] RiskRateArr = ArrWarnText[2].Split(' ');
                                if (RiskRateArr != null)
                                {
                                    if (RiskRateArr.Length > 0)
                                    {
                                        RiskRate = RiskRateArr[0] + " " + RiskRateArr[1];
                                        WarningText = RiskScore + "" + RiskRate;
                                    }
                                }
                            }
                        }

                        while (WarningText.Contains("Proposal will be created under Campaign"))
                        {
                            WarningText = WarningText.Replace("Proposal will be created under Campaign", "");
                        }
                        if (WarningText.Trim().Length > 15) //expecting atleast 15 char warning
                        {
                            if (WarningText.Contains("The Proposal has been referred for Credit Score discount"))
                            {
                                hdnIsWarningPresent.Value = "0"; //IF CREDIT SCORE WARNING THEN ALLOW SENDING PAYMENT LINK OR REVIEW CONFIRM LINK - (THIS REQUIRMENT GIVEN AND IMPLEMENTED ON 01-SEP-2017)
                                lblWarningTextForProposalSuccessPopUp.Text = "Status: This Proposal will be referred for Credit Score Discount.";
                            }
                            else if (WarningText.ToLower().Contains("proposal will be created under campaign"))
                            {
                                hdnIsWarningPresent.Value = "0"; //IF CREDIT SCORE WARNING THEN ALLOW SENDING PAYMENT LINK OR REVIEW CONFIRM LINK - (THIS REQUIRMENT GIVEN AND IMPLEMENTED ON 01-SEP-2017)
                                lblWarningTextForProposalSuccessPopUp.Text = "Status: " + WarningText;
                            }
                            else if (WarningText.ToLower().Contains("risk score"))
                            {
                                hdnIsWarningPresent.Value = "0"; //IF CREDIT SCORE WARNING THEN ALLOW SENDING PAYMENT LINK OR REVIEW CONFIRM LINK - (THIS REQUIRMENT GIVEN AND IMPLEMENTED ON 01-SEP-2017)
                                lblWarningTextForProposalSuccessPopUp.Text = "Status: " + WarningText;
                            }
                            else
                            {
                                hdnIsWarningPresent.Value = "1"; //IF ANY OTHER WARNING PRESENT THEN DO NO SENT THE PAYMENT OR REVIEW CONFIRM LINK
                                lblWarningTextForProposalSuccessPopUp.Text = "Status: " + WarningText;
                                trOptionButtonRowForLinkSendingOptions.Visible = false;
                                trEmailIdAndRemarksRow.Visible = false;
                                modalProposalSuccessFooter.Visible = true;
                            }
                        }

                        if (IsSendPaymentLink == false)
                        {
                            hdnIsWarningPresent.Value = "1"; //IF ANY OTHER WARNING PRESENT THEN DO NO SENT THE PAYMENT OR REVIEW CONFIRM LINK
                            lblWarningTextForProposalSuccessPopUp.Text = "Status: " + "Since Policy start date is less than today’s date, option to send digital payment link is disabled.";
                            trOptionButtonRowForLinkSendingOptions.Visible = false;
                            trEmailIdAndRemarksRow.Visible = false;
                            modalProposalSuccessFooter.Visible = true;
                        }


                       
                        CreateQuotePDF objCreateQuotePDF = new CreateQuotePDF();
                        QuotePDFParams objQuotePDFParams = new QuotePDFParams();
                        objQuotePDFParams.ProductCode = 3121;
                        objQuotePDFParams.QuoteNumber = lblQuoteNum.Text;
                        objQuotePDFParams.chkIsGetCreditScore = true; // chkIsGetCreditScore.Checked;
                        objQuotePDFParams.CustomerName = CustomerName;
                        objQuotePDFParams.CreditScoreCustomerName = CustomerName;
                        objQuotePDFParams.drpDrivingLicenseNumberOrAadhaarNumber = "PAN";
                        objQuotePDFParams.txtDrivingLicenseNumberOrAadhaarNumber = lblCustomerIDProofNumber.Text;
                        objQuotePDFParams.txtRTOAuthorityCode = lblRTO.Text; // txtRTOAuthorityCode.Text.Trim();
                        objQuotePDFParams.rbbtRollOver = IsNewBusiness == false ? true : false;
                        objQuotePDFParams.rbctIndividual = rbtIndividual.Checked;
                        objQuotePDFParams.txtMobileNumber = rbtIndividual.Checked ? txtMobileNumber.Text : txtMobileNumberOrz.Text;
                        objQuotePDFParams.drpTenureOwnerDriver = TenureOwnerDriver;
                        objQuotePDFParams.chkDepreciationCover = IsDepreciationCover; // chkDepreciationCover.Checked;
                        objQuotePDFParams.chkDailyCarAllowance = IsDailyCarAllowance; // chkDailyCarAllowance.Checked;
                        objQuotePDFParams.ddlDailyCarAllowance = ddlDailyCarAllowance;
                        objQuotePDFParams.chkKeyReplacement = IsKeyReplacement; // chkKeyReplacement.Checked;
                        objQuotePDFParams.ddlKeyReplacement = ddlKeyReplacement;
                        objQuotePDFParams.chkLossofPersonalBelongings = IsLossofPersonalBelongings; // chkLossofPersonalBelongings.Checked;
                        objQuotePDFParams.ddlLossofPersonalBelongingsSI = ddlLossofPersonalBelongingsSI;
                        objQuotePDFParams.drpVDA = drpVDA; //Voluntary Deductible Amount
                        objQuotePDFParams.rbbtNewBusiness = IsNewBusiness;
                        objQuotePDFParams.txtDateOfRegistration = RegistrationDate;
                        objQuotePDFParams.drpProductType = IRDA_ProductCode; // drpProductType.SelectedValue;

                        objQuotePDFParams.NetPremium = Convert.ToDecimal(objServiceResult.UserData.NetPremium).ToIndianCurrencyFormat();
                        objQuotePDFParams.ServiceTax = objServiceResult.UserData.ServiceTax.ToString();
                        objQuotePDFParams.TotalPremium = Convert.ToDecimal(objServiceResult.UserData.TotalPremium).ToIndianCurrencyFormat();
                        objQuotePDFParams.CGSTAmount = objServiceResult.UserData.CGSTAmount.ToString();
                        objQuotePDFParams.CGSTPercentage = objServiceResult.UserData.CGSTPercentage.ToString();
                        objQuotePDFParams.SGSTAmount = objServiceResult.UserData.SGSTAmount.ToString();
                        objQuotePDFParams.SGSTPercentage = objServiceResult.UserData.SGSTPercentage.ToString();
                        objQuotePDFParams.IGSTAmount = objServiceResult.UserData.IGSTAmount.ToString();
                        objQuotePDFParams.IGSTPercentage = objServiceResult.UserData.IGSTPercentage.ToString();
                        objQuotePDFParams.UGSTAmount = objServiceResult.UserData.UGSTAmount.ToString();
                        objQuotePDFParams.UGSTPercentage = objServiceResult.UserData.UGSTPercentage.ToString();
                        objQuotePDFParams.TotalGSTAmount = Convert.ToDecimal(objServiceResult.UserData.TotalGSTAmount).ToIndianCurrencyFormat();
                        objQuotePDFParams.MaxQuoteVersion = Convert.ToInt32(hdnMaxQuoteVersion.Value);
                        
                        objQuotePDFParams.PercentServiceTax = lblPercentServiceTax.Text;
                        objQuotePDFParams.TotalPremiumKerala = "0"; // lblTotalPremiumKerala.Text; //CR775A - Asked to remove kerala cess - Hasmukh

                        objQuotePDFParams.VDEPCoverAmount = ConfigurationManager.AppSettings["VDEPCoverAmount"].ToString();
                        objQuotePDFParams.CustomerId = lblPDFCustomerId.Text; // = CustomerId.ToString();
                        objQuotePDFParams.ProposalNumber = lblPDFProposalNumber.Text; // = ProposalNumber;
                        objQuotePDFParams.CustomerEmailId = rbtIndividual.Checked ? txtEmailAddress.Text : txtEmailIdOrz.Text;

                        objCreateQuotePDF.SaveQuotePDF(strQuoteNo, SaveProposalRequestXML, objServiceResult.UserData.UserResultXml, objQuotePDFParams);


                    }
                }
                ViewState["objQuoteDetails"] = null; //setting it null here so that gridiview can fetch data from database intead of viewstate itself
                QuoteGridView.DataBind();
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
            }

        }

        private string GetRequestXML_ThinCustomer()
        {
            string strXmlPath = "";
            strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "ThinCustomer.xml";

            XmlNode node = null;
            XmlDocument xmlfile = null;
            string xmlString = "";
            try
            {
                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);
                string strDOB = string.Empty;

                if (rbtIndividual.Checked)
                {
                    DateTime dtDOB = DateTime.ParseExact(txtDateofBirth.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    strDOB = dtDOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/IsThinCustomer");
                node.Attributes["Value"].Value = "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/FirstName");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? txtFirstName.Text.Trim() : txtOrganizationName.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MiddleName");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? txtMiddleName.Text.Trim() : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/LastName");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? txtLastName.Text.Trim() : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/DOB");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? strDOB : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/EmailId");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? txtEmailAddress.Text.Trim() : txtEmailIdOrz.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MobileNo");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? txtMobileNumber.Text.Trim() : txtMobileNumberOrz.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/CustomerType");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? "I" : "C";

                // CR 145 Starts here

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MaritalStatus");
                node.Attributes["Value"].Value = ddlMaritalStatus.SelectedValue.ToString();

                if (rbtIndividual.Checked && !string.IsNullOrEmpty(txtUID.Text.Trim()))
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/IDProof");
                    node.Attributes["Value"].Value = "7";  // For Adhar card value is 7

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/IDProofDetails");
                    node.Attributes["Value"].Value = txtUID.Text.Trim();
                }
                else if (rbtIndividual.Checked && string.IsNullOrEmpty(txtUID.Text.Trim()))
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/IDProof");
                    node.Attributes["Value"].Value = "3";  // For voter Icard value is 3

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/IDProofDetails");
                    node.Attributes["Value"].Value = "99999";

                }
                // CR 145 ends here

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/Gender");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? (rbtMale.Checked ? "Male" : "Female") : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/Salutation");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? drpCustomerTitle.SelectedItem.Text : "M/s";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/ContactPerson");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? "" : txtContactPerson.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/PanNo");
                node.Attributes["Value"].Value = txtPanNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/PinCode");
                node.Attributes["Value"].Value = txtPinCode.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/AddressLine1");
                node.Attributes["Value"].Value = txtAddressLine1.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/AddressLine2");
                node.Attributes["Value"].Value = txtAddressLine2.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/AddressLine3");
                node.Attributes["Value"].Value = txtAddressLine3.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/PinCode");
                node.Attributes["Value"].Value = txtPinCode.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/PinCodeLocality");
                node.Attributes["Value"].Value = lblPincodeLocality.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/StateCode");
                node.Attributes["Value"].Value = hdnStateId.Value;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/StateName");
                node.Attributes["Value"].Value = lblStateName.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/CityDistrictCode");
                node.Attributes["Value"].Value = hdnDistrictId.Value;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/CityDistrictName");
                node.Attributes["Value"].Value = lblDistrictName.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/CityId");
                node.Attributes["Value"].Value = hdnCityId.Value;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/MailLocation/CityName");
                node.Attributes["Value"].Value = lblCityName.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/CustomerDetails/PermanentLocation/MobileNo");
                node.Attributes["Value"].Value = rbtIndividual.Checked ? txtMobileNumber.Text.Trim() : txtMobileNumberOrz.Text.Trim();

                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetRequestXML_ThinCustomer Method");
            }
            return xmlString;
        }

        private void SaveInvoiceLink(string PayInvoiceURL, string ShortURL, string CustomerEmailId, string QuoteNumber, string CustomerMobileNumber, string TransactionId)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_PAYU_INVOICE_LINK";

                        cmd.Parameters.AddWithValue("@QuoteNumber", QuoteNumber);
                        cmd.Parameters.AddWithValue("@CustomerEmailId", CustomerEmailId);
                        cmd.Parameters.AddWithValue("@PayInvoiceURL", PayInvoiceURL);

                        cmd.Parameters.AddWithValue("@CustomerMobileNumber", CustomerMobileNumber);
                        cmd.Parameters.AddWithValue("@ShortURL", ShortURL);

                        cmd.Parameters.AddWithValue("@TransactionId", TransactionId);
                        cmd.Parameters.AddWithValue("@MaxQuoteVersion", Convert.ToInt16(hdnMaxQuoteVersion.Value));

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Error Occured, " + ex.Message);
                ExceptionUtility.LogException(ex, "SaveInvoiceLink Method");
            }
        }

        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private void GoogleURLShortner2(string longURL, out string shortURL)
        {
            shortURL = string.Empty;
            // If we did not construct the service so far, do it now.
            if (service == null)
            {
                BaseClientService.Initializer initializer = new BaseClientService.Initializer();
                // You can enter your developer key for services requiring a developer key.
                initializer.ApiKey = "AIzaSyAUDi9QAlEmAPy1lhPwfEHxDeXiQtnKgUI";
                service = new UrlshortenerService(initializer);
            }

            string url = longURL;
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            // Execute methods on the UrlShortener service based upon the type of the URL provided.
            try
            {
                string IsUseNetworkProxy = ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString();

                string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();

                string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();
                string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();

                WebRequest myWebRequest = WebRequest.Create("http://www.googleapis.com/");
                if (IsUseNetworkProxy == "1")
                {
                    WebProxy proxy = new WebProxy(proxy_Address_Full);
                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;
                    myWebRequest.Proxy = proxy;

                    /////
                    myWebRequest.Proxy = WebRequest.DefaultWebProxy;
                    myWebRequest.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    myWebRequest.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                }

                string resultURL;
                if (IsShortUrl(url))
                {
                    // Expand the URL by using a Url.Get(..) request.
                    Url result = service.Url.Get(url).Execute();
                    resultURL = result.LongUrl;
                }
                else
                {
                    // Shorten the URL by inserting a new Url.
                    Url toInsert = new Url { LongUrl = url };
                    toInsert = service.Url.Insert(toInsert).Execute();
                    resultURL = toInsert.Id;
                }
                shortURL = resultURL;
                //string s = string.Format("<a href=\"{0}\">{0}</a>", resultURL);

                WebRequest.DefaultWebProxy = null;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
            }
        }

        private void GoogleURLShortner(string longURL, out string shortURL)
        {
            shortURL = string.Empty;
            try
            {
                string ErrorMsg = string.Empty;
                WebRequest.DefaultWebProxy = null;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                PdfService.KGIServiceClient proxy = new PdfService.KGIServiceClient();
                string UserId = ConfigurationManager.AppSettings["UserIdForShortURL"].ToString();
                string AccessKey = ConfigurationManager.AppSettings["AccessKeyForShortURL"].ToString();
                proxy.ConvertLongURLToShortURL(UserId, AccessKey, longURL, out shortURL, out ErrorMsg);
                proxy.Close();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
            }
        }

        private string Create_Invoice(string TransactionId, string TotalPremium, string CsutomerName, string EmailAddress, string MobileNumber, string MakeModelVariant, string ProposalNumber)
        {
            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Error.txt", "TotalPremium: " + TotalPremium);
            //string logFile = AppDomain.CurrentDomain.BaseDirectory + "/Error.txt";
            //StreamWriter sw = new StreamWriter(logFile, true);
            //sw.WriteLine("TotalPremium: " + TotalPremium);
            //sw.Close();

            string URL = string.Empty;

            var obj = new RootObject
            {
                amount = TotalPremium //.Replace(",", "")
                ,
                txnid = ProposalNumber //TransactionId //QuoteNumber.Replace(" -7", "") + "MAPP"
                ,
                productinfo = "Created By " + Session["vUserLoginId"].ToString().ToUpper() + " " + Session["vUserLoginDesc"].ToString().ToUpper() //, Proposal Number:" + lblProposalNumber.Text + ", Variant: " + MakeModelVariant + ""
                ,
                firstname = CsutomerName //"Hasmukh"
                ,
                email = EmailAddress //"kgi.hasmukh-jain@kotak.com"
                ,
                phone = MobileNumber //"7738284116" //"7738284116" //8588819411 = sushil tomhar
                ,
                address1 = ""
                ,
                city = ""
                ,
                state = ""
                ,
                country = ""
                ,
                zipcode = ""
                //,                template_id = "14"
                ,
                validation_period = ConfigurationManager.AppSettings["validation_period"].ToString()
                ,
                send_email_now = "0"
                ,
                send_sms = "0"
            };
            var json = new JavaScriptSerializer().Serialize(obj);
            Console.WriteLine(json);

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                string Url = ConfigurationManager.AppSettings["PayUWebService"].ToString();

                string method = "create_invoice";
                string salt = ConfigurationManager.AppSettings["salt"].ToString();
                string key = ConfigurationManager.AppSettings["key"].ToString();
                //string var1 = "{"amount":"10","txnid":"abc3332","productinfo":"jnvjrenv","firstname":"test","email":"test @test.com","phone":"1234567890","address1":"testaddress","city":"test","state":"test","country":"test","zipcode":"122002","template_id":"14","validation_period":6,"send_email_now":"1"}";
                string var1 = json; //"{'amount':'10','txnid':'abc3332','productinfo':'jnvjrenv','firstname':'Hasmukh','email':'kgi.hasmukh-jain@kotak.com','phone':'7738284116','address1':'testaddress','city':'test','state':'test','country':'test','zipcode':'122002','template_id':'14','validation_period':1,'send_email_now':'1'}";
                string var2 = " ";//TokenID of the merchant
                string var3 = " ";//Amount to be use in refund

                string toHash = key + "|" + method + "|" + var1 + "|" + salt;

                string Hashed = Generatehash512(toHash);

                string postString = "key=" + key +
                    "&command=" + method +
                    "&hash=" + Hashed +
                    "&var1=" + var1 +
                    "&var2=" + var2 +
                    "&var3=" + var3 +
                    "&udf1=" + "testUDF";

                string IsUseNetworkProxy = ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString();

                string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();

                string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();
                string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();

                WebRequest myWebRequest = WebRequest.Create(Url);
                if (IsUseNetworkProxy == "1")
                {
                    WebProxy proxy = new WebProxy(proxy_Address_Full);
                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;
                    myWebRequest.Proxy = proxy;

                    /////
                    myWebRequest.Proxy = WebRequest.DefaultWebProxy;
                    myWebRequest.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    myWebRequest.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                }

                myWebRequest.Method = "POST";
                myWebRequest.ContentType = "application/x-www-form-urlencoded";
                myWebRequest.Timeout = 180000;
                StreamWriter requestWriter = new StreamWriter(myWebRequest.GetRequestStream());
                requestWriter.Write(postString);
                requestWriter.Close();

                //StreamReader responseReader = new StreamReader(myWebRequest.GetResponse().GetResponseStream());
                WebResponse myWebResponse = myWebRequest.GetResponse();
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);

                string response = readStream.ReadToEnd();
                if (IsJson(response))
                {
                    JObject account = JObject.Parse(response);
                    Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                    URL = values["URL"];
                }
                else
                {
                    //Response.Write(response);
                    string strPath = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                    File.WriteAllText(strPath, "Error: " + response);
                }
            }
            catch (JsonReaderException jex)
            {
                ExceptionUtility.LogException(jex, "Create_Invoice Method");
            }
            catch (Exception ex)
            {
                string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                ExceptionUtility.LogException(ex, "Create_Invoice Method " + network_domain);
            }
            return URL;
        }

        private string Create_ReviewAndConfirmLink(string QuoteNumber, string ProposalNumber)
        {
            string FinalURL = string.Empty;
            try
            {
                string ReviewAndConfirmLink = ConfigurationManager.AppSettings["ReviewAndConfirmLink"].ToString() + Encryption.EncryptText(ProposalNumber) + "&vPayerType=1&vSourceSystem=PASS&vPolicyno=NA";
                string googleShortURL = string.Empty;
                GoogleURLShortner(ReviewAndConfirmLink, out googleShortURL);
                string shortURL = googleShortURL;

                bool checkIsShortURL = IsShortUrl(shortURL);
                FinalURL = checkIsShortURL ? shortURL : ReviewAndConfirmLink;

                SaveReviewConfirmLink(QuoteNumber, ProposalNumber, ReviewAndConfirmLink, shortURL);
            }
            catch (Exception ex)
            {
                FinalURL = "";
                ExceptionUtility.LogException(ex, "Create_ReviewAndConfirmLink Method");
            }

            return FinalURL;
        }

        private void SaveReviewConfirmLink(string QuoteNumber, string ProposalNumber, string ReviewAndConfirmLink, string ReviewAndConfirmShortURL)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_REVIEW_CONFIRM_LINK";

                        cmd.Parameters.AddWithValue("@QuoteNumber", QuoteNumber);
                        cmd.Parameters.AddWithValue("@ProposalNumber", ProposalNumber);
                        cmd.Parameters.AddWithValue("@ReviewAndConfirmLink", ReviewAndConfirmLink);
                        cmd.Parameters.AddWithValue("@ReviewAndConfirmShortURL", ReviewAndConfirmShortURL);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString());
                        cmd.Parameters.AddWithValue("@MaxQuoteVersion", Convert.ToInt16(hdnMaxQuoteVersion.Value));
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveReviewConfirmLink Method");
            }
        }

        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        #region CodeForGridView

        IEnumerable<ProjectPASS.QuoteDetails> objQuoteDetails = null;
        public IEnumerable<ProjectPASS.QuoteDetails> QuoteGridView_GetData([Control("txtSearchQuoteNumber")] string QuoteNumber, int maximumRows, int startRowIndex, out int totalRowCount)
        {
            objQuoteDetails = GetQuoteDetails();
            int pageSize = maximumRows;
            int pageIndex = 0;
            //int totalCount = 0;
            totalRowCount = objQuoteDetails.Count();

            if (startRowIndex > 0)
            {
                pageIndex = (int)Math.Round(((double)startRowIndex / (double)pageSize));
            }

            return objQuoteDetails.OrderByDescending(x => x.QuoteDate).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public List<QuoteDetails> GetQuoteDetails()
        {
            List<QuoteDetails> objQuoteDetails = new List<QuoteDetails>();
            QuoteDetails objQuoteDetail = new QuoteDetails();
            if (ViewState["objQuoteDetails"] == null)
            {
                int NumberOfDaysQuoteDetailsRequired = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfDaysQuoteDetailsRequired"].ToString());
                string LoginUserId = Session["vUserLoginId"].ToString();
                string QuoteNumber = txtSearchQuoteNumber.Text;
                DateTime FromDate;
                if (string.IsNullOrEmpty(HdnFromDate.Value))
                {
                   // FromDate = DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    FromDate = DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", null);
                }
                else
                {
                    FromDate = DateTime.ParseExact(HdnFromDate.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                txtFromDate.Text = FromDate.ToString("dd/MM/yyyy");

                DateTime ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                string CustomerName = txtCustomerName.Text.Trim();

                List<string> IntrCds = new List<string>();
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandTimeout = 99000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_GET_QUOTE_DETAILS";

                        cmd.Parameters.AddWithValue("@NumberOfDaysQuoteDetailsRequired", NumberOfDaysQuoteDetailsRequired);
                        cmd.Parameters.AddWithValue("@LoginUserId", LoginUserId);
                        cmd.Parameters.AddWithValue("@QuoteNumber", string.IsNullOrEmpty(QuoteNumber) ? "" : QuoteNumber.Trim());

                        cmd.Parameters.AddWithValue("@FromDate", FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);
                        cmd.Parameters.AddWithValue("@CustomerName", CustomerName);

                        cmd.Connection = conn;
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                objQuoteDetail = this.CreateQuoteDetails(reader);
                                objQuoteDetails.Add(objQuoteDetail);
                            }
                        }
                        conn.Close();
                    }
                }

                ViewState["objQuoteDetails"] = objQuoteDetails;
                return objQuoteDetails;
            }
            else
            {
                objQuoteDetails = (List<QuoteDetails>)ViewState["objQuoteDetails"];
                return objQuoteDetails;
            }
        }

        private QuoteDetails CreateQuoteDetails(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("No Quote detail exist.");
            }

            return new QuoteDetails
            {
                QuoteNumber = Convert.ToString(reader["QuoteNumber"]),
                QuoteDate = Convert.ToDateTime(reader["CreatedDate"]),
                Make = Convert.ToString(reader["Manufacture"]),
                Model = Convert.ToString(reader["Model"]),
                Variant = Convert.ToString(reader["ModelVariant"]),
                TotalPremium = Convert.ToString(reader["TotalPremium"]),
                ProposalNumber = Convert.ToString(reader["ProposalNumber"]),
                PolicyStartDate = Convert.ToString(reader["PropPolicyEffectivedate_Fromdate_Mandatary"]),
                BusinessType = Convert.ToString(reader["PropGeneralProposalInformation_BusinessType_Mandatary"]),
                CustomerType = Convert.ToString(reader["CustomerType"]),
                PaymentStatus = Convert.ToString(reader["PaymentStatus"]),
                PolicyNumber = Convert.ToString(reader["Policy_Number"]),
                QuoteVersion = Convert.ToInt16(reader["QuoteVersion"]),
                IsProposalExistsForQuoteNumber = Convert.ToString(reader["IsProposalExistsForQuoteNumber"]),
                ReviewAndConfirmLink = Convert.ToString(reader["PaymentStatus"]).ToUpper() == "SUCCESS" ? "" : Convert.ToString(reader["ReviewAndConfirmLink"]),
                SourceQuoteCreator = Convert.ToString(reader["SourceQuoteCreator"]),
                Remarks = Convert.ToString(reader["Remarks"]),
                IsAllowPolicyStartDateEdit = Convert.ToString(reader["IsAllowPolicyStartDateEdit"]),
                CampaignCode = Convert.ToString(reader["CampaignCode"]),
                
            };
        }

        protected void QuoteGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            QuoteGridView.PageIndex = e.NewPageIndex;
            if (!string.IsNullOrEmpty(HdnFromDate.Value))
            {
                DateTime dtFromDate = DateTime.ParseExact(HdnFromDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                txtFromDate.Text = dtFromDate.ToString("dd/MM/yyyy");
            }
        }

        #endregion

        protected void QuoteGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {


                Reset();

                bool isNewBusiness = false;
                string NetPremium = string.Empty;
                string ServiceTax = string.Empty;
                string TotalPremium = string.Empty;
                string RequestXML = string.Empty;
                string ResultXML = string.Empty;
                string MarketMovement = string.Empty;
                long CreditScoreId = 0;
                string CreditScoreCustomerName = string.Empty;
                string CreditScoreIDProof = string.Empty;
                string CreditScoreIDProofNumber = string.Empty;

                string CGSTAmount = string.Empty;
                string CGSTPercentage = string.Empty;
                string SGSTAmount = string.Empty;
                string SGSTPercentage = string.Empty;
                string IGSTAmount = string.Empty;
                string IGSTPercentage = string.Empty;
                string UGSTAmount = string.Empty;
                string UGSTPercentage = string.Empty;
                string TotalGSTAmount = string.Empty;
                bool IsFastlaneFlow = false;

                string RegistrationDate = string.Empty;
                string IRDA_ProductCode = string.Empty;
                string TenureOwnerDriver = string.Empty;
                txtSearchQuoteNumber.Text = "";
                txtCustomerName.Text = "";
                int NumberOfDaysQuoteDetailsRequired = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfDaysQuoteDetailsRequired"].ToString());
                txtFromDate.Text = DateTime.Now.AddDays(NumberOfDaysQuoteDetailsRequired).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                bool IsCustomerMale = true;

                if (e.CommandName == "recalculate")
                {
                    GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int SelectedQuoteVersion = int.Parse(rowSelect.Cells[1].Text);
                    TextBox txtPolicyStartDate = rowSelect.FindControl("txtPolicyStartDate") as TextBox;
                    string EditedPolicyStartDate = string.Empty;

                    if (txtPolicyStartDate != null)
                    {
                        EditedPolicyStartDate = txtPolicyStartDate.Text;
                    }

                    hdnQuoteVersion.Value = SelectedQuoteVersion.ToString();

                    bool IsIndividual = true; string CustMobileNumber = ""; bool IsDepreciationCover = false; bool IsDailyCarAllowance = false; string ddlDailyCarAllowance = "0";
                    bool IsKeyReplacement = false; string ddlKeyReplacement = ""; bool IsLossofPersonalBelongings = false; string ddlLossofPersonalBelongingsSI = "0"; string drpVDA = "0";

                    hdnCreditScoreId.Value = "0";
                    btnOpenCustomerPopUp.Visible = true;
                    LinkButton lnkRecalculate = (LinkButton)e.CommandSource;
                    string QuoteNumber = lnkRecalculate.CommandArgument;
                    GetResultXMLFromDB(QuoteNumber, ref isNewBusiness, ref NetPremium, ref ServiceTax, ref TotalPremium, ref RequestXML, ref ResultXML, ref MarketMovement
                        , ref CreditScoreId, ref CreditScoreCustomerName, ref CreditScoreIDProof, ref CreditScoreIDProofNumber, ref CGSTAmount, ref CGSTPercentage
                        , ref SGSTAmount, ref SGSTPercentage, ref IGSTAmount, ref IGSTPercentage, ref UGSTAmount, ref UGSTPercentage, ref TotalGSTAmount, ref IsFastlaneFlow
                        , ref SelectedQuoteVersion, ref RegistrationDate, ref IRDA_ProductCode, ref IsCustomerMale, ref TenureOwnerDriver
                          , ref IsIndividual, ref CustMobileNumber, ref IsDepreciationCover, ref IsDailyCarAllowance, ref ddlDailyCarAllowance
            , ref IsKeyReplacement, ref ddlKeyReplacement, ref IsLossofPersonalBelongings, ref ddlLossofPersonalBelongingsSI, ref drpVDA);

                    ReCalculatePremium(RequestXML, QuoteNumber, MarketMovement, CreditScoreId, CreditScoreCustomerName, CreditScoreIDProof, CreditScoreIDProofNumber
                        , IsFastlaneFlow, SelectedQuoteVersion, isNewBusiness, RegistrationDate, IRDA_ProductCode, EditedPolicyStartDate, TenureOwnerDriver
                         ,  IsIndividual, CustMobileNumber,  IsDepreciationCover, IsDailyCarAllowance,  ddlDailyCarAllowance
            , IsKeyReplacement, ddlKeyReplacement, IsLossofPersonalBelongings, ddlLossofPersonalBelongingsSI, drpVDA);


                    rbtMale.Checked = IsCustomerMale ? true : false;
                    rbtFemale.Checked = IsCustomerMale ? false : true;

                }
                else if (e.CommandName == "QuoteNumber")
                {
                    //Determine the RowIndex of the Row whose Button was clicked.
                    GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int rowIndex = rowSelect.RowIndex;

                    int QuoteVersion = int.Parse(rowSelect.Cells[1].Text);
                    hdnQuoteVersion.Value = QuoteVersion.ToString();
                    hdnMaxQuoteVersion.Value = QuoteVersion.ToString(); //THIS IS REQUIRED BECAUSE PDF NEEDS TO BE DOWNLOADED OF THE SAME VERSION;
                                                                        //Reference the GridView Row.
                    GridViewRow row = QuoteGridView.Rows[rowIndex];

                    //Access Cell values.
                    //DateTime dtQuoteDate = Convert.ToDateTime(row.Cells[2].Text);
                    //btnOpenCustomerPopUp.Visible = dtQuoteDate.ToShortDateString() == DateTime.Now.ToShortDateString() ? true : false;
                    btnOpenCustomerPopUp.Visible = false;

                   
                    bool IsIndividual = true; string CustMobileNumber = ""; bool IsDepreciationCover = false; bool IsDailyCarAllowance = false; string ddlDailyCarAllowance = "0";
                    bool IsKeyReplacement = false; string ddlKeyReplacement = ""; bool IsLossofPersonalBelongings = false; string ddlLossofPersonalBelongingsSI = "0"; string drpVDA = "0";

                    GetResultXMLFromDB(e.CommandArgument.ToString(), ref isNewBusiness, ref NetPremium, ref ServiceTax, ref TotalPremium, ref RequestXML, ref ResultXML, ref MarketMovement, ref CreditScoreId, ref CreditScoreCustomerName, ref CreditScoreIDProof, ref CreditScoreIDProofNumber, ref CGSTAmount, ref CGSTPercentage, ref SGSTAmount, ref SGSTPercentage, ref IGSTAmount, ref IGSTPercentage, ref UGSTAmount, ref UGSTPercentage, ref TotalGSTAmount, ref IsFastlaneFlow, ref QuoteVersion, ref RegistrationDate, ref IRDA_ProductCode, ref IsCustomerMale, ref TenureOwnerDriver
                          , ref IsIndividual, ref CustMobileNumber, ref IsDepreciationCover, ref IsDailyCarAllowance, ref ddlDailyCarAllowance
            , ref IsKeyReplacement, ref ddlKeyReplacement, ref IsLossofPersonalBelongings, ref ddlLossofPersonalBelongingsSI, ref drpVDA);
                    bool IsRollover = isNewBusiness ? false : true;
                    SetResultXMLValuesToPopUpLabel(false, ResultXML, e.CommandArgument.ToString(), NetPremium, ServiceTax, TotalPremium, IsRollover, CreditScoreId, CreditScoreCustomerName, CreditScoreIDProof, CreditScoreIDProofNumber, CGSTAmount, CGSTPercentage, SGSTAmount, SGSTPercentage, IGSTAmount, IGSTPercentage, UGSTAmount, UGSTPercentage, TotalGSTAmount, QuoteVersion.ToString(), RequestXML);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalViewPremium();", true);
                }
                else if (e.CommandName == "modifyquote")
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton lnkModifyQuote = (LinkButton)e.CommandSource;
                    string QuoteNumber = lnkModifyQuote.CommandArgument;
                    int QuoteVersion = int.Parse(gvr.Cells[1].Text);
                    Response.Redirect("FrmPremiumCalculatorMotor.aspx?quotenumber=" + QuoteNumber + "&quoteversion=" + QuoteVersion, false);
                }
            }
            catch (Exception ex)
            {
                lblstatus.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        protected void btnSearchQuoteNumber_Click(object sender, EventArgs e)
        {
            DateTime dtFromDate = DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            HdnFromDate.Value = txtFromDate.Text.Trim();

            DateTime dtToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (dtFromDate.Date > dtToDate.Date)
            {
                lblstatus.Text = "From Date Cannot be greater than To Date";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else if (dtToDate.Date > DateTime.Now.Date)
            {
                lblstatus.Text = "To Date Cannot be greater than today's date";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                ViewState["objQuoteDetails"] = null; //setting it null here so that gridiview can fetch data from database intead of viewstate itself
                QuoteGridView.DataBind();
                txtFromDate.Text = dtFromDate.ToString("dd/MM/yyyy");
            }
        }

        protected void QuoteGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnProposalNumber = e.Row.FindControl("hdnProposalNumber") as HiddenField;
                LinkButton lnkRecalculate = e.Row.FindControl("lnkRecalculate") as LinkButton;
                LinkButton lnkModifyQuote = e.Row.FindControl("lnkModifyQuote") as LinkButton;
                HiddenField hdnIsProposalExistsForQuoteNumber = e.Row.FindControl("hdnIsProposalExistsForQuoteNumber") as HiddenField;

                HiddenField hdnIsAllowPolicyStartDateEdit = e.Row.FindControl("hdnIsAllowPolicyStartDateEdit") as HiddenField;

                TextBox txtPolicyStartDate = e.Row.FindControl("txtPolicyStartDate") as TextBox;
                Label lblGVPolicyStartDate = e.Row.FindControl("lblGVPolicyStartDate") as Label;

                if (hdnProposalNumber.Value.ToString().Trim().Length > 0)
                {
                    lnkRecalculate.Visible = false;
                    lnkModifyQuote.Visible = false;
                }

                if (hdnIsProposalExistsForQuoteNumber.Value.Trim() == "1")
                {
                    lnkRecalculate.Visible = false;
                    lnkModifyQuote.Visible = false;
                }

                if (hdnIsAllowPolicyStartDateEdit.Value.ToLower() == "true" && hdnIsProposalExistsForQuoteNumber.Value.Trim() != "1")
                {
                    txtPolicyStartDate.Visible = true;
                    lblGVPolicyStartDate.Visible = false;
                }
                else
                {
                    txtPolicyStartDate.Visible = false;
                    lblGVPolicyStartDate.Visible = true;
                }
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetPincode(string prefix)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_PINCODE";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IntrCds.Add(string.Format("{0}~{1}", sdr["NUM_PINCODE"], sdr["TXT_PINCODE_LOCALITY"]));
                        }
                    }
                    conn.Close();
                }
            }
            return IntrCds.ToArray();
        }



        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetFinacier(string prefix)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_FINANCIER_DETAILS";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IntrCds.Add(string.Format("{0}~{1}", sdr["NUM_FINANCIER_CD"], sdr["TXT_FINANCIER_NAME"]));
                        }
                    }
                    conn.Close();
                }
            }
            return IntrCds.ToArray();
        }


        protected void SetStateCityDistrict(string NUM_PINCODE, string TXT_PINCODE_LOCALITY)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_STATE_CITY_DISTRICT";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "NUM_PINCODE", DbType.String, ParameterDirection.Input, "@NUM_PINCODE", DataRowVersion.Current, NUM_PINCODE);
            db.AddParameter(dbCommand, "TXT_PINCODE_LOCALITY", DbType.String, ParameterDirection.Input, "@TXT_PINCODE_LOCALITY", DataRowVersion.Current, TXT_PINCODE_LOCALITY);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnStateId.Value = ds.Tables[0].Rows[0]["StateCode"].ToString();
                    lblStateName.Text = ds.Tables[0].Rows[0]["StateName"].ToString();
                    hdnDistrictId.Value = ds.Tables[0].Rows[0]["CityDistrictCode"].ToString();
                    lblDistrictName.Text = ds.Tables[0].Rows[0]["CityDistrictName"].ToString();
                    hdnCityId.Value = ds.Tables[0].Rows[0]["CityId"].ToString();
                    lblCityName.Text = ds.Tables[0].Rows[0]["CityName"].ToString();
                    hdnCreditScoreStateId.Value = ds.Tables[0].Rows[0]["CreditScore_StateCode"].ToString();
                }
            }
        }
        //SandeepCR919
        private void CheckFraudulentRegistrationNumber(string RegistrationNumber, out int IsInFraud_RegistrationNumber)
        {
            IsInFraud_RegistrationNumber = 0;
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnBPOS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_GET_Fraudulent_Registration_Number_STATUS";

                        cmd.Parameters.AddWithValue("@RegistrationNumber", RegistrationNumber);
                        
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(ds);
                    }
                }

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            
                           // IsInFraud_RegistrationNumber = ds.Tables[0].Rows[0]["IsInFraud_RegistrationNumber"];
                            IsInFraud_RegistrationNumber = Convert.ToInt32(ds.Tables[0].Rows[0]["IsInFraud_RegistrationNumber"].ToString());

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionUtility.LogException(ex, "CheckFraudulentRegistrationNumber");
            }
        }

        protected void btnGetPincodeDetails_Click(object sender, EventArgs e)
        {
            lblPincodeLocality.Text = hdnPinCodeLocality.Value;
            string pincode = hdnPinCode.Value;
            SetStateCityDistrict(hdnPinCode.Value, hdnPinCodeLocality.Value);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposal();", true);
        }

        protected void btnFinancierName_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalSaveProposal();", true);
        }
    }
}