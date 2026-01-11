using SeguroContratacao.Application.DTOs;

namespace SeguroContratacao.Application.Interfaces
{
    public interface IContratacaoService
    {
        Task ContratarPropostaAsync(ContratacaoDTO contratacaoDto);
    }
}
