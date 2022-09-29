using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PayUPayment
{
    [DataContract]
    public class PayURequest
    {
        public string hash { get; set; }
        public string txnid { get; set; }
        public string key { get; set; }
        public string amount { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string productinfo { get; set; }
        public string surl { get; set; }
        public string furl { get; set; }
        public string lastname { get; set; }
        public string curl { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string pg { get; set; }


    }

    public class PayUVerifyRequest
    {
        public string key { get; set; }
        public string command { get; set; }
        public string hash { get; set; }
        public string var1 { get; set; }

        public string salt { get; set; }
    }

    public class PayUResponse
    {
        public string mihpayid { get; set; }
        public string request_id { get; set; }
        public string bank_ref_num { get; set; }
        public string amt { get; set; }
        public string transaction_amount { get; set; }
        public string txnid { get; set; }
        public string additional_charges { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string bankcode { get; set; }
        public string udf1 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public object field2 { get; set; }
        public string field9 { get; set; }
        public string error_code { get; set; }
        public string addedon { get; set; }
        public string payment_source { get; set; }
        public object card_type { get; set; }
        public string error_Message { get; set; }
        public string net_amount_debit { get; set; }
        public string disc { get; set; }
        public string mode { get; set; }
        public string PG_TYPE { get; set; }
        public string card_no { get; set; }
        public string udf2 { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public object Merchant_UTR { get; set; }
        public string Settled_At { get; set; }
    }
    public class PayUTransactionDetails
    {
        public string vworkFlowId { get; set; }
        public string vquoteNumber { get; set; }
        public string vCustID { get; set; }

        public string intermediaryCode { get; set; }
        public string intermediaryLocCode { get; set; }

        public string businesstype { get; set; }

        public string applicationNo { get; set; }

        public string PrevPolicyNumber { get; set; }

        public string vloggedInUsertID { get; set; }

        public string vCustomerEmailId { get; set; }

        public string nCustomerMobileNumber { get; set; }

        public PayUResponse PayuResp { get; set; }
    }


    public class PayUResponseDetails
    {
        public int status { get; set; }
        public string msg { get; set; }
        public List<PayUResponse> transaction_details { get; set; }
    }
}
