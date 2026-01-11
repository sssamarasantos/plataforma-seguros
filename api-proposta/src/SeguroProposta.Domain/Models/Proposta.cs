using SeguroProposta.Domain.Common;
using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.ValueObjects;

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
        public string EmailContratante { get; private set; } = string.Empty;

        protected Proposta() { }

        private Proposta(
            string titulo,
            string descricao,
            decimal valorPremio,
            decimal valorCobertura,
            Email emailContratante)
        {
            NumeroProposta = ValueObjects.NumeroProposta.Gerar();
            Status = StatusProposta.EmAnalise;
            DataHoraInclusao = DateTime.UtcNow;
            Titulo = titulo;
            Descricao = descricao;
            ValorPremio = valorPremio;
            ValorCobertura = valorCobertura;
            EmailContratante = emailContratante;
        }

        public static ResultadoOperacao<Proposta> Criar(
            string titulo,
            string descricao,
            decimal valorPremio,
            decimal valorCobertura,
            string emailContratante)
        {
            var resultadoTitulo = ValidarTitulo(titulo);
            if (resultadoTitulo.EhFalha)
                return ResultadoOperacao.Falha<Proposta>(resultadoTitulo.Erro!);

            var resultadoDescricao = ValidarDescricao(descricao);
            if (resultadoDescricao.EhFalha)
                return ResultadoOperacao.Falha<Proposta>(resultadoDescricao.Erro!);

            var resultadoValorPremio = ValidarValorPremio(valorPremio);
            if (resultadoValorPremio.EhFalha)
                return ResultadoOperacao.Falha<Proposta>(resultadoValorPremio.Erro!);

            var resultadoValorCobertura = ValidarValorCobertura(valorPremio, valorCobertura);
            if (resultadoValorCobertura.EhFalha)
                return ResultadoOperacao.Falha<Proposta>(resultadoValorCobertura.Erro!);

            var resultadoEmail = Email.Criar(emailContratante);
            if (resultadoEmail.EhFalha)
                return ResultadoOperacao.Falha<Proposta>(resultadoEmail.Erro!);

            return new Proposta(
                titulo,
                descricao,
                valorPremio,
                valorCobertura,
                resultadoEmail.Valor);
        }

        public ResultadoOperacao AlterarStatus(StatusProposta novoStatus)
        {
            if (novoStatus == Status)
                return ErrosDomain.StatusIgualAtual;

            var resultadoTransicao = ValidarTransicaoDeStatus(novoStatus);
            if (resultadoTransicao.EhFalha)
                return resultadoTransicao;

            Status = novoStatus;

            return ResultadoOperacao.Sucesso();
        }

        private static ResultadoOperacao ValidarTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return ErrosDomain.TituloObrigatorio;

            if (titulo.Length > 100)
                return ErrosDomain.TituloExcedeTamanhoMaximo;

            return ResultadoOperacao.Sucesso();
        }

        private static ResultadoOperacao ValidarDescricao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                return ErrosDomain.DescricaoObrigatoria;

            if (descricao.Length > 255)
                return ErrosDomain.DescricaoExcedeTamanhoMaximo;

            return ResultadoOperacao.Sucesso();
        }

        private static ResultadoOperacao ValidarValorPremio(decimal valor)
        {
            if (valor <= 0)
                return ErrosDomain.ValorPremioInvalido;

            return ResultadoOperacao.Sucesso();
        }

        private static ResultadoOperacao ValidarValorCobertura(decimal valorPremio, decimal valorCobertura)
        {
            if (valorCobertura <= valorPremio)
                return ErrosDomain.ValorCoberturaInvalido;

            return ResultadoOperacao.Sucesso();
        }

        private ResultadoOperacao ValidarTransicaoDeStatus(StatusProposta novoStatus)
        {
            if (Status == StatusProposta.Aprovada && novoStatus == StatusProposta.EmAnalise)
                return ErrosDomain.TransicaoStatusInvalida;

            return ResultadoOperacao.Sucesso();
        }
    }
}
