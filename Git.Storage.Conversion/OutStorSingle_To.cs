/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:46:48
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:46:48
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.OutStorage;

namespace Git.Storage.Conversion
{
	public partial class OutStorSingle_To
	{
		public OutStorSingle_To()
		{
		}

		public static OutStorSingleEntity To(OutStorSingle_CE item)
		{
			OutStorSingleEntity target = new OutStorSingleEntity();
			target.ID=item.ID;
			target.DetailNum=item.DetailNum;
			target.OutStorNum=item.OutStorNum;
			target.SingleNum=item.SingleNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.Price=item.Price;
			target.CreateTime=item.CreateTime;
			target.LocalNum=item.LocalNum;
			target.StorageNum=item.StorageNum;
			target.BatchNum=item.BatchNum;
			return target;
		}

		public static OutStorSingle_CE ToCE(OutStorSingleEntity item)
		{
			OutStorSingle_CE target = new OutStorSingle_CE();
			target.ID=item.ID;
			target.DetailNum=item.DetailNum;
			target.OutStorNum=item.OutStorNum;
			target.SingleNum=item.SingleNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.Price=item.Price;
			target.CreateTime=item.CreateTime;
			target.LocalNum=item.LocalNum;
			target.StorageNum=item.StorageNum;
			target.BatchNum=item.BatchNum;
			return target;
		}
	}
}
