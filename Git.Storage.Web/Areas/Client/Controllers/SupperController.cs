using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Client.Controllers
{
    public class SupperController : MasterPage
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
