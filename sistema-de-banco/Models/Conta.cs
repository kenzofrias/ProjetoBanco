using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sistema_de_banco.Exceptions;

namespace sistema_de_banco.Models
{
    public abstract class Conta
    {
        protected string Numero { get; private set; }
        protected string Titular { get; private set; }
        protected decimal Saldo { get; set; }
        protected bool Ativa { get; private set; } = false; 
        protected Stack<string> Historico;

        public Conta(string numero, string titular, decimal saldo)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldo;
            Ativa = true;
            Historico = new Stack<string>();
        }

        public virtual void Depositar(decimal valor, int operacao) 
        {
            if (Ativa && valor > 0 && operacao == 1)
            {
                Saldo += valor;
                Historico.Push($"Depósito: +{valor:C} | Saldo anterior: {Saldo - valor:C} | Saldo atual: {Saldo:C}");
            } else if (Ativa && valor > 0 && operacao == 2)
            {
                Saldo += valor;
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

        public virtual void Sacar(decimal valor)
        {
            if (Ativa && valor > 0 && Saldo >= valor)
            {
                Saldo -= valor;
                Historico.Push($"Saque: -{valor:C} | Saldo anterior: {Saldo + valor:C} | Saldo atual: {Saldo:C}");
            }
            else if (Ativa && valor > 0 && Saldo < valor)
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

        public virtual void Transferir(Conta destino, decimal valor)
        {
            if (Ativa && valor > 0 && Saldo >= valor)
            {
                Saldo -= valor;
                destino.Depositar(valor, 2);
                Historico.Push($"Transferência: -{valor:C} | Saldo anterior: {Saldo + valor:C} | Saldo atual: {Saldo:C}");
            }
            else if (Ativa && valor > 0 && Saldo < valor)
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

        public void ExibirExtrato()
        {
            Console.WriteLine($"Extrato de {GetType().Name} | Titular: {Titular} | Saldo: {Saldo:C}");
            Console.WriteLine("Lançamentos:");

            if (Historico.Count == 0)
            {
                Console.WriteLine("Nenhuma transação realizada.");
            }
            else
            {
                foreach (var item in Historico)
                {
                    Console.WriteLine(item);
                }
            }
        }

        public decimal ObterSaldo() => Math.Round(Saldo, 2);

        public override string ToString() => $"Titular: {Titular} | Saldo: {Saldo:C}";

        public virtual void CalcularTarifaMensal() { }
        public virtual void AplicarRendimento() { }
    }
}