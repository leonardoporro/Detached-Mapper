using System.Linq.Expressions;

namespace Detached.Mappers.Projections
{
    public interface ITypeBinder
    {
        bool CanBind<TSource, TTarget>();

        Expression Bind<TSource, TTarget>(TTarget target, Mapper mapper)
            where TSource : class
            where TTarget : class;
    }
}