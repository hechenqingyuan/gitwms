/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 23:00:56
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 23:00:56
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Base
{
	[TableAttribute(DbName = "JooWMS", Name = "Measure", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class MeasureEntity:BaseEntity
	{
		public MeasureEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public MeasureEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "SN", DbType = DbType.String,Length=50,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string SN { get;  set; }

		public MeasureEntity IncludeSN (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("SN"))
			{
				this.ColumnList.Add("SN");
			}
			return this;
		}

		[DataMapping(ColumnName = "MeasureNum", DbType = DbType.String,Length=100,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string MeasureNum { get;  set; }

		public MeasureEntity IncludeMeasureNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("MeasureNum"))
			{
				this.ColumnList.Add("MeasureNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "MeasureName", DbType = DbType.String,Length=100,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string MeasureName { get;  set; }

		public MeasureEntity IncludeMeasureName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("MeasureName"))
			{
				this.ColumnList.Add("MeasureName");
			}
			return this;
		}

	}
}
