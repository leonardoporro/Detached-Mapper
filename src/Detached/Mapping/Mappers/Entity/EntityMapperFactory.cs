using Detached.Mapping.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mapping.Mappers.Entity
{
    public abstract class EntityMapperFactory : ComplexTypeMapperFactory
    {
        public Expression CreateBackReference(TypeMap typeMap)
        {
            if (typeMap.BackReference != null)
            {
                BackReferenceMap backRef = typeMap.BackReference;

                if (backRef.MemberTypeOptions.IsCollection)
                {
                    Expression list = backRef.MemberOptions.GetValue(typeMap.Target, typeMap.Context);
                    Expression item = backRef.Parent.Target;
                    Expression newList = backRef.MemberTypeOptions.Construct(typeMap.Context);

                    return Block(
                        If(IsNull(list), Assign(list, newList)),
                        IfThen(Not(Call("Contains", list, item)),
                            Call("Add", list, item)
                        )
                    );
                }
                else
                {
                    return typeMap.BackReference.MemberOptions.SetValue(
                                  typeMap.Target,
                                  typeMap.BackReference.Parent.Target,
                                  typeMap.Context);
                }
            }
            else
            {
                return Empty();
            }
        }

        protected virtual Expression CreateKey(TypeMap typeMap)
        {
            if (typeMap.TargetKey == null || typeMap.SourceKey == null)
            {
                List<MemberMap> keyMembers = new List<MemberMap>();
                foreach (MemberMap memberMap in typeMap.Members)
                {
                    if (memberMap.IsKey)
                        keyMembers.Add(memberMap);
                }

                if (keyMembers.Count == 0)
                {
                    throw new MapperException($"Entity '{typeMap.TargetOptions.Type}' doesn't have a Key defined.");
                }

                Expression[] sourceKeyMembers = new Expression[keyMembers.Count];
                Expression[] targetKeyMembers = new Expression[keyMembers.Count];
                Type[] keyMemberTypes = new Type[keyMembers.Count];

                for (int i = 0; i < keyMembers.Count; i++)
                {
                    MemberMap keyMember = keyMembers[i];
                    sourceKeyMembers[i] = keyMember.SourceOptions.GetValue(typeMap.Source, typeMap.Context);
                    targetKeyMembers[i] = keyMember.TargetOptions.GetValue(typeMap.Target, typeMap.Context);
                    sourceKeyMembers[i] = CallMapper(keyMember.TypeMap, sourceKeyMembers[i], Default(targetKeyMembers[i].Type));
                    keyMemberTypes[i] = keyMembers[i].TargetOptions.Type;
                }

                if (keyMembers.Count == 0)
                {
                    throw new InvalidOperationException($"Map {typeMap} doesn't have a key.");
                }
                else
                {
                    Type keyType = GetKeyType(keyMemberTypes);
                    typeMap.SourceKey = New(keyType, sourceKeyMembers);
                    typeMap.TargetKey = New(keyType, targetKeyMembers);
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