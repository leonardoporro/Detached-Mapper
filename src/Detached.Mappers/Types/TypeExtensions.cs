using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.RuntimeTypes.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Detached.Mappers.Types
{
    public static class TypeExtensions
    {
        static Dictionary<Type, string> _shortNames = new Dictionary<Type, string>
        {
            {typeof(sbyte), "sbyte"},
            {typeof(bool), "bool"},
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(decimal), "decimal"},
            {typeof(char), "char"},
            {typeof(string), "string"},
            {typeof(object), "object"},
            {typeof(void), "void"},
            {typeof(int), "int"},
            {typeof(uint), "uint"},
            {typeof(long), "long"},
            {typeof(ulong), "ulong"},
            {typeof(short), "short"},
            {typeof(ushort), "ushort"},
            {typeof(byte), "byte"}
        };

        public static string GetFriendlyName(this Type type, Dictionary<Type, string> translations)
        {
            if (translations.ContainsKey(type))
                return translations[type];
            else if (type.IsArray)
            {
                var rank = type.GetArrayRank();
                var commas = rank > 1
                    ? new string(',', rank - 1)
                    : "";
                return GetFriendlyName(type.GetElementType(), translations) + $"[{commas}]";
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return type.GetGenericArguments()[0].GetFriendlyName() + "?";
            else if (type.IsGenericType)
                return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(x => GetFriendlyName(x)).ToArray()) + ">";
            else
                return type.Name;
        }

        public static string GetFriendlyName(this Type type)
        {
            return type.GetFriendlyName(_shortNames);
        }

        public static PropertyInfo GetPropertyInfo(this ITypeMember memberOptions)
        {
            if (memberOptions is ClassTypeMember clrMemberOptions)
            {
                return clrMemberOptions.PropertyInfo;
            }
            else
            {
                return null;
            }
        }

        public static bool IsPrimitive(this IType type)
        {
            return type.MappingSchema == MappingSchema.Primitive;
        }

        public static bool IsCollection(this IType type)
        {
            return type.MappingSchema == MappingSchema.Collection;
        }

        public static bool IsComplex(this IType type)
        {
            return type.MappingSchema == MappingSchema.Complex;
        }

        public static bool IsComplexOrEntity(this IType type)
        {
            return type.IsComplex() || type.IsEntity();
        }

        public static bool IsNullable(this IType type)
        {
            return type.ClrType.IsNullable(out _);
        }

        public static bool IsConcrete(this IType type)
        {
            return !(type.IsAbstract() || type.IsInherited());
        }

        public static ITypeMember? GetKeyMember(this IType type)
        {
            ITypeMember result = null;

            if (type.MemberNames != null)
            {
                foreach (string memberName in type.MemberNames)
                {
                    var member = type.GetMember(memberName);

                    if (member.IsKey())
                    {
                        if (result == null)
                        {
                            result = member;
                        }
                        else
                        {
                            result = null;
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}