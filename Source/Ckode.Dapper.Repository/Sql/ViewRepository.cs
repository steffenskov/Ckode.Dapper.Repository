using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ckode.Dapper.Repository.Interfaces;

namespace Ckode.Dapper.Repository.Sql
{
	public abstract class ViewRepository<TEntity> : DbRepository<TEntity>, IViewRepository<TEntity>
	where TEntity : DbEntity
	{
		protected abstract string ViewName { get; }

		protected string FormattedViewName => $"[{Schema}].[{ViewName}]";

		private readonly SqlQueryGenerator _queryGenerator;

		private readonly QueryResultChecker<TEntity, TEntity> _resultChecker;

		public ViewRepository()
		{
			_queryGenerator = new SqlQueryGenerator(ViewName, Schema);
			_resultChecker = new QueryResultChecker<TEntity, TEntity>();
		}

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
	}
}
