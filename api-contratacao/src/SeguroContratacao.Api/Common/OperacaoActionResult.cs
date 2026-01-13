using Microsoft.AspNetCore.Mvc;
using SeguroContratacao.Domain.Common;

namespace SeguroContratacao.Api.Common
{
    public class OperacaoActionResult : IActionResult
    {
        private readonly ResultadoOperacao _result;
        private readonly Func<IActionResult>? _onSuccess;

        private OperacaoActionResult(ResultadoOperacao result, Func<IActionResult>? onSuccess = null)
        {
            _result = result;
            _onSuccess = onSuccess;
        }

        public static implicit operator OperacaoActionResult(ResultadoOperacao result)
        {
            return new OperacaoActionResult(result);
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            IActionResult actionResult;

            if (_result.EhFalha)
            {
                actionResult = OperacaoActionResultHelper.HandleFailure(_result);
            }
            else
            {
                actionResult = _onSuccess?.Invoke() ?? new OkResult();
            }

            return actionResult.ExecuteResultAsync(context);
        }
    }

    public class OperacaoActionResult<T> : IActionResult
    {
        private readonly ResultadoOperacao<T> _result;
        private readonly Func<T, IActionResult>? _onSuccess;

        private OperacaoActionResult(ResultadoOperacao<T> result, Func<T, IActionResult>? onSuccess = null)
        {
            _result = result;
            _onSuccess = onSuccess;
        }

        public static implicit operator OperacaoActionResult<T>(ResultadoOperacao<T> result)
        {
            return new OperacaoActionResult<T>(result);
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            IActionResult actionResult;

            if (_result.EhFalha)
            {
                actionResult = OperacaoActionResultHelper.HandleFailure(_result);
            }
            else
            {
                actionResult = _onSuccess?.Invoke(_result.Valor) ?? new OkObjectResult(_result.Valor);
            }

            return actionResult.ExecuteResultAsync(context);
        }
    }

    internal static class OperacaoActionResultHelper
    {
        public static ObjectResult HandleFailure(ResultadoOperacao result)
        {
            return result.Erro?.TipoErro switch
            {
                TipoErro.NaoEncontrado => new NotFoundObjectResult(new { result.Erro.Mensagem }),
                TipoErro.Validacao => new BadRequestObjectResult(new { result.Erro.Mensagem }),
                TipoErro.RegraDeNegocio => new UnprocessableEntityObjectResult(new { result.Erro.Mensagem }),
                TipoErro.ErroOperacional => new ObjectResult(new { result.Erro.Mensagem }) { StatusCode = 500 },
                _ => new ObjectResult(new { Mensagem = result.Erro?.Mensagem ?? "Ocorreu um erro desconhecido." }) { StatusCode = 500 }
            };
        }
    }
}
