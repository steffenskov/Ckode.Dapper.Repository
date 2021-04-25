using System.Data;
using Ckode.Dapper.Repository.Sql;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public abstract class MyPrimaryKeyRepository<TPrimaryKeyEntity, TEntity> : PrimaryKeyRepository<TPrimaryKeyEntity, TEntity>
	where TPrimaryKeyEntity : TableEntity
	where TEntity : TPrimaryKeyEntity
	{
		protected override IDbConnection CreateConnection()
		{
			return ConnectionFactory.CreateConnection();
		}

		protected override IDapperInjection<T> CreateDapperInjection<T>()
		{
			return new DapperInjection<T>();
		}
	}
}
