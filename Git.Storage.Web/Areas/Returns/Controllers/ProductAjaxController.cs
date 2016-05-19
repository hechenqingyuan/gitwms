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
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Returns.Controllers
{
    public class ProductAjaxController : AjaxPage
    {
        /// <summary>
        /// 加载出库信息
        /// 1. 判断是否有正在退货的情况 存在订单而且状态为等待审核
        /// 2. 如果存在已经退过货的情况，需要计数还有多少可以退
        /// 返回值：
        /// 1000 ： 操作成功
        /// 1001 ： 存在正在退货的单据
        /// 1002 :  该单据没有审核通过，不能处理申请退货
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Load()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            if (!OrderNum.IsEmpty())
            {
                ReturnProvider provider = new ReturnProvider();
                ReturnOrderEntity backEntity = provider.CheckOrder(OrderNum);
                if (backEntity != null && backEntity.Status == (int)EAudite.Wait)
                {
                    this.ReturnJson.AddProperty("d", "1001");
                    return Content(this.ReturnJson.ToString());
                }
                Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
                OutStorageEntity entity = new OutStorageEntity();
                entity.OrderNum = OrderNum;
                entity = bill.GetOrder(entity);
                if (entity.IsNotNull())
                {
                    if (entity.Status != (int)EAudite.Pass)
                    {
                        this.ReturnJson.AddProperty("d", "1002");
                        return Content(this.ReturnJson.ToString());
                    }
                    this.ReturnJson.AddProperty("CusNum", entity.CusNum);
                    this.ReturnJson.AddProperty("CusName", entity.CusName);
                    this.ReturnJson.AddProperty("Address", entity.Address);
                    this.ReturnJson.AddProperty("Contact", entity.Contact);
                    this.ReturnJson.AddProperty("Phone", entity.Phone);
                    //加载详细内容
                    OutStoDetailEntity detail = new OutStoDetailEntity();
                    detail.OrderNum = OrderNum;
                    List<OutStoDetailEntity> list = bill.GetOrderDetail(detail);
                    list = list.IsNull() ? new List<OutStoDetailEntity>() : list;
                    //获得已经退货的数量
                    List<ReturnDetailEntity> listResult = provider.GetDetailByOrder(OrderNum);
                    if (!listResult.IsNullOrEmpty())
                    {
                        list.ForEach(a =>
                        {
                            double qty = listResult.Where(b => b.BarCode == a.BarCode && b.ProductNum == a.ProductNum && b.LocalNum == a.LocalNum && b.StorageNum == a.StorageNum && b.BatchNum==a.BatchNum).Sum(b => b.BackNum);
                            a.BackNum = qty;
                        });
                    }
                    Session[CacheKey.TEMPDATE_CACHE_RETURNPRODUCTDETAIL] = list;
                    string json = ConvertJson.ListToJson<OutStoDetailEntity>(list, "List");
                    this.ReturnJson.AddProperty("data", json);
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 创建退货单
        /// 1005 请选择退货的产品
        /// 1000 退货单申请成功
        /// 1001 退货单申请失败
        /// 1002 退货单修改成功
        /// 1003 退货单修改失败
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Create([ModelBinder(typeof(JsonBinder<List<ReturnDetailEntity>>))] List<ReturnDetailEntity> list)
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int ReturnType = WebUtil.GetFormValue<int>("ReturnType", (int)EReturnType.Sell);
            int ProductType = WebUtil.GetFormValue<int>("ProductType", (int)EProductType.Goods);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            string CusNum = WebUtil.GetFormValue<string>("CusNum", string.Empty);
            string CusName = WebUtil.GetFormValue<string>("CusName", string.Empty);
            string Contact = WebUtil.GetFormValue<string>("Contact", string.Empty);
            string Phone = WebUtil.GetFormValue<string>("Phone", string.Empty);
            string Address = WebUtil.GetFormValue<string>("Address", string.Empty);
            DateTime CreateTime = WebUtil.GetFormValue<DateTime>("CreateTime", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);

            ReturnOrderEntity entity = new ReturnOrderEntity();
            entity.OrderNum = OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(ReturnOrderEntity)) : OrderNum;
            List<OutStoDetailEntity> listOrder = Session[CacheKey.TEMPDATE_CACHE_RETURNPRODUCTDETAIL] as List<OutStoDetailEntity>;
            string StorageNum = ResourceManager.GetSettingEntity("STORE_DEFAULT_PRODUCT").Value;
            if (!list.IsNullOrEmpty() && !listOrder.IsNullOrEmpty())
            {
                list.ForEach(a =>
                {
                    OutStoDetailEntity detail = listOrder.SingleOrDefault(b => b.ProductNum == a.ProductNum && b.BarCode == a.BarCode && b.LocalNum == a.LocalNum && b.StorageNum == StorageNum && b.BatchNum==a.BatchNum);
                    if (detail != null)
                    {
                        a.ProductName = detail.ProductName;
                        a.BatchNum = detail.BatchNum;
                        a.OutPrice = detail.OutPrice;
                        a.Num = detail.Num;
                        a.Amount = detail.Amount;
                        a.BackAmount = a.OutPrice * a.BackNum;
                        a.CreateTime = DateTime.Now;
                        a.StorageNum = StorageNum;
                        a.ContractOrder = ContractOrder;
                    }
                    a.SnNum = SequenceProvider.GetSequence(typeof(ReturnDetailEntity));
                });
                entity.ReturnType = ReturnType;
                entity.ProductType = ProductType;
                entity.CusNum = CusNum;
                entity.CusName = CusName;
                entity.Phone = Phone;
                entity.Contact = Contact;
                entity.Address = Address;
                entity.Num = list.Sum(a => a.BackNum);
                entity.Amount = list.Sum(a => a.BackAmount);
                entity.Status = (int)EAudite.Wait;
                entity.IsDelete = (int)EIsDelete.NotDelete;
                entity.CreateTime = CreateTime;
                entity.CreateUser = this.LoginUser.UserCode;
                entity.OperateType = (int)EOpType.PC;
                entity.EquipmentNum = string.Empty;
                entity.EquipmentCode = string.Empty;
                entity.Remark = Remark;
                entity.ContractOrder = ContractOrder;
                entity.StorageNum = this.DefaultStore;

                Bill<ReturnOrderEntity, ReturnDetailEntity> bill = new ReturnOrder();
                //如果单号不存在则为创建
                if (OrderNum.IsEmpty())
                {
                    string returnValue = bill.Create(entity, list);
                    if (returnValue == EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success))
                    {
                        this.ReturnJson.AddProperty("d", "1000");
                    }
                    else
                    {
                        this.ReturnJson.AddProperty("d", "1001");
                    }
                }
                else
                {
                    string returnValue = bill.EditOrder(entity, list);
                    this.ReturnJson.AddProperty("d", returnValue);
                }
            }
            else
            {
                this.ReturnJson.AddProperty("d", "1005");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 加载编辑退货单信息
        /// 1000: 数据加载成功
        /// 1001: 退货单已经处理
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Edit()
        {
            //退货单号
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            if (!OrderNum.IsEmpty())
            {
                //退货单部分
                Bill<ReturnOrderEntity, ReturnDetailEntity> bill = new ReturnOrder();
                ReturnOrderEntity entity = new ReturnOrderEntity();
                entity.OrderNum = OrderNum;
                entity = bill.GetOrder(entity);
                if (entity.Status != (int)EAudite.Wait)
                {
                    this.ReturnJson.AddProperty("d", "1001");
                    return Content(this.ReturnJson.ToString());
                }
                ReturnDetailEntity detail = new ReturnDetailEntity();
                detail.OrderNum = OrderNum;
                List<ReturnDetailEntity> listDetails = bill.GetOrderDetail(detail);
                //出货单部分
                Bill<OutStorageEntity, OutStoDetailEntity> provider = new OutStorageOrder();
                OutStoDetailEntity items = new OutStoDetailEntity();
                items.OrderNum = entity.ContractOrder;
                List<OutStoDetailEntity> list = provider.GetOrderDetail(items);

                //已经退货部分
                ReturnProvider returnProvider = new ReturnProvider();
                List<ReturnDetailEntity> listResult = returnProvider.GetDetailByOrder(entity.ContractOrder);

                if (!list.IsNullOrEmpty())
                {
                    list.ForEach(a =>
                    {
                        ReturnDetailEntity item = listDetails.FirstOrDefault(b => b.BarCode == a.BarCode && b.ProductNum == a.ProductNum && b.LocalNum == a.LocalNum && b.StorageNum == a.StorageNum);
                        if (item != null)
                        {
                            a.Qty = item.BackNum;
                        }
                        a.BackNum = listResult.Where(b => b.BarCode == a.BarCode && b.ProductNum == a.ProductNum && b.LocalNum == a.LocalNum && b.StorageNum == a.StorageNum).Sum(b => b.BackNum);
                    });
                }
                Session[CacheKey.TEMPDATE_CACHE_RETURNPRODUCTDETAIL] = list;
                string json = ConvertJson.ListToJson<OutStoDetailEntity>(list, "List");
                this.ReturnJson.AddProperty("data", json);
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}
