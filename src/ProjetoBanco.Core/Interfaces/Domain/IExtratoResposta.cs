using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Interfaces
{
    public interface IExtratoResposta
    {
        ExtratoId Id { get; }
        ContaId ContaId { get; }
        string NumeroConta { get; }
        decimal SaldoAnterior { get; }
        decimal SaldoAtual { get; }
    }
}