namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos;

public abstract class ConcurrencyStampBaseDto
{
    public int ConcurrencyToken { get; set; }
}