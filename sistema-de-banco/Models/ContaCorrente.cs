using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sistema_de_banco.Exceptions;

namespace sistema_de_banco.Models
{
    public class ContaCorrente : Conta
    {
        private decimal _limiteChequeEspecial;
        private decimal _saldoDisponivel;
        private decimal _taxaManutencao;

        public ContaCorrente(string numero, string titular, decimal saldo, decimal limiteChequeEspecial) : base(numero, titular, saldo)
        {
            _limiteChequeEspecial = limiteChequeEspecial;
            _saldoDisponivel = saldo + limiteChequeEspecial;
            _taxaManutencao = 25; // Exemplo de taxa de manutenção para conta corrente
        }

        private void AtualizarSaldoDisponivel(int operacao, decimal valor)
        {
            if (operacao == 1) // Depósito
            {
                _saldoDisponivel += valor;
            }
            else if (operacao == 2) // Saque
            {
                _saldoDisponivel -= valor;
            }
        }

        public override void Depositar(decimal valor, int operacao)
        {
            if (Ativa && valor > 0 && operacao == 1)
            {
                Saldo += valor;
                AtualizarSaldoDisponivel(1, valor);
                Historico.Push($"Depósito: +{valor:C} | Saldo anterior: {Saldo - valor:C} | Saldo atual: {Saldo:C}");
            } else if (Ativa && valor > 0 && operacao == 2)
            {
                Saldo += valor;
                AtualizarSaldoDisponivel(1, valor);
                Historico.Push($"Transferência recebida: +{valor:C} | Saldo anterior: {Saldo - valor:C} | Saldo atual: {Saldo:C}");
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
            if (Ativa && valor > 0 && Saldo >= valor)
            {
                Saldo -= valor;
                AtualizarSaldoDisponivel(2, valor);
                Historico.Push($"Saque: -{valor:C} | Saldo anterior: {Saldo + valor:C} | Saldo atual: {Saldo:C}");
            }
            else if (Ativa && valor > 0 && Saldo < valor && _saldoDisponivel >= valor)
            {
                Saldo -= valor;
                AtualizarSaldoDisponivel(2, valor); 
                Historico.Push($"Saque: -{valor:C} (Cheque Especial) | Saldo anterior: {Saldo + valor:C} | Saldo atual: {Saldo:C} | Cheque Especial Disponível: {_saldoDisponivel:C}");
            }
            else if (Ativa && valor > 0 && Saldo < valor && _saldoDisponivel < valor)
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
            if (Ativa && valor > 0 && Saldo >= valor)
            {
                Saldo -= valor;
                AtualizarSaldoDisponivel(2, valor);
                destino.Depositar(valor, 2);
                Historico.Push($"Transferência: -{valor:C} | Saldo anterior: {Saldo + valor:C} | Saldo atual: {Saldo:C} | Cheque Especial Disponível: {_saldoDisponivel:C}");
            }
            else if (Ativa && valor > 0 && Saldo < valor && _saldoDisponivel >= valor)
            {
                Saldo = valor - _saldoDisponivel;
                AtualizarSaldoDisponivel(2, valor);
                destino.Depositar(valor, 2);
                Historico.Push($"Transferência: -{valor:C} (Cheque Especial) | Saldo anterior: {Saldo + valor:C} | Saldo atual: {Saldo:C} | Cheque Especial Disponível: {_saldoDisponivel:C}");
            }
            else if (Ativa && valor > 0 && Saldo < valor && _saldoDisponivel < valor)
            {
                throw new SaldoInsuficienteException("[ERRO] Saldo insuficiente. Não é possível realizar a transferência.");
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
            if (Saldo >= _taxaManutencao)
            {
                Saldo -= _taxaManutencao;
                AtualizarSaldoDisponivel(2, _taxaManutencao);
                Historico.Push($"Tarifa Mensal: -{_taxaManutencao:C} | Saldo anterior: {Saldo + _taxaManutencao:C} | Saldo atual: {Saldo:C}");
            }
            else if (Saldo < _taxaManutencao)
            {
                Saldo = Saldo - _taxaManutencao;
                AtualizarSaldoDisponivel(2, _taxaManutencao);
                Historico.Push($"Tarifa Mensal: -{_taxaManutencao:C} (Cheque Especial) | Saldo: {Saldo:C} | Saldo Disponível: {_saldoDisponivel:C}");
            }
        }

        public decimal ObterSaldoDisponivel() => _saldoDisponivel;
        public override string ToString() => $"Titular: {Titular} | Saldo: {Saldo:C} | Saldo Disponível (Cheque Especial): {_saldoDisponivel:C}";
    }
}