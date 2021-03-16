using Detached.Mappers.Annotations;
using Detached.Mappers.Model.Types.Class.Conventions;
using Detached.PatchTypes;
using Detached.RuntimeTypes.Reflection;
using System;
using System.Linq.Expressions;
using System.Reflection;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Model.Types.Class
{
    public class ClassOptionsFactory : ITypeOptionsFactory
    {
        public ITypeOptions Create(MapperOptions options, Type type)
        {
            ClassTypeOptions typeOptions = new ClassTypeOptions();
            typeOptions.Type = type;

            // determine the object type: Entity (has members), Collection or Plain Value
            if (IsPrimitive(options, type))
            {
                typeOptions.IsValue = true;
            }
            else if (type.IsEnumerable(out Type itemType))
            {
                typeOptions.ItemType = itemType;
                typeOptions.IsCollection = true;
            }
            else
            {
                typeOptions.IsComplexType = true;

                // generate members.
                foreach (PropertyInfo propInfo in type.GetRuntimeProperties())
                {
                    if (ShouldMap(propInfo))
                    {
                        ClassMemberOptions memberOptions = new ClassMemberOptions();
                        memberOptions.Name = propInfo.Name;
                        memberOptions.Type = propInfo.PropertyType;

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

            CreateConstructor(options, type, typeOptions);

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

        protected virtual void CreateConstructor(MapperOptions options, Type type, ClassTypeOptions typeOptions)
        {
            if (type.IsInterface)
            {
                Type concreteType;

                if (type.IsGenericType)
                {
                    options.ConcreteTypes.TryGetValue(type.GetGenericTypeDefinition(), out concreteType);

                    concreteType = concreteType.MakeGenericType(type.GetGenericArguments());
                }
                else
                {
                    options.ConcreteTypes.TryGetValue(type.GetGenericTypeDefinition(), out concreteType);
                }

                typeOptions.Constructor = Lambda(New(concreteType));
            }
            else if (type.IsClass && type.GetConstructor(new Type[0]) != null)
            {
                typeOptions.Constructor = Lambda(New(type));
            }
        }

        protected virtual bool IsPrimitive(MapperOptions options, Type type)
        {
            if (type.IsEnum)
                return true;
            else if (type.IsEnumerable(out Type elementType) && IsPrimitive(options, elementType))
                return true;
            else if (type.IsGenericType)
                return options.Primitives.Contains(type.GetGenericTypeDefinition());
            else
                return options.Primitives.Contains(type);
        }
    }
}