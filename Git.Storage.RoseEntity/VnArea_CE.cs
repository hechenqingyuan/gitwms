/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:46:07
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:46:07
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class VnArea_CE
	{
		public VnArea_CE()
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

		private string _AName;
		public string AName
		{
			set { this._AName = value; }
			get { return this._AName;}
		}

		private string _ANameEn;
		public string ANameEn
		{
			set { this._ANameEn = value; }
			get { return this._ANameEn;}
		}

		private string _CCode;
		public string CCode
		{
			set { this._CCode = value; }
			get { return this._CCode;}
		}

	}
}
