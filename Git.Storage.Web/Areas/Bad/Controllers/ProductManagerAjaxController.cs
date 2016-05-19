using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Storage.Entity.Bad;
using Git.Storage.Provider;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.Json;
using Git.Storage.Provider.Bad;
using Git.Framework.Controller.Mvc;
using System.Data;
using Git.Storage.Common.Excel;

namespace Git.Storage.Web.Areas.Bad.Controllers
{
    public class ProductManagerAjaxController : AjaxPage
    {
        /// <summary>
        /// 审核报损单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Audit()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int Status = WebUtil.GetFormValue<int>("Status", 0);
            string Reason = WebUtil.GetFormValue<string>("Reason", string.Empty);
            BadReportEntity entity = new BadReportEntity();
            entity.Status = Status;
            entity.OrderNum = OrderNum;
            entity.AuditUser = this.LoginUserCode;
            entity.OperateType = (int)EOpType.PC;
            entity.EquipmentCode = string.Empty;
            entity.EquipmentNum = string.Empty;
            entity.Reason = Reason;
            Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
            string returnValue = bill.Audite(entity);
            this.ReturnJson.AddProperty("d", returnValue);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 加载报损单管理列表界面
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            int Status = WebUtil.GetFormValue<int>("Status", 0);
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string BadType = WebUtil.GetFormValue<string>("BadType", string.Empty);
            string ProductType = WebUtil.GetFormValue<string>("ProductType", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            BadReportEntity entity = new BadReportEntity();
            if (Status > 0)
            {
                entity.Where(a => a.Status == Status);
            }
            if (!OrderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + OrderNum + "%");
            }
            if (!ProductType.IsEmpty())
            {
                entity.Where("ProductType", ECondition.Eth, ProductType);
            }
            if (!BadType.IsEmpty())
            {
                entity.Where("BadType", ECondition.Eth, BadType);
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("CreateTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            entity.And(a => a.StorageNum == this.DefaultStore);
            Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
            List<BadReportEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<BadReportEntity>() : listResult;
            string json = ConvertJson.ListToJson<BadReportEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", json);
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
            int Status = WebUtil.GetFormValue<int>("Status", 0);
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string BadType = WebUtil.GetFormValue<string>("BadType", string.Empty);
            string ProductType = WebUtil.GetFormValue<string>("ProductType", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            BadReportEntity entity = new BadReportEntity();
            if (Status > 0)
            {
                entity.Where(a => a.Status == Status);
            }
            if (!OrderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + OrderNum + "%");
            }
            if (!ProductType.IsEmpty())
            {
                entity.Where("ProductType", ECondition.Eth, ProductType);
            }
            if (!BadType.IsEmpty())
            {
                entity.Where("BadType", ECondition.Eth, BadType);
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("CreateTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            entity.And(a => a.StorageNum == this.DefaultStore);
            Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
            List<BadReportEntity> listResult = bill.GetList(entity, ref pageInfo);
            if (!listResult.IsNullOrEmpty())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号 "));
                dt.Columns.Add(new DataColumn("单据编号"));
                dt.Columns.Add(new DataColumn("报损类型"));
                dt.Columns.Add(new DataColumn("关联单号"));
                dt.Columns.Add(new DataColumn("报损数量"));
                dt.Columns.Add(new DataColumn("状态"));
                dt.Columns.Add(new DataColumn("操作方式"));
                dt.Columns.Add(new DataColumn("创建人"));                
                dt.Columns.Add(new DataColumn("创建时间"));
                int count = 1;
                foreach (BadReportEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.OrderNum;
                    row[2] = EnumHelper.GetEnumDesc<EBadType>(t.BadType);
                    row[3] = t.ContractOrder;
                    row[4] = t.Num;
                    row[5] = EnumHelper.GetEnumDesc<EAudite>(t.Status);
                    row[6] = EnumHelper.GetEnumDesc<EOpType>(t.OperateType);
                    row[7] = t.CreateUser;
                    row[8] = t.CreateTime.ToString("yyyy-MM-dd");
                    dt.Rows.Add(row);
                    count++;
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("报损管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("报损管理", "报损单", System.IO.Path.Combine(filePath, filename));
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
        /// 批量删除
        /// </summary>
        /// <param name="OrderNumItems"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult BatchDel([ModelBinder(typeof(JsonBinder<string[]>))] string[] OrderNumItems)
        {
            Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
            string result = string.Empty;
            for (int i = 0; i < OrderNumItems.Length; i++)
            {
                BadReportEntity entity = new BadReportEntity();
                entity.OrderNum = OrderNumItems[i];
                result = bill.Delete(entity);
            }
            this.ReturnJson.AddProperty("d", result);
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete()
        {
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
            BadReportEntity entity = new BadReportEntity();
            entity.OrderNum = orderNum;
            string result = bill.Delete(entity);
            this.ReturnJson.AddProperty("d", result);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除报损单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DeleteBatch([ModelBinder(typeof(JsonBinder<List<string>>))] List<string> list)
        {
            if (!list.IsNullOrEmpty())
            {
                foreach (string orderNum in list)
                {
                    Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
                    BadReportEntity entity = new BadReportEntity();
                    entity.OrderNum = orderNum;
                    string result = bill.Delete(entity);
                    this.ReturnJson.AddProperty("d", result);
                }
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}
