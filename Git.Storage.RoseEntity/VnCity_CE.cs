/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:45:42
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:45:42
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class VnCity_CE
	{
		public VnCity_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _Code;
		public string Code
		{
			set { this._Code = value; }
			get { return this._Code;}
		}

		private string _CName;
		public string CName
		{
			set { this._CName = value; }
			get { return this._CName;}
		}

		private string _CNameEn;
		public string CNameEn
		{
			set { this._CNameEn = value; }
			get { return this._CNameEn;}
		}

		private string _PCode;
		public string PCode
		{
			set { this._PCode = value; }
			get { return this._PCode;}
		}

	}
}
