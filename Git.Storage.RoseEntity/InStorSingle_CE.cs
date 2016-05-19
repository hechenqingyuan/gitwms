/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014-09-29 12:25:13
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014-09-29 12:25:13
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class InStorSingle_CE
	{
		public InStorSingle_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _DetailNum;
		public string DetailNum
		{
			set { this._DetailNum = value; }
			get { return this._DetailNum;}
		}

		private string _InStorNum;
		public string InStorNum
		{
			set { this._InStorNum = value; }
			get { return this._InStorNum;}
		}

		private string _SingleNum;
		public string SingleNum
		{
			set { this._SingleNum = value; }
			get { return this._SingleNum;}
		}

		private string _ProductName;
		public string ProductName
		{
			set { this._ProductName = value; }
			get { return this._ProductName;}
		}

		private string _BarCode;
		public string BarCode
		{
			set { this._BarCode = value; }
			get { return this._BarCode;}
		}

		private string _ProductNum;
		public string ProductNum
		{
			set { this._ProductNum = value; }
			get { return this._ProductNum;}
		}

		private double _InPrice;
		public double InPrice
		{
			set { this._InPrice = value; }
			get { return this._InPrice;}
		}

		private string _BatchNum;
		public string BatchNum
		{
			set { this._BatchNum = value; }
			get { return this._BatchNum;}
		}

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

		private string _LocalNum;
		public string LocalNum
		{
			set { this._LocalNum = value; }
			get { return this._LocalNum;}
		}

		private string _StorageNum;
		public string StorageNum
		{
			set { this._StorageNum = value; }
			get { return this._StorageNum;}
		}

	}
}
