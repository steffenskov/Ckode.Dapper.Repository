using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ckode.Dapper.Repository.Interfaces;
using Ckode.Dapper.Repository.MetaInformation;

namespace Ckode.Dapper.Repository.Sql
{
	/// <summary>
	/// Provides a repository for "heap" tables (tables without a primary key)
	/// </summary>
	public abstract class HeapRepository<TEntity> : DbRepository<TEntity>, IHeapRepository<TEntity>
	where TEntity : TableEntity
	{

		private readonly QueryGenerator _queryGenerator;
		private readonly QueryResultChecker<TEntity, TEntity> _resultChecker;

		public HeapRepository()
		{
			_queryGenerator = new QueryGenerator(TableName, Schema);
			_resultChecker = new QueryResultChecker<TEntity, TEntity>();
		}


		#region Delete
		public TEntity Delete(TEntity entity)
		{
			return DeleteInternalAsync(entity, (query, input) => Task.FromResult(Query(query, input)))
						.GetAwaiter()
						.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TEntity> DeleteAsync(TEntity entity)
		{
			return await DeleteInternalAsync(entity, async (query, input) => await QueryAsync(query, input));
		}

		private async Task<TEntity> DeleteInternalAsync(TEntity entity, Func<string, TEntity, Task<IEnumerable<TEntity>>> execute)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var info = EntityInformationCache.GetEntityInformation<TEntity>();

			var query = _queryGenerator.GenerateDeleteQuery<TEntity>();
			var result = await execute(query, entity);

			return _resultChecker.ReturnOrThrowIfEntityIsNull(entity, info, result.FirstOrDefault(), query);
		}
		#endregion

		#region Get
		public TEntity Get(TEntity entity)
		{
			return GetInternalAsync(entity, (query, input) => Task.FromResult(Query(query, input)))
						.GetAwaiter()
						.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TEntity> GetAsync(TEntity entity)
		{
			return await GetInternalAsync(entity, async (query, input) => await QueryAsync(query, input));
		}

		private async Task<TEntity> GetInternalAsync(TEntity entity, Func<string, TEntity, Task<IEnumerable<TEntity>>> execute)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var info = EntityInformationCache.GetEntityInformation<TEntity>();

			var query = _queryGenerator.GenerateGetQuery<TEntity>();
			var result = await execute(query, entity);

			return _resultChecker.ReturnOrThrowIfEntityIsNull(entity, info, result.FirstOrDefault(), query);
		}
		#endregion

		#region GetAll
		public IEnumerable<TEntity> GetAll()
		{
			return GetAllInternalAsync((query) => Task.FromResult(Query(query)))
						.GetAwaiter()
						.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await GetAllInternalAsync(async (query) => await QueryAsync(query));
		}

		private async Task<IEnumerable<TEntity>> GetAllInternalAsync(Func<string, Task<IEnumerable<TEntity>>> execute)
		{
			var query = _queryGenerator.GenerateGetAllQuery<TEntity>();
			return await execute(query);
		}
		#endregion

		#region Insert
		public TEntity Insert(TEntity entity)
		{
			return InsertInternalAsync(entity, (query, input) => Task.FromResult(QuerySingle(query, input)))
					.GetAwaiter()
					.GetResult();
		}

		public async Task<TEntity> InsertAsync(TEntity entity)
		{
			return await InsertInternalAsync(entity, async (query, input) => await QuerySingleAsync(query, input));
		}

		private async Task<TEntity> InsertInternalAsync(TEntity entity, Func<string, TEntity, Task<TEntity>> execute)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var query = _queryGenerator.GenerateInsertQuery(entity);
			return await execute(query, entity);
		}

		#endregion
	}
}