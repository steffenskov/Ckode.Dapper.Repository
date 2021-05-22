using System;
using System.Collections.Generic;
using System.Linq;
using Ckode.Dapper.Repository.Interfaces;
using Ckode.Dapper.Repository.MetaInformation;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

namespace Ckode.Dapper.Repository.MySql
{
	public class MySqlQueryGenerator : IQueryGenerator
	{
		private readonly string _table;

		public MySqlQueryGenerator(string tableName)
		{
			if (tableName == null)
			{
				throw new ArgumentNullException(nameof(tableName));
			}


			if (string.IsNullOrWhiteSpace(tableName))
			{
				throw new ArgumentException($"Invalid tableName: {tableName}", nameof(tableName));
			}

			_table = tableName;
		}

		public string GenerateDeleteQuery<TEntity>() where TEntity : DbEntity
		{
			var info = EntityInformationCache.GetEntityInformation<TEntity>();

			var whereClause = info.PrimaryKeys.Count == 0
								? GenerateWhereClauseWithoutPrimaryKey(info)
								: GenerateWhereClauseWithPrimaryKeys(info);

			var outputColumns = GenerateColumnsList(_table, info.Columns);
			return $@"SELECT {outputColumns} FROM {_table} WHERE {whereClause};
DELETE FROM {_table} WHERE {whereClause};";
		}

		public string GenerateGetAllQuery<TEntity>() where TEntity : DbEntity
		{
			var info = EntityInformationCache.GetEntityInformation<TEntity>();
			var columnsList = GenerateColumnsList(_table, info.Columns);
			return $"SELECT {columnsList} FROM {_table};";
		}

		public string GenerateGetQuery<TEntity>() where TEntity : DbEntity
		{
			throw new NotImplementedException();
		}

		public string GenerateInsertQuery<TEntity>(TEntity entity) where TEntity : DbEntity
		{
			throw new NotImplementedException();
		}

		public string GenerateUpdateQuery<TEntity>() where TEntity : DbEntity
		{
			throw new NotImplementedException();
		}

		#region Helpers

		private string GenerateWhereClauseWithoutPrimaryKey(EntityInformation info)
		{
			return string.Join(" AND ", info.Columns.Select(column => $"{_table}.{column.ColumnName} = @{column.Name}"));
		}

		private string GenerateWhereClauseWithPrimaryKeys(EntityInformation info)
		{
			var primaryKeyProperties = info.PrimaryKeys.Select(pk => pk.Property).ToList();
			var primaryKeys = info.Columns
								.Where(column => primaryKeyProperties.Contains(column.Property));

			return string.Join(" AND ", primaryKeys.Select(column => $"{_table}.{column.ColumnName} = @{column.Name}"));
		}

		private string GenerateColumnsList(string tableName, IEnumerable<ColumnPropertyInfo> columns)
		{
			return string.Join(", ", columns.Select(column => GenerateColumnClause(tableName, column)));
		}

		private static string GenerateColumnClause(string tableName, ColumnPropertyInfo column)
		{
			if (column.IsCustomColumnName)
			{
				return $"{tableName}.{column.ColumnName} AS {column.Name}";
			}
			else
			{
				return $"{tableName}.{column.ColumnName}";
			}
		}
		#endregion
	}
}
