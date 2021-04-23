using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ckode.Dapper.Repository.Interfaces
{
	public interface IHeapRepository<TRecord>
		where TRecord : TableRecord
	{
		TRecord Delete(TRecord record);
		Task<TRecord> DeleteAsync(TRecord record);

		TRecord Get(TRecord record);
		Task<TRecord> GetAsync(TRecord record);

		IEnumerable<TRecord> GetAll();
		Task<IEnumerable<TRecord>> GetAllAsync();

		TRecord Insert(TRecord record);
		Task<TRecord> InsertAsync(TRecord record);

	}
}
