using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ProjetoBanco.Core.Exceptions;

namespace ProjetoBanco.Core.Models
{
    public class ContaPoupanca : Conta
    {
        private decimal _taxaRendimento;
        public ContaPoupanca(string numero, string titular, decimal saldo, decimal taxaRendimento) : base(numero, titular, saldo)
        {
            _taxaRendimento = taxaRendimento / 100; // Convertendo a taxa de rendimento de percentual para decimal
        }
    }
}