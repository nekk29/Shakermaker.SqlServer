using Shakermaker.SqlServer.Core.Base;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Context;
using System;

namespace Shakermaker.SqlServer.Core.CommandQueryRunner
{
    public class CreateQueryExecutionTableCommandQueryRunner : BaseCommandQueryRunner
    {
        public CreateQueryExecutionTableCommandQueryRunner(DatabaseContext databaseContext) : base(databaseContext)
        {

        }

        protected override string Command =>
            string.Concat(CreateTable, Environment.NewLine, CreatePrimaryKey);

        private string CreateTable =>
            $@"
                CREATE TABLE [{Constants.Schema.MainSchema}].[{Constants.Table.QueryExecution}] (
                  [QueryExecutionId] uniqueidentifier NOT NULL,
                  [Application] varchar(255) NOT NULL,
                  [Release] varchar(255) NOT NULL,
                  [Environment] varchar(255) NOT NULL,
                  [Result] varchar(1000) NOT NULL,
                  [Log] text NOT NULL,
                  [ExecutionUser] varchar(255) NOT NULL,
                  [ExecutionStartDate] datetimeoffset(7) NOT NULL,
                  [ExecutionEndDate] datetimeoffset(7) NULL
                );
            ";

        private string CreatePrimaryKey =>
            $@"
                ALTER TABLE [{Constants.Schema.MainSchema}].[{Constants.Table.QueryExecution}]
                ADD CONSTRAINT [{Constants.Table.QueryExecution}_pkey] PRIMARY KEY ([QueryExecutionId]);
            ";
    }
}
