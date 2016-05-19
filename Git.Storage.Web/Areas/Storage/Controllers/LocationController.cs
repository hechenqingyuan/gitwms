using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;
using Git.Framework.Json;
using Git.Storage.Provider;
using Git.Framework.Controller.Mvc;
using Git.Framework.ORM;
using Git.Storage.Common;
using Git.Storage.Web.Lib.Filter;
using Git.Framework.Resource;
using System.Text;
namespace Git.Storage.Web.Areas.Storage.Controllers
{
    public class LocationController : MasterPage
    {
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            ViewBag.StorageNumList = LocalHelper.GetStorageNumList(string.Empty);
            ViewBag.LocalTypeList = EnumHelper.GetOptions<ELocalType>(string.Empty);
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            string LocalNum = WebUtil.GetQueryStringValue<string>("LocalNum");
            if (LocalNum.IsEmpty())
            {
                ViewBag.Storage = LocalHelper.GetStorageNumList(string.Empty);
                ViewBag.LocalTypeList = EnumHelper.GetOptions<ELocalType>(string.Empty, "请选择库位类型");
                ViewBag.IsDefault = EnumHelper.GetOptions<EBool>(string.Empty, "请选择");
                ViewBag.Location = new LocationEntity();
            }
            else
            {
                LocationProvider provider = new LocationProvider();
                LocationEntity entity = provider.GetSingleByNum(LocalNum);
                entity = entity == null ? new LocationEntity() : entity;
                ViewBag.StorageType = EnumHelper.GetOptions<EStorageType>(entity.StorageType, "请选择仓库类型");
                ViewBag.IsDefault = EnumHelper.GetOptions<EBool>(entity.IsDefault, "请选择");
                ViewBag.Storage = LocalHelper.GetStorageNumList(entity.StorageNum);
                ViewBag.LocalTypeList = EnumHelper.GetOptions<ELocalType>(entity.LocalType, "请选择仓库类型");
                ViewBag.Location = entity;
            }
            return View();
        }

        /// <summary>
        /// 弹出对话框中显示所有的库位信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            ViewBag.LocalType = EnumHelper.GetOptions<ELocalType>(string.Empty, "请选择库位类型");
            return View();
        }

    }
}

