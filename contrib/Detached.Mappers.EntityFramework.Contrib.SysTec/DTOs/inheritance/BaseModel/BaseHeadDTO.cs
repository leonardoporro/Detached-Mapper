namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.inheritance.BaseModel;

public abstract class BaseHeadDTO : IdBaseDTO
{
  public string Discriminator { get; set; }  
}