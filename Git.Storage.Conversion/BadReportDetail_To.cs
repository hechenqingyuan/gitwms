/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:54
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:54
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Bad;

namespace Git.Storage.Conversion
{
	public partial class BadReportDetail_To
	{
		public BadReportDetail_To()
		{
		}

		public static BadReportDetailEntity To(BadReportDetail_CE item)
		{
			BadReportDetailEntity target = new BadReportDetailEntity();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.OrderNum=item.OrderNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.BatchNum=item.BatchNum;
			target.Num=item.Num;
			target.InPrice=item.InPrice;
			target.Amount=item.Amount;
			target.CreateTime=item.CreateTime;
			target.StorageNum=item.StorageNum;
			target.FromLocalNum=item.FromLocalNum;
			target.ToLocalNum=item.ToLocalNum;
			return target;
		}

		public static BadReportDetail_CE ToCE(BadReportDetailEntity item)
		{
			BadReportDetail_CE target = new BadReportDetail_CE();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.OrderNum=item.OrderNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.BatchNum=item.BatchNum;
			target.Num=item.Num;
			target.InPrice=item.InPrice;
			target.Amount=item.Amount;
			target.CreateTime=item.CreateTime;
			target.StorageNum=item.StorageNum;
			target.FromLocalNum=item.FromLocalNum;
			target.ToLocalNum=item.ToLocalNum;
			return target;
		}
	}
}
