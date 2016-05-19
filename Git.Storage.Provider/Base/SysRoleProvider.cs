/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-10-28 22:53:45
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-10-28 22:53:45       情缘
*********************************************************************************/

using Git.Framework.Cache;
using Git.Framework.Log;
using Git.Framework.ORM;
using Git.Storage.Entity.Base;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Git.Storage.Common;

namespace Git.Storage.Provider.Base
{
    public partial class SysRoleProvider : DataFactory
    {
        private Log log = Log.Instance(typeof(SysRoleProvider));

        public SysRoleProvider()
        {
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="entity"></param>
        public int AddRole(SysRoleEntity entity)
        {
            entity.IncludeAll();
            int line = this.SysRole.Add(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSROLE_CACHE);
            }
            return line;
        }


        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateRole(SysRoleEntity entity)
        {
            entity.IncludeRoleName(true).IncludeIsDelete(true).IncludeRemark(true)
                .Where(a => a.RoleNum == entity.RoleNum)
                ;
            int line = this.SysRole.Update(entity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSROLE_CACHE);
            }
            return line;
        }

        /// <summary>
        /// 根据角色编号获得角色信息
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        public SysRoleEntity GetRoleEntity(string roleNum)
        {
            List<SysRoleEntity> listSource = GetList();
            if (!listSource.IsNullOrEmpty())
            {
                return listSource.SingleOrDefault(item => item.RoleNum == roleNum);
            }
            return null;
        }

        /// <summary>
        /// 根据主键编号获得角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysRoleEntity GetRoleEntity(int id)
        {
            List<SysRoleEntity> listSource = GetList();
            if (listSource.IsNullOrEmpty())
            {
                return null;
            }
            SysRoleEntity entity = listSource.SingleOrDefault(item => item.ID == id);
            return entity;
        }
       
        /// <summary>
        /// 获得所有角色信息
        /// </summary>
        /// <returns></returns>
        public List<SysRoleEntity> GetList()
        {
            List<SysRoleEntity> list = CacheHelper.Get(CacheKey.JOOSHOW_SYSROLE_CACHE) as List<SysRoleEntity>;
            if (!list.IsNullOrEmpty())
            {
                return list;
            }
            SysRoleEntity sysRole = new SysRoleEntity();
            sysRole.IncludeAll();
            int isDelete = (int)EIsDelete.NotDelete;
            sysRole.Where(a => a.IsDelete == isDelete);
            list = this.SysRole.GetList(sysRole);
            if (!list.IsNullOrEmpty())
            {
                CacheHelper.Insert(CacheKey.JOOSHOW_SYSROLE_CACHE, list);
            }
            return list;
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        public int DeleteRole(string roleNum)
        {
            SysRoleEntity roleEntity = new SysRoleEntity();
            roleEntity.IsDelete = (int)EIsDelete.Deleted;
            roleEntity.IncludeIsDelete(true);
            roleEntity.Where(a => a.RoleNum == roleNum);
            int line = this.SysRole.Update(roleEntity);
            if (line > 0)
            {
                CacheHelper.Remove(CacheKey.JOOSHOW_SYSROLE_CACHE);
            }
            return line;
        }

    }
}
