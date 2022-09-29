using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class ClaimServicesStub : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*private void ClaimSaveRegistration()
        {
            try
            {

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.CUD_Registration objServiceResult = new ServiceReference1.CUD_Registration();

                objServiceResult.LOBCode = "31";
                objServiceResult.ProductCode = "3121";

                objServiceResult.NotificationNumber = "0";
                objServiceResult.OperationType = "M";

                objServiceResult.IsOrphanClm = false;
                objServiceResult.DummyPolNo = "";

                objServiceResult.IsPolicyWithoutClm = false;
                objServiceResult.IsNonSystemClm = false;

                objServiceResult.ClaimFrom = "";
                objServiceResult.ClaimNoGenMode = "PORTAL";

                objServiceResult.CLMInsuredName = "Madiha";
                objServiceResult.SAR_MLR_No = "";
                objServiceResult.CLM_Loss_Code = "";

                objServiceResult.ClaimStatusName = "Suspended";
                objServiceResult.ClaimStatusCode = "1";

                objServiceResult.ClaimCategoryName = "";
                objServiceResult.ClaimCategoryCode = "";

                objServiceResult.CatastrophyTypeName = "Local";
                objServiceResult.CatastrophyTypeCode = "1";

                objServiceResult.Customer_Claim_No = "10110001282";

                objServiceResult.CauseOfLossName = "FALLING OBJECT";
                objServiceResult.CauseOfLossCode = "0014";

                objServiceResult.AnatName = "Arm";
                objServiceResult.AnatCode = "003";

                objServiceResult.InjuryName = "Allergy";
                objServiceResult.InjuryCode = "002";

                objServiceResult.ExaminerName = "Service ID for MUMBAI(KALINA)";
                objServiceResult.ExaminerCode = "ServiceID-90002";

                objServiceResult.CLM_Date_Diff = "";
                objServiceResult.CLM_Certificate_No = "";

                objServiceResult.FeatureNo = "-";
                objServiceResult.NewPolicy = "-";

                objServiceResult.LossDescriptionCode = "Accessories of Insured vehicle stolen while parked";
                objServiceResult.OtherLossDesc = "";

                objServiceResult.AccidentLocation = "Park Street, Kolkata";
                objServiceResult.ISCatastrophySave = "Y";

                objServiceResult.LossDescriptionText = "Glass damage and dent in road accident";
                objServiceResult.RoadTypeDesc = "Hilly Roads";

                objServiceResult.RoadTypeId = "1";
                objServiceResult.IsInjuryOrDeathInvolved = "2";

                objServiceResult.IsOtherVehicleInvolved = "1";
                objServiceResult.PolicyNumber = "155001863";

                objServiceResult.IntimationNumber = "10110000000222";
                objServiceResult.ServicingOfficeCode = "90002";

                objServiceResult.NOLCode = "9948";
                objServiceResult.LossDate = "07/06/2016";

                objServiceResult.LossTime = "10.:00";
                objServiceResult.IntimationDate = "07/06/2016";

                objServiceResult.IntimationTime = "11:50";
                objServiceResult.LossLocationCode = "Park Street, Mumbai";

                objServiceResult.LocationLandmark = "Besides Big Cinemas";
                objServiceResult.LossLocationPINCode = "400033";

                objServiceResult.IsProximity = true;
                objServiceResult.CatastropheCode = "Flood";

                objServiceResult.ExpReserveAmt = 2000;
                objServiceResult.LossLocationPINDesc = "Chinchpokli";

                objServiceResult.IsChildClaim = false;
                objServiceResult.MasterClaimNumber = "10110001282";

                objServiceResult.PevClaimStatus = "";
                objServiceResult.LossReserve = "50000";

                objServiceResult.ExpReserve = "2000";
                objServiceResult.PrevLossReserve = "0";

                objServiceResult.PrevExpReserve = "0";
                objServiceResult.UserFinancial_Limit = "";

                objServiceResult.ClaimType = "OD";
               // objServiceResult.WindscreenClaimDesc = "1";

               // objServiceResult.VehicleStatusDesc = "1";
                objServiceResult.NodeName = "REGISTRATION";

                objServiceResult.TotalReserveAmount = "";
                objServiceResult.AlertMessage = "";

                objServiceResult.ClaimSerialNumber = "1";
                objServiceResult.VoucherNumber = "20160606103100000000";

                objServiceResult.VoucherDate = "06/06/2016";
               // objServiceResult.FraudAlertMsg = "Fraud Triggered";

                #region MasterClaimDtls
                ServiceReference1.MasterClaimDtls objMasterClaimDetails = new ServiceReference1.MasterClaimDtls();
                objMasterClaimDetails.ClaimNumber = "10110001282";
                objMasterClaimDetails.NotificationNumber = "10110001282";
                objServiceResult.MasterClaimDetails = objMasterClaimDetails;
                #endregion

                #region GDtRiskDetails_INR
                ServiceReference1.GDtRiskDetails_INR objIndianCurrencyRiskDetails = new ServiceReference1.GDtRiskDetails_INR();
                objIndianCurrencyRiskDetails.Num_Serial = "";
                objIndianCurrencyRiskDetails.Policy_Risk_Serial = "";
                objIndianCurrencyRiskDetails.Txt_Risk_Element_Type = "";
                objIndianCurrencyRiskDetails.Risk_Detail_Serial = "";
                objIndianCurrencyRiskDetails.Txt_Risk_Element = "VehicleBaseValue";
                objIndianCurrencyRiskDetails.Num_SI = "1500000";
                objIndianCurrencyRiskDetails.Num_Cover_Code = "";
                objIndianCurrencyRiskDetails.NUM_LOCATION_CD = "";
                objIndianCurrencyRiskDetails.Num_Claim_Amount = "50000";
                objIndianCurrencyRiskDetails.Num_Assessed_Amount = "50000";

                List<ServiceReference1.GDtRiskDetails_INR> listGDtRiskDetails_INR = new List<ServiceReference1.GDtRiskDetails_INR>();
                listGDtRiskDetails_INR.Add(objIndianCurrencyRiskDetails);
                objServiceResult.IndianCurrencyRiskDetails = listGDtRiskDetails_INR.ToArray();
                #endregion

                #region AdditionalDetails
                ServiceReference1.GDtAdditionalDtls objAdditionalDetails = new ServiceReference1.GDtAdditionalDtls();
                objAdditionalDetails.MandatoryOrNot = "Y";
                objAdditionalDetails.StorageType = "9876543210";
                objAdditionalDetails.FieldLabel = "Contact No";
                objAdditionalDetails.LengthVal = "20";
                objAdditionalDetails.DataType = "INTEGER";
                objAdditionalDetails.FieldIndex = "1";

                List<ServiceReference1.GDtAdditionalDtls> list_AdditionalDetails = new List<ServiceReference1.GDtAdditionalDtls>();
                list_AdditionalDetails.Add(objAdditionalDetails);
                objServiceResult.AdditionalDetails = list_AdditionalDetails.ToArray();
                #endregion

                ServiceReference1.ServiceClaimDetails objServiceClaimDetails = new ServiceReference1.ServiceClaimDetails();
                objServiceClaimDetails.Srv_Clm_InceptionDt = "";
                objServiceClaimDetails.Srv_Clm_ExpirationDt = "";
                objServiceClaimDetails.Srv_Clm_AccountNo = "";
                objServiceClaimDetails.Srv_Clm_ProducerName = "";
                objServiceClaimDetails.Srv_Clm_CoInsuranceLeadComp = "";
                objServiceClaimDetails.Srv_Clm_OwnPercent = "";
                objServiceClaimDetails.Srv_Clm_IssuingOfficeCode = "";
                objServiceClaimDetails.Srv_Clm_MajorLineCode = "";
                objServiceClaimDetails.ManagerCode = "";
                objServiceClaimDetails.ManagerDesc = "";
                objServiceClaimDetails.ServiceClaimStatusCode = "";
                objServiceClaimDetails.MarketSegmentCode = "";
                objServiceClaimDetails.IssueingCompCode = "";
                objServiceResult.ServiceClaimDetails = objServiceClaimDetails;

                ServiceReference1.ClaimantDetails objClaimantDetails = new ServiceReference1.ClaimantDetails();
                objClaimantDetails.Claimaint_Name = "Madiha";
                objClaimantDetails.Claimaint_IsInsured = "1";
                objClaimantDetails.Claimaint_Address1 = "Flower Ville";
                objClaimantDetails.Claimaint_Address2 = "Rose Street";
                objClaimantDetails.Claimaint_Address3 = "Park Circle";
                objClaimantDetails.Claimaint_Pincode = "400033";
                objClaimantDetails.Claimaint_District = "MUMBAI";
                objClaimantDetails.Claimaint_PinLocation = "Chinchpokli";
                objClaimantDetails.Claimaint_State = "Maharashtra";
                objClaimantDetails.Claimaint_TypeID = "1";
                objClaimantDetails.Claimaint_RelationID = "1";
                objServiceResult.ClaimantDetails = objClaimantDetails;

                ServiceReference1.DriverDetails objDriverDetails = new ServiceReference1.DriverDetails();
                objDriverDetails.Driver_Name = "Madiha";
                objDriverDetails.Driver_LicenseNo = "123456789";
                objDriverDetails.Driver_LicenseIssueDate = "31/08/2015";
                objDriverDetails.Driver_ExpiryDate = "31/12/2020";
                objDriverDetails.Driver_Age = "23";
                objDriverDetails.Driver_AddressLine1 = "Flower Ville";
                objDriverDetails.Driver_AddressLine2 = "Rose Street";
                objDriverDetails.Driver_AddressLine3 = "Park Circle";
                objDriverDetails.Driver_Pincode = "400033";
                objDriverDetails.Driver_PinLocation = "";
                objDriverDetails.Driver_District = "MUMBAI";
                objDriverDetails.Driver_State = "Maharashtra";
                objDriverDetails.Driver_DOB = "31/08/1993";
                objDriverDetails.Driver_TypeID = "1";
                objDriverDetails.Driver_TypeDesc = "Owner";
                objDriverDetails.Driver_EducationID = "";
                objDriverDetails.Driver_YearOfExp = "2";
                objDriverDetails.Driver_MonthsOfExp = "3";
                objDriverDetails.Driver_LicenceType = "2";
                objDriverDetails.Driver_IsAccompaniedPermanentLicence = "0";
                objDriverDetails.Driver_PermanentLicenceHolderName = "Khan";
                objDriverDetails.Driver_PermanentLicenceHolderNumber = "9632587410";
                objDriverDetails.Driver_DateOfAuthorizationFrom = "31/08/2015";
                objDriverDetails.Driver_DateOfAuthorizationTo = "31/12/2020";
                objDriverDetails.Driver_LicenceValid = "1";
                objDriverDetails.Driver_LicenceStatus = "1";
                objDriverDetails.Driver_OtherClassDesc = "";
                objDriverDetails.Driver_IsCopyFromPolicy = "1";
                objDriverDetails.Driver_WasVehicleParked = "0";
                objDriverDetails.Driver_OccupationCode = "";
                objDriverDetails.Driver_ClassCode = "0";
                objDriverDetails.Driver_GenderCode = "1";
                objDriverDetails.Driver_IssuingAuthority = "MUMBAI RTO";
                objServiceResult.DriverDetails = objDriverDetails;

                ServiceReference1.SummonDetails objSummonDetails = new ServiceReference1.SummonDetails();
                objSummonDetails.Intimation_Branch = "90002";
                objSummonDetails.Intimation_Mode = "1";
                objSummonDetails.Type_of_Summons = "1";
                objSummonDetails.Summon_IssueDate = "07/06/2016";
                objSummonDetails.Summon_ReceiptDate = "08/06/2016";
                objSummonDetails.Summon_AppearanceDate = "08/06/2016";
                objSummonDetails.Court_Loc_State = "20";
                objSummonDetails.Court_Loc_District = "5732";
                objSummonDetails.Court_Name = "257896";
                objSummonDetails.Case_Prefix = "1";
                objSummonDetails.Case_Number = "21335";
                objSummonDetails.Case_Year = "2016";
                objSummonDetails.Case_Title = "Motor Accident Case";
                objSummonDetails.Claim_Ser_Office = "90002";
                objSummonDetails.List_Doc_Rec = "1";
                objSummonDetails.SummonID = "";
                objServiceResult.SummonDetails = objSummonDetails;

                proxy.ClaimSaveRegistration(
                      strUserId: "GC0014"
                    , strPassword: "cmc123"
                    , WebSessionID: "b6d95661-37e4-41ff-a5d2-203841322029"
                    , SessionID: "100000000324368"
                    , TransactionID: "1"
                    , TransactionDateTime: DateTime.Now.ToString()
                    , objServiceResult: ref objServiceResult
                    );

                string ErrorMessage = objServiceResult.Message.Message.ToString();

                if (ErrorMessage.Length > 0)
                {
                    Response.Write("NotificationNumber: " + ErrorMessage);
                }
                else
                {
                    string strNotificationNumber = objServiceResult.NotificationNumber;
                    Response.Write("NotificationNumber: " + strNotificationNumber);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }*/

        private void ClaimSaveRegistrationNew()
        {
            try
            {

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.CUD_Registration objServiceResult = new ServiceReference1.CUD_Registration();

                objServiceResult.LOBCode = "31";
                objServiceResult.ProductCode = "3121";

                objServiceResult.NotificationNumber = "";
                objServiceResult.OperationType = ""; //"M";

                objServiceResult.IsOrphanClm = false;
                objServiceResult.DummyPolNo = "";

                objServiceResult.IsPolicyWithoutClm = false;
                objServiceResult.IsNonSystemClm = false;

                objServiceResult.ClaimFrom = "";
                objServiceResult.ClaimNoGenMode = "DIRECT"; //"PORTAL";

                objServiceResult.CLMInsuredName = "RAJEEV RATAN BAKHSHI";
                objServiceResult.SAR_MLR_No = "";
                objServiceResult.CLM_Loss_Code = "";

                objServiceResult.ClaimStatusName = "Suspended";
                objServiceResult.ClaimStatusCode = "1";

                objServiceResult.ClaimCategoryName = "";
                objServiceResult.ClaimCategoryCode = "";

                objServiceResult.CatastrophyTypeName = "";
                objServiceResult.CatastrophyTypeCode = "";

                objServiceResult.Customer_Claim_No = "";

                //objServiceResult.CauseOfLossName = "FALLING OBJECT";
                //objServiceResult.CauseOfLossCode = "0014";

                //objServiceResult.AnatName = "Arm";
                //objServiceResult.AnatCode = "003";

                //objServiceResult.InjuryName = "Allergy";
                //objServiceResult.InjuryCode = "002";

                objServiceResult.ExaminerName = "Service ID for MUMBAI(KALINA)";
                objServiceResult.ExaminerCode = "ServiceID-90002";

                objServiceResult.CLM_Date_Diff = "0";
                objServiceResult.CLM_Certificate_No = "";

                objServiceResult.FeatureNo = "01";
                objServiceResult.NewPolicy = "NEW";

                objServiceResult.LossDescriptionCode = "2"; //Accessories of Insured vehicle stolen while parked
                objServiceResult.OtherLossDesc = "Accessories of Insured vehicle stolen while parked";

                objServiceResult.AccidentLocation = "NEW PRABHADEVI ROAD";
                objServiceResult.ISCatastrophySave = "N";

                objServiceResult.LossDescriptionText = "Glass damage and dent in road accident";

                objServiceResult.RoadTypeDesc = "City";
                objServiceResult.RoadTypeId = "5";

                objServiceResult.IsInjuryOrDeathInvolved = "2";

                objServiceResult.IsOtherVehicleInvolved = "2";
                objServiceResult.PolicyNumber = "1000318100"; //"1000318100"; //1000322700

                objServiceResult.IntimationNumber = "";// "0";
                objServiceResult.ServicingOfficeCode = "90002";

                objServiceResult.NOLCode = "24971"; //cover code = nol code

                objServiceResult.LossDate = "24/11/2016";
                objServiceResult.LossTime = "07:00";

                objServiceResult.IntimationDate = "24/11/2016";
                objServiceResult.IntimationTime = "09:00";

                objServiceResult.LossLocationCode = "10000002871"; //"Park Street, Mumbai";

                objServiceResult.LocationLandmark = "Besides Big Cinemas";
                objServiceResult.LossLocationPINCode = "400025";

                objServiceResult.IsProximity = false;
                //objServiceResult.CatastropheCode = "Flood";

                objServiceResult.ExpReserveAmt = 0;
                objServiceResult.LossLocationPINDesc = "NEW PRABHADEVI ROAD";

                //objServiceResult.IsChildClaim = false;
                //objServiceResult.MasterClaimNumber = "";

                objServiceResult.PevClaimStatus = "";
                objServiceResult.LossReserve = "23000";

                objServiceResult.ExpReserve = "0";
                objServiceResult.PrevLossReserve = "0";

                objServiceResult.PrevExpReserve = "0";
                objServiceResult.UserFinancial_Limit = "";

                objServiceResult.ClaimType = "OD";
                // objServiceResult.WindscreenClaimDesc = "1";

                // objServiceResult.VehicleStatusDesc = "1";
                objServiceResult.NodeName = "REGISTRATION";

                objServiceResult.TotalReserveAmount = "23000"; //LossReserve + ExpReserve = TotalReserveAmount
                objServiceResult.AlertMessage = "";

                objServiceResult.ClaimSerialNumber = "1";
                // objServiceResult.VoucherNumber = "20160606103100000000";
                // objServiceResult.VoucherDate = "20/11/2016";
                // objServiceResult.FraudAlertMsg = "Fraud Triggered";

                #region MasterClaimDtls
                ServiceReference1.MasterClaimDtls objMasterClaimDetails = new ServiceReference1.MasterClaimDtls();
                objMasterClaimDetails.ClaimNumber = "";
                objMasterClaimDetails.NotificationNumber = "";
                objServiceResult.MasterClaimDetails = objMasterClaimDetails;
                #endregion

                #region GDtRiskDetails_INR
                ServiceReference1.GDtRiskDetails_INR objIndianCurrencyRiskDetails = new ServiceReference1.GDtRiskDetails_INR();
                objIndianCurrencyRiskDetails.Num_Serial = "";
                objIndianCurrencyRiskDetails.Policy_Risk_Serial = "1";
                objIndianCurrencyRiskDetails.Txt_Risk_Element_Type = "N093891G0072";
                objIndianCurrencyRiskDetails.Risk_Detail_Serial = "0";
                objIndianCurrencyRiskDetails.Txt_Risk_Element = "Own Damage"; //Own Damage
                objIndianCurrencyRiskDetails.Num_SI = "448050.00";
                objIndianCurrencyRiskDetails.Num_Cover_Code = "24971";
                objIndianCurrencyRiskDetails.NUM_LOCATION_CD = "10000002871";
                objIndianCurrencyRiskDetails.Num_Claim_Amount = "23000";
                objIndianCurrencyRiskDetails.Num_Assessed_Amount = "0"; //"23000";

                List<ServiceReference1.GDtRiskDetails_INR> listGDtRiskDetails_INR = new List<ServiceReference1.GDtRiskDetails_INR>();
                listGDtRiskDetails_INR.Add(objIndianCurrencyRiskDetails);
                objServiceResult.IndianCurrencyRiskDetails = listGDtRiskDetails_INR.ToArray();
                #endregion

                #region AdditionalDetails
                //ServiceReference1.GDtAdditionalDtls objAdditionalDetails = new ServiceReference1.GDtAdditionalDtls();
                //objAdditionalDetails.MandatoryOrNot = ""; //"Y";
                //objAdditionalDetails.StorageType = ""; //"9876543210";
                //objAdditionalDetails.FieldLabel = ""; //"Contact No";
                //objAdditionalDetails.LengthVal = ""; //"20";
                //objAdditionalDetails.DataType = ""; //"INTEGER";
                //objAdditionalDetails.FieldIndex = ""; //"1";

                //List<ServiceReference1.GDtAdditionalDtls> list_AdditionalDetails = new List<ServiceReference1.GDtAdditionalDtls>();
                //list_AdditionalDetails.Add(objAdditionalDetails);
                //objServiceResult.AdditionalDetails = list_AdditionalDetails.ToArray();
                #endregion

                ServiceReference1.ServiceClaimDetails objServiceClaimDetails = new ServiceReference1.ServiceClaimDetails();
                objServiceClaimDetails.Srv_Clm_InceptionDt = "";
                objServiceClaimDetails.Srv_Clm_ExpirationDt = "";
                objServiceClaimDetails.Srv_Clm_AccountNo = "";
                objServiceClaimDetails.Srv_Clm_ProducerName = "";
                objServiceClaimDetails.Srv_Clm_CoInsuranceLeadComp = "";
                objServiceClaimDetails.Srv_Clm_OwnPercent = "";
                objServiceClaimDetails.Srv_Clm_IssuingOfficeCode = "";
                objServiceClaimDetails.Srv_Clm_MajorLineCode = "";
                objServiceClaimDetails.ManagerCode = "";
                objServiceClaimDetails.ManagerDesc = "";
                objServiceClaimDetails.ServiceClaimStatusCode = "";
                objServiceClaimDetails.MarketSegmentCode = "";
                objServiceClaimDetails.IssueingCompCode = "";
                objServiceResult.ServiceClaimDetails = objServiceClaimDetails;

                ServiceReference1.ClaimantDetails objClaimantDetails = new ServiceReference1.ClaimantDetails();
                objClaimantDetails.Claimaint_Name = "RAJEEV RATAN BAKHSHI";
                objClaimantDetails.Claimaint_IsInsured = "1";
                objClaimantDetails.Claimaint_Address1 = "NERUL";
                objClaimantDetails.Claimaint_Address2 = "";
                objClaimantDetails.Claimaint_Address3 = "";
                objClaimantDetails.Claimaint_Pincode = "400706";
                objClaimantDetails.Claimaint_District = "THANE";
                objClaimantDetails.Claimaint_PinLocation = "DARAVE";
                objClaimantDetails.Claimaint_State = "Maharashtra";
                objClaimantDetails.Claimaint_TypeID = "1";
                objClaimantDetails.Claimaint_RelationID = "1";
                objServiceResult.ClaimantDetails = objClaimantDetails;

                /*ServiceReference1.DriverDetails objDriverDetails = new ServiceReference1.DriverDetails();
                objDriverDetails.Driver_Name = "Madiha";
                objDriverDetails.Driver_LicenseNo = "123456789";
                objDriverDetails.Driver_LicenseIssueDate = "31/08/2015";
                objDriverDetails.Driver_ExpiryDate = "31/12/2020";
                objDriverDetails.Driver_Age = "23";
                objDriverDetails.Driver_AddressLine1 = "Flower Ville";
                objDriverDetails.Driver_AddressLine2 = "Rose Street";
                objDriverDetails.Driver_AddressLine3 = "Park Circle";
                objDriverDetails.Driver_Pincode = "400033";
                objDriverDetails.Driver_PinLocation = "";
                objDriverDetails.Driver_District = "MUMBAI";
                objDriverDetails.Driver_State = "Maharashtra";
                objDriverDetails.Driver_DOB = "31/08/1993";
                objDriverDetails.Driver_TypeID = "1";
                objDriverDetails.Driver_TypeDesc = "Owner";
                objDriverDetails.Driver_EducationID = "";
                objDriverDetails.Driver_YearOfExp = "2";
                objDriverDetails.Driver_MonthsOfExp = "3";
                objDriverDetails.Driver_LicenceType = "2";
                objDriverDetails.Driver_IsAccompaniedPermanentLicence = "0";
                objDriverDetails.Driver_PermanentLicenceHolderName = "Khan";
                objDriverDetails.Driver_PermanentLicenceHolderNumber = "9632587410";
                objDriverDetails.Driver_DateOfAuthorizationFrom = "31/08/2015";
                objDriverDetails.Driver_DateOfAuthorizationTo = "31/12/2020";
                objDriverDetails.Driver_LicenceValid = "1";
                objDriverDetails.Driver_LicenceStatus = "1";
                objDriverDetails.Driver_OtherClassDesc = "";
                objDriverDetails.Driver_IsCopyFromPolicy = "1";
                objDriverDetails.Driver_WasVehicleParked = "0";
                objDriverDetails.Driver_OccupationCode = "";
                objDriverDetails.Driver_ClassCode = "0";
                objDriverDetails.Driver_GenderCode = "1";
                objDriverDetails.Driver_IssuingAuthority = "MUMBAI RTO";
                objServiceResult.DriverDetails = objDriverDetails;*/

                //ServiceReference1.SummonDetails objSummonDetails = new ServiceReference1.SummonDetails();
                //objSummonDetails.Intimation_Branch = "90002";
                //objSummonDetails.Intimation_Mode = "1";
                //objSummonDetails.Type_of_Summons = "1";
                //objSummonDetails.Summon_IssueDate = "07/06/2016";
                //objSummonDetails.Summon_ReceiptDate = "08/06/2016";
                //objSummonDetails.Summon_AppearanceDate = "08/06/2016";
                //objSummonDetails.Court_Loc_State = "20";
                //objSummonDetails.Court_Loc_District = "5732";
                //objSummonDetails.Court_Name = "257896";
                //objSummonDetails.Case_Prefix = "1";
                //objSummonDetails.Case_Number = "21335";
                //objSummonDetails.Case_Year = "2016";
                //objSummonDetails.Case_Title = "Motor Accident Case";
                //objSummonDetails.Claim_Ser_Office = "90002";
                //objSummonDetails.List_Doc_Rec = "1";
                //objSummonDetails.SummonID = "";
                //objServiceResult.SummonDetails = objSummonDetails;

                proxy.ClaimSaveRegistration(
                      strUserId: "GC0014"
                    , strPassword: "cmc123"
                    , WebSessionID: "b6d95661-37e4-41ff-a5d2-203841322029"
                    , SessionID: "100000000324368"
                    , TransactionID: "1"
                    , TransactionDateTime: "24/11/2016" //DateTime.Now.ToString()
                    , objServiceResult: ref objServiceResult
                    );

                proxy.Close();

                string ErrorMessage = objServiceResult.Message.Message.ToString();

                if (ErrorMessage.Length > 0)
                {
                    Response.Write("NotificationNumber: " + ErrorMessage);
                }
                else
                {
                    string strNotificationNumber = objServiceResult.NotificationNumber;
                    Response.Write("NotificationNumber: " + strNotificationNumber);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void ClaimSaveRegistrationNewsUSHMA()
        {
            try
            {

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.CUD_Registration objServiceResult = new ServiceReference1.CUD_Registration();

                objServiceResult.LOBCode = "31";
                objServiceResult.ProductCode = "3121";

                objServiceResult.NotificationNumber = "";
                objServiceResult.OperationType = ""; //"M";

                objServiceResult.IsOrphanClm = false;
                objServiceResult.DummyPolNo = "";

                objServiceResult.IsPolicyWithoutClm = false;
                objServiceResult.IsNonSystemClm = false;

                objServiceResult.ClaimFrom = "";
                objServiceResult.ClaimNoGenMode = "DIRECT"; //"PORTAL";

                objServiceResult.CLMInsuredName = "abc";
                objServiceResult.SAR_MLR_No = "";
                objServiceResult.CLM_Loss_Code = "";

                objServiceResult.ClaimStatusName = "Suspended";
                objServiceResult.ClaimStatusCode = "1";

                objServiceResult.ClaimCategoryName = "";
                objServiceResult.ClaimCategoryCode = "";

                objServiceResult.CatastrophyTypeName = "";
                objServiceResult.CatastrophyTypeCode = "";

                objServiceResult.Customer_Claim_No = "";

                //objServiceResult.CauseOfLossName = "FALLING OBJECT";
                //objServiceResult.CauseOfLossCode = "0014";

                //objServiceResult.AnatName = "Arm";
                //objServiceResult.AnatCode = "003";

                //objServiceResult.InjuryName = "Allergy";
                //objServiceResult.InjuryCode = "002";

                objServiceResult.ExaminerName = "Service ID for MUMBAI(KALINA)";
                objServiceResult.ExaminerCode = "ServiceID-90002";

                objServiceResult.CLM_Date_Diff = "0";
                objServiceResult.CLM_Certificate_No = "";

                objServiceResult.FeatureNo = "01";
                objServiceResult.NewPolicy = "NEW";

                objServiceResult.LossDescriptionCode = "2"; //Accessories of Insured vehicle stolen while parked
                objServiceResult.OtherLossDesc = "Accessories of Insured vehicle stolen while parked";

                objServiceResult.AccidentLocation = "abc";
                objServiceResult.ISCatastrophySave = "N";

                objServiceResult.LossDescriptionText = "Glass damage and dent in road accident";

                objServiceResult.RoadTypeDesc = "City";
                objServiceResult.RoadTypeId = "5";

                objServiceResult.IsInjuryOrDeathInvolved = "2";

                objServiceResult.IsOtherVehicleInvolved = "2";
                objServiceResult.PolicyNumber = "1000318100"; //"1000318100"; //1000322700

                objServiceResult.IntimationNumber = "";// "0";
                objServiceResult.ServicingOfficeCode = "90002";

                objServiceResult.NOLCode = "24971"; //cover code = nol code

                objServiceResult.LossDate = "24/11/2016";
                objServiceResult.LossTime = "07:00";

                objServiceResult.IntimationDate = "19/01/2017";
                objServiceResult.IntimationTime = "09:00";

                objServiceResult.LossLocationCode = "10000002871"; //"Park Street, Mumbai";

                objServiceResult.LocationLandmark = "Besides Big Cinemas";
                objServiceResult.LossLocationPINCode = "400025";

                objServiceResult.IsProximity = false;
                //objServiceResult.CatastropheCode = "Flood";

                objServiceResult.ExpReserveAmt = 0;
                objServiceResult.LossLocationPINDesc = "NEW PRABHADEVI ROAD";

                //objServiceResult.IsChildClaim = false;
                //objServiceResult.MasterClaimNumber = "";

                objServiceResult.PevClaimStatus = "";
                objServiceResult.LossReserve = "23000";

                objServiceResult.ExpReserve = "0";
                objServiceResult.PrevLossReserve = "0";

                objServiceResult.PrevExpReserve = "0";
                objServiceResult.UserFinancial_Limit = "";

                objServiceResult.ClaimType = "OD";
                // objServiceResult.WindscreenClaimDesc = "1";

                // objServiceResult.VehicleStatusDesc = "1";
                objServiceResult.NodeName = "REGISTRATION";

                objServiceResult.TotalReserveAmount = "23000"; //LossReserve + ExpReserve = TotalReserveAmount
                objServiceResult.AlertMessage = "";

                objServiceResult.ClaimSerialNumber = "1";
                // objServiceResult.VoucherNumber = "20160606103100000000";
                // objServiceResult.VoucherDate = "20/11/2016";
                // objServiceResult.FraudAlertMsg = "Fraud Triggered";

                #region MasterClaimDtls
                ServiceReference1.MasterClaimDtls objMasterClaimDetails = new ServiceReference1.MasterClaimDtls();
                objMasterClaimDetails.ClaimNumber = "";
                objMasterClaimDetails.NotificationNumber = "";
                objServiceResult.MasterClaimDetails = objMasterClaimDetails;
                #endregion

                #region GDtRiskDetails_INR
                ServiceReference1.GDtRiskDetails_INR objIndianCurrencyRiskDetails = new ServiceReference1.GDtRiskDetails_INR();
                objIndianCurrencyRiskDetails.Num_Serial = "";
                objIndianCurrencyRiskDetails.Policy_Risk_Serial = "1";
                objIndianCurrencyRiskDetails.Txt_Risk_Element_Type = "N093891G0072";
                objIndianCurrencyRiskDetails.Risk_Detail_Serial = "0";
                objIndianCurrencyRiskDetails.Txt_Risk_Element = "Own Damage"; //Own Damage
                objIndianCurrencyRiskDetails.Num_SI = "448050.00";
                objIndianCurrencyRiskDetails.Num_Cover_Code = "24971";
                objIndianCurrencyRiskDetails.NUM_LOCATION_CD = "10000002871";
                objIndianCurrencyRiskDetails.Num_Claim_Amount = "23000";
                objIndianCurrencyRiskDetails.Num_Assessed_Amount = "0"; //"23000";

                List<ServiceReference1.GDtRiskDetails_INR> listGDtRiskDetails_INR = new List<ServiceReference1.GDtRiskDetails_INR>();
                listGDtRiskDetails_INR.Add(objIndianCurrencyRiskDetails);
                objServiceResult.IndianCurrencyRiskDetails = listGDtRiskDetails_INR.ToArray();
                #endregion

                #region AdditionalDetails
                //ServiceReference1.GDtAdditionalDtls objAdditionalDetails = new ServiceReference1.GDtAdditionalDtls();
                //objAdditionalDetails.MandatoryOrNot = ""; //"Y";
                //objAdditionalDetails.StorageType = ""; //"9876543210";
                //objAdditionalDetails.FieldLabel = ""; //"Contact No";
                //objAdditionalDetails.LengthVal = ""; //"20";
                //objAdditionalDetails.DataType = ""; //"INTEGER";
                //objAdditionalDetails.FieldIndex = ""; //"1";

                //List<ServiceReference1.GDtAdditionalDtls> list_AdditionalDetails = new List<ServiceReference1.GDtAdditionalDtls>();
                //list_AdditionalDetails.Add(objAdditionalDetails);
                //objServiceResult.AdditionalDetails = list_AdditionalDetails.ToArray();
                #endregion

                ServiceReference1.ServiceClaimDetails objServiceClaimDetails = new ServiceReference1.ServiceClaimDetails();
                objServiceClaimDetails.Srv_Clm_InceptionDt = "";
                objServiceClaimDetails.Srv_Clm_ExpirationDt = "";
                objServiceClaimDetails.Srv_Clm_AccountNo = "";
                objServiceClaimDetails.Srv_Clm_ProducerName = "";
                objServiceClaimDetails.Srv_Clm_CoInsuranceLeadComp = "";
                objServiceClaimDetails.Srv_Clm_OwnPercent = "";
                objServiceClaimDetails.Srv_Clm_IssuingOfficeCode = "";
                objServiceClaimDetails.Srv_Clm_MajorLineCode = "";
                objServiceClaimDetails.ManagerCode = "";
                objServiceClaimDetails.ManagerDesc = "";
                objServiceClaimDetails.ServiceClaimStatusCode = "";
                objServiceClaimDetails.MarketSegmentCode = "";
                objServiceClaimDetails.IssueingCompCode = "";
                objServiceResult.ServiceClaimDetails = objServiceClaimDetails;

                ServiceReference1.ClaimantDetails objClaimantDetails = new ServiceReference1.ClaimantDetails();
                objClaimantDetails.Claimaint_Name = "RAJEEV RATAN BAKHSHI";
                objClaimantDetails.Claimaint_IsInsured = "1";
                objClaimantDetails.Claimaint_Address1 = "NERUL";
                objClaimantDetails.Claimaint_Address2 = "";
                objClaimantDetails.Claimaint_Address3 = "";
                objClaimantDetails.Claimaint_Pincode = "400706";
                objClaimantDetails.Claimaint_District = "THANE";
                objClaimantDetails.Claimaint_PinLocation = "DARAVE";
                objClaimantDetails.Claimaint_State = "Maharashtra";
                objClaimantDetails.Claimaint_TypeID = "1";
                objClaimantDetails.Claimaint_RelationID = "1";
                objServiceResult.ClaimantDetails = objClaimantDetails;

                /*ServiceReference1.DriverDetails objDriverDetails = new ServiceReference1.DriverDetails();
                objDriverDetails.Driver_Name = "Madiha";
                objDriverDetails.Driver_LicenseNo = "123456789";
                objDriverDetails.Driver_LicenseIssueDate = "31/08/2015";
                objDriverDetails.Driver_ExpiryDate = "31/12/2020";
                objDriverDetails.Driver_Age = "23";
                objDriverDetails.Driver_AddressLine1 = "Flower Ville";
                objDriverDetails.Driver_AddressLine2 = "Rose Street";
                objDriverDetails.Driver_AddressLine3 = "Park Circle";
                objDriverDetails.Driver_Pincode = "400033";
                objDriverDetails.Driver_PinLocation = "";
                objDriverDetails.Driver_District = "MUMBAI";
                objDriverDetails.Driver_State = "Maharashtra";
                objDriverDetails.Driver_DOB = "31/08/1993";
                objDriverDetails.Driver_TypeID = "1";
                objDriverDetails.Driver_TypeDesc = "Owner";
                objDriverDetails.Driver_EducationID = "";
                objDriverDetails.Driver_YearOfExp = "2";
                objDriverDetails.Driver_MonthsOfExp = "3";
                objDriverDetails.Driver_LicenceType = "2";
                objDriverDetails.Driver_IsAccompaniedPermanentLicence = "0";
                objDriverDetails.Driver_PermanentLicenceHolderName = "Khan";
                objDriverDetails.Driver_PermanentLicenceHolderNumber = "9632587410";
                objDriverDetails.Driver_DateOfAuthorizationFrom = "31/08/2015";
                objDriverDetails.Driver_DateOfAuthorizationTo = "31/12/2020";
                objDriverDetails.Driver_LicenceValid = "1";
                objDriverDetails.Driver_LicenceStatus = "1";
                objDriverDetails.Driver_OtherClassDesc = "";
                objDriverDetails.Driver_IsCopyFromPolicy = "1";
                objDriverDetails.Driver_WasVehicleParked = "0";
                objDriverDetails.Driver_OccupationCode = "";
                objDriverDetails.Driver_ClassCode = "0";
                objDriverDetails.Driver_GenderCode = "1";
                objDriverDetails.Driver_IssuingAuthority = "MUMBAI RTO";
                objServiceResult.DriverDetails = objDriverDetails;*/

                //ServiceReference1.SummonDetails objSummonDetails = new ServiceReference1.SummonDetails();
                //objSummonDetails.Intimation_Branch = "90002";
                //objSummonDetails.Intimation_Mode = "1";
                //objSummonDetails.Type_of_Summons = "1";
                //objSummonDetails.Summon_IssueDate = "07/06/2016";
                //objSummonDetails.Summon_ReceiptDate = "08/06/2016";
                //objSummonDetails.Summon_AppearanceDate = "08/06/2016";
                //objSummonDetails.Court_Loc_State = "20";
                //objSummonDetails.Court_Loc_District = "5732";
                //objSummonDetails.Court_Name = "257896";
                //objSummonDetails.Case_Prefix = "1";
                //objSummonDetails.Case_Number = "21335";
                //objSummonDetails.Case_Year = "2016";
                //objSummonDetails.Case_Title = "Motor Accident Case";
                //objSummonDetails.Claim_Ser_Office = "90002";
                //objSummonDetails.List_Doc_Rec = "1";
                //objSummonDetails.SummonID = "";
                //objServiceResult.SummonDetails = objSummonDetails;

                proxy.ClaimSaveRegistration(
                      strUserId: "GC0014"
                    , strPassword: "cmc123"
                    , WebSessionID: "b6d95661-37e4-41ff-a5d2-203841322029"
                    , SessionID: "100000000324368"
                    , TransactionID: "1"
                    , TransactionDateTime: "24/11/2016" //DateTime.Now.ToString()
                    , objServiceResult: ref objServiceResult
                    );

                proxy.Close();

                string ErrorMessage = objServiceResult.Message.Message.ToString();

                if (ErrorMessage.Length > 0)
                {
                    Response.Write("NotificationNumber: " + ErrorMessage);
                }
                else
                {
                    string strNotificationNumber = objServiceResult.NotificationNumber;
                    Response.Write("NotificationNumber: " + strNotificationNumber);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void ClaimServiceUpdateDriverDetails()
        {
            try
            {

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());
                ServiceReference1.DriverDetails objDriverDetails = new ServiceReference1.DriverDetails();
                objDriverDetails.Driver_Name = "KOTAK";
                objDriverDetails.Driver_LicenseNo = "123456789";
                objDriverDetails.Driver_LicenseIssueDate = "11/11/2015";
                objDriverDetails.Driver_ExpiryDate = "10/11/2035";
                objDriverDetails.Driver_Age = "23";
                objDriverDetails.Driver_AddressLine1 = ""; //"Flower Ville";
                objDriverDetails.Driver_AddressLine2 = ""; // "Rose Street";
                objDriverDetails.Driver_AddressLine3 = ""; // "Park Circle";
                objDriverDetails.Driver_Pincode = "400097";
                //objDriverDetails.Driver_PinLocation = "";
                objDriverDetails.Driver_District = "MUMBAI";
                objDriverDetails.Driver_State = "Maharashtra";
                objDriverDetails.Driver_DOB = "31/08/1993";
                objDriverDetails.Driver_TypeID = "1";
                objDriverDetails.Driver_TypeDesc = "Owner";
                objDriverDetails.Driver_EducationID = "2";
                objDriverDetails.Driver_YearOfExp = "11";
                objDriverDetails.Driver_MonthsOfExp = "11";
                objDriverDetails.Driver_LicenceType = "0";
                objDriverDetails.Driver_IsAccompaniedPermanentLicence = "0";
                objDriverDetails.Driver_PermanentLicenceHolderName = "";
                objDriverDetails.Driver_PermanentLicenceHolderNumber = "";
                objDriverDetails.Driver_DateOfAuthorizationFrom = "";
                objDriverDetails.Driver_DateOfAuthorizationTo = "";
                objDriverDetails.Driver_LicenceValid = "0";
                objDriverDetails.Driver_LicenceStatus = "0";
                //objDriverDetails.Driver_OtherClassDesc = "";
                //objDriverDetails.Driver_IsCopyFromPolicy = "1";
                //objDriverDetails.Driver_WasVehicleParked = "0";
                //objDriverDetails.Driver_OccupationCode = "";
                objDriverDetails.Driver_ClassCode = "4";
                objDriverDetails.Driver_GenderCode = "2";
                objDriverDetails.Driver_IssuingAuthority = "MUMBAI RTO";

                //ServiceReference1.DDtDropDownList objDDtDropDownList = new ServiceReference1.DDtDropDownList();
                //objDDtDropDownList.RowIndex = 0;
                //objDDtDropDownList.ItemCode = "";
                //objDDtDropDownList.ItemDesc = "";
                //List<ServiceReference1.DDtDropDownList> list_DDtDropDownList = new List<ServiceReference1.DDtDropDownList>();
                //list_DDtDropDownList.Add(objDDtDropDownList);
                //objDriverDetails.Driver_AccompaniedPermanentLicenceTypeList = list_DDtDropDownList.ToArray();



                objDriverDetails.DrvBadgeNo = "";
                objDriverDetails.DrvBadgeDateOfIssue = "";
                objDriverDetails.Badge_AddressLine1 = "";
                objDriverDetails.Badge_AddressLine2 = "";
                objDriverDetails.Badge_AddressLine3 = "";
                objDriverDetails.Badge_Pincode = "";
                objDriverDetails.Badge_District = "";
                objDriverDetails.Badge_State = "";
                objDriverDetails.DrvDriverDeathInjuryDetails = "";

                objDriverDetails.Driver_OccupationCode = "1";
                objDriverDetails.Driver_WasVehicleParked = "False";
                //, SessionID: "100000000324368"
                //   , TransactionID: "1"

                string status = proxy.ClaimServiceUpdateDriverDetails(
                      strUserId: "GC0014"
                    , strPassword: "cmc123"
                    , ClaimNo: "10410000179"
                    , TransId: "169859"
                    , SessionId: "201612021017"
                    , ObjDriverDetails: ref objDriverDetails
                    );

                proxy.Close();


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void ClaimServiceSaveNote()
        {
            try
            {

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();

                ServiceReference1.CUD_Note objCUDNote = new ServiceReference1.CUD_Note();

                ServiceReference1.GDt_ClaimNotes objGDtClaimNotes = new ServiceReference1.GDt_ClaimNotes();
                objGDtClaimNotes.Notes_Remarks = "NA";
                objGDtClaimNotes.Notes_UserID = "GC0011";
                objGDtClaimNotes.Notes_ClaimNo = "10410000179";
                objGDtClaimNotes.Notes_DateTime = "02/12/2016";

                objCUDNote.PolicyNumber = "1000112200";

                List<ServiceReference1.GDt_ClaimNotes> list_GDt_ClaimNotes = new List<ServiceReference1.GDt_ClaimNotes>();
                list_GDt_ClaimNotes.Add(objGDtClaimNotes);
                objCUDNote.GDtClaimNotesList = list_GDt_ClaimNotes.ToArray();




                string status = proxy.ClaimServiceSaveNote(
                      strUserId: "GC0014"
                    , strPassword: "cmc123"
                    , ClaimNo: "10410000179"
                    , ObjClaimNote: ref objCUDNote
                    );

                proxy.Close();


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void ClaimSaveAssessmentSheet()
        {
            try
            {

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                ServiceReference1.ClaimServiceResult_SurveyorDataEntry objClaimServiceResult_SurveyorDataEntry = new ServiceReference1.ClaimServiceResult_SurveyorDataEntry();
                ServiceReference1.CUD_SurveroyDataEntry objCUD_SurveroyDataEntry = new ServiceReference1.CUD_SurveroyDataEntry();

                objCUD_SurveroyDataEntry.NotificationNumber = "10410000179";
                objCUD_SurveroyDataEntry.MotorClaimType = "OD"; //"OD/ TP / THEFT";
                objCUD_SurveroyDataEntry.LOBCode = "31"; //"31(for MOTOR)";
                objCUD_SurveroyDataEntry.ClaimType = "OD"; // OD/ TP / PA
                objCUD_SurveroyDataEntry.VehicalRegDate = "01/01/2016";
                objCUD_SurveroyDataEntry.NOLType = "Claim Nature of Loss";
                objCUD_SurveroyDataEntry.PageSpecificCommonVariable = "1"; // Repair Basis/ Salvage Loss / Total Loss / Cash Loss(1 / 2 / 3 / 4)
                objCUD_SurveroyDataEntry.NodeName = "MOTOR CLAIM PROCESS & SETTLEMENT"; //  MOTOR CLAIM PROCESS &SETTLEMENT(Activity / Page Name)
                objCUD_SurveroyDataEntry.AppUserID = "GC123"; // GC123 (User ID)
                objCUD_SurveroyDataEntry.SessionID = "201612021017"; // Session ID
                objCUD_SurveroyDataEntry.TransactionID = "169859"; // Transaction ID
                objCUD_SurveroyDataEntry.TransactionDateTime = "02/12/2016"; // Transaction Date(System Date)
                objClaimServiceResult_SurveyorDataEntry.ClaimData = objCUD_SurveroyDataEntry;

                ServiceReference1.ClaimComputationSummary objClaimComputationSummary = new ServiceReference1.ClaimComputationSummary();
                ServiceReference1.ClaimComputationSheetClassLibrary objClaimComputationSheetClassLibrary = new ServiceReference1.ClaimComputationSheetClassLibrary();


                objClaimComputationSummary.MarketValueIDV = "300000";
                objClaimComputationSummary.GrossPartAmountPaintMaterial = "6512.5";
                objClaimComputationSummary.GrossPartAmountMaterial = "900";
                objClaimComputationSummary.NetCostOfPart = "900";
                objClaimComputationSummary.NetCostOfPaintMaterial = "6512.5";
                objClaimComputationSummary.NetCostOfLabour = "41075";
                objClaimComputationSummary.LabourServiceTax = "6161.25";
                objClaimComputationSummary.WctRate = "0";
                objClaimComputationSummary.WctPaint = "0";
                objClaimComputationSummary.TowingCharges = "0";
                objClaimComputationSummary.SpotRepair = "0";
                objClaimComputationSummary.EngineSafeCoverAmount = "900";
                objClaimComputationSummary.Part_Cost_VAT = "0";
                objClaimComputationSummary.Paint_Cost_VAT = "0";
                objClaimComputationSummary.Paint_80Cost_VAT = "0";
                objClaimComputationSummary.NilPartsDeprAmount = "0";
                objClaimComputationSummary.NilPaintDeprAmount = "0";
                objClaimComputationSummary.TotNilDeprAmount = "0";
                objClaimComputationSummary.Paint_Labour_VAT = "0";
                objClaimComputationSummary.SalvageValue = "0";
                objClaimComputationSummary.CompulsaryExcess = "2000";
                objClaimComputationSummary.VoluntaryExcess = "0";
                objClaimComputationSummary.ImposedExcess = "0";
                objClaimComputationSummary.NCBDeduction = "0";
                objClaimComputationSummary.ReturnToInvoiceValue = "0";
                objClaimComputationSummary.OtherAmount = "0";
                objClaimComputationSummary.NetLiability = "52648.75";
                objClaimComputationSummary.TypeOfSettlement = "1"; // Repair Basis(1)    Repair Basis/ Salvage Loss / Total Loss / Cash Loss(1 / 2 / 3 / 4)
                objClaimComputationSummary.ConsumableAmount = "0";
                objClaimComputationSummary.LabourPercentage = "75";  //multiples of 5 upto 100(5, 10, 15, 20, …, 100)
                objClaimComputationSummary.MaterialOnPaint = "25"; // multiples of 5 upto 100(5, 10, 15, 20, …, 100)
                objClaimComputationSummary.InvoiceTypeId = "1"; // Insured Select.. / KGI / Insured / KGI A / C Insured / Insured A / C KGI
                objClaimComputationSummary.OtherAmountDeducted = "True"; //Yes / No(True / False)   True / False


                objClaimComputationSheetClassLibrary.SummaryDetails = objClaimComputationSummary;
                objClaimServiceResult_SurveyorDataEntry.ClaimData.Motor_OD_ComputationSheetObject = objClaimComputationSheetClassLibrary;

                ServiceReference1.GdtCompOD objGdtCompOD = new ServiceReference1.GdtCompOD();

                objGdtCompOD.RepairReplace = "Repair"; // Repair
                objGdtCompOD.AddonCover = "EngineSafe"; // EngineSafe
                objGdtCompOD.PartName = "GUDGEON PIN"; // GUDGEON PIN
                objGdtCompOD.RRCharges = "1000"; // 1000
                objGdtCompOD.RepairCharges = "1000"; // 1000
                objGdtCompOD.PaintCharges = "52100"; // 52100
                objGdtCompOD.Depriciation = "10"; // 10
                objGdtCompOD.PaintLabour = "39075"; // 39075

                objGdtCompOD.RowIndex = 1;
                objGdtCompOD.RepairReplace = "Repair"; //  Repair / Replace
                objGdtCompOD.AddonCover = "EngineSafe"; //  EngineSafe / Consumable / NA
                objGdtCompOD.PartName = "GUDGEON PIN";
                objGdtCompOD.PartId = ""; //(from Master)
                objGdtCompOD.Material = "Metal";
                objGdtCompOD.MaterialTypeID = ""; //(from Master)
                objGdtCompOD.PartRate = "1000";
                objGdtCompOD.RRCharges = "1000";
                objGdtCompOD.RepairCharges = "1000";
                objGdtCompOD.PaintCharges = "52100";
                objGdtCompOD.Depriciation = "10";
                objGdtCompOD.Imt = "NA";
                objGdtCompOD.PaintLabour = "39075";
                objGdtCompOD.PaintMaterial = "13025";
                objGdtCompOD.DepreciatedPartAmount = "100";
                objGdtCompOD.DepreciatedPaintMaterialAmt = "6512.5";
                objGdtCompOD.NetPartAmount = "900";
                objGdtCompOD.NetPaintPartAmount = "6512.5";
                objGdtCompOD.TotLabourAmount = "41075";



                List<ServiceReference1.GdtCompOD> list_GdtCompOD = new List<ServiceReference1.GdtCompOD>();
                list_GdtCompOD.Add(objGdtCompOD);
                objClaimServiceResult_SurveyorDataEntry.ClaimData.Motor_OD_ComputationSheetObject.ODPartsList = list_GdtCompOD.ToArray();

                ServiceReference1.ClaimResourceDataEntry_MotorClass objClaimResourceDataEntry_MotorClass = new ServiceReference1.ClaimResourceDataEntry_MotorClass();
                objClaimResourceDataEntry_MotorClass.ClaimSettlementTypeDesc = "Partial Loss"; // Partial Loss
                objClaimServiceResult_SurveyorDataEntry.ClaimData.MotorClass_GeneralInformation_Details = objClaimResourceDataEntry_MotorClass;


                string status = proxy.ClaimSaveAssessmentSheet(
                      strUserId: "GC0014"
                    , strPassword: "cmc123"
                    , ClaimNo: "10410000179"
                    , objServiceResult: ref objClaimServiceResult_SurveyorDataEntry
                    );

                proxy.Close();

                Response.Write(status);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void ClaimSaveAssessmentSheet_Madiha()
        {
            try
            {

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.ClaimServiceResult_SurveyorDataEntry objClaimServiceResult_SurveyorDataEntry = new ServiceReference1.ClaimServiceResult_SurveyorDataEntry();
                ServiceReference1.CUD_SurveroyDataEntry objCUD_SurveroyDataEntry = new ServiceReference1.CUD_SurveroyDataEntry();

                objCUD_SurveroyDataEntry.NotificationNumber = "10410000179";
                objCUD_SurveroyDataEntry.MotorClaimType = "OD"; //"OD/ TP / THEFT";
                objCUD_SurveroyDataEntry.LOBCode = "31"; //"31(for MOTOR)";
                objCUD_SurveroyDataEntry.ClaimType = "OD"; // OD/ TP / PA
                objCUD_SurveroyDataEntry.VehicalRegDate = "01/01/2016";
                objCUD_SurveroyDataEntry.NOLType = "Claim Nature of Loss";
                objCUD_SurveroyDataEntry.PageSpecificCommonVariable = "1"; // Repair Basis/ Salvage Loss / Total Loss / Cash Loss(1 / 2 / 3 / 4)
                objCUD_SurveroyDataEntry.NodeName = "MOTOR CLAIM PROCESS & SETTLEMENT"; //  MOTOR CLAIM PROCESS &SETTLEMENT(Activity / Page Name)
                objCUD_SurveroyDataEntry.AppUserID = "123"; // GC123 (User ID)
                objCUD_SurveroyDataEntry.SessionID = "201612021017"; // Session ID
                objCUD_SurveroyDataEntry.TransactionID = "169859"; // Transaction ID
                objCUD_SurveroyDataEntry.TransactionDateTime = "02/12/2016"; // Transaction Date(System Date)
                objClaimServiceResult_SurveyorDataEntry.ClaimData = objCUD_SurveroyDataEntry;

                ServiceReference1.ClaimComputationSummary objClaimComputationSummary = new ServiceReference1.ClaimComputationSummary();
                ServiceReference1.ClaimComputationSheetClassLibrary objClaimComputationSheetClassLibrary = new ServiceReference1.ClaimComputationSheetClassLibrary();


                objClaimComputationSummary.MarketValueIDV = "300000";
                objClaimComputationSummary.GrossPartAmountPaintMaterial = "6512.5";
                objClaimComputationSummary.GrossPartAmountMaterial = "900";
                objClaimComputationSummary.NetCostOfPart = "900";
                objClaimComputationSummary.NetCostOfPaintMaterial = "6512.5";
                objClaimComputationSummary.NetCostOfLabour = "41075";
                objClaimComputationSummary.LabourServiceTax = "6161.25";
                objClaimComputationSummary.WctRate = "0";
                objClaimComputationSummary.WctPaint = "0";
                objClaimComputationSummary.TowingCharges = "0";
                objClaimComputationSummary.SpotRepair = "0";
                objClaimComputationSummary.EngineSafeCoverAmount = "900";
                objClaimComputationSummary.Part_Cost_VAT = "0";
                objClaimComputationSummary.Paint_Cost_VAT = "0";
                objClaimComputationSummary.Paint_80Cost_VAT = "0";
                objClaimComputationSummary.NilPartsDeprAmount = "0";
                objClaimComputationSummary.NilPaintDeprAmount = "0";
                objClaimComputationSummary.TotNilDeprAmount = "0";
                objClaimComputationSummary.Paint_Labour_VAT = "0";
                objClaimComputationSummary.SalvageValue = "0";
                objClaimComputationSummary.CompulsaryExcess = "2000";
                objClaimComputationSummary.VoluntaryExcess = "0";
                objClaimComputationSummary.ImposedExcess = "0";
                objClaimComputationSummary.NCBDeduction = "0";
                objClaimComputationSummary.ReturnToInvoiceValue = "0";
                objClaimComputationSummary.OtherAmount = "0";
                objClaimComputationSummary.NetLiability = "52648.75";
                objClaimComputationSummary.TypeOfSettlement = "1"; // Repair Basis(1)    Repair Basis/ Salvage Loss / Total Loss / Cash Loss(1 / 2 / 3 / 4)
                objClaimComputationSummary.ConsumableAmount = "0";
                objClaimComputationSummary.LabourPercentage = "75";  //multiples of 5 upto 100(5, 10, 15, 20, …, 100)
                objClaimComputationSummary.MaterialOnPaint = "25"; // multiples of 5 upto 100(5, 10, 15, 20, …, 100)
                objClaimComputationSummary.InvoiceTypeId = "1"; // Insured Select.. / KGI / Insured / KGI A / C Insured / Insured A / C KGI
                objClaimComputationSummary.OtherAmountDeducted = "True"; //Yes / No(True / False)   True / False


                objClaimComputationSheetClassLibrary.SummaryDetails = objClaimComputationSummary;
                objClaimServiceResult_SurveyorDataEntry.ClaimData.Motor_OD_ComputationSheetObject = objClaimComputationSheetClassLibrary;

                ServiceReference1.GdtCompOD objGdtCompOD = new ServiceReference1.GdtCompOD();

                objGdtCompOD.RepairReplace = "Repair"; // Repair
                objGdtCompOD.AddonCover = "EngineSafe"; // EngineSafe
                objGdtCompOD.PartName = "GUDGEON PIN"; // GUDGEON PIN
                objGdtCompOD.RRCharges = "1000"; // 1000
                objGdtCompOD.RepairCharges = "1000"; // 1000
                objGdtCompOD.PaintCharges = "52100"; // 52100
                objGdtCompOD.Depriciation = "10"; // 10
                objGdtCompOD.PaintLabour = "39075"; // 39075

                objGdtCompOD.RowIndex = 1;
                objGdtCompOD.RepairReplace = "Repair"; //  Repair / Replace
                objGdtCompOD.AddonCover = "EngineSafe"; //  EngineSafe / Consumable / NA
                objGdtCompOD.PartName = "GUDGEON PIN";
                objGdtCompOD.PartId = ""; //(from Master)
                objGdtCompOD.Material = "Metal";
                objGdtCompOD.MaterialTypeID = ""; //(from Master)
                objGdtCompOD.PartRate = "1000";
                objGdtCompOD.RRCharges = "1000";
                objGdtCompOD.RepairCharges = "1000";
                objGdtCompOD.PaintCharges = "52100";
                objGdtCompOD.Depriciation = "10";
                objGdtCompOD.Imt = "NA";
                objGdtCompOD.PaintLabour = "39075";
                objGdtCompOD.PaintMaterial = "13025";
                objGdtCompOD.DepreciatedPartAmount = "100";
                objGdtCompOD.DepreciatedPaintMaterialAmt = "6512.5";
                objGdtCompOD.NetPartAmount = "900";
                objGdtCompOD.NetPaintPartAmount = "6512.5";
                objGdtCompOD.TotLabourAmount = "41075";



                List<ServiceReference1.GdtCompOD> list_GdtCompOD = new List<ServiceReference1.GdtCompOD>();
                list_GdtCompOD.Add(objGdtCompOD);
                objClaimServiceResult_SurveyorDataEntry.ClaimData.Motor_OD_ComputationSheetObject.ODPartsList = list_GdtCompOD.ToArray();

                ServiceReference1.ClaimResourceDataEntry_MotorClass objClaimResourceDataEntry_MotorClass = new ServiceReference1.ClaimResourceDataEntry_MotorClass();
                objClaimResourceDataEntry_MotorClass.ClaimSettlementTypeDesc = "Partial Loss"; // Partial Loss
                objClaimServiceResult_SurveyorDataEntry.ClaimData.MotorClass_GeneralInformation_Details = objClaimResourceDataEntry_MotorClass;


                string status = proxy.ClaimSaveAssessmentSheet(
                      strUserId: "123"
                    , strPassword: "cmc123"
                    , ClaimNo: "10410000179"
                    , objServiceResult: ref objClaimServiceResult_SurveyorDataEntry
                    );

                proxy.Close();

                Response.Write(status);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void ClaimSaveAssessmentSheet_New()
        {
            try
            {

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.ClaimServiceResult_SurveyorDataEntry objClaimServiceResult_SurveyorDataEntry = new ServiceReference1.ClaimServiceResult_SurveyorDataEntry();
                ServiceReference1.CUD_SurveroyDataEntry objCUD_SurveroyDataEntry = new ServiceReference1.CUD_SurveroyDataEntry();

                objCUD_SurveroyDataEntry.NotificationNumber = "10110000582";
                objCUD_SurveroyDataEntry.MotorClaimType = "OD"; //"OD/ TP / THEFT";
                objCUD_SurveroyDataEntry.LOBCode = "31"; //"31(for MOTOR)";
                objCUD_SurveroyDataEntry.ClaimType = "OD"; // OD/ TP / PA
                objCUD_SurveroyDataEntry.VehicalRegDate = "15/06/2016";
                objCUD_SurveroyDataEntry.NOLType = "Insured vehicle was damaged while parked";
                objCUD_SurveroyDataEntry.PageSpecificCommonVariable = "1"; // Repair Basis/ Salvage Loss / Total Loss / Cash Loss(1 / 2 / 3 / 4)
                objCUD_SurveroyDataEntry.NodeName = "MOTOR CLAIM PROCESS & SETTLEMENT"; //  MOTOR CLAIM PROCESS &SETTLEMENT(Activity / Page Name)
                objCUD_SurveroyDataEntry.AppUserID = "GC0022"; // GC123 (User ID)
                objCUD_SurveroyDataEntry.SessionID = "172851"; // Session ID
                objCUD_SurveroyDataEntry.TransactionID = "172851"; // Transaction ID
                objCUD_SurveroyDataEntry.TransactionDateTime = "02/01/2017"; // Transaction Date(System Date)
                objClaimServiceResult_SurveyorDataEntry.ClaimData = objCUD_SurveroyDataEntry;

                ServiceReference1.ClaimComputationSummary objClaimComputationSummary = new ServiceReference1.ClaimComputationSummary();
                ServiceReference1.ClaimComputationSheetClassLibrary objClaimComputationSheetClassLibrary = new ServiceReference1.ClaimComputationSheetClassLibrary();


                objClaimComputationSummary.MarketValueIDV = "588118";
                objClaimComputationSummary.GrossPartAmountPaintMaterial = "6512.5";
                objClaimComputationSummary.GrossPartAmountMaterial = "900";
                objClaimComputationSummary.NetCostOfPart = "7647.50";
                objClaimComputationSummary.NetCostOfPaintMaterial = "1282.50";
                objClaimComputationSummary.NetCostOfLabour = "7700.00";
                objClaimComputationSummary.LabourServiceTax = "1155.00";
                objClaimComputationSummary.WctRate = "8";
                objClaimComputationSummary.WctPaint = "540.00";
                objClaimComputationSummary.TowingCharges = "0.00";
                objClaimComputationSummary.SpotRepair = "0.00";
                objClaimComputationSummary.EngineSafeCoverAmount = "0";
                objClaimComputationSummary.Part_Cost_VAT = "15";
                objClaimComputationSummary.Paint_Cost_VAT = "14";
                objClaimComputationSummary.Paint_80Cost_VAT = "4";
                objClaimComputationSummary.NilPartsDeprAmount = "0";
                objClaimComputationSummary.NilPaintDeprAmount = "0";
                objClaimComputationSummary.TotNilDeprAmount = "0";
                objClaimComputationSummary.Paint_Labour_VAT = "0";
                objClaimComputationSummary.SalvageValue = "0";
                objClaimComputationSummary.CompulsaryExcess = "2000";
                objClaimComputationSummary.VoluntaryExcess = "0";
                objClaimComputationSummary.ImposedExcess = "0";
                objClaimComputationSummary.NCBDeduction = "0";
                objClaimComputationSummary.ReturnToInvoiceValue = "0";
                objClaimComputationSummary.OtherAmount = "0";
                objClaimComputationSummary.NetLiability = "18226";
                objClaimComputationSummary.TypeOfSettlement = "1"; // Repair Basis(1)    Repair Basis/ Salvage Loss / Total Loss / Cash Loss(1 / 2 / 3 / 4)
                objClaimComputationSummary.ConsumableAmount = "0";
                objClaimComputationSummary.LabourPercentage = "75";  //multiples of 5 upto 100(5, 10, 15, 20, …, 100)
                objClaimComputationSummary.MaterialOnPaint = "25"; // multiples of 5 upto 100(5, 10, 15, 20, …, 100)
                objClaimComputationSummary.InvoiceTypeId = "1"; // Insured Select.. / KGI / Insured / KGI A / C Insured / Insured A / C KGI
                objClaimComputationSummary.OtherAmountDeducted = "True"; //Yes / No(True / False)   True / False


                objClaimComputationSheetClassLibrary.SummaryDetails = objClaimComputationSummary;
                objClaimServiceResult_SurveyorDataEntry.ClaimData.Motor_OD_ComputationSheetObject = objClaimComputationSheetClassLibrary;

                ServiceReference1.GdtCompOD objGdtCompOD = new ServiceReference1.GdtCompOD();

                ServiceReference1.GdtCompOD objGdtCompOD2 = new ServiceReference1.GdtCompOD();

                //objGdtCompOD.RepairReplace = "Replace"; // Repair
                //objGdtCompOD.AddonCover = "NA"; // EngineSafe
                //objGdtCompOD.PartName = "Hood"; // GUDGEON PIN
                //objGdtCompOD.RRCharges = "500.00"; // 1000
                //objGdtCompOD.RepairCharges = "0.0"; // 1000
                //objGdtCompOD.PaintCharges = "4500.0"; // 52100
                //objGdtCompOD.Depriciation = "5"; // 10
                //objGdtCompOD.PaintLabour = "3375.0"; // 39075

                objGdtCompOD.RowIndex = 0;
                objGdtCompOD.RepairReplace = "Replace"; //  Repair / Replace
                objGdtCompOD.AddonCover = "NA"; //  EngineSafe / Consumable / NA
                objGdtCompOD.PartName = "Hood";
                objGdtCompOD.PartId = ""; //(from Master)
                objGdtCompOD.Material = "Metal";
                objGdtCompOD.MaterialTypeID = ""; //(from Master)
                objGdtCompOD.PartRate = "5000.0";
                objGdtCompOD.RRCharges = "500.00";
                objGdtCompOD.RepairCharges = "0.0";
                objGdtCompOD.PaintCharges = "4500.0";
                objGdtCompOD.Depriciation = "5";
                objGdtCompOD.Imt = "NA";
                objGdtCompOD.PaintLabour = "3375.0";
                objGdtCompOD.PaintMaterial = "1125.0";
                objGdtCompOD.DepreciatedPartAmount = "250.0";
                objGdtCompOD.DepreciatedPaintMaterialAmt = "562.5";
                objGdtCompOD.NetPartAmount = "0";
                objGdtCompOD.NetPaintPartAmount = "0";
                objGdtCompOD.TotLabourAmount = "0";


                objGdtCompOD2.RowIndex = 1;
                objGdtCompOD2.RepairReplace = "Replace"; //  Repair / Replace
                objGdtCompOD2.AddonCover = "NA"; //  EngineSafe / Consumable / NA
                objGdtCompOD2.PartName = "Hood Hinges";
                objGdtCompOD2.PartId = ""; //(from Master)
                objGdtCompOD2.Material = "Metal";
                objGdtCompOD2.MaterialTypeID = ""; //(from Master)
                objGdtCompOD2.PartRate = "2000.0";
                objGdtCompOD2.RRCharges = "450.0";
                objGdtCompOD2.RepairCharges = "0.0";
                objGdtCompOD2.PaintCharges = "4500.0";
                objGdtCompOD2.Depriciation = "5";
                objGdtCompOD2.Imt = "NA";
                objGdtCompOD2.PaintLabour = "3375.0";
                objGdtCompOD2.PaintMaterial = "1125.0";
                objGdtCompOD2.DepreciatedPartAmount = "100.0";
                objGdtCompOD2.DepreciatedPaintMaterialAmt = "562.5";
                objGdtCompOD2.NetPartAmount = "0";
                objGdtCompOD2.NetPaintPartAmount = "0";
                objGdtCompOD2.TotLabourAmount = "0";

                List<ServiceReference1.GdtCompOD> list_GdtCompOD = new List<ServiceReference1.GdtCompOD>();
                list_GdtCompOD.Add(objGdtCompOD);
                list_GdtCompOD.Add(objGdtCompOD2);
                objClaimServiceResult_SurveyorDataEntry.ClaimData.Motor_OD_ComputationSheetObject.ODPartsList = list_GdtCompOD.ToArray();

                ServiceReference1.ClaimResourceDataEntry_MotorClass objClaimResourceDataEntry_MotorClass = new ServiceReference1.ClaimResourceDataEntry_MotorClass();
                objClaimResourceDataEntry_MotorClass.ClaimSettlementTypeDesc = "Partial Loss"; // Partial Loss
                objClaimServiceResult_SurveyorDataEntry.ClaimData.MotorClass_GeneralInformation_Details = objClaimResourceDataEntry_MotorClass;


                string status = proxy.ClaimSaveAssessmentSheet(
                      strUserId: "GC0022"
                    , strPassword: "cmc123"
                    , ClaimNo: "10110000582"
                    , objServiceResult: ref objClaimServiceResult_SurveyorDataEntry
                    );

                proxy.Close();

                Response.Write(status);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void ClaimSaveAssessmentSheet_Hitesh()
        {
            try
            {

                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.ClaimServiceResult_SurveyorDataEntry objClaimServiceResult_SurveyorDataEntry = new ServiceReference1.ClaimServiceResult_SurveyorDataEntry();
                ServiceReference1.CUD_SurveroyDataEntry objCUD_SurveroyDataEntry = new ServiceReference1.CUD_SurveroyDataEntry();

                objCUD_SurveroyDataEntry.NotificationNumber = "10110000582";
                objCUD_SurveroyDataEntry.MotorClaimType = "OD"; //"OD/ TP / THEFT";
                objCUD_SurveroyDataEntry.LOBCode = "31"; //"31(for MOTOR)";
                objCUD_SurveroyDataEntry.ClaimType = "OD"; // OD/ TP / PA
                objCUD_SurveroyDataEntry.VehicalRegDate = "15/06/2016";
                objCUD_SurveroyDataEntry.NOLType = "Insured vehicle was damaged while parked";
                objCUD_SurveroyDataEntry.PageSpecificCommonVariable = "1"; // Repair Basis/ Salvage Loss / Total Loss / Cash Loss(1 / 2 / 3 / 4)
                objCUD_SurveroyDataEntry.NodeName = "MOTOR CLAIM PROCESS & SETTLEMENT"; //  MOTOR CLAIM PROCESS &SETTLEMENT(Activity / Page Name)
                objCUD_SurveroyDataEntry.AppUserID = "GC0022"; // GC123 (User ID)
                objCUD_SurveroyDataEntry.SessionID = "172851"; // Session ID
                objCUD_SurveroyDataEntry.TransactionID = "172851"; // Transaction ID
                objCUD_SurveroyDataEntry.TransactionDateTime = "02/01/2017"; // Transaction Date(System Date)
                objClaimServiceResult_SurveyorDataEntry.ClaimData = objCUD_SurveroyDataEntry;

                ServiceReference1.ClaimComputationSummary objClaimComputationSummary = new ServiceReference1.ClaimComputationSummary();
                ServiceReference1.ClaimComputationSheetClassLibrary objClaimComputationSheetClassLibrary = new ServiceReference1.ClaimComputationSheetClassLibrary();


                objClaimComputationSummary.MarketValueIDV = "588118";
                objClaimComputationSummary.GrossPartAmountPaintMaterial = "";
                objClaimComputationSummary.GrossPartAmountMaterial = "";
                objClaimComputationSummary.NetCostOfPart = "7647.50";
                objClaimComputationSummary.NetCostOfPaintMaterial = "1282.50";
                objClaimComputationSummary.NetCostOfLabour = "7700.00";
                objClaimComputationSummary.LabourServiceTax = "1155.00";
                objClaimComputationSummary.WctRate = "8";
                objClaimComputationSummary.WctPaint = "540.00";
                objClaimComputationSummary.TowingCharges = "0.00";
                objClaimComputationSummary.SpotRepair = "0.00";
                objClaimComputationSummary.EngineSafeCoverAmount = "0";
                objClaimComputationSummary.Part_Cost_VAT = "15";
                objClaimComputationSummary.Paint_Cost_VAT = "14";
                objClaimComputationSummary.Paint_80Cost_VAT = "4";
                objClaimComputationSummary.NilPartsDeprAmount = "0";
                objClaimComputationSummary.NilPaintDeprAmount = "0";
                objClaimComputationSummary.TotNilDeprAmount = "0";
                objClaimComputationSummary.Paint_Labour_VAT = "0";
                objClaimComputationSummary.SalvageValue = "0"; //0.00 //c
                objClaimComputationSummary.CompulsaryExcess = "2000.00";
                objClaimComputationSummary.VoluntaryExcess = "0.00"; //c
                objClaimComputationSummary.ImposedExcess = "0";
                objClaimComputationSummary.NCBDeduction = "0";
                objClaimComputationSummary.ReturnToInvoiceValue = "0";
                objClaimComputationSummary.OtherAmount = "0.00"; //c
                objClaimComputationSummary.NetLiability = "18226";
                objClaimComputationSummary.TypeOfSettlement = "1"; // Repair Basis(1)    Repair Basis/ Salvage Loss / Total Loss / Cash Loss(1 / 2 / 3 / 4)
                objClaimComputationSummary.ConsumableAmount = "0";
                objClaimComputationSummary.LabourPercentage = "75";  //multiples of 5 upto 100(5, 10, 15, 20, …, 100) //c
                objClaimComputationSummary.MaterialOnPaint = "25"; // multiples of 5 upto 100(5, 10, 15, 20, …, 100) //c
                objClaimComputationSummary.InvoiceTypeId = "1"; // Insured Select.. / KGI / Insured / KGI A / C Insured / Insured A / C KGI
                objClaimComputationSummary.OtherAmountDeducted = "False"; //Yes / No(True / False)   True / False


                objClaimComputationSheetClassLibrary.SummaryDetails = objClaimComputationSummary;
                objClaimServiceResult_SurveyorDataEntry.ClaimData.Motor_OD_ComputationSheetObject = objClaimComputationSheetClassLibrary;

                ServiceReference1.GdtCompOD objGdtCompOD = new ServiceReference1.GdtCompOD();

                

                objGdtCompOD.RowIndex = 0;
                objGdtCompOD.RepairReplace = "Replace"; //  Repair / Replace
                objGdtCompOD.AddonCover = "NA"; //  EngineSafe / Consumable / NA
                objGdtCompOD.PartName = "Hood";
                objGdtCompOD.PartId = ""; //(from Master) //c
                objGdtCompOD.Material = "Metal";
                objGdtCompOD.MaterialTypeID = ""; //(from Master) //c
                objGdtCompOD.PartRate = "5000.0";
                objGdtCompOD.RRCharges = "500.0";
                objGdtCompOD.RepairCharges = "0.0";
                objGdtCompOD.PaintCharges = "4500.0";
                objGdtCompOD.Depriciation = "5";
                objGdtCompOD.Imt = "NA";
                objGdtCompOD.PaintLabour = "3375.0";
                objGdtCompOD.PaintMaterial = "1125.0";
                objGdtCompOD.DepreciatedPartAmount = "250.0";
                objGdtCompOD.DepreciatedPaintMaterialAmt = "562.5";
                objGdtCompOD.NetPartAmount = "900"; //c
                objGdtCompOD.NetPaintPartAmount = "6512.5"; //c
                objGdtCompOD.TotLabourAmount = "41075"; //c



                List<ServiceReference1.GdtCompOD> list_GdtCompOD = new List<ServiceReference1.GdtCompOD>();
                list_GdtCompOD.Add(objGdtCompOD);
                objClaimServiceResult_SurveyorDataEntry.ClaimData.Motor_OD_ComputationSheetObject.ODPartsList = list_GdtCompOD.ToArray();

                ServiceReference1.ClaimResourceDataEntry_MotorClass objClaimResourceDataEntry_MotorClass = new ServiceReference1.ClaimResourceDataEntry_MotorClass();
                objClaimResourceDataEntry_MotorClass.ClaimSettlementTypeDesc = "Partial Loss"; // Partial Loss
                objClaimServiceResult_SurveyorDataEntry.ClaimData.MotorClass_GeneralInformation_Details = objClaimResourceDataEntry_MotorClass;


                string status = proxy.ClaimSaveAssessmentSheet(
                      strUserId: "GC0014"
                    , strPassword: "cmc123"
                    , ClaimNo: "10110000583"
                    , objServiceResult: ref objClaimServiceResult_SurveyorDataEntry
                    );

                proxy.Close();

                Response.Write(status);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnSaveClaimRegistration_Click(object sender, EventArgs e)
        {
            //ClaimSaveRegistrationNew();
            //ClaimServiceUpdateDriverDetails();
            //ClaimServiceSaveNote();
            //ClaimSaveAssessmentSheet();
            //ClaimSaveAssessmentSheet_Madiha();
            //ClaimSaveAssessmentSheet_New();

            ClaimSaveAssessmentSheet_Hitesh();
        }
    }
}