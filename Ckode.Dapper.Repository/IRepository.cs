using System.Collections.Generic;

namespace Ckode.Dapper.Repository
{
	internal interface IRepository<TRecord> where TRecord : BaseTableRecord
	{
		bool Insert(TRecord entry);
		bool Update(TRecord entry);
		bool Delete(TRecord entry);
		IEnumerable<TRecord> GetAll();
	}
}
