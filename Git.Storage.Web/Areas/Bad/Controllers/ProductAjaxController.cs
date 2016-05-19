using Git.Framework.Controller;
using Git.Framework.Json;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Bad;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Entity.Bad;
using Git.Storage.Provider;
using Git.Storage.Common;
using Git.Framework.DataTypes;
using Git.Framework.Controller.Mvc;
using Git.Framework.Log;
using Git.Framework.Resource;
using Git.Storage.Provider.Store;
using Git.Framework.Cache;
using System.Threading.Tasks;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Bad.Controllers
{
    public class ProductAjaxController : AjaxPage
    {
        private Log log = Log.Instance(typeof(ProductAjaxController));

        /// <summary>
        /// 报损单新增产品
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetLocalDetail()
        {
            string barCode = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            BadProvider provider = new BadProvider();
            List<LocalProductEntity> list = provider.GetList(barCode);
            if (!list.IsNullOrEmpty())
            {
                Parallel.ForEach(list, item => { item.Num = ConvertHelper.ToType<double>(item.Num.ToString(), 0); });
            }
            this.ReturnJson.AddProperty("List", ConvertJson.ListToJson<LocalProductEntity>(list, "Data"));
            return Content(this.ReturnJson.ToString());
        }


        /// <summary>
        /// 报损单加载产品 
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult LoadProduct([ModelBinder(typeof(JsonBinder<List<LocalProductEntity>>))]List<LocalProductEntity> list)
        {
            List<BadReportDetailEntity> ListProducts = Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] as List<BadReportDetailEntity>;
            ListProducts = ListProducts.IsNull() ? new List<BadReportDetailEntity>() : ListProducts;
            if (!list.IsNullOrEmpty())
            {
                LocationProvider localProvider = new LocationProvider();
                foreach (LocalProductEntity item in list)
                {
                    string BarCode = item.BarCode;
                    string ProductNum = item.ProductNum;
                    string StorageNum = item.StorageNum;
                    string FromLocalNum = item.LocalNum;
                    string BatchNum = item.BatchNum;
                    if (ListProducts.Exists(a => a.BarCode == BarCode && a.ProductNum == ProductNum && a.BatchNum == BatchNum && a.StorageNum == StorageNum && a.FromLocalNum == FromLocalNum))
                    {
                        BadReportDetailEntity entity = ListProducts.FirstOrDefault(a => a.BarCode == BarCode && a.ProductNum == ProductNum && a.BatchNum == BatchNum && a.StorageNum == StorageNum && a.FromLocalNum == FromLocalNum);
                        entity.Num += item.Num;
                    }
                    else
                    {
                        BadReportDetailEntity entity = new BadReportDetailEntity();
                        entity.SnNum = SequenceProvider.GetSequence(typeof(BadReportDetailEntity));
                        ProductProvider provider = new ProductProvider();
                        ProductEntity product = provider.GetProductBySn(ProductNum);
                        entity.ProductName = product.ProductName;
                        entity.BarCode = product.BarCode;
                        entity.ProductNum = product.SnNum;
                        entity.BatchNum = item.BatchNum;
                        entity.LocalNum = item.Num;
                        entity.Num = item.Qty;
                        entity.Amount = product.InPrice * item.Num;
                        entity.InPrice = product.InPrice;
                        entity.CreateTime = DateTime.Now;
                        entity.StorageNum = StorageNum;
                        entity.FromLocalNum = item.LocalNum;
                        LocationEntity fromLocal = localProvider.GetSingleByNumCache(item.LocalNum);
                        if (fromLocal != null)
                        {
                            entity.FromLocalName = fromLocal.LocalName;
                        }
                        LocationEntity localtion = localProvider.GetDefaultLocal(StorageNum, ELocalType.Bad);
                        if (localtion != null)
                        {
                            entity.ToLocalNum = localtion.LocalNum;
                        }
                        ListProducts.Add(entity);
                    }
                }
                Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] = ListProducts;
            }
            return Content(string.Empty);
        }

        /// <summary>
        /// 报损单加载产品
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult LoadLocalProduct()
        {
            List<BadReportDetailEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] as List<BadReportDetailEntity>;
            listResult = listResult.IsNull() ? new List<BadReportDetailEntity>() : listResult;
            this.ReturnJson.AddProperty("List", ConvertJson.ListToJson<BadReportDetailEntity>(listResult, "Data"));
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 创建报损单据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Create()
        {
            int BadType = WebUtil.GetFormValue<int>("BadType",(int)EBadType.Bad);
            int ProductType = WebUtil.GetFormValue<int>("ProductType", (int)EProductType.Goods);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            string CrateUser = WebUtil.GetFormValue<string>("CrateUser", string.Empty);
            DateTime OrderTime = WebUtil.GetFormValue<DateTime>("OrderTime", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);
            BadReportEntity badEntity = new BadReportEntity();
            badEntity.OrderNum = SequenceProvider.GetSequence(typeof(BadReportEntity));
            badEntity.BadType = BadType;
            badEntity.ProductType = ProductType;
            badEntity.ContractOrder = ContractOrder;
            badEntity.Status = (int)EAudite.Wait;
            badEntity.Num = 0;
            badEntity.IsDelete = (int)EIsDelete.NotDelete;
            badEntity.CreateTime = OrderTime;
            badEntity.CreateUser = CrateUser;
            badEntity.OperateType = (int)EOpType.PC;
            badEntity.EquipmentNum = "";
            badEntity.EquipmentCode = "";
            badEntity.Remark = Remark;
            badEntity.StorageNum = this.DefaultStore;

            List<BadReportDetailEntity> ListProducts = Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] as List<BadReportDetailEntity>;
            if (!ListProducts.IsNullOrEmpty())
            {
                foreach (BadReportDetailEntity item in ListProducts)
                {
                    item.OrderNum = badEntity.OrderNum;
                    badEntity.Num += item.Num;
                    badEntity.Amount += item.Amount;
                }
                Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
                string returnValue = bill.Create(badEntity, ListProducts);
                this.ReturnJson.AddProperty("d", returnValue);
                Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] = null;
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 编辑报损单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Edit()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int BadType = WebUtil.GetFormValue<int>("BadType", (int)EBadType.Bad);
            int ProductType = WebUtil.GetFormValue<int>("ProductType", (int)EProductType.Goods);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            string CrateUser = WebUtil.GetFormValue<string>("CrateUser", string.Empty);
            DateTime OrderTime = WebUtil.GetFormValue<DateTime>("OrderTime", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);
            BadReportEntity badEntity = new BadReportEntity();
            badEntity.OrderNum = OrderNum;
            badEntity.BadType = BadType;
            badEntity.ProductType = ProductType;
            badEntity.ContractOrder = ContractOrder;
            badEntity.Status = (int)EAudite.Wait;
            badEntity.Num = 0;
            badEntity.IsDelete = (int)EIsDelete.NotDelete;
            badEntity.CreateTime = OrderTime;
            badEntity.CreateUser = CrateUser;
            badEntity.OperateType = (int)EOpType.PC;
            badEntity.EquipmentNum = "";
            badEntity.EquipmentCode = "";
            badEntity.Remark = Remark;
            badEntity.StorageNum = this.DefaultStore;

            List<BadReportDetailEntity> ListProducts = Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] as List<BadReportDetailEntity>;
            if (!ListProducts.IsNullOrEmpty())
            {
                foreach (BadReportDetailEntity item in ListProducts)
                {
                    item.OrderNum = badEntity.OrderNum;
                }
                Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
                string returnValue = bill.EditOrder(badEntity, ListProducts);
                this.ReturnJson.AddProperty("d", returnValue);
                Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] = null;
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除入库单详细，删除缓存中的内容
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete()
        {
            string SnNum = WebUtil.GetFormValue<string>("SnNum", string.Empty);
            if (!SnNum.IsEmpty())
            {
                List<BadReportDetailEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] as List<BadReportDetailEntity>;
                if (!listResult.IsNullOrEmpty())
                {
                    listResult.Remove(a=>a.SnNum==SnNum);
                }
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}
