using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_de_banco.Exceptions
{
    public class SaldoInsuficienteException : Exception
    {
        public SaldoInsuficienteException() : base("[ERRO] Saldo insuficiente. Não é possível realizar operações."){}
        public SaldoInsuficienteException(string message) : base(message){}
    }
}