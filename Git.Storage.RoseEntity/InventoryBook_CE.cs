/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:33
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:33
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class InventoryBook_CE
	{
		public InventoryBook_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _ProductNum;
		public string ProductNum
		{
			set { this._ProductNum = value; }
			get { return this._ProductNum;}
		}

		private string _BarCode;
		public string BarCode
		{
			set { this._BarCode = value; }
			get { return this._BarCode;}
		}

		private string _ProductName;
		public string ProductName
		{
			set { this._ProductName = value; }
			get { return this._ProductName;}
		}

		private double _Num;
        public double Num
		{
			set { this._Num = value; }
			get { return this._Num;}
		}

		private Int32 _Type;
		public Int32 Type
		{
			set { this._Type = value; }
			get { return this._Type;}
		}

		private string _ContactOrder;
		public string ContactOrder
		{
			set { this._ContactOrder = value; }
			get { return this._ContactOrder;}
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

		private string _StoreNum;
		public string StoreNum
		{
			set { this._StoreNum = value; }
			get { return this._StoreNum;}
		}

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

		private string _CreateUser;
		public string CreateUser
		{
			set { this._CreateUser = value; }
			get { return this._CreateUser;}
		}

	}
}
