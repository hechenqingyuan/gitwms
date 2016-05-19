/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-01-01 14:42:04
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-01-01 14:42:04       情缘
*********************************************************************************/

using Git.Framework.Cache;
using Git.Framework.Log;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Entity.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Git.Storage.Common;
using System.Transactions;

namespace Git.Storage.Provider.Store
{
    public partial class LocationProvider : DataFactory
    {
        private readonly Log log = Log.Instance(typeof(LocationProvider));

        public LocationProvider() { }

        /// <summary>
        /// 新增库位
        /// 如果设置为默认库位，则其他的库位修改为非默认库位
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(LocationEntity entity)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                //设置默认值
                if (entity.IsDefault == (int)EBool.Yes)
                {
                    LocationEntity temp = new LocationEntity();
                    temp.IsDefault = (int)EBool.No;
                    temp.IncludeIsDefault(true);
                    temp.Where(a => a.LocalNum == entity.LocalNum);
                    this.Location.Update(temp);
                }

                //绑定仓库信息
                StorageProvider storageProvider = new StorageProvider();
                List<StorageEntity> listStorage = storageProvider.GetList();
                if (entity.StorageNum.IsEmpty())
                {
                    if (!listStorage.IsNullOrEmpty())
                    {
                        StorageEntity storage = listStorage.FirstOrDefault(a => a.IsDefault == (int)EBool.Yes);
                        if (storage != null)
                        {
                            entity.StorageNum = storage.StorageNum;
                            entity.StorageType = storage.StorageType;
                        }
                    }
                }
                else
                {
                    if (!listStorage.IsNullOrEmpty())
                    {
                        StorageEntity storage = listStorage.FirstOrDefault(a => a.StorageNum == entity.StorageNum);
                        if (storage != null)
                        {
                            entity.StorageType = storage.StorageType;
                        }
                    }
                }
                entity.IncludeAll();
                int line = this.Location.Add(entity);
                if (line > 0)
                {
                    Clear();
                }
                ts.Complete();
                return line;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(LocationEntity entity)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                if (entity.IsDefault == (int)EBool.Yes)
                {
                    LocationEntity temp = new LocationEntity();
                    temp.IsDefault = (int)EBool.No;
                    temp.IncludeIsDefault(true);
                    this.Location.Update(temp);
                }

                //绑定仓库信息
                StorageProvider storageProvider = new StorageProvider();
                List<StorageEntity> listStorage = storageProvider.GetList();
                if (entity.StorageNum.IsEmpty())
                {
                    if (!listStorage.IsNullOrEmpty())
                    {
                        StorageEntity storage = listStorage.FirstOrDefault(a => a.IsDefault == (int)EBool.Yes);
                        if (storage != null)
                        {
                            entity.StorageNum = storage.StorageNum;
                            entity.StorageType = storage.StorageType;
                        }
                    }
                }
                else
                {
                    if (!listStorage.IsNullOrEmpty())
                    {
                        StorageEntity storage = listStorage.FirstOrDefault(a => a.StorageNum == entity.StorageNum);
                        if (storage != null)
                        {
                            entity.StorageType = storage.StorageType;
                        }
                    }
                }

                int line = this.Location.Update(entity);
                if (line > 0)
                {
                    Clear();
                }
                ts.Complete();
                return line;
            }
        }

        /// <summary>
        /// 获得所有的库位
        /// </summary>
        /// <returns></returns>
        public List<LocationEntity> GetList()
        {
            List<LocationEntity> listResult = CacheHelper.Get<List<LocationEntity>>(CacheKey.JOOSHOW_LOCATION_CACHE);
            if (!listResult.IsNullOrEmpty())
            {
                return listResult;
            }
            LocationEntity entity = new LocationEntity();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity.IncludeAll();
            listResult = this.Location.GetList(entity);
            if (!listResult.IsNullOrEmpty())
            {
                StorageProvider storageProvider = new StorageProvider();
                List<StorageEntity> listStorage = storageProvider.GetList();
                listStorage = listStorage.IsNull() ? new List<StorageEntity>() : listStorage;
                foreach (LocationEntity item in listResult)
                {
                    StorageEntity storage = listStorage.FirstOrDefault(a => a.StorageNum == item.StorageNum);
                    if (storage != null)
                    {
                        item.StorageName = storage.StorageName;
                    }
                }
                CacheHelper.Insert(CacheKey.JOOSHOW_LOCATION_CACHE, listResult, null, DateTime.Now.AddHours(6));
            }
            return listResult;
        }

        /// <summary>
        /// 根据仓库获得所有的库位
        /// </summary>
        /// <returns></returns>
        public List<LocationEntity> GetList(string storageNum)
        {
            List<LocationEntity> listResult = GetList();
            if (!listResult.IsNullOrEmpty())
            { 
                return listResult.Where(a=>a.StorageNum==storageNum).ToList();
            }
            return null;
        }

        /// <summary>
        /// 查询默认库位
        /// </summary>
        /// <param name="storageNum"></param>
        /// <param name="localType"></param>
        /// <returns></returns>
        public LocationEntity GetDefaultLocal(string storageNum, ELocalType localType)
        {
            List<LocationEntity> listResult = GetList(storageNum);
            if (!listResult.IsNullOrEmpty())
            {
                return listResult.Where(a => a.LocalType == (int)localType).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 根据库存编码查询仓库信息
        /// </summary>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public LocationEntity GetSingleByNum(string LocalNum)
        {
            LocationEntity entity = new LocationEntity();
            entity.IncludeAll();
            entity.Where(a => a.LocalNum == LocalNum);
            entity = this.Location.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 根据库位编号查询库位缓存中的库位
        /// </summary>
        /// <param name="localNum"></param>
        /// <returns></returns>
        public LocationEntity GetSingleByNumCache(string localNum)
        {
            List<LocationEntity> listResult = GetList();
            if (!listResult.IsNullOrEmpty())
            {
                return listResult.FirstOrDefault(a => a.LocalNum == localNum);
            }
            return null;
        }


        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<LocationEntity> GetList(LocationEntity entity, ref PageInfo pageInfo)
        {
            try
            {
                entity.IncludeAll();
                entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
                entity.OrderBy(a => a.ID, EOrderBy.DESC);
                int rowCount = 0;
                List<LocationEntity> listResult = this.Location.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
                pageInfo.RowCount = rowCount;
                if (!listResult.IsNullOrEmpty())
                {
                    StorageProvider storageProvider = new StorageProvider();
                    List<StorageEntity> listStorage = storageProvider.GetList();
                    listStorage = listStorage.IsNull() ? new List<StorageEntity>() : listStorage;
                    foreach (LocationEntity item in listResult)
                    {
                        StorageEntity storage = listStorage.FirstOrDefault(a => a.StorageNum == item.StorageNum);
                        if (storage != null)
                        {
                            item.StorageName = storage.StorageName;
                        }
                        else
                        {
                            item.StorageName = "";
                        }
                    }
                }
                return listResult;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
            return null;
        }

        /// <summary>
        /// 删除库位信息
        /// </summary>
        /// <param name="storageNum"></param>
        /// <returns></returns>
        public int Delete(string LocalNum)
        {
            LocationEntity entity = new LocationEntity();
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.LocalNum == LocalNum);
            int line = this.Location.Update(entity);
            if (line > 0)
            {
                Clear();
            }
            return line;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <returns></returns>
        public int Clear()
        {
            CacheHelper.Remove(CacheKey.JOOSHOW_LOCATION_CACHE);
            return 0;
        }
    }
}
