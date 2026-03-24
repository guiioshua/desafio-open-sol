using System;
using DesafioGerenciadorTarefas.Core.Enums;
using DesafioGerenciadorTarefas.Core.Exceptions;

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

        private TaskItem() { }

        public TaskItem(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException(ErrorCode.TitleRequired);
                
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
                throw new DomainException(ErrorCode.TitleRequired);
                
            Title = title;
            Description = description;
            LastUpdate = DateTime.UtcNow;
        }

        public void UpdateStatus(StatusTask newStatus)
        {
            if (Status == StatusTask.Done)
                throw new DomainException(ErrorCode.CompletedTaskCannotBeChanged); 

            if (Status == StatusTask.Pending && newStatus != StatusTask.InProgress)
                throw new DomainException(ErrorCode.InvalidStateTransitionToInProgress); 

            if (Status == StatusTask.InProgress && newStatus != StatusTask.Done)
                throw new DomainException(ErrorCode.InvalidStateTransitionToDone); 

            Status = newStatus;
            LastUpdate = DateTime.UtcNow;
        }
    }
}