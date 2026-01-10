using System.Data;

namespace SeguroContratacao.Infraestructure.Interfaces
{
    public interface IDbContextFactory
    {
        IDbConnection CreateConnection();
    }
}
