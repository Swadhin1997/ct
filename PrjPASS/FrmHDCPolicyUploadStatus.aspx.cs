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
using ProjectPASS;

namespace PrjPASS
{
    public partial class FrmHDCPolicyUploadStatus : System.Web.UI.Page
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
                using (SqlCommand cmd = new SqlCommand())
                {
                    string strvPolicyId = "";
                    string strvUploadId = "";

                    if (txtPolicyId.Text.Trim() != "")
                    {
                        strvPolicyId = " and vCertificateNo ='" + txtPolicyId.Text.Trim() + "'";
                    }
                    if (txtUploadId.Text.Trim() != "")
                    {
                        strvUploadId = " and vUploadId='" + txtUploadId.Text.Trim() + "'";
                    }
                    if (txtFromDate.Text == "" && txtUploadId.Text.Trim() != " ")
                    {
                        Alert.Show("Please Select From Date.");
                        return;
                    }
                    if (txtToDate.Text == "" && txtUploadId.Text.Trim() != " ")
                    {
                        Alert.Show("Please Select To Date.");
                        return;
                    }
                    string cDateFormat = ConfigurationManager.AppSettings["cDateFormat"].ToString();
                    string cCultureFormat = ConfigurationManager.AppSettings["cCultureFormat"].ToString();

                    //string columnS = "vProductCode,vProductName,vTransactionType,vPolicyType,vKGIBranchName,vKGIBranchCode,vKGIBranchAddress,vRMCode,vKGIGSTN,vUINno,vUniqueAccDebitRefNo,isnull(Format(Convert(datetime,vAccountDebitDate,103),'dd-MMM-yy'),'') vAccountDebitDate,vDebitAmount,vPaymentMode,vIntermediaryName,vIntermediaryCode,vIntermediaryContact,vBankBranch,vPlanName,vCorporateName,vMasterPolicyNo,vMasterPolicyHolder,isnull(Format(Convert(datetime,vMasterPolicyIssueDate,103),'dd-MMM-yy'),'') vMasterPolicyIssueDate,vMasterPolicyIssueLocation,vGroupType,vCertificateNo,isnull(Format(Convert(datetime,vTransactionDate,103),'dd-MMM-yy'),'') vTransactionDate,isnull(Format(Convert(datetime,vPolicyStartdate,103),'dd-MMM-yy'),'') vPolicyStartdate,isnull(Format(Convert(datetime,vPolicyEndDate,103),'dd-MMM-yy'),'') vPolicyEndDate,vPolicyTenure,vPolicyCategory,vLoanTenure,isnull(Format(Convert(datetime,vLoanDisbursementDate,103),'dd-MMM-yy'),'') vLoanDisbursementDate,vLoanOutAmount,vCustomerType,vFinancerName,[vUnique_Id_No/LANNo],vCrnNo,vAccountNo,vCustomerName,vCustomerGender,isnull(Format(Convert(datetime,vCustomerDob,103),'dd-MMM-yy'),'') vCustomerDob,vOccupation,vRelationWithInsured,vProposerAddLine1,vProposerAddLine2,vProposerAddLine3,vProposerCity,vProposerState,vProposerPinCode,vMobileNo,vEmailId,vNomineeName,vNomineeRelation,case when isdate([vNomineeDOB/Age])=0 then [vNomineeDOB/Age] else isnull(Format(Convert(datetime,[vNomineeDOB/Age],103),'dd/MM/yyyy'),'') end  [vNomineeDOB/Age],vCustomerPAN,vCustomerAadhar,vCustomerGSTIN,vStampDuty,vChallanNo,isnull(Format(Convert(datetime,vChallanDate,103),'dd-MMM-yy'),'') vChallanDate,vTotalPolicySumInsured,vBasePremium,[vLoading/Discount],vNetPremium,vIGST,vCGST,vSGST,vUGST,vTotalPremium,vDeductableofBaseCover,vHospitalDailyCashBenefit,vAccidentDailyCashBenefit,vICUDailyCashBenefit,vConvalescenceBenefit,vCompanionBenefit,vJointHospitalisation,vParentAccommodation,vDayCareProcedureBenefit,vSurgeryBenefit,vAccidentalHospitalisationBenefit,vBrokenBones,vBurns,vMaternityBenefit,vNewBornBabyBenefit,vAlternativeTreatmentBenefit,vWorldwideCover,vPersonalAccidentBenefit,vCriticalIllnessBenefit,vCondition1,vCondition2,vCondition3,vCondition4,vCondition5,[vComments/Remarks],vAddCol1,vAddCol2,vAddCol3,vEndorsementType,vEndorsementReason,vEndorsementDesc,vEndorsementEffectiveDate,vEndorsementIssueDate,vEndorsementStatus,vUploadId,vErrorFlag,vErrorDesc,vTransType";
                    //cmd.CommandText = "SELECT " + columnS + " FROM TBL_HDC_POLICY_TABLE where DateAdd(day, datediff(day,0, dCreatedDate), 0) between Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ") " + strvPolicyId + " " + strvUploadId + "  UNION ALL" +
                    //" SELECT " + columnS + " FROM TBL_HDC_POLICY_TABLE_ERROR_LOG where DateAdd(day, datediff(day,0, dCreatedDate), 0) between Convert(datetime,'" + txtFromDate.Text + "'," + cDateFormat + ") and Convert(datetime,'" + txtToDate.Text + "'," + cDateFormat + ") " + strvPolicyId + " " + strvUploadId + "";

                    //cmd.CommandText = "PROC_HDC_POLICY_STATUS";
                    cmd.CommandText = "PROC_HDC_POLICY_REPLICA_STATUS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", txtFromDate.Text);
                    cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
                    cmd.Parameters.AddWithValue("@PolicyID", txtPolicyId.Text.Trim());
                    cmd.Parameters.AddWithValue("@UploadID", txtUploadId.Text.Trim());
                    cmd.Connection = sqlCon;
                    sqlCon.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string filePath = Server.MapPath("~/Reports");

                        string _DownloadableProductFileName = "HDC_UPLOAD_DUMP_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls";

                        String strfilename = filePath + "\\" + _DownloadableProductFileName;

                        if (System.IO.File.Exists(strfilename))
                        {
                            System.IO.File.Delete(strfilename);
                        }

                        if (ExportDataTableToExcel(dt, "HDC_UPLOAD_DUMP", strfilename) == true)
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