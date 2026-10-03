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
        Task<bool> ContaExisteAsync(ContaId contaId);
        Task<Conta?> ObterContaPorIdAsync(ContaId contaId);
        Task<IEnumerable<Conta?>> ObterTodasContasAsync();
        Task<IEnumerable<Conta?>> ObterTodasContasCorrenteAsync();
        Task<IEnumerable<Conta?>> ObterTodasContasPoupançaAsync();
        Task AdicionarContaAsync(Conta conta);
        Task AtualizarContaAsync(Conta conta);
        Task RemoverContaAsync(string numeroConta);
    }
}