using Git.Framework.Controller;
using Git.Framework.Log;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Common;
using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Entity.Check;
using Git.Storage.Provider;
using Git.Storage.Web.Lib.Filter;
using Git.Storage.Provider.Check;
using Git.Framework.Resource;
using Git.Framework.Controller.Mvc;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Framework.Json;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Check.Controllers
{
    public class ProductAjaxController : AjaxPage
    {
        /// <summary>
        /// 新建盘点单保存到数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Save()
        {
            int Type = WebUtil.GetFormValue<int>("Type", (int)ECheckType.Local);
            int ProductType = WebUtil.GetFormValue<int>("ProductType", (int)EProductType.Goods);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            DateTime CreateTime = WebUtil.GetFormValue<DateTime>("CreateTime", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);
            string TargetNum = WebUtil.GetFormValue<string>("TargetNum", string.Empty);
            CheckStockEntity entity = new CheckStockEntity();
            string orderNum = SequenceProvider.GetSequence(typeof(CheckStockEntity));
            entity.OrderNum = orderNum;
            entity.Type = Type;
            entity.ProductType = ProductType;
            entity.ContractOrder = ContractOrder;
            entity.Status = (int)EAudite.Wait;
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.CreateTime = CreateTime;
            entity.CreateUser = this.LoginUser.UserCode;
            entity.OperateType = (int)EOpType.PC;
            entity.IsComplete = (int)EBool.No;
            entity.Remark = Remark;
            entity.StorageNum = this.DefaultStore;

            List<ProductEntity> ListProducts = Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] as List<ProductEntity>;
            ListProducts = ListProducts.IsNull() ? new List<ProductEntity>() : ListProducts;

            List<CheckStockInfoEntity> listDetail = new List<CheckStockInfoEntity>();
            string storageNum = this.DefaultStore;
            foreach (ProductEntity key in ListProducts)
            {
                CheckStockInfoEntity detail = new CheckStockInfoEntity();
                detail.OrderNum = orderNum;
                detail.TargetNum = key.SnNum;
                detail.StorageNum = storageNum;
                detail.CreateTime = DateTime.Now;
                listDetail.Add(detail);
            }

            Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
            string returnValue = bill.Create(entity, listDetail);
            if (returnValue == EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success))
            {
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "盘点单创建成功");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 盘点添加产品信息
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddProduct([ModelBinder(typeof(JsonBinder<string[]>))] string[] ProductItems)
        {
            List<ProductEntity> ListProducts = Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] as List<ProductEntity>;
            ListProducts = ListProducts.IsNull() ? new List<ProductEntity>() : ListProducts;
            if (!ProductItems.IsNullOrEmpty())
            {
                ProductProvider provider = new ProductProvider();
                List<ProductEntity> ListSource = provider.GetListByCache();
                ListSource = ListSource.IsNull() ? new List<ProductEntity>() : ListSource;
                foreach (string key in ProductItems)
                {
                    if (ListSource.Exists(a => a.SnNum == key))
                    {
                        ProductEntity entity = ListSource.First(a => a.SnNum == key);
                        if (!ListProducts.Exists(a => a.SnNum == entity.SnNum))
                        {
                            ListProducts.Add(entity);
                        }
                    }
                }
            }
            Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] = ListProducts;
            return Content(string.Empty);
        }

        /// <summary>
        /// 权限盘点数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddAll()
        {
            string productNum = WebUtil.GetFormValue<string>("productNum",string.Empty);
            string cateNum = WebUtil.GetFormValue<string>("cateNum", string.Empty);
            ProductProvider provider = new ProductProvider();
            List<ProductEntity> listSource = provider.GetListByCache();
            if (!listSource.IsNullOrEmpty())
            {
                List<ProductEntity> listResult = listSource;
                if (!productNum.IsEmpty())
                {
                    listResult = listResult.Where(a => a.BarCode.Contains(productNum) || a.ProductName.Contains(productNum)).ToList();
                }
                if (!cateNum.IsEmpty())
                {
                    listResult = listResult.Where(a => a.CateNum == cateNum || a.CateName == cateNum).ToList();
                }

                List<ProductEntity> ListProducts = Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] as List<ProductEntity>;
                ListProducts = ListProducts.IsNull() ? new List<ProductEntity>() : ListProducts;

                foreach (ProductEntity item in listResult)
                {
                    if (!ListProducts.Exists(a => a.SnNum == item.SnNum))
                    {
                        ListProducts.Add(item);
                    }
                }
                Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] = ListProducts;
            }
            return Content(string.Empty);
        }

        /// <summary>
        /// 加载盘点单详细
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult LoadDetail()
        {
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);

            List<ProductEntity> ListProducts = Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] as List<ProductEntity>;
            ListProducts = ListProducts.IsNull() ? new List<ProductEntity>() : ListProducts;
            int rowCount = ListProducts.Count;
            ListProducts = ListProducts.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            string json = ConvertJson.ListToJson(ListProducts, "List");
            this.ReturnJson.AddProperty("Data", json);
            this.ReturnJson.AddProperty("RowCount", rowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除选中的盘点目标
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete()
        {
            List<ProductEntity> ListProducts = Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] as List<ProductEntity>;
            ListProducts = ListProducts.IsNull() ? new List<ProductEntity>() : ListProducts;
            string targetNum = WebUtil.GetFormValue<string>("targetNum");
            if (ListProducts.Exists(a => a.SnNum == targetNum))
            {
                ListProducts.Remove(a => a.SnNum == targetNum);
            }
            Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] = ListProducts;
            return Content("");
        }

        /// <summary>
        /// 编辑盘点单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Edit()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int Type = WebUtil.GetFormValue<int>("Type", (int)ECheckType.Local);
            int ProductType = WebUtil.GetFormValue<int>("ProductType", (int)EProductType.Goods);
            string ContractOrder = WebUtil.GetFormValue<string>("ContractOrder", string.Empty);
            DateTime CreateTime = WebUtil.GetFormValue<DateTime>("CreateTime", DateTime.Now);
            string Remark = WebUtil.GetFormValue<string>("Remark", string.Empty);
            string TargetNum = WebUtil.GetFormValue<string>("TargetNum", string.Empty);
            CheckStockEntity entity = new CheckStockEntity();

            entity.OrderNum = OrderNum;
            entity.Type = Type;
            entity.ProductType = ProductType;
            entity.ContractOrder = ContractOrder;
            entity.Status = (int)EAudite.Wait;
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.CreateTime = CreateTime;
            entity.CreateUser = this.LoginUser.UserCode;
            entity.OperateType = (int)EOpType.PC;
            entity.IsComplete = (int)EBool.No;
            entity.Remark = Remark;
            entity.StorageNum = this.DefaultStore;

            List<ProductEntity> ListProducts = Session[CacheKey.JOOSHOW_CHECKDETAIL_CACHE] as List<ProductEntity>;
            ListProducts = ListProducts.IsNull() ? new List<ProductEntity>() : ListProducts;

            List<CheckStockInfoEntity> listDetail = new List<CheckStockInfoEntity>();
            string storageNum = this.DefaultStore;
            foreach (ProductEntity key in ListProducts)
            {
                CheckStockInfoEntity detail = new CheckStockInfoEntity();
                detail.OrderNum = OrderNum;
                detail.TargetNum = key.SnNum;
                detail.StorageNum = storageNum;
                detail.CreateTime = DateTime.Now;
                listDetail.Add(detail);
            }

            Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
            string returnValue = bill.EditOrder(entity, listDetail);
            if (returnValue == EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success))
            {
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "盘点单创建成功");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 更新盘点数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult CheckData()
        {
            CheckDataEntity entity = new CheckDataEntity();
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string LocalNum = WebUtil.GetFormValue<string>("LocalNum", string.Empty);
            string ProductNum = WebUtil.GetFormValue<string>("ProductNum", string.Empty);
            string BarCode = WebUtil.GetFormValue<string>("BarCode", string.Empty);
            string BatchNum = WebUtil.GetFormValue<string>("BatchNum", string.Empty);
            string StorageNum = ResourceManager.GetSettingEntity("STORE_DEFAULT_PRODUCT").Value;
            string LocalName = WebUtil.GetFormValue<string>("LocalName", string.Empty);
            double Qty = WebUtil.GetFormValue<double>("Qty", 0);
            int ID = WebUtil.GetFormValue<int>("ID",0);
            entity.ID = ID;
            entity.OrderNum = OrderNum;
            entity.LocalNum = LocalNum;
            entity.LocalName = LocalName;
            entity.ProductNum = ProductNum;
            entity.BarCode = BarCode;
            entity.StorageNum = StorageNum;
            entity.FirstQty = Qty;
            entity.BatchNum = BatchNum;
            CheckDataProvider provider = new CheckDataProvider();
            int line = provider.UpdateCheckInfoNum(entity);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "盘点单创建成功");
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}
