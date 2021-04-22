using System.Collections.Generic;
using System.Data;

namespace Ckode.Dapper.Repository
{
	public delegate T QuerySingleDelegate<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

	public delegate IEnumerable<T> QueryDelegate<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
}