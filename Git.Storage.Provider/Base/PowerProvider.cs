/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-02-19 10:46:19
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-02-19 10:46:19       情缘
*********************************************************************************/

using Git.Framework.Cache;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.Resource;
using Git.Storage.Common;
using Git.Storage.Entity.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Git.Storage.Provider.Base
{
    public partial class PowerProvider : DataFactory
    {
        public PowerProvider() { }

        /// <summary>
        /// 获得所有的已经分配的权限关系列表
        /// </summary>
        /// <returns></returns>
        private List<SysRelationEntity> GetList()
        {
            List<SysRelationEntity> list = CacheHelper.Get(CacheKey.JOOSHOW_ALLOTPOWER_CACHE) as List<SysRelationEntity>;
            if (!list.IsNullOrEmpty())
            {
                return list;
            }
            SysRelationEntity entity = new SysRelationEntity();
            entity.IncludeAll();
            list = this.SysRelation.GetList(entity);
            if (!list.IsNullOrEmpty())
            {
                CacheHelper.Insert(CacheKey.JOOSHOW_ALLOTPOWER_CACHE, list, null, DateTime.Now.AddDays(1));
            }
            return list;
        }

        /// <summary>
        /// 给某个角色分配权限
        /// </summary>
        /// <param name="roleNum"></param>
        /// <param name="resItems"></param>
        /// <returns></returns>
        public int AllotPower(string roleNum, List<string> resItems)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                int line = 0;
                if (!resItems.IsNullOrEmpty())
                {
                    SysRelationEntity entity = new SysRelationEntity();
                    entity.Where(a => a.RoleNum == roleNum);
                    line = this.SysRelation.Delete(entity);

                    SysResourceProvider provider = new SysResourceProvider();

                    List<SysRelationEntity> list = new List<SysRelationEntity>();
                    foreach (string resNum in resItems)
                    {
                        SysResourceEntity resource = provider.GetResource(resNum);
                        short ResType = resource != null ? resource.ResType : (short)EResourceType.Page;
                        entity = new SysRelationEntity() { RoleNum = roleNum, ResNum = resNum, ResType = ResType };
                        entity.IncludeAll();
                        list.Add(entity);
                    }
                    line += this.SysRelation.Add(list);
                }
                ts.Complete();
                if (line > 0)
                {
                    CacheHelper.Remove(string.Format(CacheKey.JOOSHOW_ROLEPOWER_CACHE, roleNum));
                    CacheHelper.Remove(CacheKey.JOOSHOW_ALLOTPOWER_CACHE);
                }
                return line;
            }
        }

        /// <summary>
        /// 获得某个角色所分配的权限
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        private List<SysRelationEntity> GetList(string roleNum)
        {
            List<SysRelationEntity> list = GetList();
            if (!list.IsNullOrEmpty())
            {
                List<SysRelationEntity> listResult= list.Where(a => a.RoleNum == roleNum).ToList();
                return listResult;
            }
            return null;
        }

        /// <summary>
        /// 判断是否有权限操作
        /// </summary>
        /// <param name="ResNum"></param>
        /// <param name="RoleNum"></param>
        /// <returns></returns>
        public bool HasPower(string ResNum, string RoleNum)
        {
            List<SysResourceEntity> ListResult = GetRoleResource(RoleNum);
            ListResult = ListResult.IsNull() ? new List<SysResourceEntity>() : ListResult;
            bool hasPower = ListResult.Exists(a => a.ResNum.ToLower() == ResNum.ToLower() || a.Url.ToLower() == ResNum.ToLower());
            //超级管理员权限
            if (RoleNum == ResourceManager.GetSettingEntity("Super_AdminRole").Value)
            {
                hasPower = true;
            }
            return hasPower;
        }

        /// <summary>
        /// 获得某个角色的权限
        /// </summary>
        /// <param name="RoleNum"></param>
        /// <returns></returns>
        public List<SysResourceEntity> GetRoleResource(string RoleNum)
        {
            List<SysResourceEntity> ListResult = CacheHelper.Get(string.Format(CacheKey.JOOSHOW_ROLEPOWER_CACHE, RoleNum)) as List<SysResourceEntity>;
            if (ListResult.IsNullOrEmpty())
            {
                SysResourceProvider provider = new SysResourceProvider();
                List<SysResourceEntity> ListSource = provider.GetList();
                ListSource = ListSource.IsNull() ? new List<SysResourceEntity>() : ListSource;

                ListSource = JsonConvert.DeserializeObject<List<SysResourceEntity>>(JsonConvert.SerializeObject(ListSource));

                List<SysRelationEntity> ListRole = GetList(RoleNum);
                ListRole = ListRole.IsNull() ? new List<SysRelationEntity>() : ListRole;

                ListResult = ListSource.Where(a => ListRole.Exists(b => b.ResNum == a.ResNum)).ToList();

                //超级管理员权限
                if (RoleNum == ResourceManager.GetSettingEntity("Super_AdminRole").Value)
                {
                    ListResult = ListSource;
                }

                if (!ListResult.IsNullOrEmpty())
                {
                    CacheHelper.Insert(string.Format(CacheKey.JOOSHOW_ROLEPOWER_CACHE, RoleNum), ListResult, null, DateTime.Now.AddDays(1));
                }
            }
            return ListResult;
        }

        /// <summary>
        /// 获得权限分配
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        public Params<List<SysResourceEntity>, List<SysResourceEntity>, List<SysResourceEntity>> GetRole(string roleNum)
        {
            Params<List<SysResourceEntity>, List<SysResourceEntity>, List<SysResourceEntity>> param = new Params<List<SysResourceEntity>, List<SysResourceEntity>, List<SysResourceEntity>>();
            SysResourceProvider provider = new SysResourceProvider();
            param.Item1 = provider.GetList();
            param.Item2 = GetRoleResource(roleNum);

            param.Item1 = param.Item1.IsNull() ? new List<SysResourceEntity>() : param.Item1;
            param.Item2 = param.Item2.IsNull() ? new List<SysResourceEntity>() : param.Item2;
            param.Item3 = param.Item1.Where(a => !param.Item2.Exists(b => b.ResNum == a.ResNum)).ToList();
            param.Item3 = param.Item3.IsNull() ? new List<SysResourceEntity>() : param.Item3;
            List<SysResourceEntity> listTemp = new List<SysResourceEntity>();
            foreach (SysResourceEntity item in param.Item2)
            {
                if (!item.ParentNum.IsEmpty() && !param.Item2.Exists(a => a.ResNum == item.ParentNum))
                {
                    SysResourceEntity entity = param.Item1.First(a => a.ResNum == item.ParentNum);
                    if (!listTemp.Exists(a => a.ResNum == entity.ResNum))
                    {
                        listTemp.Add(entity);
                    }

                    if (!entity.ParentNum.IsEmpty() && !param.Item2.Exists(a => a.ResNum == entity.ParentNum))
                    {
                        entity = param.Item1.First(a => a.ResNum == entity.ParentNum);
                        if (!listTemp.Exists(a => a.ResNum == entity.ResNum))
                        {
                            listTemp.Add(entity);
                        }
                    }
                }
            }
            param.Item2.AddRange(listTemp);

            listTemp = new List<SysResourceEntity>();
            if (!param.Item3.IsNullOrEmpty())
            {
                foreach (SysResourceEntity item in param.Item3)
                {
                    if (!item.ParentNum.IsEmpty() && !param.Item3.Exists(a => a.ResNum == item.ParentNum))
                    {
                        SysResourceEntity entity = param.Item1.First(a => a.ResNum == item.ParentNum);
                        if (!listTemp.Exists(a => a.ResNum == entity.ResNum))
                        {
                            listTemp.Add(entity);
                        }

                        if (!entity.ParentNum.IsEmpty() && !param.Item3.Exists(a => a.ResNum == entity.ParentNum))
                        {
                            entity = param.Item1.First(a => a.ResNum == entity.ParentNum);
                            if (!listTemp.Exists(a => a.ResNum == entity.ResNum))
                            {
                                listTemp.Add(entity);
                            }
                        }
                    }
                }
            }
            param.Item3.AddRange(listTemp);

            return param;
        }

        /// <summary>
        /// 获得已经分配的权限
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        public List<SysResourceEntity> GetAllotedPower(string roleNum)
        {
            Params<List<SysResourceEntity>, List<SysResourceEntity>, List<SysResourceEntity>> param = GetRole(roleNum);
            return param.Item2;
        }
    }
}
