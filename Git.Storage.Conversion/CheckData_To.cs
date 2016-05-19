/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:20
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:20
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Check;

namespace Git.Storage.Conversion
{
	public partial class CheckData_To
	{
		public CheckData_To()
		{
		}

		public static CheckDataEntity To(CheckData_CE item)
		{
			CheckDataEntity target = new CheckDataEntity();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.LocalNum=item.LocalNum;
			target.LocalName=item.LocalName;
			target.StorageNum=item.StorageNum;
			target.ProductNum=item.ProductNum;
			target.BarCode=item.BarCode;
			target.ProductName=item.ProductName;
			target.LocalQty=item.LocalQty;
			target.FirstQty=item.FirstQty;
			target.SecondQty=item.SecondQty;
			target.DifQty=item.DifQty;
			target.FirstUser=item.FirstUser;
			target.SecondUser=item.SecondUser;
			target.CreateTime=item.CreateTime;
			return target;
		}

		public static CheckData_CE ToCE(CheckDataEntity item)
		{
			CheckData_CE target = new CheckData_CE();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.LocalNum=item.LocalNum;
			target.LocalName=item.LocalName;
			target.StorageNum=item.StorageNum;
			target.ProductNum=item.ProductNum;
			target.BarCode=item.BarCode;
			target.ProductName=item.ProductName;
			target.LocalQty=item.LocalQty;
			target.FirstQty=item.FirstQty;
			target.SecondQty=item.SecondQty;
			target.DifQty=item.DifQty;
			target.FirstUser=item.FirstUser;
			target.SecondUser=item.SecondUser;
			target.CreateTime=item.CreateTime;
			return target;
		}
	}
}
