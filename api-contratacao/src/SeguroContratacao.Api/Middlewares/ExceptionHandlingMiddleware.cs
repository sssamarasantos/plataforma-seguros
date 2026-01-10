using SeguroContratacao.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace SeguroContratacao.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                ContratacaoInvalidaException => (HttpStatusCode.BadRequest, exception.Message),
                RegraDeNegocioException => (HttpStatusCode.UnprocessableEntity, exception.Message),
                DomainException => (HttpStatusCode.BadRequest, exception.Message),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                InvalidOperationException => (HttpStatusCode.Conflict, exception.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Recurso não encontrado."),
                _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.")
            };

            context.Response.StatusCode = (int)statusCode;

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(exception, "Erro não tratado ao processar a requisição.");
            }
            else
            {
                _logger.LogWarning("Erro de domínio: {Message}", exception.Message);
            }

            var response = new
            {
                message,
                timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
