using DesafioGerenciadorTarefas.Core.Entities;
using DesafioGerenciadorTarefas.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioGerenciadorTarefas.Core.Interfaces
{
    public interface ITaskRepository
    {
        // Task se refere Task do .NET, não TaskItem
        Task AddAsync(TaskItem task);
        Task<TaskItem?> GetByIdAsync(Guid id);

        // Paginação por offset
        Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, StatusTask? statusFilter = null);

        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(TaskItem task);
    }
}
