using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Git.Storage.Common
{
    public enum ECheckType
    {
        [Description("库位盘点")]
        Local = 1,

        [Description("产品盘点")]
        Product = 2
    }
}
