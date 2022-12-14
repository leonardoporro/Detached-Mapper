namespace Detached.Mappers.Types.Conventions
{
    public interface IPropertyNameConvention
    {
        string GetSourcePropertyName(IType sourceType, IType targetType, string memberName);
    }
}
