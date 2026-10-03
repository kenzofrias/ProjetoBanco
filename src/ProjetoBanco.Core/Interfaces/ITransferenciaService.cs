using System;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Interfaces;

public interface ITransferenciaService
{
    Task Transferir(ContaId origemId, ContaId destinoId, decimal valor);
    Task RealizarTransferencia(Conta contaOrigem, decimal valor);
    Task ReceberTransferencia(Conta contaDestino, decimal valor);
}
