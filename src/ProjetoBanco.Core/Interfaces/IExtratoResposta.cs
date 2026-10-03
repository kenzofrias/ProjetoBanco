using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoBanco.Core.Interfaces
{
    public interface IExtratoResposta
    {
        int Id { get; }
        string NumeroConta { get; }
        decimal SaldoAnterior { get; }
        decimal SaldoAtual { get; }
    }
}