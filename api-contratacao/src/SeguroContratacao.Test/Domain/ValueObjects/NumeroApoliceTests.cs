using SeguroContratacao.Domain.ValueObjects;

namespace SeguroContratacao.Test.Domain.ValueObjects
{
    public class NumeroApoliceTests
    {
        [Fact]
        public void Gerar_DeveRetornarNumeroApoliceNaoNulo()
        {
            // Act
            var numeroApolice = NumeroApolice.Gerar();

            // Assert
            Assert.NotNull(numeroApolice);
            Assert.NotNull(numeroApolice.Valor);
        }

        [Fact]
        public void Gerar_DeveRetornarNumeroComDezCaracteres()
        {
            // Act
            var numeroApolice = NumeroApolice.Gerar();

            // Assert
            Assert.Equal(10, numeroApolice.Valor.Length);
        }

        [Fact]
        public void Gerar_DeveRetornarNumeroEmMaiusculas()
        {
            // Act
            var numeroApolice = NumeroApolice.Gerar();

            // Assert
            Assert.Equal(numeroApolice.Valor.ToUpper(), numeroApolice.Valor);
        }

        [Fact]
        public void Gerar_DeveRetornarNumeroSemHifens()
        {
            // Act
            var numeroApolice = NumeroApolice.Gerar();

            // Assert
            Assert.DoesNotContain("-", numeroApolice.Valor);
        }

        [Fact]
        public void Gerar_DeveRetornarApenasCaracteresAlfanumericos()
        {
            // Act
            var numeroApolice = NumeroApolice.Gerar();

            // Assert
            Assert.Matches("^[A-Z0-9]+$", numeroApolice.Valor);
        }

        [Fact]
        public void Gerar_DeveGerarNumerosUnicos()
        {
            // Act
            var numeroApolice1 = NumeroApolice.Gerar();
            var numeroApolice2 = NumeroApolice.Gerar();
            var numeroApolice3 = NumeroApolice.Gerar();

            // Assert
            Assert.NotEqual(numeroApolice1.Valor, numeroApolice2.Valor);
            Assert.NotEqual(numeroApolice1.Valor, numeroApolice3.Valor);
            Assert.NotEqual(numeroApolice2.Valor, numeroApolice3.Valor);
        }

        [Fact]
        public void Gerar_ChamadoMultiplasVezes_DeveGerarNumerosDistintos()
        {
            // Arrange
            var numeros = new HashSet<string>();
            var quantidadeGeracoes = 100;

            // Act
            for (int i = 0; i < quantidadeGeracoes; i++)
            {
                var numeroApolice = NumeroApolice.Gerar();
                numeros.Add(numeroApolice.Valor);
            }

            // Assert
            Assert.Equal(quantidadeGeracoes, numeros.Count);
        }

        [Fact]
        public void Valor_DeveRetornarValorCorreto()
        {
            // Act
            var numeroApolice = NumeroApolice.Gerar();

            // Assert
            Assert.NotEmpty(numeroApolice.Valor);
        }

        [Fact]
        public void OperadorImplicito_DeveConverterParaString()
        {
            // Arrange
            var numeroApolice = NumeroApolice.Gerar();

            // Act
            string valorComoString = numeroApolice;

            // Assert
            Assert.Equal(numeroApolice.Valor, valorComoString);
        }

        [Fact]
        public void OperadorImplicito_DevePermitirAtribuicaoDireta()
        {
            // Arrange
            var numeroApolice = NumeroApolice.Gerar();

            // Act
            string resultado = numeroApolice;

            // Assert
            Assert.IsType<string>(resultado);
            Assert.Equal(10, resultado.Length);
        }
    }
}