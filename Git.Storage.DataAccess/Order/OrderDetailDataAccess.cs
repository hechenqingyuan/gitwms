/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014-08-06 20:13:04
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014-08-06 20:13:04
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;
using Git.Framework.MsSql;
using Git.Storage.Entity.Order;
using Git.Storage.IDataAccess.Order;

namespace Git.Storage.DataAccess.Order
{
	public partial class OrderDetailDataAccess : DbHelper<OrderDetailEntity>, IOrderDetail
	{
		public OrderDetailDataAccess()
		{
		}

	}
}
