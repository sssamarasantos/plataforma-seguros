using System.Data;

namespace SeguroProposta.Infraestructure.Interfaces
{
    public interface IDbContextFactory
    {
        IDbConnection CreateConnection();
    }
}
