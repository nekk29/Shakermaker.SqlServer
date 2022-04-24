using Shakermaker.SqlServer.Core.Context;
using System.Threading.Tasks;

namespace Shakermaker.SqlServer.Core.Base
{
    public abstract class BaseCommandQueryRunner : BaseQueryRunner
    {
        public BaseCommandQueryRunner(DatabaseContext databaseContext) : base(databaseContext)
        {

        }

        protected abstract string Command { get; }

        public override async Task<int> Execute()
        {
            return await ExecuteNonQueryAsync(Command);
        }
    }
}
