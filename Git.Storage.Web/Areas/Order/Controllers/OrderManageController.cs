using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Storage.Entity.Order;
using Git.Storage.Provider;
using Git.Storage.Provider.Order;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;

namespace Git.Storage.Web.Areas.Order.Controllers
{
    public class OrderManageController : MasterPage
    {
        /// <summary>
        /// 订单管理页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Index()
        {
            ViewBag.OrderType = EnumHelper.GetOptions<EOrderType>(string.Empty, "请选择订单类型");
            return View();
        }

        /// <summary>
        /// 订单管理添加页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            ViewBag.CraterUser = this.LoginUserName;
            Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] = null;
            ViewBag.OrderType = EnumHelper.GetOptions<EOrderType>(string.Empty, "请选择订单类型");
            return View();
        }

        /// <summary>
        /// 选择订单产品
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddProduct()
        {
            ViewBag.UnitNameOptions = BaseHelper.GetMeasureNameList(string.Empty);
            return View();
        }

        /// <summary>
        /// 查看订单详细
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Detail()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            string flag = WebUtil.GetQueryStringValue<string>("flag", "1");
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            OrdersEntity entity = new OrdersEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity == null ? new OrdersEntity() : entity;
            entity.OrderTypeLable = EnumHelper.GetEnumDesc<EOrderStatus>(entity.Status);
            entity.StatusLable = EnumHelper.GetEnumDesc<EAudite>(entity.AuditeStatus);
            ViewBag.Orders = entity;
            ViewBag.Flag = flag;

            OrderDetailEntity detail = new OrderDetailEntity();
            detail.OrderNum = orderNum;
            List<OrderDetailEntity> list = bill.GetOrderDetail(detail);
            list = list == null ? new List<OrderDetailEntity>() : list;
            ViewBag.Detail = list;
            return View();
        }

        /// <summary>
        /// 订单导入
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult ImportFileList()
        {
            return View();
        }

        /// <summary>
        /// 编辑订单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Edit()
        {
            Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] = null;
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            OrdersEntity entity = new OrdersEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity == null ? new OrdersEntity() : entity;
            ViewBag.Orders = entity;
            ViewBag.OrderType = EnumHelper.GetOptions<EOrderType>(entity.OrderType, "请选择订单类型");

            OrderDetailEntity detail = new OrderDetailEntity();
            detail.OrderNum = orderNum;
            List<OrderDetailEntity> list = bill.GetOrderDetail(detail);
            list = list == null ? new List<OrderDetailEntity>() : list;
            if (list.IsNullOrEmpty() == false)
            {
                foreach (OrderDetailEntity item in list)
                {
                    item.Amount = item.Amount == 0 ? item.Price * item.Num : item.Amount;
                }
                Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] = list;
            }
            ViewBag.Detail = list;
            return View();
        }

        /// <summary>
        /// 编辑导入订单信息
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult EditImportFile()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            List<OrdersEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_ORDERIMPORT] as List<OrdersEntity>;
            listResult = listResult == null ? new List<OrdersEntity>() : listResult;
            OrdersEntity entity = listResult.Where(a => a.OrderNum == orderNum).FirstOrDefault();
            ViewBag.Orders = entity;
            ViewBag.OrderType = EnumHelper.GetOptions<EOrderType>(entity.OrderType, "请选择订单类型");
            List<OrderDetailEntity> list = entity.ListDetail;
            list = list == null ? new List<OrderDetailEntity>() : list;
            Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] = list;
            foreach (OrderDetailEntity item in list)
            {
                item.Size = item.Size.IsEmpty() ? "" : item.Size;
            }
            ViewBag.Detail = list;
            return View();
        }

      
    }
}
