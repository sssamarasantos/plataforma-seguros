using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Application.Interfaces
{
    public interface IProcessamentoContratacaoService
    {
        Task<ResultadoOperacao> ProcessarContratacaoAsync(Contratacao contratacao);
    }
}