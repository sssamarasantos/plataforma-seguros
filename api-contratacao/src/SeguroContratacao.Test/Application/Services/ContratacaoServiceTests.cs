using Moq;
using SeguroContratacao.Application.DTOs;
using SeguroContratacao.Application.Services;
using SeguroContratacao.Domain.Enums;
using SeguroContratacao.Domain.Exceptions;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Test.Application.Services
{
    public class ContratacaoServiceTests
    {
        private readonly Mock<IContratacaoRepository> _mockContratacaoRepository;
        private readonly Mock<IPropostaApi> _mockPropostaApi;
        private readonly ContratacaoService _contratacaoService;

        public ContratacaoServiceTests()
        {
            _mockContratacaoRepository = new Mock<IContratacaoRepository>();
            _mockPropostaApi = new Mock<IPropostaApi>();
            _contratacaoService = new ContratacaoService(_mockContratacaoRepository.Object, _mockPropostaApi.Object);
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

            var propostaDto = new Proposta(123, StatusProposta.Aprovada, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(propostaDto);

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(true);

            // Act
            await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            _mockPropostaApi.Verify(x => x.ObterPropostaPorIdAsync(contratacaoDto.IdProposta), Times.Once);
            _mockContratacaoRepository.Verify(x => x.InserirAsync(It.IsAny<Contratacao>()), Times.Once);
        }

        [Fact]
        public async Task ContratarPropostaAsync_ComStatusAprovada_DeveChamarRepositorio()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 456,
                ValorPremioFinal = 300.00m,
                ValorCoberturaFinal = 5000.00m
            };

            var propostaDto = new Proposta(456, StatusProposta.Aprovada, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(propostaDto);

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(true);

            // Act
            await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            _mockContratacaoRepository.Verify(x => x.InserirAsync(It.Is<Contratacao>(c =>
                c.IdProposta == contratacaoDto.IdProposta &&
                c.ValorPremioFinal == contratacaoDto.ValorPremioFinal &&
                c.ValorCoberturaFinal == contratacaoDto.ValorCoberturaFinal &&
                c.EmailContratante == propostaDto.EmailContratante
            )), Times.Once);
        }

        [Fact]
        public async Task ContratarPropostaAsync_ComStatusEmAnalise_DeveLancarRegraDeNegocioException()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 789,
                ValorPremioFinal = 400.00m,
                ValorCoberturaFinal = 8000.00m
            };

            var propostaDto = new Proposta(789, StatusProposta.EmAnalise, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(propostaDto);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<RegraDeNegocioException>(() =>
                _contratacaoService.ContratarPropostaAsync(contratacaoDto));

            Assert.Equal("A proposta deve estar aprovada para finalizar a contratação.", exception.Message);
            _mockContratacaoRepository.Verify(x => x.InserirAsync(It.IsAny<Contratacao>()), Times.Never);
        }

        [Fact]
        public async Task ContratarPropostaAsync_ComStatusRejeitada_DeveLancarRegraDeNegocioException()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 321,
                ValorPremioFinal = 600.00m,
                ValorCoberturaFinal = 12000.00m
            };

            var propostaDto = new Proposta(321, StatusProposta.Rejeitada, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(propostaDto);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<RegraDeNegocioException>(() =>
                _contratacaoService.ContratarPropostaAsync(contratacaoDto));

            Assert.Equal("A proposta deve estar aprovada para finalizar a contratação.", exception.Message);
            _mockContratacaoRepository.Verify(x => x.InserirAsync(It.IsAny<Contratacao>()), Times.Never);
        }

        [Fact]
        public async Task ContratarPropostaAsync_DeveChamarPropostaApiComIdCorreto()
        {
            // Arrange
            var idProposta = 999;
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = idProposta,
                ValorPremioFinal = 750.00m,
                ValorCoberturaFinal = 15000.00m
            };

            var propostaDto = new Proposta(idProposta, StatusProposta.Aprovada, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(idProposta))
                .ReturnsAsync(propostaDto);

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(true);

            // Act
            await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            _mockPropostaApi.Verify(x => x.ObterPropostaPorIdAsync(idProposta), Times.Once);
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

            var propostaDto = new Proposta(idProposta, StatusProposta.Aprovada, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(idProposta))
                .ReturnsAsync(propostaDto);

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(true);

            // Act
            await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            _mockContratacaoRepository.Verify(x => x.InserirAsync(It.IsAny<Contratacao>()), Times.Once);
        }

        [Fact]
        public async Task ContratarPropostaAsync_ComPropostaNaoEncontrada_DeveLancarException()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 999,
                ValorPremioFinal = 500.00m,
                ValorCoberturaFinal = 10000.00m
            };

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(contratacaoDto.IdProposta))
                .ReturnsAsync((Proposta?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() =>
                _contratacaoService.ContratarPropostaAsync(contratacaoDto));

            _mockContratacaoRepository.Verify(x => x.InserirAsync(It.IsAny<Contratacao>()), Times.Never);
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

            var propostaDto = new Proposta(555, StatusProposta.Aprovada, "contratante@email.com");

            Contratacao? contratacaoCapturada = null;

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(propostaDto);

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .Callback<Contratacao>(c => contratacaoCapturada = c)
                .ReturnsAsync(true);

            // Act
            await _contratacaoService.ContratarPropostaAsync(contratacaoDto);

            // Assert
            Assert.NotNull(contratacaoCapturada);
            Assert.Equal(contratacaoDto.IdProposta, contratacaoCapturada.IdProposta);
            Assert.Equal(contratacaoDto.ValorPremioFinal, contratacaoCapturada.ValorPremioFinal);
            Assert.Equal(contratacaoDto.ValorCoberturaFinal, contratacaoCapturada.ValorCoberturaFinal);
            Assert.Equal(propostaDto.EmailContratante, contratacaoCapturada.EmailContratante);
            Assert.NotEmpty(contratacaoCapturada.NumeroApolice);
        }

        [Fact]
        public async Task ContratarPropostaAsync_QuandoRepositorioRetornaFalse_DeveLancarException()
        {
            // Arrange
            var contratacaoDto = new ContratacaoDTO
            {
                IdProposta = 123,
                ValorPremioFinal = 500.00m,
                ValorCoberturaFinal = 10000.00m
            };

            var propostaDto = new Proposta(123, StatusProposta.Aprovada, "contratante@email.com");

            _mockPropostaApi
                .Setup(x => x.ObterPropostaPorIdAsync(contratacaoDto.IdProposta))
                .ReturnsAsync(propostaDto);

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _contratacaoService.ContratarPropostaAsync(contratacaoDto));

            Assert.Equal("Falha ao registrar a contratação.", exception.Message);
            _mockContratacaoRepository.Verify(x => x.InserirAsync(It.IsAny<Contratacao>()), Times.Once);
        }
    }
}