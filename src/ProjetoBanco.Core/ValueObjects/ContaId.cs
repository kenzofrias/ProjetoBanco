namespace ProjetoBanco.Core.ValueObjects;

public record class ContaId
{
    public Guid Valor { get; }

    public ContaId()
    {
        Valor = Guid.NewGuid();
    }

    public ContaId(Guid valor)
    {
        Valor = valor;
    }
}
