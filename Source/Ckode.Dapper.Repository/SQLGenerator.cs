using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Ckode.Dapper.Repository.MetaInformation;

[assembly: InternalsVisibleTo("Ckode.Dapper.Repository.UnitTests")]
namespace Ckode.Dapper.Repository
{
	internal class SQLGenerator
	{
		private readonly string _tableName;

		public SQLGenerator(string tableName)
		{
			if (tableName == null)
			{
				throw new ArgumentNullException(nameof(tableName));
			}

			if (string.IsNullOrWhiteSpace(tableName))
			{
				throw new ArgumentException($"Invalid tableName: {tableName}", nameof(tableName));
			}
			_tableName = tableName;
		}

		public string GenerateDeleteQuery<TRecord>(TRecord record)
			where TRecord : BaseTableRecord
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation(record);
			string whereClause;
			if (info.PrimaryKeys.Count == 0)
			{
				whereClause = GenerateWhereClauseWithoutPrimaryKey(info);
			}
			else
			{
				whereClause = GenerateWhereClauseWithPrimaryKeys(info);
			}

			return $"DELETE FROM {_tableName} OUTPUT deleted.* WHERE {whereClause}";
		}

		public string GenerateInsertQuery<TRecord>(TRecord record)
			where TRecord : BaseTableRecord
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation(record);
			var identityColumns = info.PrimaryKeys.Where(pk => pk.IsIdentity).Select(pk => pk.Property).ToList();

			var columnsToInsert = info.Columns
										.Where(column => !identityColumns.Contains(column.Property))
										.ToList();

			return $"INSERT INTO {_tableName} ({string.Join(", ", columnsToInsert.Select(column => column.ColumnName))}) OUTPUT inserted.* VALUES ({string.Join(", ", columnsToInsert.Select(column => $"@{column.Name}"))})";
		}

		public string GenerateGetAllQuery()
		{
			return $"SELECT * FROM {_tableName}";
		}

		public string GenerateGetQuery<TRecord>(TRecord record)
			where TRecord : BaseTableRecord
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation(record);
			if (!info.PrimaryKeys.Any())
			{
				throw new InvalidOperationException($"GenerateGetQuery for record of type {typeof(TRecord).FullName} failed as the type has no properties marked with [PrimaryKey].");
			}

			return $"SELECT * FROM {_tableName} WHERE {GenerateWhereClauseWithPrimaryKeys(info)}";
		}

		public string GenerateUpdateQuery<TRecord>(TRecord record)
			where TRecord : BaseTableRecord
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation(record);
			if (!info.PrimaryKeys.Any())
			{
				throw new InvalidOperationException($"GenerateGetQuery for record of type {typeof(TRecord).FullName} failed as the type has no properties marked with [PrimaryKey].");
			}

			var setClause = GenerateSetClause(info);

			return $"UPDATE {_tableName} SET {setClause} OUTPUT inserted.* WHERE {GenerateWhereClauseWithPrimaryKeys(info)}";
		}


		private static string GenerateSetClause(RecordInformation info)
		{
			var primaryKeys = info.PrimaryKeys.Select(pk => pk.Property).ToList();
			var columnsToSet = info.Columns.Where(column => !primaryKeys.Contains(column.Property));
			return string.Join(", ", columnsToSet.Select(column => $"{column.ColumnName} = @{column.Name}"));
		}

		private static string GenerateWhereClauseWithoutPrimaryKey(RecordInformation info)
		{
			return string.Join(" AND ", info.Columns.Select(column => $"{column.ColumnName} = @{column.Name}"));
		}

		private static string GenerateWhereClauseWithPrimaryKeys(RecordInformation info)
		{
			var primaryKeyProperties = info.PrimaryKeys.Select(pk => pk.Property).ToList();
			var primaryKeys = info.Columns
								.Where(column => primaryKeyProperties.Contains(column.Property));

			return string.Join(" AND ", primaryKeys.Select(column => $"{column.ColumnName} = @{column.Name}"));
		}
	}
}
