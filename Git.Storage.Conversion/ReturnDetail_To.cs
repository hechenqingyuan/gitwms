/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:12
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:12
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Return;

namespace Git.Storage.Conversion
{
	public partial class ReturnDetail_To
	{
		public ReturnDetail_To()
		{
		}

		public static ReturnDetailEntity To(ReturnDetail_CE item)
		{
			ReturnDetailEntity target = new ReturnDetailEntity();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.OrderNum=item.OrderNum;
			target.ContractOrder=item.ContractOrder;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.BatchNum=item.BatchNum;
			target.LocalNum=item.LocalNum;
			target.StorageNum=item.StorageNum;
			target.Num=item.Num;
			target.OutPrice=item.OutPrice;
			target.Amount=item.Amount;
			target.BackNum=item.BackNum;
			target.BackAmount=item.BackAmount;
			target.CreateTime=item.CreateTime;
			return target;
		}

		public static ReturnDetail_CE ToCE(ReturnDetailEntity item)
		{
			ReturnDetail_CE target = new ReturnDetail_CE();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.OrderNum=item.OrderNum;
			target.ContractOrder=item.ContractOrder;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.BatchNum=item.BatchNum;
			target.LocalNum=item.LocalNum;
			target.StorageNum=item.StorageNum;
			target.Num=item.Num;
			target.OutPrice=item.OutPrice;
			target.Amount=item.Amount;
			target.BackNum=item.BackNum;
			target.BackAmount=item.BackAmount;
			target.CreateTime=item.CreateTime;
			return target;
		}
	}
}
