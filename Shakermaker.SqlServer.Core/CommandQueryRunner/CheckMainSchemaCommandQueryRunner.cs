using Shakermaker.SqlServer.Core.Base;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Context;
using System.Threading.Tasks;

namespace Shakermaker.SqlServer.Core.CommandQueryRunner
{
    public class CheckMainSchemaCommandQueryRunner : BaseCommandQueryRunner
    {
        public CheckMainSchemaCommandQueryRunner(DatabaseContext databaseContext) : base(databaseContext)
        {

        }

        public async Task<bool> SchemaExists()
        {
            return await ExecuteScalarAsync<bool>(Command);
        }

        protected override string Command =>
            $@"
                DECLARE @Schema varchar(255);
	            SELECT @Schema = name FROM sys.schemas
                WHERE name = '{Constants.Schema.MainSchema}';
                SELECT CAST(CASE WHEN @Schema IS NULL THEN 0 ELSE 1 END AS BIT);
            ";
    }
}
