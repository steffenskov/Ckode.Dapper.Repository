using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Ckode.Dapper.Repository.Interfaces;
using Ckode.Dapper.Repository.MetaInformation;

namespace Ckode.Dapper.Repository.Sql
{
	public abstract class HeapRepository<TRecord> : IHeapRepository<TRecord>
	where TRecord : TableRecord
	{

		private readonly QueryGenerator _queryGenerator;
		private readonly QueryResultChecker<TRecord, TRecord> _resultChecker;

		public HeapRepository()
		{
			_queryGenerator = new QueryGenerator(TableName, Schema);
			_resultChecker = new QueryResultChecker<TRecord, TRecord>();
		}

		protected abstract string TableName { get; }

		protected string Schema => "dbo";

		protected string FormattedTableName => $"[{Schema}].[{TableName}]";

		protected abstract IDbConnection CreateConnection();

		#region Injection points for Dapper methods

		protected abstract QuerySingleDelegate<TRecord> QuerySingle { get; }

		protected abstract QueryDelegate<TRecord> Query { get; }

		#endregion

		public TRecord Delete(TRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			var query = _queryGenerator.GenerateDeleteQuery<TRecord>();
			using var connection = CreateConnection();
			var result = Query.Invoke(connection, query, record);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result.FirstOrDefault(), query);
		}

		public TRecord Get(TRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			var query = _queryGenerator.GenerateGetQuery<TRecord>();
			using var connection = CreateConnection();
			var result = Query.Invoke(connection, query, record);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result.FirstOrDefault(), query);
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

			var query = _queryGenerator.GenerateInsertQuery(record);
			using var connection = CreateConnection();
			return QuerySingle.Invoke(connection, query, record);
		}
	}
}