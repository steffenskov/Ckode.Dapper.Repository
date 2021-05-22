using System;

namespace Ckode.Dapper.Repository.Interfaces
{
	public interface IQueryGenerator
	{
		string GenerateDeleteQuery<TEntity>()
		where TEntity : DbEntity;

		string GenerateInsertQuery<TEntity>(TEntity entity)
	   	where TEntity : DbEntity;

		string GenerateGetAllQuery<TEntity>()
		where TEntity : DbEntity;

		string GenerateGetQuery<TEntity>()
		where TEntity : DbEntity;

		string GenerateUpdateQuery<TEntity>()
		where TEntity : DbEntity;
	}
}
