namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.Bug18;

public abstract class ArtikelBaseDto : IdBaseDto
{
    public string Discriminator { get; set; }
}