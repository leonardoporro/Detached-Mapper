using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Detached.Mappers.Extensions
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
    }
}