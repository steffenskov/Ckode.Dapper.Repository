using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

		protected abstract IDapperInjection<TRecord> DapperInjection { get; }

		#endregion

		#region Delete
		public TRecord Delete(TRecord record)
		{
			return DeleteInternalAsync(record, (connection, query, input) => Task.FromResult(DapperInjection.Query.Invoke(connection, query, input)))
						.GetAwaiter()
						.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TRecord> DeleteAsync(TRecord record)
		{
			return await DeleteInternalAsync(record, (connection, query, input) => DapperInjection.QueryAsync.Invoke(connection, query, input));
		}

		private async Task<TRecord> DeleteInternalAsync(TRecord record, Func<IDbConnection, string, TRecord, Task<IEnumerable<TRecord>>> execute)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			var query = _queryGenerator.GenerateDeleteQuery<TRecord>();
			using var connection = CreateConnection();
			var result = await execute(connection, query, record);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result.FirstOrDefault(), query);
		}
		#endregion

		#region Get
		public TRecord Get(TRecord record)
		{
			return GetInternalAsync(record, (connection, query, input) => Task.FromResult(DapperInjection.Query.Invoke(connection, query, input)))
						.GetAwaiter()
						.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TRecord> GetAsync(TRecord record)
		{
			return await GetInternalAsync(record, (connection, query, input) => DapperInjection.QueryAsync.Invoke(connection, query, input));
		}

		private async Task<TRecord> GetInternalAsync(TRecord record, Func<IDbConnection, string, TRecord, Task<IEnumerable<TRecord>>> execute)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			var query = _queryGenerator.GenerateGetQuery<TRecord>();
			using var connection = CreateConnection();
			var result = await execute(connection, query, record);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result.FirstOrDefault(), query);
		}
		#endregion

		#region GetAll
		public IEnumerable<TRecord> GetAll()
		{
			return GetAllInternalAsync((connection, query) => Task.FromResult(DapperInjection.Query.Invoke(connection, query)))
						.GetAwaiter()
						.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<IEnumerable<TRecord>> GetAllAsync()
		{
			return await GetAllInternalAsync(async (connection, query) => await DapperInjection.QueryAsync.Invoke(connection, query));
		}

		private async Task<IEnumerable<TRecord>> GetAllInternalAsync(Func<IDbConnection, string, Task<IEnumerable<TRecord>>> execute)
		{
			var query = _queryGenerator.GenerateGetAllQuery<TRecord>();
			using var connection = CreateConnection();
			return await execute(connection, query);
		}
		#endregion

		#region Insert
		public TRecord Insert(TRecord record)
		{
			return InsertInternalAsync(record, (connection, query, input) => Task.FromResult(DapperInjection.QuerySingle.Invoke(connection, query, input)))
					.GetAwaiter()
					.GetResult();
		}

		public async Task<TRecord> InsertAsync(TRecord record)
		{
			return await InsertInternalAsync(record, async (connection, query, input) => await DapperInjection.QuerySingleAsync.Invoke(connection, query, input));
		}

		private async Task<TRecord> InsertInternalAsync(TRecord record, Func<IDbConnection, string, TRecord, Task<TRecord>> execute)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var query = _queryGenerator.GenerateInsertQuery(record);
			using var connection = CreateConnection();
			return await execute(connection, query, record);
		}

		#endregion
	}
}