/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:54
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:54
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class SysDepart_CE
	{
		public SysDepart_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _DepartNum;
		public string DepartNum
		{
			set { this._DepartNum = value; }
			get { return this._DepartNum;}
		}

		private string _DepartName;
		public string DepartName
		{
			set { this._DepartName = value; }
			get { return this._DepartName;}
		}

		private Int32 _ChildCount;
		public Int32 ChildCount
		{
			set { this._ChildCount = value; }
			get { return this._ChildCount;}
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
