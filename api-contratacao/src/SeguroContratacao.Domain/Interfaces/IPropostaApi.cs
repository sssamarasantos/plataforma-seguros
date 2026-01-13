using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Domain.Interfaces
{
    public interface IPropostaApi
    {
        Task<ResultadoOperacao<Proposta>> ObterPropostaPorIdAsync(int idProposta);
    }
}
