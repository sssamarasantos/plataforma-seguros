using SeguroProposta.Application.DTOs;
using SeguroProposta.Domain.Models;

namespace SeguroProposta.Application.Mappers
{
    public static class PropostaMapper
    {
        public static Proposta ToDomain(this CriaPropostaDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            // Use o método de fábrica público estático Proposta.Criar em vez do construtor inacessível
            return Proposta.Criar(dto.Titulo, dto.Descricao, dto.ValorPremio, dto.ValorCobertura);
        }

        public static PropostaDTO ToDTO(this Proposta proposta)
        {
            ArgumentNullException.ThrowIfNull(proposta);

            return new PropostaDTO
            {
                Id = proposta.Id,
                NumeroProposta = proposta.NumeroProposta,
                Status = proposta.Status,
                DataHoraInclusao = proposta.DataHoraInclusao,
                Titulo = proposta.Titulo,
                Descricao = proposta.Descricao,
                ValorPremio = proposta.ValorPremio,
                ValorCobertura = proposta.ValorCobertura
            };
        }
    }
}