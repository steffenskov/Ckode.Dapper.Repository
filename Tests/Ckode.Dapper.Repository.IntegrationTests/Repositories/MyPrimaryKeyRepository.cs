using System.Data;
using Ckode.Dapper.Repository.Sql;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public abstract class MyPrimaryKeyRepository<TPrimaryKeyRecord, TRecord> : PrimaryKeyRepository<TPrimaryKeyRecord, TRecord>
		where TPrimaryKeyRecord : TableRecord
		where TRecord : TPrimaryKeyRecord
	{
		protected override IDapperInjection<TRecord> DapperInjection => MyDapperInjection<TRecord>.Instance;

		protected override IDbConnection CreateConnection()
		{
			return ConnectionFactory.CreateConnection();
		}
	}
}
