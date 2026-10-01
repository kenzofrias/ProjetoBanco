using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoBanco.Core.Interfaces
{
    public interface IHistoricoResposta
    {
        int Id { get; }
        string NumeroConta { get; }
        DateTime Data { get; }
        string Operacao { get; }
        decimal Valor { get; }
        decimal SaldoAnterior { get; }
        decimal SaldoAtual { get; }
    }
}