/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:43:15
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:43:15
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class CusAddress_CE
	{
		public CusAddress_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _SnNum;
		public string SnNum
		{
			set { this._SnNum = value; }
			get { return this._SnNum;}
		}

		private string _CusNum;
		public string CusNum
		{
			set { this._CusNum = value; }
			get { return this._CusNum;}
		}

		private string _Contact;
		public string Contact
		{
			set { this._Contact = value; }
			get { return this._Contact;}
		}

		private string _Phone;
		public string Phone
		{
			set { this._Phone = value; }
			get { return this._Phone;}
		}

		private string _Address;
		public string Address
		{
			set { this._Address = value; }
			get { return this._Address;}
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

		private string _CreateUser;
		public string CreateUser
		{
			set { this._CreateUser = value; }
			get { return this._CreateUser;}
		}

		private string _Remark;
		public string Remark
		{
			set { this._Remark = value; }
			get { return this._Remark;}
		}

	}
}
