/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-01-01 14:56:42
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-01-01 14:56:42       情缘
*********************************************************************************/

using Git.Framework.Log;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Entity.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Framework.Cache;
using Git.Storage.Common;
using System.Transactions;
using Git.Storage.Entity.Procedure;
using Git.Storage.Entity.Order;

namespace Git.Storage.Provider.Store
{
    public partial class ProductProvider : DataFactory
    {
        private Log log = Log.Instance(typeof(ProductProvider));

        public ProductProvider() { }

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(ProductEntity entity)
        {
            entity.IncludeAll();
            entity.InPrice = entity.AvgPrice;
            entity.OutPrice = entity.AvgPrice;
            int line = this.Product.Add(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCT_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(ProductEntity entity)
        {
            entity.InPrice = entity.AvgPrice;
            entity.OutPrice = entity.AvgPrice;
            entity.Include(a => new { a.ProductName, a.BarCode, a.MinNum, a.MaxNum, a.UnitNum, a.UnitName, a.CateNum, a.CateName, a.Size, a.InPrice, a.OutPrice, a.AvgPrice, a.GrossWeight, a.NetWeight, a.StorageNum, a.DefaultLocal, a.CusNum, a.CusName, a.Description });
            entity.Where(a => a.SnNum == entity.SnNum);
            int line = this.Product.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCT_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="productName"></param>
        /// <param name="unit"></param>
        /// <param name="snNum"></param>
        /// <returns></returns>
        public int Update(string barCode, string productName, string unit, string snNum)
        {
            ProductEntity entity = new ProductEntity();
            entity.BarCode = barCode;
            entity.ProductName = productName;
            entity.UnitNum = unit;
            entity.Include(a => new { a.ProductName, a.BarCode, a.UnitNum });
            entity.Where(a => a.SnNum == snNum);
            int line = this.Product.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCT_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="snNum"></param>
        /// <returns></returns>
        public int Delete(string snNum)
        {
            ProductEntity entity = new ProductEntity();
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.Where(a => a.SnNum == snNum);
            entity.IncludeIsDelete(true);
            int line = this.Product.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCT_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 批量删产品
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public int BatchDel(string[] items)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = this.Product.Delete(items);
                ts.Complete();
                return line;
            }
        }


        /// <summary>
        /// 获得产品的缓存数据
        /// </summary>
        /// <returns></returns>
        public List<ProductEntity> GetListByCache()
        {
            List<ProductEntity> list = CacheHelper.Get(CacheKey.JOOSHOW_PRODUCT_CACHE) as List<ProductEntity>;
            if (!list.IsNullOrEmpty())
            {
                return list;
            }
            ProductEntity entity = new ProductEntity();
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            list = this.Product.GetList(entity);
            if (!list.IsNullOrEmpty())
            {
                CacheHelper.Insert(CacheKey.JOOSHOW_PRODUCT_CACHE, list);
            }
            return list;
        }

        /// <summary>
        /// 查询产品列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<ProductEntity> GetList(ProductEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<ProductEntity> list = this.Product.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            if (!list.IsNullOrEmpty())
            {
                List<ProductCategoryEntity> listCate = new ProductCategoryProvider().GetList();
                listCate = listCate.IsNull() ? new List<ProductCategoryEntity>() : listCate;
                foreach (ProductEntity item in list)
                {
                    if (listCate.Exists(a => a.CateNum == item.CateNum))
                    {
                        item.CateName = listCate.First(a => a.CateNum == item.CateNum).CateName;
                    }
                }
            }
            pageInfo.RowCount = rowCount;
            return list;
        }

        /// <summary>
        /// 存储过程查询在线产品库存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <param name="ProductName"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<ProductEntity> GetList(ProductEntity entity, ref PageInfo pageInfo, string searchKey, string begin, string end)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<ProductEntity> list = this.Product.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            List<ProductEntity> listResult = new List<ProductEntity>();
            pageInfo.RowCount = rowCount;
            DateTime beginTime = ConvertHelper.ToType<DateTime>(begin, DateTime.Now.AddDays(-1));
            DateTime endTime = ConvertHelper.ToType<DateTime>(end, DateTime.Now);
            Proc_ProductReportEntity prEntity = new Proc_ProductReportEntity();
            prEntity.SearchKey = searchKey;
            prEntity.BeginTime = beginTime;
            prEntity.EndTime = endTime;
            if (!begin.IsEmpty() && !end.IsEmpty())
            {
                prEntity.IsTime = 1;//加上时间条件
            }
            else
            {
                prEntity.IsTime = 0;
            }
            if (!list.IsNullOrEmpty())
            {
                List<ProductCategoryEntity> listCates = new ProductCategoryProvider().GetList();
                listCates = listCates.IsNull() ? new List<ProductCategoryEntity>() : listCates;
                foreach (ProductEntity item in list)
                {
                    if (item.CateName.IsEmpty())
                    {
                        ProductCategoryEntity cate= listCates.FirstOrDefault(a => a.CateNum == item.CateNum);
                        item.CateName = cate == null ? "" : cate.CateName;
                    }
                    prEntity.ProductNum = item.SnNum;
                    prEntity.IsDelete = (int)EIsDelete.NotDelete;
                    prEntity.Status = (int)EAudite.Pass;
                    int line = this.Proc_ProductReport.ExecuteNonQuery(prEntity);
                    item.LocalProductNum = prEntity.LocalProductNum;
                    item.InStorageNum = prEntity.InStorageNum;
                    item.OutStorageNum = prEntity.OutStorageNum;
                    item.BadReportNum = prEntity.BadReportNum;
                    item.TotalLocalProductNum = prEntity.TotalLocalProductNum;
                    item.TotalInStorageNum = prEntity.TotalInStorageNum;
                    item.TotalOutStorageNum = prEntity.TotalOutStorageNum;
                    item.TotalBadReportNum = prEntity.TotalBadReportNum;
                    if (item.InStorageNum > 0 && item.TotalInStorageNum > 0)
                    {
                        item.InStorageNumPCT = (item.InStorageNum * 100.00f) / item.TotalInStorageNum;
                    }
                    if (item.OutStorageNum > 0 && item.TotalOutStorageNum > 0)
                    {
                        item.OutStorageNumPCT = (item.OutStorageNum * 100.00f) / item.TotalOutStorageNum;
                    }
                    listResult.Add(item);
                }
            }
            return listResult;
        }

        /// <summary>
        /// 根据产品编号查询产品信息
        /// </summary>
        /// <param name="snNum"></param>
        /// <returns></returns>
        public ProductEntity GetProductBySn(string snNum)
        {
            List<ProductEntity> list = GetListByCache();
            if (!list.IsNullOrEmpty())
            {
                return list.FirstOrDefault(a => a.SnNum == snNum);
            }
            return null;
        }

        /// <summary>
        /// 根据产品条码查询产品集合
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public List<ProductEntity> GetProductByBarCode(string barCode)
        {
            List<ProductEntity> list = GetListByCache();
            if (!list.IsNullOrEmpty())
            {
                return list.Where(a => a.BarCode == barCode).ToList();
            }
            return null;
        }

        /// <summary>
        /// 根据产品条码获取其数量
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public int GetCountByBarCode(string barCode)
        {
            ProductEntity entity = new ProductEntity();
            entity.Where(a => a.BarCode == barCode);
            int count = this.Product.GetCount(entity);
            return count;
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string ImportProCateData(List<ProductEntity> list)
        {
            string msg = "";
            try
            {
                foreach (ProductEntity entity in list)
                {
                    entity.IncludeAll();
                }
                List<ProductEntity> all = this.Product.GetList();
                if (all == null)
                {
                    all = new List<ProductEntity>();
                }
                List<int> ids = (from i in all select i.ID).ToList();
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    this.Product.Delete(ids);
                    //先删除在添加
                    this.Product.Add(list);
                    ts.Complete();
                }
                CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCT_CACHE);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                log.Info(msg);
            }
            return msg;
        }

    }
}
