using System;

namespace Detached.Mappers.TypeMappers.Entity
{
    public struct NoKey : IEntityKey
    {
        public static NoKey Instance { get; } = new NoKey();

        public bool IsEmpty => true;

        public object[] ToObject()
        {
            return new object[0];
        }

        public override bool Equals(object obj) => false;

        public override int GetHashCode()
        {
            return new Random().Next(int.MaxValue);
        }

        public static bool operator ==(NoKey a, NoKey b) => false;

        public static bool operator !=(NoKey a, NoKey b) => true;
    }

    public struct EntityKey<T1> : IEntityKey
    {
        public EntityKey(T1 value1)
        {
            Value1 = value1;
        }

        public T1 Value1 { get; set; }

        public bool IsEmpty => Equals(default(T1), Value1);

        public override bool Equals(object obj)
            => obj is EntityKey<T1> ek && Equals(ek.Value1, Value1);

        public override int GetHashCode() => HashCode.Combine(Value1);

        public object[] ToObject() => new object[] { Value1 };

        public static bool operator ==(EntityKey<T1> a, EntityKey<T1> b)
        {
            return Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b);
        }

        public static bool operator !=(EntityKey<T1> a, EntityKey<T1> b)
        {
            return !(Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b));
        }
    }

    public struct EntityKey<T1, T2> : IEntityKey
    {
        public EntityKey(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public T1 Value1 { get; set; }

        public T2 Value2 { get; set; }

        public bool IsEmpty => Equals(default(T1), Value1) || Equals(default(T2), Value2);

        public override bool Equals(object obj)
            => obj is EntityKey<T1, T2> ek
                && Equals(ek.Value1, Value1)
                && Equals(ek.Value2, Value2);

        public override int GetHashCode() => HashCode.Combine(Value1, Value2);

        public object[] ToObject() => new object[] { Value1, Value2 };

        public static bool operator ==(EntityKey<T1, T2> a, EntityKey<T1, T2> b)
        {
            return Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b);
        }

        public static bool operator !=(EntityKey<T1, T2> a, EntityKey<T1, T2> b)
        {
            return !(Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b));
        }
    }

    public struct EntityKey<T1, T2, T3> : IEntityKey
    {
        public EntityKey(T1 value1, T2 value2, T3 value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public bool IsEmpty => Equals(default(T1), Value1) || Equals(default(T2), Value2) || Equals(default(T3), Value3);

        public T1 Value1 { get; set; }

        public T2 Value2 { get; set; }

        public T3 Value3 { get; set; }

        public override bool Equals(object obj)
            => obj is EntityKey<T1, T2, T3> ek
                && Equals(ek.Value1, Value1)
                && Equals(ek.Value2, Value2)
                && Equals(ek.Value3, Value3);

        public override int GetHashCode() => HashCode.Combine(Value1);

        public object[] ToObject() => new object[] { Value1, Value2, Value3 };

        public static bool operator ==(EntityKey<T1, T2, T3> a, EntityKey<T1, T2, T3> b)
        {
            return Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b);
        }

        public static bool operator !=(EntityKey<T1, T2, T3> a, EntityKey<T1, T2, T3> b)
        {
            return !(Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b));
        }
    }

    public struct EntityKey<T1, T2, T3, T4> : IEntityKey
    {
        public EntityKey(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
        }

        public T1 Value1 { get; set; }

        public T2 Value2 { get; set; }

        public T3 Value3 { get; set; }

        public T4 Value4 { get; set; }

        public bool IsEmpty => Equals(default(T1), Value1) || Equals(default(T2), Value2) || Equals(default(T3), Value3) || Equals(default(T4), Value4);

        public override bool Equals(object obj)
            => obj is EntityKey<T1, T2, T3, T4> ek
                && Equals(ek.Value1, Value1)
                && Equals(ek.Value2, Value2)
                && Equals(ek.Value3, Value3)
                && Equals(ek.Value4, Value4);

        public override int GetHashCode() => HashCode.Combine(Value1, Value2, Value3, Value4);

        public object[] ToObject() => new object[] { Value1, Value2, Value3, Value4 };

        public static bool operator ==(EntityKey<T1, T2, T3, T4> a, EntityKey<T1, T2, T3, T4> b)
        {
            return Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b);
        }

        public static bool operator !=(EntityKey<T1, T2, T3, T4> a, EntityKey<T1, T2, T3, T4> b)
        {
            return !(Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b));
        }
    }

    public struct EntityKey<T1, T2, T3, T4, T5> : IEntityKey
    {
        public EntityKey(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
            Value5 = value5;
        }

        public T1 Value1 { get; set; }

        public T2 Value2 { get; set; }

        public T3 Value3 { get; set; }

        public T4 Value4 { get; set; }

        public T5 Value5 { get; set; }

        public bool IsEmpty => Equals(default(T1), Value1) || Equals(default(T2), Value2) || Equals(default(T3), Value3) || Equals(default(T4), Value4) || Equals(default(T5), Value5);

        public override bool Equals(object obj)
            => obj is EntityKey<T1, T2, T3, T4, T5> ek
                && Equals(ek.Value1, Value1)
                && Equals(ek.Value2, Value2)
                && Equals(ek.Value3, Value3)
                && Equals(ek.Value4, Value4)
                && Equals(ek.Value5, Value5);

        public override int GetHashCode() => HashCode.Combine(Value1, Value2, Value3, Value4, Value5);

        public object[] ToObject() => new object[] { Value1, Value2, Value3, Value4, Value5 };

        public static bool operator ==(EntityKey<T1, T2, T3, T4, T5> a, EntityKey<T1, T2, T3, T4, T5> b)
        {
            return Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b);
        }

        public static bool operator !=(EntityKey<T1, T2, T3, T4, T5> a, EntityKey<T1, T2, T3, T4, T5> b)
        {
            return !(Equals(a, null) && Equals(b, null) || !Equals(a, null) && a.Equals(b));
        }
    }
}