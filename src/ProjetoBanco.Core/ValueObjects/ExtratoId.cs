namespace ProjetoBanco.Core.ValueObjects;

public record class ExtratoId
{
    public Guid Valor { get; }

    public ExtratoId()
    {
        Valor = Guid.NewGuid();
    }

    public ExtratoId(Guid valor)
    {
        Valor = valor;
    }
}
