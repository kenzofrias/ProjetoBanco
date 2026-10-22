using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Interfaces
{
    public interface IContaRepositorio
    {
        Task<bool> ContaExistePorIdAsync(ContaId contaId);
        Task<bool> ContaExistePorNumeroEAgenciaAsync(string numeroConta, string agencia);
        Task<Conta?> ObterContaPorIdAsync(ContaId contaId);
        Task<Conta?> ObterContaPorNumeroEAgenciaAsync(string numeroConta, string agencia);
        Task<IEnumerable<Conta?>> ObterTodasContasAsync();
        Task<IEnumerable<Conta?>> ObterTodasContasCorrenteAsync();
        Task<IEnumerable<Conta?>> ObterTodasContasPoupançaAsync();
        Task AdicionarContaAsync(Conta conta);
        Task AtualizarContaAsync(Conta conta);
        Task RemoverContaAsync(ContaId contaId);
        Task SalvarAlteracoesAsync();
    }
}