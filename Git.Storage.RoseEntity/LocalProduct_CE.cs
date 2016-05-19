/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:25
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:25
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class LocalProduct_CE
	{
		public LocalProduct_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _Sn;
		public string Sn
		{
			set { this._Sn = value; }
			get { return this._Sn;}
		}

		private string _StorageNum;
		public string StorageNum
		{
			set { this._StorageNum = value; }
			get { return this._StorageNum;}
		}

		private string _StorageName;
		public string StorageName
		{
			set { this._StorageName = value; }
			get { return this._StorageName;}
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

		private Int32 _LocalType;
		public Int32 LocalType
		{
			set { this._LocalType = value; }
			get { return this._LocalType;}
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

		private string _CreateName;
		public string CreateName
		{
			set { this._CreateName = value; }
			get { return this._CreateName;}
		}

		private string _Remark;
		public string Remark
		{
			set { this._Remark = value; }
			get { return this._Remark;}
		}

	}
}
