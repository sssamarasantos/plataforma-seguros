using Moq;
using SeguroContratacao.Application.Common;
using SeguroContratacao.Application.Services;
using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Test.Application.Services
{
    public class ProcessamentoContratacaoServiceTests
    {
        private readonly Mock<IContratacaoRepository> _mockContratacaoRepository;
        private readonly Mock<INotificacaoService> _mockNotificacaoService;
        private readonly ProcessamentoContratacaoService _processamentoContratacaoService;

        public ProcessamentoContratacaoServiceTests()
        {
            _mockContratacaoRepository = new Mock<IContratacaoRepository>();
            _mockNotificacaoService = new Mock<INotificacaoService>();
            _processamentoContratacaoService = new ProcessamentoContratacaoService(
                _mockContratacaoRepository.Object,
                _mockNotificacaoService.Object);
        }

        [Fact]
        public async Task ProcessarContratacaoAsync_ComSucesso_DevePersistirENotificar()
        {
            // Arrange
            var resultadoContratacao = Contratacao.Criar(
                123,
                500.00m,
                10000.00m,
                "contratante@email.com"
            );
            var contratacao = resultadoContratacao.Valor;

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _processamentoContratacaoService.ProcessarContratacaoAsync(contratacao);

            // Assert
            Assert.True(resultado.EhSucesso);
            _mockContratacaoRepository.Verify(x => x.InserirAsync(contratacao), Times.Once);
            _mockNotificacaoService.Verify(x => x.NotificarContratacaoEfetivadaAsync(
                contratacao.Id,
                contratacao.EmailContratante,
                contratacao.NumeroApolice), Times.Once);
        }

        [Fact]
        public async Task ProcessarContratacaoAsync_QuandoRepositorioFalha_DeveRetornarFalhaSemNotificar()
        {
            // Arrange
            var resultadoContratacao = Contratacao.Criar(
                456,
                300.00m,
                5000.00m,
                "contratante@email.com"
            );
            var contratacao = resultadoContratacao.Valor;

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await _processamentoContratacaoService.ProcessarContratacaoAsync(contratacao);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Equal(ErrosApplication.ResultadoContratacaoFalhou.Mensagem, resultado.Erro!.Mensagem);
            Assert.Equal(TipoErro.ErroOperacional, resultado.Erro.TipoErro);
            _mockContratacaoRepository.Verify(x => x.InserirAsync(contratacao), Times.Once);
            _mockNotificacaoService.Verify(x => x.NotificarContratacaoEfetivadaAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ProcessarContratacaoAsync_DeveChamarRepositorioComContratacaoCorreta()
        {
            // Arrange
            var resultadoContratacao = Contratacao.Criar(
                789,
                750.00m,
                15000.00m,
                "teste@email.com"
            );
            var contratacao = resultadoContratacao.Valor;
            Contratacao? contratacaoCapturada = null;

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .Callback<Contratacao>(c => contratacaoCapturada = c)
                .ReturnsAsync(true);

            // Act
            var resultado = await _processamentoContratacaoService.ProcessarContratacaoAsync(contratacao);

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.NotNull(contratacaoCapturada);
            Assert.Equal(contratacao.Id, contratacaoCapturada.Id);
            Assert.Equal(contratacao.IdProposta, contratacaoCapturada.IdProposta);
            Assert.Equal(contratacao.ValorPremioFinal, contratacaoCapturada.ValorPremioFinal);
            Assert.Equal(contratacao.ValorCoberturaFinal, contratacaoCapturada.ValorCoberturaFinal);
            Assert.Equal(contratacao.EmailContratante, contratacaoCapturada.EmailContratante);
        }

        [Fact]
        public async Task ProcessarContratacaoAsync_DeveChamarNotificacaoComDadosCorretos()
        {
            // Arrange
            var resultadoContratacao = Contratacao.Criar(
                999,
                1000.00m,
                20000.00m,
                "notificacao@email.com"
            );
            var contratacao = resultadoContratacao.Valor;

            int idCapturado = 0;
            string emailCapturado = string.Empty;
            string numeroApoliceCapturado = string.Empty;

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(true);

            _mockNotificacaoService
                .Setup(x => x.NotificarContratacaoEfetivadaAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Callback<int, string, string>((id, email, numeroApolice) =>
                {
                    idCapturado = id;
                    emailCapturado = email;
                    numeroApoliceCapturado = numeroApolice;
                })
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _processamentoContratacaoService.ProcessarContratacaoAsync(contratacao);

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.Equal(contratacao.Id, idCapturado);
            Assert.Equal(contratacao.EmailContratante, emailCapturado);
            Assert.Equal(contratacao.NumeroApolice, numeroApoliceCapturado);
        }

        [Theory]
        [InlineData(100, 200.00, 5000.00, "teste1@email.com")]
        [InlineData(200, 350.50, 7500.00, "teste2@email.com")]
        [InlineData(300, 1000.75, 25000.00, "teste3@email.com")]
        public async Task ProcessarContratacaoAsync_ComDiferentesContratacoes_DeveProcessarComSucesso(
            int idProposta,
            decimal valorPremio,
            decimal valorCobertura,
            string email)
        {
            // Arrange
            var resultadoContratacao = Contratacao.Criar(
                idProposta,
                valorPremio,
                valorCobertura,
                email
            );
            var contratacao = resultadoContratacao.Valor;

            _mockContratacaoRepository
                .Setup(x => x.InserirAsync(It.IsAny<Contratacao>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _processamentoContratacaoService.ProcessarContratacaoAsync(contratacao);

            // Assert
            Assert.True(resultado.EhSucesso);
            _mockContratacaoRepository.Verify(x => x.InserirAsync(It.IsAny<Contratacao>()), Times.Once);
            _mockNotificacaoService.Verify(x => x.NotificarContratacaoEfetivadaAsync(
                It.IsAny<int>(),
                email,
                It.IsAny<string>()), Times.Once);
        }
    }
}