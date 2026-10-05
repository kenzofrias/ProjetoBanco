using System;
using System.Text;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Models;

public class ExtratoService : IExtratoService
{
    private readonly IContaRepositorio _contaRepositorio;
    public ExtratoService(IContaRepositorio contaRepositorio)
    {
        _contaRepositorio = contaRepositorio;
    }


    public async Task<ExtratoResposta> AdicionarMovimentacao(ContaId contaId, ExtratoResposta extratoResposta)
    {
        bool contaExiste = await _contaRepositorio.ContaExisteAsync(contaId);
        if (!contaExiste)
        {
            throw new KeyNotFoundException($"[ERRO] A conta com ID {contaId} não foi encontrada.");
        }

        var conta = await _contaRepositorio.ObterContaPorIdAsync(contaId);

        conta.AdicionarMovimentacaoExtrato(extratoResposta);
        return extratoResposta;
    }

    public async Task<string> GerarExtrato(ContaId contaId)
    {
        bool contaExiste = await _contaRepositorio.ContaExisteAsync(contaId);
        if (!contaExiste)
        {
            throw new KeyNotFoundException($"[ERRO] A conta com ID {contaId} não foi encontrada.");
        }

        var conta = await _contaRepositorio.ObterContaPorIdAsync(contaId);

        var sb = new StringBuilder();
        string tipoConta = conta.GetType().Name == "ContaPoupanca" ? "Conta Poupança" : "Conta Corrente";

        sb.AppendLine($"\n=== Extrato de {tipoConta} ===");
        sb.AppendLine(conta.ToString());
        sb.AppendLine("- Movimentações:");

        if (!conta.Historico.Any())
        {
            sb.AppendLine("  Nenhuma transação realizada.");
        }
        else
        {
            // Ordena do mais recente para o mais antigo simulando uma Stack
            foreach (var item in conta.Historico.OrderByDescending(h => h.Data))
            {
                sb.AppendLine($"  {item.ToString()}");
            }
        }
        sb.AppendLine("=======================================");

        return sb.ToString();
    }
    
    public async Task<string> ExibirExtrato(ContaId contaId)
    {
        bool contaExiste = await _contaRepositorio.ContaExisteAsync(contaId);
        if (!contaExiste)
        {
            throw new KeyNotFoundException($"[ERRO] A conta com ID {contaId} não foi encontrada.");
        }

        var conta = await _contaRepositorio.ObterContaPorIdAsync(contaId);
        return await GerarExtrato(contaId);
    }
}