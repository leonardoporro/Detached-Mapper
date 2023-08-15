namespace Detached.Mappers.Types.Conventions
{
    public class CamelCasePropertyNameConvention : IPropertyNameConvention
    {
        public string GetSourcePropertyName(IType sourceType, IType targetType, string targetMemberName)
        {
            return char.ToLower(targetMemberName[0]) + targetMemberName.Substring(1);
        }
    }
}