using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Ckode.Dapper.Repository.MetaInformation;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

[assembly: InternalsVisibleTo("Ckode.Dapper.Repository.UnitTests")]
namespace Ckode.Dapper.Repository
{
	internal class SQLGenerator
	{
		private readonly string _schemaAndTable;

		public SQLGenerator(string tableName, string schema = "dbo")
		{
			if (tableName == null)
			{
				throw new ArgumentNullException(nameof(tableName));
			}

			if (schema == null)
			{
				throw new ArgumentNullException(nameof(schema));
			}

			if (string.IsNullOrWhiteSpace(tableName))
			{
				throw new ArgumentException($"Invalid tableName: {tableName}", nameof(tableName));
			}
			_schemaAndTable = $"{EnsureSquareBrackets(schema)}.{EnsureSquareBrackets(tableName)}";
		}

		public string GenerateDeleteQuery<TRecord>()
			where TRecord : TableRecord
		{
			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			var whereClause = info.PrimaryKeys.Count == 0
								? GenerateWhereClauseWithoutPrimaryKey(info)
								: GenerateWhereClauseWithPrimaryKeys(info);

			var outputColumns = GenerateColumnsList("deleted", info.Columns);
			return $"DELETE FROM {_schemaAndTable} OUTPUT {outputColumns} WHERE {whereClause}";
		}

		public string GenerateInsertQuery<TRecord>()
			where TRecord : TableRecord
		{
			var info = RecordInformationCache.GetRecordInformation<TRecord>();
			var identityColumns = info.PrimaryKeys.Where(pk => pk.IsIdentity).Select(pk => pk.Property).ToList();

			var columnsToInsert = info.Columns
										.Where(column => !identityColumns.Contains(column.Property))
										.ToList();

			var outputColumns = GenerateColumnsList("inserted", info.Columns);
			return $"INSERT INTO {_schemaAndTable} ({string.Join(", ", columnsToInsert.Select(column => AddSquareBrackets(column.ColumnName)))}) OUTPUT {outputColumns} VALUES ({string.Join(", ", columnsToInsert.Select(column => $"@{column.Name}"))})";
		}

		public string GenerateGetAllQuery<TRecord>()
			where TRecord : TableRecord
		{
			var info = RecordInformationCache.GetRecordInformation<TRecord>();
			var columnsList = GenerateColumnsList(_schemaAndTable, info.Columns);
			return $"SELECT {columnsList} FROM {_schemaAndTable}";
		}

		public string GenerateGetQuery<TRecord>()
			where TRecord : TableRecord
		{
			var info = RecordInformationCache.GetRecordInformation<TRecord>();
			if (!info.PrimaryKeys.Any())
			{
				throw new InvalidOperationException($"GenerateGetQuery for record of type {typeof(TRecord).FullName} failed as the type has no properties marked with [PrimaryKey].");
			}

			var columnsList = GenerateColumnsList(_schemaAndTable, info.Columns);

			return $"SELECT {columnsList} FROM {_schemaAndTable} WHERE {GenerateWhereClauseWithPrimaryKeys(info)}";
		}

		public string GenerateUpdateQuery<TRecord>()
			where TRecord : TableRecord
		{
			var info = RecordInformationCache.GetRecordInformation<TRecord>();
			if (!info.PrimaryKeys.Any())
			{
				throw new InvalidOperationException($"GenerateGetQuery for record of type {typeof(TRecord).FullName} failed as the type has no properties marked with [PrimaryKey].");
			}

			var setClause = GenerateSetClause(info);

			var outputColumns = GenerateColumnsList("inserted", info.Columns);

			return $"UPDATE {_schemaAndTable} SET {setClause} OUTPUT {outputColumns} WHERE {GenerateWhereClauseWithPrimaryKeys(info)}";
		}


		private string GenerateSetClause(RecordInformation info)
		{
			var primaryKeys = info.PrimaryKeys.Select(pk => pk.Property).ToList();
			var columnsToSet = info.Columns.Where(column => !primaryKeys.Contains(column.Property));
			return string.Join(", ", columnsToSet.Select(column => $"{_schemaAndTable}.{AddSquareBrackets(column.ColumnName)} = @{column.Name}"));
		}

		private string GenerateWhereClauseWithoutPrimaryKey(RecordInformation info)
		{
			return string.Join(" AND ", info.Columns.Select(column => $"{_schemaAndTable}.{AddSquareBrackets(column.ColumnName)} = @{column.Name}"));
		}

		private string GenerateWhereClauseWithPrimaryKeys(RecordInformation info)
		{
			var primaryKeyProperties = info.PrimaryKeys.Select(pk => pk.Property).ToList();
			var primaryKeys = info.Columns
								.Where(column => primaryKeyProperties.Contains(column.Property));

			return string.Join(" AND ", primaryKeys.Select(column => $"{_schemaAndTable}.{AddSquareBrackets(column.ColumnName)} = @{column.Name}"));
		}


		private string GenerateColumnsList(string tableName, IEnumerable<ColumnPropertyInfo> columns)
		{
			tableName = EnsureSquareBrackets(tableName);

			return string.Join(", ", columns.Select(column => GenerateColumnClause(tableName, column)));
		}

		private static string GenerateColumnClause(string tableName, ColumnPropertyInfo column)
		{
			if (column.IsCustomColumnName)
			{
				return $"{tableName}.{AddSquareBrackets(column.ColumnName)} AS {AddSquareBrackets(column.Name)}";
			}
			else
			{
				return $"{tableName}.{AddSquareBrackets(column.ColumnName)}";
			}
		}

		private static string EnsureSquareBrackets(string name)
		{
			if (!name.StartsWith('['))
				return AddSquareBrackets(name);
			else
				return name;
		}

		private static string AddSquareBrackets(string name)
		{
			return $"[{name}]";
		}
	}
}
