using Git.Framework.MsSql;
using Git.Storage.Entity.Store;
using Git.Storage.IDataAccess.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Storage.DataAccess.Store
{
    public partial class Proc_ProductReportDataAccess : DbProcHelper<Proc_ProductReportEntity>, IProc_ProductReport
    {
        public Proc_ProductReportDataAccess()
        {

        }
    }
}
