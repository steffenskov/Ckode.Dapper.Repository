using SqlMapper = Dapper.SqlMapper;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class MyDapperInjection<TRecord> : IDapperInjection<TRecord>
	where TRecord : TableRecord
	{
		public static MyDapperInjection<TRecord> Instance { get; }

		static MyDapperInjection()
		{
			Instance = new MyDapperInjection<TRecord>();
		}

		private MyDapperInjection() // Singleton pattern
		{
		}

		public QuerySingleDelegate<TRecord> QuerySingle => SqlMapper.QuerySingle<TRecord>;

		public QuerySingleDelegate<TRecord> QuerySingleOrDefault => SqlMapper.QuerySingleOrDefault<TRecord>;

		public QueryDelegate<TRecord> Query => SqlMapper.Query<TRecord>;

		public QuerySingleAsyncDelegate<TRecord> QuerySingleAsync => SqlMapper.QuerySingleAsync<TRecord>;

		public QuerySingleAsyncDelegate<TRecord> QuerySingleOrDefaultAsync => SqlMapper.QuerySingleOrDefaultAsync<TRecord>;

		public QueryAsyncDelegate<TRecord> QueryAsync => SqlMapper.QueryAsync<TRecord>;
	}
}