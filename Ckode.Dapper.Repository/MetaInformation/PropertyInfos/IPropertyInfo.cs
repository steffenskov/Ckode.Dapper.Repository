using System;

namespace Ckode.Dapper.Repository.MetaInformation.PropertyInfos
{
	internal interface IPropertyInfo
	{
		string Name { get; }
		Type Type { get; }
	}
}
