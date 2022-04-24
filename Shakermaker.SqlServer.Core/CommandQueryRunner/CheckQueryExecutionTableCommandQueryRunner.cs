using Shakermaker.SqlServer.Core.Base;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Context;
using System.Threading.Tasks;

namespace Shakermaker.SqlServer.Core.CommandQueryRunner
{
    public class CheckQueryExecutionTableCommandQueryRunner : BaseCommandQueryRunner
    {
        public CheckQueryExecutionTableCommandQueryRunner(DatabaseContext databaseContext) : base(databaseContext)
        {

        }

        public async Task<bool> TableExists()
        {
            return await ExecuteScalarAsync<bool>(Command);
        }

        protected override string Command =>
            $@"
                DECLARE @Table varchar(255);
                SELECT @Table = TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = '{Constants.Schema.MainSchema}'
                AND TABLE_NAME = '{Constants.Table.QueryExecution}';
                SELECT CAST(CASE WHEN @Table IS NULL THEN 0 ELSE 1 END AS BIT);
            ";
    }
}
