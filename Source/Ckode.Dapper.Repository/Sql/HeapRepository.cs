using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Ckode.Dapper.Repository.Interfaces;
using Ckode.Dapper.Repository.MetaInformation;

namespace Ckode.Dapper.Repository.Sql
{
	public abstract class HeapRepository<TEntity> : IHeapRepository<TEntity>
	where TEntity : TableEntity
	{

		private readonly QueryGenerator _queryGenerator;
		private readonly QueryResultChecker<TEntity, TEntity> _resultChecker;

		public HeapRepository()
		{
			_queryGenerator = new QueryGenerator(TableName, Schema);
			_resultChecker = new QueryResultChecker<TEntity, TEntity>();
		}

		protected abstract string TableName { get; }

		protected string Schema => "dbo";

		protected string FormattedTableName => $"[{Schema}].[{TableName}]";

		protected abstract IDbConnection CreateConnection();

		#region Injection points for Dapper methods

		protected abstract IDapperInjection<TEntity> DapperInjection { get; }

		#endregion

		#region Delete
		public TEntity Delete(TEntity entity)
		{
			return DeleteInternalAsync(entity, (connection, query, input) => Task.FromResult(DapperInjection.Query.Invoke(connection, query, input)))
						.GetAwaiter()
						.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TEntity> DeleteAsync(TEntity entity)
		{
			return await DeleteInternalAsync(entity, (connection, query, input) => DapperInjection.QueryAsync.Invoke(connection, query, input));
		}

		private async Task<TEntity> DeleteInternalAsync(TEntity entity, Func<IDbConnection, string, TEntity, Task<IEnumerable<TEntity>>> execute)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var info = EntityInformationCache.GetEntityInformation<TEntity>();

			var query = _queryGenerator.GenerateDeleteQuery<TEntity>();
			using var connection = CreateConnection();
			var result = await execute(connection, query, entity);

			return _resultChecker.ReturnOrThrowIfEntityIsNull(entity, info, result.FirstOrDefault(), query);
		}
		#endregion

		#region Get
		public TEntity Get(TEntity entity)
		{
			return GetInternalAsync(entity, (connection, query, input) => Task.FromResult(DapperInjection.Query.Invoke(connection, query, input)))
						.GetAwaiter()
						.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TEntity> GetAsync(TEntity entity)
		{
			return await GetInternalAsync(entity, (connection, query, input) => DapperInjection.QueryAsync.Invoke(connection, query, input));
		}

		private async Task<TEntity> GetInternalAsync(TEntity entity, Func<IDbConnection, string, TEntity, Task<IEnumerable<TEntity>>> execute)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var info = EntityInformationCache.GetEntityInformation<TEntity>();

			var query = _queryGenerator.GenerateGetQuery<TEntity>();
			using var connection = CreateConnection();
			var result = await execute(connection, query, entity);

			return _resultChecker.ReturnOrThrowIfEntityIsNull(entity, info, result.FirstOrDefault(), query);
		}
		#endregion

		#region GetAll
		public IEnumerable<TEntity> GetAll()
		{
			return GetAllInternalAsync((connection, query) => Task.FromResult(DapperInjection.Query.Invoke(connection, query)))
						.GetAwaiter()
						.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await GetAllInternalAsync(async (connection, query) => await DapperInjection.QueryAsync.Invoke(connection, query));
		}

		private async Task<IEnumerable<TEntity>> GetAllInternalAsync(Func<IDbConnection, string, Task<IEnumerable<TEntity>>> execute)
		{
			var query = _queryGenerator.GenerateGetAllQuery<TEntity>();
			using var connection = CreateConnection();
			return await execute(connection, query);
		}
		#endregion

		#region Insert
		public TEntity Insert(TEntity entity)
		{
			return InsertInternalAsync(entity, (connection, query, input) => Task.FromResult(DapperInjection.QuerySingle.Invoke(connection, query, input)))
					.GetAwaiter()
					.GetResult();
		}

		public async Task<TEntity> InsertAsync(TEntity entity)
		{
			return await InsertInternalAsync(entity, async (connection, query, input) => await DapperInjection.QuerySingleAsync.Invoke(connection, query, input));
		}

		private async Task<TEntity> InsertInternalAsync(TEntity entity, Func<IDbConnection, string, TEntity, Task<TEntity>> execute)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var query = _queryGenerator.GenerateInsertQuery(entity);
			using var connection = CreateConnection();
			return await execute(connection, query, entity);
		}

		#endregion
	}
}