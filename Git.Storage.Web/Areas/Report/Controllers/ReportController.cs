using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;
using Git.Storage.Web.Lib;
using Git.Storage.Web.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Report.Controllers
{
    public class ReportController : MasterPage
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 库存清单报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult StockBillReport()
        {
            ViewBag.LocalTypeList = EnumHelper.GetOptions<ELocalType>(string.Empty, "请选择库位类型");
            return View();
        }

        /// <summary>
        /// 产品在线库存报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult ProductReport()
        {
            return View();
        }

        /// <summary>
        /// 产品出入库报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult ProductInOutReport()
        {
            return View();
        }

        /// <summary>
        /// 入库报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult InStorageReport()
        {
            return View();
        }

        /// <summary>
        /// 出库报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult OutStorageReport()
        {
            return View();
        }

        /// <summary>
        /// 客户报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult CustomerReport()
        {
            return View();
        }

        /// <summary>
        /// 供应商报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult SupplierReport()
        {
            return View();
        }

        /// <summary>
        /// 报损报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult BadReport()
        {
            return View();
        }

        /// <summary>
        /// 退货报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult ReturnReport()
        {
            return View();
        }

        /// <summary>
        /// 仓库库存记录变化台账报表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult InventoryReport()
        {
            ViewBag.ChangeList = EnumHelper.GetOptions<EChange>(string.Empty, "请选择台帐类型");
            return View();
        }

        /// <summary>
        ///  产品明细
        /// </summary>
        /// <param name="snNum"></param>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult GoodsDetail(string snNum)
        {

            ProductEntity entity = new ProductEntity();
            ProductProvider provider = new ProductProvider();
            entity = provider.GetProductBySn(snNum);
            ViewBag.Category = BaseHelper.GetProductCategory(entity.CateNum);
            ViewBag.Storage = LocalHelper.GetStorageNumList(entity.StorageNum);
            ViewBag.Local = LocalHelper.GetLocalNumList(entity.StorageNum, entity.DefaultLocal);
            ViewBag.Customer = BaseHelper.GetCustomerList(entity.CusNum);
            ViewBag.Goods = entity;
            //ViewBag.Unit = EnumHelper.GetOptions<EUnit>(entity.Unit, "请选择单位");
            ViewBag.Unit = BaseHelper.GetMeasureNameList(entity.UnitNum);
            return View();
        }
    }
}
