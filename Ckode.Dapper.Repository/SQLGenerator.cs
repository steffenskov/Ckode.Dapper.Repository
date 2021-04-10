using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Ckode.Dapper.Repository.MetaInformation;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

[assembly: InternalsVisibleTo("Ckode.Dapper.Repository.Tests")]
namespace Ckode.Dapper.Repository
{
	internal class SQLGenerator
	{
		public string GenerateDeleteQuery<TRecord>(TRecord entity)
			where TRecord : BaseTableRecord
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var info = RecordInformationCache.GetRecordInformation(entity);
			string whereClause;
			if (info.PrimaryKeys.Count == 0)
			{
				whereClause = GenerateDeleteWhereClauseWithoutPrimaryKey(entity, info);
			}
			else
			{
				whereClause = GenerateDeleteWhereClauseWithPrimaryKeys(entity, info);
			}
		
			return $"DELETE FROM {entity.TableName} OUTPUT deleted.* WHERE {whereClause}";
		}

		private string GenerateDeleteWhereClauseWithoutPrimaryKey<TRecord>(TRecord entity, RecordInformation info) where TRecord : BaseTableRecord
		{
			var allColumns = info.ForeignKeys.Cast<IPropertyInfo>()
									.Concat(info.OtherColumns); // No need to concat primary key, as this method is only called where there are none

			return string.Join(" AND ", allColumns.Select(column => $"{column.Name} = @{column.Name}"));
		}

		private string GenerateDeleteWhereClauseWithPrimaryKeys<TRecord>(TRecord entity, RecordInformation info) where TRecord : BaseTableRecord
		{
			return string.Join(" AND ", info.PrimaryKeys.Select(key => $"{key.Name} = @{key.Name}"));
		}

		public string GenerateInsertQuery<TRecord>(TRecord entity)
			where TRecord : BaseTableRecord
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var info = RecordInformationCache.GetRecordInformation(entity);
			var columnsToIgnore = info.PrimaryKeys.Where(pk => pk.HasDefaultValue(entity)).ToList(); // Assume these primary keys are identify columns

			var columnsToInsert = info.PrimaryKeys
										.Where(pk => !columnsToIgnore.Contains(pk))
										.Cast<IPropertyInfo>()
										.Concat(info.ForeignKeys)
										.Concat(info.OtherColumns)
										.ToList();

			return $"INSERT INTO {entity.TableName} ({string.Join(", ", columnsToInsert.Select(column => column.Name))}) OUTPUT inserted.* VALUES ({string.Join(", ", columnsToInsert.Select(column => "@" + column.Name))})";
		}
	}
}
