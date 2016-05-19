/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 11:57:57
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 11:57:57
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Store
{
	[TableAttribute(DbName = "JooWMS", Name = "Customer", PrimaryKeyName = "ID", IsInternal = false)]
	public partial class CustomerEntity:BaseEntity
	{
		public CustomerEntity()
		{
		}

		[DataMapping(ColumnName = "ID", DbType = DbType.Int32,Length=4,CanNull=false,DefaultValue=null,PrimaryKey=true,AutoIncrement=true,IsMap=true)]
		public Int32 ID { get;  set; }

		public CustomerEntity IncludeID (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("ID"))
			{
				this.ColumnList.Add("ID");
			}
			return this;
		}

		[DataMapping(ColumnName = "CusNum", DbType = DbType.String,Length=20,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string CusNum { get;  set; }

		public CustomerEntity IncludeCusNum (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CusNum"))
			{
				this.ColumnList.Add("CusNum");
			}
			return this;
		}

		[DataMapping(ColumnName = "CusName", DbType = DbType.String,Length=40,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string CusName { get;  set; }

		public CustomerEntity IncludeCusName (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CusName"))
			{
				this.ColumnList.Add("CusName");
			}
			return this;
		}

		[DataMapping(ColumnName = "Phone", DbType = DbType.String,Length=20,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Phone { get;  set; }

		public CustomerEntity IncludePhone (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Phone"))
			{
				this.ColumnList.Add("Phone");
			}
			return this;
		}

		[DataMapping(ColumnName = "Email", DbType = DbType.String,Length=30,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Email { get;  set; }

		public CustomerEntity IncludeEmail (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Email"))
			{
				this.ColumnList.Add("Email");
			}
			return this;
		}

		[DataMapping(ColumnName = "Fax", DbType = DbType.String,Length=30,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Fax { get;  set; }

		public CustomerEntity IncludeFax (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Fax"))
			{
				this.ColumnList.Add("Fax");
			}
			return this;
		}

		[DataMapping(ColumnName = "Address", DbType = DbType.String,Length=400,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Address { get;  set; }

		public CustomerEntity IncludeAddress (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Address"))
			{
				this.ColumnList.Add("Address");
			}
			return this;
		}

		[DataMapping(ColumnName = "CusType", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 CusType { get;  set; }

		public CustomerEntity IncludeCusType (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CusType"))
			{
				this.ColumnList.Add("CusType");
			}
			return this;
		}

		[DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32,Length=4,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public Int32 IsDelete { get;  set; }

		public CustomerEntity IncludeIsDelete (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("IsDelete"))
			{
				this.ColumnList.Add("IsDelete");
			}
			return this;
		}

		[DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime,Length=8,CanNull=false,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public DateTime CreateTime { get;  set; }

		public CustomerEntity IncludeCreateTime (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CreateTime"))
			{
				this.ColumnList.Add("CreateTime");
			}
			return this;
		}

		[DataMapping(ColumnName = "CreateUser", DbType = DbType.String,Length=20,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string CreateUser { get;  set; }

		public CustomerEntity IncludeCreateUser (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("CreateUser"))
			{
				this.ColumnList.Add("CreateUser");
			}
			return this;
		}

		[DataMapping(ColumnName = "Remark", DbType = DbType.String,Length=400,CanNull=true,DefaultValue=null,PrimaryKey=false,AutoIncrement=false,IsMap=true)]
		public string Remark { get;  set; }

		public CustomerEntity IncludeRemark (bool flag) 
		{
			if (flag && !this.ColumnList.Contains("Remark"))
			{
				this.ColumnList.Add("Remark");
			}
			return this;
		}

	}
    public partial class CustomerEntity
    {
        /// <summary>
        /// 联系人
        /// </summary>
        [DataMapping(ColumnName = "Contact", DbType = DbType.String, Length = 200, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = false)]
        public string Contact { get; set; }

        /// <summary>
        /// 当前客户类型
        /// </summary>
        public string NowCusType { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public double Num { get; set; }
    }


}
