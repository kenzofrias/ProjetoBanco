using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Models
{
    public class ContaCorrente : Conta
    {
        public decimal LimiteChequeEspecial { get; protected set; }
        public decimal SaldoDisponivel => Saldo + LimiteChequeEspecial;
        public decimal TaxaManutencao { get; protected set; }
 
        public ContaCorrente() { }
        public ContaCorrente(string numero, string agencia, decimal saldo, decimal limiteChequeEspecial, decimal taxaManutencao = 20.00m) : base(numero, agencia, saldo)
        {
            LimiteChequeEspecial = limiteChequeEspecial; 
            TaxaManutencao = taxaManutencao;
        }

        protected override bool PodeRealizarOperacao(decimal valor) =>
            SaldoDisponivel >= valor;

        public override void CalcularTarifaMensal()
        {
            if (Status != Status.Ativa) 
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível calcular tarifa mensal.");
            if (!PodeRealizarOperacao(TaxaManutencao)) 
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível calcular a tarifa mensal.");

            var valorTarifa = TaxaManutencao;
            Saldo -= valorTarifa;
            UltimaMovimentacao = new UltimaMovimentacao(DateTime.UtcNow, TipoOperacao.TarifaMensal, valorTarifa);
            AdicionarMovimentacaoExtrato(new ExtratoResposta(Id, NumeroConta, UltimaMovimentacao, Saldo + valorTarifa, Saldo));
        }

        public override string ToString() =>
            $"Numero: {NumeroConta} | Agência: {Agencia} | Saldo: {Saldo:C} | Saldo Disponível (Cheque Especial): {SaldoDisponivel:C}";
    }
}