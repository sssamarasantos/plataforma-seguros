using System.Data;

namespace SeguroProposta.Infrastructure.Interfaces
{
    public interface IDbContextFactory
    {
        IDbConnection CreateConnection();
    }
}
