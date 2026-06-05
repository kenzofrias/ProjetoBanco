using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;

namespace ProjetoBanco.Core.Models
{
    public class ContaCorrente : Conta
    {
        private decimal _limiteChequeEspecial;
        private decimal _taxaManutencao;

        public decimal LimiteChequeEspecial => _limiteChequeEspecial;
        public decimal SaldoDisponivel => _limiteChequeEspecial + Saldo;
        public decimal TaxaManutencao => _taxaManutencao;

        public ContaCorrente() { }
        public ContaCorrente(string numero, string titular, decimal saldo, decimal limiteChequeEspecial) : base(numero, titular, saldo)
        {
            _limiteChequeEspecial = limiteChequeEspecial;
            _taxaManutencao = 20.00m;
        }

        protected override bool PodeRealizarOperacao(decimal valor) => SaldoDisponivel >= valor;

        public override void CalcularTarifaMensal()
        {
            if (!Ativa) throw new ContaInativaException("[ERRO] Conta inativa. Não é possível calcular tarifa mensal.");
            if (!PodeRealizarOperacao(TaxaManutencao)) throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível calcular a tarifa mensal.");

            Saldo -= TaxaManutencao;
            AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TarifaMensal, TaxaManutencao, Saldo + TaxaManutencao, Saldo));
        }

        public override string ToString() => $"Titular: {Titular} | Saldo: {Saldo:C} | Saldo Disponível (Cheque Especial): {SaldoDisponivel:C}";
    }
}