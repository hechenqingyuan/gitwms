/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:38
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:38
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class Equipment_CE
	{
		public Equipment_CE()
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

		private string _EquipmentName;
		public string EquipmentName
		{
			set { this._EquipmentName = value; }
			get { return this._EquipmentName;}
		}

		private string _EquipmentNum;
		public string EquipmentNum
		{
			set { this._EquipmentNum = value; }
			get { return this._EquipmentNum;}
		}

		private Int32 _IsImpower;
		public Int32 IsImpower
		{
			set { this._IsImpower = value; }
			get { return this._IsImpower;}
		}

		private string _Flag;
		public string Flag
		{
			set { this._Flag = value; }
			get { return this._Flag;}
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

		private string _CreateUser;
		public string CreateUser
		{
			set { this._CreateUser = value; }
			get { return this._CreateUser;}
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
