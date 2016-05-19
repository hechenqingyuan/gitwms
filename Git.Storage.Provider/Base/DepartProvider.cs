/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-09-26 21:04:18
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-09-26 21:04:18       情缘
*********************************************************************************/

using Git.Framework.Cache;
using Git.Framework.Log;
using Git.Framework.Resource;
using Git.Storage.Entity.Base;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Storage.Common;

namespace Git.Storage.Provider.Base
{
    public partial class DepartProvider : DataFactory
    {
        private Log log = Log.Instance(typeof(DepartProvider));

        public DepartProvider() { }

        /// <summary>
        /// 获得所有的部门信息
        /// </summary>
        /// <returns></returns>
        public List<SysDepartEntity> GetList()
        {
            List<SysDepartEntity> listResult = CacheHelper.Get(CacheKey.JOOSHOW_SYSDEPART_CACHE) as List<SysDepartEntity>;
            if (!listResult.IsNullOrEmpty())
            {
                return listResult;
            }
            SysDepartEntity temp = new SysDepartEntity();
            temp.IncludeID(true)
                .IncludeChildCount(true)
                .IncludeCreateTime(true)
                .IncludeDepartName(true)
                .IncludeDepartNum(true)
                .IncludeDepth(true)
                .IncludeParentNum(true)
                ;
            temp.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            listResult = this.SysDepart.GetList(temp);
            if (!listResult.IsNullOrEmpty())
            {
                foreach (SysDepartEntity entity in listResult.Where(itemParent => !string.IsNullOrEmpty(itemParent.ParentNum)))
                {
                    SysDepartEntity tempEntity = listResult.SingleOrDefault(item => item.DepartNum == entity.ParentNum);
                    if (!tempEntity.IsNull())
                    {
                        entity.ParentName = tempEntity.DepartName;
                    }
                }
                CacheHelper.Insert(CacheKey.JOOSHOW_SYSDEPART_CACHE, listResult, null, DateTime.Now.AddHours(5));
            }
            return listResult;
        }

        /// <summary>
        /// 根据部门编号获得部门信息
        /// </summary>
        /// <param name="departNum"></param>
        /// <returns></returns>
        public SysDepartEntity GetSingle(string departNum)
        {
            List<SysDepartEntity> listResult = GetList();
            if (!listResult.IsNullOrEmpty() && !departNum.IsEmpty())
            {
                SysDepartEntity entity = listResult.SingleOrDefault(item => item.DepartNum == departNum);
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(SysDepartEntity entity)
        {
            entity.DepartNum = entity.DepartNum.IsEmpty() ? DateTime.Now.ToString("yyyyMMddHHmmss") + (new Random(DateTime.Now.Millisecond)).Next(1000, 9999) : entity.DepartNum;
            entity.ChildCount = 0;
            SysDepartEntity parent = GetSingle(entity.ParentNum);
            if (parent.IsNotNull())
            {
                entity.Depth = parent.Depth + 1;
                parent.ChildCount++;
                parent.IncludeDepth(true)
                    .IncludeChildCount(true)
                    .Where<SysDepartEntity>("DepartNum", ECondition.Eth);
                this.SysDepart.Update(parent);
            }
            entity.IncludeDepartNum(true)
                .IncludeDepartName(true)
                .IncludeChildCount(true)
                .IncludeParentNum(true)
                .IncludeDepth(true)
                .IncludeIsDelete(true)
                .IncludeCreateTime(true)
                ;
            int line = this.SysDepart.Add(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSDEPART_CACHE);
            }
            return line;
        }


        /// <summary>
        /// 修改部门信息(修改部门名称)
        /// 根据主键和父类编号修改部门信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateDepart(SysDepartEntity entity)
        {
            entity.ThrowIfNull("修改部门信息对象为空");
            entity.IncludeDepartName(true)
                ;
            //entity.Where<SysDepartEntity>("ID", ECondition.Eth)
            //    .And<SysDepartEntity>("ParentNum", ECondition.Eth);
            entity.Where(a => a.DepartNum == entity.DepartNum);
            int line = this.SysDepart.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSDEPART_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据主键编号修改部门信息(修改部门名和部门级别)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateDepartByID(SysDepartEntity entity)
        {
            entity.IncludeDepartName(true)
                .IncludeParentNum(true);
            entity.Where<SysDepartEntity>("ID", ECondition.Eth);
            int line = this.SysDepart.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSDEPART_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据主键编号获得部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysDepartEntity GetDepartEntity(int id)
        {
            return this.SysDepart.GetSingle(id);
        }

        /// <summary>
        /// 根据部门编号获得部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysDepartEntity GetDepartEntity(string DepartNum)
        {
            SysDepartEntity entity = new SysDepartEntity() { DepartNum = DepartNum };
            entity.IncludeID(true)
                .IncludeDepartNum(true)
                .IncludeDepartName(true)
                .IncludeParentNum(true)
                .Where<SysDepartEntity>("DepartNum", ECondition.Eth);
            return this.SysDepart.GetSingle(entity);
        }

        /// <summary>
        /// 根据主键编号和父类编号获得部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public SysDepartEntity GetDepartEntity(int id, string ParentNum)
        {
            SysDepartEntity entity = new SysDepartEntity() { ID = id, ParentNum = ParentNum };
            entity.IncludeID(true)
                .IncludeDepartNum(true)
                .IncludeDepartName(true)
                .IncludeParentNum(true)
                .IncludeChildCount(true)
                .IncludeCreateTime(true)
                .Where<SysDepartEntity>("ID", ECondition.Eth)
                .And<SysDepartEntity>("ParentNum", ECondition.Eth);
            List<SysDepartEntity> list = this.SysDepart.GetList(entity);
            return list.IsNullOrEmpty() ? null : list[0];
        }

        /// <summary>
        /// 根据主键编号获得父类部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysDepartEntity GetParent(int id)
        {
            List<SysDepartEntity> listSource = GetList();
            if (listSource.IsNullOrEmpty())
            {
                return null;
            }
            SysDepartEntity entity = listSource.SingleOrDefault(item => item.ID == id);
            if (entity.IsNull())
            {
                return null;
            }
            return listSource.SingleOrDefault(item => item.DepartNum == entity.ParentNum);
        }

        /// <summary>
        /// 根据父类编号获得部门信息
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public List<SysDepartEntity> GetList(string ParentNum)
        {
            List<SysDepartEntity> listSource = GetList();
            if (listSource.IsNullOrEmpty())
            {
                return null;
            }
            return listSource.Where(item => item.ParentNum == ParentNum).ToList();
        }


        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteDepart(int id)
        {
            int line = 0;
            SysDepartEntity entity = GetDepartEntity(id);
            if (entity.IsNotNull())
            {
                entity.IncludeIsDelete(true);
                entity.IsDelete = (int)EIsDelete.Deleted;
                entity.Where(a => a.ID == id);
                line = this.SysDepart.Update(entity);
                if (line > 0)
                {
                    CacheHelper.Remove(CacheKey.JOOSHOW_SYSDEPART_CACHE);
                }
            }
            return line;
        }

        /// <summary>
        /// 批量删除部门
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public int DeleteDepart(int[] items)
        {
            SysDepartEntity entity = new SysDepartEntity();
            int line = this.SysDepart.Delete(items);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSDEPART_CACHE);
            }
            return line;
        }
    }
}
