using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace PrjPASS
{
    public partial class FrmMotorLeadCapture : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["apac"] != null)
                {
                    string APACNumber = Request.QueryString["apac"].ToString();

                    DataSet dsAPACDetails = new DataSet();

                    dsAPACDetails = GetAPACDetails(APACNumber);

                    if (dsAPACDetails != null)
                    {
                        if (dsAPACDetails.Tables.Count > 0)
                        {
                            if (dsAPACDetails.Tables[0].Rows.Count > 0)
                            {
                                SetAllFields(dsAPACDetails);
                                sectionMain.Visible = true;
                                sectionThankYou.Visible = false;
                            }
                            else
                            {
                                sectionRecordNotFound.Visible = true;
                                sectionMain.Visible = false;
                                sectionThankYou.Visible = false;
                                btnCallback.Visible = false;

                            }
                        }
                        else
                        {
                            sectionRecordNotFound.Visible = true;
                            sectionMain.Visible = false;
                            sectionThankYou.Visible = false;
                            btnCallback.Visible = false;
                        }
                    }
                    else
                    {
                        sectionRecordNotFound.Visible = true;
                        sectionMain.Visible = false;
                        sectionThankYou.Visible = false;
                        btnCallback.Visible = false;
                        //Alert.Show("No Data Exists for the given APAC Number", "");
                        //return;
                    }
                }
                else
                {
                    sectionRecordNotFound.Visible = true;
                    sectionMain.Visible = false;
                    sectionThankYou.Visible = false;
                    btnCallback.Visible = false;
                }
            }
        }

        private DataSet GetAPACDetails(string APAC_Number)
        {
            DataSet ds = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase("cnPASS");

                string sqlCommand = "PROC_GET_MOTOR_LEADS_USING_APAC";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddParameter(dbCommand, "APAC_Number", DbType.String, ParameterDirection.Input, "APAC_Number", DataRowVersion.Current, APAC_Number);

                dbCommand.CommandType = CommandType.StoredProcedure;

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "GetAPACDetails Method");
            }
            return ds;
        }

        private void SetAllFields(DataSet dsAPACDetails)
        {
            try
            {
                DataTable dtAPACDetails = dsAPACDetails.Tables[0];

                lblApacNumber.Text = dtAPACDetails.Rows[0]["APAC_Number"].ToString();
                lblTotalPremium2.Text = Convert.ToDecimal(dtAPACDetails.Rows[0]["TotalPremiumIncludingGST"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                lblMakeModelVariant.Text = dtAPACDetails.Rows[0]["VehicleMake"].ToString() + " " + dtAPACDetails.Rows[0]["VehicleModel"].ToString() + " " + dtAPACDetails.Rows[0]["VehicleVariant"].ToString();

                string input = dtAPACDetails.Rows[0]["VehicleRegistrationNumber"].ToString();

                string strLastFourDigit = input.Substring(input.Length - 4);
                string strAllCharSkipLastFour = input.Remove(input.Length - 4);

                lblRegistrationNumber.Text = Regex.Replace(strAllCharSkipLastFour, ".{2}", "$0 ") + " " + strLastFourDigit;

                lblFuelType.Text = dtAPACDetails.Rows[0]["FuelType"].ToString();
                lblNCB.Text = dtAPACDetails.Rows[0]["NCB"].ToString();
                lblBasicODPremium.Text = Convert.ToDecimal(dtAPACDetails.Rows[0]["BasicOwnDamagePremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                lblIDV.Text = Convert.ToDecimal(dtAPACDetails.Rows[0]["IDV"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                lblNCBDiscount.Text = Convert.ToDecimal(dtAPACDetails.Rows[0]["NCBDiscount"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                lblNetODPremium.Text = Convert.ToDecimal(dtAPACDetails.Rows[0]["NetODPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                lblBasicTPPremium.Text = Convert.ToDecimal(dtAPACDetails.Rows[0]["BasicTPPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                lblPAForOwnerDriver.Text = Convert.ToDecimal(dtAPACDetails.Rows[0]["PAForOwnerDriver"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                lblNetPremium2.Text = Convert.ToDecimal(dtAPACDetails.Rows[0]["TotalNetPremium"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();

                decimal TotalPremium = Convert.ToDecimal(dtAPACDetails.Rows[0]["TotalNetPremium"]);
                decimal GST = (TotalPremium * 18) / 100;
                lblGST.Text = Convert.ToDecimal(GST.ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();
                lblTotalPremium.Text = Convert.ToDecimal(dtAPACDetails.Rows[0]["TotalPremiumIncludingGST"].ToString()).ToIndianCurrencyFormatWithoutRuppeeSymbol();

                SendEmail();

                UpdateIsPageViewedFlag();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "SetAllFields Method");
            }
        }

        private void UpdateIsPageViewedFlag()
        {
            try
            {
                if (lblApacNumber.Text.Trim() != "" && lblApacNumber.Text.Trim() != "-")
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "PROC_SAVE_APAC_PAGE_VIEWED_FLAG";

                            cmd.Parameters.AddWithValue("@APACNumber", lblApacNumber.Text.Trim());

                            cmd.Connection = conn;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "UpdateIsPageViewedFlag Method");
            }
        }

        protected void btnCallback_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    if (lblApacNumber.Text.Trim() != "" && lblApacNumber.Text.Trim() != "-")
                    {
                        using (SqlConnection conn = new SqlConnection())
                        {
                            conn.ConnectionString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "PROC_SAVE_MOTOR_LEADS_USING_APAC_CALLBACK_RESPONSE";

                                cmd.Parameters.AddWithValue("@APACNumber", lblApacNumber.Text.Trim());

                                cmd.Connection = conn;
                                conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();

                                sectionMain.Visible = false;
                                sectionThankYou.Visible = true;
                                sectionRecordNotFound.Visible = false;
                                hdnIsDisclaimerShown.Value = "1";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "Error on click of Callback button, btnCallback_Click");
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
           //args.IsValid = chkIAgree.Checked;
        }

        //send mail on load

        private string SendEmail()
        {
            string ToEmailIds = ConfigurationManager.AppSettings["APACPageVisitedMailTo"].ToString();
            string strMessage = string.Empty;
            string[] ToEmailAddr = ToEmailIds.Split(';');

            string smtp_Username = ConfigurationManager.AppSettings["smtp_Username"].ToString();
            string smtp_Password = ConfigurationManager.AppSettings["smtp_Password"].ToString();
            string smtp_Host = ConfigurationManager.AppSettings["smtp_Host"].ToString();
            string smtp_FromMailId = ConfigurationManager.AppSettings["APACPageVisited_FromMailId"].ToString();

            string strPath = string.Empty;
            string MailBody = string.Empty;

            string attachmentFilename = null;

            string PayULink = string.Empty;
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = smtp_Host; //"192.168.201.61"; //"kgirelay.kgi.kotakgroup.com";
                                         //client.EnableSsl = true;
                client.Timeout = 3600000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(smtp_Username, smtp_Password);


                strPath = AppDomain.CurrentDomain.BaseDirectory + "EmailBodyForAPAC.html";
                MailBody = File.ReadAllText(strPath);
                MailBody = MailBody.Replace("lblApacNumber", lblApacNumber.Text.Trim());
                
                MailBody = MailBody.Replace("lblMakeModelVariant", lblMakeModelVariant.Text.Trim());
                MailBody = MailBody.Replace("lblRegistrationNumber", lblRegistrationNumber.Text.Trim());
                MailBody = MailBody.Replace("lblFuelType", lblFuelType.Text.Trim());

                MailBody = MailBody.Replace("lblNCB", lblNCB.Text.Trim());
                MailBody = MailBody.Replace("lblBasicODPremium", lblBasicODPremium.Text.Trim());
                MailBody = MailBody.Replace("lblIDV", lblIDV.Text.Trim());
                MailBody = MailBody.Replace("lblDiscountNCB", lblNCBDiscount.Text.Trim());
                MailBody = MailBody.Replace("lblNetODPremium", lblNetODPremium.Text.Trim());
                MailBody = MailBody.Replace("lblBasicTPPremium", lblBasicTPPremium.Text.Trim());
                MailBody = MailBody.Replace("lblPAForOwnerDriver", lblPAForOwnerDriver.Text.Trim());

                MailBody = MailBody.Replace("lblNetPremium", lblNetPremium2.Text.Trim());
                MailBody = MailBody.Replace("lblGST", lblGST.Text.Trim());
                MailBody = MailBody.Replace("lblTotalPremium", lblTotalPremium.Text.Trim());


                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(smtp_FromMailId);
                mm.Subject = "Information: APAC (" + lblApacNumber.Text.Trim() +") Visited";
                mm.Body = MailBody;
                mm.IsBodyHtml = true;

                foreach (var toMailId in ToEmailAddr)
                {
                    if (toMailId != null)
                    {
                        if (toMailId.Length > 5)
                        {
                            mm.To.Add(toMailId);
                        }
                    }
                }

                //mm.CC.Add(smtp_DefaultCCMailId);

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (attachmentFilename != null)
                {
                    Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mm.Attachments.Add(attachment);
                }

                client.Send(mm);
                strMessage = "success";
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                ExceptionUtility.LogException(ex, "Error in SendMail, FrmMotorLeadCapture.aspx");
            }

            return strMessage;
        }
    }
}