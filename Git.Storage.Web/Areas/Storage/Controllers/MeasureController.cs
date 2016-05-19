using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Storage.Controllers
{
    public class MeasureController : MasterPage
    {
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }

        [LoginFilter]
        public ActionResult AddMeasure()
        {
            return View();
        }
    }
}
