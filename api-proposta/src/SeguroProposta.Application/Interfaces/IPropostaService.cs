using SeguroProposta.Application.Dtos;
using SeguroProposta.Application.DTOs;
using SeguroProposta.Domain.Common;

namespace SeguroProposta.Application.Interfaces
{
    public interface IPropostaService
    {
        Task<ResultadoOperacao> InserirAsync(CriaPropostaDTO proposta);
        Task<IEnumerable<PropostaDTO>> BuscarTodasAsync();
        Task<PropostaDTO?> BuscarPorIdAsync(int id);
        Task<ResultadoOperacao> AlterarStatusAsync(AlteraStatusDTO alteraStatusDto);
    }
}
