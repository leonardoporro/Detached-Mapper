using System.ComponentModel;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public enum CustomerKindId : int
    {
        [Description("Company Customer")]
        Company = 1,
        
        [Description("Private Customer")]
        Private = 2,
    }
}