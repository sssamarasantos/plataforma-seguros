using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Domain.Interfaces
{
    public interface IContratacaoRepository
    {
        Task<int> InserirAsync(Contratacao contratacao);
    }
}
