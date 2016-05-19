using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Git.Framework.Controller;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Client;
using Git.Storage.Web.Lib;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Json;
using Git.Framework.Controller.Mvc;
using Git.Storage.Provider;
using Git.Framework.Cache;
using Git.Framework.DataTypes;
using Git.Framework.ORM;
using Git.Storage.Common;
using Git.Storage.Web.Lib.Filter;
using System.Text;
using Git.Storage.Provider.Base;

namespace Git.Storage.Web.Areas.Client.Controllers
{
    public class CustomerAjaxController : AjaxPage
    {
        /// <summary>
        /// 获得客户列表
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetCustomerList()
        {
            int pageIndex = WebUtil.GetFormValue<int>("pageIndex", 1);
            int pageSize = WebUtil.GetFormValue<int>("pageSize", 15);
            string CusNum = WebUtil.GetFormValue<string>("CusNum", string.Empty);
            int CusType = WebUtil.GetFormValue<int>("CusType", 0);

            CustomerProvider provider = new CustomerProvider();
            CustomerEntity entity = new CustomerEntity();
            PageInfo pageInfo = new PageInfo() { PageIndex = pageIndex, PageSize = pageSize };
            if (!CusNum.IsEmpty())
            {
                entity.Begin<CustomerEntity>()
                   .Where<CustomerEntity>("CusNum", ECondition.Like, "%" + CusNum + "%")
                   .Or<CustomerEntity>("CusName", ECondition.Like, "%" + CusNum + "%")
                   .End<CustomerEntity>();
            }
            if (CusType != 0)
            {
                entity.Where<CustomerEntity>(a => a.CusType == CusType);
            }

            List<CustomerEntity> listResult = provider.GetCustomerList(entity, ref pageInfo);
            string json = ConvertJson.ListToJson<CustomerEntity>(listResult, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            this.ReturnJson.AddProperty("RowCount", pageInfo.RowCount);
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddCustomer([ModelBinder(typeof(JsonBinder<CustomerEntity>))] CustomerEntity entity)
        {
            List<CusAddressEntity> list = Session[CacheKey.JOOSHOW_CUSADDRESS_CACHE] as List<CusAddressEntity>;
            list = list == null ? new List<CusAddressEntity>() : list;
            CustomerProvider provider = new CustomerProvider();
            int line = 0;
            if (entity.CusNum.IsEmpty())
            {
                entity.CreateTime = DateTime.Now;
                entity.CusNum = SequenceProvider.GetSequence(typeof(CustomerEntity));
                line = provider.AddCustomer(entity, list);
            }
            else
            {
                line = provider.Update(entity, list);
            }
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="cusNum"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Delete(string cusNum)
        {
            CustomerProvider provider = new CustomerProvider();
            int line = provider.Delete(cusNum);
            if (line > 0)
            {
                this.ReturnJson.AddProperty("d", "success");
            }
            else
            {
                this.ReturnJson.AddProperty("d", "");
            }
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="cusNum"></param>
        /// <param name="snNum"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult DelCusAddress(string cusNum, string snNum)
        {
            List<CusAddressEntity> list = Session[CacheKey.JOOSHOW_CUSADDRESS_CACHE] as List<CusAddressEntity>;
            list = list == null ? new List<CusAddressEntity>() : list;
            if (list.Exists(a => a.SnNum == snNum))
            {
                list.Remove(a=>a.SnNum==snNum);
                CustomerProvider provider = new CustomerProvider();
                provider.DelCusAddress(snNum, cusNum);
            }
            Session[CacheKey.JOOSHOW_CUSADDRESS_CACHE] = list;
            this.ReturnJson.AddProperty("d", "success");
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 添加或修改地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult AddAddress([ModelBinder(typeof(JsonBinder<CusAddressEntity>))] CusAddressEntity entity)
        {
            List<CusAddressEntity> list = Session[CacheKey.JOOSHOW_CUSADDRESS_CACHE] as List<CusAddressEntity>;
            list = list == null ? new List<CusAddressEntity>() : list;
            if (entity.SnNum.IsEmpty())
            {
                entity.CreateTime = DateTime.Now;
                entity.SnNum = SequenceProvider.GetSequence(typeof(CusAddressEntity));
                list.Add(entity);
                this.ReturnJson.AddProperty("d", "success");
            }
            else
            {
                list.ForEach(a =>
                {
                    if (a.SnNum == entity.SnNum)
                    {
                        a.Address = entity.Address;
                        a.Contact = entity.Contact;
                        a.Phone = entity.Phone;
                    }
                });
                this.ReturnJson.AddProperty("d", "success");
            }

            Session[CacheKey.JOOSHOW_CUSADDRESS_CACHE] = list;
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 获得所有的地址
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetAddList()
        {
            //string CusNum = WebUtil.GetFormValue<string>("CusNum", string.Empty);
            //List<CusAddressEntity> list = null;
            //if (!CusNum.IsEmpty())
            //{
            //    CustomerProvider provider = new CustomerProvider();
            //    list = provider.GetAddressList(CusNum);
            //}
            List<CusAddressEntity> list = Session[CacheKey.JOOSHOW_CUSADDRESS_CACHE] as List<CusAddressEntity>;
            list = list == null ? new List<CusAddressEntity>() : list;
            string json = ConvertJson.ListToJson<CusAddressEntity>(list, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 获得客户的可选地址
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetSelectAddress()
        {
            string CusNum = WebUtil.GetFormValue<string>("CusNum", string.Empty);
            List<CusAddressEntity> list = null;
            if (!CusNum.IsEmpty())
            {
                CustomerProvider provider = new CustomerProvider();
                list = provider.GetAddressList(CusNum);
            }
            list = list == null ? new List<CusAddressEntity>() : list;
            string json = ConvertJson.ListToJson<CusAddressEntity>(list, "List");
            this.ReturnJson.AddProperty("Data", new JsonObject(json));
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult RemoveCache()
        {
            CacheHelper.Remove(CacheKey.JOOSHOW_CUSADDRESS_CACHE);
            this.ReturnJson.AddProperty("d", "");
            return Content(this.ReturnJson.ToString());
        }

        /// <summary>
        /// 获客户详细
        /// </summary>
        /// <param name="supName"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult Auto(string cusName)
        {
            CustomerProvider provider = new CustomerProvider();
            List<CustomerEntity> list = provider.GetList();
            if (!list.IsNullOrEmpty() && !cusName.IsEmpty())
            {
                list = list.Where(a => a.CusNum.Contains(cusName) || a.CusName.Contains(cusName)).ToList();
            }
            list = list.IsNull() ? new List<CustomerEntity>() : list;
            StringBuilder sb = new StringBuilder();
            JsonObject jsonObject = null;
            foreach (CustomerEntity t in list)
            {
                jsonObject = new JsonObject();
                jsonObject.AddProperty("CusNum", t.CusNum);
                jsonObject.AddProperty("CusName", t.CusName);
                //jsonObject.AddProperty("Phone", t.Phone);
                sb.Append(jsonObject.ToString() + "\n");

            }
            if (sb.Length == 0)
            {
                sb.Append("\n");
            }
            return Content(sb.ToString());
        }


        /// <summary>
        /// 获得联系人
        /// </summary>
        /// <param name="supName"></param>
        /// <returns></returns>
        [LoginAjaxFilter]
        public ActionResult GetContact()
        {
            string snNum = WebUtil.GetFormValue<string>("SnNum",string.Empty);
            CustomerProvider provider = new CustomerProvider();
            CusAddressEntity  entity= new CusAddressEntity();
            entity = provider.GetSingleAddress(snNum);
            if (entity.IsNotNull())
            {
                StringBuilder sb = new StringBuilder();
                List<CusAddressEntity> listResult = new List<CusAddressEntity>();
                listResult.Add(entity);
                string json = ConvertJson.ListToJson(listResult, "List");
                this.ReturnJson.AddProperty("Data", new JsonObject(json));
            }
            return Content(this.ReturnJson.ToString());
        }

    }
}
