/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 23:01:06
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 23:01:06
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class TNum_To
	{
		public TNum_To()
		{
		}

		public static TNumEntity To(TNum_CE item)
		{
			TNumEntity target = new TNumEntity();
			target.ID=item.ID;
			target.Num=item.Num;
			target.MinNum=item.MinNum;
			target.MaxNum=item.MaxNum;
			target.Day=item.Day;
			target.TabName=item.TabName;
			return target;
		}

		public static TNum_CE ToCE(TNumEntity item)
		{
			TNum_CE target = new TNum_CE();
			target.ID=item.ID;
			target.Num=item.Num;
			target.MinNum=item.MinNum;
			target.MaxNum=item.MaxNum;
			target.Day=item.Day;
			target.TabName=item.TabName;
			return target;
		}
	}
}
