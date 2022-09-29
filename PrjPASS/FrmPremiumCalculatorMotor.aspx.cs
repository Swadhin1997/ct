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
using System.Threading;
using cryptoPDF.api;

namespace PrjPASS
{

    public partial class FrmPremiumCalculatorMotor : System.Web.UI.Page
    {
        public decimal PACoverForOwnerDriver;
        public decimal TotalPremiumOwnDamage;

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
                if (Session["IsUWApproval"] != null)
                {
                    if (Convert.ToString(Session["IsUWApproval"]).ToUpper() == "FALSE" || Convert.ToString(Session["IsUWApproval"]) == "0")
                    {
                        trDeviation.Visible = false;
                        txtBasicODdeviation.Text = "0";
                        txtAddOnDeviation.Text = "0";
                    }
                    else
                    {
                        trDeviation.Visible = true;
                    }
                }
                else
                {
                    trDeviation.Visible = false;
                    txtBasicODdeviation.Text = "0";
                    txtAddOnDeviation.Text = "0";
                }

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

                if (Request.QueryString["quotenumber"] != null && Request.QueryString["quoteversion"] != null)
                {
                    DataSet dsQuoteDetails = new DataSet();

                    string QuoteNumber = Request.QueryString["quotenumber"].ToString().Trim();
                    string QuoteVersion = Request.QueryString["quoteversion"].ToString().Trim();
                    int intQuoteVersion;
                    bool isNumericQuoteVersion = int.TryParse(QuoteVersion, out intQuoteVersion);
                    if (isNumericQuoteVersion)
                    {
                        dsQuoteDetails = GetQuoteDetailsTobeModified(QuoteNumber, intQuoteVersion);
                        if (dsQuoteDetails != null)
                        {
                            if (dsQuoteDetails.Tables.Count > 0)
                            {
                                SetAllFields(dsQuoteDetails);
                            }
                            else
                            {
                                Alert.Show("No Data Exists for the given Quote Number and Quote version", "FrmMainMenu.aspx");
                                return;
                            }
                        }
                        else
                        {
                            Alert.Show("No Data Exists for the given Quote Number and Quote version", "FrmMainMenu.aspx");
                            return;
                        }
                    }
                    else
                    {
                        Alert.Show("Invalid Quote Version.", "FrmMainMenu.aspx");
                        return;
                    }
                }
                else
                {

                    FillDrpManufacturerList();

                    string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (rbbtNewBusiness.Checked)
                    {
                        txtDateOfRegistration.Text = strCurrentDate;
                        txtMfgYear.Text = DateTime.Now.Year.ToString();
                    }
                    else
                    {
                        string strDOR = DateTime.Now.AddYears(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        txtDateOfRegistration.Text = strDOR;
                        txtMfgYear.Text = DateTime.Now.AddYears(-1).Year.ToString();
                    }

                    string strPreviousPolicyExpiryDate = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    txtPreviousPolicyExpiryDate.Text = strPreviousPolicyExpiryDate;
                    txtPolicyStartDate.Text = strCurrentDate;


                    string strCustomerDOB = DateTime.Now.AddYears(-25).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    txtCustomerDOB.Text = strCustomerDOB;

                    GetIntermediary();
                    GetMinMaxMarketMovementForLoggedInUser();

                    chkSelectAllCovers.Enabled = false;
                    chkSelectAllCovers.Checked = false;

                    chkConsumableCover.Enabled = false;
                    chkDepreciationCover.Enabled = false;
                    chkEngineProtect.Enabled = false;
                    chkReturnToInvoice.Enabled = false;
                    chkRoadsideAssistance.Enabled = false;

                    chkNCBProtect.Enabled = false;
                    chkLossofPersonalBelongings.Enabled = false;
                    chkKeyReplacement.Enabled = false;
                    chkDailyCarAllowance.Enabled = false;
                    chkTyreCover.Enabled = false;

                    hdnEnabledAddOnsName.Value = "";
                }

            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseModalSaveProposal();", true);
            }

            lblDOR.Text = rbbtNewBusiness.Checked ? "Date of Purchase (dd/MM/yyyy)" : "Date of Registration (dd/MM/yyyy)";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalViewPremium();", true);
        }

        protected void FillDrpIntermediaryList()
        {
            //Database db = DatabaseFactory.CreateDatabase("cnPASS");

            //string sqlCommand = "PROC_GET_INTERMEDIARY";
            //DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            //dbCommand.CommandType = CommandType.StoredProcedure;
            //DataSet ds = null;
            //ds = db.ExecuteDataSet(dbCommand);

            //drpIntermediaryName.DataValueField = "TXT_INTERMEDIARY_CD";
            //drpIntermediaryName.DataTextField = "TXT_INTERMEDIARY_NAME";
            //drpIntermediaryName.DataSource = ds.Tables[0];
            //drpIntermediaryName.DataBind();

            //drpIntermediaryName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

            //lblIntermediaryCode.Text = drpIntermediaryName.SelectedValue;
        }

        protected void FillDrpRTOList(string REGISTRATION_CODE)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_RTO_LOCATION";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "NUM_REGISTRATION_CODE", DbType.String, ParameterDirection.Input, "NUM_REGISTRATION_CODE", DataRowVersion.Current, REGISTRATION_CODE);


            dbCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);

            drpRTOLocation.DataValueField = "TXT_RTO_LOCATION_CODE";
            drpRTOLocation.DataTextField = "TXT_RTO_LOCATION_DESC";
            drpRTOLocation.DataSource = ds.Tables[0];
            drpRTOLocation.DataBind();

            drpRTOLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            drpRTOLocation.SelectedIndex = 1;

            SetRTOCluster(drpRTOLocation.SelectedValue);
        }

        protected void FillDrpManufacturerList()
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_VEHICLE_MANUFACTURERS";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            dbCommand.CommandType = CommandType.StoredProcedure;
            DataSet dsManufacturers = null;
            dsManufacturers = db.ExecuteDataSet(dbCommand);

            drpVehicleMake.DataValueField = "VEHICLEMANUFACTURERCODE";
            drpVehicleMake.DataTextField = "VEHICLEMANUFACTURERNAME";
            drpVehicleMake.DataSource = dsManufacturers.Tables[0];
            drpVehicleMake.DataBind();

            drpVehicleMake.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

            FillDrpModelList(drpVehicleMake.SelectedValue);
        }

        protected void FillDrpModelList(string ManufacturerCode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_VEHICLE_MODEL";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "MANUFACTURERCODE", DbType.String, ParameterDirection.Input, "MANUFACTURERCODE", DataRowVersion.Current, ManufacturerCode);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                drpVehicleModel.DataValueField = "VEHICLEMODELCODE";
                drpVehicleModel.DataTextField = "VEHICLEMODEL";
                drpVehicleModel.DataSource = ds.Tables[0];
                drpVehicleModel.DataBind();

                drpVehicleModel.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

                FillDrpVehicleVariantList(drpVehicleMake.SelectedValue, drpVehicleModel.SelectedValue);
            }
        }

        protected void FillDrpVehicleVariantList(string ManufacturerCode, string ModelCode)
        {
            if (ModelCode.Length > 0)
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_VEHICLE_VARIANT";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "MANUFACTURERCODE", DbType.String, ParameterDirection.Input, "MANUFACTURERCODE", DataRowVersion.Current, ManufacturerCode);
                db.AddParameter(dbCommand, "MODELCODE", DbType.String, ParameterDirection.Input, "MODELCODE", DataRowVersion.Current, ModelCode);

                DataSet ds = null;
                ds = db.ExecuteDataSet(dbCommand);
                if (ds.Tables.Count > 0)
                {
                    drpVehicleSubType.DataValueField = "VEHICLEMODELCODE";
                    drpVehicleSubType.DataTextField = "TXT_VARIANT";
                    drpVehicleSubType.DataSource = ds.Tables[0];
                    drpVehicleSubType.DataBind();

                    drpVehicleSubType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

                    lblVehicleSegment.Text = "-";
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        lblVehicleSegment.Text = ds.Tables[1].Rows[0][0].ToString();
                    }

                    SetSeatingCapacityAndFuelType(drpVehicleMake.SelectedValue, drpVehicleModel.SelectedValue);

                    //CR164
                    if (ds.Tables.Count > 2)
                    {
                        if (ds.Tables[2] != null)
                        {
                            FillAddOnCoverSIoptions(ds.Tables[2]);
                        }
                    }
                }


            }
        }

        //CR164
        protected void FillAddOnCoverSIoptions(DataTable dt)
        {
            try
            {
                //Fill Loss of Personal Belongings
                ddlLossofPersonalBelongingsSI.Items.Clear();
                //ddlLossofPersonalBelongingsSI.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "Select"));
                DataRow[] Rows_LOPB = dt.Select(@"CoverShortName = 'LOPB'");
                foreach (DataRow row in Rows_LOPB)
                {
                    string[] SIOptions = row["SumInsuredOptions"].ToString().Split(',');
                    foreach (string option in SIOptions)
                    {
                        ddlLossofPersonalBelongingsSI.Items.Add(new System.Web.UI.WebControls.ListItem(option.ToString(), option.ToString()));
                    }

                    break; //break because only first matching row's sum insured options will be added on the list
                }


                //Fill Key Replacement
                ddlKeyReplacement.Items.Clear();
                //ddlKeyReplacement.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "Select"));
                DataRow[] Rows_KR = dt.Select(@"CoverShortName = 'KR'");
                foreach (DataRow row in Rows_KR)
                {
                    string[] SIOptions = row["SumInsuredOptions"].ToString().Split(',');
                    foreach (string option in SIOptions)
                    {
                        ddlKeyReplacement.Items.Add(new System.Web.UI.WebControls.ListItem(option.ToString(), option.ToString()));
                    }

                    break; //break because only first matching row's sum insured options will be added on the list
                }


                //Fill Daily Car Allowance
                ddlDailyCarAllowance.Items.Clear();
                //ddlDailyCarAllowance.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "Select"));
                DataRow[] Rows_DCA = dt.Select(@"CoverShortName = 'DCA'");
                foreach (DataRow row in Rows_DCA)
                {
                    string[] SIOptions = row["SumInsuredOptions"].ToString().Split(',');
                    foreach (string option in SIOptions)
                    {
                        ddlDailyCarAllowance.Items.Add(new System.Web.UI.WebControls.ListItem(option.ToString(), option.ToString()));
                    }

                    break; //break because only first matching row's sum insured options will be added on the list
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FillAddOnCoverSIoptions CR164");
                throw;
            }
        }


        protected void SetRTOCluster(string RTO_LOCATION_CODE)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_LOCATION_CLUSTER";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "RTO_LOCATION_CODE", DbType.String, ParameterDirection.Input, "RTO_LOCATION_CODE", DataRowVersion.Current, RTO_LOCATION_CODE);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblRTOCluster.Text = ds.Tables[0].Rows[0]["TXT_RTO_CLUSTER"].ToString().ToUpper();
                    hdnRTOZone.Value = ds.Tables[0].Rows[0]["TXT_REGISTRATION_ZONE"].ToString();
                }
            }
        }
        protected void SetSeatingCapacityAndFuelType(string ManufacturerCode, string ModelCode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_VEHICLE_SEATING_CAPACITY_AND_FUEL_TYPE";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "MANUFACTURERCODE", DbType.String, ParameterDirection.Input, "MANUFACTURERCODE", DataRowVersion.Current, ManufacturerCode);
            db.AddParameter(dbCommand, "MODELCODE", DbType.String, ParameterDirection.Input, "MODELCODE", DataRowVersion.Current, ModelCode);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblSeatingCapacityt.Text = ds.Tables[0].Rows[0][0].ToString();
                    lblFuelTypet.Text = ds.Tables[0].Rows[0][1].ToString();
                    txtNumberOfPersonsUnnamed.Text = ds.Tables[0].Rows[0][0].ToString();

                    lblModelCluster.Text = ds.Tables[0].Rows[0][2].ToString().ToUpper();
                    lblCubicCapacityt.Text = ds.Tables[0].Rows[0][3].ToString();
                }

                //if (ds.Tables[1].Rows.Count > 0)
                //{
                //    txtIDVofVehicle.Text = ds.Tables[1].Rows[0][0].ToString();
                //}
                //else
                //{
                //    txtIDVofVehicle.Text = "0";
                //}
            }
        }
        protected void SetFuelType(string strManufacturerCode, string strVehicleModel, string strVariant, string strVehicleModelCode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_FUEL_TYPE2";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "MANUFACTURERCODE", DbType.String, ParameterDirection.Input, "MANUFACTURERCODE", DataRowVersion.Current, strManufacturerCode);
            db.AddParameter(dbCommand, "VEHICLEMODEL", DbType.String, ParameterDirection.Input, "VEHICLEMODEL", DataRowVersion.Current, strVehicleModel);
            db.AddParameter(dbCommand, "TXT_VARIANT", DbType.String, ParameterDirection.Input, "TXT_VARIANT", DataRowVersion.Current, strVariant);
            db.AddParameter(dbCommand, "VEHICLEMODELCODE", DbType.String, ParameterDirection.Input, "VEHICLEMODELCODE", DataRowVersion.Current, strVehicleModelCode);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblSeatingCapacityt.Text = ds.Tables[0].Rows[0][0].ToString();
                    lblFuelTypet.Text = ds.Tables[0].Rows[0][1].ToString();
                    txtNumberOfPersonsUnnamed.Text = ds.Tables[0].Rows[0][0].ToString();

                    lblModelCluster.Text = ds.Tables[0].Rows[0][2].ToString().ToUpper();
                    lblCubicCapacityt.Text = ds.Tables[0].Rows[0][3].ToString();
                }

                //if (ds.Tables[1].Rows.Count > 0)
                //{
                //    txtIDVofVehicle.Text = ds.Tables[1].Rows[0][0].ToString();
                //}
                //else
                //{
                //    txtIDVofVehicle.Text = "0";
                //}
            }
        }

        protected void SetIntermediaryBusinessChaneelType(string strIntermediaryCode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");

            string sqlCommand = "PROC_GET_INTERMEDIARY_BUSINESS_CHANNEL_TYPE";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

            db.AddParameter(dbCommand, "TXT_INTERMEDIARY_CD", DbType.String, ParameterDirection.Input, "@TXT_INTERMEDIARY_CD", DataRowVersion.Current, strIntermediaryCode);

            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblIntermediaryBusineeChannelType.Text = ds.Tables[0].Rows[0]["TXT_CHANNEL_NAME"].ToString();
                    hdnPrimaryMOCode.Value = ds.Tables[0].Rows[0]["PrimaryMOCode"].ToString();
                    hdnPrimaryMOName.Value = ds.Tables[0].Rows[0]["PrimaryMOName"].ToString();
                    hdnOfficeCode.Value = ds.Tables[0].Rows[0]["NUM_OFFICE_CD"].ToString();
                    hdnOfficeName.Value = ds.Tables[0].Rows[0]["TXT_OFFICE"].ToString();
                    hdnIntermediaryType.Value = ds.Tables[0].Rows[0]["IntermediaryType"].ToString();

                    if ((ds.Tables[0].Rows[0]["TXT_CHANNEL_NAME"].ToString().ToUpper().Trim() == "CORPORATE AGENT" || ds.Tables[0].Rows[0]["TXT_CHANNEL_NAME"].ToString().ToUpper().Trim() == "BANCASSURANCE") && rbbtRollOver.Checked)
                    {
                        tdNCBProtectCover1.Visible = true;
                        tdNCBProtectCover2.Visible = true;
                        chkNCBProtect.Visible = true;
                    }
                    else
                    {
                        tdNCBProtectCover1.Visible = false;
                        tdNCBProtectCover2.Visible = false;
                        chkNCBProtect.Checked = false;
                        chkNCBProtect.Visible = false;
                    }
                }
            }
        }

        //protected void drpIntermediaryName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    lblIntermediaryCode.Text = drpIntermediaryName.SelectedValue;
        //    SetIntermediaryBusinessChaneelType(drpIntermediaryName.SelectedValue);
        //}

        protected void drpRTOLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRTOCluster(drpRTOLocation.SelectedValue);
        }

        protected void drpVehicleMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIDVofVehicle.Text = "0";
            hdnFinalIDV.Value = "0";
            txtIDVofVehicle.Enabled = false;
            lblCubicCapacityt.Text = "-";
            lblModelCluster.Text = "-";
            lblSeatingCapacityt.Text = "-";
            lblFuelTypet.Text = "-";

            FillDrpModelList(drpVehicleMake.SelectedValue);
        }

        protected void drpVehicleModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIDVofVehicle.Text = "0";
            hdnFinalIDV.Value = "0";
            txtIDVofVehicle.Enabled = false;
            lblCubicCapacityt.Text = "-";
            FillDrpVehicleVariantList(drpVehicleMake.SelectedValue, drpVehicleModel.SelectedValue);
        }

        protected void drpVehicleSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIDVofVehicle.Text = "0";
            hdnFinalIDV.Value = "0";
            txtIDVofVehicle.Enabled = false;

            if (drpVehicleSubType.SelectedIndex != 0)
            {
                string[] strVST = drpVehicleSubType.SelectedItem.Text.Split('[');
                SetFuelType(drpVehicleMake.SelectedValue, drpVehicleModel.SelectedItem.Text, strVST[0].Trim(), strVST[1].Replace("]", "").Trim());
                GetVehicleAgeNew(IsQuoteModification: false); //added on 14-Sep-2017
            }
        }

        //FILL SEATING CAPACITY
        //FILL FUEL TYPE

        private string GetRequestXML(bool IsPremiumCalc, long CustomerId, string CustomerName, string CreditScore)
        {
            string strXmlPath = "";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarWorkingFine.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarNew.XML";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCar.XML";
            if (IsPremiumCalc)
            {
                strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "new.XML";
            }
            else
            {
                strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarSaveProposal_CP.XML";
                //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PrivateCarSaveProposal_Request_New.XML";
            }

            XmlNode node = null;
            XmlDocument xmlfile = null;
            string xmlString = "";
            try
            {
                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);

                DateTime dtAppDate = DateTime.Now;
                string strCurrentDate = dtAppDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropMODetails_PrimaryMOCode");
                node.Attributes["Value"].Value = hdnPrimaryMOCode.Value;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropMODetails_PrimaryMOName");
                node.Attributes["Value"].Value = hdnPrimaryMOName.Value;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_BranchOfficeCode");
                node.Attributes["Value"].Value = hdnOfficeCode.Value;

                string officeCode = hdnOfficeCode.Value;
                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_DisplayOfficeCode");
                node.Attributes["Value"].Value = officeCode.Substring(1, officeCode.Length - 1);

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_OfficeCode");
                node.Attributes["Value"].Value = hdnOfficeCode.Value;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_OfficeName");
                node.Attributes["Value"].Value = hdnOfficeName.Value;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_NoPrevInsuranceFlag");
                node.Attributes["Value"].Value = rbbtNewBusiness.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralNodes_ApplicationDate");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropBranchDetails_IMDBranchName");
                node.Attributes["Value"].Value = ""; // drpBranchName.SelectedItem.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropBranchDetails_IMDBranchCode");
                node.Attributes["Value"].Value = ""; // drpBranchName.SelectedValue;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryCode");
                node.Attributes["Value"].Value = lblIntermediaryCode.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryName");
                node.Attributes["Value"].Value = txtIntermediaryName.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropProductName");
                node.Attributes["Value"].Value = "Comprehensive Policy"; //drpProductType.SelectedValue;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropFieldUserDetails_FiledUserUserID");
                node.Attributes["Value"].Value = "";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_LeadGenerator");
                //node.Attributes["Value"].Value = txtLeadGenerator.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_BusinessType_Mandatary");
                node.Attributes["Value"].Value = rbbtNewBusiness.Checked ? "New Business" : "Roll Over";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropDistributionChannel_BusineeChanneltype");
                node.Attributes["Value"].Value = lblIntermediaryBusineeChannelType.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropIntermediaryDetails_IntermediaryType");
                node.Attributes["Value"].Value = hdnIntermediaryType.Value;

                string CUSTOMERID_INDIVIDUAL = ConfigurationManager.AppSettings["CUSTOMERID_INDIVIDUAL"].ToString();
                string CUSTOMERNAME_INDIVIDUAL = ConfigurationManager.AppSettings["CUSTOMERNAME_INDIVIDUAL"].ToString();

                string CUSTOMERID_INDIVIDUAL_FEMALE = ConfigurationManager.AppSettings["CUSTOMERID_INDIVIDUAL_FEMALE"].ToString();
                string CUSTOMERNAME_INDIVIDUAL_FEMALE = ConfigurationManager.AppSettings["CUSTOMERNAME_INDIVIDUAL_FEMALE"].ToString();

                string CUSTOMERID_ORG = ConfigurationManager.AppSettings["CUSTOMERID_ORG"].ToString();
                string CUSTOMERNAME_ORG = ConfigurationManager.AppSettings["CUSTOMERNAME_ORG"].ToString();

                if (rbctIndividual.Checked)
                {
                    if (drpCustomerGender.SelectedIndex == 0)
                    {
                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerID_Mandatary");
                        node.Attributes["Value"].Value = CustomerId > 0 ? CustomerId.ToString() : (rbctIndividual.Checked ? CUSTOMERID_INDIVIDUAL : CUSTOMERID_ORG);

                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerName");
                        node.Attributes["Value"].Value = CustomerId > 0 ? CustomerName : (rbctIndividual.Checked ? CUSTOMERNAME_INDIVIDUAL : CUSTOMERNAME_ORG);
                    }
                    else
                    {
                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerID_Mandatary");
                        node.Attributes["Value"].Value = CustomerId > 0 ? CustomerId.ToString() : CUSTOMERID_INDIVIDUAL_FEMALE;

                        node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerName");
                        node.Attributes["Value"].Value = CustomerId > 0 ? CustomerName : CUSTOMERNAME_INDIVIDUAL_FEMALE;
                    }
                }
                else
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerID_Mandatary");
                    node.Attributes["Value"].Value = CustomerId > 0 ? CustomerId.ToString() : CUSTOMERID_ORG;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerName");
                    node.Attributes["Value"].Value = CustomerId > 0 ? CustomerName : CUSTOMERNAME_ORG;
                }



                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_MainDriver");
                node.Attributes["Value"].Value = rbctIndividual.Checked ? "Self - Owner Driver" : "Any Other";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ExtraDD5");
                node.Attributes["Value"].Value = rbctIndividual.Checked ? drpCustomerGender.SelectedValue : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ExtraDD1");
                node.Attributes["Value"].Value = drpProductType.SelectedValue;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ExtraDD6");
                node.Attributes["Value"].Value = drpTenureOwnerDriver.SelectedValue == "0" ? "" : drpTenureOwnerDriver.SelectedValue;

                // CR 132 started here
                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_BasicODDeviation");
                node.Attributes["Value"].Value = txtBasicODdeviation.Text;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_AddOnDeviation");
                node.Attributes["Value"].Value = txtAddOnDeviation.Text;

                // CR 132 ended here
                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropCustomerDtls_CustomerType");
                node.Attributes["Value"].Value = rbctIndividual.Checked ? "I" : "C"; //rbctIndividual.Checked ? "Individual" : "Organization";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_PrevYearNCB");
                node.Attributes["Value"].Value = rbbtRollOver.Checked ? drpPreviousYearNCBSlab.SelectedValue : "0";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPreviousPolicyDetails_NCBPercentage");
                node.Attributes["Value"].Value = rbbtRollOver.Checked ? drpPreviousYearNCBSlab.SelectedValue : "0";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Manufacture");
                node.Attributes["Value"].Value = drpVehicleMake.SelectedItem.Text.ToUpper();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ManufacturerCode");
                node.Attributes["Value"].Value = drpVehicleMake.SelectedValue.ToUpper();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Model");
                node.Attributes["Value"].Value = drpVehicleModel.SelectedItem.Text.ToUpper();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ModelCode"); //uncommented on 06-Oct-2017
                node.Attributes["Value"].Value = drpVehicleModel.SelectedValue;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_TypeofPolicyHolder");
                node.Attributes["Value"].Value = drpPolicyHolderType.SelectedValue;


                if (drpVehicleSubType.SelectedIndex > 0)
                {
                    string[] strVST = drpVehicleSubType.SelectedItem.Text.Split('[');

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ModelVariant");
                    node.Attributes["Value"].Value = strVST[0].Trim().ToUpper();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VariantCode");
                    node.Attributes["Value"].Value = drpVehicleSubType.SelectedValue.ToUpper();
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VehicleSegment");
                node.Attributes["Value"].Value = lblVehicleSegment.Text.Trim().ToUpper();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_SeatingCapacity");
                node.Attributes["Value"].Value = lblSeatingCapacityt.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_FuelType");
                node.Attributes["Value"].Value = lblFuelTypet.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IsExternalCNGLPGAvailable");
                node.Attributes["Value"].Value = chkCNGLPG.Checked ? "True" : "False";

                if (chkCNGLPG.Checked)
                {
                    StringBuilder OtherDetailsGrid_InnerXML = new StringBuilder();
                    string strExternalCNGLPG_XML_Node = string.Format(@"<CNGandLPGKitDetails Name=""CNG and LPG Kit"" Value=""0""><CNGandLPGKitDetails Type=""GroupData""><SI Name=""SI"" Value=""{0}"" Type=""Double""/></CNGandLPGKitDetails></CNGandLPGKitDetails>", txtLPGKitSumInsured.Text.Trim());
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    OtherDetailsGrid_InnerXML = new StringBuilder(node.InnerXml);
                    OtherDetailsGrid_InnerXML.Append(strExternalCNGLPG_XML_Node);
                    node.InnerXml = OtherDetailsGrid_InnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CNGLPGkitValue");
                    node.Attributes["Value"].Value = txtLPGKitSumInsured.Text.Trim();
                }

                DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strDOR = dtDOR.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DateofRegistration");
                node.Attributes["Value"].Value = strDOR;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Dateofpurchase");
                node.Attributes["Value"].Value = strDOR;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ManufactureYear");
                node.Attributes["Value"].Value = txtMfgYear.Text;


                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VehicleAge_Mandatary");
                //node.Attributes["Value"].Value = txtVehicleAge.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RTOCode");
                node.Attributes["Value"].Value = drpRTOLocation.SelectedValue.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_AuthorityLocation");
                node.Attributes["Value"].Value = hfSelectedRTO.Value; // txtRTOAuthorityCode.Text.Trim();


                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RTOCluster");
                node.Attributes["Value"].Value = lblRTOCluster.Text.Trim().ToUpper();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_Zone_Mandatary");
                node.Attributes["Value"].Value = hdnRTOZone.Value.Trim() == "" ? "Zone-A" : hdnRTOZone.Value.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CubicCapacity");
                node.Attributes["Value"].Value = lblCubicCapacityt.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ModelCluster");
                node.Attributes["Value"].Value = lblModelCluster.Text.Trim().ToUpper();

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CSD");
                //node.Attributes["Value"].Value = chkCSD.Checked ? "True" : "False";

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfClaimFreeYearsCompleted");
                //node.Attributes["Value"].Value = txtNoofClaimFreeYearsCompleted.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IDVofthevehicle");
                node.Attributes["Value"].Value = txtIDVofVehicle.Text.Trim() == "" ? "0" : txtIDVofVehicle.Text.Trim();

                if (chkNEAR.Checked)
                {
                    StringBuilder NEAR_InnerXML = new StringBuilder();
                    string XML_Node = string.Format(@"<NonElectricalAccessoriesDetails Name=""Non Electrical Accessories"" Value=""0""><NonElectricalAccessoriesDetails Type=""GroupData""><SI Name=""SI"" Value=""{0}"" Type=""Double""/></NonElectricalAccessoriesDetails></NonElectricalAccessoriesDetails><NonElectricalAccessiories Name=""NonElectricalAccessiories"" Value=""GRP288""><NonElectricalAccessiories Type=""GroupData""><Make Name=""Make"" Value=""hhhh"" Type=""String"" /><Model Name=""Model"" Value=""h11"" Type=""String"" /><Year Name=""Year"" Value=""2016"" Type=""Double"" /><IDV Name=""IDV"" Value=""{0}"" Type=""Double"" /><TypeofAccessories Name=""TypeofAccessories"" Value=""Lugguage Carrier"" Type=""String"" /><Description Name=""Description"" Value=""iiii"" Type=""String"" /><SerialNo Name=""SerialNo"" Value=""h1111"" Type=""String"" /><Remarks Name=""Remarks"" Value=""jjjj"" Type=""String"" /></NonElectricalAccessiories></NonElectricalAccessiories>", txtNeaSumInsured.Text.Trim(), txtNeaSumInsured.Text.Trim());
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    NEAR_InnerXML = new StringBuilder(node.InnerXml);
                    NEAR_InnerXML.Append(XML_Node);
                    node.InnerXml = NEAR_InnerXML.ToString();
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NonElectricalAccessories");
                    node.Attributes["Value"].Value = txtNeaSumInsured.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IsNonElctrclAcesrsRequired");
                    node.Attributes["Value"].Value = "True";
                }

                if (chkEAR.Checked)
                {
                    StringBuilder EAR_InnerXML = new StringBuilder();
                    string XML_Node = string.Format(@"<ElectronicAccessoriesDetails Name=""Electronic Accessories"" Value=""0""><ElectronicAccessoriesDetails Type=""GroupData""><SI Name=""SI"" Value=""{0}"" Type=""Double""/></ElectronicAccessoriesDetails></ElectronicAccessoriesDetails><ElectricalAccessiories Name=""ElectricalAccessiories"" Value=""GRP289""><ElectricalAccessiories Type=""GroupData""><Make Name=""Make"" Value=""kkkk"" Type=""String"" /><Model Name=""Model"" Value=""k11"" Type=""String"" /><Year Name=""Year"" Value=""2015"" Type=""Double"" /><IDV Name=""IDV"" Value=""{0}"" Type=""Double"" /><TypeofAccessories Name=""TypeofAccessories"" Value=""Fan"" Type=""String"" /><Description Name=""Description"" Value=""lllll"" Type=""String"" /><SerialNo Name=""SerialNo"" Value=""l22"" Type=""String"" /><Remarks Name=""Remarks"" Value=""mmmm"" Type=""String"" /></ElectricalAccessiories></ElectricalAccessiories>", txtEaSumInsured.Text.Trim(), txtEaSumInsured.Text.Trim());
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    EAR_InnerXML = new StringBuilder(node.InnerXml);
                    EAR_InnerXML.Append(XML_Node);
                    node.InnerXml = EAR_InnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ElectricalAccessories");
                    node.Attributes["Value"].Value = txtEaSumInsured.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_EmployeesDiscount");
                    node.Attributes["Value"].Value = "True";
                }

                if (rbctIndividual.Checked)
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_IsPAToOwnerDriverExcluded");
                    node.Attributes["Value"].Value = drpTenureOwnerDriver.SelectedValue == "0" ? "True" : "False";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ValidDrvgLisc");
                    node.Attributes["Value"].Value = drpTenureOwnerDriver.SelectedValue == "0" ? "False" : "True";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CompulsoryPAwithOwnerDriver");
                    node.Attributes["Value"].Value = drpTenureOwnerDriver.SelectedValue == "0" ? "False" : "True";
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
                //node.Attributes["Value"].Value = rbctIndividual.Checked ? "True" : "False"; //rbCpwYes.Checked ? "True" : "False"; 

                // this rbCpwYes is commented because as per the new requirement when customer type is individual then 
                // only CompulsoryPAwithOwnerDriver will be true else false;


                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ImprtdVehcleWoutPaymtCustmDuty");
                //node.Attributes["Value"].Value = rbivwpYes.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VoluntaryDeductibleAmount");
                node.Attributes["Value"].Value = drpVDA.SelectedValue;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ReturnToInvoice");
                node.Attributes["Value"].Value = chkReturnToInvoice.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_RoadsideAssistance");
                node.Attributes["Value"].Value = chkRoadsideAssistance.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_EngineSecure");
                node.Attributes["Value"].Value = chkEngineProtect.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DepreciationReimbursement");
                node.Attributes["Value"].Value = chkDepreciationCover.Checked ? "True" : "False";


                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_VlntryDedctbleFrDprctnCover");
                node.Attributes["Value"].Value = chkDepreciationCover.Checked ? ConfigurationManager.AppSettings["VDEPCoverAmount"].ToString() : "0";


                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_ConsumablesExpenses");
                node.Attributes["Value"].Value = chkConsumableCover.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_MarketMovement");
                node.Attributes["Value"].Value = txtMarketMovement.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_InsuredCreditScore");
                node.Attributes["Value"].Value = CreditScore; // txtInsuredCreditScore.Text.Trim();

                //CR164
                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_LossofPersonalBelongingschk");
                node.Attributes["Value"].Value = chkLossofPersonalBelongings.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_KeyReplacementChk");
                node.Attributes["Value"].Value = chkKeyReplacement.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NCBProtectChk");
                node.Attributes["Value"].Value = chkNCBProtect.Visible && chkNCBProtect.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_TyreCoverChk");
                node.Attributes["Value"].Value = chkTyreCover.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DailyCarAllowanceChk");
                node.Attributes["Value"].Value = chkDailyCarAllowance.Checked ? "True" : "False";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_LossofPersonalBelongingsSIDD");
                node.Attributes["Value"].Value = chkLossofPersonalBelongings.Checked ? ddlLossofPersonalBelongingsSI.SelectedValue : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_KeyReplacementSIDD");
                node.Attributes["Value"].Value = chkKeyReplacement.Checked ? ddlKeyReplacement.SelectedValue : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_DailyCarAllowanceSIDD");
                node.Attributes["Value"].Value = chkDailyCarAllowance.Checked ? ddlDailyCarAllowance.SelectedValue : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_TyreCoverText");
                node.Attributes["Value"].Value = chkTyreCover.Checked ? txtTyreCoverDetails.Text.Trim() : "";
                //

                if (chkPACoverUnnamedPersons.Checked)
                {
                    StringBuilder sbInnerXML = new StringBuilder();
                    string XML_Node = @"<UnnamedPAcoverDetails Name=""Unnamed PA cover"" Value=""0""><UnnamedPAcoverDetails Type=""GroupData""><SI Name=""SI"" Value=""0"" Type=""Double""/></UnnamedPAcoverDetails></UnnamedPAcoverDetails>";
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    sbInnerXML = new StringBuilder(node.InnerXml);
                    sbInnerXML.Append(XML_Node);
                    node.InnerXml = sbInnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NumberofPersonsUnnamed");
                    node.Attributes["Value"].Value = txtNumberOfPersonsUnnamed.Text.Trim();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_CapitalSIPerPerson");
                    node.Attributes["Value"].Value = drpCapitalSIPerPerson.Text.Trim();
                }

                /*if (chkPACoverNamedPersons.Checked)
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
                    node.Attributes["Value"].Value = txtNumberofPersonsNamed.Text.Trim();

                    StringBuilder sbInnerXML2 = new StringBuilder();
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail");
                    sbInnerXML2 = new StringBuilder(node.InnerXml);

                    string xmlnode2 = @"<NamedpassengerNomineeDetail Type=""GroupData""><NamedPerson Name=""NamedPerson"" Value=""Rahul8"" Type=""String""/><CapitalSIPerPerson Name=""CapitalSIPerPerson"" Value=""0"" Type=""Double""/><Nominee Name=""Nominee"" Value=""ooooo"" Type=""String""/><Relationship Name=""Relationship"" Value=""Dependent Son"" Type=""String""/><AgeofNominee Name=""AgeofNominee"" Value=""16"" Type=""Double""/><Nameofappointee Name=""Nameofappointee"" Value=""pppp"" Type=""String""/><RelationshipToNominee Name=""RelationshipToNominee"" Value=""Dependent Son"" Type=""String""/></NamedpassengerNomineeDetail>";
                    for (int i = 0; i < Convert.ToInt16(txtNumberofPersonsNamed.Text.Trim()); i++)
                    {
                        sbInnerXML2.Append(xmlnode2);
                    }

                    node.InnerXml = sbInnerXML2.ToString();

                    int intNumberofPersonsNamed = 0;
                    XmlNodeList nodeList_NamedpassengerNomineeDetail = node.SelectNodes("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail/NamedpassengerNomineeDetail");
                    foreach (XmlElement item in nodeList_NamedpassengerNomineeDetail)
                    {
                        intNumberofPersonsNamed++;
                        node = item.SelectSingleNode("CapitalSIPerPerson");
                        node.Attributes["Value"].Value = drpCapitalSINamed.Text.Trim();

                        if (intNumberofPersonsNamed == Convert.ToInt16(txtNumberofPersonsNamed.Text.Trim()))
                        {
                            break;
                        }
                    }

                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid/NamedpassengerNomineeDetail/NamedpassengerNomineeDetail/CapitalSIPerPerson");
                    //node.Attributes["Value"].Value = drpCapitalSINamed.Text.Trim();
                }*/

                if (chkPACoverPaidDriver.Checked)
                {
                    StringBuilder sbInnerXML = new StringBuilder();
                    string XML_Node = @"<PaiddriverPAcoverDetails Name=""Paid driver PA cover"" Value=""0""><PaiddriverPAcoverDetails Type=""GroupData""><SI Name=""SI"" Value=""0"" Type=""Double""/></PaiddriverPAcoverDetails></PaiddriverPAcoverDetails>";
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/OtherDetailsGrid");
                    sbInnerXML = new StringBuilder(node.InnerXml);
                    sbInnerXML.Append(XML_Node);
                    node.InnerXml = sbInnerXML.ToString();

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonsPaidDriver");
                    node.Attributes["Value"].Value = drpNoofPaidDrivers.SelectedValue;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_SumInsuredPaidDriver");
                    node.Attributes["Value"].Value = drpSIPaidDriver.SelectedValue;
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_WiderLegalLiabilityToPaid");
                node.Attributes["Value"].Value = chkWLLPD.Checked ? "True" : "False";

                if (chkWLLPD.Checked)
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonWiderLegalLiability");
                    node.Attributes["Value"].Value = txtNoofPersonsWLL.Text.Trim();
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_LegalLiabilityToEmployees");
                node.Attributes["Value"].Value = chkLLEE.Checked ? "True" : "False";

                if (chkLLEE.Checked)
                {
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoOfPersonsLiabilityEmployee");
                    node.Attributes["Value"].Value = txtNoOfEmployees.Text.Trim();
                }

                //DateTime dtAppDate1 = DateTime.Now.AddDays(2);
                //string strCurrentDate1 = dtAppDate1.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "Error2.txt";
                if (rbbtRollOver.Checked)
                {
                    #region logic before policy start date field
                    //DateTime dt = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //dt = dt.AddDays(1);
                    //string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                    //node.Attributes["Value"].Value = strdate;
                    #endregion

                    DateTime dt = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                    node.Attributes["Value"].Value = strdate;
                }
                else
                {
                    #region logic before policy start date field
                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                    //node.Attributes["Value"].Value = strCurrentDate;
                    #endregion

                    DateTime dt = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_PolicyEffectivedate");
                    node.Attributes["Value"].Value = strdate;
                }

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromhour_Mandatary");
                node.Attributes["Value"].Value = "00:00"; // DateTime.Now.ToShortTimeString();


                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_ProposalDate_Mandatary");
                node.Attributes["Value"].Value = strCurrentDate;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralNodes_PartnerApplicationDate");
                //node.Attributes["Value"].Value = strCurrentDate;

                //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_BranchInwardDate");
                //node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropReferenceNoDate_ReferenceDate_Mandatary");
                node.Attributes["Value"].Value = strCurrentDate;

                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_BreakInInsurance");
                node.Attributes["Value"].Value = GetBreakInInsuranceType();

                if (rbbtRollOver.Checked)
                {
                    #region logic before policy start date field
                    //DateTime dt = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //dt = dt.AddDays(1);
                    //string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                    //node.Attributes["Value"].Value = strdate;

                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    //node.Attributes["Value"].Value = dt.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    #endregion


                    DateTime dt = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                    node.Attributes["Value"].Value = strdate;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    //node.Attributes["Value"].Value = dt.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node.Attributes["Value"].Value = FN_Get_Policy_End_Date(strdate, 1);
                }
                else
                {
                    #region logic before policy start date field
                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    //node.Attributes["Value"].Value = DateTime.Now.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    //node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                    //node.Attributes["Value"].Value = strCurrentDate;
                    #endregion

                    DateTime dt = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Fromdate_Mandatary");
                    node.Attributes["Value"].Value = strdate;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPolicyEffectivedate_Todate_Mandatary");
                    //node.Attributes["Value"].Value = dt.AddYears(3).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node.Attributes["Value"].Value = FN_Get_Policy_End_Date(strdate, 3);
                }

                if (rbbtRollOver.Checked)
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


                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropPreviousPolicyDetails_PreviousPolicyType");
                    node.Attributes["Value"].Value = drpCoverType.SelectedValue;
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_TotalClaimsAmount");
                    node.Attributes["Value"].Value = txtTotalClaimAmount.Text;

                    DateTime dtPE = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //dtPE = dtPE.AddDays(-1);
                    string strPE = dtPE.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyEffectiveTo");
                    node.Attributes["Value"].Value = strPE;

                    string PreviousPolicyStartDate = dtPE.AddYears(-1).AddDays(1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyEffectiveFrom");
                    node.Attributes["Value"].Value = PreviousPolicyStartDate;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_NoOfClaims");
                    node.Attributes["Value"].Value = drpTotalClaimCount.SelectedValue;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_DocumentProof");
                    node.Attributes["Value"].Value = drpTotalClaimCount.SelectedValue;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_ClaimAmount");
                    node.Attributes["Value"].Value = txtTotalClaimAmount.Text;

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyNo");
                    node.Attributes["Value"].Value = "11223344";

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/PreviousPolicyDetails/PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyYear_Mandatary");
                    node.Attributes["Value"].Value = dtPE.AddYears(-1).Year.ToString();  //"2015";

                    if (drpCoverType.SelectedIndex == 0) //only comprehensive product type
                    {
                        if (drpTotalClaimCount.SelectedIndex == 0) //no claim
                        {
                            if (drpPreviousYearNCBSlab.SelectedItem.Text == "50" || drpPreviousYearNCBSlab.SelectedItem.Text == "55" || drpPreviousYearNCBSlab.SelectedItem.Text == "65")
                            {
                                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoClaimBonusApplicable");
                                node.Attributes["Value"].Value = drpPreviousYearNCBSlab.SelectedItem.Text;
                            }
                            else
                            {
                                string selectedValue = drpPreviousYearNCBSlab.SelectedItem.Text;
                                drpPreviousYearNCBSlab.SelectedIndex = drpPreviousYearNCBSlab.SelectedIndex + 1;
                                node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NoClaimBonusApplicable");
                                node.Attributes["Value"].Value = drpPreviousYearNCBSlab.SelectedItem.Text;
                                drpPreviousYearNCBSlab.SelectedIndex = drpPreviousYearNCBSlab.SelectedItem.Text == selectedValue ? drpPreviousYearNCBSlab.SelectedIndex : drpPreviousYearNCBSlab.SelectedIndex - 1;
                            }
                            node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/GeneralProposalInformation/PropGeneralProposalInformation_IsNCBApplicable");
                            node.Attributes["Value"].Value = "True";
                        }
                    }

                    node = xmlfile.DocumentElement.SelectSingleNode("/Root/ProposalDetails/RiskDetails/Block/PropRisks_NCBConfirmation");
                    node.Attributes["Value"].Value = "On declaration";
                }

                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");

            }
            catch (Exception ex)
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPath, "Error: " + ex.StackTrace.ToString());

                ExceptionUtility.LogException(ex, "GetRequestXML " + txtPreviousPolicyExpiryDate.Text);

                lblstatus.Text = ex.Message; // ex.StackTrace; //Commented the stack trace due to the VAPT point "Internal Path Disclosure" 
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


        private string GetBreakInInsuranceType()
        {
            string BreakInInsuranceType = "No Break";
            try
            {
                DateTime dtPreviousPolicyExpiryDate = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string strPE = dtPreviousPolicyExpiryDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime dtPolicyStartDate = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string strdate = dtPolicyStartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                int numDaysDiff = Math.Abs(dtPolicyStartDate.Subtract(dtPreviousPolicyExpiryDate).Days);

                if (numDaysDiff <= 1)
                {
                    BreakInInsuranceType = "No Break";
                }
                else if (numDaysDiff >= 2 && numDaysDiff <= 30)
                {
                    BreakInInsuranceType = "Yes - Upto 30 Days";
                }
                else if (numDaysDiff >= 31 && numDaysDiff <= 60)
                {
                    BreakInInsuranceType = "Yes - 31 To 60 Days";
                }
                else if (numDaysDiff >= 61 && numDaysDiff <= 90)
                {
                    BreakInInsuranceType = "Yes - 61 To 90 Days";
                }
                else if (numDaysDiff >= 91)
                {
                    BreakInInsuranceType = "Yes - beyond 90 Days";
                }
            }
            catch (Exception ex)
            {
                BreakInInsuranceType = "No Break";
                ExceptionUtility.LogException(ex, "GetBreakInInsuranceType Method");
            }
            return BreakInInsuranceType;
        }

        private string GetResultXML(string ResultXML, string strQuoteNo, string NetPremium, string ServiceTax, string TotalPremium, string CGSTAmount, string CGSTPercentage, string SGSTAmount, string SGSTPercentage, string IGSTAmount, string IGSTPercentage, string UGSTAmount, string UGSTPercentage, string TotalGSTAmount, int MaxQuoteVersion)
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
            lblRateBasicOD.Text = "0.00";
            lblRateCC.Text = "0.00";
            lblRateDC.Text = "0.00";
            lblRateEP.Text = "0.00";
            lblRateRTI.Text = "0.00";

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

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_BasicODDeviation");
                strBasicODDeviation = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_AddOnDeviation");
                strAddOnDeviation = SingleNode.InnerXml;

                //lblQuoteNumber.Text = strQuoteNo; //commenting CODEMMC
                //lblQuoteNumber.Text = strQuoteNo + " " + txtMarketMovement.Text.Trim() + " (" + MaxQuoteVersion.ToString() + ")"; //only append market movement to show in QUOTE PDF CODEMMC
                if (strAddOnDeviation.ToString() == "0" && strBasicODDeviation.ToString() == "0")
                {
                    lblQuoteNumber.Text = strQuoteNo + " " + txtMarketMovement.Text.Trim() + " (" + MaxQuoteVersion.ToString() + ")";
                }
                else
                {
                    lblQuoteNumber.Text = strQuoteNo + " " + txtMarketMovement.Text.Trim() + "," + strBasicODDeviation.ToString() + "," + strAddOnDeviation.ToString() + "  (" + MaxQuoteVersion.ToString() + ")"; //only append market movement to show in QUOTE PDF CODEMMC
                }

                string CampaignCode = "";
                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Extratxt1");
                if (SingleNode != null)
                {
                    CampaignCode = !string.IsNullOrEmpty(SingleNode.InnerXml) ? SingleNode.InnerXml : "-";
                    lblQuoteNumber.Text = !string.IsNullOrEmpty(SingleNode.InnerXml) ? lblQuoteNumber.Text + " (C)" : lblQuoteNumber.Text;
                }

                if (chkIsGetCreditScore.Checked)
                {
                    lblCreditScoreCustomerName.Text = txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim();
                    lblCustomerIDProof.Text = drpDrivingLicenseNumberOrAadhaarNumber.SelectedItem.Text;
                    lblCustomerIDProofNumber.Text = txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim();
                }
                else
                {
                    lblCreditScoreCustomerName.Text = "-";
                    lblCustomerIDProof.Text = drpDrivingLicenseNumberOrAadhaarNumber.SelectedItem.Text;
                    lblCustomerIDProofNumber.Text = "-";
                }

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_IDVFlag");
                lblSystemIDV.Text = Convert.ToDecimal(SingleNode.InnerXml).ToIndianCurrencyFormat();
                strSystemIDV = Convert.ToDecimal(SingleNode.InnerXml).ToString();


                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_IDVofthevehicle");
                lblFinalIDV.Text = Convert.ToDecimal(SingleNode.InnerXml).ToIndianCurrencyFormat();
                strFinalIDV = Convert.ToDecimal(SingleNode.InnerXml).ToString();

                txtIDVofVehicle.Text = strFinalIDV; // strFinalIDV;
                hdnFinalIDV.Value = strSystemIDV; // strFinalIDV; ////Final
                txtIDVofVehicle.Enabled = true;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_AuthorityLocation");
                lblRTO.Text = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_RTOCode");
                lblRTO.Text = "Code: " + SingleNode.InnerXml + " (" + lblRTO.Text + " - " + txtRTOAuthorityCode.Text.Trim() + ")";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropProductName");
                lblCoverType.Text = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropCustomerDtls_CustomerType");
                lblOwnershipType.Text = SingleNode.InnerXml.ToString().ToLower() == "i" ? "Individual" : "Organization";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_CubicCapacity");
                lblCubicCapacity.Text = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_SeatingCapacity");
                lblSeatingCapacity.Text = SingleNode.InnerXml;

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_TypeofPolicyHolder");
                lblPolicyHolderType.Text = SingleNode.InnerXml;

                if (rbbtRollOver.Checked)
                {
                    lblDORorDOP.Text = "Registration Date";
                    SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_DateofRegistration");
                    lblRagistrationDate.Text = SingleNode.InnerXml;
                }
                else
                {
                    lblDORorDOP.Text = "Purchase Date";
                    SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_Dateofpurchase");
                    lblRagistrationDate.Text = SingleNode.InnerXml;
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

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_NonElectricalAccessories");
                lblNonElectricalAccessoriesIDV.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "0";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_ElectricalAccessories");
                lblElectricalAccessoriesIDV.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "0";

                SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropRisks_CNGLPGkitValue");
                lblCNGLPGKitIDV.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "0";

                lblCustomerContactNo.Text = rbctIndividual.Checked ? txtMobileNumber.Text : "";

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

                if (rbbtRollOver.Checked)
                {
                    SingleNode = xmlfile.DocumentElement.SelectSingleNode("/ServiceResult/GetUserData/PropGeneralProposal_PreviousPolicyDetails_Col/GeneralProposal_PreviousPolicyDetails/PropPreviousPolicyDetails_PolicyEffectiveTo");
                    lblPreviousPolicyExpiryDate.Text = SingleNode.InnerXml.Length > 0 ? SingleNode.InnerXml : "-";
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
                                lblExternalBiFuelSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblExternalBiFuelSI = PropCoverDetails_Premium;
                                break;
                            case "Engine Protect":
                                lblEngineProtect.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblEngineProtect = PropCoverDetails_Premium;
                                lblRateEP.Text = PropCoverDetails_Rate;
                                break;
                            case "Return to Invoice":
                                lblReturnToInvoice.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblReturnToInvoice = PropCoverDetails_Premium;
                                lblRateRTI.Text = PropCoverDetails_Rate;
                                break;
                            case "Road Side Assistance":
                                lblRSA.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblRSA = PropCoverDetails_Premium;
                                break;
                            //case "Restricted to Own Premises":
                            //    lblRestrictedToOwnPremises.Text = PropCoverDetails_Premium;
                            //    break;
                            case "CNG Kit TP":
                                lblLiabilityForBiFuel.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblLiabilityForBiFuel = PropCoverDetails_Premium;
                                break;
                            case "Unnamed Passengers Personal Accident":
                                lblPAForUnnamedPassengerSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblPAForUnnamedPassengerSI = PropCoverDetails_Premium;
                                break;
                            case "Named Passengers Personal Accident":
                                lblPAForNamedPassengerSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblPAForNamedPassengerSI = PropCoverDetails_Premium;
                                break;
                            case "Paid Driver PA Cover":
                                lblPAToPaidDriverSI.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat(); ;
                                strlblPAToPaidDriverSI = PropCoverDetails_Premium;
                                break;
                            case "Owner Driver":
                                lblPACoverForOwnerDriver.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                PACoverForOwnerDriver = Convert.ToDecimal(PropCoverDetails_Premium);
                                if (drpTenureOwnerDriver.SelectedValue == "0")
                                {
                                    lblTenureOwnerDriver.Text = "";
                                }
                                else if (drpTenureOwnerDriver.SelectedValue == "1")
                                {
                                    lblTenureOwnerDriver.Text = "1 Year";
                                }
                                else if (drpTenureOwnerDriver.SelectedValue == "3")
                                {
                                    lblTenureOwnerDriver.Text = "3 Years";
                                }
                                strlblPACoverForOwnerDriver = PropCoverDetails_Premium;
                                break;
                            case "Legal Liability for paid driver cleaner conductor":
                                lblLegalLiabilityToPaidDriverNo.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblLegalLiabilityToPaidDriverNo = PropCoverDetails_Premium;
                                break;
                            case "Legal Liability for Employees other than paid driver conductor cleaner":
                                lblLLEOPDCC.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
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
                                lbllblDailyCarAllowanceSI.Text = chkDailyCarAllowance.Checked ? "(SI: Rs." + ddlDailyCarAllowance.Text + ")" : "";
                                break;
                            case "Key Replacement":
                                lblKeyReplacement.Text = Convert.ToDecimal(PropCoverDetails_Premium).ToIndianCurrencyFormat();
                                strlblKeyReplacement = PropCoverDetails_Premium;
                                lblRateKR.Text = PropCoverDetails_Rate;
                                lblKeyReplacementSI.Text = chkKeyReplacement.Checked ? "(SI: Rs." + ddlKeyReplacement.Text + ")" : "";
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
                                lblLossofPersonalBelongingsSI.Text = chkLossofPersonalBelongings.Checked ? "(SI: Rs." + ddlLossofPersonalBelongingsSI.Text + ")" : "";
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
                TotalPremiumOwnDamage = Convert.ToDecimal(dcmlTotalPremiumOwnDamageTable);

                decimal dcmlTotalPremiumLiability = Convert.ToDecimal(strlblBasicTPPremium) + Convert.ToDecimal(strlblLiabilityForBiFuel) +
                Convert.ToDecimal(strlblPAForUnnamedPassengerSI) + Convert.ToDecimal(strlblPAForNamedPassengerSI) + Convert.ToDecimal(strlblPAToPaidDriverSI) +
                Convert.ToDecimal(strlblPACoverForOwnerDriver) + Convert.ToDecimal(strlblLegalLiabilityToPaidDriverNo) + Convert.ToDecimal(strlblLLEOPDCC);

                lblTotalPremiumLiability.Text = dcmlTotalPremiumLiability.ToIndianCurrencyFormat();

                SaveResponseValues(strQuoteNo, strSystemIDV, strFinalIDV, NetPremium, ServiceTax, TotalPremium,
                    lblRTO.Text, lblRTO.Text, lblCoverType.Text, lblOwnershipType.Text,
                lblCubicCapacity.Text, lblSeatingCapacity.Text, lblPolicyHolderType.Text, lblRagistrationDate.Text, lblRagistrationDate.Text, lblMake.Text,
                lblModel.Text, lblVariant.Text, strlblBasicTPPremium, strlblOwnDamagePremium, strlblConsumableCover, strlblDepreciationCover,
                strlblElectronicSI, strlblNonElectronicSI, strlblExternalBiFuelSI, strlblEngineProtect, strlblReturnToInvoice, strlblRSA, strlblLiabilityForBiFuel,
                strlblPAForUnnamedPassengerSI, strlblPAForNamedPassengerSI, strlblPAToPaidDriverSI, strlblPACoverForOwnerDriver, strlblLegalLiabilityToPaidDriverNo,
                strlblLLEOPDCC, lblNCBPercentage.Text, strlblNCB, strlblVoluntaryDeduction, strlblVoluntaryDeductionforDepWaiver,
                dcmlTotalPremiumOwnDamageTable.ToString(), dcmlTotalPremiumLiability.ToString(),
                CGSTAmount, CGSTPercentage, SGSTAmount, SGSTPercentage, IGSTAmount, IGSTPercentage, UGSTAmount, UGSTPercentage, TotalGSTAmount, MaxQuoteVersion,
                 lblRateBasicOD.Text, lblRateCC.Text, lblRateDC.Text, lblRateEP.Text, lblRateRTI.Text
                 , strlblDailyCarAllowance, strlblTyreCover, strlblNCBProtect, strlblLossofPersonalBelongings, strlblKeyReplacement
                 , lblRateLOPB.Text, lblRateDCA.Text, lblRateKR.Text, lblRateNCBP.Text, lblRateTC.Text, CampaignCode);

                DateTime dt_NewBusinessCondition = DateTime.ParseExact("01/09/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (rbbtNewBusiness.Checked && DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= dt_NewBusinessCondition.Date)
                {
                    if (drpProductType.SelectedValue == "1063")
                    {
                        lblODYearText.Text = "(1 Year)";
                        lblTPYearText.Text = "(3 Years)";
                    }
                    else if (drpProductType.SelectedValue == "1062")
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
            }
            catch (Exception ex)
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPath, "Error: " + ex.Message.ToString());

                lblstatus.Text = ex.Message; // ex.StackTrace; //Commented the stack trace due to the VAPT point "Internal Path Disclosure" 
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            finally
            {
            }


            // CR 354 Start Here

            int PremiumWithPAtoOwnerDriver = 0;
            int PremiumWithoutPAtoOwnerDriver = 0;

            #region Old Logic
            /*
            // Condition 2 where Product is 1+1 or 1+3 and with 1 year PA

            if ((drpProductType.SelectedValue == "1063" || drpProductType.SelectedValue == "1011")
                //        &&
                //(Convert.ToDouble(PACoverForOwnerDriver) > 0)
                        &&
                ((drpTenureOwnerDriver.SelectedValue == "1")  || (drpTenureOwnerDriver.SelectedValue == "0"))
                )
            {
                decimal a = TotalPremiumOwnDamage;
                decimal b = PACoverForOwnerDriver;
                decimal GSTAmount = ( ((a + b) * 18) / 100);

                PremiumWithPAtoOwnerDriver =  (int)Math.Round(((TotalPremiumOwnDamage) + (PACoverForOwnerDriver) + GSTAmount));


                a = TotalPremiumOwnDamage;
                GSTAmount = ((a) * 18) / 100;

                PremiumWithoutPAtoOwnerDriver = (int)Math.Round(((TotalPremiumOwnDamage) + GSTAmount));


                lblPremiumWithPAtoOwnerDriver.Text = PremiumWithPAtoOwnerDriver.ToString();
                lblPremiumWithoutPAtoOwnerDriver.Text = PremiumWithoutPAtoOwnerDriver.ToString();

            }
            // Condition 2 End here


            // Condition 3 where Product is 1+1 or 1+3 and with 3 year PA

            if ((drpProductType.SelectedValue == "1063" || drpProductType.SelectedValue == "1011")
                //              &&
                //(Convert.ToInt32(PACoverForOwnerDriver) > 0)
                              &&
                        (drpTenureOwnerDriver.SelectedValue == "3")
             )
            {

                decimal a = TotalPremiumOwnDamage;
                decimal b = PACoverForOwnerDriver / 3;
                decimal GSTAmount = ((a + b) * 18) / 100;
                PremiumWithPAtoOwnerDriver = (int)Math.Round((TotalPremiumOwnDamage + (PACoverForOwnerDriver / 3) + GSTAmount));

                a = TotalPremiumOwnDamage;
                GSTAmount = ((a) * 18) / 100;

                PremiumWithoutPAtoOwnerDriver = (int)Math.Round((TotalPremiumOwnDamage + GSTAmount));


                //lblPremiumWithPAtoOwnerDriver.Text = PremiumWithPAtoOwnerDriver.ToString();
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
            if (drpProductType.SelectedValue == "1062")
            {
                lilblPremiumWithoutPAtoOwnerDriver.Visible = false;
                lilblPremiumWithPAtoOwnerDriver.Visible = false;
            }
            // Condition 4 End here */

            #endregion

            #region New Logic
            decimal GSTAmount = ((TotalPremiumOwnDamage) * 18) / 100;
            PremiumWithoutPAtoOwnerDriver = (int)Math.Round(((TotalPremiumOwnDamage) + GSTAmount));
            lblPremiumWithoutPAtoOwnerDriver.Text = PremiumWithoutPAtoOwnerDriver.ToString();

            if (drpProductType.SelectedValue == "1062")
            {
                lilblPremiumWithoutPAtoOwnerDriver.Visible = false;
            }

            #endregion

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

        #region Credit SCore
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private bool IsCreditScoreExistsForCustomer(ref string CreditScore)
        {
            DateTime dtDOB = DateTime.ParseExact(txtCustomerDOB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            CreditScore = string.Empty;
            bool IsCSExists = false;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_CREDIT_SCORE_FOR_CUSTOMER_IF_EXISTS";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "CustomerFirstName", DbType.String, ParameterDirection.Input, "CustomerFirstName", DataRowVersion.Current, txtFirstName.Text.Trim());
                db.AddParameter(dbCommand, "CustomerLastName", DbType.String, ParameterDirection.Input, "CustomerLastName", DataRowVersion.Current, txtLastName.Text.Trim());
                db.AddParameter(dbCommand, "CustomerDateofBirth", DbType.DateTime, ParameterDirection.Input, "CustomerDateofBirth", DataRowVersion.Current, dtDOB.Date);

                dbCommand.CommandType = CommandType.StoredProcedure;
                DataSet ds = null;
                ds = db.ExecuteDataSet(dbCommand);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            IsCSExists = true;
                            CreditScore = ds.Tables[0].Rows[0]["Response_CreditScore"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "IsCreditScoreExistsForCustomer");
            }
            return IsCSExists;
        }
        private void GetCreditScore(ref string RequestXML_CreditScore, ref string ResponseXML_CreditScore, ref string Response_CreditScore, ref string Response_AddressLine1, ref string Response_AddressLine2, ref string Response_AddressLine3, ref string Response_Exact_match, ref string ErrorMessageIfAny, ref string CreditScoreUserMessageText, ref string Response_IncomeTaxPanNumber, ref string Response_PassportNumber, ref string Response_PhoneNumber, ref string Response_EmailAddress)
        {

            // CR 133
            try
            {
                WebRequest.DefaultWebProxy = null;

                //string strXmlPath22 = AppDomain.CurrentDomain.BaseDirectory + "TEST.txt";
                //File.WriteAllText(strXmlPath22, "1");

                string IsUseNetworkProxyForExperianCreditScore = ConfigurationManager.AppSettings["IsUseNetworkProxyForExperianCreditScore"].ToString();

                string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();
                string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();

                string proxy_Address = ConfigurationManager.AppSettings["proxy_Address"].ToString();
                string proxy_Port = ConfigurationManager.AppSettings["proxy_Port"].ToString();

                //File.WriteAllText(strXmlPath22, "2");

                RequestXML_CreditScore = GetRequestXML_CreditScore();
                string strUri = ConfigurationManager.AppSettings["CREDIT_SCORE_URL"].ToString(); // "https://194.60.184.149/nextgen-ind-pds-webservices-cbv2/endpoint"; //CONFIGURE THIS
                Uri url = new Uri(strUri);

                //File.WriteAllText(strXmlPath22, "3");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "text/xml; charset=utf-8";
                request.Timeout = Timeout.Infinite;
                request.KeepAlive = true;
                request.ReadWriteTimeout = Timeout.Infinite;
                request.Proxy = null;

                //File.WriteAllText(strXmlPath22, "4");
                if (IsUseNetworkProxyForExperianCreditScore == "1")
                {
                    string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();
                    WebProxy proxy = new WebProxy(proxy_Address_Full);
                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;
                    request.Proxy = proxy;
                    //File.WriteAllText(strXmlPath22, "5");
                    /////
                    request.Proxy = WebRequest.DefaultWebProxy;
                    request.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    request.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                }

                //File.WriteAllText(strXmlPath22, "6");

                Stream objstrm = GenerateStreamFromString(RequestXML_CreditScore);
                StreamReader reader = new StreamReader(objstrm);
                String strLine = null;
                String xml = "";

                //File.WriteAllText(strXmlPath22, "7");
                while ((strLine = reader.ReadLine()) != null)
                {
                    xml += strLine;
                }

                reader.Close();

                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(xml);
                writer.Flush();
                writer.Close();
                //File.WriteAllText(strXmlPath22, "8");
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => { return true; };

                WebResponse responce = request.GetResponse();
                Stream readers = responce.GetResponseStream();

                StreamReader sReader = new StreamReader(readers);
                string outResult = sReader.ReadToEnd();
                sReader.Close();
                ResponseXML_CreditScore = outResult; // outResult.Replace("&lt;", "<");
                GetResultXML_CreditScore(outResult, ref Response_CreditScore, ref Response_AddressLine1, ref Response_AddressLine2, ref Response_AddressLine3, ref Response_Exact_match, ref CreditScoreUserMessageText, ref Response_IncomeTaxPanNumber, ref Response_PassportNumber, ref Response_PhoneNumber, ref Response_EmailAddress);

                string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "ExperianResult.xml";
                File.WriteAllText(strXmlPath, String.Empty);
                File.WriteAllText(strXmlPath, outResult);

            }
            catch (System.Net.WebException ex)
            {
                try
                {
                    string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    ErrorMessageIfAny = message;
                }
                catch
                {
                    ErrorMessageIfAny = ex.Message;
                }
                ExceptionUtility.LogException(ex, "GetCreditScore");
            }
            catch (Exception ex)
            {
                ErrorMessageIfAny = ex.Message;
                ExceptionUtility.LogException(ex, "GetCreditScore");
            }

        }

        private string GetRequestXML_CreditScore()
        {
            string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "/CreditScoreRequestXML_Inner.XML";

            XmlNode node = null;
            XmlDocument xmlfile = null;
            string xmlString = string.Empty;

            try
            {
                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);
                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Identification/XMLUser");
                node.InnerText = ConfigurationManager.AppSettings["CreditScore_UserName"].ToString(); // "kotakcpu_uat";

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Identification/XMLPassword");
                node.InnerText = ConfigurationManager.AppSettings["CreditScore_Password"].ToString(); // "Kotak@123";

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/Surname");
                node.InnerText = txtLastName.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/FirstName");
                node.InnerText = txtFirstName.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/MiddleName1");
                node.InnerText = txtMiddleName.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/GenderCode");
                node.InnerText = rbtnMale.Checked ? "1" : "2";

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/IncomeTaxPAN");
                node.InnerText = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "PanNumber" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/PassportNumber");
                node.InnerText = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "Passport" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/VoterIdentityCard");
                node.InnerText = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "VoterIDNumber" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/Universal_ID_Number");
                node.InnerText = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "AADHAARNumber" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/Driver_License_Number");
                node.InnerText = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "DrivingLicense" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";

                DateTime dtDOB = DateTime.ParseExact(txtCustomerDOB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/DateOfBirth");
                node.InnerText = dtDOB.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/PhoneNumber");
                node.InnerText = txtMobileNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Applicant/MobilePhone");
                node.InnerText = txtMobileNumber.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Address/City");
                node.InnerText = lblCityName.Text.Trim();

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Address/State");
                node.InnerText = hdnCreditScoreStateId.Value;

                node = xmlfile.DocumentElement.SelectSingleNode("/INProfileRequest/Address/PinCode");
                node.InnerText = hdnPinCode.Value;

                xmlString = xmlfile.InnerXml;

                //outerXML = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:cbv2'><soapenv:Header/><soapenv:Body><urn:process><urn:in>@INNERXML</urn:in></urn:process></soapenv:Body></soapenv:Envelope>";
                //xmlString = outerXML.Replace("@INNERXML", xmlString);

                string outerXML = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:nex='http://nextgenws.ngwsconnect.experian.com'><soapenv:Header/><soapenv:Body><nex:process><nex:cbv2String><![CDATA[@INNERXML]]></nex:cbv2String></nex:process></soapenv:Body></soapenv:Envelope>";
                xmlString = outerXML.Replace("@INNERXML", xmlString);

                string _strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "ExperianRequest.xml";
                File.WriteAllText(_strXmlPath, String.Empty);
                File.WriteAllText(_strXmlPath, xmlString);

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetRequestXML_CreditScore");
            }
            return xmlString;
        }

        public void GetResultXML_CreditScore(string ResponseXML, ref string Response_CreditScore, ref string Response_AddressLine1, ref string Response_AddressLine2, ref string Response_AddressLine3, ref string Response_Exact_match, ref string CreditScoreUserMessageText, ref string Response_IncomeTaxPanNumber, ref string Response_PassportNumber, ref string Response_PhoneNumber, ref string Response_EmailAddress)
        {
            Response_CreditScore = string.Empty;
            Response_AddressLine1 = string.Empty;
            Response_AddressLine2 = string.Empty;
            Response_AddressLine3 = string.Empty;
            Response_Exact_match = string.Empty;
            CreditScoreUserMessageText = string.Empty;
            Response_IncomeTaxPanNumber = string.Empty;
            Response_PassportNumber = string.Empty;

            Stream objResultXMLStream = GenerateStreamFromString(ResponseXML);

            XmlNode node = null;
            XmlDocument xmlfile = null;
            XmlDocument _xmlfile = null;
            string xmlString = string.Empty;

            try
            {
                xmlfile = new XmlDocument();
                xmlfile.Load(objResultXMLStream);

                string s = xmlfile.DocumentElement.InnerText;

                using (Stream objstrm = GenerateStreamFromString(s))
                {
                    _xmlfile = new XmlDocument();
                    _xmlfile.Load(objstrm);
                }

                string DefaultCreditScoreIfScoreNullOrEmpty = ConfigurationManager.AppSettings["DefaultCreditScoreIfScoreNullOrEmpty"].ToString();

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/UserMessage/UserMessageText");
                CreditScoreUserMessageText = (node == null) ? "" : node.InnerText;

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/SCORE/BureauScore");
                if (node == null)
                {
                    Response_CreditScore = DefaultCreditScoreIfScoreNullOrEmpty;
                }
                else if (node.InnerText == "0")
                {
                    Response_CreditScore = DefaultCreditScoreIfScoreNullOrEmpty;
                }
                else
                {
                    Response_CreditScore = node.InnerText;
                }

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/CAIS_Account/CAIS_Account_DETAILS/CAIS_Holder_Address_Details/First_Line_Of_Address_non_normalized");
                Response_AddressLine1 = (node == null) ? "" : node.InnerText;

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/CAIS_Account/CAIS_Account_DETAILS/CAIS_Holder_Address_Details/Second_Line_Of_Address_non_normalized");
                Response_AddressLine2 = (node == null) ? "" : node.InnerText;

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/CAIS_Account/CAIS_Account_DETAILS/CAIS_Holder_Address_Details/Third_Line_Of_Address_non_normalized");
                Response_AddressLine3 = (node == null) ? "" : node.InnerText;

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/Match_result/Exact_match");
                Response_Exact_match = (node == null) ? "" : node.InnerText;

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/CAIS_Account/CAIS_Account_DETAILS/CAIS_Holder_Details/Income_TAX_PAN");
                Response_IncomeTaxPanNumber = (node == null) ? "" : node.InnerText;

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/CAIS_Account/CAIS_Account_DETAILS/CAIS_Holder_Details/Passport_Number");
                Response_PassportNumber = (node == null) ? "" : node.InnerText;

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/CAIS_Account/CAIS_Account_DETAILS/CAIS_Holder_Phone_Details/Mobile_Telephone_Number");
                Response_PhoneNumber = (node == null) ? "" : node.InnerText;

                node = _xmlfile.DocumentElement.SelectSingleNode("/INProfileResponse/CAIS_Account/CAIS_Account_DETAILS/CAIS_Holder_Phone_Details/EMailId");
                Response_EmailAddress = (node == null) ? "" : node.InnerText;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetResultXML_CreditScore");
            }
        }

        private void GetCreditScoreThroughGIST(ref string RequestXML_CreditScore, ref string ResponseXML_CreditScore, ref string Response_CreditScore, ref string Response_AddressLine1, ref string Response_AddressLine2, ref string Response_AddressLine3, ref string Response_Exact_match, ref string ErrorMessageIfAny, ref string CreditScoreUserMessageText, ref string Response_IncomeTaxPanNumber, ref string Response_PassportNumber, ref string Response_PhoneNumber, ref string Response_EmailAddress)
        {
            string CreditScore = "0";
            string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString(); //  GC0004
            string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString(); //  cmc123
            ErrorMessageIfAny = string.Empty;
            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager

                ////START -SR79011 - Implementation of new credit score service in PASS application - HASMUKH
                svcGCInternetProxy.CustomerPortalServiceClient proxy = new svcGCInternetProxy.CustomerPortalServiceClient();
                svcGCInternetProxy.ServiceResult objServiceResult = new svcGCInternetProxy.ServiceResult();
                objServiceResult.UserData = new svcGCInternetProxy.clsUserData();
                ////END -SR79011 - Implementation of new credit score service in PASS application - HASMUKH

                objServiceResult.UserData.CustomerFirstName = txtFirstName.Text.Trim();
                objServiceResult.UserData.CustomerMiddleName = txtMiddleName.Text.Trim();
                objServiceResult.UserData.CustomerLastName = txtLastName.Text.Trim();
                objServiceResult.UserData.CustomerDOB = txtCustomerDOB.Text.Trim();
                objServiceResult.UserData.MobileNumber = string.IsNullOrEmpty(txtMobileNumber.Text.Trim()) ? "9999999999" : txtMobileNumber.Text.Trim();

                objServiceResult.UserData.PanNumber = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "PanNumber" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";
                objServiceResult.UserData.PassportNumber = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "Passport" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";
                objServiceResult.UserData.VoterIDNumber = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "VoterIDNumber" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";
                objServiceResult.UserData.AADHAARNumber = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "AADHAARNumber" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";
                objServiceResult.UserData.DrivingLicense = drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "DrivingLicense" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "";

                objServiceResult.UserData.CustomerGender = rbtnMale.Checked ? "1" : "2";
                objServiceResult.UserData.StateCode = hdnStateId.Value;
                objServiceResult.UserData.CityDistrictName = lblCityName.Text;
                objServiceResult.UserData.FinancierPinCode = hdnPinCode.Value;
                objServiceResult.UserData.CustomerId = "";
                objServiceResult.UserData.CreatedBy = "PASS";

                ExceptionUtility.LogEvent("Requested GIST Credit Score for PAN Number:" + txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() + " at" + DateTime.Now.ToString()); //SR79011 - Implementation of new credit score service in PASS application - HASMUKH
                WebRequest.DefaultWebProxy = null;
                proxy.GetCreditScore(strUserId, strPassword, ref objServiceResult);
                proxy.Close();
                WebRequest.DefaultWebProxy = null;
                ExceptionUtility.LogEvent("Response Received for Credit Score for PAN Number:" + txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() + " at" + DateTime.Now.ToString()); //SR79011 - Implementation of new credit score service in PASS application - HASMUKH

                if (string.IsNullOrEmpty(objServiceResult.UserData.ErrorText))
                {
                    CreditScore = string.IsNullOrEmpty(objServiceResult.UserData.CreditScore) ? "0" : objServiceResult.UserData.CreditScore;

                    Response_CreditScore = CreditScore;
                    if (!string.IsNullOrEmpty(objServiceResult.UserData.Response_PhoneNumber))
                    {
                        string resphone = objServiceResult.UserData.Response_PhoneNumber;
                        Response_PhoneNumber = resphone.Length == 12 ? resphone.Substring(2, resphone.Length - 2) : resphone;
                    }
                    else
                    {
                        Response_PhoneNumber = "";
                    }

                    Response_EmailAddress = string.IsNullOrEmpty(objServiceResult.UserData.Response_EmailAddress) ? "" : objServiceResult.UserData.Response_EmailAddress;
                    Response_AddressLine1 = string.IsNullOrEmpty(objServiceResult.UserData.Response_AddressLine1) ? "" : objServiceResult.UserData.Response_AddressLine1;
                    Response_AddressLine2 = string.IsNullOrEmpty(objServiceResult.UserData.Response_AddressLine2) ? "" : objServiceResult.UserData.Response_AddressLine2;
                    Response_AddressLine3 = string.IsNullOrEmpty(objServiceResult.UserData.Response_AddressLine3) ? "" : objServiceResult.UserData.Response_AddressLine3;

                    Response_Exact_match = "";
                    ErrorMessageIfAny = "";
                    CreditScoreUserMessageText = "";
                    Response_IncomeTaxPanNumber = string.IsNullOrEmpty(objServiceResult.UserData.Response_IncomeTaxPanNumber) ? "" : objServiceResult.UserData.Response_IncomeTaxPanNumber;
                    Response_PassportNumber = "";
                }
                else
                {
                    ErrorMessageIfAny = objServiceResult.UserData.ErrorText;
                    CreditScore = "0";
                    Response_CreditScore = CreditScore;
                }
            }
            catch (Exception ex)
            {
                ErrorMessageIfAny = ex.Message;
                ExceptionUtility.LogException(ex, "GetCreditScore");
            }
        }
        #endregion



        private void CalculatePremium()
        {
            bool IsSuccess = false;
            string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
            string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();
            var regex = @"^([A-Za-z]){3}([Pp]){1}([A-Za-z]){1}([0-9]){4}([A-Za-z]){1}$";
            //string strUserId = "GC0014"; string strPassword = "cmc123"; //for uat
            //string strUserId = "GI0033"; string strPassword = "May@2016"; //for prod
            try
            {
                string RequestXML_CreditScore = string.Empty;
                string ResponseXML_CreditScore = string.Empty;
                string CreditScore = string.Empty;
                string Response_AddressLine1 = string.Empty;
                string Response_AddressLine2 = string.Empty;
                string Response_AddressLine3 = string.Empty;
                string Response_Exact_match = string.Empty;
                string ErrorMessageIfAny = string.Empty;
                string CreditScoreUserMessageText = string.Empty;
                string Response_IncomeTaxPanNumber = string.Empty;
                string Response_PassportNumber = string.Empty;
                string Response_PhoneNumber = string.Empty;
                string Response_EmailAddress = string.Empty;


                //string strXmlPath22 = AppDomain.CurrentDomain.BaseDirectory + "/TEST.txt";
                //File.WriteAllText(strXmlPath22, "1");
                bool IsCreditScoreExists = false;
                if (rbctIndividual.Checked && chkIsGetCreditScore.Checked)
                {
                    //   if (rbctIndividual.Checked && txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim().Length > 0)
                    //{

                    //CR_P1_676_Start_Pan_NUmber Validation 
                    if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "PanNumber")
                    {
                       if (Regex.IsMatch(txtDrivingLicenseNumberOrAadhaarNumber.Text, regex))
                       {
                          if (CommonExtensions.fn_Check_PanNumber(txtDrivingLicenseNumberOrAadhaarNumber.Text))
                           {
                              IsCreditScoreExists = IsCreditScoreExistsForCustomer(ref CreditScore);
                              if (!IsCreditScoreExists)
                              {
                                    string IsCallGISTForCreditScore = ConfigurationManager.AppSettings["IsCallGISTForCreditScore"].ToString();
                                    if (IsCallGISTForCreditScore == "0")
                                    {
                                        GetCreditScore(ref RequestXML_CreditScore, ref ResponseXML_CreditScore, ref CreditScore, ref Response_AddressLine1, ref Response_AddressLine2, ref Response_AddressLine3, ref Response_Exact_match, ref ErrorMessageIfAny, ref CreditScoreUserMessageText, ref Response_IncomeTaxPanNumber, ref Response_PassportNumber, ref Response_PhoneNumber, ref Response_EmailAddress);
                                    }
                                    else
                                    {
                                        GetCreditScoreThroughGIST(ref RequestXML_CreditScore, ref ResponseXML_CreditScore, ref CreditScore, ref Response_AddressLine1, ref Response_AddressLine2, ref Response_AddressLine3, ref Response_Exact_match, ref ErrorMessageIfAny, ref CreditScoreUserMessageText, ref Response_IncomeTaxPanNumber, ref Response_PassportNumber, ref Response_PhoneNumber, ref Response_EmailAddress);
                                    }
                              }
                          }
                            //IsError = true;
                            //strErrorMsg = "Please Enter Valid PanCard Number";
                              

                           }
                       }
                   // }
                  //File.WriteAllText(strXmlPath22, "1");
                   else if(drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue != "PanNumber")//other than PAN number selected**
                    {
                        IsCreditScoreExists = IsCreditScoreExistsForCustomer(ref CreditScore);
                        if (!IsCreditScoreExists)
                        {
                            string IsCallGISTForCreditScore = ConfigurationManager.AppSettings["IsCallGISTForCreditScore"].ToString();
                            if (IsCallGISTForCreditScore == "0")
                            {
                                GetCreditScore(ref RequestXML_CreditScore, ref ResponseXML_CreditScore, ref CreditScore, ref Response_AddressLine1, ref Response_AddressLine2, ref Response_AddressLine3, ref Response_Exact_match, ref ErrorMessageIfAny, ref CreditScoreUserMessageText, ref Response_IncomeTaxPanNumber, ref Response_PassportNumber, ref Response_PhoneNumber, ref Response_EmailAddress);
                            }
                            else
                            {
                                GetCreditScoreThroughGIST(ref RequestXML_CreditScore, ref ResponseXML_CreditScore, ref CreditScore, ref Response_AddressLine1, ref Response_AddressLine2, ref Response_AddressLine3, ref Response_Exact_match, ref ErrorMessageIfAny, ref CreditScoreUserMessageText, ref Response_IncomeTaxPanNumber, ref Response_PassportNumber, ref Response_PhoneNumber, ref Response_EmailAddress);
                            }
                        }
                    }
                    //CR_P1_676_End_Pan_NUmber Validation 

                    //IsCreditScoreExists = IsCreditScoreExistsForCustomer(ref CreditScore);
                    //if (!IsCreditScoreExists)
                    //{
                    //    string IsCallGISTForCreditScore = ConfigurationManager.AppSettings["IsCallGISTForCreditScore"].ToString();
                    //    if (IsCallGISTForCreditScore == "0")
                    //    {
                    //        GetCreditScore(ref RequestXML_CreditScore, ref ResponseXML_CreditScore, ref CreditScore, ref Response_AddressLine1, ref Response_AddressLine2, ref Response_AddressLine3, ref Response_Exact_match, ref ErrorMessageIfAny, ref CreditScoreUserMessageText, ref Response_IncomeTaxPanNumber, ref Response_PassportNumber, ref Response_PhoneNumber, ref Response_EmailAddress);
                    //    }
                    //    else
                    //    {
                    //        GetCreditScoreThroughGIST(ref RequestXML_CreditScore, ref ResponseXML_CreditScore, ref CreditScore, ref Response_AddressLine1, ref Response_AddressLine2, ref Response_AddressLine3, ref Response_Exact_match, ref ErrorMessageIfAny, ref CreditScoreUserMessageText, ref Response_IncomeTaxPanNumber, ref Response_PassportNumber, ref Response_PhoneNumber, ref Response_EmailAddress);
                    //    }
                    //}
                }
                string strCreditScore = "0";
                if (IsCreditScoreExists)
                {
                    strCreditScore = string.IsNullOrEmpty(CreditScore) ? "0" : CreditScore;
                }
                else
                {
                    if (rbctIndividual.Checked && chkIsGetCreditScore.Checked)
                    {

                        //CR_P1_676_Start_Pan_NUmber Validation
                        if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "PanNumber")
                        {
                            //var regex = @"^([A-Za-z]){3}([Pp]){1}([A-Za-z]){1}([0-9]){4}([A-Za-z]){1}$";

                            if (Regex.IsMatch(txtDrivingLicenseNumberOrAadhaarNumber.Text, regex))
                            {
                                if (CommonExtensions.fn_Check_PanNumber(txtDrivingLicenseNumberOrAadhaarNumber.Text))
                                {
                                    //IsError = true;
                                    //strErrorMsg = "Please Enter Valid PanCard Number";
                                    string DefaultCreditScoreIfScoreNullOrEmpty = ConfigurationManager.AppSettings["DefaultCreditScoreIfScoreNullOrEmpty"].ToString();
                                    if (string.IsNullOrEmpty(CreditScore))
                                    {
                                        strCreditScore = DefaultCreditScoreIfScoreNullOrEmpty;
                                    }
                                    else if (CreditScore == "0")
                                    {
                                        strCreditScore = DefaultCreditScoreIfScoreNullOrEmpty;
                                    }
                                    else
                                    {
                                        strCreditScore = CreditScore;
                                    }

                                }

                            }
                        }

                        else if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue != "PanNumber")
                        {
                            string DefaultCreditScoreIfScoreNullOrEmpty = ConfigurationManager.AppSettings["DefaultCreditScoreIfScoreNullOrEmpty"].ToString();
                            if (string.IsNullOrEmpty(CreditScore))
                            {
                                strCreditScore = DefaultCreditScoreIfScoreNullOrEmpty;
                            }
                            else if (CreditScore == "0")
                            {
                                strCreditScore = DefaultCreditScoreIfScoreNullOrEmpty;
                            }
                            else
                            {
                                strCreditScore = CreditScore;
                            }
                        }
                        //CR_P1_676_End_Pan_NUmber Validation
                        

                        //string DefaultCreditScoreIfScoreNullOrEmpty = ConfigurationManager.AppSettings["DefaultCreditScoreIfScoreNullOrEmpty"].ToString();
                        //if (string.IsNullOrEmpty(CreditScore))
                        //{
                        //    strCreditScore = DefaultCreditScoreIfScoreNullOrEmpty;
                        //}
                        //else if (CreditScore == "0")
                        //{
                        //    strCreditScore = DefaultCreditScoreIfScoreNullOrEmpty;
                        //}
                        //else
                        //{
                        //    strCreditScore = CreditScore;
                        //}
                    }
                }

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
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestXML(IsPremiumCalc: true, CustomerId: 0, CustomerName: string.Empty, CreditScore: strCreditScore));
                objServiceResult.UserData.IsInternalRiskGrid = true;

                objServiceResult.UserData.ErrorText = "";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager

                WebRequest.DefaultWebProxy = null; //THIS LINE IS ADDED BCOZ AFTER HITTING CREDIT SCORE SERVICE : GOT PROXY ANONYMOUS ERROR ON SERVER 12.44 //Final
                proxy.CalculatePremium(strUserId, strPassword, ref objServiceResult);
                proxy.Close();

                string ErrorText = string.Empty;
                IsSuccess = string.IsNullOrEmpty(objServiceResult.UserData.ErrorText) ? true : false;
                if (IsSuccess)
                {
                    IsSuccess = string.IsNullOrEmpty(objServiceResult.UserData.UserResultXml) ? false : true;
                    ErrorText = (IsSuccess == false) ? "objServiceResult.UserData.UserResultXml is NULL" : "";
                }
                else
                {
                    ErrorText = objServiceResult.UserData.ErrorText;
                }

                string strQuoteNo = string.Empty;
                int MaxQuoteVersion = 0;
                SaveRequestResponse(strRequestXml: objServiceResult.UserData.ConsumeProposalXML.ToString(), strResponseXml: objServiceResult.UserData.UserResultXml, IsSuccess: IsSuccess, strErrorMessage: objServiceResult.UserData.ErrorText, strQuoteNo: ref strQuoteNo, MaxQuoteVersion: ref MaxQuoteVersion);
                SaveRequestValues(strQuoteNo, IsSuccess, ErrorText, strCreditScore, "ChotuPass", MaxQuoteVersion);

                //SAVE CREDIT SCORE DETAILS
                if (rbctIndividual.Checked && chkIsGetCreditScore.Checked)
                {
                    //if (!IsCreditScoreExists)
                    //{
                    //    SaveRequestResponse_CreditScore(strQuoteNo, RequestXML_CreditScore, ResponseXML_CreditScore,
                    //    strCreditScore, Response_AddressLine1, Response_AddressLine2, Response_AddressLine3, Response_Exact_match,
                    //    ErrorMessageIfAny, CreditScoreUserMessageText, Response_IncomeTaxPanNumber, Response_PassportNumber, Response_PhoneNumber, Response_EmailAddress, MaxQuoteVersion);
                    //}
                    //else
                    //{
                    //    SaveRequestResponse_CreditScore_From_Existing(NewQuoteNumber: strQuoteNo, MaxQuoteVersion: MaxQuoteVersion);
                    //}

                    //CR_P1_676_Start_Pan_NUmber Validation
                    if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "PanNumber")
                    {
                        if (Regex.IsMatch(txtDrivingLicenseNumberOrAadhaarNumber.Text, regex))
                        {
                            if (CommonExtensions.fn_Check_PanNumber(txtDrivingLicenseNumberOrAadhaarNumber.Text))
                            {
                                if (!IsCreditScoreExists)
                                {
                                    SaveRequestResponse_CreditScore(strQuoteNo, RequestXML_CreditScore, ResponseXML_CreditScore,
                                    strCreditScore, Response_AddressLine1, Response_AddressLine2, Response_AddressLine3, Response_Exact_match,
                                    ErrorMessageIfAny, CreditScoreUserMessageText, Response_IncomeTaxPanNumber, Response_PassportNumber, Response_PhoneNumber, Response_EmailAddress, MaxQuoteVersion);
                                }
                                else
                                {
                                    SaveRequestResponse_CreditScore_From_Existing(NewQuoteNumber: strQuoteNo, MaxQuoteVersion: MaxQuoteVersion);
                                }


                            }

                        }

                    }
                    else if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue != "PanNumber")
                    {
                        if (!IsCreditScoreExists)
                        {
                            SaveRequestResponse_CreditScore(strQuoteNo, RequestXML_CreditScore, ResponseXML_CreditScore,
                            strCreditScore, Response_AddressLine1, Response_AddressLine2, Response_AddressLine3, Response_Exact_match,
                            ErrorMessageIfAny, CreditScoreUserMessageText, Response_IncomeTaxPanNumber, Response_PassportNumber, Response_PhoneNumber, Response_EmailAddress, MaxQuoteVersion);
                        }
                        else
                        {
                            SaveRequestResponse_CreditScore_From_Existing(NewQuoteNumber: strQuoteNo, MaxQuoteVersion: MaxQuoteVersion);
                        }
                    }
                    //CR_P1_676_End_Pan_NUmber Validation
                }

                if (!string.IsNullOrEmpty(objServiceResult.UserData.ErrorText))
                {
                    lblstatus.Text = objServiceResult.UserData.ErrorText;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                else
                {
                    if (string.IsNullOrEmpty(objServiceResult.UserData.UserResultXml))
                    {
                        lblstatus.Text = "objServiceResult.UserData.UserResultXml is NULL";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else if (objServiceResult.UserData.CurrentIDV > 10000000)
                    {
                        lblstatus.Text = "For IDV Greater Than 1 Crore, Please Contact Service Branch";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
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

                        try
                        {
                            string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "ResultXML.xml";
                            File.WriteAllText(strXmlPath, String.Empty);
                            File.WriteAllText(strXmlPath, ResultXML);
                        }
                        catch
                        {
                            //do not write exception log as only possible error is file access denied
                        }
                        

                        GetResultXML(ResultXML, strQuoteNo, objServiceResult.UserData.NetPremium.ToString(), objServiceResult.UserData.ServiceTax.ToString(), objServiceResult.UserData.TotalPremium.ToString(), objServiceResult.UserData.CGSTAmount.ToString(), objServiceResult.UserData.CGSTPercentage.ToString(), objServiceResult.UserData.SGSTAmount.ToString(), objServiceResult.UserData.SGSTPercentage.ToString(), objServiceResult.UserData.IGSTAmount.ToString(), objServiceResult.UserData.IGSTPercentage.ToString(), objServiceResult.UserData.UGSTAmount.ToString(), objServiceResult.UserData.UGSTPercentage.ToString(), objServiceResult.UserData.TotalGSTAmount.ToString(), MaxQuoteVersion);

                        DownloadPDF(IsOnlySavePDF: true, MaxQuoteVersion: MaxQuoteVersion.ToString());

                        CreateQuotePDF objCreateQuotePDF = new CreateQuotePDF();
                        QuotePDFParams objQuotePDFParams = new QuotePDFParams();
                        objQuotePDFParams.ProductCode = 3121;
                        objQuotePDFParams.QuoteNumber = strQuoteNo;
                        objQuotePDFParams.chkIsGetCreditScore = chkIsGetCreditScore.Checked;
                        objQuotePDFParams.txtFirstName = txtFirstName.Text.Trim();
                        objQuotePDFParams.txtLastName = txtLastName.Text.Trim();
                        objQuotePDFParams.drpDrivingLicenseNumberOrAadhaarNumber = drpDrivingLicenseNumberOrAadhaarNumber.SelectedItem.Text;
                        objQuotePDFParams.txtDrivingLicenseNumberOrAadhaarNumber = txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim();
                        objQuotePDFParams.txtRTOAuthorityCode = txtRTOAuthorityCode.Text.Trim();
                        objQuotePDFParams.rbbtRollOver = rbbtRollOver.Checked;
                        objQuotePDFParams.rbctIndividual = rbctIndividual.Checked;
                        objQuotePDFParams.txtMobileNumber = txtMobileNumber.Text.Trim();
                        objQuotePDFParams.drpTenureOwnerDriver = drpTenureOwnerDriver.SelectedValue;
                        objQuotePDFParams.chkDepreciationCover = chkDepreciationCover.Checked;
                        objQuotePDFParams.chkDailyCarAllowance = chkDailyCarAllowance.Checked;
                        objQuotePDFParams.ddlDailyCarAllowance = ddlDailyCarAllowance.SelectedValue;
                        objQuotePDFParams.chkKeyReplacement = chkKeyReplacement.Checked;
                        objQuotePDFParams.ddlKeyReplacement = ddlKeyReplacement.SelectedValue;
                        objQuotePDFParams.chkLossofPersonalBelongings = chkLossofPersonalBelongings.Checked;
                        objQuotePDFParams.ddlLossofPersonalBelongingsSI = ddlLossofPersonalBelongingsSI.SelectedValue;
                        objQuotePDFParams.rbbtNewBusiness = rbbtNewBusiness.Checked;
                        objQuotePDFParams.txtDateOfRegistration = txtDateOfRegistration.Text.Trim();
                        objQuotePDFParams.drpProductType = drpProductType.SelectedValue;

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
                        objQuotePDFParams.drpVDA = drpVDA.SelectedValue; //Voluntary Deductible Amount
                        objQuotePDFParams.VDEPCoverAmount = ConfigurationManager.AppSettings["VDEPCoverAmount"].ToString();
                        objCreateQuotePDF.SaveQuotePDF(strQuoteNo, objServiceResult.UserData.ConsumeProposalXML.ToString(), ResultXML, objQuotePDFParams);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalViewPremium();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "CalculatePremium Method");
                lblstatus.Text = ex.Message; // + "------------------" + ex.StackTrace; //Commented the stack trace due to the VAPT point "Internal Path Disclosure" 
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }

        }

        private void SaveRequestResponse_CreditScore(string QuoteNumber, string RequestXML_CreditScore, string ResponseXML_CreditScore, string Response_CreditScore, string Response_AddressLine1, string Response_AddressLine2, string Response_AddressLine3, string Response_Exact_match, string ErrorMessageIfAny, string CreditScoreUserMessageText, string Response_IncomeTaxPanNumber, string Response_PassportNumber, string Response_PhoneNumber, string Response_EmailAddress, int MaxQuoteVersion)
        {
            DateTime dtDOB = DateTime.ParseExact(txtCustomerDOB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strDateOfBirth = dtDOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_CREDIT_SCORE_REQUEST_RESPONSE";

                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.Parameters.AddWithValue("@QuoteNumber", QuoteNumber);
                        cmd.Parameters.AddWithValue("@Response_CreditScore", Response_CreditScore);
                        cmd.Parameters.AddWithValue("@CustomerType", "Individual");
                        cmd.Parameters.AddWithValue("@CustomerFirstName", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerMiddleName", txtMiddleName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerLastName", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerGender", rbtnMale.Checked ? "Male" : "Female");
                        cmd.Parameters.AddWithValue("@IDProofGCID", "0"); //PENDING
                        cmd.Parameters.AddWithValue("@IDProof", drpDrivingLicenseNumberOrAadhaarNumber.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@IDProofNumber", txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@IncomeTaxPAN", drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "PanNumber" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "");
                        cmd.Parameters.AddWithValue("@PassportNumber", drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "Passport" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "");
                        cmd.Parameters.AddWithValue("@VoterIdentityCard", drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "VoterIDNumber" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "");
                        cmd.Parameters.AddWithValue("@DrivingLicenseNumber", drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "DrivingLicense" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "");
                        cmd.Parameters.AddWithValue("@AADHAARNumber_Universal_ID_Number", drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "AADHAARNumber" ? txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim() : "");
                        cmd.Parameters.AddWithValue("@CustomerPhoneNumber", txtMobileNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerDateofBirth", dtDOB.Date);
                        cmd.Parameters.AddWithValue("@CustomerPincode", hdnPinCode.Value);
                        cmd.Parameters.AddWithValue("@PinCodeLocality", hdnPinCodeLocality.Value);
                        cmd.Parameters.AddWithValue("@CountryID", "64");
                        cmd.Parameters.AddWithValue("@CountryName", "India");
                        cmd.Parameters.AddWithValue("@CreditScore_StateCode", hdnCreditScoreStateId.Value);
                        cmd.Parameters.AddWithValue("@StateCode", hdnStateId.Value);
                        cmd.Parameters.AddWithValue("@StateName", lblStateName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CityDistrictCode", hdnDistrictId.Value);
                        cmd.Parameters.AddWithValue("@CityDistrictName", lblDistrictName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CityId", hdnCityId.Value);
                        cmd.Parameters.AddWithValue("@CityName", lblCityName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Landmark", "NA");
                        cmd.Parameters.AddWithValue("@Response_AddressLine1", Response_AddressLine1);
                        cmd.Parameters.AddWithValue("@Response_AddressLine2", Response_AddressLine2);
                        cmd.Parameters.AddWithValue("@Response_AddressLine3", Response_AddressLine3);
                        cmd.Parameters.AddWithValue("@Response_Exact_match", Response_Exact_match);
                        cmd.Parameters.AddWithValue("@RequestXML", RequestXML_CreditScore);
                        cmd.Parameters.AddWithValue("@ResponseXML", ResponseXML_CreditScore);
                        cmd.Parameters.AddWithValue("@ErrorMessageIfAny", ErrorMessageIfAny);
                        cmd.Parameters.AddWithValue("@CreditScoreUserMessageText", CreditScoreUserMessageText);

                        cmd.Parameters.AddWithValue("@Response_IncomeTaxPanNumber", Response_IncomeTaxPanNumber);
                        cmd.Parameters.AddWithValue("@Response_PassportNumber", Response_PassportNumber);
                        cmd.Parameters.AddWithValue("@Response_PhoneNumber", Response_PhoneNumber);
                        cmd.Parameters.AddWithValue("@Response_EmailAddress", Response_EmailAddress);
                        cmd.Parameters.AddWithValue("@QuoteVersion", MaxQuoteVersion);

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveRequestResponse_CreditScore Method");
            }
        }

        private void SaveRequestResponse_CreditScore_From_Existing(string NewQuoteNumber, int MaxQuoteVersion)
        {
            DateTime dtDOB = DateTime.ParseExact(txtCustomerDOB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strDateOfBirth = dtDOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_CREDIT_SCORE_REQUEST_RESPONSE_FROM_EXISTING";

                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.Parameters.AddWithValue("@NewQuoteNumber", NewQuoteNumber);
                        cmd.Parameters.AddWithValue("@CustomerFirstName", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerLastName", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerDateofBirth", dtDOB.Date);
                        cmd.Parameters.AddWithValue("@QuoteVersion", MaxQuoteVersion);
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveRequestResponse_CreditScore_From_Existing Method");
            }
        }

        private void SaveRequestResponse(string strRequestXml, string strResponseXml, bool IsSuccess, string strErrorMessage, ref string strQuoteNo, ref int MaxQuoteVersion)
        {
            try
            {
                string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();

                if (hdnQuoteNumber.Value.Trim().Length <= 0)
                {
                    Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
                    SqlConnection _con = new SqlConnection(dbCOMMON.ConnectionString);
                    _con.Open();
                    SqlTransaction _tran = _con.BeginTransaction();
                    try
                    {
                        strQuoteNo = wsGen.fn_Gen_Cert_No(DateTime.Now.ToString("ddMMyy"), Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
                        //strQuoteNo = strQuoteNo + " " + txtMarketMovement.Text.Trim(); //Detach Market Movement value from Quote Number - 21-Sep-2017 CODEMMC
                        _tran.Commit();
                        _con.Close();
                    }
                    catch (Exception ex)
                    {
                        _tran.Rollback();
                        _con.Close();
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    strQuoteNo = hdnQuoteNumber.Value.Trim();
                }
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

                hdnQuoteVersion.Value = MaxQuoteVersion.ToString();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveRequestResponse Method, QuoteNumber: " + strQuoteNo + " , Request Executed On: " + CommonExtensions.GetLocalIPAddress());
                throw new Exception(ex.Message);
            }
        }

        private void SaveRequestValues(string strQuoteNo, bool IsSuccess, string strErrorMessage, string CreditScore, string SourceOfRequest, int MaxQuoteVersion)
        {
            string PropGeneralProposalInformation_NoPrevInsuranceFlag = string.Empty;
            string PropGeneralNodes_ApplicationDate = string.Empty;
            string PropBranchDetails_IMDBranchName = string.Empty;
            string PropBranchDetails_IMDBranchCode = string.Empty;
            string PropIntermediaryDetails_IntermediaryCode = string.Empty;
            string PropIntermediaryDetails_IntermediaryName = string.Empty;
            string PropProductName = string.Empty;
            string PropFieldUserDetails_FiledUserUserID = string.Empty;
            string PropGeneralProposalInformation_BusinessType_Mandatary = string.Empty;
            string PropDistributionChannel_BusineeChanneltype = string.Empty;
            string PropIntermediaryDetails_IntermediaryType = string.Empty;
            
            string PropCustomerDtls_CustomerID_Mandatary = string.Empty;
            string PropCustomerDtls_CustomerName = string.Empty;
            string PropRisks_MainDriver = string.Empty;
            string PropCustomerDtls_CustomerType = string.Empty;
            string PropRisks_PrevYearNCB = string.Empty;
            string PropRisks_Manufacture = string.Empty;
            string PropRisks_ManufacturerCode = string.Empty;
            string PropRisks_Model = string.Empty;
            string PropRisks_ModelCode = string.Empty;
            string PropRisks_TypeofPolicyHolder = string.Empty;
            string PropRisks_ModelVariant = string.Empty;
            string PropRisks_VariantCode = string.Empty;
            string PropRisks_VehicleSegment = string.Empty;
            string PropRisks_SeatingCapacity = string.Empty;
            string PropRisks_FuelType = string.Empty;
            string PropRisks_IsExternalCNGLPGAvailable = string.Empty;
            string PropRisks_CNGLPGkitValue = string.Empty;
            string PropRisks_DateofRegistration = string.Empty;
            string PropRisks_Dateofpurchase = string.Empty;
            string PropRisks_ManufactureYear = string.Empty;
            string PropRisks_RTOCode = string.Empty;
            string PropRisks_AuthorityLocation = string.Empty;
            string PropRisks_RTOCluster = string.Empty;
            string PropRisks_Zone_Mandatary = string.Empty;
            string PropRisks_CubicCapacity = string.Empty;
            string PropRisks_ModelCluster = string.Empty;
            string PropRisks_IDVofthevehicle = string.Empty;
            string PropRisks_NonElectricalAccessories = string.Empty;
            string PropRisks_ElectricalAccessories = string.Empty;
            string PropRisks_CompulsoryPAwithOwnerDriver = string.Empty;
            string PropRisks_VoluntaryDeductibleAmount = string.Empty;
            string PropRisks_ReturnToInvoice = string.Empty;
            string PropRisks_RoadsideAssistance = string.Empty;
            string PropRisks_EngineSecure = string.Empty;
            string PropRisks_DepreciationReimbursement = string.Empty;
            string PropRisks_VlntryDedctbleFrDprctnCover = string.Empty;
            string PropRisks_ConsumablesExpenses = string.Empty;
            string PropRisks_MarketMovement = string.Empty;
            string PropRisks_InsuredCreditScore = string.Empty;
            string PropRisks_NumberofPersonsUnnamed = string.Empty;
            string PropRisks_CapitalSIPerPerson = string.Empty;
            string NamedpassengerNomineeDetail = string.Empty;
            string PropRisks_NoOfPersonsNamed = string.Empty;
            string CapitalSIPerPerson = string.Empty;
            string PropRisks_NoOfPersonsPaidDriver = string.Empty;
            string PropRisks_SumInsuredPaidDriver = string.Empty;
            string PropRisks_WiderLegalLiabilityToPaid = string.Empty;
            string PropRisks_NoOfPersonWiderLegalLiability = string.Empty;
            string PropRisks_LegalLiabilityToEmployees = string.Empty;
            string PropRisks_NoOfPersonsLiabilityEmployee = string.Empty;
            string PropGeneralProposalInformation_PolicyEffectivedate = string.Empty;
            string PropGeneralProposalInformation_ProposalDate_Mandatary = string.Empty;
            string PropGeneralNodes_PartnerApplicationDate = string.Empty;
            string PropGeneralProposalInformation_BranchInwardDate = string.Empty;
            string PropReferenceNoDate_ReferenceDate_Mandatary = string.Empty;
            string PropPolicyEffectivedate_Todate_Mandatary = string.Empty;
            string PropPolicyEffectivedate_Fromdate_Mandatary = string.Empty;
            string PropPreviousPolicyDetails_PreviousPolicyType = string.Empty;
            string PropPreviousPolicyDetails_TotalClaimsAmount = string.Empty;
            string PropPreviousPolicyDetails_PolicyEffectiveTo = string.Empty;
            string PropPreviousPolicyDetails_PolicyEffectiveFrom = string.Empty;
            string PropPreviousPolicyDetails_NoOfClaims = string.Empty;
            string PropPreviousPolicyDetails_PolicyNo = string.Empty;
            string PropPreviousPolicyDetails_PolicyYear_Mandatary = string.Empty;
            string PropRisks_NoClaimBonusApplicable = string.Empty;
            string PropGeneralProposalInformation_IsNCBApplicable = string.Empty;
            string PropRisks_AuthorityLocationName = string.Empty;
            string PropRisks_BreakInInsurance = string.Empty;
            string PropRisks_BasicODDeviation = txtBasicODdeviation.Text.Trim();
            string PropRisks_AddOnDeviation = txtAddOnDeviation.Text.Trim();


            string PropMODetails_PrimaryMOCode = string.Empty;
            string PropMODetails_PrimaryMOName = string.Empty;

            string PropGeneralProposalInformation_OfficeCode = string.Empty;
            string PropGeneralProposalInformation_OfficeName = string.Empty;

            DateTime dtAppDate = DateTime.Now;
            string strCurrentDate = dtAppDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strDOR = dtDOR.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime dtPE = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strPE = dtPE.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            string PreviousPolicyStartDate = dtPE.AddYears(-1).AddDays(1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            PropMODetails_PrimaryMOCode = hdnPrimaryMOCode.Value.Trim();
            PropMODetails_PrimaryMOName = hdnPrimaryMOName.Value.Trim();

            PropGeneralProposalInformation_OfficeCode = hdnOfficeCode.Value.Trim();
            PropGeneralProposalInformation_OfficeName = hdnOfficeName.Value.Trim();

            PropGeneralProposalInformation_NoPrevInsuranceFlag = rbbtNewBusiness.Checked ? "True" : "False";
            PropGeneralNodes_ApplicationDate = strCurrentDate;
            PropBranchDetails_IMDBranchName = "";
            PropBranchDetails_IMDBranchCode = "";
            PropIntermediaryDetails_IntermediaryCode = lblIntermediaryCode.Text;
            PropIntermediaryDetails_IntermediaryName = txtIntermediaryName.Text.Trim();
            PropProductName = "Comprehensive Policy"; //drpProductType.SelectedValue;
            PropFieldUserDetails_FiledUserUserID = "";
            PropGeneralProposalInformation_BusinessType_Mandatary = rbbtNewBusiness.Checked ? "New Business" : "Roll Over";
            PropDistributionChannel_BusineeChanneltype = lblIntermediaryBusineeChannelType.Text.Trim();
            PropIntermediaryDetails_IntermediaryType = hdnIntermediaryType.Value;

            string CUSTOMERID_INDIVIDUAL = ConfigurationManager.AppSettings["CUSTOMERID_INDIVIDUAL"].ToString();
            string CUSTOMERNAME_INDIVIDUAL = ConfigurationManager.AppSettings["CUSTOMERNAME_INDIVIDUAL"].ToString();
            string CUSTOMERID_ORG = ConfigurationManager.AppSettings["CUSTOMERID_ORG"].ToString();
            string CUSTOMERNAME_ORG = ConfigurationManager.AppSettings["CUSTOMERNAME_ORG"].ToString();


            string CUSTOMERID_INDIVIDUAL_FEMALE = ConfigurationManager.AppSettings["CUSTOMERID_INDIVIDUAL_FEMALE"].ToString();
            string CUSTOMERNAME_INDIVIDUAL_FEMALE = ConfigurationManager.AppSettings["CUSTOMERNAME_INDIVIDUAL_FEMALE"].ToString();


            if (rbctIndividual.Checked)
            {
                if (drpCustomerGender.SelectedIndex == 0)
                {
                    PropCustomerDtls_CustomerID_Mandatary = rbctIndividual.Checked ? CUSTOMERID_INDIVIDUAL : CUSTOMERID_ORG;
                    PropCustomerDtls_CustomerName = rbctIndividual.Checked ? CUSTOMERNAME_INDIVIDUAL : CUSTOMERNAME_ORG;
                }
                else
                {
                    PropCustomerDtls_CustomerID_Mandatary = CUSTOMERID_INDIVIDUAL_FEMALE;
                    PropCustomerDtls_CustomerName = CUSTOMERNAME_INDIVIDUAL_FEMALE;
                }
            }
            else
            {
                PropCustomerDtls_CustomerID_Mandatary = CUSTOMERID_ORG;
                PropCustomerDtls_CustomerName = CUSTOMERNAME_ORG;
            }

            PropRisks_MainDriver = rbctIndividual.Checked ? "Self - Owner Driver" : "Any Other";
            PropCustomerDtls_CustomerType = rbctIndividual.Checked ? "Individual" : "Organization";
            PropRisks_PrevYearNCB = drpPreviousYearNCBSlab.SelectedValue;
            PropRisks_Manufacture = drpVehicleMake.SelectedItem.Text.ToUpper();
            PropRisks_ManufacturerCode = drpVehicleMake.SelectedValue.ToUpper();
            PropRisks_Model = drpVehicleModel.SelectedItem.Text.ToUpper();
            PropRisks_ModelCode = drpVehicleModel.SelectedValue;

            PropRisks_TypeofPolicyHolder = drpPolicyHolderType.SelectedValue;

            string[] strVST = drpVehicleSubType.SelectedItem.Text.Split('[');
            PropRisks_ModelVariant = strVST[0].Trim().ToUpper();

            PropRisks_VariantCode = drpVehicleSubType.SelectedValue.ToUpper();
            PropRisks_VehicleSegment = lblVehicleSegment.Text.Trim().ToUpper();
            PropRisks_SeatingCapacity = lblSeatingCapacityt.Text.Trim();
            PropRisks_FuelType = lblFuelTypet.Text.Trim();
            PropRisks_IsExternalCNGLPGAvailable = chkCNGLPG.Checked ? "True" : "False";
            PropRisks_CNGLPGkitValue = txtLPGKitSumInsured.Text.Trim();

            PropRisks_DateofRegistration = strDOR;
            PropRisks_Dateofpurchase = strDOR;
            PropRisks_ManufactureYear = txtMfgYear.Text; //dtDOR.Year.ToString();
            PropRisks_RTOCode = drpRTOLocation.SelectedValue.Trim();
            PropRisks_AuthorityLocation = hfSelectedRTO.Value; // txtRTOAuthorityCode.Text.Trim();
            PropRisks_AuthorityLocationName = drpRTOLocation.SelectedItem.Text.Trim();
            PropRisks_RTOCluster = lblRTOCluster.Text.Trim().ToUpper();
            PropRisks_Zone_Mandatary = hdnRTOZone.Value;
            PropRisks_CubicCapacity = lblCubicCapacityt.Text.Trim();
            PropRisks_ModelCluster = lblModelCluster.Text.Trim().ToUpper();
            PropRisks_IDVofthevehicle = txtIDVofVehicle.Text.Trim() == "" ? "0" : txtIDVofVehicle.Text.Trim();
            PropRisks_NonElectricalAccessories = txtNeaSumInsured.Text.Trim();
            PropRisks_ElectricalAccessories = txtEaSumInsured.Text.Trim();
            PropRisks_CompulsoryPAwithOwnerDriver = (rbctIndividual.Checked && drpTenureOwnerDriver.SelectedValue != "0") ? "True" : "False";
            PropRisks_VoluntaryDeductibleAmount = drpVDA.SelectedValue;
            PropRisks_ReturnToInvoice = chkReturnToInvoice.Checked ? "True" : "False";
            PropRisks_RoadsideAssistance = chkRoadsideAssistance.Checked ? "True" : "False";
            PropRisks_EngineSecure = chkEngineProtect.Checked ? "True" : "False";
            PropRisks_DepreciationReimbursement = chkDepreciationCover.Checked ? "True" : "False";
            PropRisks_VlntryDedctbleFrDprctnCover = chkDepreciationCover.Checked ? ConfigurationManager.AppSettings["VDEPCoverAmount"].ToString() : "0";
            PropRisks_ConsumablesExpenses = chkConsumableCover.Checked ? "True" : "False";
            PropRisks_MarketMovement = txtMarketMovement.Text.Trim();
            PropRisks_InsuredCreditScore = CreditScore;
            PropRisks_NumberofPersonsUnnamed = txtNumberOfPersonsUnnamed.Text.Trim();
            PropRisks_CapitalSIPerPerson = drpCapitalSIPerPerson.Text.Trim();
            NamedpassengerNomineeDetail = "GRP291";
            PropRisks_NoOfPersonsNamed = "0"; // txtNumberofPersonsNamed.Text.Trim();
            CapitalSIPerPerson = "0"; // drpCapitalSINamed.Text.Trim();
            PropRisks_NoOfPersonsPaidDriver = drpNoofPaidDrivers.SelectedValue;
            PropRisks_SumInsuredPaidDriver = drpSIPaidDriver.SelectedValue;
            PropRisks_WiderLegalLiabilityToPaid = chkWLLPD.Checked ? "True" : "False";
            PropRisks_NoOfPersonWiderLegalLiability = txtNoofPersonsWLL.Text.Trim();
            PropRisks_LegalLiabilityToEmployees = chkLLEE.Checked ? "True" : "False";
            PropRisks_NoOfPersonsLiabilityEmployee = txtNoOfEmployees.Text.Trim();

            DateTime dtPolicyEffectivedate = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strdate_PolicyEffectivedate = dtPolicyEffectivedate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            PropGeneralProposalInformation_PolicyEffectivedate = strdate_PolicyEffectivedate;

            PropGeneralProposalInformation_ProposalDate_Mandatary = strCurrentDate;
            PropGeneralNodes_PartnerApplicationDate = strCurrentDate;
            PropGeneralProposalInformation_BranchInwardDate = strCurrentDate;
            PropReferenceNoDate_ReferenceDate_Mandatary = strCurrentDate;
            //PropPolicyEffectivedate_Todate_Mandatary = DateTime.Now.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            //PropPolicyEffectivedate_Fromdate_Mandatary = strCurrentDate;

            PropRisks_BreakInInsurance = GetBreakInInsuranceType();

            if (rbbtRollOver.Checked)
            {
                //DateTime dt = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //dt = dt.AddDays(1);
                //string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //PropPolicyEffectivedate_Fromdate_Mandatary = strdate;
                //PropPolicyEffectivedate_Todate_Mandatary = dt.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime dt = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                PropPolicyEffectivedate_Fromdate_Mandatary = strdate;
                //PropPolicyEffectivedate_Todate_Mandatary = dt.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                PropPolicyEffectivedate_Todate_Mandatary = FN_Get_Policy_End_Date(strdate, 1);
            }
            else
            {
                //PropPolicyEffectivedate_Todate_Mandatary = DateTime.Now.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //PropPolicyEffectivedate_Fromdate_Mandatary = strCurrentDate;

                DateTime dt = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strdate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                PropPolicyEffectivedate_Fromdate_Mandatary = strdate;
                //PropPolicyEffectivedate_Todate_Mandatary = dt.AddYears(3).AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                PropPolicyEffectivedate_Todate_Mandatary = FN_Get_Policy_End_Date(strdate, 3);
            }

            if (rbbtRollOver.Checked)
            {
                PropPreviousPolicyDetails_PreviousPolicyType = drpCoverType.SelectedValue;
                PropPreviousPolicyDetails_TotalClaimsAmount = txtTotalClaimAmount.Text;

                PropPreviousPolicyDetails_PolicyEffectiveTo = strPE;
                PropPreviousPolicyDetails_PolicyEffectiveFrom = PreviousPolicyStartDate;

                PropPreviousPolicyDetails_NoOfClaims = drpTotalClaimCount.SelectedValue;
                PropPreviousPolicyDetails_PolicyNo = "11223344";
                PropPreviousPolicyDetails_PolicyYear_Mandatary = dtPE.AddYears(-1).Year.ToString();  //"2015";

                PropRisks_NoClaimBonusApplicable = string.Empty;
                PropGeneralProposalInformation_IsNCBApplicable = string.Empty;

                if (drpCoverType.SelectedIndex == 0) //only comprehensive product type
                {
                    if (drpTotalClaimCount.SelectedIndex == 0) //no claim
                    {
                        if (drpPreviousYearNCBSlab.SelectedItem.Text == "50" || drpPreviousYearNCBSlab.SelectedItem.Text == "55" || drpPreviousYearNCBSlab.SelectedItem.Text == "65")
                        {
                            PropRisks_NoClaimBonusApplicable = drpPreviousYearNCBSlab.SelectedItem.Text;
                        }
                        else
                        {
                            string selectedValue = drpPreviousYearNCBSlab.SelectedItem.Text;
                            drpPreviousYearNCBSlab.SelectedIndex = drpPreviousYearNCBSlab.SelectedIndex + 1;
                            PropRisks_NoClaimBonusApplicable = drpPreviousYearNCBSlab.SelectedItem.Text;
                            drpPreviousYearNCBSlab.SelectedIndex = drpPreviousYearNCBSlab.SelectedItem.Text == selectedValue ? drpPreviousYearNCBSlab.SelectedIndex : drpPreviousYearNCBSlab.SelectedIndex - 1;
                        }
                        PropGeneralProposalInformation_IsNCBApplicable = "True";
                    }
                }
            }

            //SAVE
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_MOTOR_PREMIUM_CALC_REQUEST";
                        cmd.Parameters.AddWithValue("@QuoteNumber", strQuoteNo);
                        cmd.Parameters.AddWithValue("@IsSuccess", IsSuccess);
                        cmd.Parameters.AddWithValue("@ErrorMessage", strErrorMessage);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["vUserLoginId"].ToString().ToUpper());

                        cmd.Parameters.AddWithValue("@PropGeneralProposalInformation_NoPrevInsuranceFlag", PropGeneralProposalInformation_NoPrevInsuranceFlag);
                        cmd.Parameters.AddWithValue("@PropGeneralNodes_ApplicationDate", PropGeneralNodes_ApplicationDate);
                        cmd.Parameters.AddWithValue("@PropBranchDetails_IMDBranchName", PropBranchDetails_IMDBranchName);
                        cmd.Parameters.AddWithValue("@PropBranchDetails_IMDBranchCode", PropBranchDetails_IMDBranchCode);
                        cmd.Parameters.AddWithValue("@PropIntermediaryDetails_IntermediaryCode", PropIntermediaryDetails_IntermediaryCode);
                        cmd.Parameters.AddWithValue("@PropIntermediaryDetails_IntermediaryName", PropIntermediaryDetails_IntermediaryName);
                        cmd.Parameters.AddWithValue("@PropProductName", PropProductName);
                        cmd.Parameters.AddWithValue("@PropFieldUserDetails_FiledUserUserID", PropFieldUserDetails_FiledUserUserID);
                        cmd.Parameters.AddWithValue("@PropGeneralProposalInformation_BusinessType_Mandatary", PropGeneralProposalInformation_BusinessType_Mandatary);
                        cmd.Parameters.AddWithValue("@PropDistributionChannel_BusineeChanneltype", PropDistributionChannel_BusineeChanneltype);
                        cmd.Parameters.AddWithValue("@PropIntermediaryDetails_IntermediaryType", PropIntermediaryDetails_IntermediaryType);
                        cmd.Parameters.AddWithValue("@PropCustomerDtls_CustomerID_Mandatary", PropCustomerDtls_CustomerID_Mandatary);
                        cmd.Parameters.AddWithValue("@PropCustomerDtls_CustomerName", PropCustomerDtls_CustomerName);
                        cmd.Parameters.AddWithValue("@PropRisks_MainDriver", PropRisks_MainDriver);
                        cmd.Parameters.AddWithValue("@PropCustomerDtls_CustomerType", PropCustomerDtls_CustomerType);
                        cmd.Parameters.AddWithValue("@PropRisks_PrevYearNCB", PropRisks_PrevYearNCB);
                        cmd.Parameters.AddWithValue("@PropRisks_Manufacture", PropRisks_Manufacture);
                        cmd.Parameters.AddWithValue("@PropRisks_ManufacturerCode", PropRisks_ManufacturerCode);
                        cmd.Parameters.AddWithValue("@PropRisks_Model", PropRisks_Model);
                        cmd.Parameters.AddWithValue("@PropRisks_TypeofPolicyHolder", PropRisks_TypeofPolicyHolder);
                        cmd.Parameters.AddWithValue("@PropRisks_ModelVariant", PropRisks_ModelVariant);
                        cmd.Parameters.AddWithValue("@PropRisks_VariantCode", PropRisks_VariantCode);
                        cmd.Parameters.AddWithValue("@PropRisks_VehicleSegment", PropRisks_VehicleSegment);
                        cmd.Parameters.AddWithValue("@PropRisks_SeatingCapacity", PropRisks_SeatingCapacity);
                        cmd.Parameters.AddWithValue("@PropRisks_FuelType", PropRisks_FuelType);
                        cmd.Parameters.AddWithValue("@PropRisks_IsExternalCNGLPGAvailable", PropRisks_IsExternalCNGLPGAvailable);
                        cmd.Parameters.AddWithValue("@PropRisks_CNGLPGkitValue", PropRisks_CNGLPGkitValue);
                        cmd.Parameters.AddWithValue("@PropRisks_DateofRegistration", PropRisks_DateofRegistration);
                        cmd.Parameters.AddWithValue("@PropRisks_Dateofpurchase", PropRisks_Dateofpurchase);
                        cmd.Parameters.AddWithValue("@PropRisks_ManufactureYear", PropRisks_ManufactureYear);
                        cmd.Parameters.AddWithValue("@PropRisks_RTOCode", PropRisks_RTOCode);
                        cmd.Parameters.AddWithValue("@PropRisks_AuthorityLocation", PropRisks_AuthorityLocation);
                        cmd.Parameters.AddWithValue("@PropRisks_AuthorityLocationName", PropRisks_AuthorityLocationName);
                        cmd.Parameters.AddWithValue("@PropRisks_RTOCluster", PropRisks_RTOCluster);
                        cmd.Parameters.AddWithValue("@PropRisks_Zone_Mandatary", PropRisks_Zone_Mandatary);
                        cmd.Parameters.AddWithValue("@PropRisks_CubicCapacity", PropRisks_CubicCapacity);
                        cmd.Parameters.AddWithValue("@PropRisks_ModelCluster", PropRisks_ModelCluster);
                        cmd.Parameters.AddWithValue("@PropRisks_IDVofthevehicle", PropRisks_IDVofthevehicle);
                        cmd.Parameters.AddWithValue("@PropRisks_NonElectricalAccessories", PropRisks_NonElectricalAccessories);
                        cmd.Parameters.AddWithValue("@PropRisks_ElectricalAccessories", PropRisks_ElectricalAccessories);
                        cmd.Parameters.AddWithValue("@PropRisks_CompulsoryPAwithOwnerDriver", PropRisks_CompulsoryPAwithOwnerDriver);
                        cmd.Parameters.AddWithValue("@PropRisks_VoluntaryDeductibleAmount", PropRisks_VoluntaryDeductibleAmount);
                        cmd.Parameters.AddWithValue("@PropRisks_ReturnToInvoice", PropRisks_ReturnToInvoice);
                        cmd.Parameters.AddWithValue("@PropRisks_RoadsideAssistance", PropRisks_RoadsideAssistance);
                        cmd.Parameters.AddWithValue("@PropRisks_EngineSecure", PropRisks_EngineSecure);
                        cmd.Parameters.AddWithValue("@PropRisks_DepreciationReimbursement", PropRisks_DepreciationReimbursement);
                        cmd.Parameters.AddWithValue("@PropRisks_VlntryDedctbleFrDprctnCover", PropRisks_VlntryDedctbleFrDprctnCover);
                        cmd.Parameters.AddWithValue("@PropRisks_ConsumablesExpenses", PropRisks_ConsumablesExpenses);
                        cmd.Parameters.AddWithValue("@PropRisks_MarketMovement", PropRisks_MarketMovement);
                        cmd.Parameters.AddWithValue("@PropRisks_InsuredCreditScore", PropRisks_InsuredCreditScore);
                        cmd.Parameters.AddWithValue("@PropRisks_NumberofPersonsUnnamed", PropRisks_NumberofPersonsUnnamed);
                        cmd.Parameters.AddWithValue("@PropRisks_CapitalSIPerPerson", PropRisks_CapitalSIPerPerson);
                        cmd.Parameters.AddWithValue("@NamedpassengerNomineeDetail", NamedpassengerNomineeDetail);
                        cmd.Parameters.AddWithValue("@PropRisks_NoOfPersonsNamed", PropRisks_NoOfPersonsNamed);
                        cmd.Parameters.AddWithValue("@CapitalSIPerPerson", CapitalSIPerPerson);
                        cmd.Parameters.AddWithValue("@PropRisks_NoOfPersonsPaidDriver", PropRisks_NoOfPersonsPaidDriver);
                        cmd.Parameters.AddWithValue("@PropRisks_SumInsuredPaidDriver", PropRisks_SumInsuredPaidDriver);
                        cmd.Parameters.AddWithValue("@PropRisks_WiderLegalLiabilityToPaid", PropRisks_WiderLegalLiabilityToPaid);
                        cmd.Parameters.AddWithValue("@PropRisks_NoOfPersonWiderLegalLiability", PropRisks_NoOfPersonWiderLegalLiability);
                        cmd.Parameters.AddWithValue("@PropRisks_LegalLiabilityToEmployees", PropRisks_LegalLiabilityToEmployees);
                        cmd.Parameters.AddWithValue("@PropRisks_NoOfPersonsLiabilityEmployee", PropRisks_NoOfPersonsLiabilityEmployee);
                        cmd.Parameters.AddWithValue("@PropGeneralProposalInformation_PolicyEffectivedate", PropGeneralProposalInformation_PolicyEffectivedate);
                        cmd.Parameters.AddWithValue("@PropGeneralProposalInformation_ProposalDate_Mandatary", PropGeneralProposalInformation_ProposalDate_Mandatary);
                        cmd.Parameters.AddWithValue("@PropGeneralNodes_PartnerApplicationDate", PropGeneralNodes_PartnerApplicationDate);
                        cmd.Parameters.AddWithValue("@PropGeneralProposalInformation_BranchInwardDate", PropGeneralProposalInformation_BranchInwardDate);
                        cmd.Parameters.AddWithValue("@PropReferenceNoDate_ReferenceDate_Mandatary", PropReferenceNoDate_ReferenceDate_Mandatary);
                        cmd.Parameters.AddWithValue("@PropPolicyEffectivedate_Todate_Mandatary", PropPolicyEffectivedate_Todate_Mandatary);
                        cmd.Parameters.AddWithValue("@PropPolicyEffectivedate_Fromdate_Mandatary", PropPolicyEffectivedate_Fromdate_Mandatary);
                        cmd.Parameters.AddWithValue("@PropPreviousPolicyDetails_PreviousPolicyType", PropPreviousPolicyDetails_PreviousPolicyType);
                        cmd.Parameters.AddWithValue("@PropPreviousPolicyDetails_TotalClaimsAmount", PropPreviousPolicyDetails_TotalClaimsAmount);
                        cmd.Parameters.AddWithValue("@PropPreviousPolicyDetails_PolicyEffectiveTo", PropPreviousPolicyDetails_PolicyEffectiveTo);
                        cmd.Parameters.AddWithValue("@PropPreviousPolicyDetails_PolicyEffectiveFrom", PropPreviousPolicyDetails_PolicyEffectiveFrom);
                        cmd.Parameters.AddWithValue("@PropPreviousPolicyDetails_NoOfClaims", PropPreviousPolicyDetails_NoOfClaims);
                        cmd.Parameters.AddWithValue("@PropPreviousPolicyDetails_PolicyNo", PropPreviousPolicyDetails_PolicyNo);
                        cmd.Parameters.AddWithValue("@PropPreviousPolicyDetails_PolicyYear_Mandatary", PropPreviousPolicyDetails_PolicyYear_Mandatary);
                        cmd.Parameters.AddWithValue("@PropRisks_NoClaimBonusApplicable", PropRisks_NoClaimBonusApplicable);
                        cmd.Parameters.AddWithValue("@PropGeneralProposalInformation_IsNCBApplicable", PropGeneralProposalInformation_IsNCBApplicable);
                        cmd.Parameters.AddWithValue("@PropMODetails_PrimaryMOCode", PropMODetails_PrimaryMOCode);
                        cmd.Parameters.AddWithValue("@PropMODetails_PrimaryMOName", PropMODetails_PrimaryMOName);

                        cmd.Parameters.AddWithValue("@PropGeneralProposalInformation_OfficeCode", PropGeneralProposalInformation_OfficeCode);
                        cmd.Parameters.AddWithValue("@PropGeneralProposalInformation_OfficeName", PropGeneralProposalInformation_OfficeName);

                        cmd.Parameters.AddWithValue("@SourceOfRequest", SourceOfRequest);
                        cmd.Parameters.AddWithValue("@RequestExecutedOnServer", CommonExtensions.GetLocalIPAddress());

                        cmd.Parameters.AddWithValue("@PropRisks_BreakInInsurance", PropRisks_BreakInInsurance);
                        cmd.Parameters.AddWithValue("@PropRisks_ModelCode", PropRisks_ModelCode);
                        cmd.Parameters.AddWithValue("@QuoteVersion", MaxQuoteVersion);

                        cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text.Trim());
                        cmd.Parameters.AddWithValue("@PropRisks_BasicODDeviation", PropRisks_BasicODDeviation);
                        cmd.Parameters.AddWithValue("@PropRisks_AddOnDeviation", PropRisks_AddOnDeviation);

                        cmd.Parameters.AddWithValue("@IRDA_ProductCode", drpProductType.SelectedValue);
                        cmd.Parameters.AddWithValue("@ProductDescription", drpProductType.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@TenureForOwnerDriver", drpTenureOwnerDriver.SelectedValue);
                        cmd.Parameters.AddWithValue("@CustomerGender", rbctIndividual.Checked ? drpCustomerGender.SelectedValue : "");

                        cmd.Parameters.AddWithValue("@PropRisks_LossofPersonalBelongingschk", chkLossofPersonalBelongings.Checked ? "True" : "False");
                        cmd.Parameters.AddWithValue("@PropRisks_TyreCoverChk", chkTyreCover.Checked ? "True" : "False");
                        cmd.Parameters.AddWithValue("@PropRisks_DailyCarAllowanceChk", chkDailyCarAllowance.Checked ? "True" : "False");
                        cmd.Parameters.AddWithValue("@PropRisks_NCBProtectChk", chkNCBProtect.Visible && chkNCBProtect.Checked ? "True" : "False");
                        cmd.Parameters.AddWithValue("@PropRisks_KeyReplacementChk", chkKeyReplacement.Checked ? "True" : "False");

                        cmd.Parameters.AddWithValue("@PropRisks_LossofPersonalBelongingsSIDD", chkLossofPersonalBelongings.Checked ? ddlLossofPersonalBelongingsSI.SelectedValue : "");
                        cmd.Parameters.AddWithValue("@PropRisks_KeyReplacementSIDD", chkKeyReplacement.Checked ? ddlKeyReplacement.SelectedValue : "");
                        cmd.Parameters.AddWithValue("@PropRisks_DailyCarAllowanceSIDD", chkDailyCarAllowance.Checked ? ddlDailyCarAllowance.SelectedValue : "");
                        cmd.Parameters.AddWithValue("@PropRisks_TyreCoverText", chkTyreCover.Checked ? txtTyreCoverDetails.Text.Trim() : "");

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveRequestValues Method");
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
        string IGSTPercentage, string UGSTAmount, string UGSTPercentage, string TotalGSTAmount, int MaxQuoteVersion, string Rate_BasicOD, string Rate_CC, string Rate_DC, string Rate_EP, string Rate_RTI
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

        private string GetIntermediary()
        {
            string strvIntermediaryCode = string.Empty;
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_GET_INTERMEDIARY_CODE_FOR_LOGGEDIN_USER";
                        cmd.Parameters.AddWithValue("@vUserLoginId", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.Connection = conn;
                        conn.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                txtIntermediaryName.Text = sdr["vIntermediaryName"].ToString();
                                lblIntermediaryCode.Text = sdr["vIntermediaryCode"].ToString();
                                lblIntermediaryBusineeChannelType.Text = sdr["vBusinessChannelName"].ToString();
                                hdnIntermediaryType.Value = sdr["IntermediaryType"].ToString();
                                hdnPrimaryMOCode.Value = sdr["PrimaryMOCode"].ToString();
                                hdnPrimaryMOName.Value = sdr["PrimaryMOName"].ToString();

                                hdnOfficeCode.Value = sdr["NUM_OFFICE_CD"].ToString();
                                hdnOfficeName.Value = sdr["TXT_OFFICE"].ToString();

                                hdnMinMarketMovement.Value = sdr["Min_MarketMovement"].ToString();
                                hdnMaxMarketMovement.Value = sdr["Max_MarketMovement"].ToString();
                                //txtMarketMovement.Text = hdnMaxMarketMovement.Value;
                                //txtMarketMovement.Enabled = false;

                            }
                        }
                        conn.Close();
                    }
                }

                if (txtIntermediaryName.Text.Trim().Length > 0)
                {
                    txtIntermediaryName.Enabled = false;
                }

                if (lblIntermediaryBusineeChannelType.Text.ToUpper().Trim() == "CORPORATE AGENT" || lblIntermediaryBusineeChannelType.Text.ToUpper().Trim() == "BANCASSURANCE" && rbbtRollOver.Checked)
                {
                    tdNCBProtectCover1.Visible = true;
                    tdNCBProtectCover2.Visible = true;
                    chkNCBProtect.Visible = true;
                }
                else
                {
                    tdNCBProtectCover1.Visible = false;
                    tdNCBProtectCover2.Visible = false;
                    chkNCBProtect.Checked = false;
                    chkNCBProtect.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblstatus.Text = "Error: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            return strvIntermediaryCode;
        }

        private void GetMinMaxMarketMovementForLoggedInUser()
        {
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_GET_MIN_MAX_MARKETMOVEMENT_FOR_LOGGEDIN_USER";
                        cmd.Parameters.AddWithValue("@vUserLoginId", Session["vUserLoginId"].ToString().ToUpper());
                        cmd.Connection = conn;
                        conn.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                hdnMinMarketMovement.Value = sdr["Min_MarketMovement"].ToString();
                                hdnMaxMarketMovement.Value = sdr["Max_MarketMovement"].ToString();
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetMinMaxMarketMovementForLoggedInUser Method");
            }
        }

        private bool IsDeclined_RTO_MODEL_Combination(string strRTOCode, string strModelCode)
        {
            bool IsDeclined = false;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_VALIDATE_RTO_MODEL_COMBINATION";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "MODEL_CODE", DbType.String, ParameterDirection.Input, "MODEL_CODE", DataRowVersion.Current, strModelCode);
                db.AddParameter(dbCommand, "RTO_CODE", DbType.String, ParameterDirection.Input, "RTO_CODE", DataRowVersion.Current, strRTOCode);

                dbCommand.CommandType = CommandType.StoredProcedure;
                DataSet ds = null;
                ds = db.ExecuteDataSet(dbCommand);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsDeclined = Convert.ToBoolean(ds.Tables[0].Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "IsDeclined_RTO_MODEL_Combination Method");
            }
            return IsDeclined;
        }

        protected void btnViewPremium_Click(object sender, EventArgs e)
        {
            string strErrorMsg = string.Empty;
            if (Validation(out strErrorMsg))
            {
                lblstatus.Text = "Error: " + strErrorMsg;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                //string userName = WebConfigurationManager.AppSettings["PFUserName"];
                Reset();
                CalculatePremium();
            }
        }

        public bool IsValidDateFormat(string date)
        {
            DateTime d;
            return DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        }

        private bool Validation(out string strErrorMsg)
        {
            bool IsError = false;
            try
            {
                string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dtDOB = DateTime.ParseExact(txtCustomerDOB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strDateOfBirth = dtDOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                //string strXmlPath22 = AppDomain.CurrentDomain.BaseDirectory + "TEST.txt";
                //File.WriteAllText(strXmlPath22, strDateOfBirth + "----" + strCurrentDate);

                //Added for CR Number P1_124 Check in again
                decimal MinDeviationIDV = Convert.ToDecimal(ConfigurationManager.AppSettings["MinDeviationIDV"]);
                decimal MaxDeviationIDV = Convert.ToDecimal(ConfigurationManager.AppSettings["MaxDeviationIDV"]);
                // End here 

                decimal FinalIDV = Convert.ToDecimal(hdnFinalIDV.Value);
                decimal MinPercentIdv = (FinalIDV * MinDeviationIDV) / 100;
                decimal MaxPercentIdv = (FinalIDV * MaxDeviationIDV) / 100;
                decimal minIDV = FinalIDV - MinPercentIdv;
                decimal maxIDV = FinalIDV + MaxPercentIdv;

                DateTime dtPolicyStartDate = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strPolicyStartDate = dtPolicyStartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime dtDateOfRegistration = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strDateOfRegistration = dtDateOfRegistration.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime dt_NewBusinessCondition = DateTime.ParseExact("01/09/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture);



                int days = (DateTime.Now - dtDateOfRegistration).Days;

                DateTime now = DateTime.Today;
                int age = now.Year - dtDateOfRegistration.Year;
                if (now < dtDateOfRegistration.AddYears(age)) age--;


                strErrorMsg = "";

                DateTime dtPE = DateTime.ParseExact(txtPreviousPolicyExpiryDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string PreviousPolicyStartDate = dtPE.AddYears(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                decimal BasicOdDeviationMin = Convert.ToDecimal(ConfigurationManager.AppSettings["BasicOdDeviationMin"]);
                decimal BasicOdDeviationMax = Convert.ToDecimal(ConfigurationManager.AppSettings["BasicOdDeviationMax"]);
                decimal AddOnDeviationMin = Convert.ToDecimal(ConfigurationManager.AppSettings["AddOnDeviationMin"]);
                decimal AddOnDeviationMax = Convert.ToDecimal(ConfigurationManager.AppSettings["AddOnDeviationMax"]);

                //CR 132
                int RegYear = dtDateOfRegistration.Year;
                int MfgYear = Convert.ToInt32(txtMfgYear.Text);
                //CR 132

                //if (drpProductType.SelectedIndex == 0)
                //{
                //    IsError = true;
                //    strErrorMsg = "Please Select Product Type";
                //}
                //else 
                if (drpPolicyHolderType.SelectedIndex == 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Select Policy Holder Type";
                }
                else if (txtIntermediaryName.Text.Trim().Length <= 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Select Intermediary Name and code";
                }
                else if (lblIntermediaryBusineeChannelType.Text.Trim() == "Select")
                {
                    IsError = true;
                    strErrorMsg = "Please Select Intermediary/Businee Channel Type";
                }
                else if (!Regex.IsMatch(txtMarketMovement.Text.Trim(), @"^-?\d{0,9}(\.\d{1,2})?$"))
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Valid Market Movement, Only 2 digits after decimal are allowed and allowed characters are numbers, single dot(.) and minus(-) symbol";
                }
                // CR 132 start here
                else if (!Regex.IsMatch(txtBasicODdeviation.Text.Trim(), @"^-?\d{0,9}(\.\d{1,2})?$"))
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Valid Basic OD deviation, Only 2 digits after decimal are allowed and allowed characters are numbers, single dot(.) and minus(-) symbol";
                }

                else if (!Regex.IsMatch(txtAddOnDeviation.Text.Trim(), @"^-?\d{0,9}(\.\d{1,2})?$"))
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Valid Add-On deviation, Only 2 digits after decimal are allowed and allowed characters are numbers, single dot(.) and minus(-) symbol";
                }
                else if (Convert.ToDecimal(txtBasicODdeviation.Text) < BasicOdDeviationMin || Convert.ToDecimal(txtBasicODdeviation.Text) > BasicOdDeviationMax)
                {
                    IsError = true;
                    strErrorMsg = "Basic OD deviation must between " + BasicOdDeviationMin.ToString() + " and " + BasicOdDeviationMax.ToString();
                }

                else if (Convert.ToDecimal(txtAddOnDeviation.Text) < AddOnDeviationMin || Convert.ToDecimal(txtAddOnDeviation.Text) > AddOnDeviationMax)
                {
                    IsError = true;
                    strErrorMsg = "Add on deviation must between " + AddOnDeviationMin.ToString() + " and " + AddOnDeviationMax.ToString();
                }
                // CR 132 end here

                // CR 132 start here
                else if (MfgYear > RegYear)
                {
                    IsError = true;
                    strErrorMsg = "Manufacture year can not be greater than Registration year.";
                }

                //CR 132 end here

                else if (lblIntermediaryBusineeChannelType.Text.Trim().ToUpper() == "BANCASSURANCE" && (Convert.ToDecimal(txtMarketMovement.Text) < Convert.ToDecimal(hdnMinMarketMovement.Value) || Convert.ToDecimal(txtMarketMovement.Text) > Convert.ToDecimal(hdnMaxMarketMovement.Value)))
                {
                    IsError = true;
                    //strErrorMsg = "Please Enter Market Movement between -15 to 15 percent";
                    strErrorMsg = "Maximum permissible market movement limit exceeds as defined for the user";
                }
                else if (lblIntermediaryBusineeChannelType.Text.Trim().ToUpper() == "CORPORATE AGENT" && (Convert.ToDecimal(txtMarketMovement.Text) < Convert.ToDecimal(hdnMinMarketMovement.Value) || Convert.ToDecimal(txtMarketMovement.Text) > Convert.ToDecimal(hdnMaxMarketMovement.Value)))
                {
                    IsError = true;
                    //strErrorMsg = "Please Enter Market Movement between -15 to 15 percent"; // ALLOWED -15 PREVIOUS IT WAS -10
                    strErrorMsg = "Maximum permissible market movement limit exceeds as defined for the user";
                }
                else if (lblIntermediaryBusineeChannelType.Text.Trim().ToUpper() != "BANCASSURANCE" && (Convert.ToDecimal(txtMarketMovement.Text) < Convert.ToDecimal(hdnMinMarketMovement.Value) || Convert.ToDecimal(txtMarketMovement.Text) > Convert.ToDecimal(hdnMaxMarketMovement.Value)))
                {
                    IsError = true;
                    //strErrorMsg = "Please Enter Market Movement between -15 to 15 percent";
                    strErrorMsg = "Maximum permissible market movement limit exceeds as defined for the user";
                }
                else if (lblIntermediaryBusineeChannelType.Text.Trim().ToUpper() != "CORPORATE AGENT" && (Convert.ToDecimal(txtMarketMovement.Text) < Convert.ToDecimal(hdnMinMarketMovement.Value) || Convert.ToDecimal(txtMarketMovement.Text) > Convert.ToDecimal(hdnMaxMarketMovement.Value)))
                {
                    IsError = true;
                    //strErrorMsg = "Please Enter Market Movement between -15 to 15 percent";
                    strErrorMsg = "Maximum permissible market movement limit exceeds as defined for the user";
                }
                else if (txtRTOAuthorityCode.Text.Trim().Length <= 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter RTO Authority Code";
                }
                else if (txtRTOAuthorityCode.Text.Trim().Length <= 2)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter valid RTO Authority Code";
                }
                else if (drpRTOLocation.SelectedIndex == 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter RTO Authority Location";
                }
                else if (txtRTOAuthorityCode.Text != hfSelectedRTO.Value)
                {
                    IsError = true;
                    strErrorMsg = "Please Select RTO Code from the suggestion list";
                }
                else if (drpVehicleMake.SelectedIndex == 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Select Vehicle Make";
                }
                else if (drpVehicleModel.SelectedIndex == 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Select Vehicle Model";
                }
                else if (drpVehicleSubType.Items.Count > 1 && drpVehicleSubType.SelectedIndex == 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Select Vehicle Sub Type";
                }
                else if (IsDeclined_RTO_MODEL_Combination(drpRTOLocation.SelectedValue.ToString(), drpVehicleModel.SelectedValue.ToString()))
                {
                    IsError = true;
                    strErrorMsg = "This quote cannot be processed, please contact nearest KGI branch.";
                }
                else if (Convert.ToDecimal(txtIDVofVehicle.Text.Trim()) < minIDV || Convert.ToDecimal(txtIDVofVehicle.Text.Trim()) > maxIDV)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter IDV Between " + minIDV.ToString() + " and " + maxIDV.ToString() + " for the selected vehicle";
                }
                else if (Convert.ToDecimal(txtIDVofVehicle.Text.Trim()) > 10000000)
                {
                    IsError = true;
                    strErrorMsg = "For IDV Greater Than 1 Crore, Please Contact Service Branch";
                }
                else if (txtInsuredCreditScore.Text.Trim().Length <= 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Insured Credit Score";
                }
                else if (Convert.ToDecimal(txtInsuredCreditScore.Text.Trim()) > 1000 || Convert.ToDecimal(txtInsuredCreditScore.Text.Trim()) < 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Insured Credit Score between 0 to 1000";
                }
                else if (txtMarketMovement.Text.Trim().Length <= 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Market Movement";
                }
                else if (txtDateOfRegistration.Text.Trim().Length <= 0)
                {
                    IsError = true;
                    strErrorMsg = "Registration/Purchase Date is Mandatory";
                }
                else if (!IsValidDateFormat(txtDateOfRegistration.Text.Trim()))
                {
                    IsError = true;
                    strErrorMsg = "Please enter valid Registration/Purchase Date (dd/mm/yyyy)";
                }
                else if (dtDateOfRegistration.Year > DateTime.Now.Year)
                {
                    IsError = true;
                    strErrorMsg = "Year of Manufacturer cannot be future year, year of manufacturer is derived from purchase/registration date";
                }
                else if (((age > 1) || (days > 365)) && rbbtNewBusiness.Checked)
                {
                    IsError = true;
                    strErrorMsg = "Age of the vehicle is greater than 1 year or more than 365 days,  hence type of business cannot be New.  Please change the same to Rollover";
                }
                else if (rbbtNewBusiness.Checked && DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date < dt_NewBusinessCondition.Date)
                {
                    IsError = true;
                    strErrorMsg = "In Case Of New Business: Date of Registration cannot be less than 01/09/2018";
                }
                else if (txtPolicyStartDate.Text.Trim().Length <= 0)
                {
                    IsError = true;
                    strErrorMsg = "Policy Start Date is Mandatory";
                }
                else if (!IsValidDateFormat(txtPolicyStartDate.Text.Trim()))
                {
                    IsError = true;
                    strErrorMsg = "Please enter valid Policy Start Date (dd/mm/yyyy)";
                }
                else if (Convert.ToInt32(txtNeaSumInsured.Text.Trim()) > 50000)
                {
                    IsError = true;
                    strErrorMsg = "Non Electrical Accessories Sum Insured cannot be greater than 50000";
                }
                else if (Convert.ToInt32(txtEaSumInsured.Text.Trim()) > 50000)
                {
                    IsError = true;
                    strErrorMsg = "Non Electrical Accessories Sum Insured cannot be greater than 50000";
                }
                else if (rbctOrganization.Checked && chkIsGetCreditScore.Checked)
                {
                    IsError = true;
                    strErrorMsg = "Please uncheck get credit score as it is only applicable for the Individual Customer Type";
                }
                else if (dtDateOfRegistration.Date > dtPolicyStartDate.Date)
                {
                    IsError = true;
                    strErrorMsg = "Purchase/Registration Date cannot be After policy start Date";
                }
                else if (dtPolicyStartDate.Date > DateTime.Now.AddDays(60).Date)
                {
                    IsError = true;
                    strErrorMsg = "Policy Start Date Can be Upto 60 Days Only From Current Date";
                }
                else if (rbbtRollOver.Checked && dtPE.Date >= dtPolicyStartDate.Date)
                {
                    IsError = true;
                    strErrorMsg = "Previous Policy Expiry Date Cannot be Greater Than or Equal To Policy Start Date";
                }
                //else if (rbbtRollOver.Checked && dtDateOfRegistration.Date > dtPE.AddYears(-1).Date)
                //{
                //    IsError = true;
                //    strErrorMsg = "Registration Date ("+ txtDateOfRegistration.Text.Trim() + ") Cannot be After Previous Policy Start Date (" + PreviousPolicyStartDate + ")";
                //}
                else if (dtDateOfRegistration.Date > DateTime.Now.Date)
                {
                    IsError = true;
                    strErrorMsg = "Purchase/Registration Date Cannot Be Future Dated";
                }
                else if (dtPolicyStartDate.Date < DateTime.Now.Date)
                {
                    IsError = true;
                    strErrorMsg = "Policy Start Date Cannot be Back Dated";
                }
                else if (txtFirstName.Text.Trim().Length <= 0 && rbctIndividual.Checked)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer First Name For Credit Score, Credit Score Details Are Mandatory for Individual Customer Type";
                }
                else if (txtLastName.Text.Trim().Length <= 0 && rbctIndividual.Checked)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer Last Name For Credit Score, Credit Score Details Are Mandatory for Individual Customer Type";
                }
                else if (txtMobileNumber.Text.Trim().Length <= 0 && rbctIndividual.Checked)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer Mobile Number For Credit Score, Credit Score Details Are Mandatory for Individual Customer Type";
                }
                else if (!Regex.IsMatch(txtMobileNumber.Text.Trim(), "^[0-9]*$") && rbctIndividual.Checked)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Valid Mobile Number";
                }
                else if (!Regex.IsMatch(txtMobileNumber.Text.Trim(), "^[0-9]*$") && rbctIndividual.Checked && txtMobileNumber.Text.Trim().Length > 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Valid Mobile Number";
                }
                else if (txtMobileNumber.Text.Trim().Length != 10 && rbctIndividual.Checked && txtMobileNumber.Text.Trim().Length > 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Valid 10 Digit Mobile Number";
                }
                else if ((txtMobileNumber.Text.StartsWith("7") || txtMobileNumber.Text.StartsWith("8") || txtMobileNumber.Text.StartsWith("9")) == false && rbctIndividual.Checked)
                {
                    IsError = true;
                    strErrorMsg = "Please enter correct customer contact details"; //"Invalid Mobile Number, mobile number must start with 7 or 8 or 9";
                }
                else if (txtMobileNumber.Text.Distinct().Count() == 1 && rbctIndividual.Checked)
                {
                    IsError = true;
                    strErrorMsg = "Please enter correct customer contact details";
                }
                //Added validation for 7 Consecutive number repeatation SR IM77652
                //Option for Consecutive number repeatation => Regex objNotWholePattern = new Regex(".*(?:(\\d)\\1{6})"); 
                else if ((txtMobileNumber.Text.Substring(0, 7).Distinct().Count() == 1 || txtMobileNumber.Text.Substring(1, 7).Distinct().Count() == 1 || txtMobileNumber.Text.Substring(2, 7).Distinct().Count() == 1) && rbctIndividual.Checked)
                {
                    IsError = true;
                    strErrorMsg = "Please enter correct customer contact details";
                }
                
                //Start: CR 571
                else if (CommonExtensions.fn_Check_MobileNo(txtMobileNumber.Text))
                {
                    IsError = true;
                    strErrorMsg = "The given contact number (Mobile) is blacklisted;  please change the contact number of the customer to proceed ahead.";//Msg changed on 22/12/2020
                }
                //End
                else if (rbctIndividual.Checked && txtCustomerDOB.Text.Trim().Length <= 0)
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer Date of Birth";
                }
                else if (rbctIndividual.Checked && !IsValidDateFormat(txtCustomerDOB.Text.Trim()))
                {
                    IsError = true;
                    strErrorMsg = "Please Enter Customer Date of Birth (dd/MM/yyyy)";
                }
                else if (rbctIndividual.Checked && dtDOB.Date > DateTime.Now.Date)
                {
                    IsError = true;
                    strErrorMsg = "Customer Date of Birth Cannot Be Future Date";
                }
                else if (rbctIndividual.Checked && hdnPinCode.Value == "0")
                {
                    IsError = true;
                    strErrorMsg = "Please select pincode from the list of pincode suggested while entering pincode";
                }
                else if (chkTyreCover.Checked && txtTyreCoverDetails.Text.Trim() == "")
                {
                    IsError = true;
                    strErrorMsg = "Please enter tyre cover details if tyre cover is selected.";
                }
                else if (rbctIndividual.Checked && txtDrivingLicenseNumberOrAadhaarNumber.Text.Trim().Length <= 0)
                {
                    IsError = true;
                    if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "PanNumber")
                    {
                        strErrorMsg = "Please Enter Pan Number";
                    }
                    else if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "Passport")
                    {
                        strErrorMsg = "Please Enter Passport Number";
                    }
                    else if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "DrivingLicense")
                    {
                        strErrorMsg = "Please Enter Driving License Number";
                    }
                    else if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "AADHAARNumber")
                    {
                        strErrorMsg = "Please Enter AADHAAR Number";
                    }
                    else if (drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue == "VoterIDNumber")
                    {
                        strErrorMsg = "Please Enter Voter ID Number";
                    }
                }
              
                else if (txtDateOfRegistration.Text.Trim().Length > 0)
                {
                    DateTime dtCurrentDate = DateTime.Now;
                    string strYearMinus8 = dtCurrentDate.AddYears(-12).ToShortDateString();

                    DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strRegistrationYear = dtDOR.ToShortDateString();

                    if (Convert.ToDateTime(strYearMinus8) > Convert.ToDateTime(strRegistrationYear))
                    {
                        IsError = true;
                        strErrorMsg = "Date of Registration should be less than 12 years";
                    }
                }

            }
            catch (Exception ex)
            {
                IsError = false;
                strErrorMsg = ex.Message;
                ExceptionUtility.LogException(ex, "Validation Method");
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
            string MaxQuoteVersion = hdnQuoteVersion.Value;
            DownloadPDF(IsOnlySavePDF: false, MaxQuoteVersion: MaxQuoteVersion);
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

        private void DownloadPDF(bool IsOnlySavePDF, string MaxQuoteVersion)
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

            bool isSuccess = false;
            if (!File.Exists(pdfPath))
            {
                isSuccess = SavePDF(strQuoteNo, strHtml, MaxQuoteVersion);
            }
            else
            {
                isSuccess = true;
            }

            if (!IsOnlySavePDF)
            {
                if (isSuccess)
                {
                    //CreatePDF(strQuoteNo, strHtml);
                    DownloadSavedPDF(strQuoteNo, pdfPath, MaxQuoteVersion);
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

        private void CreatePDF(string strQuoteNo, string strHTML)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + strQuoteNo + ".pdf");
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

                if (rbbtNewBusiness.Checked && DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= dt_NewBusinessCondition.Date)
                {
                    switch (drpProductType.SelectedValue)
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

        private void DownloadSavedPDF(string strQuoteNo, string PDFFilePath, string MaxQuoteVersion)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + strQuoteNo + "_" + MaxQuoteVersion + ".pdf");
            Response.TransmitFile(PDFFilePath);
            Response.End();
        }

        private bool SavePDF(string strQuoteNumber, string strHTML, string MaxQuoteVersion)
        {
            bool isSuccess = false;
            string fileName = string.Empty;

            DateTime fileCreationDatetime = DateTime.Now;

            fileName = string.Format("{0}.pdf", strQuoteNumber + "_" + MaxQuoteVersion);

            //pdfPath = AppDomain.CurrentDomain.BaseDirectory + (@"\PDFs\") + fileName;
            string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles_OldFormat"].ToString();
            string pdfPath = (KotakQuotesPDFFiles) + fileName;

            Document pdfDoc = new Document(PageSize.A4, 13f, 13f, 100f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

            try
            {
                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    string strProductName = "Car Secure (1+1)";

                    DateTime dt_NewBusinessCondition = DateTime.ParseExact("01/09/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (rbbtNewBusiness.Checked && DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= dt_NewBusinessCondition.Date)
                    {
                        switch (drpProductType.SelectedValue)
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

                    if (drpTenureOwnerDriver.SelectedValue != "0" || rbctOrganization.Checked == true)
                    {
                        strHTML = strHTML.Replace(@"<li>This quote is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>", "");
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

        private void Reset()
        {
            lblSystemIDV.Text = "0.00";
            lblFinalIDV.Text = "0.00";
            lblBasicTPPremium.Text = "0.00";
            lblOwnDamagePremium.Text = "0.00";
            lblConsumableCover.Text = "0.00";
            lblDepreciationCover.Text = "0.00";
            lblVoluntaryDeductionforDepWaiver.Text = "0.00";
            lblElectronicSI.Text = "0.00";
            lblNonElectronicSI.Text = "0.00";
            lblExternalBiFuelSI.Text = "0.00";
            lblEngineProtect.Text = "0.00";
            lblReturnToInvoice.Text = "0.00";
            lblRSA.Text = "0.00";
            lblLiabilityForBiFuel.Text = "0.00";
            lblPAForUnnamedPassengerSI.Text = "0.00";
            lblPAForNamedPassengerSI.Text = "0.00";
            lblPAToPaidDriverSI.Text = "0.00";
            lblPACoverForOwnerDriver.Text = "0.00";
            lblLegalLiabilityToPaidDriverNo.Text = "0.00";
            lblLLEOPDCC.Text = "0.00";
            lblVoluntaryDeduction.Text = "0.00";
            lblNCB.Text = "0.00";
            lblNetPremium.Text = "0.00";
            lblServiceTax.Text = "0.00";
            lblTotalPremium.Text = "0.00";
            //lblTotalPremiumKerala.Text = "0.00"; //CR775A - Asked to remove kerala cess - Hasmukh
            lblstatus.Text = "Status: ";
            lblTenureOwnerDriver.Text = "";

            lblDailyCarAllowance.Text = "0.00";
            lblKeyReplacement.Text = "0.00";
            lblTyreCover.Text = "0.00";
            lblNCBProtect.Text = "0.00";
            lblLossofPersonalBelongings.Text = "0.00";

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
        }

        protected void chkCNGLPG_CheckedChanged(object sender, EventArgs e)
        {
            txtLPGKitSumInsured.Text = "0";
            txtLPGKitSumInsured.Enabled = chkCNGLPG.Checked ? true : false;
            txtLPGKitSumInsured.ToolTip = chkCNGLPG.Checked ? "" : "Readonly";
        }

        protected void chkNEAR_CheckedChanged(object sender, EventArgs e)
        {
            txtNeaSumInsured.Text = "0";
            txtNeaSumInsured.Enabled = chkNEAR.Checked ? true : false;
            txtNeaSumInsured.ToolTip = chkNEAR.Checked ? "" : "Readonly";
        }

        protected void chkEAR_CheckedChanged(object sender, EventArgs e)
        {
            txtEaSumInsured.Text = "0";
            txtEaSumInsured.Enabled = chkEAR.Checked ? true : false;
            txtEaSumInsured.ToolTip = chkEAR.Checked ? "" : "Readonly";
        }

        protected void chkPACoverUnnamedPersons_CheckedChanged(object sender, EventArgs e)
        {
            txtNumberOfPersonsUnnamed.Text = chkPACoverUnnamedPersons.Checked ? lblSeatingCapacityt.Text.Trim() : "0";
            drpCapitalSIPerPerson.SelectedIndex = chkPACoverUnnamedPersons.Checked ? 10 : 0;
            drpCapitalSIPerPerson.Enabled = chkPACoverUnnamedPersons.Checked ? true : false;
            drpCapitalSIPerPerson.ToolTip = chkPACoverUnnamedPersons.Checked ? "" : "Readonly";

            //txtNumberOfPersonsUnnamed.Enabled = chkPACoverUnnamedPersons.Checked ? true : false; //commented on 08-Jan-2018 as should not be editable
            txtNumberOfPersonsUnnamed.ToolTip = chkPACoverUnnamedPersons.Checked ? "" : "Readonly";
        }

        /*protected void chkPACoverNamedPersons_CheckedChanged(object sender, EventArgs e)
        {
            txtNumberofPersonsNamed.Text = chkPACoverNamedPersons.Checked ? "1" : "0";
            txtNumberofPersonsNamed.Enabled = chkPACoverNamedPersons.Checked ? true : false;
            txtNumberofPersonsNamed.ToolTip = chkPACoverNamedPersons.Checked ? "" : "Readonly";

            drpCapitalSINamed.SelectedIndex = chkPACoverNamedPersons.Checked ? 10 : 0;
            drpCapitalSINamed.Enabled = chkPACoverNamedPersons.Checked ? true : false;
            drpCapitalSINamed.ToolTip = chkPACoverNamedPersons.Checked ? "" : "Readonly";

        }*/

        protected void chkPACoverPaidDriver_CheckedChanged(object sender, EventArgs e)
        {
            drpNoofPaidDrivers.SelectedIndex = chkPACoverPaidDriver.Checked ? 1 : 0;
            drpNoofPaidDrivers.Enabled = chkPACoverPaidDriver.Checked ? true : false;
            drpSIPaidDriver.SelectedIndex = chkPACoverPaidDriver.Checked ? 10 : 0;
            drpSIPaidDriver.Enabled = chkPACoverPaidDriver.Checked ? true : false;
        }

        protected void chkLLEE_CheckedChanged(object sender, EventArgs e)
        {
            txtNoOfEmployees.Enabled = chkLLEE.Checked ? true : false;
            txtNoOfEmployees.ToolTip = chkLLEE.Checked ? "" : "Readonly";
            txtNoOfEmployees.Text = chkLLEE.Checked ? "1" : "0";
        }

        protected void chkWLLPD_CheckedChanged(object sender, EventArgs e)
        {
            txtNoofPersonsWLL.Enabled = chkWLLPD.Checked ? true : false;
            txtNoofPersonsWLL.ToolTip = chkWLLPD.Checked ? "" : "Readonly";
            txtNoofPersonsWLL.Text = chkWLLPD.Checked ? "1" : "0";
        }

        protected void drpTotalClaimCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTotalClaimAmount.Text = drpTotalClaimCount.SelectedIndex == 0 ? "0" : "100";
        }

        protected void btnGetRtoCode_Click(object sender, EventArgs e)
        {
            hfSelectedRTO.Value = txtRTOAuthorityCode.Text.Trim();
            FillDrpRTOList(txtRTOAuthorityCode.Text.Trim());
        }

        protected void btnGetIntermediaryCode_Click(object sender, EventArgs e)
        {
            string IntermediaryCode = hfIntermediaryCode.Value;
            lblIntermediaryCode.Text = IntermediaryCode;
            SetIntermediaryBusinessChaneelType(IntermediaryCode);
        }



        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetRTO(string prefix)
        {
            List<string> RTOs = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_RTO_CODE_AUTO";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            RTOs.Add(string.Format("{0}", sdr["NUM_REGISTRATION_CODE"]));
                        }
                    }
                    conn.Close();
                }
            }
            return RTOs.ToArray();
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetIntermediaryCode(string prefix)
        {
            List<string> IntrCds = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PROC_GET_INTERMEDIARY_CODE_AUTO";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IntrCds.Add(string.Format("{0}~{1}", sdr["TXT_INTERMEDIARY_NAME"], sdr["TXT_INTERMEDIARY_CD"]));
                        }
                    }
                    conn.Close();
                }
            }
            return IntrCds.ToArray();
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

        protected void chkDepreciationCover_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepreciationCover.Checked)
            {
                chkRoadsideAssistance.Checked = true;
                chkConsumableCover.Checked = true;
                chkRoadsideAssistance.Enabled = false;
                chkConsumableCover.Enabled = false;
            }
            else
            {
                chkRoadsideAssistance.Checked = false;
                chkConsumableCover.Checked = false;
                chkRoadsideAssistance.Enabled = true;
                chkConsumableCover.Enabled = true;
            }
        }

        protected void chkReturnToInvoice_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEngineProtect.Checked || chkReturnToInvoice.Checked || chkKeyReplacement.Checked || chkTyreCover.Checked)
            {
                chkDepreciationCover.Checked = true;
                chkDepreciationCover.Enabled = false;

                chkRoadsideAssistance.Checked = true;
                chkRoadsideAssistance.Enabled = false;

                chkConsumableCover.Checked = true;
                chkConsumableCover.Enabled = false;
            }
            else
            {
                chkDepreciationCover.Checked = false;
                chkDepreciationCover.Enabled = true;

                chkRoadsideAssistance.Checked = false;
                chkRoadsideAssistance.Enabled = true;

                chkConsumableCover.Checked = false;
                chkConsumableCover.Enabled = true;
            }
        }

        protected void chkEngineProtect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEngineProtect.Checked || chkReturnToInvoice.Checked || chkKeyReplacement.Checked || chkTyreCover.Checked)
            {
                chkDepreciationCover.Checked = true;
                chkDepreciationCover.Enabled = false;

                chkRoadsideAssistance.Checked = true;
                chkRoadsideAssistance.Enabled = false;

                chkConsumableCover.Checked = true;
                chkConsumableCover.Enabled = false;
            }
            else
            {
                chkDepreciationCover.Checked = false;
                chkDepreciationCover.Enabled = true;

                chkRoadsideAssistance.Checked = false;
                chkRoadsideAssistance.Enabled = true;

                chkConsumableCover.Checked = false;
                chkConsumableCover.Enabled = true;
            }
        }

        protected void chkRoadsideAssistance_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkConsumableCover_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void txtDateOfRegistration_TextChanged(object sender, EventArgs e)
        {
            txtIDVofVehicle.Text = "0";
            hdnFinalIDV.Value = "0";
            txtIDVofVehicle.Enabled = false;

            string strErrorMsg = "";
            string[] formats = { "dd/MM/yyyy" };

            DateTime temp;
            if (DateTime.TryParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out temp))
            {
                DateTime dtCurrentDate = DateTime.Now;
                string strYearMinus8 = dtCurrentDate.AddYears(-12).ToShortDateString();

                DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strRegistrationYear = dtDOR.ToShortDateString();

                if (Convert.ToDateTime(strYearMinus8) > Convert.ToDateTime(strRegistrationYear))
                {
                    strErrorMsg = "Date of Registration should be less than 12 years";
                    lblstatus.Text = "Error: " + strErrorMsg;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    txtDateOfRegistration.Text = strCurrentDate;
                    // CR 132
                    txtMfgYear.Text = Convert.ToDateTime(strRegistrationYear).Year.ToString();
                    // CR 132
                }
                else
                {
                    //GetVehicleAge();
                    // CR 132
                    txtMfgYear.Text = Convert.ToDateTime(strRegistrationYear).Year.ToString();
                    // CR 132
                    GetVehicleAgeNew(IsQuoteModification: false);
                }

            }
            else
            {
                strErrorMsg = "Please Enter Registration Date in dd/MM/yyyy format";
                lblstatus.Text = "Error: " + strErrorMsg;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        private DataSet GetEligileCover(string NUM_MAX_AGE, string TXT_SEGMENT)
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_ELIGIBLE_COVER_BY_VEHICLE_AGE";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "NUM_MAX_AGE", DbType.String, ParameterDirection.Input, "NUM_MAX_AGE", DataRowVersion.Current, NUM_MAX_AGE);
                db.AddParameter(dbCommand, "TXT_SEGMENT", DbType.String, ParameterDirection.Input, "TXT_SEGMENT", DataRowVersion.Current, TXT_SEGMENT);

                dbCommand.CommandType = CommandType.StoredProcedure;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetEligileCover Method");
            }
            return ds;
        }

        private void GetVehicleAge()
        {
            try
            {
                decimal month = 0;
                string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strDOR = dtDOR.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                month = ((DateTime.Now.Year - dtDOR.Year) * 12) + DateTime.Now.Month - dtDOR.Month;

                month = month + 2;
                month = month / 12;
                string strVehicleAge = Math.Round(month, 3).ToString();
                month = decimal.Round(month, 0, MidpointRounding.ToEven);
                string[] mon = strVehicleAge.Split('.');

                chkConsumableCover.Checked = false;
                chkConsumableCover.Enabled = true;

                chkDepreciationCover.Checked = false;
                chkDepreciationCover.Enabled = true;

                chkEngineProtect.Checked = false;
                chkEngineProtect.Enabled = true;

                chkRoadsideAssistance.Checked = false;
                chkRoadsideAssistance.Enabled = true;

                chkReturnToInvoice.Checked = false;
                chkReturnToInvoice.Enabled = true;

                if (Convert.ToInt16(mon[0]) > 1)
                {
                    chkReturnToInvoice.Enabled = false;
                }
                if (Convert.ToInt16(mon[0]) > 2)
                {
                    chkEngineProtect.Enabled = false;
                    chkDepreciationCover.Enabled = false;
                }
                if (Convert.ToInt16(mon[0]) > 3)
                {
                    chkConsumableCover.Enabled = false;
                }

                txtDateOfRegistration.Text = dtDOR.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //calRegistration.SelectedDate = dtDOR;
            }
            catch (Exception ex)
            {
                string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strXmlPath, "Error: " + ex.Message.ToString() + "...");
            }
        }

        private void GetVehicleAgeNew(bool IsQuoteModification)
        {
            try
            {

                //decimal month = 0;
                //string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                //DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string strDOR = dtDOR.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                //month = ((DateTime.Now.Year - dtDOR.Year) * 12) + DateTime.Now.Month - dtDOR.Month;

                //month = month + 2;
                //month = month / 12;
                //string strVehicleAge = Math.Round(month, 3).ToString();

                //above is old logic and was wokring till 25-Sep-2017

                DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dtPSD = DateTime.ParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime Add60PSD = dtPSD.AddDays(0); //CR ID P1_097 - CHANGED 60 DAYS TO 0 DAYS - CHANGE DONE BY HASMUKH

                TimeSpan span = Add60PSD.Subtract(dtDOR);
                int numDifferenceInDays = (int)span.TotalDays;

                double finVehicleAge = (double)numDifferenceInDays / 365;
                string strVehicleAge = Math.Round(finVehicleAge, 3).ToString();

                if (IsQuoteModification == false)
                {
                    chkConsumableCover.Checked = false;
                    chkConsumableCover.Enabled = false;

                    chkDepreciationCover.Checked = false;
                    chkDepreciationCover.Enabled = false;

                    chkEngineProtect.Checked = false;
                    chkEngineProtect.Enabled = false;

                    chkRoadsideAssistance.Checked = false;
                    chkRoadsideAssistance.Enabled = false;

                    chkReturnToInvoice.Checked = false;
                    chkReturnToInvoice.Enabled = false;

                    //CR164
                    chkNCBProtect.Checked = false;
                    chkNCBProtect.Enabled = false;

                    chkLossofPersonalBelongings.Checked = false;
                    chkLossofPersonalBelongings.Enabled = false;
                    ddlLossofPersonalBelongingsSI.Enabled = false;

                    chkKeyReplacement.Checked = false;
                    chkKeyReplacement.Enabled = false;
                    ddlKeyReplacement.Enabled = false;

                    chkDailyCarAllowance.Checked = false;
                    chkDailyCarAllowance.Enabled = false;
                    ddlDailyCarAllowance.Enabled = false;

                    chkTyreCover.Checked = false;
                    chkTyreCover.Enabled = false;
                    txtTyreCoverDetails.Enabled = false;
                }

                DataSet ds = GetEligileCover(strVehicleAge, lblVehicleSegment.Text.Trim().ToUpper());

                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            bool IsCC = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsCC"].ToString());
                            bool IsDC = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDC"].ToString());
                            bool IsEP = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsEP"].ToString());
                            bool IsRTI = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRTI"].ToString());
                            bool IsRSA = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRSA"].ToString());


                            bool IsNCB = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsNCB"].ToString());
                            bool IsLOPB = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsLOPB"].ToString());
                            bool IsKR = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsKR"].ToString());
                            bool IsDCA = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDCA"].ToString());
                            bool IsTC = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsTC"].ToString());

                            chkConsumableCover.Enabled = IsCC;
                            chkDepreciationCover.Enabled = IsDC;
                            chkEngineProtect.Enabled = IsEP;
                            chkReturnToInvoice.Enabled = IsRTI;
                            chkRoadsideAssistance.Enabled = IsRSA;

                            //CR164
                            if (chkNCBProtect.Visible)
                            {
                                chkNCBProtect.Enabled = IsNCB;
                            }

                            chkLossofPersonalBelongings.Enabled = IsLOPB;
                            chkKeyReplacement.Enabled = IsKR;
                            chkDailyCarAllowance.Enabled = IsDCA;
                            chkTyreCover.Enabled = IsTC;


                            if (IsQuoteModification)
                            {
                                chkConsumableCover.Checked = IsCC && chkConsumableCover.Checked ? true : false;
                                chkConsumableCover.Enabled = IsCC || chkConsumableCover.Checked ? true : false;

                                chkDepreciationCover.Checked = IsDC && chkDepreciationCover.Checked ? true : false;
                                chkDepreciationCover.Enabled = IsDC || chkDepreciationCover.Checked ? true : false;

                                chkEngineProtect.Checked = IsEP && chkEngineProtect.Checked ? true : false;
                                chkEngineProtect.Enabled = IsEP || chkEngineProtect.Checked ? true : false;

                                chkReturnToInvoice.Checked = IsRTI && chkReturnToInvoice.Checked ? true : false;
                                chkReturnToInvoice.Enabled = IsRTI || chkReturnToInvoice.Checked ? true : false;

                                chkRoadsideAssistance.Checked = IsRSA && chkRoadsideAssistance.Checked ? true : false;
                                chkRoadsideAssistance.Enabled = IsRSA || chkRoadsideAssistance.Checked ? true : false;

                                //CR164
                                if (chkNCBProtect.Visible)
                                {
                                    chkNCBProtect.Checked = IsNCB && chkNCBProtect.Checked ? true : false;
                                    chkNCBProtect.Enabled = IsNCB || chkNCBProtect.Checked ? true : false;
                                }

                                chkLossofPersonalBelongings.Checked = IsLOPB && chkLossofPersonalBelongings.Checked ? true : false;
                                chkLossofPersonalBelongings.Enabled = IsLOPB || chkLossofPersonalBelongings.Checked ? true : false;

                                chkKeyReplacement.Checked = IsKR && chkKeyReplacement.Checked ? true : false;
                                chkKeyReplacement.Enabled = IsKR || chkKeyReplacement.Checked ? true : false;

                                chkDailyCarAllowance.Checked = IsDCA && chkDailyCarAllowance.Checked ? true : false;
                                chkDailyCarAllowance.Enabled = IsDCA || chkDailyCarAllowance.Checked ? true : false;

                                chkTyreCover.Checked = IsTC && chkTyreCover.Checked ? true : false;
                                chkTyreCover.Enabled = IsTC || chkTyreCover.Checked ? true : false;

                                if (chkDepreciationCover.Checked)
                                {
                                    chkRoadsideAssistance.Enabled = chkRoadsideAssistance.Checked ? false : true;
                                    chkConsumableCover.Enabled = chkConsumableCover.Checked ? false : true;
                                }

                                if (chkKeyReplacement.Checked || chkTyreCover.Checked)
                                {
                                    chkDepreciationCover.Enabled = chkDepreciationCover.Checked ? false : true;
                                    chkRoadsideAssistance.Enabled = chkRoadsideAssistance.Checked ? false : true;
                                    chkConsumableCover.Enabled = chkConsumableCover.Checked ? false : true;
                                }

                                if (chkReturnToInvoice.Checked || chkEngineProtect.Checked)
                                {
                                    chkDepreciationCover.Enabled = chkDepreciationCover.Checked ? false : true;
                                    chkRoadsideAssistance.Enabled = chkRoadsideAssistance.Checked ? false : true;
                                    chkConsumableCover.Enabled = chkConsumableCover.Checked ? false : true;
                                }
                            }

                            hdnEnabledAddOnsName.Value = chkConsumableCover.Enabled || chkConsumableCover.Checked ? "CC" : "";
                            hdnEnabledAddOnsName.Value = chkDepreciationCover.Enabled || chkDepreciationCover.Checked ? hdnEnabledAddOnsName.Value + ",DC" : hdnEnabledAddOnsName.Value;
                            hdnEnabledAddOnsName.Value = chkEngineProtect.Enabled || chkEngineProtect.Checked ? hdnEnabledAddOnsName.Value + ",EP" : hdnEnabledAddOnsName.Value;
                            hdnEnabledAddOnsName.Value = chkReturnToInvoice.Enabled || chkReturnToInvoice.Checked ? hdnEnabledAddOnsName.Value + ",RTI" : hdnEnabledAddOnsName.Value;
                            hdnEnabledAddOnsName.Value = chkRoadsideAssistance.Enabled || chkRoadsideAssistance.Checked ? hdnEnabledAddOnsName.Value + ",RSA" : hdnEnabledAddOnsName.Value;

                            //CR164
                            if (chkNCBProtect.Visible)
                            {
                                hdnEnabledAddOnsName.Value = chkNCBProtect.Enabled || chkNCBProtect.Checked ? hdnEnabledAddOnsName.Value + ",NCB" : hdnEnabledAddOnsName.Value;
                            }

                            hdnEnabledAddOnsName.Value = chkLossofPersonalBelongings.Enabled || chkLossofPersonalBelongings.Checked ? hdnEnabledAddOnsName.Value + ",LOPB" : hdnEnabledAddOnsName.Value;
                            hdnEnabledAddOnsName.Value = chkKeyReplacement.Enabled || chkKeyReplacement.Checked ? hdnEnabledAddOnsName.Value + ",KR" : hdnEnabledAddOnsName.Value;
                            hdnEnabledAddOnsName.Value = chkDailyCarAllowance.Enabled || chkDailyCarAllowance.Checked ? hdnEnabledAddOnsName.Value + ",DAC" : hdnEnabledAddOnsName.Value; //INSTEAD OF DCA HERE HAVE SET "DAC" BCOZ IN CHKSELECTALL EVENT ALREADY "DC" CHECK EXISTS
                            hdnEnabledAddOnsName.Value = chkTyreCover.Enabled || chkTyreCover.Checked ? hdnEnabledAddOnsName.Value + ",TC" : hdnEnabledAddOnsName.Value;

                            chkSelectAllCovers.Enabled = true;
                            chkSelectAllCovers.Checked = false;
                        }
                        else
                        {
                            chkConsumableCover.Enabled = false;
                            chkDepreciationCover.Enabled = false;
                            chkEngineProtect.Enabled = false;
                            chkReturnToInvoice.Enabled = false;
                            chkRoadsideAssistance.Enabled = false;

                            chkNCBProtect.Enabled = false;
                            chkLossofPersonalBelongings.Enabled = false;
                            chkKeyReplacement.Enabled = false;
                            chkDailyCarAllowance.Enabled = false;
                            chkTyreCover.Enabled = false;

                            chkSelectAllCovers.Enabled = false;
                            chkSelectAllCovers.Checked = false;
                            hdnEnabledAddOnsName.Value = "";
                        }
                    }
                }

                txtDateOfRegistration.Text = dtDOR.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //calRegistration.SelectedDate = dtDOR;
            }
            catch (Exception ex)
            {
                string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strXmlPath, "Error: " + ex.Message.ToString() + "...");
            }
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
            string MakeModelVariant = lblMake.Text + " " + lblModel.Text + " " + lblVariant.Text;

            string KotakQuotesPDFFiles = ConfigurationManager.AppSettings["KotakQuotesPDFFiles"].ToString();

            string MaxQuoteVersion = hdnQuoteVersion.Value;
            string fileName = string.Format("{0}.pdf", strQuoteNo + "_" + MaxQuoteVersion);
            string pdfPath = (KotakQuotesPDFFiles) + fileName;

            if (!File.Exists(pdfPath))
            {
                SavePDF(strQuoteNo, strHtml, MaxQuoteVersion);
            }

            if (txtEmailId.Text.Trim().Length > 0)
            {
                string strMailSubject = "Quotation [" + strQuoteNo + "] for your vehicle " + MakeModelVariant;
                string msg = SendEmail(txtEmailId.Text.Trim(), strQuoteNo, strMailSubject, MakeModelVariant, MaxQuoteVersion);
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

        private string SendEmail(string ToEmailIds, string QuoteNumber, string MailSubject, string MakeModelVariant, string MaxQuoteVersion)
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
            string attachmentFilename = (KotakQuotesPDFFiles + QuoteNumber + "_" + MaxQuoteVersion + ".pdf");


            //string PayULink = Create_Invoice(QuoteNumber.Replace(" ", "").Replace("-", "").Trim(), lblTotalPremium.Text.Substring(2, lblTotalPremium.Text.Trim().Length - 2).Replace(",", ""), "KGICustomer", "kgi.hasmukh-jain@kotak.com");
            //string PayULink = "https://test.payu.in/processInvoice?invoiceId=e676767cd80d362794d913a8267ccac62a6a37c018cd2c459a57696ff1ac260a";
            string PayULink = string.Empty;
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

                Uri uriResult;
                bool IsValidURL = Uri.TryCreate(PayULink, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (IsValidURL)
                {
                    //SAVE URL IN TABLE
                    string googleShortURL = string.Empty;
                    GoogleURLShortner(PayULink, out googleShortURL);
                    string shortURL = googleShortURL;

                    SaveInvoiceLink(PayULink, shortURL, ToEmailIds, QuoteNumber, "", QuoteNumber.Replace(" ", "").Replace("-", "").Trim());

                    string buttonHTML = "<a href='" + shortURL + "' style='background-color: #1d4581;border: 1px solid black;color: white;padding: 1px 32px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px;cursor: pointer;margin-left:2px'> Click Here </a>&nbsp; to Make Payment <br /><br />";
                    MailBody = MailBody.Replace("PayULink", buttonHTML);
                }
                else
                {
                    //do not create button to pay
                    MailBody = MailBody.Replace("PayULink", "");
                }

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(smtp_FromMailId);
                mm.Subject = MailSubject;
                mm.Body = MailBody;
                mm.IsBodyHtml = true;

                foreach (var toMailId in ToEmailAddr)
                {
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



                //mm.CC.Add(smtp_DefaultCCMailId);

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
                string strPathErrorFile = AppDomain.CurrentDomain.BaseDirectory + "Error.txt";
                File.WriteAllText(strPathErrorFile, "Error: " + strMessage);
            }

            return strMessage;
        }

        private void SaveProposalDetails(string QuoteNumber, string ProposalNumber, string CustomerId, string TotalPremium)
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

        private void GoogleURLShortner(string longURL, out string shortURL)
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
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GoogleURLShortner Method");
            }
        }

        private string Create_Invoice(string TransactionId, string TotalPremium, string FirstName, string EmailAddress)
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
                txnid = TransactionId //QuoteNumber.Replace(" -7", "") + "MAPP"
                ,
                productinfo = "Kotak Car Secure Package"
                ,
                firstname = FirstName //"Hasmukh"
                ,
                email = EmailAddress //"kgi.hasmukh-jain@kotak.com"
                ,
                phone = "7738284116" //"7738284116" //8588819411 = sushil tomhar
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

                WebRequest myWebRequest = WebRequest.Create(Url);
                if (IsUseNetworkProxy == "1")
                {
                    string proxy_Address_Full = ConfigurationManager.AppSettings["proxy_Address_Full"].ToString();
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

        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        protected void btnGetPincodeDetails_Click(object sender, EventArgs e)
        {
            lblPincodeLocality.Text = hdnPinCodeLocality.Value;
            string pincode = hdnPinCode.Value;
            SetStateCityDistrict(hdnPinCode.Value, hdnPinCodeLocality.Value);
        }

        protected void chkIsGetCreditScore_CheckedChanged(object sender, EventArgs e)
        {
            ResetCreditScoreFields();
        }

        private void ResetCreditScoreFields()
        {
            hdnPinCode.Value = "0";

            if (chkIsGetCreditScore.Checked)
            {
                txtFirstName.Enabled = true;
                txtMiddleName.Enabled = true;
                txtLastName.Enabled = true;
                //rbtnMale.Enabled = true;
                //rbtnFemale.Enabled = true;
                drpDrivingLicenseNumberOrAadhaarNumber.Enabled = true;
                txtDrivingLicenseNumberOrAadhaarNumber.Enabled = true;
                txtMobileNumber.Enabled = true;
                txtCustomerDOB.Enabled = true;
                txtPinCode.Enabled = true;
                btnGetPincodeDetails.Enabled = true;
                lblStateName.Enabled = true;
                lblCityName.Enabled = true;
                lblDistrictName.Enabled = true;
            }
            else
            {


                txtFirstName.Enabled = false;
                txtMiddleName.Enabled = false;
                txtLastName.Enabled = false;
                rbtnMale.Enabled = false;
                rbtnFemale.Enabled = false;
                drpDrivingLicenseNumberOrAadhaarNumber.Enabled = false;
                txtDrivingLicenseNumberOrAadhaarNumber.Enabled = false;
                txtMobileNumber.Enabled = false;
                txtCustomerDOB.Enabled = false;
                txtPinCode.Enabled = false;
                btnGetPincodeDetails.Enabled = false;
                lblPincodeLocality.Text = "-";
                lblStateName.Enabled = false;
                lblCityName.Enabled = false;
                lblDistrictName.Enabled = false;

                txtFirstName.Text = "";
                txtMiddleName.Text = "";
                txtLastName.Text = "";
                txtDrivingLicenseNumberOrAadhaarNumber.Text = "";
                txtMobileNumber.Text = "";
                txtPinCode.Text = "";
                hdnPinCode.Value = "";
                hdnPinCodeLocality.Value = "";
                lblPincodeLocality.Text = "-";
                lblStateName.Text = "-";
                lblCityName.Text = "-";
                lblDistrictName.Text = "-";
            }
        }

        protected void txtPolicyStartDate_TextChanged(object sender, EventArgs e)
        {
            DateTime temp;
            if (DateTime.TryParseExact(txtPolicyStartDate.Text.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out temp))
            {
                GetVehicleAgeNew(IsQuoteModification: false);
            }
            else
            {
                string strErrorMsg = "Please Enter Policy Start Date in dd/MM/yyyy format";
                lblstatus.Text = "Error: " + strErrorMsg;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        //QuoteModification

        private DataSet GetQuoteDetailsTobeModified(string QuoteNumber, int QuoteVersion)
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_MOTOR_QUOTE_DETAILS";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "vUserLoginId", DbType.String, ParameterDirection.Input, "vUserLoginId", DataRowVersion.Current, Session["vUserLoginId"].ToString().ToUpper());
                db.AddParameter(dbCommand, "QuoteNumber", DbType.String, ParameterDirection.Input, "QuoteNumber", DataRowVersion.Current, QuoteNumber);
                db.AddParameter(dbCommand, "QuoteVersion", DbType.Int16, ParameterDirection.Input, "QuoteVersion", DataRowVersion.Current, QuoteVersion);

                dbCommand.CommandType = CommandType.StoredProcedure;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetQuoteDetailsTobeModified Method");
            }
            return ds;
        }

        private void SetAllFields(DataSet dsQuoteDetails)
        {
            try
            {
                if (dsQuoteDetails != null)
                {
                    if (dsQuoteDetails.Tables.Count > 0)
                    {
                        if (dsQuoteDetails.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtQuoteDetails = dsQuoteDetails.Tables[0];
                            DataTable dtCreditScoreDetails = null;

                            if (dsQuoteDetails.Tables.Count > 1)
                            {
                                dtCreditScoreDetails = dsQuoteDetails.Tables[1];
                            }

                            if (dtQuoteDetails.Rows[0]["CustomerGender"] != null)
                            {
                                if (string.IsNullOrEmpty(dtQuoteDetails.Rows[0]["CustomerGender"].ToString()))
                                {
                                    drpCustomerGender.SelectedIndex = 0;
                                    rbtnMale.Checked = true;
                                    rbtnFemale.Checked = false;
                                }
                                else
                                {
                                    drpCustomerGender.SelectedIndex = dtQuoteDetails.Rows[0]["CustomerGender"].ToString().ToUpper() == "MALE" ? 0 : 1;
                                    rbtnMale.Checked = dtQuoteDetails.Rows[0]["CustomerGender"].ToString().ToUpper() == "MALE" ? true : false;
                                    rbtnFemale.Checked = dtQuoteDetails.Rows[0]["CustomerGender"].ToString().ToUpper() == "MALE" ? false : true; ;
                                }
                            }
                            else
                            {
                                drpCustomerGender.SelectedIndex = 0;
                                rbtnMale.Checked = true;
                                rbtnFemale.Checked = false;
                            }

                            drpProductType.SelectedValue = dtQuoteDetails.Rows[0]["IRDA_ProductCode"].ToString();
                            drpProductType.Enabled = false;

                            drpTenureOwnerDriver.SelectedValue = dtQuoteDetails.Rows[0]["TenureForOwnerDriver"].ToString();
                            drpTenureOwnerDriver.Enabled = false;

                            drpPolicyHolderType.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_TypeofPolicyHolder"].ToString();
                            drpPolicyHolderType.Enabled = false;
                            rbbtNewBusiness.Checked = dtQuoteDetails.Rows[0]["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString().ToLower() == "roll over" ? false : true;
                            rbbtRollOver.Checked = dtQuoteDetails.Rows[0]["PropGeneralProposalInformation_BusinessType_Mandatary"].ToString().ToLower() == "roll over" ? true : false;

                            rbbtNewBusiness.Enabled = false;
                            rbbtRollOver.Enabled = false;

                            SetAddOnCoversShowHide();

                            txtIntermediaryName.Text = dtQuoteDetails.Rows[0]["PropIntermediaryDetails_IntermediaryName"].ToString();
                            lblIntermediaryCode.Text = dtQuoteDetails.Rows[0]["PropIntermediaryDetails_IntermediaryCode"].ToString();
                            hfIntermediaryCode.Value = dtQuoteDetails.Rows[0]["PropIntermediaryDetails_IntermediaryCode"].ToString();
                            lblIntermediaryBusineeChannelType.Text = dtQuoteDetails.Rows[0]["PropDistributionChannel_BusineeChanneltype"].ToString();
                            hdnIntermediaryType.Value = dtQuoteDetails.Rows[0]["PropIntermediaryDetails_IntermediaryType"].ToString();

                            if (lblIntermediaryBusineeChannelType.Text.ToUpper().Trim() == "CORPORATE AGENT" || lblIntermediaryBusineeChannelType.Text.ToUpper().Trim() == "BANCASSURANCE" && rbbtRollOver.Checked)
                            {
                                tdNCBProtectCover1.Visible = true;
                                tdNCBProtectCover2.Visible = true;
                                chkNCBProtect.Visible = true;
                            }
                            else
                            {
                                tdNCBProtectCover1.Visible = false;
                                tdNCBProtectCover2.Visible = false;
                                chkNCBProtect.Checked = false;
                                chkNCBProtect.Visible = false;
                            }

                            txtIntermediaryName.Enabled = false;

                            hdnPrimaryMOCode.Value = dtQuoteDetails.Rows[0]["PropMODetails_PrimaryMOCode"].ToString();
                            hdnPrimaryMOName.Value = dtQuoteDetails.Rows[0]["PropMODetails_PrimaryMOName"].ToString();
                            hdnOfficeCode.Value = dtQuoteDetails.Rows[0]["PropGeneralProposalInformation_OfficeCode"].ToString();
                            hdnOfficeName.Value = dtQuoteDetails.Rows[0]["PropGeneralProposalInformation_OfficeName"].ToString();

                            rbctIndividual.Checked = (dtQuoteDetails.Rows[0]["PropCustomerDtls_CustomerType"].ToString().ToLower() == "individual" || dtQuoteDetails.Rows[0]["PropCustomerDtls_CustomerType"].ToString().ToLower() == "i") ? true : false;
                            rbctOrganization.Checked = (dtQuoteDetails.Rows[0]["PropCustomerDtls_CustomerType"].ToString().ToLower() == "organization" || dtQuoteDetails.Rows[0]["PropCustomerDtls_CustomerType"].ToString().ToLower() == "c") ? true : false;
                            rbctIndividual.Enabled = false;
                            rbctOrganization.Enabled = false;

                            if (rbctIndividual.Checked)
                            {
                                drpCustomerGender.Visible = true;
                                lblCustGender.Visible = true;
                                accCreditScore.Visible = true;
                            }
                            else
                            {
                                drpCustomerGender.Visible = false;
                                lblCustGender.Visible = false;
                                accCreditScore.Visible = false;
                                chkIsGetCreditScore.Checked = false;
                            }

                            txtRTOAuthorityCode.Text = dtQuoteDetails.Rows[0]["PropRisks_AuthorityLocation"].ToString();
                            hfSelectedRTO.Value = dtQuoteDetails.Rows[0]["PropRisks_AuthorityLocation"].ToString();

                            FillDrpRTOList(txtRTOAuthorityCode.Text);
                            lblRTOCluster.Text = dtQuoteDetails.Rows[0]["PropRisks_RTOCluster"].ToString();

                            FillDrpManufacturerList();

                            drpVehicleMake.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_ManufacturerCode"].ToString();

                            FillDrpModelList(drpVehicleMake.SelectedValue);

                            drpVehicleModel.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_ModelCode"].ToString();

                            FillDrpVehicleVariantList(drpVehicleMake.SelectedValue, drpVehicleModel.SelectedValue);

                            drpVehicleSubType.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_VariantCode"].ToString();

                            string[] strVST = drpVehicleSubType.SelectedItem.Text.Split('[');
                            SetFuelType(drpVehicleMake.SelectedValue, drpVehicleModel.SelectedItem.Text, strVST[0].Trim(), strVST[1].Replace("]", "").Trim());


                            lblModelCluster.Text = dtQuoteDetails.Rows[0]["PropRisks_ModelCluster"].ToString();
                            lblVehicleSegment.Text = dtQuoteDetails.Rows[0]["PropRisks_VehicleSegment"].ToString();
                            lblCubicCapacity.Text = dtQuoteDetails.Rows[0]["PropRisks_CubicCapacity"].ToString();
                            lblSeatingCapacity.Text = dtQuoteDetails.Rows[0]["PropRisks_SeatingCapacity"].ToString();
                            lblFuelType.Text = dtQuoteDetails.Rows[0]["PropRisks_FuelType"].ToString();

                            txtDateOfRegistration.Text = dtQuoteDetails.Rows[0]["PropRisks_DateofRegistration"].ToString();
                            txtMfgYear.Text = dtQuoteDetails.Rows[0]["PropRisks_ManufactureYear"].ToString();

                            drpVDA.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_VoluntaryDeductibleAmount"].ToString();

                            txtMarketMovement.Text = dtQuoteDetails.Rows[0]["PropRisks_MarketMovement"].ToString();

                            hdnMinMarketMovement.Value = dtQuoteDetails.Rows[0]["Min_MarketMovement"].ToString();
                            hdnMaxMarketMovement.Value = dtQuoteDetails.Rows[0]["Max_MarketMovement"].ToString();

                            txtPolicyStartDate.Text = dtQuoteDetails.Rows[0]["PropGeneralProposalInformation_PolicyEffectivedate"].ToString();
                            txtCustomerName.Text = dtQuoteDetails.Rows[0]["CustomerName"].ToString();

                            chkNEAR.Checked = Convert.ToInt32(dtQuoteDetails.Rows[0]["PropRisks_NonElectricalAccessories"].ToString()) > 0 ? true : false;
                            txtNeaSumInsured.Text = dtQuoteDetails.Rows[0]["PropRisks_NonElectricalAccessories"].ToString();

                            chkEAR.Checked = Convert.ToInt32(dtQuoteDetails.Rows[0]["PropRisks_ElectricalAccessories"].ToString()) > 0 ? true : false;
                            txtEaSumInsured.Text = dtQuoteDetails.Rows[0]["PropRisks_ElectricalAccessories"].ToString();

                            chkCNGLPG.Checked = Convert.ToInt32(dtQuoteDetails.Rows[0]["PropRisks_CNGLPGkitValue"].ToString()) > 0 ? true : false;
                            txtLPGKitSumInsured.Text = dtQuoteDetails.Rows[0]["PropRisks_CNGLPGkitValue"].ToString();

                            txtIDVofVehicle.Text = dtQuoteDetails.Rows[0]["FinalIDV"].ToString();
                            txtIDVofVehicle.Enabled = true;
                            hdnFinalIDV.Value = dtQuoteDetails.Rows[0]["SystemIDV"].ToString();

                            drpCoverType.SelectedValue = string.IsNullOrEmpty(dtQuoteDetails.Rows[0]["PropPreviousPolicyDetails_PreviousPolicyType"].ToString().Trim()) ? "ComprehensivePolicy" : dtQuoteDetails.Rows[0]["PropPreviousPolicyDetails_PreviousPolicyType"].ToString();

                            drpPreviousYearNCBSlab.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_PrevYearNCB"].ToString();

                            drpTotalClaimCount.SelectedValue = dtQuoteDetails.Rows[0]["PropPreviousPolicyDetails_NoOfClaims"].ToString();

                            txtTotalClaimAmount.Text = dtQuoteDetails.Rows[0]["PropPreviousPolicyDetails_TotalClaimsAmount"].ToString();

                            if (string.IsNullOrEmpty(dtQuoteDetails.Rows[0]["PropPreviousPolicyDetails_PolicyEffectiveTo"].ToString().Trim()))
                            {
                                string strPreviousPolicyExpiryDate = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                txtPreviousPolicyExpiryDate.Text = strPreviousPolicyExpiryDate;
                            }
                            else
                            {
                                txtPreviousPolicyExpiryDate.Text = dtQuoteDetails.Rows[0]["PropPreviousPolicyDetails_PolicyEffectiveTo"].ToString();
                            }

                            if (Convert.ToInt32(dtQuoteDetails.Rows[0]["PropRisks_CapitalSIPerPerson"].ToString()) > 0)
                            {
                                chkPACoverUnnamedPersons.Checked = true;
                                txtNumberOfPersonsUnnamed.Text = dtQuoteDetails.Rows[0]["PropRisks_NumberofPersonsUnnamed"].ToString();
                                drpCapitalSIPerPerson.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_CapitalSIPerPerson"].ToString();
                            }
                            /*if (Convert.ToInt32(dtQuoteDetails.Rows[0]["PropRisks_NoOfPersonsNamed"].ToString()) > 0)
                            {
                                chkPACoverNamedPersons.Checked = true;
                                txtNumberofPersonsNamed.Text = dtQuoteDetails.Rows[0]["PropRisks_NoOfPersonsNamed"].ToString();
                                drpCapitalSINamed.SelectedValue = dtQuoteDetails.Rows[0]["CapitalSIPerPerson"].ToString();
                            }*/
                            if (Convert.ToInt32(dtQuoteDetails.Rows[0]["PropRisks_NoOfPersonsPaidDriver"].ToString()) > 0)
                            {
                                chkPACoverPaidDriver.Checked = true;
                                drpNoofPaidDrivers.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_NoOfPersonsPaidDriver"].ToString();
                                drpSIPaidDriver.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_SumInsuredPaidDriver"].ToString();
                            }

                            chkWLLPD.Checked = dtQuoteDetails.Rows[0]["PropRisks_WiderLegalLiabilityToPaid"].ToString().ToLower().Trim() == "true" ? true : false;
                            txtNoofPersonsWLL.Text = dtQuoteDetails.Rows[0]["PropRisks_NoOfPersonWiderLegalLiability"].ToString();

                            chkLLEE.Checked = dtQuoteDetails.Rows[0]["PropRisks_LegalLiabilityToEmployees"].ToString().ToLower().Trim() == "true" ? true : false;
                            txtNoOfEmployees.Text = dtQuoteDetails.Rows[0]["PropRisks_NoOfPersonsLiabilityEmployee"].ToString();

                            chkReturnToInvoice.Checked = dtQuoteDetails.Rows[0]["PropRisks_ReturnToInvoice"].ToString().ToLower().Trim() == "true" ? true : false;
                            chkEngineProtect.Checked = dtQuoteDetails.Rows[0]["PropRisks_EngineSecure"].ToString().ToLower().Trim() == "true" ? true : false;
                            chkDepreciationCover.Checked = dtQuoteDetails.Rows[0]["PropRisks_DepreciationReimbursement"].ToString().ToLower().Trim() == "true" ? true : false;
                            chkRoadsideAssistance.Checked = dtQuoteDetails.Rows[0]["PropRisks_RoadsideAssistance"].ToString().ToLower().Trim() == "true" ? true : false;
                            chkConsumableCover.Checked = dtQuoteDetails.Rows[0]["PropRisks_ConsumablesExpenses"].ToString().ToLower().Trim() == "true" ? true : false;

                            chkLossofPersonalBelongings.Checked = Convert.ToString(dtQuoteDetails.Rows[0]["PropRisks_LossofPersonalBelongingschk"]).ToLower().Trim() == "true" ? true : false;
                            chkTyreCover.Checked = Convert.ToString(dtQuoteDetails.Rows[0]["PropRisks_TyreCoverChk"]).ToLower().Trim() == "true" ? true : false;
                            chkDailyCarAllowance.Checked = Convert.ToString(dtQuoteDetails.Rows[0]["PropRisks_DailyCarAllowanceChk"]).ToLower().Trim() == "true" ? true : false;

                            if (chkNCBProtect.Visible)
                            {
                                chkNCBProtect.Checked = Convert.ToString(dtQuoteDetails.Rows[0]["PropRisks_NCBProtectChk"]).ToLower().Trim() == "true" ? true : false;
                            }
                            chkKeyReplacement.Checked = Convert.ToString(dtQuoteDetails.Rows[0]["PropRisks_KeyReplacementChk"]).ToLower().Trim() == "true" ? true : false;

                            if (!string.IsNullOrEmpty(dtQuoteDetails.Rows[0]["PropRisks_LossofPersonalBelongingsSIDD"].ToString()))
                            {
                                ddlLossofPersonalBelongingsSI.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_LossofPersonalBelongingsSIDD"].ToString();
                            }

                            if (!string.IsNullOrEmpty(dtQuoteDetails.Rows[0]["PropRisks_KeyReplacementSIDD"].ToString()))
                            {
                                ddlKeyReplacement.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_KeyReplacementSIDD"].ToString();
                            }

                            if (!string.IsNullOrEmpty(dtQuoteDetails.Rows[0]["PropRisks_DailyCarAllowanceSIDD"].ToString()))
                            {
                                ddlDailyCarAllowance.SelectedValue = dtQuoteDetails.Rows[0]["PropRisks_DailyCarAllowanceSIDD"].ToString();
                            }

                            if (!string.IsNullOrEmpty(dtQuoteDetails.Rows[0]["PropRisks_TyreCoverText"].ToString()))
                            {
                                txtTyreCoverDetails.Text = dtQuoteDetails.Rows[0]["PropRisks_TyreCoverText"].ToString();
                            }

                            GetVehicleAgeNew(IsQuoteModification: true);

                            #region dtCreditScoreDetails

                            if (dtCreditScoreDetails != null)
                            {
                                if (dtCreditScoreDetails.Rows.Count > 0)
                                {
                                    chkIsGetCreditScore.Checked = true;
                                    txtFirstName.Text = dtCreditScoreDetails.Rows[0]["CustomerFirstName"].ToString();
                                    txtMiddleName.Text = dtCreditScoreDetails.Rows[0]["CustomerMiddleName"].ToString();
                                    txtLastName.Text = dtCreditScoreDetails.Rows[0]["CustomerLastName"].ToString();
                                    rbtnMale.Checked = dtCreditScoreDetails.Rows[0]["CustomerGender"].ToString().ToLower().Trim() == "male" ? true : false;
                                    rbtnFemale.Checked = dtCreditScoreDetails.Rows[0]["CustomerGender"].ToString().ToLower().Trim() == "male" ? false : true;

                                    if (rbtnMale.Checked)
                                    {
                                        drpCustomerGender.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        drpCustomerGender.SelectedIndex = 1;
                                    }

                                    drpDrivingLicenseNumberOrAadhaarNumber.SelectedValue = dtCreditScoreDetails.Rows[0]["CustomerLastName"].ToString();
                                    txtDrivingLicenseNumberOrAadhaarNumber.Text = dtCreditScoreDetails.Rows[0]["IDProofNumber"].ToString();
                                    txtMobileNumber.Text = dtCreditScoreDetails.Rows[0]["CustomerPhoneNumber"].ToString();

                                    txtCustomerDOB.Text = dtCreditScoreDetails.Rows[0]["CustomerDateofBirth"].ToString().Replace("12:00:00 AM", "").Replace(" ", "");

                                    txtPinCode.Text = dtCreditScoreDetails.Rows[0]["CustomerPincode"].ToString();
                                    hdnPinCode.Value = dtCreditScoreDetails.Rows[0]["CustomerPincode"].ToString();

                                    lblPincodeLocality.Text = dtCreditScoreDetails.Rows[0]["PinCodeLocality"].ToString();

                                    hdnCreditScoreStateId.Value = dtCreditScoreDetails.Rows[0]["CreditScore_StateCode"].ToString();
                                    hdnStateId.Value = dtCreditScoreDetails.Rows[0]["StateCode"].ToString();
                                    lblStateName.Text = dtCreditScoreDetails.Rows[0]["StateName"].ToString();

                                    hdnDistrictId.Value = dtCreditScoreDetails.Rows[0]["CityDistrictCode"].ToString();
                                    lblDistrictName.Text = dtCreditScoreDetails.Rows[0]["CityDistrictName"].ToString();

                                    hdnCityId.Value = dtCreditScoreDetails.Rows[0]["CityId"].ToString();
                                    lblCityName.Text = dtCreditScoreDetails.Rows[0]["CityName"].ToString();


                                    txtFirstName.Enabled = true;
                                    txtMiddleName.Enabled = true;
                                    txtLastName.Enabled = true;
                                    //rbtnMale.Enabled = true;
                                    //rbtnFemale.Enabled = true;
                                    drpDrivingLicenseNumberOrAadhaarNumber.Enabled = true;
                                    txtDrivingLicenseNumberOrAadhaarNumber.Enabled = true;
                                    txtMobileNumber.Enabled = true;
                                    txtCustomerDOB.Enabled = true;
                                    txtPinCode.Enabled = true;
                                }
                                else
                                {
                                    string strCustomerDOB = DateTime.Now.AddYears(-25).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    txtCustomerDOB.Text = strCustomerDOB;
                                }
                            }
                            else
                            {
                                string strCustomerDOB = DateTime.Now.AddYears(-25).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                txtCustomerDOB.Text = strCustomerDOB;
                            }

                            ResetCreditScoreFields();



                            #endregion

                            hdnQuoteNumber.Value = Request.QueryString["quotenumber"].ToString().Trim();
                            //hdnQuoteVersion.Value = Request.QueryString["quoteversion"].ToString().Trim();

                            // CR 132 Start
                            if (Session["IsUWApproval"] != null)
                            {
                                if (Convert.ToString(Session["IsUWApproval"]).ToUpper() == "FALSE" || Convert.ToString(Session["IsUWApproval"]) == "0")
                                {
                                    trDeviation.Visible = false;
                                    txtBasicODdeviation.Text = "0";
                                    txtAddOnDeviation.Text = "0";
                                }
                                else
                                {
                                    trDeviation.Visible = true;
                                    txtBasicODdeviation.Text = dtQuoteDetails.Rows[0]["PropRisks_BasicODDeviation"].ToString();
                                    txtAddOnDeviation.Text = dtQuoteDetails.Rows[0]["PropRisks_AddOnDeviation"].ToString();
                                }
                            }
                            else
                            {
                                trDeviation.Visible = false;
                                txtBasicODdeviation.Text = "0";
                                txtAddOnDeviation.Text = "0";
                            }


                            // CR 132 End


                        }
                        else
                        {
                            Alert.Show("No Data Exists for the given Quote Number and Quote version", "FrmMainMenu.aspx");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SetAllFields Method, Quote Number: " + Request.QueryString["quotenumber"].ToString().Trim());
            }
        }

        protected void txtMfgYear_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string strErrorMsg = "";
                string[] formats = { "dd/MM/yyyy" };

                DateTime temp;
                if (DateTime.TryParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out temp))
                {
                    DateTime dtCurrentDate = DateTime.Now;
                    string strYearMinus8 = dtCurrentDate.AddYears(-12).ToShortDateString();

                    DateTime dtDOR = DateTime.ParseExact(txtDateOfRegistration.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strRegistrationYear = dtDOR.ToShortDateString();
                    int RegYear = Convert.ToInt32(Convert.ToDateTime(strRegistrationYear).Year.ToString());
                    int MfgYear = Convert.ToInt32(txtMfgYear.ToString());
                    if (MfgYear > RegYear)
                    {
                        strErrorMsg = "Manufacturing year can not be greater than Registration year";
                        lblstatus.Text = "Error: " + strErrorMsg;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.Show(ex.ToString());
            }

        }

        protected void chkSelectAllCovers_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAllCovers.Checked)
            {

                chkReturnToInvoice.Checked = hdnEnabledAddOnsName.Value.Contains("RTI"); // chkReturnToInvoice.Enabled;
                chkReturnToInvoice.Enabled = false;

                chkEngineProtect.Checked = hdnEnabledAddOnsName.Value.Contains("EP"); // chkEngineProtect.Enabled;
                chkEngineProtect.Enabled = false;

                chkDepreciationCover.Checked = hdnEnabledAddOnsName.Value.Contains("DC");  // chkDepreciationCover.Enabled;
                chkDepreciationCover.Enabled = false;

                chkRoadsideAssistance.Checked = hdnEnabledAddOnsName.Value.Contains("RSA");  //chkRoadsideAssistance.Enabled;
                chkRoadsideAssistance.Enabled = false;

                chkConsumableCover.Checked = hdnEnabledAddOnsName.Value.Contains("CC");  //chkConsumableCover.Enabled;
                chkConsumableCover.Enabled = false;

                //CR164
                chkNCBProtect.Checked = hdnEnabledAddOnsName.Value.Contains("NCB") && drpProductType.SelectedValue != "1062";
                chkNCBProtect.Enabled = false;

                chkLossofPersonalBelongings.Checked = hdnEnabledAddOnsName.Value.Contains("LOPB") && drpProductType.SelectedValue != "1062";
                chkLossofPersonalBelongings.Enabled = false;
                ddlLossofPersonalBelongingsSI.Enabled = chkLossofPersonalBelongings.Checked;

                chkKeyReplacement.Checked = hdnEnabledAddOnsName.Value.Contains("KR") && drpProductType.SelectedValue != "1062";
                chkKeyReplacement.Enabled = false;
                ddlKeyReplacement.Enabled = chkKeyReplacement.Checked;

                chkDailyCarAllowance.Checked = hdnEnabledAddOnsName.Value.Contains("DAC") && drpProductType.SelectedValue != "1062";
                chkDailyCarAllowance.Enabled = false;
                ddlDailyCarAllowance.Enabled = chkDailyCarAllowance.Checked;

                chkTyreCover.Checked = hdnEnabledAddOnsName.Value.Contains("TC") && drpProductType.SelectedValue != "1062";
                chkTyreCover.Enabled = false;
                txtTyreCoverDetails.Enabled = chkTyreCover.Checked;
            }
            else
            {
                if (chkReturnToInvoice.Checked && chkReturnToInvoice.Enabled == false)
                {
                    chkReturnToInvoice.Enabled = true;
                    chkReturnToInvoice.Checked = false;
                }

                if (chkEngineProtect.Checked && chkEngineProtect.Enabled == false)
                {
                    chkEngineProtect.Enabled = true;
                    chkEngineProtect.Checked = false;
                }

                if (chkDepreciationCover.Checked && chkDepreciationCover.Enabled == false)
                {
                    chkDepreciationCover.Enabled = true;
                    chkDepreciationCover.Checked = false;
                }

                if (chkRoadsideAssistance.Checked && chkRoadsideAssistance.Enabled == false)
                {
                    chkRoadsideAssistance.Enabled = true;
                    chkRoadsideAssistance.Checked = false;
                }

                if (chkConsumableCover.Checked && chkConsumableCover.Enabled == false)
                {
                    chkConsumableCover.Enabled = true;
                    chkConsumableCover.Checked = false;
                }

                //CR164
                if (chkNCBProtect.Checked && chkNCBProtect.Enabled == false)
                {
                    chkNCBProtect.Enabled = true;
                    chkNCBProtect.Checked = false;
                }

                if (chkLossofPersonalBelongings.Checked && chkLossofPersonalBelongings.Enabled == false)
                {
                    chkLossofPersonalBelongings.Enabled = true;
                    chkLossofPersonalBelongings.Checked = false;
                    ddlLossofPersonalBelongingsSI.Enabled = false;
                }

                if (chkKeyReplacement.Checked && chkKeyReplacement.Enabled == false)
                {
                    chkKeyReplacement.Enabled = true;
                    chkKeyReplacement.Checked = false;
                    ddlKeyReplacement.Enabled = false;
                }

                if (chkDailyCarAllowance.Checked && chkDailyCarAllowance.Enabled == false)
                {
                    chkDailyCarAllowance.Enabled = true;
                    chkDailyCarAllowance.Checked = false;
                    ddlDailyCarAllowance.Enabled = false;
                }

                if (chkTyreCover.Checked && chkTyreCover.Enabled == false)
                {
                    chkTyreCover.Enabled = true;
                    chkTyreCover.Checked = false;
                    txtTyreCoverDetails.Enabled = false;
                }
            }
        }

        protected void drpPolicyHolderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpPolicyHolderType.SelectedIndex == 1) //only applicable for Individual Owner
            {
                drpCustomerGender.Visible = true;
                rbctIndividual.Checked = true;
                rbctOrganization.Checked = false;

                rbctIndividual.Enabled = false;
                rbctOrganization.Enabled = false;

                lblCustGender.Visible = true;

                chkIsGetCreditScore.Enabled = false;
                accCreditScore.Visible = true;
                chkIsGetCreditScore.Checked = true;
            }
            else
            {
                drpCustomerGender.Visible = false;
                rbctIndividual.Checked = false;
                rbctOrganization.Checked = true;

                rbctIndividual.Enabled = false;
                rbctOrganization.Enabled = false;

                chkIsGetCreditScore.Checked = false;
                chkIsGetCreditScore.Enabled = false;

                lblCustGender.Visible = false;
                accCreditScore.Visible = false;
            }
        }

        protected void rbctIndividual_CheckedChanged(object sender, EventArgs e)
        {
            if (rbctIndividual.Checked == true)
            {
                drpCustomerGender.Visible = true;
                lblCustGender.Visible = true;

                chkIsGetCreditScore.Enabled = false;
                chkIsGetCreditScore.Checked = true;
                accCreditScore.Visible = true;
            }
            else
            {
                drpCustomerGender.Visible = false;
                lblCustGender.Visible = false;
                chkIsGetCreditScore.Checked = false;
                chkIsGetCreditScore.Enabled = false;
                accCreditScore.Visible = false;
            }
        }

        protected void rbctOrganization_CheckedChanged(object sender, EventArgs e)
        {
            if (rbctIndividual.Checked == true)
            {
                drpCustomerGender.Visible = true;
                lblCustGender.Visible = true;

                chkIsGetCreditScore.Enabled = false;
                chkIsGetCreditScore.Checked = true;
                accCreditScore.Visible = true;
            }
            else
            {
                drpCustomerGender.Visible = false;
                lblCustGender.Visible = false;
                chkIsGetCreditScore.Checked = false;
                chkIsGetCreditScore.Enabled = false;
                accCreditScore.Visible = false;
            }
        }

        protected void drpCustomerGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpCustomerGender.SelectedIndex == 0)
            {
                rbtnMale.Checked = true;
                rbtnFemale.Checked = false;
                rbtnMale.Enabled = false;
                rbtnFemale.Enabled = false;
            }
            else
            {
                rbtnFemale.Checked = true;
                rbtnMale.Checked = false;
                rbtnMale.Enabled = false;
                rbtnFemale.Enabled = false;
            }
        }

        protected void chkLossofPersonalBelongings_CheckedChanged(object sender, EventArgs e)
        {
            ddlLossofPersonalBelongingsSI.Enabled = chkLossofPersonalBelongings.Checked;
        }

        protected void chkKeyReplacement_CheckedChanged(object sender, EventArgs e)
        {
            ddlKeyReplacement.Enabled = chkKeyReplacement.Checked;

            if (chkEngineProtect.Checked || chkReturnToInvoice.Checked || chkKeyReplacement.Checked || chkTyreCover.Checked)
            {
                chkDepreciationCover.Checked = hdnEnabledAddOnsName.Value.Contains("DC");
                chkDepreciationCover.Enabled = false;

                chkRoadsideAssistance.Checked = hdnEnabledAddOnsName.Value.Contains("RSA"); ;
                chkRoadsideAssistance.Enabled = false;

                chkConsumableCover.Checked = hdnEnabledAddOnsName.Value.Contains("CC"); ;
                chkConsumableCover.Enabled = false;
            }
            else
            {
                chkDepreciationCover.Checked = false;
                chkDepreciationCover.Enabled = true;

                chkRoadsideAssistance.Checked = false;
                chkRoadsideAssistance.Enabled = true;

                chkConsumableCover.Checked = false;
                chkConsumableCover.Enabled = true;
            }
        }

        protected void chkDailyCarAllowance_CheckedChanged(object sender, EventArgs e)
        {
            ddlDailyCarAllowance.Enabled = chkDailyCarAllowance.Checked;
        }

        protected void chkTyreCover_CheckedChanged(object sender, EventArgs e)
        {
            txtTyreCoverDetails.Enabled = chkTyreCover.Checked;
            txtTyreCoverDetails.Text = chkTyreCover.Checked == false ? "" : txtTyreCoverDetails.Text;

            if (chkEngineProtect.Checked || chkReturnToInvoice.Checked || chkKeyReplacement.Checked || chkTyreCover.Checked)
            {
                chkDepreciationCover.Checked = hdnEnabledAddOnsName.Value.Contains("DC");
                chkDepreciationCover.Enabled = false;

                chkRoadsideAssistance.Checked = hdnEnabledAddOnsName.Value.Contains("RSA"); ;
                chkRoadsideAssistance.Enabled = false;

                chkConsumableCover.Checked = hdnEnabledAddOnsName.Value.Contains("CC"); ;
                chkConsumableCover.Enabled = false;
            }
            else
            {
                chkDepreciationCover.Checked = false;
                chkDepreciationCover.Enabled = true;

                chkRoadsideAssistance.Checked = false;
                chkRoadsideAssistance.Enabled = true;

                chkConsumableCover.Checked = false;
                chkConsumableCover.Enabled = true;
            }
        }

        protected void rbbtNewBusiness_CheckedChanged(object sender, EventArgs e)
        {
            SetAddOnCoversShowHide();
        }

        protected void rbbtRollOver_CheckedChanged(object sender, EventArgs e)
        {
            SetAddOnCoversShowHide();
        }

        private void SetAddOnCoversShowHide()
        {
            if (rbbtNewBusiness.Checked && drpProductType.SelectedValue == "1062")
            {
                trNewAddOns.Visible = false;
                chkLossofPersonalBelongings.Checked = false;
                chkKeyReplacement.Checked = false;
                chkDailyCarAllowance.Checked = false;
                chkNCBProtect.Checked = false;
                chkTyreCover.Checked = false;
                txtTyreCoverDetails.Text = "";
                tdTyreCover1.Visible = false;
                tdTyreCover2.Visible = false;
                tdNCBProtectCover1.Visible = false;
                tdNCBProtectCover2.Visible = false;
                chkNCBProtect.Checked = false;
                chkNCBProtect.Visible = false;
            }
            else
            {
                trNewAddOns.Visible = true;
                tdTyreCover1.Visible = true;
                tdTyreCover2.Visible = true;

                if ((lblIntermediaryBusineeChannelType.Text.ToUpper().Trim() == "CORPORATE AGENT" || lblIntermediaryBusineeChannelType.Text.ToUpper().Trim() == "BANCASSURANCE") && rbbtRollOver.Checked)
                {
                    tdNCBProtectCover1.Visible = true;
                    tdNCBProtectCover2.Visible = true;
                    chkNCBProtect.Visible = true;
                }
                else
                {
                    tdNCBProtectCover1.Visible = false;
                    tdNCBProtectCover2.Visible = false;
                    chkNCBProtect.Checked = false;
                    chkNCBProtect.Visible = false;
                }
            }

            chkLossofPersonalBelongings.Enabled = true;
            chkKeyReplacement.Enabled = true;
            chkDailyCarAllowance.Enabled = true;
            chkNCBProtect.Enabled = true;
            chkTyreCover.Enabled = true;

            chkSelectAllCovers.Checked = false;
            chkReturnToInvoice.Checked = false;
            chkEngineProtect.Checked = false;
            chkDepreciationCover.Checked = false;
            chkRoadsideAssistance.Checked = false;
            chkConsumableCover.Checked = false;

            chkLossofPersonalBelongings.Checked = false;
            chkKeyReplacement.Checked = false;
            chkDailyCarAllowance.Checked = false;
            chkNCBProtect.Checked = false;
            chkTyreCover.Checked = false;
        }

        protected void drpProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAddOnCoversShowHide();
        }

    }
    public class MessageInpector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message
         reply, object correlationState)
        {
            try
            {
                string strReply = reply.ToString();
                string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "ResultXML.xml";
                File.WriteAllText(strXmlPath, String.Empty);
                File.WriteAllText(strXmlPath, strReply);
            }
            catch
            {

            }
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message
         request, System.ServiceModel.IClientChannel channel)
        {
            try
            {
                string strRequest = request.ToString();
                string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "RequestXML.xml";
                File.WriteAllText(strXmlPath, String.Empty);
                File.WriteAllText(strXmlPath, strRequest);
            }
            catch
            {

            }
            return null;
        }
    }

    class CustomBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint serviceEndpoint,
         System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint serviceEndpoint,
        System.ServiceModel.Dispatcher.ClientRuntime behavior)
        {
            //Add the inspector
            behavior.MessageInspectors.Add(new MessageInpector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint serviceEndpoint,
        System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }
        public void Validate(ServiceEndpoint serviceEndpoint)
        {
        }
    }
   


    public class RootObject
    {
        public string amount { get; set; }
        public string txnid { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string template_id { get; set; }
        public string validation_period { get; set; }
        public string send_email_now { get; set; }
        public string send_sms { get; set; }

    }
}
