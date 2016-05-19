/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:44:12
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:44:12
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Store;

namespace Git.Storage.Conversion
{
    public partial class Product_To
    {
        public Product_To()
        {
        }

        public static ProductEntity To(Product_CE item)
        {
            ProductEntity target = new ProductEntity();
            target.ID = item.ID;
            target.SnNum = item.SnNum;
            target.BarCode = item.BarCode;
            target.ProductName = item.ProductName;
            target.Num = item.Num;
            target.MinNum = item.MinNum;
            target.MaxNum = item.MaxNum;
            target.UnitNum = item.UnitNum;
            target.UnitName = item.UnitName;
            target.CateNum = item.CateNum;
            target.CateName = item.CateName;
            target.Size = item.Size;
            target.Color = item.Color;
            target.InPrice = item.InPrice;
            target.OutPrice = item.OutPrice;
            target.AvgPrice = item.AvgPrice;
            target.NetWeight = item.NetWeight;
            target.GrossWeight = item.GrossWeight;
            target.Description = item.Description;
            target.PicUrl = item.PicUrl;
            target.IsDelete = item.IsDelete;
            target.CreateTime = item.CreateTime;
            target.CreateUser = item.CreateUser;
            target.StorageNum = item.StorageNum;
            target.DefaultLocal = item.DefaultLocal;
            target.CusNum = item.CusNum;
            target.CusName = item.CusName;
            target.Display = item.Display;
            target.Remark = item.Remark;
            return target;
        }

        public static Product_CE ToCE(ProductEntity item)
        {
            Product_CE target = new Product_CE();
            target.ID = item.ID;
            target.SnNum = item.SnNum;
            target.BarCode = item.BarCode;
            target.ProductName = item.ProductName;
            target.Num = item.Num;
            target.MinNum = item.MinNum;
            target.MaxNum = item.MaxNum;
            target.UnitNum = item.UnitNum;
            target.UnitName = item.UnitName;
            target.CateNum = item.CateNum;
            target.CateName = item.CateName;
            target.Size = item.Size;
            target.Color = item.Color;
            target.InPrice = item.InPrice;
            target.OutPrice = item.OutPrice;
            target.AvgPrice = item.AvgPrice;
            target.NetWeight = item.NetWeight;
            target.GrossWeight = item.GrossWeight;
            target.Description = item.Description;
            target.PicUrl = item.PicUrl;
            target.IsDelete = item.IsDelete;
            target.CreateTime = item.CreateTime;
            target.CreateUser = item.CreateUser;
            target.StorageNum = item.StorageNum;
            target.DefaultLocal = item.DefaultLocal;
            target.CusNum = item.CusNum;
            target.CusName = item.CusName;
            target.Display = item.Display;
            target.Remark = item.Remark;
            return target;
        }
    }
}
