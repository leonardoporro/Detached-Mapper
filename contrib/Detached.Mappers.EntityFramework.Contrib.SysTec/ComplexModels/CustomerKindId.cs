using System.ComponentModel;

namespace GraphInheritenceTests.ComplexModels
{
    public enum CustomerKindId : int
    {
        [Description("Company Customer")]
        Company = 1,
        
        [Description("Private Customer")]
        Private = 2,
    }
}