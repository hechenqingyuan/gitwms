using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Entity.Move;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Provider.Move;
using Git.Storage.Provider;
using Git.Framework.Json;
using Git.Storage.Common;

namespace Git.Storage.Web.Areas.Move.Controllers
{
    public class ProductController : MasterPage
    {
        /// <summary>
        /// 添加移库单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            Session[CacheKey.TEMPDATA_CACHE_MOVERODUCTDETAIL] = null;
            ViewBag.MoveType = EnumHelper.GetOptions<EMoveType>(EMoveType.RackToRack, "请选择移库类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(EProductType.Goods, "请选择入库产品类型");
            return View();
        }

        /// <summary>
        /// 移库单详细
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddProduct()
        {
            return View();
        }

        ///// <summary>
        ///// 移库单详细页面
        ///// </summary>
        ///// <returns></returns>
        [LoginFilter]
        public ActionResult Detail()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            string flag = WebUtil.GetQueryStringValue<string>("flag", string.Empty);
            Bill<MoveOrderEntity, MoveOrderDetailEntity> bill = new MoveOrder();
            MoveOrderEntity entity = new MoveOrderEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity.IsNull() ? new MoveOrderEntity() : entity;
            ViewBag.BadReport = entity;
            ViewBag.Status = EnumHelper.GetEnumDesc<EAudite>(entity.Status);

            MoveOrderDetailEntity detail = new MoveOrderDetailEntity();
            detail.OrderNum = orderNum;
            List<MoveOrderDetailEntity> listResult = bill.GetOrderDetail(detail);
            listResult = listResult.IsNull() ? new List<MoveOrderDetailEntity>() : listResult;
            ViewBag.Detail = listResult;
            ViewBag.Flag = flag;
            return View();
        }

        /// <summary>
        /// 编辑移库单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Edit()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            if (orderNum.IsEmpty())
            {
                return Redirect("/Move/Product/List");
            }
            Bill<MoveOrderEntity, MoveOrderDetailEntity> bill = new MoveOrder();
            MoveOrderEntity entity = new MoveOrderEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            if (entity.IsNull())
            {
                return Redirect("/Move/Product/List");
            }
            ViewBag.Entity = entity;
            ViewBag.Status = EnumHelper.GetEnumDesc<EAudite>(entity.Status);
            ViewBag.MoveType = EnumHelper.GetOptions<EMoveType>(entity.MoveType, "请选择移库类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(entity.ProductType, "请选择入库产品类型");

            MoveOrderDetailEntity detail = new MoveOrderDetailEntity();
            detail.OrderNum = orderNum;
            List<MoveOrderDetailEntity> listResult = bill.GetOrderDetail(detail);
            Session[CacheKey.TEMPDATA_CACHE_MOVERODUCTDETAIL] = listResult;
            return View();
        }

        /// <summary>
        /// 移库管理分页
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            return View();
        }

    }
}
