using System;
using DesafioGerenciadorTarefas.Core.Enums;

namespace DesafioGerenciadorTarefas.Core.Entities
{
    public class TaskItem 
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public StatusTask Status { get; private set; }
        public DateTime CreatedTime { get; private set; }
        public DateTime LastUpdate { get; private set; }

        public TaskItem(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));
                
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Status = StatusTask.Pending; 
            CreatedTime = DateTime.UtcNow;
            LastUpdate = DateTime.UtcNow;
        }

        public void UpdateDetails(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));
                
            Title = title;
            Description = description;
            LastUpdate = DateTime.UtcNow;
        }

        public void UpdateStatus(StatusTask newStatus)
        {
            if (Status == StatusTask.Done)
                throw new InvalidOperationException("Cannot change the status of a completed task."); 

            if (Status == StatusTask.Pending && newStatus != StatusTask.InProgress)
                throw new InvalidOperationException("Invalid state transition. Pending tasks can only be changed to InProgress."); 

            if (Status == StatusTask.InProgress && newStatus != StatusTask.Done)
                throw new InvalidOperationException("Invalid state transition. InProgress tasks can only be changed to Done."); 

            Status = newStatus;
            LastUpdate = DateTime.UtcNow;
        }
    }
}