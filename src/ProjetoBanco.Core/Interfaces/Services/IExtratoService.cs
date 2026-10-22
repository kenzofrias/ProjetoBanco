using System;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Interfaces;

public interface IExtratoService
{
    Task<ExtratoResposta> AdicionarMovimentacao(ContaId contaId, ExtratoResposta extratoResposta);
    Task<string> GerarExtrato(ContaId contaId);
    Task<string> ExibirExtrato(ContaId contaId);
}
