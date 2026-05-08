using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_de_banco.Exceptions
{
    public class ValorNegativo : Exception
    {
        public ValorNegativo() : base("[ERRO] Valor deve ser positivo."){}
        public ValorNegativo(string message) : base(message){}
    }
}