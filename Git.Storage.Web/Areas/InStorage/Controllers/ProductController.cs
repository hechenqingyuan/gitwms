using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Framework.Resource;
using Git.Storage.Common;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Provider;
using Git.Storage.Provider.InStorage;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Entity.InStorage;

namespace Git.Storage.Web.Areas.InStorage.Controllers
{
    public class ProductController : MasterPage
    {
        /// <summary>
        /// 入库单列表管理
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            ViewBag.InType = EnumHelper.GetOptions<EInType>(0, "请选择入库单类型");
            ViewBag.ReportNum = ResourceManager.GetSettingEntity("InOrder_Template").Value;
            return View();
        }

        /// <summary>
        /// 新增入库单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            ViewBag.InType = EnumHelper.GetOptions<EInType>((int)EInType.Produce, "请选择入库单类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>((int)EProductType.Goods, "请选择入库产品类型");
            Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] = null;
            return View();
        }

        /// <summary>
        /// 入库添加产品
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddProduct()
        {
            //查询成品管理正式库位
            string storeNum = this.DefaultStore;
            ViewBag.LocalOptions = LocalHelper.GetLocalNum(storeNum,ELocalType.Normal,string.Empty);
            return View();
        }

        /// <summary>
        /// 查看订单详细
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Detail()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum",string.Empty);
            string flag = WebUtil.GetQueryStringValue<string>("flag", "1");
            Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
            InStorageEntity entity = new InStorageEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity=entity==null ? new InStorageEntity(): entity;
            entity.InTypeLable = EnumHelper.GetEnumDesc<EInType>(entity.InType);
            entity.StatusLable = EnumHelper.GetEnumDesc<EAudite>(entity.Status);
            ViewBag.InStorage=entity;
            ViewBag.Flag = flag;

            InStorDetailEntity detail=new InStorDetailEntity();
            detail.OrderNum=orderNum;
            List<InStorDetailEntity> list=bill.GetOrderDetail(detail);
            list = list == null ? new List<InStorDetailEntity>() : list;
            ViewBag.Detail = list;
            return View();
        }


        /// <summary>
        /// 编辑入库单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Edit()
        {
            Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] = null;
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
            InStorageEntity entity = new InStorageEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity == null ? new InStorageEntity() : entity;
            entity.InTypeLable = EnumHelper.GetEnumDesc<EInType>(entity.InType);
            entity.StatusLable = EnumHelper.GetEnumDesc<EAudite>(entity.Status);
            ViewBag.InStorage = entity;
            ViewBag.InType = EnumHelper.GetOptions<EInType>(entity.InType, "请选择入库单类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(entity.Status, "请选择入库产品类型");

            InStorDetailEntity detail = new InStorDetailEntity();
            detail.OrderNum = orderNum;
            List<InStorDetailEntity> list = bill.GetOrderDetail(detail);
            list = list == null ? new List<InStorDetailEntity>() : list;
            if (list.IsNullOrEmpty() == false)
            {
                foreach (InStorDetailEntity item in list)
                {
                    item.Size = item.Size.IsEmpty() ? "" : item.Size;
                    item.Amount = item.Amount == 0 ? item.InPrice * item.Num : item.Amount;
                    item.TotalPrice = item.Amount;
                }
                Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] = list;
            }
            ViewBag.Detail = list;
            return View();
        }

    }
}
