using Amazon.SecretsManager.Extensions.Caching;

namespace SeguroContratacao.Infraestructure.AWS
{
    public static class SecretsManager
    {
        public static async Task<string> ObterAsync(string nomeSegredo)
        {
            var config = new SecretCacheConfiguration
            {
                CacheItemTTL = 4294967295 // Tempo de vida do cache em segundos (maximo permitido, 49,7 dias)  
            };

            using var cache = new SecretsManagerCache(config);

            string segredoString = await cache.GetSecretString(nomeSegredo);

            return segredoString;
        }
    }
}
