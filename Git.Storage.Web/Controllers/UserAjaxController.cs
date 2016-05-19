/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-09-01 15:18:43
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-09-01 15:18:43       情缘
*********************************************************************************/

using Git.Storage.Web.Lib;
using Git.Framework.Controller.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes;
using Git.Storage.Provider.Base;
using Git.Storage.Entity.Base;
using Git.Framework.Json;
using Git.Framework.Controller;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Provider;
using Git.Storage.Web.Lib.Filter;
using System.Data;
using Git.Storage.Common.Excel;
using Storage.Common;
using Newtonsoft.Json;

namespace Git.Storage.Web.Controllers
{
    public partial class UserAjaxController : AjaxPage
    {
        /// <summary>
        /// 获得管理员列表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetAdminList()
        {
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 1);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 15);
            string userCode = WebUtil.GetFormValue<string>("userCode", string.Empty);
            string userName = WebUtil.GetFormValue<string>("userName", string.Empty);
            string roleNum = WebUtil.GetFormValue<string>("roleNum", string.Empty);
            string departNum = WebUtil.GetFormValue<string>("departNum", string.Empty);
            string search = WebUtil.GetFormValue<string>("search", string.Empty);

            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            AdminProvider provider = new AdminProvider();
            AdminEntity entity = new AdminEntity();
            if (!search.IsEmpty())
            {
                entity.Where("UserCode", ECondition.Like, "%" + search + "%");
                entity.Or("RealName", ECondition.Like, "%" + search + "%");
                entity.Or("UserName", ECondition.Like, "%" + search + "%");
            }
            else
            {
                if (!userCode.IsEmpty())
                {
                    entity.Where("UserCode", ECondition.Like, "%" + userCode + "%");
                    entity.Or("RealName", ECondition.Like, "%" + userCode + "%");
                }
                if (!userName.IsEmpty())
                {
                    entity.Where("UserName", ECondition.Like, "%" + userName + "%");
                }
                if (!roleNum.IsEmpty())
                {
                    entity.And(a => a.RoleNum == roleNum);
                }
                if (!departNum.IsEmpty())
                {
                    entity.And(a => a.DepartNum == departNum);
                }
            }
            List<AdminEntity> listResult = provider.GetList(entity, ref pageInfo);
            string json = ConvertJson.ListToJson<AdminEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddUser([ModelBinder(typeof(JsonBinder<AdminEntity>))] AdminEntity entity)
        {
            AdminProvider provider = new AdminProvider();
            entity.UpdateTime = DateTime.Now;
            int line = 0;
            if (entity.UserCode.IsEmpty())
            {
                bool isExist = provider.IsExist(entity.UserName);
                if (isExist)
                {
                    this.ReturnJson.AddProperty("d", "该用户名已经存在！");
                    return Content(this.ReturnJson.ToString());
                }
                entity.UserCode = SequenceProvider.GetSequence(typeof(AdminEntity));
                line = provider.AddAdmin(entity);
            }
            else
            {
                line = provider.Update(entity);
            }
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="currentPassword"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ChangePwd(string currentPassword, string passWord)
        {
            AdminProvider provider = new AdminProvider();
            AdminEntity entity = provider.Login(this.LoginUser.UserName, currentPassword);
            int line = 0;
            if (entity.IsNotNull())
            {
                entity.PassWord = passWord;
                line = provider.UpdatePwd(entity);
            }
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(string userCode)
        {
            AdminProvider provider = new AdminProvider();
            int line = provider.Delete(userCode);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult BatchDel(string userCode)
        {
            AdminProvider provider = new AdminProvider();
            string[] list = userCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> ls = new List<string>();
            int line = 0;
            foreach (string t in list)
            {
                line += provider.Delete(t);
            }
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ToExcel()
        {
            PageInfo pageInfo = new Git.Framework.DataTypes.PageInfo() { PageIndex = 1, PageSize = Int32.MaxValue };
            string userCode = WebUtil.GetFormValue<string>("userCode", string.Empty);
            string userName = WebUtil.GetFormValue<string>("userName", string.Empty);
            string roleNum = WebUtil.GetFormValue<string>("roleNum", string.Empty);
            string departNum = WebUtil.GetFormValue<string>("departNum", string.Empty);
            AdminProvider provider = new AdminProvider();
            AdminEntity entity = new AdminEntity();
            if (!userCode.IsEmpty())
            {
                entity.Where("UserCode", ECondition.Like, userCode + "%");
            }
            if (!userName.IsEmpty())
            {
                entity.Where("UserName", ECondition.Like, userName + "%");
            }
            if (!roleNum.IsEmpty())
            {
                entity.And(a => a.RoleNum == roleNum);
            }
            if (!departNum.IsEmpty())
            {
                entity.And(a => a.DepartNum == departNum);
            }
            List<AdminEntity> listResult = provider.GetList(entity, ref pageInfo);
            if (!listResult.IsNullOrEmpty())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("用户名"));
                dt.Columns.Add(new DataColumn("编号"));
                dt.Columns.Add(new DataColumn("真名"));
                dt.Columns.Add(new DataColumn("Email"));
                dt.Columns.Add(new DataColumn("联系方式"));
                dt.Columns.Add(new DataColumn("登录次数"));
                dt.Columns.Add(new DataColumn("部门"));
                dt.Columns.Add(new DataColumn("角色"));
                foreach (AdminEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = t.UserName;
                    row[1] = t.UserCode;
                    row[2] = t.RealName;
                    row[3] = t.Email;
                    row[4] = t.Mobile;
                    row[5] = t.LoginCount;
                    row[6] = t.DepartName;
                    row[7] = t.RoleName;
                    dt.Rows.Add(row);
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("员工管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("员工管理", "员工", System.IO.Path.Combine(filePath, filename));
                excel.ToExcel(dt);
                this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
            }
            else
            {
                this.ReturnJson.AddProperty("d", "无数据导出!");
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 登录
        /// 1000: 登录成功
        /// 1001: 登录失败
        /// 1002: 验证码过期
        /// 1003：验证码错误
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public ActionResult Login(string userName, string passWord, string code)
        {
            //string valCode=SessionHelper.Get("ValCode");
            //if (valCode.IsEmpty())
            //{
            //    return Content("1002");
            //}
            //if (valCode.ToLower()!=code.ToLower())
            //{
            //    return Content("1003");
            //}
            AdminProvider provider = new AdminProvider();
            AdminEntity entity = provider.Login(userName, passWord);
            if (entity != null)
            {
                this.LoginUser = entity;
                provider.UpdateLoginCount(entity.UserName, entity.PassWord, entity.LoginCount);
                return Content("1000");
            }
            return Content("1001");
        }

        /// <summary>
        /// 序号
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Sequence()
        {
            string TabName = WebUtil.GetFormValue<string>("TabName",string.Empty);
            string Day = WebUtil.GetFormValue<string>("Day","");
            int PageIndex = WebUtil.GetFormValue<int>("PageIndex",1);
            int PageSize = WebUtil.GetFormValue<int>("PageSize",10);
            TNumProivder provider = new TNumProivder();
            TNumEntity entity = new TNumEntity();
            if (!TabName.IsEmpty())
            {
                entity.Where("TabName",ECondition.Like,"%"+TabName+"%");
            }
            if (!Day.IsEmpty())
            {
                entity.Where(a => a.Day == Day);
            }
            PageInfo pageInfo = new PageInfo() { PageIndex=PageIndex,PageSize=PageSize };
            List<TNumEntity> listResult = provider.GetList(entity,ref pageInfo);
            listResult = listResult.IsNull() ? new List<TNumEntity>() : listResult;
            string json = JsonConvert.SerializeObject(listResult);
            this.ReturnJson.AddProperty("Data", json);
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }


        /// <summary>
        /// 标识符管理
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult SN()
        {
            string TabName = WebUtil.GetFormValue<string>("TabName", string.Empty);
            int PageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            int PageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            SequenceProvider provider = new SequenceProvider();
            SequenceEntity entity = new SequenceEntity();
            if (!TabName.IsEmpty())
            {
                entity.Where("TabName", ECondition.Like, "%" + TabName + "%");
            }
            
            PageInfo pageInfo = new PageInfo() { PageIndex = PageIndex, PageSize = PageSize };
            List<SequenceEntity> listResult = provider.GetList(entity, ref pageInfo);
            listResult = listResult.IsNull() ? new List<SequenceEntity>() : listResult;
            string json = JsonConvert.SerializeObject(listResult);
            this.ReturnJson.AddProperty("Data", json);
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 修改标识符规则
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult UpdateSn()
        {
            SequenceEntity entity = WebUtil.GetFormObject<SequenceEntity>("entity");
            if (entity != null)
            {
                SequenceProvider provider = new SequenceProvider();
                int line = provider.Update(entity);
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}