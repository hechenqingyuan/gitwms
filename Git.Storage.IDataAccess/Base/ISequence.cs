/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2015/10/08 11:42:17
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2015/10/08 11:42:17
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;
using Git.Storage.Entity.Base;

namespace Git.Storage.IDataAccess.Base
{
	public partial interface ISequence : IDbHelper<SequenceEntity>
	{
        /// <summary>
        /// 查询系统所有的自定义表信息
        /// </summary>
        /// <returns></returns>
        DataTable GetTables();
	}
}
