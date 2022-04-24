using Microsoft.EntityFrameworkCore;
using Shakermaker.SqlServer.Core.Context;
using System;
using System.Threading.Tasks;

namespace Shakermaker.SqlServer.Core.Base
{
    public abstract class BaseQueryRunner
    {
        protected readonly DatabaseContext _databaseContext;

        public BaseQueryRunner(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException("databaseContext");
        }

        public abstract Task<int> Execute();

        protected async Task<int> ExecuteNonQueryAsync(string commandText)
        {
            using var command = _databaseContext.Database.GetDbConnection().CreateCommand();

            command.CommandText = commandText;

            _databaseContext.Database.OpenConnection();

            var result = await command.ExecuteNonQueryAsync();

            _databaseContext.Database.CloseConnection();

            return result;
        }

        protected async Task<T> ExecuteScalarAsync<T>(string commandText)
        {
            using var command = _databaseContext.Database.GetDbConnection().CreateCommand();

            command.CommandText = commandText;

            _databaseContext.Database.OpenConnection();

            var scalar = await command.ExecuteScalarAsync();
            var result = scalar.GetType() == typeof(T) ? (T)scalar : default;

            _databaseContext.Database.CloseConnection();

            return result;
        }
    }
}
