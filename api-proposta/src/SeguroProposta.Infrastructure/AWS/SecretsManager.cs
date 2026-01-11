using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;

namespace SeguroProposta.Infrastructure.AWS
{
    public sealed class SecretsManager : IDisposable
    {
        private readonly SecretsManagerCache _cache;

        public SecretsManager()
        {
            var client = new AmazonSecretsManagerClient(RegionEndpoint.USEast1);

            var config = new SecretCacheConfiguration
            {
                CacheItemTTL = 3600, // 1 hora 
                Client = client
            };

            _cache = new SecretsManagerCache(config);
        }

        public async Task<string> ObterAsync(string nomeSegredo)
        {
            if (string.IsNullOrWhiteSpace(nomeSegredo))
                throw new ArgumentException("Nome do segredo não pode ser vazio.", nameof(nomeSegredo));

            try
            {
                return await _cache.GetSecretString(nomeSegredo);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Erro ao obter segredo '{nomeSegredo}' do AWS Secrets Manager.", ex);
            }
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}
