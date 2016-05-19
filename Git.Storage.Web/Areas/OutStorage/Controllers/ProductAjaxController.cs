using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Web.Lib.Filter;
using Git.Storage.Entity.OutStorage;
using Git.Storage.Provider;
using Git.Framework.Controller;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Common;
using Git.Framework.Json;
using Git.Storage.Provider.OutStorage;
using Git.Framework.DataTypes;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Framework.Controller.Mvc;
using Git.Storage.Entity.Order;
using Git.Storage.Provider.Order;
using Git.Framework.ORM;
using System.Text;
using Git.Framework.Resource;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.OutStorage.Controllers
{
    public class ProductAjaxController : AjaxPage
    {
        /// <summary>
        /// 新建出库单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Create()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int OutType = WebUtil.GetFormValue<int>("OutType", 0);
            int ProductType = WebUtil.GetFormValue<int>("ProductType", 0);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            string CusNum = WebUtil.GetFormValue<string>("CusNum", string.Empty);
            string CusName = WebUtil.GetFormValue<string>("CusName", string.Empty);
            string Address = WebUtil.GetFormValue<string>("Address", string.Empty);
            string ContactName = WebUtil.GetFormValue<string>("ContactName", string.Empty);
            string Phone = WebUtil.GetFormValue<string>("Phone", string.Empty);
            DateTime SendDate = WebUtil.GetFormValue<DateTime>("SendDate", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);

            OutStorageEntity entity = new OutStorageEntity();
            entity.OrderNum = OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(OutStorageEntity)) : OrderNum;
            entity.OutType = OutType;
            entity.ProductType = ProductType;
            entity.CusNum = CusNum;
            entity.CusName = CusName;
            entity.Contact = ContactName;
            entity.Phone = Phone;
            entity.Address = Address;
            entity.ContractOrder = ContractOrder;
            entity.Status = (int)EAudite.Wait;
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.SendDate = SendDate;
            entity.CreateTime = DateTime.Now;
            entity.CreateUser = this.LoginUserCode;
            entity.AuditUser = string.Empty;
            entity.AuditeTime = DateTime.MinValue;
            entity.PrintUser = string.Empty;
            entity.PrintTime = DateTime.MinValue;
            entity.Reason = string.Empty;
            entity.OperateType = (int)EOpType.PC;
            entity.EquipmentNum = string.Empty;
            entity.EquipmentCode = string.Empty;
            entity.Remark = Remark;
            entity.StorageNum = this.DefaultStore;

            List<OutStoDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] as List<OutStoDetailEntity>;
            if (list.IsNullOrEmpty())
            {
                this.ReturnJson.AddProperty("Key", "1001");
                this.ReturnJson.AddProperty("Value", "请选择要出库的产品以及数量");
                return Content(this.ReturnJson.ToString());
            }
            list.ForEach(a => { a.OrderNum = entity.OrderNum; });
            Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
            if (OrderNum.IsEmpty())
            {
                string returnValue = bill.Create(entity, list);
                if (returnValue == EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success))
                {
                    Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] = null;
                    this.ReturnJson.AddProperty("Key", "1000");
                    this.ReturnJson.AddProperty("Value", "出库单创建成功");
                }
            }
            else
            {
                string returnValue = bill.EditOrder(entity, list);
                if (returnValue == EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success))
                {
                    Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] = null;
                    this.ReturnJson.AddProperty("Key", "1000");
                    this.ReturnJson.AddProperty("Value", "出库单编辑成功");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 选择相应的出库产品并且获取相应的出库数量
        /// 确定之后将该数据填充到缓存之中
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddProduct([ModelBinder(typeof(JsonBinder<List<LocalProductEntity>>))] List<LocalProductEntity> list)
        {
            List<OutStoDetailEntity> ListCache = Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] as List<OutStoDetailEntity>;
            ListCache = ListCache.IsNull() ? new List<OutStoDetailEntity>() : ListCache;
            if (!list.IsNullOrEmpty())
            {
                List<ProductEntity> ListSource = new ProductProvider().GetListByCache();
                foreach (LocalProductEntity item in list)
                {
                    ProductEntity product = ListSource.FirstOrDefault(a => a.SnNum == item.ProductNum);
                    if (product.IsNotNull())
                    {
                        if (!ListCache.Exists(a => a.ProductNum == item.ProductNum && a.BatchNum == item.BatchNum && a.LocalNum == item.LocalNum))
                        {
                            OutStoDetailEntity entity = new OutStoDetailEntity();
                            entity.SnNum = SequenceProvider.GetSequence(typeof(OutStoDetailEntity));
                            entity.ProductName = product.ProductName;
                            entity.BarCode = product.BarCode;
                            entity.BatchNum = item.BatchNum;
                            entity.ProductNum = product.SnNum;
                            entity.LocalNum = item.LocalNum;
                            entity.LocalName = item.LocalName;
                            entity.StorageNum = this.DefaultStore;
                            entity.Num = item.Num;
                            entity.IsPick = (int)EBool.No;
                            entity.Size = product.Size.IsEmpty() ? "" : product.Size;
                            entity.RealNum = 0;
                            entity.OutPrice = product.InPrice;
                            entity.Amount = product.InPrice * entity.Num;
                            entity.CreateTime = DateTime.Now;
                            ListCache.Add(entity);
                        }
                        else
                        {
                            OutStoDetailEntity entity = ListCache.First(a => a.ProductNum == item.ProductNum && a.BatchNum == item.BatchNum && a.LocalNum == item.LocalNum);
                            entity.Num += item.Num;
                            entity.OutPrice = product.InPrice;
                            entity.Amount = product.InPrice * entity.Num;
                            entity.CreateTime = DateTime.Now;
                        }
                    }
                }
            }
            if (!ListCache.IsNullOrEmpty())
            {
                Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] = ListCache;
            }
            return Content(string.Empty);
        }

        /// <summary>
        /// 根据产品的条码编号查询该产品的库存情况
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetLocalProductList()
        {
            string BarCode = WebUtil.GetFormValue<string>("BarCode", string.Empty);
            LocalProductProvider provider = new LocalProductProvider();
            List<LocalProductEntity> listResult = provider.GetList(BarCode);
            listResult = listResult.IsNull() ? new List<LocalProductEntity>() : listResult;
            string json = ConvertJson.ListToJson<LocalProductEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 加载出库产品内存中的详细
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult LoadProduct()
        {
            List<OutStoDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] as List<OutStoDetailEntity>;
            list = list.IsNull() ? new List<OutStoDetailEntity>() : list;
            this.ReturnJson.AddProperty("List", ConvertJson.ListToJson<OutStoDetailEntity>(list, "Data"));
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除出库单详细，删除缓存中的内容
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DelDetail()
        {
            string snNum = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            List<OutStoDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] as List<OutStoDetailEntity>;
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
        /// 编辑出库单详细中的数量
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult EditNum()
        {
            string snNum = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            double num = WebUtil.GetFormValue<double>("num", 0);
            OutStorageProvider provider = new OutStorageProvider();
            int line = provider.EditInOrderNum(snNum, num);
            List<OutStoDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] as List<OutStoDetailEntity>;
            if (!list.IsNullOrEmpty())
            {
                if (list.Exists(a => a.SnNum == snNum))
                {
                    OutStoDetailEntity detail = list.First(a => a.SnNum == snNum);
                    detail.Num = num;
                    detail.Amount = detail.OutPrice * detail.Num;
                }
            }
            return Content(string.Empty);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Cancel()
        {
            Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] = null;
            return Content(string.Empty);
        }

        /******************************************************************************************订单出货*******************************************************************************************************/
        /// <summary>
        /// 选择相应的订单中的产品并且获取相应的出库数量
        /// 确定之后将该数据填充到缓存之中
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddOrderDetailProduct([ModelBinder(typeof(JsonBinder<string[]>))] string[] ProductItems,
            [ModelBinder(typeof(JsonBinder<string[]>))] string[] SnItems,
            [ModelBinder(typeof(JsonBinder<int[]>))]int[] QtyItems)
        {
            List<OutStoDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] as List<OutStoDetailEntity>;
            list = list.IsNull() ? new List<OutStoDetailEntity>() : list;

            OrderProvider provider = new OrderProvider();
            for (int i = 0; i < ProductItems.Length; i++)
            {
                string snNum = SnItems[i];
                int qty = QtyItems[i];
                string productNum = ProductItems[i];
                OrderDetailEntity detail = provider.GetOrderDetailBySnNum(snNum);
                detail = detail.IsNull() ? new OrderDetailEntity() : detail;
                if (detail.IsNotNull())
                {
                    list.Remove(a => a.ContractSn == detail.SnNum);
                    OutStoDetailEntity entity = new OutStoDetailEntity();
                    entity.SnNum = SequenceProvider.GetSequence(typeof(OutStoDetailEntity));
                    entity.ProductName = detail.ProductName;
                    entity.BarCode = detail.BarCode;
                    entity.ContractOrder = detail.OrderNum;
                    entity.ContractSn = detail.SnNum;
                    entity.BatchNum = "";
                    entity.ProductNum = detail.ProductNum;
                    entity.LocalNum = ResourceManager.GetSettingEntity("STORE_DEFAULT_MATERIAL_LocalNum").Value;//detail.LocalNum;
                    entity.LocalName = ResourceManager.GetSettingEntity("STORE_DEFAULT_MATERIAL_LocalName").Value;//detail.LocalName;
                    entity.StorageNum = this.DefaultStore;
                    entity.Num = qty;
                    entity.IsPick = (int)EBool.No;
                    entity.Size = detail.Size.IsEmpty() ? "" : detail.Size;
                    entity.RealNum = detail.RealNum;
                    entity.OutPrice = 0;//detail.InPrice;
                    entity.Amount = 0;//detail.InPrice * entity.Num;
                    entity.CreateTime = DateTime.Now;
                    list.Add(entity);
                }
            }
            if (!list.IsNullOrEmpty())
            {
                Session[CacheKey.TEMPDATA_CACHE_OUTSTORDETAIL] = list;
            }
            return Content(string.Empty);
        }

        /// <summary>
        /// 自动加载订单信息
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AutoOrder(string productName)
        {
            string orderNum = WebUtil.GetFormValue<string>("orderNum", string.Empty);
            PageInfo page = new PageInfo { PageIndex = 1, PageSize = 5 };
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            OrdersEntity entity = new OrdersEntity();
            if (orderNum != string.Empty)
            {
                entity.Where("OrderNum", ECondition.Like, "%" + orderNum + "%");
            }
            entity.Where(a => a.AuditeStatus == (int)EAudite.Pass).And(a => a.Status != (int)EOrderStatus.AllDelivery);
            List<OrdersEntity> list = bill.GetList(entity, ref page);

            list = list.IsNull() ? new List<OrdersEntity>() : list;
            StringBuilder sb = new StringBuilder();
            JsonObject jsonObject = null;
            foreach (OrdersEntity t in list)
            {
                jsonObject = new JsonObject();
                jsonObject.AddProperty("OrderNum", t.OrderNum);
                jsonObject.AddProperty("CusName", t.CusName);
                sb.Append(jsonObject.ToString() + "\n");
            }
            if (sb.Length == 0)
            {
                sb.Append("\n");
            }
            return Content(sb.ToString());
        }

        /// <summary>
        /// 获得订单详细
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetOrderDetail()
        {
            string orderNum = WebUtil.GetFormValue<string>("orderNum", string.Empty);
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            if (!orderNum.IsEmpty())
            {
                OrderDetailEntity detail = new OrderDetailEntity();
                detail.OrderNum = orderNum;
                List<OrderDetailEntity> listResult = bill.GetOrderDetail(detail);
                if (!listResult.IsNullOrEmpty())
                {
                    listResult.Remove(a => a.Num == 0);
                    LocalProductProvider provider = new LocalProductProvider();
                    string storageNum = this.DefaultStore;
                    foreach (OrderDetailEntity item in listResult)
                    {
                        item.LocalNum = provider.GetLocalNum(storageNum, item.ProductNum);
                    }
                    string json = ConvertJson.ListToJson<OrderDetailEntity>(listResult, "List");
                    JsonObject jsonObject = new JsonObject(json);
                    this.ReturnJson.AddProperty("Data", jsonObject);
                }
            }
            return Content(this.ReturnJson.ToString());
        }


    }
}
