using Moq;
using SeguroProposta.Application.Dtos;
using SeguroProposta.Application.DTOs;
using SeguroProposta.Application.Services;
using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.Interfaces;
using SeguroProposta.Domain.Models;

namespace SeguroProposta.Test.Application.Services
{
    public class PropostaServiceTests
    {
        private readonly Mock<IPropostaRepository> _mockRepository;
        private readonly PropostaService _service;

        public PropostaServiceTests()
        {
            _mockRepository = new Mock<IPropostaRepository>();
            _service = new PropostaService(_mockRepository.Object);
        }

        [Fact]
        public async Task InserirAsync_AdicionaProposta_QuandoDadosValidos()
        {
            // Arrange
            var dto = new CriaPropostaDTO
            {
                Titulo = "Seguro Auto",
                Descricao = "Cobertura completa",
                ValorPremio = 150.0m,
                ValorCobertura = 50000.0m,
                EmailContratante = "teste@email.com"
            };

            _mockRepository.Setup(r => r.InserirAsync(It.IsAny<Proposta>()))
                .ReturnsAsync(true);

            // Act
            await _service.InserirAsync(dto);

            // Assert
            _mockRepository.Verify(
                r => r.InserirAsync(It.Is<Proposta>(p =>
                    p.Titulo == dto.Titulo &&
                    p.ValorPremio == dto.ValorPremio)),
                Times.Once);
        }

        [Fact]
        public async Task InserirAsync_LancaArgumentNullException_QuandoDtoNulo()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _service.InserirAsync(null!));

            _mockRepository.Verify(
                r => r.InserirAsync(It.IsAny<Proposta>()),
                Times.Never);
        }

        [Fact]
        public async Task InserirAsync_RetornaFalha_QuandoInsercaoFalha()
        {
            // Arrange
            var dto = new CriaPropostaDTO
            {
                Titulo = "Seguro Auto",
                Descricao = "Cobertura completa",
                ValorPremio = 150.0m,
                ValorCobertura = 50000.0m,
                EmailContratante = "teste@email.com"
            };

            _mockRepository.Setup(r => r.InserirAsync(It.IsAny<Proposta>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await _service.InserirAsync(dto);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Contains("Erro ao inserir a proposta", resultado.Erro!.Mensagem);
            _mockRepository.Verify(
                r => r.InserirAsync(It.IsAny<Proposta>()),
                Times.Once);
        }

        [Fact]
        public async Task BuscarTodasAsync_RetornaListaVazia_QuandoNaoExistemPropostas()
        {
            // Arrange
            _mockRepository.Setup(r => r.BuscarTodasAsync())
                .ReturnsAsync(new List<Proposta>());

            // Act
            var resultado = await _service.BuscarTodasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task BuscarTodasAsync_RetornaPropostas_QuandoExistemDados()
        {
            // Arrange
            var resultadoProposta1 = Proposta.Criar("Titulo 1", "Descricao 1", 100.0m, 1000.0m, "teste1@email.com");
            var resultadoProposta2 = Proposta.Criar("Titulo 2", "Descricao 2", 200.0m, 2000.0m, "teste2@email.com");
            
            var propostas = new List<Proposta> { resultadoProposta1.Valor, resultadoProposta2.Valor };

            _mockRepository.Setup(r => r.BuscarTodasAsync())
                .ReturnsAsync(propostas);

            // Act
            var resultado = await _service.BuscarTodasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, p => p.Titulo == "Titulo 1");
        }

        [Fact]
        public async Task BuscarPorIdAsync_RetornaNull_QuandoPropostaNaoExiste()
        {
            // Arrange
            _mockRepository.Setup(r => r.BuscarPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Proposta?)null);

            // Act
            var resultado = await _service.BuscarPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task BuscarPorIdAsync_RetornaPropostaDTO_QuandoPropostaExiste()
        {
            // Arrange
            var resultadoProposta = Proposta.Criar("Titulo", "Descricao", 100.0m, 1000.0m, "teste@email.com");
            var proposta = resultadoProposta.Valor;
            var id = 1;

            _mockRepository.Setup(r => r.BuscarPorIdAsync(id))
                .ReturnsAsync(proposta);

            // Act
            var resultado = await _service.BuscarPorIdAsync(id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(proposta.Titulo, resultado.Titulo);
            Assert.Equal(proposta.ValorPremio, resultado.ValorPremio);
        }

        [Fact]
        public async Task AlterarStatusAsync_AtualizaStatus_QuandoPropostaExiste()
        {
            // Arrange
            var resultadoProposta = Proposta.Criar("Titulo", "Descricao", 100.0m, 1000.0m, "teste@email.com");
            var proposta = resultadoProposta.Valor;
            proposta.Id = 1;
            var dto = new AlteraStatusDTO
            {
                Id = 1,
                Status = StatusProposta.Aprovada
            };

            _mockRepository.Setup(r => r.BuscarPorIdAsync(dto.Id))
                .ReturnsAsync(proposta);

            _mockRepository.Setup(r => r.AtualizaStatusAsync(It.IsAny<int>(), It.IsAny<StatusProposta>()))
                .ReturnsAsync(true);

            // Act
            await _service.AlterarStatusAsync(dto);

            // Assert
            _mockRepository.Verify(
                r => r.AtualizaStatusAsync(dto.Id, StatusProposta.Aprovada),
                Times.Once);
        }

        [Fact]
        public async Task AlterarStatusAsync_LancaArgumentNullException_QuandoDtoNulo()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _service.AlterarStatusAsync(null!));
        }

        [Fact]
        public async Task AlterarStatusAsync_RetornaErro_QuandoAtualizacaoFalha()
        {
            // Arrange
            var resultadoProposta = Proposta.Criar("Titulo", "Descricao", 100.0m, 1000.0m, "teste@email.com");
            var proposta = resultadoProposta.Valor;
            proposta.Id = 1;
            var dto = new AlteraStatusDTO
            {
                Id = 1,
                Status = StatusProposta.Aprovada
            };

            _mockRepository.Setup(r => r.BuscarPorIdAsync(dto.Id))
                .ReturnsAsync(proposta);

            _mockRepository.Setup(r => r.AtualizaStatusAsync(It.IsAny<int>(), It.IsAny<StatusProposta>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await _service.AlterarStatusAsync(dto);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Contains("Erro ao atualizar o status da proposta", resultado.Erro!.Mensagem);
            _mockRepository.Verify(
                r => r.AtualizaStatusAsync(dto.Id, StatusProposta.Aprovada),
                Times.Once);
        }

        [Fact]
        public async Task AlterarStatusAsync_RetornaErro_QuandoMesmoStatus()
        {
            // Arrange
            var resultadoProposta = Proposta.Criar("Titulo", "Descricao", 100.0m, 1000.0m, "teste@email.com");
            var proposta = resultadoProposta.Valor;
            var dto = new AlteraStatusDTO
            {
                Id = 1,
                Status = StatusProposta.EmAnalise // Mesmo status inicial
            };

            _mockRepository.Setup(r => r.BuscarPorIdAsync(dto.Id))
                .ReturnsAsync(proposta);

            // Act
            var resultado = await _service.AlterarStatusAsync(dto);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.Contains("Status igual ao atual.", resultado.Erro!.Mensagem);
        }
    }
}