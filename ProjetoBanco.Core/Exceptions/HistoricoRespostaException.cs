using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoBanco.Core.Exceptions
{
    public class HistoricoRespostaException : Exception
    {
        public HistoricoRespostaException() : base("[ERRO] Ocorreu um erro ao processar o histórico de resposta."){}
        public HistoricoRespostaException(string message) : base(message){}
    }
}