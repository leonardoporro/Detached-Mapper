using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug18;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.Bug18;

public class ArtikelDto : ArtikelBaseDto
{
    public ArtikelDto()
    {
        Discriminator = nameof(Artikel);
    }

    public OwnedOneDto OwnedOne { get; set; }
    
    public OwnedTwoDto OwnedTwo { get; set; }
}