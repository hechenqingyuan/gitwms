/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/04/15 10:47:44
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/04/15 10:47:44
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Check
{
	[TableAttribute(DbName = "JooWMS", Name = "Proc_CreateCheck",  IsInternal = false,MapType=MapType.Proc)]
	public partial class Proc_CreateCheckEntity:BaseEntity
	{
		public Proc_CreateCheckEntity()
		{
		}

		[DataMapping(ColumnName = "OrderNum", DbType = DbType.String, Length = 20,ColumnType=ColumnType.InPut)]
		public string OrderNum { get;  set; }

		[DataMapping(ColumnName = "CreateUser", DbType = DbType.String, Length = 50,ColumnType=ColumnType.InPut)]
		public string CreateUser { get;  set; }

		[DataMapping(ColumnName = "CreateName", DbType = DbType.String, Length = 50,ColumnType=ColumnType.InPut)]
		public string CreateName { get;  set; }

		[DataMapping(ColumnName = "ReturnValue", DbType = DbType.String, Length = 50,ColumnType=ColumnType.InOutPut)]
		public string ReturnValue { get;  set; }

	}
}
