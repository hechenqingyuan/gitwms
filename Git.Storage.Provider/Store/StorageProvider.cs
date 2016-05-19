/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-01-01 14:40:49
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-01-01 14:40:49       情缘
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
using Git.Framework.Cache;
using Git.Storage.Common;

namespace Git.Storage.Provider.Store
{
    public partial class StorageProvider : DataFactory
    {
        private readonly Log log = Log.Instance(typeof(StorageProvider));

        public StorageProvider() { }

        /// <summary>
        /// 注册仓库
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(StorageEntity entity)
        {
            //如果是默认仓库
            if (entity.IsDefault == (int)EBool.Yes)
            {
                StorageEntity temp = new StorageEntity();
                temp.Where(a => a.IsDefault == (int)EBool.Yes);
                if (this.Storage.GetCount(temp) > 0)
                {
                    temp = new StorageEntity();
                    temp.IsDefault = (int)EBool.No;
                    temp.IncludeIsDefault(true);
                    temp.Where(a => a.IsDefault == (int)EBool.Yes);
                    this.Storage.Update(temp);
                }
            }
            entity.IncludeAll();
            int line = this.Storage.Add(entity);
            if (line > 0)
            {
                Clear();
            }
            return line;
        }

        /// <summary>
        /// 查询所有的仓库数据信息
        /// </summary>
        /// <returns></returns>
        public List<StorageEntity> GetList()
        {
            StorageEntity entity = new StorageEntity();
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            List<StorageEntity> listResult = this.Storage.GetList();
            return listResult;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<StorageEntity> GetList(ref PageInfo pageInfo)
        {
            List<StorageEntity> listResult = CacheHelper.Get<List<StorageEntity>>(CacheKey.JOOSHOW_STORAGE_CACHE);
            if (!listResult.IsNullOrEmpty())
            {
                return listResult.Skip((pageInfo.PageIndex - 1) * pageInfo.PageSize).Take(pageInfo.PageSize).ToList();
            }
            return null;
        }

        /// <summary>
        /// 在缓存中查询仓库对象
        /// </summary>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public StorageEntity GetStoreByCache(string storageNum)
        {
            List<StorageEntity> listResult = CacheHelper.Get<List<StorageEntity>>(CacheKey.JOOSHOW_STORAGE_CACHE);
            if (!listResult.IsNullOrEmpty())
            {
                return listResult.FirstOrDefault(a => a.StorageNum == storageNum);
            }
            return null;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<StorageEntity> GetList(StorageEntity entity, ref PageInfo pageInfo)
        {
            try
            {
                entity.IncludeAll();
                entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
                entity.OrderBy(a => a.ID, EOrderBy.DESC);
                int rowCount = 0;
                List<StorageEntity> listResult = this.Storage.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
                pageInfo.RowCount = rowCount;
                return listResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据仓库编码查询仓库信息
        /// </summary>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public StorageEntity GetSingleByNum(string storageNum)
        {
            StorageEntity entity = new StorageEntity();
            entity.IncludeAll();
            entity.Where(a => a.StorageNum == storageNum);
            entity = this.Storage.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 删除仓库信息
        /// </summary>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public int Delete(string storageNum)
        {
            StorageEntity entity = new StorageEntity();
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.StorageNum == storageNum);
            int line = this.Storage.Update(entity);
            if (line > 0)
            {
                Clear();
            }
            return line;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(StorageEntity entity)
        {
            //如果是默认仓库
            if (entity.IsDefault == (int)EBool.Yes)
            {
                StorageEntity temp = new StorageEntity();
                temp.Where(a => a.IsDefault == (int)EBool.Yes);
                if (this.Storage.GetCount(temp) > 0)
                {
                    temp = new StorageEntity();
                    temp.IsDefault = (int)EBool.No;
                    temp.IncludeIsDefault(true);
                    temp.Where(a => a.IsDefault == (int)EBool.Yes);
                    this.Storage.Update(temp);
                }
            }
            int line = this.Storage.Update(entity);
            if (line > 0)
            {
                Clear();
            }
            return line;
        }

        /// <summary>
        /// 设置某个仓库为默认仓库值
        /// </summary>
        /// <param name="sotrageNum"></param>
        /// <returns></returns>
        public int SetDefault(string sotrageNum)
        {
            StorageEntity entity = new StorageEntity();
            entity.IsDefault = (int)EBool.No;
            entity.IncludeIsDefault(true);
            int line = this.Storage.Update(entity);

            entity = new StorageEntity();
            entity.IsDefault = (int)EBool.Yes;
            entity.IncludeIsDefault(true);
            entity.Where(a => a.StorageNum == sotrageNum);
            line += this.Storage.Update(entity);

            if (line > 0)
            {
                Clear();
            }
            return line;
        }

        /// <summary>
        /// 设置仓库的禁用和启用状态
        /// </summary>
        /// <param name="storageNum"></param>
        /// <param name="isForbid"></param>
        /// <returns></returns>
        public int SetForbid(string storageNum, EBool isForbid)
        {
            StorageEntity entity = new StorageEntity();
            entity.IsDefault = (int)isForbid;
            entity.IncludeIsDefault(true);
            entity.Where(a => a.StorageNum == storageNum);
            int line = this.Storage.Update(entity);
            return line;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <returns></returns>
        public int Clear()
        {
            CacheHelper.Remove(CacheKey.JOOSHOW_STORAGE_CACHE);
            return 0;
        }
    }
}
