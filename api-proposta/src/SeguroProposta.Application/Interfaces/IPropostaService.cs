using SeguroProposta.Application.Dtos;
using SeguroProposta.Application.VOs;

namespace SeguroProposta.Application.Interfaces
{
    public interface IPropostaService
    {
        Task InserirAsync(PropostaDTO proposta);
        Task<IEnumerable<PropostaVO>> BuscarTodasAsync();
        Task<PropostaVO?> BuscarPorIdAsync(int id);
        Task AlterarStatusAsync(AlteraStatusDTO alteraStatusDto);
    }
}
