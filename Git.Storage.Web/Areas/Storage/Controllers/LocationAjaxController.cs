using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Provider.Store;
using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Entity.Store;
using Git.Framework.ORM;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Provider;
using Git.Framework.Json;
using Git.Framework.Controller.Mvc;
using Git.Storage.Common;
using System.Data;
using Git.Storage.Common.Excel;
using Git.Storage.Web.Lib.Filter;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Storage.Controllers
{
    public class LocationAjaxController : AjaxPage
    {

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 库位管理列表页面
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 1);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 15);
            string LocalName = WebUtil.GetFormValue<string>("LocalName", string.Empty);
            string LocalType = WebUtil.GetFormValue<string>("LocalType", string.Empty);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            LocationProvider provider = new LocationProvider();
            LocationEntity entity = new LocationEntity();
            StorageEntity SEntity = new StorageEntity();
            SEntity.Include(a => new { StorageName = a.StorageName });
            entity.Left<StorageEntity>(SEntity, new Params<string, string>() { Item1 = "StorageNum", Item2 = "StorageNum" });
            string StorageNum = this.DefaultStore;
            entity.Where<LocationEntity>("StorageNum", ECondition.Eth, StorageNum);
            if (!LocalName.IsEmpty())
            {
                entity.Begin<LocationEntity>()
                    .Where<LocationEntity>("LocalName", ECondition.Like, "%" + LocalName + "%")
                    .Or<LocationEntity>("LocalNum", ECondition.Like, "%" + LocalName + "%")
                    .Or<LocationEntity>("LocalBarCode", ECondition.Like, "%" + LocalName + "%")
                    .End<LocationEntity>()
                    ;
            }
            if (!LocalType.IsEmpty())
            {
                entity.Where<LocationEntity>("LocalType", ECondition.Eth, LocalType);
            }

            List<LocationEntity> listResult = provider.GetList(entity, ref pageInfo);
            string json = ConvertJson.ListToJson<LocationEntity>(listResult, "List");
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
            PageInfo pageInfo = new Git.Framework.DataTypes.PageInfo() { PageIndex = 1, PageSize = Int32.MaxValue };
            string StorageNum = WebUtil.GetFormValue<string>("StorageName", string.Empty);
            string LocalName = WebUtil.GetFormValue<string>("LocalName", string.Empty);
            string LocalType = WebUtil.GetFormValue<string>("LocalType", string.Empty);
            LocationProvider provider = new LocationProvider();
            LocationEntity entity = new LocationEntity();

            StorageEntity SEntity = new StorageEntity();
            SEntity.Include(a => new { StorageName = a.StorageName });
            entity.Left<StorageEntity>(SEntity, new Params<string, string>() { Item1 = "StorageNum", Item2 = "StorageNum" });
            if (!StorageNum.IsEmpty())
            {
                entity.Where<LocationEntity>("StorageNum", ECondition.Like, "%" + StorageNum + "%");
            }
            if (!LocalName.IsEmpty())
            {
                entity.Begin<LocationEntity>()
                    .Where<LocationEntity>("LocalName", ECondition.Like, "%" + LocalName + "%")
                    .Or<LocationEntity>("LocalNum", ECondition.Like, "%" + LocalName + "%")
                    .Or<LocationEntity>("LocalBarCode", ECondition.Like, "%" + LocalName + "%")
                    .End<LocationEntity>()
                    ;
            }
            if (!LocalType.IsEmpty())
            {
                entity.Where<LocationEntity>("LocalType", ECondition.Like, "%" + LocalType + "%");
            }
            List<LocationEntity> listResult = provider.GetList(entity, ref pageInfo);
            listResult = listResult.IsNull() ? new List<LocationEntity>() : listResult;
            if (listResult.IsNotNull())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号 "));
                dt.Columns.Add(new DataColumn("库位编号 "));
                dt.Columns.Add(new DataColumn("库位名 "));
                dt.Columns.Add(new DataColumn("所在仓库 "));
                dt.Columns.Add(new DataColumn("库位类型"));
                dt.Columns.Add(new DataColumn("是否禁用"));
                dt.Columns.Add(new DataColumn("是否默认"));
                dt.Columns.Add(new DataColumn("创建时间"));
                int count = 1;
                foreach (LocationEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.LocalNum;
                    row[2] = t.LocalName;
                    row[3] = t.StorageName;
                    row[4] = EnumHelper.GetEnumDesc<ELocalType>(t.LocalType);
                    row[5] = EnumHelper.GetEnumDesc<EBool>(t.IsForbid);
                    row[6] = EnumHelper.GetEnumDesc<EBool>(t.IsDefault);
                    row[7] = t.CreateTime.ToString("yyyy-MM-dd");
                    dt.Rows.Add(row);
                    count++;
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("库位管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("库位管理", "库位", System.IO.Path.Combine(filePath, filename));
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
        public ActionResult Add([ModelBinder(typeof(JsonBinder<LocationEntity>))] LocationEntity entity)
        {
            LocationProvider provider = new LocationProvider();
            if (entity.LocalNum.IsEmpty())
            {
                entity.LocalNum = SequenceProvider.GetSequence(typeof(LocationEntity));
                entity.LocalBarCode = entity.LocalBarCode.IsEmpty() ? entity.LocalNum : entity.LocalBarCode;
                entity.IsForbid = (int)EBool.No;
                entity.IsDelete = (int)EIsDelete.NotDelete;
                entity.UnitNum = "";
                entity.UnitName = "";
                entity.CreateTime = DateTime.Now;
                int line = provider.Add(entity);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            else
            {
                entity.LocalBarCode = entity.LocalBarCode.IsEmpty() ? entity.LocalNum : entity.LocalBarCode;
                entity.Include(a => new { a.LocalName, a.LocalBarCode, a.StorageNum, a.LocalType, a.IsDefault });
                entity.Where(a => a.LocalNum == entity.LocalNum);
                int line = provider.Update(entity);
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(string LocalNum)
        {
            LocationProvider provider = new LocationProvider();
            int line = provider.Delete(LocalNum);
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
        public ActionResult Audit(string LocalNum, string IsForbid)
        {
            LocationProvider provider = new LocationProvider();
            LocationEntity entity = new LocationEntity();
            entity.IncludeIsForbid(true);
            entity.IsForbid = ConvertHelper.ToType<int>(IsForbid);
            entity.Where(a => a.LocalNum == LocalNum);
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
