using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sistema_de_banco.Exceptions;

namespace sistema_de_banco.Models
{
    public abstract class Conta
    {
        protected string Numero { get; set; }
        protected string Titular { get; set; }
        protected decimal Saldo { get; set; }
        protected bool Ativa { get; set; } = false;
        protected Stack<string> Historico;

        public Conta(string numero, string titular, decimal saldo)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldo;
            Ativa = true;
            Historico = new Stack<string>();
        }

        public virtual void Depositar(decimal valor) 
        {
            if (Ativa && valor > 0)
            {
                Saldo += valor;
                Historico.Push($"Depósito: +{valor:C} | Saldo: {Saldo:C}");
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
                Historico.Push($"Saque: -{valor:C} | Saldo: {Saldo:C}");
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
                destino.Depositar(valor);
                Historico.Push($"Transferência: -{valor:C} para {destino.Titular} | Saldo: {Saldo:C}");
                destino.Historico.Push($"Transferência: +{valor:C} de {Titular} | Saldo: {destino.Saldo:C}");
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

        public decimal ObterSaldo() => Saldo;

        public virtual void CalcularTarifaMensal() { }
        public virtual void AplicarRendimento() { }
    }
}