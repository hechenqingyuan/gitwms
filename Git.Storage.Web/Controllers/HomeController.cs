using Git.Framework.Controller;
using Git.Storage.Entity.Base;
using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.DataTypes;
using Git.Framework.ORM;
using Git.Storage.Provider.Base;
using Git.Storage.Web.Lib.Filter;
using System.Net.Http;
using Git.Framework.Resource;

namespace Git.Storage.Web.Controllers
{
    public class HomeController : MasterPage
    {
        public ActionResult Index()
        {
            string url = WebUtil.GetQueryStringValue<string>("returnurl", string.Empty);
            ViewBag.ReferrerUrl = url;

            string sign = Git.Framework.Encrypt.Encrypt.TripleDESDecrypting(ResourceManager.GetSettingEntity("loginsign").Value);
            ViewBag.LoginSign = sign;

            return View();
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult UserList()
        {
            ViewBag.RoleList = BaseHelper.GetRoleList(string.Empty);
            ViewBag.DepartList = BaseHelper.GetDepartList(string.Empty);
            return View();
        }

        /// <summary>
        /// 用户弹出框
        /// </summary>
        /// <returns></returns>
        public ActionResult Dialog()
        {
            return View();
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult RoleList()
        {
            return View();
        }

        /// <summary>
        /// 部门管理
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult DepartList()
        {
            return View();
        }

        /// <summary>
        /// 序号管理
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Sequence()
        {
            return View();
        }

        /// <summary>
        /// 标识符管理
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult SN()
        {
            return View();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddUser()
        {
            string userCode = WebUtil.GetQueryStringValue<string>("userCode");
            if (userCode.IsEmpty())
            {
                ViewBag.RoleList = BaseHelper.GetRoleList(string.Empty);
                ViewBag.DepartList = BaseHelper.GetDepartList(string.Empty);
                ViewBag.Admin = new AdminEntity();
                return View();
            }
            else
            {
                AdminProvider provider = new AdminProvider();
                AdminEntity entity = provider.GetAdmin(userCode);
                entity = entity == null ? new AdminEntity() : entity;
                ViewBag.RoleList = BaseHelper.GetRoleList(entity.RoleNum);
                ViewBag.DepartList = BaseHelper.GetDepartList(entity.DepartNum);
                entity.RoleName = this.LoginUser.RoleName;
                ViewBag.Admin = entity;
                return View();
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddRole()
        {
            string roleNum = WebUtil.GetQueryStringValue<string>("roleNum");
            if (roleNum.IsEmpty())
            {
                ViewBag.SysRole = new SysRoleEntity();
                return View();
            }
            else
            {
                SysRoleProvider provider = new SysRoleProvider();
                SysRoleEntity entity = provider.GetRoleEntity(roleNum);
                entity = entity == null ? new SysRoleEntity() : entity;
                ViewBag.SysRole = entity;
                return View();
            }
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddDepart()
        {
            string departNum = WebUtil.GetQueryStringValue<string>("departNum");
            if (departNum.IsEmpty())
            {
                ViewBag.SysDepart = new SysDepartEntity();
                return View();
            }
            else
            {
                DepartProvider provider = new DepartProvider();
                SysDepartEntity entity = provider.GetDepartEntity(departNum);
                entity = entity == null ? new SysDepartEntity() : entity;
                ViewBag.SysDepart = entity;
                return View();
            }
        }

        /// <summary>
        /// 账户设置
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AccountSetting()
        {
            AdminProvider provider = new AdminProvider();
            AdminEntity entity = provider.GetAdmin(this.LoginUser.UserCode);
            ViewBag.RoleList = BaseHelper.GetRoleList(this.LoginUser.RoleNum);
            ViewBag.DepartList = BaseHelper.GetDepartList(this.LoginUser.DepartNum);
            ViewBag.Admin = entity;
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult ChangePwd()
        {
            return View();
        }

        /// <summary>
        /// 切换仓库
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Change()
        {
            string storageNum = WebUtil.GetQueryStringValue<string>("storageNum", string.Empty);
            this.DefaultStore = storageNum;
            string url = this.ReferrerUrl;
            return Redirect(url);
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult LoginOut()
        {
            RemoveLogin();
            return Redirect("/");
        }

        /// <summary>
        /// 错误友好提示界面
        /// </summary>
        /// <returns></returns>
        [LoginFilter(false,false)]
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [LoginFilter(false, false)]
        public ActionResult Welcome()
        {
            return View();
        }
    }
}
