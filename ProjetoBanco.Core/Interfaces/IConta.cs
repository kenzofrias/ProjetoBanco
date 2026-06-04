using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoBanco.Core.Interfaces
{
    public interface IConta
    {
        string Numero { get; }
        string Titular { get; }
        decimal Saldo { get; }
        bool Ativa { get; }
        IReadOnlyCollection<IHistoricoResposta> Historico { get; }  
    }
}