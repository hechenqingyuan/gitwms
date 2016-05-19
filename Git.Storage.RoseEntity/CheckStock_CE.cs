/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:47:01
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:47:01
*********************************************************************************/

using System;
using System.Text;
using System.Data;

namespace Git.Storage.RoseEntity
{
	[Serializable]
	public partial class CheckStock_CE
	{
		public CheckStock_CE()
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

		private Int32 _Type;
		public Int32 Type
		{
			set { this._Type = value; }
			get { return this._Type;}
		}

		private Int32 _ProductType;
		public Int32 ProductType
		{
			set { this._ProductType = value; }
			get { return this._ProductType;}
		}

		private string _ContractOrder;
		public string ContractOrder
		{
			set { this._ContractOrder = value; }
			get { return this._ContractOrder;}
		}

		private Int32 _Status;
		public Int32 Status
		{
			set { this._Status = value; }
			get { return this._Status;}
		}

        private double _LocalQty;
        public double LocalQty
		{
			set { this._LocalQty = value; }
			get { return this._LocalQty;}
		}

        private double _CheckQty;
        public double CheckQty
		{
			set { this._CheckQty = value; }
			get { return this._CheckQty;}
		}

		private Int32 _IsDelete;
		public Int32 IsDelete
		{
			set { this._IsDelete = value; }
			get { return this._IsDelete;}
		}

		private Int32 _IsComplete;
		public Int32 IsComplete
		{
			set { this._IsComplete = value; }
			get { return this._IsComplete;}
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

		private string _AuditUser;
		public string AuditUser
		{
			set { this._AuditUser = value; }
			get { return this._AuditUser;}
		}

		private DateTime _AuditeTime;
		public DateTime AuditeTime
		{
			set { this._AuditeTime = value; }
			get { return this._AuditeTime;}
		}

		private string _PrintUser;
		public string PrintUser
		{
			set { this._PrintUser = value; }
			get { return this._PrintUser;}
		}

		private DateTime _PrintTime;
		public DateTime PrintTime
		{
			set { this._PrintTime = value; }
			get { return this._PrintTime;}
		}

		private string _Reason;
		public string Reason
		{
			set { this._Reason = value; }
			get { return this._Reason;}
		}

		private Int32 _OperateType;
		public Int32 OperateType
		{
			set { this._OperateType = value; }
			get { return this._OperateType;}
		}

		private string _EquipmentNum;
		public string EquipmentNum
		{
			set { this._EquipmentNum = value; }
			get { return this._EquipmentNum;}
		}

		private string _EquipmentCode;
		public string EquipmentCode
		{
			set { this._EquipmentCode = value; }
			get { return this._EquipmentCode;}
		}

		private string _Remark;
		public string Remark
		{
			set { this._Remark = value; }
			get { return this._Remark;}
		}

	}
}
