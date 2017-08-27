using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Vida.API
{
    public class SimpleCallController : ApiController
    {
        public string Get()
        {
            return "data";
        }
    }
}
