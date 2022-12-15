using System;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Types.Class.Builder
{
    public class ClassTypeMemberBuilder<TType, TMember> : ClassTypeBuilder<TType>
    {
        public ClassTypeMemberBuilder(ClassType typeOptions, ClassTypeMember memberOptions, MapperOptions mapperOptions)
            : base(typeOptions, mapperOptions)
        {
            MemberOptions = memberOptions;
        }

        public ClassTypeMember MemberOptions { get; }

        public ClassTypeMemberBuilder<TType, TMember> Getter(LambdaExpression lambda)
        {
            MemberOptions.Getter = lambda;
            return this;
        }

        public ClassTypeMemberBuilder<TType, TMember> Getter(Func<TType, IMapContext, TMember> fn)
        {
            Expression instance = null;
            if (fn.Target != null)
            {
                instance = Constant(fn.Target, fn.Target.GetType());
            }

            MemberOptions.Getter =
                Lambda(
                   typeof(Func<TType, IMapContext, TMember>),
                   Parameter("target", typeof(TType), out Expression target),
                   Parameter("context", typeof(IMapContext), out Expression context),

                   Call(instance, fn.Method, target, context)
                );

            return this;
        }

        public ClassTypeMemberBuilder<TType, TMember> Setter(LambdaExpression lambda)
        {
            MemberOptions.Setter = lambda;
            return this;
        }

        public ClassTypeMemberBuilder<TType, TMember> Setter(Action<TType, TMember, IMapContext> fn)
        {
            Expression instance = null;
            if (fn.Target != null)
            {
                instance = Constant(fn.Target, fn.Target.GetType());
            }

            MemberOptions.Setter =
                Lambda(
                   typeof(Action<TType, TMember, IMapContext>),
                   Parameter("target", typeof(TType), out Expression target),
                   Parameter("value", typeof(TMember), out Expression value),
                   Parameter("context", typeof(IMapContext), out Expression context),

                   Call(instance, fn.Method, target, value, context)
                );

            return this;
        }
    }
}