/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 23:01:06
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 23:01:06
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class TNum_CE
	{
		public TNum_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private Int32 _Num;
		public Int32 Num
		{
			set { this._Num = value; }
			get { return this._Num;}
		}

		private Int32 _MinNum;
		public Int32 MinNum
		{
			set { this._MinNum = value; }
			get { return this._MinNum;}
		}

		private Int32 _MaxNum;
		public Int32 MaxNum
		{
			set { this._MaxNum = value; }
			get { return this._MaxNum;}
		}

		private string _Day;
		public string Day
		{
			set { this._Day = value; }
			get { return this._Day;}
		}

		private string _TabName;
		public string TabName
		{
			set { this._TabName = value; }
			get { return this._TabName;}
		}

	}
}
