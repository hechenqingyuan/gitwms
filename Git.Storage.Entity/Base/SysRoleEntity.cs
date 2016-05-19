/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 11:54:39
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 11:54:39
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Base
{
	[TableAttribute(DbName = "JooWMS", Name = "SysRole", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class SysRoleEntity:BaseEntity
	{
		public SysRoleEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public SysRoleEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "RoleNum", DbType = DbType.String,Length=20,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string RoleNum { get;  set; }

		public SysRoleEntity IncludeRoleNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("RoleNum"))
			{
				this.ColumnList.Add("RoleNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "RoleName", DbType = DbType.String,Length=40,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string RoleName { get;  set; }

		public SysRoleEntity IncludeRoleName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("RoleName"))
			{
				this.ColumnList.Add("RoleName");
			}
			return this;
		}

		[DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 IsDelete { get;  set; }

		public SysRoleEntity IncludeIsDelete (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("IsDelete"))
			{
				this.ColumnList.Add("IsDelete");
			}
			return this;
		}

		[DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime,Length=8,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public DateTime CreateTime { get;  set; }

		public SysRoleEntity IncludeCreateTime (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CreateTime"))
			{
				this.ColumnList.Add("CreateTime");
			}
			return this;
		}

		[DataMapping(ColumnName = "Remark", DbType = DbType.String,Length=400,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Remark { get;  set; }

		public SysRoleEntity IncludeRemark (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Remark"))
			{
				this.ColumnList.Add("Remark");
			}
			return this;
		}

	}
}
