using SeguroContratacao.Application.DTOs;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Application.Mappers
{
    public static class ContratacaoMapper
    {
        public static Contratacao ToDomain(this ContratacaoDTO contratacaoDto)
        {
            ArgumentNullException.ThrowIfNull(contratacaoDto, nameof(contratacaoDto));

            return new Contratacao(
                contratacaoDto.IdProposta,
                contratacaoDto.ValorPremioFinal,
                contratacaoDto.ValorCoberturaFinal);
        }
    }
}
