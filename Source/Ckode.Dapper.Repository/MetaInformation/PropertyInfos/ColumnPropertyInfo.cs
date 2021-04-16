using System;
using System.Reflection;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.MetaInformation.PropertyInfos
{
	internal class ColumnPropertyInfo : IPropertyInfo
	{
		public string ColumnName { get; init; }
		public bool IsCustomColumnName { get; init; }
		public PropertyInfo Property { get; init; }
		public string Name => Property.Name;
		public Type Type => Property.PropertyType;

		public ColumnPropertyInfo(PropertyInfo property, ColumnAttribute column)
		{
			Property = property;
			ColumnName = column.ColumnName ?? property.Name;
			IsCustomColumnName = column.ColumnName != null;
		}
	}
}
