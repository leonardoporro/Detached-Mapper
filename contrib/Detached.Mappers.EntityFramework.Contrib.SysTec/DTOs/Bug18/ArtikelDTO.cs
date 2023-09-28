using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug18;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug18;

public class ArtikelDTO : ArtikelBaseDTO
{
    public ArtikelDTO()
    {
        Discriminator = nameof(Artikel);
    }

    public OwnedOneDTO OwnedOne { get; set; }
    
    public OwnedTwoDTO OwnedTwo { get; set; }
}