// 1. DesafioGerenciadorTarefas.Core/DTOs/TaskDTOs.cs
using System;
using System.Collections.Generic;
using DesafioGerenciadorTarefas.Core.Enums;

namespace DesafioGerenciadorTarefas.Core.DTOs
{
    public record CreateTaskRequest(string Title, string Description);

    public record UpdateTaskDetailsRequest(string Title, string Description);

    public record UpdateTaskStatusRequest(StatusTask Status);

    public record TaskResponse(
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