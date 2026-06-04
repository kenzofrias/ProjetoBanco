using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;

namespace ProjetoBanco.Core.Models
{
    public class ContaCorrente : Conta
    {
        private decimal _saldoDisponivel => _limiteChequeEspecial + Saldo;
        private decimal _limiteChequeEspecial;
        private decimal _taxaManutencao;

        public decimal LimiteChequeEspecial => _limiteChequeEspecial;
        public decimal SaldoDisponivel => _saldoDisponivel;
        public decimal TaxaManutencao => _taxaManutencao;

        public ContaCorrente() { }
        public ContaCorrente(string numero, string titular, decimal saldo, decimal limiteChequeEspecial) : base(numero, titular, saldo)
        {
            _limiteChequeEspecial = limiteChequeEspecial;
            _taxaManutencao = 20.00m;
        }
    }
}