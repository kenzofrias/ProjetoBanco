﻿using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.Services;
using ProjetoBanco.Infrastructure.Data;
using ProjetoBanco.Infrastructure.Repositories;

Console.WriteLine("--- Iniciando Sistema Bancário ---");

// Aplicação de migrations
Console.WriteLine("Verificando e aplicando migrations do banco de dados...");
using (var dbContextMigration = new BancoDBContext())
{
    dbContextMigration.Database.Migrate();
}
Console.WriteLine("Migrations aplicadas com sucesso.");

using var dbContext = new BancoDBContext();
var repositorio = new ContaRepositorio(dbContext);
var transferenciaService = new TransferenciaService(repositorio);
var extratoService = new ExtratoService(repositorio);

async Task<Conta> ObterOuCriarContaAsync(Conta conta)
{
    var contaExistente = await repositorio.ObterContaPorNumeroEAgenciaAsync(
        conta.NumeroConta,
        conta.Agencia);

    if (contaExistente is not null)
    {
        return contaExistente;
    }

    await repositorio.AdicionarContaAsync(conta);
    return conta;
}

Console.WriteLine("\n--- Criando ou Carregando Contas ---");
Conta c1 = await ObterOuCriarContaAsync(new ContaCorrente("12345", "0001", 1200m, 500m));
Conta c2 = await ObterOuCriarContaAsync(new ContaCorrente("54321", "0001", 1400m, 750m));
Conta c3 = await ObterOuCriarContaAsync(new ContaPoupanca("67890", "0001", 2000m, 0.05m));
Conta c4 = await ObterOuCriarContaAsync(new ContaPoupanca("09876", "0001", 1500m, 0.03m));

await repositorio.SalvarAlteracoesAsync();
Console.WriteLine("Contas salvas com sucesso.");

Console.WriteLine("\n--- Realizando Operações ---");
c1.Creditar(100m);
c2.Debitar(200m);
await transferenciaService.Transferir(c1.Id, c3.Id, 50m);
await transferenciaService.Transferir(c4.Id, c1.Id, 100m);
Console.WriteLine("Operações salvas no banco de dados.");

Console.WriteLine(await extratoService.GerarExtrato(c1.Id));
Console.WriteLine(await extratoService.GerarExtrato(c2.Id));
Console.WriteLine(await extratoService.GerarExtrato(c3.Id));
Console.WriteLine(await extratoService.GerarExtrato(c4.Id));

Console.WriteLine("\n--- Finalizando a Aplicação ---");