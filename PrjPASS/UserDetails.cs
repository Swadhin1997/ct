using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPASS
{
    public class UserDetails
    {
        public string vUserLoginId { get; set; }
        public string vUserLoginDesc { get; set; }
        public string vUserPassword { get; set; }
        public string vUserEmailId { get; set; }
        public string vIntermediaryCode { get; set; }
        public string vIntermediaryBranch { get; set; }
        public bool bIsActivate { get; set; }
        public bool IsExternalUser { get; set; }
        public int Min_MarketMovement { get; set; }
        public bool IsAllowLoginFromMobile { get; set; }
        public bool IsAllowLoginFromChotuPASS { get; set; }
        public string TypeOfUser { get; set; }
        public bool IsAllowEPOSQuoteView { get; set; }
        public string RegionalDeptHeadEmailId { get; set; }
        public bool vLocked { get; set; }

    }

    [Serializable]
    public class QuoteDetails
    {
        //commnet
        public string QuoteNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Variant { get; set; }
        public string TotalPremium { get; set; }
        public DateTime QuoteDate { get; set; }
        public string ProposalNumber { get; set; }
        public string PolicyStartDate { get; set; }
        public string BusinessType { get; set; }
        public string CustomerType { get; set; }
        public string PaymentStatus { get; set; }
        public int QuoteVersion { get; set; }
        public string IsProposalExistsForQuoteNumber { get; set; }
        public string ReviewAndConfirmLink { get; set; }
        public string PolicyNumber { get; set; }
        public string Remarks { get; set; }
        public string SourceQuoteCreator { get; set; }

        public string IsAllowPolicyStartDateEdit { get; set; }

        public string CampaignCode { get; set; }

    }

    public class PayUSavedDetails
    {//
        public string UniqueRowId { get; set; }
        public string TransactionId { get; set; }
        public string ProductInfo { get; set; }
        public string Amount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmailId { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string ValidationPeriod { get; set; }
        public string IsSendPayUEmail { get; set; }
        public string IsSendPayUSMS { get; set; }
        public string IsSendKGIEmail { get; set; }
        public string IsSendKGISMS { get; set; }
        public string IsSendShortURL { get; set; }
        public string IsDataFromBulkUpload { get; set; }
        public string PayInvoiceURL { get; set; }
        public string ShortURL { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
    public class KPaySavedDetails
    {//
        public string UniqueRowId { get; set; }
        public string TransactionId { get; set; }
        public string ProductInfo { get; set; }
        public string Amount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmailId { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string ValidationPeriod { get; set; }
        public string IsSendPayUEmail { get; set; }
        public string IsSendPayUSMS { get; set; }
        public string IsSendKGIEmail { get; set; }
        public string IsSendKGISMS { get; set; }
        public string IsSendShortURL { get; set; }
        public string IsDataFromBulkUpload { get; set; }
        public string PayInvoiceURL { get; set; }
        public string ShortURL { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
    public class FileUploadedInformation
    {
        public string FileUploadTransactionId { get; set; }
        public string FileName { get; set; }
        public string FileUploadedBy { get; set; }
        public string FileUploadedOn { get; set; }
        public string IsFileProcessed { get; set; }
        public string FileProcessedOn { get; set; }
    }

    public class EproposalDetails
    {//
        public string UniqueRowId { get; set; }
        public string ReferenceNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string Product { get; set; }
        public string SumInsuredAmt { get; set; }
        public string IMDCODE { get; set; }
        public string IMDName { get; set; }
        public string BranchCode { get; set; }
        public string BranchLocationName { get; set; }
        public string PremiumAmt { get; set; }
        public string ProposalNo { get; set; }
        public string RowCreatedBy { get; set; }
        public DateTime RowCreatedOn { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string IsFileUploadedToKites { get; set; }
        public DateTime FileUploadedToKitesOn { get; set; }
        public string IsSMSSent { get; set; }
        public string IsEmailSent { get; set; }
        public string IsProposalVerified { get; set; }
        public DateTime ProposalVerifiedOn { get; set; }
        public string IsActive { get; set; }
        public string OTPNumber { get; set; }
    }
}