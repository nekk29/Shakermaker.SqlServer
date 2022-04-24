using Shakermaker.SqlServer.Core.Base;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Context;
using System;

namespace Shakermaker.SqlServer.Core.CommandQueryRunner
{
    public class CreateQueryFileHistoryTableCommandQueryRunner : BaseCommandQueryRunner
    {
        public CreateQueryFileHistoryTableCommandQueryRunner(DatabaseContext databaseContext) : base(databaseContext)
        {

        }

        protected override string Command =>
            string.Concat(CreateTable, Environment.NewLine, CreatePrimaryKey, Environment.NewLine, CreateForeingKeys);

        private string CreateTable =>
            $@"
                CREATE TABLE [{Constants.Schema.MainSchema}].[{Constants.Table.QueryFileHistory}] (
                    [QueryFileHistoryId] uniqueidentifier NOT NULL,
                    [QueryExecutionId] uniqueidentifier NOT NULL,
                    [QueryType] varchar(255) NOT NULL,
                    [FileName] varchar(1000) NOT NULL,
                    [Content] text NOT NULL,
                    [ExecutionStartDate] datetimeoffset(7) NOT NULL,
                    [ExecutionEndDate] datetimeoffset(7) NULL
                );
            ";

        private string CreatePrimaryKey =>
            $@"
                ALTER TABLE [{Constants.Schema.MainSchema}].[{Constants.Table.QueryFileHistory}]
                ADD CONSTRAINT [{Constants.Table.QueryFileHistory}_pkey] PRIMARY KEY ([QueryFileHistoryId]);
            ";

        private string CreateForeingKeys =>
            $@"
                ALTER TABLE [{Constants.Schema.MainSchema}].[{Constants.Table.QueryFileHistory}] WITH CHECK
                ADD CONSTRAINT [{Constants.Table.QueryFileHistory}_{Constants.Table.QueryExecution}_fkey] FOREIGN KEY([QueryExecutionId])
                REFERENCES [{Constants.Schema.MainSchema}].[{Constants.Table.QueryExecution}] ([QueryExecutionId]);
            ";
    }
}
