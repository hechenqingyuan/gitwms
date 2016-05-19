/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 23:00:56
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 23:00:56
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class Measure_CE
	{
		public Measure_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _SN;
		public string SN
		{
			set { this._SN = value; }
			get { return this._SN;}
		}

		private string _MeasureNum;
		public string MeasureNum
		{
			set { this._MeasureNum = value; }
			get { return this._MeasureNum;}
		}

		private string _MeasureName;
		public string MeasureName
		{
			set { this._MeasureName = value; }
			get { return this._MeasureName;}
		}

	}
}
