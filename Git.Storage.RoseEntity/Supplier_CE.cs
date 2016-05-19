/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:49
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:49
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class Supplier_CE
	{
		public Supplier_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _SupNum;
		public string SupNum
		{
			set { this._SupNum = value; }
			get { return this._SupNum;}
		}

		private string _SupName;
		public string SupName
		{
			set { this._SupName = value; }
			get { return this._SupName;}
		}

		private string _Phone;
		public string Phone
		{
			set { this._Phone = value; }
			get { return this._Phone;}
		}

		private string _Fax;
		public string Fax
		{
			set { this._Fax = value; }
			get { return this._Fax;}
		}

		private string _Email;
		public string Email
		{
			set { this._Email = value; }
			get { return this._Email;}
		}

		private string _ContactName;
		public string ContactName
		{
			set { this._ContactName = value; }
			get { return this._ContactName;}
		}

		private string _Address;
		public string Address
		{
			set { this._Address = value; }
			get { return this._Address;}
		}

		private string _CreateUser;
		public string CreateUser
		{
			set { this._CreateUser = value; }
			get { return this._CreateUser;}
		}

		private string _Description;
		public string Description
		{
			set { this._Description = value; }
			get { return this._Description;}
		}

		private Int32 _IsDelete;
		public Int32 IsDelete
		{
			set { this._IsDelete = value; }
			get { return this._IsDelete;}
		}

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

	}
}
