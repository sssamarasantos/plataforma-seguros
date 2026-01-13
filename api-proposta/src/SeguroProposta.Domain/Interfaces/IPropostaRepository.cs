using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.Models;

namespace SeguroProposta.Domain.Interfaces
{
    public interface IPropostaRepository
    {
        Task<bool> InserirAsync(Proposta proposta);
        Task<IEnumerable<Proposta>> BuscarTodasAsync();
        Task<Proposta?> BuscarPorIdAsync(int id);
        Task<bool> AtualizaStatusAsync(int id, StatusProposta statusProposta);
    }
}
