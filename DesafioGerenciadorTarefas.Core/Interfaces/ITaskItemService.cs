using System;
using System.Threading.Tasks;
using DesafioGerenciadorTarefas.Core.DTOs;
using DesafioGerenciadorTarefas.Core.Enums;

namespace DesafioGerenciadorTarefas.Core.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponse> CreateAsync(CreateTaskRequest request);
        Task<TaskResponse> GetByIdAsync(Guid id);
        Task<PagedResponse<TaskResponse>> GetPagedAsync(int pageNumber, int pageSize, StatusTask? statusFilter);
        Task UpdateDetailsAsync(Guid id, UpdateTaskDetailsRequest request);
        Task UpdateStatusAsync(Guid id, UpdateTaskStatusRequest request);
        Task DeleteAsync(Guid id);
    }
}