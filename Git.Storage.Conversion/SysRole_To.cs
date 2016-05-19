/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:40:50
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:40:50
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class SysRole_To
	{
		public SysRole_To()
		{
		}

		public static SysRoleEntity To(SysRole_CE item)
		{
			SysRoleEntity target = new SysRoleEntity();
			target.ID=item.ID;
			target.RoleNum=item.RoleNum;
			target.RoleName=item.RoleName;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.Remark=item.Remark;
			return target;
		}

		public static SysRole_CE ToCE(SysRoleEntity item)
		{
			SysRole_CE target = new SysRole_CE();
			target.ID=item.ID;
			target.RoleNum=item.RoleNum;
			target.RoleName=item.RoleName;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			target.Remark=item.Remark;
			return target;
		}
	}
}
