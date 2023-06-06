namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug18;

public abstract class ArtikelBaseDTO : IdBaseDTO
{
    public string Discriminator { get; set; }
}