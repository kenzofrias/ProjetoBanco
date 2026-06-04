using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoBanco.Core.Exceptions;
using ProjetoBanco.Core.Interfaces;

namespace ProjetoBanco.Core.Models
{
    public abstract class Conta : IConta
    {
        private readonly Stack<IHistoricoResposta> _historico;

        public string Numero { get; } = string.Empty;
        public string Titular { get; } = string.Empty;
        public decimal Saldo { get; internal set; }
        public bool Ativa { get; } = false;
        public IReadOnlyCollection<IHistoricoResposta> Historico => _historico;

        public Conta() { }
        public Conta(string numero, string titular, decimal saldo)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldo;
            Ativa = true;
            _historico = new Stack<IHistoricoResposta>();
        }

        public IHistoricoResposta AdicionarMovimentacaoHistorico(IHistoricoResposta historicoResposta)
        {
            if(historicoResposta == null)
                throw new HistoricoRespostaException("[ERRO] O histórico de resposta não pode ser nulo.");
            _historico.Push(historicoResposta);
            return historicoResposta;
        }
    }
}