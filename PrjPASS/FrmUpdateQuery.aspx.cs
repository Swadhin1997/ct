using Microsoft.Practices.EnterpriseLibrary.Data;
using Obout.ComboBox;
using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmUpdateQuery : System.Web.UI.Page
    {
        public Cls_General_Functions wsGen = new Cls_General_Functions();

        string FrmUpdateQueryLog = ConfigurationManager.AppSettings["FrmUpdateQuery"];
        public string logfile = "FrmUpdateQuery_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
        public string servernname = ConfigurationManager.AppSettings["ServerNameForUpdateQuery"];
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
                        string userid = Session["vUserLoginId"].ToString();
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
                FillProductData();
            }
        }


        private void FillProductData()
        {
            drpType.Items.Clear();
            Microsoft.Practices.EnterpriseLibrary.Data.Database db = DatabaseFactory.CreateDatabase("cnPASS");
            string sqlCommand = "SELECT * FROM TBL_FILLDROP_FOR_UPDATE_IMD WHERE bIsForBPOS = " + (rdoPASS.Checked ? "0" : "1");
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet dsCover = null;
            dsCover = db.ExecuteDataSet(dbCommand);
            if (dsCover.Tables[0].Rows.Count > 0)
            {
                drpType.DataValueField = "SelectList";
                drpType.DataTextField = "SelectList1";
                drpType.DataSource = dsCover.Tables[0];
                drpType.DataBind();
                ComboBoxItem l_lstItem = new ComboBoxItem("Select", "-1");
                //ddlProduct.Items.Insert(0, l_lstItem);
                drpType.Items.Insert(0, l_lstItem);
            }
            else
            {
                Alert.Show("No Schema Defined in Master");
                return;
            }
        }

        protected void Upload(object sender, EventArgs e)
        {

            Microsoft.Practices.EnterpriseLibrary.Data.Database dbCOMMON = DatabaseFactory.CreateDatabase("cnPASS");
            Cls_General_Functions wsDocNo = new Cls_General_Functions();
            string cYearMonth = "", vUploadId = "";
            string vCertificateNo = "";
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

            //vUploadId = wsDocNo.fn_Gen_Doc_Master_No("PUPL", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy hh:mm:ss tt"), ref _tran, ref _con);

            try
            {

                //Upload and save the file

                string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(FileUpload2.PostedFile.FileName);

                FileUpload2.SaveAs(excelPath);

                string conString = string.Empty;

                string extension = Path.GetExtension(FileUpload2.PostedFile.FileName);

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
                DataTable dtExcelData = new DataTable();
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();
                    string sheet1 = rdoPASS.Checked ? "Intermediary_code$" : excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                        // Session["data"] = dtExcelData;
                    }
                    excel_con.Close();
                }


                string ipAd = string.Empty;
                try
                {
                    if (rdoPASS.Checked)
                    {
                        //if (Session["InActive"] == "1")
                        //{
                        string strHtml;
                        DataTable dt1 = new DataTable();
                        //dt1 = (DataTable)Session["data"];
                        dt1 = dtExcelData;
                        StringBuilder sb = new StringBuilder();
                        //sb.Append("'");
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            //sb.Append("'");
                            //sb.Append(dt1.Rows[i]["IntermediaryCode"]);
                            //sb.Append("'','");

                            // sb.Append("'");
                            sb.Append(dt1.Rows[i]["IntermediaryCode"]);
                            sb.Append(",");
                        }
                        strHtml = sb.ToString();
                        //strHtml = strHtml.Remove(strHtml.Length - 2, 2);
                        string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                        ipAd = GetMachineIPAddress();

                        SqlConnection con = new SqlConnection(consString);
                        con.Open();
                        DataTable dt = new DataTable();
                        SqlCommand cmd = new SqlCommand("UPDATEINTERMEDIARYCODE", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        //SqlCommand cmd = new SqlCommand("UPDATE OPENQUERY (" + servernname + ", 'SELECT STATUS,ENDDATE,Dat_Modify_Date,NUM_MODIFY_TRANS_ID  FROM CNFGTR_USER_MSTR a WHERE a.USERID IN (" + strHtml + ") AND a.STATUS = ''Active'' AND a.STATUS NOT IN (''Pending'',''InActive'') AND a.USERID in (select TXT_INTERMEDIARY_CD  from genmst_intermediary where a.USERID = TXT_INTERMEDIARY_CD )')   SET STATUS = 'InActive' , ENDDATE = GETDATE(),Dat_Modify_Date=GETDATE(),NUM_MODIFY_TRANS_ID=99999;", con);
                        //cmd.CommandType = CommandType.Text;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@USERID", Session["vUserLoginId"]);
                        cmd.Parameters.AddWithValue("@INTERMEDIARYCODE", strHtml);
                        cmd.Parameters.AddWithValue("@Action1", Session["InActive"]);
                        //cmdd2.Parameters.AddWithValue("@ActivityDateTime", Convert.ToDateTime(System.DateTime.Now.ToString()));
                        cmd.Parameters.AddWithValue("@IPADDRESS", ipAd);


                        cmd.ExecuteNonQuery();
                        con.Close();

                        //ipAd = GetMachineIPAddress();


                        //string Dat_Modify_Date = string.Empty;
                        //using (SqlConnection con8 = new SqlConnection(consString))
                        //{
                        //    if (con8.State == ConnectionState.Closed)
                        //    {
                        //        con8.Open();
                        //    }
                        //    SqlCommand cmd3 = new SqlCommand(" SELECT * FROM OPENQUERY(" + servernname + ", 'select Dat_Modify_Date from CNFGTR_USER_MSTR WHERE USERID in (" + strHtml + ")') ", con8);
                        //    cmd3.CommandType = CommandType.Text;
                        //    SqlDataReader dr = cmd3.ExecuteReader();
                        //    {
                        //        if (dr.HasRows)
                        //        {
                        //            while (dr.Read())
                        //            {
                        //                Dat_Modify_Date = dr[0].ToString();
                        //            }
                        //        }
                        //    }
                        //}

                        //SqlConnection conn2 = new SqlConnection(consString);

                        //DataTable dtd1 = new DataTable();
                        //conn2.Open();
                        //SqlCommand cmd = new SqlCommand("IMD_UPDATED_DATA_HISTORY", conn2);
                        //cmdd2.CommandType = CommandType.StoredProcedure;
                        //cmdd2.Parameters.AddWithValue("@USERID", Session["vUserLoginId"]);
                        //cmdd2.Parameters.AddWithValue("@INTERMEDIARYCODE", strHtml);
                        //cmdd2.Parameters.AddWithValue("@Action1", "InActive");
                        ////cmdd2.Parameters.AddWithValue("@ActivityDateTime", Convert.ToDateTime(System.DateTime.Now.ToString()));
                        //cmdd2.Parameters.AddWithValue("@ActivityDateTime", Dat_Modify_Date);
                        //cmdd2.Parameters.AddWithValue("@IPADDRESS", ipAd);
                        ////SqlDataAdapter da1 = new SqlDataAdapter(cmdd1);
                        ////da1.Fill(dtd1);
                        //cmdd2.ExecuteNonQuery();
                        //conn2.Close();

                        //string Message = "Records " + Session["InActive"] =="1" ? "In -Activated" : "Activated" + " Successfully.";

                        string Message = Convert.ToString(Session["InActive"]) == "1" ? "Records In-Activated Successfully." : " Records Activated Successfully.";

                        Alert.Show(Message);
                        Session.Remove("InActive");
                        Session["InActive"] = "";
                        drpType.SelectedIndex = 0;

                        //}
                        //else
                        //{
                        //    if (Session["Active"] == "2")
                        //    {
                        //        string strHtml;
                        //        DataTable dt1 = new DataTable();
                        //        dt1 = dtExcelData;
                        //        StringBuilder sb = new StringBuilder();
                        //        sb.Append("'");
                        //        for (int i = 0; i < dt1.Rows.Count; i++)
                        //        {
                        //            sb.Append("'");
                        //            sb.Append(dt1.Rows[i]["IntermediaryCode"]);
                        //            sb.Append("'','");
                        //        }
                        //        strHtml = sb.ToString();
                        //        strHtml = strHtml.Remove(strHtml.Length - 2, 2);
                        //        string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

                        //        SqlConnection con = new SqlConnection(consString);
                        //        con.Open();
                        //        DataTable dt = new DataTable();
                        //        //SqlCommand cmd = new SqlCommand("UPDATEINTERMEDIARYCODE", con);
                        //        //cmd.CommandType = CommandType.StoredProcedure;
                        //        SqlCommand cmd = new SqlCommand("UPDATE OPENQUERY (" + servernname + ", 'SELECT  STATUS,ENDDATE,Dat_Modify_Date,NUM_MODIFY_TRANS_ID  FROM CNFGTR_USER_MSTR a WHERE a.USERID IN (" + strHtml + ") AND a.STATUS = ''InActive'' AND a.STATUS NOT IN (''Pending'',''Active'') AND a.USERID in (select TXT_INTERMEDIARY_CD  from genmst_intermediary where a.USERID = TXT_INTERMEDIARY_CD )')   SET STATUS = 'Active' , ENDDATE = GETDATE(),Dat_Modify_Date=GETDATE(),NUM_MODIFY_TRANS_ID=99999;", con);
                        //        cmd.CommandType = CommandType.Text;
                        //        cmd.ExecuteNonQuery();
                        //        con.Close();

                        //        ipAd = GetMachineIPAddress();

                        //        string Dat_Modify_Date = string.Empty;
                        //        using (SqlConnection con8 = new SqlConnection(consString))
                        //        {
                        //            if (con8.State == ConnectionState.Closed)
                        //            {
                        //                con8.Open();
                        //            }
                        //            SqlCommand cmd3 = new SqlCommand(" SELECT * FROM OPENQUERY(" + servernname + ", 'select Dat_Modify_Date from CNFGTR_USER_MSTR WHERE USERID in (" + strHtml + ")') ", con8);
                        //            cmd3.CommandType = CommandType.Text;
                        //            SqlDataReader dr = cmd3.ExecuteReader();
                        //            {
                        //                if (dr.HasRows)
                        //                {
                        //                    while (dr.Read())
                        //                    {
                        //                        Dat_Modify_Date = dr[0].ToString();
                        //                    }
                        //                }
                        //            }
                        //        }



                        //        SqlConnection conn1 = new SqlConnection(consString);

                        //        DataTable dtd1 = new DataTable();
                        //        conn1.Open();
                        //        SqlCommand cmdd1 = new SqlCommand("IMD_UPDATED_DATA_HISTORY", conn1);
                        //        cmdd1.CommandType = CommandType.StoredProcedure;
                        //        cmdd1.Parameters.AddWithValue("@USERID", Session["vUserLoginId"]);
                        //        cmdd1.Parameters.AddWithValue("@INTERMEDIARYCODE", strHtml);
                        //        cmdd1.Parameters.AddWithValue("@Action1", "Active");
                        //        //cmdd1.Parameters.AddWithValue("@ActivityDateTime", Convert.ToDateTime(System.DateTime.Now.ToString()));
                        //        cmdd1.Parameters.AddWithValue("@ActivityDateTime", Dat_Modify_Date);
                        //        cmdd1.Parameters.AddWithValue("@IPADDRESS", ipAd);
                        //        //SqlDataAdapter da1 = new SqlDataAdapter(cmdd1);
                        //        //da1.Fill(dtd1);
                        //        cmdd1.ExecuteNonQuery();
                        //        conn1.Close();
                        //        Session.Remove("Active");
                        //        Session["Active"] = "";
                        //        //SqlCommand cmdd = new SqlCommand("UPDATE OPENQUERY (" + servernname + ", 'SELECT  STATUS,ENDDATE,Dat_Modify_Date,NUM_MODIFY_TRANS_ID  FROM CNFGTR_USER_MSTR a WHERE a.USERID IN (" + strHtml + ") AND a.STATUS = ''InActive'' AND a.USERID in (select TXT_INTERMEDIARY_CD  from genmst_intermediary where a.USERID = TXT_INTERMEDIARY_CD )')   SET STATUS = 'Active' , ENDDATE = GETDATE(),Dat_Modify_Date=GETDATE(),NUM_MODIFY_TRANS_ID=99999;", con);

                        //        //}
                        //        drpType.SelectedIndex = 0;
                        //        //drpType.SelectedValue = "Select";
                        //        Alert.Show("Records Activated Successfully.");
                        //    }
                        //}
                        //if (drpType.SelectedIndex == 0)
                        //{

                        //}
                        //else
                        //{
                        //    Session.Remove("data");
                        //    Session["data"] = "";
                        //    Session["data"] = null;
                        //}
                        //Session.Clear();
                    }
                    else if (rdoBPOS.Checked)
                    {
                        if (drpType.SelectedValue == "")
                        {
                            Alert.Show("No option selected from Dropdown. Please select and proceed", "FrmUpdateQuery.aspx");
                            return;
                        }
                        int noOfUsers = 0;
                        StringBuilder sb = new StringBuilder();

                        for (int i = 0; i < dtExcelData.Rows.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(dtExcelData.Rows[i][0].ToString()))
                            {
                                noOfUsers++;
                                sb.Append(i == 0 ? "'" : ",'");
                                sb.Append(dtExcelData.Rows[i][0]);
                                sb.Append("'");
                            }
                        }

                        if (noOfUsers == 0)
                        {
                            Alert.Show("No record found in Uploaded Excel Sheet. Please check!", "FrmUpdateQuery.aspx");
                            return;
                        }

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnBPOS"].ConnectionString))
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("UPDATE TBL_USER_MASTER SET bIsDocumentProcessed = " + drpType.SelectedValue + ", dModifiedDate = GETDATE(), vModifiedBy = '" + Session["vUserLoginId"].ToString().ToUpper() + "' WHERE vLoginEmailId IN(" + sb.ToString() + ")", con);
                            clsAppLogs.LogEvent("User - " + Session["vUserLoginId"].ToString().ToUpper() + " Executing Command (" + cmd.CommandText + ") on BPOS Database");
                            cmd.ExecuteNonQuery();
                            Alert.Show("Records Saved Successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(FrmUpdateQueryLog + logfile, "drpType_SelectedIndexChanged ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(FrmUpdateQueryLog + logfile, "btnImport_Click ::Error occured  :" + ex.Message + " and stack trace is : " + ex.StackTrace + DateTime.Now + Environment.NewLine);
            }

        }

        //protected void rdbActive_CheckedChanged(object sender, EventArgs e)
        //{

        //    string strHtml;

        //    DataTable dt1 = new DataTable();
        //    dt1 = (DataTable)Session["data"];
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i <= dt1.Rows.Count; i++)
        //    {
        //        sb.Append("'");
        //        sb.Append(dt1.Rows[i]);
        //        sb.Append("';");
        //    }
        //    strHtml = sb.ToString();
        //    string consString = ConfigurationManager.ConnectionStrings["cnPASS"].ConnectionString;

        //    using (SqlConnection con = new SqlConnection(consString))
        //    {
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("UPDATEINTERMEDIARYCODE", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@USERID", strHtml);
        //        //cmd.Parameters.AddWithValue("@TXT_INTERMEDIARY_CD", Convert.ToString(Session["vUserLoginId"]));
        //        //cmd.Parameters.AddWithValue("@TXT_INTERMEDIARY_CD", strHtml);
        //        //cmd.Parameters.AddWithValue("@flag", "1");
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        cmd.ExecuteNonQuery();
        //    }
        //}

        public string GetMachineIPAddress()
        {
            string ipAd = string.Empty;
            ipAd = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(ipAd))
            {
                ipAd = Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(ipAd))
            {
                ipAd = Request.UserHostAddress;
            }
            return ipAd;
        }

        protected void drpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoPASS.Checked)
            {
                if (drpType.SelectedIndex == 1)
                {
                    Session["Inactive"] = "1";
                }
                if (drpType.SelectedIndex == 2)
                {
                    Session["InActive"] = "2";
                }
            }
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/Reports");
            String strfilename = filePath + "\\" + (rdoPASS.Checked ? "Imdcode_sample.xlsx" : "BPOSUsersIsDocumentProcessed_Sample.xlsx");
            DownloadFile(strfilename);
        }

        protected void rdo_CheckedChanged(object sender, EventArgs e)
        {
            FillProductData();
        }
    }
}