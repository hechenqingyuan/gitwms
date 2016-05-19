/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 22:46:07
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 22:46:07
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class VnArea_To
	{
		public VnArea_To()
		{
		}

		public static VnAreaEntity To(VnArea_CE item)
		{
			VnAreaEntity target = new VnAreaEntity();
			target.ID=item.ID;
			target.Code=item.Code;
			target.AName=item.AName;
			target.ANameEn=item.ANameEn;
			target.CCode=item.CCode;
			return target;
		}

		public static VnArea_CE ToCE(VnAreaEntity item)
		{
			VnArea_CE target = new VnArea_CE();
			target.ID=item.ID;
			target.Code=item.Code;
			target.AName=item.AName;
			target.ANameEn=item.ANameEn;
			target.CCode=item.CCode;
			return target;
		}
	}
}
