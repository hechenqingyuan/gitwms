/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 23:01:06
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 23:01:06
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Base
{
	[TableAttribute(DbName = "JooWMS", Name = "TNum", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class TNumEntity:BaseEntity
	{
		public TNumEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public TNumEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "Num", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 Num { get;  set; }

		public TNumEntity IncludeNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Num"))
			{
				this.ColumnList.Add("Num");
			}
			return this;
		}

		[DataMapping(ColumnName = "MinNum", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 MinNum { get;  set; }

		public TNumEntity IncludeMinNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("MinNum"))
			{
				this.ColumnList.Add("MinNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "MaxNum", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 MaxNum { get;  set; }

		public TNumEntity IncludeMaxNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("MaxNum"))
			{
				this.ColumnList.Add("MaxNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "Day", DbType = DbType.String,Length=20,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Day { get;  set; }

		public TNumEntity IncludeDay (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Day"))
			{
				this.ColumnList.Add("Day");
			}
			return this;
		}

		[DataMapping(ColumnName = "TabName", DbType = DbType.String,Length=50,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string TabName { get;  set; }

		public TNumEntity IncludeTabName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("TabName"))
			{
				this.ColumnList.Add("TabName");
			}
			return this;
		}

	}
}
