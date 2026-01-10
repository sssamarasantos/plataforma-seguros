using SeguroContratacao.Domain.DTOs;

namespace SeguroContratacao.Domain.Interfaces
{
    public interface IPropostaApi
    {
        Task<PropostaDTO> ObterPropostaPorIdAsync(int idProposta);
    }
}
