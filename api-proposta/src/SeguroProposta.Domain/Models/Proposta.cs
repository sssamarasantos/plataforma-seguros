using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.Exceptions;

namespace SeguroProposta.Domain.Models
{
    public class Proposta
    {
        public int Id { get; internal set; }
        public string NumeroProposta { get; private set; } = string.Empty;
        public StatusProposta Status { get; private set; }
        public string Titulo { get; private set; } = string.Empty;
        public string Descricao { get; private set; } = string.Empty;
        public DateTime DataHoraInclusao { get; private set; }
        public decimal ValorPremio { get; private set; }
        public decimal ValorCobertura { get; private set; }

        protected Proposta() { }

        private Proposta(string titulo, string descricao, decimal valorPremio, decimal valorCobertura)
        {
            NumeroProposta = ValueObjects.NumeroProposta.Gerar();
            Status = StatusProposta.EmAnalise;
            DataHoraInclusao = DateTime.UtcNow;
            Titulo = titulo;
            Descricao = descricao;
            ValorPremio = valorPremio;
            ValorCobertura = valorCobertura;
        }

        public static Proposta Criar(string titulo, string descricao, decimal valorPremio, decimal valorCobertura)
        {
            ValidarTitulo(titulo);
            ValidarDescricao(descricao);
            ValidarValorPremio(valorPremio);
            ValidarValorCobertura(valorPremio, valorCobertura);

            return new Proposta(titulo, descricao, valorPremio, valorCobertura);
        }

        public void AlterarStatus(StatusProposta novoStatus)
        {
            if (novoStatus == Status)
                throw new RegraDeNegocioException("Status igual ao atual.");

            ValidarTransicaoDeStatus(novoStatus);

            Status = novoStatus;
        }

        private static void ValidarTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new PropostaInvalidaException("Título é obrigatório.");

            if (titulo.Length > 100)
                throw new PropostaInvalidaException("Título não pode exceder 100 caracteres.");
        }

        private static void ValidarDescricao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new PropostaInvalidaException("Descrição é obrigatória.");

            if (descricao.Length > 255)
                throw new PropostaInvalidaException("Descrição não pode exceder 255 caracteres.");
        }

        private static void ValidarValorPremio(decimal valor)
        {
            if (valor <= 0)
                throw new PropostaInvalidaException("Valor do prêmio deve ser maior que zero.");
        }

        private static void ValidarValorCobertura(decimal valorPremio, decimal valorCobertura)
        {
            if (valorCobertura <= valorPremio)
                throw new PropostaInvalidaException("Valor da cobertura deve ser maior que o prêmio.");
        }

        private void ValidarTransicaoDeStatus(StatusProposta novoStatus)
        {
            if (Status == StatusProposta.Aprovada && novoStatus == StatusProposta.EmAnalise)
                throw new RegraDeNegocioException("Não é possível retornar uma proposta aprovada para análise.");
        }
    }
}
