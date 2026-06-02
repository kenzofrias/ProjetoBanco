using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.Exceptions;

namespace ProjetoBanco.Tests;

    public class ContaPoupancaTests
    {
        private ContaPoupanca _contaPoupanca = new ContaPoupanca("12345", "João C O Silva", 1000m, 5m);
        private ContaPoupanca _contaPoupançaZerada = new ContaPoupanca("67890", "Ana M R Oliveira", 0m, 5m);
        private ContaCorrente _contaCorrente = new ContaCorrente("54321", "Maria A B Souza", 500m, 200m);

        public ContaPoupancaTests()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        // Teste de depósito
        [Fact]
        public void DeveDepositar100ReaisERetornarSado1100Reais()
        {
            decimal valorDeposito = 100m;

            _contaPoupanca.Depositar(valorDeposito,1);

            Assert.Equal(1100m, _contaPoupanca.ObterSaldo());
        }

        [Fact]
        public void DeveDepositar100ReaisNegativosERetornarExcecao()
        {
            decimal valorDeposito = -100m;

            var ex = Assert.Throws<ValorInsuficienteException>(
                () => _contaPoupanca.Depositar(valorDeposito,1));

            Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.", ex.Message);
        }

        [Fact]
        public void DeveDepositar0ReaisERetornarExcecao()
        {
            decimal valorDeposito = 0m;

            var ex = Assert.Throws<ValorInsuficienteException>(
                () => _contaPoupanca.Depositar(valorDeposito,1));

            Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.", ex.Message);
        }

        // Testes de saque
        [Fact]
        public void DeveSacar34522ReaisERetornarSaldo65478()
        {
            // Given
            decimal valorSaque = 345.22m;

            // When
            _contaPoupanca.Sacar(valorSaque);

            // Then
            Assert.Equal(654.78m, _contaPoupanca.ObterSaldo());
        }

        [Fact]
        public void DeveSacar2000ReaisERetornarExcecao()
        {
            // Given
            decimal valorSaque = 2000m;

            // When
            var ex = Assert.Throws<SaldoInsuficienteException>(
                () => _contaPoupanca.Sacar(valorSaque));

            // Then
            Assert.Equal("[ERRO] Saldo insuficiente. Não é possível realizar o saque.", ex.Message);
        }

        [Fact]
        public void DeveSacar100ReaisNegativosERetornarExcecao()
        {
            // Given
            decimal valorSaque = -100m;

            // When
            var ex = Assert.Throws<ValorInsuficienteException>(
                () => _contaPoupanca.Sacar(valorSaque));

            // Then
            Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o saque.", ex.Message);
        }

        // Testes de transferência
        [Fact]
        public void DeveTransferir34522ERetornar65478ESaldosDoDestino()
        {
            // Given
            decimal valorTransferencia = 345.22m;

            // When
            _contaPoupanca.Transferir(_contaCorrente, valorTransferencia);

            // Then
            Assert.Equal(654.78m, _contaPoupanca.ObterSaldo());
            Assert.Equal(845.22m, _contaCorrente.ObterSaldo());
            Assert.Equal(1045.22m, _contaCorrente.ObterSaldoDisponivel());
        }

        [Fact]
        public void DeveTransferir2000ReaisERetornarExcecaoESaldosDoDestino()
        {
            // Given
            decimal valorTransferencia = 2000m;

            // When
            var ex = Assert.Throws<SaldoInsuficienteException>(
                () => _contaPoupanca.Transferir(_contaCorrente, valorTransferencia));

            // Then
            Assert.Equal("[ERRO] Saldo insuficiente. Não é possível realizar a transferência.", ex.Message);
            Assert.Equal(500m, _contaCorrente.ObterSaldo());
            Assert.Equal(700m, _contaCorrente.ObterSaldoDisponivel());
        }

        [Fact]
        public void DeveTransferir100ReaisNegativosERetornarExcecaoESaldosDoDestino()
        {
            // Given
            decimal valorTransferencia = -100m;

            // When
            var ex = Assert.Throws<ValorInsuficienteException>(
                () => _contaPoupanca.Transferir(_contaCorrente, valorTransferencia));

            // Then
            Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar a transferência.", ex.Message);
            Assert.Equal(500m, _contaCorrente.ObterSaldo());
            Assert.Equal(700m, _contaCorrente.ObterSaldoDisponivel());
        }

        // Testes de aplicação de rendimento
        [Fact]
        public void DeveAplicarRendimentoUmaVezERetornarSaldoNovo()
        {
            // Given

            // When
            _contaPoupanca.AplicarRendimento();

            // Then
            Assert.Equal(1050m, _contaPoupanca.ObterSaldo());
        }

        [Fact]
        public void DeveAplicarRendimento5VezesERetornarSaldoNovo()
        {
            // Given
            int contador = 1;

            // When
            while (contador <= 5)
            {
                _contaPoupanca.AplicarRendimento();
                contador++;
            }

            // Then
            Assert.Equal(1276.28m, _contaPoupanca.ObterSaldo());
        }
        
        [Fact]
        public void DeveAplicarRendimentoUmaVezERetornarExcecao()
        {
            // When
            var ex = Assert.Throws<SaldoInsuficienteException>(() => _contaPoupançaZerada.AplicarRendimento());
        
            // Then
            Assert.Equal("[ERRO] Saldo insuficiente para aplicar rendimento. O saldo deve ser maior que zero.", ex.Message);
            Assert.Equal(0m, _contaPoupançaZerada.ObterSaldo());
        }
    }