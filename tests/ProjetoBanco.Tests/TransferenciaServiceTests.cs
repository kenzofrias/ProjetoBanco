using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.Services;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Tests;

public class TransferenciaServiceTests
{
    [Fact]
    public async Task TransferirDebitaOrigemECreditaDestino()
    {
        var origem = new ContaCorrente("12345", "0001", 1000m, 200m);
        var destino = new ContaPoupanca("67890", "0001", 500m);
        var repositorio = new RepositorioEmMemoria(origem, destino);
        var service = new TransferenciaService(repositorio);

        await service.Transferir(origem.Id, destino.Id, 250m);

        Assert.Equal(750m, origem.Saldo);
        Assert.Equal(750m, destino.Saldo);
        Assert.Equal("Transferência Enviada", Assert.Single(origem.Historico).Operacao);
        Assert.Equal("Transferência Recebida", Assert.Single(destino.Historico).Operacao);
        Assert.Equal(2, repositorio.QuantidadeDeAtualizacoes);
    }

    [Fact]
    public async Task TransferirComSaldoInsuficienteNaoCreditaDestino()
    {
        var origem = new ContaPoupanca("12345", "0001", 100m);
        var destino = new ContaCorrente("67890", "0001", 500m, 200m);
        var repositorio = new RepositorioEmMemoria(origem, destino);
        var service = new TransferenciaService(repositorio);

        await Assert.ThrowsAsync<SaldoInsuficienteException>(
            () => service.Transferir(origem.Id, destino.Id, 250m));

        Assert.Equal(100m, origem.Saldo);
        Assert.Equal(500m, destino.Saldo);
        Assert.Empty(origem.Historico);
        Assert.Empty(destino.Historico);
        Assert.Equal(0, repositorio.QuantidadeDeAtualizacoes);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task TransferirComValorInvalidoNaoAlteraAsContas(decimal valor)
    {
        var origem = new ContaCorrente("12345", "0001", 1000m, 200m);
        var destino = new ContaPoupanca("67890", "0001", 500m);
        var repositorio = new RepositorioEmMemoria(origem, destino);
        var service = new TransferenciaService(repositorio);

        await Assert.ThrowsAsync<ValorInsuficienteException>(
            () => service.Transferir(origem.Id, destino.Id, valor));

        Assert.Equal(1000m, origem.Saldo);
        Assert.Equal(500m, destino.Saldo);
        Assert.Empty(origem.Historico);
        Assert.Empty(destino.Historico);
    }

    [Fact]
    public async Task TransferirComContaInexistenteLancaExcecao()
    {
        var origem = new ContaCorrente("12345", "0001", 1000m, 200m);
        var destinoId = new ContaId();
        var repositorio = new RepositorioEmMemoria(origem);
        var service = new TransferenciaService(repositorio);

        var excecao = await Assert.ThrowsAsync<Exception>(
            () => service.Transferir(origem.Id, destinoId, 100m));

        Assert.Equal("[ERRO] Uma das contas não foi encontrada.", excecao.Message);
        Assert.Equal(1000m, origem.Saldo);
        Assert.Empty(origem.Historico);
    }

    [Fact]
    public async Task TransferirParaContaInativaNaoDebitaOrigem()
    {
        var origem = new ContaCorrente("12345", "0001", 1000m, 200m);
        var destino = new ContaPoupancaTestavel("67890", "0001", 500m);
        destino.Bloquear();
        var repositorio = new RepositorioEmMemoria(origem, destino);
        var service = new TransferenciaService(repositorio);

        await Assert.ThrowsAsync<ContaInativaException>(
            () => service.Transferir(origem.Id, destino.Id, 250m));

        Assert.Equal(1000m, origem.Saldo);
        Assert.Equal(500m, destino.Saldo);
        Assert.Empty(origem.Historico);
        Assert.Empty(destino.Historico);
        Assert.Equal(0, repositorio.QuantidadeDeAtualizacoes);
    }

    private sealed class ContaPoupancaTestavel : ContaPoupanca
    {
        public ContaPoupancaTestavel(string numero, string agencia, decimal saldo)
            : base(numero, agencia, saldo)
        {
        }

        public void Bloquear() => Status = Status.Bloqueada;
    }

    private sealed class RepositorioEmMemoria : IContaRepositorio
    {
        private readonly Dictionary<Guid, Conta> _contas;

        public int QuantidadeDeAtualizacoes { get; private set; }

        public RepositorioEmMemoria(params Conta[] contas)
        {
            _contas = contas.ToDictionary(conta => conta.Id.Valor);
        }

        public Task<bool> ContaExistePorIdAsync(ContaId contaId) =>
            Task.FromResult(_contas.ContainsKey(contaId.Valor));

        public Task<bool> ContaExistePorNumeroEAgenciaAsync(string numeroConta, string agencia) =>
            Task.FromResult(_contas.Values.Any(conta =>
                conta.NumeroConta == numeroConta && conta.Agencia == agencia));

        public Task<Conta?> ObterContaPorIdAsync(ContaId contaId) =>
            Task.FromResult(_contas.GetValueOrDefault(contaId.Valor));

        public Task<Conta?> ObterContaPorNumeroEAgenciaAsync(string numeroConta, string agencia) =>
            Task.FromResult(_contas.Values.FirstOrDefault(conta =>
                conta.NumeroConta == numeroConta && conta.Agencia == agencia));

        public Task<IEnumerable<Conta?>> ObterTodasContasAsync() =>
            Task.FromResult<IEnumerable<Conta?>>(_contas.Values);

        public Task<IEnumerable<Conta?>> ObterTodasContasCorrenteAsync() =>
            Task.FromResult<IEnumerable<Conta?>>(_contas.Values.OfType<ContaCorrente>());

        public Task<IEnumerable<Conta?>> ObterTodasContasPoupançaAsync() =>
            Task.FromResult<IEnumerable<Conta?>>(_contas.Values.OfType<ContaPoupanca>());

        public Task AdicionarContaAsync(Conta conta)
        {
            _contas.Add(conta.Id.Valor, conta);
            return Task.CompletedTask;
        }

        public Task AtualizarContaAsync(Conta conta)
        {
            QuantidadeDeAtualizacoes++;
            _contas[conta.Id.Valor] = conta;
            return Task.CompletedTask;
        }

        public Task RemoverContaAsync(ContaId contaId)
        {
            _contas.Remove(contaId.Valor);
            return Task.CompletedTask;
        }

        public Task SalvarAlteracoesAsync() => Task.CompletedTask;
    }
}
