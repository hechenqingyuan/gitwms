/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 11:54:38
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 11:54:38
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Base
{
	[TableAttribute(DbName = "JooWMS", Name = "SysDepart", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class SysDepartEntity:BaseEntity
	{
		public SysDepartEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public SysDepartEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "DepartNum", DbType = DbType.String,Length=20,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string DepartNum { get;  set; }

		public SysDepartEntity IncludeDepartNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("DepartNum"))
			{
				this.ColumnList.Add("DepartNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "DepartName", DbType = DbType.String,Length=40,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string DepartName { get;  set; }

		public SysDepartEntity IncludeDepartName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("DepartName"))
			{
				this.ColumnList.Add("DepartName");
			}
			return this;
		}

		[DataMapping(ColumnName = "ChildCount", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 ChildCount { get;  set; }

		public SysDepartEntity IncludeChildCount (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ChildCount"))
			{
				this.ColumnList.Add("ChildCount");
			}
			return this;
		}

		[DataMapping(ColumnName = "ParentNum", DbType = DbType.String,Length=20,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string ParentNum { get;  set; }

		public SysDepartEntity IncludeParentNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ParentNum"))
			{
				this.ColumnList.Add("ParentNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "Depth", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 Depth { get;  set; }

		public SysDepartEntity IncludeDepth (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Depth"))
			{
				this.ColumnList.Add("Depth");
			}
			return this;
		}

		[DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 IsDelete { get;  set; }

		public SysDepartEntity IncludeIsDelete (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("IsDelete"))
			{
				this.ColumnList.Add("IsDelete");
			}
			return this;
		}

		[DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime,Length=8,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public DateTime CreateTime { get;  set; }

		public SysDepartEntity IncludeCreateTime (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CreateTime"))
			{
				this.ColumnList.Add("CreateTime");
			}
			return this;
		}

	}

    public partial class SysDepartEntity
    {
        /// <summary>
        /// 上级部门名称
        /// </summary>
        public string ParentName { get; set; }
    }
}
