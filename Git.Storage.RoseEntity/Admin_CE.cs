/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:43:19
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:43:19
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class Admin_CE
	{
		public Admin_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _UserName;
		public string UserName
		{
			set { this._UserName = value; }
			get { return this._UserName;}
		}

		private string _PassWord;
		public string PassWord
		{
			set { this._PassWord = value; }
			get { return this._PassWord;}
		}

		private string _UserCode;
		public string UserCode
		{
			set { this._UserCode = value; }
			get { return this._UserCode;}
		}

		private string _RealName;
		public string RealName
		{
			set { this._RealName = value; }
			get { return this._RealName;}
		}

		private string _Email;
		public string Email
		{
			set { this._Email = value; }
			get { return this._Email;}
		}

		private string _Mobile;
		public string Mobile
		{
			set { this._Mobile = value; }
			get { return this._Mobile;}
		}

		private string _Phone;
		public string Phone
		{
			set { this._Phone = value; }
			get { return this._Phone;}
		}

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

		private string _CreateIp;
		public string CreateIp
		{
			set { this._CreateIp = value; }
			get { return this._CreateIp;}
		}

		private string _CreateUser;
		public string CreateUser
		{
			set { this._CreateUser = value; }
			get { return this._CreateUser;}
		}

		private Int32 _LoginCount;
		public Int32 LoginCount
		{
			set { this._LoginCount = value; }
			get { return this._LoginCount;}
		}

		private string _Picture;
		public string Picture
		{
			set { this._Picture = value; }
			get { return this._Picture;}
		}

		private DateTime _UpdateTime;
		public DateTime UpdateTime
		{
			set { this._UpdateTime = value; }
			get { return this._UpdateTime;}
		}

		private Int16 _IsDelete;
		public Int16 IsDelete
		{
			set { this._IsDelete = value; }
			get { return this._IsDelete;}
		}

		private Int16 _Status;
		public Int16 Status
		{
			set { this._Status = value; }
			get { return this._Status;}
		}

		private string _DepartNum;
		public string DepartNum
		{
			set { this._DepartNum = value; }
			get { return this._DepartNum;}
		}

		private string _ParentCode;
		public string ParentCode
		{
			set { this._ParentCode = value; }
			get { return this._ParentCode;}
		}

		private string _RoleNum;
		public string RoleNum
		{
			set { this._RoleNum = value; }
			get { return this._RoleNum;}
		}

		private string _Remark;
		public string Remark
		{
			set { this._Remark = value; }
			get { return this._Remark;}
		}

	}
}
