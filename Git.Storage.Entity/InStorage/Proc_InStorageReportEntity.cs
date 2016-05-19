/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014-05-16 14:37:08
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014-05-16 14:37:08
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.InStorage
{
    [TableAttribute(DbName = "JooWMS", Name = "Proc_InStorageReport", IsInternal = false, MapType = MapType.Proc)]
    public partial class Proc_InStorageReportEntity : BaseEntity
    {
        public Proc_InStorageReportEntity()
        {
        }

        [DataMapping(ColumnName = "Num", DbType = DbType.Double, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public double Num { get; set; }

        [DataMapping(ColumnName = "Amount", DbType = DbType.Double, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public double Amount { get; set; }

        [DataMapping(ColumnName = "RecordCount", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public Int32 RecordCount { get; set; }

        [DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public DateTime CreateTime { get; set; }

        [DataMapping(ColumnName = "PageIndex", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InPut)]
        public Int32 PageIndex { get; set; }

        [DataMapping(ColumnName = "StorageNum", DbType = DbType.String, Length = 50, ColumnType = ColumnType.InPut)]
        public string StorageNum { get; set; }

        [DataMapping(ColumnName = "PageSize", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InPut)]
        public Int32 PageSize { get; set; }

        [DataMapping(ColumnName = "BeginTime", DbType = DbType.DateTime, Length = 4000, ColumnType = ColumnType.InPut)]
        public DateTime BeginTime { get; set; }

        [DataMapping(ColumnName = "EndTime", DbType = DbType.DateTime, Length = 4000, ColumnType = ColumnType.InPut)]
        public DateTime EndTime { get; set; }

    }
}
