using Git.Framework.Controller;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.DataTypes;
using Git.Framework.ORM;
using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Storage.Common;
using Git.Storage.Web.Lib.Filter;
using Git.Storage.Entity.Order;
using Git.Storage.Provider.Order;

namespace Git.Storage.Web.Areas.Product.Controllers
{
    public class GoodsController : MasterPage
    {
        /// <summary>
        /// 产品类别管理
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 添加或者编辑产品类别
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            string cateNum = WebUtil.GetQueryStringValue<string>("cateNum", string.Empty);
            ProductCategoryEntity entity = null;
            if (!cateNum.IsEmpty())
            {
                ProductCategoryProvider provider = new ProductCategoryProvider();
                entity = provider.GetSingle(cateNum);
            }
            entity = entity.IsNull() ? new ProductCategoryEntity() : entity;
            ViewBag.Category = entity;
            return View();
        }


        /********************************************产品管理*****************************************************/

        /// <summary>
        /// 产品管理列表页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Index()
        {
            ViewBag.Category = BaseHelper.GetProductCategory(string.Empty);
            return View();
        }

        /// <summary>
        /// 产品编辑或添加页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Detail(string snNum)
        {
            ProductEntity entity = null;
            if (!snNum.IsEmpty())
            {
                ProductProvider provider = new ProductProvider();
                entity = provider.GetProductBySn(snNum);
                ViewBag.Category = BaseHelper.GetProductCategory(entity.CateNum);
                ViewBag.Storage = LocalHelper.GetStorageNumList(entity.StorageNum);
                ViewBag.Local = LocalHelper.GetLocalNumList(entity.StorageNum, entity.DefaultLocal);
                ViewBag.Customer = BaseHelper.GetCustomerList(entity.CusNum);
                ViewBag.Goods = entity;
                ViewBag.Unit = BaseHelper.GetMeasureNameList(entity.UnitNum);
            }
            else
            {
                ViewBag.Goods = new ProductEntity();
                ViewBag.Category = BaseHelper.GetProductCategory(string.Empty);
                ViewBag.Storage = LocalHelper.GetStorageNumList(string.Empty);
                ViewBag.Local = LocalHelper.GetLocalNumList(string.Empty, string.Empty);
                ViewBag.Customer = BaseHelper.GetCustomerList(string.Empty);
                ViewBag.Unit = BaseHelper.GetMeasureNameList(string.Empty);
            }
            return View();

        }
		/// <summary>
        /// 选择产品视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Dialog()
        {
            string Locals = WebUtil.GetQueryStringValue<string>("target", string.Empty);
            ViewBag.Category = BaseHelper.GetProductCategory(string.Empty);
            ViewBag.Locals = Locals;
            return View();
        }
    }
}
