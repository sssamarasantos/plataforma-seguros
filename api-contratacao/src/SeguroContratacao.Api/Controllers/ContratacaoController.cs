using Microsoft.AspNetCore.Mvc;
using SeguroContratacao.Api.Common;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<OperacaoActionResult> ContratarProposta(ContratacaoDTO contratacaoDto)
        {
            var resultado = await _contratacaoService.ContratarPropostaAsync(contratacaoDto);
            return resultado;
        }
    }
}
