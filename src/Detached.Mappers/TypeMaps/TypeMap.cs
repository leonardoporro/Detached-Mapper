using AgileObjects.ReadableExpressions.Extensions;
using Detached.Mappers.TypeOptions;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMaps
{
    public class TypeMap
    {
        public Mapper Mapper { get; set; }

        public TypeMap ParentTypeMap { get; set; }

        public BackReferenceMap BackReferenceMap { get; set; }

        public TypeMap ItemTypeMap { get; set; }

        public List<MemberMap> Members { get; } = new List<MemberMap>();

        public ITypeOptions SourceTypeOptions { get; set; }

        public ITypeOptions TargetTypeOptions { get; set; }

        public IMemberOptions DiscriminatorMember { get; set; }

        public ParameterExpression BuildContextExpression { get; set; }

        public ParameterExpression SourceExpression { get; set; }

        public ParameterExpression TargetExpression { get; set; }

        public ParameterExpression MappingFunctionExpression { get; set; }

        public Expression SourceKeyExpression { get; set; }

        public Expression TargetKeyExpression { get; set; }

        public bool IsComposition { get; set; }

        public override string ToString() 
            => $"{SourceTypeOptions.ClrType.GetFriendlyName()} to {TargetTypeOptions.ClrType.GetFriendlyName()} (TypeMap)";
    }
}