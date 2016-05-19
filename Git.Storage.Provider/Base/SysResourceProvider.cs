/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-10-28 23:08:26
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-10-28 23:08:26       情缘
*********************************************************************************/

using Git.Storage.Entity.Base;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Framework.Cache;
using Git.Storage.Common;

namespace Git.Storage.Provider.Base
{
    public partial class SysResourceProvider : DataFactory
    {
        /// <summary>
        /// 新增菜单项或者操作项
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddResource(SysResourceEntity entity)
        {
            entity.ThrowIfNull("添加资源信息对象为空");
            //父类的处理
            if (!entity.ParentNum.IsEmpty())
            {
                SysResourceEntity parentRes = GetResource(entity.ParentNum);
                if (parentRes.IsNotNull())
                {
                    parentRes.ChildCount++;
                    entity.Depth = parentRes.Depth + 1;
                    parentRes.IncludeChildCount(true)
                        .Where<SysResourceEntity>("ResNum", ECondition.Eth);
                    this.SysResource.Update(parentRes);
                }
            }
            entity.ResNum = entity.ResNum.IsNullOrEmpty() ? SequenceProvider.GetSequence(typeof(SysResourceEntity)) : entity.ResNum;
            entity.IncludeAll();
            int line = this.SysResource.Add(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSRESOURCE_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据资源编号获得资源对象
        /// </summary>
        /// <param name="resNum"></param>
        /// <returns></returns>
        public SysResourceEntity GetResource(string resNum)
        {
            List<SysResourceEntity> listSource = GetList();
            if (!listSource.IsNullOrEmpty())
            {
                return listSource.SingleOrDefault(item => item.ResNum == resNum);
            }
            return null;
        }

        /// <summary>
        /// 查询获取父类节点
        /// </summary>
        /// <param name="resNum"></param>
        /// <returns></returns>
        public SysResourceEntity GetParentRes(string resNum)
        {
            List<SysResourceEntity> listSource = GetList();
            if (listSource.IsNullOrEmpty())
            {
                return null;
            }
            SysResourceEntity entity = listSource.SingleOrDefault(a => a.ResNum == resNum);
            if (entity.IsNull())
            {
                return null;
            }
            string parentNum = entity.ParentNum;
            if (!parentNum.IsEmpty())
            {
                return listSource.SingleOrDefault(a => a.ResNum == parentNum);
            }
            return null;
        }

        /// <summary>
        /// 获得所有资源
        /// </summary>
        /// <returns></returns>
        public List<SysResourceEntity> GetList()
        {
            List<SysResourceEntity> list = CacheHelper.Get(CacheKey.JOOSHOW_SYSRESOURCE_CACHE) as List<SysResourceEntity>;
            if (!list.IsNullOrEmpty())
            {
                return list;
            }
            SysResourceEntity entity = new SysResourceEntity();
            entity.IncludeAll();
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            list = this.SysResource.GetList(entity);
            if (!list.IsNullOrEmpty())
            {
                list.ForEach(a => 
                {
                    a.ResouceType = EnumHelper.GetEnumDesc<EResourceType>(a.ResType);
                    a.ParentName = a.ParentNum.Do((string parentNum) => 
                    {
                        SysResourceEntity item = list.SingleOrDefault(b => b.ResNum == parentNum);
                        return item.IsNotNull() ? item.ResName : "";
                    },"");
                });
                CacheHelper.Insert(CacheKey.JOOSHOW_SYSRESOURCE_CACHE, list);
            }
            return list;
        }

        /// <summary>
        /// 根据父类编号获得资源信息
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public List<SysResourceEntity> GetChildList(string resNum)
        {
            List<SysResourceEntity> listSource = GetList();
            if (listSource.IsNullOrEmpty())
            {
                return null;
            }
            return listSource.Where(item => item.ParentNum == resNum).ToList();
        }

        /// <summary>
        /// 修改资源信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateResource(SysResourceEntity entity)
        {
            entity.IncludeResName(true)
                .IncludeSort(true)
                .IncludeUrl(true)
                .IncludeUpdateIp(true)
                .IncludeUpdateTime(true)
                .IncludeUpdateUser(true)
                .IncludeRemark(true)
                .IncludeDepart(true)
                .IncludeResType(true)
                .IncludeCssName(true)
                .IncludeDepart(true)
                .IncludeResType(true)
                .IncludeParentNum(true)
                .Where<SysResourceEntity>("ResNum", ECondition.Eth);
            int line = this.SysResource.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSRESOURCE_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 删除资源 逻辑删除 不是物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteResource(string ResNum)
        {

            SysResourceEntity entity = new SysResourceEntity();
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.ResNum == ResNum);
            int line = this.SysResource.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSRESOURCE_CACHE);
            }
            return line;
        }

    }
}
