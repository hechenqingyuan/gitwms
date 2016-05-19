using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Entity.Base;
using Git.Storage.Provider.Base;
using Git.Storage.Web.Lib;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Json;
using Git.Framework.Controller.Mvc;
using Git.Storage.Provider;
using Git.Storage.Common;
using Git.Storage.Web.Lib.Filter;
using Git.Framework.DataTypes;
using Git.Framework.Controller;
using System.Data;
using Git.Storage.Common.Excel;

namespace Git.Storage.Web.Controllers
{
    public class DepartAjaxController : AjaxPage
    {
        // <summary>
        /// 获得部门信息
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DepartList(int pageIndex, int pageSize, string departName)
        {
            DepartProvider provider = new DepartProvider();
            List<SysDepartEntity> list = provider.GetList();
            List<SysDepartEntity> listResult = new List<SysDepartEntity>();
            List<SysDepartEntity> returnList = new List<SysDepartEntity>();
            if (!list.IsNullOrEmpty())
            {
                listResult = list.Where(a => a.DepartName.Contains(departName) || a.DepartNum.Contains(departName)).ToList();
                returnList = listResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.ID).ToList();
            }
            this.ReturnJson.AddProperty("Data", ConvertJson.ListToJson<SysDepartEntity>(returnList, "List"));
            this.ReturnJson.AddProperty("RowCount", listResult.Count);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddDepart([ModelBinder(typeof(JsonBinder<SysDepartEntity>))] SysDepartEntity entity)
        {
            DepartProvider provider = new DepartProvider();
            int line = 0;
            if (entity.DepartNum.IsEmpty())
            {
                entity.DepartNum = SequenceProvider.GetSequence(typeof(SysDepartEntity));
                entity.IsDelete = (int)EIsDelete.NotDelete;
                entity.CreateTime = DateTime.Now;
                line = provider.Add(entity);
            }
            else
            {
                line = provider.UpdateDepart(entity);
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
        /// 删除部门
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(int id)
        {
            DepartProvider provider = new DepartProvider();
            int line = provider.DeleteDepart(id);
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
        public ActionResult BatchDel(string id)
        {
            DepartProvider provider = new DepartProvider();
            string[] list = id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> ls = new List<string>();
            int line = 0;
            foreach (string t in list)
            {
                line += provider.DeleteDepart(Convert.ToInt32(t));
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
            string departName = WebUtil.GetFormValue<string>("departName", string.Empty);
            DepartProvider provider = new DepartProvider();
            SysDepartEntity entity = new SysDepartEntity();
            List<SysDepartEntity> list = provider.GetList();
            List<SysDepartEntity> listResult = new List<SysDepartEntity>();
            if (!list.IsNullOrEmpty())
            {
                listResult = list.Where(a => a.DepartName.Contains(departName) || a.DepartNum.Contains(departName)).OrderByDescending(a => a.ID).ToList();
            }

            if (!listResult.IsNullOrEmpty())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号"));
                dt.Columns.Add(new DataColumn("部门编号"));
                dt.Columns.Add(new DataColumn("部门名"));
                dt.Columns.Add(new DataColumn("创建时间"));
                int count = 1;
                foreach (SysDepartEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.DepartNum;
                    row[2] = t.DepartName;
                    row[3] = t.CreateTime;
                    count++;
                    dt.Rows.Add(row);
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("部门管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("部门管理", "部门", System.IO.Path.Combine(filePath, filename));
                excel.ToExcel(dt);
                this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
            }
            else
            {
                this.ReturnJson.AddProperty("d", "无数据导出!");
            }

            return Content(this.ReturnJson.ToString());
        }
    }
}
