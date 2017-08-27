using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Vida.Models;

namespace Vida.BusinessLogic
{
    public class BusinessClass
    {
        public Hashtable DesrializeItems(string items)
        {
            string[] sep = new string[] { "&" };
            string[] result = items.Split(sep, StringSplitOptions.None);

            Hashtable hs = new Hashtable();
            foreach (string src in result)
            {
                string[] sp = new string[] { "=" };
                string[] rs = src.Split(sp, StringSplitOptions.None);
                if (rs.Length == 1)
                {
                    hs.Add(rs[0], "");
                }
                else
                {
                    if (rs[1] != "<?xml version") { rs[1] = rs[1].Replace('+', ' '); }
                    else
                    {
                        string s = src.Replace(rs[0] + "=", "");
                        try
                        {
                            s = src.Substring(rs[0].Length + 1, src.Length - (rs[0].Length + 1));
                        }
                        catch (Exception ex)
                        {
                            s = src.Replace(rs[0] + "=", "");
                        }
                        rs[1] = s;
                    }


                    hs.Add(rs[0], rs[1]);
                }
            }
            return hs;
        }
        public string ProcessSQLToJson(string storedProc, Dictionary<string, string> mParam)
        {
            string message = "";

            MyDataAccess da = MyDataAccess.Instance;

            List<SqlParameter> sqlparamsList = new List<SqlParameter>();

            //Convert dictionary values into SqlParamerter List
            foreach (KeyValuePair<string, string> entry in mParam)
            {
                SqlParameter sqlParameter = SqlHelper.Param(entry.Key, entry.Value);
                sqlparamsList.Add(sqlParameter);
            }
            


            try
            {

                message = da.SqlReturnToJson(storedProc, sqlparamsList);

            }
            catch (Exception ex)
            {
                message = "{\"error\":\"" + ex.Message + "\"}";
            }



            return message;
        }
        public string Login(string username, string password, string RemoteIP, string RemotePort)
        {
            BusinessClass blBusinessClass = new BusinessClass();
            Dictionary<string, string> paramdictionary = new Dictionary<string, string>();

            paramdictionary.Add("UserName", username);
            paramdictionary.Add("Password", password);
            paramdictionary.Add("ApiIp", RemoteIP);
            paramdictionary.Add("ApiPort", RemotePort);

            
            string message = "";


            try
            {

                message = blBusinessClass.ProcessSQLToJson("ValidateUser", paramdictionary);

            }
            catch (Exception ex)
            {
                message = "{\"error\":\"" + ex.Message + "\"}";
            }


            return message;
        }

        public string  Logout(string key)
        {
            BusinessClass blBusinessClass = new BusinessClass();
            Dictionary<string, string> paramdictionary = new Dictionary<string, string>();

            paramdictionary.Add("ApiKey", key);


            string message = "";


            try
            {

                message = blBusinessClass.ProcessSQLToJson("sp_Logout", paramdictionary);

            }
            catch (Exception ex)
            {
                message = "{\"error\":\"" + ex.Message + "\"}";
            }


            return message;
        }
        public bool ValidateKey(string key, string RemoteIP, string RemotePort)
        {
            BusinessClass blBusinessClass = new BusinessClass();
            Dictionary<string, string> paramdictionary = new Dictionary<string, string>();

            paramdictionary.Add("ApiKey", key);
            paramdictionary.Add("ApiIp", RemoteIP);
            paramdictionary.Add("ApiPort", RemotePort);


            string message = "";


            try
            {

                message = blBusinessClass.ProcessSQLToJson("ValidateKey", paramdictionary);

            }
            catch (Exception ex)
            {
                message = "{\"error\":\"" + ex.Message + "\"}";
            }
            bool retMsg = true;

            if (message.IndexOf("Fail") > -1) retMsg = false;
            if (message.IndexOf("error") > -1) retMsg = false;

            return retMsg;
        }
    }

    public sealed class MyDataAccess
    {
        #region "Constructor"
        private static readonly MyDataAccess instance = new MyDataAccess();

        private MyDataAccess()
        {
            try
            {
                //sqlconn.ConnectionString = ConnectionString;
                //sqlconn.Open();

                string connString = ConnectionString;
                sqlconn = new SqlConnection(connString);
                sqlconn.Open();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

        }

        public static MyDataAccess Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        private string _ConnectionString = "Data Source=palm.arvixe.com;Initial Catalog=_vida;Persist Security Info=True;User ID=arthuruser;Password=mercury3356";
        //private string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["HSCConnectionstring"].ConnectionString;
        SqlConnection sqlconn = new SqlConnection();
        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }
        public SqlConnection SqlConn
        {
            get { return sqlconn; }
        }
        public SqlDataReader GetDataReader(string _storedprocedure, params SqlParameter[] sqlparams)
        {
            SqlDataReader sqlreader = null;
            if (sqlconn.State == ConnectionState.Open)
            {
                SqlCommand sqlcomm = new SqlCommand();
                int i;
                try
                {

                    sqlcomm.Connection = sqlconn;
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.CommandText = _storedprocedure;

                    for (i = 0; i < sqlparams.Length; i++)
                    {
                        sqlcomm.Parameters.Add(sqlparams[i]);

                        if (sqlparams[i].Direction == ParameterDirection.Output)
                        {
                            sqlparams[i].Direction = ParameterDirection.Output;
                        }

                    }


                    sqlreader = sqlcomm.ExecuteReader();

                }
                catch (Exception ex)
                {
                    Reconnect(ex.Message);
                    throw ex;

                }
                finally
                {
                    sqlcomm = null;

                }

            }
            else
            {
                if (sqlconn.State == ConnectionState.Closed) Reconnect("");
                if (sqlconn.State == ConnectionState.Broken) Reconnect("");
                throw new System.ArgumentException("Database Error in Connection");
            }


            return sqlreader;
        }

        public int SqlExecuteNonQuery(string _storeprocedure, params SqlParameter[] myparams)
        {
            int ret = 0;
            if (sqlconn.State == ConnectionState.Open)
            {
                SqlCommand sqlcomm = new SqlCommand();

                try
                {
                    sqlcomm.Connection = sqlconn;
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.CommandText = _storeprocedure;

                    for (int i = 0; i < myparams.Length; i++)
                    {
                        sqlcomm.Parameters.Add(myparams[i]);

                        if (myparams[i].Direction == ParameterDirection.Output)
                        {
                            myparams[i].Direction = ParameterDirection.Output;
                        }

                    }

                    return sqlcomm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Reconnect(ex.Message);
                    throw ex;
                }
                finally
                {
                    sqlcomm = null;
                }

            }
            else
            {
                if (sqlconn.State == ConnectionState.Closed) Reconnect("");
                if (sqlconn.State == ConnectionState.Broken) Reconnect("");
            }

            return ret;

        }
        public object SqlExecuteScalar(string _storeprocedure, params SqlParameter[] myparams)
        {
            object obj = 0;
            if (sqlconn.State == ConnectionState.Open)
            {

                SqlCommand sqlcomm = new SqlCommand();

                try
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();

                    sqlcomm.Connection = sqlconn;
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.CommandText = _storeprocedure;
                    for (int i = 0; i < myparams.Length; i++)
                    {
                        sqlcomm.Parameters.Add(myparams[i]);

                        if (myparams[i].Direction == ParameterDirection.Output)
                        {
                            myparams[i].Direction = ParameterDirection.Output;
                        }

                    }
                    return sqlcomm.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Reconnect(ex.Message);
                    throw ex;
                }
                finally
                {
                    sqlcomm = null;
                }
            }
            else
            {
                if (sqlconn.State == ConnectionState.Closed) Reconnect("");
                if (sqlconn.State == ConnectionState.Broken) Reconnect("");
                return obj;
            }


        }

        //Convert SQL returns to Jason Formated Object
        public string SqlReturnToJson(string storeprocedure,  List<SqlParameter> myparams)
        {
            SqlDataReader sqlreader = null;

            string sjson = "";
            string coma = "";

            object obj = 0;
            if (sqlconn.State == ConnectionState.Open)
            {

                SqlCommand sqlcomm = new SqlCommand();

                try
                {
                    if (sqlconn.State == ConnectionState.Closed)
                        sqlconn.Open();

                    sqlcomm.Connection = sqlconn;
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.CommandText = storeprocedure;

                    for (int i = 0; i < myparams.Count; i++)
                    {
                        sqlcomm.Parameters.Add(myparams[i]);

                        if (myparams[i].Direction == ParameterDirection.Output)
                        {
                            myparams[i].Direction = ParameterDirection.Output;
                        }
                    }

                    sqlreader = sqlcomm.ExecuteReader();

                    while (sqlreader.Read() == true)
                    {
                        List<Dictionary<string, string>> cmdLD = new List<Dictionary<string, string>>();
                        string srow = "";
                        string fldcoma = "";
                        for (int col = 0; col < sqlreader.FieldCount; col++)
                        {
                            string fn = sqlreader.GetName(col).ToString();
                            string vn = sqlreader[fn].ToString();

                            srow = srow + fldcoma + "\"" + fn + "\":\"" + vn + "\"";
                            fldcoma = ",";
                        }
                        sjson = sjson + coma + "{" + srow + "}" ;
                        coma = ",";
                    }
                    sqlreader.Close();
                    sjson = "{\"" + storeprocedure + "\":[" + sjson + "]}";

                }
                catch (Exception ex)
                {
                    Reconnect(ex.Message);
                    sjson = "{\"error\":\""+ ex.Message +"\"}";
                    throw ex;
                }
                finally
                {
                    sqlcomm = null;
                    //sjson = "{\"empty\":\"empty\"}";
                }
            }
            else
            {
                string constate = "cannot connect ";
                if (sqlconn.State == ConnectionState.Closed) {constate = constate + "Closed";}
                if (sqlconn.State == ConnectionState.Broken) { constate = constate + "Broken"; }
                if (sqlconn.State == ConnectionState.Connecting) { constate = constate + "Connecting"; }
                if (sqlconn.State == ConnectionState.Executing) { constate = constate + "Executing"; }
                if (sqlconn.State == ConnectionState.Fetching) { constate = constate + "Fetching"; }


                sjson = "{\"Connection Failed\":\"" + constate + "\"}";
            }

            return sjson;
        }
        private void Reconnect(string ex)
        {
            //sqlconn.Close();
            sqlconn = null;
            string connString = ConnectionString;
            try
            {
                sqlconn = new SqlConnection(connString);
                sqlconn.Open();
            }
            catch
            {

            }

        }
    }

    public class SqlHelper
    {
        public static MyDataAccess da = MyDataAccess.Instance;
        public static SqlParameter Param(string _name, object _value)
        {
            return new SqlParameter(_name, _value);
        }
        public static SqlParameter Param(string _name, object _value, bool isoutput)
        {
            return new SqlParameter(_name, SqlDbType.Int, 0, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Current, _value);
        }
    }

    public class GenealogyObjects
    {
        public string GenName;
        public string PackageType;
        public string Position;
        public string DirectUpline;
        public int Memberid;
        public int Genealogyid;
    }
}