using System;
using System.Data;
using Ckode.Dapper.Repository.Sql;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public abstract class MyViewRepository<TEntity> : ViewRepository<TEntity>
	where TEntity : DapperEntity
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
