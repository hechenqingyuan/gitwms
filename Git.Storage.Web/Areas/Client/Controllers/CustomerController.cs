using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.Controller;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Client;
using Git.Framework.DataTypes;
using Git.Storage.Common;
using Git.Storage.Web.Lib.Filter;
using Git.Storage.Provider;

namespace Git.Storage.Web.Areas.Client.Controllers
{
    public class CustomerController : MasterPage
    {
        /// <summary>
        /// 客户信息列表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Index()
        {
            ViewBag.DdlCusType = EnumHelper.GetOptions<ECusType>("", "请选择客户类别");
            return View();
        }


        /// <summary>
        /// 添加客户
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Add()
        {
            Session[CacheKey.JOOSHOW_CUSADDRESS_CACHE] = null;
            string CusNum = WebUtil.GetQueryStringValue<string>("CusNum", string.Empty);
            if (CusNum.IsEmpty())
            {
                ViewBag.Customer = new CustomerEntity();
                ViewBag.DdlCusType = EnumHelper.GetOptions<ECusType>("","请选择客户类别");
                return View();
            }
            else
            {
                CustomerProvider provider = new CustomerProvider();
                CustomerEntity entity = provider.GetSingleCustomer(CusNum);
                entity = entity == null ? new CustomerEntity() : entity;
                ViewBag.DdlCusType = EnumHelper.GetOptions<ECusType>(entity.CusType, "请选择客户类别");
                ViewBag.Customer = entity;

                List<CusAddressEntity> listResult = provider.GetAddressList(entity.CusNum);
                if (!listResult.IsNullOrEmpty())
                {
                    Session[CacheKey.JOOSHOW_CUSADDRESS_CACHE] = listResult;
                }
                return View();
            }
        }

        /// <summary>
        /// 新增客户地址
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Address()
        {
            string SnNum = WebUtil.GetQueryStringValue<string>("SnNum", string.Empty);
            if (SnNum.IsEmpty())
            {
                ViewBag.CusAddress = new CusAddressEntity();
                return View();
            }
            else
            {
                List<CusAddressEntity> list = Session[CacheKey.JOOSHOW_CUSADDRESS_CACHE] as List<CusAddressEntity>;
                if (!list.IsNullOrEmpty())
                {
                    CusAddressEntity entity = list.FirstOrDefault(a => a.SnNum == SnNum);
                    entity = entity.IsNull() ? new CusAddressEntity() : entity;
                    ViewBag.CusAddress = entity;
                }
                else
                {
                    CustomerProvider provider = new CustomerProvider();
                    CusAddressEntity entity = provider.GetSingleAddress(SnNum);
                    entity = entity == null ? new CusAddressEntity() : entity;
                    ViewBag.CusAddress = entity;
                }
                return View();
            }
        }

        /// <summary>
        /// 选择客户对话框
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Dialog()
        {
            return View();
        }
    }
}
