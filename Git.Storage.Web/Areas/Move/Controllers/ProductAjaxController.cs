using Git.Framework.Controller;
using Git.Framework.Controller.Mvc;
using Git.Framework.Json;
using Git.Storage.Entity.Store;
using Git.Storage.Provider;
using Git.Storage.Provider.Move;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Entity.Move;
using Git.Storage.Common;
using Git.Storage.Provider.Store;
using Git.Framework.Resource;
using Git.Framework.DataTypes;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Move.Controllers
{
    public class ProductAjaxController : AjaxPage
    {
        /// <summary>
        /// 产品移库处理,获得库存清单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetLocalDetail()
        {
            string barCode = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            MoveProvider provider = new MoveProvider();
            List<LocalProductEntity> list = provider.GetList(barCode);
            if (!list.IsNullOrEmpty())
            {
                System.Threading.Tasks.Parallel.ForEach(list, item => { item.Num = ConvertHelper.ToType<double>(item.Num.ToString(), 0); });
            }
            this.ReturnJson.AddProperty("List", ConvertJson.ListToJson<LocalProductEntity>(list, "Data"));
            return Content(this.ReturnJson.ToString());
        }


        /// <summary>
        /// 选择移库产品提交
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult LoadProduct([ModelBinder(typeof(JsonBinder<List<MoveOrderDetailEntity>>))] List<MoveOrderDetailEntity> List)
        {
            List<MoveOrderDetailEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_MOVERODUCTDETAIL] as List<MoveOrderDetailEntity>;
            listResult = listResult.IsNull() ? new List<MoveOrderDetailEntity>() : listResult;
            if (!List.IsNullOrEmpty())
            {
                string storageNum = this.DefaultStore;
                ProductProvider provider = new ProductProvider();
                List<ProductEntity> listProduct = provider.GetListByCache();
                LocationProvider localProvider = new LocationProvider();
                List<LocationEntity> listLocals = localProvider.GetList(storageNum);
                foreach (MoveOrderDetailEntity item in List)
                {
                    ProductEntity product = listProduct.FirstOrDefault(a => a.BarCode == item.BarCode && a.SnNum == item.ProductNum);
                    if (product != null)
                    {
                        MoveOrderDetailEntity entity = new MoveOrderDetailEntity();
                        entity.SnNum = SequenceProvider.GetSequence(typeof(MoveOrderDetailEntity));
                        entity.ProductName = product.ProductName;
                        entity.BarCode = product.BarCode;
                        entity.ProductNum = product.SnNum;
                        entity.BatchNum = item.BatchNum;
                        entity.Num = item.Num;
                        entity.InPrice = product.InPrice;
                        entity.Amout = entity.InPrice * item.Num;
                        entity.IsPick = (int)EBool.Yes;
                        entity.RealNum = item.Num;
                        entity.CreateTime = DateTime.Now;
                        entity.StorageNum = storageNum;
                        entity.FromLocalNum = item.FromLocalNum;
                        entity.ToLocalNum = item.ToLocalNum;
                        LocationEntity local = listLocals.FirstOrDefault(a => a.LocalNum == item.FromLocalNum);
                        entity.FromLocalName = local != null ? local.LocalName : "";
                        local = listLocals.FirstOrDefault(a => a.LocalNum == item.ToLocalNum);
                        entity.ToLocalName = local != null ? local.LocalName : "";
                        listResult.Add(entity);
                    }
                }
                Session[CacheKey.TEMPDATA_CACHE_MOVERODUCTDETAIL] = listResult;
            }
            return Content(string.Empty);
        }

        /// <summary>
        /// 移库单产品
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult LoadLocalProduct()
        {
            List<MoveOrderDetailEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_MOVERODUCTDETAIL] as List<MoveOrderDetailEntity>;
            listResult = listResult.IsNull() ? new List<MoveOrderDetailEntity>() : listResult;
            this.ReturnJson.AddProperty("List", ConvertJson.ListToJson<MoveOrderDetailEntity>(listResult, "Data"));
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 创建报损单据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Create()
        {
            List<MoveOrderDetailEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_MOVERODUCTDETAIL] as List<MoveOrderDetailEntity>;
            if (listResult.IsNullOrEmpty())
            {
                this.ReturnJson.AddProperty("d", "1006");
                return Content(this.ReturnJson.ToString());
            }
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int MoveType = WebUtil.GetFormValue<int>("MoveType", (int)EMoveType.MoveToBad);
            int ProductType = WebUtil.GetFormValue<int>("ProductType", (int)EProductType.Goods);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            DateTime OrderTime = WebUtil.GetFormValue<DateTime>("OrderTime", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);
            MoveOrderEntity badEntity = new MoveOrderEntity();
            badEntity.OrderNum = OrderNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(MoveOrderEntity)) : OrderNum;
            badEntity.MoveType = MoveType;
            badEntity.ProductType = ProductType;
            badEntity.ContractOrder = ContractOrder;
            badEntity.Status = (int)EAudite.Wait;
            badEntity.Num = 0;
            badEntity.IsDelete = (int)EIsDelete.NotDelete;
            badEntity.CreateTime = OrderTime;
            badEntity.CreateUser = this.LoginUser.UserCode;
            badEntity.OperateType = (int)EOpType.PC;
            badEntity.EquipmentNum = string.Empty;
            badEntity.EquipmentCode = string.Empty;
            badEntity.Remark = Remark;
            badEntity.Num = listResult.Sum(a=>a.Num);
            badEntity.Amout = listResult.Sum(a => a.Amout);
            badEntity.StorageNum = this.DefaultStore;

            Bill<MoveOrderEntity, MoveOrderDetailEntity> bill = new MoveOrder();
            if (OrderNum.IsEmpty())
            {
                string returnValue = bill.Create(badEntity, listResult);
                this.ReturnJson.AddProperty("d", returnValue);
                Session[CacheKey.TEMPDATA_CACHE_MOVERODUCTDETAIL] = null;
            }
            else
            {
                string returnValue = bill.EditOrder(badEntity, listResult);
                this.ReturnJson.AddProperty("d", returnValue);
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 删除缓存中的移库单内容
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete()
        {
            string SnNum = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            List<MoveOrderDetailEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_MOVERODUCTDETAIL] as List<MoveOrderDetailEntity>;
            if (!SnNum.IsEmpty() && !listResult.IsNullOrEmpty())
            {
                if (listResult.Exists(a => a.SnNum == SnNum))
                {
                    listResult.Remove(a => a.SnNum == SnNum);
                }
            }
            return Content(string.Empty);
        }

    }
}
