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
using Git.Framework.ORM;
using Git.Framework.Json;
using Git.Framework.Controller.Mvc;
using Git.Storage.Provider;
using Git.Storage.Common;
using Git.Storage.Web.Lib.Filter;

namespace Git.Storage.Web.Areas.Storage.Controllers
{
    public class StoreController : MasterPage
    {
        /// <summary>
        /// 仓库管理列表页面
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            ViewBag.IsDefaultList = EnumHelper.GetOptions<EBool>(string.Empty, "请选择");
            ViewBag.StroageType = EnumHelper.GetOptions<EStorageType>(string.Empty, "请选择");
            ViewBag.IsForbid = EnumHelper.GetOptions<EBool>(string.Empty, "请选择");
            return View();
        }

        /// <summary>
        /// 编辑和修改仓库页面-浮动层
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 新增、修改判断
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            string StorageNum = WebUtil.GetQueryStringValue<string>("StorageNum");
            if (StorageNum.IsEmpty())
            {
                ViewBag.StorageType = EnumHelper.GetOptions<EStorageType>(string.Empty, "请选择仓库类型");
                ViewBag.IsDefaultList = EnumHelper.GetOptions<EBool>(string.Empty, "请选择");
                ViewBag.StorageStatus = EnumHelper.GetOptions<EBool>(string.Empty, "请选择");
                ViewBag.Storage = new StorageEntity();
            }
            else
            {
                StorageProvider provider = new StorageProvider();
                StorageEntity entity = provider.GetSingleByNum(StorageNum);
                entity = entity == null ? new StorageEntity() : entity;
                ViewBag.StorageType = EnumHelper.GetOptions<EStorageType>(entity.StorageType, "请选择仓库类型");
                ViewBag.IsDefaultList = EnumHelper.GetOptions<EBool>(entity.IsDefault, "请选择");
                ViewBag.StorageStatus = EnumHelper.GetOptions<EBool>(entity.Status, "请选择");
                ViewBag.Storage = entity;
                
            }  
            return View();    
        }
    }
}
