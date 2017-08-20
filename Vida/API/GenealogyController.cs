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
    public class GenealogyController : ApiController
    {
        public JObject Get()
        {
            var qs = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string key = qs["key"];
            string storedProc = qs["StoredProc"];

            Dictionary<string, string> paramdictionary = new Dictionary<string, string>();

            foreach (var keys in  qs)
            {
                string tmpkey = keys.ToString();
                if ((tmpkey != "key") && (tmpkey != "StoredProc"))
                {
                    paramdictionary.Add(tmpkey, qs[tmpkey].ToString());
                }
                
            }

            BusinessClass blBusinessClass =  new BusinessClass();
            string data = blBusinessClass.ProcessSQLToJson(storedProc, paramdictionary);

            var jsonObject = JObject.Parse(data);
            return jsonObject;

        }
        public JObject Post()
        {
            //Response.Headers.Add("Content-type", "application/json");
            var qs = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string key = qs["key"];
            string storedProc = qs["StoredProc"];

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
            string data = blBusinessClass.ProcessSQLToJson(storedProc, paramdictionary);

            var jsonObject = JObject.Parse(data);
            return jsonObject;

        }

    }
}
