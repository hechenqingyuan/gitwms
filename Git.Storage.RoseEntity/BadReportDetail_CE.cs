/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:54
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:54
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class BadReportDetail_CE
	{
		public BadReportDetail_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _SnNum;
		public string SnNum
		{
			set { this._SnNum = value; }
			get { return this._SnNum;}
		}

		private string _OrderNum;
		public string OrderNum
		{
			set { this._OrderNum = value; }
			get { return this._OrderNum;}
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

		private string _BatchNum;
		public string BatchNum
		{
			set { this._BatchNum = value; }
			get { return this._BatchNum;}
		}

		private double _Num;
        public double Num
		{
			set { this._Num = value; }
			get { return this._Num;}
		}

        private double _InPrice;
        public double InPrice
		{
			set { this._InPrice = value; }
			get { return this._InPrice;}
		}

        private double _Amount;
        public double Amount
		{
			set { this._Amount = value; }
			get { return this._Amount;}
		}

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

		private string _StorageNum;
		public string StorageNum
		{
			set { this._StorageNum = value; }
			get { return this._StorageNum;}
		}

		private string _FromLocalNum;
		public string FromLocalNum
		{
			set { this._FromLocalNum = value; }
			get { return this._FromLocalNum;}
		}

		private string _ToLocalNum;
		public string ToLocalNum
		{
			set { this._ToLocalNum = value; }
			get { return this._ToLocalNum;}
		}

	}
}
