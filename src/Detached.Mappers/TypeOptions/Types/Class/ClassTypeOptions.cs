using AgileObjects.ReadableExpressions.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;
using static Detached.Mappers.Extensions.MapperExpressionExtensions;

namespace Detached.Mappers.TypeOptions.Types.Class
{
    public class ClassTypeOptions : ITypeOptions
    {
        public virtual Type Type { get; set; }

        public virtual Type ItemType { get; set; }

        public virtual ClassMemberOptionsCollection Members { get; set; } = new ClassMemberOptionsCollection();

        public virtual IEnumerable<string> MemberNames => Members.Keys;

        public virtual string DiscriminatorName { get; set; }

        public virtual Dictionary<object, Type> DiscriminatorValues { get; } = new Dictionary<object, Type>();

        public virtual IMemberOptions GetMember(string memberName)
        {
            Members.TryGetValue(memberName, out ClassMemberOptions memberOptions);
            return memberOptions;
        }

        public virtual Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public virtual bool IsPrimitiveType { get; set; }

        public virtual bool IsCollectionType { get; set; }

        public virtual bool IsComplexType { get; set; }

        public virtual bool IsEntityType { get; set; }

        public virtual bool IsFragment { get; set; }

        public virtual LambdaExpression Constructor { get; set; }

        public virtual Expression Construct(Expression context, Expression discriminator)
        {
            if (DiscriminatorName != null)
            {
                Type funcType = typeof(Func<>).MakeGenericType(Type);
                Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(discriminator.Type, funcType);
                IDictionary typeTable = (IDictionary)Activator.CreateInstance(dictionaryType);

                foreach (var entry in DiscriminatorValues)
                {
                    typeTable.Add(entry.Key, Lambda(Convert(New(entry.Value), Type)).Compile());
                }

                Expression construct = Block(
                    Variable("type", funcType, out Expression typeVar),
                    If(Not(Call("TryGetValue", Constant(typeTable, dictionaryType), discriminator, typeVar)),
                       ThrowMapperException("{0} is not a valid value for discriminator in entity {1}.", discriminator, Constant(Type))
                    ),
                    Invoke(typeVar)
                );

                return construct;
            }
            else
            {
                if (Constructor == null)
                {
                    throw new InvalidOperationException($"Can't construct {Type.GetFriendlyName()}. It does not have a parameterless constructor or a concrete type specified.");
                }

                return Import(Constructor, context);
            }
        }

        public override string ToString() => $"{Type.GetFriendlyName()} (EntityOptions)";
    }
}