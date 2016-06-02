using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Storage.Entity.Report;
using Git.Storage.Provider.Report;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.ORM;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Provider;
using FastReport.Web;
using System.Data;
using System.Web.UI.WebControls;
using Git.Framework.Io;
using Git.Framework.Resource;

namespace Git.Storage.Web.Areas.Report.Controllers
{
    public class ManagerController : MasterPage
    {
        /// <summary>
        /// 新增报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] = null;
            ViewBag.ReportType = EnumHelper.GetOptions<EReportType>(string.Empty);
            ViewBag.DataSourceType = EnumHelper.GetOptions<EDataSourceType>(string.Empty);
            return View();
        }

        /// <summary>
        /// 添加参数对话框
        /// </summary>
        /// <returns></returns>
        public ActionResult AddParam()
        {
            ViewBag.ElementTypeList = EnumHelper.GetOptions<EElementType>(1);
            ViewBag.DataTypeList = BaseHelper.GetDataType(string.Empty);
            return View();
        }

        /// <summary>
        /// 编辑报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Edit()
        {
            string ReportNum = WebUtil.GetQueryStringValue<string>("ReportNum", string.Empty);
            Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] = null;
            if (ReportNum.IsEmpty())
            {
                return Redirect("/Report/Manager/List");
            }
            ReportProvider provider = new ReportProvider();
            ReportsEntity entity = provider.GetReport(ReportNum);
            if (entity.IsNull())
            {
                return Redirect("/Report/Manager/List");
            }
            ViewBag.Entity = entity;
            ViewBag.ReportType = EnumHelper.GetOptions<EReportType>(entity.ReportType);
            ViewBag.DataSourceType = EnumHelper.GetOptions<EDataSourceType>(entity.DsType);

            List<ReportParamsEntity> list = provider.GetParams(ReportNum);
            if (!list.IsNullOrEmpty())
            {
                Session[CacheKey.JOOSHOW_REPORTPARAM_CACHE] = list;
            }
            return View();
        }

        /// <summary>
        /// 报表列表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            ViewBag.ReportType = EnumHelper.GetOptions<EReportType>(string.Empty);
            return View();
        }

        /// <summary>
        /// 报表设计
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Designer()
        {
            string ReportNum = WebUtil.GetQueryStringValue<string>("ReportNum", string.Empty);
            ReportProvider provider = new ReportProvider();
            if (ReportNum.IsEmpty())
            {
                return Redirect("/Report/Manager/List");
            }
            ReportsEntity entity = provider.GetReport(ReportNum);
            if (entity.IsNull())
            {
                return Redirect("/Report/Manager/List");
            }
            List<ReportParamsEntity> list = provider.GetParams(ReportNum);

            WebReport webReport = new WebReport();
            webReport.Width = Unit.Percentage(100);
            webReport.Height = 600;
            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Black;
            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Black;
            webReport.PrintInBrowser = true;
            webReport.PrintInPdf = true;
            webReport.ShowExports = true;
            webReport.ShowPrint = true;
            webReport.SinglePage = true;
            DataSet ds = null;
            int orderType = 0;
            if (ReportNum == ResourceManager.GetSettingEntity("InOrder_Template").Value)
            {
                orderType = (int)EOrder.InOrder;
            }
            else if (ReportNum == ResourceManager.GetSettingEntity("OutOrder_Template").Value)
            {
                orderType = (int)EOrder.OutOrder;
            }
            ds = new ReportProvider().GetDataSource(entity, list, orderType, "");
            string path = Server.MapPath("~" + entity.FileName);
            if (!FileManager.FileExists(path))
            {
                string template = Server.MapPath("~/Theme/content/report/temp/Report.frx");
                System.IO.File.Copy(template, path, true);
            }
            webReport.Report.Load(path);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                webReport.Report.RegisterData(ds);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    webReport.Report.GetDataSource(ds.Tables[i].TableName).Enabled = true;
                }
            }
            webReport.DesignerPath = "~/WebReportDesigner/index.html";
            webReport.DesignReport = true;
            webReport.DesignScriptCode = true;
            webReport.DesignerSavePath = "~/Theme/content/report/temp/";
            webReport.DesignerSaveCallBack = "~/Report/Manager/SaveDesignedReport";
            webReport.ID = ReportNum;

            ViewBag.WebReport = webReport;
            return View();
        }

        /// <summary>
        /// 保存报表设计回调函数
        /// </summary>
        /// <param name="reportID"></param>
        /// <param name="reportUUID"></param>
        /// <returns></returns>
        public ActionResult SaveDesignedReport(string reportID, string reportUUID)
        {
            ReportProvider provider = new ReportProvider();
            if (reportID.IsEmpty())
            {
                return Redirect("/Report/Manager/List");
            }
            ReportsEntity entity = provider.GetReport(reportID);
            if (entity.IsNull())
            {
                return Redirect("/Report/Manager/List");
            }
            string FileRealPath = Server.MapPath("~" + entity.FileName);
            string FileTempPath = Server.MapPath("~/Theme/content/report/temp/" + reportUUID);
            FileManager.DeleteFile(FileRealPath);
            System.IO.File.Copy(FileTempPath, FileRealPath, true);
            return Content("");
        }

        /// <summary>
        /// 显示报表内容
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Show()
        {
            string ReportNum = WebUtil.GetQueryStringValue<string>("ReportNum", string.Empty);
            string orderNum = WebUtil.GetQueryStringValue<string>("OrderNum",string.Empty);
            ReportProvider provider = new ReportProvider();
            if (ReportNum.IsEmpty())
            {
                return Redirect("/Report/Manager/List");
            }
            ReportsEntity entity = provider.GetReport(ReportNum);
            if (entity.IsNull())
            {
                return Redirect("/Report/Manager/List");
            }
            List<ReportParamsEntity> list = provider.GetParams(ReportNum);
            list = list.IsNull() ? new List<ReportParamsEntity>() : list;
            string SearchValues = WebUtil.GetQueryStringValue<string>("SearchValues");
            SearchValues = SearchValues.UnEscapge();
            List<ReportParamsEntity> listParams = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReportParamsEntity>>(SearchValues);
            if (!listParams.IsNullOrEmpty())
            {
                foreach (ReportParamsEntity item in listParams)
                {
                    item.ParamName = item.ParamName.Replace("arg_", "@");
                    if (list.Exists(a => a.ParamName == item.ParamName))
                    {
                        list.First(a => a.ParamName == item.ParamName).DefaultValue = item.DefaultValue;
                    }
                }
            }
            ViewBag.Entity = entity;
            ViewBag.ListParam = list;

            WebReport webReport = new WebReport();
            webReport.Width = Unit.Percentage(100);
            webReport.Height = 600;
            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Black;
            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Black;
            webReport.PrintInBrowser = true;
            webReport.PrintInPdf = true;
            webReport.ShowExports = true;
            webReport.ShowPrint = true;
            webReport.SinglePage = true;

            DataSet ds = null;
            int orderType = 0;
            if (ReportNum == ResourceManager.GetSettingEntity("InOrder_Template").Value)
            {
                orderType = (int)EOrder.InOrder;
            }
            else if (ReportNum == ResourceManager.GetSettingEntity("OutOrder_Template").Value)
            {
                orderType = (int)EOrder.OutOrder;
            }
            ds = new ReportProvider().GetDataSource(entity, list, orderType, orderNum);
            string path = Server.MapPath("~" + entity.FileName);
            if (!FileManager.FileExists(path))
            {
                string template = Server.MapPath("~/Theme/content/report/temp/Report.frx");
                System.IO.File.Copy(template, path, true);
            }
            webReport.Report.Load(path);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                webReport.Report.RegisterData(ds);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    webReport.Report.GetDataSource(ds.Tables[i].TableName).Enabled = true;
                }
            }
            webReport.ID = ReportNum;
            ViewBag.WebReport = webReport;
            return View();
        }
    }
}
