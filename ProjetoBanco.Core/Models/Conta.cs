using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;

namespace ProjetoBanco.Core.Models
{
    public abstract class Conta : IConta
    {
        private readonly Stack<IHistoricoResposta> _historico;
        private decimal _saldo;

        public string Numero { get; } = string.Empty;
        public string Titular { get; } = string.Empty;
        public bool Ativa { get; } = false;
        public decimal Saldo => _saldo;
        public IReadOnlyCollection<IHistoricoResposta> Historico => _historico;

        public Conta() { _historico  = new Stack<IHistoricoResposta>(); }
        public Conta(string numero, string titular, decimal saldo)
        {
            Numero = numero;
            Titular = titular;
            _saldo = saldo;
            Ativa = true;
            _historico = new Stack<IHistoricoResposta>();
        }

        public void AumentarSaldo(decimal valor)
        {
            _saldo += valor;
        }

        public void DiminuirSaldo(decimal valor)
        {
            _saldo -= valor;
        }

        public virtual void Depositar(decimal valor) 
        {
            if (Ativa && valor > 0)
            {
                AumentarSaldo(valor);
                AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Deposito, valor, Saldo - valor, Saldo)); 
            }
            else if (Ativa && valor <= 0)
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
            if (Ativa && valor > 0 && Saldo >= valor)
            { 
                DiminuirSaldo(valor);
                AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Saque, valor, Saldo + valor, Saldo));
            }
            else if (Ativa && Saldo < valor)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar o saque.");
            }
            else if (Ativa && valor <= 0)
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
            if (Ativa && valor > 0 && Saldo >= valor)
            {
                DiminuirSaldo(valor);
                destino.Depositar(valor);

                AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaEnviada, valor, Saldo + valor, Saldo));
                destino.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaRecebida, valor, destino.Saldo - valor, destino.Saldo));
            }
            else if (Ativa && Saldo < valor)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar a transferência.");
            }
            else if (Ativa && valor <= 0)
            {
                throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar a transferência.");
            }
            else
            {
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar a transferência.");
            }
        }
        
        public IHistoricoResposta AdicionarMovimentacaoHistorico(IHistoricoResposta historicoResposta)
        {
            if(historicoResposta == null)
                throw new HistoricoRespostaException("[ERRO] O histórico de resposta não pode ser nulo.");
            _historico.Push(historicoResposta);
            return historicoResposta;
        }

        public void ExibirExtrato()
        {
            Console.WriteLine($"\nExtrato de {(GetType().Name == "ContaPoupanca" ? "Conta Poupança" : "Conta Corrente")} | Titular: {Titular} | Saldo: {Saldo:C}");

            if (!Historico.Any())
            {
                Console.WriteLine("Nenhuma transação realizada.");
            }
            else
            {
                foreach (var item in Historico)
                {
                    Console.WriteLine(item);
                }
            }
        }

        public override string ToString() => $"Titular: {Titular} | Saldo: {Saldo:C}";
        public virtual void CalcularTarifaMensal() { }
        public virtual void AplicarRendimento() { }
    }
}