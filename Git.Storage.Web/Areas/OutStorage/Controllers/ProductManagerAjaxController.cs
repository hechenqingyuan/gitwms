using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Web.Lib.Filter;
using Git.Framework.Controller;
using Git.Storage.Entity.OutStorage;
using Git.Storage.Provider.OutStorage;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Provider;
using Git.Framework.Json;
using Git.Storage.Common;
using Git.Storage.Common.Excel;
using System.Data;
using Git.Storage.Provider.Order;
using Git.Framework.Controller.Mvc;

namespace Git.Storage.Web.Areas.OutStorage.Controllers
{
    public class ProductManagerAjaxController : AjaxPage
    {
        /// <summary>
        /// 审核出库单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Audit()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int Status = WebUtil.GetFormValue<int>("Status");
            string Reason = WebUtil.GetFormValue<string>("Reason", string.Empty);
            OutStorageEntity entity = new OutStorageEntity();
            entity.Status = Status;
            entity.OrderNum = OrderNum;
            entity.AuditUser = this.LoginUserCode;
            entity.OperateType = (int)EOpType.PC;
            entity.EquipmentCode = string.Empty;
            entity.EquipmentNum = string.Empty;
            entity.Remark = string.Empty;
            entity.Reason = Reason;
            Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
            string returnValue = bill.Audite(entity);
            this.ReturnJson.AddProperty("d", returnValue);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 加载出库单管理列表界面
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            int Status = WebUtil.GetFormValue<int>("Status", 0);
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string CusName = WebUtil.GetFormValue<string>("CusName", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);
            string order = WebUtil.GetFormValue<string>("order", string.Empty);
            int OutType = WebUtil.GetFormValue<int>("OutType", 0);
            string planNum = WebUtil.GetFormValue<string>("planNum");
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            OutStorageEntity entity = new OutStorageEntity();
            if (Status > 0)
            {
                entity.Where(a => a.Status == Status);
            }
            if (!OrderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + OrderNum + "%");
            }
            if (!CusName.IsEmpty())
            {
                entity.Begin<OutStorageEntity>()
                    .And<OutStorageEntity>("CusNum", ECondition.Like, "%" + CusName + "%")
                    .Or<OutStorageEntity>("CusName", ECondition.Like, "%" + CusName + "%")
                    .End<OutStorageEntity>()
                    ;
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("OrderTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            entity.And(a => a.StorageNum == this.DefaultStore);
            if (!order.IsEmpty())
            {
                OrderProvider orderProvider = new OrderProvider();
                List<string> listContractOrder = orderProvider.GetOrderPlan(order);
                listContractOrder = listContractOrder.IsNull() ? new List<string>() : listContractOrder;
                if (listContractOrder.Count == 0)
                {
                    listContractOrder.Add(order);
                }
                entity.And("ContractOrder", ECondition.In, listContractOrder.ToArray());
            }
            if (OutType > 0)
            {
                entity.And(a => a.OutType == OutType);
            }
            if (!planNum.IsEmpty())
            {
                OutStoDetailEntity detail = new OutStoDetailEntity();
                detail.Where("ContractOrder", ECondition.Like, "%" + planNum + "%");
                entity.Left<OutStoDetailEntity>(detail, new Params<string, string>() { Item1 = "OrderNum", Item2 = "OrderNum" });
            }
            Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
            List<OutStorageEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<OutStorageEntity>() : listResult;
            string json = ConvertJson.ListToJson<OutStorageEntity>(listResult, "List");
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
            string CusName = WebUtil.GetFormValue<string>("CusName", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);

            PageInfo pageInfo = new PageInfo() { PageIndex = 1, PageSize = int.MaxValue };
            OutStorageEntity entity = new OutStorageEntity();
            if (Status > 0)
            {
                entity.Where(a => a.Status == Status);
            }
            if (!OrderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + OrderNum + "%");
            }
            if (!CusName.IsEmpty())
            {
                entity.Begin<OutStorageEntity>()
                    .And<OutStorageEntity>("CusNum", ECondition.Like, "%" + CusName + "%")
                    .Or<OutStorageEntity>("CusName", ECondition.Like, "%" + CusName + "%")
                    .End<OutStorageEntity>()
                    ;
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("OrderTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            entity.And(a => a.StorageNum == this.DefaultStore);
            Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
            List<OutStorageEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<OutStorageEntity>() : listResult;
            if (listResult.IsNotNull())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号 "));
                dt.Columns.Add(new DataColumn("出库单编号 "));
                dt.Columns.Add(new DataColumn("出库类型"));
                dt.Columns.Add(new DataColumn("客户名称"));
                dt.Columns.Add(new DataColumn("关联单号"));
                dt.Columns.Add(new DataColumn("总数量"));
                dt.Columns.Add(new DataColumn("总金额"));
                dt.Columns.Add(new DataColumn("状态"));
                dt.Columns.Add(new DataColumn("制单人"));
                dt.Columns.Add(new DataColumn("操作方式"));
                dt.Columns.Add(new DataColumn("创建时间"));
                int count = 1;
                foreach (OutStorageEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.OrderNum;
                    row[2] = EnumHelper.GetEnumDesc<EOutType>(t.OutType);
                    row[3] = t.CusName;
                    row[4] = t.ContractOrder;
                    row[5] = t.Num;
                    row[6] = t.Amount;
                    row[7] = EnumHelper.GetEnumDesc<EAudite>(t.Status);
                    row[8] = t.CreateUserName;
                    row[9] = EnumHelper.GetEnumDesc<EOpType>(t.OperateType);
                    row[10] = t.CreateTime.ToString("yyyy-MM-dd");
                    dt.Rows.Add(row);
                    count++;
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("出库管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("出库管理", "出库单", System.IO.Path.Combine(filePath, filename));
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
        /// 删除出库单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            OutStorageEntity entity = new OutStorageEntity();
            entity.OrderNum = OrderNum;
            Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
            string returnValue = bill.Delete(entity);
            this.ReturnJson.AddProperty("d", returnValue);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除出库单
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
                    OutStorageEntity entity = new OutStorageEntity();
                    entity.OrderNum = orderNum;
                    Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
                    string returnValue = bill.Delete(entity);
                    this.ReturnJson.AddProperty("d", returnValue);
                }
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}
