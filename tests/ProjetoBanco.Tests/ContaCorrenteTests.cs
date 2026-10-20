using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Models;

namespace ProjetoBanco.Tests;

public class ContaCorrenteTests
{
    [Fact]
    public void CreditarAtualizaSaldoESaldoDisponivel()
    {
        var conta = new ContaCorrente("12345", "0001", 345.22m, 500m);

        conta.Creditar(500m);

        Assert.Equal(845.22m, conta.Saldo);
        Assert.Equal(1345.22m, conta.SaldoDisponivel);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-200)]
    public void CreditarComValorInvalidoLancaExcecao(decimal valor)
    {
        var conta = new ContaCorrente("12345", "0001", 345.22m, 500m);

        var excecao = Assert.Throws<ValorInsuficienteException>(() => conta.Creditar(valor));

        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o depósito.", excecao.Message);
    }

    [Fact]
    public void DebitarUsaChequeEspecialSemUltrapassarLimite()
    {
        var conta = new ContaCorrente("12345", "0001", 345.22m, 500m);

        conta.Debitar(500m);

        Assert.Equal(-154.78m, conta.Saldo);
        Assert.Equal(345.22m, conta.SaldoDisponivel);
    }

    [Fact]
    public void DebitarAcimaDoSaldoDisponivelLancaExcecao()
    {
        var conta = new ContaCorrente("12345", "0001", 345.22m, 500m);

        var excecao = Assert.Throws<SaldoInsuficienteException>(() => conta.Debitar(1000m));

        Assert.Equal("[ERRO] Saldo insuficiente. Não é possível realizar o saque.", excecao.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-200)]
    public void DebitarComValorInvalidoLancaExcecao(decimal valor)
    {
        var conta = new ContaCorrente("12345", "0001", 345.22m, 500m);

        var excecao = Assert.Throws<ValorInsuficienteException>(() => conta.Debitar(valor));

        Assert.Equal("[ERRO] Valor deve ser positivo. Não é possível realizar o saque.", excecao.Message);
    }

    [Fact]
    public void CreditarEDebitarRegistramHistorico()
    {
        var conta = new ContaCorrente("12345", "0001", 345.22m, 500m);

        conta.Creditar(100m);
        conta.Debitar(50m);

        Assert.Equal(2, conta.Historico.Count);
        Assert.Equal("Depósito", conta.Historico.ElementAt(0).Operacao);
        Assert.Equal("Saque", conta.Historico.ElementAt(1).Operacao);
        Assert.All(conta.Historico, extrato => Assert.Equal(conta.Id, extrato.ContaId));
    }

    [Fact]
    public void CalcularTarifaMensalDebitaTaxa()
    {
        var conta = new ContaCorrente("12345", "0001", 345.22m, 500m, 20m);

        conta.CalcularTarifaMensal();

        Assert.Equal(325.22m, conta.Saldo);
        Assert.Equal(825.22m, conta.SaldoDisponivel);
        Assert.Equal("Tarifa Mensal", Assert.Single(conta.Historico).Operacao);
    }
}
