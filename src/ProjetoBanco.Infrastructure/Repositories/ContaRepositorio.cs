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

        public async Task<bool> ContaExistePorIdAsync(ContaId contaId)
        {
            return await _context.Contas.AnyAsync(c => c.Id == contaId);
        }

        public async Task<bool> ContaExistePorNumeroEAgenciaAsync(string numeroConta, string agencia)
        {
            return await _context.Contas.AnyAsync(c => c.NumeroConta == numeroConta && c.Agencia == agencia);
        }

        public async Task AdicionarContaAsync(Conta conta)
        {
            ArgumentNullException.ThrowIfNull(conta);

            if (await ContaExistePorNumeroEAgenciaAsync(conta.NumeroConta, conta.Agencia))
            {
                throw new InvalidOperationException($"[ERRO] A conta de número {conta.NumeroConta} e agência {conta.Agencia} já existe.");
            }
            if (await ContaExistePorIdAsync(conta.Id))
            {
                throw new InvalidOperationException($"[ERRO] A conta de número {conta.Id} já existe.");
            }

            _context.Contas.Add(conta);
        }

        public async Task AtualizarContaAsync(Conta conta)
        {
            ArgumentNullException.ThrowIfNull(conta);

            var contaExiste = await ContaExistePorIdAsync(conta.Id);
            if (!contaExiste)
            {
                throw new KeyNotFoundException($"[ERRO] A conta de número {conta.Id} não foi encontrada para atualização.");
            }
            
            _context.Contas.Update(conta);
        }

        public async Task<Conta?> ObterContaPorIdAsync(ContaId contaId)
        {
            var conta = await _context.Contas
                .Include(c => c.Historico)
                .FirstOrDefaultAsync(c => c.Id == contaId);

            return conta;
        }

        public async Task<Conta?> ObterContaPorNumeroEAgenciaAsync(string numeroConta, string agencia)
        {
            var conta = await _context.Contas
                .Include(c => c.Historico)
                .FirstOrDefaultAsync(c => c.NumeroConta == numeroConta && c.Agencia == agencia);

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

            if (contaRemover is null)
            {
                throw new KeyNotFoundException($"[ERRO] A conta de número {contaId.Valor} não foi encontrada para remoção.");
            }

            _context.Contas.Remove(contaRemover);
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}