using System;
using System.Reflection;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.MetaInformation.PropertyInfos
{
	internal class ForeignKeyPropertyInfo : IPropertyInfo
	{
		public string ReferencedColumnName { get; init; }
		public PropertyInfo Property { get; init; }
		public string Name => Property.Name;
		public Type Type => Property.PropertyType;

		public ForeignKeyPropertyInfo(PropertyInfo property, ForeignKeyAttribute foreignKey)
		{
			Property = property;
			ReferencedColumnName = foreignKey.ReferencedColumnName ?? property.Name;
		}
	}
}
