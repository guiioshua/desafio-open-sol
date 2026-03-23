using System;
using System.Collections.Generic;
using DesafioGerenciadorTarefas.Core.Enums;

namespace DesafioGerenciadorTarefas.Core.DTOs
{
    public record CreateTaskItemRequest(string Title, string Description);

    public record UpdateTaskItemDetailsRequest(string Title, string Description);

    public record UpdateTaskItemStatusRequest(StatusTask Status);

    public record TaskItemResponse(
        Guid Id,
        string Title,
        string Description,
        string Status,
        DateTime CreatedTime,
        DateTime LastUpdate);

    public record PagedResponse<T>(
        IEnumerable<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize);
}