/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:25
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:25
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Check;

namespace Git.Storage.Conversion
{
	public partial class CloneTemp_To
	{
		public CloneTemp_To()
		{
		}

		public static CloneTempEntity To(CloneTemp_CE item)
		{
			CloneTempEntity target = new CloneTempEntity();
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

		public static CloneTemp_CE ToCE(CloneTempEntity item)
		{
			CloneTemp_CE target = new CloneTemp_CE();
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
