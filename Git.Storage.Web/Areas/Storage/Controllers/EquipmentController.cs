using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.Controller;
using Git.Storage.Entity.Store;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Provider.Store;
using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Storage.Web.Lib.Filter;

namespace Git.Storage.Web.Areas.Storage.Controllers
{
    public class EquipmentController : MasterPage
    {
        /// <summary>
        /// 添加或者编辑设备
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            string SnNum = WebUtil.GetQueryStringValue<string>("SnNum");
            if (SnNum.IsEmpty())
            {
                ViewBag.Status = EnumHelper.GetOptions<EEquipmentStatus>(string.Empty,"请选择");
                ViewBag.Equipment = new EquipmentEntity();
                return View();
            }
            else
            {
                EquipmentProvider provider = new EquipmentProvider();
                EquipmentEntity entity = provider.GetEquipment(SnNum);
                entity = entity == null ? new EquipmentEntity() : entity;
                ViewBag.Status = EnumHelper.GetOptions<EEquipmentStatus>(entity.Status, "请选择");
                ViewBag.Equipment = entity;
                return View();
            }
        }

        /// <summary>
        /// 设备管理列表页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            ViewBag.Status = EnumHelper.GetOptions<EEquipmentStatus>(string.Empty, "请选择");
            return View();
        }

        /// <summary>
        /// 设备授权
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Empower()
        {
            string SnNum = WebUtil.GetQueryStringValue<string>("SnNum");
            if (SnNum.IsEmpty())
            {
                ViewBag.Equipment = new EquipmentEntity();
                return View();
            }
            else
            {
                EquipmentProvider provider = new EquipmentProvider();
                EquipmentEntity entity = provider.GetEquipment(SnNum);
                entity = entity == null ? new EquipmentEntity() : entity;
                ViewBag.Equipment = entity;
                return View();
            }
        }


    }
}
