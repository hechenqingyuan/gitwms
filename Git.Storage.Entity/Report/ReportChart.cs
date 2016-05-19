/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2014-09-29 19:24:03
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2014-09-29 19:24:03       情缘
*********************************************************************************/

using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Git.Storage.Entity.Report
{
    public partial class ReportChart
    {
        public ReportChart() { }

        [DataMapping(ColumnName = "ProductNum", DbType = DbType.String)]
        public string ProductNum { get; set; }

        [DataMapping(ColumnName = "BarCode", DbType = DbType.String)]
        public string BarCode { get; set; }

        [DataMapping(ColumnName = "ProductName", DbType = DbType.String)]
        public string ProductName { get; set; }

        [DataMapping(ColumnName = "Size", DbType = DbType.String)]
        public string Size { get; set; }

        [DataMapping(ColumnName = "Num", DbType = DbType.Double)]
        public double Num { get; set; }
    }
}
