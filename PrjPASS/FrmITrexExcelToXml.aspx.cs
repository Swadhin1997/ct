using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using ProjectPASS;
using System.Text;
using System.IO.Compression;


namespace PrjPASS
{
    public partial class FrmITrexExcelToXml : System.Web.UI.Page
    {
        DataTable dtxmlData = new DataTable();
        DataTable dtxmlconfig = new DataTable();
        Cls_General_Functions wsGen = new Cls_General_Functions();
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

        protected void BtnUpload_Click(object sender,EventArgs e)
        {

            string filePath = Server.MapPath("~/Uploads/ITrex_excel/")+ Path.GetFileName(DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()+"_" + FileUpload1.PostedFile.FileName);
            string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
            FileUpload1.SaveAs(filePath);
            string cs = string.Empty;
           
            switch (fileExtension)
            {
                case ".xls":
                    cs = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    cs = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                    break;
            }
            cs = string.Format(cs , filePath);
            using (OleDbConnection con = new OleDbConnection(cs))
            {

                //OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;
                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
               
               // oleDbDataAdapter.Fill(dt);
                DataTable dtSheet = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = dtSheet.Rows[0]["table_name"].ToString();
                cmd.CommandText = "Select * from ["+sheetName+"]";
                oleDbDataAdapter.SelectCommand = cmd;
                oleDbDataAdapter.Fill(dtxmlData);
                cmd.CommandType = CommandType.Text;
                con.Close();
            }
            if (dtxmlData.Rows.Count>0)
            {
                //List<string> dtcol = dtxmlData.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
                
                    Session["UploadedFile"] = FileUpload1.PostedFile.FileName;
                    GetXMLNodeData();
                    GenerateXML();
                    //Alert.Show("File Converted Successfully!!");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('File Converted Successfully!!!');", true);
                
               
            }
            else
            {
                Session["UploadedFile"] = null;

                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('No Records Found!!!');", true);
            }

        }

        private void GetXMLNodeData()
        {
            string cs = string.Empty;
             cs = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
            string xmlConfigPath = Server.MapPath("~/xmlNodeConfig.xlsx");
            cs = string.Format(cs, xmlConfigPath);

            using (OleDbConnection con = new OleDbConnection(cs))
            {
                OleDbCommand oleDbCommand = new OleDbCommand();
                OleDbDataAdapter daxml = new OleDbDataAdapter(oleDbCommand);
                oleDbCommand.Connection = con;
                oleDbCommand.CommandType = CommandType.Text;
                con.Open();
                DataTable dtSheet = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = dtSheet.Rows[0]["table_name"].ToString();
             
                oleDbCommand.CommandText = "select *  from [" + sheetName + "]";
                daxml.SelectCommand = oleDbCommand;
                daxml.Fill(dtxmlconfig);
            }
        }

        private void GenerateXML()
        {
            string xmlheader = string.Empty;
            string xmlParentNode = string.Empty;
            String strFileName = string.Empty;
            string filePath = string.Empty;
            string fileName = string.Empty;
            //Create folder on sever to save xml file**
            filePath = Server.MapPath("~/Reports/ITrexXml" + "_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString()+DateTime.Now.Second.ToString());
            
            string zipFilePath = Server.MapPath("~/ITREX.zip");
            // If file already exist then delete the same**
            if (Directory.Exists(filePath))
            {
                var files = from file in
                            Directory.EnumerateFiles(filePath)
                            select file;
                foreach (var item in files)
                {
                    //delete all the exisiting file from the server folder**
                    File.Delete(item);
                }

                //Deleting subfolder form the path**
                Directory.Delete(filePath);
                // again created new folder to save XMl file
                Directory.CreateDirectory(filePath);
            }
            Directory.CreateDirectory(filePath);
            try
            {
                for (int i = 0; i < dtxmlData.Rows.Count; i++)
                {
                    XmlDocument doc = new XmlDocument();
                    XmlDeclaration declaire = doc.CreateXmlDeclaration("1.0", "utf-8", null);

                    doc.InsertBefore(declaire, doc.DocumentElement);
                    XmlElement rootnode = doc.CreateElement(dtxmlconfig.Rows[0][2].ToString());
                    doc.AppendChild(rootnode);
                    XmlElement childnode = null;
                    XmlNode childelement = null;
                    XmlElement childelementextra = null;
                    int parentcount = 0;
                    int count = 0;
                    //List<FileInfo> file = new List<FileInfo>();
                    for (int j = 0; j < dtxmlconfig.Rows.Count; j++)
                    {

                        if (string.IsNullOrEmpty(xmlheader) || xmlheader != dtxmlconfig.Rows[j]["XML_ASSIGN_NODE"].ToString())
                        {
                            xmlheader = dtxmlconfig.Rows[j]["XML_ASSIGN_NODE"].ToString().Trim();
                            xmlParentNode = dtxmlconfig.Rows[j]["XML_PARENT_NODE"].ToString().Trim();
                            count = 0;
                        }
                        for (int k = 0; k < dtxmlconfig.Rows.Count; k++)
                        {

                            if (dtxmlconfig.Rows[k]["XML_DATA_NODE"].ToString() == dtxmlData.Columns[j].ColumnName)
                            {

                                if (dtxmlconfig.Rows[k]["XML_ASSIGN_NODE"].ToString() == xmlheader && count == 0)
                                {
                                    childnode = doc.CreateElement(dtxmlconfig.Rows[k]["XML_ASSIGN_NODE"].ToString());
                                    count = 1;
                                    //break;
                                }
                                childelement = doc.CreateElement(dtxmlconfig.Rows[j]["XML_DATA_NODE"].ToString().Trim());
                                childelement.InnerText = dtxmlData.Rows[i][j].ToString();
                                childnode.AppendChild(childelement);


                            }
                        }

                        if (dtxmlconfig.Rows[j]["XML_PARENT_NODE"].ToString().Trim() == dtxmlconfig.Rows[0][2].ToString().Trim())
                        {
                            rootnode.AppendChild(childnode);
                        }
                        else
                        {
                            doc.GetElementsByTagName(xmlParentNode)[0].AppendChild(childnode);


                            //rootnode.InsertBefore()
                        }
                    }

                    fileName = dtxmlData.Rows[i]["Policy_No"].ToString().Trim().Replace(@"/", "").Replace(@"\", "") + ".xml";
                    strFileName = filePath + "\\" + fileName;
                    doc.Save(strFileName);
                    
                }
                if (!Directory.Exists(zipFilePath))
                {
                    // delete the exisiting zip file from the server****
                    File.Delete(zipFilePath);
                }
                // Create New Zip FIle using ZIPFILE
                    ZipFile.CreateFromDirectory(filePath, zipFilePath);
                
                    DownloadFile(zipFilePath);
            }
            catch (Exception ex)
            {
                Response.Redirect("ErrorPage.aspx?vErrorMsg=" + ex.Message, false);
                return;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMainMenu.aspx");
        }

        public bool DownloadFile(string strfilename)
        {
            bool res = false;
            string filePath = Server.MapPath("~/Reports/XML");
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
            //Response.AddHeader("Accept-Ranges", "bytes");
            Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
            Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);

            //Set the ContentType
            //Response.ContentType = "application/octet-stream";
            Response.ContentType = "application/zip";
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

            if (i < maxCount)
            {
                res = false;
            }
            else
            {
                res = true;
            }

            //Close Binary reader and File stream
            _BinaryReader.Close();
            myFile.Close();
            return res;

        }
    }
}