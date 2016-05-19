/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:21
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:21
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class Storage_CE
	{
		public Storage_CE()
		{
		}

		private Int32 _ID;
		public Int32 ID
		{
			set { this._ID = value; }
			get { return this._ID;}
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

		private Int32 _StorageType;
		public Int32 StorageType
		{
			set { this._StorageType = value; }
			get { return this._StorageType;}
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

		private string _Action;
		public string Action
		{
			set { this._Action = value; }
			get { return this._Action;}
		}

		private Int32 _IsDelete;
		public Int32 IsDelete
		{
			set { this._IsDelete = value; }
			get { return this._IsDelete;}
		}

		private Int32 _Status;
		public Int32 Status
		{
			set { this._Status = value; }
			get { return this._Status;}
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

		private DateTime _CreateTime;
		public DateTime CreateTime
		{
			set { this._CreateTime = value; }
			get { return this._CreateTime;}
		}

		private string _Remark;
		public string Remark
		{
			set { this._Remark = value; }
			get { return this._Remark;}
		}

	}
}
