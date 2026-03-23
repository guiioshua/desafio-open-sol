using DesafioGerenciadorTarefas.Core.Enums;
using DesafioGerenciadorTarefas.Core.Exceptions;
using System.Text.Json;

namespace DesafioGerenciadorTarefas.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        // TODO: Fere single responsibility
        private static readonly Dictionary<ErrorCode, string> _errorMessages = new()
        {
            { ErrorCode.TitleRequired, "O título da tarefa é obrigatório." },
            { ErrorCode.CompletedTaskCannotBeChanged, "Tarefas finalizadas (Done) não permitem alterações de status." },
            { ErrorCode.InvalidStateTransitionToInProgress, "Fluxo inválido: Tarefas pendentes só podem ser alteradas para Em Andamento." },
            { ErrorCode.InvalidStateTransitionToDone, "Fluxo inválido: Apenas tarefas Em Andamento podem ser alteradas para Concluído." },
            { ErrorCode.TaskNotFound, "A tarefa informada não foi encontrada." }
        };

        // TODO: Fere single responsibility
        private static readonly Dictionary<ErrorCode, int> _errorStatusCodes = new()
        {
            { ErrorCode.TitleRequired, StatusCodes.Status422UnprocessableEntity },
            { ErrorCode.CompletedTaskCannotBeChanged, StatusCodes.Status409Conflict },
            { ErrorCode.InvalidStateTransitionToInProgress, StatusCodes.Status409Conflict },
            { ErrorCode.InvalidStateTransitionToDone, StatusCodes.Status409Conflict },
            { ErrorCode.TaskNotFound, StatusCodes.Status404NotFound }
        };

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                await HandleDomainExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleUnexpectedExceptionAsync(context, ex);
            }
        }

        private static Task HandleDomainExceptionAsync(HttpContext context, DomainException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = _errorStatusCodes.GetValueOrDefault(ex.Code, StatusCodes.Status422UnprocessableEntity);

            var message = _errorMessages.GetValueOrDefault(ex.Code, "Erro de validação desconhecido.");

            var response = new
            {
                error = message,
                code = ex.Code.ToString()
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static Task HandleUnexpectedExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new { error = "Ocorreu um erro interno no servidor." };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
