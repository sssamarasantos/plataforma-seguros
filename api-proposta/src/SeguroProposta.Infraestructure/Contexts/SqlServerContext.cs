using Microsoft.Data.SqlClient;
using SeguroProposta.Infraestructure.Interfaces;
using System.Data;
using System.Data.Common;

namespace SeguroProposta.Infraestructure.Contexts
{
    public sealed class SqlServerContext : IDbContextFactory
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
