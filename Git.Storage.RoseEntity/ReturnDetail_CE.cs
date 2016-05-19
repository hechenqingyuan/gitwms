/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:12
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:12
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class ReturnDetail_CE
	{
		public ReturnDetail_CE()
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

		private string _ContractOrder;
		public string ContractOrder
		{
			set { this._ContractOrder = value; }
			get { return this._ContractOrder;}
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

		private double _Num;
        public double Num
		{
			set { this._Num = value; }
			get { return this._Num;}
		}

        private double _OutPrice;
        public double OutPrice
		{
			set { this._OutPrice = value; }
			get { return this._OutPrice;}
		}

        private double _Amount;
        public double Amount
		{
			set { this._Amount = value; }
			get { return this._Amount;}
		}

        private double _BackNum;
        public double BackNum
		{
			set { this._BackNum = value; }
			get { return this._BackNum;}
		}

        private double _BackAmount;
        public double BackAmount
		{
			set { this._BackAmount = value; }
			get { return this._BackAmount;}
		}

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

	}
}
