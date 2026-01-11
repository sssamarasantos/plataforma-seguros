using Dapper;
using SeguroProposta.Domain.Enums;
using SeguroProposta.Domain.Interfaces;
using SeguroProposta.Domain.Models;
using SeguroProposta.Infrastructure.Interfaces;
using System.Data;

namespace SeguroProposta.Infrastructure.Repositories
{
    public class PropostaRepository : IPropostaRepository
    {
        private readonly IDbContextFactory _connectionFactory;

        public PropostaRepository(IDbContextFactory connectionFactory)
        {
            _connectionFactory = connectionFactory
                ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<bool> InserirAsync(Proposta proposta)
        {
            ArgumentNullException.ThrowIfNull(proposta);

            const string query = @"
                INSERT INTO PROPOSTA (
                    NUMERO_PROPOSTA
                    , STATUS 
                    , TITULO 
                    , DESCRICAO 
                    , DATAHORA_INCLUSAO         
                    , VALOR_PREMIO
                    , VALOR_COBERTURA
                    , EMAIL_CONTRATANTE)
                VALUES (
                    @NumeroProposta
                    , @Status
                    , @Titulo
                    , @Descricao
                    , @DataHoraInclusao
                    , @ValorPremio
                    , @ValorCobertura
                    , @EmailContratante)";

            var parametros = new DynamicParameters();
            parametros.Add("@NumeroProposta", proposta.NumeroProposta, DbType.String, size: 30);
            parametros.Add("@Status", proposta.Status.ToString(), DbType.String, size: 15);
            parametros.Add("@Titulo", proposta.Titulo, DbType.String, size: 100);
            parametros.Add("@Descricao", proposta.Descricao, DbType.String, size: 255);
            parametros.Add("@DataHoraInclusao", proposta.DataHoraInclusao, dbType: DbType.DateTime2);
            parametros.Add("@ValorPremio", proposta.ValorPremio, DbType.Decimal);
            parametros.Add("@ValorCobertura", proposta.ValorCobertura, DbType.Decimal);
            parametros.Add("@EmailContratante", proposta.EmailContratante, DbType.String, size: 100);

            using var connection = _connectionFactory.CreateConnection();

            var rowsAffected = await connection.ExecuteAsync(query, param: parametros, commandType: CommandType.Text);
            
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Proposta>> BuscarTodasAsync()
        {
            const string query = @"
                SELECT ID as Id
                    , NUMERO_PROPOSTA as NumeroProposta
                    , STATUS as Status
                    , TITULO as Titulo
                    , DESCRICAO as Descricao
                    , DATAHORA_INCLUSAO as DataHoraInclusao
                    , VALOR_PREMIO as ValorPremio
                    , VALOR_COBERTURA as ValorCobertura
                    , EMAIL_CONTRATANTE as EmailContratante
                FROM PROPOSTA";

            using var connection = _connectionFactory.CreateConnection();
            var propostas = await connection.QueryAsync<Proposta>(query, commandType: CommandType.Text);

            return propostas;
        }

        public async Task<Proposta?> BuscarPorIdAsync(int id)
        {
            const string query = @"
                SELECT 
                    ID as Id
                    , NUMERO_PROPOSTA as NumeroProposta
                    , STATUS as Status
                    , TITULO as Titulo
                    , DESCRICAO as Descricao
                    , DATAHORA_INCLUSAO as DataHoraInclusao
                    , VALOR_PREMIO as ValorPremio
                    , VALOR_COBERTURA as ValorCobertura
                    , EMAIL_CONTRATANTE as EmailContratante
                FROM PROPOSTA 
                WHERE ID = @Id";
            
            var parametros = new DynamicParameters();
            parametros.Add("@Id", id, DbType.Int32);
            
            using var connection = _connectionFactory.CreateConnection();
            
            var proposta = await connection.QueryFirstOrDefaultAsync<Proposta>(query, parametros, commandType: CommandType.Text);
            return proposta;
        }

        public async Task<bool> AtualizaStatusAsync(int id, StatusProposta statusProposta)
        {
            const string query = "UPDATE PROPOSTA SET STATUS = @Status WHERE ID = @Id";

            var parametros = new DynamicParameters();
            parametros.Add("@Status", statusProposta.ToString(), DbType.String, size: 15);
            parametros.Add("@Id", id, DbType.Int32);

            using var connection = _connectionFactory.CreateConnection();

            var rowsAffected = await connection.ExecuteAsync(query, parametros, commandType: CommandType.Text);
            
            return rowsAffected > 0;
        }
    }
}
