using Dapper;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.Models;
using SeguroContratacao.Infraestructure.Interfaces;
using System.Data;

namespace SeguroContratacao.Infraestructure.Repositories
{
    public class ContratacaoRepository : IContratacaoRepository
    {
        private readonly IDbContextFactory _connectionFactory;

        public ContratacaoRepository(IDbContextFactory connectionFactory)
        {
            _connectionFactory = connectionFactory
                ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<int> InserirAsync(Contratacao contratacao)
        {
            const string query = @"
                INSERT INTO CONTRATACAO (
                    NUMERO_APOLICE
                    , DATAHORA_CONTRATACAO
                    , ID_PROPOSTA
                    , VALOR_PREMIO_FINAL
                    , VALOR_COBERTURA_FINAL)
                VALUES (
                    @NumeroApolice
                    , @DataContratacao
                    , @IdProposta
                    , @ValorPremioFinal
                    , @valorCoberturaFinal)";

            var parametros = new DynamicParameters();
            parametros.Add("@NumeroApolice", contratacao.NumeroApolice, DbType.String, size: 30);
            parametros.Add("@DataHoraContratacao", contratacao.DataHoraContratacao, DbType.DateTime2);
            parametros.Add("@IdProposta", contratacao.IdProposta, DbType.Int32);
            parametros.Add("@ValorPremioFinal", contratacao.ValorPremioFinal, DbType.Decimal);
            parametros.Add("@ValorCoberturaFinal", contratacao.ValorCoberturaFinal, DbType.Decimal);

            using var conexao = _connectionFactory.CreateConnection();

            var id = await conexao.ExecuteAsync(query, parametros, commandType: CommandType.Text);

            return id;
        }
    }
}
