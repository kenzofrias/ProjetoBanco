using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoBanco.Core.Exceptions
{
    public class ValorInsuficienteException : Exception
    {
        public ValorInsuficienteException() : base("[ERRO] Valor insuficiente para realizar a operação."){}
        public ValorInsuficienteException(string message) : base(message){}
    }
}