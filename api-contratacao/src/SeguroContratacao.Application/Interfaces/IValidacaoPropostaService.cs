using SeguroContratacao.Domain.Common;
using SeguroContratacao.Infrastructure.DTOs;

namespace SeguroContratacao.Application.Interfaces
{
    public interface IValidacaoPropostaService
    {
        Task<ResultadoOperacao<PropostaDTO>> ValidarPropostaParaContratacaoAsync(int idProposta);
    }
}