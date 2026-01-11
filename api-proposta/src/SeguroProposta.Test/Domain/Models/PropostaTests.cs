using SeguroProposta.Domain.Enums;
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
            var emailContratante = "teste@email.com";

            // Act
            var momentoAntes = DateTime.UtcNow;
            var resultado = Proposta.Criar(titulo, descricao, valorPremio, valorCobertura, emailContratante);
            var momentoDepois = DateTime.UtcNow;

            // Assert
            Assert.True(resultado.EhSucesso);
            var proposta = resultado.Valor;
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
        public void Criar_RetornaFalha_QuandoTituloInvalido(string tituloInvalido)
        {
            // Arrange
            var descricao = "Descricao";
            decimal valorPremio = 100.0m;
            decimal valorCobertura = 1000.0m;

            // Act
            var resultado = Proposta.Criar(tituloInvalido!, descricao, valorPremio, valorCobertura, "teste@email.com");

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.NotNull(resultado.Erro);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Criar_RetornaFalha_QuandoDescricaoInvalida(string descricaoInvalida)
        {
            // Arrange
            var titulo = "Titulo";
            decimal valorPremio = 100.0m;
            decimal valorCobertura = 1000.0m;

            // Act
            var resultado = Proposta.Criar(titulo, descricaoInvalida!, valorPremio, valorCobertura, "teste@email.com");

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.NotNull(resultado.Erro);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Criar_RetornaFalha_QuandoValorPremioInvalido(decimal valorInvalido)
        {
            // Arrange
            var titulo = "Titulo";
            var descricao = "Descricao";
            decimal valorCobertura = 1000.0m;

            // Act
            var resultado = Proposta.Criar(titulo, descricao, valorInvalido, valorCobertura, "teste@email.com");

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.NotNull(resultado.Erro);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Criar_RetornaFalha_QuandoValorCoberturaInvalido(decimal valorInvalido)
        {
            // Arrange
            var titulo = "Titulo";
            var descricao = "Descricao";
            decimal valorPremio = 100.0m;

            // Act
            var resultado = Proposta.Criar(titulo, descricao, valorPremio, valorInvalido, "teste@email.com");

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.NotNull(resultado.Erro);
        }

        [Fact]
        public void AlterarStatus_AlteraQuandoDiferente()
        {
            // Arrange
            var resultadoCriacao = Proposta.Criar("Titulo", "Descricao", 50.0m, 500.0m, "teste@email.com");
            var proposta = resultadoCriacao.Valor;

            // Act
            var resultadoAlteracao = proposta.AlterarStatus(StatusProposta.Aprovada);

            // Assert
            Assert.True(resultadoAlteracao.EhSucesso);
            Assert.Equal(StatusProposta.Aprovada, proposta.Status);
        }

        [Fact]
        public void AlterarStatus_RetornaErro_QuandoMesmoStatus()
        {
            // Arrange
            var resultadoCriacao = Proposta.Criar("Titulo", "Descricao", 50.0m, 500.0m, "teste@email.com");
            var proposta = resultadoCriacao.Valor;

            // Act
            var resultadoAlteracao = proposta.AlterarStatus(proposta.Status);

            // Assert
            Assert.True(resultadoAlteracao.EhFalha);
            Assert.NotNull(resultadoAlteracao.Erro);
            Assert.Contains("Status igual ao atual", resultadoAlteracao.Erro.Mensagem);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Criar_RetornaFalha_QuandoEmailVazio(string emailInvalido)
        {
            // Arrange
            var titulo = "Titulo";
            var descricao = "Descricao";
            decimal valorPremio = 100.0m;
            decimal valorCobertura = 1000.0m;

            // Act
            var resultado = Proposta.Criar(titulo, descricao, valorPremio, valorCobertura, emailInvalido!);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.NotNull(resultado.Erro);
            Assert.Contains("obrigatório", resultado.Erro.Mensagem, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("emailinvalido")]
        [InlineData("sem@dominio")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@")]
        public void Criar_RetornaFalha_QuandoEmailInvalido(string emailInvalido)
        {
            // Arrange
            var titulo = "Titulo";
            var descricao = "Descricao";
            decimal valorPremio = 100.0m;
            decimal valorCobertura = 1000.0m;

            // Act
            var resultado = Proposta.Criar(titulo, descricao, valorPremio, valorCobertura, emailInvalido);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.NotNull(resultado.Erro);
            Assert.Contains("inválido", resultado.Erro.Mensagem, StringComparison.OrdinalIgnoreCase);
        }
    }
}
