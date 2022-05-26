using Detached.Mappers.Annotations;
using Detached.Mappers.TypeOptions.Class.Conventions;
using Detached.PatchTypes;
using Detached.RuntimeTypes.Reflection;
using System;
using System.Linq.Expressions;
using System.Reflection;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeOptions.Class
{
    public class ClassTypeOptionsFactory : ITypeOptionsFactory
    {
        public ITypeOptions Create(MapperOptions options, Type type)
        {
            ClassTypeOptions typeOptions = new ClassTypeOptions();
            typeOptions.ClrType = type;

            typeOptions.IsAbstract = type == typeof(object) || type.IsAbstract || type.IsInterface;

            if (IsPrimitive(options, type))
            {
                typeOptions.IsPrimitive = true;
            }
            else if (type.IsEnumerable(out Type itemType))
            {
                typeOptions.ItemType = itemType;
                typeOptions.IsCollection = true;
            }
            else if (type.IsNullable(out Type baseType))
            {
                typeOptions.IsNullable = true;
                typeOptions.ItemType = baseType;
            }
            else
            {
                typeOptions.IsComplex = true;

                // generate members.
                foreach (PropertyInfo propInfo in type.GetRuntimeProperties())
                {
                    if (ShouldMap(propInfo))
                    {
                        ClassMemberOptions memberOptions = new ClassMemberOptions();
                        memberOptions.Name = propInfo.Name;
                        memberOptions.ClrType = propInfo.PropertyType;

                        // generate getter.
                        if (propInfo.CanRead)
                        {
                            memberOptions.Getter = Lambda(
                                    Parameter(type, out Expression instanceExpr),
                                    Property(instanceExpr, propInfo)
                                );
                        }

                        // generate setter.
                        if (propInfo.CanWrite)
                        {
                            memberOptions.Setter = Lambda(
                                   Parameter(type, out Expression instanceExpr),
                                   Parameter(propInfo.PropertyType, out Expression valueExpr),
                                   Assign(Property(instanceExpr, propInfo), valueExpr)
                               );
                        }

                        // apply member attributes.
                        foreach (Attribute annotation in propInfo.GetCustomAttributes())
                        {
                            if (options.AnnotationHandlers.TryGetValue(annotation.GetType(), out IAnnotationHandler handler))
                            {
                                handler.Apply(annotation, options, typeOptions, memberOptions);
                            }
                        }

                        typeOptions.Members.Add(memberOptions);
                    }
                }
            }

            if (typeOptions.IsComplex && !typeOptions.IsAbstract)
            {
                typeOptions.Constructor = Lambda(New(type));
            }

            // apply type attributes.
            foreach (Attribute annotation in type.GetCustomAttributes())
            {
                if (options.AnnotationHandlers.TryGetValue(annotation.GetType(), out IAnnotationHandler handler))
                {
                    handler.Apply(annotation, options, typeOptions, null);
                }
            }

            // apply conventions.
            foreach (ITypeOptionsConvention convention in options.Conventions)
            {
                convention.Apply(options, typeOptions);
            }

            // manual configuration is applied after all of this.
            return typeOptions;
        }

        protected virtual bool ShouldMap(PropertyInfo propInfo)
        {
            bool result = true;

            if (propInfo.Name == "Modified"
                && typeof(IPatch).IsAssignableFrom(propInfo.DeclaringType))
            {
                result = false;
            }

            return result;
        }

        protected virtual bool IsPrimitive(MapperOptions options, Type type)
        {
            if (type.IsEnum)
                return true;
            //else if (type.IsEnumerable(out Type elementType) && IsPrimitive(options, elementType))
            //    return true;
            else if (type.IsGenericType)
                return options.Primitives.Contains(type.GetGenericTypeDefinition());
            else
                return options.Primitives.Contains(type);
        }
    }
}