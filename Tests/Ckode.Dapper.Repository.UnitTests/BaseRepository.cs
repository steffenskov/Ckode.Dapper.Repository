using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckode.Dapper.Repository.Tests
{
	public abstract class BaseRepository<TRecord> : DapperSQLRepository<TRecord>
		where TRecord: BaseTableRecord
	{
		protected override IDbConnection CreateConnection()
		{
			throw new NotImplementedException();
		}
	}
}
