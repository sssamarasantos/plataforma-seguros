using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Domain.Interfaces
{
    public interface IPropostaApi
    {
        Task<Proposta> ObterPropostaPorIdAsync(int idProposta);
    }
}
