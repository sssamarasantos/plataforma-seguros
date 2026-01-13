using SeguroProposta.Application.DTOs;
using SeguroProposta.Application.Mappers;
using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.Models;

namespace SeguroProposta.Test.Application.Mappers
{
    public class PropostaMapperTests
    {
        [Fact]
        public void ToDTO_ConverteCorretamente_QuandoPropostaValida()
        {
            // Arrange
            var resultadoProposta = Proposta.Criar("Seguro Residencial", "Cobertura básica", 50.0m, 10000.0m, "teste@email.com");
            var proposta = resultadoProposta.Valor;

            // Act
            var dto = proposta.ToDTO();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(proposta.Id, dto.Id);
            Assert.Equal(proposta.NumeroProposta, dto.NumeroProposta);
            Assert.Equal(proposta.Titulo, dto.Titulo);
            Assert.Equal(proposta.Descricao, dto.Descricao);
            Assert.Equal(proposta.ValorPremio, dto.ValorPremio);
            Assert.Equal(proposta.ValorCobertura, dto.ValorCobertura);
            Assert.Equal(proposta.Status, dto.Status);
            Assert.Equal(proposta.DataHoraInclusao, dto.DataHoraInclusao);
        }

        [Fact]
        public void ToDomain_ConverteCorretamente_QuandoDTOValido()
        {
            // Arrange
            var dto = new CriaPropostaDTO
            {
                Titulo = "Seguro Vida",
                Descricao = "Cobertura total",
                ValorPremio = 200.0m,
                ValorCobertura = 100000.0m,
                EmailContratante = "teste@email.com"
            };

            // Act
            var resultado = dto.ToDomain();

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal(dto.Titulo, resultado.Valor.Titulo);
            Assert.Equal(dto.Descricao, resultado.Valor.Descricao);
            Assert.Equal(dto.ValorPremio, resultado.Valor.ValorPremio);
            Assert.Equal(dto.ValorCobertura, resultado.Valor.ValorCobertura);
            Assert.Equal(StatusProposta.EmAnalise, resultado.Valor.Status);
            Assert.False(string.IsNullOrWhiteSpace(resultado.Valor.NumeroProposta));
        }
    }
}