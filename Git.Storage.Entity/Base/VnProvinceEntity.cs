/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 11:54:42
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 11:54:42
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Base
{
	[TableAttribute(DbName = "JooWMS", Name = "VnProvince", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class VnProvinceEntity:BaseEntity
	{
		public VnProvinceEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public VnProvinceEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "Code", DbType = DbType.String,Length=10,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Code { get;  set; }

		public VnProvinceEntity IncludeCode (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Code"))
			{
				this.ColumnList.Add("Code");
			}
			return this;
		}

		[DataMapping(ColumnName = "PName", DbType = DbType.String,Length=30,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string PName { get;  set; }

		public VnProvinceEntity IncludePName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("PName"))
			{
				this.ColumnList.Add("PName");
			}
			return this;
		}

		[DataMapping(ColumnName = "PNameEn", DbType = DbType.String,Length=30,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string PNameEn { get;  set; }

		public VnProvinceEntity IncludePNameEn (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("PNameEn"))
			{
				this.ColumnList.Add("PNameEn");
			}
			return this;
		}

	}
}
