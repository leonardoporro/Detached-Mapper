using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeBinders.Binders
{
    public class ComplexTypeBinder : ITypeBinder
    {
        public bool CanBind(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsComplex() && typePair.TargetType.IsComplex();
        }

        public Expression Bind(Mapper mapper, TypePair typePair, Expression sourceExpr)
        {
            MapperOptions options = mapper.Options;

            List<MemberBinding> bindings = new List<MemberBinding>();

            foreach (TypePairMember memberPair in typePair.Members.Values.Where(v => v.IsMapped()))
            {
                PropertyInfo propInfo = memberPair.TargetMember.GetPropertyInfo();
                if (propInfo != null)
                {
                    IType projectionMemberType = options.GetType(memberPair.TargetMember.ClrType);
                    IType entityMemberType = options.GetType(memberPair.SourceMember.ClrType);
                    TypePair memberTypePair = options.GetTypePair(entityMemberType, projectionMemberType, memberPair);

                    Expression memberSourceExpr = memberPair.SourceMember.BuildGetExpression(sourceExpr, null);

                    var memberBinder = mapper.GetTypeBinder(memberTypePair);

                    var memberExpr = memberBinder.Bind(mapper, memberTypePair, memberSourceExpr);

                    if (memberExpr != null)
                    {
                        if (!entityMemberType.IsPrimitive())
                        {
                            memberExpr = Condition(NotEqual(memberSourceExpr, Constant(null, memberSourceExpr.Type)), memberExpr, Constant(null, memberExpr.Type));
                        }

                        bindings.Add(Expression.Bind(propInfo, memberExpr));
                    }
                }
            }

            return MemberInit(New(typePair.TargetType.ClrType), bindings.ToArray());
        }
    }
}
