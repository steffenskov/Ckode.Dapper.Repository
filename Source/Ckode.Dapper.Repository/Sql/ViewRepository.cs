using Ckode.Dapper.Repository.BaseRepositories;
using Ckode.Dapper.Repository.Interfaces;

namespace Ckode.Dapper.Repository.Sql
{
	public abstract class ViewRepository<TEntity> : BaseViewRepository<TEntity>, IViewRepository<TEntity>
	where TEntity : DbEntity
	{
		protected virtual string Schema { get; } = "dbo";

		protected override string FormattedViewName => $"[{Schema}].[{ViewName}]";

		protected override IQueryGenerator<TEntity> CreateQueryGenerator()
		{
			return new SqlQueryGenerator<TEntity>(Schema, ViewName);
		}

		public ViewRepository()
		{
		}
	}
}
