using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Ckode.Dapper.Repository.Interfaces;
using Ckode.Dapper.Repository.MetaInformation;

namespace Ckode.Dapper.Repository.Sql
{
	public abstract class PrimaryKeyRepository<TPrimaryKeyRecord, TRecord> : IRepository<TPrimaryKeyRecord, TRecord>
		where TPrimaryKeyRecord : TableRecord
		where TRecord : TPrimaryKeyRecord
	{
		private readonly QueryGenerator _queryGenerator;
		private readonly QueryResultChecker<TPrimaryKeyRecord, TRecord> _resultChecker;

		public PrimaryKeyRepository()
		{
			_queryGenerator = new QueryGenerator(TableName, Schema);
			_resultChecker = new QueryResultChecker<TPrimaryKeyRecord, TRecord>();
		}

		protected abstract string TableName { get; }

		protected string Schema => "dbo";

		protected string FormattedTableName => $"[{Schema}].[{TableName}]";

		protected abstract IDbConnection CreateConnection();

		#region Injection points for Dapper methods

		protected abstract QuerySingleDelegate<TRecord> QuerySingle { get; }

		protected abstract QuerySingleDelegate<TRecord> QuerySingleOrDefault { get; }

		protected abstract QueryDelegate<TRecord> Query { get; }

		#endregion

		public TRecord Delete(TPrimaryKeyRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			CheckForDefaultPrimaryKeys(info, record);

			var query = _queryGenerator.GenerateDeleteQuery<TRecord>();
			var result = ExecuteQuerySingleOrDefault(record, query);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result, query);
		}

		public TRecord Get(TPrimaryKeyRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			CheckForDefaultPrimaryKeys(info, record);

			var query = _queryGenerator.GenerateGetQuery<TRecord>();
			var result = ExecuteQuerySingleOrDefault(record, query);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result, query);
		}
		public IEnumerable<TRecord> GetAll()
		{
			var query = _queryGenerator.GenerateGetAllQuery<TRecord>();
			using var connection = CreateConnection();
			return Query.Invoke(connection, query);
		}

		public TRecord Insert(TRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			var invalidIdentityColumns = info.PrimaryKeys
												.Where(pk => pk.IsIdentity && !pk.HasDefaultValue(record))
												.ToList();

			if (invalidIdentityColumns.Any())
			{
				throw new ArgumentException($"record has the following primary keys marked with IsIdentity, which have non-default values: {string.Join(", ", invalidIdentityColumns.Select(col => col.Name))}", nameof(record));
			}

			var query = _queryGenerator.GenerateInsertQuery(record);
			using var connection = CreateConnection();
			return QuerySingle.Invoke(connection, query, record);
		}

		public TRecord Update(TRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			CheckForDefaultPrimaryKeys(info, record);

			var query = _queryGenerator.GenerateUpdateQuery<TRecord>();
			var result = ExecuteQuerySingleOrDefault(record, query);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result, query);
		}


		private static void CheckForDefaultPrimaryKeys(RecordInformation info, TPrimaryKeyRecord record)
		{
			var invalidPrimaryKeys = info.PrimaryKeys
											   .Where(pk => pk.HasDefaultValue(record))
											   .ToList();

			if (invalidPrimaryKeys.Any())
			{
				throw new ArgumentException($"record has the following primary keys which have default values: {string.Join(", ", invalidPrimaryKeys.Select(col => col.Name))}", nameof(record));
			}
		}

		private TRecord? ExecuteQuerySingleOrDefault(TPrimaryKeyRecord record, string query)
		{
			using var connection = CreateConnection();
			return QuerySingleOrDefault.Invoke(connection, query, record);
		}
	}
}
