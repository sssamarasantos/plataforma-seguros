using System.Data;

namespace SeguroContratacao.Infrastructure.Interfaces
{
    public interface IDbContextFactory
    {
        IDbConnection CreateConnection();
    }
}
