using Git.Framework.ORM;
using Git.Storage.Entity.Bad;
using Git.Storage.Entity.InStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Storage.IDataAccess.Bad
{
    public partial interface IProc_AuditeBadReport : IDbProcHelper<Proc_AuditeBadReportEntity>
    {
    }
}
