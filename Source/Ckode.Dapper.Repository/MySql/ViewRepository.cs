using Ckode.Dapper.Repository.BaseRepositories;
using Ckode.Dapper.Repository.Interfaces;

namespace Ckode.Dapper.Repository.MySql
{
	public abstract class ViewRepository<TEntity> : BaseViewRepository<TEntity>, IViewRepository<TEntity>
	where TEntity : DbEntity
	{
		protected override IQueryGenerator<TEntity> CreateQueryGenerator()
		{
			return new MySqlQueryGenerator<TEntity>(ViewName);
		}

		public ViewRepository()
		{
		}
	}
}
