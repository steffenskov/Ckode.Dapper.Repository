using System.Data;
using Ckode.Dapper.Repository.Sql;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public abstract class MyHeapRepository<TRecord> : HeapRepository<TRecord>
		where TRecord : TableRecord
	{

		protected override IDapperInjection<TRecord> DapperInjection => MyDapperInjection<TRecord>.Instance;

		protected override IDbConnection CreateConnection()
		{
			return ConnectionFactory.CreateConnection();
		}
	}
}
