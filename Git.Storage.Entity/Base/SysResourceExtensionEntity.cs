/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-10-28 23:12:36
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-10-28 23:12:36       情缘
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Storage.Entity.Base
{
    public partial class SysResourceEntity
    {
        public string ParentName { get; set; }

        public string ResouceType { get; set; }


        public SysResourceEntity Parent { get; set; }

        public List<SysResourceEntity> Children { get; set; }
    }
}
