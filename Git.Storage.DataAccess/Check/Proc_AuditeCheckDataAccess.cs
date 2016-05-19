/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/04/15 21:38:12
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/04/15 21:38:12
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
	public partial class Proc_AuditeCheckDataAccess : DbProcHelper<Proc_AuditeCheckEntity>, IProc_AuditeCheck
	{
		public Proc_AuditeCheckDataAccess()
		{
		}

	}
}
