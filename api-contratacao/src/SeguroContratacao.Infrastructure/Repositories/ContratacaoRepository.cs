using Dapper;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.Models;
using SeguroContratacao.Infrastructure.Interfaces;
using System.Data;

namespace SeguroContratacao.Infrastructure.Repositories
{
    public class ContratacaoRepository : IContratacaoRepository
    {
        private readonly IDbContextFactory _connectionFactory;

        public ContratacaoRepository(IDbContextFactory connectionFactory)
        {
            _connectionFactory = connectionFactory
                ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<bool> InserirAsync(Contratacao contratacao)
        {
            const string query = @"
                INSERT INTO CONTRATACAO (
                    NUMERO_APOLICE
                    , DATAHORA_CONTRATACAO
                    , ID_PROPOSTA
                    , VALOR_PREMIO_FINAL
                    , VALOR_COBERTURA_FINAL
                    , EMAIL_CONTRATANTE)
                VALUES (
                    @NumeroApolice
                    , @DataHoraContratacao
                    , @IdProposta
                    , @ValorPremioFinal
                    , @valorCoberturaFinal
                    , @EmailContratante)";

            var parametros = new DynamicParameters();
            parametros.Add("@NumeroApolice", contratacao.NumeroApolice, DbType.String, size: 30);
            parametros.Add("@DataHoraContratacao", contratacao.DataHoraContratacao, DbType.DateTime2);
            parametros.Add("@IdProposta", contratacao.IdProposta, DbType.Int32);
            parametros.Add("@ValorPremioFinal", contratacao.ValorPremioFinal, DbType.Decimal);
            parametros.Add("@ValorCoberturaFinal", contratacao.ValorCoberturaFinal, DbType.Decimal);
            parametros.Add("@EmailContratante", contratacao.EmailContratante, DbType.String, size: 100);

            using var conexao = _connectionFactory.CreateConnection();

            var linhasAfetadas = await conexao.ExecuteAsync(query, parametros, commandType: CommandType.Text);

            return linhasAfetadas > 0;
        }
    }
}
