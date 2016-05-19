/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-11-29 23:31:50
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-11-29 23:31:50
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;
using Git.Framework.MsSql;
using Git.Storage.Entity.Bad;
using Git.Storage.IDataAccess.Bad;

namespace Git.Storage.DataAccess.Bad
{
	public partial class BadReportDataAccess : DbHelper<BadReportEntity>, IBadReport
	{
		public BadReportDataAccess()
		{
		}

	}
}
