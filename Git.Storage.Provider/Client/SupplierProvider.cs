/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-09-01 15:10:06
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-09-01 15:10:06       情缘
*********************************************************************************/

using Git.Framework.DataTypes;
using Git.Storage.Entity.Base;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Storage.Common;
using Git.Storage.Entity.Store;
using Git.Framework.Log;
using Git.Framework.Cache;

namespace Git.Storage.Provider.Client
{
    public partial class SupplierProvider : DataFactory
    {
        private Log log = Log.Instance(typeof(SupplierProvider));

        public SupplierProvider() { }

        /// <summary>
        /// 获得所有供应商信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<SupplierEntity> GetList()
        {
            //List<SupplierEntity> listResult = CacheHelper.Get(CacheKey.JOOSHOW_SUPPLIER_CACHE) as List<SupplierEntity>;
            //if (!listResult.IsNullOrEmpty())
            //{
            //    return listResult;
            //}
            SupplierEntity entity = new SupplierEntity();
            entity.IncludeAll();
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            List<SupplierEntity> listResult = this.Supplier.GetList(entity);
            //if (!listResult.IsNullOrEmpty())
            //{
            //    CacheHelper.Insert(CacheKey.JOOSHOW_SUPPLIER_CACHE, listResult);
            //}
            return listResult;
          
        }

        /// <summary>
        /// 获得所有供应商信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<SupplierEntity> GetList(SupplierEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            int rowCount = 0;
            List<SupplierEntity> listResult = this.Supplier.GetList(entity,pageInfo.PageSize,pageInfo.PageIndex,out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;

        }

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddSupplier(SupplierEntity entity)
        {
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.CreateTime = DateTime.Now;
            entity.CreateUser = "";
            entity.IncludeAll();
            int line = this.Supplier.Add(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SUPPLIER_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public int Delete(string supNum)
        {
            SupplierEntity entity = new SupplierEntity();
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.SupNum == supNum);
            int line = this.Supplier.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SUPPLIER_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据供应商编号获得供应商信息
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public SupplierEntity GetSupplier(string supNum)
        {
            List<SupplierEntity> listSource = GetList();
            if (listSource.IsNullOrEmpty())
            {
                return null;
            }
            SupplierEntity entity = listSource.SingleOrDefault(item => item.SupNum == supNum);
            return entity;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(SupplierEntity entity)
        {
            entity.Include(a => new { a.ID, a.SupNum, a.SupName, a.SupType,a.Email, a.Phone, a.Fax, a.ContactName, a.Address, a.Description });
            entity.Where(a => a.SupNum == entity.SupNum);
            int line = this.Supplier.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SUPPLIER_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string ImportProCateData(List<SupplierEntity> list)
        {
            string msg = "";
            try
            {
                foreach (var entity in list)
                {
                    entity.IncludeAll();
                }
                var all = this.Supplier.GetList();
                if (all == null)
                {
                    all = new List<SupplierEntity>();
                }
                var ids = (from i in all select i.ID).ToList();
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    this.Supplier.Delete(ids);
                    //先删除在添加
                    this.Supplier.Add(list);
                    ts.Complete();
                }
                CacheHelper.Remove(CacheKey.JOOSHOW_PRODUCT_CACHE);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }
    }
}
