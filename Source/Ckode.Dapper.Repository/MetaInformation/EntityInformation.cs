using System.Collections.Generic;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

namespace Ckode.Dapper.Repository.MetaInformation
{
	internal record EntityInformation(IReadOnlyCollection<PrimaryKeyPropertyInfo> PrimaryKeys, IReadOnlyCollection<ForeignKeyPropertyInfo> ForeignKeys, IReadOnlyCollection<ColumnPropertyInfo> Columns)
	{
	}
}
