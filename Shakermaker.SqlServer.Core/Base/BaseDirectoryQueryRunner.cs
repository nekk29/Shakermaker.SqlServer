using Shakermaker.SqlServer.Core.Context;
using Shakermaker.SqlServer.Core.Entity;
using Shakermaker.SqlServer.Core.Utils;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shakermaker.SqlServer.Core.Base
{
    public abstract class BaseDirectoryQueryRunner : BaseQueryRunner
    {
        private readonly string _sourceDirectory;
        private readonly QueryExecution _queryExecution;

        public BaseDirectoryQueryRunner(DatabaseContext databaseContext, QueryExecution queryExecution, string sourceDirectory) : base(databaseContext)
        {
            _sourceDirectory = sourceDirectory;
            _queryExecution = queryExecution;
        }

        protected abstract string QueriesDirectory { get; }
        protected virtual string EnvironmentPrefixFilter { get; set; }
        protected virtual bool CheckPreviousExecution => true;

        public override async Task<int> Execute()
        {
            var result = default(int);
            var executionsCounter = default(int);
            var absQueriesDirectory = string.IsNullOrEmpty(_sourceDirectory) ? QueriesDirectory : string.Concat(_sourceDirectory, "/", QueriesDirectory);

            if (!Directory.Exists(absQueriesDirectory))
            {
                Logger.LogWarning($"- The directory for this section does not exist");
                return result;
            }

            var queryFileHistoryRepository = new BaseRepository<QueryFileHistory>(_databaseContext);

            var queryType = QueriesDirectory.Replace("/", ".");
            var queryFiles = Directory.GetFiles(absQueriesDirectory, "*.sql");
            var filesInfo = queryFiles.Select(x => new FileInfo(x));

            if (!string.IsNullOrEmpty(EnvironmentPrefixFilter))
                filesInfo = filesInfo.Where(x => x.Name.StartsWith($"{EnvironmentPrefixFilter}-"));

            filesInfo = filesInfo.OrderBy(x => x.Name);

            foreach (var fileInfo in filesInfo)
            {
                var fileContent = File.ReadAllText(fileInfo.FullName);

                if (CheckPreviousExecution)
                {
                    var queryFileHistoryExisting = await queryFileHistoryRepository.GetByAsync(x =>
                        x.QueryExecution.Application == _queryExecution.Application &&
                        x.QueryExecution.Release == _queryExecution.Release &&
                        x.QueryExecution.Environment == _queryExecution.Environment &&
                        x.QueryType == queryType &&
                        x.FileName == fileInfo.Name
                    );

                    if (queryFileHistoryExisting != null)
                        continue;
                }

                var executionStartDate = DateTimeOffset.Now;

                Logger.Log($"- Executing script from file '{fileInfo.Name}'");

                result = await ExecuteNonQueryAsync(fileContent);

                executionsCounter++;

                var executionEndDate = DateTimeOffset.Now;

                await queryFileHistoryRepository.AddAsync(new QueryFileHistory
                {
                    QueryExecutionId = _queryExecution.QueryExecutionId,
                    QueryType = queryType,
                    FileName = fileInfo.Name,
                    Content = fileContent,
                    ExecutionStartDate = executionStartDate,
                    ExecutionEndDate = executionEndDate
                });

                await queryFileHistoryRepository.SaveAsync();
            }

            if (executionsCounter == default)
                Logger.LogWarning($"- There are no scripts to execute in this section");

            return result;
        }
    }
}
