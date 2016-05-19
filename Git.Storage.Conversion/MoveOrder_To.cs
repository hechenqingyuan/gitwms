/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:37
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:37
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Move;

namespace Git.Storage.Conversion
{
	public partial class MoveOrder_To
	{
		public MoveOrder_To()
		{
		}

		public static MoveOrderEntity To(MoveOrder_CE item)
		{
			MoveOrderEntity target = new MoveOrderEntity();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.MoveType=item.MoveType;
			target.ProductType=item.ProductType;
			target.ContractOrder=item.ContractOrder;
			target.Status=item.Status;
			target.IsDelete=item.IsDelete;
			target.Num=item.Num;
			target.Amout=item.Amout;
			target.Weight=item.Weight;
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

		public static MoveOrder_CE ToCE(MoveOrderEntity item)
		{
			MoveOrder_CE target = new MoveOrder_CE();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.MoveType=item.MoveType;
			target.ProductType=item.ProductType;
			target.ContractOrder=item.ContractOrder;
			target.Status=item.Status;
			target.IsDelete=item.IsDelete;
			target.Num=item.Num;
			target.Amout=item.Amout;
			target.Weight=item.Weight;
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
