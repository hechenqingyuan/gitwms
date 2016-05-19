using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Controller;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;
using Git.Framework.Json;
using Git.Framework.Controller.Mvc;
using Git.Storage.Provider;
using Git.Storage.Common;
using Git.Storage.Web.Lib.Filter;
using Git.Framework.ORM;
using Git.Framework.DataTypes;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Storage.Controllers
{
    public class EquipmentAjaxController : AjaxPage
    {
        /// <summary>
        /// 获得设备列表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetEquipmentList()
        {
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 1);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 15);
            string EquipmentName = WebUtil.GetFormValue<string>("EquipmentName", string.Empty);
            int Status = WebUtil.GetFormValue<int>("Status", 0);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            EquipmentProvider provider = new EquipmentProvider();
            EquipmentEntity entity = new EquipmentEntity();
            if (!EquipmentName.IsEmpty())
            {
                entity.Begin<EquipmentEntity>()
                   .Where<EquipmentEntity>("SnNum", ECondition.Like, "%" + EquipmentName + "%")
                   .Or<EquipmentEntity>("EquipmentName", ECondition.Like, "%" + EquipmentName + "%")
                   .End<EquipmentEntity>();
            }
            if (Status != 0)
            {
                entity.Where<EquipmentEntity>(a => a.Status == Status);
            }
            List<EquipmentEntity> list = provider.GetList(entity, ref pageInfo);
            string json = ConvertJson.ListToJson<EquipmentEntity>(list, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", list.Count);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddEquipment([ModelBinder(typeof(JsonBinder<EquipmentEntity>))] EquipmentEntity entity)
        {
            EquipmentProvider provider = new EquipmentProvider();
            entity.CreateTime = DateTime.Now;
            int line = 0;
            if (entity.SnNum.IsEmpty())
            {
                string barcode = SequenceProvider.GetSequence(typeof(EquipmentEntity));
                entity.SnNum = barcode;
                entity.EquipmentNum = barcode;
                entity.CreateUser = this.LoginUserCode;
                line = provider.AddEquipment(entity);
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
        /// <param name="snNum"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(string snNum)
        {
            EquipmentProvider provider = new EquipmentProvider();
            int line = provider.Delete(snNum);
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
