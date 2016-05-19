using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Web.Lib.Filter;
using Git.Framework.Controller.Mvc;
using Git.Storage.Entity.Base;
using Git.Storage.Provider;
using Git.Storage.Provider.Base;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Framework.ORM;
using Git.Framework.Json;
using Storage.Common;
using Git.Storage.Common.Excel;
using System.Data;
using Git.Storage.Common;

namespace Git.Storage.Web.Controllers
{
    public class ResAjaxController : AjaxPage
    {
        /// <summary>
        /// 获得菜单列表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetMenuList(int pageIndex, int pageSize, string resName, string parentNum)
        {
            SysResourceProvider provider = new SysResourceProvider();
            List<SysResourceEntity> list = provider.GetList();
            if (!list.IsNullOrEmpty())
            {
                List<SysResourceEntity> listResult = list;
                if (!resName.IsEmpty())
                {
                    listResult = listResult.Where(a => a.ResName.Contains(resName)).ToList();
                }
                if (!parentNum.IsEmpty())
                {
                    listResult = listResult.Where(a => a.ParentNum.Contains(parentNum)).ToList();
                }
                List<SysResourceEntity> returnList = listResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.ID).ToList();
                this.ReturnJson.AddProperty("Data", ConvertJson.ListToJson<SysResourceEntity>(returnList, "List"));
                this.ReturnJson.AddProperty("RowCount", listResult.Count);
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddMenu([ModelBinder(typeof(JsonBinder<SysResourceEntity>))] SysResourceEntity entity)
        {
            SysResourceProvider provider = new SysResourceProvider();
            entity.UpdateTime = DateTime.Now;
            entity.ParentPath = "";
            entity.CreateUser = this.LoginUser.UserCode;
            entity.UpdateUser = this.LoginUser.UserCode;
            entity.CreateIp = "";
            entity.UpdateIp = "";
            entity.Remark = "";
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.IsHide = (int)EBool.No;
            int line = 0;
            if (entity.ResNum.IsEmpty())
            {
                entity.CreateTime = DateTime.Now;
                line = provider.AddResource(entity);
            }
            else
            {
                line = provider.UpdateResource(entity);
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
        /// 保存权限分配
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Save([ModelBinder(typeof(JsonBinder<List<string>>))] List<string> List,string roleNum)
        {
            PowerProvider provider = new PowerProvider();
            int line = provider.AllotPower(roleNum,List);
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
        /// 删除菜单
        /// </summary>
        /// <param name="resNum"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(string ResNum)
        {
            SysResourceProvider provider = new SysResourceProvider();
            int line = provider.DeleteResource(ResNum);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 获得分配的数据权限
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetTreeSource()
        {
            string RoleNum = WebUtil.GetFormValue<string>("RoleNum",this.LoginUser.RoleNum);
            PowerProvider provider = new PowerProvider();
            Params<List<SysResourceEntity>, List<SysResourceEntity>, List<SysResourceEntity>> param = provider.GetRole(RoleNum);
            List<SysResourceEntity> listAlloted = null;
            List<SysResourceEntity> listNotAlloted = null;
            List<SysResourceEntity> listSource = null;
            listSource = param.Item1;
            listAlloted = param.Item2;
            listNotAlloted = param.Item3;
            listSource = listSource.IsNull() ? new List<SysResourceEntity>() : listSource;
            listAlloted = listAlloted.IsNull() ? new List<SysResourceEntity>() : listAlloted;
            listNotAlloted = listNotAlloted.IsNull() ? new List<SysResourceEntity>() : listNotAlloted;
            this.ReturnJson.AddProperty("ListNotAlloted", ConvertJson.ListToJson<SysResourceEntity>(listNotAlloted));
            this.ReturnJson.AddProperty("ListAlloted", ConvertJson.ListToJson<SysResourceEntity>(listAlloted));
            return Content(this.ReturnJson.ToString());
        }
    }
}
