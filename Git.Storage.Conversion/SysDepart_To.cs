/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:48:54
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:48:54
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class SysDepart_To
	{
		public SysDepart_To()
		{
		}

		public static SysDepartEntity To(SysDepart_CE item)
		{
			SysDepartEntity target = new SysDepartEntity();
			target.ID=item.ID;
			target.DepartNum=item.DepartNum;
			target.DepartName=item.DepartName;
			target.ChildCount=item.ChildCount;
			target.ParentNum=item.ParentNum;
			target.Depth=item.Depth;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			return target;
		}

		public static SysDepart_CE ToCE(SysDepartEntity item)
		{
			SysDepart_CE target = new SysDepart_CE();
			target.ID=item.ID;
			target.DepartNum=item.DepartNum;
			target.DepartName=item.DepartName;
			target.ChildCount=item.ChildCount;
			target.ParentNum=item.ParentNum;
			target.Depth=item.Depth;
			target.IsDelete=item.IsDelete;
			target.CreateTime=item.CreateTime;
			return target;
		}
	}
}
