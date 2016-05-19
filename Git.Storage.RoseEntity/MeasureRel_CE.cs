/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 23:01:01
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 23:01:01
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class MeasureRel_CE
	{
		public MeasureRel_CE()
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

		private string _MeasureSource;
		public string MeasureSource
		{
			set { this._MeasureSource = value; }
			get { return this._MeasureSource;}
		}

		private string _MeasureTarget;
		public string MeasureTarget
		{
			set { this._MeasureTarget = value; }
			get { return this._MeasureTarget;}
		}

		private double _Rate;
		public double Rate
		{
			set { this._Rate = value; }
			get { return this._Rate;}
		}

	}
}
