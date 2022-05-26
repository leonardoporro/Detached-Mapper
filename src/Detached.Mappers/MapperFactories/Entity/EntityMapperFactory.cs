using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.MapperFactories.Entity
{
    public abstract class EntityMapperFactory : ComplexTypeMapperFactory
    {
        public Expression CreateBackReference(TypeMap typeMap)
        {
            if (typeMap.BackReferenceMap != null)
            {
                BackReferenceMap backRef = typeMap.BackReferenceMap;

                if (backRef.MemberTypeOptions.IsCollection)
                {
                    Expression list = backRef.MemberOptions.BuildGetterExpression(typeMap.TargetExpression, typeMap.BuildContextExpression);
                    Expression item = backRef.Parent.TargetExpression;
                    Expression newList = backRef.MemberTypeOptions.BuildNewExpression(typeMap.BuildContextExpression, null);

                    return Block(
                        If(IsNull(list), Assign(list, newList)),
                        IfThen(Not(Call("Contains", list, item)),
                            Call("Add", list, item)
                        )
                    );
                }
                else
                {
                    return typeMap.BackReferenceMap.MemberOptions.BuildSetterExpression(
                                  typeMap.TargetExpression,
                                  typeMap.BackReferenceMap.Parent.TargetExpression,
                                  typeMap.BuildContextExpression);
                }
            }
            else
            {
                return Empty();
            }
        }

        protected virtual bool CanMapKey(TypeMap typeMap)
        {
            return typeMap.Members.Where(m => m.IsKey).Any();
        }

        protected virtual Expression CreateKey(TypeMap typeMap)
        {
            if (typeMap.TargetKeyExpression == null || typeMap.SourceKeyExpression == null)
            {
                List<MemberMap> keyMembers = new List<MemberMap>();
                foreach (MemberMap memberMap in typeMap.Members)
                {
                    if (memberMap.IsKey)
                        keyMembers.Add(memberMap);
                }

                if (keyMembers.Count == 0)
                {
                    typeMap.SourceKeyExpression = Constant(NoKey.Instance);
                    typeMap.TargetKeyExpression = Constant(NoKey.Instance);
                }
                else
                {
                    Expression[] sourceKeyMembers = new Expression[keyMembers.Count];
                    Expression[] targetKeyMembers = new Expression[keyMembers.Count];
                    Type[] keyMemberTypes = new Type[keyMembers.Count];

                    for (int i = 0; i < keyMembers.Count; i++)
                    {
                        MemberMap keyMember = keyMembers[i];
                        sourceKeyMembers[i] = keyMember.SourceOptions.BuildGetterExpression(typeMap.SourceExpression, typeMap.BuildContextExpression);
                        targetKeyMembers[i] = keyMember.TargetOptions.BuildGetterExpression(typeMap.TargetExpression, typeMap.BuildContextExpression);
                        sourceKeyMembers[i] = CallMapper(keyMember.TypeMap, sourceKeyMembers[i], Default(targetKeyMembers[i].Type));
                        keyMemberTypes[i] = keyMembers[i].TargetOptions.ClrType;
                    }

                    Type keyType = GetKeyType(keyMemberTypes);
                    typeMap.SourceKeyExpression = New(keyType, sourceKeyMembers);
                    typeMap.TargetKeyExpression = New(keyType, targetKeyMembers);
                }
            }

            return Empty();
        }

        protected virtual Type GetKeyType(Type[] types)
        {
            switch (types.Length)
            {
                case 1:
                    return typeof(EntityKey<>).MakeGenericType(types);
                case 2:
                    return typeof(EntityKey<,>).MakeGenericType(types);
                case 3:
                    return typeof(EntityKey<,,>).MakeGenericType(types);
                case 4:
                    return typeof(EntityKey<,,,>).MakeGenericType(types);
                case 5:
                    return typeof(EntityKey<,,,,>).MakeGenericType(types);
                default:
                    throw new InvalidOperationException("Maximum of 5 key members allowed.");
            }
        }
    }
}