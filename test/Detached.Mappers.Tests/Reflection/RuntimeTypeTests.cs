using Detached.Mappers.Reflection;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;
using static System.Linq.Expressions.Expression;
using static Detached.Mappers.Expressions.ExtendedExpression;

namespace Detached.Mappers.Tests.Reflection
{
    public class RuntimeTypeTests
    {
        [Fact]
        public void define_autoproperty()
        {
            RuntimeTypeBuilder typeBuilder = new RuntimeTypeBuilder("DefineAutoProperty");
            typeBuilder.DefineAutoProperty("TestProp", typeof(int));

            Type newType = typeBuilder.Create();
            object newInstance = Activator.CreateInstance(newType);

            PropertyInfo testPropInfo = newType.GetProperty("TestProp");
            testPropInfo.SetValue(newInstance, 5);
            int result = (int)testPropInfo.GetValue(newInstance);

            Assert.Equal(5, result);
        }

        [Fact]
        public void define_method()
        {
            RuntimeTypeBuilder typeBuilder = new RuntimeTypeBuilder("DefineMethod");
            ParameterExpression a = Parameter(typeof(int), "a");
            ParameterExpression b = Parameter(typeof(int), "b");

            typeBuilder.DefineMethod("TestMethod",
                new[] { a, b },
                Add(a, b)
            );

            Type newType = typeBuilder.Create();
            object newInstance = Activator.CreateInstance(newType);

            MethodInfo testMethodInfo = newType.GetMethod("TestMethod");
            object result = testMethodInfo.Invoke(newInstance, new object[] { 5, 4 });

            Assert.Equal(9, result);
        }

        [Fact]
        public void access_local_field()
        {
            RuntimeTypeBuilder typeBuilder = new RuntimeTypeBuilder("AccessLocalField");
            ParameterExpression a = Parameter(typeof(int), "a");
            ParameterExpression b = Parameter(typeof(int), "b");

            var field = typeBuilder.DefineField("TestField", typeof(string));

            typeBuilder.DefineMethod("TestMethod",
                null,
                Field(typeBuilder.This, field)
            );

            Type newType = typeBuilder.Create();
            object newInstance = Activator.CreateInstance(newType);

            FieldInfo fieldInfo = newType.GetField("TestField");
            fieldInfo.SetValue(newInstance, "testValue");
            MethodInfo testMethodInfo = newType.GetMethod("TestMethod");
            object result = testMethodInfo.Invoke(newInstance, null);

            Assert.Equal("testValue", result);
        }

        [Fact]
        public void call_base_method()
        {
            RuntimeTypeBuilder typeBuilder = new RuntimeTypeBuilder("CallBaseMethod", typeof(BaseMethodClass));
            MethodInfo getTextInfo = typeof(BaseMethodClass).GetMethod("GetText");

            typeBuilder.OverrideMethod(getTextInfo,
                null,
                Call("Concat", typeof(string), Call(typeBuilder.Base, getTextInfo), Constant(" (overriden)"))
            );

            Type newType = typeBuilder.Create();
            object newInstance = Activator.CreateInstance(newType);

            MethodInfo testMethodInfo = newType.GetMethod("GetText");
            object result = testMethodInfo.Invoke(newInstance, null);

            Assert.Equal("this is the base class! (overriden)", result);
        }

        [Fact]
        public void override_property()
        {
            RuntimeTypeBuilder typeBuilder = new RuntimeTypeBuilder("OverrideProperty", typeof(BasePropertyClass));

            PropertyInfo propInfo = typeof(BasePropertyClass).GetProperty("Text");

            typeBuilder.OverrideMethod(propInfo.GetGetMethod(),
                null,
                Call("Concat", typeof(string), Call(typeBuilder.Base, propInfo.GetGetMethod()), Constant(" (overriden)"))
            );
 
            Type newType = typeBuilder.Create();
            object newInstance = Activator.CreateInstance(newType);

            propInfo.SetValue(newInstance, "this is a property!");
            string result = (string)propInfo.GetValue(newInstance);

            Assert.Equal("this is a property! (overriden)", result);
        }

        [Fact]
        public void auto_implement_interface()
        {
            RuntimeTypeBuilder typeBuilder = new RuntimeTypeBuilder("AutoImplenetInterface", typeof(BasePropertyClass));
            typeBuilder.AutoImplementInterface(typeof(ITextProperty));

            Type newType = typeBuilder.Create();
            ITextProperty newInstance = (ITextProperty)Activator.CreateInstance(newType);

            newInstance.Text = "test text";

            Assert.Equal("test text", newInstance.Text);
        }

        public class BasePropertyClass
        {
            public virtual string Text { get; set; }
        }

        public interface ITextProperty
        {
            string Text { get; set; }
        }

        public class BaseMethodClass
        {
            public virtual string GetText() => "this is the base class!";
        }
    }
}