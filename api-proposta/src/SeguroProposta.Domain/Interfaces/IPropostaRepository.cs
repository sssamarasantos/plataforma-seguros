using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.Models;

namespace SeguroProposta.Domain.Interfaces
{
    public interface IPropostaRepository
    {
        Task InserirAsync(Proposta proposta);
        Task<IEnumerable<Proposta>> BuscarTodasAsync();
        Task<Proposta?> BuscarPorNumeroPropostaAsync(int numeroProposta);
        Task AtualizaStatusAsync(int numeroProposta, StatusProposta statusProposta);
    }
}
