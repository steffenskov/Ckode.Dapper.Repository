using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ckode.Dapper.Repository.Delegates;
using Ckode.Dapper.Repository.Exceptions;
using Ckode.Dapper.Repository.Interfaces;
using Ckode.Dapper.Repository.MetaInformation;

namespace Ckode.Dapper.Repository.Sql
{
	/// <summary>
	/// Provides a repository for "heap" tables (tables without a primary key)
	/// </summary>
	public abstract class HeapRepository<TEntity> : DbRepository<TEntity>, IHeapRepository<TEntity>
	where TEntity : DbEntity
	{
		protected abstract string TableName { get; }

		protected string FormattedTableName => $"[{Schema}].[{TableName}]";

		private readonly SqlQueryGenerator _queryGenerator;

		private readonly QueryResultChecker<TEntity, TEntity> _resultChecker;

		#region Events

		public event PreOperationDelegate<TEntity>? PreInsert;
		public event PostOperationDelegate<TEntity>? PostInsert;
		public event PreOperationDelegate<TEntity>? PreDelete;
		public event PostOperationDelegate<TEntity>? PostDelete;
		#endregion

		public HeapRepository()
		{
			_queryGenerator = new SqlQueryGenerator(TableName, Schema);
			_resultChecker = new QueryResultChecker<TEntity, TEntity>();
		}

		#region Delete
		public TEntity Delete(TEntity entity)
		{
			InvokePreOperation(PreDelete, entity);
			var result = DeleteInternalAsync(entity, (query, input) => Task.FromResult(Query(query, input)))
							.GetAwaiter()
							.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part

			InvokePostOperation(PostDelete, result);
			return result;
		}

		public async Task<TEntity> DeleteAsync(TEntity entity)
		{
			InvokePreOperation(PreDelete, entity);
			var result = await DeleteInternalAsync(entity, async (query, input) => await QueryAsync(query, input));
			InvokePostOperation(PostDelete, result);
			return result;
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
			InvokePreOperation(PreInsert, entity);
			var result = InsertInternalAsync(entity, (query, input) => Task.FromResult(QuerySingle(query, input)))
							.GetAwaiter()
							.GetResult();
			InvokePostOperation(PostInsert, result);
			return result;
		}

		public async Task<TEntity> InsertAsync(TEntity entity)
		{
			InvokePreOperation(PreInsert, entity);
			var result = await InsertInternalAsync(entity, async (query, input) => await QuerySingleAsync(query, input));
			InvokePostOperation(PostInsert, result);
			return result;
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

		#region Event invokation
		protected void InvokePreOperation(PreOperationDelegate<TEntity>? @delegate, TEntity entity)
		{
			var cancelArg = new CancelEventArgs();
			try
			{
				@delegate?.Invoke(entity, cancelArg);
			}
			catch { }
			if (cancelArg.Cancel)
			{
				throw new CanceledException("Cancelled by event");
			}
		}

		protected void InvokePostOperation(PostOperationDelegate<TEntity>? @delegate, TEntity entity)
		{
			try
			{
				@delegate?.Invoke(entity);
			}
			catch { }
		}
		#endregion
	}
}