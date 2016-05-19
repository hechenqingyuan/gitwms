/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:30
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:30
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class Location_CE
	{
		public Location_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
		}

		private string _LocalNum;
		public string LocalNum
		{
			set { this._LocalNum = value; }
			get { return this._LocalNum;}
		}

		private string _LocalBarCode;
		public string LocalBarCode
		{
			set { this._LocalBarCode = value; }
			get { return this._LocalBarCode;}
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

		private Int32 _StorageType;
		public Int32 StorageType
		{
			set { this._StorageType = value; }
			get { return this._StorageType;}
		}

		private Int32 _LocalType;
		public Int32 LocalType
		{
			set { this._LocalType = value; }
			get { return this._LocalType;}
		}

		private string _Rack;
		public string Rack
		{
			set { this._Rack = value; }
			get { return this._Rack;}
		}

		private double _Length;
		public double Length
		{
			set { this._Length = value; }
			get { return this._Length;}
		}

		private double _Width;
		public double Width
		{
			set { this._Width = value; }
			get { return this._Width;}
		}

		private double _Height;
		public double Height
		{
			set { this._Height = value; }
			get { return this._Height;}
		}

		private double _X;
		public double X
		{
			set { this._X = value; }
			get { return this._X;}
		}

		private double _Y;
		public double Y
		{
			set { this._Y = value; }
			get { return this._Y;}
		}

		private double _Z;
		public double Z
		{
			set { this._Z = value; }
			get { return this._Z;}
		}

		private Int32 _Unit;
		public Int32 Unit
		{
			set { this._Unit = value; }
			get { return this._Unit;}
		}

		private string _UnitName;
		public string UnitName
		{
			set { this._UnitName = value; }
			get { return this._UnitName;}
		}

		private string _Remark;
		public string Remark
		{
			set { this._Remark = value; }
			get { return this._Remark;}
		}

		private Int32 _IsForbid;
		public Int32 IsForbid
		{
			set { this._IsForbid = value; }
			get { return this._IsForbid;}
		}

		private Int32 _IsDefault;
		public Int32 IsDefault
		{
			set { this._IsDefault = value; }
			get { return this._IsDefault;}
		}

		private Int32 _IsDelete;
		public Int32 IsDelete
		{
			set { this._IsDelete = value; }
			get { return this._IsDelete;}
		}

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

	}
}
