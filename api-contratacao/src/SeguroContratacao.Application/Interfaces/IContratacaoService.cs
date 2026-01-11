using SeguroContratacao.Application.DTOs;
using SeguroContratacao.Domain.Common;

namespace SeguroContratacao.Application.Interfaces
{
    public interface IContratacaoService
    {
        Task<ResultadoOperacao> ContratarPropostaAsync(ContratacaoDTO contratacaoDto);
    }
}
