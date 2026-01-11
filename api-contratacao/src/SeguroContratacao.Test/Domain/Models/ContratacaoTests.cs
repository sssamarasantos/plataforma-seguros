using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Enums;
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
            var emailContratante = "contratante@email.com";

            // Act
            var resultado = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal(idProposta, resultado.Valor.IdProposta);
            Assert.Equal(valorPremio, resultado.Valor.ValorPremioFinal);
            Assert.Equal(valorCobertura, resultado.Valor.ValorCoberturaFinal);
            Assert.Equal(emailContratante, resultado.Valor.EmailContratante);
            Assert.NotEmpty(resultado.Valor.NumeroApolice);
            Assert.True(resultado.Valor.DataHoraContratacao <= DateTime.UtcNow);
            Assert.True(resultado.Valor.DataHoraContratacao >= DateTime.UtcNow.AddSeconds(-1));
        }

        [Fact]
        public void Criar_ComIdPropostaZero_DeveRetornarFalha()
        {
            // Arrange
            var idProposta = 0;
            var valorPremio = 500.00m;
            var valorCobertura = 10000.00m;
            var emailContratante = "contratante@email.com";

            // Act
            var resultado = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosDomain.IdPropostaInvalido.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.Validacao, resultado.Erro.TipoErro);
        }

        [Fact]
        public void Criar_ComValorPremioZero_DeveRetornarFalha()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 0m;
            var valorCobertura = 10000.00m;
            var emailContratante = "contratante@email.com";

            // Act
            var resultado = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosDomain.ValorPremioInvalido.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.Validacao, resultado.Erro.TipoErro);
        }

        [Fact]
        public void Criar_ComValorPremioNegativo_DeveRetornarFalha()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = -100.00m;
            var valorCobertura = 10000.00m;
            var emailContratante = "contratante@email.com";

            // Act
            var resultado = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosDomain.ValorPremioInvalido.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.Validacao, resultado.Erro.TipoErro);
        }

        [Fact]
        public void Criar_ComValorCoberturaMenorQueValorPremio_DeveRetornarFalha()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 10000.00m;
            var valorCobertura = 500.00m;
            var emailContratante = "contratante@email.com";

            // Act
            var resultado = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosDomain.ValorCoberturaInvalido.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.Validacao, resultado.Erro.TipoErro);
        }

        [Fact]
        public void Criar_ComValorCoberturaIgualAoValorPremio_DeveRetornarFalha()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 10000.00m;
            var valorCobertura = 10000.00m;
            var emailContratante = "contratante@email.com";

            // Act
            var resultado = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosDomain.ValorCoberturaInvalido.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.Validacao, resultado.Erro.TipoErro);
        }

        [Fact]
        public void ValidarStatus_ComStatusAprovada_DeveRetornarSucesso()
        {
            // Arrange
            var status = StatusProposta.Aprovada;

            // Act
            var resultado = Contratacao.ValidarStatus(status);

            // Assert
            Assert.True(resultado.EhSucesso);
        }

        [Fact]
        public void ValidarStatus_ComStatusEmAnalise_DeveRetornarFalha()
        {
            // Arrange
            var status = StatusProposta.EmAnalise;

            // Act
            var resultado = Contratacao.ValidarStatus(status);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosDomain.TituloObrigatorio.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.RegraDeNegocio, resultado.Erro.TipoErro);
        }

        [Fact]
        public void ValidarStatus_ComStatusRecusada_DeveRetornarFalha()
        {
            // Arrange
            var status = StatusProposta.Rejeitada;

            // Act
            var resultado = Contratacao.ValidarStatus(status);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosDomain.TituloObrigatorio.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.RegraDeNegocio, resultado.Erro.TipoErro);
        }

        [Fact]
        public void Criar_DeveGerarNumeroApoliceUnico()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 500.00m;
            var valorCobertura = 10000.00m;
            var emailContratante = "contratante@email.com";

            // Act
            var resultado1 = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);
            var resultado2 = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);

            // Assert
            Assert.True(resultado1.EhSucesso);
            Assert.True(resultado2.EhSucesso);
            Assert.NotEqual(resultado1.Valor.NumeroApolice, resultado2.Valor.NumeroApolice);
        }

        [Fact]
        public void Criar_DeveDefinirDataHoraContratacaoComoUtcNow()
        {
            // Arrange
            var idProposta = 123;
            var valorPremio = 500.00m;
            var valorCobertura = 10000.00m;
            var emailContratante = "contratante@email.com";
            var dataAntesExecucao = DateTime.UtcNow;

            // Act
            var resultado = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);
            var dataAposExecucao = DateTime.UtcNow;

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.True(resultado.Valor.DataHoraContratacao >= dataAntesExecucao);
            Assert.True(resultado.Valor.DataHoraContratacao <= dataAposExecucao);
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
            // Arrange
            var emailContratante = "contratante@email.com";

            // Act
            var resultado = Contratacao.Criar(idProposta, valorPremio, valorCobertura, emailContratante);

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal(idProposta, resultado.Valor.IdProposta);
            Assert.Equal(valorPremio, resultado.Valor.ValorPremioFinal);
            Assert.Equal(valorCobertura, resultado.Valor.ValorCoberturaFinal);
            Assert.Equal(emailContratante, resultado.Valor.EmailContratante);
        }
    }
}