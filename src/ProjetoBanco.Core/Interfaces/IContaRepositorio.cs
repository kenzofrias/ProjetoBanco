using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Models;

namespace ProjetoBanco.Core.Interfaces
{
    public interface IContaRepositorio
    {
        Task<Conta?> ObterContaPorNumeroAsync(string numero);
        Task<IEnumerable<Conta?>> ObterTodasContasAsync();
        Task<IEnumerable<Conta?>> ObterTodasContasCorrenteAsync();
        Task<IEnumerable<Conta?>> ObterTodasContasPoupançaAsync();
        Task AdicionarContaAsync(Conta conta);
        Task AtualizarContaAsync(Conta conta);
        Task RemoverContaAsync(string numeroConta);
    }
}