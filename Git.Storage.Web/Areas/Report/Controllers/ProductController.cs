using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Report.Controllers
{
    public class ProductController : MasterPage
    {
        /// <summary>
        /// 库存清单(报表)
        /// </summary>
        /// <returns></returns>
        public ActionResult StockBill()
        {
            ViewBag.LocalTypeList = EnumHelper.GetOptions<ELocalType>(string.Empty, "请选择库位类型");
            return View();
        }

    }
}
