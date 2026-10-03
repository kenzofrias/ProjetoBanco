using ProjetoBanco.Core.Enums;

namespace ProjetoBanco.Core.ValueObjects;

public record class UltimaMovimentacao(
    DateTime DataMovimentacao,
    TipoOperacao TipoOperacao,
    decimal ValorOperacao
);