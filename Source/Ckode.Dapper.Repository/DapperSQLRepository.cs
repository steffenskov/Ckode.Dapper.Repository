using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Ckode.Dapper.Repository.MetaInformation;

namespace Ckode.Dapper.Repository
{
	public abstract class DapperSQLRepository<TRecord> : IRepository<TRecord>
		where TRecord : BaseTableRecord
	{
		#region Dapper delegates

		protected delegate T QuerySingleDelegate<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

		#endregion

		private readonly SQLGenerator generator;

		public DapperSQLRepository()
		{
			generator = new SQLGenerator(TableName);
		}

		protected abstract string TableName { get; }

		protected abstract IDbConnection CreateConnection();

		#region Injection points for Dapper methods

		protected abstract QuerySingleDelegate<TRecord> QuerySingle { get; }

		protected abstract QuerySingleDelegate<TRecord> QuerySingleOrDefault { get; }

		#endregion

		public TRecord Delete(TRecord record)
		{
			var query = generator.GenerateDeleteQuery(record);
			using var connection = CreateConnection();
			return QuerySingleOrDefault.Invoke(connection, query, record);
		}

		public TRecord Get(TRecord record)
		{
			var query = generator.GenerateGetQuery(record);
			// Invoke dapper query on connection
			return record;
		}

		public IEnumerable<TRecord> GetAll()
		{
			// TODO: How do I get table name here?
			// Depend on service locator maybe? not the prettiest solution....

			throw new NotImplementedException();
		}

		public TRecord Insert(TRecord record)
		{
			var info = RecordInformationCache.GetRecordInformation(record);

			var invalidIdentityColumns = info.PrimaryKeys
												.Where(pk => pk.IsIdentity && !pk.HasDefaultValue(record))
												.ToList();

			if (invalidIdentityColumns.Any())
			{
				throw new ArgumentException($"record has the following primary keys marked with IsIdentity, which have non-default values: {string.Join(", ", invalidIdentityColumns.Select(col => col.Name))}", nameof(record));
			}

			var query = generator.GenerateInsertQuery(record);
			using var connection = CreateConnection();
			return QuerySingle.Invoke(connection, query, record);
		}

		public TRecord Update(TRecord record)
		{
			var query = generator.GenerateUpdateQuery(record);
			// Invoke dapper query on connection
			return record;
		}
	}
}
