using Microsoft.AspNetCore.Mvc;
using SeguroProposta.Api.Common;
using SeguroProposta.Application.Dtos;
using SeguroProposta.Application.DTOs;
using SeguroProposta.Application.Interfaces;

namespace SeguroProposta.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PropostaController : ControllerBase
    {
        private readonly ILogger<PropostaController> _logger;
        private readonly IPropostaService _propostaService;

        public PropostaController(ILogger<PropostaController> logger, IPropostaService propostaService)
        {
            _logger = logger;
            _propostaService = propostaService;
        }

        /// <summary>
        /// Retorna todas as propostas
        /// </summary>
        /// <returns>Uma coleção de objetos <see cref="PropostaDTO"/> representando todas as propostas. A coleção está vazia se nenhuma
        /// proposta for encontrada.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PropostaDTO>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PropostaDTO>> BuscarTodas()
        {
            var propostas = await _propostaService.BuscarTodasAsync();
            return propostas;
        }

        /// <summary>
        /// Recupera uma proposta pelo seu identificador único..
        /// </summary>
        /// <param name="id">O identificador único da proposta a ser recuperada.</param>
        /// <returns>Um <see cref="PropostaDTO"/> representando a proposta com o identificador especificado, ou <c>null</c> se nenhuma
        /// proposta for encontrada.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropostaDTO), StatusCodes.Status200OK)]
        public async Task<PropostaDTO?> BuscarPorId([FromRoute] int id)
        {
            var proposta = await _propostaService.BuscarPorIdAsync(id);
            return proposta;
        }

        /// <summary>
        /// Criação de proposta
        /// </summary>
        /// <param name="proposta"></param>
        /// <returns>Retorna uma resposta HTTP 201 Created se for criado com sucesso</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<OperacaoActionResult> Inserir([FromBody] CriaPropostaDTO proposta)
        {
            var resultado = await _propostaService.InserirAsync(proposta);
            return resultado;
        }

        /// <summary>
        /// Atualiza o status de uma proposta pelo número da proposta
        /// </summary>
        /// <param name="alteraStatusDTO">An object containing the information required to alter the status of the proposal. Cannot be null.</param>
        /// <returns>Retorna uma resposta HTTP 200 OK se o status for atualizado com sucesso</returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<OperacaoActionResult> AlterarStatus([FromBody] AlteraStatusDTO alteraStatusDTO)
        {
            var resultado = await _propostaService.AlterarStatusAsync(alteraStatusDTO);
            return resultado;
        }
    }
}
