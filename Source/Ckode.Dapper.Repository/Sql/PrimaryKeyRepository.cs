using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

		protected abstract IDapperInjection<TRecord> DapperInjection { get; }

		#endregion

		#region Delete
		public TRecord Delete(TPrimaryKeyRecord record)
		{
			return DeleteInternalAsync(record, (pk, query) => Task.FromResult(ExecuteQuerySingleOrDefault(pk, query)))
					.GetAwaiter()
					.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TRecord> DeleteAsync(TPrimaryKeyRecord record)
		{
			return await DeleteInternalAsync(record, async (pk, query) => await ExecuteQuerySingleOrDefaultAsync(pk, query));
		}

		private async Task<TRecord> DeleteInternalAsync(TPrimaryKeyRecord record, Func<TPrimaryKeyRecord, string, Task<TRecord?>> execute)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			CheckForDefaultPrimaryKeys(info, record);

			var query = _queryGenerator.GenerateDeleteQuery<TRecord>();
			var result = await execute(record, query);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result, query);
		}
		#endregion

		#region Get
		public TRecord Get(TPrimaryKeyRecord record)
		{
			return GetInternal(record, (pk, query) => Task.FromResult(ExecuteQuerySingleOrDefault(pk, query)))
					.GetAwaiter()
					.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TRecord> GetAsync(TPrimaryKeyRecord record)
		{
			return await GetInternal(record, async (pk, query) => await ExecuteQuerySingleOrDefaultAsync(pk, query));
		}

		private async Task<TRecord> GetInternal(TPrimaryKeyRecord record, Func<TPrimaryKeyRecord, string, Task<TRecord?>> execute)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			CheckForDefaultPrimaryKeys(info, record);

			var query = _queryGenerator.GenerateGetQuery<TRecord>();
			var result = await execute(record, query);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result, query);
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
			return await execute(connection, query, record);
		}
		#endregion

		#region Update
		public TRecord Update(TRecord record)
		{
			return UpdateInternalAsync(record, (input, query) => Task.FromResult(ExecuteQuerySingleOrDefault(input, query)))
						.GetAwaiter()
						.GetResult();
		}

		public async Task<TRecord> UpdateAsync(TRecord record)
		{
			return await UpdateInternalAsync(record, async (input, query) => await ExecuteQuerySingleOrDefaultAsync(input, query));
		}

		private async Task<TRecord> UpdateInternalAsync(TRecord record, Func<TRecord, string, Task<TRecord?>> execute)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			var info = RecordInformationCache.GetRecordInformation<TRecord>();

			CheckForDefaultPrimaryKeys(info, record);

			var query = _queryGenerator.GenerateUpdateQuery<TRecord>();
			var result = await execute(record, query);

			return _resultChecker.ReturnOrThrowIfRecordIsNull(record, info, result, query);
		}
		#endregion

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
			return DapperInjection.QuerySingleOrDefault.Invoke(connection, query, record);
		}

		private async Task<TRecord?> ExecuteQuerySingleOrDefaultAsync(TPrimaryKeyRecord record, string query)
		{
			using var connection = CreateConnection();
			return await DapperInjection.QuerySingleOrDefaultAsync.Invoke(connection, query, record);
		}
	}
}
