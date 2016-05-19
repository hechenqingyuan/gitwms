using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;
using Git.Framework.ORM;
using Git.Framework.Json;
using Git.Storage.Provider;
using Git.Framework.Controller.Mvc;
using Git.Storage.Common;
using System.Data;
using Git.Storage.Common.Excel;
using Git.Storage.Web.Lib.Filter;
using Git.Storage.Provider.Base;
namespace Git.Storage.Web.Areas.Storage.Controllers
{
    public class StoreAjaxController : AjaxPage
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 仓库管理列表页面
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 1);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 10);
            string StorageName = WebUtil.GetFormValue<string>("StorageName", string.Empty);
            int StorageType = WebUtil.GetFormValue<int>("StorageType", -1);
            int IsForbid = WebUtil.GetFormValue<int>("IsForbid", -1);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            StorageProvider provider = new StorageProvider();
            StorageEntity entity = new StorageEntity();
            if (IsForbid > -1)
            {
                entity.Where<StorageEntity>("IsForbid", ECondition.Eth, IsForbid);
            }
            if (StorageType > -1)
            {
                entity.Where<StorageEntity>("StorageType", ECondition.Eth, StorageType);
            }
            if (!StorageName.IsEmpty())
            {
                entity.Begin<StorageEntity>()
                    .Where<StorageEntity>("StorageName", ECondition.Like, "%" + StorageName + "%")
                    .Or<StorageEntity>("StorageNum", ECondition.Like, "%" + StorageName + "%")
                    .End<StorageEntity>()
                    ;
            }
            List<StorageEntity> listResult = provider.GetList(entity, ref pageInfo);
            string json = ConvertJson.ListToJson<StorageEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ToExcel()
        {
            string StorageName = WebUtil.GetFormValue<string>("StorageName", string.Empty);
            int StorageType = WebUtil.GetFormValue<int>("StorageType", -1);
            int IsForbid = WebUtil.GetFormValue<int>("IsForbid", -1);
            PageInfo pageInfo = new Git.Framework.DataTypes.PageInfo() { PageIndex = 1, PageSize = Int32.MaxValue };
            StorageProvider provider = new StorageProvider();
            StorageEntity entity = new StorageEntity();
            if (IsForbid > -1)
            {
                entity.Where<StorageEntity>("IsForbid", ECondition.Eth, IsForbid);
            }
            if (StorageType > -1)
            {
                entity.Where<StorageEntity>("StorageType", ECondition.Eth, StorageType);
            }
            if (!StorageName.IsEmpty())
            {
                entity.Begin<StorageEntity>()
                    .Where<StorageEntity>("StorageName", ECondition.Like, "%" + StorageName + "%")
                    .Or<StorageEntity>("StorageNum", ECondition.Like, "%" + StorageName + "%")
                    .End<StorageEntity>()
                    ;
            }
            List<StorageEntity> listResult = provider.GetList(entity, ref pageInfo);
            listResult = listResult.IsNull() ? new List<StorageEntity>() : listResult;
            if (listResult.IsNotNull())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号 "));
                dt.Columns.Add(new DataColumn("仓库编号 "));
                dt.Columns.Add(new DataColumn("仓库名 "));
                dt.Columns.Add(new DataColumn("仓库类型 "));
                dt.Columns.Add(new DataColumn("是否禁用"));
                dt.Columns.Add(new DataColumn("是否默认"));
                dt.Columns.Add(new DataColumn("备注"));
                dt.Columns.Add(new DataColumn("创建时间"));
                int count = 1;
                foreach (StorageEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.StorageNum;
                    row[2] = t.StorageName;
                    row[3] = EnumHelper.GetEnumDesc<EStorageType>(t.StorageType);
                    row[4] = EnumHelper.GetEnumDesc<EBool>(t.IsForbid);
                    row[5] = EnumHelper.GetEnumDesc<EBool>(t.IsDefault);
                    row[6] = t.Remark;
                    row[7] = t.CreateTime.ToString("yyyy-MM-dd");
                    dt.Rows.Add(row);
                    count++;
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("仓库管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("仓库管理", "仓库", System.IO.Path.Combine(filePath, filename));
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
        /// 添加
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Add([ModelBinder(typeof(JsonBinder<StorageEntity>))] StorageEntity entity)
        {
            StorageProvider provider = new StorageProvider();
            int line = 0;
            if (entity.StorageNum.IsEmpty())
            {
                entity.CreateTime = DateTime.Now;
                entity.StorageNum = entity.StorageNum.IsEmpty() ? SequenceProvider.GetSequence(typeof(StorageEntity)) : entity.StorageNum;
                entity.IsDelete = (int)EIsDelete.NotDelete;
                entity.IsForbid = (int)EBool.No;
                line = provider.Add(entity);
            }
            else
            {
                entity.Include(a => new { a.StorageName, a.StorageType, a.Length, a.Width, a.Height, a.Action, a.Status, a.IsDefault, a.Remark });
                entity.Where(a => a.StorageNum == entity.StorageNum);
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
        public ActionResult Delete(string StorageNum)
        {
            StorageProvider provider = new StorageProvider();
            int line = provider.Delete(StorageNum);
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
        /// 启用、禁用
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Audit(string StorageNum, string IsForbid)
        {
            StorageProvider provider = new StorageProvider();
            StorageEntity entity = new StorageEntity();
            entity.IncludeIsForbid(true);
            entity.IsForbid = ConvertHelper.ToType<int>(IsForbid);
            entity.Where(a => a.StorageNum == StorageNum);
            int line = provider.Update(entity);
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

      
       
    }
}
