/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-01-01 14:38:43
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-01-01 14:38:43       情缘
*********************************************************************************/

using Git.Framework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Framework.Cache;
using Git.Storage.Entity.Store;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.DataTypes;
using Git.Storage.Common;

namespace Git.Storage.Provider.Client
{
    public partial class CustomerProvider : DataFactory
    {
        private readonly Log log = Log.Instance(typeof(CustomerProvider));

        public CustomerProvider()
        {
        }

        /// <summary>
        /// 分页查询客户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<CustomerEntity> GetCustomerList(CustomerEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            int rowCount = 0;
            List<CustomerEntity> listResult = this.Customer.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }


        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int AddCustomer(CustomerEntity entity, List<CusAddressEntity> list)
        {
            entity.IncludeAll();
            int line = this.Customer.Add(entity);
            if (!list.IsNullOrEmpty())
            {
                list.ForEach(a =>
                {
                    a.CusNum = entity.CusNum;
                    a.IncludeAll();
                });
                line += this.CusAddress.Add(list);

            }
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_CUSADDRESS_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="cusNum"></param>
        /// <returns></returns>
        public int Delete(string cusNum)
        {
            CustomerEntity entity = new CustomerEntity();
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.CusNum == cusNum);
            int line = this.Customer.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_CUSADDRESS_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据客户编号获得客户信息
        /// </summary>
        /// <param name="cusNum"></param>
        /// <returns></returns>
        public CustomerEntity GetSingleCustomer(string cusNum)
        {
            CustomerEntity entity = new CustomerEntity();
            entity.IncludeAll();
            entity.Where(a => a.CusNum == cusNum);
            entity = this.Customer.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Update(CustomerEntity entity, List<CusAddressEntity> list)
        {
            entity.IncludeCusName(true)
                .IncludeEmail(true)
                .IncludeFax(true)
                .IncludePhone(true)
                .IncludeRemark(true)
                .IncludeCusType(true)
                ;
            entity.Where(a => a.CusNum == entity.CusNum);
            int line = this.Customer.Update(entity);
            if (!list.IsNullOrEmpty())
            {
                foreach (CusAddressEntity item in list)
                {
                    item.CusNum = entity.CusNum;
                    CusAddressEntity tempEntity = new CusAddressEntity();
                    tempEntity.IncludeAll();
                    tempEntity.Where(a => a.CusNum == item.CusNum).And(a => a.SnNum == item.SnNum);
                    tempEntity = this.CusAddress.GetSingle(tempEntity);
                    if (tempEntity.IsNotNull())
                    {
                        item.IncludeAddress(true)
                        .IncludeContact(true)
                        .IncludePhone(true)
                        ;
                        item.Where(a => a.SnNum == item.SnNum).And(a => a.CusNum == item.CusNum);
                        line += this.CusAddress.Update(item);
                    }
                    else
                    {
                        item.IncludeAll();
                        line = this.CusAddress.Add(item);
                    }
                }
                if (line > 0)
                {
                    CacheHelper.Remove(CacheKey.JOOSHOW_CUSADDRESS_CACHE);
                }
            }
            return line;
        }


        /// <summary>
        /// 获得所有的地址
        /// </summary>
        /// <param name="cusNum"></param>
        /// <returns></returns>
        public List<CusAddressEntity> GetAddressList(string cusNum)
        {
            CusAddressEntity entity = new CusAddressEntity();
            entity.IncludeAll();
            entity.Where<CusAddressEntity>(a => a.CusNum == cusNum).And<CusAddressEntity>(a => a.IsDelete == (int)EIsDelete.NotDelete);
            List<CusAddressEntity> listResult = this.CusAddress.GetList(entity);
            return listResult;
        }

        /// <summary>
        /// 根据收货单位编号获得地址信息
        /// </summary>
        /// <param name="snNum"></param>
        /// <returns></returns>
        public CusAddressEntity GetSingleAddress(string snNum)
        {
            try
            {
                CusAddressEntity entity = new CusAddressEntity();
                entity.IncludeAll();
                entity.Where(a => a.SnNum == snNum);
                entity = this.CusAddress.GetSingle(entity);
                return entity;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddCusAddress(CusAddressEntity entity)
        {
            entity.IncludeAll();
            int line = this.CusAddress.Add(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_CUSADDRESS_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="snNum"></param>
        /// <param name="cusNum"></param>
        /// <returns></returns>
        public int DelCusAddress(string snNum, string cusNum)
        {
            int line = 0;
            List<CusAddressEntity> list = GetAddressList(cusNum);
            list.ForEach(a =>
            {
                if (!snNum.IsNullOrEmpty() && !cusNum.IsNullOrEmpty())
                {
                    CusAddressEntity entity = new CusAddressEntity();
                    entity.IncludeIsDelete(true);
                    entity.IsDelete = (int)EIsDelete.Deleted;
                    entity.Where(c => c.SnNum == snNum).And(c => c.CusNum == cusNum);
                    line += this.CusAddress.Update(entity);
                    if (a.SnNum == snNum)
                    {
                        a.IsDelete = (int)EIsDelete.Deleted;
                        line++;
                    }
                }
                else
                {
                    if (a.SnNum == snNum)
                    {
                        a.IsDelete = (int)EIsDelete.Deleted;
                        line++;
                    }
                }
            });
            list.Remove(a => a.IsDelete == (int)EIsDelete.Deleted);
            CacheHelper.Insert(CacheKey.JOOSHOW_CUSADDRESS_CACHE, list);
            return line;
        }

        /// <summary>
        /// 查询所有的客户数据信息
        /// </summary>
        /// <returns></returns>
        public List<CustomerEntity> GetList()
        {
            CustomerEntity entity = new CustomerEntity();
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            List<CustomerEntity> listResult = this.Customer.GetList();
            return listResult;
        }

    }
}
