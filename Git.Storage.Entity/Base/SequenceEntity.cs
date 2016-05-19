/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2015/10/08 11:42:17
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2015/10/08 11:42:17
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Base
{
    [TableAttribute(DbName = "JooWMS", Name = "Sequence", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class SequenceEntity:BaseEntity
	{
		public SequenceEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public SequenceEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "SN", DbType = DbType.String,Length=50,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string SN { get;  set; }

		public SequenceEntity IncludeSN (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("SN"))
			{
				this.ColumnList.Add("SN");
			}
			return this;
		}

		[DataMapping(ColumnName = "TabName", DbType = DbType.String,Length=50,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string TabName { get;  set; }

		public SequenceEntity IncludeTabName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("TabName"))
			{
				this.ColumnList.Add("TabName");
			}
			return this;
		}

		[DataMapping(ColumnName = "FirstType", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 FirstType { get;  set; }

		public SequenceEntity IncludeFirstType (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("FirstType"))
			{
				this.ColumnList.Add("FirstType");
			}
			return this;
		}

		[DataMapping(ColumnName = "FirstRule", DbType = DbType.String,Length=200,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string FirstRule { get;  set; }

		public SequenceEntity IncludeFirstRule (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("FirstRule"))
			{
				this.ColumnList.Add("FirstRule");
			}
			return this;
		}

		[DataMapping(ColumnName = "FirstLength", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 FirstLength { get;  set; }

		public SequenceEntity IncludeFirstLength (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("FirstLength"))
			{
				this.ColumnList.Add("FirstLength");
			}
			return this;
		}

		[DataMapping(ColumnName = "SecondType", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 SecondType { get;  set; }

		public SequenceEntity IncludeSecondType (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("SecondType"))
			{
				this.ColumnList.Add("SecondType");
			}
			return this;
		}

		[DataMapping(ColumnName = "SecondRule", DbType = DbType.String,Length=200,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string SecondRule { get;  set; }

		public SequenceEntity IncludeSecondRule (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("SecondRule"))
			{
				this.ColumnList.Add("SecondRule");
			}
			return this;
		}

		[DataMapping(ColumnName = "SecondLength", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 SecondLength { get;  set; }

		public SequenceEntity IncludeSecondLength (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("SecondLength"))
			{
				this.ColumnList.Add("SecondLength");
			}
			return this;
		}

		[DataMapping(ColumnName = "ThirdType", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 ThirdType { get;  set; }

		public SequenceEntity IncludeThirdType (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ThirdType"))
			{
				this.ColumnList.Add("ThirdType");
			}
			return this;
		}

		[DataMapping(ColumnName = "ThirdRule", DbType = DbType.String,Length=200,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string ThirdRule { get;  set; }

		public SequenceEntity IncludeThirdRule (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ThirdRule"))
			{
				this.ColumnList.Add("ThirdRule");
			}
			return this;
		}

		[DataMapping(ColumnName = "ThirdLength", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 ThirdLength { get;  set; }

		public SequenceEntity IncludeThirdLength (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ThirdLength"))
			{
				this.ColumnList.Add("ThirdLength");
			}
			return this;
		}

		[DataMapping(ColumnName = "FourType", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 FourType { get;  set; }

		public SequenceEntity IncludeFourType (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("FourType"))
			{
				this.ColumnList.Add("FourType");
			}
			return this;
		}

		[DataMapping(ColumnName = "FourRule", DbType = DbType.String,Length=200,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string FourRule { get;  set; }

		public SequenceEntity IncludeFourRule (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("FourRule"))
			{
				this.ColumnList.Add("FourRule");
			}
			return this;
		}

		[DataMapping(ColumnName = "FourLength", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 FourLength { get;  set; }

		public SequenceEntity IncludeFourLength (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("FourLength"))
			{
				this.ColumnList.Add("FourLength");
			}
			return this;
		}

		[DataMapping(ColumnName = "JoinChar", DbType = DbType.String,Length=10,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string JoinChar { get;  set; }

		public SequenceEntity IncludeJoinChar (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("JoinChar"))
			{
				this.ColumnList.Add("JoinChar");
			}
			return this;
		}

		[DataMapping(ColumnName = "Sample", DbType = DbType.String,Length=200,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Sample { get;  set; }

		public SequenceEntity IncludeSample (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Sample"))
			{
				this.ColumnList.Add("Sample");
			}
			return this;
		}

		[DataMapping(ColumnName = "CurrentValue", DbType = DbType.String,Length=200,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string CurrentValue { get;  set; }

		public SequenceEntity IncludeCurrentValue (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CurrentValue"))
			{
				this.ColumnList.Add("CurrentValue");
			}
			return this;
		}

		[DataMapping(ColumnName = "Remark", DbType = DbType.String,Length=400,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Remark { get;  set; }

		public SequenceEntity IncludeRemark (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Remark"))
			{
				this.ColumnList.Add("Remark");
			}
			return this;
		}

	}
}
