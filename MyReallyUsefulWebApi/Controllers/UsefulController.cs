using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyReallyUsefulWebApi.Controllers
{
    public class UsefulController : ApiController
    {
        // GET api/<controller>
        [Authorize]
        public string Get()
        {
            return "hello world";
        }
    }
}