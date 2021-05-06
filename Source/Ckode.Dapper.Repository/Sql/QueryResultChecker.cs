using System;
using System.Collections.Generic;
using System.Linq;
using Ckode.Dapper.Repository.MetaInformation;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

namespace Ckode.Dapper.Repository.Sql
{
	internal class QueryResultChecker<TPrimaryKeyEntity, TEntity>
	where TPrimaryKeyEntity : DbEntity
	where TEntity : TPrimaryKeyEntity
	{
		public TEntity ReturnOrThrowIfEntityIsNull(TPrimaryKeyEntity entity, EntityInformation info, TEntity? result, string query)
		{
			if (result == null)
			{
				var formattedPrimaryKeys = PrintPrimaryKeys(info.PrimaryKeys, entity);
				var errorMessage = info.PrimaryKeys.Any()
										? $"No matching entity found with the given primary keys."
										: $"No matching entity found with the given values.";

				throw new NoEntityFoundException(query, formattedPrimaryKeys, errorMessage);
			}
			return result;
		}

		private static string PrintPrimaryKeys(IReadOnlyCollection<PrimaryKeyPropertyInfo> primaryKeys, TPrimaryKeyEntity entity)
		{
			return string.Join(Environment.NewLine, primaryKeys.Select(pk => $"{pk.Name} = {FormatPrimaryKeyValue(pk, entity)}"));
		}

		private static string FormatPrimaryKeyValue(PrimaryKeyPropertyInfo primaryKey, TPrimaryKeyEntity entity)
		{
			var valueAsString = primaryKey.GetValue(entity)?.ToString() ?? "null";
			if (primaryKey.Type == typeof(string))
				return $@"""{valueAsString}""";
			else
				return valueAsString;
		}
	}
}