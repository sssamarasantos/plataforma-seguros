using Microsoft.AspNetCore.Mvc;
using SeguroProposta.Application.Dtos;
using SeguroProposta.Application.Interfaces;
using SeguroProposta.Application.VOs;
using System.Net;

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
        /// <returns>Uma coleção de objetos <see cref="PropostaVO"/> representando todas as propostas. A coleção está vazia se nenhuma
        /// proposta for encontrada.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<PropostaVO>))]
        public async Task<IEnumerable<PropostaVO>> BuscarTodas()
        {
            var propostas = await _propostaService.BuscarTodasAsync();
            return propostas;
        }

        /// <summary>
        /// Criação de proposta
        /// </summary>
        /// <param name="proposta"></param>
        /// <returns>Retorna uma resposta HTTP 201 Created se for criado com sucesso</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Inserir(PropostaDTO proposta)
        {
            await _propostaService.InserirAsync(proposta);
            return Created();
        }

        /// <summary>
        /// Atualiza o status de uma proposta pelo número da proposta
        /// </summary>
        /// <param name="alteraStatusDTO">An object containing the information required to alter the status of the proposal. Cannot be null.</param>
        /// <returns>Retorna uma resposta HTTP 200 OK se o status for atualizado com sucesso</returns>
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> AlterarStatus(AlteraStatusDTO alteraStatusDTO)
        {
            await _propostaService.AlterarStatusAsync(alteraStatusDTO);
            return Ok();
        }
    }
}
