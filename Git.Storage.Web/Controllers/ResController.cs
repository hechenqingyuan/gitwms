using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Web.Lib.Filter;
using Git.Framework.Controller;
using Git.Storage.Entity.Base;
using Git.Storage.Provider.Base;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.DataTypes;
using Git.Storage.Common;

namespace Git.Storage.Web.Controllers
{
    public class ResController : MasterPage
    {
        /// <summary>
        /// 菜单管理页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Index()
        {
            ViewBag.RoleList = BaseHelper.GetParentMenu(string.Empty);
            return View();
        }

        /// <summary>
        /// 权限分配
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Power()
        {
            string roleNum = WebUtil.GetQueryStringValue<string>("roleNum",string.Empty);
            if (roleNum.IsEmpty())
            {
                roleNum=this.LoginUser.RoleNum;
            }
            if (roleNum.IsEmpty())
            {
                return Redirect("/Home/RoleList");
            }
            ViewBag.RoleNum = roleNum;
            //SysRoleProvider roleProvider = new SysRoleProvider();
            //SysRoleEntity entity = roleProvider.GetRoleEntity(roleNum);
            //if (entity != null)
            //{
            //    ViewBag.RoleName = entity.RoleName;
            //}
            //PowerProvider provider = new PowerProvider();
            //Params<List<SysResourceEntity>, List<SysResourceEntity>, List<SysResourceEntity>> param=provider.GetRole(roleNum);
            //List<SysResourceEntity> listAlloted = null;
            //List<SysResourceEntity> listNotAlloted = null;
            //List<SysResourceEntity> listSource = null;
            //listSource = param.Item1;
            //listAlloted = param.Item2;
            //listNotAlloted = param.Item3;
            //listSource = listSource.IsNull() ? new List<SysResourceEntity>() : listSource;
            //listAlloted = listAlloted.IsNull() ? new List<SysResourceEntity>() : listAlloted;
            //listNotAlloted = listNotAlloted.IsNull() ? new List<SysResourceEntity>() : listNotAlloted;
            //ViewBag.ListSource = listSource;
            //ViewBag.ListAlloted = listAlloted;
            //ViewBag.ListNotAlloted = listNotAlloted;
            return View();
        }

        /// <summary>
        /// 系统日志
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult SysLog()
        {
            return View();
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult AddMenu()
        {
            string resNum = WebUtil.GetQueryStringValue<string>("resNum");
            
            if (resNum.IsEmpty())
            {
                ViewBag.RoleList = BaseHelper.GetParentMenu(string.Empty);
                SysResourceEntity entity = new SysResourceEntity();
                ViewBag.Menu = entity;
                ViewBag.ResourceType = EnumHelper.GetOptions<EResourceType>(EResourceType.Page,"请选择菜单类型");
                return View();
            }
            else
            {
                SysResourceProvider provider = new SysResourceProvider();
                SysResourceEntity entity = provider.GetResource(resNum);
                entity = entity == null ? new SysResourceEntity() : entity;
                ViewBag.RoleList = BaseHelper.GetParentMenu(entity.ParentNum);
                ViewBag.Menu = entity;
                ViewBag.ResourceType = EnumHelper.GetOptions<EResourceType>(entity.ResType, "请选择菜单类型");
                return View();
            }
        }
    }
}
