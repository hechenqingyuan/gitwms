/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/04/15 10:47:41
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/04/15 10:47:41
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;
using Git.Framework.MsSql;
using Git.Storage.Entity.Check;
using Git.Storage.IDataAccess.Check;

namespace Git.Storage.DataAccess.Check
{
	public partial class Proc_CreateCheckDataAccess : DbProcHelper<Proc_CreateCheckEntity>, IProc_CreateCheck
	{
		public Proc_CreateCheckDataAccess()
		{
		}

	}
}
