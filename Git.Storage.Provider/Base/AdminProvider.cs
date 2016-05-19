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
using Git.Framework.Log;
using System.Transactions;

namespace Git.Storage.Provider.Base
{
    public partial class AdminProvider : DataFactory
    {
        private Log log = Log.Instance(typeof(AdminProvider));

        public AdminProvider() { }

        /// <summary>
        /// 查询用户管理员分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public List<AdminEntity> GetList(AdminEntity entity, ref PageInfo pageInfo)
        {
            entity.IncludeAll();
            entity.OrderBy(a => a.ID, EOrderBy.DESC);
            entity.Where(a => a.IsDelete == (int)EIsDelete.NotDelete);
            SysRoleEntity roleEntity = new SysRoleEntity();
            roleEntity.Include("RoleName", "RoleName");
            entity.Left<SysRoleEntity>(roleEntity, new Params<string, string>() { Item1 = "RoleNum", Item2 = "RoleNum" });
            SysDepartEntity departEntity = new SysDepartEntity();
            departEntity.Include("DepartName", "DepartName");
            entity.Left<SysDepartEntity>(departEntity, new Params<string, string>() { Item1 = "DepartNum", Item2 = "DepartNum" });
            int rowCount = 0;
            List<AdminEntity> listResult = this.Admin.GetList(entity, pageInfo.PageSize, pageInfo.PageIndex, out rowCount);
            pageInfo.RowCount = rowCount;
            return listResult;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddAdmin(AdminEntity entity)
        {
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.CreateTime = DateTime.Now;
            entity.ParentCode = "";
            entity.IncludeAll();
            int line = this.Admin.Add(entity);
            return line;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public int Delete(string userCode)
        {
            AdminEntity entity = new AdminEntity();
            entity.IsDelete = (int)EIsDelete.Deleted;
            entity.IncludeIsDelete(true);
            entity.Where(a => a.UserCode == userCode);
            int line = this.Admin.Update(entity);
            return line;
        }

        /// <summary>
        /// 根据用户编号获得用户信息
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public AdminEntity GetAdmin(string userCode)
        {
            AdminEntity entity = new AdminEntity();
            entity.Include(a => new { a.ID, a.UserCode, a.UserName, a.PassWord, a.Email, a.Phone, a.Mobile, a.RealName, a.RoleNum, a.DepartNum });
            entity.Where(a => a.UserCode == userCode);
            entity = this.Admin.GetSingle(entity);
            return entity;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(AdminEntity entity)
        {
            entity.Include(a => new { a.Phone, a.Email, a.Mobile, a.RealName, a.RoleNum, a.DepartNum, a.UserName, a.PassWord, a.UpdateTime });
            entity.Where(a => a.UserCode == entity.UserCode);
            int line = this.Admin.Update(entity);
            return line;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdatePwd(AdminEntity entity)
        {
            entity.Include(a => a.PassWord);
            entity.Where(a => a.UserCode == entity.UserCode);
            int line = this.Admin.Update(entity);
            return line;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public AdminEntity Login(string userName, string passWord)
        {
            AdminEntity entity = new AdminEntity();
            entity.IncludeAll();
            entity.Where(a => a.UserName == userName).And(a => a.PassWord == passWord);
            SysRoleEntity roleEntity = new SysRoleEntity();
            roleEntity.Include("RoleName", "RoleName");
            entity.Left<SysRoleEntity>(roleEntity, new Params<string, string>() { Item1 = "RoleNum", Item2 = "RoleNum" });
            entity = this.Admin.GetSingle(entity);
            if (entity != null)
            {
                AdminEntity admin = new AdminEntity();
                admin.LoginCount = admin.LoginCount + 1;
                admin.IncludeLoginCount(true);
                admin.Where(a => a.UserCode == entity.UserCode);
                this.Admin.Update(admin);
            }
            return entity;
        }

        /// <summary>
        /// 修改员工登录次数
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="loginCount"></param>
        /// <returns></returns>
        public int UpdateLoginCount(string userName, string passWord, int loginCount)
        {
            AdminEntity countEntity = new AdminEntity() { UserName = userName, PassWord = passWord };
            countEntity.IncludeLoginCount(true);
            countEntity.LoginCount = loginCount + 1;
            countEntity.Where<AdminEntity>("UserName", ECondition.Eth).And<AdminEntity>("PassWord", ECondition.Eth);
            int line = this.Admin.Update(countEntity);
            return line;
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string ImportProCateData(List<AdminEntity> list)
        {
            string msg = "";
            try
            {
                foreach (var entity in list)
                {
                    entity.IncludeAll();
                    entity.UserCode = SequenceProvider.GetSequence(typeof(AdminEntity));
                    entity.PassWord = "000000";
                    entity.IsDelete = (int)EIsDelete.NotDelete;
                    entity.CreateTime = DateTime.Now;
                    entity.UpdateTime = DateTime.Now;
                    entity.ParentCode = string.Empty;
                    entity.RoleNum = string.Empty;
                }
                this.Admin.Add(list);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsExist(string userName)
        {
            AdminEntity entity = new AdminEntity();
            entity.Include(a => new { a.ID, a.UserCode, a.UserName, a.PassWord, a.Email, a.Phone, a.Mobile, a.RealName, a.RoleNum, a.DepartNum });
            entity.Where(a => a.UserName == userName).And(a => a.IsDelete == (int)EIsDelete.NotDelete);
            entity = this.Admin.GetSingle(entity);
            if (entity.IsNotNull())
            {
                return true;
            }
            return false;
        }
    }
}
