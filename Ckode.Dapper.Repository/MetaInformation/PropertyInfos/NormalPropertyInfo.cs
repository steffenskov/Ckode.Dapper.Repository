using System;
using System.Reflection;

namespace Ckode.Dapper.Repository.MetaInformation.PropertyInfos
{
	internal record NormalPropertyInfo : IPropertyInfo
	{
		public string Name { get; init; }
		public Type Type { get; init; }

		public NormalPropertyInfo(PropertyInfo property)
		{
			Name = property.Name;
			Type = property.PropertyType;
		}
	}
}
