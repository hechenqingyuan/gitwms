/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:05
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:05
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Check;

namespace Git.Storage.Conversion
{
	public partial class CheckStockInfo_To
	{
		public CheckStockInfo_To()
		{
		}

		public static CheckStockInfoEntity To(CheckStockInfo_CE item)
		{
			CheckStockInfoEntity target = new CheckStockInfoEntity();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.StorageNum=item.StorageNum;
			target.TargetNum=item.TargetNum;
			target.CreateTime=item.CreateTime;
			return target;
		}

		public static CheckStockInfo_CE ToCE(CheckStockInfoEntity item)
		{
			CheckStockInfo_CE target = new CheckStockInfo_CE();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.StorageNum=item.StorageNum;
			target.TargetNum=item.TargetNum;
			target.CreateTime=item.CreateTime;
			return target;
		}
	}
}
