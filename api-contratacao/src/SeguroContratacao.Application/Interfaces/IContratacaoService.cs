using SeguroContratacao.Application.DTOs;

namespace SeguroContratacao.Application.Interfaces
{
    public interface IContratacaoService
    {
        Task<int> ContratarPropostaAsync(ContratacaoDTO contratacaoDto);
    }
}
