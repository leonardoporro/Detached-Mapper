using Detached.Mappers.Context;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeMaps
{
    public class TypeMapFactory
    {
        public TypeMap Create(Mapper mapper, MapperOptions options, TypeMap parentMap, Type sourceType, Type targetType, bool isComposition)
        {
            TypeMap typeMap = new TypeMap();
            typeMap.Parent = parentMap;
            typeMap.Mapper = mapper;
            typeMap.SourceOptions = options.GetTypeOptions(sourceType);
            typeMap.Source = Parameter(sourceType, "source_" + sourceType.Name);
            typeMap.TargetOptions = options.GetTypeOptions(targetType);
            typeMap.Target = Parameter(targetType, "target_" + targetType.Name);
            typeMap.IsComposition = isComposition;

            if (parentMap == null)
                typeMap.Context = Parameter(typeof(IMapperContext), "context");
            else
                typeMap.Context = parentMap.Context;

            while (parentMap != null)
            {
                if (typeMap.TargetOptions == parentMap.TargetOptions
                    && typeMap.SourceOptions == parentMap.SourceOptions
                    && typeMap.IsComposition == parentMap.IsComposition)
                {
                    return parentMap;
                }
                parentMap = parentMap.Parent;
            }

            if (typeMap.TargetOptions.IsCollection)
            {
                Type sourceItemType = typeMap.SourceOptions.ItemType;
                Type targetItemType = typeMap.TargetOptions.ItemType;

                if (typeMap.SourceOptions.Type == typeof(object))
                    sourceItemType = typeof(object);

                typeMap.ItemMap = Create(mapper, options, typeMap, sourceItemType, targetItemType, isComposition);
            }
            else if (typeMap.TargetOptions.IsComplexType)
            {
                IEnumerable<string> memberNames = typeMap.TargetOptions.MemberNames;
                if (memberNames != null)
                {
                    foreach (string memberName in memberNames)
                    {
                        IMemberOptions targetMemberOptions = typeMap.TargetOptions.GetMember(memberName);

                        if (targetMemberOptions != null && targetMemberOptions.CanWrite && !targetMemberOptions.Ignored)
                        {
                            if (IsBackReference(options, typeMap, targetMemberOptions, out BackReferenceMap backRefMap))
                            {
                                typeMap.BackReference = backRefMap;
                            }
                            else
                            {
                                IMemberOptions sourceMemberOptions = typeMap.SourceOptions.GetMember(memberName);

                                if (sourceMemberOptions != null && sourceMemberOptions.CanRead && !sourceMemberOptions.Ignored)
                                {
                                    MemberMap memberMap = new MemberMap();
                                    memberMap.SourceOptions = sourceMemberOptions;
                                    memberMap.TargetOptions = targetMemberOptions;
                                    memberMap.TypeMap = Create(
                                        mapper,
                                        options,
                                        typeMap,
                                        memberMap.SourceOptions.Type,
                                        memberMap.TargetOptions.Type,
                                        targetMemberOptions.IsComposition);

                                    typeMap.Members.Add(memberMap);

                                    if (memberName == typeMap.TargetOptions.DiscriminatorName)
                                    {
                                        typeMap.Discriminator = memberMap.SourceOptions;
                                    }
                                }
                            }
                        }
                    }
                }
            }
              
            return typeMap;
        }

        public bool IsBackReference(
            MapperOptions options,
            TypeMap typeMap,
            IMemberOptions memberOptions,
            out BackReferenceMap backReferenceMap)
        {
            bool result = false;
            backReferenceMap = null;

            ITypeOptions memberTypeOptions = options.GetTypeOptions(memberOptions.Type);

            if (typeMap.Parent != null)
            {
                if (typeMap.TargetOptions.IsEntity
                    && (typeMap.Parent.TargetOptions.Type == memberTypeOptions.Type
                           || typeMap.Parent.TargetOptions.Type == memberTypeOptions.ItemType))
                {
                    backReferenceMap = new BackReferenceMap
                    {

                        MemberOptions = memberOptions,
                        MemberTypeOptions = options.GetTypeOptions(memberOptions.Type),
                        Parent = typeMap.Parent
                    };
                    result = true;
                }
                else if (typeMap.Parent.Parent != null
                       && typeMap.Parent.TargetOptions.IsCollection
                       && (typeMap.Parent.Parent.TargetOptions.Type == memberTypeOptions.Type
                           || typeMap.Parent.Parent.TargetOptions.Type == memberTypeOptions.ItemType))
                {
                    backReferenceMap = new BackReferenceMap
                    {
                        MemberOptions = memberOptions,
                        MemberTypeOptions = options.GetTypeOptions(memberOptions.Type),
                        Parent = typeMap.Parent.Parent
                    };
                    result = true;
                }
            }

            return result;
        }
    }
}