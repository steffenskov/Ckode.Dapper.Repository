using System.Data;
using Ckode.Dapper.Repository.Sql;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public abstract class MyHeapRepository<TEntity> : HeapRepository<TEntity>
		where TEntity : TableEntity
	{

		protected override IDapperInjection<TEntity> DapperInjection => MyDapperInjection<TEntity>.Instance;

		protected override IDbConnection CreateConnection()
		{
			return ConnectionFactory.CreateConnection();
		}
	}
}
