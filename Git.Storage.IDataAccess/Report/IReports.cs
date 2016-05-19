/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2015/09/05 13:07:30
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2015/09/05 13:07:30
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;
using Git.Storage.Entity.Report;

namespace Git.Storage.IDataAccess.Report
{
	public partial interface IReports : IDbHelper<ReportsEntity>
	{
        /// <summary>
        /// 根据存储过程名称查询元数据
        /// </summary>
        /// <param name="argProceName"></param>
        /// <returns></returns>
        List<ProceMetadata> GetMetadataList(string argProceName);

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        DataSet GetDataSource(ReportsEntity entity, List<ReportParamsEntity> list);
	}
}
