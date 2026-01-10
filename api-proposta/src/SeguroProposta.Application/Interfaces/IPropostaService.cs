using SeguroProposta.Application.Dtos;
using SeguroProposta.Application.DTOs;

namespace SeguroProposta.Application.Interfaces
{
    public interface IPropostaService
    {
        Task InserirAsync(CriaPropostaDTO proposta);
        Task<IEnumerable<PropostaDTO>> BuscarTodasAsync();
        Task<PropostaDTO?> BuscarPorIdAsync(int id);
        Task AlterarStatusAsync(AlteraStatusDTO alteraStatusDto);
    }
}
