using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Web.Http;
using Antlr.Runtime;
using Vida.BusinessLogic;
using Newtonsoft.Json;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Vida.API
{
    public class PowerCallController : ApiController
    {
        public JObject Get()
        {
            var qs = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string key = qs["key"];
            string storedProc = qs["StoredProc"];


            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string port = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_PORT"];
            if (string.IsNullOrEmpty(ip)) { ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }


            Dictionary<string, string> paramdictionary = new Dictionary<string, string>();
            foreach (var keys in qs)
            {
                string tmpkey = keys.ToString();
                if ((tmpkey != "key") && (tmpkey != "StoredProc"))
                {
                    paramdictionary.Add(tmpkey, qs[tmpkey].ToString());
                }

            }

            

            BusinessClass blBusinessClass = new BusinessClass();
            string data;
            if (blBusinessClass.ValidateKey(key, ip, port))
            {
                data = blBusinessClass.ProcessSQLToJson(storedProc, paramdictionary);
            }
            else
            {

                data = "{\"Athentication\":\"" + "Failed" + "\"}";
            }




            var jsonObject = JObject.Parse(data);
            return jsonObject;

        }
        public JObject Post()
        {
            var qs = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string key = qs["key"];
            string storedProc = qs["StoredProc"];


            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string port = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_PORT"];
            if (string.IsNullOrEmpty(ip)) { ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }


            Dictionary<string, string> paramdictionary = new Dictionary<string, string>();
            foreach (var keys in qs)
            {
                string tmpkey = keys.ToString();
                if ((tmpkey != "key") && (tmpkey != "StoredProc"))
                {
                    paramdictionary.Add(tmpkey, qs[tmpkey].ToString());
                }

            }



            BusinessClass blBusinessClass = new BusinessClass();
            string data;
            if (blBusinessClass.ValidateKey(key, ip, port))
            {
                data = blBusinessClass.ProcessSQLToJson(storedProc, paramdictionary);
            }
            else
            {

                data = "{\"Athentication\":\"" + "Failed" + "\"}";
            }




            var jsonObject = JObject.Parse(data);
            return jsonObject;

        }
    }
}
