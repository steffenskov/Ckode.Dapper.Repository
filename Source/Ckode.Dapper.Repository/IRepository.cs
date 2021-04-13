using System.Collections.Generic;

namespace Ckode.Dapper.Repository
{
	internal interface IRepository<TRecord> where TRecord : BaseTableRecord
	{
		TRecord Get(TRecord record);
		TRecord Insert(TRecord record);
		TRecord Update(TRecord record);
		TRecord Delete(TRecord record);
		IEnumerable<TRecord> GetAll();
	}
}
