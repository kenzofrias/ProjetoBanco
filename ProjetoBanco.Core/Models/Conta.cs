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

        public string Numero { get; protected set; } = string.Empty;
        public string Titular { get; protected set; } = string.Empty;
        public bool Ativa { get; protected set; } = false;
        public decimal Saldo { get; protected set; }
        public IReadOnlyCollection<IHistoricoResposta> Historico => _historico;

        public Conta() { _historico  = new Stack<IHistoricoResposta>(); }
        public Conta(string numero, string titular, decimal saldo)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldo;
            Ativa = true;
            _historico = new Stack<IHistoricoResposta>();
        }

        public virtual void Depositar(decimal valor)
        {
            if (!Ativa) throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar o depósito.");
            if (valor <= 0) throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.");
            
            Saldo += valor;
            AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Deposito, valor, Saldo - valor, Saldo));
        }

        protected virtual bool PodeRealizarOperacao(decimal valor) => Saldo >= valor;

        public virtual void Sacar(decimal valor)
        {
            if (!Ativa) throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar o saque.");
            if (valor <= 0) throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar o saque.");
            if (!PodeRealizarOperacao(valor)) throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar o saque.");

            Saldo -= valor;
            AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Saque, valor, Saldo + valor, Saldo));
        }

        protected void ReceberTransferencia(decimal valor)
        {
            if (!Ativa) throw new ContaInativaException("[ERRO] Conta inativa. Não é possível receber a transferência.");
            if (valor <= 0) throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível receber a transferência.");

            Saldo += valor;
            AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaRecebida, valor, Saldo - valor, Saldo));
        }

        public virtual void Transferir(Conta destino, decimal valor)
        {
            if (!Ativa) throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar a transferência.");
            if (valor <= 0) throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar a transferência.");
            if (!PodeRealizarOperacao(valor)) throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar a transferência.");

            Saldo -= valor;
            destino.ReceberTransferencia(valor);
            AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaEnviada, valor, Saldo + valor, Saldo));
        }
        
        protected IHistoricoResposta AdicionarMovimentacaoHistorico(IHistoricoResposta historicoResposta)
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