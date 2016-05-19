/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/03/10 17:31:28
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/03/10 17:31:28
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Check
{
	[TableAttribute(DbName = "JooWMS", Name = "CheckStock", PrimaryKeyName = "ID", IsInternal = false)]
    public partial class CheckStockEntity : BaseEntity
    {
        public CheckStockEntity()
        {
        }

        [DataMapping(ColumnName = "ID", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = true, AutoIncrement = true, IsMap = true)]
        public Int32 ID { get; set; }

        public CheckStockEntity IncludeID(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ID"))
            {
                this.ColumnList.Add("ID");
            }
            return this;
        }

        [DataMapping(ColumnName = "OrderNum", DbType = DbType.String, Length = 20, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string OrderNum { get; set; }

        public CheckStockEntity IncludeOrderNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("OrderNum"))
            {
                this.ColumnList.Add("OrderNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "Type", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 Type { get; set; }

        public CheckStockEntity IncludeType(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Type"))
            {
                this.ColumnList.Add("Type");
            }
            return this;
        }

        [DataMapping(ColumnName = "ProductType", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 ProductType { get; set; }

        public CheckStockEntity IncludeProductType(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ProductType"))
            {
                this.ColumnList.Add("ProductType");
            }
            return this;
        }

        [DataMapping(ColumnName = "StorageNum", DbType = DbType.String, Length = 50, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string StorageNum { get; set; }

        public CheckStockEntity IncludeStorageNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("StorageNum"))
            {
                this.ColumnList.Add("StorageNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "ContractOrder", DbType = DbType.String, Length = 50, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string ContractOrder { get; set; }

        public CheckStockEntity IncludeContractOrder(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ContractOrder"))
            {
                this.ColumnList.Add("ContractOrder");
            }
            return this;
        }

        [DataMapping(ColumnName = "Status", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 Status { get; set; }

        public CheckStockEntity IncludeStatus(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Status"))
            {
                this.ColumnList.Add("Status");
            }
            return this;
        }

        [DataMapping(ColumnName = "LocalQty", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double LocalQty { get; set; }

        public CheckStockEntity IncludeLocalQty(bool flag)
        {
            if (flag && !this.ColumnList.Contains("LocalQty"))
            {
                this.ColumnList.Add("LocalQty");
            }
            return this;
        }

        [DataMapping(ColumnName = "CheckQty", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double CheckQty { get; set; }

        public CheckStockEntity IncludeCheckQty(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CheckQty"))
            {
                this.ColumnList.Add("CheckQty");
            }
            return this;
        }

        [DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 IsDelete { get; set; }

        public CheckStockEntity IncludeIsDelete(bool flag)
        {
            if (flag && !this.ColumnList.Contains("IsDelete"))
            {
                this.ColumnList.Add("IsDelete");
            }
            return this;
        }

        [DataMapping(ColumnName = "IsComplete", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 IsComplete { get; set; }

        public CheckStockEntity IncludeIsComplete(bool flag)
        {
            if (flag && !this.ColumnList.Contains("IsComplete"))
            {
                this.ColumnList.Add("IsComplete");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public DateTime CreateTime { get; set; }

        public CheckStockEntity IncludeCreateTime(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateTime"))
            {
                this.ColumnList.Add("CreateTime");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateUser", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string CreateUser { get; set; }

        public CheckStockEntity IncludeCreateUser(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateUser"))
            {
                this.ColumnList.Add("CreateUser");
            }
            return this;
        }

        [DataMapping(ColumnName = "AuditUser", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string AuditUser { get; set; }

        public CheckStockEntity IncludeAuditUser(bool flag)
        {
            if (flag && !this.ColumnList.Contains("AuditUser"))
            {
                this.ColumnList.Add("AuditUser");
            }
            return this;
        }

        [DataMapping(ColumnName = "AuditeTime", DbType = DbType.DateTime, Length = 8, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public DateTime AuditeTime { get; set; }

        public CheckStockEntity IncludeAuditeTime(bool flag)
        {
            if (flag && !this.ColumnList.Contains("AuditeTime"))
            {
                this.ColumnList.Add("AuditeTime");
            }
            return this;
        }

        [DataMapping(ColumnName = "PrintUser", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string PrintUser { get; set; }

        public CheckStockEntity IncludePrintUser(bool flag)
        {
            if (flag && !this.ColumnList.Contains("PrintUser"))
            {
                this.ColumnList.Add("PrintUser");
            }
            return this;
        }

        [DataMapping(ColumnName = "PrintTime", DbType = DbType.DateTime, Length = 8, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public DateTime PrintTime { get; set; }

        public CheckStockEntity IncludePrintTime(bool flag)
        {
            if (flag && !this.ColumnList.Contains("PrintTime"))
            {
                this.ColumnList.Add("PrintTime");
            }
            return this;
        }

        [DataMapping(ColumnName = "Reason", DbType = DbType.String, Length = 800, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Reason { get; set; }

        public CheckStockEntity IncludeReason(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Reason"))
            {
                this.ColumnList.Add("Reason");
            }
            return this;
        }

        [DataMapping(ColumnName = "OperateType", DbType = DbType.Int32, Length = 4, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 OperateType { get; set; }

        public CheckStockEntity IncludeOperateType(bool flag)
        {
            if (flag && !this.ColumnList.Contains("OperateType"))
            {
                this.ColumnList.Add("OperateType");
            }
            return this;
        }

        [DataMapping(ColumnName = "EquipmentNum", DbType = DbType.String, Length = 20, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string EquipmentNum { get; set; }

        public CheckStockEntity IncludeEquipmentNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("EquipmentNum"))
            {
                this.ColumnList.Add("EquipmentNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "EquipmentCode", DbType = DbType.String, Length = 20, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string EquipmentCode { get; set; }

        public CheckStockEntity IncludeEquipmentCode(bool flag)
        {
            if (flag && !this.ColumnList.Contains("EquipmentCode"))
            {
                this.ColumnList.Add("EquipmentCode");
            }
            return this;
        }

        [DataMapping(ColumnName = "Remark", DbType = DbType.String, Length = 800, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Remark { get; set; }

        public CheckStockEntity IncludeRemark(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Remark"))
            {
                this.ColumnList.Add("Remark");
            }
            return this;
        }

    }

    public partial class CheckStockEntity
    {
        [DataMapping(ColumnName = "CreateUserName", DbType = DbType.String)]
        public string CreateUserName { get; set; }

        public string CheckTypeLable { get; set; }

        public string ProductTypeLable { get; set; }

        public string StatusLable { get; set; }
    }
}
