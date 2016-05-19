/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:46:32
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:46:32
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.InStorage;

namespace Git.Storage.Conversion
{
	public partial class InStorDetail_To
	{
		public InStorDetail_To()
		{
		}

		public static InStorDetailEntity To(InStorDetail_CE item)
		{
			InStorDetailEntity target = new InStorDetailEntity();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.OrderNum=item.OrderNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.BatchNum=item.BatchNum;
			target.Num=item.Num;
			target.IsPick=item.IsPick;
			target.RealNum=item.RealNum;
			target.InPrice=item.InPrice;
			target.Amount=item.Amount;
			target.CreateTime=item.CreateTime;
			target.LocalNum=item.LocalNum;
			target.StorageNum=item.StorageNum;
			return target;
		}

		public static InStorDetail_CE ToCE(InStorDetailEntity item)
		{
			InStorDetail_CE target = new InStorDetail_CE();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.OrderNum=item.OrderNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.BatchNum=item.BatchNum;
			target.Num=item.Num;
			target.IsPick=item.IsPick;
			target.RealNum=item.RealNum;
			target.InPrice=item.InPrice;
			target.Amount=item.Amount;
			target.CreateTime=item.CreateTime;
			target.LocalNum=item.LocalNum;
			target.StorageNum=item.StorageNum;
			return target;
		}
	}
}
