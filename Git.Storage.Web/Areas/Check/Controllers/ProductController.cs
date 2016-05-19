using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Common;
using Git.Storage.Entity.Check;
using Git.Storage.Entity.Store;
using Git.Storage.Provider;
using Git.Storage.Provider.Check;
using Git.Storage.Provider.Store;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Check.Controllers
{
    public class ProductController : MasterPage
    {
        /// <summary>
        /// 新增盘点单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            string checkType = EnumHelper.GetOptions<ECheckType>(ECheckType.Product);
            ViewBag.CheckType = checkType;
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(EProductType.Goods);
            Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] = null;
            return View();
        }

        /// <summary>
        /// 判断新增数据
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddProduct()
        {
            return View();
        }

        /// <summary>
        /// 盘点单管理列表界面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 详细盘差表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Detail()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("OrderNum", string.Empty);
            Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
            CheckStockEntity entity = new CheckStockEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            if (entity.IsNull())
            {
                return Redirect("/Check/Product/List");
            }
            entity.CheckTypeLable = EnumHelper.GetEnumDesc<ECheckType>(entity.Type);
            entity.ProductTypeLable = EnumHelper.GetEnumDesc<EProductType>(entity.ProductType);
            entity.StatusLable = EnumHelper.GetEnumDesc<EAudite>(entity.Status);
            ViewBag.Entity = entity;
            return View();
        }

        /// <summary>
        /// 复核盘点单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Upload()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("OrderNum", string.Empty);
            Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
            CheckStockEntity entity = new CheckStockEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            if (entity.IsNull())
            {
                return Redirect("/Check/Product/List");
            }
            entity.CheckTypeLable = EnumHelper.GetEnumDesc<ECheckType>(entity.Type);
            entity.ProductTypeLable = EnumHelper.GetEnumDesc<EProductType>(entity.ProductType);
            entity.StatusLable = EnumHelper.GetEnumDesc<EAudite>(entity.Status);
            ViewBag.Entity = entity;
            return View();
        }

        /// <summary>
        /// 盘点审核
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Audite()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("OrderNum", string.Empty);
            Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
            CheckStockEntity entity = new CheckStockEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            if (entity.IsNull())
            {
                return Redirect("/Check/Product/List");
            }
            entity.CheckTypeLable = EnumHelper.GetEnumDesc<ECheckType>(entity.Type);
            entity.ProductTypeLable = EnumHelper.GetEnumDesc<EProductType>(entity.ProductType);
            entity.StatusLable = EnumHelper.GetEnumDesc<EAudite>(entity.Status);
            ViewBag.Entity = entity;
            return View();
        }

        /// <summary>
        /// 编辑盘点单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Edit()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("OrderNum", string.Empty);
            Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
            CheckStockEntity entity = new CheckStockEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            if (entity.IsNull())
            {
                return Redirect("/Check/Product/List");
            }
            string checkType = EnumHelper.GetOptions<ECheckType>(entity.Type);
            ViewBag.CheckType = checkType;
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(entity.ProductType);
            ViewBag.Entity = entity;
            CheckStockInfoEntity info = new CheckStockInfoEntity();
            info.OrderNum = orderNum;
            List<CheckStockInfoEntity> list = bill.GetOrderDetail(info);

            List<ProductEntity> ListProducts = new List<ProductEntity>();
            List<ProductEntity> ListSource = new ProductProvider().GetListByCache();
            if (!list.IsNullOrEmpty())
            {
                Parallel.ForEach(list, item => 
                {
                    if (ListSource.Exists(a => a.SnNum == item.TargetNum))
                    {
                        ProductEntity target = ListSource.FirstOrDefault(a => a.SnNum == item.TargetNum);
                        ListProducts.Add(target);
                    }
                });
            }
            Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] = ListProducts;
            return View();
        }
    }
}