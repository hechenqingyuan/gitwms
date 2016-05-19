/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2014/05/09 23:01:01
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Copyright: 太数智能科技（上海）有限公司 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2014/05/09 23:01:01
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Git.Storage.RoseEntity;
using Git.Storage.Entity.Base;

namespace Git.Storage.Conversion
{
	public partial class MeasureRel_To
	{
		public MeasureRel_To()
		{
		}

		public static MeasureRelEntity To(MeasureRel_CE item)
		{
			MeasureRelEntity target = new MeasureRelEntity();
			target.ID=item.ID;
			target.SN=item.SN;
			target.MeasureSource=item.MeasureSource;
			target.MeasureTarget=item.MeasureTarget;
			target.Rate=item.Rate;
			return target;
		}

		public static MeasureRel_CE ToCE(MeasureRelEntity item)
		{
			MeasureRel_CE target = new MeasureRel_CE();
			target.ID=item.ID;
			target.SN=item.SN;
			target.MeasureSource=item.MeasureSource;
			target.MeasureTarget=item.MeasureTarget;
			target.Rate=item.Rate;
			return target;
		}
	}
}
