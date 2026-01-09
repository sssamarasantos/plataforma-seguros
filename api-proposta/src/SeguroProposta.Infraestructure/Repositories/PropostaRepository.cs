using Dapper;
using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.Interfaces;
using SeguroProposta.Domain.Models;
using SeguroProposta.Infraestructure.Interfaces;
using System.Data;

namespace SeguroProposta.Infraestructure.Repositories
{
    public class PropostaRepository : IPropostaRepository
    {
        private readonly IDbContextFactory _connectionFactory;

        public PropostaRepository(IDbContextFactory connectionFactory)
        {
            _connectionFactory = connectionFactory
                ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task InserirAsync(Proposta proposta)
        {
            ArgumentNullException.ThrowIfNull(proposta);

            const string query = @"
                INSERT INTO PROPOSTA (
                    ID
                    , STATUS 
                    , TITULO 
                    , DESCRICAO 
                    , DATA_INCLUSAO
                    , VALOR_PREMIO
                    , VALOR_COBERTURA)
                VALUES (
                    @Id
                    , @Status
                    , @Titulo
                    , @Descricao
                    , @DataInclusao
                    , @ValorPremio
                    , @ValorCobertura)";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", proposta.Id, DbType.Guid);
            dynamicParameters.Add("@Status", proposta.Status.ToString(), DbType.String, size: 15);
            dynamicParameters.Add("@Titulo", proposta.Titulo, DbType.String, size: 100);
            dynamicParameters.Add("@Descricao", proposta.Descricao, DbType.String, size: 255);
            dynamicParameters.Add("@DataInclusao", proposta.DataInclusao, dbType: DbType.DateTime);
            dynamicParameters.Add("@ValorPremio", proposta.ValorPremio, DbType.Decimal);
            dynamicParameters.Add("@ValorCobertura", proposta.ValorCobertura, DbType.Decimal);

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(query, param: dynamicParameters, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<Proposta>> BuscarTodasAsync()
        {
            const string query = @"
                SELECT ID as Id
                    , NUMERO_PROPOSTA as NumeroProposta
                    , STATUS as Status
                    , TITULO as Titulo
                    , DESCRICAO as Descricao
                    , DATA_INCLUSAO as DataInclusao
                    , VALOR_PREMIO as ValorPremio
                    , VALOR_COBERTURA as ValorCobertura
                FROM PROPOSTA";

            using var connection = _connectionFactory.CreateConnection();
            var propostas = await connection.QueryAsync<Proposta>(query, commandType: CommandType.Text);

            return propostas;
        }

        public async Task<Proposta?> BuscarPorNumeroPropostaAsync(int numeroProposta)
        {
            const string query = "SELECT Status FROM PROPOSTA WHERE NUMERO_PROPOSTA = @NumeroProposta";
            
            var parametros = new DynamicParameters();
            parametros.Add("@NumeroProposta", numeroProposta, DbType.String);
            
            using var connection = _connectionFactory.CreateConnection();
            
            var proposta = await connection.QueryFirstOrDefaultAsync<Proposta>(query, parametros, commandType: CommandType.Text);
            return proposta;
        }

        public async Task AtualizaStatusAsync(int numeroProposta, StatusProposta statusProposta)
        {
            const string query = "UPDATE PROPOSTA SET STATUS = @Status WHERE NUMERO_PROPOSTA = @NumeroProposta";

            var parametros = new DynamicParameters();
            parametros.Add("@Status", statusProposta.ToString(), DbType.String, size: 15);
            parametros.Add("@NumeroProposta", numeroProposta, DbType.Int32);

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(query, parametros, commandType: CommandType.Text);
        }
    }
}
