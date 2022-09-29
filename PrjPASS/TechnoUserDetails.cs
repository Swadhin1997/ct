using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPASS
{
    public class clsTechnoUserDetails
    {
        public enum enMaritalStatus
        {
         Single =1, Married=2, Divorced=3, Widowed=4,Others=5
        };
        public enum enGender
        {
            Male =1, Female=2,Other=3
        };
        public enum enIdProofTypes
        {
            PANCard=1, PassportNo=2, VoterID=3, DrivingLicens=4, GovtIssuedIDCard=5,StudentIDCard=6
        };

        public struct DatePart
        {
            public static IEnumerable<int> DayItems
            {
                get
                {
                    return Enumerable.Range(1, 31);
                }
            }

            public static IEnumerable<int> MonthItems
            {
                get
                {
                    return Enumerable.Range(1, 12);
                }
            }

            public static IEnumerable<int> YearItems
            {
                get
                {
                    return Enumerable.Range(DateTime.Now.Year - 110, 110 + 1);
                }
            }
        }
        public string vPrimaryIMEINos { get; set; }
        public string vScratchCardNos { get; set; }
        public int nUserId { get; set; }
        public int nSalutationId { get; set; }
        public string vFirstName { get; set; }
        public string vMiddleName { get; set; }
        public string vLastName { get; set; }
        public DateTime dDOB { get; set; }
        public int nGenderId { get; set; }
        public int nMaritalStatusId { get; set; }   
        public int nOccupationId { get; set; }
        public string  vNomineeName { get; set; }
        public int nNomineeRelationshipId { get; set; }
        public int nIdProofTypeId { get; set; }
        public string vUniqueProofNos { get; set; }
        public DateTime dPurchaseDate { get; set; }
        public string vInvoiceNos { get; set; }
        public clsTechnoUserAddrDetails objUserAddr { get; set; }
        public DateTime dCreatedDate { get; set; }
        public DateTime dModifiedDate { get; set; }

    }   
    public class clsTechnoIMEI
    {
        public string vPrimaryIMEINos { get; set; }
        public string vMasterPolicyholderName { get; set; }
        public string vPolicyNos { get; set; }
        public string dPolicyStartDate { get; set; }
        public string dPolicyEndDate { get; set; }
        public DateTime dRegValidFrom { get; set; }
        public DateTime dRegValidTo { get; set; }
        public DateTime dCreatedDate { get; set; }
        public DateTime dModifiedDate { get; set; }
    }
    public class clsTechnoIMEIScratchCardMapping
    {
        public string vPrimaryIMEINos { get; set; }
        public string vScratchCardNos { get; set; }
        public DateTime dRegDate { get; set; }
        public DateTime dCreatedDate { get; set; }
        public DateTime dModifiedDate { get; set; }
        public int nUserId {get;set;}

    }
    public class clsTechnoUserAddrDetails
    {
        public int nUserId { get; set; }
        public string vAddr1 { get; set; }
        public string vAddr2 { get; set; }
        public string vAddr3 { get; set; }
        public int nPincode { get; set; }
        public string vMobileNos { get; set; }
        public string vEmailId { get; set; }
        public DateTime dCreatedDate { get; set; }
        public DateTime dModifiedDate { get; set; }
    }
    public class clsMasterData
    {
        public string ID { get; set; }

        public string Value { get; set; }

    }
    public class clsMasterFetchClasses
    {
        public List<clsMasterData> oMasterData { get; set; }

        public string vErrorMsg { get; set; }

        public bool fn_validateProperty(List<clsMasterData> oData, string value)
        {
            foreach (var item in oData)
            {
                if (item.Value.ToLower() == value.ToLower() || item.ID.ToLower() == value.ToLower())
                {
                    return false;
                }
            }
            return true;
        }    

    }
}