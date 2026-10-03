using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Models
{
    public abstract class Conta : IConta
    {
        private readonly IExtratoService _extratoService;
        private readonly List<ExtratoResposta> _historico;

        public ContaId Id { get; protected set; } = new();
        public string NumeroConta { get; protected set; } = string.Empty;
        public string Agencia { get; protected set; } = string.Empty;
        public TipoConta TipoConta { get; protected set; }
        public Status Status { get; protected set; }
        public decimal Saldo { get; protected set; }
        public decimal LimiteEspecial { get; protected set; }
        public DateTime DataAbertura { get; protected set; }
        public UltimaMovimentacao? UltimaMovimentacao { get; protected set; }
        public IReadOnlyCollection<ExtratoResposta> Historico => _historico.AsReadOnly(); // Não permite adicionar ou remover elementos

        public Conta() { _historico = new List<ExtratoResposta>(); }
        public Conta(IExtratoService extratoService, string numero, string agencia, decimal saldo)
        {
            _extratoService = extratoService;

            if (string.IsNullOrWhiteSpace(numero) || numero.Length > 7)
                throw new ArgumentException("[ERRO] Número da conta inválido. Deve conter no máximo 7 caracteres.", nameof(numero));
            if (string.IsNullOrWhiteSpace(agencia) || agencia.Length > 5)
                throw new ArgumentException("[ERRO] Agência inválida. Deve conter no máximo 5 caracteres.", nameof(agencia));

            NumeroConta = numero;
            Agencia = agencia;
            Status = Status.Ativa;
            Saldo = saldo;
            DataAbertura = DateTime.UtcNow;
            _historico = new List<ExtratoResposta>();
        }

        public virtual void Depositar(decimal valor)
        {
            if (Status != Status.Ativa) throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar o depósito.");
            if (valor <= 0) throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.");

            Saldo += valor;
            UltimaMovimentacao = new UltimaMovimentacao(DateTime.UtcNow, TipoOperacao.Deposito, valor);
            // AdicionarMovimentacaoHistorico(new ExtratoResposta(Numero, TipoOperacao.Deposito, valor, Saldo - valor, Saldo));
        }

        protected virtual bool PodeRealizarOperacao(decimal valor) => Saldo >= valor;

        public virtual void Sacar(decimal valor)
        {
            if (Status != Status.Ativa) throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar o saque.");
            if (valor <= 0) throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar o saque.");
            if (!PodeRealizarOperacao(valor)) throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar o saque.");

            Saldo -= valor;
            UltimaMovimentacao = new UltimaMovimentacao(DateTime.UtcNow, TipoOperacao.Saque, valor);
            // AdicionarMovimentacaoHistorico(new HistoricoResposta(Numero, TipoOperacao.Saque, valor, Saldo + valor, Saldo));
        }

        public void AdicionarMovimentacaoExtrato(ExtratoResposta extratoResposta) => _historico.Add(extratoResposta);
        public void ExibirExtrato() => _extratoService.GerarExtrato(Id);

        public override string ToString() => $"Numero: {NumeroConta} | Agência: {Agencia} | Saldo: {Saldo:C}";
        public virtual void CalcularTarifaMensal() { }
        public virtual void AplicarRendimento() { }
    }
}