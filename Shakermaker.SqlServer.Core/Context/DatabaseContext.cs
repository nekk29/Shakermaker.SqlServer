using Microsoft.EntityFrameworkCore;
using Shakermaker.SqlServer.Core.Common;
using Shakermaker.SqlServer.Core.Entity;

namespace Shakermaker.SqlServer.Core.Context
{
    public class DatabaseContext : DbContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(Constants.Schema.MainSchema);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<QueryExecution> QueryExecution { get; set; }
        public DbSet<QueryFileHistory> QueryFileHistory { get; set; }
    }
}
