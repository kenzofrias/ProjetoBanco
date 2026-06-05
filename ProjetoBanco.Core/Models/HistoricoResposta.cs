using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Interfaces;

namespace ProjetoBanco.Core.Models
{
    public class HistoricoResposta : IHistoricoResposta
    {
        public DateTime Data { get; }
        public string Operacao { get; }
        public decimal Valor { get; }
        public decimal SaldoAnterior { get; }
        public decimal SaldoAtual { get; }

        public HistoricoResposta(){  }
        public HistoricoResposta(TipoOperacao tipoOperacao, decimal valor, decimal saldoAnterior, decimal saldoAtual)
        {
            Data = DateTime.Now;
            Operacao = tipoOperacao switch
            {
                TipoOperacao.Deposito => "Depósito",
                TipoOperacao.Saque => "Saque",
                TipoOperacao.TransferenciaEnviada => "Transferência Enviada",
                TipoOperacao.TransferenciaRecebida => "Transferência Recebida",
                TipoOperacao.Rendimento => "Rendimento",
                TipoOperacao.TarifaMensal => "Tarifa Mensal",
                _ => "Operação Desconhecida"
            };
            Valor = valor;
            SaldoAnterior = saldoAnterior;
            SaldoAtual = saldoAtual;
        }

        public override string ToString()
        {
            return $"{Data:dd/MM/yyyy} - {Operacao}: {((Operacao == "Saque" || Operacao == "Transferência Enviada" || Operacao == "Tarifa Mensal") ? "-" : "+")}{Valor:C} | Saldo anterior: {SaldoAnterior:C} | Saldo atual: {SaldoAtual:C}";
        }
    }
} 