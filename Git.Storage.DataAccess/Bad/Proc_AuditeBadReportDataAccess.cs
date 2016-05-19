using Git.Framework.MsSql;
using Git.Storage.Entity.Bad;
using Git.Storage.IDataAccess.Bad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Storage.DataAccess.Bad
{
    public partial class Proc_AuditeBadReportDataAccess : DbProcHelper<Proc_AuditeBadReportEntity>, IProc_AuditeBadReport
    {
        public Proc_AuditeBadReportDataAccess()
        {
        }

    }

}
