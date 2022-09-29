using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Net.Mail;
using System.Net;

namespace ProjectPASS
{
    public class Cls_General_Functions
    {
        public DataTable GetGroupedBy(DataTable dt, string columnNamesInDt, string groupByColumnNames, string typeOfCalculation,string vColumnName)
        {
            //Return its own if the column names are empty
            if (columnNamesInDt == string.Empty || groupByColumnNames == string.Empty)
            {
                return dt;
            }

            //Once the columns are added find the distinct rows and group it bu the numbet
            DataTable _dt = dt.DefaultView.ToTable(true, groupByColumnNames);

            //The column names in data table
            string[] _columnNamesInDt = columnNamesInDt.Split(',');

            for (int i = 0; i < _columnNamesInDt.Length; i = i + 1)
            {
                if (_columnNamesInDt[i] != groupByColumnNames)
                {
                    _dt.Columns.Add(_columnNamesInDt[i]);
                }
            }

            //Gets the collection and send it back
            for (int i = 0; i < _dt.Rows.Count; i = i + 1)
            {
                for (int j = 0; j < _columnNamesInDt.Length; j = j + 1)
                {
                    if (_columnNamesInDt[j] != groupByColumnNames)
                    {
                        _dt.Rows[i][j] = dt.Select(groupByColumnNames + " = '" + _dt.Rows[i][groupByColumnNames].ToString() + "'")[0][vColumnName].ToString();
                    }
                }
            }

            return _dt;
        }


        
        public bool Fn_Check_Rights_For_Page(string vRoleCode, string vPageName)
        {
            Database db = DatabaseFactory.CreateDatabase("cnPASS");
            DataSet dsRights = null;
            DbCommand dbCommand = db.GetSqlStringCommand("Select * from TBL_USER_ROLE_TO_RIGHTS_MAPPING where vRoleCode = '" + vRoleCode + "'" +
            " and nOptionID in (select nOptionID from TBL_DMENU_PASS_TABLE where NAVIGATEURL IS NOT NULL and UPPER(NAVIGATEURL) = UPPER('" + vPageName + "'))");
            dsRights = db.ExecuteDataSet(dbCommand);
            if (dsRights.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        public DateTime FirstDayOfMonthFromDateTime(DateTime dateTime) 
        { 
            return new DateTime(dateTime.Year, dateTime.Month, 1); 
        }

        public DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
        { 
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1); 
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public string fn_Gen_Cert_No(string vDocumentType, string vDocumentContext, ref SqlTransaction Trans, ref SqlConnection Conn)
        {
            string functionReturnValue = null;
            string vCurrentYear = null, vCurrentMonth = null, vTransactionNumberCur = null, vCurrentDay = null;
          
            int vDifferenceInYear = Convert.ToInt32(vDocumentContext.Substring(6, 4)) - 1991;

            if (vDifferenceInYear > 36)
            {
                vDifferenceInYear = vDifferenceInYear % 36;
            }
            if (vDifferenceInYear < 10)
            {
                vCurrentYear = Convert.ToString(vDifferenceInYear).Trim();
            }
            else
            {
                vCurrentYear = Strings.LTrim(Convert.ToString(Strings.Chr(vDifferenceInYear + 55)));
            }

            vCurrentMonth = Strings.LTrim(Convert.ToString(Strings.Chr(Convert.ToInt32(Strings.Mid(vDocumentContext, 4, 2)) + 64)));


            string _UpdateCommand = "UPDATE TBL_TRANSATION_ID_SEQUENCE SET vLastNo=vLastNo WHERE vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' AND cCharacterDay='" + vCurrentDay + "'";
            SqlCommand _Command = new SqlCommand(_UpdateCommand, Conn);
            _Command.Transaction = Trans;
            if (_Command.ExecuteNonQuery() <= 0)
            {
                string lcINSSTR = "INSERT INTO TBL_TRANSATION_ID_SEQUENCE(vTransType,cCharacterYear,cCharacterMonth,cCharacterDay,vLastNo)";
                lcINSSTR = lcINSSTR + " VALUES ('" + vDocumentType + "','" + vCurrentYear + "','" + vCurrentMonth + "','" + vCurrentDay + "','0')";
                _Command = new SqlCommand(lcINSSTR, Conn);
                _Command.Transaction = Trans;
                _Command.ExecuteNonQuery();
            }

            string _SelectCmd = "Select vLastNo from TBL_TRANSATION_ID_SEQUENCE where vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' AND cCharacterDay='" + vCurrentDay + "'";
            SqlDataAdapter Adapter = new SqlDataAdapter();
            _Command = new SqlCommand(_SelectCmd, Conn);
            _Command.Transaction = Trans;
            Adapter.SelectCommand = _Command;
            DataSet dsDocNo = new DataSet();

            Adapter.Fill(dsDocNo);

            if (dsDocNo.Tables[0].Rows.Count > 0)
            {
                vTransactionNumberCur = Convert.ToString(Convert.ToDouble(dsDocNo.Tables[0].Rows[0]["vLastNo"]) + 1);
            }
            else
            {
                vTransactionNumberCur = "1";
            }

            vTransactionNumberCur = vTransactionNumberCur.ToString().PadLeft(6, '0');

            functionReturnValue = vDocumentType + vTransactionNumberCur;

            _UpdateCommand = "UPDATE TBL_TRANSATION_ID_SEQUENCE SET vLastNo='" + Convert.ToDouble(vTransactionNumberCur) + "' WHERE vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' AND cCharacterDay='" + vCurrentDay + "' ";
            try
            {
                _Command = new SqlCommand(_UpdateCommand, Conn);
                _Command.Transaction = Trans;
                _Command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                functionReturnValue = "Error :" + e.Message;
            }
            return functionReturnValue;
        }
        public string fn_Gen_Doc_No(string vCompanyCode,string vPropertyCode, string vDocumentType, string vDocumentContext,ref SqlTransaction Trans,ref SqlConnection Conn)
        {
            string functionReturnValue = null;
            string vCurrentYear = null, vCurrentMonth = null, vTransactionNumberCur = null;
            vPropertyCode = vPropertyCode.Trim();

            int vDifferenceInYear = Convert.ToInt32(vDocumentContext.Substring(6, 4)) - 1991;

            if (vDifferenceInYear > 36)
            {
                vDifferenceInYear = vDifferenceInYear % 36;
            }
            if (vDifferenceInYear < 10)
            {
                vCurrentYear = Convert.ToString(vDifferenceInYear).Trim();
            }
            else
            {
                vCurrentYear = Strings.LTrim(Convert.ToString(Strings.Chr(vDifferenceInYear + 55)));
            }

            vCurrentMonth = Strings.LTrim(Convert.ToString(Strings.Chr(Convert.ToInt32(Strings.Mid(vDocumentContext, 4, 2)) + 64)));

            string _UpdateCommand = "UPDATE TBL_TRANSATION_ID_SEQUENCE SET vLastNo=vLastNo WHERE vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' AND vPropertyCode='" + vPropertyCode + "' ";
            SqlCommand _Command = new SqlCommand(_UpdateCommand, Conn);
            _Command.Transaction = Trans;
            if (_Command.ExecuteNonQuery() <= 0)
            {
                string lcINSSTR = "INSERT INTO TBL_TRANSATION_ID_SEQUENCE(vTransType,vPropertyCode,cCharacterYear,cCharacterMonth,vLastNo)";
                lcINSSTR = lcINSSTR + " VALUES ('" + vDocumentType + "','" + vPropertyCode + "','" + vCurrentYear + "','" + vCurrentMonth + "','0')";
                _Command = new SqlCommand(lcINSSTR, Conn);
                _Command.Transaction = Trans;
                _Command.ExecuteNonQuery();
            }

            string _SelectCmd = "Select vLastNo from TBL_TRANSATION_ID_SEQUENCE where vPropertyCode='" + vPropertyCode + "'  AND vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "'";
            SqlDataAdapter Adapter = new SqlDataAdapter();
            _Command = new SqlCommand(_SelectCmd, Conn);
            _Command.Transaction = Trans;
            Adapter.SelectCommand = _Command;
            DataSet dsDocNo = new DataSet();

            Adapter.Fill(dsDocNo);

            if (dsDocNo.Tables[0].Rows.Count > 0)
            {
                vTransactionNumberCur = Convert.ToString(Convert.ToDouble(dsDocNo.Tables[0].Rows[0]["vLastNo"]) + 1);
            }
            else
            {
                vTransactionNumberCur = "1";
            }

            vTransactionNumberCur = vTransactionNumberCur.ToString().PadLeft(6,'0');

            functionReturnValue = vPropertyCode + vDocumentType + vCurrentYear + vCurrentMonth + vTransactionNumberCur;

            _UpdateCommand = "UPDATE TBL_TRANSATION_ID_SEQUENCE SET vLastNo='" + Convert.ToDouble(vTransactionNumberCur) + "' WHERE vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' AND vPropertyCode='" + vPropertyCode + "' ";
            try
            {
                _Command = new SqlCommand(_UpdateCommand, Conn);
                _Command.Transaction = Trans;
                _Command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                functionReturnValue = "Error :" + e.Message;
            }
            return functionReturnValue;
        }
        
        public string fn_check_first_login(string vEmployeeCode)
        {
            String StrFirstLogin="N";
            String sqlCommand="";
            Database db = DatabaseFactory.CreateDatabase("cnIMS");

            sqlCommand = "SELECT dLastLogin FROM TBL_USER_LOGIN where vEmployeeCode='" + vEmployeeCode + "'";
            DbCommand dbCommandreq = db.GetSqlStringCommand(sqlCommand);
            DataSet dsreq = null;
            dsreq = db.ExecuteDataSet(dbCommandreq);
            if (dsreq.Tables[0].Rows[0]["dLastLogin"].ToString()=="")
            {
                StrFirstLogin = "Y";
            }
            return StrFirstLogin;

        }
        public string fn_Gen_Doc_Master_No(string vDocumentType, string vDocumentContext,ref SqlTransaction Trans,ref SqlConnection Conn)
        {
            string functionReturnValue = null;
            string vCurrentYear = null, vCurrentMonth = null, vTransactionNumberCur = null;

            int vDifferenceInYear = Convert.ToInt32(vDocumentContext.Substring(6, 4)) - 1991;

            if (vDifferenceInYear > 36)
            {
                vDifferenceInYear = vDifferenceInYear % 36;
            }
            if (vDifferenceInYear < 10)
            {
                vCurrentYear = Convert.ToString(vDifferenceInYear).Trim();
            }
            else
            {
                vCurrentYear = Strings.LTrim(Convert.ToString(Strings.Chr(vDifferenceInYear + 55)));
            }

            vCurrentMonth = Strings.LTrim(Convert.ToString(Strings.Chr(Convert.ToInt32(Strings.Mid(vDocumentContext, 4, 2)) + 64)));
            string _UpdateCommand = "";
           
            _UpdateCommand = "UPDATE TBL_TRANSATION_ID_SEQUENCE SET vLastNo=vLastNo WHERE vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' ";
            
            SqlCommand _Command = new SqlCommand(_UpdateCommand, Conn);
            _Command.Transaction = Trans;
            if (_Command.ExecuteNonQuery() <= 0)
            {
                string lcINSSTR = "INSERT INTO TBL_TRANSATION_ID_SEQUENCE(vTransType,cCharacterYear,cCharacterMonth,vLastNo)";
                lcINSSTR = lcINSSTR + " VALUES ('" + vDocumentType + "','" + vCurrentYear + "','" + vCurrentMonth + "','0')";
                _Command = new SqlCommand(lcINSSTR, Conn);
                _Command.Transaction = Trans;
                _Command.ExecuteNonQuery();
            }

            string _SelectCmd = "";
           _SelectCmd = "Select vLastNo from TBL_TRANSATION_ID_SEQUENCE where  vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "'";
            
            SqlDataAdapter Adapter = new SqlDataAdapter();
            _Command = new SqlCommand(_SelectCmd, Conn);
            _Command.Transaction = Trans;
            Adapter.SelectCommand = _Command;
            DataSet dsDocNo = new DataSet();
            Adapter.Fill(dsDocNo);

            if (dsDocNo.Tables[0].Rows.Count > 0)
            {
                vTransactionNumberCur = Convert.ToString(Convert.ToDouble(dsDocNo.Tables[0].Rows[0]["vLastNo"]) + 1);
            }
            else
            {
                vTransactionNumberCur = "1";
            }
           
           functionReturnValue = vDocumentType + vCurrentYear + vCurrentMonth + Convert.ToDouble(vTransactionNumberCur);
           
            
           _UpdateCommand = "UPDATE TBL_TRANSATION_ID_SEQUENCE SET vLastNo='" + vTransactionNumberCur + "' WHERE vTransType='" + vDocumentType + "' AND cCharacterYear='" + vCurrentYear + "' AND cCharacterMonth='" + vCurrentMonth + "' ";
            
            try
            {
                _Command = new SqlCommand(_UpdateCommand, Conn);
                _Command.Transaction = Trans;
                _Command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                functionReturnValue = "Error :" + e.Message;
            }
            return functionReturnValue;
        }
      
        public string Fn_Get_User_Role(string vEmployeeCode)
        {
            Database db = DatabaseFactory.CreateDatabase("cnIMS");
            string sqlCommand = "select * from TBL_USER_EMPLOYEE_TO_ROLE_MAPPING Where vEmployeeCode ='" + vEmployeeCode + "'";
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);
            DataSet ds = null;
            ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["vRoleCode"].ToString();
            }
            else
            {
                return "";
            }
        }
       
        public string Fn_Get_Month_Description(Int32 nMonth,string cYear)
        {
            string vMonthDesc="";
            switch (nMonth)
            {
                case 1:
                    vMonthDesc = "Jan "+cYear;
                    return vMonthDesc;
                case 2:
                    vMonthDesc = "Feb " + cYear;
                    return vMonthDesc;
                case 3:
                    vMonthDesc = "Mar " + cYear;
                    return vMonthDesc;
                case 4:
                    vMonthDesc = "Apr " + cYear;
                    return vMonthDesc;
                case 5:
                    vMonthDesc = "May " + cYear;
                    return vMonthDesc;
                case 6:
                    vMonthDesc = "Jun " + cYear;
                    return vMonthDesc;
                case 7:
                    vMonthDesc = "Jul " + cYear;
                    return vMonthDesc;
                case 8:
                    vMonthDesc = "Aug " + cYear;
                    return vMonthDesc;
                case 9:
                    vMonthDesc = "Sep " + cYear;
                    return vMonthDesc;
                case 10:
                    vMonthDesc = "Oct " + cYear;
                    return vMonthDesc;
                case 11:
                    vMonthDesc = "Nov " + cYear;
                    return vMonthDesc;
                case 12:
                    vMonthDesc = "Dec " + cYear;
                    return vMonthDesc;
            }
            return vMonthDesc;
        }
        public string Fn_Send_Email(string strToAddress, string strMailSubject, string strMailBody)
        {
            var strFromAddress = "bharat.moily@panindiafoods.com";
            const string strFromPassword = "P@ssword123";
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "mail.panindiafoods.com";
                smtp.Port = 25;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(strFromAddress, strFromPassword);
                smtp.Timeout = 20000;
            }
            try
            {
                smtp.Send(strFromAddress, strToAddress, strMailSubject, strMailBody);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;  
            }
        }
    }
}