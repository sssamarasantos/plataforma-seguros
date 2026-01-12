using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SeguroContratacao.Domain.Interfaces;

namespace SeguroContratacao.Infrastructure.AWS
{
    public class SnsNotificacaoService : INotificacaoService
    {
        private readonly string _topicArn;

        public SnsNotificacaoService(string topicArn)
        {
            _topicArn = topicArn ?? throw new ArgumentNullException(nameof(topicArn));
        }

        public async Task NotificarContratacaoEfetivadaAsync(int idContratacao, string emailContratante, string numeroApolice)
        {
            using var client = new AmazonSimpleNotificationServiceClient();

            var mensagem = new
            {
                Tipo = "ContratacaoEfetivada",
                IdContratacao = idContratacao,
                EmailContratante = emailContratante,
                NumeroApolice = numeroApolice,
                DataHora = DateTime.UtcNow
            };

            var mensagemJson = JsonSerializer.Serialize(mensagem);

            var request = new PublishRequest
            {
                TopicArn = _topicArn,
                Message = mensagemJson,
                Subject = "Nova Contratação Efetivada"
            };

            await client.PublishAsync(request);
        }
    }
}