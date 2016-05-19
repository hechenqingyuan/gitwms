using Git.Framework.Controller;
using Git.Framework.Controller.Mvc;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Json;
using Git.Framework.ORM;
using Git.Framework.Resource;
using Git.Storage.Common;
using Git.Storage.Common.Excel;
using Git.Storage.Entity.Check;
using Git.Storage.Provider;
using Git.Storage.Provider.Check;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using Storage.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Check.Controllers
{
    public class ProductManagerAjaxController : AjaxPage
    {
        /// <summary>
        /// 盘点单管理分页
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            int status = WebUtil.GetFormValue<int>("Status", 0);
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("BeginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("EndTime", string.Empty);
            CheckStockEntity entity = new CheckStockEntity();
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
                entity.Where("CreateTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime, DateTime.Now.AddDays(-1)), ConvertHelper.ToType<DateTime>(endTime, DateTime.Now));
            }
            entity.And(a => a.StorageNum == this.DefaultStore);
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
            List<CheckStockEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult.IsNull() ? new List<CheckStockEntity>() : listResult;
            string json = ConvertJson.ListToJson(listResult, "List");
            this.ReturnJson.AddProperty("Data", json);
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除盘点单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            if (!OrderNum.IsEmpty())
            {
                Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
                CheckStockEntity entity = new CheckStockEntity();
                entity.OrderNum = OrderNum;
                string returnValue = bill.Delete(entity);
                this.ReturnJson.AddProperty("d", returnValue);
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除盘点单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DeleteBatch([ModelBinder(typeof(JsonBinder<List<string>>))] List<string> list)
        {
            if (!list.IsNullOrEmpty())
            {
                foreach (string orderNum in list)
                {
                    Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
                    CheckStockEntity entity = new CheckStockEntity();
                    entity.OrderNum = orderNum;
                    string returnValue = bill.Delete(entity);
                    this.ReturnJson.AddProperty("d", returnValue);
                }
            }
            return Content(this.ReturnJson.ToString());
        }
        
        /// <summary>
        /// 查询盘点盘差详细信息
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetCheckDif()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            CheckDataProvider provider = new CheckDataProvider();
            List<CheckDataEntity> listResult = provider.GetCheckDifList(OrderNum, ref pageInfo);
            listResult = listResult == null ? new List<CheckDataEntity>() : listResult;
            System.Threading.Tasks.Parallel.ForEach(listResult, item => 
            { 
                item.LocalQty = ConvertHelper.ToType<double>(item.LocalQty.ToString());
                item.FirstQty = ConvertHelper.ToType<double>(item.FirstQty.ToString());
                item.DifQty = ConvertHelper.ToType<double>((item.FirstQty - item.LocalQty).ToString());
                item.LocalName = LocalHelper.GetLocalNumList(this.DefaultStore,item.LocalNum);
            });
            string json = ConvertJson.ListToJson<CheckDataEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", json);
            this.ReturnJson.AddProperty("RowCount", pageInfo.PageCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Audite()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            int Status = WebUtil.GetFormValue<int>("Status", 0);
            string Reason = WebUtil.GetFormValue<string>("Reason", string.Empty);
            CheckStockEntity entity = new CheckStockEntity();
            entity.Status = Status;
            entity.OrderNum = OrderNum;
            entity.AuditUser = this.LoginUser.UserCode;
            entity.Reason = Reason;
            entity.OperateType = (int)EOpType.PC;
            entity.EquipmentNum = string.Empty;
            entity.EquipmentCode = string.Empty;
            Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
            string returnValue = bill.Audite(entity);
            this.ReturnJson.AddProperty("d", returnValue);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 完成盘点
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Complete()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            CheckStockEntity entity = new CheckStockEntity();
            entity.IsComplete = (int)EBool.Yes;
            entity.OrderNum = OrderNum;
            CheckDataProvider provider = new CheckDataProvider();
            int line = provider.CompleteCheck(entity);
            if (line > 0)
            {
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("Key", "1000");
                    this.ReturnJson.AddProperty("Value", "操作完成");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 导出盘点单Excel
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ToExcel()
        {
            int status = WebUtil.GetFormValue<int>("Status", 0);
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("BeginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("EndTime", string.Empty);
            CheckStockEntity entity = new CheckStockEntity();
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
                entity.Where("CreateTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime, DateTime.Now.AddDays(-1)), ConvertHelper.ToType<DateTime>(endTime, DateTime.Now));
            }
            entity.And(a => a.StorageNum == this.DefaultStore);
            PageInfo pageInfo = new PageInfo() { PageIndex = 1, PageSize = Int32.MaxValue };
            Bill<CheckStockEntity, CheckStockInfoEntity> bill = new CheckOrder();
            List<CheckStockEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult.IsNull() ? new List<CheckStockEntity>() : listResult;
            if (!listResult.IsNullOrEmpty())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("盘点单号"));
                dt.Columns.Add(new DataColumn("盘点类型"));
                dt.Columns.Add(new DataColumn("关联单号"));
                dt.Columns.Add(new DataColumn("状态"));
                dt.Columns.Add(new DataColumn("制单人"));
                dt.Columns.Add(new DataColumn("操作方式"));
                dt.Columns.Add(new DataColumn("创建时间"));
                foreach (CheckStockEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = t.OrderNum;
                    row[1] = EnumHelper.GetEnumDesc<ECheckType>(t.Type);
                    row[2] = t.ContractOrder;
                    row[3] = EnumHelper.GetEnumDesc<EAudite>(t.Status);
                    row[4] = t.CreateUserName;
                    row[5] = EnumHelper.GetEnumDesc<EOpType>(t.OperateType);
                    row[6] = t.CreateTime.ToString("yyyy-MM-dd");
                    dt.Rows.Add(row);
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("盘点管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("盘点管理", "盘点单", System.IO.Path.Combine(filePath, filename));
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
        /// 下载盘点单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ToCheckExcel()
        {
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            List<CheckDataEntity> listResult = new CheckDataProvider().GetCheckOrder(orderNum);
            if (!listResult.IsNullOrEmpty())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("库位名称"));
                dt.Columns.Add(new DataColumn("库位编号"));
                dt.Columns.Add(new DataColumn("产品编码"));
                dt.Columns.Add(new DataColumn("产品条码"));
                dt.Columns.Add(new DataColumn("产品名称"));
                dt.Columns.Add(new DataColumn("批次"));
                dt.Columns.Add(new DataColumn("盘点数"));
                foreach (CheckDataEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = t.LocalName;
                    row[1] = t.LocalNum;
                    row[2] = t.ProductNum;
                    row[3] = t.BarCode;
                    row[4] = t.ProductName;
                    row[5] = t.BatchNum;
                    row[6] = "";
                    dt.Rows.Add(row);
                }
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("盘点管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                NPOIExcel excel = new NPOIExcel("盘点管理", "盘点单", System.IO.Path.Combine(filePath, filename));
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
        ///复核盘点数据 
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult CheckData()
        {
            string Url = WebUtil.GetFormValue<string>("Url", string.Empty);
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            DataSet dataset = ExcelHelper.LoadDataFromExcel(Server.MapPath(Url), "盘点管理$");
            if (dataset != null && dataset.Tables.Count > 0)
            {
                DataTable table = dataset.Tables[0];
                int index = 0;
                int line = 0;
                foreach (DataRow row in table.Rows)
                {
                    if (index > 0)
                    {
                        CheckDataEntity entity = new CheckDataEntity();
                        string LocalNum = row[1].ToString();
                        string ProductNum = row[2].ToString();
                        string BarCode = row[3].ToString();
                        string StorageNum = this.DefaultStore;
                        string BatchNum = row[5].ToString();
                        double Qty = ConvertHelper.ToType<double>(row[6].ToString(), 0);
                        entity.OrderNum = OrderNum;
                        entity.LocalNum = LocalNum;
                        entity.ProductNum = ProductNum;
                        entity.BarCode = BarCode;
                        entity.StorageNum = StorageNum;
                        entity.FirstQty = Qty;
                        entity.BatchNum = BatchNum;
                        CheckDataProvider provider = new CheckDataProvider();
                        line += provider.UpdateCheckInfoNum(entity);
                    }
                    index++;
                }
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("Key","1000");
                    this.ReturnJson.AddProperty("Value", "提交数据成功");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加盘点数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddCheckData()
        {
            string OrderNum = WebUtil.GetFormValue<string>("OrderNum",string.Empty);
            string LocalNum = WebUtil.GetFormValue<string>("LocalNum", string.Empty);
            string LocalName = WebUtil.GetFormValue<string>("LocalName", string.Empty);
            string ProductNum = WebUtil.GetFormValue<string>("ProductNum", string.Empty);
            string BarCode = WebUtil.GetFormValue<string>("BarCode", string.Empty);
            string ProductName = WebUtil.GetFormValue<string>("ProductName", string.Empty);
            string BatchNum = WebUtil.GetFormValue<string>("BatchNum", string.Empty);
            double FirstQty = WebUtil.GetFormValue<double>("FirstQty", 0);
            string StorageNum = this.DefaultStore;
            string FirstUser = this.LoginUser.UserCode;

            CheckDataEntity entity = new CheckDataEntity();
            entity.OrderNum = OrderNum;
            entity.LocalNum = LocalNum;
            entity.LocalName = LocalName;
            entity.StorageNum = StorageNum;
            entity.ProductNum = ProductNum;
            entity.ProductName = ProductName;
            entity.BarCode = BarCode;
            entity.BatchNum = BatchNum;
            entity.LocalQty = 0;
            entity.FirstQty = FirstQty;
            entity.SecondQty = 0;
            entity.DifQty = 0;
            entity.FirstUser = FirstUser;
            entity.SecondUser = "";
            entity.CreateTime = DateTime.Now;

            CheckDataProvider provider = new CheckDataProvider();
            int line = provider.AddCheckData(entity);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("Key", "1000");
            }
            return Content(this.ReturnJson.ToString());
        }


        /// <summary>
        /// 删除盘点数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DeleteCheckData()
        {
            int id = WebUtil.GetFormValue<int>("id",0);
            CheckDataEntity entity = new CheckDataEntity();
            entity.Where(a => a.ID == id);
            CheckDataProvider provider = new CheckDataProvider();
            int line = provider.DeleteCheckData(entity);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("Key", "1000");
            }
            return Content(this.ReturnJson.ToString());
        }
    }
}
