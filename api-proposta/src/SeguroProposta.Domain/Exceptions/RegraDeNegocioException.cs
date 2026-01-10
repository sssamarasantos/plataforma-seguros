namespace SeguroProposta.Domain.Exceptions
{
    public class RegraDeNegocioException : DomainException
    {
        public RegraDeNegocioException(string message) : base(message) { }

        public RegraDeNegocioException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}