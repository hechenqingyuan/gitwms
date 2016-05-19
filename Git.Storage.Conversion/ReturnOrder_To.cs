/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:16
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:16
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Return;

namespace Git.Storage.Conversion
{
	public partial class ReturnOrder_To
	{
		public ReturnOrder_To()
		{
		}

		public static ReturnOrderEntity To(ReturnOrder_CE item)
		{
			ReturnOrderEntity target = new ReturnOrderEntity();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.ReturnType=item.ReturnType;
			target.ProductType=item.ProductType;
			target.CusNum=item.CusNum;
			target.CusName=item.CusName;
			target.Contact=item.Contact;
			target.Phone=item.Phone;
			target.Address=item.Address;
			target.ContractOrder=item.ContractOrder;
			target.Num=item.Num;
			target.Amount=item.Amount;
			target.Weight=item.Weight;
			target.Status=item.Status;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.AuditUser=item.AuditUser;
			target.AuditeTime=item.AuditeTime;
			target.PrintUser=item.PrintUser;
			target.PrintTime=item.PrintTime;
			target.Reason=item.Reason;
			target.OperateType=item.OperateType;
			target.EquipmentNum=item.EquipmentNum;
			target.EquipmentCode=item.EquipmentCode;
			target.Remark=item.Remark;
			return target;
		}

		public static ReturnOrder_CE ToCE(ReturnOrderEntity item)
		{
			ReturnOrder_CE target = new ReturnOrder_CE();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.ReturnType=item.ReturnType;
			target.ProductType=item.ProductType;
			target.CusNum=item.CusNum;
			target.CusName=item.CusName;
			target.Contact=item.Contact;
			target.Phone=item.Phone;
			target.Address=item.Address;
			target.ContractOrder=item.ContractOrder;
			target.Num=item.Num;
			target.Amount=item.Amount;
			target.Weight=item.Weight;
			target.Status=item.Status;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.CreateUser=item.CreateUser;
			target.AuditUser=item.AuditUser;
			target.AuditeTime=item.AuditeTime;
			target.PrintUser=item.PrintUser;
			target.PrintTime=item.PrintTime;
			target.Reason=item.Reason;
			target.OperateType=item.OperateType;
			target.EquipmentNum=item.EquipmentNum;
			target.EquipmentCode=item.EquipmentCode;
			target.Remark=item.Remark;
			return target;
		}
	}
}
