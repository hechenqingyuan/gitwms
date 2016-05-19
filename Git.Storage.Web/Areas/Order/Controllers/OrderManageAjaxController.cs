using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Storage.Entity.Order;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Storage.Provider;
using Git.Storage.Provider.Order;
using Git.Framework.Json;
using System.Data;
using Git.Storage.Common;
using Git.Storage.Common.Excel;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;
using Git.Framework.Controller.Mvc;
using Storage.Common;
using Git.Framework.Io;
using Git.Storage.Provider.Client;
using Git.Storage.Provider.Base;
using Git.Storage.Entity.Base;

namespace Git.Storage.Web.Areas.Order.Controllers
{
    public class OrderManageAjaxController : AjaxPage
    {
        /// <summary>
        /// 分页查询入库单列表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetList()
        {
            int status = WebUtil.GetFormValue<int>("Status", 0);
            int orderType = WebUtil.GetFormValue<int>("OrderType", 0);
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);
            string planNum = WebUtil.GetFormValue<string>("PlanNum", string.Empty);
            int pageSize = WebUtil.GetFormValue<int>("PageSize", 10);
            int pageIndex = WebUtil.GetFormValue<int>("PageIndex", 1);
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            OrdersEntity entity = new OrdersEntity();
            if (status > 0)
            {
                entity.Where(a => a.Status == status);
            }
            if (!orderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + orderNum + "%");
            }
            if (!planNum.IsEmpty())
            {
                entity.Where("ContractOrder", ECondition.Like, "%" + planNum + "%");
            }
            if (orderType > 0)
            {
                entity.Where(a => a.OrderType == orderType);
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("OrderTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }

            entity.And(a => a.IsDelete == (int)EIsDelete.NotDelete);
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            List<OrdersEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<OrdersEntity>() : listResult;
            string json = ConvertJson.ListToJson<OrdersEntity>(listResult, "List");
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
                Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
                OrdersEntity entity = new OrdersEntity();
                entity.OrderNum = orderNum;
                string returnValue = bill.Delete(entity);
                this.ReturnJson.AddProperty("d", returnValue);
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 批量删除订单
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult BatchDel([ModelBinder(typeof(JsonBinder<List<string>>))] List<string> List)
        {
            if (!List.IsNullOrEmpty())
            {
                int line = 0;
                Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
                foreach (string item in List)
                {
                    OrdersEntity entity = new OrdersEntity();
                    entity.OrderNum = item;
                    if (EnumHelper.GetEnumDesc<EReturnStatus>(EReturnStatus.Success) == bill.Delete(entity))
                    {
                        line++;
                    }
                }
                if (line > 0)
                {
                    this.ReturnJson.AddProperty("d", "success");
                }
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult ToExcel()
        {
            int status = WebUtil.GetFormValue<int>("Status", 0);
            int orderType = WebUtil.GetFormValue<int>("OrderType", 0);
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);

            PageInfo pageInfo = new PageInfo() { PageIndex = 1, PageSize = int.MaxValue };
            OrdersEntity entity = new OrdersEntity();
            if (status > 0)
            {
                entity.Where(a => a.Status == status);
            }
            if (!orderNum.IsEmpty())
            {
                entity.Where("OrderNum", ECondition.Like, "%" + orderNum + "%");
            }
            if (orderType > 0)
            {
                entity.Where(a => a.OrderType == orderType);
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("OrderTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            List<OrdersEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<OrdersEntity>() : listResult;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("订单编号"));
            dt.Columns.Add(new DataColumn("订单类型"));
            dt.Columns.Add(new DataColumn("客户编号"));
            dt.Columns.Add(new DataColumn("客户名称"));
            dt.Columns.Add(new DataColumn("关联单号"));
            dt.Columns.Add(new DataColumn("总数量"));
            dt.Columns.Add(new DataColumn("订单时间"));
            dt.Columns.Add(new DataColumn("发货日期"));
            dt.Columns.Add(new DataColumn("创建人"));
            dt.Columns.Add(new DataColumn("备注"));
            foreach (OrdersEntity t in listResult)
            {
                DataRow row = dt.NewRow();
                row[0] = t.OrderNum;
                row[1] = EnumHelper.GetEnumDesc<EOrderStatus>(t.Status);
                row[2] = t.CusNum;
                row[3] = t.CusName;
                row[4] = t.Contact;
                row[5] = t.Num;
                row[6] = t.OrderTime.ToString("yyyy-MM-dd");
                row[7] = t.SendDate.ToString("yyyy-MM-dd");
                row[8] = t.CreateUser;
                row[9] = t.Remark;
                dt.Rows.Add(row);
            }
            string filePath = Server.MapPath("~/UploadFiles/");
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string filename = string.Format("订单管理{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            AsposeExcel excel = new AsposeExcel(System.IO.Path.Combine(filePath, filename), "");
            excel.DatatableToExcel(dt, "订单管理", "订单管理");
            this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());

            return Content(this.ReturnJson.ToString());
        }


        /****************************************************************************************/

        /// <summary>
        /// 导入Excel获取用户订单数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult doImportFile()
        {
            string msg = "";
            try
            {
                string filename = WebUtil.GetFormValue<string>("Url", string.Empty);
                string path = Server.MapPath(filename);
                if (FileManager.FileExists(path) && (path.EndsWith(".xls") || path.EndsWith(".xlsx")))
                {
                    AsposeExcel ap = new AsposeExcel(path);
                    DataTable table = ap.ExcelToDatatalbe("");

                    if (table != null && table.Columns.Count == 27)
                    {
                        List<OrdersEntity> list = new List<OrdersEntity>();
                        GetFu(table, list);
                        if (list.Count > 0)
                        {
                            Session[CacheKey.TEMPDATA_CACHE_ORDERIMPORT] = list;
                            this.ReturnJson.AddProperty("d", msg);
                        }
                    }
                    else if (table != null && table.Columns.Count == 23)
                    {
                        List<OrdersEntity> list = new List<OrdersEntity>();
                        GetProCatInfo(table, list);
                        if (list.Count > 0)
                        {
                            Session[CacheKey.TEMPDATA_CACHE_ORDERIMPORT] = list;
                            this.ReturnJson.AddProperty("d", msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                this.ReturnJson.AddProperty("d", "error");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 根据订单编号删除订单
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DeleteImportFileList()
        {
            string orderNum = WebUtil.GetFormValue<string>("OrderNum", string.Empty);
            if (!orderNum.IsEmpty())
            {
                List<OrdersEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_ORDERIMPORT] as List<OrdersEntity>;
                listResult.Remove(a => a.OrderNum == orderNum);
                Session[CacheKey.TEMPDATA_CACHE_ORDERIMPORT] = listResult;
                this.ReturnJson.AddProperty("d", "Success");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 清除已经上传
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Clear()
        {
            Session[CacheKey.TEMPDATA_CACHE_ORDERIMPORT] = null;
            return Content("");
        }

        /// <summary>
        /// 获得导入文件信息列表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ImportFileList()
        {
            List<OrdersEntity> listResult = Session[CacheKey.TEMPDATA_CACHE_ORDERIMPORT] as List<OrdersEntity>;
            listResult = listResult == null ? new List<OrdersEntity>() : listResult;
            Bill<OrdersEntity, OrderDetailEntity> bill = new OrderBill();
            foreach (OrdersEntity item in listResult)
            {
                OrdersEntity order = new OrdersEntity();
                order.OrderNum = item.OrderNum;
                order.Where(a => a.OrderNum == item.OrderNum);
                order.And(a => a.IsDelete == (int)EIsDelete.NotDelete);
                if (bill.GetCount(order) > 0)
                {
                    item.StatusLable = "已导入";
                }
                else
                {
                    item.StatusLable = "";
                }
            }
            string json = ConvertJson.ListToJson<OrdersEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", json);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 从文件获取产品信息(芜湖日立)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="list"></param>
        /// <param name="listDetail"></param>
        private void GetProCatInfo(DataTable table, List<OrdersEntity> list)
        {
            var rows = table.Rows;
            if (rows.Count > 0)
            {
                string cusNum = "0003";
                CustomerProvider provider = new CustomerProvider();
                CustomerEntity customer = provider.GetSingleCustomer(cusNum);
                customer = customer.IsNull() ? new CustomerEntity() : customer;

                MeasureProvider measureProvider = new MeasureProvider();
                for (var i = 0; i < rows.Count; i++)
                {
                    OrdersEntity entity = new OrdersEntity();
                    OrderDetailEntity detail = new OrderDetailEntity();
                    var row = rows[i];

                    entity.SnNum = SequenceProvider.GetSequence(typeof(OrdersEntity));
                    entity.OrderNum = row[1].ToString();
                    entity.OrderType = (int)EOrderType.Really;
                    entity.CusNum = customer.CusNum;
                    entity.CusName = customer.CusName;
                    entity.Contact = customer.Contact;
                    entity.Phone = customer.Phone;
                    entity.Address = string.Empty;
                    entity.ContractOrder = string.Empty;
                    entity.CreateUser = this.LoginUser.UserCode;
                    entity.Remark = string.Empty;
                    entity.Num = ConvertHelper.ToType<int>(row[4].ToString());
                    entity.Amount = 0;
                    entity.SendDate = DateTime.ParseExact(row[8].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    entity.OrderTime = DateTime.Now;
                    entity.AuditeStatus = (int)EAudite.Wait;
                    entity.IsDelete = (int)EIsDelete.NotDelete;
                    entity.CreateTime = DateTime.Now;
                    entity.Reason = string.Empty;

                    string barCode = row[2].ToString();
                    string productName = row[3].ToString();
                    List<ProductEntity> listProducts = new ProductProvider().GetListByCache();
                    ProductEntity product = null;
                    if (!listProducts.IsNullOrEmpty())
                    {
                        product = listProducts.Where(a => a.BarCode.Contains(barCode)).FirstOrDefault();
                    }

                    detail.SnNum = SequenceProvider.GetSequence(typeof(OrderDetailEntity));
                    detail.OrderSnNum = entity.SnNum;
                    detail.OrderNum = row[1].ToString();
                    string unitName = row[5].ToString();
                    MeasureEntity measureEntity = measureProvider.GetMeasureByName(unitName);
                    if (product != null)
                    {
                        detail.BarCode = product.BarCode;
                        detail.ProductNum = product.SnNum;
                        detail.ProductName = product.ProductName;
                    }
                    else
                    {
                        //product = new ProductEntity();
                        //product.SnNum = new TNumProivder().GetSwiftNum(typeof(ProductEntity), 5);
                        //product.BarCode = barCode;
                        //product.ProductName = productName;
                        //product.CreateTime = DateTime.Now;
                        //product.Display = productName;
                        //if (measureEntity != null)
                        //{
                        //    product.Unit = measureEntity.ID;
                        //    product.UnitName = measureEntity.MeasureName;
                        //}
                        //else
                        //{
                        //    product.Unit = 0;
                        //    product.UnitName = "";
                        //}
                        //product.CateNum = "04";
                        //product.CateName = "外购件";
                        //if (new ProductProvider().GetCountByBarCode(barCode) == 0)
                        //{
                        //    new ProductProvider().Add(product);
                        //}
                        //detail.BarCode = barCode;
                        //detail.ProductNum = "";
                        //detail.ProductName = productName + "【新增】";
                    }

                    detail.Num = ConvertHelper.ToType<int>(row[4].ToString());
                    detail.RealNum = 0;
                    if (measureEntity != null)
                    {
                        detail.UnitNum = measureEntity.MeasureNum;
                        detail.UnitName = measureEntity.MeasureName;
                    }
                    else
                    {
                        detail.UnitNum = "";
                        detail.UnitName = "";
                    }
                    detail.Price = 0;
                    detail.Amount = 0;
                    detail.Status = (int)EOrderStatus.CreateOrder;
                    detail.SendTime = DateTime.ParseExact(row[8].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);// ConvertHelper.ToType<DateTime>(row[8].ToString());
                    detail.ContractID = string.Empty;
                    detail.Remark = string.Empty;
                    detail.CreateTime = DateTime.Now;
                    entity.BarCode = detail.BarCode.IsEmpty() ? barCode : detail.BarCode;
                    entity.ProductName = detail.ProductName.IsEmpty() ? productName: detail.ProductName;
                    entity.ProductNum = detail.ProductNum.IsEmpty() ? "" : detail.ProductNum;
                    entity.UnitName = detail.UnitName;
                    entity.ListDetail = new List<OrderDetailEntity>() { detail };

                    list.Add(entity);
                }
            }
        }

        /// <summary>
        /// 读取文件中产品信息(富士通)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="list"></param>
        private void GetFu(DataTable table, List<OrdersEntity> list)
        {
            DataRowCollection rows = table.Rows;
            if (rows.Count > 0)
            {
                string cusNum = "0002";
                CustomerProvider provider = new CustomerProvider();
                CustomerEntity customer = provider.GetSingleCustomer(cusNum);
                customer = customer.IsNull() ? new CustomerEntity() : customer;

                MeasureProvider measureProvider = new MeasureProvider();
                for (var i = 0; i < rows.Count; i++)
                {
                    OrdersEntity entity = new OrdersEntity();
                    OrderDetailEntity detail = new OrderDetailEntity();
                    var row = rows[i];

                    entity.SnNum = SequenceProvider.GetSequence(typeof(OrdersEntity));
                    entity.OrderNum = row[10].ToString();
                    entity.OrderType = (int)EOrderType.Really;
                    entity.CusNum = customer.CusNum;
                    entity.CusName = customer.CusName;
                    entity.Contact = customer.Contact;
                    entity.Phone = customer.Phone;
                    entity.Address = string.Empty;
                    entity.ContractOrder = string.Empty;
                    entity.CreateUser = this.LoginUser.UserCode;
                    entity.Remark = string.Empty;
                    entity.Num = ConvertHelper.ToType<int>(row[14].ToString());
                    entity.Amount = 0;
                    entity.SendDate = ConvertHelper.ToType<DateTime>(row[13].ToString());
                    entity.OrderTime = DateTime.Now;
                    entity.AuditeStatus = (int)EAudite.Wait;
                    entity.IsDelete = (int)EIsDelete.NotDelete;
                    entity.CreateTime = DateTime.Now;
                    entity.Reason = string.Empty;

                    string barCode = row[6].ToString();
                    string productName = row[8].ToString();
                    List<ProductEntity> listProducts = new ProductProvider().GetListByCache();
                    ProductEntity product = null;
                    if (!listProducts.IsNullOrEmpty())
                    {
                        product = listProducts.Where(a => a.BarCode.Contains(barCode)).FirstOrDefault();
                    }

                    detail.SnNum = SequenceProvider.GetSequence(typeof(OrderDetailEntity));
                    detail.OrderSnNum = entity.SnNum;
                    detail.OrderNum = entity.OrderNum;
                    //string unitName = row[5].ToString();
                    string unitName = "J";
                    MeasureEntity measureEntity = measureProvider.GetMeasureByName(unitName);
                    if (product != null)
                    {
                        detail.BarCode = product.BarCode;
                        detail.ProductNum = product.SnNum;
                        detail.ProductName = product.ProductName;
                    }
                    else
                    {
                        //product = new ProductEntity();
                        //product.SnNum = new TNumProivder().GetSwiftNum(typeof(ProductEntity), 5);
                        //product.BarCode = barCode;
                        //product.ProductName = productName;
                        //product.CreateTime = DateTime.Now;
                        //product.Display = productName;
                        //if (measureEntity != null)
                        //{
                        //    product.Unit = measureEntity.ID;
                        //    product.UnitName = measureEntity.MeasureName;
                        //}
                        //else
                        //{
                        //    product.Unit = 0;
                        //    product.UnitName = "";
                        //}
                        //product.CateNum = "04";
                        //product.CateName = "外购件";
                        //if (new ProductProvider().GetCountByBarCode(barCode) == 0)
                        //{
                        //    new ProductProvider().Add(product);
                        //}
                        //detail.BarCode = barCode;
                        //detail.ProductNum = "";
                        //detail.ProductName = productName + "【新增】";
                    }

                    detail.Num = ConvertHelper.ToType<int>(row[14].ToString());
                    detail.RealNum = 0;
                    if (measureEntity != null)
                    {
                        detail.UnitNum = measureEntity.MeasureNum;
                        detail.UnitName = measureEntity.MeasureName;
                    }
                    else
                    {
                        detail.UnitNum = "";
                        detail.UnitName = "";
                    }
                    detail.Price = 0;
                    detail.Amount = 0;
                    detail.Status = (int)EOrderStatus.CreateOrder;
                    detail.SendTime = ConvertHelper.ToType<DateTime>(row[13].ToString());
                    detail.ContractID = string.Empty;
                    detail.Remark = string.Empty;
                    detail.CreateTime = DateTime.Now;
                    entity.BarCode = detail.BarCode.IsEmpty() ? barCode : detail.BarCode;
                    entity.ProductName = detail.ProductName.IsEmpty() ? productName : detail.ProductName;
                    entity.ProductNum = detail.ProductNum.IsEmpty() ? "" : detail.ProductNum;
                    entity.UnitName = detail.UnitName;
                    entity.ListDetail = new List<OrderDetailEntity>() { detail };

                    list.Add(entity);
                }
            }
        }
    }
}
