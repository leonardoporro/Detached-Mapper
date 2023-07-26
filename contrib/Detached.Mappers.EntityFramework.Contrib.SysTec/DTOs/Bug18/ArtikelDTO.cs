namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug18;

public class ArtikelDTO : ArtikelBaseDTO
{
    public OwnedOneDTO OwnedOne { get; set; }
    
    public OwnedTwoDTO OwnedTwo { get; set; }
}