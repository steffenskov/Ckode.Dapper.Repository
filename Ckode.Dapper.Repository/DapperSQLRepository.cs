using System;
using System.Collections.Generic;

namespace Ckode.Dapper.Repository
{
	public abstract class DapperSQLRepository<TRecord> : IRepository<TRecord>
		where TRecord : BaseTableRecord
	{
		private readonly SQLGenerator generator;
		public DapperSQLRepository()
		{
			generator = new SQLGenerator();
		}

		protected abstract string TableName { get; }

		public bool Delete(TRecord record)
		{
			var query = generator.GenerateDeleteQuery(record);
			// Invoke dapper query on connection
			return true;
		}

		public TRecord Get(TRecord record)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TRecord> GetAll()
		{
			throw new NotImplementedException();
		}

		public bool Insert(TRecord entry)
		{
			throw new NotImplementedException();
		}

		public bool Update(TRecord entry)
		{
			throw new NotImplementedException();
		}
	}
}
