/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-01-01 14:39:57
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-01-01 14:39:57       情缘
*********************************************************************************/

using Git.Framework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Storage.Entity.Store;
using Git.Framework.Cache;
using Git.Storage.Common;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.DataTypes;

namespace Git.Storage.Provider.Store
{
    public partial class EquipmentProvider : DataFactory
    {
        private readonly Log log = Log.Instance(typeof(EquipmentProvider));

        public EquipmentProvider() { }

        /// <summary>
        ///  获得所有设备信息
        /// </summary>
        /// <returns></returns>
        public List<EquipmentEntity> GetList()
        {
            List<EquipmentEntity> listResult = CacheHelper.Get(CacheKey.JOOSHOW_EQUIPMENT_CACHE) as List<EquipmentEntity>;
            if (!listResult.IsNullOrEmpty())
            {
                return listResult;
            }
            EquipmentEntity entity = new EquipmentEntity();
            entity.IncludeAll();
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            listResult = this.Equipment.GetList(entity);
            if (!listResult.IsNullOrEmpty())
            {
                CacheHelper.Insert(CacheKey.JOOSHOW_EQUIPMENT_CACHE, listResult);
            }
            return listResult;
        }

        /// <summary>
        /// 分页查询设备信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<EquipmentEntity> GetList(EquipmentEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            int rowCount = 0;
            List<EquipmentEntity> listResult = this.Equipment.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }


        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddEquipment(EquipmentEntity entity)
        {
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.CreateTime = DateTime.Now;
            entity.IncludeAll();
            int line = this.Equipment.Add(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_EQUIPMENT_CACHE);
            }
            return line;
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="snNum"></param>
        /// <returns></returns>
        public int Delete(string snNum)
        {
            EquipmentEntity entity = new EquipmentEntity();
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.SnNum == snNum);
            int line = this.Equipment.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_EQUIPMENT_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据设备编号获得设备信息
        /// </summary>
        /// <param name="snNum"></param>
        /// <returns></returns>
        public EquipmentEntity GetEquipment(string snNum)
        {
            List<EquipmentEntity> listSource = GetList();
            if (listSource.IsNullOrEmpty())
            {
                return null;
            }
            EquipmentEntity entity = listSource.SingleOrDefault(item => item.SnNum == snNum);
            return entity;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(EquipmentEntity entity)
        {
            entity.Include(a => new { a.SnNum, a.EquipmentName, a.IsImpower, a.Flag, a.IsDelete, a.Remark,a.Status });
            entity.Where(a => a.SnNum == entity.SnNum);
            int line = this.Equipment.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_EQUIPMENT_CACHE);
            }
            return line;
        }
    }
}
