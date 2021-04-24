using System;
using System.Collections.Generic;
using System.Linq;
using Ckode.Dapper.Repository.MetaInformation;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

namespace Ckode.Dapper.Repository.Sql
{
	internal class QueryResultChecker<TPrimaryKeyEntity, TEntity>
	where TPrimaryKeyEntity : TableEntity
	where TEntity : TPrimaryKeyEntity
	{
		public TEntity ReturnOrThrowIfEntityIsNull(TPrimaryKeyEntity record, EntityInformation info, TEntity? result, string query)
		{
			if (result == null)
			{
				var formattedPrimaryKeys = PrintPrimaryKeys(info.PrimaryKeys, record);
				var errorMessage = info.PrimaryKeys.Any()
										? $"No matching record found with the given primary keys."
										: $"No matching record found with the given values.";

				throw new NoEntityFoundException(query, formattedPrimaryKeys, errorMessage);
			}
			return result;
		}

		private static string PrintPrimaryKeys(IReadOnlyCollection<PrimaryKeyPropertyInfo> primaryKeys, TPrimaryKeyEntity record)
		{
			return string.Join(Environment.NewLine, primaryKeys.Select(pk => $"{pk.Name} = {FormatPrimaryKeyValue(pk, record)}"));
		}

		private static string FormatPrimaryKeyValue(PrimaryKeyPropertyInfo primaryKey, TPrimaryKeyEntity record)
		{
			var valueAsString = primaryKey.GetValue(record)?.ToString() ?? "null";
			if (primaryKey.Type == typeof(string))
				return $@"""{valueAsString}""";
			else
				return valueAsString;
		}
	}
}