using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_de_banco.Exceptions
{
    public class SaldoInsuficiente : Exception
    {
        public SaldoInsuficiente() : base("[ERRO] Saldo insuficiente. Não é possível realizar operações."){}
        public SaldoInsuficiente(string message) : base(message){}
    }
}