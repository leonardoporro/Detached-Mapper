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

        public ParameterExpression Source { get; set; }

        public ITypeOptions SourceOptions { get; set; }

        public ParameterExpression Target { get; set; }

        public ITypeOptions TargetOptions { get; set; }

        public ParameterExpression Context { get; set; }

        public ParameterExpression MapReference { get; set; }

        public BackReferenceMap BackReference { get; set; }

        public Expression SourceKey { get; set; }

        public Expression TargetKey { get; set; }

        public bool IsComposition { get; set; }

        public TypeMap ItemMap { get; set; }

        public List<MemberMap> Members { get; } = new List<MemberMap>();

        public override string ToString() 
            => $"{SourceOptions.Type.GetFriendlyName()} to {TargetOptions.Type.GetFriendlyName()} (TypeMap)";
    }
}