using System;
using System.Collections.Generic;
using System.Data;

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

		protected abstract IDbConnection CreateConnection();

		// TODO: Make abstract methods for injecting dapper functionality

		public TRecord Delete(TRecord record)
		{
			var query = generator.GenerateDeleteQuery(record);
			using (var connection = CreateConnection())
			{
				// TODO: Execute query
			}
			return record;
		}

		public TRecord Get(TRecord record)
		{
			var query = generator.GenerateGetQuery(record);
			// Invoke dapper query on connection
			return record;
		}

		public IEnumerable<TRecord> GetAll()
		{
			// TODO: How do I get table name here?
			// Depend on service locator maybe? not the prettiest solution....

			throw new NotImplementedException();
		}

		public TRecord Insert(TRecord record)
		{
			var query = generator.GenerateInsertQuery(record);
			// Invoke dapper query on connection
			return record;
		}

		public TRecord Update(TRecord record)
		{
			var query = generator.GenerateUpdateQuery(record);
			// Invoke dapper query on connection
			return record;
		}
	}
}
