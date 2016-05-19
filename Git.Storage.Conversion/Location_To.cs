/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:30
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:30
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Store;

namespace Git.Storage.Conversion
{
	public partial class Location_To
	{
		public Location_To()
		{
		}

		public static LocationEntity To(Location_CE item)
		{
			LocationEntity target = new LocationEntity();
			target.ID=item.ID;
			target.LocalNum=item.LocalNum;
			target.LocalBarCode=item.LocalBarCode;
			target.LocalName=item.LocalName;
			target.StorageNum=item.StorageNum;
			target.StorageType=item.StorageType;
			target.LocalType=item.LocalType;
			target.Rack=item.Rack;
			target.Length=item.Length;
			target.Width=item.Width;
			target.Height=item.Height;
			target.X=item.X;
			target.Y=item.Y;
			target.Z=item.Z;
			target.Unit=item.Unit;
			target.UnitName=item.UnitName;
			target.Remark=item.Remark;
			target.IsForbid=item.IsForbid;
			target.IsDefault=item.IsDefault;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			return target;
		}

		public static Location_CE ToCE(LocationEntity item)
		{
			Location_CE target = new Location_CE();
			target.ID=item.ID;
			target.LocalNum=item.LocalNum;
			target.LocalBarCode=item.LocalBarCode;
			target.LocalName=item.LocalName;
			target.StorageNum=item.StorageNum;
			target.StorageType=item.StorageType;
			target.LocalType=item.LocalType;
			target.Rack=item.Rack;
			target.Length=item.Length;
			target.Width=item.Width;
			target.Height=item.Height;
			target.X=item.X;
			target.Y=item.Y;
			target.Z=item.Z;
			target.Unit=item.Unit;
			target.UnitName=item.UnitName;
			target.Remark=item.Remark;
			target.IsForbid=item.IsForbid;
			target.IsDefault=item.IsDefault;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			return target;
		}
	}
}
