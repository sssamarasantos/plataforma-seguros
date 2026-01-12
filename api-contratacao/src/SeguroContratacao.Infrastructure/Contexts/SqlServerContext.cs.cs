using Microsoft.Data.SqlClient;
using SeguroContratacao.Infrastructure.Interfaces;
using System.Data;
using System.Data.Common;

namespace SeguroContratacao.Infrastructure.Contexts
{
    public class SqlServerContext : IDbContextFactory
    {
        private readonly string _connectionString;

        public SqlServerContext(DbConnectionStringBuilder dbConnectionStringBuilder)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionStringBuilder);

            _connectionString = dbConnectionStringBuilder.ConnectionString
                ?? throw new ArgumentException("ConnectionString cannot be null.", nameof(dbConnectionStringBuilder));
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection; // o chamador deve dar Dispose()
        }
    }
}
