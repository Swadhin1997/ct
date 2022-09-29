using Microsoft.Practices.EnterpriseLibrary.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace PrjPASS
{
    public partial class FrmIIBClaimDataUpload : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session.Timeout = 30;
                //For Testing Purpose
                //Fn_GET_CLAIM_DATA_FROM_XML();
                //clsIIBRequest objIIB = new clsIIBRequest();
                //objIIB.RegNo = "88888888888"; objIIB.ChassisNo = "8848484848484";  objIIB.EngineNo= "7676767678"; objIIB.policyNo = ""; objIIB.insurerName = "test";
                ////objIIB.ClaimObject = new string[4];
                //String[] objClaimDtl = { };
                //IIBWebService.IBWebServicePortTypeClient obj = new IIBWebService.IBWebServicePortTypeClient();
                ////System.Threading.Tasks.Task<IIBWebService.getResultsResponse> res = obj.getResultsAsync("xyz","xyz","hytfdr","78787878787","1324422","hjhjh","hjhjhjio");
                //var res = obj.getResults("xyz", "xyz", ref objIIB.RegNo, ref objIIB.ChassisNo, ref objIIB.EngineNo, objIIB.policyNo, objIIB.insurerName, out objClaimDtl);
                //Fn_Get_Claim_Details();
                //DownloadExcel();
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
            }
            catch (Exception ex)
            {

            }
        }
        public void Fn_Get_Claim_Details()
        {
            try
            {
                clsIIBRequest objIIB = new clsIIBRequest();
                objIIB.RegNo = "AP28DS0871";
                objIIB.ChassisNo = "MALAN51CMDM350048";
                objIIB.EngineNo = "G4LADM987379";
                Fn_GET_IIB_CLAIM_RESPONSE(objIIB);
                //DataTable dtBulkRecord = Fn_GET_DATATABLE_FOR_BULK_INSERT();
                //DataRow drToAddBulkRecord = null;
                //Fn_GET_CLAIM_DATA_FROM_XML("", ref dtBulkRecord);
                //Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
                //using (SqlConnection con = new SqlConnection(dbCOMMON.ConnectionString))
                //{
                //    //create object of SqlBulkCopy which help to insert  
                //    SqlBulkCopy objbulk = new SqlBulkCopy(con);
                //    //assign Destination table name  
                //    objbulk.DestinationTableName = "tbl_IIB_CLAIM_DATA";
                //    Fn_GET_COLUMN_MAPPING_FOR_BULK_INSERT(ref objbulk);
                //    con.Open();
                //    //insert bulk Records into DataBase.  
                //    objbulk.WriteToServer(dtBulkRecord);
                //    con.Close();
                //}
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
            }
        }
        public void Fn_UPLOAD_CLAIM_DATA(DataTable dtExcelData)
        {
            try
            {
                DataTable dtClaimRecord = Fn_GET_DATATABLE_FOR_BULK_INSERT();
                DataTable dtPolicyRecord = Fn_GET_DATATABLE_FOR_POLICY_INSERT();
                DataTable dtClaimRecordError = dtClaimRecord.Clone();
                for (int i = 0; i < dtExcelData.Rows.Count; i++)
                {
                    clsIIBRequest objIIBReq = Fn_GET_IIB_CLAIM_REQUEST(dtExcelData.Rows[i]["RegNo"].ToString(), dtExcelData.Rows[i]["ChassisNo"].ToString(), dtExcelData.Rows[i]["EngineNo"].ToString());
                    string objIIBReqJSONstring = Newtonsoft.Json.JsonConvert.SerializeObject(objIIBReq);
                    ExceptionUtility.LogEvent("Step12: objIIBReqJSONstring: " + objIIBReqJSONstring + DateTime.Now.ToString());
                    string strXML = Fn_GET_IIB_CLAIM_RESPONSE(objIIBReq);
                    Fn_GET_CLAIM_DATA_FROM_XML(strXML, ref dtClaimRecord, ref dtClaimRecordError, ref dtPolicyRecord, dtExcelData.Rows[i]["RegNo"].ToString());
                }
                Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

                if (dtClaimRecord.Rows.Count > 0 || dtClaimRecordError.Rows.Count > 0 || dtPolicyRecord.Rows.Count > 0)
                {
                    using (SqlConnection con = new SqlConnection(dbCOMMON.ConnectionString))
                    {
                        //create object of SqlBulkCopy which help to insert  
                        SqlBulkCopy objbulk = new SqlBulkCopy(con);
                        objbulk.BatchSize = 1000;//Added on 29/10/2020
                        Fn_GET_COLUMN_MAPPING_FOR_BULK_INSERT(ref objbulk);

                        con.Open();

                        if (dtClaimRecord.Rows.Count > 0)
                        {

                            objbulk.DestinationTableName = "tbl_IIB_CLAIM_DATA";
                            objbulk.WriteToServer(dtClaimRecord);

                        }
                        if (dtClaimRecordError.Rows.Count > 0)
                        {

                            objbulk.DestinationTableName = "tbl_IIB_CLAIM_DATA_ERROR";
                            objbulk.WriteToServer(dtClaimRecordError);

                        }
                        if (dtPolicyRecord.Rows.Count > 0)
                        {

                            SqlBulkCopy objbulkPolicy = new SqlBulkCopy(con);
                            objbulkPolicy.BatchSize = 1000;//Added on 29/10/2020
                            Fn_GET_COLUMN_MAPPING_FOR_POLICY_INSERT(ref objbulkPolicy);
                            objbulkPolicy.DestinationTableName = "tbl_IIB_CLAIM_DATA_POLICY";
                            objbulkPolicy.WriteToServer(dtPolicyRecord);
                        }
                        con.Close();

                    }
                }
                else
                {
                    Alert.Show("Record(s) not found.");
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                Alert.Show(ex.Message);
            }

        }
        public DataTable Fn_GET_DATATABLE_FOR_BULK_INSERT()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("ID", typeof(Int32)));
            tbl.Columns.Add(new DataColumn("regNo", typeof(string)));
            tbl.Columns.Add(new DataColumn("chasisNo", typeof(string)));
            tbl.Columns.Add(new DataColumn("engineNo", typeof(string)));
            tbl.Columns.Add(new DataColumn("insurerName", typeof(string)));
            tbl.Columns.Add(new DataColumn("typeOfClaim", typeof(string)));
            tbl.Columns.Add(new DataColumn("dateOfLoss", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("claimIntimationDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ODClaimsPaid", typeof(string)));
            tbl.Columns.Add(new DataColumn("odOpenClaimProvison", typeof(string)));
            tbl.Columns.Add(new DataColumn("odCloseClaimProvison", typeof(string)));
            tbl.Columns.Add(new DataColumn("whetherTotalLossClaim", typeof(string)));
            tbl.Columns.Add(new DataColumn("whetherTheftClaim", typeof(string)));
            tbl.Columns.Add(new DataColumn("totalTPClaimsPaid", typeof(string)));
            tbl.Columns.Add(new DataColumn("tpOpenClaimProvison", typeof(string)));
            tbl.Columns.Add(new DataColumn("claimstatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("expensesPaid", typeof(string)));
            tbl.Columns.Add(new DataColumn("errorCode", typeof(string)));
            tbl.Columns.Add(new DataColumn("warningmessage", typeof(string)));
            tbl.Columns.Add(new DataColumn("dCreatedDate", typeof(DateTime)));
            //Added On 22/10/2020
            tbl.Columns.Add(new DataColumn("vCreatedBy", typeof(string)));


            return tbl;
        }
        public void Fn_GET_DATAROW_FOR_BULK_INSERT(ref DataRow dr)
        {
            //dr["ID"]="";
            dr["regNo"] = "";
            dr["chasisNo"] = "";
            dr["engineNo"] = "";
            dr["insurerName"] = "";
            dr["typeOfClaim"] = "";
            dr["dateOfLoss"] = "";
            dr["claimIntimationDate"] = "";
            dr["ODClaimsPaid"] = "";
            dr["odOpenClaimProvison"] = "";
            dr["odCloseClaimProvison"] = "";
            dr["whetherTotalLossClaim"] = "";
            dr["whetherTheftClaim"] = "";
            dr["totalTPClaimsPaid"] = "";
            dr["tpOpenClaimProvison"] = "";
            dr["claimstatus"] = "";
            dr["expensesPaid"] = "";
            dr["errorCode"] = "";
            dr["warningmessage"] = "";
        }
        public void Fn_GET_COLUMN_MAPPING_FOR_BULK_INSERT(ref SqlBulkCopy objbulk)
        {
            objbulk.ColumnMappings.Add("regNo", "vRegNo");
            objbulk.ColumnMappings.Add("chasisNo", "vChasisNo");
            objbulk.ColumnMappings.Add("engineNo", "vEngineNo");
            objbulk.ColumnMappings.Add("insurerName", "vInsurerName");
            objbulk.ColumnMappings.Add("typeOfClaim", "vTypeOfClaim");
            objbulk.ColumnMappings.Add("dateOfLoss", "dDateOfLoss");
            objbulk.ColumnMappings.Add("claimIntimationDate", "dClaimIntimationDate");
            objbulk.ColumnMappings.Add("ODClaimsPaid", "vODClaimsPaid");
            objbulk.ColumnMappings.Add("odOpenClaimProvison", "vOdOpenClaimProvison");
            objbulk.ColumnMappings.Add("odCloseClaimProvison", "vOdCloseClaimProvison");
            objbulk.ColumnMappings.Add("whetherTotalLossClaim", "vWhetherTotalLossClaim");
            objbulk.ColumnMappings.Add("whetherTheftClaim", "vWhetherTheftClaim");
            objbulk.ColumnMappings.Add("totalTPClaimsPaid", "vTotalTPClaimsPaid");
            objbulk.ColumnMappings.Add("tpOpenClaimProvison", "vTpOpenClaimProvison");
            objbulk.ColumnMappings.Add("claimstatus", "vClaimstatus");
            objbulk.ColumnMappings.Add("expensesPaid", "vExpensesPaid");
            objbulk.ColumnMappings.Add("errorCode", "vErrorCode");
            objbulk.ColumnMappings.Add("warningmessage", "vWarningmessage");
            objbulk.ColumnMappings.Add("dCreatedDate", "dCreatedDate");
            //Added On 22/10/2020
            objbulk.ColumnMappings.Add("vCreatedBy", "vCreatedBy");



        }
        public DataTable Fn_GET_DATATABLE_FOR_POLICY_INSERT()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("regNo", typeof(string)));
            tbl.Columns.Add(new DataColumn("chasisNo", typeof(string)));
            tbl.Columns.Add(new DataColumn("engineNo", typeof(string)));
            tbl.Columns.Add(new DataColumn("policyNo", typeof(string)));
            tbl.Columns.Add(new DataColumn("policyStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("termOfPolicy", typeof(string)));
            tbl.Columns.Add(new DataColumn("has90DaysCrossedAfterExpiryDate", typeof(string)));
            tbl.Columns.Add(new DataColumn("riskExpiryDate", typeof(string)));
            tbl.Columns.Add(new DataColumn("dCreatedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("vCreatedBy", typeof(string)));

            return tbl;
        }
        public void Fn_GET_DATAROW_FOR_POLICY_INSERT(ref DataRow dr)
        {
            //dr["ID"]="";
            dr["regNo"] = "";
            dr["chasisNo"] = "";
            dr["engineNo"] = "";
            dr["policyNo"] = "";
            dr["typeOfClaim"] = "";
            dr["termOfPolicy"] = "";
            dr["has90DaysCrossedAfterExpiryDate"] = "";
            dr["riskExpiryDate"] = "";

        }
        public void Fn_GET_COLUMN_MAPPING_FOR_POLICY_INSERT(ref SqlBulkCopy objbulk)
        {
            objbulk.ColumnMappings.Add("regNo", "regNo");
            objbulk.ColumnMappings.Add("chasisNo", "chasisNo");
            objbulk.ColumnMappings.Add("engineNo", "engineNo");
            objbulk.ColumnMappings.Add("policyNo", "policyNo");
            objbulk.ColumnMappings.Add("policyStatus", "policyStatus");
            objbulk.ColumnMappings.Add("termOfPolicy", "termOfPolicy");
            objbulk.ColumnMappings.Add("has90DaysCrossedAfterExpiryDate", "has90DaysCrossedAfterExpiryDate");
            objbulk.ColumnMappings.Add("riskExpiryDate", "riskExpiryDate");
            objbulk.ColumnMappings.Add("dCreatedDate", "dCreatedDate");
            objbulk.ColumnMappings.Add("vCreatedBy", "vCreatedBy");
        }
        public clsIIBRequest Fn_GET_IIB_CLAIM_REQUEST(string RegNo, string ChassisNo, string EngineNo)
        {
            //IIBWebService.IBWebServicePortTypeClient obj = new IIBWebService.IBWebServicePortTypeClient();
            clsIIBRequest objIIBReq = new clsIIBRequest();
            objIIBReq.UserName = "";
            objIIBReq.Password = "";
            objIIBReq.RegNo = RegNo;
            objIIBReq.ChassisNo = ChassisNo;
            objIIBReq.EngineNo = EngineNo;
            objIIBReq.policyNo = "";
            objIIBReq.insurerName = "";
            return objIIBReq;
        }
        public string Fn_GET_IIB_CLAIM_RESPONSE(clsIIBRequest objclsIIB)
        {
            try
            {
                //Calling CreateSOAPWebRequest method  
                //string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "IIBGetRequest.XML";
                //XmlNode node = null;
                //XmlDocument xmlfile = null;
                //string xmlString = string.Empty;
                //xmlfile = new XmlDocument();
                //xmlfile.Load(strXmlPath);
                objclsIIB.UserName = ConfigurationManager.AppSettings["IIB_Claim_Serv_UserName"].ToString();
                objclsIIB.Password = ConfigurationManager.AppSettings["IIB_Claim_Serv_Password"].ToString();
                DataTable oDt = new DataTable();//Your DataTable which you want to convert

                //HttpWebRequest request = CreateSOAPWebRequest();
                WebRequest request = CreateSOAPWebRequest2();

                //request.AllowAutoRedirect = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                XmlDocument SOAPReqBody = new XmlDocument();


                //SOAP Body Request      
                SOAPReqBody.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:iib=""http://localhost:9096/IIBWebService/"">
                                <soapenv:Header/>
                                <soapenv:Body>
                                    <iib:getResults>
                                        <UserName>@UID</UserName>
                                        <Password>@PSWD</Password>
                                        <RegNo>@REGNO</RegNo>
                                        <ChassisNo>@CHSNO</ChassisNo>
                                        <EngineNo>@ENO</EngineNo>
                                        <policyNo></policyNo>
                                        <insurerName></insurerName>
                                    </iib:getResults >
                                </soapenv:Body >
                            </soapenv:Envelope>".Replace("@UID", objclsIIB.UserName).Replace("@PSWD", objclsIIB.Password)
                                .Replace("@REGNO", objclsIIB.RegNo).Replace("@CHSNO", objclsIIB.ChassisNo)
                                .Replace("@ENO", objclsIIB.EngineNo));

                var XMLREQ = SOAPReqBody.InnerXml;
                ExceptionUtility.LogEvent("XMLREQ " + XMLREQ + DateTime.Now.ToString());


                //StreamWriter requestWriter = new StreamWriter(myWebRequest.GetRequestStream());
                //requestWriter.Write(postString);
                //requestWriter.Close();
                StreamWriter stream = new StreamWriter(request.GetRequestStream());
                stream.Write(XMLREQ);
                stream.Close();

                //using (Stream stream = request.GetRequestStream())
                //{
                //    stream.Write()
                //    SOAPReqBody.Save(stream);
                //}
                //Geting response from request  
                var ServiceResult = string.Empty;
                using (WebResponse Serviceres = request.GetResponse())
                {
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
                    //                   SecurityProtocolType.Tls11 |
                    //                   SecurityProtocolType.Tls |
                    //                   SecurityProtocolType.Ssl3;

                    using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                    {
                        //reading stream  
                        ServiceResult = rd.ReadToEnd();
                        //writting stream result on console  
                        //Console.WriteLine(ServiceResult);
                        //Console.ReadLine();
                        //writting stream result on Datatable
                        //oDt.WriteXml(ServiceResult);
                        return ServiceResult;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");

                Alert.Show(ex.Message);
                return "";
            }
            //return oDt.Rows[0];

        }
        public HttpWebRequest CreateSOAPWebRequest1()
        {
            string URL = ConfigurationManager.AppSettings["IIB_Claim_Serv_ReqURL"].ToString();
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(URL);

            //Making Web Request  
            if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "0")
            {
                //SOAPAction  
                Req.Headers.Add(@"SOAPAction:http://tempuri.org/Addition");
                //Content_type  
                Req.ContentType = "text/xml;charset=\"utf-8\"";
                Req.Accept = "text/xml";
                //HTTP method  
                Req.Method = "POST";
            }
            else
            {
                //SOAPAction  
                Req.Headers.Add(@"SOAPAction:http://tempuri.org/Addition");
                //Content_type  
                Req.ContentType = "text/xml;charset=\"utf-8\"";
                Req.Accept = "text/xml";
                //HTTP method  
                Req.Method = "POST";

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

                //string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);

                //string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                Req.Proxy = proxy;
                Req.Proxy = WebRequest.DefaultWebProxy;
                Req.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                Req.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);

            }
            //return HttpWebRequest  
            return Req;

        }

        private WebRequest CreateSOAPWebRequest2()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager

                string url = ConfigurationManager.AppSettings["IIB_Claim_Serv_ReqURL"].ToString();

                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";

                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Timeout = Timeout.Infinite;
                //request.KeepAlive = true;
                request.Proxy = null;

                //Making Web Request  
                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "1")
                {
                    WebProxy proxy = new WebProxy(ConfigurationManager.AppSettings["proxy_Url"].ToString());

                    string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                    string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();
                    string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();


                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;
                    request.Proxy = proxy;


                    request.Proxy = WebRequest.DefaultWebProxy;
                    request.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    request.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                }
                return request;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                return null;
            }

        }



        #region Create SOAP Web Request With Proxy

        private HttpWebRequest CreateSOAPWebRequest()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager

                string url = ConfigurationManager.AppSettings["IIB_Claim_Serv_ReqURL"].ToString();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Timeout = Timeout.Infinite;
                request.KeepAlive = true;
                request.Proxy = null;

                //Making Web Request  
                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "1")
                {
                    WebProxy proxy = new WebProxy(ConfigurationManager.AppSettings["proxy_Url"].ToString());

                    string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                    string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();
                    string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();


                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;
                    request.Proxy = proxy;


                    request.Proxy = WebRequest.DefaultWebProxy;
                    request.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    request.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                }
                return request;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                return null;
            }

        }

        private WebRequest CreateSOAPWebRequest2_old()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; //Added ServicePointManager

                string url = ConfigurationManager.AppSettings["IIB_Claim_Serv_ReqURL"].ToString();

                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";

                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Timeout = Timeout.Infinite;
                //request.KeepAlive = true;
                request.Proxy = null;

                //Making Web Request  
                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "1")
                {
                    WebProxy proxy = new WebProxy(ConfigurationManager.AppSettings["proxy_Url"].ToString());

                    string network_Username = ConfigurationManager.AppSettings["network_Username"].ToString();
                    string network_Password = ConfigurationManager.AppSettings["network_Password"].ToString();
                    string network_domain = ConfigurationManager.AppSettings["network_domain"].ToString();


                    proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;
                    request.Proxy = proxy;


                    request.Proxy = WebRequest.DefaultWebProxy;
                    request.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    request.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                }
                return request;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                return null;
            }

        }


        #endregion
        public void Fn_GET_CLAIM_DATA_FROM_XML(string XmlString, ref DataTable dtClaim, ref DataTable dtClaimError, ref DataTable dtPolicy, string RegNo)
        {
            try
            {
                //XmlString = GetResponseForTesting();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(XmlString); // suppose that myXmlString contains "<Names>...</Names>"

                while (xmlDoc.DocumentElement.Name == "soapenv:Envelope" || xmlDoc.DocumentElement.Name == "soapenv:Body")
                {
                    string tempXmlString = xmlDoc.DocumentElement.InnerXml;
                    xmlDoc.LoadXml(tempXmlString);
                }
                XmlNamespaceManager xmlnsManager = new System.Xml.XmlNamespaceManager(xmlDoc.NameTable);

                xmlnsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                xmlnsManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xmlnsManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
                xmlnsManager.AddNamespace("ns2", "http://localhost/IIBWebService/");

                //You'd access the full path like this
                //XmlNode node = xmlDoc.SelectSingleNode("/soapenv:Envelope/soapenv:Body/ns2:getResultsResponse/si:FirstName", xmlnsManager);
                //Fn_GET_DATAROW_FOR_BULK_INSERT(ref drU);
                try
                {
                    //XmlNodeList xnChildNodes = xmlDoc.SelectNodes("/ns2:getResultsResponse/ClaimDetails/output", xmlnsManager);
                    //int NodeCount = xnChildNodes.Count;
                    #region Claim
                    int i = 0;
                    bool IsClaimFound = false;
                    XmlNodeList xnList = null;
                    do
                    {
                        i++;
                        xnList = xmlDoc.SelectNodes("/ns2:getResultsResponse/ClaimDetails/output/claimDetails" + i.ToString(), xmlnsManager);
                        //XmlNode node = xmlDoc.SelectSingleNode("/soap:Envelope/soap:Body/si:LoginResponse/si:FirstName", xmlnsManager);
                        if (xnList != null && xnList.Count > 0)
                        {
                            foreach (XmlNode xn in xnList)
                            {
                                IsClaimFound = true;
                                DataRow dr = dtClaim.NewRow();

                                dr["regNo"] = xn["regNo" + i.ToString()] == null ? "" : xn["regNo" + i.ToString()].InnerText;
                                dr["chasisNo"] = xn["chasisNo" + i.ToString()] == null ? "" : xn["chasisNo" + i.ToString()].InnerText;
                                dr["engineNo"] = xn["engineNo" + i.ToString()] == null ? "" : xn["engineNo" + i.ToString()].InnerText;
                                dr["insurerName"] = xn["insurerName" + i.ToString()] == null ? "" : xn["insurerName" + i.ToString()].InnerText;
                                dr["typeOfClaim"] = xn["typeOfClaim" + i.ToString()] == null ? "" : xn["typeOfClaim" + i.ToString()].InnerText;
                                dr["dateOfLoss"] = xn["dateOfLoss" + i.ToString()] == null ? DBNull.Value.ToString() : xn["dateOfLoss" + i.ToString()].InnerText;
                                dr["claimIntimationDate"] = xn["claimIntimationDate" + i.ToString()] == null ? DBNull.Value.ToString() : xn["claimIntimationDate" + i.ToString()].InnerText;
                                dr["ODClaimsPaid"] = xn["ODClaimsPaid" + i.ToString()] == null ? "" : xn["ODClaimsPaid" + i.ToString()].InnerText;
                                dr["odOpenClaimProvison"] = xn["odOpenClaimProvison" + i.ToString()] == null ? "" : xn["odOpenClaimProvison" + i.ToString()].InnerText;
                                dr["odCloseClaimProvison"] = xn["odCloseClaimProvison" + i.ToString()] == null ? "" : xn["odCloseClaimProvison" + i.ToString()].InnerText;
                                dr["whetherTotalLossClaim"] = xn["whetherTotalLossClaim" + i.ToString()] == null ? "" : xn["whetherTotalLossClaim" + i.ToString()].InnerText;
                                dr["whetherTheftClaim"] = xn["whetherTheftClaim" + i.ToString()] == null ? "" : xn["whetherTheftClaim" + i.ToString()].InnerText;
                                dr["totalTPClaimsPaid"] = xn["totalTPClaimsPaid" + i.ToString()] == null ? "" : xn["totalTPClaimsPaid" + i.ToString()].InnerText;
                                dr["tpOpenClaimProvison"] = xn["tpOpenClaimProvison" + i.ToString()] == null ? "" : xn["tpOpenClaimProvison" + i.ToString()].InnerText;
                                dr["claimstatus"] = xn["claimstatus" + i.ToString()] == null ? "" : xn["claimstatus" + i.ToString()].InnerText;
                                dr["expensesPaid"] = xn["expensesPaid" + i.ToString()] == null ? "" : xn["expensesPaid" + i.ToString()].InnerText;
                                dr["errorCode"] = "";//xn["errorCode" + i.ToString()].InnerText;
                                dr["warningmessage"] = "";// xn["warningmessage" + i.ToString()].InnerText;
                                dr["dCreatedDate"] = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
                                //Added On 22/10/2020
                                dr["vCreatedBy"] = Convert.ToString(Session["vUserLoginId"]).ToUpper();
                                dtClaim.Rows.Add(dr);
                            }
                        }
                        //Added on 30/10/2020
                        else
                        {
                            string errorcode = "";
                            string warning = "";
                            string message = "";
                            if (IsClaimFound == false)
                            {
                                //xnList = xmlDoc.SelectNodes("/ns2:getResultsResponse/ClaimDetails/output/errorCode", xmlnsManager);
                                XmlNodeList xnErrorCode = xmlDoc.SelectNodes("/ns2:getResultsResponse/ClaimDetails/output/errorCode", xmlnsManager);
                                XmlNodeList xnWarning = xmlDoc.SelectNodes("/ns2:getResultsResponse/ClaimDetails/output/warning", xmlnsManager);
                                XmlNodeList xnMessage = xmlDoc.SelectNodes("/ns2:getResultsResponse/ClaimDetails/output/message", xmlnsManager);
                                if (xnErrorCode != null && xnErrorCode.Count > 0)
                                {
                                    errorcode = xnErrorCode[0].InnerText;
                                }
                                if (xnWarning != null && xnWarning.Count > 0)
                                {
                                    warning = xnWarning[0].InnerText;
                                }
                                if (xnMessage != null && xnMessage.Count > 0)
                                {
                                    message = xnMessage[0].InnerText;
                                }
                                //foreach (XmlNode xn in xnList)
                                //{
                                DataRow dr = dtClaimError.NewRow();

                                dr["regNo"] = RegNo;
                                dr["chasisNo"] = "NA";
                                dr["engineNo"] = "NA";
                                dr["insurerName"] = "NA";
                                dr["typeOfClaim"] = "NA";
                                dr["dateOfLoss"] = DBNull.Value;
                                dr["claimIntimationDate"] = DBNull.Value;
                                dr["ODClaimsPaid"] = "";
                                dr["odOpenClaimProvison"] = "NA";
                                dr["odCloseClaimProvison"] = "NA";
                                dr["whetherTotalLossClaim"] = "N";
                                dr["whetherTheftClaim"] = "NA";
                                dr["totalTPClaimsPaid"] = "NA";
                                dr["tpOpenClaimProvison"] = "NA";
                                dr["claimstatus"] = "NA";
                                dr["expensesPaid"] = "NA";
                                dr["errorCode"] = errorcode;
                                dr["warningmessage"] = warning == "" ? message : warning;
                                dr["dCreatedDate"] = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
                                //Added On 22/10/2020
                                dr["vCreatedBy"] = Convert.ToString(Session["vUserLoginId"]).ToUpper();
                                dtClaimError.Rows.Add(dr);
                                //}
                            }
                        }
                        //End
                    } while (xnList.Count > 0);
                    #endregion Claim

                    #region Policy
                    i = 0;
                    //XmlNodeList xnList=null;
                    do
                    {
                        i++;
                        xnList = xmlDoc.SelectNodes("/ns2:getResultsResponse/ClaimDetails/output/policyDetails/policyDetails" + i.ToString(), xmlnsManager);
                        //XmlNode node = xmlDoc.SelectSingleNode("/soap:Envelope/soap:Body/si:LoginResponse/si:FirstName", xmlnsManager);
                        if (xnList != null && xnList.Count > 0)
                        {
                            foreach (XmlNode xn in xnList)
                            {
                                DataRow dr = dtPolicy.NewRow();

                                dr["regNo"] = RegNo;
                                dr["chasisNo"] = "";
                                dr["engineNo"] = "";
                                dr["policyNo"] = xn["policyNo" + i.ToString()] == null ? "" : xn["policyNo" + i.ToString()].InnerText;
                                dr["policyStatus"] = xn["policyStatus" + i.ToString()] == null ? "" : xn["policyStatus" + i.ToString()].InnerText;
                                dr["termOfPolicy"] = xn["termOfPolicy" + i.ToString()] == null ? DBNull.Value.ToString() : xn["termOfPolicy" + i.ToString()].InnerText;
                                dr["has90DaysCrossedAfterExpiryDate"] = xn["has90DaysCrossedAfterExpiryDate" + i.ToString()] == null ? DBNull.Value.ToString() : xn["has90DaysCrossedAfterExpiryDate" + i.ToString()].InnerText;
                                dr["riskExpiryDate"] = xn["riskExpiryDate"] == null ? "" : xn["riskExpiryDate"].InnerText;
                                dr["dCreatedDate"] = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
                                dr["vCreatedBy"] = Convert.ToString(Session["vUserLoginId"]).ToUpper();
                                dtPolicy.Rows.Add(dr);
                            }
                        }
                    } while (xnList.Count > 0);

                    #endregion Policy

                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");

                Alert.Show(ex.Message);
            }

        }
        public string GetResponseForTesting()
        {
            string strResponse = @"
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
   <soapenv:Body>
      <ns2:getResultsResponse xmlns:ns2=""http://localhost/IIBWebService/"">
         <Authentication>Authenticated</Authentication>
         <RegNo>AP05TU3645</RegNo>
         <ChassisNo>28518</ChassisNo>
         <EngineNo>193914</EngineNo>
         <ClaimDetails>
            <output>
               <claimDetails1>
                  <regNo1>AP05TU3645</regNo1>
                  <chasisNo1>28518</chasisNo1>
                  <engineNo1>193914</engineNo1>
                  <insurerName1>HDFC ERGO General Insurance Co. Ltd.</insurerName1>
                  <typeOfClaim1>OD</typeOfClaim1>
                  <dateOfLoss1>2013-01-07</dateOfLoss1>
                  <claimIntimationDate1>2013-01-10</claimIntimationDate1>
                  <ODClaimsPaid1>80000.5</ODClaimsPaid1>
                  <odOpenClaimProvison1>105241</odOpenClaimProvison1>
                  <odCloseClaimProvison1>0</odCloseClaimProvison1>
                  <whetherTotalLossClaim1>No</whetherTotalLossClaim1>
                  <whetherTheftClaim1>No</whetherTheftClaim1>
                  <totalTPClaimsPaid1>0.0</totalTPClaimsPaid1>
                  <tpOpenClaimProvison1>0</tpOpenClaimProvison1>
                  <tpCloseClaimProvison1>0</tpCloseClaimProvison1>
                  <claimstatus1>NA</claimstatus1>
                  <expensesPaid1>6325</expensesPaid1>
		<searchBasedOn1>CHASSIS_NUMBER</searchBasedOn1>
               </claimDetails1>
               <claimDetails2>
                  <regNo2>AP05TU3645</regNo2>
                  <chasisNo2>28518</chasisNo2>
                  <engineNo2>193914</engineNo2>
                  <insurerName2>HDFC ERGO General Insurance Co. Ltd.</insurerName2>
                  <typeOfClaim2>TP</typeOfClaim2>
                  <dateOfLoss2>2013-01-07</dateOfLoss2>
                  <claimIntimationDate2>2013-08-19</claimIntimationDate2>
                  <ODClaimsPaid2>0.0</ODClaimsPaid2>
                  <odOpenClaimProvison2>0</odOpenClaimProvison2>
                  <odCloseClaimProvison2>0</odCloseClaimProvison2>
                  <whetherTotalLossClaim2>NA</whetherTotalLossClaim2>
                  <whetherTheftClaim2>NA</whetherTheftClaim2>
                  <totalTPClaimsPaid2>3500.0</totalTPClaimsPaid2>
                  <tpOpenClaimProvison2>611500</tpOpenClaimProvison2>
                  <tpCloseClaimProvison2>261500</tpCloseClaimProvison2>
                  <claimstatus2>OPEN</claimstatus2>
                  <expensesPaid2>0</expensesPaid2>
<searchBasedOn1>CHASSIS_NUMBER</searchBasedOn1>
               </claimDetails2>
               <claimDetails3>
                  <regNo3>AP05TU3645</regNo3>
                  <chasisNo3>28518</chasisNo3>
                  <engineNo3>193914</engineNo3>
                  <insurerName3>HDFC ERGO General Insurance Co. Ltd.</insurerName3>
                  <typeOfClaim3>OD</typeOfClaim3>
                  <dateOfLoss3>2013-05-08</dateOfLoss3>
                  <claimIntimationDate3>2013-05-09</claimIntimationDate3>
                  <ODClaimsPaid3>160000.0</ODClaimsPaid3>
                  <odOpenClaimProvison3>0</odOpenClaimProvison3>
                  <odCloseClaimProvison3>0</odCloseClaimProvison3>
                  <whetherTotalLossClaim3>Yes</whetherTotalLossClaim3>
                  <whetherTheftClaim3>No</whetherTheftClaim3>
                  <totalTPClaimsPaid3>0.0</totalTPClaimsPaid3>
                  <tpOpenClaimProvison3>0</tpOpenClaimProvison3>
                  <tpCloseClaimProvison3>0</tpCloseClaimProvison3>
                  <claimstatus3>NA</claimstatus3>
                  <expensesPaid3>8469</expensesPaid3>
<searchBasedOn1>CHASSIS_NUMBER</searchBasedOn1>
               </claimDetails3>
               <policyDetails>Policy Details not available for the provided details</policyDetails>
            </output>
         </ClaimDetails>
      </ns2:getResultsResponse>
   </soapenv:Body>
</soapenv:Envelope>";
            return strResponse;
        }

        [DataContract]
        public class clsIIBRequest
        {
            [DataMember]
            public string UserName { get; set; }
            [DataMember]
            public string Password { get; set; }
            [DataMember]
            public string RegNo;
            [DataMember]
            public string ChassisNo;
            [DataMember]
            public string EngineNo;
            [DataMember]
            public string policyNo;
            [DataMember]
            public string insurerName;
            [DataMember]
            public object ClaimObject;
        }
        public class clsIIBResponse
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string RegNo;
            public string ChassisNo;
            public string EngineNo;
            public string policyNo;
            public string insurerName;
            public object ClaimObject;
        }

        #region Excel Operation
        protected void Upload(object sender, EventArgs e)
        {
            ExceptionUtility.LogEvent("Step1 " + DateTime.Now.ToString());

            System.Threading.Thread.Sleep(3000);
            string allowedExtensions = ".xlsx";


            if (String.IsNullOrEmpty(FileUpload1.FileName.ToString()))
            {
                Alert.Show("Please Select valid excel file or Excel file not selected!");
                return;
            }

            if (!FileUpload1.HasFile)
            {
                Alert.Show("Please select the file");
                //Session["ErrorCallingPage"] = "FrmGPAClaimsUpload.aspx";
                string vStatusMsg = "Please select valid excel file for upload";
                //Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
            }


            if (FileUpload1.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                if (fileExtension != allowedExtensions)
                {

                    Alert.Show("Invalid file Extension, Only XLSX files are allowed to be Uploaded");
                    //Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                    return;
                }
            }

           
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            SqlConnection _con = new SqlConnection(dbCOMMON.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();

           
            try
            {
                //Upload and save the file
                btnImport.Enabled = false;
                string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (System.IO.File.Exists(excelPath))
                {
                    Alert.Show("File already exists");
                    return;
                }

                FileUpload1.SaveAs(excelPath);
               
                string conString = string.Empty;
                string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;

                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;
                }

                conString = string.Format(conString, excelPath);

                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    DataTable dtExcelData = new DataTable();
                    bool GetMappingData = false;
                    string sqlCommand = "SELECT  * FROM TBL_IIB_CLAIM_COLUMN_MAPPING_MASTER where bExcludeForUpload='N'";
                    DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                    DataSet ds = null;
                    ds = dbCOMMON.ExecuteDataSet(dbCommand);

                    ExceptionUtility.LogEvent("Step6 " + DateTime.Now.ToString());

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GetMappingData = true;
                    }

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }

                    excel_con.Close();

                   
                    if (dtExcelData.Rows.Count > 0)
                    {
                        dtExcelData.Columns.Add("vErrorFlag");
                        dtExcelData.Columns.Add("vErrorDesc");
                        dtExcelData.Columns.Add("vTransType");


                        foreach (DataRow excelrow in dtExcelData.Rows)
                        {
                            ExceptionUtility.LogEvent("Step8 " + DateTime.Now.ToString());
                            string vDestinationFieldName = "";
                            string vSourceFieldName = "";
                            string vFieldValue = "";
                            string vErrorDesc = "";
                            string bMandatoryForClaims = "";
                            string[] chkValidFlag;

                            if (GetMappingData == true)
                            {
                                bool insertflag = true;

                                for (int i = 1; i <= dtExcelData.Columns.Count; i++)
                                {

                                    string searchExpression = "vSourceColumnName = '" + dtExcelData.Columns[i - 1].ColumnName.ToString().Trim() + "'";

                                    DataRow[] foundRows = ds.Tables[0].Select(searchExpression);

                                    if (foundRows.Count() > 0)
                                    {
                                        vDestinationFieldName = foundRows[0]["vDestinationColumnName"].ToString();
                                        vSourceFieldName = foundRows[0]["vSourceColumnName"].ToString();
                                        vFieldValue = excelrow[dtExcelData.Columns[i - 1].ColumnName.ToString().Trim()].ToString();

                                        chkValidFlag = Fn_Check_Business_Validation(vDestinationFieldName, vFieldValue);
                                        if (vSourceFieldName == "RegNo")
                                        {
                                            if (String.IsNullOrEmpty(vFieldValue))
                                            {
                                                chkValidFlag[0] = "false";
                                                chkValidFlag[1] = "RegNo is mandatory";
                                            }
                                            else
                                            {
                                                if (vFieldValue.ToUpper().Trim() == "NEW" || vFieldValue.ToUpper().Trim() == "AWAITED")
                                                {
                                                    chkValidFlag[0] = "false";
                                                    chkValidFlag[1] = "New and Awaited status not allowed for RegNo.";
                                                }

                                            }
                                        }

                                        if (chkValidFlag[0] == "false")
                                        {
                                            insertflag = false;
                                            vErrorDesc = vErrorDesc + chkValidFlag[1].ToString();
                                        }
                                    }
                                }
                                if (insertflag == false)
                                {
                                    excelrow["vTransType"] = "PUP";
                                    excelrow["vErrorFlag"] = "Y";
                                    excelrow["vErrorDesc"] = vErrorDesc;
                                }
                                else
                                {
                                    excelrow["vTransType"] = "PUP";
                                    excelrow["vErrorFlag"] = "N";
                                    excelrow["vErrorDesc"] = "";
                                }
                            }
                        }

                        DataTable dtValidatedExcelData = null;
                        DataTable dtUploadErrorLog = null;
                        string searchExpressionPass = "vErrorFlag = 'N'";
                        DataRow[] foundRows1 = dtExcelData.Select(searchExpressionPass);
                        if (foundRows1.Count() > 0)
                        {
                            dtValidatedExcelData = dtExcelData.Select(searchExpressionPass).CopyToDataTable();
                        }
                        string searchExpressionFail = "vErrorFlag = 'Y'";
                        DataRow[] foundRows2 = dtExcelData.Select(searchExpressionFail);
                        if (foundRows2.Count() > 0)
                        {
                            dtUploadErrorLog = dtExcelData.Select(searchExpressionFail).CopyToDataTable();
                        }

                        if (dtUploadErrorLog != null)
                        {
                            if (dtUploadErrorLog.Rows.Count > 0)
                            {
                                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                                using (SqlConnection con = new SqlConnection(consString))
                                {
                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                    {
                                        ////Set the database table name

                                        //sqlBulkCopy.DestinationTableName = "dbo.TBL_IIB_CLAIM_DATA_ERROR_LOG";

                                        ////[OPTIONAL]: Map the Excel columns with that of the database table

                                        ////Getting Columns and Mapping from the Mapping Table

                                        //sqlCommand = "SELECT  * FROM TBL_IIB_CLAIM_DATA_COLUMN_MAPPING_MASTER where bExcludeForClaimsUpload='N'";
                                        //#region Implement for Log
                                        ////dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        ////ds = null;
                                        ////ds = dbCOMMON.ExecuteDataSet(dbCommand);

                                        ////if (ds.Tables[0].Rows.Count > 0)
                                        ////{
                                        ////    sqlBulkCopy.ColumnMappings.Add("vTransType", "vTransType");
                                        ////    sqlBulkCopy.ColumnMappings.Add("vErrorFlag", "vErrorFlag");
                                        ////    sqlBulkCopy.ColumnMappings.Add("vErrorDesc", "vErrorDesc");

                                        ////    foreach (DataRow row in ds.Tables[0].Rows)
                                        ////    {
                                        ////        sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                                        ////    }
                                        ////}
                                        ////con.Open();
                                        ////sqlBulkCopy.WriteToServer(dtUploadErrorLog);
                                        //con.Close();
                                        #endregion
                                    }
                                }
                            }
                        }

                        if (dtValidatedExcelData != null)
                        {
                            if (dtValidatedExcelData.Rows.Count > 0)
                            {
                                ExceptionUtility.LogEvent("Step10 " + DateTime.Now.ToString());
                                Fn_UPLOAD_CLAIM_DATA(dtValidatedExcelData);
                                ExceptionUtility.LogEvent("Step23 " + DateTime.Now.ToString());
                            }
                            Alert.Show("Data uploaded successfully.");
                            btnImport.Enabled = true;
                        }
                        else
                        {
                            Alert.Show("No valid records found!");
                        }
                    }
                    else
                    {
                        Alert.Show("No records found!");
                    }
                }
                _tran.Commit();

                ExceptionUtility.LogEvent("Step24 " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                _tran.Rollback();

                ExceptionUtility.LogEvent("Step25 " + DateTime.Now.ToString());
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");

                Alert.Show(ex.Message);
                btnImport.Enabled = true;
                //log the error
            }
            finally
            {
                btnImport.Enabled = true;
                _con.Close();
            }
        }
        protected string[] Fn_Check_Business_Validation(string vFieldName, string vFieldValue)
        {
            string[] ckvalidflag = new string[2];
            ckvalidflag[0] = "true";
            ckvalidflag[1] = " ";

            //if (vFieldName == "vCertificateNo")
            //{
            //    if (vFieldValue.Length < 5)
            //    {
            //        ckvalidflag[0] = "false";
            //        ckvalidflag[1] = "Policy Id Lenght is Less then 5";
            //    }
            //}

            return ckvalidflag;
        }
        protected void btnDownloadLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmIIBClaimDataDownload.aspx");

        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
        public bool DownloadFile(string strfilename)
        {
            string filePath = Server.MapPath("~/Reports");
            string _DownloadableProductFileName = strfilename;

            System.IO.FileInfo FileName = new System.IO.FileInfo(strfilename);
            FileStream myFile = new FileStream(strfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //Reads file as binary values
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            long startBytes = 0;
            string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
            string _EncodedData = HttpUtility.UrlEncode
                (_DownloadableProductFileName, Encoding.UTF8) + lastUpdateTiemStamp;

            //Clear the content of the response
            Response.Clear();
            Response.Buffer = false;
            Response.AddHeader("Accept-Ranges", "bytes");
            Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
            Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);

            //Set the ContentType
            Response.ContentType = "application/octet-stream";

            //Add the file name and attachment, 
            //which will force the open/cancel/save dialog to show, to the header
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName.Name);

            //Add the file size into the response header
            Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
            Response.AddHeader("Connection", "Keep-Alive");

            //Set the Content Encoding type
            Response.ContentEncoding = Encoding.UTF8;

            //Send data
            _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

            //Dividing the data in 1024 bytes package
            int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

            //Download in block of 1024 bytes
            int i;
            for (i = 0; i < maxCount && Response.IsClientConnected; i++)
            {
                Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                Response.Flush();
            }

            //compare packets transferred with total number of packets
            if (i < maxCount) return false;
            return true;

            //Close Binary reader and File stream
            _BinaryReader.Close();
            myFile.Close();
        }

        public void DownloadExcel()
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            string sqlCommand = "SELECT * FROM tbl_IIB_CLAIM_DATA";
            DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
            DataSet dsColumnNames = null;
            dsColumnNames = dbCOMMON.ExecuteDataSet(dbCommand);
            DataTable dt = dsColumnNames.Tables[0];
            string attachment = "attachment; filename=CLAIM_DATA.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            string sqlCommand = "SELECT * FROM TBL_IIB_CLAIM_COLUMN_MAPPING_MASTER";
            DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
            DataSet dsColumnNames = null;
            dsColumnNames = dbCOMMON.ExecuteDataSet(dbCommand);

            if (dsColumnNames.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsColumnNames.Tables[0].Rows)
                {
                    TemplateTable.Columns.Add(row["vSourceColumnName"].ToString());
                }
            }

            DataRow newBlankRow1 = TemplateTable.NewRow();
            TemplateTable.Rows.InsertAt(newBlankRow1, 0);

            string filePath = Server.MapPath("~/Reports");

            string _DownloadableProductFileName = "IIB_CLAIM_DATA_UPLOAD_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "IIB_CLAIM_DATA_UPLOAD_TEMPLATE", strfilename) == true)
            {
                DownloadFile(strfilename);
            }
        }


        #region Download
        private static bool DataSetToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation, string logfile)
        {
            //File.AppendAllText(Properties.Settings.Default.logfile + logfile, "Status  : entered into fn DataSetToExcel" + DateTime.Now + Environment.NewLine);
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    dataTable.TableName = "Download_Log_Records";
                    ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dataTable.TableName);
                    workSheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;
                    pck.SaveAs(new FileInfo(saveAsLocation));
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                //File.AppendAllText(Properties.Settings.Default.logfile + logfile, "Error occured  : " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                return false;
            }
        }
        private static bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        {
            //Creae an Excel application instance
            //File.AppendAllText(Properties.Settings.Default.logfile + logfile, "Status  : entered into fn ExportDataTableToExcel" + DateTime.Now + Environment.NewLine);
            ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
            ExcelRange oRng;

            try
            {

                // Workk sheet
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheet.Name = worksheetName;
                int rowcount = 0;
                if (worksheetName.ToUpper() != "IIB_CLAIM_DATA_UPLOAD_TEMPLATE")
                {
                    rowcount = 2;
                    excelSheet.Cells[1, 1].Value = "CLAIM Download Details";
                    excelSheet.Cells[2, 1].Value = "Report Taken On Date : " + DateTime.Now.ToShortDateString();
                }
                else
                {
                    rowcount = 0;
                }
                // loop through each row and add values to our sheet



                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;

                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 1)
                        {
                            excelSheet.Cells[1, i].Value = dataTable.Columns[i - 1].ColumnName;
                            // excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
                        }

                        excelSheet.Cells[rowcount + 1, i].Value = datarow[i - 1].ToString();

                        //for alternate rows
                        if (rowcount > 2)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    oRng = (ExcelRange)excelSheet.Cells[rowcount, dataTable.Columns.Count];
                                    //FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
                                }
                            }
                        }
                    }
                }

                // now we resize the columns
                oRng = (ExcelRange)excelSheet.Cells[3, dataTable.Columns.Count];
                oRng.AutoFitColumns();
                BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));
                rowcount = 1;
                for (int col = 1; col <= dataTable.Columns.Count; col++)
                {

                    oRng = (ExcelRange)excelSheet.Cells[rowcount, col];
                    FormattingExcelCells(oRng, "#873260", System.Drawing.Color.White, true);
                }


                //now save the workbook and exit Excel

                excelApp.Save();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "FrmIIBClaimDataUpload");
                //  Alert.Show(ex.Message);
                //File.AppendAllText(Properties.Settings.Default.logfile + logfile, "Error occured  : " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                return false;
            }
            finally
            {
                excelApp = null;
            }
        }
        private static void BorderAround(ExcelRange range, int colour)
        {
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.ColorTranslator.FromOle(colour));
        }
        public static void FormattingExcelCells(ExcelRange range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.Indexed = 16;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
            range.Style.Font.Size = 12;
            //range.Style.Font.Color = System.Drawing.Color.White;
            if (IsFontbool == true)
            {
                range.Style.Font.Bold = IsFontbool;
            }
        }

        #endregion

        #region Existing Excel Generate Method
        //private bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        //{
        //    //Creae an Excel application instance

        //    ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
        //    ExcelRange oRng;

        //    try
        //    {

        //        // Workk sheet
        //        var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
        //        excelSheet.Name = worksheetName;

        //        //excelSheet.Cells[1, 1] = "Detail";
        //        //excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

        //        // loop through each row and add values to our sheet
        //        int rowcount = 0;


        //        foreach (DataRow datarow in dataTable.Rows)
        //        {
        //            rowcount += 1;

        //            for (int i = 1; i <= dataTable.Columns.Count; i++)
        //            {
        //                // on the first iteration we add the column headers
        //                if (rowcount == 1)
        //                {
        //                    excelSheet.Cells[1, i].Value = dataTable.Columns[i - 1].ColumnName;
        //                    // excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
        //                }

        //                excelSheet.Cells[rowcount + 1, i].Value = datarow[i - 1].ToString();

        //                //for alternate rows
        //                if (rowcount > 1)
        //                {
        //                    if (i == dataTable.Columns.Count)
        //                    {
        //                        if (rowcount % 2 == 0)
        //                        {
        //                            oRng = (ExcelRange)excelSheet.Cells[rowcount, dataTable.Columns.Count];
        //                            FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        // now we resize the columns
        //        oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
        //        oRng.AutoFitColumns();
        //        BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));

        //        oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
        //        FormattingExcelCells(oRng, "#000099", System.Drawing.Color.White, true);
        //        //now save the workbook and exit Excel

        //        excelApp.Save();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Alert.Show(ex.Message);
        //        return false;
        //    }
        //    finally
        //    {
        //        excelApp = null;
        //    }
        //}
        //private void BorderAround(ExcelRange range, int colour)
        //{
        //    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.ColorTranslator.FromOle(colour));
        //}
        //public void FormattingExcelCells(ExcelRange range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        //{
        //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    range.Style.Fill.BackgroundColor.Indexed = 16;
        //    range.Style.Font.Size = 12;
        //    //range.Style.Font.Color = System.Drawing.Color.White;
        //    if (IsFontbool == true)
        //    {
        //        range.Style.Font.Bold = IsFontbool;
        //    }
        //}

        #endregion
    }
}