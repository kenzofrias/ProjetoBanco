using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoBanco.Core.Exceptions
{
    public class ContaInativaException : Exception
    {
        public ContaInativaException() : base("[ERRO] Conta inativa. Não é possível realizar operações."){}
        public ContaInativaException(string message) : base(message){}
    }
}