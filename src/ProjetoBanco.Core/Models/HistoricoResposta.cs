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
        public int Id { get; protected set; }
        public string NumeroConta { get; protected set; } = string.Empty;
        public DateTime Data { get; protected set; }
        public string Operacao { get; protected set; }
        public decimal Valor { get; protected set; }
        public decimal SaldoAnterior { get; protected set; }
        public decimal SaldoAtual { get; protected set; }

        public HistoricoResposta(){  }
        public HistoricoResposta(string numeroConta, TipoOperacao tipoOperacao, decimal valor, decimal saldoAnterior, decimal saldoAtual)
        {
            NumeroConta = numeroConta;
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
            // Compara de forma matemática se o saldo reduziu para definir o sinal de exibição dinamicamente
            string sinal = SaldoAtual < SaldoAnterior ? "-" : "+";
            return $"{Data:dd/MM/yyyy} - {Operacao}: {sinal}{Valor:C} | Saldo anterior: {SaldoAnterior:C} | Saldo atual: {SaldoAtual:C}";
        }
    }
}