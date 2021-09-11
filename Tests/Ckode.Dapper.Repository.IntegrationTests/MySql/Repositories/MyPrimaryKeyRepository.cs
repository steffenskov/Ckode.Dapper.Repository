using System.Data;
using Ckode.Dapper.Repository.MySql;

namespace Ckode.Dapper.Repository.IntegrationTests.MySql.Repositories
{
	public abstract class MyPrimaryKeyRepository<TPrimaryKeyEntity, TEntity> : PrimaryKeyRepository<TPrimaryKeyEntity, TEntity>
	where TPrimaryKeyEntity : DbEntity
	where TEntity : TPrimaryKeyEntity
	{
		protected override IDbConnection CreateConnection()
		{
			return ConnectionFactory.CreateMySqlConnection();
		}

		protected override IDapperInjection<T> CreateDapperInjection<T>()
		{
			return new DapperInjection<T>();
		}
	}
}
