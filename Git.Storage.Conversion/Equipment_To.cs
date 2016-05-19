/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:38
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:38
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Store;

namespace Git.Storage.Conversion
{
	public partial class Equipment_To
	{
		public Equipment_To()
		{
		}

		public static EquipmentEntity To(Equipment_CE item)
		{
			EquipmentEntity target = new EquipmentEntity();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.EquipmentName=item.EquipmentName;
			target.EquipmentNum=item.EquipmentNum;
			target.IsImpower=item.IsImpower;
			target.Flag=item.Flag;
			target.IsDelete=item.IsDelete;
			target.Status=item.Status;
			target.CreateUser=item.CreateUser;
			target.CreateTime=item.CreateTime;
			target.Remark=item.Remark;
			return target;
		}

		public static Equipment_CE ToCE(EquipmentEntity item)
		{
			Equipment_CE target = new Equipment_CE();
			target.ID=item.ID;
			target.SnNum=item.SnNum;
			target.EquipmentName=item.EquipmentName;
			target.EquipmentNum=item.EquipmentNum;
			target.IsImpower=item.IsImpower;
			target.Flag=item.Flag;
			target.IsDelete=item.IsDelete;
			target.Status=item.Status;
			target.CreateUser=item.CreateUser;
			target.CreateTime=item.CreateTime;
			target.Remark=item.Remark;
			return target;
		}
	}
}
