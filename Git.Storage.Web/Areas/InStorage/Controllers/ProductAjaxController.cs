using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Json;
using Git.Framework.ORM;
using Git.Framework.Resource;
using Git.Storage.Common;
using Git.Storage.Entity.InStorage;
using Git.Storage.Entity.Store;
using Git.Storage.Provider;
using Git.Storage.Provider.Base;
using Git.Storage.Provider.InStorage;
using Git.Storage.Provider.Store;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.InStorage.Controllers
{
    public class ProductAjaxController : AjaxPage
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
            int InType = WebUtil.GetFormValue<int>("InType", 0);
            int ProductType = WebUtil.GetFormValue<int>("ProductType", 0);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            string SupNum = WebUtil.GetFormValue<string>("SupNum", string.Empty);
            string SupName = WebUtil.GetFormValue<string>("SupName", string.Empty);
            string ContactName = WebUtil.GetFormValue<string>("ContactName", string.Empty);
            string Phone = WebUtil.GetFormValue<string>("Phone", string.Empty);
            DateTime OrderTime = WebUtil.GetFormValue<DateTime>("OrderTime", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);

            InStorageEntity entity = new InStorageEntity();
            entity.OrderNum = SequenceProvider.GetSequence(typeof(InStorageEntity));
            entity.InType = InType;
            entity.ProductType = ProductType;
            entity.SupNum = SupNum;
            entity.SupName = SupName;
            entity.ContactName = ContactName;
            entity.Phone = Phone;
            entity.Address = "";
            entity.ContractOrder = ContractOrder;
            entity.Status = (int)EAudite.Wait;
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.OrderTime = OrderTime;
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

            List<InStorDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] as List<InStorDetailEntity>;
            if (list.IsNullOrEmpty())
            {
                this.ReturnJson.AddProperty("Key", "1001");
                this.ReturnJson.AddProperty("Value", "请选择要入库的产品以及数量");
                return Content(this.ReturnJson.ToString());
            }
            entity.Num = list.Sum(a => a.Num);
            entity.Amount = list.Sum(a => a.Amount);
            Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
            string returnValue = bill.Create(entity, list);
            if (returnValue == EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success))
            {
                Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] = null;
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "入库单创建成功");
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
            int InType = WebUtil.GetFormValue<int>("InType", 0);
            int ProductType = WebUtil.GetFormValue<int>("ProductType", 0);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            string SupNum = WebUtil.GetFormValue<string>("SupNum", string.Empty);
            string SupName = WebUtil.GetFormValue<string>("SupName", string.Empty);
            string ContactName = WebUtil.GetFormValue<string>("ContactName", string.Empty);
            string Phone = WebUtil.GetFormValue<string>("Phone", string.Empty);
            DateTime OrderTime = WebUtil.GetFormValue<DateTime>("OrderTime", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            InStorageEntity entity = new InStorageEntity();
            entity.OrderNum = orderNum;
            entity.InType = InType;
            entity.ProductType = ProductType;
            entity.SupNum = SupNum;
            entity.SupName = SupName;
            entity.ContactName = ContactName;
            entity.Phone = Phone;
            entity.Address = "";
            entity.ContractOrder = ContractOrder;
            entity.Status = (int)EAudite.Wait;
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.OrderTime = OrderTime;
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

            List<InStorDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] as List<InStorDetailEntity>;
            if (list.IsNullOrEmpty())
            {
                this.ReturnJson.AddProperty("Key", "1001");
                this.ReturnJson.AddProperty("Value", "入库单必须包含入库产品,请重新核对");
                return Content(this.ReturnJson.ToString());
            }
            Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
            string returnValue = bill.EditOrder(entity, list);
            if (returnValue == EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success))
            {
                Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] = null;
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "入库单编辑成功");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 入库单新增产品
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddProduct()
        {
            string ProductNum = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            string BarCode = WebUtil.GetFormValue<string>("BarCode", string.Empty);
            string ProductName = WebUtil.GetFormValue<string>("ProductName", string.Empty);
            string BatchNum = WebUtil.GetFormValue<string>("ProductBatch", string.Empty);
            string LocalNum = WebUtil.GetFormValue<string>("LocalNum", string.Empty);
            string Size = WebUtil.GetFormValue<string>("Size", string.Empty);
            double LocalQty = WebUtil.GetFormValue<double>("LocalQty", 0);
            double Num = WebUtil.GetFormValue<double>("Num", 0);
            double InPrice = WebUtil.GetFormValue<double>("Price", 0);

            InStorDetailEntity entity = new InStorDetailEntity();
            entity.SnNum = SequenceProvider.GetSequence(typeof(InStorDetailEntity));
            entity.OrderNum = string.Empty;
            entity.ProductName = ProductName;
            entity.BarCode = BarCode;
            entity.ProductNum = ProductNum;
            entity.Num = Num;
            entity.IsPick = (int)EBool.No;
            entity.RealNum = 0;
            entity.InPrice = InPrice;
            entity.CreateTime = DateTime.Now;
            entity.LocalNum = LocalNum;
            LocationEntity localtion = new LocationProvider().GetSingleByNumCache(LocalNum);
            if (localtion != null)
            {
                entity.LocalName = new LocationProvider().GetSingleByNumCache(LocalNum).LocalName;
            }
            else
            {
                entity.LocalName = "";
            }
            entity.Amount = entity.InPrice * entity.Num;
            entity.StorageNum = this.DefaultPStore;
            entity.Size = Size;
            entity.BatchNum = BatchNum;
            entity.TotalPrice = entity.Num * entity.InPrice;
            List<InStorDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] as List<InStorDetailEntity>;
            list = list.IsNull() ? new List<InStorDetailEntity>() : list;
            list.Add(entity);
            Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] = list;

            return Content(string.Empty);
        }

        /// <summary>
        /// 入库单加载产品
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult LoadProduct()
        {
            List<InStorDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] as List<InStorDetailEntity>;
            list = list.IsNull() ? new List<InStorDetailEntity>() : list;
            this.ReturnJson.AddProperty("List", ConvertJson.ListToJson<InStorDetailEntity>(list, "Data"));
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除入库单详细，删除缓存中的内容
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DelDetail()
        {
            string snNum = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            List<InStorDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] as List<InStorDetailEntity>;
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
        /// 编辑入库单详细中的数量
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult EditNum()
        {
            string snNum = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            double num = WebUtil.GetFormValue<double>("num", 0);
            InStorageProvider provider = new InStorageProvider();
            int line = provider.EditInOrderNum(snNum, num);
            List<InStorDetailEntity> list = Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] as List<InStorDetailEntity>;
            if (!list.IsNullOrEmpty())
            {
                if (list.Exists(a => a.SnNum == snNum))
                {
                    InStorDetailEntity detail = list.First(a => a.SnNum == snNum);
                    detail.Num = num;
                    detail.Amount = num * detail.Amount;
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
            Session[CacheKey.TEMPDATA_CACHE_INSTORDETAIL] = null;
            return Content(string.Empty);
        }

        /// <summary>
        /// 获得当前产品的库存信息
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetLocalNum()
        {
            string storageNum = this.DefaultStore;
            string productNum = WebUtil.GetFormValue<string>("productNum", string.Empty);
            LocalProductProvider provider = new LocalProductProvider();
            double sum = provider.GetLocalNum(storageNum, productNum);
            this.ReturnJson.AddProperty("Sum", sum);
            return Content(this.ReturnJson.ToString());
        }
    }
}
