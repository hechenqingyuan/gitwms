using Git.Framework.Controller;
using Git.Framework.Controller.Mvc;
using Git.Storage.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Git.Storage.WebAPI.Controllers
{
    public class OrderController : ApiController
    {
        // GET api/order
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/order/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/order
        public void Post([ModelBinder(typeof(JsonBinder<AdminEntity>))] AdminEntity entity)
        {
            string id=WebUtil.GetFormValue<string>("id");
        }

        // PUT api/order/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/order/5
        public void Delete(int id)
        {
        }
    }
}
