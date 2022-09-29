using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text;

namespace ProjectPASS
{
    public partial class FrmGPAPolicyUploadStatus : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

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

        protected void btnGetPolicy_Click(object sender, EventArgs e)
        {
            string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

            using (SqlConnection sqlCon = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand("PROC_GET_DATA_POLICY_UPLOAD_STATUS",sqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string strvPolicyId = "";
                    string strvUploadId = "";

                    if (txtPolicyId.Text.Trim() != "")
                    {
                        //strvPolicyId = " and vCertificateNo ='" + txtPolicyId.Text.Trim() + "'";
                        cmd.Parameters.AddWithValue("@vPolicyId" , txtPolicyId.Text.Trim());
                    }
                    if (txtUploadId.Text.Trim() != "")
                    {
                        //strvUploadId = " and vUploadId='" + txtUploadId.Text.Trim() + "'";
                        cmd.Parameters.AddWithValue("@vUploadId" , txtUploadId.Text.Trim());
                    }
                    if (txtFromDate.Text == "" && txtUploadId.Text.Trim() != " ")
                    {
                        Alert.Show("Please Select From Date.");
                        return;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@vFromDate" , txtFromDate.Text.Trim());
                    }
                    if (txtToDate.Text == "" && txtUploadId.Text.Trim() != " ")
                    {
                        Alert.Show("Please Select To Date.");
                        return;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@vToDate" , txtToDate.Text.Trim());
                    }
                    string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString();
                    string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();



                    //                    string columnS = "vUploadId,vProductName,vBusinesssType,vRMCode,vBranchName,vBranchCode,vCrnNo,vAccountNo,vCustomerName,vCustomerGender,dCustomerDob,vProposerAddLine1,vProposerAddLine2,vProposerAddLine3,vProposerCity,vProposerState,vProposerPinCode,vUniqueIdentificationNo,vMobileNo,vEmailId,vNomineeName,vNomineeRelation"
                    //+ ", vNomineeAge, vNomineeGuardian, vNomineeRelWithGuardian, vUniqueAccDebitRefNo, dAccountDebitDate, nDebitAmount, vIntermediaryCode, vIntermediaryName, vCorporateID, vCorporateName, dPolicyEndDate, vPlanName, vMasterPolicyNo, vBankName"
                    //+ ", nNetPremium, nLoadingDiscount, nServiceTax, nSwachBharatTax, nKKC, nTotalPolicyPremium, dPolicyStartDate, dProposalDate, vCertificateNo, vAccidentalDeath, nAccidentalDeathSI, vPermTotalDiability, nPermTotalDiabilitySI, vPermPartialDiability, nPermPartialDiabilitySI"
                    //+ ", vTempTotalDiability, nTempTotalDiabilitySI, vAmbulanceCover, nAmbulanceCoverSI, vModificationAllowance, nModificationAllowanceSI, vCostOfSupportItems, nCostOfSupportItemsSI, vOutPatientTreatCover, nOutPatientTreatCoverSI, vChildEduGrant, nChildEduGrantSI"
                    //+ ", vMarriageBenefitChild, nMarriageBenefitChildSI, vDisappearBenefit, nDisappearBenefitSI, vCompassVisit, nCompassVisitSI, vSportsActCover, nSportsActCoverSI, vCarriageOfDeadBody, nCarriageOfDeadBodySI, vFuneralExpense, nFuneralExpenseSI, vDoubleInsuranceBenifit, nDoubleInsuranceBenifitSI"
                    //+ ", vHospitalCash, nHospitalCashSI, vConvalescence, nConvalescenceSI, vBurnBenefit, nBurnBenefitSI, vBrokenBoneBenefit, nBrokenBoneBenefitSI, vComaBenefit, nComaBenefitSI, vInPatientHos, nInPatientHosSI, vDomTravelForMedTreat, nDomTravelForMedTreatSI, vLossOfJob, nLossOfJobSI"
                    //+ ", vEndorsementType, vEndorsementReason, vEndorsementDesc, dEndorsementEffectiveDate, dEndorsementIssueDate, bEndorsementStatus, vAdditional_column_1, vAdditional_column_2, vAdditional_column_3"
                    //+ ", vAdditional_column_4, vAdditional_column_5, vAdditional_column_6, vClaimNo, vPaymentTo, vInstrumentNo, dClaimIntimationDate, dClaimPaidRepudClosedDate, nClaimAmount, bClaimStatus, vCreatedBy, vModifiedBy, CONVERT(VARCHAR(10), dCreatedDate, 105) as dCreatedDate,CONVERT(VARCHAR(10), dModifiedDate, 105) as dModifiedDate,CONVERT(VARCHAR(10), dDat_End_Date, 105) as dDat_End_Date"
                    //+ ",vErrorFlag,vErrorDesc,vTransType,vClaimsUploadId,vClaimRemarks,vChallanNumber,CONVERT(VARCHAR(10), dChallanDate, 105) as dChallanDate,nStampDuty,nSectionAPrem,nExtToSectionAPrem,nSectionBPrem,nPlanSI,vAccidentalDeathSIText,vPermTotalDiabilitySIText"
                    //+ ",vPermPartialDiabilitySIText,vTempTotalDiabilitySIText,vAmbulanceCoverSIText,vModificationAllowanceSIText,vCostOfSupportItemsSIText,vOutPatientTreatCoverSIText,vChildEduGrantSIText,vMarriageBenefitChildSIText,vDisappearBenefitSIText,vCompassVisitSIText,vSportsActCoverSIText,vCarriageOfDeadBodySIText,vFuneralExpenseSIText"
                    //+ ",vDoubleInsuranceBenifitSIText,vHospitalCashSIText,vConvalescenceSIText,vBurnBenefitSIText,vBrokenBoneBenefitSIText,vComaBenefitSIText,vInPatientHosSIText,vDomTravelForMedTreatSIText,vLossOfJobSIText"
                    ////+ ",vBankBranch,vProductCode,vProductName,vPlanCode,vProposalType,vFinancerName,vSIBasis,vMasterPolicyDate,vMasterPolicyLoc,vLoanType,vPolicyTenure,vLanNo,vLoanTenure,dLoanDisbursementDate,dLoanSanctionDate,vLoanOutAmount,vCustomerType,vOccupation,vComments,vRelationWithInsured,vAddCol1,vAddCol2,vAddCol3";
                    //+ ",vBankBranch,vProductCode,vProductName,vPlanCode,vProposalType,vFinancerName,vSIBasis,vMasterPolicyDate,vMasterPolicyLoc,vLoanType,vPolicyTenure,vLanNo,vLoanTenure,dLoanDisbursementDate,dLoanSanctionDate,vLoanOutAmount,vCustomerType,vOccupation,vComments,vRelationWithInsured,vAddCol1,vAddCol2,vAddCol3,vAccidentalDeathADSIText,vPermTotalDisablePTDSIText,vPermPartialDisablePTDSIText,vTempTotalDisableTTDSIText,vAccidentalMedicalExpSIText,vPurchaseofBloodSIText"
                    //+ ",vTransportationofImpMedicineSIText,vDisappearanceBenefitSIText,vDisappearanceBenefitSItext,vModificationofVehicleSIText,vCommonCarrierSIText,vMarraigeExpensesSIText,vSportsActivityCoverSIText,vWidowhoodCoverSIText,vDailyCashBenefitSIText,vAccidentalHospitalizationSIText,vOPDTreatmentSIText,vAccidentalDentalExpenseSIText,vConvalescenceBenefitSIText,vBurnsSIText,vBrokenBonesSIText,vComaSIText,vLossOfEmploymentSIText,vOnDutyCoverSIText,vLegalExpensesSIText"
                    //+ ",vAccidentalDeathAD,vPermTotalDisablePTD,vPermPartialDisablePTD,vTempTotalDisableTTD,vAccidentalMedicalExp,vPurchaseofBlood,vTransportationofImpMedicine,vDisappearanceBenefit,vModificationofVehicle,vCommonCarrier,vMarraigeExpenses,vSportsActivityCover,vWidowhoodCover,vDailyCashBenefit,vAccidentalHospitalization,vOPDTreatment,vAccidentalDentalExpense,vConvalescenceBenefit,vBurns,vBrokenBones,vComa,vLossOfEmployment,vOnDutyCover,vLegalExpenses,nAccidentalDeathADSI"
                    //+ ",nPermTotalDisablePTDSI,nPermPartialDisablePTDSI,nTempTotalDisableTTDSI,nAccidentalMedicalExpSI,nPurchaseofBloodSI,nTransportationofImpMedicineSI,nDisappearanceBenefitSI,nModificationofVehicleSI,nCommonCarrierSI,nMarraigeExpensesSI,nSportsActivityCoverSI,nWidowhoodCoverSI,nDailyCashBenefitSI,nAccidentalHospitalizationSI,nOPDTreatmentSI,nAccidentalDentalExpenseSI,nConvalescenceBenefitSI,nBurnsSI,nBrokenBonesSI,nComaSI,nLossOfEmploymentSI,nOnDutyCoverSI,nLegalExpensesSI"
                    //+ ",vCarraigeDeadBody,nCarraigeDeadBodySI,vCarraigeDeadBodySIText,vFuneralExpenses,nFuneralExpensesSI,vFuneralExpensesSIText,vCompassionateVisit,vCompassionateVisitSIText,nCompassionateVisitSI,vCostSupportItems,vCostSupportItemsSIText,nCostSupportItemsSI,vDomesticTravelForTreatment,vDomesticTravelForTreatmentSIText,nDomesticTravelForTreatmentSI,vPrevPolicyNumber,vAppointeeName,vPolicyType,vIntermediaryNumber";


//                    string columnS = "vUploadId,vProductName,vBusinesssType,vRMCode,vBranchName,vBranchCode,vCrnNo,vAccountNo,vCustomerName,vCustomerGender,dCustomerDob,vProposerAddLine1,vProposerAddLine2,vProposerAddLine3,vProposerCity,vProposerState,vProposerPinCode,vUniqueIdentificationNo,vMobileNo,vEmailId,vNomineeName,vNomineeRelation"
//+ ", vNomineeAge, vNomineeGuardian, vNomineeRelWithGuardian, vUniqueAccDebitRefNo, dAccountDebitDate, nDebitAmount, vIntermediaryCode, vIntermediaryName, vCorporateName, dPolicyEndDate, vPlanName, vMasterPolicyNo, vBankName"
//+ ", nNetPremium, nLoadingDiscount, nTotalPolicyPremium, dPolicyStartDate, dProposalDate, vCertificateNo, nAccidentalDeathSI,  nPermTotalDiabilitySI,  nPermPartialDiabilitySI"
//+ ",  nTempTotalDiabilitySI,  nAmbulanceCoverSI,  nModificationAllowanceSI,  nCostOfSupportItemsSI,  nOutPatientTreatCoverSI,  nChildEduGrantSI"
//+ ",  nMarriageBenefitChildSI,  nDisappearBenefitSI,  nCompassVisitSI,  nSportsActCoverSI,  nCarriageOfDeadBodySI,  nFuneralExpenseSI,  nDoubleInsuranceBenifitSI"
//+ ",  nHospitalCashSI,  nConvalescenceSI,  nBurnBenefitSI,  nBrokenBoneBenefitSI,  nComaBenefitSI, nInPatientHosSI,  nDomTravelForMedTreatSI,  nLossOfJobSI"
//+ ", vEndorsementType, vEndorsementReason, vEndorsementDesc, dEndorsementEffectiveDate, dEndorsementIssueDate, bEndorsementStatus"
//+ ", vClaimNo, vPaymentTo, vInstrumentNo, dClaimIntimationDate, dClaimPaidRepudClosedDate, nClaimAmount, bClaimStatus, vCreatedBy, vModifiedBy, CONVERT(VARCHAR(10), dCreatedDate, 105) as dCreatedDate,CONVERT(VARCHAR(10), dModifiedDate, 105) as dModifiedDate,CONVERT(VARCHAR(10), dDat_End_Date, 105) as dDat_End_Date"
//+ ",vErrorFlag,vErrorDesc,vTransType,vClaimsUploadId,vClaimRemarks,vChallanNumber,CONVERT(VARCHAR(10), dChallanDate, 105) as dChallanDate,nStampDuty,nSectionAPrem,nExtToSectionAPrem,nSectionBPrem,nPlanSI,vAccidentalDeathSIText,vPermTotalDiabilitySIText"
//+ ",vPermPartialDiabilitySIText,vTempTotalDiabilitySIText,vAmbulanceCoverSIText,vModificationAllowanceSIText,vCostOfSupportItemsSIText,vOutPatientTreatCoverSIText,vChildEduGrantSIText,vMarriageBenefitChildSIText,vDisappearBenefitSIText,vCompassVisitSIText,vSportsActCoverSIText,vCarriageOfDeadBodySIText,vFuneralExpenseSIText"
//+ ",vDoubleInsuranceBenifitSIText,vHospitalCashSIText,vConvalescenceSIText,vBurnBenefitSIText,vBrokenBoneBenefitSIText,vComaBenefitSIText,vInPatientHosSIText,vDomTravelForMedTreatSIText,vLossOfJobSIText"
////+ ",vBankBranch,vProductCode,vProductName,vPlanCode,vProposalType,vFinancerName,vSIBasis,vMasterPolicyDate,vMasterPolicyLoc,vLoanType,vPolicyTenure,vLanNo,vLoanTenure,dLoanDisbursementDate,dLoanSanctionDate,vLoanOutAmount,vCustomerType,vOccupation,vComments,vRelationWithInsured,vAddCol1,vAddCol2,vAddCol3";
//+ ",vBankBranch,vProductCode,vProductName,vPlanCode,vProposalType,vFinancerName,vSIBasis,vMasterPolicyDate,vMasterPolicyLoc,vLoanType,vPolicyTenure,vLanNo,vLoanTenure,dLoanDisbursementDate,dLoanSanctionDate,vLoanOutAmount,vCustomerType,vOccupation,vComments,vRelationWithInsured,vAddCol1,vAddCol2,vAddCol3,vAccidentalDeathADSIText,vPermTotalDisablePTDSIText,vPermPartialDisablePTDSIText,vTempTotalDisableTTDSIText,vAccidentalMedicalExpSIText,vPurchaseofBloodSIText"
//+ ",vTransportationofImpMedicineSIText,vDisappearanceBenefitSIText,vDisappearanceBenefitSItext,vModificationofVehicleSIText,vCommonCarrierSIText,vMarraigeExpensesSIText,vSportsActivityCoverSIText,vWidowhoodCoverSIText,vDailyCashBenefitSIText,vAccidentalHospitalizationSIText,vOPDTreatmentSIText,vAccidentalDentalExpenseSIText,vConvalescenceBenefitSIText,vBurnsSIText,vBrokenBonesSIText,vComaSIText,vLossOfEmploymentSIText,vOnDutyCoverSIText,vLegalExpensesSIText"
//+ ",nAccidentalDeathADSI"
//+ ",nPermTotalDisablePTDSI,nPermPartialDisablePTDSI,nTempTotalDisableTTDSI,nAccidentalMedicalExpSI,nPurchaseofBloodSI,nTransportationofImpMedicineSI,nDisappearanceBenefitSI,nModificationofVehicleSI,nCommonCarrierSI,nMarraigeExpensesSI,nSportsActivityCoverSI,nWidowhoodCoverSI,nDailyCashBenefitSI,nAccidentalHospitalizationSI,nOPDTreatmentSI,nAccidentalDentalExpenseSI,nConvalescenceBenefitSI,nBurnsSI,nBrokenBonesSI,nComaSI,nLossOfEmploymentSI,nOnDutyCoverSI,nLegalExpensesSI"
//+ ",vCarraigeDeadBody,nCarraigeDeadBodySI,vCarraigeDeadBodySIText,vFuneralExpenses,nFuneralExpensesSI,vFuneralExpensesSIText,vCompassionateVisit,vCompassionateVisitSIText,nCompassionateVisitSI,vCostSupportItems,vCostSupportItemsSIText,nCostSupportItemsSI,vDomesticTravelForTreatment,vDomesticTravelForTreatmentSIText,nDomesticTravelForTreatmentSI,vPrevPolicyNumber,vAppointeeName,vPolicyType,vIntermediaryNumber,ugstPercentage,ugstAmount,cgstPercentage,cgstAmount,sgstPercentage,sgstAmount,igstPercentage,igstAmount,totalGSTAmount";

//                    cmd.CommandText = "SELECT "+ columnS + " FROM TBL_GPA_POLICY_TABLE where DateAdd(day, datediff(day,0, dCreatedDate), 0) between Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ") " + strvPolicyId + " " + strvUploadId + "  UNION ALL" +
//                    " SELECT " + columnS + " FROM TBL_GPA_POLICY_TABLE_ERROR_LOG where DateAdd(day, datediff(day,0, dCreatedDate), 0) between Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ") " + strvPolicyId + " " + strvUploadId + "";

                    cmd.Connection = sqlCon;
                    sqlCon.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string filePath = Server.MapPath("~/Reports");

                        string _DownloadableProductFileName = "GPA_UPLOAD_DUMP_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString()+ DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls";

                        String strfilename = filePath + "\\" + _DownloadableProductFileName;

                        if (System.IO.File.Exists(strfilename))
                        {
                            System.IO.File.Delete(strfilename);
                        }

                        if (ExportDataTableToExcel(dt, "GPA_UPLOAD_DUMP", strfilename) == true)
                        {
                            DownloadFile(strfilename);
                        }
                        sqlCon.Close();
                    }
                    else
                    {
                        sqlCon.Close();
                        Alert.Show("No Records found for selected filters");
                        return;
                    }
                }
            }
        }

        private bool ExportDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation)
        {
            //Creae an Excel application instance

            ExcelPackage excelApp = new ExcelPackage(new FileInfo(saveAsLocation));
            ExcelRange oRng;

            try
            {

                // Workk sheet
                var excelSheet = excelApp.Workbook.Worksheets.Add(worksheetName);
                excelSheet.Name = worksheetName;

                excelSheet.Cells[1, 1].Value = "Upload Details Dump";
                excelSheet.Cells[2, 1].Value = "Report Taken On Date : " + DateTime.Now.ToShortDateString();

                // loop through each row and add values to our sheet
                int rowcount = 2;


                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;

                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 3)
                        {
                            excelSheet.Cells[3, i].Value = dataTable.Columns[i - 1].ColumnName;
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
                                    FormattingExcelCells(oRng, "#CCCCFF", System.Drawing.Color.Black, false);
                                }
                            }
                        }
                    }
                }

                // now we resize the columns
                oRng = (ExcelRange)excelSheet.Cells[3, dataTable.Columns.Count];
                oRng.AutoFitColumns();
                BorderAround(oRng, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189)));

                oRng = (ExcelRange)excelSheet.Cells[3, dataTable.Columns.Count];
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

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}