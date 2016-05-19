using Git.Framework.Controller;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.DataTypes;
using Git.Framework.Json;
using Git.Storage.Web.Lib;
using System.Data;
using Git.Storage.Common.Excel;
using Git.Framework.Controller.Mvc;
using Git.Storage.Provider;
using Git.Storage.Common;
using Storage.Common;
using System.Xml;
using Git.Storage.Entity.InStorage;
using Git.Storage.Provider.InStorage;
using Git.Storage.Entity.OutStorage;
using Git.Storage.Provider.OutStorage;
using Git.Storage.Provider.Client;
using Git.Storage.Entity.Bad;
using Git.Storage.Provider.Bad;
using Git.Storage.Provider.Returns;
using Git.Storage.Entity.Return;
using Aspose.Cells;
using System.Text;
using Git.Storage.Entity.Report;
using Git.Framework.Cache;

namespace Git.Storage.Web.Areas.Report.Controllers
{
    public class ReportAjaxController : AjaxPage
    {
        /// <summary>
        /// 查询产品类别分页
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ProductReportList()
        {
            string searchKey = WebUtil.GetFormValue<string>("ProductName", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("BeginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("EndTime", string.Empty);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string storageNum = this.DefaultStore;
            ProductProvider provider = new ProductProvider();
            ProductEntity entity = new ProductEntity();
            entity.StorageNum = storageNum;
            if (!searchKey.IsEmpty())
            {
                entity.Begin<ProductEntity>()
                 .Where<ProductEntity>("ProductName", ECondition.Like, "%" + searchKey + "%")
                 .Or<ProductEntity>("SnNum", ECondition.Like, "%" + searchKey + "%")
                 .Or<ProductEntity>("BarCode", ECondition.Like, "%" + searchKey + "%")
                 .End<ProductEntity>();
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("CreateTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }

            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            List<ProductEntity> listResult = provider.GetList(entity, ref pageInfo, searchKey, beginTime, endTime);
            string json = ConvertJson.ListToJson<ProductEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 导出产品在线库存报表Excel
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ToProductReportExcel()
        {
            PageInfo pageInfo = new Git.Framework.DataTypes.PageInfo() { PageIndex = 1, PageSize = Int32.MaxValue };
            string searchKey = WebUtil.GetFormValue<string>("ProductName", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("BeginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("EndTime", string.Empty);
            string storageNum = this.DefaultStore;

            ProductProvider provider = new ProductProvider();
            ProductEntity entity = new ProductEntity();
            if (!searchKey.IsEmpty())
            {
                entity.Begin<ProductEntity>()
                 .Where<ProductEntity>("ProductName", ECondition.Like, "%" + searchKey + "%")
                 .Or<ProductEntity>("SnNum", ECondition.Like, "%" + searchKey + "%")
                 .Or<ProductEntity>("BarCode", ECondition.Like, "%" + searchKey + "%")
                 .End<ProductEntity>();
            }
            if (!beginTime.IsEmpty() && !endTime.IsEmpty())
            {
                entity.Where("CreateTime", ECondition.Between, ConvertHelper.ToType<DateTime>(beginTime), ConvertHelper.ToType<DateTime>(endTime));
            }
            if (storageNum.IsNotNull())
            {
                entity.Where("StorageNum", ECondition.Eth, storageNum);
            }
            List<ProductEntity> listResult = provider.GetList(entity, ref pageInfo, searchKey, beginTime, endTime);
            listResult = listResult.IsNull() ? new List<ProductEntity>() : listResult;
            if (!listResult.IsNullOrEmpty())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号 "));
                dt.Columns.Add(new DataColumn("产品编号"));
                dt.Columns.Add(new DataColumn("产品条码"));
                dt.Columns.Add(new DataColumn("产品名称"));
                dt.Columns.Add(new DataColumn("类别名称"));
                dt.Columns.Add(new DataColumn("预警值下限"));
                dt.Columns.Add(new DataColumn("预警值上限"));
                dt.Columns.Add(new DataColumn("规格"));
                dt.Columns.Add(new DataColumn("价格"));
                dt.Columns.Add(new DataColumn("库存数"));
                dt.Columns.Add(new DataColumn("进货总数"));
                dt.Columns.Add(new DataColumn("出货总数"));
                dt.Columns.Add(new DataColumn("报损总数"));

                int count = 1;
                foreach (ProductEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.SnNum;
                    row[2] = t.BarCode;
                    row[3] = t.ProductName;
                    row[4] = t.CateName;
                    row[5] = t.MinNum;
                    row[6] = t.MaxNum;
                    row[7] = t.Size;
                    row[8] = t.AvgPrice;
                    row[9] = t.LocalProductNum;
                    row[10] = t.InStorageNum;
                    row[11] = t.OutStorageNum;
                    row[12] = t.BadReportNum;
                    dt.Rows.Add(row);
                    count++;
                }
                DataRow rowTemp = dt.NewRow();
                rowTemp[0] = count;
                rowTemp[1] = "";
                rowTemp[2] = "";
                rowTemp[3] = "";
                rowTemp[4] = "";
                rowTemp[5] = "";
                rowTemp[6] = "";
                rowTemp[7] = "";
                rowTemp[8] = "总计";
                rowTemp[9] = listResult[0].TotalLocalProductNum;
                rowTemp[10] = listResult[0].TotalInStorageNum;
                rowTemp[11] = listResult[0].TotalOutStorageNum;
                rowTemp[12] = listResult[0].TotalBadReportNum;
                dt.Rows.Add(rowTemp);
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("产品在线库存报表{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                AsposeExcel excel = new AsposeExcel(System.IO.Path.Combine(filePath, filename), "");
                excel.DatatableToExcel(dt, "产品在线库存报表", "产品在线库存报表");
                this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
                this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
            }
            else
            {
                this.ReturnJson.AddProperty("d", "无数据导出!");
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 库存清单报表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult StockBillList()
        {
            string searchKey = WebUtil.GetFormValue<string>("ProductName", string.Empty);
            string localName = WebUtil.GetFormValue<string>("LocalName", string.Empty);
            string localType = WebUtil.GetFormValue<string>("LocalType", string.Empty);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string storageNum = this.DefaultStore;

            LocalProductProvider provider = new LocalProductProvider();
            LocalProductEntity entity = new LocalProductEntity();

            if (!localType.IsEmpty())
            {
                entity.Where("LocalType", ECondition.Eth, localType);
            }
            if (storageNum.IsNotNull())
            {
                entity.Where("StorageNum", ECondition.Eth, storageNum);
            }
            if (!localName.IsEmpty())
            {
                entity.Where("LocalName", ECondition.Like, "%" + localName + "%");
                entity.Or("LocalNum", ECondition.Like, "%" + localName + "%");
            }
            if (!searchKey.IsEmpty())
            {
                entity.Begin<LocalProductEntity>()
                 .Where<LocalProductEntity>("ProductName", ECondition.Like, "%" + searchKey + "%")
                 .Or<LocalProductEntity>("ProductNum", ECondition.Like, "%" + searchKey + "%")
                 .Or<LocalProductEntity>("BarCode", ECondition.Like, "%" + searchKey + "%")
                 .End<LocalProductEntity>();
            }
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            List<LocalProductEntity> listResult = provider.GetList(entity, ref pageInfo);
            int allNum = provider.GetAllNum(localName, localType, searchKey, storageNum);
            double allTotalPrice = provider.GetAllTotalPrice(localName, localType, searchKey, storageNum);
            if (!listResult.IsNullOrEmpty())
            {
                listResult.ForEach(a =>
                {
                    a.TotalPrice = a.Num * a.AvgPrice;
                });
            }
            string json = ConvertJson.ListToJson<LocalProductEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            this.ReturnJson.AddProperty("AllNum", allNum);
            this.ReturnJson.AddProperty("AllTotalPrice", allTotalPrice);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 导出库存清单报表Excel
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ToStockBilReportExcel()
        {
            string searchKey = WebUtil.GetFormValue<string>("ProductName", string.Empty);
            string localName = WebUtil.GetFormValue<string>("LocalName", string.Empty);
            string localType = WebUtil.GetFormValue<string>("LocalType", string.Empty);
            string storageNum = this.DefaultStore;

            LocalProductProvider provider = new LocalProductProvider();
            LocalProductEntity entity = new LocalProductEntity();
            if (!localType.IsEmpty())
            {
                entity.Where("LocalType", ECondition.Eth, localType);
            }
            if (storageNum.IsNotNull())
            {
                entity.Where("StorageNum", ECondition.Eth, storageNum);
            }
            if (!localName.IsEmpty())
            {
                entity.Where("LocalName", ECondition.Like, "%" + localName + "%");
            }
            if (!searchKey.IsEmpty())
            {
                entity.Begin<LocalProductEntity>()
                 .Where<LocalProductEntity>("ProductName", ECondition.Like, "%" + searchKey + "%")
                 .Or<LocalProductEntity>("ProductNum", ECondition.Like, "%" + searchKey + "%")
                 .Or<LocalProductEntity>("BarCode", ECondition.Like, "%" + searchKey + "%")
                 .End<LocalProductEntity>();
            }

            PageInfo pageInfo = new PageInfo() { PageIndex = 1, PageSize = Int32.MaxValue };
            List<LocalProductEntity> listResult = provider.GetList(entity, ref pageInfo);
            int allNum = provider.GetAllNum(localName, localType, searchKey, storageNum);
            double allTotalPrice = provider.GetAllTotalPrice(localName, localType, searchKey, storageNum);
            if (!listResult.IsNullOrEmpty())
            {
                listResult.ForEach(a =>
                {
                    a.TotalPrice = a.Num * a.AvgPrice;
                });
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("序号 "));
                dt.Columns.Add(new DataColumn("库位 "));
                dt.Columns.Add(new DataColumn("库位类型"));
                dt.Columns.Add(new DataColumn("产品编号"));
                dt.Columns.Add(new DataColumn("产品条码"));
                dt.Columns.Add(new DataColumn("产品名称"));
                dt.Columns.Add(new DataColumn("类别名称"));
                dt.Columns.Add(new DataColumn("规格"));
                dt.Columns.Add(new DataColumn("预警值下限"));
                dt.Columns.Add(new DataColumn("预警值上限"));
                dt.Columns.Add(new DataColumn("库存数"));
                dt.Columns.Add(new DataColumn("价格"));
                dt.Columns.Add(new DataColumn("总价"));

                int count = 1;
                foreach (LocalProductEntity t in listResult)
                {
                    DataRow row = dt.NewRow();
                    row[0] = count;
                    row[1] = t.LocalName;
                    row[2] = EnumHelper.GetEnumDesc<ELocalType>(t.LocalType);
                    row[3] = t.ProductNum;
                    row[4] = t.BarCode;
                    row[5] = t.ProductName;
                    row[6] = t.CateName;
                    row[7] = t.Size;
                    row[8] = t.MinNum;
                    row[9] = t.MaxNum;
                    row[10] = t.Num;
                    row[11] = t.AvgPrice;
                    row[12] = t.TotalPrice;
                    dt.Rows.Add(row);
                    count++;
                }
                DataRow rowTemp = dt.NewRow();
                rowTemp[0] = count;
                rowTemp[1] = "";
                rowTemp[2] = "";
                rowTemp[3] = "";
                rowTemp[4] = "";
                rowTemp[5] = "";
                rowTemp[6] = "";
                rowTemp[7] = "";
                rowTemp[8] = "";
                rowTemp[9] = "总计";
                rowTemp[10] = allNum;
                rowTemp[11] = "";
                rowTemp[12] = allTotalPrice;
                dt.Rows.Add(rowTemp);
                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("库存清单报表{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                AsposeExcel excel = new AsposeExcel(System.IO.Path.Combine(filePath, filename), "");
                excel.DatatableToExcel(dt, "库存清单报表", "库存清单报表");
                this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
                this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
            }
            else
            {
                this.ReturnJson.AddProperty("d", "无数据导出!");
            }
            return Content(this.ReturnJson.ToString());

        }

        /// <summary>
        /// 产品出入库报表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ProductInOutReportList()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            string storageNum = this.DefaultStore;
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);

            string Key = "InOutReprt" + DateTime.Now.AddDays(-queryTime).ToString("yyMMdd") + DateTime.Now.ToString("yyMMdd") + storageNum;
            List<ProductEntity> listResult = CacheHelper.Get(Key) as List<ProductEntity>;
            if (listResult.IsNullOrEmpty())
            {
                listResult = new List<ProductEntity>();
                InStorageProvider inProvider = new InStorageProvider();
                List<ReportChart> listInSource = inProvider.GetChartTop((int)EAudite.Pass, this.DefaultStore, DateTime.Now.AddDays(-queryTime), DateTime.Now);
                if (!listInSource.IsNullOrEmpty())
                {
                    foreach (ReportChart chart in listInSource)
                    {
                        ProductEntity entity = new ProductEntity();
                        entity.SnNum = chart.ProductNum;
                        entity.BarCode = chart.BarCode;
                        entity.ProductName = chart.ProductName;
                        entity.InStorageNum = chart.Num;
                        entity.Size = chart.Size.IsEmpty() ? "" : chart.Size;
                        listResult.Add(entity);
                    }
                }
                OutStorageProvider outProvider = new OutStorageProvider();
                List<ReportChart> listOutSource = listInSource = outProvider.GetChartTop((int)EAudite.Pass, this.DefaultStore, DateTime.Now.AddDays(-queryTime), DateTime.Now);
                if (!listOutSource.IsNullOrEmpty())
                {
                    foreach (ReportChart chart in listOutSource)
                    {
                        if (listResult.Exists(a => a.SnNum == chart.ProductNum))
                        {
                            ProductEntity entity = listResult.First(a => a.SnNum == chart.ProductNum);
                            entity.OutStorageNum = chart.Num;
                        }
                        else
                        {
                            ProductEntity entity = new ProductEntity();
                            entity.SnNum = chart.ProductNum;
                            entity.BarCode = chart.BarCode;
                            entity.ProductName = chart.ProductName;
                            entity.OutStorageNum = chart.Num;
                            entity.Size = chart.Size.IsEmpty() ? "" : chart.Size;
                            listResult.Add(entity);
                        }
                    }
                }
                if (!listResult.IsNullOrEmpty())
                {
                    double inTotalNum = listResult.Sum(a => a.InStorageNum);
                    double outTotalNum = listResult.Sum(a => a.OutStorageNum);
                    listResult.ForEach(a =>
                    {
                        a.InStorageNumPCT = inTotalNum > 0 ? a.InStorageNum*100 / inTotalNum : 0;
                        a.OutStorageNumPCT = outTotalNum > 0 ? a.OutStorageNum*100 / outTotalNum : 0;
                    });
                    CacheHelper.Insert(Key, listResult, null, DateTime.Now.AddMinutes(30));
                }
            }
            int rowCount = listResult.Count;
            listResult = listResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            string json = ConvertJson.ListToJson<ProductEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", rowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 出入库报表--绑定饼图数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult BindPieData()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            string storageNum = this.DefaultStore;
            InStorageProvider inProvider = new InStorageProvider();
            List<ReportChart> listInSource = inProvider.GetChartTop((int)EAudite.Pass, this.DefaultStore, DateTime.Now.AddDays(-queryTime), DateTime.Now);
            StringBuilder sbInStorage = new StringBuilder();
            sbInStorage.Append("<pie>");
            listInSource.ForEach(a =>
            {
                sbInStorage.AppendFormat("<slice title=\"{0}\">{1}</slice>", a.ProductName, a.Num);
            });
            sbInStorage.Append("</pie>");

            OutStorageProvider outProvider = new OutStorageProvider();
            listInSource = outProvider.GetChartTop((int)EAudite.Pass, this.DefaultStore, DateTime.Now.AddDays(-queryTime), DateTime.Now);
            StringBuilder sbOutStorage = new StringBuilder();
            sbOutStorage.Append("<pie>");
            listInSource.ForEach(a =>
            {
                sbOutStorage.AppendFormat("<slice title=\"{0}\">{1}</slice>", a.ProductName, a.Num);
            });
            sbOutStorage.Append("</pie>");
            this.ReturnJson.AddProperty("InStorageData", sbInStorage.ToString());
            this.ReturnJson.AddProperty("OutStorageData", sbOutStorage.ToString());
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 入库报表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult InStorageReport()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string storageNum = this.DefaultStore;

            InStorageEntity entity = new InStorageEntity();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            if (queryTime > 0)
            {
                entity.Where("CreateTime", ECondition.Between, DateTime.Now.AddDays(-queryTime), DateTime.Now);
            }
            if (storageNum.IsNotNull())
            {
                entity.Where("StorageNum", ECondition.Eth, storageNum);
            }
            Bill<InStorageEntity, InStorDetailEntity> bill = new InStorageOrder();
            List<InStorageEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<InStorageEntity>() : listResult;
            string json = ConvertJson.ListToJson<InStorageEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 根据时间段显示入库的情况以及图表情况
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult InStorageReportDetail()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string storageNum = this.DefaultStore;

            PageInfo procPageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            InStorageProvider provider = new InStorageProvider();
            List<Proc_InStorageReportEntity> detailList = provider.GetList(queryTime, ref procPageInfo, storageNum);
            detailList = detailList == null ? new List<Proc_InStorageReportEntity>() : detailList;
            detailList.ForEach(a =>
            {
                a.Amount = a.Amount / 10000;
            });
            string jsonDetail = ConvertJson.ListToJson<Proc_InStorageReportEntity>(detailList, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(jsonDetail));
            this.ReturnJson.AddProperty("RowCount", procPageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 入库报表波形图数据写入
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult InStorageAmpie()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = 10;
            string storageNum = this.DefaultStore;

            PageInfo procPageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            InStorageProvider provider = new InStorageProvider();
            List<Proc_InStorageReportEntity> detailList = provider.GetList(queryTime, ref procPageInfo, storageNum);
            detailList = detailList == null ? new List<Proc_InStorageReportEntity>() : detailList;

            StringBuilder sb = new StringBuilder();
            sb.Append("<chart>");
            sb.Append("<series>");
            int index = 0;
            detailList.ForEach(a =>
            {
                //日期
                sb.AppendFormat("<value xid=\"{0}\">{1}</value>", index.ToString(), a.CreateTime.ToString("MM-dd"));
                index++;
            });
            index = 0;
            sb.Append("</series>");
            sb.Append("<graphs>");
            sb.Append("<graph gid=\"1\">");
            detailList.ForEach(a =>
            {
                //数量
                sb.AppendFormat("<value xid=\"{0}\">{1}</value>", index.ToString(), a.Num.ToString());
                index++;
            });
            index = 0;
            sb.Append("</graph>");
            sb.Append("<graph gid=\"2\">");
            detailList.ForEach(a =>
            {
                //数量
                sb.AppendFormat("<value xid=\"{0}\">{1}</value>", index.ToString(), (a.Amount / 10000).ToString());
                index++;
            });
            sb.Append("</graph>");
            sb.Append("</graphs>");
            sb.Append("</chart>");
            this.ReturnJson.AddProperty("InStorageData", sb.ToString());
            this.ReturnJson.AddProperty("RowCount", procPageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 出库报表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult OutStorageReport()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string storageNum = this.DefaultStore;

            ProductProvider provider = new ProductProvider();
            OutStorageEntity entity = new OutStorageEntity();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            if (queryTime > 0)
            {
                entity.Where("CreateTime", ECondition.Between, DateTime.Now.AddDays(-queryTime), DateTime.Now);
            }
            if (storageNum.IsNotNull())
            {
                entity.Where("StorageNum", ECondition.Eth, storageNum);
            }
            Bill<OutStorageEntity, OutStoDetailEntity> bill = new OutStorageOrder();
            List<OutStorageEntity> listResult = bill.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<OutStorageEntity>() : listResult;
            string json = ConvertJson.ListToJson<OutStorageEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 根据时间段显示出库的情况以及图表情况
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult OutStorageReportDetail()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string storageNum = this.DefaultStore;

            PageInfo procPageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            OutStorageProvider provider = new OutStorageProvider();
            List<Proc_OutStorageReportEntity> detailList = provider.GetList(queryTime, ref procPageInfo, storageNum);
            detailList = detailList == null ? new List<Proc_OutStorageReportEntity>() : detailList;
            detailList.ForEach(a =>
            {
                a.Amount = a.Amount / 10000;
            });
            string jsonDetail = ConvertJson.ListToJson<Proc_OutStorageReportEntity>(detailList, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(jsonDetail));
            this.ReturnJson.AddProperty("RowCount", procPageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 出库报表波形图数据写入
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult OutStorageAmpie()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = 10;
            string storageNum = this.DefaultStore;

            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            OutStorageProvider provider = new OutStorageProvider();
            List<Proc_OutStorageReportEntity> detailList = provider.GetList(queryTime, ref pageInfo, storageNum);
            detailList = detailList == null ? new List<Proc_OutStorageReportEntity>() : detailList;

            StringBuilder sb = new StringBuilder();
            sb.Append("<chart>");
            sb.Append("<series>");
            int index = 0;
            detailList.ForEach(a =>
            {
                //日期
                sb.AppendFormat("<value xid=\"{0}\">{1}</value>", index.ToString(), a.CreateTime.ToString("MM-dd"));
                index++;
            });
            index = 0;
            sb.Append("</series>");
            sb.Append("<graphs>");
            sb.Append("<graph gid=\"1\">");
            detailList.ForEach(a =>
            {
                //数量
                sb.AppendFormat("<value xid=\"{0}\">{1}</value>", index.ToString(), a.Num.ToString());
                index++;
            });
            index = 0;
            sb.Append("</graph>");
            sb.Append("<graph gid=\"2\">");
            detailList.ForEach(a =>
            {
                //数量
                sb.AppendFormat("<value xid=\"{0}\">{1}</value>", index.ToString(), (a.Amount / 10000).ToString());
                index++;
            });
            sb.Append("</graph>");
            sb.Append("</graphs>");
            sb.Append("</chart>");
            this.ReturnJson.AddProperty("OutStorageData", sb.ToString());
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 客户报表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult CustomerReportTOP10()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = 1;
            int pageSize = 10;
            string storageNum = this.DefaultStore;

            OutStorageProvider provider = new OutStorageProvider();
            OutStorageEntity entity = new OutStorageEntity();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };

            List<OutStorageEntity> listResult = provider.GetListTOP10(queryTime, storageNum);
            listResult = listResult.IsNull() ? new List<OutStorageEntity>() : listResult;
            CustomerProvider cusProvider = new CustomerProvider();
            foreach (OutStorageEntity item in listResult)
            {
                CustomerEntity tempItem = cusProvider.GetSingleCustomer(item.CusNum);
                item.CusName = tempItem.CusName;
                item.CusType = tempItem.CusType;
            }
            /*******************************************订单数量排名前十的客户饼图数据****************************************************/
            StringBuilder sb = new StringBuilder();
            sb.Append("<pie>");
            listResult.ForEach(a =>
            {
                if (a.Num > 0)
                {
                    sb.AppendFormat("<slice title=\"{0}\">{1}</slice>", a.CusName, a.Num.ToString());
                }
            });
            sb.Append("</pie>");
            this.ReturnJson.AddProperty("InStorageData", sb.ToString());
            string json = ConvertJson.ListToJson<OutStorageEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 客户详细信息
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult CustomerDetailList()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string storageNum = this.DefaultStore;

            CustomerProvider provider = new CustomerProvider();
            CustomerEntity entity = new CustomerEntity();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            List<CustomerEntity> listResult = provider.GetCustomerList(entity, ref pageInfo);
            listResult = listResult.IsNull() ? new List<CustomerEntity>() : listResult;
            OutStorageProvider outProvider = new OutStorageProvider();
            foreach (CustomerEntity item in listResult)
            {
                item.Num = outProvider.GetNumByCusNum(item.CusNum, queryTime, storageNum);
            }
            string json = ConvertJson.ListToJson<CustomerEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 供应商报表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult SupplierReportTOP10()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = 1;
            int pageSize = 10;
            string storageNum = this.DefaultStore;

            InStorageProvider provider = new InStorageProvider();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };

            List<InStorageEntity> listResult = provider.GetListTOP10(queryTime, storageNum);
            listResult = listResult.IsNull() ? new List<InStorageEntity>() : listResult;
            SupplierProvider cusProvider = new SupplierProvider();
            foreach (InStorageEntity item in listResult)
            {
                SupplierEntity tempItem = cusProvider.GetSupplier(item.SupNum);
                item.SupName = tempItem.SupName;
                item.Description = tempItem.Description;
            }
            /*******************************************订单数量排名前十的供应商饼图数据****************************************************/
            StringBuilder sb = new StringBuilder();
            sb.Append("<pie>");
            listResult.ForEach(a =>
            {
                if (a.Num > 0)
                {
                    sb.AppendFormat("<slice title=\"{0}\">{1}</slice>", a.SupName, a.Num.ToString());
                }
            });
            sb.Append("</pie>");
            this.ReturnJson.AddProperty("InStorageData", sb.ToString());
            string json = ConvertJson.ListToJson<InStorageEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 供应商详细信息
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult SupplierDetailList()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string storageNum = this.DefaultStore;

            SupplierProvider provider = new SupplierProvider();
            SupplierEntity entity = new SupplierEntity();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            List<SupplierEntity> listResult = provider.GetList(entity, ref pageInfo);
            listResult = listResult.IsNull() ? new List<SupplierEntity>() : listResult;
            InStorageProvider inProvider = new InStorageProvider();
            foreach (SupplierEntity item in listResult)
            {
                item.Num = inProvider.GetNumBySupNum(item.SupNum, queryTime, storageNum);
            }
            string json = ConvertJson.ListToJson<SupplierEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 报损报表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult BadReportList()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);

            string storageNum = this.DefaultStore;


            BadProvider provider = new BadProvider();
            BadReportEntity entity = new BadReportEntity();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            if (queryTime > 0)
            {
                entity.Where("CreateTime", ECondition.Between, DateTime.Now.AddDays(-queryTime), DateTime.Now);
            }

            if (storageNum.IsNotNull())
            {
                entity.Where("StorageNum", ECondition.Eth, storageNum);
            }

            entity.And(a => a.StorageNum == this.DefaultStore);

            List<BadReportEntity> listResult = provider.GetList(entity, ref pageInfo, storageNum);
            listResult = listResult == null ? new List<BadReportEntity>() : listResult;
            string json = ConvertJson.ListToJson<BadReportEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 绑定饼图数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult BadTOP10Num()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = 1;
            int pageSize = 10;

            string storageNum = this.DefaultStore;

            BadProvider provider = new BadProvider();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            List<Proc_BadTOP10NumEntity> listResult = provider.GetListTOP10(queryTime, storageNum);
            listResult = listResult.IsNull() ? new List<Proc_BadTOP10NumEntity>() : listResult;
            StringBuilder sb = new StringBuilder();
            sb.Append("<pie>");
            listResult.ForEach(a =>
            {
                if (a.TotalNum > 0)
                {
                    sb.AppendFormat("<slice title=\"{0}\">{1}</slice>", a.ProductName, a.TotalNum.ToString());
                }
            });
            sb.Append("</pie>");
            this.ReturnJson.AddProperty("InStorageData", sb.ToString());
            string json = ConvertJson.ListToJson<Proc_BadTOP10NumEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 退货报表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ReturnReportList()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string storageNum = this.DefaultStore;

            ReturnProvider provider = new ReturnProvider();
            ReturnOrderEntity entity = new ReturnOrderEntity();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            if (queryTime > 0)
            {
                entity.Where("CreateTime", ECondition.Between, DateTime.Now.AddDays(-queryTime), DateTime.Now);
            }
            if (storageNum.IsNotNull())
            {
                entity.Where("StorageNum", ECondition.Eth, storageNum);
            }
            List<ReturnOrderEntity> listResult = provider.GetList(entity, ref pageInfo);
            listResult = listResult == null ? new List<ReturnOrderEntity>() : listResult;
            string json = ConvertJson.ListToJson<ReturnOrderEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 绑定饼图数据
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ReturnTOP10Num()
        {
            int queryTime = WebUtil.GetFormValue<int>("QueryTime", 0);
            int pageIndex = 1;
            int pageSize = 10;
            string storageNum = this.DefaultStore;

            ReturnProvider provider = new ReturnProvider();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };

            List<Proc_ReturnTOP10NumEntity> listResult = provider.GetListTOP10(queryTime, storageNum);
            listResult = listResult.IsNull() ? new List<Proc_ReturnTOP10NumEntity>() : listResult;

            /*******************************************订单数量排名前十的客户饼图数据****************************************************/
            StringBuilder sb = new StringBuilder();
            sb.Append("<pie>");
            listResult.ForEach(a =>
            {
                if (a.TotalNum > 0)
                {
                    sb.AppendFormat("<slice title=\"{0}\">{1}</slice>", a.ProductName, a.TotalNum.ToString());
                }
            });
            sb.Append("</pie>");
            this.ReturnJson.AddProperty("InStorageData", sb.ToString());

            string json = ConvertJson.ListToJson<Proc_ReturnTOP10NumEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 台帐报表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult InventoryList()
        {
            string searchKey = WebUtil.GetFormValue<string>("SearchKey", string.Empty);
            string change = WebUtil.GetFormValue<string>("Change", string.Empty);
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 0);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 0);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);
            string storageNum = this.DefaultStore;

            InventoryProvider provider = new InventoryProvider();
            InventoryBookEntity entity = new InventoryBookEntity();

            if (!searchKey.IsEmpty())
            {
                entity.Begin<InventoryBookEntity>()
                 .Where<InventoryBookEntity>("ProductName", ECondition.Like, "%" + searchKey + "%")
                 .Or<InventoryBookEntity>("ProductNum", ECondition.Like, "%" + searchKey + "%")
                 .Or<InventoryBookEntity>("BarCode", ECondition.Like, "%" + searchKey + "%")
                 .End<InventoryBookEntity>();
            }
            if (!storageNum.IsEmpty())
            {
                entity.Where("StoreNum", ECondition.Eth, storageNum);
            }
            if (!change.IsEmpty())
            {
                entity.Where("Type", ECondition.Eth, change);
            }
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            List<InventoryBookEntity> listResult = provider.GetList(entity, ref pageInfo);
            LocationProvider locaProvider = new LocationProvider();

            foreach (InventoryBookEntity item in listResult)
            {
                LocationEntity locaEntity1 = locaProvider.GetSingleByNum(item.FromLocalNum);
                if (locaEntity1.IsNotNull())
                {
                    item.FromLocalName = locaEntity1.LocalName;
                }
                else
                {
                    item.FromLocalName = string.Empty;
                }
                LocationEntity locaEntity2 = locaProvider.GetSingleByNum(item.ToLocalNum);
                if (locaEntity2.IsNotNull())
                {
                    item.ToLocalName = locaEntity2.LocalName;
                }
                else
                {
                    item.ToLocalName = string.Empty;
                }
            }
            string json = ConvertJson.ListToJson<InventoryBookEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 导出台帐报表Excel
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult ToInventoryReportExcel()
        {
            string searchKey = WebUtil.GetFormValue<string>("SearchKey", string.Empty);
            string change = WebUtil.GetFormValue<string>("Change", string.Empty);
            string beginTime = WebUtil.GetFormValue<string>("beginTime", string.Empty);
            string endTime = WebUtil.GetFormValue<string>("endTime", string.Empty);

            string storageNum = this.DefaultStore;

            InventoryProvider provider = new InventoryProvider();
            InventoryBookEntity entity = new InventoryBookEntity();

            if (!searchKey.IsEmpty())
            {
                entity.Begin<InventoryBookEntity>()
                 .Where<InventoryBookEntity>("ProductName", ECondition.Like, "%" + searchKey + "%")
                 .Or<InventoryBookEntity>("ProductNum", ECondition.Like, "%" + searchKey + "%")
                 .Or<InventoryBookEntity>("BarCode", ECondition.Like, "%" + searchKey + "%")
                 .End<InventoryBookEntity>();
            }
            if (!storageNum.IsEmpty())
            {
                entity.Where("StoreNum", ECondition.Eth, storageNum);
            }
            if (!change.IsEmpty())
            {
                entity.Where("Type", ECondition.Eth, change);
            }
            PageInfo pageInfo = new PageInfo() { PageIndex = 1, PageSize = Int32.MaxValue };
            List<InventoryBookEntity> listResult = provider.GetList(entity, ref pageInfo);
            LocationProvider locaProvider = new LocationProvider();
            foreach (InventoryBookEntity item in listResult)
            {
                LocationEntity locaEntity1 = locaProvider.GetSingleByNum(item.FromLocalNum);
                if (locaEntity1.IsNotNull())
                {
                    item.FromLocalName = locaEntity1.LocalName;
                }
                else
                {
                    item.FromLocalName = string.Empty;
                }
                LocationEntity locaEntity2 = locaProvider.GetSingleByNum(item.ToLocalNum);
                if (locaEntity2.IsNotNull())
                {
                    item.ToLocalName = locaEntity2.LocalName;
                }
                else
                {
                    item.ToLocalName = string.Empty;
                }
            }
            if (!listResult.IsNullOrEmpty())
            {

                Workbook book = new Workbook();
                Worksheet sheet = book.Worksheets[0];

                Action<Cell> action = (Cell cellItem) =>
                {
                    cellItem.Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    cellItem.Style.Borders[BorderType.BottomBorder].Color = System.Drawing.Color.Black;

                    cellItem.Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    cellItem.Style.Borders[BorderType.TopBorder].Color = System.Drawing.Color.Black;

                    cellItem.Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    cellItem.Style.Borders[BorderType.LeftBorder].Color = System.Drawing.Color.Black;

                    cellItem.Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    cellItem.Style.Borders[BorderType.RightBorder].Color = System.Drawing.Color.Black;
                };

                sheet.Name = "台帐报表";
                sheet.Cells.Merge(0, 0, 1, 11);

                //设置高度
                Cells cells = sheet.Cells;
                cells.SetRowHeight(0, 30);
                for (int i = 0; i < 11; i++)
                {
                    cells.SetColumnWidth(i, 15);
                }

                Cell cell = sheet.Cells[0, 0];
                cell.PutValue("台帐报表");
                cell.Style.HorizontalAlignment = TextAlignmentType.Center;
                cell.Style.Font.Color = System.Drawing.Color.Blue;
                cell.Style.Font.Size = 12;
                cell.Style.Font.IsBold = true;
                action(cell);

                int rowIndex = 1;
                sheet.Cells.SetRowHeight(1, 27);
                sheet.Cells[rowIndex, 0].PutValue("序号");
                sheet.Cells[rowIndex, 1].PutValue("产品编号");
                sheet.Cells[rowIndex, 2].PutValue("产品条码");
                sheet.Cells[rowIndex, 3].PutValue("产品名称");
                sheet.Cells[rowIndex, 4].PutValue("数量");
                sheet.Cells[rowIndex, 5].PutValue("台帐类型");
                sheet.Cells[rowIndex, 6].PutValue("关联订单号");
                sheet.Cells[rowIndex, 7].PutValue("原库位");
                sheet.Cells[rowIndex, 8].PutValue("目标库位");
                sheet.Cells[rowIndex, 9].PutValue("创建时间");
                sheet.Cells[rowIndex, 10].PutValue("操作人");
                for (int i = 0; i < 11; i++)
                {
                    sheet.Cells[rowIndex, i].Style.Font.IsBold = true;
                    action(sheet.Cells[rowIndex, i]);
                }
                rowIndex++;
                Dictionary<string, int> dic = new Dictionary<string, int>();

                string tempProductNum = listResult[0].ProductNum;
                int index = 0;
                int tempIndex = 1;
                foreach (InventoryBookEntity item in listResult)
                {
                    if (item.ProductNum == tempProductNum)
                    {
                        index++;
                    }
                    if (item.ProductNum != tempProductNum)
                    {
                        dic[tempProductNum] = index;
                        index = 1;
                    }
                    if (listResult.Count == tempIndex)
                    {
                        Dictionary<string, int>.KeyCollection keys = dic.Keys;
                        if (!keys.Exists(a => a == item.ProductNum))
                        {
                            dic[item.ProductNum] = index;
                        }

                    }
                    tempProductNum = item.ProductNum;
                    tempIndex++;
                }
                index = 0;
                int number = 1;
                foreach (InventoryBookEntity t in listResult)
                {
                    sheet.Cells.SetRowHeight(rowIndex, 25);
                    sheet.Cells[rowIndex, 0].PutValue(number);
                    if (index == 0)
                    {
                        index = dic[t.ProductNum];
                        sheet.Cells[rowIndex, 1].PutValue(t.ProductNum);
                        sheet.Cells[rowIndex, 2].PutValue(t.BarCode);
                        sheet.Cells[rowIndex, 3].PutValue(t.ProductName);
                        for (int i = 1; i < 4; i++)
                        {
                            sheet.Cells.Merge(rowIndex, i, index, 1);
                            sheet.Cells[rowIndex, i].Style.HorizontalAlignment = TextAlignmentType.Center;
                        }
                    }
                    sheet.Cells[rowIndex, 4].PutValue(t.Num);
                    sheet.Cells[rowIndex, 5].PutValue(EnumHelper.GetEnumDesc<EChange>(t.Type));
                    sheet.Cells[rowIndex, 6].PutValue(t.ContactOrder);
                    sheet.Cells[rowIndex, 7].PutValue(t.FromLocalName);
                    sheet.Cells[rowIndex, 8].PutValue(t.ToLocalName);
                    sheet.Cells[rowIndex, 9].PutValue(t.CreateTime.ToString("yyyy-MM-dd"));
                    sheet.Cells[rowIndex, 10].PutValue(t.UserName);

                    for (int i = 0; i < 11; i++)
                    {
                        //sheet.Cells[rowIndex, i].Style.Font.IsBold = true;
                        sheet.Cells[rowIndex, i].Style.Font.Size = 13;
                        action(sheet.Cells[rowIndex, i]);
                    }
                    number++;
                    rowIndex++;
                    index--;
                }

                string filePath = Server.MapPath("~/UploadFiles/");
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string filename = string.Format("台帐报表{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                book.Save(filePath + filename);
                this.ReturnJson.AddProperty("Path", ("/UploadFiles/" + filename).Escape());
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
