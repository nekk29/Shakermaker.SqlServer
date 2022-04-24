using Shakermaker.SqlServer.Core;

namespace Shakermaker.SqlServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new DatabaseMigrator().ExecuteMigration(args).Wait();
        }
    }
}
