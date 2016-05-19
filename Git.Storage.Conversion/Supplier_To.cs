/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:49
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:49
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Store;

namespace Git.Storage.Conversion
{
	public partial class Supplier_To
	{
		public Supplier_To()
		{
		}

		public static SupplierEntity To(Supplier_CE item)
		{
			SupplierEntity target = new SupplierEntity();
			target.ID=item.ID;
			target.SupNum=item.SupNum;
			target.SupName=item.SupName;
			target.Phone=item.Phone;
			target.Fax=item.Fax;
			target.Email=item.Email;
			target.ContactName=item.ContactName;
			target.Address=item.Address;
			target.CreateUser=item.CreateUser;
			target.Description=item.Description;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			return target;
		}

		public static Supplier_CE ToCE(SupplierEntity item)
		{
			Supplier_CE target = new Supplier_CE();
			target.ID=item.ID;
			target.SupNum=item.SupNum;
			target.SupName=item.SupName;
			target.Phone=item.Phone;
			target.Fax=item.Fax;
			target.Email=item.Email;
			target.ContactName=item.ContactName;
			target.Address=item.Address;
			target.CreateUser=item.CreateUser;
			target.Description=item.Description;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			return target;
		}
	}
}
