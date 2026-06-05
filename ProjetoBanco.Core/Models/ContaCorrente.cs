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

        public override void Depositar(decimal valor)
        {
            if (Ativa && valor > 0)
            {
                AumentarSaldo(valor);
                AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Deposito, valor, Saldo - valor, Saldo));
            }
            else if (Ativa && valor <= 0)
            {
                throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.");
            }
            else
            {
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar o depósito.");
            }
        }

        public override void Sacar(decimal valor)
        {
            if (Ativa && valor > 0 && SaldoDisponivel >= valor)
            {
                DiminuirSaldo(valor);
                AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Saque, valor, Saldo + valor, Saldo));
            }
            else if (Ativa && SaldoDisponivel < valor)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar o saque.");
            }
            else if (Ativa && valor <= 0)
            {
                throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar o saque.");
            }
            else
            {
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar o saque.");
            }
        }

        public override void Transferir(Conta destino, decimal valor)
        {
            if (Ativa && valor > 0 && SaldoDisponivel >= valor)
            {
                DiminuirSaldo(valor);
                destino.AumentarSaldo(valor);

                AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaEnviada, valor, Saldo + valor, Saldo));
                destino.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaRecebida, valor, destino.Saldo - valor, destino.Saldo));
            }
            else if (Ativa && SaldoDisponivel < valor)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo disponível insuficiente. Não é possível realizar a transferência.");
            }
            else if (Ativa && valor <= 0)
            {
                throw new ValorInsuficienteException("[ERRO] Valor deve ser positivo. Não é possível realizar a transferência.");
            }
            else
            {
                throw new ContaInativaException("[ERRO] Conta inativa. Não é possível realizar a transferência.");
            }
        }

        public override void CalcularTarifaMensal()
        {
            if (SaldoDisponivel >= TaxaManutencao)
            {
                DiminuirSaldo(TaxaManutencao);
                AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TarifaMensal, TaxaManutencao, Saldo + TaxaManutencao, Saldo));
            }
        }

        public override string ToString() => $"Titular: {Titular} | Saldo: {Saldo:C} | Saldo Disponível (Cheque Especial): {SaldoDisponivel:C}";
    }
}