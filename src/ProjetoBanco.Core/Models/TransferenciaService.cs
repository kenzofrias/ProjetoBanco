using System;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Models;

public class TransferenciaService : ITransferenciaService
{
    private readonly IContaRepositorio _contaRepositorio;
    public TransferenciaService(IContaRepositorio contaRepositorio)
    {
        _contaRepositorio = contaRepositorio;
    }

    public async Task Transferir(ContaId origemId, ContaId destinoId, decimal valor)
    {
        var contaOrigem = await _contaRepositorio.ObterContaPorIdAsync(origemId);
        var contaDestino = await _contaRepositorio.ObterContaPorIdAsync(destinoId);

        if (contaOrigem == null || contaDestino == null)
            throw new Exception("[ERRO] Uma das contas não foi encontrada.");

        await RealizarTransferencia(contaOrigem, valor);
        await ReceberTransferencia(contaDestino, valor);

        await _contaRepositorio.AtualizarContaAsync(contaOrigem);
        await _contaRepositorio.AtualizarContaAsync(contaDestino);
    }

    public async Task RealizarTransferencia(Conta contaOrigem, decimal valor)
    {
        var origem = contaOrigem;
        
        if (origem == null)
            throw new Exception("[ERRO] A conta de origem não foi encontrada.");

        origem.Sacar(valor);
    }
    
    public async Task ReceberTransferencia(Conta contaDestino, decimal valor)
    {
        if (contaDestino == null)
            throw new Exception("[ERRO] A conta de destino não foi encontrada.");

        contaDestino.Depositar(valor);
    }
}
