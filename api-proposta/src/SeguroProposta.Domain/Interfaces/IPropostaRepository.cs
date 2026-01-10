using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.Models;

namespace SeguroProposta.Domain.Interfaces
{
    public interface IPropostaRepository
    {
        Task InserirAsync(Proposta proposta);
        Task<IEnumerable<Proposta>> BuscarTodasAsync();
        Task<Proposta?> BuscarPorIdAsync(int id);
        Task AtualizaStatusAsync(int id, StatusProposta statusProposta);
    }
}
