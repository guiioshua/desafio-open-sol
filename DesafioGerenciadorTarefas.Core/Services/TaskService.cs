// 3. DesafioGerenciadorTarefas.Core/Services/TaskService.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using DesafioGerenciadorTarefas.Core.DTOs;
using DesafioGerenciadorTarefas.Core.Entities;
using DesafioGerenciadorTarefas.Core.Enums;
using DesafioGerenciadorTarefas.Core.Exceptions;
using DesafioGerenciadorTarefas.Core.Interfaces;

namespace DesafioGerenciadorTarefas.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskResponse> CreateAsync(CreateTaskRequest request)
        {
            var task = new TaskItem(request.Title, request.Description);

            await _repository.AddAsync(task);

            return MapToResponse(task);
        }

        public async Task<TaskResponse> GetByIdAsync(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                throw new DomainException(ErrorCode.TaskNotFound);

            return MapToResponse(task);
        }

        public async Task<PagedResponse<TaskResponse>> GetPagedAsync(int pageNumber, int pageSize, StatusTask? statusFilter)
        {
            var (items, totalCount) = await _repository.GetPagedAsync(pageNumber, pageSize, statusFilter);

            var responseItems = items.Select(MapToResponse);

            return new PagedResponse<TaskResponse>(responseItems, totalCount, pageNumber, pageSize);
        }

        public async Task UpdateDetailsAsync(Guid id, UpdateTaskDetailsRequest request)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                throw new DomainException(ErrorCode.TaskNotFound);

            task.UpdateDetails(request.Title, request.Description);

            await _repository.UpdateAsync(task);
        }

        public async Task UpdateStatusAsync(Guid id, UpdateTaskStatusRequest request)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                throw new DomainException(ErrorCode.TaskNotFound);

            task.UpdateStatus(request.Status);

            await _repository.UpdateAsync(task);
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                throw new DomainException(ErrorCode.TaskNotFound);

            await _repository.DeleteAsync(task);
        }

        private static TaskResponse MapToResponse(TaskItem task)
        {
            return new TaskResponse(
                task.Id,
                task.Title,
                task.Description,
                task.Status.ToString(),
                task.CreatedTime,
                task.LastUpdate
            );
        }
    }
}