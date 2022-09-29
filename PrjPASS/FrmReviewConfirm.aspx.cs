using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{

    public partial class FrmReviewConfirm : System.Web.UI.Page
    {
        public string propNumber = string.Empty;
        public string action1 = string.Empty;
        public string hash1 = string.Empty;
        public string firstName = string.Empty;
        public string lastName = string.Empty;
        public string proposal = string.Empty;
        public string eposFlag = string.Empty;
        public string typeofUser = string.Empty;
        //start added for payment entry
        public string custID = string.Empty;        
        //end added for payment entry 


        //public static DateTime dt = DateTime.Now.AddDays(1);
        //public string logfile = "log_kgipass_" + dt.ToString("dd-MMM-yyyy") + ".txt";
        public string logfile = "log_kgipass_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                //  divPopUp.Visible = false;

             

                string encryptProp = Request.QueryString["key"];
                string propNumber = DecryptText(encryptProp);
                proposal = propNumber;

                //   if (!IsPostBack)
                //   {


                if (!String.IsNullOrEmpty(propNumber))
                {
                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "PageLoad ::Displaying data for Proposal Number :" + propNumber + " " + DateTime.Now + Environment.NewLine);
                    DisplayData(propNumber);
                }
                else
                {
                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "PageLoad :: Prop Number is null for encrypt string : " + encryptProp + DateTime.Now + Environment.NewLine);
                    Response.Redirect("FrmCustomErrorPage.aspx");
                }
                // }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "PageLoad ::Error occured : " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
            }
        }

        //private void DisplayData(string propNumber)
        //{

        //    #region commented code

        //    //string customerID = string.Empty;

        //    //string Manufacture = string.Empty;
        //    //string Model = string.Empty;
        //    //string ModelVariant = string.Empty;
        //    //string SeatingCapacity = string.Empty;
        //    //string RTOCode = string.Empty;
        //    //string CubicCapacity = string.Empty;
        //    //string CustomerType = string.Empty;
        //    //string ProductName = string.Empty;
        //    //string NonElecAccessories = string.Empty;
        //    //string ElectricalAccessories = string.Empty;
        //    //string policyStartDate = string.Empty;
        //    //string OwnDamage = string.Empty;
        //    //string ODNonElectrical = string.Empty;
        //    //string ODElectrical = string.Empty;
        //    //string ODCNGKit = string.Empty;

        //    //string EngineProtect = string.Empty;
        //    //string ReturnToInvoice = string.Empty;
        //    //string RSA = string.Empty;
        //    //string UnnamedPA = string.Empty;
        //    //string namedPA = string.Empty;
        //    //string PAPaidDriver = string.Empty;
        //    //string PAOwnerDriver = string.Empty;
        //    //string legalLiabilityConductor = string.Empty;
        //    //string legalLiabilityOther = string.Empty;
        //    //string NCBPer = string.Empty;
        //    //string NCBAmount = string.Empty;

        //    //string VoluntaryDeduct = string.Empty;
        //    //string VoluntaryDedAsDepCover = string.Empty;
        //    //string TotalA = string.Empty;
        //    //string TotalB = string.Empty;
        //    //string gstAmount = string.Empty;
        //    //string systemIDV = string.Empty;

        //    //string finalIDV = string.Empty;
        //    //string NetPremium = string.Empty;
        //    //string TotalPremium = string.Empty;

        //    //string ConsumeCover = string.Empty;
        //    //string DepCover = string.Empty;
        //    //string PrevPolicyEnd = string.Empty;
        //    #endregion

        //    try
        //    {
        //        string quoteNumber = string.Empty;

        //        Database db = DatabaseFactory.CreateDatabase("cnPASS");
        //        using (SqlConnection con = new SqlConnection(db.ConnectionString))
        //        {
        //            con.Open();
        //            SqlCommand command = new SqlCommand("GET_QUOTE_NUMBER", con);
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@propNumber", propNumber);
        //            object objCustState = command.ExecuteScalar();
        //            quoteNumber = Convert.ToString(objCustState);
        //        }

        //        if (!String.IsNullOrEmpty(quoteNumber))
        //        {
        //            using (SqlConnection con = new SqlConnection(db.ConnectionString))
        //            {
        //                con.Open();
        //                SqlCommand command = new SqlCommand("GET_QUOTE_DETAILS", con);
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@quoteNumber", quoteNumber);
        //                SqlDataAdapter sda = new SqlDataAdapter(command);
        //                DataSet ds = new DataSet();
        //                sda.Fill(ds);

        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    lblProposalText.Text = propNumber;
        //                    lblCustomerID.Text = ds.Tables[0].Rows[0]["CustomerId"].ToString();
        //                    lblMakeText.Text = ds.Tables[0].Rows[0]["PropRisks_Manufacture"].ToString();

        //                    lblModelText.Text = ds.Tables[0].Rows[0]["PropRisks_Model"].ToString();
        //                    lblVariantText.Text = ds.Tables[0].Rows[0]["PropRisks_ModelVariant"].ToString();
        //                    lblseatingText.Text = ds.Tables[0].Rows[0]["PropRisks_SeatingCapacity"].ToString();
        //                    lblRTOText.Text = ds.Tables[0].Rows[0]["RTOCode"].ToString();
        //                    lblCubicText.Text = ds.Tables[0].Rows[0]["PropRisks_CubicCapacity"].ToString();
        //                    lblOwnerShipText.Text = ds.Tables[0].Rows[0]["CustomerType"].ToString();
        //                    lblCoverTypeText.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
        //                    lblNonElectricalText.Text = ds.Tables[0].Rows[0]["PropRisks_NonElectricalAccessories"].ToString();
        //                    lblElectricalText.Text = ds.Tables[0].Rows[0]["PropRisks_ElectricalAccessories"].ToString();
        //                    lblPolicyStartText.Text = ds.Tables[0].Rows[0]["PropPolicyEffectivedate_Fromdate_Mandatary"].ToString();
        //                    lblOwnDamage.Text = ds.Tables[0].Rows[0]["OwnDamage"].ToString();
        //                    lblElectricalItems.Text = ds.Tables[0].Rows[0]["ElectronicAccessoriesOD"].ToString();
        //                    lblNonElectricalItems.Text = ds.Tables[0].Rows[0]["NonElectricalAccessoriesOD"].ToString();
        //                    lblBiFuelKit.Text = ds.Tables[0].Rows[0]["CNGKitOD"].ToString();
        //                    lblEngineProtect.Text = ds.Tables[0].Rows[0]["EngineProtect"].ToString();
        //                    lblReturnToInvoice.Text = ds.Tables[0].Rows[0]["ReturnToInvoice"].ToString();
        //                    lblRSA.Text = ds.Tables[0].Rows[0]["RoadSideAssistance"].ToString();
        //                    lblPAUnnamed.Text = ds.Tables[0].Rows[0]["UnnamedPA"].ToString();
        //                    lblPANamed.Text = ds.Tables[0].Rows[0]["namedPAssengerPA"].ToString();
        //                    lblPAtoPaid.Text = ds.Tables[0].Rows[0]["PAPaidDriver"].ToString();
        //                    lblPACoverForDriver.Text = ds.Tables[0].Rows[0]["PAOwnerDriver"].ToString();
        //                    lblLiabilityForDriver.Text = ds.Tables[0].Rows[0]["legalLiabilityConductor"].ToString();
        //                    lblLiabilityEmployees.Text = ds.Tables[0].Rows[0]["legalLiabilityOtherThanConductor"].ToString();
        //                    lblNCBPer.Text = ds.Tables[0].Rows[0]["NCBPer"].ToString();
        //                    lblNCBAmount.Text = ds.Tables[0].Rows[0]["NCBAmount"].ToString();
        //                    lblVoluntary.Text = ds.Tables[0].Rows[0]["VoluntaryDeduction"].ToString();
        //                    lblVoluntaryDep.Text = ds.Tables[0].Rows[0]["volumntaryDedAsDepreciationCover"].ToString();
        //                    lblTotalA.Text = ds.Tables[0].Rows[0]["TotalA"].ToString();

        //                    lblPolicyHolderText.Text = ds.Tables[0].Rows[0]["PropRisks_TypeofPolicyHolder"].ToString();
        //                    lblCreditText.Text = ds.Tables[0].Rows[0]["Response_CreditScore"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["Response_CreditScore"].ToString();
        //                    lblRegistrationText.Text = ds.Tables[0].Rows[0]["DateofRegistration"].ToString();
        //                    lblTPPremium.Text = ds.Tables[0].Rows[0]["BasicTPIncludingTPPDPremium"].ToString();
        //                    lblLiabilityBiFuel.Text = ds.Tables[0].Rows[0]["CNGKitTP"].ToString();

        //                    lblTotalB.Text = ds.Tables[0].Rows[0]["TotalB"].ToString();
        //                    lblGSTAmount.Text = ds.Tables[0].Rows[0]["gstAmount"].ToString();
        //                    lblSystemIDV.Text = ds.Tables[0].Rows[0]["SystemIDV"].ToString();
        //                    lblFinalIDV.Text = ds.Tables[0].Rows[0]["FinalIDV"].ToString();
        //                    lblNetPremium.Text = ds.Tables[0].Rows[0]["NetPremium"].ToString();
        //                    lblTotalPremium.Text = ds.Tables[0].Rows[0]["TotalPremium"].ToString();

        //                    lblConsumableCover.Text = ds.Tables[0].Rows[0]["ConsumableCover"].ToString();
        //                    lblDepreciationCover.Text = ds.Tables[0].Rows[0]["DepreciationCover"].ToString();
        //                    lblPreviousPolicyText.Text = ds.Tables[0].Rows[0]["PrevPolicyEnd"].ToString();

        //                }
        //                else
        //                {
        //                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "DisplayData :: Quote data not found for quote number : " + quoteNumber + DateTime.Now + Environment.NewLine);
        //                    Response.Redirect("FrmCustomErrorPage.aspx");
        //                }

        //                SqlCommand cmdCustomer = new SqlCommand("GET_CUSTOMER_DETAILS", con);
        //                cmdCustomer.CommandType = CommandType.StoredProcedure;
        //                cmdCustomer.Parameters.AddWithValue("@quoteNumber", quoteNumber);
        //                SqlDataAdapter sdaCust = new SqlDataAdapter(cmdCustomer);
        //                DataSet dsCust = new DataSet();
        //                sdaCust.Fill(dsCust);

        //                if (dsCust.Tables[0].Rows.Count > 0)
        //                {
        //                    lblCustomerName.Text = dsCust.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsCust.Tables[0].Rows[0]["MiddleName"].ToString() + " " + dsCust.Tables[0].Rows[0]["LastName"].ToString();
        //                    firstName = dsCust.Tables[0].Rows[0]["FirstName"].ToString();
        //                    lastName = dsCust.Tables[0].Rows[0]["LastName"].ToString();
        //                    lblMobile.Text = dsCust.Tables[0].Rows[0]["MobileNumber"].ToString();
        //                    lblEmail.Text = dsCust.Tables[0].Rows[0]["EmailAddress"].ToString();
        //                    lblDOB.Text = dsCust.Tables[0].Rows[0]["DateOfBirth"].ToString();
        //                    lblPincode.Text = dsCust.Tables[0].Rows[0]["CustomerPincode"].ToString();
        //                    lblState.Text = dsCust.Tables[0].Rows[0]["StateName"].ToString();
        //                    lblCity.Text = dsCust.Tables[0].Rows[0]["CityName"].ToString();
        //                    lblAddress.Text = dsCust.Tables[0].Rows[0]["AddressLine1"].ToString() + " " + dsCust.Tables[0].Rows[0]["AddressLine2"].ToString() + " " + dsCust.Tables[0].Rows[0]["AddressLine3"].ToString();

        //                }
        //                else
        //                {
        //                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "DisplayData :: Customer data not found for quote number : " + quoteNumber + DateTime.Now + Environment.NewLine);
        //                    Response.Redirect("FrmCustomErrorPage.aspx");
        //                }

        //            }
        //        }
        //        else
        //        {
        //            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "DisplayData :: Quote number is null or empty " + DateTime.Now + Environment.NewLine);
        //            Response.Redirect("FrmCustomErrorPage.aspx");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "DisplayData ::Error occured while displaying data for :" + propNumber + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace);
        //        Response.Redirect("FrmCustomErrorPage.aspx");
        //    }
        //}


        private void DisplayData(string propNumber)
        {

            #region commented code

            //string customerID = string.Empty;

            //string Manufacture = string.Empty;
            //string Model = string.Empty;
            //string ModelVariant = string.Empty;
            //string SeatingCapacity = string.Empty;
            //string RTOCode = string.Empty;
            //string CubicCapacity = string.Empty;
            //string CustomerType = string.Empty;
            //string ProductName = string.Empty;
            //string NonElecAccessories = string.Empty;
            //string ElectricalAccessories = string.Empty;
            //string policyStartDate = string.Empty;
            //string OwnDamage = string.Empty;
            //string ODNonElectrical = string.Empty;
            //string ODElectrical = string.Empty;
            //string ODCNGKit = string.Empty;

            //string EngineProtect = string.Empty;
            //string ReturnToInvoice = string.Empty;
            //string RSA = string.Empty;
            //string UnnamedPA = string.Empty;
            //string namedPA = string.Empty;
            //string PAPaidDriver = string.Empty;
            //string PAOwnerDriver = string.Empty;
            //string legalLiabilityConductor = string.Empty;
            //string legalLiabilityOther = string.Empty;
            //string NCBPer = string.Empty;
            //string NCBAmount = string.Empty;

            //string VoluntaryDeduct = string.Empty;
            //string VoluntaryDedAsDepCover = string.Empty;
            //string TotalA = string.Empty;
            //string TotalB = string.Empty;
            //string gstAmount = string.Empty;
            //string systemIDV = string.Empty;

            //string finalIDV = string.Empty;
            //string NetPremium = string.Empty;
            //string TotalPremium = string.Empty;

            //string ConsumeCover = string.Empty;
            //string DepCover = string.Empty;
            //string PrevPolicyEnd = string.Empty;
            #endregion

            try
            {
                string quoteNumber = string.Empty;
                int quoteVersion = 0;

                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("GET_QUOTE_NUMBER", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@propNumber", propNumber);
                    //   object objCustState = command.ExecuteScalar();
                    // quoteNumber = Convert.ToString(objCustState);
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        quoteNumber = ds.Tables[0].Rows[0]["QuoteNumber"].ToString();
                        quoteVersion = Convert.ToInt32(ds.Tables[0].Rows[0]["QuoteVersion"].ToString());
                    }

                }

                if (!String.IsNullOrEmpty(quoteNumber))
                {
                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("GET_QUOTE_DETAILS", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@quoteNumber", quoteNumber);
                        command.Parameters.AddWithValue("@quoteVersion", quoteVersion);
                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //check for vb 64
                            DateTime ddCheck = DateTime.ParseExact(ds.Tables[0].Rows[0]["PropPolicyEffectivedate_Fromdate_Mandatary"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                          //  DateTime ddCheck = DateTime.ParseExact("19/09/2017", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            DateTime ddnow = DateTime.Now;
                            double diffDate = Math.Floor((ddCheck - ddnow).TotalDays);
                            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "DisplayData :: diffdate is : " + diffDate + DateTime.Now + Environment.NewLine);

                            if (diffDate < Convert.ToInt32(ConfigurationManager.AppSettings["datediff"])) //datediff is set to 0 
                            {
                                Response.Redirect("FrmContactPage.aspx", false);
                            }

                            else
                            {

                                lblQuoteNumber.Text = quoteNumber;
                                lblProposalText.Text = propNumber;
                                lblCustomerID.Text = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                                custID = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                                lblMakeText.Text = ds.Tables[0].Rows[0]["PropRisks_Manufacture"].ToString();

                                lblFuelType.Text = ds.Tables[0].Rows[0]["PropRisks_FuelType"].ToString();

                                lblPolicyStartText.Text = ds.Tables[0].Rows[0]["PropPolicyEffectivedate_Fromdate_Mandatary"].ToString();
                                lblPolicyEndDate.Text = ds.Tables[0].Rows[0]["PropPolicyEffectivedate_Todate_Mandatary"].ToString();

                                lblModelText.Text = ds.Tables[0].Rows[0]["PropRisks_Model"].ToString();
                                lblVariantText.Text = ds.Tables[0].Rows[0]["PropRisks_ModelVariant"].ToString();
                                lblseatingText.Text = ds.Tables[0].Rows[0]["PropRisks_SeatingCapacity"].ToString();
                                lblRTOText.Text = ds.Tables[0].Rows[0]["RTOCode"].ToString();
                                lblCubicText.Text = ds.Tables[0].Rows[0]["PropRisks_CubicCapacity"].ToString();
                                lblOwnerShipText.Text = ds.Tables[0].Rows[0]["CustomerType"].ToString();
                                lblCoverTypeText.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
                                lblNonElectricalText.Text = ds.Tables[0].Rows[0]["PropRisks_NonElectricalAccessories"].ToString();
                                lblElectricalText.Text = ds.Tables[0].Rows[0]["PropRisks_ElectricalAccessories"].ToString();
                                lblPolicyStartText.Text = ds.Tables[0].Rows[0]["PropPolicyEffectivedate_Fromdate_Mandatary"].ToString();
                                lblOwnDamage.Text = ds.Tables[0].Rows[0]["OwnDamage"].ToString();
                                lblElectricalItems.Text = ds.Tables[0].Rows[0]["ElectronicAccessoriesOD"].ToString();
                                lblNonElectricalItems.Text = ds.Tables[0].Rows[0]["NonElectricalAccessoriesOD"].ToString();
                                lblBiFuelKit.Text = ds.Tables[0].Rows[0]["CNGKitOD"].ToString();
                                lblEngineProtect.Text = ds.Tables[0].Rows[0]["EngineProtect"].ToString();
                                lblReturnToInvoice.Text = ds.Tables[0].Rows[0]["ReturnToInvoice"].ToString();
                                lblRSA.Text = ds.Tables[0].Rows[0]["RoadSideAssistance"].ToString();

                                //CR164 - NEW ADD ONS
                                lblDailyCarAllowance.Text = string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["DailyCarAllowance"])) ? "0.00" : Convert.ToString(ds.Tables[0].Rows[0]["DailyCarAllowance"]);
                                lblTyreCover.Text = string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["DailyCarAllowance"])) ? "0.00" : Convert.ToString(ds.Tables[0].Rows[0]["TyreCover"]);
                                lblNCBProtect.Text = string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["DailyCarAllowance"])) ? "0.00" : Convert.ToString(ds.Tables[0].Rows[0]["NCBProtect"]);
                                lblLossofPersonalBelongings.Text = string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["DailyCarAllowance"])) ? "0.00" : Convert.ToString(ds.Tables[0].Rows[0]["LossofPersonalBelongings"]);
                                lblKeyReplacement.Text = string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["DailyCarAllowance"])) ? "0.00" : Convert.ToString(ds.Tables[0].Rows[0]["KeyReplacement"]);
                                //

                                lblPAUnnamed.Text = ds.Tables[0].Rows[0]["UnnamedPA"].ToString();
                                lblPANamed.Text = ds.Tables[0].Rows[0]["namedPAssengerPA"].ToString();
                                lblPAtoPaid.Text = ds.Tables[0].Rows[0]["PAPaidDriver"].ToString();
                                lblPACoverForDriver.Text = ds.Tables[0].Rows[0]["PAOwnerDriver"].ToString();
                                lblLiabilityForDriver.Text = ds.Tables[0].Rows[0]["legalLiabilityConductor"].ToString();
                                lblLiabilityEmployees.Text = ds.Tables[0].Rows[0]["legalLiabilityOtherThanConductor"].ToString();
                                lblNCBPer.Text = ds.Tables[0].Rows[0]["NCBPer"].ToString();
                                lblNCBAmount.Text = ds.Tables[0].Rows[0]["NCBAmount"].ToString();
                                lblVoluntary.Text = ds.Tables[0].Rows[0]["VoluntaryDeduction"].ToString();
                                lblVoluntaryDep.Text = ds.Tables[0].Rows[0]["volumntaryDedAsDepreciationCover"].ToString();
                                //lblTotalA.Text = ds.Tables[0].Rows[0]["TotalA"].ToString();

                                lblPolicyHolderText.Text = ds.Tables[0].Rows[0]["PropRisks_TypeofPolicyHolder"].ToString();
                                lblCreditText.Text = ds.Tables[0].Rows[0]["Response_CreditScore"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["Response_CreditScore"].ToString();
                                lblRegistrationText.Text = ds.Tables[0].Rows[0]["DateofRegistration"].ToString();
                                lblTPPremium.Text = ds.Tables[0].Rows[0]["BasicTPIncludingTPPDPremium"].ToString();
                                lblLiabilityBiFuel.Text = ds.Tables[0].Rows[0]["CNGKitTP"].ToString();

                                //lblTotalB.Text = ds.Tables[0].Rows[0]["TotalB"].ToString();
                                lblGSTAmount.Text = ds.Tables[0].Rows[0]["gstAmount"].ToString();
                                lblSystemIDV.Text = ds.Tables[0].Rows[0]["SystemIDV"].ToString();
                                lblFinalIDV.Text = ds.Tables[0].Rows[0]["FinalIDV"].ToString();
                                lblFinalIDV2.Text = ds.Tables[0].Rows[0]["FinalIDV"].ToString(); //idv change 04/09/2017
                                lblNetPremium.Text = ds.Tables[0].Rows[0]["NetPremium"].ToString();
                                lblTotalPremium.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalPremium"].ToString()).ToIndianCurrencyFormat();
                                lblTotalPremium2.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalPremium"].ToString()).ToIndianCurrencyFormat();

                                lblConsumableCover.Text = ds.Tables[0].Rows[0]["ConsumableCover"].ToString();
                                lblDepreciationCover.Text = ds.Tables[0].Rows[0]["DepreciationCover"].ToString();
                                lblPreviousPolicyText.Text = ds.Tables[0].Rows[0]["PrevPolicyEnd"].ToString();

                                //code start to get epos flag
                                SqlCommand cmdEpos = new SqlCommand("GET_EPOS_FLAG", con);
                                cmdEpos.CommandType = CommandType.StoredProcedure;
                                cmdEpos.Parameters.AddWithValue("@proposalNumber", propNumber);
                                //object objEpos = cmdEpos.ExecuteScalar();
                                //eposFlag = Convert.ToString(objEpos);
                                SqlDataAdapter sdaEpos = new SqlDataAdapter(cmdEpos);
                                DataSet dsEpos = new DataSet();
                                sdaEpos.Fill(dsEpos);
                                if (dsEpos.Tables[0].Rows.Count > 0)
                                {
                                    eposFlag = dsEpos.Tables[0].Rows[0]["IsAllowLoginFromMobile"].ToString();
                                    typeofUser = dsEpos.Tables[0].Rows[0]["TypeOfUser"].ToString();
                                }
                                //code end to get epos flag
                            }
                        }
                        else
                        {
                            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "DisplayData :: Quote data not found for quote number : " + quoteNumber + DateTime.Now + Environment.NewLine);
                            Response.Redirect("FrmCustomErrorPage.aspx",true);
                        }

                        SqlCommand cmdCustomer = new SqlCommand("GET_CUSTOMER_DETAILS", con);
                        cmdCustomer.CommandType = CommandType.StoredProcedure;
                        cmdCustomer.Parameters.AddWithValue("@quoteNumber", quoteNumber);
                        SqlDataAdapter sdaCust = new SqlDataAdapter(cmdCustomer);
                        DataSet dsCust = new DataSet();
                        sdaCust.Fill(dsCust);

                        if (dsCust.Tables[0].Rows.Count > 0)
                        {
                            lblCustomerName.Text = dsCust.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsCust.Tables[0].Rows[0]["MiddleName"].ToString() + " " + dsCust.Tables[0].Rows[0]["LastName"].ToString();
                            firstName = dsCust.Tables[0].Rows[0]["FirstName"].ToString();
                            lastName = dsCust.Tables[0].Rows[0]["LastName"].ToString();
                            if (lblOwnerShipText.Text.Trim().ToLower() == "organization")
                            {
                                lblCustomerName.Text = dsCust.Tables[0].Rows[0]["ContactPerson"].ToString();
                                firstName = dsCust.Tables[0].Rows[0]["ContactPerson"].ToString();
                                lastName = "";
                            }

                            lblMobile.Text = dsCust.Tables[0].Rows[0]["MobileNumber"].ToString();
                            lblEmail.Text = dsCust.Tables[0].Rows[0]["EmailAddress"].ToString();
                            //lblDOB.Text = dsCust.Tables[0].Rows[0]["DateOfBirth"].ToString();

                            if (dsCust.Tables[0].Rows[0]["DateOfBirth"].ToString() != string.Empty)
                            {
                                lblDOB.Text = Convert.ToDateTime(dsCust.Tables[0].Rows[0]["DateOfBirth"]).ToString("MM/dd/yyyy");
                            }
                            else {

                                lblDOB.Text = "";
                            }

                            lblPanNumber.Text = string.IsNullOrEmpty(dsCust.Tables[0].Rows[0]["PanNumber"].ToString()) ? "-" : dsCust.Tables[0].Rows[0]["PanNumber"].ToString();

                            lblAddress.Text = dsCust.Tables[0].Rows[0]["AddressLine1"].ToString() + " " +
                                dsCust.Tables[0].Rows[0]["AddressLine2"].ToString() + " " +
                                dsCust.Tables[0].Rows[0]["AddressLine3"].ToString() + " " +
                                dsCust.Tables[0].Rows[0]["CityName"].ToString() + " " +
                                dsCust.Tables[0].Rows[0]["StateName"].ToString() + " " +
                                dsCust.Tables[0].Rows[0]["CustomerPincode"].ToString();

                            lblRegistrationNumber.Text = dsCust.Tables[0].Rows[0]["RegistrationNumber1"].ToString() + " " +
                                dsCust.Tables[0].Rows[0]["RegistrationNumber2"].ToString() + " " +
                                dsCust.Tables[0].Rows[0]["RegistrationNumber3"].ToString() + " " +
                                dsCust.Tables[0].Rows[0]["RegistrationNumber4"].ToString();

                            lblEngineNumber.Text = dsCust.Tables[0].Rows[0]["EngineNumber"].ToString();
                            lblChassisNumber.Text = dsCust.Tables[0].Rows[0]["ChassisNumber"].ToString();
                            lblNomineeName.Text = dsCust.Tables[0].Rows[0]["NomineeName"].ToString();

                            lblNomineeDOB.Text = dsCust.Tables[0].Rows[0]["NomineeDOB"].ToString();
                            lblRelationshipWithInsured.Text = dsCust.Tables[0].Rows[0]["NomineeRelationShip"].ToString();
                            lblAppointee.Text = string.IsNullOrEmpty(dsCust.Tables[0].Rows[0]["AppointeeName"].ToString()) ? "-" : dsCust.Tables[0].Rows[0]["AppointeeName"].ToString();
                            lblAppointeeRelationship.Text = dsCust.Tables[0].Rows[0]["AppointeeRelationShip"].ToString() == "0" ? "-" : dsCust.Tables[0].Rows[0]["AppointeeRelationShip"].ToString();
                        }
                        else
                        {
                            File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "DisplayData :: Customer data not found for quote number : " + quoteNumber + DateTime.Now + Environment.NewLine);
                            Response.Redirect("FrmCustomErrorPage.aspx",true);
                        }

                    }
                }
                else
                {
                    File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "DisplayData :: Quote number is null or empty " + DateTime.Now + Environment.NewLine);
                    Response.Redirect("FrmCustomErrorPage.aspx",true);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "DisplayData ::Error occured while displaying data for :" + propNumber + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace);
                Response.Redirect("FrmCustomErrorPage.aspx");
            }
        }
        public string DecryptText(string cipherText)
        {
            try
            {
                string EncryptionKey = "KGIMAV2BNI1907";
                byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in decrypttext for text :" + cipherText + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
                return null;

            }


        }
        
        //to generate otp
        protected void btnMakePayment_Click(object sender, EventArgs e)
        {
            try
            {
                //otp code 
                
                int Num = int.Parse(hdnOTPSentCount.Value);

                if (Page.IsValid && Num < 3)
                {
              //      anchorLink.Disabled = true;
                    Num = int.Parse(hdnOTPSentCount.Value) + 1;
                    hdnOTPSentCount.Value = Num.ToString();

                    string mobileno = lblMobile.Text;
                    string emailid = lblEmail.Text;
                    string name = firstName;
                    string propNumber = lblProposalText.Text;
                    string generateOTP = GenerateOTP(mobileno, emailid, "", name, propNumber);

                    otpPanel.Visible = true;
                    btnMobileVerify.Focus();
                    ScriptManager.RegisterStartupScript(UpdatePanel_Detail1, UpdatePanel_Detail1.GetType(), "testpage", "runme();", true);
                    UpdatePanel_Detail1.Update();
                }               

                else
                {
                  //  anchorLink.Disabled = false;
                    cvtxtOtpNumber.IsValid = false;
                     cvtxtOtpNumber.ErrorMessage = "Maximum OTP Send limit is over, kindly contact nearest branch. Or Check your latest OTP";
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in btnMakePayment :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
            }
            #region commented code
            //Database db = DatabaseFactory.CreateDatabase("cnPASS");
            //using (SqlConnection con = new SqlConnection(db.ConnectionString))
            //{
            //    con.Open();
            //    SqlCommand command = new SqlCommand("UPDATE_PAY_HIT_COUNT", con);
            //    command.CommandType = CommandType.StoredProcedure;
            //    command.Parameters.AddWithValue("@propNumber", proposal);
            //    command.ExecuteNonQuery();
            //}


            //    string[] hashVarsSeq;
            //string hash_string = string.Empty;
            //hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|');
            //foreach (string hash_var in hashVarsSeq)
            //{
            //    if (hash_var == "key")
            //    {
            //        hash_string = hash_string + ConfigurationManager.AppSettings["key"];
            //        hash_string = hash_string + '|';
            //    }
            //    else if (hash_var == "txnid")
            //    {
            //        hash_string = hash_string + proposal;
            //        hash_string = hash_string + '|';
            //    }
            //    else if (hash_var == "amount")
            //    {
            //        hash_string = hash_string + Convert.ToDecimal(lblTotalPremium.Text).ToString("g29");
            //        hash_string = hash_string + '|';
            //    }

            //    else if (hash_var == "productinfo")
            //    {
            //        hash_string = hash_string + ConfigurationManager.AppSettings["productinfo"];
            //        hash_string = hash_string + '|';
            //    }

            //    else if (hash_var == "firstname")
            //    {
            //        hash_string = hash_string + firstName + " " + lastName;
            //        hash_string = hash_string + '|';
            //    }

            //    else if (hash_var == "email")
            //    {
            //        hash_string = hash_string + lblEmail.Text;
            //        hash_string = hash_string + '|';
            //    }

            //    else
            //    {

            //        hash_string = hash_string + (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
            //        hash_string = hash_string + '|';
            //    }
            //}

            //hash_string += ConfigurationManager.AppSettings["salt"];
            //hash1 = Generatehash512(hash_string).ToLower();
            //action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";

            //if (!string.IsNullOrEmpty(hash1))
            //{
            //    //hash.Value = hash1;
            //    //txnid.Value = txnid1;

            //    System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
            //    data.Add("hash", hash1);
            //    data.Add("txnid", proposal);
            //    data.Add("key", ConfigurationManager.AppSettings["key"]);
            //    string AmountForm = Convert.ToDecimal(lblTotalPremium.Text).ToString("g29");// eliminating trailing zeros
            //                                                                                //amount.Text = AmountForm;
            //    data.Add("amount", AmountForm);
            //    data.Add("firstname", firstName + " " + lastName);
            //    data.Add("email", lblEmail.Text);
            //    data.Add("phone", lblMobile.Text);
            //    data.Add("productinfo", "Private Car"); //as of now need to pass private car
            //    data.Add("surl", ConfigurationManager.AppSettings["surl"]);
            //    data.Add("furl", ConfigurationManager.AppSettings["furl"]);
            //    data.Add("lastname", lastName);
            //    data.Add("curl", "");
            //    data.Add("address1", "");
            //    data.Add("address2", "");
            //    data.Add("city", "");
            //    data.Add("state", "");
            //    data.Add("country", "");
            //    data.Add("zipcode", "");
            //    data.Add("udf1", "");
            //    data.Add("udf2", "");
            //    data.Add("udf3", "");
            //    data.Add("udf4", "");
            //    data.Add("udf5", "");
            //    data.Add("pg", "");


            //    string strForm = PreparePOSTForm(action1, data);
            //    Page.Controls.Add(new LiteralControl(strForm));
            #endregion
        }
        private string GenerateOTP(string mobileno, string emailid, string v, string name, string propNumber)
        {
            try
            {
                //mobileno = "7045041046";

                string strPath = string.Empty;
                string smsBody = string.Empty;

                Random r = new Random();
                int OTPNumber = r.Next(100000, 999999);
                DataSet ds = new DataSet();
                ds = InsertOTPData("0", mobileno, lblCustomerID.Text, lblCustomerName.Text, propNumber, OTPNumber.ToString(), "", "", "", "", "3121");

                smsBody = ConfigurationManager.AppSettings["smsBody"];
                //smsBody = File.ReadAllText(strPath);
                smsBody = smsBody.Replace("@otpNumber", Convert.ToString(OTPNumber));

                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "0")
                {
                    //string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
                    var client = new System.Net.WebClient();
                    var content = client.DownloadString(URI);
                }
                else
                {
                    string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                    string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                    string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();
                    string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                    string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();

                    string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();
                    WebProxy proxy = new WebProxy(proxy_Address_Full);
                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;

                    //string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", mobileno, smsBody);
                    

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                    var client = new System.Net.WebClient();
                    client.Proxy = proxy;

                    client.Proxy = WebRequest.DefaultWebProxy;
                    client.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    client.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);

                    var content = client.DownloadString(URI);
                    client.Proxy = null;
                }

                WebRequest.DefaultWebProxy = null;
                HttpContext.Current.Session["OTPId"] = Convert.ToString(ds.Tables[0].Rows[0]["Id"]);
                HttpContext.Current.Session["OTPNumber"] = Convert.ToString(ds.Tables[0].Rows[0]["OTP"]);
                return OTPNumber.ToString();
            }

            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in GenerateOTP for proposal number:" + lblProposalText.Text + " and error message is : " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
                return null;
            }

        }

        private DataSet InsertOTPData(string identity, string mobileno, string custID, string custName, string propNumber, string otpNumber, string v4, string v5, string v6, string v7, string productcode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            using (SqlConnection con = new SqlConnection(db.ConnectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("INSERT_OTP_DATA", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@identity", identity);
                command.Parameters.AddWithValue("@mobileno", mobileno);
                command.Parameters.AddWithValue("@custID", custID);
                command.Parameters.AddWithValue("@custName", custName);
                command.Parameters.AddWithValue("@propNumber", propNumber);
                command.Parameters.AddWithValue("@otpNumber", otpNumber);
                command.Parameters.AddWithValue("@productcode", productcode);
                DataSet myDataSet = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(myDataSet);
                return myDataSet;
            }


        }

        public string Generatehash512(string text)
        {
            try
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
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in Generatehash512 for text :" + text + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
                return null;
            }


        }

        private string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
            try
            {
                //Set a name for the form
                string formID = "PostForm";
                //Build the form using the specified data to be posted.
                StringBuilder strForm = new StringBuilder();
                strForm.Append("<form id=\"" + formID + "\" name=\"" +
                               formID + "\" action=\"" + url +
                               "\" method=\"POST\">");

                foreach (System.Collections.DictionaryEntry key in data)
                {

                    strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                                   "\" value=\"" + key.Value + "\">");
                }


                strForm.Append("</form>");
                //Build the JavaScript which will do the Posting operation.
                StringBuilder strScript = new StringBuilder();
                strScript.Append("<script language='javascript'>");
                strScript.Append("var v" + formID + " = document." +
                                 formID + ";");
                strScript.Append("v" + formID + ".submit();");
                strScript.Append("</script>");
                //Return the form and the script concatenated.
                //(The order is important, Form then JavaScript)
                return strForm.ToString() + strScript.ToString();
            }

            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in Preparepostform for url :" + url + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
                return null;
            }

        }

        protected void onclick_btnPayment(object sender, EventArgs e)
        {
            try
            {
                btnMakePayment.Focus();

                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("UPDATE_PAY_HIT_COUNT", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@propNumber", proposal);
                    command.ExecuteNonQuery();
                }


                string[] hashVarsSeq;
                string hash_string = string.Empty;


                hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|');
                foreach (string hash_var in hashVarsSeq)
                {
                    if (hash_var == "key")
                    {
                        hash_string = hash_string + ConfigurationManager.AppSettings["key_payu"];
                        hash_string = hash_string + '|';
                    }
                    else if (hash_var == "txnid")
                    {
                        hash_string = hash_string + proposal;
                        hash_string = hash_string + '|';
                    }
                    else if (hash_var == "amount")
                    {
                        string[] prem = lblTotalPremium.Text.Split(' ');

                        //hash_string = hash_string + Convert.ToDecimal(lblTotalPremium.Text).ToString("g29");
                        hash_string = hash_string + Convert.ToDecimal(prem[1]).ToString("g29");
                        hash_string = hash_string + '|';
                    }

                    else if (hash_var == "productinfo")
                    {
                        hash_string = hash_string + ConfigurationManager.AppSettings["productinfo"];
                        hash_string = hash_string + '|';
                    }

                    else if (hash_var == "firstname")
                    {
                        hash_string = hash_string + firstName + " " + lastName;
                        hash_string = hash_string + '|';
                    }

                    else if (hash_var == "email")
                    {
                        hash_string = hash_string + lblEmail.Text;
                        hash_string = hash_string + '|';
                    }

                    else if (hash_var == "mobile")
                    {
                        hash_string = hash_string + lblMobile.Text;
                        hash_string = hash_string + '|';
                    }

                    else if (hash_var == "udf1")
                    {
                        hash_string = hash_string + lblQuoteNumber.Text;
                        hash_string = hash_string + '|';
                    }

                    else if (hash_var == "udf2")
                    {
                        hash_string = hash_string + typeofUser;
                        hash_string = hash_string + '|';
                    }

                    else if (hash_var == "udf3")
                    {
                        hash_string = hash_string + custID;
                        hash_string = hash_string + '|';
                    }

                 


                    else
                    {

                        hash_string = hash_string + (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
                        hash_string = hash_string + '|';
                    }
                }

                hash_string += ConfigurationManager.AppSettings["salt_payu"];
                hash1 = Generatehash512(hash_string).ToLower();
                action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";

                if (!string.IsNullOrEmpty(hash1))
                {
                    //hash.Value = hash1;
                    //txnid.Value = txnid1;

                    System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                    data.Add("hash", hash1);
                    data.Add("txnid", proposal);
                    data.Add("key", ConfigurationManager.AppSettings["key_payu"]);
                    string[] prem = lblTotalPremium.Text.Split(' ');
                    //string AmountForm = Convert.ToDecimal(lblTotalPremium.Text).ToString("g29");// eliminating trailing zeros
                    //amount.Text = AmountForm;
                    string AmountForm = Convert.ToDecimal(prem[1]).ToString("g29");
                    data.Add("amount", AmountForm);
                    data.Add("firstname", firstName + " " + lastName);
                    data.Add("email", lblEmail.Text);
                    data.Add("phone", lblMobile.Text);
                    data.Add("productinfo", "Private Car"); //as of now need to pass private car
                    data.Add("surl", ConfigurationManager.AppSettings["surl"]);
                    data.Add("furl", ConfigurationManager.AppSettings["furl"]);
                    data.Add("lastname", lastName);
                    data.Add("curl", "");
                    data.Add("address1", "");
                    data.Add("address2", "");
                    data.Add("city", "");
                    data.Add("state", "");
                    data.Add("country", "");
                    data.Add("zipcode", "");
                    data.Add("udf1", lblQuoteNumber.Text);
                    data.Add("udf2", typeofUser);
                    data.Add("udf3", custID);
                    data.Add("udf4", "");
                    data.Add("udf5", "");
                    data.Add("pg", "");


                    string strForm = PreparePOSTForm(action1, data);
                    Page.Controls.Add(new LiteralControl(strForm));
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured  in onclick_btnpayment and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");

            }
        }

        protected void onClickbtnMobileReSend(object sender, EventArgs e)
        {
            btnMakePayment_Click(sender, e);
        }

        protected void onClickbtnMobileVerify(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (txtOtpNumber.Text == HttpContext.Current.Session["OTPNumber"].ToString())
                {
                    bool otpFlag = UpdateOTPData(HttpContext.Current.Session["OTPId"].ToString(), hdnOTPSentCount.Value , lblProposalText.Text, Convert.ToInt32(txtOtpNumber.Text),"3121");
                    if (otpFlag)
                    {
                        onclick_btnPayment(sender, e);
                    }
                    else
                    {
                        File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in onClickbtnMobileVerify for proposal :" + proposal + " and is in else condition of otpflag " + DateTime.Now + Environment.NewLine);
                        Response.Redirect("FrmCustomErrorPage.aspx");
                    }
                }
            }
        }

        private bool UpdateOTPData(string identity, string otpCount, string proposal, int otpNumber,string prodCode)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("UPDATE_OTP_DATA", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", Convert.ToInt32(identity));                    
                    command.Parameters.AddWithValue("@propNumber", proposal);
                    command.Parameters.AddWithValue("@OTPNoFromCustomer", otpNumber);
                    command.Parameters.AddWithValue("@productcode", prodCode);
                    command.Parameters.AddWithValue("@AttemptCount", Convert.ToInt32(otpCount));
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in UpdateOTPData for proposal :" + proposal + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);                
                Response.Redirect("FrmCustomErrorPage.aspx");                
            }
            return false;

        }

        protected void OnServerValidatecvtxtOtpNumber(object sender, ServerValidateEventArgs e)
        {
            if (txtOtpNumber.Text == HttpContext.Current.Session["OTPNumber"].ToString())
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;

                //int Num = int.Parse(hdnOTPSentCount.Value) + 1;
                //hdnOTPSentCount.Value = Num.ToString();

                //agreewithbtn.Visible = false;
                otpPanel.Visible = true;
                txtOtpNumber.Text = "";
                btnMakePayment.Enabled = false;
                btnMobileReSend.Enabled = false;
                btnMobileVerify.Focus();
                cvtxtOtpNumber.ErrorMessage = "Please provide valid otp number.";
                ScriptManager.RegisterStartupScript(UpdatePanel_Detail1, UpdatePanel_Detail1.GetType(), "testpage", "runme();", true);
            }
        }

        protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (chkSummaryAgree.Checked)
            {
                e.IsValid = true;
                chkSummaryAgree.Disabled = true;
            }
            else
                e.IsValid = false;
            btnMakePayment.Focus();
        }

        protected void btnSendMail_ServerClick(object sender, EventArgs e)
        {
            try
            {

                if (typeofUser.ToLower() == ConfigurationManager.AppSettings["epos_user"].Trim())
                {

                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["careEmail_epos"].Trim()))
                    {
                        string emailId = ConfigurationManager.AppSettings["careEmail_epos"].Trim();
                        string[] arrMail = emailId.Split(',');

                        string strPath = string.Empty;
                        string MailBody = string.Empty;
                        SmtpClient smtpClient = new SmtpClient();
                        smtpClient.Port = 25;
                        smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                        smtpClient.Timeout = 3600000;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);

                        Database db = DatabaseFactory.CreateDatabase("cnPASS");
                        using (SqlConnection con = new SqlConnection(db.ConnectionString))
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand("INSERT_PROPOSAL_MODIFY_DETAILS", con);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@proposal", propNumber);
                            command.Parameters.AddWithValue("@mobile", lblMobile.Text);
                            command.Parameters.AddWithValue("@email", lblEmail.Text);
                            command.Parameters.AddWithValue("@quote", lblQuoteNumber.Text);
                            string details = txtDetails.Text.Replace('\r', ' ');
                            details = details.Replace('\n', ' ');
                            SqlParameter prmDetail = new SqlParameter();

                            prmDetail.ParameterName = "@text"; 
                            prmDetail.SqlDbType = SqlDbType.VarChar; 
                            prmDetail.Direction = ParameterDirection.Input;
                            command.Parameters.Add(prmDetail);
                            prmDetail.Value = details;
                            command.ExecuteNonQuery();                            
                        }

                        strPath = ConfigurationManager.AppSettings["email_bodydetails"];
                        MailBody = File.ReadAllText(strPath);

                        MailBody = MailBody.Replace("@proposalNumber", propNumber);
                        MailBody = MailBody.Replace("@emailID", lblEmail.Text);
                        MailBody = MailBody.Replace("@mobileNumber", lblMobile.Text);
                        MailBody = MailBody.Replace("@details", txtDetails.Text);




                        MailMessage mm = new MailMessage();
                        mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                        mm.Subject = ConfigurationManager.AppSettings["email_subject"];
                        mm.Body = MailBody;
                        //mm.Body = "mail";
                        mm.IsBodyHtml = true;

                        for (int i = 0; i < arrMail.Count(); i++)
                        {

                            if (Regex.IsMatch(arrMail[i].Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                            {
                                mm.To.Add(arrMail[i]);

                            }
                        }

                        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["replyToEmail"]))
                        {
                            MailAddress adminAddress = new MailAddress(ConfigurationManager.AppSettings["replyToEmail"]);
                            mm.ReplyTo = adminAddress;
                        }

                        mm.BodyEncoding = UTF8Encoding.UTF8;
                        smtpClient.Send(mm);

                    }
                }

                else if (typeofUser.ToLower() != ConfigurationManager.AppSettings["epos_user"].Trim())
                {

                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["careEmail"].Trim()))
                    {
                        string emailId = ConfigurationManager.AppSettings["careEmail"].Trim();
                        string[] arrMail = emailId.Split(',');

                        string strPath = string.Empty;
                        string MailBody = string.Empty;
                        SmtpClient smtpClient = new SmtpClient();
                        smtpClient.Port = 25;
                        smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                        smtpClient.Timeout = 3600000;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);

                        Database db = DatabaseFactory.CreateDatabase("cnPASS");
                        using (SqlConnection con = new SqlConnection(db.ConnectionString))
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand("INSERT_PROPOSAL_MODIFY_DETAILS", con);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@proposal", proposal);
                            command.Parameters.AddWithValue("@mobile", lblMobile.Text);
                            command.Parameters.AddWithValue("@email", lblEmail.Text);
                            command.Parameters.AddWithValue("@quote", lblQuoteNumber.Text);
                            string details = txtDetails.Text.Replace('\r', ' ');
                            details = details.Replace('\n', ' ');

                            SqlParameter prmDetail = new SqlParameter();

                            prmDetail.ParameterName = "@text";
                            prmDetail.SqlDbType = SqlDbType.VarChar;
                            prmDetail.Direction = ParameterDirection.Input;
                            command.Parameters.Add(prmDetail);
                            prmDetail.Value = details;
                            command.ExecuteNonQuery();
                        }

                        strPath = ConfigurationManager.AppSettings["email_bodydetails"];
                        MailBody = File.ReadAllText(strPath);

                        MailBody = MailBody.Replace("@proposalNumber", proposal);
                        MailBody = MailBody.Replace("@emailID", lblEmail.Text);
                        MailBody = MailBody.Replace("@mobileNumber", lblMobile.Text);
                        MailBody = MailBody.Replace("@details", txtDetails.Text);


                        MailMessage mm = new MailMessage();
                        mm.From = new MailAddress(ConfigurationManager.AppSettings["smtp_mail_FromMailId"]);
                        mm.Subject = ConfigurationManager.AppSettings["email_subject"];
                        mm.Body = MailBody;
                        //mm.Body = "mail";
                        mm.IsBodyHtml = true;

                        for (int i = 0; i < arrMail.Count(); i++)
                        {

                            if (Regex.IsMatch(arrMail[i].Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                            {
                                mm.To.Add(arrMail[i]);

                            }
                        }

                        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["replyToEmail"]))
                        {
                            MailAddress adminAddress = new MailAddress(ConfigurationManager.AppSettings["replyToEmail"]);
                            mm.ReplyTo = adminAddress;
                        }

                        mm.BodyEncoding = UTF8Encoding.UTF8;
                        smtpClient.Send(mm);

                    }
                }
                Response.Redirect("FrmThankYouPage.aspx",false);
            }
            catch(Exception ex)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in sendmail details for proposal :" + proposal + " and error is " + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                Response.Redirect("FrmCustomErrorPage.aspx");
            }
        }

    }
}