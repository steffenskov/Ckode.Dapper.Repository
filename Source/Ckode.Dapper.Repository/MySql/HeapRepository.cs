using Ckode.Dapper.Repository.BaseRepositories;
using Ckode.Dapper.Repository.Interfaces;

namespace Ckode.Dapper.Repository.MySql
{
	/// <summary>
	/// Provides a repository for "heap" tables (tables without a primary key)
	/// </summary>
	public abstract class HeapRepository<TEntity> : BaseHeapRepository<TEntity>, IHeapRepository<TEntity>
	where TEntity : DbEntity
	{
		protected override IQueryGenerator<TEntity> CreateQueryGenerator()
		{
			return new MySqlQueryGenerator<TEntity>(TableName);
		}

		protected HeapRepository() : base()
		{
		}
	}
}