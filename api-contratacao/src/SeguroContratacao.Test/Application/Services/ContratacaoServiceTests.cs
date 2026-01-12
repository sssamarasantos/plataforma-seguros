using Moq;
using SeguroContratacao.Application.DTOs;
using SeguroContratacao.Application.Interfaces;
using SeguroContratacao.Application.Services;
using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Enums;
using SeguroContratacao.Domain.Models;
using SeguroContratacao.Infrastructure.DTOs;

namespace SeguroContratacao.Test.Application.Services
{
    public class ContratacaoServiceTests
    {
        private readonly Mock<IValidacaoPropostaService> _mockValidacaoPropostaService;
        private readonly Mock<IProcessamentoContratacaoService> _mockProcessamentoContratacaoService;
        private readonly ContratacaoService _contratacaoService;

        public ContratacaoServiceTests()
        {
            _mockValidacaoPropostaService = new Mock<IValidacaoPropostaService>();
            _mockProcessamentoContratacaoService = new Mock<IProcessamentoContratacaoService>();
            _contratacaoService = new ContratacaoService(
                _mockValidacaoPropostaService.Object,
                _mockProcessamentoContratacaoService.Object);
        }

        [Fact]
        public async Task ContratarPropostaAsync_ComDadosValidos_DeveContratarComSucesso()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 123,
                ValorPremioFinal = 500.00m,
                ValorCoberturaFinal = 10000.00m
            };

            var propostaDto = new PropostaDTO
            {
                Id = 123,
                Status = StatusProposta.Aprovada,
                EmailContratante = "contratante@email.com"
            };

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(ResultadoOperacao.Sucesso(propostaDto));

            _mockProcessamentoContratacaoService
                .Setup(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(ResultadoOperacao.Sucesso());

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhSucesso);
            _mockValidacaoPropostaService.Verify(x => x.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta), Times.Once);
            _mockProcessamentoContratacaoService.Verify(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()), Times.Once);
        }

        [Fact]
        public async Task ContratarPropostaAsync_ComStatusAprovada_DeveChamarProcessamento()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 456,
                ValorPremioFinal = 300.00m,
                ValorCoberturaFinal = 5000.00m
            };

            var propostaDto = new PropostaDTO
            {
                Id = 456,
                Status = StatusProposta.Aprovada,
                EmailContratante = "contratante@email.com"
            };

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(ResultadoOperacao.Sucesso(propostaDto));

            _mockProcessamentoContratacaoService
                .Setup(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(ResultadoOperacao.Sucesso());

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhSucesso);
            _mockProcessamentoContratacaoService.Verify(x => x.ProcessarContratacaoAsync(It.Is<Contratacao>(c =>
                c.IdProposta == contratacaoDto.IdProposta &&
                c.ValorPremioFinal == contratacaoDto.ValorPremioFinal &&
                c.ValorCoberturaFinal == contratacaoDto.ValorCoberturaFinal &&
                c.EmailContratante == propostaDto.EmailContratante
            )), Times.Once);
        }

        [Fact]
        public async Task ContratarPropostaAsync_ComStatusEmAnalise_DeveRetornarFalha()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 789,
                ValorPremioFinal = 400.00m,
                ValorCoberturaFinal = 8000.00m
            };

            var erro = new Erro(TipoErro.RegraDeNegocio, "Status inválido");

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(ResultadoOperacao.Falha<PropostaDTO>(erro));

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(erro.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.RegraDeNegocio, resultado.Erro.TipoErro);
            _mockProcessamentoContratacaoService.Verify(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()), Times.Never);
        }

        [Fact]
        public async Task ContratarPropostaAsync_ComStatusRejeitada_DeveRetornarFalha()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 321,
                ValorPremioFinal = 600.00m,
                ValorCoberturaFinal = 12000.00m
            };

            var erro = new Erro(TipoErro.RegraDeNegocio, "Status inválido");

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(ResultadoOperacao.Falha<PropostaDTO>(erro));

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(erro.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.RegraDeNegocio, resultado.Erro.TipoErro);
            _mockProcessamentoContratacaoService.Verify(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()), Times.Never);
        }

        [Fact]
        public async Task ContratarPropostaAsync_DeveChamarValidacaoComIdCorreto()
        {
            // Arrange
            var idProposta = 999;
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = idProposta,
                ValorPremioFinal = 750.00m,
                ValorCoberturaFinal = 15000.00m
            };

            var propostaDto = new PropostaDTO
            {
                Id = idProposta,
                Status = StatusProposta.Aprovada,
                EmailContratante = "contratante@email.com"
            };

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(idProposta))
                .ReturnsAsync(ResultadoOperacao.Sucesso(propostaDto));

            _mockProcessamentoContratacaoService
                .Setup(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(ResultadoOperacao.Sucesso());

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhSucesso);
            _mockValidacaoPropostaService.Verify(x => x.ValidarPropostaParaContratacaoAsync(idProposta), Times.Once);
        }

        [Theory]
        [InlineData(100, 200.00, 5000.00)]
        [InlineData(200, 350.50, 7500.00)]
        [InlineData(300, 1000.75, 25000.00)]
        public async Task ContratarPropostaAsync_ComDiferentesValores_DeveContratarComSucesso(
            int idProposta,
            decimal valorPremio,
            decimal valorCobertura)
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = idProposta,
                ValorPremioFinal = valorPremio,
                ValorCoberturaFinal = valorCobertura
            };

            var propostaDto = new PropostaDTO
            {
                Id = idProposta,
                Status = StatusProposta.Aprovada,
                EmailContratante = "contratante@email.com"
            };

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(idProposta))
                .ReturnsAsync(ResultadoOperacao.Sucesso(propostaDto));

            _mockProcessamentoContratacaoService
                .Setup(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(ResultadoOperacao.Sucesso());

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhSucesso);
            _mockProcessamentoContratacaoService.Verify(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()), Times.Once);
        }

        [Fact]
        public async Task ContratarPropostaAsync_ComPropostaNaoEncontrada_DeveRetornarFalha()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 999,
                ValorPremioFinal = 500.00m,
                ValorCoberturaFinal = 10000.00m
            };

            var erro = new Erro(TipoErro.NaoEncontrado, "Proposta não encontrada");

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(ResultadoOperacao.Falha<PropostaDTO>(erro));

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(erro.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.NaoEncontrado, resultado.Erro.TipoErro);
            _mockProcessamentoContratacaoService.Verify(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()), Times.Never);
        }

        [Fact]
        public async Task ContratarPropostaAsync_DevePassarValoresCorretosParaCriarContratacao()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 555,
                ValorPremioFinal = 450.25m,
                ValorCoberturaFinal = 9500.50m
            };

            var propostaDto = new PropostaDTO
            {
                Id = 555,
                Status = StatusProposta.Aprovada,
                EmailContratante = "contratante@email.com"
            };

            Contratacao? contratacaoCapturada = null;

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(ResultadoOperacao.Sucesso(propostaDto));

            _mockProcessamentoContratacaoService
                .Setup(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()))
                .Callback<Contratacao>(c => contratacaoCapturada = c)
                .ReturnsAsync(ResultadoOperacao.Sucesso());

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.NotNull(contratacaoCapturada);
            Assert.Equal(contratacaoDto.IdProposta, contratacaoCapturada.IdProposta);
            Assert.Equal(contratacaoDto.ValorPremioFinal, contratacaoCapturada.ValorPremioFinal);
            Assert.Equal(contratacaoDto.ValorCoberturaFinal, contratacaoCapturada.ValorCoberturaFinal);
            Assert.Equal(propostaDto.EmailContratante, contratacaoCapturada.EmailContratante);
            Assert.NotEmpty(contratacaoCapturada.NumeroApolice);
        }

        [Fact]
        public async Task ContratarPropostaAsync_QuandoProcessamentoFalha_DeveRetornarFalha()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 123,
                ValorPremioFinal = 500.00m,
                ValorCoberturaFinal = 10000.00m
            };

            var propostaDto = new PropostaDTO
            {
                Id = 123,
                Status = StatusProposta.Aprovada,
                EmailContratante = "contratante@email.com"
            };

            var erro = new Erro(TipoErro.ErroOperacional, "Erro ao processar contratação");

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(ResultadoOperacao.Sucesso(propostaDto));

            _mockProcessamentoContratacaoService
                .Setup(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(ResultadoOperacao.Falha(erro));

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(erro.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.ErroOperacional, resultado.Erro.TipoErro);
            _mockProcessamentoContratacaoService.Verify(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()), Times.Once);
        }

        [Fact]
        public async Task ContratarPropostaAsync_QuandoValidacaoFalha_DeveRetornarFalha()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 123,
                ValorPremioFinal = 500.00m,
                ValorCoberturaFinal = 10000.00m
            };

            var erro = new Erro(TipoErro.ErroOperacional, "Erro ao validar proposta");

            _mockValidacaoPropostaService
                .Setup(x => x.ValidarPropostaParaContratacaoAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(ResultadoOperacao.Falha<PropostaDTO>(erro));

            // Act
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(erro.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.ErroOperacional, resultado.Erro.TipoErro);
            _mockProcessamentoContratacaoService.Verify(x => x.ProcessarContratacaoAsync(It.IsAny<Contratacao>()), Times.Never);
        }
    }
}