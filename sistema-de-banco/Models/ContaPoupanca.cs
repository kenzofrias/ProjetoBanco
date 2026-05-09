using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using sistema_de_banco.Exceptions;

namespace sistema_de_banco.Models
{
    public class ContaPoupanca : Conta
    {
        private decimal _taxaRendimento;
        public ContaPoupanca(string numero, string titular, decimal saldo, decimal taxaRendimento) : base(numero, titular, saldo)
        {
            _taxaRendimento = taxaRendimento / 100; // Convertendo a taxa de rendimento de percentual para decimal
        }

        public override void AplicarRendimento()
        {
            if (Ativa && Saldo > 0)
            {
                decimal rendimento = Saldo * _taxaRendimento;
                Saldo += rendimento;
                Historico.Push($"Rendimento aplicado: +{rendimento:C} | Saldo anterior: {Saldo - rendimento:C} | Saldo atual: {Saldo:C}");
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
    }
}