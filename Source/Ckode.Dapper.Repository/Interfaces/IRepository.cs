using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ckode.Dapper.Repository.Interfaces
{
	public interface IRepository<TPrimaryKeyRecord, TRecord>
		where TPrimaryKeyRecord : TableRecord
		where TRecord : TPrimaryKeyRecord
	{
		TRecord Delete(TPrimaryKeyRecord record);
		Task<TRecord> DeleteAsync(TPrimaryKeyRecord record);

		TRecord Get(TPrimaryKeyRecord record);
		Task<TRecord> GetAsync(TPrimaryKeyRecord record);

		IEnumerable<TRecord> GetAll();
		Task<IEnumerable<TRecord>> GetAllAsync();

		TRecord Insert(TRecord record);
		Task<TRecord> InsertAsync(TRecord record);

		TRecord Update(TRecord record);
		Task<TRecord> UpdateAsync(TRecord record);
	}
}
