using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ckode.Dapper.Repository.Interfaces
{
	public interface IRepository<TPrimaryKeyEntity, TEntity>
		where TPrimaryKeyEntity : TableEntity
		where TEntity : TPrimaryKeyEntity
	{
		TEntity Delete(TPrimaryKeyEntity record);
		Task<TEntity> DeleteAsync(TPrimaryKeyEntity record);

		TEntity Get(TPrimaryKeyEntity record);
		Task<TEntity> GetAsync(TPrimaryKeyEntity record);

		IEnumerable<TEntity> GetAll();
		Task<IEnumerable<TEntity>> GetAllAsync();

		TEntity Insert(TEntity entity);
		Task<TEntity> InsertAsync(TEntity entity);

		TEntity Update(TEntity entity);
		Task<TEntity> UpdateAsync(TEntity entity);
	}
}
