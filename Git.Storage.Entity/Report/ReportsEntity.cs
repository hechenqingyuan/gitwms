/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2015/09/05 13:07:30
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2015/09/05 13:07:30
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Report
{
	[TableAttribute(DbName = "JooWMS", Name = "Reports", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class ReportsEntity:BaseEntity
	{
		public ReportsEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public ReportsEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "ReportNum", DbType = DbType.String,Length=50,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string ReportNum { get;  set; }

		public ReportsEntity IncludeReportNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ReportNum"))
			{
				this.ColumnList.Add("ReportNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "ReportName", DbType = DbType.String,Length=100,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string ReportName { get;  set; }

		public ReportsEntity IncludeReportName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ReportName"))
			{
				this.ColumnList.Add("ReportName");
			}
			return this;
		}

		[DataMapping(ColumnName = "ReportType", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 ReportType { get;  set; }

		public ReportsEntity IncludeReportType (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ReportType"))
			{
				this.ColumnList.Add("ReportType");
			}
			return this;
		}

		[DataMapping(ColumnName = "Remark", DbType = DbType.String,Length=400,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Remark { get;  set; }

		public ReportsEntity IncludeRemark (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Remark"))
			{
				this.ColumnList.Add("Remark");
			}
			return this;
		}

		[DataMapping(ColumnName = "DataSource", DbType = DbType.String,Length=8000,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string DataSource { get;  set; }

		public ReportsEntity IncludeDataSource (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("DataSource"))
			{
				this.ColumnList.Add("DataSource");
			}
			return this;
		}

		[DataMapping(ColumnName = "DsType", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 DsType { get;  set; }

		public ReportsEntity IncludeDsType (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("DsType"))
			{
				this.ColumnList.Add("DsType");
			}
			return this;
		}

		[DataMapping(ColumnName = "FileName", DbType = DbType.String,Length=400,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string FileName { get;  set; }

		public ReportsEntity IncludeFileName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("FileName"))
			{
				this.ColumnList.Add("FileName");
			}
			return this;
		}

		[DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 IsDelete { get;  set; }

		public ReportsEntity IncludeIsDelete (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("IsDelete"))
			{
				this.ColumnList.Add("IsDelete");
			}
			return this;
		}

		[DataMapping(ColumnName = "Status", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 Status { get;  set; }

		public ReportsEntity IncludeStatus (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Status"))
			{
				this.ColumnList.Add("Status");
			}
			return this;
		}

		[DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime,Length=8,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public DateTime CreateTime { get;  set; }

		public ReportsEntity IncludeCreateTime (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CreateTime"))
			{
				this.ColumnList.Add("CreateTime");
			}
			return this;
		}

	}
}
