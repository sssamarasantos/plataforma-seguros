namespace SeguroContratacao.Domain.ValueObjects
{
    public class NumeroApolice
    {
        public string Valor { get; }

        private NumeroApolice(string valor)
        {
            Valor = valor;
        }

        public static NumeroApolice Gerar()
        {
            var numero = Guid.NewGuid().ToString().Replace("-", "").ToUpper().Substring(0, 10);
            return new NumeroApolice(numero);
        }

        public static implicit operator string(NumeroApolice numeroApolice) => numeroApolice.Valor;

    }
}
