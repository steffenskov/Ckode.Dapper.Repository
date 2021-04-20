using System;
using System.Reflection;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.MetaInformation.PropertyInfos
{
    internal class PrimaryKeyPropertyInfo : IPropertyInfo
    {
        private readonly MemberAccessor _accessor;
        private readonly object? _defaultValue;
        public bool IsIdentity { get; init; }
        public PropertyInfo Property { get; init; }
        public string Name => Property.Name;
        public Type Type => Property.PropertyType;

        public PrimaryKeyPropertyInfo(PropertyInfo property, PrimaryKeyAttribute primaryKey) // PrimaryKeyAttribute currently doesn't contain any information, however it's kept for future usage, e.g. Identity information
        {
            Property = property;
            IsIdentity = primaryKey.IsIdentity;
            var type = property.PropertyType;
            _accessor = new MemberAccessor(property);
            if (type.IsValueType)
            {
                _defaultValue = Activator.CreateInstance(type);
            }
            else
            {
                _defaultValue = null;
            }
        }

        public bool HasDefaultValue<T>(T record) where T : TableRecord
        {
            var value = _accessor.getter(record);

            return value == _defaultValue || value?.Equals(_defaultValue) == true;
        }

        public object GetValue<T>(T record) where T : TableRecord
        {
            return _accessor.getter(record);
        }
    }
}
