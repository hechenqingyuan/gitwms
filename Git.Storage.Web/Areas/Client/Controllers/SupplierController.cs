using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.Controller;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Client;
using Git.Storage.Web.Lib.Filter;
using Git.Framework.DataTypes;
using Git.Storage.Common;


namespace Git.Storage.Web.Areas.Client.Controllers
{
    public class SupplierController : MasterPage
    {
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddSupplier()
        {
            string SupNum = WebUtil.GetQueryStringValue<string>("SupNum");
            if (SupNum.IsEmpty())
            {
                ViewBag.Supplier = new SupplierEntity();
                ViewBag.SupType = EnumHelper.GetOptions<ESupType>((int)ESupType.Invented, "请选择供应商类型");
                return View();
            }
            else
            {
                SupplierProvider provider = new SupplierProvider();
                SupplierEntity entity = provider.GetSupplier(SupNum);
                entity = entity == null ? new SupplierEntity() : entity;
                ViewBag.SupType = EnumHelper.GetOptions<ESupType>(entity.SupType,"请选择供应商类型");
                ViewBag.Supplier = entity;
                return View();
            }
        }

        /// <summary>
        /// 选择供应商
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Dialog()
        {
            return View();
        }
    }
}
