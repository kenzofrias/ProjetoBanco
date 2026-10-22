using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Models;

namespace ProjetoBanco.Tests;

public class ExcecoesTests
{
    [Fact]
    public void CriarContaComNumeroInvalidoLancaArgumentException()
    {
        var excecao = Assert.Throws<ArgumentException>(
            () => new ContaCorrente("", "0001", 100m, 50m));

        Assert.Contains("Número da conta inválido", excecao.Message);
    }

    [Fact]
    public void CriarContaComAgenciaInvalidaLancaArgumentException()
    {
        var excecao = Assert.Throws<ArgumentException>(
            () => new ContaCorrente("12345", "", 100m, 50m));

        Assert.Contains("Agência inválida", excecao.Message);
    }

    [Fact]
    public void CriarContaComSaldoInicialNegativoLancaArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new ContaCorrente("12345", "0001", -1m, 50m));
    }

    [Fact]
    public void CriarContaPoupancaDefineTipoEUsaTaxaDeterministica()
    {
        var conta = new ContaPoupanca("12345", "0001", 100m, 0.05m);

        Assert.Equal(TipoConta.Poupanca, conta.TipoConta);
        Assert.Equal(0.05m, conta.TaxaRendimento);
    }

    [Fact]
    public void CreditarContaInativaLancaContaInativaException()
    {
        var conta = new ContaCorrenteTestavel("12345", "0001", 100m, 50m);
        conta.Bloquear();

        var excecao = Assert.Throws<ContaInativaException>(() => conta.Creditar(10m));

        Assert.Equal("[ERRO] Conta inativa. Não é possível realizar o depósito.", excecao.Message);
    }

    [Fact]
    public void DebitarContaInativaLancaContaInativaException()
    {
        var conta = new ContaCorrenteTestavel("12345", "0001", 100m, 50m);
        conta.Bloquear();

        var excecao = Assert.Throws<ContaInativaException>(() => conta.Debitar(10m));

        Assert.Equal("[ERRO] Conta inativa. Não é possível realizar o saque.", excecao.Message);
    }

    [Fact]
    public void CalcularTarifaDeContaInativaLancaContaInativaException()
    {
        var conta = new ContaCorrenteTestavel("12345", "0001", 100m, 50m);
        conta.Bloquear();

        var excecao = Assert.Throws<ContaInativaException>(() => conta.CalcularTarifaMensal());

        Assert.Equal("[ERRO] Conta inativa. Não é possível calcular tarifa mensal.", excecao.Message);
    }

    [Fact]
    public void AplicarRendimentoEmContaInativaLancaContaInativaException()
    {
        var conta = new ContaPoupancaTestavel("12345", "0001", 100m);
        conta.Bloquear();

        var excecao = Assert.Throws<ContaInativaException>(() => conta.AplicarRendimento());

        Assert.Equal("[ERRO] Conta inativa. Não é possível aplicar rendimento.", excecao.Message);
    }

    [Fact]
    public void AplicarRendimentoComSaldoZeroLancaSaldoInsuficienteException()
    {
        var conta = new ContaPoupanca("12345", "0001", 0m);

        var excecao = Assert.Throws<SaldoInsuficienteException>(() => conta.AplicarRendimento());

        Assert.Equal("[ERRO] Saldo insuficiente para aplicar rendimento. O saldo deve ser maior que zero.", excecao.Message);
    }

    private sealed class ContaCorrenteTestavel : ContaCorrente
    {
        public ContaCorrenteTestavel(string numero, string agencia, decimal saldo, decimal limite)
            : base(numero, agencia, saldo, limite)
        {
        }

        public void Bloquear() => Status = Status.Bloqueada;
    }

    private sealed class ContaPoupancaTestavel : ContaPoupanca
    {
        public ContaPoupancaTestavel(string numero, string agencia, decimal saldo)
            : base(numero, agencia, saldo)
        {
        }

        public void Bloquear() => Status = Status.Bloqueada;
    }
}
