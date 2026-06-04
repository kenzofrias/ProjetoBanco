using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.Models;

namespace ProjetoBanco.Core.Services
{
    public class ContaCorrenteService : ContaService
    {
        private readonly ContaCorrente _contaCorrente;
        public ContaCorrenteService(ContaCorrente conta) : base(conta)
        {
            _contaCorrente = conta;
        }
        public override void Depositar(decimal valor)
        {
            if (_contaCorrente.Ativa && valor > 0)
            {
                _contaCorrente.Saldo += valor;
                _contaCorrente.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Deposito, valor, _contaCorrente.Saldo - valor, _contaCorrente.Saldo)); 
            } else if (_contaCorrente.Ativa && valor > 0)
            {
                _contaCorrente.Saldo += valor;
                _contaCorrente.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaRecebida, valor, _contaCorrente.Saldo - valor, _contaCorrente.Saldo));
            }
            else if (_contaCorrente.Ativa && valor <= 0)
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
            if (_contaCorrente.Ativa && valor > 0 && _contaCorrente.SaldoDisponivel >= valor)
            {
                _contaCorrente.Saldo -= valor;
                _contaCorrente.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.Saque, valor, _contaCorrente.Saldo + valor, _contaCorrente.Saldo));
            }
            else if (_contaCorrente.Ativa && valor > 0 && _contaCorrente.SaldoDisponivel < valor)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar o saque.");
            }
            else if (_contaCorrente.Ativa && valor <= 0)
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
            if (_contaCorrente.Ativa && valor > 0 && _contaCorrente.SaldoDisponivel >= valor)
            {
                _contaCorrente.Saldo -= valor;
                destino.Saldo += valor;

                _contaCorrente.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaEnviada, valor, _contaCorrente.Saldo + valor, _contaCorrente.Saldo));
                destino.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TransferenciaRecebida, valor, destino.Saldo - valor, destino.Saldo));
            }
            else if (_contaCorrente.Ativa && valor > 0 && _contaCorrente.SaldoDisponivel < valor)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo disponível insuficiente. Não é possível realizar a transferência.");
            }
            else if (_contaCorrente.Ativa && valor <= 0)
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
            if (_contaCorrente.Saldo >= _contaCorrente.TaxaManutencao)
            {
                _contaCorrente.Saldo -= _contaCorrente.TaxaManutencao;
                _contaCorrente.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TarifaMensal, _contaCorrente.TaxaManutencao, _contaCorrente.Saldo + _contaCorrente.TaxaManutencao, _contaCorrente.Saldo));
            }
            else if (_contaCorrente.Saldo < _contaCorrente.TaxaManutencao && _contaCorrente.SaldoDisponivel >= _contaCorrente.TaxaManutencao)
            {
                _contaCorrente.Saldo -= _contaCorrente.TaxaManutencao;
                _contaCorrente.AdicionarMovimentacaoHistorico(new HistoricoResposta(TipoOperacao.TarifaMensal, _contaCorrente.TaxaManutencao, _contaCorrente.Saldo + _contaCorrente.TaxaManutencao, _contaCorrente.Saldo));
            }
        }

        public override string ToString() => $"Titular: {_contaCorrente.Titular} | Saldo: {_contaCorrente.Saldo:C} | Saldo Disponível (Cheque Especial): {_contaCorrente.SaldoDisponivel:C}";
    }
}