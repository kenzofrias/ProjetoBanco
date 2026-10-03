using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoBanco.Core.Exceptions
{
    public class ExtratoRespostaException : Exception
    {
        public ExtratoRespostaException() : base("[ERRO] Ocorreu um erro ao processar o extrato de resposta."){}
        public ExtratoRespostaException(string message) : base(message){}
    }
}