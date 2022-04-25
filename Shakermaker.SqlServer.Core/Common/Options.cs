using CommandLine;

namespace Shakermaker.SqlServer.Core.Common
{
    public class Options
    {
        [Option('a', "application", Required = false, HelpText = "The application identifier for the deployment, empty parameter is taken as \"Main\".")]
        public string Application { get; set; }

        [Option('r', "release", Required = true, HelpText = "The release identifier for the deployment.")]
        public string Release { get; set; }

        [Option('e', "environment", Required = true, HelpText = "Deployment environment or stage e.g. dev | qa | prod.")]
        public string Environment { get; set; }

        [Option('s', "source-directory", Required = false, HelpText = "The folder where the sql directory structure is located")]
        public string SourceDirectory { get; set; }

        [Option('c', "connection-string", Required = true, HelpText = "The SQL Server connection string where the changes are going to be deployed.")]
        public string ConnectionString { get; set; }
    }
}
