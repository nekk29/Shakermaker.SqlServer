using Shakermaker.SqlServer.Core.Base;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Context;
using Shakermaker.SqlServer.Core.Entity;

namespace Shakermaker.SqlServer.Core.DirectoryQueryRunner
{
    public class DataEnvironmentsDirectoryQueryRunner : BaseDirectoryQueryRunner
    {
        public DataEnvironmentsDirectoryQueryRunner(DatabaseContext databaseContext, QueryExecution queryExecution, string sourceDirectory, string environmentPrefixFilter) : base(databaseContext, queryExecution, sourceDirectory)
        {
            EnvironmentPrefixFilter = environmentPrefixFilter;
        }

        protected override string QueriesDirectory => Constants.QueryDirectory.Data_Environments;
    }
}
