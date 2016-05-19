using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.Log;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Entity.OutStorage;
using Git.Storage.Provider;
using Git.Storage.Provider.OutStorage;
using Git.Framework.Json;
using Git.Storage.Entity.Return;
using Git.Storage.Common;
using Git.Framework.Controller.Mvc;
using Git.Framework.Resource;
using Git.Storage.Provider.Returns;

namespace Git.Storage.Web.Areas.Returns.Controllers
{
    public class ProductController : MasterPage
    {
        /// <summary>
        /// 退货管理 列表界面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 新增退货管理界面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            ViewBag.ReturnType = EnumHelper.GetOptions<EReturnType>(EReturnType.Sell, "请选择出库单类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(EProductType.Goods, "请选择出库产品类型");
            return View();
        }

        /// <summary>
        /// 编辑退货单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Edit()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("OrderNum", string.Empty);
            Bill<ReturnOrderEntity, ReturnDetailEntity> bill = new ReturnOrder();
            ReturnOrderEntity entity = new ReturnOrderEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity.IsNull() ? new ReturnOrderEntity() : entity;
            ViewBag.Entity = entity;
            ViewBag.ReturnType = EnumHelper.GetOptions<EReturnType>(entity.ReturnType, "请选择出库单类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(entity.ProductType, "请选择出库产品类型");
            return View();
        }

        /// <summary>
        /// 查看单据详细
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Detail()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("OrderNum", string.Empty);
            string flag = WebUtil.GetQueryStringValue<string>("flag", string.Empty);
            Bill<ReturnOrderEntity, ReturnDetailEntity> bill = new ReturnOrder();
            ReturnOrderEntity entity = new ReturnOrderEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity.IsNull() ? new ReturnOrderEntity() : entity;
            ViewBag.Entity = entity;
            ViewBag.ReturnType = EnumHelper.GetEnumDesc<EReturnType>(entity.ReturnType);
            ViewBag.Status = EnumHelper.GetEnumDesc<EAudite>(entity.Status);

            ReturnDetailEntity detail = new ReturnDetailEntity();
            detail.OrderNum = orderNum;
            List<ReturnDetailEntity> list = bill.GetOrderDetail(detail);
            list = list.IsNull() ? new List<ReturnDetailEntity>() : list;
            ViewBag.Detail = list;
            ViewBag.Flag = flag;
            return View();
        }
    }
}
