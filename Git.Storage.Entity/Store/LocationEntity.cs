/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-11-26 17:16:49
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-11-26 17:16:49
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Store
{
    [TableAttribute(DbName = "JooWMS", Name = "Location", PrimaryKeyName = "ID", IsInternal = false)]
    public partial class LocationEntity : BaseEntity
    {
        public LocationEntity()
        {
        }

        [DataMapping(ColumnName = "ID", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = true, AutoIncrement = true, IsMap = true)]
        public Int32 ID { get; set; }

        public LocationEntity IncludeID(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ID"))
            {
                this.ColumnList.Add("ID");
            }
            return this;
        }

        [DataMapping(ColumnName = "LocalNum", DbType = DbType.String, Length = 20, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string LocalNum { get; set; }

        public LocationEntity IncludeLocalNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("LocalNum"))
            {
                this.ColumnList.Add("LocalNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "LocalBarCode", DbType = DbType.String, Length = 20, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string LocalBarCode { get; set; }

        public LocationEntity IncludeLocalBarCode(bool flag)
        {
            if (flag && !this.ColumnList.Contains("LocalBarCode"))
            {
                this.ColumnList.Add("LocalBarCode");
            }
            return this;
        }

        [DataMapping(ColumnName = "LocalName", DbType = DbType.String, Length = 60, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string LocalName { get; set; }

        public LocationEntity IncludeLocalName(bool flag)
        {
            if (flag && !this.ColumnList.Contains("LocalName"))
            {
                this.ColumnList.Add("LocalName");
            }
            return this;
        }

        [DataMapping(ColumnName = "StorageNum", DbType = DbType.String, Length = 20, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string StorageNum { get; set; }

        public LocationEntity IncludeStorageNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("StorageNum"))
            {
                this.ColumnList.Add("StorageNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "StorageType", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 StorageType { get; set; }

        public LocationEntity IncludeStorageType(bool flag)
        {
            if (flag && !this.ColumnList.Contains("StorageType"))
            {
                this.ColumnList.Add("StorageType");
            }
            return this;
        }

        [DataMapping(ColumnName = "LocalType", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 LocalType { get; set; }

        public LocationEntity IncludeLocalType(bool flag)
        {
            if (flag && !this.ColumnList.Contains("LocalType"))
            {
                this.ColumnList.Add("LocalType");
            }
            return this;
        }

        [DataMapping(ColumnName = "Rack", DbType = DbType.String, Length = 200, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Rack { get; set; }

        public LocationEntity IncludeRack(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Rack"))
            {
                this.ColumnList.Add("Rack");
            }
            return this;
        }

        [DataMapping(ColumnName = "Length", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double Length { get; set; }

        public LocationEntity IncludeLength(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Length"))
            {
                this.ColumnList.Add("Length");
            }
            return this;
        }

        [DataMapping(ColumnName = "Width", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double Width { get; set; }

        public LocationEntity IncludeWidth(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Width"))
            {
                this.ColumnList.Add("Width");
            }
            return this;
        }

        [DataMapping(ColumnName = "Height", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double Height { get; set; }

        public LocationEntity IncludeHeight(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Height"))
            {
                this.ColumnList.Add("Height");
            }
            return this;
        }

        [DataMapping(ColumnName = "X", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double X { get; set; }

        public LocationEntity IncludeX(bool flag)
        {
            if (flag && !this.ColumnList.Contains("X"))
            {
                this.ColumnList.Add("X");
            }
            return this;
        }

        [DataMapping(ColumnName = "Y", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double Y { get; set; }

        public LocationEntity IncludeY(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Y"))
            {
                this.ColumnList.Add("Y");
            }
            return this;
        }

        [DataMapping(ColumnName = "Z", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double Z { get; set; }

        public LocationEntity IncludeZ(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Z"))
            {
                this.ColumnList.Add("Z");
            }
            return this;
        }

        [DataMapping(ColumnName = "UnitNum", DbType = DbType.String, Length = 20, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string UnitNum { get; set; }

        public LocationEntity IncludeUnitNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("UnitNum"))
            {
                this.ColumnList.Add("UnitNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "UnitName", DbType = DbType.String, Length = 20, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string UnitName { get; set; }

        public LocationEntity IncludeUnitName(bool flag)
        {
            if (flag && !this.ColumnList.Contains("UnitName"))
            {
                this.ColumnList.Add("UnitName");
            }
            return this;
        }

        [DataMapping(ColumnName = "Remark", DbType = DbType.String, Length = 8000, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Remark { get; set; }

        public LocationEntity IncludeRemark(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Remark"))
            {
                this.ColumnList.Add("Remark");
            }
            return this;
        }

        [DataMapping(ColumnName = "IsForbid", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 IsForbid { get; set; }

        public LocationEntity IncludeIsForbid(bool flag)
        {
            if (flag && !this.ColumnList.Contains("IsForbid"))
            {
                this.ColumnList.Add("IsForbid");
            }
            return this;
        }

        [DataMapping(ColumnName = "IsDefault", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 IsDefault { get; set; }

        public LocationEntity IncludeIsDefault(bool flag)
        {
            if (flag && !this.ColumnList.Contains("IsDefault"))
            {
                this.ColumnList.Add("IsDefault");
            }
            return this;
        }

        [DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 IsDelete { get; set; }

        public LocationEntity IncludeIsDelete(bool flag)
        {
            if (flag && !this.ColumnList.Contains("IsDelete"))
            {
                this.ColumnList.Add("IsDelete");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public DateTime CreateTime { get; set; }

        public LocationEntity IncludeCreateTime(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateTime"))
            {
                this.ColumnList.Add("CreateTime");
            }
            return this;
        }

    }

    public partial class LocationEntity
    {
        /// <summary>
        /// 是否为默认值
        /// </summary>
        [DataMapping(ColumnName = "IsDefaultStr", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = false)]
        public string IsDefaultStr { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [DataMapping(ColumnName = "IsForbidStr", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = false)]
        public string IsForbidStr { get; set; }

        /// <summary>
        /// 库位类型
        /// </summary>
        [DataMapping(ColumnName = "LocalTypeName", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = false)]
        public string LocalTypeName { get; set; }

        public string StorageName { get; set; }

    }
}
