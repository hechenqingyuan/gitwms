/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-01-01 14:55:46
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-01-01 14:55:46       情缘
*********************************************************************************/

using Git.Framework.Log;
using Git.Storage.Entity.Store;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Storage.Common;
using Git.Framework.Cache;
using System.Transactions;
using Git.Storage.Provider.Base;
using Git.Storage.Entity.Base;

namespace Git.Storage.Provider.Store
{
    public partial class ProductCategoryProvider : DataFactory
    {
        private readonly Log log = Log.Instance(typeof(ProductCategoryProvider));

        public ProductCategoryProvider() { }

        /// <summary>
        /// 添加产品类别
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(ProductCategoryEntity entity)
        {
            entity.IncludeAll();
            int line = this.ProductCategory.Add(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCTCATEGORY_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据产品类别删除删除
        /// </summary>
        /// <param name="cateNum"></param>
        /// <returns></returns>
        public int Delete(string cateNum)
        {
            ProductCategoryEntity entity = new ProductCategoryEntity();
            entity.Where(a => a.CateNum == cateNum);
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            int line = this.ProductCategory.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCTCATEGORY_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 批量删除产品类型
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int DelBat(List<string> list)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                foreach (string cateNum in list)
                {
                    ProductCategoryEntity entity = new ProductCategoryEntity();
                    entity.Where(a => a.CateNum == cateNum);
                    entity.IsDelete = (int)EIsDelete.Deleted;
                    entity.IncludeIsDelete(true);
                    line += this.ProductCategory.Update(entity);
                }
                ts.Complete();
                if (line > 0)
                {
                    CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCTCATEGORY_CACHE);
                }
                return line;
            }
        }

        /// <summary>
        /// 修改产品类别
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(ProductCategoryEntity entity)
        {
            entity.IncludeCateName(true).IncludeRemark(true)
                .Where(a => a.CateNum == entity.CateNum)
                ;
            int line = this.ProductCategory.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCTCATEGORY_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据产品类别编号查询
        /// </summary>
        /// <param name="cateNum"></param>
        /// <returns></returns>
        public ProductCategoryEntity GetSingle(string cateNum)
        {
            List<ProductCategoryEntity> list = GetList();
            if (!list.IsNullOrEmpty())
            {
                return list.FirstOrDefault(a => a.CateNum == cateNum);
            }
            return null;
        }

        /// <summary>
        /// 根据产品类别名称查询
        /// </summary>
        /// <param name="cateNum"></param>
        /// <returns></returns>
        public ProductCategoryEntity GetSingleCateName(string cateName)
        {
            List<ProductCategoryEntity> list = GetList();
            if (!list.IsNullOrEmpty())
            {
                return list.FirstOrDefault(a => a.CateName  == cateName);
            }
            return null;
        }

        /// <summary>
        /// 查询所有的产品类别
        /// </summary>
        /// <returns></returns>
        public List<ProductCategoryEntity> GetList()
        {
            List<ProductCategoryEntity> list = CacheHelper.Get(CacheKey.JOOSHOW_PRODUCTCATEGORY_CACHE) as List<ProductCategoryEntity>;
            if (!list.IsNullOrEmpty())
            {
                return list;
            }
            ProductCategoryEntity entity = new ProductCategoryEntity();
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            list = this.ProductCategory.GetList(entity);
            if (!list.IsNullOrEmpty())
            {
                CacheHelper.Insert(CacheKey.JOOSHOW_PRODUCTCATEGORY_CACHE, list);
            }
            return list;
        }

        /// <summary>
        /// 查询产品类别分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<ProductCategoryEntity> GetList(ProductCategoryEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            int rowCount = 0;
            List<ProductCategoryEntity> list = this.ProductCategory.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            if (!list.IsNullOrEmpty())
            {
                AdminProvider adminProvider = new AdminProvider();
                foreach (ProductCategoryEntity item in list)
                {
                    if (!item.CreateUser.IsEmpty())
                    {
                        AdminEntity admin = adminProvider.GetAdmin(item.CreateUser);
                        item.CreateUser = admin.IsNotNull() ? admin.UserName : item.CreateUser;
                    }
                }
            }
            return list;
        }

    }
}
