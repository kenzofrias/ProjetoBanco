using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Enums;
using ProjetoBanco.Core.Interfaces;
using ProjetoBanco.Core.ValueObjects;

namespace ProjetoBanco.Core.Models
{
    public class ExtratoResposta : IExtratoResposta
    {
        public int Id { get; protected set; }
        public string NumeroConta { get; protected set; } = string.Empty;
        public DateTime Data { get; protected set; }
        public string Operacao { get; protected set; }
        public decimal Valor { get; protected set; }
        public decimal SaldoAnterior { get; protected set; }
        public decimal SaldoAtual { get; protected set; }

        public ExtratoResposta(){  }
        public ExtratoResposta(string numeroConta, UltimaMovimentacao ultimaMovimentacao, decimal saldoAnterior, decimal saldoAtual)
        {
            NumeroConta = numeroConta;
            Data = ultimaMovimentacao.DataMovimentacao;
            Operacao = ultimaMovimentacao.TipoOperacao switch
            {
                TipoOperacao.Deposito => "Depósito",
                TipoOperacao.Saque => "Saque",
                TipoOperacao.TransferenciaEnviada => "Transferência Enviada",
                TipoOperacao.TransferenciaRecebida => "Transferência Recebida",
                TipoOperacao.Rendimento => "Rendimento",
                TipoOperacao.TarifaMensal => "Tarifa Mensal",
                _ => "Operação Desconhecida"
            };
            Valor = ultimaMovimentacao.ValorOperacao;
            SaldoAnterior = saldoAnterior;
            SaldoAtual = saldoAtual;
        }

        public override string ToString()
        {
            // Compara de forma matemática se o saldo reduziu para definir o sinal de exibição dinamicamente
            string sinal = SaldoAtual < SaldoAnterior ? "-" : "+";
            
            return $"{Data:dd/MM/yyyy} - {Operacao}: {sinal}{Valor:C} | Saldo anterior: {SaldoAnterior:C} | Saldo atual: {SaldoAtual:C}";
        }
    }
}