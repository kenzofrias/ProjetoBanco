using System;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Services;

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

        if (contaDestino.Status != Status.Ativa)
            throw new ContaInativaException("[ERRO] Conta inativa. Não é possível receber a transferência.");

        contaOrigem.Debitar(valor, TipoOperacao.TransferenciaEnviada);
        contaDestino.Creditar(valor, TipoOperacao.TransferenciaRecebida);

        await _contaRepositorio.AtualizarContaAsync(contaOrigem);
        await _contaRepositorio.AtualizarContaAsync(contaDestino);
        await _contaRepositorio.SalvarAlteracoesAsync();
    }
}
