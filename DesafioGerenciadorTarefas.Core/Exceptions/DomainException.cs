using DesafioGerenciadorTarefas.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioGerenciadorTarefas.Core.Exceptions
{
    public class DomainException : Exception
    {
        public ErrorCode Code { get; }
        public DomainException(ErrorCode code) : base(code.ToString()) 
        {
            Code = code;
        }
    }
}
