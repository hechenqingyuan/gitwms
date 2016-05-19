using Git.Storage.Entity.Order;
using Git.Storage.Provider;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Json;
using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Storage.Provider.Order;
using Git.Framework.Controller.Mvc;
using Git.Framework.ORM;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Order.Controllers
{
    public class OrderAjaxController : AjaxPage
    {
        /// <summary>
        /// 新建入库单
        /// 返回值说明:
        /// 1001 : 请选择要入库的产品以及数量
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Create()
        {
            int OrderType = WebUtil.GetFormValue<int>("OrderType", 0);
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            string ContactName = WebUtil.GetFormValue<string>("ContactName", string.Empty);
            string CusNum = WebUtil.GetFormValue<string>("CusNum", string.Empty);
            string CusName = WebUtil.GetFormValue<string>("CusName", string.Empty);
            string Address = WebUtil.GetFormValue<string>("Address", string.Empty);
            string Phone = WebUtil.GetFormValue<string>("Phone", string.Empty);
            string CrateUser = WebUtil.GetFormValue<string>("CrateUser", string.Empty);
            DateTime OrderTime = WebUtil.GetFormValue<DateTime>("OrderTime", DateTime.Now);
            DateTime SendDate = WebUtil.GetFormValue<DateTime>("SendDate", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);

            OrdersEntity entity = new OrdersEntity();
            entity.SnNum = SequenceProvider.GetSequence(typeof(OrdersEntity));
            entity.OrderNum = OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(OrdersEntity)) : OrderNum;
            entity.OrderType = OrderType;
            entity.CusNum = CusNum;
            entity.CusName = CusName;
            entity.Contact = ContactName;
            entity.Phone = Phone;
            entity.Address = Address;
            entity.ContractOrder = ContractOrder;
            entity.OrderTime = OrderTime;
            entity.SendDate = SendDate;
            entity.CreateTime = DateTime.Now;
            entity.AuditeStatus = (int)EAudite.Wait;
            entity.Status = (int)EOrderStatus.OrderConfirm;
            entity.CreateUser = this.LoginUser.UserCode;
            entity.Remark = Remark;

            List<OrderDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] as List<OrderDetailEntity>;
            if (list.IsNullOrEmpty())
            {
                this.ReturnJson.AddProperty("Key", "1001");
                this.ReturnJson.AddProperty("Value", "请选择要订单的产品以及数量");
                return Content(this.ReturnJson.ToString());
            }
            list.ForEach(a => { a.OrderSnNum = entity.SnNum; a.OrderNum = entity.OrderNum; });
            entity.Num = list.Sum(a => a.Num);
            entity.Amount = list.Sum(a => a.Amount);
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            string returnValue = bill.Create(entity, list);
            if (returnValue == EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success))
            {
                Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] = null;
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "订单创建成功");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量生成订单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult CreateBat([ModelBinder(typeof(JsonBinder<List<string>>))] List<string> List)
        {
            List<OrdersEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_ORDERIMPORT] as List<OrdersEntity>;
            listResult = listResult == null ? new List<OrdersEntity>() : listResult;
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            foreach (string orderNum in List)
            {
                OrdersEntity entity = listResult.FirstOrDefault(a => a.OrderNum == orderNum);
                if (entity != null && !entity.ListDetail.IsNullOrEmpty())
                {
                    entity.Status = (int)EOrderStatus.OrderConfirm;
                    string returnValue = bill.Create(entity, entity.ListDetail);
                    this.ReturnJson.AddProperty("Key", "1000");
                    this.ReturnJson.AddProperty("Value", "订单创建成功");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 编辑出库单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Edit()
        {
            int orderStatus = WebUtil.GetFormValue<int>("OrderStatus", 0);
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            string ContactName = WebUtil.GetFormValue<string>("ContactName", string.Empty);
            string CusNum = WebUtil.GetFormValue<string>("CusNum", string.Empty);
            string CusName = WebUtil.GetFormValue<string>("CusName", string.Empty);
            string Address = WebUtil.GetFormValue<string>("Address", string.Empty);
            string Phone = WebUtil.GetFormValue<string>("Phone", string.Empty);
            string CrateUser = WebUtil.GetFormValue<string>("CrateUser", string.Empty);
            DateTime OrderTime = WebUtil.GetFormValue<DateTime>("OrderTime", DateTime.Now);
            DateTime SendDate = WebUtil.GetFormValue<DateTime>("SendDate", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);

            OrdersEntity entity = new OrdersEntity();
            entity.OrderNum = OrderNum;
            entity.Status = orderStatus;
            entity.CusNum = CusNum;
            entity.CusName = CusName;
            entity.Contact = ContactName;
            entity.Phone = Phone;
            entity.Address = Address;
            entity.ContractOrder = ContractOrder;
            entity.OrderTime = OrderTime;
            entity.SendDate = SendDate;
            entity.CreateTime = DateTime.Now;
            entity.AuditeStatus = (int)EAudite.Wait;
            entity.CreateUser = CrateUser;
            entity.Remark = Remark;

            List<OrderDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] as List<OrderDetailEntity>;
            if (list.IsNullOrEmpty())
            {
                this.ReturnJson.AddProperty("Key", "1001");
                this.ReturnJson.AddProperty("Value", "入库单必须包含入库产品,请重新核对");
                return Content(this.ReturnJson.ToString());
            }
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            string returnValue = bill.EditOrder(entity, list);
            if (returnValue == EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success))
            {
                Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] = null;
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "入库单编辑成功");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 审核(排产)
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Audite()
        {
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            
            return Content(this.ReturnJson.ToString());
        }


        /// <summary>
        /// 订单新增产品
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddProduct()
        {
            string ProductNum = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            string BarCode = WebUtil.GetFormValue<string>("BarCode", string.Empty);
            string UnitNum = WebUtil.GetFormValue<string>("UnitNum", string.Empty);
            string ProductName = WebUtil.GetFormValue<string>("ProductName", string.Empty);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);
            string UnitName = WebUtil.GetFormValue<string>("UnitName", string.Empty);
            string Size = WebUtil.GetFormValue<string>("Size", string.Empty);
            int Num = WebUtil.GetFormValue<int>("Num", 0);
            double Price = WebUtil.GetFormValue<double>("Price", 0);

            OrderDetailEntity entity = new OrderDetailEntity();
            entity.OrderSnNum = SequenceProvider.GetSequence(typeof(OrderDetailEntity));
            entity.SnNum = SequenceProvider.GetSequence(typeof(OrderDetailEntity));
            entity.OrderNum = string.Empty;
            entity.ProductName = ProductName;
            entity.BarCode = BarCode;
            entity.ProductNum = ProductNum;
            entity.Num = Num;
            entity.Price = Price;
            entity.RealNum = 0;
            entity.UnitNum = UnitNum;
            entity.ContractID = string.Empty;
            entity.CreateTime = DateTime.Now;
            entity.Amount = entity.Price * entity.Num;
            entity.Size = Size;
            entity.UnitName = UnitName;
            entity.Remark = Remark;
            List<OrderDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] as List<OrderDetailEntity>;
            list = list.IsNull() ? new List<OrderDetailEntity>() : list;
            list.Add(entity);
            Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] = list;

            return Content(string.Empty);
        }

        /// <summary>
        /// 订单加载产品
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult LoadProduct()
        {
            List<OrderDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] as List<OrderDetailEntity>;
            list = list.IsNull() ? new List<OrderDetailEntity>() : list;
            this.ReturnJson.AddProperty("List", ConvertJson.ListToJson<OrderDetailEntity>(list, "Data"));
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除订单单详细，删除缓存中的内容
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DelDetail()
        {
            string snNum = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            List<OrderDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] as List<OrderDetailEntity>;
            if (!list.IsNullOrEmpty())
            {
                if (list.Exists(a => a.SnNum == snNum))
                {
                    list.Remove(a => a.SnNum == snNum);
                }
            }
            return Content(string.Empty);
        }

        /// <summary>
        /// 判断订单编号是否存在
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult IsExist()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            OrdersEntity entity = new OrdersEntity();
            entity.OrderNum = OrderNum;
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            OrdersEntity returnEntity = bill.GetOrder(entity);
            if (returnEntity.IsNotNull())
            {
                this.ReturnJson.AddProperty("Key", "error");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Cancel()
        {
            Session[CacheKey.TEMPDATA_CACHE_ORDERDETAIL] = null;
            return Content(string.Empty);
        }
    }
}
