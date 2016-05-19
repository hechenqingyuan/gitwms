/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:43:39
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:43:39
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class SysRelation_To
	{
		public SysRelation_To()
		{
		}

		public static SysRelationEntity To(SysRelation_CE item)
		{
			SysRelationEntity target = new SysRelationEntity();
			target.ID=item.ID;
			target.RoleNum=item.RoleNum;
			target.ResNum=item.ResNum;
			target.ResType=item.ResType;
			return target;
		}

		public static SysRelation_CE ToCE(SysRelationEntity item)
		{
			SysRelation_CE target = new SysRelation_CE();
			target.ID=item.ID;
			target.RoleNum=item.RoleNum;
			target.ResNum=item.ResNum;
			target.ResType=item.ResType;
			return target;
		}
	}
}
