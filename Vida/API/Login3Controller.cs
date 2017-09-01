using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Vida.BusinessLogic;

namespace Vida.API
{
    public class Login3Controller : ApiController
    {


        // GET api/login3/5
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

        // POST api/login3
        public void Post([FromBody]string value)
        {
        }

        // PUT api/login3/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/login3/5
        public void Delete(int id)
        {
        }
    }
}
