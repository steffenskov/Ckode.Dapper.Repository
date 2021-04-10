using System;
using System.Reflection;

namespace Ckode.Dapper.Repository.MetaInformation.PropertyInfos
{
	internal record PrimaryKeyPropertyInfo : NormalPropertyInfo, IPropertyInfo
	{
		private readonly MemberAccessor _accessor;
		private readonly object _defaultValue;

		public PrimaryKeyPropertyInfo(PropertyInfo property) : base(property)
		{
			_accessor = new MemberAccessor(property);
			if (Type.IsValueType)
			{
				_defaultValue = Activator.CreateInstance(Type);
			}
			else
			{
				_defaultValue = null;
			}
		}

		public bool HasDefaultValue<T>(T entity) where T : BaseTableRecord
		{
			var value = _accessor.getter(entity);

			return value == _defaultValue || value?.Equals(_defaultValue) == true;
		}
	}
}
