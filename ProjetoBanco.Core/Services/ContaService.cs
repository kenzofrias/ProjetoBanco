using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.Models;

namespace ProjetoBanco.Core.Services
{
    public abstract class ContaService
    {
        private readonly Conta _conta;
        public ContaService(Conta conta)
        {
            _conta = conta;
        }
        public virtual void Depositar(decimal valor) 
        {
            if (_conta.Ativa && valor > 0)
            {
                _conta.Saldo += valor;
                _conta.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Deposito, valor, _conta.Saldo - valor, _conta.Saldo)); 
            } else if (_conta.Ativa && valor > 0)
            {
                _conta.Saldo += valor;
                _conta.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaRecebida, valor, _conta.Saldo - valor, _conta.Saldo));
            }
            else if (_conta.Ativa && valor <= 0)
            {
                throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.");
            }
            else
            {
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar o depósito.");
            }
        }

        public virtual void Sacar(decimal valor)
        {
            if (_conta.Ativa && valor > 0 && _conta.Saldo >= valor)
            {
                _conta.Saldo -= valor;
                _conta.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Saque, valor, _conta.Saldo + valor, _conta.Saldo));
            }
            else if (_conta.Ativa && valor > 0 && _conta.Saldo < valor)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar o saque.");
            }
            else if (_conta.Ativa && valor <= 0)
            {
                throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar o saque.");
            }
            else
            {
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar o saque.");
            }
        }

        public virtual void Transferir(Conta destino, decimal valor)
        {
            if (_conta.Ativa && valor > 0 && _conta.Saldo >= valor)
            {
                _conta.Saldo -= valor;
                destino.Saldo += valor;
                
                _conta.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaEnviada, valor, _conta.Saldo + valor, _conta.Saldo));
                destino.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaRecebida, valor, destino.Saldo - valor, destino.Saldo));
            }
            else if (_conta.Ativa && valor > 0 && _conta.Saldo < valor)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar a transferência.");
            }
            else if (_conta.Ativa && valor <= 0)
            {
                throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar a transferência.");
            }
            else
            {
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar a transferência.");
            }
        }

        public void ExibirExtrato()
        {
            Console.WriteLine($"\nExtrato de {_conta.GetType().Name} | Titular: {_conta.Titular} | Saldo: {_conta.Saldo:C}");
            Console.WriteLine("Lançamentos:");

            if (!_conta.Historico.Any())
            {
                Console.WriteLine("Nenhuma transação realizada.");
            }
            else
            {
                foreach (var item in _conta.Historico)
                {
                    Console.WriteLine(item);
                }
            }
        }

        public decimal ObterSaldo() => Math.Round(_conta.Saldo, 2);

        public override string ToString() => $"Titular: {_conta.Titular} | Saldo: {_conta.Saldo:C}";

        public virtual void CalcularTarifaMensal() { }
        public virtual void AplicarRendimento() { }
    }
}