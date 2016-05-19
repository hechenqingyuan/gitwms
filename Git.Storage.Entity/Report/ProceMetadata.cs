/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2015/9/6 8:59:50
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2015/9/6 8:59:50       情缘
*********************************************************************************/

using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Git.Storage.Entity.Report
{
    public partial class ProceMetadata
    {
        public ProceMetadata() { }

        [DataMapping(ColumnName = "SPECIFIC_CATALOG", DbType = DbType.String)]
        public string SPECIFIC_CATALOG { get; set; }

        [DataMapping(ColumnName = "SPECIFIC_NAME", DbType = DbType.String)]
        public string SPECIFIC_NAME { get; set; }

        [DataMapping(ColumnName = "ORDINAL_POSITION", DbType = DbType.Int32)]
        public int ORDINAL_POSITION { get; set; }

        [DataMapping(ColumnName = "PARAMETER_MODE", DbType = DbType.String)]
        public string PARAMETER_MODE { get; set; }

        [DataMapping(ColumnName = "PARAMETER_NAME", DbType = DbType.String)]
        public string PARAMETER_NAME { get; set; }

        [DataMapping(ColumnName = "DATA_TYPE", DbType = DbType.String)]
        public string DATA_TYPE { get; set; }

        [DataMapping(ColumnName = "CHARACTER_MAXIMUM_LENGTH", DbType = DbType.String)]
        public string CHARACTER_MAXIMUM_LENGTH { get; set; }

        [DataMapping(ColumnName = "ShowName", DbType = DbType.String)]
        public string ShowName { get; set; }

        [DataMapping(ColumnName = "F_Param_Element", DbType = DbType.String)]
        public string F_Param_Element { get; set; }
    }
}
