/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:21
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:21
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Store;

namespace Git.Storage.Conversion
{
	public partial class Storage_To
	{
		public Storage_To()
		{
		}

		public static StorageEntity To(Storage_CE item)
		{
			StorageEntity target = new StorageEntity();
			target.ID=item.ID;
			target.StorageNum=item.StorageNum;
			target.StorageName=item.StorageName;
			target.StorageType=item.StorageType;
			target.Length=item.Length;
			target.Width=item.Width;
			target.Height=item.Height;
			target.Action=item.Action;
			target.IsDelete=item.IsDelete;
			target.Status=item.Status;
			target.IsForbid=item.IsForbid;
			target.IsDefault=item.IsDefault;
			target.CreateTime=item.CreateTime;
			target.Remark=item.Remark;
			return target;
		}

		public static Storage_CE ToCE(StorageEntity item)
		{
			Storage_CE target = new Storage_CE();
			target.ID=item.ID;
			target.StorageNum=item.StorageNum;
			target.StorageName=item.StorageName;
			target.StorageType=item.StorageType;
			target.Length=item.Length;
			target.Width=item.Width;
			target.Height=item.Height;
			target.Action=item.Action;
			target.IsDelete=item.IsDelete;
			target.Status=item.Status;
			target.IsForbid=item.IsForbid;
			target.IsDefault=item.IsDefault;
			target.CreateTime=item.CreateTime;
			target.Remark=item.Remark;
			return target;
		}
	}
}
