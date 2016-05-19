using Git.Framework.Controller;
using Git.Storage.Entity.Report;
using Git.Storage.Provider;
using Git.Storage.Provider.Report;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Newtonsoft.Json;
using Git.Storage.Common;
using Git.Framework.ORM;
using Git.Framework.Io;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Report.Controllers
{
    public class ManagerAjaxController : AjaxPage
    {
        /// <summary>
        /// 创建报表
        /// 1000: 报表添加成功
        /// 1001: 报表添加失败
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Create()
        {
            ReportProvider provider = new ReportProvider();
            ReportsEntity oldEntity = null;

            ReportsEntity entity = WebUtil.GetFormObject<ReportsEntity>("entity", null);
            entity.CreateTime = DateTime.Now;
            entity.IsDelete = (int)EIsDelete.NotDelete;
            entity.Status = (int)EBool.No; //是否被禁用
            entity.FileName = entity.FileName.IsEmpty() ? "/Theme/content/report/" + Guid.NewGuid().ToString() + ".frx" : entity.FileName;
            List<ReportParamsEntity> listSource = Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] as List<ReportParamsEntity>;

            if (!entity.ReportNum.IsEmpty())
            {
                oldEntity = provider.GetReport(entity.ReportNum);
            }

            if (oldEntity != null)
            {
                if (oldEntity.FileName != entity.FileName)
                {
                    string FileRealPath = Server.MapPath("~" + oldEntity.FileName);
                    string FileTempPath = Server.MapPath("~" + entity.FileName);
                    FileManager.DeleteFile(FileRealPath);
                    System.IO.File.Copy(FileTempPath, FileRealPath, true);
                    entity.FileName = oldEntity.FileName;
                }
            }
            else
            {
                FileItem fileItem = FileManager.GetItemInfo(Server.MapPath("~" + entity.FileName));
                string FileRealPath = Server.MapPath("~/Theme/content/report/" + fileItem.Name);
                string FileTempPath = Server.MapPath("~" + entity.FileName);
                FileManager.MoveFile(FileTempPath, FileRealPath);
                entity.FileName = "/Theme/content/report/" + fileItem.Name;
            }
            int line = provider.Create(entity, listSource);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "操作成功");
            }
            else
            {
                this.ReturnJson.AddProperty("Key", "1001");
                this.ReturnJson.AddProperty("Value", "操作失败");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 加载报表对应参数
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult LoadParam()
        {
            List<ReportParamsEntity> list = Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] as List<ReportParamsEntity>;
            list = list.IsNull() ? new List<ReportParamsEntity>() : list;
            string json = JsonConvert.SerializeObject(list);
            this.ReturnJson.AddProperty("List", json);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 加载存储过程的元数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetMetadata()
        {
            ReportProvider provider = new ReportProvider();
            string ProceName = WebUtil.GetFormValue<string>("ProceName", string.Empty);
            List<ReportParamsEntity> list = provider.GetProceMetadata(ProceName);
            if (!list.IsNullOrEmpty())
            {
                Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] = list;
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddParam()
        {
            List<ReportParamsEntity> list = Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] as List<ReportParamsEntity>;
            list = list.IsNull() ? new List<ReportParamsEntity>() : list;

            ReportParamsEntity entity = WebUtil.GetFormObject<ReportParamsEntity>("entity",null);
            if (entity != null)
            {
                if (!list.Exists(a => a.ParamName == entity.ParamName))
                {
                    entity.ParamNum = SequenceProvider.GetSequence(typeof(ReportParamsEntity));
                    list.Add(entity);
                }
                Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] = list;
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 编辑存储过程元数据信息
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult EditMetadata()
        {
            List<ReportParamsEntity> list = WebUtil.GetFormObject<List<ReportParamsEntity>>("list", null);
            list = list.IsNull() ? new List<ReportParamsEntity>() : list;
            List<ReportParamsEntity> listSource = Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] as List<ReportParamsEntity>;
            listSource = listSource.IsNull() ? new List<ReportParamsEntity>() : listSource;

            foreach (ReportParamsEntity item in list)
            {
                if (listSource.Exists(a => a.ParamNum == item.ParamNum))
                {
                    ReportParamsEntity entity = listSource.First(a => a.ParamNum == item.ParamNum);
                    entity.ParamData = item.ParamData;
                    entity.DefaultValue = item.DefaultValue;
                    entity.ParamElement = item.ParamElement;
                    entity.ShowName = item.ShowName;
                }
            }
            Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] = listSource;
            this.ReturnJson.AddProperty("Key", "1000");
            this.ReturnJson.AddProperty("Value", "编辑成功");
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除参数
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DeleteParam()
        {
            List<ReportParamsEntity> list = Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] as List<ReportParamsEntity>;
            list = list.IsNull() ? new List<ReportParamsEntity>() : list;
            string ParamNum = WebUtil.GetFormValue<string>("ParamNum",string.Empty);
            list.Remove(a => a.ParamNum == ParamNum);
            Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] = list;
            return Content(string.Empty);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            int status = WebUtil.GetFormValue<int>("Status", 0);
            int ReportType = WebUtil.GetFormValue<int>("ReportType", -1);
            string ReportName = WebUtil.GetFormValue<string>("ReportName", string.Empty);
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            ReportsEntity entity = new ReportsEntity();
            ReportProvider provider = new ReportProvider();
            if (status > -1)
            {
                entity.Where(a => a.Status == status);
            }
            if (!ReportName.IsEmpty())
            {
                entity.Where("ReportName", ECondition.Like, "%" + ReportName + "%");
            }
            if (ReportType > 0)
            {
                entity.Where(a => a.ReportType == ReportType);
            }
            List<ReportsEntity> listResult = provider.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<ReportsEntity>() : listResult;
            string json = JsonConvert.SerializeObject(listResult);
            this.ReturnJson.AddProperty("Data", json);
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除报表
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete()
        {
            string ReportNum = WebUtil.GetFormValue<string>("ReportNum", string.Empty);
            ReportProvider provider = new ReportProvider();
            int line = provider.Delete(ReportNum);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "删除成功");
            }
            else
            {
                this.ReturnJson.AddProperty("Key", "1001");
                this.ReturnJson.AddProperty("Value", "删除失败");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteBatch()
        {
            List<string> list = WebUtil.GetFormObject<List<string>>("list", null);
            ReportProvider provider = new ReportProvider();
            int line = provider.Delete(list);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("Key", "1000");
                this.ReturnJson.AddProperty("Value", "删除成功");
            }
            else
            {
                this.ReturnJson.AddProperty("Key", "1001");
                this.ReturnJson.AddProperty("Value", "删除失败");
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}
