/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-09-26 21:07:02
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-09-26 21:07:02       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Storage.Provider
{
    public static class CacheKey
    {
        public static string SESSION_LOGIN_ADMIN = "SESSION_LOGIN_ADMIN";

        //部门相关的缓存KEY值
        public static string JOOSHOW_SYSDEPART_CACHE = "JOOSHOW_SYSDEPART_CACHE";

        //角色相关的缓存KEY值
        public static string JOOSHOW_SYSROLE_CACHE = "JOOSHOW_SYSROLE_CACHE";

        /// <summary>
        /// 某个角色的权限缓存
        /// </summary>
        public static string JOOSHOW_ROLEPOWER_CACHE = "JOOSHOW_ROLEPOWER_CACHE_{0}";

        //菜单资源相关的缓存KEY值
        public static string JOOSHOW_SYSRESOURCE_CACHE = "JOOSHOW_SYSRESOURCE_CACHE";

        //供应商资源相关的缓存KEY值
        public static string JOOSHOW_SUPPLIER_CACHE = "JOOSHOW_SUPPLIER_CACHE";

        //设备管理资源相关的缓存KEY值
        public static string JOOSHOW_EQUIPMENT_CACHE = "JOOSHOW_EQUIPMENT_CACHE";

        //仓库资源相关的缓存KEY值
        public static string JOOSHOW_STORAGE_CACHE = "JOOSHOW_STORAGE_CACHE";

        //库位资源相关的缓存KEY值
        public static string JOOSHOW_LOCATION_CACHE = "JOOSHOW_LOCATION_CACHE";

        //客户收货单位资源相关的缓存KEY值CusAddress
        public static string JOOSHOW_CUSADDRESS_CACHE = "JOOSHOW_CUSADDRESS_CACHE";

        //产品组成关系相关的缓存KEY值
        public static string JOOSHOW_PRODUCTREL_CACHE = "JOOSHOW_PRODUCTREL_CACHE";

        //产品类别管理缓存
        public static string JOOSHOW_PRODUCTCATEGORY_CACHE = "JOOSHOW_PRODUCTCATEGORY_CACHE";

        //产品缓存
        public static string JOOSHOW_PRODUCT_CACHE = "JOOSHOW_PRODUCT_CACHE";

        /// <summary>
        /// 已经分配的权限缓存
        /// </summary>
        public static string JOOSHOW_ALLOTPOWER_CACHE = "JOOSHOW_ALLOTPOWER_CACHE";

        /// <summary>
        /// 入库单产品缓存
        /// </summary>
        public static string TEMPDATA_CACHE_INSTORDETAIL = "TEMPDATA_CACHE_INSTORDETAIL";

        /// <summary>
        /// 订单单产品缓存
        /// </summary>
        public static string TEMPDATA_CACHE_ORDERDETAIL = "TEMPDATA_CACHE_ORDERDETAIL";

        /// <summary>
        /// 订单导入产品缓存
        /// </summary>
        public static string TEMPDATA_CACHE_ORDERIMPORT = "TEMPDATA_CACHE_ORDERIMPORT";

        /// <summary>
        /// 订单导入产品详细缓存
        /// </summary>
        public static string TEMPDATA_CACHE_ORDERDETAILIMPORT = "TEMPDATA_CACHE_ORDERDETAILIMPORT";

        /// <summary>
        /// 出库产品缓存
        /// </summary>
        public static string TEMPDATA_CACHE_OUTSTORDETAIL = "TEMPDATA_CACHE_OUTSTORDETAIL";

        /// <summary>
        /// 报损产品缓存
        /// </summary>
        public static string TEMPDATA_CACHE_BADPRODUCTDETAIL = "TEMPDATA_CACHE_BADPRODUCTDETAIL";

        /// <summary>
        /// 移库产品缓存
        /// </summary>
        public static string TEMPDATA_CACHE_MOVERODUCTDETAIL = "TEMPDATA_CACHE_MOVERODUCTDETAIL";

        /// <summary>
        /// 退货详情
        /// </summary>
        public static string TEMPDATE_CACHE_RETURNPRODUCTDETAIL = "TEMPDATE_CACHE_RETURNPRODUCTDETAIL";

        /// <summary>
        /// 当前库位编号
        /// </summary>
        public static string SESSION_STORAGE_CURRENT = "SESSION_STORAGE_CURRENT";

        /// <summary>
        /// 当前库位
        /// </summary>
        public static string SESSION_STORAGE_CACHE = "SESSION_STORAGE_CACHE";

        /// <summary>
        /// 菜单的显示状态问题
        /// </summary>
        public static string SESSION_MENU_STATUS = "SESSION_MENU_STATUS";

        /// <summary>
        /// 获得系统版本
        /// </summary>
        public static string CACHE_SYS_VERSION = "CACHE_SYS_VERSION";

        /// <summary>
        /// 计量单位的统计
        /// </summary>
        public static string JOOSHOW_MEASURE_CACHE = "JOOSHOW_MEASURE_CACHE";

        /// <summary>
        /// 计量单位换算
        /// </summary>
        public static string JOOSHOW_MEASUREREL_CACHE = "JOOSHOW_MEASUREREL_CACHE";

        /// <summary>
        /// 用于盘点单详细产品盘点
        /// </summary>
        public static string JOOSHOW_CHECKDETAIL_CACHE = "JOOSHOW_CHECKDETAIL_CACHE";

        /// <summary>
        /// 报表管理参数
        /// </summary>
        public static string JOOSHOW_REPORTPARAM_CACHE = "JOOSHOW_REPORTPARAM_CACHE";

    }
}
