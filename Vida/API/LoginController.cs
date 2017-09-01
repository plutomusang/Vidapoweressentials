using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Newtonsoft.Json;
using Vida.BusinessLogic;
using Vida.Models;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;

namespace Vida.API
{
    public class LoginController : ApiController
    {

        public JObject Post()
        {
            BusinessClass bl = new BusinessClass();

            var qs = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string genkey = qs["key"];
            string sjson = "{\"ApiKey\":\"" +  "\"}";


            if (genkey == "1234567890")
            {

                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string port = System.Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_PORT"]);
                if (string.IsNullOrEmpty(ip)) { ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
                sjson = bl.Login(qs["Username"], qs["Password"], ip, port);
            }


            var jsonObject = JObject.Parse(sjson);
            return jsonObject;

        }
        public JObject Get()
        {
            BusinessClass bl = new BusinessClass();

            var qs = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string genkey = qs["key"];
            string sjson = "{\"ApiKey\":\"" + "\"}";


            if (genkey == "1234567890")
            {

                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string port = System.Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_PORT"]);
                if (string.IsNullOrEmpty(ip)) { ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
                sjson = bl.Login(qs["Username"], qs["Password"], ip, port);
            }


            var jsonObject = JObject.Parse(sjson);
            return jsonObject;

        }
    }
    public class Login2Controller : ApiController
    {

        public string Post(List<string> val)
        {
            BusinessClass bl = new BusinessClass();

            string frm = val[0];
            frm = frm.Replace("%40", "@");
            frm = frm.Replace('+', ' ');
            frm = frm.Replace("%2C", ",");
            Hashtable qs = bl.DesrializeItems(frm);

            string genkey = qs["key"].ToString();
            string sjson = "{\"ApiKey\":\"" + "\"}";


            if (genkey == "1234567890")
            {

                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string port = System.Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_PORT"]);
                if (string.IsNullOrEmpty(ip)) { ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
                sjson = bl.Login(qs["UserName"].ToString(), qs["Password"].ToString(), ip, port);
            }

            return sjson;

        }

    }
    public class LogOutController : ApiController
    {


        public JObject Post()
        {
            BusinessClass bl = new BusinessClass();

            var qs = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string genkey = qs["key"];
            string sjson = "{\"ApiKey\":\"" + "\"}";
            string generalKey = ConfigurationManager.ConnectionStrings["GeneralKey"].ConnectionString;

            if (genkey == generalKey)
            {

                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string port = System.Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_PORT"]);
                if (string.IsNullOrEmpty(ip)) { ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
                sjson = bl.Logout(qs["ApiKey"]);
            }


            var jsonObject = JObject.Parse(sjson);
            return jsonObject;

        }
    }
}
