namespace SeguroProposta.Domain.ValueObjects
{
    public sealed class NumeroProposta
    {
        public string Valor { get; }

        private NumeroProposta(string valor)
        {
            Valor = valor;
        }

        public static NumeroProposta Gerar()
        {
            var numero = Guid.NewGuid().ToString().Replace("-", "").ToUpper().Substring(0, 10);
            return new NumeroProposta(numero);
        }

        public static implicit operator string(NumeroProposta numeroProposta) => numeroProposta.Valor;
    }
}