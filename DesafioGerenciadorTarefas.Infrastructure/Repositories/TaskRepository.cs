using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DesafioGerenciadorTarefas.Core.Entities;
using DesafioGerenciadorTarefas.Core.Enums;
using DesafioGerenciadorTarefas.Core.Interfaces;
using DesafioGerenciadorTarefas.Infrastructure.Data;

namespace DesafioGerenciadorTarefas.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, StatusTask? statusFilter = null)
        {
            var query = _context.Tasks.AsQueryable();

            if (statusFilter.HasValue)
            {
                query = query.Where(t => t.Status == statusFilter.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.CreatedTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}