using System.Collections.Generic;

namespace Ckode.Dapper.Repository.Interfaces
{
	public interface IHeapRepository<TRecord>
		where TRecord : TableRecord
	{
		TRecord Get(TRecord record);
		TRecord Insert(TRecord record);
		TRecord Delete(TRecord record);
		IEnumerable<TRecord> GetAll();
	}
}
