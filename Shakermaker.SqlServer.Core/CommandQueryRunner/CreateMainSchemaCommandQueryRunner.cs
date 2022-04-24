using Shakermaker.SqlServer.Core.Base;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Context;

namespace Shakermaker.SqlServer.Core.CommandQueryRunner
{
    public class CreateMainSchemaCommandQueryRunner : BaseCommandQueryRunner
    {
        public CreateMainSchemaCommandQueryRunner(DatabaseContext databaseContext) : base(databaseContext)
        {

        }

        protected override string Command => $@"CREATE SCHEMA {Constants.Schema.MainSchema};";
    }
}
