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

            // Act
            var proposta = new Proposta(id, status);

            // Assert
            Assert.NotNull(proposta);
            Assert.Equal(id, proposta.Id);
            Assert.Equal(status, proposta.Status);
        }

        [Fact]
        public void Construtor_ComStatusEmAnalise_DeveCriarPropostaComStatusCorreto()
        {
            // Arrange
            var id = 10;
            var status = StatusProposta.EmAnalise;

            // Act
            var proposta = new Proposta(id, status);

            // Assert
            Assert.Equal(id, proposta.Id);
            Assert.Equal(StatusProposta.EmAnalise, proposta.Status);
        }

        [Fact]
        public void Construtor_ComStatusAprovada_DeveCriarPropostaComStatusCorreto()
        {
            // Arrange
            var id = 20;
            var status = StatusProposta.Aprovada;

            // Act
            var proposta = new Proposta(id, status);

            // Assert
            Assert.Equal(id, proposta.Id);
            Assert.Equal(StatusProposta.Aprovada, proposta.Status);
        }

        [Fact]
        public void Construtor_ComStatusRejeitada_DeveCriarPropostaComStatusCorreto()
        {
            // Arrange
            var id = 30;
            var status = StatusProposta.Rejeitada;

            // Act
            var proposta = new Proposta(id, status);

            // Assert
            Assert.Equal(id, proposta.Id);
            Assert.Equal(StatusProposta.Rejeitada, proposta.Status);
        }

        [Theory]
        [InlineData(1, StatusProposta.EmAnalise)]
        [InlineData(100, StatusProposta.Aprovada)]
        [InlineData(999, StatusProposta.Rejeitada)]
        [InlineData(5000, StatusProposta.EmAnalise)]
        public void Construtor_ComDiferentesValores_DeveCriarPropostaCorretamente(int id, StatusProposta status)
        {
            // Act
            var proposta = new Proposta(id, status);

            // Assert
            Assert.Equal(id, proposta.Id);
            Assert.Equal(status, proposta.Status);
        }
    }
}