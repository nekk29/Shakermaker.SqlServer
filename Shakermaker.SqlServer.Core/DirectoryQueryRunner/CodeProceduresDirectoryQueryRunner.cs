using Shakermaker.SqlServer.Core.Base;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Context;
using Shakermaker.SqlServer.Core.Entity;

namespace Shakermaker.SqlServer.Core.DirectoryQueryRunner
{
    public class CodeProceduresDirectoryQueryRunner : BaseDirectoryQueryRunner
    {
        public CodeProceduresDirectoryQueryRunner(DatabaseContext databaseContext, QueryExecution queryExecution, string sourceDirectory) : base(databaseContext, queryExecution, sourceDirectory)
        {

        }

        protected override string QueriesDirectory => Constants.QueryDirectory.Code_Procedures;
        protected override bool CheckPreviousExecution => false;
    }
}
