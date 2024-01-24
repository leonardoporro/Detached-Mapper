namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.Bug18;

public class ZubehoerDto : ArtikelBaseDto
{
    public OwnedOneDto OwnedOne { get; set; }
    
    public OwnedTwoDto OwnedTwo { get; set; }
}