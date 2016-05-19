using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Storage.Web.Lib.Filter;
using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Framework.Controller;
using Git.Storage.Provider;
using Git.Storage.Entity.Bad;
using Git.Storage.Provider.Bad;
using Git.Framework.DataTypes.ExtensionMethods;

namespace Git.Storage.Web.Areas.Bad.Controllers
{
    public class ProductController : MasterPage
    {
        /// <summary>
        /// 添加报损清单
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] = null;
            ViewBag.BadType = EnumHelper.GetOptions<EBadType>(EBadType.Bad, "请选择报损类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(EProductType.Goods, "请选择入库产品类型");
            return View();
        }

        /// <summary>
        /// 添加报损产品选择页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult AddProduct()
        {
            return View();
        }

        /// <summary>
        /// 报损清单详细页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Detail()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum", string.Empty);
            string flag = WebUtil.GetQueryStringValue<string>("flag", string.Empty);
            Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
            BadReportEntity entity = new BadReportEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            entity = entity.IsNull() ? new BadReportEntity() : entity;
            entity.StatusLable = EnumHelper.GetEnumDesc<EAudite>(entity.Status);
            ViewBag.BadReport = entity;

            BadReportDetailEntity detail = new BadReportDetailEntity();
            detail.OrderNum = orderNum;
            List<BadReportDetailEntity> listResult = bill.GetOrderDetail(detail);
            listResult = listResult.IsNull() ? new List<BadReportDetailEntity>() : listResult;
            ViewBag.Detail = listResult;

            ViewBag.Flag = flag;
            return View();
        }

        /// <summary>
        /// 报损清单编辑页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Edit()
        {
            string orderNum = WebUtil.GetQueryStringValue<string>("orderNum",string.Empty);
            if (orderNum.IsEmpty())
            {
                Response.Redirect("/Bad/Product/List");
            }
            Bill<BadReportEntity, BadReportDetailEntity> bill = new BadOrder();
            BadReportEntity entity = new BadReportEntity();
            entity.OrderNum = orderNum;
            entity = bill.GetOrder(entity);
            if (entity.IsNull())
            {
                Response.Redirect("/Bad/Product/List");
            }
            ViewBag.BadType = EnumHelper.GetOptions<EBadType>(entity.BadType, "请选择报损类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(entity.ProductType, "请选择入库产品类型");
            ViewBag.Entity = entity;
            BadReportDetailEntity detail = new BadReportDetailEntity();
            detail.OrderNum = orderNum;
            List<BadReportDetailEntity> listDetail = bill.GetOrderDetail(detail);
            Session[CacheKey.TEMPDATA_CACHE_BADPRODUCTDETAIL] = listDetail;
            return View();
        }

        /// <summary>
        /// 报损清单管理列表页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult List()
        {
            ViewBag.BadType = EnumHelper.GetOptions<EBadType>(-1, "请选择报损类型");
            ViewBag.ProductType = EnumHelper.GetOptions<EProductType>(-1, "请选择入库产品类型");
            return View();
        }
    }
}
