using Git.Framework.Controller;
using Git.Framework.Controller.Mvc;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Json;
using Git.Framework.ORM;
using Git.Storage.Common;
using Git.Storage.Common.Excel;
using Git.Storage.Entity.InStorage;
using Git.Storage.Provider;
using Git.Storage.Provider.InStorage;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.InStorage.Controllers
{
    public class ProductManagerAjaxController : AjaxPage
    {
        /// <summary>
        /// 分页查询入库单列表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            int status = WebUtil.GetFormValue<int>("Status", 0);
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string supName = WebUtil.GetFormValue<string>("SupName", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            int InType = WebUtil.GetFormValue<int>("InType", 0);
            string planNum = WebUtil.GetFormValue<string>("planNum", string.Empty);

            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            InStorageEntity entity = new InStorageEntity();
            if (status > 0)
            {
                entity.Where(a => a.Status == status);
            }
            if (!orderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + orderNum + "%");
            }
            if (!supName.IsEmpty())
            {
                entity.AndBegin<InStorageEntity>()
                    .And<InStorageEntity>("SupNum", ECondition.Like, "%" + supName + "%")
                    .Or<InStorageEntity>("SupName", ECondition.Like, "%" + supName + "%")
                    .End<InStorageEntity>()
                    ;
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("OrderTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            entity.And(a => a.StorageNum == this.DefaultStore);
            if (InType > 0)
            {
                entity.And(a => a.InType == InType);
            }
            if (!planNum.IsEmpty())
            {
                entity.And("ContractOrder", ECondition.Like, "%" + planNum + "%");
            }

            Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
            List<InStorageEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<InStorageEntity>() : listResult;
            string json = ConvertJson.ListToJson<InStorageEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", json);
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 根据订单编号删除订单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete()
        {
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            if (!orderNum.IsEmpty())
            {
                Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
                InStorageEntity entity = new InStorageEntity();
                entity.OrderNum = orderNum;
                string returnValue = bill.Delete(entity);
                this.ReturnJson.AddProperty("d", returnValue);
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除入库单
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
                    Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
                    InStorageEntity entity = new InStorageEntity();
                    entity.OrderNum = orderNum;
                    string returnValue = bill.Delete(entity);
                    this.ReturnJson.AddProperty("d", returnValue);
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 审核入库单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Audite()
        {
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int status = WebUtil.GetFormValue<int>("Status", 0);
            string reason = WebUtil.GetFormValue<string>("Reason", string.Empty);
            InStorageEntity entity = new InStorageEntity();
            entity.Status = status;
            entity.OrderNum = orderNum;
            entity.AuditUser = this.LoginUserCode;
            entity.OperateType = (int)EOpType.PC;
            entity.EquipmentCode = string.Empty;
            entity.EquipmentNum = string.Empty;
            entity.Remark = string.Empty;
            entity.Reason = reason;
            Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
            string returnValue = bill.Audite(entity);
            this.ReturnJson.AddProperty("d", returnValue);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult ToExcel()
        {
            int status = WebUtil.GetFormValue<int>("Status", 0);
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string supName = WebUtil.GetFormValue<string>("SupName", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);
            int pageSize = Int32.MaxValue;
            int pageIndex = 1;
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            InStorageEntity entity = new InStorageEntity();
            if (status > 0)
            {
                entity.Where(a => a.Status == status);
            }
            if (!orderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + orderNum + "%");
            }
            if (!supName.IsEmpty())
            {
                entity.AndBegin<InStorageEntity>()
                    .And<InStorageEntity>("SupNum", ECondition.Like, "%" + supName + "%")
                    .Or<InStorageEntity>("SupName", ECondition.Like, "%" + supName + "%")
                    .End<InStorageEntity>()
                    ;
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("OrderTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
            List<InStorageEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<InStorageEntity>() : listResult;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("入库单单号"));
            dt.Columns.Add(new DataColumn("入库类型"));
            dt.Columns.Add(new DataColumn("供应商"));
            dt.Columns.Add(new DataColumn("关联单号"));
            dt.Columns.Add(new DataColumn("货品总数"));
            dt.Columns.Add(new DataColumn("总金额"));
            dt.Columns.Add(new DataColumn("状态"));
            dt.Columns.Add(new DataColumn("制单人"));
            dt.Columns.Add(new DataColumn("操作方式"));
            dt.Columns.Add(new DataColumn("创建时间"));
            foreach (InStorageEntity t in listResult)
            {
                DataRow row = dt.NewRow();
                row[0] = t.OrderNum;
                row[1] = EnumHelper.GetEnumDesc<EInType>(t.InType);
                row[2] = t.SupName;
                row[3] = t.ContractOrder;
                row[4] = t.Num;
                row[5] = t.Amount.ToString("0.00")+" 元";
                row[6] = EnumHelper.GetEnumDesc<EAudite>(t.Status);
                row[7] = t.CreateUserName;
                row[8] = EnumHelper.GetEnumDesc<EOpType>(t.OperateType);
                row[9] = t.OrderTime.ToString("yyyy-MM-dd");
                dt.Rows.Add(row);
            }
            string filePath = Server.MapPath("~/UploadFiles/");
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string filename = string.Format("入库单{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            NPOIExcel excel = new NPOIExcel("入库管理", "入库单", System.IO.Path.Combine(filePath, filename));
            excel.ToExcel(dt);
            this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());

            return Content(this.ReturnJson.ToString());
        }
    }
}
