/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-11-26 17:16:48
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-11-26 17:16:48
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Store
{
	[TableAttribute(DbName = "JooWMS", Name = "Storage", PrimaryKeyName = "ID", IsInternal = false)]
    public partial class StorageEntity : BaseEntity
    {
        public StorageEntity()
        {
        }

        [DataMapping(ColumnName = "ID", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = true, AutoIncrement = true, IsMap = true)]
        public Int32 ID { get; set; }

        public StorageEntity IncludeID(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ID"))
            {
                this.ColumnList.Add("ID");
            }
            return this;
        }

        [DataMapping(ColumnName = "StorageNum", DbType = DbType.String, Length = 20, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string StorageNum { get; set; }

        public StorageEntity IncludeStorageNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("StorageNum"))
            {
                this.ColumnList.Add("StorageNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "StorageName", DbType = DbType.String, Length = 100, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string StorageName { get; set; }

        public StorageEntity IncludeStorageName(bool flag)
        {
            if (flag && !this.ColumnList.Contains("StorageName"))
            {
                this.ColumnList.Add("StorageName");
            }
            return this;
        }

        [DataMapping(ColumnName = "StorageType", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 StorageType { get; set; }

        public StorageEntity IncludeStorageType(bool flag)
        {
            if (flag && !this.ColumnList.Contains("StorageType"))
            {
                this.ColumnList.Add("StorageType");
            }
            return this;
        }

        [DataMapping(ColumnName = "Length", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double Length { get; set; }

        public StorageEntity IncludeLength(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Length"))
            {
                this.ColumnList.Add("Length");
            }
            return this;
        }

        [DataMapping(ColumnName = "Width", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double Width { get; set; }

        public StorageEntity IncludeWidth(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Width"))
            {
                this.ColumnList.Add("Width");
            }
            return this;
        }

        [DataMapping(ColumnName = "Height", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double Height { get; set; }

        public StorageEntity IncludeHeight(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Height"))
            {
                this.ColumnList.Add("Height");
            }
            return this;
        }

        [DataMapping(ColumnName = "Action", DbType = DbType.String, Length = 400, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Action { get; set; }

        public StorageEntity IncludeAction(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Action"))
            {
                this.ColumnList.Add("Action");
            }
            return this;
        }

        [DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 IsDelete { get; set; }

        public StorageEntity IncludeIsDelete(bool flag)
        {
            if (flag && !this.ColumnList.Contains("IsDelete"))
            {
                this.ColumnList.Add("IsDelete");
            }
            return this;
        }

        [DataMapping(ColumnName = "Status", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 Status { get; set; }

        public StorageEntity IncludeStatus(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Status"))
            {
                this.ColumnList.Add("Status");
            }
            return this;
        }

        [DataMapping(ColumnName = "IsForbid", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 IsForbid { get; set; }

        public StorageEntity IncludeIsForbid(bool flag)
        {
            if (flag && !this.ColumnList.Contains("IsForbid"))
            {
                this.ColumnList.Add("IsForbid");
            }
            return this;
        }

        [DataMapping(ColumnName = "IsDefault", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 IsDefault { get; set; }

        public StorageEntity IncludeIsDefault(bool flag)
        {
            if (flag && !this.ColumnList.Contains("IsDefault"))
            {
                this.ColumnList.Add("IsDefault");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public DateTime CreateTime { get; set; }

        public StorageEntity IncludeCreateTime(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateTime"))
            {
                this.ColumnList.Add("CreateTime");
            }
            return this;
        }

        [DataMapping(ColumnName = "Remark", DbType = DbType.String, Length = 400, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Remark { get; set; }

        public StorageEntity IncludeRemark(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Remark"))
            {
                this.ColumnList.Add("Remark");
            }
            return this;
        }

    }

    public partial class StorageEntity
    {
        /// <summary>
        /// 仓库类型
        /// </summary>
        [DataMapping(ColumnName = "StorageTypeName", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = false)]
        public string StorageTypeName { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [DataMapping(ColumnName = "IsForbidStr", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = false)]
        public string IsForbidStr { get; set; }  
        
        /// <summary>
        /// 是否默认
        /// </summary>
        [DataMapping(ColumnName = "IsDefaultStr", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = false)]
        public string IsDefaultStr { get; set; }
    }
}
