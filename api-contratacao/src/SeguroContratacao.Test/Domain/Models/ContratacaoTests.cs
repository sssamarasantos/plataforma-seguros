using SeguroContratacao.Domain.Enums;
using SeguroContratacao.Domain.Exceptions;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Test.Domain.Models
{
    public class ContratacaoTests
    {
        [Fact]
        public void Criar_ComDadosValidos_DeveCriarContratacaoComSucesso()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 500.00m;
            var valorCobertura = 10000.00m;

            // Act
            var contratacao = Contratacao.Criar(idProposta, valorPremio, valorCobertura);

            // Assert
            Assert.NotNull(contratacao);
            Assert.Equal(idProposta, contratacao.IdProposta);
            Assert.Equal(valorPremio, contratacao.ValorPremioFinal);
            Assert.Equal(valorCobertura, contratacao.ValorCoberturaFinal);
            Assert.NotEmpty(contratacao.NumeroApolice);
            Assert.True(contratacao.DataHoraContratacao <= DateTime.UtcNow);
            Assert.True(contratacao.DataHoraContratacao >= DateTime.UtcNow.AddSeconds(-1));
        }

        [Fact]
        public void Criar_ComIdPropostaZero_DeveLancarContratacaoInvalidaException()
        {
            // Arrange
            var idProposta = 0;
            var valorPremio = 500.00m;
            var valorCobertura = 10000.00m;

            // Act & Assert
            var exception = Assert.Throws<ContratacaoInvalidaException>(() =>
                Contratacao.Criar(idProposta, valorPremio, valorCobertura));

            Assert.Equal("O ID da proposta não pode ser zero.", exception.Message);
        }

        [Fact]
        public void Criar_ComValorPremioZero_DeveLancarContratacaoInvalidaException()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 0m;
            var valorCobertura = 10000.00m;

            // Act & Assert
            var exception = Assert.Throws<ContratacaoInvalidaException>(() =>
                Contratacao.Criar(idProposta, valorPremio, valorCobertura));

            Assert.Equal("Valor do prêmio deve ser maior que zero.", exception.Message);
        }

        [Fact]
        public void Criar_ComValorPremioNegativo_DeveLancarContratacaoInvalidaException()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = -100.00m;
            var valorCobertura = 10000.00m;

            // Act & Assert
            var exception = Assert.Throws<ContratacaoInvalidaException>(() =>
                Contratacao.Criar(idProposta, valorPremio, valorCobertura));

            Assert.Equal("Valor do prêmio deve ser maior que zero.", exception.Message);
        }

        [Fact]
        public void Criar_ComValorCoberturaMenorQueValorPremio_DeveLancarContratacaoInvalidaException()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 10000.00m;
            var valorCobertura = 500.00m;

            // Act & Assert
            var exception = Assert.Throws<ContratacaoInvalidaException>(() =>
                Contratacao.Criar(idProposta, valorPremio, valorCobertura));

            Assert.Equal("Valor da cobertura deve ser maior que o prêmio.", exception.Message);
        }

        [Fact]
        public void Criar_ComValorCoberturaIgualAoValorPremio_DeveLancarContratacaoInvalidaException()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 10000.00m;
            var valorCobertura = 10000.00m;

            // Act & Assert
            var exception = Assert.Throws<ContratacaoInvalidaException>(() =>
                Contratacao.Criar(idProposta, valorPremio, valorCobertura));

            Assert.Equal("Valor da cobertura deve ser maior que o prêmio.", exception.Message);
        }

        [Fact]
        public void ValidarStatus_ComStatusAprovada_NaoDeveLancarExcecao()
        {
            // Arrange
            var status = StatusProposta.Aprovada;

            // Act & Assert
            var exception = Record.Exception(() => Contratacao.ValidarStatus(status));
            Assert.Null(exception);
        }

        [Fact]
        public void ValidarStatus_ComStatusEmAnalise_DeveLancarRegraDeNegocioException()
        {
            // Arrange
            var status = StatusProposta.EmAnalise;

            // Act & Assert
            var exception = Assert.Throws<RegraDeNegocioException>(() =>
                Contratacao.ValidarStatus(status));

            Assert.Equal("A proposta deve estar aprovada para finalizar a contratação.", exception.Message);
        }

        [Fact]
        public void ValidarStatus_ComStatusRecusada_DeveLancarRegraDeNegocioException()
        {
            // Arrange
            var status = StatusProposta.Rejeitada;

            // Act & Assert
            var exception = Assert.Throws<RegraDeNegocioException>(() =>
                Contratacao.ValidarStatus(status));

            Assert.Equal("A proposta deve estar aprovada para finalizar a contratação.", exception.Message);
        }

        [Fact]
        public void Criar_DeveGerarNumeroApoliceUnico()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 500.00m;
            var valorCobertura = 10000.00m;

            // Act
            var contratacao1 = Contratacao.Criar(idProposta, valorPremio, valorCobertura);
            var contratacao2 = Contratacao.Criar(idProposta, valorPremio, valorCobertura);

            // Assert
            Assert.NotEqual(contratacao1.NumeroApolice, contratacao2.NumeroApolice);
        }

        [Fact]
        public void Criar_DeveDefinirDataHoraContratacaoComoUtcNow()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 500.00m;
            var valorCobertura = 10000.00m;
            var dataAntesExecucao = DateTime.UtcNow;

            // Act
            var contratacao = Contratacao.Criar(idProposta, valorPremio, valorCobertura);
            var dataAposExecucao = DateTime.UtcNow;

            // Assert
            Assert.True(contratacao.DataHoraContratacao >= dataAntesExecucao);
            Assert.True(contratacao.DataHoraContratacao <= dataAposExecucao);
        }

        [Theory]
        [InlineData(1, 100.00, 1000.00)]
        [InlineData(999, 50.50, 5000.00)]
        [InlineData(12345, 1500.75, 50000.25)]
        public void Criar_ComDiferentesCombinacoes_DeveCriarContratacaoComSucesso(
            int idProposta, 
            decimal valorPremio, 
            decimal valorCobertura)
        {
            // Act
            var contratacao = Contratacao.Criar(idProposta, valorPremio, valorCobertura);

            // Assert
            Assert.NotNull(contratacao);
            Assert.Equal(idProposta, contratacao.IdProposta);
            Assert.Equal(valorPremio, contratacao.ValorPremioFinal);
            Assert.Equal(valorCobertura, contratacao.ValorCoberturaFinal);
        }
    }
}