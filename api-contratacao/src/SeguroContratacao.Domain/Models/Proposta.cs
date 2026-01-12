using SeguroContratacao.Domain.Enums;

namespace SeguroContratacao.Domain.Models
{
    public class Proposta
    {
        public int Id { get; private set; }
        public StatusProposta Status { get; private set; }
        public string EmailContratante { get; private set; }

        public Proposta(int id, StatusProposta status, string emailContratante)
        {
            Id = id;
            Status = status;
            EmailContratante = emailContratante;
        }
    }
}
