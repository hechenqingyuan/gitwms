/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:49
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:49
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Bad;

namespace Git.Storage.Conversion
{
	public partial class BadReport_To
	{
		public BadReport_To()
		{
		}

		public static BadReportEntity To(BadReport_CE item)
		{
			BadReportEntity target = new BadReportEntity();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.BadType=item.BadType;
			target.ProductType=item.ProductType;
			target.ContractOrder=item.ContractOrder;
			target.Status=item.Status;
			target.Num=item.Num;
			target.Amount=item.Amount;
			target.Weight=item.Weight;
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

		public static BadReport_CE ToCE(BadReportEntity item)
		{
			BadReport_CE target = new BadReport_CE();
			target.ID=item.ID;
			target.OrderNum=item.OrderNum;
			target.BadType=item.BadType;
			target.ProductType=item.ProductType;
			target.ContractOrder=item.ContractOrder;
			target.Status=item.Status;
			target.Num=item.Num;
			target.Amount=item.Amount;
			target.Weight=item.Weight;
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
