using AgileObjects.ReadableExpressions.Extensions;
using Detached.Mappers.TypeOptions;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMaps
{
    public class TypeMap
    {
        public Mapper Mapper { get; set; }

        public TypeMap Parent { get; set; }

        public BackReferenceMap BackReference { get; set; }

        public TypeMap ItemMap { get; set; }

        public List<MemberMap> Members { get; } = new List<MemberMap>();

        public ITypeOptions SourceOptions { get; set; }

        public ITypeOptions TargetOptions { get; set; }

        public IMemberOptions Discriminator { get; set; }

        public ParameterExpression BuildContextExpr { get; set; }

        public ParameterExpression SourceExpr { get; set; }

        public ParameterExpression TargetExpr { get; set; }

        public ParameterExpression MapperFnExpr { get; set; }

        public Expression SourceKeyExpr { get; set; }

        public Expression TargetKeyExpr { get; set; }

        public bool IsComposition { get; set; }

        public override string ToString() 
            => $"{SourceOptions.Type.GetFriendlyName()} to {TargetOptions.Type.GetFriendlyName()} (TypeMap)";
    }
}