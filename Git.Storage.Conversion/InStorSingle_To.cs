/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:46:36
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:46:36
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.InStorage;

namespace Git.Storage.Conversion
{
	public partial class InStorSingle_To
	{
		public InStorSingle_To()
		{
		}

		public static InStorSingleEntity To(InStorSingle_CE item)
		{
			InStorSingleEntity target = new InStorSingleEntity();
			target.ID=item.ID;
			target.DetailNum=item.DetailNum;
			target.InStorNum=item.InStorNum;
			target.SingleNum=item.SingleNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.InPrice=item.InPrice;
			target.BatchNum=item.BatchNum;
			target.CreateTime=item.CreateTime;
			target.LocalNum=item.LocalNum;
			target.StorageNum=item.StorageNum;
			return target;
		}

		public static InStorSingle_CE ToCE(InStorSingleEntity item)
		{
			InStorSingle_CE target = new InStorSingle_CE();
			target.ID=item.ID;
			target.DetailNum=item.DetailNum;
			target.InStorNum=item.InStorNum;
			target.SingleNum=item.SingleNum;
			target.ProductName=item.ProductName;
			target.BarCode=item.BarCode;
			target.ProductNum=item.ProductNum;
			target.InPrice=item.InPrice;
			target.BatchNum=item.BatchNum;
			target.CreateTime=item.CreateTime;
			target.LocalNum=item.LocalNum;
			target.StorageNum=item.StorageNum;
			return target;
		}
	}
}
