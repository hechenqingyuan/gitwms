/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:33
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:33
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Check;

namespace Git.Storage.Conversion
{
	public partial class CloneHistory_To
	{
		public CloneHistory_To()
		{
		}

		public static CloneHistoryEntity To(CloneHistory_CE item)
		{
			CloneHistoryEntity target = new CloneHistoryEntity();
			target.CloneID=item.CloneID;
			target.OrderNum=item.OrderNum;
			target.ID=item.ID;
			target.Sn=item.Sn;
			target.StorageNum=item.StorageNum;
			target.StorageName=item.StorageName;
			target.LocalNum=item.LocalNum;
			target.LocalName=item.LocalName;
			target.LocalType=item.LocalType;
			target.ProductNum=item.ProductNum;
			target.BarCode=item.BarCode;
			target.ProductName=item.ProductName;
			target.Num=item.Num;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.CreateName=item.CreateName;
			target.Remark=item.Remark;
			return target;
		}

		public static CloneHistory_CE ToCE(CloneHistoryEntity item)
		{
			CloneHistory_CE target = new CloneHistory_CE();
			target.CloneID=item.CloneID;
			target.OrderNum=item.OrderNum;
			target.ID=item.ID;
			target.Sn=item.Sn;
			target.StorageNum=item.StorageNum;
			target.StorageName=item.StorageName;
			target.LocalNum=item.LocalNum;
			target.LocalName=item.LocalName;
			target.LocalType=item.LocalType;
			target.ProductNum=item.ProductNum;
			target.BarCode=item.BarCode;
			target.ProductName=item.ProductName;
			target.Num=item.Num;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.CreateName=item.CreateName;
			target.Remark=item.Remark;
			return target;
		}
	}
}
