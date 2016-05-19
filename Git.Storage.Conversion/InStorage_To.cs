/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:46:26
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:46:26
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.InStorage;

namespace Git.Storage.Conversion
{
	public partial class InStorage_To
	{
		public InStorage_To()
		{
		}

		public static InStorageEntity To(InStorage_CE item)
		{
			InStorageEntity target = new InStorageEntity();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.InType=item.InType;
			target.ProductType=item.ProductType;
			target.SupNum=item.SupNum;
			target.SupName=item.SupName;
			target.ContactName=item.ContactName;
			target.Phone=item.Phone;
			target.Address=item.Address;
			target.ContractOrder=item.ContractOrder;
			target.ContractType=item.ContractType;
			target.Status=item.Status;
			target.IsDelete=item.IsDelete;
			target.Num=item.Num;
			target.Amount=item.Amount;
			target.NetWeight=item.NetWeight;
			target.GrossWeight=item.GrossWeight;
			target.OrderTime=item.OrderTime;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.AuditUser=item.AuditUser;
			target.AuditeTime=item.AuditeTime;
			target.PrintUser=item.PrintUser;
			target.PrintTime=item.PrintTime;
			target.StoreKeeper=item.StoreKeeper;
			target.Reason=item.Reason;
			target.OperateType=item.OperateType;
			target.EquipmentNum=item.EquipmentNum;
			target.EquipmentCode=item.EquipmentCode;
			target.Remark=item.Remark;
			return target;
		}

		public static InStorage_CE ToCE(InStorageEntity item)
		{
			InStorage_CE target = new InStorage_CE();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.InType=item.InType;
			target.ProductType=item.ProductType;
			target.SupNum=item.SupNum;
			target.SupName=item.SupName;
			target.ContactName=item.ContactName;
			target.Phone=item.Phone;
			target.Address=item.Address;
			target.ContractOrder=item.ContractOrder;
			target.ContractType=item.ContractType;
			target.Status=item.Status;
			target.IsDelete=item.IsDelete;
			target.Num=item.Num;
			target.Amount=item.Amount;
			target.NetWeight=item.NetWeight;
			target.GrossWeight=item.GrossWeight;
			target.OrderTime=item.OrderTime;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.AuditUser=item.AuditUser;
			target.AuditeTime=item.AuditeTime;
			target.PrintUser=item.PrintUser;
			target.PrintTime=item.PrintTime;
			target.StoreKeeper=item.StoreKeeper;
			target.Reason=item.Reason;
			target.OperateType=item.OperateType;
			target.EquipmentNum=item.EquipmentNum;
			target.EquipmentCode=item.EquipmentCode;
			target.Remark=item.Remark;
			return target;
		}
	}
}
