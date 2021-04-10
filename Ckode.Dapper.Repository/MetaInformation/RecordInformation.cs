using System.Collections.Generic;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

namespace Ckode.Dapper.Repository.MetaInformation
{
	internal record RecordInformation(IReadOnlyCollection<PrimaryKeyPropertyInfo> PrimaryKeys, IReadOnlyCollection<ForeignKeyPropertyInfo> ForeignKeys, IReadOnlyCollection<NormalPropertyInfo> OtherColumns)
	{
	}
}
