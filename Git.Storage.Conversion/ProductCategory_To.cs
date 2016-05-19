/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:43:45
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:43:45
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Store;

namespace Git.Storage.Conversion
{
	public partial class ProductCategory_To
	{
		public ProductCategory_To()
		{
		}

		public static ProductCategoryEntity To(ProductCategory_CE item)
		{
			ProductCategoryEntity target = new ProductCategoryEntity();
			target.ID=item.ID;
			target.CateNum=item.CateNum;
			target.CateName=item.CateName;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.Remark=item.Remark;
			return target;
		}

		public static ProductCategory_CE ToCE(ProductCategoryEntity item)
		{
			ProductCategory_CE target = new ProductCategory_CE();
			target.ID=item.ID;
			target.CateNum=item.CateNum;
			target.CateName=item.CateName;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.Remark=item.Remark;
			return target;
		}
	}
}
