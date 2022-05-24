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
            typeMap.ParentTypeMap = parentMap;
            typeMap.Mapper = mapper;
            typeMap.SourceTypeOptions = options.GetTypeOptions(sourceType);
            typeMap.SourceExpression = Parameter(sourceType, "source_" + sourceType.Name);
            typeMap.TargetTypeOptions = options.GetTypeOptions(targetType);
            typeMap.TargetExpression = Parameter(targetType, "target_" + targetType.Name);
            typeMap.IsComposition = isComposition;

            if (parentMap == null)
                typeMap.BuildContextExpression = Parameter(typeof(IMapperContext), "context");
            else
                typeMap.BuildContextExpression = parentMap.BuildContextExpression;

            while (parentMap != null)
            {
                if (typeMap.TargetTypeOptions == parentMap.TargetTypeOptions
                    && typeMap.SourceTypeOptions == parentMap.SourceTypeOptions
                    && typeMap.IsComposition == parentMap.IsComposition)
                {
                    return parentMap;
                }
                parentMap = parentMap.ParentTypeMap;
            }

            if (typeMap.TargetTypeOptions.IsCollectionType)
            {
                Type sourceItemType = typeMap.SourceTypeOptions.ItemType;
                Type targetItemType = typeMap.TargetTypeOptions.ItemType;

                if (typeMap.SourceTypeOptions.Type == typeof(object))
                    sourceItemType = typeof(object);

                typeMap.ItemTypeMap = Create(mapper, options, typeMap, sourceItemType, targetItemType, isComposition);
            }
            else if (typeMap.TargetTypeOptions.IsComplexType)
            {
                IEnumerable<string> memberNames = typeMap.TargetTypeOptions.MemberNames;
                if (memberNames != null)
                {
                    foreach (string memberName in memberNames)
                    {
                        IMemberOptions targetMemberOptions = typeMap.TargetTypeOptions.GetMember(memberName);

                        if (targetMemberOptions != null && targetMemberOptions.CanWrite && !targetMemberOptions.Ignored)
                        {
                            if (IsBackReference(options, typeMap, targetMemberOptions, out BackReferenceMap backRefMap))
                            {
                                typeMap.BackReferenceMap = backRefMap;
                            }
                            else
                            {
                                IMemberOptions sourceMemberOptions = typeMap.SourceTypeOptions.GetMember(memberName);

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

                                    if (memberName == typeMap.TargetTypeOptions.DiscriminatorName)
                                    {
                                        typeMap.DiscriminatorMember = memberMap.SourceOptions;
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

            if (typeMap.ParentTypeMap != null)
            {
                if (typeMap.TargetTypeOptions.IsEntityType
                    && (typeMap.ParentTypeMap.TargetTypeOptions.Type == memberTypeOptions.Type
                           || typeMap.ParentTypeMap.TargetTypeOptions.Type == memberTypeOptions.ItemType))
                {
                    backReferenceMap = new BackReferenceMap
                    {

                        MemberOptions = memberOptions,
                        MemberTypeOptions = options.GetTypeOptions(memberOptions.Type),
                        Parent = typeMap.ParentTypeMap
                    };
                    result = true;
                }
                else if (typeMap.ParentTypeMap.ParentTypeMap != null
                       && typeMap.ParentTypeMap.TargetTypeOptions.IsCollectionType
                       && (typeMap.ParentTypeMap.ParentTypeMap.TargetTypeOptions.Type == memberTypeOptions.Type
                           || typeMap.ParentTypeMap.ParentTypeMap.TargetTypeOptions.Type == memberTypeOptions.ItemType))
                {
                    backReferenceMap = new BackReferenceMap
                    {
                        MemberOptions = memberOptions,
                        MemberTypeOptions = options.GetTypeOptions(memberOptions.Type),
                        Parent = typeMap.ParentTypeMap.ParentTypeMap
                    };
                    result = true;
                }
            }

            return result;
        }
    }
}