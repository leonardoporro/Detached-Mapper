namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug18;

public abstract class ArtikelBase : IdBase
{
    public string Discriminator { get; set; }
}