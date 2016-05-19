/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-12-13 14:53:41
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-12-13 14:53:41       情缘
*********************************************************************************/

using Git.Storage.Entity.Base;
using Git.Storage.Provider.Base;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Client;
using Git.Storage.Common;

namespace Git.Storage.Web.Lib
{
    public class BaseHelper
    {
        /// <summary>
        /// 获得角色的下拉列表
        /// </summary>
        /// <param name="roleNum">角色编号</param>
        /// <returns></returns>
        public static string GetRoleList(string roleNum)
        {
            SysRoleProvider provider = new SysRoleProvider();
            List<SysRoleEntity> list = provider.GetList();
            StringBuilder sb = new StringBuilder();
            string roleTemplate = "<option value='{0}' {1}>{2}</option>";
            sb.AppendFormat(roleTemplate, "", "", "请选择角色");
            if (!list.IsNullOrEmpty())
            {
                foreach (SysRoleEntity role in list)
                {
                    sb.AppendFormat(roleTemplate, role.RoleNum, role.RoleNum == roleNum ? "selected='selected'" : string.Empty, role.RoleName);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 客户下拉列表
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        public static string GetCustomerList(string cusNum)
        {
            CustomerProvider provider = new CustomerProvider();
            List<CustomerEntity> list = provider.GetList();
            StringBuilder sb = new StringBuilder();
            string CusTemplate = "<option value='{0}' {1}>{2}</option>";
            sb.AppendFormat(CusTemplate, "", "", "请选择客户");
            if (!list.IsNullOrEmpty())
            {
                foreach (CustomerEntity cus in list)
                {
                    sb.AppendFormat(CusTemplate, cus.CusNum, cus.CusNum == cusNum ? "selected='selected'" : string.Empty, cus.CusName);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 供应商下拉列表
        /// </summary>
        /// <param name="roleNum"></param>
        /// <returns></returns>
        public static string GetSupNameList(string SupNum)
        {
            SupplierProvider provider = new SupplierProvider();
            List<SupplierEntity> list = provider.GetList();
            StringBuilder sb = new StringBuilder();
            string storeTemplate = "<option value='{0}' {1}>{2}</option>";
            if (!list.IsNullOrEmpty())
            {
                foreach (SupplierEntity store in list)
                {
                    sb.AppendFormat(storeTemplate, store.SupNum, store.SupNum == SupNum ? "selected='selected'" : string.Empty, store.SupName);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 产品单位
        /// </summary>
        /// <param name="MeasureNum"></param>
        /// <returns></returns>
        public static string GetMeasureNameList(string MeasureNum)
        {
            MeasureProvider provider = new MeasureProvider();
            List<MeasureEntity> list = provider.GetList();
            StringBuilder sb = new StringBuilder();
            string template = "<option value='{0}' {1}>{2}</option>";
            sb.AppendFormat(template, "", "", "请选择单位");
            if (!list.IsNullOrEmpty())
            {
                foreach (MeasureEntity measure in list)
                {
                    sb.AppendFormat(template, measure.MeasureNum, measure.MeasureNum == MeasureNum ? "selected='selected'" : string.Empty, measure.MeasureName);
                }
            }
            return sb.ToString();

        }

        /// <summary>
        /// 获得部门的下来列表
        /// </summary>
        /// <param name="departNum"></param>
        /// <returns></returns>
        public static string GetDepartList(string departNum)
        {
            DepartProvider provider = new DepartProvider();
            List<SysDepartEntity> list = provider.GetList();
            StringBuilder sb = new StringBuilder();
            string departTemplate = "<option value='{0}' {1}>{2}</option>";
            sb.AppendFormat(departTemplate, "", "", "请选择部门");
            if (!list.IsNullOrEmpty())
            {
                foreach (SysDepartEntity depart in list)
                {
                    sb.AppendFormat(departTemplate, depart.DepartNum, depart.DepartNum == departNum ? "selected='selected'" : string.Empty, depart.DepartName);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获得产品的类别
        /// </summary>
        /// <param name="cateNum"></param>
        /// <returns></returns>
        public static string GetProductCategory(string cateNum)
        {
            ProductCategoryProvider provider = new ProductCategoryProvider();
            List<ProductCategoryEntity> list = provider.GetList();
            StringBuilder sb = new StringBuilder();
            string departTemplate = "<option value='{0}' {1}>{2}</option>";
            sb.AppendFormat(departTemplate, "", "", "请选择类别");
            if (!list.IsNullOrEmpty())
            {
                foreach (ProductCategoryEntity depart in list)
                {
                    sb.AppendFormat(departTemplate, depart.CateNum, depart.CateNum == cateNum ? "selected='selected'" : string.Empty, depart.CateName);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获得父类菜单
        /// </summary>
        /// <param name="parentNum"></param>
        /// <returns></returns>
        public static string GetParentMenu(string parentNum)
        {
            SysResourceProvider sysRes = new SysResourceProvider();
            List<SysResourceEntity> list = sysRes.GetList();
            StringBuilder sb = new StringBuilder();
            string menuTemplate = "<option value='{0}' {1}>{2}</option>";
            sb.AppendFormat(menuTemplate, "", "", "请选择");
            if (!list.IsNullOrEmpty())
            {
                foreach (SysResourceEntity entity in list.Where(a => a.ResType == (short)EResourceType.Page))
                {
                    sb.AppendFormat(menuTemplate, entity.ResNum, entity.ResNum == parentNum ? "selected='selected'" : string.Empty, entity.ResName);
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 获得数据类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string GetDataType(string dbType)
        {
            List<string> list = new List<string>() 
            { 
                "varchar","nvarchar","text","datetime","int"
            };
            StringBuilder sb = new StringBuilder();
            string menuTemplate = "<option value='{0}' {1}>{2}</option>";
            sb.AppendFormat(menuTemplate, "", "", "请选择");
            if (!list.IsNullOrEmpty())
            {
                foreach (string item in list)
                {
                    sb.AppendFormat(menuTemplate, item, item == dbType ? "selected='selected'" : string.Empty, item);
                }
            }
            return sb.ToString();
        }
    }
}