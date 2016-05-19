using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Web.Lib.Filter;
using Git.Storage.Common;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Json;
using Git.Storage.Provider;
using Git.Storage.Entity.OutStorage;
using Git.Framework.Controller;
using Git.Storage.Provider.OutStorage;
using Git.Framework.ORM;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;
using Git.Storage.Entity.Order;
using Git.Storage.Provider.Order;
using Git.Framework.Resource;

namespace Git.Storage.Web.Areas.OutStorage.Controllers
{
    public class ProductController : MasterPage
    {
        /// <summary>
        /// 添加出库单界面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] = null;
            ViewBag.CraterUser = this.LoginUserName;
            ViewBag.OutType = EnumHelper.GetOptions<EOutType>((int)EOutType.Sell, "请选择出库单类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>((int)EProductType.Goods, "请选择出库产品类型");
            return View();
        }

        /// <summary>
        /// 选择出库产品
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddProduct()
        {
            //查询成品管理正式库位
            string storeNum = this.DefaultPStore;
            ViewBag.LocalOptions = LocalHelper.GetLocalNum(storeNum, ELocalType.Normal, string.Empty);
            return View();
        }

        /// <summary>
        /// 出库单详细页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Detail()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            string flag = WebUtil.GetQueryStringValue<string>("flag", string.Empty);
            Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
            OutStorageEntity entity = new OutStorageEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity.IsNull() ? new OutStorageEntity() : entity;
            ViewBag.Entity = entity;
            ViewBag.OutType = EnumHelper.GetEnumDesc<EOutType>(entity.OutType);
            ViewBag.Status = EnumHelper.GetEnumDesc<EAudite>(entity.Status);


            OutStoDetailEntity detail = new OutStoDetailEntity();
            detail.OrderNum = orderNum;
            List<OutStoDetailEntity> listResult = bill.GetOrderDetail(detail);
            listResult = listResult.IsNull() ? new List<OutStoDetailEntity>() : listResult;

            ProductProvider provider = new ProductProvider();
            List<ProductEntity> list = provider.GetListByCache();
            list = list.IsNull() ? new List<ProductEntity>() : list;
            listResult.ForEach(a =>
            {
                ProductEntity product = null;
                if (a.BarCode.IsEmpty())
                {
                    product = list.SingleOrDefault(b => b.SnNum == a.ProductNum);
                }
                else
                {
                    product = list.SingleOrDefault(b => b.SnNum == a.ProductNum && b.BarCode == a.BarCode);
                }
                if (product.IsNotNull())
                {
                    a.Size = product.Size.IsEmpty() ? "" : product.Size;
                }
                else
                {
                    a.Size = "";
                }
            });
            ViewBag.Detail = listResult;
            ViewBag.Flag = flag;
            return View();
        }

        /// <summary>
        /// 打印送货单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Print()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
            OutStorageEntity entity = new OutStorageEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity.IsNull() ? new OutStorageEntity() : entity;
            ViewBag.Entity = entity;

            OutStoDetailEntity detail = new OutStoDetailEntity();
            detail.OrderNum = orderNum;
            List<OutStoDetailEntity> listResult = bill.GetOrderDetail(detail);
            listResult = listResult.IsNull() ? new List<OutStoDetailEntity>() : listResult;
            OrderProvider orderProvider = new OrderProvider();
            foreach (OutStoDetailEntity item in listResult)
            {
                OrderDetailEntity orderDetail = new OrderDetailEntity();
                orderDetail.Where(a => a.SnNum == item.ContractSn).And(a => a.OrderNum == item.ContractOrder);
                orderDetail = orderProvider.GetOrderDetail(orderDetail);
                if (orderDetail != null)
                {
                    item.Qty = orderDetail.Num;
                }
            }
            ViewBag.Detail = listResult;
            return View();
        }

        /// <summary>
        /// 出库单管理列表页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            ViewBag.OutType = EnumHelper.GetOptions<EOutType>(0, "请选择出库单类型");
            ViewBag.ReportNum = ResourceManager.GetSettingEntity("OutOrder_Template").Value;
            return View();
        }

        /// <summary>
        /// 出库单编辑界面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Edit()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
            OutStorageEntity entity = new OutStorageEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity.IsNull() ? new OutStorageEntity() : entity;
            ViewBag.Entity = entity;
            ViewBag.OutType = EnumHelper.GetOptions<EInType>(entity.OutType, "请选择入库单类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(entity.ProductType, "请选择入库产品类型");

            OutStoDetailEntity detail = new OutStoDetailEntity();
            detail.OrderNum = orderNum;
            List<OutStoDetailEntity> listResult = bill.GetOrderDetail(detail);
            listResult = listResult.IsNull() ? new List<OutStoDetailEntity>() : listResult;

            ProductProvider provider = new ProductProvider();
            List<ProductEntity> list = provider.GetListByCache();
            list = list.IsNull() ? new List<ProductEntity>() : list;
            listResult.ForEach(a =>
            {
                ProductEntity product = null;
                if (a.BarCode.IsEmpty())
                {
                    product = list.SingleOrDefault(b => b.SnNum == a.ProductNum);
                }
                else
                {
                    product = list.SingleOrDefault(b => b.SnNum == a.ProductNum && b.BarCode == a.BarCode);
                }
                a.OutPrice = product != null ? product.OutPrice : 0;
                if (product != null)
                {
                    a.Size = product.Size.IsEmpty() ? "" : product.Size;
                }
                else
                {
                    a.Size = "";
                }
            });
            Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] = listResult;
            return View();
        }


        /// <summary>
        /// 根据订单出库
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult OrderDetail()
        {
            return View();
        }
    }
}
