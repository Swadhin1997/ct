using Microsoft.Practices.EnterpriseLibrary.Data;
using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmGPADispatchUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
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
                            command.CommandText = "SELECT COUNT(1) FROM TBL_GPA_POLICY_TABLE WHERE vCertificateNo = '" + vCertificateNo + "' and vEndorsementType like '%canc%'";
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

        protected void Upload(object sender, EventArgs e)
        {
            string allowedExtensions = ".xlsx";

            if (FileUpload1.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

                if (fileExtension != allowedExtensions)
                {
                    Session["ErrorCallingPage"] = "FrmGPADispatchUpload.aspx";
                    string vStatusMsg = "Invalid file Extension, Only XLSX files are allowed to be Uploaded";
                    Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                    return;
                }

            }
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

            vUploadId = wsDocNo.fn_Gen_Doc_Master_No("DUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);
            // _con.Close();

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

                    // string sqlCommand = "SELECT  * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N'";

                    //string sqlCommand = "SELECT * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N' and vSourceColumnName in ('UploadId','ProductName','BusinesssType','RMCode','BranchName','BranchCode','CrnNo','AccountNo','CustomerName','CustomerGender','CustomerDob','ProposerAddLine1','ProposerAddLine2','ProposerAddLine3','ProposerCity','ProposerState','ProposerPinCode','UniqueIdentificationNo','MobileNo','EmailId','NomineeName','NomineeRelation','NomineeAge','NomineeGuardian','NomineeRelWithGuardian','UniqueAccDebitRefNo','AccountDebitDate','DebitAmount','IntermediaryName','IntermediaryCode','CorporateName','PolicyEndDate','PlanName','MasterPolicyNo','BankName','NetPremium','LoadingDiscount','ServiceTax','SwachBharatTax','KKC','TotalPolicyPremium','PolicyStartDate','ProposalDate','CertificateNo','AccidentalDeathSI','PermTotalDiabilitySI','PermPartialDiabilitySI','TempTotalDiabilitySI','AmbulanceCoverSI','ModificationAllowanceSI','CostOfSupportItemsSI','OutPatientTreatCoverSI','ChildEduGrantSI','MarriageBenefitChildSI','DisappearBenefitSI','CompassVisitSI','SportsActCoverSI','CarriageOfDeadBodySI','FuneralExpenseSI','DoubleInsuranceBenifitSI','HospitalCashSI','ConvalescenceSI','BurnBenefitSI','BrokenBoneBenefitSI','ComaBenefitSI','InPatientHosSI','DomTravelForMedTreatSI','LossOfJobSI','EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus','ChallanNo','ChallanDate','BankBranch','PolicyTenure','LanNo','LoanTenure','LoanDisbursementDate','LoanSanctionDate','LoanOutAmount','CustomerType','Occupation','RelationWithInsured','Comments','AddCol1','AddCol2','AddCol3','ProductCode','IntermediaryContactDetail','PreviousPolicyNumber','AppointeeName','PolicyType')";

                    string sqlCommand = "SELECT * FROM GPA_Certificates_Dispatch_mapping_master";

                    DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);

                    DataSet ds = null;

                    ds = dbCOMMON.ExecuteDataSet(dbCommand);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GetMappingData = true;
                    }

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    //using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "A:HI]", excel_con))
                    {

                        oda.Fill(dtExcelData);

                    }


                    excel_con.Close();

                    if (dtExcelData.Rows.Count > 0)
                    {

                        //Business Validation Commented on 15-Dec-2015

                        foreach (DataRow excelrow in dtExcelData.Rows)
                        {

                            //string vDestinationFieldName = "";
                            //string vSourceFieldName = "";
                            //string vFieldValue = "";
                            //string vErrorDesc = "";
                            //string bMandatoryForPolicy = "";
                            //string vCertificateNo = "";
                            //string[] chkValidFlag;

                            if (GetMappingData == true)
                            {
                              //  bool insertflag = true;

                               // for (int i = 1; i <= dtExcelData.Columns.Count; i++)
                                //{
                                //    string trimString = dtExcelData.Columns[i - 1].ColumnName.ToString().TrimStart('v');

                                //    //string searchExpression = "vSourceColumnName = '" + dtExcelData.Columns[i - 1].ColumnName.ToString().Trim() + "'";

                                //    string searchExpression = "vSourceColumnName = '" + trimString.Trim() + "'";

                                //    DataRow[] foundRows = ds.Tables[0].Select(searchExpression);

                                //    if (foundRows.Count() > 0)
                                //    {
                                //        vDestinationFieldName = foundRows[0]["vDestinationColumnName"].ToString();
                                //        vSourceFieldName = foundRows[0]["vSourceColumnName"].ToString();
                                //        vFieldValue = excelrow[dtExcelData.Columns[i - 1].ColumnName.ToString().Trim()].ToString();
                                //        bMandatoryForPolicy = foundRows[0]["bMandatoryForPolicy"].ToString();

                                //        if (vSourceFieldName != "vCertificateNo")
                                //        {
                                //            vCertificateNo = excelrow[dtExcelData.Columns["CertificateNo"].ToString().Trim()].ToString();
                                //            //vCertificateNo = excelrow[dtExcelData.Columns["vCertificateNo"].ToString().Trim()].ToString();
                                //        }

                                //        if (bMandatoryForPolicy == "Y" && vFieldValue.Trim().Length == 0)
                                //        {
                                //            string[] ckvalidflag = new string[2];
                                //            ckvalidflag[0] = "false";
                                //            ckvalidflag[1] = vSourceFieldName + " is Mandatory";
                                //            insertflag = false;
                                //            vErrorDesc = vErrorDesc + ckvalidflag[1].ToString();
                                //        }

                                //        if (vDestinationFieldName == "dChallanDate")
                                //        {
                                //            DateTime date = DateTime.ParseExact(vFieldValue, "dd-MM-yyyy", null);
                                //            excelrow[dtExcelData.Columns["ChallanDate"]] = date;

                                //        }

                                //        if (vDestinationFieldName == "dLoanDisbursementDate")
                                //        {
                                //            if (String.IsNullOrEmpty(vFieldValue))
                                //            {
                                //                excelrow[dtExcelData.Columns["LoanDisbursementDate"]] = "1/1/1900";
                                //            }
                                //        }

                                //        if (vDestinationFieldName == "dLoanSanctionDate")
                                //        {
                                //            if (String.IsNullOrEmpty(vFieldValue))
                                //            {
                                //                excelrow[dtExcelData.Columns["LoanSanctionDate"]] = "1/1/1900";
                                //            }
                                //        }


                                //        chkValidFlag = Fn_Check_Business_Validation(vDestinationFieldName, vFieldValue, vCertificateNo);

                                //        if (chkValidFlag[0] == "false")
                                //        {
                                //            insertflag = false;

                                //            vErrorDesc = vErrorDesc + chkValidFlag[1].ToString();
                                //        }
                                //    }

                                //}
                                //if (insertflag == false)
                                //{

                                //    excelrow["vTransType"] = "CPL";
                                //    excelrow["vErrorFlag"] = "Y";
                                //    excelrow["vErrorDesc"] = vErrorDesc;
                                //}
                                //else
                                //{
                                //    excelrow["vTransType"] = "CPL";
                                //    excelrow["vErrorFlag"] = "N";
                                //    excelrow["vErrorDesc"] = "";
                                //}
                            }
                        }

                        DataTable dtValidatedExcelData = null;
                        // DataTable dtUploadErrorLog = null;

                        dtValidatedExcelData = dtExcelData.Copy();

                        //string searchExpressionPass = "vErrorFlag = 'N'";
                        //DataRow[] foundRows1 = dtExcelData.Select(searchExpressionPass);

                        
                        //if (foundRows1.Count() > 0)
                        //{
                        //    dtValidatedExcelData = dtExcelData.Select(searchExpressionPass).CopyToDataTable();
                        //}
                        //string searchExpressionFail = "vErrorFlag = 'Y'";
                        //DataRow[] foundRows2 = dtExcelData.Select(searchExpressionFail);
                        //if (foundRows2.Count() > 0)
                        //{
                        //    dtUploadErrorLog = dtExcelData.Select(searchExpressionFail).CopyToDataTable();
                        //}

                        if (dtValidatedExcelData != null)
                        {
                            foreach (DataRow row in dtValidatedExcelData.Rows)
                            {
                                row["uploadId"] = vUploadId;
                            }
                        }
                        //if (dtUploadErrorLog != null)
                        //{
                        //    foreach (DataRow row in dtUploadErrorLog.Rows)
                        //    {
                        //        row["UploadId"] = vUploadId;
                        //    }
                        //}



                       // if (dtUploadErrorLog != null)
                        //{
                        //    if (dtUploadErrorLog.Rows.Count > 0)
                        //    {
                        //        string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                        //        using (SqlConnection con = new SqlConnection(consString))
                        //        {

                        //            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        //            {

                        //                //Set the database table name

                        //                sqlBulkCopy.DestinationTableName = "dbo.TBL_GPA_POLICY_TABLE_ERROR_LOG";

                        //                //[OPTIONAL]: Map the Excel columns with that of the database table


                        //                //Getting Columns and Mapping from the Mapping Table

                        //                //  sqlCommand = "SELECT  * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N'";
                        //                //sqlCommand = "SELECT * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N' and vSourceColumnName in ('UploadId','ProductName','BusinesssType','RMCode','BranchName','BranchCode','CrnNo','AccountNo','CustomerName','CustomerGender','CustomerDob','ProposerAddLine1','ProposerAddLine2','ProposerAddLine3','ProposerCity','ProposerState','ProposerPinCode','UniqueIdentificationNo','MobileNo','EmailId','NomineeName','NomineeRelation','NomineeAge','NomineeGuardian','NomineeRelWithGuardian','UniqueAccDebitRefNo','AccountDebitDate','DebitAmount','IntermediaryName','IntermediaryCode','CorporateName','PolicyEndDate','PlanName','MasterPolicyNo','BankName','NetPremium','LoadingDiscount','ServiceTax','SwachBharatTax','KKC','TotalPolicyPremium','PolicyStartDate','ProposalDate','CertificateNo','AccidentalDeathSI','PermTotalDiabilitySI','PermPartialDiabilitySI','TempTotalDiabilitySI','AmbulanceCoverSI','ModificationAllowanceSI','CostOfSupportItemsSI','OutPatientTreatCoverSI','ChildEduGrantSI','MarriageBenefitChildSI','DisappearBenefitSI','CompassVisitSI','SportsActCoverSI','CarriageOfDeadBodySI','FuneralExpenseSI','DoubleInsuranceBenifitSI','HospitalCashSI','ConvalescenceSI','BurnBenefitSI','BrokenBoneBenefitSI','ComaBenefitSI','InPatientHosSI','DomTravelForMedTreatSI','LossOfJobSI','EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus','ChallanNo','ChallanDate','BankBranch','PolicyTenure','LanNo','LoanTenure','LoanDisbursementDate','LoanSanctionDate','LoanOutAmount','CustomerType','Occupation','RelationWithInsured','Comments','AddCol1','AddCol2','AddCol3','ProductCode','IntermediaryContactDetail','PreviousPolicyNumber','AppointeeName','PolicyType')";

                        //                sqlCommand = "SELECT * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N' and vSourceColumnName in ('UploadId','ProductName','BusinesssType','RMCode','BranchName','BranchCode','CrnNo','AccountNo','CustomerName','CustomerGender','CustomerDob','ProposerAddLine1','ProposerAddLine2','ProposerAddLine3','ProposerCity','ProposerState','ProposerPinCode','UniqueIdentificationNo','MobileNo','EmailId','NomineeName','NomineeRelation','NomineeAge','NomineeGuardian','NomineeRelWithGuardian','UniqueAccDebitRefNo','AccountDebitDate','DebitAmount','IntermediaryName','IntermediaryCode','CorporateName','PolicyEndDate','PlanName','MasterPolicyNo','BankName','NetPremium','LoadingDiscount','TotalPolicyPremium','PolicyStartDate','ProposalDate','CertificateNo','AccidentalDeathSI','PermTotalDiabilitySI','PermPartialDiabilitySI','TempTotalDiabilitySI','AmbulanceCoverSI','ModificationAllowanceSI','CostOfSupportItemsSI','OutPatientTreatCoverSI','ChildEduGrantSI','MarriageBenefitChildSI','DisappearBenefitSI','CompassVisitSI','SportsActCoverSI','CarriageOfDeadBodySI','FuneralExpenseSI','DoubleInsuranceBenifitSI','HospitalCashSI','ConvalescenceSI','BurnBenefitSI','BrokenBoneBenefitSI','ComaBenefitSI','InPatientHosSI','DomTravelForMedTreatSI','LossOfJobSI','EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus','ChallanNo','ChallanDate','BankBranch','PolicyTenure','LanNo','LoanTenure','LoanDisbursementDate','LoanSanctionDate','LoanOutAmount','CustomerType','Occupation','RelationWithInsured','Comments','AddCol1','AddCol2','AddCol3','ProductCode','IntermediaryContactDetail','PreviousPolicyNumber','AppointeeName','PolicyType')";

                        //                dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                        //                ds = null;
                        //                ds = dbCOMMON.ExecuteDataSet(dbCommand);

                        //                if (ds.Tables[0].Rows.Count > 0)
                        //                {
                        //                    sqlBulkCopy.ColumnMappings.Add("vTransType", "vTransType");
                        //                    sqlBulkCopy.ColumnMappings.Add("vErrorFlag", "vErrorFlag");
                        //                    sqlBulkCopy.ColumnMappings.Add("vErrorDesc", "vErrorDesc");

                        //                    foreach (DataRow row in ds.Tables[0].Rows)
                        //                    {
                        //                        sqlBulkCopy.ColumnMappings.Add(row["vSourceColumnName"].ToString(), row["vDestinationColumnName"].ToString());
                        //                    }
                        //                }
                        //                con.Open();
                        //                sqlBulkCopy.WriteToServer(dtUploadErrorLog);
                        //                con.Close();
                        //            }
                        //        }
                        //    }
                        //}

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

                                        sqlBulkCopy.DestinationTableName = "dbo.GPA_Certificates_Dispatch";

                                        //[OPTIONAL]: Map the Excel columns with that of the database table


                                        //Getting Columns and Mapping from the Mapping Table

                                        //  sqlCommand = "SELECT  * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N'";

                                        //sqlCommand = "SELECT * FROM TBL_GPA_COLUMN_MAPPING_MASTER where bExcludeForPolicyUpload='N' and vSourceColumnName in ('UploadId','ProductName','BusinesssType','RMCode','BranchName','BranchCode','CrnNo','AccountNo','CustomerName','CustomerGender','CustomerDob','ProposerAddLine1','ProposerAddLine2','ProposerAddLine3','ProposerCity','ProposerState','ProposerPinCode','UniqueIdentificationNo','MobileNo','EmailId','NomineeName','NomineeRelation','NomineeAge','NomineeGuardian','NomineeRelWithGuardian','UniqueAccDebitRefNo','AccountDebitDate','DebitAmount','IntermediaryName','IntermediaryCode','CorporateName','PolicyEndDate','PlanName','MasterPolicyNo','BankName','NetPremium','LoadingDiscount','ServiceTax','SwachBharatTax','KKC','TotalPolicyPremium','PolicyStartDate','ProposalDate','CertificateNo','AccidentalDeathSI','PermTotalDiabilitySI','PermPartialDiabilitySI','TempTotalDiabilitySI','AmbulanceCoverSI','ModificationAllowanceSI','CostOfSupportItemsSI','OutPatientTreatCoverSI','ChildEduGrantSI','MarriageBenefitChildSI','DisappearBenefitSI','CompassVisitSI','SportsActCoverSI','CarriageOfDeadBodySI','FuneralExpenseSI','DoubleInsuranceBenifitSI','HospitalCashSI','ConvalescenceSI','BurnBenefitSI','BrokenBoneBenefitSI','ComaBenefitSI','InPatientHosSI','DomTravelForMedTreatSI','LossOfJobSI','EndorsementType','EndorsementReason','EndorsementDesc','EndorsementEffectiveDate','EndorsementIssueDate','EndorsementStatus','ChallanNo','ChallanDate','BankBranch','PolicyTenure','LanNo','LoanTenure','LoanDisbursementDate','LoanSanctionDate','LoanOutAmount','CustomerType','Occupation','RelationWithInsured','Comments','AddCol1','AddCol2','AddCol3','ProductCode','IntermediaryContactDetail','PreviousPolicyNumber','AppointeeName','PolicyType')";

                                        sqlCommand = "SELECT * FROM GPA_Certificates_Dispatch_mapping_master";

                                        dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                                        ds = null;
                                        ds = dbCOMMON.ExecuteDataSet(dbCommand);

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            //sqlBulkCopy.ColumnMappings.Add("vTransType", "vTransType");
                                            //sqlBulkCopy.ColumnMappings.Add("vErrorFlag", "vErrorFlag");
                                            //sqlBulkCopy.ColumnMappings.Add("vErrorDesc", "vErrorDesc");

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


                             //   using (SqlConnection con = new SqlConnection(consString))
//                                {
//                                    using (SqlCommand command = new SqlCommand("", con))
//                                    {
//                                        // Updating destination table, and dropping temp table
//                                        command.CommandTimeout = 300;
//                                        con.Open();

//                                        //command.CommandText = "INSERT INTO TBL_GPA_POLICY_TABLE_HISTORY SELECT * FROM TBL_GPA_POLICY_TABLE where vCertificateNo in (SELECT vCertificateNo from TBL_GPA_POLICY_TABLE_TEMP where vUploadId ='" + vUploadId + "')";

//                                        command.CommandText = "INSERT INTO TBL_GPA_POLICY_TABLE_HISTORY SELECT " +
//                                            "[vUploadId],[vProductName],[vBusinesssType],[vRMCode],[vBranchName],[vBranchCode],[vCrnNo],[vAccountNo],[vCustomerName]" +
//      ",[vCustomerGender],[dCustomerDob],[vProposerAddLine1],[vProposerAddLine2],[vProposerAddLine3],[vProposerCity],[vProposerState],[vProposerPinCode],[vUniqueIdentificationNo]" +
//            ",[vMobileNo]" +
//      ",[vEmailId]" +
//      ",[vNomineeName]" +
//      ",[vNomineeRelation]" +
//      ",[vNomineeAge]" +
//      ",[vNomineeGuardian]" +
//      ",[vNomineeRelWithGuardian]" +
//      ",[vUniqueAccDebitRefNo]" +
//      ",[dAccountDebitDate]" +
//      ",[nDebitAmount]" +
//      ",[vIntermediaryCode]" +
//      ",[vIntermediaryName]" +
//      ",[vCorporateID]" +
//      ",[vCorporateName]" +
//      ",[dPolicyEndDate]" +
//      ",[vPlanName]" +
//      ",[vMasterPolicyNo]" +
//      ",[vBankName]" +
//      ",[nNetPremium]" +
//      ",[nLoadingDiscount]" +
//      ",[nServiceTax]" +
//      ",[nSwachBharatTax]" +
//      ",[nKKC]" +
//      ",[nTotalPolicyPremium]" +
//      ",[dPolicyStartDate]" +
//      ",[dProposalDate]" +
//      ",[vCertificateNo]" +
//      ",[vAccidentalDeath]" +
//      ",[nAccidentalDeathSI]" +
//      ",[vPermTotalDiability]" +
//      ",[nPermTotalDiabilitySI]" +
//      ",[vPermPartialDiability]" +
//      ",[nPermPartialDiabilitySI]" +
//      ",[vTempTotalDiability]" +
//      ",[nTempTotalDiabilitySI]" +
//      ",[vAmbulanceCover]" +
//      ",[nAmbulanceCoverSI]" +
//      ",[vModificationAllowance]" +
//      ",[nModificationAllowanceSI]" +
//      ",[vCostOfSupportItems]" +
//      ",[nCostOfSupportItemsSI]" +
//      ",[vOutPatientTreatCover]" +
//      ",[nOutPatientTreatCoverSI]" +
//      ",[vChildEduGrant]" +
//      ",[nChildEduGrantSI]" +
//      ",[vMarriageBenefitChild]" +
//      ",[nMarriageBenefitChildSI]" +
//      ",[vDisappearBenefit]" +
//      ",[nDisappearBenefitSI]" +
//      ",[vCompassVisit]" +
//      ",[nCompassVisitSI]" +
//      ",[vSportsActCover]" +
//      ",[nSportsActCoverSI]" +
//      ",[vCarriageOfDeadBody]" +
//      ",[nCarriageOfDeadBodySI]" +
//      ",[vFuneralExpense]" +
//      ",[nFuneralExpenseSI]" +
//      ",[vDoubleInsuranceBenifit]" +
//      ",[nDoubleInsuranceBenifitSI]" +
//      ",[vHospitalCash]" +
//      ",[nHospitalCashSI]" +
//      ",[vConvalescence]" +
//      ",[nConvalescenceSI]" +
//      ",[vBurnBenefit]" +
//      ",[nBurnBenefitSI]" +
//      ",[vBrokenBoneBenefit]" +
//      ",[nBrokenBoneBenefitSI]" +
//      ",[vComaBenefit]" +
//      ",[nComaBenefitSI]" +
//      ",[vInPatientHos]" +
//      ",[nInPatientHosSI]" +
//      ",[vDomTravelForMedTreat]" +
//      ",[nDomTravelForMedTreatSI]" +
//      ",[vLossOfJob]" +
//      ",[nLossOfJobSI]" +
//      ",[vEndorsementType]" +
//      ",[vEndorsementReason]" +
//      ",[vEndorsementDesc]" +
//      ",[dEndorsementEffectiveDate]" +
//      ",[dEndorsementIssueDate]" +
//      ",[bEndorsementStatus]" +
//      ",[vAdditional_column_1]" +
//      ",[vAdditional_column_2]" +
//      ",[vAdditional_column_3]" +
//      ",[vAdditional_column_4]" +
//      ",[vAdditional_column_5]" +
//      ",[vAdditional_column_6]" +
//      ",[vClaimNo]" +
//      ",[vPaymentTo]" +
//      ",[vInstrumentNo]" +
//      ",[dClaimIntimationDate]" +
//      ",[dClaimPaidRepudClosedDate]" +
//      ",[nClaimAmount]" +
//      ",[bClaimStatus]" +
//      ",[vCreatedBy]" +
//      ",[vModifiedBy] ='" + Session["vUserLoginId"] + "'" +
//      ",[dCreatedDate]" +
//      ",[dModifiedDate] = GETDATE()" +
//      ",[dDat_End_Date]" +
//      ",[vErrorFlag]" +
//      ",[vErrorDesc]" +
//      ",[vTransType]" +
//      ",[vClaimsUploadId]" +
//      ",[vClaimRemarks]" +
//      ",[vChallanNumber]" +
//      ",[dChallanDate]" +
//      ",[nStampDuty]" +
//      ",[nSectionAPrem]" +
//      ",[nExtToSectionAPrem]" +
//      ",[nSectionBPrem]" +
//      ",[nPlanSI]" +
//      ",[vAccidentalDeathSIText]" +
//      ",[vPermTotalDiabilitySIText]" +
//      ",[vPermPartialDiabilitySIText]" +
//      ",[vTempTotalDiabilitySIText]" +
//      ",[vAmbulanceCoverSIText]" +
//      ",[vModificationAllowanceSIText]" +
//      ",[vCostOfSupportItemsSIText]" +
//      ",[vOutPatientTreatCoverSIText]" +
//      ",[vChildEduGrantSIText]" +
//      ",[vMarriageBenefitChildSIText]" +
//      ",[vDisappearBenefitSIText]" +
//      ",[vCompassVisitSIText]" +
//      ",[vSportsActCoverSIText]" +
//      ",[vCarriageOfDeadBodySIText]" +
//      ",[vFuneralExpenseSIText]" +
//      ",[vDoubleInsuranceBenifitSIText]" +
//      ",[vHospitalCashSIText]" +
//      ",[vConvalescenceSIText]" +
//      ",[vBurnBenefitSIText]" +
//      ",[vBrokenBoneBenefitSIText]" +
//      ",[vComaBenefitSIText]" +
//      ",[vInPatientHosSIText]" +
//      ",[vDomTravelForMedTreatSIText]" +
//      ",[vLossOfJobSIText]" +
//      ",[nIsMailSent]" +
//      //
//      ",[vBankBranch]" +
//      ",[vPolicyTenure]" +
//      ",[vLanNo]" +
//      ",[vLoanTenure]" +
//      ",[dLoanDisbursementDate]" +
//      ",[dLoanSanctionDate]" +
//      ",[vLoanOutAmount]" +
//      ",[vCustomerType]" +
//      ",[vOccupation]" +
//      ",[vRelationWithInsured]" +
//",[vComments]" +
//",[vAddCol1]" +
//",[vAddCol2]" +
//",[vAddCol3]" +
//",[vProductCode]" +
//",[vSIBasis]" +
//",[vFinancerName]" +
//",[vMasterPolicyDate]" +
//",[vMasterPolicyLoc]" +
//",[vLoanType]" +
//",[vProposalType]" +
//",[vPlanCode]" +
//",[vAccidentalDeathADSIText]" +
//",[vPermTotalDisablePTDSIText]" +
//",[vPermPartialDisablePTDSIText]" +
//",[vTempTotalDisableTTDSIText]" +
//",[vAccidentalMedicalExpSIText]" +
//",[vPurchaseofBloodSIText]" +
//",[vTransportationofImpMedicineSIText]" +
//",[vDisappearanceBenefitSIText]" +
//",[vModificationofvehicleSIText]" +
//",[vCommonCarrierSIText]" +
//",[vMarraigeExpensesSIText]" +
//",[vSportsActivityCoverSIText]" +
//",[vWidowhoodCoverSIText]" +
//",[vDailyCashBenefitSIText]" +
//",[vAccidentalHospitalizationSIText]" +
//",[vOPDTreatmentSIText]" +
//",[vAccidentalDentalExpenseSIText]" +
//",[vConvalescenceBenefitSIText]" +
//",[vBurnsSIText]" +
//",[vBrokenBonesSIText]" +
//",[vComaSIText]" +
//",[vLossOfEmploymentSIText]" +
//",[vOnDutyCoverSIText]" +
//",[vLegalExpensesSIText]" +
//",[vAccidentalDeathAD]" +
//",[vPermTotalDisablePTD]" +
//",[vPermPartialDisablePTD]" +
//",[vTempTotalDisableTTD]" +
//",[vAccidentalMedicalExp]" +
//",[vPurchaseofBlood]" +
//",[vTransportationofImpMedicine]" +
//",[vDisappearanceBenefit]" +
//",[vModificationofvehicle]" +
//",[vCommonCarrier]" +
//",[vMarraigeExpenses]" +
//",[vSportsActivityCover]" +
//",[vWidowhoodCover]" +
//",[vDailyCashBenefit]" +
//",[vAccidentalHospitalization]" +
//",[vOPDTreatment]" +
//",[vAccidentalDentalExpense]" +
//",[vConvalescenceBenefit]" +
//",[vBurns]" +
//",[vBrokenBones]" +
//",[vComa]" +
//",[vLossOfEmployment]" +
//",[vOnDutyCover]" +
//",[vLegalExpenses]" +
//",[vCarraigeDeadBody]" +
//",[vCarraigeDeadBodySIText]" +
//",[vFuneralExpenses]" +
//",[vFuneralExpensesSIText]" +
//",[vCompassionatevisit]" +
//",[vCompassionatevisitSIText]" +
//",[vCostSupportItems]" +
//",[vCostSupportItemsSIText]" +
//",[vDomesticTravelForTreatment]" +
//",[vDomesticTravelForTreatmentSIText]" +

//",[nAccidentalDeathADSI]" +
//",[nPermTotalDisablePTDSI]" +
//",[nPermPartialDisablePTDSI]" +
//",[nTempTotalDisableTTDSI]" +
//",[nAccidentalMedicalExpSI]" +
//",[nPurchaseofBloodSI]" +
//",[nTransportationofImpMedicineSI]" +
//",[nDisappearanceBenefitSI]" +
//",[nModificationofvehicleSI]" +
//",[nCommonCarrierSI]" +
//",[nMarraigeExpensesSI]" +
//",[nSportsActivityCoverSI]" +
//",[nWidowhoodCoverSI]" +
//",[nDailyCashBenefitSI]" +
//",[nAccidentalHospitalizationSI]" +
//",[nOPDTreatmentSI]" +
//",[nAccidentalDentalExpenseSI]" +
//",[nConvalescenceBenefitSI]" +
//",[nBurnsSI]" +
//",[nBrokenBonesSI]" +
//",[nComaSI]" +
//",[nLossOfEmploymentSI]" +
//",[nOnDutyCoverSI]" +
//",[nLegalExpensesSI]" +
//",[nCarraigeDeadBodySI]" +
//",[nFuneralExpensesSI]" +
//",[nCompassionatevisitSI]" +
//",[nCostSupportItemsSI]" +
//",[nDomesticTravelForTreatmentSI]" +
//",[vPrevPolicyNumber]" +
//",[vAppointeeName]" +
//",[vPolicyType]" +
//",[vIntermediaryNumber]" +

//",[ugstPercentage]" +
//",[ugstAmount]" +
//",[cgstPercentage]" +
//",[cgstAmount]" +
//",[sgstPercentage]" +
//",[sgstAmount]" +
//",[igstPercentage]" +
//",[igstAmount]" +
//",[totalGSTAmount]" +
//",[gstCustState]" +
//",[gstIntermediaryState]" +



//        " FROM TBL_GPA_POLICY_TABLE where vCertificateNo in (SELECT vCertificateNo from TBL_GPA_POLICY_TABLE_TEMP where vUploadId ='" + vUploadId + "')";
//                                        command.ExecuteNonQuery();



//                                        //  command.CommandText = "UPDATE TBL_GPA_POLICY_TABLE_HISTORY SET dModifiedDate = GETDATE(), vModifiedBy='" + Session["vUserLoginId"] + "' WHERE vCertificateNo in (SELECT vCertificateNo from TBL_GPA_POLICY_TABLE_TEMP where vUploadId ='" + vUploadId + "')  ";
//                                        // command.ExecuteNonQuery();

//                                        command.CommandText = "UPDATE T SET T.vEndorsementType = TEMP.vEndorsementType,T.vEndorsementReason = TEMP.vEndorsementReason,T.vEndorsementDesc = TEMP.vEndorsementDesc," +
//                                         "T.dEndorsementEffectiveDate = TEMP.dEndorsementEffectiveDate,T.dEndorsementIssueDate = TEMP.dEndorsementIssueDate, T.bEndorsementStatus = 'Y'," +
//                                         "T.dModifiedDate = GETDATE(),T.vModifiedBy='" + Session["vUserLoginId"] + "', T.vUploadId = TEMP.vUploadId" +
//                                         " FROM TBL_GPA_POLICY_TABLE T INNER JOIN TBL_GPA_POLICY_TABLE_TEMP Temp ON T.vCertificateNo = TEMP.vCertificateNo";
//                                        command.ExecuteNonQuery();

//                                        command.CommandText = "Delete from TBL_GPA_POLICY_TABLE_TEMP where vuploadId ='" + vUploadId + "'";
//                                        command.ExecuteNonQuery();
//                                        con.Close();
//                                    }
//                                }
                            }
                        }
                    }
                    else
                    {
                      //  sqlCommand = "Delete from TBL_GPA_POLICY_TABLE_TEMP where vuploadId ='" + vUploadId + "'";
                      //  dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
                      //  dbCOMMON.ExecuteNonQuery(dbCommand);
                        _tran.Rollback();
                        Session["ErrorCallingPage"] = "FrmGPADispatchUpload.aspx";
                        string vStatusMsg = "No Policy Record for Upload";
                        Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                        return;
                    }
                }
                _tran.Commit();
                Session["ErrorCallingPage"] = "FrmGPADispatchUpload.aspx";
                string vStatusMsg1 = "Dispatch Policy Uploaded with Upload Id  " + vUploadId;
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg1, false);
                return;
            }
            catch (Exception ex)
            {
             //   string sqlCommand = "Delete from TBL_GPA_POLICY_TABLE_TEMP where vuploadId ='" + vUploadId + "'";
              //  DbCommand dbCommand = dbCOMMON.GetSqlStringCommand(sqlCommand);
             //   dbCOMMON.ExecuteNonQuery(dbCommand);
                _tran.Rollback();
                Session["ErrorCallingPage"] = "FrmGPADispatchUpload.aspx";
                string vStatusMsg = "No Records for Upload or " + ex.Message.ToString().Replace("\r\n", "");
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + vStatusMsg, false);
                return;
                //log the error
            }
        }

    
    }
}