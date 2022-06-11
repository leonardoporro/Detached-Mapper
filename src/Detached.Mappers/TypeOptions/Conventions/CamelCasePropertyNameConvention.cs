namespace Detached.Mappers.TypeOptions.Conventions
{
    public class CamelCasePropertyNameConvention : IPropertyNameConvention
    {
        public string GetSourcePropertyName(ITypeOptions sourceType, ITypeOptions targetType, string targetMemberName)
        {
            return char.ToLower(targetMemberName[0]) + targetMemberName.Substring(1);
        }
    }
}
