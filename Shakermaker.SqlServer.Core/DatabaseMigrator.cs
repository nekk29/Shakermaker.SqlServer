using CommandLine;
using Microsoft.EntityFrameworkCore;
using Shakermaker.SqlServer.Core.Base;
using Shakermaker.SqlServer.Core.CommandQueryRunner;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Context;
using Shakermaker.SqlServer.Core.DirectoryQueryRunner;
using Shakermaker.SqlServer.Core.Entity;
using Shakermaker.SqlServer.Core.Utils;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Shakermaker.SqlServer.Core
{
    public class DatabaseMigrator
    {
        public async Task ExecuteMigration(string[] args)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            Logger.Reset();

            Logger.Log($"==================================================================================");
            Logger.Log($"Task            : Shakermaker Continuous Integration Tool");
            Logger.Log($"Description     : Run Continuous Interation Database for SQL Server");
            Logger.Log($"Version         : {executingAssembly.GetName().Version}");
            Logger.Log($"Author          : Eleven Technologies");
            Logger.Log($"==================================================================================");

            var parseArguments = Parser.Default.ParseArguments<Options>(args);

            var options = parseArguments.MapResult(
                opt =>
                {
                    return opt;
                },
                errors =>
                {
                    Logger.LogErrorObject(errors);
                    throw new Exception("An error has ocurred parsing the arguments");
                }
            );

            Logger.LogInfo($"Instancing SQL Server database connection");

            var databaseContext = new DatabaseContext(options.ConnectionString);
            var databaseContextConnection = databaseContext.Database.GetDbConnection();

            Logger.Log($"DataSource: {databaseContextConnection.DataSource}");
            Logger.Log($"Database: {databaseContextConnection.Database}");

            Logger.LogInfo($"Initializing database objects for continuous integration");


            Logger.Log($"Checking schema '{Constants.Schema.MainSchema}'");

            var objectExists = await new CheckMainSchemaCommandQueryRunner(databaseContext).SchemaExists();

            if (objectExists)
                Logger.Log($"Schema '{Constants.Schema.MainSchema}' already exists");
            else
            {
                Logger.Log($"Creating schema '{Constants.Schema.MainSchema}'");
                await new CreateMainSchemaCommandQueryRunner(databaseContext).Execute();
            }


            Logger.Log($"Checking table '{Constants.Table.QueryExecution}'");

            objectExists = await new CheckQueryExecutionTableCommandQueryRunner(databaseContext).TableExists();

            if (objectExists)
                Logger.Log($"Table '{Constants.Table.QueryExecution}' already exists");
            else
            {
                Logger.Log($"Creating table '{Constants.Table.QueryExecution}'");
                await new CreateQueryExecutionTableCommandQueryRunner(databaseContext).Execute();
            }


            Logger.Log($"Checking table '{Constants.Table.QueryFileHistory}'");

            objectExists = await new CheckQueryFileHistoryTableCommandQueryRunner(databaseContext).TableExists();

            if (objectExists)
                Logger.Log($"Table '{Constants.Table.QueryFileHistory}' already exists");
            else
            {
                Logger.Log($"Creating table '{Constants.Table.QueryFileHistory}'");
                await new CreateQueryFileHistoryTableCommandQueryRunner(databaseContext).Execute();
            }


            Logger.LogInfo($"Creating query execution for release '{options.Release}' in environment '{options.Environment}'");

            var queryExecution = new QueryExecution
            {
                Application = string.IsNullOrEmpty(options.Application) ? Constants.Application.MainApplication : options.Application,
                Release = options.Release,
                Environment = options.Environment,
                Result = Constants.Result.QueryExecution.Created,
                Log = string.Empty,
                ExecutionUser = Environment.UserName,
                ExecutionStartDate = DateTimeOffset.Now
            };

            var queryExecutionRepository = new BaseRepository<QueryExecution>(databaseContext);

            await queryExecutionRepository.AddAsync(queryExecution);
            await queryExecutionRepository.SaveAsync();

            Logger.Log($"Query execution for release '{options.Release}' in environment '{options.Environment}' was created");


            Logger.LogInfo($"Executing database updates for release '{options.Release}' in environment '{options.Environment}'");


            Logger.Log($"Starting query execution for '{options.Release}' in environment '{options.Environment}'");

            queryExecution.Result = Constants.Result.QueryExecution.InProgress;

            await queryExecutionRepository.UpdateAsync(queryExecution);
            await queryExecutionRepository.SaveAsync();

            try
            {
                Logger.Log($"Executing changes for sequences section");
                await new SchemaSequencesDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory).Execute();

                Logger.Log($"Executing changes for tables section");
                await new SchemaTablesDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory).Execute();

                Logger.Log($"Executing changes for types section");
                await new SchemaTypesDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory).Execute();

                Logger.Log($"Executing changes for views section");
                await new SchemaViewsDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory).Execute();

                Logger.Log($"Executing changes for synonyms section");
                await new SchemaSynonymsDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory).Execute();

                Logger.Log($"Executing changes for functions section");
                await new CodeFunctionsDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory).Execute();

                Logger.Log($"Executing changes for procedures section");
                await new CodeProceduresDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory).Execute();

                Logger.Log($"Executing changes for data fixes section");
                await new DataFixesDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory).Execute();

                Logger.Log($"Executing changes for data settings section");
                await new DataSettingsDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory, options.Environment).Execute();

                Logger.Log($"Executing changes for data per environment section");
                await new DataEnvironmentsDirectoryQueryRunner(databaseContext, queryExecution, options.SourceDirectory, options.Environment).Execute();

                Logger.LogSuccess($"Finalizing succeded query execution for '{options.Release}' in environment '{options.Environment}'");

                queryExecution.Result = Constants.Result.QueryExecution.Succeded;
                queryExecution.ExecutionEndDate = DateTimeOffset.Now;

                await queryExecutionRepository.UpdateAsync(queryExecution);
                await queryExecutionRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Finalizing query execution with error for '{options.Release}' in environment '{options.Environment}'");

                var log = ex.InnerException == null ?
                    string.Concat(ex.Message, Environment.NewLine, ex.StackTrace) :
                    string.Concat(ex.InnerException.Message, Environment.NewLine, ex.InnerException.StackTrace);

                queryExecution.Result = Constants.Result.QueryExecution.Error;
                queryExecution.Log = log;
                queryExecution.ExecutionEndDate = DateTimeOffset.Now;

                await queryExecutionRepository.UpdateAsync(queryExecution);
                await queryExecutionRepository.SaveAsync();

                throw;
            }

            Logger.Log($"==================================================================================");

            Logger.Reset();
        }
    }
}
