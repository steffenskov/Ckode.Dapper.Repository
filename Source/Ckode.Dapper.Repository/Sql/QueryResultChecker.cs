using System;
using System.Collections.Generic;
using System.Linq;
using Ckode.Dapper.Repository.MetaInformation;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

namespace Ckode.Dapper.Repository.Sql
{
	internal class QueryResultChecker<TPrimaryKeyRecord, TRecord>
	where TPrimaryKeyRecord : TableRecord
	where TRecord : TPrimaryKeyRecord
	{
		public TRecord ReturnOrThrowIfRecordIsNull(TPrimaryKeyRecord record, RecordInformation info, TRecord? result, string query)
		{
			if (result == null)
			{
				var formattedPrimaryKeys = PrintPrimaryKeys(info.PrimaryKeys, record);
				var errorMessage = info.PrimaryKeys.Any()
										? $"No matching record found with the given primary keys."
										: $"No matching record found with the given values.";

				throw new NoRecordFoundException(query, formattedPrimaryKeys, errorMessage);
			}
			return result;
		}

		private static string PrintPrimaryKeys(IReadOnlyCollection<PrimaryKeyPropertyInfo> primaryKeys, TPrimaryKeyRecord record)
		{
			return string.Join(Environment.NewLine, primaryKeys.Select(pk => $"{pk.Name} = {FormatPrimaryKeyValue(pk, record)}"));
		}

		private static string FormatPrimaryKeyValue(PrimaryKeyPropertyInfo primaryKey, TPrimaryKeyRecord record)
		{
			var valueAsString = primaryKey.GetValue(record)?.ToString() ?? "null";
			if (primaryKey.Type == typeof(string))
				return $@"""{valueAsString}""";
			else
				return valueAsString;
		}
	}
}