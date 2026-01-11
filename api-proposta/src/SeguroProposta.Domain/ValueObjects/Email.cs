using SeguroProposta.Domain.Common;
using System.Text.RegularExpressions;

namespace SeguroProposta.Domain.ValueObjects
{
    public sealed record Email
    {
        public string Valor { get; }

        private Email(string valor)
        {
            Valor = valor;
        }

        public static ResultadoOperacao<Email> Criar(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return ErrosDomain.EmailObrigatorio;

            email = email.Trim();

            if (!IsValidEmail(email))
                return ErrosDomain.EmailInvalido;

            return new Email(email);
        }

        private static bool IsValidEmail(string email)
        {
            if (email.Length > 254)
                return false;

            var emailRegex = new Regex(
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));

            try
            {
                return emailRegex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static implicit operator string(Email email) => email.Valor;

        public override string ToString() => Valor;
    }
}
