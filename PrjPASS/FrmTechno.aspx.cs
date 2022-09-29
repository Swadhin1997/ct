using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Obout.Grid;
using Obout.Interface;
using System.Web.Services;
using PrjPASS;
using System.Text.RegularExpressions;
using System.Web.ModelBinding;
using System.Net;
using System.IO;
using System.Globalization;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Threading;

namespace ProjectPASS
{
    public partial class FrmTechno : System.Web.UI.Page
    {
        AutoResetEvent stopWaitHandle = new AutoResetEvent(false);
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        string FrmDownloadPolicyScheduleLog = AppDomain.CurrentDomain.BaseDirectory + "//FrmDownloadPolicySchedule//log.txt";
        string FrmDownloadPolicyScheduleLogDirectory = AppDomain.CurrentDomain.BaseDirectory + "//FrmDownloadPolicySchedule";

        protected void Page_Load(object sender, EventArgs e)
        {
            //btnValidate.Attributes.Remove("ob_iBOv");
            //btnValidate.Attributes.Remove("ob_iBC");
            //btnValidate.Attributes.Remove("ob_iBCN");
            //btnValidate.Style.Add("font-weight", "bold");
            //btnValidate.Style.Add("font-size", "20px");
            //UpdatePanel_OPS.Update();
            //btnValidate.Attributes.Add("class", "ob_iBC");

            if (!IsPostBack)
            {
                lblCounter.Text = "0";
                hdnCounter.Value = "0";
                btnSave.Focus();
                //btnSave.Style.Add("css")
                //btnValidate.CssClass = "cssActive";
                btnSave.CssClass = "cssActive";
                if (Session["OTPTimerValue"] == null)//this avoid resetting of the session on postback
                {
                    Session["OTPTimerValue"] = 0;
                }

                Session["vUserLoginId"] = "CUSTOMERPORTAL";
                Session["Resend"] = "FALSE";
                lblSuccessMsg.Text = "";
                lblMsg.Text = "";
 
                if (ConfigurationManager.AppSettings["TECHNO_IsUAT"].ToString() == "TRUE")
                {
                    FillControl();
                }
                
                FillDOBDateList();
                FillPDateList();
                FillMaritalStatusList();
                FillOccupationList();
                FillRelationShipList();
                FillSalutationList();
                FillIdProofList();
                FillGenderList();
            }
            else
            {          
            }
        }

        protected void UpdateTimer_Tick(object sender, EventArgs e)
        {
            txtOtpNumber.Focus();
            Session["OTPTimerValue"]= Convert.ToInt32(Session["OTPTimerValue"]) - 1;
            int RemainingSec = Convert.ToInt32(Session["OTPTimerValue"]);

            if (RemainingSec > 1)
            {
                btnMobileReSend.Enabled = false;
                btnMobileVerify.Enabled = true;
                lblTimer.Visible = true;
                lblTimer.Text = RemainingSec.ToString();
            }
            else
            {
                btnMobileReSend.Enabled = true;
                btnMobileVerify.Enabled = false;
                Session["OTPTimerValue"] = "0";
                lblTimer.Visible = false;
                //UpdateTimer.Enabled = false;
            }

        }

        public async Task AsynSendSMS( string Msg,string MobileNo)
        {
            await Task.Run(() =>
            {
                //Alert.Show("HI");
                SendSMS(Msg,MobileNo);
            });
        }

        public void ShowSuccessMsg(string msg)
        {
            Alert.Show(msg);
            new Task(() =>
            {
                Thread.Sleep(5000);
                Stop_Callback(); // invoke the callback
            }).Start();
        }
        private void Stop_Callback()
        {
            // signal the wait handle
            stopWaitHandle.Set();
        }
     
        public  void HideControl()
        {
            //Alert.Show("HI");
            lblSuccessMsg.Text = " ";
            otpPanel.Visible = false;
            captchaPanel.Visible = false;
            //ResetControl();
            pnlReg.Visible = false;
            UpdatePanel_OPS.Update();
        }
        public void FillControl()
        {
            txtUniqueIdentificationNos.Text= "981068344107953";
            txtScratchCardUniqueNos.Text= "12345679014";
            //ddlSalutation.SelectedValue="1";
            txtProposerFirstName.Text="Test";
            txtProposerMiddleName.Text="TEST";
            txtProposerLastName.Text="TEST";
            //txtDOBDD.Text = "01";
            //txtDOBMM.Text = "02";
            //txtDOBYYYY.Text="2000";
            
            //ddlGender.SelectedValue="1";
            //ddlMaritalStatus.SelectedValue="1";
            //ddlOccupation.SelectedValue="1";
            txtNomineeName.Text = "TEST NOMINEE";
            //ddlOccupation.SelectedValue="1";
            //ddlOccupation.SelectedValue="1";
            txtUniqueProofNos.Text = "JKOPS5805L";
            //txtPDD.Text = "28";
            //txtPMM.Text = "08";
            //txtPYYYY.Text="2020";
            ddlCustomerIdProof.SelectedValue = "1";
            ddlOccupation.SelectedValue = "1";
            ddlNomineeRel.SelectedValue = "1";
            ddlGender.SelectedValue = "1";
            ddlSalutation.SelectedIndex = 0;
            ddlPYYYY.SelectedValue = "2020";
            ddlPMM.SelectedValue = "9";
            ddlPDD.SelectedValue = "1";
            txtInvoiceNos.Text= "12345679015";
            txtAddr1.Text="ghghghg";
            txtAddr2.Text="hjhjhjh";
            txtAddr3.Text="jhjhjh";
            txtPincode.Text="210001";
            txtEmailId.Text="kgi.rajesh-soni@kotak.com";
            txtMobileNos.Text="7678014270";
        }
        public void ResetControl()
        {
            Response.Redirect("FrmTechno.aspx", true);
        }
        public void ResetControl2()
           {
            txtUniqueIdentificationNos.Text = "";
            txtScratchCardUniqueNos.Text = "";
            txtMasterPolicyHolderName.Text = "";
            ddlSalutation.ClearSelection();
            txtProposerFirstName.Text = "";
            txtProposerMiddleName.Text = "";
            txtProposerLastName.Text = "";
            //txtDOBDD.Text = "";
            //txtDOBMM.Text = "";
            //txtDOBYYYY.Text = "";
            ddlDOBDD.ClearSelection();
            ddlDOBMM.ClearSelection();
            ddlDOBYYYY.ClearSelection();
            ddlDOBYYYY.SelectedValue = DateTime.Now.Year.ToString();

            ddlGender.ClearSelection();
            ddlMaritalStatus.ClearSelection() ;
            ddlOccupation.ClearSelection();
            txtNomineeName.Text = "";
            ddlNomineeRel.ClearSelection();
            ddlCustomerIdProof.ClearSelection();
            txtUniqueProofNos.Text = "";
            //txtPDD.Text = "";
            //txtPMM.Text = "";
            //txtPYYYY.Text = "";
            ddlPDD.ClearSelection();
            ddlPMM.ClearSelection();
            ddlPYYYY.ClearSelection();
            ddlPYYYY.SelectedValue = DateTime.Now.Year.ToString();

            txtInvoiceNos.Text = "";
            txtAddr1.Text = "";
            txtAddr2.Text = "";
            txtAddr3.Text = "";
            txtPincode.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtEmailId.Text = "";
            txtMobileNos.Text = "";
            chkbAgree.Checked = false;
            chkbConfirm.Checked = false;
            txtCaptcha.Text = "";
            txtOtpNumber.Text = "";
            lblMsg.Text = "";
            lblSuccessMsg.Text = "";

            Random random = new Random();
            string combination = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            StringBuilder captcha = new StringBuilder();
            for (int i = 0; i < 7; i++)
                captcha.Append(combination[random.Next(combination.Length)]);
            Session["CaptchaVerify"] = captcha.ToString();

            imgCaptcha.ImageUrl="Captcha.aspx?"+DateTime.Now.Ticks.ToString();
            UpdatePanel_OPS.Update();
        }     
        public clsMasterFetchClasses Fn_Get_Master(string masterName)
        {
            List<clsMasterData> objMaster = new List<clsMasterData>();
            clsMasterFetchClasses objsum = new clsMasterFetchClasses();
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            try
            {

                DataSet objDataSet = null;
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    SqlCommand cmdCheck = new SqlCommand("PROC_GET_MASTER_DETAILS", con);
                    cmdCheck.CommandType = CommandType.StoredProcedure;                   
                    cmdCheck.Parameters.AddWithValue("@vmasterName", masterName);
                    //cmdCheck.Parameters.AddWithValue("@vextraVerify", extraparam);

                    SqlDataAdapter sda = new SqlDataAdapter(cmdCheck);
                    objDataSet = new DataSet();
                    sda.Fill(objDataSet);
                }
                for (int i = 0; i < objDataSet.Tables[0].Rows.Count; i++)
                {
                    objMaster.Add(new clsMasterData()
                    {
                        ID = Convert.ToString(objDataSet.Tables[0].Rows[i]["ID"]),
                        Value = Convert.ToString(objDataSet.Tables[0].Rows[i]["Value"])
                    });
                }

                objsum.oMasterData = objMaster;
                objsum.vErrorMsg = "Success";
                //  return objsuminsured;
            }
            catch (Exception ex)
            {
                objsum.vErrorMsg = ex.Message;
                // return objsuminsured;
            }
            return objsum;
        }
        public DataSet Fn_Get_Master_DataSet(string masterName)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            try
            {
                DataSet objDataSet = null;
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    SqlCommand cmdCheck = new SqlCommand("PROC_TECHNO_GET_MASTER_DETAILS", con);
                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.Parameters.AddWithValue("@vmasterName", masterName);
                    //cmdCheck.Parameters.AddWithValue("@vextraVerify", extraparam);

                    SqlDataAdapter sda = new SqlDataAdapter(cmdCheck);
                    objDataSet = new DataSet();
                    sda.Fill(objDataSet);
                }
                
               return objDataSet;
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        [System.Web.Services.WebMethod]
        public static DataSet Fn_Get_City_AND_State_By_Pincode_DataSet(string Pincode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            try
            {
                DataSet objDataSet = null;
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    SqlCommand cmdCheck = new SqlCommand("PROC_GET_CITY_AND_STATE_BY_PINCODE", con);
                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.Parameters.AddWithValue("@vPincode", Pincode);
                    //cmdCheck.Parameters.AddWithValue("@vextraVerify", extraparam);

                    SqlDataAdapter sda = new SqlDataAdapter(cmdCheck);
                    objDataSet = new DataSet();
                    sda.Fill(objDataSet);
                }

                return objDataSet;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #region Fill Master drop Down

        protected void FillDOBDateList()
        {
            IEnumerable<int> Days= clsTechnoUserDetails.DatePart.DayItems;
            IEnumerable<int> Months = clsTechnoUserDetails.DatePart.MonthItems;
            IEnumerable<int> Years = clsTechnoUserDetails.DatePart.YearItems;
            var index = 0;
            ddlDOBDD.DataValueField = "TEXT";
            ddlDOBDD.DataTextField = "TEXT";
            ddlDOBDD.DataSource = Days.Select(x => new { Text = x, Value = index++ });
            ddlDOBDD.DataBind();

            ddlDOBMM.DataValueField = "TEXT";
            ddlDOBMM.DataTextField = "TEXT";
            ddlDOBMM.DataSource = Months.Select(x => new { Text = x, Value = index++ });
            ddlDOBMM.DataBind();

            ddlDOBYYYY.DataValueField = "TEXT";
            ddlDOBYYYY.DataTextField = "TEXT";
            ddlDOBYYYY.DataSource = Years.Select(x => new { Text = x, Value = index++ });
            ddlDOBYYYY.DataBind();
            ddlDOBYYYY.SelectedValue = DateTime.Now.Year.ToString();
        }
        protected void FillPDateList()
        {
            IEnumerable<int> Days = clsTechnoUserDetails.DatePart.DayItems;
            IEnumerable<int> Months = clsTechnoUserDetails.DatePart.MonthItems;
            IEnumerable<int> Years = clsTechnoUserDetails.DatePart.YearItems;
            var index = 0;
            ddlPDD.DataValueField = "TEXT";
            ddlPDD.DataTextField = "TEXT";
            ddlPDD.DataSource = Days.Select(x => new { Text = x, Value = index++ });
            ddlPDD.DataBind();

            ddlPMM.DataValueField = "TEXT";
            ddlPMM.DataTextField = "TEXT";
            ddlPMM.DataSource = Months.Select(x => new { Text = x, Value = index++ });
            ddlPMM.DataBind();

            ddlPYYYY.DataValueField = "TEXT";
            ddlPYYYY.DataTextField = "TEXT";
            ddlPYYYY.DataSource = Years.Select(x => new { Text = x, Value = index++ });
            ddlPYYYY.DataBind();
            ddlPYYYY.SelectedValue = DateTime.Now.Year.ToString();

        }
        protected void FillOccupationList()
        {
            
            DataSet ds = null;
            ds = Fn_Get_Master_DataSet("OCCUPATIONMASTER");
            if (ds.Tables.Count > 0)
            {
                ddlOccupation.DataValueField = "ID";
                ddlOccupation.DataTextField = "Value";
                ddlOccupation.DataSource = ds.Tables[0];
                ddlOccupation.DataBind();

                //drpDept.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

                //FillDeptRoleList(drpDept.SelectedValue);
            }
            ddlOccupation.Items.Insert(0, new ListItem("-Select-", "-Select-"));
        }
        protected void FillRelationShipList()
        {

            DataSet ds = null;
            ds = Fn_Get_Master_DataSet("RELATIONMASTER");
            
            if (ds.Tables.Count > 0)
            {
                ddlNomineeRel.DataValueField = "ID";
                ddlNomineeRel.DataTextField = "Value";
                ddlNomineeRel.DataSource = ds.Tables[0];
                ddlNomineeRel.DataBind();
            }
            ddlNomineeRel.Items.Insert(0, new ListItem("-Select-", "-Select-"));
        }
        protected void FillSalutationList()
        {

            DataSet ds = null;
            ds = Fn_Get_Master_DataSet("TITLEMASTER");

            if (ds.Tables.Count > 0)
            {
                ddlSalutation.DataValueField = "ID";
                ddlSalutation.DataTextField = "Value";
                ddlSalutation.DataSource = ds.Tables[0];
                ddlSalutation.DataBind();
            }
            ddlSalutation.Items.Insert(0, new ListItem("-Select-", "-Select-"));
        }
        public void FillMaritalStatusList()
        {
            //1:
            clsTechnoUserDetails objclsTechnoUserDetails = new clsTechnoUserDetails();
            Array itemValues = System.Enum.GetValues(typeof(clsTechnoUserDetails.enMaritalStatus));
            Array itemNames = System.Enum.GetNames(typeof(clsTechnoUserDetails.enMaritalStatus));

            foreach (String name in itemNames)
            {
                //get the enum item value
                Int32 value = (Int32)Enum.Parse(typeof(clsTechnoUserDetails.enMaritalStatus), name);
                ListItem listItem = new ListItem(name, value.ToString());
                ddlMaritalStatus.Items.Add(listItem);
            }
            //ddlMaritalStatus.Items.Insert(0, new ListItem("-Select-", "-Select-"));

            //2:

            //ddlMaritalStatus.DataSource = Enum.GetNames(typeof(clsTechnoUserDetails.enMaritalStatus)).
            //Select(o => new { Text = o, Value = (byte)(Enum.Parse(typeof(clsTechnoUserDetails.enMaritalStatus), o)) });
            //ddlMaritalStatus.DataTextField = "Text";
            //ddlMaritalStatus.DataValueField = "Value";
            //ddlMaritalStatus.DataBind();

            //3:
            //ddlMaritalStatus.DataSource = Enum.GetNames(typeof(clsTechnoUserDetails.enMaritalStatus));
            //ddlMaritalStatus.DataBind();
            ////if you want the Enum value Back on Selection....
            ////clsTechnoUserDetails.enMaritalStatus empType = (clsTechnoUserDetails.enMaritalStatus)Enum.Parse(typeof(clsTechnoUserDetails.enMaritalStatus), ddlMaritalStatus.SelectedValue);
        }
        public void FillGenderList()
        {
            //1:

            Array itemValues = System.Enum.GetValues(typeof(clsTechnoUserDetails.enGender));
            Array itemNames = System.Enum.GetNames(typeof(clsTechnoUserDetails.enGender));

            foreach (String name in itemNames)
            {
                //get the enum item value
                Int32 value = (Int32)Enum.Parse(typeof(clsTechnoUserDetails.enGender), name);
                ListItem listItem = new ListItem(name, value.ToString());
                ddlGender.Items.Add(listItem);
            }
            ddlGender.Items.Insert(0, new ListItem("-Select-", "-Select-"));
        }
        public void FillIdProofList()
        {
            //1:

            Array itemValues = System.Enum.GetValues(typeof(clsTechnoUserDetails.enIdProofTypes));
            Array itemNames = System.Enum.GetNames(typeof(clsTechnoUserDetails.enIdProofTypes));

            foreach (String name in itemNames)
            {
                //get the enum item value
                Int32 value = (Int32)Enum.Parse(typeof(clsTechnoUserDetails.enIdProofTypes), name);
                ListItem listItem = new ListItem(name, value.ToString());
                ddlCustomerIdProof.Items.Add(listItem);
            }
            ddlCustomerIdProof.Items.Insert(0, new ListItem("-Select-", "-Select-"));
        }

        #endregion

        private string SaveUserDetails(clsTechnoUserDetails objUserDetails)
        {
            string ReturnMessage = string.Empty;

            //For Test Code should Be commented for UAT
            //Session["vUserLoginId"] = "GV0093";
            //

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            //Cls_General_Functions wsDocNo = new Cls_General_Functions();
            //string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
            //string vEmpCode = objUserDetails.vUserLoginId.ToUpper();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("PROC_SAVE_TECHNO_USER_DETAILS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@vPrimaryIMEINos", objUserDetails.vPrimaryIMEINos);
                cmd.Parameters.AddWithValue("@vScratchCardNos", objUserDetails.vScratchCardNos);
                cmd.Parameters.AddWithValue("@nSalutationId", objUserDetails.nSalutationId);
                cmd.Parameters.AddWithValue("@vFirstName", objUserDetails.vFirstName);
                cmd.Parameters.AddWithValue("@vMiddleName", objUserDetails.vMiddleName);
                cmd.Parameters.AddWithValue("@vLastName", objUserDetails.vLastName);
                cmd.Parameters.AddWithValue("@dDOB", objUserDetails.dDOB);
                
                //cmd.Parameters.AddWithValue("@bIsActivate", OboutChkActive.Checked == true ? 'Y' : 'N');
                //cmd.Parameters.AddWithValue("@vIntermediaryCode", objUserDetails.dDOB);
                cmd.Parameters.AddWithValue("@nGenderId", objUserDetails.nGenderId);
                cmd.Parameters.AddWithValue("@nMaritalStatusId", objUserDetails.nMaritalStatusId);
                cmd.Parameters.AddWithValue("@nOccupationId", objUserDetails.nOccupationId);
                cmd.Parameters.AddWithValue("@vNomineeName", objUserDetails.vNomineeName);
                cmd.Parameters.AddWithValue("@nNomineeRelationshipId", objUserDetails.nNomineeRelationshipId);
                cmd.Parameters.AddWithValue("@nIdProofTypeId", objUserDetails.nIdProofTypeId);
                cmd.Parameters.AddWithValue("@vUniqueProofNos", objUserDetails.vUniqueProofNos);
                cmd.Parameters.AddWithValue("@dPurchaseDate", objUserDetails.dPurchaseDate);
                cmd.Parameters.AddWithValue("@vInvoiceNos", objUserDetails.vInvoiceNos);
                cmd.Parameters.AddWithValue("@vCreatedBy", Session["vUserLoginId"].ToString());

                cmd.Parameters.AddWithValue("@vAddr1", objUserDetails.objUserAddr.vAddr1);
                cmd.Parameters.AddWithValue("@vAddr2", objUserDetails.objUserAddr.vAddr2);
                cmd.Parameters.AddWithValue("@vAddr3", objUserDetails.objUserAddr.vAddr3);
                cmd.Parameters.AddWithValue("@nPincode", objUserDetails.objUserAddr.nPincode);
                cmd.Parameters.AddWithValue("@vEmailId", objUserDetails.objUserAddr.vEmailId);
                cmd.Parameters.AddWithValue("@vMobileNos", objUserDetails.objUserAddr.vMobileNos);
                SqlParameter outputMessage = new SqlParameter("@outputMessage", SqlDbType.VarChar, 200);
                outputMessage.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputMessage);
                cmd.ExecuteNonQuery();
                ReturnMessage = outputMessage.Value.ToString();
                //Alert.Show(ReturnMessage.ToString(), "FrmCreateUserMasterNewEdit.aspx");
                return ReturnMessage;
            }
            catch (Exception ex)
            {
                // log exception
                ExceptionUtility.LogException(ex, "SaveUserDetails");
                return ex.Message;

            }
            finally
            {
                con.Close();
                HdFldSave.Value = "Insert";
                // BindData();  
            }
             
        }
        protected void onClickbtnMobileReSend(object sender, EventArgs e)
        {
            Session["Resend"] = "TRUE";
            btnSave_Click(sender, e);
        }
        protected void OnServerValidatecvtxtOtpNumber(object sender, ServerValidateEventArgs e)
        {
            if(txtOtpNumber.Text == Convert.ToString(HttpContext.Current.Session["OTPNumber"]))
            {
                e.IsValid = true;
            }
            else
            {
                if (Session["Resend"].ToString() != "TRUE")
                {
                    e.IsValid = false;
                    otpPanel.Visible = true;
                    txtOtpNumber.Text = "";
                    btnSave.Enabled = false;
                    btnMobileReSend.Enabled = false;
                    btnMobileVerify.Focus();
                    cvtxtOtpNumber.ErrorMessage = "Please provide valid OTP number.";
                    ScriptManager.RegisterStartupScript(UpdatePanel_OPS, UpdatePanel_OPS.GetType(), "testpage", "StartTimer();", true);
                }
            }
        }
        protected void onClickbtnMobileVerify(object sender, EventArgs e)
        {
            //if (Page.IsValid)
            //{
                Session["Resend"] = "FALSE";
                if (txtOtpNumber.Text == Convert.ToString(HttpContext.Current.Session["OTPNumber"]))
                {
                    //btnRegister_Click(sender, e);
                    string[] vErrorMsg = new string[2];

                    vErrorMsg = Fn_Verify_OTP(txtUniqueIdentificationNos.Text, "TECHNO", txtMobileNos.Text, txtOtpNumber.Text);

                  if (vErrorMsg[0] == "Success")
                    {
                    //Fn_Update_OTP_Email(propNumber, lblQuoteNumber.Text, lblEmail.Text, txtOtpNumber.Text);
                    btnRegister_Click(sender, e);
                    }
                    else
                    {
                      Alert.Show(vErrorMsg[1]);
                      //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in onClickbtnMobileVerify for proposal :" + proposal + " and is in else condition of otpflag " + DateTime.Now + Environment.NewLine);
                      //Response.Redirect("FrmCustomErrorPage.aspx");
                    }
                 }
                else
                {
                    Alert.Show("Enterd OTP Is Invalid!");
                    //hdnIsPostBack.Value = "1";
                    ScriptManager.RegisterStartupScript(UpdatePanel_OPS, UpdatePanel_OPS.GetType(), "testpage", "StartTimer();", true);
                }
            //}
        }

        public string[] Fn_Verify_OTP(string vUniqueID, string vAppID, string vMobileNo, string vOTPNumber)
        {
            string[] vErrorMsg = new string[2];
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    SqlCommand command = new SqlCommand("PROC_TECHNO_GET_OTP_FROM_TABLE", conn);
                    //SqlCommand command = new SqlCommand("GET_OTP_FROM_TABLE", conn);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@mobileNo", vMobileNo);
                    command.Parameters.AddWithValue("@uniqueID", vUniqueID);
                    command.Parameters.AddWithValue("@appID", vAppID);
                    command.Parameters.AddWithValue("@otp", vOTPNumber);

                    Object objScalar = command.ExecuteScalar();
                    string retString = Convert.ToString(objScalar);
                    //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "KGI_ADService : Return string for otp " + retString + " for mobile " + vMobileNo + " " + DateTime.Now + Environment.NewLine);
                    if (String.IsNullOrEmpty(retString))
                    {
                        vErrorMsg[0] = "Fail";
                        vErrorMsg[1] = "Invalid OTP / Try again.";
                        return vErrorMsg;
                    }

                    else if (retString == "-1")
                    {
                        vErrorMsg[0] = "Fail";
                        vErrorMsg[1] = "OTP has expired.";
                        return vErrorMsg;
                    }

                    else if (retString == "-2")
                    {
                        vErrorMsg[0] = "Fail";
                        vErrorMsg[1] = "Invalid OTP.";
                        return vErrorMsg;
                    }

                    else
                    {
                        vErrorMsg[0] = "Success";
                        vErrorMsg[1] = vOTPNumber;
                        return vErrorMsg;
                    }
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "KGI_ADService : Error occured in VerifyOTP " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                vErrorMsg[0] = "Fail";
                vErrorMsg[1] = ex.Message;
                return vErrorMsg;
            }
        }

        private void Fn_Update_OTP_Email(string vProposalNumber, string vQuoteId, string vEmailId, string vOtpNumber)
        {
            try
            {
                //Prj_Lib_Common_Utility.CommonUtility.Fn_LogEvent("Start of updating otp email for " + vProposalNumber + " and email " + vEmailId);

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["cnBPOS"]))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("PROC_UPDATE_OTP_EMAIL", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@vEmailId", vEmailId);
                    command.Parameters.AddWithValue("@vProposalId", vProposalNumber);
                    command.Parameters.AddWithValue("@vQuoteId", vQuoteId);
                    command.Parameters.AddWithValue("@otpNumber", vOtpNumber);
                    command.ExecuteNonQuery();

                }
                //Prj_Lib_Common_Utility.CommonUtility.Fn_LogEvent("End of updating otp email for " + vProposalNumber + " and email " + vEmailId);

            }
            catch (Exception ex)
            {
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "FrmReviewConfirm.aspx ::Error occured in updating otp for email for proposal :" + proposal + "and error is " + ex.Message + " and stack trace is " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                //Response.Redirect("FrmCustomErrorPage.aspx");
                Alert.Show(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //UpdateTimer.Enabled = true;
            //Session["OTPTimerValue"] = "180";
            Page.Validate();
            if (Page.IsValid && chkbAgree.Checked==true && chkbConfirm.Checked==true)//&& Session["IsValid"]!=null && Session["IsValid"]=="TRUE")
            {
                string strMsg = string.Empty;
                if (txtCaptcha.Text.Trim().ToUpper() == Session["CaptchaVerify"].ToString().Trim().ToUpper())
                {
                    lblSuccessMsg.Text = " ";
                    
                    int Num = int.Parse(hdnOTPSentCount.Value);

                    if (Num < 3)
                    {
                        Num = int.Parse(hdnOTPSentCount.Value) + 1;
                        hdnOTPSentCount.Value = Num.ToString();
                        strMsg = GenerateOTPNew(txtMobileNos.Text);

                        if (strMsg.Trim().ToUpper() == "SUCCESS")
                        {
                            btnSave.Enabled = false;
                            otpPanel.Visible = true;
                            btnMobileVerify.Focus();
                            ScriptManager.RegisterStartupScript(UpdatePanel_OPS, UpdatePanel_OPS.GetType(), "testpage", "StartTimer();", true);
                            lblTimer.Visible = true;
                            btnMobileVerify.Enabled = false;
                            //btnMobileReSend.Enabled = false;
                            UpdatePanel_OPS.Update();
                        }
                        else
                        {
                            btnSave.Enabled = true;
                            cvtxtOtpNumber.IsValid = false;
                            cvtxtOtpNumber.ErrorMessage = "There Was some technical error , Please try after sometime";
                        }
                    }
                    else
                    {
                        //  anchorLink.Disabled = false;
                        cvtxtOtpNumber.IsValid = false;
                        cvtxtOtpNumber.ErrorMessage = "Maximum OTP Send limit is over, kindly contact nearest branch. Or Check your latest OTP";
                        btnMobileReSend.Enabled = false;
                        btnMobileVerify.Enabled = false;
                        UpdatePanel_OPS.Update();
                    }
                }
                else
                {
                    lblSuccessMsg.Text = "Please enter valid Captcha.";
                    lblSuccessMsg.ForeColor = System.Drawing.Color.Red;
                }
            }

            else
            {
                //Alert.Show("Please enter correct Captcha!");
            }
          }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Fn_Register_Policy();
        }
        protected void chkbConfirm_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbConfirm.Checked && chkbAgree.Checked)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }
        protected void chkbAgree_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbConfirm.Checked && chkbAgree.Checked)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }
        public void Fn_Register_Policy()
        {
            string strReturnMessage = string.Empty;
            try
            {
                //if (Page.IsValid)
                //{
                    clsTechnoUserDetails objUserDetails = new clsTechnoUserDetails();
                    //objUserDetails.vUserLoginId = txtUserId.Text.Trim();
                    objUserDetails.vPrimaryIMEINos = txtUniqueIdentificationNos.Text;
                    objUserDetails.vScratchCardNos = txtScratchCardUniqueNos.Text;
                    objUserDetails.nSalutationId =Convert.ToInt32(ddlSalutation.SelectedValue);
                    objUserDetails.vFirstName = txtProposerFirstName.Text.Trim();
                    objUserDetails.vMiddleName = txtProposerMiddleName.Text.Trim();
                    objUserDetails.vLastName = txtProposerLastName.Text.Trim();
                    string DOB = GetFormattedMonthOrDate(ddlDOBDD.SelectedItem.Text) + "/" + GetFormattedMonthOrDate(ddlDOBMM.SelectedItem.Text) + "/" + ddlDOBYYYY.SelectedItem.Text;
                    DateTime dDOB = DateTime.ParseExact(DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    objUserDetails.dDOB = dDOB;
                    objUserDetails.nGenderId = Convert.ToInt32(ddlGender.SelectedValue);
                    objUserDetails.nMaritalStatusId = Convert.ToInt32(ddlMaritalStatus.SelectedValue);
                    objUserDetails.nOccupationId = Convert.ToInt32(ddlOccupation.SelectedValue);
                    objUserDetails.vNomineeName = txtNomineeName.Text;
                    objUserDetails.nNomineeRelationshipId = Convert.ToInt32(ddlNomineeRel.SelectedValue);
                    objUserDetails.nIdProofTypeId = Convert.ToInt32(ddlCustomerIdProof.SelectedValue);
                    objUserDetails.vUniqueProofNos = txtUniqueProofNos.Text;
                    string PurchaseDate = GetFormattedMonthOrDate(ddlPDD.SelectedItem.Text) + "/" + GetFormattedMonthOrDate(ddlPMM.SelectedItem.Text) + "/" + ddlPYYYY.SelectedItem.Text; ;
                    DateTime dPurchaseDate = DateTime.ParseExact(PurchaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    objUserDetails.dPurchaseDate = dPurchaseDate;
                    objUserDetails.vInvoiceNos = txtInvoiceNos.Text;
                    objUserDetails.objUserAddr = new clsTechnoUserAddrDetails();
                    objUserDetails.objUserAddr.vAddr1 = txtAddr1.Text;
                    objUserDetails.objUserAddr.vAddr2 = txtAddr2.Text;
                    objUserDetails.objUserAddr.vAddr3 = txtAddr3.Text;
                    objUserDetails.objUserAddr.nPincode = Convert.ToInt32(txtPincode.Text);
                    objUserDetails.objUserAddr.vEmailId = txtEmailId.Text;
                    objUserDetails.objUserAddr.vMobileNos = txtMobileNos.Text;
                    strReturnMessage= SaveUserDetails(objUserDetails);
                    lblSuccessMsg.Text = strReturnMessage;
                    if (strReturnMessage.ToUpper().Contains("ACKNOWLEDGEMENT"))
                    {
                        string strFullName = ddlSalutation.SelectedItem.Text + " "+ objUserDetails.vFirstName + " " + objUserDetails.vMiddleName + " " + objUserDetails.vLastName;
                        lblSuccessMsg.ForeColor = System.Drawing.Color.Green;
                        string strMsg = ConfigurationManager.AppSettings["Techno_SMS"].ToString();
                        strMsg = strMsg.Replace("#ScratchCardNos", txtScratchCardUniqueNos.Text);
                        strMsg = strMsg.Replace("#IMEINos", objUserDetails.vPrimaryIMEINos);
                        HideControl();                     
                        fnSendEmail(objUserDetails.objUserAddr.vEmailId, strFullName, objUserDetails.vPrimaryIMEINos, objUserDetails.vScratchCardNos);                     
                        //SendSMS(strMsg, objUserDetails.objUserAddr.vMobileNos);
                        AsynSendSMS(strMsg, objUserDetails.objUserAddr.vMobileNos);
                        ScriptManager.RegisterStartupScript(UpdatePanel_OPS, UpdatePanel_OPS.GetType(), "testpage", "Confirm('" + strReturnMessage + "')", true);
                    }
                    else
                    {
                        lblSuccessMsg.ForeColor = System.Drawing.Color.Red;
                    }
                //}
            }
            catch(Exception ex)
            {
                ExceptionUtility.LogException(ex, "Fn_Register_Policy");
                Alert.Show(ex.Message);
            }
        }

        public void OnConfirm()
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                ResetControl();
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }
        }



        protected void btnValidate_Click(object sender, EventArgs e)
        {
            #region Start
            try
            {
                clsTechnoIMEIScratchCardMapping objTechnoIMEIScratchCardMapping = new clsTechnoIMEIScratchCardMapping();
                objTechnoIMEIScratchCardMapping.vScratchCardNos = txtScratchCardUniqueNos.Text;
                objTechnoIMEIScratchCardMapping.vPrimaryIMEINos = txtUniqueIdentificationNos.Text;
                string strErrorMsg = Fn_Validate_IMEI_ScratchCard_And_GetData(objTechnoIMEIScratchCardMapping);
                //strErrorMsg = Fn_Validate_PurchaseDate(dPurchaseDate);

                if (strErrorMsg.Trim() != "")
                {
                    pnlReg.Visible = false;
                    captchaPanel.Visible = false;
                    btnSave.Visible = false;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = strErrorMsg;
                    Alert.Show(strErrorMsg);
                }
                else
                {
                    pnlReg.Visible = true;
                    captchaPanel.Visible = true;
                    btnSave.Visible = true;
                    lblMsg.Text = "";
                }
                UpdatePanel_OPS.Update();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "btnValidate_Click");            
            }
            #endregion End
        }
        public void Fn_Validate_MobileNo(object source, ServerValidateEventArgs args)
        {
            
            string searchText = txtMobileNos.Text;
            string regexPattern1 = @"9";
            string regexPattern2 = @"0";
            int Count1 = Regex.Matches(searchText, regexPattern1).Count;
            int Count2 = Regex.Matches(searchText, regexPattern2).Count;
            if (Count1 == 10)
            {
                CustomValidator2.ErrorMessage = "Please enter valid Mobile No.";
                args.IsValid = false;
            }
            else if (Count2 == 9)
            {
                CustomValidator2.ErrorMessage = "Please enter valid Mobile No.";
                args.IsValid = false;
            }
            else
            {
                CustomValidator2.ErrorMessage = "";
                args.IsValid = true;
            }
        }
        public string Fn_Validate_IMEI_ScratchCard_And_GetData(clsTechnoIMEIScratchCardMapping objTechnoIMEIScratchCardMapping)
        {
            string ReturnMessage = "";
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            //Cls_General_Functions wsDocNo = new Cls_General_Functions();
            //string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString(); string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();
            //string vEmpCode = objUserDetails.vUserLoginId.ToUpper();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("PROC_TECHNO_VALIDATE_IMEI_SCRATCHCARD_AND_GETDATA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@vPrimaryIMEINos", objTechnoIMEIScratchCardMapping.vPrimaryIMEINos);
                cmd.Parameters.AddWithValue("@vScratchCardNos", objTechnoIMEIScratchCardMapping.vScratchCardNos);
                SqlParameter outputMessage = new SqlParameter("@outputMessage", SqlDbType.VarChar, 200);
                outputMessage.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputMessage);
                string strMasterPolicyHolderName = Convert.ToString(cmd.ExecuteScalar());
                if (strMasterPolicyHolderName.Trim() != "")
                    txtMasterPolicyHolderName.Text = strMasterPolicyHolderName;

                ReturnMessage = outputMessage.Value.ToString();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Fn_Validate_IMEI_And_ScratchCard");
            }
            return ReturnMessage;
        }
        protected void Fn_Validate_Id_Proof(object source, ServerValidateEventArgs args)
        {
            try
            {
                String regExpPCNo = "";
                String regExpPPNo = "";
                String regExpDLNo = "";

                string strErrorMsg = "";

                string Type = ddlCustomerIdProof.SelectedValue.ToString();
                Regex regexId = null;
                Match match = null;
                regExpPCNo = @"[A-Z]{5}\d{4}[A-Z]{1}";
                regExpDLNo = @"[a-zA-Z]{2}[0-9]{13}$";
                regExpPPNo = @"[a-zA-Z][0-9]{6,7}$";
                if (Type == "1" || Type == "2" || Type == "4")
                {
                    if (Type == "1")
                    {
                        regexId = new Regex(regExpPCNo, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                        match = regexId.Match(txtUniqueProofNos.Text);
                        //if(!match.Success)
                        strErrorMsg = "Invalid PAN CARD nos";
                        //"Length has to be 10.First 5 characters has to be Alphabets,followed by 4 Numeric digits.Last Digit has to be Alphabet.";
                    }
                    else if (Type == "2")
                    {
                        regexId = new Regex(regExpPPNo, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                        match = regexId.Match(txtUniqueProofNos.Text);
                        //if(!match.Success)
                        strErrorMsg = "Invalid Passport Nos";
                    }
                    else if (Type == "4")
                    {
                        regexId = new Regex(regExpDLNo, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                        match = regexId.Match(txtUniqueProofNos.Text);
                        //if(!match.Success)
                        strErrorMsg = "Invalid Driving License Nos";
                    }

                    if (match.Success)
                    {
                        //MessageBox.Show(match.Value);
                        args.IsValid = true;
                    }
                    else
                    {
                        CustomValidator1.ErrorMessage = strErrorMsg;
                        args.IsValid = false;
                        Alert.Show(strErrorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Fn_Validate_Id_Proof");
                CustomValidator1.ErrorMessage = ex.Message;
                args.IsValid = false;
            }
        }
        public string GetFormattedMonthOrDate(string Value)
        {
            if (Convert.ToInt32(Value) < 10)
            {
                Value = "0" + Value;
            }
            return Value;
        }
        public void Fn_Validate_PurchaseDate(object source, ServerValidateEventArgs args)
        {

            if (txtUniqueIdentificationNos.Text != "")
            {
                string PurchaseDate = GetFormattedMonthOrDate(ddlPDD.SelectedItem.Text) + "/" + GetFormattedMonthOrDate(ddlPMM.SelectedItem.Text) + "/" + ddlPYYYY.SelectedItem.Text;
                DateTime dPurchaseDate = DateTime.ParseExact(PurchaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string ReturnMessage = "";
                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString);
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("PROC_TECHNO_VALIDATE_PURCHASEDATE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dPurchaseDate", dPurchaseDate);
                    cmd.Parameters.AddWithValue("@vPrimaryIMEINos", txtUniqueIdentificationNos.Text);

                    int i = Convert.ToInt32(cmd.ExecuteScalar());
                    if (i == 1)
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        args.IsValid = false;
                        CVPurchaseDate.ErrorMessage = "Registration of policy cannot be done for this policy";
                        //Alert.Show(ReturnMessage);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogException(ex, "Fn_Validate_PurchaseDate");
                    args.IsValid = false;
                    CVPurchaseDate.ErrorMessage = ex.Message;
                    Alert.Show(ex.Message);
                }
            }
        }

        //protected void Fn_Validate_MobileNos(object source, ServerValidateEventArgs args)
        //{
        //    try
        //    {
        //        String regExp = "";
        //        string strErrorMsg = "";

        //        string Type = ddlCustomerIdProof.SelectedValue.ToString();
        //        Regex regexId = null;
        //        Match match = null;
        //        regExp = @"[A-Z]{5}\d{4}[A-Z]{1}";
                
        //        regexId = new Regex(regExp, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        //        match = regexId.Match(txtUniqueProofNos.Text);
        //        if (match.Success)
        //        {
        //            args.IsValid = true;
        //        }
        //        else
        //        {
        //            strErrorMsg = "Invalid Mobile Nos";
        //            cvMobile.ErrorMessage = strErrorMsg;
        //            args.IsValid = false;
        //            Alert.Show(strErrorMsg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogException(ex, "Fn_Validate_Id_Proof");
        //        cvMobile.ErrorMessage = ex.Message;
        //        args.IsValid = false;
        //    }
        //}

        [WebMethod]
      //[System.Web.Script.Services.ScriptMethod()]
        public static string Fn_Get_City_And_State_By_Pincode(int nPincode)
        {
            string responseJson = "";
            //int nPincode = 210001;
            DataSet dsCityState = null;

            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            try
            {
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    SqlCommand cmdCheck = new SqlCommand("PROC_GET_CITY_AND_STATE_BY_PINCODE", con);
                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.Parameters.AddWithValue("@vPincode", nPincode);
                    //cmdCheck.Parameters.AddWithValue("@vextraVerify", extraparam);

                    SqlDataAdapter sda = new SqlDataAdapter(cmdCheck);
                    dsCityState = new DataSet();
                    sda.Fill(dsCityState);
                }
            }
            catch (Exception ex)
            {

            }

            string City = string.Empty;
            string State = string.Empty;
            List<string> strLCityState = new List<string>();
            if (dsCityState != null && dsCityState.Tables.Count > 0 && dsCityState.Tables[0].Rows.Count > 1)
            {
                City = dsCityState.Tables[0].Rows[0]["City"].ToString();
                State = dsCityState.Tables[0].Rows[0]["State"].ToString();
                //responseJson = "{City: '" + City + "', State: '" + State + "'}";
                responseJson = City + "|" + State;
                //strLCityState.Add(string.Format("{0}~{1}", City, State));
            }
            //return strLCityState.ToArray();
            return responseJson;
        }
        protected void btnProceed_Click(object sender, EventArgs e)
        {

        }

        #region OTP     

        public DataSet Fn_Insert_OTP_Data(string vUniqueID, string vAppID, string vMobileNo, string vOTPNumber)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                    SqlCommand command = new SqlCommand("INSERT_OTPAD_DATA", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@mobileno", vMobileNo);
                    command.Parameters.AddWithValue("@uniqueID", vUniqueID);
                    command.Parameters.AddWithValue("@appID", vAppID);
                    command.Parameters.AddWithValue("@otpNumber", vOTPNumber);

                    DataSet myDataSet = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(myDataSet);
                    return myDataSet;
                }
            }

            catch (Exception ex)
            {
                //File.AppendAllText(ConfigurationManager.AppSettings["logfile"] + logfile, "KGI_ADService : Error occured in InsertOTPData " + ex.Message + " and stack trace is : " + ex.StackTrace + " " + DateTime.Now + Environment.NewLine);
                return null;
            }
        }


        private string GenerateOTPNew(string mobileNumber)
        {
            string GeneratedOTP = string.Empty;
            try
            {
                Random r = new Random();
                GeneratedOTP = r.Next(100000, 999999).ToString();
                //if (ConfigurationManager.AppSettings["TECHNO_IsUAT"].ToString().ToUpper() == "TRUE")
                //{
           
                //    HttpContext.Current.Session["OTPNumber"] = ConfigurationManager.AppSettings["TECHNO_OTP"].ToString().ToUpper();
                //}
                //else
                //{
                    HttpContext.Current.Session["OTPNumber"] = GeneratedOTP;
                //}

                if (mobileNumber.Length == 10)
                {
                    string strPath = string.Empty;
                    string smsBody = string.Empty;

                    Random ran = new Random();
                    int nOTPNumber = ran.Next(100000, 999999);
                    DataSet ds = new DataSet();

                    ds = Fn_Insert_OTP_Data(txtUniqueIdentificationNos.Text, "TECHNO", mobileNumber, nOTPNumber.ToString());

                    bool IsSendSMSSuccess = SendOTPSMS(GeneratedOTP, mobileNumber);

                    File.WriteAllText(FrmDownloadPolicyScheduleLog, "OTP " + GeneratedOTP + "sent to Mobile number " + mobileNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);

                }
            }
            catch (Exception ex)
            {
                GeneratedOTP = "";
                ExceptionUtility.LogException(ex, "Error in GenerateOTP on FrmSTP Page");
            }
            return string.IsNullOrEmpty(GeneratedOTP) ? "" : "success";
        }
        private bool SendOTPSMS(string GeneratedOTP, string MobileNumber)
        {
            bool IsSendSMSSuccess = false;

            try
            {
                string strPath = string.Empty;
                string smsBody = string.Empty;

                smsBody = ConfigurationManager.AppSettings["smsBody"];
                smsBody = smsBody.Replace("@otpNumber", Convert.ToString(GeneratedOTP));

                if (ConfigurationManager.AppSettings["IsUseNetworkProxy"].ToString() == "0")
                {
                    // string URI = string.Format("http://otp.zone:7501/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);
                    var client = new System.Net.WebClient();
                    var content = client.DownloadString(URI);
                }
                else
                {
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

                    string URI = string.Format("https://japi.instaalerts.zone/failsafe/HttpLink?aid=640548&pin=kot@12&mnumber={0}&signature=KOTAKG&message={1}", MobileNumber, smsBody);


                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;


                    var client = new System.Net.WebClient();
                    client.Proxy = proxy;

                    client.Proxy = WebRequest.DefaultWebProxy;
                    client.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);
                    client.Proxy.Credentials = new NetworkCredential(network_Username, network_Password, network_domain);

                    var content = client.DownloadString(URI);
                    IsSendSMSSuccess = true;
                    File.WriteAllText(FrmDownloadPolicyScheduleLog, "OTP " + GeneratedOTP + "sent to mobile Number" + MobileNumber + "  " + DateTime.Now.ToString() + System.Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                IsSendSMSSuccess = false;
                ExceptionUtility.LogException(ex, "Error in SendSMS on FrmTechno Page");
                //sectionMain.Visible = false;
                //sectionError.Visible = true;
            }
            return IsSendSMSSuccess;
        }
        private void fnSendEmail(string emailId,string FullName,string IMEINos,string ScratchCardNos)
        {
            try
            {
                //string emailId = txtEmailforPolicy.Text.Trim();
                string strPath = string.Empty;
                string MailBody = string.Empty;
                string filename = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.Host = ConfigurationManager.AppSettings["smtp_mail_host"];
                smtpClient.Timeout = 3600000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtp_mail_username"], ConfigurationManager.AppSettings["smtp_mail_password"]);
                strPath = AppDomain.CurrentDomain.BaseDirectory + "Techno_Deposit_Receipt.html";
                MailBody = File.ReadAllText(strPath);

                MailBody = MailBody.Replace("#FullName", FullName);
                MailBody = MailBody.Replace("#IMEINos", IMEINos);
                MailBody = MailBody.Replace("#ScratchCardNos", ScratchCardNos);
                MailBody = MailBody.Replace("#Date",  DateTime.Today.ToString("dd/MM/yyyy") );

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(ConfigurationManager.AppSettings["Techno_Mail_FromMailId"]);
                mm.Subject = "Confirmation for Corona Kavach Group Policy, Kotak Mahindra General Insurance Company Limited";
                mm.Body = MailBody;
                mm.To.Add(emailId);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                
                smtpClient.Send(mm);
                //fnAddLogGistPolicyDownload(emailId, CustomerName, certificateNo, Session["vUserLoginId"].ToString().ToUpper(), "Email");
                Alert.Show("Mail sent on Email ID " + emailId);
               
            }
            catch (Exception ex)
            {
                Alert.Show("Some Error Occured while sending email to " + emailId);
            }
        }     
        private static int SendSMS(string MsgString, string MobileNo)
        {

            int IsSent = 0;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnConnect");

                //messageString = messageString.Replace("@payutxnid", mihpayid);

                string mobileNum = MobileNo;
                if (!String.IsNullOrEmpty(mobileNum) && !String.IsNullOrEmpty(MsgString))
                {

                    using (SqlConnection con = new SqlConnection(db.ConnectionString))
                    {
                        con.Open();
                        SqlCommand cmdCheck = new SqlCommand("INSERT_DATA_CUSTOMER_SMS_LOG_BPOS", con);
                        cmdCheck.CommandType = CommandType.StoredProcedure;
                        cmdCheck.Parameters.AddWithValue("@mobile", mobileNum);
                        cmdCheck.Parameters.AddWithValue("@msg", MsgString);
                        cmdCheck.ExecuteNonQuery();
                    }
                    IsSent = 1;
                }
                return IsSent;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex,"SendSMS");
                //File.AppendAllText(folderpath + logfile, ex.Message + Environment.NewLine);
                return 0;
            }
        }

        #endregion

        private bool VerifyIntermediary(string IntermediaryCode)
        {
            bool IsCorrect = false;
            if (string.IsNullOrEmpty(IntermediaryCode))
            {
                IsCorrect = true;
            }
            else
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_VERIFY_INTERMEDIARY";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "INTERMEDIARY_CODE", DbType.String, ParameterDirection.Input, "INTERMEDIARY_CODE", DataRowVersion.Current, IntermediaryCode);

                DataSet ds = null;
                ds = db.ExecuteDataSet(dbCommand);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsCorrect = ds.Tables[0].Rows[0][0].ToString() == "1" ? true : false;
                    }
                }
            }
            return IsCorrect;
        }
    
        private bool IsError(out string ErrorMsg)
        {
            bool IsError = false;
            ErrorMsg = "";
            //string MarketMovementDeviation = ConfigurationManager.AppSettings["MarketMovementDeviation"].ToString();

            //if (!Regex.IsMatch(txtMinMarketMovementDeviation.Text.Trim(), @"^-?\d{0,9}(\.\d{1,2})?$"))
            //{
            //    IsError = true;
            //    ErrorMsg = "Please Enter Valid Market Movement, Only 2 digits after decimal are allowed and allowed characters are numbers, single dot(.) and minus(-) symbol";
            //}
            //else if (Convert.ToDecimal(txtMinMarketMovementDeviation.Text) < Convert.ToDecimal(MarketMovementDeviation) || Convert.ToDecimal(txtMinMarketMovementDeviation.Text) > Convert.ToDecimal(0))
            //{
            //    IsError = true;
            //    ErrorMsg = "Please Enter Valid Market Movement, It should be between 0 to " + MarketMovementDeviation;
            //}
            //else if (chkIsExternalUser.Checked && string.IsNullOrEmpty(txtIntermediaryCode.Text.Trim()))
            //{
            //    IsError = true;
            //    ErrorMsg = "Please select correct intermediary code as you have selected external user";
            //}
            //else if (chkIsExternalUser.Checked && string.IsNullOrEmpty(hfIntermediaryCode.Value.Trim()))
            //{
            //    IsError = true;
            //    ErrorMsg = "Please select correct intermediary code as you have selected external user";
            //}
            //else if (txtIntermediaryCode.Text.Trim() != hfIntermediaryCode.Value.Trim())
            //{
            //    IsError = true;
            //    ErrorMsg = "Entered intermediary code does not match with selected intermedairy code";
            //}
            //else if (chkIsExternalUser.Checked == false && txtUserId.Text.StartsWith("G") == false)
            //{
            //    IsError = true;
            //    ErrorMsg = "For Internal User Creation Login Id Must Start With 'G' letter";
            //}
            return IsError;
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
        protected void btnGetIntermediaryCode_Click(object sender, EventArgs e)
        {
            //string IntermediaryCode = hfIntermediaryCode.Value;
            //lblIntermediaryCode.Text = IntermediaryCode;
            //SetIntermediaryBusinessChaneelType(IntermediaryCode);
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
                            IntrCds.Add(string.Format("{0}~{1}", sdr["TXT_INTERMEDIARY_CD"], sdr["TXT_INTERMEDIARY_NAME"]));
                        }
                    }
                    conn.Close();
                }
            }
            return IntrCds.ToArray();
        }

        private UserDetails CreateUser(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("No User detail exist.");
            }

            return new UserDetails
            {
                vUserLoginId = Convert.ToString(reader["vUserLoginId"]),
                vUserLoginDesc = Convert.ToString(reader["vUserLoginDesc"]),
                vUserEmailId = Convert.ToString(reader["vUserEmailId"]),
                vIntermediaryCode = Convert.IsDBNull(reader["vIntermediaryCode"]) ? string.Empty : Convert.ToString(reader["vIntermediaryCode"]),
                vIntermediaryBranch = Convert.IsDBNull(reader["vIntermediaryBranch"]) ? string.Empty : Convert.ToString(reader["vIntermediaryBranch"]),
                bIsActivate = Convert.ToString(reader["bIsActivate"]).ToUpper() == "Y" ? true : false //Convert.ToBoolean(reader["bIsActivate"])
            };
        }

        protected void OboutChkMobileLogin_CheckedChanged(object sender, EventArgs e)
        {
            //if (OboutChkMobileLogin.Checked == true)
            //{

            //    drpUserType.SelectedValue = "BPOS";
            //    drpUserType.Enabled = false;
            //    OboutChkEPOSQuoteView.Checked = false;
            //    OboutChkEPOSQuoteView.Enabled = false;
            //}
            //else
            //{
            //    OboutChkEPOSQuoteView.Enabled = true;
            //    drpUserType.Enabled = true;
            //    drpUserType.SelectedValue = "KGI";
            //}
            return;
        }

        protected void OboutReset_Click(object sender, EventArgs e)
        {
            string msg = "";
            //ScriptManager.RegisterStartupScript(UpdatePanel_OPS, UpdatePanel_OPS.GetType(), "testpage", "Confirm('" + msg + "')", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "Confirm('" +msg+ "')", true);
            //ShowSuccessMsg("xyz");
            //stopWaitHandle.WaitOne();
             ResetControl();
            //ResetControl();
            //Response.Redirect("FrmCreateUserMasterNewEdit.aspx");
        }

        protected void txtPincode_TextChanged(object sender, EventArgs e)
        {
            DataSet dsCityState = Fn_Get_City_AND_State_By_Pincode_DataSet(txtPincode.Text);
        if (dsCityState != null && dsCityState.Tables.Count > 0 && dsCityState.Tables[0].Rows.Count > 1)
            {
                txtCity.Text = dsCityState.Tables[0].Rows[0]["City"].ToString();
                txtState.Text = dsCityState.Tables[0].Rows[0]["State"].ToString();
            }
        }

        

        protected void Button1_Click(object sender, EventArgs e)
        {
            HideControl();
            AsynSendSMS("","");
        }
        //[WebMethod]
        //[System.Web.Script.Services.ScriptMethod()]
    }
}

    