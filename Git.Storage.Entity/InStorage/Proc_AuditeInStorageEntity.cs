/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/03/02 09:57:38
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/03/02 09:57:38
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.InStorage
{
	[TableAttribute(DbName = "JooWMS", Name = "Proc_AuditeInStorage",  IsInternal = false,MapType=MapType.Proc)]
	public partial class Proc_AuditeInStorageEntity:BaseEntity
	{
		public Proc_AuditeInStorageEntity()
		{
		}

		[DataMapping(ColumnName = "OrderNum", DbType = DbType.String, Length = 50,ColumnType=ColumnType.InPut)]
		public string OrderNum { get;  set; }

		[DataMapping(ColumnName = "Status", DbType = DbType.Int32, Length = 4000,ColumnType=ColumnType.InPut)]
		public Int32 Status { get;  set; }

        [DataMapping(ColumnName = "AuditUser", DbType = DbType.String, Length = 50, ColumnType = ColumnType.InPut)]
		public string AuditUser { get;  set; }

		[DataMapping(ColumnName = "Reason", DbType = DbType.String, Length = 400,ColumnType=ColumnType.InPut)]
		public string Reason { get;  set; }

		[DataMapping(ColumnName = "OperateType", DbType = DbType.Int32, Length = 4000,ColumnType=ColumnType.InPut)]
		public Int32 OperateType { get;  set; }

        [DataMapping(ColumnName = "EquipmentNum", DbType = DbType.String, Length = 50, ColumnType = ColumnType.InPut)]
		public string EquipmentNum { get;  set; }

        [DataMapping(ColumnName = "EquipmentCode", DbType = DbType.String, Length = 50, ColumnType = ColumnType.InPut)]
		public string EquipmentCode { get;  set; }

		[DataMapping(ColumnName = "Remark", DbType = DbType.String, Length = 400,ColumnType=ColumnType.InPut)]
		public string Remark { get;  set; }

		[DataMapping(ColumnName = "ReturnValue", DbType = DbType.String, Length = 50,ColumnType=ColumnType.InOutPut)]
		public string ReturnValue { get;  set; }

	}
}
