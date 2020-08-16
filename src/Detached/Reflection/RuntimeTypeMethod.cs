using System;
using System.Reflection.Emit;

namespace Detached.Reflection
{
    public class RuntimeTypeMethod
    {
        public RuntimeTypeMethod(string name, MethodBuilder methodBuilder, Type[] paramTypes, Type returnType)
        {
            Name = name;
            Method = methodBuilder;
            ParameterTypes = paramTypes;
            ReturnType = returnType;
        }

        public string Name { get; }

        public MethodBuilder Method { get; }

        public Type[] ParameterTypes { get; }

        public Type ReturnType { get; }
    }
}