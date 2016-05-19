/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-10-01 10:18:06
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-10-01 10:18:06       情缘
*********************************************************************************/

using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.ORM;
using Git.Framework.Log;
using Git.Storage.RoseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Git.Storage.Entity.Store;
using Git.Storage.Conversion;
using Git.Storage.Provider.Store;

namespace Git.Storage.Rose.Server.Api
{
    public partial class Api_ProductProvider
    {
        private Log log = Log.Instance(typeof(Api_ProductProvider));

        public Api_ProductProvider() { }

        /// <summary>
        /// 根据产品SN号码查询产品的详细信息
        /// SNNum为产品的唯一编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Product_CE GetProductBySn(string snNum)
        {
            if (snNum.IsEmpty())
            {
                return null;
            }
            ProductProvider provider = new ProductProvider();
            ProductEntity entity = provider.GetProductBySn(snNum);
            if (entity != null)
            {
                Product_CE productCe = Product_To.ToCE(entity);
                return productCe;
            }
            return null;
        }

        /// <summary>
        /// 根据产品条码查询产品的集合
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Product_CE> GetListByBarCode(string barCode)
        {
            if (barCode.IsEmpty())
            {
                return null;
            }
            ProductProvider provider = new ProductProvider();
            List<ProductEntity> list = provider.GetProductByBarCode(barCode);
            if (!list.IsNullOrEmpty())
            {
                List<Product_CE> listResult = new List<Product_CE>();
                foreach (ProductEntity iten in list)
                {
                    Product_CE ce = Product_To.ToCE(iten);
                    listResult.Add(ce);
                }
                return listResult;
            }
            return null;
        }

        /// <summary>
        /// 查询所有的产信息
        /// </summary>
        /// <returns></returns>
        public List<Product_CE> GetList()
        {
            ProductProvider provider = new ProductProvider();
            List<ProductEntity> list = provider.GetListByCache();
            if (!list.IsNullOrEmpty())
            {
                List<Product_CE> listResult = new List<Product_CE>();
                foreach (ProductEntity iten in list)
                {
                    Product_CE ce = Product_To.ToCE(iten);
                    listResult.Add(ce);
                }
                return listResult;
            }
            return null;
        }

        /// <summary>
        /// 新增产品信息
        /// </summary>
        /// <param name="ce"></param>
        /// <returns></returns>
        public int Add(Product_CE ce)
        {
            if (ce != null)
            {
                ProductEntity entity = Product_To.To(ce);
                ProductProvider provider = new ProductProvider();
                int line = provider.Add(entity);
                return line;
            }
            return 0;
        }
    }
}