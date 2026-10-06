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
        public decimal TaxaRendimento { get; protected set; }

        public ContaPoupanca(string numero, string agencia, decimal saldo) : base(numero, agencia, saldo)
        {
            var random = new Random();
            double sorteadoBruto = random.NextDouble();
            double sorteadoIntevalar = sorteadoBruto * (0.3 - 0.1) + 0.1; // Gera um número aleatório entre 0.1 e 0.3
            double sorteadoArredondado = Math.Round(sorteadoIntevalar, 2);

            TaxaRendimento = (decimal)sorteadoArredondado;
        }
        
        protected override bool PodeRealizarOperacao(decimal valor) => 
            Saldo >= valor;

        public override void AplicarRendimento()
        {
            if (Status != Status.Ativa) 
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível aplicar rendimento.");
            if (!PodeRealizarOperacao(0)) 
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