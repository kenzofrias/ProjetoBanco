using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Interfaces
{
    public interface IConta
    {
        ContaId Id { get; } // Chave primária futura
        string NumeroConta { get; }
        string Agencia { get; }
        TipoConta TipoConta { get; }
        Status Status { get; }
        decimal Saldo { get; }
        decimal LimiteEspecial { get; }
        DateTime DataAbertura { get; }
        UltimaMovimentacao? UltimaMovimentacao { get; }
    }
}