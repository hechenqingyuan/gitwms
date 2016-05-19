/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-11-29 23:30:39
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-11-29 23:30:39
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Store
{
    [TableAttribute(DbName = "JooWMS", Name = "LocalProduct", PrimaryKeyName = "ID", IsInternal = false)]
    public partial class LocalProductEntity : BaseEntity
    {
        public LocalProductEntity()
        {
        }

        [DataMapping(ColumnName = "ID", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = true, AutoIncrement = true, IsMap = true)]
        public Int32 ID { get; set; }

        public LocalProductEntity IncludeID(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ID"))
            {
                this.ColumnList.Add("ID");
            }
            return this;
        }

        [DataMapping(ColumnName = "Sn", DbType = DbType.String, Length = 50, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Sn { get; set; }

        public LocalProductEntity IncludeSn(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Sn"))
            {
                this.ColumnList.Add("Sn");
            }
            return this;
        }

        [DataMapping(ColumnName = "StorageNum", DbType = DbType.String, Length = 50, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string StorageNum { get; set; }

        public LocalProductEntity IncludeStorageNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("StorageNum"))
            {
                this.ColumnList.Add("StorageNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "StorageName", DbType = DbType.String, Length = 100, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string StorageName { get; set; }

        public LocalProductEntity IncludeStorageName(bool flag)
        {
            if (flag && !this.ColumnList.Contains("StorageName"))
            {
                this.ColumnList.Add("StorageName");
            }
            return this;
        }

        [DataMapping(ColumnName = "LocalNum", DbType = DbType.String, Length = 50, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string LocalNum { get; set; }

        public LocalProductEntity IncludeLocalNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("LocalNum"))
            {
                this.ColumnList.Add("LocalNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "LocalName", DbType = DbType.String, Length = 100, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string LocalName { get; set; }

        public LocalProductEntity IncludeLocalName(bool flag)
        {
            if (flag && !this.ColumnList.Contains("LocalName"))
            {
                this.ColumnList.Add("LocalName");
            }
            return this;
        }

        [DataMapping(ColumnName = "LocalType", DbType = DbType.Int32, Length = 4, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 LocalType { get; set; }

        public LocalProductEntity IncludeLocalType(bool flag)
        {
            if (flag && !this.ColumnList.Contains("LocalType"))
            {
                this.ColumnList.Add("LocalType");
            }
            return this;
        }

        [DataMapping(ColumnName = "ProductNum", DbType = DbType.String, Length = 50, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string ProductNum { get; set; }

        public LocalProductEntity IncludeProductNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ProductNum"))
            {
                this.ColumnList.Add("ProductNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "BarCode", DbType = DbType.String, Length = 50, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string BarCode { get; set; }

        public LocalProductEntity IncludeBarCode(bool flag)
        {
            if (flag && !this.ColumnList.Contains("BarCode"))
            {
                this.ColumnList.Add("BarCode");
            }
            return this;
        }

        [DataMapping(ColumnName = "ProductName", DbType = DbType.String, Length = 400, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string ProductName { get; set; }

        public LocalProductEntity IncludeProductName(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ProductName"))
            {
                this.ColumnList.Add("ProductName");
            }
            return this;
        }

        [DataMapping(ColumnName = "BatchNum", DbType = DbType.String, Length = 100, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string BatchNum { get; set; }

        public LocalProductEntity IncludeBatchNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("BatchNum"))
            {
                this.ColumnList.Add("BatchNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "Num", DbType = DbType.Double, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public double Num { get; set; }

        public LocalProductEntity IncludeNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Num"))
            {
                this.ColumnList.Add("Num");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public DateTime CreateTime { get; set; }

        public LocalProductEntity IncludeCreateTime(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateTime"))
            {
                this.ColumnList.Add("CreateTime");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateUser", DbType = DbType.String, Length = 50, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string CreateUser { get; set; }

        public LocalProductEntity IncludeCreateUser(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateUser"))
            {
                this.ColumnList.Add("CreateUser");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateName", DbType = DbType.String, Length = 100, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string CreateName { get; set; }

        public LocalProductEntity IncludeCreateName(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateName"))
            {
                this.ColumnList.Add("CreateName");
            }
            return this;
        }

        [DataMapping(ColumnName = "Remark", DbType = DbType.String, Length = 400, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Remark { get; set; }

        public LocalProductEntity IncludeRemark(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Remark"))
            {
                this.ColumnList.Add("Remark");
            }
            return this;
        }

    }

    public partial class LocalProductEntity
    {
        /// <summary>
        /// 提交数量
        /// </summary>
        [DataMapping(ColumnName = "Qty", DbType = DbType.Decimal)]
        public double Qty { get; set; }

        /// <summary>
        /// 产品规格
        /// </summary>
        [DataMapping(ColumnName = "Size", DbType = DbType.String)]
        public string Size { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [DataMapping(ColumnName = "CateName", DbType = DbType.String)]
        public string CateName { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [DataMapping(ColumnName = "CateNum", DbType = DbType.String)]
        public string CateNum { get; set; }

        /// <summary>
        /// 预警值下限
        /// </summary>
        [DataMapping(ColumnName = "MinNum", DbType = DbType.Decimal)]
        public double MinNum { get; set; }

        /// <summary>
        /// 预警值上限
        /// </summary>
        [DataMapping(ColumnName = "MaxNum", DbType = DbType.Decimal)]
        public double MaxNum { get; set; }

        /// <summary>
        /// 平均价格
        /// </summary>
        [DataMapping(ColumnName = "AvgPrice", DbType = DbType.Decimal)]
        public double AvgPrice { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        [DataMapping(ColumnName = "InPrice", DbType = DbType.Decimal)]
        public double InPrice { get; set; }

        /// <summary>
        /// 报损数量
        /// </summary>
        [DataMapping(ColumnName = "BadNum", DbType = DbType.Decimal)]
        public double BadNum { get; set; }

        /// <summary>
        /// 移库数量
        /// </summary>
        [DataMapping(ColumnName = "MoveNum", DbType = DbType.Decimal)]
        public double MoveNum { get; set; }

        /// <summary>
        /// 移入库位
        /// </summary>
        [DataMapping(ColumnName = "ToLocalNum", DbType = DbType.Decimal)]
        public double ToLocalNum { get; set; }

        /// <summary>
        /// 总价格
        /// </summary>
        public double TotalPrice { get; set; }

        /// <summary>
        /// 所有总价格的和
        /// </summary>
        public double AllTotalPrice { get; set; }

        /// <summary>
        /// 所有库存数的和
        /// </summary>
        public double AllNum { get; set; }
    }
}
