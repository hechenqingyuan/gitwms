/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:43:50
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:43:50
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class VnProvince_CE
	{
		public VnProvince_CE()
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

		private string _PName;
		public string PName
		{
			set { this._PName = value; }
			get { return this._PName;}
		}

		private string _PNameEn;
		public string PNameEn
		{
			set { this._PNameEn = value; }
			get { return this._PNameEn;}
		}

	}
}
