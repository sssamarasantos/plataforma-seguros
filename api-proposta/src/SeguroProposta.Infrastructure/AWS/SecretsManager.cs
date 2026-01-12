using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Microsoft.Extensions.Logging;

namespace SeguroProposta.Infrastructure.AWS
{
    public sealed class SecretsManager : IDisposable
    {
        private readonly SecretsManagerCache? _cache;
        private readonly ILogger<SecretsManager>? _logger;

        public SecretsManager(ILogger<SecretsManager>? logger = null)
        {
            _logger = logger;
            try
            {
                var client = new AmazonSecretsManagerClient(RegionEndpoint.USEast1);

                var config = new SecretCacheConfiguration
                {
                    CacheItemTTL = 3600,
                    Client = client
                };

                _cache = new SecretsManagerCache(config);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Não foi possível inicializar o AWS Secrets Manager. Será usado fallback para variáveis de ambiente.");
                _cache = null;
            }
        }

        public async Task<string?> ObterAsync(string nomeSegredo)
        {
            if (string.IsNullOrWhiteSpace(nomeSegredo))
                throw new ArgumentException("Nome do segredo não pode ser vazio.", nameof(nomeSegredo));

            if (_cache == null)
            {
                _logger?.LogInformation("Cache do AWS Secrets Manager não está disponível. Retornando null para usar fallback.");
                return null;
            }

            try
            {
                return await _cache.GetSecretString(nomeSegredo);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Erro ao obter segredo '{NomeSegredo}' do AWS Secrets Manager. Retornando null para usar fallback.", nomeSegredo);
                return null;
            }
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}
