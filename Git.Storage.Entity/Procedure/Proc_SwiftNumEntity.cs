/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-09-26 22:18:10
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-09-26 22:18:10
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Procedure
{
    /// <summary>
    /// 该存储过程主要用于管理流水号问题
    /// </summary>
	[TableAttribute(DbName = "JooWMS", Name = "Proc_SwiftNum",  IsInternal = false,MapType=MapType.Proc)]
	public partial class Proc_SwiftNumEntity:BaseEntity
	{
		public Proc_SwiftNumEntity()
		{
		}

		[DataMapping(ColumnName = "Day", DbType = DbType.String, Length = 20,ColumnType=ColumnType.InPut)]
		public string Day { get;  set; }

		[DataMapping(ColumnName = "TabName", DbType = DbType.String, Length = 20,ColumnType=ColumnType.InPut)]
		public string TabName { get;  set; }

		[DataMapping(ColumnName = "Num", DbType = DbType.Int32, Length = 4000,ColumnType=ColumnType.InOutPut)]
		public Int32 Num { get;  set; }

	}
}
