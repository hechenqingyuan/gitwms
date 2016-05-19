/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/03/02 22:02:07
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/03/02 22:02:07
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Store
{
	[TableAttribute(DbName = "JooWMS", Name = "InventoryBook", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class InventoryBookEntity:BaseEntity
	{
		public InventoryBookEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public InventoryBookEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "ProductNum", DbType = DbType.String,Length=50,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string ProductNum { get;  set; }

		public InventoryBookEntity IncludeProductNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ProductNum"))
			{
				this.ColumnList.Add("ProductNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "BarCode", DbType = DbType.String,Length=50,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string BarCode { get;  set; }

		public InventoryBookEntity IncludeBarCode (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("BarCode"))
			{
				this.ColumnList.Add("BarCode");
			}
			return this;
		}

		[DataMapping(ColumnName = "ProductName", DbType = DbType.String,Length=100,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string ProductName { get;  set; }

		public InventoryBookEntity IncludeProductName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ProductName"))
			{
				this.ColumnList.Add("ProductName");
			}
			return this;
		}

		[DataMapping(ColumnName = "Num", DbType = DbType.Double,Length=8,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public double Num { get;  set; }

		public InventoryBookEntity IncludeNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Num"))
			{
				this.ColumnList.Add("Num");
			}
			return this;
		}

		[DataMapping(ColumnName = "Type", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 Type { get;  set; }

		public InventoryBookEntity IncludeType (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Type"))
			{
				this.ColumnList.Add("Type");
			}
			return this;
		}

		[DataMapping(ColumnName = "ContactOrder", DbType = DbType.String,Length=50,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string ContactOrder { get;  set; }

		public InventoryBookEntity IncludeContactOrder (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ContactOrder"))
			{
				this.ColumnList.Add("ContactOrder");
			}
			return this;
		}

		[DataMapping(ColumnName = "FromLocalNum", DbType = DbType.String,Length=50,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string FromLocalNum { get;  set; }

		public InventoryBookEntity IncludeFromLocalNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("FromLocalNum"))
			{
				this.ColumnList.Add("FromLocalNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "ToLocalNum", DbType = DbType.String,Length=50,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string ToLocalNum { get;  set; }

		public InventoryBookEntity IncludeToLocalNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ToLocalNum"))
			{
				this.ColumnList.Add("ToLocalNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "StoreNum", DbType = DbType.String,Length=50,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string StoreNum { get;  set; }

		public InventoryBookEntity IncludeStoreNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("StoreNum"))
			{
				this.ColumnList.Add("StoreNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime,Length=8,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public DateTime CreateTime { get;  set; }

		public InventoryBookEntity IncludeCreateTime (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CreateTime"))
			{
				this.ColumnList.Add("CreateTime");
			}
			return this;
		}

		[DataMapping(ColumnName = "CreateUser", DbType = DbType.String,Length=50,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string CreateUser { get;  set; }

		public InventoryBookEntity IncludeCreateUser (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CreateUser"))
			{
				this.ColumnList.Add("CreateUser");
			}
			return this;
		}

	}

    public partial class InventoryBookEntity
    {
        /// <summary>
        /// 相关操作人
        /// </summary>
        [DataMapping(ColumnName = "UserName", DbType = DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 原库位
        /// </summary>
        [DataMapping(ColumnName = "FromLocalName", DbType = DbType.String)]
        public string FromLocalName { get; set; }

        /// <summary>
        /// 目标库位
        /// </summary>
        [DataMapping(ColumnName = "ToLocalName", DbType = DbType.String)]
        public string ToLocalName { get; set; }

	}

}
