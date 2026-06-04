using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoBanco.Core.Services
{
    public class ContaPoupancaService
    {
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