using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
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

            (HttpStatusCode statusCode, string message) resultado = exception switch
            {
                // Database Exceptions (direto do Dapper/ADO.NET)
                SqlException => (HttpStatusCode.ServiceUnavailable, "Erro ao acessar o banco de dados. Tente novamente mais tarde."),

                // HTTP Client Exceptions (para chamadas externas)
                HttpRequestException => (HttpStatusCode.BadGateway, "Erro ao comunicar com serviço externo."),
                TaskCanceledException => (HttpStatusCode.GatewayTimeout, "Timeout ao comunicar com serviço externo."),

                // Framework Exceptions
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                InvalidOperationException => (HttpStatusCode.Conflict, exception.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Recurso não encontrado."),
                ValidationException => (HttpStatusCode.BadRequest, exception.Message),

                // Unhandled Exceptions
                _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.")
            };

            var (statusCode, message) = resultado;

            context.Response.StatusCode = (int)statusCode;

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);
            }
            else if (statusCode >= HttpStatusCode.InternalServerError)
            {
                _logger.LogError(exception, "Erro de infraestrutura: {Message}", exception.Message);
            }
            else
            {
                _logger.LogWarning("Erro de negócio/validação: {Message}", exception.Message);
            }

            var response = new { Mensagem = message };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
