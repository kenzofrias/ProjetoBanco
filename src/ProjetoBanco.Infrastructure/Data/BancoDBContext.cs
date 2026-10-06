using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Infrastructure.Data
{
    public class BancoDBContext : DbContext
    {
        public DbSet<Conta> Contas { get; set; }
        public DbSet<ContaCorrente> ContasCorrentes { get; set; }
        public DbSet<ContaPoupanca> ContasPoupancas { get; set; }
        public DbSet<ExtratoResposta> Historicos { get; set; }

        // Construtor para Injeção de Dependência (usado pela Web API e testes)
        public BancoDBContext(DbContextOptions<BancoDBContext> options) : base(options){ }

        // Construtor padrão para uso no ConsoleApp e migrations via 'dotnet ef'
        public BancoDBContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("A variável de ambiente 'CONNECTION_STRING' não está definida.");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var contaIdConverter = new ValueConverter<ContaId, Guid>(
                contaId => contaId.Valor,
                valor => new ContaId(valor));

            modelBuilder.Entity<Conta>().HasKey(c => c.Id);
            modelBuilder.Entity<Conta>().Property(c => c.Id)
                .HasConversion(contaIdConverter)
                .ValueGeneratedNever();
            modelBuilder.Entity<Conta>().Property(c => c.NumeroConta).HasMaxLength(7).IsRequired();
            modelBuilder.Entity<Conta>().Property(c => c.Agencia).HasMaxLength(5).IsRequired();
            modelBuilder.Entity<Conta>().Property(c => c.TipoConta).HasConversion<string>().IsRequired();
            modelBuilder.Entity<Conta>().Property(c => c.Status).HasConversion<string>().IsRequired();
            modelBuilder.Entity<Conta>().Property(c => c.DataAbertura).IsRequired();
            modelBuilder.Entity<Conta>().Ignore(c => c.UltimaMovimentacao);

            modelBuilder.Entity<Conta>().Property(c => c.Saldo).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ContaCorrente>().Property(c => c.LimiteChequeEspecial).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ContaCorrente>().Property(c => c.TaxaManutencao).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ContaPoupanca>().Property(c => c.TaxaRendimento).HasColumnType("decimal(18,2)");

            var extratoIdConverter = new ValueConverter<ExtratoId, Guid>(
                extratoId => extratoId.Valor,
                valor => new ExtratoId(valor));

            modelBuilder.Entity<ExtratoResposta>().HasKey(h => h.Id); 
            modelBuilder.Entity<ExtratoResposta>().Property(c => c.Id)
                .HasConversion(extratoIdConverter)
                .ValueGeneratedNever();
            modelBuilder.Entity<ExtratoResposta>().Property(h => h.ContaId)
                .HasConversion(contaIdConverter)
                .IsRequired();
            modelBuilder.Entity<ExtratoResposta>().Property(h => h.NumeroConta).HasMaxLength(7).IsRequired();
            modelBuilder.Entity<ExtratoResposta>().Property(h => h.Operacao).IsRequired();
            modelBuilder.Entity<ExtratoResposta>().Property(h => h.Valor).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ExtratoResposta>().Property(h => h.SaldoAnterior).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ExtratoResposta>().Property(h => h.SaldoAtual).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Conta>()
                .HasMany(c => c.Historico)
                .WithOne()
                .HasForeignKey(h => h.ContaId)
                .HasPrincipalKey(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Conta>()
                .HasDiscriminator<string>("TipoConta")
                .HasValue<ContaCorrente>("ContaCorrente")
                .HasValue<ContaPoupanca>("ContaPoupanca");
        }
    }
}