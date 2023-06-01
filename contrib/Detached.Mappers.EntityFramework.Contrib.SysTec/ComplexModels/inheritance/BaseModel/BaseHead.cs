namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance.BaseModel;

public abstract class BaseHead : IdBase
{
  public string Discriminator { get; set; }  
}