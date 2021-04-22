using System.Data;
using System.Data.SqlClient;
using Ckode.Dapper.Repository.Sql;
using SqlMapper = Dapper.SqlMapper;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public abstract class MyHeapRepository<TRecord> : HeapRepository<TRecord>
		where TRecord : TableRecord
	{

		protected override QuerySingleDelegate<TRecord> QuerySingle => SqlMapper.QuerySingle<TRecord>;
		protected override QueryDelegate<TRecord> Query => SqlMapper.Query<TRecord>;

		protected override IDbConnection CreateConnection()
		{
			return ConnectionFactory.CreateConnection();
		}
	}
}
