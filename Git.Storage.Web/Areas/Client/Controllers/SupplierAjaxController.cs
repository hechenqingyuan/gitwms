using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Web.Lib;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Provider.Base;
using Git.Storage.Entity.Base;
using Git.Framework.ORM;
using Git.Framework.Json;
using Git.Framework.Controller.Mvc;
using Git.Storage.Provider;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Client;
using System.Text;
using Git.Storage.Common.Excel;
using System.Data;
using Storage.Common;
using Git.Storage.Common;
using Git.Storage.Web.Lib.Filter;

namespace Git.Storage.Web.Areas.Client.Controllers
{
    public class SupplierAjaxController : AjaxPage
    {
        /// <summary>
        /// 获得供应商列表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetSupplierList()
        {
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 1);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 15);
            string SupNum = WebUtil.GetFormValue<string>("SupNum", string.Empty);
            SupplierProvider provider = new SupplierProvider();
            SupplierEntity entity = new SupplierEntity();
            List<SupplierEntity> list = provider.GetList();
            if (!list.IsNullOrEmpty())
            {
                List<SupplierEntity> listResult = list.Where(a => a.SupNum.Contains(SupNum) || a.SupName.Contains(SupNum)).ToList();
                List<SupplierEntity> returnList = listResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.ID).ToList();
                string json = ConvertJson.ListToJson<SupplierEntity>(returnList, "List");
                this.ReturnJson.AddProperty("Data", new JsonObject(json));
                this.ReturnJson.AddProperty("RowCount", listResult.Count);
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddSupplier([ModelBinder(typeof(JsonBinder<SupplierEntity>))] SupplierEntity entity)
        {
            SupplierProvider provider = new SupplierProvider();
            entity.CreateTime = DateTime.Now;
            int line = 0;
            if (entity.SupNum.IsEmpty())
            {
                entity.SupNum = SequenceProvider.GetSequence(typeof(SupplierEntity));
                line = provider.AddSupplier(entity);
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
        /// 删除
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(string supNum)
        {
            SupplierProvider provider = new SupplierProvider();
            int line = provider.Delete(supNum);
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
        /// 自动提示供应商信息
        /// </summary>
        /// <param name="supName"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Auto(string supName)
        {
            SupplierProvider provider = new SupplierProvider();
            List<SupplierEntity> list = provider.GetList();
            if (!list.IsNullOrEmpty() && !supName.IsEmpty())
            {
                list = list.Where(a => a.SupNum.Contains(supName) || a.SupName.Contains(supName)).ToList();
            }
            list = list.IsNull() ? new List<SupplierEntity>() : list;
            StringBuilder sb = new StringBuilder();
            JsonObject jsonObject = null;
            foreach (SupplierEntity t in list)
            {
                jsonObject = new JsonObject();
                jsonObject.AddProperty("SupNum", t.SupNum);
                jsonObject.AddProperty("SupName", t.SupName);
                jsonObject.AddProperty("ContactName", t.ContactName);
                jsonObject.AddProperty("Phone", t.Phone);
                sb.Append(jsonObject.ToString() + "\n");
            }
            if (sb.Length == 0)
            {
                sb.Append("\n");
            }
            return Content(sb.ToString());
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult BatchDel(string SupNum)
        {
            SupplierProvider provider = new SupplierProvider();
            var list = SupNum.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
            string SupNum = WebUtil.GetFormValue<string>("SupNum", string.Empty);
            SupplierProvider provider = new SupplierProvider();
            SupplierEntity entity = new SupplierEntity();
            List<SupplierEntity> list = provider.GetList();
            if (!list.IsNullOrEmpty())
            {
                List<SupplierEntity> listResult = list.Where(a => a.SupNum.Contains(SupNum) || a.SupName.Contains(SupNum)).ToList();
                listResult = listResult.IsNull() ? new List<SupplierEntity>() : listResult;
                if (listResult.IsNotNull())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("序号 "));
                    dt.Columns.Add(new DataColumn("供应商编号"));
                    dt.Columns.Add(new DataColumn("供应商名称"));
                    dt.Columns.Add(new DataColumn("电话"));
                    dt.Columns.Add(new DataColumn("传真"));
                    dt.Columns.Add(new DataColumn("Email"));
                    dt.Columns.Add(new DataColumn("联系人"));
                    dt.Columns.Add(new DataColumn("地址"));
                    dt.Columns.Add(new DataColumn("描述"));
                    int count = 1;
                    foreach (SupplierEntity t in listResult)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = count;
                        row[1] = t.SupNum;
                        row[2] = t.SupName;
                        row[3] = t.Phone;
                        row[4] = t.Fax;
                        row[5] = t.Email;
                        row[6] = t.ContactName;
                        row[7] = t.Address;
                        row[8] = t.Description;
                        dt.Rows.Add(row);
                        count++;
                    }
                    string filePath = Server.MapPath("~/UploadFiles/");
                    if (!System.IO.Directory.Exists(filePath))
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Format("供应商管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    NPOIExcel excel = new NPOIExcel("供应商管理", "供应商", System.IO.Path.Combine(filePath, filename));
                    excel.ToExcel(dt);
                    this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
                }
                else
                {
                    this.ReturnJson.AddProperty("d", "无数据导出!")
                        ;
                }
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult doImportFile()
        {
            string msg = "";
            try
            {
                string filename = WebUtil.GetFormValue<string>("Url", string.Empty);
                var path = Server.MapPath(filename);
                var dataset = ExcelHelper.LoadDataFromExcel(path, "供应商管理$", "", "2007");
                List<SupplierEntity> list = new List<SupplierEntity>();

                //获取产品信息，
                GetProCatInfo(dataset, list);
                if (list.Count > 0)
                {
                    SupplierProvider provider = new SupplierProvider();
                    msg = provider.ImportProCateData(list);
                    this.ReturnJson.AddProperty("d", msg);
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                this.ReturnJson.AddProperty("d", "error");
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 从文件获取产品分类信息
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="list"></param>
        [LoginAjaxFilter]
        private static void GetProCatInfo(System.Data.DataSet dataset, List<SupplierEntity> list)
        {

            var rows = dataset.Tables[0].Rows;
            if (rows.Count > 2)
            {
                for (var i = 2; i < rows.Count; i++)
                {
                    SupplierEntity entity = new SupplierEntity();
                    var row = rows[i];
                    entity.SupNum = row[1].ToString();
                    if (string.IsNullOrEmpty(entity.SupNum)) continue;
                    entity.SupName = row[2].ToString();
                    entity.Phone = row[3].ToString();
                    entity.Fax = row[4].ToString();
                    entity.Email = row[5].ToString();
                    entity.ContactName = row[6].ToString();
                    entity.Address = row[7].ToString();
                    entity.Description = row[8].ToString();
                    entity.IsDelete = (int)EIsDelete.NotDelete;
                    entity.CreateTime = DateTime.Now;
                    list.Add(entity);
                }

            }
        }
    }
}
