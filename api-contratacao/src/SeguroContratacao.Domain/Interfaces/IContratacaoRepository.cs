using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Domain.Interfaces
{
    public interface IContratacaoRepository
    {
        Task<bool> InserirAsync(Contratacao contratacao);
    }
}
