using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.RuntimeTypes.Reflection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.EntityFramework.Loaders
{
    /// <summary>
    /// LoaderQueryFactory is a 
    public class LoaderQueryFactory
    {
        /// <summary>
        /// Constructor for LoaderQueryFactory class.
        /// </summary>
        /// <param name="options">MapperOptions object.</param>
        /// <returns>
        /// No return value.
        /// </returns>
        public LoaderQueryFactory(MapperOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// Gets the MapperOptions object.
        /// </summary>
        public MapperOptions Options { get; }

        /// <summary>
        /// Creates a new ILoaderQuery object based on the given source and target CLR types.
        /// </summary>
        /// <param name="sourceClrType">The source CLR type.</param>
        /// <param name="targetClrType">The target CLR type.</param>
        /// <returns>A new ILoaderQuery object.</returns>
        public ILoaderQuery Create(Type sourceClrType, Type targetClrType)
        {
            IType sourceType = Options.GetType(sourceClrType);
            IType targetType = Options.GetType(targetClrType);
            TypePair typePair = Options.GetTypePair(sourceType, targetType, null);

            var parameter = Constant(null, sourceClrType);
            var filter = CreateFilterExpression(typePair, parameter);

            if (filter == null)
            {
                return new NoopQuery();
            }

            var includes = GetIncludes(typePair);

            var loaderType = typeof(LoaderQuery<,>).MakeGenericType(sourceClrType, targetClrType);

            return (ILoaderQuery)Activator.CreateInstance(loaderType, parameter, filter, includes);
        }

        /// <summary>
        /// Creates a filter expression for the given type pair and source constant.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <param name="sourceConstant">The source constant.</param>
        /// <returns>The filter expression.</returns>
        Expression CreateFilterExpression(TypePair typePair, ConstantExpression sourceConstant)
        {
            var targetParam = Parameter(typePair.TargetType.ClrType, "e");

            Expression expression = null;

            foreach (TypePairMember memberPair in typePair.Members.Values.Where(p => p.IsKey()))
            {
                if (memberPair.IsIgnored() || memberPair.SourceMember == null)
                {
                    return null;
                }

                var targetExpr = memberPair.TargetMember.BuildGetExpression(targetParam, null);
                var sourceExpr = memberPair.SourceMember.BuildGetExpression(sourceConstant, null);

                if (sourceExpr.Type.IsNullable(out Type sourceBaseType) && targetExpr.Type == sourceBaseType)
                {
                    sourceExpr = OrElse(Equal(sourceExpr, Constant(null, sourceExpr.Type)), Equal(Property(sourceExpr, "Value"), targetExpr));
                }
                else if (sourceExpr.Type != targetExpr.Type)
                {
                    sourceExpr = Equal(Convert(sourceExpr, targetExpr.Type), targetExpr);
                }
                else
                {
                    sourceExpr = Equal(sourceExpr, targetExpr);
                }

                if (expression == null)
                    expression = sourceExpr;
                else
                    expression = And(expression, sourceExpr);
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typePair.TargetType.ClrType, typeof(bool));

            return Expression.Lambda(delegateType, expression, targetParam);
        }

        /// <summary>
        /// Gets the includes for a given type pair.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>A list of includes.</returns>
        List<string> GetIncludes(TypePair typePair)
        {
            List<string> result = new List<string>();
            Stack<IType> stack = new Stack<IType>();

            GetIncludes(typePair, stack, null, result);

            for (int i = result.Count - 1; i >= 0; i--)
            {
                string descendantPrefix = result[i] + ".";
                if (result.Exists(i => i.StartsWith(descendantPrefix)))
                {
                    result.RemoveAt(i);
                }
            }

            return result;
        }

        /// <summary>
        /// Recursively gets the includes for a given type pair.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <param name="stack">The stack.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="result">The result.</param>
        /// <returns>
        /// A list of includes for the given type pair.
        /// </returns>
        void GetIncludes(TypePair typePair, Stack<IType> stack, string prefix, List<string> result)
        {
            stack.Push(typePair.TargetType);

            foreach (TypePairMember memberPair in typePair.Members.Values)
            {
                if (memberPair.IsMapped() && !memberPair.TargetMember.IsParent() && !memberPair.IsSetAsPrimitive() && memberPair.SourceMember != null)
                {
                    IType sourceMemberType = Options.GetType(memberPair.SourceMember.ClrType);
                    IType targetMemberType = Options.GetType(memberPair.TargetMember.ClrType);

                    if (!stack.Contains(targetMemberType))
                    {
                        if (targetMemberType.IsCollection() && Options.GetType(targetMemberType.ItemClrType).IsEntity())
                        {
                            string name = prefix + memberPair.TargetMember.Name;
                            result.Add(name);

                            if (memberPair.TargetMember.IsComposition())
                            {
                                IType sourceItemType = Options.GetType(sourceMemberType.ItemClrType);
                                IType targetItemType = Options.GetType(targetMemberType.ItemClrType);
                                TypePair itemTypePair = Options.GetTypePair(sourceItemType, targetItemType, memberPair);

                                GetIncludes(itemTypePair, stack, name + ".", result);
                            }

                        }
                        else if (targetMemberType.IsEntity())
                        {
                            string name = prefix + memberPair.TargetMember.Name;
                            result.Add(name);

                            if (memberPair.TargetMember.IsComposition())
                            {
                                TypePair memberTypePair = Options.GetTypePair(sourceMemberType, targetMemberType, memberPair);
                                GetIncludes(memberTypePair, stack, name + ".", result);
                            }
                        }
                    }
                }
            }

            stack.Pop();
        }
    }
}