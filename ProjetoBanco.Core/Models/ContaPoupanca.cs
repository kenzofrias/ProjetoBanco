using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;

namespace ProjetoBanco.Core.Models
{
    public class ContaPoupanca : Conta
    {
        private decimal _taxaRendimento;
        public decimal TaxaRendimento => _taxaRendimento;
        public ContaPoupanca(string numero, string titular, decimal saldo, decimal taxaRendimento) : base(numero, titular, saldo)
        {
            _taxaRendimento = taxaRendimento / 100; // Convertendo a taxa de rendimento de percentual para decimal
        }

        public override void AplicarRendimento()
        {
            if (Ativa && Saldo > 0)
            {
                decimal rendimento = Saldo * TaxaRendimento;
                AumentarSaldo(rendimento);
                AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Rendimento, rendimento, Saldo - rendimento, Saldo));
            }
            else if (Ativa && Saldo <= 0)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente para aplicar rendimento. O saldo deve ser maior que zero.");
            }
            else
            {
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível aplicar rendimento.");
            }
        }

        public override string ToString() => $"Titular: {Titular} | Saldo: {Saldo:C}";
    }
}