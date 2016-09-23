using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Runtime.CompilerServices;

namespace EntityFrameworkCore.Detached.Metadata
{
    public class AuditProperties
    {
        Property createdBy;
        public Property CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                if (!IsValidTypeForUser(value.ClrType))
                    RaiseException(value);

                createdBy = value;
            }
        }

        Property createdDate;
        public Property CreatedDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                if (!IsValidTypeForDate(value.ClrType))
                    RaiseException(value);

                createdDate = value;
            }
        }

        Property modifiedBy;
        public Property ModifiedBy
        {
            get
            {
                return modifiedBy;
            }
            set
            {
                if (!IsValidTypeForUser(value.ClrType))
                    RaiseException(value);

                modifiedBy = value;
            }
        }

        Property modifiedDate;
        public Property ModifiedDate
        {
            get
            {
                return modifiedDate;
            }
            set
            {
                if (!IsValidTypeForDate(value.ClrType))
                    RaiseException(value);

                modifiedDate = value;
            }
        }

        void RaiseException(Property property, [CallerMemberName]string clrPropertyName = null)
        {
            throw new Exception($"Property {property.Name} has a type {property.ClrType} that is not a valid type for {clrPropertyName}.");
        }

        public bool IsValidTypeForDate(Type propType)
        {
            return propType == typeof(string) ||
                propType == typeof(DateTime) ||
                propType == typeof(long);
        }

        public bool IsValidTypeForUser(Type propType)
        {
            return propType == typeof(string);
        }

        public object GetValueForDate(Type propType, DateTime dateTime)
        {
            if (propType == typeof(string))
                return dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            else if (propType == typeof(long))
                return dateTime.Ticks;
            else if (propType == typeof(DateTime))
                return dateTime;
            else
                throw new Exception("Invalid property type for DateTime conversion.");
        }
    }
}
