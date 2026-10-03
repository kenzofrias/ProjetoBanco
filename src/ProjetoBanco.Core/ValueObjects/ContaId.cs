namespace ProjetoBanco.Core.ValueObjects;

public record class ContaId
{
    public Guid Valor { get; } = Guid.NewGuid();
}
