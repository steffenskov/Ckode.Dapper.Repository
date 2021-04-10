using System.Reflection;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.MetaInformation.PropertyInfos
{
	internal record ForeignKeyPropertyInfo : NormalPropertyInfo, IPropertyInfo
	{
		public string? ReferencedColumnName { get; init; }

		public ForeignKeyPropertyInfo(PropertyInfo property) : base(property)
		{
			ReferencedColumnName = property.GetCustomAttribute<ForeignKeyAttribute>()?.ReferencedColumnName;
		}
	}
}
