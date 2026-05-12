using System.ComponentModel;
using System.Runtime.InteropServices;
using sistema_de_banco.Exceptions;
using sistema_de_banco.Models;
using System.Text;

namespace teste_sistema_de_banco;

public class ContaCorrenteTests
{
    private ContaCorrente _contaCorrenteSaldo20 = new ContaCorrente("54321", "Carlos D E Lima", 20m, 100m);
    private ContaCorrente _contaCorrente = new ContaCorrente("12345", "João C O Silva", 345.22m, 500m);
    private ContaCorrente _contaCorrenteDestino = new ContaCorrente("67890", "Maria A B Souza", 1000m, 300m);

    public ContaCorrenteTests()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    // Testes de depósito
    [Fact]
    public void DeveDepositar500ReaisSeAtivaERetornar84522()
    {
        decimal valorDeposito = 500m;

        _contaCorrente.Depositar(valorDeposito,1);

        Assert.Equal(845.22m, _contaCorrente.ObterSaldo());
        Assert.Equal(1345.22m, _contaCorrente.ObterSaldoDisponivel());
    }

    [Fact]
    public void DeveDepositar722ReaisERetornar106722()
    {
        decimal valorDeposito = 722m;

        _contaCorrente.Depositar(valorDeposito,1);

        Assert.Equal(1067.22m, _contaCorrente.ObterSaldo());
        Assert.Equal(1567.22m, _contaCorrente.ObterSaldoDisponivel());
    }

    [Fact]
    public void DeveTentarDepositar200ReaisNegativosERetornarExcecao()
    {
        // Given
        decimal valorDeposito = -200m;
        // When
        var ex = Assert.Throws<ValorInsuficienteException>(
        () => _contaCorrente.Depositar(valorDeposito,1));

        // Then
        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.", ex.Message);
    }

    [Fact]
    public void DeveTentarDepositar0ReaisERetornarExcecao()
    {
        // Given
        decimal valorDeposito = 0m;
        // When
        var ex = Assert.Throws<ValorInsuficienteException>(
        () => _contaCorrente.Depositar(valorDeposito,1));

        // Then
        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.", ex.Message);
    }

    [Fact]
    public void DeveFazerVariosDepositosERetonarSaldoCorretoFinal()
    {
        // When
        _contaCorrente.Depositar(100m,1);
        _contaCorrente.Depositar(505.35m,1);
        _contaCorrente.Depositar(92.45m,1);

        // Then
        Assert.Equal(1043.02m, _contaCorrente.ObterSaldo());
        Assert.Equal(1543.02m, _contaCorrente.ObterSaldoDisponivel());
    }

    // Testes de saque
    [Fact]
    public void DeveSacar150E5522ReaisERetornarSaldoCorreto()
    {
        // Given
        decimal valorSaqueUm = 150m;
        decimal valorSaqueDois = 55.22m;

        // When
        _contaCorrente.Sacar(valorSaqueUm);
        _contaCorrente.Sacar(valorSaqueDois);

        // Then
        Assert.Equal(140m, _contaCorrente.ObterSaldo());
        Assert.Equal(640m, _contaCorrente.ObterSaldoDisponivel());
    }

    [Fact]
    public void DeveSacar500ReaisComChequeEspecialERetornarSaldoCorretoNegativoESaldoDisponivel()
    {
        // Given
        decimal valorSaque = 500m;

        // When
        _contaCorrente.Sacar(valorSaque);

        // Then
        Assert.Equal(-154.78m, _contaCorrente.ObterSaldo());
        Assert.Equal(345.22m, _contaCorrente.ObterSaldoDisponivel());
    }

    [Fact]
    public void DeveTentarSacar1000ReaisERetornarExcecao()
    {
        // Given
        decimal valorSaque = 1000m;

        // When
        var ex = Assert.Throws<SaldoInsuficienteException>(
        () => _contaCorrente.Sacar(valorSaque));

        // Then
        Assert.Equal("[ERRO] Saldo insuficiente. Não é possível realizar o saque.", ex.Message);
    }

    [Fact]
    public void DeveSacarSequenciaDeValoresEMostrarSaldoNegativoEExtrato()
    {
        _contaCorrente.Sacar(100m);
        _contaCorrente.Sacar(55m);
        _contaCorrente.Sacar(359.22m);

        Assert.Equal(-169m, _contaCorrente.ObterSaldo());
        Assert.Equal(331m, _contaCorrente.ObterSaldoDisponivel());
    }

    [Fact]
    public void DeveTentarSacar200ReaisNegativosERetornarExcecao()
    {
        // Given
        decimal valorSaque = -200m;
        // When
        var ex = Assert.Throws<ValorInsuficienteException>(
        () => _contaCorrente.Sacar(valorSaque));

        // Then
        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o saque.", ex.Message);
    }

    [Fact]
    public void DeveTentarSacar0ReaisERetornarExcecao()
    {
        // Given
        decimal valorSaque = 0m;
        // When
        var ex = Assert.Throws<ValorInsuficienteException>(
        () => _contaCorrente.Sacar(valorSaque));

        // Then
        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o saque.", ex.Message);
    }

    // Testes de transferência
    [Fact]
    public void DeveEnviar0ReaisERetornarExcecao()
    {
        // Given
        decimal valorTransferencia = 0m;

        // When
        var ex = Assert.Throws<ValorInsuficienteException>(
            () => _contaCorrente.Transferir(_contaCorrenteDestino, valorTransferencia));

        // Then
        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar a transferência.", ex.Message);
    }

    [Fact]
    public void DeveEnviar200ReaisParaContaDestinoERetornarSaldoCorreto()
    {
        // Given
        decimal valorTransferencia = 200m;

        // When
        _contaCorrente.Transferir(_contaCorrenteDestino, valorTransferencia);

        // Then
        Assert.Equal(145.22m, _contaCorrente.ObterSaldo());
        Assert.Equal(1200m, _contaCorrenteDestino.ObterSaldo());

        Assert.Equal(645.22m, _contaCorrente.ObterSaldoDisponivel());
        Assert.Equal(1500m, _contaCorrenteDestino.ObterSaldoDisponivel());
    }

    [Fact]
    public void DeveEnviar500ReaisComChequeParaContaDestinoERetornarSaldoCorreto()
    {
        // Given
        decimal valorTransferencia = 500m;

        // When
        _contaCorrente.Transferir(_contaCorrenteDestino, valorTransferencia);

        // Then
        Assert.Equal(-345.22m, _contaCorrente.ObterSaldo());
        Assert.Equal(1500m, _contaCorrenteDestino.ObterSaldo());

        Assert.Equal(345.22m, _contaCorrente.ObterSaldoDisponivel());
        Assert.Equal(1800m, _contaCorrenteDestino.ObterSaldoDisponivel());
    }

    [Fact]
    public void TentarEnviar1000ReaisERetornarExcecao()
    {
        // Given
        decimal valorTransferencia = 1000m;

        // When
        var ex = Assert.Throws<SaldoInsuficienteException>(
            () => _contaCorrente.Transferir(_contaCorrenteDestino, valorTransferencia));

        // Then
        Assert.Equal("[ERRO] Saldo insuficiente. Não é possível realizar a transferência.", ex.Message);
    }

    // Teste de Tarifa Mensal
    [Fact]
    public void DeveCalcularTarifaMensalERetornarSaldoCorreto()
    {
        // Given

        // When
        _contaCorrente.CalcularTarifaMensal();

        // Then
        Assert.Equal(320.22m, _contaCorrente.ObterSaldo());
        Assert.Equal(820.22m, _contaCorrente.ObterSaldoDisponivel());
    }

    [Fact]
    public void DeveCalcularTarifaMensalComChequeEspecialERetornarSaldoCorreto()
    {
        // Given

        // When
        _contaCorrenteSaldo20.CalcularTarifaMensal();

        // Then
        Assert.Equal(-5m, _contaCorrenteSaldo20.ObterSaldo());
        Assert.Equal(95m, _contaCorrenteSaldo20.ObterSaldoDisponivel());
    }

    // Teste de Exibir Extrato
    [Fact]
    public void DeveExibirExtratoERetornarSaldosCorretos()
    {
        _contaCorrente.Depositar(100m,1);
        _contaCorrente.Depositar(255.25m,1);

        _contaCorrente.Sacar(50m);
        _contaCorrente.Transferir(_contaCorrenteDestino, 432.99m);

        _contaCorrente.CalcularTarifaMensal();

        _contaCorrente.Sacar(500m);

        _contaCorrente.ExibirExtrato();

        Assert.Equal(-307.52m, _contaCorrente.ObterSaldo());
        Assert.Equal(1432.99m, _contaCorrenteDestino.ObterSaldo());

        Assert.Equal(192.48m, _contaCorrente.ObterSaldoDisponivel());
        Assert.Equal(1732.99m, _contaCorrenteDestino.ObterSaldoDisponivel());
    }
}

