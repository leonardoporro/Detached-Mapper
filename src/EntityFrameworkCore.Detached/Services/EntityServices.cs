using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Detached.Services
{
    public class KeyServices<TEntity> : IEntityServices<TEntity>, IEntityServices
    {
        #region Fields

        IEntityType _entityType;
        IKey _key;
        int _keySize;
        List<IKeyGetter> _keyGetters;

        #endregion

        #region Ctor.

        public KeyServices(IEntityType entityType)
        {
            _entityType = entityType;
            _key = entityType.FindPrimaryKey();
            InitializeKeyGetters();
        }

        void InitializeKeyGetters()
        {
            _keyGetters = new List<IKeyGetter>();
            HashSet<Property> visited = new HashSet<Property>();
            foreach (Property property in _key.Properties)
            {
                if (property.IsForeignKey())
                {
                    if (!visited.Contains(property))
                    {
                        IEnumerable<ForeignKey> foreignKeys = property.GetContainingForeignKeys();
                        if (foreignKeys.Count() != 1)
                            throw new Exception("not supported");

                        ForeignKey foreignKey = foreignKeys.First();
                        foreach (Property prop in foreignKey.Properties)
                            visited.Add(prop);

                        INavigation navigation = foreignKey.GetNavigations()
                                                     .Where(n => n.DeclaringEntityType.ClrType == _entityType.ClrType)
                                                     .Single();

                        IEntityType fkType = navigation.GetTargetType();
                        IKey fkKey = fkType.FindPrimaryKey();

                        List<IClrPropertyGetter> fkKeyGetters = new List<IClrPropertyGetter>();
                        foreach (Property fkKeyProperty in fkKey.Properties)
                        {
                            fkKeyGetters.Add(fkKeyProperty.GetGetter());
                            _keySize++;
                        }
                        _keyGetters.Add(new ForeignKeyGetter(navigation.GetGetter(), fkKeyGetters));
                    }
                }
                else
                {
                    visited.Add(property);
                    _keyGetters.Add(new SimpleKeyGetter(property.GetGetter()));
                    _keySize++;
                }
            }
        }

        #endregion

        public IKey GetKey()
        {
            return _key;
        }

        public object[] GetKeyValues(object entity)
        {
            object[] values = new object[_keySize];
            int offset = 0;
            foreach (IKeyGetter getter in _keyGetters)
                getter.Get(entity, values, ref offset);
            return values;
        }

        public int GetHashCode(object entity)
        {
            return StructuralComparisons.StructuralEqualityComparer.GetHashCode(GetKeyValues(entity));
        }

        public bool Equal(object entityA, object entityB)
        {
            if (entityA != null && entityB != null)
            {
                object[] keyA = GetKeyValues(entityA);
                object[] keyB = GetKeyValues(entityB);
                for (int i = 0; i < _keySize; i++)
                {
                    if (!Equals(keyA[i], keyB[i]))
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Dictionary<object[], object> CreateTable(IEnumerable entities)
        {
            Dictionary<object[], object> table = new Dictionary<object[], object>(KeyEqualityComparer.Instance);
            foreach (TEntity entity in entities)
            {
                table.Add(GetKeyValues(entity), entity);
            }
            return table;
        }

        public Expression<Func<TEntity, bool>> CreateEqualityExpression(object[] keyValues)
        {
            if (keyValues == null || keyValues.Any(kv => kv == null))
                throw new ArgumentException("Key values cannot be null.", nameof(keyValues));

            if (_key.Properties.Count != keyValues.Length)
                throw new ArgumentException($"Key values count mismatch, expected {string.Join(",", _key.Properties.Select(p => p.Name))}");

            ParameterExpression param = Expression.Parameter(_key.DeclaringEntityType.ClrType);
            Func<int, Expression> buildCompare = i =>
            {
                object keyValue = keyValues[i];
                IProperty keyProperty = _key.Properties[i];
                if (keyValue.GetType() != keyProperty.ClrType)
                {
                    keyValue = Convert.ChangeType(keyValue, keyProperty.ClrType);
                }

                return Expression.Equal(Expression.Property(param, keyProperty.PropertyInfo),
                                        Expression.Constant(keyValue));
            };

            Expression findExpr = buildCompare(0);
            for (int i = 1; i < _key.Properties.Count; i++)
            {
                findExpr = Expression.AndAlso(findExpr, buildCompare(i));
            }

            return Expression.Lambda<Func<TEntity, bool>>(findExpr, param);
        }

        class ForeignKeyGetter : IKeyGetter
        {
            IClrPropertyGetter _entityGetter;
            IEnumerable<IClrPropertyGetter> _keyGetters;

            public ForeignKeyGetter(IClrPropertyGetter entityGetter, IEnumerable<IClrPropertyGetter> keyGetters)
            {
                _entityGetter = entityGetter;
                _keyGetters = keyGetters;
            }

            public void Get(object instance, object[] result, ref int offset)
            {
                object entity = _entityGetter.GetClrValue(instance);
                if (entity == null)
                    throw new Exception("");

                foreach (IClrPropertyGetter getter in _keyGetters)
                {
                    result[offset] = getter.GetClrValue(entity);
                    offset++;
                }
            }
        }

        class SimpleKeyGetter : IKeyGetter
        {
            IClrPropertyGetter _keyGetter;

            public SimpleKeyGetter(IClrPropertyGetter keyGetter)
            {
                _keyGetter = keyGetter;
            }

            public void Get(object instance, object[] result, ref int offset)
            {
                result[offset] = _keyGetter.GetClrValue(instance);
                offset++;
            }
        }

        class KeyEqualityComparer : IEqualityComparer<object[]>
        {
            public static KeyEqualityComparer Instance { get; } = new KeyEqualityComparer();

            public bool Equals(object[] x, object[] y)
            {
                if (x.Length != y.Length)
                    return false;

                for (int i = 0; i < x.Length; i++)
                {
                    if (!object.Equals(x[i], y[i]))
                        return false;
                }
                return true;
            }

            public int GetHashCode(object[] obj)
            {
                return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
            }
        }

        interface IKeyGetter
        {
            void Get(object instance, object[] result, ref int offset);
        }
    }
}
