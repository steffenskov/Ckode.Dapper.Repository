using SqlMapper = Dapper.SqlMapper;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class MyDapperInjection<TEntity> : IDapperInjection<TEntity>
	where TEntity : TableEntity
	{
		public static MyDapperInjection<TEntity> Instance { get; }

		static MyDapperInjection()
		{
			Instance = new MyDapperInjection<TEntity>();
		}

		private MyDapperInjection() // Singleton pattern
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