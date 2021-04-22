using System;
using System.Reflection;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.MetaInformation.PropertyInfos
{
	internal class PrimaryKeyPropertyInfo : ColumnPropertyInfo
	{
		public bool IsIdentity { get; }

		public PrimaryKeyPropertyInfo(PropertyInfo property, PrimaryKeyColumnAttribute primaryKey) // PrimaryKeyAttribute currently doesn't contain any information, however it's kept for future usage, e.g. Identity information
			: base(property, primaryKey)
		{
			IsIdentity = primaryKey.IsIdentity;

		}
	}
}
