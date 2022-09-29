using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class ClaimServiceAdjusterAppointment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // GetClaimServiceAdjusterAppointment();
            GetClaimServiceAdjusterAppointment_NewData();
        }

        private void GetClaimServiceAdjusterAppointment()
        {
            try
            {
                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.ClaimServiceResult objClaimServiceResult = new ServiceReference1.ClaimServiceResult();
                ServiceReference1.ClaimUserData objClaimUserData = new ServiceReference1.ClaimUserData();
                
                string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
                string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();

                objClaimUserData.NotificationNumber = "10410000179";
                objClaimUserData.MotorClaimType = "OD";
                objClaimUserData.IsTheftType = false;
                objClaimUserData.SessionID = "100000000324369";
                objClaimUserData.TransactionID = "100000000324369";
                objClaimUserData.TransactionDateTime = "02/12/2016";
                objClaimUserData.NodeName = "INTERMEDIARY APPOINTMENT"; //"INTERMEDIARY APPOINTMENT (Activity/ Page Name)";
                objClaimUserData.AppUserID = "GC0011";
                objClaimUserData.ClaimAmount = "0";
                objClaimUserData.NotificationDate = "17/11/2016";
                objClaimServiceResult.ClaimData = objClaimUserData;

                ServiceReference1.GDtIntermediaryAppointment objGDtIntermediaryAppointment = new ServiceReference1.GDtIntermediaryAppointment();
                objGDtIntermediaryAppointment.TypeOfIntermediaryCode = "33";
                objGDtIntermediaryAppointment.EditDeleteFlag = "0";
                objGDtIntermediaryAppointment.SurveyorCategory = "C";
                objGDtIntermediaryAppointment.IntermediaryCode = "1000000003";
                objGDtIntermediaryAppointment.Branch_Id = ""; //90002
                objGDtIntermediaryAppointment.TypeOfSurveyCode = "1";
                objGDtIntermediaryAppointment.IntermediaryName = "ARKYADEEP";
                objGDtIntermediaryAppointment.DateOfAppointment = "02/12/2016";
                objGDtIntermediaryAppointment.ExpectedDateofSubmission = "16/12/2016";
                objGDtIntermediaryAppointment.LicenseExpiryDate = "23/11/2018";

                //ServiceReference1.ClaimUserData objClaimUserData = new ServiceReference1.ClaimUserData();
                List<ServiceReference1.GDtIntermediaryAppointment> listGDtIntermediaryAppointment = new List<ServiceReference1.GDtIntermediaryAppointment>();
                listGDtIntermediaryAppointment.Add(objGDtIntermediaryAppointment);
                //objClaimUserData.IntermediaryAppointmentDetails = listGDtIntermediaryAppointment.ToArray();
                objClaimServiceResult.ClaimData.IntermediaryAppointmentDetails = listGDtIntermediaryAppointment.ToArray();


                string Status = proxy.ClaimAdjusterAppointment(strUserId, strPassword, objClaimServiceResult.ClaimData.NotificationNumber, ref objClaimServiceResult);
                proxy.Close();

                Response.Write(Status);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void GetClaimServiceAdjusterAppointment_NewData()
        {
            try
            {
                ServiceReference1.CustomerPortalServiceClient proxy = new ServiceReference1.CustomerPortalServiceClient();
                proxy.Endpoint.Behaviors.Add(new CustomBehavior());

                ServiceReference1.ClaimServiceResult objClaimServiceResult = new ServiceReference1.ClaimServiceResult();
                ServiceReference1.ClaimUserData objClaimUserData = new ServiceReference1.ClaimUserData();

                string strUserId = ConfigurationManager.AppSettings["strUserId"].ToString();
                string strPassword = ConfigurationManager.AppSettings["strPassword"].ToString();

                objClaimUserData.NotificationNumber = "10110000581";
                objClaimUserData.MotorClaimType = "OD";
                objClaimUserData.IsTheftType = false;
                objClaimUserData.SessionID = "800123";
                objClaimUserData.TransactionID = "454858";
                objClaimUserData.TransactionDateTime = "02/01/2017";
                objClaimUserData.NodeName = "INTERMEDIARY APPOINTMENT"; //"INTERMEDIARY APPOINTMENT (Activity/ Page Name)";
                objClaimUserData.AppUserID = "GC0022";
                objClaimUserData.ClaimAmount = "0";
                objClaimUserData.NotificationDate = "02/01/2017";
                objClaimServiceResult.ClaimData = objClaimUserData;

                ServiceReference1.GDtIntermediaryAppointment objGDtIntermediaryAppointment = new ServiceReference1.GDtIntermediaryAppointment();
                objGDtIntermediaryAppointment.TypeOfIntermediaryCode = "33";
                objGDtIntermediaryAppointment.EditDeleteFlag = "0";
                objGDtIntermediaryAppointment.SurveyorCategory = "C";
                objGDtIntermediaryAppointment.IntermediaryCode = "368";
                objGDtIntermediaryAppointment.Branch_Id = ""; //90002
                objGDtIntermediaryAppointment.TypeOfSurveyCode = "1";
                objGDtIntermediaryAppointment.IntermediaryName = "AVANTIKAOZA";
                objGDtIntermediaryAppointment.DateOfAppointment = "02/01/2017";
                objGDtIntermediaryAppointment.ExpectedDateofSubmission = "17/01/2017";
                objGDtIntermediaryAppointment.LicenseExpiryDate = "01/01/2099";

                //ServiceReference1.ClaimUserData objClaimUserData = new ServiceReference1.ClaimUserData();
                List<ServiceReference1.GDtIntermediaryAppointment> listGDtIntermediaryAppointment = new List<ServiceReference1.GDtIntermediaryAppointment>();
                listGDtIntermediaryAppointment.Add(objGDtIntermediaryAppointment);
                //objClaimUserData.IntermediaryAppointmentDetails = listGDtIntermediaryAppointment.ToArray();
                objClaimServiceResult.ClaimData.IntermediaryAppointmentDetails = listGDtIntermediaryAppointment.ToArray();


                string Status = proxy.ClaimAdjusterAppointment(strUserId, strPassword, objClaimServiceResult.ClaimData.NotificationNumber, ref objClaimServiceResult);
                proxy.Close();

                Response.Write(Status);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}