using Shakermaker.SqlServer.Core.Base;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Context;
using Shakermaker.SqlServer.Core.Entity;

namespace Shakermaker.SqlServer.Core.DirectoryQueryRunner
{
    public class DataFixesDirectoryQueryRunner : BaseDirectoryQueryRunner
    {
        public DataFixesDirectoryQueryRunner(DatabaseContext databaseContext, QueryExecution queryExecution, string sourceDirectory) : base(databaseContext, queryExecution, sourceDirectory)
        {

        }

        protected override string QueriesDirectory => Constants.QueryDirectory.Data_Fixes;
    }
}
