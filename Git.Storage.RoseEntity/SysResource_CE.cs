/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:45
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:45
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class SysResource_CE
	{
		public SysResource_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _ResNum;
		public string ResNum
		{
			set { this._ResNum = value; }
			get { return this._ResNum;}
		}

		private string _ResName;
		public string ResName
		{
			set { this._ResName = value; }
			get { return this._ResName;}
		}

		private string _ParentNum;
		public string ParentNum
		{
			set { this._ParentNum = value; }
			get { return this._ParentNum;}
		}

		private Int32 _Depth;
		public Int32 Depth
		{
			set { this._Depth = value; }
			get { return this._Depth;}
		}

		private string _ParentPath;
		public string ParentPath
		{
			set { this._ParentPath = value; }
			get { return this._ParentPath;}
		}

		private Int32 _ChildCount;
		public Int32 ChildCount
		{
			set { this._ChildCount = value; }
			get { return this._ChildCount;}
		}

		private Int32 _Sort;
		public Int32 Sort
		{
			set { this._Sort = value; }
			get { return this._Sort;}
		}

		private Int16 _IsHide;
		public Int16 IsHide
		{
			set { this._IsHide = value; }
			get { return this._IsHide;}
		}

		private Int16 _IsDelete;
		public Int16 IsDelete
		{
			set { this._IsDelete = value; }
			get { return this._IsDelete;}
		}

		private string _Url;
		public string Url
		{
			set { this._Url = value; }
			get { return this._Url;}
		}

		private string _CssName;
		public string CssName
		{
			set { this._CssName = value; }
			get { return this._CssName;}
		}

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

		private Int16 _Depart;
		public Int16 Depart
		{
			set { this._Depart = value; }
			get { return this._Depart;}
		}

		private Int16 _ResType;
		public Int16 ResType
		{
			set { this._ResType = value; }
			get { return this._ResType;}
		}

		private DateTime _UpdateTime;
		public DateTime UpdateTime
		{
			set { this._UpdateTime = value; }
			get { return this._UpdateTime;}
		}

		private string _CreateUser;
		public string CreateUser
		{
			set { this._CreateUser = value; }
			get { return this._CreateUser;}
		}

		private string _UpdateUser;
		public string UpdateUser
		{
			set { this._UpdateUser = value; }
			get { return this._UpdateUser;}
		}

		private string _CreateIp;
		public string CreateIp
		{
			set { this._CreateIp = value; }
			get { return this._CreateIp;}
		}

		private string _UpdateIp;
		public string UpdateIp
		{
			set { this._UpdateIp = value; }
			get { return this._UpdateIp;}
		}

		private string _Remark;
		public string Remark
		{
			set { this._Remark = value; }
			get { return this._Remark;}
		}

	}
}
