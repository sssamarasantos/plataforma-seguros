using SeguroContratacao.Domain.Enums;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Test.Domain.Models
{
    public class PropostaTests
    {
        [Fact]
        public void Construtor_ComParametrosValidos_DeveCriarPropostaCorretamente()
        {
            // Arrange
            var id = 1;
            var status = StatusProposta.Aprovada;
            var emailContratante = "contratante@email.com";

            // Act
            var proposta = new Proposta(id, status, emailContratante);

            // Assert
            Assert.NotNull(proposta);
            Assert.Equal(id, proposta.Id);
            Assert.Equal(status, proposta.Status);
            Assert.Equal(emailContratante, proposta.EmailContratante);
        }

        [Fact]
        public void Construtor_ComStatusEmAnalise_DeveCriarPropostaComStatusCorreto()
        {
            // Arrange
            var id = 10;
            var status = StatusProposta.EmAnalise;
            var emailContratante = "contratante@email.com";

            // Act
            var proposta = new Proposta(id, status, emailContratante);

            // Assert
            Assert.Equal(id, proposta.Id);
            Assert.Equal(StatusProposta.EmAnalise, proposta.Status);
            Assert.Equal(emailContratante, proposta.EmailContratante);
        }

        [Fact]
        public void Construtor_ComStatusAprovada_DeveCriarPropostaComStatusCorreto()
        {
            // Arrange
            var id = 20;
            var status = StatusProposta.Aprovada;
            var emailContratante = "contratante@email.com";

            // Act
            var proposta = new Proposta(id, status, emailContratante);

            // Assert
            Assert.Equal(id, proposta.Id);
            Assert.Equal(StatusProposta.Aprovada, proposta.Status);
            Assert.Equal(emailContratante, proposta.EmailContratante);
        }

        [Fact]
        public void Construtor_ComStatusRejeitada_DeveCriarPropostaComStatusCorreto()
        {
            // Arrange
            var id = 30;
            var status = StatusProposta.Rejeitada;
            var emailContratante = "contratante@email.com";

            // Act
            var proposta = new Proposta(id, status, emailContratante);

            // Assert
            Assert.Equal(id, proposta.Id);
            Assert.Equal(StatusProposta.Rejeitada, proposta.Status);
            Assert.Equal(emailContratante, proposta.EmailContratante);
        }

        [Theory]
        [InlineData(1, StatusProposta.EmAnalise)]
        [InlineData(100, StatusProposta.Aprovada)]
        [InlineData(999, StatusProposta.Rejeitada)]
        [InlineData(5000, StatusProposta.EmAnalise)]
        public void Construtor_ComDiferentesValores_DeveCriarPropostaCorretamente(int id, StatusProposta status)
        {
            // Arrange
            var emailContratante = "contratante@email.com";

            // Act
            var proposta = new Proposta(id, status, emailContratante);

            // Assert
            Assert.Equal(id, proposta.Id);
            Assert.Equal(status, proposta.Status);
            Assert.Equal(emailContratante, proposta.EmailContratante);
        }
    }
}