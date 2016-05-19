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
	[TableAttribute(DbName = "JooWMS", Name = "VnCity", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class VnCityEntity:BaseEntity
	{
		public VnCityEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public VnCityEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "Code", DbType = DbType.String,Length=10,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Code { get;  set; }

		public VnCityEntity IncludeCode (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Code"))
			{
				this.ColumnList.Add("Code");
			}
			return this;
		}

		[DataMapping(ColumnName = "CName", DbType = DbType.String,Length=30,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string CName { get;  set; }

		public VnCityEntity IncludeCName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CName"))
			{
				this.ColumnList.Add("CName");
			}
			return this;
		}

		[DataMapping(ColumnName = "CNameEn", DbType = DbType.String,Length=30,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string CNameEn { get;  set; }

		public VnCityEntity IncludeCNameEn (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CNameEn"))
			{
				this.ColumnList.Add("CNameEn");
			}
			return this;
		}

		[DataMapping(ColumnName = "PCode", DbType = DbType.String,Length=10,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string PCode { get;  set; }

		public VnCityEntity IncludePCode (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("PCode"))
			{
				this.ColumnList.Add("PCode");
			}
			return this;
		}

	}
}
