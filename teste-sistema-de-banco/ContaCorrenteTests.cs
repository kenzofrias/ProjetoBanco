using System.ComponentModel;
using System.Runtime.InteropServices;
using sistema_de_banco.Exceptions;
using sistema_de_banco.Models;
using System.Text;

namespace teste_sistema_de_banco;

public class UnitTest1
{
    private ContaCorrente _contaCorrente = new ContaCorrente("12345", "João C O Silva", 345.22m, 500m);

    public UnitTest1()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    // Testes de depósito
    [Fact]
    public void DeveDepositar500ReaisSeAtivaERetornar84522()
    {
        decimal valorDeposito = 500m;

        _contaCorrente.Depositar(valorDeposito);

        Assert.Equal(845.22m, _contaCorrente.ObterSaldo());
    }

    [Fact]
    public void DeveDepositar722ReaisERetornar106722()
    {
        decimal valorDeposito = 722m;

        _contaCorrente.Depositar(valorDeposito);

        Assert.Equal(1067.22m, _contaCorrente.ObterSaldo());
    }

    [Fact]
    public void DeveTentarDepositar200ReaisNegativosERetornarExcecao()
    {
        // Given
        decimal valorDeposito = -200m;
        // When
        var ex = Assert.Throws<ValorNegativo>(
        () => _contaCorrente.Depositar(valorDeposito));

        // Then
        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.", ex.Message);
    }

    [Fact]
    public void DeveTentarDepositar0ReaisERetornarExcecao()
    {
        // Given
        decimal valorDeposito = 0m;
        // When
        var ex = Assert.Throws<ValorNegativo>(
        () => _contaCorrente.Depositar(valorDeposito));

        // Then
        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.", ex.Message);
    }

    [Fact]
    public void DeveFazerVariosDepositosERetonarSaldoCorretoFinal()
    {
        // When
        _contaCorrente.Depositar(100m);
        _contaCorrente.Depositar(505.35m);
        _contaCorrente.Depositar(92.45m);

        // Then
        Assert.Equal(1043.02m, _contaCorrente.ObterSaldo());
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
    }

    [Fact]
    public void DeveSacar500ReaisComChequeEspecialERetornarSaldoCorretoNegativoESaldoDisponivel()
    {
        // Given
        decimal valorSaque = 500m;

        // When
        _contaCorrente.Sacar(valorSaque);

        // Then
        Assert.Equal(-345.22m, _contaCorrente.ObterSaldo());
        Assert.Equal(345.22m, _contaCorrente.ObterSaldoDisponivel());
    }

    [Fact]
    public void DeveTentarSacar1000ReaisERetornarExcecao()
    {
        // Given
        decimal valorSaque = 1000m;

        // When
        var ex = Assert.Throws<SaldoInsuficiente>(
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
        _contaCorrente.ExibirExtrato();

        Assert.Equal(-331m, _contaCorrente.ObterSaldo());
    }

    [Fact]
    public void DeveTentarSacar200ReaisNegativosERetornarExcecao()
    {
        // Given
        decimal valorSaque = -200m;
        // When
        var ex = Assert.Throws<ValorNegativo>(
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
        var ex = Assert.Throws<ValorNegativo>(
        () => _contaCorrente.Sacar(valorSaque));

        // Then
        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o saque.", ex.Message);
    }

    // Testes de transferência
}
