using Moq;
using SeguroContratacao.Application.Services;
using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Enums;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.Models;
using SeguroContratacao.Infrastructure.Common;

namespace SeguroContratacao.Test.Application.Services
{
    public class ValidacaoPropostaServiceTests
    {
        private readonly Mock<IPropostaApi> _mockPropostaApi;
        private readonly ValidacaoPropostaService _validacaoPropostaService;

        public ValidacaoPropostaServiceTests()
        {
            _mockPropostaApi = new Mock<IPropostaApi>();
            _validacaoPropostaService = new ValidacaoPropostaService(_mockPropostaApi.Object);
        }

        [Fact]
        public async Task ValidarPropostaParaContratacaoAsync_ComPropostaAprovada_DeveRetornarSucesso()
        {
            // Arrange
            var idProposta = 123;
            var proposta = new Proposta(idProposta, StatusProposta.Aprovada, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(idProposta))
                .ReturnsAsync(ResultadoOperacao.Sucesso(proposta));

            // Act
            var resultado = await _validacaoPropostaService.ValidarPropostaParaContratacaoAsync(idProposta);

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.Equal(proposta.Id, resultado.Valor.Id);
            Assert.Equal(proposta.EmailContratante, resultado.Valor.EmailContratante);
            _mockPropostaApi.Verify(x => x.ObterPropostaPorIdAsync(idProposta), Times.Once);
        }

        [Fact]
        public async Task ValidarPropostaParaContratacaoAsync_ComPropostaEmAnalise_DeveRetornarFalha()
        {
            // Arrange
            var idProposta = 456;
            var proposta = new Proposta(idProposta, StatusProposta.EmAnalise, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(idProposta))
                .ReturnsAsync(ResultadoOperacao.Sucesso(proposta));

            // Act
            var resultado = await _validacaoPropostaService.ValidarPropostaParaContratacaoAsync(idProposta);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(TipoErro.RegraDeNegocio, resultado.Erro!.TipoErro);
            _mockPropostaApi.Verify(x => x.ObterPropostaPorIdAsync(idProposta), Times.Once);
        }

        [Fact]
        public async Task ValidarPropostaParaContratacaoAsync_ComPropostaRejeitada_DeveRetornarFalha()
        {
            // Arrange
            var idProposta = 789;
            var proposta = new Proposta(idProposta, StatusProposta.Rejeitada, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(idProposta))
                .ReturnsAsync(ResultadoOperacao.Sucesso(proposta));

            // Act
            var resultado = await _validacaoPropostaService.ValidarPropostaParaContratacaoAsync(idProposta);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(TipoErro.RegraDeNegocio, resultado.Erro!.TipoErro);
            _mockPropostaApi.Verify(x => x.ObterPropostaPorIdAsync(idProposta), Times.Once);
        }

        [Fact]
        public async Task ValidarPropostaParaContratacaoAsync_ComPropostaNaoEncontrada_DeveRetornarFalha()
        {
            // Arrange
            var idProposta = 999;

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(idProposta))
                .ReturnsAsync(ResultadoOperacao.Falha<Proposta>(ErrosInfrastructure.PropostaNaoEncontrada));

            // Act
            var resultado = await _validacaoPropostaService.ValidarPropostaParaContratacaoAsync(idProposta);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosInfrastructure.PropostaNaoEncontrada.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.NaoEncontrado, resultado.Erro.TipoErro);
            _mockPropostaApi.Verify(x => x.ObterPropostaPorIdAsync(idProposta), Times.Once);
        }

        [Fact]
        public async Task ValidarPropostaParaContratacaoAsync_ComErroNaApi_DeveRetornarFalha()
        {
            // Arrange
            var idProposta = 111;

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(idProposta))
                .ReturnsAsync(ResultadoOperacao.Falha<Proposta>(ErrosInfrastructure.ErroAoComunicarComApiProposta));

            // Act
            var resultado = await _validacaoPropostaService.ValidarPropostaParaContratacaoAsync(idProposta);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosInfrastructure.ErroAoComunicarComApiProposta.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.ErroOperacional, resultado.Erro.TipoErro);
            _mockPropostaApi.Verify(x => x.ObterPropostaPorIdAsync(idProposta), Times.Once);
        }
    }
}
