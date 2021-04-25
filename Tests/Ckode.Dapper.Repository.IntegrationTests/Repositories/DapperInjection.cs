using SqlMapper = Dapper.SqlMapper;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class DapperInjection<TEntity> : IDapperInjection<TEntity>
	where TEntity : TableEntity
	{
		public static DapperInjection<TEntity> Instance { get; }

		static DapperInjection()
		{
			Instance = new DapperInjection<TEntity>();
		}

		private DapperInjection() // Singleton pattern
		{
		}

		public QuerySingleDelegate<TEntity> QuerySingle => SqlMapper.QuerySingle<TEntity>;

		public QuerySingleDelegate<TEntity> QuerySingleOrDefault => SqlMapper.QuerySingleOrDefault<TEntity>;

		public QueryDelegate<TEntity> Query => SqlMapper.Query<TEntity>;

		public QuerySingleAsyncDelegate<TEntity> QuerySingleAsync => SqlMapper.QuerySingleAsync<TEntity>;

		public QuerySingleAsyncDelegate<TEntity> QuerySingleOrDefaultAsync => SqlMapper.QuerySingleOrDefaultAsync<TEntity>;

		public QueryAsyncDelegate<TEntity> QueryAsync => SqlMapper.QueryAsync<TEntity>;
	}
}