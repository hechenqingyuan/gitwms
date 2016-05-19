/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:43
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:43
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Move;

namespace Git.Storage.Conversion
{
	public partial class MoveOrderDetail_To
	{
		public MoveOrderDetail_To()
		{
		}

		public static MoveOrderDetailEntity To(MoveOrderDetail_CE item)
		{
			MoveOrderDetailEntity target = new MoveOrderDetailEntity();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.OrderNum=item.OrderNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.BatchNum=item.BatchNum;
			target.Num=item.Num;
			target.InPrice=item.InPrice;
			target.Amout=item.Amout;
			target.IsPick=item.IsPick;
			target.RealNum=item.RealNum;
			target.CreateTime=item.CreateTime;
			target.StorageNum=item.StorageNum;
			target.FromLocalNum=item.FromLocalNum;
			target.ToLocalNum=item.ToLocalNum;
			return target;
		}

		public static MoveOrderDetail_CE ToCE(MoveOrderDetailEntity item)
		{
			MoveOrderDetail_CE target = new MoveOrderDetail_CE();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.OrderNum=item.OrderNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.BatchNum=item.BatchNum;
			target.Num=item.Num;
			target.InPrice=item.InPrice;
			target.Amout=item.Amout;
			target.IsPick=item.IsPick;
			target.RealNum=item.RealNum;
			target.CreateTime=item.CreateTime;
			target.StorageNum=item.StorageNum;
			target.FromLocalNum=item.FromLocalNum;
			target.ToLocalNum=item.ToLocalNum;
			return target;
		}
	}
}
