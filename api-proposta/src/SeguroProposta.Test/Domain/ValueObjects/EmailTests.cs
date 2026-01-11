using SeguroProposta.Domain.ValueObjects;

namespace SeguroProposta.Test.Domain.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Criar_RetornaSucesso_QuandoEmailValido()
        {
            // Arrange
            var emailValido = "teste@exemplo.com";

            // Act
            var resultado = Email.Criar(emailValido);

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.Equal(emailValido, resultado.Valor.Valor);
        }

        [Theory]
        [InlineData("usuario@dominio.com")]
        [InlineData("nome.sobrenome@empresa.com.br")]
        [InlineData("email+tag@dominio.co")]
        [InlineData("user123@test-domain.com")]
        public void Criar_RetornaSucesso_QuandoEmailsValidos(string email)
        {
            // Act
            var resultado = Email.Criar(email);

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.Equal(email, resultado.Valor.Valor);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Criar_RetornaErro_QuandoEmailVazio(string emailInvalido)
        {
            // Act
            var resultado = Email.Criar(emailInvalido!);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.NotNull(resultado.Erro);
            Assert.Contains("obrigatório", resultado.Erro.Mensagem, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("semArroba")]
        [InlineData("sem@dominio")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@")]
        [InlineData("usuario @dominio.com")]
        [InlineData("usuario@ dominio.com")]
        [InlineData("usuario@dominio .com")]
        public void Criar_RetornaErro_QuandoEmailInvalido(string emailInvalido)
        {
            // Act
            var resultado = Email.Criar(emailInvalido);

            // Assert
            Assert.True(resultado.EhFalha);
            Assert.NotNull(resultado.Erro);
            Assert.Contains("inválido", resultado.Erro.Mensagem, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Criar_RetornaErro_QuandoEmailMuitoLongo()
        {
            // Arrange
            var emailLongo = new string('a', 250) + "@test.com"; // Mais de 254 caracteres

            // Act
            var resultado = Email.Criar(emailLongo);

            // Assert
            Assert.True(resultado.EhFalha);
        }

        [Fact]
        public void ToString_RetornaValor()
        {
            // Arrange
            var emailString = "teste@exemplo.com";
            var resultado = Email.Criar(emailString);
            var email = resultado.Valor;

            // Act
            var toString = email.ToString();

            // Assert
            Assert.Equal(emailString, toString);
        }

        [Fact]
        public void ConversaoImplicita_ParaString_Funciona()
        {
            // Arrange
            var emailString = "teste@exemplo.com";
            var resultado = Email.Criar(emailString);
            var email = resultado.Valor;

            // Act
            string valorConvertido = email;

            // Assert
            Assert.Equal(emailString, valorConvertido);
        }

        [Fact]
        public void Criar_RemoveEspacos_QuandoEmailComEspacos()
        {
            // Arrange
            var emailComEspacos = "  teste@exemplo.com  ";

            // Act
            var resultado = Email.Criar(emailComEspacos);

            // Assert
            Assert.True(resultado.EhSucesso);
            Assert.Equal("teste@exemplo.com", resultado.Valor.Valor);
        }

        [Fact]
        public void Email_EhRecord_ComparaCorretamente()
        {
            // Arrange
            var resultado1 = Email.Criar("teste@exemplo.com");
            var resultado2 = Email.Criar("teste@exemplo.com");
            var resultado3 = Email.Criar("outro@exemplo.com");

            // Assert
            Assert.Equal(resultado1.Valor, resultado2.Valor);
            Assert.NotEqual(resultado1.Valor, resultado3.Valor);
        }
    }
}
