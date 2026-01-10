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
            var proposta = Proposta.Criar("Seguro Residencial", "Cobertura básica", 50.0m, 10000.0m);

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
        public void ToDTO_LancaArgumentNullException_QuandoPropostaNula()
        {
            // Arrange
            Proposta? proposta = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => proposta!.ToDTO());
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
                ValorCobertura = 100000.0m
            };

            // Act
            var proposta = dto.ToDomain();

            // Assert
            Assert.NotNull(proposta);
            Assert.Equal(dto.Titulo, proposta.Titulo);
            Assert.Equal(dto.Descricao, proposta.Descricao);
            Assert.Equal(dto.ValorPremio, proposta.ValorPremio);
            Assert.Equal(dto.ValorCobertura, proposta.ValorCobertura);
            Assert.Equal(StatusProposta.EmAnalise, proposta.Status);
            Assert.False(string.IsNullOrWhiteSpace(proposta.NumeroProposta));
        }

        [Fact]
        public void ToDomain_LancaArgumentNullException_QuandoDTONulo()
        {
            // Arrange
            CriaPropostaDTO? dto = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => dto!.ToDomain());
        }
    }
}