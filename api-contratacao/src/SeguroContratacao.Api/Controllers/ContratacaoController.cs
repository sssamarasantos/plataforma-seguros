using Microsoft.AspNetCore.Mvc;
using SeguroContratacao.Application.DTOs;
using SeguroContratacao.Application.Interfaces;

namespace SeguroContratacao.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContratacaoController : ControllerBase
    {
        private readonly IContratacaoService _contratacaoService;

        public ContratacaoController(IContratacaoService contratacaoService)
        {
            _contratacaoService = contratacaoService;
        }

        /// <summary>
        /// Contratar uma proposta de seguro
        /// </summary>
        /// <param name="contratacaoDto"></param>
        /// <returns>Retorna o identificador único da contratação.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> ContratarProposta(ContratacaoDTO contratacaoDto)
        {
            var id = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);
            return Ok(id);
        }
    }
}
