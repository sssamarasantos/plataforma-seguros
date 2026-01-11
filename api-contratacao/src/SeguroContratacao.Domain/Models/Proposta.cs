using SeguroContratacao.Domain.Enums;

namespace SeguroContratacao.Domain.Models
{
    public class Proposta
    {
        public int Id { get; private set; }
        public StatusProposta Status { get; private set; }
        public string EmailContratante { get; private set; }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public Proposta(int id, StatusProposta status, string emailContratante)
        {
            Id = id;
            Status = status;
            EmailContratante = emailContratante;
        }
    }
}
