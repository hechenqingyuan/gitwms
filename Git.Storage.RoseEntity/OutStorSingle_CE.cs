/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:46:48
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:46:48
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class OutStorSingle_CE
	{
		public OutStorSingle_CE()
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

		private string _OutStorNum;
		public string OutStorNum
		{
			set { this._OutStorNum = value; }
			get { return this._OutStorNum;}
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

        private double _Price;
        public double Price
		{
			set { this._Price = value; }
			get { return this._Price;}
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

		private string _BatchNum;
		public string BatchNum
		{
			set { this._BatchNum = value; }
			get { return this._BatchNum;}
		}

	}
}
