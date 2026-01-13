namespace SeguroProposta.Domain.Common
{
    public static class ErrosDomain
    {
        private const int TituloTamanhoMaximo = 100;
        private const int DescricaoTamanhoMaximo = 255;

        public static Erro TituloObrigatorio { get; } = new(TipoErro.Validacao, "Título é obrigatório.");
        public static Erro TituloExcedeTamanhoMaximo { get; } = new(TipoErro.Validacao, $"Título não pode exceder {TituloTamanhoMaximo} caracteres.");
        public static Erro DescricaoObrigatoria { get; } = new(TipoErro.Validacao, "Descrição é obrigatória.");
        public static Erro DescricaoExcedeTamanhoMaximo { get; } = new(TipoErro.Validacao, $"Descrição não pode exceder {DescricaoTamanhoMaximo} caracteres.");
        public static Erro ValorCoberturaInvalido { get; } = new(TipoErro.Validacao, "Valor da cobertura deve ser maior que o prêmio.");
        public static Erro ValorPremioInvalido { get;  } = new(TipoErro.Validacao, "Valor do prêmio deve ser maior que o zero.");
        public static Erro StatusIgualAtual { get; } = new(TipoErro.RegraDeNegocio, "Status igual ao atual.");
        public static Erro TransicaoStatusInvalida { get; } = new(TipoErro.RegraDeNegocio, "Não é possível retornar uma proposta aprovada para análise.");
        public static Erro EmailObrigatorio { get; } = new(TipoErro.Validacao, "Email é obrigatório.");
        public static Erro EmailInvalido { get; } = new(TipoErro.Validacao, "Email inválido.");
    }
}
