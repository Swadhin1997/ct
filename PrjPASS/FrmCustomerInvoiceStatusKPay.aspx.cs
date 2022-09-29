using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CCA.Util;
using System.Collections.Specialized;

namespace PrjPASS
{
    public partial class FrmCustomerInvoiceStatusKPay : System.Web.UI.Page
    {
       
    
       
        string encResp = string.Empty;
        string orderNo = string.Empty;
        string crossSellUrl = string.Empty;
        string order_id = string.Empty;
        string tracking_id = string.Empty;
        string bank_ref_no = string.Empty;
        string order_status = string.Empty;
        string failure_message = string.Empty;
        string payment_mode = string.Empty;
        string card_name = string.Empty;
        string status_code = string.Empty;
        string status_message = string.Empty;
        string currency = string.Empty;
        string amount = string.Empty;
        string billing_name = string.Empty;
        string billing_address = string.Empty;
        string billing_city = string.Empty;
        string billing_state = string.Empty;
        string billing_zip = string.Empty;
        string billing_country = string.Empty;
        string billing_tel = string.Empty;
        string billing_email = string.Empty;
        string delivery_name = string.Empty;
        string delivery_address = string.Empty;
        string delivery_city = string.Empty;
        string delivery_state = string.Empty;
        string delivery_zip = string.Empty;
        string delivery_country = string.Empty;
        string delivery_tel = string.Empty;
        string merchant_param1 = string.Empty;
        string merchant_param2 = string.Empty;
        string merchant_param3 = string.Empty;
        string merchant_param4 = string.Empty;
        string merchant_param5 = string.Empty;
        string vault = string.Empty;

        string offer_type = string.Empty;
        string offer_code = string.Empty;
        string discount_value = string.Empty;
        string mer_amount = string.Empty;
        string eci_value = string.Empty;
        string retry = string.Empty;
        string response_code = string.Empty;

        string billing_notes = string.Empty;
        string trans_date = string.Empty;
        string bin_country = string.Empty;
        string inv_mer_reference_no = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!IsPostBack)
                {
                    
                    CCACrypto ccaCrypto = new CCACrypto();
                    //string workingKey = "8C04E7175291C53474B024D5AC080F7A";
                    string workingKey = ConfigurationManager.AppSettings["workingKeyKpayForDecrypt"].ToString();
                    ExceptionUtility.LogEvent("KPAY Working Key : "+workingKey);
                    string[] keys = Request.Form.AllKeys;

                    for (int i = 0; i < keys.Length; i++)
                    {

                        if (keys[i] == "encResp")
                        {
                            encResp = Request.Form[keys[i]];
                            //string encResp = "41f403bff6f50ba6f246f5d9e1b97441a5f60fa34ad7400b36ebfd543c47e2a26ff23dc8b016e3ad995acc1528b3448c5e0a815b6dc2e114c468f5e2d3d1b8154f0fa228728b22ac5e91c32b15fe679859ae4eb61f7a27f9677419f2fbb1a52d13a32c569f286616a2eade8e66acc3a8eb013fe0816ab10dd549035b6668d52cdcb2c58868264d13cd890ae8729f17805a2aa9e65dea1949b8f969620127db634253c5d54f2738b788934af2541d5a1313f75f7babc3e509c56d48e7acead7c0de2eafec923b687e6667bbb68248d080a1c65a4e1c85a1e352b6cafa095633814139726090a70e8bff53075d8d1bede55e6382838ff595ea90c397e4bf30f537ed40c023c0eff11a1dd9f6b92dc1ce7e741263ab54a13f74a723e2a033e22c440923ed13c04125b6a6f6cedb16a995e4e865a5ef26396006c2d01a1e12699bd0305f1444cd420aae12424de662ff96a1592b6d1f9ec91622373f5a6e6e843eb2814a70a59b8459a5778adaf39b730488aba77bd5a2d40761ffd94e6e82ce198007dc30fcb68a0b01533e987485c915629b8de1608203391cadc407f14e3ea727074e922acc7233fb49cc17b628b584977759f0113cd78a87c1aa89ae62b11e0b879678ba983c780ffa3817a67c76f1f64bf97809720ddf34a6ee2cd316163760c27023d5faa081d465961379496aa2b89e8fc121b8785c103d2d0939e827e38a857182b99c997ecdc10ec7eb695bc7bfb561db2fafef94200efee99f5ad0abdb928cf92ce8986f0f6c86d50ce05a263844a919c256fb93e692d4a22d0365b206a197ad2474af825263fa826bb6ed5a980952c8c7727bc5b355f9c7c129b1f303a567dae3319404d4094db2389e1b54d0c39ccf42693aa05dd0105963bb7e696911dd6d875ef031e7d0516713c7d9d03263837b2170913d2a8e52f93706b7e887499ece14174c849d2eadcdc4d3e203fc52204e66cfb47565e4648acb690eb227a21b904e6009b2fd16cacbe6bdddec69ef9ad91bd3c70c0a7c1b73ee3cc93cdfff939c5a909679728ab37b51f5b4b01506b00a7ab1c7720f7bbeac8461825d9042b6d74e7d0ffda4813c4c6ad144123496d23f1b5d925d3d8e264b49b5e2f0f2a4e1feecac8a027656968c1779148226dd44215a2d079aa0c7f999686d8c7f0e"; // string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"],workingKey);
                            ExceptionUtility.LogEvent("KPAY enc Resp  : " + encResp);
                            // NameValueCollection param = getResponseMap(encResp);
                            // string workingKey = ConfigurationManager.AppSettings["workingKey"].ToString();
                            String ResJson = ccaCrypto.Decrypt(encResp, workingKey);
                            ExceptionUtility.LogEvent("KPAY ResJson :  " + ResJson);
                            NameValueCollection Params = new NameValueCollection();
                            string[] segments = ResJson.Split('&');
                            
                            order_id = segments[0].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY order_id" + segments[0]);
                            tracking_id = segments[1].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY tracking_id" + segments[1]);
                            lblPAYUTransactionId.Text = tracking_id;
                            bank_ref_no = segments[2].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY bank_ref_no" + segments[2]);
                            order_status = segments[3].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY order_status" + segments[3]);
                            lblThankYouOrSorry.Text = order_status.ToLower() == "success" ? "Thank You" : "Sorry";
                            
                            lblPaymentStatus.Text = order_status.ToUpper();
                            failure_message = segments[4].Split('=')[1];
                            ExceptionUtility.LogEvent(" KPAY failure_message" + segments[4]);
                            payment_mode = segments[5].Split('=')[1];
                            ExceptionUtility.LogEvent(" KPAY payment_mode" + segments[5]);
                            card_name = segments[6].Split('=')[1]; 
                            status_code = segments[7].Split('=')[1];
                            ExceptionUtility.LogEvent(" KPAY status_code" + segments[7]);
                            status_message = segments[8].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY status_message" + segments[8]);
                            currency = segments[9].Split('=')[1]; ;
                            amount = segments[10].Split('=')[1]; ;
                            lblTransactionAmount.Text = amount;
                            billing_name = segments[11].Split('=')[1];
                            lblCustomerName.Text = billing_name;
                            //lblCustomerName.Text = billing_name.Split('=')[1]; 
                             billing_address = segments[12].Split('=')[1]; ;
                             billing_city = segments[13].Split('=')[1]; ;
                             billing_state = segments[14].Split('=')[1]; ;
                            billing_zip = segments[15].Split('=')[1]; ;
                            billing_country = segments[16].Split('=')[1]; ;
                            billing_tel = segments[17].Split('=')[1]; 
                            lblCustomerPhone.Text = billing_tel;
                            billing_email = segments[18].Split('=')[1]; 
                            lblCustomerEmail.Text = billing_email;
                            delivery_name = segments[19].Split('=')[1]; 
                            delivery_address = segments[20].Split('=')[1]; 
                            delivery_city = segments[21].Split('=')[1]; 
                            delivery_state = segments[22].Split('=')[1]; 
                            delivery_zip = segments[23].Split('=')[1]; 
                            delivery_country = segments[24].Split('=')[1]; 
                            delivery_tel = segments[25].Split('=')[1]; 
                            merchant_param1 = segments[26].Split('=')[1];
                            ExceptionUtility.LogEvent(" KPAY merchant_param1" + segments[26]);
                            lblKGITransactionId.Text = merchant_param1;
                            merchant_param2 = segments[27].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY merchant_param2" + segments[27]);
                            merchant_param3 = segments[28].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY merchant_para31" + segments[28]);
                            merchant_param4 = segments[29].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY merchant_param4" + segments[29]);
                            merchant_param5 = segments[30].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY merchant_param5" + segments[30]);
                            vault = segments[31].Split('=')[1]; 
                            offer_type = segments[32].Split('=')[1]; 
                            offer_code = segments[33].Split('=')[1]; 
                            discount_value = segments[34].Split('=')[1]; 
                            mer_amount = segments[35].Split('=')[1]; 
                            eci_value = segments[36].Split('=')[1]; 
                            retry = segments[37].Split('=')[1]; 
                            response_code = segments[38].Split('=')[1]; 
                            billing_notes = segments[39].Split('=')[1]; 
                            trans_date = segments[40].Split('=')[1]; 
                            bin_country = segments[41].Split('=')[1]; 
                            inv_mer_reference_no = segments[42].Split('=')[1];
                            ExceptionUtility.LogEvent("KPAY inv_mer_reference_no" + segments[42]);


                        }
                            if (keys[i] == "orderNo")
                            {
                                orderNo = Request.Form[keys[i]];
                                //Response.Write(custID);
                            }

                            if (keys[i] == "crossSellUrl")
                            {
                                crossSellUrl = Request.Form[keys[i]];

                            }
                        }

                    
                        SaveTransactionStatus
                        (inv_mer_reference_no, merchant_param5, amount, billing_name, billing_email, billing_tel, order_id, tracking_id, bank_ref_no,
                        order_status, failure_message, payment_mode, card_name, status_code, status_message, merchant_param1, merchant_param2, merchant_param3, merchant_param4, merchant_param5,
                        discount_value, trans_date);


                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmCustomerInvoiceStatusKpay, Page Load");
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }


        }

        private string MaskEmailId(string EmailId)
        {
            string result = EmailId;
            try
            {
                string input = EmailId;
                string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
                result = Regex.Replace(input, pattern, m => new string('*', m.Length));
            }
            catch (Exception ex)
            {
                result = EmailId;
                ExceptionUtility.LogException(ex, "MaskEmailId");
            }
            return result;
        }

        private string MaskMobnumber(string mobNumber)
        {
            string result = mobNumber;
            try
            {
                result = mobNumber.Substring(mobNumber.Length - 4).PadLeft(mobNumber.Length, '*');
            }
            catch (Exception ex)
            {
                result = mobNumber;
                ExceptionUtility.LogException(ex, "MaskEmailId");
            }
            return result;
        }

        private void SaveTransactionStatus(string TransactionId, string ProductInfo, string Amount, string CustomerName, string CustomerEmailId
                                        , string CustomerMobileNumber, string PAYUTxnID, string TrackingId,string BankRefNo, string PaymentStatus, 
            string error_Message, string Payment_Mode, string Card_Name,string Status_Code,
            string Status_Message,string Merchant_Param1, string Merchant_Param2, string Merchant_Param3, string Merchant_Param4, string Merchant_Param5, string Discount_Value,string Trans_Date)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PROC_SAVE_KPAY_INVOICE_LINKS_PAYMENT_STATUS";

                        cmd.Parameters.AddWithValue("@TransactionId", TransactionId);
                        cmd.Parameters.AddWithValue("@ProductInfo", ProductInfo);
                        cmd.Parameters.AddWithValue("@Amount", Amount);
                        cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                        cmd.Parameters.AddWithValue("@CustomerEmailId", CustomerEmailId);
                        cmd.Parameters.AddWithValue("@CustomerMobileNumber", CustomerMobileNumber);
                        cmd.Parameters.AddWithValue("@PAYUTxnID", PAYUTxnID);
                        cmd.Parameters.AddWithValue("@PaymentStatus", PaymentStatus);
                        cmd.Parameters.AddWithValue("@TrackingId", TrackingId);
                        cmd.Parameters.AddWithValue("@BankRefNo", BankRefNo);
                        cmd.Parameters.AddWithValue("@Payment_Mode", Payment_Mode);
                        cmd.Parameters.AddWithValue("@Card_Name", Card_Name);
                        cmd.Parameters.AddWithValue("@Status_Code", Status_Code);
                        cmd.Parameters.AddWithValue("@Status_Message", Status_Message);
                        cmd.Parameters.AddWithValue("@Merchant_Param1", Merchant_Param1);
                        cmd.Parameters.AddWithValue("@Merchant_Param2", Merchant_Param2);
                        cmd.Parameters.AddWithValue("@Merchant_Param3", Merchant_Param3);
                        cmd.Parameters.AddWithValue("@Merchant_Param4", Merchant_Param4);
                        cmd.Parameters.AddWithValue("@Merchant_Param5", Merchant_Param5);
                        cmd.Parameters.AddWithValue("@Discount_Value", Discount_Value);
                        cmd.Parameters.AddWithValue("@Trans_Date", Trans_Date);
                        cmd.Parameters.AddWithValue("@error_Message", error_Message);

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SaveTransactionStatus Method");
            }
        }
    }
}