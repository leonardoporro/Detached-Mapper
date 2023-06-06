namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug18;

public class Artikel : ArtikelBase
{
    public OwnedOne OwnedOne { get; set; }
    
    public OwnedTwo OwnedTwo { get; set; }
}