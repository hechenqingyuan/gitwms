using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.Log;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Entity.OutStorage;
using Git.Storage.Provider;
using Git.Storage.Provider.OutStorage;
using Git.Framework.Json;
using Git.Storage.Entity.Return;
using Git.Storage.Common;
using Git.Framework.Controller.Mvc;
using Git.Framework.Resource;
using Git.Storage.Provider.Returns;
using System.Data;
using Git.Storage.Common.Excel;

namespace Git.Storage.Web.Areas.Returns.Controllers
{
    public class ProductManagerAjaxController : AjaxPage
    {
        /// <summary>
        /// 退货管理
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            int status = WebUtil.GetFormValue<int>("Status", 0);
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            ReturnOrderEntity entity = new ReturnOrderEntity();
            if (status > 0)
            {
                entity.Where(a => a.Status == status);
            }
            if (!orderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + orderNum + "%");
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("OrderTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            entity.And(a => a.StorageNum == this.DefaultStore);
            Bill<ReturnOrderEntity, ReturnDetailEntity> bill = new ReturnOrder();
            List<ReturnOrderEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<ReturnOrderEntity>() : listResult;
            string json = ConvertJson.ListToJson<ReturnOrderEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", json);
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 退货审核
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Audit()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int Status = WebUtil.GetFormValue<int>("Status");
            string Reason = WebUtil.GetFormValue<string>("Reason", string.Empty);
            ReturnOrderEntity entity = new ReturnOrderEntity();
            entity.Status = Status;
            entity.OrderNum = OrderNum;
            entity.AuditUser = this.LoginUserCode;
            entity.OperateType = (int)EOpType.PC;
            entity.EquipmentCode = string.Empty;
            entity.EquipmentNum = string.Empty;
            entity.Remark = string.Empty;
            entity.Reason = Reason;
            Bill<ReturnOrderEntity, ReturnDetailEntity> bill = new ReturnOrder();
            string returnValue = bill.Audite(entity);
            this.ReturnJson.AddProperty("d", returnValue);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除退货单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DeleteBatch([ModelBinder(typeof(JsonBinder<List<string>>))] List<string> list)
        {
            if (!list.IsNullOrEmpty())
            {
                foreach (string orderNum in list)
                {
                    Bill<ReturnOrderEntity, ReturnDetailEntity> bill = new ReturnOrder();
                    ReturnOrderEntity entity = new ReturnOrderEntity();
                    entity.OrderNum = orderNum;
                    string returnValue = bill.Delete(entity);
                    this.ReturnJson.AddProperty("d", returnValue);
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除退货单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete()
        {
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            if (!orderNum.IsEmpty())
            {
                Bill<ReturnOrderEntity, ReturnDetailEntity> bill = new ReturnOrder();
                ReturnOrderEntity entity = new ReturnOrderEntity();
                entity.OrderNum = orderNum;
                string returnValue = bill.Delete(entity);
                this.ReturnJson.AddProperty("d", returnValue);
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 导出退货单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ToExcel()
        {
            int status = WebUtil.GetFormValue<int>("Status", 0);
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);
            int pageSize = Int32.MaxValue;
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            ReturnOrderEntity entity = new ReturnOrderEntity();
            if (status > 0)
            {
                entity.Where(a => a.Status == status);
            }
            if (!orderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + orderNum + "%");
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("OrderTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            entity.And(a => a.StorageNum == this.DefaultStore);
            Bill<ReturnOrderEntity, ReturnDetailEntity> bill = new ReturnOrder();
            List<ReturnOrderEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<ReturnOrderEntity>() : listResult;

            if (!listResult.IsNullOrEmpty())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("退货单"));
                dt.Columns.Add(new DataColumn("退货类型"));
                dt.Columns.Add(new DataColumn("退货数量"));
                dt.Columns.Add(new DataColumn("关联单号"));
                dt.Columns.Add(new DataColumn("状态"));
                dt.Columns.Add(new DataColumn("制单人"));
                dt.Columns.Add(new DataColumn("操作方式"));
                dt.Columns.Add(new DataColumn("创建时间"));
                foreach (ReturnOrderEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = t.OrderNum;
                    row[1] = EnumHelper.GetEnumDesc<ECheckType>(t.ReturnType);
                    row[2] = t.Num;
                    row[3] = t.ContractOrder;
                    row[4] = EnumHelper.GetEnumDesc<EAudite>(t.Status);
                    row[5] = t.CreateUserName;
                    row[6] = EnumHelper.GetEnumDesc<EOpType>(t.OperateType);
                    row[7] = t.CreateTime.ToString("yyyy-MM-dd");
                    dt.Rows.Add(row);
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("退货单{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("退货管理", "退货单", System.IO.Path.Combine(filePath, filename));
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
