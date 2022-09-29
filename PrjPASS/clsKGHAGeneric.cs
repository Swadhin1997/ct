using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PrjPASS
{
    public class clsKGHAGeneric
    {
    }
    [DataContract]
    public class clsProposerPaymentDetailsResponse
    {
        [DataMember]
        public string VProposalno { get; set; }
        [DataMember]

        public string vMemberuniqueid { get; set; }
        [DataMember]
        public string VProposerName { get; set; }
        [DataMember]
        public string vProposerGender { get; set; }
        [DataMember]
        public string vProposerdob { get; set; }
        [DataMember]
        public string vProposerAddress { get; set; }
        [DataMember]
        public string vProposerMobile { get; set; }
        [DataMember]
        public string vProposerocc { get; set; }
        [DataMember]
        public string vProposeremail { get; set; }
        [DataMember]
        public string vPolicyNo { get; set; }
        [DataMember]
        public string vpolicystartDate { get; set; }
        [DataMember]
        public string vpolicystartEnd { get; set; }
        [DataMember]

        public string vStatus { get; set; }
        [DataMember]
        public string vErrorMsg { get; set; }
        [DataMember]
        public string vplan { get; set; }

        public string vPlanDesc { get; set; }
        [DataMember]
        public string vSumInsured { get; set; }
        [DataMember]
        public string vpolicyTenure { get; set; }
        [DataMember]
        public string vNetPremium { get; set; }
        [DataMember]
        public string vGST { get; set; }
        [DataMember]
        public string vMemberCovered { get; set; }
        [DataMember]
        public Double vTotal { get; set; }
        [DataMember]
        public string vTotalPremium { get; set; }
        [DataMember]
        public string vbankingref { get; set; }
        [DataMember]
        public string vtrandate { get; set; }
        [DataMember]
        public string vpaymentmode { get; set; }
        [DataMember]
        public string vBankName { get; set; }
        [DataMember]
        public string vCity { get; set; }
    }



}