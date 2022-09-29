using Microsoft.Practices.EnterpriseLibrary.Data;
using ProjectPASS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjPASS
{
    public partial class FrmUploadCTIFile : System.Web.UI.Page
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

        protected void Upload(object sender, EventArgs e)
        {           
            try
            {

                //Upload and save the file
                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string excelPath = Server.MapPath("~/CTIUploads/") + fileName;

                FileUpload1.SaveAs(excelPath);

                string conString = string.Empty;

                Database db = DatabaseFactory.CreateDatabase("cnPASS");
                using (SqlConnection con = new SqlConnection(db.ConnectionString))
                {
                    con.Open();
                    string insertCommand = "INSERT INTO TBL_CTI_UPLOAD_DETAILS (CTI_File_Name,CTI_Uploaded_User) VALUES (@fileName,@userName)";
                    SqlCommand insertCmd = new SqlCommand(insertCommand, con);

                    var sqlParamFile = new SqlParameter("fileName", SqlDbType.VarChar);
                    var sqlParamUser = new SqlParameter("userName", SqlDbType.VarChar);

                    sqlParamFile.Value = fileName;
                    sqlParamUser.Value = Session["vUserLoginId"].ToString();

                    insertCmd.Parameters.Add(sqlParamFile);
                    insertCmd.Parameters.Add(sqlParamUser);                    
                    insertCmd.ExecuteNonQuery();
                }
                
                Session["ErrorCallingPage"] = "FrmUploadCTIFile.aspx";
                string statusMsg = "File Uploaded Successfully";
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + statusMsg, false);
                return;
            }
            catch (Exception ex)
            {                               
                Session["ErrorCallingPage"] = "FrmUploadCTIFile.aspx";
                string statusMsg = "No File Was Uplpoaded or " + ex.Message.ToString().Replace("\r\n", "");
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + statusMsg, false);
                return;
                //log the error
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }
    }
}