using SeguroContratacao.Domain.Enums;

namespace SeguroContratacao.Domain.Models
{
    public class Proposta
    {
        public int Id { get; private set; }
        public StatusProposta Status { get; private set; }
        public Proposta(int id, StatusProposta status)
        {
            Id = id;
            Status = status;
        }
    }
}
