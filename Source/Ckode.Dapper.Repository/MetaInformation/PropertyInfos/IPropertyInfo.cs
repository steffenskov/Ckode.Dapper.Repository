using System;
using System.Reflection;

namespace Ckode.Dapper.Repository.MetaInformation.PropertyInfos
{
	internal interface IPropertyInfo
	{
		PropertyInfo Property { get; }
		string Name { get; }
		Type Type { get; }
	}
}
