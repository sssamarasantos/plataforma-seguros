using SeguroProposta.Domain.ValueObjects;

namespace SeguroProposta.Test.Domain.ValueObjects
{
    public class NumeroPropostaTests
    {
        [Fact]
        public void Gerar_CriaInstanciaValida_QuandoChamado()
        {
            // Act
            var numeroProposta = NumeroProposta.Gerar();

            // Assert
            Assert.NotNull(numeroProposta);
            Assert.NotNull(numeroProposta.Valor);
        }

        [Fact]
        public void Gerar_RetornaNumeroComDezCaracteres_Sempre()
        {
            // Act
            var numeroProposta = NumeroProposta.Gerar();

            // Assert
            Assert.Equal(10, numeroProposta.Valor.Length);
        }

        [Fact]
        public void Gerar_RetornaApenasCaracteresHexadecimaisMaiusculos_Sempre()
        {
            // Act
            var numeroProposta = NumeroProposta.Gerar();

            // Assert
            Assert.Matches("^[0-9A-F]{10}$", numeroProposta.Valor);
        }

        [Fact]
        public void Gerar_GeraNumerosDiferentes_QuandoChamadoMultiplasVezes()
        {
            // Act
            var numeros = new HashSet<string>();
            for (int i = 0; i < 100; i++)
            {
                numeros.Add(NumeroProposta.Gerar().Valor);
            }

            // Assert
            Assert.Equal(100, numeros.Count);
        }

        [Fact]
        public void ConversaoImplicita_RetornaValorCorreto_QuandoConvertidoParaString()
        {
            // Arrange
            var numeroProposta = NumeroProposta.Gerar();
            var valorEsperado = numeroProposta.Valor;

            // Act
            string numeroComoString = numeroProposta;

            // Assert
            Assert.Equal(valorEsperado, numeroComoString);
        }

        [Fact]
        public void Valor_RetornaNumeroGerado_QuandoAcessado()
        {
            // Act
            var numeroProposta = NumeroProposta.Gerar();

            // Assert
            Assert.NotNull(numeroProposta.Valor);
            Assert.Equal(10, numeroProposta.Valor.Length);
            Assert.All(numeroProposta.Valor, c => Assert.True(char.IsUpper(c) || char.IsDigit(c)));
        }

        [Fact]
        public void Gerar_NaoContemHifens_NoNumeroGerado()
        {
            // Act
            var numeroProposta = NumeroProposta.Gerar();

            // Assert
            Assert.DoesNotContain("-", numeroProposta.Valor);
        }

        [Fact]
        public void Gerar_TodosCaracteresSaoMaiusculos_QuandoContemLetras()
        {
            // Arrange
            var numeroProposta = NumeroProposta.Gerar();

            // Assert
            Assert.Equal(numeroProposta.Valor.ToUpper(), numeroProposta.Valor);
        }
    }
}