using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text;

namespace ProjectPASS
{
    public partial class FrmKavachCancelUpload : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();
        //
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
        }

        protected void Upload(object sender, EventArgs e)
        {
            //string allowedExtensions = ".xlsx";

            //if (FileUpload1.HasFile)
            //{
            //    String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

            //    if (fileExtension != allowedExtensions)
            //    {
            //        Session["ErrorCallingPage"] = "FrmKAVACHCancelUpload.aspx";
            //        string vStatusMsg = "Invalid file Extension, Only XLSX files are allowed to be Uploaded";
            //        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
            //        return;
            //    }

            //}
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cYearMonth = "", vUploadId = "";


            SqlConnection _con = new SqlConnection(dbCOMMON.ConnectionString);
            _con.Open();
            SqlTransaction _tran = _con.BeginTransaction();

            cYearMonth = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                cYearMonth = cYearMonth + "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                cYearMonth = cYearMonth + DateTime.Now.Month.ToString();
            }

            vUploadId = wsDocNo.fn_Gen_Doc_Master_No("CANUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);

            try
            {

                //Upload and save the file

                string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(FileUpload1.PostedFile.FileName);

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

                    string sqlCommand = "SELECT * FROM TBL_KAVACH_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N' and vSourceColumnName in ('UploadId','ProductName','BusinesssType','RMCode','BranchName','BranchCode','CrnNo','AccountNo','CustomerName','CustomerGender','CustomerDob','ProposerAddLine1','ProposerAddLine2','ProposerAddLine3','ProposerCity','ProposerState','ProposerPinCode','UniqueIdentificationNo','MobileNo','EmailId','NomineeName','NomineeRelation','NomineeAge','NomineeGuardian','NomineeRelWithGuardian','UniqueAccDebitRefNo','AccountDebitDate','DebitAmount','IntermediaryName','IntermediaryCode','CorporateName','PolicyEndDate','PlanName','MasterPolicyNo','BankName','NetPremium','LoadingDiscount','TotalPolicyPremium','PolicyStartDate','ProposalDate','CertificateNo','AccidentalDeathSI','PermTotalDiabilitySI','PermPartialDiabilitySI','TempTotalDiabilitySI','AmbulanceCoverSI','ModificationAllowanceSI','CostOfSupportItemsSI','OutPatientTreatCoverSI','ChildEduGrantSI','MarriageBenefitChildSI','DisappearBenefitSI','CompassVisitSI','SportsActCoverSI','CarriageOfDeadBodySI','FuneralExpenseSI','DoubleInsuranceBenifitSI','HospitalCashSI','ConvalescenceSI','BurnBenefitSI','BrokenBoneBenefitSI','ComaBenefitSI','InPatientHosSI','DomTravelForMedTreatSI','LossOfJobSI','EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus','ChallanNo','ChallanDate','BankBranch','PolicyTenure','LanNo','LoanTenure','LoanDisbursementDate','LoanSanctionDate','LoanOutAmount','CustomerType','Occupation','RelationWithInsured','Comments','AddCol1','AddCol2','AddCol3','ProductCode','IntermediaryContactDetail','PreviousPolicyNumber','AppointeeName','PolicyType')";

                    DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);

                    DataSet ds = null;

                    ds = dbCOMMON.ExecuteDataSet(dbCommand);

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

                        for (int i = 1; i <= dtExcelData.Columns.Count; i++)
                        {
                            dtExcelData.Columns[i - 1].ColumnName = dtExcelData.Columns[i - 1].ColumnName.ToString().TrimStart('v', 'd', 'n', 'b');
                            if (dtExcelData.Columns[i - 1].ColumnName.StartsWith("Additional"))
                            {
                                dtExcelData.Columns[i - 1].ColumnName = dtExcelData.Columns[i - 1].ColumnName.Replace("Additional_", "Additional ");
                            }

                            if (dtExcelData.Columns[i - 1].ColumnName == "ChallanNumber")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "ChallanNo";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "IntermediaryNumber")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "IntermediaryContact";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "Covid_19Diagnosis")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Covid-19 Diagnosis";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "ICUDailyCash")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "ICU Daily Cash";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "In_PatientHospitalisation")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "In-patient Hospitalisation";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "HomeCareTreatmentExpenses")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Home Care Treatment Expenses";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "AYUSH_Treatment")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "AYUSH Treatment";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "PreHospitalizationMedicalExpenses")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Pre Hospitalization Medical Expenses";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "PostHospitalizationMedicalExpenses")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Post Hospitalization Medical Expenses";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "HospitalDailyCash")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Hospital Daily Cash";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "RoomRentCapping")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Room Rent Capping";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "NoOfLives")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "No of Lives";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "Condition1")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Condition 1";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "Condition2")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Condition 2";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "Condition3")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Condition 3";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "Condition4")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Condition 4";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "Condition5")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Condition 5";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "Condition6")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Condition 6";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "Condition7")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Condition 7";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "AdditionalCover1")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Additional Cover 1";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "AdditionalCover2")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Additional Cover 2";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "AdditionalCover3")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Additional Cover 3";
                            }
                            else if (dtExcelData.Columns[i - 1].ColumnName == "AdditionalCover4")
                            {
                                dtExcelData.Columns[i - 1].ColumnName = "Additional Cover 4";
                            }

                        }

                        if (!dtExcelData.Columns.Contains("vErrorFlag"))
                        {
                            dtExcelData.Columns.Add("vErrorFlag");
                        }

                        if (!dtExcelData.Columns.Contains("vErrorDesc"))
                        {
                            dtExcelData.Columns.Add("vErrorDesc");
                        }

                        if (!dtExcelData.Columns.Contains("vTransType"))
                        {
                            dtExcelData.Columns.Add("vTransType");
                        }

                        if (!dtExcelData.Columns.Contains("UploadId"))
                        {
                            dtExcelData.Columns.Add("UploadId");
                        }

                        foreach (DataRow excelrow in dtExcelData.Rows)
                        {

                            string vDestinationFieldName = "";
                            string vSourceFieldName = "";
                            string vFieldValue = "";
                            string vErrorDesc = "";
                            string bMandatoryForPolicy = "";
                            string vCertificateNo = "";
                            string[] chkValidFlag;

                            if (GetMappingData == true)
                            {
                                bool insertflag = true;

                                for (int i = 1; i <= dtExcelData.Columns.Count; i++)
                                {
                                    string trimString = dtExcelData.Columns[i - 1].ColumnName.ToString().TrimStart('v');

                                    string searchExpression = "vSourceColumnName = '" + trimString.Trim() + "'";

                                    DataRow[] foundRows = ds.Tables[0].Select(searchExpression);

                                    if (foundRows.Count() > 0)
                                    {
                                        vDestinationFieldName = foundRows[0]["vDestinationColumnName"].ToString();
                                        vSourceFieldName = foundRows[0]["vSourceColumnName"].ToString();
                                        vFieldValue = excelrow[dtExcelData.Columns[i - 1].ColumnName.ToString().Trim()].ToString();
                                        bMandatoryForPolicy = foundRows[0]["bMandatoryForPolicy"].ToString();

                                        if (vSourceFieldName != "vCertificateNo")
                                        {
                                            vCertificateNo = excelrow[dtExcelData.Columns["CertificateNo"].ToString().Trim()].ToString();
                                        }

                                        if (bMandatoryForPolicy == "Y" && vFieldValue.Trim().Length == 0)
                                        {
                                            string[] ckvalidflag = new string[2];
                                            ckvalidflag[0] = "false";
                                            ckvalidflag[1] = vSourceFieldName + " is Mandatory";
                                            insertflag = false;
                                            vErrorDesc = vErrorDesc + ckvalidflag[1].ToString();
                                        }

                                        if (vDestinationFieldName == "dChallanDate")
                                        {
                                            DateTime date = DateTime.ParseExact(vFieldValue, "dd-MM-yyyy", null);
                                            excelrow[dtExcelData.Columns["ChallanDate"]] = date;

                                        }

                                        if (vDestinationFieldName == "dLoanDisbursementDate")
                                        {
                                            if (String.IsNullOrEmpty(vFieldValue))
                                            {
                                                excelrow[dtExcelData.Columns["LoanDisbursementDate"]] = "1/1/1900";
                                            }
                                        }

                                        if (vDestinationFieldName == "dLoanSanctionDate")
                                        {
                                            if (String.IsNullOrEmpty(vFieldValue))
                                            {
                                                excelrow[dtExcelData.Columns["LoanSanctionDate"]] = "1/1/1900";
                                            }
                                        }


                                        chkValidFlag = Fn_Check_Business_Validation(vDestinationFieldName, vFieldValue, vCertificateNo);

                                        if (chkValidFlag[0] == "false")
                                        {
                                            insertflag = false;

                                            vErrorDesc = vErrorDesc + chkValidFlag[1].ToString();
                                        }
                                    }

                                }
                                if (insertflag == false)
                                {

                                    excelrow["vTransType"] = "CPL";
                                    excelrow["vErrorFlag"] = "Y";
                                    excelrow["vErrorDesc"] = vErrorDesc;
                                }
                                else
                                {
                                    excelrow["vTransType"] = "CPL";
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

                        if (dtValidatedExcelData != null)
                        {
                            foreach (DataRow row in dtValidatedExcelData.Rows)
                            {
                                row["UploadId"] = vUploadId;
                            }
                        }
                        if (dtUploadErrorLog != null)
                        {
                            foreach (DataRow row in dtUploadErrorLog.Rows)
                            {
                                row["UploadId"] = vUploadId;
                            }
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
                                        //Set the database table name
                                        sqlBulkCopy.DestinationTableName = "dbo.TBL_KAVACH_POLICY_TABLE_ERROR_LOG";

                                        //[OPTIONAL]: Map the Excel columns with that of the database table
                                        //Getting Columns and Mapping from the Mapping Table

                                        //sqlCommand = "SELECT * FROM TBL_KAVACH_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N' and vSourceColumnName in ('UploadId','ProductName','ProductType','RMCode','Branch','CrnNo','AccountNo','CustomerName','CustomerGender','CustomerDob','ProposerAddLine1','ProposerAddLine2','ProposerAddLine3','ProposerCity','ProposerState','ProposerPinCode','UniqueIdentificationNo','MobileNo','EmailId','NomineeName','NomineeRelation','NomineeAge','UniqueAccDebitRefNo','AccountDebitDate','DebitAmount','IntermediaryName','IntermediaryCode','CorporateName','PolicyEndDate','PlanName','MasterPolicyNo','NetPremium','LoadingDiscount','TotalPolicyPremium','PolicyStartDate','ProposalDate','CertificateNo','EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus','ChallanNo','ChallanDate','PolicyTenure','LanNo','LoanTenure','LoanDisbursementDate','LoanSanctionDate','LoanOutAmount','CustomerType','Occupation','RelationWithInsured','Comments','ProductCode','IntermediaryContact','PolicyType','Covid-19 Diagnosis','Convalescence','ICU Daily Cash','In-patient Hospitalisation','Home Care Treatment Expenses','Ambulance','AYUSH Treatment','Pre Hospitalization Medical Expenses','Post Hospitalization Medical Expenses','Hospital Daily Cash','OPD','Room Rent Capping','Additional Cover 1','Additional Cover 2','Additional Cover 3','Additional Cover 4','Comments/Remarks','No of Lives','Condition 1','Condition 2','Condition 3','Condition 4','Condition 5','Condition 6','Condition 7')";
                                        sqlCommand = "SELECT * FROM TBL_KAVACH_COLUMN_MAPPING_MASTER where vSourceColumnName in ('EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus') OR (bExcludeForPolicyUpload='N' and vSourceColumnName in ('UploadId','ProductName','ProductType','RMCode','Branch','CrnNo','AccountNo','CustomerName','CustomerGender','CustomerDob','ProposerAddLine1','ProposerAddLine2','ProposerAddLine3','ProposerCity','ProposerState','ProposerPinCode','UniqueIdentificationNo','MobileNo','EmailId','NomineeName','NomineeRelation','NomineeAge','UniqueAccDebitRefNo','AccountDebitDate','DebitAmount','IntermediaryName','IntermediaryCode','CorporateName','PolicyEndDate','PlanName','MasterPolicyNo','NetPremium','LoadingDiscount','TotalPolicyPremium','PolicyStartDate','ProposalDate','CertificateNo','ChallanNo','ChallanDate','PolicyTenure','LanNo','LoanTenure','LoanDisbursementDate','LoanSanctionDate','LoanOutAmount','CustomerType','Occupation','RelationWithInsured','Comments','ProductCode','IntermediaryContact','PolicyType','Covid-19 Diagnosis','Convalescence','ICU Daily Cash','In-patient Hospitalisation','Home Care Treatment Expenses','Ambulance','AYUSH Treatment','Pre Hospitalization Medical Expenses','Post Hospitalization Medical Expenses','Hospital Daily Cash','OPD','Room Rent Capping','Additional Cover 1','Additional Cover 2','Additional Cover 3','Additional Cover 4','Comments/Remarks','No of Lives','Condition 1','Condition 2','Condition 3','Condition 4','Condition 5','Condition 6','Condition 7'))";

                                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        ds = null;
                                        ds = dbCOMMON.ExecuteDataSet(dbCommand);

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            sqlBulkCopy.ColumnMappings.Add("vTransType", "vTransType");
                                            sqlBulkCopy.ColumnMappings.Add("vErrorFlag", "vErrorFlag");
                                            sqlBulkCopy.ColumnMappings.Add("vErrorDesc", "vErrorDesc");

                                            foreach (DataRow row in ds.Tables[0].Rows)
                                            {
                                                sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                                            }
                                        }
                                        con.Open();
                                        sqlBulkCopy.WriteToServer(dtUploadErrorLog);
                                        con.Close();
                                    }
                                }
                            }
                        }

                        if (dtValidatedExcelData != null)
                        {
                            if (dtValidatedExcelData.Rows.Count > 0)
                            {
                                string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                                using (SqlConnection con = new SqlConnection(consString))
                                {

                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                    {
                                        //Set the database table name
                                        sqlBulkCopy.DestinationTableName = "dbo.TBL_KAVACH_POLICY_TABLE_TEMP";

                                        //[OPTIONAL]: Map the Excel columns with that of the database table
                                        //Getting Columns and Mapping from the Mapping Table

                                        //sqlCommand = "SELECT * FROM TBL_KAVACH_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N' and vSourceColumnName in ('UploadId','ProductName','ProductType','RMCode','Branch','CrnNo','AccountNo','CustomerName','CustomerGender','CustomerDob','ProposerAddLine1','ProposerAddLine2','ProposerAddLine3','ProposerCity','ProposerState','ProposerPinCode','UniqueIdentificationNo','MobileNo','EmailId','NomineeName','NomineeRelation','NomineeAge','UniqueAccDebitRefNo','AccountDebitDate','DebitAmount','IntermediaryName','IntermediaryCode','CorporateName','PolicyEndDate','PlanName','MasterPolicyNo','NetPremium','LoadingDiscount','TotalPolicyPremium','PolicyStartDate','ProposalDate','CertificateNo','EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus','ChallanNo','ChallanDate','PolicyTenure','LanNo','LoanTenure','LoanDisbursementDate','LoanSanctionDate','LoanOutAmount','CustomerType','Occupation','RelationWithInsured','Comments','ProductCode','IntermediaryContact','PolicyType','Covid-19 Diagnosis','Convalescence','ICU Daily Cash','In-patient Hospitalisation','Home Care Treatment Expenses','Ambulance','AYUSH Treatment','Pre Hospitalization Medical Expenses','Post Hospitalization Medical Expenses','Hospital Daily Cash','OPD','Room Rent Capping','Additional Cover 1','Additional Cover 2','Additional Cover 3','Additional Cover 4','Comments/Remarks','No of Lives','Condition 1','Condition 2','Condition 3','Condition 4','Condition 5','Condition 6','Condition 7')";
                                        sqlCommand = "SELECT * FROM TBL_KAVACH_COLUMN_MAPPING_MASTER where vSourceColumnName in ('EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus') OR (bExcludeForPolicyUpload='N' and vSourceColumnName in ('UploadId','ProductName','ProductType','RMCode','Branch','CrnNo','AccountNo','CustomerName','CustomerGender','CustomerDob','ProposerAddLine1','ProposerAddLine2','ProposerAddLine3','ProposerCity','ProposerState','ProposerPinCode','UniqueIdentificationNo','MobileNo','EmailId','NomineeName','NomineeRelation','NomineeAge','UniqueAccDebitRefNo','AccountDebitDate','DebitAmount','IntermediaryName','IntermediaryCode','CorporateName','PolicyEndDate','PlanName','MasterPolicyNo','NetPremium','LoadingDiscount','TotalPolicyPremium','PolicyStartDate','ProposalDate','CertificateNo','ChallanNo','ChallanDate','PolicyTenure','LanNo','LoanTenure','LoanDisbursementDate','LoanSanctionDate','LoanOutAmount','CustomerType','Occupation','RelationWithInsured','Comments','ProductCode','IntermediaryContact','PolicyType','Covid-19 Diagnosis','Convalescence','ICU Daily Cash','In-patient Hospitalisation','Home Care Treatment Expenses','Ambulance','AYUSH Treatment','Pre Hospitalization Medical Expenses','Post Hospitalization Medical Expenses','Hospital Daily Cash','OPD','Room Rent Capping','Additional Cover 1','Additional Cover 2','Additional Cover 3','Additional Cover 4','Comments/Remarks','No of Lives','Condition 1','Condition 2','Condition 3','Condition 4','Condition 5','Condition 6','Condition 7'))";

                                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        ds = null;
                                        ds = dbCOMMON.ExecuteDataSet(dbCommand);

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            sqlBulkCopy.ColumnMappings.Add("vTransType", "vTransType");
                                            sqlBulkCopy.ColumnMappings.Add("vErrorFlag", "vErrorFlag");
                                            sqlBulkCopy.ColumnMappings.Add("vErrorDesc", "vErrorDesc");

                                            foreach (DataRow row in ds.Tables[0].Rows)
                                            {
                                                sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                                            }
                                        }
                                        con.Open();
                                        sqlBulkCopy.WriteToServer(dtValidatedExcelData);
                                        con.Close();
                                    }
                                }

                                using (SqlConnection con = new SqlConnection(consString))
                                {
                                    #region History Data
                                    using (SqlCommand command = new SqlCommand("dbo.PROC_KAVACH_POLICY_CANCEL_UPLOAD", con))
                                    {
                                        // Updating destination table, and dropping temp table
                                        command.CommandTimeout = 300;
                                        con.Open();

                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@UploadID", vUploadId.Trim());
                                        command.Parameters.AddWithValue("@CancelledBy", Session["vUserLoginId"].ToString());
                                        command.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    else
                    {
                        sqlCommand = "Delete from TBL_KAVACH_POLICY_TABLE_TEMP where vuploadId ='" + vUploadId + "'";
                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                        dbCOMMON.ExecuteNonQuery(dbCommand);
                        _tran.Rollback();
                        Session["ErrorCallingPage"] = "FrmKAVACHCancelUpload.aspx";
                        string vStatusMsg = "No Policy Record for Upload";
                        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        return;
                    }
                }
                _tran.Commit();
                Session["ErrorCallingPage"] = "FrmKAVACHCancelUpload.aspx";
                string vStatusMsg1 = "Cancel Policy Uploaded with Upload Id  " + vUploadId;
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg1, false);
                return;
            }
            catch (Exception ex)
            {
                string sqlCommand = "Delete from TBL_KAVACH_POLICY_TABLE_TEMP where vuploadId ='" + vUploadId + "'";
                DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                dbCOMMON.ExecuteNonQuery(dbCommand);
                _tran.Rollback();
                Session["ErrorCallingPage"] = "FrmKAVACHCancelUpload.aspx";
                string vStatusMsg = "No Records for Upload or " + ex.Message.ToString().Replace("\r\n", "");
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
                //log the error
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");

            //Create an Template DataTable
            DataTable TemplateTable = new DataTable("Template");

            //string sqlCommand = "SELECT  * FROM TBL_KAVACH_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload ='N'";
            string sqlCommand = "SELECT  * FROM TBL_KAVACH_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload ='N' OR vSourceColumnName in ('EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus')";
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

            string _DownloadableProductFileName = "KAVACH_UPLOAD_TEMPLATE_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

            String strfilename = filePath + "\\" + _DownloadableProductFileName;

            if (System.IO.File.Exists(strfilename))
            {
                System.IO.File.Delete(strfilename);
            }

            if (ExportDataTableToExcel(TemplateTable, "HDC_UPLOAD_SHEET", strfilename) == true)
            {
                DownloadFile(strfilename);
            }
        }

        private bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        {
            //Creae an Excel application instance

            ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
            ExcelRange oRng;

            try
            {

                // Work sheet
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheet.Name = worksheetName;

                //excelSheet.Cells[1, 1] = "Detail";
                //excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

                // loop through each row and add values to our sheet
                int rowcount = 0;


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
                        if (rowcount > 1)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    oRng = (ExcelRange)excelSheet.Cells[rowcount, dataTable.Columns.Count];
                                    FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
                                }
                            }
                        }
                    }
                }

                // now we resize the columns
                oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
                oRng.AutoFitColumns();
                BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));

                oRng = (ExcelRange)excelSheet.Cells[1, dataTable.Columns.Count];
                FormattingExcelCells(oRng, "#000099", System.Drawing.Color.White, true);


                //now save the workbook and exit Excel

                excelApp.Save();
                return true;
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
                return false;
            }
            finally
            {
                excelApp = null;
            }
        }

        private void BorderAround(ExcelRange range, int colour)
        {
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.ColorTranslator.FromOle(colour));
        }
        public void FormattingExcelCells(ExcelRange range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.Indexed = 16;
            range.Style.Font.Size = 12;
            //range.Style.Font.Color = System.Drawing.Color.White;
            if (IsFontbool == true)
            {
                range.Style.Font.Bold = IsFontbool;
            }
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

        protected string[] Fn_Check_Business_Validation(string vFieldName, string vFieldValue, string vCertificateNo)
        {
            string[] ckvalidflag = new string[2];
            ckvalidflag[0] = "true";
            ckvalidflag[1] = " ";

            if (vFieldName == "vCertificateNo")
            {
                if (vFieldValue.Length < 5)
                {
                    ckvalidflag[0] = "false";
                    ckvalidflag[1] = "Certificate No Length is not valid";
                }
            }

            if (vFieldName == "vEndorsementType")
            {

                if (vFieldValue == string.Empty)
                {
                    ckvalidflag[0] = "false";
                    ckvalidflag[1] = "EndorsementType cannot be Empty";
                }



                if (vFieldValue.ToLower().Contains("canc"))
                {
                    int ret = 0;
                    string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(consString))
                    {
                        using (SqlCommand command = new SqlCommand("", con))
                        {
                            // Updating destination table, and dropping temp table
                            command.CommandTimeout = 300;
                            con.Open();
                            command.CommandText = "SELECT COUNT(1) FROM TBL_KAVACH_POLICY_TABLE WHERE vCertificateNo = '" + vCertificateNo + "' and vEndorsementType like '%canc%'";
                            ret = (Int32)command.ExecuteScalar();
                            con.Close();
                        }
                    }

                    if (ret > 0)
                    {
                        ckvalidflag[0] = "false";
                        ckvalidflag[1] = "Policy Already Cancelled";
                    }
                }

                if (vFieldValue != string.Empty && !vFieldValue.ToLower().Contains("canc"))
                {
                    ckvalidflag[0] = "false";
                    ckvalidflag[1] = "EndorsementType cannot be other than Cancel";
                }
            }

            return ckvalidflag;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}