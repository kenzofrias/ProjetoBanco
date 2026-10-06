using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.ValueObjects;
using ProjetoBanco.Infrastructure.Data;

namespace ProjetoBanco.Infrastructure.Repositories
{
    public class ContaRepositorio : IContaRepositorio
    {
        private readonly BancoDBContext _context;
        public ContaRepositorio(BancoDBContext context)
        {
            _context = context;
        }

        public async Task<bool> ContaExisteAsync(ContaId contaId)
        {
            return await _context.Contas.AnyAsync(c => c.Id == contaId);
        }

        public async Task AdicionarContaAsync(Conta conta)
        {
            if (conta == null)
            {
                throw new ArgumentNullException(nameof(conta));
            }

            _context.Contas.Add(conta);
        }

        public async Task AtualizarContaAsync(Conta conta)
        {
            var contaExiste = await ContaExisteAsync(conta.Id);
            if (!contaExiste)
            {
                throw new KeyNotFoundException($"[ERRO] A conta de número {conta.Id} não foi encontrada para atualização.");
            }
            
            _context.Contas.Update(conta);
        }

        public async Task<Conta?> ObterContaPorIdAsync(ContaId contaId)
        {
            // Realiza apenas 1 viagem ao banco de dados.
            var conta = await _context.Contas
                .Include(c => c.Historico)
                .FirstOrDefaultAsync(c => c.Id == contaId);

            return conta;
        }

        public async Task<IEnumerable<Conta?>> ObterTodasContasAsync()
        {
            var contas = await _context.Contas.ToListAsync();
            return contas;
        }

        public async Task<IEnumerable<Conta?>> ObterTodasContasCorrenteAsync()
        {
            var contasCorrente = await _context.ContasCorrentes.ToListAsync();
            return contasCorrente;
        }

        public async Task<IEnumerable<Conta?>> ObterTodasContasPoupançaAsync()
        {
            var contasPoupanca = await _context.ContasPoupancas.ToListAsync();
            return contasPoupanca;
        }

        public async Task RemoverContaAsync(ContaId contaId)
        {
            var contaRemover = await ObterContaPorIdAsync(contaId);

            _context.Contas.Remove(contaRemover);
            // A responsabilidade de salvar foi movida para a camada de serviço/aplicação.
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}