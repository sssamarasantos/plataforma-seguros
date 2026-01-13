using SeguroProposta.Application.DTOs;
using SeguroProposta.Domain.Common;
using SeguroProposta.Domain.Models;

namespace SeguroProposta.Application.Mappers
{
    public static class PropostaMapper
    {
        public static ResultadoOperacao<Proposta> ToDomain(this CriaPropostaDTO dto)
        {
            return Proposta.Criar(dto.Titulo, dto.Descricao, dto.ValorPremio, dto.ValorCobertura, dto.EmailContratante);
        }

        public static PropostaDTO ToDTO(this Proposta proposta)
        {
            return new PropostaDTO
            {
                Id = proposta.Id,
                NumeroProposta = proposta.NumeroProposta,
                Status = proposta.Status,
                DataHoraInclusao = proposta.DataHoraInclusao,
                Titulo = proposta.Titulo,
                Descricao = proposta.Descricao,
                ValorPremio = proposta.ValorPremio,
                ValorCobertura = proposta.ValorCobertura,
                EmailContratante = proposta.EmailContratante
            };
        }
    }
}