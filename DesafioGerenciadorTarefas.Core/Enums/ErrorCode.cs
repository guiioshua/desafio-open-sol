using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioGerenciadorTarefas.Core.Enums
{
    public enum ErrorCode
    {
        TitleRequired,
        CompletedTaskCannotBeChanged,
        InvalidStateTransitionToInProgress,
        InvalidStateTransitionToDone,
        TaskNotFound
    }
}