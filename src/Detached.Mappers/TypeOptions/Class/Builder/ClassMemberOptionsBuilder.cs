using Detached.Mappers.Annotations;
using System;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;

namespace Detached.Mappers.TypeOptions.Class.Builder
{
    public class ClassMemberOptionsBuilder<TType, TMember> : ClassTypeOptionsBuilder<TType>
    {
        public ClassMemberOptionsBuilder(ClassTypeOptions typeOptions, ClassMemberOptions memberOptions, MapperOptions mapperOptions)
            : base(typeOptions, mapperOptions)
        {
            MemberOptions = memberOptions;
        }

        public ClassMemberOptions MemberOptions { get; }

        public ClassMemberOptionsBuilder<TType, TMember> Getter(LambdaExpression lambda)
        {
            MemberOptions.Getter = lambda;
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Getter(Func<TType, IMapContext, TMember> fn)
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

        public ClassMemberOptionsBuilder<TType, TMember> Exclude()
        {
            MemberOptions.IsNotMapped(true);
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Include()
        {
            MemberOptions.IsNotMapped(false);
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Setter(LambdaExpression lambda)
        {
            MemberOptions.Setter = lambda;
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Setter(Action<TType, TMember, IMapContext> fn)
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