namespace SeguroContratacao.Domain.Common
{
    public static class ErrosDomain
    {
        public static Erro TituloObrigatorio { get; } = new(TipoErro.RegraDeNegocio, "A proposta deve estar aprovada para finalizar a contratação.");

        public static Erro IdPropostaInvalido { get; } = new(TipoErro.Validacao, "O ID da proposta não pode ser zero.");

        public static Erro ValorPremioInvalido { get; } = new(TipoErro.Validacao, "Valor do prêmio deve ser maior que zero.");

        public static Erro ValorCoberturaInvalido { get; } = new(TipoErro.Validacao, "Valor da cobertura deve ser maior que o prêmio.");
    }
}
