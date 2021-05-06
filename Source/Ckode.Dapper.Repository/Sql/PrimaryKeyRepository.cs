using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Ckode.Dapper.Repository.Interfaces;
using Ckode.Dapper.Repository.MetaInformation;

namespace Ckode.Dapper.Repository.Sql
{
	/// <summary>
	/// Provides a repository for tables with a primary key defined (either single column or composite)
	/// </summary>
	public abstract class PrimaryKeyRepository<TPrimaryKeyEntity, TEntity> : DbRepository<TEntity>, IRepository<TPrimaryKeyEntity, TEntity>
	where TPrimaryKeyEntity : DbEntity
	where TEntity : TPrimaryKeyEntity
	{
		protected abstract string TableName { get; }

		protected string FormattedTableName => $"[{Schema}].[{TableName}]";

		private readonly QueryGenerator _queryGenerator;

		private readonly QueryResultChecker<TPrimaryKeyEntity, TEntity> _resultChecker;

		public PrimaryKeyRepository()
		{
			_queryGenerator = new QueryGenerator(TableName, Schema);
			_resultChecker = new QueryResultChecker<TPrimaryKeyEntity, TEntity>();
		}

		#region Delete
		public TEntity Delete(TPrimaryKeyEntity entity)
		{
			return DeleteInternalAsync(entity, (pk, query) => Task.FromResult(QuerySingleOrDefault(query, pk)))
					.GetAwaiter()
					.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TEntity> DeleteAsync(TPrimaryKeyEntity entity)
		{
			return await DeleteInternalAsync(entity, async (pk, query) => await QuerySingleOrDefaultAsync(query, pk));
		}

		private async Task<TEntity> DeleteInternalAsync(TPrimaryKeyEntity entity, Func<TPrimaryKeyEntity, string, Task<TEntity?>> execute)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var info = EntityInformationCache.GetEntityInformation<TEntity>();

			CheckForDefaultPrimaryKeys(info, entity);

			var query = _queryGenerator.GenerateDeleteQuery<TEntity>();
			var result = await execute(entity, query);

			return _resultChecker.ReturnOrThrowIfEntityIsNull(entity, info, result, query);
		}
		#endregion

		#region Get
		public TEntity Get(TPrimaryKeyEntity entity)
		{
			return GetInternal(entity, (pk, query) => Task.FromResult(QuerySingleOrDefault(query, pk)))
					.GetAwaiter()
					.GetResult(); // This is safe because we're using Task.FromResult as the only "async" part
		}

		public async Task<TEntity> GetAsync(TPrimaryKeyEntity entity)
		{
			return await GetInternal(entity, async (pk, query) => await QuerySingleOrDefaultAsync(query, pk));
		}

		private async Task<TEntity> GetInternal(TPrimaryKeyEntity entity, Func<TPrimaryKeyEntity, string, Task<TEntity?>> execute)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var info = EntityInformationCache.GetEntityInformation<TEntity>();

			CheckForDefaultPrimaryKeys(info, entity);

			var query = _queryGenerator.GenerateGetQuery<TEntity>();
			var result = await execute(entity, query);

			return _resultChecker.ReturnOrThrowIfEntityIsNull(entity, info, result, query);
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

			var info = EntityInformationCache.GetEntityInformation<TEntity>();

			var invalidIdentityColumns = info.PrimaryKeys
												.Where(pk => pk.IsIdentity && !pk.HasDefaultValue(entity))
												.ToList();

			if (invalidIdentityColumns.Any())
			{
				throw new ArgumentException($"entity has the following primary keys marked with IsIdentity, which have non-default values: {string.Join(", ", invalidIdentityColumns.Select(col => col.Name))}", nameof(entity));
			}

			var query = _queryGenerator.GenerateInsertQuery(entity);
			return await execute(query, entity);
		}
		#endregion

		#region Update
		public TEntity Update(TEntity entity)
		{
			return UpdateInternalAsync(entity, (input, query) => Task.FromResult(QuerySingleOrDefault(query, input)))
						.GetAwaiter()
						.GetResult();
		}

		public async Task<TEntity> UpdateAsync(TEntity entity)
		{
			return await UpdateInternalAsync(entity, async (input, query) => await QuerySingleOrDefaultAsync(query, input));
		}

		private async Task<TEntity> UpdateInternalAsync(TEntity entity, Func<TEntity, string, Task<TEntity?>> execute)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var info = EntityInformationCache.GetEntityInformation<TEntity>();

			CheckForDefaultPrimaryKeys(info, entity);

			var query = _queryGenerator.GenerateUpdateQuery<TEntity>();
			var result = await execute(entity, query);

			return _resultChecker.ReturnOrThrowIfEntityIsNull(entity, info, result, query);
		}
		#endregion

		private static void CheckForDefaultPrimaryKeys(EntityInformation info, TPrimaryKeyEntity entity)
		{
			var invalidPrimaryKeys = info.PrimaryKeys
											   .Where(pk => pk.HasDefaultValue(entity))
											   .ToList();

			if (invalidPrimaryKeys.Any())
			{
				throw new ArgumentException($"entity has the following primary keys which have default values: {string.Join(", ", invalidPrimaryKeys.Select(col => col.Name))}", nameof(entity));
			}
		}
	}
}
