/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/06/09 14:54:49
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/06/09 14:54:49
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Return
{
    [TableAttribute(DbName = "JooWMS", Name = "Proc_ReturnTOP10Num", IsInternal = false, MapType = MapType.Proc)]
    public partial class Proc_ReturnTOP10NumEntity : BaseEntity
    {
        public Proc_ReturnTOP10NumEntity()
        {
        }

        [DataMapping(ColumnName = "StorageNum", DbType = DbType.String, Length = 50, ColumnType = ColumnType.InPut)]
        public string StorageNum { get; set; }

        [DataMapping(ColumnName = "ProductNum", DbType = DbType.String, Length = 50, ColumnType = ColumnType.InPut)]
        public string ProductNum { get; set; }

        [DataMapping(ColumnName = "ProductName", DbType = DbType.String, Length = 100, ColumnType = ColumnType.InOutPut)]
        public string ProductName { get; set; }

        [DataMapping(ColumnName = "TotalNum", DbType = DbType.Double, Length = 4000, ColumnType = ColumnType.InOutPut)]
        public double TotalNum { get; set; }

        [DataMapping(ColumnName = "BeginTime", DbType = DbType.DateTime, Length = 4000, ColumnType = ColumnType.InPut)]
        public DateTime BeginTime { get; set; }

        [DataMapping(ColumnName = "EndTime", DbType = DbType.DateTime, Length = 4000, ColumnType = ColumnType.InPut)]
        public DateTime EndTime { get; set; }

        [DataMapping(ColumnName = "Status", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InPut)]
        public Int32 Status { get; set; }

        [DataMapping(ColumnName = "IsDelete", DbType = DbType.Int32, Length = 4000, ColumnType = ColumnType.InPut)]
        public Int32 IsDelete { get; set; }

    }
}
