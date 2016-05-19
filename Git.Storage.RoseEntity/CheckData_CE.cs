/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:20
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:20
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class CheckData_CE
	{
		public CheckData_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _OrderNum;
		public string OrderNum
		{
			set { this._OrderNum = value; }
			get { return this._OrderNum;}
		}

		private string _LocalNum;
		public string LocalNum
		{
			set { this._LocalNum = value; }
			get { return this._LocalNum;}
		}

		private string _LocalName;
		public string LocalName
		{
			set { this._LocalName = value; }
			get { return this._LocalName;}
		}

		private string _StorageNum;
		public string StorageNum
		{
			set { this._StorageNum = value; }
			get { return this._StorageNum;}
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

		private Int32 _LocalQty;
		public Int32 LocalQty
		{
			set { this._LocalQty = value; }
			get { return this._LocalQty;}
		}

		private Int32 _FirstQty;
		public Int32 FirstQty
		{
			set { this._FirstQty = value; }
			get { return this._FirstQty;}
		}

		private Int32 _SecondQty;
		public Int32 SecondQty
		{
			set { this._SecondQty = value; }
			get { return this._SecondQty;}
		}

		private Int32 _DifQty;
		public Int32 DifQty
		{
			set { this._DifQty = value; }
			get { return this._DifQty;}
		}

		private string _FirstUser;
		public string FirstUser
		{
			set { this._FirstUser = value; }
			get { return this._FirstUser;}
		}

		private string _SecondUser;
		public string SecondUser
		{
			set { this._SecondUser = value; }
			get { return this._SecondUser;}
		}

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

	}
}
