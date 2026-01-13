namespace SeguroContratacao.Domain.Events
{
    public class ContratacaoEfetivadaEvent
    {
        public int IdContratacao { get; }
        public string EmailContratante { get; }
        public string NumeroApolice { get; }
        public DateTime DataHoraContratacao { get; }

        public ContratacaoEfetivadaEvent(int idContratacao, string emailContratante, string numeroApolice, DateTime dataHoraContratacao)
        {
            IdContratacao = idContratacao;
            EmailContratante = emailContratante;
            NumeroApolice = numeroApolice;
            DataHoraContratacao = dataHoraContratacao;
        }
    }
}