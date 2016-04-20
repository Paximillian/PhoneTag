using MongoDB.Driver;
using PhoneTag.SharedCodebase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PhoneTag.WebServices.Controllers
{
    public class ServiceController : ApiController
    {
        // GET api/values
        public bool Get()
        {
            return Mongo.IsReady;
        }
    }
}