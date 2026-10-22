using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Models
{
    public class ContaPoupanca : Conta
    {
        public override TipoConta TipoConta => TipoConta.Poupanca;
        public decimal TaxaRendimento { get; protected set; }

        public ContaPoupanca() { }

        public ContaPoupanca(string numero, string agencia, decimal saldo, decimal? taxaRendimento = null) : base(numero, agencia, saldo)
        {
            var taxa = taxaRendimento ?? 0.2m;
            if (taxa < 0)
                throw new ArgumentOutOfRangeException(nameof(taxaRendimento), "[ERRO] A taxa de rendimento não pode ser negativa.");

            TaxaRendimento = taxa;
        }
        
        protected override bool PodeRealizarOperacao(decimal valor) => 
            Saldo >= valor;

        public override void AplicarRendimento()
        {
            if (Status != Status.Ativa) 
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível aplicar rendimento.");
            if (Saldo <= 0)
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente para aplicar rendimento. O saldo deve ser maior que zero.");

            decimal rendimento = Saldo * TaxaRendimento;
            Saldo += rendimento;
            UltimaMovimentacao = new UltimaMovimentacao(DateTime.UtcNow, TipoOperacao.Rendimento, rendimento);
            AdicionarMovimentacaoExtrato(new ExtratoResposta(Id, NumeroConta, UltimaMovimentacao, Saldo - rendimento, Saldo));
        }

        public override string ToString() =>
            $"Numero: {NumeroConta} | Agência: {Agencia} | Saldo: {Saldo:C}";
    }
}