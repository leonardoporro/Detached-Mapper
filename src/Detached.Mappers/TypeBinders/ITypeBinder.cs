using Detached.Mappers.TypePairs;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeBinders
{
    public interface ITypeBinder
    {
        bool CanBind(Mapper mapper, TypePair typePair);

        Expression Bind(Mapper mapper, TypePair typePair, Expression sourceExpr);
    }
}