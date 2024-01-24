namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.inheritance.BaseModel;

public abstract class BaseHeadDto : IdBaseDto
{
  public string Discriminator { get; set; }  
}