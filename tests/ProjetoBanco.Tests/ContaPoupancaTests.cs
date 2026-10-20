using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Models;

namespace ProjetoBanco.Tests;

public class ContaPoupancaTests
{
    [Fact]
    public void CreditarAtualizaSaldo()
    {
        var conta = new ContaPoupanca("12345", "0001", 1000m);

        conta.Creditar(100m);

        Assert.Equal(1100m, conta.Saldo);
    }

    [Fact]
    public void DebitarNaoPermiteUltrapassarSaldo()
    {
        var conta = new ContaPoupanca("12345", "0001", 1000m);

        conta.Debitar(345.22m);

        Assert.Equal(654.78m, conta.Saldo);
        Assert.Throws<SaldoInsuficienteException>(() => conta.Debitar(2000m));
    }

    [Fact]
    public void AplicarRendimentoUsaTaxaDaConta()
    {
        var conta = new ContaPoupanca("12345", "0001", 1000m);
        var saldoInicial = conta.Saldo;
        var rendimentoEsperado = saldoInicial * conta.TaxaRendimento;

        conta.AplicarRendimento();

        Assert.Equal(saldoInicial + rendimentoEsperado, conta.Saldo);
        Assert.Equal("Rendimento", Assert.Single(conta.Historico).Operacao);
    }

    [Fact]
    public void AplicarRendimentoComSaldoZeroLancaExcecao()
    {
        var conta = new ContaPoupanca("12345", "0001", 0m);

        var excecao = Assert.Throws<SaldoInsuficienteException>(() => conta.AplicarRendimento());

        Assert.Equal("[ERRO] Saldo insuficiente para aplicar rendimento. O saldo deve ser maior que zero.", excecao.Message);
    }

    [Fact]
    public void HistoricoMantemIdDaConta()
    {
        var conta = new ContaPoupanca("12345", "0001", 1000m);

        conta.Creditar(100m);

        Assert.All(conta.Historico, extrato => Assert.Equal(conta.Id, extrato.ContaId));
    }
}
