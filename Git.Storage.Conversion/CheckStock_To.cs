/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:01
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:01
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Check;

namespace Git.Storage.Conversion
{
	public partial class CheckStock_To
	{
		public CheckStock_To()
		{
		}

		public static CheckStockEntity To(CheckStock_CE item)
		{
			CheckStockEntity target = new CheckStockEntity();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.Type=item.Type;
			target.ProductType=item.ProductType;
			target.ContractOrder=item.ContractOrder;
			target.Status=item.Status;
			target.LocalQty=item.LocalQty;
			target.CheckQty=item.CheckQty;
			target.IsDelete=item.IsDelete;
			target.IsComplete=item.IsComplete;
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

		public static CheckStock_CE ToCE(CheckStockEntity item)
		{
			CheckStock_CE target = new CheckStock_CE();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.Type=item.Type;
			target.ProductType=item.ProductType;
			target.ContractOrder=item.ContractOrder;
			target.Status=item.Status;
			target.LocalQty=item.LocalQty;
			target.CheckQty=item.CheckQty;
			target.IsDelete=item.IsDelete;
			target.IsComplete=item.IsComplete;
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
