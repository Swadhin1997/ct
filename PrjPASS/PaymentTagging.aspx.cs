using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace PrjPASS
{
    public partial class PaymentTagging : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Click(object sender, EventArgs e)
        {
            PaymentAllocationMethod();
            //PaymentTaggingMethod();
        }

        private void PaymentTaggingMethod()
        {
            #region
            //bool IsSuccess = false;
            //string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
            //string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();
            //try
            //{
            //    ServiceReference4.CustomerPortalServiceClient proxy = new ServiceReference4.CustomerPortalServiceClient();
            //    proxy.Endpoint.Behaviors.Add(new CustomBehavior());

            //    ServiceReference4.ServiceResult objServiceResult = new ServiceReference4.ServiceResult();
            //    objServiceResult.UserData = new ServiceReference4.clsUserData();

            //    objServiceResult.UserData.IsWorkSheetRequired = false;
            //    objServiceResult.UserData.UserId = strUserId;
            //    objServiceResult.UserData.Password = strPassword;
            //    objServiceResult.UserData.UserRole = "ADMIN";
            //    objServiceResult.UserData.ProductCode = "3121";
            //    objServiceResult.UserData.ModeOfOperation = "NEWPOLICY";
            //    objServiceResult.UserData.AuthenticateKey = Auth(strUserId, strPassword);
            //    objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestXML_Tagging());
            //    objServiceResult.UserData.IsInternalRiskGrid = true;

            //    objServiceResult.UserData.ErrorText = "";

            //    proxy.PaymentTagging(objServiceResult.UserData.AuthenticateKey, ref objServiceResult);
            //    proxy.Close();

            //    IsSuccess = objServiceResult.UserData.ErrorText.Trim().Length > 0 ? false : true;

            //    string NewErrorProp = objServiceResult.UserData.ServiceErrorText;

            //    if (objServiceResult.UserData.ErrorText.Trim().Length > 0)
            //    {
            //        Response.Write(objServiceResult.UserData.ErrorText.Trim());
            //    }
            //    else
            //    {
            //        string ResultXML = objServiceResult.UserData.UserResultXml;
            //        string PaymentId = objServiceResult.UserData.PaymentId;

            //        Response.Write("PAYMENT ID: " + objServiceResult.UserData.PaymentId);
            //        Response.Write("Policy NUMBER: " + objServiceResult.UserData.PolicyNO);
            //        Response.Write("PolicyNumber: " + objServiceResult.UserData.PolicyNumber);

            //        string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PaymentEntry_Result.xml";
            //        File.WriteAllText(strXmlPath, String.Empty);
            //        File.WriteAllText(strXmlPath, ResultXML);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Response.Write(ex.Message);
            //}
            #endregion
            #region
            bool IsSuccess = false;
            string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
            string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();
            try
            {
                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.ServiceResult objServiceResult = new ServiceReference1.ServiceResult();
                objServiceResult.UserData = new ServiceReference1.clsUserData();

                //objServiceResult.UserData.IsWorkSheetRequired = false;
                objServiceResult.UserData.UserId = strUserId;
                objServiceResult.UserData.Password = strPassword;
                objServiceResult.UserData.UserRole = "ADMIN";
                objServiceResult.UserData.ProductCode = "2876"; //3121 FOR MOTOR AND 2876 FOR HEALTH
                objServiceResult.UserData.ModeOfOperation = "NEWPOLICY";
                //objServiceResult.UserData.AuthenticateKey = Auth(strUserId, strPassword);
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestXML_Tagging());
                objServiceResult.UserData.IsInternalRiskGrid = true;
                objServiceResult.UserData.WorksheetInByte = true;

                objServiceResult.UserData.ErrorText = "";

                proxy.PaymentTagging(strUserId, strPassword, ref objServiceResult);
                proxy.Close();

                IsSuccess = objServiceResult.UserData.ErrorText.Trim().Length > 0 ? false : true;

                string NewErrorProp = objServiceResult.UserData.ServiceErrorText;

                if (objServiceResult.UserData.ErrorText.Trim().Length > 0)
                {
                    Response.Write(objServiceResult.UserData.ErrorText.Trim());
                }
                else
                {
                    string ResultXML = objServiceResult.UserData.UserResultXml;
                    string PaymentId = objServiceResult.UserData.PaymentId;

                    Response.Write("PAYMENT ID: " + objServiceResult.UserData.PaymentId);
                    Response.Write("Policy NUMBER: " + objServiceResult.UserData.PolicyNO);
                    Response.Write("PolicyNumber: " + objServiceResult.UserData.PolicyNumber);

                    string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PaymentEntry_Result.xml";
                    File.WriteAllText(strXmlPath, String.Empty);
                    File.WriteAllText(strXmlPath, ResultXML);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            #endregion

        }

        public string Auth(string UserId, string Passwd)
        {
            ServiceReference4.CustomerPortalServiceClient proxy = new ServiceReference4.CustomerPortalServiceClient();

            proxy.Endpoint.Behaviors.Add(new CustomBehavior());

            ServiceReference4.ServiceResult objServiceResult = new ServiceReference4.ServiceResult();

            string AuthKey = proxy.Authenticate(UserId, Passwd);
            proxy.Close();

            return AuthKey;
            //return "";
        }

        private string GetRequestXML_Tagging()
        {
            string strXmlPath = "";
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PaymentTagging_Request.xml"; //THIS XML IS FOR MOTOR
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PaymentTagging_Request_Bhavik.xml"; //THIS XML IS FOR HEALTH
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PaymentTagging_Request_Ashutosh.xml";
            strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "QH20160916518PAYMENTTAGGING.xml"; //THIS XML IS FOR HEALTH

            XmlDocument xmlfile = null;
            string xmlString = "";
            try
            {
                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);

                DateTime dtAppDate = DateTime.Now;
                string strCurrentDate = dtAppDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
            return xmlString;
        }

        private void PaymentAllocationMethod()
        {
            bool IsSuccess = false;
            string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
            string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();
            try
            {
                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.ServiceResult objServiceResult = new ServiceReference1.ServiceResult();
                objServiceResult.UserData = new ServiceReference1.clsUserData();

                objServiceResult.UserData.IsWorkSheetRequired = false;
                objServiceResult.UserData.UserId = strUserId;
                objServiceResult.UserData.Password = strPassword;
                objServiceResult.UserData.UserRole = "ADMIN";
                objServiceResult.UserData.ProductCode = "3121"; //3121 FOR MOTOR AND 2876 FOR HEALTH
                objServiceResult.UserData.ModeOfOperation = "NEWPOLICY";
                //objServiceResult.UserData.AuthenticateKey = Auth(strUserId, strPassword);
                objServiceResult.UserData.ConsumeProposalXML = new StringBuilder(GetRequestXML_Allocation());
                objServiceResult.UserData.IsInternalRiskGrid = true;
                objServiceResult.UserData.WorksheetInByte = true;

                objServiceResult.UserData.ErrorText = "";

                proxy.PaymentAllocation(strUserId, strPassword, ref objServiceResult);
                proxy.Close();

                IsSuccess = objServiceResult.UserData.ErrorText.Trim().Length > 0 ? false : true;

                string NewErrorProp = objServiceResult.UserData.ServiceErrorText;

                if (objServiceResult.UserData.ErrorText.Trim().Length > 0)
                {
                    Response.Write(objServiceResult.UserData.ErrorText.Trim());
                }
                else
                {
                    string ResultXML = objServiceResult.UserData.UserResultXml;
                    string PaymentId = objServiceResult.UserData.PaymentId;

                    Response.Write("PaymentId_From_Allocation: " + objServiceResult.UserData.PaymentId);

                    string strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PaymentEntry_Result.xml";
                    File.WriteAllText(strXmlPath, String.Empty);
                    File.WriteAllText(strXmlPath, ResultXML);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private string GetRequestXML_Allocation()
        {
            string strXmlPath = "";
            strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PaymentAllocation_Request.xml"; //THIS XML IS FOR MOTOR
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PaymentAllocation_Request2.xml"; //THIS XML IS FOR HEALTH
            //strXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PaymentAllocation_Request_Ashutosh.xml";

            XmlDocument xmlfile = null;
            string xmlString = "";
            try
            {
                xmlfile = new XmlDocument();
                xmlfile.Load(strXmlPath);

                DateTime dtAppDate = DateTime.Now;
                string strCurrentDate = dtAppDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                xmlString = xmlfile.InnerXml;
                xmlString = xmlString.Replace("></element>", "/>");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
            return xmlString;
        }

        protected void btnAllocation_Click(object sender, EventArgs e)
        {
            PaymentAllocationMethod();
        }
    }
}