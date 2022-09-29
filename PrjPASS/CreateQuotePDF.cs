using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace PrjPASS
{
    public class CreateQuotePDF
    {
        public void SaveQuotePDF(string QuoteId, string RequestXML, string ResultXML, QuotePDFParams objQuotePDFParams)
        {
            try
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "htmlbodytemplates/QuotePDF3121_WithoutProposalNumber.html";

                switch (objQuotePDFParams.ProductCode)
                {
                    case 3121:
                        //strPath = IsProposalCreated ? AppDomain.CurrentDomain.BaseDirectory + "htmlbodytemplates/QuotePDF3121_WithProposalNumber.html" : AppDomain.CurrentDomain.BaseDirectory + "htmlbodytemplates/QuotePDF3121_WithoutProposalNumber.html";
                        strPath = AppDomain.CurrentDomain.BaseDirectory + "htmlbodytemplates/QuotePDF3121.html";
                        break;
                    case 3151:
                        strPath = AppDomain.CurrentDomain.BaseDirectory + "htmlbodytemplates/QuotePDF3151.html";
                        break;

                }

                string htmlBody = File.ReadAllText(strPath);
                string strHtml = htmlBody;

                objQuotePDFParams = PrepareQuotePDFParams(RequestXML, ResultXML, objQuotePDFParams);
                strHtml = PrepareHTML(strHtml, objQuotePDFParams);

                string strErrorMsg = "";
                CreateQuoteScheduleHTMLtoPDF(strHtml, QuoteId + "_" + objQuotePDFParams.MaxQuoteVersion, out strErrorMsg);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void CreateQuoteScheduleHTMLtoPDF(string strHtml, string QuoteId, out string strErrorMsg)
        {
            clsConvertHtmlToPdf objHtmlToPdf = new clsConvertHtmlToPdf();
            try
            {
                objHtmlToPdf.IsWithoutHeaderFooter = false;
                byte[] outPdfBuffer = objHtmlToPdf.ConvertToPdfNew(strHtml, out strErrorMsg);
                if (string.IsNullOrEmpty(strErrorMsg))
                {
                    PDFave(outPdfBuffer, false, QuoteId, ref strErrorMsg);
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = "error";
                ExceptionUtility.LogException(ex, "CreateQuoteScheduleHTMLtoPDF");
            }

        }

        private void DigitalSignPDFAndSave(byte[] _outPdfBuffer, bool IsWithoutHeaderFooter, string QuoteId, ref string strErrorMsg)
        {
            try
            {
                string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();
                byte[] outPdfBuffer = IsWithoutHeaderFooter ? _outPdfBuffer : clsDigitalCertificate.Sign(_outPdfBuffer);
                File.WriteAllBytes(KotakQuotesPDFFiles + (IsWithoutHeaderFooter == false ? QuoteId + ".pdf" : QuoteId + "_PrintCopy.pdf"), outPdfBuffer); // Requires System.IO
                strErrorMsg = string.Empty;
            }
            catch (Exception ex)
            {
                strErrorMsg = "error";
                ExceptionUtility.LogException(ex, "DigitalSignPDFAndSave");
            }
        }

        private void PDFave(byte[] _outPdfBuffer, bool IsWithoutHeaderFooter, string QuoteId, ref string strErrorMsg)
        {
            try
            {
                string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();
                File.WriteAllBytes(KotakQuotesPDFFiles + (IsWithoutHeaderFooter == false ? QuoteId + ".pdf" : QuoteId + ".pdf"), _outPdfBuffer); // Requires System.IO
                strErrorMsg = string.Empty;
            }
            catch (Exception ex)
            {
                strErrorMsg = "error";
                ExceptionUtility.LogException(ex, "PDFave");
            }
        }


        private string PrepareHTML(string strHtml, QuotePDFParams objQuotePDFParams)
        {
            try
            {

                string ProductName = "Kotak Car Secure (Comprehensive Policy)";

                DateTime dt_NewBusinessCondition = DateTime.ParseExact("01/09/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (objQuotePDFParams.rbbtNewBusiness && DateTime.ParseExact(objQuotePDFParams.txtDateOfRegistration, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= dt_NewBusinessCondition.Date)
                {
                    switch (objQuotePDFParams.drpProductType)
                    {
                        case "1011":
                            ProductName = "Kotak Car Secure (Comprehensive Policy)";
                            break;
                        case "1062":
                            ProductName = "Kotak Car Secure – Bundled (Comprehensive Policy)";
                            break;
                        case "1063":
                            ProductName = "Kotak Car Secure – Bundled (Comprehensive Policy)";
                            break;
                    }
                }

                if (objQuotePDFParams.ProductCode == 3151)
                {
                    ProductName = "Kotak Car Secure - OD Only";
                }

                strHtml = strHtml.Replace("@CustomerEmailId", string.IsNullOrWhiteSpace(objQuotePDFParams.CustomerEmailId) ? "-" : objQuotePDFParams.CustomerEmailId);
                strHtml = strHtml.Replace("@CustomerId", string.IsNullOrWhiteSpace(objQuotePDFParams.CustomerId) ? "-" : objQuotePDFParams.CustomerId);
                strHtml = strHtml.Replace("@ProposalNumber", string.IsNullOrWhiteSpace(objQuotePDFParams.ProposalNumber) ? "-" : objQuotePDFParams.ProposalNumber);
                strHtml = strHtml.Replace("@QuoteNumber", objQuotePDFParams.QuoteNumber);
                strHtml = strHtml.Replace("@OwnDamagePremium", objQuotePDFParams.OwnDamagePremium);
                strHtml = strHtml.Replace("@CurrentDateTime", DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt"));
                strHtml = strHtml.Replace("@ProductName", ProductName);


                strHtml = strHtml.Replace("@APLUSBNetPremium", objQuotePDFParams.NetPremium);
                strHtml = strHtml.Replace("@NetPremium", objQuotePDFParams.NetPremium);
                strHtml = strHtml.Replace("@ServiceTax", objQuotePDFParams.ServiceTax);
                strHtml = strHtml.Replace("@TotalPremium", objQuotePDFParams.TotalPremium);
                strHtml = strHtml.Replace("@CGSTAmount", objQuotePDFParams.CGSTAmount);
                strHtml = strHtml.Replace("@CGSTPercentage", objQuotePDFParams.CGSTPercentage);
                strHtml = strHtml.Replace("@SGSTAmount", objQuotePDFParams.SGSTAmount);
                strHtml = strHtml.Replace("@SGSTPercentage", objQuotePDFParams.SGSTPercentage);
                strHtml = strHtml.Replace("@IGSTAmount", objQuotePDFParams.IGSTAmount);
                strHtml = strHtml.Replace("@IGSTPercentage", objQuotePDFParams.IGSTPercentage);
                strHtml = strHtml.Replace("@UGSTAmount", objQuotePDFParams.UGSTAmount);
                strHtml = strHtml.Replace("@UGSTPercentage", objQuotePDFParams.UGSTPercentage);
                strHtml = strHtml.Replace("@AmountGST", objQuotePDFParams.TotalGSTAmount);
                strHtml = strHtml.Replace("@GSTOrServiceTax", objQuotePDFParams.PercentServiceTax);
                strHtml = strHtml.Replace("@MaxQuoteVersion", objQuotePDFParams.MaxQuoteVersion.ToString());
                strHtml = strHtml.Replace("@KeralaTotalPremium", objQuotePDFParams.TotalPremiumKerala);

                strHtml = strHtml.Replace("@CustomerName", string.IsNullOrWhiteSpace(objQuotePDFParams.txtFirstName) ? objQuotePDFParams.CustomerName : objQuotePDFParams.txtFirstName + " " + objQuotePDFParams.txtLastName);

                strHtml = strHtml.Replace("@drpDrivingLicenseNumberOrAadhaarNumber", objQuotePDFParams.drpDrivingLicenseNumberOrAadhaarNumber);
                strHtml = strHtml.Replace("@txtDrivingLicenseNumberOrAadhaarNumber", objQuotePDFParams.txtDrivingLicenseNumberOrAadhaarNumber);
                strHtml = strHtml.Replace("@txtRTOAuthorityCode", objQuotePDFParams.txtRTOAuthorityCode);
                //strHtml = strHtml.Replace("@rbbtRollOver", objQuotePDFParams.rbbtRollOver);
                //strHtml = strHtml.Replace("@rbctIndividual", objQuotePDFParams.rbctIndividual);
                strHtml = strHtml.Replace("@txtMobileNumber", objQuotePDFParams.txtMobileNumber);
                strHtml = strHtml.Replace("@drpTenureOwnerDriver", objQuotePDFParams.drpTenureOwnerDriver);
                //strHtml = strHtml.Replace("@chkDailyCarAllowance", objQuotePDFParams.chkDailyCarAllowance);
                strHtml = strHtml.Replace("@ddlDailyCarAllowance", objQuotePDFParams.ddlDailyCarAllowance);
                // strHtml = strHtml.Replace("@chkKeyReplacement", objQuotePDFParams.chkKeyReplacement);
                strHtml = strHtml.Replace("@ddlKeyReplacement", objQuotePDFParams.ddlKeyReplacement);
                //  strHtml = strHtml.Replace("@chkLossofPersonalBelongings", objQuotePDFParams.chkLossofPersonalBelongings);
                strHtml = strHtml.Replace("@ddlLossofPersonalBelongingsSI", objQuotePDFParams.ddlLossofPersonalBelongingsSI);
                //  strHtml = strHtml.Replace("@rbbtNewBusiness", objQuotePDFParams.rbbtNewBusiness);
                strHtml = strHtml.Replace("@txtDateOfRegistration", objQuotePDFParams.txtDateOfRegistration);
                strHtml = strHtml.Replace("@drpProductType", objQuotePDFParams.drpProductType);

                strHtml = strHtml.Replace("@QuoteNumber", objQuotePDFParams.QuoteNumber);
                strHtml = strHtml.Replace("@OwnDamagePremium", objQuotePDFParams.OwnDamagePremium);



                strHtml = strHtml.Replace("@ProductType", objQuotePDFParams.ProductType);

                strHtml = strHtml.Replace("@MarketMovement", objQuotePDFParams.MarketMovement);
                strHtml = strHtml.Replace("@TPBasic", objQuotePDFParams.BasicTPPremium);
                strHtml = strHtml.Replace("@CNGLPGKitIDV", objQuotePDFParams.CNGLPGKitIDV);
                strHtml = strHtml.Replace("@ConsumableCover", string.IsNullOrWhiteSpace(objQuotePDFParams.ConsumableCover) ? "0.00" : objQuotePDFParams.ConsumableCover);
                strHtml = strHtml.Replace("@CoverType", objQuotePDFParams.CoverType);
                strHtml = strHtml.Replace("@CreditScore", objQuotePDFParams.CreditScore);
                strHtml = strHtml.Replace("@CreditScoreCustomerName", objQuotePDFParams.CreditScoreCustomerName);

                strHtml = strHtml.Replace("@CubicCapacity", objQuotePDFParams.CubicCapacity);
                strHtml = strHtml.Replace("@CustomerContactNo", objQuotePDFParams.CustomerContactNo);
                strHtml = strHtml.Replace("@CustomerGender", objQuotePDFParams.CustomerGender);
                strHtml = strHtml.Replace("@CustomerIDProof", objQuotePDFParams.CustomerIDProof);
                strHtml = strHtml.Replace("@PANNumber", objQuotePDFParams.CustomerIDProofNumber);



                strHtml = strHtml.Replace("@DepreciationCover", string.IsNullOrWhiteSpace(objQuotePDFParams.DepreciationCover) ? "0.00" : objQuotePDFParams.DepreciationCover);

                strHtml = strHtml.Replace("@DORorDOP", objQuotePDFParams.DORorDOP);


                strHtml = strHtml.Replace("@ElectricalAccessoriesIDV", string.IsNullOrWhiteSpace(objQuotePDFParams.ElectricalAccessoriesIDV) ? "0.00" : objQuotePDFParams.ElectricalAccessoriesIDV);

                strHtml = strHtml.Replace("@ElectronicItems", string.IsNullOrWhiteSpace(objQuotePDFParams.ElectronicSI) ? "0.00" : objQuotePDFParams.ElectronicSI);

                strHtml = strHtml.Replace("@EngineProtectCover", string.IsNullOrWhiteSpace(objQuotePDFParams.EngineProtect) ? "0.00" : objQuotePDFParams.EngineProtect);

                strHtml = strHtml.Replace("@ExternalBiFuelKit", string.IsNullOrWhiteSpace(objQuotePDFParams.ExternalBiFuelSI) ? "0.00" : objQuotePDFParams.ExternalBiFuelSI);


                strHtml = strHtml.Replace("@FinalIDV", objQuotePDFParams.FinalIDV);
                strHtml = strHtml.Replace("@FuelType", objQuotePDFParams.FuelType);
                strHtml = strHtml.Replace("@IMDCode", objQuotePDFParams.IMDCode);
                strHtml = strHtml.Replace("@LLPDCC", string.IsNullOrWhiteSpace(objQuotePDFParams.LegalLiabilityToPaidDriverNo) ? "0.00" : objQuotePDFParams.LegalLiabilityToPaidDriverNo);
                strHtml = strHtml.Replace("@LiabilityForBiFuel", string.IsNullOrWhiteSpace(objQuotePDFParams.LiabilityForBiFuel) ? "0.00" : objQuotePDFParams.LiabilityForBiFuel);
                strHtml = strHtml.Replace("@LLEPD", string.IsNullOrWhiteSpace(objQuotePDFParams.LLEOPDCC) ? "0.00" : objQuotePDFParams.LLEOPDCC);


                strHtml = strHtml.Replace("@KeyReplacement", string.IsNullOrWhiteSpace(objQuotePDFParams.KeyReplacement) ? "0.00" : objQuotePDFParams.KeyReplacement);

                if (objQuotePDFParams.chkKeyReplacement)
                {
                    strHtml = strHtml.Replace("@SIKeyReplacement", objQuotePDFParams.KeyReplacementSI);
                }
                else
                {
                    strHtml = strHtml.Replace("@SIKeyReplacement", "");
                }

                if (objQuotePDFParams.chkDailyCarAllowance)
                {
                    strHtml = strHtml.Replace("@SIDailyCarAllowance", objQuotePDFParams.lblDailyCarAllowanceSI);
                }
                else
                {
                    strHtml = strHtml.Replace("@SIDailyCarAllowance", "");
                }
                strHtml = strHtml.Replace("@DailyCarAllowance", string.IsNullOrWhiteSpace(objQuotePDFParams.DailyCarAllowance) ? "0.00" : objQuotePDFParams.DailyCarAllowance);

                strHtml = strHtml.Replace("@LossofPersonalBelongings", string.IsNullOrWhiteSpace(objQuotePDFParams.LossofPersonalBelongings) ? "0.00" : objQuotePDFParams.LossofPersonalBelongings);


                if (objQuotePDFParams.chkLossofPersonalBelongings)
                {
                    strHtml = strHtml.Replace("@SILossofPersonalBelongings", objQuotePDFParams.LossofPersonalBelongingsSI);
                }
                else
                {
                    strHtml = strHtml.Replace("@SILossofPersonalBelongings", "");
                }

                strHtml = strHtml.Replace("@Make", objQuotePDFParams.Make);
                strHtml = strHtml.Replace("@Model", objQuotePDFParams.Model);
                strHtml = strHtml.Replace("@NoClaimBonus", string.IsNullOrWhiteSpace(objQuotePDFParams.NCB) ? "0.00" : objQuotePDFParams.NCB);
                strHtml = strHtml.Replace("@PercentNCB", objQuotePDFParams.NCBPercentage);
                strHtml = strHtml.Replace("@NCBProtect", string.IsNullOrWhiteSpace(objQuotePDFParams.NCBProtect) ? "0.00" : objQuotePDFParams.NCBProtect);
                strHtml = strHtml.Replace("@NonElectricalAccessoriesIDV", objQuotePDFParams.NonElectricalAccessoriesIDV);
                strHtml = strHtml.Replace("@NonElectronicItems", string.IsNullOrWhiteSpace(objQuotePDFParams.NonElectronicSI) ? "0.00" : objQuotePDFParams.NonElectronicSI);
                strHtml = strHtml.Replace("@ODYearText", objQuotePDFParams.ODYearText);


                strHtml = strHtml.Replace("@OwnershipType", objQuotePDFParams.OwnershipType);
                strHtml = strHtml.Replace("@PACoverForOwner", objQuotePDFParams.PACoverForOwnerDriver);
                strHtml = strHtml.Replace("@PAForNamed", string.IsNullOrWhiteSpace(objQuotePDFParams.PAForNamedPassengerSI) ? "0.00" : objQuotePDFParams.PAForNamedPassengerSI);

                strHtml = strHtml.Replace("@PAForUnnamed", string.IsNullOrWhiteSpace(objQuotePDFParams.PAForUnnamedPassengerSI) ? "0.00" : objQuotePDFParams.PAForUnnamedPassengerSI);
                strHtml = strHtml.Replace("@PAToPaidDriver", string.IsNullOrWhiteSpace(objQuotePDFParams.PAToPaidDriverSI) ? "0.00" : objQuotePDFParams.PAToPaidDriverSI);

                strHtml = strHtml.Replace("@PolicyHolderType", objQuotePDFParams.PolicyHolderType);
                strHtml = strHtml.Replace("@PolicyStartDate", objQuotePDFParams.PolicyStartDate);

                strHtml = strHtml.Replace("@PremiumWithoutPAtoOwnerDriver", string.IsNullOrWhiteSpace(objQuotePDFParams.PremiumWithoutPAtoOwnerDriver) ? "0.00" : objQuotePDFParams.PremiumWithoutPAtoOwnerDriver);

                strHtml = strHtml.Replace("@PreviousPolicyExpiryDate", string.IsNullOrWhiteSpace(objQuotePDFParams.PreviousPolicyExpiryDate) ? "-" : objQuotePDFParams.PreviousPolicyExpiryDate);
                strHtml = strHtml.Replace("@RagistrationDate", objQuotePDFParams.RagistrationDate);

                strHtml = strHtml.Replace("@RateBasicOD", objQuotePDFParams.RateBasicOD);
                strHtml = strHtml.Replace("@RateCC", objQuotePDFParams.RateCC);
                strHtml = strHtml.Replace("@RateDC", objQuotePDFParams.RateDC);
                strHtml = strHtml.Replace("@RateDCA", objQuotePDFParams.RateDCA);
                strHtml = strHtml.Replace("@RateEP", objQuotePDFParams.RateEP);
                strHtml = strHtml.Replace("@RateKR", objQuotePDFParams.RateKR);
                strHtml = strHtml.Replace("@RateLOPB", objQuotePDFParams.RateLOPB);
                strHtml = strHtml.Replace("@RateNCBP", objQuotePDFParams.RateNCBP);
                strHtml = strHtml.Replace("@RateRTI", objQuotePDFParams.RateRTI);
                strHtml = strHtml.Replace("@RateTC", objQuotePDFParams.RateTC);
                strHtml = strHtml.Replace("@ReturntoInvoice", string.IsNullOrWhiteSpace(objQuotePDFParams.ReturnToInvoice) ? "0.00" : objQuotePDFParams.ReturnToInvoice);
                strHtml = strHtml.Replace("@RoadSideAssistance", string.IsNullOrWhiteSpace(objQuotePDFParams.RSA) ? "0.00" : objQuotePDFParams.RSA);
                strHtml = strHtml.Replace("@RTO", objQuotePDFParams.RTO);

                strHtml = strHtml.Replace("@SeatingCapacity", objQuotePDFParams.SeatingCapacity);
                strHtml = strHtml.Replace("@SystemIDV", objQuotePDFParams.SystemIDV);
                strHtml = strHtml.Replace("@TenureOwnerDriver", objQuotePDFParams.TenureOwnerDriver);

                strHtml = strHtml.Replace("@LiabilityTotalPremium", objQuotePDFParams.TotalPremiumLiability);
                strHtml = strHtml.Replace("@OwnDamageTotalPremium", objQuotePDFParams.TotalPremiumOwnDamage);
                strHtml = strHtml.Replace("@TPYearText", objQuotePDFParams.TPYearText);

                strHtml = strHtml.Replace("@TyreCover", string.IsNullOrWhiteSpace(objQuotePDFParams.TyreCover) ? "0.00" : objQuotePDFParams.TyreCover);
                strHtml = strHtml.Replace("@Variant", objQuotePDFParams.Variant);
                strHtml = strHtml.Replace("@VoluntaryDeduction", string.IsNullOrWhiteSpace(objQuotePDFParams.VoluntaryDeduction) ? "0.00" : objQuotePDFParams.VoluntaryDeduction);
                strHtml = strHtml.Replace("@VDforDepWaiver", string.IsNullOrWhiteSpace(objQuotePDFParams.VoluntaryDeductionforDepWaiver) ? "0.00" : objQuotePDFParams.VoluntaryDeductionforDepWaiver);

                if (objQuotePDFParams.chkDepreciationCover)
                {
                    strHtml = strHtml.Replace("[@VDEPCoverAmount]", "( Rs." + objQuotePDFParams.VDEPCoverAmount + ")");
                }
                else
                {
                    strHtml = strHtml.Replace("[@VDEPCoverAmount]", "");
                }

                if (objQuotePDFParams.rbctIndividual == false || objQuotePDFParams.drpTenureOwnerDriver != "0")
                {
                    strHtml = strHtml.Replace("[@CPAMsg]", "");
                }
                else
                {
                    strHtml = strHtml.Replace("[@CPAMsg]", "<li>This quote is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>");
                }

                if (objQuotePDFParams.drpProductType == "1062")
                {
                    strHtml = strHtml.Replace("[@ODOnlyCover]", "");
                }
                else
                {
                    strHtml = strHtml.Replace("[@ODOnlyCover]", "<li>OD only cover (including GST) Rs." + objQuotePDFParams.PremiumWithoutPAtoOwnerDriver) + "</li>";
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return strHtml;
        }

        private QuotePDFParams PrepareQuotePDFParams(string RequestXML, string ResultXML, QuotePDFParams objQuotePDFParams)
        {
            decimal PACoverForOwnerDriver = 0;
            decimal TotalPremiumOwnDamage = 0;
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
            objQuotePDFParams.RateBasicOD = "0.00";
            objQuotePDFParams.RateCC = "0.00";
            objQuotePDFParams.RateDC = "0.00";
            objQuotePDFParams.RateEP = "0.00";
            objQuotePDFParams.RateRTI = "0.00";

            string strlblDailyCarAllowance = "0.00";
            string strlblKeyReplacement = "0.00";
            string strlblTyreCover = "0.00";
            string strlblNCBProtect = "0.00";
            string strlblLossofPersonalBelongings = "0.00";

            objQuotePDFParams.RateDCA = "0.00";
            objQuotePDFParams.RateKR = "0.00";
            objQuotePDFParams.RateTC = "0.00";
            objQuotePDFParams.RateNCBP = "0.00";
            objQuotePDFParams.RateLOPB = "0.00";

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            XmlDocument xmlfile = null; DataTable dt = new DataTable();
            string xmlString = "";

            XmlNode SingleNode = null;

            xmlfile = new XmlDocument();
            xmlfile.LoadXml(ResultXML);

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_BasicODDeviation");
            strBasicODDeviation = SingleNode.InnerXml;

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_AddOnDeviation");
            strAddOnDeviation = SingleNode.InnerXml;

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_MarketMovement");
            objQuotePDFParams.MarketMovement = SingleNode.InnerXml;

            //objQuotePDFParams.QuoteNumber = strQuoteNo; //commenting CODEMMC
            //objQuotePDFParams.QuoteNumber = strQuoteNo + " " + txtMarketMovement.Trim() + " (" + MaxQuoteVersion.ToString() + ")"; //only append market movement to show in QUOTE PDF CODEMMC
            if (strAddOnDeviation.ToString() == "0" && strBasicODDeviation.ToString() == "0")
            {
                objQuotePDFParams.QuoteNumber = objQuotePDFParams.QuoteNumber + " " + objQuotePDFParams.MarketMovement.Trim() + " (" + objQuotePDFParams.MaxQuoteVersion.ToString() + ")";
            }
            else
            {
                objQuotePDFParams.QuoteNumber = objQuotePDFParams.QuoteNumber + " " + objQuotePDFParams.MarketMovement.Trim() + "," + strBasicODDeviation.ToString() + "," + strAddOnDeviation.ToString() + "  (" + objQuotePDFParams.MaxQuoteVersion.ToString() + ")"; //only append market movement to show in QUOTE PDF CODEMMC
            }

            string CampaignCode = "";
            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Extratxt1");
            if (SingleNode != null)
            {
                CampaignCode = !string.IsNullOrEmpty(SingleNode.InnerXml) ? SingleNode.InnerXml : "-";
                objQuotePDFParams.QuoteNumber = !string.IsNullOrEmpty(SingleNode.InnerXml) ? objQuotePDFParams.QuoteNumber + " (C)" : objQuotePDFParams.QuoteNumber;
            }

            if (objQuotePDFParams.chkIsGetCreditScore)
            {
                objQuotePDFParams.CreditScoreCustomerName = string.IsNullOrWhiteSpace(objQuotePDFParams.CreditScoreCustomerName) ? objQuotePDFParams.txtFirstName.Trim() + " " + objQuotePDFParams.txtLastName.Trim() : objQuotePDFParams.CreditScoreCustomerName;
                objQuotePDFParams.CustomerIDProof = objQuotePDFParams.drpDrivingLicenseNumberOrAadhaarNumber;
                objQuotePDFParams.CustomerIDProofNumber = objQuotePDFParams.txtDrivingLicenseNumberOrAadhaarNumber.Trim();
            }
            else
            {
                objQuotePDFParams.CreditScoreCustomerName = "-";
                objQuotePDFParams.CustomerIDProof = objQuotePDFParams.drpDrivingLicenseNumberOrAadhaarNumber;
                objQuotePDFParams.CustomerIDProofNumber = "-";
            }

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_IDVFlag");
            objQuotePDFParams.SystemIDV = Convert.ToDecimal(SingleNode.InnerXml).ToIndianCurrencyFormat();
            strSystemIDV = Convert.ToDecimal(SingleNode.InnerXml).ToString();


            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_IDVofthevehicle");
            objQuotePDFParams.FinalIDV = Convert.ToDecimal(SingleNode.InnerXml).ToIndianCurrencyFormat();
            strFinalIDV = Convert.ToDecimal(SingleNode.InnerXml).ToString();


            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_AuthorityLocation");
            objQuotePDFParams.RTO = SingleNode.InnerXml;

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_RTOCode");
            objQuotePDFParams.RTO = objQuotePDFParams.txtRTOAuthorityCode.Contains("Code") ? objQuotePDFParams.txtRTOAuthorityCode : "Code: " + SingleNode.InnerXml + " (" + objQuotePDFParams.RTO + " - " + objQuotePDFParams.txtRTOAuthorityCode.Trim() + ")";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropProductName");
            objQuotePDFParams.CoverType = objQuotePDFParams.ProductCode == 3151 ? "OD Only Policy" : SingleNode.InnerXml;

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropCustomerDtls_CustomerType");
            objQuotePDFParams.OwnershipType = SingleNode.InnerXml.ToString().ToLower() == "i" ? "Individual" : "Organization";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_CubicCapacity");
            objQuotePDFParams.CubicCapacity = SingleNode.InnerXml;

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_SeatingCapacity");
            objQuotePDFParams.SeatingCapacity = SingleNode.InnerXml;

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_TypeofPolicyHolder");
            objQuotePDFParams.PolicyHolderType = SingleNode.InnerXml;

            if (objQuotePDFParams.rbbtRollOver)
            {
                objQuotePDFParams.DORorDOP = "Registration Date";
                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_DateofRegistration");
                objQuotePDFParams.RagistrationDate = SingleNode.InnerXml;
            }
            else
            {
                objQuotePDFParams.DORorDOP = "Purchase Date";
                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Dateofpurchase");
                objQuotePDFParams.RagistrationDate = SingleNode.InnerXml;
            }
            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Manufacture");
            objQuotePDFParams.Make = SingleNode.InnerXml;

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Model");
            objQuotePDFParams.Model = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_ModelVariant");
            objQuotePDFParams.Variant = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_FuelType");
            objQuotePDFParams.FuelType = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropPolicyEffectivedate_Fromdate_Mandatary");
            objQuotePDFParams.PolicyStartDate = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_InsuredCreditScore");
            objQuotePDFParams.CreditScore = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_NonElectricalAccessories");
            objQuotePDFParams.NonElectricalAccessoriesIDV = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "0";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_ElectricalAccessories");
            objQuotePDFParams.ElectricalAccessoriesIDV = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "0";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_CNGLPGkitValue");
            objQuotePDFParams.CNGLPGKitIDV = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "0";

            objQuotePDFParams.CustomerContactNo = objQuotePDFParams.rbctIndividual ? objQuotePDFParams.txtMobileNumber : "";

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropIntermediaryDetails_IntermediaryCode");
            if (SingleNode != null)
            {
                objQuotePDFParams.IMDCode = !string.IsNullOrEmpty(SingleNode.InnerXml) ? SingleNode.InnerXml : "-";
            }

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_ExtraDD5");
            if (SingleNode != null)
            {
                objQuotePDFParams.CustomerGender = !string.IsNullOrEmpty(SingleNode.InnerXml) ? SingleNode.InnerXml : "-";
            }

            if (objQuotePDFParams.rbbtRollOver)
            {
                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropGeneralProposal_PreviousPolicyDetails_Col/GeneralProposal_PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyEffectiveTo");
                objQuotePDFParams.PreviousPolicyExpiryDate = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";
            }

            XmlNodeList nodeList = xmlfile.DocumentElement.SelectNodes("/ServiceResult/GetUserData/PropRisks_Col/Risks");

            foreach (XmlElement item in nodeList)
            {
                dt.Merge(ConvertXmlNodeListToDataTable(item["PropRisks_CoverDetails_Col"].ChildNodes));

                foreach (XmlNode node in item["PropRisks_CoverDetails_Col"].ChildNodes)
                {
                    string PropCoverDetails_CoverGroups = node["PropCoverDetails_CoverGroups"].InnerText;
                    string PropCoverDetails_Premium = node["PropCoverDetails_Premium"].InnerText;
                    string PropCoverDetails_Rate = node["PropCoverDetails_Rate"].InnerText;

                    PropCoverDetails_CoverGroups = regex.Replace(PropCoverDetails_CoverGroups, " ");

                    switch (PropCoverDetails_CoverGroups)
                    {
                        case "Basic TP including TPPD premium":
                            objQuotePDFParams.BasicTPPremium = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblBasicTPPremium = PropCoverDetails_Premium;
                            break;
                        case "Own Damage":
                            objQuotePDFParams.OwnDamagePremium = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblOwnDamagePremium = PropCoverDetails_Premium;
                            objQuotePDFParams.RateBasicOD = PropCoverDetails_Rate;
                            break;
                        case "Consumable Cover":
                            objQuotePDFParams.ConsumableCover = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblConsumableCover = PropCoverDetails_Premium;
                            objQuotePDFParams.RateCC = PropCoverDetails_Rate;
                            break;
                        case "Depreciation Cover":
                            objQuotePDFParams.DepreciationCover = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblDepreciationCover = PropCoverDetails_Premium;
                            objQuotePDFParams.RateDC = PropCoverDetails_Rate;
                            break;

                        case "Electronic Accessories OD":
                            objQuotePDFParams.ElectronicSI = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblElectronicSI = PropCoverDetails_Premium;
                            break;
                        case "Non Electrical Accessories OD":
                            objQuotePDFParams.NonElectronicSI = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblNonElectronicSI = PropCoverDetails_Premium;
                            break;
                        case "CNG Kit OD":
                            objQuotePDFParams.ExternalBiFuelSI = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblExternalBiFuelSI = PropCoverDetails_Premium;
                            break;
                        case "Engine Protect":
                            objQuotePDFParams.EngineProtect = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblEngineProtect = PropCoverDetails_Premium;
                            objQuotePDFParams.RateEP = PropCoverDetails_Rate;
                            break;
                        case "Return to Invoice":
                            objQuotePDFParams.ReturnToInvoice = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblReturnToInvoice = PropCoverDetails_Premium;
                            objQuotePDFParams.RateRTI = PropCoverDetails_Rate;
                            break;
                        case "Road Side Assistance":
                            objQuotePDFParams.RSA = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblRSA = PropCoverDetails_Premium;
                            break;

                        case "CNG Kit TP":
                            objQuotePDFParams.LiabilityForBiFuel = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblLiabilityForBiFuel = PropCoverDetails_Premium;
                            break;
                        case "Unnamed Passengers Personal Accident":
                            objQuotePDFParams.PAForUnnamedPassengerSI = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblPAForUnnamedPassengerSI = PropCoverDetails_Premium;
                            break;
                        case "Named Passengers Personal Accident":
                            objQuotePDFParams.PAForNamedPassengerSI = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblPAForNamedPassengerSI = PropCoverDetails_Premium;
                            break;
                        case "Paid Driver PA Cover":
                            objQuotePDFParams.PAToPaidDriverSI = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                            strlblPAToPaidDriverSI = PropCoverDetails_Premium;
                            break;
                        case "Owner Driver":
                            objQuotePDFParams.PACoverForOwnerDriver = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();

                            if (objQuotePDFParams.drpTenureOwnerDriver == "0")
                            {
                                objQuotePDFParams.TenureOwnerDriver = "";
                            }
                            else if (objQuotePDFParams.drpTenureOwnerDriver == "1")
                            {
                                objQuotePDFParams.TenureOwnerDriver = "1 Year";
                            }
                            else if (objQuotePDFParams.drpTenureOwnerDriver == "3")
                            {
                                objQuotePDFParams.TenureOwnerDriver = "3 Years";
                            }
                            strlblPACoverForOwnerDriver = PropCoverDetails_Premium;
                            break;
                        case "Legal Liability for paid driver cleaner conductor":
                            objQuotePDFParams.LegalLiabilityToPaidDriverNo = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblLegalLiabilityToPaidDriverNo = PropCoverDetails_Premium;
                            break;
                        case "Legal Liability for Employees other than paid driver conductor cleaner":
                            objQuotePDFParams.LLEOPDCC = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblLLEOPDCC = PropCoverDetails_Premium;
                            break;

                        case "Daily Car Allowance":
                            objQuotePDFParams.DailyCarAllowance = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblDailyCarAllowance = PropCoverDetails_Premium;
                            objQuotePDFParams.RateDCA = PropCoverDetails_Rate;
                            objQuotePDFParams.lblDailyCarAllowanceSI = objQuotePDFParams.chkDailyCarAllowance ? "(SI: Rs." + objQuotePDFParams.ddlDailyCarAllowance + ")" : "";
                            break;
                        case "Key Replacement":
                            objQuotePDFParams.KeyReplacement = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblKeyReplacement = PropCoverDetails_Premium;
                            objQuotePDFParams.RateKR = PropCoverDetails_Rate;
                            objQuotePDFParams.KeyReplacementSI = objQuotePDFParams.chkKeyReplacement ? "(SI: Rs." + objQuotePDFParams.ddlKeyReplacement + ")" : "";
                            break;
                        case "Tyre Cover":
                            objQuotePDFParams.TyreCover = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblTyreCover = PropCoverDetails_Premium;
                            objQuotePDFParams.RateTC = PropCoverDetails_Rate;
                            break;
                        case "NCB Protect":
                            objQuotePDFParams.NCBProtect = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblNCBProtect = PropCoverDetails_Premium;
                            objQuotePDFParams.RateNCBP = PropCoverDetails_Rate;
                            break;
                        case "Loss of Personal Belongings":
                            objQuotePDFParams.LossofPersonalBelongings = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                            strlblLossofPersonalBelongings = PropCoverDetails_Premium;
                            objQuotePDFParams.RateLOPB = PropCoverDetails_Rate;
                            objQuotePDFParams.LossofPersonalBelongingsSI = objQuotePDFParams.chkLossofPersonalBelongings ? "(SI: Rs." + objQuotePDFParams.ddlLossofPersonalBelongingsSI + ")" : "";
                            break;
                    }
                }
            }

            SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_OriginalNCBPercentage");
            objQuotePDFParams.NCBPercentage = SingleNode.InnerXml + "%";

            XmlNodeList nodeList_LoadingDiscount = xmlfile.DocumentElement.SelectNodes("/ServiceResult/GetUserData/PropLoadingDiscount_Col/LoadingDiscount");
            foreach (XmlElement item in nodeList_LoadingDiscount)
            {
                if (item["PropLoadingDiscount_Description"].InnerText == "No Claim Bonus")
                {
                    objQuotePDFParams.NCB = Convert.ToDecimal(item["PropLoadingDiscount_EndorsementAmount"].InnerText).ToIndianCurrencyFormat();
                    strlblNCB = item["PropLoadingDiscount_EndorsementAmount"].InnerText;
                }

                if (item["PropLoadingDiscount_Description"].InnerText == "Voluntary Excess Discount")
                {
                    objQuotePDFParams.VoluntaryDeduction = Convert.ToDecimal(item["PropLoadingDiscount_EndorsementAmount"].InnerText).ToIndianCurrencyFormat();
                    strlblVoluntaryDeduction = item["PropLoadingDiscount_EndorsementAmount"].InnerText;
                }

                if (item["PropLoadingDiscount_Description"].InnerText == "Voluntary Deductible For Depreciation Cover discount")
                {
                    objQuotePDFParams.VoluntaryDeductionforDepWaiver = Convert.ToDecimal(item["PropLoadingDiscount_EndorsementAmount"].InnerText).ToIndianCurrencyFormat();
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

            objQuotePDFParams.TotalPremiumOwnDamage = dcmlTotalPremiumOwnDamageTable.ToIndianCurrencyFormat();
            TotalPremiumOwnDamage = Convert.ToDecimal(dcmlTotalPremiumOwnDamageTable);

            decimal dcmlTotalPremiumLiability = Convert.ToDecimal(strlblBasicTPPremium) + Convert.ToDecimal(strlblLiabilityForBiFuel) +
            Convert.ToDecimal(strlblPAForUnnamedPassengerSI) + Convert.ToDecimal(strlblPAForNamedPassengerSI) + Convert.ToDecimal(strlblPAToPaidDriverSI) +
            Convert.ToDecimal(strlblPACoverForOwnerDriver) + Convert.ToDecimal(strlblLegalLiabilityToPaidDriverNo) + Convert.ToDecimal(strlblLLEOPDCC);

            objQuotePDFParams.TotalPremiumLiability = dcmlTotalPremiumLiability.ToIndianCurrencyFormat();



            DateTime dt_NewBusinessCondition = DateTime.ParseExact("01/09/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (objQuotePDFParams.rbbtNewBusiness && DateTime.ParseExact(objQuotePDFParams.txtDateOfRegistration.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= dt_NewBusinessCondition.Date)
            {
                if (objQuotePDFParams.drpProductType == "1063")
                {
                    objQuotePDFParams.ODYearText = "(1 Year)";
                    objQuotePDFParams.TPYearText = "(3 Years)";
                }
                else if (objQuotePDFParams.drpProductType == "1062")
                {
                    objQuotePDFParams.ODYearText = "(3 Years)";
                    objQuotePDFParams.TPYearText = "(3 Years)";
                }
            }
            else
            {
                objQuotePDFParams.ODYearText = "";
                objQuotePDFParams.TPYearText = "";
            }



            // CR 354 Start Here

            int PremiumWithPAtoOwnerDriver = 0;
            int PremiumWithoutPAtoOwnerDriver = 0;




            #region New Logic
            decimal GSTAmount = ((TotalPremiumOwnDamage) * 18) / 100;
            PremiumWithoutPAtoOwnerDriver = (int)Math.Round(((TotalPremiumOwnDamage) + GSTAmount));
            objQuotePDFParams.PremiumWithoutPAtoOwnerDriver = PremiumWithoutPAtoOwnerDriver.ToString();

            if (objQuotePDFParams.drpProductType == "1062")
            {
                //lilblPremiumWithoutPAtoOwnerDriver.Visible = false;
            }

            #endregion

            return objQuotePDFParams; ;
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
    }

    public class QuotePDFParams
    {
        public int ProductCode { get; set; }
        public string NetPremium { get; set; }
        public string ServiceTax { get; set; }
        public string TotalPremium { get; set; }
        public string CGSTAmount { get; set; }
        public string CGSTPercentage { get; set; }
        public string SGSTAmount { get; set; }
        public string SGSTPercentage { get; set; }
        public string IGSTAmount { get; set; }
        public string IGSTPercentage { get; set; }
        public string UGSTAmount { get; set; }
        public string UGSTPercentage { get; set; }
        public string TotalGSTAmount { get; set; }
        public int MaxQuoteVersion { get; set; }
        public string PercentServiceTax { get; set; }
        public string TotalPremiumKerala { get; set; }


        public bool chkIsGetCreditScore { get; set; }
        public string txtFirstName { get; set; }
        public string txtLastName { get; set; }
        public string CustomerName { get; set; }

        public string drpDrivingLicenseNumberOrAadhaarNumber { get; set; }
        public string txtDrivingLicenseNumberOrAadhaarNumber { get; set; }
        public string txtRTOAuthorityCode { get; set; }
        public bool rbbtRollOver { get; set; }
        public bool rbctIndividual { get; set; }
        public string txtMobileNumber { get; set; }
        public string drpTenureOwnerDriver { get; set; }
        public bool chkDepreciationCover { get; set; }
        public bool chkDailyCarAllowance { get; set; }
        public string ddlDailyCarAllowance { get; set; }
        public bool chkKeyReplacement { get; set; }
        public string ddlKeyReplacement { get; set; }
        public bool chkLossofPersonalBelongings { get; set; }
        public string ddlLossofPersonalBelongingsSI { get; set; }
        public bool rbbtNewBusiness { get; set; }
        public string txtDateOfRegistration { get; set; }
        public string drpProductType { get; set; }
        public string ProposalNumber { get; set; }
        public string CustomerId { get; set; }

        public string CustomerEmailId { get; set; }

        public string QuoteNumber { get; set; }
        public string OwnDamagePremium { get; set; }

        public bool IsNewBusiness { get; set; }
        public string DateOfRegistration { get; set; }

        public string ProductType { get; set; }

        public string MarketMovement { get; set; }
        public string BasicTPPremium { get; set; }
        public string CNGLPGKitIDV { get; set; }
        public string ConsumableCover { get; set; }
        public string CoverType { get; set; }
        public string CreditScore { get; set; }
        public string CreditScoreCustomerName { get; set; }

        public string CubicCapacity { get; set; }
        public string CustomerContactNo { get; set; }
        public string CustomerGender { get; set; }
        public string CustomerIDProof { get; set; }

        public string CustomerIDProofNumber { get; set; }

        public string DailyCarAllowance { get; set; }
        public string DepreciationCover { get; set; }
        public string DORorDOP { get; set; }

        public string ElectricalAccessoriesIDV { get; set; }
        public string ElectronicSI { get; set; }
        public string EngineProtect { get; set; }
        public string ExternalBiFuelSI { get; set; }
        public string FinalIDV { get; set; }
        public string FuelType { get; set; }
        public string IMDCode { get; set; }
        public string KeyReplacement { get; set; }
        public string KeyReplacementSI { get; set; }
        public string lblDailyCarAllowanceSI { get; set; }
        public string LegalLiabilityToPaidDriverNo { get; set; }
        public string LiabilityForBiFuel { get; set; }
        public string LLEOPDCC { get; set; }
        public string LossofPersonalBelongings { get; set; }
        public string LossofPersonalBelongingsSI { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string NCB { get; set; }
        public string NCBPercentage { get; set; }
        public string NCBProtect { get; set; }
        public string NonElectricalAccessoriesIDV { get; set; }
        public string NonElectronicSI { get; set; }
        public string ODYearText { get; set; }


        public string OwnershipType { get; set; }
        public string PACoverForOwnerDriver { get; set; }
        public string PAForNamedPassengerSI { get; set; }
        public string PAForUnnamedPassengerSI { get; set; }
        public string PAToPaidDriverSI { get; set; }
        public string PolicyHolderType { get; set; }
        public string PolicyStartDate { get; set; }
        public string PremiumWithoutPAtoOwnerDriver { get; set; }
        public string PreviousPolicyExpiryDate { get; set; }
        public string RagistrationDate { get; set; }

        public string RateBasicOD { get; set; }

        public string RateCC { get; set; }

        public string RateDC { get; set; }

        public string RateDCA { get; set; }

        public string RateEP { get; set; }

        public string RateKR { get; set; }

        public string RateLOPB { get; set; }

        public string RateNCBP { get; set; }

        public string RateRTI { get; set; }

        public string RateTC { get; set; }

        public string ReturnToInvoice { get; set; }
        public string RSA { get; set; }
        public string RTO { get; set; }

        public string SeatingCapacity { get; set; }
        public string SystemIDV { get; set; }
        public string TenureOwnerDriver { get; set; }

        public string TotalPremiumLiability { get; set; }
        public string TotalPremiumOwnDamage { get; set; }
        public string TPYearText { get; set; }

        public string TyreCover { get; set; }
        public string Variant { get; set; }
        public string VoluntaryDeduction { get; set; }
        public string VoluntaryDeductionforDepWaiver { get; set; }

        public string drpVDA { get; set; }

        public string VDEPCoverAmount { get; set; }
    }
}