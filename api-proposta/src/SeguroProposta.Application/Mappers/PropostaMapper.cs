using SeguroProposta.Application.Dtos;
using SeguroProposta.Application.VOs;
using SeguroProposta.Domain.Models;

namespace SeguroProposta.Application.Mappers
{
    public static class PropostaMapper
    {
        public static Proposta ToDomain(this PropostaDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new Proposta(dto.Titulo, dto.Descricao, dto.ValorPremio, dto.ValorCobertura);
        }

        public static PropostaVO ToVO(this Proposta proposta)
        {
            ArgumentNullException.ThrowIfNull(proposta);

            return new PropostaVO
            {
                Id = proposta.Id,
                NumeroProposta = proposta.NumeroProposta,
                Status = proposta.Status,
                DataInclusao = proposta.DataInclusao,
                Titulo = proposta.Titulo,
                Descricao = proposta.Descricao,
                ValorPremio = proposta.ValorPremio,
                ValorCobertura = proposta.ValorCobertura
            };
        }
    }
}