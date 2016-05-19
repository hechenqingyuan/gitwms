/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 11:54:43
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 11:54:43
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Base
{
	[TableAttribute(DbName = "JooWMS", Name = "VnArea", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class VnAreaEntity:BaseEntity
	{
		public VnAreaEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public VnAreaEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "Code", DbType = DbType.String,Length=10,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Code { get;  set; }

		public VnAreaEntity IncludeCode (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Code"))
			{
				this.ColumnList.Add("Code");
			}
			return this;
		}

		[DataMapping(ColumnName = "AName", DbType = DbType.String,Length=30,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string AName { get;  set; }

		public VnAreaEntity IncludeAName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("AName"))
			{
				this.ColumnList.Add("AName");
			}
			return this;
		}

		[DataMapping(ColumnName = "ANameEn", DbType = DbType.String,Length=30,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string ANameEn { get;  set; }

		public VnAreaEntity IncludeANameEn (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ANameEn"))
			{
				this.ColumnList.Add("ANameEn");
			}
			return this;
		}

		[DataMapping(ColumnName = "CCode", DbType = DbType.String,Length=10,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string CCode { get;  set; }

		public VnAreaEntity IncludeCCode (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CCode"))
			{
				this.ColumnList.Add("CCode");
			}
			return this;
		}

	}
}
