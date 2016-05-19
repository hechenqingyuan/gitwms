/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:43:39
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:43:39
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class SysRelation_CE
	{
		public SysRelation_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _RoleNum;
		public string RoleNum
		{
			set { this._RoleNum = value; }
			get { return this._RoleNum;}
		}

		private string _ResNum;
		public string ResNum
		{
			set { this._ResNum = value; }
			get { return this._ResNum;}
		}

		private Int16 _ResType;
		public Int16 ResType
		{
			set { this._ResType = value; }
			get { return this._ResType;}
		}

	}
}
