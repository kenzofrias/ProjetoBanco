using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_de_banco.Exceptions
{
    public class ContaInativa : Exception
    {
        public ContaInativa() : base("[ERRO] Conta inativa. Não é possível realizar operações."){}
        public ContaInativa(string message) : base(message){}
    }
}