using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Models;

namespace ProjetoBanco.Core.Interfaces
{
    public interface IConta
    {
        string Numero { get; }
        string Titular { get; }
        decimal Saldo { get; }
        bool Ativa { get; }
        IReadOnlyCollection<HistoricoResposta> Historico { get; }  
    }
}