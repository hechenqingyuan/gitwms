using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Entity.Move;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Provider.Move;
using Git.Storage.Provider;
using Git.Framework.Json;
using Git.Storage.Common;
using System.Data;
using Git.Storage.Common.Excel;
using Git.Framework.Controller.Mvc;

namespace Git.Storage.Web.Areas.Move.Controllers
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
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int status = WebUtil.GetFormValue<int>("Status", 0);
            string reason = WebUtil.GetFormValue<string>("Reason", string.Empty);
            MoveOrderEntity entity = new MoveOrderEntity();
            entity.Status = status;
            entity.OrderNum = orderNum;
            entity.AuditUser = this.LoginUserCode;
            entity.OperateType = (int)EOpType.PC;
            entity.EquipmentCode = string.Empty;
            entity.EquipmentNum = string.Empty;
            entity.Remark = string.Empty;
            entity.Reason = reason;
            Bill<MoveOrderEntity, MoveOrderDetailEntity> bill = new MoveOrder();
            string returnValue = bill.Audite(entity);
            this.ReturnJson.AddProperty("d", returnValue);
            return Content(this.ReturnJson.ToString());
        }


        /// <summary>
        /// 移库管理分页查询
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
            MoveOrderEntity entity = new MoveOrderEntity();
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
            Bill<MoveOrderEntity, MoveOrderDetailEntity> bill = new MoveOrder();
            List<MoveOrderEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<MoveOrderEntity>() : listResult;
            string json = ConvertJson.ListToJson<MoveOrderEntity>(listResult, "List");
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
            PageInfo pageInfo = new PageInfo() { PageIndex = 1, PageSize = int.MaxValue };
            MoveOrderEntity entity = new MoveOrderEntity();
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
            Bill<MoveOrderEntity, MoveOrderDetailEntity> bill = new MoveOrder();
            List<MoveOrderEntity> listResult = bill.GetList(entity, ref pageInfo);
            if (!listResult.IsNullOrEmpty())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号 "));
                dt.Columns.Add(new DataColumn("移库单编号 "));
                dt.Columns.Add(new DataColumn("移库类型"));
                dt.Columns.Add(new DataColumn("关联单号"));
                dt.Columns.Add(new DataColumn("移库总数"));
                dt.Columns.Add(new DataColumn("移库人"));
                dt.Columns.Add(new DataColumn("状态"));
                dt.Columns.Add(new DataColumn("创建时间"));
                int count = 1;
                foreach (MoveOrderEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.OrderNum;
                    row[2] = EnumHelper.GetEnumDesc<EMoveType>(t.MoveType); 
                    row[3] = t.ContractOrder;
                    row[4] = t.Num;
                    row[5] = t.CreateUserName;
                    row[6] = EnumHelper.GetEnumDesc<EAudite>(t.Status);
                    row[7] = t.CreateTime.ToString("yyyy-MM-dd");
                    dt.Rows.Add(row);
                    count++;
                }

                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("移库管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("移库管理", "移库单", System.IO.Path.Combine(filePath, filename));
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
        /// 根据单号删除移库单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete()
        {
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            if (!orderNum.IsEmpty())
            {
                Bill<MoveOrderEntity, MoveOrderDetailEntity> bill = new MoveOrder();
                MoveOrderEntity entity = new MoveOrderEntity();
                entity.OrderNum = orderNum;
                string returnValue = bill.Delete(entity);
                this.ReturnJson.AddProperty("d", returnValue);
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除移库单
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
                    Bill<MoveOrderEntity, MoveOrderDetailEntity> bill = new MoveOrder();
                    MoveOrderEntity entity = new MoveOrderEntity();
                    entity.OrderNum = orderNum;
                    string returnValue = bill.Delete(entity);
                    this.ReturnJson.AddProperty("d", returnValue);
                }
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}
