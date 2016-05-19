using Git.Framework.Controller.Mvc;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Json;
using Git.Framework.ORM;
using Git.Storage.Common;
using Git.Storage.Entity.Base;
using Git.Storage.Provider.Base;
using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Provider;
using Git.Storage.Web.Lib.Filter;
using Git.Framework.Controller;
using System.Data;
using Git.Storage.Common.Excel;

namespace Git.Storage.Web.Controllers
{
    public class RoleAjaxController : AjaxPage
    {
        /// <summary>
        /// 角色信息分页
        /// 可以根据角色编号或者角色名称来搜索
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页行数</param>
        /// <param name="roleName">搜索关键字</param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult RoleList(int pageIndex, int pageSize, string roleName)
        {
            SysRoleProvider provider = new SysRoleProvider();
            List<SysRoleEntity> list = provider.GetList();
            List<SysRoleEntity> listResult = new List<SysRoleEntity>();
            List<SysRoleEntity> returnList = new List<SysRoleEntity>();
            if (!list.IsNullOrEmpty())
            {
                listResult = list.Where(a => a.RoleNum.Contains(roleName) || a.RoleName.Contains(roleName)).ToList();
                returnList = listResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.ID).ToList();
            }
            this.ReturnJson.AddProperty("Data", ConvertJson.ListToJson<SysRoleEntity>(returnList, "List"));
            this.ReturnJson.AddProperty("RowCount", listResult.Count);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加修改角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Add([ModelBinder(typeof(JsonBinder<SysRoleEntity>))] SysRoleEntity entity)
        {
            SysRoleProvider provider = new SysRoleProvider();
            if (entity.RoleNum.IsEmpty())
            {
                entity.RoleNum = SequenceProvider.GetSequence(typeof(SysRoleEntity));
                entity.IsDelete = (int)EIsDelete.NotDelete;
                entity.CreateTime = DateTime.Now;
                int line = provider.AddRole(entity);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            else
            {
                entity.IsDelete = (int)EIsDelete.NotDelete;
                int line = provider.UpdateRole(entity);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(string roleNum)
        {
            SysRoleProvider provider = new SysRoleProvider();
            int line = provider.DeleteRole(roleNum);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult BatchDel(string roleNum)
        {
            SysRoleProvider provider = new SysRoleProvider();
            string[] list = roleNum.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> ls = new List<string>();
            int line = 0;
            foreach (string t in list)
            {
                line += provider.DeleteRole(t);
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
            string roleName = WebUtil.GetFormValue<string>("roleName", string.Empty);
            SysRoleProvider provider = new SysRoleProvider();
            SysRoleEntity entity = new SysRoleEntity();
            List<SysRoleEntity> list = provider.GetList();
            List<SysRoleEntity> listResult = new List<SysRoleEntity>();
            if (!list.IsNullOrEmpty())
            {
                listResult = list.Where(a => a.RoleNum.Contains(roleName) || a.RoleName.Contains(roleName)).OrderByDescending(a => a.ID).ToList();
            }

            if (!listResult.IsNullOrEmpty())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号"));
                dt.Columns.Add(new DataColumn("角色编号"));
                dt.Columns.Add(new DataColumn("角色名"));
                dt.Columns.Add(new DataColumn("创建时间"));
                dt.Columns.Add(new DataColumn("备注"));
                int count = 1;
                foreach (SysRoleEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.RoleNum;
                    row[2] = t.RoleName;
                    row[3] = t.CreateTime;
                    row[4] = t.Remark;
                    count++;
                    dt.Rows.Add(row);
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("角色管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                
                NPOIExcel excel = new NPOIExcel("角色管理", "角色", System.IO.Path.Combine(filePath, filename));
                excel.ToExcel(dt);
                this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
            }
            else
            {
                this.ReturnJson.AddProperty("d", "无数据导出!")
                    ;
            }

            return Content(this.ReturnJson.ToString());
        }

    }
}
