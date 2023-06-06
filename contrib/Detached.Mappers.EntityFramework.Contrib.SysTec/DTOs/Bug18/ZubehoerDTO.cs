namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug18;

public class ZubehoerDTO : ArtikelBaseDTO
{
    public OwnedOneDTO OwnedOne { get; set; }
    
    public OwnedTwoDTO OwnedTwo { get; set; }
}