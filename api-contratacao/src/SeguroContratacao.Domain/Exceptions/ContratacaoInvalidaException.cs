namespace SeguroContratacao.Domain.Exceptions
{
    public class ContratacaoInvalidaException : DomainException
    {
        public ContratacaoInvalidaException(string message) : base(message) { }

        public ContratacaoInvalidaException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
