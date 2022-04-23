using Detached.Mappers.Exceptions;
using System;
using Xunit;
using static Detached.Mappers.Extensions.MapperExpressionExtensions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Tests.Extensions
{
    public class ExtensionTests
    {
        [Fact]
        public void throw_exception()
        {
            var fn = Lambda<Action>(Block(ThrowMapperException("Detached library, {0}% fat!", Constant(0)))).Compile();

            try
            {
                fn();
                throw new Exception("Exception was not even thrown!");
            }
            catch (Exception ex)
            {
                Assert.IsType<MapperException>(ex);
                Assert.Equal("Detached library, 0% fat!", ex.Message);
            }
        }
    }
}
