/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-10-01 11:08:42
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-10-01 11:08:42       情缘
*********************************************************************************/

using Git.Framework.Log;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Git.Storage.RoseEntity;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;
using Git.Storage.Conversion;

namespace Git.Storage.Rose.Server.Api
{
    public partial class Api_CategoryProvider
    {
        public Log log = Log.Instance(typeof(Api_CategoryProvider));

        public Api_CategoryProvider() { }

        /// <summary>
        /// 查询所有的货品种类
        /// </summary>
        /// <returns></returns>
        public List<ProductCategory_CE> GetList()
        {
            ProductCategoryProvider provider = new ProductCategoryProvider();
            List<ProductCategoryEntity> listResult=provider.GetList();
            if (!listResult.IsNullOrEmpty())
            {
                List<ProductCategory_CE> list = new List<ProductCategory_CE>();
                foreach (ProductCategoryEntity item in listResult)
                {
                    ProductCategory_CE ce = ProductCategory_To.ToCE(item);
                    list.Add(ce);
                }
                return list;
            }
            return null;
        }

        /// <summary>
        /// 查询类别信息
        /// </summary>
        /// <param name="cateNum"></param>
        /// <returns></returns>
        public ProductCategory_CE GetSingle(string cateNum)
        {
            if (cateNum.IsEmpty())
            {
                return null;
            }
            ProductCategoryProvider provider = new ProductCategoryProvider();
            ProductCategoryEntity entity = provider.GetSingle(cateNum);
            if (entity.IsNotNull())
            {
                ProductCategory_CE ce = ProductCategory_To.ToCE(entity);
                return ce;
            }
            return null;
        }


        /// <summary>
        /// 根据类别名称查询类别信息
        /// </summary>
        /// <param name="cateName"></param>
        /// <returns></returns>
        public ProductCategory_CE GetSingleCateName(string cateName)
        {
            if (cateName.IsEmpty())
            {
                return null;
            }
            ProductCategoryProvider provider = new ProductCategoryProvider();
            ProductCategoryEntity entity = provider.GetSingleCateName(cateName);
            if (entity.IsNotNull())
            {
                ProductCategory_CE ce = ProductCategory_To.ToCE(entity);
                return ce;
            }
            return null;
        }
    }
}