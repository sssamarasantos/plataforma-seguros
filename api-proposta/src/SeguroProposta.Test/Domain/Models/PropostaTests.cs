using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.Exceptions;
using SeguroProposta.Domain.Models;

namespace SeguroProposta.Test.Domain.Models
{
    public class PropostaTests
    {
        [Fact]
        public void Criar_ValidaPropriedades_QuandoParametrosValidos()
        {
            // Arrange
            var titulo = "Titulo Teste";
            var descricao = "Descricao Teste";
            decimal valorPremio = 100.0m;
            decimal valorCobertura = 1000.0m;

            // Act
            var momentoAntes = DateTime.UtcNow;
            var proposta = Proposta.Criar(titulo, descricao, valorPremio, valorCobertura);
            var momentoDepois = DateTime.UtcNow;

            // Assert
            Assert.NotNull(proposta);
            Assert.Equal(titulo, proposta.Titulo);
            Assert.Equal(descricao, proposta.Descricao);
            Assert.Equal(valorPremio, proposta.ValorPremio);
            Assert.Equal(valorCobertura, proposta.ValorCobertura);
            Assert.Equal(StatusProposta.EmAnalise, proposta.Status);
            Assert.False(string.IsNullOrWhiteSpace(proposta.NumeroProposta));
            Assert.Equal(10, proposta.NumeroProposta.Length);
            Assert.Matches("^[A-Z0-9]{10}$", proposta.NumeroProposta);
            Assert.True(proposta.DataHoraInclusao >= momentoAntes && proposta.DataHoraInclusao <= momentoDepois);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Criar_LancaPropostaInvalidaException_QuandoTituloInvalido(string tituloInvalido)
        {
            // Arrange
            var descricao = "Descricao";
            decimal valorPremio = 100.0m;
            decimal valorCobertura = 1000.0m;

            // Act & Assert
            Assert.Throws<PropostaInvalidaException>(
                () => Proposta.Criar(tituloInvalido!, descricao, valorPremio, valorCobertura));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Criar_LancaPropostaInvalidaException_QuandoDescricaoInvalida(string descricaoInvalida)
        {
            // Arrange
            var titulo = "Titulo";
            decimal valorPremio = 100.0m;
            decimal valorCobertura = 1000.0m;

            // Act & Assert
            Assert.Throws<PropostaInvalidaException>(() => Proposta.Criar(titulo, descricaoInvalida!, valorPremio, valorCobertura));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Criar_LancaPropostaInvalidaException_QuandoValorPremioInvalido(decimal valorInvalido)
        {
            // Arrange
            var titulo = "Titulo";
            var descricao = "Descricao";
            decimal valorCobertura = 1000.0m;

            // Act & Assert
            Assert.Throws<PropostaInvalidaException>(
                () => Proposta.Criar(titulo, descricao, valorInvalido, valorCobertura));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Criar_LancaPropostaInvalidaException_QuandoValorCoberturaInvalido(decimal valorInvalido)
        {
            // Arrange
            var titulo = "Titulo";
            var descricao = "Descricao";
            decimal valorPremio = 100.0m;

            // Act & Assert
            Assert.Throws<PropostaInvalidaException>(
                () => Proposta.Criar(titulo, descricao, valorPremio, valorInvalido));
        }

        [Fact]
        public void AlterarStatus_AlteraQuandoDiferente()
        {
            // Arrange
            var proposta = Proposta.Criar("Titulo", "Descricao", 50.0m, 500.0m);

            // Act
            proposta.AlterarStatus(StatusProposta.Aprovada);

            // Assert
            Assert.Equal(StatusProposta.Aprovada, proposta.Status);
        }

        [Fact]
        public void AlterarStatus_LancaRegraDeNegocioException_QuandoMesmoStatus()
        {
            // Arrange
            var proposta = Proposta.Criar("Titulo", "Descricao", 50.0m, 500.0m);

            // Act & Assert
            Assert.Throws<RegraDeNegocioException>(() => proposta.AlterarStatus(proposta.Status));
        }
    }
}
