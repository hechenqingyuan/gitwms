/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:33
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:33
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Store;

namespace Git.Storage.Conversion
{
	public partial class InventoryBook_To
	{
		public InventoryBook_To()
		{
		}

		public static InventoryBookEntity To(InventoryBook_CE item)
		{
			InventoryBookEntity target = new InventoryBookEntity();
			target.ID=item.ID;
			target.ProductNum=item.ProductNum;
			target.BarCode=item.BarCode;
			target.ProductName=item.ProductName;
			target.Num=item.Num;
			target.Type=item.Type;
			target.ContactOrder=item.ContactOrder;
			target.FromLocalNum=item.FromLocalNum;
			target.ToLocalNum=item.ToLocalNum;
			target.StoreNum=item.StoreNum;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			return target;
		}

		public static InventoryBook_CE ToCE(InventoryBookEntity item)
		{
			InventoryBook_CE target = new InventoryBook_CE();
			target.ID=item.ID;
			target.ProductNum=item.ProductNum;
			target.BarCode=item.BarCode;
			target.ProductName=item.ProductName;
			target.Num=item.Num;
			target.Type=item.Type;
			target.ContactOrder=item.ContactOrder;
			target.FromLocalNum=item.FromLocalNum;
			target.ToLocalNum=item.ToLocalNum;
			target.StoreNum=item.StoreNum;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			return target;
		}
	}
}
